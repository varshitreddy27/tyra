<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.LibraryPersistentUrl" Title="LibraryPersistentUrl" Codebehind="LibraryPersistentUrl.aspx.cs" %>

<%@ Register src="UserControls/LibraryPersistentUrl.ascx" tagname="LibUrl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <uc1:LibUrl ID="LibUrl1" runat="server" />
    
</asp:Content>
