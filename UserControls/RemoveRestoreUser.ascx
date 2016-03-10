<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.RemoveRestoreUser" Codebehind="RemoveRestoreUser.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="RemoveRestoreUserErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:Label ID="RemoveRestoreUserTitleTextLabel" runat="server" Text="Remove this user from their subscription or restore this user to their subscription"
                CssClass="b24-helptext" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="RemoveUserLabel" runat="server" Text="Remove this user from their subscription"
                Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="RestoreUserLabel" runat="server" Text="Restore this user to their subscription?"
                Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:CheckBox ID="ConfirmCheckBox" runat="server" Text="Confirm" Visible="false" />
        </td>
    </tr>
    <tr>
        <td align="center">
            
            <asp:Button ID="UpdateButton" runat="server" OnClick="UpdateButton_Click" Visible="false" />           
        </td>
    </tr>
</table>
