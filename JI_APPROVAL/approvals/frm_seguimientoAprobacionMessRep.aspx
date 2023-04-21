<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPopUp.Master"  Inherits="RMS_APPROVAL.frm_seguimientoAprobacionMessRep" Codebehind="frm_seguimientoAprobacionMessRep.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


  <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
        
  <div class="row invoice-info">     
      <div class="col-md-12"> 
               
         <div class="panel-group">

                      <div class="panel panel-default">
                        <div class="panel-heading"><h3><asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVAL SYSTEM</asp:Label></h3></div>
                      </div>

                      <div class="panel panel-default">

                             <table class="table table-responsive table-condensed box box-primary">                                
                                <tr class="box box-success">
                                    <td colspan="3" class="text-center" >                                   
                                        <h4><asp:Label ID="lbl_nameProc" runat="server"></asp:Label></h4>                                   
                                    </td>
                                </tr>  
                              </table>

                            <table class="table table-responsive table-condensed box box-primary">
                              <tr >

                                 <td colspan="2" >
                                   <asp:Label ID="lblIDocumento" runat="server"  CssClass="control-label text-bold" Text="" Visible="false"></asp:Label>                                                        
                                   <asp:Label ID="lblIdRuta" runat="server"  CssClass="control-label text-bold" Text="" Visible="false"></asp:Label>
                                   <asp:HiddenField runat="server" ID="h_Filter" Value="0" />  
                                </td>
                              </tr>
                             <%-- <tr>
                                <td colspan="2" class="text-right" style="padding-right:4em;">
                                  <asp:Label ID="lbl_IdCodigoAPP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                  <asp:Label ID="lbl_categoria1" runat="server"  CssClass="control-label text-bold">/</asp:Label>
                                  <asp:Label ID="lbl_IdCodigoSAP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                </td>
                              </tr>--%>
                              <%--<tr>
                                 <td colspan="2" class="text-left">
                                  
                                        <telerik:RadGrid ID="grd_cate" 
                                            runat="server" 
                                            AllowAutomaticDeletes="True" 
                                            Skin="Office2010Blue" 
                                            AllowAutomaticUpdates ="True" 
                                            AutoGenerateColumns="False" 
                                            CellSpacing="0"
                                            GridLines="None" 
                                            Width="95%" ShowHeader="False">
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
                                                      <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column" HeaderText="Id" SortExpression="orden" UniqueName="orden">                                                          
                                                      </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column" HeaderText="Unit" SortExpression="nombre_rol" UniqueName="nombre_rol">
                                                           <HeaderStyle CssClass="WithColumnDecrease10" />
                                                          <ItemStyle CssClass="WithColumnDecrease10" />
                                                      </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column" HeaderText="User" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                           <HeaderStyle CssClass="WithColumnDecrease40" />
                                                          <ItemStyle CssClass="WithColumnDecrease40" />
                                                      </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"  HeaderText="State" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                          <HeaderStyle CssClass="WithColumnDecrease10" />
                                                          <ItemStyle CssClass="WithColumnDecrease10" />
                                                      </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn DataField="fecha_aprobacion" FilterControlAltText="Filter fecha_aprobacion column"  HeaderText="Date of approval" SortExpression="fecha_aprobacion" UniqueName="fecha_aprobacion">
                                                          <HeaderStyle CssClass="WithColumnDecrease20" />
                                                          <ItemStyle CssClass="WithColumnDecrease20" />
                                                      </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn DataField="Alerta" FilterControlAltText="Filter Alerta column" HeaderText="Alert" SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">                                                          
                                                      </telerik:GridBoundColumn>
                                                      <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                        <ItemTemplate>
                                                          <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png" ToolTip="Indicador Incompleto">
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
                                  </td>
                              </tr> class="box box-success" --%>  
                              <tr >
                                 <td style="width:15%" class="text-left">
                                       <!--Tittle -->
                                     <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                 </td>
                                 <td>
                                       <!--Control -->
                                     <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>                                                                                                         
                                </td>
                            </tr>   
                               <tr>
                                    <td style="width:15%" class="text-left">
                                      <!--Tittle -->
                                     <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Approvals"></asp:Label>                                                                   
                                    </td>
                                    <td>
                                      <!--Control -->
                                     <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>                                                                                                                                                                                          
                                    </td>
                                </tr>
                                <tr>                                     
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                      <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>
                                    </td>
                                    <td>                                     
                                       <!--Control -->
                                       <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>                                                                        
                                    </td>
                            </tr>
                            <tr>
                                <td style="width:15%" class="text-left">
                                  <!--Tittle -->
                                  <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                </td>
                                <td>                                 
                                  <!--Control -->
                                  <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>                                                                                                          
                                </td>
                            </tr>
                             <tr>
                                 <td style="width:15%" class="text-left">
                                   <!--Tittle -->
                                    <asp:Label ID="lblt_tipoDocumento" runat="server"  CssClass="control-label text-bold" Text="Document Type"></asp:Label>                                                                                       
                                  </td>  
                                <td>
                                    <!--Control -->
                                    <asp:Label ID="lbl_tipoDocumento" runat="server" ></asp:Label>                 
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%" class="text-left">
                                     <!--Tittle -->
                                      <asp:Label ID="lblt_proccess_name" runat="server"   CssClass="control-label text-bold" Text="Name of Process"></asp:Label>
                                </td>
                                <td>                                     
                                      <!--Control -->
                                      <asp:Label ID="lbl_proceso" runat="server" style="font-weight: 700"></asp:Label>
                                 </td>
                            </tr>
                             <tr>
                                <td style="width:15%" class="text-left">
                                   <!--Tittle -->                                                    
                                  <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Code of Process"></asp:Label>                    
                                </td>
                                <td>                                                                   
                                  <!--Control -->                                                                   
                                  <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                  <td style="width:15%" class="text-left">
                                       <!--Tittle -->
                                    <asp:Label ID="lblt_region" runat="server"  CssClass="control-label text-bold"  Text="Region"></asp:Label>
                                  </td> 
                                <td>                                   
                                    <!--Control -->
                                    <asp:Label ID="lbl_region" runat="server" ></asp:Label>                                                                                             
                                </td>
                            </tr>
                            <tr>
                                 <td style="width:15%" class="text-left">
                                      <!--Tittle -->
                                    <asp:Label ID="lblt_montoProyecto" runat="server"  CssClass="control-label text-bold"  Text="Project Contribution"></asp:Label>
                                    
                                 </td>
                                <td>
                                   <!--Control -->
                                    <asp:Label ID="lbl_montoProyecto" runat="server" style="font-weight: 700"></asp:Label>
                                                                                              
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%" class="text-left">
                                    <!--Tittle -->
                                    <asp:Label ID="lblt_montoTotal" runat="server"  CssClass="control-label text-bold"  Text="Total Amount "></asp:Label>
                                </td>
                                <td>                                    
                                    <!--Control -->
                                    <asp:Label ID="lbl_montoTotal" runat="server" style="font-weight: 700"></asp:Label>                                                                                       
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%" class="text-left">
                                    <!--Tittle -->
                                      <asp:Label ID="Label1" runat="server"  CssClass="control-label text-bold"  Text="Exchange rate"></asp:Label>
                                </td>
                                <td>
                                     <!--Tittle -->
                                      <asp:Label ID="lblt_tasaCambio" runat="server"  CssClass="control-label text-bold"  Text="Exchange rate"></asp:Label>
                                      <!--Control -->                                                                        
                                      <asp:Label ID="lbl_tasaCambio" runat="server" style="font-weight: 700"></asp:Label>
                                                                                             
                                </td>
                            </tr>
                              <tr>
                                <td style="width:15%" class="text-left">
                                     <!--Tittle -->  
                                    <asp:Label ID="lblt_instrument_number" runat="server"  CssClass="control-label text-bold"  Text="Instrument Number"></asp:Label>
                                </td>
                                <td>                                   
                                    <!--Control -->
                                    <asp:Label ID="lbl_instrumento" runat="server"></asp:Label>
                                                   
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%" class="text-left">
                                  <!--Tittle -->
                                   <asp:Label ID="lblt_beneficiary" runat="server"  CssClass="control-label text-bold"  Text="In reference to"></asp:Label>
                                </td>
                                <td>                                   
                                   <!--Control -->
                                   <asp:Label ID="lbl_beneficiario" runat="server" style="font-weight: 700"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="width:15%" class="text-left">
                                    <!--Tittle -->
                                   <asp:Label ID="lblt_Datedcreated" runat="server"  CssClass="control-label text-bold" Text="Date Created"></asp:Label>
                                </td>  
                                <td>                                  
                                   <!--Control -->
                                   <asp:Label ID="lbl_datecreated" runat="server" style="font-weight: 700"></asp:Label>                                                                                              
                                </td>
                               </tr>
                               <tr>
                                <td style="width:15%" class="text-left">
                                     <!--Tittle -->                                                    
                                    <asp:Label ID="lblt_dateApproved" runat="server"  CssClass="control-label text-bold" Text="Date Approved"></asp:Label>                                                                                          
                                </td>
                                <td><!--Control -->
                                    <asp:Label ID="lbl_dateapproved" runat="server"  style="font-weight: 700" ></asp:Label>
                                    <asp:Label ID="lbl_ErrApprovedBy" runat="server" CssClass="control-label text-bold"  ToolTip="Date of receipt- Pending"></asp:Label>
                                </td>
                                </tr>
                                <tr>
                                    <td style="width:15%" class="text-left">
                                        <!--Tittle -->
                                        <asp:Label ID="lblt_createdBy" runat="server"  CssClass="control-label text-bold" Text="Created By"></asp:Label>
                                    </td>
                                    <td>                                        
                                        <!--Control -->
                                        <asp:Label ID="lbl_createdby" runat="server" ></asp:Label>                                                                                              
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%" class="text-left">
                                         <!--Tittle -->
                                         <asp:Label ID="lblt_approvedBy" runat="server"  CssClass="control-label text-bold" Text="Approver"></asp:Label>
                                    </td>
                                    <td>                                        
                                         <!--Control -->
                                         <asp:Label ID="lbl_approvedby" runat="server" style="font-weight: 700"></asp:Label>
                                    </td>
                                </tr>             
                                <tr>
                                     <td style="width:15%" class="text-left">
                                       <!--Tittle -->
                                       <asp:Label ID="tblt_status" runat="server"  CssClass="control-label text-bold" Text="Status"></asp:Label>
                                     </td>
                                    <td>                                      
                                       <!--Control -->
                                       <asp:Label ID="lbl_status" runat="server" style="font-weight: 700"></asp:Label>

                                    </td>
                                </tr>
                               <%-- <tr class="box box-success">                                   
                                    <td class="text-left" colspan="2">
                                      <!--Tittle -->                                           
                                      <asp:Label ID="lblbt_comment" runat="server"  CssClass="control-label text-bold" Text="Comment"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                         <!--Control -->
                                         <asp:Label ID="lbl_Comment" runat="server" CssClass="text-justify "  Width="90%"></asp:Label>                                                                                                       
                                    </td>
                                </tr>--%>
                                <tr class="box box-success">
                                    <td  class="text-left"  colspan="2">
                                       <asp:Label ID="lblt_Documents" runat="server"  CssClass="control-label text-bold"   Text="Documents"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  colspan="2" class="text-left">
                                         <telerik:RadGrid ID="grd_Document" 
                                             runat="server" 
                                             AllowAutomaticDeletes="True" 
                                             AllowAutomaticUpdates ="True" 
                                             AutoGenerateColumns="False" 
                                             CellSpacing="0" 
                                             GridLines="None" 
                                             Skin="Office2010Blue" 
                                             Width="95%" 
                                             ShowHeader="False">
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
                                          HeaderText="Number" SortExpression="Number" UniqueName="Number">
                                          </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn DataField="archivo_NombreCorto" FilterControlAltText="Filter archivo column"
                                           HeaderText="Archive" SortExpression="archivo_NombreCorto" UniqueName="archivo_NombreCorto">
                                               <HeaderStyle CssClass="WithColumnDecrease40" />
                                               <ItemStyle CssClass="WithColumnDecrease40" />
                                           </telerik:GridBoundColumn>
                                           <telerik:GridBoundColumn DataField="nombre_documento" FilterControlAltText="Filter nombre_documento column"
                                           HeaderText="Document Type" SortExpression="nombre_documento" UniqueName="nombre_documento">
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
                                             HeaderText="Type" SortExpression="extension" UniqueName="extension">
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="archivo" FilterControlAltText="Filter fecha_recepcion column"
                                             HeaderText="archivo" SortExpression="archivo" UniqueName="archivo" Visible ="true" Display="false"> 
                                             </telerik:GridBoundColumn>
                                             <telerik:GridTemplateColumn UniqueName="AttachFile" FilterControlAltText="Filter Completo column">
                                             <ItemTemplate>
                                             <asp:HyperLink ID="Attach" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png" ToolTip ="Download document"></asp:HyperLink>
                                             </ItemTemplate>
                                                 <HeaderStyle CssClass="additionalColumn" />
                                                 <ItemStyle CssClass="additionalColumn" />
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
                                        
                                     <br />  

                                    </td>
                                </tr>  
                                
                                  <tr class="box box-success">
                                    <td  class="text-left"  colspan="2">
                                      <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                        <div class="box-header">
                                           <i class="fa fa-comments-o"></i>
                                           <h3 class="box-title">Comments</h3>                                              
                                        </div>
                                        
                                    </td>
                                  </tr>
                                <tr>
                                    <td  colspan="2" class="text-left">
                                        <br />                                    
                                         <%-- <div class="direct-chat-messages">--%>

                                                     <asp:Repeater ID="rept_msgApproval" runat="server">
                                                            <ItemTemplate>
                                                                  <div class="direct-chat-msg <%# Eval("align1")%> "  >
                                                                      <div class="direct-chat-info clearfix">
                                                                       <i class="fa <%# Eval("fa_icon")%> fa-2x <%# Eval("align2")%> " aria-hidden="true" title="<%# Eval("icon_message")%>"></i>&nbsp;&nbsp;&nbsp; <span class="direct-chat-name  <%# Eval("align2")%> "><%# Eval("empleado")%></span>
                                                                        <span class="direct-chat-timestamp  <%# Eval("align3") %> "> <%# getFecha(Eval("fecha_comentario"), "m", False) %> <i class="fa fa-clock-o"></i> <%#  getHora(Eval("fecha_comentario")) %> </span>
                                                                      </div><!-- /.direct-chat-info -->
                                                                       <img class="direct-chat-img <%# Eval("bColor")%> " src="<%# Eval("userImagen")%>" alt="<%# Eval("empleado")%>"><!-- /.direct-chat-img -->
                                                                      <div class="direct-chat-text">
                                                                        <%# Eval("comentario")%>
                                                                      </div><!-- /.direct-chat-text -->
                                                                    </div><!-- /.direct-chat-msg -->                                                                 
                                                            </ItemTemplate>
                                                        </asp:Repeater>                 
                                            <%--  </div><!--/.direct-chat-messages-->--%>                                       
                                           
                                    </td>
                                </tr>   
                                <tr>
                                    <td colspan="2" class="text text-center" >                                                          
                                       <asp:LinkButton ID="bntlk_print" runat="server" Width="10%"   CssClass="btn btn-sm btn-default additionalColumn" OnClientClick="javascript:window.print();" ToolTip="Print Report"  > <span class="icon ion-ios-printer"></span></asp:LinkButton>                                                               
                                    </td>
                                </tr>                                                                                                                                                              
                          </table>             
               
             </div>  <%--<div class="panel panel-default">--%>
             
         </div>    <%--<div class="panel-group">--%>

       </div> <%--<div class="col-md-12">--%>

     </div> <%--<div class="row invoice-info">--%>

 </asp:Content>

