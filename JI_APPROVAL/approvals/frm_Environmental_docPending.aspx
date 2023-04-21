<%@ Page Language="VB" MasterPageFile="~/Site.Master"  AutoEventWireup="false"    Inherits="RMS_APPROVAL.frm_environmental_docsPending" Codebehind="frm_Environmental_docPending.aspx.vb"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    
          <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">ENVIRONMENTAL APPROVALS</asp:Label>
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
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                          <asp:Label ID="lbltt_startDate" runat="server" CssClass="control-label text-bold" Text="Start Date "></asp:Label>
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
                                                                <li class="col1">Instrument Number</li>
                                                                <li class="col2">Approvals</li>
                                                                <li class="col3">Beneficiary</li>
                                                                <li class="col4">Description</li>
                                                            </ul>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <ul>
                                                                <li class="col1">
                                                                    <%# DataBinder.Eval(Container.DataItem, "numero_instrumento")%></li>
                                                                <li class="col2">
                                                                    <%# DataBinder.Eval(Container.DataItem, "descripcion_aprobacion")%></li>
                                                                <li class="col3">
                                                                    <%# DataBinder.Eval(Container.DataItem, "nom_beneficiario")%></li>
                                                                <li class="col4">
                                                                    <%# DataBinder.Eval(Container.DataItem, "observacion")%></li>
                                                            </ul>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            Total record founded &nbps; 
                                                            <asp:Literal runat="server" ID="RadComboItemsCount" />                                                           
                                                        </FooterTemplate>
                                              </telerik:RadComboBox>                                                                                                                                    
                                       </div>
                                    </div>
                                  </div>
                                                                
                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                           <!--Control -->
                                            &nbsp;<telerik:RadButton ID="btn_buscar" runat="server" Text="Search " Width="100px" CssClass="btn btn-sm" Enabled="false">
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
                                                  <div style="text-align=left" >
                                                    <asp:Label  ID="lblt_Search" runat="server" Font-Italic="True" ForeColor="Red"  Font-Size="10px" Text="Testing"></asp:Label>
                                                  </div>
                                                    <div style="text-align= right">
                                                    <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px" ></asp:Label>
                                                     &nbsp; &nbsp;</div>
                                           </div>
                                        </div>
                                      </div>

                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-left">
                                         <!--Control -->
                                              <telerik:RadGrid 
                                                  ID="grd_cate"  
                                                  Skin="Office2010Blue"   
                                                  runat="server" 
                                                  AllowAutomaticDeletes="True" 
                                                  CellSpacing="0" 
                                                  GridLines="None" 
                                                  AllowAutomaticUpdates="True"  
                                                  AutoGenerateColumns="False" 
                                                  AllowSorting="True"
                                                  AllowPaging="True" 
                                                  PageSize="20" 
                                                  Width="95%" 
                                                  EnableViewState="true" >                                              

                                                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                <WebServiceSettings>
                                                <ODataSettings InitialContainerName=""></ODataSettings>
                                                </WebServiceSettings>
                                                </HeaderContextMenu>

                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                  <Selecting AllowRowSelect="True"></Selecting>
                                                 </ClientSettings>

                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_documento_ambiental">
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

                                                            <telerik:GridBoundColumn  DataField="id_documento_ambiental"  FilterControlAltText="Filter id_documento_ambiental column"  SortExpression="id_documento_ambiental" UniqueName="id_documento_ambiental" Visible="true" Display="false">
                                                            </telerik:GridBoundColumn>                                                                                                                          
     
                                                           <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_approvals" Visible="false">
                                                               <ItemTemplate>
                                                               <asp:HyperLink ID="aprobar" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="approvals" Target="_blank" />
                                                               </ItemTemplate>
                                                               <ItemStyle Width="5px" />
                                                           </telerik:GridTemplateColumn>

                                                          <telerik:GridBoundColumn DataField="numero_instrumento" 
                                                            FilterControlAltText="Filter numero_instrumento column" 
                                                            HeaderText="Instrument #" SortExpression="numero_instrumento" 
                                                            UniqueName="colm_numero_instrumento">
                                                            </telerik:GridBoundColumn>
                                                          
                                                           <telerik:GridBoundColumn DataField="observacion" 
                                                            FilterControlAltText="Filter observacion column" 
                                                            HeaderText="Description" SortExpression="observacion" 
                                                            UniqueName="colm_observacion">
                                                            </telerik:GridBoundColumn>                                                                                                                               
   
                                                            <telerik:GridBoundColumn DataField="descripcion_aprobacion" 
                                                             FilterControlAltText="Filter descripcion_aprobacion column" 
                                                             HeaderText="Approval Name" SortExpression="descripcion_aprobacion" 
                                                             UniqueName="colm_descripcion_doc">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="fecha_creado" 
                                                                    FilterControlAltText="Filter fecha_creado column" HeaderText="Created" 
                                                                    SortExpression="fecha_creado" 
                                                                    UniqueName="colm_fecha_creado" Visible ="true" Display="true">
                                                            </telerik:GridBoundColumn>

                                                 
                                                            <telerik:GridBoundColumn DataField="elapsed" 
                                                                    FilterControlAltText="Filter elapsed column" HeaderText="Elapsed Time (Days)" 
                                                                    SortExpression="elapsed" 
                                                                    UniqueName="colm_elapsed" Visible ="true" Display="true">
                                                            </telerik:GridBoundColumn>                                                                
        
                                                            <telerik:GridTemplateColumn FilterControlAltText="filtro colm_print column" UniqueName="colm_print" Visible="false">
                                                               <ItemTemplate>
                                                                <asp:HyperLink ID="Imprimir" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" ToolTip="Print details" Target="_blank" />
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

