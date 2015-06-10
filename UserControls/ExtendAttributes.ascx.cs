using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data.SqlClient;
using System.Globalization; 

public partial class ExtendAttributes : System.Web.UI.UserControl
{
    #region Private Members
    private GlobalVariables global = GlobalVariables.GetInstance();
    private Logger logger = new Logger(Logger.LoggerType.AccountInfo);
    private BasePage baseObject;
    #endregion

    #region Public Property
    /// <summary>
    /// Subscription Id
    /// </summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// To set Read only View or Edit View
    /// </summary>
    public bool SaveButtonView { get; set; }
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
    /// To Add New Attribute for dropdown list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonAddNewAttribute_Click(object sender, EventArgs e)
    {
        ListItem itm = new ListItem(TextAddNewAttribute.Text, TextAddNewAttributeId.Text);
        ListAddAttribute.Items.Add(itm);

        TextAddNewAttribute.Text = "";
        TextAddNewAttributeId.Text = "0";

        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "showpanel" + DateTime.Now.Ticks, "AddAttribute()", true);         
    }

    /// <summary>
    /// To save the Extend attribute
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonSaveAttribute_Click(object sender, EventArgs e)
    {
        try
        {
            B24.Common.ExtendAttributes extendAttr = new B24.Common.ExtendAttributes();
            ExtendAttributesFactory extendAttrFac = new ExtendAttributesFactory(baseObject.UserConnStr);

            extendAttr.AttributeName = TextAttributeName.Text.Trim();
            extendAttr.AttributeType = DropDownAttributeType.SelectedValue;
            extendAttr.InputRequired = (CheckInputRequired.Checked) ? 1 : 0;
            extendAttr.HasDomain = (CheckHasDomain.Checked) ? 1 : 0;
            extendAttr.DefaultValue = TextDefaultValue.Text.Trim();
            extendAttr.AttributeDescription = TextDisplayName.Text.Trim();

            extendAttrFac.PutExtendAttribute(SubscriptionId, extendAttr);

            if (extendAttr.HasDomain == 1)
            {
                if (extendAttr.FieldValues != null)
                {
                    extendAttr.FieldValues.Clear();
                }
                else
                {
                    extendAttr.FieldValues = new List<ExtendAttributeValues>();
                }
                for (int i = 0; i < ListAddAttribute.Items.Count; i++)
                {
                    ExtendAttributeValues extAttrValues = new ExtendAttributeValues();
                    extAttrValues.AttributeName = TextAttributeName.Text;
                    extAttrValues.AttributeValue = ListAddAttribute.Items[i].Text;
                    extAttrValues.AttributeID = Convert.ToInt64(ListAddAttribute.Items[i].Value, CultureInfo.InvariantCulture);
                    extendAttr.FieldValues.Add(extAttrValues);
                }

                extendAttrFac.PutExtAttrDomain(SubscriptionId, extendAttr);
            }

            ExtendAttributeErrorLabel.Text = Resources.Resource.ExtendAttributeAdded;
            ExtendAttributeErrorLabel.Visible = true;
            ClearSaveAttribute();
           
        }
        catch (SqlException ex)
        {
            logger.Log(Logger.LogLevel.Error, Resources.Resource.ErrorAddingExtendAttribute, ex);

            ExtendAttributeErrorLabel.Text = Resources.Resource.ErrorAddingExtendAttribute;
            ExtendAttributeErrorLabel.Visible = true;

        }
    }
    private void ClearSaveAttribute()
    {
        TextAttributeName.Text = String.Empty;
        CheckInputRequired.Checked = false;
        TextDefaultValue.Text = String.Empty;
        TextDisplayName.Text = String.Empty;
        CheckHasDomain.Checked = false;
        ListAddAttribute.Items.Clear();
        TextAddNewAttribute.Text = string.Empty;
        TextAddNewAttributeId.Text = string.Empty;

 
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        baseObject = this.Page as BasePage;
        if (SubscriptionId == null || SubscriptionId == Guid.Empty)
        {
            return;
        }
        ButtonSaveAttribute.Visible = SaveButtonView;
    }
}

