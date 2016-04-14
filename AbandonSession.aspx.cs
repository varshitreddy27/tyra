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
using B24.Common.Web;

namespace B24.Sales4.UI
{
  public partial class AbandonSession : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
        // Clear State
      State.Remove("errMsg");
      // Kill the .Net session and delete the "ticket" (cookie)
      FormsAuthentication.SignOut();
      HttpCookie cookie = this.Context.Response.Cookies[FormsAuthentication.FormsCookieName];
      cookie.Expires = DateTime.Now.AddDays(-1);
      // The signout above leaves the IsAuthenticated property set, so redirect to make the change take effect
      if (User != null && User.Identity.IsAuthenticated)
        B24Redirect(ThisPage);
    }
  }
}