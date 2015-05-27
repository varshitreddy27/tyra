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


namespace B24.Sales3.UI
{
    public partial class ManagerUserTest : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitPopulate();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void InitPopulate()
        {
            BasePage p = this.Page as BasePage;

            //*** User Detail *******//

            // User details for Login User
            UserDetail1.UserId = User.UserID;  // user suresh 

            // User details for other user 'sureshhhh'
            // UserDetail1.UserId = new Guid("295DED82-0F83-47CB-80E4-7F57C438724C");


            //*** Change Password *******//
            //Properties
            //Userid => ID of the user whose password we want to change
            // RequestorId => User(Admin) making password change request

            //If the user wants to change his password
            //ChangePassword1.UserId = User.UserID; // "87BDCC8B-2A20-4EC7-BFCE-FECCB224C39F" // user suresh 
            //ChangePassword1.RequestorId = User.UserID; // "87BDCC8B-2A20-4EC7-BFCE-FECCB224C39F" // user suresh   

            //If the user(Admin) wants to change other user's password
            ChangePassword1.UserId = new Guid("295DED82-0F83-47CB-80E4-7F57C438724C"); // user sureshhhh
            ChangePassword1.RequestorId = User.UserID; // "87BDCC8B-2A20-4EC7-BFCE-FECCB224C39F" // user suresh 
            

            //*** Impersonation *******//

            // Impersonate the user 'sureshhhh'
             Impersonate1.UserId = new Guid("295DED82-0F83-47CB-80E4-7F57C438724C"); //sureshhhh
        }
    }
}
