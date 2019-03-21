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
    public partial class FormLog : Form
    {
        Point moveFormWithMouse;
        byte[] password = null;
        bool returner;//nhận biết người dùng nhấn buttonClose, false là nhấn, true là đăng nhập thành công
        TreeNode node;

        public FormLog()
        {
            InitializeComponent();
        }

        public bool Show(TreeNode node, Form form)
        {
            //show form
            Left = form.Left + form.Width / 2 - Width / 2;
            Top = form.Top + form.Height / 2 - Height / 2;
            textBoxPassword.Select();
            new Animation(this);
            this.node = node;
            ShowDialog();
            textBoxPassword.Text = "";
            this.node = null;
            password = null;
            return returner;//trả về true khi giải mã thành công
        }

        void Log()//giải mã và nạp dữ liệu
        {
            //giải mã nội dung
            WindowsForm.Loading2.Show(this);
            byte[] pass = AdvancedEncryptionStandard.Hash(password);
            string str = AdvancedEncryptionStandard.Decoding(Encoding.Default.GetBytes(node.Name), pass);
            //if(str.Substring(NetworkNodes.checkingCode.Length) == "\u0010ÛâÈ r”r']{0S‡J%\u008dPm\b\u000eAáÁ\t\u0017í+¬*‡„")
            //    MessageBox.Show("");
            if (NetworkNodes.CheckPassword(str))
            {
                node.Name = str.Substring(NetworkNodes.checkingCode.Length);
                try
                {
                    //giải mã và nạp các nodes con
                    NetworkNodes.Create(node, AdvancedEncryptionStandard.Decoding(Encoding.Default.GetBytes(((Tag_of_Node)node.Tag).nodes), pass));
                }
                catch (Exception ex)
                {
                    WindowsForm.Loading2.End();
                    WindowsForm.Notification.Show(MessageBoxButtons.OK, "Can not create a TreeNode from the string!", ex, this);
                }
                WindowsForm.Loading2.End();
                ((Tag_of_Node)node.Tag).password = pass;
                ((Tag_of_Node)node.Tag).unlocked = true;
                ((Tag_of_Node)node.Tag).nodes = null;
                returner = true;
                panel1.MouseMove -= panel1_MouseMove;
                Hide();
            }
            else
            {
                WindowsForm.Loading2.End();
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Wrong!", this);
                textBoxPassword.Text = "";
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsKeyLocked(Keys.CapsLock))
            {
                label1.ForeColor = Color.OrangeRed;
                label1.Text = "on";
            }
            else
            {
                label1.ForeColor = Color.Silver;
                label1.Text = "off";
            }
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
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Special character detection: \"" + textBoxPassword.Text.Substring(textBoxPassword.Text.Length - 1, 1) + "\"", this);
                textBoxPassword.Text = "";
            }
            password = Encoding.ASCII.GetBytes(textBoxPassword.Text);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //kiểm tra độ dài
            if (password == null || password.Length < 8)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Number of chars must be more than EIGHT chars", this);
                textBoxPassword.Select();
                return;
            }
            Log();
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

        //"button Close"
        #region
        private void labelClose_MouseEnter(object sender, EventArgs e)
        {
            labelClose.BackColor = Color.Red;
        }
        private void labelClose_MouseLeave(object sender, EventArgs e)
        {
            labelClose.BackColor = panel1.BackColor;
        }
        private void labelClose_Click(object sender, EventArgs e)
        {
            returner = false;
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
        }
        #endregion


    }
}
