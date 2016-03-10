using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Logs;


namespace B24.Sales4.UI
{
    public partial class ManageAccount : BasePage
    {
        #region Private Members

        /// <summary>
        /// Logger object to log exceptions 
        /// </summary>
        private Logger logger;

        /// <summary>
        /// received subscription id
        /// </summary>
        private Guid subscriptionId;

        /// <summary>
        /// module id received from querystring used to get the sub module menu item
        /// </summary>
        private int module;

        /// <summary>
        /// submodule id received from querystring. Used to get the role
        /// </summary>
        private int subModule = 0;

        /// <summary>
        /// Base master page object to access base value
        /// </summary>
        private BaseMaster masterPage;

        /// <summary>
        /// Action Edit view
        /// </summary>
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
                ManageAccountControl.GroupCodeChanged += new EventHandler<EventArgs>(SyncSingleSignOnGroupCode);
                SingleSignonUserControl.GroupCodeChanged += new EventHandler<EventArgs>(SyncManageAccountGroupCode);
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
            subscriptionId = Guid.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["module"]))
                {
                    module = int.Parse(Request.QueryString["module"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["subModule"]))
                {
                    subModule = int.Parse(Request.QueryString["subModule"], CultureInfo.InvariantCulture);
                }
                // Grap the subscription id from the querystring or form or state
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
        ///  get the searched user details and set the subscriptionId to the corresponding usercontrols
        /// </summary>
        protected void InitPopulate()
        {
            try
            {
                if (subModule > 0 && subscriptionId != null && subscriptionId != Guid.Empty)
                {
                    BasePage baseObject = this.Page as BasePage;
                    PermissionFactory permFactory = new PermissionFactory(UserConnStr);
                    Permission permissions = permFactory.LoadPermissionsById(User.UserID, baseObject.User.Identity.Name, String.Empty);
 
                    // MANAGE ACCOUNT
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                    Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(subscriptionId);

                    MasterDataFactory masterDataFactory = new MasterDataFactory(UserConnStr);
                    // MasterData masterData = masterDataFactory.GetSalesGroup(Guid.Empty, User.Identity.Name, "", manageSubscription.ResellerCode);

                    // Subscription Info Details Control
                    SubscriptionInfoDetails.Subscription = manageSubscription;
                    SubscriptionInfoDetails.UserId = User.UserID;
                    SubscriptionInfoDetails.Login = User.Identity.Name;


                    if (subModule == Sales4Module.SubModuleManageAccountInfo)
                    {
                        // Manage account read only view
                        ManageAccountControl.Subscription = manageSubscription;
                        ManageAccountControl.UserId = User.UserID;
                        ManageAccountControl.Login = User.Identity.Name;
                        ManageAccountControl.EditBtnview = false;
                    }

                    if (subModule == Sales4Module.SubModuleCollections)
                    {
                        // COLLECTIONS
                        CollectionUserControl.UserId = User.UserID;
                        CollectionUserControl.ApplicationName = manageSubscription.ApplicationName;
                        CollectionUserControl.Subscription = manageSubscription;
                        CollectionUserControl.Login = User.Identity.Name;
                        CollectionUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        CollectionUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                    }
                    if (subModule == Sales4Module.SubModuleImplementation)
                    {
                        // Manage account edit view
                        ManageAccountEditControl.Subscription = manageSubscription;
                        ManageAccountEditControl.UserId = User.UserID;
                        ManageAccountEditControl.Login = User.Identity.Name;
                        ManageAccountEditControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        ManageAccountEditControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // SINGLE SIGN ON                
                        SingleSignonUserControl.PasswordRoot = Subscription.PasswordRoot;
                        SingleSignonUserControl.Subscription = manageSubscription;
                        SingleSignonUserControl.UserId = User.UserID;
                        SingleSignonUserControl.Login = User.Identity.Name;
                        SingleSignonUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        SingleSignonUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // PARTNER LOGO
                        BrandLogoUserControl.PassRoot = manageSubscription.PasswordRoot;
                        BrandLogoUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        BrandLogoUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // SUBSCRIPTION FLAGS
                        SubscriptionFlagUserControl.SubscriptionId = subscriptionId;
                        SubscriptionFlagUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        SubscriptionFlagUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // SELF REGISTRATION INSTRUCTIONS
                        SelfRegistrationInstructionUserControl.SubscriptionId = subscriptionId;
                        SelfRegistrationInstructionUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        SelfRegistrationInstructionUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // COST CENTER
                        CostCenterUserControl.SubscriptionID = subscriptionId;
                        CostCenterUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        CostCenterUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // EXTEND ATTRIBUTES
                        ExtendAttributesUserControl.SubscriptionId = subscriptionId;
                        ExtendAttributesUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        ExtendAttributesUserControl.UpdateInfo = new EventHandler(UpdateInfo);

                        // INGENIOUS SANDBOX
                        IngeniousSandboxUserControl.SubscriptionId = subscriptionId;
                        IngeniousSandboxUserControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        IngeniousSandboxUserControl.UpdateInfo = new EventHandler(UpdateInfo);
                    }

                    if (subModule == Sales4Module.SubModuleAddUsers)
                    {
                        // Add Users
                        AddUsersControl.Subscription = manageSubscription;
                        AddUsersControl.UserId = User.UserID;
                        AddUsersControl.Login = User.Identity.Name;
                        AddUsersControl.InfoDetailsUpdatePanel = InfoDetailsUpdatePanel;
                        AddUsersControl.UpdateInfo = new EventHandler(UpdateInfo);
                    }

                    if (subModule == Sales4Module.SubModuleSettings)
                    {
                        // Welcome Message Sender
                        WelcomeMsgSenderControl.SubId = subscriptionId;

                        //Subscription Notes
                        SubscriptionNotesControl.Subscription = manageSubscription;
                        SubscriptionNotesControl.UserId = User.UserID;
                        SubscriptionNotesControl.Login = User.Identity.Name;

                        // Self registration
                        Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(User.UserID);
                        bool allowSelfReg = SearchForString(salesGroupUsers, "MSVL");
                        SelfRegistrationControl.Subscription = manageSubscription;
                        SelfRegistrationControl.UserId = User.UserID;
                        SelfRegistrationControl.Login = User.Identity.Name;
                        if (allowSelfReg || permissions.GeneralAdmin == 1 || permissions.SuperUser == 1)
                        {
                            SelfRegistrationAccordionPane.Visible = true;
                        }

                        // Extend Trail
                        ExtendTrailControl.UserId = User.UserID;
                        ExtendTrailControl.Login = User.Identity.Name;
                        ExtendTrailControl.ExtendTrialReadView = true;
                        ExtendTrailControl.Subscription = manageSubscription;

                        // Alerts 
                        AlertsControl.UserId = User.UserID;
                        AlertsControl.Login = User.Identity.Name;
                        AlertsControl.AlertsReadView = true;
                        AlertsControl.Subscription = manageSubscription;
                    }

                    if (subModule == Sales4Module.SubModuleReports)
                    {
                        // Report control
                        AdvanceReportUserControl.RequestorSubscription = manageSubscription;
                    }

                    if (subModule == Sales4Module.SubModulePurchaseOrRenew)
                    {
                        // Update This Account
                        UpdateAccountControl.UserId = User.UserID;
                        UpdateAccountControl.Login = User.Identity.Name;
                        UpdateAccountControl.UpdateAccountReadView = true;
                        UpdateAccountControl.Subscription = manageSubscription;
                    }

                }
                else if (subModule > 0 && String.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Invalid or empty Subscriptionid");
                    PageErrorLabel.Visible = true;
                    ManageAccountMainView.Visible = false;
                }
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "InitPopulate() Sql Exception", ex);
                Response.Write("Error.aspx?message=" + ex.Message);
            }
            catch (Exception ex)
            {
                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "Error Loading User", ex);
                throw;
            }
        }

        /// <summary>
        /// Initialize submodule menu control values
        /// </summary>
        private void InitializeControls()
        {
            try
            {
                AccountLookupControl.UpdateCart = new EventHandler(UpdateCart);
                if (subscriptionId == Guid.Empty)
                {
                    return; // No need to build sub module menu items 
                }
                if (!IsPostBack)
                {
                    BuildSubModuleMenu();
                }
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Logger.LogLevel.Error, "InitializeControls ArgumentNullexception", ex);
                return;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "InitializeControls Sql Exception", ex);
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
            List<RoleBasedAccess> subModules = access.GetSubModule(this.ApplicationId, Sales4Module.ModuleManageAccounts);
            MenuItem item = null;
            string args = string.Empty;
            if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
            {
                Guid sub = new Guid(Request.QueryString["arg"]);
                args = "&arg=" + HttpUtility.UrlEncode(sub.ToString("B").ToUpperInvariant());
            }
            else
            {
                Guid sub = User.SubscriptionId;
                args = "&arg=" + HttpUtility.UrlEncode(sub.ToString("B").ToUpperInvariant());
            }

            string url = "~/ManageAccount.aspx?module=" + Sales4Module.ModuleManageAccounts + args + "&subModule=";

            if (subModules.Count > 0)
            {
                masterPage.SubMenuPanel.Visible = true;

                foreach (RoleBasedAccess subMod in subModules)
                {
                    item = null;
                    switch (subMod.SubModuleId)
                    {
                        case Sales4Module.SubModuleManageAccountInfo:
                            {
                                item = new MenuItem("Info", Sales4Module.SubModuleManageAccountInfo.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleManageAccountInfo.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModuleCollections:
                            {
                                item = new MenuItem("Collections", Sales4Module.SubModuleCollections.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleCollections.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModuleImplementation:
                            {
                                item = new MenuItem("Implementation", Sales4Module.SubModuleImplementation.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleImplementation.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModuleAddUsers:
                            {
                                item = new MenuItem("Add Users", Sales4Module.SubModuleAddUsers.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleAddUsers.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModuleSettings:
                            {
                                item = new MenuItem("Settings", Sales4Module.SubModuleSettings.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleSettings.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModuleReports:
                            {
                                item = new MenuItem("Reports", Sales4Module.SubModuleReports.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModuleReports.ToString(CultureInfo.InvariantCulture));
                                break;
                            }
                        case Sales4Module.SubModulePurchaseOrRenew:
                            {
                                item = new MenuItem("Purchase/Renew", Sales4Module.SubModulePurchaseOrRenew.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales4Module.SubModulePurchaseOrRenew.ToString(CultureInfo.InvariantCulture));
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
        /// Set the Feature based on the user role
        /// </summary>
        private void SetFeatures()
        {
            if (subModule > 0)
            {
                try
                {
                    RoleBasedAccessFactory access = new RoleBasedAccessFactory(this.UserConnStr);
                    access.UserId = User.UserID;
                    List<RoleBasedAccess> subModulesFeature = access.GetSubModuleFeature(ApplicationId, Sales4Module.ModuleManageAccounts, subModule);
                    if (subModulesFeature.Count > 0)
                    {
                        foreach (RoleBasedAccess feature in subModulesFeature)
                        {
                            switch (feature.FeatureId)
                            {
                                case Sales4Module.FeatureManageAccountInfo:
                                    {
                                        SubscriptionInfoDetails.Visible = false;
                                        ManageAccountControl.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureCollections:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            CollectionUserControl.UpdateApplicationButtonView = true;
                                            CollectionUserControl.AddButtonView = true;
                                        }
                                        CollectionUserControl.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureManageAccount:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            ManageAccountEditControl.EditBtnview = true;
                                        }
                                        AccountDetailsAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureSingleSignOn:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            SingleSignonUserControl.UpdateButtonView = true;
                                        }
                                        SingleSignOnAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureLogo:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            BrandLogoUserControl.EditButtonView = true;
                                        }
                                        PartnerLogoAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureSubscriptionflags:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            SubscriptionFlagUserControl.EditButtonView = true;
                                        }
                                        SubscriptionFlagAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureSelfRegistrationInstruction:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            SelfRegistrationInstructionUserControl.UpdateButtonView = true;
                                        }
                                        SelfRegistrationsInstructionsAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureCostcenter:
                                    {

                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            CostCenterUserControl.AddButtonView = true;
                                        }
                                        CostCentersAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureExtendedAttributes:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            ExtendAttributesUserControl.SaveButtonView = true;
                                        }
                                        ExtendAttributesAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureIngeniusSandbox:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            IngeniousSandboxUserControl.SandBoxCreateButtonView = true;
                                        }
                                        IngeniousSandboxAP.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureAddUsers:
                                    {

                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            AddUsersControl.EditButtonView = true;
                                        }
                                        AddUsersControl.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureWelcomeMessageSender:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            WelcomeMsgSenderControl.ChangeSenderButtonView = true;
                                        }

                                        WelcomeMessageSenderAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureAddSubscriptionNotes:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            SubscriptionNotesControl.EditButtonView = true;
                                        }
                                        SubscriptionNotesAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureSelfRegistration:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            SelfRegistrationControl.EditButtonView = true;
                                        }
                                        SelfRegistrationAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureReactivateOrExtendThisTrial:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            ExtendTrailControl.EditButtonView = true;
                                        }
                                        ExtendTrialAccordionPane.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureReports:
                                    {
                                        AdvanceReportUserControl.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureUpdateThisAccount:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            UpdateAccountControl.EditButtonView = true;
                                        }
                                        UpdateAccountControl.Visible = true;
                                        break;
                                    }
                                case Sales4Module.FeatureModuleAlerts:
                                    {
                                        if (feature.Action.Contains(EditPermission))
                                        {
                                            AlertsControl.EditButtonView = true;
                                        }
                                        AlertsAccordionPanel.Visible = true;
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
                case Sales4Module.SubModuleManageAccountInfo:
                    {
                        ManageAccountMainView.ActiveViewIndex = 1;
                        break;
                    }
                case Sales4Module.SubModuleCollections:
                    {
                        ManageAccountMainView.ActiveViewIndex = 2;
                        break;
                    }
                case Sales4Module.SubModuleImplementation:
                    {
                        ManageAccountMainView.ActiveViewIndex = 3;
                        break;
                    }
                case Sales4Module.SubModuleAddUsers:
                    {
                        ManageAccountMainView.ActiveViewIndex = 4;
                        break;
                    }
                case Sales4Module.SubModuleSettings:
                    {
                        ManageAccountMainView.ActiveViewIndex = 5;
                        break;
                    }
                case Sales4Module.SubModuleReports:
                    {
                        ManageAccountMainView.ActiveViewIndex = 6;
                        break;
                    }
                case Sales4Module.SubModulePurchaseOrRenew:
                    {
                        ManageAccountMainView.ActiveViewIndex = 7;
                        break;
                    }
                default:
                    {
                        ManageAccountMainView.ActiveViewIndex = 0;
                        SubscriptionInfoDetails.Visible = false;
                        break;
                    }
            }

        }

        /// <summary>
        /// update the ManageAccountUpdatePanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncManageAccountGroupCode(Object sender, EventArgs e)
        {
            ManageAccountControl.UpdatedGroupCode();
            ManageAccountUpdatePanel.Update();
        }

        /// <summary>
        /// Update the SingleSignOnUpdatePanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncSingleSignOnGroupCode(Object sender, EventArgs e)
        {
            SingleSignonUserControl.UpdatedGroupCode();
            SingleSignOnUpdatePanel.Update();
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
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales4Module.ModuleManageAccounts, subModule))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                ManageAccountMainView.Visible = false;
            }
            return hasAccess;
        }

        #endregion

        #region Public Methods

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
            SubscriptionInfoDetails.UpdateData();
        }
        #endregion
    }
}