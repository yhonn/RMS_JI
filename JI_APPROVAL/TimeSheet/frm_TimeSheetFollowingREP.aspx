<%@ Page Language="VB" MasterPageFile="~/MasterPopUp.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetFollowingREP"  Codebehind="frm_TimeSheetFollowingREP.aspx.vb" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

        <uc:Confirm runat="server" ID="MsgGuardar" />
        <uc:ReturnConfirm runat="server" ID="MsgReturn" />
        <link rel="stylesheet" href="../Content/hr_Styles.css" />
        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEET</asp:Label>
                </h1>
            </section>
            <section class="content content_ts">
                <div class="box">                  
                
                        <div class="box-body row">                                                           
                                 
                                     <div class="form-group row" style="display:none;">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                            <asp:HiddenField ID="hd_IDtimeSheet" runat="server" Value="0" />
                                             <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />

                                        </div>
                                    </div>  
                                  <div class="col-lg-12 row">
                                       <div class="box-body"> 
                                           <div class="col-md-2 col-sm-4 col-xs-4 text-bold employee">Employee Name/Nombre:</div>
                                           <div class="col-md-3 col-sm-4 col-xs-4 text-bold employee" style="text-decoration:underline;"><%=userName %></div>
                                           <div class="col-md-1 col-sm-4 col-xs-4 text-bold employee">Position/Cargo:</div>
                                           <div class="col-md-3 col-sm-4 col-xs-4 text-bold employee" style="text-decoration:underline;"><%=userJOB_tittle %></div>
                                           <div class="col-md-1 col-sm-4 col-xs-4 text-bold employee">Month/Mes:</div>
                                           <div class="col-md-2 col-sm-4 col-xs-4 text-bold employee" style="text-decoration:underline;"><%=Month_TS%> - <%=Year_TS%></div>
                                       </div>
                                  </div>
                                 
                            <div class="col-lg-12 row">
                               <div class="box-body">

                                   
                                    <div class="form-group row hr_ts">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>
                                     <div class="form-group row">                                                                                                                                                        

                                        <div class="col-sm-12 text-left">
                                               
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

                                                          .bg-primary{
                                                               color: #000;
                                                           }
                                                          .employee{
                                                              font-size: 10px;
                                                          }
                                                          .hr_ts{
                                                              display:none;
                                                          }
                                                          .content_ts{
                                                              padding: 0px;
                                                          }
                                                          .progress-xs{
                                                              display:none;
                                                          }
                                                          .total_hours_ts{
                                                              font-size:12px;
                                                          }
                                                          .total_hours_ts tbody tr td{
                                                              font-size:12px;
                                                              padding:0px;
                                                          }
                                                          .box{
                                                              border-top: 0px;
                                                              margin-bottom: 0px
                                                          }
                                                          .content-header{
                                                              display:none;
                                                          }
                                                          .page-header{
                                                              margin-bottom: 0px;
                                                          }
                                                          .box-body{
                                                              padding: 3px 10px;
                                                          }
                                                        }
                                                     
                                                       .bg-primary{
                                                           font-weight: bold;
                                                       }
                                                                                                
                                           </style>                                                                                                                                                                      
                                            
                                                <!---The table whit the TimeSheet Here//-->
                                                <div style="max-width:115%; overflow-y:auto;" >                                                                                      
                                                      <%= strTableResult %>
                                               </div>

                                           </div>
                                      </div>

                                   <div class="form-group row">
                                         <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                           <!--Control -->
                                           <p runat="server" id="PDescription" style=" max-width:100%; width:auto; font-weight:bold;"></p>
                                           <%--<telerik:RadTextBox ID="txt_description" Runat="server"  EmptyMessage="Type Description here.." Width="90%"  ValidationGroup="1" TextMode="MultiLine" Columns="40" Rows="3">
                                           </telerik:RadTextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_description" ValidationGroup="1"></asp:RequiredFieldValidator>--%>
                                          
                                       </div>  
                                                                            
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <div class="box" style="border-top-color:lightgray">
                                                    <div class="box-header">
                                                      <h3 class="box-title">Resumen</h3>
                                                    </div><!-- /.box-header -->
                                                    <div class="box-body no-padding">
                                                      <table class="table table-striped total_hours_ts">
                                                        <tr>
                                                          <th style="width: 10px">#</th>
                                                          <th>Billable Time</th>
                                                          <th>Days Registered</th>
                                                          <th>Total Hours</th>
                                                          <th>LOE</th>             
                                                          <th></th>                                             
                                                        </tr>
                                                           <asp:Repeater ID="reptTable" runat="server">
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
                                                           <th style="border-top:1px solid lightgray;"><%=TOThrs %></th>
                                                           <th style="border-top:1px solid lightgray;"><%=TOTloe %></th>             
                                                           <th></th>        
                                                        </tr>
                                                      </table>
                                                    </div><!-- /.box-body -->
                                                  </div><!-- /.box -->
                                        </div>
                                    </div>
                                   <div class="form-group row" runat="server" visible="false" id="documentos">
                                       <div class="col-sm-12">                                                                                              
                                                                                                   
                                                    <!--Control -->
                                                        <br />
                                                        <div class="small-box-XR box-warning">
                                                            <div class="box-header">
                                                                <h3 class="box-title">Support Documents</h3>                                                                                                                     
                                                            </div><!-- /.box-header -->
                                                            <div class="box-body table-responsive no-padding">
                                                                        <table class="table table-hover">
                                                                        <tr>
                                                                            <th>#</th>
                                                                            <th>Document Type</th>
                                                                            <th></th>
                                                                            <th>File Name</th>                                                                                                                          
                                                                            <th>Rev</th>
                                                                        </tr>
                                                                            <asp:Repeater ID="rpt_support_docs" runat="server" >
                                                                                <ItemTemplate> 
                                                                                    <tr>
                                                                                        <td> <%# Eval("no") %></td>
                                                                                        <td> <%# Eval("nombre_documento") %></td>
                                                                                        <td><a class="btn btn-lg btn-outline-secondary" target="_blank"  href="..\FileUploads\ApprovalProcc\<%# Eval("archivo") %>" role="button"><i class="<%# Eval("ext_icon") %>"></i></a></td>
                                                                                        <td><a class="btn btn-sm btn-outline-secondary" target="_blank"  href="..\FileUploads\ApprovalProcc\<%# Eval("archivo") %>" role="button"><%# Eval("archivo") %></a></td>                                                                                                                                      
                                                                                        <td style="text-align:center"> <%# Eval("ver") %></td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>                                                                                                                              
                                                                        </table>
                                                            </div><!-- /.box-body -->
                                                            </div><!-- /.box -->                                                                                                                                                                                                                   
                                                                                               
                                                                                                               
                                        </div>
                                   </div>
                                   <div class="form-group row">
                                       <div class="col-sm-6">
                                           <div class="form-group row">
                                                <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4 text-bold">Información del empleado:</div>
                                               </div>
                                               <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4"><span class="text-bold ">Employee Name/Nombre: </span> <span style="text-decoration: underline"><%=userName %></span> </div>
                                               </div>
                                               <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4"><span class="text-bold ">Position/Cargo: </span> <span style="text-decoration: underline"> <%=userJOB_tittle %></span> </div>
                                               </div>
                                           </div>
                                       </div>
                                        <div class="col-sm-6" runat="server" visible="false" id="aprobadorInfo">
                                           <div class="form-group row">
                                            <div class="col-sm-12">
                                                <div class="col-md-12 col-sm-4 col-xs-4 text-bold">Información del aprobador:</div>
                                            </div>
                                               <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4"><span class="text-bold ">Employee Name/Nombre: </span> <span style="text-decoration: underline"><%=aprobador %></span> </div>
                                               </div>
                                               <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4"><span class="text-bold ">Position/Cargo: </span> <span style="text-decoration: underline"> <%=cargo_aprobador %></span> </div>
                                               </div>
                                               <div class="col-sm-12">
                                                   <div class="col-md-12 col-sm-4 col-xs-4"><span class="text-bold ">Fecha de aprobación: </span> <span style="text-decoration: underline"> <%=fecha_aprobacion %></span> </div>
                                               </div>
                                           </div>
                                       </div>
                                   </div>

                                    <div class="form-group row" style="display:none;">
                                       <div class="col-sm-3">
                                           <!--Control -->
                                           <asp:Label ID="lbl_employeeType" runat="server" CssClass="control-label info-box-text" Text=""></asp:Label> 
                                           <asp:HiddenField ID="hd_id_employee_type" runat="server" Value="0" />
                                       </div> 
                                        <br /><br />  

                                    </div>
                                         
                                    


                                    <asp:HiddenField ID="hd_id_documento_timesheet" runat="server" Value="0" />

                                  </div>
                               </div> 
              
                          </div>

                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer text-center">                            
                        <!--Controls -->
                       ***Esta autorización de hoja de tiempo no requiere firma autógrafa ya que fue elaborada y aprobada a través del sistema SIME***
                              
                   </div>
           </section>



    
    

    </asp:Content>

