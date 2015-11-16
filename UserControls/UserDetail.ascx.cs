using System;
using System.Web.UI;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;

namespace B24.Sales3.UserControl
{
    /// <summary>
    /// User control to manage user data.
    /// </summary>
    public partial class UserDetail : System.Web.UI.UserControl
    {
        private Sales3.UI.BasePage basePage;
        Logger logger;

        #region Property

        /// <summary>
        /// User details.
        /// </summary>
        public User user { get; set; }
        /// <summary>
        /// User Id to be updated.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Show/Hide header title text.
        /// </summary>        
        public bool ShowHeaderText { set { HeaderText.Visible = value; } }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Initialize the control values
            InitControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            basePage = this.Page as Sales3.UI.BasePage;
            if (!Page.IsPostBack)
            {
                try
                {
                    LoadUserDetail();
                }
                catch (Exception ex)
                {
                    UserDetailError.Visible = true;
                    UserDetailError.Text = Resources.Resource.UserLoadError;

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "User Detail", ex);
                }
            }
        }

        /// <summary>
        /// Update event to update the user information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) // If validation is success
            {
                try
                {
                    user = new User();
                    user.UserID = UserId;
                    user.FirstName = txtFirstName.Text.Trim();
                    user.LastName = txtLastName.Text.Trim();
                    user.Email = txtEmail.Text.Trim();
                    user.FixedUserName = txtSkillPortId.Text.Trim();

                    UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                    userFactory.UpdateUserInfo(user, user.UserID);

                    UserDetailError.Text = Resources.Resource.UserDetailSuccess;
                    UserDetailError.Visible = true;
                }
                catch (Exception ex)
                {
                    UserDetailError.Text = Resources.Resource.UserDetailFail;
                    UserDetailError.Visible = true;

                    logger = new Logger(Logger.LoggerType.UserInfo);
                    logger.Log(Logger.LogLevel.Error, "User Detail", ex);
                }
            }
        }

        #endregion

        # region private methods

        //Load user data
        private void LoadUserDetail()
        {
            if (false == Guid.Empty.Equals(UserId))
            {
                //If user object is not assigned from the page
                if (user == null)
                {
                    UserFactory userFactory = new UserFactory(basePage.UserConnStr);
                    user = userFactory.GetUserByID(UserId);
                }
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtEmail.Text = user.Email;
                txtSkillPortId.Text = user.FixedUserName;
            }
            else
            {
                UserDetailError.Visible = true;
                UserDetailError.Text = Resources.Resource.UserLoadError;

                UpdateButton.Enabled = false;
            }
        }

        // Load the error messages from the resource file
        private void InitControls()
        {
            this.firstNameRequiredValidator.ErrorMessage = Resources.Resource.Required;
            this.lastNameRequiredValidator.ErrorMessage = Resources.Resource.Required;
            this.emailIdRequiredValidator.ErrorMessage = Resources.Resource.Required;
            this.emailFormatValidator.ErrorMessage = Resources.Resource.InvalidEmail;
        }

        #endregion
    }
}