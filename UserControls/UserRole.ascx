<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRole.ascx.cs" Inherits="B24.Sales4.UserControls.UserRole" %>
<table>
    <tr>
        <td>
            <asp:Label ID="RoleLable" runat="server" Text="Change the role for this user." CssClass="b24-helptext">
            </asp:Label>
        </td>
    </tr>
    <tr>
    <td align="left"></td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="UserRoleDropDownList" runat="server" AutoPostBack="true" EnableViewState="true"
                OnSelectedIndexChanged="UserRoleDropDownList_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_Click" />
        </td>
    </tr>
</table>
