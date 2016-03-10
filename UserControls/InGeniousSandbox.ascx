<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales4.UserControl.IngeniousSandbox" Codebehind="IngeniousSandbox.ascx.cs" %>
    <%@ Register Tagname="SandboxProperty" tagprefix="Sales3" src="~/UserControls/ChangeSandboxProperties.ascx"%>
<link href="App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<table>
    <tr>
        <td>
            <asp:Label ID="MessageLabel" runat="server" Text="This subscription is part of an inGenius Sandbox"
                CssClass="b24-helptext"></asp:Label>
        </td>
    </tr>
    <tr runat="server" id="ConfirmByCompanyRow" visible="false">
        <td>
            <asp:Label ID="ConfirmByCompanyLabel" runat="server" Text="Sandbox found for this subscription, confirm this Sandbox">
            </asp:Label>
            &nbsp;
            <asp:Button ID="ConfirmByCompanyButton" runat="server" Text="Confirm" OnClick="ConfirmByCompanyButton_Click" />
        </td>
    </tr>
    <tr runat="server" id="ConfirmByPasswordRootRow" visible="false">
        <td>
            <asp:Label ID="ConfirmByPasswordLabel" runat="server" Text="Sandbox found for this subscription, confirm this Sandbox"
                CssClass="b24-doc-subtitle"></asp:Label>       
            &nbsp; <asp:Button ID="ConfirmByPasswordButton" runat="server" Text="Confirm" OnClick="ConfirmByPasswordButton_Click"
                ToolTip="Confirm this SandBox" />
        </td>
    </tr>
    <tr id="NewSandboxRow" runat="server" visible="false">
        <td>
            <table>
                <tr ID="CreateNewSandBoxRow" runat="server">
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="NewSandboxHeaderLabel" runat="server" 
                                        Text="Create a new inGenius Sandbox" CssClass="b24-doc-subtitle">
                                        </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p class="b24-helptext">
                                        Click Create Sandbox button to create a new sandbox for this subscription. This will check for an existing sandbox for this SubID</p>
                                </td>
                            </tr>
                            <tr>
                                <td>                                    
                                    <asp:Button ID="SandBoxCreateButton" Text="Create SandBox" OnClick="SandBoxCreateButton_Click"
                                        runat="server" ToolTip="CreateNewSandBox"/>
                                </td>
                            </tr>
                        </table>
                       <br />
                       <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="AddtoSandBox" runat="server" Text="Add to an existing inGenius Sandbox"
                                        CssClass="b24-doc-subtitle"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <p class="b24-helptext">
                                        Enter the SubID of the subscription already in the Sandbox that you want to add this subscription to as well
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="SandBoxNotFoundLabel" runat="server" CssClass="b24-errortext" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td style="width:30">
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="SubID"></asp:Label>
                                </td>
                                <td style="width:250px">
                                    <asp:TextBox ID="FindSandBoxTextBox" runat="server" Width="200"                                   
                                        ToolTip="Enter PasswordRoot to be Searched"></asp:TextBox>  
                                    &nbsp;  
                                    <asp:RequiredFieldValidator ID="FindsandboxrequiredValidator" runat="server" 
                                        ControlToValidate="FindSandBoxTextBox" ValidationGroup="FindSandboxes" ErrorMessage="**">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Button ID="FindSandBoxButton" runat="server" Text="LookUp" Width="75"
                                        OnClick="FindSandBoxButton_Click" ValidationGroup="FindSandboxes" 
                                        ToolTip="Find AccessKey SandBox" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="ConfirmSandBoxRow" runat="server" visible="false">
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <p class="b24-helptext">
                                        Select a company matching the SubID Entered,and Click Add to add this subscription
                                        to this sandbox.</p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="companyFoundLabel" runat="server" Text="Select a Company" 
                                        CssClass="b24-doc-subtitle"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="SandBoxRadioList" runat="server" RepeatColumns="4"></asp:RadioButtonList>          
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <asp:Button ID="UpdateToSandBoxButton" runat="server" OnClick="UpdateToSandBox_Click" ToolTip="Add to the Selected SandBox" Text="Add"/>
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="SandboxIdHidden" runat="server" Value="" />
            <asp:HiddenField ID="SandboxCompanyNameHidden" runat="server" Value="" />
            <asp:HiddenField ID="SandboxPasswordRootHidden" runat="server" Value="" />
        </td>
    </tr>
</table>
<%--<div><Sales3:SandboxProperty ID="SandboxPropertyUserControl" runat="server"/></div>--%>
