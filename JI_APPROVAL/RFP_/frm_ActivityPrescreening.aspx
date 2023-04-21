﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityPrescreening.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityPrescreening" %>

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

   
        <script  type="text/javascript">


            function OnClientLoad(sender, args)  
                {  
                    //calculate myHeight and myWidth here  
                    sender.get_contentAreaElement().style.height ='200px';  
                   // sender.get_contentAreaElement().style.width = '97%';  
                }  
 

        </script>
        
        <div class="box">
            <div class="box-header with-border">               
                 <div class="col-sm-11">   
                     <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Applicants - PreScreening</asp:Label>
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

                <div class="col-lg-12">
                    <div  id="dvTab" class="box-body">
                        <div class="col-lg-12">
                                <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_definicion" class="hidden" href="#">ACTIVITY</a></li>                               
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation" class="active" ><a class="primary" runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation" ><a runat="server" id="alink_submission">APPLICATION</a></li>
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
                        <asp:Label ID="lbl_id_sol_app" runat="server" Text="" Visible="false" />

                             <asp:HiddenField ID="hfTab" runat="server" />  
                             <asp:HiddenField ID="Hiddenindi" runat="server" />
                      
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

                                     if (QueryVariable != '' && QueryVariable != null) {
                                         $('#dvTab a[href="#' + QueryVariable + '"]').tab('show');
                                     }
                                            



                                    });                    

                            </script>


                        <div class="panel-body div-bordered">      
                            
                                  <div id="SolicitationALERT" runat="server"  class="form-group row" style="display:none;">
                                     <div class="col-sm-7">
                                             <div class="alert alert-info alert-dismissable">
                                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                <h4><i class="icon fa fa-info"></i> The Application has been successfully submitted</h4>
                                                    You can go through the evaluation process to follow it up.
                                              </div>
                                     </div>
                                   </div>

                                    <div id="SolicitationRejected" runat="server"  class="form-group row" style="display:none;">
                                     <div class="col-sm-7">
                                             <div class="alert alert-danger alert-dismissable">
                                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                <h4><i class="icon fa fa-info"></i> The Application has been received</h4>
                                                    The preliminary review did not meet the requirements mentioned for proceeding with the process. Thanks for submitting the proposal, and we will keep posting for any further project opportunities.
                                              </div>
                                     </div>
                                   </div>
                            
                              <br />  
                            
                              <div id="ADDons" runat="server" class="box-body" style="border:1px solid thick; display:block;">

                                  <p id="_Documents">&nbsp;</p>
                                   <div class="box-header"  id="Tabs_options">
                                       <ul class="nav nav-tabs">
                                         <li class="active"><a data-toggle="tab" href="#Solicitation"><asp:Label runat="server" ID="lblt_Solicitation" CssClass="control-label text-bold">Solicitation</asp:Label></a></li>                                    
                                         <li><a data-toggle="tab" href="#Applicants"><asp:Label runat="server" ID="lblt_Applicants" CssClass="control-label text-bold">Applicants</asp:Label></a></li>      
                                         <li><a data-toggle="tab" href="#Applications"><asp:Label runat="server" ID="lblt_Applications" CssClass="control-label text-bold">PreScreening</asp:Label></a></li>      
                                          <%--       <li><a data-toggle="tab" href="#Applicants"><asp:Label runat="server" ID="lblt_Applicants" CssClass="control-label text-bold">Applicants</asp:Label></a></li>
                                              --%>         
                                       </ul>
                                  </div>

                                  <asp:HiddenField ID="TabName" runat="server" Value="Solicitation" />

                                    <div class="tab-content">

                                             <div id="Solicitation" class="tab-pane fade in active">
                                             
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
                                                                                        <div class="col-sm-8">
                                                                                            <telerik:RadTextBox ID="txt_tittle" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="2000" ReadOnly="true"  BorderStyle="None">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                                                ControlToValidate="txt_tittle" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                        </div>
                                                                                     </div>
                                                                                      <br />
                                                                                      <div class="form-group row">
                                                                                        <div class="col-sm-2 text-right">
                                                                                            <asp:Label runat="server" ID="lbl_purpose" CssClass="control-label text-bold">Purpose</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-sm-8">
                                                                                            <telerik:RadTextBox ID="txt_purpose" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="2000"  ReadOnly="true" BorderStyle="None">
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
                                                                                        <div class="col-sm-8">
                                                                                            <telerik:RadTextBox ID="txt_solicitation" runat="server" Rows="7" TextMode="MultiLine" Width="95%" MaxLength="5000"   ReadOnly="true" BorderStyle="None">
                                                                                            </telerik:RadTextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                                ControlToValidate="txt_solicitation" CssClass="Error" Display="Dynamic"
                                                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                        </div>
                                                                                     </div>

                                                                               
                                                                                           <hr /> 

                                                                                          <div class="form-group row">

                                                                                              <div class="col-sm-11">

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
                                                                                                                <br />
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                        
                                                                                                  </div>

                                                                                          </div>
                                                                                                                           
                                                                                           <div class="form-group row">

                                                                                               <div class="col-sm-1 text-right">
                                                                                     
                                                                                                </div>

                                                                                           </div>  

                                                      </div>

                                        
                                        <%--************************************************************************APPLICANTS************************************************************--%>


                                            <div id="Applicants" class="tab-pane fade">
                                             
                                                     <br />
                                                        <div class="form-group row">
                                                        

                                                                  <div class="col-sm-12">

                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>


                                                                                     <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" AllowSorting="true"
                                                                                            CellSpacing="0" GridLines="None" PageSize="6" AllowPaging="true" Width="100%"  Height="500px" 
                                                                                            AutoGenerateColumns="False">
                                                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                                   <Resizing AllowColumnResize="true"  />
                                                                                                   <Scrolling AllowScroll="True" SaveScrollPosition="True" />
                                                                                            </ClientSettings>
                                                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_ORGANIZATION_APP" AllowAutomaticUpdates="True">

                                                                                                <Columns>
                                                                                                    <telerik:GridBoundColumn DataField="ID_ORGANIZATION_APP"
                                                                                                        FilterControlAltText="Filter ID_ORGANIZATION_APP column"
                                                                                                        SortExpression="ID_ORGANIZATION_APP" UniqueName="ID_ORGANIZATION_APP"
                                                                                                        Visible="False" DataType="System.Int32" HeaderText="ID_ORGANIZATION_APP"
                                                                                                        ReadOnly="True">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ID_SOLICITATION_APP"
                                                                                                        UniqueName="ID_SOLICITATION_APP"
                                                                                                        Visible="true" Display="false" HeaderText="ID_SOLICITATION_APP" >
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

                                                                                                <%--    <telerik:GridTemplateColumn FilterControlAltText="Filter delete column" UniqueName="colm_delete" Visible="true">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="col_hlk_delete" runat="server" Width="10"
                                                                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                                                                                                    OnClick="Elimina_Elemento">
                                                                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                                                                </asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderStyle Width="5px" />
                                                                                                            <ItemStyle Width="10px" />
                                                                                                    </telerik:GridTemplateColumn>--%>

                                                                                                      <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                                                        UniqueName="colm_select" Visible="true">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="True"
                                                                                                                OnCheckedChanged="Select_Applicant" />
                                                                                                            <cc1:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server"
                                                                                                                CheckedImageUrl="../Imagenes/iconos/accept.png" ImageHeight="22" ImageWidth="22"
                                                                                                                TargetControlID="chkSelected" UncheckedImageUrl="../Imagenes/iconos/icon-warningAlert.png">
                                                                                                            </cc1:ToggleButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle Width="30px" />
                                                                                                        <ItemStyle Width="30px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                    <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_Edit" Visible="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar ejecutor" Target="_self" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="5px" />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                    <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print" Visible="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir detalles" Target="_blank" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="5px" />
                                                                                                    </telerik:GridTemplateColumn>
                                                                                                    <telerik:GridBoundColumn DataField="SOLICITATION_APP_CODE"
                                                                                                        FilterControlAltText="Filter SOLICITATION_APP_CODE column"
                                                                                                        HeaderText="CODE" SortExpression="SOLICITATION_APP_CODE"
                                                                                                        UniqueName="colm_SOLICITATION_APP_CODE">
                                                                                                         <HeaderStyle Width="150px" />
                                                                                                          <ItemStyle Width="150px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                    <telerik:GridTemplateColumn 
                                                                                                          HeaderText="&nbsp;" UniqueName="colm_ORG" >
                                                                                                         <ItemTemplate>                                     
                                                                                                             <asp:Label runat="server" ID="lblt_organization_n" CssClass="control-label text-bold" Text="Organization: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_organization_n" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                             <asp:Label runat="server" ID="lblt_representative" CssClass="control-label text-bold" Text="Representative: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_representative" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                             <asp:Label runat="server" ID="lbl_email" CssClass="control-label" Text=""></asp:Label>&nbsp;-&nbsp;<asp:Label runat="server" ID="lbl_tyep" CssClass="control-label" Text=""></asp:Label><br /><br />
                                                                                                         </ItemTemplate>
                                                                                                          <HeaderStyle Width="275px" />
                                                                                                          <ItemStyle Width="275px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                <%--     <telerik:GridBoundColumn DataField="NAMEALIAS"
                                                                                                        FilterControlAltText="Filter NAMEALIAS column"
                                                                                                        HeaderText="Acronyms" SortExpression="NAMEALIAS"
                                                                                                        UniqueName="colm_NAMEALIAS">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="ORGANIZATIONNAME"
                                                                                                        FilterControlAltText="Filter ORGANIZATIONNAME column"
                                                                                                        HeaderText="Name" SortExpression="ORGANIZATIONNAME"
                                                                                                        UniqueName="colm_ORGANIZATIONNAME">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                    <telerik:GridBoundColumn DataField="PERSONNAME"
                                                                                                        FilterControlAltText="Filter PERSONNAME column" HeaderText="Representative"
                                                                                                        SortExpression="PERSONNAME"
                                                                                                        UniqueName="colm_PERSONNAME">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                     <telerik:GridBoundColumn DataField="ORGANIZATIONEMAIL"
                                                                                                        FilterControlAltText="Filter ORGANIZATIONEMAIL column" HeaderText="Email"
                                                                                                        SortExpression="ORGANIZATIONEMAIL"
                                                                                                        UniqueName="colm_ORGANIZATIONEMAIL">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                     <telerik:GridBoundColumn DataField="ORGANIZATION_TYPE"
                                                                                                        FilterControlAltText="Filter ORGANIZATION_TYPE column" HeaderText="ORG TYPE"
                                                                                                        SortExpression="ORGANIZATION_TYPE"
                                                                                                        UniqueName="colm_ORGANIZATION_TYPE">
                                                                                                    </telerik:GridBoundColumn>--%>

                                                                                                     <telerik:GridTemplateColumn 
                                                                                                          HeaderText="&nbsp;" UniqueName="colm_location" >
                                                                                                         <ItemTemplate>                                     
                                                                                                             <asp:Label runat="server" ID="lblt_country_n" CssClass="control-label text-bold" Text="Country: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_country_n" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                             <asp:Label runat="server" ID="lblt_City_n" CssClass="control-label text-bold" Text="City: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_City_n" CssClass="control-label" Text=""></asp:Label><br />                                                                                                             
                                                                                                         </ItemTemplate>
                                                                                                          <HeaderStyle Width="100px" />
                                                                                                          <ItemStyle Width="100px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                     <telerik:GridTemplateColumn 
                                                                                                          HeaderText="&nbsp;" UniqueName="colm_dates" >
                                                                                                         <ItemTemplate>                                     
                                                                                                             <asp:Label runat="server" ID="lblt_sent_date" CssClass="control-label text-bold" Text="Sent: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_sent_date" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                             <asp:Label runat="server" ID="lblt_received_date" CssClass="control-label text-bold" Text="Opened: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_received_date" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                             <asp:Label runat="server" ID="lblt_submitted_date" CssClass="control-label text-bold" Text="Submitted: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_submitted_date" CssClass="control-label" Text=""></asp:Label><br />
                                                                                                         </ItemTemplate>
                                                                                                          <HeaderStyle Width="300px" />
                                                                                                          <ItemStyle Width="300px" />
                                                                                                    </telerik:GridTemplateColumn>

                                                                                                   <%-- <telerik:GridBoundColumn DataField="ADDRESSCOUNTRYREGIONID"
                                                                                                        FilterControlAltText="Filter ADDRESSCOUNTRYREGIONID column"
                                                                                                        HeaderText="Country" SortExpression="COUNTRY"
                                                                                                        UniqueName="colm_ADDRESSCOUNTRYREGIONID">
                                                                                                    </telerik:GridBoundColumn>
                                                                                                      <telerik:GridBoundColumn DataField="ADDRESSCITY"
                                                                                                        FilterControlAltText="Filter ADDRESSCITY column"
                                                                                                        HeaderText="City" SortExpression="ADDRESSCITY"
                                                                                                        UniqueName="colm_ADDRESSCITY">
                                                                                                    </telerik:GridBoundColumn>--%>

                                                                                                    <telerik:GridBoundColumn DataField="SCREENING_STATUS"
                                                                                                        FilterControlAltText="Filter SCREENING_STATUS column" HeaderText="STATUS"
                                                                                                        SortExpression="SCREENING_STATUS" UniqueName="colm_SCREENING_STATUS" Visible="true">
                                                                                                      <HeaderStyle Width="100px" />
                                                                                                      <ItemStyle Width="100px" />
                                                                                                    </telerik:GridBoundColumn>

                                                                                                  <%--  <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                                                        UniqueName="colm_solicitate" Visible="true">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True"
                                                                                                                OnCheckedChanged="SOLITICITATE_APP" />
                                                                                                            <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                                                                                                CheckedImageUrl="../Imagenes/iconos/resend_email.png" ImageHeight="22" ImageWidth="22"
                                                                                                                TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/warning.gif">
                                                                                                            </cc1:ToggleButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle Width="20px" />
                                                                                                        <ItemStyle Width="20px" />
                                                                                                    </telerik:GridTemplateColumn>--%>
                                                                                                
                                                                                                </Columns>
                                                                                            </MasterTableView>
                                                                                      
                                                                                         
                                                                                     </telerik:RadGrid>
                                                                                         <br />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                        
                                                                      </div>

                                                              </div>


                                              
                                                </div>


                                           <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                      

                                              <div id="Applications" class="tab-pane fade" >
                                                <br />
                                                 
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
                                                                                               <%--alert-sm bg-blue text-center <telerik:RadComboBox ID="cmb_Apply_status" runat="server" Width="300px"  EmptyMessage="Pick one category"></telerik:RadComboBox>--%>
                                                                                               <%-- <div class="  " runat="server" id="div2" style="width: 150px;">
                                                                                                   </div>--%>
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

                                                                                  <div class="form-group row">
                                                                                       <div class="col-sm-2 text-right">
                                                                                          <asp:Label runat="server" ID="lblt_score" CssClass="control-label text-bold">Score</asp:Label>
                                                                                       </div>
                                                                                      <div class="col-sm-4">                                                                                              
                                                                                          <h4><span id="span_score" runat="server" class='label ' > <asp:Label runat="server" ID="lbl_score" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;</span><span id="spa_icons" runat="server"></span><asp:Label runat="server" ID="lbl_score_status" Text=""></asp:Label></h4>
                                                                                          <%--&nbsp;&nbsp;&nbsp;<i class="fas fa-smile"></i><i class="fas fa-frown"></i>--%>
                                                                                      </div>
                                                                                        <div class="col-sm-2 text-right">
                                                                                          <asp:Label runat="server" ID="lblt_remainin_score" CssClass="control-label text-bold">Max remaining score</asp:Label>
                                                                                       </div>
                                                                                      <div class="col-sm-4">      
                                                                                        <h3><span id="span_remaining" runat="server" class='badge bg-danger ' > <asp:Label runat="server" ID="lbl_remaining_score" Text=""></asp:Label></span></h3>
                                                                                          <asp:HiddenField runat="server" ID="hd_totScore" Value ="0" />
                                                                                          <asp:HiddenField runat="server" ID="hd_remaining" Value ="0" />
                                                                                      </div>
                                                                                  </div>


                                                                           <%--**********************************************************************************APPLICANTS PRESCREENING*************************************************************--%>
                                      
                                                                           <hr />

                                                                                         <div  id="questions_prescreening" runat="server" style="display:block;" class=" bg-gray-light ">


                                                                                              <div  id="dv_prescreening" runat="server" style="display:block;" class=" bg-gray-light ">

                                                                                                                 <telerik:RadGrid ID="grd_screening" runat="server" AllowAutomaticDeletes="True" PageSize="30"
                                                                                                                  AllowSorting="True" AutoGenerateColumns="False" >
                                                                                                                  <ClientSettings EnableRowHoverStyle="true">
                                                                                                                      <Selecting AllowRowSelect="True"></Selecting>                                                                                                                                       
                                                                                                                  </ClientSettings>
                                                                                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_question, id_measurement_answer_scale, id_measurement_question_config" AllowAutomaticUpdates="True">
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

                                                                                                                            <telerik:GridBoundColumn DataField="SCREENING_TOTALPASS"
                                                                                                                              DataType="System.Int32" HeaderText="SCREENING_TOTALPASS" ReadOnly="True"
                                                                                                                               UniqueName="SCREENING_TOTALPASS" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>

                                                                                                                            <telerik:GridBoundColumn DataField="id_measurement_question_config"
                                                                                                                              DataType="System.Int32" HeaderText="id_measurement_question_config" ReadOnly="True"
                                                                                                                               UniqueName="id_measurement_question_config" Visible="true" Display="false">
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
                                                                                                                                  <telerik:RadTextBox ID="txt_colm_SCREENING_QUESTIONS" runat="server" Rows="3" TextMode="MultiLine" Width="100%" ReadOnly="true" Resize="Both"  ></telerik:RadTextBox>
                                                                                                                              </ItemTemplate>
                                                                                                                               <ItemStyle Width="60%" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="60%" />

                                                                                                                          </telerik:GridTemplateColumn>

                                                                                                                          <telerik:GridTemplateColumn FilterControlAltText="Filter answer column" HeaderText="&nbsp;" UniqueName="colm_answer_CONTROL" >
                                                                                                                              <ItemTemplate>                                                                                                                  
                                                                                                                                  <telerik:RadComboBox ID="cmb_answer" runat="server" Width="100%" EmptyMessage="Pick the anwser" ></telerik:RadComboBox>                                                                                            
                                                                                                                                  <telerik:RadTextBox ID="txt_answer_text" runat="server" Rows="3" TextMode="MultiLine" Width="100%" Resize="Both" ></telerik:RadTextBox>
                                                                                                                                  <telerik:RadNumericTextBox  runat="server" ID="txt_answer_value" name="txt_answer_value"  Width="100%" Value="0" ></telerik:RadNumericTextBox>
                                                                                                                              </ItemTemplate>

                                                                                                                               <ItemStyle Width="40%" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="40%" />
                                                                                                                          </telerik:GridTemplateColumn>

                                                                                                                                                                 
                                                                                                                      </Columns>
                                                                                                                  </MasterTableView>
                                                                                                              </telerik:RadGrid>    
                                                                                                  
                                                                                                </div>

                                                                                               <div  id="dv_answers" runat="server" style="display:block;" >

                                                                                                    <telerik:RadGrid ID="grd_answers" runat="server" AllowAutomaticDeletes="True" PageSize="30"
                                                                                                                  AllowSorting="True" AutoGenerateColumns="False"  Width="100%" Height="1000px" ShowFooter="True" >
                                                                                                           
                                                                                                              <ClientSettings EnableRowHoverStyle="true">
                                                                                                                    <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                                                                    <Resizing AllowColumnResize="true" AllowResizeToFit="true" />                                        
                                                                                                                    <Scrolling AllowScroll="True" SaveScrollPosition="True" />
                                                                                                              </ClientSettings>

                                                                                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_question, id_measurement_answer_scale, id_measurement_question_config, id_measurement_question_eval" AllowAutomaticUpdates="True"  ShowFooter="True"  ShowGroupFooter="false" >
                                                                                                                      <Columns>

                                                                                                                          <telerik:GridBoundColumn DataField="id_measurement_question"
                                                                                                                              DataType="System.Int32" HeaderText="id_measurement_question" ReadOnly="True"
                                                                                                                               UniqueName="id_measurement_question" Visible="true" Display="false">
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                           <telerik:GridBoundColumn DataField="id_measurement_question_config"
                                                                                                                              DataType="System.Int32" HeaderText="id_measurement_question_config" ReadOnly="True"
                                                                                                                               UniqueName="id_measurement_question_config" Visible="true" Display="false">
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridBoundColumn DataField="id_measurement_question_eval"
                                                                                                                              DataType="System.Int32" HeaderText="id_measurement_question_eval" ReadOnly="True"
                                                                                                                               UniqueName="id_measurement_question_eval" Visible="true" Display="false">
                                                                                                                          </telerik:GridBoundColumn>
                                                                                                                          
                                                                                                                             <telerik:GridBoundColumn DataField="id_measurement_answer_scale"
                                                                                                                              DataType="System.Int32" HeaderText="id_measurement_answer_scale" ReadOnly="True"
                                                                                                                               UniqueName="id_measurement_answer_scale" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>

                                                                                                                           <telerik:GridBoundColumn DataField="ID_APPLY_SCREENING_ANSWER"
                                                                                                                              DataType="System.Int32" HeaderText="ID_APPLY_SCREENING_ANSWER" ReadOnly="True"
                                                                                                                               UniqueName="ID_APPLY_SCREENING_ANSWER" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                                                                                                                                                     
                                                                                                                          
                                                                                                                          <telerik:GridBoundColumn DataField="answer_type_code"
                                                                                                                              HeaderText="answer_type_code" ReadOnly="True"
                                                                                                                               UniqueName="answer_type_code" Visible="true" Display="false">
                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                          
                                                                                                                          <telerik:GridBoundColumn DataField="order_numberQC"
                                                                                                                              DataType="System.Int32" HeaderText="No" ReadOnly="True"
                                                                                                                               UniqueName="order_numberQC" Visible="true" Display="true">
                                                                                                                              <ItemStyle Width="25px" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="25px" />
                                                                                                                            </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridBoundColumn DataField="question_name" HeaderText="question_name" UniqueName="question_name" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridBoundColumn DataField="percent_valueQC" HeaderText="percent_valueQC" UniqueName="percent_valueQC" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>
                                                                  
                                                                                                                       <%--   <telerik:GridBoundColumn DataField="No" FilterControlAltText="Filter No column"
                                                                                                                              HeaderText="No" SortExpression="No" UniqueName="No" >
                                                                                                                          </telerik:GridBoundColumn>--%>
                                                                  
                                                                                                                          <telerik:GridBoundColumn DataField="measurement_answer_text" HeaderText="measurement_answer_text" UniqueName="measurement_answer_text" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridBoundColumn DataField="measurement_answer_value" HeaderText="measurement_answer_value" UniqueName="measurement_answer_value" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridTemplateColumn FilterControlAltText="Filter Question column"
                                                                                                                              HeaderText="&nbsp;" UniqueName="colm_Question" ItemStyle-Width="40%" >
                                                                                                                              <ItemTemplate>
                                                                                                                                  <telerik:RadTextBox ID="txt_colm_SCREENING_QUESTIONS" runat="server" Rows="3" TextMode="MultiLine" Width="100%" ReadOnly="true" Resize="Both"  ></telerik:RadTextBox>
                                                                                                                              </ItemTemplate>
                                                                                                                               <ItemStyle Width="250px" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="250px" />
                                                                                                                          </telerik:GridTemplateColumn>

                                                                                                                           <telerik:GridBoundColumn DataField="percent_valueAO" HeaderText="percent_valueAO" UniqueName="percent_valueAO" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                          <telerik:GridTemplateColumn FilterControlAltText="Filter answer column" HeaderText="&nbsp;" UniqueName="colm_answer_CONTROL" >
                                                                                                                              <ItemTemplate>                                                                                                                  
                                                                                                                                  <telerik:RadComboBox ID="cmb_answer" runat="server" Width="100%" EmptyMessage="Pick the answer" ></telerik:RadComboBox>                                                                                            
                                                                                                                                  <telerik:RadTextBox ID="txt_answer_text" runat="server" Rows="3" TextMode="MultiLine" Width="100%" Resize="Both"  ></telerik:RadTextBox>
                                                                                                                                  <telerik:RadNumericTextBox  runat="server" ID="txt_answer_value" name="txt_answer_value"  Width="100%" Value="0" ></telerik:RadNumericTextBox>
                                                                                                                              </ItemTemplate>
                                                                                                                                <ItemStyle Width="250px" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="250px" />
                                                                                                                          </telerik:GridTemplateColumn>
                                                                                                                                                                                                                                                    
                                                                                                                          <telerik:GridBoundColumn DataField="percent_valueAO" HeaderText="percent_valueAO" UniqueName="percent_valueAO" Visible="true" Display="false" >
                                                                                                                          </telerik:GridBoundColumn>

                                                                                                                         <telerik:GridTemplateColumn FilterControlAltText="Filter score column" HeaderText="Score" UniqueName="colm_score_CONTROL" >
                                                                                                                               <ItemTemplate>  
                                                                                                                                   <telerik:RadNumericTextBox  runat="server" ID="txt_score" name="txt_score"  Width="100%" Value="0" ></telerik:RadNumericTextBox>
                                                                                                                                </ItemTemplate>
                                                                                                                              <ItemStyle Width="100px" CssClass="wrapWord"  />
                                                                                                                              <HeaderStyle Width="100px" />
                                                                                                                         </telerik:GridTemplateColumn>

                                                                                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter Question column"
                                                                                                                              HeaderText="&nbsp;" UniqueName="colm_Question" ItemStyle-Width="40%" >
                                                                                                                              <ItemTemplate>
                                                                                                                                  <telerik:RadTextBox ID="txt_colm_EVALUATION_QUESTION" runat="server" Rows="3" TextMode="MultiLine" Width="100%" ReadOnly="true" Resize="Both"  ></telerik:RadTextBox>
                                                                                                                              </ItemTemplate>
                                                                                                                               <ItemStyle Width="250px" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="250px" />
                                                                                                                          </telerik:GridTemplateColumn>

                                                                                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter answer column" HeaderText="Evaluate" UniqueName="colm_Evaluate_CONTROL" >
                                                                                                                              <ItemTemplate>                                                                                                                  
                                                                                                                                  <telerik:RadComboBox ID="cmb_evaluate" runat="server" Width="100%" EmptyMessage="Pick the answer" ></telerik:RadComboBox>                                                                                                                                  
                                                                                                                              </ItemTemplate>
                                                                                                                                <ItemStyle Width="200px" CssClass="wrapWord"  />
                                                                                                                               <HeaderStyle Width="200px" />
                                                                                                                          </telerik:GridTemplateColumn>
                                                                                                                                                                                                                                                                                           
                                                                                                                      </Columns>
                                                                                                                  </MasterTableView>
                                                                                                              </telerik:RadGrid>


                                                                                               </div>



                                                                             </div>

                                                                     <hr /> 
                                                                     <div class="form-group row">

                                                                          <div class="row">
                                                                              <div class="col-sm-4">
                                                                                   <div class="form-group">
                                                                                        <asp:Label runat="server" ID="lblt_error" Visible="false" ForeColor="Red">Some questions are pending answers,  please complete the questions.</asp:Label>
                                                                                    </div>
                                                                              </div>       
                                                                             <div class="col-sm-4 text-center">
                                                                                   <div class="form-group">
                                                                                       <asp:LinkButton ID="btn_save_app" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="200" class="btn btn-info btn-sm margin-r-5" data-toggle="Export" ><i class="fa fa-edit fa-2x"></i>&nbsp;&nbsp;Evaluate PreScreening</asp:LinkButton>                                                                                                   
                                                                                    </div>
                                                                              </div>
                                                                             <div class="col-sm-4">
                                                                             
                                                                             </div>
                                                                           </div>
                                                                        </div>
                                                                                         
                                                                                                                                      
                                                          
                                                                                <%--**********************************************************************************APPLICANTS PRESCREENING*************************************************************--%>
                                                                                    
                                                                                <div  id="Buttons_app" runat="server"   style="display:block;">

                                                                                         <br />
                                                                                           <div class="form-group row">
                                                                                                  <div class="col-sm-12">
                                                                                                             <asp:HiddenField runat="server" ID="id_app_" />
                                                                                                                    <asp:Label runat="server" ID="lblt_apply_desc" CssClass="control-label text-bold">Application comments</asp:Label><br /><br />

                                                                                                                    <telerik:RadTextBox ID="txt_apply_desc" runat="server" Rows="10" TextMode="MultiLine" Width="95%" >
                                                                                                                    </telerik:RadTextBox>
                                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                                                        ControlToValidate="txt_apply_desc" CssClass="Error" Display="Dynamic"
                                                                                                                        ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                                                    </div>
                                                                                              </div>
                                                                               
                                                                                          <br />

                                                                                         <div  class="form-group row" >

                                                                                                      <div class="col-sm-6">

                                                                                                          <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right" Visible="false">
                                                                                                          </telerik:RadButton>
                                                                                              
                                                                                                          <telerik:RadButton ID="btn_continue" runat="server" Text="Save Application" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                                                                             Width="100px" ValidationGroup="1" Visible="false">
                                                                                                          </telerik:RadButton>

                                                                                                      
                                                                                                                                                              
                                                                                                      </div>
                                                                                                     <div class="col-sm-6">                                                                                                     
                                                                                                          <asp:LinkButton ID="btnlk_Apply" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="200" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Export"  ><i class="fa fa-paper-plane fa-2x"></i>&nbsp;&nbsp;Apply</asp:LinkButton>                                          
                                                                                                     </div>
                                                                                     </div>


                                                                                 </div>

                                                                                  <hr style="border-color:black" />
                                                        
                                                                                      <div  id="app_prescreening" runat="server" style="display:none;" class=" bg-gray-light ">


                                                                                            <ul class="timeline">

                                                                                                      <asp:Repeater ID="rept_PrescreeningDates" runat="server">
                                                                                                        <ItemTemplate>                                                                                            
                                                                                                            <li class="time-label">
                                                                                                                <span class="<%# Eval("nColor") %>">
                                                                                                                        <%# getFecha(Eval("date_created"), "D", False) %>
                                                                                                                </span>
                                                                                                            </li>
                                                                                                             <asp:Repeater ID="rept_PrescreeningComm" runat="server">
                                                                                                                <ItemTemplate>
                                                                                                                     <!-- timeline item -->
                                                                                                                        <li>
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
                                                                                                                        </li>
                                                                                                                        <!-- END timeline item -->
                                                                                                                 </ItemTemplate>  
                                                                                                             </asp:Repeater>  
                                                                                                
                                                                                                       </ItemTemplate>
                                                                                                    </asp:Repeater>          
                                                                                                  </ul>                                                                                                                                                                                                                                                                                                                                        
                                                                                          
                                                                                         <br />
                                                                                    
                                                                                    </div> <%--app_Prescreening--%>


                                                                                  <div  id="Buttons_approve" runat="server" style="display:none; padding-left:10px;" class="bg-gray-light">

                                                                                        <div  class="form-group row" style="height:450px;" >
                                                                                             <div class="col-sm-12 ">                                                                                                                                                                                                 

                                                                                                         <asp:Label runat="server" ID="lblt_approvalComments" CssClass="control-label text-bold">PreScreening Comments</asp:Label><br /><br />
                                                                                                  
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

                                                                                            <div class="col-sm-1">
                                                                                            </div>
                                                                                          <div class="col-sm-2">                                                                                                     
                                                                                              <asp:LinkButton ID="btnlk_comment" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="99%" class="btn btn-default  btn-sm margin-r-5 pull-left" data-toggle="Accept"  ValidationGroup="4" ><i class="fa fa-comment fa-2x" ></i>&nbsp;&nbsp;Comment</asp:LinkButton>                                          
                                                                                           </div>

                                                                                          <div class="col-sm-2">                                                                                                     
                                                                                              <asp:LinkButton ID="bntlk_accept" runat="server" AutoPostBack="True" SingleClick="true"  Text="Accept" Width="99%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Accept" ValidationGroup="4" ><i class="fa fa-thumbs-o-up fa-2x"></i>&nbsp;&nbsp;Accept</asp:LinkButton>                                          
                                                                                          </div>

                                                                                         <div class="col-sm-2">                                                                                                     
                                                                                              <asp:LinkButton ID="btnlk_reject" runat="server" AutoPostBack="True" SingleClick="true"  Text="Reject" Width="99%" class="btn btn-danger btn-sm margin-r-5 pull-left" data-toggle="Reject"  ValidationGroup="4" ><i class="fa fa-thumbs-o-down fa-2x"></i>&nbsp;&nbsp;Reject</asp:LinkButton>                                          
                                                                                         </div>

                                                                                         <div class="col-sm-2">                                                                                                     
                                                                                              <asp:LinkButton ID="btnlk_hold" runat="server" AutoPostBack="True" SingleClick="true"  Text="Hold" Width="99%" class="btn btn-warning btn-sm margin-r-5 pull-left hidden" data-toggle="Hold"   ValidationGroup="4"  ><i class="fa  fa-hand-stop-o fa-2x"></i>&nbsp;&nbsp;On Hold</asp:LinkButton>                                          
                                                                                         </div>
                                                                                         <div class="col-sm-2">                                                                                                     
                                                                                              <asp:LinkButton ID="btnlk_Apply2" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="99%" class="btn btn-success btn-sm margin-r-5 pull-left hidden" data-toggle="Export"  ><i class="fa fa-paper-plane fa-2x"></i>&nbsp;&nbsp;Apply</asp:LinkButton>
                                                                                         </div>
                                                                                         <div class="col-sm-1">
                                                                                         </div>

                                                                                      </div>
                                                                                      <br />
                                                                                               
                                                                            </div>  

                                                 <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                                   

                                             </div>  <%--id="Applications"--%>


                                           <%--**********************************************************************************APLICATIONS*************************************************************--%>
                                      
                                     
                                        </div> <%--class="tab-content"--%>                                                                       
                                  
                               
                                     </div>  <%--class="box-body"--%>

                            
               <%--******************************************************************************************************--%>
                                               
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

    
    <script>

                
                     var prm = Sys.WebForms.PageRequestManager.getInstance();
                          prm.add_endRequest(function() {

                              var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Documents";
                               console.log('Displaying tab ' + tabName);
                              $('#Tabs_options a[href="#' + tabName + '"]').tab('show');             

                                $("#Tabs_options a").click(function () {

                                   console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                                   $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                               });
                              
                          });


           $(document).ready(function () {                                    

                //  var QueryVariable = getParameterByName('_tab');
                //  //alert(QueryVariable);              
                ////console.log('Displaying tab ' + QueryVariable);
                //if (QueryVariable != '' && QueryVariable != null) {
                //   $('#ADDons a[href="#' + QueryVariable + '"]').tab('show');
                //  }

               var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Applicants";
                    $('#Tabs_options a[href="#' + tabName + '"]').tab('show');                

                   $("#Tabs_options a").click(function () {

                      // console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                       $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                   });


                });  


        function FuncModatTrim() {
            $('#modalTasaCambio').modal('show');
        }
                      

    </script>
</asp:Content>
