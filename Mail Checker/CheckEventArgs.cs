using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mail_Checker
{
    enum CheckErrors {
        proxyError,
        mailError,
        noError
    }


    class CheckEventArgs
    {
        CheckErrors error;
        string login, pass;
        int messages;
        public CheckEventArgs(CheckErrors err, string login="", string pass="", int messages=0)
        {
            error = err;
            this.login = login;
            this.pass = pass;
            this.messages = messages;
        }
    }
}
