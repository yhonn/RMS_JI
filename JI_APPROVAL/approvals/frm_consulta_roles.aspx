<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_consulta_roles" Codebehind="frm_consulta_roles.aspx.vb" %>


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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Roles</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                      <telerik:RadTextBox ID="txt_doc" Runat="server" 
                            EmptyMessage="Type Role name here.." LabelWidth="" Width="360px" ValidationGroup="1">
                            <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                      </telerik:RadTextBox>
                      
                        <telerik:RadButton ID="btn_buscar" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="Search"  >
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="New Role" Enabled="false"  >
                        </telerik:RadButton>

                        <telerik:RadComboBox ID="cmb_type_role" Runat="server" CausesValidation="False" DataSourceID="SqlDataSource1" Enabled="false" 
                             DataTextField="name_type_role" DataValueField="id_type_role"  Width="15%">                                     
                        </telerik:RadComboBox>


                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                                   <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                      CellSpacing="0" DataSourceID="SqlDataSource2" 
                                       GridLines="None" Width="95%" AllowAutomaticUpdates="True"  
                                              AutoGenerateColumns="False">
                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                            <WebServiceSettings>
                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                            </WebServiceSettings>
                                            </HeaderContextMenu>
                                            <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                                    </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_rol, id_type_role" DataSourceID="SqlDataSource2">
                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            </RowIndicatorColumn>

                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            </ExpandCollapseColumn>

                                                <Columns>
                                                     
                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter edit column" 
                                                                                            UniqueName="colm_edit" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="editar" runat="server"   
                                                                                                    ImageUrl="~/Imagenes/iconos/b_edit.png" ToolTip="Edit" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="10px" />
                                                                                        </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn  DataField="id_rol" DataType="System.Int32" 
                                                        FilterControlAltText="Filter id_categoria column" 
                                                        ReadOnly="True" SortExpression="id_rol" UniqueName="id_rol" 
                                                        Visible="true" Display="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_rol" 
                                                        FilterControlAltText="Filter descripcion_cat column" 
                                                        HeaderText="Name of role" SortExpression="nombre_rol" 
                                                        UniqueName="nombre_rol">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_rol" 
                                                        FilterControlAltText="Filter extension column" HeaderText="Description" 
                                                        SortExpression="descripcion_rol" UniqueName="descripcion_rol">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_empleado" 
                                                        FilterControlAltText="Filter nombre_empleado column" HeaderText="Employee" 
                                                        SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                    </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn DataField="id_type_role" 
                                                        FilterControlAltText="Filter id_type_role column" HeaderText="id_type_role" 
                                                        SortExpression="id_type_role" UniqueName="id_type_role" Display="false">
                                                    </telerik:GridBoundColumn>
                                                </Columns>

                                                 <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="Type" FieldName="Tipo_rol" 
                                                                            FormatString="" HeaderText="" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldAlias="id_type_role" FieldName="id_type_role" 
                                                                            FormatString="" HeaderText="" />
                                                                    </GroupByFields>
                                                                </telerik:GridGroupByExpression>
                                                            </GroupByExpressions>

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


                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                 ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                 
                                                 SelectCommand="SELECT tipo_rol, id_type_role, id_rol, nombre_rol, descripcion_rol, id_usuario, nombre_empleado, email, usuario, codigo_SAP, nombre_proyecto, id_programa FROM vw_ta_roles_emplead WHERE (id_programa = @id_program)">
                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>

                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                           ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                           ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"                             
                                           SelectCommand="select id_type_role, name_type_role from ta_type_role" >                                                    
                                    </asp:SqlDataSource>



                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                   </div>

                </div>
           </section>

    </asp:Content>

