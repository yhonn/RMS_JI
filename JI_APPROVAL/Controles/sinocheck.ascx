<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="sinocheck.ascx.vb" Inherits="ACS_SIME.sinocheck" %>
<telerik:RadButton ID="btnToggleSI" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
</telerik:RadButton>
<telerik:RadButton ID="btnToggleNO" runat="server" ToggleType="Radio" ButtonType="StandardButton" GroupName="StandardButton">
</telerik:RadButton>
<asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ErrorMessage="*" ValidationGroup="1" CssClass="Error"
    ClientValidationFunction="CustomValidator1_ClientValidate" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>

<script language="javascript" type="text/javascript">
    function CustomValidator1_ClientValidate(source, args) {
        if (document.getElementById("<%= btnToggleSI.ClientID%>").classList.contains("rbSkinnedButtonChecked")
            || document.getElementById("<%= btnToggleNO.ClientID %>").classList.contains("rbSkinnedButtonChecked")) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
        var idSI = "ctl00_" + source.id.toString().replace("CustomValidator1", "") + "btnToggleSI";
        var idNO = "ctl00_" + source.id.toString().replace("CustomValidator1", "") + "btnToggleNO";
        if (document.getElementById(idSI).classList.contains("rbSkinnedButtonChecked")
            || document.getElementById(idNO).classList.contains("rbSkinnedButtonChecked")) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }
    //-->
</script>
