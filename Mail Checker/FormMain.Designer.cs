namespace Mail_Checker
{
    partial class Form1main
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1main));
            this.button1mails = new System.Windows.Forms.Button();
            this.button2proxys = new System.Windows.Forms.Button();
            this.button3start = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7threads = new System.Windows.Forms.Label();
            this.label8valid = new System.Windows.Forms.Label();
            this.label9novalid = new System.Windows.Forms.Label();
            this.label10mails = new System.Windows.Forms.Label();
            this.label11proxys = new System.Windows.Forms.Label();
            this.label12errors = new System.Windows.Forms.Label();
            this.textBox1threads = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2timeout = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1query = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1login = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2pass = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox2doubles = new System.Windows.Forms.CheckBox();
            this.checkBox1proxyCheck = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1mails
            // 
            this.button1mails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1mails.Location = new System.Drawing.Point(25, 13);
            this.button1mails.Name = "button1mails";
            this.button1mails.Size = new System.Drawing.Size(149, 30);
            this.button1mails.TabIndex = 0;
            this.button1mails.Text = "Загрузить почты";
            this.button1mails.UseVisualStyleBackColor = true;
            this.button1mails.Click += new System.EventHandler(this.button1mails_Click);
            // 
            // button2proxys
            // 
            this.button2proxys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2proxys.Location = new System.Drawing.Point(25, 49);
            this.button2proxys.Name = "button2proxys";
            this.button2proxys.Size = new System.Drawing.Size(149, 30);
            this.button2proxys.TabIndex = 1;
            this.button2proxys.Text = "Загрузить прокси";
            this.button2proxys.UseVisualStyleBackColor = true;
            this.button2proxys.Click += new System.EventHandler(this.button2proxys_Click);
            // 
            // button3start
            // 
            this.button3start.BackColor = System.Drawing.Color.LightGreen;
            this.button3start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3start.Location = new System.Drawing.Point(25, 284);
            this.button3start.Name = "button3start";
            this.button3start.Size = new System.Drawing.Size(149, 48);
            this.button3start.TabIndex = 2;
            this.button3start.Text = "Старт";
            this.button3start.UseVisualStyleBackColor = false;
            this.button3start.Click += new System.EventHandler(this.button3start_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(228, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(342, 469);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Логин";
            this.columnHeader1.Width = 153;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Пароль";
            this.columnHeader2.Width = 143;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ост. прокси:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ост. почты:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Потоки:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ошибки:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Валидные:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Невалидные:";
            // 
            // label7threads
            // 
            this.label7threads.AutoSize = true;
            this.label7threads.Location = new System.Drawing.Point(126, 96);
            this.label7threads.Name = "label7threads";
            this.label7threads.Size = new System.Drawing.Size(16, 17);
            this.label7threads.TabIndex = 10;
            this.label7threads.Text = "0";
            // 
            // label8valid
            // 
            this.label8valid.AutoSize = true;
            this.label8valid.Location = new System.Drawing.Point(126, 45);
            this.label8valid.Name = "label8valid";
            this.label8valid.Size = new System.Drawing.Size(16, 17);
            this.label8valid.TabIndex = 11;
            this.label8valid.Text = "0";
            // 
            // label9novalid
            // 
            this.label9novalid.AutoSize = true;
            this.label9novalid.Location = new System.Drawing.Point(126, 62);
            this.label9novalid.Name = "label9novalid";
            this.label9novalid.Size = new System.Drawing.Size(16, 17);
            this.label9novalid.TabIndex = 12;
            this.label9novalid.Text = "0";
            // 
            // label10mails
            // 
            this.label10mails.AutoSize = true;
            this.label10mails.Location = new System.Drawing.Point(126, 28);
            this.label10mails.Name = "label10mails";
            this.label10mails.Size = new System.Drawing.Size(16, 17);
            this.label10mails.TabIndex = 13;
            this.label10mails.Text = "0";
            // 
            // label11proxys
            // 
            this.label11proxys.AutoSize = true;
            this.label11proxys.Location = new System.Drawing.Point(126, 11);
            this.label11proxys.Name = "label11proxys";
            this.label11proxys.Size = new System.Drawing.Size(16, 17);
            this.label11proxys.TabIndex = 14;
            this.label11proxys.Text = "0";
            // 
            // label12errors
            // 
            this.label12errors.AutoSize = true;
            this.label12errors.Location = new System.Drawing.Point(126, 79);
            this.label12errors.Name = "label12errors";
            this.label12errors.Size = new System.Drawing.Size(16, 17);
            this.label12errors.TabIndex = 15;
            this.label12errors.Text = "0";
            // 
            // textBox1threads
            // 
            this.textBox1threads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1threads.Location = new System.Drawing.Point(95, 94);
            this.textBox1threads.Name = "textBox1threads";
            this.textBox1threads.Size = new System.Drawing.Size(79, 22);
            this.textBox1threads.TabIndex = 0;
            this.textBox1threads.Text = "300";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Потоки:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Таймаут:";
            // 
            // textBox2timeout
            // 
            this.textBox2timeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2timeout.Location = new System.Drawing.Point(95, 119);
            this.textBox2timeout.Name = "textBox2timeout";
            this.textBox2timeout.Size = new System.Drawing.Size(79, 22);
            this.textBox2timeout.TabIndex = 18;
            this.textBox2timeout.Text = "10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(63, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Запросы:";
            // 
            // textBox1query
            // 
            this.textBox1query.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1query.Location = new System.Drawing.Point(25, 220);
            this.textBox1query.Multiline = true;
            this.textBox1query.Name = "textBox1query";
            this.textBox1query.Size = new System.Drawing.Size(149, 58);
            this.textBox1query.TabIndex = 20;
            this.textBox1query.Text = "@somesite1.com\r\n@somesite2.com";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1login,
            this.toolStripMenuItem2pass});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(218, 52);
            // 
            // toolStripMenuItem1login
            // 
            this.toolStripMenuItem1login.Name = "toolStripMenuItem1login";
            this.toolStripMenuItem1login.Size = new System.Drawing.Size(217, 24);
            this.toolStripMenuItem1login.Text = "Копировать логин";
            this.toolStripMenuItem1login.Click += new System.EventHandler(this.toolStripMenuItem1login_Click);
            // 
            // toolStripMenuItem2pass
            // 
            this.toolStripMenuItem2pass.Name = "toolStripMenuItem2pass";
            this.toolStripMenuItem2pass.Size = new System.Drawing.Size(217, 24);
            this.toolStripMenuItem2pass.Text = "Копировать пароль";
            this.toolStripMenuItem2pass.Click += new System.EventHandler(this.toolStripMenuItem2pass_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7threads);
            this.panel1.Controls.Add(this.label12errors);
            this.panel1.Controls.Add(this.label8valid);
            this.panel1.Controls.Add(this.label11proxys);
            this.panel1.Controls.Add(this.label9novalid);
            this.panel1.Controls.Add(this.label10mails);
            this.panel1.Location = new System.Drawing.Point(13, 358);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 123);
            this.panel1.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.checkBox2doubles);
            this.panel2.Controls.Add(this.checkBox1proxyCheck);
            this.panel2.Controls.Add(this.textBox1query);
            this.panel2.Controls.Add(this.button1mails);
            this.panel2.Controls.Add(this.button2proxys);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.button3start);
            this.panel2.Controls.Add(this.textBox2timeout);
            this.panel2.Controls.Add(this.textBox1threads);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(13, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 340);
            this.panel2.TabIndex = 22;
            // 
            // checkBox2doubles
            // 
            this.checkBox2doubles.AutoSize = true;
            this.checkBox2doubles.Location = new System.Drawing.Point(25, 147);
            this.checkBox2doubles.Name = "checkBox2doubles";
            this.checkBox2doubles.Size = new System.Drawing.Size(128, 21);
            this.checkBox2doubles.TabIndex = 22;
            this.checkBox2doubles.Text = "Удалять дубли";
            this.checkBox2doubles.UseVisualStyleBackColor = true;
            // 
            // checkBox1proxyCheck
            // 
            this.checkBox1proxyCheck.AutoSize = true;
            this.checkBox1proxyCheck.Location = new System.Drawing.Point(25, 173);
            this.checkBox1proxyCheck.Name = "checkBox1proxyCheck";
            this.checkBox1proxyCheck.Size = new System.Drawing.Size(127, 21);
            this.checkBox1proxyCheck.TabIndex = 21;
            this.checkBox1proxyCheck.Text = "Чекать прокси";
            this.checkBox1proxyCheck.UseVisualStyleBackColor = true;
            this.checkBox1proxyCheck.CheckedChanged += new System.EventHandler(this.checkBox1proxyCheck_CheckedChanged);
            // 
            // Form1main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 493);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 540);
            this.Name = "Form1main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Worst MYRQ Checker r14";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1mails;
        private System.Windows.Forms.Button button2proxys;
        private System.Windows.Forms.Button button3start;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7threads;
        private System.Windows.Forms.Label label8valid;
        private System.Windows.Forms.Label label9novalid;
        private System.Windows.Forms.Label label10mails;
        private System.Windows.Forms.Label label11proxys;
        private System.Windows.Forms.Label label12errors;
        private System.Windows.Forms.TextBox textBox1threads;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2timeout;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1query;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1login;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2pass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox checkBox2doubles;
        private System.Windows.Forms.CheckBox checkBox1proxyCheck;
    }
}

