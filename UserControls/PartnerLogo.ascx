<%@ Control Language="C#" AutoEventWireup="true" Inherits="B24.Sales3.UserControl.PartnerLogo" Codebehind="PartnerLogo.ascx.cs" %>
<link href="App_Themes/Classic/sales3.css" rel="stylesheet" type="text/css" />
<p id="HeaderText" runat="server" class="b24-doc-title" visible="false">
    Parner Logo</p>

<script type="text/javascript">
    function toggleView(showEdit) {
        var readPanel = document.getElementById('ReadOnlyPanel');
        var editPanel = document.getElementById('EditViewPanel');
        var errorText = document.getElementById('<%=PartnerLogoErrorLabel.ClientID %>');
        errorText.innerHTML = "";
        if (showEdit == '1') {
            readPanel.style.display = "none";
            editPanel.style.display = "";
        }
        else {
            readPanel.style.display = "";
            editPanel.style.display = "none";
        }
    }
    function hideButton() {
        var hideBtn = document.getElementById("EditButton");
        hideBtn.style.visibility = "hidden";
    }
</script>

<asp:UpdatePanel ID="PartnerLogoUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:Label ID="PartnerLogoErrorLabel" runat="server" CssClass="b24-errortext"></asp:Label></div>
        <div id="ReadOnlyPanel">
            <table border="0" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="LogoImage" runat="server" height="60" width="60" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="PartnerLogoName" runat="server" Text="Click on Edit button to upload a image"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="EditButton" type="button" value="Edit" onclick="javascript:toggleView('1');" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="EditViewPanel" style="display: none">
            <table border="0" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:FileUpload ID="PartLogoFileUpload" runat="server" ToolTip="Click Browse to Upload a File"
                            TabIndex="1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="UpdatePartnerLogoButton" runat="server" OnClick="UpdatePartnerLogo_Click"
                            TabIndex="2" Text="Update" /><input id="CancelButton" type="button" value="Cancel"
                                onclick="javascript:toggleView('0');" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="UpdatePartnerLogoButton" />
    </Triggers>
</asp:UpdatePanel>
