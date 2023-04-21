<%@ Page Language="VB" MasterPageFile="~/MasterPopUp.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetFollowingREP_pay"  Codebehind="frm_TimeSheetFollowingREP_pay.aspx.vb" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

        <uc:Confirm runat="server" ID="MsgGuardar" />
        <uc:ReturnConfirm runat="server" ID="MsgReturn" />
        <link rel="stylesheet" href="../Content/hr_Styles.css" />
        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEETS REPORT</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">                  

                            <div class="box-body">                                                           
                                 
                                     <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                                                                
                                              <asp:HiddenField ID="hd_IDtimeSheet" runat="server" Value="0" />
                                              <asp:HiddenField ID="hd_month_var" runat="server" Value="0" />
                                              <asp:HiddenField ID="hd_month" runat="server" Value="<%=Month_TS%>" />

                                              <asp:HiddenField ID="hd_year_var" runat="server" Value="0" />
                                              <asp:HiddenField ID="hd_year" runat="server" Value="<%=Year_TS%>" />
                                              <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />
                                        </div>
                                    </div>  

                                           <style type="text/css" >

                                                     /* body {
                                                        /* default is 1rem or 16px * /
                                                        font-size: 10px !important;}*/
                                                     

                                                    ErrCol {   cursor: not-allowed;
                                                                display: table-row;
                                                                vertical-align: inherit;
                                                                border-color: inherit; 
                                                                background-color:#dd4b39 !important; 
                                                    }

                                                     .table-bordered-ts {

                                                         border:2px solid lightgray !important;
                                                         padding:1px 1px 1px 1px !important;
                                                         font-size: 12px !important;
                                                         
                                                     }

                                                     .padding-required {
                                                         padding:1px 1px 1px 1px!important;
                                                     }

                                                     .table-width-auto {                                                          
                                                          width: 35%  !important;                                                          
                                                     }

                                                     .table tbody >tr .bordered-row.bordered-row >td {
                                                        border-bottom: 1px solid red !important;
                                                      }
                                                     

                                                     .table-with-val{
                                                          width:100%!important;
                                                     }

                                                     @media screen and (max-width: 1024px) {
                                                         .table-with-val {
                                                             width:160%!important;
                                                         }
                                                     }

                                                     @media screen and (max-width: 1300px) {
                                                         .table-with-val {
                                                             width:150%!important;
                                                         }
                                                     }

                                                     /*@media print {
                                                          .rows-print-as-pages .row {
                                                            page-break-before: always;
                                                          }
                                                          /* include this style if you want the first row to be on the same page as whatever precedes it */
                                                          /*
                                                          .rows-print-as-pages .row:first-child {
                                                            page-break-before: avoid;
                                                          }
                                                          */
                                                       


                                                       @media print {

                                                            @page {
                                                                size: A4 landscape;
                                                              }

                                                          .rows-print-as-pages .row {
                                                            page-break-before: always;
                                                          }

                                                          .col-md-1,.col-md-2,.col-md-3,.col-md-4,
                                                          .col-md-5,.col-md-6,.col-md-7,.col-md-8, 
                                                          .col-md-9,.col-md-10,.col-md-11,.col-md-12 {
                                                            float: left;
                                                          }

                                                          .col-md-1 {
                                                            width: 8%;
                                                          }
                                                          .col-md-2 {
                                                            width: 16%;
                                                          }
                                                          .col-md-3 {
                                                            width: 25%;
                                                          }
                                                          .col-md-4 {
                                                            width: 33%;
                                                          }
                                                          .col-md-5 {
                                                            width: 42%;
                                                          }
                                                          .col-md-6 {
                                                            width: 50%;
                                                          }
                                                          .col-md-7 {
                                                            width: 58%;
                                                          }
                                                          .col-md-8 {
                                                            width: 66%;
                                                          }
                                                          .col-md-9 {
                                                            width: 75%;
                                                          }
                                                          .col-md-10 {
                                                            width: 83%;
                                                          }
                                                          .col-md-11 {
                                                            width: 92%;
                                                          }
                                                          .col-md-12 {
                                                            width: 100%;
                                                          }
                                                        }
                                                     
                                                                                                
                                           </style>     

                                                                    
                                          <asp:Repeater ID="rep_Report" runat="server" OnItemDataBound="rep_Report_ItemDataBound" >
                                               <ItemTemplate>
                                                   
                                                  <div class="col-md-12 div-bordered">
                                                     

                                                     <hr class="hr-primary" />   

                                                       <div class="col-md-12">
                                                           <div class="box-body">   
                                                                <div class="col-md-1">
                                                                  <div class="info-box">
                                                                    <span class="info-box-icon bg-gray"><%# Eval("NR")%></span>
                                                                    <div class="info-box-content">
                                                                         <span class="info-box-text"> &nbsp;&nbsp; </span>
                                                                         <span class="text-bold"> &nbsp; </span>      
                                                                         <br /><br />
                                                                        
                                                                    </div><!-- /.info-box-content -->
                                                                  </div><!-- /.info-box -->
                                                                </div><!-- /.col -->

                                                                <div class="col-md-4">
                                                                  <div class="info-box">
                                                                    <span class="info-box-icon bg-gray"><i class="fa fa-user"></i></span>
                                                                    <div class="info-box-content">
                                                                         <span class="info-box-text"> <%# Eval("nombre_usuario")%> </span>
                                                                         <span class="text-bold"> <%# Eval("job")%> </span>    
                                                                         <br /><br />
                                                                    </div><!-- /.info-box-content -->
                                                                  </div><!-- /.info-box -->
                                                                </div><!-- /.col -->
                                                                 <div class="col-md-3">
                                                                  <div class="info-box">
                                                                     <span class="info-box-icon  bg-gray"><i class="fa fa-calendar"></i></span>
                                                                    <div class="info-box-content">
                                                                         <span class="info-box-text"><span id="sp_mes" runat="server"></span> </span>
                                                                        
                                                                         <span class="text-bold"><%# Eval("anio")%></span>             
                                                                         <br /><br />
                                                                         
                                                                    </div><!-- /.info-box-content -->
                                                                  </div><!-- /.info-box -->
                                                                </div><!-- /.col -->
                                                               <div class="col-md-3">
                                                                  <div class="info-box">
                                                                      <span class="info-box-icon bg-gray"><i class="fa fa-flag-o"></i></span>
                                                                    <div class="info-box-content">
                                                                         <span class="info-box-text"><%# Eval("timesheet_estado")%></span>
                                                                         <span class="text-bold"> <span id="sp_date" runat="server"></span> <i class="fa fa-clock-o"></i><span id="sp_hour" runat="server"></span></span>             
                                                                    </div><!-- /.info-box-content -->
                                                                  </div><!-- /.info-box -->
                                                                </div><!-- /.col -->                                           
                                                            </div>
                                                        </div>

                                                       <div class="col-md-12">
                                                            <div class="box-body">
   
                                                                <div class="form-group row">
                                                                     <div class="col-sm-2 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                       <!--Control -->
                                                                       <p runat="server" id="PDescription" style=" max-width:100%; width:auto; border: 1px solid lightgray; height:50px;  padding: 3px 3px 3px 3px;  "></p>                                                                                                                
                                                                   </div>                                                                              
                                                                </div>

                                                                 <div class="form-group row">
                                                                    <div class="col-sm-12 text-left">                                                                                                                                   
                                            
                                                                            <!---The table whit the TimeSheet Here//-->
                                                                            <div runat="server" id="dv_TS" style="max-width:115%; overflow-y:auto;" >                                                                                      
                                                                                 
                                                                           </div>

                                                                       </div>
                                                                  </div>

                                                                <div class="form-group row">
                                                                   <div class="col-sm-2 text-left">
                                                                     <!--Tittle -->
                                                                       <br />
                                                                      <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                                                   </div>
                                                                   <div class="col-sm-8">                                                                     
                                                                        <br />
                                                                        <p runat="server" id="Pnotes" style=" max-width:100%; width:auto; border: 1px solid lightgray; height:auto; max-height:150px;  padding: 3px 3px 3px 3px;  "></p>                                                                                
                                                                   </div>
                                                                </div>
                                                                                                                               
                                                            </div>
                                                      </div>



                                                    <div class="form-group row">
                                                        <div class="col-sm-12 text-left">
                                                             <div class="box" style="border-top-color:lightgray">
                                                                    <div class="box-header">
                                                                      <h3 class="box-title">TimeSheet Summary</h3>
                                                                    </div><!-- /.box-header -->
                                                                    <div class="box-body no-padding">
                                                                      <table class="table table-striped">
                                                                        <tr>
                                                                          <th style="width: 10px">#</th>
                                                                          <th>Billable Time</th>
                                                                          <th>Days Registered</th>
                                                                          <th>Total Hours</th>
                                                                          <th>LOE</th>             
                                                                          <th></th>                                             
                                                                        </tr>
                                                                           <asp:Repeater ID="reptTable" runat="server" >
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                      <td><%# Eval("numberITEM")%></td>
                                                                                      <td>    
                                                                                        <%# Eval("billable_item")%>                                                          
                                                                                        <div class="progress progress-xs">
                                                                                          <div class="progress-bar <%# Eval("progress_bar")%>" style="width:<%# Eval("progress_value")%>%"></div>
                                                                                        </div>
                                                                                      </td>
                                                                                      <td><%# Eval("dias")%></td>
                                                                                      <td><%# Eval("TOThours")%></td>
                                                                                      <td><%# Eval("LOE")%></td>                                                         
                                                                                      <td><span class="badge <%# Eval("bg_color")%>"> <%# Eval("progress_value")%>%</span></td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                         </asp:Repeater>                                                     
                                                                        <tr style="border-bottom:1px solid lightgray;">
                                                                           <th></th>
                                                                           <th>Total</th>
                                                                           <th></th>
                                                                           <th style="border-top:1px solid lightgray;"><span id="sp_tot_hours" runat="server"></span></th>
                                                                           <th style="border-top:1px solid lightgray;"><span id="sp_tot_days" runat="server"></span></th>             
                                                                           <th></th>        
                                                                        </tr>
                                                                      </table>
                                                                    </div><!-- /.box-body -->
                                                                  </div><!-- /.box -->
                                                        </div>
                                                    </div>

                                                   

                                                    <div class="form-group row rows-print-as-pages" runat="server" id="lyHistory" visible="true">
                                                            <div class="col-sm-12 text-left">

                                                                       <%--TAble here--%>
                                                                       <table class="table table-responsive table-condensed box box-primary ">
                                                                              <tr class="box box-default ">
                                                                                    <td  class="text-left"  colspan="2">
                                                                                      <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                                                                        <div class="box-header">
                                                                                           <i class="fa fa-history"></i>
                                                                                           <h3 class="box-title">Approval Process</h3>                                              
                                                                                        </div>
                                        
                                                                                    </td>
                                                                                  </tr>
                                                                                <tr>
                                                                                    <td  colspan="2" class="text-left">
                                                                                        <br />                                    
                                                                                         <%-- <div class="direct-chat-messages">--%>

                                                                                                     <asp:Repeater ID="rept_msgApproval" runat="server">
                                                                                                            <ItemTemplate>
                                                                                                                  <div class="direct-chat-msg <%# Eval("align1")%> "  >
                                                                                                                      <div class="direct-chat-info clearfix">
                                                                                                                       <i class="fa <%# Eval("fa_icon")%> fa-2x <%# Eval("align2")%> " aria-hidden="true" title="<%# Eval("icon_message")%>"></i>&nbsp;&nbsp;&nbsp; <span class="direct-chat-name  <%# Eval("align2")%> "><%# Eval("empleado")%></span>
                                                                                                                        <span class="direct-chat-timestamp  <%# Eval("align3") %> "> <%# getFecha(Eval("fecha_comentario"), "m", False) %> <i class="fa fa-clock-o"></i> <%#  getHora(Eval("fecha_comentario")) %> </span>
                                                                                                                      </div><!-- /.direct-chat-info -->
                                                                                                                       <img class="direct-chat-img <%# Eval("bColor")%> " src="<%# Eval("userImagen")%>" alt="<%# Eval("empleado")%>"><!-- /.direct-chat-img -->
                                                                                                                      <div class="direct-chat-text">
                                                                                                                        <%# Eval("comentario")%>
                                                                                                                      </div><!-- /.direct-chat-text -->
                                                                                                                    </div><!-- /.direct-chat-msg -->                                                                 
                                                                                                            </ItemTemplate>
                                                                                                        </asp:Repeater>                 
                                                                                            <%--  </div><!--/.direct-chat-messages-->--%>                                      
                                          
                                                                                    </td>
                                                                                </tr>  
                                                                        </table>

                                                                </div>
                                                            </div>                                                       
                                                   
                                                      <br />

                                                  </div>          
                                                 
                                                   

                                             </ItemTemplate>
                                           </asp:Repeater>
                                                                                                      
                                                                              
                      

                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->

                              
                   </div>

                </div>
           </section>



    
    

    </asp:Content>

