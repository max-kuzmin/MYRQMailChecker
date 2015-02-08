using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing;



namespace Mail_Checker
{
    public partial class Form1main : Form
    {

        Checker checker;
        string[] mails, proxys;
        SetStateDelegate d;
        List<StreamWriter> goodMailsWithMessagesOut;
        StreamWriter goodMailsOut;
        bool started = false;
        StreamWriter outMailsLasts;


        public Form1main()
        {
            InitializeComponent();

            d += SetState;

            //new MailBrowser(new string[] { "roman_dolzhenov@mail.ru", "oleg2510" }).Show();
            //new MailBrowser(new string[] { "zhe-st@yandex.ru", "nastya06" }).Show();
            //new MailBrowser(new string[] { "zemlyak-66@rambler.ru", "091966" }).Show();
            //new MailBrowser(new string[] { "yura426@qip.ru", "bar123sik" }).Show();

        }

        private void button1mails_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            if (dialog.FileName == "") return;

            mails = File.ReadAllLines(dialog.FileName);

            if (checkBox2doubles.Checked) mails = DoubleRemover(mails);

            label10mails.Text = mails.Length.ToString();



        }

        private void button2proxys_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName == "") return;

            if (!started)
            {
                proxys = File.ReadAllLines(dialog.FileName);
                if (checkBox2doubles.Checked) proxys = DoubleRemover(proxys);

                label11proxys.Text = proxys.Length.ToString();
            }
            else
            {
                string[] newProxys = File.ReadAllLines(dialog.FileName);
                if (checkBox2doubles.Checked) newProxys = DoubleRemover(newProxys);
                checker.AddProxys(newProxys);
            }
        }


        string outNameTime;


        private void button3start_Click(object sender, EventArgs e)
        {
            
            if (button3start.Text == "Старт")
            {

                if (mails == null || proxys == null) return;


                try
                {
                    Convert.ToInt32(textBox1threads.Text);
                    Convert.ToInt32(textBox2timeout.Text);
                }
                catch
                {
                    return;
                }
                


                List<string> querys = new List<string>(textBox1query.Text.Split('\n'));
                for (int i = 0; i < querys.Count; i++)
                {
                    querys[i] = querys[i].Replace("\r", "");
                    if (querys[i] == "")
                    {
                        querys.Remove(querys[i]);
                        i--;
                    }
                }

                listView1.Items.Clear();
                listView1.Columns.Clear();

                if (querys.Count > 0)
                {


                    
                    this.MinimumSize = new Size(270 + (querys.Count) * 75 + 150 * 2, this.Height);
                    this.MaximumSize = new Size(0, 0);
                    this.Width = 270 + (querys.Count) * 75 + 150 * 2;
                    listView1.Width = (querys.Count) * 75 + 150 * 2 + 15;
                    listView1.Columns.Add("Login", "Логин", 150);
                    listView1.Columns.Add("Pass", "Пароль", 150);
                    for (int i = 0; i < querys.Count; i++)
                    {
                        listView1.Columns.Add(querys[i], querys[i], 75);
                    }
                }
                else
                {
                    
                    this.MinimumSize = new Size(240, this.Height);
                    this.MaximumSize = new Size(240, this.Height);
                    this.Width = 250;
                    listView1.Width = 0;
                }
                

                Directory.CreateDirectory("results");

                outNameTime = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString().Replace(':', '.');

                try
                {
                    goodMailsOut = File.CreateText("results\\" + outNameTime + " - good.txt");

                    goodMailsWithMessagesOut = new List<StreamWriter>();

                    for (int i = 0; i < querys.Count; i++)
                    {
                        goodMailsWithMessagesOut.Add(File.CreateText("results\\" + outNameTime + " - " + querys[i] + ".txt"));
                    }
                }
                catch (IOException)
                {
                    return;
                }

                checker = new Checker(mails, proxys, querys.ToArray(), Convert.ToInt32(textBox1threads.Text), Convert.ToInt32(textBox2timeout.Text), checkBox1proxyCheck.Checked);

                checker.OneCheckDone += ChangeLabels;

                checker.Start();
                started = true;

                button3start.BackColor = Color.LightSalmon;
                button3start.Text = "Стоп";
                textBox1threads.Enabled = false;
                textBox2timeout.Enabled = false;
                button1mails.Enabled = false;
                //button2proxys.Enabled = false;
                textBox1query.Enabled = false;
                checkBox1proxyCheck.Enabled = false;
                checkBox2doubles.Enabled = false;
            }
            else
            {
                checker.Stop();
                started = false;

                button3start.BackColor = Color.LightGreen;
                button3start.Text = "Старт";
                textBox1threads.Enabled = true;
                textBox2timeout.Enabled = true;
                button1mails.Enabled = true;
                //button2proxys.Enabled = true;
                textBox1query.Enabled = true;
                checkBox1proxyCheck.Enabled = true;
                checkBox2doubles.Enabled = true;

                outMailsLasts = File.CreateText("results\\" + outNameTime + " - " + "not_checked_mails" + ".txt");
                string[] lastsMails = checker.mails.ToArray();
                for (int i = 0; i < lastsMails.Length; i++)
                {
                    outMailsLasts.Write(lastsMails[i]+"\n");
                }
                outMailsLasts.Flush();
                outMailsLasts.AutoFlush = true;
                //outMailsLasts.Close();
            }
        }


        delegate void SetStateDelegate(CheckEventArgs e);

        void SetState (CheckEventArgs e) {

            label10mails.Text = e.state.mails.ToString();
            label11proxys.Text = e.state.proxys.ToString();
            label7threads.Text = e.state.threads.ToString();
            label12errors.Text = e.state.errors.ToString();
            label8valid.Text = e.state.valid.ToString();
            label9novalid.Text = e.state.novalid.ToString();

            if (e.error == CheckErrors.noError)
            {
                goodMailsOut.WriteLine(e.mail.login + ":" + e.mail.pass);
                goodMailsOut.Flush();

                bool notEmpty = false;


                for (int i = 0; i < e.mail.messages.Length; i++)
                {
                    if (e.mail.messages[i] > 0)
                    {
                        notEmpty = true;
                        break;
                    }
                }

                if (notEmpty)
                {

                    ListViewItem item = new ListViewItem(e.mail.login);
                    item.SubItems.Add(e.mail.pass);


                    for (int i = 0; i < e.mail.messages.Length; i++)
                    {
                        
                        if (e.mail.messages[i] > 0)
                        {
                            item.SubItems.Add(e.mail.messages[i].ToString());
                            string toWrite = e.mail.login + ":" + e.mail.pass + "                                                                                                  ";
                            toWrite = toWrite.Remove(50);
                            toWrite += " - " + e.mail.messages[i];
                            goodMailsWithMessagesOut[i].WriteLine(toWrite);
                            goodMailsWithMessagesOut[i].Flush();
                        }
                        else
                        {
                            item.SubItems.Add(" ");
                        }
                    }

                    listView1.Items.Add(item);


                }
            }

            if (!started && e.error != CheckErrors.noError)
            {
                outMailsLasts.Write(e.mail.login + ":" + e.mail.pass + "\n");
            }

        }

        private void ChangeLabels(object sender, CheckEventArgs e)
        {
            try
            {
                this.Invoke(d, new object[] { e });
            }
            catch 
            {
                Application.Exit();
            }
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string[] mailElements = { listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].SubItems[1].Text };

            MailBrowser b = new MailBrowser(mailElements);
            b.Show();
            this.Enabled = false;
            b.FormClosed += b_FormClosed;
        }

        void b_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        private void toolStripMenuItem1login_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text);
        }

        private void toolStripMenuItem2pass_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(listView1 ,e.Location.X, e.Location.Y);
            }
        }



        string[] DoubleRemover(string[] input)
        {
            List<string> inpList = new List<string>(input);
            List<string> outpList = new List<string>();

            inpList.Sort();
            outpList.Add(inpList[0]);

            for (int i = 1; i < inpList.Count; i++)
            {
                if (inpList[i] != outpList[outpList.Count - 1]) outpList.Add(inpList[i]);
            }

            return outpList.ToArray();
        }


    }


}
