<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_rolesAPP" Codebehind="frm_APProles.aspx.vb" %>


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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Role</asp:Label>
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
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                               <telerik:RadTextBox ID="txt_cat" Runat="server" 
                                                EmptyMessage="Type Role name here.." LabelWidth="" Width="150px" ValidationGroup="1">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                              </telerik:RadTextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
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
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                        ControlToValidate="txt_des" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_Employee" runat="server"  CssClass="control-label text-bold"  Text="Employee"></asp:Label>
                                        </div>
                                       <div class="col-sm-8 text-left">
                                           <!--Control -->
                                           
                                            <telerik:RadComboBox ID="cmb_usu" 
                                                Runat="server" 
                                                CausesValidation="False" 
                                                DataSourceID="SqlDataSource2" 
                                                 EmptyMessage="Select a user..."   
                                                 AllowCustomText="true" 
                                                 Filter="Contains"                                                  
                                                 Height="200px"
                                                 DataTextField="nombre_empleado" 
                                                 DataValueField="id_usuario"  
                                                 CssClass="pull-left"  
                                                 Width="60%">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                             </telerik:RadComboBox>
                                                                                      
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" 
                                                ControlToValidate="cmb_usu" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                           <br /><br />
                                            <asp:Label ID="lblt_filerequeried" runat="server" Text="We don´t have employes to relate at this role" ForeColor="Red" Visible="false"></asp:Label>
                                                                                         

                                       </div>
                                    </div>
                                                                 
                                   

                                  </div>
                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                       <telerik:RadButton ID="btn_save" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="Save" ValidationGroup="1" CssClass="btn btn-sm pull-left"  Width="100px" Enabled="false" ></telerik:RadButton>
                       <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 
                       <telerik:RadButton ID="btn_cancel" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="Cancel" CssClass="btn btn-sm pull-left"  Width="100px" ></telerik:RadButton>
                         
                   </div>

                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                    ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"                             
                                                    SelectCommand=" Select id_usuario, nombre_empleado, estado 
                                                                      from vw_user_role_simple
                                                                       where id_programa =  @id_program 
	                                                                   and (upper(estado) = 'ACTIVE' or upper(estado) = 'ACTIVO') 
	                                                                   and id_rol_user = 0 " >
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                  

                </div>


           </section>
    
    </asp:Content>

