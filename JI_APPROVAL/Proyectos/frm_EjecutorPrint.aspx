<%@ Page Title="" Language="vb"  AutoEventWireup="false" MasterPageFile="~/Site.Mobile.master" Inherits="RMS_APPROVAL.frm_EjecutorPrint" Codebehind="frm_EjecutorPrint.aspx.vb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <table cellpadding="0" 
            
           style="width: 850px;  border-collapse: collapse; border: 1px solid #9A7328">
               <tr>
                <td>
                   
                   
                        <table cellpadding="0" 
                            style="border-collapse: collapse; width: 822px; border-color: #9a7328; border-width: 0">
                            <tr>
                                <td style="width: 24px; height: 10px">
                                </td>
                                <td style="height: 10px">
                                </td>
                                <td style="text-align: center; height: 10px;">
                                </td>
                                <td style="text-align: right; height: 10px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 24px">
                                    &nbsp;</td>
                                <td>
                                    <asp:Image ID="Image1" runat="server" 
                                        ImageUrl="~/Imagenes/logos/LogoConsolidacionTerritorial.png" />
                                </td>
                                <td style="text-align: center">
                        <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Size="17px" 
                            Text="Sistema de Monitoreo y Evaluación - Chemonics"></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    <asp:Image ID="Image2" runat="server" 
                                        ImageUrl="~/Imagenes/logos/LogoColombiaResponde.png" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 24px; height: 10px">
                                </td>
                                <td style="height: 10px">
                                </td>
                                <td style="text-align: center; height: 10px;">
                                </td>
                                <td style="text-align: right; height: 10px;">
                                    &nbsp;</td>
                            </tr>
                        </table>
                     </td>
            </tr>
               <tr>
                <td style="font-weight: bold; background-color: #C2B88B">
                   
                   
                        &nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="12px" 
                            Text="Ficha de registro de ejecutor"></asp:Label>
                        <asp:Label ID="lbl_id_ejecutor" runat="server" Font-Bold="True" Font-Names="Arial"
                                                Font-Size="X-Small" ForeColor="#C00000" 
                            Visible="False"></asp:Label>
                        <hr />
                   
                   </td>
            </tr>
               <tr>
                <td >

    
    <table border="0" style="width: 838px">
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                &nbsp;</td>
            <td align="left" style="text-align: right">
                        &nbsp;</td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px; vertical-align: top;">
                <asp:Label ID="lblproyecto16" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Código ejecutor:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                        <asp:Label ID="lbl_codigo_ejecutor" runat="server" Font-Bold="True" 
                            Font-Size="14px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px; vertical-align: top;">
                <asp:Label ID="lblproyecto" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Nombre ejecutor:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_nombreEjecutor" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblsector0" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Nombre corto:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_NombreCorto" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto4" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="NIT:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
               
               
               
                <asp:Label ID="txt_nit" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
               
               
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto5" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Teléfono ejecutor:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
               
               
               
                <asp:Label ID="txt_telefono_ejecutor" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
               
               
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto15" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="E-mail:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_email" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto9" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Representante legal:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_representante" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto14" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Cédula representante:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_cedula" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto10" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" 
                    Text="Teléfono representante:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_telefono_representante" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
            </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto11" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" Text="Número de socios:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="txt_numsocios" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
                </td>
        </tr>
        <tr>
            <td align="right" class="style10" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style10" style="width: 152px">
                <asp:Label ID="lblproyecto12" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" 
                    Text="Fecha de constitución:"></asp:Label>
            </td>
            <td align="left" style="text-align: left">
                <asp:Label ID="dt_fecha_inicio" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                </td>
        </tr>
        <tr>
            <td align="right" class="style11" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style11" style="width: 152px">
                <asp:Label ID="lblproyecto17" runat="server" Font-Bold="True" 
                    Font-Strikeout="False" Font-Underline="False" 
                    Text="Estado:"></asp:Label>
            </td>
            <td>
                   
                <asp:Label ID="lbl_estado" runat="server" Font-Bold="False" 
                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                          
                </td>
        </tr>
        <tr>
            <td align="right" class="style11" style="width: 6px">
                &nbsp;</td>
            <td align="right" class="style11" style="width: 152px">
                &nbsp;</td>
            <td style="text-align: right">
                    <input id="Button1" 
                        type="button" value="Imprimir" onclick = "javascript:window.print();" /></td>
        </tr>
        </table>
     
     </td>
</tr>
</table>

</asp:Content>
