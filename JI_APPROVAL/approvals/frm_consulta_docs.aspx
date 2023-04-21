<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_consulta_docs" Codebehind="frm_consulta_docs.aspx.vb"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.DynamicData" tagprefix="cc1" %>


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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Search</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_status" runat="server" Text="Status" CssClass="control-label text-bold"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                                <telerik:RadComboBox ID="cmb_Status" 
                                                    Runat="server"                                                     
                                                    Width="150px"                                                                                                         
                                                    DataTextField="descripcion_estado" 
                                                    DataValueField="id_estadoDoc" 
                                                    AutoPostBack="True"                                                                        
                                                    HighlightTemplatedItems="true"                                                                                                                                                         
                                                    EmptyMessage="Select a status..." 
                                                    OnSelectedIndexChanged="cmb_Status_SelectedIndexChanged">                                                        
                                                    </telerik:RadComboBox>
                                                    <asp:CheckBox ID="chk_allstatus"  runat="server" Text="All Status" CssClass="btn btn-sm" AutoPostBack="true" />                                                   
                        
                        
                                       </div>
                                    </div>
                                  </div>


                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lblt_category" runat="server" CssClass="control-label text-bold"  Text="Category"></asp:Label>
                                           <asp:Label ID="lbl_RolID" runat="server" Visible="false" ></asp:Label> 
                                           <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label> 
                                           <asp:Label ID="lbl_ALL_RolID" runat="server" Visible="false" ></asp:Label>                                            
                                           <asp:Label ID="lbl_ALL_SIMPLE_RolID" runat="server" Visible="false" ></asp:Label> 
                                           <asp:HiddenField runat="server" ID="h_Filter" Value="" />                                          
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                             <telerik:RadComboBox ID="cmb_cat" 
                                               Runat="server" 
                                               AutoPostBack="True"                                                
                                               Width="350px"                                             
                                               DataTextField="descripcion_cat" 
                                               DataValueField="id_categoria"
                                               AllowCustomText="true" 
                                               HighlightTemplatedItems="true"
                                               EnableLoadOnDemand="True" 
                                               OnItemsRequested="cmb_cat_ItemsRequested"
                                               OnItemDataBound="cmb_cat_ItemDataBound" 
                                               EmptyMessage="Select a category..." 
                                               OnSelectedIndexChanged="cmb_cat_SelectedIndexChanged"> 
                                            </telerik:RadComboBox>
                                            <asp:CheckBox ID="chk_All_Categories"  runat="server" Text="All Categories" CssClass="btn btn-sm" AutoPostBack="true" />                                            
                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_approval" runat="server" Text="Approval" CssClass="control-label text-bold"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                                <telerik:RadComboBox ID="cmb_app" 
                                                    Runat="server"                                                     
                                                    Width="350px"                                                                                                         
                                                    DataTextField="descripcion_aprobacion" 
                                                    DataValueField="id_tipoDocumento" 
                                                    AutoPostBack="True"                                                                        
                                                    HighlightTemplatedItems="true"
                                                    EnableLoadOnDemand="True" 
                                                    OnItemsRequested="cmb_app_ItemsRequested"
                                                    OnItemDataBound="cmb_app_ItemDataBound" 
                                                    EmptyMessage="Select an approval..." 
                                                    OnSelectedIndexChanged="cmb_app_SelectedIndexChanged">                                                        
                                                    </telerik:RadComboBox>
                                                    <asp:CheckBox ID="chk_all_app"  runat="server" Text="All Approvals" CssClass="btn btn-sm" AutoPostBack="true" /> 
                        
                                       </div>
                                    </div>
                                  </div>
                                                                
                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_conditions" runat="server"  CssClass="control-label text-bold"   Text="Conditions"> </asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <asp:Label ID="lbl_condition" CssClass="control-label text-bold"   runat="server" ></asp:Label>
                                       </div>
                                    </div>
                                  </div>
                                                               
                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_starDate" runat="server" CssClass="control-label text-bold" Text="Received between the dates:"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <telerik:RadDatePicker ID="txt_finicio" 
                                                                    Runat="server" >
                                                <Calendar UseRowHeadersAsSelectors="False" 
                                                    UseColumnHeadersAsSelectors="False" 
                                                    ViewSelectorText="x"  >
                                                </Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                 <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                            </telerik:RadDatePicker>&nbsp;Y&nbsp;
                                             <telerik:RadDatePicker ID="txt_ffin" Runat="server" >
                                                <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" ></Calendar>
                                                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                            </telerik:RadDatePicker>                                           
                                            <asp:CheckBox ID="chkfilterDate" runat="server" Text="Incluir filtro de fecha"  AutoPostBack="True" />

                                       </div>
                                    </div>
                                  </div>

                                    <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_keyword" runat="server" Text="KeyWord" CssClass="control-label text-bold"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <div class="combobox-panel">
                                                <telerik:RadComboBox ID="rdKeyWord" 
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
                                                    OnSelectedIndexChanged="rdKeyWord_SelectedIndexChanged"
                                                    OnDataBound="rdKeyWord_DataBound" 
                                                    OnClientItemsRequested="UpdateItemCountField"
                                                    OnItemsRequested="rdKeyWord_ItemsRequested"
                                                    OnItemDataBound="rdKeyWord_ItemDataBound"
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
                                                                    A total of
                                                                    <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                                    items
                                                                </FooterTemplate>
                                                    </telerik:RadComboBox>
                                         </div><br />
                                              <asp:Label  ID="lblHidden_search" runat="server" Font-Italic="True" ForeColor="Black" Text=""></asp:Label>

                                       </div>
                                    </div>
                                  </div>

                                    <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--bottom1 -->
                                            <telerik:RadButton ID="btn_buscar" runat="server"  Text=" Search " Width="100px" Enabled="false"></telerik:RadButton>&nbsp;&nbsp;
                                        </div>
                                       <div class="col-sm-2">
                                           <!--buttom2 -->
                                           <telerik:RadButton ID="btn_return" runat="server"  Text="Ongoing Approvals"  Width="190px" Enabled="false"></telerik:RadButton>
                                       </div>
                                    </div>
                                  </div>


                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                         <!--SEARCH lABELS -->
                                            
                                           <div style="text-align=left;" >
                                                <asp:Label  ID="lblSearch" runat="server" Font-Italic="True" ForeColor="Red"  Font-Size="10px" Text="Testing"></asp:Label>
                                           </div><br /><br />

                                            <div style="text-align= right;">
                                              <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px" ></asp:Label>
                                           </div>

                                            <hr />

                                        </div>
                                       
                                    </div>
                                  </div>                                                          


                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 ">
                                         <!--GRID SEARCH -->


                                                 <telerik:RadGrid ID="grd_cate" runat="server" 
                                                     AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" 
                                                     Skin="Office2010Blue" AllowAutomaticUpdates="True"  
                                                     AutoGenerateColumns="False" AllowSorting="True"
                                                     AllowPaging="True" PageSize="20" Width="98%" EnableViewState="true">

                                                        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                                    <WebServiceSettings>
                                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                                    </WebServiceSettings>
                                                                    </HeaderContextMenu>
                                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                            </ClientSettings>
                                                        
                                                     <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_documento" >
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
                                                                              <ItemStyle Width="5px" />
                                                                            </telerik:GridBoundColumn>
     
                                                                        <telerik:GridBoundColumn  DataField="id_documento"  FilterControlAltText="Filter id_documento column"  SortExpression="id_documento" UniqueName="id_documento" Visible="true" Display="false">
                                                                        </telerik:GridBoundColumn>
    
                                                                         <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_approvals" Visible="false">
                                                                           <ItemTemplate>
                                                                           <asp:HyperLink ID="aprobar" runat="server" ImageUrl="" Target="_blank" />
                                                                           </ItemTemplate>
                                                                           <ItemStyle Width="5px" />
                                                                       </telerik:GridTemplateColumn>
    
     
                                                                        <telerik:GridBoundColumn DataField="descripcion_cat" 
                                                                        FilterControlAltText="Filter descripcion_cat column" 
                                                                        HeaderText="Tipo de aprobación" SortExpression="descripcion_cat" 
                                                                        UniqueName="descripcion_cat">
                                                                         <ItemStyle Width="10%" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="descripcion_aprobacion" 
                                                                            FilterControlAltText="Filter descripcion_aprobacion column" HeaderText="Proceso de aprobación" 
                                                                            SortExpression="descripcion_aprobacion" 
                                                                            UniqueName="descripcion_aprobacion">
                                                                            <ItemStyle Width="15%" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="numero_instrumento" 
                                                                                FilterControlAltText="Filter descripcion_cat column" 
                                                                                HeaderText="Código" SortExpression="numero_instrumento" 
                                                                                UniqueName="numero_instrumento">
                                                                                   <ItemStyle Width="12%" />
                                                                        </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="descripcion_doc" 
                                                                                FilterControlAltText="Filter descripcion_aprobacion column" 
                                                                                HeaderText="Proceso" SortExpression="descripcion_doc" 
                                                                                UniqueName="descripcion_doc">                                                                               
                                                                            </telerik:GridBoundColumn>
                                                                              <telerik:GridBoundColumn DataField="descripcion_estado" 
                                                                                FilterControlAltText="Filter descripcion_aprobacion column" 
                                                                                HeaderText="Estado" SortExpression="descripcion_estado" 
                                                                                UniqueName="colm_descripcion_doc">
                                                                               <ItemStyle Width="10%" />
                                                                            </telerik:GridBoundColumn>        
                                                                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_print" Visible="false">
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
        
                                                     <telerik:GridTemplateColumn UniqueName="colm_Tracking" Visible="false" >
                                                      <ItemTemplate>
                                                     <asp:HyperLink ID="Tracking" runat="server" ImageUrl="~/Imagenes/iconos/ico_table.png"
                                                     ToolTip="Tracking">
                                                     </asp:HyperLink>
                                                     </ItemTemplate>
                                                         <ItemStyle Width="5px" />
                                                    </telerik:GridTemplateColumn>
        
                                                     <telerik:GridTemplateColumn FilterControlAltText="Filter estado column" 
                                                        UniqueName="estado">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgStatus" runat="server" />
                                                        </ItemTemplate>
                                                         <ItemStyle Width="5px" />
                                                    </telerik:GridTemplateColumn>
        
                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter status column" 
                                                        UniqueName="completo">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgCompleto" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5px" />
                                                    </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="id_ruta" 
                                                FilterControlAltText="Filter descripcion_aprobacion column" HeaderText="Ruta" 
                                                SortExpression="id_ruta" UniqueName="id_ruta" Visible ="true" Display="false"> 
                                            </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn  DataField="idUserOwner"  FilterControlAltText="Filter idUserOwner column"  SortExpression="idUserOwner" UniqueName="idUserOwner" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn  DataField="id_estadoDoc"  FilterControlAltText="Filter id_estadoDoc column"  SortExpression="id_estadoDoc" UniqueName="id_estadoDoc" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn  DataField="idOriginador"  FilterControlAltText="Filter idOriginador column"  SortExpression="idOriginador" UniqueName="idOriginador" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn  DataField="Alerta"   SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn  DataField="descripcion_estado"   SortExpression="descripcion_estado" UniqueName="descripcion_estado" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn  DataField="propietario"   SortExpression="propietario" UniqueName="propietario" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="icon_msj"   SortExpression="icon_msj" UniqueName="icon_msj" Visible="true" Display="false">
                                                </telerik:GridBoundColumn>

                                         <telerik:GridBoundColumn DataField="rol_owner" 
                                         FilterControlAltText="Filter rol_owner column" 
                                         HeaderText="rol_owner" SortExpression="rol_owner" 
                                         UniqueName="rol_owner" Visible ="true" Display="false">
                                         </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn  DataField="idRolOriginator"  FilterControlAltText="Filter idRolOriginator column"  SortExpression="idRolOriginator" UniqueName="idRolOriginator" Visible="true" Display="false">
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
                                                 SelectCommand="SELECT * FROM vw_ta_documentos WHERE (id_proyecto = @id_proy) AND (id_categoria = @id_categoria) AND (id_tipoDocumento = @id_tipoDocumento)" 
                                                 DeleteCommand="DELETE FROM ta_documento WHERE id_documento=@id_doc">
                                                 <SelectParameters>
                                                     <asp:SessionParameter Name="id_proy" SessionField="E_IdProy" />
                                                     <asp:ControlParameter ControlID="cmb_cat" DefaultValue="-1" Name="id_categoria" 
                                                         PropertyName="SelectedValue" />
                                                     <asp:ControlParameter ControlID="cmb_app" DefaultValue="-1" 
                                                         Name="id_tipoDocumento" PropertyName="SelectedValue" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>


                                        
                                            <!--GRID SEARCH -->
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

