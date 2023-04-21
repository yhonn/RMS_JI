<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_hito_entregablePrint.aspx.vb" Inherits="RMS_APPROVAL.frm_hito_entregablePrint" %>
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
                            <asp:Label ID="lbl_programa" runat="server" Font-Bold="False" Font-Size="17px" Visible="false"></asp:Label>
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Contratos - Entregable</asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Image ID="Image2" runat="server" Visible="false"
                                ImageUrl="~/Imagenes/logos/LogoColombiaResponde.png" />
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
        <tr style="display: none;">
            <td style="font-weight: bold; background-color: #C9C9C9">&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="12px"
                            Text="Ficha de proyecto"></asp:Label>
                <asp:Label ID="lbl_id_proyecto" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="X-Small" ForeColor="#C00000"
                    Visible="False"></asp:Label>
                <asp:Label ID="lbl_id_componente" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="X-Small" ForeColor="#C00000"
                    Visible="False"></asp:Label>
                <hr />

            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; padding-left: 30px; padding-right: 30px;">
                     <tr>
                        <td class="subtituloPAR">Código del convenio:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_codigo"></asp:Label></td>
                        <td style="padding-left: 20px" class="subtituloPAR">Valor del convenio:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center"><asp:Label runat="server" ID="lbl_valor_contrato"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">Nombre ejecutor:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_contratista"></asp:Label></td>
                        <td style="padding-left: 20px" class="subtituloPAR">Supervisor:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center"><asp:Label runat="server" ID="lbl_supervisor"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 150px;" class="subtituloPAR">Fecha de inicio del convenio:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_inicio"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha de finalización del convenio:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_fin"></asp:Label></td>
                    </tr>
                  
                     <tr>
                        <td class="subtituloPAR">Número de entregable:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_numero_entregable"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Valor del entregable:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_valor_entregable"></asp:Label></td>
                    </tr>
                      <tr>
                        <td class="subtituloPAR">Fecha esperada de entrega:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_esperada_entrega"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha de entrega:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_entrega"></asp:Label></td>
                    </tr>
                   <tr>
                        <td class="subtituloPAR">Regional:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_regional"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR" colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Nombre del convenio:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label runat="server" ID="lbl_objeto"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Entregable
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label runat="server" ID="lbl_producto"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Productos
                        </td>
                    </tr>
                    <tr>
                       <td colspan="4">
                           <telerik:RadGrid ID="grd_productos" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_entregable_hito" ShowFooter="false">
                                    <Columns>
                                            <telerik:GridBoundColumn DataField="nro_entregable" FilterControlAltText="Filter nro_entregable column"
                                            HeaderText="No." SortExpression="nro_entregable" UniqueName="nro_entregable">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion_entregable" FilterControlAltText="Filter descripcion_entregable column"
                                            HeaderText="Producto" SortExpression="descripcion_entregable" UniqueName="descripcion_entregable">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="unidad_medidad" FilterControlAltText="Filter unidad_medidad column"
                                            HeaderText="Unidad de medida" SortExpression="unidad_medidad" UniqueName="unidad_medidad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" FilterControlAltText="Filter cantidad column"
                                            HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="soporte" FilterControlAltText="Filter soporte column" display="false"
                                            HeaderText="soporte" SortExpression="soporte" UniqueName="soporte">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="col_hlk_view">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlk_view" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                    ToolTip="Soporte" Target="_new">
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Ruta de aprobación
                        </td>
                    </tr>
                    <tr>
                       <td colspan="4">
                           <telerik:RadGrid ID="grd_ruta" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta_hito" ShowFooter="false">
                                    <Columns>
                                            <telerik:GridBoundColumn DataField="responsable" FilterControlAltText="Filter responsable column"
                                            HeaderText="Área encargada" SortExpression="responsable" UniqueName="responsable">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="usuario_aprueba" FilterControlAltText="Filter usuario_aprueba column"
                                            HeaderText="Usuario responsable" SortExpression="usuario_aprueba" UniqueName="usuario_aprueba">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_envio" FilterControlAltText="Filter fecha_envio column"
                                            HeaderText="Fecha de envío" SortExpression="fecha_envio" UniqueName="fecha_envio">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estado_visible" FilterControlAltText="Filter estado_visible column"
                                            HeaderText="Estado" SortExpression="estado_visible" UniqueName="estado_visible">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                       </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center; font-weight:600;">
                            <hr />
                            ***Esta autorización de entregable no requiere firma autógrafa ya que fue elaborada y aprobada a través del sistema SIME***
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>

</asp:Content>
