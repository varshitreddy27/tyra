<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.ChangePassword" Codebehind="ChangePassword.ascx.cs" %>

<p id="HeaderText" runat="server" class="b24-doc-title">
    Assign New Password</p>
<asp:Label ID="ChangePasswordError" runat="server" Text="" Visible="false" CssClass="b24-errortext"></asp:Label>
<table class="b24-form" border="0" cellspacing="2" cellpadding="0">
    <tr>
        <td>
            <span class="b24-forminputlabel">New Password</span>
        </td>
        <td>
            <asp:TextBox ID="txtNewPassword" runat="server" ToolTip="New Password" TabIndex="1"
                MaxLength="63" TextMode="Password" CssClass="b24-forminput"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="NewPasswordRequiredValidator" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="Required" Display="Dynamic" ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
            <span class="b24-forminputlabel">Confirm</span>
        </td>
        <td>
            <asp:TextBox ID="txtConfirm" runat="server" ToolTip="Confirm" TabIndex="2" MaxLength="63"
                TextMode="Password" CssClass="b24-forminput"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="ConfirmRequiredValidator" runat="server" ControlToValidate="txtConfirm"
                ErrorMessage="Required" Display="Dynamic" ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
            <span class="b24-forminputlabel">Type</span>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="typeList" runat="server" ToolTip="Password Type" TabIndex="3" CssClass="b24-forminput">
            </asp:DropDownList>
        </td>
    </tr>
    <tr >
        <td colspan"3">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td colspan"2">
            <asp:Button ID="ChangeButton" runat="server" Text="Change" OnClick="ChangeButton_Click"
                TabIndex="4" ToolTip="Change Password" ValidationGroup="ChangePassword" CssClass="b24-forminput"/>
        </td>
    </tr>
</table>
