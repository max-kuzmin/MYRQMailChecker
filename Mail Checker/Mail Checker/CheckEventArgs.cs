using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mail_Checker
{
    enum CheckErrors {
        proxyError,
        mailError,
        anotherError,
        noError
    }

    

    struct MailInfo
    {
        public MailInfo(string login, string pass, int[] messages)
        {
            this.login = login;
            this.pass = pass;
            this.messages = messages;
        }


        public string login, pass;
        public int[] messages;
    }

    struct CheckState
    {
        public CheckState(int mails, int proxys, int errors, int valid, int novalid, int threads)
        {
            this.mails = mails;
            this.proxys = proxys;
            this.errors = errors;
            this.valid = valid;
            this.novalid = novalid;
            this.threads = threads;
        }


        public int mails, proxys, errors, valid, novalid, threads;
    }

    class CheckEventArgs: EventArgs
    {
        public CheckErrors error;
        public MailInfo mail;
        public CheckState state;

        public CheckEventArgs(CheckErrors err, MailInfo mail, CheckState state)
        {
            error = err;
            this.mail = mail;
            this.state = state;
        }
    }
}
