namespace Mail_Checker
{
    partial class FormProxy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProxy));
            this.button1start = new System.Windows.Forms.Button();
            this.label1lasts = new System.Windows.Forms.Label();
            this.label1good = new System.Windows.Forms.Label();
            this.label1l = new System.Windows.Forms.Label();
            this.label1g = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1threads = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1start
            // 
            this.button1start.BackColor = System.Drawing.Color.PaleGreen;
            this.button1start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1start.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1start.Location = new System.Drawing.Point(39, 157);
            this.button1start.Name = "button1start";
            this.button1start.Size = new System.Drawing.Size(225, 43);
            this.button1start.TabIndex = 0;
            this.button1start.Text = "Открыть";
            this.button1start.UseVisualStyleBackColor = false;
            this.button1start.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1lasts
            // 
            this.label1lasts.AutoSize = true;
            this.label1lasts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1lasts.Location = new System.Drawing.Point(159, 19);
            this.label1lasts.Name = "label1lasts";
            this.label1lasts.Size = new System.Drawing.Size(13, 18);
            this.label1lasts.TabIndex = 1;
            this.label1lasts.Text = "-";
            // 
            // label1good
            // 
            this.label1good.AutoSize = true;
            this.label1good.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1good.Location = new System.Drawing.Point(159, 54);
            this.label1good.Name = "label1good";
            this.label1good.Size = new System.Drawing.Size(13, 18);
            this.label1good.TabIndex = 2;
            this.label1good.Text = "-";
            // 
            // label1l
            // 
            this.label1l.AutoSize = true;
            this.label1l.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1l.Location = new System.Drawing.Point(24, 19);
            this.label1l.Name = "label1l";
            this.label1l.Size = new System.Drawing.Size(81, 18);
            this.label1l.TabIndex = 3;
            this.label1l.Text = "Осталось:";
            // 
            // label1g
            // 
            this.label1g.AutoSize = true;
            this.label1g.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1g.Location = new System.Drawing.Point(24, 54);
            this.label1g.Name = "label1g";
            this.label1g.Size = new System.Drawing.Size(75, 18);
            this.label1g.TabIndex = 4;
            this.label1g.Text = "Хорошие:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1threads);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label1good);
            this.panel1.Controls.Add(this.label1l);
            this.panel1.Controls.Add(this.label1lasts);
            this.panel1.Controls.Add(this.label1g);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 127);
            this.panel1.TabIndex = 5;
            // 
            // label1threads
            // 
            this.label1threads.AutoSize = true;
            this.label1threads.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1threads.Location = new System.Drawing.Point(159, 91);
            this.label1threads.Name = "label1threads";
            this.label1threads.Size = new System.Drawing.Size(13, 18);
            this.label1threads.TabIndex = 6;
            this.label1threads.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(24, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 18);
            this.label12.TabIndex = 5;
            this.label12.Text = "Потоки:";
            // 
            // FormProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(308, 218);
            this.Controls.Add(this.button1start);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormProxy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proxy Checker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProxy_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1start;
        private System.Windows.Forms.Label label1lasts;
        private System.Windows.Forms.Label label1good;
        private System.Windows.Forms.Label label1l;
        private System.Windows.Forms.Label label1g;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1threads;
        private System.Windows.Forms.Label label12;
    }
}

