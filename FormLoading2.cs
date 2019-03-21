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
    public partial class FormLoading2 : Form
    {
        Thread thread;
        byte index = 0;

        public FormLoading2()
        {
            InitializeComponent();
        }

        public void Show(Form form)
        {
            StartPosition = FormStartPosition.Manual;
            Left = form.Left + form.Width / 2 - Width / 2;
            Top = form.Top + form.Height / 2 - Height / 2;

            thread = new Thread(StartThread);
            thread.Name = "FormLoading2";
            thread.Start();
        }

        public new void Show()
        {
            StartPosition = FormStartPosition.CenterScreen;

            thread = new Thread(StartThread);
            thread.Name = "FormLoading2";
            thread.Start();
        }

        void StartThread()
        {
            timer1.Enabled = true;
            ShowDialog();
        }

        public void End()
        {
            while (!IsHandleCreated) ;//đợi form handle được tạo ra
            Invoke(new MethodInvoker(delegate ()
            {
                timer1.Enabled = false;
                Hide();
            }));
            while (IsHandleCreated) ;//đợi form ẩn đi
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (index < 3)
            {
                label1.Text += ".";
                index++;
            }
            else
            {
                label1.Text = "Loading";
                index = 0;
            }
        }
    }
}
