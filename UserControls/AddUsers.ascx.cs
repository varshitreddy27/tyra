using System;
using System.Collections.ObjectModel;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Globalization;
using System.Data.SqlClient;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// To add the users in the subscription
    /// </summary>
    public partial class AddUsers : System.Web.UI.UserControl
    {

        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.Sales3);
        MasterDataFactory masterDataFactory;
        BasePage baseObject;

        /// <summary>
        /// to store the collectionstring
        /// </summary>
        private string collectionString;
        /// <summary>
        /// to keep all pendingregistrationid of the subscription
        /// </summary>
        private string PendingRegistrationIdString ="";
        /// <summary>
        /// usercount value
        /// </summary>
        private int userCount = 5;
        /// <summary>
        /// To store access Level
        /// </summary>
        private int level;
        /// <summary>
        /// to keep access to update
        /// </summary>
        private bool allowAddUser;
        /// <summary>
        /// to store the PendingRegistrationId
        /// </summary>
        private Guid pendingRegistrationId;
        /// <summary>
        /// to check whether the textbox feilds are empty
        /// </summary>
        private bool fieldsNotEmpty;
        /// <summary>
        /// to keep the access
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// sales assigned-collection is locked unless in sales or email context
        /// </summary>
        private bool collectionLocked;
        /// <summary>
        /// condition to check some condition to check the checkbox list item
        /// </summary>
        private bool toCheckTheBox;
        /// <summary>
        /// the collectionstring of the subscription collection
        /// </summary>
        private string collectionstr;
        /// <summary>
        /// check whether collectionstr contains collection name 
        /// </summary>
        private bool hasCollectionName;
        
        #endregion

        #region Public Property
        /// <summary>
        /// To get or set the Subscription
        /// </summary>
        public Subscription Subscription { get; set; }
        /// <summary>
        /// To get or set the UserID of the user
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// To get or set the Login of the user
        /// </summary>
        public string Login { get; set; }
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

        #region Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            baseObject = this.Page as Sales4.UI.BasePage;
            if (!IsPostBack)
            {
                InitializeControls();
            }
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Give access to support guys 
        /// </summary>
        /// <returns> no value</returns>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                //if (allowAddUser == true)
                {
                    RegisterUsers();
                }
                AddUserErrorLabel.Text = Resources.Resource.UsersAdded;
                AddUserErrorLabel.Visible = true;
            }
            catch (NullReferenceException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                AddUserErrorLabel.Text = Resources.Resource.Error;
                AddUserErrorLabel.Visible = true;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                AddUserErrorLabel.Text = Resources.Resource.UsersNotAdded + "(" + ex.Message +")";
                AddUserErrorLabel.Visible = true;
            }
            ClearFields();
            InitializeControls();
            Multiview.ActiveViewIndex = 1;
            // To update the subscription details.
            UpdateInfo(null, null);
            InfoDetailsUpdatePanel.Update();  

        }

        /// <summary>
        /// Change the view to edit mode
        /// </summary>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            Multiview.ActiveViewIndex = 0;
            AddUserErrorLabel.Text = string.Empty;
        }

        /// <summary>
        /// Cancel the edit view and show the read only view
        /// </summary>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            Multiview.ActiveViewIndex = 1;
            AddUserErrorLabel.Text = string.Empty;
        }
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initialize page controls
        /// </summary>
        private void InitializeControls()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
            masterDataFactory = new MasterDataFactory(baseObject.UserConnStr);

            try
            {
                int seatCount = Subscription.Seats;
                int seatsUsed = subscriptionFactory.GetSeatsUsed(Subscription.SubscriptionID);

                // check for paid subscription
                if (CheckIsPaidSub())
                {
                    int availableSeats = ((seatCount - seatsUsed) > 0) ? seatCount - seatsUsed : 0;
                    userCount = (availableSeats <= 5) ? availableSeats : 5;
                }

                // give access to the support guys
                CheckHasAccess();

                // Calculate the level
                CalculateLevel();

                // To check whether the sales group contains 'MSVL' , hide adduser form for microsoftref
                Collection<MasterData> salesGroupUsers = masterDataFactory.GetSalesGroupsForUser(UserId);

                bool showAddUsers = CheckShowAddUsers(salesGroupUsers);

                if (level > 0 && showAddUsers)
                {
                    DisplayAddUser();
                }
                EditButton.Visible = EditButtonView;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                AddUserErrorLabel.Text = Resources.Resource.UserLoadError;
                AddUserErrorLabel.Visible = true;
            }

        }

        /// <summary>
        /// To Check to show the add user form.
        /// </summary>
        /// <param name="salesGroupUsers"></param>
        /// <returns></returns>
        private bool CheckShowAddUsers(Collection<MasterData> salesGroupUsers)
        {
            bool returnVar = true;
            if (salesGroupUsers.Count > 0)
            {
                for (int loopCounter = 0; loopCounter < salesGroupUsers.Count; loopCounter++)
                {
                    MasterData salesGroup = salesGroupUsers[loopCounter];
                    if (salesGroup.ResellerCode.ToUpper() == "MSVL2")
                    {
                        returnVar = false;
                        break;
                    }
                }
            }
            return returnVar;
        }

        /// <summary>
        /// To check whether Subscription Type is Paid or PaidPending
        /// </summary>
        /// <returns></returns>
        private bool CheckIsPaidSub()
        {
            if (Subscription.Type == SubscriptionType.Paid || Subscription.Type == SubscriptionType.PaidPending)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Check the Permission 
        /// </summary>
        private void CheckHasAccess()
        {

            PermissionFactory permFactory = new PermissionFactory(baseObject.UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(UserId, baseObject.User.Identity.Name, String.Empty);
            if (permissions.SuperUser == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1)
            {
                hasAccess = true;
            }
            else
            {
                hasAccess = false;
            }
        }
        /// <summary>
        /// calculate the level.
        /// </summary>
        private void CalculateLevel()
        {
            bool skillportsub;
            skillportsub = (Subscription.ApplicationName.ToLower() == "skillport" || Subscription.ApplicationName.ToLower() == "smartforce") ? true : false;
            if (skillportsub)
                level = 0;
            else
                level = 2;
        }

        /// <summary>
        ///  to add the pendingregistration and AddUsers form
        /// </summary>
        private void DisplayAddUser()
        {
            // retrieve any pending registrations
            PendingRegistrations();

            allowAddUser = true;

        }

        /// <summary>
        /// This method will bind the collections in the check box list
        /// </summary>
        private void BindCollections()
        {

            CollectionFactory collectionFactory = new CollectionFactory(baseObject.UserConnStr);
            List<Collection> collectionList = collectionFactory.GetCollections(Subscription.SubscriptionID, null);// collectionFactory.GetCollectionsForUser(Subscription.SubscriptionID, UserId, String.Empty, null, Collection.CollectionType.All);
            CollectionsCheckBoxList.DataSource = collectionList;
            CollectionsCheckBoxList.DataTextField = "Description";
            CollectionsCheckBoxList.DataValueField = "CollectionID";
            CollectionsCheckBoxList.DataBind();

            Dictionary<Guid, string> collcetionNames = new Dictionary<Guid, string>();
            Dictionary<Guid, int> collcetionDefault = new Dictionary<Guid, int>();

            int collectionListCount = 1;
            foreach (Collection collection in collectionList)
            {               
                collcetionNames.Add(collection.CollectionID, collection.Name);
                collcetionDefault.Add(collection.CollectionID, collection.IsDefault);

                if (collectionListCount < collectionList.Count)
                {
                    collectionString += collection.Name + ",";
                    collectionListCount++;
                }
                else
                {
                    collectionString += collection.Name;
                }             
            }

            string collectionstr = DefaultCollectionStr(collectionList, true, false);

            // check the box based on condition
            foreach (ListItem item in CollectionsCheckBoxList.Items)
            {
                item.Selected = false;
                item.Enabled = true;
                hasCollectionName = false;
                collectionLocked = false;
                toCheckTheBox = false;

                int isDefault = collcetionDefault[new Guid(item.Value)];
                string collName = collcetionNames[new Guid(item.Value)];
                if (isDefault == 1)
                {
                    hasCollectionName = true;
                    collectionLocked = true;
                }

                if (collectionstr.Contains(collName + ","))
                {
                    hasCollectionName = true;
                }
                else
                {
                    hasCollectionName = false;
                }

                if (hasCollectionName && isDefault == -1)                 // sales assigned-collection is locked unless in sales or email context.
                {
                    collectionLocked = true;
                }
                
                toCheckTheBox = (hasCollectionName || (collectionLocked && isDefault == 1));
                if (hasCollectionName || (isDefault == 1 && collectionLocked))
                {
                    item.Selected = true;
                }
                if (collectionLocked && (toCheckTheBox || Convert.ToInt16(item.Value) < 2))
                {
                    item.Enabled = false;
                }
            }            
        }

        /// <summary>
        /// This function will calculate the "default" collection string
        /// based on the settings of the various collections
        /// </summary>
        /// <param name="colls"></param>
        /// <param name="dopreview"></param>
        /// <param name="doall"></param>
        /// <returns></returns>
        private string DefaultCollectionStr(List<Collection> collections, bool dopreview, bool doall)
        {
            string collectionstr = "";

            if (collections != null && collections.Count > 0)
            {
                for (int collectionCount = 0; collectionCount < collections.Count; collectionCount++)
                {
                    Collection collection = collections[collectionCount];
                    if (collection.IsDefault == 1)
                        collectionstr += "," + collection.Name;
                    else if (dopreview && (collection.IsDefault == 2 || collection.IsDefault == 3))
                        collectionstr += "," + collection.Name;
                    else if (doall)
                        collectionstr += "," + collection.Name;
                }
            }
            if (collectionstr.IndexOf(",", StringComparison.OrdinalIgnoreCase) == 0)
                collectionstr = collectionstr.Substring(1);

            return collectionstr;
        }
        /// <summary>
        ///  Display the pending registration for the subscription if any
        /// </summary>
        private void PendingRegistrations()
        {
            PendingRegistrationFactory pendingRegistrationFactory = new PendingRegistrationFactory(baseObject.UserConnStr);
            Collection<PendingRegistration> pendingRegistrationCollection = pendingRegistrationFactory.GetPendingRegistrations(Subscription.SubscriptionID, UserId, Guid.Empty);
            if (pendingRegistrationCollection.Count > 0)
            {
                PendingRegistrationGridView.DataSource = pendingRegistrationCollection;
                PendingRegistrationGridView.DataBind();
                PendingRegistrationReadGridView.DataSource = pendingRegistrationCollection;
                PendingRegistrationReadGridView.DataBind();
                PendingRegistrationsLabel.Visible = true;
            }

            int noDirectAccess = (Subscription != null) ? Subscription.PreAuthenticated : 0;

            if (noDirectAccess == 1) //No direct access user
            {
                NodirectAccessLabel.Visible = true;
            }
            else
            {
                EditButton.Visible = true;
                if (userCount > 0)
                {
                    AddUserHelpTextLabel.Visible = true;
                    AddUserHelpText2Label.Visible = true;

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
                    if (this.level > 1 && this.hasAccess)
                    {
                        UpdateButton.Visible = true;
                    }
                    else
                    {
                        B24AsistanceLabel.Visible = true;
                    }
                }
                else
                {
                    NoSeatsLabel.Visible = true;
                }
            }
        }
        /// <summary>
        /// Register the user(s)
        /// </summary>
        private void RegisterUsers()
        {
            // Get selected collection string
            CollectionFactory collectionFactory = new CollectionFactory(baseObject.UserConnStr);
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
            PendingRegistrationFactory pendingRegistrationFactory = new PendingRegistrationFactory(baseObject.UserConnStr);
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
                PasswordTextBox1.Text, 0, SendWelcomeEmailCheckBox.Checked, String.Empty, Guid.Empty, String.Empty,
                String.Empty, collectionString, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, UserId, pendingreg);
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
                    PasswordTextBox2.Text, 0, SendWelcomeEmailCheckBox.Checked, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, UserId, pendingreg);
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
                    PasswordTextBox3.Text, 0, SendWelcomeEmailCheckBox.Checked, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, UserId, pendingreg);
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
                    PasswordTextBox4.Text, 0, SendWelcomeEmailCheckBox.Checked, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, UserId, pendingreg);
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
                    PasswordTextBox5.Text, 0, SendWelcomeEmailCheckBox.Checked, String.Empty, Guid.Empty, String.Empty,
                    String.Empty, collectionsSelected, 0, String.Empty);
                pendingRegistrationFactory.PutPendingRegistration(Subscription.SubscriptionID, UserId, pendingreg);
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
            string login, string email, string password, int useCookie, bool sendWelcomeEmail, string costCenter, Guid preReqstID, string fixedUserName,
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
            pendingRegistration.SendEmail = (sendWelcomeEmail) ? 1 : 0;
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
        /// to clear the values in text boxes
        /// </summary>
        private void ClearFields()
        {
            int counter = 1;
            TextBox FirstNameTextBox = (TextBox)FindControl("FirstNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox LastNameTextBox = (TextBox)FindControl("LastNameTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox EmailTextBox = (TextBox)FindControl("EmailTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox LoginTextBox = (TextBox)FindControl("LoginTextBox" + counter.ToString(CultureInfo.InvariantCulture));
            TextBox PasswordTextBox = (TextBox)FindControl("PasswordTextBox" + counter.ToString(CultureInfo.InvariantCulture));

            for (counter = 1; counter <= 5; counter++)
            {
                FirstNameTextBox.Text = string.Empty;
                LastNameTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                LoginTextBox.Text = string.Empty;
                PasswordTextBox.Text = string.Empty;
            }
        }

        #endregion Private Methods
    }
}