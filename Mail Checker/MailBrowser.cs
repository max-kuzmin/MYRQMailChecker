using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chilkat;
using System.Net;


namespace Mail_Checker
{
    public partial class MailBrowser : Form
    {
        string[] mailElements;
        string [] domainLogin;
        string cookies = "";

        WebProxy myProxy = new WebProxy("109.175.8.45:8080");
            

        public MailBrowser(string[] mailElements)
        {
            InitializeComponent();
            this.mailElements = mailElements;
            domainLogin = mailElements[0].Split(new char[] { '@' });



            //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create("https://auth.mail.ru/cgi-bin/auth?from=splash&Domain=" + domainLogin[1] + "&Login=" + domainLogin[0] + "&Password=" + mailElements[1]);
            //myRequest.Proxy = myProxy;

            //HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            //webBrowser1.DocumentStream = myResponse.GetResponseStream();

            //webBrowser1.Navigating += new WebBrowserNavigatingEventHandler(webBrowser1_Navigating);




            //this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            webBrowser1.Navigate("auth.mail.ru/cgi-bin/auth?from=splash&Domain="+domainLogin[1]+"&Login="+domainLogin[0]+"&Password=" + mailElements[1]);
            //this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //webBrowser1.Document.Cookie = cookies;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //cookies = webBrowser1.Document.Cookie;
        }


        //void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        //{

        //        HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create("https://e.mail.ru");
        //        myRequest.Proxy = myProxy;

        //        HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

        //        webBrowser1.DocumentStream = myResponse.GetResponseStream();
        //        e.Cancel = true;

        //}

    }
}
