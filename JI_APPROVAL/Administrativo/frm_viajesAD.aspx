﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viajesAD.aspx.vb" Inherits="RMS_APPROVAL.frm_viajesAD" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Registrar viaje</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_itinerario" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        
                       <div class="form-group row" runat="server" visible="false" id="subRegionVisible">
                           <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label34" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_sub_Region" AutoPostBack="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server"
                                                ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                       </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="lbl_tipo_actividad_" CssClass="control-label text-bold">Fecha solicitud del viaje viaje</asp:Label>
                                <br />
                                 <asp:Label runat="server" ID="lbl_fecha_solicitud" CssClass="control-label">Fecha solicitud del viaje viaje</asp:Label>
                            </div>
                           <%-- <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha de inicio del viaje</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_inicio" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_inicio" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Fecha de finalización del viaje</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_fin" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                            ControlToValidate="dt_fecha_fin" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>--%>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Número Contacto:</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_numero_contacto" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                            ControlToValidate="txt_numero_contacto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3" runat="server" visible="false" id="tipoViaje">
                                <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Tipo de viaje:</asp:Label>
                                <br />
                              
                                <asp:RadioButtonList ID="rbn_tipo_viaje" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="false">
                                    <asp:ListItem Value="1">Operativo &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="2">Técnico &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                            ControlToValidate="rbn_tipo_viaje" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>

                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="blt_estrategia" CssClass="control-label text-bold">Estrategia:</asp:Label>
                                <br />
                               <telerik:RadComboBox ID="cmb_estrategia"  emptymessage="Seleccione..." Height="200px" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="cmb_estrategia" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Motivo del Viaje:</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_motivo_viaje" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_motivo_viaje" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Registrar Itinerario</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Fecha de viaje</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_viaje" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="dt_fecha_viaje" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Hora de salida</asp:Label>
                                <br />
                                 <telerik:RadTimePicker runat="server" ID="rt_hora_salida" Width="90%"  DateInput-ReadOnly="true">
                                 </telerik:RadTimePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="rt_hora_salida" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>

                            </div>


                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Departamento de origen</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_departamento_origen" AutoPostBack="true" Height="200px" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                            ControlToValidate="cmb_departamento_origen" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Municipio origen</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_municipio_origen" AutoPostBack="false" Height="200px" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                            ControlToValidate="cmb_municipio_origen" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>


                          <%--  <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Ciudad de origen</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_ciudad_origen" runat="server" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                            ControlToValidate="txt_ciudad_origen" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>--%>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Departamento de destino</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_departamento_destino" Height="200px" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                            ControlToValidate="cmb_departamento_destino" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Municipio destino</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_municipio_destino" Height="200px" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="cmb_municipio_destino" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                            <%--<div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Ciudad de destino:</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_ciudad_destino" runat="server" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="txt_ciudad_destino" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>--%>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Requiere transporte aéreo</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_transporte_aereo" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="rbn_transporte_aereo" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                <div runat="server" visible="false" id="transporte_aereo">
                                    <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Línea aérea</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_linea_aerea" runat="server" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Requiere vehiculo del proyecto :</asp:Label>
                                <br />
                                 <asp:RadioButtonList ID="rbn_vehiculo_proyecto" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                            ControlToValidate="rbn_vehiculo_proyecto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                <div runat="server" visible="false" id="vehiculo_proyecto">
                                    <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Detalle del vehiculo del proyecto</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_vehiculo_proyecto" runat="server" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Requiere transporte fluvial</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_transporte_fluvial" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                            ControlToValidate="rbn_transporte_fluvial" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>

                                <div runat="server" visible="false" id="transporte_fluvial">
                                    <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Detalle del ransporte fluvial</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_transporte_fluvial" runat="server" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Requiere servicio público</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_servicio_publico" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                            ControlToValidate="rbn_servicio_publico" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                <div id="servicio_pubiclo" runat="server" visible="false">
                                    <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Detalle del servicio público</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_servicio_publico" runat="server" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group row">
                            <div class="col-sm-8"></div>
                            <div class="col-sm-4 text-right">
                                <telerik:RadButton ID="btn_agregar_itinerario" runat="server" CssClass="btn btn-sm" Text="Agregar itinerario" ValidationGroup="2" Width="100px">
                                </telerik:RadButton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Detalle itinerario</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <telerik:RadGrid ID="grd_itinerario" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" 
                                ShowColumnFooters="true"
                                ShowGroupFooters="true"
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
                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                            <HeaderStyle Width="3%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                    OnClick="delete_itinerario">
                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
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


                         <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Registrar Alojamiento</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Fecha de llegada</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_llegada" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                            ControlToValidate="dt_fecha_llegada" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Fecha de salida</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_salida" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                            ControlToValidate="dt_fecha_salida" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Departamento</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_departamento_hotel" Height="200px" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                            ControlToValidate="cmb_departamento_hotel" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Municipio</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_municipio_hotel" Height="200px" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                            ControlToValidate="cmb_municipio_hotel" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Hotel preferido</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_hotel" runat="server" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                            ControlToValidate="txt_hotel" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-8"></div>
                            <div class="col-sm-4 text-right">
                                <telerik:RadButton ID="btn_agregar_alojamiento" runat="server" CssClass="btn btn-sm" Text="Agregar alojamiento" ValidationGroup="3" Width="120px">
                                </telerik:RadButton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Detalle del alojamiento</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <telerik:RadGrid ID="grd_alojamiento" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" 
                                ShowColumnFooters="true"
                                ShowGroupFooters="true"
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
                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                            <HeaderStyle Width="3%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                    OnClick="delete_alojamiento">
                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
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
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Componente</asp:Label></h4>
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
                    <div class="form-group row">
                        <div class="col-sm-12 text-center">                                              
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                            <br /><br />
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="idviaje" Value="0" />
                    <asp:HiddenField runat="server" ID="habilitar_registro_viaje" Value="0" />
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-body" runat="server" visible="false" id="mensajeNumeroDiasHabiles">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div class="alert alert-danger" role="alert">
                                ¡Ha superado el número de viajes permitidos por mes!
                                <br />
                                Viajes realizados en el mes: <%=numViajes %>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar_2" runat="server" Text="Guardar " Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar y enviar por aprobación" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
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
</asp:Content>