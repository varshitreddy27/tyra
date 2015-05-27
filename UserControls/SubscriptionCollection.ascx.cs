using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using B24.Common;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using B24.Common.Logs;


namespace B24.Sales3.UserControl
{
    public partial class SubscriptionCollection : System.Web.UI.UserControl
    {
        #region Class Members
        private GlobalVariables global = GlobalVariables.GetInstance();
        private SubscriptionFactory subFac;
        private Logger logger = new Logger(Logger.LoggerType.AccountInfo);
        private Dictionary<int, string> accessListDict;
        private int chapterPDFDownloadLimit;
 		private Dictionary<string, Collection> chapterDownloadDictionary = new Dictionary<string,Collection>();

        #endregion

        #region Public Properties
        /// <summary>
        /// Get the subscription from page
        /// </summary>
        public Subscription Subscription { get; set; }       
        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// Login name
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Chapter download limit
        /// </summary>
        public string ChapterDownloadLimit { get; set; }

        /// <summary>
        ///  To set Read only View or Edit View
        /// </summary>
        public bool AddButtonView { get; set; }

        public bool UpdateApplicationButtonView { get; set; }
        /// <summary>
        /// To update the subscription details panel
        /// </summary>
        public UpdatePanel InfoDetailsUpdatePanel { get; set; }
        /// <summary>
        /// Event handler 
        /// </summary>
        public EventHandler UpdateInfo { get; set; }
        #endregion

        /// <summary>
        /// Initialize page control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Subscription == null)
            {
                return;
            }
            InintializeControls();
            // initialize the common data
            InitializeCommonData();

            if (!Page.IsPostBack)
            {
                try
                {
                    // Bind Application DropDownList
                    LoadApplication();

                    // Bind Collection grid
                    LoadCollectionGrid();

                    // populate collection dropdown list of Add new collection section
                    LoadAddCollection();

                    // populate access dropdown list of Add new collection section
                    LoadAddAccessList();
                }
                catch(Exception ex)
                {
                    UpdateErrorPanel(ex.Message, false);
                }
            }
        }

        /// <summary>
        /// Initialize control events
        /// </summary>
        private void InintializeControls()
        {
            //event handlers
            CollectionGridView.RowUpdating += new GridViewUpdateEventHandler(CollectionGridView_RowUpdating);
            CollectionGridView.RowDeleting += new GridViewDeleteEventHandler(CollectionGridView_RowDeleting);
            CollectionGridView.RowDataBound += new GridViewRowEventHandler(CollectionGridView_OnRowDataBound);

            UpdateApplicationButton.Visible = UpdateApplicationButtonView;
            AddButton.Visible = AddButtonView;
        }

        /// <summary>
        /// Initialize common data
        /// </summary>
        private void InitializeCommonData()
        {
            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
            chapterPDFDownloadLimit = subscriptionFactory.GetDownloadRoyaltyInfo(Subscription.SubscriptionID);

            accessListDict = new Dictionary<int, string>();
            accessListDict.Add(-1, "None");
            accessListDict.Add(0, "Teaser");
            accessListDict.Add(1, "All");
            accessListDict.Add(2, "User Select");
            accessListDict.Add(3, "Admin Select");

        }
        /// <summary>
        /// Load the data for Add section
        /// </summary>
        void LoadAddAccessList()
        {
            NewAccessDropDownList.DataSource = accessListDict.Values;
            NewAccessDropDownList.DataBind();
            NewAccessDropDownList.SelectedIndex = 2;
        }

        /// <summary>
        /// Load collection from DB
        /// </summary>
        void LoadAddCollection()
        {
            PermissionFactory permFactory = new PermissionFactory(global.UserConnStr);
            Permission permissions = permFactory.LoadPermissionsById(UserId, Login, String.Empty);
            
            Guid subscriptionID = Subscription.SubscriptionID;

            if (permissions.GeneralAdmin == 1)
            {
                subscriptionID = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
            }

            CollectionFactory collectionFactory = new CollectionFactory(global.UserConnStr);
            List<Collection> collectionNameList = collectionFactory.GetCollectionsForUser(subscriptionID, UserId, String.Empty, null, Collection.CollectionType.All);

            AddCollectionDropDownList.DataSource = collectionNameList;
            AddCollectionDropDownList.DataTextField = "name";
            AddCollectionDropDownList.DataValueField = "collectionId";
            AddCollectionDropDownList.DataBind();

            AddCollectionDropDownList.Items.Insert(0, "---Select One---");
            NewExpiryDateTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Load application list
        /// </summary>
        void LoadApplication()
        {
            ApplicationFactory applicationFactory = new ApplicationFactory(global.UserConnStr);
            Collection<Application> SubApplicationList = applicationFactory.GetAllApplicationForUser(UserId);
            int index = 0;
            foreach (Application application in SubApplicationList)
            {
                if (application.Name.Equals(ApplicationName))
                {
                    break;
                }
                index++;
            }
            ApplicationDropDownList.DataSource = SubApplicationList;
            ApplicationDropDownList.DataTextField = "name";
            ApplicationDropDownList.DataValueField = "description";
            ApplicationDropDownList.DataBind();

            ApplicationDropDownList.SelectedIndex = index;
        }

        /// <summary>
        /// Retrieve the data to bind data in grid
        /// </summary>
        private void LoadCollectionGrid()
        {
            CollectionFactory collectionFac = new CollectionFactory(global.UserConnStr);
            List<Collection> collectionList = collectionFac.GetCollectionsForUser(Subscription.SubscriptionID, UserId, String.Empty, null, Collection.CollectionType.All);

            SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);

            if (!(subscriptionFactory.GetDownloadRoyaltyInfo(Subscription.SubscriptionID) > 0 && Subscription.UpliftAllowed))
            {
                // Hide the "Chapter Downloads Allowed" column
                CollectionGridView.Columns[4].Visible = false;
            }
            else
            {
                ChapterDownloadFactory chapterFactory = new ChapterDownloadFactory(global.UserConnStr);
                chapterDownloadDictionary = chapterFactory.GetChapterCollectionBySubscriptionId(Subscription.SubscriptionID);
            }
            CollectionGridView.DataSource = collectionList;
            CollectionGridView.DataBind();
        }

        /// <summary>
        /// Delete the row from grid and from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView CollectionGridView = sender as GridView;                                           // Gridview throwing the event
            Guid collectionId = new Guid(CollectionGridView.DataKeys[e.RowIndex].Value.ToString());     // The unique id of rule being updated            
           
            // Update the rule
            try
            {
                CollectionFactory collectionFac = new CollectionFactory(global.UserConnStr);
                collectionFac.DeleteCollection(collectionId, Subscription.SubscriptionID);

                UpdateErrorPanel(Resources.Resource.CollectionDeleted, true);

                LoadCollectionGrid();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.CollectionNotDeleted, ex);

                UpdateErrorPanel(ex.Message, true);
            }
        }

        /// <summary>
        /// Update data in grid and database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView CollectionGridView = sender as GridView;                                           // Gridview throwing the event
            Guid collectionId = new Guid(CollectionGridView.DataKeys[e.RowIndex].Value.ToString());     // The unique id of rule being updated
            GridViewRow row = CollectionGridView.Rows[e.RowIndex];                                      // The row being updated               

            Label DescriptionLabel = row.FindControl("DescriptionLabel") as Label;
            DropDownList AccessDDL = row.FindControl("AccessDropDownList") as DropDownList;
            TextBox SeatCountTB = row.FindControl("SeatCountTextBox") as TextBox;
            TextBox AccessExpiresTB = row.FindControl("AccessExpiresTextBox") as TextBox;
            Label AccessExpiresLabelTB = row.FindControl("AccessExpiresLabel") as Label;
            DropDownList ChapterDownloadDDL = row.FindControl("ChapterDownloadDropDownList") as DropDownList;

            Collection newCollection = new Collection();
            newCollection.CollectionID = collectionId;
            newCollection.Capacity = -1000;
            newCollection.Type = Collection.TypeID.AllBooks;
            newCollection.FullAccess = -1;
            newCollection.IsPrivate = 0;

            newCollection.AccessExpires = new DateTime();
            if (AccessExpiresTB.Visible && !String.IsNullOrEmpty(AccessExpiresTB.Text))
            {
                newCollection.AccessExpires = Convert.ToDateTime(AccessExpiresTB.Text);
            }
            if (!AccessExpiresTB.Visible && !String.IsNullOrEmpty(AccessExpiresLabelTB.Text))
            {
                newCollection.AccessExpires = Convert.ToDateTime(AccessExpiresLabelTB.Text);
            }
            newCollection.Name = DescriptionLabel.Text;
            if (String.Equals(SeatCountTB.Text,"All") || String.IsNullOrEmpty(SeatCountTB.Text))
            {
                newCollection.SeatCount = -1;
            }
            else
            {
                newCollection.SeatCount = Convert.ToInt16(SeatCountTB.Text);
            }
            
            newCollection.IsDefault = AccessDDL.SelectedIndex - 1;
            
            if( ChapterDownloadDDL.SelectedIndex == 0)
            {
                // Allow
                newCollection.ChapterDownloadLimit = 5;
            }
            else
            {
                // Not Allow
                newCollection.ChapterDownloadLimit = 0;
            }

            try
            {               
                CollectionFactory collectionFactory = new CollectionFactory(global.UserConnStr);
                collectionFactory.PutCollection(newCollection, Subscription.SubscriptionID);
                collectionFactory.UpdateCollectionChapterDownload(Subscription.SubscriptionID, newCollection.CollectionID, newCollection.ChapterDownloadLimit);
                
                UpdateErrorPanel(Resources.Resource.CollectionUpdated, true);                
                
                LoadCollectionGrid();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.CollectionNotUpdated, ex);

                UpdateErrorPanel(ex.Message, true);
            }
        }

        /// <summary>
        /// Bind the data in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Collection collection = (Collection)e.Row.DataItem;

                // Set Update and delete button status
                ImageButton updateButton = e.Row.FindControl("UpdateCollectionButton") as ImageButton;
                ImageButton deleteButton = e.Row.FindControl("DeleteButton") as ImageButton;
                updateButton.Enabled = AddButtonView;
                deleteButton.Enabled = AddButtonView;

                // Access Dropdownlist
                DropDownList accessListDDL = e.Row.FindControl("AccessDropDownList") as DropDownList;
                accessListDDL.DataSource = accessListDict.Values;
                accessListDDL.DataBind();

                accessListDDL.SelectedIndex = collection.IsDefault + 1;

                // Seat Count	
                if (collection.SeatCount == -1)
                {
                    TextBox seatCountTextBox = e.Row.FindControl("SeatCountTextBox") as TextBox;
                    seatCountTextBox.Text = "All";
                }
                //Collection Expiration Date
                string collectionExpires = string.Empty;
                if (collection.AccessExpires.Date != DateTime.Now.Date)
                {
                    collectionExpires = collection.AccessExpires.ToString("MMM dd, yyyy");
                }
                // is this a "teaser", i.e., trial collection?
                if (collection.Name.StartsWith("~"))
                {
                    //Panel accessExpiresPanel = e.Row.FindControl("AccessExpiresPanel") as Panel;
                    //accessExpiresPanel.Visible = true;

                    TextBox accessExpiresTextBox = e.Row.FindControl("AccessExpiresTextBox") as TextBox;
                    accessExpiresTextBox.Text = collectionExpires;
                    accessExpiresTextBox.Visible = true;
                }
                else
                {
                    Label accessExpiresLabel = e.Row.FindControl("AccessExpiresLabel") as Label;
                    accessExpiresLabel.Visible = true;
                    accessExpiresLabel.Text = collectionExpires;
                }

                //Chapter Downloads Allowed
                DropDownList chapterDownloadDropDownList = e.Row.FindControl("ChapterDownloadDropDownList") as DropDownList;

                if (Subscription.UpliftAllowed)
                {
                    if (chapterDownloadDictionary.Keys.Contains(collection.Name))
                    {
                        
                        chapterDownloadDropDownList.SelectedIndex = 0;
                    }
                    else
                    {
                        chapterDownloadDropDownList.SelectedIndex = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Update selected application in db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateApplicationButton_Click(object sender, EventArgs e)
        {
            try
            {
                subFac = new SubscriptionFactory(global.UserConnStr);
                Subscription.ApplicationName = ApplicationDropDownList.SelectedItem.Text.Trim();
                subFac.PutSubscription(Subscription, UserId, Login);

                UpdateErrorPanel(Resources.Resource.ApplicationChanged, true);

				//LoadApplication();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.CollectionNotDeleted, ex);

                UpdateErrorPanel(ex.Message, true);
            }
        }
        /// <summary>
        /// Event to add new collection in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                Collection newCollection = new Collection();
                newCollection.CollectionID = new Guid(AddCollectionDropDownList.SelectedValue.ToString());
                newCollection.Capacity = -1000;
                newCollection.Type = Collection.TypeID.AllBooks;
                newCollection.FullAccess = -1;
                newCollection.IsPrivate = 0;

                if (!String.IsNullOrEmpty(NewExpiryDateTextBox.Text.Trim()))
                {
                    newCollection.AccessExpires = Convert.ToDateTime(NewExpiryDateTextBox.Text + " 11:59:59 PM");
                }

                //else
                //{
                //newCollection.AccessExpires = Convert.ToDateTime(NewExpiryDateTextBox.Text + " 00:00:00 AM");
                //}
                newCollection.Name = AddCollectionDropDownList.SelectedItem.ToString();

                if (NewSeatCountTextBox.Text.Equals("All") || String.IsNullOrEmpty(NewSeatCountTextBox.Text.Trim()))
                {
                    newCollection.SeatCount = -1;
                }
                else
                {
                    newCollection.SeatCount = Convert.ToInt16(NewSeatCountTextBox.Text);
                }
                newCollection.IsDefault = NewAccessDropDownList.SelectedIndex - 1;

                if (NewChapterDownloadDropDownList.SelectedIndex == 0)
                {
                    // Allow
                    SubscriptionFactory subscriptionFactory = new SubscriptionFactory(global.UserConnStr);
                    chapterPDFDownloadLimit = subscriptionFactory.GetDownloadRoyaltyInfo(Subscription.SubscriptionID);
                    newCollection.ChapterDownloadLimit = chapterPDFDownloadLimit;
                }
                else
                {
                    // Not Allow
                    newCollection.ChapterDownloadLimit = 0;
                }

                CollectionFactory collectionFactory = new CollectionFactory(global.UserConnStr);
                collectionFactory.PutCollection(newCollection, Subscription.SubscriptionID);
                collectionFactory.UpdateCollectionChapterDownload(Subscription.SubscriptionID, newCollection.CollectionID, newCollection.ChapterDownloadLimit);

                UpdateErrorPanel(Resources.Resource.CollectionAdded, true);

                // update the grid area                
                LoadCollectionGrid();
                CollectionListUpdatePanel.Update();

                LoadAddCollection();
                LoadAddAccessList();
            }
            catch (Exception ex)
            {
                logger.Log(Logger.LogLevel.Error, Resources.Resource.CollectionNotAdded, ex);
                UpdateErrorPanel(ex.Message, true);
            }
        }

        /// <summary>
        /// Update error panel
        /// </summary>
        /// <param name="text"></param>
        /// <param name="doUpdate"></param>
        void UpdateErrorPanel(string text, bool doUpdate)
        {
            CollectionErrorLabel.Text = text;            
            if (doUpdate)
            {
                CollectionErrorLabel.Visible = true;
                ErrorTextUpdatePanel.Update();
            }
        }
    }
}