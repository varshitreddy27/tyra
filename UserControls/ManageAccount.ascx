<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.ManageAccount" CodeBehind="ManageAccount.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<link href="App_Themes/Classic/Calendar.css" rel="stylesheet" type="text/css" />
<link href="App_Themes/Classic/jquery-ui-custom.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript">

    function numberOnly(textString) {
        if (textString.value.length > 0) {
            textString.value = textString.value.replace(/[^\d]+/g, '');
        }
    }
    function checkDate(sender, args) {
        if (sender._selectedDate < new Date()) {
            alert("Select a date greater than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
</script>

<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Manage Account
</p>
<div>
    <asp:Label ID="ManageAccountErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
</div>
<table class="b24-subscription">
    <tr>
        <td style="width: 30px"></td>
        <td>
            <asp:MultiView ID="Multiview" runat="server">
                <asp:View ID="EditView" runat="server">
                    <table border="0" cellpadding="1" cellspacing="1">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px" align="right">
                                            <asp:Label ID="Label1" runat="server" Text="Company :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="CompanyTextBox" runat="server" ToolTip="Company Name"
                                                Width="200" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="CompanyRequiredFieldValidator" runat="server"
                                                ErrorMessage="**" ControlToValidate="CompanyTextBox"
                                                ValidationGroup="ManageSubsription"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="SubIDLbl" runat="server" Text="B24 SubID :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="SubIDLblRead" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label2" runat="server" Text="Department :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="DepartmentTextBox" runat="server"
                                                ToolTip="Department/Company Name"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label3" runat="server" Text="Application :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ApplicationDropDown" runat="server" ToolTip="Application"
                                                Width="206">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ContractNumLbl" runat="server" Text="Contract # :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="ContractNumTextBox" runat="server"
                                                ToolTip="Contract Number"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label4" runat="server" Text="Group Code :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="GroupCodeTextBox" runat="server" ToolTip="GroupCode"
                                                Width="200"
                                                Enabled="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="StartDateLbl2" runat="server" Text="Start Date :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="StartDateLbl2Read" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label6" runat="server" Text="End Date :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="EndDateTextBox" runat="server" ToolTip="End Date" onFocus="javascript:vDateType='1'"
                                                Width="200"></asp:TextBox><asp:Image ID="CalenderImage" runat="server"
                                                    Height="19px" ImageUrl="~/images/Calendar.gif" />
                                            <cc1:CalendarExtender Format="MMM dd, yyyy" ID="AccessExpiresAccordinCalender" runat="server"
                                                TargetControlID="EndDateTextBox" PopupButtonID="CalenderImage"
                                                OnClientDateSelectionChanged="checkDate">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label7" runat="server" Text="Type :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="TypeDropDown" runat="server" ToolTip="Subscription Type"
                                                Width="206">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <div style="width: 50px">
                                </div>
                            </td>
                            <td valign="top">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="SeatsLbl" runat="server" Text="Seats :" onChange="numberOnly(this);"
                                                onKeyUp="numberOnly(this);" onKeyPress="numberOnly(this);"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="SeatsTextBox" runat="server" CssClass="b24-forminput" ToolTip="Number of Seats"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="RegisteredUsersLbl" runat="server" Text="Registered Users :" onChange="numberOnly(this);"
                                                onKeyUp="numberOnly(this);" onKeyPress="numberOnly(this);"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="RegisteredUsersTextBox" runat="server" CssClass="b24-forminput" ToolTip="Number of Registered Users"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label8" runat="server" Text="Buy Book URL :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="BuyBookURLTextBox" runat="server" CssClass="b24-forminput" ToolTip="BuyBookURL"
                                                Width="200"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid URL, Provide a valid url like 'http://books24x7.com'"
                                                ControlToValidate="BuyBookURLTextBox" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                                ValidationGroup="ManageSubsription"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label9" runat="server" Text="Set CTG Limit :" onChange="numberOnly(this);"
                                                onKeyUp="numberOnly(this);" onKeyPress="numberOnly(this);"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="SetCTGLimitTextBox" runat="server" CssClass="b24-forminput" ToolTip="Set CTG Limit"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="CTGLbl" runat="server" Text="Chapters To Go :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="CTGLblTextBox" runat="server" CssClass="b24-forminput" ToolTip="Chapters To Go Status"
                                                Width="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label21" runat="server" Text="SalesPerson :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="SalespersonLabel" runat="server" ToolTip="SalesPerson"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label5" runat="server" Text="Assign To :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="SalesPersonDropDown" runat="server" ToolTip="SalesPerson"
                                                Width="206">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label10" runat="server" Text="SalesGroup :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="SalesGroupDropDown" runat="server" ToolTip="SalesGroup"
                                                Width="206">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="SeatOverflowCheckBox" runat="server"
                                                Text="Allow seat overflow" ToolTip="Allow seat overflow"
                                                Width="200"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="DisabledCheckBox" runat="server" Text="Disabled" ToolTip="Disabled"
                                                Width="200"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="EcommerceCheckBox" runat="server" Text="ECommerce" ToolTip="ECommerce"
                                                Width="200" Visible="false"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Button ID="UpdateButton" OnClick="UpdateButton_Click" runat="server" Text="Update"
                                    Width="75" ValidationGroup="ManageSubsription" />
                                <asp:Button ID="EditCancelButton" OnClick="EditCancelButton_Click" runat="server" Text="Cancel"
                                    Width="75" ValidationGroup="ManageSubsription" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ReadOnlyView" runat="server">
                    <table border="0" cellpadding="1" cellspacing="1">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px" align="right">
                                            <asp:Label ID="Label11" runat="server" Text="Company :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="CompanyReadLabel" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px" align="right">
                                            <asp:Label ID="SubIDLbl2" runat="server" Text="B24 SubID :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="SubIDLbl2Read" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label12" runat="server" Text="Department :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="DepartmentReadLabel" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label13" runat="server" Text="Application :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="ApplicationReadLabel" runat="server" ToolTip="Application"
                                                Width="206px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ContractNumLbl2" runat="server" Text="Contract # :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="ContractNumLbl2Read" runat="server" ToolTip="Contract Number"
                                                Width="206px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label14" runat="server" Text="Group Code :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="GroupCodeReadLabel" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="StartDateLbl" runat="server" Text="Start Date :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="StartDateLblRead" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="EndDateReadLabel" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label16" runat="server" Text="Type :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="TypeReadLabel" runat="server" ToolTip="Subscription Type"
                                                Width="206px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <div style="width: 50px">
                                </div>
                            </td>
                            <td valign="top">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="SeatsLbl2" runat="server" Text="Seats :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="SeatsLbl2Read" runat="server" CssClass="b24-forminput" ToolTip="Number of Seats"
                                                Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="RegisteredUsersLbl2" runat="server" Text="Registered Users :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="RegisteredUsersLbl2Read" runat="server" CssClass="b24-forminput" ToolTip="Number of Registered Users"
                                                Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label17" runat="server" Text="Buy Book URL :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="BuyBookUrlReadLabel" runat="server" CssClass="b24-forminput" ToolTip="BuyBookURL"
                                                Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label18" runat="server" Text="Set CTG Limit :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="CTGLimitReadLabel" runat="server" ToolTip="Set CTG Limit"
                                                Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="CTGLbl2" runat="server" Text="Chapters To Go :"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="CTGLbl2Read" runat="server" ToolTip="Chapters to Go Status"
                                                Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label19" runat="server" Text="Sales Person :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="SalespersonReadLabel" runat="server" ToolTip="SalesPerson"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label20" runat="server" Text="SalesGroup :"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="SalesGroupReadLabel" runat="server" ToolTip="SalesGroup"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="SeatOverflowReadCheckBox" runat="server"
                                                Text="Allow seat overflow" ToolTip="Allow seat overflow"
                                                Width="200" Enabled="false"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="DisabledReadCheckBox" runat="server" Text="Disabled" ToolTip="Disabled"
                                                Width="200" Enabled="false"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="EcommerceReadCheckBox" runat="server" Text="ECommerce" ToolTip="ECommerce"
                                                Width="200" Enabled="false" Visible="false"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Button ID="EditButton" OnClick="EditButton_Click" runat="server"
                                    Text="Edit" Width="75" />
                            </td>
                        </tr>
                    </table>

                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>
<asp:Panel ID="SubscriptionInfoDetailOanel" runat="server">
    <div>
        <br />
        <asp:Label ID="NotesLbl" runat="server" CssClass="b24-doc-subtitle" Visible="false">Notes:</asp:Label>
        <br />
        <asp:Label ID="NotesContentLbl" runat="server" Visible="false"></asp:Label>
    </div>
    <asp:PlaceHolder ID="DetailsTablePH" runat="server">
    </asp:PlaceHolder>
</asp:Panel>
