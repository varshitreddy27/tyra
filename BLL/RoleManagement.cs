using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B24.Sales4.UI
{
    public static class Sales4Module
    {
        // Module
        public const int ModuleInfo = 1;            // Default page
        public const int ModuleManageAccounts = 2;  // Subscription manage page
        public const int ModuleManageUsers = 3;     // User management page
        public const int ModuleCreateNewTrial = 4;  // Page to create new trial
        public const int ModuleGeneralReporting = 5;// Page to generate general reports
        public const int ModuleCart = 6;            // Ecommerce page

        //Sub Module
        public const int SubModuleManageAccountInfo = 1; // Manage Accounts
        public const int SubModuleCollections = 2;      // Manage Accounts
        public const int SubModuleImplementation = 3;   // Manage Accounts
        public const int SubModuleAddUsers = 4;         // Manage Accounts
        public const int SubModuleSettings = 5;         // Manage Accounts
        public const int SubModuleReports = 6;          // Manage Accounts
        public const int SubModulePurchaseOrRenew = 7;  // Manage Accounts
        public const int SubModuleManageUserInfo = 8;   // Manage Users
        public const int SubModulePassword = 9;         // Manage Users
        public const int SubModuleAdminRoles = 10;      // Manage Users
        public const int SubModuleEmailSettings = 11;   // Manage Users
        public const int SubModuleHomeInfo = 12;        // Start
        public const int SubModuleCreateNewTrial = 13;  // Create New Trial
        public const int SubModuleGeneralReporting = 14;// General Reporting
        public const int SubModuleCart = 15;            // Cart
        public const int SubModuleCreateNewTrialUser = 16; // Create New Trial User
       
        // Features
        public const int FeatureManageAccountInfo = 1;          //	Manage Account-> Info
        public const int FeatureCollections = 2;                //	Manage Account-> Collections
        public const int FeatureManageAccount = 3;             //	Manage Account-> Implementation
        public const int FeatureSingleSignOn = 4;               //	Manage Account-> Implementation
        public const int FeatureLogo = 5;                       //	Manage Account-> Implementation
        public const int FeatureSubscriptionflags = 6;          //	Manage Account-> Implementation
        public const int FeatureSelfRegistrationInstruction = 7; //	Manage Account-> Implementation
        public const int FeatureCostcenter = 8;                 //	Manage Account-> Implementation
        public const int FeatureExtendedAttributes = 9;         //	Manage Account-> Implementation
        public const int FeatureIngeniusSandbox = 10;           //	Manage Account-> Implementation
        public const int FeatureAddUsers = 11;                  //	Manage Account-> Add Users
        public const int FeatureWelcomeMessageSender = 12;      //	Manage Account-> Settings
        public const int FeatureAddSubscriptionNotes = 13;      //	Manage Account-> Settings
        public const int FeatureSelfRegistration = 14;          //	Manage Account-> Settings
        public const int FeatureReactivateOrExtendThisTrial = 15; // Manage Account-> Settings
        public const int FeatureDisableOrRestoreUser = 16;      //	Manage Account-> Settings
        public const int FeatureReports = 17;                   //  Manage Account->Reports
        public const int FeatureUpdateThisAccount = 18;         //	Manage Account->Purchase/Renew
        public const int FeatureUserDetails = 19;               //	Manage Users ->Info
        public const int FeatureChangeUserPassword = 20;        //	Manage Users-> Password
        public const int FeatureEmailUserPassword = 21;         //	Manage Users-> Password
        public const int FeatureAdminPermissons = 22;           //	Manage Users-> Admin roles
        public const int FeatureEmailSettings = 23;             //	Manage Users-> Settings
        public const int FeatureDisableOrRestore = 24;          //	Manage Users-> Settings
        public const int FeatureImpersonate = 25;               //	Manage Users-> Settings
        public const int FeatureModuleAlerts = 26;              // Manage Account-> Settings
    }
}
