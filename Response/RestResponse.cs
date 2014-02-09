using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;

namespace tibbrExplorer.Response
{
    class RestResponse
    {
        private HttpStatusCode strStatusCode;

        public HttpStatusCode statusCode
        {
            get { return strStatusCode; }
            set { strStatusCode = value; }
        }

        public virtual void parseDoc(XmlDocument xmlDoc)
        {
            
        }
    }
}
