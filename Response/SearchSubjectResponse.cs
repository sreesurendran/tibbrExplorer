using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using tibbrExplorer.Beans;

namespace tibbrExplorer.Response
{
    class SearchSubjectResponse : RestResponse
    {
        #region Attributes
        private SubjectBean sbSubjectResponse = new SubjectBean();

        #endregion

        #region Properties
        public SubjectBean subjectResponse
        {
            get { return sbSubjectResponse; }
            set { sbSubjectResponse = value; }
        }

        #endregion

        #region Methods
        public override void parseDoc(XmlDocument xmlDoc)
        {
            XmlNodeList subjects = xmlDoc.SelectNodes("/subjects/subject");
            foreach (XmlNode subject in subjects)
            {
                if (subject.SelectSingleNode("allow-children") != null)
                    sbSubjectResponse.allowchildren = subject.SelectSingleNode("allow-children").InnerText;
                if (subject.SelectSingleNode("deleted") != null)
                    sbSubjectResponse.isDeleted = subject.SelectSingleNode("deleted").InnerText;
                if (subject.SelectSingleNode("description") != null)
                    sbSubjectResponse.description = subject.SelectSingleNode("description").InnerText;
                if (subject.SelectSingleNode("id") != null)
                    sbSubjectResponse.subjectId = subject.SelectSingleNode("id").InnerText;
                if (subject.SelectSingleNode("name") != null)
                    sbSubjectResponse.name = subject.SelectSingleNode("name").InnerText;
                if (subject.SelectSingleNode("scope") != null)
                    sbSubjectResponse.scope = subject.SelectSingleNode("scope").InnerText;
            }

        }

        #endregion

    }
}
