using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// To Add Subscription Notes
    /// </summary>
    public partial class SubscriptionNotes : System.Web.UI.UserControl
    {
        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        Sales4.UI.BasePage basePage;
        #endregion

        #region Public Property
        /// <summary>
        /// To get or set the subscription object
        /// </summary>
        public Subscription Subscription { get; set; }
        /// <summary>
        /// to get or set the userid of the user
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// to get or set the login name of the user
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        #endregion

        #region Protected Methods
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
            basePage = this.Page as Sales4.UI.BasePage;
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadSubscriptionNotes();
                }

                catch (Exception ex)
                {
                    SubscriptionNotesErrorLabel.Text = Resources.Resource.Error;
                    SubscriptionNotesErrorLabel.Visible = true;
                    Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                    logger.Log(Logger.LogLevel.Error, "Null Value", ex);
                }
            }
            EnableAddButton();
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Update Subscription details
        /// </summary>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                User getUser = userFactory.GetUserByID(UserId);

                if (SubscriptionNotesTextBox.Text != String.Empty)
                {
                    Subscription.Notes += "<br/> On " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " " + getUser.FirstName + " " + getUser.LastName + " " + "Wrote: " + SubscriptionNotesTextBox.Text;
                }

                //update subscription details                   
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
                subscriptionFactory.PutSubscription(Subscription, UserId, Login);

                // Reload the values
                LoadSubscriptionNotes();

                // Change the view to readonly mode
                Multiview.ActiveViewIndex = 1;

                // Clear the text in the textbox
                SubscriptionNotesTextBox.Text = "";
            }
            catch (SqlException ex)
            {
                SubscriptionNotesErrorLabel.Text = Resources.Resource.SubFail;
                SubscriptionNotesErrorLabel.Visible = true;
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Subscription Failed ", ex);
            }

        }
        /// <summary>
        /// change the view to edit mode
        /// </summary>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            SubscriptionNotesErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 0;
        }

        /// <summary>
        /// Cancel the edit view and show the read only view
        /// </summary>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            SubscriptionNotesErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Method to show or hide the header title text
        /// </summary>
        /// <param name="value"></param>
        private void ShowHeaderText(bool value)
        {
            HeaderText.Visible = value;
        }

        /// <summary>
        /// Load the subscription Notes
        /// </summary>
        private void LoadSubscriptionNotes()
        {

            SubscriptionNotesLabel.Text = Subscription.Notes;
            SubscriptionNotesReadLabel.Text = Subscription.Notes;

            EditButton.Visible = EditButtonView;
        }

        /// <summary>
        /// Show button to add notes 
        /// </summary>
        public void EnableAddButton()
        {
            
            PermissionFactory permFactory = new PermissionFactory(basePage.UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(UserId, basePage.User.Identity.Name, String.Empty);

            if (permissions.SalesMarketing == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1 || permissions.SuperUser == 1)
            {
                AddButton.Visible = true;
            }
            else
            {
                AddButton.Visible = false;
            }
        }
        #endregion
    }
}