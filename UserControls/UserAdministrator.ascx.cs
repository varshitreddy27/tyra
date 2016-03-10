using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Logs;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Data.SqlClient;

namespace B24.Sales4.UserControl
{
    public partial class UserAdministrator : System.Web.UI.UserControl
    {
        #region Private Members
        private Sales4.UI.BasePage basePage;
        Logger logger = new Logger(Logger.LoggerType.Sales3);
        Boolean editButtonView = false;
        #endregion

        #region Public Property

        /// <summary>    
        /// User(Admin) making change request
        /// </summary>
        public Guid RequestorId { get; set; }

        /// <summary>
        /// User whose permission we want to change
        /// </summary>
        public User User { get; set; }

        /// <summary>
        ///  Subscription Details
        /// </summary>
        public Subscription subscription { get; set; }

        public Boolean EditButtonView
        {
            get { return editButtonView; }
            set { editButtonView = value; }
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

        #region Private Variable
        /// <summary>
        /// check condition to show form
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// Calculate level to update 
        /// </summary>
        private int level;
        /// <summary>
        /// ckeck condition to showAdmin
        /// </summary>
        private bool showAdmin;
        
        #endregion

        #region Events

        /// <summary>
        /// PAhe load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (User == null)
                {
                    return;
                }
                basePage = this.Page as Sales4.UI.BasePage;
                if (!Page.IsPostBack)
                {
                    IntitializeControls();
                }
                Multiview.ActiveViewIndex = 1;
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, "Exception in PageLoad", ex);
            }
        }

        /// <summary>
        /// Event to update the Admisitrator 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (ConfirmCheckBox.Checked == true)
            {
                try
                {
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                    if (AddorRemoveConfirmHiddenField.Value == "add") //if confirm check box is selected to add
                    {
                        subscriptionFactory.AdminReassign(subscription, RequestorId, User.UserID);
                        AdminPermissionsErrorLabel.Text = Resources.Resource.UserAdministratorAdded;


                    }
                    if (AddorRemoveConfirmHiddenField.Value == "remove") //if confirm check box is selected to remove
                    {
                        subscriptionFactory.RemoveAdministrator(subscription, RequestorId, User.UserID);
                        AdminPermissionsErrorLabel.Text = Resources.Resource.UserAdministratorRemoved;

                    }
                    AdminPermissionsErrorLabel.Visible = true;

                }
                catch (NullReferenceException ex)
                {
                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                    AdminPermissionsErrorLabel.Text = Resources.Resource.Error;
                    AdminPermissionsErrorLabel.Visible = true;
                }
                catch (SqlException ex)
                {
                    logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                    AdminPermissionsErrorLabel.Text = Resources.Resource.UsersNotAdded;
                    AdminPermissionsErrorLabel.Visible = true;
                }
            }
            else
            {
                AdminPermissionsErrorLabel.Text = Resources.Resource.UserAdministratorConfirm;
                AdminPermissionsErrorLabel.Visible = true;
            }
            //reload the form
            ShowForm();
            Multiview.ActiveViewIndex = 1;
            ConfirmCheckBox.Checked = false;
        }

        /// <summary>
        /// Edit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            AdminPermissionsErrorLabel.Text = String.Empty;
            Multiview.ActiveViewIndex = 0;

        }

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            AdminPermissionsErrorLabel.Text = String.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        #endregion

        #region private Methods

        /// <summary>
        /// to initialize the control to show the details
        /// </summary>
        private void IntitializeControls()
        {
            CheckHasAccess();
            MasterDataFactory masterDataFactory = new MasterDataFactory(basePage.UserConnStr);
            Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(User.UserID);
            // User Admin form is hidden for microsofteref3 users
            showAdmin = SearchForString(salesGroupUsers, "MSVL2");
            if (showAdmin && hasAccess)
            {
                ShowForm();
            }
        }

        /// <summary>
        /// to calculate level and access
        /// </summary>
        private void CheckHasAccess()
        {
            if (subscription.ApplicationName.ToLower(CultureInfo.InvariantCulture) == "skillport" || subscription.ApplicationName.ToLower(CultureInfo.InvariantCulture) == "smartforce")
            {
                hasAccess = false;
                level = 0;
            }
            else
            {
                hasAccess = true;
                level = 2;
            }
        }

        /// <summary>
        /// to view form detail
        /// </summary>
        private void ShowForm()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
            Subscription subscriptionOfUser = subscriptionFactory.GetSubscriptionByUserID(User.UserID);
            string adminsistrator = subscriptionOfUser.Administrators;
            string TempUserIdString = User.UserID.ToString();
            UserFactory userfactory = new UserFactory(basePage.UserConnStr);
            User CutomerAdminIDUser = userfactory.GetUserByID(subscriptionOfUser.CustomerAdminID);
            User admin = (adminsistrator.IndexOf(TempUserIdString, StringComparison.OrdinalIgnoreCase) > -1) ? User : CutomerAdminIDUser;

            AdminChangeTextLabel.Visible = true;
            AdminChangeTextReadLabel.Visible = true;
            bool isadmin = (User.UserID == admin.UserID) ? true : false;
            if (isadmin)
            {
                CurrentAdminUserTitleLabel.Visible = true;
                RemoveAdministratorLabel.Visible = true;
                RemoveCurrentAdministratorLabel.Text = User.FirstName + " " + User.LastName + " " + "(" + User.Email + ")";
                RemoveCurrentAdministratorLabel.Visible = true;
                ConfirmCheckBox.Visible = true;
                CurrentAdminUserTitleReadLabel.Visible = true;
                RemoveAdministratorReadLabel.Visible = true;
                RemoveCurrentAdministratorReadLabel.Text = User.FirstName + " " + User.LastName + " " + "(" + User.Email + ")";
                RemoveCurrentAdministratorReadLabel.Visible = true;
                AddorRemoveConfirmHiddenField.Value = "remove";

                CurrentAdminLabel.Visible = false;
                AddCurrentAdministratorTextLabel.Visible = false;
                AddAdministratorTextLabel.Visible = false;
                AddAdministratorLabel.Visible = false;
                CurrentAdminReadLabel.Visible = false;
                AddCurrentAdministratorTextReadLabel.Visible = false;
                AddAdministratorTextReadLabel.Visible = false;
                AddAdministratorReadLabel.Visible = false;
            }

            else
            {
                CurrentAdminLabel.Visible = true;
                AddCurrentAdministratorTextLabel.Text = admin.FirstName + " " + admin.LastName + " " + "(" + admin.Email + ")";
                AddCurrentAdministratorTextLabel.Visible = true;
                AddAdministratorTextLabel.Visible = true;
                AddAdministratorLabel.Text = User.FirstName + " " + User.LastName + " " + "(" + User.Email + ")";
                AddAdministratorLabel.Visible = true;
                CurrentAdminReadLabel.Visible = true;
                AddCurrentAdministratorTextReadLabel.Text = admin.FirstName + " " + admin.LastName + " " + "(" + admin.Email + ")";
                AddCurrentAdministratorTextReadLabel.Visible = true;
                AddAdministratorTextReadLabel.Visible = true;
                AddAdministratorReadLabel.Text = User.FirstName + " " + User.LastName + " " + "(" + User.Email + ")";
                AddAdministratorReadLabel.Visible = true;
                ConfirmCheckBox.Visible = true;
                AddorRemoveConfirmHiddenField.Value = "add";

                CurrentAdminUserTitleLabel.Visible = false;
                RemoveAdministratorLabel.Visible = false;
                RemoveCurrentAdministratorLabel.Visible = false;
                CurrentAdminUserTitleReadLabel.Visible = false;
                RemoveAdministratorReadLabel.Visible = false;
                RemoveCurrentAdministratorReadLabel.Visible = false;
            }
            EditButton.Visible = editButtonView;
            if (level > 1)
            {
                UpdateButton.Visible = true;
                EditCancelButton.Visible = true;
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

        #endregion
    }
}