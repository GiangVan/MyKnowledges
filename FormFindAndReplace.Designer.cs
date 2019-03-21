namespace YourExperience
{
    partial class FormFindAndReplace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFindAndReplace));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.contextMenuStripNone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBoxReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonFind = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.buttonReplaceAll = new System.Windows.Forms.Button();
            this.labelLength = new System.Windows.Forms.Label();
            this.labelFound = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelClose = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // textBoxFind
            // 
            this.textBoxFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFind.ContextMenuStrip = this.contextMenuStripNone;
            this.textBoxFind.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.textBoxFind.ForeColor = System.Drawing.Color.DimGray;
            this.textBoxFind.Location = new System.Drawing.Point(10, 41);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxFind.Size = new System.Drawing.Size(334, 23);
            this.textBoxFind.TabIndex = 0;
            this.textBoxFind.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // contextMenuStripNone
            // 
            this.contextMenuStripNone.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripNone.Name = "contextMenuStrip1";
            this.contextMenuStripNone.Size = new System.Drawing.Size(61, 4);
            // 
            // textBoxReplace
            // 
            this.textBoxReplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReplace.ContextMenuStrip = this.contextMenuStripNone;
            this.textBoxReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReplace.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.textBoxReplace.ForeColor = System.Drawing.Color.DimGray;
            this.textBoxReplace.Location = new System.Drawing.Point(0, 0);
            this.textBoxReplace.Name = "textBoxReplace";
            this.textBoxReplace.Size = new System.Drawing.Size(334, 23);
            this.textBoxReplace.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Replace with:";
            // 
            // buttonFind
            // 
            this.buttonFind.BackColor = System.Drawing.Color.Silver;
            this.buttonFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonFind.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonFind.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonFind.Location = new System.Drawing.Point(269, 115);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 4;
            this.buttonFind.Text = "Find next";
            this.buttonFind.UseVisualStyleBackColor = false;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.Color.DimGray;
            this.button1.Location = new System.Drawing.Point(269, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Nothing";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // buttonReplace
            // 
            this.buttonReplace.Enabled = false;
            this.buttonReplace.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonReplace.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonReplace.ForeColor = System.Drawing.Color.DimGray;
            this.buttonReplace.Location = new System.Drawing.Point(188, 115);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(75, 23);
            this.buttonReplace.TabIndex = 5;
            this.buttonReplace.Text = "Replace";
            this.buttonReplace.UseVisualStyleBackColor = false;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // buttonReplaceAll
            // 
            this.buttonReplaceAll.Enabled = false;
            this.buttonReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonReplaceAll.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonReplaceAll.ForeColor = System.Drawing.Color.DimGray;
            this.buttonReplaceAll.Location = new System.Drawing.Point(188, 141);
            this.buttonReplaceAll.Name = "buttonReplaceAll";
            this.buttonReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.buttonReplaceAll.TabIndex = 6;
            this.buttonReplaceAll.Text = "Replace all";
            this.buttonReplaceAll.UseVisualStyleBackColor = false;
            this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.labelLength.ForeColor = System.Drawing.Color.Gray;
            this.labelLength.Location = new System.Drawing.Point(0, 31);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(55, 13);
            this.labelLength.TabIndex = 7;
            this.labelLength.Text = "Length: 0";
            // 
            // labelFound
            // 
            this.labelFound.AutoSize = true;
            this.labelFound.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.labelFound.ForeColor = System.Drawing.Color.Gray;
            this.labelFound.Location = new System.Drawing.Point(0, 1);
            this.labelFound.Name = "labelFound";
            this.labelFound.Size = new System.Drawing.Size(75, 13);
            this.labelFound.TabIndex = 8;
            this.labelFound.Text = "Had found: 0";
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.labelPosition.ForeColor = System.Drawing.Color.Gray;
            this.labelPosition.Location = new System.Drawing.Point(0, 16);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(77, 13);
            this.labelPosition.TabIndex = 9;
            this.labelPosition.Text = "Location: null";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelLength);
            this.panel1.Controls.Add(this.labelPosition);
            this.panel1.Controls.Add(this.labelFound);
            this.panel1.Location = new System.Drawing.Point(10, 115);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 49);
            this.panel1.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(1860, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.TabStop = false;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBoxReplace);
            this.panel3.Location = new System.Drawing.Point(10, 86);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(334, 23);
            this.panel3.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Location = new System.Drawing.Point(-2, -30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(385, 51);
            this.panel2.TabIndex = 14;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // labelClose
            // 
            this.labelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelClose.BackColor = System.Drawing.Color.DimGray;
            this.labelClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelClose.ForeColor = System.Drawing.SystemColors.Control;
            this.labelClose.Location = new System.Drawing.Point(321, -4);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(36, 25);
            this.labelClose.TabIndex = 15;
            this.labelClose.Text = "x";
            this.labelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelClose.Click += new System.EventHandler(this.labelClose_Click);
            this.labelClose.MouseEnter += new System.EventHandler(this.labelClose_MouseEnter);
            this.labelClose.MouseLeave += new System.EventHandler(this.labelClose_MouseLeave);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.DimGray;
            this.panel4.Location = new System.Drawing.Point(-8, 171);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(385, 51);
            this.panel4.TabIndex = 15;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DimGray;
            this.panel5.Location = new System.Drawing.Point(-8, 11);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 169);
            this.panel5.TabIndex = 16;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Location = new System.Drawing.Point(353, -1);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(10, 187);
            this.panel6.TabIndex = 17;
            // 
            // FormFindAndReplace
            // 
            this.AcceptButton = this.buttonFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(355, 177);
            this.Controls.Add(this.labelClose);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.textBoxFind);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonReplaceAll);
            this.Controls.Add(this.buttonReplace);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Enabled = false;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(355, 177);
            this.Name = "FormFindAndReplace";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find and Replace";
            this.Deactivate += new System.EventHandler(this.FormFindAndReplace_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFindAndReplace_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.TextBox textBoxReplace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.Button buttonReplaceAll;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Label labelFound;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNone;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
    }
}