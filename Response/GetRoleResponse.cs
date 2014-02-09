using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Xml;

namespace tibbrExplorer.Response
{
    class GetRoleResponse : RestResponse
    {
        #region
        //Attributes
        //private List<UserBean> lstUsersinRole = new List<UserBean>();
        private string strRoleId;

        #endregion

        #region
        //Properties
        /*
        public List<UserBean> usersResponse
        {
            get { return lstUsersinRole; }
            set { lstUsersinRole = value; }
        }
        */
        public string roleId
        {
            get { return strRoleId; }
            set { strRoleId = value; }
        }

        #endregion

        #region
        //Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {
            //int count = 0;
            XmlNodeList roles = xmlDoc.SelectNodes("role");
            foreach (XmlNode role in roles)
            {
                if (role.SelectSingleNode("id") != null)
                    strRoleId = role.SelectSingleNode("id").InnerText;
            }
            
            /*
            XmlNodeList users = xmlDoc.SelectNodes("/role/members/user");
            foreach (XmlNode user in users)
            {
                if (user.SelectSingleNode("deleted") != null)
                    lstUsersinRole[count].isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("description") != null)
                    lstUsersinRole[count].aboutMe = user.SelectSingleNode("description").InnerText;
                if (user.SelectSingleNode("email") != null)
                    lstUsersinRole[count].email = user.SelectSingleNode("email").InnerText;
                if (user.SelectSingleNode("first-name") != null)
                    lstUsersinRole[count].firstName = user.SelectSingleNode("first-name").InnerText;
                if (user.SelectSingleNode("id") != null)
                    lstUsersinRole[count].userId = user.SelectSingleNode("id").InnerText;
                if (user.SelectSingleNode("last-name") != null)
                    lstUsersinRole[count].lastName = user.SelectSingleNode("last-name").InnerText;
                if (user.SelectSingleNode("display-name") != null)
                    lstUsersinRole[count].displayNameStd = user.SelectSingleNode("display-name").InnerText;
                if (user.SelectSingleNode("displayName") != null)
                    lstUsersinRole[count].displayNameCustom = user.SelectSingleNode("displayName").InnerText;
                count++;
            }
            */
        }

        #endregion

    }
}
