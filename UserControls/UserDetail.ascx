<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales4.UserControl.UserDetail" Codebehind="UserDetail.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title">
    User Detail</p>
<table border="0" cellpadding="0" cellspacing="0" class="b24-form">
    <tr>
        <td>
            <asp:Label ID="UserDetailError" runat="server" Text="" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span class="b24-formdescription">First Name</span>
        </td>
        <td>
            <asp:TextBox ID="txtFirstName" runat="server" ToolTip="First Name" TabIndex="1" MaxLength="80"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="firstNameRequiredValidator" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="txtFirstName" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span class="b24-formdescription">Last Name</span>
        </td>
        <td>
            <asp:TextBox ID="txtLastName" runat="server" ToolTip="Last Name" TabIndex="2" MaxLength="80"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="lastNameRequiredValidator" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="txtLastName" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span class="b24-formdescription">Email</span>
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" ToolTip="Email" TabIndex="3" MaxLength="96"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="emailIdRequiredValidator" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="txtEmail" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
        <td>
            <asp:RegularExpressionValidator ID="emailFormatValidator" runat="server" Display="Dynamic"
                ControlToValidate="txtEmail" ErrorMessage="Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span class="b24-formdescription">Skillport User
                ID &nbsp;</span>
        </td>
        <td>
            <asp:TextBox ID="txtSkillPortId" runat="server" ToolTip="Fixed User Name" TabIndex="4"
                MaxLength="64"></asp:TextBox>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            &nbsp;
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            &nbsp;
        </td>
        <td>
            <asp:Button ID="UpdateButton" TabIndex="5" OnClick="UpdateButton_Click" runat="server"
                ValidationGroup="UserDetail" Text="Update" ToolTip="Update User Details" />
        </td>
    </tr>
</table>
