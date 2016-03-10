using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Text;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// User control to impersonate.
    /// Generetes ticket and login to the main application Books24*7
    /// </summary>
    public partial class Impersonate : System.Web.UI.UserControl
    {
        #region Private Members
        private B24.Common.Web.BasePage basePage;
        private Logger logger;
        private Guid subscriberId;
        private string displayLogin;
        private string applicationName;
        private string baseDir;
        private string baseUrl;
        private string loginTicket;
        private bool hasPermission = true;
        #endregion

        #region Public Property
        /// <summary>
        /// User to create the login ticket.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Show/hide header title text
        /// </summary>        
        public bool ShowHeaderText { set { HeaderText.Visible = value; } }

        public bool ImpersonateButtonView { get; set; }
        #endregion

        #region protected methods

        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as BasePage;

            if (basePage == null)
            {
                return;
            }
            if (false == IsPostBack)
            {
                ImpersonateButton.Visible = ImpersonateButtonView;
                if (UserId == Guid.Empty)
                {
                    ImpersonateError.Text = Resources.Resource.ErrorLoadControl;
                    ImpersonateError.Visible = true;

                    ImpersonateButton.Enabled = false;
                }
                else
                {
                    UserIdHidden.Value = UserId.ToString();
                }
            }
        }
        #endregion

        #region Private Methods

        // Load the User details
        private void LoadSubscriber()
        {
            UserId = new Guid(UserIdHidden.Value);

            UserFactory userFactory = new UserFactory(basePage.UserConnStr);
            User user = userFactory.GetUserByID(UserId);
            displayLogin = user.BaseLogin.Trim();
            applicationName = user.ApplicationName.Trim();
            subscriberId = user.UserID;
        }

        // Load and verify User's permissions
        // Couldn't impersonate if the user is a super user or sales manager or general admin
        private bool VerifyPermission()
        {
            PermissionFactory permFactory = new PermissionFactory(basePage.UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(subscriberId, basePage.User.Identity.Name, String.Empty);
            if (permissions.SuperUser == 1 || permissions.SalesManager == 1 || permissions.GeneralAdmin == 1)
            {
                // The user is not having permission to impersonate.
                hasPermission = false;

                ImpersonateError.Text = Resources.Resource.ImpersonatePermission;
                ImpersonateError.Visible = true;

                return false;
            }
            return true;
        }

        // Load the application
        private bool LoadApplication()
        {
            if (applicationName.Length > 0)
            {
                ApplicationFactory appFactory = new ApplicationFactory(basePage.UserConnStr);
                Application application = appFactory.GetApplication(applicationName);
                // Find the application support tickets
                switch (applicationName.ToLower())
                {
                    case "b24library":
                        baseDir = "zorilla.net";
                        break;
                    case "skillport":
                    case "smartforce":
                    case "cigna":
                    case "teckskills":
                    default:
                        // we are going to allow all app to be impersonated
                        baseDir = applicationName;
                        // Other apps don't support tickets.
                       // hasPermission = false;
                        break;
                }

                if (basePage.WebServer.Location == ServerLocation.dev)
                {
                    baseUrl = "";
                    //baseUrl = "http://patty/";//added for testing
                }
                else
                {
                    baseUrl = application.BaseUrl.Trim();
                    // in other environment url will be just baseurl
                    baseDir = String.Empty;
                }

    
            }
            if (!hasPermission)
            {
                ImpersonateError.Text = Resources.Resource.ImpersonateAppError;
                ImpersonateError.Visible = true;
                return false;
            }
            return true;
        }

        // Generate an encrypted ticket
        private void GenerateLoginTicket()
        {
            // If the subscriber has permission
            if (hasPermission)
            {
                LoginTicket ticket = new LoginTicket();
                ticket.Userid = UserId;
                if (!String.IsNullOrEmpty(displayLogin))
                {
                    ticket.Login = displayLogin;
                }
                if (!String.IsNullOrEmpty(applicationName))
                {
                    ticket.AppName = applicationName;
                }
                loginTicket = ticket.Save();
            }
        }
        // Constrcut and redirect the URL into the main site
        private void RedirectURL()
        {
            if (hasPermission)
            {
                string loginURL;

                if (baseUrl.EndsWith("/"))
                {

                    if (String.IsNullOrEmpty(baseDir))
                    {
                        loginURL = baseUrl + "authenticate.asp?ic=1&ticket=" + loginTicket;
                    }
                    else
                    {
                        loginURL = baseUrl + baseDir + "/authenticate.asp?ic=1&ticket=" + loginTicket;
                    }
                }
                else
                {

                    if (String.IsNullOrEmpty(baseDir))
                    {
                        loginURL = baseUrl + "/authenticate.asp?ic=1&ticket=" + loginTicket;
                    }
                    else
                    {

                        loginURL = baseUrl + "/" + baseDir + "/authenticate.asp?ic=1&ticket=" + loginTicket;
                    }
                }

                StringBuilder strScript = new StringBuilder();
                strScript.Append("var w = screen.width, h = screen.height;");
                strScript.Append("window.open('" + loginURL + "','_blank','width=\' + w + \', height=\' + h + \',left=0,top=0,location=yes,resizable=yes,menubar=yes,toolbar=yes,status=yes');");


                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "subscribescript" + DateTime.Now.Ticks, strScript.ToString(), true);
            }
        }

        #endregion
        protected void ImpersonateButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSubscriber();

                // If the subscriber's permission verified
                if (VerifyPermission())
                {
                    if (LoadApplication())
                    {
                        GenerateLoginTicket();
                        RedirectURL();
                    }
                }
            }
            catch (Exception ex)
            {
                ImpersonateError.Text = Resources.Resource.ImpersonateFail;
                ImpersonateError.Visible = true;

                logger = new Logger(Logger.LoggerType.UserInfo);
                logger.Log(Logger.LogLevel.Error, "Impersonation", ex);
            }
        }
    }
}
