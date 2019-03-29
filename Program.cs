using System;
using System.Collections;
using System.Windows.Forms;
using YourExperience.Other_Classes;

namespace YourExperience
{
	static class Program
	{
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
		static void Main()
		{
            //Application.Run(new FormMain());
            try
            {
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                ErrorFile.Save(ex);
                SystemFile.Save((int.Parse(SystemFile.Get(3)) + 1).ToString(), 3);
                throw ex;
            }
        }
    }
}
