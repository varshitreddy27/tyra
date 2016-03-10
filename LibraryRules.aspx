<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales4.UI.LibraryRules" Title="Manage Library Rules" Codebehind="LibraryRules.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <p class="b24-doc-title">Library Authentication Rules</p>
    <%-- Show the Sub Info Panel (to do: make this a control) --%>
    <asp:Panel ID="SubInfoPanel" runat="server" CssClass="b24-report-frame">
      <table class="b24-subscription" border="0" cellpadding="2" cellspacing="2">
        <tr><td>Account Code:</td><td><asp:Label ID=AccountCodeLabel runat=server /></td></tr>
        <tr><td>Company Name:</td><td><asp:Label ID=CompanyNameLabel runat=server /></td></tr>
        <tr><td>Starts:</td><td><asp:Label ID=StartsLabel runat=server /></td></tr>
        <tr><td>Expires:</td><td><asp:Label ID=ExpiresLabel runat=server /></td></tr>
        <tr><td>Seats:</td><td><asp:Label ID=SeatsLabel runat=server /></td></tr>
        <tr><td></td><td class="b24-report-link-label"><asp:HyperLink ID=AccountInfoHyperLink runat=server><IMG align=absmiddle SRC="images/Go.gif" width=22 height=15 /></asp:HyperLink>AccountInfo</td></tr>
        <tr><td></td><td class="b24-report-link-label"><asp:HyperLink ID=ManageAccountHyperLink runat=server><IMG align=absmiddle SRC="images/Go.gif" width=22 height=15 /></asp:HyperLink>ManageAccount</td></tr>
        <tr><td></td><td class="b24-report-link-label"><asp:HyperLink ID=AdvancedReportHyperLink runat=server><IMG align=absmiddle SRC="images/Go.gif" width=22 height=15 /></asp:HyperLink>Reports</td></tr>
      </table>
    </asp:Panel>

    <%-- Now Show the actual rules for this sub --%>
    <br />
    <asp:Panel ID="RulesPanel" runat="server" CssClass="b24-report-frame">
      <asp:GridView ID="RulesGridView" runat="server"  AutoGenerateColumns="false" 
        HeaderStyle-CssClass="b24-report-title" DataKeyNames="RuleID" EditRowStyle-CssClass="b24-editrow" 
        CellPadding=4 >
        <Columns>

          <asp:TemplateField ItemStyle-Wrap=false>
            <ItemTemplate>
              <asp:ImageButton ID="EditButton" CommandArgument='<%# Eval("RuleID") %>' CommandName="Edit" 
                runat="server" ImageUrl="~/images/GVEditButton.gif" AlternateText=Edit Visible=false />
              <asp:ImageButton ID="DeleteButton" CommandArgument='<%# Eval("RuleID") %>' CommandName="Delete" 
                runat="server" ImageUrl="~/images/GVDeleteButton.gif" AlternateText=Delete Visible=false 
                OnClientClick="javascript:return confirm('Delete this record?  Are you sure?');" />
              <asp:ImageButton ID="UpdateButton" CommandArgument='<%# Eval("RuleID") %>' CommandName="Update" 
                runat="server" ImageUrl="~/images/GVUpdateButton.gif" AlternateText=Update Visible=false />
              <asp:ImageButton ID="CancelButton" CommandArgument='<%# Eval("RuleID") %>' CommandName="Cancel" 
                runat="server" ImageUrl="~/images/GVCancelButton.gif" AlternateText=Cancel Visible=false />
            </ItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="IP Mask" ItemStyle-Wrap=false>
            <ItemTemplate>
              <asp:Label ID=IPAddrMaskLabel runat=server Text='<%# Eval("IPAddrMask") %>'/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:TextBox ID=IPAddrMaskTextBox runat=server Text='<%# Eval("IPAddrMask") %>' Width=150 />
            </EditItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="URL Mask" ItemStyle-Wrap=false>
            <ItemTemplate>
              <asp:Label ID=UrlMaskLabel runat=server Text='<%# Eval("UrlMask") %>'/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:TextBox ID=UrlMaskTextBox runat=server Text='<%# Eval("UrlMask") %>' Width=150 />
            </EditItemTemplate>
          </asp:TemplateField>


          <asp:TemplateField HeaderText="Disallow Access<br />From This IP" ItemStyle-HorizontalAlign=center >
            <ItemTemplate>
              <asp:CheckBox ID=DisallowCheckBox runat=server Checked='<%# Eval("Disallow") %>' Enabled=false/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:CheckBox ID=DisallowCheckBox runat=server Checked='<%# Eval("Disallow") %>' />
            </EditItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Priority" ItemStyle-HorizontalAlign=center>
            <ItemTemplate>
              <asp:Label ID=PriorityLabel runat=server Text='<%# Eval("Priority") %>'/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:TextBox ID=PriorityTextBox runat=server Text='<%# Eval("Priority") %>' Width=20 />
            </EditItemTemplate>
          </asp:TemplateField>

          <asp:BoundField DataField=Time HeaderText="Created" DataFormatString="{0:dd-MMM-yyyy}" ReadOnly=true />

          <asp:TemplateField HeaderText="No<br/>Anonymous<br />Access" ItemStyle-HorizontalAlign=center >
            <ItemTemplate>
              <asp:CheckBox ID=NoAnonymousCheckBox runat=server Checked='<%# Eval("NoAnonymous") %>' Enabled=false/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:CheckBox ID=NoAnonymousCheckBox runat=server Checked='<%# Eval("NoAnonymous") %>' />
            </EditItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign=center >
            <ItemTemplate>
              <asp:CheckBox ID=ActiveCheckBox runat=server Checked='<%# Eval("Active") %>' Enabled=false/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:CheckBox ID=ActiveCheckBox runat=server Checked='<%# Eval("Active") %>' />
            </EditItemTemplate>
          </asp:TemplateField>

          <asp:TemplateField HeaderText="Notes">
            <ItemTemplate>
              <img src="images/_.gif" height=1 width=150/><br />
              <asp:Label ID=CommentLabel runat=server Text='<%# Eval("Comment") %>'/>
            </ItemTemplate>
            <EditItemTemplate>
              <asp:TextBox ID=CommentTextBox runat=server TextMode=multiLine Text='<%# Eval("Comment") %>' Width=150 Height=40/>
            </EditItemTemplate>
          </asp:TemplateField>

        </Columns>
      </asp:GridView>
    </asp:Panel>
    
  <%-- 
      Finally, show the Add New Rule Panel
      The select control queries the the type of rule from the user; 
      The remianing form controls are hidden.
      When the text changes in teh select control, a client-side javascript function
      shows or hides the form fields that are relevant for the selected type of rule. 
  --%>
  <asp:Panel ID="AddRulePanel" runat="server" CssClass="b24-report-frame">
    <p class="b24-doc-title2">Add a New Rule</p>
    <div class="Label">Rule Type:</div>
    <div class="Input">
      <select name="RuleType" ID="RuleTypeDropDownList" OnChange="javascript:ToggleFormDetails(this)">
        <option Value="">Select One</option>
        <option Value="IP">IP</option>
        <option Value="URL">URL</option>
        <option Value="EZProxy">EZProxy Secure</option>
      </select>
    </div>
    <div id="AddRule" style="display:none">
        <div id="IP" style="visibility:hidden">
          <div class="Label">IP Mask:</div>
          <div class="Input"><asp:TextBox ID="IPTextBox" runat="server" Width=200 /></div>
        </div>
        <div id="URL" style="visibility:hidden">
          <div class="Label">URL Mask:</div>
          <div class="Input"><asp:TextBox ID="URLTextBox" runat="server" Width="200" /></div>
        </div>
        <div id="EZProxy" style="visibility:hidden">
          <div class="Label">URL Mask:</div>
          <div class="Input"><asp:TextBox ID="EZProxyTextBox" runat="server" Width="200" Enabled="false" Text="ezproxy://" /></div>
        </div>
        <div id="Notes" style="visibility:hidden">
          <div class="Label">Notes:</div>
          <div class="Input"><asp:TextBox ID="NotesTextBox" runat="server" TextMode="multiLine" Width="150" Height="40" /></div>
        </div>
        <div id="Priority" style="visibility:hidden">
          <div class="Label">Priority:</div>
          <div class="Input"><asp:TextBox ID="PriorityTextBox" runat="server" Width="20" /></div>
        </div>
        <div id="Disallow" style="visibility:hidden">
          <div class="InputCheckbox"><asp:CheckBox ID="DisallowCheckBox" runat="server" /></div>
          <div class="LabelCheckbox">Disallow Access</div>
        </div>
        <div id="Add" style="visibility:hidden">
          <asp:Button ID="AddButton" runat="server" Text="Add" />
        </div>
    </div>
  </asp:Panel>
    <asp:Panel ID="AddDomainPanel" runat="server" CssClass="b24-report-frame" Visible="false">
        <p class="b24-doc-subtitle">Add an Email Domain</p>
        <p class="b24-helptext">Allow users from the specified domain to register into this account.  Enter the at sign followed by the full domain (e.g., @skillsoft.com)</p>
        <div class="Label"><asp:Label ID="EmailDomainLabel" runat="server" Text="New Domain" CssClass="b24-forminputlabel"></asp:Label></div>
        <div class="Input"><asp:TextBox runat="server" ID="EmailDomainTextBox" CssClass="b24-forminput" MaxLength="96"></asp:TextBox>
        <asp:RegularExpressionValidator ID="EmailDomainValidator" ControlToValidate="EmailDomainTextBox" CssClass="b24-forminputlabel" Display="Dynamic" ErrorMessage="Invalid Domain Name. 
        domains must be in the form @domain.com" runat="server" ValidationExpression="^@[\w.']*\.\w{2,3}$"></asp:RegularExpressionValidator></div>
        <asp:CheckBox ID="CB_Email" runat="server" /><span class="b24-helptext">Users can register with any/all email domains</span>
    </asp:Panel>
       <asp:Panel ID="RemoveDomainPanel" runat="server" CssClass="b24-report-frame" Visible="false">
        <p class="b24-doc-subtitle"> Remove an Existing Email Domain</p>
        <p class="b24-helptext">Select domain(s) to remove.</p>
        <div><asp:CheckBoxList runat="server" ID="EmailDomainCollection" RepeatDirection="Vertical"></asp:CheckBoxList></div>
    </asp:Panel>
    <div id="UpdateEmailDomainDiv" runat="server" class="b24-report-frame" visible="false"><asp:Button Text="Update" ID="UpdateEmailDomain" runat="server" OnClick="SubmitClick_UpdateDomain" /></div>
    
    <asp:Panel ID="SharedSecretPanel" runat="server" CssClass="b24-report-frame" Visible="false">    
        <hr />  
        <p class="b24-doc-subtitle">Shared Secret</p>
        <p class="b24-helptext">Add a shared secret for use with tokens and EZProxy.
        Click on the Generate button and the system will generate and save a new shared secret.  Or manually enter a shared secret and click Save.</p>
        <div><asp:Label runat="server" CssClass="b24-forminputlabel" Text="Existing Shared Secret: "></asp:Label><asp:Label ID="SharedSecretCode" CssClass="b24-forminputlabel" runat="server"></asp:Label></div>
        <br />
        <div>
        <%--<asp:ScriptManager ID="SharedSecretScriptManager" EnablePartialRendering="true" runat="server" />--%>
        <asp:UpdatePanel ID="SharedSecretUpdatePanel" runat="server" >
        <ContentTemplate>
        <table>
        <tr><td><asp:Button runat="server" ID="Button_GenerateSharedSecretCode" Text="Generate new shared secret" OnClick="SubmitClick_NewSharedSecretCode" Width="200"/></td></tr>
        <tr><td style="text-align:center">OR</td></tr>
        <tr><td><asp:TextBox runat="server" CssClass="b24-forminput" ID="NewSharedSecretCode" MaxLength="64"></asp:TextBox><asp:Button Text="Save" ID="SubmitSharedSecret" runat="server" OnClick="SubmitClick_SharedSecret"/></td></tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <hr />    
    </asp:Panel>     
  <script type="text/javascript">
  //=========================================================================
  // Show the form elements that are relevent for the given rule type
  // type: The type of rule from the select control
  //=========================================================================
  function ToggleFormDetails(type)
  {
    // Get all the form components
    var ip = document.getElementById("IP");
    var url = document.getElementById("URL");
    var ezproxy = document.getElementById("EZProxy");
    var notes = document.getElementById("Notes");
    var priority = document.getElementById("Priority");
    var disallow = document.getElementById("Disallow");
    var add = document.getElementById("Add");
    var addrule = document.getElementById("AddRule");
    
    // Hide everyone
    addrule.style.visibility = "hidden";
    ip.style.visibility = "hidden";
    url.style.visibility = "hidden";
    ezproxy.style.visibility = "hidden";
    notes.style.visibility = "hidden";
    priority.style.visibility = "hidden";
    disallow.style.visibility = "hidden";
    add.style.visibility = "hidden";

    // Everyone starts out displayed
    addrule.style.display = "inline";
    ip.style.display = "inline";
    url.style.display = "inline";
    ezproxy.style.display = "inline";
    notes.style.display = "inline";
    priority.style.display = "inline";
    disallow.style.display = "inline";
    add.style.display = "inline";
 
    // Now show the elements that are relevent for this type of rule
    switch(type.value)
    {
      case "IP":
        addrule.style.visibility = "visible";
        ip.style.visibility = "visible";
        url.style.display = "none";
        ezproxy.style.display = "none";
        notes.style.visibility = "visible";
        priority.style.visibility = "visible";
        disallow.style.visibility = "visible";
        add.style.visibility = "visible";
        break;
      case "URL":
        addrule.style.visibility = "visible";
        ip.style.display = "none";
        url.style.visibility = "visible";
        ezproxy.style.display = "none";
        notes.style.visibility = "visible";
        priority.style.visibility = "visible";
        disallow.style.display = "none";
        add.style.visibility = "visible";
        break;
      case "EZProxy":
        addrule.style.visibility = "visible";
        ip.style.display = "none";
        url.style.display = "none";
        ezproxy.style.visibility = "visible";
        notes.style.visibility = "visible";
        priority.style.display = "none";
        disallow.style.display = "none";
        add.style.visibility = "visible";
        break;
      default:
        addrule.style.display = "none";
        break;
    }
  }
  </script>
</asp:Content>
