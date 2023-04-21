<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_DeliverableEdit_"  Codebehind="frm_DeliverableEdit.aspx.vb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLE</asp:Label>
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

                                                              }
                                                                                                                            

                    <%-- function getActivity(sender, eventArgs) {

                                                                 var combobox = $find("<%= cmb_activity.ClientID %>");
                                                                
                                                                 var value = combobox.get_selectedItem().get_value();
                                                                 var texto = combobox.get_selectedItem().get_text();
                                                                                                                              
                                                                 var strText = texto.split('==>>');

                                                                 $('#<%=lbl_activity_Code.ClientID %>').text(strText[0]);
                                                                 $('#<%=lbl_activity_name.ClientID %>').text(strText[1]);
                                                                                                                              

                                                                 $('#<%=lbl_period.ClientID %>').text(strText[2] + ' to ' + strText[3]);
                                                                 
                                                                // alert('{idFichaProyecto:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }');

                                                              $.ajax({
                                                                     type: "POST",
                                                                     url: "frm_DeliverableAdd.aspx/GetDeliverables",
                                                                     data: '{idFichaProyecto:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }',
                                                                     contentType: "application/json; charset=utf-8",
                                                                     dataType: "json",
                                                                     success: function (msg) {

                                                                         jsonResult = msg.d;
                                                                         //alert('Result ' + jsonResult);

                                                                         var jsonResultARR = jsonResult.split('||');
                                                                                  
                                                                         $('#<%= ltr_rows_Deliverables.ClientID %>').html(jsonResultARR[0]);   
                                                                         
                                                                         $('#<%= lbl_totalACT.ClientID %>').text(jsonResultARR[2]);
                                                                         $('#<%= lbl_totalACT_usd.ClientID %>').text(jsonResultARR[1]);

                                                                         $('#<%= lbl_totalPerf.ClientID %>').text(jsonResultARR[6]);
                                                                         $('#<%= lbl_totalPerf_usd.ClientID %>').text(jsonResultARR[5]);

                                                                         $('#<%= lbl_totalPend.ClientID %>').text(jsonResultARR[4]);
                                                                         $('#<%= lbl_totalPend_usd.ClientID %>').text(jsonResultARR[3]);                                                                                                                                                 
                                                                         
                                                                         //comboboxType_.clearSelection();
                                                                         
                                                                         $('.PorcDeliv').val(jsonResultARR[7]);
                                                                         $('.PorcDeliv').trigger('change');
                                                                        $('.PorcDeliv').css('font-size', '16px');
                                                                         $(".PorcDeliv").knob({
                                                                                 'format': function (value) {
                                                                                     return value + '%';
                                                                                 }
                                                                         });

                                                                        $('#<%= dvNEXT_delieverable.ClientID %>').html(jsonResultARR[8]);   
                                                                        $('#<%= hd_id_ficha_entregable.ClientID %>').val(jsonResultARR[9]);
                                                     
                                                                         
                                                                     },
                                                                     failure: function (response) {
                                                                         alert('Error: ' + response.d);
                                                                     }   
                                                                 });
                                                 
                                                    }--%>


                                        function Calculate(sender, args, params) {

                                            //alert(params);

                                            var strID = params.toString().split("||");

                                            //alert(strID[0]);
                                            //alert(strID[1]);

                                            var idMeta_ficha = strID[0]; //params.toString().split("||")[0];
                                            var idAvance_ficha = strID[1]; //params.toString().split("||")[1];
                                            
                                            var ctrl_name = 'txt_value_' + idMeta_ficha;
                                            var ctrl_total_value = 'hd_ind_report_value_' + idAvance_ficha;
                                            var ctrl_reported_value = 'hd_ind_value_' + idAvance_ficha;
                                                           
                                            //alert(ctrl_name);                                                                                       
                                            //alert($$("txtSymbol", $("#wrapper")).attr("id"));
                                            //alert($$(ctrl_name).attr("id"));
                                            //alert($$(ctrl_name).val());

                                            var radControl_destination = $find($$(ctrl_name).attr("id"));
                                            var radControl_source = sender;
                                         // var hd_controlValue = $find($$(ctrl_value).attr("id"));
                                                                                       
                                            //alert($$(ctrl_value).attr("id"));
                                            //alert($$("hd_ind_report_value_40-87").attr("id"));

                                            //alert($('input#hd_ind_report_value_4087').val());
                                            //alert($(':hidden#hd_ind_report_value').val());
                                            //hd_ind_report_value_4087
                                            //alert($('').val());

                                           // alert(ctrl_value);
                                            
                                            //$.each($('input[id*=' + ctrl_value + ']'), function (i, val) {
                                            //    if ($(this).attr("type") == "hidden") {
                                            //        var valueOfHidFiled = $(this).val();
                                            //        alert($(this).attr("id") + ': ' + valueOfHidFiled);
                                            //    }
                                            //});

                                            var hdValueTotal = $('input[id*=' + ctrl_reported_value + ']');

                                           // alert($("[id$=_hd_ind_report_value_4087]").attr("id"));
                                                                                        
                                            //$$(ctrl_name);
                                            //alert('Total Indicator: ' + radControl_destination.get_value());
                                            //alert(hd_controlValue.Value);
                                            //alert('Total Report Changed: ' + radControl_source.get_value());
                                            //alert(hd_controlValue.Value);
                                            //alert('Total Report : ' + hdValueTotal.val());
                                            
                                            var textBoxElement = radControl_source._textBoxElement;
                                            textBoxElement.className = textBoxElement.className + ' text_ctrl';
                                            //radControl_source.Skin = "Simple";

                                            var valNW = (radControl_destination.get_value() - hdValueTotal.val()) + radControl_source.get_value(); 'Rest older value and summary the new one'

                                            radControl_destination.set_value(valNW);                                           

                                      }



                                    Function.prototype.indice = function () {
                                        var method = this, args = Array.prototype.slice.call(arguments);                                        
                                        return function () {
                                            return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                                        };
                                    };

                                    function $$(id, context) {
                                        var el = $("#" + id, context);
                                        if (el.length < 1)
                                            el = $("[id$=_" + id + "]", context);
                                        return el;
                                    }
                                                             
                   </script>  

                <div class="box">

                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Deliverable Indicators Results</asp:Label>
                        </h3>
                     </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">
                                                              
                                  <div class="stepwizard">
                                    <div class="stepwizard-row setup-panel">

                                        <div class="stepwizard-step" style="width:25%">
                                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                            <p>
                                                <asp:Label runat="server" ID="lblt_informaciongeneral">Deliverable Information</asp:Label>
                                            </p>
                                         </div>

                                         <div class="stepwizard-step" style="width:25%">
                                             <a   href="#step-2" id="anchorResults" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                             <p>
                                                <asp:Label runat="server" ID="lblt_personal_status">Deliverable Result</asp:Label>
                                             </p>
                                         </div>

                                         <div class="stepwizard-step" style="width:25%">
                                             <a   href="#step-3" id="anchorDocuments" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                             <p>
                                                <asp:Label runat="server" ID="lblt_deliverable_documents">Deliverable Documents</asp:Label>
                                             </p>
                                         </div>

                                         <div class="stepwizard-step" style="width:25%">
                                               <a   href="#step-4" id="anchorFollowUp" runat="server" type="button" class="btn btn-default btn-circle">4</a>
                                                 <p>
                                                   <asp:Label runat="server" ID="lblt_complementary">Follow Up</asp:Label>
                                                 </p>
                                          </div>         
                                        
                                        
                                                                            
                                            </div>
                                         </div>

                                      <div class="form-group row">                                          
                                         <div class="col-sm-12 text-left">
                                            <hr />
                                        </div>                                        
                                      </div>

                                      <div class="form-group row">
                                         <div class="col-sm-12">
                                                 <div class="box-body">                                 
                                                        <div class="col-md-4 col-sm-8 col-xs-16">
                                                          <div class="info-box">
                                                            <span class="info-box-icon bg-gray"><i class="fa fa-university"></i></span>
                                                            <div class="info-box-content">
                                                                 <span class="info-box-text"><%= userName %></span>
                                                                 <span class="text-bold"><%= userImplementer %></span>             
                                                            </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->
                                                         <div class="col-md-4 col-sm-8 col-xs-16">
                                                          <div class="info-box">
                                                                <span class="info-box-icon  bg-gray"><i class="fa fa-tasks"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label ID="lbl_activity_name" runat="server"></asp:Label></span>                                                           
                                                                     <span class="text-bold"><asp:Label ID="lbl_activity_Code" runat="server"></asp:Label><br /><i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>                                                                          
                                                                </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->
                                                       <div class="col-md-4 col-sm-8 col-xs-16">
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
                                          
                                               <div class="col-sm-12">
                                                    <div class="box-body">    

                                                          <div class="col-md-4 col-sm-8 col-xs-16">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-money"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></span>
                                                                     <span class="text-bold">Total Activity</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->

                                                         <div class="col-md-4 col-sm-8 col-xs-16 vertical-align-center"  >                                                                                                                                                             
                                                            <div style="text-align:center; vertical-align:middle;"> 
                                                               <asp:HiddenField ID="hd_percent" runat="server" Value="0" />      
                                                               <input type="text" class="PorcDeliv" value="0" data-width="120" data-height="120" data-thickness="0.30" data-fgColor="#3c8dbc" data-skin="tron" readonly>                                                   
                                                            </div>
                                                         </div> 

                                                         <div class="col-md-4 col-sm-8 col-xs-16">
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
                                             <div class="col-sm-12">
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
                                                                                                    
                                                              <div class="col-sm-12">
                                                                   <%-- <div class="col-sm-4 pull-right">
                                                                     <div style="text-align:right;">                                                 
                                                                          <h4>Total Activity: <asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label></h4>
                                                                          <h4><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                                          <h4>Total Performed: <asp:Label runat="server" ID="lbl_totalPerf" Text ="0 UGX"></asp:Label></h4>
                                                                          <h4><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                                          <h3>Pending: <asp:Label runat="server" ID="lbl_totalPend" Text ="0 UGX"></asp:Label></h3>
                                                                          <h3><asp:Label runat="server" ID="lbl_totalPend_usd" Text ="0 USD"></asp:Label></h3>
                                                                     </div>
                                                                   </div>--%>

                                                                   <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                                                                   <asp:HiddenField runat="server" ID="hd_id_ficha_entregable" Value="0" />                                                     
                                                                   <asp:HiddenField ID="hd_tasa_cambio" runat="server" Value="0" />
                                                      
                                                                    <div class="box-body table-responsive no-padding">
                                                                       <%--   <table class="table table-hover">
                                                                            <tr>
                                                                              <th>Deliverable #</th>
                                                                              <td><span class="badge bg-primary">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                            </tr>
                                                                            <tr>                                                                  
                                                                              <td colspan="2">Develop a curriculum and execution plan for Executive Course on Management Information Systems and completion of staff recruitment for 10 interns (below 35 years of age), 6 new staff, and 5 short term consultants.</td>                                                                  
                                                                            </tr>
                                                                            <tr>
                                                                              <th>Due Date</th>
                                                                              <td>25/10/2018</td>   
                                                                            </tr>     
                                                                            <tr>
                                                                              <th>Status</th>
                                                                              <td><span class='label label-danger'>Expired&nbsp;<i class='fa fa-clock-o'></i>&nbsp;3&nbsp;Weeks</span></td>   
                                                                            </tr>  
                                                                             <tr>
                                                                              <th>Porcent</th>
                                                                              <td>25%</td>   
                                                                            </tr>   
                                                                            <tr>
                                                                              <th>Amount</th>
                                                                              <td>305,456,789 UGX / 258,325.56 USD</td>   
                                                                            </tr>                                                               
                                                                          </table>--%>

                                                                         <table class='table table-hover'>

                                                                        <asp:Repeater ID="rep_DelivINFO" runat="server" >
                                                                                  <ItemTemplate> 
                                                                                                <tr>

                                                                                                   <th>Deliverable #</th>
                                                                                                    <td class='vertical-align-center' ><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;<%# Eval("numero_entregable")%>&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                                                    <th>Amount</th>
                                                                                                    <td> <%# String.Format(cultureUSer, "{0:N2} {1}", Eval("valor"), cultureUSer.NumberFormat.CurrencySymbol) %> / <%# String.Format("{0:N2}", Math.Round((Eval("valor") / Convert.ToDouble(Eval("tasa_Cambio"))), 2, MidpointRounding.AwayFromZero))%> USD</td>   
                                                                                                    <th>Percent</th>
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
                                                                <div class="col-sm-12">
                                                                     <hr style="border-color:black;" />
                                                               </div> 

                                                           <%-- <table class="table no-margin">
                                                              <thead>
                                                                <tr>
                                                                  <th style="width:2%;"></th>                                   
                                                                  <th style="width:3%;">#</th>                    
                                                                  <th style="width:25%;">Milestone</th>
                                                                  <th style="width:35%;">Verification</th>
                                                                  <th style="width:8%;">Due Date</th>
                                                                  <th style="width:8%;">Delivered Date</th>
                                                                  <th style="width:3%;">%</th>
                                                                  <th style="width:8%;">Amount</th>
                                                                  <th style="width:8%;">Status</th>
                                                                </tr>
                                                              </thead>
                                                              <tbody>                                                   
                                                                            <tr>
                                                                               <td><div class="tools"><a href="/RMS_APPROVAL/approvals/frm_seguimientoAprobacionRep.aspx?IdDoc=0&IdRuta=0" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                               <td>1</td>
                                                                               <td>A database of 120 youth,  40% women, service providers in the maize and beans value chains</td>
                                                                               <td>
                                                                                   <div style="overflow:scroll; text-align:left; max-width:100%; max-height:300px;">
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                    </div>

                                                                               </td>
                                                                               <td>2017-12-04</td>
                                                                               <td>--</td>
                                                                               <td>24.89%</td>
                                                                               <td>35,498,615.30</td>
                                                                               <td>                                                                       
                                                                                  <span class="label label-danger">Expired&nbsp;<i class="fa fa-clock-o"></i>&nbsp;15&nbsp;Weeks</span>
                                                                               </td>
                                                                            </tr>
                                                                            <tr>
                                                                               <td colspan="9">                                                                     

                                                                                    <div class="progress">
                                                                                        <div class="progress-bar progress-bar-warning progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                          <span>11% (Planned)</span>
                                                                                        </div>
                                                                                        <div class="progress-bar progress-bar-danger   progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                          <span >22% (Delayed)</span>
                                                                                        </div>
                                                                                      </div>

                                                                               </td>  
                                                                            </tr>                      
                                                      
                                                                               <tr>
                                                                               <td><div class="tools"><a href="/RMS_APPROVAL/approvals/frm_seguimientoAprobacionRep.aspx?IdDoc=<%# Eval("id_documento")%>&IdRuta=<%# Eval("id_ruta")%>" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                               <td>2</td>
                                                                               <td>A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.</td>
                                                                               <td>
                                                                                   <div style="overflow-y:auto; text-align:left; max-width:100%; max-height:300px;">
                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.
                                                                                    </div>

                                                                               </td>
                                                                               <td>2017-02-28</td>
                                                                               <td>--</td>
                                                                               <td>22.79%</td>
                                                                               <td>32,503,553.34</td>
                                                                               <td>                                                                       
                                                                                  <span class="label label-warning">Pending&nbsp;<i class="fa fa-clock-o"></i>&nbsp;6&nbsp;Days</span>
                                                                               </td>
                                                                            </tr>
                                                                            <tr>
                                                                               <td colspan="9">                                                                     

                                                                                    <div class="progress">
                                                                                        <div class="progress-bar  progress-bar-warning  progress-bar-striped" role="progressbar" aria-valuenow="22" aria-valuemin="0" aria-valuemax="100" style="width: 22%">
                                                                                          <span >22% (On Time)</span>
                                                                                        </div> 
                                                                                        <div class="progress-bar progress-bar-primary progress-bar-striped" role="progressbar" aria-valuenow="8" aria-valuemin="0" aria-valuemax="100" style="width: 8%">
                                                                                          <span>30% (Planned)</span>
                                                                                        </div>                                                                            
                                                                                      </div>
                                                                               </td>  
                                                                            </tr>                                                                                            
                                                              </tbody>
                                                            </table>--%>
                                                             <%-- <div ID="ltr_rows_Deliverables" runat="server" ></div>--%>                                              
                                             
                                                            <div class="col-sm-2 text-left">
                                                             <!--Tittle -->
                                                                <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                            </div>
                                                           <div class="col-sm-8">                                                   
                                                                  <div ID="dv_description" Runat="server" class=" text-justify ">

                                                                  </div>
                                                               <!--Control -->                                              
                                                           </div>   
                                                           <div class="col-sm-12 text-left">
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
                                                        </div><!-- /.box-body -->

                                                          <div class="box-footer clearfix">                                                

                                                                <div class="col-sm-12">
                                                                  <hr style="border-color:black;" />
                                                               </div>    
                                                   
                                                           </div> <!-- /.box-footer -->                                                                                            
                                         
                                                </div><!-- /.box -->

                                            </div>
                            
                                        </div>                             
                          
                                          <div class="form-group row">
                                            <div class="col-sm-12 text-left">


                                                              <!-- TABLE:  DELIVERABLE RESULT-->
                                                              <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">REPORTED INDICATOR</h3>
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding">
                                                                    <div class="table-responsive ">    
                                                                          
                                                                              <!--**************************HERE THE INDICATOR RESULT*************************************-->
                                                                 
                                                                              <table class="table table-striped">
                                                                                <tr>
                                                                                  <th></th>
                                                                                  <th style="width:50%;">Indicator</th>
                                                                                  <th>Target</th>
                                                                                  <th>Prev Progress</th>
                                                                                  <th>Curr Progress</th>             
                                                                                  <th></th>                                             
                                                                                </tr>
                                                                                   <asp:Repeater ID="reptTable" runat="server" OnItemDataBound="reptTable_ItemDataBound" >
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                              <td>
                                                                                                  <asp:CheckBox ID="chk_sel_indicator" runat="server" Checked="true"  />&nbsp;&nbsp;&nbsp;<%# Eval("codigo_indicador")%>
                                                                                                  <asp:HiddenField runat="server" ID="hd_id_meta_indicador_ficha" Value=<%# Eval("id_meta_indicador_ficha")%>  />
                                                                                                  <asp:HiddenField runat="server" ID="hd_deliverable_reported" Value="0"  /> 
                                                                                              </td>
                                                                                              <td class="text-justify">    
                                                                                                <%# Eval("nombre_indicador_LB")%>                                                                                                                                                         
                                                                                              </td>
                                                                                              <td><%# Eval("meta_total")%></td>
                                                                                              <td><%# Eval("avance_previo")%></td>
                                                                                              <td class="text-bold" ><telerik:RadNumericTextBox  runat="server" ID="txt_value" name="txt_value"  Width="125px" Value="0" ></telerik:RadNumericTextBox>
                                                                                                  <asp:HiddenField runat="server" ID="hd_value" Value=<%# Eval("avance_actual")%>  />                                                                                                  
                                                                                              </td>                                                         
                                                                                              <td><span class="badge <%# Eval("bg_color")%>"> <%# Eval("porc_total")%>%</span></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="6">
                                                                                                    <div class="box-body no-padding  pull-right">   
                                                                                                        
                                                                                                        
                                                                                                                <div class="small-box-XR box-default">
                                                                                                                    <div class="box-header with-border">
                                                                                                                      <h3 class="box-title"><%# Eval("codigo_indicador")%> - reports</h3>
                                                                                                                         <div class="box-tools pull-right">
                                                                                                                            <span class="label  label-default"><%# Eval("reports")%> Reports</span>
                                                                                                                            <button class="btn btn-box-tool"  onclick="$('#reports_<%# Eval("id_meta_indicador_ficha")%>').addClass('collapse');" ><i class="fa fa-minus"></i></button>                                                                                                
                                                                                                                        </div>
                                                                                                                    </div><!-- /.box-header -->
                                                                                                                    <div id="reports_<%# Eval("id_meta_indicador_ficha")%>"  style="padding-left:10%;" >
                                                                                                                    
                                                                                                                            <table class="table table-condensed">
                                                                                                                                <tr>
                                                                                                                                  <th style="width: 10px" >No</th>
                                                                                                                                  <th style="width: 50px">Period</th>
                                                                                                                                  <th style="width: 100px">Progress</th>
                                                                                                                                  <th style="width: 100px">To Report</th>
                                                                                                                                  <th style="width: 100px"></th>                                                                                                                  
                                                                                                                                </tr>
                                                                                                                                     <asp:Repeater ID="reptTable_Ind" runat="server" OnItemDataBound="reptTable_Ind_ItemDataBound" >
                                                                                                                                       <ItemTemplate>
                                                                                                                                         <tr>
                                                                                                                                            <td class="text-bold">
                                                                                                                                                <%# Eval("N")%>
                                                                                                                                                <asp:HiddenField runat="server" ID="hd_id_meta_indicador_ficha" Value=<%# Eval("id_meta_indicador_ficha")%>  />
                                                                                                                                                <asp:HiddenField runat="server" ID="hd_id_avance_meta_indicador" Value=<%# Eval("id_avance_meta_indicador")%>  />
                                                                                                                                                <asp:HiddenField runat="server" ID="hd_id_deliverable_result" Value=<%# Eval("id_deliverable_results")%>  />
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <%# Eval("fiscal_year")%>
                                                                                                                                            </td>
                                                                                                                                             <td>
                                                                                                                                                <%# Eval("valor_avance")%>
                                                                                                                                                <asp:HiddenField runat="server" ID="hd_ind_report_value"  Value=<%# Eval("valor_avance") %>  />
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <asp:HiddenField runat="server" ID="hd_ind_value" Value=<%# If((Eval("id_deliverable_results") = 0), Eval("valor_avance"), Eval("Reported")) %>  />
                                                                                                                                                <telerik:RadNumericTextBox  runat="server" ID="txt_RepValue_" name="txt_RepValue_"  MinValue="0" MaxValue="9999999999" Width="125px" Value="0" >                                                                                                                                     
                                                                                                                                                </telerik:RadNumericTextBox>
                                                                                                                                             </td>
                                                                                                                                              <td>
                                                                                                                                                  &nbsp;
                                                                                                                                              </td>
                                                                                                                                         </tr>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:Repeater>
                                                                                                                             </table>

                                                                                    
                                                                                                                    </div><!-- /.box-body -->
                                                                                                                    <div class="small-box-footer text-center">
                                                                                                                      <a href="javascript::" class="uppercase">&nbsp;</a>
                                                                                                                    </div><!-- /.box-footer -->
                                                                                                                  </div><!--/.box -->
                                                                                                                 <!-- INDICATORS REPORTS -->
                                                                                                        
                                                                                                                                                                                                         
                                                                                                         
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                               <td colspan="6">
                                                                                                <div class="progress">
                                                                                                    <div class="progress-bar <%# Eval("progress_bar_previo")%>" role="progressbar<%# Eval("id_meta_indicador_ficha")%>" aria-valuenow="<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo")) %>" aria-valuemin="0" aria-valuemax="100" style="width:<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo"))%>%">
                                                                                                      <span><%# Eval("porc_previo")%>%</span>
                                                                                                    </div>
                                                                                                    <div class="progress-bar <%# Eval("progress_bar_actual")%>" role="progressbar<%# Eval("id_meta_indicador_ficha")%>" aria-valuenow="<%# If(Eval("porc_actual") > 100, 100, Eval("porc_actual"))%>" aria-valuemin="0" aria-valuemax="100" style="width:<%# If(Eval("porc_actual") > 100, 100, Eval("porc_actual")) %>%">
                                                                                                      <span ><%# Eval("porc_actual")%>%</span>
                                                                                                    </div>
                                                                                                  </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                 </asp:Repeater>                                                                             
                                                                                    <%--    <div class="progress progress-xs">
                                                                                              <div class="progress-bar <%# Eval("progress_bar")%>" style="width:<%# Eval("progress_value")%>%"></div>
                                                                                            </div>
                                                                                                                                                                              
                                                                                            <div class="progress">
                                                                                                    <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span>11% (Planned)</span>
                                                                                                    </div>
                                                                                                    <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span >22% (Delayed)</span>
                                                                                                    </div>
                                                                                                  </div>--%>                                                                                                                                                                                                                   
                                                                              </table>

                                                                                <!--**************************HERE THE INDICATOR RESULT*************************************-->

    					                                             </div><!-- /.table-responsive -->
                                                                </div><!-- /.box-body -->

                                                                <div class="box-footer clearfix"> 
                                                                   <%--<div class="col-sm-12">
                                                                      <hr style="border-color:black;" />
                                                                   </div>  --%>  
                                                                </div> <!-- /.box-footer -->                                                                                            
                                         
                                                        </div><!-- /.box -->
                                                        <!-- TABLE:  DELIVERABLE RESULT-->                                                                       

                                                 </div>
                                            
                                          </div>
                                                                            
                                            <div class="form-group row">
                                               <div class="col-sm-12 text-left">

                                                      <!-- TABLE:  INDICATOR RESULT--> 
                                                   
                                                            <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">INDICATOR RESULT</h3>
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding">
                                                                    <div class="table-responsive ">    
                                                                          
                                                                              <!--**************************HERE THE INDICATOR RESULT*************************************-->
                                                                 
                                                                              <table class="table table-striped">
                                                                                <tr>
                                                                                  <th> </th>
                                                                                  <th style="width:50%;">Indicator</th>
                                                                                  <th>Targuet</th>
                                                                                  <th>Progress</th>                                                                                  
                                                                                  <th></th>                                             
                                                                                </tr>
                                                                                   <asp:Repeater ID="reptTable_2" runat="server">
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                              <td><%# Eval("codigo_indicador")%></td>
                                                                                              <td class="text-justify">    
                                                                                                <%# Eval("nombre_indicador_LB")%>                                                                                                                                                         
                                                                                              </td>
                                                                                              <td><%# Eval("meta_total")%></td>
                                                                                              <td class="text-bold"  ><%# Eval("avance_previo")%></td>                                                                                              
                                                                                              <td><span class="badge <%# Eval("bg_color")%>"> <%# Eval("porc_total")%>%</span></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                               <td colspan="5">
                                                                                                <div class="progress">
                                                                                                    <div class="progress-bar <%# Eval("progress_bar_previo")%>" role="progressbar<%# Eval("id_meta_indicador_ficha")%>" aria-valuenow="<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo")) %>" aria-valuemin="0" aria-valuemax="100" style="width:<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo"))%>%">
                                                                                                      <span><%# Eval("porc_previo")%>%</span>
                                                                                                    </div>                                                                                                
                                                                                                  </div>
                                                                                               </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                 </asp:Repeater>                                                                             
                                                                                    <%--    <div class="progress progress-xs">
                                                                                              <div class="progress-bar <%# Eval("progress_bar")%>" style="width:<%# Eval("progress_value")%>%"></div>
                                                                                            </div>
                                                                                                                                                                              
                                                                                            <div class="progress">
                                                                                                    <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span>11% (Planned)</span>
                                                                                                    </div>
                                                                                                    <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span >22% (Delayed)</span>
                                                                                                    </div>
                                                                                                  </div>--%>                                                                                                                                      
                                                                              
                                                                              </table>

                                                                                <!--**************************HERE THE INDICATOR RESULT*************************************-->

    					                                             </div><!-- /.table-responsive -->
                                                                </div><!-- /.box-body -->

                                                                <div class="box-footer clearfix"> 
                                                                   <div class="col-sm-12">
                                                                      <hr style="border-color:black;" />
                                                                   </div>    
                                                                </div> <!-- /.box-footer -->                                                                                            
                                         
                                                                </div><!-- /.box -->   
                                                   
                                                     
                                                      <!-- TABLE:  INDICATOR RESULT-->          

                                               </div>
                                            </div>                                 
                                                  
                                            <%--<div class="form-group row">
                                               <div class="col-sm-12 text-left">

                                                      <!-- TABLE:  SUPPORT DOCUMENT--> 

                                                       <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">INDICATOR RESULT</h3>
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>                                                                  
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding">
                                                                    <div class="table-responsive ">    
                                                                          
                                                                        <!--**************************HERE SUPPORT DOCUMENTS*************************************-->
                                                                 
                                                                                        <div class="col-md-6">
                                                                                          <!-- INDICATORS REPORTS -->
                                                                                          <div class="small-box-XR box-default">
                                                                                            <div class="box-header with-border">
                                                                                              <h3 class="box-title">Indicator Reports</h3>
                                                                                              <div class="box-tools pull-right">
                                                                                                   <span class="label  label-default">10 Reports</span>
                                                                                                   <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>                                                                                                
                                                                                              </div>
                                                                                            </div><!-- /.box-header -->
                                                                                            <div class="box-body no-padding">

                                                                                                Reports Here
                                                                                    
                                                                                            </div><!-- /.box-body -->
                                                                                            <div class="small-box-footer text-center">
                                                                                              <a href="javascript::" class="uppercase">View All Users</a>
                                                                                            </div><!-- /.box-footer -->
                                                                                          </div><!--/.box -->
                                                                                         <!-- INDICATORS REPORTS -->
                                                                                        </div><!-- /.col -->
                                                                                                                                                 
                                                                        <!--**************************HERE SUPPORT DOCUMENTS*************************************-->

    					                                             </div><!-- /.table-responsive -->
                                                                </div><!-- /.box-body -->

                                                                <div class="box-footer clearfix"> 
                                                                   <div class="col-sm-12">
                                                                      <hr style="border-color:black;" />
                                                                   </div>    
                                                                </div> <!-- /.box-footer -->                                                                                            
                                         
                                                                </div><!-- /.box -->   
                                                   

                                                      <!-- TABLE:  SUPPORT DOCUMENT-->          

                                               </div>
                                            </div>--%>


                                  </div>
                                
                               </div> 
              
                          </div>
                        
                   </div>

                  <%--  </div>--%>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->                    
                       
			          <div class="form-group row">
                             <div class="col-sm-2 text-left">
                               <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm " Width="100" ValidationGroup="1" ></telerik:RadButton>
                             </div>
                            <div class="col-sm-2 text-left">
                               <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Save and Continue" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-o-right"></i></asp:LinkButton>                                              
                            </div>
                            <div class="col-sm-8 text-left">
                               <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                            </div>
                       </div>       
                                                                
                   </div>
                
                <%--</div>--%>

           </section>



    
    

    </asp:Content>

