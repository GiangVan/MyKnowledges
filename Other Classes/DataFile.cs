using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YourExperience.OtherClasses;

namespace YourExperience
{
    static class DataFile
    {
        /*

                    đây là một lớp được tạo ra với mục đích lưu trữ các thiết lập cài đặt và các dữ liệu cơ bản của chương trình vào một file có tên là "Setting.txt"

        */

        static readonly string path = Application.StartupPath + "\\Setting.txt";
        //khai báo các biến sử dụng cho form main
        #region
        static internal string WindowState;
        static internal int Width, Height, LeftOfpanelMid;
        #endregion

        //constructer nạp dữ liệu khi chương trình được khởi động
        #region
        static DataFile()
        {
            FontConverter fontConverter = new FontConverter();
            string str = null;

            CheckFile();
            string[] data = null;
        ReadAgain:
            try
            {
                data = File.ReadAllLines(path);
                if(data == null) return;
            }
            catch (Exception ex)
            {
                if(WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Read the setups of this app failed!\r\n", ex, null) == DialogResult.Retry) goto ReadAgain;
            }

            //checkBox__Start_with_windows
            try
            {
                WindowsForm.Setting.checkBox__Start_with_windows.Checked = bool.Parse(Find(data, "checkBox__Start_with_windows"));
            }
            catch { }
            //checkBox__Automatically_open_file_when_start_the_program
            try
            {
                WindowsForm.Setting.checkBox__Automatically_open_file_when_this_application_start.Checked = bool.Parse(Find(data, "checkBox__Automatically_open_file_when_this_application_start"));
            }
            catch { }

            //radioButton__Open_my_chosen_file
            try
            {
                WindowsForm.Setting.radioButton__Open_my_chosen_file.Checked = bool.Parse(Find(data, "radioButton__Open_my_chosen_file"));
            }
            catch { }

            //radioButton__Open_the_most_recent_active_file
            try
            {
                WindowsForm.Setting.radioButton__Open_the_most_recent_active_file.Checked = bool.Parse(Find(data, "radioButton__Open_the_most_recent_active_file"));
            }
            catch { }

            //checkBox__Automatically_minimize_when_start
            try
            {
                WindowsForm.Setting.checkBox__Automatically_minimize_when_start.Checked = bool.Parse(Find(data, "checkBox__Automatically_minimize_when_start"));
            }
            catch { }


            //checkBox__Automatically_lock_the_Node
            try
            {
                WindowsForm.Setting.checkBox__Automatically_lock_the_Nodes.Checked = bool.Parse(Find(data, "checkBox__Automatically_lock_the_Nodes"));
            }
            catch { }

            //checkBox__Show_tutorials
            try
            {
                WindowsForm.Setting.checkBox__Show_tutorials.Checked = bool.Parse(Find(data, "checkBox__Show_tutorials"));
            }
            catch { }

            //checkBox__Automatically_update
            try
            {
                WindowsForm.Setting.checkBox__Automatically_update.Checked = bool.Parse(Find(data, "checkBox__Automatically_update"));
            }
            catch { }

            //checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L
            try
            {
                WindowsForm.Setting.checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L.Checked = bool.Parse(Find(data, "checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L"));
            }
            catch { }

            //checkBox__Automatically_add_tabs_when_down_line
            try
            {
                WindowsForm.Setting.checkBox__Automatically_add_tabs_when_down_line.Checked = bool.Parse(Find(data, "checkBox__Automatically_add_tabs_when_down_line"));
            }
            catch { }

            //checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes
            try
            {
                WindowsForm.Setting.checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes.Checked = bool.Parse(Find(data, "checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes"));
            }
            catch { }

            //checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes
            try
            {
                WindowsForm.Setting.checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes.Checked = bool.Parse(Find(data, "checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes"));
            }
            catch { }

            //trackBar__AutoSave
            try
            {
                WindowsForm.Setting.trackBar__AutoSave.Value = int.Parse(Find(data, "trackBar__AutoSave"));
            }
            catch { }

            //button__TextBox
            try
            {
                WindowsForm.Setting.button__TextBox.Font = (Font)fontConverter.ConvertFromString(Find(data, "button__TextBox"));
            }
            catch { }

            //button__TreeView
            try
            {
                WindowsForm.Setting.button__TreeView.Font = (Font)fontConverter.ConvertFromString(Find(data, "button__TreeView"));
            }
            catch { }

            //Width
            try
            {
                Width = int.Parse(Find(data, "Width"));
            }
            catch { }

            //Height
            try
            {
                Height = int.Parse(Find(data, "Height"));
            }
            catch { }

            //WindowState
            try
            {
                WindowState = Find(data, "WindowState");
            }
            catch { }

            //LeftOfpanelMid
            try
            {
                LeftOfpanelMid = int.Parse(Find(data, "LeftOfpanelMid"));
            }
            catch { }

            //customColorsBox
            try
            {
                str = Find(data, "customColorsBox");
                int[] array = new int[16];

                for (int i = 0; i < 16; i++)
                {
                    array[i] = int.Parse(str.Substring(0, str.IndexOf(',')));
                    str = str.Substring(str.IndexOf(',') + 1);
                }

                Dialogs.colorDialog.CustomColors = array;
            }
            catch { }

            //checkBox__Automatically_open_editing_when_have_just_created_a_node
            try
            {
                WindowsForm.Setting.checkBox__Automatically_open_editing_when_have_just_created_a_node.Checked = bool.Parse(Find(data, "checkBox__Automatically_open_editing_when_have_just_created_a_node"));
            }
            catch { }

            //checkBox__Automatically_open_editing_when_double_click_to_a_node
            try
            {
                WindowsForm.Setting.checkBox__Automatically_open_editing_when_double_click_to_a_node.Checked = bool.Parse(Find(data, "checkBox__Automatically_open_editing_when_double_click_to_a_node"));
            }
            catch { }
        }
        #endregion

        public static void Save(string key, object value)
        {
            CheckFile();
            string[] data = null;
            ReadAgain:
            try
            {
                data = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Read the setups of the app failed!\n", ex, null) == DialogResult.Retry) goto ReadAgain;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Substring(0, key.Length) == key)
                {
                    data[i] = key + "=" + value.ToString();
                    WriteAgain:
                    try { File.WriteAllLines(path, data); }
                    catch (Exception ex)
                    {
                        if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Write the setups of the app failed!\n", ex, null) == DialogResult.Retry) goto WriteAgain;
                    }
                    return;
                }
            }
            //nếu chưa có thì tạo mới
            if(data == null || data.Length == 0) data = new string[] {"\r\n" + key + "=" + value.ToString()};
            else data[data.Length - 1] += "\r\n" + key + "=" + value.ToString();
            WriteAgain2:
            try { File.WriteAllLines(path, data); }
            catch (Exception ex)
            {
                if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Write the setups of the app failed!", ex, null) == DialogResult.Retry) goto WriteAgain2;
            }
        }

        public static void SaveAll()
        {
            FontConverter fontConverter = new FontConverter();
            CheckFile();
            string[] str = new string[]
            {
                "checkBox__Start_with_windows", WindowsForm.Setting.checkBox__Start_with_windows.Checked.ToString(),
                "checkBox__Automatically_minimize_when_start", WindowsForm.Setting.checkBox__Automatically_minimize_when_start.Checked.ToString(),
                "checkBox__Automatically_lock_the_Nodes", WindowsForm.Setting.checkBox__Automatically_lock_the_Nodes.Checked.ToString(),
                "checkBox__Show_tutorials", WindowsForm.Setting.checkBox__Show_tutorials.Checked.ToString(),
                "checkBox__Automatically_update", WindowsForm.Setting.checkBox__Automatically_update.Checked.ToString(),
                "checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L", WindowsForm.Setting.checkBox__Auto_save_the_text_has_cut_when_using_Ctrl_L.Checked.ToString(),
                "checkBox__Automatically_add_tabs_when_down_line", WindowsForm.Setting.checkBox__Automatically_add_tabs_when_down_line.Checked.ToString(),
                "checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes", WindowsForm.Setting.checkBox__Automatically_save_zoomfactor_of_the_node_when_transfer_to_another_nodes.Checked.ToString(),
                "checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes", WindowsForm.Setting.checkBox__Automatically_save_the_selected_text_of_the_node_when_transfer_to_another_nodes.Checked.ToString(),
                "trackBar__AutoSave", WindowsForm.Setting.trackBar__AutoSave.Value.ToString(),
                "button__TextBox", fontConverter.ConvertToString(WindowsForm.Setting.button__TextBox.Font),
                "button__TreeView", fontConverter.ConvertToString(WindowsForm.Setting.button__TreeView.Font),
                "Width", Width.ToString(),
                "Height", Height.ToString(),
                "WindowState", WindowState,
                "LeftOfpanelMid", LeftOfpanelMid.ToString(),
                "customColorsBox", Dialogs.CustomColorsToString(),
                "checkBox__Automatically_open_file_when_this_application_start", WindowsForm.Setting.checkBox__Automatically_open_file_when_this_application_start.Checked.ToString(),
                "radioButton__Open_the_most_recent_active_file", WindowsForm.Setting.radioButton__Open_the_most_recent_active_file.Checked.ToString(),
                "radioButton__Open_my_chosen_file", WindowsForm.Setting.radioButton__Open_my_chosen_file.Checked.ToString(),
                "checkBox__Automatically_open_editing_when_have_just_created_a_node", WindowsForm.Setting.checkBox__Automatically_open_editing_when_have_just_created_a_node.Checked.ToString(),
                "checkBox__Automatically_open_editing_when_double_click_to_a_node", WindowsForm.Setting.checkBox__Automatically_open_editing_when_double_click_to_a_node.Checked.ToString()

            };

            string[] data = new string[str.Length/2];
            for (int i = 0; i < str.Length; i+=2)
            {
                data[i/2] = str[i] + "=" + str[i+1];
            }

        WriteAgain:
            try { File.WriteAllLines(path, data); }
            catch (Exception ex)
            {
                if (WindowsForm.Notification.Show(MessageBoxButtons.RetryCancel, "Write the setups of the app failed!", ex, null) == DialogResult.Retry) goto WriteAgain;
            }
        }

        public static string Get(string key)
        {
            CheckFile();
            return Find(File.ReadAllLines(path), key);
        }

        static string Find(string[] data, string key)//tìm kiếm và trả về dòng chứa key
        {
            if (data == null || data.Length == 0) return null;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Length > key.Length && data[i].Substring(0, key.Length) == key && !data[i].Substring(0, key.Length).Contains("="))
                {
                    return data[i].Substring(key.Length + 1);
                }
            }
            return null;
        }

        private static void CheckFile()//kiểm tra và tạo file nếu file chưa tồn tại
        {
            if (!File.Exists(path))
            {
                try { File.Create(path).Close(); }
                catch
                {
                    Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));
                    File.Create(path).Close();
                }
            }
        }
    }
}
