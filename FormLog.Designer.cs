namespace YourExperience
{
    partial class FormLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.contextMenuStripNone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.labelCapsLock = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelClose = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::YourExperience.Properties.Resources.padlock;
            this.pictureBox1.Location = new System.Drawing.Point(14, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(126, 131);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonLogin
            // 
            this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonLogin.Location = new System.Drawing.Point(149, 58);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(160, 44);
            this.buttonLogin.TabIndex = 1;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPassword.ContextMenuStrip = this.contextMenuStripNone;
            this.textBoxPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.textBoxPassword.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxPassword.Location = new System.Drawing.Point(149, 27);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '●';
            this.textBoxPassword.Size = new System.Drawing.Size(160, 25);
            this.textBoxPassword.TabIndex = 0;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_KeyDown);
            // 
            // contextMenuStripNone
            // 
            this.contextMenuStripNone.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripNone.Name = "contextMenuStrip1";
            this.contextMenuStripNone.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(290, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "off";
            // 
            // labelCapsLock
            // 
            this.labelCapsLock.AutoSize = true;
            this.labelCapsLock.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelCapsLock.ForeColor = System.Drawing.Color.Silver;
            this.labelCapsLock.Location = new System.Drawing.Point(237, 103);
            this.labelCapsLock.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCapsLock.Name = "labelCapsLock";
            this.labelCapsLock.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelCapsLock.Size = new System.Drawing.Size(61, 13);
            this.labelCapsLock.TabIndex = 7;
            this.labelCapsLock.Text = "CapsLock: ";
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(1083, 102);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "button1";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.labelClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(-8, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 20);
            this.panel1.TabIndex = 10;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // labelClose
            // 
            this.labelClose.BackColor = System.Drawing.Color.Black;
            this.labelClose.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.labelClose.ForeColor = System.Drawing.SystemColors.Control;
            this.labelClose.Location = new System.Drawing.Point(293, -2);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(29, 21);
            this.labelClose.TabIndex = 1;
            this.labelClose.Text = "x";
            this.labelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelClose.Click += new System.EventHandler(this.labelClose_Click);
            this.labelClose.MouseEnter += new System.EventHandler(this.labelClose_MouseEnter);
            this.labelClose.MouseLeave += new System.EventHandler(this.labelClose_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(-7, 161);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(385, 20);
            this.panel2.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(320, -11);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 174);
            this.panel3.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(-8, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 174);
            this.panel4.TabIndex = 13;
            // 
            // FormLog
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(322, 166);
            this.Controls.Add(this.labelClose);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCapsLock);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(322, 166);
            this.Name = "FormLog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCapsLock;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}