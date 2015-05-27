<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales3.UI.LibraryReportAccess" Title="Report Access" Codebehind="LibraryReportAccess.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div class="b24-report-frame"><asp:Literal ID="ltMessage" runat="server" Visible = "false"></asp:Literal></DIV>

<div id="ReportingUserTitle" style="">
<img border="0" alt="" src="images/greydot.gif" width="90%" height="1" align="middle" />
<div id="ReportingUserSubTitle1" class="b24-formbanner">Add Reporting Users</div>
<div id="ReportingUserSubTitle2" class="b24-helptext">Register users here to allow reporting on this subscription.</div>
</div>
<div>
<table cellspacing="0" cellpadding="2" border="0" class="b24-form">
<tr class="b24-formelement">
<td colspan="4" class="b24-formtableheading">First Name *</td>
<td colspan="4" class="b24-formtableheading">Last Name *</td>
<td colspan="4" class="b24-formtableheading">Email *</td>
<td colspan="4" class="b24-formtableheading">Login</td>
<td colspan="4" class="b24-formtableheading">Password **</td>
</tr>
<tr class="b24-formelement">
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="FirstNameTextBox1" Width="100" 
        runat="server" MaxLength="80" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ControlToValidate="FirstNameTextBox1" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LastNameTextBox1" Width="100" 
        runat="server" MaxLength="80" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
        ControlToValidate="LastNameTextBox1" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="EmailTextBox1" Width="100" 
        runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="EmailTextBox1" ErrorMessage="*"></asp:RequiredFieldValidator>    
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LoginTextBox1" Width="100" 
        runat="server" MaxLength="50" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="PasswordTextBox1" Width="100" 
        runat="server" MaxLength="50" TextMode="Password" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="PasswordTextBox1" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
 <td>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
        ControlToValidate="EmailTextBox1" ErrorMessage="*Invalid Email" 
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
 </td>
</tr>
<tr class="b24-formelement">
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="FirstNameTextBox2" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LastNameTextBox2" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="EmailTextBox2" Width="100px" 
        runat="server" MaxLength="50" />    
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LoginTextBox2" Width="100" 
        runat="server" MaxLength="50" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="PasswordTextBox2" Width="100" 
        runat="server" MaxLength="50" TextMode="Password" /></td>
<td>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
        ControlToValidate="EmailTextBox2" ErrorMessage="*Invalid Email" 
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
 </td>
</tr>
<tr class="b24-formelement">
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="FirstNameTextBox3" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LastNameTextBox3" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="EmailTextBox3" Width="100" 
        runat="server" MaxLength="50" />    
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LoginTextBox3" Width="100" 
        runat="server" MaxLength="50" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="PasswordTextBox3" Width="100" 
        runat="server" MaxLength="50" TextMode="Password" /></td>
 <td>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
        ControlToValidate="EmailTextBox3" ErrorMessage="*Invalid Email" 
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
 </td>
</tr>
<tr class="b24-formelement">
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="FirstNameTextBox4" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LastNameTextBox4" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="EmailTextBox4" Width="100" 
        runat="server" MaxLength="50" />   
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LoginTextBox4" Width="100" 
        runat="server" MaxLength="50" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="PasswordTextBox4" Width="100" 
        runat="server" MaxLength="50" TextMode="Password" /></td>
 <td>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
        ControlToValidate="EmailTextBox4" ErrorMessage="*Invalid Email" 
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
 </td>
</tr>
<tr class="b24-formelement">
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="FirstNameTextBox5" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LastNameTextBox5" Width="100" 
        runat="server" MaxLength="80" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="EmailTextBox5" Width="100" 
        runat="server" MaxLength="50" />    
    </td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="LoginTextBox5" Width="100" 
        runat="server" MaxLength="50" /></td>
<td colspan="4">
    <asp:TextBox CssClass="b24-forminput" id="PasswordTextBox5" Width="100" 
        runat="server" MaxLength="50" TextMode="Password" /></td>
<td>
 <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic"
        ControlToValidate="EmailTextBox5" ErrorMessage="*Invalid Email" 
        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
 </td>
</tr>
<tr><td colspan="24">
<table cellspacing="2" cellpadding="0" border="0" class="b24-form">
<tr class="b24-formelement">
<td align="right">
    &nbsp;</td>
<td align="right">
    <asp:CheckBoxList ID="ReportCategoryCheckBoxList" runat="server" 
        CssClass="b24-formdescription" RepeatColumns="5" RepeatDirection="Horizontal">
    </asp:CheckBoxList>
</td>
<td align="right">
    &nbsp;</td>
</tr>
<tr class="b24-formelement">
<td align="right" colspan="2" nowrap="nowrap" class="b24-forminputlabel">
    <asp:Button ID="SubmitButton" runat="server" Text="Update"  
        CssClass="b24-forminput" onclick="SubmitButton_Click"/> </td>
<td>
    &nbsp;</td>
    <td class="b24-forminputhelp">
        &nbsp;</td>
</tr>
    </table>
<p class="b24-helptext">
				* required field<br />** temporary password, to be changed at first login
</p>
</td></tr>
</table> 

</div>
</asp:Content>

