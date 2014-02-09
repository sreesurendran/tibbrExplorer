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
    class AddRoleMembersRequest : RestRequest
    {
        #region
        //Attributes
        private UserBean ubLoggedinUser;
        private string strUsertoAddtoRole;
        private string strRoleId;
        private string strContext;

        #endregion

        #region
        //Constructor
        public AddRoleMembersRequest(UserBean loggedinUser, string userstoAddtoRole, string roleId)
        {
            ubLoggedinUser = loggedinUser;
            strUsertoAddtoRole = userstoAddtoRole;
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
            qString["params[user_ids]"] = strUsertoAddtoRole;
            strURI = qString.ToString();
            strContext = "/a/roles/" + strRoleId.Trim() + "/add_members.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion

    }
}
