<%@ Page Language="VB" MasterPageFile="~/Site.Master"  AutoEventWireup="false"    Inherits="RMS_APPROVAL.frm_consulta_docsPending" Codebehind="frm_consulta_docsPending.aspx.vb"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    
          <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVALS PROCESSES</asp:Label>
                </h1>
          </section>
          <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Ongoing Approvals</asp:Label>
                        </h3>
                    </div>

                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class=" row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_groupBy" runat="server" CssClass="control-label text-bold"  Text="Group by"></asp:Label> 
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <asp:Label ID="lbl_RolID" runat="server" Visible="false" ></asp:Label> 
                                           <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label> 
                                           <asp:Label ID="lbl_ALL_RolID" runat="server" Visible="false" ></asp:Label>                                            
                                           <asp:Label ID="lbl_ALL_SIMPLE_RolID" runat="server" Visible="false" ></asp:Label> 
                                            <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" Checked="True" GroupName="GrdPending"  Text="   Pendiente por "  CssClass="labelRadiobutton"  /> <br />
                                            <asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   Aprobaciones pendientes creadas por " CssClass="labelRadiobutton" /><br />
                                            <asp:RadioButton ID="RadioButton3" runat="server" AutoPostBack="True" GroupName="GrdPending" Text= "   Aprobaciones pendientes en donde {0} participa" CssClass="labelRadiobutton" /><br />
                                            <asp:RadioButton ID="rdb_allpending" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   Ver todos los procesos de aprobación pendientes"  CssClass="labelRadiobutton" Visible ="False"   />
                                       </div>
                                    </div>
                                  </div>
                                                               

                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lbltt_startDate" runat="server" CssClass="control-label text-bold" Text="Fecha de inicio "></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadDatePicker ID="txt_finicio" Runat="server" >
                                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" ></Calendar>
                                                     <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                      <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                            </telerik:RadDatePicker>
                                            
                                           <asp:CheckBox ID="chkfilterDate" runat="server" Text="Include date filter" AutoPostBack="True" />

                                       </div>
                                    </div>
                                  </div>

                                  
                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_endDate" CssClass="control-label text-bold" runat="server"  Text="End Date"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                                <telerik:RadDatePicker ID="txt_ffin" Runat="server" >
                                                  
                                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"  ></Calendar>
                                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                        <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>

                                                </telerik:RadDatePicker>
                                       </div>
                                    </div>
                                  </div>


                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                               <asp:Label ID="lblt_keyword" runat="server"  CssClass="control-label text-bold" Text="KeyWord"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            
                                              <telerik:RadComboBox ID="RadComboBox3" 
                                                        AllowCustomText="true" 
                                                        runat="server" 
                                                        Width="90%"
                                                        Height="350px"  
                                                        MarkFirstMatch="true"
                                                        AutoPostBack="true"  
                                                        HighlightTemplatedItems="true"
                                                        EnableLoadOnDemand="True" 
                                                        ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true" 
                                                        EmptyMessage="Search using a key word..." 
                                                        OnSelectedIndexChanged="RadComboBox3_SelectedIndexChanged"
                                                        OnDataBound="RadComboBox3_DataBound" 
                                                        OnClientItemsRequested="UpdateItemCountField"
                                                        OnItemsRequested="RadComboBox3_ItemsRequested"
                                                        OnItemDataBound="RadComboBox3_ItemDataBound"
                                                         Filter="Contains"   >
                                                        <HeaderTemplate>
                                                            <ul>
                                                                <li class="col1">Beneficiary Name</li>
                                                                <li class="col2">Instrument Number</li>
                                                                <li class="col3">Approval Description</li>
                                                            </ul>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <ul>
                                                                <li class="col1">
                                                                    <%# DataBinder.Eval(Container.DataItem, "nom_beneficiario")%></li>
                                                                <li class="col2">
                                                                    <%# DataBinder.Eval(Container.DataItem, "numero_instrumento")%></li>
                                                                <li class="col3">
                                                                    <%# DataBinder.Eval(Container.DataItem, "descripcion_doc")%></li>
                                                            </ul>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            Total record founded &nbps; 
                                                            <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                           
                                                        </FooterTemplate>
                                              </telerik:RadComboBox>
                                             <br />
                                                                                       
                                       </div>
                                    </div>
                                  </div>

                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                           <!--Control -->
                                            <hr />
                                       </div>
                                    </div>
                                  </div>
                                
                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                           <!--Control -->

                                            &nbsp;<telerik:RadButton ID="btn_buscar" runat="server" Text="Search " Width="100px" CssClass="btn btn-sm">
                                                  </telerik:RadButton>

                                                   <telerik:RadButton ID="btn_buscar0" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="Quick search " Width="100px" Visible="False" CssClass="btn btn-sm">
                                                   </telerik:RadButton>

                                                    <telerik:RadButton ID="btn_buscar1" runat="server"   SingleClick="true" SingleClickText="Processing..." Text="View all approved" Width="100px" Visible="False" CssClass="btn btn-sm">
                                                    </telerik:RadButton>

                                                    <telerik:RadButton ID="btn_nuevo2" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Group by status" Width="100px" CssClass="btn btn-sm" Visible="false">
                                                    </telerik:RadButton>

                                                    <telerik:RadButton ID="btn_nuevo0" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="Group by type" Width="100px" CssClass="btn btn-sm" Visible="false">
                                                    </telerik:RadButton>
 
                                                    <telerik:RadButton ID="btn_nuevo" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="New Approval Process" Width="150px" CssClass="btn btn-sm" Enabled="false" Visible="false" >
                                                    </telerik:RadButton>
                                        
                                       </div>
                                    </div>
                                  </div>

                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                           <!--Control -->
                                            <hr />
                                       </div>
                                    </div>
                                  </div>


                                    <div class="box-body">
                                        <div class="form-group row">
                                            <div class="col-sm12 text-left">
                                              <!--Control -->
                                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                     SelectCommand="SELECT * FROM vw_ta_documentos WHERE (id_programa = @id_programa" 
                                                     DeleteCommand="DELETE FROM ta_documento WHERE id_documento=@id_doc" 
                                                     ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>">
                                                     <SelectParameters>
                                                         <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>

                                                  <div style="text-align=left;" >
                                                    <asp:Label  ID="lblt_Search" runat="server" Font-Italic="True" ForeColor="Red"  Font-Size="10px" Text="--"></asp:Label>
                                                  </div>

                                                    <div style="text-align= right;">
                                                    <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px" ></asp:Label>
                                                     &nbsp; &nbsp;</div>

                                           </div>
                                        </div>
                                      </div>

                                     <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                         <!--Control -->
                                              <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                             CellSpacing="0" GridLines="None" AllowAutomaticUpdates="True"  AutoGenerateColumns="False" AllowSorting="True"
                                             AllowPaging="True" PageSize="20" Width="98%" EnableViewState="true" >                                              

                                                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                <WebServiceSettings>
                                                <ODataSettings InitialContainerName=""></ODataSettings>
                                                </WebServiceSettings>
                                                </HeaderContextMenu>

                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                  <Selecting AllowRowSelect="True"></Selecting>
                                                 </ClientSettings>

                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_documento">
                                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                        <HeaderStyle Width="20px"></HeaderStyle>
                                                        </RowIndicatorColumn>

                                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                        <HeaderStyle Width="20px"></HeaderStyle>
                                                        </ExpandCollapseColumn>

                                                            <Columns>
  
                                                           <telerik:GridBoundColumn  DataField="No" 
                                                              FilterControlAltText="Filter No column"  
                                                              SortExpression="No" UniqueName="No" 
                                                              Visible="true"  
                                                              HeaderText="No">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn  DataField="id_documento"  FilterControlAltText="Filter id_documento column"  SortExpression="id_documento" UniqueName="id_documento" Visible="true" Display="false">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn  DataField="idRolOriginator"  FilterControlAltText="Filter idRolOriginator column"  SortExpression="idRolOriginator" UniqueName="idRolOriginator" Visible="true" Display="false">
                                                            </telerik:GridBoundColumn>

     
                                                             <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_approvals" Visible="false">
                                                               <ItemTemplate>
                                                               <asp:HyperLink ID="aprobar" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="approvals" Target="_blank" />
                                                               </ItemTemplate>
                                                               <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>
    
     
                                                            <telerik:GridBoundColumn DataField="descripcion_cat" 
                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                            HeaderText="Tipo de aprobación" SortExpression="descripcion_cat" 
                                                            UniqueName="descripcion_cat">
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="descripcion_aprobacion" 
                                                                    FilterControlAltText="Filter descripcion_aprobacion column" HeaderText="Proceso de aprobación" 
                                                                    SortExpression="descripcion_aprobacion" 
                                                                    UniqueName="descripcion_aprobacion">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="numero_instrumento" 
                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                            HeaderText="Código" SortExpression="numero_instrumento" 
                                                            UniqueName="numero_instrumento">
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="descripcion_doc" 
                                                                    FilterControlAltText="Filter descripcion_aprobacion column" 
                                                                    HeaderText="Proceso" SortExpression="descripcion_doc" 
                                                                    UniqueName="descripcion_doc">
                                                                </telerik:GridBoundColumn>
        
                                                                <telerik:GridTemplateColumn FilterControlAltText="filtro colm_print column" UniqueName="colm_print" Visible="false">
                                                                <ItemTemplate>
                                                               <asp:HyperLink ID="Imprimir" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" ToolTip="Print details" Target="_blank" />
                                                             </ItemTemplate>
                                                           <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>
        
                                                           <telerik:GridTemplateColumn FilterControlAltText="Filter Vistadoc column" UniqueName="colm_Vistadoc" Visible="false">
                                                              <ItemTemplate>
                                                               <asp:Hyperlink ID="verdocFlowchart" runat="server" ToolTip="See flowchart" />
                                                              </ItemTemplate>
                                                              <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>
   
                                                           <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_comentarios" Visible="false">
                                                              <ItemTemplate>
                                                              <asp:Hyperlink ID="historial" runat="server" ToolTip="Comments of document" />
                                                              </ItemTemplate>
                                                              <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>       

                                                         <%--    <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_resend" Visible="false">
                                                              <ItemTemplate>
                                                              <asp:Hyperlink ID="Resend_" runat="server" ToolTip="Resend the notification" />
                                                              </ItemTemplate>
                                                              <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>--%>       

                                                                 <telerik:GridTemplateColumn UniqueName="colm_Tracking" Visible="false" Display="false">
                                                                  <ItemTemplate>
                                                                 <asp:HyperLink ID="Tracking" runat="server" ImageUrl="~/Imagenes/iconos/ico_table.png"
                                                                 ToolTip="Tracking">
                                                                 </asp:HyperLink>
                                                                 </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
        
                                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter estado column" 
                                                                    UniqueName="estado">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgStatus" runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
        
                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter status column" 
                                                                    UniqueName="completo">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgCompleto" runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="id_ruta" 
                                                                    FilterControlAltText="Filter descripcion_aprobacion column" HeaderText="Ruta" 
                                                                    SortExpression="id_ruta" 
                                                                    UniqueName="id_ruta" Visible ="true" Display="false">
                                                                </telerik:GridBoundColumn>
        
                                                                 <telerik:GridBoundColumn DataField="icon_msj" 
                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                            HeaderText="icon_msj" SortExpression="icon_msj" 
                                                            UniqueName="icon_msj" Visible ="true" Display="false">
                                                            </telerik:GridBoundColumn>
    
                                                            <telerik:GridBoundColumn DataField="descripcion_estado" 
                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                            HeaderText="descripcion_estado" SortExpression="descripcion_estado" 
                                                            UniqueName="descripcion_estado" Visible ="true" Display="false">
                                                            </telerik:GridBoundColumn>

                                                                
                                                            <telerik:GridBoundColumn DataField="rol_owner" 
                                                            FilterControlAltText="Filter rol_owner column" 
                                                            HeaderText="rol_owner" SortExpression="rol_owner" 
                                                            UniqueName="rol_owner" Visible ="true" Display="false">
                                                            </telerik:GridBoundColumn>
                                                                                                                                                                                                
    
                                                            <telerik:GridBoundColumn DataField="propietario" 
                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                            HeaderText="propietario" SortExpression="propietario" 
                                                            UniqueName="propietario" Visible ="true" Display="false">
                                                                </telerik:GridBoundColumn>
        
                                                            <telerik:GridBoundColumn  DataField="idUserOwner"  FilterControlAltText="Filter idUserOwner column"  SortExpression="idUserOwner" UniqueName="idUserOwner" Visible="true" Display="false">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  DataField="id_estadoDoc"  FilterControlAltText="Filter id_estadoDoc column"  SortExpression="id_estadoDoc" UniqueName="id_estadoDoc" Visible="true" Display="false">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  DataField="idOriginador"  FilterControlAltText="Filter idOriginador column"  SortExpression="idOriginador" UniqueName="idOriginador" Visible="true"  Display="false">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  DataField="Alerta"   SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">
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

                                               <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple">
                                                </telerik:RadWindowManager>


                                       </div>
                                    </div>
                                  </div>




                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                   </div>

                </div>
           </section>
       
    
                <script type="text/javascript">
                    function UpdateItemCountField(sender, args) {
                        //set the footer text
                        sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
                    }
                </script>        
       
     
    </asp:Content>

