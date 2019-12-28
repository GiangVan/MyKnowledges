using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using YourExperience.Other_Classes;

namespace YourExperience.OtherClasses
{
    /*

        Lớp cung cấp các Me+thods hỗ trợ cho việc thao tác với cây nodes

    */
    static class NetworkNodes
    {
        static string nodesString;
        public static readonly string checkingCode = "Giang dep trai hahahaha";//dùng để nhận biết đã nhập đúng mật khẩu. Được thêm vào vị trí đầu content của một node
        static int index, n;

        //kiểm tra mật khẩu
        public static bool CheckPassword(string text)
        {
            if(string.IsNullOrEmpty(text)) return false;
            return text.Substring(0, checkingCode.Length) == checkingCode;//đúng mật khẩu trả về true
        }

        //tạo mạng nodes từ file dữ liệu
        public static void Create(TreeView treeView, string path)
        {
            n = 0;
            nodesString = File.ReadAllText(path);
            index = nodesString.IndexOf(@"\") + 1;

            //xoá các nodes cũ
            treeView.Nodes.Clear();
            
            WindowsForm.Loading.Show(int.Parse(nodesString.Substring(0, index - 1)), "Creating the TreeNode");
            try
            {
                while (index != nodesString.Length)
                    treeView.Nodes.Add(Recursive_ToTreeNode(null));//Nạp vào TreeView một nhánh lớn của 1 mạng Node
            }
            catch
            {
                WindowsForm.Loading.End();
                throw;
            }
            WindowsForm.Loading.End();
            nodesString = null;
            //change form's name when the nodes were loaded
            //get path name
            string pathName = path.Substring(0, path.LastIndexOf("."));
            if(pathName.Contains(@"\"))
            {
                pathName = pathName.Substring(pathName.LastIndexOf(@"\") + 1);
            }
            treeView.FindForm().Text = pathName;
        }

        //tạo mạng nodes từ một chuỗi dữ liệu
        public static void Create(TreeNode node, string data)
        {
            nodesString = data;
            index = 0;

            while (index != nodesString.Length)
                node.Nodes.Add(Recursive_ToTreeNode(null));//Nạp vào TreeView một nhánh lớn của 1 mạng Node

            nodesString = null;
        }

        //đệ quy trả về một node hoàn chỉnh sau khi được tách từ chuỗi
        static TreeNode Recursive_ToTreeNode(TreeNode node)// Recursive - đệ quy - ˌriːˈkərsɪv
        {
            //  "name \bcontent \ccolor \ddate \iis_locked \nnodes \e"

            int index1 = nodesString.IndexOf(@" \b", index);
            Tag_of_Node newTag = new Tag_of_Node();
            TreeNode newNode = new TreeNode(nodesString.Substring(index, index1 - index).Replace(@"\\", @"\"));
            n++;
            WindowsForm.Loading.Update(n + " - " + newNode.Text, 1);
            newNode.Tag = newTag;
            if(node != null) node.Nodes.Add(newNode);
            //content
            int index2 = nodesString.IndexOf(@" \c", index1);
            newNode.Name = nodesString.Substring(index1 + 3, index2 - index1 - 3).Replace(@"\\", @"\");
            //color
            index1 = nodesString.IndexOf(@" \d", index2);
            newNode.ForeColor = Color.FromArgb(int.Parse(nodesString.Substring(index2 + 3, index1 - index2 - 3)));
            //date
            index2 = nodesString.IndexOf(@" \i", index1);
            newTag.date = nodesString.Substring(index1 + 3, index2 - index1 - 3);
            //is_lockNode
            index1 = nodesString.IndexOf(@" \n", index2);
            newTag.is_lockNode = bool.Parse(nodesString.Substring(index2 + 3, index1 - index2 - 3));
            //nodes
            //chia làm hai hướng
            if (newTag.is_lockNode)
            {
                index2 = nodesString.IndexOf(@" \e", index1);
                newTag.nodes = nodesString.Substring(index1 + 3, index2 - 3 - index1).Replace(@"\\", @"\");
                index = index2 + 3;
            }
            else
            {
                index = index1 + 3;
                while (nodesString.Substring(index, 3) != @" \e")
                {
                    Recursive_ToTreeNode(newNode);
                }
                index += 3;
            }
            return newNode;
        }

        //lưu toàn bộ mạng nodes 
        public static void SaveAll(TreeView tree, string path)
        {
            if(!NodesEditingHistory.Check()) return;//nếu dữ liệu k có thay đổi thì out
        TryAgain:
            n = 0;
            nodesString = tree.GetNodeCount(true) + @"\";
            WindowsForm.Loading.Show(tree.GetNodeCount(true), "Saving the TreeNode");
            //chuyển treeView sang chuỗi
            try
            {
                foreach (TreeNode a_node in tree.Nodes)
                {
                    Recursive_ToString_1(a_node);//Tạo ra một nhánh lớn của 1 mạng Node
                }
            }
            catch (Exception ex)
            {
                if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Convert the List Nodes to string failed!", ex, null) == DialogResult.Retry)
                {
                    WindowsForm.Loading.End();
                    goto TryAgain;
                }
                else
                    WindowsForm.Loading.End();
            }
        TryAgain2:
            //lưu chuỗi vừa tạo vào file
            try
            {
                File.WriteAllText(path, nodesString);
            }
            catch (Exception ex)
            {
                if(WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Save the List Nodes failed!", ex, null) == DialogResult.Retry)
                {
                    goto TryAgain2;
                }
                else
                    WindowsForm.Loading.End();
            }
            WindowsForm.Loading.End();
            nodesString = null;
            NodesEditingHistory.Clear();
        }

        //đệ quy chuyển một cây node truyền vào thành một chuỗi string
        static void Recursive_ToString_1(TreeNode node)// Recursive - đệ quy - ˌriːˈkərsɪv
        {
            n++;
            WindowsForm.Loading.Update(n + " - " + node.Text, 1);
            //  "name \bcontent \ccolor \ddate \iis_lockNode \nnodes \e"
            //lưu ý: đối với các thành phần có thể chứa ký tự "\" thì phải replace \ sang \\. Ví dụ như: name, content,...

            //mã hoá nội dung với node đang ở trạng thái mở
            string content = node.Name;
            if (((Tag_of_Node)node.Tag).is_lockNode && ((Tag_of_Node)node.Tag).unlocked)
                content = Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(checkingCode + node.Name, ((Tag_of_Node)node.Tag).password));
            //nối chuỗi các thuộc tính của node
            nodesString += node.Text.Replace(@"\", @"\\") + @" \b"
                        + content.Replace(@"\", @"\\") + @" \c"
                        + node.ForeColor.ToArgb() + @" \d"
                        + ((Tag_of_Node)node.Tag).date + @" \i"
                        + ((Tag_of_Node)node.Tag).is_lockNode.ToString() + @" \n";
            //nếu không phải là một node bình thường
            if (((Tag_of_Node)node.Tag).is_lockNode)
            {
                //nếu node này đang khoá
                if (!((Tag_of_Node)node.Tag).unlocked)
                {
                    nodesString += ((Tag_of_Node)node.Tag).nodes.Replace(@"\", @"\\") + @" \e";
                }
                //nếu node này đang mở
                else
                {
                    string lockNodesString = "";
                    if (node.Nodes != null && node.Nodes.Count > 0)
                    {
                        foreach (TreeNode a_node in node.Nodes)
                        {
                            lockNodesString += Recursive_ToString_2(a_node);
                        }
                    }
                    nodesString += Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(lockNodesString, ((Tag_of_Node)node.Tag).password)).Replace(@"\", @"\\") + @" \e";
                }
            }
            //nếu là một node bình thường
            else
            {
                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    foreach (TreeNode a_node in node.Nodes)
                    {
                        Recursive_ToString_1(a_node);
                    }
                }
                nodesString += @" \e";
            }
        }

        //tương tự như Recursive_ToString_1 thay vì nối từng đoạn string về một node khi kết thúc hàm vào biến nodesString, thì Recursive_ToString_2 return đoạn string của node đó mỗi khi kết thúc hàm
        //so về tốc độ thì Recursive_ToString_2 chậm hơn Recursive_ToString_1 nhưng linh hoạt hơn nên nó được để public
        public static string Recursive_ToString_2(TreeNode node)
        {
            string returnString = "";
            n++;
            WindowsForm.Loading.Update(n + " - " + node.Text, 1);
            //  "name \bcontent \ccolor \ddate \iis_lockNode \nnodes \e"
            //lưu ý: đối với các thành phần có thể chứa ký tự "\" thì phải replace \ sang \\. Ví dụ như: name, content,...

            //mã hoá nội dung với node đang ở trạng thái mở
            string content = node.Name;
            if (((Tag_of_Node)node.Tag).is_lockNode && ((Tag_of_Node)node.Tag).unlocked)
                content = Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(checkingCode + node.Name, ((Tag_of_Node)node.Tag).password));
            //nối chuỗi các thuộc tính của node
            returnString += node.Text.Replace(@"\", @"\\") + @" \b"
                        + content.Replace(@"\", @"\\") + @" \c"
                        + node.ForeColor.ToArgb() + @" \d"
                        + ((Tag_of_Node)node.Tag).date + @" \i"
                        + ((Tag_of_Node)node.Tag).is_lockNode.ToString() + @" \n";
            //nếu không phải là một node bình thường
            if (((Tag_of_Node)node.Tag).is_lockNode)
            {
                //nếu node này đang khoá
                if (!((Tag_of_Node)node.Tag).unlocked)
                {
                    return returnString + ((Tag_of_Node)node.Tag).nodes.Replace(@"\", @"\\") + @" \e";
                }
                //nếu node này đang mở
                else
                {
                    string lockNodesString = "";
                    if (node.Nodes != null && node.Nodes.Count > 0)
                    {
                        foreach (TreeNode a_node in node.Nodes)
                        {
                            lockNodesString += Recursive_ToString_2(a_node);
                        }
                    }
                    byte[] bytes = ((Tag_of_Node)node.Tag).password;
                    return returnString + Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(lockNodesString, bytes)).Replace(@"\", @"\\") + @" \e";
                }
            }
            //nếu là một node bình thường
            else
            {
                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    foreach (TreeNode a_node in node.Nodes)
                    {
                        returnString += Recursive_ToString_2(a_node);
                    }
                }
                return returnString + @" \e";
            }
        }

        //khoá node đang mở
        public static void SaveAndLockAUnlockedNode(TreeNode node)
        {
            //mã hoá nội dung
            node.Name = Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(checkingCode + node.Name, ((Tag_of_Node)node.Tag).password));
            //đưa các nodes con về chuỗi để mã hoá
            string nodesString = "";
            foreach (TreeNode a_node in node.Nodes)
            {
                nodesString += Recursive_ToString_2(a_node);
            }
            node.Nodes.Clear();
            //mã hoá các nodes con
            ((Tag_of_Node)node.Tag).nodes = Encoding.Default.GetString(AdvancedEncryptionStandard.Encoding(nodesString, ((Tag_of_Node)node.Tag).password));
            ((Tag_of_Node)node.Tag).password = null;
            ((Tag_of_Node)node.Tag).unlocked = false;
        }

        //tìm kiếm và trả về true nếu node truyền vào nằm trong root node
        public static bool SearchANode(TreeNode node, TreeNode root)
        {
            if (root.Nodes == null || root.Nodes.Count == 0)
                return false;
            if(root.Nodes.Contains(node)) return true;
            else
                foreach (TreeNode item in root.Nodes)
                {
                    if (SearchANode(node, item)) return true;
                }
            return false;
        }

        //khoá tất cả các nodes nằm bên ngoài node truyền vào, trong phạm vi các nodes con của root
        public static void LockNodes(TreeNode node, TreeNode root)
        {
            if(node == root) return;
            //nếu node nằm trong root thì tìm sâu vào root node và khoá các nodes khác node truyền vào
            if (SearchANode(node, root))
            {
                foreach (TreeNode a_node in root.Nodes)
                {
                    if (node != a_node) LockNodes(node, a_node);
                }
            }
            //nếu không node truyền vào không nằm trong root node thì khoá cả một cây nodes
            else
                KillNodes(root);
        }

        //khoá cả một cây nodes
        public static void KillNodes(TreeNode root)
        {
            //nếu root node không có các nodes con và đang ở trạng thái unlocked thì khoá lại
            if ((root.Nodes == null || root.Nodes.Count == 0) && ((Tag_of_Node)root.Tag).unlocked) SaveAndLockAUnlockedNode(root);
            //ngược lại
            else
            {
                //tìm và khoá các nodes con
                foreach (TreeNode a_node in root.Nodes)
                {
                    KillNodes(a_node);
                }
                //khoá root node sau khi khoá các nodes con với điều kiện root node đang unlock
                if (((Tag_of_Node)root.Tag).unlocked)
                    SaveAndLockAUnlockedNode(root);
            }
        }
    }
}
