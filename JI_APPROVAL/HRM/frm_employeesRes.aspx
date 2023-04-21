<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_employeesRes.aspx.vb" Inherits="RMS_APPROVAL.frm_employeeRes" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración de Empleados</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Resumen de Empleados</asp:Label>
                </h3>
            </div>
            <div class="box-body">


                      <asp:HiddenField ID="TabName" runat="server" Value="Employees" />
                           
                                   <div class="p-0 pt-1" id="Tabs">                                 
                                     <ul class="nav nav-tabs">                                          
                                          <li class="active"><a data-toggle="tab" href="#AnnualLeave"><asp:Label runat="server" ID="lblt_Employees" CssClass="control-label text-bold">Annual Leave</asp:Label></a></li> 
                                          <li><a data-toggle="tab" href="#Casual"><asp:Label runat="server" ID="lblt_AnnualLEave" CssClass="control-label text-bold">Casual Leave</asp:Label></a></li>
                                          <li><a data-toggle="tab" href="#Sick"><asp:Label runat="server" ID="lblt_tiempo" CssClass="control-label text-bold">Sick Leave</asp:Label></a></li>                                          
                                      </ul>
                                  </div>

                              

             <div class="tab-content">
          
                         
                  <div id="AnnualLeave" class="tab-pane fade in active">

                          <div class="form-group row" style="width:100%; margin: 0 auto">

			                    <%--***********REGISTER EMPLOYEES***********--%>

                                   <div class="col-lg-12">
                                     
                                       <br /><br />
                                           
                                           <asp:LinkButton ID="btnlk_Export" runat="server" AutoPostBack="True" SingleClick="true"  Text="Exportar" Width="12%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export"  ><i class="fa fa-file-excel-o fa-2x"></i>&nbsp;&nbsp;Export Detail</asp:LinkButton>                                              
                                           <button class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export" title=""  Width="20%"  runat="server" onserverclick="export_button_annual_ServerClick" id="export_button_annual"><i class="fa fa-download"></i>Export Table</button>
                                           <asp:HiddenField ID="hd_tp" runat="server" Value="0" />
                                           <asp:HiddenField ID="hd_dtANNUAL" runat="server" Value="" />
                                           <asp:HiddenField ID="hd_dtCASUAL" runat="server" Value="" />
                                           <asp:HiddenField ID="hd_dtSICK" runat="server" Value="" />


                                           <%-- <telerik:RadGrid 
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
                                                    <MasterTableView AutoGenerateColumns="true" ShowFooter="True" ShowGroupFooter="true">  
                                     
                                                    </MasterTableView>
                                             </telerik:RadGrid>--%>                                         

                                      <%--    <style type="text/css">

                                               .MyGridClass .rgDataDiv
                                                    {
                                                    height : auto !important ;
                                                    }

                                           </style>--%>

                                         <style type="text/css">               

                                                .RadPivotGrid,
                                                .RadPivotGrid .rpgContentZoneDiv,                        
                                                .RadPivotGrid .rpgRowHeaderZoneDiv {
                                                    overflow:auto !important;            
                                                }

                                                 /*.rpgRowHeaderZoneDiv,*/
                                                 /*.RadPivotGrid .rpgColumnHeaderDiv, , .rpgRowTotalDataCell */

                                                  .rpgColumnHeader{
                                                      height:35px !important;    
                                                  }

                                                  /*.rpgRowHeaderGrandTotal{
                                                      height:50px !important;    
                                                  }*/
                                                                                                    
                                                  /*.rpgRowHeaderTotal{
                                                      height:35px !important;    
                                                  }*/

                                                  /*.rpgRowGrandTotalDataCell, .rpgColumnGrandTotalDataCell {*/
                                                 
                                                  
                                                  /*td.rpgDataCell.rpgRowGrandTotalDataCell.rpgColumnGrandTotalDataCell {
                                                      height:35px !important;  
                                                  }

                                                     tr#ctl00_MainContent_radgrid_emp_reported_OT__lhRow {
                                                           height:45px !important;
                                                     }*/


                                                   /*.rpgRowHeaderGrandTotal{
                                                      height:45px !important;
                                                    }*/
                                                 
                                                  /*.rpgRowHeader, .rpgRowHeaderField{
                                                       height:28px !important; 
                                                  }*/

                                                  #ctl00_MainContent_radgrid_emp_reported_ctl02_RowZone1 , 
                                                  #ctl00_MainContent_radgrid_casual_reported_ctl02_RowZone1, 
                                                  #ctl00_MainContent_radgrid_casual_reported_ctl02_RowZone2                                                  
                                                  {
                                                     height:90px !important;            
                                                  }
                                                  
                                                  /*#ctl00_MainContent_radgrid_casual_reported_OT__lhRow{*/                                                                                                  
                                                  
                                                  #ctl00_MainContent_radgrid_sick_reported_ctl02_RowZone1, 
                                                  #ctl00_MainContent_radgrid_sick_reported_ctl02_RowZone2 {
                                                       height:75px !important;     
                                                  }
          

                                            </style>
                                      

                                             <telerik:RadPivotGrid 
                                                      RenderMode="Lightweight" 
                                                      runat="server" 
                                                      ID="radgrid_emp_reported" 
                                                      Width  = "100%"                                                     
                                                      ColumnHeaderZoneText="ColumnHeaderZone" >
                                                                 
                                                 <%--RowsSubTotalsPosition="First"--%>

                                                 <TotalsSettings RowsSubTotalsPosition="None"  RowGrandTotalsPosition="Last" 
                                                                 ColumnsSubTotalsPosition="None" ColumnGrandTotalsPosition="First" />

                                                     <Fields>                                                                                                                    

                                                            <telerik:PivotGridRowField DataField="anio" Caption="Year" CellStyle-Width="60px" CellStyle-Height="35px"  ZoneIndex="0">
                                                            </telerik:PivotGridRowField>

                                                            <telerik:PivotGridRowField DataField="nombre_usuario" Caption="Employee Name" CellStyle-Width="200px" CellStyle-Height="35px"   ZoneIndex="0" >
                                                            </telerik:PivotGridRowField>      
                                                         
                                                            <telerik:PivotGridRowField DataField="contract_date" Caption="Contract Date" DataFormatString="{0:MM/dd/yyyy}" CellStyle-Width="100px" CellStyle-Height="35px"   ZoneIndex="0" >
                                                            </telerik:PivotGridRowField>      

                                                            <telerik:PivotGridRowField DataField="billable_time" Caption="billable_time" CellStyle-Width="115px" CellStyle-Height="35px"   ZoneIndex="0" >
                                                            </telerik:PivotGridRowField>                        
                                                         
                                                            <telerik:PivotGridColumnField  DataField="mes" Caption="month"  CellStyle-Height="35px"   >
                                                            </telerik:PivotGridColumnField>                                                                                                                

                                                             <telerik:PivotGridAggregateField Caption="Employment Days" DataField ="Employment_days"  Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="35px" CellStyle-Width="90px"   >
                                                                   <HeaderCellTemplate >Employment Days</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>

                                                             <telerik:PivotGridAggregateField Caption="Annual Leave Accrued" DataField="leave_accrued" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="35px" CellStyle-Width="90px"  >
                                                                   <HeaderCellTemplate >Annual Leave Accrued</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>
                                                            
                                                            <telerik:PivotGridAggregateField DataField="Leave_taken" Caption="Leave Taken" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="35px" CellStyle-Width="90px"  >
                                                               <HeaderCellTemplate >Leave Taken</HeaderCellTemplate>
                                                            </telerik:PivotGridAggregateField>

                                                            <telerik:PivotGridAggregateField DataField="Annual Leave Balance" CalculationDataFields="leave_accrued, Leave_taken" CalculationExpression="{0}-{1}" DataFormatString="{0:N2}" CellStyle-Font-Italic="true">
                                                                <HeaderCellTemplate >Annual Leave Balance</HeaderCellTemplate>
                                                            </telerik:PivotGridAggregateField>

                                                            
                                                     </Fields>                                          
                                                      
                                                      <ColumnHeaderCellStyle Width="70px"></ColumnHeaderCellStyle>
                                                      <DataCellStyle Width="70px" />
                                                 
                                                      <SortExpressions>
                                                         <telerik:PivotGridSortExpression FieldName="mes" SortOrder="Ascending"></telerik:PivotGridSortExpression>
                                                      </SortExpressions>
                
                                               </telerik:RadPivotGrid>
                              
                                                                                     


                                    </div> <%--<div class="col-lg-12">--%>

                                     <%--***********REGISTER EMPLOYEES***********--%>

                          </div>    <%--<div class="form-group row">   --%> 
       
                     </div> <%--id=Annual"--%>
              
                 <div id="Casual" class="tab-pane fade">

                     <div class="form-group row">
                            <div class="col-sm-12 text-left">



                                <br /><br />                               
                               <asp:LinkButton ID="btnlk_Export_casual" runat="server" AutoPostBack="True" SingleClick="true"  Text="Exportar" Width="12%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export"  ><i class="fa fa-file-excel-o fa-2x"></i>&nbsp;&nbsp;Export Detail</asp:LinkButton>                                   
                               <button class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export" title=""  Width="20%"  runat="server" onserverclick="export_button_casual_ServerClick" id="export_button_casual"><i class="fa fa-download"></i>Export Table</button>
      
                              
                                <%--RowGrandTotalsPosition="None" --%>

                                    <telerik:RadPivotGrid 
                                                      RenderMode="Lightweight" 
                                                      runat="server" 
                                                      ID="radgrid_casual_reported" 
                                                      Width  = "100%"                                                     
                                                      ColumnHeaderZoneText="ColumnHeaderZone"  >
                                                                      
                                                 <TotalsSettings RowsSubTotalsPosition="None" 
                                                        ColumnsSubTotalsPosition="None" ColumnGrandTotalsPosition="First" />

                                                     <Fields>                                                                                                                    

                                                            <telerik:PivotGridRowField DataField="anio" Caption="Year" CellStyle-Width="60px" CellStyle-Height="29px"  ZoneIndex="0">
                                                            </telerik:PivotGridRowField>

                                                            <telerik:PivotGridRowField DataField="nombre_usuario" Caption="Employee Name" CellStyle-Width="200px" CellStyle-Height="29px"  ZoneIndex="1">
                                                            </telerik:PivotGridRowField>      
                                                         
                                                            <telerik:PivotGridRowField DataField="contract_date" Caption="Contract Date" DataFormatString="{0:MM/dd/yyyy}" CellStyle-Width="100px" CellStyle-Height="29px"  ZoneIndex="1">
                                                            </telerik:PivotGridRowField>      

                                                            <telerik:PivotGridRowField DataField="billable_time" Caption="billable_time" CellStyle-Height="29px" CellStyle-Width="165px"  ZoneIndex="2">
                                                            </telerik:PivotGridRowField>                        
                                                         
                                                            <telerik:PivotGridColumnField  DataField="mes" Caption="month"  CellStyle-Height="29px"   >
                                                            </telerik:PivotGridColumnField>                                                                                                                

                                                             <telerik:PivotGridAggregateField Caption="Employment Days" DataField ="Employment_days"  Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"   >
                                                                   <HeaderCellTemplate >Employment Days</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>

                                                             <telerik:PivotGridAggregateField Caption="Casual Leave Accrued" DataField="leave_accrued" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"  >
                                                                   <HeaderCellTemplate >Casual Leave Accrued</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>
                                                            
                                                            <telerik:PivotGridAggregateField DataField="Leave_taken" Caption="Leave Taken" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"  >
                                                               <HeaderCellTemplate >Leave Taken</HeaderCellTemplate>
                                                            </telerik:PivotGridAggregateField>

                                                            <telerik:PivotGridAggregateField DataField="Casual Leave Balance" CalculationDataFields="leave_accrued, Leave_taken" CalculationExpression="{0}-{1}" DataFormatString="{0:N2}" CellStyle-Font-Italic="true">
                                                            </telerik:PivotGridAggregateField>
                                                            
                                                     </Fields>                                          
                                                      
                                                      <ColumnHeaderCellStyle Width="70px"></ColumnHeaderCellStyle>
                                                      <DataCellStyle Width="70px" />
                                                 
                                                      <SortExpressions>
                                                         <telerik:PivotGridSortExpression FieldName="mes" SortOrder="Ascending"></telerik:PivotGridSortExpression>
                                                      </SortExpressions>
                
                                                  </telerik:RadPivotGrid>

                            </div>
                     </div>
                     
                 </div>


                 <div id="Sick" class="tab-pane fade">

                   <div class="form-group row">
                            <div class="col-sm-12 text-left">

                                <br /><br />
                                <asp:LinkButton ID="btnlk_Export_sick" runat="server" AutoPostBack="True" SingleClick="true"  Text="Exportar" Width="12%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export"  ><i class="fa fa-file-excel-o fa-2x"></i>&nbsp;&nbsp;Export Detail</asp:LinkButton>
                                <button class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export" title=""  Width="20%"  runat="server" onserverclick="export_button_sick_ServerClick" id="export_button_sick"><i class="fa fa-download"></i>Export Table</button>


                                    <telerik:RadPivotGrid 
                                                      RenderMode="Lightweight" 
                                                      runat="server" 
                                                      ID="radgrid_sick_reported" 
                                                      Width  = "100%"                                                     
                                                      ColumnHeaderZoneText="ColumnHeaderZone"  >
                                                                      
                                                 <TotalsSettings RowsSubTotalsPosition="None" RowGrandTotalsPosition="None" 
                                                        ColumnsSubTotalsPosition="None" ColumnGrandTotalsPosition="First" />

                                                     <Fields>                                                                                                                    

                                                            <telerik:PivotGridRowField DataField="anio" Caption="Year" CellStyle-Width="60px" CellStyle-Height="29px"  ZoneIndex="0">
                                                            </telerik:PivotGridRowField>

                                                            <telerik:PivotGridRowField DataField="nombre_usuario" Caption="Employee Name" CellStyle-Width="200px" CellStyle-Height="29px"  ZoneIndex="1">
                                                            </telerik:PivotGridRowField>      
                                                         
                                                            <telerik:PivotGridRowField DataField="contract_date" Caption="Contract Date" DataFormatString="{0:MM/dd/yyyy}" CellStyle-Width="100px" CellStyle-Height="29px"  ZoneIndex="1">
                                                            </telerik:PivotGridRowField>      

                                                            <telerik:PivotGridRowField DataField="billable_time" Caption="billable_time" CellStyle-Width="115px" CellStyle-Height="29px"   ZoneIndex="2">
                                                            </telerik:PivotGridRowField>                        
                                                         
                                                            <telerik:PivotGridColumnField  DataField="mes" Caption="month"  CellStyle-Height="29px"   >
                                                            </telerik:PivotGridColumnField>                                                                                                                

                                                             <telerik:PivotGridAggregateField Caption="Employment Days" DataField ="employment_days"  Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"   >
                                                                   <HeaderCellTemplate >Employment Days</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>

                                                             <telerik:PivotGridAggregateField Caption="Sick Leave Accrued" DataField="leave_accrued" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"  >
                                                                   <HeaderCellTemplate >Casual Leave Accrued</HeaderCellTemplate>
                                                             </telerik:PivotGridAggregateField>
                                                            
                                                            <telerik:PivotGridAggregateField DataField="Leave_taken" Caption="Leave Taken" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px" CellStyle-Width="90px"  >
                                                               <HeaderCellTemplate >Leave Taken</HeaderCellTemplate>
                                                            </telerik:PivotGridAggregateField>

                                                            <telerik:PivotGridAggregateField DataField="Sick Leave Balance" CalculationDataFields="leave_accrued, Leave_taken" CalculationExpression="{0}-{1}" DataFormatString="{0:N2}" CellStyle-Font-Italic="true">
                                                            </telerik:PivotGridAggregateField>
                                                            
                                                     </Fields>                                          
                                                      
                                                      <ColumnHeaderCellStyle Width="70px"></ColumnHeaderCellStyle>
                                                      <DataCellStyle Width="70px" />
                                                 
                                                      <SortExpressions>
                                                         <telerik:PivotGridSortExpression FieldName="mes" SortOrder="Ascending"></telerik:PivotGridSortExpression>
                                                      </SortExpressions>
                
                                                  </telerik:RadPivotGrid>

                            </div>
                     </div>          
                     
                </div>


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
    </section>


     <script type="text/javascript">
                                   

               var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_endRequest(function (s, e) {
                          loadscript();
                    })

                $(document).ready(function () {
                      loadscript();
                })


             function loadscript() {

                 console.log('Starting LoadScript...');

                    //*******************************************TABS**************************************************************************
                    //*******************************************TABS**************************************************************************

                    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Employees";
                    $('#Tabs a[href="#' + tabName + '"]').tab('show');

                    $("#Tabs a").click(function () {

                        console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                    });

                    //*******************************************TABS**************************************************************************
                    //*******************************************TABS**************************************************************************

                }

         </script>
              

    <!-- /.content -->
</asp:Content>

