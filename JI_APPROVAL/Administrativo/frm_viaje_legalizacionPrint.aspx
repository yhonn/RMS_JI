<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_viaje_legalizacionPrint.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_legalizacionPrint" %>
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

        .textrightalign, .rgFooter td{
            text-align: right;
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
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Legalización de viaje</asp:Label>
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
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha de envío</td>
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
                        <td class="subtituloPAR">Fecha radicación</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_radicacion"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Tasa SER</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_Tasa_ser"></asp:Label></td>
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
                        <td colspan="4" class="subtituloPAR">Motivo del viaje</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_motivo"></asp:Label>
                            
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Fecha y hora de inicio del viaje</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_hora_inicio"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha y hora de finalización del viaje</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_hora_fin"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Valor total de la legalización</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_total_legalizacion"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR"></td>
                        <td style="width: 210px; class="center">
                            <asp:Label runat="server" ID="Label3"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">PASAJES</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_pasajes" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true">
                                    <Columns>
                                         <telerik:GridBoundColumn DataField="fecha" DataFormatString="{0:MM/dd/yyyy}"
                                            FilterControlAltText="Filter fecha fecha" HeaderStyle-Font-Bold="true"
                                            HeaderText="Fecha adquirio el servicio" SortExpression="fecha_viaje"
                                            UniqueName="colm_fecha">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_facturacion"
                                            FilterControlAltText="Filter codigo_facturacion column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                            UniqueName="codigo_facturacion">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nro_rec" 
                                            FilterControlAltText="Filter nro_rec column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="# Rec" SortExpression="nro_rec"
                                            UniqueName="nro_rec">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_gasto"
                                            FilterControlAltText="Filter descripcion_gasto column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                            UniqueName="descripcion_gasto">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_legalizacion_pasajes"
                                            FilterControlAltText="Filter codigo_legalizacion_pasajes column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Code Pasajes" SortExpression="codigo_legalizacion_pasajes"
                                            UniqueName="codigo_legalizacion_pasajes">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="monto_pasajes" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter monto_pasajes column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto pasajes" SortExpression="monto_pasajes"
                                            Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            UniqueName="monto_pasajes">
                                            <ItemStyle CssClass="textrightalign" />
                                            <HeaderStyle CssClass="textrightalign"  />
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
                        <td colspan="4" class="subtituloPAR">REUNIONES</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_reuniones" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha column" HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha adquirio el servicio" SortExpression="fecha"
                                                UniqueName="fecha">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                FilterControlAltText="Filter codigo_facturacion column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                UniqueName="codigo_facturacion">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                FilterControlAltText="Filter nro_rec column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                UniqueName="nro_rec">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="ent_mtg"
                                                FilterControlAltText="Filter ent_mtg column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="ENT/MTG" SortExpression="ent_mtg"
                                                UniqueName="ent_mtg">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="nro_participantes"
                                                FilterControlAltText="Filter nro_participantes column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="# Participantes" SortExpression="nro_participantes"
                                                UniqueName="nro_participantes">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                FilterControlAltText="Filter descripcion_gasto column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                UniqueName="descripcion_gasto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="monto_total"
                                            FilterControlAltText="Filter monto_total column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto" SortExpression="monto_total"
                                            Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            UniqueName="monto_total">
                                            <HeaderStyle CssClass="wrapWord"  />
                                              <ItemStyle CssClass="textrightalign" />
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
                        <td colspan="4" class="subtituloPAR">MISCELANEOS</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_miscelaneos" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha column" HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha adquirio el servicio" SortExpression="fecha"
                                                UniqueName="fecha">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                FilterControlAltText="Filter codigo_facturacion column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                UniqueName="codigo_facturacion">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                FilterControlAltText="Filter nro_rec column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                UniqueName="nro_rec">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                FilterControlAltText="Filter descripcion_gasto column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                UniqueName="descripcion_gasto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="monto_total"
                                            FilterControlAltText="Filter monto_total column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto" SortExpression="monto_total"
                                            Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            UniqueName="monto_total">
                                            <HeaderStyle CssClass="wrapWord"  />
                                              <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4" class="subtituloPAR">ALIMENTACIÓN Y ALOJAMIENTO</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_alimentacion_alojamiento" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_legalizacion_viaje_detalle" ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true">
                                    <Columns>
                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha column" HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha adquirio el servicio" SortExpression="fecha"
                                                UniqueName="fecha">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_facturacion"
                                                FilterControlAltText="Filter codigo_facturacion column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Código de facturación" SortExpression="codigo_facturacion"
                                                UniqueName="codigo_facturacion">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nro_rec"
                                                FilterControlAltText="Filter nro_rec column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="# Rec" SortExpression="nro_rec"
                                                UniqueName="nro_rec">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ubicacion_alojamiento"
                                                FilterControlAltText="Filter ubicacion_alojamiento column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Lugar" SortExpression="ubicacion_alojamiento"
                                                UniqueName="ubicacion_alojamiento">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_gasto"
                                                FilterControlAltText="Filter descripcion_gasto column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Descripción del gasto" SortExpression="descripcion_gasto"
                                                UniqueName="descripcion_gasto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="porcentaje_perdiem"
                                            FilterControlAltText="Filter porcentaje_perdiem column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="% viatico" SortExpression="porcentaje_perdiem"
                                            UniqueName="porcentaje_perdiem">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <%-- <telerik:GridBoundColumn DataField="valor_perdiem"
                                            FilterControlAltText="Filter valor_perdiem column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="M&IE Rate" SortExpression="valor_perdiem"
                                            DataFormatString="{0:n0}"
                                            UniqueName="valor_perdiem">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>--%>
                                         <telerik:GridBoundColumn DataField="valor_perdiem"
                                            FilterControlAltText="Filter valor_perdiem column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Tarifa base" SortExpression="valor_perdiem"
                                            DataFormatString="{0:n0}"
                                            UniqueName="valor_perdiem">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="numero_dias"
                                            FilterControlAltText="Filter numero_dias column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="# días" SortExpression="numero_dias"
                                            UniqueName="numero_dias">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descuento_alimentacion" Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            FilterControlAltText="Filter descuento_alimentacion column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Descuento alimentación" SortExpression="descuento_alimentacion"
                                            DataFormatString="{0:n0}"
                                            UniqueName="descuento_alimentacion">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="valor_total_alimentacion" Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            FilterControlAltText="Filter valor_total_alimentacion column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto alimentación" SortExpression="valor_total_alimentacion"
                                            DataFormatString="{0:n0}"
                                            UniqueName="valor_total_alimentacion">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="monto_alojamiento" Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            FilterControlAltText="Filter monto_alojamiento column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto alojamiento por noche" SortExpression="monto_alojamiento"
                                            DataFormatString="{0:n0}"
                                            UniqueName="monto_alojamiento">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="monto_total"  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter monto_total column"  HeaderStyle-Font-Bold="true"
                                            HeaderText="Monto total" SortExpression="monto_total"
                                            Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                            UniqueName="monto_total">
                                            <HeaderStyle CssClass="wrapWord"  />
                                              <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>

                    
                    <tr>
                        <td colspan="4" class="subtituloPAR">SOPORTES DE LA LEGALIZACIÓN</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_soportes" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_soporte_legalizacion_viaje" ShowFooter="false">
                                    <Columns>
                                            <telerik:GridBoundColumn DataField="fecha"  DataFormatString="{0:MM/dd/yyyy}"
                                                FilterControlAltText="Filter fecha column" HeaderStyle-Font-Bold="true"
                                                HeaderText="Fecha" SortExpression="fecha"
                                                UniqueName="fecha">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="tipo_soporte_legalizacion"
                                                FilterControlAltText="Filter tipo_soporte_legalizacion column"  HeaderStyle-Font-Bold="true"
                                                HeaderText="Tipo de soporte" SortExpression="tipo_soporte_legalizacion"
                                                UniqueName="tipo_soporte_legalizacion">
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

                       <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td colspan="4" class="subtituloPAR">COMENTARIOS DE LA APROBACIÓN</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_comentarios" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" ShowFooter="false">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="fecha_comentario" FilterControlAltText="Filter fecha_comentario column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Fecha" SortExpression="fecha_comentario" UniqueName="fecha_comentario">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="empleado" FilterControlAltText="Filter empleado column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Usuario" SortExpression="empleado" UniqueName="empleado">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="comentario" FilterControlAltText="Filter comentario column" HeaderStyle-Font-Bold="true"
                                            HeaderText="Comentario" SortExpression="comentario" UniqueName="comentario">
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
                            <br />
                        </td>
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
                            ***Esta legalización de viaje no requiere firma autógrafa ya que fue elaborada y aprobada a través del sistema SIME***
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
