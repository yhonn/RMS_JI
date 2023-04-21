<%@ Page Language="VB" MasterPageFile="~/Site.Master"  AutoEventWireup="false"    Inherits="RMS_APPROVAL.frm_seguimientoNotificacion" Codebehind="frm_seguimientoNotificacion.aspx.vb"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>



<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">


<script>


    function openWindow() {

        var idDOc =  $('#<%#lblIDocumento.ClientID%>').val(); //$find("<#lblIDocumento.ClientID %>");
        var idRuta = $('#<%#lblIdRuta.ClientID %>').val();  //$find("<#lblIdRuta.ClientID  %>");
                
       // window.alert('frm_seguimientoAprobacionMessRep.aspx?IdDoc=' + idDOc + '&IdRuta=' + idRuta);

        //var OpenScript = 'frm_seguimientoAprobacionMessRep.aspx?IdDoc=' + idDOc.html + '&IdRuta=' + idRuta.html;
        //window.alert(OpenScript);
        //window.open(OpenScript, '_blank');

    }

</script>

    <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVALS</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla"> Comments of Approval process </asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-8 text-left">
                                         <!--Tittle -->                                         
                                            <asp:Label ID="lblIDocumento0" runat="server"  Visible="false"></asp:Label>
                                            <asp:Label ID="lblIDocumento" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblIDocumento1" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblAppIDocumento" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblIdRuta" runat="server" Visible="false"></asp:Label>                                            
                                            <asp:LinkButton ID="btnlk_printing_" runat="server" AutoPostBack="True" SingleClick="true"  Text="Export" Width="15%" class="btn btn-success btn-sm margin-r-5" data-toggle="Export"  ><i class="fa fa-print fa-2x"></i>&nbsp;&nbsp;Vista de impresión</asp:LinkButton>                                          
                                            <asp:HiddenField runat="server" ID="h_Filter" Value="0" />  
                                        </div>
                                       <div class="col-sm-4  text-left"">
                                           <!--Control -->
                                             <%--<asp:Label ID="lbl_IdCodigoAPP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                                 <asp:Label ID="lbl_categoria1" runat="server"  CssClass="control-label text-bold">/</asp:Label>
                                                 <asp:Label ID="lbl_IdCodigoSAP" runat="server" CssClass="control-label text-bold"></asp:Label><br />
                                                 <asp:Label ID="lbl_status" runat="server"  CssClass="control-label text-bold"></asp:Label>  OnClientClick="openWindow();"   --%>                                                 
                                       </div>
                                    </div>
                                  </div>
                               </div> 
              
                          </div>
                    </div>

                     

                    <div class="box-body" >
                            
                                 <div class="row">
                                               
                                                  <div class="col-sm-12 text-left">
                                                       <!--box top --><!--Grid-->

                                                           <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" Skin="Office2010Blue" 
                                                               AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0"
                                                                GridLines="None" Width="90%" ShowHeader="True">
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
                                                                                    HeaderText="Unit" SortExpression="nombre_rol" UniqueName="nombre_rol" Visible="false">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column"
                                                                                    HeaderText="Usuario" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"
                                                                                    HeaderText="Estado" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="fecha_aprobacion" 
                                                                                        FilterControlAltText="Filter fecha_aprobacion column" 
                                                                                        HeaderText="Fecha de aprobación" SortExpression="fecha_aprobacion" 
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
                                                                                                                           
                                                      

                                                       <!--box TOP --><!--Grid-->
                                                      <hr />
                                                   </div> 
                                            </div>

                                          
                                           <div class =" row"><!--box Group1-->

                                                 <div class="col-sm-12 text-left ">
                                                    <!--box bottom--><!--CONTROLS-->

                                                             <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                         <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                          <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>                                                                        
                                                                   </div>
                                                                </div>


                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                     <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Approvals"></asp:Label>                                
                   
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>                                                                                           
                                                                   </div>
                                                                 </div>

                                                                 <div class=" row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>

                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>                                                                        
                                                                   </div>
                                                                 </div>

                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                       <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>                                                                        
                                                                   </div>
                                                                 </div>
                                                   
                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                       <asp:Label ID="lblt_tipoDocumento" runat="server"  CssClass="control-label text-bold" Text="Document Type"></asp:Label>                                                        
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_tipoDocumento" runat="server" ></asp:Label>                                                                                                                                          
                                                                   </div>
                                                                 </div>

                                                                  <div class="row">
                                                                        <div class="col-sm-4 text-left">
                                                                         <!--Tittle -->
                                                                          <asp:Label ID="lblt_proccess_name" runat="server"   CssClass="control-label text-bold" Text="Name of Process"></asp:Label>
                        
                                                                        </div>
                                                                       <div class="col-sm-7">
                                                                           <!--Control -->
                                                                           <asp:Label ID="lbl_proceso" runat="server" style="font-weight: 700"></asp:Label>
                                                                       </div>
                                                                 </div>      

                                                                 
                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->                                                    
                                                                        <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Code of Process"></asp:Label>                    
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->                                                                   
                                                                        <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>   
                                                        

                                                                  <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_region" runat="server"  CssClass="control-label text-bold"  Text="Region"></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_region" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>
                                                     
                                                                 <div class="row hidden">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_montoProyecto" runat="server"  CssClass="control-label text-bold"  Text="Project Contribution"></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_montoProyecto" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div> 
  

                                                                  <div class="row hidden">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_montoTotal" runat="server"  CssClass="control-label text-bold"  Text="Total Amount "></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        
                                                                        <asp:Label ID="lbl_montoTotal" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div> 

                                                                <div class="row hidden">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_tasaCambio" runat="server"  CssClass="control-label text-bold"  Text="Exchange rate"></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->                                                                        
                                                                         <asp:Label ID="lbl_tasaCambio" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>                                                                                      
                                        
                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->  
                                                                          <asp:Label ID="lblt_instrument_number" runat="server"  CssClass="control-label text-bold"  Text="Instrument Number"></asp:Label>                                                  
                                                                    
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_instrumento" runat="server"></asp:Label>
                                                   
                                                                   </div>
                                                                 </div>
                                                                
                                            
                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_beneficiary" runat="server"  CssClass="control-label text-bold"  Text="Beneficiary Name"></asp:Label></div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                         <asp:Label ID="lbl_beneficiario" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>
                                                   
                                                                  <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Datedcreated" runat="server"  CssClass="control-label text-bold" Text="Date Created"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_datecreated" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>


                                                               <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->                                                    
                                                                           <asp:Label ID="lblt_dateApproved" runat="server"  CssClass="control-label text-bold" Text="Date Approved"></asp:Label>                                                                                                                            
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_dateapproved" runat="server" style="font-weight: 700" ></asp:Label >
                                                                       <asp:Label ID="lbl_ErrApprovedBy" runat="server" CssClass="control-label text-bold"  ToolTip="Date of receipt- Pending" ></asp:Label>
                                                   
                                                                   </div>
                                                               </div>
                                                                                                                   
                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_createdBy" runat="server"  CssClass="control-label text-bold" Text="Created By"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                        <asp:Label ID="lbl_createdby" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>
                                                               

                                                               <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_approvedBy" runat="server"  CssClass="control-label text-bold" Text="Approved By"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                           <asp:Label ID="lbl_approvedby" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="tblt_status" runat="server"  CssClass="control-label text-bold" Text="Estado"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-7">
                                                                       <!--Control -->
                                                                             <asp:Label ID="lbl_status2" runat="server" style="font-weight: 700"></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                <div class="box-body">
                                                                    <div class=" row">
                                                                        <div class="col-sm-2 text-left">
                                                                         <!--Tittle -->                                           
                                                                            <asp:Label ID="lblbt_comment" runat="server"  CssClass="control-label text-bold" Text="Description"></asp:Label>
                                                                        </div>
                                                                       <div class="col-sm-10">
                                                                           <!--Control -->
                                                                           <asp:Label ID="lbl_Comment" runat="server" CssClass="text-justify "  Width="90%"></asp:Label>
                                                                       </div>
                                                                    </div>
                                                                  </div>

                                                       
                                                                   <div class="box-body"> <!--File Box -->
                                                                        <div class="row">
                                                                            <div class="col-sm-2 text-left">
                                                                             <!--Tittle -->
                                                                                  <asp:Label ID="lblt_Documents" runat="server"  CssClass="control-label text-bold"   Text="Documents"></asp:Label><br />
                                                                            </div>
                                                                           <div class="col-sm-10">
                                                                               <!--Control -->
                                                                           </div>

                                                                                <div class="col-sm-12 "><!--Grid-->

                                                                                        <telerik:RadGrid ID="grd_Document" runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Office2010Blue" ShowHeader="True">
                                                                                                    <HeaderContextMenu >
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
                                                                                                             <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter archivo column"
                                                                                                                     HeaderText="No" SortExpression="Number" UniqueName="Number">
                                                                                                             </telerik:GridBoundColumn>
                                                                                                               
                                                                                                                 <telerik:GridBoundColumn DataField="archivo_NombreCorto" FilterControlAltText="Filter archivo column"
                                                                                                                     HeaderText="Soporte" SortExpression="archivo_NombreCorto" UniqueName="archivo_NombreCorto">
                                                                                                                 </telerik:GridBoundColumn>
                                                                                                                 <telerik:GridBoundColumn DataField="nombre_documento" FilterControlAltText="Filter nombre_documento column"
                                                                                                                     HeaderText="Tipo de documento" SortExpression="nombre_documento" UniqueName="nombre_documento">
                                                                                                                 </telerik:GridBoundColumn>

                                                                                                                 <telerik:GridBoundColumn DataField="ver" 
                                                                                                                      FilterControlAltText="Filter ver column" HeaderText="Rev" 
                                                                                                                    SortExpression="ver" UniqueName="ver">
                                                                                                                    </telerik:GridBoundColumn>
                                                                                                                 <telerik:GridBoundColumn DataField="nombre_rol" 
                                                                                                                    FilterControlAltText="Filter nombre_rol column" HeaderText="Rol Rev" 
                                                                                                                    SortExpression="nombre_rol" UniqueName="nombre_rol">
                                                                                                                 </telerik:GridBoundColumn>

                                                                                                                 <telerik:GridBoundColumn DataField="extension" FilterControlAltText="Filter nombre_documento column"
                                                                                                                     HeaderText="Tipo" SortExpression="extension" UniqueName="extension">
                                                                                                                 </telerik:GridBoundColumn>
                                                                                                                 <telerik:GridBoundColumn DataField="archivo" FilterControlAltText="Filter fecha_recepcion column"
                                                                                                                     HeaderText="archivo" SortExpression="archivo" UniqueName="archivo" Visible ="true" Display="false"> 
                                                                                                                 </telerik:GridBoundColumn>
                                                                                                                 <telerik:GridTemplateColumn UniqueName="AttachFile" FilterControlAltText="Filter Completo column">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:HyperLink ID="Attach" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png" ToolTip ="Download document"></asp:HyperLink>
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


                                                                                </div>
                                                                        </div>
                                                                  </div>  <!--File Box -->
                                                                                                                     
                                                                   <div class="box-body"><!--Comment Box -->
                                                                        <div class="row">
                                                                           
                                                                            <div class="col-sm-6 text-left">
                                                                             <!--Tittle -->
                                                                                 <asp:Label ID="lblt_commentOnApproval" runat="server" CssClass="control-label text-bold"  Text="Comments on the approval process"></asp:Label><br />
                                                                            </div>
                                                                            <div class="col-sm-5">
                                                                               <!--Control -->
                                                                            </div>

                                                                           <div class="col-sm-12 "><!--Grid-->
                                                                               
                                                                               <telerik:RadGrid ID="grd_comment" runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" AutoGenerateColumns="False" 
                                                                                        CellSpacing="0" GridLines="None" Skin="Office2010Blue"  Width="90%" >
                                                                               <HeaderContextMenu >
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
                                                    
                                                                                                              <telerik:GridTemplateColumn UniqueName="codAction" FilterControlAltText="Filter codAction column">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:HyperLink ID="imgEvent" runat="server" ImageUrl="~/Imagenes/iconos/ray.png"
                                                                                                                        ToolTip="Message created by">
                                                                                                                    </asp:HyperLink>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridBoundColumn DataField="step" FilterControlAltText="Filter step column"
                                                                                                                     HeaderText="Paso" SortExpression="step" UniqueName="step">
                                                                                                             </telerik:GridBoundColumn>
                                                                                                             <telerik:GridBoundColumn DataField="fecha_comentario" 
                                                                                                                     DataType="System.DateTime" 
                                                                                                                     FilterControlAltText="Filter fecha_comentario column" 
                                                                                                                     HeaderText="Fecha" SortExpression="fecha_comentario" 
                                                                                                                     UniqueName="fecha_comentario">
                                                                                                                     <ItemStyle Width="140px" />
                                                                                                                 </telerik:GridBoundColumn>
                                                                                                              <telerik:GridTemplateColumn UniqueName="Flag" FilterControlAltText="Filter Flag column">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:HyperLink ID="imgFlag" runat="server" ImageUrl="~/Imagenes/Iconos/flag_yellow.png">
                                                                                                                    </asp:HyperLink>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                             <telerik:GridBoundColumn DataField="descripcion_estado"  FilterControlAltText="Filter descripcion_estado column"
                                                                                                                     HeaderText="Estado" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                                                                             </telerik:GridBoundColumn>
                                                                                                             <telerik:GridBoundColumn DataField="empleado" 
                                                                                                                     FilterControlAltText="Filter empleado column" HeaderText="Usuario" 
                                                                                                                     ReadOnly="True" SortExpression="empleado" UniqueName="empleado">
                                                                                                                     <ItemStyle Width="140px" />
                                                                                                             </telerik:GridBoundColumn>   
                                                                                                              <telerik:GridBoundColumn DataField="comentario" 
                                                                                                                     FilterControlAltText="Filter comentario column" HeaderText="Comentario" 
                                                                                                                     SortExpression="comentario" UniqueName="comentario">
                                                                                                                 </telerik:GridBoundColumn>
                                                         

                                                                                                                 <telerik:GridBoundColumn DataField="messAct" 
                                                                                                                     FilterControlAltText="Filter messAct column" 
                                                                                                                     HeaderText="messAct" SortExpression="messAct" 
                                                                                                                     UniqueName="messAct" Visible = "true" Display="false">
                                                                                                                 </telerik:GridBoundColumn>

                                                                                                                 <telerik:GridBoundColumn DataField="iconAct" 
                                                                                                                     FilterControlAltText="Filter iconAct column" 
                                                                                                                     HeaderText="iconAct" SortExpression="messAct" 
                                                                                                                     UniqueName="iconAct" Visible = "true" Display="false">
                                                                                                                </telerik:GridBoundColumn>

                                                                                                                 <telerik:GridBoundColumn DataField="nombre_rol" 
                                                                                                                     FilterControlAltText="Filter nombre_rol column" 
                                                                                                                     HeaderText="nombre_rol" SortExpression="nombre_rol" 
                                                                                                                     UniqueName="nombre_rol" Visible = "true" Display="false">
                                                                                                                </telerik:GridBoundColumn>

                                                                                                                <telerik:GridBoundColumn DataField="icon_msj" 
                                                                                                                     FilterControlAltText="Filter icon_msj column" 
                                                                                                                     HeaderText="icon_msj" SortExpression="icon_msj" 
                                                                                                                     UniqueName="icon_msj" Visible = "true" Display="false">
                                                                                                                </telerik:GridBoundColumn>

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
 

                                                                                 </div>
                                                                      
                                                                            
                                                                              </div>
                                                                    
                                                                        </div>  <!--Comment Box -->
                                                     

                                                                       <div class="box-body"><!--Write your Comment Box -->

                                                                        <div class="row">

                                                                                <div class="col-sm-2 text-left">
                                                                                 <!--Tittle -->
                                                                                     <asp:Label ID="lblt_comentarios" runat="server" CssClass="control-label text-bold"  Text="Write your comments: "></asp:Label>
                                                                                </div>

                                                                                <div class="col-sm-10">
                                                                                   <!--Control -->
                                                                                    <div style="text-align:right; padding-right:25px;">
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RadEditor1"  ForeColor="Red" ErrorMessage="Message required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                    <telerik:RadWindowManager runat="server" ID="RadWindowManager1" EnableShadow="true" ShowContentDuringLoad="false">
                                                                                    </telerik:RadWindowManager>
                                                                                </div>

                                                                                <div class="col-sm-12 " ><!--Editor-->
                                                                                           
                                                                                    <telerik:RadEditor runat="server" ID="RadEditor1" SkinID="Office2010Blue" Height="350" Width="90%" >
                                                                                        <ImageManager ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                            UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                                                                                            DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations">
                                                                                        </ImageManager>
                                                                                    </telerik:RadEditor>

                                                                                 </div>
                                                                      
                                                                            
                                                                           </div>
                                                                    
                                                                      </div>  <!--Write your Comment Box -->


                                                 </div>  <!--box bottom--><!--CONTROLS-->

                                           
                                           </div><!--box Group1-->                                                              
                    
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                        
                          
                       <telerik:RadButton ID="btn_guardar" runat="server" Text="    Send the comments   " Height="25px" Width="150px" OnClick="btn_guardar_Click"  Enabled="false" ValidationGroup="1" ></telerik:RadButton><br /><br />
                       <asp:Label ID="lblError" runat="server" Font-Names="Arial" ForeColor="#C00000" Visible="False"></asp:Label>
                                           


                   </div>

                </div>
           </section>
      
                 
               
     
        
    </asp:Content>

