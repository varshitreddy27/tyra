<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceReport.ascx.cs"
    Inherits="B24.Sales3.UserControl.AdvanceReport" %>

<script type="text/javascript">

    function SetReportName(reportname, task, reportDescription) {
        document.getElementById("<%= ReportSPNameHiddenField.ClientID %>").value = reportname;
        document.getElementById("<%= TaskHiddenField.ClientID %>").value = task;
        document.getElementById("<%= ReportNameHiddenField.ClientID %>").value = reportDescription;
        thisForm = document.Forms[0];
        thisForm.submit();
    }
    function SetReportQId(reportqid, task) {
        document.getElementById("<%= TaskHiddenField.ClientID %>").value = task;
        document.getElementById("<%=ReportQueueIDHiddenField.ClientID%>").value = reportqid;
        thisForm = document.Forms[0];
        thisForm.submit();
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
                                    <asp:LinkButton ID="DownLoadLinkButton" runat="server" CssClass="GoButton" Text="DownLoad" />
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
                        <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClientClick="ChangeTask('Cancel')" />
                    </td>
                </tr>
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
    </asp:View>
    <asp:View runat="server" ID="ReportResultView">
        <asp:PlaceHolder ID="ReportResultViewPlaceHolder" runat="server">
            <asp:Button ID="ResultOkButton" runat="server" Text="Ok" OnClientClick="ChangeTask('Cancel')" />
            <br />
            <br />
            <asp:Button ID="DownLoadReportButton" runat="server" Text="DownLoad Report" OnClientClick="ChangeTask('DownLoad')" />
        </asp:PlaceHolder>
    </asp:View>
</asp:MultiView>
<asp:HiddenField ID="ReportSPNameHiddenField" runat="server" />
<asp:HiddenField ID="ReportNameHiddenField" runat="server" />
<asp:HiddenField ID="ReportQueueIDHiddenField" runat="server" />
<asp:HiddenField ID="TaskHiddenField" runat="server" />
