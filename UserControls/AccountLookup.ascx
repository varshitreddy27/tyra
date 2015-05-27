<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountLookup.ascx.cs"
    Inherits="B24.Sales3.UserControls.AccountLookup" %>

<asp:Panel ID="LookupPanel" runat="server" Width="450" DefaultButton="FindButton">
    <p class="b24-doc-title">
        Account Lookup</p>
    <p class="b24-doc-para">
        To locate an account, specify one or more of the criteria below. The results will
        reflect the closest match to all the parameters specified; the more you specify
        the narrower the result.</p>
    <table class="b24-form" border="0" cellspacing="2" cellpadding="0">
        <tr>
            <td align="right">
                <asp:Label ID="SubIDLabel" runat="server" Text="B24 SubID" CssClass="b24-forminputlabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="SubIDTextBox" runat="server" CssClass="b24-forminput" MaxLength="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="CompanyNameLabel" runat="server" Text="Company Name" CssClass="b24-forminputlabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="CompanyNameTextBox" runat="server" CssClass="b24-forminput" MaxLength="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="SalespersonFirstNameLabel" runat="server" Text="Salesperson First Name"
                    CssClass="b24-forminputlabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="SalespersonFirstNameTextBox" runat="server" CssClass="b24-forminput"
                    MaxLength="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="SalespersonLastNameLabel" runat="server" Text="Salesperson Last Name"
                    CssClass="b24-forminputlabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="SalespersonLastNameTextBox" runat="server" CssClass="b24-forminput"
                    MaxLength="80"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="ContractNumberLabel" runat="server" Text="Contract Number" CssClass="b24-forminputlabel"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="ContractNumberTextBox" runat="server" CssClass="b24-forminput" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
                <asp:CheckBox ID="ExactMatchCheckBox" runat="server" CssClass="b24-forminputlabel"
                    Text="Exact Match" />
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top" align="left">
                <asp:CheckBox ID="ActiveAccountsOnlyCheckBox" runat="server" CssClass="b24-forminputlabel"
                    Text="Active Accounts Only" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
                <asp:Button ID="FindButton" runat="server" Text="Find" CssClass="b24-forminput" OnClick="FindButton_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<br />
<asp:Panel ID="ResultCountPanel" runat="server" Visible="false">
    <asp:Label ID="AccountMatchesLabel" runat="server" Text="Account matches: "></asp:Label><asp:Label
        ID="ResultCountLabel" runat="server" Text=""></asp:Label>
    <br /><br />
    <asp:Label ID="NoResultFound" runat="server" Text="No Result Found" Visible="False"></asp:Label>
</asp:Panel>
<asp:Panel ID="ResultPanel" runat="server" Visible="false">
    <asp:GridView ID="ResultGridView" runat="server" AutoGenerateColumns="False"
        OnRowDataBound="ResultGridView_RowDataBound" CssClass="resultGrid">
        <Columns>
            <asp:BoundField HeaderText="B24 SubID" DataField="PasswordRoot" />
            <asp:TemplateField HeaderText="Company">
                <ItemTemplate>
                    <b>
                        <asp:Label ID="CompanyLabel" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label></b>
                    <br />
                    <asp:Label ID="DepartmentLabel" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Type" DataField="UserSubscriptionType" />
            <asp:BoundField HeaderText="Status" DataField="UserSubscriptionStatus" />
            <asp:BoundField HeaderText="Application" DataField="applicationname" />
            <asp:TemplateField HeaderText="Dates">
                <ItemTemplate>
                    <asp:Label ID="StartDateLabel" runat="server" CssClass="subscription-starts" Text='<%# string.Format("{0:MMM dd, yyyy}",Eval("starts")) %>'></asp:Label>
                    <br />
                    <asp:Label ID="EndDateLabel" runat="server" CssClass="subscription-expires" Text='<%# string.Format("{0:MMM dd, yyyy}",Eval("expires")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="SalesGroup" DataField="SalesGroup" />
            <asp:BoundField HeaderText="Seats" DataField="seats" />
            <asp:TemplateField HeaderText="Subscriptionid" Visible="false" ItemStyle-Wrap="false"
                ControlStyle-Width="100%">
                <ItemTemplate>
                    <asp:Label ID="SubscriptionidLabel" runat="server" Text='<%# Eval("Subscriptionid") %>'
                        Visible="false" />
                </ItemTemplate>
                <ItemStyle Wrap="False"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:HiddenField ID="SubscriptionIdHiddenField" runat="server" Value='<%#Eval("Subscriptionid") %>' />
                    <br />
                    <span style="margin:1px 2px 2px"><asp:HyperLink ID="AccountInfoHyperLink" CssClass="GoButton" runat="server">AccountInfo</asp:HyperLink></span>
                    <br />
                    <span style="margin:1px 2px 2px"><asp:HyperLink ID="ManageAccountHyperLink" CssClass="GoButton" runat="server">Manage Account</asp:HyperLink></span>
                    <br />
                    <span style="margin:1px 2px 2px"><asp:HyperLink ID="ReportsHyperLink" CssClass="GoButton" runat="server">Reports</asp:HyperLink></span>
                    <br />
                    <span style="margin:1px 2px 2px"><asp:CheckBox ID="CartCheckBox" runat="server" Text="Cart" CssClass="b24-cartlink-label"/></span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div>
        <br />
        <table width="100%">
            <tr>
                <td colspan="2">
                    <asp:Button ID="UpdateCartButton" runat="server" Text="Update Cart" OnClick="UpdateCartButton_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
