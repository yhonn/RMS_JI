<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityF.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityF" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    
      <link rel="stylesheet" href="../Content/slider_style.css">
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
      <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>
   
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
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activity Funds</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                         <asp:Label ID="lbl_id_ficha_aw" runat="server" Visible="False"></asp:Label>
                        <asp:HiddenField ID="tasaCambio" runat="server" />
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
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                              <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_definicion" class="hidden" href="#">ACTIVITY</a></li>                                
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation" ><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>      
                                <li role="presentation"><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation" class="active"><a class="primary"  runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>    
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>

                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:Label ID="LBL_ID_AWARD" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">



                                  
                            <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>

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
                                                                                        <li style="font-weight:700;" >Technical Code / Status</li>
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
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_aporte_origen" CssClass="control-label text-bold">Funding Source</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox ID="cmb_aporte_origen" runat="server" Width="500px" AutoPostBack="true" Filter="Contains"></telerik:RadComboBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="cmb_aporte_origen" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="* Required" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_fuente_aporte" CssClass="control-label text-bold">Funding Name</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox ID="cmb_fuente_aporte" runat="server" Width="500px" AutoPostBack="true" Filter="Contains"></telerik:RadComboBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="cmb_fuente_aporte" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="* Required" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                </div>
                                <div class="col-sm-2">
                                    <br />
                                    <telerik:RadButton ID="btn_guardarAporte" runat="server" CssClass="btn btn-sm" Text="Agregar" ValidationGroup="2" Width="75px">
                                    </telerik:RadButton><br /><br />
                                </div>
                                 <div class="col-sm-2">                                       
                                        <br />
                                        <asp:Label runat="server" ID="lblt_exchange_rate" CssClass="control-label text-bold">Exchange Rate: </asp:Label>
                                        <telerik:RadNumericTextBox ID="txt_tasa_cambio" runat="server" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                ControlToValidate="txt_tasa_cambio" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="* Required" ValidationGroup="2"></asp:RequiredFieldValidator>
                                      <asp:HiddenField ID="curr_local" runat="server" Value="" />
                                      <asp:HiddenField ID="curr_International" runat="server" Value="" />
                                  </div>
                            </div>

                             <div class="form-group row">
                                <hr style="color:#808080" />
                            </div>

                             <div class="form-group row">                                     
                                   
                                    <div class="col-sm-3 text-right">
                                       
                                    </div>
                              </div>

                            <div class="form-group row">
                                <hr style="color:#808080" />
                            </div>

                            <div class="form-group row">                                
                                <div class="col-sm-7">
                                    <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True"
                                        AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_ACTIVITY_AW_FUNDING" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="ID_ACTIVITY_AW_FUNDING"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter ID_ACTIVITY_AW_FUNDING column"
                                                    HeaderText="ID_ACTIVITY_AW_FUNDING" ReadOnly="True"
                                                    SortExpression="ID_ACTIVITY_AW_FUNDING" UniqueName="ID_ACTIVITY_AW_FUNDING"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar">
                                                    <HeaderStyle Width="10" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                            OnClick="EliminarAporte_Click">
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridBoundColumn DataField="nombre_aporte" FilterControlAltText="Filter nombre_aporte column"
                                                    HeaderText="Source of Funding" SortExpression="nombre_aporte" UniqueName="colm_nombre_aporte">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="Total Funding" UniqueName="colm_meta_total">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txt_total_aporte" runat="server" MinValue="0">
                                                            <ClientEvents  OnValueChanged="Grd_total_Change" /> 
                                                            <NumberFormat ZeroPattern="n" />
                                                        </telerik:RadNumericTextBox>
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="Total Funding USD" UniqueName="colm_total_usd">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txt_total_aporte_usd" runat="server" MinValue="0" >
                                                            <ClientEvents  OnValueChanged="Grd_total_ChangeUSD" /> 
                                                            <NumberFormat ZeroPattern="n" />
                                                        </telerik:RadNumericTextBox>
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridBoundColumn DataField="monto_aporte" FilterControlAltText="Filter monto_aporte column"
                                                    HeaderText="monto_aporte" SortExpression="monto_aporte" UniqueName="monto_aporte" Visible="true" Display="false">
                                                </telerik:GridBoundColumn>

                                                 <telerik:GridBoundColumn DataField="TotalUSD" FilterControlAltText="Filter TotalUSD column"
                                                    HeaderText="TotalUSD" SortExpression="TotalUSD" UniqueName="TotalUSD" Visible="true" Display="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="tasa_cambio" FilterControlAltText="Filter monto_aporte column"
                                                    HeaderText="tasa_cambio" SortExpression="tasa_cambio" UniqueName="tasa_cambio" Visible="true" Display="false">
                                                </telerik:GridBoundColumn>

                                            </Columns>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias="Source" FieldName="nombre_AporteOrigen"
                                                            FormatString="" HeaderText="" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="nombre_AporteOrigen" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                        </MasterTableView>
                                    </telerik:RadGrid><span id="span_curr_entry" style="font-weight:600" class="pull-right" ></span><br />
                                      <div class="text-right">
                                              <h4>
                                                <asp:Label runat="server" ID="lblt_total_aporte" CssClass="text-bold">Total Funding:</asp:Label>
                                                <asp:Label runat="server" ID="lbl_total"></asp:Label>
                                              </h4>
                                              <%--<h4>
                                                <asp:Label runat="server" ID="lblt_tasa_cambio" CssClass="text-bold">Tasa de cambio:</asp:Label>
                                                <asp:Label runat="server" ID="lbl_tasa_cambio"></asp:Label>
                                              </h4>--%>
                                              <h4>
                                                <asp:Label runat="server" ID="lblt_total_aporteUSD" CssClass="text-bold">Total Funding USD:</asp:Label>
                                                <asp:Label runat="server" ID="lbl_totalUSD"></asp:Label>
                                              </h4>
                                       </div>
                                </div>
                                <div class="col-sm-5">
                                    <asp:HiddenField runat="server" ID="hdnFunding" />
                                    <div id="containerPie" style="width: 100%; margin: 0 auto" class="col-sm-5"></div>
                                </div>
                            </div>                          
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar Aportes" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
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
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
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
          
    <script type="text/javascript">                           

        $(document).ready(function () {                       

            Highcharts.setOptions({
                lang: {
                    thousandsSep: ','
                }
            });
                        
            var currencySymbol = $('input[id*=curr_local]');
            //loadchart(currencySymbol.val());
            setTimeout(loadchart(currencySymbol.val()),20000);

        });
        
        var bndUPD = 0;       


        function loadchart(currSymbol) {
                        
            //console.log('Chart: ' + Highcharts.chart + ' Values:' + $('#<%=Me.hdnFunding.ClientID%>').val() );
            
            //Highcharts.chart('containerPie', {
            $('#containerPie').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                credits: false,
                title: {
                    text: 'Activity Funding'
                },
                tooltip: {
                    pointFormat: '{series.name}: ' + currSymbol + ' {point.y:,.0f} <br /> <b>{point.percentage:.2f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false                          
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Funding',
                    colorByPoint: true,
                    data: [<%=Me.hdnFunding.Value %>]
                }]
            });
            
        }


         function Grd_total_Change(sender, eventArgs) {

            if (bndUPD == 0) {

                if (eventArgs.get_newValue() == "")
                    eventArgs.set_cancel(true);
                else {

                     var masterTable = $find('<%= grd_aportes.ClientID %>');

                    if (masterTable) {

                        bndUPD = 1;
                        var tableView = masterTable.get_masterTableView();
                        var rows = tableView.get_dataItems();
                                     
                        var ctrl_reported_value = 'monto_proyecto'
                        var valorAporte = 0;
                        var valorAporteUSD = 0;
                        var valueMonto_fix = 0;
                        var valorTotal = $('input[id*=' + ctrl_reported_value + ']');
                        var exchange_rate = 0;

                        var currencySymbol = $('input[id*=curr_local]');          
                        //alert(valorTotal.val().replace(',','.'));

                        var tasa_cambio = 0                         
                        var ctrl_tasa_cambio = $find('<%= txt_tasa_cambio.ClientID %>');
                        var tasa_cambio = parseFloat(ctrl_tasa_cambio.get_value());
                        
                            for (var i = 0; i < rows.length; i++) { //Row Total
                                   // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                                   // alert(ControlText.value);
                                
                                    if (tasa_cambio == NaN) {
                                            exchange_rate = rows[i].get_cell('tasa_cambio').innerHTML;
                                    } else {
                                            exchange_rate = tasa_cambio
                                    }

                                var Control1 = rows[i].findControl('txt_total_aporte');  
                                var valueCTRL = Control1.get_value();

                                var Control2 = rows[i].findControl('txt_total_aporte_usd');  
                                var valueCTRL_usd = Control2.get_value();

                                //var Control3 = rows[i].findControl('txt_tasa_cambio_');  
                                //var valueCTRL_exchange = Control3.get_value();
                                                                
                                //var Control4 = rows[i].findControl('txt_col_porcentaje');  
                                ////var valueCTRL_porcentage = Control4.get_value();

                                valueMonto_fix = parseFloat(valueCTRL) / parseFloat(exchange_rate);
                                Control2.set_value(valueMonto_fix);

                                //Control4.set_value((valueCTRL / parseFloat(valorTotal.val().replace(',', '.')))*100);
                                //alert(Control3.get_value());

                                valorAporte += parseFloat(valueCTRL);
                                valorAporteUSD += parseFloat(valueMonto_fix);

                            }                        

                        $('#<%=lbl_total.ClientID%>').html(currencyFormat(valorAporte,currencySymbol.val()));

                        $('#<%=lbl_totalUSD.ClientID%>').html(currencyFormat(valorAporteUSD,'$'));


                        bndUPD = 0;

                    }//if (masterTable) {

                 }

              }// bndUPD == 1;

        }


          function Grd_total_ChangeUSD(sender, eventArgs) {

              if (bndUPD == 0) {


                        if (eventArgs.get_newValue() == "")
                            eventArgs.set_cancel(true);
                        else {

                             var masterTable = $find('<%= grd_aportes.ClientID %>');

                            if (masterTable) {

                                bndUPD = 1;
                                var tableView = masterTable.get_masterTableView();
                                var rows = tableView.get_dataItems();
                                     
                                //var ctrl_reported_value = 'monto_proyecto'
                                var valorAporte = 0;
                                var valorAporteUSD = 0;
                                var valueMonto_fix = 0;
                                //var valorTotal = $('input[id*=' + ctrl_reported_value + ']');
                                 var exchange_rate = 0;
                                //alert(valorTotal.val().replace(',','.'));

                                var currencySymbol = $('input[id*=curr_local]');  

                                 var tasa_cambio = 0                         
                                 var ctrl_tasa_cambio = $find('<%= txt_tasa_cambio.ClientID %>');
                                 var tasa_cambio = parseFloat(ctrl_tasa_cambio.get_value());
                                
                                    for (var i = 0; i < rows.length; i++) { //Row Total
                                           // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                                           // alert(ControlText.value);

                                        //var cell = tableView.getCellByColumnUniqueName(rows[i], 'tasa_cambio');
                                        //here cell.innerHTML holds the value of the cell  

                                        if (tasa_cambio == NaN) {
                                             exchange_rate = rows[i].get_cell('tasa_cambio').innerHTML;
                                        } else {
                                              exchange_rate = tasa_cambio
                                        }                                         
                                        //exchange_rate = cell.innerHTML;

                                        var Control1 = rows[i].findControl('txt_total_aporte');  
                                        var valueCTRL = Control1.get_value();

                                        var Control2 = rows[i].findControl('txt_total_aporte_usd');  
                                        var valueCTRL_usd = Control2.get_value();

                                        //var Control3 = rows[i].findControl('txt_tasa_cambio_');  
                                        //var valueCTRL_exchange = Control3.get_value();
                                                                
                                        //var Control4 = rows[i].findControl('txt_col_porcentaje');  
                                        ////var valueCTRL_porcentage = Control4.get_value();

                                        //alert( 'ex: ' + exchange_rate + ' val:' + valueCTRL_usd);

                                        valueMonto_fix = parseFloat(valueCTRL_usd) * parseFloat(exchange_rate);
                                        Control1.set_value(valueMonto_fix);

                                        valorAporte += parseFloat(valueMonto_fix);
                                        valorAporteUSD += parseFloat(valueCTRL_usd);

                                    }

                             
                                $('#<%=lbl_total.ClientID%>').html(currencyFormat(valorAporte,currencySymbol.val()));

                                $('#<%=lbl_totalUSD.ClientID%>').html(currencyFormat(valorAporteUSD,'$'));

                             bndUPD = 0;

                            }//if (masterTable) {

                        }

                    
              }// bndUPD == 1;

        }


        function currencyFormat(num,currencySymbol) {
          return currencySymbol + num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

       function UpdateItemCountField_aw(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " contracts";
        }

    </script>
</asp:Content>
