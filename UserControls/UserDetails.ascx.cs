using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Logs;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// To display the User information 
    /// </summary>
    public partial class UserDetails : System.Web.UI.UserControl
    {
        #region Private Members

        private Sales4.UI.BasePage basePage;
        Logger logger = new Logger(Logger.LoggerType.UserInfo);
        Boolean editView=false;

        #endregion

        #region Public Property

        /// <summary>
        /// To get or Set the userid of the user
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// To update the email password  panel
        /// </summary>
        public UpdatePanel EmailPasswordUpdatePanel { get; set; }
        /// <summary>
        /// Event handler to handle email password change.
        /// </summary>
        public EventHandler UpdateEmailPassword { get; set; }

        public Boolean EditView
        {
            get { return editView; }
            set { editView = value; }
        }

        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }

        #endregion

        #region Events
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserId == Guid.Empty)
            {
                return;
            }
            basePage = this.Page as Sales4.UI.BasePage;
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadUserData();
                    ShowReadView();
                }
                catch (Exception ex)
                {
                    UserDetailsErrorLabel.Text = Resources.Resource.UserLoadError;
                    UserDetailsErrorLabel.Visible = true;
                    logger.Log(Logger.LogLevel.Error, Resources.Resource.UserLoadError, ex);
                }
            }
        }

        /// <summary>
        /// To Show Editiable view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            ShowEditView();
        }

        /// <summary>
        /// To update the changed values for user details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                User user = userFactory.GetUserByID(UserId);

                user.FirstName = FirstNameTextBox.Text;
                user.LastName = LastNameTextBox.Text;
                user.Email = EmailTextBox.Text.Trim();
                if (CostCenterDropDownList.SelectedIndex != -1)
                {
                    user.CostCenter = CostCenterDropDownList.SelectedItem.Value;
                }
                userFactory.UpdateUserInfo(user, user.UserID);
                if (CostCenterDropDownList.SelectedIndex != -1)
                {
                    userFactory.UpdateUserCostCenterInfo(user);
                }
                UserDetailsErrorLabel.Visible = true;
                UserDetailsErrorLabel.Text = Resources.Resource.UserDetailSuccess;
            }
            catch (SqlException sqlException)
            {
                UserDetailsErrorLabel.Visible = true;
                UserDetailsErrorLabel.Text = Resources.Resource.UserDetailFail;
                logger.Log(Logger.LogLevel.Error, "User Details", sqlException);
            }
            catch (ArgumentNullException argEx)
            {
                UserDetailsErrorLabel.Visible = true;
                UserDetailsErrorLabel.Text = Resources.Resource.UserDetailFail;
                logger.Log(Logger.LogLevel.Error, "User Details", argEx);
            }
            finally
            {
                LoadUserData();
                ShowReadView();
                UpdateEmailPassword(null, null);
                EmailPasswordUpdatePanel.Update(); 
            }

        }

        /// <summary>
        /// To Cancel the changes on User details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            LoadUserData();
            ShowReadView();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load the subscription data
        /// </summary>
        private void LoadUserData()
        {
            try
            {                
                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                User getUser = userFactory.GetUserByID(UserId);

                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                Subscription subscription = subscriptionFactory.GetSubscriptionByID(getUser.SubscriptionID);

                SubIdLabel.Text = subscription.PasswordRoot;
                CompanyDepartmentLabel.Text = subscription.CompanyName + "/" + subscription.Department;
                ApplicationLabel.Text = subscription.ApplicationName;

                LoginLabel.Text = getUser.Login;
                B24NameLabel.Text = getUser.FirstName + " " + getUser.LastName;
                FirstNameTextBox.Text = getUser.FirstName;
                LastNameTextBox.Text = getUser.LastName;
                EmailLabel.Text = getUser.Email;
                EmailTextBox.Text = getUser.Email;

                string collectionString = userFactory.GetUserCollectionString(UserId);
                collectionString = collectionString.Replace(",", ", ");
                CollectionLabel.Text = collectionString;

                CostCenterFactory costCenterFactory = new CostCenterFactory(basePage.UserConnStr);
                List<B24.Common.CostCenter> costCenter = costCenterFactory.GetCostCenterList(subscription.SubscriptionID);
                CostCenterDropDownList.DataSource = costCenter;
                CostCenterDropDownList.DataTextField = "Description";
                CostCenterDropDownList.DataValueField = "CostCenters";
                CostCenterDropDownList.DataBind();
                CostCenterLabel.Text = getUser.CostCenter;
                int count = 0;
                foreach (B24.Common.CostCenter cc in costCenter)
                {
                    if ((getUser.CostCenter != null) && (cc.CostCenters.ToLower(CultureInfo.InvariantCulture) == getUser.CostCenter.ToLower(CultureInfo.InvariantCulture)))
                    {
                        CostCenterLabel.Text = cc.Description;
                        CostCenterDropDownList.SelectedIndex = count;
                        break;
                    }
                    count++;
                }

                if (getUser.ProbationLevel > 0)
                    ScrambledLabel.Text = "yes";
                else
                    ScrambledLabel.Text = "no";

                RestrictionsLabel.Text = showRestriction();
                LostLoginLabel.Text = getUser.LastLogin.ToString("MMM dd yyyy  hh:mmtt", CultureInfo.InvariantCulture);
            }
            catch (SqlException sqlException)
            {
                UserDetailsErrorLabel.Visible = true;
                UserDetailsErrorLabel.Text = Resources.Resource.UserLoadError;
                logger.Log(Logger.LogLevel.Error, "User Details", sqlException);
            }
            catch (ArgumentNullException argEx)
            {
                UserDetailsErrorLabel.Visible = true;
                UserDetailsErrorLabel.Text = Resources.Resource.UserLoadError;
                logger.Log(Logger.LogLevel.Error, "User Details", argEx);
            }

        }

        /// <summary>
        /// To Calculate and show the Restriction
        /// </summary>
        /// <returns></returns>
        private string showRestriction()
        {
            int warningLevel = 0;
            int ackLevel = 0;
            UserPreferencesFactory userPreferenceFactory = new UserPreferencesFactory(basePage.UserConnStr);

            // Getpreferencestring will return user preference string and subscription preference string
            string[] preferenceStrings = userPreferenceFactory.GetPreferenceString(UserId, Guid.Empty);
            string userPreference = preferenceStrings[0];
            if (!String.IsNullOrEmpty(userPreference))
            {
                if (userPreference.IndexOf("warning=", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    int startIndex = userPreference.IndexOf("warning=", StringComparison.OrdinalIgnoreCase);
                    int noOfChar = 0;
                    if (userPreference.IndexOf('|') >= 0)
                        noOfChar = userPreference.IndexOf('|', startIndex) - (startIndex + 8);
                    warningLevel = Convert.ToInt32(userPreference.Substring(startIndex + 8, noOfChar));
                }

                if (userPreference.IndexOf("acklevel=") >= 0)
                {
                    int startIndex = userPreference.IndexOf("acklevel=", StringComparison.OrdinalIgnoreCase);
                    int noOfChar = 0;
                    if (userPreference.IndexOf('|') >= 0)
                        noOfChar = userPreference.IndexOf('|', startIndex) - (startIndex + 9);
                    ackLevel = Convert.ToInt32(userPreference.Substring(startIndex + 9, noOfChar));
                }
            }

            string userRestriction = "";
            if (warningLevel == 1 && ackLevel == 1)
                userRestriction = "Warning 1 (acknowledged)";
            else if (warningLevel == 1)
                userRestriction = "Warning 1";
            else if (warningLevel == 2 && ackLevel == 2)
                userRestriction = "Warning 2 (acknowledged)";
            else if (warningLevel == 2)
                userRestriction = "Warning 2";
            else if (warningLevel >= 3 && ackLevel >= warningLevel)
                userRestriction = "Access Reinstated";
            else if (warningLevel >= 3 && ackLevel < warningLevel)
                userRestriction = "Access Revoked";
            else
                userRestriction = "None";

            return userRestriction;
        }

        /// <summary>
        /// To Show editable view
        /// </summary>
        private void ShowEditView()
        {
            UpdateButton.Visible = true;
            CancelButton.Visible = true;
            EditButton.Visible = false;
            UserDetailsErrorLabel.Visible = false;
            NameLabel.Visible = false;
            CostCenterLabel.Visible = false;
            CostCenterDropDownList.Visible = true;
            EmailLabel.Visible = false;
            EmailTextBox.Visible = true;
            FirstNameLabel.Visible = true;
            FirstNameTextBox.Visible = true;
            LastNameLabel.Visible = true;
            LastNameTextBox.Visible = true;
        }

        /// <summary>
        /// To show the read only view
        /// </summary>
        private void ShowReadView()
        {
            NameLabel.Visible = true;
            B24NameLabel.Visible = true;
            CostCenterLabel.Visible = true;
            CostCenterDropDownList.Visible = false;
            EmailTextBox.Visible = false;
            EmailLabel.Visible = true;
            FirstNameLabel.Visible = false;
            FirstNameTextBox.Visible = false;
            LastNameLabel.Visible = false;
            LastNameTextBox.Visible = false;
            EditButton.Visible = editView;
            UpdateButton.Visible = false;
            CancelButton.Visible = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// To update expire date when trail extended
        /// </summary>
        public void UpdateData()
        {
            UserFactory userFactory = new UserFactory(basePage.UserConnStr);
            User getUser = userFactory.GetUserByID(UserId);
            LoadUserData();
        }


        #endregion
    }
}
