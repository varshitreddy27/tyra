using System;
using B24.Common;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace B24.Sales3.UserControl
{
    public partial class SubSnapShot : System.Web.UI.UserControl
    {

        #region Private Variable

        private GlobalVariables global = GlobalVariables.GetInstance();
        private Logger logger = new Logger(Logger.LoggerType.Sales3);
        private UserFactory userFactory;
        private User user;

        /// <summary>
        /// check condition to show form
        /// </summary>
        private bool hasAccess;
        /// <summary>
        /// Calculate level to update 
        /// </summary>
        private int level;

        #endregion

        #region Public Property

        /// <summary>    
        /// User(Admin) making change request
        /// </summary>
        public Guid RequestorId { get; set; }

        /// <summary>
        /// User whose permission we want to change
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///  Subscription Details
        /// </summary>
        public Subscription Subscription { get; set; }
        #endregion

        #region Events


        /// <summary>
        /// Page load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadSubscriptionData();

                }
                catch (IndexOutOfRangeException ex)
                {
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Index Out Of Range Exception", ex);
                //    ManageAccountErrorLabel.Text = Resources.Resource.Error;
                //    ManageAccountErrorLabel.Visible = true;
                }
                catch (NullReferenceException ex)
                {
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                    //ManageAccountErrorLabel.Text = Resources.Resource.Error;
                    //ManageAccountErrorLabel.Visible = true;
                }

            } 
        }

        #endregion

        #region private Methods

        /// <summary>
        /// check the access and level to show the form
        /// </summary>
        //private void CalculateLevel()
        //{
        //    if (user.ApplicationName.ToLower(CultureInfo.InvariantCulture) != "skillport")
        //    {
        //        hasAccess = true;
        //        level = 2;
        //    }
        //    else
        //    {
        //        hasAccess = false;
        //        level = 0;
        //    }
        //}

    

        /// <summary>
        /// Get the existing subscription data
        /// </summary>
        private void LoadSubscriptionData()
        {
            SubIDReadLbl.Text = Subscription.PasswordRoot;
            CompanyReadLbl.Text = Subscription.CompanyName;
            ApplicationReadLbl.Text = Subscription.ApplicationName;
            ContractReadLbl.Text = Subscription.ContactID.ToString();
            SeatsReadLbl.Text = Subscription.Seats.ToString();
            RegUsersReadLbl.Text = Subscription.RegisteredUsers.ToString();
            SalesGroupReadLbl.Text = Subscription.ResellerCode;
            SalesPersReadLbl.Text = Subscription.SalesPerson;

            TypeReadLbl.Text = Subscription.Type.ToString();
            StatusReadLbl.Text = Subscription.Status.ToString();
            StartsReadLbl.Text = Subscription.Starts.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture); 
            if (Subscription.Expires != DateTime.MinValue)
            {
                ExpiresReadLbl.Text = Subscription.Expires.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture); 
            }
            //ChaptersToGoReadLbl.Text = Subscription.


        }

        #endregion
        
        #region Public Method
       
        #endregion
    }
}
