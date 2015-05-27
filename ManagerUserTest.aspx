<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.ManagerUserTest" Codebehind="ManagerUserTest.aspx.cs" %>

<%@ Register src="UserControls/UserDetail.ascx" tagname="UserDetail" tagprefix="uc1" %>
<%@ Register src="UserControls/ChangePassword.ascx" tagname="ChangePassword" tagprefix="uc2" %>
<%@ Register src="UserControls/Impersonate.ascx" tagname="Impersonate" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <br />
    <uc1:UserDetail ID="UserDetail1" runat="server" />
    <br />
    <uc2:ChangePassword ID="ChangePassword1" runat="server" />
    <br />
    <uc3:Impersonate ID="Impersonate1" runat="server" />
    <br />
</asp:Content>

