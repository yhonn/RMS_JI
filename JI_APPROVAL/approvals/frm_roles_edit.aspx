<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_roles_edit" Codebehind="frm_roles_edit.aspx.vb" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   
         <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">CATALOGS</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Edit Role</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_Nombre_Role" runat="server" CssClass="control-label text-bold" Text="Role"></asp:Label>
                                              <asp:HiddenField ID="HiddenField1" runat="server" />
                                              <asp:HiddenField ID="hidd_id_rol_user" runat="server" />
  
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                               <telerik:RadTextBox ID="txt_cat" Runat="server" 
                                                EmptyMessage="Type Role name here.." LabelWidth="" Width="150px" ValidationGroup="1">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                              </telerik:RadTextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  ForeColor="Red" runat="server" 
                                                ErrorMessage="Required" ControlToValidate="txt_cat" 
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_Description" runat="server" CssClass="control-label text-bold"  Text="Description"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                               <telerik:RadTextBox ID="txt_des" Runat="server" EmptyMessage="Type Description here.." LabelWidth="" Width="500px" ValidationGroup="1">
                                                        <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                                </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  ForeColor="Red" runat="server" 
                                                        ControlToValidate="txt_des" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_Employee" runat="server"  CssClass="control-label text-bold"  Text="Employee"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                          <telerik:RadComboBox ID="cmb_usu" 
                                              Runat="server" 
                                              CausesValidation="False" 
                                              DataSourceID="SqlDataSource2" 
                                              DataTextField="nombre_empleado" 
                                              DataValueField="id_usuario"
                                              EmptyMessage="Select a user..."   
                                              AllowCustomText="true" 
                                              Filter="Contains"                                                  
                                              Height="200px"
                                              Width="350px">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                             </telerik:RadComboBox> &nbsp;&nbsp;
                                            
                                             <asp:CheckBox ID="chk_RemoveUser" runat="server" Text="  Remove User  " AutoPostBack="True" Width="100px" CssClass="btn btn-sm"  />

                                            <asp:Label ID="lblt_filerequiered" runat="server" Text="Field required" Visible="false"></asp:Label>
                                               
                                               <asp:SqlDataSource ID="SqlDataSource2" runat="server" 

                                                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 

                                                    ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"    
                                                                            
                                                    SelectCommand=" SELECT a.id_usuario, nombre_empleado 
                                                                      FROM vw_user_role_simple a    
                                                                       where a.id_programa = @id_program     
                                                                       and (a.estado = 'ACTIVE' or a.estado = 'ACTIVO')  ">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" />
                                                        
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                       </div>
                                    </div>



                                  </div>
                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer" style="padding-right:10em;">                            
                        <!--Controls -->
                         <telerik:RadButton ID="btn_save" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Save" ValidationGroup="1" CssClass="btn btn-sm pull-left" Width="100px" Enabled="false"></telerik:RadButton>
                         <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>  
                         <telerik:RadButton ID="btn_cancel" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Cancel" CssClass="btn btn-sm pull-left" Width="100px">
                         </telerik:RadButton>                     
                         <span class=" pull-right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>                         
                         <asp:Label ID="lblmsj_err" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                         
                   </div>

                </div>
           </section>
    
     

    </asp:Content>

