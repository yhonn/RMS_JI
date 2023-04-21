<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_groups"  Codebehind="frm_groups.aspx.vb" %>


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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Group</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lbl_idTemp" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="lblt_role_associated" runat="server" CssClass="control-label text-bold"   Text="Role associated"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadComboBox  ID="cmb_rol" 
                                                                   runat ="server" 
                                                                    CausesValidation="False"  
                                                                    DataTextField="nombre_rol" 
                                                                    DataValueField="id_rol"                                                                     
                                                                    AutoPostBack="True"
                                                                     EmptyMessage="Select a user..."   
                                                                      AllowCustomText="true" 
                                                                      Filter="Contains"                                                  
                                                                      Height="200px"
                                                                      Width="50%">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                            </telerik:RadComboBox>

                                           <%--DataSourceID="sql_rol"--%>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_rol" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>

                                            <asp:SqlDataSource ID="sql_rol" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                        
                                                SelectCommand=" select a.id_rol, b.nombre_rol, b.descripcion_rol, a.id_usuario, b.id_programa  
		                                                          from  vw_user_role_simple a
		                                                         inner join ta_roles b on (a.id_rol = b.id_rol)
                                                                 WHERE (a.id_programa = @id_program) 
		                                                         AND (a.id_rol NOT IN (SELECT DISTINCT id_rol FROM ta_gruposRoles))" >

                                                <SelectParameters>
                                                    <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" 
                                                        DefaultValue="-1" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                       </div>
                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_empleoyee_ass" runat="server"  CssClass="control-label text-bold"  Text="Employee associated"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <asp:Label ID="lblEmpleado" runat="server" ssClass="control-label text-bold"  ></asp:Label>
                                           <asp:Label ID="lbl_IdEmpleado" runat="server" Visible="False"></asp:Label>
                                       </div>
                                    </div>


                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_group_name" runat="server" CssClass="control-label text-bold" Text="Group Name"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <telerik:RadTextBox ID="txt_grupo" Runat="server"  EmptyMessage="Type group name here.." Width="250px"  ValidationGroup="1">
                                           </telerik:RadTextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txt_grupo" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>


                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lblt_description" runat="server" CssClass="control-label text-bold" Text="Description"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadTextBox ID="txt_des" Runat="server" EmptyMessage="Type description here.."  Width="70%" ValidationGroup="1" Height="75px" TextMode="MultiLine">
                                             </telerik:RadTextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_des" ErrorMessage="Required" ForeColor="Red" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                   
                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_employee_member" runat="server" CssClass="control-label text-bold" Text="Employee Member"></asp:Label>
                                        </div>
                                       <div class="col-sm-10">
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
                                                  Width="70%">
                                              </telerik:RadComboBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cmb_usu" ErrorMessage="Required"  ForeColor="Red" ValidationGroup="2"></asp:RequiredFieldValidator>
                                             <asp:ImageButton ID="imgUpd" runat="server"  ImageUrl="~/Imagenes/iconos/updateico.png" />                                                                                                                                                                                                                                                                       
                                             <asp:LinkButton ID="bntlk_add_employee" runat="server" AutoPostBack="True" Width="20%"  CssClass="btn btn-sm btn-default " ValidationGroup="2"  >Add Member <span class="glyphicon glyphicon-plus-sign"></span></asp:LinkButton> 
                                                
                                                
                                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"  ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                    SelectCommand="SELECT id_usuario, nombre_usuario as nombre_empleado, estado  FROM vw_t_usuarios WHERE (id_usuario NOT IN (SELECT id_usuario FROM ta_gruposRoles_temp WHERE id_temp = @id_temp)) AND (id_programa = @id_program) AND (id_usuario <> @id_usuario) AND (estado = 'ACTIVE' or estado = 'ACTIVO')">                                                        
                                                    <SelectParameters>
                                                            <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" 
                                                                DefaultValue="-1" />
                                                            <asp:ControlParameter ControlID="lbl_idTemp" DefaultValue="-1" Name="id_temp" 
                                                                PropertyName="Text" />
                                                            <asp:ControlParameter ControlID="lbl_IdEmpleado" DefaultValue="-1" 
                                                                Name="id_usuario" PropertyName="Text" />
                                                        </SelectParameters>
                                            </asp:SqlDataSource>
                                       </div>
                                    </div>

                                     <div class="form-group row">

                                         <div class="col-sm-2 text-left">
                                         <!--Tittle -->                                             
                                             <asp:Label ID="lblt_memebre_list" runat="server" CssClass="control-label text-bold" Text="Members List"></asp:Label>
                                        </div>

                                          

                                        <div class="col-sm-8 text-left">

                                              <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple">
                                              </telerik:RadWindowManager>

                                              
                                                <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                                 CellSpacing="0" GridLines="None" Width="80%" AutoGenerateColumns="False" AllowAutomaticUpdates="True" DataSourceID="SqlDataSource3">
                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default"></HeaderContextMenu>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                            </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource3">
                                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </RowIndicatorColumn>

                                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                        <HeaderStyle Width="20px"></HeaderStyle>
                                                        </ExpandCollapseColumn>

                                                            <Columns>
                                                            <telerik:GridButtonColumn ConfirmText="Confirm delete this element?" ConfirmDialogType="RadWindow"
                                                                                    ConfirmTitle="Delete users" ButtonType="ImageButton" 
                                                                    CommandName="Delete" ConfirmDialogHeight="100px"
                                                                                    ConfirmDialogWidth="400px" UniqueName="ELIMINAR" 
                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" >
                                                                <ItemStyle Width="10px" />
                                                                </telerik:GridButtonColumn>
                                                                <telerik:GridBoundColumn DataField="nombre_rol" 
                                                                    FilterControlAltText="Filter descripcion_cat column" 
                                                                    HeaderText="Role associated" SortExpression="nombre_rol" 
                                                                    UniqueName="nombre_rol">
                                                                    <ItemStyle Width="250px" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="nombre_empleado" 
                                                                    FilterControlAltText="Filter extension column" HeaderText="Employee" 
                                                                    SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                    <ItemStyle Width="300px" />
                                                                </telerik:GridBoundColumn>
        
                                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter Orden column" 
                                                                    UniqueName="approval" HeaderText="Approval">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_app"  runat="server" 
                                                                            oncheckedchanged="chk_app_CheckedChanged" AutoPostBack="True" />
                                                                        </ItemTemplate>
                                                                     <ItemStyle Width="15px" />
                                                                </telerik:GridTemplateColumn>

                                                                 <telerik:GridBoundColumn DataField="aprueba" FilterControlAltText="Filter aprueba column" 
                                                                    SortExpression="aprueba" UniqueName="Caprueba" Visible="true" Display="false">
                                                                </telerik:GridBoundColumn>
        
                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter Orden column" 
                                                                    UniqueName="comment" HeaderText="Comments">
                                                                    <ItemTemplate>
                                                                      <asp:CheckBox ID="chk_comment" runat="server" 
                                                                            oncheckedchanged="chk_comment_CheckedChanged" AutoPostBack="True" />    
                                                                   </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                   <telerik:GridBoundColumn DataField="comenta" FilterControlAltText="Filter comenta column" 
                                                                    SortExpression="comenta" UniqueName="Ccomenta" Visible="true" Display="false">
                                                                </telerik:GridBoundColumn>
        

                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter Orden column" 
                                                                    UniqueName="Query" HeaderText="Query">
                                                                    <ItemTemplate>
                                                                      <asp:CheckBox ID="chk_Query" runat="server" 
                                                                            oncheckedchanged="chk_Query_CheckedChanged" AutoPostBack="True" />    
                                                                   </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridBoundColumn DataField="consulta" FilterControlAltText="Filter consulta column" 
                                                                    SortExpression="consulta" UniqueName="Cconsulta" Visible="true" Display="false">
                                                                </telerik:GridBoundColumn>
        
                                                                <telerik:GridBoundColumn DataField="id" FilterControlAltText="Filter id column" 
                                                                    SortExpression="id" UniqueName="id" Visible="true" Display="false">
                                                                </telerik:GridBoundColumn>
        
                                                                <telerik:GridBoundColumn DataField="aprueba" 
                                                                    FilterControlAltText="Filter aprueba column" SortExpression="aprueba" 
                                                                    UniqueName="aprueba" Visible="False">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="comenta" 
                                                                    FilterControlAltText="Filter comenta column" SortExpression="comenta" 
                                                                    UniqueName="comenta" Visible="False">
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
                       
                                                 <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"  ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                                           
                                                         SelectCommand="SELECT a.id, id_temp, a.id_rol, a.id_usuario, a.aprueba, a.comenta, a.consulta, b.nombre_usuario + ' ' + b.apellidos_usuario  AS nombre_empleado, c.nombre_rol 
	                                                                     FROM ta_gruposRoles_temp a
		                                                                  inner join  t_usuarios b on (a.id_usuario = b.id_usuario)
		                                                                   inner join ta_roles c on (c.id_rol = a.id_rol)
		                                                                    WHERE (id_temp = @id_temp)" 
                                                         DeleteCommand="DELETE FROM ta_gruposRoles_temp WHERE id=@id">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lbl_idTemp" DefaultValue="-1" Name="id_temp" 
                                                                PropertyName="Text" />
                                                        </SelectParameters>

                                                </asp:SqlDataSource>


                                        </div>
                                     </div>


                                  </div>
                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                        <telerik:RadButton ID="btn_save" runat="server" SingleClick="true" SingleClickText="Processing..."  Text=" Save " ValidationGroup="1" CssClass="btn btn-sm pull-left" Width="100" Enabled="false"></telerik:RadButton>
                         <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <telerik:RadButton ID="btn_cancel" runat="server"   SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm pull-left" Width="100"></telerik:RadButton>
                       &nbsp;&nbsp;
                      
                   </div>

                </div>
           </section>



    
    

    </asp:Content>

