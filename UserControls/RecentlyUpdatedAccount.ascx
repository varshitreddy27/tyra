<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentlyUpdatedAccount.ascx.cs"
    Inherits="B24.Sales4.UserControls.RecentlyUpdatedAccount" %>
<table>
    <tr>
        <td align="left">
            <asp:Label ID="WelcomeLabel" runat="server" CssClass="b24-doc-subtitle"> 
            </asp:Label>
            <p class="b24-report-banner">
                My 10 most recently updated subscriptions
            </p>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="RecentSubcriptionGridview" runat="server" AutoGenerateColumns="False"
                CssClass="resultGrid" CellPadding="4" OnRowDataBound="RecentSubcriptionGridview_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Last Updated " ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="LastUpdateLabel" runat="server" Text='<%# string.Format("{0:MMM dd,yyyy}",Eval("LastUpdated")) %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="B24 SubID" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="B24SubIDLable" runat="server" Text='<%# Eval("B24SubID") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="CompanyLabel" runat="server" Text='<%# Eval("Company") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="TypeLabel" runat="server" Text='<%# Eval("Type") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Application" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="ApllicationLabel" runat="server" Text='<%# Eval("Application") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sales Group" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="SalesGroupLabel" runat="server" Text='<%# Eval("SalesGroup") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seats" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="SeatsLabel" runat="server" Text='<%# Eval("Seats") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="@Subscriptionid" Visible="false" ItemStyle-Wrap="false"
                        ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="SubscriptionidLabel" runat="server" Text='<%# Eval("Seats") %>' Visible="false" />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                        <ItemTemplate>
                            <asp:HiddenField ID="SubscriptionIdHiddenField" runat="server" Value='<%#Eval("Subscriptionid") %>' />
                            <asp:HyperLink ID="AccountInfoHyperLink" runat="server" Text="Account Info" CssClass="GoButton"
                                Style="display: inline">
                            </asp:HyperLink>
                            <br />
                            <asp:HyperLink ID="ManageAccount" runat="server" Text="Manage Account" CssClass="GoButton"
                                Style="display: inline">
                            </asp:HyperLink>
                            <br />
                            <asp:HyperLink ID="Reports" runat="server" Text="Reports " CssClass="GoButton" Style="display: inline">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
        </td>
    </tr>
    <tr>
        <td align="left" class="b24-report-banner">
            <p>
                <asp:HyperLink ID="ManageAccountLookupHyperLink" runat="server" Text="Find an existing account">
                </asp:HyperLink>&nbsp;within the system.</p>
            <p>
                <asp:HyperLink ID="ManageUserLookupHyperLink" runat="server" Text="Find an existing user">
                </asp:HyperLink>&nbsp;within the system.</p>
        </td>
    </tr>
</table>
