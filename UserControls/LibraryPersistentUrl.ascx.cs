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
using B24.Sales3.DAL;
using B24.Common.Web;
using B24.Common.Web.Controls;

namespace B24.Sales3.UserControl
{
    public partial class LibraryPersistentUrl : System.Web.UI.UserControl
    {
        #region Properties
        public Guid SubId
        {
            get;
            set;
        }
        public string PwdRoot
        {
            get;
            set;
        }
        #endregion

        #region Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetReadView();
            }
            MultiView.ActiveViewIndex = 0;
        }

        protected void butUpdate_OnClick(object sender, EventArgs e)
        {
            string url = String.Empty;
            if (!String.IsNullOrEmpty(txtNewSender.Text))
            {
                url = txtNewSender.Text;
                try
                {
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory((this.Page as BasePage).UserConnStr);
                    Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(SubId);
                    if (ValidateSub(manageSubscription))
                    {
                        NBNCustomizationFactory nbnFactory = new NBNCustomizationFactory((this.Page as BasePage).UserConnStr);
                        NBNCustomization nbnCustomization = new NBNCustomization();
                        nbnCustomization.URI = Server.UrlEncode(url);
                        nbnCustomization.NBNCuctomizationTypeValue = NBNCustomizationType.PASSWORDROOT;
                        nbnCustomization.NBNCustomizationID = manageSubscription.PasswordRoot;
                        nbnFactory.PutNBNCustomization(nbnCustomization);
                        LibraryPersistentUrlErrorLabel.Text = "Library Persistent Url is successfully modified";
                        LibraryPersistentUrlErrorLabel.Visible = true;
                        SetReadView();
                        MultiView.ActiveViewIndex = 0;
                    }
                    else
                    {
                        LibraryPersistentUrlErrorLabel.Visible = true;
                        LibraryPersistentUrlErrorLabel.Text = "Can't modify URL for this subscription";
                    }
                }
                catch (Exception)
                {
                    LibraryPersistentUrlErrorLabel.Visible = true;
                    LibraryPersistentUrlErrorLabel.Text = "Modification is failed";
                }
            }

        }
        
        protected void PersistentURLchangeButton_Click(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 1;
        }
        
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 0;
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
            isValid = manageSubscription.ApplicationName.ToLower().Contains("library") && !manageSubscription.ApplicationName.ToLower().Contains("b24library");
            //
            return isValid;
        }

        /// <summary>
        /// Set the values for read view
        /// </summary>
        private void SetReadView()
        {
            NBNCustomizationFactory customizationFactory = new NBNCustomizationFactory((this.Page as BasePage).UserConnStr);
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory((this.Page as BasePage).UserConnStr);
            Subscription manageSubscription = subscriptionFactory.GetSubscriptionByID(SubId);
            PwdRoot = manageSubscription.PasswordRoot;
            NBNCustomization nbnCustomization = customizationFactory.GetNBNCustomization(NBNCustomizationType.PASSWORDROOT, PwdRoot);

            try
            {
                string defaultUrl = "none – use default system URL";
                CurrentURLLabel.Text = nbnCustomization != null ? Server.UrlDecode(nbnCustomization.URI ): defaultUrl;
            }
            catch (Exception ex)
            {
                LibraryPersistentUrlErrorLabel.Text = ex.Message;
                LibraryPersistentUrlErrorLabel.Visible = true;
            }
        }
        #endregion
    }
}
