using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Logs;
using B24.Common.Web;
using B24.Sales4.UI;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// User control to manage user data.
    /// </summary>
    public partial class UserLookup : System.Web.UI.UserControl
    {
        #region Private Members

        //Create Basepage object
        private B24.Common.Web.BasePage basePage;

        // TO get count for Non SoftDeletedUser
        private int nonSoftDeletedUserCount;

        //Create Logger object
        private Logger logger = new Logger(Logger.LoggerType.Sales3);

        #endregion

        #region Public Member
        /// <summary>
        /// Event handler to handle cart change.
        /// </summary>
        public EventHandler UpdateCart{ get; set; }
        #endregion

        #region Protected Events

        /// <summary>
        /// Load the base page values to the current page on page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as B24.Common.Web.BasePage;
        }
        /// <summary>
        /// Find event to Get the user details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        protected void FindButton_Click(object sender, EventArgs e)
        {
            try
            {
                UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                List<User> lookupResult = userFactory.UserLookup(basePage.User.UserID, LogOnTextBox.Text, FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text.Trim(), ExactMatchCheckBox.Checked, ActiveAccountsOnlyCheckBox.Checked);
                if (lookupResult.Count > 0)
                {
                    ResultGridView.DataSource = lookupResult;
                    ResultGridView.DataBind();
                    ResultPanel.Visible = true;
                    NoResultsLabel.Visible = false;
                    UserMatchesPanel.Visible = true;
                    CartCheck();
                }
                else
                {
                    ResultCountLabel.Text = lookupResult.Count.ToString(CultureInfo.InvariantCulture);
                    ResultPanel.Visible = false;
                    UserMatchesPanel.Visible = true;
                    NoResultsLabel.Visible = true;
                }
            }
            catch (ArgumentNullException exception)
            {
                logger.Log(Logger.LogLevel.Error, " FindButton_Click ArgumentNullException ", exception);
                return;
            }
            catch (SqlException exception)
            {
                logger.Log(Logger.LogLevel.Error, " FindButton_Click Sql Exception", exception);
                return;
            }

        }
        /// <summary>
        /// Bind the data in GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void ResultGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DateTime datatime = Convert.ToDateTime(e.Row.Cells[6].Text);

                    //To show Never for not logged in user.
                    if (datatime == DateTime.MinValue)
                        e.Row.Cells[6].Text = "Never";

                    HiddenField userIDHidden = (HiddenField)e.Row.FindControl("userIDHiddenField");
                    HiddenField subscriptionStatusIDHidden = (HiddenField)e.Row.FindControl("SubscriptionStatusIDHidden");
                    HyperLink UserInfoHyperLink = (HyperLink)e.Row.FindControl("UserInfoHyperLink");
                    HyperLink ManageUserHyperLink = (HyperLink)e.Row.FindControl("ManageUserHyperLink");

                    string userID = HttpUtility.UrlEncode(new Guid(userIDHidden.Value).ToString("B").ToUpper(CultureInfo.InvariantCulture));

                    UserInfoHyperLink.NavigateUrl = "~/ManageUser.aspx?module=" + Sales4Module.ModuleManageUsers + "&submodule=" + Sales4Module.SubModuleManageUserInfo + "&arg=" + userID;
                    ManageUserHyperLink.NavigateUrl = "~/ManageUser.aspx?module=" + Sales4Module.ModuleManageUsers + "&submodule=" + Sales4Module.SubModuleEmailSettings + "&arg=" + userID;

                    if (subscriptionStatusIDHidden.Value == "8")
                    {
                        Label subscriptionStatusIDLabel = (Label)e.Row.FindControl("SubscriptionStatusIDLabel");
                        subscriptionStatusIDLabel.Visible = true;
                        e.Row.CssClass = "b24-report-data-gray";
                    }
                    else
                    {
                        e.Row.CssClass = "b24-report-data";
                        nonSoftDeletedUserCount++;
                    }

                    //To encode the values in ResultPanel in order to avoid script execution

                    Label userNameLabel = (Label)e.Row.FindControl("UserNameLabel");
                    userNameLabel.Text = Server.HtmlEncode(userNameLabel.Text);

                    Label firstNameLabel = (Label)e.Row.FindControl("FirstNameLabel");
                    firstNameLabel.Text = Server.HtmlEncode(firstNameLabel.Text);

                    Label lastNameLabel = (Label)e.Row.FindControl("LastNameLabel");
                    lastNameLabel.Text = Server.HtmlEncode(lastNameLabel.Text);

                }
                ResultCountLabel.Text = nonSoftDeletedUserCount.ToString();
            }

            catch (FormatException exception)
            {
                logger.Log(Logger.LogLevel.Error, " ResultGridView_RowDataBound FormatException ", exception);
                return;
            }
            catch (Exception exception)
            {
                logger.Log(Logger.LogLevel.Error, " ResultGridView_RowDataBound Exception", exception);
                return;
            }

        }
        /// <summary>
        /// Update cart information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void UpdateCartButton_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox cartCheckBox;
                string cartChecked = string.Empty;
                if (!string.IsNullOrEmpty(basePage.State["userCart"]))
                {
                    cartChecked = basePage.State["userCart"];
                }
                foreach (GridViewRow row in ResultGridView.Rows)
                {
                    cartCheckBox = (CheckBox)row.FindControl("CartCheckBox");
                    HiddenField userIDHiddenCart = (HiddenField)row.FindControl("userIDHiddenField");
                    if (cartCheckBox != null && cartCheckBox.Checked)
                    {
                        if (!string.IsNullOrEmpty(cartChecked))
                        {
                            cartChecked = cartChecked + ",";
                        }
                        cartChecked += userIDHiddenCart.Value;
                    }
                    else if (cartCheckBox != null && !cartCheckBox.Checked)
                    {
                        if (cartChecked.Contains(userIDHiddenCart.Value))
                        {
                            cartChecked = cartChecked.Replace(userIDHiddenCart.Value, string.Empty);
                        }
                    }
                }
                // Remove any duplicate value
                if (!string.IsNullOrEmpty(cartChecked))
                {
                    string[] checkedItems = cartChecked.Split(',').Distinct().ToArray().Where(item => !string.IsNullOrEmpty(item)).ToArray();
                    cartChecked = string.Join(",", checkedItems);
                }
                basePage.State.Add("userCart", cartChecked);
                basePage.State.Save();
                UpdateCart(null, null);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, this.GetType(), "fade" + DateTime.Now.Ticks, "fadeMessageBlock('fadeBlock','Updated cart');", true);
            }
            catch (ArgumentNullException exception)
            {
                logger.Log(Logger.LogLevel.Error, " UpdateCartButton_Click ArgumentNullException", exception);
                return;
            }
            catch (Exception exception)
            {
                logger.Log(Logger.LogLevel.Error, " UpdateCartButton_Click Exception", exception);
                return;
            }
        }

        #endregion

        # region private methods

        /// <summary>
        /// Retrieving cart details
        /// </summary>
        private void CartCheck()
        {
            CheckBox cartCheckBox;
            string cartChecked = string.Empty;
            if (string.IsNullOrEmpty(basePage.State["userCart"]))
            {
                return;
            }
            else
            {
                cartChecked = basePage.State["userCart"].ToString();
            }

            for (int i = 0; i < ResultGridView.Rows.Count; i++)
            {
                HiddenField userIDHiddenCart = (HiddenField)ResultGridView.Rows[i].FindControl("userIDHiddenField");
                cartCheckBox = (CheckBox)ResultGridView.Rows[i].FindControl("CartCheckBox");
                cartCheckBox.Checked = cartChecked.Contains(userIDHiddenCart.Value);
            }
        }

        #endregion
    }
}