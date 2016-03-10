<%@ Control Language="C#" AutoEventWireup="true"
    Inherits=" B24.Sales4.UserControl.UpdateAccount" Codebehind="UpdateAccount.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    .style1
    {
        width: 96px;
    }
</style>
<p>
<asp:Label ID="AccountUpdateErrorLabel" runat="server" Visible="False" CssClass="b24-errortext"></asp:Label>
</p>
<p><asp:Label CssClass="b24-doc-title" ID="AdjustCollectionLabel" runat="server" Text="Adjust Collections"
    Visible="False"></asp:Label></p>

<table>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp3Label1" runat="server" Text="You may add collections to this subscription at this time."
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp3Label2" runat="server" Text="- Enter a seat count number to assign collections to specific users."></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp3Label3" runat="server" Text="- Leave the default all to indicate that all users will have access to the collection."
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp3Label4" runat="server" Text="- Check Admin Selects to allow B24 subscription admins to manage collections. You must enter the number of user licenses, if checked."
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp12Label1" runat="server" Text="Select the collections which will be part of
					this subscription." Visible="False"></asp:Label>
            <asp:Label CssClass="b24-helptext" ID="CollectionHelp1Label2" runat="server" Text="Enter a seat count number to assign collections to specific users or leave the default 'all' to indicate that all users will have access to the collection."
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="PaidCollectionLabel" runat="server" Text="You may add collections to this subscription at this time. Enter a seat count number to assign collections to specific users or leave the default 'all' to indicate that all users will have					access to the collection."
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="SkillPortPaidCollectionLabel" runat="server"
                Text="You must enter a seat count for each collection based on what the customer has purchased.  The 'all' access value is not valid for SkillPort subscriptions.  The Admin Selects option should also NOT be checked.  This form of assigning collections does not operate in SkillPort subscriptions."
                Visible="False"></asp:Label>
        </td>
    </tr>
</table>
<br />
<asp:GridView ID="CollectionGridView" runat="server" AutoGenerateColumns="False"
    HeaderStyle-CssClass="b24-report-title" BorderColor="Gray" CellPadding="4" GridLines="None"
    OnRowDataBound="CollectionGridview_RowDataBound">
    <Columns>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:CheckBox ID="CollectionCheckBox" runat="server" Text='<%# Eval("CollectionCheckbox")%>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False" BorderColor="Gray"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:Label ID="CollectionName" runat="server" Text='<%# Eval("CollectionName")%>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False" BorderColor="Gray"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:TextBox ID="NoOfUser" runat="server" Width="50" Text='<%# Eval("NoOfUser") %>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False" BorderColor="Gray"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:Label ID="UserLabel" runat="server" TabIndex="5" Text='<%# Eval("UserLabel") %>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False" BorderColor="Gray"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:CheckBox ID="AdminSelect" runat="server" Text='<%# Eval("AdminCheckBox")%>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False" BorderColor="Gray"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:Label ID="AdminSelectLabel" runat="server" TabIndex="5" Text='<%# Eval("AdminSelect") %>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
            <ItemTemplate>
                <asp:Label ID="CollName" runat="server" TabIndex="5" Text='<%# Eval("CollName") %>' />
            </ItemTemplate>
            <HeaderStyle BorderColor="Gray"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
        </asp:TemplateField>
    </Columns>
    <HeaderStyle CssClass="b24-report-title"></HeaderStyle>
</asp:GridView>
<table>
    <tr>
        <td>
            <asp:Label CssClass="b24-doc-title" ID="ConverTrialLabel" runat="server" Text="Convert Trial (Optional)"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="RenewalLabel" runat="server" CssClass="b24-doc-title" Text="Renew Subscription"
                Visible="False"></asp:Label>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="ConvertTrialHelpLablel2" runat="server" Text="Choose the number of subscription seats purchased for the final Paid account. Note: if the Paid account has less
					seats than the Trial, you must choose below which registered Trial users
					will be converted to Paid.  Any users not converted will be deleted.
					Users may be registered after the account has been converted; seats and
					collections cannot be added." Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="PaidTransactForm" runat="server" Text="You may adjust the subscription seat count for
					this account. However, if there are fewer seat left in the subscription
					than there are currently registered users, you will need to choose which
					users will be remain and which will be deleted." Visible="False"></asp:Label>
        </td>
    </tr>
</table>
<br />
<table>
    <tr>
        <td colspan="2">
            <asp:CheckBox ID="ConvertTrailToPaidCheckBox" runat="server" Text="Convert Trail into a Paid Subscription!"
                Visible="False" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="ExpirationLabel" runat="server" Text="Expiration date  "></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ExpirationDateTextBox" runat="server" Width="159px" Enabled = "false" ></asp:TextBox>
            <asp:Image ID="CalenderImage" 
                        runat="server" Height="19px" ImageUrl="~/images/Calendar.gif" 
                Visible="False" />
                        <cc1:CalendarExtender Format="MMMM dd, yyyy" ID="NewExpiryDateAccordinCalender" runat="server"
                         TargetControlID="ExpirationDateTextBox" PopupButtonID="CalenderImage"></cc1:CalendarExtender>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox ID="SkillPortEnableCheckBox" runat="server" Text="SkillPort Enabled"
                Visible="False" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="User Seats"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="UserSeatsTextBox" runat="server" Width="85px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="DurationLabel" runat="server" Text="Duration" Visible="False"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="DurationDropDownList" runat="server" Width="110px" Visible="False"
                Height="22px">
                <asp:ListItem Value="1year">1 year</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<br />
<table>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="UserForm1Label" runat="server" Text="Select those users who will remain with the
					converted account." Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="UserForm2Label" runat="server" Text="Select those users who will remain with the converted account. You <u>must</u> remove all users when converting to a SkillPort enabled subscription"
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="b24-helptext" ID="PaidorSkillPortUserLabel" runat="server" Text="You may remove users from this subscription by
					unchecking the appropriate box(es) below.." Visible="False"></asp:Label>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td>
            <asp:CheckBoxList ID="UserListCheckBoxList" runat="server" RepeatColumns="3">
            </asp:CheckBoxList>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td class="style1">
        </td>
        <td>
            <asp:Button ID="UpdateButton" runat="server" Visible="false" Text="Update" Width="106px"
                OnClick="UpdateButton_Click" />
            <asp:Button ID="CancelButton" runat="server" Visible="false" Text="Cancel" Width="106px"
                OnClick="CancelButton_Click" />
        </td>
    </tr>
    <tr>
        <td class="style1">
        </td>
        <td>
            <asp:Button ID="EditButton" runat="server" Text="Edit" Width="106px" OnClick="EditButton_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Label ID="B24AsistanceLabel" CssClass="b24-assistancerequired" runat="server"
                Text="Books24x7 Assistance Required." Visible="False"></asp:Label>
            <br />
            <asp:Label ID="B24ContactLabel" CssClass="b24-assistancerequired" runat="server"
                Text="Please contact Technical Implementation: ti-team@books24x7.com" Visible="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="AccountUpdateDisabledLabel" runat="server" Visible="False">This subscription is curently disabled.</asp:Label>
        </td>
    </tr>
</table>
