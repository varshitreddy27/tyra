using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using B24.Common;
using B24.Sales4.DAL;
using B24.Common.Web;
using B24.Common.Web.Controls;

namespace B24.Sales4.UserControl
{
    public partial class WelcomeMsgSender : System.Web.UI.UserControl
    {
        #region Public Properties
        public Guid SubId
        {
            get;
            set;
        }
        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool ChangeSenderButtonView { get; set; }
        #endregion

        #region Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SubId == null || SubId == Guid.Empty)
            {
                return;
            }
            if (!IsPostBack)
            {
                SetReadView();
            }
            MultiView.ActiveViewIndex = 0;
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            string senderEmailaddr = String.Empty;
            senderEmailaddr = NewSenderTextBox.Text;
            try
            {
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory((this.Page as BasePage).UserConnStr);
                Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(SubId);
                if (ValidateSub(manageSubscription))
                {
                    subscriptionFactory.PutSubNotifyEmailFrom(SubId, senderEmailaddr);
                    WelcomeMessageErrorLabel.Text = Resources.Resource.WelcomeMessageUpdated;// "Welcome message sender is successfully modified";
                    WelcomeMessageErrorLabel.Visible = true;
                    SetReadView();
                    MultiView.ActiveViewIndex = 0;
                }
                else
                {
                    WelcomeMessageErrorLabel.Visible = true;
                    WelcomeMessageErrorLabel.Text = Resources.Resource.WelcomeMessageCannotModify;// "Can't modify email id for this subscription";
                }
            }
            catch (Exception)
            {
                WelcomeMessageErrorLabel.Visible = true;
                WelcomeMessageErrorLabel.Text = Resources.Resource.WelcomeMessageFailed;// "Modification is failed";
            }
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 0;
        }

        protected void ChangeSenderButton_Click(object sender, EventArgs e)
        {
            NewSenderTextBox.Text = string.Empty;
            ConfirmTextBox.Text = string.Empty;
            WelcomeMessageErrorLabel.Text = string.Empty;
            MultiView.ActiveViewIndex = 1;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Verify that the target subscription is valid (e.g., is b24library)
        /// </summary>
        /// <param name="subID">The subid to verify</param>
        /// <returns>True if target is valid; false otherwise</returns>
        private bool ValidateSub(Subscription manageSubscription)
        {
            bool isValid = false;
            isValid = manageSubscription.ApplicationName.ToLower().Contains("library") || manageSubscription.ApplicationName.ToLower().Contains("b24library");
            //
            return isValid;
        }

        /// <summary>
        /// Set the values for read only mode
        /// </summary>
        private void SetReadView()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory((this.Page as BasePage).UserConnStr);
            Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(SubId);
            CurrentSenderLabel.Text = manageSubscription.NotifyFromEmail != null ? manageSubscription.NotifyFromEmail : (this.Page as BasePage).User.Email;
            ChangeSenderButton.Visible = ChangeSenderButtonView;
        }
        #endregion
    }
}