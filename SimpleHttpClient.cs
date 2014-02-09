using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tibbrExplorer.Request;
using tibbrExplorer.Response;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace tibbrExplorer
{
    class SimpleHttpClient
    {
        #region
        //Methods
        public static void execute(RestRequest req, RestResponse res, string httpMethod)
        {
            //MessageBox.Show("Req URL: " + req.getURLString());
            Uri reqUri = new Uri(req.getURLString());

            //Workaround to make the client application accept all ther server certificates
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(reqUri);
            wr.Method = httpMethod;
            wr.ContentType = "application/xml";
            wr.Headers.Add("Request", "message");
            wr.Headers.Add("Accept-Charset", "UTF-8;q=0.7,*;q=0.7");
            wr.KeepAlive = false;
            wr.ServicePoint.ConnectionLimit = 1;
            /*
            ProxyDefinition pd = new ProxyDefinition();
            wr.Proxy = pd.wprox;
            */

            //wr.Proxy = new WebProxy("127.0.0.1", 8888);

            if (req.getContent() != null)
            {
                try
                {
                    string strRequestXML = req.getContent();
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] inputXML = Encoding.UTF8.GetBytes(strRequestXML);
                    wr.ContentLength = inputXML.Length;
                    Stream reqStream = wr.GetRequestStream();
                    reqStream.Write(inputXML, 0, inputXML.Length);
                    //reqStream.Flush();
                    reqStream.Close();
                }
                catch (Exception exp)
                {
                    Logger.WriteLog("\r\n---- Error ----\r\n");
                    if (exp.InnerException != null)
                        Logger.WriteLog("\r\n" + exp.InnerException.ToString() + "\r\n");
                    Logger.WriteLog("\r\n" + exp.StackTrace + "\r\n");
                    Logger.WriteLog("\r\n" + exp.Message + "\r\n");
                    Logger.WriteLog("\r\n---------------\r\n\r\n");
                }
            }
            
            //MessageBox.Show("Address: " + wr.Address);

            try
            {                
                HttpWebResponse wrp = (HttpWebResponse)wr.GetResponse();
                if (wrp.ContentLength > 1)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(wrp.GetResponseStream());
                    res.parseDoc(xmlDoc);                    
                }
                res.statusCode = wrp.StatusCode;
                wrp.Close();
            }
            catch (WebException exp)
            {
                Logger.WriteLog("\r\n---- Error ----\r\n");
                MessageBox.Show(exp.StackTrace);
                //MessageBox.Show(exp.Data.ToString());
                if (exp.InnerException != null)
                {
                    MessageBox.Show(exp.InnerException.ToString());
                    Logger.WriteLog("\r\n" + exp.InnerException.ToString() + "\r\n");
                }
                MessageBox.Show(exp.Message);
                Logger.WriteLog("\r\n" + exp.Message + "\r\n");

                //string strMessage = exp.Message;
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
                    
                }
                Logger.WriteLog("\r\n---------------\r\n\r\n");
            }


            //if (wrp.StatusCode != HttpStatusCode.OK)
            //{
            //    throw new Exception("Error calling service\nStatus: " + wrp.StatusCode.ToString() + "\nDescription: " + wrp.StatusDescription.ToString());
            //}

            /*
            Stream objStream = wrp.GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            MessageBox.Show(objReader.ReadToEnd());
            //objReader.Close();
            */

        }


        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        #endregion


        
    }

}
