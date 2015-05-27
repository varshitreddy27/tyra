<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="B24.Sales3.UserControl.SubscriptionCollection" Codebehind="SubscriptionCollection.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<link href="App_Themes/Classic/Calendar.css" rel="stylesheet" type="text/css" />


<script type="text/javascript" language="javascript">
    function checkDate(sender, args) {
        if (sender._selectedDate < new Date()) {
            alert("Select a date greater than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
</script>
<p>
    <asp:UpdatePanel ID="ErrorTextUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="CollectionErrorLabel" runat="server" Visible="false" CssClass="b24-errortext"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</p>
<asp:UpdatePanel ID="ApplicationListUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table cellspacing="2" cellpadding="2">
            <tbody>
                <tr>
                    <td style="width:120px">
                        <asp:Label ID="Label2" runat="server" Text="Application :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ApplicationDropDownList" runat="server" Width="206px" >
                        </asp:DropDownList>
                    </td>
                    
                        <td>
                            <asp:Button ID="UpdateApplicationButton" runat="server" OnClick="UpdateApplicationButton_Click"
                                Text="Update" ToolTip="Update Application " Width="75" />
                        </td>
                        <td style="text-align:left;"><span style="margin-right:8px;"><i>Note: Changing the application will change the login url for all users.</i></span></td>
                </tr>
            </tbody>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="CollectionListUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <br />
        <br />
        <asp:GridView ID="CollectionGridView" runat="server" AutoGenerateColumns="false"
            HeaderStyle-CssClass="b24-report-title" BorderColor="Gray" DataKeyNames="CollectionID" CellPadding="4">
            <Columns>
                <asp:TemplateField HeaderText="Collection Name" ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Name") %>'
                            Width="150" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Access" ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        <asp:DropDownList ID="AccessDropDownList" runat="server" Width="100" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Seat Count" ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        <asp:TextBox ID="SeatCountTextBox" runat="server" Width="75" Text='<%# Eval("SeatCount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Collection Expiration Date" ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        
                        <asp:TextBox ID="AccessExpiresTextBox" runat="server" ToolTip="Expiration Date" TabIndex="5"
                        Width="150" Visible="false"></asp:TextBox>
                        <cc1:CalendarExtender ID="ca" OnClientDateSelectionChanged="checkDate" runat="server"
                        TargetControlID="AccessExpiresTextBox"/>
                    
                        <asp:Label ID="AccessExpiresLabel" runat="server" Visible="false" Text="" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Chapter Downloads Allowed" ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        <asp:DropDownList ID="ChapterDownloadDropDownList" Width="156"  runat="server" >
                            <asp:ListItem Text="Allowed" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Not Allowed" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>               
                <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-BorderColor="Gray" ItemStyle-BorderColor="Gray">
                    <ItemTemplate>
                        <asp:ImageButton ID="UpdateCollectionButton" CommandArgument='<%# Eval("CollectionID") %>'
                            CommandName="Update" runat="server" ImageUrl="~/images/GVUpdateButton.gif" AlternateText="Update" />
                        <asp:ImageButton ID="DeleteButton" CommandArgument='<%# Eval("CollectionID") %>'
                            CommandName="Delete" runat="server" ImageUrl="~/images/GVDeleteButton.gif" AlternateText="Delete" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="b24-report-title"></HeaderStyle>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<br />
<br />
<asp:UpdatePanel ID="AddCollectionUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Label ID="AddCollectionLabel" runat="server" CssClass="b24-doc-subtitle" Text="Add A New Colletion" />
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="2">
            <tr align="center">
                <td>
                    <asp:Label ID="NewCollectionLabel" runat="server" Text="Collection Name"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="NewAccessLabel" runat="server" Text="Access"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="NewSeatCountLabel" runat="server" Text="Seat Count"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="NewExpiryDateLabel" runat="server" Text="Collection Expiriation Date"></asp:Label>
                </td>               
                <td>
                    <asp:Label ID="NewChapterDownLoadLabel" runat="server" Text="Chapter DownLoads Allowed"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:DropDownList ID="AddCollectionDropDownList" runat="server" Width="186px" />
                </td>
                <td>
                    <asp:DropDownList ID="NewAccessDropDownList" runat="server" Width="186px" />
                </td>
                <td>
                    <asp:TextBox ID="NewSeatCountTextBox" runat="server" Text="All" Width="180px" />
                </td>
                <td>
                    <asp:TextBox ID="NewExpiryDateTextBox" runat="server" Width="180px" /><asp:Image ID="CalenderImage" 
                        runat="server" Height="19px" ImageUrl="~/images/Calendar.gif" />
                    <cc1:CalendarExtender Format="MMM dd, yyyy" ID="NewExpiryDateAccordinCalender" runat="server"
                         TargetControlID="NewExpiryDateTextBox" PopupButtonID="CalenderImage"></cc1:CalendarExtender>                   
                </td>
                <td>
                    <asp:DropDownList ID="NewChapterDownloadDropDownList" runat="server" Width="156px">
                        <asp:ListItem Text="Allowed" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Not Allowed" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="AddButton" runat="server" Text="Add" Width="75" OnClick="AddButton_OnClick" />
                </td>
                <tr>
                <td colspan="3">
                </td>
                <td>
                </td>    
                <td colspan="2">
                </td>            
                </tr>
            </tr>
        </table>
        <br />
        <br />
    </ContentTemplate>
</asp:UpdatePanel>
