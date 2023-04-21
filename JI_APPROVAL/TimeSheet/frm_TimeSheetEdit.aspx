<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetEdit"  Codebehind="frm_TimeSheetEdit.aspx.vb" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

        <uc:Confirm runat="server" ID="MsgGuardar" />
        <uc:ReturnConfirm runat="server" ID="MsgReturn" />
        <link rel="stylesheet" href="../Content/hr_Styles.css" />
        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEET</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box"> 
                     <div class="box-header with-border">
                         <div class="col-sm-6">  
                            <h3 class="box-title">
                                <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Billable Time</asp:Label>
                            </h3>
                            <asp:HiddenField ID="hd_IDtimeSheet" runat="server" Value="0" />
                             <asp:HiddenField ID="anio_ts" runat="server" Value="0" />
                             <asp:HiddenField ID="mes_ts" runat="server" Value="0" />
                             <asp:HiddenField ID="ts_leave" runat="server" Value="0" />
                             <asp:HiddenField ID="anio" runat="server" Value="0" />
                             <asp:HiddenField ID="mes" runat="server" Value="0" />
                             <asp:HiddenField ID="dia" runat="server" Value="0" />
                            <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />

                        </div>
                         <div class="col-sm-6 text-right">   
                            <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('TimeSheet_03.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                        </div>
                    </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">

                                   <div class="col-lg-12">
                                     <div class="box-body">
                                                              
                                            <div class="stepwizard">
                                                    <div class="stepwizard-row setup-panel">
                                                        <div class="stepwizard-step" style="width:33%">
                                                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_informaciongeneral">Time Sheet Information</asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="stepwizard-step" style="width:33%">
                                                            <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_personal_status">Billable Time</asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="stepwizard-step" style="width:33%">
                                                             <a   href="#step-3" id="anchorSupportDocs" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                                             <p>
                                                                 <asp:Label runat="server" ID="lblt_Support_Documents">TimeSheet Documents</asp:Label>
                                                             </p>
                                                         </div>
                                                         <div class="stepwizard-step" style="width:33%">
                                                             <a   href="#step-4" id="anchorFollowUp" runat="server" type="button" class="btn btn-default btn-circle">4</a>
                                                             <p>
                                                                 <asp:Label runat="server" ID="lblt_complementary">Follow Up</asp:Label>
                                                             </p>
                                                         </div>                                        
                                                    </div>
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
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <asp:Label ID="lbl_employeeType" runat="server" CssClass="control-label info-box-text" Text=""></asp:Label> 
                                           <asp:HiddenField ID="hd_id_employee_type" runat="server" Value="0" />
                                           <asp:HiddenField ID="hd_dtBill" runat="server" Value="" />
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
                                           <telerik:RadTextBox ID="txt_description" Runat="server"  EmptyMessage="Type Description here.." Width="90%"  ValidationGroup="1" TextMode="MultiLine" Columns="40" Rows="3">
                                           </telerik:RadTextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_description" ValidationGroup="1"></asp:RequiredFieldValidator>
                                          
                                       </div>                                        
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
                                    </div>
                                                                      
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lbl_idTemp" runat="server" Visible="False"></asp:Label>
                                           <asp:Label ID="lblt_Employee_associated" runat="server" CssClass="control-label text-bold"   Text="Billable Type"></asp:Label>
                                        </div>
                                       <div class="col-sm-3">
                                    
                                            <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_billable_Type" 
                                                                   runat ="server" 
                                                                    CausesValidation="False"                                                                     
                                                                    EmptyMessage="Select the billable type..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains"                                                                                                                      
                                                                    Width="99%"
                                                                    OnClientSelectedIndexChanged ="getBillableItem" > 
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="cmb_billable_Type" ValidationGroup="2"></asp:RequiredFieldValidator>
                                       
                    
                                           <script type="text/javascript">
                                            
                                               function getBillableItem(sender, eventArgs) {

                                                 var comboboxType = $find("<%= cmb_billable_Type.ClientID %>");
                                                 var value = comboboxType.get_selectedItem().get_value();
                                                 var texto = comboboxType.get_selectedItem().get_text();
                                                 var comboboxOptions = $find("<%= cmb_billable_Item.ClientID %>");

                                                 //alert('User: ' + value + ' ' + texto + ' Program: ' + <%=Me.Session("E_IDPrograma")%>);

                                                $.ajax({
                                                     type: "POST",
                                                     url: "frm_TimeSheetEdit.aspx/GetBillableOptions",
                                                     data: '{idBillType:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'", idTimeSheet:"'+ <%=me.hd_IDtimeSheet.Value %>  +'", ts_leave:"' + + <%=Me.hd_leave.Value%>  + '" }',
                                                     contentType: "application/json; charset=utf-8",
                                                     dataType: "json",
                                                     success: function (data) {                                                       
                                                         fillCombo(comboboxOptions, data);
                                                         //comboboxOptions.highlightAllMatches(comboboxOptions.get_text());                                                                                                               

                                                     },
                                                     failure: function (response) {
                                                         alert('Error: ' + response.d);
                                                     }   
                                                 });
                                                 
                                               }


                                               // Use this for all of your RadComboBox to populate from JQuery 
                                               function fillCombo(combo, result) {

                                                   combo.clearItems();
                                                 
                                                   var items = result.d || result;
                                                   //alert('items:' + items + ' lenght:' + items.length);
                                                   data = jQuery.parseJSON(items);
                                                   //alert('items:' + data + ' lenght:' + data.length);

                                                   // This just lets your users know that nothing was returned with their search 
                                                   if (data.length == 0) {
                                                       var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                                       comboItem.set_text("No records found");
                                                       comboItem.set_value("0");
                                                       combo.get_items().add(comboItem);
                                                       combo.clearSelection();
                                                   }

                                                   
                                                   for (var i = 0; i < data.length; i++) {

                                                       var item = data[i];
                                                       //alert('item'+ i + ': ' + item.Text + ' ' + item.Value);                                                       
                                                       var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                                       comboItem.set_text(item.Text);
                                                       comboItem.set_value(item.Value);
                                                       combo.get_items().add(comboItem);
                                                   }

                                                   combo.clearSelection();

                                               }
                                               
      
                                            </script>  

                                       </div>
                                          <div class="col-sm-2 text-left">
                                            <!--Tittle -->
                                            <asp:Label ID="lblt_billableItem" runat="server" CssClass="control-label text-bold"   Text="Billable Option"></asp:Label>
                                        </div>
                                            <div class="col-sm-3">                                    
                                            <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_billable_Item" 
                                                                   runat ="server" 
                                                                    CausesValidation="False"                                                                     
                                                                    EmptyMessage="Select the billable option..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains"                                                                                                                      
                                                                    Width="99%"> 
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="cmb_billable_Item" ValidationGroup="2"></asp:RequiredFieldValidator>
                                                                            
                                       </div>

                                       <div class="col-sm-2 text-left">
                                         <asp:LinkButton ID="bntlk_addItem" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Add Billable Item" Width="95%" class="btn btn-primary btn-sm margin-r-5"  ValidationGroup="2" >Add&nbsp;&nbsp;&nbsp;<i class="fa fa-plus-circle"></i></asp:LinkButton>                                                                                                      
                                       </div>

                                    </div>


                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                             <hr class="hr-primary" />                                             
                                        </div>
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
                                                                                                
                                           </style>

                                               <script type="text/javascript">

                                                   var footerBoxControl = []; 
                                                   var currTotal;
                                                  
                                                   function Calculate(button, args, params) {

                                                       //alert(params);
                                                       //alert(button.get_element());

                                                            var row = Telerik.Web.UI.Grid.GetFirstParentByTagName(button.get_element(), "tr");
                                                            currTotal = 0;
                                                            Total = 0;

                                                            //guarantee to get to the table row
                                                            while (!(row.id && row.id.indexOf("__") > -1)) {
                                                                row = Telerik.Web.UI.Grid.GetFirstParentByTagName(button, "tr");
                                                            }

                                                            //get index
                                                            var index = row.id.split("__")[1];                                                             

                                                            //get all table data items
                                                            var tableId = row.id.split("__")[0];
                                                            var tableView = $find(tableId);
                                                            

                                                            var dataItems = tableView.get_dataItems();
                                                            //find the data item related to the row id
                                                            var dataItem = $find(row.id);                
                                                            
                                                            //get element cells
                                                            totalRow = $telerik.getElementByClassName(dataItem.get_cell("Total"), "riTextBox");

                                                           
                                                           //******************FOR THE ROW**********************
                                                            var ColSel = 'colm_d' + params;
                                                            var RowVal =parseFloat(0);
                                                            var ColVal = 0;
                                                            var ColTable = 'colm_d';
                                                            var ColumnTable = '';

                                                            for (var i = 1; i < 32; i++) {
                                                                 
                                                                ColumnTable = ColTable + i;
                                                                ColVal = $telerik.getElementByClassName(dataItem.get_cell(ColumnTable), "riTextBox");
                                                                //alert(ColumnTable + ': ' + ColVal.value);

                                                                if (!isNaN(ColVal.value) && (parseFloat(ColVal.value) > 0)) {
                                                                    RowVal = RowVal + parseFloat(ColVal.value);
                                                                    //alert(ColumnTable + ': ' + ColVal.value+ ' RowVal: ' + RowVal);
                                                                 }
                                                                
                                                            }                                                                                                                

                                                            //alert('RowVal: ' + ColVal);
                                                            totalRow.value = RowVal;                                                                                                                                                                               
                                                         
                                                           //******************FOR THE ROW**********************
                                                       
                                                       //******************FOR THE COLUMN**********************
                                                                                                                
                                                            for (var i = 1; i < dataItems.length; i++) {

                                                                var tempCell = $telerik.getElementByClassName(dataItems[i].get_cell(ColSel), "riTextBox")

                                                                //alert('i: ' + tempCell + ' value:' + parseFloat(tempCell.value));
                                                                if (!isNaN(tempCell.value) && (parseFloat(tempCell.value) > 0)) {
                                                                    currTotal = currTotal + parseFloat(tempCell.value);
                                                                    //alert('currTotal: ' + currTotal);                                                                
                                                                }

                                                            }
                                                                                                                     

                                                            footerBoxControl[params].set_value(currTotal);
                                                                     
                                                                var masterTable = $find('<%= grd_cate.ClientID %>').get_masterTableView();
                                                                var row = masterTable.get_dataItems();
                                                                var ControlText;
                                                                
                                                                var ColorNormal = '#ffffff';
                                                                var ColorSunday = '#D3D3D3';
                                                                var ColorError =  '#f59e93';
                                                                var ColorPiv = '';


                                                            if (currTotal != 8) {
                                                                                                                                                                                              
                                                                for (var i = 1; i < row.length; i++) {

                                                                    ControlText = $telerik.getElementByClassName(dataItems[i].get_cell(ColSel), "riTextBox")
                                                                    row[i].get_cell(ColSel).style.backgroundColor = ColorError;
                                                                    ControlText.style.backgroundColor = ColorError;                                                                    
                                                                    
                                                                    //document.getElementById("totalP").textContent = currTotal;                                                                                
                                                                }

                                                                row[0].get_cell(ColSel).style.backgroundColor = ColorError;
                                                                document.getElementById("msgGRID").textContent = 'Reporting hours: The date ' + row[0].get_cell(ColSel).innerHTML + ' ' + params + 'has more or less than 8 hours, please be sure to report it, in the proper billable option.';
                                                                
                                                              
                                                            } else {
                                                                <%--$("#<%= Me.btnlk_save.ClientID %>").css("display", "block");--%>

                                                                                    
                                                                if ( row[0].get_cell(ColSel).innerHTML.trim() == 'sáb.' || row[0].get_cell(ColSel).innerHTML.trim() == 'Sat')  {                                                                
                                                                   //console.log('Reserved Days ' + row[0].get_cell(ColSel).innerHTML + ' Color:' + ColorSunday);
                                                                    ColorPiv = ColorSunday;
                                                                } else {
                                                                    //console.log('Week Days ' + row[0].get_cell(ColSel).innerHTML + ' Color:' + ColorNormal);
                                                                    ColorPiv = ColorNormal;                                                                                                                                     
                                                                }
                                                                
                                                                                                                                                                                                   


                                                                for (var i = 1; i < row.length; i++) {

                                                                    ControlText = $telerik.getElementByClassName(dataItems[i].get_cell(ColSel), "riTextBox")
                                                                    row[i].get_cell(ColSel).style.backgroundColor = ColorPiv;
                                                                    ControlText.style.backgroundColor = ColorPiv;
                                                                                                                                      
                                                                }
                                                                                                                               
                                                               

                                                                row[0].get_cell(ColSel).style.backgroundColor = ColorPiv;
                                                                document.getElementById("msgGRID").textContent =""

                                                            }
                                                     
                                                       //******************FOR THE COLUMN**********************
                                                       
                                                       //******************FOR THE COLUMN TOTAL**********************

                                                            for (var i = 1; i < dataItems.length; i++) {

                                                                var tempCell = $telerik.getElementByClassName(dataItems[i].get_cell("Total"), "riTextBox")
                                                                                                                              
                                                                if (!isNaN(tempCell.value) && (parseFloat(tempCell.value) > 0)) {
                                                                    Total = Total + parseFloat(tempCell.value);
                                                                }
                                                                
                                                            }
                                                       
                                                            footerBoxControl[32].set_value(Total);
                                                                                                              
                                                       //******************FOR THE COLUMN TOTAL**********************
                                                                                                               
                                                       //document.getElementById("totalP").textContent = currTotal;                                                                                
                                                          
                                                       var valorMayo8 = false;
                                                       for (var j = 1; j < 32; j++) {
                                                           if ($(".tot_" + j).val() != 8 && $(".tot_" + j).val() > 0) {
                                                               valorMayo8 = true;
                                                           }
                                                       }
                                                       if (valorMayo8) {
                                                           $("#<%= Me.btnlk_save.ClientID %>").css("display", "none");
                                                       }
                                                       else {
                                                           $("#<%= Me.btnlk_save.ClientID %>").css("display", "block");
                                                       }

                                                   }

                                                        function FooterLoaded(sender, args, params) {
                                                                                                                   
                                                            footerBoxControl[parseInt(params)] = sender         
                                                                                                                     
                                                        }

                                                                                                       
                                                        function Focus(sender, eventArgs) {
                                                            sender.set_value(colm_d29.value + colm_d30.value);
                                                        }


                                                        Function.prototype.curry = function () {
                                                            var method = this, args = Array.prototype.slice.call(arguments);
                                                            return function () {
                                                                return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                                                            };
                                                        };

                                                   
                                                   function OnClientSelectedIndexChanged(sender, args) {
                                                        var item = args.get_item();
                                                        var masterTable = $find('<%= grd_cate.ClientID %>').get_masterTableView();
                                                            var row = masterTable.get_dataItems();
                                                        if (item.get_text() == "0")
                                                        {
                                                            row[0].addCssClass("ClassA");
                                                         }
                                                             else
                                                         {
                                                          row[0].addCssClass("ClassB");
                                                         }
                                                   }


                                                   function CalculateATall() {

                                                       //alert(params);       
                                                        var ColSel = 'colm_d';
                                                        var masterTable = $find('<%= grd_cate.ClientID %>').get_masterTableView();
                                                        var rows = masterTable.get_dataItems();
                                                        var ControlText;
                                                        var rTOTAL = 0;
                                                        var cTOTAL = 0;
                                                  

                                                       //******************sumary of the total Row********************
                                                       ////******************FOR THE ROW**********************
                                                        for (var i = 1; i < rows.length; i++) { //Row Total

                                                            rTOTAL = 0;
                                                            for (var j = 1; j < 32; j++) { 

                                                                ColSel = 'colm_d' + j;
                                                                ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox")

                                                                if (!isNaN(ControlText.value) && (parseFloat(ControlText.value) > 0)) {
                                                                    rTOTAL = rTOTAL + parseFloat(ControlText.value);
                                                                }

                                                                if (rTOTAL > 0){
                                                                    totalRow = $telerik.getElementByClassName(rows[i].get_cell("Total"), "riTextBox");
                                                                    totalRow.value = rTOTAL;
                                                                }

                                                            }

                                                             //row[i].get_cell(ColSel).style.backgroundColor = ColorError;
                                                            //ControlText.style.backgroundColor = ColorError;                                                            
                                                            //document.getElementById("totalP").textContent = currTotal;                                                                                

                                                        }
                                                       //******************sumary of the total Row********************
                                                       ////******************FOR THE ROW**********************
                                                       
                                                   
                                                       //******************sumary of the total Columns********************
                                                       ////******************FOR THE COLUMNS**********************
                                                        
                                                        for (var j = 1; j < 33; j++) {

                                                            cTOTAL = 0;
                                                            if (j < 32) {
                                                                ColSel = 'colm_d' + j;
                                                            } else {
                                                                ColSel = 'Total';
                                                            }
                                                            
                                                            for (var i = 1; i < rows.length; i++) { //Row Total

                                                                ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox")

                                                                if (!isNaN(ControlText.value) && (parseFloat(ControlText.value) > 0)) {
                                                                    cTOTAL = cTOTAL + parseFloat(ControlText.value);
                                                                }

                                                                if (cTOTAL > 0) { 
                                                                  footerBoxControl[j].set_value(cTOTAL);
                                                                }

                                                            }

                                                        }
                                                       //******************sumary of the total Columns********************
                                                       ////******************FOR THE COLUMNS**********************

                                                       
                                                   }
                                                   $(document).ready(function () {
                                                       setTimeout(function () {
                                                           var valorMayo8 = false;
                                                            for (var j = 1; j < 32; j++) {
                                                                if ($(".tot_" + j).val() != 8 && $(".tot_" + j).val() > 0) {
                                                                    valorMayo8 = true;
                                                                }
                                                            }
                                                            if (valorMayo8) {
                                                                $("#<%= Me.btnlk_save.ClientID %>").css("display", "none");
                                                           }
                                                           else {
                                                               $("#<%= Me.btnlk_save.ClientID %>").css("display", "block");
                                                           }
                                                       }, 300)
                                                       
                                                   });

                                                   window.addEventListener('keydown', function (e) { if (e.keyIdentifier == 'U+000A' || e.keyIdentifier == 'Enter' || e.keyCode == 13) { if (e.target.nodeName == 'INPUT' && e.target.type == 'text') { e.preventDefault(); return false; } } }, true);
                                          </script>                                                                                          
                                          
                                             <telerik:RadGrid ID="grd_cate" 
                                                     Skin="Office2010Blue"   
                                                    runat="server" 
                                                    CellSpacing="0"  CellPadding="0"                                                   
                                                    GridLines="none" CssClass="MyGridClass" >

                                                     <ClientSettings>                               
                                                          <Selecting AllowRowSelect="True" />  
                                                          <Scrolling AllowScroll="True"  ScrollHeight="500"  UseStaticHeaders="true" ></Scrolling>   
                                                          <Resizing AllowColumnResize="True" 
                                                              AllowRowResize="false" 
                                                              ResizeGridOnColumnResize="false"
                                                              ClipCellContentOnResize="true" 
                                                              EnableRealTimeResize="false" 
                                                              AllowResizeToFit="true"  />                                                                                                       
                                                    </ClientSettings>
                                                    
                                                    <MasterTableView AutoGenerateColumns="False" TableLayout="Fixed" ShowFooter ="true">

                                                         <GroupByExpressions>
                                                           <telerik:GridGroupByExpression>       
                                                                 <SelectFields>
                                                                    <telerik:GridGroupByField FieldAlias="billable_time_type" FieldName="billable_time_type" HeaderText="" HeaderValueSeparator=":"></telerik:GridGroupByField>
                                                                </SelectFields>                                                                                                               
                                                                <GroupByFields>
                                                                    <telerik:GridGroupByField FieldName="billable_time_type"></telerik:GridGroupByField>
                                                                </GroupByFields>
                                                            </telerik:GridGroupByExpression>
                                                        </GroupByExpressions>

                                                       <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings> 
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="50px" />                                                                                                                             
                                                        <Columns>

                                                               <telerik:GridBoundColumn DataField="DATE"
                                                                FilterControlAltText="Filter DATE column"
                                                                HeaderText="DATE" SortExpression="DATE"
                                                                UniqueName="colm_DATE" >
                                                                <HeaderStyle Width="200px" />
                                                                </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn  DataField="id_billable_time" UniqueName="id_billable_time" Visible="true" Display="false">
                                                             </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn  DataField="id_billable_time" UniqueName="id_billable_time" Visible="true" Display="false">
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  DataField="id_billable_time_type" UniqueName="billable_time_type" Visible="true" Display="false">
                                                             </telerik:GridBoundColumn>                                                          
                                                             <telerik:GridBoundColumn  DataField="visible" UniqueName="visible" Visible="true" Display="false">
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  DataField="ts_leave" UniqueName="ts_leave" Visible="true" Display="false">
                                                             </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="1" UniqueName="1" Visible="true" Display="false"></telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="2"  UniqueName="2" Visible="true" Display="false"></telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="3"  UniqueName="3" Visible="true" Display="false"></telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="4"  UniqueName="4" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="5"  UniqueName="5" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="6"  UniqueName="6" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="7"  UniqueName="7" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="8"  UniqueName="8" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="9"  UniqueName="9" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="10"  UniqueName="10" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="11"  UniqueName="11" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="12"  UniqueName="12" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="13"  UniqueName="13" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="14"  UniqueName="14" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="15"  UniqueName="15" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="16"  UniqueName="16" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="17"  UniqueName="17" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="18"  UniqueName="18" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="19"  UniqueName="19" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="20"  UniqueName="20" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="21"  UniqueName="21" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="22"  UniqueName="22" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="23"  UniqueName="23" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="24"  UniqueName="24" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="25"  UniqueName="25" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="26"  UniqueName="26" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="27"  UniqueName="27" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="28"  UniqueName="28" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="29"  UniqueName="29" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="30"  UniqueName="30" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  DataField="31"  UniqueName="31" Visible="true" Display="false"> </telerik:GridBoundColumn>
                                                              
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d1 column"
                                                                HeaderText="1" UniqueName="colm_d1" >
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d1" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('1')" /> 
                                                                    </telerik:RadNumericTextBox>                                                               
                                                                </ItemTemplate>  
                                                                 <HeaderStyle Width="49px" />                                                                                                                                                                                       
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_1" disabled="true" NumberFormat-GroupSeparator="" CssClass="tot_1" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('1')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>                                                           
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d2 column"
                                                                HeaderText="2" UniqueName="colm_d2">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d2" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('2')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>  
                                                                <HeaderStyle Width="49px" />                                                         
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_2" NumberFormat-GroupSeparator="" CssClass="tot_2" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('2')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>                                                            
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d3 column"
                                                                HeaderText="3" UniqueName="colm_d3">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d3" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('3')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>     
                                                                <HeaderStyle Width="49px" />                                                            
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_3" NumberFormat-GroupSeparator="" CssClass="tot_3" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('3')" />
                                                                    </telerik:RadNumericTextBox>
                                                               </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d4 column"
                                                                HeaderText="4" UniqueName="colm_d4">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d4" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('4')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate> 
                                                                <HeaderStyle Width="49px" />                                                                
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_4" NumberFormat-GroupSeparator="" CssClass="tot_4" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('4')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d5 column"
                                                                HeaderText="5" UniqueName="colm_d5" >
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d5" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('5')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>      
                                                                 <HeaderStyle Width="49px" />                                                                
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_5" NumberFormat-GroupSeparator="" CssClass="tot_5" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('5')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d6 column"
                                                                HeaderText="6" UniqueName="colm_d6">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d6" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('6')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>   
                                                                <HeaderStyle Width="49px" />                                                                 
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_6" NumberFormat-GroupSeparator="" CssClass="tot_6" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('6')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d7 column"
                                                                HeaderText="7" UniqueName="colm_d7">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d7" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('7')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate> 
                                                                <HeaderStyle Width="49px" />                                                                    
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_7" NumberFormat-GroupSeparator="" CssClass="tot_7" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('7')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d8 column"
                                                                HeaderText="8" UniqueName="colm_d8">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d8" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('8')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>                                                                    
                                                                <HeaderStyle Width="49px" />                                                                 
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_8" NumberFormat-GroupSeparator="" CssClass="tot_8" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('8')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>  
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d9 column"
                                                                HeaderText="9" UniqueName="colm_d9">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d9" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('9')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>    
                                                                <HeaderStyle Width="49px" />                                                                 
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_9" NumberFormat-GroupSeparator="" CssClass="tot_9" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('9')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d10 column"
                                                                HeaderText="10" UniqueName="colm_d10">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d10" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('10')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_10" NumberFormat-GroupSeparator="" CssClass="tot_10" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('10')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d11 column"
                                                                HeaderText="11" UniqueName="colm_d11">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d11" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('11')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_11" NumberFormat-GroupSeparator="" CssClass="tot_11" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('11')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>  
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d12 column"
                                                                HeaderText="12" UniqueName="colm_d12">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d12" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('12')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_12" NumberFormat-GroupSeparator="" CssClass="tot_12" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('12')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                         
                                                            </telerik:GridTemplateColumn> 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d13 column"
                                                                HeaderText="13" UniqueName="colm_d13">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d13" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('13')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_13" NumberFormat-GroupSeparator="" CssClass="tot_13" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('13')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                         
                                                            </telerik:GridTemplateColumn>  
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d14 column" HeaderText="14" UniqueName="colm_d14">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d14" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('14')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_14" NumberFormat-GroupSeparator="" CssClass="tot_14" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('14')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                         
                                                            </telerik:GridTemplateColumn>   		
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d15 column"
                                                                HeaderText="15" UniqueName="colm_d15">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d15" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('15')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />  
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_15" NumberFormat-GroupSeparator="" CssClass="tot_15" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('15')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                       
                                                            </telerik:GridTemplateColumn>   	
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d16 column"
                                                                HeaderText="16" UniqueName="colm_d16">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d16" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('16')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_16" NumberFormat-GroupSeparator="" CssClass="tot_16" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('16')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   	
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d17 column"
                                                                HeaderText="17" UniqueName="colm_d17">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d17" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('17')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_17" NumberFormat-GroupSeparator="" CssClass="tot_17" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('17')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d18 column"
                                                                HeaderText="18" UniqueName="colm_d18">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d18" MinValue="0" MaxValue="8" Width="35px">
                                                                        <ClientEvents OnValueChanged="Calculate.curry('18')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_18" NumberFormat-GroupSeparator="" CssClass="tot_18" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('18')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   			
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d19 column"
                                                                HeaderText="19" UniqueName="colm_d19">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d19" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('19')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_19" NumberFormat-GroupSeparator="" CssClass="tot_19" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('19')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   	
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d20 column"
                                                                HeaderText="20" UniqueName="colm_d20">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d20" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('20')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                 <HeaderStyle Width="49px" />
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_20" NumberFormat-GroupSeparator="" CssClass="tot_20" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('20')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                         
                                                            </telerik:GridTemplateColumn> 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d21 column"
                                                                HeaderText="21" UniqueName="colm_d21">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d21" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('21')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                 <HeaderStyle Width="49px" />                                                                     
                                                                 <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_21" NumberFormat-GroupSeparator="" CssClass="tot_21" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('21')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>  
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d22 column"
                                                                HeaderText="22" UniqueName="colm_d22">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d22" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('22')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" />    
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_22" NumberFormat-GroupSeparator="" CssClass="tot_22" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('22')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   		
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d23 column"
                                                                HeaderText="23" UniqueName="colm_d23">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d23" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('23')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" /> 
                                                                     <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_23" NumberFormat-GroupSeparator="" CssClass="tot_23" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('23')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d24 column"
                                                                HeaderText="24" UniqueName="colm_d24">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d24" MinValue="0" MaxValue="8" Width="35px">
                                                                        <ClientEvents OnValueChanged="Calculate.curry('24')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" />    
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_24" NumberFormat-GroupSeparator="" CssClass="tot_24" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('24')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                    
                                                            </telerik:GridTemplateColumn> 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d25 column"
                                                                HeaderText="25" UniqueName="colm_d25">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d25" MinValue="0" MaxValue="8" Width="35px">
                                                                        <ClientEvents OnValueChanged="Calculate.curry('25')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" />   
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_25" NumberFormat-GroupSeparator="" CssClass="tot_25" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('25')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                     
                                                            </telerik:GridTemplateColumn> 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d26 column"
                                                                HeaderText="26" UniqueName="colm_d26">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d26" MinValue="0" MaxValue="8" Width="35px">
                                                                        <ClientEvents OnValueChanged="Calculate.curry('26')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" />   
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_26" NumberFormat-GroupSeparator="" CssClass="tot_26" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('26')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                     
                                                            </telerik:GridTemplateColumn>   	
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d27 column"
                                                                HeaderText="27" UniqueName="colm_d27">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d27" MinValue="0" MaxValue="8" Width="35px">
                                                                          <ClientEvents OnValueChanged="Calculate.curry('27')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                    <HeaderStyle Width="49px" />   
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_27" NumberFormat-GroupSeparator="" CssClass="tot_27" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('27')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                  
                                                            </telerik:GridTemplateColumn>   	 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d28 column"
                                                                HeaderText="28" UniqueName="colm_d28">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d28" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('28')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_28" NumberFormat-GroupSeparator="" CssClass="tot_28" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('28')" />
                                                                </telerik:RadNumericTextBox>
                                                                </FooterTemplate>  
                                                            </telerik:GridTemplateColumn>   	 
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d29 column"
                                                                HeaderText="29" UniqueName="colm_d29">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d29" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('29')" /> 
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />  
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_29" NumberFormat-GroupSeparator="" CssClass="tot_29" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('29')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                                                                                                   
                                                            </telerik:GridTemplateColumn>   	 	
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d30 column"
                                                                HeaderText="30" UniqueName="colm_d30">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d30" MinValue="0" MaxValue="8" Width="35px">
                                                                         <ClientEvents OnValueChanged="Calculate.curry('30')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_30" NumberFormat-GroupSeparator="" CssClass="tot_30" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('30')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter colm_d31 column" HeaderText="31" UniqueName="colm_d31">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_d31" MinValue="0" MaxValue="8" Width="35px">
                                                                        <ClientEvents OnValueChanged="Calculate.curry('31')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="49px" />                                                                     
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_31" NumberFormat-GroupSeparator="" CssClass="tot_31" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('31')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>   
                                                             <telerik:GridTemplateColumn UniqueName="Total" HeaderText="Grand Total" Visible="true" Display="true">
                                                                <ItemTemplate>
                                                                    <telerik:RadNumericTextBox ID="txt_all" NumberFormat-GroupSeparator="" runat="server"  Width="35px" >
                                                                        <ClientEvents OnValueChanged="Calculate.curry('32')"  />
                                                                    </telerik:RadNumericTextBox>
                                                                </ItemTemplate>    
                                                                <HeaderStyle Width="49px" />   
                                                                  <FooterTemplate>
                                                                    <telerik:RadNumericTextBox ID="tot_all" NumberFormat-GroupSeparator="" CssClass="tot_32" disabled="true" runat="server"  Width="35px">
                                                                      <ClientEvents OnLoad="FooterLoaded.curry('32')" />
                                                                    </telerik:RadNumericTextBox>
                                                                </FooterTemplate>                                                         
                                                            </telerik:GridTemplateColumn>
                                                            	   	  			 					 		
                                                           </Columns>
                                                  
                                                        </MasterTableView>

                                               </telerik:RadGrid><br>
                                             
                                              <p id="msgGRID" style="color:red"></p>

                                              <style type="text/css">

                                            .MyGridClass .rgDataDiv
                                            {
                                                  height : auto !important ;
                                            }

                                     </style>             

                               </div>
                          </div>


                                    <div class="form-group row">
                                       <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lblt_Notes" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                       </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadTextBox ID="txt_notes" Runat="server" EmptyMessage="Type notes here.."  Width="70%" ValidationGroup="1" Height="75px" TextMode="MultiLine">
                                             </telerik:RadTextBox>                                            
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
                                                          <th>Días registrados</th>
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
                                                           <th style="border-top:1px solid lightgray;"><%=TOTloe %></th>             
                                                           <th></th>        
                                                        </tr>
                                                      </table>
                                                    </div><!-- /.box-body -->
                                                  </div><!-- /.box -->
                                        </div>
                                    </div>

                                  </div>
                               </div> 
              
                          </div>
                        
                        </div>

                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->

                        <div class="form-group row">
                             <div class="col-sm-2 text-left">
                               <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm pull-left" Width="100"></telerik:RadButton><br /><br />                      
                             </div>
                            <div class="col-sm-2 text-left">
                               <asp:LinkButton ID="btnlk_save" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Guardar y continuar" Width="99%" class="btn btn-primary btn-sm margin-r-5"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-o-right"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-8 text-left">
                                <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                            </div>
                       </div>                   
                   </div>

                </div>                 

           </section>



    
    

    </asp:Content>

