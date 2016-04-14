using System;
using System.Collections.Generic;
using B24.Common;
using B24.Common.Web;
using B24.Common.IGE;
using SkillSoft.IGE.IGEClient;
using B24.Common.Logs;
using System.Configuration;
using System.Web.UI;

namespace B24.Sales4.UserControl
{
    public partial class IngeniousSandbox : System.Web.UI.UserControl
    {

        #region private members
        private Logger logger = new Logger(Logger.LoggerType.Sales4);
        private bool isB24 = true; // to identify if sub is B24 sub or not
        private Sales4.UI.BasePage basePage;
        #endregion

        #region public Property
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool SandBoxCreateButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Set the values on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as Sales4.UI.BasePage;
            if (basePage == null)
            {
                return;
            }
            if (SubscriptionId == null || SubscriptionId == Guid.Empty)
            {
                return;
            }
            if (!IsPostBack)
            {
                Subscription subscription = GetSubscription(SubscriptionId);
                SandBoxCreateButton.Visible = SandBoxCreateButtonView;
          //      InitializeIgeProperty();
          //      SandboxPropertyUserControl.BindData();
                //Already the subscription has a sandbox id mapped to it
                if (!subscription.SandBoxID.Equals(Guid.Empty))
                {
                    MessageLabel.Visible = true;
                    //Get SandBox by sandboxid
                    SandBox sandbox = GetSandBox(subscription.SandBoxID);
                    MessageLabel.Text += " and Sandbox Name is " + sandbox.SandboxName;
                    return;
                }
                else
                {
                    //Find SandBox by PasswordRoot                
                    List<KeyValuePair<string, string>> argsList = new List<KeyValuePair<string, string>>(1);
                    argsList.Add(new KeyValuePair<string, string>("SubId", subscription.PasswordRoot));
                    List<SandBox> sandBoxByPasswordRoot = FindSandBox(argsList);

                    if (sandBoxByPasswordRoot.Count > 0)//SandBox found using paswordroot
                    {
                        SetSanboxInHidden(sandBoxByPasswordRoot[0]);

                        MessageLabel.Visible = false;
                        ConfirmByPasswordRootRow.Visible = true;
                        return;

                    }

                    //Find SandBox by CompanyName // no need for search by CompanyName any more
                    //argsList = new List<KeyValuePair<string, string>>(1);
                    //argsList.Add(new KeyValuePair<string, string>("Company", subscription.CompanyName));
                    //List<SandBox> sandBoxByCompanyName = FindSandBox(argsList);

                    //if (sandBoxByCompanyName.Count > 0)//SandBox found using company
                    //{
                    //    SetSanboxInHidden(sandBoxByCompanyName[0]);

                    //    MessageLabel.Visible = false;
                    //    ConfirmByCompanyRow.Visible = true;
                    //    return;

                    //}

                    // Nothing worked out? now time to create new sandbox
                    MessageLabel.Visible = false;
                    NewSandboxRow.Visible = true;
                    return;
                }
            }
            else
            {
             //   InitializeIgeProperty();
            }
        }

        /// <summary>
        /// If the user conforming the sandbox based on the company name then 
        /// * update the sandbox id in books db.
        /// * Update the password root in IG 
        /// * Update the user's ig id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConfirmByCompanyButton_Click(object sender, EventArgs e)
        {
            SandBox sandbox = GetSanboxInHidden();

            //Update the sandboxid to the Subscription
            UpdateSanboxIdInSubcscription(sandbox.Id.Value);

            //If the sandbox was found via the company name update the sandbox to include the subscription password root.
            if (String.IsNullOrEmpty(sandbox.Subscription) || String.Empty.Equals(sandbox.Subscription.Trim() ))
            {
                SandBoxClient client = new SandBoxClient(sandbox.Id.Value, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
                SandBox updateSandbox = new SandBox();
                Subscription subscription = GetSubscription(SubscriptionId);
                updateSandbox.Subscription = subscription.PasswordRoot;
                updateSandbox.CreatorId = basePage.GetSandboxRequesterId(isB24);
                client.PutSandbox(updateSandbox);
            }
            UpdateAdminSandboxInLibUser(sandbox.Id.Value, SubscriptionId);

            MessageLabel.Visible = true;
            ConfirmByCompanyRow.Visible = false;
         //   InitializeIgeProperty();
        //    SandboxPropertyUserControl.BindData();
        }

        /// <summary>
        /// If the sandbox confirmed through Password root then update the users between IG and the Books DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConfirmByPasswordButton_Click(object sender, EventArgs e)
        {
            SandBox sandbox = GetSanboxInHidden();

            //update the sandbox Id in the the Subscription
            UpdateSanboxIdInSubcscription(sandbox.Id.Value);

            UpdateAdminSandboxInLibUser(sandbox.Id.Value, SubscriptionId);

            MessageLabel.Visible = true;
            ConfirmByPasswordRootRow.Visible = false;
        //    InitializeIgeProperty();
       //     SandboxPropertyUserControl.BindData();
        }

        /// <summary>
        /// Create new sandbox using the subscription Company name and password root
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SandBoxCreateButton_Click(object sender, EventArgs e)
        {                        
            Subscription subscription = GetSubscription(SubscriptionId);

            //Create the sandbox with the correct company name and passwordroot.
            SandBox newSandBox = new SandBox();
            newSandBox.SandboxName = subscription.CompanyName + "_" + subscription.PasswordRoot;  //according to TT1883
            newSandBox.Subscription = subscription.PasswordRoot;
            newSandBox.CreatorId = basePage.GetSandboxRequesterId(isB24);

            try
            {
                SandBoxClient sandBoxClient = new SandBoxClient(basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
                newSandBox = sandBoxClient.AddSandbox(newSandBox);
            }
            catch (IGECustomException igeException)
            {
                logger.Log(Logger.LogLevel.Error, "Create new Sandbox failed", igeException);
                return;
            }
            catch (Exception exception)
            {
                logger.Log(Logger.LogLevel.Error, "Create new Sandbox failed", exception);
                return;
            }

            // update the subscription and its users
            UpdateSubscriptionForSandbox(newSandBox, (Guid)newSandBox.Id);

            MessageLabel.Visible = true;
            MessageLabel.Text += " and Sandbox Name is " + newSandBox.SandboxName;
            NewSandboxRow.Visible = false;
        //    InitializeIgeProperty();
       //     SandboxPropertyUserControl.BindData();

        }

        /// <summary>
        /// Lookup sandboxes based on password root
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FindSandBoxButton_Click(Object sender, EventArgs e)
        {
            //Find SandBox by PasswordRoot                
            List<KeyValuePair<string, string>> argsList = new List<KeyValuePair<string, string>>(1);
            argsList.Add(new KeyValuePair<string, string>("SubId", FindSandBoxTextBox.Text.Trim()));
            
            List<SandBox> sandBoxByPasswordRoot = FindSandBox(argsList);
            
            if (sandBoxByPasswordRoot.Count > 0)
            {
                List<KeyValuePair<Guid, string>> sandboxList = new List<KeyValuePair<Guid, string>>(sandBoxByPasswordRoot.Count);

                foreach (B24.Common.IGE.SandBox sandbox in sandBoxByPasswordRoot)
                {
                    sandboxList.Add(new KeyValuePair<Guid, string>((Guid)sandbox.Id, sandbox.SandboxName));
                }
                SandBoxRadioList.DataSource = sandboxList;
                SandBoxRadioList.DataTextField = "value";
                SandBoxRadioList.DataValueField = "key";
                SandBoxRadioList.DataBind();

                // select the first radio button by default
                if (sandboxList.Count > 0)
                {
                    SandBoxRadioList.SelectedIndex = 0;

                }

                SandBoxNotFoundLabel.Visible = false;
                ConfirmSandBoxRow.Visible = true;
            }
            else
            {
                ConfirmSandBoxRow.Visible = false;
                SandBoxNotFoundLabel.Visible = true;
                SandBoxNotFoundLabel.Text = "SandBox Not Found";
                return;
            }
        }

        /// <summary>
        /// Update the subscription id against the selected sandbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateToSandBox_Click(Object sender, EventArgs e)
        {
            Guid sandboxId = new Guid(SandBoxRadioList.SelectedValue.ToString());
            SandBoxClient sandboxClient = new SandBoxClient(sandboxId, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
            B24.Common.IGE.SandBox sandbox = sandboxClient.GetSandBox();

            SandboxMemberClient client = new SandboxMemberClient(sandboxId, Guid.Empty, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
            B24.Common.IGE.SandboxMember addSandboxMember = new B24.Common.IGE.SandboxMember();
            addSandboxMember.CompanyAlias = sandbox.SandboxName;
            if (!String.IsNullOrEmpty(sandbox.Subscription))
            {
                addSandboxMember.Subscription = sandbox.Subscription;
            }
            client.AddSandboxMember(addSandboxMember);

            // update the subscription and its users            
            UpdateSubscriptionForSandbox(sandbox, sandboxId);

            MessageLabel.Visible = true;
            NewSandboxRow.Visible = false;
        //    InitializeIgeProperty();
       //     SandboxPropertyUserControl.BindData();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set the values in hidden control
        /// </summary>
        /// <param name="sandbox"></param>
        private void SetSanboxInHidden(SandBox sandbox)
        {
            SandboxIdHidden.Value = sandbox.Id.ToString();
            SandboxPasswordRootHidden.Value = sandbox.Subscription;
            SandboxCompanyNameHidden.Value = sandbox.SandboxName;
        }

        /// <summary>
        /// Get the values from hidden control
        /// </summary>
        /// <returns></returns>
        private SandBox GetSanboxInHidden()
        {
            SandBox sandbox = new SandBox();
            sandbox.Id = new Guid(SandboxIdHidden.Value);
            sandbox.Subscription = SandboxPasswordRootHidden.Value;
            sandbox.SandboxName = SandboxCompanyNameHidden.Value;

            return sandbox;
        }

        /// <summary>
        /// If the sandbox created without any problem then sync the users
        /// Create an account with admin and moderator roles in IGE sandbox for each bookdb admin.
        /// Update libuser ingenuserid field for each admin.
        /// </summary>
        /// <param name="sandbox"></param>
        private void UpdateSubscriptionForSandbox(B24.Common.IGE.SandBox sandbox, Guid sandboxId)
        {
            // update the sandbox id in the subscription
            UpdateSanboxIdInSubcscription(sandboxId);

            // update the users ingenious id in current subscription
            B24.Common.UserFactory userFactory = new B24.Common.UserFactory(basePage.UserConnStr);
            B24.Common.User[] libraryUsers = userFactory.GetListBySubID(SubscriptionId);
            
            foreach (B24.Common.User libraryUser in libraryUsers)
            {
                if (userFactory.IsUserAdmin(SubscriptionId, libraryUser.UserID))
                {
                    try
                    {
                        B24.Common.IGE.User user = new B24.Common.IGE.User();
                        user.UserName = libraryUser.Login;
                        user.QualifiedUserName = libraryUser.Login;
                        user.Role = "Admin";

                        UserClient client = new UserClient(sandboxId, libraryUser.UserID, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);                    
                        B24.Common.IGE.User newUser = client.AddUser(user);

                        // update the ingenious id for this library user
                        userFactory.UpdateIngenUser(libraryUser, newUser.UserId);

                    }
                    catch (IGECustomException igeException)
                    {
                        logger.Log(Logger.LogLevel.Error, "CouldNotAdd Users", igeException);
                    }
                    catch (Exception exception)
                    {
                        logger.Log(Logger.LogLevel.Error, "AddAdminLibusertoSandBox Failed", exception);
                    }
                }

            }          
        }

        /// <summary>
        ///Add the sandboxid to the subscription record.
        /// </summary>
        /// <param name="sandboxId"></param>
        /// <param name="subId"></param>
        private void UpdateSanboxIdInSubcscription(Guid sandboxId)
        {
            B24.Common.SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
            Subscription subscription = GetSubscription(SubscriptionId);

            subscriptionFactory.PutSubscriptionSandboxID(subscription, sandboxId);
        }

        /// <summary>
        /// Update the Libuser with Ingenious user Id for Admin users in sandbox
        /// </summary>
        /// <param name="sandboxId"></param>
        /// <param name="subscriptionId"></param>
        private void UpdateAdminSandboxInLibUser(Guid sandboxId, Guid subscriptionId)
        {
            try
            {
                //Identify the admin users in the sandbox and ensure they exist in the Books subscription and have their ige userid stored in their libuser record.
                B24.Common.UserFactory userFactory = new B24.Common.UserFactory(basePage.UserConnStr);
                B24.Common.User[] booksUsers = userFactory.GetListBySubID(subscriptionId);
                int booksUserCount = booksUsers.Length;

                // We dont have direct API to access users under a sandbox. Only work around for now is search the profile and get the user list.
                // Then use User service to pull out the user information from IG
                int totalCount = 0;
                ProfileClient profileClient = new ProfileClient(sandboxId, (Guid)basePage.User.IngeniousUserId, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
                List<KeyValuePair<string, string>> argList = new List<KeyValuePair<string, string>>();
                argList.Add(new KeyValuePair<string, string>("lookup", "lastname=^[a-z]"));
                argList.Add(new KeyValuePair<string, string>("bin", "1"));
                argList.Add(new KeyValuePair<string, string>("binsize", "4000"));
                profileClient.RequestArgs = argList;

                IList<Profile> findProfiles = profileClient.FindProfiles(out totalCount);
                foreach (B24.Common.IGE.Profile profile in findProfiles)
                {
                    UserClient userClient = new UserClient(sandboxId, (Guid)profile.Id, basePage.GetSandboxRequesterId(isB24), basePage.IGEServiceBaseUri);
                    B24.Common.IGE.User igUser = userClient.GetUser();
                    if (igUser.IsUserAdmin())
                    {
                        for (int i = 0; i < booksUserCount; i++)
                        {
                            if (booksUsers[i].Login.ToLower() == igUser.UserName.ToLower())
                            {
                                userFactory.UpdateIngenUser(booksUsers[i], igUser.UserId);
                                break;
                            }
                        }
                    }
                }                        
            }
            catch (IGECustomException igeException)
            {
                logger.Log(Logger.LogLevel.Error, "UpdateAdminSandboxInLibUser failed", igeException);                
            }
        }
        /// <summary>
        /// get the subscription object by subscriptionId
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        private Subscription GetSubscription(Guid subscriptionId)
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
            return subscriptionFactory.GetSubscriptionByID(subscriptionId);
        }

        /// <summary>
        /// Find the sandbox
        /// </summary>
        /// <param name="companyName"></param>
        private List<SandBox> FindSandBox(List<KeyValuePair<string, string>> argsList)
        {
            SandBoxClient sandboxClient = new SandBoxClient(basePage.SandboxRequesterId, basePage.IGEServiceBaseUri);            
            sandboxClient.RequestArgs = argsList;

            List<SandBox> sandboxList = new List<SandBox>();
            try
            {
                sandboxList = sandboxClient.FindSandboxes();
            }
            catch (IGECustomException igeEx)
            {
                if (igeEx.IGEHttpStatus != System.Net.HttpStatusCode.NotFound)
                {
                    logger.Log(Logger.LogLevel.Error, "Search SandboxByCompany", igeEx);
                    // Not found
                }
            }
            return sandboxList;
        }
        /// <summary>
        /// Find the sandbox
        /// </summary>
        /// <param name="companyName"></param>
        private SandBox GetSandBox(Guid sandboxId)
        {
            SandBoxClient sandboxClient = new SandBoxClient(sandboxId,basePage.SandboxRequesterId, basePage.IGEServiceBaseUri);

            SandBox sandbox = new SandBox();
            try
            {
                sandbox = sandboxClient.GetSandBox();
            }
            catch (IGECustomException igeEx)
            {
                if (igeEx.IGEHttpStatus != System.Net.HttpStatusCode.NotFound)
                {
                    logger.Log(Logger.LogLevel.Error, "Get Sandbox By sandboxid", igeEx);
                }
            }
            return sandbox;
        }

        //private void InitializeIgeProperty()
        //{
        //    Subscription manageSubscription = GetSubscription(SubscriptionId);
        //    if (manageSubscription != null && manageSubscription.SandBoxID != null && manageSubscription.SandBoxID != Guid.Empty)
        //    {
        //        SandboxPropertyUserControl.Visible = true;
        //        bool isB24 = true;
        //        if (manageSubscription.ApplicationName.ToLower() == "skillport")
        //        {
        //            isB24 = false;
        //        }
        //        SandboxPropertyUserControl.SubID = SubscriptionId;
        //        SandboxPropertyUserControl.SandboxID = manageSubscription.SandBoxID;
        //        // skillsoft and b24 sub will use different requesterid to create and modify sandbox
        //        SandboxPropertyUserControl.RequestorID = basePage.GetSandboxRequesterId(isB24);
        //    }
        //    else
        //    {
        //        SandboxPropertyUserControl.Visible = false;
        //    }
        //}
        #endregion
    }
}