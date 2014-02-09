using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms;
using tibbrExplorer.Request;
using tibbrExplorer.Response;
using tibbrExplorer.Beans;
using System.Web;

namespace tibbrExplorer
{
    public partial class tibbrExplorerLogin : Form
    {
        string strEnvFile = "";
        public tibbrExplorerLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //validate whether all the fields have been entered
            btnLogin.Enabled = false;

            if ((cboEnvironment.Text == null || cboEnvironment.Text == "") ||
                    (txtUsername.Text == null || txtUsername.Text == "") ||
                        (txtPassword.Text == null || txtPassword.Text == "") ||
                            (txtClientKey.Text == null || txtClientKey.Text == ""))
            {
                if (cboEnvironment.Text == null || cboEnvironment.Text == "")
                {
                    MessageBox.Show("Please select an environment");
                }
                if (txtUsername.Text == null || txtUsername.Text == "")
                {
                    MessageBox.Show("Please enter a valid username");
                }
                if (txtPassword.Text == null || txtPassword.Text == "")
                {
                    MessageBox.Show("Please enter a password");
                }
                if (txtClientKey.Text == null || txtClientKey.Text == "")
                {
                    MessageBox.Show("Please enter a valid client key");
                }
            }
            else
            {
                //if the current enviornment doesn't exist in Ennvironment.ini, append it to the file 
                if (strEnvFile != null)
                {
                    if (!strEnvFile.ToLower().Trim().Contains(cboEnvironment.Text.Trim()))
                    {
                        try
                        {
                            StreamWriter swEnv = new StreamWriter(@"Environments.ini", true);
                            //swEnv.Write("\n" + cboEnvironment.Text.Trim());
                            swEnv.WriteLine(cboEnvironment.Text.Trim());
                            swEnv.Close();
                        }
                        catch (Exception exp)
                        {
                            Logger.WriteLog("\r\nUnable to update Environments.ini file\r\n");
                        }
                    }
                }
                /*
                StreamReader srEnv = new StreamReader(@"Environments.ini");
                string strFile = srEnv.ReadToEnd();
                MessageBox.Show(strFile);
                srEnv.Close();
                */

                //write into Configuration.ini file            
                List<string> lstConfigLines = new List<string>();
                lstConfigLines.Add("username;" + txtUsername.Text.Trim());
                lstConfigLines.Add("password;" + txtPassword.Text.Trim());
                lstConfigLines.Add("client_key;" + txtClientKey.Text.Trim());
                lstConfigLines.Add("environment;" + cboEnvironment.Text.Trim());

                try
                {
                    StreamWriter swConfig = new StreamWriter(@"Configuration.ini", false);
                    foreach (string strConfigLine in lstConfigLines)
                    {
                        swConfig.WriteLine(strConfigLine);
                    }
                    swConfig.Close();
                }
                catch (Exception exp)
                {
                    Logger.WriteLog("\r\nUnable to update Configuration.ini file\r\n");
                }
                /*
                StreamReader srConfig = new StreamReader(@"Configuration.ini");
                string strFile = srConfig.ReadToEnd();
                MessageBox.Show(strFile);
                srConfig.Close();
                */

                //login to tibbr to get the auth_token
                loginTibbr();
            }
            btnLogin.Enabled = true;
        }

        private void tibbrExplorerLogin_Load(object sender, EventArgs e)
        {
            //initaite the log file
            Logger.logPath = Application.StartupPath + "\\Log Files\\tibbrExplorerLog_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
            MessageBox.Show("Path: " + Logger.logPath);
            Logger.WriteLog("\r\n************ LOG ************\r\n");

            if (!Directory.Exists(@"Log Files"))
            {
                try
                {
                    Directory.CreateDirectory(@"Log Files");
                }
                catch (Exception exp)
                {
                    Logger.WriteLog("\r\nUnable to create 'Log Files' folder\r\n");
                }
            }

            try
            {
                //populate the list of environments into cboEnviornment from the Environments.ini file
                StreamReader srEnv = new StreamReader(@"Environments.ini");                
                do
                {
                    string strLine = srEnv.ReadLine().Trim();
                    cboEnvironment.Items.Add(strLine);    
                    strEnvFile+=strLine + "\n";

                } while (srEnv.Peek() != -1);
                srEnv.Close();
                
            }
            catch(System.Exception envexp)
            {
                Logger.WriteLog("\r\nCould not fetch data from Environment.ini\r\n");
            }
            

            try
            {
                //populate the username, password and client_key into txtUsername, txtPassword, txtClientKey from the Configuration.ini file                
                StreamReader srConfig = new StreamReader(@"Configuration.ini");                
                char[] delim = new char[] { ';' };
                do
                {
                    string[] strParts = srConfig.ReadLine().Split(delim);
                    if (strParts[0].ToLower().Trim().Contains("username"))
                        txtUsername.Text = strParts[1].Trim();
                    else if (strParts[0].ToLower().Trim().Contains("password"))
                        txtPassword.Text = strParts[1].Trim();
                    else if (strParts[0].ToLower().Trim().Contains("client_key"))
                        txtClientKey.Text = strParts[1].Trim();
                    else if (strParts[0].ToLower().Trim().Contains("environment"))
                        cboEnvironment.Text = strParts[1].Trim();

                }while(srConfig.Peek() != -1);
                srConfig.Close();
            }
            catch (Exception configexp)
            {
                Logger.WriteLog("\r\nCould not fetch data from Configuration.ini\r\n");
            }
        }

        private void loginTibbr()
        {
            UserBean loggedinUser = new UserBean();
            RestRequest.setBase(cboEnvironment.Text.Trim());
            LoginRequest logReq = new LoginRequest(txtUsername.Text.Trim(), txtPassword.Text.Trim(), "false", txtClientKey.Text.Trim());
            UserResponse logRes = new UserResponse();
            SimpleHttpClient.execute(logReq, logRes, "POST");
            if (logRes.userResponse.userId != null)
            {
                MessageBox.Show("UserID: " + logRes.userResponse.userId + "\nAuth-token: " + logRes.userResponse.authToken);
                Logger.WriteLog("\r\nLogin successful\r\n");
                Logger.WriteLog("\r\nLogged-in user: " + logRes.userResponse.displayNameStd);
                Logger.WriteLog("\r\nLogged-in userID: " + logRes.userResponse.userId.Trim());
                Logger.WriteLog("\r\nLogged-in user authToken: " + logRes.userResponse.authToken + "\r\n");
                //loggedinUser.authToken = logRes.userResponse.authToken;
                //loggedinUser.userId = logRes.userResponse.userId;                
                loggedinUser = logRes.userResponse;
                loggedinUser.clientKey = txtClientKey.Text.Trim();
                tibbrExplorerMain runAPI = new tibbrExplorerMain(loggedinUser);
                this.SendToBack();
                runAPI.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login unsuccessful\n\nPlease check your login credentials and try again");
                Logger.WriteLog("\r\nLogin unsuccessful");
                Logger.WriteLog("\r\nPlease check your login credentials and try again\r\n");
            }
            
        }

       
    }
}
