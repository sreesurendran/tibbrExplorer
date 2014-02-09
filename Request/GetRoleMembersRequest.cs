using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Collections.Specialized;
using System.Web;
using System.Windows.Forms;

namespace tibbrExplorer.Request
{
    class GetRoleMembersRequest : RestRequest
    {
        #region
        //Attributes        
        private UserBean ubLoggedinUser;
        private string strRoleId;
        private string strPageNumber;
        private string strNumMembersPerPage;
        private string strContext;

        #endregion

        #region
        //Constructor
        public GetRoleMembersRequest(UserBean loggedinUser, string roleId, string pageNumber, string numMembersPerPage)
        {
            ubLoggedinUser = loggedinUser;
            strRoleId = roleId;
            strPageNumber = pageNumber;
            strNumMembersPerPage = numMembersPerPage;
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
            qString["params[page]"] = strPageNumber;
            qString["params[per_page]"] = strNumMembersPerPage;
            strURI = qString.ToString();
            strContext = "/a/roles/" + strRoleId.Trim() + "/members.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }


        #endregion

    }
}
