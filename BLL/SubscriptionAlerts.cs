using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Management;
using B24.Common;

namespace B24.Sales4.BLL
{
    [DataObject(true)]
    public class SubscriptionAlerts
    {
        #region Private Members

        private string UserConnectionString;
        private Guid SubscriptionID = Guid.Empty;
        private List<SubscriptionAlert> AlertList = null;
        
        #endregion

        #region Constructors

        public SubscriptionAlerts() { }

        public SubscriptionAlerts(string userConn, Guid subID) 
        {
            this.UserConnectionString = userConn;
            this.SubscriptionID = subID;
            InitializeAlertList();

        }
        #endregion

        #region Public Methods

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<SubscriptionAlert> GetAlerts()
        {
            return AlertList;
        }

        #endregion

        #region Private methods

        private void InitializeAlertList()
        {
            if ((!String.IsNullOrEmpty(this.UserConnectionString)) && (this.SubscriptionID != Guid.Empty))
            {
                SubscriptionAlertFactory subscriptionAlertFactory = new SubscriptionAlertFactory(this.UserConnectionString);
                AlertList = subscriptionAlertFactory.GetListOfSubscriptionAlerts(this.SubscriptionID);
            }
        }

        #endregion

    }
}