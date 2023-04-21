<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_rolesSH" Codebehind="frm_rolesSH.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Role (Shared)</asp:Label>
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
                                             <asp:HiddenField ID="hd_dtRoles" runat="server" Value="" />
                                             <asp:HiddenField ID="hd_id_rol" runat="server" Value="" />
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                               <telerik:RadTextBox ID="txt_cat" Runat="server" 
                                                EmptyMessage="Type Role name here.." LabelWidth="" Width="150px" ValidationGroup="1">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                              </telerik:RadTextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ForeColor="Red" 
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
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ForeColor="Red" 
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
                                           
                                            <telerik:RadComboBox ID="cmb_usu" Runat="server" CausesValidation="False"
                                                 DataTextField="nombre_empleado" 
                                                 DataValueField="id_usuario"  
                                                 CssClass=" btn-default input-sm pull-left " 
                                                 EmptyMessage="Select a user..."   
                                                 AllowCustomText="true" 
                                                 Filter="Contains"                                                  
                                                 Height="200px"
                                                 Width="60%">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                             </telerik:RadComboBox>
                                           <span style="width:5%; ">&nbsp;&nbsp;&nbsp;&nbsp;</span> 
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ForeColor="Red" 
                                                    ControlToValidate="cmb_usu" ErrorMessage="Required" 
                                                    ValidationGroup="3"></asp:RequiredFieldValidator>
                                           
                                           <asp:LinkButton ID="btnlk_addEmployeed" runat="server" AutoPostBack="True" SingleClick="true" SingleClickText="Processing..."  Text="Add Employee" Width="20%" CssClass="btn btn-sm btn-default " ValidationGroup="3"    >Add Employee <span class="glyphicon glyphicon-plus-sign"></span></asp:LinkButton> 
                                                       

                                       </div>
                                    </div>
                                                                 
                                   

                                  </div>
                               </div> 



                                      <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 ">
                                           <!--Control -->
                                              
                                                  <asp:Label ID="lblt_filerequeried" runat="server" Text="We don´t have more employes to relate at this role" ForeColor="Red" Visible="false"></asp:Label>
                                                   <hr />    
                                                 <asp:Label ID="lblt_requeired" runat="server" Text="Required" ForeColor="Red" Visible="false"></asp:Label>
                                            
                                                   <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                                      CellSpacing="0"  GridLines="None" Width="90%" AllowAutomaticUpdates="True"   AutoGenerateColumns="False">
                                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                            <WebServiceSettings>
                                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                                            </WebServiceSettings>
                                                            </HeaderContextMenu>
                                                            <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                    </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="False" >
                                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                            <HeaderStyle Width="20px"></HeaderStyle>
                                                            </RowIndicatorColumn>

                                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                            <HeaderStyle Width="20px"></HeaderStyle>
                                                            </ExpandCollapseColumn>

                                                                <Columns>                                                                                                                                         
                                                                                                                        

                                                                      <telerik:GridTemplateColumn FilterControlAltText="Filter Remove User column" 
                                                                                                            UniqueName="remove">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkRemove" runat="server" AutoPostBack="True"   
                                                                                                                    oncheckedchanged="chkRemove_CheckedChanged"  ToolTip="Remove from this role" />
                                                                                                                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                                                                    CheckedImageUrl="~/Imagenes/iconos/icon-warningAlert.png" ImageHeight="16" ImageWidth="16"  
                                                                                                                    TargetControlID="chkRemove" UncheckedImageUrl="~/Imagenes/iconos/b_drop.png">
                                                                                                                </ajaxToolkit:ToggleButtonExtender>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="10px" />
                                                                     </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn  DataField="id_rol_user" DataType="System.Int32" 
                                                                        ReadOnly="True" SortExpression="id_rol_user" UniqueName="id_rol_user" 
                                                                        Visible="true" Display="false">
                                                                    </telerik:GridBoundColumn>
                                                                    
                                                                     <telerik:GridBoundColumn  DataField="id_usuario" DataType="System.Int32" 
                                                                        ReadOnly="True" SortExpression="id_usuario" UniqueName="id_usuario" 
                                                                        Visible="true" Display="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="job" 
                                                                        FilterControlAltText="Filter job column" HeaderText="Job Tittle" 
                                                                        SortExpression="job" UniqueName="job">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="nombre_usuario" 
                                                                        FilterControlAltText="Filter nombre_usuario column" HeaderText="Employee Name" 
                                                                        SortExpression="nombre_usuario" UniqueName="nombre_usuario">
                                                                    </telerik:GridBoundColumn>

                                                                     <telerik:GridBoundColumn DataField="email_usuario" 
                                                                        FilterControlAltText="Filter  email_usuario column" HeaderText="Email" 
                                                                        SortExpression="email_usuario" UniqueName="email_usuario" Display="true">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="fecha_contrato" 
                                                                        FilterControlAltText="Filter fecha_contrato column" HeaderText="Contract Date" 
                                                                        SortExpression="fecha_contrato" UniqueName="fecha_contrato">
                                                                    </telerik:GridBoundColumn>

                                                                     <telerik:GridBoundColumn DataField="estado" 
                                                                        HeaderText="State" 
                                                                        SortExpression="estado" UniqueName="estado" Display="false">
                                                                    </telerik:GridBoundColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter state column" UniqueName="state">
                                                                          <ItemTemplate>
                                                                              <asp:ImageButton ID="state_user" runat="server" ImageUrl="~/Imagenes/iconos/drop-yes.gif" ToolTip="ACTIVE"  />
                                                                          </ItemTemplate>
                                                                          <ItemStyle Width="10px" />
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                </Columns>                                                                          

                                           
                                                           <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column" 
                                                                    UniqueName="EditCommandColumn1" ></EditColumn>
                                                            </EditFormSettings>
                                                            </MasterTableView>

                                                            <FilterMenu EnableImageSprites="False">
                                                            <WebServiceSettings>
                                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                                            </WebServiceSettings>
                                                            </FilterMenu>
                                           </telerik:RadGrid>    



                                       </div>
                                    </div>
                                  </div>
                               </div> 
              
                          </div>


                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                         <telerik:RadButton ID="btn_save" AutoPostBack="True" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Save" ValidationGroup="1" CssClass="btn btn-sm pull-left"  Width="100px" Enabled="false" ></telerik:RadButton>
                         <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 
                         <telerik:RadButton ID="btn_cancel" AutoPostBack="True" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Cancel" CssClass="btn btn-sm pull-left"  Width="100px" ></telerik:RadButton>
                       
                   </div>

                  
                  

                </div>


           </section>
    
    </asp:Content>

