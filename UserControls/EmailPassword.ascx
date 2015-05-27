<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.EmailPassword" Codebehind="EmailPassword.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="EmailPasswordErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:Label ID="EmailPasswordTitleTextLabel" runat="server" Text="Have the system email a complete Welcome Package (including login and password information) to the specified email address."
                CssClass="b24-helptext" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="noDirectLoginLabel" runat="server" Text="Cannot send password email, direct login not allowed for this subscription."
                CssClass="b24-helptext" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="noEmailIdLabel" runat="server" Text="Cannot send password email, no email address on file for this user."
                CssClass="b24-helptext" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="EmailPasswordLabel" runat="server" Text="Email Address : " Visible="false"></asp:Label>
            <asp:Label ID="EmailPasswordEmailLabel" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Button ID="SendButton" runat="server" Text="Send" OnClick="SendButton_Click"
                Visible="false" Width="75" />
        </td>
    </tr>
</table>
