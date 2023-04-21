<%@ Page Language="VB" MasterPageFile="~/Site.Master"   AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_DocAprobacion" Codebehind="frm_DocAprobacion.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <link rel="stylesheet" type="text/css" href="../Content/Upload_styles.css" />

             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVAL</asp:Label>
                </h1>
            </section>
            <section class="content">

                <style type="text/css">

                    .RadListBox span.rlbText 
                    { 
                        font-size: large !important; 
                        font-family: Verdana, Arial, Helvetica,sans-serif; 
                        color: darkblue; 
                        font-weight: bold;
                    }

                    .wrapWord { 
                                word-wrap: break-word;
                                word-break:break-all; 
                              }
                    
                </style>

                 <link rel="stylesheet" href="../Content/slider_style.css?tst=0.002">

                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Approval in process</asp:Label>
                        </h3>                         
                    </div>
                    <div class="box-body  no-padding  text-left">                       
                            <div class="col-lg-12">                                                                      
                                <br /><br /><br />


                                                          <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                                           
                                                             <div class="panel panel-default">  <%--First panel--%> 
                                                                <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                                    <h4 class="panel-title">
                                                                        <a role="button" data-toggle="collapse" href="#collapseOne"
                                                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_informacion">Información de la aprobación
                                                                        </a>
                                                                    </h4>
                                                                </div>
                                                                <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                                                    <div class="panel-body">
                                                                             
                                                                            <!--**************************CONTENT HERE*************************************-->

                                                                                    <div class="col-sm-12 text-right"> <!--div 0 lg-12-->                                     
                                                                                      
                                                                                                <div class="form-group row">
                                                                                                         <div class="col-sm-4 text-left">
                                                                                                                 <!--Tittle -->
                                                                                                                    <asp:Label ID="lblIDocumento" runat="server"   Visible="False"></asp:Label>
                                                                                                                    <asp:Label ID="lblTipoDoc"    runat  ="server" Visible="False"></asp:Label>
                                                                                                                    <asp:Label ID="lblnextruta"  runat ="server"   Visible="False"></asp:Label>
                                                                                                                    <asp:Label ID="lblNextRole"  runat ="server"    Visible="False"></asp:Label>
                                                                                                                    <asp:Label ID="lblNextUserID"  runat ="server"    Visible="False"></asp:Label>
                                                                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                                                    <asp:HiddenField ID="hd_TipoDoc" runat="server" />                                                                                                             

                                                                                                                    <asp:HiddenField ID="hd_ROL" runat="server" />                                                              
                                                                                                                    <asp:HiddenField runat="server" ID="h_Filter" Value="0" />  

                                                                                                         </div>
                                                                                                         <div class="col-sm-8 text-right">
                                                                                                                   <!--Control -->
                                                                                                                    <asp:Label ID="lbl_IdCodigoAPP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                                                                                                    <asp:Label ID="lbl_categoria1" runat="server"  CssClass="control-label text-bold">/</asp:Label>
                                                                                                                    <asp:Label ID="lbl_IdCodigoSAP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                                                                                         </div>
                                                                                               </div>
                                                                                               
                                                                                            <div class="form-group row">
                                                                                                <div class="col-sm-12 text-left">
                                                                                                        <!--box top --><!--Grid-->

                                                                                                                                               <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" 
                                                                                                                                                   AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0"
                                                                                                                                                    GridLines="None" Width="100%" ShowHeader="False">
                                                                                                                                                          <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset">
                                                                                                                                                            <WebServiceSettings>
                                                                                                                                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                                                                                            </WebServiceSettings>
                                                                                                                                                            </HeaderContextMenu>
                                                                                                                                                        <MasterTableView >
                                                                                                                                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                                                                                                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                                                                                                                            </RowIndicatorColumn>
                                                                                                                                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                                                                                                                            </ExpandCollapseColumn>
                                                                                                                                                            <Columns>
                                                                                                                                                                    <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column"
                                                                                                                                                                        HeaderText="Id" SortExpression="orden" UniqueName="orden">
                                                                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                                                                    <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column"
                                                                                                                                                                        HeaderText="Unit" SortExpression="nombre_rol" UniqueName="nombre_rol">
                                                                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                                                                    <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column"
                                                                                                                                                                        HeaderText="User" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                                                                    <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"
                                                                                                                                                                        HeaderText="State" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                                                                    <telerik:GridBoundColumn DataField="fecha_aprobacion" 
                                                                                                                                                                            FilterControlAltText="Filter fecha_aprobacion column" 
                                                                                                                                                                            HeaderText="Date of approval" SortExpression="fecha_aprobacion" 
                                                                                                                                                                            UniqueName="fecha_aprobacion">
                                                                                                                                                                        </telerik:GridBoundColumn>
        
                                                                                                                                                                    <telerik:GridBoundColumn DataField="Alerta" FilterControlAltText="Filter Alerta column"
                                                                                                                                                                        HeaderText="Alert" SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">
                                                                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                                                                    <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                                                                                                                                        <ItemTemplate>
                                                                                                                                                                            <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                                                                                                                                                ToolTip="Indicador Incompleto">
                                                                                                                                                                        </asp:HyperLink>
                                                                                                                                                                        </ItemTemplate>
                                                                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                                                                </Columns>
                                                                                                                                                                <EditFormSettings>
                                                                                                                                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column" UniqueName="EditCommandColumn1">
                                                                                                                                                                    </EditColumn>
                                                                                                                                                                </EditFormSettings>
                                                                                                                                                            </MasterTableView>
                                                                                                                                                            <ClientSettings AllowDragToGroup="True" EnableRowHoverStyle="True">
                                                                                                                                                                <Selecting AllowRowSelect="True" />
                                                                                                                                                            </ClientSettings>
                                                                                                                                                            <FilterMenu EnableImageSprites="False">
                                                                                                                                                                <WebServiceSettings>
                                                                                                                                                                <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                                                                                                </WebServiceSettings>
                                                                                                                                                               </FilterMenu>
                                                                                                                                                           </telerik:RadGrid>

                                                                                                                                                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                                                                                                                             ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                                                                                                                             SelectCommand="SELECT * FROM dbo.FN_Ta_RutaSeguimiento(11) " 
                                                                                                                                                             UpdateCommand="UPDATE ta_categoria SET descripcion_cat = @descripcion_cat WHERE (id_categoria = @id_categoria)">
                                                                                                                                                             <UpdateParameters>
                                                                                                                                                                 <asp:Parameter Name="descripcion_cat" />
                                                                                                                                                                 <asp:Parameter Name="id_categoria" />
                                                                                                                                                             </UpdateParameters>
                                                                                                                                                         </asp:SqlDataSource>
                                                      

                                                                                                                                  <!--box left --><!--Grid-->
                                                                                                                                  <hr />
                                                                                                                             </div>    
                                                                                                                       </div>
                                                                                             
                                                                                             
                                                                                                     <div class ="form-group row">
                                                                                                           <div class="col-sm-12 text-left ">
                                                                                                                <!--box Right --><!--CONTROLS-->
                                            

                                                                                                                             <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                     <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                      <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                            </div>


                                                                                                                             <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                 <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Approvals"></asp:Label>                                
                   
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                             </div>

                                                                                                                             <div class=" row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                    <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>

                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                             </div>

                                            
                                                                                                                             <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                      <asp:Label ID="lblt_owner" runat="server"  CssClass="control-label text-bold"  Text="Actual owner"></asp:Label></div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_owner" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                             </div>

                                                                                                                              <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                    <asp:Label ID="lblt_NextApp" runat="server"  CssClass="control-label text-bold" Text="Next Approver"></asp:Label>
                 
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_aprueba" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                             </div>



                                                                                                                             <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->
                                                                                                                                   <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>
                                                                                                                               </div>
                                                                                                                             </div>


                                                                                                                        <div class="row">
                                                                                                                            <div class="col-sm-4 text-left">
                                                                                                                             <!--Tittle -->
                                                                                                                              <asp:Label ID="lblt_proccess_name" runat="server" CssClass="control-label text-bold" Text="Name of Process"></asp:Label> 
                        
                                                                                                                            </div>
                                                                                                                           <div class="col-sm-5">
                                                                                                                               <!--Control -->
                                                                                                                               <asp:Label ID="lbl_proceso" runat="server" ></asp:Label>
                                                                                                                           </div>
                                                                                                                            <div class="col-sm-2">
                                                                                                                               <!--Control -->
                                                                                                                               <div runat="server" id="ToolsViewer" visible="false" ><a href="#" runat="server" id="hrefVIEWER" class="btn btn-sm btn-success"><asp:Label ID="lbl_tool_viewer" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search"></span></a></div>
                                                                                                                           </div>
                                                                                                                         </div>                                                                    

                                                                                                                        <div class="row">
                                                                                                                            <div class="col-sm-4 text-left">
                                                                                                                             <!--Tittle -->                                                    
                                                                                                                                <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Sequence"></asp:Label>                    
                                                                                                                            </div>
                                                                                                                           <div class="col-sm-5">
                                                                                                                               <!--Control -->
                                                                                                                               <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                                                                                           </div>
                                                                                                                         </div>                                                                      
                                                                                
                                        
                                                                                                                         <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->  
                                                                                                                                      <asp:Label ID="lblt_instrument_number" runat="server"  CssClass="control-label text-bold"  Text="Internal Code"></asp:Label>                                                  
                                                                    
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_instrumento" runat="server" ></asp:Label>
                                                   
                                                                                                                               </div>
                                                                                                                             </div>
                                          
                                        

                                                                                                                          <div class="row">
                                                                                                                                <div class="col-sm-4 text-left">
                                                                                                                                 <!--Tittle -->                                                    
                                                                                                                                       <asp:Label ID="lblt_benficiary_NAme" runat="server"  CssClass="control-label text-bold" Text="In Reference to"></asp:Label>                                                        
                                                                    
                                                                                                                                </div>
                                                                                                                               <div class="col-sm-5">
                                                                                                                                   <!--Control -->
                                                                                                                                   <asp:Label ID="lbl_beneficiario" runat="server" ></asp:Label>
                                                   
                                                                                                                               </div>
                                                                                                                             </div>
                                                                                      
                                                                                                                <!--box Right -->
                                                                                                              </div> 


                                                                                                     </div> 
                                                                                         
                                                                                                        <div class="col-sm-12">
                                                                                                          <hr style="border-color:black;" />
                                                                                                       </div> 
                                                                                        
                                                                                                      <div class=" row">
                                                                                                        <div class="col-sm-2 text-left">
                                                                                                         <!--Tittle -->                                           
                                                                                                            <asp:Label ID="lblbt_comment" runat="server"  CssClass="control-label text-bold" Text="Comentario"></asp:Label>
                                                                                                        </div>
                                                                                                       <div class="col-sm-10">
                                                                                                           <!--Control -->
                                                                                                           <asp:Label ID="lbl_Comment" runat="server" CssClass="text-justify "  Width="90%"></asp:Label>
                                                                                                       </div>
                                                                                                    </div>
                                                                                        
                                                                                        
                                                                                         <%--here new row--%>                 
                                                                                              
                                                                                  </div> <!--div 0 lg-12-->                                                                  
                                                                         
                                                                        <!--**************************CONTENT HERE*************************************-->                                                                                                                                                

                                                                    </div>
                                                                </div>
                                                            </div>   <%--First panel--%> 


                                                           <div class="panel panel-default"> <%--Second panel--%> 
                                                              <div class="panel-heading" role="tab" id="headingTwo" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                                                    <h4 class="panel-title">
                                                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseTwo"
                                                                            aria-expanded="false" aria-controls="collapseTwo" runat="server" id="alink_descripcion">Adjuntar documento
                                                                        </a>
                                                                    </h4>
                                                                </div>
                                                                <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                                                                    <div class="panel-body">


                                                                          <!--**************************CONTENT HERE*************************************-->
                                                                       
                                                                            <div class="col-sm-12 "> <!--div 0 lg-12-->         

                                                                                        <div class=" row">
                                                                                                <div class="col-sm-2 text-left">
                                                                                                 <!--Tittle --><br />
                                                                                                     <asp:Label ID="lbl_documentsAPP" runat="server" CssClass="control-label text-bold"   Text="Seleccione el tipo de documento"></asp:Label>
                                                                                                </div>
                                                                                               <div class="col-sm-10">
                                                                                                   <!--Control -->

                                                                                                     <asp:ImageButton ID="ImageButton1" runat="server" 
                                                                                                            ImageUrl="~/Imagenes/Iconos/updateico.png" style="width: 16px" CssClass="pull-left" /><br />
                                             
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
                                                                                                                                                <ClientEvents OnRowDataBound="RowDataBound" />
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
                                                                                                                                                            <asp:CheckBox ID="chkSelect" runat="server" 
                                                                                                                                                                AutoPostBack="True" 
                                                                                                                                                                OnClick="CheckedChangedDOCS(this);"   
                                                                                                                                                                oncheckedchanged="chkVisible_CheckedChangedDOCS"     />
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
                                                                                                                                                    HeaderText="Nombre del documento" SortExpression="nombre_documento" 
                                                                                                                                                    UniqueName="descripcion_cat">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                                <telerik:GridBoundColumn DataField="extension" 
                                                                                                                                                    FilterControlAltText="Filter extension column" HeaderText="Extensiones permitidas" 
                                                                                                                                                    SortExpression="extension" UniqueName="extension">
                                                                                                                                                </telerik:GridBoundColumn>
                                    
                                                                                                                                                 <telerik:GridBoundColumn DataField="Template" 
                                                                                                                                                    FilterControlAltText="Filter Template column" HeaderText="Plantilla del documento" 
                                                                                                                                                    UniqueName="Template" Visible="true" Display="false">
                                                                                                                                                </telerik:GridBoundColumn>

                                                                                                                                                  <telerik:GridBoundColumn  DataField="max_size" 
                                                                                                                                                        FilterControlAltText="Filter max_size column" HeaderText="Peso maximo(MB)" 
                                                                                                                                                        SortExpression="max_size" UniqueName="max_size" 
                                                                                                                                                        Visible="true" Display="true">
                                                                                                                                                    </telerik:GridBoundColumn>
                                    
                                                                                                                                             <telerik:GridTemplateColumn 
                                                                                                                                                  FilterControlAltText="Filter colm_template column"  HeaderText="Descargar plantilla" 
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
                                                                                                   <br />                                                                                                 
                                                                                                   <br />
                                                                                                   <asp:HiddenField ID="hd_id_doc" runat="server" Value="0" />                                                                                                                                              
                                                                                                   <asp:Label ID="lbl_errExtension" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="false" Text="Select a type of document"></asp:Label> 
                                                                                                                           
                                                                                               </div>
                                                                                            </div>
                                                                                
                                                                                           <div class=" row">

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
                                      
                                                                                                                       <script>


                                                                                                                           function Refresh_DOC_grid() {
                                                                                                                               
                                                                                                                               var id_programa = parseInt(<%=Me.Session("E_IDPrograma")%>);
                                                                                                                               var app_TipoDoc = $('input[id*=hd_TipoDoc]').val();
                                                                                                                               var idDoc = $('input[id*=HiddenField1]').val();

                                                                                                                               grdDocs_ = $find("<%=grd_documentos.ClientID%>").get_masterTableView();                          

                                                                                                                              // alert("{idProgram:'" + id_programa + "', id_TipoDoc:'" + app_TipoDoc + "', IdDoc:'" + idDoc + "' }");
                                                                                                                               
                                                                                                                               $.ajax({

                                                                                                                                   type: "POST",
                                                                                                                                   url: "frm_DocAprobacion.aspx/get_DocTYPE",
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

                                                                                                <div class="col-sm-5 ">

                                                                                                        

                                                                                                </div>

                                                                                             </div>
                                                                                              
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


                                                                          
                                                                                
                                                                                  </div> <%--END--%>
                                                                                                                                                 
                                                                        <!--**************************CONTENT HERE*************************************-->


                                                                    </div>
                                                                </div>
                                                            </div> <%--Second panel--%> 



                                                            <div class="panel panel-default"> <%--third panel--%> 
                                                              <div class="panel-heading" role="tab" id="headingTree" data-toggle="collapse" data-parent="#accordion" href="#collapseTree" aria-expanded="false" aria-controls="collapseTree">
                                                                    <h4 class="panel-title">
                                                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseTree" aria-expanded="false" aria-controls="collapseTwo" runat="server" id="a1">
                                                                              Validar y ajustar documentos
                                                                        </a>
                                                                    </h4>
                                                              </div>
                                                              <div id="collapseTree" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTree">
                                                                    <div class="panel-body">
                                                                                                                                                    
                                                                           <div class="row">

                                                                                    <div class="col-sm-2 text-left">
                                                                                     <!--Tittle -->
                                                                                          <asp:Label ID="lblt_FileAtth" runat="server"  CssClass="control-label text-bold"   Text="Attached Files"></asp:Label>
                                                                                    </div>
                                                                                   <div class="col-sm-10">
                                                                                       <!--Control -->
                                                                                   </div>

                                                                                   <div class="col-sm-12 "><!--Grid-->
                                                                                                                                        
                                                                                             <telerik:RadGrid ID="grd_archivos" runat="server" CellSpacing="0" GridLines="None" 
                                                                                                 AllowAutomaticDeletes="True" AutoGenerateColumns="False" Skin="Office2010Blue" >
                                                 
                                                                                                        <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                                                                             <Selecting AllowRowSelect="True"></Selecting>
                                                                                                         </ClientSettings>
                                                                                                        <MasterTableView
                                                                                                            DataKeyNames="id_archivo" >
                                                                                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                                                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                                                                                Visible="True">
                                                                                                            </RowIndicatorColumn>
                                                                                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                                                                                Visible="True">
                                                                                                            </ExpandCollapseColumn>
                                                                                                            <Columns>
                                                                                                               <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column" UniqueName="ImageDownload" HeaderButtonType="PushButton" Visible="True" Display="True"  >
                                                                                                                   <ItemTemplate>
                                                                                                                       <asp:HyperLink ID="ImageDownload" runat="server">[ImageDownload]</asp:HyperLink>
                                                                                                                   </ItemTemplate>
                                                                                                                    <HeaderStyle Width="1%" />
                                                                                                                    <ItemStyle Width="1%"  />
                                                                                                              </telerik:GridTemplateColumn>
                                                                                                                 <telerik:GridBoundColumn DataField="nombre_documento" 
                                                                                                                    FilterControlAltText="Filter nombre_documento column" 
                                                                                                                    HeaderText="Nombre del documento" 
                                                                                                                    SortExpression="nombre_documento" UniqueName="nombre_documento" Visible="True" Display="True">
                                                                                                                      <ItemStyle Width="15%" CssClass="wrapWord"  />
                                                                                                                </telerik:GridBoundColumn>
                                                                                                                <telerik:GridBoundColumn DataField="archivo" 
                                                                                                                    FilterControlAltText="Filter ruta_archivos column" 
                                                                                                                    HeaderText="Adjunto" 
                                                                                                                    SortExpression="archivo"                                                              
                                                                                                                    UniqueName="ruta_archivos" 
                                                                                                                    Visible="True" 
                                                                                                                    Display="True">
                                                                                                                     <ItemStyle Width="300px" CssClass="wrapWord"  />
                                                                                                                </telerik:GridBoundColumn>   
                                                                                                                 <telerik:GridBoundColumn DataField="ver" 
                                                                                                                    FilterControlAltText="Filter ver column" HeaderText="Versión" 
                                                                                                                    SortExpression="ver" UniqueName="ver"  Visible="True" Display="True">
                                                                                                                </telerik:GridBoundColumn>                                                                                                             
                                                                                                                <telerik:GridBoundColumn DataField="nombre_rol" 
                                                                                                                    FilterControlAltText="Filter nombre_rol column" HeaderText="Rol Rev" 
                                                                                                                    SortExpression="nombre_rol" UniqueName="nombre_rol" Visible="True" Display="True">
                                                                                                                     <ItemStyle Width="20%"  CssClass="wrapWord"  />
                                                                                                                </telerik:GridBoundColumn>
                                                                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter fileControl column" 
                                                                                                                    UniqueName="fileControl" HeaderText="Versión de actualización" >
                                                                                                                    <ItemTemplate>
                                                                                                                        <telerik:RadAsyncUpload ID="RadAsyncUpload1" 
                                                                                                                            runat="server"                                                                                                                              
                                                                                                                            MaxFileInputsCount="1" 
                                                                                                                            PostBackTriggers="btn_return,btn_Approved,btn_Completed,btn_STandBy,btn_NotApproved,btn_Cancelled"
                                                                                                                            TemporaryFolder="~/FileUploads/Temp/" 
                                                                                                                            TargetFolder="~/FileUploads/Temp/"     
                                                                                                                            CssClass=" btn-sm btn-default "
                                                                                                                            Width="200px">
                                                                                                                        </telerik:RadAsyncUpload>                                                              
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridBoundColumn DataField="extension" 
                                                                                                                    FilterControlAltText="Filter extension column" SortExpression="extension" 
                                                                                                                    UniqueName="extension" Visible="True" Display="False" >
                                                                                                                </telerik:GridBoundColumn>
                                                                                                                <telerik:GridBoundColumn DataField="id_archivo" 
                                                                                                                    FilterControlAltText="Filter id_archivo column" SortExpression="id_archivo" 
                                                                                                                    UniqueName="id_archivo" Visible="True" Display="False" >
                                                                                                                </telerik:GridBoundColumn>
                                                                                                                <telerik:GridBoundColumn DataField="id_doc_soporte" 
                                                                                                                    FilterControlAltText="Filter id_doc_soporte column" 
                                                                                                                    SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" Visible="True" Display="False">
                                                                                                                </telerik:GridBoundColumn>
                                                                                                                 <telerik:GridBoundColumn DataField="id_App_Documento" 
                                                                                                                    FilterControlAltText="Filter id_App_Documento column" 
                                                                                                                    SortExpression="id_App_Documento" UniqueName="id_App_Documento" Visible="True" Display="False">
                                                                                                                </telerik:GridBoundColumn>
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

                                                                                            <asp:SqlDataSource ID="archivos_temp" runat="server" 
                                                                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                                                            ConflictDetection="CompareAllValues" 
                                                                                            OldValuesParameterFormatString="original_{0}" 
                                                
                                                                                                SelectCommand="SELECT ta_AppDocumento.id_documento, ta_AppDocumento.id_ruta, ta_AppDocumento.id_App_Documento, ta_docs_soporte.id_doc_soporte, ta_archivos_documento.id_archivo, ta_archivos_documento.archivo, ta_docs_soporte.nombre_documento, ta_docs_soporte.extension FROM ta_archivos_documento INNER JOIN ta_AppDocumento ON ta_archivos_documento.id_App_Documento = ta_AppDocumento.id_App_Documento INNER JOIN ta_docs_soporte ON ta_archivos_documento.id_doc_soporte = ta_docs_soporte.id_doc_soporte WHERE (ta_AppDocumento.id_documento = @id_doc) AND (ta_AppDocumento.id_App_Documento = @id_appDoc)" 
                                                                                                ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>">
                                                                                            <SelectParameters>
                                                                                                <asp:QueryStringParameter DefaultValue="-1" Name="id_doc" 
                                                                                                    QueryStringField="IdDoc" />
                                                                                                <asp:ControlParameter ControlID="lblIDocumento" DefaultValue="-1" 
                                                                                                    Name="id_appDoc" PropertyName="Text" />
                                                                                            </SelectParameters>
                                                                                        </asp:SqlDataSource>             
                                                                                      
                                       
                                                                                    </div><!--Grid-->


                                                                           </div>
                                                                             

                                                                    </div>
                                                                </div>
                                                            </div> <%--third panel--%> 
                                                      
                                        
                                                </div> <%--panel group--%>
                                                                                    
                                

                                                       <div class="box-body">
                                                        <div class="row">
                                                            <div class="col-sm-4 text-left">
                                                             <!--Tittle --><br /><br />
                                                                <asp:Label ID="lblt_writcomments" runat="server"  CssClass="control-label text-bold"  Text="Write your Comments"></asp:Label>
                                                            </div>
                                                           <div class="col-sm-8">
                                                               <!--Control -->
                                                           </div>
                                                            <div class="col-sm-12">
                                                                    <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px"  TextMode="MultiLine" Width="85%">
                                                                    </telerik:RadTextBox>                                                 
                                                                <br />

                                                             </div>
                                                        </div>
                                                      </div>


                                  <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12 text-left form-group-sm  ">
                                          <!--Buttoms -->
                                              

                                       </div>
                                    </div>
                                  </div>
                                
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12 text-center">                                              
                                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                                            <br /><br />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 text-center  ">                                           
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_Approved" runat="server" Text="APPROVED"  OnClick="btn_Approved_Click"  OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="65%" />
                                            <asp:Button ID="btn_Completed" runat="server" Text="APPROVED" OnClick="btn_Completed_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-info" /> 
                                       </div>
                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_STandBy" runat="server" Text="STAND BY" OnClick="btn_STandBy_Click"   OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-warning"  Width="65%" />                                            
                                       </div>
                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                              <asp:Button ID="btn_NotApproved" runat="server" Text="NOT APPROVED"  OnClick="btn_NotApproved_Click"   OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn-lg btn-danger"  Width="65%" />                                                                                        
                                       </div>

                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_Cancelled" runat="server" Text="CANCELLED"  OnClick="btn_Cancelled_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-danger" Width="65%" />                                                  
                                       </div>

                                    </div>
                                  </div>


                              <%-- </div> <!--col-lg-12-->--%>
                                       
                        </div>

                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->

                        <div class="col-sm-12 text-center  ">
                             
                            <br />                            
                            <asp:Button ID="btn_return" runat="server" Text="RETURN WITHOUT SAVING"  OnClick="btn_return_Click"  OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn btn-default" />                                                  
                            
                            </div>
                           <asp:Label ID="lbl_fileUpload" runat="server"></asp:Label>   
                           
                                     
                   </div>

                      
                  </div>
                </div>
                <asp:HiddenField ID="hd_tasa_cambio" runat="server" Value="0" />
                <asp:HiddenField ID="hd_is_tool" runat="server" Value="0" />
                <asp:HiddenField ID="hd_id_delivered" runat="server" Value="0" />               
                <div class="modal fade bs-example-modal-lg" id="modal_confirm_amount" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-lg vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                    <h4 class="modal-title" runat="server" id="H2">Allocate amount and exchange rate</h4>
                                </div>
                                <div class="modal-body">
                                     <div class="row">                                    
                                        <div id="Current_Deliverable" runat="server" class="col-sm-10" >
                                            

                                        </div>
                                        <br /><br />
                                    </div>

                                     <div class="row">                                    
                                        <div class="col-sm-3" >
                                             <asp:Label runat="server" ID="lblt_current_exchange" CssClass="control-label text-bold">Current Exchange Rate</asp:Label>
                                        </div>
                                         <div class="col-sm-5" >
                                            <asp:Label ID="lbl_period" runat="server" CssClass="control-label" Text="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_current_exchange_rate" runat="server" CssClass="control-label text-bold" Text="" />
                                        </div>
                                       <br /><br />
                                    </div>   
                                    <div class="row">
                                            <div class="col-sm-2 text-right">
                                               <asp:Label runat="server" ID="lblt_currency_entry" CssClass="control-label text-bold">Currency data Entry:</asp:Label>
                                            </div>
                                            <div class="col-sm-2 text-right">   
                                                <br />
                                                <label class="switch">
                                                  <input id="chk_data_in" runat="server" type="checkbox" onchange="Currency_input()">
                                                  <span class="slider round"></span>
                                                </label>                                              
                                            </div>       
                                            <div class="col-sm-1 text-left" >                                        
                                              <h2><span id="currency_entry" class="label label-info">R$</span></h2>      
                                            </div>
                                            <div class="col-sm-3 text-right">
                                                <asp:HiddenField ID="curr_local" runat="server" Value="" />
                                                <asp:HiddenField ID="curr_International" runat="server" Value="" />                                                
                                            </div>
                                            <div class="col-sm-3 text-right">
                                            </div>
                                    </div>                                      
                                     <div class="row">
                                         <div class="col-sm-4" >                                              
                                               <asp:Label runat="server" ID="lblt_Local_Value" CssClass="control-label text-bold">Allocated Amount</asp:Label>
                                               <br />
                                               <telerik:RadNumericTextBox ID="txt_Local_Value"  CssClass="form-control "  runat="server" Width="50%">
                                                    <ClientEvents OnValueChanging="calc_Tot" />
                                               </telerik:RadNumericTextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="txt_Local_Value" CssClass="Error" Display="Dynamic" ErrorMessage="(*)" ValidationGroup="3">(*)</asp:RequiredFieldValidator>
                                        </div> 
                                        <div class="col-sm-4" >                                              
                                               <asp:Label runat="server" ID="lblt_tasa_Cambio" CssClass="control-label text-bold">Exchange Rate</asp:Label>
                                               <br />
                                               <telerik:RadNumericTextBox ID="txt_total_tasa_cambio"  CssClass="form-control "  runat="server" Width="50%">
                                                   <ClientEvents OnValueChanging="calc_mountEX" />
                                               </telerik:RadNumericTextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txt_total_tasa_cambio" CssClass="Error" Display="Dynamic" ErrorMessage="(*)" ValidationGroup="3">(*)</asp:RequiredFieldValidator>  
                                        </div>
                                        <div class="col-sm-4" >                                              
                                               <asp:Label runat="server" ID="lblt_USD_value" CssClass="control-label text-bold">Allocated Amount (USD)</asp:Label>
                                               <br />
                                               <telerik:RadNumericTextBox ID="txt_USD_val"  CssClass="form-control "  runat="server" Width="50%">
                                                   <ClientEvents OnValueChanging="calc_Tot_USD" />
                                               </telerik:RadNumericTextBox> 
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="txt_USD_val" CssClass="Error" Display="Dynamic" ErrorMessage="(*)" ValidationGroup="3">(*)</asp:RequiredFieldValidator> <br />
                                            <asp:Label ID="lbl_updatedBy" runat="server" CssClass="control-label text-bold" ForeColor="Red" Text="" /> 
                                        </div>                                         
                                    </div> 
                                    <div class="row">                                    
                                        <div class="col-sm-12" >
                                             <br /><br />
                                            <asp:Label ID="lblt_message_allocated_mount" runat="server" Text="Confirm the value allocated and it's respective exchange rates" />                                             
                                             <br />
                                        </div>
                                    </div>                                    
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_registrar_tc" CssClass="btn btn-sm btn-primary btn-ok" Text="Confirm" data-dismiss="modal" UseSubmitBehavior="false" ValidationGroup="3" Width="15%" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button3" onclick="javacript:cancel_rate();" Width="15%" >Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

               <div class="modal fade bs-example-modal-sm" id="modalTasaCambio" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                    <h4 class="modal-title" runat="server" id="H1">Exchange Rate</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblt_message_exchange_rate" runat="server" Text="Se debe ingresar la tasa de cambio correspondiente al periodo, favor llame al administrador del sistema" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_add_exchange_Rate" CssClass="btn btn-sm btn-primary btn-ok" Text="Registrar tasa de cambio" data-dismiss="modal" UseSubmitBehavior="false"  />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2" onclick="javacript:cancel_rate();">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <script>

                    function FuncModal_Monto() {
                        $('#modal_confirm_amount').modal('show');
                    }

                    function FuncModatTrimTasaCambio() {
                        $('#modalTasaCambio').modal('show');
                    }

                   function cancel_rate() {
                        $('#modalTasaCambio').modal('hide');
                    }
                    

                   $('#<%=chk_data_in.ClientID %>').prop('checked', false);//default value LOCA curr       

                  function Currency_input() {
                       if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {           
                           //alert('Checkeado');               
                           var currencySymbol =  $('input[id*=curr_International]');
                           $("#currency_entry").html(currencySymbol.val());
                           $("#span_curr_entry").html(currencySymbol.val());               
                        } else {
                           //alert('NO Checkeado');
                           var currencySymbol = $('input[id*=curr_local]');
                           $("#currency_entry").html(currencySymbol.val());
                           $("#span_curr_entry").html(currencySymbol.val());
                        }

                    }


                    var UpdatingValues = 0;

                                        function calc_Tot(sender, eventArgs) {                                                    

                                            if (UpdatingValues==0){

                                                   UpdatingValues = 1;
                                                    var clientSideExchangeRate = $find("<%=txt_total_tasa_cambio.ClientID  %>");
                                                    var vExchangeRATE = clientSideExchangeRate.get_value(); 

                                                    var Local_currency_contribution = eventArgs.get_newValue();
                                                    var Value_USD = 0;

                                                    if (!isNaN(vExchangeRATE) && !isNaN(Local_currency_contribution)) {
                                                        //do some thing if it's a number
                                                        Value_USD = Local_currency_contribution / vExchangeRATE;
                                                       // alert('USD Total: ' + Value1_USD.format(2, 3, ',', '.'));
                                                       // $("#%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                                    } else {
                                                        //do some thing if it's NOT a number
                                                        //$("#%= lblt_totalproyectoUS.ClientID %>").html("0.00");
                                                        Value_USD = 0;
                                                    }
                                                                                           
                                                    //alert(Value_USD);

                                                  var clientSideUSD_value = $find("<%=txt_USD_val.ClientID  %>");                                                 
                                                      clientSideUSD_value.set_value(Value_USD);
                                                        //eventArgs.set_newValue(vExchangeRATE);
                                                      UpdatingValues = 0;
                                             
                                            }// if (UpdatingValues==0){

                                        }


                                        function calc_mountEX(sender, eventArgs) {

                                            if (UpdatingValues == 0) {

                                                UpdatingValues = 1;
                                                var client_Local_amount = $find("<%=txt_Local_Value.ClientID %>");
                                                var clientSideUSD_value = $find("<%=txt_USD_val.ClientID%>");

                                                var USD_contribution = clientSideUSD_value.get_value();
                                                var Local_currency_contribution = client_Local_amount.get_value();

                                                var vExchangeRATE = eventArgs.get_newValue();
                                                //var Local_currency_contribution = eventArgs.get_newValue();
                                                var Value_USD = 0;
                                                var Value_LOCAL = 0;
                                                
                                               if ($('#<%=chk_data_in.ClientID %>').is(":checked")) { //USD

                                                            if (!isNaN(vExchangeRATE) && !isNaN(USD_contribution)) {
                                                                    //do some thing if it's a number
                                                               // Value_USD = Local_currency_contribution / vExchangeRATE;
                                                                Value_LOCAL = USD_contribution * vExchangeRATE;
                                                                    // alert('USD Total: ' + Value1_USD.format(2, 3, ',', '.'));
                                                                    // $("#%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                                            } else {
                                                                    //do some thing if it's NOT a number
                                                                    //$("#%= lblt_totalproyectoUS.ClientID %>").html("0.00");
                                                                Value_USD = 0;
                                                                Value_LOCAL = 0;
                                                            }

                                                        client_Local_amount.set_value(Value_LOCAL);

                                                } else { //Local Currency
                                                                                                                                                                                   
                                                            if (!isNaN(vExchangeRATE) && !isNaN(Local_currency_contribution)) {
                                                                    //do some thing if it's a number
                                                                    Value_USD = Local_currency_contribution / vExchangeRATE;
                                                                    // alert('USD Total: ' + Value1_USD.format(2, 3, ',', '.'));
                                                                    // $("#%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                                            } else {
                                                                    //do some thing if it's NOT a number
                                                                    //$("#%= lblt_totalproyectoUS.ClientID %>").html("0.00");
                                                                Value_USD = 0;
                                                                Value_LOCAL = 0;

                                                             }

                                                       clientSideUSD_value.set_value(Value_USD);
                                                            
                                                }
                                                                                                                                               
                                                //alert(Value_USD);                                                                                     
                                                //eventArgs.set_newValue(vExchangeRATE);
                                                UpdatingValues = 0;

                                            }//if (UpdatingValues==0){

                                         }
                                  

                     function calc_Tot_USD(sender, eventArgs) {
                                                    
                                         
                                  if (UpdatingValues == 0) {

                                      UpdatingValues = 1;
                                      var clientSideExchangeRate = $find("<%=txt_total_tasa_cambio.ClientID  %>");
                                      var vExchangeRATE = clientSideExchangeRate.get_value();

                                      var Value_USD = eventArgs.get_newValue();
                                      var Local_currency_contribution = 0;

                                      if (!isNaN(vExchangeRATE) && !isNaN(Value_USD)) {
                                          Local_currency_contribution = (Value_USD * vExchangeRATE);
                                          // alert('USD Total: ' + Value1_USD.format(2, 3, ',', '.'));
                                          // $("#%= lblt_totalproyectoUS.ClientID %>").html(Value1_USD.format(2, 3, ',', '.'));
                                      } else {
                                          Local_currency_contribution = 0;
                                      }

                                      //alert(Local_currency_contribution);

                                      var client_Local_amount = $find("<%=txt_Local_Value.ClientID%>");
                                      client_Local_amount.set_value(Local_currency_contribution);

                                      UpdatingValues = 0;
                                  }//if (UpdatingValues == 0) {

                    }



                     
                    function OpenRadWindowTool(url) {

                        var oWnd = $find("<%= RadWindowTool.ClientID %>");
                        //console.log("http://rms.ftfyla.com/RMS_SIME/Deliverable/frm_DeliverableFollowingRep.aspx?ID=" + idDeliverable);
                        oWnd.moveTo(200, 400);
                        oWnd.add_close(OnClientClose_Tool); //set a function to be called when
                        oWnd.show();
                        oWnd.setSize(1024, 800);
                        oWnd.setUrl(url); //'frm_DeliverableFollowingRep.aspx?ID=' + id
                        //oWnd.setUrl('http://www.yahoo.com');
                        oWnd.minimize();
                        oWnd.maximize();
                        oWnd.restore();                                         
                        
                     }


                    function OnClientClose_Tool(oWnd) {  
                        oWnd.setUrl("about:blank"); // Sets url to blank  
                        oWnd.remove_close(OnClientClose_Tool);                
                    }       

                    
                                  

                            
                
              </script>


                 <telerik:RadWindow InitialBehaviors="Maximize" RenderMode="Lightweight" runat="server" Width="800" Height="300" id="RadWindowTool" VisibleOnPageLoad="false"  >
                 
                 </telerik:RadWindow>   


           </section>    
         
      
    </asp:Content>

