using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Web;
using System.Windows.Forms;

namespace tibbrExplorer.Request
{
    class SearchUserRequest : RestRequest
    {
        #region Attributes       
        private string strUserLoginToSearch;
        private UserBean ubLoggedinUser;
        private string strIncludeInaccessible;
        private string strIncludeDeleted;
        private string strContext;

        #endregion

        #region Constructor

        public SearchUserRequest(UserBean loggedinUser, string userLoginToSearch, string includeInaccessible, string includeDeleted)
        {
            strUserLoginToSearch = userLoginToSearch;
            ubLoggedinUser = loggedinUser;
            strIncludeInaccessible = includeInaccessible;
            strIncludeDeleted = includeDeleted;
        }

        #endregion

        #region Methods
        public override string getURLString()
        {
            string strURI;
            System.Collections.Specialized.NameValueCollection qString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            qString["client_key"] = ubLoggedinUser.clientKey;
            qString["auth_token"] = ubLoggedinUser.authToken;
            qString["params[login]"] = strUserLoginToSearch;
            qString["params[include_deleted]"] = strIncludeDeleted;
            qString["params[include_inaccessible]"] = strIncludeInaccessible;
            strURI = qString.ToString();
            strContext = "/a/users/" + ubLoggedinUser.userId + "/search_users.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion
    }
}
