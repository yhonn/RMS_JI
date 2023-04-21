<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_DeliverableDocs"  Codebehind="frm_DeliverableDocs.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLE</asp:Label>
                </h1>
            </section>
            <section class="content">

            <style type="text/css">

                    .text_ctrl
                    {
	                    color: Black !important;
                        background-color:#ffcb00!important;
                        border-color:#ff6a00!important;
                    }

            </style>

                <script src="../Content/plugins/knob/jquery.knob.js"></script>
                <script type="text/javascript">



                                                        $(document).ready(function () {

                                                            
                                                            var hdValueTotal = $('input[id*=hd_percent]');

                                                          //  alert(hdValueTotal.val());

                                                                  $('.PorcDeliv').val(hdValueTotal.val());
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '16px');
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });
                                                                })                                                                                                                                                                                                                                               

                                                        function set_Percent(valPercent) {

                                                                //alert('Percent value ' + valPercent);

                                                                  $('.PorcDeliv').val(valPercent);
                                                                  $('.PorcDeliv').trigger('change');
                                                                  $('.PorcDeliv').css('font-size', '16px');
                                                                  $(".PorcDeliv").knob({
                                                                      'format': function (value) {
                                                                          return value + '%';
                                                                      }
                                                                  });

                                                              }
                                                                                                                            

                                                       
                                        function Calculate(sender, args, params) {

                                            //alert(params);

                                            var strID = params.toString().split("||");

                                            //alert(strID[0]);
                                            //alert(strID[1]);

                                            var idMeta_ficha = strID[0]; //params.toString().split("||")[0];
                                            var idAvance_ficha = strID[1]; //params.toString().split("||")[1];
                                            
                                            var ctrl_name = 'txt_value_' + idMeta_ficha;
                                            var ctrl_total_value = 'hd_ind_report_value_' + idAvance_ficha;
                                            var ctrl_reported_value = 'hd_ind_value_' + idAvance_ficha;
                                                           
                                            //alert(ctrl_name);                                                                                       
                                            //alert($$("txtSymbol", $("#wrapper")).attr("id"));
                                            //alert($$(ctrl_name).attr("id"));
                                            //alert($$(ctrl_name).val());

                                            var radControl_destination = $find($$(ctrl_name).attr("id"));
                                            var radControl_source = sender;
                                         // var hd_controlValue = $find($$(ctrl_value).attr("id"));
                                                                                       
                                            //alert($$(ctrl_value).attr("id"));
                                            //alert($$("hd_ind_report_value_40-87").attr("id"));

                                            //alert($('input#hd_ind_report_value_4087').val());
                                            //alert($(':hidden#hd_ind_report_value').val());
                                            //hd_ind_report_value_4087
                                            //alert($('').val());

                                           // alert(ctrl_value);
                                            
                                            //$.each($('input[id*=' + ctrl_value + ']'), function (i, val) {
                                            //    if ($(this).attr("type") == "hidden") {
                                            //        var valueOfHidFiled = $(this).val();
                                            //        alert($(this).attr("id") + ': ' + valueOfHidFiled);
                                            //    }
                                            //});

                                            var hdValueTotal = $('input[id*=' + ctrl_reported_value + ']');

                                           // alert($("[id$=_hd_ind_report_value_4087]").attr("id"));
                                                                                        
                                            //$$(ctrl_name);
                                            //alert('Total Indicator: ' + radControl_destination.get_value());
                                            //alert(hd_controlValue.Value);
                                            //alert('Total Report Changed: ' + radControl_source.get_value());
                                            //alert(hd_controlValue.Value);
                                            //alert('Total Report : ' + hdValueTotal.val());
                                            
                                            var textBoxElement = radControl_source._textBoxElement;
                                            textBoxElement.className = textBoxElement.className + ' text_ctrl';
                                            //radControl_source.Skin = "Simple";

                                            var valNW = (radControl_destination.get_value() - hdValueTotal.val()) + radControl_source.get_value(); 'Rest older value and summary the new one'

                                            radControl_destination.set_value(valNW);                                           

                                      }



                                    Function.prototype.indice = function () {
                                        var method = this, args = Array.prototype.slice.call(arguments);                                        
                                        return function () {
                                            return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                                        };
                                    };


                                    function $$(id, context) {
                                        var el = $("#" + id, context);
                                        if (el.length < 1)
                                            el = $("[id$=_" + id + "]", context);
                                        return el;
                                    }                                  

                                   

                             function grd_documents_Refresh(sender, eventArgs) {

                                        var item = eventArgs.get_item();

                                        var id_programa = parseInt(<%=Me.Session("E_IDPrograma")%>);
                                        var app_TipoDoc = item.get_value();
                                        $('input[id*=hd_TipoDoc]').val(app_TipoDoc);
                                        var idDocs = '';
                                                                                                                 
                                     //alert("{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDocs:'" + idDocs + "' }");                                                                                                                         
                                         
                                     grdDocs_ = $find("<%=grd_documentos.ClientID%>").get_masterTableView();                          
                                                  
                                                    $.ajax({

                                                        type: "POST",
                                                        url: "frm_DeliverableDocs.aspx/get_DocTYPE",
                                                        data: "{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDocs:'" + idDocs + "' }",
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        success: function (data) {

                                                            jsonResult = data.d;
                                                            //alert('Result ' + jsonResult);

                                                            var jsonResultARR = jsonResult.split('||');
                                                            //  alert('Result ' + jsonResultARR[0]);

                                                            if (jsonResultARR[0] != '[]') {//Documents Type

                                                                //fillCombo(comboPeriodo, jsonResultARR[0]);                      
                                                                var data = jQuery.parseJSON(jsonResultARR[0]);

                                                                //alert(data);
                                                                grdDocs_.set_dataSource(data);
                                                                grdDocs_.dataBind();
                                                                //grdDocs_.rebind();


                                                            }


                                                        },
                                                        failure: function (response) {
                                                            Populating = false;
                                                            alert('Error Loagind Data: ' + response.d);
                                                        }
                                                    });

                                                                              
                                      }


                    //var itemIndex = 0;
                    //function OnDataBound(sender, eventArgs) {
                    //    itemIndex = 0;
                    //}



           function RowDataBound(sender, args) {

                     // conditional formatting  
                     //var chk_control = args.get_item().findControl('chkSelect');
                     var chk_control = args.get_item().findElement('chkSelect');
                     var ToogleControl = args.get_item().findControl('ToggleButtonExtender1');    

                      //var row = masterTable.get_dataItems();
                     console.log('check Control Toogle ' +  ToogleControl);

               console.log('Checked Value Before: ' + chk_control.checked);
               //chk_control.checked = false;

               if (chk_control.checked) {
                   chk_control.click();
               }
               
               console.log('Checked Value After: ' + chk_control.checked);

                     //var arrCTRL = getRowControls(args.get_item(), 'chkSelect');

                     //alert(args.get_item().get_cell('colm_select'));
                     //alert(args.get_item().findElement('chkSelect'));

                     //alert($$("chkSelect", args.get_item().get_cell('colm_select')).attr("id"))
                          
                     // var contactName = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[0], "ContactName").innerHTML
                     //alert(arrCTRL.length);

                    // alert(args.get_dataItem()["nombre_documento"]);
                                                                   

                    //alert(chk_control.checked);

                    //if (args.get_dataItem()["descripcion_cat"] == "Dr.") {
                    //    args.get_item().get_cell("TitleOfCourtesy").style.fontWeight = "bold";
                    //}

                    //   var sb = new Sys.StringBuilder();
                    //   sb.appendLine("<b>RowDataBound</b><br />");
                    //   for (var item in args.get_dataItem()) {
                    //       sb.appendLine(String.format("{0} : {1}<br />", item, args.get_dataItem()[item]));
                    //   }

                    //   sb.appendLine("<br />");
                    //   sb.appendLine("<br />");

                    // itemIndex++;

                    }



                    //*****************************************************************************************************

                        function getRowControls(row, controlName) {

                                var result = Array();
                                var grid = $find("<%=grd_documentos.ClientID%>");
                                var master = grid.get_masterTableView();
                                var columns = master.get_columns();

                                //for (var i = 0; i < columns.length; i++) {
                                for (var i = 0; i < 4; i++) {

                                    alert(row.get_cell(columns[i].get_uniqueName()));
                                    //findElement('chkSelect')

                                    //row.get_cell(columns[i].get_uniqueName()).fid

                                    //var control = getControlFromCell(row.get_cell(columns[i].get_uniqueName()), controlName);
                                    //if (control)
                                    //    result.push(control);

                                }

                                return result;

                            }

                            function getControlFromCell(cell, ctlName) {

                                if (cell == null) return null;
                                var cellControls = cell.getElementsByTagName("*");

                                console.log(cellControls.length);

                                for (var a = 0, b = cellControls.length; a < b; a++) {
                                    var f = cellControls[a].id;
                                    //alert(f);
                                    if (f && f.endsWith(ctlName)) {
                                        return $find(f);
                                    }
                                }
                                return null;
                            }


                    //********************************************************************************************************


                                function CheckedChangedDOCS(sender) {

                                   // alert('is it selected? :' + sender.checked);                                                                                                                              
                                   // alert('Value :' + sender.value);                                                                                                                               

                                    //$('input[id*=hd_id_doc_support]').val(sender.value);

                                    //var typeDoc = $('input[id*=hd_id_doc_support]').val();
                                    //alert('Value Hidden :' + typeDoc);
                                                                                                                                       
                                     <%--  var dvElement = $("#<%=dv_DOC_TYPE.ClientID%>");
                                     dvElement.hide();--%>

                                     // $('#dv_DOC_TYPE').css('display', 'none');
                                     //$('#dv_DOC_TYPE').addClass("hidden");

                                    if (sender.checked)
                                        div_Control(false);
                                    else
                                        div_Control(true);
                                                                                                                                                                                                                                                                                                                                    
                                    var radSync = $find("<%= RadSync_NewFile.ClientID %>");                                                                                                                              
                                    radSync.set_enabled(true);

                                    //radSync.click();
                                    // radSync.enabled = true;
                                    //alert(radSync.enabled);
                                    //alert($('#dv_DOC_TYPE').html);
                                    //$('#dv_DOC_TYPE').addClass("hidden");
                                    <%--$find("<%= msg_document_type.ClientID %>").hide();--%>


                                }

                               
                    function div_Control(bolShowUP) {

                        // var dvElement = $("#%=dv_DOC_TYPE.ClientID%>");
                        //alert($('#dv_DOC_TYPE').html);

                        if (bolShowUP) {
                            //dvElement.show();
                            $('#dv_DOC_TYPE').css('display', 'block');
                        } else {
                            //dvElement.hide();
                            $('#dv_DOC_TYPE').css('display', 'none');
                        }

                    }
                    
                    function Refresh_DOC_grid() {
                                                                                                                               
                        var id_programa = parseInt(<%=Me.Session("E_IDPrograma")%>);
                        var comboboxType = $find("<%= cmb_approvals.ClientID %>");
                        var app_TipoDoc = comboboxType.get_selectedItem().get_value();
                        var idDocs = $('input[id*=hd_files_selected]').val();

                          grdDocs_ = $find("<%=grd_documentos.ClientID%>").get_masterTableView();                          

                          //alert("Refresh_DOC_grid:  {idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDocs:'" + idDocs + "' }");
                        
                          $.ajax({

                              type: "POST",
                              url: "frm_DeliverableDocs.aspx/get_DocTYPE",
                              data: "{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDocs:'" + idDocs + "' }",
                              contentType: "application/json; charset=utf-8",
                              dataType: "json",
                              success: function (data) {

                                  jsonResult = data.d;
                                  //alert('Result ' + jsonResult);

                                  var jsonResultARR = jsonResult.split('||');

                                  //alert('Result ' + jsonResultARR[0]);

                                  if (jsonResultARR[0] != '[]') {//Documents Type

                                      //fillCombo(comboPeriodo, jsonResultARR[0]);                      
                                      var data = jQuery.parseJSON(jsonResultARR[0]);


                                      //var hdValueTotal = $('input[id*=hd_percent]');
                                      ////  alert(hdValueTotal.val());
                                      //set_Percent(hdValueTotal.val());

                                      //alert(data);
                                      grdDocs_.set_dataSource(data);
                                      grdDocs_.dataBind();
                                      //grdDocs_.rebind();                                                                       

                                  }


                              },
                              failure: function (response) {
                                  Populating = false;
                                  alert('Error Loagind Data: ' + response.d);
                              }
                          });
                                                      
                        //alert('Finsh Result ' + jsonResult);
                                                 
                      

                     }


                                                        
                    function changeUpload(fileUploaded) {

                            document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;                                                                                                           
                                
                        <%-- var img = document.getElementById("<%= imgUser.ClientID%>");
                        //img.className = "hidden";
                        document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;--%>

                    }


                       function AddItem(fileUploaded){

                       // alert(fileUploaded);
                       //hd_id_doc_support.Value = 
                       //var hd_id_doc_supportument = $find("<%= hd_id_doc_support.ClientID %>");
                           
                       var typeDoc = $('input[id*=hd_id_doc_support]').val();
                        <%-->   $find("<%= hd_id_doc_support.ClientID %>").val(); //bind with a propper document type value --%>
                        // alert('Type of File: ' + typeDoc);

                           var rdList = $find("<%= rdListBox_files.ClientID %>");   
                           var items = rdList.get_items();
                           rdList.trackChanges();
                           var item = new Telerik.Web.UI.RadListBoxItem();

                           item.set_text(fileUploaded);
                           item.set_value(typeDoc);
                           items.add(item);
                           rdList.commitChanges();
                           
                           //alert('typeDoc: ' + typeDoc);
                           var idDocs = $('input[id*=hd_files_selected]').val() + ',' + typeDoc;                           
                           $('input[id*=hd_files_selected]').val(idDocs);
                           //alert('Id Docs: ' + idDocs);
                                                                                                                                                             
                           // var dvElement = $("#%=dv_DOC_TYPE.ClientID%>");
                           //dvElement.show();

                            <%--      dvElement = document.getElementById('<%=dv_DOC_TYPE.ClientID%>')
                            //dvElement.style.display = "block";
                            alert(dvElement.innerHTML);
                            $('#<%=dv_DOC_TYPE.ClientID%>').show();--%>

                            <%-- var img = document.getElementById("<%= imgUser.ClientID%>");
                            //img.className = "hidden";
                            document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;--%>

                       }
                               
                    

                    function Check_deleted(sender, e) {

                        //alert("Successfully deleted: " + e.get_item().get_text());                                                      
                        var listAFdeleted = $find("<%= rdListBox_files.ClientID %>");
                        //alert('Elements: ' + listAFdeleted.get_items().get_count());

                        if (listAFdeleted.get_items().get_count() == 0) {                                                          
                             hasFiles("false");
                         }

                    }

                    function hasFiles(valor) {
                          document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                    }


                   </script>  

                <div class="box">

                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Deliverable Support Documents</asp:Label>
                        </h3>
                     </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">
                                                              
                                  <div class="stepwizard">
                                    <div class="stepwizard-row setup-panel">

                                        <div class="stepwizard-step" style="width:25%">
                                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                            <p>
                                                <asp:Label runat="server" ID="lblt_informaciongeneral">Deliverable Information</asp:Label>
                                            </p>
                                         </div>

                                         <div class="stepwizard-step" style="width:25%">
                                             <a   href="#step-2" id="anchorResults" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                             <p>
                                                <asp:Label runat="server" ID="lblt_personal_status">Deliverable Result</asp:Label>
                                             </p>
                                         </div>

                                         <div class="stepwizard-step" style="width:25%">
                                             <a   href="#step-3" id="anchorDocuments" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                             <p>
                                                <asp:Label runat="server" ID="lblt_deliverable_documents">Deliverable Documents</asp:Label>
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

                                      <div class="form-group row">                                          
                                         <div class="col-sm-12 text-left">
                                            <hr />
                                        </div>                                        
                                      </div>

                                      <div class="form-group row">
                                         <div class="col-sm-12">
                                                 <div class="box-body">                                 
                                                        <div class="col-md-4 col-sm-8 col-xs-16">
                                                          <div class="info-box">
                                                            <span class="info-box-icon bg-gray"><i class="fa fa-university"></i></span>
                                                            <div class="info-box-content">
                                                                 <span class="info-box-text"><%= userName %></span>
                                                                 <span class="text-bold"><%= userImplementer %></span>             
                                                            </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                        </div><!-- /.col -->
                                                         <div class="col-md-4 col-sm-8 col-xs-16">
                                                          <div class="info-box">
                                                                <span class="info-box-icon  bg-gray"><i class="fa fa-tasks"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label ID="lbl_activity_name" runat="server"></asp:Label></span>                                                           
                                                                     <span class="text-bold"><asp:Label ID="lbl_activity_Code" runat="server"></asp:Label><br /><i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>                                                                          
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
                                          
                                               <div class="col-sm-12">
                                                    <div class="box-body">    

                                                          <div class="col-md-4 col-sm-8 col-xs-16">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-money"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></span>
                                                                     <span class="text-bold">Total Activity</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->

                                                         <div class="col-md-4 col-sm-8 col-xs-16 vertical-align-center"  >                                                                                                                                                             
                                                            <div style="text-align:center; vertical-align:middle;">     
                                                                 <asp:HiddenField ID="hd_percent" runat="server" Value="0" />  
                                                               <input type="text" class="PorcDeliv" value="0" data-width="120" data-height="120" data-thickness="0.30" data-fgColor="#3c8dbc" data-skin="tron" readonly>                                                   
                                                            </div>
                                                         </div> 

                                                         <div class="col-md-4 col-sm-8 col-xs-16">
                                                            <div class="info-box">
                                                                <span class="info-box-icon bg-orange-active"><i class="fa fa-line-chart"></i></span>
                                                                <div class="info-box-content">
                                                                     <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalPerf" Text ="0 UGX"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0 USD"></asp:Label></span>
                                                                     <span class="text-bold">Total Performed</span>             
                                                                </div><!-- /.info-box-content -->
                                                            </div><!-- /.info-box -->
                                                         </div><!-- /.col -->

                                                   </div>
                                               </div>                             
                                      </div>
                                    
                                         <div class="form-group row">
                                             <div class="col-sm-12">
                                                   <!-- TABLE: ACTIVITY DELIVERABLE -->
                                                      <div class="box box-info">
                                                        <div class="box-header with-border">
                                                          <h3 class="box-title">DELIVERABLE INFO</h3>
                                                          <div class="box-tools pull-right">
                                                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                            <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                          </div>
                                                        </div><!-- /.box-header -->
                                                        <div class="box-body">
                                                          <div class="table-responsive">
                                                                                                    
                                                              <div class="col-sm-12">
                                                                   <%-- <div class="col-sm-4 pull-right">
                                                                     <div style="text-align:right;">                                                 
                                                                          <h4>Total Activity: <asp:Label runat="server" ID="lbl_totalACT" Text ="0 UGX"></asp:Label></h4>
                                                                          <h4><asp:Label runat="server" ID="lbl_totalACT_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                                          <h4>Total Performed: <asp:Label runat="server" ID="lbl_totalPerf" Text ="0 UGX"></asp:Label></h4>
                                                                          <h4><asp:Label runat="server" ID="lbl_totalPerf_usd" Text ="0 USD"></asp:Label></h4><hr style="border-color:black;" />
                                                                          <h3>Pending: <asp:Label runat="server" ID="lbl_totalPend" Text ="0 UGX"></asp:Label></h3>
                                                                          <h3><asp:Label runat="server" ID="lbl_totalPend_usd" Text ="0 USD"></asp:Label></h3>
                                                                     </div>
                                                                   </div>--%>

                                                                   <asp:HiddenField runat="server" ID="hd_id_deliverable" Value="0" />
                                                                   <asp:HiddenField runat="server" ID="hd_id_ficha_entregable" Value="0" />  
                                                                  <asp:HiddenField runat="server" ID="hd_performed" Value="0.0" />                                                   
                                                                  <asp:HiddenField ID="hd_tasa_cambio" runat="server" Value="0" />
                                                      
                                                                    <div  class="box-body table-responsive no-padding">
                                                                       <%--
                                                                           
                                                                           id="dvNEXT_delieverable" runat="server"

                                                                              <table class="table table-hover">
                                                                            <tr>
                                                                              <th>Deliverable #</th>
                                                                              <td><span class="badge bg-primary">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                            </tr>
                                                                            <tr>                                                                  
                                                                              <td colspan="2">Develop a curriculum and execution plan for Executive Course on Management Information Systems and completion of staff recruitment for 10 interns (below 35 years of age), 6 new staff, and 5 short term consultants.</td>                                                                  
                                                                            </tr>
                                                                            <tr>
                                                                              <th>Due Date</th>
                                                                              <td>25/10/2018</td>   
                                                                            </tr>     
                                                                            <tr>
                                                                              <th>Status</th>
                                                                              <td><span class='label label-danger'>Expired&nbsp;<i class='fa fa-clock-o'></i>&nbsp;3&nbsp;Weeks</span></td>   
                                                                            </tr>  
                                                                             <tr>
                                                                              <th>Porcent</th>
                                                                              <td>25%</td>   
                                                                            </tr>   
                                                                            <tr>
                                                                              <th>Amount</th>
                                                                              <td>305,456,789 UGX / 258,325.56 USD</td>   
                                                                            </tr>                                                               
                                                                          </table>--%>

                                                                   <table class='table table-hover'>

                                                                        <asp:Repeater ID="rep_DelivINFO" runat="server" >
                                                                                  <ItemTemplate> 
                                                                                                <tr>

                                                                                                   <th>Deliverable #</th>
                                                                                                    <td class='vertical-align-center' ><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;<%# Eval("numero_entregable")%>&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                                                    <th>Amount</th>
                                                                                                    <td> <%# String.Format(cultureUSer, "{0:N2} {1}", Eval("valor"), cultureUSer.NumberFormat.CurrencySymbol) %> / <%# String.Format("{0:N2}", Math.Round((Eval("valor") / Convert.ToDouble(Eval("tasa_cambio"))), 2, MidpointRounding.AwayFromZero))%> USD</td>   
                                                                                                    <th>Porcent</th>
                                                                                                    <td><%# String.Format("{0:P2}", (Eval("porcentaje") / 100))%> </td> 

                                                                                                </tr>                                                                                                                                                                                                                                                                  
                                                                                                <tr>
                                                                                                    <th>Due Date</th>
                                                                                                    <td>  <%# String.Format("{0:d}", Eval("fecha"))%>  </td>   
                                                                                                    <th>Status</th>
                                                                                                    <td><span class='label <%# Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 1) %>' > <%# If(Eval("numero_entregable") = 0, Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 2), Eval("deliverable_estado"))  %>   &nbsp;<i class='fa fa-clock-o'></i>&nbsp;<%# Func_Unit(Eval("D_Days"))%></span></td>                                                                                                                                                                                                                                                                                               
                                                                                                    <th></th>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                 <tr>                                                                  
                                                                                                  <td colspan='6'>
                                                                                                     <div class='text-justify' style='max-width:100%;'>
                                                                                                         <%# Eval("descripcion_entregable") %>
                                                                                                     </div>
                                                                                                   </td>                                                                  
                                                                                                </tr>   
                                                                       
                                                                                      </ItemTemplate>
                                                                          </asp:Repeater>
                                                                                                                                  
                                                                      </table>

                                                                  </div><!-- /.box-body -->
                                                                                                              
                                                                </div>
                                                                <div class="col-sm-12">
                                                                     <hr style="border-color:black;" />
                                                               </div> 

                                                           <%-- <table class="table no-margin">
                                                              <thead>
                                                                <tr>
                                                                  <th style="width:2%;"></th>                                   
                                                                  <th style="width:3%;">#</th>                    
                                                                  <th style="width:25%;">Milestone</th>
                                                                  <th style="width:35%;">Verification</th>
                                                                  <th style="width:8%;">Due Date</th>
                                                                  <th style="width:8%;">Delivered Date</th>
                                                                  <th style="width:3%;">%</th>
                                                                  <th style="width:8%;">Amount</th>
                                                                  <th style="width:8%;">Status</th>
                                                                </tr>
                                                              </thead>
                                                              <tbody>                                                   
                                                                            <tr>
                                                                               <td><div class="tools"><a href="/RMS_APPROVAL/approvals/frm_seguimientoAprobacionRep.aspx?IdDoc=0&IdRuta=0" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                               <td>1</td>
                                                                               <td>A database of 120 youth,  40% women, service providers in the maize and beans value chains</td>
                                                                               <td>
                                                                                   <div style="overflow:scroll; text-align:left; max-width:100%; max-height:300px;">
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                       120 Youth village agents trained in the different services (which include; Digital Profiling, Crop Insurance Agency, Soil Testing, Inputs Sales, Planting, Weeding, Digital Financial Services, Field Harvesting, Shelling, Grain Cleaning, Drying and Bulking)
                                                                                    </div>

                                                                               </td>
                                                                               <td>2017-12-04</td>
                                                                               <td>--</td>
                                                                               <td>24.89%</td>
                                                                               <td>35,498,615.30</td>
                                                                               <td>                                                                       
                                                                                  <span class="label label-danger">Expired&nbsp;<i class="fa fa-clock-o"></i>&nbsp;15&nbsp;Weeks</span>
                                                                               </td>
                                                                            </tr>
                                                                            <tr>
                                                                               <td colspan="9">                                                                     

                                                                                    <div class="progress">
                                                                                        <div class="progress-bar progress-bar-warning progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                          <span>11% (Planned)</span>
                                                                                        </div>
                                                                                        <div class="progress-bar progress-bar-danger   progress-bar-striped" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                          <span >22% (Delayed)</span>
                                                                                        </div>
                                                                                      </div>

                                                                               </td>  
                                                                            </tr>                      
                                                      
                                                                               <tr>
                                                                               <td><div class="tools"><a href="/RMS_APPROVAL/approvals/frm_seguimientoAprobacionRep.aspx?IdDoc=<%# Eval("id_documento")%>&IdRuta=<%# Eval("id_ruta")%>" target="_blank" ><i class="fa fa-search" ></i></a></div>  </td>
                                                                               <td>2</td>
                                                                               <td>A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.</td>
                                                                               <td>
                                                                                   <div style="overflow-y:auto; text-align:left; max-width:100%; max-height:300px;">
                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.

                                                                                       A database of 5200 profiled farmers ( 50% will be women) supported and benefiting from the youth village agent services.
                                                                                    </div>

                                                                               </td>
                                                                               <td>2017-02-28</td>
                                                                               <td>--</td>
                                                                               <td>22.79%</td>
                                                                               <td>32,503,553.34</td>
                                                                               <td>                                                                       
                                                                                  <span class="label label-warning">Pending&nbsp;<i class="fa fa-clock-o"></i>&nbsp;6&nbsp;Days</span>
                                                                               </td>
                                                                            </tr>
                                                                            <tr>
                                                                               <td colspan="9">                                                                     

                                                                                    <div class="progress">
                                                                                        <div class="progress-bar  progress-bar-warning  progress-bar-striped" role="progressbar" aria-valuenow="22" aria-valuemin="0" aria-valuemax="100" style="width: 22%">
                                                                                          <span >22% (On Time)</span>
                                                                                        </div> 
                                                                                        <div class="progress-bar progress-bar-primary progress-bar-striped" role="progressbar" aria-valuenow="8" aria-valuemin="0" aria-valuemax="100" style="width: 8%">
                                                                                          <span>30% (Planned)</span>
                                                                                        </div>                                                                            
                                                                                      </div>
                                                                               </td>  
                                                                            </tr>                                                                                            
                                                              </tbody>
                                                            </table>--%>
                                                             <%-- <div ID="ltr_rows_Deliverables" runat="server" ></div>--%>                                              
                                             
                                                                        <div class="col-sm-2 text-left">
                                                                         <!--Tittle -->
                                                                            <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                                        </div>
                                                                       <div class="col-sm-8">                                                   
                                                                              <div ID="dv_description" Runat="server" class=" text-justify ">

                                                                              </div>
                                                                           <!--Control -->                                              
                                                                       </div>   
                                                                       <div class="col-sm-12 text-left">
                                                                           <hr />
                                                                       </div>                                             
                                                                      <div class="col-sm-2 text-left">
                                                                         <!--Tittle -->
                                                                         <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                                
                                                                       </div>
                                                                      <div class="col-sm-8">
                                                                            <div ID="dv_notes" Runat="server" class=" text-justify ">

                                                                              </div>
                                                                           <!--Control -->                                                                                     
                                                                       </div>

                                                          </div><!-- /.table-responsive -->

                                                          <div class="col-sm-12">
                                                              <hr style="border-color:black;" />
                                                          </div>    
                                                            
                                                            
                                                          <!--***************************** HERE ******************************-->
                                                         <div class="form-group row">
                                                            <div class="col-sm-12">
                                                               <h4 class="box-title">REPORTED INDICATOR</h4>  <hr /> 
                                                            </div>                                                             

                                                            <div class="col-sm-12">


                                                                  <!--**************************HERE THE INDICATOR RESULT*************************************-->
                                                                 
                                                                              <table class="table table-striped">
                                                                                <tr>
                                                                                  <th></th>
                                                                                  <th style="width:50%;">Indicator</th>
                                                                                  <th>Target</th>
                                                                                  <th>Prev Progress</th>
                                                                                  <th>Curr Progress</th>             
                                                                                  <th></th>                                             
                                                                                </tr>
                                                                                   <asp:Repeater ID="reptTable" runat="server" OnItemDataBound="reptTable_ItemDataBound" >
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                              <td>
                                                                                                  &nbsp;<%# Eval("codigo_indicador")%><asp:HiddenField runat="server" ID="hd_id_meta_indicador_ficha" Value=<%# Eval("id_meta_indicador_ficha")%>  />
                                                                                                  <asp:HiddenField runat="server" ID="hd_deliverable_reported" Value="0"  /> 
                                                                                              </td>
                                                                                              <td class="text-justify">    
                                                                                                <%# Eval("nombre_indicador_LB")%>                                                                                                                                                         
                                                                                              </td>
                                                                                              <td><%# Eval("meta_total")%></td>
                                                                                              <td><%# Eval("avance_previo")%></td>
                                                                                              <td class="text-bold" ><telerik:RadNumericTextBox  runat="server" ID="txt_value" name="txt_value"  Width="125px" Value="0" ></telerik:RadNumericTextBox>
                                                                                                  <asp:HiddenField runat="server" ID="hd_value" Value=<%# Eval("avance_actual")%>  />                                                                                                  
                                                                                              </td>                                                         
                                                                                              <td><span class="badge <%# Eval("bg_color")%>"> <%# Eval("porc_total")%>%</span></td>
                                                                                            </tr>                                                                                        
                                                                                            <tr>
                                                                                               <td colspan="6">
                                                                                                <div class="progress">
                                                                                                    <div class="progress-bar <%# Eval("progress_bar_previo")%>" role="progressbar<%# Eval("id_meta_indicador_ficha")%>" aria-valuenow="<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo")) %>" aria-valuemin="0" aria-valuemax="100" style="width:<%# If(Eval("porc_previo") > 100, 100, Eval("porc_previo"))%>%">
                                                                                                      <span><%# Eval("porc_previo")%>%</span>
                                                                                                    </div>
                                                                                                    <div class="progress-bar <%# Eval("progress_bar_actual")%>" role="progressbar<%# Eval("id_meta_indicador_ficha")%>" aria-valuenow="<%# If(Eval("porc_actual") > 100, 100, Eval("porc_actual"))%>" aria-valuemin="0" aria-valuemax="100" style="width:<%# If(Eval("porc_actual") > 100, 100, Eval("porc_actual")) %>%">
                                                                                                      <span ><%# Eval("porc_actual")%>%</span>
                                                                                                    </div>
                                                                                                  </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                 </asp:Repeater>                                                                             
                                                                                    <%--    <div class="progress progress-xs">
                                                                                              <div class="progress-bar <%# Eval("progress_bar")%>" style="width:<%# Eval("progress_value")%>%"></div>
                                                                                            </div>
                                                                                                                                                                              
                                                                                            <div class="progress">
                                                                                                    <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span>11% (Planned)</span>
                                                                                                    </div>
                                                                                                    <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="11" aria-valuemin="0" aria-valuemax="100" style="width: 11%">
                                                                                                      <span >22% (Delayed)</span>
                                                                                                    </div>
                                                                                                  </div>--%>                                                                                                                                                                                                                   
                                                                              </table>

                                                                   <!--**************************HERE THE INDICATOR RESULT*************************************-->
                                                                

                                                             </div>

                                                          </div>
                                                          <!--***************************** HERE ******************************-->



                                                        </div><!-- /.box-body -->

                                                          <div class="box-footer clearfix">                                                

                                                             
                                                   
                                                           </div> <!-- /.box-footer -->                                                                                            
                                         
                                                </div><!-- /.box -->

                                            </div>
                            
                                        </div>                             
                                                                                                                                               
                                            <div class="form-group row">
                                               <div class="col-sm-12 text-left">

                                                      <!-- TABLE:  DELIVERABLE DOCUMENTS--> 
                                                   
                                                            <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">DELIVERABLE DOCUMENTS</h3>
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding" style="padding-left:10px;" >                                 
                                                                            
                                                                       <div class="form-group row"  >
                                                                           <br />
                                                                           <div class="col-sm-2 text-left">
                                                                              <asp:Label ID="lblt_Approval" runat="server" CssClass="control-label text-bold" >Approval Flow</asp:Label>
                                                                           </div>
                                                                           <div class="col-sm-8 text-left">
                                                                                          <telerik:RadComboBox  ID="cmb_approvals" 
                                                                                                                runat ="server"  
                                                                                                                CausesValidation  ="False"                                                                     
                                                                                                                EmptyMessage="Select the approval ..."   
                                                                                                                AllowCustomText="false" 
                                                                                                                Filter="Contains"                                                          
                                                                                                                Width="45%" >                                                                                               
                                                                                           </telerik:RadComboBox>
                                                                                           <br /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="The approval flow is mandatory to continue. If you don't have approval flow assigned, please contact the system administrator." ForeColor="Red" ControlToValidate="cmb_approvals" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                  
                                                                               <asp:HiddenField ID="hd_id_documento_deliverable" runat="server" Value="0" />
                                                                               <asp:HiddenField ID="hd_id_documento" runat="server" Value="0" />
                                                                                                                                                                                        
                                                                               <%--OnClientSelectedIndexChanged="grd_documents_Refresh"--%>
                                                                                                              
                                                                              </div> 
                                                                           <br /><br />
                                                                         </div>


                                                                          <div class=" row">
                                                                                                <div class="col-sm-2 text-left">
                                                                                                 <!--Tittle -->
                                                                                                     <asp:Label ID="lblt_documents_sel" runat="server" CssClass="control-label text-bold"   Text="Support Document Selector"></asp:Label>
                                                                                                </div>
                                                                                               <div class="col-sm-10">
                                                                                                   <!--Control -->
                                                                                                                
                                                                                                      <telerik:RadGrid ID="grd_documentos"  
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
                                                                                                                              <ClientEvents OnRowDataBound="RowDataBound" />
                                                                                                                         </ClientSettings>
                                                                                                                         <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_doc_soporte" >
                                                                                                                            
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
                                                                                                                                                         HeaderText="Sel">
                                                                                                                                                        <ItemTemplate>
                                                                                                                                                            <asp:CheckBox ID="chkSelect" runat="server" 
                                                                                                                                                                AutoPostBack="True" 
                                                                                                                                                                OnClick="CheckedChangedDOCS(this);"   
                                                                                                                                                                oncheckedchanged="chkVisible_CheckedChangedDOCS" />
                                                                                                                                                                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                                                                                                                    CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                                                                                                                    TargetControlID="chkSelect" UncheckedImageUrl="~/Imagenes/iconos/icon-warningAlert.png">
                                                                                                                                                                </ajaxToolkit:ToggleButtonExtender>
                                                                                                                                                        </ItemTemplate>                                                                                                
                                                                                                                                                        <ItemStyle Width="5%" />
                                                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                                                   <telerik:GridBoundColumn  DataField="id_doc_soporte" DataType="System.Int32" 
                                                                                                                                                        FilterControlAltText="Filter id_categoria column" HeaderText="id_doc_soporte" 
                                                                                                                                                        SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" 
                                                                                                                                                        Visible="true" Display="false">
                                                                                                                                                    </telerik:GridBoundColumn>

                                                                                                                                                <telerik:GridBoundColumn DataField="nombre_documento" 
                                                                                                                                                    FilterControlAltText="Filter descripcion_cat column" 
                                                                                                                                                    HeaderText="Name of document" SortExpression="nombre_documento" 
                                                                                                                                                    UniqueName="descripcion_cat">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                                <telerik:GridBoundColumn DataField="extension" 
                                                                                                                                                    FilterControlAltText="Filter extension column" HeaderText="Allowed extension" 
                                                                                                                                                    SortExpression="extension" UniqueName="extension">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                               <telerik:GridBoundColumn DataField="max_size" 
                                                                                                                                                       FilterControlAltText="Filter max_size column" HeaderText="Max Size (MB)" 
                                                                                                                                                       SortExpression="max_size" UniqueName="colm_max_size" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
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
                                                                                                                                                  <ItemStyle Width="30%" />
                                                                                                                                               </telerik:GridTemplateColumn>
                                                                                                                                     
                                                                                                                                   </Columns>
                                                                                                                                       
                                                                                                                             </MasterTableView>

                                                                                                                             <FilterMenu EnableImageSprites="False">
                                                                                                                                <WebServiceSettings>
                                                                                                                                   <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                                                                </WebServiceSettings>
                                                                                                                             </FilterMenu>

                                                                                                           </telerik:RadGrid>  
                                                                                                   <br />                                                                                                 
                                                                                                   <br />
                                                                                                   <asp:HiddenField ID="hd_id_doc_support" runat="server" Value="0" />                                                                                                                                              
                                                                                                   <asp:Label ID="lbl_errExtension" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="false" Text="Select a type of document"></asp:Label> 
                                                                                                                           
                                                                                               </div>
                                                                                            </div>



                                                                                           <div class=" row">

                                                                                                   <div class="col-sm-2">
                                                                                                     <!--Tittle -->
                                                                                                      <asp:Label ID="lblt_new_file_" runat="server"  CssClass="control-label text-bold" Text="Attach New File"></asp:Label>                                             
                                                                                                    </div>
                                                                                                    <div class="col-sm-3 ">
                                                                                                       <!--Control --> <%----Here New file control--%>
                                              
                                                                                                                       <telerik:RadAsyncUpload 
                                                                                                                            RenderMode="Lightweight" 
                                                                                                                            runat="server" ID="RadSync_NewFile" 
                                                                                                                            MultipleFileSelection="Disabled"
                                                                                                                            Skin="Silk" 
                                                                                                                            TemporaryFolder="~/FileUploads/Temp/" 
                                                                                                                            TargetFolder="~/FileUploads/Temp/"     
                                                                                                                            HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx"                                                
                                                                                                                            OnClientValidationFailed="validationFailed"                                                
                                                                                                                            UploadedFilesRendering="AboveFileInput"  
                                                                                                                            OnClientProgressUpdating="onClientFileUploading"                                               
                                                                                                                            OnClientFileUploaded="file_approval_Uploaded"                                           
                                                                                                                            TemporaryFileExpiration="1:00:00" Enabled="false" >
                                                                                                                         </telerik:RadAsyncUpload>

                                                                                                                    <script src="../scripts/FileUploadTelerik.js?V=0.09"></script>
                                    
                                                                                                                     <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                                                                                                     <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                                                                                                     <asp:HiddenField runat="server" ID="lbl_oldFile" />
                                                                                                   
                                                                                                         <div id="dv_DOC_TYPE" style="width:300px; height:40px; position:absolute; top:0px; left:10px; z-index:1;" >
                                                                                                                <span class="badge bg-orange"><h4><i class="fa fa-hand-pointer-o"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Select document type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-pointer-o"></i></h4></span> 
                                                                                                         </div>

                                                                                                   </div>

                                                                                                <div class="col-sm-5 ">

                                                                                                        

                                                                                                </div>

                                                                                             </div>


                                                                                             <div class="form-group row">
                                                                                                        <div class="col-sm-2">
                                                                                                         <!--Tittle --><br /><br />
                                                                                                             <asp:Label ID="lblt_listName" runat="server" CssClass="control-label text-bold"   Text="Attached Files"></asp:Label>
                                                                                                        </div>
                                                                                                       <div class="col-sm-10">
                                                                                                           <!--Control --><br /><br />                                                                                                      
                                                                                                           <telerik:RadListBox CssClass="pull-left" 
                                                                                                               RenderMode="Lightweight" 
                                                                                                               OnClientDeleted="Check_deleted" 
                                                                                                               runat="server" 
                                                                                                               ID="rdListBox_files" 
                                                                                                               Height="200px"                                                                                                                 
                                                                                                               Font-Bold="true"
                                                                                                               Font-Size="Small"                                                                                                               
                                                                                                               Width="70%" 
                                                                                                               AllowDelete="true" 
                                                                                                               AllowReorder="false" 
                                                                                                              ButtonSettings-AreaWidth ="40px" ></telerik:RadListBox>                                                                                    
                                                                                                             <asp:HiddenField ID="hd_has_files" runat="server" Value="0" />       
                                                                                                             <asp:HiddenField ID="hd_files_selected" runat="server" Value="0" />                                                                                                  
                                                                                                       </div>
                                                                                                </div>

                                                                                          
                                                                          
                                                                                                                                                      

                                                                </div><!-- /.box-body -->

                                                                <div class="box-footer clearfix"> 
                                                                   <div class="col-sm-12">
                                                                      <hr style="border-color:black;" />
                                                                   </div>    
                                                                </div> <!-- /.box-footer -->                                                                                            
                                         
                                                                </div><!-- /.box -->   
                                                   
                                                     
                                                   <!-- TABLE:  DELIVERABLE DOCUMENTS-->     

                                               </div>
                                            </div>                                 
                                       


                                  </div>
                                
                               </div> 
              
                          </div>
                        
                   </div>

                  <%--  </div>--%>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->                    
                       
			          <div class="form-group row">
                             <div class="col-sm-2 text-left">
                               <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm " Width="100" ValidationGroup="1" ></telerik:RadButton>
                             </div>
                            <div class="col-sm-2 text-left">
                               <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Save and Continue" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-o-right"></i></asp:LinkButton>                                              
                            </div>
                            <div class="col-sm-8 text-left">
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                               <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                            </div>
                       </div>       
                                                                
                   </div>
                
                <%--</div>--%>

           </section>



    
    

    </asp:Content>

