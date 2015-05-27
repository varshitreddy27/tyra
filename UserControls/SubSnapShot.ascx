<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.SubSnapShot" Codebehind="SubSnapShot.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<table border="0" cellpadding="1" cellspacing="1">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="1" cellspacing="1">
                        <tr>
                            <td colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px" align="right">
                                <asp:Label ID="SubIDLbl" runat="server" Text="B24 SubID :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="SubIDReadLbl" runat="server" TabIndex="1" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="CompanLbl" runat="server" Text="Company/Dept :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="CompanyReadLbl" runat="server" TabIndex="2" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ApplicationLbl" runat="server" Text="Application :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="ApplicationReadLbl" runat="server" TabIndex="3"
                                    Width="206px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ContractLbl" runat="server" Text="Contract# :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="ContractReadLbl" runat="server" TabIndex="4" Width="200px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="SeatsLbl" runat="server" Text="Seats :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="SeatsReadLbl" runat="server" TabIndex="5" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="RegUsersLbl" runat="server" Text="Registered Users :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="RegUsersReadLbl" runat="server" 
                                    TabIndex="6" Width="206px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="SalesGroupLbl" runat="server" Text="Salesgroup :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="SalesGroupReadLbl" runat="server" 
                                    TabIndex="7" Width="206px"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td align="right">
                                <asp:Label ID="SalesPersLbl" runat="server" Text="Salesperson :"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="SalesPersReadLbl" runat="server" 
                                    TabIndex="8" Width="206px"></asp:Label>
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
                                <asp:Label ID="TypeLbl" runat="server" Text="Type :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="TypeReadLbl" runat="server" CssClass="b24-forminput"
                                    Width="200px" TabIndex="9"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="StatusLbl" runat="server" Text="Status :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="StatusReadLbl" runat="server"  
                                    Width="200px" TabIndex="10"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="StartsLbl" runat="server" Text="Starts :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="StartsReadLbl" runat="server"  
                                    TabIndex="11"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ExpiresLbl" runat="server" Text="Expires :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="ExpiresReadLbl" runat="server" 
                                    TabIndex="12"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ChaptersToGoLbl" runat="server" Text="Chapters to Go :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="ChaptersToGoReadLbl" runat="server" 
                                    TabIndex="13"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="GroupCodeLbl" runat="server" Text="Group Code :"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="GroupCodeReadLbl" runat="server"
                                    TabIndex="13"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
           
        </table>