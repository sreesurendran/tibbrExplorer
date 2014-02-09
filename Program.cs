using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace tibbrExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region
            //initialize the logger
            //create a directory, if it doesn't exist

            //create a new file, with the right pattern

            #endregion
            
            Application.Run(new tibbrExplorerLogin());
        }
    }
}
