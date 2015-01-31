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

        public Form1main()
        {
            InitializeComponent();

            d += SetState;

            //new MailBrowser(new string[] { "zin_1995@bk.ru", "1q2w3e4r5t" }).Show();
            //new MailBrowser(new string[] { "zvukopedia@yandex.ru", "sqrt225" }).Show();
            //new MailBrowser(new string[] { "kosh-udar-tru@rambler.ru", "krovosos1996" }).Show();
            //new MailBrowser(new string[] { "yura426@qip.ru", "bar123sik" }).Show();

        }

        private void button1mails_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            if (dialog.FileName == "") return;

            mails = File.ReadAllLines(dialog.FileName);

            label10mails.Text = mails.Length.ToString();



        }

        private void button2proxys_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName == "") return;

            proxys = File.ReadAllLines(dialog.FileName);


            label11proxys.Text = proxys.Length.ToString();
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

                this.Width = 270 + (querys.Count) * 75+150*2;
                this.MinimumSize = new Size(this.Width, this.Height);
                listView1.Width = (querys.Count) * 75 + 150 * 2 + 15;
                listView1.Columns.Add("Login", "Логин", 150);
                listView1.Columns.Add("Pass", "Пароль", 150);
                for (int i = 0; i < querys.Count; i++)
                {
                    listView1.Columns.Add(querys[i], querys[i], 75);
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

                checker = new Checker(mails, proxys, querys.ToArray(), Convert.ToInt32(textBox1threads.Text), Convert.ToInt32(textBox2timeout.Text));

                checker.OneCheckDone += ChangeLabels;

                checker.Start();

                button3start.BackColor = Color.LightSalmon;
                button3start.Text = "Стоп";
                textBox1threads.Enabled = false;
                textBox2timeout.Enabled = false;
                button1mails.Enabled = false;
                button2proxys.Enabled = false;
                textBox1query.Enabled = false;
            }
            else
            {
                checker.Stop();

                button3start.BackColor = Color.LightGreen;
                button3start.Text = "Старт";
                textBox1threads.Enabled = true;
                textBox2timeout.Enabled = true;
                button1mails.Enabled = true;
                button2proxys.Enabled = true;
                textBox1query.Enabled = true;

                StreamWriter outMailsLasts = File.CreateText("results\\" + outNameTime + " - " + "not_checked_mails" + ".txt");
                string[] lastsMails = checker.mails.ToArray();
                for (int i = 0; i < lastsMails.Length; i++)
                {
                    outMailsLasts.Write(lastsMails[i]+"\n");
                }
                outMailsLasts.Flush();
                outMailsLasts.Close();
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






    }


}
