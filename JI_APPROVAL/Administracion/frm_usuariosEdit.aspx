<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_usuariosEdit.aspx.vb" Inherits="RMS_APPROVAL.frm_usuariosEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Usuarios - Editar</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lbl_id_usuario" runat="server" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000"
                                    Visible="False"></asp:Label>
                                <asp:Label ID="lblt_codigo_usuario" runat="server" CssClass="control-label text-bold" Text="Código Usuario"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_job_title" 
                                    runat="server"
                                    EmptyMessage="Select a job tittle..."   
                                    AllowCustomText="true" 
                                    Filter="Contains"                                      
                                    Width="250px" 
                                    MaxLength="250">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="cmb_job_title" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                <asp:Label runat="server" ID="lbl_JobErr" CssClass="Error" Visible="false" Text="**Select a valid Job Title"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_nombre_usuario" runat="server" CssClass="control-label text-bold" Text="Nombre Usuario"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_nombreUsuario" runat="server" Rows="3" TextMode="SingleLine" Width="250px" MaxLength="250">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txt_nombreUsuario" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_apellido_usuario" runat="server" CssClass="control-label text-bold" Text="Apellido Usuario"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_apellidos" runat="server" Rows="3" Width="250px"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txt_apellidos" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_email" runat="server" CssClass="control-label text-bold" Text="Email"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_email_usuario" runat="server" MaxLength="100" Width="250px">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                    ControlToValidate="txt_email_usuario" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txt_email_usuario" ErrorMessage="* Error en Email"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="1"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="Label1" runat="server" CssClass="control-label text-bold" Text="Número de documento"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="txt_numero_documento" runat="server" Rows="3" Width="250px"
                                    MaxLength="50">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                    ControlToValidate="txt_numero_documento" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="Label2" runat="server" CssClass="control-label text-bold" Text="Código"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_codigo" runat="server" Rows="3" Width="250px"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                    ControlToValidate="txt_numero_documento" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_usuario_sistema" runat="server" CssClass="control-label text-bold" Text="Usuario en el Sistema"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_usuarioSistema" runat="server" Rows="3" Width="250px" Text="" MaxLength="50" AutoPostBack="true">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txt_usuarioSistema" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                <asp:Label runat="server" ID="lblErrorUsuario" CssClass="Error" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_contrasena" runat="server" CssClass="control-label text-bold" Text="Contraseña"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_clave_usuario" runat="server" Rows="3" Width="250px" Text="" TextMode="Password"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="pass_validator" runat="server"
                                    ControlToValidate="txt_clave_usuario" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="2">Password Required</asp:RequiredFieldValidator>
                                <asp:CheckBox ID="chk_PasswordChange" runat="server" AutoPostBack="True" Text="Change Password" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_supervisor" runat="server" CssClass="control-label text-bold" Text="Supervisor"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox 
                                    ID="cmb_supervisor" 
                                    runat="server" 
                                    EmptyMessage="Select a user..."   
                                    AllowCustomText="true" 
                                    Filter="Contains"  
                                    Width="300px">
                                </telerik:RadComboBox>
                                <asp:Label runat="server" ID="lbl_supervisorErr" CssClass="Error" Visible="false" Text="**Select a supervisor"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="Error" InitialValue=""
                                    ControlToValidate="cmb_supervisor" ErrorMessage="required"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>                                 
                                <asp:CheckBox ID="chk_SupervisorNA" runat="server" Text="Do Not Apply" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_fecha_contrato" runat="server" CssClass="control-label text-bold" Text="Fecha de Contrato"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadDatePicker ID="dt_fecha_contrato" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                    ControlToValidate="dt_fecha_contrato" ErrorMessage="*"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row" runat="server" id="div_nivel_usuario">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_nivel_usuario" runat="server" CssClass="control-label text-bold" Text="Nivel Usuario"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_tipo_nivel" runat="server" Width="300px">
                                </telerik:RadComboBox>
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
                                <asp:Label ID="lblt_tipo_usuario" runat="server" CssClass="control-label text-bold" Text="Tipo Usuario"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_tipo_usuario" runat="server" Width="300px" AutoPostBack="true">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row" id="divEjecutor" runat="server" visible="false">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_ejecutor" runat="server" CssClass="control-label text-bold" Text="Implementers"></asp:Label>
                            </div>
                            <div class="col-sm-8">

                                 <telerik:RadComboBox ID="cmb_ejecutor" runat="server" Width="300px" Filter="Contains">
                                </telerik:RadComboBox>

                               <%-- <telerik:RadGrid ID="grd_ejecutores" runat="server" AutoGenerateColumns="False" Width="300px">
                                    <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_ejecutor">
                                        <Columns>
                                            <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                HeaderText="" UniqueName="TemplateColumnAnual">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ctrl_id_ejecutor" runat="server"/>
                                                </ItemTemplate>
                                                <HeaderStyle Width="15px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="id_ejecutor" DataType="System.Int32"
                                                FilterControlAltText="Filter id_ejecutor column" HeaderText="id_ejecutor"
                                                ReadOnly="True" SortExpression="id_ejecutor" UniqueName="id_ejecutor"
                                                Display="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_ejecutor"
                                                FilterControlAltText="Filter nombre_ejecutor column"
                                                HeaderText="Ejecutor" SortExpression="nombre_ejecutor"
                                                UniqueName="colm_nombre_ejecutor" ItemStyle-Width="90%">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>--%>

                            </div>
                        </div>
                <%--        <div class="form-group row" id="divEjecutor2" runat="server" visible="false">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_ejecutor2" runat="server" CssClass="control-label text-bold" Text="Ejecutor"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                               
                            </div>
                        </div>--%>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="Label3" runat="server" CssClass="control-label text-bold" Text="Filtro busqueda de viajes"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox 
                                     ID="cmb_filtro_viajes" 
                                    runat="server" 
                                    EmptyMessage="Seleccione..."   
                                    AllowCustomText="true" 
                                    Filter="Contains"    
                                    Width="300px">
                                </telerik:RadComboBox>
                                <asp:Label runat="server" ID="Label4" CssClass="Error" Visible="false" Text="**Select a supervisor"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" CssClass="Error" InitialValue=""
                                    ControlToValidate="cmb_filtro_viajes" ErrorMessage="required"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>                                 
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_imagen" runat="server" CssClass="control-label text-bold" Text="Imagen"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadAsyncUpload 
                                    runat="server" 
                                    RenderMode="Lightweight" 
                                    ID="uploadFile"
                                    Skin="Metro" 
                                    TemporaryFolder="~/Temp" 
                                    TargetFolder="~/Temp" 
                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx"
                                    OnClientFileUploaded="fileUploaded" 
                                    OnClientFileUploadRemoving="fileUploadRemoving" 
                                    MaxFileInputsCount="1"
                                    MultipleFileSelection="Disabled" 
                                    AllowedFileExtensions="jpeg,jpg,gif,png,bmp" 
                                    PostbackTriggers="">
                                </telerik:RadAsyncUpload>
                                <%--<div class="imageContainer"></div>--%>
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
                                            function changeUpload(fileUploaded) {

                                                document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;
                                                var img = document.getElementById("<%= imgUser.ClientID%>");
                                                //img.className = "hidden";
                                                document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;
                                            }

                                            function hasFiles(valor) {
                                                document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                            }
                                        </script>
                                        <asp:Image ID="imgUser" runat="server" BackColor="#CCCCCC" BorderColor="Black"
                                            BorderStyle="Double" ImageUrl="~/Imagenes/Logo_User.png"
                                            style="max-height:150px" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_estado" runat="server" CssClass="control-label text-bold" Text="Estado"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList runat="server" ID="rb_estado" Width="150"></asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="rb_estado" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row" runat="server" >
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_actividades_habilitadas" runat="server" CssClass="control-label text-bold" Text="Filtro por actividades"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="rbn_actividades_habilitadas" runat="server" RepeatDirection="Horizontal" Width="150">
                                    <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVE" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                    ControlToValidate="rbn_actividades_habilitadas" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <div class="form-group row" runat="server" >
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="Label5" runat="server" CssClass="control-label text-bold" Text="Tipo"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="rbn_tipo" runat="server" RepeatDirection="Horizontal" Width="150">
                                    <asp:ListItem Text="Operativo" Value="Operativo"></asp:ListItem>
                                    <asp:ListItem Text="Técnico" Value="Técnico"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="rv_rbn_tipo" runat="server"
                                    ControlToValidate="rbn_tipo" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_ultima_fecha" runat="server" CssClass="control-label text-bold" Text="Ultima Fecha de Modificación"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_date_upd"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_ultimo_usuario" runat="server" CssClass="control-label text-bold" Text="Ultimo Usuario Modificación"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_id_usrupdate"></asp:Label>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <asp:CheckBox ID="chk_Notify"  runat="server" Text="Notify the user" CssClass="btn btn-sm pull-right " />
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                                Width="100px" ValidationGroup="1">
                            </telerik:RadButton>
                            <asp:Label runat="server" ID="lbl_mensajeError" CssClass="Error pull-right margin-r-5"></asp:Label>
                        </div>
                    </div>
                    <!-- /.box-footer -->
                </div>
            </div>
        </div>
    </section>
</asp:Content>
