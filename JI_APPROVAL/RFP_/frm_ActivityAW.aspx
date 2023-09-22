<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityAW.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityAW" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="timeline" Src="~/Controles/ctrl_timeline_activity.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Activity Management</asp:Label></h1>
    </section>
    <section class="content">


        
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

        <div class="box">
            <div class="box-header with-border">
                 <div class="col-sm-11">   
                   <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Edit Activity</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionProyecto"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_ficha_aw" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_award_app" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_activity_award" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lbl_id_sol_app" runat="server" Text="" Visible="false" />
                        <asp:Label ID="LBL_ID_AWARD" runat="server" Text="" Visible="false" />
                    </h3>
                  </div>
                  <div class="col-sm-1 text-right">   
                       <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp();" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                  </div>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
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
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <asp:HiddenField ID="hd_percent_sol" runat="server" />
                            <asp:HiddenField ID="hd_percent_activity" runat="server" />

                            <ul class="nav nav-tabs">

                                <li role="presentation"><a runat="server" id="alink_definicion" class="hidden" href="#">ACTIVITY</a></li>                                
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"  class="active" ><a  class="primary"  runat="server" id="alink_awarded">AWARDED</a></li>
                                <li role="presentation" ><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>     
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>    
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="Label1" runat="server" Text="" Visible="false" />

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
                            
                           <%--    <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>--%>



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
                                                                                        <%# DataBinder.Eval(Container.DataItem, "codigo_RFA")%> -- <%# DataBinder.Eval(Container.DataItem, "AWARD_STATUS")%> 
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

                                            <%--
                                                  OnItemDataBound="cmb_awards_ItemDataBound"     

                                                <li style="font-weight:500; color:#ED7620;" >
                                                    <%# DataBinder.Eval(Container.DataItem, "nombre_ejecutor")%>  
                                                </li>
                                                <li style="font-weight:500;" >
                                                   <span style="font-weight:700;"  >From</span> <%#  DataBinder.Eval(Container.DataItem, "fecha_inicio_proyecto", "{0:d}")%> <span style="font-weight:700;"  >to</span> <%# DataBinder.Eval(Container.DataItem, "fecha_fin_proyecto", "{0:d}")%>
                                                </li>   
                                                 OnClientSelectedIndexChanged ="getActivity"
                                              --%>

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


                             <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>

                            


                            <%--**********************************************************************************MAIN*************************************************************--%>


                            <%--**********************************************************************************CARDS*************************************************************--%>
                            
                                 <asp:HiddenField ID="TabName" runat="server" Value="tab_activity" />

                                     <div id="dvTab" class="box-body" style="border:1px solid thick; display:block;">

                                               <ul class="nav nav-tabs" id="Tabs" >
                                                
                                                    <li class="active"><a data-toggle="tab" href="#tab_award"><asp:Label runat="server" ID="lblt_award" CssClass="control-label text-bold">AWARD DETAIL</asp:Label></a></li>      
                                                    <li><a data-toggle="tab" href="#tab_activity"><asp:Label runat="server" ID="lblt_Activity" CssClass="control-label text-bold">ACTIVITY</asp:Label></a></li>                                                                                       
                                                   <%-- <li><a data-toggle="tab" href="#Applications"><asp:Label runat="server" ID="lblt_Applications" CssClass="control-label text-bold">Application</asp:Label></a></li>                                                                                     --%>

                                                </ul>
                                               <div class="tab-content">                                                   
                                     
                                                       <div id="tab_activity" class="tab-pane fade">

                                                           
                            <%--**********************************************************************************CONTROL SELECTOR ACTIVITY*************************************************************--%>

                            <div class="form-group row">   
                               <br />                               
                               <div class="col-lg-2">
                                   <asp:Label runat="server" ID="lblt_activities" CssClass="control-label text-bold">ACTIVITIES</asp:Label>
                               </div>
                              <div class="col-sm-4 text-left">
                                   <asp:LinkButton ID="btn_add_activity" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="45%" class="btn btn-success btn-sm margin-r-5 pull-left" data-toggle="Add" ValidationGroup="3" ><i class="fa fa-plus-circle fa-2x"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;New Activity&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:LinkButton>                                                                                                     
                              </div>
                            </div>

                            <div class="form-group row">   
                              
                               <div class="col-lg-12">
                                                                 
                                   <%--OnClick="CheckedChangedDOCS(this);"  --%>

                                    <telerik:RadGrid ID="grd_activities"  
                                          Skin="Office2010Blue"   
                                          runat="server"                                                                                
                                          CellSpacing="0" 
                                          DataSourceID="" 
                                          GridLines="None" 
                                          Width="85%"                                                                                
                                          EnableViewState="true"
                                          AllowPaging="True" 
                                          AllowSorting="True"
                                          PageSize="10" >                                              
                                               <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                   <Selecting AllowRowSelect="True"></Selecting>
                                                   <%--<ClientEvents OnRowDataBound="RowDataBound" />--%>
                                               </ClientSettings>
                                               <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_AWARDED_ACTIVITY" AllowAutomaticUpdates ="True"  ShowFooter="true"  >
                                               <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                               <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                               <HeaderStyle Width="20px"></HeaderStyle>
                                               </RowIndicatorColumn>

                                               <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                               <HeaderStyle Width="20px"></HeaderStyle>
                                               </ExpandCollapseColumn>

                                               <Columns>
                                                                                                                                                                                                     

                                                       <telerik:GridTemplateColumn 
                                                            FilterControlAltText="Filter select column" 
                                                            UniqueName="colm_select" 
                                                            Visible="true"
                                                            HeaderText=" ">
                                                           <ItemTemplate>
                                                               <asp:CheckBox ID="chkSelect" runat="server" 
                                                                   AutoPostBack="True"                                                                     
                                                                   oncheckedchanged="chkVisible_CheckedChangedActivity"     />
                                                               <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                   CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                   TargetControlID="chkSelect" UncheckedImageUrl="~/Imagenes/iconos/icon-warningAlert.png">
                                                               </ajaxToolkit:ToggleButtonExtender>
                                                           </ItemTemplate>                                                                                                
                                                           <ItemStyle Width="5%" />
                                                       </telerik:GridTemplateColumn>

                                                  <telerik:GridBoundColumn  DataField="ID_AWARDED_ACTIVITY" DataType="System.Int32" 
                                                       FilterControlAltText="Filter ID_AWARDED_ACTIVITY column" HeaderText="ID_AWARDED_ACTIVITY" 
                                                       SortExpression="ID_AWARDED_ACTIVITY" UniqueName="ID_AWARDED_ACTIVITY" 
                                                       Visible="true" Display="false">
                                                   </telerik:GridBoundColumn>

                                                   <telerik:GridBoundColumn DataField="codigo_RFA" 
                                                       FilterControlAltText="Filter codigo_RFA column" HeaderText="Technical Code" 
                                                       SortExpression="codigo_RFA" UniqueName="codigo_RFA">
                                                   </telerik:GridBoundColumn>

                                                     <telerik:GridBoundColumn DataField="codigo_MONITOR" 
                                                       FilterControlAltText="Filter codigo_MONITOR column" HeaderText="Monitoring Code" 
                                                       SortExpression="codigo_MONITOR" UniqueName="codigo_MONITOR">
                                                   </telerik:GridBoundColumn>

                                                   <telerik:GridBoundColumn DataField="nombre_proyecto" 
                                                       FilterControlAltText="Filter nombre_proyecto column" 
                                                       HeaderText="Activity Name" SortExpression="nombre_proyecto" 
                                                       UniqueName="nombre_proyecto">
                                                        <HeaderStyle Width="250px" />                                            
                                                        <ItemStyle Width="250px" />
                                                   </telerik:GridBoundColumn>

                                                   <telerik:GridBoundColumn DataField="fecha_inicio_proyecto" 
                                                       FilterControlAltText="Filter fecha_inicio_proyecto column" HeaderText="Start Date"  DataFormatString="{0:dd/MM/yyyy}"
                                                       SortExpression="fecha_inicio_proyecto" UniqueName="fecha_inicio_proyecto">
                                                   </telerik:GridBoundColumn>
                                    
                                                   <telerik:GridBoundColumn DataField="fecha_fin_proyecto" 
                                                       FilterControlAltText="Filter fecha_fin_proyecto column" HeaderText="End Date"  DataFormatString="{0:dd/MM/yyyy}"
                                                       SortExpression="fecha_fin_proyecto" UniqueName="fecha_fin_proyecto">
                                                   </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn  DataField="OBLIGATED_AMOUNT" 
                                                           FilterControlAltText="Filter OBLIGATED_AMOUNT column" HeaderText="Activity Amount (USD)"  DataFormatString="{0:N0}"
                                                           SortExpression="OBLIGATED_AMOUNT" UniqueName="OBLIGATED_AMOUNT"  Aggregate="Sum"
                                                           Visible="true" Display="true">
                                                       <HeaderStyle Width="100px" />                                            
                                                       <ItemStyle Width="100px" />
                                                    </telerik:GridBoundColumn>

                                                   <telerik:GridBoundColumn  DataField="OBLIGATED_AMOUNT_LOCAL" 
                                                           FilterControlAltText="Filter OBLIGATED_AMOUNT_LOCAL column" HeaderText="Activity Amount"  DataFormatString="{0:N0}"
                                                           SortExpression="OBLIGATED_AMOUNT_LOCAL" UniqueName="OBLIGATED_AMOUNT_LOCAL"  Aggregate="Sum"
                                                           Visible="true" Display="true">
                                                       <HeaderStyle Width="100px" />                                            
                                                       <ItemStyle Width="100px" />
                                                  </telerik:GridBoundColumn>
                                                                               
                                               </Columns>

                                           <EditFormSettings>
                                           <EditColumn FilterControlAltText="Filter EditCommandColumn column" 
                                                   UniqueName="EditCommandColumn1" ></EditColumn>
                                           </EditFormSettings>
                                           </MasterTableView>

                                           <FilterMenu EnableImageSprites="False">
                                           <WebServiceSettings>
                                           <ODataSettings InitialContainerName=""></ODataSettings>
                                           </WebServiceSettings>
                                           </FilterMenu>
                                     </telerik:RadGrid>  


                               </div>

                            </div>

                            <%--**********************************************************************************CONTROL SELECTOR ACTIVITY*************************************************************--%>


                             <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>
                                                        
                                                           <%--**********************************************************************************ACTIVITY*************************************************************--%>
                                     
                                                                <br />
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <%--<asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Código de Proyecto</asp:Label>--%>
                                                                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                                                        <%-- Para mantener el tab activo después de hacer un postback --%>
                                                                        <asp:Label ID="hfAccordionIndex" runat="server" />
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigoproyecto" runat="server" Rows="3" TextMode="SingleLine" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                            ControlToValidate="txt_codigoproyecto" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                        <asp:LinkButton ID="lnk_sugerir_codigo" runat="server">Sugerir código de proyecto</asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Activity Name</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_nombreproyecto" runat="server" Rows="5" TextMode="MultiLine" Width="500px">
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                            ControlToValidate="txt_nombreproyecto" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Activity Description</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="5" TextMode="MultiLine" Width="500px" MaxLength="5000">
                                                                        </telerik:RadTextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                            ControlToValidate="txt_descripcion" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_persona_encargada" CssClass="control-label text-bold">Supervisor Lead</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadComboBox ID="cmb_persona_encargada" runat="server" Width="500px">
                                                                        </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_mecanismo_contratacion" CssClass="control-label text-bold">Contract Sub-Mechanism</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadComboBox ID="cmb_mecanismo_contratacion" runat="server" Width="500px" Filter="Contains" EmptyMessage="Select" AutoPostBack="true">
                                                                        </telerik:RadComboBox>
                                                                    </div>
                                                                </div>

                                                                 <div class="form-group row">
                                                                     <div class="col-sm-3 text-right">
                                                                         <asp:Label runat="server" ID="lblt_sub_mecanismo" CssClass="control-label text-bold">Sub-Mecanismo de contratación</asp:Label>
                                                                     </div>
                                                                     <div class="col-sm-9">
                                                                          <telerik:RadComboBox ID="cmb_sub_mecanismo_contratacion" runat="server" Width="500px" AutoPostBack="true">
                                                                          </telerik:RadComboBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                              ControlToValidate="cmb_sub_mecanismo_contratacion" CssClass="Error" Display="Dynamic"
                                                                              ErrorMessage="Registre el Sub-mecanismo" ValidationGroup="1">(*)</asp:RequiredFieldValidator>
                                                                     </div>
                                                                  </div>

                                                                <div class="form-group row" runat="server" id="ly_activity" visible="false" >
                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_actividad_padre" CssClass="control-label text-bold">Belongs to</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-9" style="border: 1px dashed black; ">
                                                                                <br />

                                                                                <telerik:RadComboBox  ID="cmb_activity_father" 
                                                                                       runat ="server" 
                                                                                       CausesValidation="False"                                                                     
                                                                                       EmptyMessage="Seleccione la actividad padre..."   
                                                                                       AllowCustomText="true" 
                                                                                       Filter="Contains"                                                  
                                                                                       Height="200px"
                                                                                       Width="80%"
                                                                                       OnDataBound="cmb_activity_DataBound" 
                                                                                       OnItemDataBound="cmb_activity_ItemDataBound"     
                                                                                       OnSelectedIndexChanged="cmb_activity_father_SelectedIndexChanged" 
                                                                                       OnClientItemsRequested="UpdateItemCountField" AutoPostBack="true"
                                                                                      >                                                              
                                                                                       <HeaderTemplate>
                                                                                         <ul>
                                                                                            <li style="font-weight:700;" >Código de Subcontrato / Estado</li>
                                                                                            <li style="font-weight:100;" >Actividad</li>                                                                        
                                                                                            <li style="font-weight:500;" >Periodo</li>                                                                                                                                                                                                                                     
                                                                                         </ul>
                                                                                       </HeaderTemplate>
                                                                                       <ItemTemplate>
                                                                                           <ul>
                                                                                                 <li style="font-weight:700;" >
                                                                                                   <%# DataBinder.Eval(Container.DataItem, "codigo_SAPME")%> -- <%# DataBinder.Eval(Container.DataItem, "nombre_estado_ficha")%> 
                                                                                                 </li>
                                                                                                 <li style="font-weight:100;" >
                                                                                                     <%# DataBinder.Eval(Container.DataItem, "nombre_proyecto")%>  
                                                                                                 </li>
                                                                                                 <li style="font-weight:500;" >
                                                                                                  <span style="font-weight:700;"  >From</span> <%#  DataBinder.Eval(Container.DataItem, "fecha_inicio_proyecto", "{0:d}")%> <span style="font-weight:700;"  >to</span> <%# DataBinder.Eval(Container.DataItem, "fecha_fin_proyecto", "{0:d}")%>
                                                                                                </li>                                                                                    
                                                                                            </ul>
                                                                                       </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            A total of
                                                                                           <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                                                              items
                                                                                           </FooterTemplate>
                                                                                  </telerik:RadComboBox> 
                                                                                 <asp:Label runat="server" ID="lbl_Activity_error" CssClass="control-label text-bold  text-red" Visible="false">(*)</asp:Label>   
                                                                                 <br /><br />      
                                                                            </div>
                                                                        </div>

                                                                <%--HIDDEN--%>
                                                                <div class="form-group row hide">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadComboBox ID="cmb_ejecutor" runat="server" Width="500px" Filter="Contains">
                                                                        </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" Enabled="false"></telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">REgions</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                        <asp:CheckBox runat="server" ID="chk_todos" Text="Todos" AutoPostBack="true" />
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <asp:Label ID="lblt_region_message" runat="server" CssClass="text-bold">This region is going to be in the code</asp:Label>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub-Region</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                        <telerik:RadGrid ID="grd_subregion" runat="server" AutoGenerateColumns="False" Visible="false" Width="500px">
                                                                            <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_subregion">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                                        HeaderText="" UniqueName="TemplateColumnAnual">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="ctrl_id" runat="server" AutoPostBack="true" OnCheckedChanged="ctrl_id_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="id_subregion" DataType="System.Int32"
                                                                                        FilterControlAltText="Filter id_subregion column" HeaderText="id_subregion"
                                                                                        ReadOnly="True" SortExpression="id_subregion" UniqueName="id_subregion"
                                                                                        Display="False">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_subregion"
                                                                                        FilterControlAltText="Filter nombre_subregion column"
                                                                                        HeaderText="Sub Región" SortExpression="nombre_subregion"
                                                                                        UniqueName="colm_nombre_subregion" ItemStyle-Width="90%">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column" Visible="false"
                                                                                        HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                                                Enabled="false" OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true">
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_districts" CssClass="control-label text-bold">Districts</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadGrid ID="grd_district" runat="server" AutoGenerateColumns="False" Width="500px">
                                                                            <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_municipio">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                                        HeaderText="" UniqueName="TemplateColumnAnual">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="ctrl_id_municipio" runat="server" AutoPostBack="true" OnCheckedChanged="ctrl_id_municipio_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="id_municipio" DataType="System.Int32"
                                                                                        FilterControlAltText="Filter id_municipio column" HeaderText="id_municipio"
                                                                                        ReadOnly="True" SortExpression="id_municipio" UniqueName="id_municipio"
                                                                                        Display="False">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_municipio"
                                                                                        FilterControlAltText="Filter nombre_municipio column"
                                                                                        HeaderText="District" SortExpression="nombre_municipio"
                                                                                        UniqueName="colm_nombre_municipio" ItemStyle-Width="40%">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column"
                                                                                        HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                                                Enabled="false" OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true">
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="alert-sm bg-blue" runat="server" id="div_mensaje" visible="false" style="width: 500px;">
                                                                            <asp:Label runat="server" ID="lbl_errorLOECero" Visible="false" CssClass="text-center text-bold" Font-Size="14px" Style="display: block; text-align: left">The LOE of the selected regions must be greater than 0</asp:Label>
                                                                            <asp:Label runat="server" ID="lbl_errorLOE" Visible="false" CssClass="text-center text-bold" Font-Size="14px">The LOE must be 100%</asp:Label>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_private_public" CssClass="control-label text-bold">Is this a Private-Public Partnership?</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <asp:RadioButtonList ID="rbn_private_public" runat="server" RepeatDirection="Horizontal" CssClass="rbnStyle" AutoPostBack="true">
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right"></div>
                                                                    <div class="col-sm-8">
                                                                       <%-- <telerik:RadGrid ID="grd_partners" runat="server" AutoGenerateColumns="false" OnNeedDataSource="grd_partners_NeedDataSource"
                                                                                    OnItemCommand="grd_partners_ItemCommand" Width="500px" Enabled="false">
                                                                                    <PagerStyle AlwaysVisible="true" />
                                                                                    <MasterTableView DataKeyNames="id_ficha_partner" CommandItemDisplay="Top" CommandItemSettings-ShowRefreshButton="false">
                                                                                        <Columns>
                                                                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar">
                                                                                                <HeaderStyle Width="10" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar" OnClick="Eliminar_Click">
                                                                                                        <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                                                    </asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridBoundColumn DataField="id_ficha_partner" UniqueName="id_ficha_partner" HeaderText="id_ficha_partner" Visible="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn UniqueName="colm_partner_name" HeaderText="Name of Partner">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txt_nombre_partner" runat="server" Text='<%# Eval("nombre_partner") %>'></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn UniqueName="colm_partnership_type" HeaderText="Type of Partnership">
                                                                                                <ItemTemplate>
                                                                                                    <telerik:RadComboBox runat="server" ID="cmb_partner_type"></telerik:RadComboBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn UniqueName="colm_partnership_focus" HeaderText="Partnership Focus">
                                                                                                <ItemTemplate>
                                                                                                    <telerik:RadComboBox runat="server" ID="cmb_partnership_focus"></telerik:RadComboBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridBoundColumn DataField="id_partner_type" UniqueName="id_partner_type" Visible="false"></telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn DataField="id_partnership_focus" UniqueName="id_partnership_focus" Visible="false"></telerik:GridBoundColumn>
                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                </telerik:RadGrid>--%>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_componente" CssClass="control-label text-bold">Componente</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_componente" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Start Date</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadDatePicker ID="dt_fecha_inicio" runat="server" AutoPostBack="true"></telerik:RadDatePicker>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                                                            ControlToValidate="dt_fecha_inicio" ErrorMessage="*"
                                                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">End Date</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadDatePicker ID="dt_fecha_fin" runat="server"></telerik:RadDatePicker>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error"
                                                                            ControlToValidate="dt_fecha_fin" ErrorMessage="*"
                                                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>

                                                                 <div class="form-group row">
                                                                              <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_exchange_rate" CssClass="control-label text-bold">Exchange Rate</asp:Label>
                                                                              </div>
                                                                            <div class="col-sm-9">
                                                                                <telerik:RadNumericTextBox ID="txt_exchange_rate" runat="server"  NumberFormat-DecimalDigits="4" >
                                                                                     <ClientEvents OnValueChanging="calc_Exchange" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </div>
                                                                    </div>

                                                                        <div class="form-group row">
                                                                              <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_total_Amount" CssClass="control-label text-bold">Activity Amount (USD)</asp:Label>
                                                                              </div>
                                                                            <div class="col-sm-9">
                                                                                <telerik:RadNumericTextBox ID="txt_tot_amount" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                     <ClientEvents OnValueChanging="calc_Tot" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </div>
                                                                        </div>

                                                                         <div class="form-group row">
                                                                              <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_total_Amount_local" CssClass="control-label text-bold">Activity Amount (COP)</asp:Label>
                                                                              </div>
                                                                            <div class="col-sm-9">
                                                                                <telerik:RadNumericTextBox ID="txt_tot_amount_Local" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                      <ClientEvents OnValueChanging="calc_Tot_LOC" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </div>
                                                                        </div>
                                    

                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_trimestre" CssClass="control-label text-bold">Quarter</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_periodo" runat="server" Width="300px"></telerik:RadComboBox>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigoME" CssClass="control-label text-bold">Código ME</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigo_SAPME" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Status</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_estado" runat="server" Width="300px"></telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigo_AID" CssClass="control-label text-bold">AID Code</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigoAID" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigoRFA" CssClass="control-label text-bold">Technical Code</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigoRFA" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Activity Code</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <div class="alert-sm bg-blue text-center" runat="server" id="divCodigo" style="width: 300px;">
                                                                            <asp:Label ID="lbl_mensaje" runat="server" CssClass="text-bold"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigo_monitor" CssClass="control-label text-bold">Monitoring Code</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigoMonitor" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_acta_aprobacion" CssClass="control-label text-bold">Acta de Aprobación</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_acta" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigo_convenio" CssClass="control-label text-bold">Código de Convenio</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadTextBox ID="txt_codigo_convenio" runat="server" Width="250px" MaxLength="250">
                                                                        </telerik:RadTextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="hidden">
                                                                    <div class="form-group row">
                                                                        <div class="col-sm-3 text-right">
                                                                            <asp:Label runat="server" ID="lblt_imagen_proyecto" CssClass="control-label text-bold">Imagen de Proyecto</asp:Label>
                                                                        </div>
                                                                        <div class="col-sm-9">

                                                                           <%-- <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                                                                TemporaryFolder="~/Temp" AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp" />--%>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group row">

                                                                     <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                                                                        </telerik:RadButton>
                                                                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Save" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="true"
                                                                            Width="100px" ValidationGroup="1">
                                                                        </telerik:RadButton>

                                                                </div>


                                                            <%--**********************************************************************************ACTIVITY*************************************************************--%> 

					                                   </div>   <%--id="tab_activity"--%>
                                             
                                     
                                                       <div id="tab_award" class="tab-pane fade in active" > 

                                                            <%--**********************************************************************************AWARD*************************************************************--%>                  

                                                           <br /><br />
                                                            <div class="form-group row">

                                                                    <div class="col-sm-2 hidden text-right">
                                                                        <asp:Label runat="server" ID="lblt_codigo_award" CssClass="control-label text-bold">Award Code</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-4 hidden">
                                                                        <div class="alert-sm bg-blue text-center" runat="server" id="div1" style="width: 300px;">
                                                                            <asp:Label ID="lbl_award_code" runat="server" CssClass="text-bold"></asp:Label>
                                                                        </div>
                                                                    </div>

                                                                     <div class="col-sm-2 text-right">
                                                                        <asp:Label runat="server" ID="lblt_apply_status" CssClass="control-label text-bold">Award Status</asp:Label>
                                                                    </div>
                                                                     <div class="col-sm-4">
                                                                               <h4><span id="spanSTATUS" runat="server" class='label ' > <asp:Label runat="server" ID="lbl_apply_status" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;<i class='fa fa-clock-o'></i>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_Apply_time" Text=""></asp:Label></span></h4>
                                                                     </div>                                                                  
                                                             </div>


                                                          

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
                                                           <hr />
                                                            <div class="form-group row">
                                                                 <div class="col-sm-3 text-right">
                                                                               <asp:Label runat="server" ID="lblt_budget" CssClass="control-label text-bold">BUDGET</asp:Label>
                                                                  </div>
                                                                  <div class="col-sm-4">
                                                                                <telerik:RadComboBox ID="cmb_budget" runat="server" Width="80%" Filter="Contains" AllowCustomText="true" EmptyMessage="Source Budget"></telerik:RadComboBox>
                                                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                                        ControlToValidate="cmb_budget" CssClass="Error" Display="Dynamic"
                                                                                        ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                  </div>
                                                            </div>
                                                             <div class="form-group row">
                                                                 <div class="col-sm-3 text-right">
                                                                     <asp:Label runat="server" ID="lblt_mecanismo_contratacion2" CssClass="control-label text-bold">CONTRACT MECHANISM</asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-9">
                                                                     <telerik:RadComboBox ID="cmb_mecanismo_contratacion2" runat="server" Width="40%" Filter="Contains" EmptyMessage="Select" AutoPostBack="true">
                                                                     </telerik:RadComboBox>
                                                                 </div>
                                                             </div>

                                                              <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                       <asp:Label runat="server" ID="lblt_sub_mecanismo2" CssClass="control-label text-bold">CONTRACT SUB-MECHANISM</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                         <telerik:RadComboBox ID="cmb_sub_mecanismo_contratacion2" runat="server" Width="40%" AutoPostBack="true">
                                                                         </telerik:RadComboBox>
                                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                                            ControlToValidate="cmb_sub_mecanismo_contratacion" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="Registre el Sub-mecanismo" ValidationGroup="3">(*)</asp:RequiredFieldValidator>
                                                                    </div>
                                                               </div>
                                                           
                                                               <div class="form-group row">
                                                                   <div class="col-sm-3 text-right">
                                                                               <asp:Label runat="server" ID="lblt_exchange_rate2" CssClass="control-label text-bold">EXCHANGE RATE</asp:Label>
                                                                           </div>

                                                                           <div class="col-sm-2">
                                                                               <telerik:RadNumericTextBox ID="txt_Exchange_Rate_2" runat="server"  NumberFormat-DecimalDigits="4" >
                                                                                    <ClientEvents OnValueChanging="calc_Exchange_Rate" />
                                                                               </telerik:RadNumericTextBox>
                                                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                            ControlToValidate="txt_Exchange_Rate_2" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                           </div>   
                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_Tot_Activity" CssClass="control-label text-bold">TOTAL ACTIVITY AMOUNT (USD)</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-2">
                                                                                <telerik:RadNumericTextBox ID="txt_tot_activity_amount" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                      <ClientEvents OnValueChanging="calc_Tot_Activity" />                                                                                    
                                                                               </telerik:RadNumericTextBox>                                                                               
                                                                            </div>
                                                                          
                                                                           
                                                              </div>


                                                                  <div class="form-group row">
                                                                       <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_Tot_Activity_LOC" CssClass="control-label text-bold">TOTAL ACTIVITY AMOUNT (COP$)</asp:Label>
                                                                           </div>
                                                                           <div class="col-sm-2">
                                                                                <telerik:RadNumericTextBox ID="txt_tot_activity_amount_LOC" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                    <ClientEvents OnValueChanging="calc_Tot_Activity_LOC" />
                                                                                </telerik:RadNumericTextBox>                                                                               
                                                                            </div>
                                                                             <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_currency" CssClass="control-label text-bold">LOCAL CURRENCY</asp:Label>
                                                                             </div>

                                                                              <div class="col-sm-2">
                                                                                      <telerik:RadComboBox ID="cmb_currency" runat="server" Width="100px">
                                                                                      </telerik:RadComboBox>
                                                                                </div>

                                                                           
                                                                      
                                                                    </div>


                                                             
                                                                       <div class="form-group row">

                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_OBLIGATED_AMOUNT_USD" CssClass="control-label text-bold">OBLIGATED AMOUNT (USD)</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-2">
                                                                                <telerik:RadNumericTextBox ID="txt_obligated_usd" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                     <ClientEvents OnValueChanging="calc_Tot_ob" />
                                                                                </telerik:RadNumericTextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                            ControlToValidate="txt_obligated_usd" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                            </div>

                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_OBLIGATED_AMOUNT_LOCAL" CssClass="control-label text-bold">OBLIGATED AMOUNT (R$)</asp:Label>
                                                                            </div>

                                                                             <div class="col-sm-2">
                                                                                    <telerik:RadNumericTextBox ID="txt_obligated_local" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                          <ClientEvents OnValueChanging="calc_Tot_LOC_ob" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_obligated_local" CssClass="Error" Display="Dynamic"
                                                                                        ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                            </div>
                                                                           
                                                                        </div>

                                                                           <div class="form-group row">

                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_LEAVERAGED_AMOUNT_USD" CssClass="control-label text-bold">LEAVERAGED AMOUNT (USD)</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-2">
                                                                                <telerik:RadNumericTextBox ID="txt_leaveraged_usd" runat="server"  NumberFormat-DecimalDigits="2" ReadOnly="true" >
                                                                                  <%--   <ClientEvents OnValueChanging="calc_Tot_ob" />--%>
                                                                                </telerik:RadNumericTextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                                                ControlToValidate="txt_leaveraged_usd" CssClass="Error" Display="Dynamic"
                                                                                ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                            </div> 

                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_LEAVERAGED_AMOUNT_LOCAL" CssClass="control-label text-bold">LEAVERAGED AMOUNT (R$)</asp:Label>
                                                                            </div>

                                                                             <div class="col-sm-2">
                                                                                <telerik:RadNumericTextBox ID="txt_leaveraged_local" runat="server"  NumberFormat-DecimalDigits="2" ReadOnly="true" >
                                                                                  <%--   <ClientEvents OnValueChanging="calc_Tot_ob" />--%>
                                                                                </telerik:RadNumericTextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                                                ControlToValidate="txt_leaveraged_local" CssClass="Error" Display="Dynamic"
                                                                                ErrorMessage="* Required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                            </div> 

                                                                           </div>                                    

                                                             <%--**********************************************************************************AWARD DOCUMENTS*************************************************************--%>
                                      
                                                                      <hr />

                                                                      <div id="AppDOCUMENTS" runat="server" style="border:1px solid thick; ">
                                                    
                                                                                   <div class="form-group row">
                                                                                        <div class="col-sm-2 text-right">
                                                                                           <asp:Label runat="server" ID="lblt_documente_tittle" CssClass="control-label text-bold">Document Title</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-sm-4">
                                                                                           <telerik:RadTextBox ID="txt_document_tittle" runat="server" Width ="80%" MaxLength="200">
                                                                                           </telerik:RadTextBox>
                                                                                        </div>

                                                                                        <div class="col-sm-2 text-right">
                                                                                               <asp:Label runat="server" ID="lblt_Type_document" CssClass="control-label text-bold">Document Type</asp:Label>
                                                                                            </div>
                                                                                            <div class="col-sm-4">
                                                                                                <telerik:RadComboBox ID="cmb_type_of_document" runat="server" Width="300px" Filter="Contains" AllowCustomText="true" EmptyMessage="Pick one category"></telerik:RadComboBox>
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
                                                                                                                        <telerik:RadButton ID="btn_agregar_doc" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                                                                                                            Text="Add Support Document" ValidationGroup="2" Width="80%">
                                                                                                                        </telerik:RadButton><br />
                                                                                                            </div>   
                                                                                                           
                                                                                                         </div>
                                                          
                                                                                                      <hr /> <br />

                                                                                                      <div class="form-group row">

                                                                                                            <div class="form-group row">                                                                                                          
                                                                                                                  <div class="col-sm-4 text-left">
                                                                                                                      <asp:Label runat="server" ID="Lblt_Documents" CssClass="control-label text-bold">AWARD DOCUMENTS</asp:Label><br /><br />
                                                                                                                  </div>
                                                                                                           </div>

                                                                                                          <div class="col-sm-12">

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
                                                                                                                                                <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="File Attached" />
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
                                                                                                                                              FilterControlAltText="Filter colm_template column"  HeaderText="DOCUMENT" 
                                                                                                                                              UniqueName="colm_FileName" >                                      
                                                                                                                                             <ItemTemplate>                                       
                                                                                                                                                 <asp:HyperLink ID="hlk_filename" 
                                                                                                                                                     runat="server" 
                                                                                                                                                     Text=""                                                                                    
                                                                                                                                                     navigateUrl="#" ToolTip="File"></asp:HyperLink>                                       
                                                                                                                                             </ItemTemplate>
                                                                                                                                              <ItemStyle Width="20%"  />
                                                                                                                                         </telerik:GridTemplateColumn>
                                                                                                                                                                                                                                                                             
                                                                                                                                        <%--<telerik:GridBoundColumn DataField="extension"
                                                                                                                                            FilterControlAltText="Filter extension column"
                                                                                                                                            HeaderText="Files Allowed" SortExpression="extension"
                                                                                                                                            UniqueName="colm_extension">
                                                                                                                                          <ItemStyle Width="10%"  />
                                                                                                                                        </telerik:GridBoundColumn>--%>
                                                                                                                                         <telerik:GridBoundColumn DataField="max_size"
                                                                                                                                            FilterControlAltText="Filter max_size column"
                                                                                                                                            HeaderText="Max Size" SortExpression="max_size"
                                                                                                                                            UniqueName="colm_max_size">
                                                                                                                                        </telerik:GridBoundColumn>
                                                                                                                                          <telerik:GridBoundColumn DataField="Template" 
                                                                                                                                                FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                                                                                UniqueName="Template" Visible="true" Display="false">                                        
                                                                                                                                            </telerik:GridBoundColumn>
                                                                                                                                      <%--   <telerik:GridTemplateColumn 
                                                                                                                                              FilterControlAltText="Filter colm_template column"  HeaderText="Document Template" 
                                                                                                                                              UniqueName="colm_template" >                                      
                                                                                                                                             <ItemTemplate>                                       
                                                                                                                                                 <asp:HyperLink ID="hlk_Template" 
                                                                                                                                                     runat="server" 
                                                                                                                                                     Text="--none--"                                                                                    
                                                                                                                                                     navigateUrl="#"></asp:HyperLink>                                       
                                                                                                                                             </ItemTemplate>
                                                                                                                                              <ItemStyle Width="20%"  />
                                                                                                                                           </telerik:GridTemplateColumn>--%>
                                                                                                                                        
                                                                                                                                    </Columns>
                                                                                                                                </MasterTableView>
                                                                                                                            </telerik:RadGrid>
                                                                                                                            <br />
                                                                                                                        </ContentTemplate>
                                                                                                                    </asp:UpdatePanel>
                                        
                                                                                                              </div>

                                                                                                      </ div>                                                                                                   
                                              
                                                                                      </div> <%-- id="Documents" --%>
                                                                                                                                      
                                                          
                                                                                <%--**********************************************************************************AWARD DOCUMENTS*************************************************************--%>
                                                                                    
                                                                                   <div class="form-group row">

                                                                                         <div class="col-sm-4 text-left">
                                                                                           <asp:LinkButton ID="btn_save_aw" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="70%" class="btn btn-info btn-sm margin-r-5 pull-right" data-toggle="Export" ValidationGroup="3" ><i class="fa fa-save fa-2x"></i>&nbsp;&nbsp;Save Award</asp:LinkButton>                                                                                                     
                                                                                         </div>
                                                                                        <div class="col-sm-4 text-left">
                                                                                           <asp:LinkButton ID="btn_awarded" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="70%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export" ValidationGroup="3" ><i class="fa fa-book fa-2x"></i>&nbsp;&nbsp;Award Activity</asp:LinkButton>                                                                                                     
                                                                                         </div>
                                                                                        <div class="col-sm-4 text-left">
                                                                                           <asp:LinkButton ID="btn_generate_activity" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="70%" class="btn btn-warning  btn-sm margin-r-5 pull-right" data-toggle="Export" ValidationGroup="3" ><i class="fa fa-refresh fa-2x"></i>&nbsp;&nbsp;Generate/Update MEL Activity</asp:LinkButton>
                                                                                        </div>
                                                                                  
                                                                                   </div>

                                                                                 <div class="form-group row">
                                                                                     <div class="col-sm-8 text-left">
                                                                                     </div>
                                                                                     <div class="col-sm-4 text-left">
                                                                                        <asp:Label runat="server" ID="lblt_last_update" CssClass="control-label text-bold">Last Update</asp:Label><br /><asp:Label runat="server" ID="lbl_last_update" CssClass="control-label">--</asp:Label><br />
                                                                                        <asp:Label runat="server" ID="lblt_last_update_by" CssClass="control-label text-bold">By</asp:Label><br /><asp:Label runat="server" ID="lbl_last_update_by" CssClass="control-label">--</asp:Label>
                                                                                     </div>
                                                                                 
                                                                                 </div>
                                                            <%--**********************************************************************************AWARD*************************************************************--%>
                                                   

                                                       </div>  <%--id="tab_award"--%>
 

                                           </div> <%--class="tab-content"--%>                                                                       
                                  
                               
                                   </div>  <%-- id="ADDons"--%>


                              <%--**********************************************************************************CARDS*************************************************************--%>

                       

                       <%--**********************************************************************************MAIN*************************************************************--%>

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
                                        <asp:Button runat="server" ID="btn_eliminarMunicipio" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarAporte" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarIndicador" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarPartner" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" UseSubmitBehavior="false" Text="Eliminar" data-dismiss="modal" />
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
        </div>
     

    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js")%>"></script>     
    <script src="<%=ResolveUrl("~/Content/dist/js/aw_chart.js?time=0.0002")%>"></script>
    
        <script>

            var UpdatingValues = 0;

            function calc_Tot(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                    var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                    var currency_contributionUSD = eventArgs.get_newValue(); //USD values
                    var Value_LOC = 0;


                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionUSD)) {
                          Value_LOC = Math.round(currency_contributionUSD * vExchangeRATE);
                    }

                    var clientSide_value = $find("<%= txt_tot_amount_Local.ClientID  %>");                                                 
                        clientSide_value.set_value(Value_LOC);

                      UpdatingValues = 0;

                }

            }

           function calc_Exchange(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSide_valueLOC = $find("<%= txt_tot_amount_Local.ClientID  %>");
                     var currency_contributionLOC = clientSide_valueLOC.get_value(); 

                      var clientSide_valueUSD = $find("<%= txt_tot_amount.ClientID  %>");       
                      //var currency_contributionUSD = clientSide_valueUSD.get_value();                                       

                      var vExchangeRATE = eventArgs.get_newValue(); 
                      var Value_USD = 0;
                   
                      if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                            Value_USD = Math.round(currency_contributionLOC / vExchangeRATE) ;
                      }
                                                                                
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValues = 0;

                }

            }


             //// txt_obligated_local //txt_leaveraged_local
             function calc_Exchange_Rate(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                    var vExchangeRATE = eventArgs.get_newValue();

                    var clientSideTOT_Activity = $find("<%= txt_tot_activity_amount_LOC.ClientID  %>");
                    var clientSideTOT_ActivityUSD = $find("<%= txt_tot_activity_amount.ClientID  %>");
                    var totActivity =  clientSideTOT_ActivityUSD.get_value(); 
                    var totActivityLOC = 0;  

                     if (!isNaN(vExchangeRATE) && !isNaN(clientSideTOT_ActivityUSD)) {
                         totActivityLOC = Math.round(totActivity * vExchangeRATE);
                         clientSideTOT_Activity.set_value(totActivityLOC);
                    }

                    var clientSideObligated = $find("<%= txt_obligated_local.ClientID  %>");
                    var clientSideObligatedUSD = $find("<%= txt_obligated_usd.ClientID  %>");
                    var totObligated = clientSideObligatedUSD.get_value(); 
                    var totObligatedLOC = 0;  

                    if (!isNaN(vExchangeRATE) && !isNaN(totObligated)) {
                         totObligatedLOC = Math.round(totObligated * vExchangeRATE);
                         clientSideObligated.set_value(totObligatedLOC);
                    }

                    var clientSideLeveraged = $find("<%= txt_leaveraged_local.ClientID  %>");
                    var clientSideLeveragedUSD = $find("<%= txt_leaveraged_usd.ClientID  %>");
                    var totLeveraged = clientSideLeveragedUSD.get_value(); 
                    var totLeveragedLOC = 0;  

                    if (!isNaN(vExchangeRATE) && !isNaN(totLeveraged)) {
                         totLeveragedLOC = Math.round(totLeveraged * vExchangeRATE);
                         clientSideLeveraged.set_value(totLeveragedLOC);
                    }
                    
                    UpdatingValues = 0;
                    
                }

            }

            //  txt_obligated_usd ---> txt_obligated_local
             function calc_Tot_ob(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_Exchange_Rate_2.ClientID  %>");
                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                     var currency_contributionUSD = eventArgs.get_newValue();
                     var Value_LOC = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionUSD)) {
                          Value_LOC = Math.round(currency_contributionUSD * vExchangeRATE);
                    }

                    var clientSide_value = $find("<%= txt_obligated_local.ClientID  %>");                                                 
                        clientSide_value.set_value(Value_LOC);

                    UpdatingValues = 0;

                    set_Leverage(currency_contributionUSD,0);

                }

            }

             // txt_tot_activity_amount --> txt_tot_activity_amount_LOC
                        
            function calc_Tot_Activity(sender, eventArgs) {

             

                if (UpdatingValues == 0) {

                    console.log('calc_Tot_Activity: ' + eventArgs.get_newValue());

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_Exchange_Rate_2.ClientID  %>");
                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                     var tot_activityUSD = eventArgs.get_newValue();
                    var Value_LOC = 0;
                                       
                    if (!isNaN(vExchangeRATE) && !isNaN(tot_activityUSD)) {
                          Value_LOC = Math.round(tot_activityUSD * vExchangeRATE);
                    }

                    var clientSide_value = $find("<%= txt_tot_activity_amount_LOC.ClientID  %>");                                                 
                        clientSide_value.set_value(Value_LOC);

                    UpdatingValues = 0;
                                       
                    var clientSideObligate = $find("<%= txt_obligated_usd.ClientID %>");
                    var val_obligate = clientSideObligate.get_value(); 
                    set_Leverage(val_obligate,0,tot_activityUSD); // from USD Tot values

                }

            }

             // txt_tot_activity_amount --> txt_tot_activity_amount_LOC

            function calc_Tot_Activity_LOC(sender, eventArgs) {

                if (UpdatingValues == 0) {

                       console.log('calc_Tot_Activity_LOC: ' + eventArgs.get_newValue());

                    UpdatingValues = 1;

                    var clientSideExchangeRate = $find("<%= txt_Exchange_Rate_2.ClientID  %>");
                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                    var currency_contributionLOC = eventArgs.get_newValue();

                     var Value_USD = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                          Value_USD = Math.round(currency_contributionLOC / vExchangeRATE);
                    }

                    var clientSide_valueUSD = $find("<%= txt_tot_activity_amount.ClientID  %>");                                                 
                        clientSide_valueUSD.set_value(Value_USD);


                      console.log('Value_USD : ' + Value_USD);
                      UpdatingValues = 0;

                     //var clientSideObligate = $find("<%= txt_obligated_usd.ClientID  %>");                    
                     var clientSideObligate = $find("<%= txt_obligated_local.ClientID  %>");
                     var val_obligate = clientSideObligate.get_value(); 
                     set_Leverage(val_obligate, 1, currency_contributionLOC);

                }

            }


            function set_Leverage(v_ObligateAmount, v_Loc, v_TotActivity = 0) {

                 console.log('set_Leverage(v_ObligateAmount): ' + v_ObligateAmount);
                
                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                    var clientSideExchangeRate = $find("<%= txt_Exchange_Rate_2.ClientID  %>");
                    var vExchangeRATE = clientSideExchangeRate.get_value(); 
                                     
                    var clientSideTOT_Activity = $find("<%= txt_tot_activity_amount.ClientID  %>");
                    var clientSideTOT_ActivityLOC = $find("<%= txt_tot_activity_amount_LOC.ClientID  %>");

                    var vTotActivity = 0

                    if (v_TotActivity == 0) {
                         if (v_Loc == 0) { //From USD
                             vTotActivity = clientSideTOT_Activity.get_value();
                        } else { //From LOC
                             vTotActivity = clientSideTOT_ActivityLOC.get_value();                        
                        }                        
                    } else {
                        vTotActivity = v_TotActivity; //From USD or LOC
                    }


                    var Leverage_amount = 0;
                    var Leverage_amount_LOC = 0;

                    if (v_Loc == 0) { // ***From USD***
                        Leverage_amount = vTotActivity - v_ObligateAmount; //USD
                        Leverage_amount_LOC = Math.round(Leverage_amount * vExchangeRATE); //LOC
                    } else { // ***From LOC***
                        Leverage_amount_LOC = vTotActivity - v_ObligateAmount; //LOC
                        Leverage_amount = Math.round(Leverage_amount_LOC / vExchangeRATE); //USD                  
                    }
                  

                     console.log('vTotActivity: ' + vTotActivity + ', Leverage_amount: ' + Leverage_amount);

                    var clientSideLeverage = $find("<%= txt_leaveraged_usd.ClientID  %>");
                    var clientSideLeverageLOC = $find("<%= txt_leaveraged_local.ClientID  %>");

                    if (v_Loc == 0) { // ***From USD***

                        if (!isNaN(Leverage_amount) && !isNaN(vExchangeRATE)) {
                            if (Leverage_amount > 0) {
                                clientSideLeverage.set_value(Leverage_amount);
                                clientSideLeverageLOC.set_value(Leverage_amount_LOC);
                            } else {
                                clientSideLeverage.set_value(0);
                                clientSideLeverageLOC.set_value(0);
                            }
                        }
                    } else { // ***From LOC***
                        if (!isNaN(Leverage_amount_LOC) && !isNaN(vExchangeRATE)) {
                            if (Leverage_amount_LOC > 0) {
                                clientSideLeverage.set_value(Leverage_amount);
                                clientSideLeverageLOC.set_value(Leverage_amount_LOC);
                            } else {
                                clientSideLeverage.set_value(0);
                                clientSideLeverageLOC.set_value(0);
                            }
                        }
                    }
                  
                  UpdatingValues = 0;

                }
                
            }


               function calc_Tot_LOC_ob(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_Exchange_Rate_2.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 

                    var currency_contributionLOC = eventArgs.get_newValue();
                     var Value_USD = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                          Value_USD = Math.round(currency_contributionLOC / vExchangeRATE);
                    }

                    var clientSide_valueUSD = $find("<%= txt_obligated_usd.ClientID  %>");                                                 
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValues = 0;

                    set_Leverage(currency_contributionLOC,1);

                }

            }


            function calc_Tot_LOC(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 
                     var currency_contributionLOC = eventArgs.get_newValue();
                     var Value_USD = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                          Value_USD = Math.round(currency_contributionLOC / vExchangeRATE);
                    }

                    var clientSide_valueUSD = $find("<%= txt_tot_amount.ClientID  %>");                                                 
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValues = 0;

                }

            }

          
           


            function FuncModatTrim() {
                $('#modalTasaCambio').modal('show');
            }

            function UpdateItemCountField(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "Un total de " + sender.get_items().get_count() + " Actividades";
            }


            function UpdateItemCountField_aw(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " contracts";
            }


            
           function loadscript() {

            //*******************************************TABS**************************************************************************
            //*******************************************TABS**************************************************************************

            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "tab_activity";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');

            $("#Tabs a").click(function () {

                //console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

            });

            //*******************************************TABS**************************************************************************
            //*******************************************TABS**************************************************************************

            }


            
        var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function (s, e) {

                loadscript();

            });



            $(document).ready(function () {

                loadscript();

           });


<%--            
               function getAward(sender, eventArgs) {

                     var combobox = $find("<%= cmb_awards.ClientID %>");
                    
                     var value = combobox.get_selectedItem().get_value();
                     var texto = combobox.get_selectedItem().get_text();
                                                                                  
                     var strText = texto.split('==>>');

                     $('#<%=lbl_activity_Code.ClientID %>').text(strText[0]);
                     $('#<%=lbl_activity_name.ClientID %>').text(strText[1]);
                                                                                  

                     $('#<%=lbl_period.ClientID %>').text(strText[2] + ' to ' + strText[3]);
                     
                   //alert('{idFichaProyecto:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }');
                   //, UserSes:"' + <%=cl_user%> + '"
                   //url: "frm_DeliverableAdd.aspx/GetDeliverables",
                   //url: "/WebMethods/App_Deliverables.asmx/GetDeliverables",

                  $.ajax({
                         type: "POST",
                         url: "frm_DeliverableAdd.aspx/GetDeliverables",
                         data: '{idFichaProyecto:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (msg) {

                             jsonResult = msg.d;
                             //alert('Result ' + jsonResult);

                             var jsonResultARR = jsonResult.split('||');
                                      
                             $('#<%= ltr_rows_Deliverables.ClientID %>').html(jsonResultARR[0]);   
                             
                             $('#<%= lbl_totalACT.ClientID %>').text(jsonResultARR[2]);
                             $('#<%= lbl_totalACT_usd.ClientID %>').text(jsonResultARR[1]);

                             $('#<%= lbl_totalPerf.ClientID %>').text(jsonResultARR[6]);
                             $('#<%= lbl_totalPerf_usd.ClientID %>').text(jsonResultARR[5]);

                             $('#<%= lbl_totalPend.ClientID %>').text(jsonResultARR[4]);
                             $('#<%= lbl_totalPend_usd.ClientID %>').text(jsonResultARR[3]);                                                                                                                                                 
                             
                             //comboboxType_.clearSelection();
                             
                             $('.PorcDeliv').val(jsonResultARR[7]);
                             $('.PorcDeliv').trigger('change');
                             $('.PorcDeliv').css('font-size', '18px');
                             $(".PorcDeliv").knob({
                                     'format': function (value) {
                                         return value + '%';
                                     }
                             });

                            $('#<%= dvNEXT_delieverable.ClientID %>').html(jsonResultARR[8]);   
                            $('#<%= hd_id_ficha_entregable.ClientID %>').val(jsonResultARR[9]);
                
                             
                         },
                         failure: function (response) {
                             alert('Error: ' + response.d);
                         }   
                     });
                
                 }--%>

        </script>


        <%--        <script src="../Scripts/js-cookie.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                //when a group is shown, save it as the active accordion group
                $("#accordion").on('shown.bs.collapse', function () {
                    var active = $("#accordion .in").attr('id');
                    Cookies.set('activeAccordionGroup', active);
                    console.log(active);
                });
                $("#accordion").on('hidden.bs.collapse', function () {
                    Cookies.remove('activeAccordionGroup');
                });

                //function cargar()
                //{
                //    var last = Cookies.get('activeAccordionGroup');
                //    if (last != null) {
                //        //remove default collapse settings
                //        $("#accordion .panel-collapse").removeClass('in');
                //        //show the account_last visible group
                //        $("#" + last).addClass("in");
                //    }
                //}
                //cargar();
            });
        </script>--%>
    </section>
</asp:Content>

