<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales4.UI.SandboxProperties" Title="SandboxProperties" Codebehind="SandboxProperties.aspx.cs" %>
<%@ Register src="UserControls/ChangeSandboxProperties.ascx" tagname="IngenSandbox" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:IngenSandbox ID="IngenSandbox1" runat="server" />
</asp:Content>