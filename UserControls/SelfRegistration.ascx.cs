using System;
using System.Collections.ObjectModel;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;


namespace B24.Sales4.UserControl
{
    /// <summary>
    /// To Enable self registration and add or Remove domain rule
    /// </summary>
    public partial class SelfRegistration : System.Web.UI.UserControl
    {
        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.Sales3);
        MasterDataFactory masterDataFactory;
        BasePage baseObject;
        #endregion Private Members

        #region Private Variables
        /// <summary>
        /// To store access Level
        /// </summary>
        private int level;
        /// <summary>
        /// to ckeck condition to showemaildomain
        /// </summary>
        private bool showEmailDomain;
        /// <summary>
        /// salesgroup check
        /// </summary>
        private bool allowSelfReg;
        /// <summary>
        /// coprsubscription value
        /// </summary>
        private bool CheckCorpSubscription;
        /// <summary>
        /// for displaing errormessage lable
        /// </summary>
        private bool updateSuccess;
        /// <summary>
        /// to keep linkURL
        /// </summary>
        private string LinkString;
        #endregion

        #region Public Property
        /// <summary>
        ///  To get and set Subscription
        /// </summary>
        public Subscription Subscription { get; set; }
        /// <summary>
        /// To get and set userid 
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// To get and set the login of the user
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        #endregion Public Property

        #region Protected Methods
        /// <summary>
        /// Page Load   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            baseObject = this.Page as BasePage;
            if (!Page.IsPostBack)
            {
                InitializeControls();
            }
            Multiview.ActiveViewIndex = 1;
        }
        /// <summary>
        /// Update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrationRuleFactory registartionRuleFactory = new RegistrationRuleFactory(baseObject.UserConnStr);

                // Add New Domainrule 
                if (NewDomainRuleTextBox.Text != String.Empty)
                {
                    RegistrationRule registrationRule = new RegistrationRule();
                    registrationRule.EmailMask = "%" + NewDomainRuleTextBox.Text;
                    registrationRule.SubscriptionID = Subscription.SubscriptionID;
                    registartionRuleFactory.PutRule(registrationRule);
                    NewDomainRuleTextBox.Text = "";
                    updateSuccess = true;
                }

                //Delete the selected Domainrule                
                if (DomainRuleIdCheckBoxList.Items.Count > 0)
                {
                    List<ListItem> toBeDeletedRule = new List<ListItem>();
                    foreach (ListItem listItem in DomainRuleIdCheckBoxList.Items)
                    {
                        if (listItem.Selected == true)
                        {                           
                            Guid ruleId = new Guid(listItem.Value);
                            toBeDeletedRule.Add(listItem);
                            registartionRuleFactory.DeleteRule(ruleId);                            
                            updateSuccess = true;
                        }
                    }
                    foreach (ListItem listItem in toBeDeletedRule)
                    {
                        DomainRuleIdCheckBoxList.Items.Remove(listItem); 
                    }
                }
                
                if (DomainRuleIdCheckBoxList.Items.Count == 0)
                {
                    DomainRuleIdCheckBoxList.Visible = false;
                    DomainRuleIdReadCheckBoxList.Visible = false;
                    RemoveDomainTitleReadLabel.Visible = false;
                    RemoveDomainReadLabel.Visible = false;
                    RemoveDomains2ReadLabel.Visible = false;
                    RemoveDomains2Label.Visible = false;
                    RemoveDomainTitleLabel.Visible = false;
                    RemoveDomainsLabel.Visible = false;
                }

                //Enable Sef Registration
                // Verify that we have some rules for this sub before proceeding
                RegistrationRuleFactory registrationRuleFactory = new RegistrationRuleFactory(baseObject.UserConnStr);
                List<RegistrationRule> ruleList = registrationRuleFactory.GetRules(Subscription.SubscriptionID);
                if (ruleList.Count > 0)
                {
                    //Now go ahead and apply the corpsub status
                    if (EnableSelfRegistrationCheckBox.Checked == true)
                    {
                        Subscription.CorpSubscription = Subscription.SubscriptionID;
                    }
                    else
                    {
                        Subscription.CorpSubscription = Guid.Empty;
                    }
                    SubscriptionFactory subscriptionfactory = new SubscriptionFactory(baseObject.UserConnStr);
                    subscriptionfactory.PutSubscription(Subscription, UserId, Login);
                    updateSuccess = true;
                }
                else
                {
                    SelfRegistrationErrorMessageLabel.Text = Resources.Resource.SelfRegistrationCannotEnable;
                }
                if (ruleList.Count == 0)
                {
                    Subscription.CorpSubscription = Guid.Empty;
                    SubscriptionFactory subscriptionfactory = new SubscriptionFactory(baseObject.UserConnStr);
                    subscriptionfactory.PutSubscription(Subscription, UserId, Login);
                }
            }

            catch (NullReferenceException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                SelfRegistrationErrorMessageLabel.Text = Resources.Resource.Error;
            }
            if (updateSuccess == true)
            {
                SelfRegistrationErrorMessageLabel.Text = Resources.Resource.SelfRegistrationChanged;
            }
            InitializeControls();
            Multiview.ActiveViewIndex = 1;
        }
        /// <summary>
        /// Edit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {

            SelfRegistrationErrorMessageLabel.Text = String.Empty;
            Multiview.ActiveViewIndex = 0;
        }
        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            SelfRegistrationErrorMessageLabel.Text = String.Empty;
            Multiview.ActiveViewIndex = 1;
        }
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initialize page controls
        /// </summary>
        private void InitializeControls()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
            masterDataFactory = new MasterDataFactory(baseObject.UserConnStr);
            CalculateLevel();
            ShowEnableSelfRegForm();
            EditButton.Visible = EditButtonView;
        }
        /// <summary>
        /// Calculate the Level
        /// </summary>
        private void CalculateLevel()
        {

            PermissionFactory permFactory = new PermissionFactory(baseObject.UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(UserId, baseObject.User.Identity.Name, String.Empty);
            Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(UserId);
            showEmailDomain = SearchForString(salesGroupUsers, "MSVL2");
            allowSelfReg = !SearchForString(salesGroupUsers, "MSVL");
            if (allowSelfReg || permissions.GeneralAdmin == 1 || permissions.SuperUser == 1)
            {
                level = 2;         // enable form and Update Button ctrl          
            }
            else
            {
                level = 0;
            }
        }

        /// <summary>
        /// Search for a string in collection
        /// </summary>
        /// <param name="salesGroupUsers"></param>
        /// <returns></returns>
        private bool SearchForString(Collection<MasterData> salesGroupUsers, string searchString)
        {
            bool returnVar = true;
            if (salesGroupUsers.Count > 0)
            {
                for (int loopCounter = 0; loopCounter < salesGroupUsers.Count; loopCounter++)
                {
                    MasterData salesGroup = salesGroupUsers[loopCounter];
                    if (salesGroup.ResellerCode.ToUpper() == searchString)
                    {
                        returnVar = false;
                        break;
                    }
                }
            }
            return returnVar;
        }
        /// <summary>
        /// Method to Enable the form
        /// </summary>
        private void ShowEnableSelfRegForm()
        {
            if (level > 0)
            {
                CheckCorpSubscription = (Subscription.CorpSubscription != Guid.Empty) ? true : false;
                EnableSelfRegistrationTitleLabel.Visible = true;
                EnableSelfRegistrationLabel.Visible = true;
                EnableSelfRegistrationCheckBox.Visible = true;
                //read label
                EnableSelfRegistrationReadTitleLabel.Visible = true;
                EnableSelfRegistrationReadLabel.Visible = true;
                EnableSelfRegistrationReadCheckBox.Visible = true;


                if (CheckCorpSubscription == true)
                {
                    EnableSelfRegistrationCheckBox.Checked = true;
                    EnableSelfRegistrationReadCheckBox.Checked = true;
                }
                else
                {
                    EnableSelfRegistrationCheckBox.Checked = false;
                    EnableSelfRegistrationReadCheckBox.Checked = false;
                }

                if (CheckCorpSubscription)
                {
                    ApplicationFactory applicationFactory = new ApplicationFactory(baseObject.UserConnStr);
                    Application application = applicationFactory.GetApplication(Subscription.ApplicationName);
                    LinkString = application.BaseUrl + ((Subscription.ApplicationName == "AdminBriefing") ? "/register.aspx" : "gatekeeper.asp") + "?site=" + Subscription.PasswordRoot;

                    LinkURLTitleLabel.Visible = true;
                    LinkUrlLabel.Visible = true;
                    LinkUrlLabel.Text = LinkString;
                    //read Label
                    LinkURLTitleReadLabel.Visible = true;
                    LinkURLReadLabel.Visible = true;
                    LinkURLReadLabel.Text = LinkString;
                }
                else
                {
                    LinkURLTitleLabel.Visible = false;
                    LinkUrlLabel.Visible = false;
                    //read Label
                    LinkURLTitleReadLabel.Visible = false;
                    LinkURLReadLabel.Visible = false;
                }

                if (showEmailDomain)
                {
                    RegistrationRuleFactory registrationRuleFactory = new RegistrationRuleFactory(baseObject.UserConnStr);
                    List<RegistrationRule> ruleList = registrationRuleFactory.GetRules(Subscription.SubscriptionID);
                    List<RegistrationRule> newRuleList = new List<RegistrationRule>();
                    if (ruleList.Count > 0)
                    {
                        try
                        {
                            bool hasDomainRule = false;
                            foreach (RegistrationRule registrationRules in ruleList)
                            {
                                try
                                {
                                    string emailMask = (registrationRules.EmailMask).Replace("%", ""); // strip off the wild cards.  we manage this for them in Process() below
                                    if (emailMask != String.Empty)
                                    {
                                        hasDomainRule = true;
                                        break;
                                    }
                                }
                                catch (NullReferenceException ex)
                                {
                                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                                    SelfRegistrationErrorMessageLabel.Text = Resources.Resource.Error;
                                }
                            }
                            foreach (RegistrationRule registrationRules in ruleList)
                            {
                                try
                                {
                                    if (registrationRules.EmailMask != String.Empty)
                                    {
                                        registrationRules.EmailMask = (registrationRules.EmailMask).Replace("%", "");
                                        newRuleList.Add(registrationRules);
                                    }
                                }
                                catch (NullReferenceException ex)
                                {
                                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                                    SelfRegistrationErrorMessageLabel.Text = Resources.Resource.Error;
                                }
                            }

                            DomainRuleIdCheckBoxList.DataSource = newRuleList;
                            DomainRuleIdCheckBoxList.DataTextField = "EmailMask";
                            DomainRuleIdCheckBoxList.DataValueField = "RuleID";
                            DomainRuleIdCheckBoxList.DataBind();
                            DomainRuleIdCheckBoxList.Visible = true;
                            // read only domainrule checkboxlist
                            DomainRuleIdReadCheckBoxList.DataSource = newRuleList;
                            DomainRuleIdReadCheckBoxList.DataTextField = "EmailMask";
                            DomainRuleIdReadCheckBoxList.DataValueField = "RuleID";
                            DomainRuleIdReadCheckBoxList.DataBind();
                            DomainRuleIdReadCheckBoxList.Visible = true;

                            if (hasDomainRule)
                            {
                                RemoveDomainTitleLabel.Visible = true;
                                RemoveDomainsLabel.Visible = true;
                                //read Label
                                RemoveDomainTitleReadLabel.Visible = true;
                                RemoveDomainReadLabel.Visible = true;
                            }
                            else
                            {
                                RemoveDomainTitleLabel.Visible = true;
                                RemoveDomains2Label.Visible = true;
                                //read label
                                RemoveDomainTitleReadLabel.Visible = true;
                                RemoveDomains2ReadLabel.Visible = true;
                            }
                        }

                        catch (NullReferenceException ex)
                        {
                            logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                            SelfRegistrationErrorMessageLabel.Text = Resources.Resource.Error;
                        }
                    }
                    AddDomainTitleLabel.Visible = true;
                    AddDomainLabel.Visible = true;
                    NewDomainLabel.Visible = true;
                    NewDomainRuleTextBox.Visible = true;
                    HelpTextLabel.Visible = true;
                    //read label
                    AddDomainTitleReadLabel.Visible = true;
                    AddDomainReadLabel.Visible = true;
                    NewDomainReadLabel.Visible = true;
                    NewDomainReadTextBox.Visible = true;
                    HelpTextReadLabel.Visible = true;
                }
            }
            if (this.level > 1)
            {
                UpdateButton.Visible = true;
            }
        }
        #endregion Private Methods
    }
}
