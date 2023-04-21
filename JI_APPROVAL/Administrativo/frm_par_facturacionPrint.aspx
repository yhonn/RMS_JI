<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_par_facturacionPrint.aspx.vb" Inherits="RMS_APPROVAL.frm_par_facturacionPrint" %>
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
                        <td class="subtituloPAR">PAR Number
                        </td>
                        <td style="width: 210px; border-bottom: 1px solid black;" class="center">
                            <asp:Label runat="server" ID="lbl_codigo_par"></asp:Label>
                        </td>
                        <td colspan="2"></td>
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
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="subtituloPAR">10 Facturación</td>
                         <td class="subtituloPAR" style="text-align:right;"> <asp:Label runat="server" ID="tasa_ser"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_servicios_requeridos" runat="server" CellSpacing="0" PageSize="500" 
                                Culture="Spanish (Spain)" GridLines="None" ShowFooter="True"
                                Skin="Simple" AllowSorting="True">
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_factura_detalle" ShowGroupFooter="true"  ShowFooter="true">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter descripcion column" HeaderText="Descripción"
                                            SortExpression="descripcion" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter cantidad column" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderText="Cantidad" SortExpression="cantidad" 
                                            UniqueName="cantidad" DataFormatString="{0:N}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="unidad_medida" HeaderStyle-Font-Bold="true"
                                            FilterControlAltText="Filter unidad_medida column"
                                            HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                            UniqueName="unidad_medida" DataFormatString="{0:N}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="precio_unitario" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                                            HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter precio_unitario column" FooterText="Valor total COP: "
                                            HeaderText="Valor unitario" SortExpression="precio_unitario" 
                                            UniqueName="precio_unitario" DataFormatString="{0:N}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_total" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            FilterControlAltText="Filter valor_total column" Aggregate="Sum" FooterAggregateFormatString="{0:N}"
                                            HeaderText="Valor total" SortExpression="valor_total" FooterText="Total : " 
                                            UniqueName="valor_total" DataFormatString="{0:N}">
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                       <%-- <telerik:GridCalculatedColumn HeaderText="Total Price" UniqueName="TotalPrice" DataType="System.Double"
                                            DataFields="precio_unitario, cantidad" Expression="{0}*{1}" FooterText="Total : "
                                            Aggregate="Sum">
                                        </telerik:GridCalculatedColumn>--%>
                                    </Columns>
                                 <GroupByExpressions>
                                   <telerik:GridGroupByExpression>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="proveedorDe"></telerik:GridGroupByField>
                                        </GroupByFields>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldName="proveedorDe" HeaderText="Proveedor"></telerik:GridGroupByField>
                                        </SelectFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                </MasterTableView>
                            </telerik:RadGrid>
                         
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="subtituloPAR">11 Soportes de facturación</td>
                         <td class="subtituloPAR" style="text-align:right;"> <asp:Label runat="server" ID="Label3"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="grd_factura" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" Skin="Simple"
                                ShowColumnFooters="true"
                                ShowGroupFooters="true"
                                ShowGroupPanel="false">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="True"></Selecting>                                  
                                    <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_factura" AllowAutomaticUpdates="True" >
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_par_factura"
                                            FilterControlAltText="Filter id_par_factura column"
                                            SortExpression="id_par_factura" UniqueName="id_par_factura"
                                            Visible="False" HeaderText="id_par_factura"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_factura"
                                            FilterControlAltText="Filter numero_factura column" HeaderStyle-Font-Bold="true" HeaderStyle-Width="19%"
                                            HeaderText="Número de factura" SortExpression="numero_factura"
                                            UniqueName="numero_factura">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="proveedor"
                                            FilterControlAltText="Filter proveedor column" HeaderStyle-Font-Bold="true" HeaderStyle-Width="47%"
                                            HeaderText="Proveedor" SortExpression="proveedor"
                                            UniqueName="proveedor">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="categoria"
                                            FilterControlAltText="Filter categoria column" HeaderStyle-Font-Bold="true" HeaderStyle-Width="37%"
                                            HeaderText="Categoría" SortExpression="categoria"
                                            UniqueName="categoria">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_total" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            DataFormatString="{0:n}" 
                                            Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: " 
                                            FilterControlAltText="Filter valor_total column" HeaderStyle-Width="23%"
                                            HeaderText="Valor total" SortExpression="valor_total"
                                            UniqueName="valor_total">
                                            <HeaderStyle CssClass="wrapWord textrightalign"  />
                                            <ItemStyle CssClass="textrightalign" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="soporte"
                                            FilterControlAltText="Filter soporte column" visible="false"
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
