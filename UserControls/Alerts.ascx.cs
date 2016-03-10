using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using B24.Common;
using B24.Common.Security;
using B24.Common.Web;
using B24.Common.Logs;
using System.Globalization;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// Reactivate or Extent this Trail.
    /// </summary>
    public partial class Alerts : System.Web.UI.UserControl
    {

        #region Private Members
        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger;
        private LinkButton alertLB;
        BasePage baseObject;
        
        #endregion

        #region Public Property
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
        public bool AlertsReadView { get; set; }
        /// <summary>
        /// Subscription object
        /// </summary>        
        public Subscription Subscription { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        #endregion

        #region Private Variables
        /// <summary>
        /// To get a list of existing alerts
        /// </summary>
        private SubscriptionAlertFactory subscriptionAlertFactory;

        /// <summary>
        /// To keep access 
        /// </summary>
        private bool hasAccess;

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EditButtonView = true;
            if (Subscription == null || Subscription.SubscriptionID == Guid.Empty)
            {
                return;
            }
            baseObject = Page as BasePage;
            if (!IsPostBack)
            {
                
                try
                { 
                    InitControls();
                }
                catch (Exception ex)
                {
                    AlertListErrorLabel.Visible = true;
                    AlertListErrorLabel.Text = (String)GetLocalResourceObject("AlertListLoadFail");
                    AlertListErrorUpdatePnl.Update();

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "User Detail", ex);
                }
            }
           
        }

        protected void SubscriptionAlertLV_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                alertLB = (LinkButton)e.Item.FindControl("Alert_LB");

                SubscriptionAlert curItem = ((ListViewDataItem)e.Item).DataItem as SubscriptionAlert;
                alertLB.Text = curItem.TitleDescription;
            }
        }

        /// <summary>
        /// Change row binding for report category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubscriptionAlertGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label dueDateLabel = (Label)e.Row.FindControl("DueDate");
                HiddenField alertIDHiddenField = (HiddenField)e.Row.FindControl("AlertIDHiddenField");
                LinkButton viewLinkButton = (LinkButton)e.Row.FindControl("ViewLinkButton");
            }
        }

        protected void ViewLinkButton_Click(object sender, EventArgs e)
        {
            LinkButton alert = sender as LinkButton;
            string alertG = alert.Text;
            string alertid = alert.CommandArgument;
            if (!String.IsNullOrEmpty(alertid))
                LoadAlertDetail(alertid);
            ViewUpdatePanel.Update();
        }

        protected void RemoveLinkButton_Click(object sender, EventArgs e)
        {
            LinkButton alert = sender as LinkButton;
            string alertid = alert.CommandArgument;
            if (!String.IsNullOrEmpty(alertid))
               DeleteAlert(alertid);
            AlertListUPnl.Update();
        }

        /// <summary>
        /// Make sure the user can  update alert detail fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EnableAlertViewControls()
        {
            titleTbx.ReadOnly = false;
            recipientTbx.ReadOnly = false;
            StatusDDL.Enabled = true;
            DueOnTextBox.ReadOnly = false;
            notesTbx.ReadOnly = false;
            sendEmailCbx.Enabled = true;
        }

        /// <summary>
        /// Show Alert detail panel with blank fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearAlert_Click(object sender, EventArgs e)
        {
            ClearAllFields();
            ViewUpdatePanel.Update();
            //EnableAlertViewControls();
        }

        /// <summary>
        /// Save event to update the Alert information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateAlert_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            // exec B24_PutSubscriptionAlert NULL,'F4238B49-DBFB-11D2-8675-0090272AD107','59CF77BD-5EE2-4E5F-A107-BAA9FC19E5E5',NULL,'2015-02-20 00:00:00',1,'Gita Test','GLigure@books24x7.com','Testing',0
            bool newAlert = true;
            try
            {
                string saIDStr = SubscriptionAlertIDHF.Value; 
                Guid saID = Guid.Empty;
                if (!String.IsNullOrEmpty(saIDStr))
                {
                    newAlert = false;
                    saID = new Guid(saIDStr);
                }
                string creatorIDStr = CreatorIDHF.Value;
                Guid creatorID = Guid.Empty;
                if (!String.IsNullOrEmpty(creatorIDStr))
                {
                    newAlert = false;
                    creatorID = new Guid(creatorIDStr);
                }
                SubscriptionAlert sa = new SubscriptionAlert();
                if (newAlert)
                    sa.CreatorId = UserId;
                else
                {
                    sa.SubscriptionAlertId = saID;  // need this if updating existing alert
                    sa.LastUpdaterId = UserId;
                    sa.CreatorId = creatorID;
                }

                sa.SubscriptionId = Subscription.SubscriptionID;
                
                sa.DueDate = Convert.ToDateTime(DueOnTextBox.Text); //{2/20/2004 12:00:00 AM}
                sa.Status = Convert.ToInt16(StatusDDL.SelectedValue);
                sa.Title = titleTbx.Text;
                sa.ToAddress = recipientTbx.Text;
                sa.Notes = notesTbx.Text;
                sa.SendEmail = Convert.ToInt16(sendEmailCbx.Checked);

                if (subscriptionAlertFactory == null)
                    subscriptionAlertFactory = new SubscriptionAlertFactory(baseObject.UserConnStr);

                if (newAlert)
                    subscriptionAlertFactory.PutSubscriptionAlert(sa);
                else
                    subscriptionAlertFactory.UpdateSubscriptionAlert(sa);
               
                ClearAllFields();
                BindGVData();
                AlertListUPnl.Update();
                ViewUpdatePanel.Update();
            }
            catch (Exception ex)
            {
                AlertDetailErrorLabel.Visible = true;
                AlertDetailErrorLabel.Text = (String)GetLocalResourceObject("AlertUpdateFail");
                ViewUpdatePanel.Update();

                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "Update Subscription", ex);
            }
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Expose the binding work so that parent conrol can call and bind the data
        /// </summary>
        public void BindGVData()
        {
            subscriptionAlertFactory = new SubscriptionAlertFactory(baseObject.UserConnStr);
            Collection<SubscriptionAlert> alertList = subscriptionAlertFactory.GetSubscriptionAlertList(Subscription.SubscriptionID);

            if (alertList.Count < 1)
                noAlertsLbl.Visible = true;
            else
            {
                SubscriptionAlertGridview.DataSource = alertList;
                SubscriptionAlertGridview.DataBind();
            }

        }
        #endregion

        # region private methods

        /// <summary>
        /// To clear all Alert details.
        /// </summary>
        private void ClearAllFields()
        {
            titleTbx.Text = "";
            SubscriptionAlertIDHF.Value = "";
            CreatorIDHF.Value = "";
            recipientTbx.Text = "";
            StatusDDL.SelectedValue = "1";
            DueOnTextBox.Text = "";
            notesTbx.Text = "";
            sendEmailCbx.Checked = false;
            updateLbl.Text = "";
            updateLbl.Visible = false;
            createdLbl.Text = "";
            createdLbl.Visible = false;
        }

        /// <summary>
        /// To Load Alert details.
        /// </summary>
        private void LoadAlertDetail(string alertidstr)
        {
            try
            {
                Guid alertID = new Guid(alertidstr);
                Collection<SubscriptionAlert> alertDetails = new Collection<SubscriptionAlert>();
                if (subscriptionAlertFactory == null)
                    subscriptionAlertFactory = new SubscriptionAlertFactory(baseObject.UserConnStr);
                alertDetails = subscriptionAlertFactory.GetSubscriptionAlertData(alertID);
                if (alertDetails.Count > 0)
                {
                    sectionTitleLbl.Text = "Update: " + alertDetails[0].TitleDescription;
                    titleTbx.Text = alertDetails[0].Title;
                    SubscriptionAlertIDHF.Value = Convert.ToString(alertID);
                    CreatorIDHF.Value = Convert.ToString(alertDetails[0].CreatorId);
                    recipientTbx.Text = alertDetails[0].ToAddress;
                    StatusDDL.SelectedValue = Convert.ToString(alertDetails[0].Status);
                    DueOnTextBox.Text = Convert.ToString(alertDetails[0].DueDate);
                    notesTbx.Text = alertDetails[0].Notes;
                    sendEmailCbx.Checked = Convert.ToBoolean(alertDetails[0].SendEmail);

                    UserFactory userfactory = new UserFactory(baseObject.UserConnStr);
                    User creator = userfactory.GetUserByID(alertDetails[0].CreatorId);
                    
                    createdLbl.Visible = true;
                    createdLbl.Text = "Created by " + creator.FirstName + " " + creator.LastName + ", " + Convert.ToString(alertDetails[0].Created);

                    if (alertDetails[0].LastUpdaterId != null && alertDetails[0].LastUpdaterId != Guid.Empty)
                    {
                        User updator = userfactory.GetUserByID(alertDetails[0].LastUpdaterId);
                        updateLbl.Text = "Updated by " + updator.FirstName + " " + updator.LastName + ", " + Convert.ToString(alertDetails[0].LastUpdated);
                        updateLbl.Visible = true;
                    }
                    else // hide if nobody has updated alert
                        updateLbl.Visible = false;

                    if (EditButtonView)
                        EnableAlertViewControls();
                    updateAlertBtn.Visible = EditButtonView;

                  //  Page.ClientScript.RegisterStartupScript(this.GetType(), "callFN" + DateTime.Now.Ticks, "callFN();", true);

                }
            }
            catch (Exception ex)
            {
                AlertDetailErrorLabel.Visible = true;
                AlertDetailErrorLabel.Text = (String)GetLocalResourceObject("AlertLoadFail"); ;
                ViewUpdatePanel.Update();

                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "User Detail", ex);
            }     
        }

        /// <summary>
        /// Delete specific alert.
        /// </summary>
        private void DeleteAlert(string alertidstr)
        {
            Guid alertID = new Guid(alertidstr);
            if (subscriptionAlertFactory == null)
                subscriptionAlertFactory = new SubscriptionAlertFactory(baseObject.UserConnStr);
            subscriptionAlertFactory.DeleteSubscriptionAlert(alertID);
        }
        /// <summary>
        /// Initialize the control values
        /// </summary>
        private void InitControls()
        {

            PermissionFactory permissionFactory = new PermissionFactory(baseObject.UserConnStr);

            Permission permissions = permissionFactory.LoadPermissionsById(UserId, baseObject.User.Identity.Name, String.Empty);

            if (permissions.SuperUser == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1)
            {
                hasAccess = true;
            }
            else
            {
                hasAccess = false;
            }
           // hasAccess = false;
            BindGVData();

           // newAlertBtn.Visible = EditButtonView;
            if (EditButtonView)
                EnableAlertViewControls();
        }

        #endregion

    }
}