using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YourExperience.OtherClasses;

namespace YourExperience
{
    static class PanelCell
    {
        private static string key, value;

        public static System.Windows.Forms.Panel Get(string key, string value, Hashtable hashtableCMD)
        {
            System.Windows.Forms.Panel panelCell = new System.Windows.Forms.Panel();
            System.Windows.Forms.TextBox textBoxContent = new System.Windows.Forms.TextBox();
            System.Windows.Forms.TextBox textBoxName = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonX = new System.Windows.Forms.Button();
            System.Windows.Forms.ContextMenuStrip contextMenuStripNone = new System.Windows.Forms.ContextMenuStrip();
            // 
            // panelCell
            // 
            panelCell.BackColor = System.Drawing.Color.Silver;
            panelCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelCell.Controls.Add(textBoxName);
            panelCell.Controls.Add(textBoxContent);
            panelCell.Controls.Add(buttonX);
            panelCell.Location = new System.Drawing.Point(2, 2);
            panelCell.Size = new System.Drawing.Size(635, 67);
            panelCell.Leave += new EventHandler(panelCell_Leave);
            panelCell.Enter += new EventHandler(panelCell_Enter);
            // 
            // textBoxContent
            // 
            textBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            textBoxContent.BackColor = System.Drawing.Color.Gainsboro;
            textBoxContent.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            textBoxContent.ForeColor = System.Drawing.Color.Gray;
            textBoxContent.Location = new System.Drawing.Point(10, 33);
            textBoxContent.Text = value;
            textBoxContent.Size = new System.Drawing.Size(620, 33);
            textBoxContent.TabStop = false;
            textBoxContent.ContextMenuStrip = contextMenuStripNone;
            textBoxContent.Name = "textBoxContent";
            textBoxContent.KeyDown += new KeyEventHandler(textBox_KeyDown);
            // 
            // textBoxName
            // 
            textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            textBoxName.BackColor = System.Drawing.Color.Gainsboro;
            textBoxName.Font = new System.Drawing.Font("Segoe UI bold", 14F, System.Drawing.FontStyle.Bold);
            textBoxName.ForeColor = System.Drawing.Color.Gray;
            textBoxName.Location = new System.Drawing.Point(10, 2);
            textBoxName.Text = key;
            textBoxName.Size = new System.Drawing.Size(591, 23);
            textBoxName.TabStop = false;
            textBoxName.ContextMenuStrip = contextMenuStripNone;
            textBoxName.Name = "textBoxName";
            textBoxName.KeyDown += new KeyEventHandler(textBox_KeyDown);
            // 
            // buttonX
            // 
            buttonX.FlatStyle = System.Windows.Forms.FlatStyle.System;
            buttonX.Font = new System.Drawing.Font("Segoe UI", 9.2F, System.Drawing.FontStyle.Bold);
            buttonX.Location = new System.Drawing.Point(613, 2);
            buttonX.Size = new System.Drawing.Size(19, 19);
            buttonX.TabStop = false;
            buttonX.Text = "X";
            buttonX.Click += new EventHandler(buttonX_Click);
            //
            //
            //
            buttonX.Name = textBoxName.Text;
            panelCell.Tag = hashtableCMD;
            //
            //
            //
            return panelCell;
        }

        private static void buttonX_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)((Button)sender).Parent;
            UpdateData(panel);
            if (key.Replace(" ", "").Length == 0 || value.Replace(" ", "").Length == 0)
            {
                panel.Leave -= panelCell_Leave;
                panel.Dispose();
            }
            else if (WindowsForm.Question.Show("Are you sure?") == DialogResult.Yes)
            {
                panel.Leave -= panelCell_Leave;
                panel.Dispose();
            } 
        }

        private static void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private static void panelCell_Enter(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            UpdateData(panel);
            panel.BackColor = Color.CadetBlue;
            ((Hashtable)panel.Tag).Remove(key);
        }

        private static void panelCell_Leave(object sender, EventArgs e)
        {//sự kiện này chỉ chạy trong trường hợp bị out-focus bởi nhấn vào một panel khác
            Panel panel = (Panel)sender;
            UpdateData(panel);
            //kiểm tra rỗng
            if (key.Replace(" ", "").Length == 0)
            {
                panel.Dispose();
                return;
            }
            //kiểm tra trùng
            if (((Hashtable)panel.Tag).ContainsKey(key))
            {
                WindowsForm.Notification.Show(MessageBoxButtons.OK, "Namesnake!", null);
                panel.Dispose();
                return;
            }
            //cập nhật các giá trị
            panel.BackColor = Color.Silver;
            ((Hashtable)panel.Tag).Add(key, value);
        }

        private static void UpdateData(Panel panel)
        {
            foreach (Control item in panel.Controls)
            {
                if(item.Name == "textBoxName") key = item.Text;
                if (item.Name == "textBoxContent") value = item.Text;
            }
        }
    }
}
