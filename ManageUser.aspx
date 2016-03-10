<%@ Page Language="C#" MasterPageFile="~/BaseMaster.master" AutoEventWireup="true"
    Inherits="B24.Sales4.UI.ManageUser" Title="Manage User" CodeBehind="ManageUser.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UserDetails.ascx" TagName="UserDetail" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ChangePassword.ascx" TagName="ChangePassword" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Impersonate.ascx" TagName="Impersonate" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/EmailSettings.ascx" TagName="EmailSettings" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UserAdministrator.ascx" TagName="UserAdministrator"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UserReportAccess.ascx" TagName="UserReportAccess"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AdvancedUserPermissions.ascx" TagName="AdvancedUserPermission"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/RemoveRestoreUser.ascx" TagName="RemoveRestoreUser"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/EmailPassword.ascx" TagName="EmailPassword" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UserLookup.ascx" TagName="UserSearch" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UserRole.ascx" TagName="UserRole" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        // On panel click clear the error label text
        function onPanelClick(messageLabel) {
            //alert(messageLabel);
            var label = document.getElementById(messageLabel);
            if (label != null) {
                label.innerText = "";
            }
        }
        function clearAdminPermissionsErrorLabel() {
            var label = document.getElementById("<%=UserAdministratorControl.ClientID %>" + "_AdminPermissionsErrorLabel");
            if (label != null) {
                label.innerText = "";
            }
            label = null;
            label = document.getElementById("<%=AdvancedUserPermissionControl.ClientID %>" + "_AdvancedUserPermissionErrorLabel");
            if (label != null) {
                label.innerText = "";
            }
            label = null;
            label = document.getElementById("<%=UserReportAccessControl.ClientID %>" + "_UserReportAccessErrorLabel");
            if (label != null) {
                label.innerText = "";
            }
        }
    </script>
    <asp:UpdatePanel ID="UserInfoDetailsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="UserInfoDetailsTable" runat="server" class="b24-subscription">
                <tr>
                    <td style="width: 30px"></td>
                    <td>
                        <uc1:UserDetail ID="UserInfoDetails" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="progress">
                <img src="images/loading.gif" alt="Updating" style="color: Red" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator." Visible="false" CssClass="b24-errortext"></asp:Label>
    <asp:Label ID="PageErrorLabel" runat="server" Visible="false" Text="Invalid User Id Passed." CssClass="b24-errortext"></asp:Label>
    <asp:MultiView runat="server" ID="ManageUserMultiView">
        <asp:View runat="server" ID="LookupView">
            <asp:UpdatePanel ID="LookupUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UserSearch ID="UserSearchControl" runat="server"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="InfoView">
            <asp:UpdatePanel ID="UserDetailUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UserDetail ID="UserDetail" runat="server" ShowHeaderText="false" Visible = "false"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="PasswordView">
            <cc1:Accordion ID="PasswordAccordion" runat="server" FadeTransitions="True" SelectedIndex="0"
                HeaderSelectedCssClass="accordionHeaderSelected" RequireOpenedPane="false" TransitionDuration="300"
                HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                <Panes>
                    <cc1:AccordionPane ID="ChangePasswordAccordionPane" runat="server" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ChangePassword.ClientID %>_ChangePasswordError')">
                                <div>
                                    Change User Password</div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="ChangePasswordUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:ChangePassword ID="ChangePassword" runat="server" ShowHeaderText="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane ID="EmailPasswordAccordionPane" runat="server" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=EmailPasswordControl.ClientID %>_EmailPasswordErrorLabel')">
                                <div>
                                    eMail User Password</div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="EmailPasswordUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:EmailPassword ID="EmailPasswordControl" runat="server" ShowHeaderText="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                </Panes>
            </cc1:Accordion>
        </asp:View>
        <asp:View runat="server" ID="AdminRoleView"> 
            <asp:UpdatePanel ID="AdminPermissionsUpdatePanel" runat="server" UpdateMode="Conditional" Visible = "false">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="UserRole" runat="server" Text="User Role" CssClass="b24-doc-subtitle"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:UserRole ID="UserRoleControl" runat="server" ShowHeaderText="false" />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="UserAdministratorLabel" runat="server" Text="User Administrator" CssClass="b24-doc-subtitle"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:UserAdministrator ID="UserAdministratorControl" runat="server" ShowHeaderText="false" />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="AdvanceUserPermissionLabel" runat="server" CssClass="b24-doc-subtitle" Text="Advanced User Permissions"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:AdvancedUserPermission ID="AdvancedUserPermissionControl" runat="server" showHeaderText="false" />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="UserReportAccessFormLabel" runat="server" Text="Assign User Report Access"
                                    CssClass="b24-doc-subtitle"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:UserReportAccess ID="UserReportAccessControl" runat="server" showHeaderText="false" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="SettingsView">
            <cc1:Accordion ID="SettingsAccordion" runat="server" FadeTransitions="True" SelectedIndex="0"
                HeaderSelectedCssClass="accordionHeaderSelected" RequireOpenedPane="false" TransitionDuration="300"
                HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                <Panes>
                    <cc1:AccordionPane ID="EmailSettingsAccordionPane" runat="server" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=EmailSettingsControl.ClientID %>_EmailSettingsErrorLabel')">
                                <div>
                                    Email Settings
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:EmailSettings ID="EmailSettingsControl" runat="server" ShowHeaderText="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane ID="RemoveRestoreUserAccordionPane" runat="server" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=RemoveRestoreUserControl.ClientID %>_RemoveRestoreUserErrorLabel')">
                                <div>
                                    Disable/Restore User
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="RemoveRestoreUserUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:RemoveRestoreUser ID="RemoveRestoreUserControl" runat="server" ShowHeaderText="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane ID="ImpersonateAccordionPane" runat="server" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ImpersonateControl.ClientID %>_ImpersonateError')">
                                <div>
                                    Impersonate
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="ImpersonateUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc1:Impersonate ID="ImpersonateControl" runat="server" ShowHeaderText="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                </Panes>
            </cc1:Accordion>
        </asp:View>
    </asp:MultiView>
</asp:Content>
