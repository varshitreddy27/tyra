<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.ShowSubCustomizations"
  Title="Subscription Customizations" Codebehind="ShowSubCustomizations.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <span class="b24-doc-title2">Subscription Customizations</span>
  <p class="b24-doc-para">Enter a subscription id to get a report of run-time customizations for this subscription:</p>
  Sub ID: <asp:TextBox ID="SubIDTextBox" runat="server"></asp:TextBox>
  <asp:Button ID="SubmitButton" runat="server" Text="Go!" />
  
  <asp:Panel ID="ResultsPanel" runat="server">
    <p class="b24-report-banner">Customizations:</p>
    <asp:Literal runat="server" ID="CustomizationsLiteral" />
  </asp:Panel>
</asp:Content>
