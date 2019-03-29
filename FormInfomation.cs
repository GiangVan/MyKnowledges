using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormInfomation : Form
    {
        Point moveFormWithMouse;

        public FormInfomation()
        {
            InitializeComponent();
            labelVersion.Text = "v" + Application.ProductVersion + " beta";
        }

        public void Show(Form form)
        {
            Left = form.Left + form.Width / 2 - Width / 2;
            Top = form.Top + form.Height / 2 - Height / 2;
            new Animation(this);
            ShowDialog();
        }

        //"button Close"
        #region
        private void labelClose_Click(object sender, EventArgs e)
        {
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

        private void FormInfomation_Load(object sender, EventArgs e)
        {

        }
    }
}
