<%@ Page Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UI.ManageSubTest"
    MasterPageFile="~/MasterPage.master" Title="Subscription Info" ValidateRequest="false"
    EnableEventValidation="false" Codebehind="ManageSubtest.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="infoDetails" TagPrefix="uc1" Src="~/UserControls/InfoDetails.ascx" %>
<%@ Register TagName="addUsers" TagPrefix="uc1" Src="~/UserControls/AddUsers.ascx" %>
<%@ Register TagName="subscriptionFlags" TagPrefix="uc1" Src="~/UserControls/SubscriptionFlag.ascx" %>
<%@ Register TagName="subscriptionNotes" TagPrefix="uc1" Src="~/UserControls/SubscriptionNotes.ascx" %>
<%@ Register TagName="extendTrail" TagPrefix="uc1" Src="~/UserControls/ExtendTrail.ascx" %>
<%@ Register TagName="selfRegistration" TagPrefix="uc1" Src="~/UserControls/SelfRegistration.ascx" %>
<%@ Register TagName="selfRegistrationInstruction" TagPrefix="uc1" Src="~/UserControls/SelfRegistrationInstructions.ascx" %>
<%@ Register TagName="updateAccount" TagPrefix="uc1" Src="~/UserControls/UpdateAccount.ascx" %>
<%@ Register TagName="MsgSender" TagPrefix="uc1" Src="~/UserControls/WelcomeMsgSender.ascx" %>
<%@ Register TagName="LibUrl" TagPrefix="uc1" Src="~/UserControls/LibraryPersistentUrl.ascx" %>
<%@ Register TagName="LibReport" TagPrefix="uc1" Src="~/UserControls/LibraryReportAccess.ascx" %>
<%@ Register TagName="LibRules" TagPrefix="uc1" Src="~/UserControls/LibraryRules.ascx" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="ManageAccountContent"
    runat="server">
    <% 
        var subId = InfoDetailsControl.Subscription.SubscriptionID;

    %>

    <script type="text/javascript">
        // On panel click clear the error label text
        function onPanelClick(messageLabel) {
            //alert(messageLabel);
            var label = document.getElementById(messageLabel);
            if (label != null) {
                label.innerText = "";
            }
        };
    </script>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="progress">
                <img src="images/loading.gif" alt="Updating" style="color: Red" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <cc1:Accordion ID="Accordion1" runat="server" ContentCssClass="accordionContent"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
        SelectedIndex="0" TransitionDuration="300" FadeTransitions="true">
        <Panes>
            <cc1:AccordionPane runat="server" ID="AccountDetailsAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=InfoDetailsControl.ClientID %>_InfoDetailsErrorLabel')">
                        <div>
                            Account Info</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="InfoDetailsUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:infoDetails ID="InfoDetailsControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="AddUsersAccordionPane" Visible="false">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=AddUsersControl.ClientID %>_AddUserErrorLabel')">
                        <div>
                            Add Users</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:addUsers ID="AddUsersControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="ExtendTrialAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ExtendTrailControl.ClientID %>_ExtendTrailErrorLabel')">
                        <div>
                            Reactivate or Extend this Trail</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="extendTrailUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:extendTrail ID="ExtendTrailControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="SubscriptionFlagAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SubscriptionFlagsControl.ClientID %>_SubscriptionFlagErrorLabel')">
                        <div>
                            Subscription Flag</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="SubscriptionFlagUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:subscriptionFlags ID="SubscriptionFlagsControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="SelfRegistrationsInstructionsAP">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SelfRegistrationUserControl.ClientID %>_Errorlabel')">
                        <div>
                            Self Registration Instruction</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="SelfRegPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:selfRegistrationInstruction ID="SelfRegistrationUserControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="SelfRegistrationAccordionPane" Visible="false">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SelfRegistrationControl.ClientID %>_SelfRegistrationErrorMessageLabel')">
                        <div>
                            Self Registration</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="SelfRegistrationUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:selfRegistration ID="SelfRegistrationControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="AuthenticationRulesAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=LibraryRules.ClientID %>_LibraryRulesErrorLabel')">
                        <div>
                            Authentication Rules</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="LibraryRulesUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <uc1:LibRules ID="LibraryRules" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="LibraryReportAccessAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=LibReportUsers.ClientID %>_LibraryReportAccessErrorLabel')">
                        <div>
                            Library Report Users</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="LibraryUsersUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <uc1:LibReport ID="LibReportUsers" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="PersistentURLAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=LibraryUrl.ClientID %>_LibraryPersistentUrlErrorLabel')">
                        <div>
                            Persistent URL - Share Link</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="PersitentURLUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <uc1:LibUrl ID="LibraryUrl" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="WelcomeMessageSenderAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=WelcomeMsgSenderControl.ClientID %>_WelcomeMessageErrorLabel')">
                        <div>
                            Welcome Message Sender</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="WelcomeMessageUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <uc1:MsgSender ID="WelcomeMsgSenderControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="SubscriptionNotesAccordionPane">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SubscriptionNotesControl.ClientID %>_SubscriptionNotesErrorLabel')">
                        <div>
                            Add Subscription Notes</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="SubscriptionNotesUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:subscriptionNotes ID="SubscriptionNotesControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
            <cc1:AccordionPane runat="server" ID="UpdateAccountAccordionPane1">
                <Header>
                    <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=UpdateAccountControl.ClientID %>_AccountUpdateErrorLabel')">
                        <div>
                            Update This Account</div>
                    </a>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="UpdateAccountUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 30px">
                                    </td>
                                    <td>
                                        <uc1:updateAccount ID="UpdateAccountControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </cc1:AccordionPane>
        </Panes>
    </cc1:Accordion>
</asp:Content>
