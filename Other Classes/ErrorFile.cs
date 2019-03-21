using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YourExperience.Other_Classes
{
    static class ErrorFile
    {
        //file lưu các lỗi đã được phát hiện
        static readonly string path = Application.StartupPath + "\\ListErrors.txt";

        public static void Save(Exception ex)
        {
            CheckFile();

            File.WriteAllText(path, File.ReadAllText(path) + DateTime.Now.ToString() + ":\r\n" + ex.ToString() + "\r\n\r\n");
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
