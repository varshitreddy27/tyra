<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Home.aspx.cs" Inherits="B24.Sales4.UI.Home"
    MasterPageFile="~/BaseMaster.Master" %>

<%@ Register TagName="RecentAccount" TagPrefix="Sales3" Src="~/UserControls/RecentlyUpdatedAccount.ascx" %>
<asp:Content ID="MainViewContent" runat="server" ContentPlaceHolderID="Content">
    <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator." Visible="false" CssClass="b24-errortext"></asp:Label>
    <asp:UpdatePanel ID="MainViewUpdatePanel" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <Sales3:RecentAccount ID="RecentlyUpdated" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
