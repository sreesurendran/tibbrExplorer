using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;

namespace tibbrExplorer.Request
{
    class SearchSubjectRequest : RestRequest
    {
        #region Attributes       
        private string strSubjectNameToSearch;
        private UserBean ubLoggedinUser;
        private string strIncludeInaccessible;
        private string strIncludeDeleted;
        private string strContext;

        #endregion

        #region Constructor

        public SearchSubjectRequest(UserBean loggedinUser, string subjectNameToSearch, string includeInaccessible, string includeDeleted)
        {
            strSubjectNameToSearch = subjectNameToSearch;
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
            qString["params[name]"] = strSubjectNameToSearch;
            qString["params[include_deleted]"] = strIncludeDeleted;
            qString["params[include_inaccessible]"] = strIncludeInaccessible;
            strURI = qString.ToString();
            strContext = "/a/users/" + ubLoggedinUser.userId + "/search_subjects.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion

    }
}
