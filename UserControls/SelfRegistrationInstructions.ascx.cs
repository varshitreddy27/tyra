using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common.Logs;
using B24.Common;

namespace B24.Sales3.UserControl
{
    /// <summary>
    /// To Update or Set the Self Registration Instructions
    /// </summary>
    public partial class SelfRegistrationInstructions : System.Web.UI.UserControl
    {
        # region Public Properties
        public Guid SubscriptionId { get; set; }
        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool UpdateButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SubscriptionId == null || SubscriptionId == Guid.Empty)
            {
                return;
            }
            if (!IsPostBack)
            {
                LoadSelfRegistrationInstructions();
            }
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables global = GlobalVariables.GetInstance();
                SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
                Subscription getSubscription = subscriptionFactory.GetSubscriptionByID(SubscriptionId);

                RegistrationInstructionFactory selfRegistrationInstructions = new RegistrationInstructionFactory(global.UserConnStr);
                selfRegistrationInstructions.PasswordRoot = getSubscription.PasswordRoot;
                selfRegistrationInstructions.PutSelfRegistrationInstructions(InstructionsTextArea.Text.Trim());

                Errorlabel.Text = Resources.Resource.SelfRegistrationUpdated;
                Errorlabel.Visible = true;
            }
            catch (Exception ex)
            {
                Errorlabel.Text = Resources.Resource.SelfRegistrationFailed;
                Errorlabel.Visible = true;

                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SelfRegistrationFailed, ex);
            }

        }
        /// <summary>
        /// Load the Self registration instruction for the current subscription
        /// </summary>
        private void LoadSelfRegistrationInstructions()
        {
            try
            {
                GlobalVariables global = GlobalVariables.GetInstance();
                RegistrationInstructionFactory selfRegistrationInstructions = new RegistrationInstructionFactory(global.UserConnStr);
                selfRegistrationInstructions.SubscriptionId = SubscriptionId;

                InstructionsTextArea.Text = selfRegistrationInstructions.GetRegistrationInstrcution();
                UpdateButton.Visible = UpdateButtonView;
            }
            catch (Exception ex)
            {
                Errorlabel.Text = Resources.Resource.SelfRegistrationFetchFailed;
                Errorlabel.Visible = true;

                Logger logger = new Logger(Logger.LoggerType.AccountInfo);
                logger.Log(Logger.LogLevel.Error, Resources.Resource.SelfRegistrationFailed, ex);
            }
        }

    }
}
