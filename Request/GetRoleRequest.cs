using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Web;
using System.Collections.Specialized;

namespace tibbrExplorer.Request
{
    class GetRoleRequest : RestRequest
    {
        #region
        //Attributes
        private string strContextId;
        private UserBean ubLoggedinUser;
        private string strContextType;
        private string strRoleName;
        private string strContext;
        private string strIncludeMembers;

        #endregion

        #region
        //Constructor
        public GetRoleRequest(UserBean loggedinUser, string contextId, string contextType, string roleName, string includeMembers)
        {
            strContextId = contextId;
            ubLoggedinUser = loggedinUser;
            strContextType = contextType;
            strRoleName = roleName;
            strIncludeMembers = includeMembers;
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
            qString["params[context_id]"] = strContextId;
            qString["params[rolename]"] = strRoleName;
            qString["params[include_members]"] = strIncludeMembers;
            qString["params[context_type]"] = strContextType;
            strURI = qString.ToString();
            strContext = "/a/roles/find_by_rolename_and_context.xml?";            

            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion

    }
}
