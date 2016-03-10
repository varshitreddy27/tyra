<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.LibraryPersistentUrl" Codebehind="LibraryPersistentUrl.ascx.cs" %>
<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="LibraryPersistentUrlErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="MultiView" runat="server">
            <asp:View ID="ReadOnlyView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="b24-helptext" Text="This URL, provided by the institution, will replace the site's default URL in Share, RSS and New Title emails enabling token/proxy users to login to the site. To add or change the URL click 'Change URL'">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="URLLabel" runat="server" Text="Current Persistent URL:"></asp:Label>
                            <asp:Label ID="CurrentURLLabel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="PersistentURLchangeButton" runat="server" Text="Change URL" OnClick="PersistentURLchangeButton_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="EditView" runat="server">
                <p class="b24-helptext">
                    To add or change the URL in the system enter the URL provided by the institution
                    and click update</p>
                <table cellspacing="2" cellpadding="0" border="0" class="b24-form">
                    <tr class="b24-formelement">
                        <td align="left" colspan="4" class="b24-forminputlabel" nowrap="1">
                            <div class="b24-report-frame">
                                <asp:Literal ID="ltMessage" runat="server" Visible="false"></asp:Literal></div>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td align="right" colspan="2" class="b24-forminputlabel" nowrap="1">
                            <span class="b24-formrequired"></span><span class="b24-formdescription">new persistent
                                URL:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewSender" runat="server" TextMode="SingleLine"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="revNewSender" runat="server" ControlToValidate="txtNewSender"
                                ErrorMessage="Please fill in the URL"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td align="center" colspan="4" class="b24-forminputlabel" nowrap="1">
                            <asp:Button ID="butUpdate" runat="server" Text="Update" OnClick="butUpdate_OnClick" />
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
