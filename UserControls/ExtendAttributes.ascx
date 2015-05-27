<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="ExtendAttributes" Codebehind="ExtendAttributes.ascx.cs" %>
<link href="/App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript">
    function numberOnly(textString) {
        if (textString.value.length > 0) {
            textString.value = textString.value.replace(/[^\d]+/g, '');
        }
    }
</script>

<script language="javascript" type="text/javascript">
   
    function AddAttribute() {
        var chkHasDomain = document.getElementById("<%=CheckHasDomain.ClientID%>");
        var oDivAddAttr = document.getElementById("divAddNewAttr");
        if (chkHasDomain.checked) {
            oDivAddAttr.style.visibility = "visible";
            oDivAddAttr.style.display = "block";
        }
        else {
            oDivAddAttr.style.visibility = "hidden";
            oDivAddAttr.style.display = "none";
        }
    }
</script>

<table cellpadding="1" cellspacing="1">
    <tr>
        <td colspan="3">
            <asp:Label ID="ExtendAttributeErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="3">
        </td>
    </tr>
    <tr>
        <td style="width: 140px;">
            <asp:Label ID="Label1" runat="server" Text="Attribute Name :"></asp:Label>
        </td>
        <td style="width: 222px;">
            <asp:TextBox ID="TextAttributeName" runat="server" Width="200" ToolTip="Attribute Name"
                TabIndex="1"></asp:TextBox>
        </td>
        <td style="padding-left: 5px">
            <asp:RequiredFieldValidator ID="TextAttributeNameRequiredFieldValidator" runat="server"
                ErrorMessage="*" ControlToValidate="TextAttributeName" ValidationGroup="SaveAttribute"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Attribute Type :"></asp:Label>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="DropDownAttributeType" runat="server" Width="206" TabIndex="2" ToolTip="Attribute Type">
                <asp:ListItem Value="0">Active</asp:ListItem>
                <asp:ListItem Value="86">InActive</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Attribute Description :"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextDisplayName" runat="server" Width="200" ToolTip="Attribute Description"
                TabIndex="3"></asp:TextBox>
        </td>
        <td style="padding-left: 5px">
            <asp:RequiredFieldValidator ID="TextDisplayNameRequiredFieldValidator" runat="server"
                ErrorMessage="*" ControlToValidate="TextDisplayName" ValidationGroup="SaveAttribute"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Default Value :"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextDefaultValue" runat="server" Text="" Width="200" TabIndex="4"
                ToolTip="Default Value"></asp:TextBox>
        </td>
        <td style="padding-left: 5px">
            <asp:RequiredFieldValidator ID="TextDefaultValueRequiredFieldValidator" runat="server"
                ErrorMessage="*" ControlToValidate="TextDefaultValue" ValidationGroup="SaveAttribute"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td colspan="2">
            <asp:CheckBox ID="CheckInputRequired" runat="server" Text="Input Required" Width="200" TabIndex="5"
                ToolTip="Input Required" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td colspan="2">
            <asp:CheckBox ID="CheckHasDomain" runat="server" Text="Has Domain" Checked="false" OnClick="AddAttribute()"
                ToolTip="Domain Needed" TabIndex="6" />
        </td>
    </tr>
</table>
<div id="divAddNewAttr" style="visibility:hidden;display:none;" >
    <table cellpadding="1" cellspacing="1">
        <tr>
            <td style="width: 140px">
                <asp:Label ID="Label7" runat="server" Text="Attribute Id :"></asp:Label>
            </td>
            <td style="width: 100px" colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="TextAddNewAttributeId" runat="server" Text="" TabIndex="7" Width="200"
                                ToolTip="New Attribute Id" onChange="numberOnly(this);" onKeyUp="numberOnly(this);"
                                onKeyPress="numberOnly(this);"></asp:TextBox>
                        </td>
                        <td style="padding-left: 27px" colspan="2">
                            <asp:RequiredFieldValidator ID="TextAddNewAttributeIdRequiredFieldValidator" runat="server"
                                ControlToValidate="TextAddNewAttributeId" ValidationGroup="AddAttributeGroup" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 140px">
                <asp:Label ID="Label8" runat="server" Text="Attribute Value :"></asp:Label>
            </td>
            <td style="width: 100px" colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="TextAddNewAttribute" runat="server" Width="200" ToolTip="New Attribute Name"
                                TabIndex="8" />
                        </td>
                        <td style="padding-left: 27px">
                            <asp:RequiredFieldValidator ID="TextAddNewAttributeRequiredFieldValidator" runat="server"
                                ControlToValidate="TextAddNewAttribute" ValidationGroup="AddAttributeGroup" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td style="padding-left: 15px">
                            <asp:Button ID="ButtonAddNewAttribute" runat="server" Text="Add" OnClick="ButtonAddNewAttribute_Click"
                                ToolTip="Add Attribute" TabIndex="9" ValidationGroup="AddAttributeGroup" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="2">
                <asp:ListBox ID="ListAddAttribute" runat="server" Height="100px" Width="206" ToolTip="Attribute List "
                    TabIndex="10"></asp:ListBox>
            </td>
        </tr>
    </table>
</div>
<table cellpadding="1" cellspacing="1">
    <tr>
        <td colspan="2" style="height: 15px">
        </td>
    </tr>
    <tr>
        <td style="width: 140px">
        </td>
        <td>
            <asp:Button ID="ButtonSaveAttribute" runat="server" Text="Save" Width="75" OnClick="ButtonSaveAttribute_Click"
                ToolTip="Save Attribute" TabIndex="11" ValidationGroup="SaveAttribute" />
        </td>
    </tr>
</table>
