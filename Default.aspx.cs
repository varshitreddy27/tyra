using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace B24.Sales4.UI
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx", true);
//            if(reportsmode)
//  B24Redirect("advancedreport.asp")
//else
//  B24Redirect("home.aspx")
        }
    }
}
