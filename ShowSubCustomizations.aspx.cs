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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using B24.Common.Web;
using System.Text;
using System.IO;

namespace B24.Sales3.UI
{
  public partial class ShowSubCustomizations : BasePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // Events
      SubmitButton.Command += new CommandEventHandler(SubmitButton_Command);

      if (!IsPostBack)
        ResultsPanel.Visible = false;
      else
        ResultsPanel.Visible = true;
    }

    /// <summary>
    /// Gather the customizations from zorilla
    /// </summary>
    void SubmitButton_Command(object sender, CommandEventArgs e)
    {
      string subID = SubIDTextBox.Text;         // SubID from the form
      string baseURL;                           // Where to go for the xml

      // Get the base url
      if (WebServer.Location == ServerLocation.dev)
        baseURL = WebServer.Name + "/zorilla";
      else if (WebServer.Location == ServerLocation.staging)
        baseURL = "zorilla.qal.itpolecat.com";
      else
        baseURL = "www.books24x7.com";

      using (MemoryStream results = new MemoryStream())
      {
        string xmlPath = String.Format("http://{0}/custom.asp?key={1}", baseURL, subID);
        XPathDocument xpathDoc = new XPathDocument(xmlPath);
        XslCompiledTransform transform = new XslCompiledTransform();
        //Load the XSL stylsheet into the XslCompiledTransform object
        transform.Load(@"d:\sales3\xsl\ShowSubCustomizations.xsl");
        transform.Transform(xpathDoc, null, results);
        using (StreamReader reader = new StreamReader(results))
        {
          results.Position = 0;
          CustomizationsLiteral.Text = reader.ReadToEnd();
          reader.Close();
        }
        results.Close();
      }
    }
  }
}