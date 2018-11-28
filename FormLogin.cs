using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormLogin : Form
    {
        private byte[] passwordBytes;

        public FormLogin()
        {
            InitializeComponent();
            labelVersion.Text = "Version: " + Application.ProductVersion;
        }



        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex("[^!-~]");
            if (regex.Matches(textBoxPassword.Text).Count != 0)
            {
                new FormNotification("Special character detection: \"" + textBoxPassword.Text.Substring(textBoxPassword.Text.Length - 1, 1) + "\"", false);
                textBoxPassword.Text = "";
            }
            passwordBytes = System.Text.Encoding.ASCII.GetBytes(textBoxPassword.Text);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //kiểm tra độ dài
            if(passwordBytes == null || passwordBytes.Length < 8)
            {
                new FormNotification("Number of chars must be more than EIGHT chars", false);
                textBoxPassword.Select();
                return;
            }
            //
            Hide();
            using(FormMain formMain = new FormMain(passwordBytes, this))//sử dụng using để hide được notifyion :D
            {
                if (formMain.IsDisposed)
                {
                    Application.Exit();
                    return;
                }
            }
            Show();
            new FormNotification("Wrong password!", false);
            textBoxPassword.Text = "";
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (textBoxPassword.TextLength > 0) Clipboard.SetText(textBoxPassword.Text);
            if (e.Control && e.KeyCode == Keys.A)
                textBoxPassword.SelectAll();
        }
    }
}
