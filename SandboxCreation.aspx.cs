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
using B24.Sales3.DAL;

namespace B24.Sales3.UI
{
    public partial class SandboxCreation : BasePage
    {
        private Guid subscriptionId;
        private GlobalVariables global = GlobalVariables.GetInstance();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Grap the subscription id from the querystring or form or state
            GetSubscriptionID();
            InitPopulate();
        }
         private void InitPopulate()
        {
            this.ThisPage = "SandboxCreation.aspx";
            // INGENIOUS SANDBOX
            IngenSandbox1.SubscriptionId = subscriptionId;
            IngenSandbox1.SandBoxCreateButtonView = true;
        }
         #region private Methods
         /// <summary>
         /// get the subscription object by subscriptionId
         /// Todo: in the future when we move to .net subscription page, this method should be in that page or in basepage func
         /// </summary>
         /// <param name="subscriptionId"></param>
         /// <returns></returns>
         private Subscription GetSubscription(Guid subscriptionId)
         {
             SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
             return subscriptionFactory.GetSubscriptionByID(subscriptionId);
         }
         #endregion 
         /// <summary>
         /// Get the sub id from the postback or from the url
         /// </summary>
         /// <returns>A subid guid</returns>
         private void GetSubscriptionID()
         {
             subscriptionId = Guid.Empty;   // The subid to return
             try
             {
                 if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
                 {
                     subscriptionId = new Guid(Request.QueryString["arg"]);
                 }
                 else if (!String.IsNullOrEmpty(Request.Form["arg"]))
                 {
                     subscriptionId = new Guid(Request.Form["arg"]);
                 }
                 else if (!String.IsNullOrEmpty(State["AccountHolderSubId"]))
                 {
                     subscriptionId = new Guid(State["AccountHolderSubID"]);
                 }
                 else
                 {
                     Response.Redirect("Error.aspx?message=Empty subscription id provided");
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

             // Keep the sub id around for post backs...
             State.Add("AccountHolderSubId", subscriptionId.ToString());
         }
    }
}
