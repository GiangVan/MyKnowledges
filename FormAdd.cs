using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YourExperience.OtherClasses;

namespace YourExperience
{
	public partial class FormAdd : Form
	{
        Point moveFormWithMouse;
        string nameOfNode;
        string returner;

        public FormAdd()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
		{
			#region Kiểm tra các lỗi cú pháp.
			if (textBox1.Text.Replace(" ", "") == "")
			{
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Empty name!", this);
                textBox1.Text = "";
                textBox1.Select();
				return;
			}
            #endregion
            returner = textBox1.Text;
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
		}

        //show
        #region
        //chỉnh sửa node cũ
        public string Show(string name, Color color)
        {
            if(name != null && name.Length > 0)
                nameOfNode = name;
            else
                nameOfNode = "";
            buttonColor.BackColor = color;
            textBox1.Text = name;
            Left = MousePosition.X - 15;
            Top = MousePosition.Y - 7;
            textBox1.SelectAll();
            textBox1.Select();
            comboBox1.SelectedIndex = -1;
            new Animation(this);
            ShowDialog();
            return returner;
        }
        //tạo mới node
        new public string Show()
        {
            nameOfNode = "";
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            buttonColor.BackColor = Color.DimGray;
            Left = MousePosition.X - 15;
            Top = MousePosition.Y - 7;
            textBox1.SelectAll();
            textBox1.Select();
            new Animation(this);
            ShowDialog();
            return returner;
        }
        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
		{
            returner = null;
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
		}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1) textBox1.Text = DateTime.Now.ToShortDateString();
            if (comboBox1.SelectedIndex == 0) textBox1.Text = DateTime.Now.ToLongTimeString();
            if (comboBox1.SelectedIndex == 2) textBox1.Text = DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
            if (comboBox1.SelectedIndex == 3) textBox1.Text = DateTime.Now.DayOfWeek.ToString();
            if (comboBox1.SelectedIndex == 4 && !string.IsNullOrEmpty(nameOfNode)) textBox1.Text = nameOfNode;
            textBox1.Select();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.D2) textBox1.Text = DateTime.Now.ToShortDateString();
                if (e.KeyCode == Keys.D1) textBox1.Text = DateTime.Now.ToLongTimeString();
                if (e.KeyCode == Keys.D3) textBox1.Text = DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                if (e.KeyCode == Keys.D4) textBox1.Text = DateTime.Now.DayOfWeek.ToString();
                if (e.KeyCode == Keys.D5 && !string.IsNullOrEmpty(nameOfNode)) textBox1.Text = nameOfNode;
                textBox1.Focus();
                textBox1.Select(textBox1.TextLength, 0);
                if (e.KeyCode == Keys.A) textBox1.SelectAll();
            }
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

        private void buttonColor_Click(object sender, EventArgs e)
        {
            object obj = Dialogs.ShowColorDialog(buttonColor.BackColor, true);
            if(obj != null)
            {
                buttonColor.BackColor = (Color)obj;
            }
        }
    }
}
