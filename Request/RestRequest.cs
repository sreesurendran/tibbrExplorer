using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tibbrExplorer.Request
{
    class RestRequest
    {
        #region
        //Attributes
        public static string strBase = "";      

        #endregion

        #region
        //Getters
        public string getBase()
        {
            return strBase;
        }

        public virtual string getContent()
        {
            return null;
        }

        public virtual string getURLString()
        {
            return null;
        }

        #endregion

        #region
        //Setters

        public static void setBase(string strBase)
        {
            RestRequest.strBase = strBase;
        }

        #endregion

    }
}
