using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mail_Checker
{
    public partial class FormProxy : Form
    {
        ProxyChecker c = null;
        int threads = 300, timeout=10000;
        public FormProxy(int threads, int timeout)
        {
            InitializeComponent();
            this.threads = threads;
            this.timeout = timeout*1000;
            deleg += UpdGUIFunc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1start.Text == "Открыть")
            {
                FileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();

                List<string> proxys = new List<string>(Form1main.DoubleRemover(File.ReadAllLines(dialog.FileName)));


                c = new ProxyChecker(proxys, threads,timeout);
                c.CheckComplete += C_CheckComplete;
                c.CheckAsync();

                button1start.Text = "Стоп";
                button1start.BackColor = Color.LightSalmon;
            }
            else
            {
                button1start.Text = "Открыть";
                button1start.BackColor = Color.LightGreen;
                c.Stop();
            }

        }


        private void C_CheckComplete(object sender, EventArgs e)
        {
            try {
                this.Invoke(deleg);
            }
            catch{}

        }

        delegate void UpdGUI();
        UpdGUI deleg;
        void UpdGUIFunc()
        {
            label1lasts.Text = (c.ProxyLast + c.Threads).ToString();
            label1good.Text = c.Goods.ToString();
            label1threads.Text = c.Threads.ToString();

        }

        private void FormProxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (c!=null) c.Stop();
        }
    }
}
