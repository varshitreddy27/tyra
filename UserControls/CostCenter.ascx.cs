using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Globalization;

namespace B24.Sales4.UserControl
{
    public partial class CostCenter : System.Web.UI.UserControl
    {
        #region Private Members
        private GlobalVariables global = GlobalVariables.GetInstance();
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);
        private BasePage baseObject;
        #endregion

        #region Public Properties
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionID { get; set; }

        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool AddButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SubscriptionID == null || SubscriptionID == Guid.Empty)
            {
                return;
            }
            baseObject = this.Page as BasePage;
            InitializeControls();

            if (!Page.IsPostBack)
            {
                try
                {
                    GetCostCenter();
                }
                catch (IndexOutOfRangeException ex)
                {
                    logger.Log(Logger.LogLevel.Error, "Index Out Of Range Exception", ex);
                    CostCenterErrorLabel.Text = Resources.Resource.Error;
                    CostCenterErrorLabel.Visible = true;
                }
                catch (NullReferenceException ex)
                {
                    logger.Log(Logger.LogLevel.Error, "Null Reference exception ", ex);
                    CostCenterErrorLabel.Text = Resources.Resource.Error;
                    CostCenterErrorLabel.Visible = true;
                }

            }
        }

        /// <summary>
        /// To Add New Cost Center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddCostCenter_Click(object sender, EventArgs e)
        {
            // Update the rule
            try
            {
                B24.Common.CostCenter costCenter = new B24.Common.CostCenter();
                costCenter.SubscriptionID = SubscriptionID;
                costCenter.CostCenterID = Guid.NewGuid();
                costCenter.CostCenters = NewCCName.Text.Trim();
                costCenter.Description = NewCCDescription.Text.Trim();
                costCenter.Sequence = Convert.ToInt32(NewCCSequence.Text.Trim(), CultureInfo.InvariantCulture);


                CostCenterFactory costCenterFac = new CostCenterFactory(baseObject.UserConnStr);
                costCenterFac.PutCostCenter(costCenter);

                GetCostCenter();
                CostCenterErrorLabel.Text = Resources.Resource.CostCenterAdded;
                CostCenterErrorLabel.Visible = true;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorUpdatingCostCenter, ex);
                CostCenterErrorLabel.Text = Resources.Resource.ErrorUpdatingCostCenter;
                CostCenterErrorLabel.Visible = true;
            }
            finally
            {
                NewCCName.Text = "";
                NewCCSequence.Text = "";
                NewCCDescription.Text = "";
            }
        }

        /// <summary>
        /// To Get Existing Cost Center and fill the grid view 
        /// </summary>
        protected void GetCostCenter()
        {
            CostCenterFactory costCenterFac = new CostCenterFactory(baseObject.UserConnStr);
            List<B24.Common.CostCenter> costCenterList = costCenterFac.GetCostCenterList(SubscriptionID);
            if (costCenterList.Count == 0)
            {
                CostCenterManagement.Visible = false;
            }
            else
            {
                CostCenterManagement.Visible = true;
            }
            
            CostCenterGridView.DataSource = costCenterList;
            CostCenterGridView.DataBind();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize page controls
        /// </summary>
        private void InitializeControls()
        {
            // Event handlers
            CostCenterGridView.RowEditing += new GridViewEditEventHandler(CostCenterGridView_RowEditing);
            CostCenterGridView.RowCancelingEdit += new GridViewCancelEditEventHandler(CostCenterGridView_RowCancelingEdit);
            CostCenterGridView.RowUpdating += new GridViewUpdateEventHandler(CostCenterGridView_RowUpdating);
            CostCenterGridView.RowCreated += new GridViewRowEventHandler(CostCenterGridView_RowCreated);

            AddCostCenter.Visible = AddButtonView;
        }

        /// <summary>
        /// Updates to individual rows as they're created
        /// </summary>
        private void CostCenterGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView ObjectCostCenterGridView = sender as GridView;   // The cost center gridview itself
            GridViewRow row = e.Row;                            // The row being created
            int editIndex = ObjectCostCenterGridView.EditIndex;       // Index of the row currently being edited

            if (row.RowType == DataControlRowType.DataRow)
            {
                // Show the relevant buttons depending upon whether the row is edit
                ImageButton updateBT = row.FindControl("UpdateButton") as ImageButton;
                ImageButton cancelBT = row.FindControl("CancelButton") as ImageButton;
                ImageButton editBT = row.FindControl("EditButton") as ImageButton;
                if (editIndex == row.DataItemIndex)
                {
                    updateBT.Visible = true;
                    cancelBT.Visible = true;
                    editBT.Visible = false;
                    //deleteBT.Visible = false;
                }
                else
                {
                    updateBT.Visible = false;
                    cancelBT.Visible = false;
                    editBT.Visible = true;
                    //deleteBT.Visible = true;
                }
                editBT.Enabled = AddButtonView;
            }
        }

        /// <summary>
        /// Handle the row update event
        /// </summary>
        private void CostCenterGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView ObjectCostCenterGridView = sender as GridView;                                           // Gridview throwing the event
            Guid costCenterID = new Guid(ObjectCostCenterGridView.DataKeys[e.RowIndex].Value.ToString());     // The unique id of rule being updated
            GridViewRow row = ObjectCostCenterGridView.Rows[e.RowIndex];                                      // The row being updated

            // Get the controls
            TextBox sequenceTB = row.FindControl("SeqTextBox") as TextBox;
            TextBox costcenterTB = row.FindControl("CostCenterTextBox") as TextBox;
            TextBox descriptionTB = row.FindControl("DescTextBox") as TextBox;
            
            // Update the rule
            try
            {
                B24.Common.CostCenter costCenter = new B24.Common.CostCenter();
                costCenter.SubscriptionID = SubscriptionID;
                costCenter.CostCenterID = costCenterID;
                costCenter.CostCenters = costcenterTB.Text;
                costCenter.Description = descriptionTB.Text;
                costCenter.Sequence = Convert.ToInt32(sequenceTB.Text,CultureInfo.InvariantCulture );

                CostCenterFactory costCenterFac = new CostCenterFactory(baseObject.UserConnStr);
                costCenterFac.PutCostCenter(costCenter);

                ObjectCostCenterGridView.EditIndex = -1;
                GetCostCenter();
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorUpdatingCostCenter, ex);
                CostCenterErrorLabel.Text = Resources.Resource.ErrorUpdatingCostCenter;
                CostCenterErrorLabel.Visible = true;
            }
        }

        /// <summary>
        /// Handle Cancel
        /// </summary>
        private void CostCenterGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CostCenterGridView.EditIndex = -1;
            GetCostCenter();
        }
        
        /// <summary>
        /// Handle the row editing event
        /// </summary>
        private void CostCenterGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CostCenterGridView.EditIndex = e.NewEditIndex;
            GetCostCenter();
        }
        #endregion
    }
}