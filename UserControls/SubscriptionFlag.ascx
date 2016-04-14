<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.SubscriptionFlag" Codebehind="SubscriptionFlag.ascx.cs" %>
<link href="../App_Themes/Classic/sales4.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Subscription Flag</p>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="SubscriptionFlagErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:MultiView ID="Multiview" runat="server">
            <asp:View ID="EditView" runat="server">
                <table border="0" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="SubscriprionFlagCheckBoxList" runat="server" OnDataBound="SubFlagCheckBoxList_DataBound"
                                RepeatColumns="2" RepeatLayout="Table">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px">
                        </td>
                    </tr>

                   <%-- <table style="border: solid 1px black" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="SpecialSubscriptionLabel" runat="server" CssClass="b24-doc-subtitle" Visible="true"
                                    Text="Special Subscription Settings" />
                            </td>
                        </tr>
                        <tr>
                            <td class="b24-report-title">
                                <asp:Label ID="SettingsReadLabel" runat="server" Text="Settings" Visible="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="NoSettingsReadLabel" runat="server" Visible="false" Text="No Settings for this Sub"
                                    CssClass="b24-errortext"></asp:Label>
                                <asp:ListView ID="SpecialSubscriptionReadViewListView" runat="server">
                                    <LayoutTemplate>
                                        <table cellspacing="0" cellpadding="2" border="0">
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr1" runat="server">
                                            <td id="Td1" runat="server">
                                                <asp:Label ID="SpecialSubscriptionLabel" runat="server" Text='<%#Eval("description") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>--%>
                    <tr>
                        <td align="center">
                            <asp:Button ID="UpdateButton" OnClick="UpdateButton_Click" Width="75" runat="server"
                                Text="Update" />
                            <asp:Button ID="EditCancelButton" OnClick="EditCancelButton_Click" runat="server"
                                Text="Cancel" Width="75" TabIndex="12" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ReadOnlyView" runat="server">
                <table border="0" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Label ID="NosubscriptionFlagLabel" runat="server" Visible="false" Text="No Subscription flags for this Sub"
                                CssClass="b24-errortext"></asp:Label>
                            <asp:ListView ID="SubscriptionFlagListView" runat="server">
                                <LayoutTemplate>
                                    <table cellspacing="0" cellpadding="2" border="0">
                                        <tr id="ItemPlaceHolder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr1" runat="server">
                                        <td id="Td1" runat="server">
                                            <asp:Label ID="SubscriptionLabel" runat="server" Text='<%#Eval("description") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="EditButton" OnClick="EditButton_Click" Width="75" runat="server"
                                Text="Edit" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
