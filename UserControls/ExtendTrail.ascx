<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales4.UserControl.ExtendTrail" Codebehind="ExtendTrail.ascx.cs" %>
<link href="../App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<table id ="NotAccessableTable" runat="server" visible="False" >
<tr class="b24-formelement">
        <td class="b24-formdescription">
            Extend Trial is not accessable for this account.
        </td>
        <td>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<table id ="ContentTable" runat = "server" border="0" cellpadding="0" cellspacing="0" class="b24-form">
    <tr>
        <td>
            <asp:Label ID="ExtendTrailErrorLabel" runat="server" Text="" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td class="b24-formdescription">
            This trial may be reactivated or extended.
        </td>
        <td>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr class="b24-formelement">
        <td class="b24-formdescription">
            Trial Duration
        </td>
        <td>
            <asp:Label ID="TrialDurationLabel" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr class="b24-formelement">
        <td class="b24-formdescription">
            Trial Started
        </td>
        <td>
            <asp:Label ID="TrialStartedLabel" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formdescription">Extendable for</span>
        </td>
        <td>
            <asp:Label ID="ExtendableLabel" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <asp:Label ID="SubscriptionDisabledLabel" runat="server" Text="This subscription is currently disabled."></asp:Label>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span class="b24-formdescription"> &nbsp;</span>
        </td>
        <td>
            <asp:Button ID="EditButton" runat="server" TabIndex="1" Text = "Edit" 
                onclick="EditButton_Click" Width="84px"></asp:Button>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            <span class="b24-formrequired"></span><span runat ="server" id="AddSpan" class="b24-formdescription">Add &nbsp;</span>
        </td>
        <td>
            <asp:TextBox ID="ExtendTrialAddDaysTextBox" runat="server" TabIndex="1" MaxLength="64"></asp:TextBox>
        </td>
    </tr>
    <tr class="b24-formelement">
        <td>
            &nbsp;
        </td>
        <td >
            <asp:Button ID="UpdateButton" TabIndex="5" OnClick="UpdateButton_Click" runat="server"
                ValidationGroup="UserDetail" Text="Update" ToolTip="Update Extend Trail Details"
                Visible="false" Width="79px" />
            <asp:Button ID="EditCancelButton" TabIndex="6" OnClick="EditCancelButton_Click" runat="server" Text="Cancel" Visible="false" />
            <br />
            
        </td>
    </tr>
    <tr>
    <td colspan ="2">
        <asp:Label ID="B24AsistanceLabel" CssClass="b24-assistancerequired" runat="server"
                Text="Books24x7 Assistance Required." Visible="False"></asp:Label>
            <br />
            <asp:Label ID="B24ContactLabel" CssClass="b24-assistancerequired" runat="server"
                Text="Please contact Technical Implementation: ti-team@books24x7.com" 
            Visible="False"></asp:Label>
    </td>
    </tr>
</table>
<asp:HiddenField ID="LevelHiddenField" runat="server" />
<asp:HiddenField ID="ExtendHiddenField" runat="server" />
<asp:HiddenField ID="HasAccessHiddenField" runat="server" />

