<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityEvaluation.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityEvaluation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <script src="<%=ResolveUrl("~/Content/dist/js/app.js")%>"></script>
    <script src="<%=ResolveUrl("~/plugins/bootstrap-slider/bootstrap-slider.js")%>" ></script>
       <link rel="stylesheet" href="<%=ResolveUrl("~/plugins/bootstrap-slider/slider.css")%>">    

        <div class="box" id="main_box">
            <div class="box-header with-border">               
                 <div class="col-sm-11">   
                     <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activity - Apply</asp:Label>
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

               
                    <div  id="dvTab">
                        <div class="col-lg-12">
                                <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" class="hidden" id="alink_definicion" href="#">ACTIVITY</a></li>                                
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation" ><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation" class="active"><a class="primary" runat="server" id="alink_evaluation">EVALUATION</a></li>
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
                        <asp:Label ID="lbl_id_sol_app" runat="server" Text="" Visible="false" />

                            <asp:HiddenField ID="H_ID_APPLY_EVALUATION" runat="server" Value ="0" />  
                            <asp:HiddenField ID="H_ID_APPLY_APP" runat="server" Value ="0" />  
                            <asp:HiddenField ID="H_ID_EVALUATION_APP" runat="server" Value ="0" /> 
                            <asp:HiddenField ID="H_ID_SOLICITATION_APP" runat="server" Value ="0" /> 
                            <asp:HiddenField ID="H_ID_EVALUATION_ROUND" runat="server" Value ="0" /> 
                            <asp:HiddenField ID="H_ID_ACTIVITY_SOLICITATION" runat="server" Value ="0" /> 
                            <asp:HiddenField ID="H_COMM_TYPE" runat="server" Value ="1" /> 
                            <asp:HiddenField ID="H_POINTS_VAL" runat="server" Value ="0" />  
                            <asp:HiddenField ID="H_MAX_POINTS_SEL" runat="server" Value ="0" />  
                        

                             <asp:HiddenField ID="hfTab" runat="server" />  
                             <asp:HiddenField ID="Hiddenindi" runat="server" />
                      
                                                                                    
                      <%--*********************************************SOLICITATION*********************************************************--%>
                           <div class="box box-info-t  collapsed-box" id="Solicitation_Tab">
                                        <div class="box-header with-border">
                                          <h3 class="box-title">SOLICITATION</h3>
                                          <div class="box-tools pull-right">
                                            <button class="btn btn-box-tool" id="btn_Solicitation_Tab" data-widget="collapse" data-toggle="tooltip" data-target="#Solicitation_Tab"  title="Collapse"><i class="fa fa-plus"></i></button>
                                            <button class="btn btn-box-tool" id="btn_remove_Solicitation_Tab" data-widget="remove" data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div>
                                        <div class="box-body">                                          
                                              
                                                      <hr />

                                                                 <div class="form-group row">

                                                                             <div class="col-sm-2 text-right">
                                                                                 <asp:Label runat="server" ID="lblt_solicitation_code" CssClass="control-label text-bold">Solicitation Code</asp:Label>
                                                                              </div>
                                                                              <div class="col-sm-4">                                             
                                                                                   <div class="alert-sm bg-blue text-center" runat="server" id="divCodigo" style="width: 300px;">
                                                                                       <asp:Label ID="lbl_COde" runat="server" CssClass="text-bold" ></asp:Label>
                                                                                  </div>                                           
                                                                               </div>

                                                                                   <div class="col-sm-2 text-right">
                                                                                         <asp:Label runat="server" ID="lblt_Activity_code" CssClass="control-label text-bold">Apply Code</asp:Label>
                                                                                      </div>
                                                                                      <div class="col-sm-4">
                                                                                            <telerik:RadTextBox ID="txt_activity_code" runat="server" Width="80%" MaxLength="250" ReadOnly="true" >
                                                                                            </telerik:RadTextBox>
                                                                                       </div>
                                                                 
                                                                                                                                                                                    
                                                                 
                                                                            </div>
                                                                            <br />
                                                        
                                                                             <div class="form-group row">

                                                                                      <div class="col-sm-2 text-right">
                                                                                         <asp:Label runat="server" ID="lblt_solicitation_type" CssClass="control-label text-bold">Solicitation Type</asp:Label>
                                                                                      </div>
                                                                                      <div class="col-sm-4">
                                                                                         <telerik:RadComboBox ID="cmb_solicitation_type" runat="server" Width="300px" EmptyMessage="Pick one category" Enabled="false" ></telerik:RadComboBox>
                                                                                      </div>           

                                                                                     <div class="col-sm-2 text-right">
                                                                                         <asp:Label runat="server" ID="lblt_solicitation_status" CssClass="control-label text-bold">Solicitation Status</asp:Label>
                                                                                      </div>
                                                                                      <div class="col-sm-4">
                                                                                           <telerik:RadComboBox ID="cmb_solicitation_status" runat="server" Width="300px" Enabled="false" ></telerik:RadComboBox>
                                                                                       </div>                                     
                                                                               </div>
                                                                               <br />

                                                 
                                                                                      <div class="form-group row">
                                                                                            <div class="col-sm-2 text-right">
                                                                                                <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Issuance Date</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                                   <asp:Label runat="server" ID="lbl_fecha_inicio" CssClass="control-label text-bold"></asp:Label>
                                                                                            </div>
                                                                                             <div class="col-sm-2 text-right">
                                                                                                <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">Close Date</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                                <asp:Label runat="server" ID="lbl_fecha_final" CssClass="control-label text-bold">Close Date</asp:Label>
                                                                                            </div>
                                                                                    </div>
                                                                                    
                                                                                    <hr />
                                                                                      <div class="form-group row">
                                                                                        <div class="col-sm-2 text-right">
                                                                                            <asp:Label runat="server" ID="lblt_Tittle" CssClass="control-label text-bold">Title</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-sm-10">
                                                                                            <telerik:RadTextBox ID="txt_tittle" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="2000" ReadOnly="true"  BorderStyle="None"  Resize="Both">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                                                ControlToValidate="txt_tittle" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                        </div>
                                                                                     </div>
                                                                                      <br />
                                                                                      <div class="form-group row">
                                                                                        <div class="col-sm-2 text-right">
                                                                                            <asp:Label runat="server" ID="lbl_purpose" CssClass="control-label text-bold ">Purpose</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-sm-10">
                                                                                            <telerik:RadTextBox ID="txt_purpose" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="2000"  ReadOnly="true" BorderStyle="None"  Resize="Both">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                                                ControlToValidate="txt_purpose" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                        </div>
                                                                                     </div>

                                                                                     <br />
                                                                                      <div class="form-group row">
                                                                                        <div class="col-sm-2 text-right">
                                                                                            <asp:Label runat="server" ID="lblt_solicitation_content" CssClass="control-label text-bold">Solicitation</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-sm-10">
                                                                                            <telerik:RadTextBox ID="txt_solicitation" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="5000"   ReadOnly="true" BorderStyle="None" Resize="Both">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                                ControlToValidate="txt_solicitation" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                        </div>
                                                                                     </div>

                                                                               
                                                                                           <hr /> 

                                                                                          <div class="form-group row">
                                                                                              <div class="col-sm-1" >
                                                                                              
                                                                                              </div>
                                                                                              <div class="col-sm-11" >

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
                                                                                                                                    <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Adjunto" />
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
                                                                                                                           
                                                                                       
                                            
                                        </div><!-- /.box-body -->
                                        <div class="box-footer">
                                         <span class="pull-left"><h4 class="box-title">SOLICITATION</h4></span>
                                         <div class="box-tools pull-right">                                            
                                            <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" data-target="#Solicitation_Tab"  title="Collapse"><i class="fa fa-minus"></i></button>
                                            <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div><!-- /.box-footer-->
                                      </div><!-- /.box -->  
                      <%--*********************************************SOLICITATION*********************************************************--%>
                                                           
                      <%--*********************************************APPLICANTS*********************************************************--%>
                               <div class="box box-primary-t collapsed-box" id="APPLICANTS">
                                        <div class="box-header with-border">
                                          <h3 class="box-title">APPLICANTS</h3>
                                          <div class="box-tools pull-right">
                                            <button class="btn btn-box-tool" id="btn_APPLICANTS" data-widget="collapse" data-toggle="tooltip" data-target="#APPLICANTS"  title="Collapse"><i class="fa fa-minus"></i></button>
                                            <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" data-target="#APPLICANTS"   title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div>
                                        <div class="box-body">
                                              <div class="row">



                                                    <asp:Repeater ID="Repeater_Organization" runat="server">
                                                        <ItemTemplate> 
                                                        
                                                             <div class="col-md-6">
                                                                  <!-- Widget: user widget style 1 -->
                                                                  <div class="box box-widget widget-user-2">
                                                                    <!-- Add the bg color to the header using any of the bg-* classes -->
                                                                    <div class="widget-user-header <%# Eval("nColor") %> ">
                                                                      <div class="widget-user-image">
                                                                        <img class="img-circle" src="<%# Eval("USERIMAGE") %>" alt="<%# Eval("ORGANIZATIONNAME") %>"  title="<%# Eval("ORGANIZATIONNAME") %>" >
                                                                      </div><!-- /.widget-user-image -->
                                                                      <h3 class="widget-user-username"><%# Eval("ORGANIZATIONNAME") %></h3>
                                                                      <h5 class="widget-user-desc"><%# Eval("NAMEALIAS") %></h5>
                                                                    </div>
                                                                    <div class="box-footer no-padding">
                                                                      <ul class="nav nav-stacked">
                                                                        <li><a class="btn btn-default text-left" href="frm_ActivityEvaluation?ut=<%# Eval("SOLICITATION_TOKEN") %>&id=<%# Eval("ID_ACTIVITY") %>&ia=<%# Eval("ID_SOLICITATION_APP") %>&is=<%# Eval("ID_ACTIVITY_SOLICITATION") %>&_tab=APPLICATION_BOX#APP_BOX"><span class="pull-left">Application</span><span class="">&nbsp;</span><span class='label <%# Eval("STATUS_FLAG") %> text-center text-sm pull-right' >&nbsp;&nbsp;&nbsp;<i class='fa <%# Eval("STATUS_ICON") %>'></i>&nbsp;&nbsp;&nbsp;<%# Eval("APPLY_STATUS") %>&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;<%# Func_Unit(Eval("ACCEPTED_DATE"), Date.UtcNow) %></span></a></li>
                                                                          
                                                                              <asp:Repeater ID="rept_ORG_EVAL" runat="server">
                                                                                 <ItemTemplate>
                                                                                    <li><a class="btn btn-default <%# If(Eval("CURR_ROUND") = 0, "disabled", "") %>  text-left" href="frm_ActivityEvaluation?ut=<%# Eval("SOLICITATION_TOKEN") %>&id=<%# Eval("ID_ACTIVITY") %>&ia=<%# Eval("ID_SOLICITATION_APP") %>&is=<%# Eval("ID_ACTIVITY_SOLICITATION") %>&ir=<%# Eval("ID_ROUND") %>&_tab=ROUND_BOX#EVA_BOX"><span class="pull-left">Evaluation Round #<%# Eval("ID_ROUND") %></span><span class="badge <%# Eval("VOTING_FLAG") %>"><%# Eval("VOTING_TYPE") %>&nbsp;&nbsp;<%# Eval("TOT_OBTAINED") %>&nbsp;/&nbsp;<%# Eval("TARGET_TO_OBTAIN") %>&nbsp;&nbsp;</span><span class='label <%# Eval("STATUS_FLAG") %> text-center text-sm pull-right' > <%# Eval("EVALUATION_APP_STATUS") %>&nbsp;&nbsp;&nbsp;<i class='fa <%# Eval("STATUS_ICON") %>'></i>&nbsp;&nbsp;&nbsp;<%# If(Eval("ID_EVALUATION_APP_STATUS") = 0, Func_Unit(Eval("ACCEPTED_DATE"), Date.UtcNow), Func_Unit(Eval("UPDATED_DATE"), Date.UtcNow)) %></span></a></li>
                                                                                    <li>
                                                                                       <div class="progress-xxs">
                                                                                         <div class="progress-bar <%# Eval("nColor") %>" style="width:<%# Eval("PERC_OBTAINED_TO") %>%"></div>
                                                                                      </div>
                                                                                     </li> 
                                                                              </ItemTemplate>
                                                                            </asp:Repeater>                                                                                                                                                                                                                          
                                                                      </ul>
                                                                    </div>
                                                                  </div><!-- /.widget-user -->
                                                            
                                                                 </div><!-- /.col -->
                                                        </ItemTemplate>     
                                                    </asp:Repeater>

                                                 <%--    <div class="col-md-6">
                                                          <!-- Widget: user widget style 1 -->
                                                          <div class="box box-widget widget-user-2">
                                                            <!-- Add the bg color to the header using any of the bg-* classes -->
                                                            <div class="widget-user-header bg-yellow">
                                                              <div class="widget-user-image">
                                                                <img class="img-circle" src="https://ftfuganda3217.blob.core.windows.net/pbfiles/1601951681886Chemonics-log150_nw.png" alt="User Avatar">
                                                              </div><!-- /.widget-user-image -->
                                                              <h3 class="widget-user-username">Wildlife Works Carbon</h3>
                                                              <h5 class="widget-user-desc">WWC</h5>
                                                            </div>
                                                            <div class="box-footer no-padding">
                                                              <ul class="nav nav-stacked">
                                                                <li><a class="btn btn-default text-left" href="frm_activity"><span class="pull-left">Application</span><span class="">&nbsp;</span><span class='label label-success text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                 <li><a class="btn btn-default text-left" href="frm_activity"><span class="pull-left">Evaluation Round #1</span><span class="badge bg-aqua">By Popularity</span><span class='label label-warning text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li>
                                                                   <div class=" progress-xxs">
                                                                     <div class="progress-bar bg-yellow" style="width:33%"></div>
                                                                  </div>
                                                                 </li> 
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #2</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li>
                                                                   <div class=" progress-xxs">
                                                                     <div class="progress-bar bg-yellow" style="width:5%"></div>
                                                                  </div>
                                                                 </li> 
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #3</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Dismissed&nbsp;&nbsp;&nbsp;<i class='fa fa-thumbs-o-down '></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #4</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa  fa-thumbs-o-up'></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #5</span><span class="badge bg-aqua">By Negotiation</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                              </ul>
                                                            </div>
                                                          </div><!-- /.widget-user -->
                                                        </div><!-- /.col -->--%>

                                                        <%--<div class="col-md-6">
                                                          <!-- Widget: user widget style 1 -->
                                                          <div class="box box-widget widget-user-2">
                                                            <!-- Add the bg color to the header using any of the bg-* classes -->
                                                            <div class="widget-user-header bg-blue-active">
                                                              <div class="widget-user-image">
                                                                <img class="img-circle" src="https://ftfuganda3217.blob.core.windows.net/pbfiles/1601951681886Chemonics-log150_nw.png" alt="User Avatar">
                                                              </div><!-- /.widget-user-image -->
                                                              <h3 class="widget-user-username">Instituto Brasileiro do Meio Ambiente</h3>
                                                              <h5 class="widget-user-desc">IBAMA</h5>
                                                            </div>
                                                            <div class="box-footer no-padding">
                                                             <ul class="nav nav-stacked">
                                                                <li><a class="btn btn-default text-left" href="frm_activity"><span class="pull-left">Application</span><span class="">&nbsp;</span><span class='label label-success text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default text-left" href="frm_activity"><span class="pull-left">Evaluation Round #1</span><span class="badge bg-aqua">By Popularity</span><span class='label label-warning text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #2</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #3</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Dismissed&nbsp;&nbsp;&nbsp;<i class='fa fa-thumbs-o-down '></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #4</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa  fa-thumbs-o-up'></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #5</span><span class="badge bg-aqua">By Negotiation</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                              </ul>
                                                            </div>
                                                          </div><!-- /.widget-user -->
                                                        </div><!-- /.col -->--%>

                                                    <%--    <div class="col-md-6">
                                                          <!-- Widget: user widget style 1 -->
                                                          <div class="box box-widget widget-user-2">
                                                            <!-- Add the bg color to the header using any of the bg-* classes -->
                                                            <div class="widget-user-header bg-green">
                                                              <div class="widget-user-image">
                                                                <img class="img-circle" src="https://ftfuganda3217.blob.core.windows.net/pbfiles/1601951681886Chemonics-log150_nw.png" alt="User Avatar">
                                                              </div><!-- /.widget-user-image -->
                                                              <h3 class="widget-user-username">SOS MATA ATLÂNTICA</h3>
                                                              <h5 class="widget-user-desc">SOSMA</h5>
                                                            </div>
                                                            <div class="box-footer no-padding">
                                                             <ul class="nav nav-stacked">
                                                                <li><a class="btn btn-default text-left" href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&_tab=APPLICATION_BOX"><span class="pull-left">Application</span><span class="">&nbsp;</span><span class='label label-success text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default text-left" href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&ir=1&_tab=EVALUATION_BOX"><span class="pull-left">Evaluation Round #1</span><span class="badge bg-aqua">By Popularity</span><span class='label label-warning text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&ir=2&_tab=EVALUATION_BOX"><span class="pull-left">Evaluation Round #2</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&ir=3&_tab=EVALUATION_BOX"><span class="pull-left">Evaluation Round #3</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Dismissed&nbsp;&nbsp;&nbsp;<i class='fa fa-thumbs-o-down '></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&ir=4&_tab=EVALUATION_BOX"><span class="pull-left">Evaluation Round #4</span><span class="badge bg-aqua">By Points</span><span class='label label-primary text-center text-sm pull-right' > Accepted&nbsp;&nbsp;&nbsp;<i class='fa  fa-thumbs-o-up'></i>&nbsp;&nbsp;&nbsp;2 votes</span></a></li>
                                                                <li><a class="btn btn-default disabled text-left" href="frm_activity"><span class="pull-left">Evaluation Round #5</span><span class="badge bg-aqua">By Negotiation</span><span class='label label-primary text-center text-sm pull-right' > Pending&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;1.13 weeks</span></a></li>
                                                              </ul>
                                                            </div>
                                                          </div><!-- /.widget-user -->
                                                        </div><!-- /.col -->   --%>
                                                
                                              </div>
                                        </div><!-- /.box-body -->
                                        <div class="box-footer">
                                           <span class="pull-left"><h4 class="box-title">APPLICANTS</h4></span>
                                             <div class="box-tools pull-right">                                            
                                                  <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" data-target="#APPLICANTS"  title="Collapse"><i class="fa fa-minus"></i></button>
                                                <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip"  data-target="#APPLICANTS"   title="Remove"><i class="fa fa-times"></i></button>
                                            </div>    
                                        </div><!-- /.box-footer-->
                           </div><!-- /.box -->  
                      <%--*********************************************APPLICANTS*********************************************************--%>
                                      
                     <%--*********************************************APPLICATION*********************************************************--%>
                     <a name="APP_BOX"  id="APP_BOX"></a>   

                        <div class="box box-success-t collapsed-box" id="APPLICATION_BOX">
                                        <div class="box-header with-border">
                                          <h3 class="box-title">APPLICATION</h3>
                                          <div class="box-tools pull-right">
                                            <button class="btn btn-box-tool" id="btn_APPLICATION_BOX" data-widget="collapse" data-toggle="tooltip" data-target="#APPLICATION_BOX"  title="Collapse"><i class="fa fa-plus"></i></button>
                                            <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" data-target="#APPLICATION_BOX"  title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div>
                                        <div class="box-body">       
                                            <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                               <br />
                                                                        
                                                                                     <div class="form-group row">

                                                                                           <div class="col-sm-2 text-right">
                                                                                                  <asp:Label runat="server" ID="lblt_apply_code" CssClass="control-label text-bold">Application Code</asp:Label>
                                                                                           </div>
                                                                                           <div class="col-sm-4">                                             
                                                                                                <div class="alert-sm bg-blue text-center" runat="server" id="div1" style="width: 300px;">
                                                                                                    <asp:Label ID="lbl_apply_code" runat="server" CssClass="text-bold" ></asp:Label>
                                                                                                </div>                                           
                                                                                           </div>

                                                                                           <div class="col-sm-2 text-right">
                                                                                               <br />
                                                                                               <asp:Label runat="server" ID="lblt_apply_status" CssClass="control-label text-bold">Application Status</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                                <h4><span id="spanSTATUS" runat="server" class='label ' > <asp:Label runat="server" ID="lbl_apply_status" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_Apply_time" Text=""></asp:Label></span></h4>
                                                                                            </div>
                                                                                   </div>
                                                                                   <br />
                                                                                  <div class="form-group row">

                                                                                            <div class="col-sm-2 text-right">
                                                                                               <asp:Label runat="server" ID="lblt_ORganization" CssClass="control-label text-bold">Organization</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                              <asp:Label runat="server" ID="lbl_organization" CssClass="control-label text-bold"></asp:Label>
                                                                                             </div>

                                                                                           <div class="col-sm-2 text-right">
                                                                                               <asp:Label runat="server" ID="lblt_status_date" CssClass="control-label text-bold">Date updated</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                             <asp:Label runat="server" ID="lbl_status_date" CssClass="control-label text-bold"></asp:Label>
                                                                                             </div>

                                                                                  </div>

                                                  <%--**********************************************************************************APPLICANTS DOCUMENTS*************************************************************--%>
                                      
                                                                                <hr />                                                                                
                                                                                 <div id="AppDOCUMENTS" runat="server" style="border:1px solid thick;">                                                                                        
                                                                                 <br />
                                                                                                     <div class="form-group row">                                                                                                          
                                                                                                           <div class="col-sm-4 text-left">
                                                                                                                 <asp:Label runat="server" ID="Lblt_Documents" CssClass="control-label text-bold">Application Documents</asp:Label><br /><br />
                                                                                                           </div>
                                                                                                      </div>

                                                                                                      <div class="form-group row">                                                                                                            
                                                                                                          
                                                                                                          <div class="col-sm-12">

                                                                                                         <%--           <asp:UpdatePanel ID="PanelFirma" runat="server" UpdateMode="Conditional">
                                                                                                                        <ContentTemplate>--%>
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
                                                                                                                                                <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="File Attached" />
                                                                                                                                            </ItemTemplate>
                                                                                                                                            <HeaderStyle Width="5px" />
                                                                                                                                            <ItemStyle Width="5px" />
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
                                                                                                                                            <HeaderStyle Width="25%" />
                                                                                                                                            <ItemStyle Width="25%"  />
                                                                                                                                        </telerik:GridBoundColumn>
                                                                                                                                        <telerik:GridBoundColumn DataField="DOCUMENT_NAME"
                                                                                                                                            FilterControlAltText="Filter DOCUMENT_NAME column"
                                                                                                                                            HeaderText="Documents" SortExpression="DOCUMENT_NAME"
                                                                                                                                            UniqueName="colm_DOCUMENT_NAME" Visible="true" Display="false">
                                                                                                                                        <HeaderStyle Width="25%" />
                                                                                                                                        <ItemStyle Width="25%"  />
                                                                                                                                        </telerik:GridBoundColumn>

                                                                                                                                         <telerik:GridTemplateColumn 
                                                                                                                                              FilterControlAltText="Filter colm_template column"  HeaderText="File" 
                                                                                                                                              UniqueName="colm_FileName" >                                      
                                                                                                                                             <ItemTemplate>                                       
                                                                                                                                                 <asp:HyperLink ID="hlk_filename" 
                                                                                                                                                     runat="server" 
                                                                                                                                                     Text=""                                                                                    
                                                                                                                                                     navigateUrl="#" ToolTip="File"></asp:HyperLink>                                       
                                                                                                                                             </ItemTemplate>
                                                                                                                                              <ItemStyle Width="20%"  />
                                                                                                                                         </telerik:GridTemplateColumn>

                                                                                                                                          <telerik:GridTemplateColumn 
                                                                                                                                              FilterControlAltText="Filter colm_mandatory column"  HeaderText="Mandatory" 
                                                                                                                                              UniqueName="colm_mandatory" >                                      
                                                                                                                                             <ItemTemplate>                                       
                                                                                                                                                   <telerik:RadTextBox runat="server" ID="txtMandatory" Text=""></telerik:RadTextBox>                                 
                                                                                                                                             </ItemTemplate>
                                                                                                                                              <HeaderStyle Width="10%" />
                                                                                                                                              <ItemStyle Width="10%" />
                                                                                                                                           </telerik:GridTemplateColumn>
                                                                                                                                       
                                                                                                                                          <telerik:GridBoundColumn DataField="Template" 
                                                                                                                                                FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                                                                                UniqueName="Template" Visible="true" Display="false">                                        
                                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                                        
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
                                                                                                                      <%--  </ContentTemplate>
                                                                                                                    </asp:UpdatePanel>--%>
                                        
                                                                                                              </div>

                                                                                                     </div>
                                                                                                     
                                                                                </div> <%-- id="Documents" --%>
                                                                                                                                      
                                                   <%--**********************************************************************************APPLICANTS DOCUMENTS*************************************************************--%>
                                               
                                                                                  <hr style="border-color:black" />
                                                        
                                                                                  <div  id="app_History" runat="server" style="display:block;" class=" bg-gray-light ">
                                                                                       <ul class="timeline">

                                                                                          <asp:Repeater ID="rept_ApplyDates" runat="server">
                                                                                            <ItemTemplate>                                                                                            
                                                                                                  <li class="time-label">
                                                                                                    <span class="<%# Eval("nColor") %>">
                                                                                                            <%# getDate_(Eval("date_created"), "D", False) %>
                                                                                                    </span>
                                                                                                  </li>
                                                                                                 <asp:Repeater ID="rept_ApplyComm" runat="server">
                                                                                                    <ItemTemplate>
                                                                                                         <!-- timeline item -->
                                                                                                            <li>
                                                                                                                <!-- timeline icon -->
                                                                                                                <i class="fa <%# Eval("STATUS_ICON") %>" title="<%# Eval("APPLY_STATUS") %>&nbsp;by&nbsp;<%# Eval("USERNAME") %>"></i>
                                                                                                                <div class="timeline-item">
                                                                                                                    <span class="time"><i class="fa fa-clock-o"></i><%# getTime_(Eval("FECHA_CREA")) %></span>

                                                                                                                    <h3 class="timeline-header"> <img class="direct-chat-img <%# Eval("IMGCOLOR") %>" src="<%# Eval("USERIMAGEN") %>" alt="<%# Eval("USERNAME") %>" title="<%# Eval("USERNAME") %>">&nbsp;&nbsp;&nbsp;<a href="#"><%# Eval("USERNAME") %></a></h3>
                                                                                                                    <div class="timeline-body">
                                                                                                                        <%# Eval("APPLY_COMM") %>                                                                                                                       
                                                                                                                     </div>

                                                                                                                    <div class="timeline-footer">
                                                                                                                       <%-- <a class="btn btn-primary btn-xs">React</a>--%>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </li>
                                                                                                            <!-- END timeline item -->
                                                                                                     </ItemTemplate>  
                                                                                                 </asp:Repeater>  
                                                                                                
                                                                                           </ItemTemplate>
                                                                                        </asp:Repeater>  
                                                                                           
                                                                                      </ul>  
                                                                                   <br />                                                                                        
                                                                           </div>
                                 
                                                 <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                                                                                 
                                        </div><!-- /.box-body -->
                                       <div class="box-footer">
                                         <span class="pull-left"><h4 class="box-title">APPLICATION</h4></span>
                                         <div class="box-tools pull-right">                                            
                                               <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" data-target="#APPLICATION_BOX"  title="Collapse"><i class="fa fa-minus"></i></button>
                                               <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" data-target="#APPLICATION_BOX"  title="Remove"><i class="fa fa-times"></i></button>
                                         </div>    
                                      </div><!-- /.box-footer-->
                           </div><!-- /.box -->  
                      <%--*********************************************APPLICATION*********************************************************--%>
                      <%--  <hr />--%>
                     <%-- <br />--%>

                      <%--*********************************************EVALUATION OVERVIEW*********************************************************--%>
                         
                        <div class="box box-warning-t  collapsed-box" id="EVALUATION_BOX">
                                        <div class="box-header with-border">
                                          <h3 class="box-title">EVALUATION OVERVIEW</h3>
                                          <div class="box-tools pull-right">
                                            <button class="btn btn-box-tool" id="btn_EVALUATION_BOX" data-widget="collapse" data-toggle="tooltip" data-target="#EVALUATION_BOX"  title="Collapse"><i class="fa fa-plus"></i></button>
                                            <button class="btn btn-box-tool" data-widget="remove"  data-toggle="tooltip"   data-target="#EVALUATION_BOX"  title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div>
                                        <div class="box-body">
                                           
                                           <%--******************************************EVALUATION SETTUP**********************************--%>
                                                  
                                                 <div class="form-group row">
                                                        <div class="col-sm-2 text-right">
                                                            <asp:Label runat="server" ID="lblt_guidelines" CssClass="control-label text-bold">Guide Line</asp:Label>
                                                        </div>
                                                        <div class="col-sm-10">
                                                            <telerik:RadTextBox ID="txt_guidLines" runat="server" Rows="5" TextMode="MultiLine" Width="90%" Resize="Both" MaxLength="3000"   ReadOnly="true" BorderStyle="None" >
                                                            </telerik:RadTextBox>                                                          
                                                        </div>
                                                 </div>

                                               <div class="form-group row">
                                                  <br />
                                                  <div class="col-sm-2 text-right">
                                                     <asp:Label runat="server" ID="lblt_eval_start_date" CssClass="control-label text-bold">Start Date</asp:Label>
                                                 </div>
                                        
                                                  <div class="col-sm-4">    
                                                      <asp:Label runat="server" ID="lbl_evaluation_startDate" CssClass="control-label text-bold"></asp:Label>
                                                  </div>

                                                   <div class="col-sm-2 text-right">
                                                     <asp:Label runat="server" ID="lblt_eval_End_date" CssClass="control-label text-bold">End Date</asp:Label>
                                                   </div>
                                                  
                                                  <div class="col-sm-4">
                                                       <asp:Label runat="server" ID="lbl_evaluation_EndDate" CssClass="control-label text-bold"></asp:Label>
                                                  </div>  
                                              </div>

                                               <div  id="EVAL_2" runat="server" style="display:block;" class="bg-gray-light">

                                                    <hr style="border-color:#000000" />

                                                   <div class="form-group row">
                                                       <br />
                                                       <div class="col-sm-12" style="padding-left:30px;">   
                                                           
                                                            <asp:Label runat="server" ID="lblt_rounds" CssClass="control-label text-bold">Evaluation Rounds</asp:Label><br />
                                                             
                                                                               <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_rounds" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_EVALUATION_ROUND">
                                                                                            <Columns>                                                                                                                                                                                                                                                                                                 
                                                                                                 
                                                                                                <telerik:GridBoundColumn DataField="ID_ROUND"
                                                                                                    FilterControlAltText="Filter ID_ROUND column"
                                                                                                    HeaderText="ROUND" SortExpression="ID_ROUND"
                                                                                                    UniqueName="colm_ID_ROUND">
                                                                                                </telerik:GridBoundColumn>

                                                                                                  <telerik:GridBoundColumn DataField="ID_VOTING_TYPE" 
                                                                                                        FilterControlAltText="Filter ID_VOTING_TYPE column" HeaderText="" 
                                                                                                        UniqueName="colm_ID_VOTING_TYPE" Visible="true" Display="false">                                        
                                                                                                 </telerik:GridBoundColumn>
                                                                                                           
                                                                                                 <telerik:GridBoundColumn DataField="VOTING_TYPE" 
                                                                                                        FilterControlAltText="Filter VOTING_TYPE column" HeaderText="VOTING TYPE" 
                                                                                                        UniqueName="colm_VOTING_TYPE" Visible="true" >                                        
                                                                                                 </telerik:GridBoundColumn>

                                                                                                 <telerik:GridBoundColumn DataField="ROUND_START_DATE"
                                                                                                    FilterControlAltText="Filter ROUND_START_DATE column"
                                                                                                    HeaderText="START DATE" SortExpression="ROUND_START_DATE"
                                                                                                    UniqueName="colm_ROUND_START_DATE">
                                                                                                </telerik:GridBoundColumn>

                                                                                                 <telerik:GridBoundColumn DataField="ROUND_END_DATE"
                                                                                                    FilterControlAltText="Filter ROUND_END_DATE column"
                                                                                                    HeaderText="END DATE" SortExpression="ROUND_END_DATE"
                                                                                                    UniqueName="colm_ROUND_END_DATE">
                                                                                                </telerik:GridBoundColumn>

                                                                                                  <telerik:GridBoundColumn DataField="TOT_APP_SELECTED"
                                                                                                    FilterControlAltText="Filter TOT_APP_SELECTED column"
                                                                                                    HeaderText="TOTAL TO SELECTING" SortExpression="TOT_APP_SELECTED"
                                                                                                    UniqueName="colm_TOT_APP_SELECTED">
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="POINTS_TOTAL"
                                                                                                    FilterControlAltText="Filter POINTS_TOTAL column"
                                                                                                    HeaderText="TOTAL POINTS" SortExpression="POINTS_TOTAL"
                                                                                                    UniqueName="colm_POINTS_TOTAL">
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="POINTS_MAX"
                                                                                                    FilterControlAltText="Filter POINTS_MAX column"
                                                                                                    HeaderText="MAX POINTS" SortExpression="POINTS_MAX"
                                                                                                    UniqueName="colm_ROUND_END_DATE">
                                                                                                </telerik:GridBoundColumn>

                                                                                                <telerik:GridBoundColumn DataField="VOTES_MAX"
                                                                                                    FilterControlAltText="Filter VOTES_MAX column"
                                                                                                    HeaderText="MAX VOTES" SortExpression="VOTES_MAX"
                                                                                                    UniqueName="colm_VOTES_MAX">
                                                                                                </telerik:GridBoundColumn>

                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                    
                                                                                   </ContentTemplate>                                                                                
                                                                             </asp:UpdatePanel>

                                                                         <br />
                                                              </div>
                                                      
                                                     </div>
                                                                                                   
                                                     <hr style="border-color:#000000" />
                                                                                                        
                                                     <div class="form-group row">
                                                                  
                                                                  <div class="col-sm-12" style="padding-left:30px;">                                                                          
                                                                       <asp:Label runat="server" ID="lblt_team_member" CssClass="control-label text-bold">Evaluation Team:</asp:Label><br />
                                                                               <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                                                                                                                                                       
                                                                                    <telerik:RadGrid ID="grd_team" runat="server" AllowAutomaticDeletes="True"
                                                                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Width="90%">
                                                                                        <ClientSettings EnableRowHoverStyle="true">
                                                                                            <Selecting AllowRowSelect="True" />
                                                                                        </ClientSettings>
                                                                                        <MasterTableView DataKeyNames="ID_SOLICITATION_EVALUATION_TEAM">
                                                                                            <Columns>
                                                                                                                                                                                                                                                                                             
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

                                                  <%--************************DOCUMENTS**********************************--%>
                                                     <hr style="border-color:#000000" /> 

                                                              <div class="form-group row">

                                                                  <div class="col-sm-12" style="padding-left:30px;">
                                                                      <asp:Label runat="server" ID="lblt_eval_document_title" CssClass="control-label text-bold">Evaluation Documents</asp:Label><br />
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

                                         
                                        <%--******************************************EVALUATION SETTUP**********************************--%>
                                      


                                        </div><!-- /.box-body -->
                                           <div class="box-footer">
                                              <span class="pull-left"><h4 class="box-title">EVALUATION OVERVIEW</h4></span>
                                               <div class="box-tools pull-right">                                            
                                                  <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" data-target="#EVALUATION_BOX"  title="Collapse"><i class="fa fa-minus"></i></button>
                                                  <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip" data-target="#EVALUATION_BOX" title="Remove"><i class="fa fa-times"></i></button>
                                              </div>  
                                        </div><!-- /.box-footer-->
                           </div><!-- /.box -->  
                      <%--*********************************************EVALUATION OVERVIEW*********************************************************--%>
                    

                         <%--*********************************************EVALUATION ROUNDS*********************************************************--%>
                        <a name="EVA_BOX"  id="EVA_BOX"></a> 
                        <div class="box box-danger-t  collapsed-box" id="ROUND_BOX">
                                        <div class="box-header with-border">
                                          <h3 class="box-title">EVALUATION ROUND &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_round_number"  Text=""></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label runat="server" ID="lbl_round_organization"  Text=""></asp:Label></h3>
                                          <div class="box-tools pull-right">
                                            <button class="btn btn-box-tool" id="btn_ROUND_BOX" data-widget="collapse" data-toggle="tooltip" data-target="#ROUND_BOX"  title="Collapse"><i class="fa fa-plus"></i></button>
                                            <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip"  data-target="#ROUND_BOX"   title="Remove"><i class="fa fa-times"></i></button>
                                          </div>
                                        </div>
                                        <div class="box-body">                                            
                                              <div class="form-group row">
                                                  <div class="col-sm-5 text-left">
                                                     <asp:Label runat="server" ID="lblt_round_start_date" CssClass="control-label text-bold">Start Date:</asp:Label>&nbsp<asp:Label runat="server" ID="lbl_round_startDate" CssClass="control-label"></asp:Label>
                                                        <asp:HiddenField ID="H_ROUND_ID" runat="server"  Value ="0"/>   
                                                  </div>

                                                   <div class="col-sm-4 text-left">
                                                     <asp:Label runat="server" ID="lblt_round_End_date" CssClass="control-label text-bold">End Date:</asp:Label>&nbsp;<asp:Label runat="server" ID="lbl_round_endDate" CssClass="control-label"></asp:Label>
                                                   </div>                                                 
                                                 
                                                  <div class="col-sm-3 text-left">
                                                         <asp:Label runat="server" ID="lblt_Selected_Applications" CssClass="control-label text-bold">Applications to Select:</asp:Label>&nbsp;<span class="badge bg-aqua"><asp:Label runat="server" ID="lbl_app_tot" CssClass="control-label"></asp:Label></span>  
                                                  </div>
                                                                                                   
                                              </div>
                                              
                                               <div class="form-group row">
                                                  <div class="col-sm-4 text-left">
                                                     <asp:Label runat="server" ID="lblt_voting_type" CssClass="control-label text-bold">Voting Type:</asp:Label>&nbsp;<span class="badge bg-aqua"><asp:Label runat="server" ID="lbl_voting_type" CssClass="control-label"></asp:Label></span>
                                                   </div>                                                  
                                                   
                                                   <div class="col-sm-2 text-right">
                                                      <asp:Label runat="server" ID="lblt_votes" CssClass="control-label text-bold">Votes per round:</asp:Label>&nbsp;<span class="badge bg-aqua"><asp:Label runat="server" ID="lbl_tot_votes" CssClass="control-label"></asp:Label></span>
                                                   </div>                                                  
                                                   
                                                    <div class="col-sm-3 text-right">
                                                      <asp:Label runat="server" ID="lblt_Total_Points" CssClass="control-label text-bold">Total Points:</asp:Label>&nbsp;<span class="badge bg-aqua"><asp:Label runat="server" ID="lbl_total_points" CssClass="control-label"></asp:Label></span>
                                                   </div>                                                  
                                                     
                                                   <div class="col-sm-3 text-right">
                                                          <asp:Label runat="server" ID="lblt_Max_points" CssClass="control-label text-bold">Max Points</asp:Label>&nbsp;<span class="badge bg-aqua"><asp:Label runat="server" ID="lbl_max_points" CssClass="control-label"></asp:Label></span>
                                                    </div>                                                  
                                                                                                     
                                               </div>
                                                    
                                                   <hr style="border-color:#000000" />
                                                 
                                                                              <div  id="app_History_eval" runat="server" style="display:block;" class=" bg-gray-light ">

                                                                                       <ul class="timeline">

                                                                                          <asp:Repeater ID="rept_ApplyDates_eval" runat="server">
                                                                                            <ItemTemplate>                                                                                            
                                                                                                <li class="time-label">
                                                                                                    <span class="<%# Eval("nColor") %>">
                                                                                                            <%# getDate_(Eval("date_created"), "D", False) %>
                                                                                                    </span>
                                                                                                </li>
                                                                                                 <asp:Repeater ID="rept_ApplyComm_eval" runat="server">
                                                                                                    <ItemTemplate>
                                                                                                         <!-- timeline item -->
                                                                                                            <li>
                                                                                                                <!-- timeline icon -->
                                                                                                                <i class="fa <%# Eval("STATUS_ICON") %>" title="<%# Eval("EVALUATION_APP_STATUS") %>&nbsp;by&nbsp;<%# Eval("USERNAME") %>"></i>
                                                                                                                <div class="timeline-item">
                                                                                                                    <span class="time"><i class="fa fa-clock-o"></i><%# getTime_(Eval("FECHA_CREA")) %></span>

                                                                                                                    <h3 class="timeline-header"> <img class="direct-chat-img <%# Eval("IMGCOLOR") %>" src="<%# Eval("USERIMAGEN") %>" alt="<%# Eval("USERNAME") %>" title="<%# Eval("USERNAME") %>">&nbsp;&nbsp;&nbsp;<a href="#"><%# Eval("USERNAME") %></a></h3>
                                                                                                                    <div class="timeline-body">
                                                                                                                        <%# Eval("EVALUATION_COMM") %>                                                                                                                       
                                                                                                                     </div>

                                                                                                                    <div class="timeline-footer">
                                                                                                                       <%-- <a class="btn btn-primary btn-xs">React</a>--%>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </li>
                                                                                                            <!-- END timeline item -->
                                                                                                     </ItemTemplate>  
                                                                                                 </asp:Repeater>  
                                                                                                
                                                                                           </ItemTemplate>
                                                                                        </asp:Repeater>          
                                                                                      </ul>  
                                                                                   <br />                                                                                        
                                                                             </div>

                                                                             <a name="EVA_WIN"  id="EVA_WIN"></a> 
                                                                             <div  id="Buttons_approve" runat="server" style="display:block; padding-left:10px;" class="bg-gray-light">

                                                                                        <div id="app_comm"  runat="server" class="form-group row" style="height:450px;" >
                                                                                             <div class="col-sm-12 ">

                                                                                                         <asp:Label runat="server" ID="lblt_approvalComments" CssClass="control-label text-bold">Evaluation Comments</asp:Label><br /><br />
                                                                                                  
                                                                                                           <%-- <telerik:RadTextBox ID="txt_approve_comments" runat="server" Rows="5" TextMode="MultiLine" Width="97%" >
                                                                                                            </telerik:RadTextBox>  onclientLoad="OnClientLoad" --%>

                                                                                                             <telerik:RadEditor runat="server" ID="Editor_eval_comments"  Height="250" Width="97%" MaxHtmlLength="2000000" DialogsCssFile="~/Content/RadEditor_Styles.css"  >
                                                                                                                <ImageManager ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                                                    UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                                                    DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations">
                                                                                                                </ImageManager>
                                                                                                            </telerik:RadEditor>
                                                                                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor_eval_comments"  ForeColor="Red" ErrorMessage="Comment required" ValidationGroup="4"></asp:RequiredFieldValidator>
                                                                                                            <br />
                                                                                                            
                                                                                           </div>
                                                                                      </div>

                                                                                     <div class="row">
                                                                                           <div class="col-sm-12">
                                                                                               <div class="form-group">
                                                                                                    <asp:Label runat="server" ID="lblt_error" Visible="false" ForeColor="Red">Some questions are pending answers,  please complete the questions.</asp:Label>
                                                                                               </div>
                                                                                           </div>     
                                                                                      </div>

                                                                                        <div  class="form-group row" >

                                                                                                 <div class="col-sm-3">       
                                                                                                     <asp:LinkButton ID="btnlk_comment" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="99%" class="btn btn-default btn-sm margin-r-5 pull-left" data-toggle="Accept"  ValidationGroup="4" ><i id="i_comment" class="fa fa-comment fa-2x" ></i>&nbsp;&nbsp;Comment</asp:LinkButton><br /><br />
                                                                                                     <div class="btn-toolbar" role="toolbar" aria-label="Options Comments">
                                                                                                         <div class="btn-group btn-group" aria-label="Internal group" >
                                                                                                              <button type="button" class="btn btn-default" onclick="ChangeATTR('btn btn-default btn-sm margin-r-5 pull-left','fa-solid fa-comments fa-2x','Comment',1)"><i class="fa-solid fa-comments"></i></button>
                                                                                                              <button type="button" class="btn btn-warning" onclick="ChangeATTR('btn btn-warning btn-sm margin-r-5 pull-left','fa fa-warning fa-2x','Warn',2)"><i class="fa fa-warning"></i></button>
                                                                                                              <button type="button" class="btn btn-danger" onclick="ChangeATTR('btn btn-danger btn-sm margin-r-5 pull-left','fa fa-regular fa-hand fa-2x','Disagree',3)"> <i class="fa fa-regular fa-hand"></i></button>
                                                                                                         </div>
                                                                                                         <div class="btn-group" aria-label="External group" >
                                                                                                             <button type="button" class="btn btn-success" onclick="ChangeATTR('btn btn-success','fa fa-mail-forward fa-2x','Request Information',4)"><i class="fa fa-mail-forward"></i></button>
                                                                                                         </div>                                                                                            
                                                                                                     </div>
                                                                                                 </div>

                                                                                                 <div class="col-sm-3">                                                                                                     
                                                                                                      <asp:LinkButton ID="btnlk_test" runat="server" AutoPostBack="True" SingleClick="true"  Text="test" Width="80%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Vote" Visible="false" ><i class="fa fa-television fa-2x"></i>&nbsp;&nbsp;TESTING</asp:LinkButton>  
                                                                                                      <asp:LinkButton ID="btnlk_evaluate" runat="server" AutoPostBack="True" SingleClick="true"  Text="Evaluate Application" Width="80%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Vote" ><i class="fa fa-edit fa-2x"></i>&nbsp;&nbsp;Evaluate Application</asp:LinkButton>                                          
                                                                                                      <asp:LinkButton ID="btnlk_accept_Evaluation" runat="server" AutoPostBack="True" SingleClick="true"  Text="Hold" Width="80%" class="btn btn-primary btn-sm margin-r-5 pull-left" Visible="false" data-toggle="Hold"  ><i class="fa fa-thumbs-o-up fa-2x"></i>&nbsp;&nbsp;Accept Evaluation</asp:LinkButton>                                          
                                                                                                      <asp:LinkButton ID="btnlk_testII" runat="server" OnClientClick="Disabled_Buttons();" AutoPostBack="True" Text="testII" Width="80%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Vote" Visible="false" ><i class="fa fa-television fa-2x"></i>&nbsp;&nbsp;TESTING II</asp:LinkButton>  
                                                                                                  </div>

                                                                                                  <div class="col-sm-2">                                                                                                     
                                                                                                      <asp:LinkButton ID="bntlk_accept" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="99%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Vote" ValidationGroup="4" ><i class="fa fa-hand-pointer-o fa-2x"></i>&nbsp;&nbsp;Vote</asp:LinkButton>                                          
                                                                                                      <asp:LinkButton ID="btnlk_OK" runat="server" AutoPostBack="True" SingleClick="true"  Text="Hold" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left" data-toggle="Hold"   ValidationGroup="4"  ><i class="fa fa-thumbs-o-up fa-2x"></i>&nbsp;&nbsp;Accept</asp:LinkButton>                                          
                                                                                                  </div>
                                                                                            
                                                                                                 <div class="col-sm-2">                                                                                                     
                                                                                                      <asp:LinkButton ID="btnlk_Untied" runat="server" AutoPostBack="True" SingleClick="true"  Text="Reject" Width="99%" class="btn btn-success btn-sm margin-r-5 pull-left disabled" data-toggle="Reject"  ValidationGroup="4" ><i class="fa fa-hand-scissors-o fa-2x"></i>&nbsp;&nbsp;Untied</asp:LinkButton>                                          
                                                                                                      <asp:LinkButton ID="btnlk_DISMISS" runat="server" AutoPostBack="True" SingleClick="true"  Text="Hold" Width="99%" class="btn btn-danger btn-sm margin-r-5 pull-left" data-toggle="Hold"   ValidationGroup="4"  ><i class="fa fa-thumbs-o-down fa-2x"></i>&nbsp;&nbsp;Dismiss</asp:LinkButton>                                  
                                                                                                 </div>

                                                                                                 <div class="col-sm-4 text-right">                                                                                                     
                                                                                                 
                                                                                                     <asp:LinkButton ID="btnlk_Untied_Review" runat="server" AutoPostBack="True" SingleClick="true"  Text="Reject" Width="60%" class="btn btn-success btn-sm margin-r-5 pull-left disabled" data-toggle="Reject"  ValidationGroup="4" ><i class="fa fa-hand-scissors-o fa-2x"></i>&nbsp;&nbsp;Agree</asp:LinkButton>

                                                                                                     <div id="div_aggregate" runat="server" style="display:none;">
                                                                                                        <input type="text" id="sl_points" value="" class="slider form-control top" data-slider-min="1" data-slider-max="<%= MAX_POINTS_SEL %>" data-slider-step="1" data-slider-value="0" data-slider-orientation="horizontal" data-slider-selection="before" data-slider-tooltip="show" data-slider-id="red"><br />
                                                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="sl_points"  ForeColor="Red" ErrorMessage="Points required" ValidationGroup="5"></asp:RequiredFieldValidator>--%>                                                                                                         
                                                                                                        <asp:LinkButton ID="btnlk_Aggregate" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="60%" class="btn btn-success btn-sm margin-r-5 pull-left disabled hide" data-toggle="Export"  ValidationGroup="4"   ><i class="fa fa-plus-square fa-2x"></i>&nbsp;&nbsp;Aggregate</asp:LinkButton><input type="text" readonly="true" id="point_values" value="0" style="width:20%" ><br />
                                                                                                         <asp:Label runat="server" ID="lblt_values_err" CssClass="control-label text-red text-bold pull-right" Visible="false" Text="*Value must be greater than zero"></asp:Label><br />
                                                                                                    </div>                                                                                               

                                                                                                 </div>
                                                                                                

                                                                                              </div>
                                                                                              <br />
                                                                                               
                                                                                    </div>  
                                              

                                        </div><!-- /.box-body -->
                                        <div class="box-footer">
                                           <span class="pull-left"><h4 class="box-title">EVALUATION ROUND</h4></span>
                                             <div class="box-tools pull-right">                                            
                                                    <button class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" data-target="#ROUND_BOX"  title="Collapse"><i class="fa fa-minus"></i></button>
                                                   <button class="btn btn-box-tool" data-widget="remove" data-toggle="tooltip"  data-target="#ROUND_BOX"   title="Remove"><i class="fa fa-times"></i></button>
                                             </div>    
                                        </div><!-- /.box-footer-->
                           </div><!-- /.box -->  
                      <%--*********************************************EVALUATION ROUNDS*********************************************************--%>
                    



                    </div><%--<div  id="dvTab">--%>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        
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

                        <telerik:RadWindow InitialBehaviors="Maximize" RenderMode="Lightweight" runat="server" Width="1024" Height="800" id="rdEvaluation" VisibleOnPageLoad="false">
                             <ContentTemplate>   
                                  <div  id="dv_answers" runat="server" style="display:block;" class="box" >

                                         <div class="box-header bg-orange" >
                                               <h4 class="modal-title" runat="server" id="H2"><asp:Label runat="server" ID="lblt_Eval_Round">ASSESSMENT</asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_round" >#1</asp:Label>  &nbsp;&nbsp;&nbsp;</h4>
                                               <h4 class="modal-title" runat="server" id="H3"><asp:Label runat="server" ID="lblt_Applicant">APPLICANT: </asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_asses_organization" ></asp:Label></h4>
                                               <h4 class="modal-title" runat="server" id="H4"><asp:Label runat="server" ID="lblt_amount">PROPOSAL AMOUNT: </asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_amount_LOC" ></asp:Label>&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_amount" ></asp:Label></h4>
                                          </div>    
                                          <div class="box-body">
                                         
                                                <div class="row">
                                                      <div class="col-sm-12">
                                                           <div class="form-group">


                                                              <telerik:RadGrid ID="grd_screening" runat="server" AllowAutomaticDeletes="True" PageSize="30"
                                                                  AllowSorting="True" AutoGenerateColumns="False" >
                                                                  <ClientSettings EnableRowHoverStyle="true">
                                                                      <Selecting AllowRowSelect="True"></Selecting>                                                                                                                                       
                                                                  </ClientSettings>
                                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_question, id_measurement_theme, id_measurement_title, id_measurement_answer_scale, id_measurement_question_config, id_measurement_question_eval" AllowAutomaticUpdates="True">

                                                                       <GroupByExpressions>
                                                                            <telerik:GridGroupByExpression>
                                                                                <SelectFields>
                                                                                     <telerik:GridGroupByField FieldAlias="theme_name" FieldName="theme_name" HeaderText="(*)" 
                                                                                        HeaderValueSeparator=" : "></telerik:GridGroupByField>
                                                                                    <telerik:GridGroupByField FieldAlias="title_name" FieldName="title_name" HeaderText="Cat" 
                                                                                        HeaderValueSeparator=" : "></telerik:GridGroupByField>
                                                                                </SelectFields>
                                                                                <GroupByFields>
                                                                                    <telerik:GridGroupByField FieldName="order_number" SortOrder="Ascending"></telerik:GridGroupByField>
                                                                                    <telerik:GridGroupByField FieldName="title_name" SortOrder="Ascending"></telerik:GridGroupByField>
                                                                                </GroupByFields>
                                                                            </telerik:GridGroupByExpression>
                                                                         </GroupByExpressions>
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
                                                                              DataType="System.Int32" HeaderText="#" ReadOnly="True"
                                                                               UniqueName="order_numberQU" Visible="true" Display="true">
                                                                            </telerik:GridBoundColumn>

                                                                           <telerik:GridBoundColumn DataField="autofill"
                                                                              DataType="System.Int32" HeaderText="autofill" ReadOnly="True"
                                                                               UniqueName="autofill" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>

                                                                           <telerik:GridBoundColumn DataField="register_value"
                                                                              DataType="System.Int32" HeaderText="autofill" ReadOnly="True"
                                                                               UniqueName="register_value" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>

                                                                          <telerik:GridBoundColumn DataField="percent_valueMa"
                                                                               HeaderText="percent_valueMa" ReadOnly="True"
                                                                               UniqueName="percent_valueMa" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>

                                                                         
                                                                           <telerik:GridBoundColumn DataField="theme_name"
                                                                               HeaderText="theme_name" ReadOnly="True"
                                                                               UniqueName="theme_name" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>

                                                                          <telerik:GridBoundColumn DataField="percent_valueQU"
                                                                               HeaderText="percent_valueQU" ReadOnly="True"
                                                                               UniqueName="percent_valueQU" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>
                                                                          
                                                                            <telerik:GridBoundColumn DataField="id_measurement_question_config"
                                                                              DataType="System.Int32" HeaderText="id_measurement_question_config" ReadOnly="True"
                                                                               UniqueName="id_measurement_question_config" Visible="true" Display="false">
                                                                            </telerik:GridBoundColumn>

                                                                          <telerik:GridBoundColumn DataField="id_measurement_question_eval"
                                                                              DataType="System.Int32" HeaderText="id_measurement_question_config_eval" ReadOnly="True"
                                                                               UniqueName="id_measurement_question_config_eval" Visible="true" Display="false">
                                                                          </telerik:GridBoundColumn>
                                      
                                                                             <telerik:GridBoundColumn DataField="EVAL_TYPE"
                                                                              HeaderText="EVAL_TYPE" ReadOnly="True"
                                                                               UniqueName="EVAL_TYPE" Visible="true" Display="false">
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
                                                     </div>     
                                              </div>                                              

                                        </div> <%--box-body--%>

                                        <div class="box-footer">
                                              <asp:LinkButton ID="btnlk_eval" runat="server" AutoPostBack="True" SingleClick="true"  Text="Evaluate Application"  Width="30%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Vote" ><i class="fa fa-edit fa-2x"></i>&nbsp;&nbsp;Evaluate Application</asp:LinkButton>                                          
                                              <asp:LinkButton ID="btnlk_cancel_eval" runat="server" SingleClick="true"  Text="Cancel" Width="30%" class="btn btn-warning btn-sm pull-left" data-toggle="Cancel" OnClientClick="javascript:CloseRadWindowTool('');"  ><i class="fa fa-undo fa-2x"></i>&nbsp;&nbsp;Cancel Evaluation</asp:LinkButton>                                          
                                       </div>


                                   </div> <%--box--%>


                            </ContentTemplate>
                        </telerik:RadWindow>


            </div> <%--*****BOX *BODY*******--%>

        </div>

    </section>


        <script type="text/javascript">
                                                             

                              function getParameterByName(name, url) {
                                            if (!url) url = window.location.href;
                                            name = name.replace(/[\[\]]/g, '\\$&');
                                            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                                                results = regex.exec(url);
                                            if (!results) return null;
                                            if (!results[2]) return '';
                                            return decodeURIComponent(results[2].replace(/\+/g, ' '));
                                        }


                                 $(document).ready(function () {


                                      <%--  var selectedTab = $("#<%=hfTab.ClientID%>");
                                        alert('selectedTab: ' + selectedTab.val());
                                        var tabId = selectedTab.val() != "" ? selectedTab.val() : "Solicitation";
                                         alert('tabId: ' + tabId);
                                            $('#dvTab a[href="#' + tabId + '"]').tab('show');
                                            $("#dvTab a").click(function () {
                                                alert($(this).attr("href").substring(1));
                                                selectedTab.val($(this).attr("href").substring(1));
                                            });--%>

                                     var QueryVariable = getParameterByName('_tab');
                                      //alert(QueryVariable);

                                     $("#APPLICANTS").activateBox();
                                     $("#Solicitation_Tab").activateBox();
                                     $("#APPLICATION_BOX").activateBox();
                                     $("#EVALUATION_BOX").activateBox();
                                     $("#ROUND_BOX").activateBox();
                                      //$('#btn_APPLICANTS').click();

                                     console.log('Query Variable: ' +  QueryVariable);

                                     if (QueryVariable != '' && QueryVariable != null) {
                                         //$('#dvTab a[href="#' + QueryVariable + '"]').tab('show');
                                         //console.log(' "This in: btn_' + QueryVariable + '_.click()" '); 
                                         $('#btn_' + QueryVariable).click();                                      
                                     } else {
                                         //console.log(' "This: btn_APPLICANTS_.click()" '); 
                                         $('#btn_APPLICANTS').click();
                                     }                                   

                                                                        
                                      $('#sl_points').slider();

                                        $('#sl_points').on('slide', function(ev){

                                           $('#point_values').val(ev.value);
                                           var IN_COMM_TP = $('#<%= H_POINTS_VAL.ClientID%>');
                                               IN_COMM_TP.val(ev.value);
                                               //console.log('slide Value: ' +  IN_COMM_TP.val());
                                             
                                         });


                                     $('#sl_points').on("slideStop", function (event) {

                                           //console.log('slideStop value: ', event.value);
                                           $('#point_values').val(event.value);
                                           var IN_COMM_TP = $('#<%= H_POINTS_VAL.ClientID%>');
                                               IN_COMM_TP.val(event.value);
                                           //console.log(' H_POINTS_VAL: ' +  IN_COMM_TP.val());
 

                                     });



                                  <%--  $("#sl_points").on("change", function(event){
                                         //console.log('slider change value: ', event.value.oldValue, event.value.newValue);
                                        $('#point_values').val(event.newValue);
                                           var IN_COMM_TP = $('#<%= H_POINTS_VAL.ClientID%>');
                                               IN_COMM_TP.val(event.newValue);
                                         //console.log(' H_POINTS_VAL: ' +  IN_COMM_TP.val());
                                     });--%>

                                     
                                     // $("#btnlk_testII").one("click", function(event) {            
                                     //       console.log('This message will only appear once');
                                            
                                     //});

                                     //$('#btnlk_testII').on('click', function(e) {
                                     //       $(this).prop('disabled',true);
                                     //       e.preventDefault();
                                     //        console.log('This message will only appear once');
                                     //   });


                                 });  


                             function SET_Sliders() {
                                                                                                   
                                       $('#sl_points').slider();

                                        $('#sl_points').on('slide', function(ev){

                                           $('#point_values').val(ev.value);
                                           var IN_COMM_TP = $('#<%= H_POINTS_VAL.ClientID%>');
                                               IN_COMM_TP.val(ev.value);
                                               //console.log('slide Value: ' +  IN_COMM_TP.val());
                                             
                                         });


                                     $('#sl_points').on("slideStop", function (event) {

                                           //console.log('slideStop value: ', event.value);
                                           $('#point_values').val(event.value);
                                           var IN_COMM_TP = $('#<%= H_POINTS_VAL.ClientID%>');
                                               IN_COMM_TP.val(event.value);
                                           //console.log(' H_POINTS_VAL: ' +  IN_COMM_TP.val());
 

                                 });

                                   $("#APPLICANTS").activateBox();
                                   $("#Solicitation_Tab").activateBox();
                                   $("#APPLICATION_BOX").activateBox();
                                   $("#EVALUATION_BOX").activateBox();
                                   $("#ROUND_BOX").activateBox();
                                   $('#btn_ROUND_BOX').click();
                                 

            }  



            function Disabled_Buttons(){                
                 //       $(this).prop('disabled',true);
                 //       e.preventDefault();

                 <%--$('#<%= btnlk_comment.ClientID%>').addClass(bt_Attr);--%>
                 <%--$('#<%= btnlk_comment.ClientID%>').prop('value', txt_but);--%>

                console.log('This buttons are disabled addClass btnlk_eval');

               <%-- $('#<%= btnlk_testII.ClientID%>').prop('disabled', true);--%>
             <%--   $('#<%= btnlk_testii.clientid%>').addclass('disabled');--%>
                <%--$('#<%= btnlk_eval.ClientID %>').addclass('disabled');--%>
                

            }
                                    
                                 var btn_attr_ini = '';
                                 var i_attr_ini = '';

                                 function ChangeATTR(bt_Attr,i_Attr,txt_but,idTP) {
                                                                          
                                    //btnlk_comment -- btn btn-default 
                                    //i_comment -fa fa-comment fa-2x
                                     //<i id="i_comment" class="fa fa-comment fa-2x" ></i>&nbsp;&nbsp;Comment

                                     if (btn_attr_ini == '')
                                         btn_attr_ini = 'btn btn-default btn-sm margin-r-5 pull-left';

                                      if (i_attr_ini == '')
                                           i_attr_ini = 'fa fa-comment fa-2x';
                                                                                                            

                                     $('#<%= btnlk_comment.ClientID%>').removeClass(btn_attr_ini);
                                     $('#<%= btnlk_comment.ClientID%>').addClass(bt_Attr);

                                     <%--$('#<%= btnlk_comment.ClientID%>').prop('value', txt_but);--%>
                                                                                                              
                                     //console.log('ini:' + btn_attr_ini + ' new:' + bt_Attr);

                                      //$('#i_comment').removeClass(i_attr_ini);
                                      //$('#i_comment').addClass(i_Attr);

                                     var vIcon = '<i class="' + i_Attr + '" ></i>&nbsp;&nbsp;' + txt_but;
                                     
                                     //console.log('ini:' + i_attr_ini + ' new:' +  vIcon);

                                      $('#<%= btnlk_comment.ClientID%>').html( vIcon );

                                     btn_attr_ini = bt_Attr;
                                     i_attr_ini = i_Attr;
                                                                           
                                     var IN_COMM_TP = $('input[id*=H_COMM_TYPE]');

                                     //console.log('ini TYPE:' +  IN_COMM_TP.val());
                                     IN_COMM_TP.val(idTP);
                                     //console.log('end TYPE:' +  IN_COMM_TP.val());
                                     
                                 }

                             
                              function OpenRadWindowTool(url) {
                  
                                  var div = $('#EVA_WIN')[0]; 

                                        var divOffset = offset(div);
                                        console.log('this are the offsets: ' +  divOffset.left, divOffset.top);

                                        var oWnd = $find("<%= rdEvaluation.ClientID %>");
                                        //console.log("http://rms.ftfyla.com/RMS_SIME/Deliverable/frm_DeliverableFollowingRep.aspx?ID=" + idDeliverable);
                                        oWnd.moveTo(divOffset.left, divOffset.top - 250);
                                        oWnd.add_close(OnClientClose_Tool); //set a function to be called when
                                        oWnd.set_title('ASSESSMENT -- ' + '<%= ASSESSMENT_TITTLE %>');
                                        oWnd.show();
                                        oWnd.setSize(1024, 800);
                                        //oWnd.setUrl(url); //'frm_DeliverableFollowingRep.aspx?ID=' + id
                                        //oWnd.setUrl('http://www.yahoo.com');                                       
                                        oWnd.minimize();
                                        oWnd.maximize();
                                        oWnd.restore();  
                  
                                       //set_Calendar_Vars('EvalROUND_StartDate', 'HEvalROUND_StartDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
                                       //set_Calendar_Vars('EvalROUND_EndDate', 'HEvalROUND_EndDate', 'HEvalSETT_StartDate','HEvalSETT_EndDate');
                        
                        }


                                function CloseRadWindowTool(url) {

                                     var oWnd = $find("<%= rdEvaluation.ClientID %>");
                                     oWnd.close();
                                     //set_Calendars();

                                }

                             function OnClientClose_Tool(oWnd) {  
                                // oWnd.setUrl("about:blank"); // Sets url to blank  
                                 oWnd.remove_close(OnClientClose_Tool);   
                                 // set_Calendars();
                             }       


                            function offset(el) {
                                var rect = el.getBoundingClientRect(),
                                scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
                                scrollTop = window.pageYOffset || document.documentElement.scrollTop;
                                return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
                            }
            


     </script>

    
    <script>
        function FuncModatTrim() {
            $('#modalTasaCambio').modal('show');
        }
    </script>
</asp:Content>
