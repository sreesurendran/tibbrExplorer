using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Xml;

namespace tibbrExplorer.Response
{
    class UndeleteUserResponse : RestResponse
    {
        #region
        //Attributes
        private UserBean ubUserResponse = new UserBean();

        #endregion

        #region
        //Properties
        public UserBean userResponse
        {
            get { return ubUserResponse; }
            set { ubUserResponse = value; }
        }

        #endregion

        #region
        //Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {

            XmlNodeList users = xmlDoc.SelectNodes("user");
            foreach (XmlNode user in users)
            {
                if (user.SelectSingleNode("deleted") != null)
                    ubUserResponse.isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("id") != null)
                    ubUserResponse.userId = user.SelectSingleNode("id").InnerText;
            }
        }

        #endregion

    }
}
