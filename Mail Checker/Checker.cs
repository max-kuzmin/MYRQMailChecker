using Limilabs.Client;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Proxy;
using Limilabs.Proxy.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;

namespace Mail_Checker
{
    class Checker
    {

        public EventHandler<CheckEventArgs> OneCheckDone;

        char[] separs = { ':', ';' };
        ConcurrentQueue<string> mails, proxys;
        string serv = "imap.mail.ru";
        string query;
        ProxyFactory proxyMaker;
        MailBuilder builder;
        int workingThreads = 0, errors = 0, valid = 0, novalid = 0;
        int threads, timeout;
        bool started = false;

        public Checker(string[] mails, string[] proxys, string query, int threads, int timeout)
        {
            this.mails = new ConcurrentQueue<string>(mails);
            this.proxys = new ConcurrentQueue<string>(proxys);
            this.query = query;
            this.threads = threads;
            this.timeout = timeout;

            proxyMaker = new ProxyFactory();
            builder = new MailBuilder();
        }


        public void Start()
        {
            started = true;


            ThreadPool.SetMaxThreads(threads * 2, threads * 20);
            ThreadPool.SetMinThreads(threads, threads * 10);


            for (int i = 0; i < threads; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
            }
        }

        public void Stop()
        {
            started = false;
        }

        void Check(object e)
        {


            int messNum = 0;


            string proxyLine;
            if (proxys.TryDequeue(out proxyLine) == false)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
                return;
            }
            string[] proxyElements = proxyLine.Split(separs);


            string mailLine;
            if (mails.TryDequeue(out mailLine) == false)
            {
                proxys.Enqueue(proxyLine);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
                return;
            }
            string[] mailElements = mailLine.Split(separs);

            workingThreads++;

            IProxyClient proxy = null;
            Socket sock = null;
            Imap connection = null;

            try
            {

                proxy = proxyMaker.CreateProxy(ProxyType.Http, proxyElements[0], Convert.ToInt32(proxyElements[1]));
                proxy.ReceiveTimeout = TimeSpan.FromSeconds(timeout);
                sock = proxy.Connect(serv, Imap.DefaultSSLPort);

                connection = new Imap();
                connection.ReceiveTimeout = TimeSpan.FromSeconds(timeout);
                connection.AttachSSL(sock, serv);

                connection.Login(mailElements[0], mailElements[1]);

                connection.SelectInbox();


                List<long> UIDs = connection.GetAll();
                List<MessageData> messages = connection.PeekSpecificHeadersByUID(UIDs, new string[] { "From" });


                for (int i = 0; i < messages.Count; i++)
                {
                    IMail email = builder.CreateFromEml(messages[i].EmlData);
                    if (email.Sender != null && email.Sender.Address != null && email.Sender.Address.Contains(query))
                    {
                        messNum++;
                    }
                    
                }


                proxys.Enqueue(proxyLine);

                valid++;
                MailInfo mInfo = new MailInfo(mailElements[0], mailElements[1], messNum);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.noError, mInfo, chState));


            }
            catch (ProxyException)
            {
                errors++;
                mails.Enqueue(mailLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.proxyError, new MailInfo(), chState));
            }
            catch (SocketException)
            {
                errors++;
                mails.Enqueue(mailLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.proxyError, new MailInfo(), chState));
            }
            catch (IOException)
            {
                errors++;
                mails.Enqueue(mailLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.proxyError, new MailInfo(), chState));
            }
            catch (ImapResponseException)
            {
                novalid++;
                proxys.Enqueue(proxyLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.mailError, new MailInfo(), chState));
            }
            catch (ServerException)
            {
                errors++;
                mails.Enqueue(mailLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.proxyError, new MailInfo(), chState));
            }
            catch (AuthenticationException)
            {
                errors++;
                mails.Enqueue(mailLine);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(CheckErrors.proxyError, new MailInfo(), chState));
            }

            if (connection != null)
            {
                //connection.Close();
                connection.Dispose();
            }
            if (sock != null)
            {
                //sock.Close();
                sock.Dispose();
            }
            if (proxy != null) proxy.Dispose();

            workingThreads--;

            if (mails.Count > 0 && proxys.Count > 0 && started)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
            }

        }


    }
}
