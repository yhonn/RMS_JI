<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_facturacionDetalle.aspx.vb" Inherits="RMS_APPROVAL.frm_facturacionDetalle" %>
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
        #pagina {
           border-collapse: collapse;
        }

        /* And this to your table's `td` elements. */
        #page td {
           padding: 0; 
           margin: 0;
        }
        .textrightalign, .rgFooter td{
            text-align: right;
        }
    </style>
    <table cellpadding="0"
        style="width: 980px; border-collapse: collapse; border: 0px solid #c3c3c3;">
        <tr>
            <td style="background-color: #FFFFFF">


                <table id="pagina" style="width: 90%; margin:0 auto; padding-left: 30px; padding-right: 30px; border: 1px solid #c3c3c3;">
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" id="sjs-A1" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="width: 40%; border-right: 1px solid #c3c3c3;"><asp:image runat="server" ID="imgChemo" style="max-height: 70px;" ImageUrl="~/images/activities/logo_Chemonics_nw.png"  class="img-rounded" /></td>
                                    <td style="width: 60%; padding: 10px;">
                                         <span style="font-size:8.5pt;">
                                              <baseline>NIT 900.480.566 - 1<br/></baseline>
                                           </span>
                                           <span style="font-size:8pt;">
                                              <baseline>CHEMONICS INTERNATIONAL INC SUCURSAL COLOMBIA<br/>PROGRAMA: </baseline>
                                           </span>
                                           <span style="font-size:8pt;">
                                              <b>JUSTICIA INCLUSIVA</b>
                                           </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="text-align: center; padding: 10px;">
                                        <b>
                                            <asp:label id="resolucion" runat="server"></asp:label>
                                        </b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr >
                                    <td rowspan="2" style="border-right: 1px solid #c3c3c3; width: 200px; text-align: center;">
                                        FECHA:
                                    </td>
                                    <td style="border-right: 1px solid #c3c3c3; border-bottom:1px solid #c3c3c3; text-align: center;">D</td>
                                    <td style="border-right: 1px solid #c3c3c3; border-bottom:1px solid #c3c3c3; text-align: center;">M</td>
                                    <td style="border-right: 1px solid #c3c3c3; border-bottom:1px solid #c3c3c3; text-align: center;">A</td>
                                     <td rowspan="2" style=" padding:8px;">
                                        NÚMERO:  <asp:Label runat="server" ID="lbl_numero_factura"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center;"><asp:Label runat="server" ID="lbl_dia"></asp:Label></td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center;"><asp:Label runat="server" ID="lbl_mes"></asp:Label></td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center;"><asp:Label runat="server" ID="lbl_anio"></asp:Label></td>
                                   
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 200px; padding:8px;">
                                        Nombre:
                                    </td>
                                    <td style="text-align: left; padding:8px;">
                                        <asp:Label runat="server" ID="lbl_nombre"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 180px; padding:8px;">
                                        Dirección:
                                    </td>
                                    <td style="text-align: left;border-right: 1px solid #c3c3c3; width: 180px; padding:8px; ">
                                        <asp:Label runat="server" ID="lbl_direccion"></asp:Label>
                                    </td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 200px; padding:8px;">
                                        Ciudad:
                                    </td>
                                    <td style="text-align: left;border-right: 1px solid #c3c3c3; width: 180px; padding:8px; ">
                                        <asp:Label runat="server" ID="lbl_departamento"></asp:Label>
                                    </td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 200px; padding:8px;">
                                        Teléfono:
                                    </td>
                                    <td style="text-align: left; padding:8px; width: 180px;">
                                        <asp:Label runat="server" ID="lbl_telefono"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 260px; padding:8px;">
                                        DOCUMENTO DE IDENTIDAD Y/O NIT :
                                    </td>
                                    <td style="text-align: left;border-right: 1px solid #c3c3c3; padding:8px; ">
                                        <asp:Label runat="server" ID="lbl_numero_documento"></asp:Label>
                                    </td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 100px; padding:8px;">
                                        CELULAR:
                                    </td>
                                    <td style="text-align: left;border-right: 1px solid #c3c3c3; padding:8px; ">
                                        <asp:Label runat="server" ID="lbl_celular"></asp:Label>
                                    </td>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 100px; padding:8px;">
                                        Código postal:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:Label runat="server" ID="lbl_codigo_postal"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center; width: 200px; padding:8px;">
                                        Correo electronico:
                                    </td>
                                    <td style="text-align: left; padding:8px;">
                                        <asp:Label runat="server" ID="lbl_correo"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td style="border-right: 1px solid #c3c3c3; text-align: center;">
                                       <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowFooter="true" 
                                            ShowColumnFooters="true"
                                            ShowGroupFooters="true"
                                            ShowGroupPanel="false" GridLines="None"
                                            Skin="Simple">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="false" AllowResizeToFit="false" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_facturacion_producto" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_facturacion_producto"
                                                        FilterControlAltText="Filter id_facturacion_producto column"
                                                        SortExpression="id_facturacion_producto" UniqueName="id_facturacion_producto"
                                                        Visible="False" HeaderText="id_facturacion_producto"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="cantidad" HeaderStyle-Font-Bold="true"
                                                        FilterControlAltText="Filter cantidad column" HeaderStyle-Width="130px"
                                                        HeaderText="Cantidad" SortExpression="cantidad"
                                                        UniqueName="colm_cantidad">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion" HeaderStyle-Font-Bold="true"
                                                        FilterControlAltText="Filter descripcion column" HeaderStyle-Width="330px"
                                                        HeaderText="Descripción / Concepto" SortExpression="descripcion"
                                                        UniqueName="colm_descripcion">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="valor_unitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                        DataFormatString="{0:n}"  HeaderStyle-Font-Bold="true"
                                                        FilterControlAltText="Filter valor_unitario column"  HeaderStyle-Width="210px"
                                                        HeaderText="Valor unitario" SortExpression="valor_unitario"
                                                        UniqueName="colm_valor_unitario">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                        <ItemStyle CssClass="textrightalign" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                        Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                        FilterControlAltText="Filter valor column"  HeaderStyle-Width="200px"
                                                        HeaderText="Valor total" SortExpression="valor" HeaderStyle-Font-Bold="true"
                                                        UniqueName="colm_valor">
                                                        <ItemStyle CssClass="textrightalign" />
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr>
                                    <td colspan="2" rowspan="2" style="border-right: 1px solid #c3c3c3; text-align: left; width: 490px; padding: 8px;">
                                        EL PRESENTE DOCUMENTO SE EXPIDE DE CONFORMIDAD CON LO DISPUESTO POR EL ART. 55 DE LA RESOLUCION 42 DE MAYO 5  DE 2020
                                    </td>
                                    <td style="width: 195px; border-bottom: 1px solid #c3c3c3; border-right: 1px solid #c3c3c3; padding: 8px;">
                                        <b>RETE ICA ______/1000</b>
                                    </td>
                                    <td style="width: 185px; border-bottom: 1px solid #c3c3c3; padding: 8px;">
                                        
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width: 195px; border-right: 1px solid #c3c3c3; padding: 8px;">
                                        <b>RETENCIÓN EN LA FUENTE</b>
                                    </td>
                                    <td style="width: 185px; padding: 8px;">
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="border: 1px solid #c3c3c3;">
                        <td colspan="8" style="border: 1px solid #c3c3c3;">
                            <table style="border-collapse: collapse; width: 100%">
                                <tr style="height: 80px;">
                                    <td style="width: 325px; border-right: 1px solid #c3c3c3; text-align:center;" valign="bottom">
                                        FIRMA DE AUTORIZADO Y/O VO. BO.
                                    </td>
                                    <td style="width: 325px; border-right: 1px solid #c3c3c3; text-align: center;" valign="bottom">
                                        FIRMA BENEFICIARIO DEL PAGO.
                                    </td>
                                    <td style="width: 325px;">
                                        
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>
