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
    public partial class FormLog : Form
    {
        private byte[] passwordBytes = null;
        public bool is_Enter;//sử dụng để nhận biết rằng người dúng có nhấn buttonLogin hay ko

        public FormLog(string text)
        {
            InitializeComponent();
            Text = text;
            ShowDialog();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (textBoxPassword.TextLength > 0) Clipboard.SetText(textBoxPassword.Text);
            if (e.Control && e.KeyCode == Keys.A)
                textBoxPassword.SelectAll();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex("[^!-~]");
            if (regex.Matches(textBoxPassword.Text).Count != 0)
            {
                new FormNotification("Special character detection: \"" + textBoxPassword.Text.Substring(textBoxPassword.Text.Length - 1, 1) + "\"", false);
                textBoxPassword.Text = "";
            }
            passwordBytes = Encoding.ASCII.GetBytes(textBoxPassword.Text);
        }

        public byte[] GetPass() { return passwordBytes; }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //kiểm tra độ dài
            if (passwordBytes == null || passwordBytes.Length < 8)
            {
                new FormNotification("Number of chars must be more than EIGHT chars", false);
                textBoxPassword.Select();
                return;
            }
            is_Enter = true;
            Close();
        }
    }
}
