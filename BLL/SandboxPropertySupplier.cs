using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using B24.Common;
using B24.Common.IGE;
using B24.Common.Web;
using B24.Common.Web.BLL;
using B24.Common.Logs;
using SkillSoft.IGE.IGEClient;

namespace B24.Sales3.BLL
{
    public class SandboxPropertySupplier
    {
        #region private variables
        private const string NotificationPropertyDescription = "Disallow inGenius Notifications via email";
        private const string SystemInterestDescription = "Suggested Interests enabled";
        private const string IGEScoreDescription = "Display inGenius Score";
        // corresponding to UI
        private SandboxProperties thisSandboxProperties = null;
        // corresponding to DB
        private SandboxProperties dbSandBoxProperties = null;
        private Logger logger = new Logger(Logger.LoggerType.Sales3);
        private GlobalVariables global = GlobalVariables.GetInstance();
        private Sales3.UI.BasePage basePage;
        private Guid subscriptionId;
        private Guid requesterId;
        private Guid sandboxId;
        private bool hasError;
        private string errorMsg;
        bool allowNotificationEmail = true;// deliverytype is UI, means no email (a.k.a not allow email)
        bool disallowNotificationEmail = true;
        bool disableNotificationEmailcheck = false;// virable to identify if notification email checkbox needs to be disabled
                                                   // if there is any notificationtype has email assigned in DB, we need to disable the
                                                   // checkbox, so admin has no access to it.

        #endregion 
        #region public Properties
        public bool AllowNotificationEmail
        {
            get { return allowNotificationEmail; }
            set { allowNotificationEmail = value; }
        }
        public SandboxProperties DBSandBoxProperties
        {
            get { return dbSandBoxProperties; }
            set { dbSandBoxProperties = value; }
        }
        public SandboxProperties ThisSandBoxProperties
        {
            get { return thisSandboxProperties; }
            set { thisSandboxProperties = value; }
        }
        public IList<SandboxPropertyItem> SandBoxPropertyList
        {
            get { return thisSandboxProperties.Items; }
        }
        public bool HasError
        { get { 
            return hasError; 
        } 
        }
        public string ErrorMsg
        { get { return errorMsg; } }
        #endregion 

        #region Constructors
        public SandboxPropertySupplier(Guid subId, Guid requesterId, Sales3.UI.BasePage basePage) {
            this.subscriptionId = subId;
            this.requesterId = requesterId;
            this.basePage = basePage;

            try
            {
                Subscription subscription = GetSubscription(subscriptionId);
                sandboxId = subscription.SandBoxID;
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMsg = "Retrieve subscription using the provided Id failed";
                logger.Log(Logger.LogLevel.Error, errorMsg, ex);
                return;
            }
 
        }

        public SandboxPropertySupplier(Guid subId, Guid requesterId, Guid sandboxId, Sales3.UI.BasePage basePage)
        {
            this.subscriptionId = subId;
            this.requesterId = requesterId;
            this.sandboxId = sandboxId;
            this.basePage = basePage;
        }
        #endregion 

        #region public Methods
        public void GetSandboxProperties()
        {
            try
            {
                SandboxPropertiesClient client = new SandboxPropertiesClient(sandboxId, requesterId, basePage.IGEServiceBaseUri);
                dbSandBoxProperties = client.GetData();
                DBtoUIsandboxproperties();
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMsg = "Get SandboxProperties Exception";
                logger.Log(Logger.LogLevel.Error, errorMsg, ex);
            }

            SandboxPropertyItem notificationProperty = ChangeNotificationPropertiesIntoSandboxProperty();
            thisSandboxProperties.Items.Add(notificationProperty);
            return ;

        }
        public IList<SandboxPropertyItem> GetSandboxPropertieItems()
        {
            GetSandboxProperties();
            return thisSandboxProperties.Items;
        }
        public void UpdateSandboxProperties()
        {
            UItoDBsandboxproperties();
            //update sandboxproperties
            try
            {
                SandboxPropertiesClient client = new SandboxPropertiesClient(sandboxId, requesterId, basePage.IGEServiceBaseUri);
                client.PutData(dbSandBoxProperties);
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMsg = "Update SandboxProperties Exception";
                logger.Log(Logger.LogLevel.Error, errorMsg, ex);
            }
            // update sandboxnotificationproperties
            try
            {
                SandboxNotificationPropertiesClient client = new SandboxNotificationPropertiesClient(sandboxId, requesterId, basePage.IGEServiceBaseUri);
                SandboxNotificationProperties thisSandboxNotificationProperties = client.GetData();
                if (null != thisSandboxNotificationProperties && thisSandboxNotificationProperties.Items.Count > 0)
                {
                    foreach (SandboxNotificationPropertyItem sbnPropertyitem in thisSandboxNotificationProperties.Items)
                    {
                        if (sbnPropertyitem.NotificationTypeValue.Name != NotificationTypeNames.USERCONTENTFLAGABUSIVE)
                        {
                            if (!disallowNotificationEmail)
                            {
                                sbnPropertyitem.CanOverride = true;
                            }
                            else
                            {
                                sbnPropertyitem.CanOverride = false; // disallowNotificationemail = user can not override system set up
                            }
                            //if (sbnPropertyitem.DeliveryTypeFlagValue == DeliveryTypeFlag.UI && allowNotificationEmail)
                            //{

                            //    sbnPropertyitem.DeliveryTypeFlagValue = DeliveryTypeFlag.UIandEmail;
                            //    sbnPropertyitem.CanOverride = false;
                            //    updatable = true;
                            //}
                            //else
                            //{
                            //    if (sbnPropertyitem.DeliveryTypeFlagValue != DeliveryTypeFlag.UI && !allowNotificationEmail)
                            //    {
                            //        sbnPropertyitem.DeliveryTypeFlagValue = DeliveryTypeFlag.UI;
                            //        sbnPropertyitem.CanOverride = false;
                            //        updatable = true;
                            //    }
                            //}

                        }
                    }
                }
                //if sandboxnotificationproperties's deliverytype is not coincide with allownotificationemail, then call put service

                try
                {
                    client.PutData(thisSandboxNotificationProperties);
                }
                catch (Exception ex)
                {
                    hasError = true;
                    errorMsg = "Put SandboxNotificationProperties Exception";
                    logger.Log(Logger.LogLevel.Error, errorMsg, ex);
                }

            }
            catch (Exception ex)
            {
                hasError = true;
                errorMsg = "Get SandboxNotificationProperties Exception";
                logger.Log(Logger.LogLevel.Error, errorMsg, ex);
            }
            return ;

        }
        #endregion 

        #region private Methods
        /// <summary>
        /// get the subscription object by subscriptionId
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        private Subscription GetSubscription(Guid subscriptionId)
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(basePage.UserConnStr);
            return subscriptionFactory.GetSubscriptionByID(subscriptionId);
        }
        /// <summary>
        /// sandboxpropertyitem is bound to the UI, here we convert the sandboxnotificationproperties into
        /// sandboxproperty for the UI bound purpose. in DB these two properties are seperated. 
        /// </summary>
        /// <returns></returns>
        private SandboxPropertyItem ChangeNotificationPropertiesIntoSandboxProperty()
        {
            SandboxPropertyItem notificationProperty = new SandboxPropertyItem();
            notificationProperty.PropertyName = NotificationPropertyDescription;

        //    bool allowNotificationEmail = true;// deliverytype is UI, means no email (a.k.a not allow email)
            disallowNotificationEmail = true;
            try
            {
                SandboxNotificationPropertiesClient client = new SandboxNotificationPropertiesClient(sandboxId, requesterId, basePage.IGEServiceBaseUri);
                SandboxNotificationProperties thisSandboxNotificationProperties = client.GetData();
                if (null != thisSandboxNotificationProperties && thisSandboxNotificationProperties.Items.Count > 0)
                {
                    foreach (SandboxNotificationPropertyItem sbnPropertyitem in thisSandboxNotificationProperties.Items)
                    {
                        if (sbnPropertyitem.NotificationTypeValue.Name != NotificationTypeNames.USERCONTENTFLAGABUSIVE)
                        {
                            if (sbnPropertyitem.DeliveryTypeFlagValue != DeliveryTypeFlag.UI)
                            {
                                disableNotificationEmailcheck = true;
                                break;
                            }
                            if (sbnPropertyitem.CanOverride)
                            {
                                disallowNotificationEmail = false; // if allow user to override, then we allow user to opt in notification emails
                            }
                            else
                            {
                                disallowNotificationEmail = true;
                            }
                            //if (sbnPropertyitem.DeliveryTypeFlagValue == DeliveryTypeFlag.UI)
                            //{
                            //    allowNotificationEmail = false; // if one notificationtype has UI as deliverytype,then false
                            //    break;
                            //}
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMsg = "Get SandboxNotificationProperties Exception";
                logger.Log(Logger.LogLevel.Error, errorMsg, ex);
            }

            if (disallowNotificationEmail) notificationProperty.Status = ContentStatus.ON;
            else notificationProperty.Status = ContentStatus.OFF;
            if (disableNotificationEmailcheck) notificationProperty.Status = ContentStatus.PRIVATE; // we use private here to pass the information
                                                                                    // that the checkbox needs to be diabled.
            return notificationProperty;
        }
        private void UItoDBsandboxproperties()
        {
            dbSandBoxProperties = new SandboxProperties();
            foreach (SandboxPropertyItem uiItem in ThisSandBoxProperties.Items)
            {
                SandboxPropertyItem dbItem = new SandboxPropertyItem();
                switch (uiItem.PropertyName)
                {
                    case SystemInterestDescription:
                        dbItem.PropertyName = SandboxPropertyTypes.SYSTEMINTEREST;
                        dbItem.Status = uiItem.Status;
                        dbSandBoxProperties.Items.Add(dbItem);
                        break;
                    case IGEScoreDescription:
                        dbItem.PropertyName = SandboxPropertyTypes.IGESCORE;
                        dbItem.Status = uiItem.Status;
                        dbSandBoxProperties.Items.Add(dbItem);
                        break;
                    case NotificationPropertyDescription:
                        if (uiItem.Status == ContentStatus.ON)
                        {
                            disallowNotificationEmail = true;
                           // allowNotificationEmail = true;
                        }
                        else if (uiItem.Status == ContentStatus.OFF)
                        {
                            disallowNotificationEmail = false;
                           // allowNotificationEmail = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void DBtoUIsandboxproperties()
        {
            thisSandboxProperties = new SandboxProperties();
            foreach (SandboxPropertyItem dbItem in DBSandBoxProperties.Items)
            {
                SandboxPropertyItem uiItem = new SandboxPropertyItem();
                switch (dbItem.PropertyName)
                {
                    case SandboxPropertyTypes.SYSTEMINTEREST:
                        uiItem.PropertyName = SystemInterestDescription;
                        uiItem.Status = dbItem.Status;
                        thisSandboxProperties.Items.Add(uiItem);
                        break;
                    case SandboxPropertyTypes.IGESCORE:
                        uiItem.PropertyName = IGEScoreDescription;
                        uiItem.Status = dbItem.Status;
                        thisSandboxProperties.Items.Add(uiItem);
                        break;
                    default:
                        break;
                }
            }

        }
        #endregion 
    }
}
