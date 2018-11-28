using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormNotification : Form
    {
        public FormNotification(string text, bool TopMost)
        {
            InitializeComponent();
            this.TopMost = TopMost;
            label1.Text = text;
            try
            {
                System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(@"C:\Windows\media\Windows Foreground.wav");
                soundPlayer.Play();
            }
            catch { }
            
            ShowDialog();
        }
    }
}
