using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using tibbrExplorer.Beans;
using System.Windows.Forms;

namespace tibbrExplorer.Response
{
    class SearchUserResponse : RestResponse
    {
        #region Attributes
        private UserBean ubUserResponse = new UserBean();

        #endregion

        #region Properties
        public UserBean userResponse
        {
            get { return ubUserResponse; }
            set { ubUserResponse = value; }
        }

        #endregion

        #region Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {

            XmlNodeList users = xmlDoc.SelectNodes("/users/user");
            foreach (XmlNode user in users)
            {
                if (user.SelectSingleNode("deleted") != null)
                    ubUserResponse.isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("description") != null)
                    ubUserResponse.aboutMe = user.SelectSingleNode("description").InnerText;
                if(user.SelectSingleNode("email") != null)
                    ubUserResponse.email = user.SelectSingleNode("email").InnerText;
                if (user.SelectSingleNode("first-name") != null)
                    ubUserResponse.firstName = user.SelectSingleNode("first-name").InnerText;
                if (user.SelectSingleNode("id") != null)
                    ubUserResponse.userId = user.SelectSingleNode("id").InnerText;
                if (user.SelectSingleNode("last-name") != null)
                    ubUserResponse.lastName = user.SelectSingleNode("last-name").InnerText;
                if (user.SelectSingleNode("login") != null)
                    ubUserResponse.loginName = user.SelectSingleNode("login").InnerText;
                if (user.SelectSingleNode("display-name") != null)
                    ubUserResponse.displayNameStd = user.SelectSingleNode("display-name").InnerText;
                if (user.SelectSingleNode("displayName") != null)
                    ubUserResponse.displayNameCustom = user.SelectSingleNode("displayName").InnerText;
            }

            XmlNodeList userCustomProperties = xmlDoc.SelectNodes("/users/user/custom-properties");
            foreach (XmlNode userCustomProperty in userCustomProperties)
            {
                //MessageBox.Show("Node: " + userCustomProperty.LocalName);
                if (userCustomProperty.SelectSingleNode("phone") != null)
                    ubUserResponse.phone = userCustomProperty.SelectSingleNode("phone").InnerText;
                if (userCustomProperty.SelectSingleNode("mobile") != null)
                    ubUserResponse.mobile = userCustomProperty.SelectSingleNode("mobile").InnerText;
                if (userCustomProperty.SelectSingleNode("address") != null)
                    ubUserResponse.address = userCustomProperty.SelectSingleNode("address").InnerText;
                if (userCustomProperty.SelectSingleNode("office-number") != null)
                    ubUserResponse.officePhone = userCustomProperty.SelectSingleNode("office-number").InnerText;
            }
        }

        #endregion
    }
}
