

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
    public partial class Cart : BasePage
    {
        private List<CartUser> userCart;                      // List of carted users
        private List<Subscription> subCart;                   // List of carted subs
        private SubscriptionFactory subFactory;               // Factory for manipulating subs
        private CartUserFactory userFactory;                  // Factory for manipulating users
        private int subRowCount;                              // Keep track of how many data rows are being rendered in the sub grid view
        private List<int> userRows;                           // User rows selected from from
        private int subRow;                                   // Sub row selected from form

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check the user has access
            if (!CheckAccess())
            {
                return;
            }
            // Handle Events
            CancelButton.Command += new CommandEventHandler(CancelButton_Command);
            SubmitButton.Command += new CommandEventHandler(SubmitButton_Command);
            SubCartGridView.RowCreated += new GridViewRowEventHandler(SubCartGridView_RowCreated);
            UserCartGridView.RowCreated += new GridViewRowEventHandler(UserCartGridView_RowCreated);

            if (!(IsSupport || IsSkillsoft))
                throw new Exception("You do not have permission to cart users / subs");

            // Gather form data
            userRows = LoadUserRows();
            subRow = (String.IsNullOrEmpty(Request.Form["SubGroup"])) ? -1 : Convert.ToInt32(Request.Form["SubGroup"]);

            // Initialize the factories
            subFactory = new SubscriptionFactory(base.Sales3UserConnStr);
            userFactory = new CartUserFactory(base.Sales3UserConnStr);

            // Load the list of carted users and subs
            subRowCount = 0;
            subCart = LoadCartedSubs();
            SubCartGridView.DataSource = subCart;
            SubCartGridView.DataBind();

            userCart = LoadCartedUsers();
            UserCartGridView.DataSource = userCart;
            UserCartGridView.DataBind();

        }

        /// <summary>
        /// Carry out the selected task
        /// </summary>
        void SubmitButton_Command(object sender, CommandEventArgs e)
        {
            if (TaskList.SelectedIndex > -1)
            {
                switch (TaskList.Items[TaskList.SelectedIndex].Value)
                {
                    case "Move":
                        MoveUsers();
                        break;
                    case "EmptyUsers":
                        EmptyUsers();
                        break;
                    case "EmptyCart":
                        EmptyCart();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                B24Errors.Add(new B24.Common.Web.B24Error("Please choose an action and re-submit"));
            }
        }

        /// <summary>
        /// Empty the selected users from the cart
        /// </summary>
        private void EmptyUsers()
        {
            string userCartStr = "";          // the string to put back in the state
            int count = 0;                    // count of users removed
            for (int i = 0; i < UserCartGridView.Rows.Count; i++)
            {
                if (!userRows.Contains(i))
                {
                    userCartStr += String.Format("{{{0}}},", UserCartGridView.DataKeys[i].Value.ToString());
                }
                else
                {
                    count++;
                }
            }
            if (userCartStr.Length > 0)
                userCartStr = userCartStr.Remove(userCartStr.Length - 1, 1);      // remove the final comma
            // Put it in the state
            State.Add("userCart", userCartStr.ToUpper());       // uppercase it to play nice with asp pages
            State.Add("errmsg", String.Format("{0} users emptied from cart.", count));
            B24Redirect(ThisPage);
        }

        /// <summary>
        /// Load the selected user rows from the form
        /// </summary>
        /// <returns>List of selected user row indices</returns>
        List<int> LoadUserRows()
        {
            List<int> rows = new List<int>();
            if (!String.IsNullOrEmpty(Request.Form["UserGroup"]))
            {
                string[] rowStrings = Request.Form["UserGroup"].Split(',');
                for (int i = 0; i < rowStrings.Length; i++)
                {
                    rows.Add(Convert.ToInt32(rowStrings[i]));
                }
            }
            return rows;
        }

        /// <summary>
        /// Grab each user cart row and add a checkbox
        /// </summary>
        void UserCartGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal l = e.Row.FindControl("UserCheckboxMarkup") as Literal;
                if (l != null)
                {
                    l.Text = String.Format("<input type=checkbox name=UserGroup id=RowSelector{0} value={0}", e.Row.RowIndex);
                }

                if (userRows.Contains(e.Row.RowIndex))
                    l.Text += " checked=checked";

                // close the tag
                l.Text += " />";
            }
        }


        /// <summary>
        /// Grab each sub cart row and add a radio button...
        /// </summary>
        void SubCartGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal l = e.Row.FindControl("SubRadioButtonMarkup") as Literal;
                if (l != null)
                {
                    l.Text = String.Format("<input type=radio name=SubGroup id=RowSelector{0} value={0}", e.Row.RowIndex);
                }

                // See if we need to add the "checked" attribute (row is selected or none selected and this is the first row
                if (subRow == e.Row.RowIndex || (subRowCount == 0 && subRow == -1))
                    l.Text += " checked=checked";

                // close the tag
                l.Text += " />";

                subRowCount++;
            }
        }

        /// <summary>
        /// Load a list of subscriptions from subs carted in the state
        /// </summary>
        /// <returns>List of subscriptions</returns>
        private List<Subscription> LoadCartedSubs()
        {
            List<Subscription> list = new List<Subscription>();

            string subCart = State["subCart"];
            if (subCart != null)
            {
                string[] idArray = subCart.Split(',');
                foreach (string id in idArray)
                {
                    try
                    {
                        list.Add(subFactory.GetSubscriptionByID(new Guid(id)));
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        string msg = String.Format("Error loading sub ({0}): {1}", id, ex.Message);
                        B24Errors.Add(new B24.Common.Web.B24Error(msg));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Load a list of users from users carted in the state
        /// </summary>
        /// <returns>List of users</returns>
        private List<CartUser> LoadCartedUsers()
        {
            List<CartUser> list = new List<CartUser>();

            string userCart = State["userCart"];
            if (userCart != null)
            {
                string[] idArray = userCart.Split(',');
                foreach (string id in idArray)
                {
                    try
                    {
                        list.Add(userFactory.GetUserByID(new Guid(id)));
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        string msg = String.Format("Error loading user ({0}): {1}", id, ex.Message);
                        B24Errors.Add(new B24.Common.Web.B24Error(msg));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Actually move the users to the selected sub
        /// </summary>
        private void MoveUsers()
        {
            // Get the selected Subscription
            if (subRow == -1)
            {
                B24Errors.Add(new B24.Common.Web.B24Error("No subscription selected"));
                return;
            }
            Guid subID = new Guid(SubCartGridView.DataKeys[subRow].Value.ToString());

            // Verify the target subscription
            if (!ValidateSub(subID))
            {
                B24Errors.Add(new B24.Common.Web.B24Error("Target sub must be B24Library"));
                return;
            }
            if (IsInGenSub(subID))
            {
                B24Errors.Add(new B24.Common.Web.B24Error("Target sub shouldn't be Ingenius enabled sub"));
                return;
            }

            int count = 0;
            for (int i = 0; i < UserCartGridView.Rows.Count; i++)
            {
                if (userRows.Contains(i))
                {
                    Guid userID = new Guid(UserCartGridView.DataKeys[i].Value.ToString());
                    try
                    {
                        B24.Common.User u = userFactory.GetUserByID(userID);
                        if (ValidateSub(u.SubscriptionID))
                        {
                            userFactory.MoveUserToNewSub(subID, userID, User.UserID);
                            count++;
                        }
                        else
                        {
                            string msg = String.Format("Error moving user ({0}): The user's subscription is not B24Library or library", u.BaseLogin);
                            B24Errors.Add(new B24.Common.Web.B24Error(msg));
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        string msg = String.Format("Error moving user ({0}): {1}", userID, ex.Message);
                        B24Errors.Add(new B24.Common.Web.B24Error(msg));
                    }
                }
            }
            B24Errors.Add(new B24.Common.Web.B24Error(String.Format("Moved {0} users<br>", count)));
            // Re-load the users with their new sub id's.
            userCart = LoadCartedUsers();
            UserCartGridView.DataSource = userCart;
            UserCartGridView.DataBind();
        }

        /// <summary>
        /// Verify that the target subscription is valid (e.g., is b24library)
        /// </summary>
        /// <param name="subID">The subid to verify</param>
        /// <returns>True if target is valid; false otherwise</returns>
        private bool ValidateSub(Guid subID)
        {
            Subscription s = subFactory.GetSubscriptionByID(subID);
            bool isValid = false;
            isValid = s.ApplicationName.ToLower().Contains("library") || s.ApplicationName.ToLower().Contains("b24library");

            return isValid;
        }
        private bool IsInGenSub(Guid subID)
        {
            bool isInGenSub = false;
            SubscriptionFlagFactory subFlagFactory = new SubscriptionFlagFactory(base.Sales3UserConnStr);
            List<SubscriptionFlag> subFlagList = subFlagFactory.GetActiveSubscriptionFlags(subID);
            if (subFlagList != null && subFlagList.Count > 0)
            {
                foreach (SubscriptionFlag subFlag in subFlagList)
                {
                    if (subFlag.FlagID == 39)  // 30 is magic number for ingenius flag
                    {
                        isInGenSub = true;
                        break;
                    }
                }
            }
            return isInGenSub;
        }

        /// <summary>
        /// Empty the users and subscriptions from the cart.
        /// </summary>
        private void EmptyCart()
        {
            State.Remove("userCart");
            State.Remove("subCart");
            State.Add("errMsg", "Cart is now empty");
            B24Redirect(ThisPage);
        }

        /// <summary>
        /// Upon cancelling, return the user to the home page
        /// </summary>
        void CancelButton_Command(object sender, CommandEventArgs e)
        {
            B24Redirect("Home.aspx");
        }

        /// <summary>
        /// Check the user has access to the selected module and submodule
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            bool hasAccess = true;
            if (!CheckUserAccess(Sales4Module.ModuleCart, 0))
            {
                hasAccess = false;
                AccessDeniedErrorLabel.Visible = true;
                CartPanel.Visible = false;
            }
            return hasAccess;
        }
    }
}