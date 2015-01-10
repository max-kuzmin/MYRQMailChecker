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
            this.button1mails = new System.Windows.Forms.Button();
            this.button2proxys = new System.Windows.Forms.Button();
            this.button3start = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Login = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Pass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Messages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.SuspendLayout();
            // 
            // button1mails
            // 
            this.button1mails.Location = new System.Drawing.Point(38, 12);
            this.button1mails.Name = "button1mails";
            this.button1mails.Size = new System.Drawing.Size(149, 30);
            this.button1mails.TabIndex = 0;
            this.button1mails.Text = "Загрузить почты";
            this.button1mails.UseVisualStyleBackColor = true;
            this.button1mails.Click += new System.EventHandler(this.button1mails_Click);
            // 
            // button2proxys
            // 
            this.button2proxys.Location = new System.Drawing.Point(38, 48);
            this.button2proxys.Name = "button2proxys";
            this.button2proxys.Size = new System.Drawing.Size(149, 30);
            this.button2proxys.TabIndex = 1;
            this.button2proxys.Text = "Загрузить прокси";
            this.button2proxys.UseVisualStyleBackColor = true;
            this.button2proxys.Click += new System.EventHandler(this.button2proxys_Click);
            // 
            // button3start
            // 
            this.button3start.Location = new System.Drawing.Point(38, 223);
            this.button3start.Name = "button3start";
            this.button3start.Size = new System.Drawing.Size(149, 50);
            this.button3start.TabIndex = 2;
            this.button3start.Text = "Старт";
            this.button3start.UseVisualStyleBackColor = true;
            this.button3start.Click += new System.EventHandler(this.button3start_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Login,
            this.Pass,
            this.Messages});
            this.listView1.Location = new System.Drawing.Point(228, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(342, 429);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Login
            // 
            this.Login.Text = "Логин";
            this.Login.Width = 125;
            // 
            // Pass
            // 
            this.Pass.Text = "Пароль";
            this.Pass.Width = 153;
            // 
            // Messages
            // 
            this.Messages.Text = "Сообщ.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Прокси:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Почты:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Потоки:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ошибки:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Валид:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 382);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Невалид:";
            // 
            // label7threads
            // 
            this.label7threads.AutoSize = true;
            this.label7threads.Location = new System.Drawing.Point(105, 420);
            this.label7threads.Name = "label7threads";
            this.label7threads.Size = new System.Drawing.Size(94, 17);
            this.label7threads.TabIndex = 10;
            this.label7threads.Text = "label7threads";
            // 
            // label8valid
            // 
            this.label8valid.AutoSize = true;
            this.label8valid.Location = new System.Drawing.Point(105, 365);
            this.label8valid.Name = "label8valid";
            this.label8valid.Size = new System.Drawing.Size(75, 17);
            this.label8valid.TabIndex = 11;
            this.label8valid.Text = "label8valid";
            // 
            // label9novalid
            // 
            this.label9novalid.AutoSize = true;
            this.label9novalid.Location = new System.Drawing.Point(105, 382);
            this.label9novalid.Name = "label9novalid";
            this.label9novalid.Size = new System.Drawing.Size(91, 17);
            this.label9novalid.TabIndex = 12;
            this.label9novalid.Text = "label9novalid";
            // 
            // label10mails
            // 
            this.label10mails.AutoSize = true;
            this.label10mails.Location = new System.Drawing.Point(105, 348);
            this.label10mails.Name = "label10mails";
            this.label10mails.Size = new System.Drawing.Size(86, 17);
            this.label10mails.TabIndex = 13;
            this.label10mails.Text = "label10mails";
            // 
            // label11proxys
            // 
            this.label11proxys.AutoSize = true;
            this.label11proxys.Location = new System.Drawing.Point(105, 295);
            this.label11proxys.Name = "label11proxys";
            this.label11proxys.Size = new System.Drawing.Size(95, 17);
            this.label11proxys.TabIndex = 14;
            this.label11proxys.Text = "label11proxys";
            // 
            // label12errors
            // 
            this.label12errors.AutoSize = true;
            this.label12errors.Location = new System.Drawing.Point(105, 312);
            this.label12errors.Name = "label12errors";
            this.label12errors.Size = new System.Drawing.Size(92, 17);
            this.label12errors.TabIndex = 15;
            this.label12errors.Text = "label12errors";
            // 
            // textBox1threads
            // 
            this.textBox1threads.Location = new System.Drawing.Point(108, 93);
            this.textBox1threads.Name = "textBox1threads";
            this.textBox1threads.Size = new System.Drawing.Size(79, 22);
            this.textBox1threads.TabIndex = 0;
            this.textBox1threads.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Потоки:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Таймаут:";
            // 
            // textBox2timeout
            // 
            this.textBox2timeout.Location = new System.Drawing.Point(108, 118);
            this.textBox2timeout.Name = "textBox2timeout";
            this.textBox2timeout.Size = new System.Drawing.Size(79, 22);
            this.textBox2timeout.TabIndex = 18;
            this.textBox2timeout.Text = "10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(80, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Запрос:";
            // 
            // textBox1query
            // 
            this.textBox1query.Location = new System.Drawing.Point(38, 177);
            this.textBox1query.Name = "textBox1query";
            this.textBox1query.Size = new System.Drawing.Size(149, 22);
            this.textBox1query.TabIndex = 20;
            this.textBox1query.Text = "@mail.ru";
            // 
            // Form1main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 453);
            this.Controls.Add(this.textBox1query);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox2timeout);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1threads);
            this.Controls.Add(this.label12errors);
            this.Controls.Add(this.label11proxys);
            this.Controls.Add(this.label10mails);
            this.Controls.Add(this.label9novalid);
            this.Controls.Add(this.label8valid);
            this.Controls.Add(this.label7threads);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button3start);
            this.Controls.Add(this.button2proxys);
            this.Controls.Add(this.button1mails);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "Form1main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail.ru Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1mails;
        private System.Windows.Forms.Button button2proxys;
        private System.Windows.Forms.Button button3start;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Login;
        private System.Windows.Forms.ColumnHeader Pass;
        private System.Windows.Forms.ColumnHeader Messages;
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
    }
}

