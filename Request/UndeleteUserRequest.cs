using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Collections.Specialized;
using System.Web;

namespace tibbrExplorer.Request
{
    class UndeleteUserRequest : RestRequest
    {
        #region
        //Attributes        
        private UserBean ubLoggedinUser;
        private UserBean ubUserToBeUndeleted;
        private string strContext;

        #endregion

        #region
        //Constructor
        public UndeleteUserRequest(UserBean loggedinUser, UserBean userToUndelete)
        {
            ubLoggedinUser = loggedinUser;
            ubUserToBeUndeleted = userToUndelete;
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
            strURI = qString.ToString();
            strContext = "/a/users/" + ubUserToBeUndeleted.userId + "/undelete.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }


        #endregion

    }
}
