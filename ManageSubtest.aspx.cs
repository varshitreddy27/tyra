using System;
using B24.Common;
using System.Collections.ObjectModel;
using B24.Common.Logs;
using System.Collections.Generic;

namespace B24.Sales3.UI
{
    public partial class ManageSubTest : BasePage
    {
        #region Private variables
        private Guid subscriptionId;
        Logger logger = new Logger(Logger.LoggerType.Sales3);
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            GetSubscriptionID();
            try
            {
                InitPopulate();
            }
            catch (Exception exception)
            {
                Response.Redirect("Error.aspx?message=" + exception.Message);
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Get the sub id from the postback or from the url
        /// </summary>
        /// <returns>A subid guid</returns>
        private void GetSubscriptionID()
        {
            subscriptionId = Guid.Empty;   // The subid to return
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    subscriptionId = new Guid(Request.QueryString["arg"]);
                }
                else if (!String.IsNullOrEmpty(Request.Form["arg"]))
                {
                    subscriptionId = new Guid(Request.Form["arg"]);
                }
                else if (!String.IsNullOrEmpty(State["AccountHolderSubId"]))
                {
                    subscriptionId = new Guid(State["AccountHolderSubID"]);
                }
                else
                {
                    Response.Redirect("Error.aspx?message=Empty subscription id provided");
                }
            }
            catch (FormatException formatException)
            {
                Response.Redirect("Error.aspx?message=" + formatException.Message);
            }
            catch (OverflowException overflowException)
            {
                Response.Redirect("Error.aspx?message=" + overflowException.Message);
            }
            catch (ArgumentException argumentException)
            {
                Response.Redirect("Error.aspx?message=" + argumentException.Message);
            }

            // Keep the sub id around for post backs...
            State.Add("AccountHolderSubId", subscriptionId.ToString());
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
        #endregion

        #region Protected methods
        /// <summary>
        /// Initialize the value
        /// </summary>
        protected void InitPopulate()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
            Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(subscriptionId);

            // hide Library Authentication Rules, Library Report Access, Library Persistent URL for non-Library users 
            if (manageSubscription.ApplicationName.ToLower() != "library")
            {
                LibraryReportAccessAccordionPane.Visible = false;
                AuthenticationRulesAccordionPane.Visible = false;
                PersistentURLAccordionPane.Visible = false;
            }

            // Info Details Control 
            InfoDetailsControl.Subscription = manageSubscription;
            InfoDetailsControl.UserId = User.UserID;
            InfoDetailsControl.Login = User.Identity.Name;

            //Add Users
            if (manageSubscription.ApplicationName.ToLower() == "skillport" || manageSubscription.ApplicationName.ToLower() == "smartforce")
            {
                AddUsersAccordionPane.Visible = false;
            }
            else
            {
                AddUsersAccordionPane.Visible = true;
            }
            AddUsersControl.Subscription = manageSubscription;
            AddUsersControl.UserId = User.UserID;
            AddUsersControl.Login = User.Identity.Name;
            AddUsersControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
            AddUsersControl.UpdateInfo = new EventHandler(UpdateInfo);

            BasePage baseObject = this.Page as BasePage;
            PermissionFactory permFactory = new PermissionFactory(UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(User.UserID, baseObject.User.Identity.Name, String.Empty);

            //Extend Trail
            MasterDataFactory masterDataFactory = new MasterDataFactory(UserConnStr);
            MasterData masterData = masterDataFactory.GetSalesGroup(Guid.Empty, User.Identity.Name, "", manageSubscription.ResellerCode);

            int maxtrialDuration = (masterData != null && masterData.MaxTrialDays > 0) ? +masterData.MaxTrialDays : 60;       // maximum number of days for any trial

            bool isTrail = (manageSubscription != null && manageSubscription.Type == 0) ? true : false;
            double trialDuration = -1;   // how long the trial lasted
            if (manageSubscription != null)
            {
                trialDuration = manageSubscription.Expires.Subtract(manageSubscription.Starts).TotalDays;
            }
            if (!IsSupport && (permissions.SalesMarketing == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1 || permissions.SuperUser == 1))
            {
                if (!isTrail || trialDuration >= maxtrialDuration)
                {
                    ExtendTrialAccordionPane.Visible = false;
                }
                else
                {
                    ExtendTrialAccordionPane.Visible = true;
                }
            }
            else
            {
                ExtendTrialAccordionPane.Visible = false;
            }
#warning testing purpose only. remove this code in production
            ExtendTrialAccordionPane.Visible = true;
            //end warning

            ExtendTrailControl.UserId = User.UserID;
            ExtendTrailControl.Login = User.Identity.Name;
            ExtendTrailControl.ExtendTrialReadView = true;
            ExtendTrailControl.Subscription = manageSubscription;
            // add reference to update the info panel conditionally.
            ExtendTrailControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
            ExtendTrailControl.UpdateInfo = new EventHandler(UpdateInfo);

            // SUBSCRIPTION FLAGS
            SubscriptionFlagsControl.SubscriptionId = subscriptionId;

            // SELF REGISTRATION INSTRUCTIONS
            SelfRegistrationUserControl.SubscriptionId = subscriptionId;

            //Self Registration
            Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(User.UserID);
            bool allowSelfReg = SearchForString(salesGroupUsers, "MSVL");
            SelfRegistrationControl.Subscription = manageSubscription;
            SelfRegistrationControl.UserId = User.UserID;
            SelfRegistrationControl.Login = User.Identity.Name;
            if (allowSelfReg || permissions.GeneralAdmin == 1 || permissions.SuperUser == 1)
            {
                SelfRegistrationAccordionPane.Visible = true;
            }

            // Persistent URL
            LibraryUrl.SubId = subscriptionId;
            LibraryUrl.PwdRoot = manageSubscription.PasswordRoot;

            // Welcome Message Sender
            WelcomeMsgSenderControl.SubId = subscriptionId;// manageSubscription.NotifyFromEmail != null ? manageSubscription.NotifyFromEmail : User.Email;

            //Subscription Notes
            SubscriptionNotesControl.Subscription = manageSubscription;
            SubscriptionNotesControl.UserId = User.UserID;
            SubscriptionNotesControl.Login = User.Identity.Name;

            //Update This Account
            UpdateAccountControl.UserId = User.UserID;
            UpdateAccountControl.Login = User.Identity.Name;
            UpdateAccountControl.UpdateAccountReadView = true;
            UpdateAccountControl.Subscription = manageSubscription;

            // add reference to update the info panel conditionally.
            LibReportUsers.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
            LibReportUsers.UpdateInfo = new EventHandler(UpdateInfo);

            Double minDaysLeft = 30;
            int maxUsers = 100;
            bool hasUpgrade = false;
            bool isSkillPort;
            bool isTrial = (manageSubscription != null && manageSubscription.Type == 0) ? true : false;

            DateTime subexpiredate = Subscription.Expires;
            UserFactory userFactory = new UserFactory(UserConnStr);
            Collection<MasterData> salesGroupsList = masterDataFactory.GetSalesGroupsList(User.UserID);

            if (Subscription != null)
            {
                DateTime expires = manageSubscription.Expires;
                DateTime starts = manageSubscription.Starts;
            }

            foreach (MasterData salesGroup in salesGroupsList)
            {
                if (salesGroup.SalesGroupName.ToLower() == "upgrade")
                {
                    hasUpgrade = true;
                    break;
                }
            }

            isSkillPort = (Subscription != null && (manageSubscription.ApplicationName.ToLower() == "skillport" || manageSubscription.ApplicationName.ToLower() == "smartforce")) ? true : false;
            Double daysLeft = 0;

            if (!isTrial)
            {
                daysLeft = manageSubscription.Expires.Subtract(DateTime.Now.Date).TotalDays;
            }
            int seatCount = (manageSubscription != null) ? +manageSubscription.Seats : 0;

            if (!isTrial && daysLeft > minDaysLeft)
            {
                UpdateAccountAccordionPane1.Visible = false;
                if (isSkillPort && hasUpgrade)
                {
                    UpdateAccountAccordionPane1.Visible = true;
                }
            }
            else if ((isTrial || daysLeft < minDaysLeft) && seatCount > maxUsers)
            {
                UpdateAccountAccordionPane1.Visible = true;
            }
            else if (hasUpgrade)
            {
                UpdateAccountAccordionPane1.Visible = true;
            }
            else
            {
                UpdateAccountAccordionPane1.Visible = false;
            }
        }
        #endregion

        #region public mehtods
        /// <summary>
        /// To update the expire date on subscription details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateInfo(object sender, EventArgs args)
        {
            InfoDetailsControl.UpdateData();
        }
        #endregion
    }
}
