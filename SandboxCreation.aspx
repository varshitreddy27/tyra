<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.SandboxCreation" Title="SandboxCreation" Codebehind="SandboxCreation.aspx.cs" %>
<%@ Register src="UserControls/InGeniousSandbox.ascx" tagname="IngenSandbox" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:IngenSandbox ID="IngenSandbox1" runat="server" />
</asp:Content>
