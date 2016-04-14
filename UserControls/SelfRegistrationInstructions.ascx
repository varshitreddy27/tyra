<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.SelfRegistrationInstructions" Codebehind="SelfRegistrationInstructions.ascx.cs" %>
<link href="App_Themes/Classic/sales4.css" rel="stylesheet" type="text/css" />
<table cellpadding="1" cellspacing="1">
    <tr>
        <td colspan="2">
            <asp:Label ID="Errorlabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="InstructionsTextArea" runat="server" TextMode="MultiLine" Width="400"
                Height="200" TabIndex="1" ToolTip="Registration Instructions" Font-Names="Arial"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator CssClass="b24-errortext" ID="InstructionsRequiredFieldValidator"
                runat="server" ErrorMessage="**" ControlToValidate="InstructionsTextArea" ValidationGroup="SelfRegistrationInstructions"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td style="height: 15px" colspan="2">
        </td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:Button ID="UpdateButton" OnClick="UpdateButton_Click" runat="server" Text="Update"
                ValidationGroup="SelfRegistrationInstructions" TabIndex="2" />
        </td>
    </tr>
</table>
