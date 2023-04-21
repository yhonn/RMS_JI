<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPar.Master" CodeBehind="frm_viaje_informePrint.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_informePrint" %>
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
                        <td colspan="4">
                            <br />
                            <hr />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="subtituloPAR">Resultados</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_resultados"></asp:Label>
                            
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
                        <td colspan="4" class="subtituloPAR">Actividades en el informe del viaje</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_actividades"></asp:Label>
                            
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
                        <td colspan="4" class="subtituloPAR">Lugares visitados, entidades y personas con las cuáles se reunió</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:Label runat="server" ID="lbl_lugares_visitados"></asp:Label>
                            
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
