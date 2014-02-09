using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace tibbrExplorer.Request
{
    class ProxyDefinition
    {

        #region
        //Attributes
        private WebProxy wp;
        private string host = "";
        private int port;

        #endregion

        #region
        //Properties
        public WebProxy wprox
        {
            get { return wp; }
            set { wp = value; }
        }


        #endregion

        #region
        //Constructor
        public ProxyDefinition()
        {
            StreamReader srProxy = new StreamReader(@"Proxy.ini");
            char[] delim = new char[] { ';' };
            do
            {
                string[] strParts = srProxy.ReadLine().Split(delim);
                if (strParts[0].ToLower().Trim().Contains("host"))
                    host = strParts[1].Trim();
                else if (strParts[0].ToLower().Trim().Contains("port"))
                    port = int.Parse(strParts[1].Trim());

            } while (srProxy.Peek() != -1);
            srProxy.Close();
            wp = new WebProxy(host, port);
            //wp.UseDefaultCredentials = true;
            wp.BypassProxyOnLocal = true;

        }


        #endregion


    }
}
