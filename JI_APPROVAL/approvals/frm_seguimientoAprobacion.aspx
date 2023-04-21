<%@ Page Language="VB" MasterPageFile="../MasterSIM.master" AutoEventWireup="false" Inherits="approval.frm_seguimientoAprobacion" title="SAP" Codebehind="frm_seguimientoAprobacion.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        
        <table cellpadding="0" cellspacing="0" style="width: 960px;">
            <tr>
                <td>
                   
               <table cellpadding="0" style="width: 900px;  border-collapse: collapse; border: 1px solid #9A7328">
               <tr>
                <td style="font-weight: bold; background-color: #C2B88B">
                   
                   
                        &nbsp;&nbsp;</td>
            </tr>
               <tr>
                <td style="font-weight: bold; background-color: #C2B88B">
                   
                   
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="12px" 
                            Text="TRACKING TABLE"></asp:Label>
                                &nbsp;<asp:Label ID="lblIDocumento0" runat="server" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000">(</asp:Label>
                                <asp:Label ID="lblIDocumento" runat="server" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000"></asp:Label>
                                <asp:Label ID="lblIDocumento1" runat="server" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000">)</asp:Label>
                        <hr />
                   
                   </td>
            </tr>
               <tr>
                <td style="font-weight: bold">
                   
                   
            <table style="width: 904px" >
                <tr >
                    <td 
                        style="text-align: right; width: 168px; vertical-align: top;">
                        &nbsp;</td>
                    <td style="width: 5px; vertical-align: top;" >
                        &nbsp;</td>
                    <td style="width: 769px; vertical-align: top; text-align: right;" >
                        <asp:Label ID="lbl_IdCodigoAPP" runat="server" style="font-weight: 700" 
                            Font-Bold="True" Font-Size="14px"></asp:Label>
                        <asp:Label ID="lbl_categoria1" runat="server" style="font-weight: 700" 
                            Font-Bold="True" Font-Size="14px">/</asp:Label>
                        <asp:Label ID="lbl_IdCodigoSAP" runat="server" style="font-weight: 700" 
                            Font-Bold="True" Font-Size="14px"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td 
                        style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label2" runat="server" style="font-weight: 700" 
                            Text="Name of Category"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px;" >
                        <asp:Label ID="lbl_categoria" runat="server" style="font-weight: 700" 
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td 
                        style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label5" runat="server" style="font-weight: 700" 
                            Text="Approvals"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px;" >
                        <asp:Label ID="lbl_aprobacion" runat="server" style="font-weight: 700" 
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td 
                        style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label11" runat="server" style="font-weight: 700" 
                            Text="Approval level required"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px;" >
                        <asp:Label ID="lbl_nivelaprobacion" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td 
                        style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label12" runat="server" style="font-weight: 700" 
                            Text="Condition"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px;" >
                        <asp:Label ID="lbl_condicion" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label6" runat="server" style="font-weight: 700" 
                            Text="Name of Process"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_proceso" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px;">
                        <asp:Label ID="Label7" runat="server" style="font-weight: 700" 
                            Text="Code of Process"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_codigo" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px;">
                        <asp:Label ID="Label8" runat="server" style="font-weight: 700" 
                            Text="Instrument Number"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_instrumento" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px;">
                        <asp:Label ID="Label9" runat="server" style="font-weight: 700" 
                            Text="Beneficiary name"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_beneficiario" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px;">
                        <asp:Label ID="Label13" runat="server" style="font-weight: 700" 
                            Text="Date created"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_datecreated" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px; vertical-align: top;">
                        <asp:Label ID="Label10" runat="server" style="font-weight: 700" 
                            Text="Comment"></asp:Label>
                    </td>
                    <td style="width: 5px; vertical-align: top;" >
                        :</td>
                    <td style="width: 769px; text-align: left;">
                        <asp:Label ID="lbl_Comment" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td  style="text-align: right; width: 168px;">
                         &nbsp;</td>
                    <td style="width: 5px">
                        &nbsp;</td>
                    <td style="width: 769px; text-align: left;">
                                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                 ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                 
                            
                                                 SelectCommand="SELECT * FROM dbo.FN_Ta_RutaSeguimiento(11) " 
                                                 
                                                 
                                                 UpdateCommand="UPDATE ta_categoria SET descripcion_cat = @descripcion_cat WHERE (id_categoria = @id_categoria)">
                                                 <UpdateParameters>
                                                     <asp:Parameter Name="descripcion_cat" />
                                                     <asp:Parameter Name="id_categoria" />
                                                 </UpdateParameters>
                                             </asp:SqlDataSource>
                        
                        
                        
                        
                        </td>
                </tr>
                <tr >
                    <td  style="text-align: left; " colspan="3">
                                         <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" 
                                             CellSpacing="0" Culture="Spanish (Spain)" DataSourceID="SqlDataSource2" 
                                             GridLines="None" Skin="Sunset" Width="940px" AllowAutomaticUpdates="True"  
                                             AutoGenerateColumns="False">
<HeaderContextMenu>
<WebServiceSettings>
<ODataSettings InitialContainerName=""></ODataSettings>
</WebServiceSettings>
</HeaderContextMenu>
<ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
<Selecting AllowRowSelect="True"></Selecting>
        </ClientSettings>
<MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" 
            FilterControlAltText="Filter orden column" HeaderText="Id" 
            SortExpression="orden" UniqueName="orden">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="nombre_rol" 
            FilterControlAltText="Filter nombre_rol column" HeaderText="Unit" 
            SortExpression="nombre_rol" UniqueName="nombre_rol">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="nombre_empleado" 
            FilterControlAltText="Filter nombre_empleado column" 
            HeaderText="User" SortExpression="nombre_empleado" 
            UniqueName="nombre_empleado">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="fecha_recepcion"
            FilterControlAltText="Filter fecha_recepcion column" 
            HeaderText="Date of receipt" SortExpression="fecha_recepcion" 
            UniqueName="fecha_recepcion">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="fecha_limite" 
            FilterControlAltText="Filter fecha_limite column" HeaderText="Deadline" 
            SortExpression="fecha_limite" UniqueName="fecha_limite">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="fecha_aprobacion" 
            FilterControlAltText="Filter fecha_aprobacion column" 
            HeaderText="Date of approval" SortExpression="fecha_aprobacion" 
            UniqueName="fecha_aprobacion">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn  DataField="duracionChar" 
            FilterControlAltText="Filter duracionChar column" 
            HeaderText="Duration" SortExpression="duracionChar" 
            UniqueName="duracionChar">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="NDiasFaltantes" 
            FilterControlAltText="Filter NDiasFaltantes column" HeaderText="Remaining days" 
            SortExpression="NDiasFaltantes" UniqueName="NDiasFaltantes">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="NDiasProceso" 
            FilterControlAltText="Filter NDiasProceso column" HeaderText="Days in progress" 
            SortExpression="NDiasProceso" UniqueName="NDiasProceso">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="descripcion_estado" 
            FilterControlAltText="Filter descripcion_estado column" 
            HeaderText="State" SortExpression="descripcion_estado" 
            UniqueName="descripcion_estado">
        </telerik:GridBoundColumn>
          <telerik:GridButtonColumn ConfirmText="Would you like to resend this notification?" ConfirmDialogType="RadWindow"
            ConfirmTitle="Forward Alert" ButtonType="ImageButton" CommandName="Edit" Text ="Resend Notification" ConfirmDialogHeight="100px"
            ConfirmDialogWidth="400px" UniqueName="FORWARD" ImageUrl="~/Imagenes/iconos/resend_email.png"  >
           <ItemStyle Width="10px" />
           </telerik:GridButtonColumn>

        <telerik:GridBoundColumn DataField="Alerta" Visible ="false" 
            FilterControlAltText="Filter Alerta column" HeaderText="Alert" 
            SortExpression="Alerta" UniqueName="Alerta">
        </telerik:GridBoundColumn>
          <telerik:GridTemplateColumn UniqueName="Completo">
          <ItemTemplate>
          <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/iconos/alerta.png" 
             ToolTip="Indicador Incompleto">
             </asp:HyperLink>
            </ItemTemplate>
         </telerik:GridTemplateColumn> 

         <telerik:GridBoundColumn  DataField="id_App_Documento" 
            FilterControlAltText="Filter id_App_Documento column" 
            HeaderText="idApp" SortExpression="id_App_Documento" 
            UniqueName="id_App_Documento" Visible="false">
        </telerik:GridBoundColumn>

         <telerik:GridBoundColumn  DataField="id_estadoDoc" 
            FilterControlAltText="Filter id_estadoDoc column" 
            HeaderText="idEstadoDoc" SortExpression="id_estadoDoc" 
            UniqueName="id_estadoDoc" Visible="false">
        </telerik:GridBoundColumn>

        <telerik:GridBoundColumn  DataField="Observacion" 
            FilterControlAltText="Filter Observacion column" 
            HeaderText="Observacion" SortExpression="Observacion" 
            UniqueName="Observacion" Visible="false">
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
                    </td>
                </tr>
                <tr>
                  <td colspan="3">
                     <asp:Label ID="lblMsg" runat="server" Font-Size="12px"  Text="No messages"></asp:Label>
                  </td>
                </tr>
                <tr>
                    <td  style="text-align: right; width: 168px;">
                        &nbsp;</td>
                    <td style="width: 5px; vertical-align: top;" >
                        &nbsp;</td>
                    <td style="width: 769px; text-align: left;">
                        &nbsp;</td>
                </tr>
                <tr >
                    <td  style="text-align: right; " colspan="3">
                                             <telerik:RadScheduler ID="RadScheduler2" runat="server" AllowDelete="False" 
                                                 AllowEdit="False" AllowInsert="False" 
                                                 
                                                 CustomAttributeNames="nombre_empleado,descripcion_estado,Alerta,NDiasFaltantes" DataDescriptionField="descripcion_estado" 
                                                 DataEndField="fecha_aprobacionDTPart" DataKeyField="orden" 
                                                 DataSourceID="SqlDataSource1" DataStartField="fecha_aprobacionDTPart" 
                                                 DataSubjectField="USR_Observacion" DisplayDeleteConfirmation="False" 
                                                 EnableDescriptionField="True" FirstDayOfWeek="Monday"
                                                 SelectedView="MonthView" Skin="Sunset" Width="940px" Height="704px" 
                                                 Culture="Spanish (Spain)">
                                                 <AppointmentTemplate>
                                                 <div style="font-weight:bold; height:inherit">
                                                    <img src="../Imagenes/Circle_<%#Eval("Alerta")%>.png" height="16" width="16" /> <%#Eval("descripcion_estado")%> &nbsp;&nbsp;(<%#Eval("NDiasFaltantes")%>)
                                                 </div>
                                                 </AppointmentTemplate>
                                                 <WeekView UserSelectable="False" />
                                                 <ResourceStyles>
                                                     <telerik:ResourceStyleMapping ApplyCssClass="" BackColor="Cyan" Key="ID" 
                                                         Text="1" Type="Calendar" />
                                                 </ResourceStyles>

<WebServiceSettings>
<ODataSettings InitialContainerName=""></ODataSettings>
</WebServiceSettings>
                                             <DayView UserSelectable="False" />
                                             </telerik:RadScheduler>
                        
                        
                        
                        
                        </td>
                </tr>
                <tr >
                    <td  style="text-align: right; " colspan="3">
                                             &nbsp;</td>
                </tr>
                </table>       
                   
                   
                   
                   </td>
            </tr>
               <tr>
                <td style="text-align: right">
                   
                   
                    <hr />
                        &nbsp;
                                           
                   
                    <br />
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    
        
                        SelectCommand="SELECT * FROM dbo.FN_Ta_RutaSeguimiento(11) where fecha_recepcionDT IS NOT NULL">
                </asp:SqlDataSource>
                   
                   
                   
                   </td>
            </tr>
        </table> 
                </td>
            </tr>
            <tr>
               <td>
                   <div class="art-footer">
                         <div class="art-footer-text">
                            <p style="font-size:x-small"> Rev 0.0.0.1 </p> 
                            <p style="font-size:x-small">By Chemonics inc. Colombia</p>
                        </div>
                    </div>
               </td>
            </tr>
        </table>
    
    
        
    
    
     
        &nbsp;</p>
    </asp:Content>

