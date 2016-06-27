using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;

using B24.Common.Web;



public class GlobalVariables
{
    static GlobalVariables instance;
    string appName = String.Empty;
    string serverIP = String.Empty;
    string userConnStr = String.Empty;
    string assetConnStr = String.Empty;
    string aspAutoLoginCookie = String.Empty;
    private bool akamaiMode;

    private List<string> UserConnections = new List<string>();
    Server webServer = null;
    /// <summary>
    /// How we shold access user data
    /// </summary>
    public string UserConnStr
    {
        get { return userConnStr; }
    }
    public string AppName
    {
        get { return appName; }
    }
    /// <summary>
    /// Where this user comes from
    /// </summary>
    public string UserIP
    {
        get { return GetUserIP(); }
    }
    public string AspAutoLoginCookie
    {
        get
        {
            return "Polecat";
        }
    }

    #region Constructor
    GlobalVariables()
    {
        initialize();
        BuildConnectionStrings();
    }
    #endregion

    #region public methods
    public static GlobalVariables GetInstance()
    {
        lock (typeof(GlobalVariables))
        {
            if (instance == null)
            {
                instance = new GlobalVariables();
            }
            return instance;
        }
    }
    #endregion

    #region private methods
    private void initialize()
    {
        NameValueCollection appSettings = WebConfigurationManager.AppSettings as NameValueCollection;
        appName = appSettings["ApplicationName"];
        serverIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        // Get the server Server from config
        webServer = new Server(serverIP, serverName);

    }
    /// <summary>
    /// Get the user's IP from his HTTPContext 
    /// </summary>
    /// <returns></returns>
    private string GetUserIP()
    {
        string outstr = "";
        // Akamai will place the user's ip in an HTTP_X_FORWARDED_FOR header
        // if the header already exists it will append it in a comma separated
        // list with blank padding
        // e.g. "12.148.253.124, 12.148.253.124, 12.148.253.124"
        if (akamaiMode)
            outstr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        else
            outstr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        return outstr;
    }
    /// <summary>
    /// Build connection strings from the config file
    /// </summary>
    protected void BuildConnectionStrings()
    {
        string appStr = String.Format(";app={0} ({1})", appName, webServer.Name);  // app name to append to connection string
        string key;                                                                // key to use for connection string lookup

        // Configure the asset and user connection strings (required)
        key = "sales3ODBC"; //String.Format("{0}", webServer.Location);
        try
        {
            foreach (ConnectionStringSettings settings in ConfigurationManager.ConnectionStrings)
            {
                if (settings.Name.StartsWith(key))
                {
                    userConnStr = settings.ConnectionString + appStr;
                    assetConnStr = settings.ConnectionString + appStr;
                    UserConnections.Add(userConnStr);
                }
            }
        }
        catch (NullReferenceException)
        {
            throw new ConfigurationErrorsException("You must supply a default connection string in your config file for the current server location: " + webServer.Location.ToString());
        }

        this.SetConnectionStrings();
    }

    private string SetConnectionStrings()
    {
        return this.SetConnectionStrings(null);
    }
    private string SetConnectionStrings(string key)
    {
        if (key == null)
        {
            key = DateTime.Now.ToFileTime().ToString();
        }
        int hash = 0;
        foreach (char c in key)
        {
            hash += (int)c;
        }
        if (this.UserConnections.Count > 0)
        {
            int dbnum = hash % this.UserConnections.Count;
            this.userConnStr = this.UserConnections[dbnum];
        }
        return key;
    }

    #endregion

}

