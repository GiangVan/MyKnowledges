using System;
using System.IO;
//using System.Threading;
using System.Windows.Forms;
using System.Text;

namespace YourExperience
{
	public partial class FormLoad : Form
	{
        //ký tự được biệt dùng để tách dữ liệu
        private const string lockCode = "!2||-@#l";
        private const string nodesCode = "!2||-@#ns";
        private const string leftCode = "!2||-@#(";
		private const string rightCode = ")#@-||2!";
        private const string pathCode = "!2||-@#p";
        private const string nameCode = "!2||-@#x";
        private const string passCode = "!)}2#$&^( 11/07/1998 )@#$%^&($( GiangVan )%#$&*||-@#p";//dùng để nhận biết rằng người dùng đã nhập đúng password hay chưa
        //
        private string path = Application.StartupPath + "\\data.g";

        private byte U = new byte();
		private bool Done = false;//dùng để đóng form khi đọc hoặc lưu dữ liệu xong
		private TreeView treeView;


        private byte[] password;
        static private byte[] passwordBytes;
        static private string dataString = null;
        string savingString = "";

        internal bool password_is_wrong = new bool();

        private bool run_ShowComputer;
        internal string info;
        NotifyIcon notifyIcon;


        //constructor đọc dữ liệu
        public FormLoad(byte[] password, TreeView treeView, bool run_ShowComputer, Form FormLogin)
		{
            InitializeComponent();
            Left = FormLogin.Left + FormLogin.Width / 2 - Width / 2;
            Top = FormLogin.Top + FormLogin.Height / 2 - Height / 2;
            this.run_ShowComputer = run_ShowComputer;
			this.password = password;
			this.treeView = treeView;
            try
            {
                System.Threading.Thread thread = new System.Threading.Thread(ReadFile);
                thread.Start();
                ShowDialog();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
			
		}
        //constructor lưu dữ liệu
        public FormLoad(byte[] password, TreeView treeView, Form FormMain, NotifyIcon notifyIcon)
		{
			InitializeComponent();
            Left = FormMain.Left + FormMain.Width / 2 - Width / 2;
            Top = FormMain.Top + FormMain.Height / 2 - Height / 2;
            this.password = password;
			this.treeView = treeView;
            this.notifyIcon = notifyIcon;

            FormMain.Cursor = Cursors.AppStarting;
            //chuyển treeView thành chuỗi			
            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Nodes == null)
                    break;
                TreeNodeToString(node, false);
                savingString += rightCode;
                break;
            }
            if (savingString.Length == 0 || (dataString == savingString && password == passwordBytes))
            {
                FormMain.Cursor = Cursors.Default;
                return;//thoát ngay nếu dữ liệu không có gì
            }
            //
            FormMain.Cursor = Cursors.Default;

            System.Threading.Thread thread = new System.Threading.Thread(SaveFile);
			thread.Start();
			ShowDialog();
		}


        //đọc file và nạp treeView	
        #region ReadFile																																																																		
        private void ReadFile()
		{
            try
            {
                //mở file, return khi file rỗng
                FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Attributes = FileAttributes.Hidden;
                if (fileStream.Length == 0)
                {
                    Done = true;
                    fileStream.Close();
                    return;
                }
                //nạp data thô từ file			
                byte[] dataBytes = new byte[fileStream.Length];
                fileStream.Read(dataBytes, 0, dataBytes.Length);
                //giải mã	
                passwordBytes = password;
                dataString = Encoding.Unicode.GetString(GCoding.Decoding(dataBytes, passwordBytes));
                //kiểm tra đoạn passCode trước chuỗi và xoá nó đi	
                if (dataString.Substring(0, passCode.Length) != passCode)
                {
                    password_is_wrong = true;
                    Done = true;
                    fileStream.Close();
                    return;
                }
                dataString = dataString.Substring(passCode.Length);
                //nạp treeView
                foreach (TreeNode node in treeView.Nodes)
                {
                    int index = -leftCode.Length;
                    while (true)
                    {
                        index += leftCode.Length;
                        if (index == dataString.Length - 1 || dataString.Substring(index).IndexOf(rightCode) == 0) break;
                        index = AddTreeNode(node, index);//đệ quy
                    }
                    node.Expand();
                    break;
                }
                //dọn dẹp
                fileStream.Close();
                //show hardware
                try
                {
                    if (run_ShowComputer) info = ShowComputer.Show();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Done = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
		}
		private int AddTreeNode(TreeNode treeRoot, int indexCurrent)
		{
			int index = dataString.Substring(indexCurrent).IndexOf(leftCode) + indexCurrent;			
			//tạo mới một TreeNode
			TreeNode treeNode = new TreeNode();
			string dataString2 = dataString.Substring(indexCurrent, index - indexCurrent);//đoạn string chứa dữ liệu của một TreeNode
			indexCurrent = dataString2.IndexOf(nameCode);
			treeNode.Text = dataString2.Substring(0, indexCurrent);//đoạn đầu của dataString2 chứa tên của TreeNode
			treeNode.Name = dataString2.Substring(indexCurrent + nameCode.Length);//đoạn còn lại của dataString2 chứa nội dung của TreeNode
            //nếu đây là một LockedTreeNode
            if (treeNode.Name.Contains(lockCode))
            {
                string fileName = treeNode.Name.Substring(0, treeNode.Name.IndexOf(lockCode));
                try
                {
                    //tạo thư mục để lưu file (nếu chưa có)
                    string path = Application.StartupPath + "\\Data\\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    treeNode.Tag = new DataBytes(File.ReadAllBytes(path + fileName));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Can't write the file: " + fileName + "\n" + e.ToString(), "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
			treeRoot.Nodes.Add(treeNode);
			//bắt đầu đệ quy
			while (true)
			{
				index += leftCode.Length;
				if (index == dataString.Length - 1 || dataString.Substring(index).IndexOf(rightCode) == 0) return index;
				index = AddTreeNode(treeNode, index);
			}
		}
        #endregion

        //lưu dữ liệu
        #region SaveFile	
        private void SaveFile()
		{
        
            try
            {
                //lưu data
                dataString = savingString;
                FileInfo fileInfo = new FileInfo(path);
                try
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }
                catch//trường hợp nếu file không tồn tại
                {
                    using (File.Create(path))
                        fileInfo = new FileInfo(path);
                    fileInfo.Attributes = FileAttributes.Normal;
                }
                dataString = passCode + dataString;//đoạn mã dùng để kiểm tra password lúc mở chương trình
                File.WriteAllBytes(path, GCoding.Encoding(Encoding.Unicode.GetBytes(dataString.ToCharArray()), password));
                fileInfo.Attributes = FileAttributes.Hidden;
                Done = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
		}
        /*
		private void TreeNodeToString(TreeNode treeRoot)
		{
			if (treeRoot.Nodes.Count == 0) return;
			foreach (TreeNode nodes in treeRoot.Nodes)
			{
                //nếu là UnlockedTreeNode
                if (nodes.Name.Contains(lockCode) && nodes.Name.Substring(0, nodes.Name.IndexOf(lockCode)).Contains(nodesCode))
                    SaveUnlockedTreeNode(nodes);
                //nếu là LockedTreeNode
                else if (nodes.Name.Contains(lockCode))
                    
                savingString += nodes.Text + nameCode + nodes.Name + leftCode;
                TreeNodeToString(nodes);
                savingString += rightCode;
			}
		}
        */
        private string TreeNodeToString(TreeNode node, bool is_get_text_nodes)
        {
            if (node.Nodes == null)
                return "";
            string text_nodes_ = "";
            foreach (TreeNode a_node in node.Nodes)// nodes = A
            {
                //nếu là một UnlockedTreeNode
                if (a_node.Name.Contains(nodesCode))
                {
                    /*
                        Trạng thái của Unlocked: TreeNode.Name = text-content + nodesCode + text-nodes + pathCode + path + lockCode + password (ASCII)
                        Một TreeNode file sẽ lưu: passCode + text-content + nodesCode + text-nodes
                    */
                    byte[] password = Encoding.ASCII.GetBytes(a_node.Name.Substring(a_node.Name.LastIndexOf(lockCode) + lockCode.Length).ToCharArray());
                    string text_content = a_node.Name.Substring(0, a_node.Name.IndexOf(nodesCode));
                    //đưa TreeNode về trạng thái Locked (TreeNode.Name = path + lockCode)
                    a_node.Name = a_node.Name.Substring(a_node.Name.LastIndexOf(pathCode) + pathCode.Length, a_node.Name.LastIndexOf(lockCode) + lockCode.Length - a_node.Name.LastIndexOf(pathCode) - pathCode.Length);
                    //bắt đầu tiền hành các bước để lưu dữ liệu và lock TreeNode lại
                    //tạo thư mục để lưu file (nếu chưa có)
                    string path = Application.StartupPath + "\\Data\\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    //lấy đường dẫn để lưu TreeNode
                    string fileName = a_node.Name.Substring(0, a_node.Name.LastIndexOf(lockCode));
                    //đệ quy, chuyển các TreeNodes con của TreeNode đó thành chuỗi		
                    string text_nodes = text_nodes = TreeNodeToString(a_node, true) + rightCode;
                    //xoá các TreeNodes con
                    a_node.Nodes.Clear();
                    //tạo bytes dữ liệu coded_text
                    byte[] bytes = GCoding.Encoding(Encoding.Unicode.GetBytes((passCode + text_content + nodesCode + text_nodes).ToCharArray()), password);
                    a_node.Tag = new DataBytes(bytes);
                    //tiến hành lưu
                    try
                    {
                        File.WriteAllBytes(path + fileName, bytes);
                    }
                    catch (Exception E)
                    {
                        notifyIcon.ShowBalloonTip(1000, "Can't write the file: " + fileName, E.ToString(), ToolTipIcon.Warning);
                    }
                }
                //nếu là một TreeNode bình thường
                if (is_get_text_nodes)
                    text_nodes_ += a_node.Text + nameCode + a_node.Name + leftCode + TreeNodeToString(a_node, true) + rightCode;
                else
                {
                    savingString += a_node.Text + nameCode + a_node.Name + leftCode;
                    TreeNodeToString(a_node, false);
                    savingString += rightCode;
                }
            }
            return text_nodes_;
        }
        #endregion

        #region timers
        private void timer1_Tick(object sender, EventArgs e)
		{
			
			if(U < 3)
			{
				U++;
				label1.Text += '.';
                Text += '.';
            }
			else
			{
				U = 0;
				label1.Text = "Loading";
                Text = "Loading";
            }
		}
		private void timer2_Tick(object sender, EventArgs e)
		{
			if (Done) Close();
		}
        #endregion
    }
}
