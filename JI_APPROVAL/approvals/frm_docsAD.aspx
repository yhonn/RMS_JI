<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"    Inherits="RMS_APPROVAL.frm_docsAD" Codebehind="frm_docsAD.aspx.vb"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Approval Process</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row" style="display:none;">
                                        <div class="col-sm-4 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_DocumentCODE" runat="server"  Text="Document Code"  CssClass="control-label text-bold"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <asp:Label ID="lbl_idDocumento" runat="server" Font-Bold="True" Font-Size="14px" Font-Italic="True" Font-Underline="False"></asp:Label>
                                       </div>
                                    </div>
                                  </div>                                    
                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                             <asp:Label ID="lblt_rolBegin" runat="server"  CssClass="control-label text-bold" Text="Approval Beginner<br />(Logged User)" Visible="False"></asp:Label>
                                            
                                             <asp:Label ID="lblt_rol_user" runat="server"  CssClass="control-label text-bold" Text="Originador"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                              <telerik:RadComboBox ID="cmb_rol" Runat="server" Width="400px" DataTextField="nombre_rol" 
                                                                   DataValueField="id_rol" AutoPostBack="True" Visible="False"          >
                                            </telerik:RadComboBox>

                                             <asp:SqlDataSource ID="sql_rol" runat="server" 
                                                 ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"                             
                                                    SelectCommand="SELECT id_rol, nombre_rol, descripcion_rol, id_usuario, usuario, id_proyecto FROM vw_ta_roles_emplead WHERE (id_programa = @id_program) ORDER BY 2 ">
                                                 <SelectParameters>
                                                     <asp:SessionParameter Name="id_program" SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
                        
                        
                                       </div>
                                    </div>
                                  </div>

                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                               <asp:Label ID="lblt_category" CssClass="control-label text-bold" runat="server"  Text="Category"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->

                                                <telerik:RadComboBox ID="cmb_cat" Runat="server" AutoPostBack="True" 
                                                    Width="400px"  DataTextField="descripcion_cat" 
                                                    DataValueField="id_categoria">
                                                    <WebServiceSettings>
                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                    </WebServiceSettings>
                                                 </telerik:RadComboBox>
                                           
                                                   <asp:SqlDataSource ID="sql_cat" runat="server" 
                                                     ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                     SelectCommand="SELECT DISTINCT ta_categoria.id_categoria, ta_categoria.descripcion_cat, ta_roles.id_usuario, ta_rutaTipoDoc.orden FROM ta_categoria INNER JOIN ta_tipoDocumento ON ta_categoria.id_categoria = ta_tipoDocumento.id_categoria INNER JOIN ta_rutaTipoDoc ON ta_tipoDocumento.id_tipoDocumento = ta_rutaTipoDoc.id_tipoDocumento INNER JOIN ta_roles ON ta_rutaTipoDoc.id_rol = ta_roles.id_rol WHERE (ta_categoria.id_programa = @id_program) AND (ta_roles.id_usuario = @id_usuario) AND (ta_rutaTipoDoc.orden = 0) AND ( ta_categoria.visible='SI')">
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                             SessionField="E_IDPrograma" />
                                                         <asp:ControlParameter ControlID="cmb_rol" Name="id_usuario" 
                                                             PropertyName="SelectedValue" />
                                                     </SelectParameters>
                                                 </asp:SqlDataSource>
                                                
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_cat" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                        
                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_approval" runat="server" CssClass="control-label text-bold"  Text="Approvals"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                                <telerik:RadComboBox ID="cmb_app" Runat="server"  Width="400px" 
                                                   DataTextField="descripcion_aprobacion" DataValueField="id_tipoDocumento" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                             &nbsp;
                                               <asp:HyperLink ID="hlnk_import" runat="server" NavigateUrl="~/Approvals/frm_consultaContratos.aspx" Visible="False">Import</asp:HyperLink>
                                                                                         
                                                <asp:SqlDataSource ID="sql_app" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                  SelectCommand="SELECT DISTINCT ta_tipoDocumento.id_tipoDocumento, ta_tipoDocumento.descripcion_aprobacion, ta_rutaTipoDoc.orden, ta_tipoDocumento.visible FROM ta_tipoDocumento INNER JOIN ta_rutaTipoDoc ON ta_tipoDocumento.id_tipoDocumento = ta_rutaTipoDoc.id_tipoDocumento INNER JOIN ta_roles ON ta_rutaTipoDoc.id_rol = ta_roles.id_rol WHERE (ta_tipoDocumento.id_categoria = @id_categoria) AND (ta_roles.id_usuario = @id_usuario) AND (ta_rutaTipoDoc.orden = 0) AND (ta_tipoDocumento.visible = 'SI')">
                                                 
                                                    <SelectParameters>
                                                     <asp:ControlParameter ControlID="cmb_cat" DefaultValue="" Name="id_categoria" 
                                                         PropertyName="SelectedValue" />
                                                     <asp:ControlParameter ControlID="cmb_rol" Name="id_usuario" 
                                                         PropertyName="SelectedValue" />
                                                 </SelectParameters>

                                                </asp:SqlDataSource>                        
                        
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                                    ControlToValidate="cmb_app" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>

                                       </div>
                                    </div>
                                  </div>

                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_condition" CssClass="control-label text-bold"  runat="server"  Text="Condition"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                              <asp:Label ID="lbl_condition" runat="server" CssClass="control-label text-bold" ></asp:Label>
                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_documentType" runat="server" CssClass="control-label text-bold" Text="Document type"></asp:Label>
                                                           
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                              <telerik:RadComboBox ID="cmb_tipoDocumento" Runat="server" Width="400px"  DataTextField="nombreTipoAprobacion" DataValueField="id_tipoAprobacion">
                                               </telerik:RadComboBox>
                                             
                                           <asp:SqlDataSource ID="Sql_tipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" SelectCommand="SELECT [id_tipoAprobacion], [nombreTipoAprobacion] FROM [ta_tipo_aprobacion]">
                                            </asp:SqlDataSource>

                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                               <asp:Label ID="lblt_nameProcess" runat="server" CssClass="control-label text-bold" Text="Name of Process"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                             <telerik:RadTextBox ID="txt_Doc" Runat="server" Height="65px" TextMode="MultiLine" Width="85%">
                                             </telerik:RadTextBox>

                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_Doc" ErrorMessage="Required" ForeColor="Red" ValidationGroup="1"></asp:RequiredFieldValidator>

                                       </div>
                                    </div>
                                  </div>

                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_codeProcess" runat="server" CssClass="control-label text-bold" Text="Sequence"></asp:Label>                                            
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                           <telerik:RadTextBox ID="txt_codigoAID" Runat="server" Width="400px"></telerik:RadTextBox>
                                       </div>
                                    </div>
                                  </div>

                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_region" runat="server" Text="Region" CssClass="control-label text-bold"></asp:Label> 
                                        </div>
                                       <div class="col-sm-9">
                                           
                                           <!--Control -->
                                            <telerik:RadComboBox ID="cmb_region" Runat="server" DataSourceID="sql_region" DataTextField="nombre_region" 
                                                DataValueField="id_region" EmptyMessage="Type region here" ShowToggleImage="False" Width="400px"> 
                                            </telerik:RadComboBox>
                                             
                                           <asp:Label ID="lblerrRegion" runat="server" ForeColor="Red" Text="Required" Visible="False"></asp:Label>
                                             <asp:SqlDataSource ID="sql_region" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                              
                                                 SelectCommand ="SELECT id_region, nombre_region FROM t_regiones WHERE (id_programa = @id_program)">

                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>


                                       </div>
                                    </div>
                                  </div>


                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                                <asp:Label ID="lblt_exchangeRate" runat="server" CssClass="control-label text-bold" Text="Exchange rate"></asp:Label>
                                         </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                           <telerik:RadNumericTextBox ID="txt_tasacambio" Runat="server" MinValue="0" Width="150px" >
                                                <NumberFormat ZeroPattern="n" DecimalSeparator="."></NumberFormat>                                                                                           
                                                <ClientEvents OnValueChanging="calc_mountEX" />
                                            </telerik:RadNumericTextBox>
                                            &nbsp;<asp:Label ID="Label22" runat="server"  CssClass="control-label text-bold"  Text="= USD $ 1.00"></asp:Label>
                                            &nbsp;<asp:Label ID="lblerrMontoER" runat="server" ForeColor="Red" Text="Required Or more than 0" Visible="False"></asp:Label>
                                       </div>
                                    </div>
                                  </div>
                                                                
                                              <script>

                                                  /**
                                                     * Number.prototype.format(n, x, s, c)
                                                     * 
                                                     * @param integer n: length of decimal
                                                     * @param integer x: length of whole part
                                                     * @param mixed   s: sections delimiter
                                                     * @param mixed   c: decimal delimiter
                                                     */
                                                  Number.prototype.format = function (n, x, s, c) {
                                                      var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
                                                          num = this.toFixed(Math.max(0, ~~n));

                                                      return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
                                                  };

                                                  //12345678.9.format(2, 3, '.', ',');  // "12.345.678,90"

                                                function calc_mountEX(sender, eventArgs) {

                                                    var vExchangeRATE = eventArgs.get_newValue();

                                                    var clientSideMontoProject = $find("<%=txt_montoProyecto.ClientID  %>");
                                                    var clientSideMontoTotal = $find("<%=txt_montoTotal.ClientID  %>");
                                                                                                         
                                                    var ProjectContribution = clientSideMontoProject.get_value();
                                                    var ProjectTotalAmount = clientSideMontoTotal.get_value();
                                                    

                                                    //alert('Exchange Rate: ' + vExchangeRATE + ' ' + isNaN(vExchangeRATE));
                                                    //alert('Project Contribution: ' + ProjectContribution + ' ' + isNaN(ProjectContribution));
                                                    //alert('Project Total Amount: ' + ProjectTotalAmount + ' ' + isNaN(ProjectContribution));
                                                    
                                                    if (!isNaN(vExchangeRATE) && !isNaN(ProjectContribution)) {
                                                        //do some thing if it's a number
                                                        var Value1_USD = ProjectContribution / vExchangeRATE;                                                       
                                                        $("#<%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                                    } else {
                                                        //do some thing if it's NOT a number
                                                        $("#<%= lblt_totalproyectoUS.ClientID %>").html("0.00");
                                                    }

                                                    if (!isNaN(vExchangeRATE) && !isNaN(ProjectTotalAmount)) {
                                                        //do some thing if it's a number
                                                        var Value2_USD = ProjectTotalAmount / vExchangeRATE;                                                        
                                                        $("#<%= lbl_totalUS.ClientID %>").html(Value2_USD.format(2, 3, ',', '.'));
                                                    } else {
                                                        //do some thing if it's NOT a number
                                                        $("#<%= lbl_totalUS.ClientID %>").html("0.00");
                                                    }                                                                                                  
                                                    
                                                    //eventArgs.set_newValue(vExchangeRATE);

                                                }
                                             
                                                  
                                                function calc_mountMOUNT_project(sender, eventArgs) {
                                                    
                                                    var clientSideExchangeRate = $find("<%=txt_tasacambio.ClientID  %>");
                                                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                                                    var ProjectContribution = eventArgs.get_newValue();


                                                    if (!isNaN(vExchangeRATE) && !isNaN(ProjectContribution)) {
                                                        //do some thing if it's a number
                                                        var Value1_USD = ProjectContribution / vExchangeRATE;                                           
                                                       // alert('USD Total: ' + Value1_USD.format(2, 3, ',', '.'));
                                                        $("#<%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                                    } else {
                                                        //do some thing if it's NOT a number
                                                        $("#<%= lblt_totalproyectoUS.ClientID %>").html("0.00");
                                                    }
                                                                                                 
                                                    //eventArgs.set_newValue(vExchangeRATE);

                                                }

                                                function calc_mountMOUNT_Total(sender, eventArgs) {

                                                    var clientSideExchangeRate = $find("<%=txt_tasacambio.ClientID  %>");
                                                    var vExchangeRATE = clientSideExchangeRate.get_value(); 
                                                                                            
                                                    var ProjectTotalAmount = eventArgs.get_newValue();
                                                                                                             
                                                  //alert('Exchange Rate: ' + vExchangeRATE);
                                                  //alert('Project Amount: ' + ProjectTotalAmount);

                                                    if (!isNaN(vExchangeRATE) && !isNaN(ProjectTotalAmount)) {
                                                        //do some thing if it's a number
                                                        var Value2_USD =  ProjectTotalAmount / vExchangeRATE ;
                                                        $("#<%= lbl_totalUS.ClientID %>").html(Value2_USD.format(2, 3, ',', '.'));
                                                    } else {
                                                        //do some thing if it's NOT a number
                                                        $("#<%= lbl_totalUS.ClientID %>").html("0.00");
                                                    }

                                                    //eventArgs.set_newValue(vExchangeRATE);

                                                }

                                            </script>

                                  <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_projectContribution" runat="server" CssClass="control-label text-bold" Text="Project contribution"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->

                                            <telerik:RadNumericTextBox ID="txt_montoProyecto" Runat="server" MinValue="0" Width="150px" >
                                                    <NumberFormat ZeroPattern="n"></NumberFormat>
                                                    <ClientEvents OnValueChanging="calc_mountMOUNT_project" />
                                            </telerik:RadNumericTextBox>

                                            &nbsp;<asp:Label ID="lblt_dollasimbol1" CssClass="control-label text-bold" runat="server"  Text="= USD $ "></asp:Label>
                                            <asp:Label ID="lblt_totalproyectoUS"   CssClass="control-label text-bold" runat="server"  Text="0.00"></asp:Label>
                                            &nbsp;<asp:Label ID="lblerrMontoPC" runat="server" ForeColor="Red" Text="Required Or more than 0" Visible="False"></asp:Label>
                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_total_amount" runat="server" CssClass="control-label text-bold" Text="Total amount"></asp:Label>

                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                           <telerik:RadNumericTextBox ID="txt_montoTotal" Runat="server" MinValue="0" Width="150px" >
                                                <NumberFormat ZeroPattern="n"></NumberFormat>
                                                <ClientEvents OnValueChanging="calc_mountMOUNT_Total" />
                                            </telerik:RadNumericTextBox>

                                            &nbsp;<asp:Label ID="lblt_dollarsimbol2" runat="server"  CssClass="control-label text-bold" Text="= USD $ "></asp:Label>
                                            <asp:Label ID="lbl_totalUS" CssClass="control-label text-bold" runat="server" Text="0.00"></asp:Label>
                                            &nbsp;<asp:Label ID="lblerrMonto" runat="server" ForeColor="Red" Text="Required Or more than 0" Visible="False"></asp:Label>
                                       </div>
                                    </div>
                                  </div>


                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_instrument_number" runat="server" CssClass="control-label text-bold" Text="Internal Code"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                           <telerik:RadTextBox ID="txt_Number" Runat="server" Width="400px">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                                                ControlToValidate="txt_Number" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                  </div>

                                <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_beneficiaryName" runat="server" CssClass="control-label text-bold" Text="In reference to"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                           <telerik:RadTextBox ID="txt_beneficiario" Runat="server"  Width="400px">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                                            ControlToValidate="txt_beneficiario" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                  </div>


                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                           <asp:Label ID="lblt_comment" runat="server"  CssClass="control-label text-bold"  Text="Comments"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->
                                              <telerik:RadTextBox ID="txt_coments" Runat="server" Height="80px" TextMode="MultiLine" Width="80%">
                                                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                                ControlToValidate="txt_coments" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                       </div>
                                    </div>
                                  </div>
                                                              

                                
                               <div class="box-body">
                                    <div class=" row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lbl_documentsAPP" runat="server" CssClass="control-label text-bold"   Text="Documents Approvals"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->

                                             <asp:ImageButton ID="ImageButton1" runat="server" 
                                                    ImageUrl="~/Imagenes/Iconos/updateico.png" style="width: 16px" />
                                             
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
                                                                                                        oncheckedchanged="chkVisible_CheckedChangedDOCS"  />
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
                                                                                            FilterControlAltText="Filter extension column" HeaderText="Allowed extension" 
                                                                                            SortExpression="extension" UniqueName="extension">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                         <telerik:GridBoundColumn DataField="Template" 
                                                                                            FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                            UniqueName="Template" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                          <telerik:GridBoundColumn  DataField="max_size" 
                                                                                                 FilterControlAltText="Filter max_size column" HeaderText="Max Size(MB)" 
                                                                                                 SortExpression="max_size" UniqueName="max_size" 
                                                                                                 Visible="true" Display="true">
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

                                              <asp:HiddenField ID="hd_id_doc" runat="server" Value="0" />
                                           
                                           <br /> 
                                           <asp:Label ID="lbl_errExtension" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="false" Text="Select a type of document"></asp:Label> 
                                           <br />
                        
                                       </div>
                                    </div>
                                  </div>



                                 <div class="box-body">
                                    <div class="form-group row">


                                           <div class="col-sm-2">
                                                  <!--Tittle -->
                                               <asp:Label ID="lblt_new_file_" runat="server"  CssClass="control-label text-bold" Text="Attach New File"></asp:Label>                                             
                                            </div>
                                                   <div class="col-sm-3 ">
                                                      <!--Control --> <%----Here New file control--%>
                                                        <%--   --%>
                                              
                                                                      <telerik:RadAsyncUpload 
                                                                           RenderMode="Lightweight" 
                                                                           runat="server" ID="RadSync_NewFile" 
                                                                           MultipleFileSelection="Disabled"
                                                                           Skin="Silk" 
                                                                           TemporaryFolder="~/FileUploads/Temp/" 
                                                                           TargetFolder="~/FileUploads/Temp/"      
                                                                           HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx" 
                                                                           OnClientValidationFailed="validationFailed"                                                
                                                                           UploadedFilesRendering="AboveFileInput"  
                                                                           OnClientProgressUpdating="onClientFileUploading"                                               
                                                                           OnClientFileUploaded="file_approval_Uploaded"                                           
                                                                           TemporaryFileExpiration="1:00:00" Enabled="false" >
                                                                        </telerik:RadAsyncUpload>

                                                                   <script src="../scripts/FileUploadTelerik.js?V=0.08"></script>
                                    
                                                                    <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                                                    <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                                                    <asp:HiddenField runat="server" ID="lbl_oldFile" />
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                    <asp:HiddenField ID="hd_TipoDoc" runat="server" />     


                                                                      <script>


                                                                          function Refresh_DOC_grid() {
                                                                              
                                                                              var id_programa = parseInt(<%=Me.Session("E_IDPrograma")%>);
                                                                              //var app_TipoDoc = $('input[id*=hd_TipoDoc]').val();
                                                                              var app_TipoDoc = $find("<%=cmb_app.ClientID%>").get_value();

                                                                              var idDoc = $('input[id*=HiddenField1]').val();

                                                                              grdDocs_ = $find("<%=grd_documentos.ClientID%>").get_masterTableView();                          

                                                                             console.log("{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDoc:'" + idDoc + "' }");
                                                                              
                                                                              $.ajax({

                                                                                  type: "POST",
                                                                                  url: "frm_DocsAD.aspx/get_DocTYPE",
                                                                                  data: "{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDoc:'" + idDoc + "' }",
                                                                                  contentType: "application/json; charset=utf-8",
                                                                                  dataType: "json",
                                                                                  success: function (data) {

                                                                                      jsonResult = data.d;
                                                                                      //alert('Result ' + jsonResult);

                                                                                      var jsonResultARR = jsonResult.split('||');

                                                                                      //'JsonSubRegion
                                                                                      //'JsonDistrict
                                                                                      //'JsonOrgTypes
                                                                                      //'JsonSchools

                                                                                      //  alert('Result ' + jsonResultARR[0]);

                                                                                      if (jsonResultARR[0] != '[]') {//Documents Type

                                                                                          //fillCombo(comboPeriodo, jsonResultARR[0]);                      
                                                                                          var data = jQuery.parseJSON(jsonResultARR[0]);

                                                                                          //alert(data);
                                                                                          grdDocs_.set_dataSource(data);
                                                                                          grdDocs_.dataBind();
                                                                                          //grdDocs_.rebind();


                                                                                      }


                                                                                  },
                                                                                  failure: function (response) {
                                                                                      Populating = false;                                                                                                                                       
                                                                                      alert('Error Loagind Data: ' + response.d);
                                                                                  }
                                                                              });


                                                                          }

                                                                                                                                                                                                    
                                                                         function RowDataBound(sender, args) {

                                                                              // conditional formatting
                                                                                                                                                                                                           

                                                                             // var chk_control = args.get_item().findControl('chkSelect');

                                                                              //var chk_control = $(args.get_item()).find("input[id*='chkSelect']").get(0);

                                                                             // alert(chk_control);

                                                                             // alert(args.get_dataItem()["nombre_documento"] + ' ' + chk_control.checked);

                                                                                   //alert(chk_control.checked);

                                                                                    //if (args.get_dataItem()["descripcion_cat"] == "Dr.") {
                                                                                    //       args.get_item().get_cell("TitleOfCourtesy").style.fontWeight = "bold";
                                                                                    //   }

                                                                                    //   var sb = new Sys.StringBuilder();
                                                                                    //   sb.appendLine("<b>RowDataBound</b><br />");
                                                                                    //   for (var item in args.get_dataItem()) {
                                                                                    //       sb.appendLine(String.format("{0} : {1}<br />", item, args.get_dataItem()[item]));
                                                                                    //   }
                                                                                    //   sb.appendLine("<br />");
                                                                                    //   sb.appendLine("<br />");
                                                                                    <%--   $get("<%= Panel1.ClientID %>").innerHTML += sb.toString();--%>

                                                                                   }

                                                                         function CheckedChangedDOCS(sender) {

                                                                             //alert('is it selected? :' + sender.checked);                                                                                                                              
                                                                              //alert('Value :' + sender.value);                                                                                                                               

                                                                              //$('input[id*=hd_id_doc]').val(sender.value);
                                                                              //var typeDoc = $('input[id*=hd_id_doc]').val();
                                                                              //alert('Value Hidden :' + typeDoc);
                                                                                      
                                                                           <%--  var dvElement = $("#<%=dv_DOC_TYPE.ClientID%>");
                                                                             dvElement.hide();--%>

                                                                            // $('#dv_DOC_TYPE').css('display', 'none');
                                                                             //$('#dv_DOC_TYPE').addClass("hidden");

                                                                             div_Control(false);
                                                                                                                                                                                                           
                                                                              var radSync = $find("<%= RadSync_NewFile.ClientID %>");                                                                                                                              
                                                                             radSync.set_enabled(true);
                                                                             //radSync.click();

                                                                             // radSync.enabled = true;
                                                                             //alert(radSync.enabled);

                                                                              //alert($('#dv_DOC_TYPE').html);
                                                                              //$('#dv_DOC_TYPE').addClass("hidden");
                                                                             <%--$find("<%= msg_document_type.ClientID %>").hide();--%>

                                                                          }                                                                                                                             

                                                                          function changeUpload(fileUploaded) {

                                                                                 document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;                                                                                                           

                                                                                <%-- var img = document.getElementById("<%= imgUser.ClientID%>");
                                                                                 //img.className = "hidden";
                                                                                  document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;--%>
                                                                          }


                                                                             function AddItem(fileUploaded){

                                                                                 // alert(fileUploaded);
                                                                                 //hd_id_doc.Value = 
                                                                                 //var hd_id_document = $find("<%= hd_id_doc.ClientID %>");

                                                                                 var typeDoc = $('input[id*=hd_id_doc]').val();

                                                                                  <%-->   $find("<%= hd_id_doc.ClientID %>").val(); //bind with a propper document type value --%>
                                              
                                                                                // alert('Type of File: ' + typeDoc);

                                                                                 var rdList = $find("<%= rdListBox_files.ClientID %>");   
                                                                                 var items = rdList.get_items();
                                                                                 rdList.trackChanges();

                                                                                 var item = new Telerik.Web.UI.RadListBoxItem();
                                                                                 item.set_text(fileUploaded);
                                                                                 item.set_value(typeDoc);
                                                                                 items.add(item);
                                                                                 rdList.commitChanges();
                                                                                 
                                                                                 // var dvElement = $("#%=dv_DOC_TYPE.ClientID%>");
                                                                                 //dvElement.show();

                                                                           <%--      dvElement = document.getElementById('<%=dv_DOC_TYPE.ClientID%>')
                                                                                 //dvElement.style.display = "block";
                                                                                 alert(dvElement.innerHTML);

                                                                                 $('#<%=dv_DOC_TYPE.ClientID%>').show();--%>

                                                                                 <%-- var img = document.getElementById("<%= imgUser.ClientID%>");
                                                                                     //img.className = "hidden";
                                                                                     document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;--%>

                                                                             }

                                              
                                                                             function Check_deleted(sender, e) {

                                                                                 //alert("Successfully deleted: " + e.get_item().get_text());                                                      
                                                                                 var listAFdeleted = $find("<%= rdListBox_files.ClientID %>");
                                                                                 //alert('Elements: ' + listAFdeleted.get_items().get_count());

                                                                                 if (listAFdeleted.get_items().get_count() == 0) {                                                          
                                                                                     hasFiles("false");
                                                                                 }

                                                                             }


                                                                          function div_Control(bolShowUP) {

                                                                             // var dvElement = $("#%=dv_DOC_TYPE.ClientID%>");
                                                                              //alert($('#dv_DOC_TYPE').html);

                                                                              if (bolShowUP) {
                                                                              //dvElement.show();
                                                                               $('#dv_DOC_TYPE').css('display', 'block');
                                                                              }else{
                                                                                  //dvElement.hide();
                                                                                 $('#dv_DOC_TYPE').css('display', 'none');
                                                                              }

                                                                         }
                                              
                                                                           function hasFiles(valor) {
                                                                               document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                                                           }




                                                            </script>                                

                                                  
                                                        <div id="dv_DOC_TYPE" style="width:200px; height:50px; position:absolute; top:0px; left:5px; z-index:1;" >
                                                               <span class="badge bg-orange"><h4><i class="fa fa-hand-pointer-o"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Select document type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-hand-pointer-o"></i></h4></span> 
                                                        </div>

                                                  </div>






                                       <%-- <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_fileAtth" runat="server"  CssClass="control-label text-bold"   Text="File attach"></asp:Label>
                                        </div>
                                       <div class="col-sm-9">--%>


                                           <!--Control -->

                                                  <%--  <asp:UpdtePanel ID="PanelFirma" runat="server" UpdateMode="Conditional" >
                                 
                                                        <ContentTemplate>

                                                        <asp:Panel ID="Panel1" runat="server" BorderColor="#E0E0E0" BorderWidth="1px" Style="border-left-color: gray;
                                                                        border-bottom-color: gray; border-top-style: dashed; border-top-color: gray;
                                                                        border-right-style: dashed; border-left-style: dashed; border-right-color: gray;
                                                                        border-bottom-style: dashed" Width="85%">

                                                                            <CuteWebUI:Uploader ID="Uploader3" runat="server" CancelAllMsg="Cancelar todos"
                                                                                CancelUploadMsg="Cancel Carga" FileTooLargeMsg="Error al cargar el archivo, Máximo permitido 50MB"
                                                                                FileTypeNotSupportMsg="Archivo con exensión desconicida. Permitidos: doc, docx, pdf,xls, xlsx"
                                                                                InsertText="Attachment (Max)" 
                                                                                WindowsDialogLimitMsg="Imposible de seleccionar todos los archivos a la vez">
                                                                                <ValidateOption   MaxSizeKB="51200" />  
                                                                            </CuteWebUI:Uploader>

                                                                                         <asp:Panel ID="Panel1_firma" runat="server" Height="30px" Visible="False" 
                                                                                            Width="625px">
                                                                                             <table border="0" cellpadding="0" cellspacing="0" style="border: thin solid #ededed; width: 621px;
                                                                                                ">
                                                                                                 <tr>
                                                                                                     <td style="vertical-align: middle;" >
                                                                                                         <asp:HyperLink ID="HnlkArchivo" runat="server" 
                                                                                                             ImageUrl="~/Imagenes/iconos/attach.png" Target="_blank" ToolTip="Download file">HyperLink</asp:HyperLink>
                                                                                                     </td>
                                                                                                     <td style="width: 756px; height: 16px">
                                                                                                         <asp:Label ID="lblarchivo" runat="server" Text="Error.." Width="463px" 
                                                                                                             Height="18px"></asp:Label>
                                                                                                               <asp:Label ID="lblMsg" runat="server" Width="450px" 
                                                                                                             Height="18px"></asp:Label>

                                                                                                     </td>
                                                                                                     <td style="width: 53px; height: 16px">--%>
                                                                                              
                                                       <%--         &nbsp;<asp:ImageButton ID="" runat="server" --%>
                                             
                                                                                                       <%-- <asp:ImageButton ID="img_btn_agregar_temp" runat="server" 
                                                                                                             ImageUrl="~/Imagenes/iconos/accept.png" 
                                                                                                             ToolTip="Click here to attach the selected file" 
                                                                                                             CausesValidation="False" style="width: 16px" />
                                                                                                          </td>
                                                                                                     <td style="width: 77px; height: 16px">
                                                                                                
                                                                                                         <asp:ImageButton ID="img_btn_borrar_temp" runat="server" 
                                                                                                             CausesValidation="False" Height="16px" ImageUrl="~/Imagenes/iconos/b_drop.png" 
                                                                                                              ToolTip="Delete file" />
                                                                                                     </td>
                                                                                                 </tr>
                                                                                             </table>
                                                                                        </asp:Panel>
                                                                           </asp:Panel>
                                                                    <br />
                                                                </ContentTemplate>
                                                                 <Triggers>
                                                                     <asp:AsyncPostBackTrigger ControlID="img_btn_agregar_temp" EventName="Click" />
                                                                 </Triggers>
                                                          </asp:UpdtePanel>--%>
              
                                       
                                      <%-- </div>--%>



                                    </div>
                                  </div>


                               <div class="box-body">
                                  <div class="form-group row">

                                    <div class="col-sm-12">
                                        <hr style="border-color:black;" />
                                    </div> 
                                       
                                    <div class="form-group row">
                                            <div class="col-sm-2">
                                             <!--Tittle -->
                                                 <asp:Label ID="lblt_listName" runat="server" CssClass="control-label text-bold"   Text="Attached Files"></asp:Label>
                                            </div>
                                           <div class="col-sm-10">
                                               <!--Control -->                                                                                                         
                                               <telerik:RadListBox CssClass="pull-left" 
                                                   RenderMode="Lightweight" 
                                                   OnClientDeleted="Check_deleted" 
                                                   runat="server" 
                                                   ID="rdListBox_files" 
                                                   Height="100px" 
                                                   Font-Bold="true"
                                                   Font-Size="Small"                                                                                                               
                                                   Width="70%" 
                                                   AllowDelete="true" 
                                                   AllowReorder="false" 
                                                  ButtonSettings-AreaWidth ="40px" ></telerik:RadListBox>                                                                                    
                                                                                                                                                          
                                           </div>   
                                                                                                                                                                                                                                     

                                    </div>

                          </div>
                       </div>


<%--                                 <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_listDocuments" runat="server"   CssClass="control-label text-bold" Text="List of documents "></asp:Label>
                                        </div>
                                       <div class="col-sm-9">
                                           <!--Control -->

                                                  <telerik:RadGrid ID="grd_archivos"  Skin="Office2010Blue"   runat="server" CellSpacing="0"  DataSourceID="archivos_temp" GridLines="None"
                                                          Width="80%" AllowAutomaticDeletes="True" AutoGenerateColumns="False">
                                                        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                            <WebServiceSettings>
                                                                <ODataSettings InitialContainerName="">
                                                                </ODataSettings>
                                                            </WebServiceSettings>
                                                        </HeaderContextMenu>
                                                    <MasterTableView DataKeyNames="id_archivo_temp" DataSourceID="archivos_temp">
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                            Visible="True">
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                            Visible="True">
                                                        </ExpandCollapseColumn>
                                                            <Columns>
                                                  
                                                                  <telerik:GridButtonColumn ConfirmText="Are you sure you want to permanently delete this record?" 
                                                                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                                                                        ConfirmDialogWidth="400px" UniqueName="Delete" ImageUrl="../Imagenes/Iconos/b_drop.png" />
                                                        
                                                                     <telerik:GridBoundColumn DataField="nombre_documento" FilterControlAltText="Filter nombre_documento column" HeaderText="Document Type" 
                                                                          SortExpression="archivo" UniqueName="nombre_documento">
                                                                     </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn DataField="archivo" FilterControlAltText="Filter ruta_archivos column" HeaderText="Attachments" 
                                                                          SortExpression="archivo" UniqueName="ruta_archivos">
                                                                     </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column" UniqueName="ImageDownloadC" 
                                                                         HeaderButtonType="PushButton" >
                                                                    <ItemTemplate>
                                                                        <asp:hyperlink ID="ImageDownload" runat="server" 
                                                                                ImageUrl="~/imagenes/iconos/download.png" />
                                                                       </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle Width="5px"  />
                                                                  </telerik:GridTemplateColumn>

                                             
                                                            </Columns>
                                                                                <EditFormSettings>
                                                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                                                    </EditColumn>
                                                                                </EditFormSettings>
                                                                            </MasterTableView>
                                                                            <FilterMenu EnableImageSprites="False">
                                                                                <WebServiceSettings>
                                                                                    <ODataSettings InitialContainerName="">
                                                                                    </ODataSettings>
                                                                                </WebServiceSettings>
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>
                                               <asp:SqlDataSource ID="archivos_temp" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                    ConflictDetection="CompareAllValues" OldValuesParameterFormatString="original_{0}" 
                                    
                                                   SelectCommand="SELECT a.id_archivo_temp, a.id_sesion_temp, b.nombre_documento, a.archivo, a.id_documento, a.id_doc_soporte  
                                                                      FROM ta_archivos_documento_temp  a
                                                                       inner join ta_docs_soporte b on (a.id_doc_soporte = b.id_doc_soporte)
                                                                       WHERE (id_sesion_temp = @id_sesion)" 
                                                     ProviderName ="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>">
                                                           <SelectParameters>
                                                                <asp:ControlParameter ControlID="lbl_id_sesion_temp" Name="id_sesion" PropertyName="Text" DefaultValue="-1" />
                                                            </SelectParameters>
                                               </asp:SqlDataSource>
                                       </div>
                                    </div>
                                  </div>--%>

                                
                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                       <div class="col-sm-2 text-center  ">                                                            

                         </div>

                      <%-- else { console.log('All Data is completed'); return true; }--%>

                         <div class="col-sm-3 text-center  ">                                                            
                             <asp:Button ID="btn_Open" runat="server" Text="START APPROVAL PROCESS" OnClick="btn_Open_Click"  OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn btn-md btn-success"  />                              
                          </div>

                          <div class="col-sm-3 text-center  ">
                              <asp:Button ID="btn_Cancel" runat="server" Text="CANCEL" OnClick="btn_cancel_Click"  OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn btn-md btn-default" />                              
                          </div>
                       
                        <div class="col-sm-8 text-center  ">
                             
                            <br />
                            <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="True"></asp:Label> <br />
                        

                        </div>
                       
                         

                         
                   </div>

                </div>
           </section>

  
    </asp:Content>

