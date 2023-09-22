<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_anticipoDetalle.aspx.vb" Inherits="RMS_APPROVAL.frm_anticipoDetalle" %>
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

        .text-bold{
            font-weight: bold;
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
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Solicitud de anticipo</asp:Label>
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
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha requiere el anticipo</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_solicitud"></asp:Label></td>
                    </tr>
                    <tr>
                       <td class="subtituloPAR">PAR</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_par"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Código PT</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_codigo_anticipo"></asp:Label></td>
                    </tr>
                     <tr>
                        
                        <td class="subtituloPAR">Regional</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_regional"></asp:Label></td>
                         <td style="width: 150px; padding-left: 20px"  class="subtituloPAR">Tipo de anticipo</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_tipo_Anticipo"></asp:Label></td>
                    </tr>
                     <tr>
                        
                        <td class="subtituloPAR">Medio de pago</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_medio_pago"></asp:Label></td>
                         <td style="width: 150px; padding-left: 20px"  class="subtituloPAR">Observaciones medio de pago</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_observaciones_mp"></asp:Label></td>
                    </tr>
                     <tr>
                        
                        <td class="subtituloPAR">Costo total comisión giro</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_comision_mp"></asp:Label></td>
                        <td></td>
                         <td></td>
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
                    <tr runat="server" id="eventos">
                        <td colspan="4">
                            <table  cellpadding="0" style="width: 100%; border-collapse: collapse;">
                                <tr> <td colspan="4" class="subtituloPAR">Rutas</td> </tr>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="grd_rutas" runat="server" CellSpacing="0"
                                            Culture="Spanish (Spain)" GridLines="None"
                                            Skin="Simple" AllowSorting="True">
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_ruta" ShowFooter="true" 
                                                    ShowColumnFooters="true"
                                                    ShowGroupFooters="true">
                                                <Columns>
                                                      <telerik:GridBoundColumn DataField="num_ruta"
                                                                FilterControlAltText="Filter num_ruta column"  HeaderStyle-Font-Bold="true"
                                                                HeaderText="No" SortExpression="num_ruta"
                                                                UniqueName="colm_num_ruta">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                                                  
                                                            <telerik:GridBoundColumn DataField="ciudad_salida" 
                                                                FilterControlAltText="Filter ciudad_salida column"  HeaderStyle-Font-Bold="true"
                                                                HeaderText="Municipio / ciudad de salida" SortExpression="ciudad_salida"
                                                                UniqueName="colm_ciudad_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="zona_rural"
                                                                FilterControlAltText="Filter zona_rural column"  HeaderStyle-Font-Bold="true"
                                                                HeaderText="Zona rural" SortExpression="zona_rural"
                                                                UniqueName="colm_zona_rural">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ciudad_llegada"
                                                                FilterControlAltText="Filter ciudad_llegada column"  HeaderStyle-Font-Bold="true"
                                                                HeaderText="Municipio / ciudad de llegada" SortExpression="ciudad_llegada"
                                                                UniqueName="colm_ciudad_llegada">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:gridtemplatecolumn headerstyle-width="150px" HeaderStyle-Font-Bold="true"
                                                            headertext="Información adicional" uniquename="informacion_adicional">
                                                            <itemtemplate>
                                                                <%--<asp:Label runat="server" ID="lblt_rms_code" CssClass="control-label text-bold" Text="Código RMS: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_rms_code" CssClass="control-label" Text=""></asp:Label><br />--%>
                                                                <asp:Label runat="server" ID="lblt_tiempo_estimado" CssClass="control-label text-bold" Text="Tiepo estimado: "></asp:Label>
                                                                <br />
                                                                &nbsp;<asp:Label runat="server" ID="lbl_tiempo_estimado" CssClass="control-label" Text=""></asp:Label><br />
                                                                <asp:Label runat="server" ID="lblt_observaciones_trayecto" CssClass="control-label text-bold" Text="Observaciones trayecto: "></asp:Label>
                                                                <br />
                                                                &nbsp;<asp:Label runat="server" ID="lbl_observaciones_trayecto" CssClass="control-label" Text=""></asp:Label><br />
                                                                <asp:Label runat="server" ID="lblt_observaciones_ruta" CssClass="control-label text-bold" Text="Observaciones adicionales: "></asp:Label>
                                                                <br />
                                                                &nbsp;<asp:Label runat="server" ID="lbl_observaciones_ruta" CssClass="control-label" Text=""></asp:Label>
                                                                <br />

                                                            </itemtemplate>
                                                            <headerstyle width="150px" />
                                                            <itemstyle width="150px" />
                                                        </telerik:gridtemplatecolumn>
                                                    <telerik:gridboundcolumn datafield="tiempo_estimado"
                                                            filtercontrolalttext="Filter tiempo_estimado column" headertext="tiempo_estimado" uniquename="tiempo_estimado" visible="true" display="false">
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="observaciones_ruta"
                                                            filtercontrolalttext="Filter observaciones_ruta column" headertext="observaciones_ruta" uniquename="observaciones_ruta" visible="true" display="false">
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="observaciones_trayecto"
                                                            filtercontrolalttext="Filter observaciones_trayecto column" headertext="observaciones_trayecto" uniquename="observaciones_trayecto" visible="true" display="false">
                                                        </telerik:gridboundcolumn>


                                                            <telerik:GridBoundColumn DataField="cantidad_personas" 
                                                                FilterControlAltText="Filter cantidad_personas column"  HeaderStyle-Font-Bold="true"
                                                                HeaderText="# personas solicitud" SortExpression="cantidad_personas"
                                                                UniqueName="colm_cantidad_personas">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                          <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}"  HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                            headertext="Valor ida y regreso" sortexpression="valor_trayecto"
                                                            uniquename="colm_valor_trayecto">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="sub_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor total ida y regreso: {0:n}</b>" footertext="Valor total ida y regreso: "
                                                            filtercontrolalttext="Filter sub_total_ruta column" headerstyle-width="120px"  HeaderStyle-Font-Bold="true"
                                                            headertext="Valor total ida y regreso" sortexpression="sub_total_ruta"
                                                            uniquename="colm_valor_trayecto_">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="valor_total_estipendio" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor estipendio: {0:n}</b>" footertext="Valor estipendio: "
                                                            filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"  HeaderStyle-Font-Bold="true"
                                                            headertext="Valor estipendio" sortexpression="valor_total_estipendio"
                                                            uniquename="colm_valor_total_estipendio">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="valor_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor total: {0:n}</b>" footertext="Valor total: "
                                                            filtercontrolalttext="Filter valor_total_ruta column" headerstyle-width="90px"  HeaderStyle-Font-Bold="true"
                                                            headertext="Valor total" sortexpression="valor_total_ruta"
                                                            uniquename="colm_valor_total_ruta">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                                 <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                <tr> <td colspan="4" class="subtituloPAR">Listado preliminar de participantes</td> </tr>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="grd_participantes" runat="server" CellSpacing="0"
                                            Culture="Spanish (Spain)" GridLines="None"
                                            Skin="Simple" AllowSorting="True">
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_participante" ShowFooter="true">
                                                <Columns>
                                                    <telerik:gridboundcolumn datafield="num_ruta" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter num_ruta column" headerstyle-width="135px"
                                                            headertext="No Ruta" sortexpression="num_ruta"
                                                            uniquename="colm_num_ruta">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                      <telerik:gridboundcolumn datafield="tipo_ocumento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter tipo_ocumento column" headerstyle-width="135px"
                                                            headertext="Tipo de documento" sortexpression="tipo_ocumento"
                                                            uniquename="colm_tipo_ocumento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="numero_documento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter numero_documento column" headerstyle-width="135px"
                                                            headertext="Número de documento" sortexpression="numero_documento"
                                                            uniquename="colm_numero_documento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="nombres" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter nombres column" headerstyle-width="120px"
                                                            headertext="Nombres" sortexpression="nombres"
                                                            uniquename="colm_nombres">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="primer_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter primer_apellido column" headerstyle-width="135px"
                                                            headertext="Primer apellido" sortexpression="primer_apellido"
                                                            uniquename="colm_primer_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="segundo_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="135px"
                                                            headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                            uniquename="colm_segundo_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="telefono" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter telefono column" headerstyle-width="135px"
                                                            headertext="Teléfono" sortexpression="telefono"
                                                            uniquename="colm_telefono_">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="retiro_fondos_text" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter retiro_fondos_text column" headerstyle-width="135px"
                                                            headertext="Retiro los fondos" sortexpression="retiro_fondos_text"
                                                            uniquename="colm_retiro_fondos_text">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>

                                                         <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                            headertext="Valor transporte" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_trayecto">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                         <telerik:gridboundcolumn datafield="valor_estipendio" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_estipendio column" headerstyle-width="110px"
                                                            headertext="Valor alimentación" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_estipendio">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor column" headerstyle-width="110px" HeaderStyle-Font-Bold="true"
                                                            headertext="Valor" sortexpression="valor"
                                                            uniquename="colm_valor">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                     
                                </tr>

                                 <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>

                                <tr> <td colspan="4" class="subtituloPAR">Participantes que retiraron fondos / asistieron</td> </tr>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="grd_participantes_asistieron" runat="server" CellSpacing="0"
                                            Culture="Spanish (Spain)" GridLines="None"
                                            Skin="Simple" AllowSorting="True">
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_participante" ShowFooter="true">
                                                <Columns>
                                                    <telerik:gridboundcolumn datafield="num_ruta" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter num_ruta column" headerstyle-width="135px"
                                                            headertext="No Ruta" sortexpression="num_ruta"
                                                            uniquename="colm_num_ruta">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                      <telerik:gridboundcolumn datafield="tipo_ocumento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter tipo_ocumento column" headerstyle-width="135px"
                                                            headertext="Tipo de documento" sortexpression="tipo_ocumento"
                                                            uniquename="colm_tipo_ocumento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="numero_documento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter numero_documento column" headerstyle-width="135px"
                                                            headertext="Número de documento" sortexpression="numero_documento"
                                                            uniquename="colm_numero_documento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="nombres" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter nombres column" headerstyle-width="120px"
                                                            headertext="Nombres" sortexpression="nombres"
                                                            uniquename="colm_nombres">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="primer_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter primer_apellido column" headerstyle-width="135px"
                                                            headertext="Primer apellido" sortexpression="primer_apellido"
                                                            uniquename="colm_primer_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="segundo_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="135px"
                                                            headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                            uniquename="colm_segundo_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="telefono" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter telefono column" headerstyle-width="135px"
                                                            headertext="Teléfono" sortexpression="telefono"
                                                            uniquename="colm_telefono_">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                      <%--  <telerik:gridboundcolumn datafield="retiro_fondos_text" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter retiro_fondos_text column" headerstyle-width="135px"
                                                            headertext="Retiro los fondos" sortexpression="retiro_fondos_text"
                                                            uniquename="colm_retiro_fondos_text">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>--%>

                                                         <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                            headertext="Valor transporte" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_trayecto">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                         <telerik:gridboundcolumn datafield="valor_estipendio" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_estipendio column" headerstyle-width="110px"
                                                            headertext="Valor alimentación" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_estipendio">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor column" headerstyle-width="110px" HeaderStyle-Font-Bold="true"
                                                            headertext="Valor" sortexpression="valor"
                                                            uniquename="colm_valor">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                     
                                </tr>
                                 <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                 <tr> <td colspan="4" class="subtituloPAR">Participantes que no retiraron fondos / no asistieron</td> </tr>
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="grd_no_asistieron" runat="server" CellSpacing="0"
                                            Culture="Spanish (Spain)" GridLines="None"
                                            Skin="Simple" AllowSorting="True">
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_participante" ShowFooter="true">
                                                <Columns>
                                                    <telerik:gridboundcolumn datafield="num_ruta" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter num_ruta column" headerstyle-width="135px"
                                                            headertext="No Ruta" sortexpression="num_ruta"
                                                            uniquename="colm_num_ruta">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                      <telerik:gridboundcolumn datafield="tipo_ocumento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter tipo_ocumento column" headerstyle-width="135px"
                                                            headertext="Tipo de documento" sortexpression="tipo_ocumento"
                                                            uniquename="colm_tipo_ocumento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="numero_documento" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter numero_documento column" headerstyle-width="135px"
                                                            headertext="Número de documento" sortexpression="numero_documento"
                                                            uniquename="colm_numero_documento">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="nombres" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter nombres column" headerstyle-width="120px"
                                                            headertext="Nombres" sortexpression="nombres"
                                                            uniquename="colm_nombres">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="primer_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter primer_apellido column" headerstyle-width="135px"
                                                            headertext="Primer apellido" sortexpression="primer_apellido"
                                                            uniquename="colm_primer_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="segundo_apellido" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="135px"
                                                            headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                            uniquename="colm_segundo_apellido">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="telefono" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter telefono column" headerstyle-width="135px"
                                                            headertext="Teléfono" sortexpression="telefono"
                                                            uniquename="colm_telefono_">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                      <%--  <telerik:gridboundcolumn datafield="retiro_fondos_text" HeaderStyle-Font-Bold="true"
                                                            filtercontrolalttext="Filter retiro_fondos_text column" headerstyle-width="135px"
                                                            headertext="Retiro los fondos" sortexpression="retiro_fondos_text"
                                                            uniquename="colm_retiro_fondos_text">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>--%>

                                                         <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                            headertext="Valor transporte" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_trayecto">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                         <telerik:gridboundcolumn datafield="valor_estipendio" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor_estipendio column" headerstyle-width="110px"
                                                            headertext="Valor alimentación" sortexpression="valor" HeaderStyle-Font-Bold="true"
                                                            uniquename="colm_valor_estipendio">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                        <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                            dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                            filtercontrolalttext="Filter valor column" headerstyle-width="110px" HeaderStyle-Font-Bold="true"
                                                            headertext="Valor" sortexpression="valor"
                                                            uniquename="colm_valor">
                                                            <headerstyle cssclass="wrapWord" />
                                                        </telerik:gridboundcolumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                     
                                </tr>
                                  <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                 <tr>
                                    <td colspan="4" class="subtituloPAR">Observaciones de la dispersión de los fondos</td>
                                </tr>
                                <tr>
                                    <td>
                                         <asp:Label runat="server" ID="lbl_observaciones"></asp:Label>
                            
                                    </td>
                        
                                </tr>
                                 <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                <tr>
                                    <td  class="subtituloPAR">Soporte dispersión de los fondos</td>
                                </tr>
                                <tr>
                                    <td >
                                          <asp:HyperLink id="doc_df" 
                                            NavigateUrl="#"
                                            Target="_blank"
                                            Text="Ver soporte dispersión de los fondos"
                                            runat="server"/> 
                            
                                    </td>
                        
                                </tr>
                                 <tr>
                                        <td>
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                 <tr>
                                    <td  class="subtituloPAR">Soporte legalización</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                          <asp:HyperLink id="doc_legalizacion" 
                                            NavigateUrl="#"
                                            Target="_blank"
                                            Text="Ver soporte legalización"
                                            runat="server"/> 
                            
                                    </td>
                        
                                </tr>
                            </table>
                        </td>
                        
                    </tr>
                    
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                  
                        
                    <tr>
                        <td colspan="4" class="subtituloPAR">RUTA DE APROBACIÓN SOLICITUD DE ANTICIPO</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_ruta_solicitud" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None" width="100%"
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

                      <tr runat="server" id="compras">
                        <td colspan="4">
                            <table  cellpadding="0" style="width: 100%; border-collapse: collapse;">
                                <tr> <td colspan="4" class="subtituloPAR">ITEMS / SERVICIOS REQUERIDOS</td> </tr>
                                <tr>
                                    <td>
                                         <telerik:RadGrid ID="grd_compras" runat="server" CellSpacing="0"
                                            Culture="Spanish (Spain)" GridLines="None"
                                            Skin="Simple" AllowSorting="True">
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_compra" ShowFooter="false">
                                                <Columns>
                                                     <telerik:gridboundcolumn datafield="cantidad"
                                                        filtercontrolalttext="Filter cantidad column" headerstyle-width="19%"
                                                        headertext="Cantidad" sortexpression="cantidad"
                                                        uniquename="colm_cantidad">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="descripcion"
                                                        filtercontrolalttext="Filter descripcion column" headerstyle-width="47%"
                                                        headertext="Descripción / Servicio requerido" sortexpression="descripcion"
                                                        uniquename="colm_descripcion">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="valor_unitario" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                        dataformatstring="{0:n0}"
                                                        filtercontrolalttext="Filter valor_unitario column" headerstyle-width="13%"
                                                        headertext="Valor unitario" sortexpression="valor_unitario"
                                                        uniquename="colm_valor_unitario">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                        aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor total: {0:n0}</b>" footertext="Valor total: "
                                                        filtercontrolalttext="Filter valor column" headerstyle-width="19%"
                                                        headertext="Valor total" sortexpression="valor"
                                                        uniquename="colm_valor">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                    </tr>

                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                  
                        
                    <tr>
                        <td colspan="4" class="subtituloPAR"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">RUTA DE APROBACIÓN LEGALIZACIÓN ANTICIPO</td>
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

