<%@ Page Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UI.ManageAccount"
    MasterPageFile="~/BaseMaster.Master" Title="Manage Account" ValidateRequest="false"
    CodeBehind="ManageAccount.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="InfoDetails" TagPrefix="Sales3" Src="~/UserControls/InfoDetails.ascx" %>
<%@ Register TagName="Lookup" TagPrefix="Sales3" Src="~/UserControls/AccountLookup.ascx" %>
<%@ Register TagName="SSO" TagPrefix="Sales3" Src="~/UserControls/SingleSignOnsub.ascx" %>
<%@ Register TagName="BrandLogo" TagPrefix="Sales3" Src="~/UserControls/PartnerLogo.ascx" %>
<%@ Register TagName="SubFlag" TagPrefix="Sales3" Src="~/UserControls/SubscriptionFlag.ascx" %>
<%@ Register TagName="ManageAcc" TagPrefix="Sales3" Src="~/UserControls/ManageAccount.ascx" %>
<%@ Register TagName="SelfRegInstruction" TagPrefix="Sales3" Src="~/UserControls/SelfRegistrationInstructions.ascx" %>
<%@ Register TagName="Collection" TagPrefix="Sales3" Src="~/UserControls/SubscriptionCollection.ascx" %>
<%@ Register TagName="CostCenter" TagPrefix="Sales3" Src="~/UserControls/CostCenter.ascx" %>
<%@ Register TagName="ExtendAttrib" TagPrefix="Sales3" Src="~/UserControls/ExtendAttributes.ascx" %>
<%@ Register TagName="IGESandbox" TagPrefix="Sales3" Src="~/UserControls/InGeniousSandbox.ascx" %>
<%@ Register TagName="AddUser" TagPrefix="Sales3" Src="~/UserControls/AddUsers.ascx" %>
<%@ Register TagName="MsgSender" TagPrefix="Sales3" Src="~/UserControls/WelcomeMsgSender.ascx" %>
<%@ Register TagName="SubNotes" TagPrefix="Sales3" Src="~/UserControls/SubscriptionNotes.ascx" %>
<%@ Register TagName="SelfRegistration" TagPrefix="Sales3" Src="~/UserControls/SelfRegistration.ascx" %>
<%@ Register TagName="Trail" TagPrefix="Sales3" Src="~/UserControls/ExtendTrail.ascx" %>
<%@ Register TagName="UpdateAcc" TagPrefix="Sales3" Src="~/UserControls/UpdateAccount.ascx" %>
<%@ Register TagName="AdvanceReportControl" TagPrefix="Sales3" Src="~/UserControls/AdvanceReport.ascx" %>
<%@ Register TagName="AlertControl" TagPrefix="Sales3" Src="~/UserControls/Alerts.ascx" %>

<asp:Content ContentPlaceHolderID="Content" ID="ManageAccountContent" runat="server">

    <script type="text/javascript">
        // On panel click clear the error label text
        function onPanelClick(messageLabel) {
           // if (messageLabel.indexOf("Alerts") > -1) {
           //     addDatePicker();
          //  }
            var label = document.getElementById(messageLabel);
            if (label != null) { 
                label.innerText = "";
            }
        }
    </script>
    <asp:UpdatePanel ID="InfoDetailsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Sales3:InfoDetails ID="SubscriptionInfoDetails" runat="server" />
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
    <asp:Label ID="PageErrorLabel" runat="server" Visible="false" Text="Invalid Subscription Id Passed."
        CssClass="b24-errortext"></asp:Label>
    <asp:MultiView runat="server" ID="ManageAccountMainView" ActiveViewIndex="0">
        <asp:View runat="server" ID="LookupView">
            <asp:UpdatePanel ID="LookupUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <Sales3:Lookup ID="AccountLookupControl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="InfoView">
            <asp:UpdatePanel ID="ManageAccountUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <Sales3:ManageAcc ID="ManageAccountControl" runat="server" Visible="false" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="CollectionView">
            <asp:UpdatePanel ID="CollectionsPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="width: 30px"></td>
                            <td>
                                <Sales3:Collection ID="CollectionUserControl" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="ImplementationView">
            <cc1:Accordion ID="ImplementationAccordion" runat="server" ContentCssClass="accordionContent"
                HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                SelectedIndex="0" TransitionDuration="300" FadeTransitions="true" RequireOpenedPane="false">
                <Panes>
                    <cc1:AccordionPane runat="server" ID="AccountDetailsAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ManageAccountControl.ClientID %>_ManageAccountErrorLabel')">
                                <div>
                                    Manage Account
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="ManageAccountEditUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:ManageAcc ID="ManageAccountEditControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="SingleSignOnAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SingleSignonUserControl.ClientID %>_SingleSignOnErrorLabel')">
                                <div>
                                    Single sign On
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="SingleSignOnUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:SSO ID="SingleSignonUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="PartnerLogoAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=BrandLogoUserControl.ClientID %>_PartnerLogoErrorLabel')">
                                <div>
                                    Logo
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <%--<asp:UpdatePanel ID="PartnerLogoUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                            <table>
                                <tr>
                                    <td style="width: 30px"></td>
                                    <td>
                                        <Sales3:BrandLogo ID="BrandLogoUserControl" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="SubscriptionFlagAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SubscriptionFlagUserControl.ClientID %>_SubscriptionFlagErrorLabel')">
                                <div>
                                    Subscription Flags
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="SubscriptionFlagsUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:SubFlag ID="SubscriptionFlagUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="SelfRegistrationsInstructionsAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SelfRegistrationInstructionUserControl.ClientID %>_Errorlabel')">
                                <div>
                                    Self Registration Instructions
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="SelfRegPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:SelfRegInstruction ID="SelfRegistrationInstructionUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="CostCentersAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=CostCenterUserControl.ClientID %>_CostCenterErrorLabel')">
                                <div>
                                    Cost Center
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="CostCenterPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:CostCenter ID="CostCenterUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="ExtendAttributesAP" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ExtendAttributesUserControl.ClientID %>_ExtendAttributeErrorLabel')">
                                <div>
                                    Extend Attributes
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="ExtendAttributesPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:ExtendAttrib ID="ExtendAttributesUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="IngeniousSandboxAP" Visible="false">
                        <Header>
                            Ingenius Sandbox
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="IngeniousSandboxUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:IGESandbox ID="IngeniousSandboxUserControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                </Panes>
            </cc1:Accordion>
        </asp:View>
        <asp:View runat="server" ID="AddUsersView">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="width: 30px"></td>
                            <td>
                                <Sales3:AddUser ID="AddUsersControl" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="SettingsView">
            <cc1:Accordion ID="SettingsAccordion" runat="server" ContentCssClass="accordionContent"
                HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                SelectedIndex="0" TransitionDuration="300" FadeTransitions="true">
                <Panes>
                    <cc1:AccordionPane runat="server" ID="WelcomeMessageSenderAccordionPane" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=WelcomeMsgSenderControl.ClientID %>_WelcomeMessageErrorLabel')">
                                <div>
                                    Welcome Message Sender
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="WelcomeMessageUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <Sales3:MsgSender ID="WelcomeMsgSenderControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="SubscriptionNotesAccordionPane" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=SubscriptionNotesControl.ClientID %>_SubscriptionNotesErrorLabel')">
                                <div>
                                    Add Subscription Notes
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="SubscriptionNotesUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:SubNotes ID="SubscriptionNotesControl" runat="server" />
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
                                    Self Registration
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="SelfRegistrationUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:SelfRegistration ID="SelfRegistrationControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="ExtendTrialAccordionPane" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=ExtendTrailControl.ClientID %>_ExtendTrailErrorLabel')">
                                <div>
                                    Reactivate or Extend this Trail
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="extendTrailUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:Trail ID="ExtendTrailControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                    <cc1:AccordionPane runat="server" ID="AlertsAccordionPanel" Visible="false">
                        <Header>
                            <a href="#" class="accordionLink" onclick="javascript:onPanelClick('<%=AlertsControl.ClientID %>_AlertsErrorLabel')">
                                <div>
                                    Alerts
                                </div>
                            </a>
                        </Header>
                        <Content>
                            <asp:UpdatePanel ID="AlertsUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 30px"></td>
                                            <td>
                                                <Sales3:AlertControl ID="AlertsControl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Content>
                    </cc1:AccordionPane>
                </Panes>
            </cc1:Accordion>
        </asp:View>
        <asp:View runat="server" ID="ReportsView">
            <asp:UpdatePanel ID="AdvanceReportUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <Sales3:AdvanceReportControl ID="AdvanceReportUserControl" runat="server" ShowHeaderText="True" Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View runat="server" ID="PurchaseOrRenewView">
            <asp:UpdatePanel ID="UpdateAccountUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="width: 30px"></td>
                            <td>
                                <Sales3:UpdateAcc ID="UpdateAccountControl" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
