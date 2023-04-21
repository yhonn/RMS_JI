<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_consultaENVIRONMENTAL_docs" Codebehind="frm_consultaENVIRONMENTAL_docs.aspx.vb"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.DynamicData" tagprefix="cc1" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
     

            <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">ENVIRONMENTAL APPROVAL</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Search Approval</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                                                                                                                                  
                               <%-- <div class="box-body">
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
                                  </div>--%>

                                 <div class="box-body">

                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                            
                                            <asp:Label ID="lbl_review_type" runat="server" CssClass="control-label text-bold" Text="Review Type"></asp:Label>
                                            <asp:Label ID="lbl_RolID" runat="server" Visible="false" ></asp:Label> 
                                            <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label> 
                                            <asp:Label ID="lbl_ALL_RolID" runat="server" Visible="false" ></asp:Label>                                            
                                            <asp:Label ID="lbl_ALL_SIMPLE_RolID" runat="server" Visible="false" ></asp:Label> 
                                                                                        
                                       </div>

                                        <div class="col-sm-8">
                                             <telerik:RadComboBox ID="cmb_rev_type" 
                                                 Runat="server"                                                  
                                                 DataSourceID="SqlDataSource1"                                      
                                                 DataTextField="nombre_tipo" 
                                                 DataValueField="id_tipoApp_Environmental"      
                                                 OnSelectedIndexChanged="cmb_rev_type_SelectedIndexChanged"                                            
                                                 AllowCustomText="true" 
                                                 EmptyMessage="Select a type..." 
                                                 Width="35%" 
                                                 AutoPostBack="true">                                     
                                            </telerik:RadComboBox>
                                             <asp:CheckBox ID="chk_allreview"  runat="server" Text="All Status" CssClass="btn btn-sm" AutoPostBack="true" />                                                   
                                        </div>
                                                                 

                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                       ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                       ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"                             
                                                       SelectCommand="select id_TipoApp_Environmental, nombre_tipo from tipo_AppAmbiental " >                                                    
                                        </asp:SqlDataSource>
                                       
                                 </div>
 
                               </div>   
                                
                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_starDate" runat="server" CssClass="control-label text-bold" Text="Date Start"></asp:Label>
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
                                            </telerik:RadDatePicker>
                                           
                                            <asp:CheckBox ID="chkfilterDate" runat="server" Text="Include Date Filter"  AutoPostBack="True" />

                                       </div>
                                    </div>
                                  </div>
                                                                  
                                
                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_EndDate" runat="server" CssClass="control-label text-bold" Text="Date End"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <telerik:RadDatePicker ID="txt_ffin" Runat="server" >
                                                <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" ></Calendar>
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
                                            <asp:Label ID="lblt_keyword" runat="server" Text="KeyWord"></asp:Label>
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
                                                                        <%# DataBinder.Eval(Container.DataItem, "observacion")%></li>
                                                                </ul>
                                                            </ItemTemplate>
                                                        <FooterTemplate>
                                                            A total of
                                                            <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                            items
                                                        </FooterTemplate>
                                                    </telerik:RadComboBox>
                                         </div><br />


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


                                                 <telerik:RadGrid ID="grd_cate" 
                                                     runat="server" 
                                                     AllowAutomaticDeletes="True" 
                                                     CellSpacing="0" 
                                                     GridLines="None" 
                                                     Skin="Office2010Blue" 
                                                     AllowAutomaticUpdates="True"  
                                                     AutoGenerateColumns="False" 
                                                     AllowSorting="True"
                                                     AllowPaging="True" 
                                                     PageSize="20" 
                                                     Width="95%" 
                                                     EnableViewState="true">

                                                        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                                    <WebServiceSettings>
                                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                                    </WebServiceSettings>
                                                                    </HeaderContextMenu>
                                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                            </ClientSettings>
                                                        
                                                     <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_documento_ambiental" >
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
     
                                                                        <telerik:GridBoundColumn  DataField="id_documento_ambiental"  FilterControlAltText="Filter id_documento column"  SortExpression="id_documento" UniqueName="id_documento_ambiental" Visible="true" Display="false">
                                                                        </telerik:GridBoundColumn>
    
                                                                         <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_approvals" Display="true" Visible="true">
                                                                           <ItemTemplate>
                                                                           <asp:HyperLink ID="aprobar" runat="server" ImageUrl="~/Imagenes/accept.png" ToolTip="approvals" Target="_blank" />
                                                                           </ItemTemplate>
                                                                           <ItemStyle Width="5px" />
                                                                       </telerik:GridTemplateColumn>
         
                                                                        <telerik:GridBoundColumn DataField="descripcion_aprobacion" 
                                                                        FilterControlAltText="Filter descripcion_aprobacion column" 
                                                                        HeaderText="Category" SortExpression="descripcion_aprobacion" 
                                                                        UniqueName="descripcion_aprobacion">
                                                                        </telerik:GridBoundColumn>     

                                                                        <telerik:GridBoundColumn DataField="numero_instrumento" 
                                                                                FilterControlAltText="Filter numero_instrumento column" 
                                                                                HeaderText="Instrument Number" SortExpression="numero_instrumento" 
                                                                                UniqueName="numero_instrumento">
                                                                        </telerik:GridBoundColumn>                                                                                                                                                                                                                            

                                                                        <telerik:GridBoundColumn DataField="nom_beneficiario" 
                                                                                FilterControlAltText="Filter nom_beneficiario column" 
                                                                                HeaderText="In Reference to" SortExpression="nom_beneficiario" 
                                                                                UniqueName="nom_beneficiario">
                                                                        </telerik:GridBoundColumn>      
 

                                                                         <telerik:GridBoundColumn DataField="estado_ambiental" 
                                                                                FilterControlAltText="Filter estado_ambiental column" 
                                                                                HeaderText="Status" SortExpression="estado_ambiental" 
                                                                                UniqueName="estado_ambiental">
                                                                        </telerik:GridBoundColumn>                                                                                                                                                  

                                                                        <telerik:GridBoundColumn DataField="tipo_revision" 
                                                                        FilterControlAltText="Filter tipo_revision column" 
                                                                        HeaderText="Review type" SortExpression="tipo_revision" 
                                                                        UniqueName="tipo_revision">
                                                                        </telerik:GridBoundColumn>
                                                                            
                                                                        <telerik:GridBoundColumn DataField="estado_ambiental" 
                                                                                FilterControlAltText="Filter estado_ambiental column" 
                                                                                HeaderText="Status" SortExpression="estado_ambiental" 
                                                                                UniqueName="estado_ambiental">
                                                                        </telerik:GridBoundColumn>
                                                                                                                                                   

                                                                          <telerik:GridBoundColumn DataField="fecha_creado" 
                                                                                FilterControlAltText="Filter fecha_creado column" 
                                                                                HeaderText="Created Date" SortExpression="fecha_creado" 
                                                                                UniqueName="fecha_creado">
                                                                        </telerik:GridBoundColumn>
                                                                                                                                                  
                                                                        <telerik:GridBoundColumn DataField="fecha_upd" 
                                                                                FilterControlAltText="Filter fecha_upd column" 
                                                                                HeaderText="Approved Date" SortExpression="fecha_upd" 
                                                                                UniqueName="fecha_upd">
                                                                        </telerik:GridBoundColumn>

                                                                         <telerik:GridBoundColumn DataField="elapsed" 
                                                                                FilterControlAltText="Filter elapsed column" 
                                                                                HeaderText="Elapsed" SortExpression="fecha_upd" 
                                                                                UniqueName="fecha_upd">
                                                                        </telerik:GridBoundColumn>
                   
                                                                <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_print" Visible="true">
                                                                  <ItemTemplate>
                                                                    <asp:HyperLink ID="Imprimir" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" ToolTip="Print details" Target="_blank" />
                                                                  </ItemTemplate>
                                                                <ItemStyle Width="5px" />
                                                               </telerik:GridTemplateColumn>
        
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter Vistadoc column" UniqueName="colm_Vistadoc" Visible="true">
                                                                  <ItemTemplate>
                                                                   <asp:Hyperlink ID="verdocFlowchart" runat="server" ToolTip="See flowchart" />
                                                                  </ItemTemplate>
                                                          <ItemStyle Width="5px" />
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

