<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="RMS_APPROVAL._Default" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

     <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
      <!-- iCheck -->
    <link rel="stylesheet" href="plugins/iCheck/flat/blue.css">
    <!-- Date Picker -->
    <link rel="stylesheet" href="plugins/datepicker/datepicker3.css">
    <!-- Daterange picker -->
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker-bs3.css">


    <section class="content-header">       
            <h1> <asp:Label runat="server" ID="lblt_titulo_pantalla">HOME</asp:Label>          
                <small>
                  <asp:Label runat="server" ID="lblt_titulo_sys">APPROVAL</asp:Label>          
               </small>
            </h1>               
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                    <li><a href="#">Examples</a></li>
                    <li class="active">Blank page</li>
                </ol>                    
    </section>

    <!-- Main content -->
    <section class="content">
        <!-- Default box -->
        <div class="box">
            <div class="box-header with-border">

                <div class="col-sm-9">         
                        <h3 class="box-title"><asp:Label runat="server" ID="lblt_sub_titulo_pantalla">Approval System</asp:Label> </h3>
                        <%--<div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="Collapse"><i class="fa fa-minus"></i></button>
                            <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>
                        </div>--%>
                </div>

                <div class="col-sm-3 text-right">   
                    <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('appMainScreen.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                </div>

            </div>


            <div class="box-body">


                <div class="row">

                     <div class="col-lg-3 col-xs-6">     
                         <!-- small box -->
                            <div class="small-box bg-aqua">
                               <div class="inner">
                                   <h3><%=Session("N_app_procc")%></h3>                                     
                                   <p><asp:Label runat="server" ID="lblt_approval_process">Approval Process</asp:Label> </p>
                               </div>
                               <div class="icon">
                                    <i class="ion ion-android-settings"></i>                         
                               </div>
                                <a href="<%=urlSys & "/approvals/frm_consulta_docs.aspx" %>"  Class="small-box-footer"><asp:Label runat = "server" ID="lblt_your_approvals">Your Approvals</asp:Label> <i class="fa fa-arrow-circle-right"></i></a>
                         </div>
                     </div><!-- ./col -->



                       <div class="col-lg-3 col-xs-6">
                          <!-- small box -->
                          <div class="small-box bg-red">
                            <div class="inner">
                               <h3><%=Session("N_app_pending_procc")%></h3>       
                              <p><asp:Label runat="server" ID="lblt_pending_approvals">Pending Approvals</asp:Label></p>
                            </div>
                            <div class="icon">
                              <i class="ion ion-ios-timer"></i>
                            </div>
                            <a href="<%=urlSys & "/approvals/frm_consulta_docsPending.aspx" %>"   class="small-box-footer"><asp:Label runat="server" ID="lblt_your_pending_Approvals">Your Pending Approvals </asp:Label><i class="fa fa-arrow-circle-right"></i></a>
                          </div>
                        </div><!-- ./col -->

                     <div class="col-lg-3 col-xs-6">
                          <!-- small box -->
                          <div class="small-box bg-green">
                            <div class="inner">
                               <h3><%=Session("AVG_app_timing")%><sup style="font-size: 20px"><%=Session("AVG_app_unit")%></sup></h3>
                              <p><asp:Label runat="server" ID="lblt_Response_Average_Timing">Response Average Timing</asp:Label></p>
                            </div>
                            <div class="icon">
                              <i class="fa fa-hourglass-half"></i>
                            </div>
                            <a href="#" class="small-box-footer"><asp:Label runat="server" ID="lblt_More_info">More info</asp:Label><i class="fa fa-arrow-circle-right"></i></a>
                          </div>
                        </div><!-- ./col -->

                         <div class="col-lg-3 col-xs-6">
                              <!-- small box -->
                              <div class="small-box bg-yellow">
                                <div class="inner">
                                 <h3><sub style="font-size:20px;">MAX</sub>  <%=Session("MAX_app_timing")%><sup style="font-size: 20px"><%=Session("MAX_app_unit")%></sup></h3>
                                 <h3><sub style="font-size:20px;">MIN</sub> <%=Session("MIN_app_timing")%><sup style="font-size: 20px"><%=Session("MIN_app_unit")%></sup></h3>
                                  <p><asp:Label runat="server" ID="lblt_Response_timing">Response timing</asp:Label></p>
                                </div>
                                <div class="icon">
                                  <i class="ion ion-ios-pulse-strong"></i>
                                </div>
                                <a href="#" class="small-box-footer"><asp:Label runat="server" ID="lblt_More_info2">More info</asp:Label><i class="fa fa-arrow-circle-right"></i></a>
                              </div>
                            </div><!-- ./col -->
         
                           
                
                </div>
                

                <div class="row">
                    

                        <!-- Left col -->
                           <section class="col-lg-7 connectedSortable">
                                              
                                    
                                     <!-- TO DO List -->
                                          <div class="box box-primary">
                                            <div class="box-header">
                                              <i class="ion ion-gear-b"></i>
                                              <h3 class="box-title"><asp:Label runat="server" ID="lblt_Approvals_to_review">Approvals to review</asp:Label></h3>
                                              <div class="box-tools pull-right">
                                                <ul class="pagination pagination-sm inline">
                                                  <li><a href="#">&laquo;</a></li>
                                                  <li><a href="#">1</a></li>                                                                                                 
                                                  <li><a href="#">&raquo;</a></li>
                                                </ul>
                                              </div>
                                            </div><!-- /.box-header -->
                                            <div class="box-body">
                                             
                                                 <div id="Div_Review" runat="server" visible="false" >
                                                   <ul class="todo-list">   
                                                       <li>                                                 
                                                          <span class="text"><asp:Label runat="server" ID="lblt_Approvals_to_Review_not_found">Approvals to Review were not found</asp:Label></span>                                                  
                                                         </li>
                                                       </ul>
                                                 </div>

                                                 <ul class="todo-list">                                                   
                                                 
                                                 <%--<li>                                                 
                                                  <span class="text">Approvals to Review were not found</span>                                                  
                                                 </li>--%>
                                                                                                      
                                                       <asp:Repeater ID="rept_List" runat="server">
                                                            <ItemTemplate>
                                                                  <li>
                                                                      <!-- drag handle -->
                                                                      <span class="handle">
                                                                        <i class="fa fa-ellipsis-v"></i>
                                                                        <i class="fa fa-ellipsis-v"></i>
                                                                      </span>
                                                                      <!-- checkbox -->
                                                                      <input type="checkbox" value="" name="">
                                                                      <!-- todo text -->
                                                                      <span class="text"><%# Eval("approval")%></span>
                                                                      <!-- Emphasis label -->
                                                                      <small class="label <%# Eval("ico")%>" ><i class="fa fa-clock-o"></i>&nbsp;<%# Eval("value") %>&nbsp;<%# Eval("unit")%></small>
                                                                      <!-- General tools such as edit or delete-->
                                                                      <div class="tools">
                                                                        <a href="<%=urlSys%><%# Eval("url_app")%>" ><i class="fa fa-edit"></i></a>                                                    
                                                                      </div>
                                                                    </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                                                                   
                                              </ul>
                                            </div><!-- /.box-body -->
                                            <div class="box-footer clearfix no-border">
                                              <%--<button class="btn btn-default pull-right"><i class="fa fa-check"></i> Go</button>--%>
                                              <a href="<%=urlSys & "/approvals/frm_consulta_docs.aspx" %>"  class="btn btn-sm btn-default btn-flat pull-right"><i class="fa fa-check"></i><asp:Label runat="server" ID="lblt_Search">Search</asp:Label></a>&nbsp;&nbsp;&nbsp;
                                              <a href="<%=urlSys & "/approvals/frm_docsAD.aspx" %>"  class="btn btn-sm btn-info btn-flat pull-left"><asp:Label runat="server" ID="lblt_New_Approval_Process">New Approval Process</asp:Label></a>
                                              <a href="<%=urlSys & "/approvals/frm_consulta_docsPending.aspx" %>"  class="btn btn-sm btn-default btn-flat pull-right"><asp:Label runat="server" ID="lblt_Ongoing_Approvals">Ongoing Approvals</asp:Label></a>
                                            </div><!-- /.box-footer -->                                            
                                          </div><!-- /.box --> 
                                                                  
                                     <!-- Message -->

<%--                                              <div class="box box-success">
                                                <div class="box-header">
                                                  <i class="fa fa-comments-o"></i>
                                                  <h3 class="box-title">Messages</h3>
                                                  <div class="box-tools pull-right" data-toggle="tooltip" title="Status">
                                                    <div class="btn-group" data-toggle="btn-toggle" >
                                                      <button type="button" class="btn btn-default btn-sm active"><i class="fa fa-square text-green"></i></button>
                                                      <button type="button" class="btn btn-default btn-sm"><i class="fa fa-square text-red"></i></button>
                                                    </div>
                                                  </div>
                                                </div>

                                                <div class="box-body chat" id="chat-box">
                                                  <!-- chat item -->
                                                  <div class="item">
                                                    <img src="Imagenes/Logo_User.png" alt="user image" class="online">
                                                    <p class="message" style="text-align:justify">
                                                      <a href="#" class="name" >
                                                        <small class="text-muted pull-right"><i class="fa fa-clock-o"></i> 14:15</small>
                                                        Carlos Arciniegas - CELIN-16-G-030
                                                      </a>
                                                        Se adjunta el convenio descrito anteriormente para su revisión y aprobación. Esta actividad fue presentada en el comité de aprobación de USAID CELINS-16-Comite-005, desarrollado el 24 de febrero de 2016. La aprobación ambiental se encuentra en trámite, se verificará que se cuente con esta aprobación antes del envío a firma del convenio.
                                                    </p>
                                                    <div class="attachment">
                                                      <h4>Attachments:</h4>
                                                      <p class="filename">
                                                        Theme-thumbnail-image.jpg
                                                      </p>
                                                      <div class="pull-right">
                                                        <button class="btn btn-primary btn-sm btn-flat">Open</button>
                                                      </div>
                                                    </div><!-- /.attachment -->
                                                  </div><!-- /.item -->
                                                  <!-- chat item -->
                                                  <div class="item">
                                                    <img src="Imagenes/Logo_User.png" alt="user image" class="offline">
                                                    <p class="message"  style="text-align:justify">
                                                      <a href="#" class="name">
                                                        <small class="text-muted pull-right"><i class="fa fa-clock-o"></i> 5:15</small>
                                                        MIGUEL ANGEL ATUESTA - CELIS-15-G-008-MOD-001
                                                      </a>
                                                      Good morning Carlos, et al: Please consider this message and the attached signed format as the official notice of COR concurrence and CO Approval of your request to modify subgrant CELIS-15-G-008 to increase the Total Estimated Amount in Colombian Pesos to COP1,120,227,956.00, in the conditions described in this request and in any attachments included. Please be advised that this approval is not an authorization to exceed the Total Estimated Cost or the Obligated Amount of the prime contract, or a modification of any of the Terms & Conditions of the award. For any additional questions, please contact this office directly.
                                                    </p>
                                                  </div><!-- /.item -->
                                                  <!-- chat item -->
                                                  <div class="item">
                                                    <img src="Imagenes/Logo_User.png" alt="user image" class="offline">
                                                    <p class="message"  style="text-align:justify" >
                                                      <a href="#" class="name">
                                                        <small class="text-muted pull-right"><i class="fa fa-clock-o"></i> 5:30</small>
                                                        MARIA PAULA VARGAS - FO-003-2016-0003
                                                      </a>
                                                      Chemonics would like to request approval to donate 3 computers and 1 printer to the Municipality of Tumaco as per the attached request. Thanks
                                                    </p>
                                                  </div><!-- /.item -->


                                                </div><!-- /.chat -->
                                            

                                                <div class="box-footer">
                                                  <div class="input-group">                                                   
                                                    <div class="input-group-btn">
                                                       <a href="javascript::;" class="btn btn-sm btn-default btn-flat pull-right">View All Messages</a>
                                                     </div>
                                                  </div>
                                                </div>
                                              </div><!-- /.box (chat box) -->
                               --%>


                                      <!-- Calendar -->
                                              <div class="box box-solid bg-green-gradient">
                                                <div class="box-header">
                                                  <i class="fa fa-calendar"></i>
                                                  <h3 class="box-title"><asp:Label runat="server" ID="lblt_Calendar">Calendar</asp:Label></h3>
                                                  <!-- tools box -->
                                                  <div class="pull-right box-tools">
                                                    <!-- button with a dropdown -->
                                                    <div class="btn-group">
                                                      <button class="btn btn-success btn-sm dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bars"></i></button>
                                                      <ul class="dropdown-menu pull-right" role="menu">
                                                        <li><a href="#">Add new event</a></li>
                                                        <li><a href="#">Clear events</a></li>
                                                        <li class="divider"></li>
                                                        <li><a href="#">View calendar</a></li>
                                                      </ul>
                                                    </div>
                                                    <button class="btn btn-success btn-sm" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                    <button class="btn btn-success btn-sm" data-widget="remove"><i class="fa fa-times"></i></button>
                                                  </div><!-- /. tools -->
                                                </div><!-- /.box-header -->
                                                <div class="box-body no-padding">
                                                  <!--The calendar -->
                                                  <div id="calendar" style="width: 100%"></div>
                                                </div><!-- /.box-body -->
                                                <div class="box-footer text-black">
                                                  <div class="row">
                                                    <div id="app_task" runat="server" class="col-sm-7" visible="false">
                                                      <!-- Progress bars -->
                                                      <div class="clearfix">
                                                        <span class="pull-left alert alert-info"><asp:Label runat="server" ID="lblt_not_approvals_pending_to_completed">There is not approvals pending to completed</asp:Label></span>                                                       
                                                      </div>
                                                      <div class="progress xs">
                                                        <div class="progress-bar progress-bar-green" style="width: 0%;"></div>
                                                      </div>
                                                    </div><!-- /.col -->
                                                  </div><!-- /.row -->                                                  
                                                  <div class="row" id="task_detail" runat="server">                                                

                                                  </div><!-- /.row -->                                                   
                                                      
                                                   
                                                    <!-- Testing -->
                                                                                                        
                                                    <!--Testing --> 
                                                    
                                                                                              
		                                        </div>
                                           </div><!-- /.box -->
                               
                          
                           </section><!-- /.Left col -->

                        <!-- right col (We are only adding the ID to make the widgets sortable)-->
                        <section class="col-lg-5 connectedSortable">

                                 <div class="box box-primary">
                                     <div class="box-header">
                                         <br />
                                      </div>
                                          <div class="box-body no-padding" runat="server" id="containerChart">
                                             <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                 <ContentTemplate>
                                                    <div>
                                                      <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
                                                       <%--  <div id="container" style=""></div>--%>
                                                     </div><br />
                                                    <div >
                                                       <asp:Literal ID="ltrChart2" runat="server"></asp:Literal>
                                                     <%--  <div id="container2" style=""></div>--%>
                                                     </div>
                                                    </ContentTemplate>
                                                  </asp:UpdatePanel>                                               
                                            </div>
                                      <div class="box-footer clearfix no-border">
                                       </div>
                                     </div>


                              <%--       <!-- Calendar -->
                                              <div class="box box-solid bg-green-gradient">
                                                <div class="box-header">
                                                  <i class="fa fa-calendar"></i>
                                                  <h3 class="box-title">Calendar</h3>
                                                  <!-- tools box -->
                                                  <div class="pull-right box-tools">
                                                    <!-- button with a dropdown -->
                                                    <div class="btn-group">
                                                      <button class="btn btn-success btn-sm dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bars"></i></button>
                                                      <ul class="dropdown-menu pull-right" role="menu">
                                                        <li><a href="#">Add new event</a></li>
                                                        <li><a href="#">Clear events</a></li>
                                                        <li class="divider"></li>
                                                        <li><a href="#">View calendar</a></li>
                                                      </ul>
                                                    </div>
                                                    <button class="btn btn-success btn-sm" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                    <button class="btn btn-success btn-sm" data-widget="remove"><i class="fa fa-times"></i></button>
                                                  </div><!-- /. tools -->
                                                </div><!-- /.box-header -->
                                                <div class="box-body no-padding">
                                                  <!--The calendar -->
                                                  <div id="calendar" style="width: 100%"></div>
                                                </div><!-- /.box-body -->
                                                <div class="box-footer text-black">
                                                  <div class="row">
                                                    <div class="col-sm-6">
                                                      <!-- Progress bars -->
                                                      <div class="clearfix">
                                                        <span class="pull-left">CELIN-16-G-003</span>
                                                        <small class="pull-right">90%</small>
                                                      </div>
                                                      <div class="progress xs">
                                                        <div class="progress-bar progress-bar-green" style="width: 90%;"></div>
                                                      </div>

                                                      <div class="clearfix">
                                                        <span class="pull-left">CELIN-16-G-007</span>
                                                        <small class="pull-right">70%</small>
                                                      </div>
                                                      <div class="progress xs">
                                                        <div class="progress-bar progress-bar-green" style="width: 70%;"></div>
                                                      </div>
                                                    </div><!-- /.col -->
                                                    <div class="col-sm-6">
                                                      <div class="clearfix">
                                                        <span class="pull-left">CELIN-16-G-001</span>
                                                        <small class="pull-right">60%</small>
                                                      </div>
                                                      <div class="progress xs">
                                                        <div class="progress-bar progress-bar-green" style="width: 60%;"></div>
                                                      </div>

                                                      <div class="clearfix">
                                                        <span class="pull-left">CELIN-16-G-004</span>
                                                        <small class="pull-right">40%</small>
                                                      </div>
                                                      <div class="progress xs">
                                                        <div class="progress-bar progress-bar-green" style="width: 40%;"></div>
                                                      </div>
                                                    </div><!-- /.col -->
                                                  </div><!-- /.row -->
                                                </div>
                                              </div><!-- /.box -->--%>

                                           


                        </section>


                 </div>

                 <div class="row">

                     <div class="col-md-12">

                          <!-- TABLE: LATEST APPROVALS -->
                                          <div class="box box-info">
                                            <div class="box-header with-border">
                                              <h3 class="box-title"><asp:Label runat="server" ID="lblt_Latest_Approvals">Latest Approvals</asp:Label></h3>
                                              <div class="box-tools pull-right">
                                                <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                              </div>
                                            </div><!-- /.box-header -->
                                            <div class="box-body">
                                              <div class="table-responsive">
                                                <table class="table no-margin">
                                                  <thead>
                                                    <tr>                                  
                                                      <th style="width:1%;"></th>                    
                                                      <th style="width:15%;"><asp:Label runat="server" ID="lblt_col_Category">Category</asp:Label></th>
                                                      <th style="width:20%;"><asp:Label runat="server" ID="lblt_col_Approval">Approval</asp:Label></th>
                                                      <th style="width:15%;"><asp:Label runat="server" ID="lblt_col_Instrument">Tools</asp:Label></th>
                                                      <th style="width:39%;"><asp:Label runat="server" ID="lblt_col_Process">Process</asp:Label></th>
                                                      <th style="width:15%;"><asp:Label runat="server" ID="lblt_col_Status">Status</asp:Label></th>
                                                    </tr>
                                                  </thead>
                                                  <tbody>
                                                    <asp:Repeater ID="rep_lastest" runat="server">
                                                          <ItemTemplate>
                                                                <tr>
                                                                   <td><div class="tools"><a href="<%=urlSys & "/approvals/frm_seguimientoAprobacionRep.aspx?IdDoc="%><%# Eval("id_documento")%>&IdRuta=<%# Eval("id_ruta")%>"   target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                   <td><%# Eval("category")%></td>
                                                                   <td><%# Eval("descripcion_aprobacion")%></td>
                                                                   <td><a href="#"><%# Eval("numero_instrumento")%></a></td>
                                                                   <td><%# Eval("approval")%></td>
                                                                   <td> 
                                                                      <h5 style="font-weight:600;" ><%# Eval("descripcion_estado")%></h5> 
                                                                      <span class="label <%# Eval("ico")%>"><%# Eval("value")%>&nbsp;<%# Eval("unit")%></span>
                                                                   </td>
                                                                </tr>  
                                                          </ItemTemplate>
                                                      </asp:Repeater>                                                                            
                                                  </tbody>
                                                </table>
                                              </div><!-- /.table-responsive -->
                                            </div><!-- /.box-body -->
                                            <div class="box-footer clearfix">
                                              <a href="<%=urlSys & "/approvals/frm_docsAD.aspx" %>"   class="btn btn-sm btn-info btn-flat pull-left"><asp:Label runat="server" ID="lblt_New_Approval_Process2">New Approval Process</asp:Label></a>
                                              <a href="<%=urlSys & "/approvals/frm_consulta_docs.aspx" %>"  class="btn btn-sm btn-default btn-flat pull-right"><i class="fa fa-check"></i><asp:Label runat="server" ID="lblt_Search2">Search</asp:Label></a>&nbsp;&nbsp;&nbsp;
                                            </div><!-- /.box-footer -->
                                    </div><!-- /.box -->
                         
                     </div>
                       
                </div>

         <asp:Label ID="lbl_RolID" runat="server" Visible="false" ></asp:Label> 
         <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label> 
         <asp:Label ID="lbl_ALL_RolID" runat="server" Visible="false" ></asp:Label>                                            
         <asp:Label ID="lbl_ALL_SIMPLE_RolID" runat="server" Visible="false" ></asp:Label> 
                

                <div class="col-lg-12">
                    <%--<div id="container" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto"></div>--%>
                </div>

                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>
               
                <!-- daterangepicker -->
                <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.10.2/moment.min.js"></script>
                <script src="plugins/daterangepicker/daterangepicker.js"></script>
                <!-- datepicker -->
                <script src="plugins/datepicker/bootstrap-datepicker.js"></script>

               
                <script>
                            
                    //$(".todo-list").sortable({
                    //    placeholder: "sort-highlight",
                    //    handle: ".handle",
                    //    forcePlaceholderSize: true,
                    //    zIndex: 999999
                    //});

                    ///* The todo list plugin */
                    //$(".todo-list").todolist({
                    //    onCheck: function (ele) {
                    //        window.console.log("The element has been checked");
                    //        return ele;
                    //    },
                    //    onUncheck: function (ele) {
                    //        window.console.log("The element has been unchecked");
                    //        return ele;
                    //    }
                    //});



                    //var SelectedDates = {};
                    //SelectedDates[new Date('05/01/2016')] = new Date('05/01/2016');
                    //SelectedDates[new Date('05/17/2016')] = new Date('05/17/2016');
                    //SelectedDates[new Date('05/30/2016')] = new Date('05/30/2016');

                    //var SeletedText = {};
                    //SeletedText[new Date('05/01/2016')] = 'The approval request (TR-016-34) has been received ';
                    //SeletedText[new Date('05/17/2016')] = 'The approval request (TR-016-51) has been received ';
                    //SeletedText[new Date('05/30/2016')] = 'The approval request (TR-016-84) has been received ';


                    // //The Calender
                    //$("#calendar").datepicker({
                    //    multidate: true,
                    //    todayHighlight: true,
                    //    minDate: 0,
                    //    beforeShowDay: function (date) {

                    //        var Highlight = SelectedDates[date];
                    //        var HighlighText = SeletedText[date];

                    //      //  alert(date);

                    //        if (Highlight) {
                    //            return { enabled: true, classes: 'alert alert-warning', tooltip: HighlighText };
                    //        }
                    //        else {
                    //            return { enabled: true, classes: '', tooltip: date.toString().slice(0,15) };
                    //        }                                                
                    //    }
                    //});

                   // $('#calendar').datepicker('setDates', [new Date(2016, 5, 17), new Date(2016, 5, 30), new Date(2016, 5, 31)])

                    //$(function () {
                    //    $('#container').highcharts({
                    //        chart: {
                    //            plotBackgroundColor: null,
                    //            plotBorderWidth: null,
                    //            plotShadow: false,
                    //            type: 'pie'
                    //        },
                    //        title: {
                    //            text: 'Approval Processes'
                    //        },
                    //        tooltip: {
                    //            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    //        },
                    //        plotOptions: {
                    //            pie: {
                    //                allowPointSelect: true,
                    //                cursor: 'pointer',
                    //                dataLabels: {
                    //                    enabled: false
                    //                    //format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                    //                    //style: {
                    //                    //    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    //                    //}
                    //                },
                    //                showInLegend: true
                    //            }
                    //        },
                    //        series: [{
                    //            name: 'Type',
                    //            colorByPoint: true,
                    //            data: [{
                    //                name: 'Activity Fund related approvals',
                    //                y: 56.33
                    //            }, {
                    //                name: 'Travel related approvals',
                    //                y: 24.03,
                    //                sliced: true,
                    //                selected: true
                    //            }, {
                    //                name: 'Labor related approvals',
                    //                y: 10.38
                    //            }, {
                    //                name: 'Procurement',
                    //                y: 4.77
                    //            }, {
                    //                name: 'Deliverables',
                    //                y: 0.93
                                
                    //            }]
                    //        }]
                    //    });
                    //});

                    //$(function () {
                    //    $('#container2').highcharts({
                    //        title: {
                    //            text: 'Time Average of Response',
                    //            x: -20 //center
                    //        },
                    //        subtitle: {
                    //            text: 'Approval System'
                    //        },
                    //        xAxis: {
                    //            categories: ['Sep', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', ]
                    //        },
                    //        yAxis: {
                    //            title: {
                    //                text: 'Hours'
                    //            },
                    //            plotLines: [{
                    //                value: 0,
                    //                width: 1,
                    //                color: '#808080'
                    //            }]
                    //        },
                    //        legend: {
                    //            layout: 'horizontal',
                    //            align: 'bottom',
                    //           // horizontalAlign: 'left',
                    //            borderWidth: 0
                    //        },
                    //        series: [{
                    //            name: 'Activity Fund related approvals',
                    //            data: [96, 128, 365, 258, 178,245]
                    //        }, {
                    //            name: ' Labor related approvals',
                    //            data: [15, 0, 37, 0, 78, 48]
                    //        }, {
                    //            name: 'Travel related approvals',
                    //            data: [20, 25, 31, 20, 45, 35]
                    //        }, {
                    //            name: 'Procurement',
                    //            data: [0, 128, 78, 24, 45, 32]
                    //        }, {
                    //            name: 'Deliverables',
                    //            data: [120,125, 131, 120, 145, 135]
                    //        }]
                    //    });
                    //});


                </script>

              
            </div>
            <!-- /.box-body -->
            <%--<div class="box-footer">
                Footer
           
            </div>--%>
            <!-- /.box-footer-->
        </div>
        <!-- /.box -->

    </section>
    <!-- /.content -->

    <!-- Control Sidebar -->
    <!-- /.control-sidebar -->
    <!-- Add the sidebar's background. This div must be placed
           immediately after the control sidebar -->
    <div class="control-sidebar-bg"></div>
</asp:Content>
