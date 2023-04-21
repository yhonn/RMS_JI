<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viaje_facturacion.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_facturacion" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Facturación viaje</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_itinerario" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        
                       <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                                           
                            <%--<div class="panel panel-default"> 
                                <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseOne"
                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_informacion">Información de la aprobación
                                        </a>
                                    </h4>
                                </div>
                               
                                <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <div class="col-sm-12 text-right">
                                           <div class="form-group row">
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
                                        </div> <!--div 0 lg-12-->
                                    </div>
                                </div>
                                
                            </div>--%>  
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
                                        <div class="col-sm-12 text-left ">
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
                                                                Visible="false" HeaderText="id_viaje_itinerario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_viaje" DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_viaje column" HeaderStyle-Width="15%"
                                                                HeaderText="Fecha" SortExpression="fecha_viaje"
                                                                UniqueName="colm_fecha_viaje">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hora_salida"
                                                                FilterControlAltText="Filter hora_salida column" HeaderStyle-Width="11%"
                                                                HeaderText="Hora de salida" SortExpression="hora_salida"
                                                                UniqueName="colm_hora_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_origen" 
                                                                FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="16%"
                                                                HeaderText="Ciudad de origen" SortExpression="ciudad_origen"
                                                                UniqueName="colm_ciudad_origen">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_destino"
                                                                FilterControlAltText="Filter ciudad_destino column" HeaderStyle-Width="16%"
                                                                HeaderText="Ciudad de destino" SortExpression="ciudad_destino"
                                                                UniqueName="colm_ciudad_destino">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="requiere_transporte_aereo_text"
                                                                FilterControlAltText="Filter requiere_transporte_aereo_text column" HeaderStyle-Width="15%"
                                                                HeaderText="Transporte aéreo" SortExpression="requiere_transporte_aereo_text"
                                                                UniqueName="colm_requiere_transporte_aereo_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_vehiculo_proyecto_text"
                                                                FilterControlAltText="Filter requiere_vehiculo_proyecto_text column" HeaderStyle-Width="15%"
                                                                HeaderText="Vehiculo proyecto" SortExpression="requiere_vehiculo_proyecto_text"
                                                                UniqueName="colm_requiere_vehiculo_proyecto_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="requiere_vehiculo_proyecto"
                                                                FilterControlAltText="Filter requiere_vehiculo_proyecto column" visible="false" display="false"
                                                                HeaderText="requiere_vehiculo_proyecto" SortExpression="requiere_vehiculo_proyecto"
                                                                UniqueName="requiere_vehiculo_proyecto">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="requiere_transporte_aereo"
                                                                FilterControlAltText="Filter requiere_transporte_aereo column" visible="false" display="false"
                                                                HeaderText="requiere_transporte_aereo" SortExpression="requiere_transporte_aereo"
                                                                UniqueName="requiere_transporte_aereo">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_transporte_fluvial_text" 
                                                                FilterControlAltText="Filter requiere_transporte_fluvial_text column" HeaderStyle-Width="15%"
                                                                HeaderText="Transporte fluvial" SortExpression="requiere_transporte_fluvial_text"
                                                                UniqueName="colm_requiere_transporte_fluvial_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_servicio_publico_text"
                                                                FilterControlAltText="Filter requiere_servicio_publico_text column" HeaderStyle-Width="13%"
                                                                HeaderText="Servicio público" SortExpression="requiere_servicio_publico_text"
                                                                UniqueName="colm_requiere_servicio_publico_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_radicacion_aereo"
                                                                FilterControlAltText="Filter fecha_radicacion_aereo column" visible="false" display="false"
                                                                HeaderText="fecha_radicacion_aereo" SortExpression="fecha_radicacion_aereo"
                                                                UniqueName="fecha_radicacion_aereo">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_radicacion_alquiler"
                                                                FilterControlAltText="Filter fecha_radicacion_alquiler column" visible="false" display="false"
                                                                HeaderText="fecha_radicacion_alquiler" SortExpression="fecha_radicacion_alquiler"
                                                                UniqueName="fecha_radicacion_alquiler">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="numero_factura_aereo"
                                                                FilterControlAltText="Filter numero_factura_aereo column"
                                                                SortExpression="numero_factura_aereo" UniqueName="numero_factura_aereo"
                                                                Visible="False" HeaderText="numero_factura_aereo"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="valor_total_aereo"
                                                                FilterControlAltText="Filter valor_total_aereo column"
                                                                SortExpression="valor_total_aereo" UniqueName="valor_total_aereo"
                                                                Visible="False" HeaderText="valor_total_aereo"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="numero_factura_alquiler"
                                                                FilterControlAltText="Filter numero_factura_alquiler column"
                                                                SortExpression="numero_factura_alquiler" UniqueName="numero_factura_alquiler"
                                                                Visible="False" HeaderText="numero_factura_alquiler"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="valor_total_alquiler"
                                                                FilterControlAltText="Filter valor_total_alquiler column"
                                                                SortExpression="valor_total_alquiler" UniqueName="valor_total_alquiler"
                                                                Visible="False" HeaderText="valor_total_alquiler"
                                                                ReadOnly="True">
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
                                                            <telerik:GridBoundColumn DataField="numero_factura"
                                                                FilterControlAltText="Filter numero_factura column"
                                                                SortExpression="numero_factura" UniqueName="numero_factura"
                                                                Visible="False" HeaderText="numero_factura"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="valor_total"
                                                                FilterControlAltText="Filter valor_total column"
                                                                SortExpression="valor_total" UniqueName="valor_total"
                                                                Visible="False" HeaderText="valor_total"
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
                                                                HeaderText="Hotel preferido" SortExpression="hotel"
                                                                UniqueName="colm_hotel_">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="numero_noches"
                                                                FilterControlAltText="Filter numero_noches column" HeaderStyle-Width="23%"
                                                                HeaderText="Número de noches" SortExpression="numero_noches"
                                                                UniqueName="colm_numero_noches">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hotel_alojamiento"
                                                                FilterControlAltText="Filter hotel_alojamiento column"
                                                                HeaderText="hotel_alojamiento" SortExpression="hotel_alojamiento" visible="false" display="false"
                                                                UniqueName="hotel_alojamiento">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="fecha_radicacion"
                                                                FilterControlAltText="Filter fecha_radicacion column"
                                                                HeaderText="fecha_radicacion" SortExpression="fecha_radicacion" visible="false" display="false"
                                                                UniqueName="fecha_radicacion">
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
                                <div class="panel-heading" role="tab" id="infoFacturacion" data-toggle="collapse" data-parent="#accordion" href="#collapseFacturacion" aria-expanded="false" aria-controls="collapseFacturacion">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseFacturacion"
                                            aria-expanded="false" aria-controls="collapseFacturacion" runat="server" id="a2">Facturación
                                        </a>
                                    </h4>
                                </div>
                               <div id="collapseFacturacion" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoFacturacion">
                                   <div class="panel-body">
                                       <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Facturación de alquiler de vehiculo</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Fecha de radicación</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_radicacion_alquiler" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                           </div>
                                       </div>
                                       <div class="form-group row">
                                           <hr />
                                       </div>

                                        

                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Valor de la factura alquiler</asp:Label>
                                                <br />
                                                <telerik:RadNumericTextBox ID="txt_valor_factura_alquiler" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadNumericTextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                    ControlToValidate="txt_valor_factura_alquiler" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                           </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Número de factura</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_factura_alquiler" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadTextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                                    ControlToValidate="txt_factura_alquiler" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                           </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Fecha de servicio</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_servicio" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                    ControlToValidate="dt_fecha_servicio" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                           </div>
                                          
                                           <div class="col-sm-12 col-md-12 col-lg-3">
                                            <asp:Label runat="server" ID="Label36" CssClass="control-label text-bold">Soporte:</asp:Label>
                                            <br />
                                            <telerik:RadAsyncUpload 
                                                RenderMode="Lightweight" 
                                                runat="server" 
                                                ID="soporte_alquiler"
                                                OnClientFileUploaded="onClientFileUploaded"
                                                MultipleFileSelection="Automatic" 
                                                Skin="Office2007"
                                                TemporaryFolder="~/Temp" 
                                                PostbackTriggers="btn_add_factura"
                                                MaxFileInputsCount="1"
                                                    data-clientFilter="application/pdf"
                                                AllowedFileExtensions="pdf"
                                                HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                            </telerik:RadAsyncUpload>
                                           
                                        </div>
                                       </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Observaciones</asp:Label>
                                                <br />
                                                 <telerik:RadTextBox ID="txt_observaciones_alquiler" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                                </telerik:RadTextBox>
                                            </div>
                                        </div>


                                       <div class="form-group row">
                                            <div class="col-sm-8"></div>
                                            <div class="col-sm-4 text-right">
                                                <telerik:RadButton ID="btn_add_factura" runat="server" CssClass="btn btn-sm" Text="Agregar factura" ValidationGroup="6" Width="100px">
                                                </telerik:RadButton>
                                            </div>
                                        </div>

                                       <div class="form-group row">
                                            <hr />

                                        </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <telerik:RadGrid ID="grd_facturacion_alquiler" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                        ShowFooter="true" 
                                                        ShowColumnFooters="true"
                                                        ShowGroupFooters="true"
                                                        ShowGroupPanel="false">
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_solicitud_viaje_vehiculo" AllowAutomaticUpdates="True" >
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="id_solicitud_viaje_vehiculo"
                                                                    FilterControlAltText="Filter id_solicitud_viaje_vehiculo column"
                                                                    SortExpression="id_solicitud_viaje_vehiculo" UniqueName="id_solicitud_viaje_facturacion_hotel"
                                                                    Visible="False" HeaderText="id_solicitud_viaje_vehiculo"
                                                                    ReadOnly="True">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                    <HeaderStyle Width="3%" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                            OnClick="delete_alquiler">
                                                                            <asp:Image ID="Image2" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="fecha_radicacion" DataFormatString="{0:MM/dd/yyyy}"
                                                                    FilterControlAltText="Filter fecha_radicacion column" HeaderStyle-Width="19%"
                                                                    HeaderText="Fecha de radicación" SortExpression="fecha_radicacion"
                                                                    UniqueName="fecha_radicacion">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                 <telerik:GridBoundColumn DataField="valor_factura" DataFormatString="{0:n}"
                                                                    FilterControlAltText="Filter valor_factura column" HeaderStyle-Width="19%"
                                                                    HeaderText="Valor" SortExpression="valor_factura"
                                                                    UniqueName="valor_factura">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="numero_factura" 
                                                                    FilterControlAltText="Filter numero_factura column" HeaderStyle-Width="19%"
                                                                    HeaderText="# Factura" SortExpression="numero_factura"
                                                                    UniqueName="numero_factura">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                            </div>
                                            
                                        </div>

                                       <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Itinerario del viaje</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                       
                                       <div class="form-group row">
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Departamento origen</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_departamento" AutoPostBack="true" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                            ControlToValidate="cmb_departamento" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Municipio origen</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_municipio" AutoPostBack="false" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                            ControlToValidate="cmb_municipio" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Departamento destino</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_departamento_destino" AutoPostBack="true" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                            ControlToValidate="cmb_departamento_destino" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Municipio destino</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_municipio_destino" AutoPostBack="false" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                            ControlToValidate="cmb_departamento_destino" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                                  
                                            </div>
                                            
                                        </div>
                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                                 <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Fecha vuelo</asp:Label>
                                                <br />
                                                <telerik:RadDatePicker ID="dt_fecha_vuelo" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="dt_fecha_vuelo" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Aerolinea</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_linea_aerea" runat="server" Width="90%" MaxLength="1000">
                                                </telerik:RadTextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                            ControlToValidate="txt_linea_aerea" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                                            </div>
                                           <div class="col-sm-12 col-md-12 col-lg-3">
                                            <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Soporte:</asp:Label>
                                            <br />
                                            <telerik:RadAsyncUpload 
                                                RenderMode="Lightweight" 
                                                runat="server" 
                                                ID="soporte_aereo"
                                                OnClientFileUploaded="onClientFileUploaded"
                                                MultipleFileSelection="Automatic" 
                                                Skin="Office2007"
                                                TemporaryFolder="~/Temp" 
                                                PostbackTriggers="btn_agregar_itinerario"
                                                MaxFileInputsCount="1"
                                                    data-clientFilter="application/pdf"
                                                AllowedFileExtensions="pdf"
                                                HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                            </telerik:RadAsyncUpload>
                                         
                                        </div>
                                       </div>
                                       <div class="form-group row">
                                           <telerik:RadGrid ID="grd_vuelos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                        ShowFooter="true" 
                                                        ShowColumnFooters="true"
                                                        ShowGroupFooters="true"
                                                        ShowGroupPanel="false">
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_solicitud_viaje_facturacion_tiquete" AllowAutomaticUpdates="True" >
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="id_solicitud_viaje_facturacion_tiquete"
                                                                    FilterControlAltText="Filter id_solicitud_viaje_facturacion_tiquete column"
                                                                    SortExpression="id_solicitud_viaje_facturacion_tiquete" UniqueName="id_solicitud_viaje_facturacion_tiquete"
                                                                    Visible="False" HeaderText="id_solicitud_viaje_facturacion_tiquete"
                                                                    ReadOnly="True">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                    <HeaderStyle Width="3%" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                            OnClick="delete_itinerario">
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="fecha_vuelo" DataFormatString="{0:MM/dd/yyyy}"
                                                                    FilterControlAltText="Filter fecha_vuelo column" HeaderStyle-Width="19%"
                                                                    HeaderText="Fecha de vuelo" SortExpression="fecha_vuelo"
                                                                    UniqueName="colm_fecha_vuelo">
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
                                                                 <telerik:GridBoundColumn DataField="aerolinea"
                                                                    FilterControlAltText="Filter aerolinea column" HeaderStyle-Width="19%"
                                                                    HeaderText="Aerolinea" SortExpression="aerolinea"
                                                                    UniqueName="aerolinea">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                        </div>
                                       <div class="form-group row">
                                            <div class="col-sm-8"></div>
                                            <div class="col-sm-4 text-right">
                                                <telerik:RadButton ID="btn_agregar_itinerario" runat="server" CssClass="btn btn-sm" Text="Agregar vuelo" ValidationGroup="4" Width="100px">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                            <hr />
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label29" CssClass="control-label text-bold">Facturación tiquetes</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label38" CssClass="control-label text-bold">Valor total de la factura de tiquetes</asp:Label>
                                                <br />
                                                <telerik:RadNumericTextBox ID="txt_valor_factura_tiquetes" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadNumericTextBox>
                                             
                                           </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Número de factura</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_factura_tiquete" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadTextBox>
                                              
                                           </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Fecha de radicación</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_radicacion" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                
                                           </div>
                                         
                                       </div>
                                        
                                       <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Facturación de hoteles</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Hotel</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_hotel" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadTextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                            ControlToValidate="txt_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Número de factura</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_factura_hotel" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadTextBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                            ControlToValidate="txt_factura_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                           
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Fecha de llegada</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_llegada" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="dt_fecha_llegada" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Fecha de salida</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_salida" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="dt_fecha_salida" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                       </div>
                                       <div class="form-group row">
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Valor total</asp:Label>
                                                <br />
                                                <telerik:RadNumericTextBox ID="txt_valor_total_hotel" runat="server"  Width="90%" MinValue="0">
                                                    <ClientEvents/>
                                                </telerik:RadNumericTextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                            ControlToValidate="txt_valor_total_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Departamento</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_departamento_hotel" AutoPostBack="true" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                            ControlToValidate="cmb_departamento_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-3">
                                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Municipio</asp:Label>
                                                <br />
                                                <telerik:RadComboBox ID="cmb_municipio_hotel" AutoPostBack="true" Filter="Contains" emptymessage="Seleccione ..." runat="server" Width="90%">
                                                </telerik:RadComboBox>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                            ControlToValidate="cmb_municipio_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                
                                            </div>
                                           <div class="col-sm-6 col-md-6 col-lg-3">
                                               <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Fecha de radicación</asp:Label>
                                                <br />
                                               <telerik:RadDatePicker ID="dt_fecha_radicacion_hotel" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                            ControlToValidate="dt_fecha_radicacion_hotel" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                           </div>
                                       </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-3">
                                            <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Soporte:</asp:Label>
                                            <br />
                                            <telerik:RadAsyncUpload 
                                                RenderMode="Lightweight" 
                                                runat="server" 
                                                ID="soporte_hotel"
                                                OnClientFileUploaded="onClientFileUploaded"
                                                MultipleFileSelection="Automatic" 
                                                Skin="Office2007"
                                                TemporaryFolder="~/Temp" 
                                                PostbackTriggers="btn_agregar_hotel"
                                                MaxFileInputsCount="1"
                                                    data-clientFilter="application/pdf"
                                                AllowedFileExtensions="pdf"
                                                HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                            </telerik:RadAsyncUpload>
                                            
                                        </div>
                                       </div>
                                       <div class="form-group row">
                                            <div class="col-sm-8"></div>
                                            <div class="col-sm-4 text-right">
                                                <telerik:RadButton ID="btn_agregar_hotel" runat="server" CssClass="btn btn-sm" Text="Agregar hotel" ValidationGroup="3" Width="100px">
                                                </telerik:RadButton>
                                            </div>
                                        </div>
                                       <div class="form-group row">
                                            <hr />

                                        </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <telerik:RadGrid ID="grd_hotel" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                        ShowFooter="true" 
                                                        ShowColumnFooters="true"
                                                        ShowGroupFooters="true"
                                                        ShowGroupPanel="false">
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_solicitud_viaje_facturacion_hotel" AllowAutomaticUpdates="True" >
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="id_solicitud_viaje_facturacion_hotel"
                                                                    FilterControlAltText="Filter id_solicitud_viaje_facturacion_hotel column"
                                                                    SortExpression="id_solicitud_viaje_facturacion_hotel" UniqueName="id_solicitud_viaje_facturacion_hotel"
                                                                    Visible="False" HeaderText="id_solicitud_viaje_facturacion_hotel"
                                                                    ReadOnly="True">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                    <HeaderStyle Width="3%" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                            OnClick="delete_hotel">
                                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="fecha_desde" DataFormatString="{0:MM/dd/yyyy}"
                                                                    FilterControlAltText="Filter fecha_desde column" HeaderStyle-Width="19%"
                                                                    HeaderText="Fecha de llegada" SortExpression="fecha_desde"
                                                                    UniqueName="colm_fecha_viaje">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="fecha_hasta" DataFormatString="{0:MM/dd/yyyy}"
                                                                    FilterControlAltText="Filter fecha_hasta column" HeaderStyle-Width="19%"
                                                                    HeaderText="Fecha de salida" SortExpression="fecha_hasta"
                                                                    UniqueName="colm_fecha_hasta">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ciudad_origen" 
                                                                    FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="19%"
                                                                    HeaderText="Ciudad de origen" SortExpression="ciudad_origen"
                                                                    UniqueName="colm_ciudad_origen">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                 <telerik:GridBoundColumn DataField="hotel_alojamiento"
                                                                    FilterControlAltText="Filter hotel_alojamiento column" HeaderStyle-Width="19%"
                                                                    HeaderText="Hotel" SortExpression="hotel_alojamiento"
                                                                    UniqueName="hotel_alojamiento">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                 <telerik:GridBoundColumn DataField="valor_total" DataFormatString="{0:n}"
                                                                    FilterControlAltText="Filter valor_total column" HeaderStyle-Width="19%"
                                                                    HeaderText="Valor total" SortExpression="valor_total"
                                                                    UniqueName="valor_total">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="numero_factura" 
                                                                    FilterControlAltText="Filter numero_factura column" HeaderStyle-Width="19%"
                                                                    HeaderText="# Factura" SortExpression="numero_factura"
                                                                    UniqueName="numero_factura">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="fecha_radicacion" DataFormatString="{0:MM/dd/yyyy}"
                                                                    FilterControlAltText="Filter fecha_radicacion column" HeaderStyle-Width="19%"
                                                                    HeaderText="Fecha de radicación" SortExpression="fecha_radicacion"
                                                                    UniqueName="fecha_radicacion">
                                                                    <HeaderStyle CssClass="wrapWord"  />
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                            </div>
                                            
                                        </div>
                                       <div class="form-group row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Observaciones de la facturación</asp:Label>
                                                <br />
                                                 <telerik:RadTextBox ID="txt_observaciones" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                                </telerik:RadTextBox>
                                            </div>
                                        </div>
                                   </div>
                               </div>
                           </div>
                       </div>
                    </div>
                    
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                         <telerik:RadButton ID="btn_guardar_finalizar" runat="server" Text="Guardar y finalizar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
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
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
            
        })
        window.onClientFileUploaded = function (radAsyncUpload, args) {
           
        }
    </script>
</asp:Content>