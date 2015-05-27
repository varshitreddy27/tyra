using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace B24.Sales3.UserControl
{

    public partial class EmailSettings : System.Web.UI.UserControl
    {
        #region private variables

        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.UserInfo);
        bool editButtonView = false;
        #endregion

        #region Property

        /// <summary>
        /// get/set Login detail.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User Id to be updated.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Read Only view.
        /// </summary>
        public bool EmailSettingsReadView { get; set; }

        public bool EditButtonView
        {
            get { return editButtonView; }
            set { editButtonView = value; }
        }

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

        /// <summary>
        /// OnInit call base page init.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserId == Guid.Empty)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                LoadEmailSettings();
            }
        }


        /// <summary>
        /// Edit click event to show the edit view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            EmailSettingsErrorLabel.Visible = false;
            ShowEditView();
        }

        /// <summary>
        /// Updating new values. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {

            try
            {
                UserPreferencesFactory userPreferencesFactory = new UserPreferencesFactory(global.UserConnStr);

                UserPreferences userPreferences = new UserPreferences();

                userPreferences = userPreferencesFactory.Get(UserId);

                userPreferences.EmailStatus = Convert.ToInt32(NewBookNotificationRadioButtonList.Items[NewBookNotificationRadioButtonList.SelectedIndex].Value, CultureInfo.InvariantCulture);
                userPreferences.EmailFormat = Convert.ToInt32(NewBookFormatRadioButtonList.Items[NewBookFormatRadioButtonList.SelectedIndex].Value, CultureInfo.InvariantCulture);
                userPreferences.DontSendEmail = TurnOffEmailCheckBox.Checked;
                userPreferences.EmailAdminFlag = EmailAdminCheckBox.Checked;

                userPreferencesFactory.Put(userPreferences);

                EmailSettingsErrorLabel.Visible = true;
                EmailSettingsErrorLabel.Text = Resources.Resource.EmailSettingsSuccess;
                ShowReadView();
            }
            catch (SqlException sqlException)
            {
                EmailSettingsErrorLabel.Visible = true;
                EmailSettingsErrorLabel.Text = Resources.Resource.EmailSettingsFail;

                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "Email Settings", sqlException);
            }


        }

        /// <summary>
        /// Cancel the changes/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            EmailSettingsErrorLabel.Visible = false;
            LoadEmailSettings();
        }

        #endregion

        #region private methods

        /// <summary>
        /// To display read only view
        /// </summary>
        private void ShowReadView()
        {
            TurnOffEmailCheckBox.Enabled = false;
            EmailAdminCheckBox.Enabled = false;
            NewBookFormatRadioButtonList.Enabled = false;
            NewBookNotificationRadioButtonList.Enabled = false;
            EditButton.Visible = EditButtonView;
            UpdateButton.Visible = false;
            CancelButton.Visible = false;
        }

        /// <summary>
        /// To display edit mode.
        /// </summary>
        private void ShowEditView()
        {
            TurnOffEmailCheckBox.Enabled = true;
            EmailAdminCheckBox.Enabled = true;
            NewBookFormatRadioButtonList.Enabled = true;
            NewBookNotificationRadioButtonList.Enabled = true;
            EditButton.Visible = false;
            UpdateButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// Get the string from the collection of salesgroup
        /// </summary>
        /// <param name="salesGroupCollection"></param>
        /// <returns></returns>
        private string GetSalesGroups(System.Collections.ObjectModel.Collection<MasterData> salesGroupCollection)
        {
            string salesString = "";
            for (int loopCounter = 0; loopCounter < salesGroupCollection.Count; loopCounter++)
            {
                MasterData masterData = salesGroupCollection[loopCounter];
                if (loopCounter > 0)
                    salesString += "," + masterData.SalesGroupName;
                else
                    salesString += masterData.SalesGroupName;
            }
            return salesString;
        }

        /// <summary>
        /// Load the values for Email.
        /// </summary>
        private void LoadEmailSettings()
        {
            try
            {
                UserPreferencesFactory userPreferencesFactory = new UserPreferencesFactory(global.UserConnStr);
                MasterDataFactory masterDataFactory = new MasterDataFactory(global.UserConnStr);

                UserPreferences userPreferences = new UserPreferences();

                userPreferences = userPreferencesFactory.Get(UserId);

                System.Collections.ObjectModel.Collection<MasterData> salesGroupCollection = masterDataFactory.GetSalesGroupsForUser(UserId);

                string userSaleGroupString = GetSalesGroups(salesGroupCollection);
                bool showNewBookNotificaiton = !userSaleGroupString.Contains("MSVL2");

                TurnOffEmailCheckBox.Checked = userPreferences.DontSendEmail;
                EmailAdminCheckBox.Checked = userPreferences.EmailAdminFlag;

                switch (userPreferences.EmailStatus)
                {
                    case 0:
                        NewBookNotificationRadioButtonList.Items[0].Selected = true;
                        NewBookNotificationRadioButtonList.Items[1].Selected = false;
                        NewBookNotificationRadioButtonList.Items[2].Selected = false;
                        break;
                    case 1:
                        NewBookNotificationRadioButtonList.Items[0].Selected = false;
                        NewBookNotificationRadioButtonList.Items[1].Selected = true;
                        NewBookNotificationRadioButtonList.Items[2].Selected = false;
                        break;
                    case 3:
                        NewBookNotificationRadioButtonList.Items[0].Selected = false;
                        NewBookNotificationRadioButtonList.Items[1].Selected = false;
                        NewBookNotificationRadioButtonList.Items[2].Selected = true;
                        break;
                    default:
                        NewBookNotificationRadioButtonList.Items[0].Selected = false;
                        NewBookNotificationRadioButtonList.Items[1].Selected = false;
                        NewBookNotificationRadioButtonList.Items[2].Selected = false;
                        break;
                }

                switch (userPreferences.EmailFormat)
                {
                    case 0:
                        NewBookFormatRadioButtonList.Items[0].Selected = true;
                        NewBookFormatRadioButtonList.Items[1].Selected = false;
                        break;
                    case 1:
                        NewBookFormatRadioButtonList.Items[0].Selected = false;
                        NewBookFormatRadioButtonList.Items[1].Selected = true;
                        break;
                    default:
                        NewBookFormatRadioButtonList.Items[0].Selected = false;
                        NewBookFormatRadioButtonList.Items[1].Selected = false;
                        break;
                }

                if (EmailSettingsReadView)
                {
                    ShowReadView();
                }
                else
                {
                    ShowEditView();
                }
            }
            catch (SqlException sqlException)
            {
                EmailSettingsErrorLabel.Visible = true;
                EmailSettingsErrorLabel.Text = Resources.Resource.EmailSettingsLoadFail;

                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "Email Settings", sqlException);
            }

        }

        #endregion

    }
}
