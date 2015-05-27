using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using B24.Common;
using B24.Common.Logs;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace B24.Sales3.UI
{
    public partial class GeneralReport : BasePage
    {

        #region Private Variables

        Logger logger = new Logger(Logger.LoggerType.Sales3);
        B24.Common.Web.BasePage basePage;
        Guid subId;
        Subscription subscription;
        #endregion

        #region Protected Members
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check the user has access
                if (!CheckAccess())
                {
                    return;
                }

                basePage = this.Page as B24.Common.Web.BasePage;
                GetValuesFromQueryString();
                InitPopulate();
            }
            catch (Exception exception)
            {
                Response.Redirect("Error.aspx?message=" + exception.Message);
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Get all query string, form and hidden field values here
        /// </summary>
        private void GetValuesFromQueryString()
        {

            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    subId = new Guid(Request.QueryString["arg"]);
                }
            }
            catch (FormatException formatException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString FormatException ", formatException);
                return;
            }
            catch (OverflowException overflowException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString OverflowException ", overflowException);
                return;
            }
            catch (ArgumentException argumentException)
            {
                logger.Log(Logger.LogLevel.Error, " GetValuesFromQueryString ArgumentException ", argumentException);
                return;
            }
        }

        /// <summary>
        ///  Initialize submodule menu control values
        /// </summary>
        private void InitPopulate()
        {
            try
            {
                ManageAccountUserControl.EditBtnview = false;
                ManageAccountUserControl.UserId = basePage.User.UserID;
                ManageAccountUserControl.Login = basePage.User.Identity.Name;
                if (subId != Guid.Empty)
                {
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                    subscription = subscriptionFactory.GetSubscriptionByID(subId);
                    ManageAccountUserControl.Visible = true;
                    ManageAccountUserControl.Subscription = subscription;

                    AdvanceReportUserControl.RequestorSubscription = subscription;
                }
                else
                {
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(UserConnStr);
                    subscription = subscriptionFactory.GetSubscriptionByID(basePage.User.SubscriptionId);
                    ManageAccountUserControl.Visible = false;
                    ManageAccountUserControl.Subscription = subscription;
                }

            }
            catch (ArgumentNullException ex)
            {
                logger.Log(Logger.LogLevel.Error, "InitializeControls ArgumentNullexception", ex);
                return;
            }
            catch (SqlException ex)
            {
                logger.Log(Logger.LogLevel.Error, "InitializeControls Sql Exception", ex);
                return;
            }

        }

        /// <summary>
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales3Module.ModuleGeneralReporting, 0))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                ManageAccountUserControl.Visible = false;
                AdvanceReportUserControl.Visible = false;
            }
            return hasAccess;
        }

        #endregion
    }

}
