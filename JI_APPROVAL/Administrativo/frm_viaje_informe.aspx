<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viaje_informe.aspx.vb" Inherits="RMS_APPROVAL.frm_viaje_informe" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .RadUpload_Office2007 .ruStyled .ruFileInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 input.ruFakeInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 .ruButton{
            border: 1px solid #e5e5e5;
            color: #767676;
            background-color: #fff;
            background-image:none
        }
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Legalización de viaje</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row" runat="server" visible="false" id="grdRutaViaje">
                            <div class="col-sm-12 text-left">
                                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" 
                                    AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0"
                                    GridLines="None" Width="100%" ShowHeader="True">
                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset">
                                        <WebServiceSettings>
                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                        </WebServiceSettings>
                                    </HeaderContextMenu>
                                    <MasterTableView >
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column"
                                                HeaderText="Id" SortExpression="orden" UniqueName="orden">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column"
                                                HeaderText="Rol" SortExpression="nombre_rol" UniqueName="nombre_rol">
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
                                            <telerik:GridTemplateColumn UniqueName="CompletoC" visible="false" FilterControlAltText="Filter Completo column">
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
                                <hr />
                            </div>    
                        </div>

                       <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                           <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="infoViaje" data-toggle="collapse" data-parent="#accordion" href="#collapseViaje" aria-expanded="false" aria-controls="collapseViaje">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseViaje"
                                            aria-expanded="false" aria-controls="collapseViaje" runat="server" id="a1">Información del viaje
                                        </a>
                                    </h4>
                                </div>
                                 <div id="collapseViaje" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoViaje">
                                    <div class="panel-body">
                                        <asp:HiddenField runat="server" ID="idViaje" Value="0" />
                                        <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />
                                        <asp:HiddenField runat="server" ID="esEdicion" Value="0" />

                                        <div class ="form-group row">
                                                <div class="col-sm-12 text-left ">
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Tipo de aprobación"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Código de aprobación"></asp:Label>                                
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Fecha de solicitud"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_solicitud" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_owner" runat="server"  CssClass="control-label text-bold"  Text="Fecha de inicio del viaje"></asp:Label></div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_inicio_viaje" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_NextApp" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización del viaje"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_finalizacion" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Numero de contacto"></asp:Label>                                                        
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_numero_contacto" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_proccess_name" runat="server" CssClass="control-label text-bold" Text="Motivo del viaje"></asp:Label> 
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_motivo_viaje" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>                                                              
                                                    
                                                </div> 
                                            </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Detalle itinerario</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_itinerario" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="false" 
                                                    ShowColumnFooters="false"
                                                    ShowGroupFooters="false"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_itinerario" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_viaje_itinerario"
                                                                FilterControlAltText="Filter id_viaje_itinerario column"
                                                                SortExpression="id_viaje_itinerario" UniqueName="id_viaje_itinerario"
                                                                Visible="False" HeaderText="id_viaje_itinerario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_viaje" DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_viaje column" HeaderStyle-Width="19%"
                                                                HeaderText="Fecha" SortExpression="fecha_viaje"
                                                                UniqueName="colm_fecha_viaje">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hora_salida"
                                                                FilterControlAltText="Filter hora_salida column" HeaderStyle-Width="19%"
                                                                HeaderText="Hora de salida" SortExpression="hora_salida"
                                                                UniqueName="colm_hora_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_origen" 
                                                                FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="19%"
                                                                HeaderText="Ciudad de origen" SortExpression="ciudad_origen"
                                                                UniqueName="colm_ciudad_origen">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad_destino"
                                                                FilterControlAltText="Filter ciudad_destino column" HeaderStyle-Width="19%"
                                                                HeaderText="Ciudad de destino" SortExpression="ciudad_destino"
                                                                UniqueName="colm_ciudad_destino">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="requiere_transporte_aereo_text"
                                                                FilterControlAltText="Filter requiere_transporte_aereo_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Transporte aéreo" SortExpression="requiere_transporte_aereo_text"
                                                                UniqueName="colm_requiere_transporte_aereo_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_vehiculo_proyecto_text"
                                                                FilterControlAltText="Filter requiere_vehiculo_proyecto_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Vehiculo del proyecto" SortExpression="requiere_vehiculo_proyecto_text"
                                                                UniqueName="colm_requiere_vehiculo_proyecto_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_transporte_fluvial_text" 
                                                                FilterControlAltText="Filter requiere_transporte_fluvial_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Transporte fluvial" SortExpression="requiere_transporte_fluvial_text"
                                                                UniqueName="colm_requiere_transporte_fluvial_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="requiere_servicio_publico_text"
                                                                FilterControlAltText="Filter requiere_servicio_publico_text column" HeaderStyle-Width="19%"
                                                                HeaderText="Servicio público" SortExpression="requiere_servicio_publico_text"
                                                                UniqueName="colm_requiere_servicio_publico_text">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Detalle del alojamiento</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <telerik:RadGrid ID="grd_alojamiento" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                    ShowFooter="false" 
                                                    ShowColumnFooters="false"
                                                    ShowGroupFooters="false"
                                                    ShowGroupPanel="false">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_viaje_hotel" AllowAutomaticUpdates="True" >
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_viaje_hotel"
                                                                FilterControlAltText="Filter id_viaje_hotel column"
                                                                SortExpression="id_viaje_hotel" UniqueName="id_viaje_hotel"
                                                                Visible="False" HeaderText="id_viaje_hotel"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_llegada"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_llegada column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha de llegada" SortExpression="fecha_llegada"
                                                                UniqueName="colm_fecha_llegada">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="fecha_salida"  DataFormatString="{0:MM/dd/yyyy}"
                                                                FilterControlAltText="Filter fecha_salida column" HeaderStyle-Width="23%"
                                                                HeaderText="Fecha de salida" SortExpression="fecha_salida"
                                                                UniqueName="colm_fecha_salida">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ciudad"
                                                                FilterControlAltText="Filter ciudad column" HeaderStyle-Width="23%"
                                                                HeaderText="Ciudad" SortExpression="ciudad"
                                                                UniqueName="colm_ciudad">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="hotel"
                                                                FilterControlAltText="Filter hotel column" HeaderStyle-Width="23%"
                                                                HeaderText="Hotel" SortExpression="hotel"
                                                                UniqueName="colm_hotel">
                                                                <HeaderStyle CssClass="wrapWord"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                            
                                        </div>
                                    </div>

                                 </div>
                           </div>
                           <div class="panel panel-default">
                               <div class="panel-heading" role="tab" id="infoLegalizacion" data-toggle="collapse" data-parent="#accordion" href="#collapseLegalizacion" aria-expanded="false" aria-controls="collapseLegalizacion">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseLegalizacion"
                                            aria-expanded="false" aria-controls="collapseLegalizacion" runat="server" id="a2">Informe del viaje
                                        </a>
                                    </h4>
                                </div>
                               <div id="collapseLegalizacion" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoLegalizacion">
                                    <div class="panel-body">
                                        
                                            <div class="form-group row">
                                                 <div class="col-sm-12 col-md-12 col-lg-12">
                                                    <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Resultados</asp:Label>
                                                    <br />
                                                    <telerik:RadTextBox ID="txt_resultados" runat="server" Rows="3" TextMode="MultiLine" Width="98%">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                ControlToValidate="txt_resultados" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                 <div class="col-sm-12 col-md-12 col-lg-12">
                                                    <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Actividades en el informe del viaje</asp:Label>
                                                    <br />
                                                    <telerik:RadTextBox ID="txt_compromisos_conclusiones" runat="server" Rows="3" TextMode="MultiLine" Width="98%">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                ControlToValidate="txt_resultados" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        <div class="form-group row">
                                                 <div class="col-sm-12 col-md-12 col-lg-12">
                                                    <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Lugares visitados, entidades y personas con las cuáles se reunió</asp:Label>
                                                    <br />
                                                    <telerik:RadTextBox ID="txt_lugares_entidades_personas" runat="server" Rows="3" TextMode="MultiLine" Width="98%">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                ControlToValidate="txt_lugares_entidades_personas" CssClass="Error" Display="Dynamic"
                                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="panel panel-default"  runat="server" id="lyHistory" visible="false">
                        
                        <div class="panel-heading" role="tab" id="infoApprobacion" data-toggle="collapse" data-parent="#accordion" href="#collapseApp" aria-expanded="false" aria-controls="collapseApp">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" href="#collapseApp"
                                    aria-expanded="false" aria-controls="collapseApp" runat="server" id="a4">Aprobación
                                </a>
                            </h4>
                        </div>
                        <div id="collapseApp" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoApprobacion">
                            <div class="panel-body">
                                <div class="form-group row">
                                <div class="col-sm-12 text-left">

                                    <%--TAble here--%>
                                    <table class="table table-responsive table-condensed box box-primary ">
                                            <tr class="box box-default ">
                                                <td  class="text-left"  colspan="2">
                                                    <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                                    <div class="box-header">
                                                        <i class="fa fa-history"></i>
                                                        <h3 class="box-title">History</h3>                                              
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
                                    </table>

                            </div>
                        </div>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="lblt_writcomments" runat="server"  CssClass="control-label text-bold"  Text="Comentarios"></asp:Label>
                                        <br />
                                        <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px"  TextMode="MultiLine" Width="100%">
                                        </telerik:RadTextBox>         
                                    </div>
                                                                                    
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-3 text-center  ">                                           
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_Approved" runat="server" Text="Aprobar"  OnClick="btn_Approved_Click"  ValidationGroup="1"  UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="72%" />
                                        <asp:Button ID="btn_Completed" runat="server" Text="Aprobar" OnClick="btn_Completed_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-info" /> 
                                    </div>
                                    <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_STandBy" runat="server" Text="Solicitar ajustes" OnClick="btn_STandBy_Click"   OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-warning"  Width="65%" />                                            
                                    </div>
                                    <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                            <asp:Button ID="btn_NotApproved" runat="server" Text="No aprobar"  OnClick="btn_NotApproved_Click"   OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn-lg btn-danger"  Width="65%" />                                                                                        
                                    </div>

<%--                                            <div class="col-sm-3 text-center  ">
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_Cancelled" runat="server" Text="Cancelar"  OnClick="btn_Cancelled_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-danger" Width="65%" />                                                  
                                    </div>        --%>                                    
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12 text-center">                                              
                                            <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                                        <br /><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                                       <div class="form-group row">
                                            <div class="box-footer">
                                                
                                                <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                                                 </telerik:RadButton>
                                                <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                    ValidationGroup="1" CausesValidation="true">
                                                </telerik:RadButton>
                                                <telerik:RadButton ID="btn_enviar_aprobacion" runat="server" Text="Guardar y enviar por aprobación" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                                                    ValidationGroup="1">
                                                </telerik:RadButton>
                                                <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                               </div>
                           </div>
                           
                       </div>
                    </div>
                </div>
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="id_tasa_ser" Value="0" />
                <asp:HiddenField runat="server" ID="valor_perdiem" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />
                
            </div>
            <div class="modal fade" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                <div class="vertical-alignment-helper">
                    <div class="modal-dialog  modal-dialog-centered">
                        <div class="modal-content modal-lg ">
                            <div class="modal-header modal-danger">
                                <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar concepto</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Delete" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
</asp:Content>