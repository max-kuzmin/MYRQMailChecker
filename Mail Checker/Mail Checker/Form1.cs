using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Limilabs.Client.IMAP;
using Limilabs.Proxy;
using System.Threading;
using System.Net.Sockets;
using Limilabs.Mail;
using Limilabs.Proxy.Exceptions;
using Limilabs.Client;
using System.Security.Authentication;



namespace Mail_Checker
{
    public partial class Form1main : Form
    {



        ConcurrentQueue<string> mails, proxys;
        string serv = "imap.mail.ru";
        ProxyFactory proxyMaker;
        MailBuilder builder;
        int workingThreads = 0;


        public Form1main()
        {
            InitializeComponent();

            proxyMaker = new ProxyFactory();
            builder = new MailBuilder();


        }

        private void button1mails_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            mails = new ConcurrentQueue<string>(File.ReadAllLines(dialog.FileName));

            label10mails.Text = mails.Count.ToString();



        }

        private void button2proxys_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            proxys = new ConcurrentQueue<string>(File.ReadAllLines(dialog.FileName));


            label11proxys.Text = proxys.Count.ToString();
        }

        private void button3start_Click(object sender, EventArgs e)
        {
            if (button3start.Text == "Старт")
            {
                int threads = Convert.ToInt32(textBox1threads.Text);

                ThreadPool.SetMaxThreads(threads * 2, threads * 20);
                ThreadPool.SetMinThreads(threads, threads * 10);



                for (int i = 0; i < threads; i++)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
                }
                button3start.Text = "Стоп";
                textBox1threads.Enabled = false;
                textBox2timeout.Enabled = false;
                button1mails.Enabled = false;
                button2proxys.Enabled = false;
                textBox1query.Enabled = false;
            }
            else
            {
                button3start.Text = "Старт";
                textBox1threads.Enabled = true;
                textBox2timeout.Enabled = true;
                button1mails.Enabled = true;
                button2proxys.Enabled = true;
                textBox1query.Enabled = true;
            }
        }

        private void ChangeLabels(object sender, CheckEventArgs e)
        {
           /////// //label10mails.Text = (Convert.ToInt32(label10mails.Text) + 1).ToString();
            //label10mails.Text = mails.Count.ToString();
            //label11proxys.Text = proxys.Count.ToString();
            //label7threads.Text = workingThreads.ToString();
        }



        private void Check(object e)
        {


            EventHandler<CheckEventArgs> CheckDone = new EventHandler<CheckEventArgs>(ChangeLabels);


            workingThreads++;

            int messNum = 0;


            string proxyLine;
            if (proxys.TryDequeue(out proxyLine) == false)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
                return;
            }
            char[] separs = { ':', ';' };
            string[] proxyElements = proxyLine.Split(separs);


            string mailLine;
            if (mails.TryDequeue(out mailLine) == false)
            {
                proxys.Enqueue(proxyLine);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
                return;
            }
            string[] mailElements = mailLine.Split(separs);



            try
            {

                IProxyClient proxy = proxyMaker.CreateProxy(ProxyType.Http, proxyElements[0], Convert.ToInt32(proxyElements[1]));
                proxy.ReceiveTimeout = TimeSpan.FromSeconds(Convert.ToInt32(textBox2timeout.Text));
                Socket sock = proxy.Connect(serv, Imap.DefaultSSLPort);

                Imap connection = new Imap();
                connection.ReceiveTimeout = TimeSpan.FromSeconds(Convert.ToInt32(textBox2timeout.Text));
                connection.AttachSSL(sock, serv);

                connection.Login(mailElements[0], mailElements[1]);

                connection.SelectInbox();


                List<long> UIDs = connection.GetAll();
                List<MessageData> messages = connection.PeekSpecificHeadersByUID(UIDs, new string[] { "From" });


                for (int i = 0; i < messages.Count; i++)
                {
                    IMail email = new MailBuilder().CreateFromEml(messages[i].EmlData);
                    if (email.Sender.Address.Contains(textBox1query.Text))
                    {
                        messNum++;
                    }
                }


                connection.Close();

                proxys.Enqueue(proxyLine);


                CheckDone(this, new CheckEventArgs(CheckErrors.noError, mailElements[0], mailElements[1], messNum));


            }
            catch (ProxyException)
            {
                mails.Enqueue(mailLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.proxyError));
            }
            catch (SocketException)
            {
                mails.Enqueue(mailLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.proxyError));
            }
            catch (IOException)
            {
                mails.Enqueue(mailLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.proxyError));
            }
            catch (ImapResponseException)
            {
                proxys.Enqueue(proxyLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.mailError));
            }
            catch (ServerException)
            {
                mails.Enqueue(mailLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.proxyError));
            }
            catch (AuthenticationException)
            {
                mails.Enqueue(mailLine);
                CheckDone(this, new CheckEventArgs(CheckErrors.proxyError));
            }

            workingThreads--;

            if (mails.Count > 0 && proxys.Count > 0 && button3start.Text != "Старт")
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
            }

        }



    }


}
