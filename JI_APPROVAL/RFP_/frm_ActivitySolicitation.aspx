<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivitySolicitation.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivitySolicitation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="timeline" Src="~/Controles/ctrl_timeline_activity.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <link rel="stylesheet" href="../Content/slider_style.css">
    <script src="<%=ResolveUrl("~/Content/dist/js/Calendar_Script.js?ts=0.0002")%>"></script>
    
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
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activity - Solicitation</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="identity_sol" text="0" runat="server"  CssClass="deleteIdentity" data-id="" Visible="false" />
                         <asp:Label ID="identity_doc" text="0" runat="server" CssClass="deleteIdentity" data-id="" Visible="false" />
                    </h3>
                 </div>
                 <div class="col-sm-1 text-right">   
                     <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp();" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                 </div>
            </div>
            <div class="box-body">

                <div class="col-lg-12">

                    <div class="panel panel-default">
                       <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                          <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" href="#collapseOne"
                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_ActivityTIME">Activity Timeline&nbsp;&nbsp;</a>
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
                                <li role="presentation"><a runat="server" id="alink_definicion" href="#"  class="hidden" >ACTIVITY</a></li>                              
                                <li role="presentation" class="active"><a class="primary"  runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>      
                                <li role="presentation"><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>    
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_sol" runat="server" Text="" Visible="false" />



                        <div class="panel-body div-bordered">      
                                    <br />                       
                                    <div class="form-group row">
                                          <div class="col-sm-2 text-right">
                                             <asp:Label runat="server" ID="lblt_solicitation_type" CssClass="control-label text-bold">Solicitation Type</asp:Label>
                                          </div>
                                          <div class="col-sm-4">
                                             <telerik:RadComboBox ID="cmb_solicitation_type" runat="server" Width="300px" EmptyMessage="Pick one category" AutoPostBack="true" >
                                             </telerik:RadComboBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                                ControlToValidate="cmb_solicitation_type" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="* Required" ValidationGroup="1"></asp:RequiredFieldValidator>

                                          </div>
                                          <div class="col-sm-2 text-right">
                                             <asp:Label runat="server" ID="lblt_solicitation_code" CssClass="control-label text-bold">Solicitation Code</asp:Label>
                                          </div>
                                          <div class="col-sm-4">
                                             
                                               <div class="alert-sm bg-blue text-center" runat="server" id="divCodigo" style="width: 300px;">
                                                   <asp:Label ID="lbl_COde" runat="server" CssClass="text-bold" ></asp:Label>
                                              </div>
                                           
                                           </div>
                                    </div>
                                     <br />
                                  <div class="form-group row hidden">
                                          <div class="col-sm-2 text-right">
                                             <asp:Label runat="server" ID="lblt_Activity_code" CssClass="control-label text-bold">Activity Code</asp:Label>
                                          </div>
                                          <div class="col-sm-4">
                                                <telerik:RadTextBox ID="txt_activity_code" runat="server" Width="80%" MaxLength="250" ReadOnly="true">
                                                </telerik:RadTextBox>
                                           </div>                                    
                                   </div>

                                    <div class="form-group row">
                                          <div class="col-sm-2 text-right">
                                             <asp:Label runat="server" ID="lblt_solicitation_status" CssClass="control-label text-bold">Solicitation Status</asp:Label>
                                          </div>
                                          <div class="col-sm-4">
                                               <telerik:RadComboBox ID="cmb_solicitation_status" runat="server" Width="90%" ></telerik:RadComboBox>
                                           </div>                                     
                                    </div>


                            <%----**********************************************ADD INS**************************************************************--%>

                                       <hr />

                                         <div class="form-group row">
                                           <div class="col-sm-3 text-right">
                                              <asp:Label runat="server" ID="lblt_regionII" CssClass="control-label text-bold">Cluster </asp:Label>
                                            </div>
                                            <div class="col-sm-8">
                                               <telerik:RadComboBox ID="cmb_regionII" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                             <asp:CheckBox runat="server" ID="chk_todosII" Text="Multi-Region" AutoPostBack="true" />
                                             </div>
                                         </div>                                                               

                                         
                                           <div class="form-group row">
                                               <div class="col-sm-3 text-right">
                                                   <asp:Label runat="server" ID="lblt_subregionII_" CssClass="control-label text-bold">Subregión</asp:Label>
                                               </div>
                                               <div class="col-sm-8">

                                                   <%--OnCheckedChanged="ctrl_id_CheckedChanged"  AutoPostBack="true" --%>
                                                   <%--OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true"--%>

                                                   <telerik:RadComboBox ID="cmb_subregionII" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                   <telerik:RadGrid ID="grd_subregionII" runat="server" AutoGenerateColumns="False" Visible="false" Width="500px">
                                                       <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_subregion">
                                                           <Columns>
                                                               <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                   HeaderText="" UniqueName="TemplateColumnAnual">
                                                                   <ItemTemplate>
                                                                       <asp:CheckBox ID="ctrl_id" runat="server" />
                                                                   </ItemTemplate>
                                                                   <HeaderStyle Width="15px" />
                                                               </telerik:GridTemplateColumn>
                                                               <telerik:GridBoundColumn DataField="id_subregion" DataType="System.Int32"
                                                                   FilterControlAltText="Filter id_subregion column" HeaderText="id_subregion"
                                                                   ReadOnly="True" SortExpression="id_subregion" UniqueName="id_subregion"
                                                                   Display="False">
                                                               </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="nombre_region"
                                                                   FilterControlAltText="Filter nombre_region column"
                                                                   HeaderText="Region" SortExpression="nombre_region"
                                                                   UniqueName="colm_nombre_region" ItemStyle-Width="90%">
                                                                    <ItemStyle Width="30%" />
                                                               </telerik:GridBoundColumn>
                                                               <telerik:GridBoundColumn DataField="nombre_subregion"
                                                                   FilterControlAltText="Filter nombre_subregion column"
                                                                   HeaderText="Sub-Region" SortExpression="nombre_subregion"
                                                                   UniqueName="colm_nombre_subregion" ItemStyle-Width="90%">
                                                               </telerik:GridBoundColumn>
                                                               <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column" Visible="false"
                                                                   HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                   <ItemTemplate>
                                                                       <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                           Enabled="false" >
                                                                       </telerik:RadNumericTextBox>
                                                                   </ItemTemplate>
                                                                   <HeaderStyle Width="15px" />
                                                               </telerik:GridTemplateColumn>
                                                           </Columns>
                                                       </MasterTableView>
                                                   </telerik:RadGrid>
                                               </div>
                                           </div>

                                         <div class="form-group row  hidden">
                                             <div class="col-sm-3 text-right">
                                                 <asp:Label runat="server" ID="lblt_trimestre" CssClass="control-label text-bold">Quarter</asp:Label>
                                             </div>
                                             <div class="col-sm-8">
                                                 <telerik:RadComboBox ID="cmb_periodo" runat="server" Width="300px"></telerik:RadComboBox>
                                             </div>
                                         </div>

                                        <div class="form-group row hidden ">
                                            <div class="col-sm-3 text-right">
                                                <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Activity Code</asp:Label>
                                            </div>
                                            <div class="col-sm-9">
                                                <div class="alert-sm bg-blue text-center" runat="server" id="div1" visible="false" style="width: 300px;">
                                                    <asp:Label ID="lbl_mensaje" runat="server" CssClass="text-bold" Visible="False"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        

<%--                            --************************************************************************************************************--%>


                             <div id="dv_activity_info" class="hidden">
                             
                                   <br />
                                     <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:HiddenField runat="server" ID="id_documento" />
                                            <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Activity Name</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_nombreproyecto" runat="server" Rows="6" TextMode="MultiLine" Resize="Both" Width="85%" ReadOnly="true">
                                            </telerik:RadTextBox>
                                       </div>
                                    </div>
                                    <br />
                                   <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Activity Description</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="6" TextMode="MultiLine" Resize="Both" Width="85%" MaxLength="5000" ReadOnly="true">
                                            </telerik:RadTextBox>                                     
                                        </div>
                                    </div>

                                 </div>
                                    <hr />
                                      <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_Tittle" CssClass="control-label text-bold">Title</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_tittle" runat="server" Rows="6" TextMode="MultiLine" Width="85%" Resize="Both" MaxLength="1000">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="txt_tittle" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="* Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </div>
                                     </div>
                                      <br />
                                      <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_purpose" CssClass="control-label text-bold">Purpose</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_purpose" runat="server" Rows="6" TextMode="MultiLine" Width="85%" Resize="Both" MaxLength="1000">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="txt_purpose" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="* Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </div>
                                     </div>

                                     <br />
                                      <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_solicitation_content" CssClass="control-label text-bold">Solicitation</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_solicitation" runat="server" Rows="6" TextMode="MultiLine" Width="85%" Resize="Both" MaxLength="5000">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="txt_solicitation" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </div>
                                     </div>

                                       <div class="form-group row">
                                          <div class="col-sm-12">
                                             <hr />                                    
                                          </div> 
                                        </div>

                                      <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_modifications" CssClass="control-label text-bold">Solicitation Modifications</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_modifications" runat="server" Rows="6" TextMode="MultiLine" Width="85%" Resize="Both" MaxLength="5000">
                                            </telerik:RadTextBox>                                          
                                        </div>
                                     </div>

                                    <br />
                                      <div class="form-group row">
                                            <div class="col-sm-2 text-right">
                                                <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Issuance Date</asp:Label>
                                            </div>
                                            <div class="col-sm-2">

<%--                                               <telerik:RadTimePicker RenderMode="Lightweight" ID="RadTimePicker1" runat="server" Skin="Default" DateInput-DateFormat="HH:mm">
                                                    <TimeView Skin="Default"
                                                        ShowHeader="False"
                                                        StartTime="08:00:00"
                                                        Interval="00:15:00"
                                                        EndTime="18:00:00"
                                                        Columns="4">
                                                    </TimeView>
                                                </telerik:RadTimePicker> --%>

                                                <%--<telerik:RadDatePicker ID="dt_fecha_inicio" runat="server" AutoPostBack="true"></telerik:RadDatePicker>--%>
                                                <input type="text" value="" id="Eval_StartDate" />
                                                <asp:HiddenField ID="HEval_StartDate" runat="server" Value="" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                                    ControlToValidate="dt_fecha_inicio" ErrorMessage="*"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>--%>
                                                 <%--<asp:TextBox ID="Eval_StartDate" runat="server" placeholder="Select a Date"></asp:TextBox>--%>
                                                

                                            </div>
                                             <div class="col-sm-2 text-right">
                                                <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">Close Date</asp:Label>
                                            </div>
                                            <div class="col-sm-6">
                                               <%-- <telerik:RadDatePicker ID="dt_fecha_fin" runat="server"></telerik:RadDatePicker>  
                                                <asp:Label runat="server" ID="lblt_hours" CssClass="control-label text-bold">Hr &nbsp;</asp:Label>
                                                <telerik:RadNumericTextBox ID="txt_hour" runat="server" EmptyMessage="7"
                                                                                MaxValue="23" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px" >
                                                                                <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                                            </telerik:RadNumericTextBox>
                                                <asp:Label runat="server" ID="lblt_min" CssClass="control-label text-bold">Min&nbsp;</asp:Label>
                                                <telerik:RadNumericTextBox ID="txt_min" runat="server" EmptyMessage="30"
                                                                                MaxValue="59" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px" >
                                                                                <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                                            </telerik:RadNumericTextBox>--%>
                                                                                                 
                                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Error"
                                                    ControlToValidate="dt_fecha_fin" ErrorMessage="*"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>--%>                                                
                                                  <input type="text" value="" id="Eval_EndDate" />
                                                  <asp:HiddenField ID="HEval_EndDate" runat="server" />
                                                  <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Error"
                                                    ControlToValidate="HEval_EndDate" ErrorMessage="Required"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>--%>
                                                 
                                            </div>
                                    </div>

                                   <br />
                                  <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_persona_encargada" CssClass="control-label text-bold">Solicitation Lead</asp:Label>
                                        </div>
                                        <div class="col-sm-2">
                                            <telerik:RadComboBox ID="cmb_persona_encargada" runat="server" Width="500px">
                                            </telerik:RadComboBox>
                                        </div>
                                 </div>

                                 <br />
                                   <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_email_to" CssClass="control-label text-bold">Emailing TO (;)</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_email_to" runat="server" Rows="6" TextMode="MultiLine" Resize="Both" Width="55%" MaxLength="250" >
                                            </telerik:RadTextBox>                                     
                                        </div>
                                    </div>

                            <br />
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-right">
                                            <asp:Label runat="server" ID="lblt_email_cc" CssClass="control-label text-bold">Emailing CC (;)</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadTextBox ID="txt_email_CC" runat="server" Rows="6" TextMode="MultiLine" Resize="Both" Width="55%" MaxLength="250" >
                                            </telerik:RadTextBox>                                     
                                        </div>
                                    </div>  
                            
                            <br />

                                  <div class="form-group row">
                                       <div class="col-sm-1 text-right">
                                          <%-- <asp:Label runat="server" ID="lblt_time_closed" CssClass="control-label text-bold">Time Closed</asp:Label>--%>
                                        </div>
                                 
                                        <div class="col-sm-4">
                                            <br />
                                              <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false"  CssClass="btn btn-sm pull-right">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="btn_continue" runat="server" Text="Save Solicitation" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                    Width="100px" ValidationGroup="1">
                                                </telerik:RadButton>
                                        </div>

                                        <div class="col-sm-7 text-right">
                                           <asp:Label runat="server" ID="lblt_error" CssClass="control-label text-bold" ForeColor="Red"></asp:Label>
                                        </div>

                                  </div>
                                                      
                                 
                                     <hr />
                                  
                              <div id="ADDons" class="box-body" style="border:1px solid thick;">
                                  <p id="_Documents">&nbsp;</p>

                                <div class="box-header"  id="Tabs_options">
                                   <ul class="nav nav-tabs">
                                     <li class="active"><a data-toggle="tab" href="#Documents"><asp:Label runat="server" ID="lblt_Documents" CssClass="control-label text-bold">Solicitation Documents</asp:Label></a></li>                                    
                                     <li><a data-toggle="tab" href="#Applicants"><asp:Label runat="server" ID="lblt_Applicants" CssClass="control-label text-bold">Applicants</asp:Label></a></li>
                                     <li><a data-toggle="tab" href="#MAT_REQU"><asp:Label runat="server" ID="lblt_material" CssClass="control-label text-bold">Materials Required</asp:Label></a></li>
                                     <li><a data-toggle="tab" href="#PRESCREE"><asp:Label runat="server" ID="lblt_prescreening" CssClass="control-label text-bold">PreScreening Setup</asp:Label></a></li>
                                     <li><a data-toggle="tab" href="#Eval_Team"><asp:Label runat="server" ID="lblt_Evaluation_team" CssClass="control-label text-bold">Evaluation Setup</asp:Label></a></li>
                                  </ul>
                                </div>

                                   <asp:HiddenField ID="TabName" runat="server" Value="Documents" />
                                    <div class="tab-content">

                                          <div id="Documents" class="tab-pane fade in active">
                                              <br />
                                               <div class="form-group row">
                                                    <div class="col-sm-2 text-right">
                                                       <asp:Label runat="server" ID="lblt_documente_tittle" CssClass="control-label text-bold">Document Title</asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                       <telerik:RadTextBox ID="txt_document_tittle" runat="server" Width ="80%" MaxLength="200">
                                                       </telerik:RadTextBox>
                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                                ControlToValidate="txt_document_tittle" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Required" ValidationGroup="2">*</asp:RequiredFieldValidator>


                                                    </div>

                                                    <div class="col-sm-2 text-right">
                                                           <asp:Label runat="server" ID="lblt_Type_document" CssClass="control-label text-bold">Document Type</asp:Label>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <telerik:RadComboBox ID="cmb_type_of_document" runat="server" Width="300px" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox>
                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                                ControlToValidate="cmb_type_of_document" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                         </div>
                                                </div>
                                               <br />
                                                <div class="form-group row">
                                                                                                          
                                                            <div class="col-sm-2 text-right">
                                                                 <asp:Label runat="server" ID="lblt_documento" CssClass="control-label text-bold">Attach Document</asp:Label>
                                                            </div>

                                                            <div class="col-sm-4">
                                                                  <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                                                    TemporaryFolder  ="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded" />
                                                            </div>  
                                                    
                                                                    <div class="col-sm-2">
                                                                     </div>

                                                                       <div class="col-sm-4">
                                                                                <telerik:RadButton ID="btn_agregar" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                                                                    Text="Add Document" ValidationGroup="2" Width="100px">
                                                                                </telerik:RadButton><br />
                                                                        </div>   
                                                                                                           
                                                           </div>
                                                          
                                                            <hr /> 

                                                              <div class="form-group row">

                                                                  <div class="col-sm-11">

                                                                            <asp:UpdatePanel ID="PanelFirma" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <telerik:RadGrid ID="grd_archivos" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_ACTIVITY_ANNEX">
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column"
                                                                                                    HeaderButtonType="PushButton" UniqueName="ImageDownload">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Adjunto" />
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
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    <br />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                        
                                                                      </div>

                                                              </div>
                                              
                                              </div> <%-- id="Documents" --%>


                                        <%--****************************************** APPLICANTS**********************************--%>

                                            <div id="Applicants" class="tab-pane fade" >

                                                       <style type="text/css">
                                                                                                                 
                                                             .wrapWord { 
                                                                    word-wrap: break-word;
                                                                    word-break:break-all; 
                                                                  }

                                                            .rgDataDiv {
                                                                height:auto !important;                                                           
                                                            }
                    
                                                     </style>

                                                    <br />
                                                    <div class="form-group row">

                                                          <div class="col-sm-10">
                                                             <%--  <telerik:RadTextBox ID="txt_doc" runat="server" Visible="false"
                                                                        EmptyMessage="Add keyword..." LabelWidth="" Width="90%"
                                                                        >
                                                                        <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                                               </telerik:RadTextBox>--%>
                                                               <telerik:RadSearchBox RenderMode="Lightweight"
                                                                runat="server" ID="applicants"
                                                                CssClass="searchBox" 
                                                                Skin="Silk"
                                                                Width="90%" 
                                                                DropDownSettings-Height="250px"
                                                                DropDownSettings-Width="600px"
                                                                Height="23px"
                                                                DataTextField="Org_search"
                                                                DataSourceID="SqlDataSource2"
                                                                DataValueField="ID_ORGANIZATION_APP"
                                                                DataKeyNames=" ID_ORGANIZATION_APP, organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME "
                                                                EmptyMessage="Type keyword to search organizations"
                                                                Filter="StartsWith"
                                                                MaxResultCount="20"
                                                                OnDataSourceSelect="Organization_DataSourceSelect"
                                                                OnSearch="Organization_Search">
                                                            </telerik:RadSearchBox>
                                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"></asp:SqlDataSource>

                                                       
                                                           </div>

                                                          <div class="col-sm-2">
                                                                    <%--<telerik:RadButton ID="btn_buscar" runat="server"  AutoPostBack="true" Text="Buscar" Width="100px">
                                                                    </telerik:RadButton>--%>
                                                            </div>
                                                      </div>
                                                       <br />
                                                        <div class="form-group row">                                                        

                                                                  <div class="col-sm-12">

                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>


                                                                                     <telerik:RadGrid ID="grd_cate" runat="server" 
                                                                                         AllowAutomaticDeletes="True"                                                                                         
                                                                                         CellSpacing="0" 
                                                                                         GridLines="None" 
                                                                                         PageSize="15" 
                                                                                         AllowPaging="true" 
                                                                                         Width="100%"
                                                                                         AllowSorting="True" 
                                                                                         AllowFilteringByColumn="true" 
                                                                                         AutoGenerateColumns="False">
                                                                                            
                                                                                         <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />                                        
                                                                                            <Scrolling AllowScroll="True" SaveScrollPosition="True" />
                                                                                         </ClientSettings>
                                                                                         <GroupingSettings CaseSensitive="false" />
                                                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_ORGANIZATION_APP" AllowAutomaticUpdates="True">

                                                                                                <Columns>
                                                                                                    <telerik:GridBoundColumn DataField="ID_ORGANIZATION_APP"
                                                                                                        FilterControlAltText="Filter ID_ORGANIZATION_APP column"
                                                                                                        SortExpression="ID_ORGANIZATION_APP" UniqueName="ID_ORGANIZATION_APP"
                                                                                                        Visible="False" DataType="System.Int32" HeaderText="ID_ORGANIZATION_APP"
                                                                                                        ReadOnly="True">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                     <telerik:GridBoundColumn DataField="modificated"
                                                                                                        FilterControlAltText="Filter modificated column"
                                                                                                        SortExpression="modificated" UniqueName="modificated"
                                                                                                        Visible="true" Display="false"  HeaderText="modificated"
                                                                                                        ReadOnly="True">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ID_SOLICITATION_APP"
                                                                                                        UniqueName="ID_SOLICITATION_APP"
                                                                                                        Visible="false" Display="true" HeaderText="ID_SOLICITATION_APP" >
                                                                                                    </telerik:GridBoundColumn>
                                                                                                           <telerik:GridBoundColumn DataField="ID_ACTIVITY_SOLICITATION"
                                                                                                        UniqueName="ID_ACTIVITY_SOLICITATION"
                                                                                                        Visible="False" DataType="System.Int32" HeaderText="ID_ACTIVITY_SOLICITATION"
                                                                                                        ReadOnly="True">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ID_APP_STATUS"
                                                                                                        UniqueName="ID_APP_STATUS"
                                                                                                        Visible="False" DataType="System.Int32" HeaderText="ID_APP_STATUS"
                                                                                                        ReadOnly="True">
                                                                                                    </telerik:GridBoundColumn>                                                                                                    

                                                                                                    <telerik:GridTemplateColumn UniqueName="colm_delete" Visible="true">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="col_hlk_delete" runat="server" Width="10"
                                                                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                                                                                                    OnClick="Elimina_Elemento">
                                                                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                                                                </asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="30px" CssClass="wrapWord"  />
                                                                                                            <HeaderStyle Width="30px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                    <telerik:GridTemplateColumn UniqueName="colm_Edit" Visible="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar ejecutor" Target="_self" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="5px" />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                    <telerik:GridTemplateColumn UniqueName="print" Visible="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir detalles" Target="_blank" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="5px" />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                    <telerik:GridBoundColumn DataField="SOLICITATION_APP_CODE"
                                                                                                        FilterControlAltText="Filter SOLICITATION_APP_CODE column"
                                                                                                        HeaderText="APP_CODE" SortExpression="SOLICITATION_APP_CODE"
                                                                                                        UniqueName="colm_SOLICITATION_APP_CODE">
                                                                                                         <ItemStyle Width="175px" CssClass="wrapWord"  />
                                                                                                         <HeaderStyle Width="175px" />
                                                                                                    </telerik:GridBoundColumn>
                                                                                                     <telerik:GridBoundColumn DataField="NAMEALIAS"
                                                                                                        FilterControlAltText="Filter NAMEALIAS column"
                                                                                                        HeaderText="Acronyms" SortExpression="NAMEALIAS"
                                                                                                        UniqueName="colm_NAMEALIAS">
                                                                                                          <ItemStyle Width="150px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="150px" />
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ORGANIZATIONNAME"
                                                                                                        FilterControlAltText="Filter ORGANIZATIONNAME column"
                                                                                                        HeaderText="Name" SortExpression="ORGANIZATIONNAME"
                                                                                                        UniqueName="colm_ORGANIZATIONNAME">
                                                                                                         <ItemStyle Width="200px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="200px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                    <telerik:GridBoundColumn DataField="PERSONNAME"
                                                                                                        FilterControlAltText="Filter PERSONNAME column" HeaderText="Representative"
                                                                                                        SortExpression="PERSONNAME"
                                                                                                        UniqueName="colm_PERSONNAME">
                                                                                                         <ItemStyle Width="150px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="150px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                     <telerik:GridBoundColumn DataField="ORGANIZATIONEMAIL"
                                                                                                        FilterControlAltText="Filter ORGANIZATIONEMAIL column" HeaderText="Email"
                                                                                                        SortExpression="ORGANIZATIONEMAIL"
                                                                                                        UniqueName="colm_ORGANIZATIONEMAIL">
                                                                                                          <ItemStyle Width="150px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="150px" />
                                                                                                    </telerik:GridBoundColumn>


                                                                                                     <telerik:GridBoundColumn DataField="ORGANIZATION_TYPE"
                                                                                                        FilterControlAltText="Filter ORGANIZATION_TYPE column" HeaderText="ORG TYPE"
                                                                                                        SortExpression="ORGANIZATION_TYPE"
                                                                                                        UniqueName="colm_ORGANIZATION_TYPE">
                                                                                                          <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="100px" />
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ADDRESSCOUNTRYREGIONID"
                                                                                                        FilterControlAltText="Filter ADDRESSCOUNTRYREGIONID column"
                                                                                                        HeaderText="Country" SortExpression="COUNTRY"
                                                                                                        UniqueName="colm_ADDRESSCOUNTRYREGIONID">
                                                                                                         <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="100px" />
                                                                                                    </telerik:GridBoundColumn>
                                                                                                      <telerik:GridBoundColumn DataField="ADDRESSCITY"
                                                                                                        FilterControlAltText="Filter ADDRESSCITY column"
                                                                                                        HeaderText="City" SortExpression="ADDRESSCITY"
                                                                                                        UniqueName="colm_ADDRESSCITY">
                                                                                                           <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="100px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                    <telerik:GridBoundColumn DataField="APLICATION_STATUS"
                                                                                                        FilterControlAltText="Filter APLICATION_STATUS column" HeaderText="STATUS"
                                                                                                        SortExpression="APLICATION_STATUS" UniqueName="colm_APLICATION_STATUS" Visible="true">
                                                                                                          <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                          <HeaderStyle Width="100px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                    <telerik:GridTemplateColumn 
                                                                                                        UniqueName="colm_solicitate" Visible="true" HeaderText="Invite">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True" ToolTip ="Send invitation"
                                                                                                                OnCheckedChanged="SOLITICITATE_APP" />
                                                                                                            <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                                                                                                CheckedImageUrl="../Imagenes/iconos/resend_email.png" ImageHeight="22" ImageWidth="22"
                                                                                                                TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/warning.gif">
                                                                                                            </cc1:ToggleButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                         <ItemStyle Width="75px" CssClass="wrapWord"  />
                                                                                                         <HeaderStyle Width="75px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                     <telerik:GridTemplateColumn 
                                                                                                        UniqueName="colm_modification" Visible="true" HeaderText="Mod">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkActivo_mod" runat="server" AutoPostBack="True" ToolTip ="Sent modification"
                                                                                                                OnCheckedChanged="SOLICITATE_MOD" />
                                                                                                            <cc1:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server"
                                                                                                                CheckedImageUrl="../Imagenes/iconos/resend_email.png" ImageHeight="22" ImageWidth="22"
                                                                                                                TargetControlID="chkActivo_mod" UncheckedImageUrl="../Imagenes/iconos/warning.gif">
                                                                                                            </cc1:ToggleButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                            <ItemStyle Width="75px" CssClass="wrapWord"  />
                                                                                                            <HeaderStyle Width="75px" />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                
                                                                                                </Columns>
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                                                     <br />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                        
                                                                      </div>

                                                              </div>


                                            </div>   <%--id="Applicants"--%>

                                        <%--****************************************** APPLICANTS**********************************--%>


                                        <%--****************************************** MAT REQU**********************************--%>

                                          <div id="MAT_REQU" class="tab-pane fade" ><%--id="Materials"--%>

                                                      <br />
                                                       <div class="form-group row">
                                                           
                                                           <div class="col-sm-2 text-right">
                                                               <asp:Label runat="server" ID="lblt_Material_Title" CssClass="control-label text-bold">Document Title</asp:Label>
                                                            </div>
                                                            <div class="col-sm-4">
                                                               <telerik:RadTextBox ID="txt_Material_Title" runat="server" Width ="80%" TextMode="MultiLine" Rows="3" MaxLength="500">
                                                               </telerik:RadTextBox><br />
                                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                                ControlToValidate="txt_Material_Title" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Required" ValidationGroup="3">** Required</asp:RequiredFieldValidator>

                                                            </div>

                                                             <div class="col-sm-2 text-right">
                                                                <asp:Label runat="server" ID="lblt_Material_type" CssClass="control-label text-bold">Document Type</asp:Label>
                                                             </div>
                                                             <div class="col-sm-4">
                                                                 <telerik:RadComboBox ID="cmb_Material_type" runat="server" Width="300px" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox><br />
                                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                                ControlToValidate="cmb_Material_type" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Required" ValidationGroup="3">** Required</asp:RequiredFieldValidator>
                                                              </div>
                                                           </div>

                                                           
                                                         <br />
                                                          <div class="form-group row">
                                                               <div class="col-sm-2 text-right">
                                                                  <br /> <asp:Label runat="server" ID="lblt_must_be" CssClass="control-label text-bold">Mandatory Option:</asp:Label>
                                                                </div>
                                                                <div class="col-sm-1 text-left">   
                                                                    <br />
                                                                    <label class="switch">
                                                                      <input id="chk_data_in" runat="server" type="checkbox" onchange="MUST_input()">
                                                                      <span class="slider round"></span>
                                                                    </label>                                              
                                                                </div> 
                                                                <div class="col-sm-3 text-left" >                                        
                                                                   <h3><span id="Mandatory_value" class="label label-info"></span></h3>      
                                                                </div>
                                                              
                                                               <div class="col-sm-4">
                                                                  
                                                                   <asp:HiddenField ID="opt_mandatory" runat="server" Value="MANDATORY" />
                                                                   <asp:HiddenField ID="opt_optional" runat="server" Value="OPTIONAL" />
                                                                  <br />
                                                                  <telerik:RadButton ID="btn_add_material" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                                                                    Text="Add Required document" ValidationGroup="3" Width="60%">
                                                                  </telerik:RadButton><br />
                                                               </div>   

                                                          </div>
                                                                                                   
                                                          <hr /> 
                                              
                                                                                                                 
                                                           <div class="form-group row">

                                                                  <div class="col-sm-12">
                                                                          <asp:HiddenField ID="idSOLmaterial" runat="server" Value="0" />
                                                                               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_materials" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_SOLICITATION_MATERIAL">
                                                                                            <Columns>
                                                                                               <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                                                    ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                    ConfirmDialogWidth="400px"
                                                                                                    ConfirmText="would you like to delete this requiered material?"
                                                                                                    ConfirmTitle="Delete file" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                    UniqueName="Eliminar" />
                                                                                                  
                                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                                                        UniqueName="colm_select" Visible="true">
                                                                                                        <ItemTemplate>                                                                                                            
                                                                                                            <asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="True"
                                                                                                                OnCheckedChanged="Edit_Entry" />
                                                                                                            <cc1:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server"
                                                                                                                CheckedImageUrl="../Imagenes/iconos/Informacion2.gif" ImageHeight="22" ImageWidth="22"
                                                                                                                TargetControlID="chkSelected" UncheckedImageUrl="../Imagenes/iconos/b_edit.png">
                                                                                                            </cc1:ToggleButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle Width="30px" />
                                                                                                        <ItemStyle Width="30px" />
                                                                                                    </telerik:GridTemplateColumn>
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
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    
                                                                                   </ContentTemplate>                                                                                
                                                                             </asp:UpdatePanel>

                                                                         <br />
                                                                           
                                                                        
                                                                      </div>

                                                              </div>


                                         </div>
                                          
                                       <%--****************************************** MAT REQU**********************************--%>


                                         <%--******************************************PRESCREENING SETTUP**********************************--%>
                                        <div id="PRESCREE" class="tab-pane fade" ><%--id="PRESCREE"--%>

                                                <div class="form-group row">  
                                                    <br />
                                                       <div class="col-sm-3 text-right">
                                                           <asp:Label runat="server" ID="lblt_preescreening" CssClass="control-label text-bold">PreScreening Assesment:</asp:Label>
                                                       </div>
                                                       <div class="col-sm-6 text-left">   
                                                              <telerik:RadComboBox ID="cmb_prescreening"
                                                                    Runat="server" 
                                                                    CausesValidation="False"                                                                                                                                                                           
                                                                    EmptyMessage="Select a preescreening..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains" 
                                                                    Width="95%" 
                                                                    Height="200px">
                                                                </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="cmb_prescreening" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Required" ValidationGroup="94">*</asp:RequiredFieldValidator>


                                                       </div>

                                                        
                                                       <div class="col-sm-3">
                                                            <asp:LinkButton runat="server" ID="btn_add_prescreening"  AutoPostBack="True" SingleClick="true"  Text="Rounds" Width="70%" class="btn btn-info btn-sm margin-r-5 pull-left" ValidationGroup="94"   ><i class="fa fa- fa-plug-circle-plus"></i>&nbsp;&nbsp;Add PreScreening</asp:LinkButton>            
                                                            <br /><br />
                                                       </div>
                                                   </div>



                                                    
							 <div class="form-group row">
                                                                 <br />
                                                                  <div class="col-sm-12" style="padding-left:30px;">                                                                          
                                                                               <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_Prescreening" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_SOLICITATION_SCREENING">
                                                                                            <Columns>
                                                                                               <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                                                    ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                    ConfirmDialogWidth="400px"
                                                                                                    ConfirmText="would you like to delete the PreeScreening?"
                                                                                                    ConfirmTitle="Delete Memeber" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                    UniqueName="Eliminar" />
                                                                                                                                                                                                     
                                                                                                  <telerik:GridBoundColumn DataField="SCREENING_NAME"
                                                                                                    FilterControlAltText="Filter SCREENING_NAME column"
                                                                                                    HeaderText="Member" SortExpression="SCREENING_NAME"
                                                                                                    UniqueName="colm_SCREENING_NAME">
                                                                                                </telerik:GridBoundColumn>
                                                                                                           
                                                                                                 <telerik:GridBoundColumn DataField="SCREENING_TOTALPOINT" 
                                                                                                        FilterControlAltText="Filter SCREENING_TOTALPOINT column" HeaderText="TOT SCORE" 
                                                                                                        UniqueName="colm_SCREENING_TOTALPOINT" >                                        
                                                                                                 </telerik:GridBoundColumn>

											                                                   <telerik:GridBoundColumn DataField="SCREENING_TOTALPASS" 
                                                                                                        FilterControlAltText="Filter SCREENING_TOTALPASS column" HeaderText="MINIMAL SCORE" 
                                                                                                        UniqueName="colm_CREENING_TOTALPASS" >                                        
                                                                                                 </telerik:GridBoundColumn>

                                                                                              
                                                                                                  <telerik:GridBoundColumn DataField="survey_name"
                                                                                                    FilterControlAltText="Filter survey_name column"
                                                                                                    HeaderText="SOURVEY" SortExpression="survey_name"
                                                                                                    UniqueName="colm_survey_name">
                                                                                                </telerik:GridBoundColumn>

                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    
                                                                                   </ContentTemplate>                                                                                
                                                                             </asp:UpdatePanel>

                                                                         <br />                                                                     
                                                                        
                                                                      </div>

                                                         </div>


                                        </div>
                                       <%--******************************************PRESCREENING SETTUP**********************************--%>



                                        <%--******************************************EVALUATION SETTUP**********************************--%>
                                          <div id="Eval_Team" class="tab-pane fade" ><%--id="Eval_Team"--%>
                                                  
                                                 <div class="form-group row">
                                                        <br /><br />
                                                        <div class="col-sm-2 text-right">
                                                            <asp:Label runat="server" ID="lblt_guidelines" CssClass="control-label text-bold">Guide Line</asp:Label>
                                                        </div>
                                                        <div class="col-sm-10">
                                                            <telerik:RadTextBox ID="txt_guidLines" runat="server" Rows="5" TextMode="MultiLine" Width="85%" Resize="Both" MaxLength="3000">
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                ControlToValidate="txt_purpose" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Required" ValidationGroup="11">*</asp:RequiredFieldValidator>
                                                        </div>
                                                 </div>

                                               <div class="form-group row">
                                                  <br />
                                                  <div class="col-sm-2 text-right">
                                                     <asp:Label runat="server" ID="lblt_eval_start_date" CssClass="control-label text-bold">Start Date</asp:Label>
                                                 </div>
                                        
                                                  <div class="col-sm-2">    
                                                        <input type="text" value="" id="EvalSETT_StartDate" />
                                                        <asp:HiddenField ID="HEvalSETT_StartDate" runat="server" />   
                                                  </div>

                                                   <div class="col-sm-2 text-right">
                                                     <asp:Label runat="server" ID="lblt_eval_End_date" CssClass="control-label text-bold">End Date</asp:Label>
                                                   </div>
                                                  
                                                  <div class="col-sm-2">
                                                       <input type="text" value="" id="EvalSETT_EndDate" />
                                                       <asp:HiddenField ID="HEvalSETT_EndDate" runat="server" />
                                                  </div>        
                                                   
                                                  <div class="col-sm-3">
                                                    <asp:LinkButton ID="btn_save_eval" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="90%" class="btn btn-info btn-sm margin-r-5 pull-right" data-toggle="Export" ValidationGroup="11" ><i class="fa fa-save"></i>&nbsp;&nbsp;Save Evaluation</asp:LinkButton>        
                                                   <br /><br />
                                                  </div>        
                                              
                                                     

                                              </div>

                                              <div class="row">
                                                <div class="form-group col-sm-12">     
                                                   <hr style="border-color:#000000" />  
                                                </div>
                                              </div>
                                             
                                              <div  id="EVAL_2" runat="server" style="display:none;" class="bg-gray-light">

                                                   <br />
                                                     
                                                    <div class="form-group row">  
                                                       <div class="col-sm-2 text-right">
                                                           <asp:Label runat="server" ID="lblt_team_member" CssClass="control-label text-bold">Evaluation Team:</asp:Label>
                                                       </div>
                                                       <div class="col-sm-6 text-left">   
                                                              <telerik:RadComboBox ID="cmb_rol"
                                                                    Runat="server" 
                                                                    CausesValidation="False"                                                                                                                                                                           
                                                                    EmptyMessage="Select a team member..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains" 
                                                                    Width="95%" 
                                                                    Height="200px">
                                                                </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                    ControlToValidate="cmb_rol" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Required" ValidationGroup="4">*</asp:RequiredFieldValidator>


                                                       </div>

                                                        
                                                        <div class="col-sm-3">
                                                            <asp:LinkButton runat="server" ID="btn_add_memebers"  AutoPostBack="True" SingleClick="true"  Text="Rounds" Width="70%" class="btn btn-info btn-sm margin-r-5 pull-left" ValidationGroup="4"><i class="fa fa-user-plus"></i>&nbsp;&nbsp;Add Member</asp:LinkButton>            
                                                            <br /><br />
                                                       </div>
                                                   </div>

                                                   
                                                     <div class="form-group row">
                                                                 
                                                                  <div class="col-sm-12" style="padding-left:30px;">                                                                          
                                                                               <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_team" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_SOLICITATION_EVALUATION_TEAM">
                                                                                            <Columns>
                                                                                               <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                                                    ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                    ConfirmDialogWidth="400px"
                                                                                                    ConfirmText="would you like to delete the evaluation member?"
                                                                                                    ConfirmTitle="Delete Memeber" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                    UniqueName="Eliminar" />
                                                                                                                                                                                                     
                                                                                                  <telerik:GridBoundColumn DataField="nombre_usuario"
                                                                                                    FilterControlAltText="Filter nombre_usuario column"
                                                                                                    HeaderText="Member" SortExpression="nombre_usuario"
                                                                                                    UniqueName="colm_nombre_usuario">
                                                                                                </telerik:GridBoundColumn>
                                                                                                           
                                                                                                 <telerik:GridBoundColumn DataField="EVALUATION_ROLE" 
                                                                                                        FilterControlAltText="Filter EVALUATION_ROLE column" HeaderText="" 
                                                                                                        UniqueName="colm_EVALUATION_ROLE" Visible="true" Display="false">                                        
                                                                                                 </telerik:GridBoundColumn>

                                                                                                 <telerik:GridTemplateColumn 
                                                                                                      FilterControlAltText="Filter colm_role column"  HeaderText="Role" 
                                                                                                      UniqueName="colm_mandatory" >                                      
                                                                                                     <ItemTemplate>                                       
                                                                                                           <telerik:RadTextBox runat="server" ID="txtROLE" Width="100%" Text=""></telerik:RadTextBox>                                 
                                                                                                     </ItemTemplate>
                                                                                                      <ItemStyle Width="35%"  />
                                                                                                   </telerik:GridTemplateColumn>

                                                                                                  <telerik:GridBoundColumn DataField="email_usuario"
                                                                                                    FilterControlAltText="Filter email_usuario column"
                                                                                                    HeaderText="email" SortExpression="email_usuario"
                                                                                                    UniqueName="colm_email_usuario">
                                                                                                </telerik:GridBoundColumn>

                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    
                                                                                   </ContentTemplate>                                                                                
                                                                             </asp:UpdatePanel>

                                                                         <br />                                                                     
                                                                        
                                                                      </div>

                                                    </div>
                                                    
 


                                                   <div id="rounds_control" runat="server" class="form-group row">

                                                         <hr style="border-color:#000000" />
                                                         <div class="col-sm-2 text-right">
                                                             <asp:Label runat="server" ID="lblt_rounds" CssClass="control-label text-bold">Evaluation Rounds</asp:Label>
                                                         </div>
                                                         <div class="col-sm-1">
                                                            <telerik:RadNumericTextBox ID="txt_rounds" runat="server" EmptyMessage="1"
                                                                                MaxValue="100" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px" ReadOnly="true" Value="0" >
                                                                                <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                             </telerik:RadNumericTextBox>
                                                         </div>
                                                       <div class="col-sm-2">
                                                          <%--OnClick ="Javascript:FuncModal_rounds();"--%>
                                                           <asp:LinkButton runat="server" ID="btn_add_round"  AutoPostBack="True" SingleClick="true"  Text="Rounds" Width="90%" class="btn btn-info btn-sm margin-r-5 pull-left"   ><i class="fa fa-plus"></i>&nbsp;&nbsp;Add Round</asp:LinkButton>            
                                                       </div>                                

                                                   </div>
                                                             
                                                   <div class="form-group row">
                                                       <br />
                                                       <div class="col-sm-12" style="padding-left:30px;">                                                                          
                                                                               <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_rounds" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="99%">
                                                                                         <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />                                        
                                                                                            <Scrolling AllowScroll="True" SaveScrollPosition="True" />
                                                                                         </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_EVALUATION_ROUND">
                                                                                            <Columns>

                                                                                               <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                                                    ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                                                    ConfirmDialogWidth="400px"
                                                                                                    ConfirmText="would you like to delete the round?"
                                                                                                    ConfirmTitle="Delete round" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                                                    UniqueName="Delete_Round" >
                                                                                                   <ItemStyle Width="30px" CssClass="wrapWord"  />
                                                                                                   <HeaderStyle Width="30px" />
                                                                                               </telerik:GridButtonColumn>
                                                                                                   
                                                                                                  <telerik:GridBoundColumn DataField="ID_ROUND"
                                                                                                    FilterControlAltText="Filter ID_ROUND column"
                                                                                                    HeaderText="ROUND" SortExpression="ID_ROUND"
                                                                                                    UniqueName="colm_ID_ROUND">
                                                                                                   <ItemStyle Width="20px" CssClass="wrapWord"  />
                                                                                                   <HeaderStyle Width="20px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                  <telerik:GridBoundColumn DataField="ID_VOTING_TYPE" 
                                                                                                        FilterControlAltText="Filter ID_VOTING_TYPE column" HeaderText="" 
                                                                                                        UniqueName="colm_ID_VOTING_TYPE" Visible="true" Display="false">                                        
                                                                                                 </telerik:GridBoundColumn>
                                                                                                           
                                                                                                 <telerik:GridBoundColumn DataField="VOTING_TYPE" 
                                                                                                        FilterControlAltText="Filter VOTING_TYPE column" HeaderText="EVALUATION TYPE" 
                                                                                                        UniqueName="colm_VOTING_TYPE" Visible="true" >                                        
                                                                                                   <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                   <HeaderStyle Width="100px" />
                                                                                                 </telerik:GridBoundColumn>

                                                                                                 <telerik:GridBoundColumn DataField="ROUND_START_DATE"
                                                                                                    FilterControlAltText="Filter ROUND_START_DATE column"
                                                                                                    HeaderText="START DATE" SortExpression="ROUND_START_DATE"
                                                                                                    UniqueName="colm_ROUND_START_DATE">
                                                                                                      <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                      <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                 <telerik:GridBoundColumn DataField="ROUND_END_DATE"
                                                                                                    FilterControlAltText="Filter ROUND_END_DATE column"
                                                                                                    HeaderText="END DATE" SortExpression="ROUND_END_DATE"
                                                                                                    UniqueName="colm_ROUND_END_DATE">
                                                                                                      <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                      <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="TOT_APP_SELECTED"
                                                                                                    FilterControlAltText="Filter TOT_APP_SELECTED column"
                                                                                                    HeaderText="TOTAL TO SELECTING" SortExpression="TOT_APP_SELECTED"
                                                                                                    UniqueName="colm_TOT_APP_SELECTED">
                                                                                                     <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                     <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="POINTS_TOTAL"
                                                                                                    FilterControlAltText="Filter POINTS_TOTAL column"
                                                                                                    HeaderText="TOTAL POINTS" SortExpression="POINTS_TOTAL"
                                                                                                    UniqueName="colm_POINTS_TOTAL">
                                                                                                     <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                     <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="POINTS_MAX"
                                                                                                    FilterControlAltText="Filter POINTS_MAX column"
                                                                                                    HeaderText="MAX POINTS TO ASSIGNED" SortExpression="POINTS_MAX"
                                                                                                    UniqueName="colm_ROUND_MAX_POINTS">
                                                                                                    <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                     <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="VOTES_MAX"
                                                                                                    FilterControlAltText="Filter VOTES_MAX column"
                                                                                                    HeaderText="VOTES PER EVALUATOR" SortExpression="VOTES_MAX"
                                                                                                    UniqueName="colm_VOTES_MAX">
                                                                                                    <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                     <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="SCORE_BASE"
                                                                                                    FilterControlAltText="Filter SCORE_BASE column"
                                                                                                    HeaderText="MINIMAL SCORE" SortExpression="SCORE_BASE"
                                                                                                    UniqueName="colm_SCORE_BASE">
                                                                                                    <ItemStyle Width="85px" CssClass="wrapWord"  />
                                                                                                     <HeaderStyle Width="85px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                                 <telerik:GridBoundColumn DataField="survey_name"
                                                                                                    FilterControlAltText="Filter survey_name column"
                                                                                                    HeaderText="ASSESSMENT" SortExpression="survey_name"
                                                                                                    UniqueName="colm_survey_name">
                                                                                                      <ItemStyle Width="175px" CssClass="wrapWord"  />
                                                                                                      <HeaderStyle Width="175px" />
                                                                                                </telerik:GridBoundColumn>

                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    
                                                                                   </ContentTemplate>                                                                                
                                                                             </asp:UpdatePanel>

                                                                         <br />
                                                              </div>
                                                      
                                                     </div>

                                                  <%--************************DOCUMENTS**********************************--%>
                                                <hr style="border-color:#000000" /> 
                                               
                                               <br />
                                               <div class="form-group row">
                                                    <div class="col-sm-2 text-right">
                                                       <asp:Label runat="server" ID="lblt_eval_document_title" CssClass="control-label text-bold">Document Title</asp:Label>
                                                    </div>
                                                    <div class="col-sm-3">
                                                       <telerik:RadTextBox ID="txt_eval_title" runat="server" Width ="99%" MaxLength="200">
                                                       </telerik:RadTextBox>

                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                    ControlToValidate="txt_eval_title" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Required" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                                    </div>

                                                    <div class="col-sm-2 text-right">
                                                           <asp:Label runat="server" ID="lbl_eval_document_type" CssClass="control-label text-bold">Document Type</asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                             <telerik:RadComboBox ID="cmb_eval_document_type" runat="server" Width="95%" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox>
                                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                    ControlToValidate="cmb_eval_document_type" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Required" ValidationGroup="5">*</asp:RequiredFieldValidator>
                                                            
                                                        </div>
                                                </div>
                                                <br />

                                                <div class="form-group row">
                                                      
                                                              <div class="col-sm-2 text-right">
                                                                 <asp:Label runat="server" ID="lblt_eval_attach_doc" CssClass="control-label text-bold">Attach Document</asp:Label>
                                                              </div>

                                                             <div class="col-sm-3">
                                                                  <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload2"
                                                                    TemporaryFolder  ="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded" />
                                                             
                                                              <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                                    ControlToValidate="AsyncUpload2" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Required" ValidationGroup="5">*</asp:RequiredFieldValidator>--%>
                                                            
                                                             </div>  
                                                    
                                                              <div class="col-sm-2">
                                                              
                                                              </div>

                                                                       <div class="col-sm-3">
                                                                             
                                                                            <asp:LinkButton runat="server" ID="btn_add_eval_document"  AutoPostBack="True" SingleClick="true"  Text="Rounds" Width="70%" class="btn btn-info btn-sm margin-r-5 pull-left" ValidationGroup="5"   ><i class="fa  fa-file-text"></i>&nbsp;&nbsp;Add Document</asp:LinkButton><br />            
                                                         
                                                                        </div>   
                                                                                                           
                                                           </div>
                                                          
                                                            <hr /> 

                                                              <div class="form-group row">

                                                                  <div class="col-sm-12" style="padding-left:30px;">

                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <telerik:RadGrid ID="grd_eval_Document" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_ACTIVITY_ANNEX">
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column"
                                                                                                    HeaderButtonType="PushButton" UniqueName="ImageDownload">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Adjunto" />
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
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    <br />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                        
                                                                      </div>

                                                              </div>

                                                 <%--************************EVALUATION DOCUMENTS**********************************--%>
                                                    
                                              </div> <%--EVAL_2--%>     

                                          </div><%--id="Eval_Team"--%>
                                        <%--******************************************EVALUATION SETTUP**********************************--%>
                                     

                                       
                                    </div> <%--class="tab-content"--%>                                                                       
                                  
                               
                                     </div>  <%--class="box-body"--%>

                       
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


                <div class="modal fade bs-example-modal-sm" id="modal_RoundAdd" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-lg vertical-align-center">

                          

                        </div>
                    </div>
                </div>


            </div>
        </div>


        <telerik:RadWindow InitialBehaviors="Maximize" RenderMode="Lightweight" runat="server" Width="800" Height="300" id="rdEvaluationRound" VisibleOnPageLoad="false">
            <ContentTemplate>   
                
                  <div class="box">

                      <div class="box-header bg-orange" >
                           <h4 class="modal-title" runat="server" id="H1"><asp:Label runat="server" ID="lblt_Eval_Round" >Evaluation Round</asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_round" >#1</asp:Label></h4>
                      </div>                      
                      <div class="box-body">
                           
                                   <div class="form-group row">
                                         <br />
                                         <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_round_start_date" CssClass="control-label text-bold">Evaluation Round Start Date</asp:Label>
                                        </div>
                               
                                         <div class="col-sm-2">    
                                               <input type="text" value="" id="EvalROUND_StartDate" />
                                               <asp:HiddenField ID="HEvalROUND_StartDate" runat="server" />   
                                               <asp:HiddenField ID="H_ROUND_ID" runat="server"  Value ="0"/>   
                                         </div>

                                          <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_round_End_date" CssClass="control-label text-bold">Evaluation Round End Date</asp:Label>
                                          </div>
                                         
                                         <div class="col-sm-2">
                                              <input type="text" value="" id="EvalROUND_EndDate" />
                                              <asp:HiddenField ID="HEvalROUND_EndDate" runat="server" />
                                         </div>                                                   

                                     </div>
                                     
                                      <div class="form-group row">
                                          <br />
                                          <div class="col-sm-3 text-right">
                                              <asp:Label runat="server" ID="lblt_voting_type" CssClass="control-label text-bold">Evaluation Type</asp:Label>
                                          </div>
                                         
                                           <div class="col-sm-4">
                                                 <telerik:RadComboBox ID="cmb_voting_type" runat="server" Width="95%" Filter="Contains" AllowCustomText="true" EmptyMessage="Select the category" OnClientSelectedIndexChanged="SetActive"></telerik:RadComboBox>
                                           </div>                                                                                        

                                      </div>

                                      <div class="form-group row">
                                          <br />
                                           <div class="col-sm-3 text-right">
                                              <asp:Label runat="server" ID="lblt_assement" CssClass="control-label text-bold">Assessment assigned</asp:Label>
                                          </div>
                                           <div class="col-sm-4">
                                                 <telerik:RadComboBox ID="cmb_assessment" runat="server" Width="95%" Filter="Contains" AllowCustomText="true" EmptyMessage="Select the assessment" Enabled="false"  ></telerik:RadComboBox>
                                           </div>   
                                          <div class="col-sm-2 text-right">
                                              <asp:Label runat="server" ID="lblt_score_minimal" CssClass="control-label text-bold">Minimal Score</asp:Label>
                                          </div>
                                           <div class="col-sm-3">                                               
                                                  <telerik:RadNumericTextBox ID="txt_min_score" runat="server" EmptyMessage="1"
                                                                       MinValue="1"  ShowSpinButtons="true" Skin="Default" Width="51px"  Value="1" Enabled="false" >
                                                    <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                  </telerik:RadNumericTextBox>
                                           </div>   

                                     </div>

                                      <div class="form-group row">
                                          <br />

                                            <div class="col-sm-3 text-right">
                                                <asp:Label runat="server" ID="lblt_Selected_Applications" CssClass="control-label text-bold">#Applications to Accept</asp:Label>
                                            </div>
                                         
                                           <div class="col-sm-6">
                                               
                                                  <telerik:RadNumericTextBox ID="txt_app_tot" runat="server" EmptyMessage="1"
                                                                       MaxValue="100" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px"  Value="0" >
                                                                       <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                  </telerik:RadNumericTextBox>
                                                  <asp:Label runat="server" ID="lblt_warn_Of_Zero" CssClass="control-label text-bold">(0) for individual evaluation of applications</asp:Label>
                                           </div>                                              

                                       </div>

                                       <div class="form-group row">
                                          <br />
                                         <div class="col-sm-3 text-right">
                                             <asp:Label runat="server" ID="lblt_votes" CssClass="control-label text-bold"># Votes per Evaluator</asp:Label>
                                          </div>                                                  
                                          <div class="col-sm-3">                                                        
                                             <telerik:RadNumericTextBox ID="txt_tot_votes" runat="server" EmptyMessage="1"
                                                                       MaxValue="100" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px"  Value="0" >
                                                                       <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                             </telerik:RadNumericTextBox>
                                          </div>      

                                       </div>
                            
                                      <div class="form-group row">
                                          <br />
                                          <div class="col-sm-3 text-right">
                                             <asp:Label runat="server" ID="lblt_Total_Points" CssClass="control-label text-bold">Total Points Per Evaluator</asp:Label>
                                          </div>                                                  
                                           <div class="col-sm-3">
                                                                                             
                                                  <telerik:RadNumericTextBox ID="txt_total_points" runat="server" EmptyMessage="1"
                                                                       MaxValue="100" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px"  Value="0"  >
                                                                       <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                  </telerik:RadNumericTextBox>

                                           </div>  
                                              <div class="col-sm-4 text-right">
                                                 <asp:Label runat="server" ID="lblt_Max_points" CssClass="control-label text-bold">Max Points</asp:Label>
                                              </div>                                                  
                                               <div class="col-sm-2">
                                                      <telerik:RadNumericTextBox ID="txt_max_points" runat="server" EmptyMessage="1"
                                                                           MaxValue="100" MinValue="0"  ShowSpinButtons="true" Skin="Default" Width="51px"  Value="0"  >
                                                                           <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                      </telerik:RadNumericTextBox>
                                               </div>  
                                           
                                      </div>
                                
                                       


                       </div>
                       <div class="box-footer">
                           <asp:Button runat="server" ID="btn_registrar_tc" CssClass="btn btn-sm btn-primary btn-ok" Text="Save Evaluation Round" data-dismiss="modal" UseSubmitBehavior="false" />
                           <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2" onclick="CloseRadWindowTool('');">Cancel</button>
                       </div>


                   </div>

            </ContentTemplate>
        </telerik:RadWindow>

    </section>

                                   

                                        

    <script>
               

         $('#<%=chk_data_in.ClientID %>').prop('checked', true);//default value MAndatory

        function MUST_input() {
           if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {           
               //alert('Checkeado');               
               var OPT_Symbol =  $('input[id*=opt_mandatory]');
               $("#Mandatory_value").html(OPT_Symbol.val());
               //$("#span_curr_entry").html(currencySymbol.val());               
            } else {
               //alert('NO Checkeado');
               var OPT_Symbol = $('input[id*=opt_optional]');
              $("#Mandatory_value").html(OPT_Symbol.val());
               //$("#span_curr_entry").html(currencySymbol.val());
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


        var loadedB = false;

        
                     var prm = Sys.WebForms.PageRequestManager.getInstance();
                          prm.add_endRequest(function() {

                              var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Documents";
                               console.log('Displaying tab ' + tabName);
                              $('#Tabs_options a[href="#' + tabName + '"]').tab('show');             

                                $("#Tabs_options a").click(function () {

                                   console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                                   $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                              });


                                  MUST_input();

                                  $.datetimepicker.setLocale('en-GB');
                   
                                   set_Calendar_Vars('Eval_StartDate', 'HEval_StartDate');
                                   set_Calendar_Vars('Eval_EndDate', 'HEval_EndDate');

                                   set_Calendar_Vars('EvalSETT_StartDate', 'HEvalSETT_StartDate');
                                   set_Calendar_Vars('EvalSETT_EndDate', 'HEvalSETT_EndDate');

                                   set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate');
                                   set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate');

                                   loadedB = true;
                              
                          });


           $(document).ready(function () {                                    

                //  var QueryVariable = getParameterByName('_tab');
                //  //alert(QueryVariable);              
                ////console.log('Displaying tab ' + QueryVariable);
                //if (QueryVariable != '' && QueryVariable != null) {
                //   $('#ADDons a[href="#' + QueryVariable + '"]').tab('show');
                //  }

               var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Documents";
                    $('#Tabs_options a[href="#' + tabName + '"]').tab('show');                

                   $("#Tabs_options a").click(function () {

                      // console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                       $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                   });


              MUST_input();

              $.datetimepicker.setLocale('en-GB');
                   
               set_Calendar_Vars('Eval_StartDate', 'HEval_StartDate');
               set_Calendar_Vars('Eval_EndDate', 'HEval_EndDate');

               set_Calendar_Vars('EvalSETT_StartDate', 'HEvalSETT_StartDate');
               set_Calendar_Vars('EvalSETT_EndDate', 'HEvalSETT_EndDate');

               set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate');
               set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate');

               loadedB = true;

         });           


        function loadscript() {
                        
        }

        //function FuncModal_rounds() {

        //    $('#modal_RoundAdd').modal('show');
        //   //  $('#modalTasaCambio').modal('show');
        //       set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
        //       set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
         
        //}


              function OpenRadWindowTool(url) {
                  
                  var div = $("#<%= rounds_control.ClientID %>")[0]; 
                  
                        var divOffset = offset(div);
                        //console.log('this are the offsets: ' +  divOffset.left, divOffset.top);

                        var oWnd = $find("<%= rdEvaluationRound.ClientID %>");
                        //console.log("http://rms.ftfyla.com/RMS_SIME/Deliverable/frm_DeliverableFollowingRep.aspx?ID=" + idDeliverable);
                                          
                        oWnd.moveTo(divOffset.left, divOffset.top - 100);


                        oWnd.add_close(OnClientClose_Tool); //set a function to be called when
                        oWnd.show();
                        oWnd.setSize(900, 400);
                        //oWnd.setUrl(url); //'frm_DeliverableFollowingRep.aspx?ID=' + id
                        //oWnd.setUrl('http://www.yahoo.com');
                        oWnd.minimize();
                        oWnd.maximize();
                        oWnd.restore();  
                  
                       set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
                       set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
                        
        }


                function CloseRadWindowTool(url) {

                     var oWnd = $find("<%= rdEvaluationRound.ClientID %>");
                     oWnd.close();
                     set_Calendars();

                }

             function OnClientClose_Tool(oWnd) {  
                // oWnd.setUrl("about:blank"); // Sets url to blank  
                 oWnd.remove_close(OnClientClose_Tool);   
                   set_Calendars();
             }       


            function offset(el) {
                var rect = el.getBoundingClientRect(),
                scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
                scrollTop = window.pageYOffset || document.documentElement.scrollTop;
                return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
            }

        

        function set_Calendars() {

            if (loadedB) {

                set_Calendar_Vars('Eval_StartDate', 'HEval_StartDate');
                set_Calendar_Vars('Eval_EndDate', 'HEval_EndDate');

                set_Calendar_Vars('EvalSETT_StartDate', 'HEvalSETT_StartDate');
                set_Calendar_Vars('EvalSETT_EndDate', 'HEvalSETT_EndDate');

                set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate');
                set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate');

            } else {

                console.log('Calendars not setted');

            }

        }
        

        //function FgetDate() {

        //    var dateAsObject = $('#Eval_StartDate').datetimepicker('getDate'); 
        //    dateAsObject = $.datetimepicker.formatDate('mm-dd-yy', new Date(dateAsObject))
        //    //console.log(dateAsObject);
        //    console.log($.datetimepicker.formatDate('dd/mm/yy', $('#Eval_StartDate').datetimepicker('getDate')));

        //}
            
              <%--  function DateChange(sender, args) {
                    $find("<%= RadDateTimePicker1.ClientID %>").showTimePopup();
                }--%>

        function SetActive(sender, eventArgs) {


            var combobox = $find('<%=cmb_voting_type.ClientID %>');

            if (combobox.get_selectedIndex() == null)
            { 
                   console.log("You must select a category before processing search!");
                   return false;
            }

             var C_value = combobox.get_selectedItem().get_value();
             var texto = combobox.get_selectedItem().get_text();

            console.log(texto + ': ' + C_value);

            var obj_VOTES = $find('<%= txt_tot_votes.ClientID %>')
            var obj_TOT_POINTS =  $find('<%= txt_total_points.ClientID %>')
            var obj_MAX_POINTS = $find('<%= txt_max_points.ClientID %>')
            var obj_MIN_SCORE = $find('<%= txt_min_score.ClientID %>')
            var obj_ASSESMENT = $find('<%= cmb_assessment.ClientID %>')
            

            switch (parseInt(C_value)) {

                case 1:
                    obj_VOTES.disable();
                    obj_TOT_POINTS.disable();
                    obj_MAX_POINTS.disable();
                    obj_MIN_SCORE.enable();
                    obj_ASSESMENT.enable();
                    break;
                case 2:
                    obj_VOTES.enable();
                    obj_TOT_POINTS.disable();
                    obj_MAX_POINTS.disable();
                    obj_MIN_SCORE.disable();
                    obj_ASSESMENT.disable();
                    break;
                case 3:
                    obj_VOTES.disable();
                    obj_TOT_POINTS.disable();
                    obj_MAX_POINTS.disable();
                    obj_MIN_SCORE.disable();
                    obj_ASSESMENT.disable();
                   // console.log('disables VOTES');
                    break;
                 case 4:
                    obj_VOTES.disable();
                    obj_TOT_POINTS.enable();
                    obj_MAX_POINTS.enable();
                    obj_MIN_SCORE.disable();
                    obj_ASSESMENT.disable();
                    break;
                 case 5:
                    obj_VOTES.disable();
                    obj_TOT_POINTS.disable();
                    obj_MAX_POINTS.disable();
                    obj_MIN_SCORE.disable();
                    obj_ASSESMENT.disable();
                    break;
                //default:
                //    console.log('Nothing Picked');

            }


        }


    </script>
</asp:Content>
