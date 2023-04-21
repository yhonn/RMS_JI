<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_consulta_groups"  Codebehind="frm_consulta_groups.aspx.vb" %>

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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Groups</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">

                      <telerik:RadTextBox ID="txt_doc" Runat="server" EmptyMessage="Type the role group name here.." LabelWidth="" Width="360px" ValidationGroup="1">                            
                      </telerik:RadTextBox>
                      
                        <telerik:RadButton ID="btn_buscar" runat="server" Text="Search" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" Text="New Group's Role" Enabled="false"  >
                        </telerik:RadButton>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                                   
                                   <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                             CellSpacing="0"  DataSourceID="SqlDataSource2" 
                                             GridLines="None" Width="80%" AllowAutomaticUpdates="True"  
                                             AutoGenerateColumns="False">

                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                            </HeaderContextMenu>

                                            <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                                    </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_grupo" DataSourceID="SqlDataSource2">
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
                                                                        ConfirmDialogWidth="400px" UniqueName="colm_ELIMINAR"  Visible="false"
                                                        ImageUrl="../Imagenes/iconos/b_drop.png" >
                                                    <ItemStyle Width="10px" />
                                                    </telerik:GridButtonColumn>
                                                     <telerik:GridTemplateColumn  FilterControlAltText="Filter colm_edit column"  UniqueName="colm_edit" Visible="false">
                                                           <ItemTemplate>
                                                             <asp:ImageButton ID="editar" runat="server" ImageUrl="~/Imagenes/iconos/b_edit.png" ToolTip="Edit" />
                                                           </ItemTemplate>
                                                          <ItemStyle Width="10px" />
                                                      </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn  DataField="id_grupo" DataType="System.Int32" 
                                                        FilterControlAltText="Filter id_categoria column" 
                                                        ReadOnly="True" SortExpression="id_grupo" UniqueName="id_grupo" 
                                                        Visible="true" Display="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_grupo" 
                                                        FilterControlAltText="Filter descripcion_cat column" 
                                                        HeaderText="Name of group" SortExpression="nombre_grupo" 
                                                        UniqueName="nombre_grupo">
                                                        <HeaderStyle Width="175px" />
                                                        <ItemStyle Width="175px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion_grupo" 
                                                        FilterControlAltText="Filter extension column" HeaderText="Description" 
                                                        SortExpression="descripcion_grupo" UniqueName="descripcion_grupo">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="total_miembros" 
                                                        FilterControlAltText="Filter total_miembros column" HeaderText="Total Members" 
                                                        SortExpression="total_miembros" UniqueName="total_miembros">
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Width="100px" />
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

                                     <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                 ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"                                                                                            
                                                 
                                                 SelectCommand="  SELECT  a.id_grupo, a.nombre_grupo, a.descripcion_grupo, a.id_programa, 
	                                                                 case isnull (b.id_grupo,0) 
		                                                              when 0 then
		                                                                0 
		                                                              else
		                                                                count(b.id_grupo)
		                                                              end AS total_miembros 
                                                                     FROM ta_grupos a
                                                                       left join ta_gruposRoles b on (a.id_grupo = b.id_grupo)
                                                                   WHERE (a.id_programa = @id_program)
                                                                   group by b.id_grupo, a.id_grupo, a.nombre_grupo, a.descripcion_grupo, a.id_programa  ">
                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
                        
                                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple">
                                                </telerik:RadWindowManager>      


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

