using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Logs;
using System.Globalization;
using B24.Sales3.UserControl;

namespace B24.Sales3.UserControl
{
    /// <summary>
    /// To display the Subscription information 
    /// </summary>
    public partial class InfoDetails : System.Web.UI.UserControl
    {
        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        #endregion

        #region Public Property
        /// <summary>
        /// To get or Set the subscription
        /// </summary>
        public Subscription Subscription { get; set; }
        /// <summary>
        /// To get or Set the userid of the user
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// to get or set the login of the user
        /// </summary>
        public string Login { get; set; }
        #endregion

        #region Protected Methods
        /// <summary>
        /// PAge load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadSubscriptionData();
                    if (Subscription.Status.ToString().ToLower() != "active")
                    {
                        SubscriptionInfoTable.BgColor = "#eeeeee";
                    }
                }
                catch (Exception ex)
                {
                    InfoDetailsErrorLabel.Text = Resources.Resource.Error;
                    InfoDetailsErrorLabel.Visible = true;
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Null Value", ex);
                }
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Load the subscription data
        /// </summary>
        private void LoadSubscriptionData()
        {
            UserFactory userFactory = new UserFactory(global.UserConnStr);
            User getUser = userFactory.GetUserByID(UserId);

            User getuserRoyalityInfo = new User();
            getuserRoyalityInfo = userFactory.GetDownloadRoyaltyInfo(getUser);


            SubIdLabel.Text = Subscription.PasswordRoot;
            CompanyDepartmentLabel.Text = Subscription.CompanyName + "/" + Subscription.Department;
            ApplicationLabel.Text = Subscription.ApplicationName;
            SeatsLabel.Text = Subscription.Seats.ToString(CultureInfo.InvariantCulture);
            RegisteredUsersLabel.Text = Subscription.RegisteredUsers.ToString(CultureInfo.InvariantCulture);
            SalesGroupLabel.Text = Subscription.ResellerCode;
            StatusLabel.Text = Subscription.Status.ToString();
            StartsLabel.Text = Subscription.Starts.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            ExpiresLabel.Text = Subscription.Expires.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            if (getuserRoyalityInfo.ChapterPDFDownloadLimit > 0)
            {
                ChapterToGoLabel.Text = "Enabled (" + getuserRoyalityInfo.ChapterPDFDownloadLimit.ToString(CultureInfo.InvariantCulture) + " per 90 days)";
            }
            GroupCodeLabel.Text = Subscription.GroupCode;
            TypeLabel.Text = Subscription.Type.ToString();
            ContractLabel.Text = Subscription.AccountNumber;
            SalesPersonLabel.Text = Subscription.SalesPerson;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// To update expire date when trail extended
        /// </summary>
        public void UpdateData()
        {
            //To get updated expire date when extend the trail period. 
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
            Subscription = subscriptionFactory.GetSubscriptionByID(Subscription.SubscriptionID);
            //ExpiresLabel.Text = Subscription.Expires.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            LoadSubscriptionData();
            if (Subscription.Status.ToString().ToLower() != "active")
            {
                SubscriptionInfoTable.BgColor = "#eeeeee";
            }
        }

        #endregion
    }
}
