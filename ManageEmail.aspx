<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales4.UI.ManageEmail" Title="ManageEmail" Codebehind="ManageEmail.aspx.cs" %>

<%@ Register src="UserControls/WelcomeMsgSender.ascx" tagname="WelcomeMsgSender" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <uc1:WelcomeMsgSender ID="WelcomeMsgSender1" runat="server" />
    
</asp:Content>

