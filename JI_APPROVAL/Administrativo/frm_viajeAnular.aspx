<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viajeAnular.aspx.vb" Inherits="RMS_APPROVAL.frm_viajeAnular" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Anular viaje</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_itinerario" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        
                       
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:HiddenField runat="server" ID="idViaje" Value="0" />
                                <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha de inicio del viaje</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_inicio" AutoPostBack="false" Width="90%" Enabled="false" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                               
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Fecha de finalización del viaje</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_fin" AutoPostBack="false" Width="90%" Enabled="false" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                               
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Número Contacto:</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_numero_contacto" runat="server" Enabled="false" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                               
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Tipo de viaje:</asp:Label>
                                <br />
                                <asp:RadioButtonList runat="server" ID="rbn_tipo_viaje" Enabled="false" RepeatColumns="2" >
                                </asp:RadioButtonList>
                               
                            </div>
                            
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Motivo del Viaje:</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_motivo_viaje" runat="server" Rows="3" Enabled="false" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_motivo_viaje" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
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
                                <hr />
                            </div>
                        </div>
                        <div class="form-group row">
                            
                            <div class="col-sm-12">
                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Motivo anula</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_motivo_anular" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                            ControlToValidate="txt_motivo_anular" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Anular viaje" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
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