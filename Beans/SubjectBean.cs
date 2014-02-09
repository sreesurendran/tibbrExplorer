using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tibbrExplorer.Beans
{
    class SubjectBean
    {
        #region Attributes
        private string strId;
        private string strName;
        private string strRenderName;
        private string strDescription;
        private string strScope;
        private string strAllowChildren;
        private string strDeleted;

        #endregion

        #region Properties
        public string subjectId
        {
            get { return strId; }
            set { strId = value; }
        }
        public string name
        {
            get { return strName; }
            set { strName = value; }
        }
        public string rendername
        {
            get { return strRenderName; }
            set { strRenderName = value; }
        }
        public string description
        {
            get { return strDescription; }
            set { strDescription = value; }
        }
        public string scope
        {
            get { return strScope; }
            set { strScope = value; }
        }
        public string allowchildren
        {
            get { return strAllowChildren; }
            set { strAllowChildren = value; }
        }
        public string isDeleted
        {
            get { return strDeleted; }
            set { strDeleted = value; }
        }

        #endregion


    }
}
