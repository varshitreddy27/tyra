using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Logs;
using System.Globalization;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace B24.Sales4.UserControl
{
    public partial class RemoveRestoreUser : System.Web.UI.UserControl
    {
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
        public Subscription Subscription { get; set; }

        public bool ActionButtonView { get; set; }

        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }

        #endregion

        #region Private Variable

        private Sales4.UI.BasePage basePage;
        private Logger logger = new Logger(Logger.LoggerType.Sales3);
        private UserFactory userFactory;
        private User user;

        /// <summary>
        /// check condition to show form
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// Calculate level to update 
        /// </summary>
        private int level;
        /// <summary>
        /// ckeck condition to find if the user is skillportuser
        /// </summary>
        private bool isSkillportUser;
        /// <summary>
        /// to check the status of the user 
        /// </summary>
        private bool isDeletedUser;
        /// <summary>
        /// to check wherther the user salesgroup contains "EXTRAIN"
        /// </summary>
        private bool isEXTRAIN;
        /// <summary>
        /// to check whether he is skillsoft user
        /// </summary>
        private bool isSkillsoft;
        /// <summary>
        /// to check the Email of the User contains books24x7
        /// </summary>
        private bool isBooks24x7;
        /// <summary>
        /// to check whethere the salesgroup is "FREDSUP"
        /// </summary>
        private bool isSupport;
        /// <summary>
        /// to check condition to show form other than microsoftref2 salesgroup user
        /// </summary>
        private bool showRemoveUser;

        #endregion

        #region Events

        /// <summary>
        /// Page load event
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
                basePage = this.Page as Sales4.UI.BasePage;
                IntitializeControls();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, "Exception in PageLoad", ex);
            }
        }

        /// <summary>
        /// Event to Remove or restore user 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmCheckBox.Checked)
                {
                    userFactory = new UserFactory(basePage.UserConnStr);
                    user = userFactory.GetUserByID(UserId);
                    if (UpdateButton.Text == "Restore")
                    {
                        userFactory.RestoreUser(user.Login);
                    }
                    if (UpdateButton.Text == "Remove")
                    {
                        SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                        subscriptionFactory.UnAssignUser(Subscription.SubscriptionID, UserId, RequestorId);
                    }
                    RemoveRestoreUserErrorLabel.Text = Resources.Resource.UserUpdated;
                    RemoveRestoreUserErrorLabel.Visible = true;
                }
                else
                {
                    RemoveRestoreUserErrorLabel.Text = Resources.Resource.RemoveRestoreConfirm;
                    RemoveRestoreUserErrorLabel.Visible = true;
                }
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                RemoveRestoreUserErrorLabel.Text = Resources.Resource.RemoveRestoreUserFail;
                RemoveRestoreUserErrorLabel.Visible = true;
            }
            IntitializeControls();
            ConfirmCheckBox.Checked = false;
        }

        #endregion

        #region private Methods

        /// <summary>
        /// To initialize the control 
        /// </summary>
        private void IntitializeControls()
        {
            userFactory = new UserFactory(basePage.UserConnStr);
            user = userFactory.GetUserByID(UserId);
            CheckHasAccess();
            if (hasAccess && showRemoveUser)
            {
                ShowForm();
            }
        }

        /// <summary>
        /// To caheck the permission level of the user
        /// </summary>
        private void CheckHasAccess()
        {
            MasterDataFactory masterDataFactory = new MasterDataFactory(basePage.UserConnStr);
            userFactory = new UserFactory(basePage.UserConnStr);

            Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(user.UserID);
            user = userFactory.GetUserByID(UserId);

            isSkillportUser = (user.ApplicationName).ToLower(CultureInfo.InvariantCulture) == "skillport";
            isDeletedUser = user.SubscriptionStatusID == 8; // deleted user statusid
            isEXTRAIN = SearchForString(salesGroupUsers, "EXTRAIN");

            if ((user.Email.ToLower(CultureInfo.InvariantCulture).IndexOf("@books24x7.com", StringComparison.OrdinalIgnoreCase) > 0
               || (user.Email.ToLower(CultureInfo.InvariantCulture).IndexOf("@skillsoft.com", StringComparison.OrdinalIgnoreCase) > 0)
               || (user.Email.ToLower(CultureInfo.InvariantCulture).IndexOf("@smartforce.com", StringComparison.OrdinalIgnoreCase) > 0)))
            {
                isSkillsoft = true;
            }

            isBooks24x7 = (user.Email).ToLower(CultureInfo.InvariantCulture).IndexOf("@books24x7.com", StringComparison.OrdinalIgnoreCase) > 0;
            isSupport = SearchForString(salesGroupUsers, "FREDSUP");
            showRemoveUser = SearchForString(salesGroupUsers, "MSVL2");

            if ((isSupport || isBooks24x7 || isSkillsoft || isEXTRAIN)
                && (!isSkillportUser || (isSkillportUser && isDeletedUser)))
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
        /// check the users salesgroup for a particular string
        /// </summary>
        /// <param name="salesGroupUsers"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private bool SearchForString(Collection<MasterData> salesGroupUsers, string searchString)
        {
            bool returnVar = true;
            if (salesGroupUsers.Count > 0)
            {
                for (int loopCounter = 0; loopCounter < salesGroupUsers.Count; loopCounter++)
                {
                    MasterData salesGroup = salesGroupUsers[loopCounter];
                    if (salesGroup.ResellerCode.ToUpper(CultureInfo.InvariantCulture) == searchString)
                    {
                        returnVar = false;
                        break;
                    }
                }
            }
            return returnVar;
        }

        /// <summary>
        /// to view form detail
        /// </summary>
        private void ShowForm()
        {
            userFactory = new UserFactory(basePage.UserConnStr);
            user = userFactory.GetUserByID(user.UserID);
            if (user.SubscriptionStatusID == 8)
            {
                RestoreUserLabel.Visible = true;
                RemoveUserLabel.Visible = false;
                UpdateButton.Text = "Restore";
            }
            else
            {
                RemoveUserLabel.Visible = true;
                RestoreUserLabel.Visible = false;
                UpdateButton.Text = "Remove";
            }
            RemoveRestoreUserTitleTextLabel.Visible = true;
            ConfirmCheckBox.Visible = true;
            UpdateButton.Visible = ActionButtonView;
        }

        #endregion
    }
}