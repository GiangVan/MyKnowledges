using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YourExperience.Other_Classes
{
    static class SystemFile
    {
        /*

            hàng 1: số lần ứng dụng khởi động cùng Windows
            hàng 2: số lần user mở ứng dụng
            hàng 3: file data

        */


        static readonly string path = Application.StartupPath +  "\\System.ini";

        static SystemFile()
        {
            SaveTheNumberOfTimeToOpenApp();
        }

        static void SaveTheNumberOfTimeToOpenApp()
        {
            CheckFile();
            
            //kiểm tra toàn vẹn file
            string[] data = File.ReadAllLines(path);
            if (data != null && data.Length == 4)
            {
                //nhận biết ứng dụng khỏi động cùng Windows
                if(Directory.GetCurrentDirectory().ToLower() == @"c:\windows\system32")
                {
                    Save((int.Parse(data[0]) + 1).ToString(), 0);
                }
                else
                {
                    Save((int.Parse(data[1]) + 1).ToString(), 1);
                }
            }
            else
            {
                Save("", 2);
            }
        }

        static public void Save(string content, int index)
        {
            CheckFile();

            string[] data = File.ReadAllLines(path);
            //kiểm tra toàn vẹn file
            if (data == null || data.Length != 4)
                data = new string[4] { "0", "0", "", "0" };
            //
            data[index] = content;
            File.WriteAllLines(path, data);
        }

        static public string Get(int index)
        {
            CheckFile();

            string[] data = File.ReadAllLines(path);
            //kiểm tra toàn vẹn file
            if (data == null || data.Length != 4)
                data = new string[4] { "0", "0", "", "0" };
            //
            return data[index];
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
