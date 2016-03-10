<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales4.UI.Login" Title="Login" Codebehind="Login.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="LoginPnl" runat="server">
        <asp:Label ID="LogInLbl" runat="server" Text="Log In" CssClass="b24-doc-title"></asp:Label>
        <b24:B24Login ID="B24Login" runat="server" TitleText="" DisplayRememberMe="false"
            OnLoggedIn="B24Login_LoggedIn">
            <TextBoxStyle CssClass="logintbx"></TextBoxStyle>
            <LabelStyle CssClass="loginlbl"></LabelStyle>
            <LoginButtonStyle CssClass="loginbtn"></LoginButtonStyle>
            <FailureTextStyle CssClass="loginerr"></FailureTextStyle>
        </b24:B24Login>
    </asp:Panel>
     <asp:Panel ID="ChangePasswordPnl" Visible="false" runat="server" >

     </asp:Panel>
</asp:Content>

