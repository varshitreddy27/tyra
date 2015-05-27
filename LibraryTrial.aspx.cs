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
    public partial class LibraryTrail : BasePage
    {

        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);

        #region Private Members

        private MasterDataFactory masterDataFactory;
        private BasePage basePage;

        /// <summary>
        /// Base master page object to access base value
        /// </summary>
        private BaseMaster masterPage;

        #endregion Private Members

        #region Protected Methods

        /// <summary>
        /// Initialize and bind the values on page load
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verify credentials for Library User         
            try
            {
                // Check the user has access
                //if (!CheckAccess())
                //{
                //    return;
                //}
                basePage = this.Page as BasePage;
                masterPage = this.Master as BaseMaster;

                if (basePage == null)
                {
                    return;
                }
            }
            catch (NullReferenceException ex)
            {

                logger.Log(Logger.LogLevel.Error, Resources.Resource.NullReference, ex);
                this.basePage.B24Errors.Add(new B24Error(Resources.Resource.ErrorSales3));
            }

            //if ((basePage.State["salesgroup"] == null)|| 
            //    ((basePage.State["salesgroup"]!=null)&&((!basePage.State["salesgroup"].ToString().Equals(ConfigurationSettings.AppSettings["LibraryTrialSalesGroup"].ToString())))))
            //    { 
            //        Response.Redirect("Error.aspx"); 
            //    }                

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
        #endregion Protected Methods

        #region Private Methods

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

        }

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
                subscriptionFactory.PutLibraryTrial(subscription);
            }
            catch (Exception)
            {
                throw;
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

            itemLibraryTrial.Selected = true;
        }

        /// <summary>
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales3Module.ModuleCreateNewTrial, 0))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                TrialPanel.Visible = false;
            }
            return hasAccess;
        }
        #endregion Private Methods
    }
}
