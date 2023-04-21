
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="imageUploadTEST.aspx.vb" Inherits="RMS_APPROVAL.imageUploadTEST" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat="server">
    <title>Telerik ASP.NET Example</title>
    <%--<link href="../../common/styles.css" rel="stylesheet" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <link href="Content/styles.css" rel="stylesheet" />
    <script src="Scripts/scripts.js" type="text/javascript"></script>
</head>

    <body>
              
   <%-- <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <link href="styles.css" rel="stylesheet" />
    <script src="scripts.js" type="text/javascript"></script>--%>

  <form id="form1" runat="server">
   <div>

       <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
       <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnablePageHeadUpdate="false">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadImageEditor1" />
                        <telerik:AjaxUpdatedControl ControlID="AsyncUpload1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <div id="dwndWrapper">
            <telerik:RadAsyncUpload 
                RenderMode="Lightweight" 
                ID="AsyncUpload1" 
                runat="server"
                OnClientFilesUploaded="OnClientFilesUploaded" 
                OnFileUploaded="AsyncUpload1_FileUploaded"
                MaxFileSize="2097152" 
                AllowedFileExtensions="jpg,png,gif,bmp" 
                AutoAddFileInputs="false" 
                Localization-Select="Upload Image" />
            <asp:Label ID="Label1" Text="*Size limit: 2MB" runat="server" Style="font-size: 10px;"></asp:Label>
        </div>

        <telerik:RadImageEditor RenderMode="Lightweight" ID="RadImageEditor1" runat="server" Width="790" Height="450" 
                                ImageUrl="~/ImageEditor/images/waterpool.jpg" OnImageLoading="RadImageEditor1_ImageLoading">
        </telerik:RadImageEditor>
    </div>

      <script type="text/javascript">
        //<![CDATA[
        serverID("ajaxManagerID", "<%= RadAjaxManager1.ClientID %>");
        //]]>
    </script>

 </form>
 <%--</asp:Content>--%>
        </body>
</html>