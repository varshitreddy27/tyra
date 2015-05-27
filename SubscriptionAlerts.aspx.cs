using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using B24.Common;
using B24.Common.Logs;

namespace B24.Sales3.UI
{
    /// <summary>
    /// This form helps to create new alert for subscription and allows to edit the information of exisitng alerts.
    /// Input for this page is SubscriptionID.
    /// </summary>
    public partial class SubscriptionAlerts : BasePage
    {
        #region Private Members
        // The sub  to load
        private Guid subId;
        private SubscriptionAlertFactory subscriptionAlertFactory;
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);
        #endregion

        #region Protected Methods
        /// <summary>
        /// Set the values on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            subId = GetSubID();

            if (!IsPostBack)
            {
                LoadSubscriptionList();
                if (VerifyPermission())
                {
                    CreateAlertAccordionPane.Visible = true;
                    RecipientsTextBox.Text = User.Email;
                }
            }
        }

        /// <summary>
        /// On alert dropdown change will the values in edit area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AlertListDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            subscriptionAlertFactory = new SubscriptionAlertFactory(UserConnStr);
            if (VerifyPermission())
            {
                UpdateButton.Visible = true;
            }
            Guid selectedsubalertid = new Guid(AlertListDropDownList.SelectedItem.Value);
            if (Guid.Empty.Equals(selectedsubalertid))
            {
                RecipientsTextBox.Text = User.Email;
                EditAlertPanel.Visible = false;
                SubscriptionAccordion.SelectedIndex = 1;
                EditAlertUpdatePanel.Update();
            }
            else
            {
                Collection<B24.Common.SubscriptionAlert> SubAlertsData = subscriptionAlertFactory.GetSubscriptionAlertData(selectedsubalertid);
                BindSelectedSubAlertData(SubAlertsData);
                EditAlertPanel.Visible = true;      // open Alert data in a panel if a alert is selected 
                EditAlertUpdatePanel.Update();
            }
        }

        /// <summary>
        /// Insert new alert in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                IsEdit.Value = "false";
                B24.Common.SubscriptionAlert subAlert = new SubscriptionAlert();
                subAlert.CreatorId = User.UserID;
                subAlert.SubscriptionId = subId;
                subAlert.Title = Convert.ToString(TitleTextBox.Text);
                subAlert.DueDate = Convert.ToDateTime(SubAlertDueDateTextBox.Text);
                subAlert.Notes = Convert.ToString(SubAlertsNotesTextBox.Text);
                subAlert.ToAddress = RecipientsTextBox.Text;
                subAlert.Status = 1;

                subscriptionAlertFactory = new SubscriptionAlertFactory(UserConnStr);
                subscriptionAlertFactory.PutSubscriptionAlert(subAlert);

                NewAlertErrorLabel.Text = Resources.Resource.SubscriptionAlertCreated;
                NewAlertErrorLabel.Visible = true;

                LoadSubscriptionList();
                ClearNewAlert();
                SubAlertListUpdatePanel.Update();

            }

            catch (Exception ex)
            {
                NewAlertErrorLabel.Text = Resources.Resource.SubscriptionAlertNotCreated;
                NewAlertErrorLabel.Visible = true;
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubscriptionAlertNotCreated, ex);
            }
        }

        /// <summary>
        /// Update exisitng alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                IsEdit.Value = "true";
                B24.Common.SubscriptionAlert subAlert = new SubscriptionAlert();
                subAlert.SubscriptionAlertId = new Guid(AlertListDropDownList.SelectedItem.Value);
                subAlert.SubscriptionId = subId;
                subAlert.LastUpdaterId = User.UserID;
                subAlert.Title = Convert.ToString(TitleUpdateTextBox.Text);
                UpdatedSubAlertId.Value = subAlert.SubscriptionAlertId.ToString();
                subAlert.DueDate = Convert.ToDateTime(DueDateUpdateTextBox.Text);
                subAlert.Notes = Convert.ToString(NotesUpdateTextBox.Text);
                subAlert.ToAddress = RecipientsUpdateTextBox.Text;
                subAlert.Status = Convert.ToInt16(StatusList.SelectedValue.ToString());
                subAlert.CreatorId = new Guid(CreatorId.Value);

                subscriptionAlertFactory = new SubscriptionAlertFactory(UserConnStr);
                subscriptionAlertFactory.UpdateSubscriptionAlert(subAlert);

                EditAlertErrorLabel.Text = Resources.Resource.SubscriptionAlertUpdated;
                EditAlertErrorLabel.Visible = true;

                LoadSubscriptionList();
                ClearUpdateAlert();
                SubAlertListUpdatePanel.Update();
            }
            catch (Exception ex)
            {
                EditAlertErrorLabel.Text = Resources.Resource.SubscriptionAlertNotUpdated;
                EditAlertErrorLabel.Visible = true;
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SubscriptionAlertNotUpdated, ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the sub id from the postback or from the url
        /// </summary>
        /// <returns>A subid guid</returns>
        private Guid GetSubID()
        {
            Guid subscriptionId;   // The subid to return
            try
            {
                if (null != Request.QueryString["subId"] && !String.IsNullOrEmpty(Request.QueryString["subId"]))
                {
                    subscriptionId = new Guid(Request.QueryString["subId"]);
                }
                else if (Request.Form["subid"] != null)
                    subscriptionId = new Guid(Request.Form["subid"]);
                else if (State["AlertHolderSubId"] != null)
                    subscriptionId = new Guid(State["AlertHolderSubID"]);
                else
                {
                    subscriptionId = Guid.Empty;
                    Response.Redirect("Error.aspx");
                }
            }
            catch (FormatException ex)
            {
                logger.Log(Logger.LogLevel.Error, "FormatException", ex);
                subscriptionId = Guid.Empty;
                Response.Redirect("Error.aspx");
            }

            // Keep the sub id around for post backs...
            State.Add("AlertHolderSubId", subscriptionId.ToString());

            return subscriptionId;
        }
        /// <summary>
        /// To bind all subscription alert of a particular user in dropdownlist
        /// </summary>
        /// <param name="subscriptionAlertList"></param>
        private void BindSubscriptionAlertList(Collection<B24.Common.SubscriptionAlert> subscriptionAlertList, int selectedIndex)
        {
            AlertListDropDownList.DataSource = subscriptionAlertList;
            AlertListDropDownList.DataTextField = "TitleDescription";
            AlertListDropDownList.DataValueField = "SubscriptionAlertId";

            AlertListDropDownList.SelectedIndex = selectedIndex;

            AlertListDropDownList.DataBind();
        }

        /// <summary>
        /// To load the subscription alert if exist
        /// </summary>
        private void LoadSubscriptionList()
        {
            try
            {
                subscriptionAlertFactory = new SubscriptionAlertFactory(UserConnStr);

                Collection<B24.Common.SubscriptionAlert> SubAlertsList = subscriptionAlertFactory.GetSubscriptionAlertList(subId);
                Collection<B24.Common.SubscriptionAlert> alertList = new Collection<SubscriptionAlert>();
                SubscriptionAlert subscriptionAlert = new SubscriptionAlert();
                subscriptionAlert.TitleDescription = "---Select One---";
                subscriptionAlert.SubscriptionAlertId = Guid.Empty;
                subscriptionAlert.SubscriptionId = subId;
                alertList.Add(subscriptionAlert);



                //load items 
                if (!IsPostBack)
                {
                    foreach (SubscriptionAlert alert in SubAlertsList)
                    {
                        alertList.Add(alert);
                    }
                    BindSubscriptionAlertList(alertList, 0);
                }
                else
                {
                    int selectedIndex = 0;
                    for (int i = 0; i < SubAlertsList.Count; i++)
                    {
                        if (IsEdit.Value.Equals("true"))
                        {
                            if (SubAlertsList[i].SubscriptionAlertId.Equals(new Guid(UpdatedSubAlertId.Value)))
                            {
                                selectedIndex = i;
                            }
                        }
                        alertList.Add(SubAlertsList[i]);
                    }
                    if (IsEdit.Value.Equals("true"))
                    {
                        // bind the subalerts title in the alertdropdownlist
                        BindSubscriptionAlertList(alertList, selectedIndex + 1);
                    }
                    else
                    {
                        BindSubscriptionAlertList(alertList, 0);
                    }
                }
            }

            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, "No Alerts Found", ex);

            }
        }

        /// <summary>
        /// To bind a particular subscription alert data in the respective textbox
        /// </summary>
        /// <param name="selectedSubscriptionAlertData"></param>
        private void BindSelectedSubAlertData(Collection<B24.Common.SubscriptionAlert> selectedSubscriptionAlertData)
        {
            foreach (B24.Common.SubscriptionAlert item in selectedSubscriptionAlertData)
            {
                TitleUpdateTextBox.Text = item.Title;
                EditAlertErrorLabel.Visible = false;
                RecipientsUpdateTextBox.Text = item.ToAddress;
                StatusList.Items.FindByValue("0").Selected = false;
                StatusList.Items.FindByValue("1").Selected = false;
                if (item.Status == 0)
                {
                    StatusList.Items.FindByValue("0").Selected = true;
                }
                else
                {
                    StatusList.Items.FindByValue("1").Selected = true;
                }

                UserFactory userFactory = new UserFactory(UserConnStr);
                DueDateUpdateTextBox.Text = item.DueDate.ToString("MMM dd, yyyy");
                NotesUpdateTextBox.Text = item.Notes;

                CreatorId.Value = item.CreatorId.ToString();

                User Creator = userFactory.GetUserByID(item.CreatorId);
                CreatedLabel.Text = Creator.FirstName + " " + Creator.LastName + ", " + item.Created.ToShortDateString();

                if (!Guid.Empty.Equals(item.LastUpdaterId))
                {
                    User LastUpdater = userFactory.GetUserByID(item.LastUpdaterId);
                    LastUpdatedLabel.Text = LastUpdater.FirstName + " " + LastUpdater.LastName + ", " + item.LastUpdated.ToShortDateString();
                }
                SendEmailUpdateCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// Check the logged in user permission (only generaladmin can create and update the alert) 
        /// </summary>
        /// <returns></returns>
        private bool VerifyPermission()
        {
            PermissionFactory permFactory = new PermissionFactory(UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(User.UserID, User.Identity.Name, String.Empty);
            if (permissions.GeneralAdmin == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// On create button click clean up the form content
        /// </summary>
        private void ClearNewAlert()
        {
            TitleTextBox.Text = string.Empty;
            RecipientsTextBox.Text = string.Empty;
            SubAlertDueDateTextBox.Text = string.Empty;
            SubAlertsNotesTextBox.Text = string.Empty;
            SubAlertSendEmailCheckBox.Checked = false;
        }

        /// <summary>
        /// On update button click cleanup the corm content
        /// </summary>
        private void ClearUpdateAlert()
        {
            TitleUpdateTextBox.Text = string.Empty;
            RecipientsUpdateTextBox.Text = string.Empty;
            DueDateUpdateTextBox.Text = string.Empty;
            NotesUpdateTextBox.Text = string.Empty;
            CreatedLabel.Text = string.Empty;
            LastUpdatedLabel.Text = string.Empty;
            SendEmailUpdateCheckBox.Checked = false;
            StatusList.Text = string.Empty;
        }
        #endregion
    }
}