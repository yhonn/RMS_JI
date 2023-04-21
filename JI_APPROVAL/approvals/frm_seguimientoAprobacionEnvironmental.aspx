<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPopUp.Master" Inherits="RMS_APPROVAL.frm_seguimientoAprobacionEnvironmental" Codebehind="frm_seguimientoAprobacionEnvironmental.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
      
            <div class="row invoice-info">
                
               <div class="col-md-12">
                
                 <div class="panel-group">

                      <div class="panel panel-default">
                        <div class="panel-heading"><h4><asp:Label runat="server" ID="lblt_titulo_pantalla">ENVIRONMENTAL APPROVAL</asp:Label></h4></div>
                      </div>

                      <div class="panel panel-default">
                            
                          <table class="table table-responsive table-condensed box box-primary">
                              <tr class="box box-success">
                               <td>
                                   <asp:Label ID="lblt_relatedApp" runat="server"  CssClass="control-label text-bold" Text="Related Approval:    "></asp:Label>                                                                                                                                          
                                   <asp:Label ID="lblIDocumento" runat="server"  CssClass="control-label text-bold" Text="" Visible="false"></asp:Label>
                                   <asp:HiddenField runat="server" ID="hd_Id_doc" value="0" />
                                    <asp:Label ID="lbl_instrumento" runat="server" style="font-weight: 700" ></asp:Label>&nbsp;&nbsp;-&nbsp;&nbsp;<asp:Label ID="lbl_proceso" runat="server" style="font-weight: 700"></asp:Label>
                               </td>
                              </tr>
                              <tr>
                                <td class="text-left"> 
                                                                         
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
                                                                                             
                                    <hr />                        


                                </td>
                              </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                        <asp:Label ID="lbl_categoria" runat="server" ></asp:Label> 
                                    </td>
                                </tr>

                                <tr>
                                  <td>
                                      <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Source Approvals"></asp:Label>                                
                                      <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>  
                                  </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>
                                        <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>                                                                                                            
                                    </td>
                                </tr>                                                                                                                       
                             <tr>
                                <td>
                                    <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                    <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>                                                                                                            
                                </td>
                            </tr> 
                              <tr>
                                <td>
                                   <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Code of Process"></asp:Label>                    
                                   <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>                                    
                                </td>
                            </tr>
                              <tr>
                                <td>
                                  <asp:Label ID="lblt_beneficiary" runat="server"  CssClass="control-label text-bold"  Text="Beneficiary Name"></asp:Label>
                                  <asp:Label ID="lbl_beneficiario" runat="server" style="font-weight: 700"></asp:Label>                                  
                                </td>
                              </tr>
                               <tr>
                                   <td>
                                      <asp:Label ID="lblt_dateApproved" runat="server"  CssClass="control-label text-bold" Text="Date Completed"></asp:Label>                                                                                                                            
                                      <asp:Label ID="lbl_dateapproved" runat="server"  ></asp:Label>   
                                   </td>
                               </tr>
                               <tr>
                                <td>
                                   <asp:Label ID="lblt_Documents" runat="server"  CssClass="control-label text-bold"   Text="Documents"></asp:Label>
                                </td>
                               </tr>
                                <tr>
                                    <td>
                                      <telerik:RadGrid ID="grd_Document" 
                                          runat="server" 
                                          AllowAutomaticDeletes="True" 
                                          AllowAutomaticUpdates ="True" 
                                          AutoGenerateColumns="False" 
                                          CellSpacing="0" 
                                          GridLines="None" 
                                          Skin="Office2010Blue" 
                                          Width="90%" 
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



                                    
                                    </td>
                                </tr>                                                                                   

                       </table>                    
                          

                 </div> <%-- <div class="panel panel-default">--%> 
                
                  <div class="panel panel-default">

                       <table class="table table-responsive table-condensed box box-primary">
                              <tr class="box box-success">
                               <td  class="col-sm-3 text-left" >
                                    <asp:Label ID="lblt_environmental_approved" runat="server"  CssClass="control-label text-bold" Text="Environmental Approval"></asp:Label>                                                                                                                                          
                                     <asp:Label ID="lblt_environmental_code" runat="server" style="font-weight: 700" ></asp:Label>
                               </td>
                              </tr>
                             <tr>
                               <td>
                                 <asp:Label ID="lbl_Estado" runat="server" CssClass="control-label text-bold" Text="Estatus"></asp:Label>
                                 <span runat="server" id="spn_state" class="label label-danger" visible="false"></span>          
                                 <span runat="server" id="spn_state_approved" class="label label-success " visible="false"></span>                                           
                               </td>
                            </tr>
                           <tr>
                               <td>                                     
                                    <asp:Label ID="lbl_dateUpdated" runat="server" CssClass="control-label text-bold" Text="Updated"></asp:Label>&nbsp;&nbsp
                                    <asp:Label ID="lbl_updated_user" runat="server" CssClass="control-label" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                      
                                    <asp:Label ID="lbl_updated_Date" runat="server" CssClass="control-label" Text=""></asp:Label>                                    
                               </td>
                            </tr>
                           <tr>
                                <td>
                                    <asp:Label ID="lblt_review_type" runat="server" CssClass="control-label text-bold" Text="Review Type"></asp:Label>
                                    <asp:Label ID="lbl_review_type" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                               <td>
                                  <asp:Label ID="lblt_observation" runat="server" CssClass="control-label text-bold" Text="Observation"></asp:Label>
                                  <asp:Label ID="lbl_observation" runat="server" CssClass="text-justify" ></asp:Label>                                                                                                              
                               </td>
                           </tr>
                           <tr>
                                <td>
                                    <asp:Label ID="lblt_FileAtth" runat="server"  CssClass="control-label text-bold"   Text="File Attachments"></asp:Label>
                                </td>
                            </tr>
                           <tr>
                            <td>
                                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Office2010Blue">
                                              </telerik:RadWindowManager>
                                               
                                                 <telerik:RadGrid ID="grd_archivos" 
                                                     runat="server" 
                                                     CellSpacing="0" GridLines="None" 
                                                     Width="95%" 
                                                     AllowAutomaticDeletes="True" 
                                                     AutoGenerateColumns="False" 
                                                     Skin="Office2010Blue" >
                                                   
                                                      <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                        <WebServiceSettings>
                                                            <ODataSettings InitialContainerName="">
                                                            </ODataSettings>
                                                        </WebServiceSettings>
                                                    </HeaderContextMenu>
                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                     <Selecting AllowRowSelect="True"></Selecting>
                                                 </ClientSettings>
                                                <MasterTableView
                                                    DataKeyNames="id_archivo_ambiental" >
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                        Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                        Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                       <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column" UniqueName="colm_ImageDownload" HeaderButtonType="PushButton" Visible="true" Display ="true" >
                                                           <ItemTemplate>
                                                               <asp:HyperLink ID="ImageDownload" runat="server">[ImageDownload]</asp:HyperLink>
                                                                <ItemStyle Width="5px" />                                                                  
                                                           </ItemTemplate>
                                                            <HeaderStyle Width="5px" />
                                                            <ItemStyle Width="1%"  />
                                                      </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="archivo" 
                                                            FilterControlAltText="Filter archivo column" 
                                                            HeaderText="Attachments" 
                                                            SortExpression="archivo" UniqueName="colm_archivos">
                                                            <ItemStyle Width="30%"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_documento" 
                                                            FilterControlAltText="Filter nombre_documento column" HeaderText="File Type" 
                                                            SortExpression="nombre_documento" UniqueName="colm_nombre_documento">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="ver" 
                                                            FilterControlAltText="Filter ver column" HeaderText="Rev" 
                                                            SortExpression="ver" UniqueName="colm_ver" Visible="true" Display="false"> 
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="usuario" 
                                                            FilterControlAltText="Filter usuario column" HeaderText="User" 
                                                            SortExpression="usuario" UniqueName="colm_usuario">
                                                            <ItemStyle Width="20%"  />
                                                        </telerik:GridBoundColumn>  
                                                        <telerik:GridBoundColumn DataField="fecha_creado" 
                                                            FilterControlAltText="Filter fecha_creado column" HeaderText="Date Created" 
                                                            SortExpression="fecha_creado" UniqueName="colm_usuario">
                                                        </telerik:GridBoundColumn>                                                                                                               
                                                        <telerik:GridBoundColumn DataField="id_archivo_ambiental" 
                                                            FilterControlAltText="Filter id_archivo_ambiental column" SortExpression="id_archivo" 
                                                            UniqueName="id_archivo_ambiental" Visible="True" Display="False" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="id_doc_soporte" 
                                                            FilterControlAltText="Filter id_doc_soporte column" 
                                                            SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" Visible="True" Display="False">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="id_documento_ambiental" 
                                                            FilterControlAltText="Filter id_documento_ambiental column" 
                                                            SortExpression="id_documento_ambiental" UniqueName="id_documento_ambiental" Visible="True" Display="False">
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
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <asp:LinkButton ID="bntlk_print" runat="server" Width="10%"  CssClass="btn btn-sm btn-default " OnClientClick="javascript:window.print();" ToolTip="Print Report"  > <span class="icon ion-ios-printer"></span></asp:LinkButton>                                                                                       
                        </td>
                    </tr>
                   </table> 
                      
                </div>  <%--<div class="panel panel-default">--%>
                                      
             </div> <%--<div class="panel-group">--%>
        
         </div> <%--<div class="col-md-12">--%>

    </div> <%--<div class="row invoice-info">--%>

 </asp:Content>

