<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLookup.ascx.cs"
    Inherits="B24.Sales4.UserControl.UserLookup" %>
<asp:Panel ID="LookupPanel" runat="server" Width="450" DefaultButton="FindButton">
    <p class="b24-doc-title">
        User Lookup</p>
    <p class="b24-doc-para">
        To locate a user, specify one or more of the criteria below. The results will reflect
        the closest match to all the parameters specified; the more you specify the narrower
        the result.</p>
    <table class="b24-form" border="0" cellspacing="2" cellpadding="0">
        <tbody>
            <tr>
                <td align="right">
                    <asp:Label ID="LogOnLabel" runat="server" Text="Login" CssClass="b24-forminputlabel"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="LogOnTextBox" runat="server" CssClass="b24-forminput" CausesValidation="True"
                        MaxLength="64"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="EmailLabel" runat="server" Text="Email" CssClass="b24-forminputlabel"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="EmailTextBox" runat="server" CssClass="b24-forminput"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email Id"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="EmailTextBox"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="FirstNameLabel" runat="server" Text="First Name" CssClass="b24-forminputlabel"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="b24-forminput" CausesValidation="True"
                        MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="LastNameLabel" runat="server" Text="Last Name" CssClass="b24-forminputlabel"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="b24-forminput" MaxLength="80"></asp:TextBox>
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
            <tr class="b24-formelement">
                <td colspan="2">
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                </td>
                <td align="left">
                    <asp:Button ID="FindButton" runat="server" Text="Find" CssClass="b24-forminput" OnClick="FindButton_Click" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<br />
<asp:Panel ID="UserMatchesPanel" runat="server" Visible="false">
    <div>
        <asp:Label ID="UserMatchesLabel" runat="server" Text="User matches: "></asp:Label><asp:Label
            ID="ResultCountLabel" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <asp:Label ID="NoResultsLabel" runat="server" Text="No Results Found"></asp:Label>
    </div>
</asp:Panel>
<br />
<asp:Panel ID="ResultPanel" runat="server" Visible="false">
    <div>
        <asp:GridView ID="ResultGridView" runat="server" AutoGenerateColumns="False" CssClass="resultGrid"
            OnRowDataBound="ResultGridView_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="User">
                    <ItemTemplate>
                        <b>
                            <asp:Label ID="FirstNameLabel" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                            <asp:Label ID="LastNameLabel" runat="server" Text='<%# Eval("LastName") %>'></asp:Label></b>
                        <asp:Label ID="SubscriptionStatusIDLabel" runat="server" Text="(Soft deleted)" Visible="false"
                            CssClass="b24-errortext"></asp:Label>
                        <br />
                        <asp:Label ID="UserEmailLabel" runat="server" Text='<%# Eval("Email") %>' CssClass="user-email">
                        </asp:Label>
                        <br />
                        <span class="user-login">(<asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("BaseLogin") %>'></asp:Label>)</span>
                        <asp:HiddenField ID="SubscriptionStatusIDHidden" runat="server" Value='<%# Eval("SubscriptionStatusID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="B24 SubID" DataField="PasswordRoot" ControlStyle-CssClass />
                <asp:TemplateField HeaderText="Company">
                    <ItemTemplate>
                        <asp:Label ID="CompanyLabel" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                        <br />
                        <asp:Label ID="DepartmentLabel" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Type" DataField="Type" />
                <asp:BoundField HeaderText="Application" DataField="ApplicationName" />
                <asp:TemplateField HeaderText="Dates">
                    <ItemTemplate>
                        <asp:Label ID="StartDateLabel" runat="server" CssClass="subscription-starts" Text='<%#string.Format("{0:MMM dd, yyyy}",Eval("Starts")) %>'></asp:Label>
                        <br />
                        <asp:Label ID="EndDateLabel" runat="server" CssClass="subscription-expires" Text='<%# string.Format("{0:MMM dd, yyyy}",Eval("Expires")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Last Login" DataField="LastLogin" DataFormatString="{0:MMM dd,yyyy hh:mm tt}" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:HiddenField ID="UserIDHiddenField" runat="server" Value='<%#Eval("userID") %>' />
                        <asp:HyperLink ID="UserInfoHyperLink" runat="server" CssClass="GoButton">User Info</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="ManageUserHyperLink" runat="server" CssClass="GoButton">Manage User</asp:HyperLink>
                        <br />
                        <asp:CheckBox ID="CartCheckBox" runat="server" Text="Cart" CssClass="b24-cartlink-label" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="b24-report-title" />
        </asp:GridView>
    </div>
    <div>
        <asp:Button ID="UpdateCartButton" runat="server" Text="Update Cart" OnClick="UpdateCartButton_Click" />
    </div>
</asp:Panel>