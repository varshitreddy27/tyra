using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// Reactivate or Extent this Trail.
    /// </summary>
    public partial class ExtendTrail : System.Web.UI.UserControl
    {
        #region Private Members
        private Sales4.UI.BasePage basePage;
        Logger logger;

        #endregion

        #region Public Property
        /// <summary>
        /// get/set Login detail.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// User Id to be updated.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Read Only view.
        /// </summary>
        public bool ExtendTrialReadView { get; set; }
        /// <summary>
        /// Subscription object
        /// </summary>        
        public Subscription Subscription { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        #endregion

        #region Private Variables
        /// <summary>
        /// To get and set Trail Duration.
        /// </summary>
        private Double trialDuration;
        /// <summary>
        /// To get and set Trail Elapsed details.
        /// </summary>
        private Double trialElapsed;
        /// <summary>
        /// To get and set Max Trail Duration .
        /// </summary>
        private int maxtrialDuration;
        /// <summary>
        /// To get and set Max Skillsoft Trail Duration.
        /// </summary>
        private int maxSkilltrialDuration;
        /// <summary>
        /// To get and set Max Reseller Trail Duration .
        /// </summary>
        private int maxReselltrialDuration;
        /// <summary>
        /// To keep access 
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// To store access Level 
        /// </summary>
        private short level;
        /// <summary>
        /// Extend for trial
        /// </summary>
        private Double extend;
        /// <summary>
        /// User account type.
        /// </summary>
        private bool isSkillsoft;

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            basePage = this.Page as Sales4.UI.BasePage;
            if (!Page.IsPostBack)
            {
                try
                {
                    //Initialize the control values
                    InitControls();
                    LoadExtendTrailDetail();
                }
                catch (Exception ex)
                {
                    ExtendTrailErrorLabel.Visible = true;
                    ExtendTrailErrorLabel.Text = Resources.Resource.UserLoadError;

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "User Detail", ex);
                }
            }
        }
        /// <summary>
        /// Edit button click to show update details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            ShowUpdteDetail();
        }

        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            InitControls();
            LoadExtendTrailDetail();
            EditCancelButton.Visible = false;
        }
        /// <summary>
        /// Update event to update the Extend Trial information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) // If validation is success
            {
                try
                {
                    hasAccess = (HasAccessHiddenField.Value == "1") ? true : false;
                    if (hasAccess)
                    {
                        if (DateTime.Compare(DateTime.Now, Subscription.Expires) > 0)
                            Subscription.Expires = DateTime.Now.AddDays(Convert.ToInt16(ExtendTrialAddDaysTextBox.Text, CultureInfo.InvariantCulture));
                        else
                            Subscription.Expires = Subscription.Expires.AddDays(Convert.ToInt16(ExtendTrialAddDaysTextBox.Text, CultureInfo.InvariantCulture));

                        Subscription.Status = SubscriptionStatus.Active;          // activate, in case it isn't

                        SubscriptionFactory subsctiptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                        subsctiptionFactory.PutSubscription(Subscription, UserId, string.Empty);
                    }
                    else
                    {
                        ExtendTrailErrorLabel.Text = Resources.Resource.ExtendTrialFail;
                        ExtendTrailErrorLabel.Visible = true;
                    }

                    InitControls();
                    LoadExtendTrailDetail();

                    // To update the Expire date Info on subscription details.
                    UpdateInfo(null, null);
                    InfoDetailsUpdatePanel.Update();                                          
                }
                catch (SqlException ex)
                {
                    ExtendTrailErrorLabel.Text = Resources.Resource.ExtendTrialFail;
                    ExtendTrailErrorLabel.Visible = true;

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "Extend Trial", ex);
                }
            }
        }

        #endregion

        # region private methods

        /// <summary>
        /// To Load extend Trail details.
        /// </summary>
        private void LoadExtendTrailDetail()
        {
            // Loading Control
            // Commented to show for testing 
            //if (level > 0)
            //{
                Double maxdays = (isSkillsoft) ? maxSkilltrialDuration : maxReselltrialDuration;

                if (DateTime.Compare(DateTime.Now, Subscription.Expires) >= 0)
                {
                    extend = maxdays - trialElapsed; // already expired trial
                }
                else
                {
                    extend = maxdays - trialDuration;   // still-active trial
                }
                extend = (extend < 0) ? 0 : Math.Round(extend);

                TrialDurationLabel.Text = Convert.ToInt16(trialDuration).ToString(CultureInfo.InvariantCulture) + " days";
                TrialStartedLabel.Text = Convert.ToInt16(trialElapsed).ToString(CultureInfo.InvariantCulture) + " days ago";
                ExtendableLabel.Text = Convert.ToInt16(extend).ToString(CultureInfo.InvariantCulture) + " more days";

                // change 'extend' to reflect the number of days they **want** to add
                // initialize to form value or clip to 7 days if defaulting
                if (extend > 0 && extend > 7)
                {
                    extend = 7;
                }
                ExtendHiddenField.Value = extend.ToString(CultureInfo.InvariantCulture);
                if (Subscription == null || Subscription.Status == 0)
                    SubscriptionDisabledLabel.Visible = true;
                else
                    SubscriptionDisabledLabel.Visible = false;

                if (!ExtendTrialReadView)
                {
                    ShowUpdteDetail();
                }
                else
                {
                    B24AsistanceLabel.Visible = false;
                    B24ContactLabel.Visible = false;
                    EditButton.Visible = EditButtonView;
                    AddSpan.Visible = false;
                    ExtendTrialAddDaysTextBox.Visible = false;
                    UpdateButton.Visible = false;
                    EditCancelButton.Visible = false;
                }
            //}
            //else
            //{
            //    NotAccessableTable.Visible = true;
            //    ContentTable.Visible = false;
            //}
        }
        /// <summary>
        /// To Show Update button and Add days text box
        /// </summary>
        private void ShowUpdteDetail()
        {
            extend = Convert.ToDouble(ExtendHiddenField.Value, CultureInfo.InvariantCulture);
            level = Convert.ToInt16(LevelHiddenField.Value, CultureInfo.InvariantCulture);
            EditButton.Visible = false;
            B24AsistanceLabel.Visible = false;
            B24ContactLabel.Visible = false;

            if (extend > 0)
            {
                ExtendTrialAddDaysTextBox.Visible = true;
                ExtendTrialAddDaysTextBox.Text = extend.ToString(CultureInfo.InvariantCulture);
                AddSpan.Visible = true;
            }
            else
            {
                ExtendTrialAddDaysTextBox.Visible = false;
                AddSpan.Visible = false;
            }

            if (extend > 0 && level > 1)
            {
                UpdateButton.Visible = true;
                EditCancelButton.Visible = true;
            }
            else
            {
                AddSpan.Visible = false;
                UpdateButton.Visible = false;
                EditCancelButton.Visible = false;
                B24AsistanceLabel.Visible = true;
                B24ContactLabel.Visible = true;
            }
        }
        /// <summary>
        /// Initialize the control values
        /// </summary>
        private void InitControls()
        {
            PermissionFactory permissionFactory = new PermissionFactory(basePage.UserConnStr);
            Permission permission = permissionFactory.LoadPermissionsById(UserId, basePage.User.Identity.Name, String.Empty);

            UserFactory userFactory = new UserFactory(basePage.UserConnStr);
            User user = userFactory.GetUserByID(UserId);

            if (user != null && (user.Email.ToLower().IndexOf("@books24x7.com", StringComparison.OrdinalIgnoreCase) > 0
                || (user.Email.ToLower().IndexOf("@skillsoft.com", StringComparison.OrdinalIgnoreCase) > 0)
                || (user.Email.ToLower().IndexOf("@smartforce.com", StringComparison.OrdinalIgnoreCase) > 0)))
            {
                isSkillsoft = true;
            }
            else
            {
                isSkillsoft = false;
            }

            if (permission.SuperUser == 1 || permission.GeneralAdmin == 1 || permission.SalesManager == 1)
            {
                hasAccess = true;
            }

            HasAccessHiddenField.Value = (hasAccess) ? "1" : "0";
            int maxUsers = 100;

            //Initialization

            MasterDataFactory masterDataFactory = new MasterDataFactory(basePage.UserConnStr);
            MasterData masterData = masterDataFactory.GetSalesGroup(Guid.Empty, Login, "", Subscription.ResellerCode);

            maxtrialDuration = (masterData != null && masterData.MaxTrialDays > 0) ? +masterData.MaxTrialDays : 60;       // maximum number of days for any trial
            maxSkilltrialDuration = (masterData != null && masterData.MaxTrialDays > 0) ? +masterData.MaxTrialDays : 30;   // maximum number of days for a skillsoft trial
            maxReselltrialDuration = (masterData != null && masterData.MaxTrialDays > 0) ? +masterData.MaxTrialDays : 14;  // maximum number of days for a reseller trial

            bool isTrial = (Subscription != null && Subscription.Type == 0) ? true : false;
            trialDuration = -1;   // how long the trial lasted
            trialElapsed = -1;    // how long since trial was created
            if (Subscription != null)
            {
                trialDuration = Subscription.Expires.Subtract(Subscription.Starts).TotalDays;
                trialElapsed = DateTime.Now.Subtract(Subscription.Starts).TotalDays;
            }
            int seatCount = (Subscription != null) ? +Subscription.Seats : 0;

            if (!isTrial || trialDuration >= maxtrialDuration)
            {
                level = 0;
            }
            else if ((isSkillsoft && trialDuration >= maxSkilltrialDuration)    // trial already too long for skillsoft
                    || (isSkillsoft && trialDuration >= maxReselltrialDuration)  // trial already too long for a reseller
                    || seatCount > maxUsers                                       // trial has too many seats
                    || trialElapsed >= maxtrialDuration)                           // trial was too long ago
            {
                level = 1;
            }
            else
            {
                level = 2;
            }
            LevelHiddenField.Value = level.ToString(CultureInfo.InvariantCulture);
            EditButton.Visible = EditButtonView;
        }

        #endregion

    }
}