<%@ Page Language="vb"  MasterPageFile="~/MasterPop_Rep.Master"  AutoEventWireup="false"  CodeBehind="frm_Deliverable_minutePrintG.aspx.vb" Inherits="RMS_APPROVAL.frm_Deliverable_minutePrintG" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

             <section class="content-header text-center">
                <h4>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">Annex 7. Milestone Payment Request Form/Anexo 7.</asp:Label>                    
                </h4>
                <br /> <asp:Label runat="server" ID="lblt_titulo_pantalla_2">Formato Solicitud de Pago por cumplimiento de Metas</asp:Label>
            </section>

            <section class="content">

                <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                <asp:HiddenField ID="hd_id_deliverable_minute" runat="server" Value="0" />
                <asp:HiddenField ID="hd_id_documento" runat="server" Value="0" />
                
                <div class="row" style="padding:10px 10px 10px 10px; text-align:center;">
                    <div class=" col-xs-12">
                         <h4>
                            <asp:Label runat="server" ID="lblt_subtittle">FORMULARIO DE SOLICITUD DE PAGO POR PRODUCTO.</asp:Label>
                        </h4>
                    </div>                 
                </div>

                             

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-12">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_sectionI" Text="I. información del Convenio de Donación"></asp:Label></span>
                    </div>
                </div>
                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_activity_code" Text="Número de la Donación:"></asp:Label></span>
                    </div>
                     <div class=" col-xs-2">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_activity" Text=""></asp:Label></span>
                     </div>
                     <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_fecha" Text="Fecha:"></asp:Label></span>&nbsp;&nbsp;<span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_fecha" Text="Fecha:"></asp:Label></span>
                    </div>                    
                    <div class=" col-xs-6">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_Acta_no" Text="Acta No:"></asp:Label></span><span style="font-size:large;" class= "text-left">&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_acta_no" Text=""></asp:Label></span>                                 
                    </div>
                </div>
                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-2">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_Donatario" Text="Nombre del Donatario: "></asp:Label></span>                           
                    </div>
                     <div class=" col-xs-6">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_Beneficiario" Text=""></asp:Label></span>                             
                     </div>
                </div>
                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-2">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_activity_name" Text="Nombre de la Actividad: "></asp:Label></span>                           
                    </div>
                     <div class=" col-xs-8">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_activity_name" Text=""></asp:Label></span>                             
                     </div>
                </div>
                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-12">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_sectionII" Text="II. Solicitud de pago"></asp:Label></span>
                    </div>
                </div>
                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-11">
                           <table class="table table-bordered table-responsive">     
                                 <thead>
                                    <tr >
                                        <th style="text-align:center; vertical-align:central">No. de Pago</th>
                                        <th style="text-align:center; vertical-align:central">Fecha de Entrega</th>
                                        <th style="text-align:center; vertical-align:central">Descripción</th>
                                        <th style="text-align:center; vertical-align:central">Medio de Verificación (Documentación requerida que debe ser anexada a la solicitud de pago)</th>
                                        <th style="text-align:center; vertical-align:central">%</th>
                                        <th style="text-align:center; vertical-align:central">
                                            Valor del producto
                                            (<span id="currency_info" runat="server" ></span>)
                                         </th>
                                    </tr>
                                </thead> 
                                <tbody> 
                                  <tr>
                                      <td style=" text-align:center; vertical-align:central">
                                          <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lbl_No" Text=""></asp:Label></span>
                                      </td>
                                      <td>
                                           <span><asp:Label runat="server" ID="lbl_fecha_entrega" Text=""></asp:Label></span>
                                      </td>
                                      <td>
                                          <span><asp:Label runat="server" ID="lbl_descripcion_entregable" Text=""></asp:Label></span>
                                      </td>
                                       <td>
                                          <span><asp:Label runat="server" ID="lbl_medio_verificacion" Text=""></asp:Label></span>
                                       </td>
                                       <td>
                                           <span><asp:Label runat="server" ID="lbl_percen" Text=""></asp:Label></span>
                                       </td>
                                       <td>
                                           <span><asp:Label runat="server" ID="lbl_valor_PB" Text=""></asp:Label></span>
                                       </td>                                       
                                   </tr>
                                </tbody>
                          </table>
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                          <div style=" text-align:justify;" id="sp_disclaimer" >
                              Yo afirmo, que el producto(s) presentado(s) arriba para pago se ha(n) logrado de acuerdo con los términos y condiciones establecidos en el Convenio de Donación suscrito con la Actividad Páramos y Bosques de USAID (P&B) y todos los documentos incorporados, incluyendo el presupuesto aprobado.
                          </div><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_representante_legal" Text="Representante Legal del Donatario: "></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_nombre_rep" Text="Nombre: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class=" col-xs-3">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_nombre_rep" Text=""></asp:Label></span>                             
                     </div>
                </div>

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_sign" Text="Firma: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_fecha_rep" Text="Fecha: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-4">
                        <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_fecha_rep" Text=""></asp:Label></span>
                     </div>
                </div>
  
                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-12">
                        <br /><br /> <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_sectionIII" Text="III. Aprobación del producto(s). "></asp:Label></span><br /><br />
                    </div>
                </div>
  
                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_certificado_supervisor" Text="Certificado del Supervisor Responsable"></asp:Label></span>                           
                    </div>
                    <div class=" col-xs-8">
                          <div style="text-align:justify;" id="dv_disclaimer_supervisor" >
                              He revisado los elementos del producto(s) para pago y los he encontrado ajustados y completos con respecto a la información de los archivos de donación. Basado en la verificación de la documentación enviada con la solicitud de pago, Yo confirmo que los productos arriba especificados, como los especificados en el Acuerdo de Donación, han sido logrados.
                              y a los compromisos técnicos adquiridos por el donatario, por lo que recomiendo el pago del presente producto. 
                          </div><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_supervisor" Text="Supervisor Responsable Actividad Páramos y Bosques de USAID (P&B)"></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_supervisor_nombre" Text="Nombre: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class=" col-xs-3">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_supervisor_nombre" Text=""></asp:Label></span>                             
                     </div>
                </div>

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_supervisor_firma" Text="Firma: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_supervisor_fecha" Text="Fecha Aprobación: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-4">
                        <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_supervisor_fecha" Text=""></asp:Label></span>
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                     <br />
                    <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_COP_certificado" Text="Certificado del DCOP del Programa"></asp:Label></span>                           
                    </div>
                    <div class=" col-xs-8">
                          <div style="text-align:justify;" id="dv_disclaimer_COP" >
                              He revisado los elementos del producto para pago y los he encontrado exactos y completos con respecto al acuerdo de donación y a los compromisos técnicos adquiridos por el donatario, por lo que recomiendo el pago del presente producto.  
                          </div><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_COP_onCharge" Text="Certificado del DCOP del Programa"></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_COP_name" Text="Nombre: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class=" col-xs-3">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_COP_name" Text=""></asp:Label></span>                             
                     </div>
                </div>

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_COP_sign" Text="Firma: "></asp:Label></span>                           
                    </div>
                    <div style="border-bottom: 1px solid;" class="col-xs-3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_COP_Date" Text="Fecha Aprobación: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-4">
                        <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_COP_Date" Text=""></asp:Label></span>
                     </div>
                </div>

                  <div class="row" style="padding:10px 10px 10px 10px;">
                      <br /><br />
                    <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_ME_certificado" Text="Certificado de M&E del Programa "></asp:Label></span>                           
                    </div>
                    <div class=" col-xs-8">
                          <div style="text-align:justify;" id="dv_disclaimer_ME" >
                              – He revisado los elementos del producto para pago con respecto al Plan de Monitoreo y evaluación adquiridos por el donatario, y se encuentran acorde al cumplimiento del avance.  
                          </div><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_ME_onCharge" Text="Líder M&E Actividad Páramos y Bosques de USAID (P&B)"></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_ME_name" Text="Nombre: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class=" col-xs-3">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_ME_name" Text=""></asp:Label></span>                             
                     </div>
                </div>

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_ME_sign" Text="Firma: "></asp:Label></span>                           
                    </div>
                    <div style="border-bottom: 1px solid;" class="col-xs-3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_ME_Date" Text="Fecha Aprobación: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-4">
                        <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_ME_Date" Text=""></asp:Label></span><br /><br />
                     </div>
                </div>

                  <div class="row" style="padding:10px 10px 10px 10px;">
                      <br /><br />
                    <div class=" col-xs-2">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_GRANT_certificado" Text="Certificado de Donaciones "></asp:Label></span>                           
                    </div>
                    <div class=" col-xs-8">
                          <div style="text-align:justify;" id="dv_disclaimer_GRANT" >
                              (Diligenciado por la Gerente de Donaciones). Con base en la información recogida en ese momento, es justo y razonable suponer que el Producto, como se especifica en el Acuerdo de Donación, se ha logrado.  
                          </div><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_GRANT_onCharge" Text="Gerente de Donaciones Actividad Páramos y Bosques de USAID (P&B)"></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                           <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_GRANT_name" Text="Nombre: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class=" col-xs-3">
                         <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_GRANT_name" Text=""></asp:Label></span>                             
                     </div>
                </div>

                <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                       <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_GRANT_sign" Text="Firma: "></asp:Label></span>                           
                    </div>
                    <div style="border-bottom: 1px solid;" class="col-xs-3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-1">
                        <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_GRANT_Date" Text="Fecha Aprobación: "></asp:Label></span>                           
                    </div>
                     <div style="border-bottom: 1px solid;" class="col-xs-4">
                        <span style="font-size:medium;" class= "text-left"><asp:Label runat="server" ID="lbl_GRANT_Date" Text=""></asp:Label></span><br /><br />
                     </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                     <br /><br />
                    <div class=" col-xs-12">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lblt_section_IV" Text="IV. Resumen del presupuesto de la Donación "></asp:Label></span>
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                         <span style="font-weight:600;" ><asp:Label runat="server" ID="lbl_concepto_pago" Text=""></asp:Label></span><br /><br />
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-9">
                           <table class="table table-bordered table-responsive">                                      
                                <tbody>   
                                     <tr>
                                      <td >
                                          <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lblt_total_usd" Text="Valor total de la Actividad"></asp:Label></span>
                                      </td>
                                      <td>
                                           <span><asp:Label runat="server" ID="lbl_total_usd" Text=""></asp:Label></span>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td>
                                          <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lblt_totalACT_usd" Text="Valor total de la donación (Aporte en dinero P&B)"></asp:Label></span>
                                      </td>
                                      <td>
                                           <span><asp:Label runat="server" ID="lbl_totalACT_usd" Text=""></asp:Label></span>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td>
                                          <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lblt_totalPerf_usd" Text="Valor pagos acumulados (incluyendo el pago de este producto) "></asp:Label></span>
                                      </td>
                                      <td>
                                          <span><asp:Label runat="server" ID="lbl_totalPerf_usd" Text=""></asp:Label></span>
                                      </td>
                                    </tr>
                                     <tr>
                                      <td>
                                          <span style="font-weight:600;font-size:large;" ><asp:Label runat="server" ID="lblt_totalPend_usd" Text="Saldo de la donación "></asp:Label></span>
                                      </td>
                                      <td>
                                          <span><asp:Label runat="server" ID="lbl_totalPend_usd" Text=""></asp:Label></span>
                                      </td>
                                    </tr>
                                </tbody>
                          </table>
                    </div>
                </div>

                 <div class="row" style="padding:10px 10px 10px 10px;">
                    <div class=" col-xs-10">
                        <br /><br />                        
                        <asp:Label runat="server" ID="lblt_Nota" Text="NOTA: El producto original se encuentra en el archivo físico de donaciones en la carpeta de "></asp:Label>
                        <asp:Label runat="server" ID="lbl_org" Text=""></asp:Label>
                       </div>
                </div>

               <%-- --******************************************--%>


            </section>

</asp:Content>