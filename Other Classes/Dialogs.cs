using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YourExperience
{
    static class Dialogs
    {
        public static ColorDialog  colorDialog = new ColorDialog();
        static FontDialog fontDialog = new FontDialog();
        public static Panel[] customColorsBox;
        static FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        static OpenFileDialog openFileDialog = new OpenFileDialog();
        static SaveFileDialog saveFileDialog = new SaveFileDialog();

        public static string ShowOpenFileDialog(string title, string filter, string[] filesType)
        {
            openFileDialog.Title = title;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string str = openFileDialog.SafeFileName.Substring(openFileDialog.SafeFileName.LastIndexOf(".")).ToUpper();
                for (int i = 0; i < filesType.Length; i++)
                {
                    if(str.Contains(filesType[i].ToUpper())) return openFileDialog.FileName;
                }
            }
            return null;
        }

        public static string ShowSaveFileDialog(string title, string filter, string[] filesType)
        {
            saveFileDialog.Title = title;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = filter;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string str = saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf(".")).ToUpper();
                for (int i = 0; i < filesType.Length; i++)
                {
                    if (str.Contains(filesType[i].ToUpper())) return saveFileDialog.FileName;
                }
            }
            return null;
        }

        public static string CustomColorsToString()
        {
            if(colorDialog.CustomColors == null ||colorDialog.CustomColors.Length == 0) return "";
            string str = "";
            for (int i = 0; i < colorDialog.CustomColors.Length; i++)
            {
                str += colorDialog.CustomColors[i] + ",";
            }
            return str;
        }

        public static string ShowOpenFileDialog(string title)
        {
            openFileDialog.Title = title;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        public static string ShowSaveFileDialog(string title)
        {
            saveFileDialog.Title = title;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        public static object ShowColorDialog(Color startColor, bool is_fullOpen)
        {
            colorDialog.Color = startColor;
            colorDialog.FullOpen = is_fullOpen;
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }
            else
            {
                byte[] byteColor;
                if (colorDialog.CustomColors != null && customColorsBox != null)
                {
                    for (int i = 0; i < colorDialog.CustomColors.Length; i++)
                    {
                        byteColor = BitConverter.GetBytes(colorDialog.CustomColors[i]);
                        customColorsBox[i].BackColor = Color.FromArgb(byteColor[0], byteColor[1], byteColor[2]);
                    }
                }
                return colorDialog.Color;
            }
        }

        public static Font ShowFontDialog(Font startFont)
        {
            fontDialog.Font = startFont;
            if(fontDialog.ShowDialog() == DialogResult.Cancel)
                return startFont;
            else
                return fontDialog.Font;
        }
    }
}
