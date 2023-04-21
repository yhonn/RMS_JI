<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_parEdit.aspx.vb" Inherits="RMS_APPROVAL.frm_parEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .RadUpload_Office2007 .ruStyled .ruFileInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 input.ruFakeInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 .ruButton{
            border: 1px solid #e5e5e5;
            color: #767676;
            background-color: #fff;
            background-image:none
        }
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Registrar facturación PAR</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <asp:HiddenField runat="server" ID="idPar" Value="0" />
                            <asp:HiddenField runat="server" ID="esEdicion" Value="0" />
                            <asp:HiddenField runat="server" ID="esCancelado" Value="0" />
                            <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />
                            <asp:HiddenField runat="server" ID="id_documento" Value="0" />
                             <asp:HiddenField runat="server" ID="habilitar_registro" Value="0" />
                            <telerik:RadNumericTextBox ID="txt_cantidad_productos" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                      <div class="form-group row" runat="server" visible="false" id="grdRutaViaje">
                            <div class="col-sm-12 text-left">
                                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" 
                                    AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0"
                                    GridLines="None" Width="100%" ShowHeader="True">
                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset">
                                        <WebServiceSettings>
                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                        </WebServiceSettings>
                                    </HeaderContextMenu>
                                    <MasterTableView >
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column"
                                                HeaderText="Id" SortExpression="orden" UniqueName="orden">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column"
                                                HeaderText="Rol" SortExpression="nombre_rol" UniqueName="nombre_rol">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column"
                                                HeaderText="Usuario" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"
                                                HeaderText="Estado" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_aprobacion" 
                                                FilterControlAltText="Filter fecha_aprobacion column" 
                                                HeaderText="Fecha de aprobación" SortExpression="fecha_aprobacion" 
                                                UniqueName="fecha_aprobacion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Alerta" FilterControlAltText="Filter Alerta column"
                                                HeaderText="Alert" SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                        ToolTip="Indicador Incompleto">
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column" UniqueName="EditCommandColumn1">
                                            </EditColumn>
                                        </EditFormSettings>
                                    </MasterTableView>
                                    <ClientSettings AllowDragToGroup="True" EnableRowHoverStyle="True">
                                        <Selecting AllowRowSelect="True" />
                                    </ClientSettings>
                                    <FilterMenu EnableImageSprites="False">
                                        <WebServiceSettings>
                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                        </WebServiceSettings>
                                    </FilterMenu>
                                </telerik:RadGrid>
                                <hr />
                            </div>    
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-6">
                                <asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Usuario solicita:</asp:Label>
                                <asp:Label runat="server" ID="lblt_usuario_solicita" CssClass="control-label text-bold"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha de solicitud</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_solicitud" AutoPostBack="true" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_solicitud" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Tasa SER</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_tasa_ser" enabled="false" runat="server"  Width="90%" MinValue="0">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_tasa_ser" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6" visible="false" runat="server" id="errorTasaSer">
                                <div class="alert alert-danger" role="alert">
                                    La Tasa SER para la fecha seleccionada no se encuantra registrada, contactar con finanzas
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="id_tasa_ser" Value="0" />
                        <asp:HiddenField runat="server" ID="id_cargo" Value="0" />
                        <div runat="server" visible="false" id="continuarPar">
                             <div class="form-group row" runat="server" visible="false" id="subRegionVisible">
                               <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_sub_Region" AutoPostBack="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server"
                                                ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                           </div>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Fecha en la que se requieren los servicios</asp:Label>
                                    <br />
                                    <telerik:RadDatePicker ID="dt_fecha_requiere_servicios" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                ControlToValidate="dt_fecha_requiere_servicios" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div> 
                               
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Proposito del PAR:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_proposito_par" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                ControlToValidate="cmb_proposito_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Departamento de entrega:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_departamento_entrega" Height="200px" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                ControlToValidate="cmb_departamento_entrega" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Municipio de entrega:</asp:Label>
                                    <br />
                                     <telerik:RadComboBox ID="cmb_municipio_entrega" Height="200px" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                                ControlToValidate="cmb_municipio_entrega" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                            
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Tipo de par:</asp:Label>
                                    <br />
                                     <telerik:RadComboBox ID="cmb_tipo_par" emptymessage="Seleccione..." AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="cmb_tipo_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Asociar par a:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_cargo_par" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                ControlToValidate="cmb_cargo_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                               <%-- <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Código de facturación:</asp:Label>
                                    <br />
                                     <telerik:RadTextBox ID="txt_codigo_facturacion" runat="server"  Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>--%>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label33" CssClass="control-label text-bold">Asociado a comunicaciones:</asp:Label>
                                    <br />
                                    <asp:RadioButtonList ID="rbn_comunicaciones" runat="server"
                                        RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                        <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                        <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server"
                                                ControlToValidate="rbn_comunicaciones" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-12">
                                    <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Proposito uso final de los articulos / servicios solicitados</asp:Label>
                                    <br />
                                     <telerik:RadTextBox ID="txt_proposito_servicio" runat="server" Width="98%" TextMode="MultiLine" Rows="3" MaxLength="1000">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server"
                                                ControlToValidate="txt_proposito_servicio" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="form-group row">
                              
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Adjuntos al PAR:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_adjuntos_par" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                ControlToValidate="cmb_adjuntos_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                  <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Soporte:</asp:Label>
                                    <br />
                                    <telerik:RadAsyncUpload 
                                        RenderMode="Lightweight" 
                                        runat="server" 
                                        ID="soporte_adjunto"
                                        OnClientFileUploaded="onClientFileUploaded"
                                        MultipleFileSelection="Automatic" 
                                        Skin="Office2007"
                                        TemporaryFolder="~/Temp" 
                                        PostbackTriggers="btn_guardar,btn_guardar_2"
                                        MaxFileInputsCount="1"
                                        HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                    </telerik:RadAsyncUpload>
                                </div>
                                <div class="col-sm-12 col-md-12 col-lg-6">
                                    <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Descripción de adjuntos</asp:Label>
                                    <br />
                                     <telerik:RadTextBox ID="txt_descripcion_adjuntos" runat="server" TextMode="MultiLine" Rows="3" Width="97%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                             
                             </div>
                            <div id="informacion_evento" runat="server" visible="false">
                                <hr />
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label40" CssClass="control-label text-bold">Información del evento</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                                <%--<div class="form-group row">
                                    
                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Objetivo:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_objetivo" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server"
                                                    ControlToValidate="cmb_objetivo" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label30" CssClass="control-label text-bold">Resultado esperado:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_resultado_esperado" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                    ControlToValidate="cmb_resultado_esperado" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label32" CssClass="control-label text-bold">Componente:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_componente" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                                                    ControlToValidate="cmb_componente" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                
                                </div>--%>
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Fecha de inicio del evento:</asp:Label>
                                        <br />
                                         <telerik:RadDatePicker ID="dt_fecha_inicio_evento" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                    ControlToValidate="dt_fecha_inicio_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Fecha de finalización del evento:</asp:Label>
                                        <br />
                                        <telerik:RadDatePicker ID="dt_fecha_finalizacion_evento" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                    ControlToValidate="dt_fecha_finalizacion_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Tipo de evento:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_tipo_evento" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                    ControlToValidate="cmb_tipo_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Número de horas:</asp:Label>
                                        <br />
                                         <telerik:RadNumericTextBox ID="txt_nro_horas" runat="server"  Width="90%" MaxLength="1000">
                                        </telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                    ControlToValidate="txt_nro_horas" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     
                                
                                </div>
                                <div class="form-group row">
                             
                                   <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Nombre del evento:</asp:Label>
                                        <br />
                                         <telerik:RadTextBox ID="txt_nombre_evento" runat="server"  TextMode="MultiLine" Rows="3"  Width="90%" MaxLength="1000">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                    ControlToValidate="txt_nombre_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                       <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Entidad a cargo del evento:</asp:Label>
                                        <br />
                                         <telerik:RadComboBox ID="cmb_entidad" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                    ControlToValidate="cmb_entidad" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                            <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Evento con recursos apalancados:</asp:Label>
                                            <br />
                                            <asp:RadioButtonList ID="rbn_recursos_apalancados" runat="server"
                                                RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                                <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                                <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                        ControlToValidate="rbn_recursos_apalancados" CssClass="Error" Display="Dynamic"
                                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </div>
                                </div>
                            </div>
                            <div class="form-group row" id="fuentesApalancamiento" runat="server" visible="false">
                             
                                   <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Fuente del recurso apalancado:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_fuente_aporte" AutoPostBack="false" Filter="Contains" runat="server" Width="100%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                    ControlToValidate="cmb_fuente_aporte" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                       <br />
                                       <div class="text-right" style="margin-top:10px;">
                                           <telerik:RadButton ID="btn_agregar_fuente" runat="server" CssClass="btn btn-sm" Text="Agregar fuente" ValidationGroup="3" Width="100px">
                                            </telerik:RadButton>
                                       </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-">
                                        <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Fuentes asociadas:</asp:Label>
                                        <br />
                                        <telerik:RadGrid ID="grd_fuente_aporte" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowGroupPanel="false">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_fuente_aporte" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_aporte"
                                                        FilterControlAltText="Filter id_aporte column"
                                                        SortExpression="id_aporte" UniqueName="id_aporte"
                                                        Visible="False" HeaderText="id_aporte"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="id_par_fuente_aporte"
                                                        FilterControlAltText="Filter id_par_fuente_aporte column"
                                                        SortExpression="id_par_fuente_aporte" UniqueName="id_par_fuente_aporte"
                                                        Visible="false" HeaderText="id_par_fuente_aporte"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                        <HeaderStyle Width="32px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                OnClick="delete_aporte">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_aporte"
                                                        FilterControlAltText="Filter nombre_aporte column"
                                                        HeaderText="Nombre de la fuente" SortExpression="nombre_aporte"
                                                        UniqueName="nombre_aporte">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                   <%-- <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Responsable:</asp:Label>
                                        <br />
                                         <telerik:RadTextBox ID="txt_responsable" runat="server"  Width="90%" MaxLength="1000">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                    ControlToValidate="txt_responsable" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>--%>
                                   
                                </div>
                             <div class="form-group row">
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Items / Servicios requeridos</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                            </div>
                             <%--<div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                    ControlToValidate="txt_cantidad_productos" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">El número de items no puede ser 0</asp:RequiredFieldValidator>
                                </div>
                            </div>--%>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Descripción / Servicio requerido</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_descripcion_concepto" runat="server" Rows="3" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                ControlToValidate="txt_descripcion_concepto" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Unidad de medida:</asp:Label>
                                    <br />

                                     <telerik:RadComboBox ID="cmb_unidad_medida" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="cmb_unidad_medida" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Cantidad</asp:Label>
                                    <br />
                                    <telerik:RadNumericTextBox ID="txt_cantidad" runat="server"  Width="90%" MinValue="1">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                                ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Valor unitario</asp:Label>
                                    <br />
                                    <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server"
                                                ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-8"></div>
                                <div class="col-sm-4 text-right">
                                    <telerik:RadButton ID="btn_agregar_concepto" runat="server" CssClass="btn btn-sm" Text="Agregar item" ValidationGroup="2" Width="100px">
                                    </telerik:RadButton>
                                </div>
                            </div>

                            <div class="form-group row">
                                <hr />
                                <asp:HiddenField runat="server" ID="id_concpeto" Value="0" />
                                <asp:HiddenField runat="server" ID="tipo_delete" Value="0" />
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_par_detalle"
                                                    FilterControlAltText="Filter id_par_detalle column"
                                                    SortExpression="id_par_detalle" UniqueName="id_par_detalle"
                                                    Visible="False" HeaderText="id_par_detalle"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                    <HeaderStyle Width="4%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                            OnClick="delete_concepto">
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="cantidad"
                                                    FilterControlAltText="Filter cantidad column" HeaderStyle-Width="19%"
                                                    HeaderText="Cantidad" SortExpression="cantidad"
                                                    UniqueName="colm_cantidad">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion"
                                                    FilterControlAltText="Filter descripcion column" HeaderStyle-Width="47%"
                                                    HeaderText="Descripción / Servicio requerido" SortExpression="descripcion"
                                                    UniqueName="colm_descripcion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="unidad_medida"
                                                    FilterControlAltText="Filter unidad_medida column" HeaderStyle-Width="47%"
                                                    HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                                    UniqueName="colm_unidad_medida">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor_unitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    DataFormatString="{0:n}" 
                                                    FilterControlAltText="Filter valor_unitario column" HeaderStyle-Width="13%"
                                                    HeaderText="Valor unitario" SortExpression="valor_unitario"
                                                    UniqueName="colm_valor_unitario">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                    FilterControlAltText="Filter valor column" HeaderStyle-Width="19%"
                                                    HeaderText="Valor total" SortExpression="valor"
                                                    UniqueName="colm_valor">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                        
                         
                       
                        <div class="form-group row" id="infoComponente" runat="server" visible="true">
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Componente</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                            </div>
                            <div class="form-group row">
                                 <div class="col-sm-12">
                                      <telerik:RadGrid ID="grd_componente" runat="server"  GridLines="None">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
                                        <%--<MasterTableView AllowAutomaticDeletes="true" AutoGenerateColumns="False" CommandItemDisplay="None" 
                                            GroupsDefaultExpanded="false" EnableGroupsExpandAll="true"
                                            DataKeyNames="id_estructura_marcologico" >--%>
                                          <MasterTableView AllowAutomaticDeletes="true" AutoGenerateColumns="False" CommandItemDisplay="None" 
                                            DataKeyNames="id_estructura_marcologico" >
                                            <Columns>
                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                        HeaderText="" UniqueName="TemplateColumnAnual">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ctrl_id"  AutoPostBack="false" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="15px" />
                                                    </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="id_estructura_marcologico" FilterControlAltText="Filter id_estructura_marcologico column"
                                                    HeaderText="id_estructura_marcologico" SortExpression="id_estructura_marcologico" UniqueName="id_estructura_marcologico"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion_logica" FilterControlAltText="Filter descripcion_logica column"
                                                    HeaderText="Descripción" SortExpression="descripcion_logica" UniqueName="colm_descripcion_logica">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                          <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="-" FieldName="descripcion_logica_padre_3"
                                                            FormatString="" HeaderText="" HeaderValueSeparator="" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldAlias="id_estructura_marcologico_3" FieldName="id_estructura_marcologico_3"
                                                            FormatString="" HeaderText="" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="-" FieldName="descripcion_logica_padre"
                                                            FormatString="" HeaderText="" HeaderValueSeparator="" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldAlias="id_estructura_marcologico_2" FieldName="id_estructura_marcologico_2"
                                                            FormatString="" HeaderText="" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                 </div>
                            </div>
                        </div>
                        
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />

                    <div class="panel panel-default"  runat="server" id="lyHistory" visible="false">
                        
                        <div class="panel-heading" role="tab" id="infoApprobacion" data-toggle="collapse" data-parent="#accordion" href="#collapseApp" aria-expanded="false" aria-controls="collapseApp">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseApp"
                                    aria-expanded="false" aria-controls="collapseApp" runat="server" id="a2">Aprobación
                                </a>
                            </h4>
                        </div>
                        <div id="collapseApp" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoApprobacion">
                            <div class="panel-body">
                                <div class="form-group row">
                                <div class="col-sm-12 text-left">

                                    <%--TAble here--%>
                                    <table class="table table-responsive table-condensed box box-primary ">
                                            <tr class="box box-default ">
                                                <td  class="text-left"  colspan="2">
                                                    <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                                    <div class="box-header">
                                                        <i class="fa fa-history"></i>
                                                        <h3 class="box-title">History</h3>                                              
                                                    </div>
                                        
                                                </td>
                                                </tr>
                                            <tr>
                                                <td  colspan="2" class="text-left">
                                                    <br />                                    
                                                        <%-- <div class="direct-chat-messages">--%>

                                                                    <asp:Repeater ID="rept_msgApproval" runat="server">
                                                                        <ItemTemplate>
                                                                                <div class="direct-chat-msg <%# Eval("align1")%> "  >
                                                                                    <div class="direct-chat-info clearfix">
                                                                                    <i class="fa <%# Eval("fa_icon")%> fa-2x <%# Eval("align2")%> " aria-hidden="true" title="<%# Eval("icon_message")%>"></i>&nbsp;&nbsp;&nbsp; <span class="direct-chat-name  <%# Eval("align2")%> "><%# Eval("empleado")%></span>
                                                                                    <span class="direct-chat-timestamp  <%# Eval("align3") %> "> <%# getFecha(Eval("fecha_comentario"), "m", False) %> <i class="fa fa-clock-o"></i> <%#  getHora(Eval("fecha_comentario")) %> </span>
                                                                                    </div><!-- /.direct-chat-info -->
                                                                                    <img class="direct-chat-img <%# Eval("bColor")%> " src="<%# Eval("userImagen")%>" alt="<%# Eval("empleado")%>"><!-- /.direct-chat-img -->
                                                                                    <div class="direct-chat-text">
                                                                                    <%# Eval("comentario")%>
                                                                                    </div><!-- /.direct-chat-text -->
                                                                                </div><!-- /.direct-chat-msg -->                                                                 
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>                 
                                                        <%--  </div><!--/.direct-chat-messages-->--%>                                      
                                          
                                                </td>
                                            </tr>  
                                    </table>

                            </div>
                        </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lblt_writcomments" runat="server"  CssClass="control-label text-bold"  Text="Comentarios"></asp:Label>
                                        <br />
                                        <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px"  TextMode="MultiLine" Width="100%">
                                        </telerik:RadTextBox>         
                                    </div>
                                                                                    
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-3 text-center  ">                                           
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_Approved" runat="server" Text="Aprobar"  OnClick="btn_Approved_Click"  OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="72%" />
                                        <asp:Button ID="btn_Completed" runat="server" Text="Aprobar" OnClick="btn_Completed_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-info" /> 
                                    </div>
                                    <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_STandBy" runat="server" Text="Solicitar ajustes" OnClick="btn_STandBy_Click"   OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-warning"  Width="65%" />                                            
                                    </div>
                                    <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                            <asp:Button ID="btn_NotApproved" runat="server" Text="No aprobar"  OnClick="btn_NotApproved_Click"   OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn-lg btn-danger"  Width="65%" />                                                                                        
                                    </div>

<%--                                            <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_Cancelled" runat="server" Text="Cancelar"  OnClick="btn_Cancelled_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-danger" Width="65%" />                                                  
                                    </div>        --%>                                    
                                </div>
                                
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12 text-center">                                              
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                            <br /><br />
                        </div>
                    </div>
                    <div class="form-group row" runat="server" visible="false" id="alerta_dias">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="alert alert-danger" role="alert">
                                <asp:Label ID="lbl_alerta_solicitud_par" runat="server" Font-Names="Arial" Visible="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar_2" runat="server" Text="Guardar y enviar por aprobación" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
                </div>
                <div class="modal fade" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog  modal-dialog-centered">
                            <div class="modal-content modal-lg ">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar concepto</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Delete" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script type="text/javascript">
        window.onClientFileUploaded = function (radAsyncUpload, args) {
          
        }
    </script>
</asp:Content>