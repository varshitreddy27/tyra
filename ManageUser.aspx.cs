using System;
using System.Web.UI.WebControls;
using System.Globalization;
using B24.Common;
using B24.Common.Logs;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace B24.Sales4.UI
{
    /// <summary>
    ///This class is used to process the information of the particular user being searched by the logged in User
    ///It has three user controls embedded namely Impersonate,Changepassword and userdetail.
    /// </summary>
    public partial class ManageUser : BasePage
    {
        #region Private Members
        /// <summary>
        /// logger object is created 
        /// </summary>
        Logger logger = new Logger(Logger.LoggerType.Sales4);
        /// <summary>
        /// GlobalVariables boject is created 
        /// </summary>
        GlobalVariables global = GlobalVariables.GetInstance();
        /// <summary>
        /// subModule declaration
        /// </summary>
        int subModule;
        /// <summary>
        /// masterPage object is created to valus from the BaseMaster page details
        /// </summary>
        BaseMaster masterPage;

        private BasePage basePage;
        /// <summary>
        /// userID variable declaration
        /// </summary>
        Guid userId;

        string EditPermission = "1";
        #endregion

        #region Protected Method

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                masterPage = this.Master as BaseMaster;
                basePage = this.Page as BasePage;
                GetValuesFromQueryString();
                // Check the user has access
                if (!CheckAccess())
                {
                    return;
                }

                InitializeControls();

                SetView();
                SetFeatures();
                InitPopulate();
            }

            catch (Exception exception)
            {
                Response.Redirect("Error.aspx?message=" + exception.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get all query string values here
        /// </summary>
        private void GetValuesFromQueryString()
        {
            userId = Guid.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["subModule"]))
                {
                    subModule = int.Parse(Request.QueryString["subModule"], CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    userId = new Guid(Request.QueryString["arg"]);
                }
                else if (!String.IsNullOrEmpty(Request.Form["arg"]))
                {
                    userId = new Guid(Request.Form["arg"]);
                }
            }
            catch (FormatException formatException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString FormatException ", formatException);
                return;
            }
            catch (OverflowException overflowException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString OverflowException ", overflowException);
                return;
            }
            catch (ArgumentException argumentException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString ArgumentException ", argumentException);
                return;
            }
        }

        /// <summary>
        /// get the searched user details and set the userid and requesterid(logged in userId)
        /// to the corresponding usercontrols
        /// </summary>
        private void InitPopulate()
        {
            try
            {
                if (subModule > 0 && userId != null && userId != Guid.Empty)
                {
                    // subscription of the user for user administrator
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                    Subscription subscription = subscriptionFactory.GetSubscriptionByUserID(userId);
                    UserFactory userfactory = new UserFactory(basePage.UserConnStr);
                    User user = userfactory.GetUserByID(userId);

                    // Subscription Info Details Control
                   // UserInfoDetails..Subscription = manageSubscription;
                    UserInfoDetails.UserId = userId;
                 //   UserInfoDetails.Login = User.Identity.Name;

                    if (subModule == Sales4Module.SubModuleManageUserInfo)
                    {
                        UserDetail.EmailPasswordUpdatePanel = EmailPasswordUpdatePanel;
                        UserDetail.UpdateEmailPassword = new EventHandler(UpdateEmailPassword);
                        UserDetail.UserId = userId;
                        UserDetail.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        UserDetail.UpdateInfo = new EventHandler(UpdateInfo);
                    }

                    if (subModule == Sales4Module.SubModulePassword)
                    {
                        //Change password
                        ChangePassword.UserId = userId;
                        ChangePassword.RequestorId = User.UserID;

                        //Email User Password
                        EmailPasswordControl.UserId = userId;
                        EmailPasswordControl.RequestorId = User.UserID;
                        EmailPasswordControl.subscription = subscription;
                    }

                    if (subModule == Sales4Module.SubModuleAdminRoles)
                    {
                        //User role
                        UserRoleControl.UserId = userId;

                        // user administrator                   
                        UserAdministratorControl.RequestorId = User.UserID;
                        UserAdministratorControl.subscription = subscription;
                        UserAdministratorControl.User = user;
                        UserAdministratorControl.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        UserAdministratorControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // Advance User Permissions
                        AdvancedUserPermissionControl.User = user;
                        AdvancedUserPermissionControl.RequestorId = User.UserID;
                        AdvancedUserPermissionControl.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        AdvancedUserPermissionControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // User Report Access 
                        UserReportAccessControl.User = user;
                        UserReportAccessControl.RequestorId = User.UserID;
                        UserReportAccessControl.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        UserReportAccessControl.UpdateInfo = new EventHandler(UpdateInfo);
                    }

                    if (subModule == Sales4Module.SubModuleEmailSettings)
                    {
                        // Email Settings
                        EmailSettingsControl.UserId = userId;
                        EmailSettingsControl.EmailSettingsReadView = true;
                        EmailSettingsControl.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        EmailSettingsControl.UpdateInfo = new EventHandler(UpdateInfo);

                        //Remove/Restore User
                        RemoveRestoreUserControl.RequestorId = User.UserID;
                        RemoveRestoreUserControl.Subscription = subscription;
                        RemoveRestoreUserControl.UserId = userId;
                        RemoveRestoreUserControl.InfoDetailsUpdatePanel = UserInfoDetailsUpdatePanel;
                        RemoveRestoreUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        ImpersonateControl.UserId = userId;
                    }
                }
                else if (subModule > 0 && String.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    logger.Log(Logger.LogLevel.Error, "Invalid User");
                    PageErrorLabel.Visible = true;
                    ManageUserMultiView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, " InitPopulate Exception", ex);
                Response.Redirect("Error.aspx");
            }
        }
        /// <summary>
        /// Initialize submodule menu control values
        /// </summary>
        private void InitializeControls()
        {
            try
            {
                UserSearchControl.UpdateCart = new EventHandler(UpdateCart);
                if (userId == Guid.Empty)
                {
                    return; // No need to build sub module menu items 
                }
                if (!IsPostBack)
                {
                    BuildSubModuleMenu();
                }
            }

            catch (ArgumentNullException exception)
            {
                logger.Log(Logger.LogLevel.Error, " InitializeControls ArgumentNullException ", exception);
                return;
            }
            catch (SqlException exception)
            {
                logger.Log(Logger.LogLevel.Error, " InitializeControls Sql Exception", exception);
                return;
            }
        }

        /// <summary>
        /// Build submodule menu item based on user access
        /// </summary>
        private void BuildSubModuleMenu()
        {
            RoleBasedAccessFactory access = new RoleBasedAccessFactory(this.UserConnStr);
            access.UserId = User.UserID;
            List<RoleBasedAccess> subModules = access.GetSubModule(this.ApplicationId, Sales4Module.ModuleManageUsers);
            MenuItem item;
            string args = string.Empty;
            if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
            {
                args = "&arg=" + Request.QueryString["arg"];
            }

            string url = "~/manageuser.aspx?module=" + Sales4Module.ModuleManageUsers + args + "&subModule=";

            if (subModules.Count > 0)
            {
                masterPage.SubMenuPanel.Visible = true;

                foreach (RoleBasedAccess subMod in subModules)
                {
                    item = null;
                    switch (subMod.SubModuleId)
                    {
                        case Sales4Module.SubModuleManageUserInfo:
                            {
                                item = new MenuItem("Info", Sales4Module.SubModuleManageUserInfo.ToString(), "~/images/_.gif", url + Sales4Module.SubModuleManageUserInfo.ToString());
                                break;
                            }
                        case Sales4Module.SubModulePassword:
                            {
                                item = new MenuItem("Password", Sales4Module.SubModulePassword.ToString(), "~/images/_.gif", url + Sales4Module.SubModulePassword.ToString());
                                break;
                            }
                        case Sales4Module.SubModuleAdminRoles:
                            {
                                item = new MenuItem("Admin Roles", Sales4Module.SubModuleAdminRoles.ToString(), "~/images/_.gif", url + Sales4Module.SubModuleAdminRoles.ToString());
                                break;
                            }
                        case Sales4Module.SubModuleEmailSettings:
                            {
                                item = new MenuItem("Settings", Sales4Module.SubModuleEmailSettings.ToString(), "~/images/_.gif", url + Sales4Module.SubModuleEmailSettings.ToString());
                                break;
                            }
                    }
                    if (item != null)
                    {
                        if (subMod.SubModuleId == subModule)
                        {
                            item.Selected = true;
                        }
                        masterPage.SubMenu.Items.Add(item);
                    }
                }
            }
        }
        /// <summary>
        /// To set SubModule features for the user based on the user
        /// </summary>
        private void SetFeatures()
        {
            if (subModule > 0)
            {
                try
                {
                    RoleBasedAccessFactory access = new RoleBasedAccessFactory(this.UserConnStr);
                    access.UserId = User.UserID;
                    List<RoleBasedAccess> subModuleFeature = access.GetSubModuleFeature(this.ApplicationId, Sales4Module.ModuleManageUsers, subModule);
                    if (subModuleFeature.Count > 0)
                    {
                        foreach (RoleBasedAccess feature in subModuleFeature)
                        {
                            switch (feature.FeatureId)
                            {
                                case Sales4Module.FeatureUserDetails:
                                    {
                                        UserInfoDetailsTable.Visible = false;
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            UserDetail.EditView = true;
                                        }
                                        UserDetail.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureChangeUserPassword:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            this.ChangePassword.ChangeButtonView = true;
                                        }
                                        ChangePasswordAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureEmailUserPassword:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            this.EmailPasswordControl.SendButtonView = true;
                                        }
                                        EmailPasswordAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureAdminPermissons:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            UserRoleControl.EditButtonView = true;
                                            UserAdministratorControl.EditButtonView = true;
                                            AdvancedUserPermissionControl.EditButtonView = true;
                                            UserReportAccessControl.EditButtonView = true;
                                        }
                                        AdminPermissionsUpdatePanel.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureEmailSettings:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            EmailSettingsControl.EditButtonView = true;
                                        }
                                        EmailSettingsAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureDisableOrRestore:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            RemoveRestoreUserControl.ActionButtonView = true;
                                        }
                                        RemoveRestoreUserAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureImpersonate:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            ImpersonateControl.ImpersonateButtonView = true;
                                        }
                                        ImpersonateAccordionPane.Visible = true;
                                        break;
                                    }
                            }
                        }
                    }
                }
                catch (ArgumentNullException exception)
                {
                    logger.Log(Logger.LogLevel.Error, " SetFeatures ArgumentNullException ", exception);
                    return;
                }
                catch (SqlException exception)
                {
                    logger.Log(Logger.LogLevel.Error, " SetFeatures Sql Exception", exception);
                    return;
                }
            }

        }
        /// <summary>
        /// Set the appropriate view based on the selected submodule
        /// </summary>
        private void SetView()
        {
            switch (this.subModule)
            {
                case Sales4Module.SubModuleManageUserInfo:
                    {
                        ManageUserMultiView.ActiveViewIndex = 1;
                        break;
                    }
                case Sales4Module.SubModulePassword:
                    {
                        ManageUserMultiView.ActiveViewIndex = 2;
                        break;
                    }
                case Sales4Module.SubModuleAdminRoles:
                    {
                        ManageUserMultiView.ActiveViewIndex = 3;
                        break;
                    }
                case Sales4Module.SubModuleEmailSettings:
                    {
                        ManageUserMultiView.ActiveViewIndex = 4;
                        break;
                    }
                default:
                    {
                        ManageUserMultiView.ActiveViewIndex = 0;
                        UserInfoDetailsTable.Visible = false;
                        break;
                    }
            }

        }

        /// <summary>
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales4Module.ModuleManageUsers, subModule))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                ManageUserMultiView.Visible = false;
            }
            return hasAccess;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// To update the email password details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateEmailPassword(object sender, EventArgs args)
        {
            EmailPasswordControl.UpdateEmailOption();
        }

        /// <summary>
        ///  To update Menu text value when user check cart on UserLookup page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateCart(object sender, EventArgs args)
        {
            BaseMaster baseMaster = this.Master as BaseMaster;
            baseMaster.UpdateCartText();
        }

        /// <summary>
        /// To update the expire date on subscription details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateInfo(object sender, EventArgs args)
        {
            UserInfoDetails.UpdateData();
        }
        #endregion
    }
}