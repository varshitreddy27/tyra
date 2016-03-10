<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.LibraryReportAccess" Codebehind="LibraryReportAccess.ascx.cs" %>
<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <div>
            <asp:Label ID="LibraryReportAccessErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView" runat="server">
            <asp:View ID="ReadOnlyView" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="ReportUsersTextLabel" runat="server" Text="Click button to add reporting users for this library sub."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="AddreportingUsersButton" runat="server" 
                                Text="Add Reporting Users" onclick="AddreportingUsersButton_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="EditView" runat="server">
                <div id="ReportingUserTitle" style="">
                    <%--<img border="0" alt="" src="images/greydot.gif" width="90%" height="1" align="middle" />--%>
                    <%--<div id="ReportingUserSubTitle1" class="b24-formbanner">
                        Add Reporting Users</div>--%>
                    <div id="ReportingUserSubTitle2" class="b24-helptext">
                        Register users here to allow reporting on this subscription.</div>
                </div>
                <table cellspacing="0" cellpadding="2" border="0" class="b24-form">
                    <tr class="b24-formelement">
                        <td colspan="4" class="b24-formtableheading">
                            First Name *
                        </td>
                        <td colspan="4" class="b24-formtableheading">
                            Last Name *
                        </td>
                        <td colspan="4" class="b24-formtableheading">
                            Email *
                        </td>
                        <td colspan="4" class="b24-formtableheading">
                            Login
                        </td>
                        <td colspan="4" class="b24-formtableheading">
                            Password **
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox1" Width="100" runat="server"
                                MaxLength="80" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirstNameTextBox1"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox1" Width="100" runat="server"
                                MaxLength="80" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LastNameTextBox1"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox1" Width="100" runat="server"
                                MaxLength="50" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="EmailTextBox1"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox1" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox1" Width="100" runat="server"
                                MaxLength="50" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="PasswordTextBox1"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                ControlToValidate="EmailTextBox1" ErrorMessage="*Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox2" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox2" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox2" Width="100px" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox2" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox2" Width="100" runat="server"
                                MaxLength="50" TextMode="Password" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                ControlToValidate="EmailTextBox2" ErrorMessage="*Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox3" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox3" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox3" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox3" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox3" Width="100" runat="server"
                                MaxLength="50" TextMode="Password" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                                ControlToValidate="EmailTextBox3" ErrorMessage="*Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox4" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox4" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox4" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox4" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox4" Width="100" runat="server"
                                MaxLength="50" TextMode="Password" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                                ControlToValidate="EmailTextBox4" ErrorMessage="*Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr class="b24-formelement">
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox5" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox5" Width="100" runat="server"
                                MaxLength="80" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox5" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox5" Width="100" runat="server"
                                MaxLength="50" />
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox5" Width="100" runat="server"
                                MaxLength="50" TextMode="Password" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic"
                                ControlToValidate="EmailTextBox5" ErrorMessage="*Invalid Email" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="24">
                            <table cellspacing="2" cellpadding="0" border="0" class="b24-form">
                                <tr class="b24-formelement">
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:CheckBoxList ID="ReportCategoryCheckBoxList" runat="server" CssClass="b24-formdescription"
                                            RepeatColumns="5" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="b24-formelement">
                                    <td align="right" colspan="2" nowrap="nowrap" class="b24-forminputlabel">
                                        <asp:Button ID="SubmitButton" runat="server" Text="Update" CssClass="b24-forminput"
                                            OnClick="SubmitButton_Click" />
                                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" CssClass="b24-forminput"
                                            OnClick="CancelButton_Click" CausesValidation="False"/>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="b24-forminputhelp">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <p class="b24-helptext">
                                * required field<br />
                                ** temporary password, to be changed at first login
                            </p>
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
