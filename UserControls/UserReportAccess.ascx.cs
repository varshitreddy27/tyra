using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace B24.Sales3.UserControl
{
    public partial class UserReportAccess : System.Web.UI.UserControl
    {
        #region Private Members
        
        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.Sales3);        
        Dictionary<int, ReportAccess> reportDictionary;
        Boolean editButtonView = false;

        #endregion

        #region Property
       
        /// <summary>    
        /// User(Admin) making change request
        /// </summary>
        public Guid RequestorId { get; set; }

        /// <summary>
        /// User whose permission we want to change
        /// </summary>
        public User User { get; set; }

        /// <summary>
        ///  Subscription details
        /// </summary>
        public Subscription subscription { get; set; }

        public Boolean EditButtonView
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
        /// Page on Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User == null)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                InitializeControl();
            }
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Event to update the User Report catogories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                String reportCategoriesId = SelectedReportCategoriesId();
                ReportAccessFactory reportAccessFactory = new ReportAccessFactory(global.UserConnStr);
                reportAccessFactory.PutReportCategories(RequestorId, User.UserID, reportCategoriesId);              
                UserReportAccessErrorLabel.Text = Resources.Resource.UserDetailSuccess;
                UserReportAccessErrorLabel.Visible = true;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "Sql Exception", ex);
                UserReportAccessErrorLabel.Text = Resources.Resource.UserDetailFail;
                UserReportAccessErrorLabel.Visible = true;
            }

            // Reload the values
            InitializeControl();

            // Change the view to readonly mode
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// Edit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditButton_Click(object sender, EventArgs e)
        {
            UserReportAccessErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 0;
        }

        /// <summary>
        /// Handle the cancel button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditCancelButton_Click(object sender, EventArgs e)
        {
            UserReportAccessErrorLabel.Text = string.Empty;
            Multiview.ActiveViewIndex = 1;
        }

        /// <summary>
        /// report checkbox list on row data bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportsCheckBoxList_DataBound(object sender, EventArgs e)
        {
            CheckBoxList ch = (CheckBoxList)sender;
            foreach (ListItem bx in ch.Items)
            {
                if (reportDictionary[Convert.ToInt16(bx.Value, CultureInfo.InvariantCulture)].AccessLevel.ToString(CultureInfo.InvariantCulture) == "1")
                    bx.Selected = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initaialize the controls
        /// </summary>
        private void InitializeControl()
        {
            ReportAccessFactory reportAccessFactory = new ReportAccessFactory(global.UserConnStr);
            Collection<ReportAccess> reportAccessList = reportAccessFactory.GetReportCategories(RequestorId, User.UserID);
            ReportsCheckBoxList.DataTextField = "ReportCategory";
            ReportsCheckBoxList.DataValueField = "ReportCategoryID";
            reportDictionary = new Dictionary<int, ReportAccess>();
            foreach (ReportAccess reportAccess in reportAccessList)
            {
                reportDictionary.Add(reportAccess.ReportCategoryId, reportAccess);
            }
            IOrderedEnumerable<ReportAccess> reportListOrdered =
            reportAccessList.OrderBy(item => item.ReportCategory);

            ReportsCheckBoxList.DataSource = reportListOrdered;
            ReportsCheckBoxList.DataBind();
            ReportsReadCheckBoxList.DataSource = reportListOrdered;
            ReportsReadCheckBoxList.DataTextField = "ReportCategory";
            ReportsReadCheckBoxList.DataValueField = "ReportCategoryID";
            ReportsReadCheckBoxList.DataBind();

            EditButton.Visible = editButtonView;
        }

        /// <summary>
        /// collect the ReportcategoryIds
        /// </summary>
        /// <returns></returns>
        private string SelectedReportCategoriesId()
        {
            String selectedReportCategories = "";
            foreach (ListItem item in ReportsCheckBoxList.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(selectedReportCategories))
                    {
                        selectedReportCategories += ",";
                    }
                    selectedReportCategories += item.Value;
                }
            }

            return selectedReportCategories;
        }

        #endregion
    }
}
