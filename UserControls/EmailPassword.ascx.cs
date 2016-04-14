using System;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace B24.Sales4.UserControl
{
    public partial class EmailPassword : System.Web.UI.UserControl
    {

        #region Private Variable

        private GlobalVariables global = GlobalVariables.GetInstance();
        private Logger logger = new Logger(Logger.LoggerType.Sales4);
        private UserFactory userFactory;
        private User user;
        private BasePage baseObject;

        /// <summary>
        /// check condition to show form
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// Calculate level to update 
        /// </summary>
        private int level;

        #endregion

        #region Public Property

        /// <summary>    
        /// User(Admin) making change request
        /// </summary>
        public Guid RequestorId { get; set; }

        /// <summary>
        /// User whose permission we want to change
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///  Subscription Details
        /// </summary>
        public Subscription subscription { get; set; }

        public bool SendButtonView { get; set; }
        #endregion

        #region Events

        /// <summary>
        /// send button click event which wioll send the password to the user email id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (hasAccess && level > 1)
                {
                    userFactory.EmailRegistrationInfo(user.SubscriptionID, RequestorId, UserId);
                }
                EmailPasswordErrorLabel.Visible = true;
                EmailPasswordErrorLabel.Text = "Email sent to " + user.Email;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                EmailPasswordErrorLabel.Visible = true;
                EmailPasswordErrorLabel.Text = Resources.Resource.EmailSendingFail;
            }
        }

        /// <summary>
        /// Page load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (UserId == Guid.Empty)
                {
                    return;
                }
                baseObject = this.Page as B24.Common.Web.BasePage;
                IntitializeControls();
                if (!Page.IsPostBack && hasAccess)
                {
                    ShowForm();
                }
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, "Exception in PageLoad", ex);
            }
        }

        #endregion

        #region private Methods

        /// <summary>
        /// To initialize the control 
        /// </summary>
        private void IntitializeControls()
        {
            userFactory = new UserFactory(baseObject.UserConnStr);
            user = userFactory.GetUserByID(UserId);
            SendButton.Visible = SendButtonView;
            CalculateLevel();
           
        }

        /// <summary>
        /// check the access and level to show the form
        /// </summary>
        private void CalculateLevel()
        {
            if (user.ApplicationName.ToLower(CultureInfo.InvariantCulture) != "skillport")
            {
                hasAccess = true;
                level = 2;
            }
            else
            {
                hasAccess = false;
                level = 0;
            }
        }

        /// <summary>
        /// to display forms based on condition
        /// </summary>
        private void ShowForm()
        {
            UserPreferencesFactory userPreferenceFactory = new UserPreferencesFactory(baseObject.UserConnStr);
            UserPreferences userPreference = userPreferenceFactory.Get(UserId);
            bool migrated = false;
            bool isFlag30 = false;
            bool isFlag26 = false;

            EmailPasswordErrorLabel.Visible = false;

            if (userPreference.PrefString.Contains("vlmigrate"))
            {
                migrated = true;
            }
            SubscriptionFlagFactory subscriptionFlagFactory = new SubscriptionFlagFactory(baseObject.UserConnStr);
            List<B24.Common.SubscriptionFlag> subscriptionFlagList = subscriptionFlagFactory.GetAllSubscriptionFlags(user.SubscriptionID);

            foreach (B24.Common.SubscriptionFlag subscriptionFlag in subscriptionFlagList)
            {
                if (subscriptionFlag.Status == 1)
                {
                    if (subscriptionFlag.FlagID == 30) isFlag30 = true;
                    if (subscriptionFlag.FlagID == 26) isFlag26 = true;
                }
                if (isFlag30 && isFlag26)
                    break;
            }

            if (((subscription.PreAuthenticated == 1) ? true : false && isFlag30) || (isFlag26) || (subscription.GeneralFlags & 67108864) == 67108864)
            {
                noDirectLoginLabel.Visible = true;
            }
            else if (user.Email == "none@books24x7.com")
            {
                noEmailIdLabel.Visible = true;
                EmailPasswordTitleTextLabel.Visible = false;
                EmailPasswordLabel.Visible = false;
                EmailPasswordEmailLabel.Text = user.Email;
                EmailPasswordEmailLabel.Visible = false;
                SendButton.Visible = false;
            }
            else
            {
                if (!migrated)
                {
                    noEmailIdLabel.Visible = false;
                    EmailPasswordTitleTextLabel.Visible = true;
                    EmailPasswordLabel.Visible = true;
                    EmailPasswordEmailLabel.Text = user.Email;
                    EmailPasswordEmailLabel.Visible = true;
                    SendButton.Visible = SendButtonView;
                }
            }
        }

        #endregion
        
        #region Public Method
        /// <summary>
        /// To show form with updated email address.
        /// </summary>
        public void UpdateEmailOption()
        {
            IntitializeControls();
            ShowForm();
        }
        #endregion
    }
}
