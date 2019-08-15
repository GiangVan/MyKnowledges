using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using YourExperience.OtherClasses;
using YourExperience.Other_Classes;
using System.Threading;

namespace YourExperience
{
    public partial class FormMain : Form
    {

        //khai báo
        #region
        //file hiện hành đang thao tác
        string path = null;
        //dùng để nhận biết xem đã click vào một node hay chưa
        bool clickToTreeNode;
        //
        readonly Color cornflowerBlue = Color.SkyBlue;
        //Command lines
        private Hashtable hashtableCMD = new Hashtable();
        private FormEditCmd formEditCmd;
        private AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
        //
        private Random random = new Random();
        //nhận biết lần click chuột đầu tiên vào comboBoxSearch
        private bool firstClickToComboBoxSearch = true;
        //các khai báo về treeView
        TreeNode copyNode = null;
        TreeNode cutNode = null;
        //các khai báo về panelMid
        private bool clicked = new bool();
        private int left_of_mouse_and_panelMid = new int();
        private int oldLeft_of_panelMid2;
        private int oldLeft_of_panelMid;
        //các khai báo về textBoxContent
        private int index_text = new int();//lưu vị trí SelectionStart
        private int length_text = new int();//lưu độ dài chuỗi đã bôi đen
        //
        FormFindAndReplace FormFindAndReplace; //sử dụng thuộc tính Enabed để nhận biết form này có đang showing hay không
        //
        readonly string File_SearchHistory = Application.StartupPath + "\\SearchHistory.txt";
        //lưu kích thước của form trước khi thức hiện minimaze
        int oldWidth;
        int oldHeight;
        int oldLeft_panelMid;
        //
        Panel[] customColorsBox;
        //
        List<string> searchHistory = new List<string>();
        #endregion

        //constructor
        #region constructor
        public FormMain(string path)
        {
            this.path = path;
            //Process.Start(Directory.GetCurrentDirectory());
            InitializeComponent();
            WindowsForm.Setting.ChangeTimerAutoSave_FormMain = ChangeTimerAutoSave;
            WindowsForm.Loading2.Show();
            WindowsForm.Add.Owner = this;
            WindowsForm.Notification.Owner = this;
            WindowsForm.Question.Owner = this;
            WindowsForm.Password.Owner = this;
            WindowsForm.Log.Owner = this;
            WindowsForm.Infomation.Owner = this;
            //nạp dữ liệu
            ReadSettingFile();
        }
        private void FormMain_Shown(object sender, EventArgs e)
        {
            //Command Lines
            formEditCmd = new FormEditCmd(searchHistory, hashtableCMD, Activate, UpdateDataSource_comboBoxSearch);
            //
            UnselectNode.SetValues(treeView, textBoxContent);
            //khởi tạo FormFindAndReplace
            FormFindAndReplace = new FormFindAndReplace(textBoxContent, this);

            //tự động mở file khi mở chương trình
            if(path == null && WindowsForm.Setting.checkBox__Automatically_open_file_when_this_application_start.Checked)
            {
                path = SystemFile.Get(2);
            }
            //nạp network node
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    NetworkNodes.Create(treeView, path);
                    textBoxContent.Select();
                }
                Activate();
            }
            catch (Exception ex)
            {
                TopMost = true;
                Activate(); TopMost = false;
                WindowsForm.Loading2.End();
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "ERROR", ex, null);
                Enabled = true;
                return;
            }
            WindowsForm.Loading2.End();
            Enabled = true;
            trackBarZoomFactor.Select();
        }
        #endregion

        //File Setting
        #region
        private void ReadSettingFile()
        {
            //lưu kích thước form 
            #region
            try
            {
                if (DataFile.WindowState == "Maximized") WindowState = FormWindowState.Maximized;
                if (DataFile.Width > 300 && DataFile.Width < Screen.PrimaryScreen.Bounds.Width + 16) Width = DataFile.Width;
                if (DataFile.Height > 110 && DataFile.Height < Screen.PrimaryScreen.Bounds.Height - 24) Height = DataFile.Height;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            oldHeight = Height;
            oldWidth = Width;
            #endregion
            //nạp font chữ cho textBoxContent
            #region
            textBoxContent.Font = WindowsForm.Setting.button__TextBox.Font;
            #endregion
            //nạp font chữ cho treeView
            #region
            treeView.Font = WindowsForm.Setting.button__TreeView.Font;
            #endregion
            //panelMid
            #region
            try
            {
                if (DataFile.LeftOfpanelMid > 34 && DataFile.LeftOfpanelMid < Width - 34)
                {
                    panel_treeView.Left -= panelMid.Left - DataFile.LeftOfpanelMid;
                    panel_treeView.Width += panelMid.Left - DataFile.LeftOfpanelMid;
                    panelMain.Width -= panelMid.Left - DataFile.LeftOfpanelMid;
                    oldLeft_of_panelMid = panelMid.Left;
                    panelMid.Left = DataFile.LeftOfpanelMid;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            oldLeft_panelMid = panelMid.Left;
            #endregion
            //nạp lịch sử tìm kiếm
            #region
            if (File.Exists(File_SearchHistory))
            {
                try
                {
                    string[] str = File.ReadAllLines(File_SearchHistory);
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (!hashtableCMD.ContainsKey(str[i]))
                            searchHistory.Add(str[i]);
                    }
                    foreach (DictionaryEntry item in hashtableCMD)
                    {
                        if (!searchHistory.Contains((string)item.Key))
                        {
                            searchHistory.Add((string)item.Key);
                        }
                    }
                    foreach (string item in searchHistory)
                    {
                        autoCompleteStringCollection.Add(item);
                    }
                    comboBoxSearch.Enabled = false;
                    comboBoxSearch.DataSource = null;
                    comboBoxSearch.DataSource = autoCompleteStringCollection;
                    comboBoxSearch.SelectedIndex = -1;
                    comboBoxSearch.Enabled = true;
                    comboBoxSearch.Text = " Search on Google  or  Commands Line";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion
            //nạBp datas cho panelEditingTool
            #region
            //lấy màu
            Color[] color = new Color[16];
            Dialogs.colorDialog.CustomColors = Dialogs.colorDialog.CustomColors;
            //tạo mảng màu để nạp cho các cells màu
            if (Dialogs.colorDialog.CustomColors != null && Dialogs.colorDialog.CustomColors.Length > 0)
            {
                for (int i = 0; i < Dialogs.colorDialog.CustomColors.Length; i++)
                {
                    color[i] = Color.FromArgb(Dialogs.colorDialog.CustomColors[i]);
                    color[i] = Color.FromArgb(color[i].B, color[i].G, color[i].R);
                }
            }
            //tạo các cells màu
            int x;
            customColorsBox = new Panel[16];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    x = i * 8 + j;
                    customColorsBox[x] = new Panel();
                    panelEditingTool.Controls.Add(customColorsBox[x]);
                    customColorsBox[x].Click += new EventHandler(panelCell_Color_Click);
                    customColorsBox[x].MouseEnter += new EventHandler(panelCell_Color_Enter);
                    customColorsBox[x].MouseLeave += new EventHandler(panelCell_Color_Leave);
                    if (color == null)
                        customColorsBox[x].BackColor = Color.White;
                    else
                    {
                        customColorsBox[x].BackColor = color[x];
                    }
                    customColorsBox[x].Width = 22;
                    customColorsBox[x].Height = 22;
                    customColorsBox[x].Left = j * 27 + buttonColor_textBoxContent.Left + 30;
                    customColorsBox[x].Top = i * 25 + buttonColor_textBoxContent.Top + 1;
                    customColorsBox[x].BorderStyle = BorderStyle.Fixed3D;
                }
            }
            Dialogs.customColorsBox = customColorsBox;

            string[] listFonts = new string[FontFamily.Families.Length - 1];
            for (int i = 0; i < FontFamily.Families.Length - 1; i++)
            {
                listFonts[i] = FontFamily.Families[i + 1].Name;
            }
            comboBoxFont_textBoxContent.DataSource = listFonts;
            comboBoxFont_textBoxContent.SelectedItem = textBoxContent.Font.Name;
            comboBoxFontSize_textBoxContent.Text = textBoxContent.Font.Size.ToString();
            numericUpDownFont.Value = (decimal)textBoxContent.Font.Size;
            #endregion
            //tự động minisize khi khỏi động cùng windows
            #region
            if (WindowsForm.Setting.checkBox__Automatically_minimize_when_start.Checked && Directory.GetCurrentDirectory().ToLower() == @"c:\windows\system32")
            {
                WindowState = FormWindowState.Minimized;
            }
                
            #endregion
        }
        void SaveSettingFile()
        {
            DataFile.WindowState = WindowState.ToString();
            if (WindowState == FormWindowState.Minimized)
            {
                DataFile.Width = oldWidth;
                DataFile.Height = oldHeight;
                DataFile.LeftOfpanelMid = oldLeft_panelMid;
            }
            else
            {
                DataFile.Width = Width;
                DataFile.Height = Height;
                DataFile.LeftOfpanelMid = panelMid.Left;
            }

            DataFile.SaveAll();
        }
        #endregion

        //đóng form và lưu treeView data    
        #region FormMain_FormClosing
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!textBoxContent.ReadOnly) buttonSave_Click(null, null);
            // nếu có thay đổi dữ liệu
            if (NodesEditingHistory.Check() && (!string.IsNullOrEmpty(path) || treeView.GetNodeCount(true) > 0))
            {
                // nếu là file mở sẵn
                if (!string.IsNullOrEmpty(path))
                {
                    // hỏi người dùng có muốn lưu dữ liệu
                    if(WindowsForm.Question.Show("Save?", this) == DialogResult.Yes)
                    {
                        Visible = false;
                        NetworkNodes.SaveAll(treeView, path);
                    }
                    else
                    {
                        if (WindowsForm.Question.Show("Are you sure?", this) == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            // thoát chương trình và không lưu
                            Application.ExitThread();
                        }
                    }
                }
                // nếu là file mới tạo
                //nếu người dùng không muốn lưu current file
                else if (WindowsForm.Question.Show("Save the current file?", this) == DialogResult.No)
                {
                    if (WindowsForm.Question.Show("Are you sure?", this) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                //nếu người dùng muốn lưu current file
                else
                {
                    ChoosePathToSave();
                    //nếu user muốn save file và đã chọn xong path
                    if (!string.IsNullOrEmpty(path))
                    {
                        NetworkNodes.SaveAll(treeView, path);
                    }
                    //nếu user muốn save file nhưng lại huỷ việc chọn path
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else
                Visible = false;

            timerAutoSave.Enabled = false;
            FormFindAndReplace.Dispose();
            //lưu file Ktree
            if (!WindowsForm.Setting.radioButton__Open_my_chosen_file.Checked && !string.IsNullOrEmpty(path) && WindowsForm.Setting.checkBox__Automatically_open_file_when_this_application_start.Checked)
            {
                SystemFile.Save(path, 2);
            }
            //lưu commands line
            formEditCmd.Save();
            //Lưu các thiết lập
            SaveSettingFile();
            //lưu lịch sử tìm kiếm
            string[] array_str = new string[searchHistory.Count];
            searchHistory.CopyTo(array_str);
        WriteAgain:
            try { File.WriteAllLines(File_SearchHistory, array_str); }
            catch (Exception ex)
            {
                if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Save search history failed!", ex, this) == DialogResult.Retry) goto WriteAgain;
            }

            Application.ExitThread();
        }
        #endregion

        //MenuStrip
        #region MenuStrip

        #endregion

        //các thao tác trên TreeNodes
        #region TreeNode

        //bắt sự kiện Click chuột vào một TreeNode
        #region treeView_NodeMouseClick
        //click vào treeView
        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (clickToTreeNode)
            {
                clickToTreeNode = false;
            }
            else
            {
                //click chuột phải vào treeview
                if (e.Button == MouseButtons.Right)
                {
                    treeView.SelectedNode = null;
                    //paste
                    if (copyNode == null && cutNode == null)
                    {
                        toolStripMenuItem10.Enabled = false;
                    }
                    else
                    {
                        toolStripMenuItem10.Enabled = true;
                    }
                    //hiện MenuStrip
                    contextMenuStrip_TreeView.Show(MousePosition);
                }
            }
        }
        //click vào node
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            clickToTreeNode = true;
            Cursor = Cursors.AppStarting;
            //nếu đang có TreeNode ở trạng thái edit content thì lưu TreNode đó lại
            if (!textBoxContent.ReadOnly)
            {
                SaveContent();
                UnselectNode.Start(treeView.SelectedNode, e.Node);
                Cursor = Cursors.Default;
                return;
            }
            //chạy tính năng auto lock the node
            AutoLockTheNodes(e.Node);
            //cập nhật các thông số cho Tag của node đã chọn trước đó
            UpdateTheTagOfSelectedNode();
            //hiện nội dung của node.
            ShowTreeNodeContent(e.Node);
            //hiện MenuStrip và set SelectedNode = node vừa click khi click chuột phải.
            if (e.Button == MouseButtons.Right)
            {
                //paste
                if (copyNode == null && cutNode == null)
                {
                    toolStripMenuItem17.Enabled = false;
                }
                else
                {
                    toolStripMenuItem17.Enabled = true;
                }
                //nếu đây là LockNode và đang ở trạng thái khoá
                if (((Tag_of_Node)e.Node.Tag).is_lockNode && !((Tag_of_Node)e.Node.Tag).unlocked)
                {
                    //unlock
                    locktoolStripMenuItem.Text = "Unlock                   (Enter)";
                    locktoolStripMenuItem.Image = Properties.Resources.padlock_unlock_icon__1_;
                    //date
                    Date_toolStripMenuItem.Text = "Was locked";
                }
                //ngược nếu đây không phải là LockNode hoặc đang ở trạng thái mở
                else
                {
                    //lock
                    locktoolStripMenuItem.Text = "Lock                       (Alt + L)";
                    locktoolStripMenuItem.Image = Properties.Resources.padlock_lock_icon__1_;
                    //date
                    Date_toolStripMenuItem.Text = "Added at " + ((Tag_of_Node)e.Node.Tag).date;
                }
                //nếu đây là LockNode và đang ở trạng thái mở
                if (((Tag_of_Node)e.Node.Tag).is_lockNode && ((Tag_of_Node)e.Node.Tag).unlocked)
                {
                    //change password
                    changePasswordToolStripMenuItem.Enabled = true;
                    //put back into normal
                    toolStripMenuItem11.Enabled = true;
                }
                //ngược lại nếu đây không phải là LockNode hoặc đang ở trạng thái đóng
                else
                {
                    //change password
                    changePasswordToolStripMenuItem.Enabled = false;
                    //put back into normal
                    toolStripMenuItem11.Enabled = false;
                }
                //hiện MenuStrip và sét SelectedNode = node vừa click
                treeView.SelectedNode = e.Node;
                contextMenuStrip_TreeNode.Show(MousePosition);
            }
            Cursor = Cursors.Default;
        }
        #endregion

        //các thao tác trên bàn phím
        #region
        private void treeView_KeyUp(object sender, KeyEventArgs e)
        {
            //hiện nội dung của node khi sử dụng các phím điều hướng
            if (!e.Control && treeView.SelectedNode != null && (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right))
            {
                //đóng tất cả node ngoài node đã click
                if (WindowsForm.Setting.checkBox__Automatically_lock_the_Nodes.Checked)
                {
                    foreach (TreeNode a_node in treeView.Nodes)
                    {
                        NetworkNodes.LockNodes(treeView.SelectedNode, a_node);
                    }
                }
                ShowTreeNodeContent(treeView.SelectedNode);
            }
        }
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView.SelectedNode == null)
            {
                //tạo mới một Node                                                          Ctrl + N
                if (e.KeyCode == Keys.N && e.Control && !e.Shift && !e.Alt)
                {
                    CreateTreeNode();
                }
                //paste                                                                     Ctrl + V
                if (e.KeyCode == Keys.V)
                {
                    toolStripMenuItem17_Click(null, null);
                }
                return;
            }
            //tạo mới một Node từ node cha của node đã chọn                             Ctrl + Shift + N
            if (e.KeyCode == Keys.N && e.Shift && e.Control && !e.Alt)
            {
                if (treeView.SelectedNode.Parent == null)
                    CreateTreeNode();
                else
                    CreateTreeNode(treeView.SelectedNode.Parent);
            }
            //Sử chỉ dụng Ctrl
            #region
            if (e.Control && !e.Shift && !e.Alt)
            {
                //copy                                                                      Ctrl + C
                if (e.KeyCode == Keys.C)
                {
                    copyToolStripMenuItem_Click(null, null);
                }
                //paste                                                                     Ctrl + V
                if (e.KeyCode == Keys.V)
                {
                    toolStripMenuItem17_Click(null, null);
                }
                //cut                                                                       Ctrl + X
                if (e.KeyCode == Keys.X)
                {
                    moveToolStripMenuItem_Click(null, null);
                }
                //di chuyển lên top                                                         Ctrl + Page Up
                if (e.KeyCode == Keys.PageUp)
                {
                    topToolStripMenuItem_Click(null, null);
                }
                //di chuyển xuống bot                                                       Ctrl + Page Down
                if (e.KeyCode == Keys.PageDown)
                {
                    botToolStripMenuItem_Click(null, null);
                }
                //di chuyển lên                                                             Ctrl + Up
                if (e.KeyCode == Keys.Up)
                {
                    upToolStripMenuItem_Click(null, null);
                }
                //di chuyển xuống                                                           Ctrl + Down
                if (e.KeyCode == Keys.Down)
                {
                    downToolStripMenuItem_Click(null, null);
                }
                //node đang ở trạng thái chỉ đọc
                if (textBoxContent.ReadOnly)
                {
                    //tạo mới một Node                                                          Ctrl + N
                    if (e.KeyCode == Keys.N)
                    {
                        CreateTreeNode(treeView.SelectedNode);
                    }
                    //sao chép tên của node vào clipboard                                       Ctrl + 1
                    if (e.KeyCode == Keys.D1)
                    {
                        if (treeView.SelectedNode.Text.Length > 0)
                        {
                            if (treeView.SelectedNode.Text.Length > 0)
                            {
                                Clipboard.SetText(treeView.SelectedNode.Text);
                            }
                        }
                    }
                    //sao chép nội dung của node vào clipboard                                  Ctrl + 2
                    if (e.KeyCode == Keys.D2)
                    {
                        if (treeView.SelectedNode.Name.Length > 0)
                        {
                            if (treeView.SelectedNode.Name.Length > 0)
                            {
                                Clipboard.SetText(treeView.SelectedNode.Name);
                            }
                        }
                    }
                    //sao chép toàn bộ                                                          Ctrl + 3
                    if (e.KeyCode == Keys.D3)
                    {
                        if (treeView.SelectedNode.Name.Length > 0 && treeView.SelectedNode.Text.Length > 0)
                        {
                            string str = treeView.SelectedNode.Text + "\r\n\r\n" + treeView.SelectedNode.Name;
                            if (str.Length > 0)
                            {
                                Clipboard.SetText(str);
                            }
                        }
                    }

                }
            }
            #endregion

            //không được sử dụng Shift, Ctrl và node phải đang ở trạng thái chỉ đọc
            #region
            else if (textBoxContent.ReadOnly && !e.Shift)
            {
                ////cập nhật các thông số cho Tag của node đã chọn trước đó khi nhấn các phím điều hướng
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    UpdateTheTagOfSelectedNode();
                }
                //chỉnh sửa một Tree node                                                   F3
                if (e.KeyCode == Keys.F3)
                {
                    OpenEditContent_of_SelectedNode();
                }
                //Rename                                                                    F2
                if (e.KeyCode == Keys.F2)
                {
                    RenameTheNode();
                }
                //Delete                                                                    Delete
                if (e.KeyCode == Keys.Delete)
                {
                    deleteToolStripMenuItem_Click(deleteToolStripMenuItem, null);
                }
                //Lock                                                                      Alt + L
                if (e.Alt && e.KeyCode == Keys.L && !(treeView.SelectedNode.Tag != null && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked && ((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode))
                {
                    LockOrUnlockTheNode();
                }
                //Unlock                                                                    Enter
                if (e.KeyCode == Keys.Enter && treeView.SelectedNode.Tag != null && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked && ((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode)
                {
                    LockOrUnlockTheNode();
                }
            }
            #endregion
        }
        #endregion

        //tạo mới một TreeNode
        #region newToolStripMenuItem_Click
        private void newToolStripMenuItem_Click(object sender, EventArgs e)//tạo mới một nút.
        {
            CreateTreeNode(treeView.SelectedNode);
        }
        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Parent == null)
                CreateTreeNode();
            else
                CreateTreeNode(treeView.SelectedNode.Parent);
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CreateTreeNode();
        }
        #endregion

        //sửa tên một TreeNode
        #region editToolStripMenuItem_Click
        private void editToolStripMenuItem_Click(object sender, EventArgs e)//sửa tên một nút.
        {
            RenameTheNode();
        }
        #endregion

        //sửa nội dung một TreeNode
        #region edit content of a TreeNode
        private void editContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEditContent_of_SelectedNode();
        }
        private bool SaveDataWhenEditingNode_done = new bool();
        private void CloseEditting()
        {
            if (SaveDataWhenEditingNode_done && WindowsForm.Setting.trackBar__AutoSave.Value > 0)
            {
                SaveTreeView();//lưu dữ liệu 
                SaveDataWhenEditingNode_done = false;
            }
            comboBoxSearch.Select();//không nên bỏ dòng này
            panelEditingTool.Visible = false;
            panel_textBoxContent.Height += 61;
            panel_textBoxContent.Top -= 61;
            panelHL1.Visible = false;
            panelHL2.Visible = false;
            panelHL3.Visible = false;
            textBoxContent.AcceptsTab = false;
            panelHL4.Visible = false;
            textBoxContent.ReadOnly = true;
            treeView.Select();
        }
        private void buttonCancel_Click_1(object sender, EventArgs e)
        {
            CloseEditting();
            ShowTreeNodeContent(treeView.SelectedNode);
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                //nếu node đang ở trạng thái editting
                if (!textBoxContent.ReadOnly && panelEditingTool.Visible)
                {
                    SaveContent();
                }
                //nếu đang ở trạng thái chỉ đọc
                else if (sender != null && sender.GetType() == treeView.GetType())
                {
                    UpdateTheTagOfSelectedNode();
                }
            }
        }

        #endregion

        //xoá một TreeNode
        #region deleteToolStripMenuItem_Click
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)//xoá một node.
        {
            //nếu node đang khoá thì ngăn xoá node
            if (((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked)
            {
                LockOrUnlockTheNode();
            }
            else if (WindowsForm.Question.Show("Are you sure?", this) == DialogResult.Yes)
            {
                //kiểm tra nếu trong selectedNode có tồn tại một node đang khoá thì return
                if (Find_a_LockedNode(treeView.SelectedNode)) return;
                treeView.SelectedNode.Remove();
                textBoxContent.Text = "";
                treeView.SelectedNode = null;
                NodesEditingHistory.Add(new TreeNode(), true);
            }
        }
        private bool Find_a_LockedNode(TreeNode node)
        {
            if (node.Nodes == null || node.Nodes.Count == 0) return false;
            foreach (TreeNode a_node in node.Nodes)
            {
                if (((Tag_of_Node)a_node.Tag).is_lockNode && !((Tag_of_Node)a_node.Tag).unlocked)
                {
                    WindowsForm.Notification.Show(MessageBoxButtons.OK, "You must unlock: " + a_node.Text, this);
                    treeView.SelectedNode = a_node;
                    return true;
                }
                else
                {
                    if (Find_a_LockedNode(a_node)) return true;
                }
            }
            return false;
        }
        #endregion

        //sao chép vào clipboard
        #region copy to clipboard
        //sao chép nội dung của node vào clipboard
        private void copyContentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Name.Length > 0)
            {
                if (treeView.SelectedNode.Name.Length > 0)
                {
                    Clipboard.SetText(textBoxContent.Text);
                    Clipboard.SetDataObject(textBoxContent.Rtf);
                }
            }
        }
        //sao chép tên của node vào clipboard
        private void copyNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Text.Length > 0)
            {
                if (treeView.SelectedNode.Text.Length > 0)
                {
                    Clipboard.SetText(treeView.SelectedNode.Text);
                }
            }
        }
        //sao chép toàn bộ
        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Name.Length > 0 && treeView.SelectedNode.Text.Length > 0)
            {
                string str = textBoxContent.Text + "\r\n\r\n" + treeView.SelectedNode.Name;
                if (str.Length > 0)
                {
                    Clipboard.SetText(str);
                }
            }
        }
        #endregion

        //di chuyển một TreeNode
        #region move a TreeNode
        //di chuyển TreeNode lên trên cùng
        #region onTopToolStripMenuItem_Click
        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Index == 0) return;
            TreeNode a = treeView.SelectedNode;
            TreeNode b = treeView.SelectedNode.Parent;
            treeView.SelectedNode.Remove();
            if(b == null)//nếu b là một treeView
                treeView.Nodes.Insert(0, a);
            else
                b.Nodes.Insert(0, a);
            treeView.SelectedNode = a;
            NodesEditingHistory.Add(treeView.SelectedNode, true);
        }
        #endregion
        //di chuyển TreeNode xuống cuối cùng
        #region botToolStripMenuItem_Click
        private void botToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Parent == null)
            {
                if (treeView.SelectedNode.Index == treeView.Nodes.Count - 1) 
                    return;
            }
            else if (treeView.SelectedNode.Index == treeView.SelectedNode.Parent.Nodes.Count - 1) return;
            TreeNode a = treeView.SelectedNode;
            int index;
            if (treeView.SelectedNode.Parent == null)
                index = treeView.Nodes.Count;
            else
                index = treeView.SelectedNode.Parent.Nodes.Count;
            TreeNode b = treeView.SelectedNode.Parent;
            treeView.SelectedNode.Remove();
            if (b == null)//nếu b là một treeView
                treeView.Nodes.Insert(index, a);
            else
                b.Nodes.Insert(index, a);
            treeView.SelectedNode = a;
            NodesEditingHistory.Add(treeView.SelectedNode, true);
        }
        #endregion
        //di chuyển TreeNode lên 1 đơn vị
        #region upToolStripMenuItem_Click
        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Index == 0) return;
            TreeNode a = treeView.SelectedNode;
            int index = treeView.SelectedNode.Index - 1;
            TreeNode b = treeView.SelectedNode.Parent;
            treeView.SelectedNode.Remove();
            if (b == null)//nếu b là một treeView
                treeView.Nodes.Insert(index, a);
            else
                b.Nodes.Insert(index, a);
            treeView.SelectedNode = a;
            NodesEditingHistory.Add(treeView.SelectedNode, true);
        }
        #endregion
        //di chuyển TreeNode xuống 1 đơn vị
        #region downToolStripMenuItem_Click
        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Parent == null)
            {
                if (treeView.SelectedNode.Index == treeView.Nodes.Count - 1)
                    return;
            }
            else if (treeView.SelectedNode.Index == treeView.SelectedNode.Parent.Nodes.Count - 1) return;
            TreeNode a = treeView.SelectedNode;
            int index = treeView.SelectedNode.Index + 1;
            TreeNode b = treeView.SelectedNode.Parent;
            treeView.SelectedNode.Remove();
            if (b == null)//nếu b là một treeView
                treeView.Nodes.Insert(index, a);
            else
                b.Nodes.Insert(index, a);
            treeView.SelectedNode = a;
            NodesEditingHistory.Add(treeView.SelectedNode, true);
        }
        #endregion
        #endregion

        //dán một TreeNode
        #region
        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && ((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked)
            {
                LockOrUnlockTheNode();
            }
            else
            {
                TreeNode node = null;
                if (cutNode != null)
                {
                    node = cutNode;
                    cutNode = null;
                    copyNode = null;
                }
                else if (copyNode != null)
                    node = copyNode;

                if (node != null)
                {
                    if (copyNode != null)
                    {
                        node = GetCopy_a_TreeNode(node);//tạo ra một bản sao node mới
                        copyNode = node;
                    }
                    if (treeView.SelectedNode != null)
                    {
                        treeView.SelectedNode.Nodes.Add(node);
                        treeView.SelectedNode.Expand();

                    }
                    else
                    {
                        treeView.Nodes.Add(node);
                    }
                    treeView.SelectedNode = node;
                    NodesEditingHistory.Add(treeView.SelectedNode, true);
                    ShowTreeNodeContent(treeView.SelectedNode);
                }
            }
        }
        #endregion

        //cắt một TreeNode
        #region moveToolStripMenuItem_Click
        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyNode != null) copyNode = null;
            cutNode = treeView.SelectedNode;
            NodesEditingHistory.Add(treeView.SelectedNode, true);
            treeView.SelectedNode.Remove();
        }
        #endregion

        //sao chép một TreeNode
        #region
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cutNode != null)
            {
                if (WindowsForm.Question.Show("Are you Sure!", this) == DialogResult.Yes)
                {
                    cutNode = null;
                    copyNode = treeView.SelectedNode;
                }
            }
            else
                copyNode = treeView.SelectedNode;
        }
        #endregion

        //xổ ra tất cả các TreeNode con của các TreeNode cha
        #region expandAllItemsToolStripMenuItem_Click
        private void expandAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.SelectedNode.ExpandAll();

        }
        #endregion

        //???xổ ra tất cả các TreeNode con của các TreeNode cha
        #region
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            treeView.SelectedNode.Toggle();
            treeView.SelectedNode.Toggle();
        }
        #endregion

        //Alignment
        #region
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            treeView.RightToLeft = RightToLeft.No;
            toolStripMenuItem8.Image = Properties.Resources._checked;
            toolStripMenuItem9.Image = null;
            treeView.ExpandAll();
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            treeView.RightToLeft = RightToLeft.Yes;
            toolStripMenuItem8.Image = null;
            toolStripMenuItem9.Image = Properties.Resources._checked;
            treeView.ExpandAll();
        }
        #endregion

        //lock / unlock
        #region
        //double click vào một node đang khoá thì mở form đăng nhập
        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(treeView.SelectedNode == null) return;
            clickToTreeNode = true;
            if (((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked) UnlockTheNode();
            else if (WindowsForm.Setting.checkBox__Automatically_open_editing_when_double_click_to_a_node.Checked && (treeView.SelectedNode.Nodes == null || treeView.SelectedNode.Nodes.Count == 0))
            {
                OpenEditContent_of_SelectedNode();
            }
        }
        //
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            LockOrUnlockTheNode();
        }
        private void LockOrUnlockTheNode()
        {
            UseWaitCursor = true;
            if (((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked)
                UnlockTheNode();
            else
                LockTheNode();
            UseWaitCursor = false;
        }
        void UnlockTheNode()
        {
            if (WindowsForm.Log.Show(treeView.SelectedNode, this))
            {
                ShowTreeNodeContent(treeView.SelectedNode);
                NodesEditingHistory.Add(treeView.SelectedNode, true);
                treeView.SelectedNode.Expand();
            }
        }
        void LockTheNode()
        {
            //nếu là một LockNode
            if (((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode)
            {
                SaveAndLockTheUnlockedTreeNode(treeView.SelectedNode);
                ShowTreeNodeContent(treeView.SelectedNode);
                NodesEditingHistory.Add(treeView.SelectedNode, true);
            }
            //nếu là một node bình thường
            else
            {
                //tạo một mật khẩu mới
                byte[] pass = WindowsForm.Password.Show();
                if (pass != null)
                {
                    WindowsForm.Loading2.Show(this);
                    try
                    {
                        //chuyển các TreeNodes con của TreeNode đó thành chuỗi		
                        ((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode = true;
                        ((Tag_of_Node)treeView.SelectedNode.Tag).password = AdvancedEncryptionStandard.Hash(pass);
                        NetworkNodes.SaveAndLockAUnlockedNode(treeView.SelectedNode);
                        //xoá các nodes con sau khi đã lưu các nodes đó thành chuỗi
                        treeView.SelectedNode.Nodes.Clear();
                        //hiển thị node ra textbox
                        ShowTreeNodeContent(treeView.SelectedNode);
                    }
                    finally
                    {
                        WindowsForm.Loading2.End();
                    }
                }
                NodesEditingHistory.Add(treeView.SelectedNode, true);
                #region
                //    text_nodes = "";
                //    TreeNodeToString(treeView.SelectedNode);
                //    text_nodes += rightCode;
                //    //tạo thư mục để lưu file (nếu chưa có)
                //    path = Application.StartupPath + "\\Data\\";
                //    if (!Directory.Exists(path))
                //        Directory.CreateDirectory(path);
                //    //tạo tên file
                //    string fileName = DateTime.Now.Ticks.ToString() + ".g";
                //    //tạo bytes dữ liệu của RIÊNG TreeNode đó
                //    treeView.SelectedNode.Name += dateCode + ((Tag_of_a_Node)treeView.SelectedNode.Tag).date + fontSizeCode + ((Tag_of_a_Node)treeView.SelectedNode.Tag).zoomFactor;
                //    bytes = GCoding.Encoding(Encoding.Unicode.GetBytes((passCode + treeView.SelectedNode.Name + nodesCode + text_nodes).ToCharArray()), pass);
                ////tiến hành lưu
                //WriteTheFile:
                //    try
                //    {
                //        File.WriteAllBytes(path + fileName, bytes);
                //    }
                //    catch (Exception E)
                //    {
                //        textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
                //        if (MessageBox.Show("Can't write the file: " + fileName + "\n" + E.Message, "ERROR", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                //            goto WriteTheFile;
                //        else
                //            return;
                //    }
                //    //cập nhật lại TreeNode và xoá các TreeNodes con của nó
                //    textBoxContent.Rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}}{\colortbl ;\red215\green19\blue34;}\uc1\pard\cf1\f0\fs20 Was locked}";
                //    treeView.SelectedNode.Name = fileName + lockCode;
                //    ((Tag_of_a_Node)treeView.SelectedNode.Tag).bytes = bytes;
                //    ((Tag_of_a_Node)treeView.SelectedNode.Tag).Clear();
                //    treeView.SelectedNode.Nodes.Clear();

                #endregion
            }
        }
        #region
        ////đệ quy
        //#region
        ////Lấy các TreeNodes và nối thành chuỗi string
        //private void TreeNodeToString(TreeNode treeRoot)
        //{
        //    if (treeRoot.Nodes == null || treeRoot.Nodes.Count == 0) return;
        //    foreach (TreeNode nodes in treeRoot.Nodes)
        //    {
        //        text_nodes += nodes.Text + nameCode + nodes.Name + dateCode + ((Tag_of_a_Node)nodes.Tag).date + fontSizeCode + ((Tag_of_a_Node)nodes.Tag).zoomFactor + leftCode ;
        //        TreeNodeToString(nodes);
        //        text_nodes += rightCode;
        //    }
        //}
        ////Lấy các TreeNodes từ chuỗi string
        //private int AddTreeNode(TreeNode treeRoot, int indexCurrent)
        //{
        //    int index = text_nodes.Substring(indexCurrent).IndexOf(leftCode) + indexCurrent;
        //    //tạo mới một TreeNode
        //    TreeNode treeNode = new TreeNode();
        //    string dataString2 = text_nodes.Substring(indexCurrent, index - indexCurrent);//đoạn string chứa dữ liệu của một TreeNode
        //    indexCurrent = dataString2.IndexOf(nameCode);
        //    treeNode.Text = dataString2.Substring(0, indexCurrent);//đoạn đầu của dataString2 chứa tên của TreeNode
        //    treeNode.Name = dataString2.Substring(indexCurrent + nameCode.Length);//đoạn còn lại của dataString2 chứa nội dung của TreeNode
        //    treeRoot.Nodes.Add(treeNode);
        //    if (treeNode.Name.Contains(lockCode) || treeNode.Name.Contains(RootCode))//nếu đây là một LockedNode hoặc RootNode (trường hợp copy Node từ hai chương trình)
        //        treeNode.Tag = new Tag_of_a_Node(null, null, 1);
        //    else
        //    {
        //        treeNode.Tag = new Tag_of_a_Node(treeNode.Name.Substring(treeNode.Name.LastIndexOf(dateCode) + dateCode.Length, treeNode.Name.LastIndexOf(fontSizeCode) - treeNode.Name.LastIndexOf(dateCode) - dateCode.Length), null, float.Parse(treeNode.Name.Substring(treeNode.Name.LastIndexOf(fontSizeCode) + fontSizeCode.Length)));
        //        treeNode.Name = treeNode.Name.Substring(0, treeNode.Name.LastIndexOf(dateCode));
        //    }
        //    //bắt đầu đệ quy
        //    while (true)
        //    {
        //        index += leftCode.Length;
        //        if (index == text_nodes.Length - 1 || text_nodes.Substring(index).IndexOf(rightCode) == 0) return index;
        //        index = AddTreeNode(treeNode, index);
        //    }
        //}
        //#endregion
        #endregion
        #endregion

        //change password
        #region
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] pass = WindowsForm.Password.Show();
            if (pass == null)
                return;//nếu người dùng huỷ việc tạo mật khẩu thì out
            WindowsForm.Loading2.Show(this);
            ((Tag_of_Node)treeView.SelectedNode.Tag).password = AdvancedEncryptionStandard.Hash(pass);
            NodesEditingHistory.Add(treeView.SelectedNode, true);
            WindowsForm.Loading2.End();
        }
        #endregion

        //put back into normal
        #region
        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            //xoá dữ liệu
            ((Tag_of_Node)treeView.SelectedNode.Tag).Clear();
            NodesEditingHistory.Add(treeView.SelectedNode, true);
        }
        #endregion

        //copy / add a item form Clipboard
        #region
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            WindowsForm.Loading2.Show(this);
            //Nạp các TreeNodes cho TreeNode đó
            try
            {
                TreeNode newNode = new TreeNode();
                NetworkNodes.Create(newNode, Clipboard.GetText());
                foreach (TreeNode item in newNode.Nodes)
                {
                    treeView.Nodes.Add(item);
                    treeView.SelectedNode = item;
                }
                NodesEditingHistory.Add(newNode, true);
            }
            catch (Exception ex)
            {
                WindowsForm.Loading2.End();
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Error!!!", ex, this);
                return;
            }
            WindowsForm.Loading2.End();
        }
        private void addFromAStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsForm.Loading2.Show(this);
            //Nạp các TreeNodes cho TreeNode đó
            try
            {
                NetworkNodes.Create(treeView.SelectedNode, Clipboard.GetText());
                treeView.SelectedNode.Expand();
                NodesEditingHistory.Add(treeView.SelectedNode, true);
            }
            catch (Exception ex)
            {
                WindowsForm.Loading2.End();
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Error!!!", ex, this);
                return;
            }
            WindowsForm.Loading2.End();
        }
        private void copyItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsForm.Loading2.Show(this);
            try
            {
                Clipboard.SetText(NetworkNodes.Recursive_ToString_2(treeView.SelectedNode));
                if(Clipboard.GetText().Length > 4 && Clipboard.GetText().Substring(Clipboard.GetText().Length - 3) != @" \e")
                {
                    Clipboard.SetText(Clipboard.GetText() + @" \e");
                }
            }
            catch (Exception ex)
            {
                WindowsForm.Loading2.End();
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Error!!!", ex, this);
                return;
            }
            WindowsForm.Loading2.End();
        }
        #endregion

        #endregion

        //panelMid
        #region panelMid
        private void panelMid_MouseDown(object sender, MouseEventArgs e)
        {
            oldLeft_of_panelMid = panelMid.Left;
            left_of_mouse_and_panelMid = MousePosition.X - panelMid.Left;
            clicked = true;
        }
        private void panelMid_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
        }
        //change location panelMid
        private void panelMid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!clicked || panelMid.Left < 35) return;
            if (MousePosition.X - left_of_mouse_and_panelMid < 35) panelMid.Left = 35;
            else if (Width - MousePosition.X + left_of_mouse_and_panelMid < 35) panelMid.Left = Width - 35;
            else panelMid.Left = MousePosition.X - left_of_mouse_and_panelMid;
            int index = oldLeft_of_panelMid - panelMid.Left;
            panel_treeView.Left -= index;
            panel_treeView.Width += index;
            panelMain.Width -= index;
            oldLeft_of_panelMid = panelMid.Left;
            if (panelMid.Left > 35 && panelMid.Left < Width - 35) oldLeft_panelMid = panelMid.Left;
        }
        //Change size form
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            //form đang thu nhỏ
            if ((panelMid.Left - oldLeft_of_panelMid2) < 0 && panelMid.Left < 35 && WindowState != FormWindowState.Minimized)
            {
                panelMid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                panel_treeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                panelMid.Left = 35;
                panel_treeView.Left = 46;
                panel_treeView.Width = Width - panel_treeView.Left - 18;
                panelMain.Width = 12;
            }
            //form đang nở rộng
            else if (panelMid.Anchor == (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left) && panel_treeView.Width > 245)
            {
                panelMid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                panel_treeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                panelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
            oldLeft_of_panelMid2 = panelMid.Left;

            if (Width > 160 && Width < (Screen.PrimaryScreen.Bounds.Width + 16)) oldWidth = Width;
            if (Height > 28 && Height < (Screen.PrimaryScreen.Bounds.Height - 24)) oldHeight = Height;
        }
        #endregion

        //comboBoxSearch
        #region comboBoxSearch
        private void comboBoxSearch_Enter(object sender, EventArgs e)
        {
            //nếu là lần click đầu tiên
            if (firstClickToComboBoxSearch)
            {
                firstClickToComboBoxSearch = false;
                comboBoxSearch.ForeColor = Color.DarkGray;
                comboBoxSearch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                comboBoxSearch.Text = "";
            }
        }
        private void comboBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                comboBoxSearch.SelectAll();
            }

            if (e.KeyCode == Keys.Enter && comboBoxSearch.Text.Length > 0)
            {
                string str = comboBoxSearch.Text;
                comboBoxSearch.Text = "";

                //nhập cmd bình thường
                if (hashtableCMD.ContainsKey(str))
                {
                    foreach (DictionaryEntry item in hashtableCMD)
                    {
                        if (str == (string)item.Key)
                        {
                            SearchOrRun((string)item.Value);
                            break;
                        }
                    }
                }
                //nhập cmd plus
                else if (str.Contains("//") && hashtableCMD.ContainsKey(str.Substring(0, str.IndexOf("//"))))
                {
                    string str2 = str.Substring(0, str.IndexOf("//"));
                    foreach (DictionaryEntry item in hashtableCMD)
                    {
                        if (str2 == (string)item.Key)
                        {
                            SearchOrRun((string)item.Value + str.Substring(str.IndexOf("//") + ("//").Length));
                            break;
                        }
                    }
                }
                //nhập lệnh run
                else if (str.Length > 4 && str.Substring(0, 4) == "run ")
                {
                    try
                    {
                        Process.Start(str.Substring(4));
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //tìm kiếm trên google
                else
                {
                    Process.Start("https://www.google.com/search?q=" + HTML_URL_Encode(str));
                    WindowState = FormWindowState.Minimized;
                }

                //nạp lịch sử tìm kiếm
                if (!searchHistory.Contains(str))
                {
                    searchHistory.Add(str);
                    UpdateDataSource_comboBoxSearch();
                }
            }
        }
        private void buttonEditCmd_Click(object sender, EventArgs e)
        {
            formEditCmd.Show(Left, Top, Width, Height);
        }
        private void UpdateDataSource_comboBoxSearch()
        {
            string str = comboBoxSearch.Text;
            autoCompleteStringCollection.Clear();
            foreach (string item in searchHistory)
            {
                autoCompleteStringCollection.Add(item);
            }
            comboBoxSearch.Enabled = false;
            comboBoxSearch.DataSource = null;
            comboBoxSearch.DataSource = autoCompleteStringCollection;
            comboBoxSearch.SelectedIndex = -1;
            comboBoxSearch.Enabled = true;
            comboBoxSearch.Text = str;
            comboBoxSearch.Select();
        }
        #endregion

        //mọi thứ về textBoxContent
        #region textBoxContent

        //thanh chỉnh zoomfactor nhanh
        #region trackBarSizeFont
        private void trackBarZoomFactor_ValueChanged(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.Tag == null) treeView.SelectedNode.Tag = new Tag_of_Node();
                //không chỉnh được zoomFactor khi node đang ở trạng thái locked
                if (!(((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked))
                {
                    ((Tag_of_Node)treeView.SelectedNode.Tag).zoomFactor = trackBarZoomFactor.Value / 1000f;
                    textBoxContent.ZoomFactor = trackBarZoomFactor.Value / 1000f;
                }
            }
            comboBox2.Enabled = false;
            comboBox2.Text = (trackBarZoomFactor.Value / 10).ToString();
            comboBox2.Enabled = true;
            comboBox2.Focus();
            comboBox2.Select(comboBox2.Text.Length, 0);
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (!comboBox2.Enabled) return;
            string str = comboBox2.Text;
            try
            {
                trackBarZoomFactor.Value = 10 * int.Parse(comboBox2.Text);
            }
            catch
            {
                comboBox2.Text = str;
            }
        }
        #endregion

        //context menu strip
        #region contextMenuStrip_textBoxContent

        //show context menu strip
        #region show
        private void textBoxContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //undo
                if (textBoxContent.CanUndo) toolStripMenuItem_Undo.Enabled = true;
                else toolStripMenuItem_Undo.Enabled = false;
                //find and replace
                if (FormFindAndReplace.Enabled) findCtrlFToolStripMenuItem.Enabled = false;
                else findCtrlFToolStripMenuItem.Enabled = true;

                if (textBoxContent.ReadOnly)
                {
                    //edit content
                    if(treeView.SelectedNode != null)
                        editContentF3ToolStripMenuItem.Enabled = true;
                    else
                        editContentF3ToolStripMenuItem.Enabled = false;
                    //get
                    getToolStripMenuItem.Enabled = false;
                    //cut rows
                    cutRowsToolStripMenuItem.Enabled = false;
                    //paste
                    toolStripMenuItem_Paste.Enabled = false;
                    //cut
                    toolStripMenuItem_Cut.Enabled = false;
                    //font
                    fontToolStripMenuItem.Enabled = false;
                    //text color
                    changeToolStripMenuItem.Enabled = false;
                    //back color
                    backgroundColorToolStripMenuItem.Enabled = false;
                }
                else
                {
                    //edit content
                    editContentF3ToolStripMenuItem.Enabled = false;
                    //get
                    getToolStripMenuItem.Enabled = true;
                    //cut rows
                    cutRowsToolStripMenuItem.Enabled = true;
                    //paste
                    if (Clipboard.ContainsImage() || Clipboard.ContainsText() || Clipboard.ContainsAudio() || Clipboard.ContainsFileDropList()) toolStripMenuItem_Paste.Enabled = true;
                    //cut
                    toolStripMenuItem_Cut.Enabled = true;
                    //font
                    fontToolStripMenuItem.Enabled = true;
                    //text color
                    changeToolStripMenuItem.Enabled = true;
                    //back color
                    backgroundColorToolStripMenuItem.Enabled = true;
                }
            }
        }
        #endregion

        //các thao tác bàn phím
        #region textBoxContent_KeyDown
        private void textBoxContent_KeyDown(object sender, KeyEventArgs e)
        {
            //sử dụng Ctrl
            #region
            if (e.Control && !e.Alt && !e.Shift)
            {
                if (!textBoxContent.ReadOnly)
                {
                    
                    //                                                                  - Ctrl + 1
                    if (e.KeyCode == Keys.D1)
                    {
                        TextBoxContent_Paste(DateTime.Now.ToLongTimeString());
                    }
                    //                                                                  - Ctrl + 2
                    if (e.KeyCode == Keys.D2)
                    {
                        dateToolStripMenuItem_Click(null,null);
                    }
                    //                                                                  - Ctrl + 3
                    if (e.KeyCode == Keys.D3)
                    {
                        TextBoxContent_Paste(DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString());
                    }
                    //                                                                  - Ctrl + 4
                    if (e.KeyCode == Keys.D4)
                    {
                        TextBoxContent_Paste(DateTime.Now.DayOfWeek.ToString());
                    }
                    //xoá các dòng đã chọn                                              - Ctrl + L
                    if (e.KeyCode == Keys.L)
                    {
                        CutRows();
                    }
                    //lưu                                                               - Ctrl + S
                    if (e.KeyCode == Keys.S)
                    {
                        SaveContent();
                    }
                }
                //Zoom in                                                           - Ctrl + "+"
                if (e.KeyCode == Keys.Oemplus && trackBarZoomFactor.Value + 50 <= trackBarZoomFactor.Maximum)
                {
                    trackBarZoomFactor.Value += 50;
                }
                //Zoom out                                                          - Ctrl + "-"
                if (e.KeyCode == Keys.OemMinus && trackBarZoomFactor.Value - 50 >= trackBarZoomFactor.Minimum)
                {
                    trackBarZoomFactor.Value -= 50;
                }
                //Run the highlighting text                                         - Ctrl + D
                if (e.KeyCode == Keys.D)
                {
                    runTheTextToolStripMenuItem_Click_1(null, null);
                }
                //Find and Replace                                                  - Ctrl + F
                if (e.KeyCode == Keys.F && !FormFindAndReplace.Enabled)
                {
                    Show_FormFindAndReplace();
                }

                //bôi đen toàn bộ văn bản                                           - Ctrl + A
                if (e.KeyCode == Keys.A)
                {
                    textBoxContent.SelectAll();
                }
            }
            #endregion

            //Ở chế độ Edit và không được phép sử dụng Ctrl
            #region
            else if (!textBoxContent.ReadOnly)
            {
                //Cancel                                                            - Esc
                if (e.KeyCode == Keys.Escape)
                {
                    buttonCancel_Click_1(null, null);
                }
                //căn lề bằng
                if (WindowsForm.Setting.checkBox__Automatically_add_tabs_when_down_line.Checked && e.KeyCode == Keys.Enter && textBoxContent.SelectionStart > 0 && textBoxContent.TextLength > 0)
                {
                    string str = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart);
                    int index = str.LastIndexOf("\n");
                    if (index == -1) index = 0;
                    else index += 1;
                    StringBuilder stringBuilder = new StringBuilder(str.Substring(index));
                    string str2 = "\n";
                    for (int i = 0; i < stringBuilder.Length; i++)
                    {
                        if (stringBuilder.ToString(i, 1).Contains("\t")) str2 += "\t";
                        else break;
                    }
                    if (str2.Length > 1)
                    {
                        TextBoxContent_Paste(str2);
                        e.Handled = true;
                    }
                }
                //tự động thêm TAB cho nhiều dòng
                if (e.KeyCode == Keys.Tab && textBoxContent.SelectedText.Contains("\n"))
                {
                    e.SuppressKeyPress = true;
                    SeleteRows();
                    string str;
                    if (textBoxContent.SelectedText.Substring(textBoxContent.SelectedText.Length - 1) == "\n")
                        str = ("\n" + textBoxContent.SelectedText.Substring(0, textBoxContent.SelectedText.Length - 1)).Replace("\n", "\n\t");
                    else
                        str = ("\n" + textBoxContent.SelectedText.Substring(0, textBoxContent.SelectedText.Length)).Replace("\n", "\n\t");
                    TextBoxContent_Paste(str.Substring(1) + "\n\n");
                    textBoxContent.SelectionStart--;
                }
            }
            #endregion

            //Ở chế độ View và không được phép sử dụng Ctrl
            #region
            else
            {
                //                                                                  - F3
                if (e.KeyCode == Keys.F3)
                {
                    OpenEditContent_of_SelectedNode();
                }
            }
            #endregion
        }
        #endregion

        //undo
        #region toolStripMenuItem_Undo
        private void toolStripMenuItem_Undo_Click(object sender, EventArgs e)
        {
            textBoxContent.Undo();
        }
        #endregion

        //save
        #region 
        private void toolStripMenuItem_save_Click(object sender, EventArgs e)
        {
            SaveContent();
        }
        private void SaveContent()
        {
            NodesEditingHistory.Add(treeView.SelectedNode);
            //đưa TextBoxContent về trạng thái chỉ đọc
            CloseEditting();
            treeView.SelectedNode.Name = textBoxContent.Rtf;
        }

        #endregion

        //edit
        #region toolStripMenuItem_Edit
        private void OpenEditContent_of_SelectedNode()
        {
            if (treeView.SelectedNode == null) return;
            //nếu node ở trạng thái đang khoá
            if (((Tag_of_Node)treeView.SelectedNode.Tag).is_lockNode && !((Tag_of_Node)treeView.SelectedNode.Tag).unlocked)
            {
                UnlockTheNode();
            }
            else
            {
                try
                {
                    if (treeView.SelectedNode.Name.Length != 0) ShowTreeNodeContent(treeView.SelectedNode);
                    else
                    {
                        textBoxContent.Clear();//nếu Node đó là mới tạo
                        textBoxContent.SelectionBullet = false;
                        textBoxContent.Select();
                    }
                    panelEditingTool.Visible = true;
                    panel_textBoxContent.Top += 61;
                    panel_textBoxContent.Height -= 61;
                    textBoxContent.AcceptsTab = true;
                    panelHL1.Visible = true;
                    panelHL2.Visible = true;
                    panelHL3.Visible = true;
                    panelHL4.Visible = true;
                    textBoxContent.ReadOnly = false;
                    textBoxContent.Focus();
                    textBoxContent_SelectionChanged(null, null);
                }
                catch { }
            }
        }
        #endregion

        //select all
        #region
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxContent.Select();
            textBoxContent.SelectAll();
        }
        #endregion

        //cut
        #region
        private void toolStripMenuItem_Cut_Click(object sender, EventArgs e)
        {
            textBoxContent.Cut();
        }
        #endregion

        //copy
        #region
        private void toolStripMenuItem_Copy_Click(object sender, EventArgs e)
        {
            textBoxContent.Copy();
        }
        #endregion

        //paste
        #region
        private void toolStripMenuItem_Paste_Click(object sender, EventArgs e)
        {
            if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.Bitmap)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
            }
            else if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.Rtf)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Rtf));
            }
            else if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.Text)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Text));
            }
            else if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.FileDrop)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.FileDrop));
            }
            else if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.Html)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Html));
            }
            else if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.WaveAudio)))
            {
                textBoxContent.Paste(DataFormats.GetFormat(DataFormats.WaveAudio));
            }
        }
        #endregion

        //delete rows
        #region
        private void cutRowsToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (WindowsForm.Setting.checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L.Checked)
            {
                textBoxContent.Cut();
            }
            else
            {
                string str = Clipboard.GetText();
                textBoxContent.Cut();
                Clipboard.SetText(str);
            }
        }
        private void CutRows()
        {
            if (WindowsForm.Setting.checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L.Checked)
            {
                SeleteRows();
                textBoxContent.Cut();
            }
            else
            {
                string str = Clipboard.GetText();
                SeleteRows();
                textBoxContent.Cut();
                if(!string.IsNullOrEmpty(str))
                    Clipboard.SetText(str);
            }
        }
        private void SeleteRows()
        {
            if (textBoxContent.TextLength == 0) return;
            int index1 = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart).LastIndexOf('\n') + 1;
            int index2;
            if (textBoxContent.SelectionLength > 0 && textBoxContent.SelectedText.Substring(textBoxContent.SelectionLength - 1) == "\n")
                index2 = 0;
            else
                index2 = textBoxContent.Text.Substring(textBoxContent.SelectionStart + textBoxContent.SelectionLength).IndexOf('\n');
            if (index2 == -1)
            {
                index2 = textBoxContent.Text.Substring(textBoxContent.SelectionStart + textBoxContent.SelectionLength).Length;
                if (index1 - 1 > -1) index1--;
            }
            else if (index2 > -1)
            {
                index2++;
            }
            textBoxContent.Select(index1, textBoxContent.SelectionStart + textBoxContent.SelectionLength + index2 - index1);
        }
        //bôi đen vùng chuẩn bị cut khi rê chuột qua cutRowsToolStripMenuItem
        private void cutRowsToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            index_text = textBoxContent.SelectionStart;
            length_text = textBoxContent.SelectionLength;
            SeleteRows();
        }
        private void cutRowsToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            textBoxContent.SelectionStart = index_text;
            textBoxContent.SelectionLength = length_text;
        }
        #endregion

        //get
        #region
        //Date
        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxContent_Paste(DateTime.Now.Day+"//"+DateTime.Now.Month+"//"+DateTime.Now.Year);
        }
        //Day of Week
        private void dayOfWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxContent_Paste(DateTime.Now.DayOfWeek.ToString());
        }
        //Time
        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxContent_Paste(DateTime.Now.ToLongTimeString());
        }
        //DateTime
        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxContent_Paste(DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString());
        }
        //Rich text format
        private void richTextFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxContent.SelectedRtf);
        }
        #endregion

        //Run the highlighting text
        #region
        private void runTheTextToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (textBoxContent.SelectedText.Substring(0, 6) == "ftp://" || textBoxContent.SelectedText.Substring(0, 7) == "http://" || (textBoxContent.SelectedText.Substring(0, 8) == "https://") && !textBoxContent.SelectedText.Contains(" "))
                {
                    try
                    {
                        Process.Start(textBoxContent.SelectedText);
                    }
                    catch
                    {
                        Process.Start("https://www.google.com/search?q=" + HTML_URL_Encode(textBoxContent.SelectedText));
                    }
                }
                else
                {
                    Process.Start("https://www.google.com/search?q=" + HTML_URL_Encode(textBoxContent.SelectedText));
                }

            }
            catch
            {
                Process.Start("https://www.google.com/search?q=" + HTML_URL_Encode(textBoxContent.SelectedText));
            }
            WindowState = FormWindowState.Minimized;
        }
        #endregion

        //Find and Replace
        #region
        private void textBoxContent_TextChanged(object sender, EventArgs e)
        {
            if (FormFindAndReplace != null && FormFindAndReplace.Enabled)
            {
                FormFindAndReplace.CheckText();
            }
        }
        private void findCtrlFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show_FormFindAndReplace();
        }
        private void Show_FormFindAndReplace()
        {
            if (FormFindAndReplace.Enabled)
                FormFindAndReplace.Activate();
            else
                FormFindAndReplace.ShowForm();
        }
        private void textBoxContent_ReadOnlyChanged(object sender, EventArgs e)
        {
            FormFindAndReplace.SetEnabledForButtonReplace(!textBoxContent.ReadOnly);
        }
        #endregion

        //Text color
        #region
        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object a = Dialogs.ShowColorDialog(textBoxContent.SelectionColor, true);
            if (a != null)
            {
                textBoxContent.SelectionColor = (Color)a;
            }
        }
        #endregion

        //Back color
        #region
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object a = Dialogs.ShowColorDialog(textBoxContent.SelectionBackColor, true);
            if (a != null)
            {
                textBoxContent.SelectionBackColor = (Color)a;
            }
        }
        #endregion

        //Font
        #region
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object f = Dialogs.ShowFontDialog(textBoxContent.SelectionFont);
            if (f != null) textBoxContent.SelectionFont = (Font)f;
        }
        #endregion
        #endregion

        #endregion

        //các timers
        #region
        //auto save
        private void timerAutoSave_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(path))
            {
                timerAutoSave.Enabled = false;
                if (textBoxContent.ReadOnly)
                {
                    SaveTreeView();
                }
                timerAutoSave.Enabled = true;
            }
        }
        //
        void ChangeTimerAutoSave(int index)
        {
            if (index > 0)
            {
                timerAutoSave.Interval = index * 60000;
                timerAutoSave.Enabled = true;
            }
            else
                timerAutoSave.Enabled = false;
        }
        #endregion

        //panelEditingTool
        #region

        //Selection Changed
        #region
        private void textBoxContent_SelectionChanged(object sender, EventArgs e)
        {
            if (panelEditingTool.Visible)
            {
                comboBoxFont_textBoxContent.Enabled = false;
                comboBoxFontSize_textBoxContent.Enabled = false;

                if (textBoxContent.SelectionFont != null)
                {
                    comboBoxFont_textBoxContent.SelectedItem = textBoxContent.SelectionFont.Name;
                    comboBoxFontSize_textBoxContent.Text = textBoxContent.SelectionFont.Size.ToString();
                }
                CheckButtonsFont();

                buttonBackColor_textBoxContent.BackColor = textBoxContent.SelectionBackColor;
                buttonColor_textBoxContent.BackColor = textBoxContent.SelectionColor;

                comboBoxFont_textBoxContent.Enabled = true;
                comboBoxFontSize_textBoxContent.Enabled = true;
                numericUpDownFont.Value = decimal.Parse(comboBoxFontSize_textBoxContent.Text);
            }
        }
        #endregion

        //CheckButtonsFont
        #region
        private void CheckButtonsFont()
        {
            if (textBoxContent.SelectionFont != null)
            {
                if (textBoxContent.SelectionFont.Bold)
                    buttonB.BackColor = cornflowerBlue;
                else
                    buttonB.BackColor = panelEditingTool.BackColor;

                if (textBoxContent.SelectionFont.Italic)
                    buttonI.BackColor = cornflowerBlue;
                else
                    buttonI.BackColor = panelEditingTool.BackColor;

                if (textBoxContent.SelectionFont.Underline)
                    buttonU.BackColor = cornflowerBlue;
                else
                    buttonU.BackColor = panelEditingTool.BackColor;

                if (textBoxContent.SelectionFont.Strikeout)
                    buttonABC.BackColor = cornflowerBlue;
                else
                    buttonABC.BackColor = panelEditingTool.BackColor;
            }

            if (textBoxContent.SelectionCharOffset == 0)
            {
                buttonSubscript.BackColor = panelEditingTool.BackColor;
                buttonSuperscript.BackColor = panelEditingTool.BackColor;
            }
            else if (textBoxContent.SelectionCharOffset > 0)
            {
                buttonSubscript.BackColor = panelEditingTool.BackColor;
                buttonSuperscript.BackColor = cornflowerBlue;
            }
            else
            {
                buttonSubscript.BackColor = cornflowerBlue;
                buttonSuperscript.BackColor = panelEditingTool.BackColor;
            }

            if (textBoxContent.SelectionBullet)
            {
                buttonBullet.BackColor = cornflowerBlue;
            }
            else
            {
                buttonBullet.BackColor = panelEditingTool.BackColor;
            }

            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Left)
            {
                buttonAlignLeft.BackColor = cornflowerBlue;
                buttonAlignRight.BackColor = panelEditingTool.BackColor;
                buttonAlignCenter.BackColor = panelEditingTool.BackColor;
            }
            else if (textBoxContent.SelectionAlignment == HorizontalAlignment.Center)
            {
                buttonAlignLeft.BackColor = panelEditingTool.BackColor;
                buttonAlignRight.BackColor = panelEditingTool.BackColor;
                buttonAlignCenter.BackColor = cornflowerBlue;
            }
            else
            {
                buttonAlignLeft.BackColor = panelEditingTool.BackColor;
                buttonAlignRight.BackColor = cornflowerBlue;
                buttonAlignCenter.BackColor = panelEditingTool.BackColor;
            }
        }
        #endregion

        //nút đổi màu nền của chữ
        #region
        private void buttonBackColor_textBoxContent_Click(object sender, EventArgs e)
        {
            object a = Dialogs.ShowColorDialog(buttonBackColor_textBoxContent.BackColor, true);
            if (a != null)
            {
                buttonBackColor_textBoxContent.BackColor = (Color)a;
                textBoxContent.SelectionBackColor = buttonBackColor_textBoxContent.BackColor;
            }
            textBoxContent.Focus();
        }
        #endregion

        //nút đổi màu chữ
        #region
        private void buttonColor_textBoxContent_Click(object sender, EventArgs e)
        {
            object a = Dialogs.ShowColorDialog(buttonColor_textBoxContent.BackColor, true);
            if (a != null)
            {
                buttonColor_textBoxContent.BackColor = (Color)a;
                textBoxContent.SelectionColor = buttonColor_textBoxContent.BackColor;
            }
            textBoxContent.Focus();
        }
        #endregion

        //đổi font chữ
        #region
        private void comboBoxFont_textBoxContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFont_textBoxContent.Enabled)
            {
                if (textBoxContent.SelectionFont != null)
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style);
                }
                else
                {
                    try
                    {
                        textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text));
                    }
                    catch
                    {
                        textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.Font.Size);
                    }
                }
            }
            textBoxContent.Focus();
        }
        #endregion

        //đổi kích thước chữ
        #region
        private void comboBoxFontSize_textBoxContent_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxFontSize_textBoxContent.Enabled)
            {
                string oldText = comboBoxFontSize_textBoxContent.Text;
                try
                {
                    if (textBoxContent.SelectionFont != null)
                        textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, float.Parse(comboBoxFontSize_textBoxContent.Text), textBoxContent.SelectionFont.Style);
                    else
                        textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text));
                    numericUpDownFont.Value = decimal.Parse(comboBoxFontSize_textBoxContent.Text);
                }
                catch
                {
                    comboBoxFontSize_textBoxContent.Text = oldText;
                }
            }
        }
        #endregion

        //sự kiện click của các cells màu
        #region
        private void panelCell_Color_Click(object sender, EventArgs e)
        {
            buttonColor_textBoxContent.BackColor = ((Panel)sender).BackColor;
            textBoxContent.SelectionColor = buttonColor_textBoxContent.BackColor;
            textBoxContent.Focus();
        }
        private void panelCell_Color_Leave(object sender, EventArgs e)
        {
            ((Panel)sender).BorderStyle = BorderStyle.Fixed3D;
        }
        private void panelCell_Color_Enter(object sender, EventArgs e)
        {
            ((Panel)sender).BorderStyle = BorderStyle.FixedSingle;
        }
        #endregion

        //button B
        #region
        private void buttonB_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionFont != null)
            {
                if (!textBoxContent.SelectionFont.Bold)
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style | FontStyle.Bold);
                }
                else
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style ^ FontStyle.Bold);
                }
            }
            else
            {
                try
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text), FontStyle.Bold);
                }
                catch
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.Font.Size, FontStyle.Bold);
                }
            }
            CheckButtonsFont();
            textBoxContent.Focus();
        }
        #endregion

        //button I
        #region
        private void buttonI_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionFont != null)
            {
                if (!textBoxContent.SelectionFont.Italic)
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style | FontStyle.Italic);
                }
                else
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style ^ FontStyle.Italic);
                }

            }
            else
            {
                try
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text), FontStyle.Italic);
                }
                catch
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.Font.Size, FontStyle.Italic);
                }
            }
            CheckButtonsFont();
            textBoxContent.Focus();
        }
        #endregion

        //button U
        #region
        private void buttonU_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionFont != null)
            {
                if (!textBoxContent.SelectionFont.Underline)
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style | FontStyle.Underline);
                }
                else
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style ^ FontStyle.Underline);
                }
            }
            else
            {
                try
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text), FontStyle.Underline);
                }
                catch
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.Font.Size, FontStyle.Underline);
                }
            }
            CheckButtonsFont();
            textBoxContent.Focus();
        }
        #endregion

        //button ABC
        #region
        private void buttonABC_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionFont != null)
            {
                if (!textBoxContent.SelectionFont.Strikeout)
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style | FontStyle.Strikeout);
                }
                else
                {
                    textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, textBoxContent.SelectionFont.Size, textBoxContent.SelectionFont.Style ^ FontStyle.Strikeout);
                }
            }
            else
            {
                try
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text), FontStyle.Strikeout);
                }
                catch
                {
                    textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), textBoxContent.Font.Size, FontStyle.Strikeout);
                }
            }
            CheckButtonsFont();
            textBoxContent.Focus();
        }
        #endregion

        //buttonSubscript
        #region
        private void buttonSubscript_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionCharOffset < 0)
            {
                buttonSubscript.BackColor = panelEditingTool.BackColor;
                textBoxContent.SelectionCharOffset = 0;
            }
            else
            {
                buttonSubscript.BackColor = cornflowerBlue;
                if (textBoxContent.SelectionFont != null)
                    textBoxContent.SelectionCharOffset = -1 * (int)(textBoxContent.SelectionFont.Size * 0.6);
                else
                    textBoxContent.SelectionCharOffset = -1 * (int)(textBoxContent.Font.Size * 0.6);
                buttonSuperscript.BackColor = panelEditingTool.BackColor;
            }
            textBoxContent.Focus();
        }
        #endregion

        //buttonSuperscript
        #region
        private void buttonSuperscript_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionCharOffset > 0)
            {
                buttonSuperscript.BackColor = panelEditingTool.BackColor;
                textBoxContent.SelectionCharOffset = 0;
            }
            else
            {
                buttonSuperscript.BackColor = cornflowerBlue;
                if (textBoxContent.SelectionFont != null)
                    textBoxContent.SelectionCharOffset = (int)(textBoxContent.SelectionFont.Size * 0.6);
                else
                    textBoxContent.SelectionCharOffset = (int)(textBoxContent.Font.Size * 0.6);
                buttonSubscript.BackColor = panelEditingTool.BackColor;
            }
            textBoxContent.Focus();
        }
        #endregion

        //button Bullet
        #region
        private void buttonBullet_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionBullet)
            {
                textBoxContent.SelectionBullet = false;
                buttonBullet.BackColor = panelEditingTool.BackColor;
            }
            else
            {
                textBoxContent.SelectionBullet = true;
                buttonBullet.BackColor = cornflowerBlue;
            }
            textBoxContent.Focus();
        }
        #endregion

        //buttonHAHAHA
        #region
        private void buttonHAHAHA_Click(object sender, EventArgs e)
        {
            textBoxContent.SelectionCharOffset = 0;
            textBoxContent.SelectionFont = textBoxContent.Font;
            textBoxContent.SelectionColor = textBoxContent.ForeColor;
            textBoxContent.SelectionBackColor = textBoxContent.BackColor;
            textBoxContent.SelectionBullet = false;
            textBoxContent_SelectionChanged(null, null);
        }
        #endregion

        //buttonAlignLeft
        #region
        private void buttonAlignLeft_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Center)
                buttonAlignCenter.BackColor = panelEditingTool.BackColor;
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Right)
                buttonAlignRight.BackColor = panelEditingTool.BackColor;
            textBoxContent.SelectionAlignment = HorizontalAlignment.Left;
            buttonAlignLeft.BackColor = cornflowerBlue;
            textBoxContent.Focus();
        }
        #endregion

        //buttonAlignRight
        #region
        private void buttonAlignRight_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Left)
                buttonAlignLeft.BackColor = panelEditingTool.BackColor;
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Center)
                buttonAlignCenter.BackColor = panelEditingTool.BackColor;
            textBoxContent.SelectionAlignment = HorizontalAlignment.Right;
            buttonAlignRight.BackColor = cornflowerBlue;
            textBoxContent.Focus();
        }
        #endregion

        //buttonAlignCenter
        #region
        private void buttonAlignCenter_Click(object sender, EventArgs e)
        {
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Left)
                buttonAlignLeft.BackColor = panelEditingTool.BackColor;
            if (textBoxContent.SelectionAlignment == HorizontalAlignment.Right)
                buttonAlignRight.BackColor = panelEditingTool.BackColor;
            textBoxContent.SelectionAlignment = HorizontalAlignment.Center;
            buttonAlignCenter.BackColor = cornflowerBlue;
            textBoxContent.Focus();
        }
        #endregion

        //nút thêm ảnh
        #region
        private void buttonAddPicture_textBoxContent_Click(object sender, EventArgs e)
        {
            string path = Dialogs.ShowOpenFileDialog("Open Image", "Supported Files (*.bmp; *.gif; *.exif; *.jpg; *.png; *.tiff) | *.bmp; *.gif; *.exif; *.jpg; *.png; *.tiff", new string[] { "bmp", "gif", "exif", "jpg", "png", "tiff" });
            if (path != null)
            {
                try
                {
                    Stream stream = new FileStream(path, FileMode.Open);
                    Clipboard.SetImage(new Bitmap(stream));
                    stream.Close();
                    if (textBoxContent.CanPaste(DataFormats.GetFormat(DataFormats.Bitmap)))
                    {
                        textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            textBoxContent.Focus();
        }
        #endregion

        //nút thêm File
        #region
        private void buttonAddFile_textBoxContent_Click(object sender, EventArgs e)
        {
            string path = Dialogs.ShowOpenFileDialog("Open Rich-Text-Format File", "Supported Files (*.rtf; *.txt) | *.rtf; *.txt", new string[] { "rtf", "txt" });
            if (path != null && path.Length > 4)
            {
                if (path.Substring(path.Length - 4) == ".txt")
                {
                    try
                    {
                        textBoxContent.Rtf = PlainTextToRtf(File.ReadAllText(path));
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    try
                    {
                        textBoxContent.LoadFile(path);
                    }
                    catch (Exception ex)
                    {
                        WindowsForm.Notification.Show(MessageBoxButtons.OK, ex.ToString(), this);
                    }
                }
            }
            textBoxContent.Focus();
        }
        public string PlainTextToRtf(string plainText)
        {
            string escapedPlainText = plainText.Replace(@"\", @"\\").Replace("{", @"\{").Replace("}", @"\}");
            string rtf = @"{\rtf1\ansi{\fonttbl\f0\fswiss Helvetica;}\f0\pard ";
            rtf += escapedPlainText.Replace(Environment.NewLine, @" \par ");
            rtf += " }";
            return rtf;
        }
        #endregion

        #endregion

        //chung
        #region
        private string GetDateTime()
        {
            return DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "  " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        }
        private void SearchOrRun(string text)
        {
            try
            {
                Process.Start(text);
                if (text.Length > 4)
                {
                    if (text.Substring(0, 5).Contains("https") || text.Substring(0, 5).Contains("www.")) WindowState = FormWindowState.Minimized;
                }
            }
            catch
            {
                Process.Start("https://www.google.com/search?q=" + HTML_URL_Encode(text));
                WindowState = FormWindowState.Minimized;
            }
        }
        private string HTML_URL_Encode(string text)
        {
            if (text.Contains("%")) text = text.Replace("%", "%25");
            if (text.Contains("&")) text = text.Replace("&", "%26");
            if (text.Contains("+")) text = text.Replace("+", "%2b");
            if (text.Contains("#")) text = text.Replace("#", "%23");
            return text;
        }
        private void TextBoxContent_Paste(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (Clipboard.ContainsText())
                {
                    string old_str = Clipboard.GetText();
                    Clipboard.SetText(text);
                    textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Text));
                    Clipboard.SetText(old_str);
                }
                else
                {
                    Clipboard.SetText(text);
                    textBoxContent.Paste(DataFormats.GetFormat(DataFormats.Text));
                }
            }
        }
        #endregion

        //các phương thức dành cho node
        #region

        //Lưu TreeView
        #region
        void SaveTreeView()
        {
            if (NodesEditingHistory.Check())
            {
                if (string.IsNullOrEmpty(path) && treeView.GetNodeCount(true) > 0) ChoosePathToSave();
                if (!string.IsNullOrEmpty(path)) NetworkNodes.SaveAll(treeView, path);
            }
        }
        #endregion

        //tạo mới node
        #region
        private void CreateTreeNode(TreeNode node)
        {
            if (node == null) return;
            if (((Tag_of_Node)node.Tag).is_lockNode && !((Tag_of_Node)node.Tag).unlocked)
            {
                LockOrUnlockTheNode();
            }
            else
            {
                string text = WindowsForm.Add.Show();
                if (text != null)
                {
                    TreeNode newNode = new TreeNode(text);
                    newNode.Tag = new Tag_of_Node(GetDateTime());
                    newNode.ForeColor = WindowsForm.Add.buttonColor.BackColor;
                    node.Nodes.Add(newNode);
                    node.Expand();
                    treeView.SelectedNode = newNode;
                    if (WindowsForm.Setting.checkBox__Automatically_open_editing_when_have_just_created_a_node.Checked)
                        OpenEditContent_of_SelectedNode();
                    NodesEditingHistory.Add(newNode, true);
                }
            }
        }
        //tạo node và add thẳng vào treeView
        private void CreateTreeNode()
        {
            string text = WindowsForm.Add.Show();
            if (text != null)
            {
                TreeNode newNode = new TreeNode(text);
                newNode.ForeColor = WindowsForm.Add.buttonColor.BackColor;
                newNode.Tag = new Tag_of_Node(GetDateTime());
                treeView.Nodes.Add(newNode);
                treeView.SelectedNode = newNode;
                if (WindowsForm.Setting.checkBox__Automatically_open_editing_when_have_just_created_a_node.Checked)
                    OpenEditContent_of_SelectedNode();
                NodesEditingHistory.Add(newNode, true);
            }
        }
        #endregion

        //đổi tên node
        #region
        void RenameTheNode()
        {
            string text = WindowsForm.Add.Show(treeView.SelectedNode.Text, treeView.SelectedNode.ForeColor);
            if (text != null)
            {
                NodesEditingHistory.Add(treeView.SelectedNode);
                treeView.SelectedNode.Text = text;
                treeView.SelectedNode.ForeColor = WindowsForm.Add.buttonColor.BackColor;
            }
        }
        #endregion

        //hiển thị nội dung node
        #region
        private void ShowTreeNodeContent(TreeNode node)
        {
            if(node != null)
            {
                if (node.Tag == null) node.Tag = new Tag_of_Node();
                //nếu là node đang khoá
                if (((Tag_of_Node)node.Tag).is_lockNode && !((Tag_of_Node)node.Tag).unlocked)
                {
                    textBoxContent.Text = "";
                    pictureBoxLock.Visible = true;
                }
                //nếu là node bình thường hoặc node đang mở
                else
                {
                    textBoxContent.Rtf = node.Name;
                    if (pictureBoxLock.Visible) pictureBoxLock.Visible = false;
                }
                //gán ZoomFactor
                if (WindowsForm.Setting.checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes.Checked)
                    textBoxContent.ZoomFactor = ((Tag_of_Node)node.Tag).zoomFactor;
                else
                    textBoxContent.ZoomFactor = 1f;
                trackBarZoomFactor.Value = (int)(textBoxContent.ZoomFactor * 1000);
                //gán SelectionText
                if (WindowsForm.Setting.checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes.Checked && ((Tag_of_Node)node.Tag).SelecionStart != -1)
                {
                    textBoxContent.SelectionStart = ((Tag_of_Node)node.Tag).SelecionStart;
                    textBoxContent.SelectionLength = ((Tag_of_Node)node.Tag).SelectionLength;
                }
            }
        }
        #endregion

        //cập nhật lại các thông số trong Tag của node
        #region
        void UpdateTheTagOfSelectedNode()
        {
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.Tag == null) treeView.SelectedNode.Tag = new Tag_of_Node(textBoxContent.SelectionStart, textBoxContent.SelectionLength, textBoxContent.ZoomFactor);
                else
                {
                    ((Tag_of_Node)treeView.SelectedNode.Tag).SelecionStart = textBoxContent.SelectionStart;
                    ((Tag_of_Node)treeView.SelectedNode.Tag).SelectionLength = textBoxContent.SelectionLength;
                    ((Tag_of_Node)treeView.SelectedNode.Tag).zoomFactor = textBoxContent.ZoomFactor;
                }
            }
        }
        #endregion

        //lưu và khoá node
        #region
        private void SaveAndLockTheUnlockedTreeNode(TreeNode node)
        {
            UseWaitCursor = true;
            //mã hoá
            NetworkNodes.SaveAndLockAUnlockedNode(node);

            #region
            //    byte[] password = Encoding.Default.GetBytes(node.Name.Substring(node.Name.LastIndexOf(lockCode) + lockCode.Length).ToCharArray());
            //    string text_content = node.Name.Substring(0, node.Name.IndexOf(pathCode)) + dateCode + ((Tag_of_a_Node)node.Tag).date + fontSizeCode + ((Tag_of_a_Node)node.Tag).zoomFactor;
            //    //đưa TreeNode về trạng thái tiền Locked (TreeNode.Name = path + lockCode)
            //    node.Name = node.Name.Substring(node.Name.LastIndexOf(pathCode) + pathCode.Length, node.Name.LastIndexOf(lockCode) + lockCode.Length - node.Name.LastIndexOf(pathCode) - pathCode.Length);
            //    //bắt đầu tiền hành các bước để lưu dữ liệu và lock TreeNode lại
            //    //tạo thư mục để lưu file (nếu chưa có)
            //    string path = Application.StartupPath + "\\Data\\";
            //    if (!Directory.Exists(path))
            //        Directory.CreateDirectory(path);
            //    //lấy đường dẫn để lưu TreeNode
            //    string fileName = node.Name.Substring(0, node.Name.LastIndexOf(lockCode));
            //    //chuyển các TreeNodes con của TreeNode đó thành chuỗi		
            //    text_nodes = "";
            //    TreeNodeToString(node);
            //    text_nodes += rightCode;
            //    //tạo bytes dữ liệu coded_text
            //    byte[] bytes = GCoding.Encoding(Encoding.Unicode.GetBytes((passCode + text_content + nodesCode + text_nodes).ToCharArray()), password);
            ////tiến hành lưu
            //WriteTheFile:
            //    try
            //    {
            //        File.WriteAllBytes(path + fileName, bytes);
            //    }
            //    catch (Exception E)
            //    {
            //        textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
            //        if (MessageBox.Show("Can't write the file: " + fileName + "\n" + E.ToString(), "ERROR", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
            //            goto WriteTheFile;
            //        else
            //            return;
            //    }
            //    //đưa node về trạng thái locked
            //    textBoxContent.Text = "";
            //    ((Tag_of_a_Node)node.Tag).bytes = bytes;
            //    ((Tag_of_a_Node)node.Tag).Clear();
            //    node.Nodes.Clear();
            #endregion

            UseWaitCursor = false;
        }
        #endregion

        //tính năng tự động khoá các nodes đang ở trạng thái mở khi click vào một node khác nằm ngoài phạm vị của node đã chọn trước đó
        #region
        void AutoLockTheNodes(TreeNode node/*đây là node vừa click*/)
        {
            if (WindowsForm.Setting.checkBox__Automatically_lock_the_Nodes.Checked)
            {
                if (treeView.SelectedNode == null)
                {
                    //đóng tất cả node ngoài node đã click
                    foreach (TreeNode a_node in treeView.Nodes)
                    {
                        NetworkNodes.LockNodes(node, a_node);
                    }
                }
                //
                else 
                if (
                        treeView.SelectedNode != null &&
                        node != treeView.SelectedNode &&
                        !NetworkNodes.SearchANode(node, treeView.SelectedNode) &&
                        !NetworkNodes.SearchANode(treeView.SelectedNode, node)
                    )
                {
                    //khoá các nodes nằm bên ngoài node vừa click
                    foreach (TreeNode a_node in treeView.Nodes)
                    {
                        NetworkNodes.LockNodes(node, a_node);
                    }
                }
            }
        }
        #endregion

        //đệ quy giúp sao chép một treenode
        #region
        private TreeNode GetCopy_a_TreeNode(TreeNode root)
        {
            //tạo node mới và gán các giá trị
            TreeNode new_root = new TreeNode(root.Text);
            new_root.ForeColor = root.ForeColor;
            new_root.Tag = new Tag_of_Node(GetDateTime());
            new_root.Name = root.Name;
            //nếu là một LockNode
            if (((Tag_of_Node)root.Tag).is_lockNode)
            {
                ((Tag_of_Node)root.Tag).is_lockNode = true;
                ((Tag_of_Node)new_root.Tag).unlocked = ((Tag_of_Node)root.Tag).unlocked;
                ((Tag_of_Node)new_root.Tag).nodes = ((Tag_of_Node)root.Tag).nodes;
            }

            foreach (TreeNode a_node in root.Nodes)
            {
                new_root.Nodes.Add(GetCopy_a_TreeNode(a_node));
            }
            return new_root;
        }
        #endregion

        #endregion

        //setting
        #region
        //mở form setting
        private void setting_StripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsForm.Setting.Show(this);
            //cập nhật lại fonts
            if (!textBoxContent.ReadOnly)
            {
                string str = textBoxContent.Rtf;
                textBoxContent.Font = WindowsForm.Setting.button__TextBox.Font;
                treeView.Font = WindowsForm.Setting.button__TreeView.Font;
                textBoxContent.Rtf = str;
            }
            else
            {
                textBoxContent.Font = WindowsForm.Setting.button__TextBox.Font;
                treeView.Font = WindowsForm.Setting.button__TreeView.Font;
                if (treeView.SelectedNode != null)
                    ShowTreeNodeContent(treeView.SelectedNode);
            }
        }
        #endregion

        //Open
        #region
        private void open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kiểm tra để lưu file cũ
            if (!string.IsNullOrEmpty(path))
            {
                NetworkNodes.SaveAll(treeView, path);
            }
            else if (treeView.GetNodeCount(true) > 0 && NodesEditingHistory.Check())
            {
                //nếu người dùng không muốn lưu current file
                if (WindowsForm.Question.Show("Save the current file?", this) == DialogResult.No)
                {
                    if (WindowsForm.Question.Show("Are you sure?", this) == DialogResult.No)
                        return;
                    else
                        buttonSave_Click(null, null);
                }
                //nếu người dùng muốn lưu current file
                else
                {
                    ChoosePathToSave();
                    //nếu user muốn save file và đã chọn xong path
                    if (!string.IsNullOrEmpty(path))
                    {
                        NetworkNodes.SaveAll(treeView, path);
                        treeView.Nodes.Clear();
                        textBoxContent.Clear();
                    }
                    //nếu user muốn save file nhưng lại huỷ việc chọn path
                    else
                        return;
                }
            }
            //mở file mới
            try
            {
                string str = Dialogs.ShowOpenFileDialog("Open KnowledgesTree File", "Supported Files (*.Ktree) | *.Ktree", new string[] { "Ktree" });
                if (!string.IsNullOrEmpty(str) && str != path)
                {
                    treeView.Nodes.Clear();
                    NetworkNodes.Create(treeView, str);
                    treeView.SelectedNode = null;
                    textBoxContent.Text = "";
                    textBoxContent.Select();
                    path = str;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Format error!", ex, this);
            }
            catch (Exception ex)
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "ERROR", ex, this);
            }
        }
        #endregion

        //About
        #region
        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            WindowsForm.Infomation.Show(this);
        }


        #endregion

        //Save
        #region
        private void save_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonSave_Click(null, null);
            SaveTreeView();
        }
        #endregion

        //New
        #region
        private void new_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //có file đang thao tác
            if (!string.IsNullOrEmpty(path))
            {
                NetworkNodes.SaveAll(treeView, path);
                treeView.Nodes.Clear();
                path = null;
                textBoxContent.Text = "";
                pictureBoxLock.Visible = false;
            }
            //không có file nào đang thao tác
            else if (treeView.GetNodeCount(true) > 0)
            {
                ChoosePathToSave();
                if (!string.IsNullOrEmpty(path))
                {
                    buttonSave_Click(null, null);
                    NetworkNodes.SaveAll(treeView, path);
                    treeView.Nodes.Clear();
                    path = null;
                    textBoxContent.Text = "";
                    pictureBoxLock.Visible = false;
                }
            }
        }
        #endregion

        void ChoosePathToSave()
        {
            path = Dialogs.ShowSaveFileDialog("Choose the path to save your file");
            if (!string.IsNullOrEmpty(path))
            {
                //kiểm tra đuôi của file
                string str = path.Substring(path.LastIndexOf("\\"));
                if (!str.Contains("."))
                {
                    path += ".Ktree";
                }
                else if (str.Substring(str.LastIndexOf(".")) != ".Ktree")
                {
                    path = path.Substring(0, path.LastIndexOf(".")) + ".Ktree";
                }
            }
            Activate();
        }

        private void numericUpDownFont_Click(object sender, EventArgs e)
        {
            comboBoxFontSize_textBoxContent.Text = numericUpDownFont.Value.ToString();
            if (textBoxContent.SelectionFont != null)
                textBoxContent.SelectionFont = new Font(textBoxContent.SelectionFont.FontFamily, float.Parse(comboBoxFontSize_textBoxContent.Text), textBoxContent.SelectionFont.Style);
            else
                textBoxContent.SelectionFont = new Font(comboBoxFont_textBoxContent.SelectedItem.ToString(), float.Parse(comboBoxFontSize_textBoxContent.Text));
        }

        private void wWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxContent.WordWrap = !textBoxContent.WordWrap;
        }
    }
}