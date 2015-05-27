<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UserControl.Impersonate" Codebehind="Impersonate.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title">
    Impersonation</p>
<table border="0" cellpadding="0" cellspacing="0" class="b24-form">
    <tr>
        <td>
            <asp:Label ID="ImpersonateError" runat="server" Text="" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
        <td>
            <asp:HiddenField ID="UserIdHidden" runat="server" Value="" />
            <asp:Button ID="ImpersonateButton" runat="server" Text="Impersonate" OnClick="ImpersonateButton_Click" />
        </td>
    </tr>
</table>
