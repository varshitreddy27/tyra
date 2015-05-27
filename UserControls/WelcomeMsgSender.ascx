<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.WelcomeMsgSender" Codebehind="WelcomeMsgSender.ascx.cs" %>

<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="WelcomeMessageErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="MultiView" runat="server">
            <asp:View ID="ReadOnlyView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Welcome messages will be sent from the salesperson listed on the subscription unless alternate information is entered here: ">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="SenderLabel" runat="server" Text="Current sender:  " />
                            <asp:Label ID="CurrentSenderLabel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Button ID="ChangeSenderButton" runat="server" Text="Change Sender" OnClick="ChangeSenderButton_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="EditView" runat="server">
                <p class="b24-helptext">
                    Note: this email address must be from an @books24x7.com or @skillsoft.com address.
                    You can choose to enter no-reply@skillsoft.com in this field if you do not want
                    messages to come from a named user.</p>
                <table cellspacing="2" cellpadding="0" border="0" class="b24-form">
                    <tr class="b24-formelement">
                        <td align="right" class="b24-forminputlabel" nowrap="1">
                            <span class="b24-formrequired"></span><span class="b24-formdescription">New sender:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="NewSenderTextBox" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="revNewSender" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ControlToValidate="NewSenderTextBox" ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td align="right" class="b24-forminputlabel" nowrap="1">
                            <span class="b24-formrequired"></span><span class="b24-formdescription">Confirm</span>
                        </td>
                        <td>
                            <asp:TextBox ID="ConfirmTextBox" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CompareValidator ID="cvConfirm" runat="server" ControlToValidate="ConfirmTextBox"
                                ControlToCompare="NewSenderTextBox" ErrorMessage="Input the same email address as above"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td align="center" colspan="2" class="b24-forminputlabel" nowrap="1">
                            <asp:Button ID="UpdateButton" runat="server" Text="Update" OnClick="UpdateButton_OnClick" />
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_OnClick" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
