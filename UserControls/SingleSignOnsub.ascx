<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.SingleSignOnsub" Codebehind="SingleSignOnsub.ascx.cs" %>
<link href="App_Themes/Classic/sales4.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Single Sign on Subscription</p>
<table border="0" cellspacing="1" cellpadding="1">
    <tr>
        <td colspan="3">
            <asp:Label ID="SingleSignOnErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
            <br />
        </td>
    </tr>
    <tr>
        <td colspan="3">
        </td>
    </tr>
    <tr>
        <td style="width: 120px">
            <asp:Label ID="Label1" runat="server" Text="Single Sign On :"></asp:Label>
        </td>
        <td colspan="2" align="left">
            <asp:RadioButtonList ID="SingleSignOnRadioButtonList" CellPadding="2" RepeatDirection="Horizontal"
                RepeatColumns="3" CellSpacing="0" runat="server" TextAlign="Right" TabIndex="1"
                ToolTip="PreAuthentication Method">
                <asp:ListItem Value="AICC" Text="AICC" Selected="True"></asp:ListItem>
                <asp:ListItem Value="Ticket" Text="Ticket"></asp:ListItem>
                <asp:ListItem Value="Reference" Text="Reference Point"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Group Code :"></asp:Label>
        </td>
        <td colspan="2" align="left">
            <asp:TextBox ID="SingleSignOnGroupCode" runat="server" ToolTip="GroupCode" Width="200"
                TabIndex="2"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="TimeOut URL :"></asp:Label>
        </td>
        <td align="left">
            <asp:TextBox ID="TimeoutUrl" runat="server" ToolTip="Set time out Url" Width="200"
                TabIndex="3"></asp:TextBox>
        </td>
        <td>
            <asp:RegularExpressionValidator ID="TimeoutURLValidator" runat="server" ErrorMessage="Invalid URL, Provide a valid url like 'http://books24x7.com'"
                ControlToValidate="TimeoutUrl" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td colspan="2" align="left">
            <asp:CheckBox ID="SetSiteLicenseCheckBox" Text="Set Site License" runat="server" ToolTip="Set Site License"
                Checked="false" TabIndex="4" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="height: 15px">
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="UpdateButton" Text="Update" runat="server" OnClick="UpdateButton_Click"
                ToolTip="Update Single Sign On" TabIndex="5" />
        </td>
        <td>
        </td>
    </tr>
</table>
