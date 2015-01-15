﻿using System;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.IO;



namespace Mail_Checker
{
    public partial class Form1main : Form
    {

        Checker checker;
        string[] mails, proxys;
        SetStateDelegate d;
        StreamWriter goodMailsWithMessagesOut, goodMailsOut;

        public Form1main()
        {
            InitializeComponent();

            d += SetState;


        }

        private void button1mails_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            mails = File.ReadAllLines(dialog.FileName);

            label10mails.Text = mails.Length.ToString();



        }

        private void button2proxys_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            proxys = File.ReadAllLines(dialog.FileName);


            label11proxys.Text = proxys.Length.ToString();
        }




        private void button3start_Click(object sender, EventArgs e)
        {
            if (button3start.Text == "Старт")
            {

                string outNameTime = DateTime.Now.ToShortDateString() + " - "+ DateTime.Now.ToShortTimeString().Replace(':', '.');
                goodMailsWithMessagesOut = File.CreateText(outNameTime + " - good.txt");
                goodMailsOut = File.CreateText(outNameTime + " - " + textBox1query.Text + ".txt");

                checker = new Checker(mails, proxys, textBox1query.Text, Convert.ToInt32(textBox1threads.Text), Convert.ToInt32(textBox2timeout.Text));

                checker.OneCheckDone += ChangeLabels;

                checker.Start();


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

                button3start.Text = "Старт";
                textBox1threads.Enabled = true;
                textBox2timeout.Enabled = true;
                button1mails.Enabled = true;
                button2proxys.Enabled = true;
                textBox1query.Enabled = true;
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


                if (e.mail.messages > 0)
                {
                    string[] sub = { e.mail.login, e.mail.pass, e.mail.messages.ToString() };
                    listView1.Items.Add(new ListViewItem(sub));

                    goodMailsWithMessagesOut.WriteLine(e.mail.login + ":" + e.mail.pass);
                    goodMailsWithMessagesOut.Flush();

                }
            }

        }

        private void ChangeLabels(object sender, CheckEventArgs e)
        {

            this.Invoke(d, new object[] { e });
            
        }




    }


}