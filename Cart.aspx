<%@ Page Language="C#" MasterPageFile="~/BaseMaster.master" AutoEventWireup="true"
    Inherits="B24.Sales4.UI.Cart" Title="Cart" CodeBehind="Cart.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator."
        Visible="false" CssClass="b24-errortext"></asp:Label>
    <asp:Panel ID="CartPanel" runat="server">
        <span class="b24-doc-title2">User and Subscription Cart</span>
        <p class="b24-report-banner">
            Step 1: Select A Subscription</p>
        <% // radio button strategy detailed here: http://www.asp.net/learn/data-access/tutorial-51-vb.aspx %>
        <asp:GridView ID="SubCartGridView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="b24-report-title"
            DataKeyNames="SubscriptionID">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="SubRadioButtonMarkup" /></ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="SubscriptionID" DataNavigateUrlFormatString="subscription.asp?arg={{{0}}}"
                    HeaderText="Sub ID" DataTextField="PasswordRoot" />
                <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                <asp:BoundField DataField="Department" HeaderText="Department" />
                <asp:BoundField DataField="Seats" HeaderText="Seats" />
            </Columns>
        </asp:GridView>
        <p class="b24-report-banner">
            Step 2: Select Users</p>
        <% // checkbox strategy detailed here: http://www.asp.net/learn/data-access/tutorial-51-vb.aspx %>
        <asp:GridView ID="UserCartGridView" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="b24-report-title"
            DataKeyNames="UserID">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="UserCheckboxMarkup" /></ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="SubscriptionID" DataNavigateUrlFormatString="subscription.asp?arg={{{0}}}"
                    HeaderText="Sub ID" DataTextField="PasswordRoot" />
                <asp:HyperLinkField DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="user.asp?arg={{{0}}}"
                    HeaderText="Login" DataTextField="BaseLogin" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            </Columns>
        </asp:GridView>
        <p class="b24-report-banner">
            Step 3: Choose an Action</p>
        <asp:RadioButtonList ID="TaskList" runat="server">
            <asp:ListItem Text="Move selected users to selected sub" Value="Move" />
            <asp:ListItem Text="Empty selected users from cart" Value="EmptyUsers" />
            <asp:ListItem Text="Empty entire cart (users and subs)" Value="EmptyCart" />
        </asp:RadioButtonList>
        <p>
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" />
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
        </p>
    </asp:Panel>
</asp:Content>
