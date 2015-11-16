using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Data;
using System.Globalization; 

namespace B24.Sales3.UserControl
{
    public partial class UpdateAccount : System.Web.UI.UserControl
    {
        Logger logger = new Logger(Logger.LoggerType.UserInfo);
        
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
        public bool UpdateAccountReadView { get; set; }
        /// <summary>
        /// Subscription object
        /// </summary>        
        public Subscription Subscription { get; set; }
        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        #endregion

        #region Private Variables

        /// <summary>
        /// To keep access 
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// To store access Level 
        /// </summary>
        private short level;
        /// <summary>
        /// User type
        /// </summary>
        private bool isInternalUser;
        /// <summary>
        /// Upgrade details
        /// </summary>
        bool hasUpgrade;
        /// <summary>
        /// Subscription type
        /// </summary>
        bool isTrial;
        /// <summary>
        ///  Is Skillport enabled.
        /// </summary>
        bool isSkillPort;
        /// <summary>
        ///  To store all sales group.
        /// </summary>
        string salesGroupString;
        /// <summary>
        ///  To Store Sales Collection List.
        /// </summary>
        List<Collection> salesCollectionList;
        /// <summary>
        ///  To Store Subscription Collection List.
        /// </summary>
        List<Collection> subCollectionList;
        /// <summary>
        ///  To keep user List details.
        /// </summary>
        List<User> userList;

        private Sales3.UI.BasePage basePage;

        #endregion

        # region private methods

        /// <summary>
        /// To Load extend Trail details.
        /// </summary>
        private void LoadUpdateAccountDetails()
        {
            CollectionFactory collectionFactory = new CollectionFactory(basePage.UserConnStr);
            int formType = 3;

            if (isTrial)
            {
                formType = 1;
            }
            else if (isSkillPort)
            {
                formType = 2;
            }

            if (level > 0)
            {

                string extCollectionStr = DefaultCollectionStr(subCollectionList, true, false);

                if (subCollectionList != null)
                {
                    extCollectionStr = "";  

                    for (int counter = 0; counter < subCollectionList.Count; counter++)
                    {
                        Collection subCollection = subCollectionList[counter];
                        if (subCollection != null)  
                        {
                            if (counter > 0) extCollectionStr += ",";    
                            if (isInternalUser)
                            {
                                extCollectionStr += ExtendedCollectionStringWithAdminSelect(subCollection, true);
                            }
                            else
                            {
                                extCollectionStr += ExtendedCollectionStringWithAdminSelect(subCollection, false);

                            }
                        }
                    }
                }

                bool enabled = (Subscription.Status == 0) ? false : true;
                if (!enabled)
                    AccountUpdateDisabledLabel.Visible = true;
                
                //To show convert trail information
                ShowConvertTrail(formType);

                //To Show Collection captions based on privilege
                // Force seat count for Microsfot eREf to be "all" since they can only sell 
                salesGroupString = GetSalesGroupString();
                string forcedSeatCount = (salesGroupString.ToUpper(CultureInfo.InvariantCulture).IndexOf("MSVL", StringComparison.OrdinalIgnoreCase) >= 0) ? "all" : null;
                DisplayCollectionCaption(formType , forcedSeatCount);

                // Create collection datasource and bind into gridview.
                CollectionCapacity(salesCollectionList, extCollectionStr, forcedSeatCount);
                
                //To create user datasource and bind the usercheckboxlist
                LoadUserDetails(formType);
                                
                if (level > 1)
                {
                    if (!isTrial)
                    { 
                        if (isSkillPort)
                        {
                            UpdateButton.Text = "Update";
                        }
                        else
                        {
                            UpdateButton.Text = "Renew";
                        }
                    }
                }
                else
                {
                    EditButton.Visible = EditButtonView; 
                    B24AsistanceLabel.Visible = true;
                    B24ContactLabel.Visible = true;
                }
            }
        }
        /// <summary>
        /// To Show the details of convert trail
        /// </summary>
        /// <param name="formType"></param>
        private void ShowConvertTrail(int formType)
        {
            if (hasUpgrade)
            {
                ConverTrialLabel.Visible = true;
                if (isTrial)
                {
                    ConvertTrailToPaidCheckBox.Visible = true;
                    PaidTransactForm.Visible = false;
                    ConvertTrialHelpLablel2.Visible = true;
                }
                else
                {
                    ConvertTrailToPaidCheckBox.Visible = false;
                    if (formType == 3)
                        PaidTransactForm.Visible = true;
                    else
                        PaidTransactForm.Visible = false;
                    ConvertTrialHelpLablel2.Visible = false;
                    ConverTrialLabel.Visible = false;
                }
                if (isInternalUser)
                {
                    ExpirationLabel.Visible = true;
                    ExpirationDateTextBox.Visible = true;
                    ExpirationDateTextBox.Text = Subscription.Expires.ToString("MMMM dd, yyyy",CultureInfo.InvariantCulture);
                    if (!isSkillPort)
                    {
                        SkillPortEnableCheckBox.Visible = true;
                    }
                }
                else
                {
                    DurationLabel.Visible = true;
                    DurationDropDownList.Visible = true;
                }

                UserSeatsTextBox.Visible = true;
                UserSeatsTextBox.Text = Subscription.Seats.ToString(CultureInfo.InvariantCulture);

            }
        }

        /// <summary>
        /// To Load User details
        /// </summary>
        /// <param name="formType"></param>
        private void LoadUserDetails(int formType)
        {
            if (hasUpgrade)
            {
                if (Subscription != null && Subscription.Seats <= 25)
                {
                    if (isInternalUser && isTrial)
                    {
                        UserForm2Label.Visible = true;
                        PaidorSkillPortUserLabel.Visible = false;
                        UserForm1Label.Visible = false;
                    }
                    else
                    {
                        if (formType != 1)
                        {
                            PaidorSkillPortUserLabel.Visible = true;
                            UserForm1Label.Visible = false;
                            UserForm2Label.Visible = false;
                        }
                        else
                        {
                            UserForm1Label.Visible = true;
                            PaidorSkillPortUserLabel.Visible = false;
                            UserForm2Label.Visible = false;
                        }


                    }
                    DataTable userListSource = new DataTable();
                    DataColumn userDataColumn;

                    userDataColumn = new DataColumn();
                    userDataColumn.DataType = Type.GetType("System.Guid");
                    userDataColumn.ColumnName = "UserID";
                    userListSource.Columns.Add(userDataColumn);

                    userDataColumn = new DataColumn();
                    userDataColumn.DataType = Type.GetType("System.String");
                    userDataColumn.ColumnName = "UserName";
                    userListSource.Columns.Add(userDataColumn);

                    for (int i = 0; i < userList.Count; i++)
                    {
                        DataRow userRow = userListSource.NewRow();
                        userRow["UserID"] = userList[i].UserID;
                        userRow["UserName"] = userList[i].FirstName + " " + userList[i].LastName;
                        userListSource.Rows.Add(userRow);
                    }

                    UserListCheckBoxList.DataSource = userListSource;
                    UserListCheckBoxList.DataTextField = "UserName";
                    UserListCheckBoxList.DataValueField = "UserID";
                    UserListCheckBoxList.DataBind();

                    //To select all the user check box
                    for (int loopCounter = 0; loopCounter < UserListCheckBoxList.Items.Count; loopCounter++)
                    {
                        UserListCheckBoxList.Items[loopCounter].Selected = true;
                    }

                }
            }
        }
        
        /// <summary>
        /// To display caption based on user privilege
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="forcedSeatCount"></param>
        private void DisplayCollectionCaption(int formType, string forcedSeatCount)
        {
            if (isInternalUser)
                {
                    if (formType == 1)
                    {
                        AdjustCollectionLabel.Visible = true;
                        RenewalLabel.Visible = false;
                        CollectionHelp3Label1.Visible = true;
                        CollectionHelp3Label2.Visible = true;
                        CollectionHelp3Label3.Visible = true;
                        CollectionHelp3Label4.Visible = true;
                        SkillPortPaidCollectionLabel.Visible = false;
                        PaidCollectionLabel.Visible = false;
                        CollectionHelp12Label1.Visible = false;
                        CollectionHelp1Label2.Visible = false;
                    }
                    else if (formType == 2)
                    {
                        AdjustCollectionLabel.Visible = true;
                        RenewalLabel.Visible = false;
                        PaidCollectionLabel.Visible = false;
                        CollectionHelp3Label1.Visible = false;
                        CollectionHelp3Label2.Visible = false;
                        CollectionHelp3Label3.Visible = false;
                        CollectionHelp3Label4.Visible = false;
                        SkillPortPaidCollectionLabel.Visible = true;
                        CollectionHelp12Label1.Visible = false;
                        CollectionHelp1Label2.Visible = false;
                    }
                    else
                    {
                        AdjustCollectionLabel.Visible = true;
                        RenewalLabel.Visible = true;
                        ConverTrialLabel.Visible = false;
                        SkillPortPaidCollectionLabel.Visible = false;
                        PaidCollectionLabel.Visible = true;
                        CollectionHelp3Label1.Visible = false;
                        CollectionHelp3Label2.Visible = false;
                        CollectionHelp3Label3.Visible = false;
                        CollectionHelp3Label4.Visible = false;
                        CollectionHelp12Label1.Visible = false;
                        CollectionHelp1Label2.Visible = false;
                    }

                }
                else
                {
                    if (forcedSeatCount == null)
                    {
                        CollectionHelp12Label1.Visible = true;
                        CollectionHelp1Label2.Visible = true;
                        CollectionHelp3Label1.Visible = false;
                        CollectionHelp3Label2.Visible = false;
                        CollectionHelp3Label3.Visible = false;
                        CollectionHelp3Label4.Visible = false;
                    }
                    else
                    {
                        CollectionHelp12Label1.Visible = true;
                        CollectionHelp1Label2.Visible = false;
                        CollectionHelp3Label1.Visible = false;
                        CollectionHelp3Label2.Visible = false;
                        CollectionHelp3Label3.Visible = false;
                        CollectionHelp3Label4.Visible = false;
                    }
                }
        }

        /// <summary>
        /// To Get Sales Group string.
        /// </summary>
        /// <returns></returns>
        private string GetSalesGroupString()
        {
            string colectionString = "";
            MasterDataFactory masterFactory = new MasterDataFactory(basePage.UserConnStr);
            System.Collections.ObjectModel.Collection<MasterData> salesGroupList = masterFactory.GetSalesGroupsForUser(UserId);
            for (int loopCounter = 0; loopCounter < salesGroupList.Count; loopCounter++)
            {
                colectionString += "," + salesGroupList[loopCounter].ResellerCode;
            }
            return colectionString;
        }

        /// <summary>
        /// Build Collection record
        /// </summary>
        /// <param name="collectionList"></param>
        /// <param name="collectionString"></param>
        /// <param name="forcedSeatCount"></param>
        private void CollectionCapacity(List<Collection> collectionList, string collectionString, string forcedSeatCount)
        {
            if (collectionList != null && collectionList.Count > 0)
            {
                // convert string value into dictionary for easy to verification
                Dictionary<string, string> collectionDictionary = new Dictionary<string, string>();
                if (collectionDictionary != null)
                {
                    string[] splitCollection = collectionString.Split(',');
                    for (int i = 0; i < splitCollection.Length; i++)
                    {
                        string collectionName = splitCollection[i].ToLower(CultureInfo.InvariantCulture);
                        if (collectionName.IndexOf(";",StringComparison.OrdinalIgnoreCase) > 0)
                            collectionName = collectionName.Substring(0, collectionName.IndexOf(";",StringComparison.OrdinalIgnoreCase));
                        if (!collectionDictionary.ContainsKey(collectionName))                       // note: only first instance of a collection name will be retained!
                            collectionDictionary.Add(collectionName, splitCollection[i]);
                    }
                }

                if (collectionList != null)
                {
                    //Create another dictionary from sales collection list for easy comparision
                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    for (int loopCounter = 0; loopCounter < collectionList.Count; loopCounter++)
                    {
                        Collection collection = collectionList[loopCounter];
                        if (collection != null)   
                        {
                            if (!dictionary.ContainsKey(collection.Description))
                                dictionary.Add(collection.Description, 1);
                            else
                            {
                                int count = dictionary[collection.Description];
                                dictionary.Remove(collection.Description);
                                dictionary.Add(collection.Description, count + 1);
                            }
                        }
                    }

                    // creating Data source to display collection details.
                    DataTable collectionTable = CreateCollectionTable(forcedSeatCount, collectionList, collectionDictionary, dictionary);

                    if (collectionTable.Rows.Count > 0)
                    {
                        CollectionGridView.DataSource = collectionTable;
                        CollectionGridView.DataBind();
                    }

                }
            }
        }

        /// <summary>
        /// Create Table for collection details
        /// </summary>
        /// <param name="forcedSeatCount"></param>
        /// <param name="collectionList"></param>
        /// <param name="collectionDictionary"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private DataTable CreateCollectionTable(string forcedSeatCount, List<Collection> collectionList, Dictionary<string, string> collectionDictionary, Dictionary<string, int> dictionary)
        {
            DataTable collectionSource = new DataTable();
            DataColumn CollectionDataColumn;
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.Int32");
            CollectionDataColumn.ColumnName = "CollectionCheckbox";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.String");
            CollectionDataColumn.ColumnName = "CollectionName";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.String");
            CollectionDataColumn.ColumnName = "NoOfUser";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.String");
            CollectionDataColumn.ColumnName = "UserLabel";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.Int32");
            CollectionDataColumn.ColumnName = "AdminCheckBox";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.String");
            CollectionDataColumn.ColumnName = "AdminSelect";
            collectionSource.Columns.Add(CollectionDataColumn);
            CollectionDataColumn = new DataColumn();
            CollectionDataColumn.DataType = Type.GetType("System.String");
            CollectionDataColumn.ColumnName = "CollName";
            collectionSource.Columns.Add(CollectionDataColumn);

            // Looping thru the Collection list to display the collection
            for (int loopCounter = 0; loopCounter < collectionList.Count; loopCounter++)
            {
                bool isChecked = false;

                Collection collection = collectionList[loopCounter];
                if (collection != null)
                {
                    string collectionname = collection.Name.ToLower(CultureInfo.InvariantCulture);

                    string[] collectionOptions = { "" };
                    if (collectionDictionary != null && collectionDictionary.ContainsKey(collectionname.ToLower(CultureInfo.InvariantCulture)))
                    {
                        isChecked = true;
                        collectionOptions = collectionDictionary[collectionname].Split(';');
                    }

                    String collectionLabel = (dictionary != null && dictionary[collection.Description] > 1) ? collection.Description + " (" + collection.Name + ")" : collection.Description;

                    DataRow rowCollection = collectionSource.NewRow();

                    rowCollection["CollName"] = collectionname;
                    rowCollection["AdminSelect"] = "Admin Selects";
                    rowCollection["UserLabel"] = "users";
                    if (isChecked && collection.Capacity == 0)
                    {
                        rowCollection["CollectionCheckbox"] = 3; // To Display label with X symbol
                        rowCollection["CollectionName"] = collectionLabel;
                    }
                    else
                    {
                        rowCollection["CollectionCheckbox"] = (isChecked) ? 1 : 2; // to show check box with select(1) or not(2).
                        rowCollection["CollectionName"] = collectionLabel;
                    }

                    string seatCount = "all";
                    if (forcedSeatCount != null)
                        seatCount = forcedSeatCount;
                    else if (collectionOptions.Length > 1)
                        seatCount = collectionOptions[1];
                    else if (collection.Capacity == -1)
                        seatCount = (isChecked) ? "all" : "";
                    else if (collection.SeatCount > 0)
                        seatCount = (isChecked) ? collection.SeatCount.ToString(CultureInfo.InvariantCulture) : "";

                    bool disableSeatCount = (forcedSeatCount != null) ? true : false;

                    rowCollection["NoOfUser"] = seatCount;

                    if (collectionOptions.Length > 2)
                    {
                        bool selectable = false;
                        if (collectionOptions[2] == "1")
                            selectable = true;
                        else if (collection.IsDefault <= 1)
                            selectable = false;
                        else if (collection.Capacity > 0)
                            selectable = true;

                        rowCollection["AdminCheckBox"] = (selectable) ? 1 : 2; // 1- checked ,2 - unchecked

                    }
                    else
                    {
                        rowCollection["AdminCheckBox"] = 3; // for place holder
                    }

                    collectionSource.Rows.Add(rowCollection);
                }
            }
            return collectionSource;
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
            if (collectionstr.IndexOf(",",StringComparison.OrdinalIgnoreCase) == 0)
                collectionstr = collectionstr.Substring(1); 

            return collectionstr;
        }

        /// <summary>
        /// Calculate the extended collectionstring from a collection object
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private string ExtendedCollectionStringWithAdminSelect(Collection collection, bool doAdminselectable)
        {
            string outString = "";
            if (collection != null)
            {
                outString += collection.Name.ToLower(CultureInfo.InvariantCulture);
                outString += (collection.IsDefault > 0 && collection.SeatCount == -1) ? ";all" : (collection.SeatCount > 0) ? ";" + collection.SeatCount.ToString(CultureInfo.InvariantCulture) : ";"; 
                if (doAdminselectable)
                    outString += (collection.IsDefault == 3) ? ";1" : ";0";
            }

            return outString;
        }

        /// <summary>
        /// To display read view.
        /// </summary>
        private void DisplayReadView()
        {
            CollectionGridView.Enabled = false;
            UserListCheckBoxList.Enabled = false;
            ExpirationDateTextBox.Enabled = false;
            SkillPortEnableCheckBox.Enabled = false;
            ConvertTrailToPaidCheckBox.Enabled = false;
            UserSeatsTextBox.Enabled = false;
            DurationDropDownList.Enabled = false;
            UpdateButton.Visible = false;
            CancelButton.Visible = false;
            EditButton.Visible = EditButtonView;
            CalenderImage.Visible = false;            
        }

        /// <summary>
        /// To Show the Editable view
        /// </summary>
        private void DisplayEditView()
        {
            CalenderImage.Visible = true;
            CollectionGridView.Enabled = true;
            UserListCheckBoxList.Enabled = true;
            ExpirationDateTextBox.Enabled = true;
            SkillPortEnableCheckBox.Enabled = true;
            ConvertTrailToPaidCheckBox.Enabled = true;
            UserSeatsTextBox.Enabled = true;
            DurationDropDownList.Enabled = true;
            UpdateButton.Visible = true;
            CancelButton.Visible = true;
            EditButton.Visible = false;
            AccountUpdateErrorLabel.Visible = false;
        }

        /// <summary>
        /// Initialize the control values
        /// </summary>
        private void InitControls()
        {
            try
            {
                Double minDaysLeft = 30;
                int maxUsers = 100;
                isTrial = (Subscription != null && Subscription.Type == 0) ? true : false;

                DateTime subexpiredate = Subscription.Expires;
                PermissionFactory permissionFactory = new PermissionFactory(basePage.UserConnStr);
                Permission permission = permissionFactory.LoadPermissionsById(UserId, basePage.User.Identity.Name, String.Empty);

                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                User user = userFactory.GetUserByID(UserId);

                MasterDataFactory masterDataFactory = new MasterDataFactory(basePage.UserConnStr);
                CollectionFactory collectionFactory = new CollectionFactory(basePage.UserConnStr);

                System.Collections.ObjectModel.Collection<MasterData> salesGroupsList = masterDataFactory.GetSalesGroupsList(UserId);
                userList = userFactory.GetColleagues(Guid.Empty, Subscription.SubscriptionID, 0, 0, "", 1);

                string targetappname = "b24library";
                bool enabled = false;
                if (Subscription != null)
                {
                    targetappname = Subscription.ApplicationName;
                    enabled = (Subscription.Status == 0) ? false : true;
                    DateTime expires = Subscription.Expires;
                    DateTime starts = Subscription.Starts;
                }

                subCollectionList = collectionFactory.GetCollectionsForUser(Subscription.SubscriptionID, Guid.Empty, targetappname, null, Collection.CollectionType.All);

                foreach (MasterData salesGroup in salesGroupsList)
                {
                    if (salesGroup.SalesGroupName.ToLower(CultureInfo.InvariantCulture) == "upgrade")
                    {
                        hasUpgrade = true;
                        break;
                    }
                }

                bool issales = ((permission != null) && (permission.SalesMarketing == 1 || permission.SalesManager == 1 || permission.GeneralAdmin == 1 || permission.SuperUser == 1)) ? true : false;

                if (issales)
                {
                    salesCollectionList = collectionFactory.GetCollections(Guid.Empty, UserId);
                }

                isSkillPort = (Subscription != null && (Subscription.ApplicationName.ToLower(CultureInfo.InvariantCulture) == "skillport" || Subscription.ApplicationName.ToLower(CultureInfo.InvariantCulture) == "smartforce")) ? true : false;
                Double daysLeft = 0;

                if (!isTrial)
                {
                    daysLeft = Subscription.Expires.Subtract(DateTime.Now.Date).TotalDays;
                }

                if (permission.SuperUser == 1 || permission.GeneralAdmin == 1 || permission.SalesManager == 1)
                {
                    hasAccess = true;
                }
                int seatCount = (Subscription != null) ? +Subscription.Seats : 0;

                if (null != user && null != user.Email && (user.Email.IndexOf("books24x7", StringComparison.OrdinalIgnoreCase) > 0 || user.Email.IndexOf("skillsoft", StringComparison.OrdinalIgnoreCase ) > 0))
                {
                    isInternalUser = true;
                }

                if (!isTrial && daysLeft > minDaysLeft)
                {
                    level = 0;
                    if (isSkillPort && hasUpgrade)
                    {
                        level = 2;
                    }
                }
                else if ((isTrial || daysLeft < minDaysLeft) && seatCount > maxUsers)
                {
                    level = 1;
                }
                else if (hasUpgrade)
                {
                    level = 2;
                }
                else
                {
                    level = 0;
                }
                EditButton.Visible = EditButtonView;
            }
            catch (SqlException sqlException)
            {
                AccountUpdateErrorLabel.Text = Resources.Resource.UpdateAccountLoadError;
                AccountUpdateErrorLabel.Visible = true;
                logger.Log(Logger.LogLevel.Error, Resources.Resource.UpdateAccountLoadError, sqlException);
                DisplayReadView();
                return;
            }

        }

        /// <summary>
        /// Calculate the collectionid for a name collection from a list
        /// </summary>
        /// <param name="salesCollectionList"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        private Guid GetCollectionID(List<Collection> salesCollList, string collectionName)
        {
            Guid collectionID = Guid.Empty;
            for (int loopCounter = 0; loopCounter < salesCollList.Count; loopCounter++)
            {
                Collection collection = salesCollList[loopCounter];
                if (collection != null && collection.Name.ToLower(CultureInfo.InvariantCulture) == collectionName)
                {

                    collectionID = collection.CollectionID;
                    break;
                }
            }

            return collectionID;
        }
        
        #endregion

        #region Events
        /// <summary>
        /// Onit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            basePage = this.Page as Sales3.UI.BasePage;
            if (!Page.IsPostBack)
            {
                try
                {
                    //Initialize the control values
                    InitControls();
                    LoadUpdateAccountDetails();
                    DisplayReadView();
                }
                catch (Exception ex)
                {
                    AccountUpdateErrorLabel.Visible = true;
                    AccountUpdateErrorLabel.Text = Resources.Resource.UserLoadError;
                    logger.Log(Logger.LogLevel.Error, "Update Account", ex);
                }
            }
        }

        /// <summary>
        /// To Show Editable view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            InitControls();
            LoadUpdateAccountDetails();
            DisplayEditView();
        }

        /// <summary>
        /// Update the Subscription details and Collection details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            InitControls();
            if (!hasAccess)
            {
                LoadUpdateAccountDetails();
                DisplayReadView();
                return;
            }

            int seats = Convert.ToInt32(UserSeatsTextBox.Text, CultureInfo.InvariantCulture);
            // if we don't have enough seats for all the users then stop right here
            if (UserListCheckBoxList.Items.Count != 0 && seats < UserListCheckBoxList.Items.Count)
            {
                AccountUpdateErrorLabel.Text = Resources.Resource.AccountUpdateSeatNotEnough;
                AccountUpdateErrorLabel.Visible = true;
                logger.Log(Logger.LogLevel.Error, Resources.Resource.AccountUpdateSeatNotEnough);
                DisplayReadView(); 
                return;
            }
            
            // Remove all the user from subscription if it is unselect
            RemoveUsers();

            // Save subscription data
            if (Subscription.SubscriptionID != null)
            {
                bool doConvert = (!hasUpgrade) ? false : (isTrial && ConvertTrailToPaidCheckBox.Checked) ? true : (!isTrial) ? true : false;

                Subscription.LastUpdatorID = UserId;
                Subscription.Status = SubscriptionStatus.Active;
                if ((doConvert || !isTrial) )
                {
                    if (isInternalUser && !String.IsNullOrEmpty(ExpirationDateTextBox.Text))
                        Subscription.Expires = Convert.ToDateTime(ExpirationDateTextBox.Text, CultureInfo.InvariantCulture);
                    else
                    {
                        DateTime date = (DateTime.Now >= Subscription.Expires) ? DateTime.Now : Subscription.Expires;
                        Subscription.Expires = date.AddYears(1);
                    }
                }
                if (doConvert)
                {
                    Subscription.Type = SubscriptionType.Paid;
                    Subscription.Seats = seats;
                }
                if (isInternalUser && SkillPortEnableCheckBox.Checked && (doConvert || !isTrial))
                {
                    Subscription.ApplicationName = "skillport";
                    Subscription.GroupCode = Subscription.PasswordRoot;
                }
                
                try
                {
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                    subscriptionFactory.PutSubscription(Subscription, UserId, Login);
                    if (Subscription.ApplicationName.ToLower(CultureInfo.InvariantCulture) == "skillport")
                    {
                        SkillPortEnableCheckBox.Visible = false; 
                    }
                }
                catch (SqlException sqlException)
                {
                    AccountUpdateErrorLabel.Text = Resources.Resource.SubFail;
                    AccountUpdateErrorLabel.Visible = true;
                    logger.Log(Logger.LogLevel.Error, Resources.Resource.SubFail, sqlException);
                    DisplayReadView(); 
                    return;
                }
            }

            // ADJUST COLLECTIONS
            
            string[] collectionString = new string[salesCollectionList.Count];
            if (CollectionGridView.Rows.Count > 0)
            {
                for (int loopCounter = 0; loopCounter < CollectionGridView.Rows.Count; loopCounter++)
                {
                    CheckBox collectionCheckBox = (CheckBox)CollectionGridView.Rows[loopCounter].FindControl("CollectionCheckbox");
                    TextBox noOfUserTextBox = (TextBox)CollectionGridView.Rows[loopCounter].FindControl("NoOfUser");
                    // To get collection name
                    Label collecionNameLabel = (Label)CollectionGridView.Rows[loopCounter].FindControl("CollName");
                    CheckBox adminSelectCheckBox = (CheckBox)CollectionGridView.Rows[loopCounter].FindControl("AdminCheckBox");

                    if (collectionCheckBox.Checked && collectionCheckBox.Visible)
                    {
                        collectionString[loopCounter] = collecionNameLabel.Text;
                        if (!String.IsNullOrEmpty(noOfUserTextBox.Text))
                            collectionString[loopCounter] += ";" + noOfUserTextBox.Text;
                        else
                            collectionString[loopCounter] += ";all";

                        if (isInternalUser)// add the admin selectable
                        {
                            if (adminSelectCheckBox != null)
                            {
                                if (adminSelectCheckBox.Visible)
                                {
                                    if (adminSelectCheckBox.Checked)
                                        collectionString[loopCounter] += ";1";
                                    else
                                        collectionString[loopCounter] += ";2";
                                }
                                else
                                    collectionString[loopCounter] += ";0";
                            }
                            else
                            {
                                collectionString[loopCounter] += ";0";
                            }
                        }
                    }
                }
            }
            
            // Remove Collection from subscription collections
            RemoveCollections(collectionString);

            //Add Collection to subscription collections
            AddCollections(collectionString);
            
            // to show in read mode
            InitControls();
            LoadUpdateAccountDetails();
            DisplayReadView();
            AccountUpdateErrorLabel.Visible = true;
            AccountUpdateErrorLabel.Text = Resources.Resource.SubSuccess;
        }

        /// <summary>
        /// To remove user from subscription who are unseleceted
        /// </summary>
        private void RemoveUsers()
        {
            Dictionary<Guid, string> userDictionary = new Dictionary<Guid, string>();
            if (UserListCheckBoxList.Items.Count > 0)
            {
                for (int loopCounter = 0; loopCounter < UserListCheckBoxList.Items.Count; loopCounter++)
                {
                    ListItem listItem = UserListCheckBoxList.Items[loopCounter];
                    if (listItem.Selected)
                    {
                        Guid userID = new Guid(listItem.Value);
                        if (!userDictionary.ContainsKey(userID))
                            userDictionary.Add(userID, listItem.Text);
                    }
                }

                // Remove all the user from subscription if it is unselect
                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                List<User> subscriptionUserList = userFactory.GetColleagues(Guid.Empty, Subscription.SubscriptionID, 0, 0, "", 1);

                if (subscriptionUserList != null)
                {
                    if (userDictionary.Count > 0)
                    {
                        for (int loopCounter = 0; loopCounter < subscriptionUserList.Count; loopCounter++)
                        {
                            User user = subscriptionUserList[loopCounter];
                            if (!userDictionary.ContainsKey(user.UserID)) 
                            {
                                try
                                {
                                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                                    subscriptionFactory.UnAssignUser(Subscription.SubscriptionID,user.UserID, UserId);
                                }
                                catch (SqlException sqlException)
                                {
                                    AccountUpdateErrorLabel.Text = Resources.Resource.AccountUpdateUnAssignUserError;
                                    AccountUpdateErrorLabel.Visible = true;
                                    logger.Log(Logger.LogLevel.Error, Resources.Resource.AccountUpdateUnAssignUserError, sqlException);
                                    DisplayReadView(); 
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To Add collections to subscription collections
        /// </summary>
        /// <param name="collectionString"></param>
        private void AddCollections(string[] collectionString)
        {
            // Calculate the collections that need to be added
            if (collectionString.Length > 0)
            {
                Dictionary<string, string> collectionDictionary = new Dictionary<string, string>();
                //--- construct dictionary of existing collection and entitlements ---
                if (subCollectionList.Count > 0)   // if we have collections
                {
                    for (int loopCounter = 0; loopCounter < subCollectionList.Count; loopCounter++)
                    {
                        Collection subCollection = subCollectionList[loopCounter];
                        if (subCollection != null)
                        {
                            string extendedCollectionString = "";
                            if (isInternalUser)
                            {
                                extendedCollectionString = ExtendedCollectionStringWithAdminSelect(subCollection, true);
                            }
                            else
                            {
                                extendedCollectionString = ExtendedCollectionStringWithAdminSelect(subCollection, false);
                            }
                            if (!String.IsNullOrEmpty(extendedCollectionString) && !collectionDictionary.ContainsKey(extendedCollectionString))
                            {
                                collectionDictionary.Add(extendedCollectionString, extendedCollectionString.ToLower(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
                //--- loop through form data and look for differences ---
                for (var i = 0; i < collectionString.Length; i++)
                {
                    if (collectionString[i] != null)
                    {
                        if (!collectionDictionary.ContainsKey(collectionString[i].ToLower(CultureInfo.InvariantCulture)))  // check if already there.
                        {
                            string[] extendCollectionString = collectionString[i].Split(';');
                            string collectionName = (extendCollectionString.Length > 0) ? extendCollectionString[0] : "";
                            // this will be set for admin selectable so, isdefault = 3 if collectionString[2]==1
                            int isdefault = (extendCollectionString.Length > 2 && extendCollectionString[2] == "1" && extendCollectionString[1].ToLower(CultureInfo.InvariantCulture) != "all") ? 3 : (extendCollectionString.Length > 1 && extendCollectionString[1].ToLower(CultureInfo.InvariantCulture) == "all") ? 1 : -1;
                            int noOfSeats = -1;
                            if (extendCollectionString.Length > 1)
                            {
                                if (extendCollectionString[1] != "all")
                                {
                                    noOfSeats = Convert.ToInt32(extendCollectionString[1],CultureInfo.InvariantCulture);
                                }
                            }

                            Guid collectionId = GetCollectionID(salesCollectionList, collectionName);
                            if (collectionId != Guid.Empty || collectionName.ToLower(CultureInfo.InvariantCulture) == "default")
                            {
                                CollectionFactory collectionFactory = new CollectionFactory(basePage.UserConnStr);
                                Collection subCollection = new Collection();
                                subCollection.Name = collectionName;
                                subCollection.CollectionID = collectionId;
                                subCollection.IsDefault = isdefault;
                                subCollection.SeatCount = noOfSeats;
                                try
                                {
                                    collectionFactory.PutCollection(subCollection, Subscription.SubscriptionID);
                                }
                                catch (SqlException sqlException)
                                {
                                    AccountUpdateErrorLabel.Text = Resources.Resource.CollectionNotAdded;
                                    AccountUpdateErrorLabel.Visible = true;
                                    logger.Log(Logger.LogLevel.Error, Resources.Resource.CollectionNotAdded, sqlException);
                                    DisplayReadView();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// To remove collection from subscription collections
        /// </summary>
        private void RemoveCollections(string[] collectionString)
        {
            if (subCollectionList.Count > 0 && collectionString.Length > 0)
            {
                Dictionary<Guid, Guid> collectionDictinary = new Dictionary<Guid, Guid>();
                //Create Dictionary for easy checking purpose
                for (int i = 0; i < collectionString.Length; i++)
                    if (collectionString[i] != null)
                    {
                        string[] extendCollectionString = collectionString[i].Split(';');
                        string collectionName = (extendCollectionString.Length > 0) ? (extendCollectionString[0]).ToLower(CultureInfo.InvariantCulture) : "";
                        Guid collectionId = GetCollectionID(salesCollectionList, collectionName);
                        if ((collectionId != Guid.Empty || collectionName.ToLower(CultureInfo.InvariantCulture) == "default") && !collectionDictinary.ContainsKey(collectionId))
                            collectionDictinary.Add(collectionId, collectionId);
                    }
                //Delete collection from subscription collection if it is not selected on collection list
                if (subCollectionList.Count > 0)
                    for (int loopCounter = 0; loopCounter < subCollectionList.Count; loopCounter++)
                    {
                        Collection subCollection = subCollectionList[loopCounter];
                        if (subCollection != null)
                        {
                            if (!collectionDictinary.ContainsKey(subCollection.CollectionID))
                            {
                                try
                                {
                                    CollectionFactory collFac = new CollectionFactory(basePage.UserConnStr);
                                    collFac.DeleteCollection(subCollection.CollectionID, Subscription.SubscriptionID);
                                }
                                catch (SqlException sqlException)
                                {
                                    AccountUpdateErrorLabel.Text = Resources.Resource.DeleteCollectionFail;
                                    AccountUpdateErrorLabel.Visible = true;
                                    logger.Log(Logger.LogLevel.Error, Resources.Resource.DeleteCollectionFail, sqlException);
                                    DisplayReadView();
                                    return;
                                }
                                catch (ArgumentNullException argumentNullException)
                                {
                                    AccountUpdateErrorLabel.Text = Resources.Resource.DeleteCollectionFail;
                                    AccountUpdateErrorLabel.Visible = true;
                                    logger.Log(Logger.LogLevel.Error, Resources.Resource.DeleteCollectionFail, argumentNullException);
                                    DisplayReadView();
                                    return;
                                }

                            }
                        }
                    }
            }
        }

        /// <summary>
        /// To Adjust collection display details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CollectionGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // To hide the collection name field
                e.Row.Cells[6].Visible = false; 

                TextBox textBox = (TextBox)e.Row.FindControl("NoOfUser");
                textBox.Width = 50;

                CheckBox adminCheckBox = (CheckBox)e.Row.FindControl("AdminSelect");
                Label adminSelectLabel = (Label)e.Row.FindControl("AdminSelectLabel");

                CheckBox chkBox = (CheckBox)e.Row.FindControl("CollectionCheckBox");
                
                // To show check box for collection name
                if (chkBox.Text == "3")
                {
                    chkBox.Visible = false;
                    e.Row.Cells[0].Text = "X ";
                }
                else if (chkBox.Text == "1" || chkBox.Text == "2")
                {

                    if (chkBox.Text == "1")
                    {
                        chkBox.Checked = true;
                    }
                    else
                    {
                        chkBox.Checked = false;
                    }
                    chkBox.Text = "";
                }
                // To show check box for Admin select 
                if (adminCheckBox.Text == "3")
                {
                    adminCheckBox.Visible = false;
                    adminSelectLabel.Visible = false;
                }
                else if (adminCheckBox.Text == "1" || adminCheckBox.Text == "2")
                {
                    if (adminCheckBox.Text == "1")
                        adminCheckBox.Checked = true;
                    else
                        adminCheckBox.Checked = false;
                    adminCheckBox.Text = "";
                }
            }
        }

        /// <summary>
        /// Cancel the Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            InitControls();
            LoadUpdateAccountDetails();
            DisplayReadView();
        }
        #endregion
    }
}
