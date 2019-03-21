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
    public partial class FormFindAndReplace : Form
    {
        Point moveFormWithMouse;

        private RichTextBox textBoxContent;
        private Form FormMain;
        private int position_of_the_string_has_been_found = new int();
        private int number_of_units_found = new int();

        //Constructor
        #region
        public FormFindAndReplace(RichTextBox textBoxContent, Form FormMain)
        {
            InitializeComponent();

            textBoxReplace.Text = "";
            this.textBoxContent = textBoxContent;
            Owner = FormMain;
            this.FormMain = FormMain;
        }
        public FormFindAndReplace()
        {
            InitializeComponent();
        }
        #endregion

        //Show
        #region
        public void ShowForm()
        {
            textBoxFind.Text = textBoxContent.SelectedText;
            Enabled = true;
            Left = MousePosition.X - 20;
            Top = MousePosition.Y - 60;
            new Animation(this);
            Show();
        }
        #endregion

        //textBox1_TextChanged 
        //Check_Text()
        #region
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckText();
        }
        public void CheckText()
        {
            position_of_the_string_has_been_found = 0;
            labelPosition.Text = "Location: null";
            //trả về độ dài chuỗi tìm kiếm
            labelLength.Text = "Length: " + textBoxFind.TextLength;
            if (textBoxFind.TextLength == 0)
            {
                number_of_units_found = 0;
                labelFound.Text = "Had found: 0";
                return;
            }
            //trả về số lượng phần tử đã tìm được
            number_of_units_found = (textBoxContent.TextLength - textBoxContent.Text.Replace(textBoxFind.Text, "").Length) / textBoxFind.TextLength;
            labelFound.Text = "Had found: " + number_of_units_found;
            if (number_of_units_found == 0) return;            
        }

        #endregion

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (number_of_units_found == 0 || textBoxFind.TextLength == 0)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Not found!", this);
                return;
            } 
            //find
            int index = new int();
            if (position_of_the_string_has_been_found != 0) index = position_of_the_string_has_been_found + textBoxFind.TextLength;
            position_of_the_string_has_been_found = textBoxContent.Text.Substring(index).IndexOf(textBoxFind.Text) + index;
            if(position_of_the_string_has_been_found == index - 1)//nếu tìm đến thằng cuối cùng thì quay lại thằng đầu tiên
                position_of_the_string_has_been_found = textBoxContent.Text.IndexOf(textBoxFind.Text);
            labelPosition.Text = "Location: " + position_of_the_string_has_been_found;
            //bôi đen dữ liệu đã tìm thấy
            FormMain.Activate();
            textBoxContent.Select();
            textBoxContent.SelectionStart = position_of_the_string_has_been_found;
            textBoxContent.SelectionLength = textBoxFind.TextLength;
            textBoxContent.ScrollToCaret();
        }

        private void FormFindAndReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            Enabled = false;
            textBoxFind.Text = "";
            textBoxReplace.Text = "";
            Width = 361;
            Height = 219;
            Hide();
            e.Cancel = true;
        }

        public void SetEnabledForButtonReplace(bool value)
        {
            buttonReplace.Enabled = value;
            buttonReplaceAll.Enabled = value;
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            if (number_of_units_found == 0 || textBoxFind.TextLength == 0)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Not found!", this);
                return;
            }
            //find
            int index = position_of_the_string_has_been_found;
            if (position_of_the_string_has_been_found == 0) index = textBoxContent.Text.IndexOf(textBoxFind.Text);
            //bôi đen dữ liệu đã tìm thấy
            FormMain.Activate();
            textBoxContent.Select();
            textBoxContent.SelectionStart = index;
            textBoxContent.SelectionLength = textBoxFind.TextLength;
            //replace
            
            if(textBoxReplace.TextLength < 1)
            {
                textBoxContent.Text = textBoxContent.Text.Remove(index, textBoxFind.TextLength);
                textBoxContent.SelectionStart = index;
                textBoxContent.ScrollToCaret();
            }
            else
            {
                string str = Clipboard.GetText();
                Clipboard.SetText(textBoxReplace.Text);
                SendKeys.SendWait("^V");
                Clipboard.SetText(str);
            }
            //find next
            position_of_the_string_has_been_found = index + textBoxReplace.TextLength + textBoxContent.Text.Substring(textBoxContent.SelectionStart).IndexOf(textBoxFind.Text);
            if (position_of_the_string_has_been_found == index + textBoxReplace.TextLength - 1) position_of_the_string_has_been_found = 0;
        }

        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            if (number_of_units_found == 0 || textBoxFind.TextLength == 0)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Not found!", this);
                return;
            }
            if(MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                textBoxContent.Text = textBoxContent.Text.Replace(textBoxFind.Text, textBoxReplace.Text);
                FormMain.Activate();
                textBoxContent.Select();
                textBoxContent.SelectionLength = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormFindAndReplace_Deactivate(object sender, EventArgs e)
        {
            //if(!FormMain.Focused) MessageBox.Show("");
        }

        //di chuyển form theo chuột
        #region
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            panel2.MouseMove += panel2_MouseMove;
            moveFormWithMouse = new Point(MousePosition.X - Location.X, MousePosition.Y - Location.Y);
        }
        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            panel2.MouseMove -= panel2_MouseMove;
        }
        private void panel2_MouseMove(object sender, MouseEventArgs e)
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
            labelClose.BackColor = Color.DimGray;
        }
        private void labelClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}
