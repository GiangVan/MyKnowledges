using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormFindAndReplace : Form
    {
        private TextBox textBoxContent;
        private Form FormMain;
        private int position_of_the_string_has_been_found = new int();
        private int number_of_units_found = new int();

        //Constructor
        #region
        public FormFindAndReplace(TextBox textBoxContent, Form FormMain)
        {
            InitializeComponent();

            textBoxReplace.Text = "";
            this.textBoxContent = textBoxContent;
            this.FormMain = FormMain;
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
            if (textBoxFind.TextLength == 0) return;
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
                new FormNotification("Not found!", true);
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
                new FormNotification("Not found!", true);
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
            string str = Clipboard.GetText();
            Clipboard.SetText(textBoxReplace.Text);
            SendKeys.SendWait("^V");
            Clipboard.SetText(str);
            //find next
            position_of_the_string_has_been_found = index + textBoxReplace.TextLength + textBoxContent.Text.Substring(textBoxContent.SelectionStart).IndexOf(textBoxFind.Text);
            if (position_of_the_string_has_been_found == index + textBoxReplace.TextLength - 1) position_of_the_string_has_been_found = 0;
        }

        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            if (number_of_units_found == 0 || textBoxFind.TextLength == 0)
            {
                new FormNotification("Not found!", true);
                return;
            }
            if(MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                textBoxContent.Text = textBoxContent.Text.Replace(textBoxFind.Text, textBoxReplace.Text);
                FormMain.Activate();
                textBoxContent.Select();
                textBoxContent.SelectionLength = 0;
            }
        }
    }
}
