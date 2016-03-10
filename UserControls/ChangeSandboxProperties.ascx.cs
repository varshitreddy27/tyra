using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.IGE;
using SkillSoft.IGE.IGEClient;
using B24.Common.Logs;
using System.Configuration;
using B24.Sales4.BLL;

namespace B24.Sales4.UserControl
{
    public partial class ChangeSandboxProperties : System.Web.UI.UserControl
    {
        #region Class Members
        private Guid sandboxID;
        private Guid subID;
        private Guid userID;
        private Guid requestorID;
        private Sales4.UI.BasePage basePage;
        private SandboxPropertySupplier sandboxPropertySupplier;
        private CheckBox cbx;
        #endregion

        #region Public Properties
        public Guid SubID
        {
            get { return subID; }
            set { subID = value; }
        }
        /// <summary>
        /// Todo:
        /// when we move everything to .net, it would be more proper to get sandbox at this level instead of in the supplier level
        /// 
        /// </summary>
        public Guid SandboxID
        {
            get { return sandboxID; }
            set { sandboxID = value; }
        }

        public Guid RequestorID
        {
            get { return requestorID; }
            set { requestorID = value; }
        }
        public SandboxProperties SandboxPropertyItems
        {
            get
            {
                SandboxProperties items = (SandboxProperties)ViewState["SandboxPropertyItemList"];
                return items;
            }
            set
            {
                ViewState["SandboxPropertyItemList"] = value;
            }

        }
        #endregion Public Properties

        #region Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            this.basePage = this.Page as Sales4.UI.BasePage;
            if (!IsPostBack)
            {
                BindData();
            }
            else
            {
                if (subID == Guid.Empty)
                {
                    return;
                }
                sandboxPropertySupplier = new SandboxPropertySupplier(subID, requestorID, this.basePage);
            }
        }


        protected void SandboxProperties_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (SandboxPropertyItems == null || SandboxPropertyItems.Items == null || SandboxPropertyItems.Items.Count <= 0)
                return;

        }

        protected void SandboxProperties_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                cbx = (CheckBox)e.Item.FindControl("cb_Status");
                SandboxPropertyItem curItem = ((ListViewDataItem)e.Item).DataItem as SandboxPropertyItem;
                if (curItem.Status == ContentStatus.PRIVATE)
                    cbx.Enabled = false;

                if (curItem.Status == ContentStatus.ON)
                {
                    cbx.Checked = true;
                }
                else
                {
                    cbx.Checked = false;
                }
            }

        }

        /// <summary>
        /// Event handler for Save Changes button click
        /// </summary>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lv_SandboxPropertiesEdit.Items.Count; i++)
            {
                cbx = (CheckBox)lv_SandboxPropertiesEdit.Items[i].FindControl("cb_Status");
                if (SandboxPropertyItems != null && SandboxPropertyItems.Items != null && SandboxPropertyItems.Items.Count > 0)
                {
                    if (!cbx.Enabled)
                    {
                        SandboxPropertyItems.Items[i].Status = ContentStatus.PRIVATE; // use private status to represent disabled checkbox
                    }
                    if (cbx.Checked)
                    {
                        SandboxPropertyItems.Items[i].Status = ContentStatus.ON;
                    }
                    else
                    {
                        SandboxPropertyItems.Items[i].Status = ContentStatus.OFF;
                    }
                }
            }
            if (SandboxPropertyItems != null && SandboxPropertyItems.Items != null && SandboxPropertyItems.Items.Count > 0)
            {
                sandboxPropertySupplier.ThisSandBoxProperties = SandboxPropertyItems;
                sandboxPropertySupplier.UpdateSandboxProperties();
                if (sandboxPropertySupplier.HasError)
                {
                    ErrorMsg.Text = sandboxPropertySupplier.ErrorMsg;
                    ErrorMsg.Visible = true;
                }
                else
                {
                    ErrorMsg.Text = "Sandbox Properties has been successfully updated.";
                    ErrorMsg.Visible = true;
                    lv_SandboxPropertiesEdit.DataSource = SandboxPropertyItems.Items;
                    lv_SandboxPropertiesEdit.DataBind();
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Expose the binding work so that parent conrol can call and bind the data
        /// </summary>
        public void BindData()
        {
            this.basePage = this.Page as Sales4.UI.BasePage;
            if (subID == Guid.Empty)
            {
                return;
            }
            sandboxPropertySupplier = new SandboxPropertySupplier(subID, requestorID, this.basePage);

            sandboxPropertySupplier.GetSandboxProperties();
            SandboxPropertyItems = sandboxPropertySupplier.ThisSandBoxProperties;
            if (sandboxPropertySupplier.HasError)
            {
                ErrorMsg.Text = sandboxPropertySupplier.ErrorMsg;
                ErrorMsg.Visible = true;
                panel_SandboxPropertiesEdit.Visible = false;
            }
            else
            {
                lv_SandboxPropertiesEdit.DataSource = SandboxPropertyItems.Items;
                lv_SandboxPropertiesEdit.DataBind();
            }
        }
        #endregion

    }
}
