<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Partner.Master" CodeBehind="frm_ActivityApplySCRE.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityApplySCRE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm2.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />

    <%-- <link rel="stylesheet" href="../plugins/jquery-smartwizard-master/css/smart_wizard_dots.min.css">--%>
     <link rel="stylesheet" href="../plugins/jquery-smartwizard-master/css/smart_wizard_arrows.min.css">

    <section class="content-header">
        <h4>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">WELCOME </asp:Label>
        </h4>
    </section>
    <section class="content">

             <style type="text/css">

                    /*.RadListBox span.rlbText 
                    { 
                        font-size: large !important; 
                        font-family: Verdana, Arial, Helvetica,sans-serif; 
                        color: darkblue; 
                        font-weight: bold;
                    }*/

                    .wrapWord { 
                                word-wrap: break-word;
                                word-break:break-all; 
                              }
                    
                </style>

      <div class="container-fluid">

         <div class="card card-orange card-outline">
             
            
            <div class="card-body p-0">

                  <div class="card-header">

                        <h5 class="card-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla"> Apply</asp:Label>
                            <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                            <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="identity_sol" text="0" runat="server"  CssClass="deleteIdentity" data-id="" Visible="false" />
                            <asp:Label ID="identity_doc" text="0" runat="server" CssClass="deleteIdentity" data-id="" Visible="false" />
                        </h5>
                      <div class="card-tools">   
                          <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm" data-toggle="Try" OnClick="showhelp();" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                      </div>
                       
                  </div>   
         

                  <div  id="dvTab" class="card border-0 p-0">
                       
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_sol" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_sol_app" runat="server" Text="" Visible="false" />


                    <div class="card-body p-0 border-0">

                             <asp:HiddenField ID="hfTab" runat="server" />  
                             <asp:HiddenField ID="Hiddenindi" runat="server" />
                                                

                         <div id="SolicitationALERT" runat="server"  class="form-group row" style="display:none;">
                              <div class="col-sm-7">
                                      <div class="alert alert-info alert-dismissable">
                                         <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                         <h4><i class="icon fa fa-info"></i>&nbsp;&nbsp;The Application has been successfully submitted</h4>
                                             We will get back to you soon, thank you
                                       </div>
                             </div>
                        </div>

                          <div id="SolicitationACCEPTED" runat="server"  class="form-group row" style="display:none;">
                              <div class="col-sm-7">
                                      <div class="alert alert-warning alert-dismissable">
                                         <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                         <h4><i class="fas fa-2x fa-exclamation-triangle"></i>&nbsp;&nbsp;The Application has been accepted and passed to the evaluation process</h4>
                                             We will get back to you soon, thank you
                                       </div>
                             </div>
                        </div>

                        
                       <div id="SolicitationRejected" runat="server"  class="form-group row" style="display:none;">
                          <div class="col-sm-7">
                                  <div class="alert alert-danger alert-dismissable">
                                     <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                     <h4><i class="icon fa fa-info"></i>&nbsp;&nbsp;The Application has been received</h4>
                                         The preliminary review did not meet the requirements mentioned for proceeding with the process. Thanks for submitting the proposal, and we will keep posting for any further project opportunities.
                                   </div>
                          </div>
                       </div>

                          <div id="SolicitationExpired" runat="server"  class="form-group row" style="display:none;">
                              <div class="col-sm-7">
                                      <div class="alert alert-warning alert-dismissable">
                                         <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                         <h4><i class="fas fa-hourglass-end"></i>&nbsp;&nbsp;The Solicitacion has been expired</h4>                                             
                                          Thanks for trying to look into the solicitation specs, we will keep posting for any further project opportunities.
                                       </div>
                             </div>
                        </div>


                        <div id="stepwizard" class="border-0"> 
                                       
                             <p id="_Documents">&nbsp;</p>

                              <ul class="nav">
                                  <li class="nav-item">
                                        <a id="step_screening" runat="server" href="#step-1" class="nav-link" >1<br /><small> <asp:Label runat="server" ID="lblt_prescreening">Application Pre-Screening</asp:Label> </small></a>
                                    </li>
                                    <li class="nav-item">
                                        <a id="step_solic" runat="server" href="#step-2" class="nav-link" >2<br /><small> <asp:Label runat="server" ID="lblt_solicitation">Solicitation Overview</asp:Label> </small></a>
                                    </li>
                                    <li class="nav-item">
                                        <a  id="step_start_Application" runat="server" href="#step-3" class="nav-link" >3<br /><small> <asp:Label runat="server" ID="lblt_start">Start Application</asp:Label></small></a>
                                    </li>   
                                   <li class="nav-item">
                                      <a  id="step_support_documents" runat="server" href="#step-4" class="nav-link" >4<br /><small> <asp:Label runat="server" ID="lblt_support_documents">Support Documents</asp:Label></small></a>
                                   </li>
                                   <li class="nav-item">
                                         <a  id="step_apply" runat="server" href="#step-5" class="nav-link" >5<br /><small> <asp:Label runat="server" ID="lblt_apply">Apply</asp:Label></small></a>
                                   </li>
                              </ul>
                            
                            <hr />

                            <div class="card tab-content border-0">

                                 <div class="card-header">
                                     <h6 class="card-title">
                                         <asp:Label runat="server" ID="lblt_tab_label">Application Pre-Screening</asp:Label>
                                     </h6>   
                                  <%-- <div class="card-tools">
                                         <telerik:RadButton ID="btn_upload" runat="server" CssClass="btn btn-sm btn-default float-right" AutoPostBack="true" Width="15%" Text="Upload participants from excel file"></telerik:RadButton>  
                                   </div>--%>

                               </div>

                                  <%--***********************************************  STEP #1 ********************************************************************--%>
                                  <%--*********************************************** # SCREENING # ************************************************************--%>
                                  <div id="step-1" class="card-body tab-pane border-0"  role="tabpanel">

                                                       <div class="row">

                                                                 <div class="col-sm-11">                                     

                                                                                <div class="card card-warning">
                                                                                  <div class="card-header">
                                                                                    <h3 class="card-title"> <i class="fas fa-info-circle"></i>nformation</h3>
                                                                                    <div class="card-tools">
                                                                                      <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                                                                                      </button>                                                                         
                                                                                    </div>
                                                                                    <!-- /.card-tools -->
                                                                                  </div>
                                                                                  <!-- /.card-header -->
                                                                                  <div class="card-body">                                                                        
                                                                                     <p class="text-danger"><i class="fas fa-check-circle"></i>&nbsp;Follow the steps below to proceed</p>
                                                                                      <ol>
                                                                                          <li>Please read carefully the Prescreening questions.</li>
                                                                                          <li>After completing the assessment, please click on <span class="fas fa-paper-plane badge-warning">&nbsp;Send PreScreening</span>.</li> 
                                                                                          <li>The BANGLADESH HORTICULTURE, FRUITS, AND NON-FOOD CROPS ACTIVITY will get back to you soon with a response.</li>
                                                                                          <li>Thank you for your Expression of Interest.</li>
                                                                                      </ol>
                                                                                  </div>
                                                                                  <!-- /.card-body -->
                                                                                </div>
                                                                                <!-- /.card -->                                                                                                                  
                                                               </div>                                                         
                                                          </div>  


                                                  <div class="row">
                                                     <div class="col-sm-12">
                                                          <div class="form-group">
                                                              <hr />
                                                          </div>
                                                     </div>
                                                 </div>

                                                            <%--  <div class="card-body table-responsive p-0">
                                                                  <table class="table table-hover text-nowrap">
                                                                      <thead>
                                                                        <tr>
                                                                          <th>#</th>
                                                                          <th>Question</th>
                                                                          <th>Answer</th>
                                                                          <th>check</th>                                                                          
                                                                        </tr>
                                                                      </thead>
                                                                        <tbody>                            
                                                                          <asp:Repeater ID="rpt_Assesment" runat="server">
                                                                              <ItemTemplate>     


                                                                              </ItemTemplate>
                                                                           </asp:Repeater>
                                                                        </tbody>
                                                                  </table>                                                                   
                                                              </div>--%>


                                                   <div class="row">

                                                     <div class="col-sm-12">

                                                          <div class="form-group">
                                                                       
                                                                 <div  id="questions_prescreening" runat="server" style="display:block;" class=" bg-gray-light ">

                                                                               <telerik:RadGrid ID="grd_screening" runat="server" AllowAutomaticDeletes="True" PageSize="30"
                                                                              AllowSorting="True" AutoGenerateColumns="False" >
                                                                              <ClientSettings EnableRowHoverStyle="true">
                                                                                  <Selecting AllowRowSelect="True"></Selecting>                                                                                                                                       
                                                                              </ClientSettings>
                                                                              <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_question, id_measurement_answer_scale, id_measurement_question_config, id_measurement_question_eval" AllowAutomaticUpdates="True">
                                                                                  <Columns>

                                                                                      <telerik:GridBoundColumn DataField="id_measurement_question"
                                                                                          DataType="System.Int32" HeaderText="id_measurement_question" ReadOnly="True"
                                                                                           UniqueName="id_measurement_question" Visible="true" Display="false">
                                                                                      </telerik:GridBoundColumn>

                                                                                         <telerik:GridBoundColumn DataField="id_measurement_answer_scale"
                                                                                          DataType="System.Int32" HeaderText="id_measurement_answer_scale" ReadOnly="True"
                                                                                           UniqueName="id_measurement_answer_scale" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                       <telerik:GridBoundColumn DataField="order_numberQU"
                                                                                          DataType="System.Int32" HeaderText="order_numberQU" ReadOnly="True"
                                                                                           UniqueName="order_numberQU" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridBoundColumn DataField="id_measurement_question_config"
                                                                                          DataType="System.Int32" HeaderText="id_measurement_question_config" ReadOnly="True"
                                                                                           UniqueName="id_measurement_question_config" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                      <telerik:GridBoundColumn DataField="id_measurement_question_eval"
                                                                                          DataType="System.Int32" HeaderText="id_measurement_question_config_eval" ReadOnly="True"
                                                                                           UniqueName="id_measurement_question_config_eval" Visible="true" Display="false">
                                                                                      </telerik:GridBoundColumn>
                                                                                      

                                                                                      <telerik:GridBoundColumn DataField="answer_type_code"
                                                                                          HeaderText="answer_type_code" ReadOnly="True"
                                                                                           UniqueName="answer_type_code" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                                                                                         
                                                                                      <telerik:GridBoundColumn DataField="question_name" HeaderText="question_name" UniqueName="question_name" Visible="true" Display="false" >
                                                                                      </telerik:GridBoundColumn>
                                                                  
                                                                                   <%--   <telerik:GridBoundColumn DataField="No" FilterControlAltText="Filter No column"
                                                                                          HeaderText="No" SortExpression="No" UniqueName="No" >
                                                                                      </telerik:GridBoundColumn>--%>
                                                                  
                                                                                      <telerik:GridTemplateColumn FilterControlAltText="Filter Question column"
                                                                                          HeaderText="&nbsp;" UniqueName="colm_Question" ItemStyle-Width="40%" >
                                                                                          <ItemTemplate>
                                                                                              <telerik:RadTextBox ID="txt_colm_SCREENING_QUESTIONS" runat="server" Rows="3" TextMode="MultiLine" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                                                                                          </ItemTemplate>
                                                                                           <ItemStyle Width="60%" CssClass="wrapWord"  />
                                                                                           <HeaderStyle Width="60%" />

                                                                                      </telerik:GridTemplateColumn>

                                                                                      <telerik:GridTemplateColumn FilterControlAltText="Filter answer column" HeaderText="&nbsp;" UniqueName="colm_answer_CONTROL" >
                                                                                          <ItemTemplate>                                                                                                                  
                                                                                              <telerik:RadComboBox ID="cmb_answer" runat="server" Width="100%" EmptyMessage="Select the answer" ></telerik:RadComboBox>                                                                                            
                                                                                              <telerik:RadTextBox ID="txt_answer_text" runat="server" Rows="3" TextMode="MultiLine" Width="100%"  ></telerik:RadTextBox>
                                                                                              <telerik:RadNumericTextBox  runat="server" ID="txt_answer_value" name="txt_answer_value"  Width="100%" Value="0" ></telerik:RadNumericTextBox>
                                                                                          </ItemTemplate>

                                                                                           <ItemStyle Width="40%" CssClass="wrapWord"  />
                                                                                           <HeaderStyle Width="40%" />
                                                                                      </telerik:GridTemplateColumn>

                                                                                                                                                                 
                                                                                  </Columns>
                                                                              </MasterTableView>
                                                                          </telerik:RadGrid>                                                                                                            

                                                                 </div>

                                                                 <div  id="app_prescreening" runat="server" style="display:none;" class=" bg-gray-light ">

                                                                                                  <div class="timeline">

                                                                                                        <asp:Repeater ID="rept_PrescreeningDates" runat="server">
                                                                                                          <ItemTemplate>                                                                                            
                                                                                                               <div class="time-label">
                                                                                                                  <span class="<%# Eval("nColor") %>">
                                                                                                                          <%# getFecha(Eval("date_created"), "D", False) %>
                                                                                                                  </span>
                                                                                                              </div>
                                                                                                               <asp:Repeater ID="rept_PrescreeningComm" runat="server">
                                                                                                                  <ItemTemplate>
                                                                                                                       <!-- timeline item -->
                                                                                                                          <div>
                                                                                                                              <!-- timeline icon -->
                                                                                                                              <i class="fa <%# Eval("STATUS_ICON") %>" title="<%# Eval("SCREENING_STATUS") %>&nbsp;by&nbsp;<%# Eval("USERNAME") %>"></i>
                                                                                                                              <div class="timeline-item">
                                                                                                                                  <span class="time"><i class="fa fa-clock-o"></i><%# getHora(Eval("FECHA_CREA")) %></span>

                                                                                                                                  <h3 class="timeline-header"> <img class="direct-chat-img <%# Eval("IMGCOLOR") %>" src="<%# Eval("USERIMAGEN") %>" alt="<%# Eval("USERNAME") %>" title="<%# Eval("USERNAME") %>">&nbsp;&nbsp;&nbsp;<a href="#"><%# Eval("USERNAME") %></a></h3>
                                                                                                                                  <div class="timeline-body">
                                                                                                                                      <%# Eval("SCREENING_COMM") %>                                                                                                                       
                                                                                                                                   </div>

                                                                                                                                  <div class="timeline-footer">
                                                                                                                                     <%-- <a class="btn btn-primary btn-xs">React</a>--%>
                                                                                                                                  </div>
                                                                                                                              </div>

                                                                                                                          </div>
                                                                                                                          <!-- END timeline item -->
                                                                                                                   </ItemTemplate>  
                                                                                                               </asp:Repeater>  
                                                                              
                                                                                                         </ItemTemplate>
                                                                                                      </asp:Repeater>          
                                                                  
                                                                                                </div> <%--TimeLine--%>                                                                                                                                                                                                                                                         
                                                                                          
                                                                                         <br />
                                                                                    
                                                                                    </div> <%--app_Prescreening--%>
                                                                                                                                                                   

                                                          </div>

                                                     </div>

                                                 </div>

                                                  <div class="row">
                                                      <div class="col-sm-4">
                                                           <div class="form-group">
                                                                <asp:Label runat="server" ID="lblt_error" Visible="false" ForeColor="Red">Some questions are pending answers,  please complete the questions.</asp:Label>
                                                            </div>
                                                      </div>       
                                                     <div class="col-sm-4 text-center">
                                                           <div class="form-group">
                                                                  <asp:LinkButton ID="btnlk_sent_screening" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" class="btn btn-warning btn-sm" data-toggle="Send Screening" Width="90%" ><i class="fas fa-paper-plane fa-2x"></i>&nbsp;&nbsp;Send PreScreening</asp:LinkButton>                                                                                         
                                                            </div>
                                                      </div>        
                                                     <div class="col-sm-4">
                                                           <div class="form-group">
                                                    
                                                            </div>
                                                      </div>        
                                                 </div>

                                               
                                      
                                  
                                  
                                  
                                  
                                  
                                  
                                  
                                  </div>                            
                            
                                <%--***********************************************  STEP #3 ********************************************************************--%>
                                <%--*********************************************** # SOLICITATION # ************************************************************--%>
                               <div id="step-2" class="card-body tab-pane border-0"  role="tabpanel">
                                                                     
			                               <div class="row">
                                              <div class="col-sm-6">
                                                   <div class="form-group">
                                                       <asp:Label runat="server" ID="lblt_solicitation_code" CssClass="control-label text-bold">Solicitation Code</asp:Label><br />
                                                         <div class=" alert-info text-center" runat="server" id="divCodigo" style="width: 300px;">
                                                             <asp:Label ID="lbl_COde" runat="server" CssClass="text-bold" ></asp:Label>
                                                         </div>       
                                                    </div>
                                              </div>
                                             
                                                <div class="col-sm-6">
                                                  <div class="form-group">
                                                       <asp:Label runat="server" ID="lblt_Activity_code" CssClass="control-label text-bold">Application Code</asp:Label><br />
                                                       <telerik:RadTextBox ID="txt_activity_code" runat="server" Width="80%" MaxLength="250" ReadOnly="true" >
                                                       </telerik:RadTextBox>
                                                  </div>
                                              </div>
                                           
                                           </div>


                                                <div class="row">
                                                  <div class="col-sm-6">
                                                       <div class="form-group">
                                                           <asp:Label runat="server" ID="lblt_solicitation_type" CssClass="control-label text-bold">Solicitation Type</asp:Label><br />
                                                           <telerik:RadComboBox ID="cmb_solicitation_type" runat="server" Width="300px" EmptyMessage="Pick one category" Enabled="false" ></telerik:RadComboBox>
                                                        </div>
                                                  </div>
                                                  <div class="col-sm-6">
                                                       <div class="form-group">
                                                            <asp:Label runat="server" ID="lblt_solicitation_status" CssClass="control-label text-bold">Solicitation Status</asp:Label><br />
                                                            <telerik:RadComboBox ID="cmb_solicitation_status" runat="server" Width="300px" Enabled="false" ></telerik:RadComboBox>
                                                        </div>
                                                  </div>
                                                </div>

                                                <div class="row">
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                                 <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Issuance Date</asp:Label><br />
                                                                 <asp:Label runat="server" ID="lbl_fecha_inicio" CssClass="control-label text-bold"></asp:Label>
                                                            </div>
                                                      </div>
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                                <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">Close Date</asp:Label><br />
                                                                <asp:Label runat="server" ID="lbl_fecha_final" CssClass="control-label text-bold">Close Date</asp:Label>
                                                            </div>
                                                      </div>
                                                    </div>
                                                
                                                      <div class="row">
                                                          <div class="col-sm-12">
                                                               <div class="form-group">
                                                                   <hr />
                                                                </div>
                                                          </div>                                                         
                                                        </div>      
                                   
                                                     <div class="row">
                                                      <div class="col-sm-12">
                                                           <div class="form-group">
                                                                 <asp:Label runat="server" ID="lblt_Tittle" CssClass="control-label text-bold">Solicitation Title</asp:Label><br />
                                                                  <telerik:RadTextBox ID="txt_tittle" runat="server" Rows="7" TextMode="MultiLine" Width="90%" MaxLength="2000" ReadOnly="true"  BorderStyle="None">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                                                ControlToValidate="txt_tittle" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                            </div>
                                                      </div>                                                     
                                                    </div>
                                                                         
                                                                                                                                   
                                                    <div class="row">
                                                      <div class="col-sm-12">

                                                           <div class="form-group">
                                                                  <asp:Label runat="server" ID="lbl_purpose" CssClass="control-label text-bold">Purpose</asp:Label><br />
                                                                  <telerik:RadTextBox ID="txt_purpose" runat="server" Rows="7" TextMode="MultiLine" Width="90%" MaxLength="2000"  ReadOnly="true" BorderStyle="None">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                                                ControlToValidate="txt_purpose" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                            </div>
                                                      </div>                                                     
                                                    </div>                
                                                               
                                   
                                                   <%--  <div class="row">
                                                      <div class="col-sm-12">
                                                           <div class="form-group">
                                                                 <asp:Label runat="server" ID="lblt_solicitation_content" CssClass="control-label text-bold">Solicitation</asp:Label><br />
                                                                   <telerik:RadTextBox ID="txt_solicitation" runat="server" Rows="7" TextMode="MultiLine" Width="90%" MaxLength="5000"   ReadOnly="true" BorderStyle="None">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                                ControlToValidate="txt_solicitation" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                            </div>
                                                      </div>                                                     
                                                    </div> --%>

                                      
                                                         
                                                      <div class="row">
                                                          <div class="col-sm-12">
                                                               <div class="form-group">
                                                                   <hr />
                                                                </div>
                                                          </div>                                                         
                                                        </div>     
                                   
                                                        <div class="row">
                                                          <div class="col-sm-12">
                                                               <div class="form-group">
                                                                               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <telerik:RadGrid ID="grd_support_Documents" runat="server" AllowAutomaticDeletes="True"
                                                                                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                                                    <ClientSettings EnableRowHoverStyle="true">
                                                                                                                        <Selecting AllowRowSelect="True" />
                                                                                                                    </ClientSettings>
                                                                                                                    <MasterTableView DataKeyNames="ID_ACTIVITY_ANNEX">
                                                                                                                        <Columns>
                                                                                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column"
                                                                                                                                HeaderButtonType="PushButton" UniqueName="ImageDownload">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Download attachment" />
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle Width="5px" />
                                                                                                                                <ItemStyle Width="5px" />
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" Visible="false"
                                                                                                                                ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                                                ConfirmDialogWidth="400px"
                                                                                                                                ConfirmText="would you like to delete this file?"
                                                                                                                                ConfirmTitle="Delete file" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                                                UniqueName="Eliminar" />
                                                                                                                              <telerik:GridBoundColumn DataField="nombre_documento"
                                                                                                                                FilterControlAltText="Filter nombre_documento column"
                                                                                                                                HeaderText="Type" SortExpression="nombre_documento"
                                                                                                                                UniqueName="colm_nombre_documento">
                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                              <telerik:GridBoundColumn DataField="DOCUMENT_TITLE"
                                                                                                                                FilterControlAltText="FilterDOCUMENT_TITLE column"
                                                                                                                                HeaderText="Title" SortExpression="DOCUMENT_TITLE"
                                                                                                                                UniqueName="colm_DOCUMENT_TITLE">
                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                            <telerik:GridBoundColumn DataField="DOCUMENT_NAME"
                                                                                                                                FilterControlAltText="Filter DOCUMENT_NAME column"
                                                                                                                                HeaderText="Documents" SortExpression="DOCUMENT_NAME"
                                                                                                                                UniqueName="colm_DOCUMENT_NAME">
                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                        </Columns>
                                                                                                                    </MasterTableView>
                                                                                                                </telerik:RadGrid>                                                                                                               
                                                                                                            </ContentTemplate>
                                                                                 </asp:UpdatePanel>
                                                                </div>
                                                          </div>                                                         
                                                        </div>                                      
                                                    
                               
                                        </div>      

                                 <%--*********************************************** # SOLICITATION # ************************************************************--%>
                                 <%--***********************************************  STEP #2 ********************************************************************--%>


                                <%--***********************************************  STEP #3 ************************************************************--%>
                                <%--*********************************************** # START APPLICATION # ************************************************************--%>
                                <div id="step-3" class="card-body tab-pane" role="tabpanel">   
                                         

                                                   <div class="row">
                                                            <div class="col-sm-12">                                     
                                                                   <div class="card card-warning">
                                                                     <div class="card-header">
                                                                       <h3 class="card-title"> <i class="fas fa-info-circle"></i>nformation</h3>
                                                                       <div class="card-tools">
                                                                         <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                                                                         </button>                                                                         
                                                                       </div>
                                                                       <!-- /.card-tools -->
                                                                     </div>
                                                                     <!-- /.card-header -->
                                                                     <div class="card-body">                                                                        
                                                                        <p class="text-danger"><i class="fas fa-check-circle"></i>&nbsp;Before starting the application remember...</p>
                                                                         <ol>
                                                                             <li>Please read carefully the solicitation requirements.</li>
                                                                             <li>To start the application process,click on the <span class="fas fa-hourglass-start badge badge-warning">Start Application</span> button shown below.</li>
                                                                             <li>This will take you to phase 3 where you can upload the required documents.</li>
                                                                             <li>Once you have uploaded, please click Next to submit your application in Phase 4.</li>
                                                                             <li>If you have any further questions please feel free to send your questions to &nbsp; <span class="fas fa-envelope"></span>&nbsp;&nbsp;<a href="mailto:shrahman@chemonics.com">shrahman@chemonics.com</a>, we will get back to you soon.</li>
                                                                         </ol>
                                                                     </div>
                                                                     <!-- /.card-body -->
                                                                   </div>
                                                                   <!-- /.card -->                                                                                                                  
                                                               </div>                                                         
                                                          </div>      


                                     <div class="row">
                                          <div class="col-sm-4">
                                               <div class="form-group">
                                                    
                                                </div>
                                          </div>       
                                         <div class="col-sm-4 text-center">
                                               <div class="form-group">
                                                      <asp:LinkButton ID="btn_save_app" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" class="btn btn-warning btn-lg" data-toggle="Start" Width="90%" ><i class="fas fa-hourglass-start fa-2x"></i>&nbsp;&nbsp;Start Application</asp:LinkButton>                                                                                         
                                                </div>
                                          </div>        
                                         <div class="col-sm-4">
                                               <div class="form-group">
                                                    
                                                </div>
                                          </div>        
                                     </div>

                                      <div class="row">
                                        <div class="col-sm-12">
                                             <div class="form-group">
                                                 <hr />
                                              </div>
                                        </div>                                                         
                                      </div>     

                                </div> <%--step-3--%>
                                <%--*********************************************** # START APPLICATION # ************************************************************--%>
                                <%--***********************************************  STEP #3 *************************************************************************--%>


                                <%--***********************************************  STEP #3 *************************************************************************--%>
                                <%--*********************************************** # SUPPORT DOCUMENTS # ************************************************************--%>
                                <div id="step-4" class="card-body tab-pane" role="tabpanel">   
                                        
                                   <%--**********************************************************************************APPLICANTS DOCUMENTS*************************************************************--%>
                                      
                                                 <div class="row">
                                                            <div class="col-sm-12">                                     
                                                                   <div class="card card-warning">
                                                                     <div class="card-header">
                                                                       <h3 class="card-title"> <i class="fas fa-info-circle"></i>nformation</h3>
                                                                       <div class="card-tools">
                                                                         <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                                                                         </button>                                                                         
                                                                       </div>
                                                                       <!-- /.card-tools -->
                                                                     </div>
                                                                     <!-- /.card-header -->
                                                                     <div class="card-body">                                                                        
                                                                        <p class="text-danger"><i class="fas fa-check-circle"></i>&nbsp;Before uploading an application document remember...</p>
                                                                         <ol>
                                                                             <li>First, add the document title you want to upload.</li>
                                                                             <li>Select the type of document you are going to upload.</li>
                                                                             <li>Attach the document by clicking on the <span class="badge badge-secondary">Select</span> button.</li>
                                                                             <li> Then click on <span class="fas fa-upload badge badge-info">Upload document</span> button.</li>
                                                                             <li>To remove any uploaded file, click on the delete option <span class="fas fa-times"></span> from the application document list and replace by repeating steps 1-4.</li>
                                                                             <li>Click on the NEXT button to proceed to Phase 4 once you have completed all uploads.</li>
                                                                             <li>If you have any further questions please feel free to send your questions to &nbsp; <span class="fas fa-envelope"></span>&nbsp;&nbsp;<a href="mailto:shrahman@chemonics.com">shrahman@chemonics.com</a>, we will get back to you soon.</li>
                                                                         </ol>
                                                                     </div>
                                                                     <!-- /.card-body -->
                                                                   </div>
                                                                   <!-- /.card -->                                                                                                                  
                                                               </div>                                                         
                                                          </div>      

                                                 <div class="row">
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                                <asp:Label runat="server" ID="lblt_documente_tittle" CssClass="control-label text-bold">Document Title</asp:Label><h5><span class="badge badge-warning">&nbsp;1&nbsp;</span></h5>  
                                                                <telerik:RadTextBox ID="txt_document_tittle" runat="server" Width ="80%" MaxLength="200">
                                                                  </telerik:RadTextBox><br />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                    ControlToValidate="txt_document_tittle"  Display="Dynamic"
                                                                    ErrorMessage="registre una descripción" ForeColor="Red" ValidationGroup="6">*Add the title for the document to upload</asp:RequiredFieldValidator>
                                                            </div>
                                                      </div>
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                                 <asp:Label runat="server" ID="lblt_Type_document" CssClass="control-label text-bold">Document Type</asp:Label><h5><span class="badge badge-warning">&nbsp;2&nbsp;</span></h5>  
                                                                 <telerik:RadComboBox ID="cmb_type_of_document" runat="server" Width="60%" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox>
                                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                    ControlToValidate="cmb_type_of_document"  Display="Dynamic"
                                                                    ErrorMessage="registre una descripción" ForeColor="Red" ValidationGroup="6">*Select the category for the document to upload</asp:RequiredFieldValidator>
                                                            </div>
                                                      </div>
                                                    </div>

                                                    <div class="row">
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                                <asp:Label runat="server" ID="lblt_documento" CssClass="control-label text-bold">Attach Document</asp:Label> <h5><span class="badge badge-warning">&nbsp;3&nbsp;</span></h5>                                                                
                                                                 <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                                                     TemporaryFolder  ="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                    ControlToValidate="AsyncUpload1"  Display="Dynamic"
                                                                    ErrorMessage="registre una descripción" ForeColor="Red" ValidationGroup="4">*Select the document to upload</asp:RequiredFieldValidator>--%>
                                                            </div>
                                                      </div>
                                                      <div class="col-sm-6">
                                                           <div class="form-group">                                                                 
                                                               <h5><span class="badge badge-warning">&nbsp;4&nbsp;</span></h5>  
                                                               
                                                                <telerik:RadButton ID="btn_agregar" runat="server" AutoPostBack="true" Text="add"  CssClass="btn btn-lg d-none" ValidationGroup="6" Width="50%">
                                                                </telerik:RadButton>

                                                                <asp:LinkButton ID="btnlk_add_doc" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="200" class="btn btn-info btn-sm" data-toggle="Export" ValidationGroup="6"  ><i class="fas fa-upload"></i>&nbsp;&nbsp;Upload document</asp:LinkButton>                            

                                                            </div>
                                                      </div>
                                                    </div>                                                                                      

                                                   <div class="row">
                                                          <div class="col-sm-12">
                                                               <div class="form-group">
                                                                    <hr />
                                                                </div>
                                                          </div>                                                         
                                                   </div>
                                                           
                                    
                                                       <div class="row">
                                                          <div class="col-sm-12">
                                                               <div class="form-group">
                                                                     <asp:Label runat="server" ID="Lblt_Documents" CssClass="control-label text-bold">Application Documents</asp:Label><br /><br />

                                                                           <asp:UpdatePanel ID="PanelFirma" runat="server" UpdateMode="Conditional">
                                                                                 <ContentTemplate>
                                                                                     <telerik:RadGrid ID="grd_archivos" runat="server" AllowAutomaticDeletes="True"
                                                                                         AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="100%">
                                                                                         <ClientSettings EnableRowHoverStyle="true">
                                                                                             <Selecting AllowRowSelect="True" />
                                                                                         </ClientSettings>

                                                                                         <MasterTableView DataKeyNames="ID_ACTIVITY_ANNEX">
                                                                                             <Columns>

                                                                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column"
                                                                                                     HeaderButtonType="PushButton" UniqueName="ImageDownload">
                                                                                                     <ItemTemplate>
                                                                                                         <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Download attachment" />
                                                                                                     </ItemTemplate>
                                                                                                     <HeaderStyle Width="5px" />
                                                                                                     <ItemStyle Width="5px" />
                                                                                                 </telerik:GridTemplateColumn>
                                                                                                 <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                                                     ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                     ConfirmDialogWidth="400px"
                                                                                                     ConfirmText="would you like to delete this file?"
                                                                                                     ConfirmTitle="Delete file" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                     UniqueName="Eliminar" />
                                                                                                   <telerik:GridBoundColumn DataField="nombre_documento"
                                                                                                     FilterControlAltText="Filter nombre_documento column"
                                                                                                     HeaderText="Type" SortExpression="nombre_documento"
                                                                                                     UniqueName="colm_nombre_documento">
                                                                                                 </telerik:GridBoundColumn>
                                                                                                   <telerik:GridBoundColumn DataField="DOCUMENT_TITLE"
                                                                                                     FilterControlAltText="FilterDOCUMENT_TITLE column"
                                                                                                     HeaderText="Title" SortExpression="DOCUMENT_TITLE"
                                                                                                     UniqueName="colm_DOCUMENT_TITLE">
                                                                                                 </telerik:GridBoundColumn>
                                                                                                 <telerik:GridBoundColumn DataField="DOCUMENT_NAME"
                                                                                                     FilterControlAltText="Filter DOCUMENT_NAME column"
                                                                                                     HeaderText="Documents" SortExpression="DOCUMENT_NAME"
                                                                                                     UniqueName="colm_DOCUMENT_NAME">
                                                                                                 </telerik:GridBoundColumn>
                                                                                                   <telerik:GridTemplateColumn 
                                                                                                       FilterControlAltText="Filter colm_mandatory column"  HeaderText="Mandatory" 
                                                                                                       UniqueName="colm_mandatory" >                                      
                                                                                                      <ItemTemplate>                                       
                                                                                                            <telerik:RadTextBox runat="server" ID="txtMandatory" Text=""></telerik:RadTextBox>                                 
                                                                                                      </ItemTemplate>
                                                                                                       <ItemStyle Width="20%"  />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                 <telerik:GridBoundColumn DataField="extension"
                                                                                                     FilterControlAltText="Filter extension column"
                                                                                                     HeaderText="Files Allowed" SortExpression="extension"
                                                                                                     UniqueName="colm_extension">
                                                                                                 </telerik:GridBoundColumn>
                                                                                                  <telerik:GridBoundColumn DataField="max_size"
                                                                                                     FilterControlAltText="Filter max_size column"
                                                                                                     HeaderText="Max Size" SortExpression="max_size"
                                                                                                     UniqueName="colm_max_size">
                                                                                                 </telerik:GridBoundColumn>
                                                                                                   <telerik:GridBoundColumn DataField="Template" 
                                                                                                         FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                                         UniqueName="Template" Visible="true" Display="false">                                        
                                                                                                     </telerik:GridBoundColumn>
                                                                                                  <telerik:GridTemplateColumn 
                                                                                                       FilterControlAltText="Filter colm_template column"  HeaderText="Document Template" 
                                                                                                       UniqueName="colm_template" >                                      
                                                                                                      <ItemTemplate>                                       
                                                                                                          <asp:HyperLink ID="hlk_Template" 
                                                                                                              runat="server" 
                                                                                                              Text="--none--"                                                                                    
                                                                                                              navigateUrl="#"></asp:HyperLink>                                       
                                                                                                      </ItemTemplate>
                                                                                                       <ItemStyle Width="20%"  />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                      <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_approvals">
                                                                                                            <ItemTemplate>
                                                                                                            <asp:HyperLink ID="aprobar" runat="server" ImageUrl="" Target="_blank" />
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="5px" />
                                                                                                        </telerik:GridTemplateColumn>


                                                                                             </Columns>
                                                                                         </MasterTableView>
                                                                                     </telerik:RadGrid>
                                                                                     <br />
                                                                                 </ContentTemplate>
                                                                             </asp:UpdatePanel>

                                                                </div>
                                                          </div>                                                         
                                                     </div>
                                                            
                                                                                            
                                                                                               
                                     
                                         <%--**********************************************************************************APPLICANTS DOCUMENTS*************************************************************--%>                                           


                                </div> <%--step-4--%>
                               <%--*********************************************** # SUPPORT DOCUMENTS # ************************************************************--%>
                               <%--***********************************************  STEP #4 ************************************************************--%>

                             

                                <%--***********************************************  APPLY #4 ************************************************************--%>
                                <%--*********************************************** # APPLY # ************************************************************--%>
                                <div id="step-5" class="card-body tab-pane" role="tabpanel">   
                                    
                                      <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                      
                                             <%-- <div id="Applications" class="tab-pane fade" >--%>
                                                                                               
                                                    <%--**********************************************************************************APLICATIONS*************************************************************--%>                                           
                                                      



                                                                 <div class="row">
                                                                      <div class="col-sm-6">
                                                                           <div class="form-group">
                                                                                <asp:Label runat="server" ID="lblt_apply_code" CssClass="control-label text-bold">Apply Code</asp:Label><br />
                                                                                  <div class="alert bg-blue text-center" runat="server" id="div1" style="width: 60%;">
                                                                                     <asp:Label ID="lbl_apply_code" runat="server" CssClass="text-bold" ></asp:Label>
                                                                                </div>       
                                                                            </div>
                                                                      </div>
                                                                      <div class="col-sm-6">
                                                                           <div class="form-group">
                                                                               <asp:Label runat="server" ID="lblt_apply_status" CssClass="control-label text-bold">Apply Status</asp:Label><br />
                                                                               <h5><span id="spanSTATUS" runat="server" class='badge ' > <asp:Label runat="server" ID="lbl_apply_status" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_Apply_time" Text=""></asp:Label></span></h5>
                                                                           </div>
                                                                      </div>
                                                                    </div>
                                                                                             
                                                                       <div class="row">
                                                                          <div class="col-sm-6">
                                                                               <div class="form-group">
                                                                                   <asp:Label runat="server" ID="lblt_ORganization" CssClass="control-label text-bold">Organization</asp:Label><br />
                                                                                   <asp:Label runat="server" ID="lbl_organization" CssClass="control-label text-bold"></asp:Label>
                                                                                </div>
                                                                          </div>
                                                                          <div class="col-sm-6">
                                                                               <div class="form-group">
                                                                                     <asp:Label runat="server" ID="lblt_status_date" CssClass="control-label text-bold">Date updated</asp:Label><br />
                                                                                     <asp:Label runat="server" ID="lbl_status_date" CssClass="control-label text-bold"></asp:Label>
                                                                                </div>
                                                                          </div>
                                                                        </div>
                                              
                                                               <div class="row">
                                                                      <div class="col-sm-12">
                                                                           <div class="form-group">
                                                                                <hr />
                                                                            </div>
                                                                      </div>                                                         
                                                               </div>

                                                               <div class="row">
                                                                      <div class="col-sm-12">
                                                                          
                                                                         <div  id="Buttons_app" runat="server" style="display:block;">                                                                      
                                                                          <br />
                                                                                   
                                                                                   <div class="col-sm-12">
                                                                                        <div class="form-group text-center"> 
                                                                                                   <asp:HiddenField runat="server" ID="id_app_" />
                                                                                                   <asp:HiddenField runat="server" ID="tab_index_order" Value="0" />
                                                                                                   <asp:HiddenField runat="server" ID="tab_index_max" Value="1" />

                                                                                                            <asp:Label runat="server" ID="lblt_apply_desc" CssClass="control-label text-bold">Application comments</asp:Label><br /><br />

                                                                                                            <telerik:RadTextBox ID="txt_apply_desc" runat="server" Rows="10" TextMode="MultiLine" Width="95%" >
                                                                                                            </telerik:RadTextBox>
                                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                                                ControlToValidate="txt_apply_desc" CssClass="Error" Display="Dynamic"
                                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="3">Comments are required on the applying</asp:RequiredFieldValidator>
                                                                                         </div>
                                                                                    </div>                                                                               
                                                                               
                                                                                      <div class="col-sm-12">

                                                                                               <div  class="form-group text-center" >

                                                                                                        <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-info btn-sm" Visible="false">
                                                                                                        </telerik:RadButton>

                                                                                                        <telerik:RadButton ID="btn_continue" runat="server" Text="Save Application" AutoPostBack="true" CssClass="btn btn-info btn-sm "
                                                                                                        Width="100px" ValidationGroup="1" Visible="false">
                                                                                                        </telerik:RadButton>                                                                          

                                                                                                       <asp:LinkButton ID="btnlk_Apply" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="200" class="btn btn-success btn-sm" data-toggle="Export" ValidationGroup="3"  ><i class="fa fa-paper-plane fa-2x"></i>&nbsp;&nbsp;Apply</asp:LinkButton>                                                                                                                                                
                                                                                                       
                                                                                                                                           
                                                                                                </div>
                                                                                                

                                                                                        </div>

                                                                          </div>     
                                                                             

                                                                      </div>                                                         
                                                               </div>

                                                                 <div class="row">
                                                                      <div class="col-sm-12">
                                                                           <div class="form-group">
                                                                                <hr style="border-color:black" />
                                                                            </div>
                                                                      </div>                                                         
                                                               </div>                 

                                                              
                                                                <div class="row">
                                                                      <div class="col-sm-12">
                                                                           <div class="form-group">
                                                                                     <div  id="app_History" runat="server" style="display:block;" class=" bg-gray-light ">

                                                                                                  <div class="timeline">

                                                                                                        <asp:Repeater ID="rept_ApplyDates" runat="server">
                                                                                                          <ItemTemplate>                                                                                            
                                                                                                               <div class="time-label">
                                                                                                                  <span class="<%# Eval("nColor") %>">
                                                                                                                          <%# getFecha(Eval("date_created"), "D", False) %>
                                                                                                                  </span>
                                                                                                              </div>
                                                                                                               <asp:Repeater ID="rept_ApplyComm" runat="server">
                                                                                                                  <ItemTemplate>
                                                                                                                       <!-- timeline item -->
                                                                                                                          <div>
                                                                                                                              <!-- timeline icon -->
                                                                                                                              <i class="fa <%# Eval("STATUS_ICON") %>" title="<%# Eval("APPLY_STATUS") %>&nbsp;by&nbsp;<%# Eval("USERNAME") %>"></i>
                                                                                                                              <div class="timeline-item">
                                                                                                                                  <span class="time"><i class="fa fa-clock-o"></i><%# getHora(Eval("FECHA_CREA")) %></span>

                                                                                                                                  <h3 class="timeline-header"> <img class="direct-chat-img <%# Eval("IMGCOLOR") %>" src="<%# Eval("USERIMAGEN") %>" alt="<%# Eval("USERNAME") %>" title="<%# Eval("USERNAME") %>">&nbsp;&nbsp;&nbsp;<a href="#"><%# Eval("USERNAME") %></a></h3>
                                                                                                                                  <div class="timeline-body">
                                                                                                                                      <%# Eval("APPLY_COMM") %>                                                                                                                       
                                                                                                                                   </div>

                                                                                                                                  <div class="timeline-footer">
                                                                                                                                     <%-- <a class="btn btn-primary btn-xs">React</a>--%>
                                                                                                                                  </div>
                                                                                                                              </div>

                                                                                                                          </div>
                                                                                                                          <!-- END timeline item -->
                                                                                                                   </ItemTemplate>  
                                                                                                               </asp:Repeater>  
                                                                              
                                                                                                         </ItemTemplate>
                                                                                                      </asp:Repeater>          
                                                                  
                                                                                                </div> <%--TimeLine--%>                                                                                                                                                                                                                                                         
                                                                                          
                                                                                         <br />
                                                                                    
                                                                                    </div> <%--app_History--%>
                                                                            </div>
                                                                      </div>                                                         
                                                               </div>                                                       
                                                             
                                                      
                                                              
                                                                <div  id="Buttons_approve" runat="server" style="display:none; padding-left:10px;" class="bg-gray-light">

                                                                      <div  class="form-group row" style="height:450px;" >
                                                                           <div class="col-sm-12 ">

                                                                                       <asp:Label runat="server" ID="lblt_approvalComments" CssClass="control-label text-bold">Apply Comments</asp:Label><br /><br />
                                                                                
                                                                                         <%-- <telerik:RadTextBox ID="txt_approve_comments" runat="server" Rows="5" TextMode="MultiLine" Width="97%" >
                                                                                          </telerik:RadTextBox>  onclientLoad="OnClientLoad" --%>

                                                                                           <telerik:RadEditor runat="server" ID="Editor_approve_comments"  Height="250" Width="97%" MaxHtmlLength="4000" DialogsCssFile="~/Content/RadEditor_Styles.css"  >
                                                                                              <ImageManager ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                                  UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                                  DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations">
                                                                                              </ImageManager>
                                                                                          </telerik:RadEditor>
                                                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor_approve_comments"  ForeColor="Red" ErrorMessage="Message required" ValidationGroup="4"></asp:RequiredFieldValidator>
                                                                                          <br />
                                                                                          
                                                                             </div>
                                                                      </div>

                                                                      <div  class="form-group row" >

                                                                          <div class="col-sm-2">
                                                                          </div>
                                                                        <div class="col-sm-4">                                                                                                     
                                                                            <asp:LinkButton ID="btnlk_comment" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="80%" class="btn btn-default  btn-sm margin-r-5 pull-left" data-toggle="Accept"  ValidationGroup="4" ><i class="fa fa-comment fa-2x" ></i>&nbsp;&nbsp;Comment</asp:LinkButton>                                          
                                                                         </div>

                                                                   <%--     <div class="col-sm-2">                                                                                                     
                                                                               <asp:LinkButton ID="bntlk_accept" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="99%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Accept" ValidationGroup="4" ><i class="fa fa-thumbs-o-up fa-2x"></i>&nbsp;&nbsp;Accept</asp:LinkButton>                                          
                                                                            </div>

                                                                           <div class="col-sm-2">                                                                                                     
                                                                                <asp:LinkButton ID="btnlk_reject" runat="server" AutoPostBack="True" SingleClick="true"  Text="Reject" Width="99%" class="btn btn-danger btn-sm margin-r-5 pull-left" data-toggle="Reject"  ValidationGroup="4" ><i class="fa fa-thumbs-o-down fa-2x"></i>&nbsp;&nbsp;Reject</asp:LinkButton>                                          
                                                                           </div>

                                                                           <div class="col-sm-2">                                                                                                     
                                                                                <asp:LinkButton ID="btnlk_hold" runat="server" AutoPostBack="True" SingleClick="true"  Text="Hold" Width="99%" class="btn btn-warning btn-sm margin-r-5 pull-left" data-toggle="Hold"   ValidationGroup="4"  ><i class="fa  fa-hand-stop-o fa-2x"></i>&nbsp;&nbsp;On Hold</asp:LinkButton>                                          
                                                                           </div>--%>
                                                                           <div class="col-sm-4">                                                                                                     
                                                                                <asp:LinkButton ID="btnlk_Apply2" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="80%" class="btn btn-success btn-sm " data-toggle="Export" ValidationGroup="4"  ><i class="fa fa-paper-plane fa-2x"></i>&nbsp;&nbsp;Apply</asp:LinkButton>
                                                                           </div>
                                                                           <div class="col-sm-2">
                                                                           </div>

                                                                    </div>
                                                                    <br />
                                                                             
                                                          </div>  

                                                             <div class="row">
                                                                      <div class="col-sm-12">
                                                                           <div class="form-group">
                                                                                <hr style="border-color:black" />
                                                                            </div>
                                                                      </div>                                                         
                                                               </div>       
                                                  
                                                                                                                                                   
                                                 <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                                   

                                          <%--   </div> --%>  <%--id="Applications"--%>


                                           <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                      


                                </div> <%--step-5--%>
                               <%--*********************************************** # APPLY # ************************************************************--%>
                               <%--***********************************************  # APPLY # ************************************************************--%>



                       <%--       <div id="ADDons" runat="server" class="box-body" style="border:1px solid thick; display:block;">
                                    
                                   
                                       <ul class="nav nav-tabs">
                                         <li class="active"><a data-toggle="tab" href="#Solicitation"><asp:Label runat="server" ID="lblt_label_tab" CssClass="control-label text-bold">Solicitation Overview</asp:Label></a></li>                                    
                                         <li><a data-toggle="tab" href="#Applications"><asp:Label runat="server" ID="lblt_Applications" CssClass="control-label text-bold">Apply Solicitation</asp:Label></a></li>                                             
                                       </ul>
                                      <div class="tab-content">                                             
                                         
                                     
                                      </div>--%>
                                  
                                  <%--class="tab-content"--%>                                                                       
                                                                 
                             <%--</div> --%>
                                     
                            <%--class="box-body"--%>

                            
               <%--******************************************************************************************************--%>


                      </div> <%--tab-content--%>      
                            
                                               
                    </div>  <%--id="stepwizard"--%>

                
                </div> <%--card body interno--%>
                                       
            </div> <%--card interno--%>

                         
                <!-- /.box-footer -->
                <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Remove Record</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_eliminarDocumento" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal fade bs-example-modal-sm" id="modalTasaCambio" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                    <h4 class="modal-title" runat="server" id="H1">Alerta</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblt_msn_tasa_cambio" runat="server" Text="Debe ingresar la tasa de cambio correspondiente al periodo" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_registrar_tc" CssClass="btn btn-sm btn-primary btn-ok" Text="Registrar tasa de cambio" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
          
             </div>
        
         </div>
                 
   </div> <%--container-fluid--%>

 </section>


       <script type="text/javascript">
            
            //  jQuery.noConflict();

           
                     var prm = Sys.WebForms.PageRequestManager.getInstance();
                          prm.add_endRequest(function() {
                                // re-bind your jQuery events here
                                loadscript();

                          });

                          $(document).ready(function () {
                              loadscript();              
                             });

        
                           // $(document).ready(function () {

                           //         var QueryVariable = getParameterByName('_tab');
                           //          //alert(QueryVariable);

                           //         if (QueryVariable != '' || QueryVariable != null) {
                           //             $('#dvTab a[href="#' + QueryVariable + '"]').tab('show');
                           //         }
                
                           //});     

                     
                            function loadscript() {

                                var TABindex = $("[id*=tab_index_order]").val() != "" ? $("[id*=tab_index_order]").val() : "0";
                                //console.log('Tab index order ' + TABindex);
                                                                                             
                                 set_step(TABindex);     
                                
                                       //*******************************************TABS**************************************************************************
                                       //*******************************************TABS**************************************************************************

                                             //var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Beneficiaries";



                                             // $('#Tabs a[href="#' + tabName + '"]').tab('show');                                             
                      
                                             //   $("#Tabs a").click(function () {

                                             //     // console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                                             //      $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                                             // });

                                        //*******************************************TABS**************************************************************************
                                       //*******************************************TABS**************************************************************************



                                } //end Load Script Page


                                  function set_step(IndexINI,_Reset) {
                     
                                                console.log('Initialazing Step Wizard on ' + IndexINI);

                                                 //$('#stepwizard').on('leaveStep', function(e, anchorObject, currentStepIndex, nextStepIndex, stepDirection) {
                                                 //    //console.log('Do you want to leave the step ' + currentStepIndex + '?' + ' nextStep: ' + nextStepIndex + ' stepDirection: ' + stepDirection);                      
                                                 //    //FuncCancel('set_step(' + currentStepIndex + ',1)','change_step(' + nextStepIndex + ')', 'Do you want to move to the Participant?', 'frm_saleParticipants?id=' + getParamValue('id'));     
                                                 //    change_step(nextStepIndex);
                                                 //    return true;

                                                 //});

                                                      $('#stepwizard').on("showStep", function(e, anchorObject, stepIndex, stepDirection) {
                                                          fixH(stepIndex);
                                                      });

                                       

                                               var TABindexMAX = $("[id*=tab_index_max]").val() != "" ? $("[id*=tab_index_max]").val() : "0";
                                                    //'var disARRAY[];
                                                    let disabledARRAY = new Array();


                                                console.log('Tab index Max ' + TABindexMAX);
                                                                                   
                                                    //disARRAY.forEach(myFunction);
                                                   //function myFunction(value, index, array) {
                                                   //       txt += value + "<br>";
                                                   //     }

                                                    var j = 0;
                                                    for (i = parseInt(TABindexMAX) + 1 ; i < 6; i++) {                                    

                                                        disabledARRAY[j] = i;
                                                        //console.log('disabling the tab ' + disabledARRAY[j]);
                                                        j++;

                                                    }

                                      console.log(JSON.stringify(disabledARRAY) + ' length: ' + disabledARRAY.length);

                                      var ShowButtom_ = true
                                      if (disabledARRAY.length == 4)
                                          ShowButtom_ = false
                                      else
                                          ShowButtom_ = true

                                                    
                                                  if (typeof _Reset !== 'undefined') {

                                                      $('#stepwizard').smartWizard("reset");
                                                      //console.log('reseting wizard');

                                                  }


                                                $('#stepwizard').smartWizard({
                                                  selected: IndexINI,
                                                  theme: 'arrows',
                                                  Justified: true, // Nav menu justification. true/false
                                                  //transitionEffect: 'slide',
                                                  //transitionSpeed: '400',
                                                  autoAdjustHeight: false, // Automatically adjust content height
                                                  transition: {
                                                      animation: 'fade', // Effect on navigation, none/fade/slide-horizontal/slide-vertical/slide-swing
                                                      speed: '400', // Transion animation speed
                                                      easing:'' // Transition animation easing. Not supported without a jQuery easing plugin
                                                  },
                                                  toolbarSettings: {
                                                      toolbarPosition: 'both', // none, top, bottom, both
                                                      toolbarButtonPosition: 'right', // left, right, center
                                                      showNextButton: ShowButtom_, // show/hide a Next button
                                                      showPreviousButton: true, // show/hide a Previous button
                                                      toolbarExtraButtons: [] // Extra buttons to show on toolbar, array of jQuery input/buttons elements
                                                    },
                                                  keyboardSettings: {
                                                          keyNavigation: false, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
                                                          keyLeft: [37], // Left key code
                                                          keyRight: [39] // Right key code
                                                      },
                                                 anchorSettings: {
                                                      anchorClickable: true, // Enable/Disable anchor navigation
                                                      enableAllAnchors: false, // Activates all anchors clickable all times
                                                      markDoneStep: true, // Add done state on navigation
                                                      markAllPreviousStepsAsDone: true, // When a step selected by url hash, all previous steps are marked done
                                                      removeDoneStepOnNavigateBack: false, // While navigate back done step after active step will be cleared
                                                      enableAnchorOnDoneStep: true // Enable/Disable the done steps navigation
                                                      ,disabledSteps: disabledARRAY     
                                                    }
                                                    
                                                  });

                                          } //set_step


           //                                             

                                               function fixH(IdX) {

                                               console.log('Fixing Height for ' + (parseInt(IdX) + 1));

                                                   //$('#stepwizard').smartWizard("fixHeight"); //if you want to delete the defaulted height or~
                                                   $('#step-' + (parseInt(IdX) + 1)).height("100%");

                                              }


                                   function change_step(iDx) {

                                         // console.log('switching to step ' + iDx);

                                              if (iDx == 1) {
                                                    //  console.log('the id: ' + getParamValue('id'));
                                                  $('#stepwizard').smartWizard("loader", "show");                                         
                                                   window.location.href = 'frm_applied_technologyParticipants?id=' + getParamValue('id');       
                                                } else {
                                                    $('#stepwizard').smartWizard("loader", "show");
                                                    window.location.href = 'frm_applied_technologyEdit?id=' + getParamValue('id');       
                                             }

                                     }


                      function getParameterByName(name, url) {
                               if (!url) url = window.location.href;
                               name = name.replace(/[\[\]]/g, '\\$&');
                               var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                                   results = regex.exec(url);
                               if (!results) return null;
                               if (!results[2]) return '';
                               return decodeURIComponent(results[2].replace(/\+/g, ' '));
                           }


                      function getParamValue(paramName) {

                                    var url = window.location.search.substring(1); //get rid of "?" in querystring
                                    var qArray = url.split('&'); //get key-value pairs
                                    for (var i = 0; i < qArray.length; i++) {
                                        var pArr = qArray[i].split('='); //split key and value
                                        if (pArr[0] == paramName)
                                            return pArr[1]; //return value
                                    }

                          }

          
                 function FuncModatTrim() {
                        jQuery('#modalTasaCambio').modal('show');
                    }
                 

       </script>
    
   
</asp:Content>
