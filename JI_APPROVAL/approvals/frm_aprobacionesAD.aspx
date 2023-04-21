<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_aprobacionesAD" Codebehind="frm_aprobacionesAD.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>



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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Approval</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">

                                   
                                               
                                           <script type="text/javascript">

                                               //OnClientSelectedIndexChanged = "getTools"
                                               //OnSelectedIndexChanged = "getToolsPOST"
                                               //AutoPostBack = "true"
                                            
                                               function getTools(sender, eventArgs) {

                                                 var comboboxType = $find("<%= cmb_category.ClientID %>");
                                                 var value = comboboxType.get_selectedItem().get_value();
                                                 var texto = comboboxType.get_selectedItem().get_text();
                                                 var comboboxOptions = $find("<%= cmb_tools.ClientID %>");

                                                 //alert('User: ' + value + ' ' + texto + ' Program: ' + <%=Me.Session("E_IDPrograma")%>);

                                                $.ajax({
                                                     type: "POST",
                                                     url: "frm_aprobacionesAD.aspx/GetTools",
                                                     data: '{idCat:"' + value + '", idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }',
                                                     contentType: "application/json; charset=utf-8",
                                                     dataType: "json",
                                                     success: function (data) {                                                       
                                                         fillCombo(comboboxOptions, data);
                                                         //comboboxOptions.highlightAllMatches(comboboxOptions.get_text());                                                                                                               

                                                     },
                                                     failure: function (response) {
                                                         alert('Error: ' + response.d);
                                                     }   
                                                 });
                                                 
                                               }


                                               // Use this for all of your RadComboBox to populate from JQuery 
                                               function fillCombo(combo, result) {

                                                   combo.clearItems();
                                                 
                                                   var items = result.d || result;
                                                   //alert('items:' + items + ' lenght:' + items.length);
                                                   data = jQuery.parseJSON(items);
                                                   //alert('items:' + data + ' lenght:' + data.length);

                                                   // This just lets your users know that nothing was returned with their search 
                                                   if (data.length == 0) {
                                                       var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                                       comboItem.set_text("No records found");
                                                       comboItem.set_value("0");
                                                       combo.get_items().add(comboItem);
                                                       combo.clearSelection();
                                                   }

                                                   
                                                   for (var i = 0; i < data.length; i++) {

                                                       var item = data[i];
                                                       //alert('item'+ i + ': ' + item.Text + ' ' + item.Value);                                                       
                                                       var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                                                       comboItem.set_text(item.Text);
                                                       comboItem.set_value(item.Value);
                                                       //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                                       //alert('Combo Text: ' + comboItem.get_text() + ' Combo Value: ' + comboItem.get_value());

                                                       //combo.trackChanges();
                                                       //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                                       combo.get_items().add(comboItem);
                                                       //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                                       //combo.commitChanges();
                                                       //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                                                   }

                                                   combo.clearSelection();

                                               }
      
                                            </script>  

                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                              <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Category"></asp:Label>
                                        </div>
                                           <div class="col-sm-4">
                                               <!--Control -->
                                               <telerik:RadComboBox ID="cmb_category" 
                                                   Runat="server" 
                                                   CausesValidation="False" 
                                                   DataSourceID="SqlDataSource3" 
                                                   DataTextField="descripcion_cat" 
                                                   DataValueField="id_categoria" 
                                                   EmptyMessage="Select a category..." 
                                                   OnClientSelectedIndexChanged = "getTools"
                                                   AllowCustomText="true" 
                                                   Filter="Contains"                                                  
                                                   Height="200px"
                                                   Width="95%">
                                               </telerik:RadComboBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cmb_category" ErrorMessage="Required" ValidationGroup="1" ForeColor="Red" ></asp:RequiredFieldValidator>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                    ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                            
                                                    SelectCommand="SELECT id_categoria, descripcion_cat, id_programa FROM ta_categoria WHERE (id_programa= @id_program)">
                                                    <SelectParameters>
                                                        <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                            SessionField="E_IDPrograma" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                             </div>
                                            
                                               <div class="col-sm-1 text-left">
                                               <!--Tittle -->                                     
                                                 <asp:Label ID="lblt_tools" runat="server"  CssClass="control-label text-bold" Text="Tool Related"></asp:Label>
                                               </div>

                                               <div class="col-sm-4">
                                                   <telerik:RadComboBox  ID="cmb_tools" 
                                                                    runat ="server" 
                                                                    CausesValidation="False"                                                                     
                                                                    EmptyMessage="Select the related tool..."   
                                                                    AllowCustomText="true"     
                                                                    Filter="Contains"                                                                                                                                                                                     
                                                                    Width="80%"> 
                                                   </telerik:RadComboBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="cmb_tools" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        
                                           </div>


                                           </div>
                                        
                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_approvalName" runat="server"  CssClass="control-label text-bold"  Text="Approval Name"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                          <telerik:RadTextBox ID="txt_aprobacion" Runat="server" EmptyMessage="Type approval name here.." Width="350px" ValidationGroup="1">                                     
                                              <ClientEvents OnValueChanging="validCondition" />
                                          </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                ErrorMessage="Required" ControlToValidate="txt_aprobacion" 
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CheckBox ID="chk_codAct" runat="server" Text="Define activity code" />
                                       </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_condition" runat="server" CssClass="control-label text-bold"   Text="Condition"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                           <telerik:RadTextBox ID="txt_condicion" Runat="server" EmptyMessage="Type condition here.." Width="500px" ValidationGroup="1">
                                                <ClientEvents OnValueChanging="validCondition" />
                                           </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_condicion" ErrorMessage="Required" ValidationGroup="1" ForeColor="Red" ></asp:RequiredFieldValidator>
                                       
                                              <script>

                                                function validCondition(sender, eventArgs) {

                                                    var str = eventArgs.get_newValue()                                                    
                                                    str = str.replace("<U", "< U");
                                                    str = str.replace("<u", "< u");
                                                    str = str.replace("U>", "U >");
                                                    str = str.replace("u>", "u >");
                                                    eventArgs.set_newValue(str);                                                                                                                           

                                                }
                                             
                                            </script>
                                       
                                       </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lblt_app_level" runat="server" CssClass="control-label text-bold"  Text="Approval Level"></asp:Label>  
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <telerik:RadTextBox ID="txt_level" Runat="server"  EmptyMessage="Type level here.." Width="500px"  ValidationGroup="1">
                                                <ClientEvents OnValueChanging="validCondition" />
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_level" ErrorMessage="Required" ValidationGroup="1" ForeColor="Red" ></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                  
                                    </div>
                               </div> 
                              
                                   <div class="box-body">
                                       <div class="col-lg-12">
                                          <div class="box-body">
                                               <div class="form-group row">
                                                   <br />
                                                       <!--Tittle -->
                                                      <asp:Label ID="lblt_doc_supp" runat="server"  CssClass="control-label text-bold" Text="Document Support"></asp:Label>

                                                   <br />
                                               </div>                                      
                                             </div>
                                       </div>
                                     </div>

                                       <div class="box-body">
                                           <div class="col-lg-12">
                                                         <div class="box-body">
                                                             <div class="row">
                                                                   
                                                                 <telerik:RadGrid ID="grd_documentos"  
                                                                               Skin="Office2010Blue"   
                                                                               runat="server"                                                                                
                                                                               CellSpacing="0" 
                                                                               DataSourceID="" 
                                                                               GridLines="None" 
                                                                               Width="85%"                                                                                
                                                                               EnableViewState="true"
                                                                               AllowPaging="True" 
                                                                               AllowSorting="True"
                                                                               PageSize="10" >
                                                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                                                    <WebServiceSettings>
                                                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                    </WebServiceSettings>
                                                                                    </HeaderContextMenu>
                                                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                            </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_doc_soporte" >
                                                                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                                                    </RowIndicatorColumn>

                                                                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                                                    </ExpandCollapseColumn>

                                                                                    <Columns>

                                                                                             <telerik:GridTemplateColumn 
                                                                                                 FilterControlAltText="Filter select column" 
                                                                                                 UniqueName="colm_select" 
                                                                                                 Visible="true"
                                                                                                 HeaderText="Sel">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                                                                                        oncheckedchanged="chkVisible_CheckedChangedDOCS" />
                                                                                                    <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                                                        CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                                                        TargetControlID="chkSelect" UncheckedImageUrl="~/Imagenes/iconos/icon-warningAlert.png">
                                                                                                    </ajaxToolkit:ToggleButtonExtender>
                                                                                                </ItemTemplate>                                                                                                
                                                                                                <ItemStyle Width="5%" />
                                                                                            </telerik:GridTemplateColumn>

                                                                                       <telerik:GridBoundColumn  DataField="id_doc_soporte" DataType="System.Int32" 
                                                                                            FilterControlAltText="Filter id_categoria column" HeaderText="id_doc_soporte" 
                                                                                            SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" 
                                                                                            Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridBoundColumn DataField="nombre_documento" 
                                                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                                                            HeaderText="Name of document" SortExpression="nombre_documento" 
                                                                                            UniqueName="descripcion_cat">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridBoundColumn DataField="extension" 
                                                                                            FilterControlAltText="Filter extension column" HeaderText="Extension" 
                                                                                            SortExpression="extension" UniqueName="extension">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                         <telerik:GridBoundColumn DataField="Template" 
                                                                                            FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                            UniqueName="Template" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                     <telerik:GridTemplateColumn 
                                                                                          FilterControlAltText="Filter colm_template column"  HeaderText="Document Template" 
                                                                                          UniqueName="colm_template" >                                      
                                                                                         <ItemTemplate>                                       
                                                                                             <asp:HyperLink ID="hlk_Template" 
                                                                                                 runat="server" 
                                                                                                 Text="--none--"                                          
                                                                                                 navigateUrl="#"></asp:HyperLink>                                       
                                                                                         </ItemTemplate>
                                                                                          <ItemStyle Width="30%" />
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
                                                                                                                                 
                                           
                                                                </div>

                                                             </div>
                                                      </div>
                                            </div> 
                                  
                                       <div class="box-body">
                                           <div class="col-lg-12">
                                                 <div class="box-body">
                                                     <div class="form-group row">
                                                         <hr />                                           
                                                     </div>                                      
                                                 </div>
                                           </div>
                                          <div class="col-lg-12">
                                          <div class="box-body">
                                               <div class="form-group row">                                                   
                                                       <!--Tittle -->
                                                      <asp:Label ID="lbl_docSelected" runat="server"  CssClass="control-label text-bold" Text="Document Support Selected"></asp:Label>
                                                   
                                               </div>                                      
                                             </div>
                                       </div>
                                     </div>
                                                                         

                              <div class="box-body">
                               <div class="col-lg-12">
                                     <div class="box-body">
                                         <div class="form-group row">
                                               <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" CellSpacing="0" DataSourceID="SqlDataSource4" GridLines="None" Width="80%"   AutoGenerateColumns="False">
                                                    <HeaderContextMenu >
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                                    </HeaderContextMenu>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                            </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_app_docs_temp" DataSourceID="SqlDataSource4">
                                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </RowIndicatorColumn>

                                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </ExpandCollapseColumn>

                                                            <Columns>
                                                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to permanently delete this record?" 
                                                                                    ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                                                                                    ConfirmDialogWidth="400px" UniqueName="Delete" ImageUrl="../Imagenes/iconos/b_drop.png" />
                                                                <telerik:GridBoundColumn  DataField="id_app_docs_temp" 
                                                                    FilterControlAltText="Filter id_tipoDocumento column" 
                                                                    SortExpression="id_app_docs_temp" UniqueName="id_app_docs_temp" 
                                                                    Visible="true" Display="false">
                                                                    <ItemStyle Width="5px" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="nombre_documento" 
                                                                    FilterControlAltText="Filter descripcion_cat column" 
                                                                    HeaderText="Name of document" SortExpression="nombre_documento" 
                                                                    UniqueName="nombre_documento">
                                                                    <ItemStyle Width="35%" />
                                                                </telerik:GridBoundColumn>

                                                                 <telerik:GridBoundColumn DataField="extension" 
                                                                    FilterControlAltText="Filter extension column" HeaderText="Type of document required" 
                                                                    SortExpression="extension" UniqueName="extension">
                                                                     <ItemStyle Width="25%" />
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter visible column" 
                                                                    UniqueName="visible" HeaderText="Repeat file?">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkVisible" runat="server" AutoPostBack="True" 
                                                                            oncheckedchanged="chkRSvisible_CheckedChanged" />
                                                                        <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server" 
                                                                            CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                            TargetControlID="chkVisible" UncheckedImageUrl="~/Imagenes/iconos/Circle_Gray.png">
                                                                        </ajaxToolkit:ToggleButtonExtender>
                                                                    </ItemTemplate>
                                                                     <ItemStyle Width="13%" />
                                                                </telerik:GridTemplateColumn>

                                                                 <telerik:GridTemplateColumn FilterControlAltText="Filter visible column" 
                                                                    UniqueName="visibleRS" HeaderText="Required at Start?">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRSvisible" runat="server" AutoPostBack="True" 
                                                                            oncheckedchanged="chkRSvisible_CheckedChanged" />
                                                                        <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender3" runat="server" 
                                                                            CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                            TargetControlID="chkRSvisible" UncheckedImageUrl="~/Imagenes/iconos/Circle_Gray.png">
                                                                        </ajaxToolkit:ToggleButtonExtender>
                                                                    </ItemTemplate>
                                                                   <ItemStyle Width="13%" />
                                                                </telerik:GridTemplateColumn>


                                                                  <telerik:GridTemplateColumn FilterControlAltText="Filter visible column" 
                                                                    UniqueName="visibleRE" HeaderText="Required at End?">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkREvisible" runat="server" AutoPostBack="True" 
                                                                            oncheckedchanged="chkREvisible_CheckedChanged" />
                                                                        <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender4" runat="server" 
                                                                            CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                            TargetControlID="chkREvisible" UncheckedImageUrl="~/Imagenes/iconos/Circle_Gray.png">
                                                                        </ajaxToolkit:ToggleButtonExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </telerik:GridTemplateColumn>

                                                                 <telerik:GridBoundColumn DataField="PermiteRepetir" 
                                                                    FilterControlAltText="Filter PermiteRepetir column" HeaderText="PermiteRepetir" 
                                                                    SortExpression="PermiteRepetir" UniqueName="PermiteRepetir" Visible="true" Display="false" >
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="RequeridoInicio" FilterControlAltText="Filter RequeridoInicio column" HeaderText="RequeridoInicio" 
                                                                    SortExpression="RequeridoInicio" UniqueName="RequeridoInicio" Visible="true" Display="false" >
                                                                </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn DataField="RequeridoFin" FilterControlAltText="Filter RequeridoFin column" HeaderText="RequeridoFin" 
                                                                    SortExpression="RequeridoFin" UniqueName="RequeridoFin" Visible="true" Display="false" >
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
                            
                                            SelectCommand="SELECT ta_aprobacion_docs_temp.id_app_docs_temp, ta_aprobacion_docs_temp.id_sesion_temp, ta_aprobacion_docs_temp.id_doc_soporte, ta_docs_soporte.nombre_documento, ta_docs_soporte.extension, ta_aprobacion_docs_temp.id_programa, ta_aprobacion_docs_temp.PermiteRepetir, ta_aprobacion_docs_temp.RequeridoInicio,ta_aprobacion_docs_temp.RequeridoFin FROM ta_aprobacion_docs_temp INNER JOIN ta_docs_soporte ON ta_aprobacion_docs_temp.id_doc_soporte = ta_docs_soporte.id_doc_soporte WHERE (ta_aprobacion_docs_temp.id_sesion_temp = @id_sesion)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lbl_id_sesion_temp" DefaultValue="-1" 
                                                    Name="id_sesion" PropertyName="Text" />
                                            </SelectParameters>
                                         </asp:SqlDataSource>
      
                     
                                         </div>  
                                     </div>
                               </div>
                              </div>

                              <div class="col-lg-12">
                                     <div class="box-body">
                                         <div class="form-group row">
                                              <asp:Label ID="lblt_file_required" runat="server" Text="**  Field required" Visible="false" ForeColor="Red"></asp:Label>
                                              &nbsp;
                                              <asp:Label ID="lbl_docsValid" runat="server" ForeColor="Red" Text="Add at least one type of document" Visible="False"  CssClass="control-label text-bold"></asp:Label>
                                         </div>                                      
                                     </div>
                               </div>
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                         <telerik:RadButton ID="btn_save" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="   Save   " ValidationGroup="1" CssClass="btn btn-sm pull-left" Width="100" Enabled="false"> </telerik:RadButton>
                         <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                         <telerik:RadButton ID="btn_cancel" runat="server"  SingleClick="true" SingleClickText="Processing..." Text=" Cancel " CssClass="btn btn-sm pull-left" Width="100"></telerik:RadButton>                            
                          &nbsp;&nbsp;
                          <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                          </telerik:RadWindowManager>
                   </div>

                </div>
           </section>
      
         
    </asp:Content>

