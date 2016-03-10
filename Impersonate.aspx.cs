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
using System.Collections.Generic;
using B24.Common;
using B24.Sales4.DAL;

namespace B24.Sales4.UI
{
    public partial class Impersonate : BasePage
    {
        private Guid userId;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GetuserID();
            InitPopulate();
        }
        protected void InitPopulate()
        {
            BasePage p = this.Page as BasePage;

            Impersonate1.UserId = userId;
            Impersonate1.ImpersonateButtonView = true;
        }
        /// <summary>
        /// Get the sub id from the postback or from the url
        /// </summary>
        /// <returns>A subid guid</returns>
        private void GetuserID()
        {
            userId = Guid.Empty;   // The subid to return
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
                {
                    userId = new Guid(Request.QueryString["arg"]);
                }
                else if (!String.IsNullOrEmpty(Request.Form["arg"]))
                {
                    userId = new Guid(Request.Form["arg"]);
                }
                else if (!String.IsNullOrEmpty(State["AccountHolderUserId"]))
                {
                    userId = new Guid(State["AccountHolderUserId"]);
                }
                else
                {
                    Response.Redirect("Error.aspx?message=Empty user id provided");
                }
            }
            catch (FormatException formatException)
            {
                Response.Redirect("Error.aspx?message=" + formatException.Message);
            }
            catch (OverflowException overflowException)
            {
                Response.Redirect("Error.aspx?message=" + overflowException.Message);
            }
            catch (ArgumentException argumentException)
            {
                Response.Redirect("Error.aspx?message=" + argumentException.Message);
            }

            // Keep the user id around for post backs...
            State.Add("AccountHolderUserId", userId.ToString());
        }
    }
}
