<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivitySTO.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivitySTO" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="timeline" Src="~/Controles/ctrl_timeline_activity.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Activity Management</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">

                     <div class="col-sm-11">   
                            <h3 class="box-title">
                                <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activity - </asp:Label>
                                <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                                <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                                <asp:HiddenField ID="tasaCambio" runat="server" />
                            </h3>
                        </div>
                         <div class="col-sm-1 text-right">   
                            <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                        </div>

            </div>
            <div class="box-body">


                 <div class="col-lg-12">

                    <div class="panel panel-default">
                       <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                          <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" href="#collapseOne"
                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_ActivityTIME">Activity Timeline&nbsp;&nbsp;<i class='fa  fa-caret-square-o-down'></i></a>
                          </h4>
                       </div>
                        <div id="collapseOne" class="panel-collapse collapse no-margin" role="tabpanel" aria-labelledby="headingOne">
                             <div class="panel-body no-margin no-padding">
                                <uc:timeline id="timeline_activity" runat="server" />
                             </div>
                        </div>                        
                   </div>                

                </div>


                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <ul class="nav nav-tabs">
                              
                                 <li role="presentation" class="active"><a class="primary" runat="server" id="alink_definicion" href="#">ACTIVITY</a></li>                               
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>
                                <li role="presentation" ><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>   
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>   

                                <li role="presentation"><a  runat="server" id="alink_stos">_</a></li>
                                <li role="presentation"><a  runat="server" id="alink_po">_</a></li>
                                <li role="presentation"><a  runat="server" id="alink_Ik">_</a></li>

                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:HiddenField runat="server" ID="hd_tp_activity" Value="STO"  />
                        <div class="panel-body div-bordered">
                           
                                   <div class="form-group row">

                                         <div class="col-sm-12">
                                               <div class="box-body">                                 
                                                    <div class="col-md-4 col-sm-8 col-xs-16">
                                                      <div class="info-box">
                                                        <span class="info-box-icon bg-gray"><i class="fa fa-university"></i></span>
                                                        <div class="info-box-content">
                                                             <span class="info-box-text"><asp:Label ID="lblt_implementer" runat="server">Implementer</asp:Label></span>
                                                             <span class="text-bold"><asp:Label ID="lbl_implementer" runat="server"></asp:Label></span>             
                                                        </div><!-- /.info-box-content -->
                                                      </div><!-- /.info-box -->
                                                    </div><!-- /.col -->
                                                     <div class="col-md-4 col-sm-8 col-xs-16">
                                                      <div class="info-box">
                                                         <span class="info-box-icon  bg-gray"><i class="fa fa-tasks"></i></span>
                                                        <div class="info-box-content">
                                                             <span class="info-box-text"><asp:Label ID="lbl_activity_name" runat="server"></asp:Label></span>                                                           
                                                             <span class="text-bold"><asp:Label ID="lbl_activity_Code" runat="server"></asp:Label></span>                                                                          
                                                        </div><!-- /.info-box-content -->
                                                      </div><!-- /.info-box -->
                                                    </div><!-- /.col -->
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                          <div class="info-box">
                                                              <span class="info-box-icon bg-gray"><i class="fa fas fa-calendar-check"></i></span>
                                                              <div class="info-box-content">
                                                                 <span class="info-box-text"><asp:Label ID="lbl_last_Deliverable" runat="server"></asp:Label></span>
                                                                 <span class="text-bold"><asp:Label ID="lbl_status_Deliverable" runat="server"></asp:Label>&nbsp;&nbsp;<i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>             
                                                                 <%--&nbsp;--&nbsp;<asp:Label ID="lblt_date_status" runat="server"></asp:Label><br /><i class="fa fa-clock-o"></i><asp:Label ID="lblt_time_status" runat="server"></asp:Label>--%>
                                                              </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                      </div><!-- /.col -->                                                   
                                                </div>
                                              </div>

                                       
                                               <div class="col-sm-12">
                                                    <div class="box-body">    

                                                          <div class="col-md-6 col-sm-6 col-xs-16">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fas fa-sack-dollar"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT2_usd" Text ="0"></asp:Label></span>
                                                                     <span class="text-bold">Total <asp:Label runat="server" ID="lbl_total_type" Text =""></asp:Label></span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->
                                                                                                                

                                                         <div class="col-md-6 col-sm-6 col-xs-16">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-line-chart"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalPerf2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalPerf2_usd" Text =""></asp:Label></span>
                                                                     <span class="text-bold">Total Amount Obligated</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->
                                                                                                               

                                                   </div>
                                               </div>     

                                             <div class="col-sm-12">
                                                <div class="box-body">    

                                                        <div class="col-md-6 col-sm-6 col-xs-16 vertical-align-center"  >                                                                                                                                                                                                                        
                                                            
                                                             <div style="width: 250px; height: 200px; margin: 0 auto">
                                                                <div id="container-time" style="width: 250px; height: 160px; margin: 0 auto; text-align: center;"></div>
                                                                <input style="display: none;" id="valor_avance_time" runat="server" />
                                                                <h3 runat="server" id="txt_avance_time" style="margin: 0 auto;text-align: center;font-family: Tahoma;font-weight: 600; font-size: 15px;"></h3>
                                                            </div>
                                                                                                                        
                                                         </div> 


                                                        <div class="col-md-6 col-sm-6 col-xs-16 vertical-align-center"  >                                                                                                                                                                                                                        
                                                            
                                                             <div style="width: 250px; height: 200px; margin: 0 auto">
                                                                <div id="container-money" style="width: 250px; height: 160px; margin: 0 auto; text-align: center;"></div>
                                                                <input style="display: none;" id="valor_avance" runat="server" />
                                                                <h3 runat="server" id="txt_avance" style="margin: 0 auto;text-align: center;font-family: Tahoma;font-weight: 600; font-size: 15px;"></h3>
                                                            </div>
                                                                                                                        
                                                       </div> 


                                                 </div>
                                             </div>

                                          
                                                                         
                                      </div>
                                    
                            <div class="form-group row">
                                     <div class="col-sm-12">
                                           <!-- TABLE: ACTIVITY DELIVERABLE -->
                                              <div class="box box-info">
                                                <div class="box-header with-border">
                                                  <h3 class="box-title"><asp:Label runat="server" ID="lbl_total_tipo_3" Text ="SUB TASK ORDERS"></asp:Label></h3>
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
                                                          <th style="width:20%;">RMS Code</th>                    
                                                          <th style="width:20%;">Technical Code</th>
                                                          <th style="width:35%;">STO</th>
                                                          <th style="width:8%;">Start Date</th>
                                                          <th style="width:8%;">End Date</th>
                                                          <th style="width:3%;">%</th>
                                                          <th style="width:8%;">Amount</th>
                                                          <th style="width:8%;">Status</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody>                                                   
                                                                    <tr>
                                                                       <td><div class="tools"><a href="/RMS_SIME/Proyectos/frm_proyectosEdit?Id=1004" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                       <td>PF-AP-2019-SCT-001-STO-001</td>
                                                                       <td>PF-AP-2019-SCT-001-STO-001</td>
                                                                       <td>
                                                                           <div style="overflow:scroll; text-align:left; max-width:100%; max-height:300px;">
                                                                               STO #1 Description
                                                                            </div>
                                                                       </td>
                                                                       <td>01-01-2019</td>
                                                                       <td>31-01-2019</td>
                                                                       <td>31.07%</td>
                                                                       <td>25,000,000</td>
                                                                       <td>                                                                       
                                                                          <span class="label label-danger">Expired&nbsp;<i class="fa fa-clock-o"></i>&nbsp;32&nbsp;Weeks</span>
                                                                       </td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="9">                                                                     

                                                                            <div class="progress">
                                                                                <div class="progress-bar progress-bar-warning progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                  <span>19% (Planned)</span>
                                                                                </div>
                                                                                <div class="progress-bar progress-bar-danger   progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                  <span >66% (Delayed)</span>
                                                                                </div>
                                                                            </div>

                                                                       </td>  
                                                                    </tr>                      
                                                      
                                                                     <tr>
                                                                       <td><div class="tools"><a href="/RMS_SIME/Proyectos/frm_proyectosEdit?Id=1005" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                       <td>PF-AP-2019-SCT-001-STO-002</td>
                                                                       <td>REV-P&B</td>
                                                                       <td>
                                                                           <div style="overflow:scroll; text-align:left; max-width:100%; max-height:300px;">
                                                                               STO #2 Description
                                                                            </div>
                                                                       </td>
                                                                       <td>01/02/2019</td>
                                                                       <td>28/02/2019</td>
                                                                       <td>18.88%</td>
                                                                       <td>14,663</td>
                                                                       <td>                                                                       
                                                                          <span class="label label-danger">Expired&nbsp;<i class="fa fa-clock-o"></i>&nbsp;28&nbsp;Weeks</span>
                                                                       </td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="9">                                                                     

                                                                            <div class="progress">
                                                                                <div class="progress-bar  progress-bar-danger   progress-bar-striped" role="progressbar" aria-valuenow="22" aria-valuemin="0" aria-valuemax="100" style="width: 22%">
                                                                                  <span >35% (Planned)</span>
                                                                                </div> 
                                                                                <div class="progress-bar progress-bar-primary progress-bar-striped" role="progressbar" aria-valuenow="8" aria-valuemin="0" aria-valuemax="100" style="width: 8%">
                                                                                  <span>50% (Expired)</span>
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
                                                                  <h4>Total <asp:Label runat="server" ID="lbl_total_tipo_2" Text =""></asp:Label> <asp:Label runat="server" ID="lbl_totalACT" Text ="0"></asp:Label></h4>
                                                                  <h4><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0"></asp:Label></h4><hr style="border-color:black;" />
                                                                  <h4>Total Obligated: <asp:Label runat="server" ID="lbl_totalPerf" Text ="0"></asp:Label></h4>
                                                                  <h4><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0"></asp:Label></h4><hr style="border-color:black;" />
                                                                  <h3>Pending: <asp:Label runat="server" ID="lbl_totalPend" Text ="0"></asp:Label></h3>
                                                                  <h3><asp:Label runat="server" ID="lbl_totalPend_usd" Text ="0"></asp:Label></h3>
                                                             </div>
                                                       </div>

                                                        <div class="col-md-3 pull-right vertical-align-center"  >                                                                                                                                                             
                                                           <div style="text-align:right; vertical-align:middle;">   
                                                               <asp:HiddenField ID="hd_percent" runat="server" Value="0" />  
                                                               <input type="text" class="PorcDeliv" value="0" data-width="160" data-height="160" data-thickness="0.30" data-fgColor="#3c8dbc" data-skin="tron" readonly>                                                   
                                                           </div>
                                                       </div> 
                                                                   
                                                    </div>
                                        </div>   

                                        <div class="row">  
                                         <div class="col-sm-12">
                                             <hr style="border-color:black;" />
                                          </div>  
                                       </div>  
                                    

                                 </div>


                            
                                                    
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                       
                    </div>
                </div>
                <!-- /.box-footer -->
                <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar Registro</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
      <%--              <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
                        <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
                        <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
                        <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>--%>

     <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js")%>"></script>     
     <script src="<%=ResolveUrl("~/Content/dist/js/IQS_graph.js?time=0.0013")%>"></script>
     <script src="../Content/plugins/knob/jquery.knob.js"></script>

      <script type="text/javascript">
             
          //$(document).ready(function () {
              //set_Chart_IQS(25.76);
              //set_Chart_IQS_time(56.68);
          //});          

          //$(document).ready(function () {
              
          //    var hdValueTotal = $('input[id*=hd_percent]');
          //    // alert(hdValueTotal.val());

          //    $('.PorcDeliv').val(hdValueTotal.val());
          //    $('.PorcDeliv').trigger('change');
          //    $('.PorcDeliv').css('font-size', '18px');
          //    $(".PorcDeliv").knob({
          //        'format': function (value) {
          //            return value + '%';
          //        }
          //    });                        


          //});


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

        
     </script>
</asp:Content>
