<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.Login" Title="Login" Codebehind="Login.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <b24:B24Login id="B24Login" runat="server" TitleText="" DisplayRememberMe="false"  
        onloggedin="B24Login_LoggedIn">
      <TextBoxStyle  CssClass="logintbx">
      </TextBoxStyle>
      <LabelStyle CssClass="loginlbl">
      </LabelStyle>
      <LoginButtonStyle CssClass="loginbtn">
      </LoginButtonStyle>
      <FailureTextStyle CssClass="loginerr"></FailureTextStyle>
  </b24:B24Login>
</asp:Content>

