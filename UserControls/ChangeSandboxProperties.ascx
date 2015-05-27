<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeSandboxProperties.ascx.cs"
    Inherits="B24.Sales3.UserControl.ChangeSandboxProperties" %>
<asp:Label ID="NewSandboxHeaderLabel" runat="server" Text="Modify Sandbox Properties"
    CssClass="b24-doc-subtitle"></asp:Label><br />
<asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="b24-alerttext" Visible="false"></asp:Label>
<asp:Panel ID="panel_SandboxPropertiesEdit" runat="server">
    <table width="100%" border="0" cellpadding="2" cellspacing="0" class="b24-profileeditbg">
        <tr style="height: 20px">
            <td style="width: 80%; padding-top: 5px;">
                <asp:Label runat="server" ID="descrEditLbl"></asp:Label>
            </td>
            <td align="right" style="padding-right: 10px; vertical-align: top; padding-top: 5px;">
                <%--                <asp:LinkButton ID="LinkButton1" runat="server" Text="Cancel" CausesValidation="false"
                    OnClick="CancelButton_Click"></asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height: 20px; vertical-align: bottom;">
                        <td>
                            <img src="images/_.gif" />
                        </td>
                        <td style="font-weight: bold">
                            Yes/No
                        </td>
                    </tr>
                    <asp:ListView ID="lv_SandboxPropertiesEdit" runat="server" EnableViewState="true"
                        OnItemCommand="SandboxProperties_OnItemCommand" OnItemDataBound="SandboxProperties_OnItemDataBound">
                        <LayoutTemplate>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td colspan="2">
                                    <hr class="b24-grayhr" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 60%; padding-top: 10px;">
                                    <asp:Label ID="lbl_PropertyName" runat="server" CssClass="b24-notifications_preftitle"
                                        Text='<%#Eval("PropertyName") %>'></asp:Label>
                                </td>
                                <td align="right" style="vertical-align: middle; text-align: center">
                                    <asp:CheckBox ID="cb_Status" runat="server" CssClass="b24-notifications_form" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </table>
            </td>
        </tr>
        <tr style="height: 30px">
            <td colspan="2" align="right" style="padding-right: 15px">
                <asp:LinkButton ID="bt_Save" runat="server" Text="Save Changes" OnClick="SaveButton_Click"></asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Panel>
