<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPopUp.Master" CodeBehind="frm_viajeDetalle.aspx.vb" Inherits="RMS_APPROVAL.frm_viajeDetalle" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


  <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
        
  <div class="row invoice-info">
     
      <div class="col-md-12"> 
               
         <div class="panel-group">

                      <div class="panel panel-default">
                        <div class="panel-heading"><h3><asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVAL SYSTEM</asp:Label></h3></div>
                      </div>

                      <div class="panel panel-default">
                            <table class="table table-responsive table-condensed box box-primary">

                                 <tr class="tituloExitoso text-center">
                                    <td colspan="2" class="text-center">
                                        <asp:Label ID="Label4" runat="server" CssClass="text-bold">Información del viaje</asp:Label>
                                    </td>
                                </tr>
                              <tr class="box box-success">
                                 <td style="width:15%" class="text-left">
                                       <!--Tittle -->
                                     <asp:Label ID="lblt_usuario" runat="server"  CssClass="control-label text-bold" Text="Usuario:"></asp:Label>
                                 </td>
                                 <td>
                                       <!--Control -->
                                     <asp:Label ID="lbl_usuario" runat="server" ></asp:Label>                                                                                                         
                                </td>
                              </tr>   
                                <tr>
                                    <td style="width:15%" class="text-left">
                                      <!--Tittle -->
                                     <asp:Label ID="Label3" runat="server"  CssClass="control-label text-bold" Text="Número de documento:"></asp:Label>                                                                   
                                    </td>
                                    <td>
                                      <!--Control -->
                                     <asp:Label ID="lbl_numero_documento" runat="server" ></asp:Label>                                                                                                                                                                                          
                                    </td>
                                </tr>
                               <tr>
                                    <td style="width:15%" class="text-left">
                                      <!--Tittle -->
                                     <asp:Label ID="lblt_cargo" runat="server"  CssClass="control-label text-bold" Text="Cargo:"></asp:Label>                                                                   
                                    </td>
                                    <td>
                                      <!--Control -->
                                     <asp:Label ID="lbl_cargo" runat="server" ></asp:Label>                                                                                                                                                                                          
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="lblt_codigo_usuario" runat="server" CssClass="control-label text-bold" Text="Código de usuario:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_codigo_usuario" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label5" runat="server" CssClass="control-label text-bold" Text="Fecha de solicitud:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_fecha_solicitud" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label6" runat="server" CssClass="control-label text-bold" Text="Fecha de inicio del viaje:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_fecha_indicio_viaje" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label7" runat="server" CssClass="control-label text-bold" Text="Fecha de finalización del viaje:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_fecha_fin_viaje" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label8" runat="server" CssClass="control-label text-bold" Text="Motivo del viaje:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_motivo" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label2" runat="server" CssClass="text-bold">Detalle del itinerario</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td  colspan="2" class="text-left">
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
                                                    Visible="true" HeaderText="id_viaje_itinerario"
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
                                    </td>
                                </tr>
                                 <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label9" runat="server" CssClass="text-bold">Detalle del alojamiento</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td  colspan="2" class="text-left">
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
                                                            HeaderText="Hotel" SortExpression="hotel"
                                                            UniqueName="colm_hotel">
                                                            <HeaderStyle CssClass="wrapWord"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="numero_noches"
                                                            FilterControlAltText="Filter numero_noches column" HeaderStyle-Width="23%"
                                                            HeaderText="Número de noches" SortExpression="numero_noches"
                                                            UniqueName="colm_numero_noches">
                                                            <HeaderStyle CssClass="wrapWord"  />
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-center">
                                    <td colspan="2" class="text-center">
                                        <asp:Label ID="Label10" runat="server" CssClass="text-bold">Detalle de la legalización</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-success">
                                    <td colspan="2" class="text-center">
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label11" runat="server" CssClass="text-bold">General (Comunicaciones, pasajes y autos)</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
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
                                                            <telerik:GridBoundColumn DataField="codigo_legalizacion_comunicaciones"
                                                                FilterControlAltText="Filter codigo_legalizacion_comunicaciones column" HeaderStyle-Width="23%"
                                                                HeaderText="Code Comunicaciones" SortExpression="codigo_legalizacion_comunicaciones"
                                                                UniqueName="colm_codigo_legalizacion_comunicaciones">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_comunicaciones" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_comunicaciones column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto comunicaciones" SortExpression="monto_comunicaciones"
                                                                UniqueName="colm_monto_comunicaciones">
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
                                                            <telerik:GridBoundColumn DataField="millas"
                                                                FilterControlAltText="Filter millas column" HeaderStyle-Width="10%"
                                                                HeaderText="Millas" SortExpression="millas"
                                                                UniqueName="colm_millas">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="monto_auto" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_auto column" HeaderStyle-Width="12%"
                                                                HeaderText="Monto auto" SortExpression="monto_auto"
                                                                UniqueName="colm_monto_auto">
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label12" runat="server" CssClass="text-bold">Reuniones</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label13" runat="server" CssClass="text-bold">Miscelaneos</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label14" runat="server" CssClass="text-bold">Alimentación y aloamiento</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">

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
                                                            <telerik:GridBoundColumn DataField="monto_alimentacion" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                                FilterControlAltText="Filter monto_alimentacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Monto alimentación" SortExpression="monto_alimentacion"
                                                                UniqueName="colm_monto_alimentacion">
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label15" runat="server" CssClass="text-bold">Soportes de la legalización</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
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
                                                            <telerik:GridBoundColumn DataField="id_soporte_legalizacion_viaje"
                                                                FilterControlAltText="Filter id_soporte_legalizacion_viaje column"
                                                                SortExpression="id_soporte_legalizacion_viaje" UniqueName="id_soporte_legalizacion_viaje"
                                                                Visible="False" HeaderText="id_soporte_legalizacion_viaje"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                           <%-- <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                                FilterControlAltText="Filter codigo_facturacion column" HeaderStyle-Width="23%"
                                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                                UniqueName="colm_codigo_facturacion">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>--%>
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-center">
                                    <td colspan="2" class="text-center">
                                        <asp:Label ID="Label20" runat="server" CssClass="text-bold">Informe del viaje</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-success">
                                    <td colspan="2" class="text-center">
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label21" runat="server" CssClass="control-label text-bold" Text="Resultados:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lblt_resultados" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="Label22" runat="server" CssClass="control-label text-bold" Text="Compromisos/Conclusiones:"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lblt_compromisos" runat="server" ></asp:Label>                                                                        
                                    </td>
                                </tr>
                                 <tr class="tituloExitoso text-center">
                                    <td colspan="2" class="text-center">
                                        <asp:Label ID="Label16" runat="server" CssClass="text-bold">Rutas de aprobación</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-success">
                                    <td colspan="2" class="text-center">
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label17" runat="server" CssClass="text-bold">Aprobación del viaje</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
                                        
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
                                    
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label18" runat="server" CssClass="text-bold">Legalización del viaje</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
                                        <telerik:RadGrid ID="grd_cate2" runat="server" AllowAutomaticDeletes="True" 
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
                                                               <%-- <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                                            ToolTip="Indicador Incompleto">
                                                                        </asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>
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
                                    </td>
                                </tr>
                                <tr class="tituloExitoso text-left">
                                    <td colspan="2" class="text-left">
                                        <asp:Label ID="Label19" runat="server" CssClass="text-bold">Informe del viaje</asp:Label>
                                    </td>
                                </tr>
                                <tr class="box box-primary">
                                    <td colspan="2" class="text-center">
                                        <telerik:RadGrid ID="grd_cate3" runat="server" AllowAutomaticDeletes="True" 
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
                                                                <%--<telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="HyperLink2" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                                            ToolTip="Indicador Incompleto">
                                                                        </asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="text text-center">                                                          
                                       <asp:LinkButton ID="bntlk_print" runat="server" Width="10%"  CssClass="btn btn-sm btn-default additionalColumn" OnClientClick="javascript:window.print();" ToolTip="Print Report"  > <span class="icon ion-ios-printer"></span></asp:LinkButton>                        
                                    </td>
                                </tr>                             
                                                                                              
                          </table>
             
               
             </div>  <%--<div class="panel panel-default">--%>
             
         </div>    <%--<div class="panel-group">--%>

       </div> <%--<div class="col-md-12">--%>

     </div> <%--<div class="row invoice-info">--%>

 </asp:Content>

