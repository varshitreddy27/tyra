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
using B24.Common.Web;
using B24.Common.Web.Controls;
using B24.Common.Security;
using System.Collections.ObjectModel;
using B24.Common.Logs;
using System.Text.RegularExpressions;

namespace B24.Sales4.UI
{
    public enum LoginView
    {
        NotSet = -1,
        Login = 0,
        ChangePassword = 1,
        ForgotPassword = 2
    }

  public partial class Login : BasePage
  {
    private BasePage basePage;
    private Logger logger = B24.Common.Logs.Logger.GetLogger(B24.Common.Logs.Logger.LoggerType.Authentication);
    BLL.UserLoginManagement userLogin;

    #region protected methods
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            basePage = this.Page as BasePage;
            if (basePage == null)
            {
                return;
            }

            if (!IsPostBack)
            {
                LoginPageMultiView.ActiveViewIndex = (int)B24.Sales4.UI.LoginView.Login;
            }
        }
        catch (NullReferenceException)
        {
            this.basePage.B24Errors.Add(new B24Error(Resources.Resource.ErrorSales4));
            logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorSales4);
        }

   //   B24Principal user = Context.User as B24Principal;

    }

    protected void Login_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid)
        {
            this.basePage.B24Errors.Add(new B24Error(Resources.Resource.RequiredFieldsMissing));
            return;
        }
        if (userLogin == null)
            userLogin = new BLL.UserLoginManagement(basePage.UserConnStr, basePage);
        bool success = userLogin.LoginUser(usernameTbx1.Text.Trim(), passwordTbx.Text.Trim());

        if (success)
        {
            initBasePage();
            if(!String.IsNullOrEmpty(userLogin.ReturnURL))
                Response.Redirect(userLogin.ReturnURL);
        }
        else
        {
            if (userLogin.MustChangePassword == true)
            {
                logger.Log(Logger.LogLevel.Error, "The user has to change password");
                LoginPageMultiView.ActiveViewIndex = (int)B24.Sales4.UI.LoginView.ChangePassword;
                usernameTbx.Focus();

            }
            else
            {
                logger.Log(Logger.LogLevel.Error, userLogin.ErrorMessage);
                B24Errors.Add(new B24.Common.Web.B24Error(userLogin.ErrorMessage));
            }
        }
        
    }


    protected void initBasePage()
    {
        if (userLogin != null && userLogin.UserID != Guid.Empty)
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

    protected void ChangeButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid) // required field validation
        {
            this.basePage.B24Errors.Add(new B24Error(Resources.Resource.RequiredFieldsMissing));
            return;
        } 
        if (IsValidPassword()) // more granular validation
        {
            if (userLogin == null)
                userLogin = new BLL.UserLoginManagement(basePage.UserConnStr, basePage);

            try
            {
                bool success = userLogin.ChangePassword(usernameTbx.Text.Trim(), txtNewPassword.Text.Trim(), tempPasswordTbx.Text.Trim());
                if (success)
                {
                    initBasePage();
                    Response.Redirect(userLogin.ReturnURL);
                }
                else
                {
                    logger.Log(Logger.LogLevel.Error, "Could not change password");
                    B24Errors.Add(new B24.Common.Web.B24Error("Could not change password"));
                }
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, ex.Message);
                B24Errors.Add(new B24.Common.Web.B24Error(ex.Message));
            }
        }
    }

    protected void Login_ForgotPassword(object sender, EventArgs e)
    {
        LoginPageMultiView.ActiveViewIndex = (int) B24.Sales4.UI.LoginView.ForgotPassword;
        EmailAddressTbx.Focus();
    }

    protected void BackToLogin_Click(object sender, EventArgs e)
    {
        LoginPageMultiView.ActiveViewIndex = (int)B24.Sales4.UI.LoginView.Login;
    }

    protected void Login_EmailPassword(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid)
        {
            this.basePage.B24Errors.Add(new B24Error(Resources.Resource.RequiredFieldsMissing));
            return;
        } 
        if (IsValidEmail(EmailAddressTbx.Text.Trim()))
        {
            UserFactory uf = new UserFactory(basePage.UserConnStr);
            List<User> users = new List<User>();

            try
            {
                uf.EmailPassword(EmailAddressTbx.Text.Trim());
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, ex.Message);
                if (ex.Message == "Email account name not found")
                    B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.ForgotPasswordNoEmail));
                else
                    B24Errors.Add(new B24.Common.Web.B24Error("There has been an error processing your request. Please contact tech support."));
             }
        }
        else
        {
            logger.Log(Logger.LogLevel.Error, Resources.Resource.InvalidEmailAddress);
            B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.InvalidEmailAddress));
        }
    }
    #endregion protected methods

      #region private methods
    // Verify the values before changing the password
    private bool IsValidPassword()
    {
        // Verify matching passwords
        if (txtNewPassword.Text.Trim() != txtConfirm.Text.Trim())
        {
           B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.PasswordMatch));

            return false;
        }
        //Verify the password length
        if (txtNewPassword.Text.Trim().Length < 4 )
        {
            B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.PasswordLengthShort));
            return false;
        }   
        else if (txtNewPassword.Text.Trim().Length > 63)
        {
            B24Errors.Add(new B24.Common.Web.B24Error(Resources.Resource.PasswordLengthLong));
            return false;
        }
        return true;
    }

    bool IsValidEmail(string strIn)
    {
        // Return true if strIn is in valid e-mail format.
        return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

      #endregion
  }
}
