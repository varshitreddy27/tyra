<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.UserAdministrator" Codebehind="UserAdministrator.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="AdminPermissionsErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<asp:MultiView ID="Multiview" runat="server">
    <asp:View ID="EditView" runat="server">
        <table border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:Label ID="AdminChangeTextLabel" runat="server" Text="Change the User Administrator for this subscription."
                        CssClass="b24-helptext" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="CurrentAdminUserTitleLabel" Text="This user is currently the User Administrator"
                        runat="server" Visible="false" CssClass="b24-formdescription"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="RemoveAdministratorLabel" Text="Remove this user from Administrators: "
                        runat="server" Visible="false" CssClass="b24-forminputlabel"></asp:Label>
                    <asp:Label ID="RemoveCurrentAdministratorLabel" runat="server" Visible="false" CssClass="b24-inputcentralxml"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="CurrentAdminLabel" runat="server" Text="Current User administrator: "
                        Visible="false"></asp:Label>
                    <asp:Label ID="AddCurrentAdministratorTextLabel" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="AddAdministratorTextLabel" runat="server" Text="Add this User to administrators: "
                        Visible="false"></asp:Label>
                    <asp:Label ID="AddAdministratorLabel" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="ConfirmCheckBox" runat="server" Text="Confirm" Visible="true" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="UpdateButton" runat="server" Text="Update" Visible="false" OnClick="UpdateButton_Click" />
                    <asp:Button ID="EditCancelButton" runat="server" Text="Cancel" Visible="false" OnClick="EditCancelButton_Click" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ReadView" runat="server">
        <table border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:Label ID="AdminChangeTextReadLabel" runat="server" Text="Change the User Administrator for this subscription."
                        CssClass="b24-helptext" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="CurrentAdminUserTitleReadLabel" Text="This user is currently the User Administrator"
                        runat="server" Visible="false" CssClass="b24-formdescription"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="RemoveAdministratorReadLabel" Text="Remove this user from Administrators: "
                        runat="server" Visible="false" CssClass="b24-forminputlabel"></asp:Label>
                    <asp:Label ID="RemoveCurrentAdministratorReadLabel" runat="server" Visible="false"
                        CssClass="b24-inputcentralxml"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="CurrentAdminReadLabel" runat="server" Text="Current User administrator: "
                        Visible="false"></asp:Label>
                    <asp:Label ID="AddCurrentAdministratorTextReadLabel" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="AddAdministratorTextReadLabel" runat="server" Text="Add this User to administrators: "
                        Visible="false"></asp:Label>
                    <asp:Label ID="AddAdministratorReadLabel" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click"
                        Width="75" />
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
<asp:HiddenField ID="AddorRemoveConfirmHiddenField" runat="server" />
