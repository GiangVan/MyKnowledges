using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormLoading : Form
    {
        Point moveFormWithMouse;
        int max, index;
        Thread thread;

        public FormLoading()
        {
            InitializeComponent();
        }

        public void Update(string text, int value)
        {
            if (IsHandleCreated)
            {
                if (checkBox1.Checked)
                {
                    textBox1.Invoke(new MethodInvoker(delegate ()
                    {
                        textBox1.Text += text + "\r\n";
                        textBox1.SelectionStart = textBox1.TextLength;
                        textBox1.ScrollToCaret();
                    }));
                }
                index += value;
                if (index > max)
                    return;
                int i = (int)(index / (float)max * 100);
                if ((i + "%") == labelPercent.Text)
                    return;
                labelPercent.Invoke(new MethodInvoker(delegate ()
                {
                    labelPercent.Text = i + "%";
                }));
                pictureBox1.Invoke(new MethodInvoker(delegate ()
                {
                    pictureBox1.Width = i * 5;
                }));
            }
        }
        public void Show(int max, string title)
        {
            Text = title;
            this.max = max;
            index = 0;
            textBox1.Text = "";
            labelPercent.Text = "0%";
            pictureBox1.Width = 0;
            thread = new Thread(StartThread);
            thread.Name = "FormLoading";
            thread.Start();
        }

        void StartThread()
        {
            //MessageBox.Show(MdiParent.Name);
            ShowDialog();
        }

        public void End()
        {
            while (!IsHandleCreated) ;//đợi form handle được tạo ra
            Invoke(new MethodInvoker(delegate ()
            {
                Hide();
                panel1.MouseMove -= panel1_MouseMove;
            }));
            while (IsHandleCreated);//đợi form ẩn đi
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Height = 460;
                textBox1.Visible = true;
            }
            else
            {
                Height = 110;
                textBox1.Visible = false;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(moveFormWithMouse == Location))
            {
                Location = new Point(MousePosition.X - moveFormWithMouse.X, MousePosition.Y - moveFormWithMouse.Y);
            }
        }
        #endregion
    }
}
