using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;

namespace B24.Sales4.UserControl
{
    /// <summary>
    /// User control to change password for the types User and Temp.
    /// </summary>
    public partial class ChangePassword : System.Web.UI.UserControl
    {

        GlobalVariables global = GlobalVariables.GetInstance();
        Logger logger;
        BasePage baseObject;

        #region Property

        /// <summary>
        /// ID of the user whose password we want to change
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>    
        /// User(Admin) making password change request
        /// </summary>
        public Guid RequestorId { get; set; }

        /// <summary>    
        /// Show/hide header title text
        /// </summary>        
        public bool ShowHeaderText { set { HeaderText.Visible = value; } }

        public bool ChangeButtonView { get; set; }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            baseObject = this.Page as BasePage;
            //Initialize the control values
            InitControls();
            if (!Page.IsPostBack)
            {
                //Check User Id and Requestor Id value
                if (UserId == Guid.Empty || RequestorId == Guid.Empty)
                {
                    this.ChangeButton.Enabled = false;

                    ChangePasswordError.Text = Resources.Resource.ErrorLoadControl;
                    ChangePasswordError.Visible = true;
                }
                LoadPasswordTypeList();
            }
        }

        protected void ChangeButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && IsValidPassword())
            {
                try
                {
                    PasswordType type = PasswordType.User;
                    if (typeList.SelectedValue == PasswordType.Temp.ToString())
                    {
                        type = PasswordType.Temp;
                    }

                    UserFactory userFactory = new UserFactory(baseObject.UserConnStr);
                    userFactory.ChangePassword(UserId, type, txtNewPassword.Text.Trim(), RequestorId);

                    ChangePasswordError.Text = Resources.Resource.PasswordUpdate;
                    ChangePasswordError.Visible = true;

                    ClearControls();
                }
                catch (Exception ex)
                {
                    ChangePasswordError.Text = ex.Message;
                    ChangePasswordError.Visible = true;

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "User Detail", ex);
                }
            }
        }
        #endregion

        #region private methods

        // Verify the values before changing the password
        private bool IsValidPassword()
        {
            // Verify matching passwords
            if (txtNewPassword.Text.Trim() != txtConfirm.Text.Trim())
            {
                ChangePasswordError.Text = Resources.Resource.PasswordMatch;
                ChangePasswordError.Visible = true;

                return false;
            }

            //Verify the password length
            if (txtNewPassword.Text.Trim().Length < 3 || txtNewPassword.Text.Trim().Length > 63)
            {
                ChangePasswordError.Text = Resources.Resource.PasswordLength;
                ChangePasswordError.Visible = true;

                return false;
            }
            return true;
        }

        // Load password types 'User' and 'Temp' only
        private void LoadPasswordTypeList()
        {
            this.typeList.Items.Add(new ListItem(PasswordType.User.ToString(), PasswordType.User.ToString()));
            this.typeList.Items.Add(new ListItem(PasswordType.Temp.ToString(), PasswordType.Temp.ToString()));
        }

        // Clear the control values 
        private void ClearControls()
        {
            this.txtNewPassword.Text = String.Empty;
            this.txtConfirm.Text = String.Empty;
        }

        // Load the error messages from the resource file
        private void InitControls()
        {
            this.NewPasswordRequiredValidator.ErrorMessage = Resources.Resource.Required;
            this.ConfirmRequiredValidator.ErrorMessage = Resources.Resource.Required;
            ChangeButton.Visible = ChangeButtonView;
        }
        #endregion
    }
}
