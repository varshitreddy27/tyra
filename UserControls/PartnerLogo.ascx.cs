using System;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.Configuration;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;

namespace B24.Sales3.UserControl
{
    /// <summary>
    /// Partner logo upload
    /// </summary>
    public partial class PartnerLogo : System.Web.UI.UserControl
    {
        #region Private members
        private Sales3.UI.BasePage basePage;

        #endregion

        #region Public Properties
        /// <summary>
        /// Password Root
        /// </summary>
        public string PassRoot { get; set; }
        /// <summary>
        /// To set Read only View or Edit View
        /// </summary>
        public bool EditButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as Sales3.UI.BasePage;

            if (string.IsNullOrEmpty(PassRoot))
            {
                return;
            }
            if (!EditButtonView)
            {
                Page.RegisterStartupScript("CallJSMethod", "<script>hideButton();</script>");
            }
            if (!IsPostBack)
            {
                GetPartnerLogo();
            }
        }

        /// <summary>
        /// Method to show or hide the header title text
        /// </summary>
        /// <param name="value"></param>
        protected void ShowHeaderText(bool value)
        {
            HeaderText.Visible = value;
        }
        /// <summary>
        /// Gets the partner Logo for the specified Subscription
        /// </summary>
        private bool GetPartnerLogo()
        {

            PartnerLogoFactory partnerLogoFactory = new PartnerLogoFactory(basePage.UserConnStr);
            try
            {
                string PartnerLogoImageName = string.Empty;
                PartnerLogoImageName = partnerLogoFactory.GetPartnerLogoImage(PassRoot);

                if (!string.IsNullOrEmpty(PartnerLogoImageName))
                {
                    //// Hide Fileupload and Browse Button, show the Logo image
                    //UpdatePartnerLogoButton.Visible = false;
                    //PartLogoFileUpload.Visible = false;
                    LogoImage.Visible = true;

                    NameValueCollection appSettings = WebConfigurationManager.AppSettings as NameValueCollection;
                    string logoPathName = appSettings["PartnerLogoUri"];
                    LogoImage.Src = logoPathName + PartnerLogoImageName;
                    LogoImage.Alt = PartnerLogoImageName;
                    if (!string.IsNullOrEmpty(PartnerLogoImageName))
                    {
                        PartnerLogoName.Text = PartnerLogoImageName;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException exception)
            {
                PartnerLogoErrorLabel.Text = Resources.Resource.PartnerLogoGetFail + " " + exception.Message;
                PartnerLogoErrorLabel.Visible = true;
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Partner Logo", exception);
                return false;
            }

        }
        /// <summary>
        /// Updates the partner Logo for the specified Subscription
        /// </summary>
        protected void UpdatePartnerLogo_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string logoFileName = PartLogoFileUpload.FileName;
            if (string.IsNullOrEmpty(logoFileName))
            {
                PartnerLogoErrorLabel.Text = "Invalid file or file path";
                PartnerLogoErrorLabel.Visible = true;
                return;
            }
            try
            {
                NameValueCollection appSettings = WebConfigurationManager.AppSettings as NameValueCollection;
                string pathName = appSettings["PartnerLogoPath"];
                filePath = pathName + logoFileName;

                if (string.IsNullOrEmpty(pathName))
                {
                    PartnerLogoErrorLabel.Text = Resources.Resource.PathName;
                    PartnerLogoErrorLabel.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(PassRoot))
                {
                    PartnerLogoErrorLabel.Text = Resources.Resource.PartnerName;
                    PartnerLogoErrorLabel.Visible = true;
                    return;
                }
                if (PartLogoFileUpload.HasFile)
                {
                    if (!PartLogoFileUpload.PostedFile.ContentType.Contains("image"))
                    {
                        PartnerLogoErrorLabel.Text = Resources.Resource.SelectImage;
                        PartnerLogoErrorLabel.Visible = true;
                        return;
                    }
                    if (!Directory.Exists(pathName))
                    {
                        PartnerLogoErrorLabel.Text = Resources.Resource.TargetFolder;
                        PartnerLogoErrorLabel.Visible = true;
                        return;
                    }
                    if (File.Exists(filePath))
                    {
                        PartnerLogoErrorLabel.Text = Resources.Resource.FileExists;
                        PartnerLogoErrorLabel.Visible = true;
                        return;
                    }

                    PartnerLogoFactory partnerLogoFactory = new PartnerLogoFactory(basePage.UserConnStr);
                    PartLogoFileUpload.SaveAs(filePath);

                    string PartnerLogoImageName = string.Empty;
                    PartnerLogoImageName = partnerLogoFactory.GetPartnerLogoImage(PassRoot);

                    if (!string.IsNullOrEmpty(PartnerLogoImageName))
                    {
                        partnerLogoFactory.UpdatePartnerLogo(PassRoot, logoFileName);
                    }
                    else
                    {
                        partnerLogoFactory.PutPartnerLogo(PassRoot, logoFileName);
                    }

                    PartnerLogoErrorLabel.Text = Resources.Resource.PartnerLogoSuccess;
                    PartnerLogoErrorLabel.Visible = true;
                    GetPartnerLogo();
                    return;
                }
                else
                {
                    PartnerLogoErrorLabel.Text = Resources.Resource.SelectLogo;
                    PartnerLogoErrorLabel.Visible = true;
                    return;
                }
            }

            catch (Exception exception)
            {
                File.Delete(filePath);
                PartnerLogoErrorLabel.Text = Resources.Resource.PartnerLogoFail + " " + exception.Message;
                PartnerLogoErrorLabel.Visible = true;
                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, "Partner Logo", exception);
            }
        }

        //protected void EditButton_Click(object sender, EventArgs e)
        //{
        //    ReadOnlyPanel.Visible = false;
        //    EditViewPanel.Visible = true;
        //}
        #endregion

    }
}