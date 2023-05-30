<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_aprobaciones_ruta"  Codebehind="frm_aprobaciones_ruta.aspx.vb" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   
        
            <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVALS</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Approval Path and Participants</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                                             <asp:Label ID="lblt_Category" runat="server" CssClass="control-label text-bold" Text="Category"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <asp:Label ID="lbl_category" runat="server" CssClass="control-label text-bold" ></asp:Label>
                                       </div>
                                    </div>
                                  </div>
                                    <div class="box-body">
                                        <div class="form-group row">
                                            <div class="col-sm-2 text-left">
                                             <!--Tittle -->
                                                 <asp:Label ID="lblt_approval" runat="server" CssClass="control-label text-bold" Text=" Approval "></asp:Label>
                                            </div>
                                           <div class="col-sm-8">
                                               <!--Control -->
                                                <asp:Label ID="lbl_approval" runat="server"  CssClass="control-label text-bold" ></asp:Label>
                                           </div>
                                        </div>
                                      </div>
                                    
                                    <div class="box-body">
                                        <div class="form-group row">
                                            <div class="col-sm-2 text-left">
                                             <!--Tittle -->
                                                  <asp:Label ID="lblt_actor" runat="server"  CssClass="control-label text-bold" Text="Member Assignment"></asp:Label>
                                            </div>
                                           <div class="col-sm-8">
                                               <!--Control -->
                                       
                                                <telerik:RadComboBox ID="cmb_rol"
                                                    Runat="server" 
                                                    CausesValidation="False"                                                                                                         
                                                    DataTextField="role_n"
                                                    DataValueField="id_rol"
                                                    EmptyMessage="Select a role..."   
                                                    AllowCustomText="true" 
                                                    Filter="Contains" 
                                                    Width="350px" 
                                                    Height="200px">
                                                </telerik:RadComboBox>
                                                <asp:ImageButton ID="imgUpdateCmb" runat="server" 
                                                    ImageUrl="~/Imagenes/Iconos/updateico.png" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" 
                                                    ControlToValidate="cmb_rol" ErrorMessage="Required" 
                                                    ValidationGroup="2"></asp:RequiredFieldValidator>

                                               <asp:HiddenField ID="hd_dtTipoAPP" runat="server" Value="" />
                                               <asp:HiddenField ID="hd_dtDeliverableStage" runat="server" Value="" />
                                                        <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                            ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                       
                            
                                                            SelectCommand="SELECT id_rol, nombre_rol, descripcion_rol, id_usuario, id_programa
			                                                                  FROM ta_roles 
			                                                                    WHERE (id_programa = @id_programa)
			                                                                      and  (id_rol NOT IN (SELECT id_rol FROM ta_rutaTipoDoc WHERE (id_tipoDocumento = @id_tipoDocumento))) ">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="id_programa" SessionField="E_IDPrograma" DefaultValue="-1" />
                                                                <asp:QueryStringParameter DefaultValue="-1" Name="id_tipoDocumento" QueryStringField="IdType" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>--%>


                                           </div>
                                        </div>
                                  </div>

                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_add_step" runat="server"  CssClass="control-label text-bold"  Text="Adding Step"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->         
                                                                                      
                                            <%----%>
                                               <asp:LinkButton ID="btn_doc" runat="server" AutoPostBack="True" Text="Add Role to Approval" Width="24%" CssClass="btn btn-sm btn-default " OnClick="btn_doc_Click"  OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false" ValidationGroup="2" Enabled="false" > Add Role to Approval&nbsp;&nbsp;<span class="glyphicon glyphicon-plus-sign"></span> </asp:LinkButton> 
                                               
                                               <asp:Label ID="lblt_msj_step_error" runat="server" Text="Unabled to add a step at the Approval, the approval has an open proccess" ForeColor="Red" Visible="False"></asp:Label>

                                       </div>
                                    </div>
                                  </div>

                               

                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_email_external" runat="server" CssClass="control-label text-bold"  Text="Additional Email "></asp:Label>  
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <telerik:RadTextBox ID="txtemail" Runat="server"  ValidationGroup="2"></telerik:RadTextBox><span >&nbsp;&nbsp;&nbsp;</span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtemail" ErrorMessage="Required" 
                                                    ValidationGroup="3"></asp:RequiredFieldValidator>                       
                                              <telerik:RadButton ID="btn_add" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="Add email" ValidationGroup="3" CssClass="btn btn-sm" Enabled="false"></telerik:RadButton>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                    ControlToValidate="txtemail" ErrorMessage="Invalid Email" 
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                    ValidationGroup="2"></asp:RegularExpressionValidator>
                                                
                                                  
                                                
                                                   

                                       </div>
                                    </div>
                                  </div>

                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lbl_selectUserEmail" runat="server" CssClass="control-label text-bold"  Text="Additional Email from User List "></asp:Label>  
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadComboBox ID="cmb_emailList"
                                                                  Runat="server" 
                                                                  CausesValidation="False" 
                                                                  ValidationGroup="3"  
                                                                  DataTextField ="email_usuario" 
                                                                  EmptyMessage="Search user email..."   
                                                                  DataValueField="id_usuario" 
                                                                  AllowCustomText="true" 
                                                                  EnableVirtualScrolling="true"
                                                                  HighlightTemplatedItems ="true"
                                                                  Filter="Contains"
                                                                  Width="350px"
                                                                  Height ="200px"  >
                                                                    <HeaderTemplate>
                                                                   <ul>
                                                                        <li class="col1">User Name</li>
                                                                        <li class="col2">Email</li>                                                                        
                                                                    </ul>
                                                                </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <ul>
                                                                    <li class="col1">
                                                                        <%# DataBinder.Eval(Container.DataItem, "nombre_usuario")%></li>
                                                                    <li class="col2">
                                                                        <%# DataBinder.Eval(Container.DataItem, "email_usuario")%></li>                                                                    
                                                                </ul>
                                                            </ItemTemplate>
                                                        <FooterTemplate>                                                            
                                                        </FooterTemplate>
                                              </telerik:RadComboBox>

                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                    ControlToValidate="cmb_emailList" ErrorMessage="Required" 
                                                    ValidationGroup="4"></asp:RequiredFieldValidator>       

                                            <telerik:RadButton ID="btn_addEmail2" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="Add email" ValidationGroup="4" CssClass="btn btn-sm" Enabled="false"></telerik:RadButton>
                                                                                                                                                       
                                                
                                                   

                                       </div>
                                    </div>
                                  </div>


                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <asp:ListBox ID="lst_email" runat="server" ValidationGroup="2" Width="324px"></asp:ListBox><br />
                                           <telerik:RadButton ID="btn_remove" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Remove" ValidationGroup="17" CssClass="btn btn-sm pull-left" Enabled="false"></telerik:RadButton>
                                       </div>
                                    </div>
                                  </div>

                                  
                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                           <!--Control -->

                                                <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                                     CellSpacing="0" GridLines="None" Width="80%"  AutoGenerateColumns="False">

                                                        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                        <WebServiceSettings>
                                                        <ODataSettings InitialContainerName=""></ODataSettings>
                                                        </WebServiceSettings>
                                                        </HeaderContextMenu>

                                                        <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                                </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta" >
                                                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                                            <HeaderStyle Width="20px"></HeaderStyle>
                                                                            </RowIndicatorColumn>

                                                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                                            <HeaderStyle Width="20px"></HeaderStyle>
                                                                            </ExpandCollapseColumn>

                                                                                <Columns>
                                                                                    

                                                                                <telerik:GridButtonColumn 
                                                                                    ConfirmText="Confirm delete this element?" 
                                                                                    ConfirmDialogType="RadWindow"
                                                                                    ConfirmTitle="Delete users" 
                                                                                    ButtonType="ImageButton" 
                                                                                    CommandName="Delete" 
                                                                                    ConfirmDialogHeight="100px"                                                                                    
                                                                                    ConfirmDialogWidth="400px" 
                                                                                    UniqueName="colm_delete" 
                                                                                    ImageUrl="../Imagenes/Iconos/b_drop.png" >
                                                                                    <ItemStyle Width="10px" />
                                                                                    </telerik:GridButtonColumn>
                                                                                    <telerik:GridBoundColumn  DataField="id_ruta" 
                                                                                        FilterControlAltText="Filter id_tipoDocumento column" 
                                                                                        SortExpression="id_ruta" UniqueName="id_ruta" 
                                                                                        Visible="true" Display="false" >
                                                                                        <ItemStyle Width="5px" />
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_rol" 
                                                                                        FilterControlAltText="Filter descripcion_cat column" 
                                                                                        HeaderText="Role (Actor)" SortExpression="nombre_rol" 
                                                                                        UniqueName="nombre_rol">
                                                                                        <ItemStyle Width="250px" />
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_empleado" 
                                                                                        FilterControlAltText="Filter extension column" HeaderText="Employee" 
                                                                                        SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                                        <ItemStyle Width="300px" />
                                                                                    </telerik:GridBoundColumn>
        
                                                                                     <telerik:GridTemplateColumn FilterControlAltText="Filter Orden column" 
                                                                                        UniqueName="Duracion" HeaderText="Elapsed">
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox ID="txt_duracion" Runat="server"  
                                                                                                MaxValue="50" MinValue="0"  ValidationGroup="1" Width="70px" >
                                                                                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            </ItemTemplate>
                                                                                         <ItemStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
        
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter Orden column" 
                                                                                        UniqueName="Orden" HeaderText="Order" >
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox ID="txt_orden" Runat="server" AutoPostBack="True" 
                                                                                                 MinValue="0" ValidationGroup="1"  Width="70px" ReadOnly="true">
                                                                                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                            <asp:Image ID="imgStart" runat="server" ImageUrl="~/Imagenes/Iconos/Star-icon.png" 
                                                                                                Visible="False" ToolTip="Begin of the path" />
                                                                                            <asp:Label ID="lbl_validacion" runat="server" ForeColor="#FF3300" 
                                                                                                Visible="False"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                     <ItemStyle Width="15%" />
                                                                                    </telerik:GridTemplateColumn>
        
                                                                                    <telerik:GridBoundColumn DataField="orden" 
                                                                                        FilterControlAltText="Filter ordentxt column" SortExpression="orden" 
                                                                                        UniqueName="ordentxt" Visible="true" Display="false">
                                                                                    </telerik:GridBoundColumn>
        
                                                                                    <telerik:GridBoundColumn DataField="duracion" 
                                                                                        FilterControlAltText="Filter duracion column" SortExpression="duracion" 
                                                                                        UniqueName="duraciontxt" Visible="true" Display="false">
                                                                                    </telerik:GridBoundColumn>

                                                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Remove User column" UniqueName="colm_reOrderDW" Visible="false">
                                                                                                            <ItemTemplate>

                                                                                                                <asp:CheckBox ID="chkDown" runat="server" AutoPostBack="True" ToolTip=" Reorder to down " oncheckedchanged="chkDOwn_CheckedChanged"   />
                                                                                                                
                                                                                                                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                                                                    CheckedImageUrl="~/Imagenes/iconos/Stock_IndexUP_2.png" ImageHeight="16" ImageWidth="16"  
                                                                                                                    TargetControlID="chkDown" UncheckedImageUrl="~/Imagenes/iconos/Stock_IndexUp.png">
                                                                                                                </ajaxToolkit:ToggleButtonExtender>

                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="10px" />
                                                                                         </telerik:GridTemplateColumn>
                                                                                     
                                                                                          <telerik:GridTemplateColumn FilterControlAltText="Filter Remove User column" UniqueName="colm_reOrderUP" Visible="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkUP" runat="server" AutoPostBack="True" ToolTip=" Reorder to up " oncheckedchanged="chkUP_CheckedChanged"   />                                                                                                                
                                                                                                                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server" 
                                                                                                                    CheckedImageUrl="~/Imagenes/iconos/Stock_IndexDown.png" ImageHeight="16" ImageWidth="16"  
                                                                                                                    TargetControlID="chkUP" UncheckedImageUrl="~/Imagenes/iconos/Stock_IndexDown_2.png">                                                                                                                
                                                                                                                </ajaxToolkit:ToggleButtonExtender>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="10px" />
                                                                                         </telerik:GridTemplateColumn>

                                                                                         <telerik:GridTemplateColumn UniqueName="colm_trigger_tool" Visible="false" HeaderText="Trigger"  >
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chk_trigger" runat="server" AutoPostBack="True" ToolTip=" Activate trigger on tools " oncheckedchanged="chk_trigger_CheckedChanged"   />                                                                                                                
                                                                                                                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender_trigger" runat="server" 
                                                                                                                    CheckedImageUrl="~/Imagenes/iconos/Circle_Green.png" ImageHeight="16" ImageWidth="16"  
                                                                                                                    TargetControlID="chk_trigger" UncheckedImageUrl="~/Imagenes/iconos/Circle_Gray.png">                                                                                                                </ajaxToolkit:ToggleButtonExtender>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="10px" VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                                         </telerik:GridTemplateColumn>
                                                                                        
                                                                                        <telerik:GridBoundColumn DataField="trigger_tool" 
                                                                                            UniqueName="trigger_tool" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                       <telerik:GridTemplateColumn UniqueName="colm_app_tp" Visible="false" HeaderText="Action Type" >
                                                                                              <ItemTemplate>
                                                                                                     <telerik:RadComboBox
                                                                                                         ID="cmb_app_tp" 
                                                                                                         runat ="server">                                                                                               
                                                                                                     </telerik:RadComboBox>                                                                                                               
                                                                                              </ItemTemplate>                                                                                              
                                                                                       </telerik:GridTemplateColumn>
                                                                                         
                                                                                       <telerik:GridBoundColumn DataField="id_estadoTipo" 
                                                                                            UniqueName="id_estadoTipo" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
        

                                                                                     <telerik:GridTemplateColumn UniqueName="colm_deliverable_stage" Visible="false" HeaderText="Deliverable Stage" >
                                                                                              <ItemTemplate>
                                                                                                     <telerik:RadComboBox
                                                                                                         ID="cmb_deliv_stage" 
                                                                                                         runat ="server">                                                                                               
                                                                                                     </telerik:RadComboBox>                                                                                                               
                                                                                              </ItemTemplate>                                                                                              
                                                                                       </telerik:GridTemplateColumn>
                                                                                         
                                                                                       <telerik:GridBoundColumn DataField="id_deliverable_stage" 
                                                                                            UniqueName="id_deliverable_stage" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                      <telerik:GridBoundColumn DataField="id_approval_tool" 
                                                                                            UniqueName="id_approval_tool" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

                                                                                    <telerik:GridBoundColumn DataField="tool_code" 
                                                                                            UniqueName="tool_code" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>

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

                                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                            ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                            SelectCommand="SELECT id_rol, id_usuario, id_programa, id_ruta, id_categoria, nombre_empleado, nombre_proyecto, usuario, email, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, ruta_completa, descripcion_cat, orden, id_tipoDocumento, duracion, nombre_rol 
	                                                                         FROM vw_ta_ruta_aprobacion 
	                                                                        WHERE (id_tipoDocumento = @id_tipo_documento) 
	                                                                        ORDER BY orden" >
                                                            <SelectParameters>
                                                                <asp:QueryStringParameter DefaultValue="-1" Name="id_tipo_documento" 
                                                                    QueryStringField="IdType" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                             <br /><asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Iconos/Star-icon.png" />&nbsp;<asp:Label ID="lblt_msj_beginner" runat="server" CssClass="control-label "  Text="** The beginner of the approval"></asp:Label>  
                                         
                                       </div>
                                    </div>
                                  </div>


                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                         
                         <telerik:RadButton ID="btn_save" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="   Save   " ValidationGroup="1" CssClass="btn btn-sm pull-left" Width="100" Enabled="false" ></telerik:RadButton>
                           &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                         <telerik:RadButton ID="btn_cancel" runat="server" Text=" Cancel "  SingleClick="true" SingleClickText="Processing..." CssClass="btn btn-sm pull-left" Width="100"></telerik:RadButton>
                                &nbsp; &nbsp;
                         <asp:Label ID="lbl_msjItems" runat="server" Text="The approval process has to have at least 2 steps" ForeColor="Red" Visible="False"></asp:Label><br />
                        

                           <telerik:RadWindowManager ID="RadWindowManager1" runat="server" ></telerik:RadWindowManager>
                        
                   </div>

                </div>
           </section>
    
    </asp:Content>

