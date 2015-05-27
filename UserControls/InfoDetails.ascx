<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UserControl.InfoDetails" Codebehind="InfoDetails.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1
    {
        width: 65px;
    }
    .auto-style1 {
        height: 17px;
    }
    .auto-style2 {
        width: 65px;
        height: 17px;
    }
</style>
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Account Information</p>
<div>
    <asp:Label ID="InfoDetailsErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label></div>
<table id="SubscriptionInfoTable" border="0" cellpadding="1" cellspacing="1" class="b24-subscription" runat="server">
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label1" runat="server" Text="B24 SubID :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="SubIdLabel" runat="server"></asp:Label>
        </td>
        <td class="style1">
        </td>
        
        <td style="text-align:right;">
            <asp:Label ID="Label14" runat="server" Text="Type :"></asp:Label>
        </td>
        <td >
            <asp:Label ID="TypeLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label2" runat="server" Text="Company/Dept :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="CompanyDepartmentLabel" runat="server"></asp:Label>
        </td>
        <td class="style1">
        </td>
        <td style="text-align:right;">
            <asp:Label ID="Label16" runat="server" Text="Status :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="StatusLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label3" runat="server" Text="Application :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="ApplicationLabel" runat="server"></asp:Label>
        </td>
        
        <td class="style1">
        </td>
        <td style="text-align:right;">
            <asp:Label ID="Label18" runat="server" Text="Starts :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="StartsLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label6" runat="server" Text="Contract# :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="ContractLabel" runat="server"></asp:Label>
        </td>
        <td class="style1">
        </td>
        
        <td style="text-align:right;">
            <asp:Label ID="Label20" runat="server" Text="Expires :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="ExpiresLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;" class="auto-style1">
            <asp:Label ID="Label8" runat="server" Text="Seats :"></asp:Label>
        </td>
        <td class="auto-style1">
            <asp:Label ID="SeatsLabel" runat="server"></asp:Label>
        </td>
        <td class="auto-style2">
        </td>
        <td style="text-align:right;" class="auto-style1">
            <asp:Label ID="Label22" runat="server" Text="Chapters to Go :"></asp:Label>
        </td>
        <td class="auto-style1">
            <asp:Label ID="ChapterToGoLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label5" runat="server" Text="Registered Users :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="RegisteredUsersLabel" runat="server"></asp:Label>
        </td>
        
        <td class="style1">
        </td>
        <td style="text-align:right;">
            <asp:Label ID="Label4" runat="server" Text="Group Code :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="GroupCodeLabel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label10" runat="server" Text="SalesGroup :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="SalesGroupLabel" runat="server"></asp:Label>
        </td>
        <td class="style1"></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:Label ID="Label12" runat="server" Text="SalesPerson :"></asp:Label>
        </td>
        <td>
            <asp:Label ID="SalesPersonLabel" runat="server"></asp:Label>
        </td>
        <td class="style1"></td>
        <td></td>
        <td></td>
    </tr>
</table>