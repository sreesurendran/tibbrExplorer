using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Beans;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Web;

namespace tibbrExplorer.Request
{
    class UpdateUserRequest : RestRequest
    {
        #region
        //Attributes        
        private UserBean ubLoggedinUser;
        private UserBean ubUserToBeUpdated;
        private string strContext;
        private string strInputXML;
        private Dictionary<string, string> deInputXML = new Dictionary<string, string>();

        #endregion

        #region
        //Properties
        public string inputXML
        {
            get { return strInputXML; }
            set { strInputXML = value; }
        }

        #endregion

        #region
        //Constructor
        public UpdateUserRequest(UserBean loggedinUser, UserBean userToBeUpdated, Dictionary<string, string> de)
        {
            ubLoggedinUser = loggedinUser;
            ubUserToBeUpdated = userToBeUpdated;
            deInputXML = de;
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
            qString["impersonate_user_id"] = ubUserToBeUpdated.userId.Trim();
            strURI = qString.ToString();
            strContext = "/a/users/ " + ubUserToBeUpdated.userId.Trim() + ".xml?";            
            return strBase + strContext + strURI;
        }

        public override string getContent()
        {
            //create the input xml for user creation from deInputXML
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<user>");
            foreach(KeyValuePair<string,string> kvp in deInputXML)
            {
                if (kvp.Value != null)
                {
                    sb.Append("<" + kvp.Key.ToString() + ">");
                    sb.Append(kvp.Value.ToString());
                    sb.Append("</" + kvp.Key.ToString() + ">");
                }
            }
            sb.Append("</user>");
            MessageBox.Show(sb.ToString());
            return sb.ToString();            
        }

        #endregion

    }
}
