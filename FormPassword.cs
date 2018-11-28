using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormPassword : Form
    {
        private byte[] passwordBytes;

        public FormPassword(Point location, string text)
        {
            InitializeComponent();
            Location = location;
            Text = text;
            ShowDialog();
        }

        public byte[] GetPassword()
        {
            return passwordBytes;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = CheckChars(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = CheckChars(textBox2.Text);
        }
        private string CheckChars(string value)
        {
            Regex regex = new Regex("[^!-~]");
            if (regex.Matches(value).Count != 0)
            {
                new FormNotification("Special character detection: " + value.Substring(value.Length - 1, 1), false);
                value = "";
            }
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != textBox1.Text)
            {
                new FormNotification("Those passwords didn't match!\nTry again?", false);
                textBox2.Select();
                return;
            }
            if (textBox2.Text.Length < 8)
            {
                new FormNotification("Number of chars must be more than EIGHT chars", false);
                textBox1.Select();
                return;
            }
            passwordBytes = Encoding.ASCII.GetBytes(textBox2.Text);
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (textBox1.TextLength > 0) Clipboard.SetText(textBox1.Text);
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (textBox2.TextLength > 0) Clipboard.SetText(textBox2.Text);
            if (e.Control && e.KeyCode == Keys.A)
                textBox2.SelectAll();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.eye;
            textBox1.UseSystemPasswordChar = false;
            textBox2.UseSystemPasswordChar = false;
            textBox1.SelectionStart = textBox1.TextLength;
            textBox2.SelectionStart = textBox2.TextLength;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.blind_eye_sign;
            textBox1.UseSystemPasswordChar = true;
            textBox2.UseSystemPasswordChar = true;
            textBox1.SelectionStart = textBox1.TextLength;
            textBox2.SelectionStart = textBox2.TextLength;
        }

    }
}
