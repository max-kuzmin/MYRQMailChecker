using System.Windows.Forms;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace Mail_Checker
{
    public partial class MailBrowser : Form
    {
        string[] mailElements;
        string[] loginDomain;



        public MailBrowser(string[] mailElements)
        {
            InitializeComponent();
            this.mailElements = mailElements;
            loginDomain = mailElements[0].Split(new char[] { '@' });
            string serv = Checker.DetectDomain(mailElements[0]);

            
            





            if (serv == "imap.mail.ru")
            {
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate("auth.mail.ru/cgi-bin/auth?from=splash&Domain=" + loginDomain[1] + "&Login=" + loginDomain[0] + "&Password=" + mailElements[1], "", new byte[] { }, "");
            }
            else if (serv == "imap.yandex.ru")
            {           
                UTF8Encoding encoding = new UTF8Encoding();
                string requestPayload = "login=" + mailElements[0] + "&passwd=" + mailElements[1] + "&twoweeks=yes";
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate("https://passport.yandex.ru/passport?mode=auth&from=mail&origin=hostroot_new_l_enter&retpath=https://mail.yandex.ru/", "", encoding.GetBytes(requestPayload), "Content-Type: application/x-www-form-urlencoded");


            }
            else if (serv == "imap.rambler.ru")
            {
                
                UTF8Encoding encoding = new UTF8Encoding();
                string requestPayload = "back=https://mail.rambler.ru/#/folder/INBOX/&rname=mail&profile.login=" + mailElements[0] + "&profile.domain=" + loginDomain[1] + "&profile.password=" + mailElements[1] + "&button.submit=";
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate("https://id.rambler.ru/login?back=http://www.rambler.ru/&rname=main", "", encoding.GetBytes(requestPayload), "Content-Type: application/x-www-form-urlencoded");

            }
            else if (serv == "imap.qip.ru")
            {


                UTF8Encoding encoding = new UTF8Encoding();
                string requestPayload = "login=" + loginDomain[0] + "&host=" + loginDomain[1] + "&password=" + mailElements[1];
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate("https://qip.ru/login?retpath=https://mail.qip.ru/~Inbox;", "", encoding.GetBytes(requestPayload), "Content-Type: application/x-www-form-urlencoded");
                

            }


            
        }

    }
}
