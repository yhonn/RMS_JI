<%@ Page Language="vb"  MasterPageFile="~/MasterPop_Rep.Master"  AutoEventWireup="false"  CodeBehind="frm_Deliverable_minutePrint.aspx.vb" Inherits="RMS_APPROVAL.frm_Deliverable_minutePrint" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

             <section class="content-header text-center">
                <h4>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">ACTA DE RECIBO A SATISFACCION Y SOLICITUD DE PAGO.</asp:Label>
                </h4>
            </section>

            <section class="content">

                <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                <asp:HiddenField ID="hd_id_deliverable_minute" runat="server" Value="0" />
                <asp:HiddenField ID="hd_id_documento" runat="server" Value="0" />
                                                                                                                          
  
                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-12">
                        
                    <table class="table table-bordered table-responsive">                         
                          <tbody>   
                            <tr>
                                <td>
                                   <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_Acta_no" Text="Acta No:"></asp:Label></span>
                                   <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_acta_no" Text=""></asp:Label></span>                
                                </td>
                                <td colspan="2"></td>                                
                            </tr>
                            <tr>
                              <td>
                                  <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_Beneficiario" Text="Beneficiario:"></asp:Label></span><br />
                                  <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_Beneficiario" Text=""></asp:Label></span>
                              </td>
                              <td>
                                 <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_activity_code" Text="STO No."></asp:Label></span><br />
                                 <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_activity" Text=""></asp:Label></span>
                               </td>
                              <td>
                               <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lbl_pago_descricion" Text=""></asp:Label></span><br />
                              </td>                              
                            </tr>
                            <tr>
                              <td colspan="3">
                                <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_OTR" Text="Oficial Técnico Responsable - OTR:"></asp:Label></span>&nbsp;&nbsp; 
                                <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_otr_name" Text=""></asp:Label></span>                         
                              </td>
                              
                            </tr>
                            <tr>
                               <td colspan=3>
                                <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lblt_products" Text="Pago correspondiente a los siguientes Productos:"></asp:Label></span><br /><br />    
                                <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_product_name" Text=""></asp:Label></span><br />
                                    <div id="product_descrip" runat="server" class="text-justify" >
                                            
                                    </div>      
                                   <br /><br />
                              </td>
                            </tr>  
                              <tr>
                                  <td>
                                     <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_invoice_text" Text="Información para facturación"></asp:Label></span><br />                                     
                                   </td>
                                   <td colspan="2">
                                       <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_CLIN_CODE" Text="CLIN:"></asp:Label></span>
                                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_CLIN" Text=""></asp:Label></span><br />
                                       <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_GL_CODE" Text="GL Code:"></asp:Label></span>
                                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_GL" Text=""></asp:Label></span><br />
                                   </td>
                              </tr>
                              <tr>
                                  <td colspan="3" style="padding:1px 1px 1px 1px; background-color:#000000;" >
                                      <table class="table table-bordered table-responsive">  
                                          <tr>
                                              <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_municipio" Text="Municipio:"></asp:Label></span>
                                                  <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_municipio" Text="Bogotá D.C."></asp:Label></span><br />
                                              </td>
                                            </tr>
                                           <tr>
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_comment" Text="Comentario / Observaciones:"></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_comment" Text=""></asp:Label></span><br />
                                              </td>
                                            </tr>
                                            <tr> 
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_soporte_ubicacion" Text="Área y Lugar de custodia del archivo soportes del Entregable:"></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_soporte_ubicacion" Text=""></asp:Label></span><br />                                      
                                               </td>
                                          </tr>
                                           <tr> 
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_beneficiario_pago" Text="Beneficiario del Pago: "></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_beneficiario_pago" Text=""></asp:Label></span><br />                                      
                                               </td>
                                          </tr>
                                          <tr> 
                                               <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_cuenta_entidad_fiananciera" Text="Cuenta y Entidad Financiera: "></asp:Label></span>                                                  
                                               </td>
                                              <td>
                                               <div id="div_bank" runat="server" class="text-justify" >
                                                     
                                               </div>
                                              </td>
                                          </tr>
                                          <tr> 
                                               <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_identificacion_trib" Text="Identificación Tributaria: "></asp:Label></span>                                                  
                                               </td>
                                              <td>
                                               <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_identificacion_trib" Text=""></asp:Label></span><br />
                                              </td>
                                          </tr>
                                           <tr>
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_autorizacion" Text="AUTORIZACION DE PAGO:"></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_autorizacion_comment" Text=""></asp:Label></span><br />
                                              </td>
                                            </tr>

                                          <tr>
                                              <td colspan="2" style="padding:0px 0px 0px 0px;">
                                                  <%--*********************************************************************************************************************************************--%>
                                                  <%--*********************************************************************************************************************************************--%>                                                  
                                                    <div id="App_users" runat="server" style="width:100%" >

                                                    </div>
                                                  <%--*********************************************************************************************************************************************--%>
                                                  <%--*********************************************************************************************************************************************--%>

                                              </td>
                                          </tr>
                                      </table>
                                  </td>
                              </tr>
                          </tbody>
                        </table>
                </div>
              </div>
            </section>

</asp:Content>