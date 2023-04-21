<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheet_app_edit"  Codebehind="frm_TimeSheet_app_edit.aspx.vb" %>


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
                                <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Edit TimeSheet Approval</asp:Label>  
                            </h3>    
                        </div>
                         <div class="col-sm-1 pull-right">   
                         
                        </div>
                    </div>
                
                        <div class="box-body">
                             
                            <div class="col-lg-12">

                             <div class="box-body">
                                   
                             <div class="col-sm-12">   
                                  <h4 class="box-title">
                                            <asp:Label runat="server" ID="lbl_Editar_aprobacion">Edit TimeSheet Approval</asp:Label>  
                                  </h4>    
                                  <hr />
                             </div>

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
                                                                    Height="200px"
                                                                    Width="95%"                                                                                                                                                                                                                                                            
                                                                    OnClientSelectedIndexChanged ="getJOB" >
                                            </telerik:RadComboBox>


                                            <%--  <HeaderTemplate>
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
                                                        </FooterTemplate>--%>

                                           <%--DataSourceID="sql_rol"--%>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_usuario" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                           
                                                  <script type="text/javascript">

                                             //function UpdateItemCountField(sender, args) {
                                             //    //set the footer text
                                             //    sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
                                             //}


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
                                                         //comboboxType_.clearSelection();
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
                                           <asp:Label ID="lbl_anio" runat="server" CssClass="control-label text-bold" Text="Pay Period (Month):"></asp:Label>
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
                                          <asp:Label ID="lblMonth" runat="server" CssClass="control-label text-bold"   Text="Pay Period (Year):"></asp:Label>
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
                               <asp:LinkButton ID="btnlk_continue" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Approval Modification" Width="99%" class="btn btn-primary btn-sm margin-r-5 pull-left"  ValidationGroup="1" >Save and Continue&nbsp;&nbsp;&nbsp;<i class="fa fa-edit"></i></asp:LinkButton>                                              
                            </div>
                            <div class="col-sm-8 text-left">
                               <asp:Label ID="lblError" runat="server" CssClass="control-label alert-error" Visible="false" ></asp:Label>                             
                            </div>
                       </div>       
                                                                
                   </div>

                </div>

           </section>



    
    

    </asp:Content>

