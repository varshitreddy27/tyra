<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales4.UserControl.Alerts" CodeBehind="Alerts.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<!-- add styles -->
	 
<script type="text/javascript">
    if (typeof jQuery == 'undefined') {
        document.write(unescape("%3Cscript src='Scripts/jquery-1.9.1.min.js' type='text/javascript'%3E%3C/script%3E"));
        document.write(unescape("%3Cscript src='Scripts/jquery-ui-1.9.0.min.js' type='text/javascript'%3E%3C/script%3E"));
    }
</script>

<div id="mainAlertDiv">
        <div class="b24-doc-subtitle">
            <asp:Label runat="server" ID="AlertListLbl" meta:resourceKey="AlertListLabel"></asp:Label>
        </div>
        <div id="errorDiv">
        <asp:UpdatePanel ID="AlertListErrorUpdatePnl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="AlertListErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <div>
            <asp:UpdatePanel ID="AlertListUPnl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <asp:Label runat="server" ID="noAlertsLbl" meta:resourceKey="noAlertsLabel" Visible="false"></asp:Label>
                    <table id="alertListTable" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:GridView ID="SubscriptionAlertGridview" runat="server" AutoGenerateColumns="False"
                                    CssClass="resultGrid" CellPadding="4" OnRowDataBound="SubscriptionAlertGridView_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Alert Title" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ViewLinkButton" runat="server" CausesValidation="false" CssClass="GoButton" CommandArgument='<%#Eval("subscriptionAlertId") %>' Text='<%# Eval("Title") %>' ToolTip="Click to View Alert Details" OnClick="ViewLinkButton_Click" CommandName="OpenAlertDetails" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:Label ID="StatusLbl" runat="server" Text='<%# Eval("StatusString") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:Label ID="DueDateLbl" runat="server" Text='<%# Eval("DueDateShort") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                            <ItemTemplate>
                                                 <asp:LinkButton ID="RemoveLinkButton" runat="server" meta:resourceKey="RemoveLinkButton" CausesValidation="false" CssClass="GoButton" CommandArgument='<%#Eval("subscriptionAlertId") %>'  OnClick="RemoveLinkButton_Click" CommandName="DeleteAlert" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
                <br /><br />
   
        <asp:Panel ID="newAlertPnl" Visible="true" runat="server">
            
            
            <asp:Panel ID="ViewPanel" Visible="true" runat="server">
                <asp:UpdatePanel ID="ViewUpdatePanel" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="b24-doc-subtitle">
                            <asp:Label ID="sectionTitleLbl" runat="server" CssClass="b24-doc-subtitle" meta:resourceKey="sectionTitleLabel"></asp:Label>
                        </div>
                        <div id="AlertView" class="b24-alerts">
                            <table>
                                <tr>
                                    <td align="left"  colspan="2">
                                        <asp:Label ID="AlertDetailErrorLabel" runat="server" Text="" CssClass="b24-errortext" Visible="false"></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr class='b24-alerts-lblcell'>
                                    <td align="left" >
                                        <asp:Label ID="titleLbl" runat="server" Text="Title:" meta:resourceKey="titleLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" >
                                        <asp:TextBox ID="titleTbx" runat="server" Text="" ReadOnly="true" CssClass="b24-alertsinput"></asp:TextBox>
                                        <asp:HiddenField ID="SubscriptionAlertIDHF" runat="server" />
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:RequiredFieldValidator ID="titleRFV" runat="server" CssClass="b24-formdescription" ControlToValidate="titleTbx" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>  
                                </tr>
                                <tr class='b24-alerts-lblcell'>
                                    <td align="left">
                                        <asp:Label ID="recipientLbl" runat="server" meta:resourceKey="recipientLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:TextBox ID="recipientTbx" runat="server" TextMode="multiline" Rows="2" Text="" ReadOnly="true" CssClass="b24-alertsinput"></asp:TextBox>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:RequiredFieldValidator ID="recipientRFV" runat="server" CssClass="b24-formdescription" ControlToValidate="recipientTbx" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class='b24-alerts-lblcell'>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="statusLbl" runat="server" meta:resourceKey="statusLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:DropDownList ID="StatusDDL" runat="server" Enabled="false" CssClass="b24-alertsinput">
                                            <asp:ListItem meta:resourceKey="StatusDDLOpen" Value="1"></asp:ListItem>
                                            <asp:ListItem meta:resourceKey="StatusDDLClosed" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class='b24-alerts-lblcell'>
                                     <td align="left">
                                        <asp:Label ID="duedateLbl" runat="server" meta:resourceKey="duedateLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:TextBox ID="DueOnTextBox" runat="server" CssClass="b24-alertsinput" ReadOnly="true"></asp:TextBox>
                                        <script>   
                                            attachCalendar();

                                            $(document).ready(function () {
                                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

                                                function EndRequestHandler(sender, args) {
                                                    attachCalendar();
                                                }
                                            });
                                            
                                            function attachCalendar() {
                                                $('#<%= DueOnTextBox.ClientID %>').datepicker({
                                                    // The format the user actually sees
                                                    dateFormat: "mm/dd/yy",
                                                    onSelect: function (date) {
                                                    }
                                                });
                                            }
                                            
                                        </script>
                                    </td>
                                    <td style="vertical-align: top;">
                                       <asp:RequiredFieldValidator ID="duedateRFV" runat="server" CssClass="b24-formdescription" ControlToValidate="DueOnTextBox" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td> 
                                </tr>
                                <tr class='b24-alerts-lblcell'>
                                     <td align="left">
                                        <asp:Label ID="notesLbl" runat="server" Text="Notes:"></asp:Label>
                                    </td>
                                </tr>
                                <tr>  
                                    <td align="left">
                                        <asp:TextBox ID="notesTbx" runat="server" Text="" TextMode="multiline" Rows="12" ReadOnly="true" CssClass="b24-alertsinput"></asp:TextBox>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:RequiredFieldValidator ID="notesRFV" runat="server" CssClass="b24-formdescription" ControlToValidate="recipientTbx" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    </tr>
                                <tr class='b24-alerts-lblcell'>
                                    <td align="left" colspan="2">
                                        <asp:CheckBox ID="sendEmailCbx" runat="server" Enabled="false" />
                                        <asp:Label ID="sendEmailLbl" runat="server" Text="Send Email"></asp:Label>        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="createdLbl" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:HiddenField ID="CreatorIDHF" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"  colspan="2">
                                        <asp:Label ID="updateLbl" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="b24-formdescription"
                                            HeaderText="Please fill in all of the required fields." />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left" colspan="2">
                                        <asp:Button runat="server" Text="Save" ID="updateAlertBtn" OnClick="UpdateAlert_Click" />
                                        <asp:Button runat="server" Text="Clear Fields" ToolTip="Clear Alert Fields" CausesValidation="false" ID="clearAlertBtn" OnClick="ClearAlert_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    <div class="s-clear"></div>
</div>




