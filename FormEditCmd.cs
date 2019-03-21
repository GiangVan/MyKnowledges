using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace YourExperience
{
    public partial class FormEditCmd : Form
    {
        Point moveFormWithMouse;
        private string path = Application.StartupPath + "\\Other\\";
        private Hashtable hashtableCMD;
        public delegate void d_Void();
        private d_Void ShowMain;
        public delegate void D_Void();
        private D_Void UpdateDataSource_comboBoxSearch;
        List<string> searchHistory;

        public FormEditCmd(List<string> searchHistory, Hashtable hashtableCMD, d_Void ShowMain, D_Void UpdateDataSource_comboBoxSearch)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            InitializeComponent();

            this.UpdateDataSource_comboBoxSearch = UpdateDataSource_comboBoxSearch;
            this.ShowMain = ShowMain;
            this.hashtableCMD = hashtableCMD;
            this.searchHistory = searchHistory;
            ReadFile(hashtableCMD);
        }

        private void ReadFile(Hashtable hashtableCMD)
        {
            string file_name = path + "CommandLines.txt";
            if (!File.Exists(file_name))
            {

                hashtableCMD.Add("opencurrentfolder", Application.StartupPath);
                flowLayoutPanel1.Controls.Add(PanelCell.Get("opencurrentfolder", Application.StartupPath, hashtableCMD));

                hashtableCMD.Add("history", Environment.GetFolderPath(Environment.SpecialFolder.History));
                flowLayoutPanel1.Controls.Add(PanelCell.Get("history", Environment.GetFolderPath(Environment.SpecialFolder.History), hashtableCMD));

                hashtableCMD.Add("documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                flowLayoutPanel1.Controls.Add(PanelCell.Get("documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), hashtableCMD));

                hashtableCMD.Add("facebook", @"https://www.facebook.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("facebook", @"https://www.facebook.com/", hashtableCMD));

                hashtableCMD.Add("youtube", @"https://www.youtube.com/results?search_query=");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("youtube", @"https://www.youtube.com/results?search_query=", hashtableCMD));

                hashtableCMD.Add("translate", @"https://translate.google.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("translate", @"https://translate.google.com/", hashtableCMD));

                hashtableCMD.Add("maps", @"https://www.google.com/maps/search/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("maps", @"https://www.google.com/maps/search/", hashtableCMD));

                hashtableCMD.Add("drive", @"https://drive.google.com/drive/u/0/my-drive");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("drive", @"https://drive.google.com/drive/u/0/my-drive", hashtableCMD));

                hashtableCMD.Add("mediafire", @"https://www.mediafire.com/login/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("mediafire", @"https://www.mediafire.com/login/", hashtableCMD));

                hashtableCMD.Add("email", @"https://login.yahoo.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("yahoo", @"https://login.yahoo.com/", hashtableCMD));

                hashtableCMD.Add("gmail", @"https://accounts.google.com/signin/v2/identifier?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&service=mail&sacu=1&rip=1&flowName=GlifWebSignIn&flowEntry=ServiceLogin");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("gmail", @"https://accounts.google.com/signin/v2/identifier?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&service=mail&sacu=1&rip=1&flowName=GlifWebSignIn&flowEntry=ServiceLogin", hashtableCMD));

                hashtableCMD.Add("dropbox", @"https://www.dropbox.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("dropbox", @"https://www.dropbox.com/", hashtableCMD));

                hashtableCMD.Add("github", @"https://github.com/login");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("github", @"https://github.com/login", hashtableCMD));

                hashtableCMD.Add("flaticon", @"https://www.flaticon.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("flaticon", @"https://www.flaticon.com/", hashtableCMD));

                hashtableCMD.Add("cisco", @"https://www.netacad.com/login/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("cisco", @"https://www.netacad.com/login/", hashtableCMD));

                hashtableCMD.Add("ted", @"https://www.ted.com/talks");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("ted", @"https://www.ted.com/talks", hashtableCMD));

                hashtableCMD.Add("bbc", @"https://www.bbc.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("bbc", @"https://www.bbc.com/", hashtableCMD));

                hashtableCMD.Add("imagesearch", @"https://www.google.com.vn/search?authuser=0&q=");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("imagesearch", @"https://www.google.com.vn/search?authuser=0&q=", hashtableCMD));

                hashtableCMD.Add("duolingo", @"https://www.duolingo.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("duolingo", @"https://www.duolingo.com/", hashtableCMD));

                hashtableCMD.Add("stackoverflow", @"https://stackoverflow.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("stackoverflow", @"https://stackoverflow.com/", hashtableCMD));

                hashtableCMD.Add("tophonetics", @"https://tophonetics.com/");
                flowLayoutPanel1.Controls.Add(PanelCell.Get("tophonetics", @"https://tophonetics.com/", hashtableCMD));

                string savingData = "";
                foreach (DictionaryEntry item in hashtableCMD)
                {
                    savingData += item.Key + "\r\n" + item.Value + "\r\n\r\n\r\n";
                }
                try
                {
                    File.WriteAllText(file_name, savingData);
                }
                catch (Exception e)
                {
                    MessageBox.Show("File name: " + file_name + "\n\n" + e.ToString(), "Error writing form file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                return;
            }
            string[] data = null;
            ReadAgain:
            try
            {
                data = File.ReadAllLines(file_name);
            }
            catch (Exception e)
            {
                if (MessageBox.Show(e.ToString(), "Error reading form file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) goto ReadAgain;
                return;
            }
            if (data == null || data.Length == 0) return;
            try
            {
                for (int i = 0; i < data.Length; i += 4)
                {
                    hashtableCMD.Add(data[i], data[i + 1]);
                    flowLayoutPanel1.Controls.Add(PanelCell.Get(data[i], data[i + 1], hashtableCMD));
                }
            }
            catch
            {
                if (MessageBox.Show(file_name, "Error reading form file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry) goto ReadAgain;
                hashtableCMD.Clear();
                flowLayoutPanel1.Controls.Clear();
            }
        }

        //public void ShowForm()

        private void FormEditCmd_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowMain();
            buttonAdd.Select();//select để out-focus của panel nào đang nắm nó
            flowLayoutPanel1.Select();
            foreach (DictionaryEntry item in hashtableCMD)
            {
                if (!searchHistory.Contains((string)item.Key))
                {
                    searchHistory.Add((string)item.Key);
                }
            }
            UpdateDataSource_comboBoxSearch();
            //
            panel1.MouseMove -= panel1_MouseMove;
            Hide();
            e.Cancel = true;
        }

        public void Show(int left, int top, int width, int height)
        {
            Left = left + width / 2 - Width / 2;
            Top = top + height / 2 - Height / 2;
            new Animation(this);
            ShowDialog();
        }

        public void Save()
        {
            if(Directory.Exists(path)) Directory.CreateDirectory(path);
            //tiến hành lưu dữ liệu
            string savingData = "";
            string file_name = path + "CommandLines.txt";
            foreach (DictionaryEntry item in hashtableCMD)
            {
                savingData += item.Key + "\r\n" + item.Value + "\r\n\r\n\r\n";
            }
            try
            {
                File.WriteAllText(file_name, savingData);
            }
            catch (Exception E)
            {
                MessageBox.Show("File name: " + file_name + "\n\n" + E.ToString(), "Error writing form file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Panel panel = PanelCell.Get("", "", hashtableCMD);
            flowLayoutPanel1.Controls.Add(panel);
            foreach (Control item in panel.Controls)
            {
                item.Select();
                break;
            }
            panel.BackColor = Color.LightSeaGreen;
            flowLayoutPanel1.ScrollControlIntoView(panel);
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
