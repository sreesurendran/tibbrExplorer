using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Xml;

namespace tibbrExplorer.Response
{
    class GetSubjectFollowersResponse : RestResponse
    {
        #region
        //Attributes
        private List<UserBean> lstUsersFollowingSubject = new List<UserBean>();

        #endregion

        #region
        //Properties

        public List<UserBean> usersResponse
        {
            get { return lstUsersFollowingSubject; }
            set { lstUsersFollowingSubject = value; }
        }

        #endregion

        #region
        //Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {
            XmlNodeList users = xmlDoc.SelectNodes("/users/user");
            foreach (XmlNode user in users)
            {
                UserBean ubSubjectFollowerUser = new UserBean();
                if (user.SelectSingleNode("deleted") != null)
                    ubSubjectFollowerUser.isDeleted = user.SelectSingleNode("deleted").InnerText;
                if (user.SelectSingleNode("description") != null)
                    ubSubjectFollowerUser.aboutMe = user.SelectSingleNode("description").InnerText;
                if (user.SelectSingleNode("email") != null)
                    ubSubjectFollowerUser.email = user.SelectSingleNode("email").InnerText;
                if (user.SelectSingleNode("first-name") != null)
                    ubSubjectFollowerUser.firstName = user.SelectSingleNode("first-name").InnerText;
                if (user.SelectSingleNode("id") != null)
                    ubSubjectFollowerUser.userId = user.SelectSingleNode("id").InnerText;
                if (user.SelectSingleNode("last-name") != null)
                    ubSubjectFollowerUser.lastName = user.SelectSingleNode("last-name").InnerText;
                if (user.SelectSingleNode("login") != null)
                    ubSubjectFollowerUser.loginName = user.SelectSingleNode("login").InnerText;
                if (user.SelectSingleNode("display-name") != null)
                    ubSubjectFollowerUser.displayNameStd = user.SelectSingleNode("display-name").InnerText;
                if (user.SelectSingleNode("displayName") != null)
                    ubSubjectFollowerUser.displayNameCustom = user.SelectSingleNode("displayName").InnerText;
                lstUsersFollowingSubject.Add(ubSubjectFollowerUser);
            }

        }

        #endregion

    }
}
