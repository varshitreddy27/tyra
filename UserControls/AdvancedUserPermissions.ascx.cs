using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;

namespace B24.Sales3.UserControl
{
    public partial class AdvancedUserPermissions : System.Web.UI.UserControl
    {
        #region Private Members

        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.Sales3);
        Boolean editButtonView = false;
        private BasePage baseObject;

        #endregion

        #region Property

        /// <summary>    
        /// User(Admin) making change request
        /// </summary>
        public Guid RequestorId { get; set; }
        /// <summary>
        /// User whose permission we want to change
        /// </summary>
        public User User { get; set; }

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

        #region Private variables
        /// <summary>
        /// to check whether user has CFA role
        /// </summary>
        private bool isCFA;
        /// <summary>
        /// to check whether user has CBTA role
        /// </summary>
        private bool isCTA;
        /// <summary>
        /// user role
        /// </summary>
        private string role;
        #endregion

        #region Events

        /// <summary>
        /// Page on Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User == null)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                baseObject = this.Page as BasePage;
                InitializeControl();
            }

            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Event to update the Advance user permissions 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserFactory userFactory = new UserFactory(baseObject.UserConnStr);
                if (CFACheckBox.Checked)
                {
                    role = "CFA";
                    string userRoles = userFactory.GetUserRolesByID(User.UserID);
                    if (userRoles.IndexOf(role, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        userRoles = (userRoles.Length != 0) ? userRoles + "," + role : role;  // add it
                    }

                    userFactory.GrantUserRoles(User.UserID, userRoles, RequestorId);

                }
                if (!CFACheckBox.Checked)
                {
                    role = "CFA";
                    string userRoles = userFactory.GetUserRolesByID(User.UserID);
                    if (userRoles.IndexOf(role, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        userRoles = userRoles.Replace(userRoles, "");  // remove it
                    }
                    userFactory.GrantUserRoles(User.UserID, userRoles, RequestorId);
                }

                if (CTACheckBox.Checked)
                {
                    role = "CBTA";
                    string userroles = userFactory.GetUserRolesByID(User.UserID);
                    if (userroles.IndexOf(role, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        userroles = (userroles.Length != 0) ? userroles + "," + role : role;  // add it
                    }

                    userFactory.GrantUserRoles(User.UserID, userroles, RequestorId);
                }
                if (!CTACheckBox.Checked)
                {
                    role = "CBTA";
                    string userRoles = userFactory.GetUserRolesByID(User.UserID);
                    if (userRoles.IndexOf(role, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        userRoles = userRoles.Replace(userRoles, "");  // remove it
                    }
                    userFactory.GrantUserRoles(User.UserID, userRoles, RequestorId);
                }
                AdvancedUserPermissionErrorLabel.Text = Resources.Resource.UserEntitlementUpdated;
                AdvancedUserPermissionErrorLabel.Visible = true;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                AdvancedUserPermissionErrorLabel.Text = Resources.Resource.UserEntilementFailed;
                AdvancedUserPermissionErrorLabel.Visible = true;
            }
            InitializeControl();
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Edit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            AdvancedUserPermissionErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 0;
        }

        /// <summary>
        /// Handle the cancel button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            AdvancedUserPermissionErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// initailize the control
        /// </summary>
        private void InitializeControl()
        {
            UserFactory userFactory = new UserFactory(baseObject.UserConnStr);
            isCFA = userFactory.HasRole(User.UserID, "CFA");
            isCTA = userFactory.HasRole(User.UserID, "CBTA");
            if (isCFA == true)
            {
                CFACheckBox.Checked = true;
                CFAReadCheckBox.Checked = true;
            }
            else
            {
                CFACheckBox.Checked = false;
                CFAReadCheckBox.Checked = false;
            }
            if (isCTA == true)
            {
                CTACheckBox.Checked = true;
                CTAReadCheckBox.Checked = true;
            }
            else
            {
                CTACheckBox.Checked = false;
                CTAReadCheckBox.Checked = false;
            }
            EditButton.Visible = editButtonView;
        }

        #endregion
    }
}
