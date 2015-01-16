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

namespace Mail_Checker
{
    public partial class MailBrowser : Form
    {
        string[] mailElements;
        public MailBrowser(string[] mailElements)
        {
            InitializeComponent();
            this.mailElements = mailElements;


            OpenMailBox();

        }

        private void OpenMailBox()
        {
            throw new NotImplementedException();
        }




    }
}
