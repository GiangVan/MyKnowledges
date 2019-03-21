using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormQuestion : Form
    {
        Point moveFormWithMouse;
        DialogResult returner = DialogResult.No;

        public FormQuestion()
        {
            InitializeComponent();
        }

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

        //các buttons
        #region
        private void buttonNo_Click(object sender, EventArgs e)
        {
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
        }
        private void buttonYes_Click(object sender, EventArgs e)
        {
            panel1.MouseMove -= panel1_MouseMove;
            returner = DialogResult.Yes;
            Hide();
        }
        #endregion

        //show
        #region
        public DialogResult Show(string text, Form form)
        {
            Left = form.Left + form.Width / 2 - Width / 2;
            Top = form.Top + form.Height / 2 - Height / 2;
            labelText.Text = text;
            returner = DialogResult.No;
            buttonNo.Select();
            ShowDialog();
            return returner;
        }
        public DialogResult Show(string text)
        {
            Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2;
            Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2;
            labelText.Text = text;
            returner = DialogResult.No;
            buttonNo.Select();
            ShowDialog();
            return returner;
        }
        #endregion


    }
}
