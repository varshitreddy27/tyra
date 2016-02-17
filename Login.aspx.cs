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
using B24.Common;
using B24.Common.Web;
using B24.Common.Web.Controls;
using B24.Common.Security;

namespace B24.Sales3.UI
{
  public partial class Login : BasePage
  {
    private BasePage basePage;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            basePage = this.Page as BasePage;
            if (basePage == null)
            {
                return;
            }
        }
        catch (NullReferenceException)
        {
            this.basePage.B24Errors.Add(new B24Error(Resources.Resource.ErrorSales3));
        }
      // Redirect the user to the home page if logged in already
      B24Principal user = Context.User as B24Principal;
      if (user != null && user.Identity.IsAuthenticated)
      {
          Response.Redirect("home.aspx");   
      }
    }
    protected void B24Login_LoggedIn(object sender, EventArgs e)
    {
        if (B24Login.UserID != Guid.Empty)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["LibraryTrialSalesGroup1"]) && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["LibraryTrialSalesGroup2"])
                 && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SupportSalesGroup"]))
            {

                if (!basePage.State.ContainsKey("LibraryTrialSalesGroup1"))
                {
                    basePage.State.Add("LibraryTrialSalesGroup1", ConfigurationManager.AppSettings["LibraryTrialSalesGroup1"].ToString());
                }
                else
                {
                    basePage.State["LibraryTrialSalesGroup1"] = ConfigurationManager.AppSettings["LibraryTrialSalesGroup1"].ToString();
                }
                if (!basePage.State.ContainsKey("LibraryTrialSalesGroup2"))
                {
                    basePage.State.Add("LibraryTrialSalesGroup2", ConfigurationManager.AppSettings["LibraryTrialSalesGroup2"].ToString());
                }
                else
                {
                    basePage.State["LibraryTrialSalesGroup2"] = ConfigurationManager.AppSettings["LibraryTrialSalesGroup2"].ToString();
                }
            }
            //adds the key value for SupportSalesGroup(Support plus TI salesgroup) in the state object
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SupportSalesGroup"]))
            {
                if (!basePage.State.ContainsKey("SupportSalesGroup"))
                {
                    basePage.State.Add("SupportSalesGroup", ConfigurationManager.AppSettings["SupportSalesGroup"].ToString());
                }
                else
                {
                    basePage.State["SupportSalesGroup"] = ConfigurationManager.AppSettings["SupportSalesGroup"].ToString();
                }
            }
        }
    }
}
}
