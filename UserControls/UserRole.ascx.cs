using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Logs;
using B24.Common.Web;
using B24.Sales4.UI;

namespace B24.Sales4.UserControls
{
    public partial class UserRole : System.Web.UI.UserControl
    {
        #region Public Property

        // get RqstUserId 
        public Guid UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public bool EditButtonView { get; set; }

        #endregion

        #region Private Members

        //Create Basepage object
        private B24.Sales4.UI.BasePage basePage;

        //Create Logger object
        private Logger logger = new Logger(Logger.LoggerType.Sales3);

        //Selected user role id from dropdownlist
        private int userRoleId;

        //Dropdownlist selected index variable declaration
        private int selectedUserRole;

        // Userid declaration
        private Guid userId;

        #endregion

        #region Events

        /// <summary>
        /// Page on Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as B24.Sales4.UI.BasePage;
            if (UserId == Guid.Empty)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                BindApplcationRole();
            }
        }

        /// <summary>
        /// Update the user role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                RoleBasedAccessFactory roleAccessFactory = new RoleBasedAccessFactory(basePage.UserConnStr);
                if (UserRoleDropDownList.SelectedIndex > 0)
                {
                    userRoleId = Convert.ToInt32(UserRoleDropDownList.SelectedValue);
                    roleAccessFactory.UserId = basePage.User.UserID;
                    roleAccessFactory.PutUserRole(userId, basePage.ApplicationId, userRoleId);
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, this.GetType(), "fade" + DateTime.Now.Ticks, "fadeMessageBlock('fadeBlock','Updated user role');", true);
                }
                else
                {
                    return;
                }

            }
            catch (SqlException sqlException)
            {
                logger.Log(Logger.LogLevel.Error, "UpdateButton_Click", sqlException);
            }
            catch (ArgumentNullException argEx)
            {
                logger.Log(Logger.LogLevel.Error, "UpdateButton_Click", argEx);
            }
        }

        /// <summary>
        /// Get the selected index of UserRoleDropDownList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UserRoleDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserRoleDropDownList.SelectedIndex == 0)
            {
                UpdateButton.Enabled = false;
            }
            else
            {
                UpdateButton.Enabled = true;
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Bind dropdownlist for the selected application
        /// </summary>
        /// <param name="applicationId"></param>
        private void BindApplcationRole()
        {
            try
            {
                RoleBasedAccessFactory roleAccessFactory = new RoleBasedAccessFactory(basePage.UserConnStr);
                List<RoleBasedAccess> applicationRoles = roleAccessFactory.GetApplicationRoles(basePage.ApplicationId);
                UserRoleDropDownList.DataSource = applicationRoles;
                UserRoleDropDownList.DataTextField = "Description";
                UserRoleDropDownList.DataValueField = "RoleId";
                UserRoleDropDownList.DataBind();

                if (userId != Guid.Empty)
                {
                    roleAccessFactory.UserId = userId;
                    List<RoleBasedAccess> userRoles = roleAccessFactory.GetUserRoles(basePage.ApplicationId);
                    UserRoleDropDownList.Items.Insert(0, "--Select Role--");
                    if (userRoles.Count > 0)
                    {
                        UserRoleDropDownList.SelectedValue = userRoles[0].RoleId.ToString();
                        UpdateButton.Enabled = true;
                    }
                    else
                    {
                        UpdateButton.Enabled = false;
                    }
                }
                UpdateButton.Visible = EditButtonView;
            }
            catch (SqlException sqlException)
            {
                logger.Log(Logger.LogLevel.Error, "BindApplcationRole", sqlException);
            }
        }
        #endregion
    }
}