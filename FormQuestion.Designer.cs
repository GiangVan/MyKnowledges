namespace YourExperience
{
    partial class FormQuestion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuestion));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelText = new System.Windows.Forms.Label();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(-45, -31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 51);
            this.panel1.TabIndex = 15;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // labelText
            // 
            this.labelText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelText.Font = new System.Drawing.Font("Segoe UI Black", 16.75F, System.Drawing.FontStyle.Bold);
            this.labelText.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labelText.Location = new System.Drawing.Point(12, 33);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(297, 43);
            this.labelText.TabIndex = 16;
            this.labelText.Text = "Are you sure?";
            this.labelText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonYes
            // 
            this.buttonYes.BackColor = System.Drawing.Color.DimGray;
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonYes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonYes.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonYes.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonYes.Location = new System.Drawing.Point(164, 104);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(152, 30);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.TabStop = false;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = false;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.BackColor = System.Drawing.Color.DimGray;
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonNo.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonNo.Location = new System.Drawing.Point(6, 104);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(152, 30);
            this.buttonNo.TabIndex = 0;
            this.buttonNo.TabStop = false;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = false;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Location = new System.Drawing.Point(-12, 142);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(383, 51);
            this.panel2.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BackColor = System.Drawing.Color.DimGray;
            this.panel3.Location = new System.Drawing.Point(-20, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(22, 145);
            this.panel3.TabIndex = 17;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.DimGray;
            this.panel4.Location = new System.Drawing.Point(319, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(22, 145);
            this.panel4.TabIndex = 18;
            // 
            // FormQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonNo;
            this.ClientSize = new System.Drawing.Size(321, 147);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(321, 147);
            this.Name = "FormQuestion";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}