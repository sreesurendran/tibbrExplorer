using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Xml;

namespace tibbrExplorer.Response
{
    class GetRoleMembersResponse : RestResponse
    {
        #region
        //Attributes
        private List<UserBean> lstUsersinRole = new List<UserBean>();

        #endregion

        #region
        //Properties
        
        public List<UserBean> usersResponse
        {
            get { return lstUsersinRole; }
            set { lstUsersinRole = value; }
        }

        #endregion

        #region
        //Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {            
            XmlNodeList users = xmlDoc.SelectNodes("/users/user");
            foreach (XmlNode user in users)
            {
                UserBean ubRoleUser = new UserBean();
                if (user.SelectSingleNode("deleted") != null)
                    ubRoleUser.isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("description") != null)
                    ubRoleUser.aboutMe = user.SelectSingleNode("description").InnerText;
                if (user.SelectSingleNode("email") != null)
                    ubRoleUser.email = user.SelectSingleNode("email").InnerText;
                if (user.SelectSingleNode("first-name") != null)
                    ubRoleUser.firstName = user.SelectSingleNode("first-name").InnerText;
                if (user.SelectSingleNode("id") != null)
                    ubRoleUser.userId = user.SelectSingleNode("id").InnerText;
                if (user.SelectSingleNode("last-name") != null)
                    ubRoleUser.lastName = user.SelectSingleNode("last-name").InnerText;
                if (user.SelectSingleNode("login") != null)
                    ubRoleUser.loginName = user.SelectSingleNode("login").InnerText;
                if (user.SelectSingleNode("display-name") != null)
                    ubRoleUser.displayNameStd = user.SelectSingleNode("display-name").InnerText;
                if (user.SelectSingleNode("displayName") != null)
                    ubRoleUser.displayNameCustom = user.SelectSingleNode("displayName").InnerText;
                lstUsersinRole.Add(ubRoleUser);
            }
            
        }

        #endregion

    }
}
