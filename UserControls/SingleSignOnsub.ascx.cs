using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;

namespace B24.Sales3.UserControl
{
    /// <summary>
    ///  To update Single sign on subscription
    /// </summary>
    public partial class SingleSignOnsub : System.Web.UI.UserControl
    {
        public event EventHandler<EventArgs> GroupCodeChanged;
        GlobalVariables global = GlobalVariables.GetInstance();
        #region Public Property

        /// <summary>
        ///  Get Entire Subscription
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        ///  Password Root 
        /// </summary>
        public string PasswordRoot { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool UpdateButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                LoadSubscriptionData();
            }
        }

        /// <summary>
        /// Handle SignOn button click
        /// </summary>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Set site license flag
                if (SetSiteLicenseCheckBox.Checked == true)
                {
                    Subscription.CorpSubscription = Subscription.SubscriptionID;
                }
                else
                {
                    Subscription.CorpSubscription = Guid.Empty;
                }
                //Group code
                if (!String.IsNullOrEmpty(SingleSignOnGroupCode.Text))
                {
                    Subscription.GroupCode = SingleSignOnGroupCode.Text;
                }
                if (!String.IsNullOrEmpty(TimeoutUrl.Text))
                {
                    Subscription.TimeoutURL = TimeoutUrl.Text;
                }

                string SharedSecret = string.Empty;
                string type = String.Empty;
                PreAuthenticationType preAuthenticatedType = PreAuthenticationType.None;

                type = SingleSignOnRadioButtonList.SelectedItem.Value.Trim();
                if (type == "AICC")
                {
                    preAuthenticatedType = PreAuthenticationType.AICC;
                    SharedSecret = String.Empty;
                }
                else if (type == "Ticket")
                {
                    preAuthenticatedType = PreAuthenticationType.Ticket;
                    LoginTicket loginTicket = new LoginTicket();
                    SharedSecret = loginTicket.Save();
                }
                else if (type == "Reference")
                {
                    preAuthenticatedType = PreAuthenticationType.Reference;
                    SharedSecret = String.Empty;
                }

                GlobalVariables global = GlobalVariables.GetInstance();
                SubscriptionFactory subFactory = new SubscriptionFactory(global.UserConnStr);
                subFactory.PutSubscription(Subscription, UserId, Login);
                subFactory.PutSingleSignOnsub(PasswordRoot, SharedSecret, preAuthenticatedType);

                SingleSignOnErrorLabel.Text = Resources.Resource.SignOnSuccess;
                SingleSignOnErrorLabel.Visible = true;
                OnUpdategroupCode();
            }
            catch (Exception ex)
            {
                SingleSignOnErrorLabel.Text = Resources.Resource.SignOnFail;
                SingleSignOnErrorLabel.Visible = true;
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Sigle Sign On", ex);
            }
        }
        #endregion

        # region Methods
        #region PublicMethods
        public void UpdatedGroupCode()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
            Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(Subscription.SubscriptionID);
            SingleSignOnGroupCode.Text = manageSubscription.GroupCode;

        }
        #endregion
        /// <summary>
        /// Method to show or hide the header title text
        /// </summary>
        protected void ShowHeaderText(bool value)
        {
            HeaderText.Visible = value;
        }

        private void OnUpdategroupCode()
        {
            if (GroupCodeChanged != null)
            {
                GroupCodeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Load the Subscription Data
        /// </summary>
        private void LoadSubscriptionData()
        {
            if (Subscription != null)
            {
                if (Subscription.RegisteredUsers == 0)
                {
                    if (!String.IsNullOrEmpty(Subscription.GroupCode))
                    {
                        SingleSignOnGroupCode.Text = Subscription.GroupCode;
                    }
                }
                else
                {
                    SingleSignOnGroupCode.Enabled = false;
                    SingleSignOnGroupCode.Text = Subscription.GroupCode;
                }
                if (!String.IsNullOrEmpty(Subscription.TimeoutURL))
                {
                    TimeoutUrl.Text = Subscription.TimeoutURL;
                }
                UpdateButton.Visible = UpdateButtonView;
            }
        }
        #endregion
    }
}