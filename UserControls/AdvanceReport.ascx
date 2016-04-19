<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceReport.ascx.cs"
    Inherits="B24.Sales4.UserControl.AdvanceReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">

    function SetReportName(reportname, task, reportDescription) {
        document.getElementById("<%= ReportSPNameHiddenField.ClientID %>").value = reportname;
        document.getElementById("<%= TaskHiddenField.ClientID %>").value = task;
        document.getElementById("<%= ReportNameHiddenField.ClientID %>").value = reportDescription;
        if (document.Forms != undefined) {
            thisForm = document.Forms[0];
            thisForm.submit();
        }
    }
    function SetReportQId(reportqid, task) {
        document.getElementById("<%= TaskHiddenField.ClientID %>").value = task;
        document.getElementById("<%=ReportQueueIDHiddenField.ClientID%>").value = reportqid;
        if (document.Forms != undefined) {
            thisForm = document.Forms[0];
            thisForm.submit();
        }
    }
    function ChangeTask(task) {
        document.getElementById("<%= TaskHiddenField.ClientID %>").value = task;
        return true;
    }
    function GetDropDownValue(dropDownListId, textBoxId) {
        var ddList = document.getElementById(dropDownListId);
        var textBox = document.getElementById(textBoxId);
        textBox.value = ddList.options[ddList.selectedIndex].value;
    }
</script>
<asp:MultiView ID="ReportView" runat="server">
    <asp:View runat="server" ID="ReportUserControlView">
      
        <table>
            <tr>
                <td align="left">
                    <p>
                        <asp:Label ID="AvailableReport" runat="server" CssClass="b24-doc-subtitle" Text="Available Reports"> 
                        </asp:Label></p>
                    <p>
                        <asp:Label ID="HelpTextLabel" runat="server" CssClass="b24-helptext" Text="Click on the report name to run it:"> 
                        </asp:Label></p>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="AvailableReportGridview" runat="server" AutoGenerateColumns="False"
                        CssClass="resultGrid" CellPadding="4" OnRowDataBound="AvailableReportGridview_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Report" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("ReportDescription") %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="HelpTextLabel" runat="server" Text='<%# Eval("ReportHelpText") %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="false" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="ReportNameLabel" runat="server" Text='<%# Eval("ReportName")  %>'
                                        Visible="false" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="false" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="CategoryLabel" runat="server" Text='<%# Eval("ReportCategory") %>'
                                        Visible="false" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:HiddenField ID="ReportNameHiddenField" runat="server" Value='<%#Eval("ReportName") %>' />
                                    <asp:LinkButton ID="RunLinkButton" runat="server" CssClass="GoButton" Text="Run it" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="left">
                    <br />
                    <br />
                    <asp:Label ID="AvailableResultLabel" runat="server" CssClass="b24-doc-subtitle" Text="Available Results"> 
                    </asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="ResultHelpTextLabel" runat="server" CssClass="b24-helptext" Text="The following cached report results are currently available."> 
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="AvailableResultGridView" runat="server" AutoGenerateColumns="False"
                        CssClass="resultGrid" CellPadding="4" OnRowDataBound="AvailableResultGridView_RowDataBound"
                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                        <RowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Completed" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="CompletedLabel" runat="server" Text='<%# Eval("Completed") %>' />
                                </ItemTemplate>
                                <ControlStyle Width="100%"></ControlStyle>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ControlStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:Label ID="ReportHeaderLabel" runat="server" Text='<%# Eval("ReportHeader") %>' />
                                </ItemTemplate>
                                <ControlStyle Width="100%"></ControlStyle>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:HiddenField ID="ReportQidHiddenField" runat="server" Value='<%#Eval("ReportQid") %>' />
                                    <asp:LinkButton ID="ViewLinkButton" runat="server" CssClass="GoButton" Text="View" />
                                    <asp:LinkButton ID="DownloadLinkButton" runat="server" CssClass="GoButton" Text="Download" />
                                    <asp:LinkButton ID="DeleteLinkButton" runat="server" CssClass="GoButton" Text="Delete" />
                                </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                        <EditRowStyle BorderColor="Black" BorderStyle="Double" BorderWidth="1px" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="NoResultLabel" runat="server" Text="No queued results are currently available."
            Visible="False"></asp:Label>
    </asp:View>
    <asp:View runat="server" ID="ReportParameterView">
        <table>
            <tr>
                <td class="b24-doc-title">
                    Submit a New Report
                </td>
            </tr>
            <tr>
                <td class="b24-helptext">
                    Advanced Reports are calculated offline. Results are available for viewing after
                    the calculations have been completed. Previously calculated results remain available
                    for later viewing.
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID="ReportParameterPlaceHolder" runat="server">
            <asp:Panel ID="ResultSubmittedPanel" runat="server" Visible="false">
            </asp:Panel>
            <asp:HiddenField ID="DynamicFieldNames" runat="server" />
            <table>        
                <tr>
                    <td>
                        <asp:Label ID="ReportDescriptionLabel" runat="server" Text="Report"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="AnotherDateRangeButton" runat="server" Visible="false" Text="Another Date Range"
                            OnClientClick="ChangeTask('ViewRun')" />
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <div>      
            <asp:LinkButton ID="CancelLButton1" runat="server" Text="Back to Report List" OnClientClick="ChangeTask('Cancel')"></asp:LinkButton>
        </div>
    </asp:View>
    <asp:View runat="server" ID="ReportResultView">
        <asp:PlaceHolder ID="ReportResultViewPlaceHolder" runat="server">  
            <div class="buttondiv">
                <asp:Button ID="DownloadReportButton" runat="server" CssClass="Button2" Text="Download Report" OnClientClick="ChangeTask('Download')" />
                <br />
                <asp:LinkButton ID="CancelLButton2" runat="server" CssClass="Button2" Text="Back to Report List" OnClientClick="ChangeTask('Cancel')"></asp:LinkButton>
                <br />
            </div>
        </asp:PlaceHolder>

    </asp:View>
</asp:MultiView>
<asp:TextBox Visible="false" ID="EndDateTextBox" runat="server"
    Width="200"></asp:TextBox><asp:Image Visible="false" ID="CalenderImage" runat="server"
        Height="19px" ImageUrl="~/images/Calendar.gif" />
<cc1:CalendarExtender Format="MMM dd, yyyy" ID="AccessExpiresAccordinCalender" runat="server"
    TargetControlID="EndDateTextBox" PopupButtonID="CalenderImage">
</cc1:CalendarExtender>
<asp:HiddenField ID="ReportSPNameHiddenField" runat="server" />
<asp:HiddenField ID="ReportNameHiddenField" runat="server" />
<asp:HiddenField ID="ReportQueueIDHiddenField" runat="server" />
<asp:HiddenField ID="TaskHiddenField" runat="server" />
