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
using B24.Common.Web;
using B24.Common.Web.Controls;
using B24.Common.Security;
using System.Collections.Generic;

namespace B24.Sales3.UI
{
    public partial class BaseMaster : System.Web.UI.MasterPage
    {
        #region Public Members
        /// <summary>
        /// Calling page can hide the submodule menu if there are no sub module found for the selected module
        /// </summary>
        public Panel SubMenuPanel
        {
            get
            {
                return this.SubModuleMenuPanel;
            }
        }

        public Menu MainMenu
        {
            get
            {
                return this.ModuleMenu;
            }
        }

        public Menu SubMenu
        {
            get
            {
                return this.SubModuleMenu;
            }
        }
        #endregion

        #region Private Member

        /// <summary>
        ///  Get a reference to the page as a BasePage object
        /// </summary>
        private BasePage page;
        /// <summary>
        /// Get the welcome string
        /// </summary>
        private string welcomeString;

        private string cartText = "Cart (empty)";
        private string cartToolTip = string.Empty;
        #endregion

        #region Protected Events

        protected void Page_Load(object sender, EventArgs e)
        {
            B24Principal user = Context.User as B24Principal;
            welcomeString = (user != null && user.Identity.IsAuthenticated) ? String.Format("{0} {1}", user.FirstName, user.LastName) : "";
            page = this.Page as BasePage;
            WelcomeUser.Text = welcomeString;
            if (!IsPostBack)
            {
                IntializeControl();
            }
        }

        /// <summary>
        /// On menu item click redirect to appropriate page with querystring values
        /// </summary>
        protected void ModuleMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            string redirectPage = "home.aspx";
            switch (int.Parse(ModuleMenu.SelectedItem.Value))
            {
                case Sales3Module.ModuleInfo:
                    {
                        redirectPage = "Home.aspx";
                        break;
                    }
                case Sales3Module.ModuleManageAccounts:
                    {
                        redirectPage = "ManageAccount.aspx";
                        break;
                    }
                case Sales3Module.ModuleManageUsers:
                    {
                        redirectPage = "ManageUser.aspx";
                        break;
                    }
                case Sales3Module.ModuleCreateNewTrial:
                    {
                        redirectPage = "LibraryTrial.aspx";
                        break;
                    }
                case Sales3Module.ModuleGeneralReporting:
                    {
                        redirectPage = "GeneralReport.aspx";
                        break;
                    }
                case Sales3Module.ModuleCart:
                    {
                        redirectPage = "Cart.aspx";
                        break;
                    }
            }
            redirectPage += "?module=" + ModuleMenu.SelectedItem.Value;
            Response.Redirect(redirectPage);
        }

        /// <summary>
        /// User logout
        /// </summary>
        protected void Sales3LoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Response.Redirect("AbandonSession.aspx");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set the module menu bar values 
        /// </summary>
        private void IntializeControl()
        {
            int module = Sales3Module.ModuleInfo;
            if (!string.IsNullOrEmpty(Request.QueryString["module"]))
            {
                module = int.Parse(Request.QueryString["module"]);
            }
            RoleBasedAccessFactory access = new RoleBasedAccessFactory(page.UserConnStr);
            access.UserId = page.User.UserID;
            List<RoleBasedAccess> modules = access.GetModule(page.ApplicationId);
            SetCartLabel();
            MenuItem item = null;
            foreach (RoleBasedAccess mod in modules)
            {
                switch (mod.ModuleId)
                {
                    case Sales3Module.ModuleInfo:
                        {
                            item = new MenuItem("Start", mod.ModuleId.ToString());
                            break;
                        }
                    case Sales3Module.ModuleManageAccounts:
                        {
                            item = new MenuItem("Manage Account", mod.ModuleId.ToString());
                            break;
                        }
                    case Sales3Module.ModuleManageUsers:
                        {
                            item = new MenuItem("Manage Users", mod.ModuleId.ToString());
                            break;
                        }
                    case Sales3Module.ModuleCreateNewTrial:
                        {
                            item = new MenuItem("Create New Trial", mod.ModuleId.ToString());
                            break;
                        }
                    case Sales3Module.ModuleGeneralReporting:
                        {
                            item = new MenuItem("General Reporting", mod.ModuleId.ToString());
                            break;
                        }
                    case Sales3Module.ModuleCart:
                        {
                            item = new MenuItem(cartText, mod.ModuleId.ToString());
                            item.ToolTip = cartToolTip;
                            break;
                        }
                }
                if (item != null)
                {
                    if (mod.ModuleId == module)
                    {
                        item.Selected = true;
                    }
                    ModuleMenu.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Structure cart label text
        /// </summary>
        /// <returns></returns>
        private void SetCartLabel()
        {
            
            int userCartCount = 0;
            int subCartCount = 0;
            if (!string.IsNullOrEmpty(page.State["userCart"]))
            {
                userCartCount = page.State["userCart"].Split(',').Length;
            }
            if (!string.IsNullOrEmpty(page.State["subCart"]))
            {
                subCartCount = page.State["subCart"].Split(',').Length;
            }

            if (userCartCount == 0 && subCartCount == 0)
            {
                cartText = "Cart (empty)";
                cartToolTip = "empty";
            }
            else
            {
                cartText = "Cart (" + userCartCount + "," + subCartCount + ")";
                cartToolTip = userCartCount + " user(s), " + subCartCount + " subscription(s)";
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Update Cart Text when user checks cart on UserLookup & Account Lookup page 
        /// </summary>
        public void UpdateCartText()
        {
            SetCartLabel();
            MenuItem CartMenu = ModuleMenu.FindItem(Sales3Module.ModuleCart.ToString());
            if (CartMenu != null)
            {
                CartMenu.Text = cartText;
                CartMenu.ToolTip = cartToolTip;
                MenuUpdatePanel.Update();
            }
        }
        #endregion
    }
}
