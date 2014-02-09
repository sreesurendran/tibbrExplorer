using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace tibbrExplorer
{
    class Logger
    {
        private static string strErrorLogPath;

        public static string logPath
        {
            get { return strErrorLogPath; }
            set { strErrorLogPath = value; }
        }

        public static void WriteLog(string strMessage)
        {
            using (StreamWriter swLog = new StreamWriter(strErrorLogPath, true))
            {
                //swLog.WriteLine(strMessage);
                swLog.Write(strMessage);
            }
        }
    }
}
