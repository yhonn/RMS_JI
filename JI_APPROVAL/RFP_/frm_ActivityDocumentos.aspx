<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityDocumentos.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityDocumentos" %>

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
        <div class="box">
            <div class="box-header with-border">               
                 <div class="col-sm-11">   
                     <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activity - Documents</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lbl_id_ficha_aw" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
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


                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <asp:Label ID="LBL_ID_AWARD" runat="server" Text="" Visible="false" />

                            <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_definicion" class="hidden" href="#">ACTIVITY</a></li>                                
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>   
                                <li role="presentation" class="active"><a class="primary" runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>    
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">                         
                            

                                <div class="form-group row">
                                                                     
                                     <div class="col-sm-12">                                         

                                                <div class="col-md-4 col-sm-6 col-xs-16">
                                                  <div class="info-box">
                                                      <span class="info-box-icon bg-orange-active"><i class="fas fa-sack-dollar"></i></span>
                                                      <div class="info-box-content">
                                                           <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT2_usd" Text ="0"></asp:Label></span>
                                                           <span class="text-bold">Total Solicitation</span>             
                                                      </div><!-- /.info-box-content -->
                                                  </div><!-- /.info-box -->
                                               </div><!-- /.col -->

                                               <div class="col-md-4 col-sm-6 col-xs-16 "  >                                                                                                                                                                                                                        
                                                  
                                                   <div style="width: 250px; height: 200px; margin: 0 auto">
                                                      <div id="container-money" style="width: 250px; height: 160px; margin: 0 auto; text-align: center;"></div>
                                                      <input style="display: none;" id="valor_avance" runat="server" />
                                                      <h3 runat="server" id="txt_avance" style="margin: 0 auto;text-align: center;font-family: Tahoma;font-weight: 600; font-size: 15px;"></h3>
                                                  </div>
                                                                                                              
                                               </div> 
                                                                                                      

                                               <div class="col-md-4 col-sm-6 col-xs-16 ">
                                                  <div class="info-box">
                                                      <span class="info-box-icon bg-orange-active"><i class="fa-solid fa-handshake-angle"></i></span>
                                                      <div class="info-box-content">
                                                           <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalPerf2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalPerf2_usd" Text =""></asp:Label></span>
                                                           <span class="text-bold">Total Amount Awarded</span>             
                                                      </div><!-- /.info-box-content -->
                                                  </div><!-- /.info-box -->
                                               </div><!-- /.col -->                                      

                                     </div>                                                                       

                               </div>  
                            

                            
                            <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>
                            <div class="form-group row">   
                               
                               <div class="col-lg-1"></div>
                               <div class="col-lg-2">
                                   <asp:Label runat="server" ID="lblt_contract_selector" CssClass="control-label text-bold">CONTRACT</asp:Label>
                               </div>
                               <div class="col-lg-9">
                                   
                                               <!--Control -->
                                                             <telerik:RadComboBox  ID="cmb_awards" AutoPostBack="true" 
                                                                                   runat ="server" 
                                                                                    CausesValidation="False"                                                                     
                                                                                    EmptyMessage="Select an award..."   
                                                                                    AllowCustomText="true" 
                                                                                    Filter="Contains"                                                  
                                                                                    Height="200px"
                                                                                    Width="95%"
                                                                                    OnDataBound="cmb_awards_DataBound"                                                                                   
                                                                                    OnClientItemsRequested="UpdateItemCountField_aw"                                                                                                                                             
                                                                                     >                                                              
                                                                            <HeaderTemplate>
                                                                                    <ul>
                                                                                        <li style="font-weight:700;" >Contract Code / Status</li>
                                                                                        <li style="font-weight:100;" >Contract Mecanism</li> 
                                                                                        <li style="font-weight:100;" >Applicant</li>                                                                                                                                                                                                                                                                                                                       
                                                                                    </ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li style="font-weight:700;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "AWARD_CODE")%> -- <%# DataBinder.Eval(Container.DataItem, "AWARD_STATUS")%> 
                                                                                    </li>
                                                                                    <li style="font-weight:100;" >
                                                                                        <span style="font-weight:400;" > <%# DataBinder.Eval(Container.DataItem, "nombre_proyecto")%> </span>
                                                                                    </li>
                                                                                    <li style="font-weight:700; color:#ED7620;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "ORGANIZATIONNAME")%>  
                                                                                    </li>                                                                                                                                                                   
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            A total of
                                                                            <asp:Literal runat="server" ID="RadComboItemsCount_award" />
                                                                            items
                                                                        </FooterTemplate>
                                                            </telerik:RadComboBox>
                                                                              

                                       </div>

                            </div>
                                                      
                            <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>
                                                            
                               <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>

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
                                                              <span class="info-box-icon bg-gray"><i class="fas fa-calendar-check"></i></span>
                                                              <div class="info-box-content">
                                                                 <span class="info-box-text"><asp:Label ID="lbl_last_Deliverable" runat="server"></asp:Label></span>
                                                                 <span class="text-bold"><asp:Label ID="lbl_status_Deliverable" runat="server"></asp:Label>&nbsp;&nbsp;<i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>             
                                                                 <%--&nbsp;--&nbsp;<asp:Label ID="lblt_date_status" runat="server"></asp:Label><br /><i class="fa fa-clock-o"></i><asp:Label ID="lblt_time_status" runat="server"></asp:Label>--%>
                                                              </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                      </div><!-- /.col -->                                                   
                                                </div>

                                           </div>

                                                  
                               </div>


                                 <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>

                             <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>







                                  <div class="form-group row">
                                       <div class="col-sm-3 text-right">
                                           <asp:Label runat="server" ID="lblt_Tittle" CssClass="control-label text-bold">Document Tittle</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                             <telerik:RadTextBox ID="txt_document_tittle" runat="server" Width="250px" MaxLength="250">
                                             </telerik:RadTextBox>
                                         </div>
                                  </div>

                                   <div class="form-group row">
                                           <div class="col-sm-3 text-right">
                                               <asp:Label runat="server" ID="lblt_Type_document" CssClass="control-label text-bold">Document Type</asp:Label>
                                            </div>
                                            <div class="col-sm-9">
                                                <telerik:RadComboBox ID="cmb_type_of_document" runat="server" Width="300px" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox>
                                             </div>
                                    </div>

                                      <div class="form-group row">
                                           <div class="col-sm-3 text-right">
                                               <asp:Label runat="server" ID="lblt_documento" CssClass="control-label text-bold">Attach Document</asp:Label>
                                            </div>
                                            <div class="col-sm-9">
                                              <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                                    TemporaryFolder  ="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded" />

                                                    <telerik:RadButton ID="btn_agregar" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                                        Text="Add Document" ValidationGroup="2" Width="75px">
                                                    </telerik:RadButton><br />
                                             
                                              
                                             </div>
                                      </div>
                               
                                    <style type="text/css">
                                                                              
                                        .wrapWord { 
                                                    word-wrap: break-word;
                                                    word-break:break-all; 
                                                  }
                    
                                    </style>
                                    <div class="form-group row">

                                      <div class="col-sm-12">

                                                <asp:UpdatePanel ID="PanelFirma" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <telerik:RadGrid ID="grd_archivos" runat="server" 
                                                            AllowAutomaticDeletes="True"
                                                            AutoGenerateColumns="False" 
                                                            CellSpacing="0" 
                                                            GridLines="None" 
                                                            Width="100%"
                                                            AllowPaging="True" 
                                                            PageSize="15"  
                                                            AllowSorting="True" 
                                                            AllowFilteringByColumn="true"
                                                            >
                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                <Selecting AllowRowSelect="True" />
                                                            </ClientSettings>
                                                            <MasterTableView DataKeyNames="ID_ACTIVITY_AW_DOCUMENTS">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn 
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
                                                                      <telerik:GridBoundColumn DataField="DOCUMENTROLE"
                                                                        FilterControlAltText="Filter DOCUMENTROLE column"
                                                                        HeaderText="Document Source" SortExpression="DOCUMENTROLE"
                                                                        UniqueName="colm_DOCUMENTROLE">
                                                                    </telerik:GridBoundColumn>
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
                                                                       <ItemStyle Width="35%" CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                        <br />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                        
                                          </div>

                                  </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_continue" runat="server" Text="Continue" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
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
                 

    </section>

    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js")%>"></script>     
    <script src="<%=ResolveUrl("~/Content/dist/js/aw_chart.js?time=0.0002")%>"></script>
   

    <script>

          function UpdateItemCountField_aw(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " contracts";
            }

        function FuncModatTrim() {
            $('#modalTasaCambio').modal('show');
        }
                     
                
  <%--      function DateChange(sender, args) {
           $find("<%= RadDateTimePicker1.ClientID %>").showTimePopup();
        }--%>

       <%--    function ShowDateTimePopup(sender, eventArgs) {
                var picker = $find("<%= RadDateTimePicker1.ClientID %>");
                var userChar = eventArgs.get_keyCharacter();
                if (userChar == '@') {
                    picker.showPopup();
                    eventArgs.set_cancel(true);@
                }
                else if (userChar == '#') {
                    picker.showTimePopup();
                    eventArgs.set_cancel(true);
                }
            }--%>

<%--        function getTime() {
            var picker = $find("<%= RadDateTimePicker1.ClientID %>");
            var view = picker.get_timeView();
            alert(view.getTime());
        }    --%>  

    </script>
</asp:Content>
