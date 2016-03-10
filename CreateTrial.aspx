<%@ Page Title="" Language="C#" MasterPageFile="~/BaseMaster.Master" AutoEventWireup="true" CodeBehind="CreateTrial.aspx.cs" Inherits="B24.Sales4.UI.CreateTrial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator." Visible="false" CssClass="b24-errortext"></asp:Label>
    <asp:Panel ID="CreateTrailMainPnl" runat="server" Visible="true">
        <div>
            <p class="b24-doc-title">
                Signup form
            </p>
            <p class="b24-helptext">
                Use this form to create a trial subscription for on eor more users.
            </p>
        </div>
        <div>
            <p class="b24-doc-subtitle">
                Part 1: Account Information (Required)
            </p>
            <p class="b24-helptext">
                Specify the account details for this subscription (all fields required).
            </p>
        </div>
        <div>
            <table class="b24-form">
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel" align="right">
                        <asp:Label ID="CompanyNameLabel" runat="server" Text="Company"></asp:Label>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:TextBox ID="CompanyNameTextBox" runat="server" MaxLength="80"></asp:TextBox>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:RequiredFieldValidator runat="server" CssClass="b24-formdescription" ControlToValidate="CompanyNameTextBox"
                            Display="Dynamic" ErrorMessage="CompanyName is Required" ID="CompanyNameValidator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel" align="right">
                        <asp:Label ID="DepartmentLabel" CssClass="b24-forminputlabel" runat="server" Text="Department"></asp:Label>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:TextBox ID="DepartmentTextBox" runat="server" MaxLength="80"></asp:TextBox>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:RequiredFieldValidator runat="server" CssClass="b24-formdescription" ControlToValidate="DepartmentTextBox"
                            Display="Dynamic" ErrorMessage="Department is Required" ID="DepartmentValidator"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel" align="right">
                        <asp:Label ID="CountryLabel" CssClass="b24-forminputlabel" runat="server" Text="Country"></asp:Label>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:DropDownList ID="CountryDropDownList" runat="server" CssClass="b24-formdescription"
                            DataSourceID="CountryDataSource" DataTextField="Text" DataValueField="Value">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel" align="right">
                        <asp:Label ID="SalesGroupLabel" CssClass="b24-forminputlabel" runat="server" Text="Sales Group"></asp:Label>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:DropDownList ID="SalesGroupDropDownList" runat="server" CssClass="b24-formdescription">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel" align="right">
                        <asp:Label ID="AssignToLabel" CssClass="b24-forminputlabel" runat="server" Text="Assign To"></asp:Label>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:DropDownList ID="AssignToDropDownList" runat="server" CssClass="b24-formdescription">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <div>
            <p class="b24-doc-subtitle">
                Part 2: Collections (Required)
            </p>
            <p class="b24-helptext">
                Specify the collections available to this account. If you do not specify any collections,
                then this account will not be able to access any content.
            </p>
            <ul class="b24-helpbulletin">
                <li>Use common sense - Never, ever set up a trial that includes all collections!</li>
                <li>Select only specific collections that match the prospect's needs. Adding collections
                    unrelated to your client/prospect's interests can be overwhelming, prolong the sales
                    cycle, and detract from your proposed solution. </li>
            </ul>
            <table class="b24-form">
                <tr class="b24-formelement">
                    <td class="b24-forminputlabel">
                        <asp:CheckBoxList CssClass="b24-formdescription" runat="server" ID="CollectionsCheckBoxList"
                            RepeatColumns="3" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                    <td style="width: 15%"></td>
                    <td>
                        <span runat="server" class="b24-forminputlabel" id="collectionError" style="color: Red"
                            visible="false">Collections is Required</span>
                    </td>
                </tr>
            </table>
            <p class="b24-helptext">
                Note: all users will be automatically assigned <span style="text-decoration: underline">all</span> the selected collections.
            </p>
        </div>
        <hr />
        <div>
            <p class="b24-doc-subtitle">
                Part 3: Users (Optional)
            </p>
            <p class="b24-helptext">
                Users registered in this form will automatically receive a welcome email.
            </p>
            <p class="b24-helptext">
                The first user registered will be the
			  administrator for the subscription. If you need to add more users than
			  the form allows, you can do so later using "manage account."
            </p>
            <p class="b24-helptext">* required field<br />
                ** temporary password, to be changed at first login</p>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="PendingRegistrationsLabel" runat="server" Visible="false" CssClass="b24-helptext"
                            Text="The following registration have been submitted and will be processed shortly."></asp:Label>
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
                    <td class="b24-formtableheading">First Name *
                    </td>
                    <td class="b24-formtableheading">Last Name *
                    </td>
                    <td class="b24-formtableheading">Email *
                    </td>
                    <td class="b24-formtableheading">Login
                    </td>
                    <td class="b24-formtableheading">Password **
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
            
        </div>
        <hr />
        <div>
            <table class="b24-form">
                <tr class="b24-formelement">
                    <td>
                    </td>
                    <td class="b24-forminputlabel">
                        <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Click"
                            CssClass="b24-forminput" CausesValidation="true" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            
            <table>
                <asp:XmlDataSource ID="CountryDataSource" runat="server" DataFile="~/XML/CountryOptions.xml"
                    XPath="CountryParam/Options/Option"></asp:XmlDataSource>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
