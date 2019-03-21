using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YourExperience.Other_Classes;

namespace YourExperience
{
    public partial class FormNotification : Form
    {
        
        Point moveFormWithMouse;
        object returner;

        public FormNotification()
        {
            InitializeComponent();

            buttonYes.Tag = DialogResult.Yes;
            buttonOK.Tag = DialogResult.OK;
            buttonRetry.Tag = DialogResult.Retry;
            buttonCancel.Tag = DialogResult.Cancel;
            buttonNo.Tag = DialogResult.No;
        }
        //các buttons
        #region
        private void button_Click(object sender, EventArgs e)
        {
            returner = ((Control)sender).Tag;
        }
        #endregion

        //di chuyển form theo chuột
        #region
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.MouseMove += panel1_MouseMove;
            moveFormWithMouse = new Point(MousePosition.X - Location.X, MousePosition.Y - Location.Y);
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            panel1.MouseMove -= panel1_MouseMove;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(moveFormWithMouse == Location))
            {
                Location = new Point(MousePosition.X - moveFormWithMouse.X, MousePosition.Y - moveFormWithMouse.Y);
            }
        }
        #endregion

        //show
        #region
        public DialogResult Show(MessageBoxButtons buttons, string text, Form form)
        {
            if (form == null)
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
                Left = form.Left + form.Width / 2 - Width / 2;
                Top = form.Top + form.Height / 2 - Height / 2;
            }
            richTextBox1.WordWrap = true;
            Text = text;
            KhongBietDatTenSaoLuon(buttons, text);
            return (DialogResult)returner;
        }
        public DialogResult Show(MessageBoxButtons buttons, string text, Exception ex, Form form)
        {
            if(form == null)
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
                Left = form.Left + form.Width / 2 - Width / 2;
                Top = form.Top + form.Height / 2 - Height / 2;
            }
            richTextBox1.WordWrap = false;
            Text = text;
            KhongBietDatTenSaoLuon(buttons, text + "\n" + ex.Message + "\n" + ex.StackTrace);
            //lưu số lần bị lỗi vào file "System.ini"
            SystemFile.Save((int.Parse(SystemFile.Get(3)) + 1).ToString(), 3);
            ErrorFile.Save(ex);
            //
            return (DialogResult)returner;
        }
        void KhongBietDatTenSaoLuon(MessageBoxButtons buttons, string text)
        {
            if (buttons == MessageBoxButtons.RetryCancel)
            {
                buttonYes.Visible = false;
                buttonRetry.Visible = true;
                buttonNo.Visible = false;
                buttonOK.Visible = false;
                buttonCancel.Visible = true;
                AcceptButton = buttonRetry;
                CancelButton = buttonCancel;
                buttonRetry.Select();
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                buttonYes.Visible = true;
                buttonRetry.Visible = false;
                buttonNo.Visible = true;
                buttonOK.Visible = false;
                buttonCancel.Visible = false;
                AcceptButton = buttonYes;
                CancelButton = buttonNo;
                buttonYes.Select();
            }
            else
            {
                buttonYes.Visible = false;
                buttonRetry.Visible = false;
                buttonNo.Visible = false;
                buttonOK.Visible = true;
                buttonCancel.Visible = false;
                AcceptButton = buttonOK;
                CancelButton = buttonOK;
                buttonOK.Select();
            }
            richTextBox1.Text = text;
            try
            {
                System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(@"C:\Windows\media\Windows User Account Control.wav");
                soundPlayer.Play();
            }
            catch { }
            ShowDialog();
        }

        #endregion

        private void FormNotification_FormClosing(object sender, FormClosingEventArgs e)
        {
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
            e.Cancel = true;
        }
    }
}
