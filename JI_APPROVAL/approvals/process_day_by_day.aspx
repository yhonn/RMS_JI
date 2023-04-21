<%@ Page Language="VB" AutoEventWireup="false" Inherits="RMS_APPROVAL.Aprobaciones_process_day_by_day" Codebehind="process_day_by_day.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
        <div>    
            <asp:Label ID="lblResult" runat="server" Text="Ejecutando..."></asp:Label>    
        </div>
    <br />
    <asp:Panel ID="pnResult" runat="server" Height="42px">
    </asp:Panel>
    <br />
    <asp:Label ID="lblResult2" runat="server"></asp:Label>
    
    </form>
</body>
</html>
