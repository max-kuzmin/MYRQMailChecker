using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet.Net;

namespace Mail_Checker
{
    class ProxyChecker
    {
        ConcurrentQueue<string> proxys;
        StreamWriter writer;

        public event EventHandler CheckComplete;
        int errors = 0;
        int goods = 0;
        int threads = 0, threadsMax=300, timeout = 10000;
        bool stopped = false;

        public ProxyChecker(List<string> prox, int threads, int timeout)
        {
            proxys = new ConcurrentQueue<string>(prox);
            this.threadsMax = threads;
            this.timeout = timeout;
        }


        public int ProxyLast
        {
            get { return proxys.Count; }
        }

        public void Stop()
        {
            stopped = true;
            writer.Close();
            writer.Dispose();
        }





        public  void CheckAsync()
        {
            writer = File.CreateText((DateTime.Now.ToShortDateString() + '-' + DateTime.Now.ToShortTimeString()).Replace('.', '-').Replace(':', '-') + "-good-proxies.txt");
            errors = 0;
            goods = 0;
            threads = 0;
            for (int i = 0; i < threadsMax; i++)
            {
                new Thread(new ThreadStart(Check)).Start();
            }
        }

        Random r = new Random();
        private void Check()
        {
            Thread.Sleep(r.Next(10, threadsMax*30));
            threads++;
            while (proxys.Count>0 && !stopped)
            {

                string oneProxy = "";
                if (!proxys.TryDequeue(out oneProxy))
                {
                    Thread.Sleep(r.Next(10, threadsMax));
                    continue;
                }

                using (HttpRequest req = new HttpRequest())
                {
                    req.ConnectTimeout = timeout;
                    req.ReadWriteTimeout = timeout;
                    try
                    {
                        req.Proxy = ProxyClient.Parse(ProxyType.Http, oneProxy);
                    }
                    catch 
                    { 
                        continue; 
                    }
                    
                    HttpResponse res = null;
                    try
                    {
                        res = req.Get("http://www.yandex.ru/m/");
                        if (res != null && res.ToString().Contains("Яндекс"))
                        {
                            res = req.Get("https://mail.ru/?from=m");
                            if (res != null && res.ToString().Contains("Mail.Ru"))
                            {
                                goods++;
                                writer.WriteAsync(oneProxy + "\r\n");
                            }
                            else errors++;
                        }
                        else errors++;
                    }
                    catch
                    {
                        errors++;
                    }
                }
                 if (CheckComplete!=null) CheckComplete(this, new EventArgs());

            }

            threads--;
        }

        public int Errors
        {
            get
            {
                return errors;
            }
        }

        public int Goods
        {
            get
            {
                return goods;
            }

        }

        public int Threads
        {
            get
            {
                return threads;
            }

        }
    }
}
