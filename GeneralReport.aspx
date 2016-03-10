<%@ Page Language="C#" MasterPageFile="~/BaseMaster.master" AutoEventWireup="true"
    CodeBehind="GeneralReport.aspx.cs" Inherits="B24.Sales4.UI.GeneralReport" %>
<%@ Register Src="~/UserControls/AdvanceReport.ascx" TagName="AdvanceReportControl"
    TagPrefix="AdvanceReport" %>
<%@ Register Src="~/UserControls/ManageAccount.ascx" TagName="AccountInfoControl"
    TagPrefix="ManageAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div>
        <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator." Visible="false" CssClass="b24-errortext"></asp:Label>
    </div>
    <ManageAccount:AccountInfoControl ID="ManageAccountUserControl" runat="server" />
    <asp:UpdatePanel ID="AdvanceReportUpdatePanel" runat="server">
        <ContentTemplate>
            <AdvanceReport:AdvanceReportControl ID="AdvanceReportUserControl" runat="server" ShowHeaderText="True" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>