<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viaje_legalizacion.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_legalizacion" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Legalización de viaje</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
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
                                            <telerik:GridTemplateColumn UniqueName="CompletoC" visible="false" FilterControlAltText="Filter Completo column">
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
                       <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                           <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="infoViaje" data-toggle="collapse" data-parent="#accordion" href="#collapseViaje" aria-expanded="false" aria-controls="collapseViaje">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseViaje"
                                            aria-expanded="false" aria-controls="collapseViaje" runat="server" id="a1">Información del viaje
                                        </a>
                                    </h4>
                                </div>
                                 <div id="collapseViaje" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoViaje">
                                    <div class="panel-body">
                                        <asp:HiddenField runat="server" ID="idViaje" Value="0" />
                                         <asp:HiddenField runat="server" ID="esEdicion" Value="0" />
                                        <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />
                                        
                                        <div class ="form-group row">
                                                <div class="col-sm-12 text-left ">
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Tipo de aprobación"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Código de aprobación"></asp:Label>                                
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Fecha de solicitud"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_solicitud" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_owner" runat="server"  CssClass="control-label text-bold"  Text="Fecha de inicio del viaje"></asp:Label></div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_inicio_viaje" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_NextApp" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización del viaje"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_finalizacion" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Numero de contacto"></asp:Label>                                                        
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_numero_contacto" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_proccess_name" runat="server" CssClass="control-label text-bold" Text="Motivo del viaje"></asp:Label> 
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_motivo_viaje" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>                                                              
                                                    
                                                </div> 
                                            </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Detalle itinerario</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_itinerario" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="false" 
                                                    ShowColumnFooters="false"
                                                    ShowGroupFooters="false"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_itinerario" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_viaje_itinerario"
                                                                FilterControlAltText="Filter id_viaje_itinerario column"
                                                                SortExpression="id_viaje_itinerario" UniqueName="id_viaje_itinerario"
                                                                Visible="False" HeaderText="id_viaje_itinerario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_viaje" DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_viaje column" HeaderStyle-Width="19%"
                                                                HeaderText="Fecha" SortExpression="fecha_viaje"
                                                                UniqueName="colm_fecha_viaje">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hora_salida"
                                                                FilterControlAltText="Filter hora_salida column" HeaderStyle-Width="19%"
                                                                HeaderText="Hora de salida" SortExpression="hora_salida"
                                                                UniqueName="colm_hora_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_origen" 
                                                                FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="19%"
                                                                HeaderText="Ciudad de origen" SortExpression="ciudad_origen"
                                                                UniqueName="colm_ciudad_origen">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_destino"
                                                                FilterControlAltText="Filter ciudad_destino column" HeaderStyle-Width="19%"
                                                                HeaderText="Ciudad de destino" SortExpression="ciudad_destino"
                                                                UniqueName="colm_ciudad_destino">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="requiere_transporte_aereo_text"
                                                                FilterControlAltText="Filter requiere_transporte_aereo_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Transporte aéreo" SortExpression="requiere_transporte_aereo_text"
                                                                UniqueName="colm_requiere_transporte_aereo_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_vehiculo_proyecto_text"
                                                                FilterControlAltText="Filter requiere_vehiculo_proyecto_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Vehiculo del proyecto" SortExpression="requiere_vehiculo_proyecto_text"
                                                                UniqueName="colm_requiere_vehiculo_proyecto_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_transporte_fluvial_text" 
                                                                FilterControlAltText="Filter requiere_transporte_fluvial_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Transporte fluvial" SortExpression="requiere_transporte_fluvial_text"
                                                                UniqueName="colm_requiere_transporte_fluvial_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_servicio_publico_text"
                                                                FilterControlAltText="Filter requiere_servicio_publico_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Servicio público" SortExpression="requiere_servicio_publico_text"
                                                                UniqueName="colm_requiere_servicio_publico_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Detalle del alojamiento</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_alojamiento" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="false" 
                                                    ShowColumnFooters="false"
                                                    ShowGroupFooters="false"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_hotel" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_viaje_hotel"
                                                                FilterControlAltText="Filter id_viaje_hotel column"
                                                                SortExpression="id_viaje_hotel" UniqueName="id_viaje_hotel"
                                                                Visible="False" HeaderText="id_viaje_hotel"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_llegada"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_llegada column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha de llegada" SortExpression="fecha_llegada"
                                                                UniqueName="colm_fecha_llegada">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_salida"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_salida column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha de salida" SortExpression="fecha_salida"
                                                                UniqueName="colm_fecha_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad"
                                                                FilterControlAltText="Filter ciudad column" HeaderStyle-Width="23%"
                                                                HeaderText="Ciudad" SortExpression="ciudad"
                                                                UniqueName="colm_ciudad">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hotel"
                                                                FilterControlAltText="Filter hotel column" HeaderStyle-Width="23%"
                                                                HeaderText="Hotel" SortExpression="hotel"
                                                                UniqueName="colm_hotel">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            
                                        </div>
                                    </div>

                                 </div>
                           </div>
                           <div class="panel panel-default">
                               <div class="panel-heading" role="tab" id="infoLegalizacion" data-toggle="collapse" data-parent="#accordion" href="#collapseLegalizacion" aria-expanded="false" aria-controls="collapseLegalizacion">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseLegalizacion"
                                            aria-expanded="false" aria-controls="collapseLegalizacion" runat="server" id="a2">Legalización del viaje
                                        </a>
                                    </h4>
                                </div>
                               <div id="collapseLegalizacion" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoLegalizacion">
                                    <div class="panel-body">
                                        <div class="form-group row">
                                            <div class="col-sm-6 col-md-6 col-lg-4">
                                                <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Fecha y hora de inicio real del viaje</asp:Label>
                                                <br />
                                                <telerik:RadDatePicker ID="dt_fecha_inicio" AutoPostBack="true" Width="60%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <telerik:RadTimePicker runat="server" ID="rt_hora_inicio" Width="30%"  AutoPostBack="true" DateInput-ReadOnly="true">
                                                </telerik:RadTimePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                            ControlToValidate="dt_fecha_inicio" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                            ControlToValidate="rt_hora_inicio" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                               <%-- <telerik:RadDateTimePicker  ID="dt_fecha_inicio_real_viaje" AutoPostBack="true" Width="90%" Enabled="true" runat="server">

                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDateTimePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                            ControlToValidate="dt_fecha" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-4">
                                                <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Fecha y hora de finalización real del viaje</asp:Label>
                                                <br />
                                                <telerik:RadDatePicker ID="dt_fecha_fin" AutoPostBack="true" Width="60%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <telerik:RadTimePicker runat="server" ID="rt_hora_fin"  AutoPostBack="true" Width="30%"  DateInput-ReadOnly="true">
                                                </telerik:RadTimePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                            ControlToValidate="dt_fecha_fin" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                            ControlToValidate="rt_hora_fin" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Duración del viaje</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_horas_diferencia" enabled="false" runat="server"  Width="90%" MinValue="0">
                                                </telerik:RadTextBox>
                                                <asp:hiddenfield runat="server" id="numero_horas_viaje" value="0" />
                                                <asp:hiddenfield runat="server" id="numero_dias" value="0" />
                                            </div>
                                            
                                        </div>
                                         <div class="form-group row">
                                                <div class="col-sm-6 col-md-6 col-lg-4">
                                                    <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha radicación legalización</asp:Label>
                                                    <br />
                                                    <telerik:RadDatePicker ID="dt_fecha" AutoPostBack="true" Width="90%" Enabled="true" runat="server">
                                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                ControlToValidate="dt_fecha" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-4">
                                                    <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Tasa SER</asp:Label>
                                                    <br />
                                                    <telerik:RadNumericTextBox ID="txt_tasa_ser" enabled="false" runat="server"  Width="90%" MinValue="0">
                                                    </telerik:RadNumericTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                                ControlToValidate="txt_tasa_ser" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="info_soporte_tiquete">
                                                    <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Soporte de tiquetes</asp:Label>
                                                        <br />
                                                        <telerik:RadAsyncUpload 
                                                            RenderMode="Lightweight" 
                                                            runat="server" 
                                                            ID="soporte_tiquete"
                                                            OnClientFileUploaded="onClientFileUploaded"
                                                            MultipleFileSelection="Automatic" 
                                                            Skin="Office2007"
                                                            TemporaryFolder="~/Temp" 
                                                            PostbackTriggers="btn_enviar_aprobacion,btn_Approved,btn_continuar"
                                                            MaxFileInputsCount="8"
                                                            data-clientFilter="application/pdf"
                                                            AllowedFileExtensions="pdf"
                                                            HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                        </telerik:RadAsyncUpload>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                        ControlToValidate="soporte_tiquete_val" Minimumvalue="1" CssClass="Error" Display="Dynamic"
                                                        ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                        <div class="hidden">
                                                                <telerik:RadNumericTextBox ID="soporte_tiquete_val" runat="server"  Width="90%">
                                                                </telerik:RadNumericTextBox>
                                                                <telerik:RadNumericTextBox ID="requiere_soporte_tiquete" runat="server"  Width="90%" value="0">
                                                                </telerik:RadNumericTextBox>
                                                        </div>
                                                </div>
                                               <div class="col-sm-6 col-md-6 col-lg-4" visible="false" runat="server" id="errorTasaSer">
                                                    <div class="alert alert-danger" role="alert">
                                                        La Tasa SER para la fecha seleccionada no se encuantra registrada, contactar con finanzas
                                                    </div>
                                                </div>
                                                 
                                            </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-12" style="text-align:right;">
                                                <telerik:RadButton ID="btn_continuar" style="margin-top:10px;" runat="server" Text="Continuar" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                    ValidationGroup="2" CausesValidation="true">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <hr />
                                        </div>
                                        <div runat="server" visible="false" id="continuar_legalizacion">
                                            
                                             <div class="form-group row">
                                                 <hr />
                                             </div>
                                            <div>
                                                <div class="form-group row">
                                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Código de facturación:</asp:Label>
                                                        <br />
                                                        <telerik:RadComboBox ID="cmb_codigo_facturacion" AutoPostBack="false" emptymessage="Seleccione ..." Filter="Contains" runat="server" Width="90%">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                                    ControlToValidate="cmb_codigo_facturacion" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Categoría item de legalización:</asp:Label>
                                                        <br />
                                                        <telerik:RadComboBox ID="cmb_tipo_legalizacion" AutoPostBack="true" Filter="Contains"  emptymessage="Seleccione ..." runat="server" Width="90%">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="cmb_tipo_legalizacion" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Rec. #</asp:Label>
                                                        <br />
                                                        <telerik:RadTextBox ID="txt_rec" runat="server"  Width="90%">
                                                        </telerik:RadTextBox>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Fecha adquirio el servicio</asp:Label>
                                                        <br />
                                                        <telerik:RadDatePicker ID="dt_fecha_adquirio_servicio" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                    ControlToValidate="dt_fecha_adquirio_servicio" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                 
                                                </div>
                                                <div class="form-group row">
                                                     <div class="col-sm-12 col-md-12 col-lg-12">
                                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Observaciones / Descripción del gasto</asp:Label>
                                                        <br />
                                                        <telerik:RadTextBox ID="txt_descripcion_gasto" runat="server" Rows="3" TextMode="MultiLine" Width="98%">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                    ControlToValidate="txt_descripcion_gasto" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                             
                                                <div runat="server" visible ="false" id="info_general">
                                                    <div class="form-group row">
                                                    
                                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Código Pasajes:</asp:Label>
                                                            <br />
                                                            <telerik:RadComboBox ID="cmb_codigo_pasajes" AutoPostBack="false" emptymessage="Seleccione ..." Filter="Contains" runat="server" Width="90%">
                                                            </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="rv_codigo_pasajes" runat="server"
                                                                ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Monto:</asp:Label>
                                                            <br />
                                                            <telerik:RadNumericTextBox ID="txt_monto_pasajes" NumberFormat-DecimalDigits="0" AutoPostBack="true" runat="server" value="0" Width="90%" MinValue="0">
                                                                <ClientEvents OnValueChanging="calc_monto_pasajes" />
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                                <asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Soporte: <small>(Obligatorio para valores superiores a $50.000)</small></asp:Label>
                                                                <br />
                                                                <telerik:RadAsyncUpload 
                                                                    RenderMode="Lightweight" 
                                                                    runat="server" 
                                                                    ID="soporte_pasajes"
                                                                    OnClientFileUploaded="onClientFileUploaded"
                                                                    MultipleFileSelection="Automatic" 
                                                                    Skin="Office2007"
                                                                    TemporaryFolder="~/Temp" 
                                                                    PostbackTriggers="btn_guardar"
                                                                    MaxFileInputsCount="8"
                                                                     data-clientFilter="application/pdf"
                                                                    AllowedFileExtensions="pdf"
                                                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                                </telerik:RadAsyncUpload>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                ControlToValidate="soporte_pasajes_val" Minimumvalue="1" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                <div class="hidden">
                                                                        <telerik:RadNumericTextBox ID="soporte_pasajes_val" runat="server"  Width="90%" value="1">
                                                                    </telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                     </div>
                                                </div>
                                                <div runat="server" visible ="false" id="info_reuniones">
                                                    <div class="form-group row">
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                            <asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">El gasto fue para:</asp:Label>
                                                            <br />
                                                            <asp:RadioButtonList ID="rbn_gasto_reunion" runat="server"
                                                                RepeatColumns="2" Style="height: 26px" AutoPostBack="false">
                                                                <asp:ListItem Value="1">Entrenamiento (ENT) &nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="2">Reunión (MTG) &nbsp;</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator ID="rv_gasto_evento_rbn" runat="server"
                                                            ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                            <asp:Label runat="server" ID="Label29" CssClass="control-label text-bold">Número de participantes:</asp:Label>
                                                            <br />
                                                            <telerik:RadNumericTextBox ID="txt_numero_participantes" NumberFormat-DecimalDigits="0" runat="server"  Width="90%" MinValue="0">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                            <asp:Label runat="server" ID="Label30" CssClass="control-label text-bold">Monto:</asp:Label>
                                                            <br />
                                                            <telerik:RadNumericTextBox ID="txt_monto_reuniones" runat="server" NumberFormat-DecimalDigits="0" value="0" Width="90%" MinValue="0">
                                                                 <ClientEvents OnValueChanging="calc_monto_reuniones" />
                                                            </telerik:RadNumericTextBox>
                                                            <asp:RequiredFieldValidator ID="rv_monto_evento" runat="server"
                                                                        ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                                <asp:Label runat="server" ID="Label32" CssClass="control-label text-bold">Soporte:</asp:Label>
                                                                <br />
                                                                <telerik:RadAsyncUpload 
                                                                    RenderMode="Lightweight" 
                                                                    runat="server" 
                                                                    ID="soporte_Reuniones"
                                                                    OnClientFileUploaded="onClientFileUploaded"
                                                                    MultipleFileSelection="Automatic" 
                                                                    Skin="Office2007"
                                                                    TemporaryFolder="~/Temp" 
                                                                    PostbackTriggers="btn_guardar"
                                                                    MaxFileInputsCount="8"
                                                                     data-clientFilter="application/pdf"
                                                                    AllowedFileExtensions="pdf"
                                                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                                </telerik:RadAsyncUpload>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                ControlToValidate="soporte_reuniones_val" Minimumvalue="1" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                <div class="hidden">
                                                                     <telerik:RadNumericTextBox ID="soporte_reuniones_val" runat="server"  Width="90%" value="1">
                                                                    </telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                    </div>
                                                </div>
                                                <div runat="server" visible ="false" id="info_miscelaneos">
                                                    <div class="form-group row">
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                            <asp:Label runat="server" ID="Label35" CssClass="control-label text-bold">Monto:</asp:Label>
                                                            <br />
                                                            <telerik:RadNumericTextBox ID="txt_monto_miscelaneos" NumberFormat-DecimalDigits="0" runat="server" value="0" Width="90%" MinValue="0">
                                                                <ClientEvents OnValueChanging="calc_monto_miscelaneos" />
                                                            </telerik:RadNumericTextBox>
                                                            <asp:RequiredFieldValidator ID="rv_monto_miscelaneos" runat="server"
                                                                        ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-12 col-md-12 col-lg-3">
                                                                <asp:Label runat="server" ID="Label36" CssClass="control-label text-bold">Soporte:</asp:Label>
                                                                <br />
                                                                <telerik:RadAsyncUpload 
                                                                    RenderMode="Lightweight" 
                                                                    runat="server" 
                                                                    ID="soporte_miscelaneos"
                                                                    OnClientFileUploaded="onClientFileUploaded"
                                                                    MultipleFileSelection="Automatic" 
                                                                    Skin="Office2007"
                                                                    TemporaryFolder="~/Temp" 
                                                                    PostbackTriggers="btn_guardar"
                                                                    MaxFileInputsCount="8"
                                                                     data-clientFilter="application/pdf"
                                                                    AllowedFileExtensions="pdf"
                                                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                                </telerik:RadAsyncUpload>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                ControlToValidate="soporte_miscelaneos_val" Minimumvalue="1" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                <div class="hidden">
                                                                     <telerik:RadNumericTextBox ID="soporte_miscelaneos_val" runat="server"  Width="90%" value="1">
                                                                    </telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                    </div>
                                                </div>
                                                <div runat="server" visible ="false" id="info_estadida_alimentacion">
                                                     <div class="form-group row">
                                                         <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Departamento</asp:Label>
                                                            <br />
                                                            <telerik:RadComboBox ID="cmb_departamento" AutoPostBack="true" Height="200px" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Municipio</asp:Label>
                                                            <br />
                                                            <telerik:RadComboBox ID="cmb_municipio" AutoPostBack="true" Height="200px" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                            </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                                    ControlToValidate="cmb_municipio"  Visible="false" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                         <div class="col-sm-6 col-md-6 col-lg-3" runat="server" visible="false" id="zr_data">
                                                            <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">¿Es zona rural?</asp:Label>
                                                            <br />
                                                              <asp:RadioButtonList runat="server" ID="rbn_zona_legalizacion"  runat="server"
                                                                RepeatColumns="3" AutoPostBack="true">
                                                                  <asp:ListItem Value="Rural">Sí &nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="Otras ciudades">No &nbsp;</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                                    ControlToValidate="rbn_zona_legalizacion"  Visible="false" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                         <div class="col-sm-6 col-md-6 col-lg-3" runat="server" visible="false" id="viaje_8">
                                                            <asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">M&E Rate</asp:Label>
                                                            <br />
                                                            <telerik:RadNumericTextBox ID="txt_per_diem_8" NumberFormat-DecimalDigits="0" AutoPostBack="false" enabled="false" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                     </div>
                                                    <div class="form-group row" runat="server" visible="true" id="viaje_9">
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">% Viatico:</asp:Label>
                                                            <br />
                                                            <asp:RadioButtonList ID="rbn_dia" runat="server"
                                                                RepeatColumns="3" AutoPostBack="true">
                                                                <asp:ListItem Value="1">Primer día &nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="2">Día intermedio &nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="3">Ultima día &nbsp;</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator ID="rv_dia_rbn" runat="server"
                                                                    ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">M&E Rate</asp:Label>
                                                                <br />
                                                                <telerik:RadNumericTextBox ID="txt_per_diem" NumberFormat-DecimalDigits="0" enabled="false" runat="server"  Width="90%" MinValue="0">
                                                                </telerik:RadNumericTextBox>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                             <asp:Label runat="server" ID="Label38" CssClass="control-label text-bold">Valor alojamiento por noche</asp:Label>
                                                                <br />
                                                                <telerik:RadNumericTextBox ID="txt_monto_alojamiento" NumberFormat-DecimalDigits="0" runat="server"  Width="90%" MinValue="0">
                                                                    <ClientEvents OnValueChanging="calc_monto_alojamiento" />
                                                                </telerik:RadNumericTextBox>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                              <asp:Label runat="server" ID="Label39" CssClass="control-label text-bold">Soporte:</asp:Label>
                                                                <br />
                                                                <telerik:RadAsyncUpload 
                                                                    RenderMode="Lightweight" 
                                                                    runat="server" 
                                                                    ID="soporte_alojamiento"
                                                                    OnClientFileUploaded="onClientFileUploaded"
                                                                    MultipleFileSelection="Automatic" 
                                                                    Skin="Office2007"
                                                                    TemporaryFolder="~/Temp" 
                                                                    PostbackTriggers="btn_guardar"
                                                                    MaxFileInputsCount="8"
                                                                     data-clientFilter="application/pdf"
                                                                    AllowedFileExtensions="pdf"
                                                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                                </telerik:RadAsyncUpload>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                ControlToValidate="soporte_alojamiento_val" Minimumvalue="1" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                <div class="hidden">
                                                                     <telerik:RadNumericTextBox ID="soporte_alojamiento_val" runat="server"  Width="90%" value="1">
                                                                    </telerik:RadNumericTextBox>
                                                                </div>
                                                        </div>
                                                   
                                                    </div>
                                                    <div class="form-group row" runat="server" visible="true" id="viaje_9_2">
                                                         <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label41" CssClass="control-label text-bold">Descuento por evento:</asp:Label>
                                                            <br />
                                                            <asp:RadioButtonList ID="rbn_descuento" runat="server"
                                                                RepeatColumns="2" AutoPostBack="true">
                                                                <asp:ListItem Value="1">Sí  &nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="0">No  &nbsp;</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                                    ControlToValidate=""  Visible="false" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label33" CssClass="control-label text-bold">Tipo de descuento</asp:Label>
                                                            <br />
                                                            <asp:CheckBoxList ID="chk_tipo_descuento" AutoPostBack="true" runat="server">
                                                                    <Items>
                                                                        <asp:ListItem Text="Desayuno" Value="desayuno" />
                                                                        <asp:ListItem Text="Almuerzo" Value="almuerzo" />
                                                                        <asp:ListItem Text="Cena" Value="cena" />
                                                                    </Items>
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div class="col-sm-6 col-md-6 col-lg-3">
                                                            <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Descuento alimentación</asp:Label>
                                                                <br />
                                                                <telerik:RadNumericTextBox ID="txt_descuento_alimentacion" NumberFormat-DecimalDigits="0" enabled="false" runat="server" value="0"  Width="90%" MinValue="0">
                                                                </telerik:RadNumericTextBox>
                                                                <asp:hiddenfield runat="server" id="descuento" value="0" />
                                                                <asp:hiddenfield runat="server" id="descuento_desayuno" value="0" />
                                                                <asp:hiddenfield runat="server" id="descuento_almuerzo" value="0" />
                                                                <asp:hiddenfield runat="server" id="descuento_cena" value="0" />
                                                        </div>
                                                    </div>
                                                </div>
                                            
                                            </div>
                                            <div class="form-group row" runat="server" visible="false" id="errorGuardar">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="alert alert-danger" role="alert">
                                                        No se puede subir información con valores en 0
                                                    </div>
                                                </div>
                                            </div>
                                           <div class="form-group row">
                                                <div class="box-footer">
                                                    <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                        ValidationGroup="1" CausesValidation="true">
                                                    </telerik:RadButton>
                                                    <asp:Label ID="Label24" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                       
                                    </div>
                               </div>
                           </div>
                           <div class="panel panel-default">
                               <div class="panel-heading" role="tab" id="detalleLegalizacion" data-toggle="collapse" data-parent="#accordion" href="#collapseDetalleLegalizacion" aria-expanded="false" aria-controls="collapseDetalleLegalizacion">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseDetalleLegalizacion"
                                            aria-expanded="false" aria-controls="collapseDetalleLegalizacion" runat="server" id="a3">Detalle legalización del viaje
                                        </a>
                                    </h4>
                                </div>
                               <div id="collapseDetalleLegalizacion" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="detalleLegalizacion">
                                    <div class="panel-body">
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">General (Comunicaciones, pasajes y autos)</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_general" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_legalizacion_viaje_detalle"
                                                                FilterControlAltText="Filter id_legalizacion_viaje_detalle column"
                                                                SortExpression="id_legalizacion_viaje_detalle" UniqueName="id_legalizacion_viaje_detalle"
                                                                Visible="False" HeaderText="id_legalizacion_viaje_detalle"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                <HeaderStyle Width="4%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                        OnClick="delete_detalle">
                                                                        <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha column" HeaderStyle-Width="13%"
                                                                HeaderText="Fecha" SortExpression="fecha"
                                                                UniqueName="colm_fecha">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                                FilterControlAltText="Filter nro_rec column" HeaderStyle-Width="7%"
                                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                                UniqueName="colm_nro_rec">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                                FilterControlAltText="Filter descripcion_gasto column" HeaderStyle-Width="23%"
                                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                                UniqueName="colm_descripcion_gasto">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_legalizacion_pasajes"
                                                                FilterControlAltText="Filter codigo_legalizacion_pasajes column" HeaderStyle-Width="23%"
                                                                HeaderText="Code Pasajes" SortExpression="codigo_legalizacion_pasajes"
                                                                UniqueName="colm_codigo_legalizacion_pasajes">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_pasajes" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_pasajes column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto pasajes" SortExpression="monto_pasajes"
                                                                UniqueName="colm_monto_pasajes">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_total column" HeaderStyle-Width="15%"
                                                                HeaderText="Subtotal" SortExpression="monto_total"
                                                                UniqueName="colm_monto_total">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Reuniones</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_reuniones" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_legalizacion_viaje_detalle"
                                                                FilterControlAltText="Filter id_legalizacion_viaje_detalle column"
                                                                SortExpression="id_legalizacion_viaje_detalle" UniqueName="id_legalizacion_viaje_detalle"
                                                                Visible="False" HeaderText="id_legalizacion_viaje_detalle"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                <HeaderStyle Width="3%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                        OnClick="delete_detalle">
                                                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha" SortExpression="fecha"
                                                                UniqueName="colm_fecha">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                                FilterControlAltText="Filter nro_rec column" HeaderStyle-Width="23%"
                                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                                UniqueName="colm_nro_rec">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ent_mtg"
                                                                FilterControlAltText="Filter ent_mtg column" HeaderStyle-Width="23%"
                                                                HeaderText="ENT/MTG" SortExpression="ent_mtg"
                                                                UniqueName="colm_ent_mtg">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="nro_participantes"
                                                                FilterControlAltText="Filter nro_participantes column" HeaderStyle-Width="23%"
                                                                HeaderText="# Participantes" SortExpression="nro_participantes"
                                                                UniqueName="colm_nro_participantes">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                                FilterControlAltText="Filter descripcion_gasto column" HeaderStyle-Width="23%"
                                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                                UniqueName="colm_descripcion_gasto">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_total column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto" SortExpression="monto_total"
                                                                UniqueName="colm_monto_total">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Miscelaneos</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_miscelaneos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_legalizacion_viaje_detalle"
                                                                FilterControlAltText="Filter id_legalizacion_viaje_detalle column"
                                                                SortExpression="id_legalizacion_viaje_detalle" UniqueName="id_legalizacion_viaje_detalle"
                                                                Visible="False" HeaderText="id_legalizacion_viaje_detalle"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                <HeaderStyle Width="3%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                        OnClick="delete_detalle">
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha" SortExpression="fecha"
                                                                UniqueName="colm_fecha">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                                FilterControlAltText="Filter nro_rec column" HeaderStyle-Width="23%"
                                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                                UniqueName="colm_nro_rec">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                                FilterControlAltText="Filter descripcion_gasto column" HeaderStyle-Width="23%"
                                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                                UniqueName="colm_descripcion_gasto">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_total column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto" SortExpression="monto_total"
                                                                UniqueName="colm_monto_total">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Alimentación y alojamiento</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_alimentacion_alojamiento" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_legalizacion_viaje_detalle"
                                                                FilterControlAltText="Filter id_legalizacion_viaje_detalle column"
                                                                SortExpression="id_legalizacion_viaje_detalle" UniqueName="id_legalizacion_viaje_detalle"
                                                                Visible="False" HeaderText="id_legalizacion_viaje_detalle"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                <HeaderStyle Width="4%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                        OnClick="delete_detalle">
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha column" HeaderStyle-Width="18%"
                                                                HeaderText="Fecha" SortExpression="fecha"
                                                                UniqueName="colm_fecha">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ubicacion_alojamiento"
                                                                FilterControlAltText="Filter ubicacion_alojamiento column" HeaderStyle-Width="23%"
                                                                HeaderText="Lugar" SortExpression="ubicacion_alojamiento"
                                                                UniqueName="colm_ubicacion_alojamiento">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                                FilterControlAltText="Filter descripcion_gasto column" HeaderStyle-Width="23%"
                                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                                UniqueName="colm_descripcion_gasto">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="porcentaje_perdiem"
                                                                FilterControlAltText="Filter porcentaje_perdiem column" HeaderStyle-Width="8%"
                                                                HeaderText="% viatico" SortExpression="porcentaje_perdiem"
                                                                UniqueName="colm_porcentaje_perdiem">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="valor_perdiem"
                                                                FilterControlAltText="Filter valor_perdiem column" HeaderStyle-Width="12%"
                                                                HeaderText="M&IE Rate" SortExpression="valor_perdiem"
                                                                DataFormatString="{0:n0}" 
                                                                UniqueName="colm_valor_perdiem">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="numero_dias"
                                                                FilterControlAltText="Filter numero_dias column" HeaderStyle-Width="8%"
                                                                HeaderText="# días" SortExpression="numero_dias"
                                                                UniqueName="colm_numero_dias">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="descuento_alimentacion" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter descuento_alimentacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Descuento alimentación" SortExpression="descuento_alimentacion"
                                                                UniqueName="colm_descuento_alimentacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="valor_total_alimentacion" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_alimentacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto alimentación" SortExpression="valor_total_alimentacion"
                                                                UniqueName="colm_valor_total_alimentacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_alojamiento" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_alojamiento column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto alojamiento por noche" SortExpression="monto_alojamiento"
                                                                UniqueName="colm_monto_alojamiento">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_total column" HeaderStyle-Width="18%"
                                                                HeaderText="Subtotal" SortExpression="monto_total"
                                                                UniqueName="colm_monto_total">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label40" CssClass="control-label text-bold">Soportes de la legalización</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_soportes" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_soporte_legalizacion_viaje" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                             <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                <HeaderStyle Width="4%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                        OnClick="delete_detalle2">
                                                                        <asp:Image ID="Image4" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="id_soporte_legalizacion_viaje"
                                                                FilterControlAltText="Filter id_soporte_legalizacion_viaje column"
                                                                SortExpression="id_soporte_legalizacion_viaje" UniqueName="id_soporte_legalizacion_viaje"
                                                                Visible="False" HeaderText="id_soporte_legalizacion_viaje"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="codigo_facturacion" Visible="false"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha column" HeaderStyle-Width="18%"
                                                                HeaderText="Fecha" SortExpression="fecha"
                                                                UniqueName="colm_fecha">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="tipo_soporte_legalizacion"
                                                                FilterControlAltText="Filter tipo_soporte_legalizacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Tipo de soporte" SortExpression="tipo_soporte_legalizacion"
                                                                UniqueName="tipo_soporte_legalizacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="soporte"
                                                                FilterControlAltText="Filter soporte column" HeaderStyle-Width="23%"
                                                                HeaderText="Soporte" SortExpression="soporte"
                                                                UniqueName="soporte">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                           <telerik:GridTemplateColumn UniqueName="colm_soporte" Visible="true">
                                                                <HeaderStyle Width="4%" />
                                                                <ItemTemplate>
                                                                   <asp:HyperLink ID="col_hlk_soporte" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                                        ToolTip="Soporte" Target="_new">
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </div>
                                    </div>
                               </div>
                           </div>
                       </div>
                          <div class="panel panel-default"  runat="server" id="lyHistory" visible="false">
                        
                        <div class="panel-heading" role="tab" id="infoApprobacion" data-toggle="collapse" data-parent="#accordion" href="#collapseApp" aria-expanded="false" aria-controls="collapseApp">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseApp"
                                    aria-expanded="false" aria-controls="collapseApp" runat="server" id="a4">Aprobación
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
                                        <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px" OnClientClick=" this.disabled = true; this.value = 'Processing...';"  TextMode="MultiLine" Width="100%">
                                        </telerik:RadTextBox>         
                                    </div>
                                                                                    
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-3 text-center  ">                                           
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_Approved" runat="server" Text="Aprobar"  OnClick="btn_Approved_Click"  ValidationGroup="2"  UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="72%" />
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
                                <div class="form-group row">
                                    <div class="col-sm-12 text-center">                                              
                                            <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                                        <br /><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                  </div>

                </div>
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="tipoEliminar" Value="0" />
                <asp:HiddenField runat="server" ID="id_tasa_ser" Value="0" />
                <asp:HiddenField runat="server" ID="valor_perdiem" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />

              

                <div class="box-footer">
                    <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                    </telerik:RadButton>
                     <telerik:RadButton ID="btn_guardar_legalizacion" runat="server" Text="Guardar" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                        ValidationGroup="2" CausesValidation="true">
                    </telerik:RadButton>
                    <telerik:RadButton ID="btn_enviar_aprobacion" runat="server" Text="Enviar por aprobación" OnClientClick=" this.disabled = true; this.value = 'Processing...';" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                        ValidationGroup="2">
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

            <div class="modal fade" id="modalConfirmTasaSer" data-backdrop="static" data-keyboard="false">
                <div class="vertical-alignment-helper">
                    <div class="modal-dialog  modal-dialog-centered">
                        <div class="modal-content modal-lg ">
                            <div class="modal-header modal-warning">
                                <h4 class="modal-title" runat="server" id="H1">Fecha de radicación</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="Label27" runat="server" Text="Al cambiar el mes de radicación se modificaran los valores asociados a alimentación, desea continuar?" />
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btn_ajustar_valores" CssClass="btn btn-sm btn-warning btn-ok" Text="Continuar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script type="text/javascript">
        var montoComunicaciones = 0;
        var montoPasajes = 0;
        var montoAuto = 0;
        var montoReuniones = 0;
        var montoMiscelaneos = 0;
        var montoAlojamiento = 0;

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
            validarSoporte();
            validarSoporteReuniones();
            validarSoporteMiscelaneos();
            validarSoporteAlojamiento();
            validarSoporteTiquete();
        })
        window.onClientFileUploaded = function (radAsyncUpload, args) {
            validarSoporte();
            validarSoporteReuniones();
            validarSoporteMiscelaneos();
            validarSoporteAlojamiento();
            validarSoporteTiquete();
        }

        function validarSoporteTiquete() {
            var soporteTiquete = $find("<%= soporte_tiquete.ClientID %>");
            if (soporteTiquete != null) {
                if ($find("<%= requiere_soporte_tiquete.ClientID %>").get_value() >= 1 && soporteTiquete.getUploadedFiles().length == 0 && $find("<%= soporte_tiquete_val.ClientID %>").get_value() <= 0) {
                    $find("<%= soporte_tiquete_val.ClientID %>").set_value();
                }
                else {
                    $find("<%= soporte_tiquete_val.ClientID %>").set_value(1);
                }
            }
        }


        function calc_monto_comunicaciones(sender, eventArgs) {
            montoComunicaciones = eventArgs.get_newValue();
            validarSoporte();
        }
        function calc_monto_pasajes(sender, eventArgs) {
            montoPasajes = eventArgs.get_newValue();
            validarSoporte();
        }
        function calc_monto_auto(sender, eventArgs) {
            montoAuto = eventArgs.get_newValue();
            validarSoporte();
        }

        function calc_monto_reuniones(sender, eventArgs) {
            montoReuniones = eventArgs.get_newValue();
            validarSoporteReuniones();
        }

        function calc_monto_miscelaneos(sender, eventArgs) {
            montoMiscelaneos = eventArgs.get_newValue();
            validarSoporteMiscelaneos();
        }

        function calc_monto_alojamiento(sender, eventArgs) {
            montoAlojamiento = eventArgs.get_newValue();
            validarSoporteAlojamiento();
        }


        function validarSoporte()
        {
            var soportePasajes = $find("<%= soporte_pasajes.ClientID %>");
            if (soportePasajes != null) {
               
                if (montoPasajes > 50000 && soportePasajes.getUploadedFiles().length <= 0) {
                    $find("<%= soporte_pasajes_val.ClientID %>").set_value();
                }
                else {
                    $find("<%= soporte_pasajes_val.ClientID %>").set_value(1);
                }
            }
            
          
        }

        function validarSoporteReuniones() {
            var soporteReuniones = $find("<%= soporte_reuniones.ClientID %>");
            if (soporteReuniones != null) {
                if (montoReuniones > 0 && soporteReuniones.getUploadedFiles().length <= 0) {
                    $find("<%= soporte_reuniones_val.ClientID %>").set_value();
                }
                else {
                    $find("<%= soporte_reuniones_val.ClientID %>").set_value(1);
                }
            }

        }

        function validarSoporteMiscelaneos() {
            var soporteMiscelaneos = $find("<%= soporte_miscelaneos.ClientID %>");
            if (soporteMiscelaneos != null) {
                if (montoMiscelaneos > 0 && soporteMiscelaneos.getUploadedFiles().length <= 0) {
                    $find("<%= soporte_miscelaneos_val.ClientID %>").set_value();
                }
                else {
                    $find("<%= soporte_miscelaneos_val.ClientID %>").set_value(1);
                }
            }

        }

        function validarSoporteAlojamiento() {
            var soporteAlojamiento = $find("<%= soporte_alojamiento.ClientID %>");
            if (soporteAlojamiento != null) {
                if (montoAlojamiento > 0 && soporteAlojamiento.getUploadedFiles().length <= 0) {
                    $find("<%= soporte_alojamiento_val.ClientID %>").set_value();
                }
                else {
                    $find("<%= soporte_alojamiento_val.ClientID %>").set_value(1);
                }
            }

        }
        function ResetVal() {
            montoComunicaciones = 0;
            montoPasajes = 0;
            montoAuto = 0;
            montoReuniones = 0;
            montoMiscelaneos = 0;
            montoAlojamiento = 0;
            validarSoporte();
            validarSoporteReuniones();
            validarSoporteMiscelaneos();
            validarSoporteAlojamiento();
            validarSoporteTiquete();
            $('#myModal').modal('show');
        }

        function ajusteTasaSer() {
            $('#modalConfirmTasaSer').modal('show');
        }
        function cerrarModalTasaSer() {
            $(".modal-backdrop").remove();
            $('body').removeClass('modal-open');
            $('#modalConfirmTasaSer').modal('hide');
        }
    </script>
</asp:Content>