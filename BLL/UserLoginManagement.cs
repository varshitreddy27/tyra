using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using B24.Common.Web;
using B24.Common;
using B24.Common.Security;

namespace B24.Sales4.BLL
{
    public class UserLoginManagement
    {
        // Private Fields
        #region Private Fields
        private Guid userID;
        private string passwordType;
        private string newPassword;
        private string currentPassword;
        private string userName;
        private Guid requestorID;
        private Guid accessID;
        private Guid sessionID;
        private Sales4.UI.BasePage BasePage;
        private string userConnStr;
        private string errorMessage;
        private bool mustChangePassword;
        private string returnURL;
        bool isPersistent = false;

        #endregion Private Fields

        // Public Properties
        #region pubic properties
        /// <summary>
        /// Uniqe session ID from the Session table 
        /// <remarks>This property should be set during the LoggingIn event</remarks>
        /// </summary>
        public Guid SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        /// <summary>
        /// Contains subscriptionID in the case of anonymous login; otherwise userid
        /// </summary>
        public Guid AccessID
        {
            get { return accessID; }
            set { accessID = value; }
        }

        /// <summary>
        /// Unique user ID from libuser.userid
        /// </summary>
        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// Type of password - temp, partner, and user.
        /// </summary>
        public string PasswordType
        {
            get { return passwordType; }
            set { passwordType = value; }
        }

        /// <summary>
        /// New password
        /// </summary>
        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; }
        }

        /// <summary>
        /// Current (old) password
        /// </summary>
        public string CurrentPassword
        {
            get { return currentPassword; }
            set { currentPassword = value; }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// Unique user ID from libuser.userid
        /// </summary>
        public Guid RequestorID
        {
            get { return requestorID; }
            set { requestorID = value; }
        }

        /// <summary>
        /// Allow to get the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        /// <summary>
        /// Allow to get the return URL
        /// </summary>
        public string ReturnURL
        {
            get { return returnURL; }
        }

        /// <summary>
        /// To communicate that it was a temporary password
        /// </summary>
        public bool MustChangePassword
        {
            get { return mustChangePassword; }
        }
        
        /// <summary>
        /// set base page
        /// </summary>
        //public Sales4.UI.BasePage BasePage
        //{
        //    set { basePage = value; }
        //}

        #endregion

        #region constructors

        //public UserLoginManagement()
        //{
        //}

        public UserLoginManagement(string userConnectionStr, Sales4.UI.BasePage basePage)
        {
            userConnStr = userConnectionStr;
            BasePage = basePage;
        }

        #endregion
        // public Methods
        #region public methods

        public bool LoginUser(string userLogin, string Password)
        {
            bool authenticated = false;
            B24Session session = new B24Session();
            session.ConnectionString = userConnStr;
            try
            {
                authenticated = session.Authenticate(userLogin, Password, BasePage.UserIP, BasePage.AppName);
                if (authenticated)
                {
                    BasePage.SessionID = session.SessionID;
                    // accessID = session.AccessID;
                    userID = session.UserID;
             //       B24Session.WriteNewSessionCookie(session.SessionID);

                    // create user data
                    UserData data = new UserData();
                    data.ConnectionString = BasePage.UserConnStr;
                    data.Load(this.userID);

                    // initialize FormsAuthentication
                    FormsAuthentication.Initialize();
                    // create a new ticket used for authentication
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userLogin, DateTime.Now, DateTime.Now.AddMinutes(60), isPersistent, UserData.B24Serialize(data), FormsAuthentication.FormsCookiePath);
                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    // Create the cookie.
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    // Get the original URL to redirect
                    returnURL = FormsAuthentication.GetRedirectUrl(userLogin, isPersistent);

                    BasePage.SetLoggerProperties();
                }
                else
                {
                    this.errorMessage = "Invalid username or password.";
                }
            }
            catch (SqlException ex)
            {
                authenticated = false;
                switch (ex.Number)
                {
                    case 50130:
                        this.mustChangePassword = true;
                        this.userID = new Guid(ex.Message);     // the error message is the userid
                        this.errorMessage = "You must change your password in order to proceeed.";
                        break;
                    default:
                        this.errorMessage = ex.Message;
                        break;
                }
            }
            return authenticated;
        }

        public bool ChangePassword(string userLogin, string newPassword, string oldPasswrd)
        {
            bool success = false;
            FormsAuthenticationTicket authTicket = null;
            B24Session session = new B24Session();
            session.ConnectionString = userConnStr;
            //HttpCookie saveauthCookie = BasePage.Request.Cookies[B24.Common.Web.BasePage.CacheAutoLoginCookie];
            //if (saveauthCookie != null && saveauthCookie.Value.Length > 0)
            //{  // if we have a saved autologin cookie, we must nuke it otherwise can never login as anyone else.
            //    saveauthCookie.Value = "";
            //    saveauthCookie.Expires = DateTime.Now.AddDays(-1);
            //    BasePage.Request.Cookies.Add(saveauthCookie);
            //    BasePage.Response.Cookies.Add(saveauthCookie);
            //}
            try
            {
                success = session.UpdatePasswordSales4(userLogin, newPassword, oldPasswrd, BasePage.UserIP);
                if (success)
                {   // our session was good!  
                    BasePage.SessionID = session.SessionID;
                    // accessID = session.AccessID;
                    userID = session.UserID;

                    UserData data = new UserData();
                    data.ConnectionString = userConnStr;
                    data.Load(session.UserID);
                    // initialize FormsAuthentication
                    FormsAuthentication.Initialize();
                    // create a new ticket used for authentication
                    authTicket = new FormsAuthenticationTicket(1, userLogin, DateTime.Now, DateTime.Now.AddMinutes(60), isPersistent, UserData.B24Serialize(data), FormsAuthentication.FormsCookiePath);
                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    // Create the cookie.
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    // Get the original URL to redirect
                    returnURL = FormsAuthentication.GetRedirectUrl(userLogin, isPersistent);

                    BasePage.SetLoggerProperties();  

                }
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
            }

            return success;
        }
        #endregion

        
    }
}
