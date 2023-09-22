<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_viajePrint.aspx.vb" Inherits="RMS_APPROVAL.frm_viajePrint" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .center
        {
            text-align: center;
        }

        td
        {
            font-family: Arial !important;
        }

        .subtituloPAR
        {
            font-size: 14px;
            font-weight: 600;
        }

        .RadGrid_Simple, .RadGrid_Simple .rgMasterTable, .RadGrid_Simple .rgDetailTable, .RadGrid_Simple .rgGroupPanel table,
        .RadGrid_Simple .rgCommandRow table, .RadGrid_Simple .rgEditForm table, .RadGrid_Simple .rgPager table, .GridToolTip_Simple
        {
            font-family: Arial !important;
        }
    </style>
    <table cellpadding="0"
        style="width: 980px; border-collapse: collapse; border: 1px solid #9A7328;">
        <tr>
            <td style="background-color: #FFFFFF">


                <table cellpadding="0"
                    style="border-collapse: collapse; width: 85%; border-color: #9a7328; border-width: 0">
                    <tr>
                        <td style="width: 24px; height: 10px"></td>
                        <td style="height: 10px"></td>
                        <td style="text-align: center; height: 10px;"></td>
                        <td style="text-align: right; height: 10px;"></td>
                    </tr>
                    <tr>
                        <td style="width: 24px">&nbsp;</td>
                        <td>
                            <asp:image runat="server" ID="imgChemo" style="max-height: 70px;" ImageUrl="~/images/activities/logo_Chemonics_nw.png"  class="img-rounded" />
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="17px" ForeColor="Red"
                                Text="INCLUSIVE JUSTICE (JUSTICIA INCLUSIVA)"></asp:Label>
                            <br />
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Solicitud de viajes</asp:Label>
                        </td>
                        <td style="text-align: right">
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 24px; height: 10px"></td>
                        <td style="height: 10px"></td>
                        <td style="text-align: center; height: 10px;"></td>
                        <td style="text-align: right; height: 10px;"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; padding-left: 30px; padding-right: 30px;">
                    <tr>
                        <td style="width: 150px;" class="subtituloPAR">Nombres:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_nombres"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Número de documento</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_numero_documento"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Cargo</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_cargo"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha de solicitud</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_solicitud"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Regional</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_regional"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Código de viaje</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_codigo_viaje"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Estrategía</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_estrategia"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" colspan="2" class="subtituloPAR"></td>

                    </tr>
                    <%--<tr>
                        <td class="subtituloPAR" >Ver en el sistema</td>
                        <td style="border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="Label4"></asp:Label></td>
                        <td colspan="2" style="width: 210px;" class="center">
                            <asp:Label runat="server" ID="Label5"></asp:Label></td>
                    </tr>--%>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Motivo</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_motivo"></asp:Label>
                            
                        </td>
                        
                    </tr>
                  
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">ITINERARIO Y SOLICITUD DE PASAJES</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_itinerario" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_itinerario" ShowFooter="false">
                                    <Columns>
                                         <telerik:GridBoundColumn DataField="fecha_viaje" DataFormatString="{0:MM/dd/yyyy}"
                                                    FilterControlAltText="Filter fecha_viaje column" HeaderStyle-Font-Bold="true"
                                                    HeaderText="Fecha" SortExpression="fecha_viaje"
                                                    UniqueName="colm_fecha_viaje">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="hora_salida"
                                                    FilterControlAltText="Filter hora_salida column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Hora de salida" SortExpression="hora_salida"
                                                    UniqueName="colm_hora_salida">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ciudad_origen" 
                                                    FilterControlAltText="Filter ciudad_origen column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Ciudad de origen" SortExpression="ciudad_origen"
                                                    UniqueName="colm_ciudad_origen">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ciudad_destino"
                                                    FilterControlAltText="Filter ciudad_destino column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Ciudad de destino" SortExpression="ciudad_destino"
                                                    UniqueName="colm_ciudad_destino">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="requiere_transporte_aereo_text"
                                                    FilterControlAltText="Filter requiere_transporte_aereo_text column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Transporte aéreo" SortExpression="requiere_transporte_aereo_text"
                                                    UniqueName="colm_requiere_transporte_aereo_text">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="requiere_vehiculo_proyecto_text"
                                                    FilterControlAltText="Filter requiere_vehiculo_proyecto_text column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Vehiculo del proyecto" SortExpression="requiere_vehiculo_proyecto_text"
                                                    UniqueName="colm_requiere_vehiculo_proyecto_text">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="requiere_transporte_fluvial_text" 
                                                    FilterControlAltText="Filter requiere_transporte_fluvial_text column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Transporte fluvial" SortExpression="requiere_transporte_fluvial_text"
                                                    UniqueName="colm_requiere_transporte_fluvial_text">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="requiere_servicio_publico_text"
                                                    FilterControlAltText="Filter requiere_servicio_publico_text column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Servicio público" SortExpression="requiere_servicio_publico_text"
                                                    UniqueName="colm_requiere_servicio_publico_text">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR"><hr /></td>
                    </tr>
                     <tr>
                        <td colspan="4" class="subtituloPAR">REQUERIMIENTO DE HOTEL</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_hotel" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_hotel" ShowFooter="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_viaje_hotel"
                                                FilterControlAltText="Filter id_viaje_hotel column"
                                                SortExpression="id_viaje_hotel" UniqueName="id_viaje_hotel"
                                                Visible="False" HeaderText="id_viaje_hotel"  HeaderStyle-Font-Bold="true"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="numero_factura"
                                                FilterControlAltText="Filter numero_factura column"  HeaderStyle-Font-Bold="true"
                                                SortExpression="numero_factura" UniqueName="numero_factura"
                                                Visible="False" HeaderText="numero_factura"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total"
                                                FilterControlAltText="Filter valor_total column"  HeaderStyle-Font-Bold="true"
                                                SortExpression="valor_total" UniqueName="valor_total"
                                                Visible="False" HeaderText="valor_total"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_llegada"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha_llegada column" HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha de llegada" SortExpression="fecha_llegada"
                                                UniqueName="colm_fecha_llegada">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_salida"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha_salida column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha de salida" SortExpression="fecha_salida"
                                                UniqueName="colm_fecha_salida">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ciudad"
                                                FilterControlAltText="Filter ciudad column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Ciudad" SortExpression="ciudad"
                                                UniqueName="colm_ciudad">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="hotel"
                                                FilterControlAltText="Filter hotel column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Hotel" SortExpression="hotel"
                                                UniqueName="colm_hotel">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                     <tr>
                            <td colspan="4" class="subtituloPAR">Componentes asociados</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0"
                                    Culture="Spanish (Spain)" GridLines="None"
                                    Skin="Simple" AllowSorting="True">
                                    <MasterTableView AutoGenerateColumns="False" ShowFooter="true">
                                        <Columns>
                                                <telerik:GridBoundColumn DataField="objetivo" FilterControlAltText="Filter objetivo column"
                                                HeaderText="Objetivo" SortExpression="descripcion_logica" UniqueName="objetivo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="sub_objetivo" FilterControlAltText="Filter sub_objetivo column"
                                                HeaderText="Subobjetivo" SortExpression="descripcion_logica" UniqueName="sub_objetivo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="componente" FilterControlAltText="Filter componente column"
                                                HeaderText="Componente" SortExpression="componente" UniqueName="componente">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <br />
                        
                            </td>
                        </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">RUTA DE APROBACIÓN</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_cate" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" ShowFooter="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="orden" visible="false" DataType="System.Int32" FilterControlAltText="Filter orden column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Id" SortExpression="orden" UniqueName="orden">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Rol" SortExpression="nombre_rol" UniqueName="nombre_rol">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Usuario" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Estado" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_aprobacion" 
                                            FilterControlAltText="Filter fecha_aprobacion column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Fecha de aprobación" SortExpression="fecha_aprobacion" 
                                            UniqueName="fecha_aprobacion">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center; font-weight:600;">
                            <hr />
                            ***Esta autorización de viaje no requiere firma autógrafa ya que fue elaborada y aprobada a través del sistema SIME***
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR"><hr /></td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
