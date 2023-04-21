<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_DeliverableAdd"  Codebehind="frm_DeliverableAdd.aspx.vb" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLE</asp:Label>
                </h1>
            </section>
            <section class="content">

                <div class="box">

                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Add Deliverable</asp:Label>
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
                                                        <asp:Label runat="server" ID="lblt_deliverable_result">Deliverable Result</asp:Label>
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
                                          <div class="col-sm-12 text-left">
                                                
                                                 <div class="col-sm-2 text-left">

                                                 </div> 

                                                <div class="col-sm-1 text-left">
                                                     <!--Tittle -->
                                                        <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label>   
                                                       <asp:Label ID="lbl_idTemp" runat="server" Visible="False"></asp:Label>
                                                       <asp:Label ID="lblt_Employee_associated" runat="server" CssClass="control-label text-bold"   Text="Activity"></asp:Label>
                                                 </div>
                                                 <div class="col-sm-6">
                                    
                                                            <!--Control -->
                                                             <telerik:RadComboBox  ID="cmb_activity" 
                                                                                   runat ="server" 
                                                                                    CausesValidation="False"                                                                     
                                                                                    EmptyMessage="Select an activity..."   
                                                                                    AllowCustomText="true" 
                                                                                    Filter="Contains"                                                  
                                                                                    Height="200px"
                                                                                    Width="80%"
                                                                                    OnDataBound="cmb_activity_DataBound" 
                                                                                    OnItemDataBound="cmb_activity_ItemDataBound"     
                                                                                    OnClientItemsRequested="UpdateItemCountField"                                                                                                                                             
                                                                                    OnClientSelectedIndexChanged ="getActivity" >                                                              
                                                                            <HeaderTemplate>
                                                                                    <ul>
                                                                                        <li style="font-weight:700;" >Activity Code / Status</li>
                                                                                        <li style="font-weight:100;" >Activity Name</li> 
                                                                                        <li style="font-weight:100;" >Implementer</li>  
                                                                                        <li style="font-weight:500;" >Term</li>                                                                                                                                                                                                                                     
                                                                                    </ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li style="font-weight:700;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "codigo_SAPME")%> -- <%# DataBinder.Eval(Container.DataItem, "nombre_estado_ficha")%> 
                                                                                    </li>
                                                                                    <li style="font-weight:100;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "nombre_proyecto")%>  
                                                                                    </li>
                                                                                     <li style="font-weight:500; color:#ED7620;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "nombre_ejecutor")%>  
                                                                                    </li>
                                                                                    <li style="font-weight:500;" >
                                                                                       <span style="font-weight:700;"  >From</span> <%#  DataBinder.Eval(Container.DataItem, "fecha_inicio_proyecto", "{0:d}")%> <span style="font-weight:700;"  >to</span> <%# DataBinder.Eval(Container.DataItem, "fecha_fin_proyecto", "{0:d}")%>
                                                                                    </li>                                                                                    
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            A total of
                                                                            <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                                            items
                                                                        </FooterTemplate>
                                                            </telerik:RadComboBox>

                                                       

                                                           <%--DataSourceID="sql_rol"--%>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                                ControlToValidate="cmb_activity" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                                
                                                          <script src="../Content/plugins/knob/jquery.knob.js"></script>

                                                          <script type="text/javascript">

                                                              $(document).ready(function () {
                                                                           
                                                                
                                                                  var hdValueTotal = $('input[id*=hd_percent]');
                                                                 // alert(hdValueTotal.val());
                                                                  
                                                                  $('.PorcDeliv').val(hdValueTotal.val());
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '18px');
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });
                                                                })
                                                              
                                                             
                                                              function UpdateItemCountField(sender, args) {
                                                                  //set the footer text
                                                                 sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
                                                              }

                                                              function SelectComboBoxItem(itemText) {

                                                                  var combo = $find("<%= cmb_activity.ClientID %>");
                                                                  var item = combo.findItemByText(itemText);
                                                                  //alert(itemText);
                                                                    if (item) {                                                                       
                                                                        item.select();
                                                                    }
                                                              }


                                                              function set_Percent(valPercent) {

                                                                  //alert(valPercent);

                                                                  $('.PorcDeliv').val(valPercent);
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '18px');
                                                                  //$(".PorcDeliv").knob();
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });

                                                              }
                                                                                                                            

                                                           function getActivity(sender, eventArgs) {

                                                                 var combobox = $find("<%= cmb_activity.ClientID %>");
                                                                
                                                                 var value = combobox.get_selectedItem().get_value();
                                                                 var texto = combobox.get_selectedItem().get_text();
                                                                                                                              
                                                                 var strText = texto.split('==>>');

                                                                 $('#<%=lbl_activity_Code.ClientID %>').text(strText[0]);
                                                                 $('#<%=lbl_activity_name.ClientID %>').text(strText[1]);
                                                                                                                              

                                                                 $('#<%=lbl_period.ClientID %>').text(strText[2] + ' to ' + strText[3]);
                                                                 
                                                               //alert('{idFichaProyecto:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }');
                                                               //, UserSes:"' + <%=cl_user%> + '"
                                                               //url: "frm_DeliverableAdd.aspx/GetDeliverables",
                                                               //url: "/WebMethods/App_Deliverables.asmx/GetDeliverables",

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
                                                                         $('.PorcDeliv').css('font-size', '18px');
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
                                                 
                                                             }
                                                             
                                                       </script>  

                                                 </div>
                                         </div>
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
                                      </div>
                                              
                                
                             <div class="form-group row">
                                 <div class="col-sm-12">
                                     <hr /> 
                                 </div>
                             </div>  
                                    
                             <div class="form-group row">
                                 <div class="col-sm-12">
                                       <!-- TABLE: ACTIVITY DELIVERABLE -->
                                          <div class="box box-info">
                                            <div class="box-header with-border">
                                              <h3 class="box-title">ACTIVITY DELIVERABLES</h3>
                                              <div class="box-tools pull-right">
                                                  <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>                                                
                                              </div>
                                            </div><!-- /.box-header -->
                                            <div id="deliv_tab" class="box-body">
                                              <div class="table-responsive">
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
                                                  <div ID="ltr_rows_Deliverables" runat="server" ></div>
                                              </div><!-- /.table-responsive -->
                                            </div><!-- /.box-body -->

                                            <div class="box-footer clearfix">                                                
                                                                                                    
                                                     
                                            </div> <!-- /.box-footer -->                                             
                                               
                                          <%--</div>--%>
                                    </div><!-- /.box -->

                                </div>
                            
                            </div>  



                            <div class="small-box-XR box-warning" >                                                                 
                              <br />
                                 <div class="row">  
                                     
                                                  <div class="col-sm-12">
                                                        <div class="col-sm-4 pull-right">
                                                         <div style="text-align:right;">                                                 
                                                              <h4>Total Activity: <asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label></h4>
                                                              <h4><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                              <h4>Total Performed: <asp:Label runat="server" ID="lbl_totalPerf" Text ="0 UGX"></asp:Label></h4>
                                                              <h4><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                              <h3>Pending: <asp:Label runat="server" ID="lbl_totalPend" Text ="0 UGX"></asp:Label></h3>
                                                              <h3><asp:Label runat="server" ID="lbl_totalPend_usd" Text ="0 USD"></asp:Label></h3>
                                                         </div>
                                                       </div>

                                                        <div class="col-md-3 pull-right vertical-align-center"  >                                                                                                                                                             
                                                           <div style="text-align:right; vertical-align:middle;">   
                                                               <asp:HiddenField ID="hd_percent" runat="server" Value="0" />  
                                                               <input type="text" class="PorcDeliv" value="0" data-width="140" data-height="140" data-thickness="0.30" data-fgColor="#3c8dbc" data-skin="tron" readonly>                                                   
                                                           </div>
                                                       </div> 

                                                       <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                                                       <asp:HiddenField runat="server" ID="hd_id_ficha_entregable" Value="0" />
                                                       <asp:HiddenField ID="hd_tasa_cambio" runat="server" Value="0" />
                                                                                                           
                                                       <div id="dvNEXT_delieverable" runat="server" class="box-body table-responsive no-padding">
                                                       </div>     
                                                                                                       
                                                       <div id="dvNEXT_delieverable2" runat="server" class="box-body table-responsive no-padding">
                                                                                                                 
                                                            <table class='table table-hover'>
                                                                <asp:Repeater ID="rep_DelivINFO" runat="server" >
                                                                    <ItemTemplate>                                                                                   
                                                                            <tr>
                                                                              <th>Deliverable #</th>
                                                                              <td><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("numero_entregable")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                            </tr>
                                                                            <tr>                                                                  
                                                                              <td colspan='2'>
                                                                                 <div style='text-align:left; max-width:100%;'>
                                                                                     <%# Eval("descripcion_entregable") %>
                                                                                 </div>
                                                                               </td>                                                                  
                                                                            </tr>
                                                                            <tr>
                                                                              <th>Due Date</th>
                                                                              <td> <%# String.Format("{0:d}", Eval("fecha"))%> </td>   
                                                                            </tr>     
                                                                            <tr>
                                                                              <th>Status</th>
                                                                              <td><span class='label  <%# Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 1) %>' ><%# If(Eval("numero_entregable") = 0, Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 2), Eval("deliverable_estado"))  %> &nbsp;<i class='fa fa-clock-o'></i>&nbsp;<%# Func_Unit(Eval("D_Days"))%></span></td>   
                                                                            </tr>  
                                                                             <tr>
                                                                              <th>Porcent</th>
                                                                              <td><%# String.Format("{0:P2}", (Eval("porcentaje") / 100))%></td>   
                                                                            </tr>   
                                                                            <tr>
                                                                              <th>Amount</th>
                                                                              <td> <%# String.Format(cultureUSer, "{0:N2} {1}", Eval("valor"), cultureUSer.NumberFormat.CurrencySymbol) %> / <%# String.Format("{0:N2}", Math.Round((Eval("valor") / Convert.ToDouble(Eval("tasa_cambio"))), 2, MidpointRounding.AwayFromZero))%> USD</td>   
                                                                            </tr> 
                                                                    </ItemTemplate>
                                                                </asp:Repeater>                                                             

                                                              </table>

                                                           </div><!-- /.box-body -->
                                                                                                              
                                                    </div>
                                        </div>   

                                        <div class="row">  
                                         <div class="col-sm-12">
                                             <hr style="border-color:black;" />
                                          </div>  
                                       </div>                                               

                                     <div class="row">                                                              
                                          <div class="form-group row">
                                                    <div class="col-sm-2 text-left">
                                                     <!--Tittle -->
                                                        <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                    </div>
                                                   <div class="col-sm-8">
                                                       <!--Control -->
                                                       <telerik:RadTextBox ID="txt_description" Runat="server"  EmptyMessage="Type Description here.." Width="90%"  ValidationGroup="1" TextMode="MultiLine" Columns="40" Rows="6" MaxLength="249">
                                                       </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_description" ValidationGroup="1"></asp:RequiredFieldValidator>
                                          
                                                   </div>                                        
                                                </div>

                                                <div class="form-group row">
                                                    <div class="col-sm-10 text-left">
                                                         <hr />
                                                    </div>
                                                </div>
                                   
                                               <div class="form-group row">
                                                    <div class="col-sm-2 text-left">
                                                     <!--Tittle -->
                                                      <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                                    </div>
                                                   <div class="col-sm-8">
                                                       <!--Control -->
                                                         <telerik:RadTextBox ID="txt_notes" Runat="server" EmptyMessage="Type notes here.."  Width="90%" ValidationGroup="1" Height="100px" TextMode="MultiLine"  MaxLength="999">
                                                         </telerik:RadTextBox>                                            
                                                   </div>
                                                </div>
                                 
                                        </div>   
                                
                                         <div id="divObservation" runat="server" class="form-group row" visible="false"  >
                                               <div class="form-group row">
                                                    <div class="col-sm-10 text-left">
                                                         <hr />
                                                    </div>
                                                </div>
                                                    <div class="col-sm-2 text-left">
                                                     <!--Tittle -->
                                                      <asp:Label ID="lblt_Observation" runat="server" CssClass="control-label text-bold" Text="Approval Observation"></asp:Label>
                                                    </div>
                                                   <div class="col-sm-8">
                                                       <!--Control -->
                                                         <telerik:RadTextBox ID="txt_observation" Runat="server" Width="90%" Height="100px" TextMode="MultiLine" ReadOnly="true">
                                                         </telerik:RadTextBox>                                            
                                                   </div>
                                           </div>


                                 </div>

                                  <div class="box-footer clearfix"> 
                                       <div class="small-box-XR box-warning">
                                           &nbsp;
                                       </div>
                                  </div> <!-- /.box-footer -->            


                             </div>     <%--<div class="box-body">--%>
                                
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

