using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
	public partial class FormAdd : Form
	{
        private const string leftCode = "!2||-@#(";
        private const string rightCode = ")#@-||2!";
        private const string nameCode = "!2||-@#x";//dùng để nhận biết TreeRoot
        private const string lockCode = "!2||-@#l";
        private const string nodesCode = "!2||-@#ns";
        private const string pathCode = "!2||-@#p";//được sử dụng để phân tách giữa Content với Path trong một TreeNode.Name khi TreeNode.Name được unlock

        internal bool button = false;// false = nhấn nút Cancel, true = nhấn nút Ok
		private TreeNode root;
        private string nameOfNode = "";

        public FormAdd(Point location, TreeNode root)
		{
			InitializeComponent();
			Left = location.X - 108;
			Top = location.Y - 14;
			this.root = root;
			ShowDialog();
		}

		public FormAdd(Point location, TreeNode root, string nameOfNode)
		{
			InitializeComponent();
			Left = location.X - 108;
			Top = location.Y - 14;
			Text = "Rename: " + nameOfNode;
            this.nameOfNode = nameOfNode;
            this.root = root;
			textBox1.Text = nameOfNode;
			textBox1.SelectAll();
			ShowDialog();
		}
        private bool CheckSpecialCharacter(string specialCharacter)
        {
            if (!textBox1.Text.Contains(specialCharacter))
                return false;
            new FormNotification("Not allowed use: \n\"" + specialCharacter + "\"", false);
            //bôi đen đoạn chuỗi đặc biệt đó
            textBox1.SelectionStart = textBox1.Text.IndexOf(specialCharacter);
            textBox1.SelectionLength = specialCharacter.Length;
            //click vào control textBoxContent
            textBox1.ScrollToCaret();
            textBox1.Select();
            return true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
		{
			#region Kiểm tra các lỗi cú pháp.
			if (textBox1.Text.Replace(" ", "") == "")
			{
                new FormNotification("Empty name!", false);
                textBox1.Text = "";
                textBox1.Select();
				return;
			}
			foreach (TreeNode i in root.Nodes)
			{
                if (textBox1.Text == i.Text && nameOfNode != textBox1.Text)
                {
                    new FormNotification("Namesake!", false);
                    textBox1.SelectAll();
                    textBox1.Select();
                    return;
                }
            }
            if (CheckSpecialCharacter(nodesCode) || CheckSpecialCharacter(pathCode) || CheckSpecialCharacter(leftCode) || CheckSpecialCharacter(lockCode) || CheckSpecialCharacter(nameCode) || CheckSpecialCharacter(rightCode))
                return;
            #endregion
            Text = textBox1.Text;
			button = true;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Get date") textBox1.Text = DateTime.Now.ToShortDateString();
            if (comboBox1.SelectedItem.ToString() == "Get time") textBox1.Text = DateTime.Now.ToLongTimeString();
            if (comboBox1.SelectedItem.ToString() == "Get date time") textBox1.Text = DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToLongDateString();
            if (comboBox1.SelectedItem.ToString() == "Get day of week") textBox1.Text = DateTime.Now.DayOfWeek.ToString();
            if (comboBox1.SelectedItem.ToString() == "Revert") textBox1.Text = nameOfNode;
            textBox1.Select();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if(textBox1.TextLength > 0) Clipboard.SetText(textBox1.Text);
            if (e.Control && e.KeyCode == Keys.A)
                textBox1.SelectAll();
        }
    }
}
