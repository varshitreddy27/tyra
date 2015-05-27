<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UserControl.UserDetails" Codebehind="UserDetails.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Account Information</p>
<div>
    <asp:Label ID="UserDetailsErrorLabel" runat="server" Visible="False" CssClass="b24-errortext"></asp:Label></div>
<table id="SubscriptionInfoTable" border="0" cellpadding="1" cellspacing="1" runat="server">
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="LoginLabel1" Text="B24 Login :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="LoginLabel" Width="124px"></asp:Label>
        </td>
        <td>
        </td>
        <td rowspan="10" valign="top">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" Text="B24 SubID :"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="SubIdLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label2" runat="server" Text="Company/Dept :"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="CompanyDepartmentLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Text="Application :"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="ApplicationLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label6" runat="server" Text="Contract# :"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="ContractLabel" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="NameLabel" Text="Name :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="B24NameLabel"></asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="FirstNameLabel" runat="server" Text="First Name :" 
                Visible="False"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="FirstNameTextBox" runat="server" Visible="False" Width="200px" 
                MaxLength="80"></asp:TextBox>
               
         <asp:RequiredFieldValidator ID="FirstNameRequiredFieldValidator" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="FirstNameTextBox" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
        </td> 
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LastNameLabel" runat="server" Text="Last Name :" Visible="False"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="LastNameTextBox" runat="server" Style="margin-bottom: 0px" Visible="False"
                Width="200px" MaxLength="80"></asp:TextBox>
                    
            <asp:RequiredFieldValidator ID="LastNameRequiredValidator" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="LastNameTextBox" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator> 
                </td>        
        
    </tr>
    <tr>
        <td valign="top" align="right">
            <asp:Label runat="server" ID="CollectionLabel1" Text="Collections :"></asp:Label>
        </td>
        <td valign="top">
            <asp:Label runat="server" BorderStyle="None" ID="CollectionLabel" Style="margin-bottom: 6px"
                Width="310px"></asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="EmailLabel1" Text="Email :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="EmailLabel"></asp:Label>
            <asp:TextBox ID="EmailTextBox" runat="server" Visible="False" Style="margin-bottom: 0px"
                Width="200px" MaxLength="96"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="UserDetail"
                runat="server" ControlToValidate="EmailTextBox" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
          <asp:RegularExpressionValidator ID="EmailFormatValidator" runat="server" Display="Dynamic" ValidationGroup="UserDetail"
                ControlToValidate="EmailTextBox" ErrorMessage="Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
         </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="CostCenterLabel1" Text="Cost Center :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="CostCenterLabel"></asp:Label>
            <asp:DropDownList Style="margin-bottom: 0px" runat="server" ID="CostCenterDropDownList"
                Visible="False" Height="25px" Width="200px">
            </asp:DropDownList>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="ScrambledLabel1" Text="Scrambled :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="ScrambledLabel"></asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="RestrictionsLabel1" Text="Restrictions :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="RestrictionsLabel"></asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label runat="server" ID="LostLoginLabel1" Text="Last Login :"></asp:Label>
        </td>
        <td>
            <asp:Label runat="server" ID="LostLoginLabel"></asp:Label>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="5" align="center">
            <asp:Button runat="server" ID="EditButton" Text="Edit" OnClick="EditButton_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="5" align="center">
            <asp:Button runat="server" ID="UpdateButton" Text="Update" ValidationGroup="UserDetail" OnClick="UpdateButton_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="CancelButton" Text="Cancel" OnClick="CancelButton_Click" />
        </td>
    </tr>
</table>
