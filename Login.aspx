<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="B24.Sales4.UI.Login" Title="Login" Codebehind="Login.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="logincontrolarea">
    <asp:MultiView ID="LoginPageMultiView" runat="server">
      <asp:View ID="LoginView" runat="server"><!-- 0 Basic Log In-->
        <div class="loginheader">
            <asp:Label ID="LogInTitleLbl" runat="server" Text="Log In" CssClass="b24-doc-title"></asp:Label>
        </div>
        <table>
            <tr>
                <td class="loginRegCell">
                    <asp:Label runat="server" ID="usernameLbl1" Text="Username:" 
                        CssClass="logininput"></asp:Label>
                </td>
                <td class="loginRegCell">
                    <asp:TextBox ID="usernameTbx1" runat="server" ToolTip="Username" TabIndex="1" 
                        CssClass="logininput"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="usernameTbx1"
                        ErrorMessage="username required" Display="Dynamic" ValidationGroup="UserLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                <td class="loginRegCell">
                    <asp:Label runat="server" ID="passwordLbl" Text="Password:" 
                        CssClass="logininput"></asp:Label>
                </td>
                <td class="loginRegCell">
                    <asp:TextBox ID="passwordTbx" runat="server" ToolTip="Password" TabIndex="2" TextMode="Password"
                        CssClass="logininput"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="passwordTbx"
                        ErrorMessage="password required" Display="Dynamic" ValidationGroup="UserLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="loginRegCell"></td>
                <td class="loginRegCell">
                    <asp:Button runat="server" ID="LoginBtn" TabIndex="3" OnClick="Login_Click" CausesValidation="false" Text="Log In" ToolTip="Log In" />
                </td>
            </tr>
            <tr>
                <td  class="loginTallCell"></td>
                <td  class="loginTallCell">
                    <asp:LinkButton runat="server" ID="ForgorPasswordLB" TabIndex="4" CausesValidation="false" OnClick="Login_ForgotPassword" CssClass="loginFPbtn" Text="Forgotten your login information?"></asp:LinkButton>
                </t>
            </tr>
        </table>
        
    </asp:View>
     <asp:View ID="ChangePasswordView" runat="server" ><!-- 1 Change Password -->
          <asp:Label ID="ChangePasswordTitleLbl" runat="server" Text="Change your Password" CssClass="b24-doc-title"></asp:Label>
         <div class="logininfo">You must change your password in order to log in.<br />Passwords must be at least 4 characters in length.</div>
         <div>
         <table>
             <tr>
                 <td class="loginRegCell">
                     <asp:Label runat="server" class="loginlbl2" ID="usernameLbl">Username: </asp:Label>
                 </td>
                 <td class="loginRegCell">
                     <asp:TextBox ID="usernameTbx" runat="server" ToolTip="Username" TabIndex="1"
                        CssClass="logininput"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="usernameRequired" runat="server" ControlToValidate="usernameTbx"
                        ErrorMessage="Username required" Display="Dynamic" ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                 </td>
             </tr>
             <tr>
                 <td class="loginRegCell">
                      <asp:Label runat="server" class="loginlbl2" ID="tempPasswordLbl">Temporary Password: </asp:Label>    
                 </td>
                 <td class="loginRegCell">
                     <asp:TextBox ID="tempPasswordTbx" runat="server" ToolTip="Temporary Password" TabIndex="2"
                        MaxLength="63" TextMode="Password" CssClass="logininput"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="oldPasswordRequired" runat="server" ControlToValidate="txtNewPassword"
                        ErrorMessage="Temporary password required" Display="Dynamic" ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                 </td>
             </tr>
             <tr>
                 <td class="loginRegCell">
                     <asp:Label runat="server" class="loginlbl2" ID="newPasswordLbl">New Password: </asp:Label>
                 </td>
                 <td class="loginRegCell">
                     <asp:TextBox ID="txtNewPassword" runat="server" ToolTip="New Password" TabIndex="3"
                        MaxLength="65" TextMode="Password" CssClass="logininput"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="NewPasswordRequiredValidator" runat="server" ControlToValidate="txtNewPassword"
                        ErrorMessage="New password required" Display="Dynamic" ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                 </td>
             </tr>
             <tr>
                 <td class="loginRegCell">
                     <asp:Label runat="server" class="loginlbl2" ID="confirmLbl">Confirm: </asp:Label>
                 </td>
                 <td class="loginRegCell">
                     <asp:TextBox ID="txtConfirm" runat="server" ToolTip="Confirm" TabIndex="4" MaxLength="65"
                    TextMode="Password" CssClass="logininput"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmRequiredValidator" runat="server" ControlToValidate="txtConfirm"
                    ErrorMessage="Confirm password required" Display="Dynamic" ValidationGroup="ChangePassword">*</asp:RequiredFieldValidator>
                 </td>
             </tr>
             <tr>
                 <td class="loginRegCell"></td>
                 <td class="loginRegCell">
                     <asp:Button ID="ChangeButton" runat="server" Text="Log In" OnClick="ChangeButton_Click" CausesValidation="false"
                    TabIndex="4" ToolTip="Log In and Change Password" ValidationGroup="ChangePassword" CssClass="loginbtn"/>
                 </td>
             </tr>
         </table>
        </asp:View>
        <asp:View ID="ForgotPasswordView" runat="server">  <!-- 2 Forgot Password -->
            <div class="loginFParea">
                <div class="loginheader">
                    <asp:Label ID="ForgotPasswordTitleLbl" runat="server" Text="Forgot your Password?" CssClass="b24-doc-title"></asp:Label>
                </div>
                <div class="logininfo">Enter your email to have a temporary password emailed to you.</div>
                <table>
                    <tr>
                        <td class="loginRegCell">
                            <asp:Label runat="server" ID="EmailAddressLbl" Text="Email Address: "></asp:Label>
                        </td>
                        <td class="loginRegCell">
                            <asp:TextBox runat="server" ID="EmailAddressTbx" ValidationGroup="ForgotPassword"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailRequiredFieldValidator"
                                ControlToValidate="EmailAddressTbx"
                                Display="Static"
                                ValidationGroup="ForgotPassword"
                                ErrorMessage="Email Address"
                                runat="server">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="loginRegCell"></td>
                        <td class="loginRegCell">
                            <asp:Button runat="server" ID="EmailPasswordBtn" Text="Email my password" CausesValidation="false" ValidationGroup="ForgotPassword" OnClick="Login_EmailPassword" />
                        </td>
                    </tr>
                    <tr>
                        <td class="loginTallCell"></td>
                        <td class="loginTallCell">
                            <asp:LinkButton runat="server" ID="BackLoginLB2" Text="Back to Login" CausesValidation="false" CssClass="GoButton" OnClick="BackToLogin_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
    
      </asp:MultiView>
      <div class="b24-errorDiv">
         <asp:Label ID="ErrorLabel" runat="server" CssClass="b24-errorcode" Text="" ></asp:Label>
      </div>
</div>

</asp:Content>

