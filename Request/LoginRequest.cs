using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace tibbrExplorer.Request
{
    class LoginRequest : RestRequest
    {

        #region
        //Attributes
        string m_login = "";
        string m_password = "";
        string m_remember_me = "";
        string m_client_key = "";
        string m_context = "/a/users/login.xml?";

        #endregion

        #region
        //Constructor
        public LoginRequest(string login, string password, string remember_me, string client_key)
        {
            m_login = login;
            m_password = password;
            m_remember_me = remember_me;
            m_client_key = client_key;
        }

        #endregion

        #region
        //Properties
        public string clientKey
        {
            get { return m_client_key; }
            set { m_client_key = value; }
        }

        #endregion

        #region
        //Methods
        public string getURI()
        {
            string strURI;
            NameValueCollection qString = HttpUtility.ParseQueryString(string.Empty);
            qString["params[login]"] = m_login;
            qString["params[password]"] = m_password;
            qString["params[remember_me]"] = m_remember_me;
            qString["client_key"] = m_client_key;            
            strURI = qString.ToString();
            return strURI;
        }
        
        public override string getURLString()
        {
            return strBase + m_context + getURI();
        }

        public override string getContent()
        {
            return null;
        }

        #endregion


    }
}
