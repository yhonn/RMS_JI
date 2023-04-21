<%@ Page Language="VB" MasterPageFile="~/Site.Master"  AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_Deliverable_minuteAdd"  Codebehind="frm_Deliverable_minuteAdd.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
       
         <uc:Confirm runat="server" ID="MsgGuardar" />

             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLE MINUTE</asp:Label>
                </h1>
            </section>
            <section class="content">

            <style type="text/css">

                    .text_ctrl
                    {
	                    color: Black !important;
                        background-color:#ffcb00!important;
                        border-color:#ff6a00!important;

                    }

            </style>

                <link rel="stylesheet" href="../Content/slider_style.css?ids=0.001">
                <script src="../Content/plugins/knob/jquery.knob.js"></script>
                <script type="text/javascript">

                                       $(document).ready(function () {
                                                                           
                                                            
                                                             var hdValueTotal = $('input[id*=hd_percent]');

                                                                  $('.PorcDeliv').val(hdValueTotal.val());
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '16px');
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });
                                                                })                                                                                                                                                                                                                                               

                                                              function set_Percent(valPercent) {

                                                                  $('.PorcDeliv').val(valPercent);
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '16px');
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });
                                                                  
                                                                 //$('#<=chk_data_in.ClientID %>').prop('checked', false);//default value LOCA curr       
                                                                  
                                                              }
                                                                                         

                        function Currency_input() {

                            if ($('#<%=chk_data_in.ClientID %>').is(":checked")) { 
               
                               //alert('Checkeado');               
                               var currencySymbol = $('input[id*=curr_International]');                
                               var valorTotal = $('input[id*=hd_value_deliverable]');
                               var ctrl_Exchange = $('input[id*=hd_exchange_Rate]');
                               var USDvalue = ((valorTotal.val()) / parseFloat(ctrl_Exchange.val()));

                               // alert('Tot: ' + USDvalue + ' ' + currencyFormat(USDvalue));
                                
                               $("#currency_entry").html(currencySymbol.val());
                               $("#span_curr_entry").html(currencySymbol.val());  
                               $("#span_value_deliverable").html(currencyFormat(USDvalue));
               
                            } else {

                              // alert('NO Checkeado');
                                var currencySymbol = $('input[id*=curr_local]');
                                var valorTotal = $('input[id*=hd_value_deliverable]');
                                var valueTOT = parseFloat(valorTotal.val());

                                //alert(valueTOT);
                              
                               $("#currency_entry").html(currencySymbol.val());
                               $("#span_curr_entry").html(currencySymbol.val());                                
                               $("#span_value_deliverable").html(currencyFormat(valueTOT));                 

                            }

                        }
                                                                                                                            

                    function currencyFormat(num) {
                      return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
                    }


                   </script>  

                <div class="box">

                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Deliverable</asp:Label>
                        </h3>
                     </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">                                                              
                         
                                      <div class="form-group row">
                                         <div class="col-sm-12 col-xs-12">
                                                 <div class="box-body">                                 
                                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                                          <div class="info-box">
                                                            <span class="info-box-icon bg-gray"><i class="fa fa-university"></i></span>
                                                            <div class="info-box-content">
                                                                 <span class="info-box-text"><%= userName %></span>
                                                                 <span class="text-bold"><%= userImplementer %></span>             
                                                            </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->
                                                         <div class="col-md-4 col-sm-4 col-xs-4">
                                                          <div class="info-box">
                                                                <span class="info-box-icon  bg-gray"><i class="fa fa-tasks"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label ID="lbl_activity_name" runat="server"></asp:Label></span>                                                           
                                                                     <span class="text-bold"><asp:Label ID="lbl_activity_Code" runat="server"></asp:Label><br /><i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>                                                                          
                                                                </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->
                                                       <div class="col-md-4 col-sm-4 col-xs-4">
                                                          <div class="info-box">
                                                              <span class="info-box-icon bg-gray"><i class="fa fa-flag-o"></i></span>
                                                              <div class="info-box-content">
                                                                 <span class="info-box-text"><%=Status_TS%></span>
                                                                 <span class="text-bold"><%=DateStatus_TS%><i class="fa fa-clock-o"></i><%=HourStatus_TS%></span>             
                                                              </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->                                           
                                                    </div>
                                              </div>  
                                          
                                               <div class="col-sm-12 col-xs-12">
                                                    <div class="box-body">    

                                                          <div class="col-md-4 col-sm-4 col-xs-4">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-money"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></span>
                                                                     <span class="text-bold">Total Activity</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->

                                                         <div class="col-md-4 col-sm-4 col-xs-4 vertical-align-center"  >                                                                                                                                                             
                                                            <div style="text-align:center; vertical-align:middle;">     
                                                                 <asp:HiddenField ID="hd_percent" runat="server" Value="0" /> 
                                                               <input type="text" class="PorcDeliv" value="0" data-width="120" data-height="120" data-thickness="0.30" data-fgColor="#3c8dbc" data-skin="tron" readonly>                                                   
                                                            </div>
                                                         </div> 

                                                         <div class="col-md-4 col-sm-4 col-xs-4">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-line-chart"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalPerf" Text ="0 UGX"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0 USD"></asp:Label></span>
                                                                     <span class="text-bold">Total Performed</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->

                                                   </div>
                                               </div>                             
                                      </div>
                                    
                                         <div class="form-group row">
                                             <div class="col-sm-12 col-xs-12">
                                                   <!-- TABLE: ACTIVITY DELIVERABLE -->
                                                      <div class="box box-info">
                                                        <div class="box-header with-border">
                                                          <h3 class="box-title">DELIVERABLE INFO</h3>
                                                          <div class="box-tools pull-right">
                                                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                            <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                          </div>
                                                        </div><!-- /.box-header -->
                                                        <div class="box-body">
                                                          <div class="table-responsive">
                                                                                                    
                                                              <div class="col-sm-12 col-xs-12">                                                              

                                                                   <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                                                                   <asp:HiddenField runat="server" ID="hd_id_ficha_entregable" Value="0" />  
                                                                   <asp:HiddenField runat="server" ID="hd_performed" Value="0.0" />                                                   
                                                                   <asp:HiddenField ID="hd_tasa_cambio" runat="server" Value="0" />
                                                      
                                                                    <div  class="box-body table-responsive no-padding">
                                                                   
                                                                   <table class='table table-hover'>

                                                                        <asp:Repeater ID="rep_DelivINFO" runat="server" >
                                                                                  <ItemTemplate> 
                                                                                                <tr>

                                                                                                   <th>Deliverable #</th>
                                                                                                    <td class='vertical-align-center' ><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;<%# Eval("numero_entregable")%>&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                                                    <th>Amount</th>                                                                                                    
                                                                                                    <td> <%# String.Format(cultureUSer, "{0:N2} {1}", Eval("valor"), cultureUSer.NumberFormat.CurrencySymbol) %> / <%# String.Format("{0:N2}", Math.Round((Eval("valor") / Convert.ToDouble(Eval("tasa_cambio"))), 2, MidpointRounding.AwayFromZero))%> USD</td>   
                                                                                                    <th>Porcent</th>
                                                                                                    <td><%# String.Format("{0:P2}", (Eval("porcentaje") / 100))%> </td> 

                                                                                                </tr>                                                                                                                                                                                                                                                                  
                                                                                                <tr>
                                                                                                    <th>Due Date</th>
                                                                                                    <td>  <%# String.Format("{0:d}", Eval("fecha"))%>  </td>   
                                                                                                    <th>Status</th>
                                                                                                    <td><span class='label <%# Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 1) %>' > <%# If(Eval("numero_entregable") = 0, Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 2), Eval("deliverable_estado"))  %>   &nbsp;<i class='fa fa-clock-o'></i>&nbsp;<%# Func_Unit(Eval("D_Days"))%></span></td>                                                                                                                                                                                                                                                                                               
                                                                                                    <th></th>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                 <tr>                                                                  
                                                                                                  <td colspan='6'>
                                                                                                     <div class='text-justify' style='max-width:100%;'>
                                                                                                         <%# Eval("descripcion_entregable") %>
                                                                                                     </div>
                                                                                                   </td>                                                                  
                                                                                                </tr>   
                                                                       
                                                                                      </ItemTemplate>
                                                                          </asp:Repeater>
                                                                                                                                  
                                                                      </table>

                                                                  </div><!-- /.box-body -->
                                                                                                              
                                                                </div>
                                                                <div class="col-sm-12 col-xs-12">
                                                                   <hr style="border-color:black;" />
                                                                </div> 
                                                                                             
                                             
                                                                        <div class="col-sm-2 text-left">
                                                                         <!--Tittle -->
                                                                            <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                                        </div>
                                                                       <div class="col-sm-8">                                                   
                                                                              <div ID="dv_description" Runat="server" class=" text-justify ">

                                                                              </div>
                                                                           <!--Control -->                                              
                                                                       </div>   
                                                                       <div class="col-sm-12 col-xs-12 text-left">
                                                                           <hr />
                                                                       </div>                                             
                                                                      <div class="col-sm-2 text-left">
                                                                         <!--Tittle -->
                                                                         <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                                
                                                                       </div>
                                                                      <div class="col-sm-8">
                                                                            <div ID="dv_notes" Runat="server" class=" text-justify ">

                                                                              </div>
                                                                           <!--Control -->                                                                                     
                                                                       </div>

                                                          </div><!-- /.table-responsive -->

                                                          <div class="col-sm-12 col-xs-12">
                                                              <hr style="border-color:black;" />
                                                          </div>    
                                                            
                                                        </div><!-- /.box-body -->

                                                          <div class="box-footer clearfix">   



                                                   
                                                          </div> <!-- /.box-footer -->                                                                                            
                                         
                                                </div><!-- /.box -->

                                            </div>
                            
                                        </div>    

                                         <div class="form-group row">                                                    
                                                    <div class="col-sm-2 text-right">   
                                                        <br />
                                                        <label class="switch">
                                                          <input id="chk_data_in" runat="server" type="checkbox" onchange="Currency_input()">
                                                          <span class="slider round"></span>
                                                        </label>                                              
                                                    </div>       
                                                    <div class="col-sm-1 text-left" >                                        
                                                      <h2><span id="currency_entry" class="label label-info">COP</span></h2>      
                                                    </div>
                                                    <div class="col-sm-6 text-right">
                                                        <asp:HiddenField ID="hd_value_deliverable" runat="server" Value="" />
                                                        <asp:HiddenField ID="curr_local" runat="server" Value="" />
                                                        <asp:HiddenField ID="curr_International" runat="server" Value="" /><br />
                                                        <asp:Label runat="server" ID="lblt_exchange_rate" CssClass="control-label text-bold">Exchange Rate:</asp:Label>
                                                        <telerik:RadNumericTextBox ID="txt_tasa_cambio" runat="server" NumberFormat-DecimalDigits="2" Width="20%"></telerik:RadNumericTextBox>  
                                                        <asp:HiddenField ID="hd_exchange_Rate" runat="server" Value="" /><br />
                                                    </div>
                                                    <div class="col-sm-2 text-right">
                                                                                                      
                                                    </div>
                                          </div>                                                                                             
                                          <div class="form-group row">
                                          
                                           <div class="col-sm-12 col-xs-12 text-left">

                                                      <!-- TABLE:  DELIVERABLE DOCUMENTS--> 
                                                   
                                                            <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title"><asp:Label ID="lblt_minute_label" runat="server" Text="Minute Detail" ></asp:Label>&nbsp;(<asp:Label ID="lbl_approval_route" runat="server" ></asp:Label>)</h3>
                                                                  
                                                                  <asp:HiddenField ID="hd_id_documento_deliverable" runat="server" Value="0" />
                                                                  <asp:HiddenField ID="hd_dtTipoAPP" runat="server" Value="" />

                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                        
                                                                        <div class="box-body no-padding" style="padding-left:10px;" >

                                                                                           <div class="form-group row"  >
                                                                                                <br />
                                                                                                <div class="col-sm-4 text-left">
                                                                                                   <!--Tittle -->
                                                                                                   <asp:Label ID="lblt_Beneficiario" runat="server" CssClass="control-label text-bold" Text="Beneficiario"></asp:Label><br />
                                                                                                   <asp:Label ID="lbl_beneficiario" runat="server" CssClass="control-label" Text=""></asp:Label>
                                                                                                </div>
                                                                                                <div class="col-sm-4 text-left">
                                                                                                   <asp:Label ID="lbl_Activity" runat="server" CssClass="control-label text-bold" Text=""></asp:Label><br />
                                                                                                   <asp:Label ID="lbl_Activity_Code_2" runat="server" CssClass="control-label" Text=""></asp:Label>                                                                                           
                                                                                                </div>
                                                                                               <div class="col-sm-4 text-left">
                                                                                                   <asp:Label ID="lbl_pay_number" runat="server" CssClass="control-label text-bold" Text=""></asp:Label>&nbsp;<asp:Label ID="lblt_total_Value" runat="server" CssClass="control-label text-bold" Text="/ Valor Total"></asp:Label>&nbsp;<span id="span_value_deliverable" style="font-weight:600" ></span>&nbsp;<span id="span_curr_entry" style="font-weight:600" ></span><br />                                                                                                                                                                                            
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="form-group row"  >
                                                                                                <br />
                                                                                                <div class="col-sm-4 text-left">
                                                                                                   <asp:Label ID="lblt_OTR" runat="server" CssClass="control-label text-bold" Text="Oficial Técnico Responsable - OTR:"></asp:Label><br />
                                                                                                   <asp:Label ID="lbl_OTR" runat="server" CssClass="control-label" Text=""></asp:Label>                                                                                         
                                                                                                </div>
                                                                                            </div>
                                                                            
                                                                                           <div class="form-group row"  >
                                                                                                <div class="col-sm-12 col-xs-12">
                                                                                                    <hr style="border-color:black;" />
                                                                                                 </div>   
                                                                                            </div>
                                                                                                    <div class="form-group row"  >
                                                                                                        <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->
                                                                                                                  <asp:Label ID="lblt_accountable_info" runat="server" CssClass="control-label text-bold" Text="Información de Facturación"></asp:Label>
                                                                                                                </div>
                                                                                                                 <div class="col-sm-4 text-left">
                                                                                                                         <telerik:RadComboBox  ID="cmb_accountability" 
                                                                                                                                               runat ="server"   
                                                                                                                                               AutoPostBack="true"
                                                                                                                                               EmptyMessage="Select a value..."  
                                                                                                                                               Width="90%" >                                                                                               
                                                                                                                              </telerik:RadComboBox>
                                                                                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ForeColor="Red" ControlToValidate="cmb_accountability" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                                                                    
                                                                                                                 </div>
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                    <asp:Label ID="lbl_CLIN" runat="server" CssClass="control-label text-bold" Text=""></asp:Label><br />
                                                                                                                    <asp:Label ID="lbl_GL" runat="server" CssClass="control-label text-bold" Text=""></asp:Label>
                                                                                                                    
                                                                                                                </div>
                                                                                                           </div>

                                                                                                             
                                                                                                          <div class="form-group row"  >
                                                                                                                       <br />
                                                                                                                       <div class="col-sm-3 text-left">
                                                                                                                          <asp:Label ID="lblt_Departamento" runat="server" CssClass="control-label text-bold" >Departamento</asp:Label><br />
                                                                                                                           <telerik:RadComboBox  ID="cmb_departamento" 
                                                                                                                                                            runat ="server"  
                                                                                                                                                            AutoPostBack="true"
                                                                                                                                                            CausesValidation="False"                                                                     
                                                                                                                                                            EmptyMessage="Select a value..."   
                                                                                                                                                            AllowCustomText="true" 
                                                                                                                                                            Filter="Contains"                                         
                                                                                                                                                            Width="90%" >                                                                                               
                                                                                                                             </telerik:RadComboBox>
                                                                                                                       </div>                                                                                                                      
                                                                                                                       <div class="col-sm-3 text-left">
                                                                                                                           <asp:Label ID="lblt_municipio" runat="server" CssClass="control-label text-bold" >Municipio</asp:Label><br />
                                                                                                                                      <telerik:RadComboBox  ID="cmb_municipio" 
                                                                                                                                                            runat ="server"                                                                                                                                                              
                                                                                                                                                            CausesValidation="False"                                                                     
                                                                                                                                                            EmptyMessage="Select a value..."   
                                                                                                                                                            AllowCustomText="true" 
                                                                                                                                                            Filter="Contains"                                         
                                                                                                                                                            Width="90%" >                                                                                               
                                                                                                                                       </telerik:RadComboBox>
                                                                                                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="" ForeColor="Red" ControlToValidate="cmb_municipio" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                                                                                                                                             
                                                                                                                           <asp:HiddenField ID="hd_id_documento" runat="server" Value="0" />
                                                                                                                           <asp:HiddenField ID="hd_id_deliverable_minute" runat="server" Value="0" />
                                                                                                                           
                                                                                                                       </div> 
                                                                                                             
                                                                                                             </div>

                                                                                                          <div class="form-group row">
                                                                                                                <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->
                                                                                                                  <asp:Label ID="lblt_Observation" runat="server" CssClass="control-label text-bold" Text="Comentario / Observaciones"></asp:Label>
                                                                                                                </div>
                                                                                                               <div class="col-sm-8">
                                                                                                                   <!--Control -->
                                                                                                                     <telerik:RadTextBox ID="txt_notes" Runat="server" EmptyMessage="Type comments here.."  Width="90%" ValidationGroup="1" Height="100px" TextMode="MultiLine"  MaxLength="999">
                                                                                                                     </telerik:RadTextBox>                                            
                                                                                                               </div>
                                                                                                            </div>
                                                                            
                                                                                                        <div class="form-group row"  >
                                                                                                              <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->                                                                                                                 
                                                                                                                    <asp:Label ID="lblt_office" runat="server" CssClass="control-label text-bold" Text="Área y Lugar de custodia del archivo soportes del Entregable:"></asp:Label>
                                                                                                                </div>
                                                                                                                 <div class="col-sm-4 text-left">
                                                                                                                       <telerik:RadComboBox  ID="cmb_offices" 
                                                                                                                                               runat ="server"                                                                                                                                                                                                                                                                                                                                                                      
                                                                                                                                               EmptyMessage="Select a office..."                                                                                                                                                                                                                                                                                              
                                                                                                                                               Width="90%" >                                                                                               
                                                                                                                       </telerik:RadComboBox>
                                                                                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="" ForeColor="Red" ControlToValidate="cmb_offices" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                                                                    
                                                                                                                 </div>
                                                                                                           </div>

                                                                                                            <div class="form-group row"  >
                                                                                                              <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->                                                                                                                 
                                                                                                                    <asp:Label ID="lblt_pay_beneficiary" runat="server" CssClass="control-label text-bold" Text="Beneficiario del Pago:"></asp:Label>
                                                                                                                </div>
                                                                                                                 <div class="col-sm-4 text-left">
                                                                                                                       <telerik:RadTextBox ID="txt_beneficiary" runat="server" Width="80%" MaxLength="150" ReadOnly ="true">
                                                                                                                       </telerik:RadTextBox>                                     
                                                                                                                 </div>
                                                                                                             </div>

                                                                                                            <div class="form-group row"  >
                                                                                                              <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->                                                                                                                 
                                                                                                                    <asp:Label ID="lblt_billing_info" runat="server" CssClass="control-label text-bold" Text="Cuenta y Entidad Financiera:"></asp:Label>
                                                                                                                </div>
                                                                                                                 <div class="col-sm-4 text-left">
                                                                                                                       <telerik:RadTextBox ID="txt_billing_info" runat="server" Width="80%" MaxLength="500" TextMode="MultiLine" Rows="3" ReadOnly ="true">
                                                                                                                       </telerik:RadTextBox>                                     
                                                                                                                 </div>
                                                                                                             </div>

                                                                                                        <div class="form-group row"  >
                                                                                                              <br />
                                                                                                                <div class="col-sm-3 text-left">
                                                                                                                 <!--Tittle -->                                                                                                                 
                                                                                                                    <asp:Label ID="lblt_NIT_number" runat="server" CssClass="control-label text-bold" Text="Identificación Tributaria:"></asp:Label>
                                                                                                                </div>
                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                      <telerik:RadTextBox ID="txt_number_NIT" runat="server" Width="80%" MaxLength="150" ReadOnly ="true">
                                                                                                                       </telerik:RadTextBox>                                     
                                                                                                                 </div>
                                                                                                         </div>

                                                                                                        <div class="col-sm-12 col-xs-12">
                                                                                                              <hr style="border-color:black;" />
                                                                                                        </div>    
                                                                                                  
                                                                                                     <div class="form-group row"  >                                                                                                          
                                                                                                        <div class="col-sm-11 text-left">
                                                                                                             
                                                                                                               <telerik:RadGrid ID="grd_cate" 
                                                                                                                    runat="server" 
                                                                                                                    AllowAutomaticDeletes="True" 
                                                                                                                    Skin="Office2010Blue" 
                                                                                                                    AllowAutomaticUpdates ="True" 
                                                                                                                    AutoGenerateColumns="False" 
                                                                                                                    CellSpacing="0"
                                                                                                                    GridLines="None" 
                                                                                                                    Width="95%" ShowHeader="False">
                                                                                                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset">
                                                                                                                            <WebServiceSettings>
                                                                                                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                                                            </WebServiceSettings>
                                                                                                                            </HeaderContextMenu>
                                                                                                                   <MasterTableView >
                                                                                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                                                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                                                                                        </RowIndicatorColumn>
                                                                                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                                                                                        </ExpandCollapseColumn>
                                                                                                                        <Columns>
                                                                                                                             <telerik:GridTemplateColumn UniqueName="colm_Select" Visible="true" HeaderText="Sel"  >
                                                                                                                                      <ItemTemplate>
                                                                                                                                          <%--AutoPostBack="True" oncheckedchanged="chk_select_CheckedChanged"  --%>
                                                                                                                                                    <asp:CheckBox ID="chk_select" runat="server" ToolTip="Agregar al acta"  />
                                                                                                                                                    <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender_trigger" runat="server" 
                                                                                                                                                        CheckedImageUrl="~/Imagenes/iconos/Circle_Green.png" ImageHeight="16" ImageWidth="16"  
                                                                                                                                                        TargetControlID="chk_select" UncheckedImageUrl="~/Imagenes/iconos/Circle_Gray.png">                                                                                                              
                                                                                                                                                    </ajaxToolkit:ToggleButtonExtender>
                                                                                                                                      </ItemTemplate>
                                                                                                                                      <ItemStyle Width="10px" VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                                                                             </telerik:GridTemplateColumn>
                                                                                        
                                                                                                                            <telerik:GridTemplateColumn UniqueName="colm_id_app" Visible="true" Display="false" HeaderText="Sel"  >
                                                                                                                                   <ItemTemplate>
                                                                                                                                        <asp:HiddenField ID="hd_id_deliverable_minute_app" runat="server" Value ="0" />
                                                                                                                                   </ItemTemplate>                                                                                                                             
                                                                                                                             </telerik:GridTemplateColumn>

                                                                                                                             <telerik:GridBoundColumn DataField="id_ruta" 
                                                                                                                                UniqueName="id_ruta" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>

                                                                                                                             <telerik:GridBoundColumn DataField="id_App_documento" 
                                                                                                                                UniqueName="id_App_documento" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>

                                                                                                                              <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column" HeaderText="Id" SortExpression="orden" UniqueName="orden">                                                          
                                                                                                                              </telerik:GridBoundColumn>
                                                                                                                               <telerik:GridTemplateColumn UniqueName="colm_app_tp" Visible="true" HeaderText="App Type" >
                                                                                                                                      <ItemTemplate>
                                                                                                                                             <telerik:RadComboBox
                                                                                                                                                 ID="cmb_app_tp" 
                                                                                                                                                 runat ="server">                                                                                               
                                                                                                                                             </telerik:RadComboBox>                                                                                                               
                                                                                                                                      </ItemTemplate>                                                                                              
                                                                                                                               </telerik:GridTemplateColumn>
                                                                                                                               <telerik:GridBoundColumn DataField="id_estadoTipo" 
                                                                                                                                    UniqueName="id_estadoTipo" Visible="true" Display="false">
                                                                                                                                </telerik:GridBoundColumn>
                                                                                                                            
                                                                                                                              <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column" HeaderText="User" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                                                                                   <HeaderStyle CssClass="WithColumnDecrease40" />
                                                                                                                                  <ItemStyle CssClass="WithColumnDecrease40" />
                                                                                                                              </telerik:GridBoundColumn>
                                                                                                                              <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"  HeaderText="State" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                                                                                                  <HeaderStyle CssClass="WithColumnDecrease10" />
                                                                                                                                  <ItemStyle CssClass="WithColumnDecrease10" />
                                                                                                                              </telerik:GridBoundColumn>
                                                                                                                              <telerik:GridBoundColumn DataField="fecha_aprobacion" FilterControlAltText="Filter fecha_aprobacion column"  HeaderText="Date of approval" SortExpression="fecha_aprobacion" UniqueName="fecha_aprobacion">
                                                                                                                                  <HeaderStyle CssClass="WithColumnDecrease20" />
                                                                                                                                  <ItemStyle CssClass="WithColumnDecrease20" />
                                                                                                                              </telerik:GridBoundColumn>
                                                                                                                             <telerik:GridBoundColumn DataField="Alerta" FilterControlAltText="Filter Alerta column" HeaderText="Alert" SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">                                                          
                                                                                                                              </telerik:GridBoundColumn>
                                                                                                                              <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column" Visible="true" Display="false">
                                                                                                                                <ItemTemplate>
                                                                                                                                  <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png" ToolTip="Indicador Incompleto">
                                                                                                                                </asp:HyperLink>
                                                                                                                                </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                          </Columns>
                                                                                                                          <EditFormSettings>
                                                                                                                          <EditColumn FilterControlAltText="Filter EditCommandColumn column" UniqueName="EditCommandColumn1">
                                                                                                                          </EditColumn>
                                                                                                                          </EditFormSettings>
                                                                                                                          </MasterTableView>
                                                                                                                          <ClientSettings AllowDragToGroup="True" EnableRowHoverStyle="True">
                                                                                                                          <Selecting AllowRowSelect="True" />
                                                                                                                          </ClientSettings>
                                                                                                                          <FilterMenu EnableImageSprites="False">
                                                                                                                          <WebServiceSettings>
                                                                                                                          <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                                                          </WebServiceSettings>
                                                                                                                          </FilterMenu>
                                                                                                                 </telerik:RadGrid><br />

                                                                                                        </div>
                                                                                                    </div>

                                                                                                     <div class="form-group row" >                                                                                                          
                                                                                                         <div class="col-sm-1 text-left">
                                                                                                           <!--Tittle -->                                                                                                                 
                                                                                                               <br /> <asp:Label ID="lblt_minute_code" runat="server" CssClass="control-label text-bold" Text="Acta No: "></asp:Label>
                                                                                                          </div>
                                                                                                           <div class="col-sm-5 text-left" >                                        
                                                                                                                 <h2><span id="sp_code_minute" runat="server" class="label label-default">----</span></h2>      
                                                                                                           </div>
                                                                                                     </div> 
                                                               
                                                                            </div><!-- /.box-body -->

                                                                <div class="box-footer clearfix"> 
                                                                  <div class="small-box-XR box-warning">
                                                                      &nbsp;
                                                                  </div>
                                                                </div> <!-- /.box-footer -->                                                                                            
                                         
                                                        </div><!-- /.box -->   
                                                     
                                                   <!-- TABLE:  DELIVERABLE DOCUMENTS-->     

                                               </div>
                                            </div>                                 
                                       


                                  </div>
                                
                               </div> 
              
                          </div>
                        
                   </div>

                  <%--  </div>--%>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->      
                       
			                      <div class="form-group row"  runat="server" id="lyButtoms" >
                                         <div class="col-sm-2 text-left">
                                           <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm " Width="100" ValidationGroup="1" ></telerik:RadButton>
                                         </div>
                                        <div class="col-sm-2 text-left">
                                           <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Save and Continue" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" > Save </asp:LinkButton>                                              
                                        </div>                                       
                                        <div class="col-sm-2 text-left">                                           
                                          <asp:LinkButton ID="btnlk_generate_code" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Generar" Width="99%" class="btn btn-warning btn-sm margin-r-5 pull-left"  ValidationGroup="1" ><i class="fa fa-lock fa-1x"></i>&nbsp;&nbsp;Generar</asp:LinkButton>
                                        </div>
                                        <div class="col-sm-2 text-left">
                                           <asp:HyperLink ID="btnlk_print_preview" runat="server" Target="_blank" SingleClick="true"  Text="Vista Previa" Width="90%" class="btn btn-success btn-sm margin-r-5" data-toggle="Export" ><i class="fa fa-print fa-1x"></i>&nbsp;&nbsp;Vista Previa</asp:HyperLink>                                                                                    
                                        </div>
                                       <div class="col-sm-4 text-left">
                                           <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                                           <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                                        </div>
                                   </div>       
                                                                
                   </div>
                
                <%--</div>--%>

           </section>

    
    </asp:Content>

