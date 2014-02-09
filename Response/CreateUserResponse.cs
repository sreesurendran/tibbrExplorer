using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Xml;

namespace tibbrExplorer.Response
{
    class CreateUserResponse : RestResponse
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
            XmlNodeList userElements = xmlDoc.SelectNodes("user");
            foreach (XmlNode user in userElements)
            {
                if (user.SelectSingleNode("deleted") != null)
                    ubUserResponse.isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("description") != null)
                    ubUserResponse.aboutMe = user.SelectSingleNode("description").InnerText;
                if (user.SelectSingleNode("email") != null)
                    ubUserResponse.email = user.SelectSingleNode("email").InnerText;
                if (user.SelectSingleNode("first-name") != null)
                    ubUserResponse.firstName = user.SelectSingleNode("first-name").InnerText;
                if (user.SelectSingleNode("id") != null)
                    ubUserResponse.userId = user.SelectSingleNode("id").InnerText;
                if (user.SelectSingleNode("last-name") != null)
                    ubUserResponse.lastName = user.SelectSingleNode("last-name").InnerText;
                if (user.SelectSingleNode("display-name") != null)
                    ubUserResponse.displayNameStd = user.SelectSingleNode("display-name").InnerText;
                if (user.SelectSingleNode("displayName") != null)
                    ubUserResponse.displayNameCustom = user.SelectSingleNode("displayName").InnerText;

            }
        }

        #endregion

    }
}
