<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UserControl.AddUsers" Codebehind="AddUsers.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <tr>
        <td>
            <asp:Label ID="AddUserErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="AddUserHelpTextLabel" runat="server" Visible="false" CssClass="b24-helptext"
                Text="You may register additional users into this
        account. The seat count of Trials will be automatically adjusted, if
        necessary, to accomodate the additional user(s). Once a Paid account reaches
        its seat capacity, you will no longer be able to add new users."></asp:Label>
        </td>
    </tr>
    <tr>
        <td><p>
            <asp:Label ID="AddUserHelpText2Label" runat="server" Visible="false" CssClass="b24-helptext"
                Text="If all the seats are filled, this option will
        not be available."></asp:Label></p>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="NodirectAccessLabel" runat="server" Visible="false" CssClass="b24-helptext"
                Text="Users must be registered via seamless sign on (AICC, ticket etc)."></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="PendingRegistrationsLabel" runat="server" Visible="false" CssClass="b24-helptext"
                Text="The following registration have been submitted and will be processed shortly."></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="NoSeatsLabel" runat="server" Visible="false" CssClass="b24-helptext"
                Text="No seats available, cannot register additional users."></asp:Label>
        </td>
    </tr>
</table>
<asp:MultiView ID="Multiview" runat="server">
    <asp:View ID="EditView" runat="server">
        <table>
            <tr>
                <td>
                    <asp:GridView ID="PendingRegistrationGridView" runat="server" AutoGenerateColumns="False"
                        HeaderStyle-CssClass="b24-report-title" CellPadding="4">
                        <Columns>
                            <%-- First Name Column--%>
                            <asp:TemplateField HeaderText="First Name " ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="FirstNameLabel" Text='<%# Eval("FirstName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name " ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel" Text='<%# Eval("LastName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel3" Text='<%# Eval("Email") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Login" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("Login") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("Status") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Error" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("ErrorString") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="b24-report-title"></HeaderStyle>
                        <EditRowStyle CssClass="b24-editrow"></EditRowStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <th align="left">
                    <asp:Label ID="AddUserHeading" runat="server" Text="Add User" CssClass="b24-doc-subtitle"
                        Visible="true"></asp:Label>
                </th>
            </tr>
        </table>
        <table id="AdduserTable" cellspacing="0" cellpadding="2" border="0" class="b24-form">
            <tr class="b24-formelement">
                <td class="b24-formtableheading">
                    First Name *
                </td>
                <td class="b24-formtableheading">
                    Last Name *
                </td>
                <td class="b24-formtableheading">
                    Email *
                </td>
                <td class="b24-formtableheading">
                    Login
                </td>
                <td class="b24-formtableheading">
                    Password **
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox1" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox1" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox1" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox1" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox1" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                        ControlToValidate="EmailTextBox1" ErrorMessage="*Invalid Email" ValidationGroup="Addusers"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox2" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox2" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox2" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox2" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox2" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                        ControlToValidate="EmailTextBox2" ErrorMessage="*Invalid Email" ValidationGroup="Addusers"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox3" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox3" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox3" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox3" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox3" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                        ControlToValidate="EmailTextBox3" ErrorMessage="*Invalid Email" ValidationGroup="Addusers"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox4" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox4" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox4" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox4" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox4" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                        ControlToValidate="EmailTextBox4" ErrorMessage="*Invalid Email" ValidationGroup="Addusers"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr class="b24-formelement">
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="FirstNameTextBox5" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LastNameTextBox5" Width="100" runat="server"
                        MaxLength="80" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="EmailTextBox5" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="LoginTextBox5" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:TextBox CssClass="b24-forminput" ID="PasswordTextBox5" Width="100" runat="server"
                        MaxLength="50" />
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic"
                        ControlToValidate="EmailTextBox5" ErrorMessage="*Invalid Email" ValidationGroup="Addusers"
                        ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <table>
            <tr class="b24-formelement">
                <td class="b24-forminputlabel">
                    <asp:CheckBoxList  runat="server" ID="CollectionsCheckBoxList"
                        RepeatColumns="3" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <table cellspacing="2" cellpadding="0" border="0" class="b24-form">
                        <tr>
                            <td>
                                <asp:CheckBox ID="SendWelcomeEmailCheckBox" runat="server" Text="Send Welcome Email"
                                    Checked="true"></asp:CheckBox>
                            </td>
                        </tr>
                        <tr class="b24-formelement">
                            <td align="right" nowrap="nowrap" class="b24-forminputlabel">
                                <asp:Button ID="UpdateButton" runat="server" Text="Update" CssClass="b24-forminput"
                                    ValidationGroup="Addusers" OnClick="UpdateButton_Click" Visible="false" />
                                <asp:Button ID="EditCancelButton" runat="server" Text="Cancel" OnClick="EditCancelButton_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="B24AsistanceLabel" CssClass="b24-assistancerequired" runat="server"
                                    Text="Books24x7 Assistance Required." Visible="False"></asp:Label>
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
    <asp:View ID="ReadView" runat="server">
        <table>
            <tr>
                <td>
                    <asp:GridView ID="PendingRegistrationReadGridView" runat="server" AutoGenerateColumns="False"
                        HeaderStyle-CssClass="b24-report-title" CellPadding="4">
                        <Columns>
                            <%-- First Name Column--%>
                            <asp:TemplateField HeaderText="First Name " ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="FirstNameLabel" Text='<%# Eval("FirstName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name " ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel" Text='<%# Eval("LastName") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel3" Text='<%# Eval("Email") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Login" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("Login") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("Status") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Error" ItemStyle-Wrap="false" ControlStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="LastNameLabel1" Text='<%# Eval("ErrorString") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="b24-report-title"></HeaderStyle>
                        <EditRowStyle CssClass="b24-editrow"></EditRowStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="EditButton" runat="server" Text="Edit" OnClick="EditButton_Click" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>