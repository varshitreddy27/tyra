<%@ Page Language="C#" MasterPageFile="~/BaseMaster.master" AutoEventWireup="true"
    Inherits="B24.Sales3.UI.LibraryTrail" Title="Library Trial" CodeBehind="LibraryTrial.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:Label ID="AccessDeniedErrorLabel" runat="server" Text="Access denied. Contact your administrator."
        Visible="false" CssClass="b24-errortext"></asp:Label>
    <asp:Panel ID="TrialPanel" runat="server">
        <div>
            <p class="b24-doc-title">
                Create a Library Trial</p>
            <p class="b24-helptext">
                Use this form to create a library site trial subscription.</p>
        </div>
        <div>
            <p class="b24-doc-subtitle">
                Part 1: Account Information (Required)</p>
            <p class="b24-helptext">
                Specify the account details for this subscription (all fields required).</p>
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
                Part 2: Collections (Required)</p>
            <p class="b24-helptext">
                Specify the collections available to this account. If you do not specify any collections,
                then this account will not be able to access any content.</p>
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
                    <td style="width: 15%">
                    </td>
                    <td>
                        <span runat="server" class="b24-forminputlabel" id="collectionError" style="color: Red"
                            visible="false">Collections is Required</span>
                    </td>
                </tr>
            </table>
            <p class="b24-helptext">
                Note: all users will be automatically assigned <span style="text-decoration: underline">
                    all</span> the selected collections.</p>
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
