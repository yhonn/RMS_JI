<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_DeliverableRes.aspx.vb" Inherits="RMS_APPROVAL.frm_DeliverableRes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />                  
       
       <script src="<%=ResolveUrl("~/Scripts/Highcharts-8.0.0/code/highcharts.js")%>"></script>
       <script src="<%=ResolveUrl("~/Scripts/Highcharts-8.0.0/code/modules/exporting.js")%>"></script>
       <script src="<%=ResolveUrl("~/Scripts/Highcharts-8.0.0/code/modules/data.js")%>"></script>
       <script src="<%=ResolveUrl("~/Scripts/Highcharts-8.0.0/code/modules/drilldown.js")%>"></script>
       <script src="<%=ResolveUrl("~/Scripts/Highcharts-8.0.0/code/themes/sand-signika.js")%>"></script>
     
      
        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
        <section class="content-header">
        
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLES</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Deliverables</asp:Label>
                </h3>
            </div>
            <div class="box-body">

                 <div class="form-group row">
                   <div class="col-sm-1">  
                   
                   </div>
                   <div class="col-sm-9">                                     

                                    <br />
                                    <asp:Label runat="server" ID="lblt_mecanism" CssClass="control-label text-bold">Contracting Mecanism</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <telerik:RadComboBox ID="cmb_mecanism" 
                                        runat="server" 
                                        Width="300px"                                         
                                        OnClientSelectedIndexChanged="mecanismoCHG.ExtraParams(1)"    >                                        
                                    </telerik:RadComboBox>&nbsp;&nbsp;                        
                                    <asp:CheckBox ID="chk_allMecanism" runat="server" Font-Bold="True" Text="All" OnClick="changeMEC();" /><br />
                                    
                                    <asp:Label runat="server" ID="lblt_submecanism" CssClass="control-label text-bold">Contracting Sub-mecanism</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <telerik:RadComboBox ID="cmb_Sub_mecanism" runat="server" Width="300px" OnClientSelectedIndexChanged="SUBmecanismoCHG.ExtraParams(1)"   >
                                    </telerik:RadComboBox> &nbsp;&nbsp;                       
                                    <asp:CheckBox ID="chk_allSubmecanims" runat="server" Font-Bold="True"  OnClick="changeSubMec();" Text="All" Checked="true" /><br /><br /> 

                   </div>
                   <div class="col-sm-2">  
                      <asp:HiddenField ID="hd_show_this_tab" runat="server" Value="0" />
                       <%--<span id="sp_shown" style="color:#FFFFFF">0</span>
                        <telerik:RadTextBox ID="txt_flag" runat="server" Width="20px" Text="0">
                        </telerik:RadTextBox>--%>
                   </div>
                </div>

                 <ul class="nav nav-tabs">
                     <li class="active"><a class="primary" data-toggle="tab" href="#Chart_panel"><asp:Label runat="server" ID="lblt_chart" CssClass="control-label text-bold">Deliverables</asp:Label></a></li>
                     <li><a class="primary" data-toggle="tab" href="#Deliverable_panel"><asp:Label runat="server" ID="lblt_Employees" CssClass="control-label text-bold">Deliverables Detail</asp:Label></a></li>                     
                 </ul>

             <div class="tab-content">

               <script src="<%=ResolveUrl("~/Content/dist/js/Deliverable_graphs_.js?time=1.045")%>"></script>
               <link rel="stylesheet" href="../Content/slider_style.css">

                 <script>
                                        
                     $(document).ready(function () {

                         <%--var showPanel_ctrl = $find("<%= txt_flag.ClientID %>");
                         var showPanel = showPanel_ctrl.get_value();
                                                  
                         console.log('show Panel (ready): ' + showPanel);
                         if (showPanel == 0)--%>

                         $('#<%= chk_data_in.ClientID %>').prop('checked', true);//default value USD curr      

                         Currency_input();

                     });

                                            
                       // DELIV_SAMPLE_CHART();
                       // Graph_sample_2();

                     function Currency_input() {

                            //var currencySymbol_2 = $('input[id*=curr_local]');
                            //alert('local currency: ' + currencySymbol_2.val());

                            if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {           
                            //alert('Checkeado');               
                            var currencySymbol =  $('input[id*=curr_International]');
                                $("#currency_entry").html(currencySymbol.val());     
                                
                            } else {

                            //alert('NO Checkeado');
                             var currencySymbol = $('input[id*=curr_local]');
                             $("#currency_entry").html(currencySymbol.val());                                                                       

                            }                                                                                                   

                         <%--var showPanel = $('#<%= hd_show_this_tab.ClientID %>').val();

                         if (showPanel == 0)--%>
                             load_Deliverable_chart();

                     }

                     function changeMEC(){
                         
                         if ($('#<%=chk_allMecanism.ClientID %>').is(":checked")) {
                            $('#<%= chk_allSubmecanims.ClientID %>').prop('checked', true);
                          }
                                                  
                         <%--var showPanel = $('#<%= hd_show_this_tab.ClientID %>').val();
                         console.log('show panel (changeMEC()): ' + showPanel);

                         if (showPanel == 0)--%>
                           load_Deliverable_chart();

                     }
                                          
                     function changeSubMec() {
                                                  
                  <%--       var showPanel_ctrl = $find("<%= txt_flag.ClientID %>");
                         var showPanel = showPanel_ctrl.get_value();
                                                  
                         console.log('show Panel (ready): ' + showPanel);

                         if (showPanel == 0)--%>
                           load_Deliverable_chart();

                     }
                     
                      function load_Deliverable_chart() {

                                //alert(SourceRequested);
                                var JSonData;
                                var jsonStarFilter;

                                $("#LoadScreen").show();

                                var comboMec = $find("<%= cmb_mecanism.ClientID %>");
                                var cmbMec_value = 0;

                                var comboSub = $find("<%= cmb_Sub_mecanism.ClientID %>");
                                var cmbSub_value = 0;

                                if (!isNaN(comboMec.get_value()) && comboMec.get_value() != '')
                                   cmbMec_value = comboMec.get_value();
                                else
                                   cmbMec_value = 0; 
                                                    
                                if ($('#<%=chk_allMecanism.ClientID %>').is(":checked")) {
                                    cmbMec_value = -1;                                                                         
                                }
                                                                                                                                                             
                                if (!isNaN(comboSub.get_value()) && comboSub.get_value() != '')
                                     cmbSub_value = comboSub.get_value();
                                else
                                     cmbSub_value = 0;

                                  if ($('#<%= chk_allSubmecanims.ClientID %>').is(":checked")) {
                                      cmbSub_value = -1;                                                                         
                                   }
                                                       
                                //get_DataAnio(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal idIndicator As Integer) As Object
                                // alert("{vYear:'" + vYear + "', idProgram:'" + idProgram + "', idIndicator:'" + idIndicator + "' }");

                                              var vCurr = -1;
                                              var Symbol = '';                                                                                                                                           
                                              //currencySymbol.val();                                                                                    

                                             if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {
                                                vCurr = 1;
                                                var currencySymbol = $('input[id*=curr_International]');                                                                                 
                                              } else {
                                                vCurr = 0;
                                                var currencySymbol = $('input[id*=curr_local]');
                                              }

                                              Symbol = currencySymbol.val();

                                    //id_ficha_proyecto:"' + CMBvalue + '",
                                    //chk_allMecanism.ClientID = chk_allSubmecanims.ClientID = chk_data_in.ClientID
                          
                                $.ajax({
                                    type: "POST",
                                    url: "frm_DeliverableREs.aspx/get_DataGraph",
                                    //data: "{vYear:'" + vYear + "', idProgram:'" + idProgram + "', idIndicator:'" + idIndicator + "' }",
                                     data: '{idPrograma:"' + <%=Me.Session("E_IDPrograma")%> + '", vCurrency:"' + vCurr  + '", strCurrency:"' + Symbol + '", vMecanismo:"' + cmbMec_value + '", vSubMecanismo:"' + cmbSub_value + '" }',      
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {

                                        //console.log("data: " + JSON.stringify(data));
                                        jsonResult = data.d;
                                        //alert('Result ' + jsonResult);
                                        //console.log("Data: " + data);
                                        //console.log("d: " + jsonResult);
                                                                               
                                        var jsonResultARR = jsonResult.split('||');
                                        //alert('Result ' + jsonResult);                                        
                                        //console.log(' --> 0' + jsonResultARR[0]);
                                        //console.log(' --> 0' + JSON.parse(jsonResultARR[0]));
                                        //alert(JSON.parse(jsonResultARR[0]));
                                        //alert(jsonResultARR[1] + ' --> 1');
                                        //console.log(' --> 1' + jsonResultARR[1]);
                                        //console.log(' --> 1' + JSON.parse(jsonResultARR[1]));
                                        //alert(JSON.parse(jsonResultARR[1]));

                                        if (jsonResultARR[0] != '[]' && jsonResultARR[1] != '[]') {
                                          //*************************************************************
                                            Load_Leveraging_chart(JSON.parse(jsonResultARR[0]), JSON.parse(jsonResultARR[1]),Symbol);                                                                              
                                          //*************************************************************
                                        }

                                        $("#LoadScreen").hide();

                                    },
                                    failure: function (response) {
                                        console.log('Error Graphing: ' + response.d);
                                        $("#LoadScreen").hide();
                                    }

                                });


                            }//Function
                     

                     <%--function pageLoad() {

                        // alert('Hi there');
                            var grid = $find("<%= grd_cate.ClientID %>");
                            var columns = grid.get_masterTableView().get_columns();
                            for (var i = 0; i < columns.length; i++) {
                                columns[i].resizeToFit(false, true);
                            }

                     }--%>

                                       
                     
                    function UseRadWindow(idDeliverable) {
                        var oWnd = $find("<%= RadWindowDelieverable.ClientID %>");

                        //console.log("http://rms.ftfyla.com/RMS_SIME/Deliverable/frm_DeliverableFollowingRep.aspx?ID=" + idDeliverable);
                        oWnd.moveTo(200, 200);
                        oWnd.add_close(OnClientCloseHelp_Dev); //set a function to be called when
                        oWnd.show();
                        oWnd.setSize(900, 600);
                        oWnd.setUrl('frm_DeliverableFollowingRep.aspx?ID=' + idDeliverable);
                        //oWnd.setUrl('http://www.yahoo.com');
                        oWnd.minimize();
                        oWnd.maximize();
                        oWnd.restore();
                                          
                        
                     }


                    function OnClientCloseHelp_Dev(oWnd) {  
                        oWnd.setUrl("about:blank"); // Sets url to blank  
                        oWnd.remove_close(OnClientCloseHelp_Dev);                
                    }       

                     //load_Deliverable_chart();
                     //DELIV_SAMPLE_CHART_II();
                     //Graph_sample_III();
                     var Populating = false;

                     function SUBmecanismoCHG(sender, eventArgs, Params) {
                                                  
                         //var showPanel = $('input[id*=hd_show_this_tab]');
                         //console.log('show panel (SUBmecanismoCHG()): ' + showPanel.val());

                         if (!$('#<%= chk_allSubmecanims.ClientID %>').is(":checked")) {

                                  //if (showPanel.val() == 0)
                                     load_Deliverable_chart();

                         }                                                

                     }

                      function mecanismoCHG(sender, eventArgs, Params) {
                    
                                var idLevel = Params[0];

                                //alert('Level: ' + idLevel + ' Populating:' + Populating);
                                if (!Populating)  {

                                    var item = eventArgs.get_item();
                                    var clientId = sender.get_id();
                                   // alert('item: ' + item + ' ID: ' + clientId + ' idLevel: ' + Params[0]);
                                    var idMec = sender.get_selectedItem().get_value();//item.get_value();

                                   // console.log('idMec: ' + idMec);
                                    var comboSubmecanismo = $find("<%=cmb_Sub_mecanism.ClientID%>");

                                    Populating = true;             
                                   
                                    var jsonResult;                                  
                                    //alert('{IDLevel:' + idLevel + ', anio:' + vAnio + ', idImplementer:' + vImplementer + ', idActivity:' + vActivity + ', idPrograma:' + idProgram + '}');
                 
                                    $.ajax({
                                        type: "POST",
                                        url: "frm_DeliverableREs.aspx/get_subMEC",
                                        data: '{idMec:' + idMec + '}',
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (data) {
                                
                                            jsonResult = data.d;
                                            //alert('Result ' + jsonResult);
                                
                                            var jsonResultARR = jsonResult.split('||');                                
                                            //console.log('Data:' + jsonResultARR[0]);

                                            if (jsonResultARR[0] != '[]') {//Implementer
                                                   // alert('Result Implementer: ' + jsonResultARR[0]);
                                                    fillCombo(comboSubmecanismo, jsonResultARR[0]);
                                            }                                                                                                                                             
                                                 
                                            Populating = false;
                                            //loadData();

                                            <%--var showPanel = $('#<%= hd_show_this_tab.ClientID %>').val();                                              
                                            console.log('show panel (mecanismoCHG()): ' + showPanel);--%>

                                            if (!$('#<%=chk_allMecanism.ClientID %>').is(":checked")) {

                                                //if (showPanel == 0)
                                                     load_Deliverable_chart();

                                            }

                                        },
                                        failure: function (response) {
                                            alert('Error loading catalogs: ' + response.d);
                                            Populating = false;
                                        }
                                    });

                                //} else {
                                //    alert('Populating');
                          } //IF Populating


                                                


                     }



                    function fillCombo(combo, result) {

                            combo.clearItems();

                            //var items = result.d || result;
                            //alert('items:' + items + ' lenght:' + items.length);
                            data = jQuery.parseJSON(result);
                            //alert('items:' + data + ' lenght:' + data.length);

                            // This just lets your users know that nothing was returned with their search 
                            if (data.length == 0) {
                                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                comboItem.set_text("No records found");
                                comboItem.set_value("0");
                                combo.get_items().add(comboItem);
                                //combo.clearSelection();
                            }

                            for (var i = 0; i < data.length; i++) {

                                var item = data[i];

                                //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                comboItem.set_text(item.Text);
                                comboItem.set_value(item.Value);
                                //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                //alert('Combo Text: ' + comboItem.get_text() + ' Combo Value: ' + comboItem.get_value());
                                //combo.trackChanges();

                                //alert('item' + i + ': ' + comboItem.get_text() + ' ' + comboItem.get_value());
                                combo.get_items().add(comboItem);
                                //alert('item' + i + ': ' + item.Text + ' ' + item.Value);

                                //combo.commitChanges();
                                //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                            }

                            combo.clearSelection();
                            combo.trackChanges();
                            combo.get_items().getItem(0).select();
                            combo.updateClientState();
                            combo.commitChanges();

                     }


                Function.prototype.ExtraParams = function () {
                    var method = this, args = Array.prototype.slice.call(arguments);
                    return function () {
                        return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                    };
                };



                       function Refresh_Grid() {                   

                           grdGrid = $find("<%=grd_cate.ClientID%>").get_masterTableView();                                                               
                         
                               //ctrlSHOW.val('1');
                                                     
                                var comboMec = $find("<%= cmb_mecanism.ClientID %>");
                                var cmbMec_value = 0;

                                var comboSub = $find("<%= cmb_Sub_mecanism.ClientID %>");
                                var cmbSub_value = 0;

                                if (!isNaN(comboMec.get_value()) && comboMec.get_value() != '')
                                   cmbMec_value = comboMec.get_value();
                                else
                                   cmbMec_value = 0; 
                                                    
                                if ($('#<%=chk_allMecanism.ClientID %>').is(":checked")) {
                                    cmbMec_value = -1;                                                                         
                                }
                                                                                                                                                             
                                if (!isNaN(comboSub.get_value()) && comboSub.get_value() != '')
                                     cmbSub_value = comboSub.get_value();
                                else
                                     cmbSub_value = 0;

                                  if ($('#<%= chk_allSubmecanims.ClientID %>').is(":checked")) {
                                      cmbSub_value = -1;                                                                         
                           }

                                <%--var showPanel_ctrl = $find("<%= txt_flag.ClientID %>");
                           --%>
                                 grdGrid.set_dataSource(jQuery.parseJSON(null));
                                 grdGrid.rebind();   

                              <%--//$('#<%= hd_show_this_tab.ClientID %>').attr('value', '1');
                              //$('#<%= hd_show_this_tab.ClientID %>').val(1);
                                showPanel_ctrl.set_value('1');

                              //var ctrlSHOW = $('#<%= hd_show_this_tab.ClientID %>');
                              console.log('show Panel (get_Grid_Data): ' +  showPanel_ctrl.get_value());
                             --%>
                           //***********************************************************************************************************
                           //***********************************************************************************************************
                           //***********************************************************************************************************
                                    //$.ajax({
                                    //    type: "POST",
                                    //    url: "frm_DeliverableREs.aspx/get_Grid_Data",
                                    //    data: '{idMec:' + cmbMec_value + ', idSubMec:' + cmbSub_value  + '}',
                                    //    contentType: "application/json; charset=utf-8",
                                    //    dataType: "json",
                                    //    success: function (data) {
                                            
                                    //        jsonResult = data.d;
                                    //        //alert('Result ' + jsonResult);                                
                                    //        //var jsonResultARR = jsonResult.split('||');   
                                    //        //console.log('Data:' + jsonResult);
                                           
                                    //         console.log('show Panel (get_Grid_Data): ' + ctrlSHOW.val());

                                    //        if (jsonResult != '[]') {//Data_grid Value
                                    //            //console.log('Data:' + jsonResult);
                                    //            grdGrid.set_dataSource(jQuery.parseJSON(jsonResult));
                                    //            grdGrid.rebind();
                                    //        } else {
                                    //            console.log('No Data returned');
                                    //        }

                                    //    },
                                    //    failure: function (response) {
                                    //        console.log('Error loading catalogs: ' + response.d);
                                    //        Populating = false;
                                    //    }
                                    //});    
                          //***********************************************************************************************************
                          //***********************************************************************************************************
                          //***********************************************************************************************************

                     }

                        function loadTab() {
                            $('.nav-tabs a[href=#Deliverable_panel]').tab('show')
                        }

                     
               </script>
                 
                  <div id="Chart_panel" class="tab-pane fade in active">

                       <div class="box-body">
                          <div class="form-group row">
                                <div class="col-sm-1 text-right">    
                                   <br />
                                   <label class="switch">
                                    <input id="chk_data_in" runat="server" type="checkbox" onchange="Currency_input()">
                                    <span class="slider round"></span>
                                    </label>                                              
                                 </div>
                                 <div class="col-sm-2 text-left">                                        
                                    <h2><span id="currency_entry" class="label label-info">USD</span></h2>      
                                     <asp:HiddenField ID="curr_local" runat="server" Value="" />                                                                                 
                                     <asp:HiddenField ID="curr_International" runat="server" Value="" /> 
                                </div>    
                                <div class="col-sm-1 text-right">
                                   <br /> <asp:LinkButton ID="lnk_loadChart" Text="Try" Width="12%" class="btn btn-primary btn-sm margin-r-5 pull-right" data-toggle="Try" OnClick="load_Deliverable_chart();" ><i class="fa fa-refresh fa-2x"></i>&nbsp;&nbsp;Reload Chart</asp:LinkButton>
                                </div>
                                <div class="col-sm-8 text-right">                                                           
                                   <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5 pull-right" data-toggle="Try" OnClick="showhelp('Deliverable_Detail.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   <br /> 
                                </div>
                          </div>
                        </div>
                        <div id="Deliverables_chart" style="width: 95%; height: 600px; margin: 0 0 0 0;"></div>   
                  </div>

                  <div id="Deliverable_panel" class="tab-pane">
                           
                          <div class="form-group row">

                               <div class="col-sm-1 text-left">
                             
                               </div>                              
                               <div class="col-sm-1 text-right">
                                   <br /> <asp:LinkButton ID="lnk_loadTable" Text="Try" Width="12%" class="btn btn-primary btn-sm margin-r-5 pull-right" data-toggle="Try" OnClick="Refresh_Grid();" ><i class="fa fa-refresh fa-2x"></i>&nbsp;&nbsp;Load Table</asp:LinkButton>
                                </div>

                              <div class="col-sm-10 text-left">
                                 <br />
                                  <asp:LinkButton ID="btnlk_Export" runat="server" AutoPostBack="True" SingleClick="true"  Text="Exportar" Width="12%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export"  ><i class="fa fa-file-excel-o fa-2x"></i>&nbsp;&nbsp;Export Detail</asp:LinkButton>   <br /><br />                      
                                  <asp:HiddenField ID="hd_tp" runat="server" Value="0" />

                              </div>
                              
                          </div>

                          <div class="form-group row" style="width:100%; margin: 0 auto">

			                    <%--***********REGISTER EMPLOYEES***********--%>

                              <div class="col-lg-12">
                                                          

                                <telerik:radgrid
                                        ID="grd_cate" 
                                        runat="server"                                                                 
                                        CellSpacing="0"                            
                                        Width="100%"
                                        AllowPaging="True"                                    
                                        ShowGroupPanel  ="True" 
                                       PageSize="100" CssClass="MyGridClass"   >                                                                                                                                                                                        
                                       <PagerStyle NextPageText="Next" PrevPageText="Prev" Mode="NextPrevAndNumeric" /> 
                                         <ClientSettings>                               
                                              <Selecting AllowRowSelect="True" />  
                                              <Scrolling AllowScroll="True"  UseStaticHeaders="True" ScrollHeight="500"  ></Scrolling>
                                              <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" AllowResizeToFit="true"/>                               
                                         </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="true" ShowFooter="True" ShowGroupFooter="true">  <%--CommandItemDisplay="Top"--%>
                                         <%--<CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false" />--%>
                                        </MasterTableView>
                                    </telerik:radgrid>

                              <br /><br />

                           <style type="text/css">

                               .MyGridClass .rgDataDiv
                                    {
                                    height : auto !important ;
                                    }

                           </style>


                                              


                                    </div> <%--<div class="col-lg-12">--%>

                                     <%--***********REGISTER EMPLOYEES***********--%>

                          </div>    <%--<div class="form-group row">   --%> 
       
                     </div> <%--id=Employees"--%>
              
                 <div id="Users" class="tab-pane fade" >

                          <div class="form-group row" style="width:100%; margin: 0 auto">
                          <br /><br />
                             
                                  <%--***********Users by Register***********--%>

                            

  	                              <%--***********Users by Register***********--%>


                          </div>    <%--<div class="form-group row">   --%>                             

                     </div> <%--id="Users"--%>


   </div> <%--<div class="tab-content">--%>
                   
  </div>  <%--<div class="box-body">--%>



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
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

         <telerik:RadWindow InitialBehaviors="Maximize" RenderMode="Lightweight" runat="server" Width="800" Height="300" id="RadWindowDelieverable" VisibleOnPageLoad="false"  >
         </telerik:RadWindow>   

         
        <div id="LoadScreen" class="modal" >
              <asp:Image ID="Image1" runat="server" CssClass="loadingGif"  ImageUrl="~/Imagenes/iconos/Loading.gif" />
        </div>

    </section>
    <!-- /.content -->
</asp:Content>

