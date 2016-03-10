using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Logs;
using System.Linq;
using System.Globalization;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// To update the Subscription flag 
    /// </summary>
    public partial class SubscriptionFlag : System.Web.UI.UserControl
    {
        #region Private Members
        private Dictionary<int, B24.Common.SubscriptionFlag> flagDictionary;
        private Sales4.UI.BasePage basePage;
        #endregion

        #region Public Property
        /// <summary>
        /// Subscription Id 
        /// </summary>
        public Guid SubscriptionId { get; set; }
        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Protected Events
        /// <summary>
        /// Handle the page load events 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SubscriptionId == null || SubscriptionId == Guid.Empty)
            {
                return;
            }
            basePage = this.Page as Sales4.UI.BasePage;
            if (!Page.IsPostBack)
            {
                LoadAllSubscriptionFlag(SubscriptionId);
                LoadSpecialSubscriptionSettingFlag();
                EditButton.Visible = EditButtonView;
            }
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Handle the DataBound events of checklist
        /// </summary>
        protected void SubFlagCheckBoxList_DataBound(object sender, EventArgs e)
        {
            CheckBoxList ch = (CheckBoxList)sender;
            foreach (ListItem bx in ch.Items)
            {
                if (flagDictionary[Convert.ToInt16(bx.Value, CultureInfo.InvariantCulture)].Status.ToString(CultureInfo.InvariantCulture) == "1")
                    bx.Selected = true;
            }
        }


        /// <summary>
        /// Handle the update events
        /// </summary>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables global = GlobalVariables.GetInstance();
                SubscriptionFlagFactory subFlagFactory = new SubscriptionFlagFactory(basePage.UserConnStr);
                subFlagFactory.SaveAllSubscriptionFlags(SubscriptionId, PrepareSubscriptionIds());

                SubscriptionFlagErrorLabel.Text = Resources.Resource.SubFlagSuccess;
                SubscriptionFlagErrorLabel.Visible = true;
            }
            catch (Exception ex)
            {
                SubscriptionFlagErrorLabel.Text = Resources.Resource.SubFlagFail;
                SubscriptionFlagErrorLabel.Visible = true;


                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Partner Logo", ex);
            }
            // Reload the values
            LoadAllSubscriptionFlag(SubscriptionId);
            LoadSpecialSubscriptionSettingFlag();

            // Change the view to readonly mode
            Multiview.ActiveViewIndex = 1;
        }
        /// <summary>
        /// Handle the Edit button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            SubscriptionFlagErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 0;
        }
        /// <summary>
        /// Handle the cancel button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            SubscriptionFlagErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Method to show or hide the header title text
        /// </summary>
        /// <param name="value"></param>
        public void ShowHeaderText(bool value)
        {
            HeaderText.Visible = value;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method for load the subscription flag list
        /// </summary>
        /// <param name="SubId">The Subscription Id</param>
        private void LoadAllSubscriptionFlag(Guid subscriptionId)
        {
            if (subscriptionId != Guid.Empty)
            {
                SubscriptionFlagFactory subscriptionFlagFactory = new SubscriptionFlagFactory(basePage.UserConnStr);
                List<B24.Common.SubscriptionFlag> subscriptionFlagList = subscriptionFlagFactory.GetAllSubscriptionFlags(subscriptionId);

                SubscriprionFlagCheckBoxList.DataTextField = "Description";
                SubscriprionFlagCheckBoxList.DataValueField = "FlagID";

                flagDictionary = new Dictionary<int, B24.Common.SubscriptionFlag>();
                foreach (B24.Common.SubscriptionFlag subscriptionFlag in subscriptionFlagList)
                {
                    flagDictionary.Add(subscriptionFlag.FlagID, subscriptionFlag);
                }
                IOrderedEnumerable<B24.Common.SubscriptionFlag> subscriptionFlagListOrdered =
                subscriptionFlagList.OrderBy(item => item.Description);

                SubscriprionFlagCheckBoxList.DataSource = subscriptionFlagListOrdered;
                SubscriprionFlagCheckBoxList.DataBind();               
            }
        }

        /// <summary>
        /// collect the subscriptionflagIds
        /// </summary>
        private string PrepareSubscriptionIds()
        {
            string subscriptionFlagIds = string.Empty;
            foreach (ListItem listItem in SubscriprionFlagCheckBoxList.Items)
            {
                if (listItem.Selected == true)
                {
                    subscriptionFlagIds = subscriptionFlagIds + listItem.Value + ',';
                }
            }
            if (subscriptionFlagIds.Length > 1)
            {
                subscriptionFlagIds = subscriptionFlagIds.Substring(0, subscriptionFlagIds.Length - 1);
            }
            return subscriptionFlagIds;
        }

        /// <summary>
        /// Method for load the Special subscription Setting list
        /// </summary>
        private void LoadSpecialSubscriptionSettingFlag()
        {
            SubscriptionFlagFactory subscriptionFlagFactory = new SubscriptionFlagFactory(basePage.UserConnStr);
            List<B24.Common.SubscriptionFlag> subscriptionFlagList = subscriptionFlagFactory.GetActiveSubscriptionFlags(SubscriptionId);
            List<B24.Common.SubscriptionFlag> newsubscriptionFlagList = new List<B24.Common.SubscriptionFlag>();
            SubscriptionFlag subscriptionFlag = new SubscriptionFlag();
            foreach (B24.Common.SubscriptionFlag flags in subscriptionFlagList)
            {
                if (flags.FlagID.ToString(CultureInfo.InvariantCulture) == "30" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "32" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "33" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "34" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "35" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "36" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "39" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "40" ||
                    flags.FlagID.ToString(CultureInfo.InvariantCulture) == "41")
                {
                    newsubscriptionFlagList.Add(flags);
                }
                //SpecialSubscriptionSettingFlagsListBox.DataSource = newsubscriptionFlagList;
                //SpecialSubscriptionSettingFlagsListBox.DataTextField = "description";
                //SpecialSubscriptionSettingFlagsListBox.DataValueField = "name";
                //SpecialSubscriptionSettingFlagsListBox.DataBind();
                //SettingsReadListBox.DataSource = newsubscriptionFlagList;
                //SettingsReadListBox.DataTextField = "description";
                //SettingsReadListBox.DataValueField = "name";
                //SettingsReadListBox.DataBind();
                //SpecialSubscriptionReadViewListView.DataSource = newsubscriptionFlagList;
                //SpecialSubscriptionReadViewListView.DataBind();
                //SpecialSubscriptionReadViewListView.Visible = true;
                SubscriptionFlagListView.DataSource = subscriptionFlagList;
                SubscriptionFlagListView.DataBind();
                SubscriptionFlagListView.Visible = true;
                //NoSettingsReadLabel.Visible = false;
                NosubscriptionFlagLabel.Visible = false;
            }
            if (newsubscriptionFlagList.Count == 0)
            {                
                //NoSettingsReadLabel.Visible = true;
                //SpecialSubscriptionReadViewListView.Visible = false;
            }
            if (subscriptionFlagList.Count == 0)
            {
                NosubscriptionFlagLabel.Visible = true;
                SubscriptionFlagListView.Visible = false;
            }
        }

        #endregion
    }
}