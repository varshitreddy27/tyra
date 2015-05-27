<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.UserReportAccess" Codebehind="UserReportAccess.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table border="0" cellpadding="1" cellspacing="1">
    <tr>
        <td>
            <asp:UpdatePanel ID="ErrorPanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="UserReportAccessErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="UserAccessReportLabel" runat="server" Visible="true" Text="User Reporting allows customers to run ad hoc or scheduled reports on their subscription usage.  You can assign reporting privileges for specific reports to this user and have the system email a complete set of instructions for reports below.  When a report finishes, the user receives an email notification - selecting 'NOTIFY SALESPERSON' causes an email to also be sent to the user's salesperson whenever a report is run. "
                CssClass="b24-helptext"></asp:Label>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="ViewUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:MultiView ID="Multiview" runat="server">
            <asp:View ID="EditView" runat="server">
                <table border="0" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="ReportsCheckBoxList" runat="server" OnDataBound="ReportsCheckBoxList_DataBound"
                                RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="UpdateButton" OnClick="UpdateButton_Click" Width="75" runat="server"
                                Text="Update" />
                            <asp:Button ID="EditCancelButton" OnClick="EditCancelButton_Click" runat="server"
                                Text="Cancel" Width="75" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="ReadOnlyView" runat="server">
                <table border="0" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="ReportsReadCheckBoxList" runat="server" OnDataBound="ReportsCheckBoxList_DataBound"
                                RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Horizontal" Enabled="false"
                                Font-Bold="False">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="EditButton" OnClick="EditButton_Click" Width="75" runat="server"
                                Text="Edit" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
