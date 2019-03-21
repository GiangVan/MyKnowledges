using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YourExperience.Other_Classes;
using YourExperience.OtherClasses;

namespace YourExperience
{
    public partial class FormSetting : Form
    {
        Point moveFormWithMouse;
        Color color = Color.LightBlue;
        public delegate void void_int(int x);
        public void_int ChangeTimerAutoSave_FormMain;

        public FormSetting()
        {
            InitializeComponent();
        }

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
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
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

        //rê chuột vào các checkbox
        #region
        private void checkBox_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = color;
        }
        private void checkBox_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = groupBoxCheckBox.BackColor;
        }
        #endregion

        //show
        #region
        public void Show(Form form)
        {
            Left = form.Left + form.Width / 2 - Width / 2;
            Top = form.Top + form.Height / 2 - Height / 2;
            new Animation(this);
            ShowDialog();
        }
        #endregion

        //sự kiện làm mờ các checkBox con khi checkBox cha Unchecked
        #region
        private void Event_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                if (sender.Equals(checkBox__Start_with_windows))
                {
                    AnimationCheck(checkBox__Automatically_minimize_when_start);
                }
                else if (sender.Equals(checkBox__Automatically_open_file_when_this_application_start))
                {
                    AnimationCheck(radioButton__Open_the_most_recent_active_file);
                    AnimationCheck(radioButton__Open_my_chosen_file);
                }
            }
            else
            {
                if (sender.Equals(checkBox__Start_with_windows))
                {
                    AnimationUnCheck(checkBox__Automatically_minimize_when_start);
                }
                else if (sender.Equals(checkBox__Automatically_open_file_when_this_application_start))
                {
                    AnimationUnCheck(radioButton__Open_the_most_recent_active_file);
                    AnimationUnCheck(radioButton__Open_my_chosen_file);
                }
            }
        }
        void AnimationCheck(Control item)
        {
            //bỏ gạch tên
            item.Font = new Font(item.Font, FontStyle.Bold);
            item.Enabled = true;
        }
        void AnimationUnCheck(Control item)
        {
            //gạch tên
            item.Font = new Font(item.Font, FontStyle.Strikeout);
            item.Enabled = false;
        }
        #endregion

        //trackBarAutoSave
        #region
        private void trackBarAutoSave_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar__AutoSave.Value != 0)
            {
                labelAutoSave.Text = "Saving per " + trackBar__AutoSave.Value + " minutes";
            }
            else
            {
                labelAutoSave.Text = "do not save";
            }
            ChangeTimerAutoSave_FormMain(trackBar__AutoSave.Value);
        }
        #endregion

        //button font
        #region
        private void button_font_Click(object sender, EventArgs e)
        {
            ((Button)sender).Font = Dialogs.ShowFontDialog(((Button)sender).Font);
        }
        #endregion

        //radioButton__Open_my_chosen_file
        #region
        private void radioButton__Open_my_chosen_file_CheckedChanged(object sender, MouseEventArgs e)
        {
            string str = Dialogs.ShowOpenFileDialog("Open KnowledgesTree File", "Supported Files (*.Ktree) | *.Ktree", new string[] { "Ktree" });
            if (string.IsNullOrEmpty(str) && !radioButton__Open_my_chosen_file.Checked)
            {
                radioButton__Open_the_most_recent_active_file.Checked = true;
            }
            else
            {
                SystemFile.Save(str, 2);
                radioButton__Open_my_chosen_file.Checked = true;
            }
        }
        #endregion
    }
}
