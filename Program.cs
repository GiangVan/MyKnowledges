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
		static void Main(string[] args)
		{
            //Application.Run(new FormMain());
            try
            {
                if(args.Length > 0)
                {
                    Application.Run(new FormMain(args[0]));
                }
                else
                    Application.Run(new FormMain(null));
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
