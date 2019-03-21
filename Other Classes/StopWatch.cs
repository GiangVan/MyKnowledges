using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace YourExperience.Other_Classes
{

    static class StopWatch
    {
        static long time;

        public static void Begin()
        {
            Thread.Sleep(2000);
            time = DateTime.Now.Ticks;
        }

        public static string End()
        {
            time = (DateTime.Now.Ticks - time) / 10000;
            return "\r\nEnd: " + FormatNumber(time, ",") + " (milliseconds)\r\n";
        }

        static string FormatNumber(long inputNum, string character)
        {
            if ((inputNum > 999 || inputNum < -999) && character.Length == 1)
            {
                string stringOutput = inputNum.ToString();
                if (inputNum < 0) stringOutput = stringOutput.Remove(0, 1);
                int n = (stringOutput.Length + 2) / 3;
                for (int i = 1; i < n; i++)
                    stringOutput = stringOutput.Insert(stringOutput.Length - (i * 3 + i - 1), character);
                if (inputNum < 0) stringOutput = stringOutput.Insert(0, "-");
                return stringOutput;
            }
            else
                return inputNum.ToString();
        }
    }

}
