<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetDocs"  Codebehind="frm_TimeSheetDocs.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEET</asp:Label>
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

             <%--   <script src="../Content/plugins/knob/jquery.knob.js"></script>--%>
                <script type="text/javascript">



                                                        //$(document).ready(function () {

                                                            
                                                        //    var hdValueTotal = $('input[id*=hd_percent]');

                                                        //  //  alert(hdValueTotal.val());

                                                        //          $('.PorcDeliv').val(hdValueTotal.val());
                                                        //          $('.PorcDeliv').trigger('change');
                                                        //          $('.PorcDeliv').css('font-size', '16px');
                                                        //          $(".PorcDeliv").knob({
                                                        //              'format': function (value) {
                                                        //                  return value + '%';
                                                        //              }
                                                        //          });
                                                        //        })                                                                                                                                                                                                                                               


                                                            //function set_Percent(valPercent) {

                                                            //    //alert('Percent value ' + valPercent);

                                                            //      $('.PorcDeliv').val(valPercent);
                                                            //      $('.PorcDeliv').trigger('change');
                                                            //      $('.PorcDeliv').css('font-size', '16px');
                                                            //      $(".PorcDeliv").knob({
                                                            //          'format': function (value) {
                                                            //              return value + '%';
                                                            //          }
                                                            //      });

                                                            //  }
                                                                                                                            

                                                       
                                      //  function Calculate(sender, args, params) {

                                      //      //alert(params);

                                      //      var strID = params.toString().split("||");

                                      //      //alert(strID[0]);
                                      //      //alert(strID[1]);

                                      //      var idMeta_ficha = strID[0]; //params.toString().split("||")[0];
                                      //      var idAvance_ficha = strID[1]; //params.toString().split("||")[1];
                                            
                                      //      var ctrl_name = 'txt_value_' + idMeta_ficha;
                                      //      var ctrl_total_value = 'hd_ind_report_value_' + idAvance_ficha;
                                      //      var ctrl_reported_value = 'hd_ind_value_' + idAvance_ficha;
                                                           
                                      //      //alert(ctrl_name);                                                                                       
                                      //      //alert($$("txtSymbol", $("#wrapper")).attr("id"));
                                      //      //alert($$(ctrl_name).attr("id"));
                                      //      //alert($$(ctrl_name).val());

                                      //      var radControl_destination = $find($$(ctrl_name).attr("id"));
                                      //      var radControl_source = sender;
                                      //   // var hd_controlValue = $find($$(ctrl_value).attr("id"));
                                                                                       
                                      //      //alert($$(ctrl_value).attr("id"));
                                      //      //alert($$("hd_ind_report_value_40-87").attr("id"));

                                      //      //alert($('input#hd_ind_report_value_4087').val());
                                      //      //alert($(':hidden#hd_ind_report_value').val());
                                      //      //hd_ind_report_value_4087
                                      //      //alert($('').val());

                                      //     // alert(ctrl_value);
                                            
                                      //      //$.each($('input[id*=' + ctrl_value + ']'), function (i, val) {
                                      //      //    if ($(this).attr("type") == "hidden") {
                                      //      //        var valueOfHidFiled = $(this).val();
                                      //      //        alert($(this).attr("id") + ': ' + valueOfHidFiled);
                                      //      //    }
                                      //      //});

                                      //      var hdValueTotal = $('input[id*=' + ctrl_reported_value + ']');

                                      //     // alert($("[id$=_hd_ind_report_value_4087]").attr("id"));
                                                                                        
                                      //      //$$(ctrl_name);
                                      //      //alert('Total Indicator: ' + radControl_destination.get_value());
                                      //      //alert(hd_controlValue.Value);
                                      //      //alert('Total Report Changed: ' + radControl_source.get_value());
                                      //      //alert(hd_controlValue.Value);
                                      //      //alert('Total Report : ' + hdValueTotal.val());
                                            
                                      //      var textBoxElement = radControl_source._textBoxElement;
                                      //      textBoxElement.className = textBoxElement.className + ' text_ctrl';
                                      //      //radControl_source.Skin = "Simple";

                                      //      var valNW = (radControl_destination.get_value() - hdValueTotal.val()) + radControl_source.get_value(); 'Rest older value and summary the new one'

                                      //      radControl_destination.set_value(valNW);                                           

                                      //}
                    

                                    //Function.prototype.indice = function () {
                                    //    var method = this, args = Array.prototype.slice.call(arguments);                                        
                                    //    return function () {
                                    //        return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                                    //    };
                                    //};


                                    //function $$(id, context) {
                                    //    var el = $("#" + id, context);
                                    //    if (el.length < 1)
                                    //        el = $("[id$=_" + id + "]", context);
                                    //    return el;
                                    //}                                  

                                   

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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Time Sheet Documents</asp:Label>
                            <asp:HiddenField ID="hd_IDtimeSheet" runat="server" Value="0" />
                            <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />                
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
                                                                              <th>Dás registrados</th>
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

                                                    <%--    <div class="form-group row" runat="server" id="lyHistory" visible="false">
                                                            <div class="col-sm-12 text-left">

                                                                      
                                                                       <table class="table table-responsive table-condensed box box-primary ">
                                                                              <tr class="box box-default ">
                                                                                    <td  class="text-left"  colspan="2">
                                                                                    
                                                                                        <div class="box-header">
                                                                                           <i class="fa fa-history"></i>
                                                                                           <h3 class="box-title">History</h3>                                              
                                                                                        </div>
                                        
                                                                                    </td>
                                                                                  </tr>
                                                                                <tr>
                                                                                    <td  colspan="2" class="text-left">
                                                                                        <br />                                    
                                                                                       
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
                                                                                                                         
                                          
                                                                                    </td>
                                                                                </tr>  
                                                                        </table>

                                                                </div>
                                                            </div>--%>


                                                      </div>
                                                   </div> 


                                                                                                                                               
                                            <div class="form-group row">
                                               <div class="col-sm-12 text-left">

                                                      <!-- TABLE:  DELIVERABLE DOCUMENTS--> 
                                                   
                                                            <div class="box box-info">
                                                                <div class="box-header with-border">
                                                                  <h3 class="box-title">Soportes</h3>
                                                                  <div class="box-tools pull-right">
                                                                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                                  </div>
                                                                </div><!-- /.box-header -->
                                                                <div class="box-body no-padding" style="padding-left:10px;" >                                 
                                                                            
                                                                       <div class="form-group row"  >
                                                                           <br />
                                                                           <div class="col-sm-2 text-left">
                                                                              <asp:Label ID="lblt_Approval" runat="server" CssClass="control-label text-bold" >Flujo de aprobación</asp:Label>
                                                                           </div>
                                                                           <div class="col-sm-8 text-left">
                                                                                          <telerik:RadComboBox  ID="cmb_approvals" 
                                                                                                                runat ="server"  
                                                                                                                AutoPostBack="True"
                                                                                                                CausesValidation  ="False"                                                                     
                                                                                                                EmptyMessage="Select the approval ..."   
                                                                                                                AllowCustomText="false" 
                                                                                                                Filter="Contains"                                                          
                                                                                                                Width="85%" >                                                                                               
                                                                                           </telerik:RadComboBox>
                                                                                          <br /> <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="The approval flow is mandatory to continue. If you don't have approval flow assigned, please contact the system administrator." ForeColor="Red" ControlToValidate="cmb_approvals" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                  
                                                                               <asp:HiddenField ID="hd_id_documento_timesheet" runat="server" Value="0" />
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
                                                                                                                                                    HeaderText="Nombre del documento" SortExpression="nombre_documento" 
                                                                                                                                                    UniqueName="descripcion_cat">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                                <telerik:GridBoundColumn DataField="extension" 
                                                                                                                                                    FilterControlAltText="Filter extension column" HeaderText="Extensiones permitidas" 
                                                                                                                                                    SortExpression="extension" UniqueName="extension">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                               <telerik:GridBoundColumn DataField="max_size" 
                                                                                                                                                       FilterControlAltText="Filter max_size column" HeaderText="Tamaño maximo (MB)" 
                                                                                                                                                       SortExpression="max_size" UniqueName="colm_max_size" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                                                                                                </telerik:GridBoundColumn>
                                    
                                                                                                                                                 <telerik:GridBoundColumn DataField="Template" 
                                                                                                                                                    FilterControlAltText="Filter Template column" HeaderText="Plantilla" 
                                                                                                                                                    UniqueName="Template" Visible="true" Display="false">
                                                                                                                                                </telerik:GridBoundColumn>
                                    
                                                                                                                                             <telerik:GridTemplateColumn 
                                                                                                                                                  FilterControlAltText="Filter colm_template column"  HeaderText="Plantilla" 
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
                               <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Guardar y continuar" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-o-right"></i></asp:LinkButton>                                              
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

