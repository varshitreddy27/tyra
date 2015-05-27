using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using B24.Common;
using B24.Common.Security;
using B24.Common.Web;
using B24.Common.Logs;
using System.Globalization;

namespace B24.Sales3.UI
{
    public partial class CreateTrial : BasePage
    {
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);

        #region Private Members
        /// <summary>
        /// received subscription id
        /// </summary>
        private Guid subscriptionId;
        /// <summary>
        /// module id received from querystring used to get the sub module menu item
        /// </summary>
        private int module;
        /// <summary>
        /// 2= reseller generated scrambled, 3=reseller generated unscrambled
        /// </summary>
        private int trialType;
        /// <summary>
        /// submodule id received from querystring. Used to get the role
        /// </summary>
        private int subModule = 0;
        /// <summary>
        /// to keep all pendingregistrationid of the subscription
        /// </summary>
        private string PendingRegistrationIdString = "";
        /// <summary>
        /// to store the PendingRegistrationId
        /// </summary>
        private Guid pendingRegistrationId;
        /// <summary>
        /// to check whether the textbox feilds are empty
        /// </summary>
        private bool fieldsNotEmpty;

        private MasterDataFactory masterDataFactory;
        private BasePage basePage;
        /// <summary>
        /// Base master page object to access base value
        /// </summary>
        private BaseMaster masterPage;

        /// <summary>
        /// to store the collectionstring
        /// </summary>
        private string collectionString;

        #endregion Private Members


        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verify credentials for Library User         
            try
            {
                masterPage = this.Master as BaseMaster;

                GetValuesFromQueryString();
                // Check the user has access
                //if (!CheckAccess())
                //{
                //    return;
                //}
                basePage = this.Page as BasePage;
                if (basePage == null)
                {
                    return;
                }
                trialType = (IsSkillsoft) ? 3 : 2;
            }
            catch (NullReferenceException ex)
            {

                logger.Log(Logger.LogLevel.Error, Resources.Resource.NullReference, ex);
                this.basePage.B24Errors.Add(new B24Error(Resources.Resource.ErrorSales3));
            }

            if (!IsPostBack)
            {
                try
                {
                    BuildSubModuleMenu();
                    // Populate the SalesGroup description in the dropdown 
                    BindSalesGroup();
                    // Populate the Sales Colleagues name in the dropdown 
                    BindAssignTo();
                    // Populate the Collection values in the dropdown 
                    BindCollections();
                    //Set Default value for country to US
                    CountryDropDownList.SelectedValue = "US";
                    // retrieve any pending registrations
                    PendingRegistrations();
                }
                catch (Exception ex)
                {

                    logger.Log(Logger.LogLevel.Error, Resources.Resource.BindValues, ex);
                    B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.BindValues));
                }
            }

        }

        /// <summary>
        /// This method will save the subscription values
        /// </summary>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (ValidateCollections())
            {

                try
                {   //Save the Library Trial Subscription
                    SaveSubscription();
                    RegisterUsers();
                    //Redirect to the signup.asp Page
                    Response.Redirect("signup.asp?donelibrarytrial=true");
                }
                catch (Exception ex)
                {
                    logger.Log(Logger.LogLevel.Error, Resources.Resource.LibraryTrialSubscriptionNotCreated, ex);
                    B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.LibraryTrialSubscriptionNotCreated));
                }
            }
            else
            {
                collectionError.Visible = true;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method will save the subscription by passing the values to factory class
        /// </summary>
        private void SaveSubscription()
        {
            try
            {
                string selectedItems = string.Empty;

                Subscription subscription = new Subscription();
                subscription.CompanyName = CompanyNameTextBox.Text.Trim();
                subscription.Department = DepartmentTextBox.Text.Trim();
                subscription.Country = CountryDropDownList.SelectedValue;
                String selectedCollections = String.Empty;
                foreach (ListItem item in CollectionsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        if (!string.IsNullOrEmpty(selectedCollections))
                        {
                            selectedCollections += ",";
                        }
                        selectedCollections += item.Value;
                    }
                }

                subscription.CollectionString = selectedCollections;
                subscription.SalesGroup = SalesGroupDropDownList.SelectedValue;
                subscription.SalesPersonID = new Guid(AssignToDropDownList.SelectedValue);

                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                subscriptionFactory.PutSubscriptionTrial(subscription, trialType);
                //subscriptionFactory.PutLibraryTrial(subscription);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Display the pending registration for the subscription if any
        /// </summary>
        private void PendingRegistrations()
        {
            PendingRegistrationFactory pendingRegistrationFactory = new PendingRegistrationFactory(UserConnStr);
            Collection<PendingRegistration> pendingRegistrationCollection = pendingRegistrationFactory.GetPendingRegistrations(Subscription.SubscriptionID, User.UserID, Guid.Empty);
            if (pendingRegistrationCollection.Count > 0)
            {
                PendingRegistrationGridView.DataSource = pendingRegistrationCollection;
                PendingRegistrationGridView.DataBind();
                //PendingRegistrationReadGridView.DataSource = pendingRegistrationCollection;
                //PendingRegistrationReadGridView.DataBind();
                PendingRegistrationsLabel.Visible = true;
            }

            int noDirectAccess = (Subscription != null) ? Subscription.PreAuthenticated : 0;

            if (noDirectAccess == 1) //No direct access user
            {
                NodirectAccessLabel.Visible = true;
            }
            else
            { 
                //if (userCount > 0)
                //{
                    //AddUserHelpTextLabel.Visible = true;
                    //AddUserHelpText2Label.Visible = true;

                    //Bind the pending registration in the add users table (only "error" status user is added).
                    int counter = 1;
                    foreach (PendingRegistration pendingRegistration in pendingRegistrationCollection)
                    {
                        if (counter < 5)
                        {
                            if (pendingRegistration.Status.ToString() == "Error")
                            {
                                TextBox FirstNameTextBox = (TextBox)FindControl("FirstNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
                                TextBox LastNameTextBox = (TextBox)FindControl("LastNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
                                TextBox EmailTextBox = (TextBox)FindControl("EmailTextBox" + counter.ToString(CultureInfo.InvariantCulture));
                                TextBox LoginTextBox = (TextBox)FindControl("LoginTextBox" + counter.ToString(CultureInfo.InvariantCulture));
                                TextBox PasswordTextBox = (TextBox)FindControl("PasswordTextBox" + counter.ToString(CultureInfo.InvariantCulture));
                                FirstNameTextBox.Text = pendingRegistration.FirstName;
                                LastNameTextBox.Text = pendingRegistration.LastName;
                                EmailTextBox.Text = pendingRegistration.Email;
                                LoginTextBox.Text = pendingRegistration.Login;
                                if (pendingRegistration.Password.IndexOf("temp:") == 0)
                                {
                                    PasswordTextBox.Text = pendingRegistration.Password.Substring(("temp:").Length);
                                }

                                else
                                {
                                    PasswordTextBox.Text = pendingRegistration.Password;
                                }
                                PendingRegistrationIdString += pendingRegistration.PendingRegistrationId + ",";
                                counter++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    BindCollections();
                    //if (this.level > 1 && this.hasAccess)
                    //{
                    //    UpdateButton.Visible = true;
                    //}
                    //else
                    //{
                    //    B24AsistanceLabel.Visible = true;
                    //}
                //}
                //else
                //{
                //    NoSeatsLabel.Visible = true;
                //}
            }
        }

        /// <summary>
        /// This method is used to validate collections
        /// Atleast one of the collection should be selected
        /// </summary>
        private bool ValidateCollections()
        {
            if (CollectionsCheckBoxList.SelectedIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Register the user(s)
        /// </summary>
        private void RegisterUsers()
        {
            // Get selected collection string
            CollectionFactory collectionFactory = new CollectionFactory(UserConnStr);
            List<Collection> collectionList = collectionFactory.GetCollections(Subscription.SubscriptionID, null);
            int collectionCount = CollectionsCheckBoxList.Items.Count;
            string collectionsSelected = string.Empty;
            for (int position = 0; position < collectionCount; position++)
            {
                if (CollectionsCheckBoxList.Items[position].Selected)
                {
                    foreach (Collection collection in collectionList)
                    {
                        if (collection.CollectionID == new Guid(CollectionsCheckBoxList.Items[position].Value))
                        {
                            collectionsSelected += collection.Name;
                            if (position < collectionCount - 1)
                            {
                                collectionsSelected += ",";
                            }
                        }
                    }

                }
            }
            PendingRegistrationFactory pendingRegistrationFactory = new PendingRegistrationFactory(UserConnStr);
            PendingRegistration pendingreg;
            if (CheckFields(1))
            {
                //create user-1
                if (PendingRegistrationIdString.Length > 1)
                {
                    pendingRegistrationId = new Guid(PendingRegistrationIdString.Substring(0, 36));
                }
                else
                {
                    pendingRegistrationId = Guid.Empty;
                }
                pendingreg = PendingRegisterObject(pendingRegistrationId, FirstNameTextBox1.Text, LastNameTextBox1.Text, LoginTextBox1.Text, EmailTextBox1.Text, "temp:" +
                PasswordTextBox1.Text, 0, String.Empty, Guid.Empty, String.Empty,
                String.Empty, collectionString, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, basePage.User.UserID, pendingreg);
                fieldsNotEmpty = false;
            }

            if (CheckFields(2))
            {
                //create a user-2
                if (PendingRegistrationIdString.Length > 38)
                {
                    pendingRegistrationId = new Guid(PendingRegistrationIdString.Substring(37, 36));
                }
                else
                {
                    pendingRegistrationId = Guid.Empty;
                }
                pendingreg = PendingRegisterObject(pendingRegistrationId, FirstNameTextBox2.Text, LastNameTextBox2.Text, LoginTextBox2.Text, EmailTextBox2.Text, "temp:" +
                    PasswordTextBox2.Text, 0, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, basePage.User.UserID, pendingreg);
                fieldsNotEmpty = false;
            }

            if (CheckFields(3))
            {
                if (PendingRegistrationIdString.Length > 74)
                {
                    pendingRegistrationId = new Guid(PendingRegistrationIdString.Substring(74, 36));
                }
                else
                {
                    pendingRegistrationId = Guid.Empty;
                }
                //create a user-3
                pendingreg = PendingRegisterObject(pendingRegistrationId, FirstNameTextBox3.Text, LastNameTextBox3.Text, LoginTextBox3.Text, EmailTextBox3.Text, "temp:" +
                    PasswordTextBox3.Text, 0, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, basePage.User.UserID, pendingreg);
                fieldsNotEmpty = false;
            }

            if (CheckFields(4))
            {
                if (PendingRegistrationIdString.Length > 111)
                {
                    pendingRegistrationId = new Guid(PendingRegistrationIdString.Substring(111, 36));
                }
                else
                {
                    pendingRegistrationId = Guid.Empty;
                }
                //create a user-4
                pendingreg = PendingRegisterObject(pendingRegistrationId, FirstNameTextBox4.Text, LastNameTextBox4.Text, LoginTextBox4.Text, EmailTextBox4.Text, "temp:" +
                    PasswordTextBox4.Text, 0, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, basePage.User.UserID, pendingreg);
                fieldsNotEmpty = false;
            }
            if (CheckFields(5))
            {
                if (PendingRegistrationIdString.Length > 148)
                {
                    pendingRegistrationId = new Guid(PendingRegistrationIdString.Substring(148, 36));
                }
                else
                {
                    pendingRegistrationId = Guid.Empty;
                }
                //create a user-5
                pendingreg = PendingRegisterObject(pendingRegistrationId, FirstNameTextBox5.Text, LastNameTextBox5.Text, LoginTextBox5.Text, EmailTextBox5.Text, "temp:" +
                    PasswordTextBox5.Text, 0, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, basePage.User.UserID, pendingreg);
                fieldsNotEmpty = false;
            }
        }

        /// <summary>
        /// Create Pending Registration object.
        /// </summary>
        /// <param name="pendingRegistrationid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="useCookie"></param>
        /// <param name="sendWelcomeEmail"></param>
        /// <param name="costCenter"></param>
        /// <param name="preReqstID"></param>
        /// <param name="fixedUserName"></param>
        /// <param name="userDeptName"></param>
        /// <param name="collectionString"></param>
        /// <param name="status"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private PendingRegistration PendingRegisterObject(Guid pendingRegistrationid, string firstName, string lastName,
            string login, string email, string password, int useCookie,  string costCenter, Guid preReqstID, string fixedUserName,
                    string userDeptName, string collectionString, int status, string error)
        {
            PendingRegistration pendingRegistration = new PendingRegistration();
            pendingRegistration.PendingRegistrationId = pendingRegistrationid;
            pendingRegistration.FirstName = firstName;
            pendingRegistration.LastName = lastName;
            pendingRegistration.Login = login;
            pendingRegistration.Email = email;
            pendingRegistration.Password = password;
            pendingRegistration.UseCookie = useCookie;
            pendingRegistration.SendEmail = 1;
            pendingRegistration.CostCenter = costCenter;
            pendingRegistration.PreRegisterId = preReqstID;
            pendingRegistration.FixedUserName = fixedUserName;
            pendingRegistration.UserDeptartmentName = userDeptName;
            pendingRegistration.CollectionString = collectionString;
            pendingRegistration.Status = PendingRegistration.PendingRegistrationStatus.Submitted;
            pendingRegistration.ErrorString = error;

            return pendingRegistration;
        }

        private bool CheckFields(int counter)
        {
            TextBox FirstNameTextBox = (TextBox)FindControl("FirstNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox LastNameTextBox = (TextBox)FindControl("LastNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox EmailTextBox = (TextBox)FindControl("EmailTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox LoginTextBox = (TextBox)FindControl("LoginTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox PasswordTextBox = (TextBox)FindControl("PasswordTextBox" + counter.ToString(CultureInfo.InvariantCulture));


            if (!String.IsNullOrEmpty(FirstNameTextBox.Text.Trim()) || !String.IsNullOrEmpty(LastNameTextBox.Text.Trim())
              || !String.IsNullOrEmpty(EmailTextBox.Text.Trim()) || !String.IsNullOrEmpty(PasswordTextBox.Text.Trim()))
            {
                fieldsNotEmpty = true;
            }
            return fieldsNotEmpty;
        }

        /// <summary>
        /// This method will bind the sales group description in the dropdown list
        /// </summary>
        private void BindSalesGroup()
        {
            masterDataFactory = new MasterDataFactory(UserConnStr);
            Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(User.UserID);
            // Filter the records for sales group type other than zero
            IEnumerable<MasterData> salesGroups = salesGroupUsers.Where(salesObject => salesObject.SalesGroupType != 0 && salesObject.SalesGroupStatus == 1);
            SalesGroupDropDownList.DataSource = salesGroups;
            SalesGroupDropDownList.DataTextField = "SalesGroupName";
            SalesGroupDropDownList.DataValueField = "ResellerCode";
            SalesGroupDropDownList.DataBind();
        }

        /// <summary>
        /// This method will bind the sales colleagues name in the dropdown list
        /// </summary>
        private void BindAssignTo()
        {
            masterDataFactory = new MasterDataFactory(UserConnStr);
            Collection<MasterData> salesColleagues = masterDataFactory.GetSalesColleagues(User.UserID);
            IEnumerable<MasterData> ordersalesColleagues = salesColleagues.OrderBy(MasterData => MasterData.FullName);
            AssignToDropDownList.DataSource = ordersalesColleagues;
            AssignToDropDownList.DataTextField = "FullName";
            AssignToDropDownList.DataValueField = "UserId";
            AssignToDropDownList.DataBind();
            AssignToDropDownList.SelectedValue = User.UserID.ToString();

        }

        /// <summary>
        /// This method will bind the collections in the check box list
        /// </summary>
        private void BindCollections()
        {
            CollectionFactory collectionFactory = new CollectionFactory(UserConnStr);
            List<Collection> cList = collectionFactory.GetCollectionsForUser(Guid.Empty, User.UserID, AppName, null, Collection.CollectionType.All);
            CollectionsCheckBoxList.DataSource = cList;
            CollectionsCheckBoxList.DataTextField = "Description";
            CollectionsCheckBoxList.DataValueField = "Name";
            CollectionsCheckBoxList.DataBind();

            Dictionary<Guid, string> collcetionNames = new Dictionary<Guid, string>();
            Dictionary<Guid, int> collcetionDefault = new Dictionary<Guid, int>();

            int collectionListCount = 1;
            foreach (Collection collection in cList)
            {
                collcetionNames.Add(collection.CollectionID, collection.Name);
                collcetionDefault.Add(collection.CollectionID, collection.IsDefault);

                if (collectionListCount < cList.Count)
                {
                    collectionString += collection.Name + ",";
                    collectionListCount++;
                }
                else
                {
                    collectionString += collection.Name;
                }
            }
        }

        /// <summary>
        /// Build submodule menu item
        /// This should be based on user access but it is possible
        /// that it is the same access level for both menu items. 
        /// Find out from Jen (GiLi)
        /// </summary>
        private void BuildSubModuleMenu()
        {

            string args = string.Empty;
           
            masterPage.SubMenuPanel.Visible = true;

            string url = "~/CreateTrial.aspx?module=" + Sales3Module.SubModuleCreateNewTrialUser + args + "&subModule=";
            MenuItem itemRegTrial = new MenuItem("Create a Trial User", Sales3Module.SubModuleCreateNewTrialUser.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales3Module.SubModuleManageAccountInfo.ToString(CultureInfo.InvariantCulture));

            url = "~/LibraryTrial.aspx?module=" + Sales3Module.SubModuleCreateNewTrial + args + "&subModule=";
            MenuItem itemLibraryTrial = new MenuItem("Create a Library Trial", Sales3Module.SubModuleCreateNewTrial.ToString(CultureInfo.InvariantCulture), "~/images/_.gif", url + Sales3Module.SubModuleManageAccountInfo.ToString(CultureInfo.InvariantCulture));

            masterPage.SubMenu.Items.Add(itemRegTrial);
            masterPage.SubMenu.Items.Add(itemLibraryTrial);
                
            itemRegTrial.Selected = true;

        }

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
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales3Module.ModuleCreateNewTrial, subModule))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                CreateTrailMainPnl.Visible = false;
            }
            return hasAccess;
        }

        #endregion
    }
}