<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_employee_profile.aspx.vb" Inherits="RMS_APPROVAL.frm_employee_profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración de Empleados</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Employee Profile</asp:Label>
                </h3>
            </div>
            <div class="box-body">

                                            <div class="form-group row">    
                    
                                                <div class="col-md-4 " style="max-height:230px;">
                        
                                                           <div style="border-width:1px; display:block; z-index:1000; position:center; vertical-align:central; padding-left:15%;">

                                                                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                                                            <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                                                            <asp:HiddenField runat="server" ID="lbl_oldFile" />
                                                                            <script>
                                                                                function changeUpload(fileUploaded) {

                                                                                    document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;
                                                                                    var img = document.getElementById("<%= imgUser.ClientID%>");
                                                                                    //img.className = "hidden";
                                                                                    document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;
                                                                                }

                                                                                function hasFiles(valor) {
                                                                                    document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                                                                }
                                                                            </script>
                                                                            <asp:Image ID="imgUser" runat="server" BackColor="#CCCCCC" BorderColor="Black"
                                                                                BorderStyle="Double" ImageUrl="~/Imagenes/Logo_User.png"
                                                                                style="max-height:200px" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                      
                                                                  <br />

                                                                 <telerik:RadAsyncUpload 
                                                                runat="server" 
                                                                RenderMode="Lightweight" 
                                                                ID="uploadFile"
                                                                Skin="Metro" 
                                                                TemporaryFolder="~/Temp" 
                                                                TargetFolder="~/Temp" 
                                                                HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx"
                                                                OnClientFileUploaded="fileUploaded" 
                                                                OnClientFileUploadRemoving="fileUploadRemoving" 
                                                                MaxFileInputsCount="1"
                                                                MultipleFileSelection="Disabled" 
                                                                AllowedFileExtensions="jpeg,jpg,gif,png,bmp" 
                                                                PostbackTriggers="">
                                                            </telerik:RadAsyncUpload>

                             
                                                             </div>
                                                                         
                              
                                                            <%--<div class="imageContainer"></div>--%>
                                                            <script src="../Scripts/FileUploadTelerik.js?idtms=0002"></script>
                                                            <script type="text/javascript">
                                                                //Sys.Application.add_load(function () {
                                                                //    demo.initialize();
                                                                //});
                                                            </script>

                                                </div>
                                                <div class="col-md-8 text-left">

                                                        <asp:Label ID="lbl_id_usuario" runat="server" Font-Bold="True" Font-Names="Arial"
                                                                Font-Size="X-Small" ForeColor="#C00000"
                                                                Visible="False"></asp:Label>
                                                        <asp:Label ID="lblt_cargo" runat="server" CssClass="control-label text-bold" Text="Job Title"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                          <telerik:RadComboBox ID="cmb_job_title" 
                                                                    runat="server"
                                                                    EmptyMessage="Select a job tittle..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains"                                      
                                                                    Width="350px" >
                                                                </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                ControlToValidate="cmb_job_title" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="*" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                            <asp:Label runat="server" ID="lbl_JobErr" CssClass="Error" Visible="false" Text="**Select a valid Job Title"></asp:Label><br /><br />
                                                       
                                                           <asp:Label ID="lblt_nombre_usuario" runat="server" CssClass="control-label text-bold" Text="Employee Name"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                            <telerik:RadTextBox ID="txt_nombreUsuario" runat="server" Rows="3" TextMode="SingleLine" Width="350px" MaxLength="250">
                                                            </telerik:RadTextBox>43
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                ControlToValidate="txt_nombreUsuario" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />  
                                                        
                                                         <asp:Label ID="lblt_apellido_usuario" runat="server" CssClass="control-label text-bold" Text="Employee Last Name"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                            <telerik:RadTextBox ID="txt_apellidos" runat="server" Rows="3" Width="350px"
                                                                MaxLength="250">
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                ControlToValidate="txt_apellidos" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />  
                                                    
                                                       <asp:Label ID="lblt_documento" runat="server" CssClass="control-label text-bold" Text="ID"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                        <telerik:RadTextBox ID="txt_documento" runat="server" Rows="3" Width="350px" Text="" MaxLength="50" >
                                                            </telerik:RadTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                   ControlToValidate="txt_documento" CssClass="Error" Display="Dynamic"
                                                                 ErrorMessage="* Required" ValidationGroup="1"></asp:RequiredFieldValidator><br /><br />
                                               
                                                     <asp:Label ID="lblt_email" runat="server" CssClass="control-label text-bold" Text="Email"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                     <telerik:RadTextBox ID="txt_email_usuario" runat="server" MaxLength="100" Width="250px">
                                                     </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                                ControlToValidate="txt_email_usuario" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="* Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                ControlToValidate="txt_email_usuario" ForeColor="Red" ErrorMessage="* Error en Email"
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="1"></asp:RegularExpressionValidator> <br /><br />

                                                    <asp:Label ID="lblt_region" runat="server" CssClass="control-label text-bold" Text="Region"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                       <telerik:RadComboBox 
                                                                ID="cmb_office" 
                                                                runat="server" 
                                                                EmptyMessage="Select a region..."   
                                                                AllowCustomText="true" 
                                                                Filter="Contains"  
                                                                Width="350px">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Error" InitialValue=""
                                                                ControlToValidate="cmb_office" ErrorMessage="* required"
                                                                ValidationGroup="1"></asp:RequiredFieldValidator>      
                                                                                                       
                                                 </div>                      
                                            </div>

                                    <div class="form-group row">             
                                        <hr style="color:gray;" />
                                    </div>
                
                                  <asp:HiddenField ID="TabName" runat="server" Value="Employees" />
                           
                                   <div class="p-0 pt-1" id="Tabs">                                 
                                     <ul class="nav nav-tabs">
                                          <li class="active"><a data-toggle="tab" href="#Employees"><asp:Label runat="server" ID="lblt_Employees" CssClass="control-label text-bold">Employee Profile</asp:Label></a></li>                                    
                                          <li><a data-toggle="tab" href="#Vacation"><asp:Label runat="server" ID="lblt_vacation" CssClass="control-label text-bold">Annual Leave</asp:Label></a></li>
                                          <li><a data-toggle="tab" href="#Cassual"><asp:Label runat="server" ID="lblt_AnnualLEave" CssClass="control-label text-bold">Cassual Leave/ Sick Leave</asp:Label></a></li>
                                          <li><a data-toggle="tab" href="#Time"><asp:Label runat="server" ID="lblt_tiempo" CssClass="control-label text-bold">Reported Time</asp:Label></a></li>
                                          <li><a data-toggle="tab" href="#TimeSheet"><asp:Label runat="server" ID="lblt_hojaTiempo" CssClass="control-label text-bold">Time Sheets</asp:Label></a></li>
                                      </ul>
                                  </div>

                                 <div class="tab-content">
                 
                                      <div id="Employees" class="tab-pane fade in active">

                                              <div class="form-group row" style="width:100%; margin: 0 auto">

			                                        <%--***********REGISTER EMPLOYEES***********--%>
                                                   
                                                     <br /><br />
                                                              <div class="form-group row">                                        
                                                                 <div class="col-md-1 text-right">
                                                                   <asp:Label ID="lblt_gender" runat="server" CssClass="control-label text-bold" Text="Sex"></asp:Label>
                                                                 </div>
                                                                <div class="col-md-3 text-left">                                                      
                                                                       <telerik:RadComboBox 
                                                                                ID="cmb_gender" 
                                                                                runat="server" 
                                                                                EmptyMessage="Select a gender..."   
                                                                                AllowCustomText="true" 
                                                                                Filter="Contains"  
                                                                                Width="200px">
                                                                            </telerik:RadComboBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="Error" InitialValue=""
                                                                                ControlToValidate="cmb_gender" ErrorMessage="* required"
                                                                                ValidationGroup="1"></asp:RequiredFieldValidator>    
                                                                </div>
                                                                 <div class="col-md-1 text-left">
                                                                      <asp:Label ID="lblt_birth_day" runat="server" CssClass="control-label text-bold" Text="Birth Day"></asp:Label>
                                                                 </div>
                                                                 <div class="col-md-3 text-left">  
                                                                      <telerik:RadDatePicker ID="dt_birth_day" runat="server" MinDate="01/01/1940">
                                                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>                                   
                                                                        </telerik:RadDatePicker>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="Error"
                                                                            ControlToValidate="dt_birth_day" ErrorMessage="* required"
                                                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                 </div><br />
                                                             </div>

                                                            <div class="form-group row">       

                                                                  <div class="col-md-1 text-left">
                                                                      <asp:Label ID="lblt_supervisor" runat="server" CssClass="control-label text-bold" Text="Supervisor"></asp:Label>
                                                                 </div>
                                                                   <div class="col-md-8 text-left">                                                                                
                                                                              <telerik:RadComboBox 
                                                                                ID="cmb_supervisor" 
                                                                                runat="server" 
                                                                                EmptyMessage="Select a user..."   
                                                                                AllowCustomText="true" 
                                                                                Filter="Contains"  
                                                                                Width="300px">
                                                                            </telerik:RadComboBox>
                                                                           <asp:Label runat="server" ID="lbl_supervisorErr" CssClass="Error" Visible="false" Text="* Requerido"></asp:Label> &nbsp;&nbsp;&nbsp;
                                                                           <asp:CheckBox ID="chk_SupervisorNA" runat="server" Text="No Aplica" /><br /><br />
                                                                   </div>                                                                 
                                                            </div>

                                                           <div class="form-group row">                                        
                                                             <div class="col-md-1 text-right">                       
                                                               <asp:Label ID="lblt_fecha_contrato" runat="server" CssClass="control-label text-bold" Text="Contract Date"></asp:Label>
                                                             </div>
                                                            <div class="col-md-3 text-left"> 
                                                                        <telerik:RadDatePicker ID="dt_fecha_contrato" runat="server">
                                                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>                                   
                                                                        </telerik:RadDatePicker>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                                                            ControlToValidate="dt_fecha_contrato" ErrorMessage="* required"
                                                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-1 text-left">
                                                                <asp:Label ID="lblt_fecha_vence" runat="server" CssClass="control-label text-bold" Text="Expire Date"></asp:Label>                    
                                                            </div>
                                                            <div class="col-md-4 text-left">
                                                                  <telerik:RadDatePicker ID="dt_fecha_contrato_vence" runat="server">
                                                                  <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>                                   
                                                                  </telerik:RadDatePicker>
                                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator_vence" runat="server" CssClass="Error"
                                                                            ControlToValidate="dt_fecha_contrato_vence" ErrorMessage="* required"
                                                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>   
                                                             <br />
                                                          </div>

                                                        <div class="form-group row">                                        

                                                             <div class="col-md-1 text-right">                       
                                                               <asp:Label ID="lblt_telephone" runat="server" CssClass="control-label text-bold" Text="Phone Number"></asp:Label><br /><br />
                                                               <asp:Label ID="lblt_Celular" runat="server" CssClass="control-label text-bold" Text="Mobil Number"></asp:Label>
                                                             </div>
                                                             <div class="col-md-3 text-left"> 
                                                                 <telerik:RadTextBox ID="txt_telefono" runat="server" MaxLength="100" Width="150px">
                                                                 </telerik:RadTextBox><br /><br />
                                                                 <telerik:RadTextBox ID="txt_mobile" runat="server" MaxLength="100" Width="150px">
                                                                 </telerik:RadTextBox>
                                                             </div>
                                                            <div class="col-md-1 text-left">
                                                                <asp:Label ID="lblt_direccion" runat="server" CssClass="control-label text-bold" Text="Address"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4 text-left">
                                                                 <telerik:RadTextBox ID="txt_direccion" runat="server" MaxLength="200" TextMode="MultiLine"  Rows="4" Width="350px"  >
                                                                 </telerik:RadTextBox>
                                                            </div>

                                                        </div>


                                              <div class="form-group row">             
                                                <hr style="color:gray;" />
                                               </div>

                                                         <div class="form-group row">                                        
                                                             <div class="col-md-1 text-right">
                                                                <asp:Label ID="lblt_salary" runat="server" CssClass="control-label text-bold" Text="Monthly Salary"></asp:Label>
                                                             </div>
                                                             <div class="col-md-3 text-left"> 
                                                               <telerik:RadNumericTextBox  runat="server" ID="txt_salary" Width="125px" Value="0" >
                                                                    <NumberFormat DecimalDigits="2" />  
                                                               </telerik:RadNumericTextBox>
                                                             </div>
                                                            <div class="col-md-1 text-left">
                                                                 <asp:Label ID="lblt_worked_days" runat="server" CssClass="control-label text-bold" Text="">Balance Days</asp:Label><br /><br />
                                                                 <asp:Label ID="lblt_exec_days" runat="server" CssClass="control-label text-bold" Text="">Worked Days</asp:Label>
                                                            </div>
                                                            <div class="col-md-4 text-left">
                                                              <telerik:RadNumericTextBox  runat="server" ID="txt_contract_days" Width="125px" Value="0" >
                                                                    <NumberFormat  DecimalDigits="2" />  
                                                               </telerik:RadNumericTextBox><br /><br /><br />
                                                               <telerik:RadNumericTextBox  runat="server" ID="txt_worked_days" Width="125px" Value="0" >
                                                                    <NumberFormat  DecimalDigits="2" />  
                                                               </telerik:RadNumericTextBox >                       
                                                            </div>
                                                         </div>
                                                          <div class="form-group row">             
                                                            <hr style="color:gray;" />
                                                          </div>
                                                         <div class="form-group row">   
                                                             <div class="col-md-1 text-right">
                                                                <asp:Label ID="lblt_note1" runat="server" CssClass="control-label text-bold" Text="Notes"></asp:Label>
                                                             </div>
                                                             <div class="col-md-8 text-left"> 
                                                                   <telerik:RadTextBox ID="txt_note1" runat="server" TextMode="MultiLine"  Rows="8" Width="90%"  >

                                                                  </telerik:RadTextBox>
                                                             </div>                     
                                                        </div>
                                                          <div class="form-group row">                                        
                                                              <br />
                                                             <div class="col-md-1 text-right">
                                                                <asp:Label ID="lblt_note2" runat="server" CssClass="control-label text-bold" Text="Notes II "></asp:Label>
                                                             </div>
                                                             <div class="col-md-8 text-left"> 
                                                                   <telerik:RadTextBox ID="txt_note2" runat="server" TextMode="MultiLine"  Rows="8" Width="90%"  >

                                                                  </telerik:RadTextBox>
                                                             </div>

                                                        </div>                
                                                          <div class="form-group row">             
                                                            <hr style="color:gray;" />
                                                          </div>

                                                          <div class="form-group row">                                        
                                                             <div class="col-md-1 text-right">
                                                                <asp:Label ID="lblt_createdBy" runat="server" CssClass="control-label text-bold" Text="Created By"></asp:Label>
                                                             </div>
                                                             <div class="col-md-3 text-left"> 
                                                                <asp:Label runat="server" ID="lbl_createdBy"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_created_date"></asp:Label>
                                                             </div>
                                                               <div class="col-md-1 text-right">
                                                                <asp:Label ID="lblt_updatedBy" runat="server" CssClass="control-label text-bold" Text="Updated By"></asp:Label>
                                                             </div>
                                                             <div class="col-md-4 text-left">
                                                                  <asp:Label runat="server" ID="lbl_updatedBy"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lbl_updated_date"></asp:Label>
                                                             </div>
                                                         </div>
                   
                           <%--***********REGISTER EMPLOYEES***********--%>

                         </div>    <%--<div class="form-group row">   --%> 
       
                     </div> <%--id=Employees"--%>


                      <div id="Vacation" class="tab-pane fade">

                            <div class="form-group row">
                                   <div class="col-sm-12 text-left">
                                         <br /><br />
                                         <div class="box" style="border-top-color:lightgray">
                                                  <div class="box-header">
                                                      <h3 class="box-title">Annual Leave Summary</h3>
                                                  </div><!-- /.box-header -->
                                                 <div class="box-body no-padding">                                                                     
                                                             <div style=" width:100%; overflow-x:auto" >
                                                                  <%=str_TABLE %>
                                                             </div>
                                                                            
                                                    </div><!-- /.box-body -->
                                              </div><!-- /.box -->
                                      </div>
                                 </div>

                     
                     </div>


                     <div id="Cassual" class="tab-pane fade">

                            <div class="form-group row">
                                   <div class="col-sm-12 text-left">
                                         <br /><br />
                                         <div class="box" style="border-top-color:lightgray">
                                                  <div class="box-header">
                                                      <h3 class="box-title">Cassual Leave / Sick Leave</h3>
                                                  </div><!-- /.box-header -->
                                                 <div class="box-body no-padding">                                                                     
                                                             <div style=" width:100%; overflow-x:auto" >
                                                                  <%=str_TABLE_CAS %>
                                                             </div>
                                                                            
                                                    </div><!-- /.box-body -->
                                              </div><!-- /.box -->
                                      </div>
                                 </div>

                     
                     </div>

              
                     <div id="Time" class="tab-pane fade" >

                         
                        <script type="text/javascript">

                            <%-- function pageLoad() {

                                // alert('Hi there');

                                    var grid = $find("<%= grd_cate.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                             }
                            
                            <ClientEvents OnGridCreated="resizeGrid" />
                            
                            --%>

                            //function resizeGrid(sender) {
                            //           // debugger;
                            //            var grid = sender;
                            //            if (grid != null) {
                            //                var master = grid.get_masterTableView();
                            //                if (master != null) {
                            //                    var rows = master.get_dataItems();
                            //                    if (rows != null) {
                            //                        if (rows.length > 0) {
                            //                            var columns = master.get_columns();
                            //                            for (var i = 0; i < columns.length; i++) {
                            //                                columns[i].resizeToFit(false, true);
                            //                            }
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }

                               var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_endRequest(function (s, e) {
                                          loadscript();
                                    })

                                $(document).ready(function () {
                                      loadscript();
                                })


                             function loadscript() {

                                // console.log('Starting LoadScript...');

                                    //*******************************************TABS**************************************************************************
                                    //*******************************************TABS**************************************************************************

                                    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Employees";
                                    $('#Tabs a[href="#' + tabName + '"]').tab('show');

                                    $("#Tabs a").click(function () {

                                        //console.log('New tab value ' + $(this).attr("href").replace("#", "") );
                                        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));

                                    });

                                    //*******************************************TABS**************************************************************************
                                    //*******************************************TABS**************************************************************************

                                }

                           </script>
              

                          <div class="form-group row" style="width:100%; margin: 0 auto">                          
                             
                                  <%--***********Users by Register***********--%>

                               <asp:LinkButton ID="btnlk_Export" runat="server" AutoPostBack="True" SingleClick="true"  Text="Exportar" Width="12%" class="btn btn-success btn-sm margin-r-5 pull-right" data-toggle="Export"  ><i class="fa fa-file-excel-o fa-2x"></i>&nbsp;&nbsp;Export Detail</asp:LinkButton>   <br /><br />                      
                      

                                <asp:HiddenField ID="hd_tp" runat="server" Value="1" />                              

                              
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

                                          .rpgRowHeaderGrandTotal{
                                               height:45px !important;    
                                          }

                                          /*.rpgRowHeader, .rpgRowHeaderField{
                                               height:28px !important; 
                                          }*/

                                          #ctl00_MainContent_radgrid_time_reported_ctl02_RowZone1        
                                          {
                                             height:20px !important;            
                                          }
          

                                        </style>
                                      

                                             <telerik:RadPivotGrid 
                                                      RenderMode="Lightweight" 
                                                      runat="server" 
                                                      ID="radgrid_time_reported" 
                                                      Width  = "100%"                                                     
                                                      ColumnHeaderZoneText="ColumnHeaderZone"  >

                                                        

                                                        <Fields>                                                                                                                    

                                                            <telerik:PivotGridRowField DataField="anio" Caption="Year" CellStyle-Width="100px" CellStyle-Height="29px"  ZoneIndex="0">
                                                            </telerik:PivotGridRowField>

                                                             <telerik:PivotGridRowField DataField="billable_time" Caption="billable_time" CellStyle-Width="225px" CellStyle-Height="29px"   ZoneIndex="1">
                                                            </telerik:PivotGridRowField>

                                                            <telerik:PivotGridColumnField  DataField="mes" Caption="month"  CellStyle-Height="29px"   >
                                                            </telerik:PivotGridColumnField>

                                                            <telerik:PivotGridColumnField  DataField="FiscalYearNotation" Caption="Period" CellStyle-Width="100px" CellStyle-Height="35px"   >
                                                            </telerik:PivotGridColumnField>
                                                            
                                                            <telerik:PivotGridAggregateField DataField="Ds" Aggregate="Sum" DataFormatString="{0:N2}" CellStyle-Height="29px"  >
                                                            </telerik:PivotGridAggregateField>
                                                            
                                                        </Fields>                                          
                                                      
                                                      <ColumnHeaderCellStyle Width="100px"></ColumnHeaderCellStyle>
                                                      <DataCellStyle Width="80px" />
                                                 
                                                      <SortExpressions>
                                                         <telerik:PivotGridSortExpression FieldName="mes" SortOrder="Ascending"></telerik:PivotGridSortExpression>
                                                      </SortExpressions>
                
                                                  </telerik:RadPivotGrid>
                              

  	                              <%--***********Users by Register***********--%>

                            </div>    <%--<div class="form-group row">   --%>                             

                       </div> <%--id="Users"--%>

                       <div id="TimeSheet" class="tab-pane fade" >

                          <div class="form-group row" style="width:100%; margin: 0 auto">                          
                             
                                <%--***********TimeSheet Users***********--%>
                                <br /><br />

                                <telerik:RadGrid ID="grd_timesheet"  
                                          Skin="Office2010Blue"   
                                          runat="server" 
                                          AllowAutomaticDeletes="True" 
                                          CellSpacing="0"  
                                          GridLines ="None" 
                                          Width="100%" 
                                          AllowAutomaticUpdates="True"  
                                          AutoGenerateColumns="False" >

                                                <ClientSettings EnableRowHoverStyle="true" >
                                                <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                          
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_timesheet" AllowAutomaticUpdates="True" >
                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                          

                                             <Columns>                                           
                                             <telerik:GridTemplateColumn FilterControlAltText="Filter edit column" UniqueName="colm_edit">
                                                            <ItemTemplate>
                                                              <asp:ImageButton ID="editar" runat="server" 
                                                               ImageUrl   ="~/Imagenes/iconos/b_edit.png" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10px" />
                                             </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn  
                                                                    DataField="id_timesheet" 
                                                                    UniqueName="id_timesheet" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn>                                                           
                                                            <telerik:GridBoundColumn  
                                                                    DataField="usuario_creo" 
                                                                    UniqueName="usuario_creo" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn  
                                                                    DataField="id_usuario" 
                                                                    UniqueName="id_usuario" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn DataField="nombre_usuario" 
                                                                    FilterControlAltText="Filter nombre_usuario column" 
                                                                    HeaderText="Name" SortExpression="nombre_usuario"  
                                                                    UniqueName="nombre_usuario">
                                                                    <ItemStyle Width="20%" />
                                                                </telerik:GridBoundColumn>   
                                                                <telerik:GridBoundColumn DataField="TimeSheet_Type" 
                                                                    FilterControlAltText="Filter job column" HeaderText="Report type" 
                                                                    SortExpression="job" UniqueName="job" >
                                                                    <ItemStyle Width="20%" />
                                                                </telerik:GridBoundColumn>        
                                                                <telerik:GridBoundColumn DataField="anio" 
                                                                    FilterControlAltText="Filter anio column" HeaderText="Year" 
                                                                    SortExpression="anio" UniqueName="anio">
                                                                </telerik:GridBoundColumn>                                                         
                                                                <telerik:GridBoundColumn DataField="month_name" 
                                                                    FilterControlAltText="Filter mes column" HeaderText="Month" 
                                                                    SortExpression="mes" UniqueName="mes">
                                                                </telerik:GridBoundColumn>                                                    
                                                                 <telerik:GridBoundColumn DataField="description" 
                                                                    FilterControlAltText="Filter description column" 
                                                                    HeaderText="Description" SortExpression="description" 
                                                                    UniqueName="description">
                                                                    <ItemStyle Width="20%" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="timesheet_estado" 
                                                                    FilterControlAltText="Filter timesheet_estado column" HeaderText="Status" 
                                                                    SortExpression="timesheet_estado" UniqueName="timesheet_estado">
                                                                </telerik:GridBoundColumn>                                                       
                                                                                                                                                                                    
                                                                <telerik:GridBoundColumn DataField="employee_type"  HeaderText="Timesheet type" 
                                                                    FilterControlAltText="Filter employee_type column" SortExpression="employee_type" 
                                                                    UniqueName="employee_type" Visible="true" Display="true">
                                                                </telerik:GridBoundColumn>                                                              
                                                                <telerik:GridTemplateColumn UniqueName="colm_open" >
                                                                      <ItemTemplate>
                                                                           <asp:HyperLink ID="hlk_timesheet" runat="server" ImageUrl="~/Imagenes/iconos/Preview.png" 
                                                                            ToolTip="Soporte" Target="_new">
                                                                            </asp:HyperLink>
                                                                       </ItemTemplate>
                                                                    <ItemStyle Width="10px" />
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>

                                                            <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="nombre_usuario" FieldName="nombre_usuario" FormatString="" HeaderText="" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="id_usuario" />
                                                                    </GroupByFields>
                                                                </telerik:GridGroupByExpression>
                                                            </GroupByExpressions>

                                                        </MasterTableView>
                                                                                                
                                         </telerik:RadGrid>
                             
                              <br /><br />
                                <%--***********TimeSheet ***********--%>

                            </div>    <%--<div class="form-group row">   --%>                             

                       </div> <%--id="Users"--%>

                                     
                 </div> <%--<div class="tab-content">--%>
                        
                      
                        <!-- /.box-body -->
                        <div class="box-footer">
                              <telerik:RadButton ID="btn_test" runat="server" Text="Test" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" 
                                Width="100px" Visible="false" >
                            </telerik:RadButton>
                         <%--   <asp:CheckBox ID="chk_Notify"  runat="server" Text="Notify the user" CssClass="btn btn-sm pull-right " />--%>
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardar" runat="server" Text="Save" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                                Width="100px" ValidationGroup="1">
                            </telerik:RadButton>
                            <asp:Label runat="server" ID="lbl_mensajeError" CssClass="Error pull-right margin-r-5"></asp:Label>
                        </div>
                    </div>
                    <!-- /.box-footer -->
                </div>
        
    </section>
</asp:Content>
