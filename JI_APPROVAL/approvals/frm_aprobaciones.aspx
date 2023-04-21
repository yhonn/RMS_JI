<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_aprobaciones" Codebehind="frm_aprobaciones.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   
      
              <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">CATALOGS</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border" >
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Approvals</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                                            
                        <telerik:RadTextBox ID="txt_doc" Runat="server" EmptyMessage="Type a keyword here..."  Width="360px" ValidationGroup="1">
                        </telerik:RadTextBox> 
                                           
                        <telerik:RadButton ID="btn_buscar" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Search">
                        </telerik:RadButton>
                            &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btn_nuevo" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="  New  " Enabled ="false">
                        </telerik:RadButton>

                       <telerik:RadButton ID="RadButton1" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Send Email" Visible="False" ></telerik:RadButton>
                    
                    </div>

                    <div class="box-body">
                          <div class="box-body">
                               <div class="col-lg-12">

                                      <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" CellSpacing="0"  DataSourceID="SqlDataSource2" 
                                        GridLines="None" Width="90%" AllowAutomaticUpdates="True"  AutoGenerateColumns="False">
                                                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                <WebServiceSettings>
                                                <ODataSettings InitialContainerName=""></ODataSettings>
                                                </WebServiceSettings>
                                                </HeaderContextMenu>
                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                          
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_tipoDocumento" DataSourceID="SqlDataSource2">
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
                                                                <telerik:GridBoundColumn  DataField="id_tipoDocumento" 
                                                                    FilterControlAltText="Filter id_tipoDocumento column" 
                                                                    SortExpression="id_tipoDocumento" UniqueName="id_tipoDocumento" 
                                                                    Visible="true" Display="false">
                                                                    <ItemStyle Width="5px" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="descripcion_aprobacion" 
                                                                    FilterControlAltText="Filter descripcion_cat column" 
                                                                    HeaderText="Name" SortExpression="descripcion_aprobacion" 
                                                                    UniqueName="descripcion_cat">
                                                                    <ItemStyle Width="400px" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="condicion" 
                                                                    FilterControlAltText="Filter extension column" HeaderText="Condition" 
                                                                    SortExpression="condicion" UniqueName="Condition">
                                                                </telerik:GridBoundColumn>
        
                                                           <telerik:GridTemplateColumn FilterControlAltText="Filter complete column" UniqueName="colm_path" Visible="false">
                                                                 <ItemTemplate>
                                                                 <asp:ImageButton ID="ruta" runat="server"   ToolTip="Logic Path" />
                                                                 </ItemTemplate>
                                                                 <ItemStyle Width="10px" />
                                                          </telerik:GridTemplateColumn>
  
                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter visible column" UniqueName="colm_visible" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkVisible" runat="server" AutoPostBack="True" 
                                                                            oncheckedchanged="chkVisible_CheckedChanged" />
                                                                        <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                            CheckedImageUrl="~/Imagenes/iconos/Stock_IndexUp.png" ImageHeight="16" ImageWidth="16" 
                                                                            TargetControlID="chkVisible" UncheckedImageUrl="~/Imagenes/iconos/Stock_IndexDown.png">
                                                                        </ajaxToolkit:ToggleButtonExtender>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10px" />
                                                                    <ItemStyle Width="10px" />
                                                                </telerik:GridTemplateColumn>
        
                                                                <telerik:GridBoundColumn DataField="visible" 
                                                                    FilterControlAltText="Filter visibleBound column" SortExpression="visible" 
                                                                    UniqueName="visibleBound" Visible="true" Display="false">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>

                                                            <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="Category" FieldName="descripcion_cat" 
                                                                            FormatString="" HeaderText="" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldAlias="id_categoria" FieldName="id_categoria" 
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

                                         <asp:SqlDataSource ID="SqlDataSource2" runat="server"  ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                 SelectCommand="SELECT id_tipoDocumento, id_categoria, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, descripcion_cat, id_programa, nombre_proyecto, nombre_actividad, nombre_cliente, visible 
			                                                     FROM vw_aprobaciones 
			                                                      WHERE (id_programa = @id_program)">
                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
             
                               </div>
                           </div>
                    </div>

    
                 </div>    
             </section>

    

    </asp:Content>

