using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using B24.Common.Web;
using B24.Common.Web.Controls;
using B24.Common;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace B24.Sales4.UI
{
  public class BasePage : B24.Common.Web.BasePage
  {
    private StringDictionary salesGroupDict;                  // Dictionary of user's sales groups
    private bool isSupport;
    private bool isSkillsoft;
    private int applicationId = 0;
    GlobalVariables globals = GlobalVariables.GetInstance();
    private B24.Common.Logs.Logger logger = null;
 

    public StringDictionary SalesGroupDict
    {
      get { return salesGroupDict; }
      set { salesGroupDict = value; }
    }

    /// <summary>
    /// User is a member of the FREDSupport sales group?
    /// </summary>
    public bool IsSupport
    {
      get { return isSupport; }
      set { isSupport = value; }
    }

    /// <summary>
    /// Indicates that the logged in user has an email of 'books24x7.com' or 'skillsoft.com'
    /// </summary>
    public bool IsSkillsoft
    {
      get { return isSkillsoft; }
      set { isSkillsoft = value; }
    }

    public override string AppName
    {
        get { return GetLoginAppName(); }
    }
    public  string Sales4UserConnStr
    {
        get 
        { 
            return base.UserConnStr;
        }
    }
    public  string Sales4AssetConnStr
    {
        get { 
            return base.AssetConnStr;
        }
        }
      public string IGWSBaseUri
      { 
          get
          {
              return  ConfigurationManager.AppSettings["IGEWSbaseuri"].ToString();
          }
      }
      public Guid SandboxRequesterId
      {
          get{
              return new Guid(ConfigurationManager.AppSettings["B24IGEAdmin"].ToString());
          }
      }

      public B24.Common.Logs.Logger Logger
      {
          get { return logger; }
      }


      public int ApplicationId
      {
          get
          {
              RoleBasedAccessFactory roleAccessFactory = new RoleBasedAccessFactory(this.UserConnStr);
              applicationId = roleAccessFactory.GetApplicationID(ConfigurationManager.AppSettings["ApplicationName"]);
              if (applicationId == 0)
              {
                  B24Errors.Add(new B24.Common.Web.B24Error("Application id not configured for this application. Contact administrator"));
              }
              return applicationId;
          }
      }

    /// <summary>
    /// Load / Initialize the base page
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
      // Initialize the base page properties (e.g., servername, userip, default appname, etc.);
      Initialize();

      // Get  the branding cookie
      BrandEm();

      //Response.Write("serverlocation: " + String.Format("{0}", this.WebServer.Location) + "<br>");
      //Response.Write("UserConnStr: " + this.UserConnStr + "<br>");
      //Response.Write("AppName:" + this.AppName + "<br>");
      //foreach (ConnectionStringSettings settings in ConfigurationManager.ConnectionStrings)
      //{
      //    if (settings.Name.StartsWith(String.Format("{0}", this.WebServer.Location)))
      //    {
      //        Response.Write("UserConnStr from setting: " + settings.ConnectionString + "<br>");
      //        break;
      //    }
      //}

      // Get a State object using the branding cookie GUID
      State = GetState((Guid)BrandingGuid);

      // Initialize the errors collection 
      B24Errors = new B24ErrorCollection();

      // Load salesgroups for this user
      salesGroupDict = new StringDictionary();
      if (User != null && User.Identity.IsAuthenticated)
      {
        salesGroupDict = LoadSalesGroupDictionary();
      }
      isSupport = salesGroupDict.ContainsKey("fredsup");
      isSkillsoft = (User != null && User.Identity.IsAuthenticated && User.Email!=null && (User.Email.ToLower().Contains("@books24x7.com") || User.Email.ToLower().Contains("@skillsoft.com")));

      //--- Register some client scripts ---
      // Fix FireDefaultButton script for FireFox (http://blog.codesta.com/codesta_weblog/2007/12/net-gotchas---p.html)
      ClientScript.RegisterClientScriptInclude("FixFireDefault", Page.ResolveUrl("~/scripts/FireDefaultButtonFix.js"));

      base.OnLoad(e);
    }

    /// <summary>
    /// Adjust the app name according to the url
    /// </summary>
    private string GetLoginAppName()
    {
      string app;                     // The app to return
      string loginApp = "";           // The app derived from the url or servername
      Match match;                    // Regular expression match

      // Try to get the app from the servername (e.g., evc.books24x7.biz)
      string server = Request.ServerVariables["SERVER_NAME"];
      match = Regex.Match(server, @"([^\.]+)\.", RegexOptions.IgnoreCase);
      //Response.Write("server: "+server+"<br>");
      if (match.Success)
      {
        loginApp = match.Groups[1].ToString();
      }
      else
      {
        // Try to get the app from the virtual directory in the URL (e.g., /evc/login.aspx)
        string url = Request.ServerVariables["URL"];
        //Response.Write("url: " + url+ "<br>");
        match = Regex.Match(url, @"/([^\./]+)/", RegexOptions.RightToLeft | RegexOptions.IgnoreCase);
        if (match.Success)
        {
          loginApp = match.Groups[1].ToString();
        }
      }

      //Response.Write("loginapp: " + loginApp + "<br>");
      switch (loginApp)
      {
        case "evc":
          app = "evc";
          break;
        case "microsofteref":
          app = "microsofteref";
          break;
        case "microsofteref3":
          app = "microsofteref3";
          break;
        case "accenture":
          app = "AccentureSCA";
          break;
        case "cbtdirect":
          app = "SmartCertify";
          break;
        case "library":
          app = "library";
          break;
        case "giltec":
          app = "giltec";
          break;          
        default:
          //app = "b24library";
          GlobalVariables global = GlobalVariables.GetInstance();
          app = global.AppName;

          break;
      }
      //Response.Write("app: " + app + "<br>");
      return app;
    }

    /// <summary>
    /// Load all the user's sales groups into a dictionary object
    /// </summary>
    /// <remarks>
    /// This is a hack until we enable roles properly
    /// </remarks>
    /// <returns>A string dictionary loaded with the user's salesgroups</returns>
    private StringDictionary LoadSalesGroupDictionary()
    {
      StringDictionary dict = new StringDictionary();       // The dictionary to return

      try
      {
        using (SqlConnection conn = new SqlConnection(Sales4UserConnStr))
        {
          SqlCommand cmd = new SqlCommand("B24_GetSalesGroupsForUser", conn);
          cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = User.UserID;
          cmd.CommandType = CommandType.StoredProcedure;
          conn.Open();
          SqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            string key = reader["ResellerCode"].ToString().ToLower();
            if (!dict.ContainsKey(key))
              dict.Add(key, null);
          }
          conn.Close();
        }
      }
      catch (SqlException ex)
      {
        string msg = String.Format("Failed to load salesgroups: {0}", ex.Message);
        B24Errors.Add(new B24.Common.Web.B24Error(msg));
      }

      return dict;
    }


    /// <summary>
    /// All events complete, save state and show any errors, etc.
    /// </summary>
    protected override void OnLoadComplete(EventArgs e)
    {
      // Check to see if an exception was captured on the previous request.
      GetLastError();

      // Save State
      Guid currentSessionId = base.GetSessionID();
      if (currentSessionId == Guid.Empty)
      {
          if (HttpContext.Current.Request.Cookies[BasePage.SessionCookie] != null)
          {
              HttpCookie sessionCookie = HttpContext.Current.Request.Cookies[BasePage.SessionCookie];
              if (!String.IsNullOrEmpty(sessionCookie.Value) && sessionCookie.Expires > DateTime.Now)
                  currentSessionId = new Guid(Server.UrlDecode(sessionCookie.Value));
          }

      }
      if (currentSessionId != Guid.Empty)
      {
          SaveState(State, (Guid)BrandingGuid, currentSessionId);
      }
     // SaveState(State, (Guid)BrandingGuid, SessionID);

      // Show any page errors
      Label errorLabel = Utils.FindControlRecursive(this.Master, "ErrorLabel") as Label;
      if (errorLabel != null)
      {
          errorLabel.Text = B24Errors.ToHTMLString();
      }

      base.OnLoadComplete(e);
    }


    public Guid GetSandboxRequesterId(bool isB24)
    {
        if (isB24)
        {
            return new Guid(ConfigurationManager.AppSettings["B24IGEAdmin"].ToString());
        }
        else
        {
            return new Guid(ConfigurationManager.AppSettings["SkillSoftIGEAdmin"].ToString());
        }
    }

    public bool CheckUserAccess(int moduleId, int subModuleId)
    {
        bool hasAccess = false;
        RoleBasedAccessFactory roleAccessFactory = new RoleBasedAccessFactory(this.UserConnStr);
        roleAccessFactory.UserId = this.User.UserID;
        List<RoleBasedAccess> roleAccess = roleAccessFactory.GetUserRoleManagementInfo(ApplicationId);
        if (moduleId > 0 && subModuleId > 0)
        {
            if (roleAccess.Where(item => item.ModuleId == moduleId && item.SubModuleId == subModuleId).ToList().Count > 0)
            {
                hasAccess = true;
            }
        }
        else if (moduleId > 0)
        {
            if (roleAccess.Where(item => item.ModuleId == moduleId).ToList().Count > 0)
            {
                hasAccess = true;
            }
        }
        return hasAccess;
    }

    public void SetLoggerProperties()
    {

        B24.Common.Logs.Logger.AppName = this.globals.AppName;
        B24.Common.Logs.Logger.RequestUrl = String.Empty;

        string serverIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

        Server webServer = new Server(serverIP, serverName);
        B24.Common.Logs.Logger.Server = webServer;

        // All above properties must be set before instantiating the logger.
        logger = B24.Common.Logs.Logger.GetLogger(B24.Common.Logs.Logger.LoggerType.Sales4);

    }

   }
}

