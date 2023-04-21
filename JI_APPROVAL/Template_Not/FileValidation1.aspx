<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FileValidation1.aspx.vb" Inherits="RMS_APPROVAL.FileValidation1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat="server">
    <title>Telerik ASP.NET Example</title>
    <link rel="stylesheet" type="text/css" href="Content/style1.css" />
    <script type ="text/javascript" src="Scripts/script1.js"></script>
</head>
 
<body>

    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <div class="demo-container size-medium">
        <div class="qsf-demo-canvas">
            <h2>Upload your images</h2>
            <ul class="qsf-list">
                <li>
                    <strong>Allowed file types:</strong> jpg, jpeg, png, gif (client-side validation).
                </li>
                <li>
                    <strong>Allowed file size:</strong> 500 KB (client-side validation).
                </li>
                <li>
                    <strong>Allowed overall upload size:</strong> 1 MB (server-side validation).
                </li>
            </ul>
 
            <telerik:RadAsyncUpload 
                RenderMode="Lightweight" 
                runat="server" 
                ID="RadAsyncUpload1" 
                AllowedFileExtensions="jpg,jpeg,png,gif" 
                TargetFolder="" 
                MultipleFileSelection="Automatic" 
                MaxFileSize="524288" 
                Skin="Silk" 
                OnFileUploaded="RadAsyncUpload1_FileUploaded" 
                OnClientValidationFailed="validationFailed" 
                UploadedFilesRendering="BelowFileInput">
            </telerik:RadAsyncUpload>
 
            <div class="qsf-results">
                <telerik:RadButton RenderMode="Lightweight" Skin="Silk" runat="server" ID="BtnSubmit" Text="Validate the uploaded files" ></telerik:RadButton>
 
 
                <asp:Panel ID="ValidFiles" Visible="false" runat="server" CssClass="qsf-success">
                    <h3>You successfully uploaded:</h3>
                    <ul class="qsf-list" runat="server" id="ValidFilesList"></ul>
                </asp:Panel>
 
                <asp:Panel ID="InvalidFiles" Visible="false" runat="server" CssClass="qsf-error">
                    <h3>The Upload failed for:</h3>
                    <ul class="qsf-list ruError" runat="server" id="InValidFilesList">
                        <li>
                            <p class="ruErrorMessage">The size of your overall upload exceeded the maximum of 1 MB</p>
                        </li>
                    </ul>
 
                </asp:Panel>
                <telerik:RadButton RenderMode="Lightweight" Skin="Silk" ID="RefreshButton" runat="server" OnClick="RefreshButton_Click" Visible="false" Text="Back" ></telerik:RadButton>
            </div>
 
            <div class="qsf-decoration"></div>
        </div>
        <script type="text/javascript">
            //<![CDATA[
            Sys.Application.add_load(function () {
                demo.initialize();
            });
            //]]>
        </script>
    </div>
</form>
</body>
</html>
