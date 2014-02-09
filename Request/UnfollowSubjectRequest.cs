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
    class UnfollowSubjectRequest : RestRequest
    {
        #region Attributes
        private UserBean ubLoggedinUser;
        private UserBean ubUsertoUnfollowSubject;
        private SubjectBean sbSubjecttobeUnfolllowed;
        private string strContext;

        #endregion

        #region Constructor
        public UnfollowSubjectRequest(UserBean loggedinUser, UserBean usertoUnfollowSubject, SubjectBean subjecttobeUnfolllowed)
        {
            ubLoggedinUser = loggedinUser;
            ubUsertoUnfollowSubject = usertoUnfollowSubject;
            sbSubjecttobeUnfolllowed = subjecttobeUnfolllowed;
        }

        #endregion

        #region Methods
        public override string getURLString()
        {
            string strURI;
            NameValueCollection qString = HttpUtility.ParseQueryString(string.Empty);
            qString["client_key"] = ubLoggedinUser.clientKey;
            qString["auth_token"] = ubLoggedinUser.authToken;
            qString["params[subject_id]"] = sbSubjecttobeUnfolllowed.subjectId.Trim();
            strURI = qString.ToString();
            strContext = "/a/users/" + ubUsertoUnfollowSubject.userId.Trim() + "/unsubscribe.xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            return null;
        }

        #endregion

    }
}
