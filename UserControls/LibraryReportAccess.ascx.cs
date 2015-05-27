using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;

namespace B24.Sales3.UserControl
{
    public partial class LibraryReportAccess : System.Web.UI.UserControl
    {
        #region Public Properties
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Private members
        string subId = string.Empty;
        SubscriptionFactory subFactory;               // Factory for manipulating subs
        Subscription s = null;

        private ReportAccessFactory reportAccessFactory;
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);
        BasePage basePage = null;
        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as BasePage;

            InitializeVariables();
            if (!IsPostBack)
            {
                try
                {
                    //bind the retrieved values to checkbox list
                    reportAccessFactory = new ReportAccessFactory(basePage.UserConnStr);
                    Collection<ReportAccess> reportCategoryList = reportAccessFactory.GetReportCategoriesForLibrary(basePage.User.UserID);
                    if (null != reportCategoryList && reportCategoryList.Count > 0)
                    {
                        BindReportCategoriesToCheckBoxes(reportCategoryList);
                    }
                    else
                    {
                        LibraryReportAccessErrorLabel.Text = "No library reports available, submit a PSG request";
                        LibraryReportAccessErrorLabel.Visible = true;
                        SubmitButton.Enabled = false;
                    }
                    MultiView.ActiveViewIndex = 0;
                }
                catch (Exception ex)
                {
                    //display an error message in ErrorLabel box
                    logger.Log(Logger.LogLevel.Error, Resources.Resource.BindValues, ex);
                    basePage.B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.BindValues));
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                //save the selected report categories to the users
                SaveReportCategories();
                if (basePage.B24Errors.Count <= 0)
                {
                    basePage.State.Add("errmsg", Resources.Resource.UsersAdded);
                    LibraryReportAccessErrorLabel.Text = Resources.Resource.UsersAdded;
                    LibraryReportAccessErrorLabel.Visible = true;
                    UpdateInfo(null, null);
                    InfoDetailsUpdatePanel.Update();
                    MultiView.ActiveViewIndex = 0;
                }
                else
                {
                    MultiView.ActiveViewIndex = 1;
                }
                

            }
            catch (Exception ex)
            {
                //display an error message in Error Label box
                logger.Log(Logger.LogLevel.Error, Resources.Resource.UsersNotAdded, ex);
                basePage.B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.UsersNotAdded));
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetFormContent();
            MultiView.ActiveViewIndex = 0;
            LibraryReportAccessErrorLabel.Text = string.Empty;
        }

        protected void AddreportingUsersButton_Click(object sender, EventArgs e)
        {
            ResetFormContent();
            LibraryReportAccessErrorLabel.Text = string.Empty;
            MultiView.ActiveViewIndex = 1;
        }
        #endregion Protected Methods

        #region Private Methods

        private void ResetFormContent()
        {
            FirstNameTextBox1.Text = string.Empty;
            LastNameTextBox1.Text = string.Empty;
            EmailTextBox1.Text = string.Empty;
            LoginTextBox1.Text = string.Empty;
            PasswordTextBox1.Text = string.Empty;

            FirstNameTextBox2.Text = string.Empty;
            LastNameTextBox2.Text = string.Empty;
            EmailTextBox2.Text = string.Empty;
            LoginTextBox2.Text = string.Empty;
            PasswordTextBox2.Text = string.Empty;

            FirstNameTextBox3.Text = string.Empty;
            LastNameTextBox3.Text = string.Empty;
            EmailTextBox3.Text = string.Empty;
            LoginTextBox3.Text = string.Empty;
            PasswordTextBox3.Text = string.Empty;

            FirstNameTextBox4.Text = string.Empty;
            LastNameTextBox4.Text = string.Empty;
            EmailTextBox4.Text = string.Empty;
            LoginTextBox4.Text = string.Empty;
            PasswordTextBox4.Text = string.Empty;

            FirstNameTextBox5.Text = string.Empty;
            LastNameTextBox5.Text = string.Empty;
            EmailTextBox5.Text = string.Empty;
            LoginTextBox5.Text = string.Empty;
            PasswordTextBox5.Text = string.Empty;

            foreach (ListItem item in ReportCategoryCheckBoxList.Items)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                }
            }
        }
        /// <summary>
        /// This method will bind the report categories to checkbox list
        /// </summary>
        private void BindReportCategoriesToCheckBoxes(Collection<ReportAccess> reportCategoryList)
        {
            //  reportAccessFactory = new ReportAccessFactory(UserConnStr);
            //   Collection<ReportAccess> reportCategoryList = reportAccessFactory.GetReportCategoriesForLibrary(User.UserID);
            ReportCategoryCheckBoxList.DataSource = reportCategoryList;
            ReportCategoryCheckBoxList.DataTextField = "ReportCategory";
            ReportCategoryCheckBoxList.DataValueField = "ReportCategoryId";
            ReportCategoryCheckBoxList.DataBind();
        }

        /// <summary>
        /// Create User and assign report categories to that user.
        /// </summary>
        private void SaveReportCategories()
        {
            Guid newUserId = Guid.Empty;

            //check if the report categories are not selected
            String reportCategoriesId = SelectedReportCategoriesId();
            if (string.IsNullOrEmpty(reportCategoriesId.Trim()))
            {
                //display an error message in ErrorLabel box
                basePage.B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.SelectCategory));
                LibraryReportAccessErrorLabel.Text = Resources.Resource.SelectCategory;
                LibraryReportAccessErrorLabel.Visible = true;
                return;
            }

            UserFactory userFactory = userFactory = new UserFactory(basePage.UserConnStr);
            reportAccessFactory = new ReportAccessFactory(basePage.UserConnStr);
            Guid subidGuid = new Guid(subId);
            if (!String.IsNullOrEmpty(FirstNameTextBox1.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox1.Text.Trim()) && !String.IsNullOrEmpty(EmailTextBox1.Text.Trim()) && !String.IsNullOrEmpty(PasswordTextBox1.Text.Trim()))
            {
                //create a user-1

                newUserId = userFactory.RegisterUser(subidGuid, FirstNameTextBox1.Text, LastNameTextBox1.Text, LoginTextBox1.Text, EmailTextBox1.Text, PasswordTextBox1.Text, Guid.Empty, basePage.User.UserID);

                //store a user with assigned report categories
                reportAccessFactory.PutReportCategories(basePage.User.UserID, newUserId, reportCategoriesId);
                newUserId = Guid.Empty;
            }

            if (!String.IsNullOrEmpty(FirstNameTextBox2.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox2.Text.Trim()) && !String.IsNullOrEmpty(EmailTextBox2.Text.Trim()) && !String.IsNullOrEmpty(PasswordTextBox2.Text.Trim()))
            {
                //create a user-2
                newUserId = userFactory.RegisterUser(subidGuid, FirstNameTextBox2.Text, LastNameTextBox2.Text, LoginTextBox2.Text, EmailTextBox2.Text, PasswordTextBox2.Text, Guid.Empty, basePage.User.UserID);

                //store a user with assigned report categories
                reportAccessFactory.PutReportCategories(basePage.User.UserID, newUserId, reportCategoriesId);
                newUserId = Guid.Empty;
            }

            if (!String.IsNullOrEmpty(FirstNameTextBox3.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox3.Text.Trim()) && !String.IsNullOrEmpty(EmailTextBox3.Text.Trim()) && !String.IsNullOrEmpty(PasswordTextBox3.Text.Trim()))
            {
                //create a user-3
                newUserId = userFactory.RegisterUser(subidGuid, FirstNameTextBox3.Text, LastNameTextBox3.Text, LoginTextBox3.Text, EmailTextBox3.Text, PasswordTextBox3.Text, Guid.Empty, basePage.User.UserID);

                //store a user with assigned report categories
                reportAccessFactory.PutReportCategories(basePage.User.UserID, newUserId, reportCategoriesId);
                newUserId = Guid.Empty;
            }

            if (!String.IsNullOrEmpty(FirstNameTextBox4.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox4.Text.Trim()) && !String.IsNullOrEmpty(EmailTextBox4.Text.Trim()) && !String.IsNullOrEmpty(PasswordTextBox4.Text.Trim()))
            {
                //create a user-4
                newUserId = userFactory.RegisterUser(subidGuid, FirstNameTextBox4.Text, LastNameTextBox4.Text, LoginTextBox4.Text, EmailTextBox4.Text, PasswordTextBox4.Text, Guid.Empty, basePage.User.UserID);

                //store a user with assigned report categories
                reportAccessFactory.PutReportCategories(basePage.User.UserID, newUserId, reportCategoriesId);
                newUserId = Guid.Empty;
            }
            if (!String.IsNullOrEmpty(FirstNameTextBox5.Text.Trim()) && !String.IsNullOrEmpty(LastNameTextBox5.Text.Trim()) && !String.IsNullOrEmpty(EmailTextBox5.Text.Trim()) && !String.IsNullOrEmpty(PasswordTextBox5.Text.Trim()))
            {
                //create a user-5
                newUserId = userFactory.RegisterUser(subidGuid, FirstNameTextBox5.Text, LastNameTextBox5.Text, LoginTextBox5.Text, EmailTextBox5.Text, PasswordTextBox5.Text, Guid.Empty, basePage.User.UserID);

                //store a user with assigned report categories
                reportAccessFactory.PutReportCategories(basePage.User.UserID, newUserId, reportCategoriesId);
                newUserId = Guid.Empty;
            }
        }

        /// <summary>
        /// concatenate the selected report category(s) IDs as a string to assign to the users 
        /// </summary>
        /// <returns>string</returns>
        private string SelectedReportCategoriesId()
        {
            String selectedReportCategories = "";
            foreach (ListItem item in ReportCategoryCheckBoxList.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(selectedReportCategories))
                    {
                        selectedReportCategories += ",";
                    }
                    selectedReportCategories += item.Value;
                }
            }

            return selectedReportCategories;
        }
        private void InitializeVariables()
        {
            if (null != Request.QueryString["subid"] && !String.IsNullOrEmpty(Request.QueryString["subid"]))
            {
                subId = Request.QueryString["subid"];
            }
            if (null != Request.QueryString["arg"] && !String.IsNullOrEmpty(Request.QueryString["arg"]))
            {
                subId = Request.QueryString["arg"];
            }
            GlobalVariables gv = GlobalVariables.GetInstance();
            // Initialize the factories
            subFactory = new SubscriptionFactory(gv.UserConnStr);
            if (String.IsNullOrEmpty(subId) || !ValidateSub(new Guid(subId)))
            {
                LibraryReportAccessErrorLabel.Text = "Subscription is not valid library sub";
                LibraryReportAccessErrorLabel.Visible = true;
                SubmitButton.Enabled = false;

            }
            else
            {
                SubmitButton.Enabled = true;
            }
        }

        /// <summary>
        /// Verify that the target subscription is valid (e.g., is b24library)
        /// </summary>
        /// <param name="subID">The subid to verify</param>
        /// <returns>True if target is valid; false otherwise</returns>
        private bool ValidateSub(Guid subID)
        {
            s = subFactory.GetSubscriptionByID(subID);
            bool isValid = false;
            isValid = s.ApplicationName.ToLower().Contains("library") && !s.ApplicationName.ToLower().Contains("b24library");

            return isValid;
        }

        #endregion Private Methods
        
}
}
