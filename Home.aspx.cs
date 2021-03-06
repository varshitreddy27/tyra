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
using B24.Common.Web;
using B24.Common.Web.Controls;
using B24.Common.Security;

namespace B24.Sales4.UI
{
    public partial class Home : BasePage
    {
        private Logger logger = new Logger(Logger.LoggerType.Sales4);
        #region Protected Events
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check the user has access
            if (!CheckAccess())
            {
                return;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales4Module.ModuleInfo, 0))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                RecentlyUpdated.Visible = false;
                logger.Log(Logger.LogLevel.Error, "User has no access to any modules");
            }
            return hasAccess;
        }
        #endregion
    }
}
