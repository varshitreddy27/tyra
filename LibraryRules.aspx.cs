using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using B24.Common;
using B24.Common.Logs;

namespace B24.Sales3.UI
{
    public partial class LibraryRules : BasePage
    {
        // The sub  to load
        private Guid subid;
        // Factory for getting / setting auth rules       
        private AuthenticationRuleFactory ruleFactory;         
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);

        //Flag to make Visibility of the Update button for Email Domain Section
        private bool enableEmailUpdate=false;       

        protected void Page_Load(object sender, EventArgs e)
        {
            //// Verify credentials
            //if (!(IsSupport || SalesGroupDict.ContainsKey("b24lib")))
            //{
            //    throw new Exception("You do not have permission to edit Authentication Rules");
            //}

            // Initialize private members
            ruleFactory = new AuthenticationRuleFactory(UserConnStr);
            subid = GetSubID();

            // Event handlers
            RulesGridView.RowEditing += new GridViewEditEventHandler(RulesGridView_RowEditing);
            RulesGridView.RowDeleting += new GridViewDeleteEventHandler(RulesGridView_RowDeleting);
            RulesGridView.RowCancelingEdit += new GridViewCancelEditEventHandler(RulesGridView_RowCancelingEdit);
            RulesGridView.RowUpdating += new GridViewUpdateEventHandler(RulesGridView_RowUpdating);
            RulesGridView.RowCreated += new GridViewRowEventHandler(RulesGridView_RowCreated);
            AddButton.Command += new CommandEventHandler(AddButton_Command);

            // Load the sub and verify library app
            Subscription sub = LoadSub();
            if (sub != null)
            {
                if (sub.ApplicationName.ToLower() != "library")
                {
                    // Cannot edit auth rules for a non-library subscription
                    throw new InvalidOperationException(Resources.Resource.LibraryRulesAuthError);
                }
                // Databinding
                if (!IsPostBack)
                {
                    // Load the sub report
                    LoadSubReport(sub);

                    // Load the rules for this sub
                    RulesDataBind();
                    //Load the domain rules for this subscription
                    DomainRuleDataBind();
                    //Get the Default shared secret code for the user
                    if (!String.IsNullOrEmpty(GetSharedSecretByID(GetSubID())))
                    {
                        SharedSecretCode.Text = GetSharedSecretByID(GetSubID());
                    }
                    else
                    {
                        SharedSecretCode.Visible = false;

                    }
                    if (enableEmailUpdate == true)
                    {
                        UpdateEmailDomainDiv.Visible = true; 
                    }
                }
            }
        }

        #region Private Methods
        /// <summary>
        /// Load the sub matching subid
        /// </summary>
        /// <returns>The loaded subscription</returns>
        private Subscription LoadSub()
        {
            Subscription sub = null;

            try
            {
                SubscriptionFactory subFactory = new SubscriptionFactory(UserConnStr);
                sub = subFactory.GetSubscriptionByID(subid);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubscriptonNotLoaded, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SubscriptonNotLoaded));
            }

            return sub;
        }
        private  bool AllowAllEmailDomainRule()
        {
            bool allowAllEmailDomain = false;
            List<SubscriptionFlag> flaglist = null;
            try {
                SubscriptionFlagFactory flagFactory = new SubscriptionFlagFactory(UserConnStr);
                flaglist = flagFactory.GetActiveSubscriptionFlags(subid);
                SubscriptionFlags flagHelper = new SubscriptionFlags();
                flagHelper.Flags = flaglist;
                allowAllEmailDomain = flagHelper.IsFlagSet(SubscriptionFlagValues.LibraryAllowAnyEmailDomains);

            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubscriptionFlagNotLoaded, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SubscriptionFlagNotLoaded));
            }
            return allowAllEmailDomain;
        }
        /// <summary>
        /// 40 is the magic number for libaryallowanyemaildomains
        /// </summary>
        /// <param name="setActive"></param>
        private void UpdateEmailDomainRule(bool setActive)
        {
            List<SubscriptionFlag> flaglist = null;
            try
            {
                SubscriptionFlagFactory flagFactory = new SubscriptionFlagFactory(UserConnStr);
                flaglist = flagFactory.GetActiveSubscriptionFlags(subid);
                SubscriptionFlags flagHelper = new SubscriptionFlags();
                flagHelper.Flags = flaglist;
                string flagIDList = flagHelper.GetFlagIDList();
                if (setActive)
                    flagIDList = flagIDList + "40";
                else
                {
                    int index = flagIDList.IndexOf("40"); // find the flagid in list for libaryallowanyemaildomains
                    if (index >= 0)
                    {
                        string front = flagIDList.Remove(index, "40,".Length);
                    }
                }
                flagFactory.SaveAllSubscriptionFlags(subid, flagIDList);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubFlagFail, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SubFlagFail));
            }
        }
        /// <summary>
        /// Get the Shared Secret Code for the library subscription
        /// </summary>
        /// <param name="userid">logged in user ID</param>
        /// <returns></returns>
        private string GetSharedSecretByID(Guid subscriptionId)
        {
            Subscription subscription = null;
            try
            {
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                // Load the subscription details of the subscription id.
                subscription = subscriptionFactory.GetSubscriptionByID(subscriptionId);
                return subscription.SharedSecret;
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubscriptonNotLoaded, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SubscriptonNotLoaded));
                return String.Empty;
            }

        }
        /// <summary>
        /// Updates to individual rows as they're created
        /// </summary>
        private void RulesGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView rulesGridView = sender as GridView;                          // The rules gridview itself
            GridViewRow row = e.Row;                                              // The row being created
            int editIndex = rulesGridView.EditIndex;                              // Index of the row currently being edited

            if (row.RowType == DataControlRowType.DataRow)
            {
                // Show the relevant buttons depending upon whether the row is edit
                ImageButton updateBT = row.FindControl("UpdateButton") as ImageButton;
                ImageButton cancelBT = row.FindControl("CancelButton") as ImageButton;
                ImageButton editBT = row.FindControl("EditButton") as ImageButton;
                ImageButton deleteBT = row.FindControl("DeleteButton") as ImageButton;
                if (editIndex == row.DataItemIndex)
                {
                    updateBT.Visible = true;
                    cancelBT.Visible = true;
                    editBT.Visible = false;
                    deleteBT.Visible = false;
                }
                else
                {
                    updateBT.Visible = false;
                    cancelBT.Visible = false;
                    editBT.Visible = true;
                    deleteBT.Visible = true;
                }
            }
        }

        /// <summary>
        /// Handle the row update event
        /// </summary>
        private void RulesGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView rulesGridView = sender as GridView;                                      // Gridview throwing the event
            Guid ruleID = new Guid(rulesGridView.DataKeys[e.RowIndex].Value.ToString());      // The unique id of rule being updated
            GridViewRow row = rulesGridView.Rows[e.RowIndex];                                 // The row being updated

            // Get the controls
            TextBox ipAddrTB = row.FindControl("IPAddrMaskTextBox") as TextBox;
            TextBox urlTB = row.FindControl("URLMaskTextBox") as TextBox;
            CheckBox disallowCB = row.FindControl("DisallowCheckBox") as CheckBox;
            CheckBox noAnonCB = row.FindControl("NoAnonymousCheckBox") as CheckBox;
            CheckBox activeCB = row.FindControl("ActiveCheckBox") as CheckBox;
            TextBox priorityTB = row.FindControl("PriorityTextBox") as TextBox;
            TextBox commentTB = row.FindControl("CommentTextBox") as TextBox;

            // Update the rule
            try
            {
                AuthenticationRule rule = new AuthenticationRule();
                rule.SubscriptionID = subid;
                rule.RuleID = ruleID;
                rule.IPAddrMask = ipAddrTB.Text;
                rule.UrlMask = urlTB.Text;
                rule.Disallow = disallowCB.Checked;
                rule.NoAnonymous = noAnonCB.Checked;
                rule.Active = activeCB.Checked;
                rule.Priority = Convert.ToInt32(priorityTB.Text);
                rule.Comment = commentTB.Text;
                ruleFactory.PutRule(rule);
                
                State.Add("errmsg",Resources.Resource.RuleUpdated);
                B24Redirect(ThisPage);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorUpdatingRule, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.ErrorUpdatingRule));
            }
        }

        /// <summary>
        /// Bind the sub's rules to the rules grid view
        /// </summary>
        private void RulesDataBind()
        {
            //declare two variables which decide whether to enable or disable 
            //the EmailDomain and SharedSecret panels
            bool EnableSharedSecret = false;
            bool EnableDomain = false;
            try
            {
                List<AuthenticationRule> authenticationRule = ruleFactory.GetRules(subid);
                foreach (AuthenticationRule item in authenticationRule)
                {

                    if (item.UrlMask != null)
                    {
                        if (item.UrlMask.Equals("ezproxy://"))
                        {
                            EnableSharedSecret = true;
                        }
                    }
                    if ((item.IPAddrMask != null || (item.UrlMask != null && !item.UrlMask.Equals("ezproxy://"))) && item.NoAnonymous == true)
                    {
                        EnableDomain = true;
                    }
                }
                if (EnableSharedSecret)
                {
                    SharedSecretPanel.Visible = true;
                }
                if (EnableDomain)
                {
                    AddDomainPanel.Visible = true;
                    enableEmailUpdate = true;
                }
                //Bind the Gridview 
                RulesGridView.DataSource = authenticationRule;
                RulesGridView.DataBind();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorGettingRules, ex);
                
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.ErrorGettingRules));
            }

        }

        /// <summary>
        /// Handle Cancel
        /// </summary>
        private void RulesGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Nothing to really do here; redirect to clear the ViewState
            B24Redirect(ThisPage);
        }

        /// <summary>
        /// Bind the Email domain rules to the checkbox control
        /// </summary>
        private void DomainRuleDataBind()
        {

            Guid SubscriptionID = subid;
            RegistrationRuleFactory registrationRuleFactory = new RegistrationRuleFactory(UserConnStr);
            List<RegistrationRule> cList = registrationRuleFactory.GetRules(SubscriptionID);
            if (cList.Count == 0)
            {  //If there is no EmailDomain. Make the remove email domain section invisible.
                RemoveDomainPanel.Visible = false;
            }
            else
            {
                RemoveDomainPanel.Visible = true;
               enableEmailUpdate = true;
            }
            EmailDomainCollection.DataSource = cList;
            EmailDomainCollection.DataTextField = "emailMask";
            EmailDomainCollection.DataValueField = "ruleID";
            EmailDomainCollection.DataBind();
            if (AllowAllEmailDomainRule())
            {
                CB_Email.Checked = true;
            }
            else
            {
                CB_Email.Checked = false;
            }

        }

        /// <summary>
        /// Handle the row deleting event
        /// </summary>
        private void RulesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView rulesGridView = (GridView)sender as GridView;
            Guid ruleID = new Guid(rulesGridView.DataKeys[e.RowIndex].Value.ToString());

            try
            {
                ruleFactory.DeleteRule(ruleID);
                
                State.Add("errmsg", Resources.Resource.RuleDeleted);
                B24Redirect(ThisPage);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorDeletingRule, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.ErrorDeletingRule));
            }
        }


        /// <summary>
        /// Handle the row editing event
        /// </summary>
        private void RulesGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RulesGridView.EditIndex = e.NewEditIndex;
            RulesDataBind();
        }

        /// <summary>
        /// Get the sub id from the postback or from the url
        /// </summary>
        /// <returns>A subid guid</returns>
        private Guid GetSubID()
        {
            Guid subid;                                       // The subid to return
            if (null != Request.QueryString["subid"] && !String.IsNullOrEmpty(Request.QueryString["subid"]))
                {
                    subid = new Guid( Request.QueryString["subid"]);
                }
            else if (Request.Form["subid"] != null)
                subid = new Guid(Request.Form["subid"]);
            else if (State["authRulesSubID"] != null)
                subid = new Guid(State["authRulesSubID"]);
             else throw new Exception("No SubID to load");

            // Keep the sub id around for post backs...
            State.Add("authRulesSubID", subid.ToString());

            return subid;
        }

        /// <summary>
        /// Show the sub report at the top of the page
        /// </summary>
        /// <param name="sub">The subscription object to use</param>
        private void LoadSubReport(Subscription sub)
        {
            AccountCodeLabel.Text = sub.PasswordRoot;
            CompanyNameLabel.Text = String.Format("{0} / {1}", sub.CompanyName, sub.Department);
            StartsLabel.Text = sub.Starts.ToString("MMM dd, yyyy");
            ExpiresLabel.Text = sub.Expires.ToString("MMM dd, yyyy");
            SeatsLabel.Text = sub.Seats.ToString();
            AccountInfoHyperLink.NavigateUrl = String.Format("report.asp?arg={{{0}}}&report=1", sub.SubscriptionID);
            ManageAccountHyperLink.NavigateUrl = String.Format("subscription.asp?arg={{{0}}}&report=1", sub.SubscriptionID);
            AdvancedReportHyperLink.NavigateUrl = String.Format("advancedreport.asp?arg={{{0}}}", sub.SubscriptionID);
        }
        /// <summary>
        /// Save the Created /Copied Shared Secret Code in the Db for the Current user
        /// </summary>
        private bool UpdateSharedSecret()
        {
            if (!String.IsNullOrEmpty(NewSharedSecretCode.Text))
            {
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);

                Subscription subscription = new Subscription();
                subscription = subscriptionFactory.GetSubscriptionByID(GetSubID());
                subscription.SharedSecret = NewSharedSecretCode.Text.Trim();
                subscriptionFactory.PutSharedSecret(subscription);
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method will update the email domain rule by calling the factory class
        /// </summary>
        private bool UpdateDomainRule()
        {
            bool emailchecked = false;
            if (AllowAllEmailDomainRule() != CB_Email.Checked)
            {
                emailchecked = true;
            }
            if (EmailDomainCollection.SelectedIndex == -1 && String.IsNullOrEmpty(EmailDomainTextBox.Text.Trim()) && !emailchecked)
            {  
                return false;
            }
             RegistrationRuleFactory registrationRuleFactory;

            if (!String.IsNullOrEmpty(EmailDomainTextBox.Text.Trim()))
            {
                RegistrationRule registrationRule = new RegistrationRule();
                registrationRule.EmailMask = EmailDomainTextBox.Text.Trim();
                registrationRule.RuleID = Guid.NewGuid();
                registrationRule.SubscriptionID = subid;
                registrationRuleFactory = new RegistrationRuleFactory(UserConnStr);
                registrationRuleFactory.PutRule(registrationRule);
            }
          
            if (EmailDomainCollection.Items.Count > 0)
            {                

                for (int i = 0; i < EmailDomainCollection.Items.Count; i++)
                {
                    if (EmailDomainCollection.Items[i].Selected == true)
                    {
                        registrationRuleFactory = new RegistrationRuleFactory(UserConnStr);
                        Guid ruleID = new Guid(EmailDomainCollection.Items[i].Value.ToString());
                        registrationRuleFactory.DeleteRule(ruleID);
                    }
                }
            }
            else
            {
                RemoveDomainPanel.Visible = false;
            }
            // if there is intention to change this property, we make sql call
            if (emailchecked)
            {
                UpdateEmailDomainRule(CB_Email.Checked);

            }

            DomainRuleDataBind();
            return true;
        }

        #endregion Private Methods

        #region Protected Methods
        /// <summary>
        /// Add a new rule
        /// </summary>
        protected void AddButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string ruleType = Request.Form["RuleType"];               // Rule type from select (IP, URL, EZProxy)
                AuthenticationRule rule = new AuthenticationRule();
                rule.SubscriptionID = subid;
                rule.IPAddrMask = IPTextBox.Text;
                rule.UrlMask = (ruleType == "EZProxy") ? EZProxyTextBox.Text : URLTextBox.Text;
                rule.Disallow = DisallowCheckBox.Checked;
                rule.NoAnonymous = true;
                rule.Active = true;
                rule.Priority = (String.IsNullOrEmpty(PriorityTextBox.Text)) ? 0 : Convert.ToInt32(PriorityTextBox.Text);
                rule.Comment = NotesTextBox.Text;
                ruleFactory.PutRule(rule);

                State.Add("errmsg", Resources.Resource.RuleAdded);
                B24Redirect(ThisPage);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorAddingRule, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.ErrorAddingRule));
            }
        }

        /// <summary>
        /// This method will update the domain rules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        protected void SubmitClick_UpdateDomain(object sender, EventArgs e)
        {
            try
            {
                if (UpdateDomainRule())
                {
                    State.Add("errmsg", Resources.Resource.DomainUpdated);
                    B24Redirect(ThisPage);
                }
                else
                {
                    State.Add("errmsg", Resources.Resource.DomainNotSelected);
                    B24Redirect(ThisPage);                   
                }
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.DomainNotUpdated, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.DomainNotUpdated));
            }
        }

        /// <summary>
        /// This method will save the newly generated Shared secret code 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitClick_SharedSecret(object sender, EventArgs e)
        {
            try
            {
                if (UpdateSharedSecret())
                {
                    State.Add("errmsg", Resources.Resource.SharedSecretUpdated);
                    B24Redirect(ThisPage);
                }
                else
                {
                    State.Add("errmsg", Resources.Resource.SharedSecretNotSelected);
                    B24Redirect(ThisPage);                 
                }
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SharedSecretNotUpdated, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SharedSecretNotUpdated));
            }
        }

        /// <summary>
        /// Method to generate a new Shared Secret Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitClick_NewSharedSecretCode(object sender, EventArgs e)
        {
            //create a new Shared Secret Code 
            LoginTicket minimal = new LoginTicket();
            try
            {
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                Subscription subscription = new Subscription();
                // Load the subscription.
                subscription = subscriptionFactory.GetSubscriptionByID(GetSubID());
                // The shared secret generated for EZproxy should be 24 characters long.
                subscription.SharedSecret = minimal.Save().Substring(minimal.Save().Length-24);
                // Update the sharedsecret for the subscription id.
                subscriptionFactory.PutSharedSecret(subscription);
                State.Add("errmsg", Resources.Resource.SharedSecretUpdated);
                B24Redirect(ThisPage);
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SharedSecretNotUpdated, ex);
                B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SharedSecretNotUpdated));
            }
            
        }
        #endregion Protected Methods

    }
}