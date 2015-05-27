<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.SelfRegistration" Codebehind="SelfRegistration.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
        <tr>
            <td>
                <asp:Label ID="SelfRegistrationErrorMessageLabel" runat="server" CssClass="b24-errortext"></asp:Label>
            </td>
        </tr>
        </table>
        <asp:MultiView ID="Multiview" runat="server">
            <asp:View ID="EditView" runat="server">
                <table>
                    <tr>
                        <asp:Label ID="EnableSelfRegistrationTitleLabel" runat="server" Text="Enable Self Reg"
                            CssClass="b24-doc-subtitle" Visible="false" />
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="EnableSelfRegistrationLabel" runat="server" Text="Check this box to enable self registration for users in this subscription."
                                CssClass="b24-helptext" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="EnableSelfRegistrationCheckBox" runat="server" Text=" Enable Self Registration"
                                CssClass="b24-formdescription" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LinkURLTitleLabel" runat="server" Text="Registration Url: " CssClass="b24-formdescription"
                                Visible="false" />
                            <asp:Label ID="LinkUrlLabel" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AddDomainTitleLabel" runat="server" Visible="false" CssClass="b24-doc-subtitle"
                                Text="Add an Email Domain" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AddDomainLabel" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Allow users from the specified domain to register into this account. Enter the at-sign followed by the full domain (e.g., @skillsoft.com)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="NewDomainLabel" runat="server" Visible="false" Text="New Domain: "
                                CssClass="b24-formdescription" />
                            <asp:TextBox ID="NewDomainRuleTextBox" runat="server" Visible="false" />
                            <asp:Label ID="HelpTextLabel" runat="server" Visible="false" Text=" (e.g., @skillsoft.com)"
                                CssClass="b24-helptext" />
                            <asp:RegularExpressionValidator ID="emailFormatValidator" runat="server" Display="Dynamic"
                                ControlToValidate="NewDomainRuleTextBox" ErrorMessage="Error: domains must be in the form @domain.com"
                                ValidationExpression="^@[\w.']*\.\w{2,3}$" ValidationGroup="SelfRegistration">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomainTitleLabel" runat="server" Visible="false" CssClass="b24-doc-subtitle"
                                Text="Remove an Existing Email Domain" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomainsLabel" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Select domain(s) to remove." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="DomainRuleIdCheckBoxList" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomains2Label" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Submit a PSG Request to update this subscription's Self Registration" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="UpdateButton" OnClick="UpdateButton_Click" runat="server" ValidationGroup="SelfRegistration"
                                Text="Update" />
                            <asp:Button ID="EditCancelButton" OnClick="EditCancelButton_Click" runat="server"
                                Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ReadView" runat="server">
                <table>
                    <tr>
                        <asp:Label ID="EnableSelfRegistrationReadTitleLabel" runat="server" Text="Enable Self Reg"
                            CssClass="b24-doc-subtitle" Visible="false" />
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="EnableSelfRegistrationReadLabel" runat="server" Text="Check this box to enable self registration for users in this subscription."
                                CssClass="b24-helptext" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="EnableSelfRegistrationReadCheckBox" runat="server" Text=" Enable Self Registration"
                                CssClass="b24-formdescription" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LinkURLTitleReadLabel" runat="server" Text="Registration Url: " CssClass="b24-formdescription"
                                Visible="false" />
                            <asp:Label ID="LinkURLReadLabel" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AddDomainTitleReadLabel" runat="server" Visible="false" CssClass="b24-doc-subtitle"
                                Text="Add an Email Domain" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AddDomainReadLabel" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Allow users from the specified domain to register into this account. Enter the at-sign followed by the full domain (e.g., @skillsoft.com)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="NewDomainReadLabel" runat="server" Visible="false" Text="New Domain: "
                                CssClass="b24-formdescription" />
                            <asp:TextBox ID="NewDomainReadTextBox" runat="server" Visible="false" Enabled="false" />
                            <asp:Label ID="HelpTextReadLabel" runat="server" Visible="false" Text=" (e.g., @skillsoft.com)"
                                CssClass="b24-helptext" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomainTitleReadLabel" runat="server" Visible="false" CssClass="b24-doc-subtitle"
                                Text="Remove an Existing Email Domain" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomainReadLabel" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Select domain(s) to remove." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="DomainRuleIdReadCheckBoxList" runat="server" Visible="false"
                                Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RemoveDomains2ReadLabel" runat="server" Visible="false" CssClass="b24-helptext"
                                Text="Submit a PSG Request to update this subscription's Self Registration" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="EditButton" OnClick="EditButton_Click" runat="server" Text="Edit" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
