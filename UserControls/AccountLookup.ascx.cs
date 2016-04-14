using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Logs;
using B24.Sales4.UI;

namespace B24.Sales4.UserControls
{
    public partial class AccountLookup : System.Web.UI.UserControl
    {
        #region Private Member

        B24.Common.Web.BasePage baseObject;
        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger = new Logger(Logger.LoggerType.Sales4);

        #endregion
        
        #region Public Member

        /// <summary>
        /// Event handler to handle cart change.
        /// </summary>
        public EventHandler UpdateCart { get; set; }

        #endregion

        #region Protected Events

        /// <summary>
        /// Load the Base page values to current page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            baseObject = this.Page as B24.Common.Web.BasePage;
        }

        /// <summary>
        /// Get the SubscriptionLookup List from DataBase and Binding the data to Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FindButton_Click(object sender, EventArgs e)
        {
            try
            {
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(baseObject.UserConnStr);
                List<Subscription> lookupResult = subscriptionFactory.SubscriptionLookup(baseObject.User.UserID, SubIDTextBox.Text, CompanyNameTextBox.Text, SalespersonFirstNameTextBox.Text, SalespersonLastNameTextBox.Text, ContractNumberTextBox.Text, ExactMatchCheckBox.Checked, ActiveAccountsOnlyCheckBox.Checked);
                if (lookupResult.Count > 0)
                {
                    ResultCountLabel.Text = lookupResult.Count.ToString(CultureInfo.InvariantCulture);
                    ResultGridView.DataSource = lookupResult;
                    ResultGridView.DataBind();
                    ResultPanel.Visible = true;
                    NoResultFound.Visible = false;
                    ResultCountPanel.Visible = true;
                    CartCheck();
                }
                else
                {
                    ResultCountLabel.Text = lookupResult.Count.ToString(CultureInfo.InvariantCulture);
                    NoResultFound.Visible = true;
                    ResultCountPanel.Visible = true;
                    ResultPanel.Visible = false;
                }
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "FindButton_Click Sql Exception", ex);
            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Logger.LogLevel.Error, "FindButton_Click() Argument Null exception", ex);
            }

        }

        /// <summary>
        /// Dynamically Binding the NavigateUrl's for Gridview HyperLinks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ResultGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField subIdHidden = (HiddenField)e.Row.FindControl("SubscriptionIdHiddenField");
                string subId = HttpUtility.UrlEncode(new Guid(subIdHidden.Value).ToString("B").ToUpperInvariant());
                HyperLink accountInfoHyperLink = (HyperLink)e.Row.FindControl("AccountInfoHyperLink");
                HyperLink manageAccountHyperLink = (HyperLink)e.Row.FindControl("ManageAccountHyperLink");
                HyperLink reportsHyperLink = (HyperLink)e.Row.FindControl("ReportsHyperLink");
                accountInfoHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales4Module.ModuleManageAccounts + "&submodule=" + Sales4Module.SubModuleManageAccountInfo + "&arg=" + subId;
                manageAccountHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales4Module.ModuleManageAccounts + "&submodule=" + Sales4Module.SubModuleSettings + "&arg=" + subId;
                reportsHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales4Module.ModuleManageAccounts + "&submodule=" + Sales4Module.SubModuleReports + "&arg=" + subId;
                Label expireLabel = (Label)e.Row.FindControl("EndDateLabel");
                DateTime expireDate = Convert.ToDateTime(expireLabel.Text, CultureInfo.InvariantCulture);
                if (expireDate < DateTime.Now)
                {
                    e.Row.CssClass = "b24-report-data-gray";
                }
                else
                {
                    e.Row.CssClass = "b24-report-data";
                }
            }
        }

        /// <summary>
        /// Update the State, cart checkBox is checked or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateCartButton_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox cartCheckBox;
                string cartChecked = string.Empty;

                if (!string.IsNullOrEmpty(baseObject.State["subCart"]))
                {
                    cartChecked = baseObject.State["subCart"];
                }
                foreach (GridViewRow row in ResultGridView.Rows)
                {
                    cartCheckBox = (CheckBox)row.FindControl("CartCheckBox");
                    HiddenField subIdHiddenCart = (HiddenField)row.FindControl("SubscriptionIdHiddenField");
                    if (cartCheckBox != null && cartCheckBox.Checked)
                    {
                        if (!string.IsNullOrEmpty(cartChecked))
                        {
                            cartChecked += ",";
                        }
                        cartChecked += subIdHiddenCart.Value;
                    }
                    else if (cartCheckBox != null && !cartCheckBox.Checked)
                    {
                        if (cartChecked.Contains(subIdHiddenCart.Value))
                        {
                            cartChecked = cartChecked.Replace(subIdHiddenCart.Value, string.Empty);
                        }
                    }
                }
                // Remove any duplicate value
                if (!string.IsNullOrEmpty(cartChecked))
                {
                    string[] checkedItems = cartChecked.Split(',').Distinct().ToArray().Where(item => !string.IsNullOrEmpty(item)).ToArray();
                    cartChecked = string.Join(",", checkedItems);
                }
                baseObject.State.Add("subCart", cartChecked);
                baseObject.State.Save();
                UpdateCart(null, null);

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, this.GetType(), "fade" + DateTime.Now.Ticks, "fadeMessageBlock('fadeBlock','Updated cart');", true);
            }
            catch (NullReferenceException ex)
            {
                logger.Log(Logger.LogLevel.Error, "UpdateCartButton_Click() Null Reference exception", ex);
                Response.Write("Error.aspx?message=" + ex.Message);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Retrieving cart details from State
        /// </summary>
        private void CartCheck()
        {
            try
            {
                CheckBox cartCheckBox;
                string cartChecked = string.Empty;
                if (string.IsNullOrEmpty(baseObject.State["subCart"]))
                {
                    return;
                }
                else
                {
                    cartChecked = baseObject.State["subCart"].ToString();
                }
                for (int i = 0; i < ResultGridView.Rows.Count; i++)
                {
                    HiddenField subIdHiddenCart = (HiddenField)ResultGridView.Rows[i].FindControl("SubscriptionIdHiddenField");
                    cartCheckBox = (CheckBox)ResultGridView.Rows[i].FindControl("CartCheckBox");
                    cartCheckBox.Checked = cartChecked.Contains(subIdHiddenCart.Value);
                }
            }
            catch (NullReferenceException ex)
            {
                logger.Log(Logger.LogLevel.Error, " CartCheck() Null Reference exception", ex);
                Response.Write("Error.aspx?message=" + ex.Message);
            }
        }

        #endregion
    }
}