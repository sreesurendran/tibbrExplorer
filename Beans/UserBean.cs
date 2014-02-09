using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tibbrExplorer.Beans
{
    public class UserBean
    {
        #region
        //Attributes
        private string strId;
        private string strClientKey;
        private string strAuthToken;
        private string strLogin;
        private string strFName;
        private string strLName;
        private string strEmail;
        private string strDeleted;
        private string strAboutMe;
        private string strDisplayNameStd;
        private string strDisplayNameCustom;
        private string strPhone;
        private string strOfficePhone;
        private string strMobile;
        private string strAddress;

        #endregion

        #region
        //Properties
        public string userId
        {
            get { return strId; }
            set { strId = value; }
        }
        public string clientKey
        {
            get { return strClientKey; }
            set { strClientKey = value; }
        }
        public string authToken
        {
            get { return strAuthToken; }
            set { strAuthToken = value; }
        }
        public string loginName
        {
            get { return strLogin; }
            set { strLogin = value; }
        }
        public string firstName
        {
            get { return strFName; }
            set { strFName = value; }
        }
        public string lastName
        {
            get { return strLName; }
            set { strLName = value; }
        }
        public string email
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string isDeleted
        {
            get { return strDeleted; }
            set { strDeleted = value; }
        }
        public string aboutMe
        {
            get { return strAboutMe; }
            set { strAboutMe = value; }
        }
        public string displayNameStd
        {
            get { return strDisplayNameStd; }
            set { strDisplayNameStd = value; }
        }
        public string displayNameCustom
        {
            get { return strDisplayNameCustom; }
            set { strDisplayNameCustom = value; }
        }
        public string phone
        {
            get { return strPhone; }
            set { strPhone = value; }
        }
        public string officePhone
        {
            get { return strOfficePhone; }
            set { strOfficePhone = value; }
        }
        public string mobile
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        public string address
        {
            get { return strAddress; }
            set { strAddress = value; }
        }


        #endregion


        #region
        //Constructor

        #endregion

        #region
        //Methods

        #endregion






    }
}
