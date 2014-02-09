using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using tibbrExplorer.Beans;
using System.Windows.Forms;

namespace tibbrExplorer.Request
{
    class RemoveRoleMembersRequest : RestRequest
    {
        #region
        //Attributes
        private UserBean ubLoggedinUser;
        private string strUserstoRemovefromRole;
        private string strRoleId;
        private string strContext;

        #endregion

        #region
        //Constructor
        public RemoveRoleMembersRequest(UserBean loggedinUser, string userstoRemovefromRole, string roleId)
        {
            ubLoggedinUser = loggedinUser;
            strUserstoRemovefromRole = userstoRemovefromRole;
            strRoleId = roleId;
        }

        #endregion

        #region
        //Methods
        public override string getURLString()
        {
            string strURI;
            NameValueCollection qString = HttpUtility.ParseQueryString(string.Empty);
            qString["client_key"] = ubLoggedinUser.clientKey;
            qString["auth_token"] = ubLoggedinUser.authToken;
            qString["params[user_ids]"] = strUserstoRemovefromRole;
            strURI = qString.ToString();
            strContext = "/a/roles/" + strRoleId.Trim() + "/remove_members.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion

    }
}
