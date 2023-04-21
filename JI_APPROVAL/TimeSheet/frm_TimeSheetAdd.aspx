<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheetAdd"  Codebehind="frm_TimeSheetAdd.aspx.vb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
        
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEET</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                         <div class="col-sm-10">   
                            <h3 class="box-title">
                                <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Time Sheet</asp:Label><asp:Label runat="server" ID="lblt_subtitulo_pantalla_2"  Visible="false">New Leave Approval</asp:Label>  
                            </h3>    
                        </div>
                         <div class="col-sm-1 pull-right">   
                            <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('Time_sheet_02.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>                                                            
                        </div>
                    </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">
                                                              
                                            <div class="stepwizard">
                                                    <div class="stepwizard-row setup-panel">
                                                        <div class="stepwizard-step" style="width:25%">
                                                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_informaciongeneral">Información de la hoja de tiempo</asp:Label><asp:Label runat="server" ID="lblt_informaciongeneral2" Visible="false">Licencias/Vacaciones </asp:Label>
                                                            </p>
                                                        </div>
                                                        <div class="stepwizard-step" style="width:25%">
                                                            <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                                            <p>
                                                                <asp:Label runat="server" ID="lblt_personal_status">Días</asp:Label>
                                                            </p>
                                                        </div>
                                                         <div class="stepwizard-step" style="width:25%">
                                                             <a   href="#step-3" id="anchorSupportDocs" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                                             <p>
                                                                 <asp:Label runat="server" ID="lblt_Support_Documents">Documentos</asp:Label>
                                                             </p>
                                                         </div>
                                                         <div class="stepwizard-step" style="width:25%">
                                                             <a   href="#step-4" id="anchorFollowUp" runat="server" type="button" class="btn btn-default btn-circle">4</a>
                                                             <p>
                                                                 <asp:Label runat="server" ID="lblt_complementary">Seguimiento</asp:Label>
                                                             </p>
                                                         </div>                                    
                                                    </div>
                                                </div>

                                                <hr />
                                   

                            <div class="col-lg-12">
                               <div class="box-body">

                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label>   
                                           <asp:Label ID="lbl_idTemp" runat="server" Visible="False"></asp:Label>
                                           <asp:Label ID="lblt_Employee_associated" runat="server" CssClass="control-label text-bold"   Text="Employee Name"></asp:Label>
                                        </div>
                                       <div class="col-sm-4">
                                    
                                            <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_usuario" 
                                                                   runat ="server" 
                                                                    CausesValidation="False"                                                                     
                                                                    EmptyMessage="Select an employee..."   
                                                                    AllowCustomText="true" 
                                                                    Filter="Contains"                                                  
                                                                    Height="200px"
                                                                    Width="95%"
                                                                    OnDataBound="cmb_usuario_DataBound" 
                                                                    OnItemDataBound="cmb_usuario_ItemDataBound"
                                                                    OnClientItemsRequested="UpdateItemCountField"                                                                     
                                                                    OnClientSelectedIndexChanged ="getJOB" >                                                              
                                                            <HeaderTemplate>
                                                                    <ul>
                                                                        <li class="col1">Employee</li>
                                                                        <li class="col2">Job Tittle</li>                                                                        
                                                                    </ul>
                                                                </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <ul>
                                                                    <li class="col1">
                                                                        <%# DataBinder.Eval(Container.DataItem, "nombre_usuario")%></li>
                                                                    <li class="col2">
                                                                        <%# DataBinder.Eval(Container.DataItem, "job")%></li>                                                                   
                                                                </ul>
                                                            </ItemTemplate>
                                                        <FooterTemplate>
                                                            A total of
                                                            <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                            items
                                                        </FooterTemplate>
                                            </telerik:RadComboBox>

                                           <%--DataSourceID="sql_rol"--%>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_usuario" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                           
                                                  <script type="text/javascript">

                                             function UpdateItemCountField(sender, args) {
                                                 //set the footer text
                                                 sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
                                             }


                                             function getJOB(sender, eventArgs) {

                                                 var combobox = $find("<%= cmb_usuario.ClientID %>");
                                                 var comboboxType_ = $find("<%= cmb_EmployeeType.ClientID %>");
                                                 var value = combobox.get_selectedItem().get_value();
                                                 var texto = combobox.get_selectedItem().get_text();

                                                 //alert('User: ' + value + ' ' + texto + ' Program: ' + <%=Me.Session("E_IDPrograma")%>);

                                                 $.ajax({
                                                     type: "POST",
                                                     url: "frm_TimeSheetAdd.aspx/GetUSerJobTittle",
                                                     data: '{idUsuario:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }',
                                                     contentType: "application/json; charset=utf-8",
                                                     dataType: "json",
                                                     success: function (msg) {
                                                         $('#<%=Me.lblJobTittle.ClientID%>').text(msg.d);
                                                         comboboxType_.clearSelection();
                                                     },
                                                     failure: function (response) {
                                                         alert('Error: ' + response.d);
                                                     }   
                                                 });
                                                 
                                              }
                                                      
                                            </script>  

                                       </div>
                                       <div class="col-sm-3">   
                                           <!--Control -->
                                           <asp:HiddenField runat="server" ID="hd_idTimeSheet" Value ="0" />
                                           <asp:HiddenField runat="server" ID="hd_leave" Value ="0" />
                                           <asp:Label ID="lblJobTittle" runat="server" ssClass="control-label text-bold"  ></asp:Label>
                                           <asp:Label ID="lbl_IdEmpleado" runat="server" Visible="False"></asp:Label>
                                           <!--Control -->
                                       </div>

                                        <div class="col-sm-3">   
                                           <!--Control -->

                                             <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_EmployeeType" 
                                                                   runat ="server" 
                                                                   EmptyMessage="select the TS template..."                                                                                                                                                                                                                                                                                                           
                                                                   Width="100%">
                                                      
                                            </telerik:RadComboBox>

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                                                    runat="server" 
                                                    ForeColor="Red"
                                                    ControlToValidate="cmb_EmployeeType" 
                                                    ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>                 
                                           
                                           <!--Control -->
                                       </div>

                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-10 text-left">
                                         <!--Tittle -->
                                             <%--<asp:Label ID="lblt_empleoyee_ass" runat="server"  CssClass="control-label text-bold"  Text="Job Tittle"></asp:Label>--%>
                                            <br />
                                        </div>
                                      
                                    </div>


                                      <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lbl_anio" runat="server" CssClass="control-label text-bold" Text="Periodo (Año):"></asp:Label>
                                        </div>
                                       <div class="col-sm-2">
                                           
                                         
                                           <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_year" 
                                                                   runat ="server" 
                                                                   EmptyMessage="Select a year..."  
                                                                   DataTextField ="year" 
                                                                   DataValueField="id_year" 
                                                                   AutoPostBack="true" 
                                                                   Width="60%">
                                                      
                                            </telerik:RadComboBox>

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_year" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>

                                       <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lblMonth" runat="server" CssClass="control-label text-bold"   Text="Periodo (Mes):"></asp:Label>
                                        </div>
                                       <div class="col-sm-2">  
                                           <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_Month" 
                                                                   runat ="server" 
                                                                   EmptyMessage="Select a year..."  
                                                                   DataTextField ="month" 
                                                                   DataValueField="id_month"    
                                                                   AutoPostBack="true"                                               
                                                                   Width="90%" >
                                                      
                                            </telerik:RadComboBox>

                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_Month" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>


                                       </div>


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
                                        <div class="col-sm-10 text-left">
                                             <hr />
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
                               <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm " Width="100" ValidationGroup="1" ></telerik:RadButton>
                             </div>
                            <div class="col-sm-2 text-left">
                               <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Save and Continue" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-o-right"></i></asp:LinkButton>                                              
                            </div>
                            <div class="col-sm-8 text-left">
                               <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                            </div>
                       </div>       
                                                                
                   </div>


                     <div class="modal fade bs-example-modal-sm" id="modalMensaje" data-backdrop="static" data-keyboard="false">
                         <div class="vertical-alignment-helper">
                                <div class="modal-dialog modal-lg  vertical-align-center">
                                    <div class="modal-content">
                                        <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                            <asp:HiddenField runat="server" ID="h_H1" Value ="Existing TimeSheets for {0}" />
                                            <h4 class="modal-title" runat="server" id="H1"></h4>
                                        </div>
                                         <div class="modal-body">

                                             <div class="form-group row">
                                                    <div class="col-sm-10 text-left">
                                                         <asp:HiddenField runat="server" ID="h_texto_mensaje" Value ="There are {0} TimeSheet{1} for {2} already created" />
                                                        <h4 id="texto_mensaje" runat="server"></h4>
                                                         <%--<asp:Label ID="lblt_message_time_sheet" runat="server" Text="Ya existen hojas de tiempo para  creada, desea editarla?" />--%>
                                                    </div>
                                             </div>

                                             <div class="form-group row">

                                                 <div class="col-sm-12">

                                                           <!--**************************HERE THE MATCH*************************************-->
                                                                 
                                                                              <table class="table table-striped" style="width:100%;">
                                                                                <tr>
                                                                                  <th>Job Title</th>
                                                                                  <th style="width:auto;">Employee</th>
                                                                                  <th>Description</th>   
                                                                                  <th>Notes</th>   
                                                                                  <th>Status</th> 
                                                                                  <th>Date Created</th>                                                                                                                                           
                                                                                </tr>
                                                                                   <asp:Repeater ID="rept_timesheet" runat="server" >
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                              <td><%# Eval("job")%></td>
                                                                                              <td><%# Eval("nombre_usuario")%></td>
                                                                                              <td class="text-justify">    
                                                                                                <%# Eval("description")%>                                                                                                                                                         
                                                                                              </td>
                                                                                              <td class="text-justify">    
                                                                                                <%# Eval("notes")%>                                                                                                                                                         
                                                                                              </td>
                                                                                              <td><%# Eval("timesheet_estado")%></td>
                                                                                              <td>
                                                                                               <div class="direct-chat-info clearfix">
                                                                                                     <span class="direct-chat-timestamp "> <%# getFecha(Eval("fecha_creo"), "m", False) %> <i class="fa fa-clock-o"></i> <%#  getHora(Eval("fecha_creo")) %> </span>
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

                                                                                <!--**************************HERE THE MATCH*************************************-->

                                                 </div>

                                             </div>
                                           

                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btn_ok" CssClass="btn btn-md btn-primary btn-ok" Text="Edit the previous TimeSheet" data-dismiss="modal" UseSubmitBehavior="false"  />
                                            <button class="btn btn-md" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2" onclick="javacript:cancel_Mensaje();">Keep creating this new TimeSheet</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                </div>

                
                <script type="text/javascript">

                    //function doClick() {

                    //    PageMethods.EXEC_Action(On_Success, On_Failure);

                    //}

                    //function On_Success(result) {

                    //    alert(result);
                        
                    //}

                    //function On_Failure(result) {

                    //    alert(result);
                                                
                    //}

                    function FuncModal_Mensaje() {
                        $('#modalMensaje').modal('show');
                    }

              
                   function cancel_Mensaje() {
                       $('#modalMensaje').modal('hide');
                       $("#<%= btnlk_continue.ClientID%>").click();
                    }

               </script>

           </section>
       
    

    </asp:Content>

