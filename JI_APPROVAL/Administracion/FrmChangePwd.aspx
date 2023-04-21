<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FrmChangePwd.aspx.vb" Inherits="RMS_APPROVAL.FrmChangePwd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Contraseña - Edición</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_nombre" runat="server" CssClass="control-label text-bold" Text="Primer Nombre"></asp:Label>
                            </div>
                            <asp:Label ID="ID_ProgSemanal" runat="server" Font-Bold="True"
                                Font-Names="Arial" Font-Size="X-Small"
                                ForeColor="#C00000" Width="280px" Visible="False">-1</asp:Label>
                            <div class="col-sm-8">
                                <asp:Label ID="txtnomb1" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_apellido" runat="server" CssClass="control-label text-bold" Text="Apellido"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="txtapell" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_email" runat="server" CssClass="control-label text-bold" Text="Email"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="txt_email" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_usuario" runat="server" CssClass="control-label text-bold" Text="Usuario en el sistema"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="txtUsu" runat="server">Username </asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_seleccionar_idioma" runat="server" CssClass="control-label text-bold" Text="Idioma preferido"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox runat="server" ID="cmb_idioma" Width="300px">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_contrasena" runat="server" CssClass="control-label text-bold" Text="Nueva Contraseña"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtpass" runat="server" TextMode="Password" Width="300px" Enabled="False">
                                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                </telerik:RadTextBox>
                                <asp:Label ID="lblerr_pass" runat="server"
                                    Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000" Visible="False">*</asp:Label>


                                <asp:CheckBox ID="chk_cambiar" runat="server"
                                    AutoPostBack="True" Text="Cambiar claves" />

                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_confirmar" runat="server" CssClass="control-label text-bold" Text="Confirmar Contraseña"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txtpassConfirm" runat="server" TextMode="Password" Width="300px" Enabled="False">
                                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                </telerik:RadTextBox>
                                <asp:Label ID="lblerr_passconf" runat="server"
                                    Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000" Visible="False">* Las claves no coinciden</asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_imagen" runat="server" CssClass="control-label text-bold" Text="Imagen"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadAsyncUpload runat="server" RenderMode="Lightweight" ID="uploadFile"
                                    Skin="Metro" TemporaryFolder="~/Temp" TargetFolder="~/Temp" HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx"
                                    OnClientFileUploaded="fileUploaded" OnClientFileUploadRemoving="fileUploadRemoving" MaxFileInputsCount="1"
                                    MultipleFileSelection="Disabled" AllowedFileExtensions="jpeg,jpg,gif,png,bmp" PostbackTriggers="">
                                </telerik:RadAsyncUpload>


                                <div class="imageContainer"></div>
                                <script src="../Scripts/FileUploadTelerik.js"></script>
                                <script type="text/javascript">
                                    //Sys.Application.add_load(function () {
                                    //    demo.initialize();
                                    //});
                                </script>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                
                            </div>
                            <div class="col-sm-8">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                        <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                        <asp:HiddenField runat="server" ID="lbl_oldFile" />
                                        <script>
                                            function changeUpload(fileUploaded)
                                            {
                                                document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;
                                                var img = document.getElementById("<%= imgUser.ClientID%>");
                                                img.className = "hidden";
                                            }

                                            function hasFiles(valor)
                                            {
                                                document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                            }
                                        </script>
                                        <asp:Image ID="imgUser" runat="server" BackColor="#CCCCCC" BorderColor="Black"
                                            BorderStyle="Double" Height="150px" ImageUrl="~/Imagenes/Logo_User.png"
                                            Width="150px" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" runat="server" CssClass="btn btn-sm pull-right margin-r-5"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label ID="lblerrorG" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="X-Small"
                            ForeColor="#C00000" Visible="False">* Complete campos requeridos</asp:Label>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>

