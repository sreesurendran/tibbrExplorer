using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace tibbrExplorer.Response
{
    class ErrorResponse
    {
        private static string strErrorXMLResponse;

        public static string errorMessage
        {
            get { return strErrorXMLResponse; }
            set { strErrorXMLResponse = value; }
        }

        public static void parseErrorXML(XmlDocument xmlDoc)
        {
            XmlNodeList errorElements = xmlDoc.SelectNodes("errors");
            foreach (XmlNode errorElement in errorElements)
            {
                if (errorElement.SelectSingleNode("error") != null)
                    strErrorXMLResponse = errorElement.SelectSingleNode("error").InnerText;
            }         

        }
    }
}
