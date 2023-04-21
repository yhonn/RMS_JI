<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetFollowing"  Codebehind="frm_TimeSheetFollowing.aspx.vb" %>
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
            <section class="content">
                <div class="box"> 
                     <div class="box-header with-border">
                          <div class="col-sm-6">  
                            <h3 class="box-title">
                                <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Following Up</asp:Label>
                            </h3>                            
                             <asp:HiddenField ID="hd_IDtimeSheet" runat="server" Value="0" />
                             <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />                        
                          </div>
                         <div class="col-sm-6 text-right">   
                         <%--  <asp:Button ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('TimeSheet_04.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:Button>   --%>
                        </div>
                    </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">

                                   <div class="col-lg-12">
                                     <div class="box-body">
                                                              
                                            <div class="stepwizard">
                                                    <div class="stepwizard-row setup-panel">

                                                        <div class="stepwizard-step" style="width:25%">
                                                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_informaciongeneral">Time Sheet Information</asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="stepwizard-step" style="width:25%">
                                                            <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_personal_status">Billable Time</asp:Label>
                                                            </p>
                                                        </div>
                                                         <div class="stepwizard-step" style="width:25%">
                                                             <a   href="#step-3" id="anchorSupportDocs" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                                             <p>
                                                                 <asp:Label runat="server" ID="lblt_Support_Documents">TimeSheet Documents</asp:Label>
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
                                         
                                         </div>
                                    </div>  
                                 
                                     <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>  
                                 
                                     <div class="col-lg-12">
                                       <div class="box-body">                                 
                                            <div class="col-md-4 col-sm-8 col-xs-16">
                                              <div class="info-box">
                                                <span class="info-box-icon bg-gray"><i class="fa fa-user"></i></span>
                                                <div class="info-box-content">
                                                     <span class="info-box-text"><%=userName %></span>
                                                     <span class="text-bold"><%=userJOB_tittle %></span>     
                                                    <asp:HiddenField runat="server" ID="hd_IDuser" Value="0" />       
                                                </div><!-- /.info-box-content -->
                                              </div><!-- /.info-box -->
                                            </div><!-- /.col -->
                                             <div class="col-md-4 col-sm-8 col-xs-16">
                                              <div class="info-box">
                                                 <span class="info-box-icon  bg-gray"><i class="fa fa-calendar"></i></span>
                                                <div class="info-box-content">
                                                     <span class="info-box-text"><%=Month_TS%></span>
                                                     <asp:HiddenField ID="hd_month" runat="server" Value="<%=Month_TS%>" />
                                                     <span class="text-bold"><%=Year_TS%></span>             
                                                     <asp:HiddenField ID="hd_year" runat="server" Value="<%=Year_TS%>" />
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

                            <div class="col-lg-12">
                               <div class="box-body">

                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_Employee_type" runat="server" CssClass="control-label text-bold" Text="Employee type"></asp:Label>
                                            <asp:Label ID="lbl_ALL_SIMPLE_RolID" runat="server" Visible="false" ></asp:Label> 
                                        </div>
                                       <div class="col-sm-3">
                                           <!--Control -->
                                           <asp:Label ID="lbl_employeeType" runat="server" CssClass="control-label info-box-text" Text=""></asp:Label> 
                                           <asp:HiddenField ID="hd_id_employee_type" runat="server" Value="0" />
                                       </div> 
                                        <br /><br />  

                                    </div>
                                         
                                    <div class="form-group row">
                                         <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                           <!--Control -->
                                           <p runat="server" id="PDescription" style="width:90%; border: 1px solid lightgray; height:50px;  padding: 3px 3px 3px 3px;  "></p>
                                           <%--<telerik:RadTextBox ID="txt_description" Runat="server"  EmptyMessage="Type Description here.." Width="90%"  ValidationGroup="1" TextMode="MultiLine" Columns="40" Rows="3">
                                           </telerik:RadTextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_description" ValidationGroup="1"></asp:RequiredFieldValidator>--%>
                                          
                                       </div>  
                                                                            
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>
                                                                      
                                    <div class="form-group row">
                                  

                                    </div>
                                                                      
                                     <div class="form-group row">                                                                                                                                                      

                                        <div class="col-sm-12 text-left">
                                               
                                                 <style type="text/css" >

                                                    ErrCol {   cursor: not-allowed;
                                                                display: table-row;
                                                                vertical-align: inherit;
                                                                border-color: inherit; 
                                                                background-color:#dd4b39 !important; 
                                                    }

                                                     /*.table-bordered-ts {

                                                         border:2px solid lightgray !important;
                                                         padding:5px 5px 5px 5px !important;
                                                         
                                                     }

                                                     .padding-required {
                                                         padding:5px 5px 5px 5px!important;
                                                     }

                                                     .table-width-auto {                                                          
                                                          width: 35%  !important;                                                          
                                                     }

                                                     .table tbody >tr .bordered-row.bordered-row >td {
                                                        border-bottom: 1px solid red !important;
                                                      }
                                                     

                                                     .table-with-val{
                                                          width:115%!important;
                                                     }*/

                                                     
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
                                                             width:140%!important;
                                                         }
                                                     }
                                                     
                                                                                                
                                           </style>                                                                                                                                                                          
                                            
                                                <!---The table whit the TimeSheet Here//-->
                                                <div style="max-width:100%; overflow-y:auto;">                                                                                      
                                                      <%= strTableResult %>
                                               </div>

                                           </div>
                                      </div>


                                    <div class="form-group row">
                                       <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                       </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <telerik:RadTextBox ID="txt_notes" Runat="server" EmptyMessage="Type notes here.."  Width="90%" ValidationGroup="1" Height="150" TextMode="MultiLine">
                                            </telerik:RadTextBox>    
                                             <%--<p runat="server" id="Pnotes" style="width:90%; border: 1px solid lightgray; height:50px;  padding: 3px 3px 3px 3px;  "></p>--%>
                                                                                
                                       </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <div class="box" style="border-top-color:lightgray">
                                                    <div class="box-header">
                                                      <h3 class="box-title">Resumen</h3>
                                                    </div><!-- /.box-header -->
                                                    <div class="box-body no-padding">
                                                      <table class="table table-striped">
                                                        <tr>
                                                          <th style="width: 10px">#</th>
                                                          <th>Tiempo reportado</th>
                                                          <th>Días registrados</th>
                                                          <th>Total de horas</th>
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
                                                           <th style="border-top:1px solid lightgray;"><%=TOThrs %></th>             
                                                           <th></th>        
                                                        </tr>
                                                      </table>
                                                    </div><!-- /.box-body -->
                                                  </div><!-- /.box -->
                                        </div>
                                    </div>


                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>


                                       <div class="form-group row">
                                               <div class="col-sm-12 text-left">

                                                      <!-- TABLE:  DELIVERABLE DOCUMENTS--> 
                                                   
                                                            <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">Documentos asociados (<asp:Label ID="lbl_approval_route" runat="server" >--Route--</asp:Label>)</h3>
                                                                  <asp:HiddenField ID="hd_id_documento_timesheet" runat="server" Value="0" />
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding" style="padding-left:10px;" >     

                                                                          <div class=" row">                                                                                                
                                                                               <div class="col-sm-12">                                                                                              
                                                                                                   
                                                                                           <!--Control -->
                                                                                             <br />
                                                                                             <div class="small-box-XR box-warning">
                                                                                                    <div class="box-header">
                                                                                                      <h3 class="box-title">Soportes</h3>                                                                                                                     
                                                                                                    </div><!-- /.box-header -->
                                                                                                    <div class="box-body table-responsive no-padding">
                                                                                                              <table class="table table-hover">
                                                                                                                <tr>
                                                                                                                  <th>#</th>
                                                                                                                  <th>Tipo de documento</th>
                                                                                                                  <th></th>
                                                                                                                  <th>Nombre del archivo</th>                                                                                                                          
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


                                       <div class="form-group row">
                                            <div class="col-sm-12 text-left">
                                                 <hr class="hr-primary" />                                             
                                            </div>
                                        </div>


                                    <div class="form-group row" runat="server" id="lyHistory" visible="false">
                                        <div class="col-sm-12 text-left">

                                                   <%--TAble here--%>
                                                   <table class="table table-responsive table-condensed box box-primary ">
                                                          <tr class="box box-default ">
                                                                <td  class="text-left"  colspan="2">
                                                                  <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                                                    <div class="box-header">
                                                                       <i class="fa fa-history"></i>
                                                                       <h3 class="box-title">Historial</h3>                                              
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


                                  </div>
                               </div> 
              
                          </div>
                        
                        </div>

                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->

                        <div class="form-group row" runat="server" id="lyButtoms" visible="true">
                             <div class="col-sm-2 text-left">
                               <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm pull-left" Width="100"></telerik:RadButton><br /><br />                      
                             </div>
                            <div class="col-sm-2 text-left">
                              <asp:LinkButton ID="btnlk_save" runat="server" AutoPostBack="True"  Text="Save and Continue" Width="99%" class="btn btn-primary btn-sm margin-r-5"  ValidationGroup="1" >Start Approval &nbsp;&nbsp;&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                              <%--<telerik:RadButton ID="btn_Save" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Start Approval" class="btn btn-primary btn-sm margin-r-5" ValidationGroup="1" ></telerik:RadButton><br /><br />--%>
                            </div>
                            <div class="col-sm-8 text-left">
                                <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label> 
                                  <telerik:RadComboBox  ID="cmb_approvals" 
                                                        runat ="server" 
                                                        CausesValidation="False"                                                                                                                             
                                                        AllowCustomText="true" 
                                                        Filter="Contains"                                         
                                                        Width="85%" Enabled ="false"  > 
                                   </telerik:RadComboBox><br />
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="The approval flow is mandatory to continue. please select the approval in the step #3 (Documents). If you don't have approval flow assigned, please contact the system administrator." ForeColor="Red" ControlToValidate="cmb_approvals" ValidationGroup="1"></asp:RequiredFieldValidator>
                                
                            </div>
                       </div>                   
                   </div>

                </div>

           </section>
    
    </asp:Content>

