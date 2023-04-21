<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_Deliverable" Codebehind="frm_Deliverable.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

              <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">DELIVERABLES</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border" >
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Deliverables</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                                          
                         <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label>   
                        <telerik:RadTextBox ID="txt_doc" Runat="server" EmptyMessage="Type a keyword here..."  Width="360px" ValidationGroup="1">
                        </telerik:RadTextBox> 
                                           
                        <telerik:RadButton ID="btn_buscar" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Search">
                        </telerik:RadButton>
                            &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btn_nuevo" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="  New Deliverable " Enabled ="false">
                        </telerik:RadButton>

                       <telerik:RadButton ID="RadButton1" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Send Email" Visible="False" ></telerik:RadButton>

                        <telerik:RadButton ID="Buttom_testing" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Testing" Visible="false">
                        </telerik:RadButton>
                    
                    </div>

                    <div class="box-body">
                          <div class="box-body">
                               <div class="col-lg-12">

                                      <telerik:RadGrid ID="grd_cate"  
                                          Skin="Office2010Blue"   
                                          runat="server" 
                                          AllowAutomaticDeletes="True" 
                                          CellSpacing="0"  
                                          GridLines ="None" 
                                          Width="100%" 
                                          AllowAutomaticUpdates="True"  
                                          AutoGenerateColumns="False">

                                                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                                </HeaderContextMenu>
                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                          
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_deliverable">
                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            </RowIndicatorColumn>

                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            </ExpandCollapseColumn>

                                             <Columns>
                                             <telerik:GridTemplateColumn FilterControlAltText="Filter edit column" UniqueName="colm_edit" Visible="false">
                                                            <ItemTemplate>
                                                              <asp:ImageButton ID="editar" runat="server" 
                                                               ImageUrl   ="~/Imagenes/iconos/b_edit.png" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn  
                                                                    DataField="id_deliverable" 
                                                                    UniqueName="id_deliverable" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn>    
                                                            <telerik:GridBoundColumn  
                                                                    DataField="id_deliverable_minute" 
                                                                    UniqueName="id_deliverable_minute" 
                                                                    Visible="true" Display="false">                                                               
                                                            </telerik:GridBoundColumn>                                                 
                                                             <telerik:GridBoundColumn  
                                                                    DataField="id_ficha_entregable" 
                                                                    UniqueName="id_ficha_entregable" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn>    
                                                             <telerik:GridBoundColumn  
                                                                    DataField="codigo_SAPME" 
                                                                    UniqueName="codigo_SAPME"                                                                   
                                                                    HeaderText ="Activity code">      
                                                               <ItemStyle Width="10%" />                                                         
                                                              </telerik:GridBoundColumn>                                                                   

                                                              <telerik:GridBoundColumn  
                                                                    DataField="numero_entregable" 
                                                                    UniqueName="colm_numero_entregable" 
                                                                    HeaderText ="#" >                                                               
                                                             </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn DataField="descripcion_entregable" 
                                                                    FilterControlAltText="Filter descripcion_entregable column" 
                                                                    HeaderText="Deliverable" SortExpression="descripcion_entregable" 
                                                                    UniqueName="colm_descripcion_entregable">
                                                                    <ItemStyle Width="20%" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="fecha" 
                                                                    FilterControlAltText="Filter fecha column" HeaderText="Due Date" 
                                                                    SortExpression="fecha" UniqueName="colm_fecha" DataFormatString="{0:d}" >                                                                    
                                                                </telerik:GridBoundColumn>        
                                                                <telerik:GridBoundColumn DataField="D_Days" 
                                                                    FilterControlAltText="Filter D_Days column" HeaderText="Delayed Days" 
                                                                    SortExpression="D_Days" UniqueName="colm_D_Days">
                                                                </telerik:GridBoundColumn>        
                                                                <telerik:GridBoundColumn DataField="valor" 
                                                                    FilterControlAltText="Filter valor column" HeaderText="Amount (UGX)" DataFormatString="{0:N2}"
                                                                    SortExpression="valor" UniqueName="colm_valor">
                                                                </telerik:GridBoundColumn>   
                                                                 <telerik:GridBoundColumn DataField="porcentaje" 
                                                                    FilterControlAltText="Filter porcentaje column" HeaderText="Percen" 
                                                                    SortExpression="porcentaje" UniqueName="colm_porcentaje" DataFormatString="{0:N2} %" >
                                                                </telerik:GridBoundColumn> 
                                                                
                                                                 <telerik:GridBoundColumn DataField="fecha_creo" 
                                                                    FilterControlAltText="Filter fecha_creo column" HeaderText="Created Date" 
                                                                    SortExpression="fecha_creo" UniqueName="colm_fecha_creo"  DataFormatString="{0:d}"  >                                                                    
                                                                </telerik:GridBoundColumn>                                                                  
                                                                <telerik:GridBoundColumn DataField="processed_days" 
                                                                   FilterControlAltText="Filter processed_days column" HeaderText="Proc days" 
                                                                   SortExpression="processed_days" UniqueName="colm_processed_days"  >                                                                    
                                                                 </telerik:GridBoundColumn>  
                                                                <telerik:GridBoundColumn DataField="createdBy" 
                                                                    FilterControlAltText="Filter createdBy column" HeaderText="Created By" 
                                                                    SortExpression="createdBy" UniqueName="colm_createdBy" >                                                                    
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn  
                                                                    DataField="id_deliverable_estado" 
                                                                    UniqueName="id_deliverable_estado" 
                                                                    Visible="true" Display="false">                                                               
                                                                </telerik:GridBoundColumn>  
                                                               <telerik:GridBoundColumn  
                                                                    DataField="id_ficha_proyecto" 
                                                                    UniqueName="id_ficha_proyecto" 
                                                                    Visible="true" Display="false">                                                               
                                                                </telerik:GridBoundColumn> 
                                                                <telerik:GridBoundColumn  
                                                                    DataField="minute_close" 
                                                                    UniqueName="colm_minute_close" 
                                                                    Visible="true" Display="false">                                                               
                                                                </telerik:GridBoundColumn>                                                                
                                                                <telerik:GridBoundColumn DataField="deliverable_estado"  HeaderText="Status" 
                                                                    FilterControlAltText="Filter deliverable_estado column" SortExpression="deliverable_estado" 
                                                                    UniqueName="colm_deliverable_estado" >
                                                                </telerik:GridBoundColumn>
                                                                 <telerik:GridTemplateColumn UniqueName="colm_open" Visible="false">
                                                                      <ItemTemplate>
                                                                           <asp:HyperLink ID="hlk_deliverable" runat="server" ImageUrl="~/Imagenes/iconos/Preview.png" 
                                                                            ToolTip="Deliverable Preview" Target="_new">
                                                                            </asp:HyperLink>
                                                                       </ItemTemplate>
                                                                    <ItemStyle Width="10px" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="colm_minute" Visible="false">
                                                                      <ItemTemplate>
                                                                           <asp:HyperLink ID="hlk_minute" runat="server" ImageUrl="~/Imagenes/iconos/additem.png" 
                                                                            ToolTip="Crear Acta" Target="_parent">
                                                                            </asp:HyperLink>
                                                                       </ItemTemplate>
                                                                    <ItemStyle Width="10px" />
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>

                                                            <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="codigo_SAPME" FieldName="codigo_SAPME" FormatString="" HeaderText="Activity" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="codigo_SAPME" />
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

