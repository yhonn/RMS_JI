<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_viaje_facturacionPrint.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_facturacionPrint" %>
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
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Facturaación de viajes</asp:Label>
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
                     <%--<tr>
                        <td class="subtituloPAR">Valor total de facturación</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="Label1"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR"></td>
                        <td style="width: 210px;" class="center">
                            <asp:Label runat="server" ID="Label3"></asp:Label></td>
                    </tr>--%>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Observaciones de facturación</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_observaciones"></asp:Label>
                            
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
                        <td colspan="4" class="subtituloPAR">FACTURACIÓN ALQUILER VEHICULO</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_facturacion_alquiler" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_solicitud_viaje_vehiculo" ShowFooter="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="fecha_radicacion" DataFormatString="{0:MM/dd/yyyy}"
                                            FilterControlAltText="Filter fecha_radicacion column" HeaderStyle-Width="19%"
                                            HeaderText="Fecha de radicación" SortExpression="fecha_radicacion"
                                            UniqueName="fecha_radicacion">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_factura" DataFormatString="{0:n0}"
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
                                        <telerik:GridBoundColumn DataField="soporte"
                                            FilterControlAltText="Filter soporte column"  HeaderStyle-Font-Bold="true"
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
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4" class="subtituloPAR">FACTURACIÓN TIQUETES</td>
                    </tr>
                    <tr>
                         <td colspan="4">
                            <telerik:RadGrid ID="grd_facturacion_tiquetes" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" ShowFooter="false">
                                    <Columns>
                                         <telerik:GridBoundColumn DataField="fecha_radicacion_tiquete" DataFormatString="{0:MM/dd/yyyy}"
                                                    FilterControlAltText="Filter fecha_radicacion_tiquete column" HeaderStyle-Font-Bold="true"
                                                    HeaderText="Fecha de facturación" SortExpression="fecha_radicacion_tiquete"
                                                    UniqueName="fecha_radicacion_tiquete">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="factura_tiquete"
                                                    FilterControlAltText="Filter factura_tiquete column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Número de factura" SortExpression="factura_tiquete"
                                                    UniqueName="factura_tiquete">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="valor_factura_tiquetes" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    FilterControlAltText="Filter valor_factura_tiquetes column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Valor factura" SortExpression="valor_factura_tiquetes"
                                                    DataFormatString="{0:n0}" 
                                                    UniqueName="valor_factura_tiquetes">
                                                    <ItemStyle CssClass="textrightalign" />
                                                    <HeaderStyle CssClass="textrightalign"  />
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridBoundColumn DataField="soporte_tiquetes"
                                                    FilterControlAltText="Filter soporte_tiquetes column"  HeaderStyle-Font-Bold="true"
                                                    HeaderText="Soporte" SortExpression="soporte_tiquetes"
                                                    UniqueName="soporte_tiquetes">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridTemplateColumn UniqueName="colm_soporte" Visible="true">
                                                    <HeaderStyle Width="4%" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="col_hlk_soporte" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                            ToolTip="Soporte" Target="_new">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4" class="subtituloPAR">Itinerario</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_vuelos" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" ShowFooter="false">
                                    <Columns>
                                         
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
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4" class="subtituloPAR">FACTURACIÓN HOTELES</td>
                    </tr>
                     <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_hotel" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False"  ShowFooter="false">
                                    <Columns>
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
                                            <telerik:GridBoundColumn DataField="valor_total" DataFormatString="{0:n0}"
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
                                         <telerik:GridBoundColumn DataField="soporte"
                                            FilterControlAltText="Filter soporte column"  HeaderStyle-Font-Bold="true"
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
                    
                    <tr>
                        <td colspan="4" class="subtituloPAR"><hr /></td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
