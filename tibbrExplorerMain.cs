using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tibbrExplorer.Beans;
using tibbrExplorer.Request;
using tibbrExplorer.Response;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace tibbrExplorer
{
    public partial class tibbrExplorerMain : Form
    {
        #region Attributes
        private UserBean ubLoggedinUser;
        private UserBean ubFetchedSingleUser = new UserBean();
        private UserBean ubFetchedSingleUserUpdate = new UserBean();
        private UserBean ubFetchedSingleUserEditRole = new UserBean();
        private static string strNumberofEntriesPerPage = "5000";
        #endregion

        #region Constructor
        public tibbrExplorerMain(UserBean loggedinUser)
        {
            InitializeComponent();
            ubLoggedinUser = loggedinUser;
            gboxUserSingleFetch.Visible = false;
            gboxUserSingleFetchUpdateFixed.Visible = false;
            gboxUserSingleFetchUpdateEditable.Visible = false;
            gboxUserSingleFetchEditRole.Visible = false;
            btnBSCreateLog.Enabled = false;
        }
        #endregion

        private void tibbrExplorerMain_Load(object sender, EventArgs e)
        {
            try
            {
                //populate the list of roles into lbUserSingleEditRoleFetchTargetRole from the Roles.ini file
                StreamReader srRoles = new StreamReader(@"Roles.ini");
                do
                {
                    string strLine = srRoles.ReadLine().Trim();
                    lbUserSingleEditRoleFetchTargetRole.Items.Add(strLine);

                } while (srRoles.Peek() != -1);
                srRoles.Close();

            }
            catch (System.Exception envexp)
            {
                //swLog.WriteLine("Could not fetch data from Roles.ini; " + envexp.Message);
            }

        }

        #region Methods
        
        #region API Calls
        private UserBean fetchUser(UserBean ubLoggedinUser, string userNameToFetch, string includeInaccessible, string includeDeleted)
        {
            //Logger.WriteLog("Attempting to fetch user: " + userNameToFetch + "....\r\n");
            SearchUserRequest searchUserReq = new SearchUserRequest(ubLoggedinUser, userNameToFetch, includeInaccessible, includeDeleted);
            SearchUserResponse searchUserRes = new SearchUserResponse();
            SimpleHttpClient.execute(searchUserReq, searchUserRes, "GET");
            /*
            if (searchUserRes.userResponse.userId != null)
            {
                Logger.WriteLog("\r\nUser found\r\nFetched userID: " + searchUserRes.userResponse.userId.Trim() + "\r\n");
            }
            else
            {
                Logger.WriteLog("\r\nUser not found\r\n\r\nPlease enter the username and try again\r\n");
            }
            */
            return searchUserRes.userResponse;
        }

        private UserBean createUser(UserBean ubLoggedinUser, Dictionary<string,string> de)
        {
            //Logger.WriteLog("\r\nAttempting to create user: " + de["login"] + "....\r\n");
            CreateUserRequest createUserReq = new CreateUserRequest(ubLoggedinUser, de);
            CreateUserResponse createUserRes = new CreateUserResponse();
            SimpleHttpClient.execute(createUserReq, createUserRes, "POST");
            /*
            if (createUserRes.userResponse.userId != null)
            {
                MessageBox.Show("Create User successful");
                Logger.WriteLog("\r\nCreate User successful\r\nCreated userID: " + createUserRes.userResponse.userId + "\r\n");
            }
            else
            {
                MessageBox.Show("Create User failed");
                Logger.WriteLog("\r\nUser creation failed\r\n");
            }                
            */
            return createUserRes.userResponse;
        }

        private UserBean updateUser(UserBean ubLoggedinUser, UserBean ubFetchedSingleUserUpdate, Dictionary<string, string> dUpdate)
        {
            //Logger.WriteLog("\r\nAttempting to update user: " + ubFetchedSingleUserUpdate.loginName + "....\r\n");
            UpdateUserRequest updateUserReq = new UpdateUserRequest(ubLoggedinUser, ubFetchedSingleUserUpdate, dUpdate);
            UpdateUserResponse updateUserRes = new UpdateUserResponse();
            SimpleHttpClient.execute(updateUserReq, updateUserRes, "PUT");
            /*
            if (updateUserRes.userResponse.userId != null)
            {
                MessageBox.Show("Update successful");
                Logger.info("\r\nUpdate successful\r\nUpdated userID: " + updateUserRes.userResponse.userId.Trim() + "\r\n");
            }
            else
            {
                MessageBox.Show("Update failed");
                Logger.WriteLog("\r\nUpdate failed\r\n");
            }
            */
            return updateUserRes.userResponse;
        }

        private UserBean undeleteUser(UserBean ubLoggedinUser, UserBean ubFetchedSingleUser)
        {
            UndeleteUserRequest undeleteUserReq = new UndeleteUserRequest(ubLoggedinUser, ubFetchedSingleUser);
            UndeleteUserResponse undeleteUserRes = new UndeleteUserResponse();
            SimpleHttpClient.execute(undeleteUserReq, undeleteUserRes, "PUT");
            return undeleteUserRes.userResponse;
        }

        private SubjectBean createSubject(UserBean ubLoggedinUser, Dictionary<string, string> dCreate)
        {
            CreateSubjectRequest createSubjectReq = new CreateSubjectRequest(ubLoggedinUser, dCreate);
            CreateSubjectResponse createSubjectRes = new CreateSubjectResponse();
            SimpleHttpClient.execute(createSubjectReq, createSubjectRes, "POST");            
            return createSubjectRes.subjectResponse;
        }

        private SubjectBean fetchSubject(UserBean ubLoggedinUser, string subjectNameToFetch, string includeInaccessible, string includeDeleted)
        {
            SearchSubjectRequest searchSubjectReq = new SearchSubjectRequest(ubLoggedinUser, subjectNameToFetch, includeInaccessible, includeDeleted);
            SearchSubjectResponse searchSubjectRes = new SearchSubjectResponse();
            SimpleHttpClient.execute(searchSubjectReq, searchSubjectRes, "GET");
            return searchSubjectRes.subjectResponse;
        }
        private string getRoleId(UserBean ubLoggedinUser, string contextId, string contextType, string roleName, string includeMembers)
        {
            GetRoleRequest getRoleReq = new GetRoleRequest(ubLoggedinUser, contextId, contextType, roleName, includeMembers);
            GetRoleResponse getRoleRes = new GetRoleResponse();
            SimpleHttpClient.execute(getRoleReq, getRoleRes, "GET");
            return getRoleRes.roleId;
        }

        private HttpStatusCode addRoleMembers(UserBean ubLoggedinUser, string userIds, string roleId)
        {
            AddRoleMembersRequest addRoleMembersReq = new AddRoleMembersRequest(ubLoggedinUser, userIds, roleId);
            AddRoleMembersResponse addRoleMembersRes = new AddRoleMembersResponse();
            SimpleHttpClient.execute(addRoleMembersReq, addRoleMembersRes, "PUT");
            return addRoleMembersRes.statusCode;
        }

        private List<UserBean> getRoleMembers(UserBean ubLoggedinUser, string roleId, string pageNumber, string numMembersPerPage)
        {
            GetRoleMembersRequest getRoleMembersReq = new GetRoleMembersRequest(ubLoggedinUser, roleId, pageNumber, numMembersPerPage);
            GetRoleMembersResponse getRoleMembersRes = new GetRoleMembersResponse();
            SimpleHttpClient.execute(getRoleMembersReq, getRoleMembersRes, "GET");
            return getRoleMembersRes.usersResponse;
        }

        private HttpStatusCode removeRoleMembers(UserBean ubLoggedinUser, string userIds, string roleId)
        {
            RemoveRoleMembersRequest removeRoleMembersReq = new RemoveRoleMembersRequest(ubLoggedinUser, userIds, roleId);
            RemoveRoleMembersResponse removeRoleMembersRes = new RemoveRoleMembersResponse();
            SimpleHttpClient.execute(removeRoleMembersReq, removeRoleMembersRes, "PUT");
            return removeRoleMembersRes.statusCode;
        }

        private HttpStatusCode unfollowSubject(UserBean ubLoggedinUser, UserBean usertoUnfollowSubject, SubjectBean subjecttobeUnfollowed)
        {
            UnfollowSubjectRequest unfollowSubjectReq = new UnfollowSubjectRequest(ubLoggedinUser, usertoUnfollowSubject, subjecttobeUnfollowed);
            UnfollowSubjectResponse unfollowSubjectRes = new UnfollowSubjectResponse();
            SimpleHttpClient.execute(unfollowSubjectReq, unfollowSubjectRes, "PUT");
            return unfollowSubjectRes.statusCode;
        }



        private void addSubjectOwners(UserBean ubLoggedinUser, string loginNames, SubjectBean sbSubjectToAddOwners, bool isCreate)
        {
            char[] delim = new char[] { ',' };
            if (loginNames.Contains(","))
            {
                loginNames = loginNames.TrimEnd(',');
                string[] strLoginNames = loginNames.Split(delim);
                string strUserIdsofOwners = "";
                List<string> lstUsersNotFound = new List<string>();
                Dictionary<string, string> dOwnersAdded = new Dictionary<string, string>();
                foreach (string logName in strLoginNames)
                {
                    if (logName != "" || logName != null)
                    {
                        UserBean ubOwner = fetchUser(ubLoggedinUser, logName.Trim(), "false", "false");
                        if (ubOwner.userId != null && ubOwner.userId != "")
                        {
                            strUserIdsofOwners += ubOwner.userId.Trim() + ",";
                            dOwnersAdded.Add(logName.Trim(), "false");
                        }
                        else
                        {
                            MessageBox.Show("User: " + logName.Trim() + " not found in the databse / is deleted / is inaccessible");
                            //Logger.WriteLog("\r\nUser: " + logName.Trim() + " not found in the databse / is deleted / is inaccessible\r\n");
                            lstUsersNotFound.Add(logName.Trim());
                        }
                    }
                }

                if (lstUsersNotFound.Count > 0)
                {
                    string strNotFound = "";
                    foreach (string str in lstUsersNotFound)
                    {
                        strNotFound += str + "\r\n";
                    }
                    if (strNotFound != "")
                    {
                        Logger.WriteLog("\r\nThe following " + lstUsersNotFound.Count + " users are not found in the database / are deleted / are inaccessible:\r\n" + strNotFound);
                    }
                }

                if (strUserIdsofOwners != null && strUserIdsofOwners != "")
                {
                    if (strUserIdsofOwners.Contains(","))
                    {
                        strUserIdsofOwners = strUserIdsofOwners.TrimEnd(',');
                    }
                    string strRoleId = getRoleId(ubLoggedinUser, sbSubjectToAddOwners.subjectId.Trim(), "Tibbr::Subject", "subject_owner", "false");

                    if (strRoleId != null && strRoleId != "")
                    {
                        HttpStatusCode httpSCOwnerAddition = addRoleMembers(ubLoggedinUser, strUserIdsofOwners, strRoleId);
                        if (httpSCOwnerAddition == HttpStatusCode.OK)
                        {
                            MessageBox.Show("Owner addition successful; checking if all the users in the input have been added....");
                            Logger.WriteLog("\r\nOwner addition successful; checking if all the users in the input have been added....\r\n");
                            //check whether users in lstUsersToBeAddedAsOwners are subject owners
                            List<UserBean> lstSubjectOwners = getRoleMembers(ubLoggedinUser, strRoleId, "1", strNumberofEntriesPerPage);
                            foreach (UserBean ub in lstSubjectOwners)
                            {
                                if (ub.loginName != null)
                                {
                                    if (dOwnersAdded.ContainsKey(ub.loginName.Trim()))
                                    {
                                        dOwnersAdded[ub.loginName.Trim()] = "true";
                                    }
                                }
                            }

                            if (!dOwnersAdded.ContainsValue("false"))
                            {
                                MessageBox.Show("All users in the input list added as owners");
                                Logger.WriteLog(dOwnersAdded.Count.ToString() + " user(s) in the input list added as owners\r\n");
                            }
                            else
                            {
                                string strNotAdded = "";
                                int nbOwnersNotAdded = 0;
                                foreach (KeyValuePair<string, string> kvp in dOwnersAdded)
                                {
                                    if (kvp.Value == "false")
                                    {
                                        MessageBox.Show(kvp.Key);
                                        strNotAdded += kvp.Key + "\r\n";
                                        nbOwnersNotAdded++;
                                    }
                                }
                                if (strNotAdded != "")
                                {
                                    MessageBox.Show("The following users were not added as owners");
                                    Logger.WriteLog("\r\nThe following " + nbOwnersNotAdded.ToString() + " user(s) were not added as owners:\r\n\r\n" + strNotAdded);
                                }

                            }

                            if (isCreate)
                            {
                                //if the loggedin user is not present in the processed input list i.e dOwnersAdded, remove him/her as owner and follower
                                if (!dOwnersAdded.ContainsKey(ubLoggedinUser.loginName.Trim()) && lstSubjectOwners.Count > 1)
                                {
                                    //remove as owner
                                    HttpStatusCode httpsOwnerRemoval = removeRoleMembers(ubLoggedinUser, ubLoggedinUser.userId, strRoleId);
                                    if (httpsOwnerRemoval == HttpStatusCode.OK)
                                    {
                                        MessageBox.Show("Logged-in user removed as an owner");
                                        Logger.WriteLog("Logged-in user removed as an owner\r\n");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Could not remove logged-in user as an owner");
                                        Logger.WriteLog("Could not remove logged-in user as an owner\r\n");
                                    }
                                    //remove as follower
                                    HttpStatusCode httpsUnfollowSubject = unfollowSubject(ubLoggedinUser, ubLoggedinUser, sbSubjectToAddOwners);
                                    if (httpsUnfollowSubject == HttpStatusCode.OK)
                                    {
                                        MessageBox.Show("Logged-in user removed as a subject follower");
                                        Logger.WriteLog("Logged-in user removed as a subject follower\r\n");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Could not remove logged-in user as a subject follower");
                                        Logger.WriteLog("Could not remove logged-in user as a subject follower\r\n");
                                    }
                                }
                            }


                        }
                        else
                        {
                            MessageBox.Show("Owner addition failed");
                            Logger.WriteLog("\r\nOwner addition failed\r\n");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Subject owner role not found");
                        MessageBox.Show("No owners added");
                        Logger.WriteLog("\r\nSubject owner role not found\r\nNo owners added\r\n");
                    }

                }
                else
                {
                    MessageBox.Show("No owner(s) to be added");
                    Logger.WriteLog("\r\nNo owner(s) to be added\r\n");
                }

            }

        }
        #endregion

        #region Utility Methods
        private OpenFileDialog browseImages(string imagePath)
        {
            OpenFileDialog browsePic = new OpenFileDialog();
            browsePic.Multiselect = false;
            browsePic.Title = "Browse Images";
            browsePic.InitialDirectory = imagePath;
            browsePic.CheckFileExists = true;
            browsePic.CheckPathExists = true;
            browsePic.Filter = "*JPEG files (*.jpg)|*.jpg";
            return browsePic;

        }

        private OpenFileDialog browseInputFile(string filePath)
        {
            OpenFileDialog browseIPFile = new OpenFileDialog();
            browseIPFile.Multiselect = false;
            browseIPFile.Title = "Browse Input File";
            browseIPFile.InitialDirectory = filePath;
            browseIPFile.CheckFileExists = true;
            browseIPFile.CheckPathExists = true;
            browseIPFile.Filter = "*CSV files (*.csv)|*.csv";
            return browseIPFile;
        }


        private void openNotepadWithErrorLog()
        {
            try
            {
                Process.Start("notepad.exe", Logger.logPath);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Unable to open the log file\n\nPlease navigate to: " + Application.StartupPath + @"\Log Files\" + " to find the log files");
            }

        }
        #endregion

        #region Events
        private void btnUserNameSingleFetch_Click(object sender, EventArgs e)
        {            
            btnUserSingleUndelete.Enabled = false;
            if(txtUserNameSingleFetch.Text == null || txtUserNameSingleFetch.Text == "")
            {
                MessageBox.Show("Please enter a username");
            }
            else
            {
                //call fetchUser()
                btnUserNameSingleFetch.Enabled = false;
                Logger.WriteLog("\r\nAttempting to fetch user: " + txtUserNameSingleFetch.Text.Trim() + "....\r\n");
                UserBean ubFetchedUser = fetchUser(ubLoggedinUser, txtUserNameSingleFetch.Text.Trim(), "true", "true");

                if (ubFetchedUser.userId != null)
                {
                    Logger.WriteLog("\r\nUser found\r\nFetched userID: " + ubFetchedUser.userId.Trim() + "\r\n");
                    gboxUserSingleFetch.Visible = true;
                    txtUserSingleUndeleteFetchFN.Text = ubFetchedUser.firstName;
                    txtUserSingleUndeleteFetchLN.Text = ubFetchedUser.lastName;
                    txtUserSingleUndeleteFetchEmail.Text = ubFetchedUser.email;
                    cbUserSingleUndeleteEnabled.Checked = bool.Parse(ubFetchedUser.isDeleted);
                    if (cbUserSingleUndeleteEnabled.Checked)
                        btnUserSingleUndelete.Enabled = true;
                    ubFetchedSingleUser = ubFetchedUser;
                }
                else
                {
                    gboxUserSingleFetch.Visible = false;
                    MessageBox.Show("User not found\r\n\r\nPlease enter the username and try again");
                    Logger.WriteLog("\r\nUser not found\r\n\r\nPlease enter the username and try again\r\n");
                }

                btnUserNameSingleFetch.Enabled = true;
            }
        }

        private void btnUserSingleUndelete_Click(object sender, EventArgs e)
        {
            //call the undelete user API
            /*
            UndeleteUserRequest undeleteUserReq = new UndeleteUserRequest(ubLoggedinUser, ubFetchedSingleUser);
            UndeleteUserResponse undeleteUserRes = new UndeleteUserResponse();
            SimpleHttpClient.execute(undeleteUserReq, undeleteUserRes, "PUT");
            */

            btnUserSingleUndelete.Enabled = false;
            Logger.WriteLog("\r\nAttempting to undelete user: " + ubFetchedSingleUser.loginName + "....\r\n");
            UserBean ubUndeletedUser = undeleteUser(ubLoggedinUser, ubFetchedSingleUser);

            if (ubUndeletedUser.userId != null)
            {
                if (ubUndeletedUser.isDeleted == "false")
                {
                    MessageBox.Show("Undelete successful");
                    btnUserSingleUndelete.Enabled = false;
                    Logger.WriteLog("\r\nUndelete successful\r\nUndeleted userID: " + ubUndeletedUser.userId.Trim() + "\r\n");
                }
                else
                {
                    MessageBox.Show("Undelete failed");
                    Logger.WriteLog("\r\nUndelete failed\r\n");
                }
            }
            else
            {
                MessageBox.Show("User not found\r\n\r\nUndelete failed");
                Logger.WriteLog("\r\nUndelete failed\r\n");
            }

            //btnUserSingleUndelete.Enabled = true;

        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            //call the create user API
            //create the dictionary entry
            

            if ((txtSingleCreateUserUN.Text == null || txtSingleCreateUserUN.Text == "") ||
                    (txtSingleCreateUserPassword.Text == null || txtSingleCreateUserPassword.Text == "") ||
                        (txtSingleCreateUserEmail.Text == null || txtSingleCreateUserEmail.Text == "") ||
                            (txtSingleCreateUserFN.Text == null || txtSingleCreateUserFN.Text == "") ||
                                (txtSingleCreateUserLN.Text == null || txtSingleCreateUserLN.Text == ""))
            {
                if (txtSingleCreateUserUN.Text == null || txtSingleCreateUserUN.Text == "")
                {
                    MessageBox.Show("Please enter a username");
                }
                if (txtSingleCreateUserPassword.Text == null || txtSingleCreateUserPassword.Text == "")
                {
                    MessageBox.Show("Please enter a password");
                }
                if (txtSingleCreateUserEmail.Text == null || txtSingleCreateUserEmail.Text == "")
                {
                    MessageBox.Show("Please enter an email address");
                }
                if (txtSingleCreateUserFN.Text == null || txtSingleCreateUserFN.Text == "")
                {
                    MessageBox.Show("Please enter a first name");
                }
                if (txtSingleCreateUserLN.Text == null || txtSingleCreateUserLN.Text == "")
                {
                    MessageBox.Show("Please enter a lastname");
                }
            }
            else
            {
                btnCreateUser.Enabled = false;
                Dictionary<string, string> de = new Dictionary<string, string>();
                if (txtSingleCreateUserUN.Text != null && txtSingleCreateUserUN.Text != "")
                {
                    de.Add("login", txtSingleCreateUserUN.Text.Trim());
                }
                if (txtSingleCreateUserPassword.Text != null || txtSingleCreateUserPassword.Text != "")
                {
                    de.Add("password", txtSingleCreateUserPassword.Text.Trim());
                    de.Add("password-confirmation", txtSingleCreateUserPassword.Text.Trim());
                }
                if (txtSingleCreateUserEmail.Text != null || txtSingleCreateUserEmail.Text != "")
                {
                    de.Add("email", txtSingleCreateUserEmail.Text.Trim());
                }
                if (txtSingleCreateUserFN.Text != null || txtSingleCreateUserFN.Text != "")
                {
                    de.Add("first-name", txtSingleCreateUserFN.Text.Trim());
                }
                if (txtSingleCreateUserLN.Text != null || txtSingleCreateUserLN.Text != "")
                {
                    de.Add("last-name", txtSingleCreateUserLN.Text.Trim());
                }

                /*
                CreateUserRequest createUserReq = new CreateUserRequest(ubLoggedinUser, de);
                CreateUserResponse createUserRes = new CreateUserResponse();
                SimpleHttpClient.execute(createUserReq, createUserRes, "POST");
                */

                Logger.WriteLog("\r\nAttempting to create user: " + de["login"] + "....\r\n");

                UserBean ubCreatedUser = createUser(ubLoggedinUser, de);

                
                if (ubCreatedUser.userId != null)
                {
                    MessageBox.Show("Create User successful");
                    Logger.WriteLog("\r\nCreate User successful\r\nCreated userID: " + ubCreatedUser.userId.Trim() + "\r\n");
                }
                else
                {
                    MessageBox.Show("Create User failed");
                    Logger.WriteLog("\r\nUser creation failed\r\n");
                }
                btnCreateUser.Enabled = true;

            }
        }


        private void btnUserNameSingleFetchUpdate_Click(object sender, EventArgs e)
        {
            if (txtUserNameSingleFetchUpdate.Text == null || txtUserNameSingleFetchUpdate.Text == "")
            {
                MessageBox.Show("Please enter a username");
            }
            else
            {
                //call fetchUser()
                btnUserNameSingleFetchUpdate.Enabled = false;
                Logger.WriteLog("\r\nAttempting to fetch user: " + txtUserNameSingleFetchUpdate.Text.Trim() + "....\r\n");
                UserBean ubFetchedUser = fetchUser(ubLoggedinUser, txtUserNameSingleFetchUpdate.Text.Trim(), "true", "true");

                if (ubFetchedUser.userId != null)
                {
                    Logger.WriteLog("\r\nUser found\r\nFetched userID: " + ubFetchedUser.userId.Trim() + "\r\n");
                    gboxUserSingleFetchUpdateEditable.Enabled = true;
                    gboxUserSingleFetchUpdateFixed.Visible = true;
                    gboxUserSingleFetchUpdateEditable.Visible = true;
                    gboxUserSingleFetchUpdateProfilePic.Visible = true;
                    txtUserSingleUpdateFetchFN.Text = ubFetchedUser.firstName;
                    txtUserSingleUpdateFetchLN.Text = ubFetchedUser.lastName;
                    txtUserSingleUpdateFetchEmail.Text = ubFetchedUser.email;
                    txtUserSingleUpdateFetchPhone.Text = ubFetchedUser.phone;
                    txtUserSingleUpdateFetchOPhone.Text = ubFetchedUser.officePhone;
                    txtUserSingleUpdateFetchMobile.Text = ubFetchedUser.mobile;
                    txtUserSingleUpdateFetchAddress.Text = ubFetchedUser.address;
                    txtUserSingleUpdateFetchAboutMe.Text = ubFetchedUser.aboutMe;
                    cbUserSingleUpdateEnabled.Checked = bool.Parse(ubFetchedUser.isDeleted);
                    if (cbUserSingleUpdateEnabled.Checked)
                    {
                        gboxUserSingleFetchUpdateEditable.Enabled = false;
                        gboxUserSingleFetchUpdateProfilePic.Enabled = false;
                    }

                    ubFetchedSingleUserUpdate = ubFetchedUser;
                }
                else
                {
                    gboxUserSingleFetchUpdateFixed.Visible = false;
                    gboxUserSingleFetchUpdateEditable.Visible = false;
                    gboxUserSingleFetchUpdateProfilePic.Visible = false;
                    MessageBox.Show("User not found\r\n\r\nPlease enter the username and try again");
                    Logger.WriteLog("\r\nUser not found\r\n\r\nPlease enter the username and try again\r\n");
                }

                btnUserNameSingleFetchUpdate.Enabled = true;
            }


        }

        private void btnUserSingleUpdate_Click(object sender, EventArgs e)
        {
            btnUserSingleUpdate.Enabled = false;

            Dictionary<string, string> dUpdate = new Dictionary<string, string>();

            if (txtUserSingleUpdateFetchPhone.Text != null)
            {
                dUpdate.Add("phone", txtUserSingleUpdateFetchPhone.Text);
            }
            if (txtUserSingleUpdateFetchOPhone.Text != null)
            {
                dUpdate.Add("office-number", txtUserSingleUpdateFetchOPhone.Text);
            }
            if (txtUserSingleUpdateFetchMobile.Text != null)
            {
                dUpdate.Add("mobile", txtUserSingleUpdateFetchMobile.Text);
            }
            if (txtUserSingleUpdateFetchAddress.Text != null)
            {
                dUpdate.Add("address", txtUserSingleUpdateFetchAddress.Text);
            }
            if (txtUserSingleUpdateFetchAboutMe.Text != null)
            {
                dUpdate.Add("description", txtUserSingleUpdateFetchAboutMe.Text);
            }

            if (dUpdate.Count > 0)
            {
                Logger.WriteLog("\r\nAttempting to update user: " + ubFetchedSingleUserUpdate.loginName + "....\r\n");
                UserBean ubUpdatedUser = updateUser(ubLoggedinUser, ubFetchedSingleUserUpdate, dUpdate);

                
                if (ubUpdatedUser.userId != null && ubUpdatedUser.userId.Trim() == ubFetchedSingleUserUpdate.userId.Trim())
                {
                    MessageBox.Show("Update successful");
                    Logger.WriteLog("\r\nUpdate successful\r\nUpdated userID: " + ubUpdatedUser.userId.Trim() + "\r\n");
                }
                else
                {
                    MessageBox.Show("Update failed");
                    Logger.WriteLog("\r\nUpdate failed\r\n");
                }
                
            }
            else
            {
                MessageBox.Show("No fields selected for update");
            }

            btnUserSingleUpdate.Enabled = true;

        }

        private void btnUserNameSingleBrowseUpdate_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseProfilePic = browseImages(txtUserSingleUpdateFetchProfilePic.Text.Trim());

            if (browseProfilePic.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtUserSingleUpdateFetchProfilePic.Text = browseProfilePic.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error opening file");
            }
        }

        private void btnUserSingleUpdateProfilePic_Click(object sender, EventArgs e)
        {
            btnUserSingleUpdateProfilePic.Enabled = false;
            Logger.WriteLog("\r\nAttempting to update profile pic for user: " + ubFetchedSingleUserUpdate.loginName + "....\r\n");
            UserResponse userImageRes = new UserResponse();
            MultipartContentHttpClient.execute(userImageRes, ubFetchedSingleUserUpdate.userId, "", txtUserSingleUpdateFetchProfilePic.Text, ubLoggedinUser, "user", "PUT");
            if (userImageRes.userResponse.userId != null)
            {
                MessageBox.Show("User ID: " + userImageRes.userResponse.userId);
                MessageBox.Show("Image Upload successful");
                Logger.WriteLog("\r\nImage upload successful\r\nUpdated userID: " + userImageRes.userResponse.userId.Trim() + "\r\n");
            }
            else
            {
                MessageBox.Show("Image Upload failed");
                Logger.WriteLog("\r\nImage Upload failed\r\n");
            }
            btnUserSingleUpdateProfilePic.Enabled = true;
        }

        private void btnSSCreate_Click(object sender, EventArgs e)
        {           
            if ((txtSSCreateSystemName.Text == null || txtSSCreateSystemName.Text == "") ||
                    (txtSSCreateDisplayName.Text == null || txtSSCreateDisplayName.Text == "") ||
                        (!rdoSSCreatePermissionPrivate.Checked && !rdoSSCreatePermissionProtected.Checked && !rdoSSCreatePermissionPublic.Checked))
            {
                if (txtSSCreateSystemName.Text == null || txtSSCreateSystemName.Text == "")
                {
                    MessageBox.Show("Please enter a system name");
                }
                //if (txtSSCreateDisplayName.Text == null || txtSSCreateDisplayName.Text == "")
                //{
                //    MessageBox.Show("Please enter a display name");
                //}
                if (!rdoSSCreatePermissionPrivate.Checked && !rdoSSCreatePermissionProtected.Checked && !rdoSSCreatePermissionPublic.Checked)
                {
                    MessageBox.Show("Please select the scope for the subject");
                }
            }
            else
            {
                btnSSCreate.Enabled = false;
                Dictionary<string, string> dCreate = new Dictionary<string, string>();
                string strDisplayNameBeforeEvent = txtSSCreateDisplayName.Text.Trim();
                txtSSCreateSystemName.Text = Regex.Replace(txtSSCreateSystemName.Text.Trim(), @"[\W-[.]]", "_");
                txtSSCreateDisplayName.Text = strDisplayNameBeforeEvent;

                if (txtSSCreateSystemName.Text != null && txtSSCreateSystemName.Text != "")
                {
                    dCreate.Add("name", txtSSCreateSystemName.Text.Trim());
                }
                if (txtSSCreateDisplayName.Text != null && txtSSCreateDisplayName.Text != "")
                {
                    dCreate.Add("render-name", txtSSCreateDisplayName.Text.Trim());
                }
                if (txtSSCreateDescription.Text != null && txtSSCreateDescription.Text != null)
                {
                    dCreate.Add("description", txtSSCreateDescription.Text.Trim());
                }

                if (cbSSCreateHierarchy.Checked)
                {
                    dCreate.Add("allow-children", "true");
                }
                else
                {
                    dCreate.Add("allow-children", "false");
                }

                if (rdoSSCreatePermissionPrivate.Checked)
                {
                    dCreate.Add("scope", "private");
                }
                else if (rdoSSCreatePermissionPublic.Checked)
                {
                    dCreate.Add("scope", "public");
                }
                else if (rdoSSCreatePermissionProtected.Checked)
                {
                    dCreate.Add("scope", "protected");
                }

                //create subject
                Logger.WriteLog("\r\nAttempting to create subject: " + dCreate["name"] + "....\r\n");
                SubjectBean sbCreatedSubject = createSubject(ubLoggedinUser, dCreate);
                if (sbCreatedSubject.subjectId != null)
                {
                    MessageBox.Show("Subject ID: " + sbCreatedSubject.subjectId);
                    MessageBox.Show("Subject Creation successful");
                    Logger.WriteLog("\r\nSubject creation successful\r\nCreated subjectID: " + sbCreatedSubject.subjectId.Trim() + "\r\n");

                    //if image path has been specified, update subject image
                    if (txtSSCreateImagePath.Text != null && txtSSCreateImagePath.Text != "")
                    {
                        Logger.WriteLog("\r\nAttempting to upload image for the subject: " + sbCreatedSubject.rendername + "\r\n");
                        SubjectResponse subjectImageRes = new SubjectResponse();
                        MultipartContentHttpClient.execute(subjectImageRes, sbCreatedSubject.subjectId, sbCreatedSubject.name, txtSSCreateImagePath.Text.Trim(), ubLoggedinUser, "subject", "PUT");
                        if (subjectImageRes.subjectResponse.subjectId != null)
                        {
                            MessageBox.Show("Subject ID: " + subjectImageRes.subjectResponse.subjectId);
                            MessageBox.Show("Subject image Upload successful");
                            Logger.WriteLog("\r\nSubject image upload successful\r\nUploaded image for subject: " + subjectImageRes.subjectResponse.subjectId + "\r\n");
                        }
                        else
                        {
                            MessageBox.Show("Subject image Upload failed");
                            Logger.WriteLog("\r\nSubject image upload failed\r\nPlease try again\r\n");
                        }
                    }
                    else
                    {
                        Logger.WriteLog("\r\nNo image specified to be uploaded to the subject: " + sbCreatedSubject.subjectId);
                    }

                    //if owner(s) are listed, add them as owner(s) to the subject
                    if (txtSSCreateOwners.Text != null && txtSSCreateOwners.Text != "")
                    {
                        string strOwnersToBeAdded = "";
                        List<string> lstOwnersInput = new List<string>();
                        if (txtSSCreateOwners.Text.Contains("\n"))
                        {
                            string[] strParts = Regex.Split(txtSSCreateOwners.Text.Trim(), "\n");
                            foreach (string strInput in strParts)
                            {
                                string strAfterTrim = strInput;
                                if (strInput.Contains("\r"))
                                    strAfterTrim = strInput.TrimEnd('\r');
                                lstOwnersInput.Add(strAfterTrim);
                            }
                        }
                        else
                        {
                            lstOwnersInput.Add(txtSSCreateOwners.Text.Trim());
                        }
                        foreach (string loginName in lstOwnersInput)
                        {
                            strOwnersToBeAdded += loginName + ",";
                        }
                        if (strOwnersToBeAdded != null && strOwnersToBeAdded != "")
                        {
                            Logger.WriteLog("\r\nAttempting to add subject owners....\r\n");
                            addSubjectOwners(ubLoggedinUser, strOwnersToBeAdded, sbCreatedSubject,true);
                        }

                    }
                    else
                    {
                        MessageBox.Show("\r\nNo owner(s) specified to be added to the subject: " + sbCreatedSubject.subjectId);
                        Logger.WriteLog("\r\nNo owner(s) specified to be added to the subject: " + sbCreatedSubject.subjectId);
                    }

                }
                else
                {
                    MessageBox.Show("Subject Creation failed");
                    Logger.WriteLog("\r\nSubject Creation failed\r\nPlease try again\r\n");
                }

                btnSSCreate.Enabled = true;

            }
        }

        private void txtSSCreateSystemName_TextChanged(object sender, EventArgs e)
        {
            txtSSCreateDisplayName.Text = txtSSCreateSystemName.Text.Trim();
        }

        private void btnSSCreateImageBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseSubjectPic = browseImages(txtSSCreateImagePath.Text.Trim());

            if (browseSubjectPic.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtSSCreateImagePath.Text = browseSubjectPic.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error opening file");
            }

        }

        private void btnUserSingleEditRole_Click(object sender, EventArgs e)
        {
            if (lbUserSingleEditRoleFetchTargetRole.SelectedItem != null)
            {
                btnUserSingleEditRole.Enabled = false;
                Logger.WriteLog("\r\nAttempting to change the role for the user: " + ubFetchedSingleUserEditRole.loginName + "....\r\n");
                string strRoleId = getRoleId(ubLoggedinUser, "", "", lbUserSingleEditRoleFetchTargetRole.SelectedItem.ToString(), "false");

                if (strRoleId != null && strRoleId != "")
                {
                    HttpStatusCode httpsSCEditRole = addRoleMembers(ubLoggedinUser, ubFetchedSingleUserEditRole.userId.Trim(), strRoleId);
                    if (httpsSCEditRole == HttpStatusCode.OK)
                    {
                        MessageBox.Show("Role change successful; validating....");
                        Logger.WriteLog("\r\nRole change successful, validating....\r\n");
                        List<UserBean> lstUsersinRole = getRoleMembers(ubLoggedinUser, strRoleId, "1", strNumberofEntriesPerPage);
                        bool isFound = false;
                        foreach (UserBean ub in lstUsersinRole)
                        {
                            if (ub.userId.Trim() == ubFetchedSingleUserEditRole.userId.Trim())
                            {
                                isFound = true;
                                break;
                            }
                        }
                        if (isFound)
                        {
                            MessageBox.Show("Validation successful");
                            Logger.WriteLog("Validation successful\r\n");
                        }
                        else
                        {
                            MessageBox.Show("Validation failed");
                            Logger.WriteLog("Validation failed\r\n");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Role could not be changed");
                        Logger.WriteLog("\r\nRole could not be changed\r\nPlease try again\r\n");
                    }
                }
                else
                {
                    Logger.WriteLog("\r\nSpecified role not found in the system\r\n");
                }
                btnUserSingleEditRole.Enabled = true;
            }

        }

        private void btnUserNameSingleFetchEditRole_Click(object sender, EventArgs e)
        {
            if (txtUserNameSingleFetchEditRole.Text == null || txtUserNameSingleFetchEditRole.Text == "")
            {
                MessageBox.Show("Please enter a username");
            }
            else
            {
                //call fetchUser()
                btnUserNameSingleFetchEditRole.Enabled = false;
                Logger.WriteLog("\r\nAttempting to fetch user: " + txtUserNameSingleFetchEditRole.Text.Trim() + "....\r\n");
                UserBean ubFetchedUser = fetchUser(ubLoggedinUser, txtUserNameSingleFetchEditRole.Text.Trim(), "false", "false");

                if (ubFetchedUser.userId != null)
                {
                    Logger.WriteLog("\r\nUser found\r\nFetched userID: " + ubFetchedUser.userId.Trim() + "\r\n");
                    gboxUserSingleFetchEditRole.Enabled = true;
                    gboxUserSingleFetchEditRole.Visible = true;
                    txtUserSingleEditRoleFetchFN.Text = ubFetchedUser.firstName;
                    txtUserSingleEditRoleFetchLN.Text = ubFetchedUser.lastName;
                    txtUserSingleEditRoleFetchEmail.Text = ubFetchedUser.email;
                    //txtUserSingleEditRoleFetchCurrentRole.Text = ubFetchedUser.phone;
                    ubFetchedSingleUserEditRole = ubFetchedUser;
                }
                else
                {
                    gboxUserSingleFetchEditRole.Visible = false;
                    MessageBox.Show("User not found\r\n\r\nPlease enter the username and try again");
                    Logger.WriteLog("\r\nUser not found\r\n\r\nPlease enter the username and try again\r\n");
                }
                btnUserNameSingleFetchEditRole.Enabled = true;
            }

            //Get fetched user's role


        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetSubjectFollowersRequest req = new GetSubjectFollowersRequest(ubLoggedinUser, "5835", "1", strNumberofEntriesPerPage);
            GetSubjectFollowersResponse res = new GetSubjectFollowersResponse();
            SimpleHttpClient.execute(req, res, "GET");
            if (res.usersResponse.Count > 0)
            {
                foreach (UserBean ub in res.usersResponse)
                {
                    StreamWriter swLog = new StreamWriter(@"FollowersLog.txt", true);
                    swLog.Write("\r\n" + ub.loginName.Trim() + ";" + ub.email.Trim());
                    swLog.Close();
                }
            }
            else
            {
                MessageBox.Show("Could not retrieve follower data");
            }

        }

        private void btnBSCreate_Click(object sender, EventArgs e)
        {
            if (txtBSCreateBrowseFile.Text == null || txtBSCreateBrowseFile.Text == "")
            {
                MessageBox.Show("Please select an input file");                
            }
            else
            {
                btnBSCreate.Enabled = false;
                Dictionary<string, string> dCreate = new Dictionary<string, string>();
                btnBSCreateLog.Enabled = false;
                int nbSuccess = 0;
                int nbError = 0;
                Logger.WriteLog("\r\n************************ Creating Subjects (BULK) ************************\r\n");
                //read from .csv file
                //Format
                //1. System Name
                //2. Display Name
                //3. Description
                //4. Scope
                //5. Hierarchy
                //6. Image
                //7. Owners            
                TextFieldParser tfParser = new TextFieldParser(txtBSCreateBrowseFile.Text.Trim());
                tfParser.TextFieldType = FieldType.Delimited;
                tfParser.SetDelimiters(",");
                int col = 0;
                int row = 0;
                string imagePath = "";
                string ownersList = "";

                #region while loop
                while (!tfParser.EndOfData)
                {
                    string[] fields = tfParser.ReadFields();
                    if (row > 0)
                    {                        
                        imagePath = "";
                        ownersList = "";
                        col = 0;
                        
                        dCreate["name"] = "";
                        dCreate["render-name"] = "";
                        dCreate["description"] = "";
                        dCreate["allow-children"] = "";
                        dCreate["scope"] = "";
                        foreach (string field in fields)
                        {
                            #region switch case
                            switch (col)
                            {
                                case (0):
                                    if (field != null)
                                        dCreate["name"] = field.Trim();
                                    break;

                                case (1):
                                    if (field != null)
                                        dCreate["render-name"] = field.Trim();
                                    break;

                                case (2):
                                    if (field != null)
                                        dCreate["description"] = field.Trim();
                                    break;

                                case (3):
                                    if (field != null)
                                        dCreate["scope"] = field.Trim();
                                    break;

                                case (4):
                                    if (field != null)
                                        dCreate["allow-children"] = field.Trim();
                                    break;

                                case (5):
                                    if (field != null)
                                        imagePath = field.Trim();
                                    break;

                                case (6):
                                    if (field != null)
                                        ownersList = field.Trim();
                                    break;
                            }
                            #endregion
                            col++;
                        }
                        //create subject
                        Logger.WriteLog("\r\n************ Subject " + row.ToString() + " ************\r\n");
                        Logger.WriteLog("\r\nAttempting to create subject: " + dCreate["name"] + "\r\n");
                        SubjectBean sbCreatedSubject = createSubject(ubLoggedinUser, dCreate);
                        if (sbCreatedSubject.subjectId != null)
                        {
                            Logger.WriteLog("Subject creation successful\r\nCreated subjectID: " + sbCreatedSubject.subjectId.Trim() + "\r\n");
                            nbSuccess++;
                            //update image if available
                            if (imagePath != null && imagePath != "")
                            {
                                Logger.WriteLog("\r\nAttempting to upload image for the subject: " + sbCreatedSubject.rendername + "\r\n");
                                SubjectResponse subjectImageRes = new SubjectResponse();
                                MultipartContentHttpClient.execute(subjectImageRes, sbCreatedSubject.subjectId, sbCreatedSubject.name, imagePath , ubLoggedinUser, "subject", "PUT");
                                if (subjectImageRes.subjectResponse.subjectId != null)
                                {
                                    MessageBox.Show("Subject ID: " + subjectImageRes.subjectResponse.subjectId);
                                    MessageBox.Show("Subject image Upload successful");
                                    Logger.WriteLog("\r\nSubject image upload successful\r\nUploaded image for subject: " + subjectImageRes.subjectResponse.subjectId + "\r\n");
                                }
                                else
                                {
                                    MessageBox.Show("Subject image Upload failed");
                                    Logger.WriteLog("\r\nSubject image upload failed\r\nPlease try again\r\n");
                                }
                            }
                            else
                            {
                                Logger.WriteLog("\r\nNo image specified to be uploaded to the subject: " + sbCreatedSubject.subjectId + "\r\n");
                            }

                            //update owners if available
                            if (ownersList != null)
                            {                                
                                ownersList = ownersList.Trim();
                                if (ownersList != "")
                                {
                                    Logger.WriteLog("\r\nAttempting to add subject owners....\r\n");
                                    if (ownersList[ownersList.Length - 1] != ',')
                                        ownersList += ",";
                                    addSubjectOwners(ubLoggedinUser, ownersList.Trim(), sbCreatedSubject, true);
                                }
                            }
                            else
                            {
                                MessageBox.Show("\r\nNo owner(s) specified to be added to the subject: " + sbCreatedSubject.subjectId);
                                Logger.WriteLog("\r\nNo owner(s) specified to be added to the subject: " + sbCreatedSubject.subjectId);
                            }
                            Logger.WriteLog("\r\n************************************\r\n");
                        }
                        else
                        {
                            Logger.WriteLog("Subject Creation failed for subject: " + dCreate["name"]);
                            Logger.WriteLog("\r\nPlease check the input data\r\n");
                            nbError++;
                            Logger.WriteLog("\r\n************************************\r\n");
                        }
                    }
                    row++;
                }
                #endregion

                tfParser.Close();
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                Logger.WriteLog("\r\nNumber of subjects created: " + nbSuccess.ToString() + "\r\n");
                Logger.WriteLog("Number of errors: " + nbError.ToString() + "\r\n");
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                btnBSCreateLog.Enabled = true;
                btnBSCreate.Enabled = true;

                Logger.WriteLog("\r\n**************************************************************************\r\n");
            }
        }

        private void btnBSCreateBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = browseInputFile(txtBSCreateBrowseFile.Text.Trim());
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtBSCreateBrowseFile.Text = browseFile.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error opening file");
            }

        }

        private void btnBSCreateLog_Click(object sender, EventArgs e)
        {
            openNotepadWithErrorLog();
        }

        private void btnBUCreateBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = browseInputFile(txtBUCreateBrowseFile.Text.Trim());
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtBUCreateBrowseFile.Text = browseFile.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error opening file");
            }

        }

        private void btnBUCreateLog_Click(object sender, EventArgs e)
        {
            openNotepadWithErrorLog();
        }

        private void btnBUCreate_Click(object sender, EventArgs e)
        {
            if (txtBUCreateBrowseFile.Text == null || txtBUCreateBrowseFile.Text == "")
            {
                MessageBox.Show("Please select an input file");
            }
            else
            {
                btnBUCreate.Enabled = false;
                Dictionary<string, string> dCreate = new Dictionary<string, string>();
                btnBUCreateLog.Enabled = false;
                int nbSuccess = 0;
                int nbError = 0;
                Logger.WriteLog("\r\n************************ Creating Users (BULK) ************************\r\n");
                //read from .csv file
                //Format
                //1. Username
                //2. Password
                //3. Email
                //4. First Name
                //5. Last Name
                //6. Image
                TextFieldParser tfParser = new TextFieldParser(txtBUCreateBrowseFile.Text.Trim());
                tfParser.TextFieldType = FieldType.Delimited;
                tfParser.SetDelimiters(",");
                int col = 0;
                int row = 0;
                string imagePath = "";

                #region while loop
                while (!tfParser.EndOfData)
                {
                    string[] fields = tfParser.ReadFields();
                    if (row > 0)
                    {
                        imagePath = "";
                        col = 0;

                        dCreate["login"] = "";
                        dCreate["password"] = "";
                        dCreate["password-confirmation"] = "";
                        dCreate["email"] = "";
                        dCreate["first-name"] = "";
                        dCreate["last-name"] = "";
                        foreach (string field in fields)
                        {
                            #region switch case
                            switch (col)
                            {
                                case (0):
                                    if (field != null)
                                        dCreate["login"] = field;
                                    break;

                                case (1):
                                    if (field != null)
                                    {
                                        dCreate["password"] = field;
                                        dCreate["password-confirmation"] = field;
                                    }
                                    break;

                                case (2):
                                    if (field != null)
                                        dCreate["email"] = field;
                                    break;

                                case (3):
                                    if (field != null)
                                        dCreate["first-name"] = field;
                                    break;

                                case (4):
                                    if (field != null)
                                        dCreate["last-name"] = field;
                                    break;

                                case (5):
                                    if (field != null)
                                        imagePath = field.Trim();
                                    break;

                            }
                            #endregion
                            col++;
                        }
                        //create subject
                        Logger.WriteLog("\r\n************ User " + row.ToString() + " ************\r\n");
                        Logger.WriteLog("\r\nAttempting to create user: " + dCreate["login"] + "\r\n");
                        UserBean ubCreatedUser = createUser(ubLoggedinUser, dCreate);
                        if (ubCreatedUser.userId != null)
                        {
                            Logger.WriteLog("User creation successful\r\nCreated userID: " + ubCreatedUser.userId.Trim() + "\r\n");
                            nbSuccess++;
                            //update image if available
                            if (imagePath != null && imagePath != "")
                            {
                                Logger.WriteLog("\r\nAttempting to upload image for the user: " + ubCreatedUser.loginName + "\r\n");
                                UserResponse userImageRes = new UserResponse();
                                MultipartContentHttpClient.execute(userImageRes, ubCreatedUser.userId, "", imagePath, ubLoggedinUser, "user", "PUT");
                                if (userImageRes.userResponse.userId != null)
                                {
                                    MessageBox.Show("User ID: " + userImageRes.userResponse.userId);
                                    MessageBox.Show("User image Upload successful");
                                    Logger.WriteLog("\r\nUser image upload successful\r\nUploaded image for user: " + userImageRes.userResponse.userId + "\r\n");
                                }
                                else
                                {
                                    MessageBox.Show("User image Upload failed");
                                    Logger.WriteLog("\r\nUser image upload failed\r\nPlease try again\r\n");
                                }
                            }
                            else
                            {
                                Logger.WriteLog("\r\nNo image specified to be uploaded to the user: " + ubCreatedUser.userId + "\r\n");
                            }

                            Logger.WriteLog("\r\n************************************\r\n");
                        }
                        else
                        {
                            Logger.WriteLog("User Creation failed for user: " + dCreate["login"]);
                            Logger.WriteLog("\r\nPlease check the input data\r\n");
                            nbError++;
                            Logger.WriteLog("\r\n************************************\r\n");
                        }
                    }
                    row++;
                }
                #endregion

                tfParser.Close();
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                Logger.WriteLog("\r\nNumber of successes: " + nbSuccess.ToString() + "\r\n");
                Logger.WriteLog("Number of errors: " + nbError.ToString() + "\r\n");
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                btnBUCreateLog.Enabled = true;
                btnBUCreate.Enabled = true;

                Logger.WriteLog("\r\n**************************************************************************\r\n");


            }
        }

        private void btnBUUndelete_Click(object sender, EventArgs e)
        {
            if (txtBUUndeleteBrowseFile.Text == null || txtBUUndeleteBrowseFile.Text == "")
            {
                MessageBox.Show("Please select a file");
            }
            else
            {
                /*
                btnBUUndelete.Enabled = false;
                btnBUUndeleteLog.Enabled = false;
                Logger.WriteLog("\r\n************************ Undeleting Users (BULK) ************************\r\n");
                //read from .csv file
                //Format
                //1. Username
                //Fetch all users and list those not in database
                //Logger.WriteLog("\r\n************************ Fetching Users (BULK) ************************\r\n");
                TextFieldParser tfParser = new TextFieldParser(txtBSCreateBrowseFile.Text.Trim());
                tfParser.TextFieldType = FieldType.Delimited;
                    tfParser.SetDelimiters(",");
                int col = 0;
                int row = 0;
                string strUserName = "";
                List<string> lstUsersNotFound = new List<string>();
                List<string> lstUsersAlreadyUndeleted = new List<string>();
                List<string> lstUsersToBeUndeleted = new List<string>();
                List<string> lstUndeleted = new List<string>();
                List<string> lstNotUndeleted = new List<string>();

                #region while loop
                while (!tfParser.EndOfData)
                {
                    string[] fields = tfParser.ReadFields();
                    if (row > 0)
                    {
                        col = 0;
                        strUserName = "";
                        foreach (string field in fields)
                        {
                            #region switch case
                            switch (col)
                            {
                                case (0):
                                    if (field != null)
                                        strUserName = field;
                                    break;

                            }
                            #endregion
                            col++;
                        }
                        //fetch user
                        Logger.WriteLog("\r\n************ User " + row.ToString() + " ************\r\n");
                        Logger.WriteLog("\r\nAttempting to fetch user: " + strUserName + "\r\n");
                        UserBean ubFetchedUser = fetchUser(ubLoggedinUser, strUserName, "true", "true");
                        if (ubFetchedUser.userId != null)
                        {
                            Logger.WriteLog("\r\nUser fetched; fetched userID: " + ubFetchedUser.userId + "\r\n");
                            if (ubFetchedUser.isDeleted == "true")
                                lstUsersToBeUndeleted.Add(ubFetchedUser.loginName);
                            else if (ubFetchedUser.isDeleted == "false")
                                lstUsersAlreadyUndeleted.Add(ubFetchedUser.loginName);    
                            
                        }
                        else
                        {
                            Logger.WriteLog("\r\nUser not fetched; userID: " + strUserName + "\r\n");
                            lstUsersNotFound.Add(strUserName);
                        }
                        Logger.WriteLog("\r\n*************************************\r\n");

                    }
                    row++;
                }

                //Logger.WriteLog("\r\n***********************************************************************\r\n");

                foreach (string logName in lstUsersToBeUndeleted)
                {
                    Logger.WriteLog("\r\nAttempting to undelete user: " + logName + "\r\n");
                    //undelete user
                    UserBean ubUndeletedUser = undeleteUser(ubLoggedinUser, logName);
                    if (ubUndeletedUser.userId != null)
                    {
                        if (ubUndeletedUser.isDeleted == "true")
                        {
                            Logger.WriteLog("\r\nUndelete failed; userID: " + ubUndeletedUser.userId + "\r\n");
                            lstNotUndeleted.Add(logName);
                        }
                        else if (ubUndeletedUser.isDeleted == "false")
                        {
                            Logger.WriteLog("\r\nUndelete successful; undeleted userID: " + ubUndeletedUser.userId + "\r\n");
                            lstUndeleted.Add(logName);
                        }
                    }
                    else
                    {
                        Logger.WriteLog("\r\nUndelete failed; userID: " + ubUndeletedUser.userId + "\r\n");
                        lstError.Add(logName);                        
                    }
                }


                #endregion


                tfParser.Close();
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");

                Logger.WriteLog("\r\nNumber of users not found: " + "\r\n");
                foreach (string usernf in lstUsersNotFound)
                {
                    Logger.WriteLog("\r\n\t" + usernf);
                }

                Logger.WriteLog("\r\nNumber of users already undeleted: " + "\r\n");
                foreach (string userund in lstUsersAlreadyUndeleted)
                {
                    Logger.WriteLog("\r\n\t" + userund);
                }

                Logger.WriteLog("\r\nNumber of users to be undeleted: " + lstUsersToBeUndeleted.Count.ToString() + "\r\n");
                foreach (string tbu in lstUsersToBeUndeleted)
                {
                    Logger.WriteLog("\r\n\t" + tbu);
                }

                Logger.WriteLog("\r\n\r\nNumber of successes: " + nbSuccess.ToString() + "\r\n");
                foreach (string und in lstUndeleted)
                {
                    Logger.WriteLog("\r\n\t" + und);
                }

                Logger.WriteLog("\r\n\r\nNumber of errors: " + nbError.ToString() + "\r\n");
                foreach (string underr in lstNotUndeleted)
                {
                    Logger.WriteLog("\r\n\t" + underr);
                }
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");

                btnBUUndeleteLog.Enabled = true;
                btnBUUndelete.Enabled = true;
                Logger.WriteLog("\r\n**************************************************************************\r\n");
                */
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtBUUndeleteBrowseFile.Text == null || txtBUUndeleteBrowseFile.Text == "")
            {
                MessageBox.Show("Please select a file");
            }
            else
            {

            }
        }

        private void btnBSUnfollowBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = browseInputFile(txtBSUnfollowBrowseFile.Text.Trim());
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                txtBSUnfollowBrowseFile.Text = browseFile.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error opening file");
            }

        }

        private void btnBSUnfollowLog_Click(object sender, EventArgs e)
        {
            openNotepadWithErrorLog();
        }

        private void btnBSUnfollow_Click(object sender, EventArgs e)
        {
            if (txtBSUnfollowBrowseFile.Text == null || txtBSUnfollowBrowseFile.Text == "")
            {
                MessageBox.Show("Please select an input file");
            }
            else
            {
                btnBSUnfollow.Enabled = false;
                btnBSUnfollowLog.Enabled = false;
                int nbSuccess = 0;
                int nbError = 0;
                int nbSuccessUserUnsubscribe = 0;
                int nbErrorUserUnsubscribe = 0;
                Logger.WriteLog("\r\n************************ Unfollow Users from Subjects (BULK) ************************\r\n");
                //read from .csv file
                //Format
                //1. Subject System Name
                //2. User Login
                TextFieldParser tfParser = new TextFieldParser(txtBSUnfollowBrowseFile.Text.Trim());
                tfParser.TextFieldType = FieldType.Delimited;
                tfParser.SetDelimiters(",");
                int col = 0;
                int row = 0;
                string strSubjectSystemName = "";
                string strUserLogin = "";
                

                //API login
                //get subject id from subject system name
                //get user id from loginname
                //unsubscribe user from subject

                #region while loop
                while (!tfParser.EndOfData)
                {
                    string[] fields = tfParser.ReadFields();
                    if (row > 0)
                    {
                        strSubjectSystemName = "";
                        strUserLogin = "";
                        col = 0;
                        nbSuccessUserUnsubscribe = 0;
                        nbErrorUserUnsubscribe = 0;

                        foreach (string field in fields)
                        {
                            #region switch case
                            switch (col)
                            {
                                case (0):
                                    if (field != null)
                                        strSubjectSystemName = field.Trim();
                                    break;

                                case (1):
                                    if (field != null)
                                        strUserLogin = field.Trim();
                                    break;

                            }
                            #endregion
                            col++;
                        }

                        if (strSubjectSystemName != null && strSubjectSystemName != "")
                        {
                            if (strUserLogin != null && strUserLogin != "")
                            {
                                //get subject id from subject system name
                                Logger.WriteLog("\r\n************ Subject " + row.ToString() + " ************\r\n");
                                Logger.WriteLog("\r\nAttempting to find subject: " + strSubjectSystemName.Trim() + "\r\n");
                                SubjectBean sbFetchedSubject = fetchSubject(ubLoggedinUser, strSubjectSystemName, "true", "true");
                                if (sbFetchedSubject.subjectId != null)
                                {
                                    Logger.WriteLog("Subject search successful\r\nFetched subjectID: " + sbFetchedSubject.subjectId.Trim() + "\r\n");
                                    nbSuccess++;
                                    //split list of userlogins by delimiter(comma)
                                    List<string> lstUnfollowUsers = new List<string>();
                                    List<string> lstUnfollowUsersSuccessful = new List<string>();
                                    List<string> lstUsersNotFound = new List<string>();
                                    if (strUserLogin[strUserLogin.Length - 1] == ',')
                                    {
                                        strUserLogin = strUserLogin.TrimEnd(',');
                                    }
                                    if (strUserLogin.Contains(","))
                                    {
                                        string[] strParts = Regex.Split(strUserLogin, ",");
                                        foreach (string strInput in strParts)
                                        {
                                            string strAfterTrim = strInput.Trim();
                                            if (strInput.Contains("\r"))
                                                strAfterTrim = strInput.TrimEnd('\r');
                                            lstUnfollowUsers.Add(strAfterTrim);
                                        }
                                    }
                                    else
                                    {
                                        lstUnfollowUsers.Add(strUserLogin.Trim());
                                    }

                                    //for every userloginname in lstUnfollowUsers, find the userID
                                    foreach (string strUserLoginName in lstUnfollowUsers)
                                    {
                                        Logger.WriteLog("\r\n####################################\r\n");
                                        Logger.WriteLog("\r\nAttempting to find user: " + strUserLoginName + "\r\n");
                                        UserBean ubFetchedUser = fetchUser(ubLoggedinUser, strUserLoginName, "true", "true");
                                        if (ubFetchedUser.userId != null)
                                        {
                                            Logger.WriteLog("User search successful\r\nFetched userID: " + ubFetchedUser.userId.Trim() + "\r\n");
                                            Logger.WriteLog("Attempting to unsubscribe user: " + strUserLoginName + " from subject: " + strSubjectSystemName + "\r\n");
                                            HttpStatusCode httpSCode = unfollowSubject(ubLoggedinUser, ubFetchedUser, sbFetchedSubject);
                                            if (httpSCode == HttpStatusCode.OK)
                                            {
                                                Logger.WriteLog("User: " + strUserLoginName + " successfully unsubscribed from the subject: " + strSubjectSystemName + "\r\n");
                                                lstUnfollowUsersSuccessful.Add(strUserLoginName);
                                                nbSuccessUserUnsubscribe++;
                                            }
                                            else
                                            {
                                                Logger.WriteLog("User: " + strUserLoginName + " not unsubscribed from the subject: " + strSubjectSystemName + "\r\n");
                                                nbErrorUserUnsubscribe++;
                                            }

                                        }
                                        else
                                        {
                                            Logger.WriteLog("\r\nUser: " + strUserLoginName + " not found\r\n");
                                            lstUsersNotFound.Add(strUserLoginName);
                                            nbErrorUserUnsubscribe++;
                                        }
                                        Logger.WriteLog("\r\n####################################\r\n");
                                    }

                                    Logger.WriteLog("\r\nTotal number of users to be unsubscribed: " + lstUnfollowUsers.Count.ToString() + "\r\n");
                                    Logger.WriteLog("Number of users not found: " + lstUsersNotFound.Count.ToString() + "\r\n");
                                    Logger.WriteLog("Users not found:\r\n");
                                    foreach (string unotfound in lstUsersNotFound)
                                    {
                                        Logger.WriteLog(unotfound + "\r\n");
                                    }
                                    Logger.WriteLog("Number of users unsubscribed: " + nbSuccessUserUnsubscribe.ToString() + "\r\n");
                                    Logger.WriteLog("Users unsubscribed:\r\n");
                                    foreach (string uname in lstUnfollowUsersSuccessful)
                                    {
                                        Logger.WriteLog(uname + "\r\n");
                                    }
                                    Logger.WriteLog("Number of errors: " + nbErrorUserUnsubscribe.ToString() + "\r\n");

                                }
                                else
                                {
                                    Logger.WriteLog("\r\nSubject: " + strSubjectSystemName + " not found\r\n");
                                    nbError++;
                                    Logger.WriteLog("\r\n************************************\r\n");
                                }
                            }
                            else
                            {
                                Logger.WriteLog("\r\nRow " + row.ToString() + ": List of users to unsubscribe from subject not found in input file\r\n");
                            }
                        }
                        else
                        {
                            Logger.WriteLog("\r\nRow " + row.ToString() + ": Subject Name not found in input file\r\n");
                        }
                        Logger.WriteLog("\r\n************************************\r\n");
                    }
                    row++;
                }
                #endregion

                tfParser.Close();
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                Logger.WriteLog("\r\nNumber of subjects processed: " + nbSuccess.ToString() + "\r\n");
                Logger.WriteLog("Number of errors: " + nbError.ToString() + "\r\n");
                Logger.WriteLog("\r\n------------------------------------------------------------------\r\n");
                btnBSUnfollowLog.Enabled = true;
                btnBSUnfollow.Enabled = true;
                MessageBox.Show("Done");

                Logger.WriteLog("\r\n**************************************************************************\r\n");
            }

        }
        #endregion

        #endregion

    }
}
