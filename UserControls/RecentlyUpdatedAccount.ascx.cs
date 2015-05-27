using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common.Security;
using B24.Common.Web;
using B24.Common;
using B24.Sales3.UI;

namespace B24.Sales3.UserControls
{
    public partial class RecentlyUpdatedAccount : System.Web.UI.UserControl
    {
        #region Private Members

        B24.Common.Web.BasePage basePage;
        string Welcome = "Welcome to B24 Sales,";

        #endregion

        #region Protected Methods

        /// <summary>
        /// Page Load event
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as B24.Common.Web.BasePage;
            if (basePage == null)
            {
                return;
            }
            IntializeControl();
        }

        /// <summary>
        /// Set the values which are we failed to do it from design
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecentSubcriptionGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField subIdHidden = (HiddenField)e.Row.FindControl("SubscriptionIdHiddenField");
                string subId = HttpUtility.UrlEncode(new Guid(subIdHidden.Value).ToString("B").ToUpper());
                HyperLink accountInfoHyperLink = (HyperLink)e.Row.FindControl("AccountInfoHyperLink");
                HyperLink manageAccount = (HyperLink)e.Row.FindControl("ManageAccount");
                HyperLink reports = (HyperLink)e.Row.FindControl("Reports");

                accountInfoHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts + "&submodule=" + Sales3Module.SubModuleManageAccountInfo + "&arg=" + subId;
                manageAccount.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts + "&submodule=" + Sales3Module.SubModuleSettings + "&arg=" + subId;
                reports.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts + "&submodule=" + Sales3Module.SubModuleReports + "&arg=" + subId;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set the control values
        /// </summary>
        private void IntializeControl()
        {
            WelcomeLabel.Text = Welcome + " " + basePage.User.FirstName + " " + basePage.User.LastName;
            // Bind data
            RecentlyUpdatedAccountFactory reportfactory = new RecentlyUpdatedAccountFactory(basePage.UserConnStr);
            List<B24.Common.RecentlyUpdatedAccount> reportList = reportfactory.GetReport(basePage.User.UserID);
            RecentSubcriptionGridview.DataSource = reportList;
            RecentSubcriptionGridview.DataBind();

            ManageUserLookupHyperLink.NavigateUrl = "~/ManageUser.aspx?module=" + Sales3Module.ModuleManageUsers;
            ManageAccountLookupHyperLink.NavigateUrl = "~/ManageAccount.aspx?module=" + Sales3Module.ModuleManageAccounts;
        }
        #endregion
    }
}