<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.AdvancedUserPermissions" Codebehind="AdvancedUserPermissions.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="AdvancedUserPermissionErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
                <td>
                    <asp:Label ID="AdminChangeTextLabel" runat="server" Text="Adjust this Users's entitlements."
                        CssClass="b24-helptext"></asp:Label>
                </td>
            </tr>
</table>
<asp:MultiView ID="Multiview" runat="server">
    <asp:View ID="EditView" runat="server">
        <table border="0" cellpadding="1" cellspacing="1">            
            <tr>
                <td>
                    <asp:CheckBox ID="CFACheckBox" runat="server" Text="Corporate Folder Administrator (CFA)"  Checked="false"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="CTACheckBox" runat="server" Text="Corporate Tropic Tree Administrator (CTA)" Checked="false"/>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_Click" />
                    <asp:Button ID="EditCancelButton" runat="server" Text="Cancel" OnClick="EditCancelButton_Click" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ReadView" runat="server">
        <table border="0" cellpadding="1" cellspacing="1">            
            <tr>
                <td>
                    <asp:CheckBox ID="CFAReadCheckBox" runat="server" Text="Corporate Folder Administrator (CFA)" Checked="false"
                        Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="CTAReadCheckBox" runat="server" Text="Corporate Tropic Tree Administrator (CTA)" Checked="false"
                        Enabled="false" />
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