using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Request;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net;
using tibbrExplorer.Response;
using tibbrExplorer.Beans;
using tibbrExplorer.Request;

namespace tibbrExplorer
{
    class MultipartContentHttpClient
    {
        #region
        //Methods
        public static void execute(RestResponse res, string resourceID, string subjectName, string imagePath, UserBean loggedInUser, string resourceType, string httpMethod)
        {
            string strURL = "";
            string strInputXML = "";
            Uri uriUpload;
            switch (resourceType)
            {
                case "user":
                    strURL = RestRequest.strBase + "/a/users/" + resourceID.Trim() + ".xml";
                    strInputXML = "<user><id type=\"integer\">" + resourceID.Trim() + "</id><profile-image>tibbr_attachment_part_0</profile-image></user>";
                    uriUpload = new Uri(strURL.Trim());
                    executeImageUpload(res, imagePath, loggedInUser, uriUpload, strInputXML, httpMethod,resourceID);

                    break;

                case "subject":
                    strURL = RestRequest.strBase + "/a/subjects/" + resourceID.Trim() + ".xml";
                    strInputXML = "<subject><id type=\"integer\">" + resourceID.Trim() + "</id><name>" + subjectName.Trim() + "</name><subject-image>tibbr_attachment_part_0</subject-image></subject>";
                    uriUpload = new Uri(strURL.Trim());
                    executeImageUpload(res, imagePath, loggedInUser, uriUpload, strInputXML, httpMethod,resourceID);

                    break;

                case "attachment":
                    break;

                default:
                    break;
            }

        }


        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void executeImageUpload(RestResponse res, string imagePath, UserBean loggedInUser, Uri uploadUri, string requestXML, string httpMethod, string resourceId)
        {
            //Workaround to make the client application accept all ther server certificates
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            string strBoundary = "###";

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uploadUri);
            wr.Method = httpMethod;
            wr.ContentType = "multipart/mixed; boundary=" + strBoundary;
            wr.Accept = "application/xml,text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2";
            wr.Headers.Add("Cache-Control", "no-cache");
            wr.Headers.Add("client_key", loggedInUser.clientKey);
            wr.Headers.Add("auth_token", loggedInUser.authToken);
            //wr.Headers.Add("impersonate_user_id", resourceId);
            wr.KeepAlive = false;
            wr.ServicePoint.ConnectionLimit = 1;

            /*
            ProxyDefinition pd = new ProxyDefinition();
            wr.Proxy = pd.wprox;
            */
                      
            StringBuilder sbCompleteInputXML = new StringBuilder();

            sbCompleteInputXML.Append("--" + strBoundary + "\r\n");
            sbCompleteInputXML.Append("Content-Disposition: name=\"main\";\"\r\n");
            sbCompleteInputXML.Append("Content-ID: main\r\n\r\n");
            sbCompleteInputXML.Append("Content-Type: application/xml\r\n\r\n");
            sbCompleteInputXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sbCompleteInputXML.Append(requestXML);
            sbCompleteInputXML.Append("\r\n\r\n" + "--" + strBoundary + "\r\n");
            sbCompleteInputXML.Append("Content-Disposition: name=\"tibbr_attachment_part_0\"; filename=\"" + imagePath + "\"\r\n");
            sbCompleteInputXML.Append("Content-Type: image/jpeg" + "\r\n\r\n");

            string strCompleteInputXML = sbCompleteInputXML.ToString();

            MessageBox.Show("Input XML: " + strCompleteInputXML);

            StreamWriter swLog = new StreamWriter(@"Log.txt");
            swLog.Write("\n" + strCompleteInputXML);
            swLog.Close();

            /*
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] inputXML = Encoding.UTF8.GetBytes(strCompleteInputXML);
            byte[] inputBoundary = Encoding.UTF8.GetBytes("\r\n" + "--" + strBoundary + "\r\n");
            byte[] inputFile = null;
            FileStream fStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            BinaryReader bReader = new BinaryReader(fStream);
            long totalBytes = new FileInfo(imagePath).Length;
            inputFile = bReader.ReadBytes(Convert.ToInt32(totalBytes));
            var totalInput = new MemoryStream();
            totalInput.Write(inputXML, 0, inputXML.Length);
            totalInput.Write(inputFile, 0, inputFile.Length);
            totalInput.Write(inputBoundary, 0, inputBoundary.Length);
            byte[] combinedInput = totalInput.ToArray();
            wr.ContentLength = combinedInput.Length;
            Stream reqStream = wr.GetRequestStream();
            reqStream.Write(combinedInput, 0, combinedInput.Length);
            MessageBox.Show(reqStream.ToString());
            fStream.Close();
            bReader.Close();
            reqStream.Close();
            */

            /*
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] inputXML = Encoding.UTF8.GetBytes(strCompleteInputXML);
            byte[] inputBoundary = Encoding.UTF8.GetBytes("\r\n" + "--" + strBoundary + "\r\n");
            byte[] inputFile = new byte[10000];
            FileStream fStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            int bytesRead = fStream.Read(inputFile, 0, inputFile.Length);
            wr.ContentLength = inputXML.Length + inputFile.Length + inputBoundary.Length;
            Stream reqStream = wr.GetRequestStream();
            reqStream.Write(inputXML, 0, inputXML.Length);
            while (bytesRead > 0)
            {
                reqStream.Write(inputFile, 0, bytesRead);
                bytesRead = fStream.Read(inputFile, 0, inputFile.Length);
            }
            reqStream.Write(inputBoundary, 0, inputBoundary.Length);
            MessageBox.Show(reqStream.ToString());
            fStream.Close();
            reqStream.Close();
            */

            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] inputXML = Encoding.UTF8.GetBytes(strCompleteInputXML);
                byte[] inputBoundary = Encoding.UTF8.GetBytes("\r\n" + "--" + strBoundary + "\r\n");
                byte[] inputFile = null;
                FileStream fStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader bReader = new BinaryReader(fStream);
                long totalBytes = new FileInfo(imagePath).Length;
                inputFile = bReader.ReadBytes(Convert.ToInt32(totalBytes));
                wr.ContentLength = inputXML.Length + inputFile.Length + inputBoundary.Length;
                Stream reqStream = wr.GetRequestStream();
                reqStream.Write(inputXML, 0, inputXML.Length);
                reqStream.Write(inputFile, 0, inputFile.Length);
                reqStream.Write(inputBoundary, 0, inputBoundary.Length);
                MessageBox.Show(reqStream.ToString());
                fStream.Close();
                bReader.Close();
                reqStream.Close();
            }
            catch (Exception exp)
            {
                Logger.WriteLog("\r\n---- Error ----\r\n");
                Logger.WriteLog("\r\n" + exp.StackTrace + "\r\n");
                if (exp.InnerException != null)
                    Logger.WriteLog("\r\n" + exp.InnerException.ToString() + "\r\n");
                Logger.WriteLog("\r\n" + exp.Message + "\r\n");
                Logger.WriteLog("\r\n---------------\r\n\r\n");
            }

            MessageBox.Show("Address: " + wr.Address);

            try
            {
                HttpWebResponse wrp = (HttpWebResponse)wr.GetResponse();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(wrp.GetResponseStream());
                res.parseDoc(xmlDoc);
                res.statusCode = wrp.StatusCode;
                wrp.Close();
            }
            catch (WebException exp)
            {
                Logger.WriteLog("\r\n---- Error ----\r\n");
                MessageBox.Show(exp.StackTrace);
                if (exp.InnerException != null)
                {
                    MessageBox.Show(exp.InnerException.ToString());
                    Logger.WriteLog("\r\n" + exp.InnerException.ToString() + "\r\n");
                }
                MessageBox.Show(exp.Message);
                Logger.WriteLog("\r\n" + exp.Message + "\r\n");

                string strMessage = "";
                StreamReader objReader;

                if (exp.Response != null)
                {
                    
                    using (objReader = new StreamReader(exp.Response.GetResponseStream()))
                    {
                        strMessage = objReader.ReadToEnd();
                    }
                    MessageBox.Show("strMessage: " + strMessage);
                    if (strMessage != "")
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        byte[] byteArray = Encoding.UTF8.GetBytes(strMessage);
                        MemoryStream ms = new MemoryStream(byteArray);
                        xmlDoc.Load(ms);
                        ErrorResponse.parseErrorXML(xmlDoc);
                        Logger.WriteLog("\r\n" + ErrorResponse.errorMessage + "\r\n");
                    }

                    //Logger.WriteLog("\r\n" + strMessage + "\r\n");
                    objReader.Close();

                    foreach (string key in exp.Response.Headers.Keys)
                    {
                        MessageBox.Show("Key: " + key + "; Value: " + exp.Response.Headers[key]);
                    }                                        
                }
                Logger.WriteLog("\r\n---------------\r\n\r\n");
            }


        }


        #endregion

    }
}
