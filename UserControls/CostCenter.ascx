<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales4.UserControl.CostCenter" Codebehind="CostCenter.ascx.cs" %>
<link href="../App_Themes/Classic/sales4.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript">
    function numberOnly(textString) {
        if (textString.value.length > 0) {
            textString.value = textString.value.replace(/[^\d]+/g, '');
        }
    }
</script>
<p>
    <asp:Label ID="CostCenterErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
</p>
<table >
    <tr>
        <th>
            <asp:Label ID="CostCenterManagement" runat="server"  Text="Cost Center Management" CssClass="b24-doc-subtitle" Visible="false" ></asp:Label>
        </th>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="CostCenterGridView" runat="server" AutoGenerateColumns="False"
                HeaderStyle-CssClass="b24-report-title" DataKeyNames="CostCenterID" EditRowStyle-CssClass="b24-editrow"
                CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderText="Seq" ItemStyle-Wrap="false" ControlStyle-Width="20">
                        <ItemTemplate>
                            <asp:Label ID="SeqLabel" runat="server" Text='<%# Eval("Sequence") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="SeqTextBox" runat="server" Text='<%# Eval("Sequence") %>' Width="20" />
                        </EditItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" ControlStyle-Width="200">
                        <ItemTemplate>
                            <asp:Label ID="CostCenterLabel" runat="server" Text='<%# Eval("CostCenters") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="CostCenterTextBox" runat="server" Text='<%# Eval("CostCenters") %>'
                                Width="200" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" ControlStyle-Width="200">
                        <ItemTemplate>
                            <asp:Label ID="DescLabel" runat="server" Text='<%# Eval("Description") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="DescTextBox" runat="server" Text='<%# Eval("Description") %>'
                                Width="200" />
                        </EditItemTemplate>
                        <ItemStyle Wrap="true"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="EditButton" CommandArgument='<%# Eval("CostCenterID") %>' CommandName="Edit"
                                runat="server" ImageUrl="~/images/GVEditButton.gif" AlternateText="Edit" Visible="false"
                                Height="16px" />
                            <asp:ImageButton ID="UpdateButton" CommandArgument='<%# Eval("CostCenterID") %>'
                                CommandName="Update" runat="server" ImageUrl="~/images/GVUpdateButton.gif" AlternateText="Update"
                                Visible="false" />
                            <asp:ImageButton ID="CancelButton" CommandArgument='<%# Eval("CostCenterID") %>'
                                CommandName="Cancel" runat="server" ImageUrl="~/images/GVCancelButton.gif" AlternateText="Cancel"
                                Visible="false" />
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="b24-report-title"></HeaderStyle>
                <EditRowStyle CssClass="b24-editrow"></EditRowStyle>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />
<br />
<table width="100%">
    <tr align="left">
        <th class="b24-doc-subtitle">
            Add a Cost Center
        </th>
    </tr>
</table>
<table cellpadding="1" cellspacing="1">
    <tr>
        <td style="width: 100px">
            <asp:Label ID="LabelSequence" runat="server" Text="Sequence :" />
        </td>
        <td>
            <asp:TextBox ID="NewCCSequence" runat="server" Width="50" onChange="numberOnly(this);" onKeyUp="numberOnly(this);" onKeyPress="numberOnly(this);" ></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="NewCCSequenceRequiredFieldValidator" runat="server"
                ErrorMessage="*" ControlToValidate="NewCCSequence" ValidationGroup="CostCenterAdd"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Name :" />
        </td>
        <td>
            <asp:TextBox ID="NewCCName" runat="server" Width="200"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="NewCCNameRequiredFieldValidator" runat="server" ErrorMessage="*"
                ControlToValidate="NewCCName" ValidationGroup="CostCenterAdd" ></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
       <td>
            <asp:Label ID="Label2" runat="server" Text="Description :" />
        </td>
        <td colspan="2">
            <asp:TextBox ID="NewCCDescription" runat="server" Height="80" TextMode="MultiLine"
                Width="200"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="height:15px;">
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td colspan="2">
            <asp:Button ID="AddCostCenter" Text="Add" runat="server" Width="75" OnClick="AddCostCenter_Click"  ValidationGroup="CostCenterAdd" />
        </td>
    </tr>
</table>
