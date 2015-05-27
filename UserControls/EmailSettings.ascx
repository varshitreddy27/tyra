<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.EmailSettings" Codebehind="EmailSettings.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td>
            <asp:Label ID="EmailSettingsErrorLabel" CssClass="b24-errortext" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" CssClass="b24-doc-title" runat="server" Text="Turn Off All Email Communcations"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Check this box to turn off all email communcations for this user (new book notifications, newsletters, announcements, etc.). "></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="TurnOffEmailCheckBox" runat="server" Text="Disable all email communcations.  ">
            </asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />
            <asp:Label ID="Label3" CssClass="b24-doc-title" runat="server" Text="Email Administrator"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:CheckBox ID="EmailAdminCheckBox" runat="server" Text="Make this user an Email Administrator.">
            </asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />
            <asp:Label ID="Label4" CssClass="b24-doc-title" runat="server" Text="New Book Notification"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:RadioButtonList ID="NewBookNotificationRadioButtonList" runat="server">
                <asp:ListItem Selected="True" Value="0">All new books</asp:ListItem>
                <asp:ListItem Value="1">New books like those on my bookshelf</asp:ListItem>
                <asp:ListItem Value="3">Don&#39;t send new books</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />
            <asp:Label ID="Label5" CssClass="b24-doc-title" runat="server" Text="New Book Notification Format"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <asp:RadioButtonList ID="NewBookFormatRadioButtonList" runat="server">
                <asp:ListItem Selected="True" Value="0">HTML with links and graphics (default)</asp:ListItem>
                <asp:ListItem Value="1">ASCII (plain text)  </asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" />
        </td>
    </tr>
    </tr>
    <tr align="center">
        <td>
            <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" />
        </td>
    </tr>
</table>
