<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.SubscriptionAlerts"
    Title="Subscription Alert" Codebehind="SubscriptionAlerts.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/Classic/sales3.css" rel="stylesheet" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>

    <script language="javascript" type="text/javascript">
        function ValidateDate(sender, args) {
            if (sender._selectedDate < new Date()) {
                alert("Select a date greater than the current date!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        } 
    </script>

    <!--Edit Alert-->
    <cc1:Accordion ID="SubscriptionAccordion" runat="server" FadeTransitions="True" HeaderSelectedCssClass="accordionHeaderSelected"
        RequireOpenedPane="false" TransitionDuration="300" HeaderCssClass="accordionHeader"
        ContentCssClass="accordionContent">
        <Panes>
            <cc1:AccordionPane ID="SubAlertAccordionPane" runat="server">
                <Header>
                    Subscription Alerts
                </Header>
                <Content>
                    <asp:UpdatePanel ID="SubAlertListUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellspacing="0" cellpadding="3" border="0">
                                <tbody align="left">
                                    <tr align="left">
                                        <td align="left" valign="top" class="Caption" style="margin-left: 20px;">
                                            Alerts:
                                        </td>
                                        <td align="left" valign="top" class="Entry">
                                            <asp:DropDownList ID="AlertListDropDownList" runat="server" OnSelectedIndexChanged="AlertListDropDownList_SelectedIndexChanged"
                                                Width="150px" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="EditAlertUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="EditAlertPanel" Visible="false" runat="server">
                                <table cellspacing="0" cellpadding="3" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td align="left" class="b24-doc-subtitle" valign="top">
                                                <span class="Caption">Edit this Alert</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="3" border="0" style="width: 100%; margin: 10px">
                                                    <tbody>
                                                        <asp:Label ID="EditAlertErrorLabel" runat="server" Text="" CssClass="b24-errortext"
                                                            Visible="false"></asp:Label>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table cellspacing="0" cellpadding="3" border="0">
                                    <tbody>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Title:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:TextBox ID="TitleUpdateTextBox" runat="server" ToolTip="Alert title" MaxLength="255"
                                                    Width="336px">
                                                </asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:RequiredFieldValidator ID="TitleRequiredValidator" ValidationGroup="SubscriptionAlerts"
                                                    runat="server" ControlToValidate="TitleUpdateTextBox" EnableClientScript="true"
                                                    ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="CreatorId" runat="server" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Recipients:<br />
                                                (comma-separated)
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" style="margin: 20px;" valign="top">
                                                <asp:TextBox ID="RecipientsUpdateTextBox" runat="server" Columns="40" Height="80px"
                                                    Rows="4" Width="336px" ToolTip="Recipients Email" TextMode="MultiLine" MaxLength="255"
                                                    Font-Names="Verdana, Arial, Helvetica, sans-serif" Font-Size="12px"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:RequiredFieldValidator ID="emailIdRequiredValidator" ValidationGroup="SubscriptionAlerts"
                                                    runat="server" ControlToValidate="RecipientsUpdateTextBox" ErrorMessage="*" Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="emailFormatValidator" runat="server" Display="Dynamic"
                                                    ControlToValidate="RecipientsUpdateTextBox" ErrorMessage="Invalid Email" ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Status:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:DropDownList ID="StatusList" runat="server">
                                                    <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Closed" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Due Date:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:TextBox ID="DueDateUpdateTextBox" runat="server" Enabled="false" ToolTip="Due Date"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="PickDueDate" ImageUrl="~/images/Calendar.gif"
                                                    ToolTip="Pick a date" />
                                                <cc1:CalendarExtender ID="DueDateUpdateCalendarExtender" Animated="true" PopupPosition="BottomRight"
                                                    runat="server" TargetControlID="DueDateUpdateTextbox" Format="MMM dd, yyyy" OnClientDateSelectionChanged="ValidateDate"
                                                    PopupButtonID="PickDueDate" />
                                            </td>
                                            <td align="left">
                                                <asp:RequiredFieldValidator ID="DueDateValidator" ValidationGroup="SubscriptionAlerts"
                                                    runat="server" ControlToValidate="DueDateUpdateTextBox" ErrorMessage="*" Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Notes:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:TextBox ID="NotesUpdateTextBox" Rows="9" Columns="40" runat="server" Width="336px"
                                                    Height="200px" ToolTip="Notes" TextMode="MultiLine" Font-Names="Verdana, Arial, Helvetica, sans-serif"
                                                    Font-Size="12px"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:RequiredFieldValidator ID="NotesFieldValidator" ValidationGroup="SubscriptionAlerts"
                                                    runat="server" ControlToValidate="NotesUpdateTextBox" ErrorMessage="*" Display="Dynamic">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td align="right" class="Caption" valign="top">
                                                Created:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:Label ID="CreatedLabel" runat="server"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="Caption" valign="top">
                                                LastUpdated:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:Label ID="LastUpdatedLabel" runat="server"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="Caption" valign="top">
                                                Send Email:
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" class="Entry" valign="top">
                                                <asp:CheckBox ID="SendEmailUpdateCheckBox" runat="server" Checked="false" />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="3" valign="top">
                                                <asp:Button ID="UpdateButton" TabIndex="5" class="Button" runat="server" Text="Update"
                                                    ToolTip="Update Alert " OnClick="UpdateButton_Click" ValidationGroup="SubscriptionAlerts"
                                                    Visible="false" />
                                                <asp:HiddenField ID="UpdatedSubAlertId" runat="server" />
                                                <asp:HiddenField ID="IsEdit" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </tbody> </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane ID="CreateAlertAccordionPane" runat="server" Visible="false" >
                <Header>
                    Create A New Alert
                </Header>
                <Content>
                    <asp:UpdatePanel ID="CreateNewAlertPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="3" border="0">
                                        <tbody>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="NewAlertErrorLabel" runat="server" Text="Alert Created" CssClass="b24-errortext"
                                                        Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr align="right">
                                                <td align="right" class="Caption" valign="top">
                                                    Title:
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" class="Entry" valign="top">
                                                    <asp:TextBox ID="TitleTextBox" runat="server" ToolTip="Enter the alert title" Width="336px"
                                                        MaxLength="255"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="TitleValidator" ValidationGroup="SubscriptionCreateAlerts"
                                                        runat="server" ControlToValidate="TitleTextBox" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="right" class="Caption" valign="top">
                                                    Recipients :<br />
                                                    (comma-separated)
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" class="Entry" style="padding-left" width="365px" valign="top">
                                                    <asp:TextBox ID="RecipientsTextBox" Rows="4" Columns="50" runat="server" Width="336px"
                                                        Height="80px" ToolTip="Recipients Email Address" TextMode="MultiLine" Font-Names="Verdana, Arial, Helvetica, sans-serif"
                                                        Font-Size="12px"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RecipientsValidator" ValidationGroup="SubscriptionCreateAlerts"
                                                        runat="server" ControlToValidate="RecipientsTextBox" ErrorMessage="*" Display="Dynamic">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RecipientsExpressionValidator" runat="server"
                                                        Display="Dynamic" ControlToValidate="RecipientsTextBox" ErrorMessage="Invalid Email"
                                                        ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$">
                                                    </asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="right" class="Caption" valign="top">
                                                    Due Date:
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" class="Entry" valign="top" style="width: 365px">
                                                    <asp:TextBox ID="SubAlertDueDateTextBox" runat="server" Enabled="false" ToolTip="Due Date"></asp:TextBox>
                                                    <asp:ImageButton runat="server" ID="PickDate" ImageUrl="~/images/Calendar.gif" ToolTip="Pick a Date" />
                                                    <cc1:CalendarExtender Animated="true" PopupPosition="BottomRight" runat="server"
                                                        TargetControlID="SubAlertDueDateTextBox" Format="MMM dd, yyyy" OnClientDateSelectionChanged="ValidateDate"
                                                        PopupButtonID="PickDate" />
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="SubAlertDueDateValidator" ValidationGroup="SubscriptionCreateAlerts"
                                                        runat="server" ControlToValidate="SubAlertDueDateTextBox" ErrorMessage="*" Display="Dynamic">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="Caption" style="height: 81px;" valign="top">
                                                    Notes:
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" class="Entry" valign="top" style="width: 365px; height: 81px;">
                                                    <asp:TextBox ID="SubAlertsNotesTextBox" Rows="9" Columns="40" runat="server" Width="336px"
                                                        Height="200px" ToolTip="Enter Notes" Font-Names="Verdana, Arial, Helvetica, sans-serif"
                                                        Font-Size="12px" TextMode="MultiLine"> 
                                                    </asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="SubAlertsNotesValidator" ValidationGroup="SubscriptionCreateAlerts"
                                                        runat="server" ControlToValidate="SubAlertsNotesTextBox" ErrorMessage="*" Display="Dynamic">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="Error" valign="top">
                                                    Send Email:
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" class="Entry" valign="top">
                                                    <asp:CheckBox ID="SubAlertSendEmailCheckBox" runat="server" Checked="true" />
                                                    &nbsp;
                                                </td>
                                                </td>
                                                <td>
                                            </tr>
                                            <tr>
                                            <td colspan="2"></td>
                                                <td align="right" class="Entry"  valign="top" style="width:343px;">
                                                    <asp:Button ID="CreateButton" TabIndex="5" class="Button" runat="server" Text="Create"
                                                        ToolTip="Create Alert " OnClick="CreateButton_Click" ValidationGroup="SubscriptionCreateAlerts" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>
</asp:Content>
