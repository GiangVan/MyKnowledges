using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience.Other_Classes
{
    static class NodesEditingHistory
    {
        static Hashtable history = new Hashtable();
        class data
        {
            string name, content;
            Color color;
            bool is_lockNode;
            public data(string name, string content, Color color, bool is_lockNode)
            {
                this.color = color;
                this.name = name;
                this.content = content;
                this.is_lockNode = is_lockNode;
            }

            public bool Check(TreeNode node)
            {
                if(
                    color != node.ForeColor ||
                    name != node.Text ||
                    content != node.Name ||
                    is_lockNode != ((Tag_of_Node)node.Tag).is_lockNode
                  )
                    return true;
                return false;
            }

            //public void Set(TreeNode node)
            //{
            //    color = node.ForeColor;
            //    name = node.Text;
            //    content = node.Name;
            //    is_lockNode = ((Tag_of_Node)node.Tag).is_lockNode;
            //}
        }

        public static void Clear()
        {
            history.Clear();
        }

        public static void Add(TreeNode node, bool definitelySave)//gửi thông tin node trước khi bị chỉnh sửa để lấy đó làm mốc so sánh với các bản chỉnh sửa của node sau này
        {
            if (history.ContainsKey(node))
            {
                if (definitelySave)
                    history[node] = null;
            }
            else//nếu node đã tồn tại thì không thêm để ngăn sửa đổi bản chỉnh sửa gốc của node
            {
                if (definitelySave)
                    history.Add(node, null);
                else
                    history.Add(node, new data(node.Text, node.Name, node.ForeColor, ((Tag_of_Node)node.Tag).is_lockNode));
            }
        }
        public static void Add(TreeNode node)
        {
            if (!history.ContainsKey(node))
            {
                history.Add(node, new data(node.Text, node.Name, node.ForeColor, ((Tag_of_Node)node.Tag).is_lockNode));
            }
        }

        public static bool Check()//nếu có thay đổi trả về true
        {
            if(history.Count > 0)
            {
                foreach (object item in history.Keys)
                {
                    if(
                        item == null || 
                        history[item] == null ||
                        ((data)history[item]).Check((TreeNode)item)
                      )
                        return true;
                }
            }
            return false;
        }
    }
}
