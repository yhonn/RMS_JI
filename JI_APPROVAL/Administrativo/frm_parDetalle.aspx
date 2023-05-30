<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_parDetalle.aspx.vb" Inherits="RMS_APPROVAL.frm_parDetalle" %>
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
                            <asp:Label ID="lbl_subtitulo" runat="server" Font-Size="19px" Font-Bold="true">Procurement Action Request (PAR) Form</asp:Label>
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
                        <td style="width: 150px;" class="subtituloPAR">1 Date of request:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_solicitud"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">5 Date Items/Services Needed:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_entrega"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">2 Requested by:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_solicitado"></asp:Label></td>
                        <td style="width: 150px; padding-left: 20px" class="subtituloPAR">6 Requested For/Deliver To:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_aprobado"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">3 Title of Requestor:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_cargo"></asp:Label></td>
                        <td style="padding-left: 20px" class="subtituloPAR">7 Reference Info, if Applicable:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center"><asp:Label runat="server" ID="lbl_codigo_rfa"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="subtituloPAR">4 Office:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_departamento"></asp:Label></td>
                        <td style="padding-left: 20px" class="subtituloPAR">Tipo de PAR:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center"><asp:Label runat="server" ID="lbl_tipo_par"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="subtituloPAR">8 Purpose of PAR:</td>
                        <td colspan="2" class="subtituloPAR">9 Charge To (check one and enter billing code, if applicable):</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                             <asp:RadioButtonList runat="server" ID="rbn_tipo_solicitud" Enabled="false">
                            </asp:RadioButtonList>
                            
                        </td>
                        <td colspan="2"  style="padding-left: 20px">
                            <asp:RadioButtonList runat="server" ID="rbn_cargo_a" Enabled="false">
                            </asp:RadioButtonList>
                           
                           <%-- <asp:RadioButtonList runat="server" ID="rbn_administrativo" Enabled="false">
                                <asp:ListItem>Operations &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_____________________________</asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList runat="server" ID="rbn_tecnico" Enabled="false">
                                <asp:ListItem>In Kind - Grant _____________________________</asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList runat="server" ID="RadioButtonList1" Enabled="false">
                                <asp:ListItem>SAF &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_____________________________</asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList runat="server" ID="RadioButtonList2" Enabled="false">
                                <asp:ListItem>CLIN 2 TO &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;_____________________________</asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList runat="server" ID="rbn_other" Enabled="false">
                            </asp:RadioButtonList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                        <td style="padding-left: 45px">Billing code:</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center"> <asp:Label runat="server" ID="lbl_codigo_facturacion"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2"><span style="font-size: 14px; font-weight: 600;">Asociado a comunicaciones: </span> <asp:Label runat="server" ID="lbl_asociado_comunicaciones"></asp:Label></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                     
                    <tr>
                        <td colspan="3" class="subtituloPAR">10 Items/Services Requered</td>
                         <td class="subtituloPAR" style="text-align:right;"> <asp:Label runat="server" ID="tasa_ser"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_servicios_requeridos" runat="server" CellSpacing="0"
                                Culture="Spanish (Spain)" GridLines="None"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" ShowFooter="true">
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Item #"  HeaderStyle-Font-Bold="true" ItemStyle-Width="5px">
                                            <ItemStyle Width="5px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="numberLabel" runat="server" 

                                            Text='<%#Container.ItemIndex+1%>' Width="10px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter descripcion column" HeaderText="Description/Specifications of Items or Service"
                                            SortExpression="descripcion" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter cantidad column" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Quantity" SortExpression="cantidad" 
                                            UniqueName="cantidad" DataFormatString="{0:n0}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="unidad_medida" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter unidad_medida column"
                                            HeaderText="Units" SortExpression="unidad_medida"
                                            UniqueName="unidad_medida" DataFormatString="{0:n0}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="precio_unitario" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                                            HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter precio_unitario column" FooterText="TOTAL ESTIMATED COP: "
                                            HeaderText="Est. Unit Price COP" SortExpression="precio_unitario" 
                                            UniqueName="precio_unitario" DataFormatString="{0:n0}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_total" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter valor_total column" Aggregate="Sum" FooterAggregateFormatString="{0:n0}"
                                            HeaderText="Est. Total Price COP" SortExpression="valor_total" 
                                            UniqueName="valor_total" DataFormatString="{0:n0}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:Image ID="Imagealert" runat="server" Height="16px" Visible="false"
                                ImageUrl="~/Imagenes/iconos/Informacion2.png"
                                ToolTip="Montos obligados pendiente" />
                            &nbsp;<asp:Label ID="lblproyectoMontoErr" runat="server" Font-Bold="False"
                                Font-Strikeout="False" Font-Underline="False" Visible="false"
                                Text="Actualización pendiente de montos finales del proyecto" Font-Italic="True"></asp:Label>
                            <br />
                        
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4" style="font-size: 11px; font-style: italic">* For transactions above micro-purchase threshold, if final price for purchase exceeds total estimated cost by more than 10%, purchase shall not proceed without approval from approver below. 											
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
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_componente" ShowFooter="false">
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
                    <tr runat="server" visible="false" id="info_evento">
                        <td colspan="4">
                            <table style="width: 100%; padding-left: 0px; padding-right: 0px;">
                                 <tr>
                                    <td colspan="4" class="subtituloPAR">Información del evento</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;" class="subtituloPAR"> Fecha de inicio del evento:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_fecha_inicio_evento"></asp:Label></td>
                                    <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Fecha de finalización del evento:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_fech_finalizacion_evento"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;" class="subtituloPAR"> Tipo de evento:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_tipo_evento"></asp:Label></td>
                                    <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Número de horas:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_numero_horas"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;" class="subtituloPAR"> Nombre del evento:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_nombre_evento"></asp:Label></td>
                                    <td style="width: 150px; padding-left: 20px" class="subtituloPAR">Entidad a cargo del evento:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_entidad_acargo"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;" class="subtituloPAR"> Evento con recursos apalancados:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_evento_recursos_apalancados"></asp:Label></td>
                                    <td style="width: 150px; padding-left: 20px" class="subtituloPAR"></td>
                                    <td style="width: 210px;" class="center"></td>
                                </tr>
                                 <tr>
                                    <td colspan="4">
                                        <br />
                                        <hr />
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
                        <td colspan="4" class="subtituloPAR">11 Purpose and Description of End Use of Requested Items/Services:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label runat="server" ID="lbl_descripcion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">12 Special Instructions or Additional Information:
                        </td>
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">13 Attachments to PAR, such as specifications, scope of work, or other documentation (check one):
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                             <asp:RadioButtonList runat="server" ID="rbn_adjuntos_par" Enabled="false">
                            </asp:RadioButtonList>
                            
                        </td>
                        <td colspan="2" style="border-bottom: 1px solid black;">
                            <asp:Label runat="server" ID="lbl_adjuntos"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">

                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <hr />
                        </td>
                    </tr>


                    <tr>
                        <td class="subtituloPAR">14 Requestor Signature:</td>
                        <td></td>
                        <td style="padding-left: 20px;" class="subtituloPAR">15 Approver Signature:</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_firmaSolicitante"></asp:Label></td>
                        <td style="padding-left: 20px">Name</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_firmaAprobacion"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Title</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_cargofirmaSolicitante"></asp:Label></td>
                        <td style="padding-left: 20px">Title</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_cargofirmaAprobacion"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fechaSolicitante"></asp:Label></td>
                        <td style="padding-left: 20px">Date</td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_fecha_aprobacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <table style="background: #eeeeee; width: 100%">
                                <tr>
                                    <td colspan="4" class="subtituloPAR">
                                        
                                        <hr />
                                        16 Procurement Internal Use Only:</td>
                                </tr>
                                <tr>
                                    <td>PAR Number
                                    </td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_codigo_par"></asp:Label>
                                    </td>
                                    <td style="padding-left: 20px">Date Received:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="Label5"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-style: italic; padding-left: 50px">(Add PAR number and information to PAR Tracker.)</td>
                                </tr>
                                <tr>
                                    <td>Date Received: </td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                                        <asp:Label runat="server" ID="lbl_date_received"></asp:Label></td>
                                    <td style="padding-left: 20px">Assigned to:</td>
                                    <td style="width: 210px; border-bottom: 1px solid black;" class="center"></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center; font-weight:600;">
                            <hr />
                            ***Esta autorización de PAR no requiere firma autógrafa ya que fue elaborada y aprobada a través del sistema SIME***
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>

</asp:Content>
