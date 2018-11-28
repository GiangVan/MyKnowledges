using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    public partial class FormMain : Form
    {
        //khai báo
        #region
            Random random = new Random();
            //lưu thông tin về phần cứng máy tính
            private string info = null;
            //ký tự được biệt dùng để tách dữ liệu
                private const string leftCode = "!2||-@#(";
                private const string rightCode = ")#@-||2!";
                private const string nameCode = "!2||-@#x";//dùng để nhận biết TreeRoot
                private const string lockCode = "!2||-@#l";
                private const string nodesCode = "!2||-@#ns";
                private const string pathCode = "!2||-@#p";//được sử dụng để phân tách giữa Content với Path trong một TreeNode.Name khi TreeNode.Name được unlock
                private const string passCode = "!)}2#$&^( 11/07/1998 )@#$%^&($( GiangVan )%#$&*||-@#p";//dùng để nhận biết rằng người dùng đã nhập đúng password hay chưa
            //các khai báo về mật khẩu
                private byte[] passwordBytes;
            //các khai báo về treeView
                private TreeNode cutedNode = null;//lưu nút được chọn để tiến hành Cut
                private TreeNode copiedNode = null;
                private TreeNode TreeRoot;
            //các khai báo về lock / unlock
                private string text_nodes;//dùng để lưu các TreeNodes ở dạng chuỗi 
            //các khai báo về panelMid
                private bool clicked = new bool();
                private int left_of_mouse_and_panelMid = new int();
                private int oldLeft_of_panelMid2;
                private int oldLeft_of_panelMid;
            //các khai báo về textBoxContent
                private string old_text = "";//lưu text trước khi Edit để trả về text cũ nếu Cancel edit
                private int index_text = new int();//lưu vị trí SelectionStart
                private int length_text = new int();//lưu độ dài chuỗi đã bôi đen
            //
                FormFindAndReplace FormFindAndReplace; //sử dụng thuộc tính Enabed để nhận biết form này có đang showing hay không
            //
            private bool panelSetting_is_open = new bool();
        #endregion

        //constructor
        #region constructor
            public FormMain(byte[] passwordBytes, Form FormLogin)
            {
                InitializeComponent();
            
                //lấy TreeRoot từ treeView
                foreach (TreeNode item in treeView.Nodes)
                {
                    TreeRoot = item;
                    break;
                }
                //khởi tạo FormFindAndReplace
                FormFindAndReplace = new FormFindAndReplace(textBoxContent, this);
                //hiển thị phiên bản phần mềm
                textBoxContent.Text = textBoxContent.Text.Insert(9, Application.ProductVersion);
                //lưu thông tin phần mềm text vào nút (tiết kiệm :D )
                button1.Name = textBoxContent.Text;
                //nạp mật khẩu
                this.passwordBytes = passwordBytes;
                //nạp dữ liệu
                ReadFiles();
                //đóng Setting
                panelSetting.Left = -210;
                //nạp dữ liệu cho treeView
                FormLoad formLoad = new FormLoad(passwordBytes, treeView, !checkBox_TurboBoost.Checked, FormLogin);
                if (!checkBox_TurboBoost.Checked)
                    info = formLoad.info;
                if (!formLoad.password_is_wrong)
                {
                    notifyIcon.ContextMenuStrip = contextMenuStrip_notifyicon;
                    Enabled = true;
                    ShowDialog();
                }
                //ShowDialog();
        }
            private void ReadFiles()
        {
            string fileName;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\";
            try
            {
                //lưu kích thước form 
                #region
                    fileName = "WindowState.txt";
                    if (File.Exists(path + fileName))
                {
                    try
                    {
                        string str = File.ReadAllText(path + fileName);
                        int width, height;
                        string[] str2 = new string[2];
                        if (str.Length > 0)
                        {
                            if (str == "Maximized") WindowState = FormWindowState.Maximized;
                            else
                            {
                                str = "Size.txt";
                                if (File.Exists(path + str) && File.ReadAllText(path + str).Length > 0)
                                {
                                    str2 = File.ReadAllLines(path + str);
                                    width = int.Parse(str2[0]);
                                    height = int.Parse(str2[1]);
                                    if (width > 160 && width < Screen.PrimaryScreen.Bounds.Width) Width = width;
                                    if (height > 28 && height < Screen.PrimaryScreen.Bounds.Height) Height = height;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                    }
                }
                #endregion
                //ShowTime
                #region
                    fileName = "ShowTime.txt";
                    if (File.Exists(path + fileName))
                {
                    try
                    {
                        if (File.ReadAllText(path + fileName).Length > 0)
                        {
                            checkBoxShowTime.Checked = bool.Parse(File.ReadAllText(path + fileName));
                        }
                    }
                    catch (Exception e)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                    }
                }
                #endregion
                //checkBox_TurboBoost
                #region
                    fileName = "TurboBoost.txt";
                    if (File.Exists(path + fileName))
                    {
                        try
                        {
                            if (File.ReadAllText(path + fileName).Length > 0)
                            {
                                checkBox_TurboBoost.Checked = bool.Parse(File.ReadAllText(path + fileName));
                            }
                        }
                        catch (Exception e)
                        {
                            notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                        }
                    }
                #endregion
                //AutoSave
                #region
                    string pathAutoSave = path + "AutoSave\\";
                    fileName = "AutoSave.txt";
                    string fileName2 = "Value.txt";
                    if (File.Exists(pathAutoSave + fileName))
                    {
                        bool bo = checkBox_TurboBoost.Checked;
                        try
                        {
                            if (File.ReadAllText(pathAutoSave + fileName).Length > 0)
                            {
                                checkBox_TurboBoost.Checked = true;//đặt true để ngăn AnimationMOVE chạy
                                checkBoxAutoSave.Checked = bool.Parse(File.ReadAllText(pathAutoSave + fileName));
                                checkBox_TurboBoost.Checked = bo;//đặt lại như cũ
                            }
                        }
                        catch (Exception e)
                        {
                            notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                        }
                    }
                    if (checkBoxAutoSave.Checked)
                    {
                        labelAutoSave.Top = 0;
                        panel_trackBarAutoSave.Top = 14;
                    }
                    else
                    {
                        labelAutoSave.Top = 58;
                        panel_trackBarAutoSave.Top = 73;
                    }
                    if (File.Exists(pathAutoSave + fileName2))
                    {
                        try
                        {
                            if (File.ReadAllText(pathAutoSave + fileName2).Length > 0)
                            {
                                int value = int.Parse(File.ReadAllText(pathAutoSave + fileName2));
                                if(value >= 5 && value <= 180)
                                {
                                    trackBarAutoSave.Value = value;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            notifyIcon.ShowBalloonTip(1000, "Reading " + fileName2 + " file failed!", e.ToString(), ToolTipIcon.Warning);
                        }
                    }
                    labelAutoSave.Text = "Saving per " + trackBarAutoSave.Value + " minutes";
                    timerAutoSave.Interval = trackBarAutoSave.Value * 60000;
                #endregion
                //checkBox_StartWithWindows
                #region
                    fileName = "StartWithWindows.txt";
                    if (File.Exists(path + fileName))
                {
                    try
                    {
                        if (File.ReadAllText(path + fileName).Length > 0)
                        {
                            checkBox_StartWithWindows.Checked = bool.Parse(File.ReadAllText(path + fileName));
                        }
                    }
                    catch (Exception e)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                    }
                }
                #endregion
                //nạp font chữ cho textBoxContent
                #region
                    string pathFont = path + "Font\\";
                    fileName = "textBoxContent.txt";
                    if (File.Exists(pathFont + fileName))
                {
                    try
                    {
                        if (File.ReadAllText(pathFont + fileName).Length > 0)
                        {
                            FontConverter fontConverter = new FontConverter();
                            textBoxContent.Font = (Font)fontConverter.ConvertFromString(File.ReadAllText(pathFont + fileName));
                        }
                    }
                    catch (Exception e)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                    }
                }
                #endregion
                //nạp font chữ cho treeView
                #region
                    fileName = "treeView.txt";
                    if (File.Exists(pathFont + fileName))
                    {
                        try
                        {
                            if (File.ReadAllText(pathFont + fileName).Length > 0)
                            {
                                FontConverter fontConverter = new FontConverter();
                                treeView.Font = (Font)fontConverter.ConvertFromString(File.ReadAllText(pathFont + fileName));
                            }
                        }
                        catch (Exception e)
                        {
                            notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                        }
                    }
                    try
                    {
                        trackBarSizeFont.Value = (int)textBoxContent.Font.Size;
                    }
                    catch
                    {
                        trackBarSizeFont.Value = 30;
                    }
                #endregion
                //Auto lock
                #region
                    fileName = "AutoLock.txt";
                    if (File.Exists(path + fileName))
                {
                    try
                    {
                        if (File.ReadAllText(path + fileName).Length > 0)
                        {
                            checkBoxAutoLock.Checked = bool.Parse(File.ReadAllText(path + fileName));
                        }
                    }
                    catch (Exception e)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Reading " + fileName + " file failed!", e.ToString(), ToolTipIcon.Warning);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                textBoxContent.Text = "ERROR\r\n\r\n" + e.ToString();
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        //đóng form và lưu treeView data
        #region FormMain_FormClosing
            private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
            {
                timerAutoSave.Enabled = false;
                new FormLoad(passwordBytes, treeView, this, notifyIcon);//lưu dữ liệu
                FormFindAndReplace.Dispose();
                //lưu kích thước form
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\";
                try
                {
                    File.WriteAllText(path + "WindowState.txt", WindowState.ToString());
                    string[] str = new string[2] {Width.ToString(), Height.ToString() };
                    File.WriteAllLines(path + "Size.txt", str);
                }
                catch (Exception E)
                {
                    notifyIcon.ShowBalloonTip(1000, "Writing ShowTime.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                }
                //
                Dispose();//dispose để form login biết để out chương trình
            }
        #endregion

        //MenuStrip
        #region MenuStrip
            //xem thông tin phần cứng máy tính
            private void settingToolStripMenuItem_Click_1(object sender, EventArgs e)
            {
                try
                {
                    if (info == null)
                    {
                        Cursor = Cursors.AppStarting;
                        info = ShowComputer.Show();
                        Cursor = Cursors.Default;
                    }
                    else
                    {
                        textBoxContent.Text = info;
                        UnSelectedNode();
                    }
                }
                catch (Exception E)
                {
                    textBoxContent.Text = "Can't read your hardware infomation:\r\n\r\n" + E.ToString();
                    MessageBox.Show(E.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                }
            }
            //save
            private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
            {
                new FormLoad(passwordBytes, treeView, this, notifyIcon);//lưu dữ liệu
            }
        #endregion

        //các thao tác trên TreeNodes
        #region TreeNode

                //bắt sự kiện Click chuột vào một TreeNode
                #region treeView_NodeMouseClick
                    private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
                    {
                        //nếu treeView.SelectedNode ở trạng thái unlock mà chuyển sang một TreeNode khác BÊN NGOÀI treeView.SelectedNode thì lock nó với kiều kiện AutoLock == True
                        if (checkBoxAutoLock.Checked && treeView.SelectedNode != null && e.Node.Name != "!2||-@#x" && e.Node != treeView.SelectedNode && !SearchANode(e.Node, treeView.SelectedNode))
                            LockNodes(e.Node, TreeRoot);
                        //gán TreeNode đã chọn cho treeView.SelectedNode
                        if (treeView.SelectedNode != null)
                        {
                            treeView.SelectedNode.BackColor = Color.Gainsboro;
                            if (treeView.SelectedNode != cutedNode) treeView.SelectedNode.ForeColor = Color.DimGray;
                        }
                        treeView.SelectedNode = e.Node;
                        treeView.SelectedNode.BackColor = SystemColors.Highlight;
                        if (treeView.SelectedNode != cutedNode) treeView.SelectedNode.ForeColor = SystemColors.Window;
                        //hiện nội dung của node.
                        if (treeView.SelectedNode.Name == "!2||-@#x")
                            textBoxContent.Text = "";
                        else
                            ShowTreeNodeContent(e.Node);
                        //hiện MenuStrip và sét SelectedNode khi click chuột phải.
                        if (e.Button == MouseButtons.Right)
                        {
                            //nếu là root Node
                            if (treeView.SelectedNode.Name == "!2||-@#x")
                                        {
                                contextMenuStrip_TreeRoot.Show(MousePosition);
                                if (cutedNode == null)
                                    pasteToolStripMenuItem.Visible = false;
                                else
                                    pasteToolStripMenuItem.Visible = true;
                            }
                            //nếu là tree Node
                            else
                            {
                                //edit Content
                                #region
                                    editContentToolStripMenuItem.Enabled = false;
                                    try
                                    {
                                        if (textBoxContent.Text == treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(nodesCode))) editContentToolStripMenuItem.Enabled = true;
                                    }
                                    catch { }
                                    if (textBoxContent.Text == treeView.SelectedNode.Name) editContentToolStripMenuItem.Enabled = true;
                                #endregion
                                //lock / unlock
                                #region
                                    try
                                    {
                                        //nếu TreeNode được được lock
                                        if (treeView.SelectedNode.Name.Contains(lockCode) && !treeView.SelectedNode.Name.Contains(nodesCode))
                                        {
                                            locktoolStripMenuItem.Text = "Unlock                   (Alt + L)";
                                            locktoolStripMenuItem.Image = Properties.Resources.padlock_unlock_icon__1_;
                                        }
                                        //
                                        else
                                        {
                                            locktoolStripMenuItem.Text = "Lock                       (Alt + L)";
                                            locktoolStripMenuItem.Image = Properties.Resources.padlock_lock_icon__1_;
                                        }
                                    }
                                    catch
                                    {
                                        locktoolStripMenuItem.Text = "Lock                       (Alt + L)";
                                        locktoolStripMenuItem.Image = Properties.Resources.padlock_lock_icon__1_;
                                    }
                                #endregion
                                contextMenuStrip_TreeNode.Show(MousePosition);
                                //Change password
                                #region
                                    //chỉ cho phép đổi mật khẩu khi TreeNode đã được unlock
                                    if(treeView.SelectedNode.Name.Contains(nodesCode)) changePasswordToolStripMenuItem.Enabled = true;
                                    else changePasswordToolStripMenuItem.Enabled = false;
                                #endregion
                            }
                        }
                    }
                    private void WhenChoosingANode()
                    {

                    }
                    private bool SearchANode(TreeNode node, TreeNode root)
                    {
                        if(root.Nodes == null || root.Nodes.Count < 1)
                            return false;
                        foreach (TreeNode item in root.Nodes)
                        {
                            if(item == node) return true;
                            else if(SearchANode(node, item)) return true;
                        }
                        return false;
                    }
                    private void LockNodes(TreeNode node, TreeNode root)
                    {
                        if(SearchANode(node, root))
                        {
                            foreach (TreeNode a_node in root.Nodes)
                            {
                                if(node != a_node) LockNodes(node, a_node);
                            }
                        }
                        else
                            KillNodes(root);
                    }
                    private void KillNodes(TreeNode root)
                    {
                        if(root.Nodes == null && root.Name.Contains(nodesCode)) SaveAndLockTheUnlockedTreeNode(root);
                        foreach (TreeNode a_node in root.Nodes)
                        {
                            KillNodes(a_node);
                        }
                        if (root.Name.Contains(nodesCode))
                            SaveAndLockTheUnlockedTreeNode(root);
                    }
                #endregion

                //các thao tác trên bàn phím
                #region
                    private void treeView_KeyDown(object sender, KeyEventArgs e)
                    {
                        e.Handled = true;
                        //di chuyển lên top                                                         Page Up
                        if (treeView.SelectedNode != null && e.KeyCode == Keys.PageUp && treeView.SelectedNode.Index != 0)
                        {
                            TreeNode a = treeView.SelectedNode;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(0, a);
                            treeView.SelectedNode = a;
                        }
                        //di chuyển xuống bot                                                       Page Down
                        if(treeView.SelectedNode != null && treeView.SelectedNode.Parent != null && e.KeyCode == Keys.PageDown && treeView.SelectedNode.Index != treeView.SelectedNode.Parent.Nodes.Count - 1)
                        {
                            TreeNode a = treeView.SelectedNode;
                            int index = treeView.SelectedNode.Parent.Nodes.Count;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
                        }
                        //di chuyển lên                                                             Ctrl + Up
                        if (e.Control && e.KeyCode == Keys.Up && treeView.SelectedNode != null && treeView.SelectedNode.Index != 0)
                        {
                            TreeNode a = treeView.SelectedNode;
                            int index = treeView.SelectedNode.Index - 1;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
                        }
                        //di chuyển xuống                                                           Ctrl + Down
                        if (e.Control && e.KeyCode == Keys.Down && treeView.SelectedNode != null && treeView.SelectedNode.Parent != null && treeView.SelectedNode.Index != treeView.SelectedNode.Parent.Nodes.Count - 1)
                        {
                            TreeNode a = treeView.SelectedNode;
                            int index = treeView.SelectedNode.Index + 1;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
                        }
                        //chỉnh sửa một Tree node                                                   Ctrl + D or F3
                        if (((e.Control && e.KeyCode == Keys.D) || e.KeyCode == Keys.F3) && treeView.SelectedNode != null && textBoxContent.ReadOnly)
                        {
                            OpenEditContent_of_SelectedNode();
                        }
                        //tạo mới một Node                                                          Ctrl + N
                        if(e.Control && e.KeyCode == Keys.N)
                        {
                            CreateTreeNode();
                        }
                        //Rename                                                                    F2
                        if(treeView.SelectedNode != null && e.KeyCode == Keys.F2)
                        {
                            FormAdd formAdd = new FormAdd(MousePosition, treeView.SelectedNode.Parent, treeView.SelectedNode.Text);//đối tượng trả về nội dung người dùng nhập và kiểm tra, ngăn nhập tên giống nhau trong cùng một nhóm node con của node truyền vào.
                            if (formAdd.button)
                                treeView.SelectedNode.Text = formAdd.Text;
                        }
                        //Delete                                                                    Delete
                        if (treeView.SelectedNode != null && e.KeyCode == Keys.Delete && treeView.SelectedNode.Name != "!2||-@#x")
                        {
                            if (MessageBox.Show("Are you sure?", "Delete: " + treeView.SelectedNode.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
                            {
                                treeView.SelectedNode.Remove();
                                textBoxContent.Text = "";
                            }
                        }
                        //lock / Unlock                                                             Alt + L
                        if(treeView.SelectedNode != null && e.Alt && e.KeyCode == Keys.L)
                        {
                            LockOrUnlockNode();
                        }
                        //sao chép tên của node vào clipboard                                       Ctrl + 1
                        if(e.Control && e.KeyCode == Keys.D1 && treeView.SelectedNode != null)
                        {
                            if (treeView.SelectedNode.Text.Length > 0)
                            {
                                if (treeView.SelectedNode.Text.Length > 0)
                                {
                                    Clipboard.SetText(treeView.SelectedNode.Text);
                                    notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Text, ToolTipIcon.None);
                                }
                            }
                        }
                        //sao chép nội dung của node vào clipboard                                  Ctrl + 2
                        if (e.Control && e.KeyCode == Keys.D2 && treeView.SelectedNode != null)
                        {
                            if (treeView.SelectedNode.Name.Length > 0)
                            {
                                if (treeView.SelectedNode.Name.Length > 0)
                                {
                                    Clipboard.SetText(treeView.SelectedNode.Name);
                                    notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Name, ToolTipIcon.None);
                                }
                            }
                        }
                        //sao chép toàn bộ                                                          Ctrl + 3
                        if (e.Control && e.KeyCode == Keys.D3 && treeView.SelectedNode != null)
                        {
                            if (treeView.SelectedNode.Name.Length > 0 && treeView.SelectedNode.Text.Length > 0)
                            {
                                string str = treeView.SelectedNode.Text + "\r\n\r\n" + treeView.SelectedNode.Name;
                                if (str.Length > 0)
                                {
                                    Clipboard.SetText(str);
                                    notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Text + "\r\n\r\n" + treeView.SelectedNode.Name, ToolTipIcon.None);
                                }
                            }
                        }
                    }
                #endregion

                //tạo mới một TreeNode
                #region newToolStripMenuItem_Click
        private void newToolStripMenuItem_Click(object sender, EventArgs e)//tạo mới một nút.
                            {
                                CreateTreeNode();
                            }
                            private void toolStripMenuItem3_Click(object sender, EventArgs e)
                            {
                                CreateTreeNode();
                            }
                            private void CreateTreeNode()
                            {
                                FormAdd formAdd = new FormAdd(MousePosition, treeView.SelectedNode);//đối tượng trả về nội dung người dùng nhập và kiểm tra, ngăn nhập tên giống nhau trong cùng một nhóm node con của node truyền vào.
                                if (!formAdd.button) return;//nếu người dùng huỷ nhập.
                                TreeNode newNode = new TreeNode(formAdd.Text);
                                treeView.SelectedNode.Nodes.Add(newNode);
                                newNode.Parent.Expand();
                                treeView.SelectedNode.BackColor = Color.Gainsboro;
                                if (treeView.SelectedNode != cutedNode) treeView.SelectedNode.ForeColor = Color.DimGray;
                                treeView.SelectedNode = newNode;
                                treeView.SelectedNode.BackColor = SystemColors.Highlight;
                                treeView.SelectedNode.ForeColor = SystemColors.Window;
                                OpenEditting();
                            }
                #endregion

                //sửa tên một TreeNode
                #region editToolStripMenuItem_Click
                    private void editToolStripMenuItem_Click(object sender, EventArgs e)//sửa tên một nút.
                    {
                        FormAdd formAdd = new FormAdd(MousePosition, treeView.SelectedNode.Parent, treeView.SelectedNode.Text);//đối tượng trả về nội dung người dùng nhập và kiểm tra, ngăn nhập tên giống nhau trong cùng một nhóm node con của node truyền vào.
                        if (!formAdd.button) return;//nếu người dùng huỷ nhập.
                        treeView.SelectedNode.Text = formAdd.Text;
                    }
                #endregion

                //sửa nội dung một TreeNode
                #region edit content of a TreeNode
                    private void editContentToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        OpenEditting();
                    }
                    private void OpenEditting()
                    {
                        OpenEditContent_of_SelectedNode();
                    }
                    private void CloseEditting()
                    {
                        comboBoxSearch.Visible = true;
                        panel_buttonSave.Visible = false;
                        buttonCancel.Visible = false;
                        treeView.Enabled = true;
                        saveToolStripMenuItem1.Enabled = true;
                        textBoxContent.ReadOnly = true;
                        treeView.Select();
                    }
                    private void buttonCancel_Click_1(object sender, EventArgs e)
                    {
                        CloseEditting();
                        textBoxContent.Text = old_text;
                        old_text = "";
                    }
                    private void buttonSave_Click(object sender, EventArgs e)
                    {
                        SaveContent();
                    }
                    private bool CheckSpecialCharacter(string specialCharacter)
                    {
                        if (!textBoxContent.Text.Contains(specialCharacter)) return false;
                        new FormNotification("Not allowed use: \n\"" + specialCharacter + "\"", false);
                        //bôi đen đoạn chuỗi đặc biệt đó
                        textBoxContent.SelectionStart = textBoxContent.Text.IndexOf(specialCharacter);
                        textBoxContent.SelectionLength = specialCharacter.Length;
                        //click vào control textBoxContent
                        textBoxContent.ScrollToCaret();
                        textBoxContent.Select();
                        return true;
                    }
                    private void ShowTreeNodeContent(TreeNode node)
                    {
                        //nếu là TreeNode thường.
                        if (!node.Name.Contains(lockCode))
                            textBoxContent.Text = node.Name;
                        //nếu là UnlockedTreeNode
                        else if (node.Name.Contains(lockCode) && node.Name.Substring(0, node.Name.IndexOf(lockCode)).Contains(nodesCode)) 
                            textBoxContent.Text = node.Name.Substring(0, node.Name.IndexOf(nodesCode));
                        //nếu là LockedTreeNode
                        else                                
                            textBoxContent.Text = "Locked";
                        }
                #endregion

                //xoá một TreeNode
                #region deleteToolStripMenuItem_Click
                    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)//xoá một node.
                    {
                        if (treeView.SelectedNode.Name == "!2||-@#x") return;//ngăn xoá root.
                        if (MessageBox.Show("Are you sure?", "Delete: " + treeView.SelectedNode.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
                        {
                            treeView.SelectedNode.Remove();
                            textBoxContent.Text = "";
                        }
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
                                Clipboard.SetText(treeView.SelectedNode.Name);
                                notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Name, ToolTipIcon.None);
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
                                notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Text, ToolTipIcon.None);
                            }
                        }
                    }
                    //sao chép toàn bộ
                    private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        if(treeView.SelectedNode.Name.Length > 0 && treeView.SelectedNode.Text.Length > 0)
                        {
                            string str = treeView.SelectedNode.Text + "\r\n\r\n" + treeView.SelectedNode.Name;
                            if (str.Length > 0)
                            {
                                Clipboard.SetText(str);
                                notifyIcon.ShowBalloonTip(1000, "Successful copy!", treeView.SelectedNode.Text + "\r\n\r\n" + treeView.SelectedNode.Name, ToolTipIcon.None);
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
                            b.Nodes.Insert(0, a);
                            treeView.SelectedNode = a;
                        }
                    #endregion
                    //di chuyển TreeNode xuống cuối cùng
                    #region botToolStripMenuItem_Click
                        private void botToolStripMenuItem_Click(object sender, EventArgs e)
                        {
                            if (treeView.SelectedNode.Index == treeView.SelectedNode.Parent.Nodes.Count - 1) return;
                            TreeNode a = treeView.SelectedNode;
                            int index = treeView.SelectedNode.Parent.Nodes.Count;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
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
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
                        }
                    #endregion
                    //di chuyển TreeNode xuống 1 đơn vị
                    #region downToolStripMenuItem_Click
                        private void downToolStripMenuItem_Click(object sender, EventArgs e)
                        {
                            if (treeView.SelectedNode.Index == treeView.SelectedNode.Parent.Nodes.Count - 1) return;
                            TreeNode a = treeView.SelectedNode;
                            int index = treeView.SelectedNode.Index + 1;
                            TreeNode b = treeView.SelectedNode.Parent;
                            treeView.SelectedNode.Remove();
                            b.Nodes.Insert(index, a);
                            treeView.SelectedNode = a;
                        }
                    #endregion
                #endregion

                //cắt một TreeNode
                #region moveToolStripMenuItem_Click
                    //đối với tree Node
                    private void moveToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        //dán
                        if (moveToolStripMenuItem.Text == "Paste  (of cutting)")
                        {
                            if(cutedNode != treeView.SelectedNode)//kiểm tra xem có dán vào chính nó hay không
                            {
                                cutedNode.Remove();
                                cutedNode.Text = PreventDuplication(cutedNode.Text, treeView.SelectedNode);
                                treeView.SelectedNode.Nodes.Add(cutedNode);
                                treeView.SelectedNode.Expand();
                                treeView.SelectedNode.BackColor = Color.Gainsboro;
                                treeView.SelectedNode.ForeColor = Color.DimGray;
                                treeView.SelectedNode = cutedNode;
                                treeView.SelectedNode.BackColor = SystemColors.Highlight;
                                treeView.SelectedNode.ForeColor = SystemColors.Window;
                                ShowTreeNodeContent(treeView.SelectedNode);
                            }
                            cutedNode.ForeColor = Color.DimGray;
                            cutedNode = null;
                            moveToolStripMenuItem.Text = "Cut";
                        }
                        //cắt
                        else
                        {
                            treeView.SelectedNode.ForeColor = Color.LightSeaGreen;
                            moveToolStripMenuItem.Text = "Paste  (of cutting)";
                            cutedNode = treeView.SelectedNode;
                            treeView.SelectedNode = null;
                            cutedNode.BackColor = Color.Gainsboro;
                        }
                    }
                    //đối với root Node
                    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        cutedNode.Remove();
                        cutedNode.Text = PreventDuplication(cutedNode.Text, treeView.SelectedNode);
                        treeView.SelectedNode.Nodes.Add(cutedNode);
                        treeView.SelectedNode.Expand();
                        treeView.SelectedNode.BackColor = Color.Gainsboro;
                        treeView.SelectedNode.ForeColor = Color.DimGray;
                        treeView.SelectedNode = cutedNode;
                        treeView.SelectedNode.BackColor = SystemColors.Highlight;
                        treeView.SelectedNode.ForeColor = SystemColors.Window;
                        ShowTreeNodeContent(treeView.SelectedNode);
                        cutedNode.ForeColor = Color.DimGray;
                        cutedNode = null;
                        moveToolStripMenuItem.Text = "Cut";
                    }
                    //
                    private string PreventDuplication(string text, TreeNode ParentNode)
                    {
                        while (true)
                        {
                            again:
                            foreach (TreeNode a_node in ParentNode.Nodes)
                            {
                                if (a_node.Text == text)
                                {
                                    text += " - Copy";
                                    goto again;
                                }
                            }
                            return text;
                        }
                    }
                #endregion

                //sao chép một TreeNode
                #region
                    //đối với tree node
                    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        Copy_a_TreeNode();
                    }
                    //đối với tree root
                    private void toolStripMenuItem10_Click(object sender, EventArgs e)
                    {
                        Copy_a_TreeNode();
                    }
                    private void Copy_a_TreeNode()
                    {
                        //dán
                        if (copyToolStripMenuItem.Text == "Paste  (of copying)")
                        {
                            copiedNode = GetCopy_a_TreeNode(copiedNode);
                            copiedNode.Text = PreventDuplication(copiedNode.Text, treeView.SelectedNode);
                            treeView.SelectedNode.Nodes.Add(copiedNode);
                            treeView.SelectedNode.Expand();
                            treeView.SelectedNode.BackColor = Color.Gainsboro;
                            treeView.SelectedNode.ForeColor = Color.DimGray;
                            treeView.SelectedNode = copiedNode;
                            treeView.SelectedNode.BackColor = SystemColors.Highlight;
                            treeView.SelectedNode.ForeColor = SystemColors.Window;
                            ShowTreeNodeContent(treeView.SelectedNode);
                            copiedNode = null;
                            copyToolStripMenuItem.Text = "Copy";
                            toolStripMenuItem10.Text = "Copy";
                        }
                        //sao chép
                        else
                        {
                            copyToolStripMenuItem.Text = "Paste  (of copying)";
                            toolStripMenuItem10.Text = "Paste  (of copying)";
                            copiedNode = treeView.SelectedNode;
                        }
                    }
                    //đệ quy giúp sao chép một treenode
                    private TreeNode GetCopy_a_TreeNode(TreeNode root)
                    {
                        TreeNode new_root = new TreeNode(root.Text);
                        if (root.Name != "!2||-@#x")
                        {
                            //nếu là UnlockedNode
                            if (root.Name.Contains(nodesCode))
                            {
                                SaveAndLockTheUnlockedTreeNode(root);
                            }
                            //nếu là LockedNode
                            if (root.Name.Contains(lockCode))
                            {
                                new_root.Tag = root.Tag;
                                //nếu Node bị copy không có dữ liệu (có thể do đọc file trong quá trình khởi động chương trình thất bại)
                                if (new_root.Tag == null)
                                {
                                    new_root.Name = Application.StartupPath + "\\Data\\" + root.Name.Substring(0, root.Name.LastIndexOf(lockCode));
                                    return new_root;
                                }
                                //tạo thư mục để lưu file (nếu chưa có)
                                string path = Application.StartupPath + "\\Data\\";
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                //tạo tên file
                                string fileName = DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + MousePosition.X.ToString() + "-" + MousePosition.Y.ToString() + "-" + root.GetHashCode().ToString() + GetHashCode().ToString() + random.Next(-2147483648, 2147483647).ToString() + DateTime.Now.Ticks.ToString().Substring(12) + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".g";
                                //tiến hành lưu
                                WriteTheFile:
                                try
                                {
                                    File.WriteAllBytes(path + fileName, ((DataBytes)new_root.Tag).bytes);
                                }
                                catch (Exception E)
                                {
                                    textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
                                    if (MessageBox.Show("Can't write the file: " + fileName + "\n" + E.ToString(), "ERROR", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                                        goto WriteTheFile;
                                }
                                new_root.Name = fileName + lockCode;
                            }
                            //nếu là Node thường
                            else
                                new_root.Name = root.Name;
                        }
                        foreach (TreeNode a_node in root.Nodes)
                        {
                            new_root.Nodes.Add(GetCopy_a_TreeNode(a_node));
                        }
                        return new_root;
                    }
                #endregion

                //xổ ra tất cả các TreeNode con của các TreeNode cha
                #region expandAllItemsToolStripMenuItem_Click
                    private void expandAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        treeView.SelectedNode.ExpandAll();
            
                }
                    private void toolStripMenuItem4_Click(object sender, EventArgs e)
                    {
                        treeView.SelectedNode.ExpandAll();
                    }
                #endregion

                //???xổ ra tất cả các TreeNode con của các TreeNode cha
                #region
                    private void toolStripMenuItem5_Click(object sender, EventArgs e)
                    {
                        treeView.SelectedNode.Toggle();
                    }
                    private void toolStripMenuItem6_Click(object sender, EventArgs e)
                {
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
                        leftToolStripMenuItem1.Image = Properties.Resources._checked;
                        rightToolStripMenuItem1.Image = null;
                        treeView.ExpandAll();
                    }
                    private void toolStripMenuItem9_Click(object sender, EventArgs e)
                    {
                        treeView.RightToLeft = RightToLeft.Yes;
                        toolStripMenuItem8.Image = null;
                        toolStripMenuItem9.Image = Properties.Resources._checked;
                        leftToolStripMenuItem1.Image = null;
                        rightToolStripMenuItem1.Image = Properties.Resources._checked;
                        treeView.ExpandAll();
                    }
                    private void leftToolStripMenuItem1_Click(object sender, EventArgs e)
                    {
                        treeView.RightToLeft = RightToLeft.No;
                        toolStripMenuItem8.Image = Properties.Resources._checked;
                        toolStripMenuItem9.Image = null;
                        leftToolStripMenuItem1.Image = Properties.Resources._checked;
                        rightToolStripMenuItem1.Image = null;
                        treeView.ExpandAll();
                    }
                    private void rightToolStripMenuItem1_Click(object sender, EventArgs e)
                {
                    treeView.RightToLeft = RightToLeft.Yes;
                    toolStripMenuItem8.Image = null;
                    toolStripMenuItem9.Image = Properties.Resources._checked;
                    leftToolStripMenuItem1.Image = null;
                    rightToolStripMenuItem1.Image = Properties.Resources._checked;
                    treeView.ExpandAll();
                }
        #endregion

                //lock / unlock
                #region
                    private void toolStripMenuItem11_Click(object sender, EventArgs e)
                    {
                        /*

                        Khi chương trình chạy thì thuộc tính Tag của tất cả các LockedTreeNodes sẽ được thêm Databytes vào 
                        Databytes chứa dữ liệu của một file LockedTreeNode hoàn chỉnh
                        Khi chương trình lưu, các UnlockedTreeNodes sẽ được lưu vào file riêng của nó còn Name sẽ được đặt bằng path + lockCode để gộp chuỗi (tức lúc này TreeNode.Name = path + lockCode)
                        Khi chương trình lưu, các LockedTreeNodes.Name sẽ được đặt bằng path + lockCode để gộp chuỗi (tức lúc này TreeNode.Name = path + lockCode)
                        Trạng thái của Locked: TreeNode.Name = path + lockCode
                        Trạng thái của Unlocked: TreeNode.Name = text-content + nodesCode + text-nodes + pathCode + path + lockCode + password
                        Một TreeNode file sẽ lưu: passCode + text-content + nodesCode + text-nodes

                        */
                        LockOrUnlockNode();
                    }
                    private void LockOrUnlockNode()
                    {
                        string path;
                        byte[] bytes;
                        //Lock
                        #region
                        if (treeView.SelectedNode.Name.Contains(nodesCode) || !treeView.SelectedNode.Name.Contains(lockCode))
                        {
                            //nếu TreeNode đã được unlock: thì lock và lưu
                            if (treeView.SelectedNode.Name.Contains(lockCode))
                            {
                                SaveAndLockTheUnlockedTreeNode(treeView.SelectedNode);
                                textBoxContent.Text = "Locked";
                            }
                            //nếu TreeNode chưa được đặt lock: thì đặt lock và lưu nó lại
                            else
                            {
                                //tạo một mật khẩu mới
                                using (FormPassword formPassword = new FormPassword(new Point(MousePosition.X - 12, MousePosition.Y - 50), "Set a new password"))
                                {
                                    if (formPassword.GetPassword() == null)
                                        return;//nếu người dùng huỷ việc tạo mật khẩu thì out
                                    UseWaitCursor = true;
                                    //chuyển các TreeNodes con của TreeNode đó thành chuỗi		
                                    text_nodes = "";
                                    TreeNodeToString(treeView.SelectedNode);
                                    text_nodes += rightCode;
                                    //tạo thư mục để lưu file (nếu chưa có)
                                    path = Application.StartupPath + "\\Data\\";
                                    if (!Directory.Exists(path))
                                        Directory.CreateDirectory(path);
                                    //tạo tên file
                                    string fileName = DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + MousePosition.X.ToString() + "-" + MousePosition.Y.ToString() + "-" + treeView.SelectedNode.GetHashCode().ToString() + GetHashCode().ToString() + random.Next(-2147483648, 2147483647).ToString() + DateTime.Now.Ticks.ToString().Substring(12) + "-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".g";
                                    //tạo bytes dữ liệu của RIÊNG TreeNode đó
                                    bytes = GCoding.Encoding(Encoding.Unicode.GetBytes((passCode + treeView.SelectedNode.Name + nodesCode + text_nodes).ToCharArray()), formPassword.GetPassword());
                                    //tiến hành lưu
                                    WriteTheFile:
                                    try
                                    {
                                        File.WriteAllBytes(path + fileName, bytes);
                                    }
                                    catch (Exception E)
                                    {
                                        textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
                                        if (MessageBox.Show("Can't write the file: " + fileName + "\n" + E.ToString(), "ERROR", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                                            goto WriteTheFile;
                                    }
                                    //cập nhật lại TreeNode và xoá các TreeNodes con của nó
                                    textBoxContent.Text = "Locked successfully!";
                                    treeView.SelectedNode.Name = fileName + lockCode;
                                    treeView.SelectedNode.Tag = new DataBytes(bytes);
                                    treeView.SelectedNode.Nodes.Clear();
                                    UseWaitCursor = false;
                                }
                            }
                            return;
                        }
                        #endregion
                        //Unlock
                        #region 
                        UseWaitCursor = true;
                        //mở form đăng nhập
                        using (FormLog formLog = new FormLog("Unlock"))
                        {
                            if (formLog.GetPass() != null && formLog.is_Enter)
                            {
                                //lấy DataBytes
                                if (treeView.SelectedNode.Tag == null)//trường hợp không tìm thấy DataBytes (có nghĩa là việc đọc coded-text lúc khởi động chương trình thất bại) thì đọc lại
                                {
                                    //tạo thư mục để lưu file (nếu chưa có)
                                    path = Application.StartupPath + "\\Data\\";
                                    if (!Directory.Exists(path))
                                        Directory.CreateDirectory(path);
                                    string fileName = treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(lockCode));
                                    try
                                    {
                                        treeView.SelectedNode.Tag = new DataBytes(File.ReadAllBytes(path + fileName));
                                    }
                                    catch (Exception E)
                                    {
                                        textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
                                        MessageBox.Show("Can't write the file: " + fileName + "\n" + E.ToString(), "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                                        UseWaitCursor = false;
                                        return;
                                    }
                                }
                                bytes = ((DataBytes)treeView.SelectedNode.Tag).bytes;
                                //giải mã
                                string data = Encoding.Unicode.GetString(GCoding.Decoding(bytes, formLog.GetPass()));
                                if (data.Contains(passCode))//nếu nhập đúng password
                                {
                                    data = data.Substring(passCode.Length);//data lúc này chứa: text-content + nodesCode + text_nodes
                                    text_nodes = data.Substring(data.IndexOf(nodesCode) + nodesCode.Length);
                                    //Nạp các TreeNodes cho TreeNode đó
                                    int index = -leftCode.Length;
                                    while (true)
                                    {
                                        index += leftCode.Length;
                                        if (index == text_nodes.Length - 1 || text_nodes.Substring(index).IndexOf(rightCode) == 0)
                                            break;
                                        index = AddTreeNode(treeView.SelectedNode, index);//đệ quy
                                    }
                                    treeView.SelectedNode.Expand();
                                    //hiển thị text-content của TreeNode
                                    textBoxContent.Text = data.Substring(0, data.IndexOf(nodesCode));
                                    //đặt lại thuộc tính Name cho TreeNode
                                    treeView.SelectedNode.Name = data + pathCode + treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(lockCode) + lockCode.Length) + Encoding.Default.GetString(formLog.GetPass());
                                    treeView.SelectedNode.Tag = null;
                                }
                                else
                                    new FormNotification("Wrong password!", false);
                            }
                        }
                        UseWaitCursor = false;
                        #endregion
                    }
                    //đệ quy
                    #region
                    //Lấy các TreeNodes và nối thành chuỗi string
                    private void TreeNodeToString(TreeNode treeRoot)
                                    {
                                        if (treeRoot.Nodes == null) return;
                                        foreach (TreeNode nodes in treeRoot.Nodes)
                                        {
                                            text_nodes += nodes.Text + nameCode + nodes.Name + leftCode;
                                            TreeNodeToString(nodes);
                                            text_nodes += rightCode;
                                        }
                                    }
                                    //Lấy các TreeNodes từ chuỗi string
                                    private int AddTreeNode(TreeNode treeRoot, int indexCurrent)
                                    {
                                        int index = text_nodes.Substring(indexCurrent).IndexOf(leftCode) + indexCurrent;
                                        //tạo mới một TreeNode
                                        TreeNode treeNode = new TreeNode();
                                        string dataString2 = text_nodes.Substring(indexCurrent, index - indexCurrent);//đoạn string chứa dữ liệu của một TreeNode
                                        indexCurrent = dataString2.IndexOf(nameCode);
                                        treeNode.Text = dataString2.Substring(0, indexCurrent);//đoạn đầu của dataString2 chứa tên của TreeNode
                                        treeNode.Name = dataString2.Substring(indexCurrent + nameCode.Length);//đoạn còn lại của dataString2 chứa nội dung của TreeNode
                                        treeRoot.Nodes.Add(treeNode);
                                        //bắt đầu đệ quy
                                        while (true)
                                        {
                                            index += leftCode.Length;
                                            if (index == text_nodes.Length - 1 || text_nodes.Substring(index).IndexOf(rightCode) == 0) return index;
                                            index = AddTreeNode(treeNode, index);
                                        }
                                    }
                    #endregion
                #endregion

                //change password
                #region
                    private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        using (FormPassword formPassword = new FormPassword(new Point(MousePosition.X - 12, MousePosition.Y - 50), "Change password"))
                        {
                            if (formPassword.GetPassword() == null)
                                return;//nếu người dùng huỷ việc tạo mật khẩu thì out
                            treeView.SelectedNode.Name = treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(lockCode) + lockCode.Length) + Encoding.Default.GetString(formPassword.GetPassword());
                        }
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
        }
        #endregion

        //comboBoxSearch
        #region comboBoxSearch
            private void comboBoxSearch_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Command();
                }
            }
            private void Command()
        {
            if (comboBoxSearch.Text == "opencurrentfolder")
            {
                Process.Start(Directory.GetCurrentDirectory());
                comboBoxSearch.Text = "";
            }
            else if (comboBoxSearch.Text == "openspecialfolder")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge";
                if (Directory.Exists(path))
                    Process.Start(path);
                else
                    MessageBox.Show("The path: \"" + path + "\" does not exist");
                comboBoxSearch.Text = "";
            }
        }
        #endregion

        //Cài đặt
        #region Setting

            //đóng mở panelSetting
            #region
                private void settingToolStripMenuItem1_Click(object sender, EventArgs e)
                {
                    //if (!panelSetting.Enabled) return;
                    //đóng cài đặt
                    if(panelSetting_is_open)
                    {
                        panelSetting_is_open = false;
                        if (checkBox_TurboBoost.Checked)
                            panelSetting.Left = -210;
                        else
                            new AnimationMOVE(panelSetting, -210, false);
                    }
                    //mở cài đặt
                    else
                    {
                        panelSetting_is_open = true;
                        if (checkBox_TurboBoost.Checked)
                        {
                            panelSetting.Left = 0;
                            buttonFocus.Select();
                        }
                        else
                            new AnimationMOVE(panelSetting, 0, true);
                    }
                }
            #endregion
            //đóng panelSetting khi bị outfocus
            #region
                private void panelSetting_Leave(object sender, EventArgs e)
                {
                    if (panelSetting.Left == 0 && panelSetting.Enabled)
                    {
                        //đóng cài đặt
                        panelSetting_is_open = false;
                        if (checkBox_TurboBoost.Checked)
                            panelSetting.Left = -210;
                        else
                            new AnimationMOVE(panelSetting, -210, false);
                    }
                }
                private void panelSetting_LocationChanged(object sender, EventArgs e)
            {
                if (panelSetting.Left == 0) buttonFocus.Select();
            }
            #endregion
            //Change password
            #region
                private void buttonChangePassword_Click(object sender, EventArgs e)
                {
                    CheckPassword();
                }
                private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Enter) CheckPassword();
                    if (e.Control && e.KeyCode == Keys.C)
                        if (textBoxPassword.TextLength > 0) Clipboard.SetText(textBoxPassword.Text);
                    if (e.Control && e.KeyCode == Keys.A)
                        textBoxPassword.SelectAll();
                }
                private void CheckPassword()
                {
                    if (textBoxPassword.Text.Length == 0)
                    {
                        new FormNotification("Empty password!", false);
                        return;
                    }
                    //nhập đúng mật khẩu
                    if (Encoding.ASCII.GetString(passwordBytes) == textBoxPassword.Text)
                    {
                        FormPassword formPassword = new FormPassword(new Point(Left + 30, Top + 350), "Enter a new password");
                        if (formPassword.GetPassword() != null)
                        {
                            passwordBytes = formPassword.GetPassword();
                            new FormLoad(passwordBytes, treeView, this, notifyIcon);//lưu dữ liệu
                            notifyIcon.ShowBalloonTip(1000, "Change password successfully", " ", ToolTipIcon.None);
                        }
                    }
                    //nhập sai mật khẩu
                    else
                        new FormNotification("Wrong password!", false);
                    textBoxPassword.Text = "";
                }
            #endregion
            //CheckBoxs
            #region
                //checkBox_StartWithWindows
                private void checkBox_StartWithWindows_CheckedChanged(object sender, EventArgs e)
                {
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    if (checkBox_StartWithWindows.Checked)
                    {
                        if (registryKey.GetValue("My Knowledge") == null)
                            registryKey.SetValue("My Knowledge", Application.ExecutablePath);
                    }
                    else
                    {
                        if (registryKey.GetValue("My Knowledge") != null)
                            registryKey.DeleteValue("My Knowledge");
                    }

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\StartWithWindows.txt";
                    try
                    {
                        File.WriteAllText(path, checkBox_StartWithWindows.Checked.ToString());
                    }
                    catch (Exception E)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Writing StartWithWindows.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                    }
                }
                //checkBox_TurboBoost
                private void checkBox_TurboBoost_CheckedChanged(object sender, EventArgs e)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\TurboBoost.txt";
                    try
                    {
                        File.WriteAllText(path, checkBox_TurboBoost.Checked.ToString());
                    }
                    catch (Exception E)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Writing TurboBoost.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                    }
                }
                //ShowTime
                private void checkBoxShowTime_CheckedChanged(object sender, EventArgs e)
                {
                    if (checkBoxShowTime.Checked)
                    {
                        Text = "[ " + DateTime.Now.ToLongTimeString() + " ] - My Knowledge";
                        timer.Enabled = true;
                    }
                    else
                    {
                        Text = "My Knowledge";
                        timer.Enabled = false;
                    }

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\ShowTime.txt";
                    try
                    {
                        File.WriteAllText(path, checkBoxShowTime.Checked.ToString());
                    }
                    catch (Exception E)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Writing ShowTime.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                    }
                }
            //Auto lock
            private void checkBoxAotoLock_CheckedChanged(object sender, EventArgs e)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\AutoLock.txt";
                try
                {
                    File.WriteAllText(path, checkBoxAutoLock.Checked.ToString());
                }
                catch (Exception E)
                {
                    notifyIcon.ShowBalloonTip(1000, "Writing AutoLock.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                }
            }
            #endregion
            //Change fonts
            #region
            //change font of textBoxContent
            private void buttonChangeFont_textbox_Click(object sender, EventArgs e)
                    {
                        FontDialog fontDialog = new FontDialog();
                        fontDialog.Font = textBoxContent.Font;
                        fontDialog.ShowDialog();
                        if (fontDialog.Font != textBoxContent.Font)
                        {
                            textBoxContent.Font = fontDialog.Font;
                            try
                            {
                                trackBarSizeFont.Value = (int)textBoxContent.Font.Size;
                            }
                            catch
                            {
                                trackBarSizeFont.Value = 30;
                            }
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\Font";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            try
                            {
                                FontConverter fontConverter = new FontConverter();
                                File.WriteAllText(path + "\\textBoxContent.txt", fontConverter.ConvertToString(textBoxContent.Font));
                            }
                            catch (Exception E)
                            {
                                notifyIcon.ShowBalloonTip(1000, "Writing textBoxContent.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                            }
                        }
                    }
                    //change font of treeView
                    private void buttonChangeFont_listitems_Click(object sender, EventArgs e)
                    {
                        FontDialog fontDialog = new FontDialog();
                        fontDialog.Font = treeView.Font;
                        fontDialog.ShowDialog();
                        if (fontDialog.Font != treeView.Font)
                        {
                            treeView.Font = fontDialog.Font;
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\Font";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            try
                            {
                                FontConverter fontConverter = new FontConverter();
                                File.WriteAllText(path + "\\treeView.txt", fontConverter.ConvertToString(treeView.Font));
                            }
                            catch (Exception E)
                            {
                                notifyIcon.ShowBalloonTip(1000, "Writing treeView.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                            }
                        }
                    }
                    //ẩn - hiện label_Font_textBoxContent khi rê chuột vào
                    private void buttonChangeFont_textbox_MouseEnter(object sender, EventArgs e)
                    {
                        label_Font_textBoxContent.Visible = true;
                    }
                    private void buttonChangeFont_textbox_MouseLeave(object sender, EventArgs e)
                    {
                        label_Font_textBoxContent.Visible = false;
                    }
                    //ẩn - hiện label_Font_treeView khi rê chuột vào
                    private void buttonChangeFont_listitems_MouseEnter(object sender, EventArgs e)
                    {
                        label_Font_treeView.Visible = true;
                    }
                    private void buttonChangeFont_listitems_MouseLeave(object sender, EventArgs e)
                {
                    label_Font_treeView.Visible = false;
                }
                #endregion
            //AutoSave
            #region
                //checkBoxAutoSave_CheckedChanged
                private void checkBoxAutoSave_CheckedChanged(object sender, EventArgs e)
                        {
                            timerAutoSave.Enabled = checkBoxAutoSave.Checked;
                            //mở
                            if (checkBoxAutoSave.Checked)
                            {
                                if (checkBox_TurboBoost.Checked)
                                {
                                    labelAutoSave.Top = 0;
                                    panel_trackBarAutoSave.Top = 14;
                                }
                                else
                                {
                                    new AnimationMOVE(labelAutoSave, 0, false, true, 0, 17);
                                    new AnimationMOVE(panel_trackBarAutoSave, 14, false, true, 90, 17);
                                }
                            }
                            //đóng
                            else
                            {
                                if (checkBox_TurboBoost.Checked)
                                {
                                    labelAutoSave.Top = 58;
                                    panel_trackBarAutoSave.Top = 73;
                                }
                                else
                                {
                                    new AnimationMOVE(labelAutoSave, 58, true, true, 150, 19);
                                    new AnimationMOVE(panel_trackBarAutoSave, 73, true, true, 0, 19);
                                }
                            }
                            //save file
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\AutoSave";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            try
                            {
                                File.WriteAllText(path + "\\AutoSave.txt", checkBoxAutoSave.Checked.ToString());
                            }
                            catch (Exception E)
                            {
                                notifyIcon.ShowBalloonTip(1000, "Writing AutoSave.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                            }
                        }
                //trackBarAutoSave_Scroll
                private void trackBarAutoSave_Scroll(object sender, EventArgs e)
                        {
                            labelAutoSave.Text = "Saving per " + trackBarAutoSave.Value + " minutes";
                            timerAutoSave.Interval = trackBarAutoSave.Value * 60000;
                            //save file
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\AutoSave";
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            try
                            {
                                File.WriteAllText(path + "\\Value.txt", trackBarAutoSave.Value.ToString());
                            }
                            catch (Exception E)
                            {
                                notifyIcon.ShowBalloonTip(1000, "Writing Value.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                            }
                        }
            #endregion
            //nút thông tin
            #region
                private void button1_Click(object sender, EventArgs e)
                {
                    //đóng cài đặt
                    panelSetting_is_open = false;
                    if (checkBox_TurboBoost.Checked)
                        panelSetting.Left = -210;
                    else
                        new AnimationMOVE(panelSetting, -210, false);
                    //
                    UnSelectedNode();
                    //
                    textBoxContent.Text = button1.Name;//hiển thị thông tin phần mềm
                }
            #endregion
            //nút facebook
            #region
                private void buttonFB_Click(object sender, EventArgs e)
                {
                    Process.Start("https://www.facebook.com/gianglee1998");
                    WindowState = FormWindowState.Minimized;
                }
            #endregion

        #endregion

        //mọi thứ về textBoxContent
        #region textBoxContent

            //thanh chỉnh size font nhanh
            #region trackBarSizeFont
                private void trackBarSizeFont_Scroll(object sender, EventArgs e)
                    {
                        textBoxContent.Font = new Font(textBoxContent.Font.FontFamily, trackBarSizeFont.Value, textBoxContent.Font.Style);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Knowledge\\Font";
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        try
                        {
                            FontConverter fontConverter = new FontConverter();
                            File.WriteAllText(path + "\\textBoxContent.txt", fontConverter.ConvertToString(textBoxContent.Font));
                        }
                        catch (Exception E)
                        {
                            notifyIcon.ShowBalloonTip(1000, "Writing textBoxContent.txt file failed!", E.ToString(), ToolTipIcon.Warning);
                        }
                    }
            #endregion

            //context menu strip
            #region contextMenuStrip_textBoxContent

                //show context menu strip
                #region show
                    private void textBoxContent_MouseDown(object sender, MouseEventArgs e)
                    {
                        if (e.Button == MouseButtons.Left && textBoxContent.TextLength == 0) treeView.Select();
                        if (e.Button == MouseButtons.Right)
                        {
                            //undo
                            if (textBoxContent.CanUndo) toolStripMenuItem_Undo.Enabled = true;
                            else toolStripMenuItem_Undo.Enabled = false;
                            //save
                            if (!textBoxContent.ReadOnly) toolStripMenuItem_save.Enabled = true;
                            else toolStripMenuItem_save.Enabled = false;
                            //edit
                            toolStripMenuItem_Edit.Enabled = false;
                            if (treeView.SelectedNode != null && textBoxContent.ReadOnly)
                            {
                                try
                                {
                                    if (textBoxContent.Text == treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(nodesCode))) toolStripMenuItem_Edit.Enabled = true;
                                }
                                catch { }
                                if(textBoxContent.Text == treeView.SelectedNode.Name) toolStripMenuItem_Edit.Enabled = true;
                            }
                            //cut
                            if(textBoxContent.SelectedText.Length == 0 || textBoxContent.ReadOnly) toolStripMenuItem_Cut.Enabled = false;
                            else toolStripMenuItem_Cut.Enabled = true;
                            //copy
                            if (textBoxContent.SelectedText.Length == 0) toolStripMenuItem_Copy.Enabled = false;
                            else toolStripMenuItem_Copy.Enabled = true;
                            //paste
                            if (textBoxContent.ReadOnly || Clipboard.GetText().Length == 0) toolStripMenuItem_Paste.Enabled = false;
                            else toolStripMenuItem_Paste.Enabled = true;
                            //cut rows
                            if (textBoxContent.ReadOnly) cutRowsToolStripMenuItem.Enabled = false;
                            else cutRowsToolStripMenuItem.Enabled = true;
                            //select all
                            if (textBoxContent.TextLength == 0) selectAllToolStripMenuItem.Enabled = false;
                            else selectAllToolStripMenuItem.Enabled = true;
                            //get
                            if (textBoxContent.ReadOnly) getToolStripMenuItem.Enabled = false;
                            else getToolStripMenuItem.Enabled = true;
                            //find and replace
                            if (FormFindAndReplace.Enabled) findCtrlFToolStripMenuItem.Enabled = false;
                            else findCtrlFToolStripMenuItem.Enabled = true;
                            //Run the highlighting text
                            if (textBoxContent.SelectedText.Length == 0) runTheTextToolStripMenuItem.Enabled = false;
                            else runTheTextToolStripMenuItem.Enabled = true;
                            if (textBoxContent.ReadOnly) runTheTextToolStripMenuItem.Text = "Run the highlighting text   (R)";
                            else runTheTextToolStripMenuItem.Text = "Run the highlighting text   (Ctrl + R)";
                        }
                    }
                #endregion

                //các thao tác bàn phím
                #region textBoxContent_KeyDown
                    private void textBoxContent_KeyDown(object sender, KeyEventArgs e)
                    {
                        e.Handled = true;
                        //WordWrap
                        if (e.Control && e.KeyCode == Keys.W)
                        {
                            if (textBoxContent.WordWrap)
                            {
                                wordWrapToolStripMenuItem.Image = Properties.Resources._checked;
                                textBoxContent.WordWrap = false;
                            }
                            else
                            {
                                wordWrapToolStripMenuItem.Image = null;
                                textBoxContent.WordWrap = true;
                            }
                        }
                        //Cancel
                        if(!textBoxContent.ReadOnly && e.KeyCode == Keys.Escape)
                        {
                            CloseEditting();
                            textBoxContent.Text = old_text;
                            old_text = "";
                        }
                        //Run the highlighting text                                         - Ctrl + R or R
                        if (((e.Control && e.KeyCode == Keys.R && !textBoxContent.ReadOnly) || (e.KeyCode == Keys.R && textBoxContent.ReadOnly)) && textBoxContent.SelectedText.Length != 0)
                        {
                            try
                            {
                                Process.Start(textBoxContent.SelectedText);
                            }
                            catch
                            {
                                Process.Start("https://www.google.com/search?q=" + textBoxContent.SelectedText);
                            }
                        }
                        //Find and Replace                                                  - Ctrl + F
                        if (e.Control && e.KeyCode == Keys.F && !FormFindAndReplace.Enabled)
                        {
                            Show_FormFindAndReplace();
                        }
                        //xoá các dòng đã chọn                                              - Ctrl + L
                        if (e.Control && e.KeyCode == Keys.L && !textBoxContent.ReadOnly)
                        {
                            CutRows();
                        }
                        //lưu                                                               - Ctrl + S
                        if (e.Control && e.KeyCode == Keys.S && !textBoxContent.ReadOnly)
                        {
                            SaveContent();
                        }
                        //bôi đen toàn bộ văn bản                                           - Ctrl + A
                        if (e.Control && e.KeyCode == Keys.A)
                        {
                            textBoxContent.SelectAll();
                        }
                        //chỉnh sửa một Tree node                                           - Ctrl + D
                        if (e.Control && e.KeyCode == Keys.D && treeView.SelectedNode != null && textBoxContent.ReadOnly)
                        {
                            OpenEditContent_of_SelectedNode();
                        }
                        //căn lề bằng
                        if (e.KeyCode == Keys.Enter && !textBoxContent.ReadOnly && textBoxContent.SelectionStart > 0)
                        {
                        string str = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart);
                        int index = str.LastIndexOf("\r\n");
                        if (index == -1) index = 0;
                        else index += 2;
                        StringBuilder stringBuilder = new StringBuilder(str.Substring(index));
                        string str2 = "";
                        for (int i = 0; i < stringBuilder.Length; i++)
                        {
                            if (stringBuilder.ToString(i, 1).Contains("\t")) str2 += "\t";
                            else break;
                        }
                        SetClipboardAndPaste(str2);
                    }
                        //tab 2 dòng trở lên
                        if (e.KeyCode == Keys.Tab && textBoxContent.SelectedText.Contains("\r\n") && !textBoxContent.ReadOnly)
                            {
                                SeleteRows();
                                string str = textBoxContent.SelectedText.Replace("\r\n", "\r\n\t");
                                int index = textBoxContent.SelectionStart;
                                SetClipboardAndPaste(str);
                                textBoxContent.SelectionStart = index + 1;
                                textBoxContent.SelectionLength = str.Length;
                            }
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
                        if (CheckSpecialCharacter(nodesCode) || CheckSpecialCharacter(pathCode) || CheckSpecialCharacter(leftCode) || CheckSpecialCharacter(lockCode) || CheckSpecialCharacter(nameCode) || CheckSpecialCharacter(rightCode)) return;
                        CloseEditting();
                        //nếu TreeNode đó đang ở trạng thái unlocked thì lưu theo kiểu "locked"
                        if (treeView.SelectedNode.Name.Contains(nodesCode))
                            treeView.SelectedNode.Name = textBoxContent.Text + treeView.SelectedNode.Name.Substring(treeView.SelectedNode.Name.IndexOf(nodesCode));
                        //nếu không thì lưu theo kiểu bình thường
                        else
                            treeView.SelectedNode.Name = textBoxContent.Text;
                    }
                #endregion

                //edit
                #region toolStripMenuItem_Edit
                    private void toolStripMenuItem_Edit_Click(object sender, EventArgs e)
                    {
                        OpenEditContent_of_SelectedNode();
                    }
                    private void OpenEditContent_of_SelectedNode()
                    {
                        if (treeView.SelectedNode == null) return;
                        try
                        {
                            if (textBoxContent.Text == treeView.SelectedNode.Name || textBoxContent.Text == treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(nodesCode)))
                            {
                                comboBoxSearch.Visible = false;
                                panel_buttonSave.Visible = true;
                                buttonCancel.Visible = true;
                                treeView.Enabled = false;
                                saveToolStripMenuItem1.Enabled = false;
                                textBoxContent.ReadOnly = false;
                                textBoxContent.Text = treeView.SelectedNode.Name;
                                old_text = textBoxContent.Text;
                                if(treeView.SelectedNode.Name.Contains(nodesCode)) textBoxContent.Text = treeView.SelectedNode.Name.Substring(0, treeView.SelectedNode.Name.IndexOf(nodesCode));
                                old_text = textBoxContent.Text;
                                textBoxContent.Select();
                            }
                        }
                        catch { }
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
                    if(textBoxContent.SelectionLength > 0) Clipboard.SetText(textBoxContent.SelectedText);
                }
                #endregion

                //paste
                #region
                    private void toolStripMenuItem_Paste_Click(object sender, EventArgs e)
                {
                    textBoxContent.Paste(Clipboard.GetText());
                }
                #endregion

                //cut rows
                #region
                    private void cutRowsToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        CutRows();
                    }
                    private void CutRows()
                    {
                        SeleteRows();
                        SendKeys.Send("^X");
                    }
                    private void SeleteRows()
                    {
                        int indexStart;
                        if(textBoxContent.SelectedText.Length > 2) indexStart = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart + 2).LastIndexOf("\r\n");
                        else indexStart = textBoxContent.Text.Substring(0, textBoxContent.SelectionStart).LastIndexOf("\r\n");
                        int index = textBoxContent.SelectionStart + textBoxContent.SelectionLength;
                        int indexEnd = textBoxContent.Text.Substring(index).IndexOf("\r\n");
                        if (indexStart == -1) indexStart = 0;
                        if (indexStart == 0) indexEnd += 2;
                        if (indexEnd == -1) indexEnd = textBoxContent.Text.Length;
                        else indexEnd += index;
                        textBoxContent.SelectionStart = indexStart;
                        textBoxContent.SelectionLength = indexEnd - indexStart;
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

                //word wrap
                #region
                    private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
                        {
                            if (textBoxContent.WordWrap)
                            {
                                wordWrapToolStripMenuItem.Image = Properties.Resources._checked;
                                textBoxContent.WordWrap = false;
                            }
                            else
                            {
                                wordWrapToolStripMenuItem.Image = null;
                                textBoxContent.WordWrap = true;
                            }
                        }
                #endregion

                //get
                #region
                    //Date
                    private void dateToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        SetClipboardAndPaste(DateTime.Now.ToShortDateString());
                    }
                    //Day of Week
                    private void dayOfWeekToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        SetClipboardAndPaste(DateTime.Now.DayOfWeek.ToString());
                    }
                    //Time
                    private void timeToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        SetClipboardAndPaste(DateTime.Now.ToLongTimeString());
                    }
                    //DateTime
                    private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        SetClipboardAndPaste(DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToLongDateString());
                    }
                    //Path of a File
                    private void partToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName.Length > 0)
                        {
                            SetClipboardAndPaste(openFileDialog.FileName);
                        }
                    }
                    //Path of a Folder
                    private void pathOfAFolderToolStripMenuItem_Click(object sender, EventArgs e)
                    {
                        FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
                        if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.SelectedPath.Length > 0)
                        {
                            SetClipboardAndPaste(openFileDialog.SelectedPath);
                        }
                    }
                #endregion

                //text alignment
                #region
                    private void leftToolStripMenuItem_Click(object sender, EventArgs e)
                                        {
                                            textBoxContent.TextAlign = HorizontalAlignment.Left;
                                            rightToolStripMenuItem.Image = null;
                                            leftToolStripMenuItem.Image = Properties.Resources._checked;
                                            centerToolStripMenuItem.Image = null;
                                        }
                    private void rightToolStripMenuItem_Click(object sender, EventArgs e)
                                        {
                                            textBoxContent.TextAlign = HorizontalAlignment.Right;
                                            rightToolStripMenuItem.Image = Properties.Resources._checked;
                                            leftToolStripMenuItem.Image = null;
                                            centerToolStripMenuItem.Image = null;

                                        }
                    private void centerToolStripMenuItem_Click(object sender, EventArgs e)
                                        {
                                            textBoxContent.TextAlign = HorizontalAlignment.Center;
                                            rightToolStripMenuItem.Image = null;
                                            leftToolStripMenuItem.Image = null;
                                            centerToolStripMenuItem.Image = Properties.Resources._checked;
                                        }
                #endregion

                //Run the highlighting text
                #region
                    private void runTheTextToolStripMenuItem_Click_1(object sender, EventArgs e)
                        {
                            if (textBoxContent.SelectedText.Length > 0)
                            {
                                try
                                {
                                    Process.Start(textBoxContent.SelectedText);
                                }
                                catch
                                {
                                    Process.Start("https://www.google.com/search?q=" + textBoxContent.SelectedText);
                                }
                            }
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

            #endregion

        #endregion

        //Notifyicon
        #region Notifyicon
            //hiển thị show form khi double click
                private void notifyIcon1_DoubleClick(object sender, EventArgs e)
            {
                if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
                Activate();
            }
            //contextMenuStrip_notifyicon
            #region
                //show Your Hardware
                private void showYourHardwareToolStripMenuItem_Click(object sender, EventArgs e)
                {
                    //đóng cài đặt
                    if (panelSetting.Left == 0)
                    {
                        if (checkBox_TurboBoost.Checked)
                            panelSetting.Left = -210;
                        else
                            new AnimationMOVE(panelSetting, -210, false);
                    }
                    //
                    if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
                    Activate();
                    try
                    {
                        Cursor = Cursors.AppStarting;
                        if (info == null) info = ShowComputer.Show();
                        else
                        {
                            textBoxContent.Text = info;
                            UnSelectedNode();
                        }
                    }
                    catch (Exception E)
                    {
                        textBoxContent.Text = "Can't read your hardware infomation:\r\n\r\n" + E.ToString();
                        MessageBox.Show(E.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Cursor = Cursors.Default;
                }
                //Save
                private void saveToolStripMenuItem_Click(object sender, EventArgs e)
                {
                    new FormLoad(passwordBytes, treeView, this, notifyIcon);//lưu dữ liệu
                }
                //About
                private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
                {
                    //đóng cài đặt
                    if (panelSetting.Left == 0)
                    {
                        if (checkBox_TurboBoost.Checked)
                            panelSetting.Left = -210;
                        else
                            new AnimationMOVE(panelSetting, -210, false);
                    }
                    //
                    UnSelectedNode();
                    //
                    if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
                    Activate();
                    textBoxContent.Text = button1.Name;
                }
                //Setting
                private void settingToolStripMenuItem2_Click(object sender, EventArgs e)
                {
                    if (panelSetting.Left != 0)
                    {
                        //mở cài đặt
                        if (checkBox_TurboBoost.Checked)
                        {
                            panelSetting.Left = 0;
                            buttonFocus.Select();
                        }
                        else
                            new AnimationMOVE(panelSetting, 0, true);
                    }
                    if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
                    Activate();
                }
                //Logout
                private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
                {
                    Close();
                    Process.Start(Application.ExecutablePath);
                }
                //exit
                private void exitToolStripMenuItem_Click(object sender, EventArgs e)
                {
                    Close();
                }
        #endregion
        #endregion

        //các timers
        #region
            //auto save
                private void timerAutoSave_Tick(object sender, EventArgs e)
                {
                    if(saveToolStripMenuItem1.Enabled) new FormLoad(passwordBytes, treeView, this, notifyIcon);//lưu dữ liệu
                }
            //time
            private void timer_Tick(object sender, EventArgs e)
        {
            Text = "[ " + DateTime.Now.ToLongTimeString() + " ] - My Knowledge";
        }
        #endregion

        //chung
        #region
            private void SetClipboardAndPaste(string text)
            {
                string old_str = Clipboard.GetText();
                if(text.Length > 0)
                {
                    Clipboard.SetText(text);
                    SendKeys.SendWait("^V");
                    if (old_str.Length > 0) Clipboard.SetText(old_str);
                    else Clipboard.Clear();
                }
            }
            private void UnSelectedNode()
        {
            if(treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BackColor = Color.Gainsboro;
                if (treeView.SelectedNode != cutedNode) treeView.SelectedNode.ForeColor = Color.DimGray;
                treeView.SelectedNode = null;
            }
        }
            private void SaveAndLockTheUnlockedTreeNode(TreeNode node)
            {
                UseWaitCursor = true;
                byte[] password = Encoding.Default.GetBytes(node.Name.Substring(node.Name.LastIndexOf(lockCode) + lockCode.Length).ToCharArray());
                string text_content = node.Name.Substring(0, node.Name.IndexOf(nodesCode));
                //đưa TreeNode về trạng thái tiền Locked (TreeNode.Name = path + lockCode)
                node.Name = node.Name.Substring(node.Name.LastIndexOf(pathCode) + pathCode.Length, node.Name.LastIndexOf(lockCode) + lockCode.Length - node.Name.LastIndexOf(pathCode) - pathCode.Length);
                //bắt đầu tiền hành các bước để lưu dữ liệu và lock TreeNode lại
                //tạo thư mục để lưu file (nếu chưa có)
                string path = Application.StartupPath + "\\Data\\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                //lấy đường dẫn để lưu TreeNode
                string fileName = node.Name.Substring(0, node.Name.LastIndexOf(lockCode));
                //chuyển các TreeNodes con của TreeNode đó thành chuỗi		
                text_nodes = "";
                TreeNodeToString(node);
                text_nodes += rightCode;
                //tạo bytes dữ liệu coded_text
                byte[] bytes = GCoding.Encoding(Encoding.Unicode.GetBytes((passCode + text_content + nodesCode + text_nodes).ToCharArray()), password);
                //tiến hành lưu
                WriteTheFile:
                try
                {
                    File.WriteAllBytes(path + fileName, bytes);
                }
                catch (Exception E)
                {
                    textBoxContent.Text = "Can't write the file:\t" + path + fileName + "\r\n\r\n" + E.ToString();
                    if (MessageBox.Show("Can't write the file: " + fileName + "\n" + E.ToString(), "ERROR", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        goto WriteTheFile;
                }
                //đưa node về trạng thái locked
                textBoxContent.Text = "";
                node.Tag = new DataBytes(bytes);
                node.Nodes.Clear();
                UseWaitCursor = false;
            }

        #endregion

        private void textBoxContent_Leave(object sender, EventArgs e)
        {
            if(textBoxContent.ReadOnly) return;
            Text ="out";
        }
    }
}
