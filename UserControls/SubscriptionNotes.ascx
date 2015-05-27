<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.SubscriptionNotes" Codebehind="SubscriptionNotes.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Subscription Notes</p>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="SubscriptionNotesErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ViewPanel" runat="server">
<ContentTemplate>
<asp:MultiView ID="Multiview" runat="server">
    <asp:View ID="EditView" runat="server">
           <table border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:Label ID="NotesLabel" runat="server" Text="Notes  :"></asp:Label>
                    <asp:Label ID="SubscriptionNotesLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:TextBox ID="SubscriptionNotesTextBox" runat="server" Height="100px" Width="350"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="AddButton" runat="server" Text="Add" OnClick="AddButton_Click" Visible="false" />
                    <asp:Button ID="EditCancelButton" runat="server" Text="Cancel" OnClick="EditCancelButton_Click" />
                </td>
            </tr>
        </table>       
    </asp:View>
    <asp:View ID="ReadView" runat="server">
        <table border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:Label ID="NotesReadLabel" runat="server" Text="Notes  :"></asp:Label>
                    <asp:Label ID="SubscriptionNotesReadLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" />
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
</ContentTemplate>
</asp:UpdatePanel>
