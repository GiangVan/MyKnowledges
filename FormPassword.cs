using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YourExperience.OtherClasses;

namespace YourExperience
{
    public partial class FormPassword : Form
    {
        byte[] returner;
        Point moveFormWithMouse;

        public FormPassword()
        {
            InitializeComponent();
        }

        public new byte[] Show()
        {
            Left = MousePosition.X - 50;
            Top = MousePosition.Y - 8;
            textBox1.Select();
            new Animation(this);
            ShowDialog();
            textBox1.Text = "";
            textBox2.Text = "";
            byte[] bytes = returner;
            returner = null;
            return bytes;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != textBox1.Text && textBox2.ForeColor == Color.Black)
            {
                textBox2.ForeColor = Color.Red;
            }
            else if (textBox2.Text == textBox1.Text && textBox2.ForeColor != Color.Black)
                textBox2.ForeColor = Color.Black;
            textBox1.Text = CheckChars(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text != textBox1.Text && textBox2.ForeColor == Color.Black)
            {
                textBox2.ForeColor = Color.Red;
            }
            else if(textBox2.Text == textBox1.Text && textBox2.ForeColor != Color.Black)
                textBox2.ForeColor = Color.Black;
            textBox2.Text = CheckChars(textBox2.Text);
        }
        private string CheckChars(string text)
        {
            Regex regex = new Regex("[^!-~]");
            if (regex.Matches(text).Count != 0)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Special character detection: " + text.Substring(text.Length - 1, 1), this);
                text = "";
            }
            return text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.TextLength == 0 && textBox1.TextLength == 0)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Empty password!", this);
                textBox1.Select();
                return;
            }
            if (textBox1.Text.Length < 8)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Number of chars must be more than EIGHT chars", this);
                textBox1.Select();
                return;
            }
            if (textBox2.Text != textBox1.Text)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Those passwords didn't match!", this);
                textBox2.Select();
                return;
            }
            
            returner = Encoding.ASCII.GetBytes(textBox2.Text);
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
        }

        //phím Ctrl + A
        #region
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                textBox2.SelectAll();
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

        //ẩn / hiện password
        #region
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.eye;
            textBox1.PasswordChar = '\0';
            textBox2.PasswordChar = '\0';
            textBox1.SelectionStart = textBox1.TextLength;
            textBox2.SelectionStart = textBox2.TextLength;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.blind_eye_sign;
            textBox1.PasswordChar = '●';
            textBox2.PasswordChar = '●';
            textBox1.SelectionStart = textBox1.TextLength;
            textBox2.SelectionStart = textBox2.TextLength;
        }
        #endregion

        //"button Close"
        #region
        private void labelClose_Click(object sender, EventArgs e)
        {
            returner = null;
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
        }
        private void labelClose_MouseEnter(object sender, EventArgs e)
        {
            labelClose.BackColor = Color.Red;
        }
        private void labelClose_MouseLeave(object sender, EventArgs e)
        {
            labelClose.BackColor = panel1.BackColor;
        }
        #endregion

        
    }
}
