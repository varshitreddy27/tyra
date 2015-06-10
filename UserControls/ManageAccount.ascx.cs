using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Web.UI.WebControls;
using B24.Sales3.UI;

namespace B24.Sales3.UserControl
{
    public partial class ManageAccount : System.Web.UI.UserControl
    {
        #region Events
        public event EventHandler<EventArgs> GroupCodeChanged;
        #endregion

        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        B24.Common.Web.BasePage baseObject;
        int cellNum = -1;
        bool reportLinks = false;
        #endregion

        #region Public Properties
        public bool EditBtnview { get; set; }
        public Subscription Subscription { get; set; }
        public Guid UserId { get; set; }
        public string Login { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handle Page load events
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                baseObject = this.Page as B24.Common.Web.BasePage;
                try
                {
                    LoadApplication();
                    LoadSubscriptionTypes();
                    LoadSalesGroup();                   
                    LoadSubscriptionData();
                    LoadSalesPerson();
                   

                }
                catch (IndexOutOfRangeException ex)
                {
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Index Out Of Range Exception", ex);
                    ManageAccountErrorLabel.Text = Resources.Resource.Error;
                    ManageAccountErrorLabel.Visible = true;
                }
                catch (NullReferenceException ex)
                {
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                    ManageAccountErrorLabel.Text = Resources.Resource.Error;
                    ManageAccountErrorLabel.Visible = true;
                }

            }
            LoadSubscriptionDetailInfo();
            EnableEcommerceCheckBox();
            Multiview.ActiveViewIndex = 1;

            EditButton.Visible = EditBtnview;


            if (Subscription.RegisteredUsers > 0)
            {
                GroupCodeTextBox.Enabled = false;
            }

        }

        protected void detailsGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (cellNum > -1)
                {
                    if (reportLinks)
                    {
                        string subId = (e.Row.DataItem as DataRowView).Row["@subscriptionid"].ToString();   

                        HyperLink manageAccountHyperLink = new HyperLink();
                        manageAccountHyperLink.Text = "Manage Account";
                        manageAccountHyperLink.CssClass = "GoButton";
                        manageAccountHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts + "&submodule=" + Sales3Module.SubModuleSettings + "&arg=" + subId;
                        e.Row.Cells[cellNum].Controls.Add(manageAccountHyperLink);

                        e.Row.Cells[cellNum].Controls.Add(new LiteralControl("<BR>"));

                        HyperLink reportsHyperLink = new HyperLink();
                        reportsHyperLink.Text = "Reports";
                        reportsHyperLink.CssClass = "GoButton";
                        reportsHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts + "&submodule=" + Sales3Module.SubModuleReports + "&arg=" + subId;

                        e.Row.Cells[cellNum].Controls.Add(reportsHyperLink);
                    }
                    else
                    {
                        string endUserID = (e.Row.DataItem as DataRowView).Row["@enduserid"].ToString();
                        string name = (e.Row.DataItem as DataRowView).Row["Name"].ToString();

                        LiteralControl lt = new LiteralControl(addStyles(name));
                        e.Row.Cells[0].Controls.Add(lt);

                        HyperLink UserInfoHyperLink = new HyperLink();
                        UserInfoHyperLink.Text = "User Info";
                        UserInfoHyperLink.CssClass = "GoButton";
                        UserInfoHyperLink.NavigateUrl = "~/ManageUser.aspx?module=" + Sales3Module.ModuleManageUsers + "&submodule=" + Sales3Module.SubModuleManageUserInfo + "&arg=" + endUserID;

                        e.Row.Cells[cellNum].Controls.Add(UserInfoHyperLink);

                        e.Row.Cells[cellNum].Controls.Add(new LiteralControl("<BR>"));

                        HyperLink ManageUserHyperLink = new HyperLink();
                        ManageUserHyperLink.Text = "Manage User";
                        ManageUserHyperLink.CssClass = "GoButton";
                        ManageUserHyperLink.NavigateUrl = "~/ManageUser.aspx?module=" + Sales3Module.ModuleManageUsers + "&submodule=" + Sales3Module.SubModuleEmailSettings + "&arg=" + endUserID;
                        e.Row.Cells[cellNum].Controls.Add(ManageUserHyperLink);
                    }
                }
            }
        }

        /// <summary>
        /// Update Subscription details
        /// </summary>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidSubscription())
                {
                    if (!String.IsNullOrEmpty(CompanyTextBox.Text)) Subscription.CompanyName = CompanyTextBox.Text.Trim();
                    if (!String.IsNullOrEmpty(SubIDLblRead.Text)) Subscription.PasswordRoot = SubIDLblRead.Text.Trim();
                    if (!String.IsNullOrEmpty(ContractNumTextBox.Text)) Subscription.AccountNumber = ContractNumTextBox.Text.Trim();
                    if (!String.IsNullOrEmpty(GroupCodeTextBox.Text)) Subscription.GroupCode = GroupCodeTextBox.Text.Trim();
                    if (!String.IsNullOrEmpty(BuyBookURLTextBox.Text)) { Subscription.BuyBookURL = BuyBookURLTextBox.Text.Trim(); }
                    if (!String.IsNullOrEmpty(DepartmentTextBox.Text)) { Subscription.Department = DepartmentTextBox.Text.Trim(); }
                    Subscription.Expires = DateTime.Parse(EndDateTextBox.Text.Trim(), CultureInfo.InvariantCulture);

                    if (!String.IsNullOrEmpty(SeatsTextBox.Text))
                    {
                        int seatsValue = Convert.ToInt32(SeatsTextBox.Text, CultureInfo.InvariantCulture);
                        Subscription.Seats = seatsValue;
                    }

                    if (!String.IsNullOrEmpty(RegisteredUsersTextBox.Text))
                    {
                        int regUsersValue = Convert.ToInt32(RegisteredUsersTextBox.Text, CultureInfo.InvariantCulture);
                        Subscription.RegisteredUsers = regUsersValue;
                    }

                    if (DisabledCheckBox.Checked)
                    {
                        Subscription.Status = SubscriptionStatus.Inactive;
                    }
                    else
                    {
                        if (Subscription.Status == SubscriptionStatus.Inactive)
                        {
                            Subscription.Status = SubscriptionStatus.Active;
                        }
                    }
                    if (SeatOverflowCheckBox.Checked)
                    {
                        Subscription.AllowSeatOverflow = 1;
                    }
                    else
                    {
                        Subscription.AllowSeatOverflow = 0;
                    }
                    Subscription.ApplicationName = ApplicationDropDown.SelectedValue.ToString(CultureInfo.InvariantCulture);
                    int downloadLimit = Convert.ToInt16(SetCTGLimitTextBox.Text, CultureInfo.InvariantCulture);
                    Subscription.Type = Subscription.SubscriptionTypeFromInt(Convert.ToInt32(TypeDropDown.SelectedValue, CultureInfo.InvariantCulture));

                    //Updated company details
                    CompanyFactory companyFactory = new CompanyFactory(baseObject.UserConnStr);
                    Company company = new Company();
                    company = companyFactory.GetCompany(Subscription.CompanyID, Login);
                    company.CompanyName = Subscription.CompanyName;
                    companyFactory.PutCompany(company, Login);

                    //update subscription details                   
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
                    subscriptionFactory.PutSubscription(Subscription, UserId, Login);
                    subscriptionFactory.PutDownloadRoyalityInfo(downloadLimit, Subscription.SubscriptionID);


                    //update sales


                    PermissionFactory permFactory = new PermissionFactory(baseObject.UserConnStr);
                    Permission permissions = permFactory.LoadPermissionsById(UserId, baseObject.User.Identity.Name, String.Empty);

                    // permission for changing sales person and sales group and Ecomm
                    Guid salesPersonId = Subscription.SalesPersonID;
                    if (permissions.SuperUser == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1)
                    {
                        Subscription.ResellerCode = SalesGroupDropDown.SelectedValue.ToString();
                        if (SalesPersonDropDown.SelectedValue != Guid.Empty.ToString())
                        {
                            Subscription.SalesPersonID = new Guid(SalesPersonDropDown.SelectedValue.ToString());                                          }
                        subscriptionFactory.SalesReassign(Subscription, UserId);
                        if (EcommerceCheckBox.Checked)
                        {
                            subscriptionFactory.ECommify(Subscription, UserId);
                        }
                        else
                        {
                            subscriptionFactory.DeCommify(Subscription, UserId);
                        }
                        ManageAccountErrorLabel.Text = Resources.Resource.SubSuccess;
                        ManageAccountErrorLabel.Visible = true;
                    }
                    else
                    {
                        ManageAccountErrorLabel.Text = Resources.Resource.SubSuccess + ". But Doesn't have Permission to reassign Sales";
                        ManageAccountErrorLabel.Visible = true;
                    }
                }
                OnUpdategroupCode();

                // Reload the values
                SubscriptionFactory subFactory = new SubscriptionFactory(baseObject.UserConnStr);
                Subscription = subFactory.GetSubscriptionByID(Subscription.SubscriptionID);

                LoadApplication();
                LoadSubscriptionTypes();
                LoadSalesGroup();
                LoadSubscriptionData();
                LoadSalesPerson();

                // Change the view to readonly mode
                Multiview.ActiveViewIndex = 1;
            }
            catch (SqlException ex)
            {
                ManageAccountErrorLabel.Text = Resources.Resource.SubFail;
                ManageAccountErrorLabel.Visible = true;
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Subscription Failed ", ex);
            }
        }
        /// <summary>
        /// change the view to edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            ManageAccountErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 0;
        }
        /// <summary>
        /// Cancel the edit view and show the read only view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            ManageAccountErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        #endregion

        #region PublicMethods
        public void UpdatedGroupCode()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
            Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(Subscription.SubscriptionID);
            GroupCodeTextBox.Text = manageSubscription.GroupCode;
        }

        #endregion

        #region Private Methods

        //Method to show or hide the header title text
        private void ShowHeaderText(bool value)
        {
            HeaderText.Visible = value;
        }

        //Method to show or hide the subscription detail
        private void ShowSubscriptionExtraDetails(bool value)
        {
            HeaderText.Visible = value;
        } 

        /// <summary>
        /// Get the application list to fill Application drop down
        /// </summary>
        private void LoadApplication()
        {
            ApplicationFactory applicationFactory = new ApplicationFactory(baseObject.UserConnStr);
            ApplicationDropDown.DataSource = applicationFactory.GetAllApplicationForUser(UserId);
            ApplicationDropDown.DataTextField = "Name";
            ApplicationDropDown.DataValueField = "Name";
            ApplicationDropDown.DataBind();
        }
        private void OnUpdategroupCode()
        {
            if (GroupCodeChanged != null)
            {
                GroupCodeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Get the subscription types list to fill SubscriptionTypes drop down
        /// </summary>
        private void LoadSubscriptionTypes()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
            TypeDropDown.DataSource = subscriptionFactory.GetSubscriptionTypes();
            TypeDropDown.DataValueField = "SubTypeId";
            TypeDropDown.DataTextField = "SubType";
            TypeDropDown.DataBind();
        }

        /// <summary>
        /// Get the salesgroup list to fill the salesGroup dropdownList
        /// </summary>
        private void LoadSalesGroup()
        {
            MasterDataFactory masterDataFactory = new MasterDataFactory(baseObject.UserConnStr);
            Collection<B24.Common.MasterData> salesGroupList = masterDataFactory.GetSalesGroupsForUser(UserId);
            Collection<B24.Common.MasterData> groupList = new Collection<B24.Common.MasterData>();
            MasterData masterdata = new MasterData();
            masterdata.SalesGroupName= "";
            masterdata.ResellerCode = null;
            groupList.Add(masterdata);
            
            foreach (MasterData salesGroup in salesGroupList)
            {
                if (salesGroup.SalesGroupType > 0)
                {
                    groupList.Add(salesGroup);
                }
            }
            SalesGroupDropDown.DataSource = groupList;
            SalesGroupDropDown.DataTextField = "salesgroupName";
            SalesGroupDropDown.DataValueField = "resellerCode";
            SalesGroupDropDown.DataBind();
        }

        /// <summary>
        /// Get subscription details. Dynamically build tables to show data
        /// </summary>
        private void LoadSubscriptionDetailInfo()
        {
            DataSet subDetailDS = new DataSet();
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
            subscriptionFactory.SubscriptionID = Subscription.SubscriptionID;
            subDetailDS = subscriptionFactory.GetSubscriptionDetailInfo(UserId);
            DataTable dt = new DataTable();
            string tableName = "";
            for (int i = 0; i < subDetailDS.Tables.Count; i++)
            {
                dt = subDetailDS.Tables[i];
                if (dt.Rows.Count == 1 && dt.Columns.Count == 1) // must be a title table
                {
                    tableName = dt.Columns[0].ColumnName;
                    if (dt.Columns[0].ColumnName.Equals("Column1", StringComparison.OrdinalIgnoreCase)) // Must be Account Info table - ignore this and next tables
                        i++;
                    else
                    {
                        // Create and add table name
                        DetailsTablePH.Controls.Add(new LiteralControl("<BR>"));
                        System.Web.UI.WebControls.Label tableTitleLabel = new System.Web.UI.WebControls.Label();
                        tableTitleLabel.Text = tableName;
                        tableTitleLabel.CssClass = "b24-doc-subtitle";
                        DetailsTablePH.Controls.Add(tableTitleLabel);
                        DetailsTablePH.Controls.Add(new LiteralControl("<BR>"));
                    }
                }
                else
                {

                    // Now create a gridview with a table
                    GridView detailsGV = new GridView();
                    detailsGV.CssClass = "resultGrid";
                    detailsGV.CellSpacing = 0;
                    detailsGV.CellPadding = 4;
                    detailsGV.AutoGenerateColumns = false;
                    detailsGV.RowDataBound += detailsGV_RowDataBound;
                        //gdv.RowDataBound += gdv_RowDataBound
                    foreach (DataColumn column in dt.Columns)
                    { 
                        if (column.ColumnName != "HiddenColumn")
                        {
                            BoundField field = new BoundField();
                            field.DataField = column.ColumnName;
                            if (!column.ColumnName.Equals("Column1"))
                                field.HeaderText = column.ColumnName;
                            field.HtmlEncode = false;
                            if (column.ColumnName.StartsWith("@"))
                                field.Visible = false;
                            detailsGV.Columns.Add(field);
                        }
                    }

                    if (tableName.Equals("Admin Users", StringComparison.InvariantCulture) || (tableName.Equals("Registered Users", StringComparison.InvariantCulture)))
                    {
                        if ((tableName.Equals("Registered Users", StringComparison.InvariantCulture) && (dt.Rows.Count == 1 && dt.Columns.Count > 1) && dt.Rows[0].ItemArray[0].ToString().Contains("run the Registered")))
                         // too many users to show - provide links to run a report
                            reportLinks = true;
                        else
                            reportLinks = false;

                        TemplateField tfield = new TemplateField();
                        detailsGV.Columns.Add(tfield);
                        cellNum = dt.Columns.Count;       
                    }
                    else // reset
                    {
                        cellNum = -1; 
                        reportLinks = false;
                    }
                    detailsGV.DataSource = dt;
                    detailsGV.DataBind();                  

                    DetailsTablePH.Controls.Add(detailsGV);
                    DetailsTablePH.Controls.Add(new LiteralControl("<BR>"));
                }
            }

        }


        /// <summary>
        /// Get the sales person list to fill the salesperson dropdownList
        /// </summary>
        private void LoadSalesPerson()
        {
            MasterDataFactory masterDataFactory = new MasterDataFactory(baseObject.UserConnStr);
            Collection<B24.Common.MasterData> salesPersonList = masterDataFactory.GetSalesColleagues(UserId);
            Collection<B24.Common.MasterData> personList = new Collection<B24.Common.MasterData>();
            MasterData masterdata = new MasterData();
            masterdata.FullName = "";
            masterdata.UserId = Guid.Empty;
            personList.Add(masterdata);

            foreach (MasterData salesPerson in salesPersonList)
            {
                personList.Add(salesPerson);
            }
            SalesPersonDropDown.DataSource = personList;
            SalesPersonDropDown.DataTextField = "fullName";
            SalesPersonDropDown.DataValueField = "userId";
            SalesPersonDropDown.DataBind();
            int index = 0;
            int itemCount = SalesPersonDropDown.Items.Count;

            for (int i = 0; i < itemCount; i++)
            {
                if (SalesPersonDropDown.Items[i].Value == Subscription.SalesPersonID.ToString())
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            { SalesPersonDropDown.SelectedIndex = index; }
        }

        /// <summary>
        /// Ecommerce checkbox enabaling procedure
        /// </summary>
        private void EnableEcommerceCheckBox()
        {
            bool isInternalUser = false;  // true if user's email address is like @books24x7 or @skillsoft    

            UserFactory userFactory = new UserFactory(baseObject.UserConnStr);
            User user = userFactory.GetUserByID(UserId);
            if (null != user && null != user.Email && 
                (user.Email.IndexOf("books24x7") > 0 ||
                user.Email.IndexOf("skillsoft") > 0))
            {
                isInternalUser = true;
            }

            bool isecomm = (Subscription.DurationTypeID > 0);
            
            //--- check for ugrade privs ----
            bool hasupgrade = false;
            MasterDataFactory masterDataFactory = new MasterDataFactory(baseObject.UserConnStr);
            Collection<B24.Common.MasterData> salesgroups = masterDataFactory.GetSalesGroupsForUser(UserId);
             
            foreach (MasterData salesGroup in salesgroups)
            {
                if (salesGroup.SalesGroupName.ToLower() == "upgrade")
                {
                    hasupgrade = true;
                    break;
                }
            }

            if (!hasupgrade || !isInternalUser || !isecomm)
            {
                EcommerceCheckBox.Visible = false;
                EcommerceReadCheckBox.Visible = false;
            }
            else
            {
                EcommerceCheckBox.Visible = true;
                EcommerceReadCheckBox.Visible = true;
            }
        }

        /// <summary>
        /// Get the existing subscription data
        /// </summary>
        private void LoadSubscriptionData()
        {
            int index = 0;
            CompanyTextBox.Text = Subscription.CompanyName;
            SubIDLblRead.Text = Subscription.PasswordRoot;
            ContractNumTextBox.Text = Subscription.AccountNumber;
            SeatsTextBox.Text = Subscription.Seats.ToString(CultureInfo.InvariantCulture);
            RegisteredUsersTextBox.Text = Subscription.RegisteredUsers.ToString(CultureInfo.InvariantCulture);
            DepartmentTextBox.Text = Subscription.Department;
            GroupCodeTextBox.Text = Subscription.GroupCode;
            BuyBookURLTextBox.Text = Subscription.BuyBookURL;
            StartDateLblRead.Text = Subscription.Starts.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            if (Subscription.Expires != DateTime.MinValue)
            {
                EndDateTextBox.Text = Subscription.Expires.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
                EndDateReadLabel.Text = Subscription.Expires.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture); // for Readonly view
            }
            DisabledCheckBox.Checked = Subscription.Status == SubscriptionStatus.Inactive ? true : false;
            SeatOverflowCheckBox.Checked = Subscription.AllowSeatOverflow == 1 ? true : false;
            EcommerceCheckBox.Checked = Subscription.DurationTypeID > 0 ? true : false;
            SalespersonLabel.Text = Subscription.SalesPerson;
            //index = ddlApplication.Items.IndexOf(ddlApplication.Items.FindByValue(Subscription.ApplicationName));
            // Find by Value function input data is case sencitive i.e it treats b24 and B24 as different items, as a workaround we have to loop through
            int itemCount = ApplicationDropDown.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                if (ApplicationDropDown.Items[i].Value.ToLower() == Subscription.ApplicationName.ToLower())
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            { ApplicationDropDown.SelectedIndex = index; }
            //else
            //{
            //    ManageAccountErrorLabel.Text = Resources.Resource.AppName;
            //    ManageAccountErrorLabel.Visible = true;
            //}

            index = TypeDropDown.Items.IndexOf(TypeDropDown.Items.FindByText(Subscription.Type.ToString()));
            if (index >= 0)
            {
                TypeDropDown.SelectedIndex = index;
            }
            else
            {
                ManageAccountErrorLabel.Text = Resources.Resource.SubType;
                ManageAccountErrorLabel.Visible = true;
            }

            try
            {
                if (Subscription.ResellerCode != null)
                {
                    int salesGroupItemCount = SalesGroupDropDown.Items.Count;
                    for (int i = 1; i < salesGroupItemCount; i++)
                    {
                        if (SalesGroupDropDown.Items[i].Value.ToLower() == Subscription.ResellerCode.ToLower())
                        {
                            index = i;
                            break;
                        }
                    }
                }
                else
                {
                    index = 0;
                }
                SalesGroupDropDown.SelectedIndex = index;
                
            }
            catch (NullReferenceException nullException)
            {
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "No resellercode is found ", nullException);
            }

            UserFactory userFactory = new UserFactory(baseObject.UserConnStr);
            User getUser = userFactory.GetUserByID(UserId);
            User getuserRoyalityInfo = new User();
            getuserRoyalityInfo = userFactory.GetDownloadRoyaltyInfo(getUser);
            SetCTGLimitTextBox.Text = getuserRoyalityInfo.ChapterPDFDownloadLimit.ToString(CultureInfo.InvariantCulture);

            // ReadOnly data

            CompanyReadLabel.Text = Subscription.CompanyName;
            SubIDLbl2Read.Text = Subscription.PasswordRoot;
            ContractNumLbl2Read.Text = Subscription.AccountNumber;
            StartDateLbl2Read.Text = Subscription.Starts.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            SeatsLbl2Read.Text = Subscription.Seats.ToString(CultureInfo.InvariantCulture);
            RegisteredUsersLbl2Read.Text = Subscription.RegisteredUsers.ToString(CultureInfo.InvariantCulture);
            DepartmentReadLabel.Text = Subscription.Department;
            ApplicationReadLabel.Text = Subscription.ApplicationName;
            GroupCodeReadLabel.Text = Subscription.GroupCode;
            TypeReadLabel.Text = Subscription.Type.ToString();
            BuyBookUrlReadLabel.Text = Subscription.BuyBookURL;
            CTGLimitReadLabel.Text = SetCTGLimitTextBox.Text;
            SalesGroupReadLabel.Text = Subscription.ResellerCode;
            SalespersonReadLabel.Text = Subscription.SalesPerson;
            SeatOverflowReadCheckBox.Checked = Subscription.AllowSeatOverflow == 1 ? true : false;
            DisabledReadCheckBox.Checked = Subscription.Status == SubscriptionStatus.Inactive ? true : false;
            EcommerceReadCheckBox.Checked = Subscription.DurationTypeID > 0 ? true : false;

            if (!String.IsNullOrEmpty(Subscription.Notes))
            {
                NotesLbl.Visible = true;
                NotesContentLbl.Visible = true;
                NotesContentLbl.Text = Subscription.Notes;
            }
        }

        /// <summary>
        /// mandatory  check for  company and department name  
        /// </summary>
        private bool IsValidSubscription()
        {
            if (string.IsNullOrEmpty(CompanyTextBox.Text))
            {
                ManageAccountErrorLabel.Text = Resources.Resource.CompanyName;
                ManageAccountErrorLabel.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(EndDateTextBox.Text))
            {
                ManageAccountErrorLabel.Text = Resources.Resource.EndDate;
                ManageAccountErrorLabel.Visible = true;
                return false;
            }
            if (!CheckDate(EndDateTextBox.Text))
            {
                ManageAccountErrorLabel.Text = Resources.Resource.EndDateValid;
                ManageAccountErrorLabel.Visible = true;
                return false;
            }

            DateTime endDate = DateTime.Parse(EndDateTextBox.Text,CultureInfo.InvariantCulture);
            return true;
        }

        /// <summary>
        ///  Add style to the name column content  
        /// </summary>
        private string addStyles(string name)
        {
            string enhancedName = "";
            string[] separators = { "<BR/>", "<br/>", "<BR />", "<br />" };
            string[] result = name.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (result.Length > 0)
                enhancedName += "<b><span class='b24-subscriptionitem-DataBind'>" + result[0] + "</span></b><BR/>";
            if (result.Length > 1)
                enhancedName += "<span class='user-email'>" + result[1] + "</span><BR/>";
            if (result.Length > 2)
                enhancedName += "<span class='user-login'>" + result[2] + "</span><BR/>";
            return enhancedName;
        }

        /// <summary>
        ///  check for valid date  
        /// </summary>
        private bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date,CultureInfo.InstalledUICulture);
                return true;
            }
            catch (ArgumentNullException argNullException)
            {
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Subscription End Date Format ", argNullException);
                return false;
            }
            catch (FormatException fromatException)
            {
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Subscription End Date Format ", fromatException);
                return false;
            }
        }
        #endregion
    }
}