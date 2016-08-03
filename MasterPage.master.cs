using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using B24.Common.Security;
using System.Text.RegularExpressions;
using B24.Common;
using B24.Common.Web;

namespace B24.Sales4.UI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected string welcomeString;
        private BasePage page;               // Get a reference to the page as a BasePage object

        protected void Page_Load(object sender, EventArgs e)
        {
            B24Principal user = Context.User as B24Principal;
            welcomeString = (user != null && user.Identity.IsAuthenticated) ? String.Format("{0} {1}", user.FirstName, user.LastName) : "";
            page = this.Page as BasePage;
            BuildNumberLabel.Text = "v" + page.BuildNumber;
        }

        /// <summary>
        /// Update controls on this page before rendering
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }



        /// <summary>
        /// Update the login link with either login or logout depending upon user status
        /// </summary>
        /// <param name="item"></param>
        private void UpdateLoginItem(MenuItem item)
        {
            string text = "Login";                                        // The text of the menu item
            string url = FormsAuthentication.LoginUrl;                    // Url of the menu item

            B24Principal user = this.Page.User as B24Principal;
            if (user != null && user.Identity.IsAuthenticated)
            {
                text = "Logout";
                url = "AbandonSession.aspx";
            }

            item.Text = text;
            item.NavigateUrl = url;
        }

        /// <summary>
        /// Update the Cart Menu item label with the number of users and subs in the state
        /// </summary>
        /// <param name="item">The cart menu item to update</param>
        private void UpdateCartItem(MenuItem item)
        {
            BasePage page = this.Page as BasePage;

            if (page != null && page.State != null && page.User != null && page.User.Identity.IsAuthenticated)
            {
                string userCart = page.State["userCart"];
                string subCart = page.State["subCart"];
                int userCount = (userCart != null) ? userCart.Split(',').Length : 0;
                int subCount = (subCart != null) ? subCart.Split(',').Length : 0;
                if (userCount > 0 || subCount > 0)
                {
                    item.Text = String.Format("Cart ({0}, {1})", userCount, subCount);
                    item.ToolTip = String.Format("{0} user(s), {1} sub(s)", userCount, subCount);
                }
                else
                {
                    item.Text = "Cart (empty)";
                }
            }
        }

    }
}