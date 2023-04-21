<%@ Page Language="vb"  MasterPageFile="~/MasterPop_Rep.Master"  AutoEventWireup="false"  CodeBehind="frm_Deliverable_minute.aspx.vb" Inherits="RMS_APPROVAL.frm_Deliverable_minute" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

             <section class="content-header text-center">
                <h4>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">ACTA DE RECIBO A SATISFACCION Y SOLICITUD DE PAGO.</asp:Label>
                </h4>
            </section>

            <section class="content">

            <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
  
                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-12">
                        
                    <table class="table table-bordered table-responsive">                         
                          <tbody>
                            <tr>
                              <td>
                                  <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_Beneficiario" Text="Beneficiario:"></asp:Label></span><br />
                                  <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_Beneficiario" Text="Wildlife Works Carbon -WWC"></asp:Label></span>
                              </td>
                              <td>
                                 <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_activity_code" Text="STO No."></asp:Label></span><br />
                                 <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_activity" Text="SUB-IQS-220-STO-3"></asp:Label></span>
                               </td>
                              <td>
                               <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lbl_pago_descricion" Text="Pago No 2 y 4b(i) / 7 Valor Total US$: 73,304.94 "></asp:Label></span><br />
                              </td>                              
                            </tr>
                            <tr>
                              <td colspan="3">
                                <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_OTR" Text="Oficial Técnico Responsable - OTR:"></asp:Label></span>&nbsp;&nbsp; 
                                <span style="font-size:large;" class= "text-left"><asp:Label runat="server" ID="lbl_otr_name" Text="Luis Fernando Jara COP"></asp:Label></span>                         
                              </td>
                              
                            </tr>
                            <tr>
                               <td colspan=3>
                                <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lblt_products" Text="Pago correspondiente a los siguientes Productos:"></asp:Label></span><br /><br />    
                                <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_product_name" Text="Producto 2. Summary Report on Preparation for Verification Audit."></asp:Label></span><br />
                                    <div id="product_descrip" runat="server" class="text-justify" >
                                             The Subcontractor shall submit a report in Word format with a maximum of six pages that summarizes the preparation activities for at least three of the projects in the BioREDD+ portfolio for the third-party verification audit site visit, including all trainings held to review audit processes; review of the medium and higher risk conformance challenges identified in gap assessment to CCB standards; review of the MRV processes and sharing of results from social, carbon, and biodiversity monitoring completed; and review of BioREDD+ project systems for document management, data control, and audit responsibilities by the project proponents.<br />
                                             As an Annex, the Subcontractor shall include training attendance lists, training information sheet, assistance matrix and basic data sheet per crew trained. (All data formats will be provided by P&FActivity)<br /><br />
                                     
                                             In addition, the Subcontractor shall submit the following documents:<br /><br />

                                                •	Backup documentation that confirms that the Subcontractor has paid Fringe Benefits payments for Jeffrey Hayward (Health Insurance and Professional risks insurance) and Rocio Ramirez (Health Insurance, Pension Fund, Professional risks insurance) per Colombian law for November and December 2018.<br />
                                    </div>      
                                   <br />
                              </td>
                            </tr>  
                              <tr>
                                  <td>
                                     <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_invoice_text" Text="Información para facturación"></asp:Label></span><br />                                     
                                   </td>
                                   <td colspan="2">
                                       <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_CLIN_CODE" Text="CLIN:"></asp:Label></span>
                                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_CLIN" Text="433431"></asp:Label></span><br />
                                       <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_GL_CODE" Text="GL Code:"></asp:Label></span>
                                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_GL" Text="48010"></asp:Label></span><br />
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
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_comment" Text="Se aprueba el pago de los entregables No.2 por valor US$43,982.96 y No.4b(i) por valor de US$29,321.98, para un total de US$73,304.94, de la Orden de Sub-Tarea No. 3."></asp:Label></span><br />
                                              </td>
                                            </tr>
                                            <tr> 
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_soporte_ubicacion" Text="Área y Lugar de custodia del archivo soportes del Entregable:"></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_soporte_ubicacion" Text="Oficina Bogotá"></asp:Label></span><br />                                      
                                               </td>
                                          </tr>
                                           <tr> 
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_beneficiario_pago" Text="Beneficiario del Pago: "></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_beneficiario_pago" Text="Wildlife Works Carbon -WWC"></asp:Label></span><br />                                      
                                               </td>
                                          </tr>
                                          <tr> 
                                               <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_cuenta_entidad_fiananciera" Text="Cuenta y Entidad Financiera: "></asp:Label></span>                                                  
                                               </td>
                                              <td>
                                               <div id="div_bank" runat="server" class="text-justify" >
                                                      Citibank No. 204-396-675<br />
                                                      Routing number: 321171184<br />
                                                      Bank address or Branch location: <br />
                                                     130 Throckmorton Street, Mill Valley, CA 94941<br />
                                               </div>
                                              </td>
                                          </tr>
                                          <tr> 
                                               <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_identificacion_trib" Text="Identificación Tributaria: "></asp:Label></span>                                                  
                                               </td>
                                              <td>
                                               <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_identificacion_trib" Text="TIN 26-4138826"></asp:Label></span><br />
                                              </td>
                                          </tr>
                                           <tr>
                                               <td colspan="2">
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_autorizacion" Text="AUTORIZACION DE PAGO:"></asp:Label></span>
                                                  <span style="font-size:medium;"  ><asp:Label runat="server" ID="lbl_autorizacion_comment" Text="Por medio del presente documento los abajo participantes del proceso de aprobación IMP-019-006, autorizan el pago de los productos mencionados anteriormente. "></asp:Label></span><br />
                                              </td>
                                            </tr>
                                           <tr>
                                             <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="lblt_approver1" Text="LUIS FERNANDO JARA"></asp:Label></span><br />
                                                  <span style="font-size:medium;">APROBACIÓN COP PARAMOS Y BOSQUES </span><br />
                                                  <span style="font-size:medium;">Fecha APROBACIÓN 23 de septiembre 11:19 a.m. UTC -5</span><br />
                                              </td>
                                               <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="Label1" Text="LORENA HERNÁNDEZ DÍAZ"></asp:Label></span><br />
                                                  <span style="font-size:medium;">VBO. ESPECIALISTA DE SUBCONTRATOS</span><br />
                                                  <span style="font-size:medium;">Fecha VBO. 23 de septiembre 3:30 p.m. UTC -5</span><br />
                                              </td>
                                            </tr>
                                            <tr>
                                             <td>
                                                  <span style="font-weight:600;" class= "text-left"><asp:Label runat="server" ID="Label2" Text="MARIA PAULA VARGAS"></asp:Label></span><br />
                                                  <span style="font-size:medium;">VBO. DIRECTORA DE OPSFIN</span><br />
                                                  <span style="font-size:medium;">Fecha VBO. 25 de septiembre 09:12 a.m. UTC -5</span><br />
                                              </td>
                                              <td>

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