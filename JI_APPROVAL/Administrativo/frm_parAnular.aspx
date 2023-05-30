<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_parAnular.aspx.vb" Inherits="RMS_APPROVAL.frm_parAnular" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Seguimiento PAR</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-2 text-right">   
                                <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('aprobacion_par_vf.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                            </div>
                        </div>
                         <hr />
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_itinerario" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        
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
                                        <div class="col-sm-12 text-right">
                                            <div class="form-group row">
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
                                                    <hr />
                                                </div>    
                                            </div>
                                            <div class ="form-group row">
                                                <div class="col-sm-12 text-left ">
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label5" runat="server"  CssClass="control-label text-bold" Text="1 Date of request"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                               <asp:Label runat="server" ID="lbl_fecha_solicitud"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label9" runat="server"  CssClass="control-label text-bold" Text="5 Date Items/Services Needed"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_fecha_entrega"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label13" runat="server"  CssClass="control-label text-bold" Text="2 Requested by"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_solicitado"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label15" runat="server"  CssClass="control-label text-bold" Text="6 Requested For/Deliver To"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_aprobado"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label17" runat="server"  CssClass="control-label text-bold" Text="3 Title of Requestor"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_cargo"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label20" runat="server"  CssClass="control-label text-bold" Text="7 Reference Info, if Applicable"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_codigo_rfa"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label22" runat="server"  CssClass="control-label text-bold" Text="4 Office"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_departamento"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label24" runat="server"  CssClass="control-label text-bold" Text="Tipo de PAR"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_tipo_par"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="row">
                                                         <hr />
                                                     </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="lll" runat="server"  CssClass="control-label text-bold" Text="8 Purpose of PAR"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_proposito_par"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-6">
                                                                <asp:Label ID="Label3" runat="server"  CssClass="control-label text-bold" Text="9 Charge To (check one and enter billing code, if applicable)"></asp:Label>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Label runat="server" ID="lbl_asociado_a_par"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="row">
                                                         <hr />
                                                     </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <div class="col-sm-12">
                                                                <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="10 Items/Services Requered"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="col-sm-12">
                                                                <telerik:RadGrid ID="grd_servicios_requeridos" runat="server" CellSpacing="0"
                                                                    Culture="Spanish (Spain)" GridLines="None"
                                                                    Skin="Simple" AllowSorting="True">
                                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" ShowFooter="true">
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Item #"  HeaderStyle-Font-Bold="true" ItemStyle-Width="5px">
                                                                                <ItemStyle Width="5px"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="numberLabel" runat="server" 

                                                                                Text='<%#Container.ItemIndex+1%>' Width="10px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="50px" />
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridBoundColumn DataField="descripcion" HeaderStyle-Font-Bold="true"
                                                                                FilterControlAltText="Filter descripcion column" HeaderText="Description/Specifications of Items or Service"
                                                                                SortExpression="descripcion" UniqueName="descripcion">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="cantidad" HeaderStyle-Font-Bold="true"
                                                                                FilterControlAltText="Filter cantidad column" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                                HeaderText="Quantity" SortExpression="cantidad" 
                                                                                UniqueName="cantidad" DataFormatString="{0:n0}">
                                                                                <ItemStyle CssClass="textrightalign" />
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="unidad_medida" HeaderStyle-Font-Bold="true"
                                                                                FilterControlAltText="Filter unidad_medida column"
                                                                                HeaderText="Units" SortExpression="unidad_medida"
                                                                                UniqueName="unidad_medida" DataFormatString="{0:n0}">
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="precio_unitario" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                                                                                HeaderStyle-HorizontalAlign="Right"
                                                                                FilterControlAltText="Filter precio_unitario column" FooterText="TOTAL ESTIMATED COP: "
                                                                                HeaderText="Est. Unit Price COP" SortExpression="precio_unitario" 
                                                                                UniqueName="precio_unitario" DataFormatString="{0:n0}">
                                                                                <ItemStyle CssClass="textrightalign" />
                                                                            </telerik:GridBoundColumn>
                                                                            <telerik:GridBoundColumn DataField="valor_total" FooterStyle-Font-Bold="true" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                                FilterControlAltText="Filter valor_total column" Aggregate="Sum" FooterAggregateFormatString="{0:n0}"
                                                                                HeaderText="Est. Total Price COP" SortExpression="valor_total" 
                                                                                UniqueName="valor_total" DataFormatString="{0:n0}">
                                                                                <ItemStyle CssClass="textrightalign" />
                                                                            </telerik:GridBoundColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                            </div>
                                                        </div>
                                                    </div>

<%--

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
                                                     <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label34" runat="server"  CssClass="control-label text-bold" Text="Código PT"></asp:Label>                                
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_codigo_pt" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label2" runat="server" CssClass="control-label text-bold" Text="Usuario solicita"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_usuario_solicitud" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label3" runat="server" CssClass="control-label text-bold" Text="Tasa SER"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_tasa_ser" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label4" runat="server" CssClass="control-label text-bold" Text="Regional"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_regional" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label6" runat="server" CssClass="control-label text-bold" Text="Sub región"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_sub_region" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label8" runat="server" CssClass="control-label text-bold" Text="Fecha en la que se requieren los servicios"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_requiere_servicios" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label10" runat="server" CssClass="control-label text-bold" Text="Proposito del PAR"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_proposito_par" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="Label12" runat="server" CssClass="control-label text-bold" Text="Municipio de entrega"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_municipio_entrega" runat="server" ></asp:Label>
                                                        </div>
                                                    </div>--%>
                                                    <div class=" row">
                                                   

                                                    <%--<div class="row">
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
                                                    </div>                --%>                                              
                                                    
                                                </div> 
                                            </div> 
                                        </div> <!--div 0 lg-12-->
                                    </div>
                                         <div class="form-group row">
                                            <div class="col-sm-12">
                                                <hr />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                            
                                            <div class="col-sm-12">
                                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Motivo anula</asp:Label>
                                                <br />
                                                <telerik:RadTextBox ID="txt_motivo_anular" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                                </telerik:RadTextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                            ControlToValidate="txt_motivo_anular" CssClass="Error" Display="Dynamic"
                                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                </div>
                                <asp:HiddenField runat="server" ID="idPar" Value="0" />
                                <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />

                                    
                            </div>  
                                
                                <%--First panel--%> 
                          <%-- <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="infoViaje" data-toggle="collapse" data-parent="#accordion" href="#collapseViaje" aria-expanded="false" aria-controls="collapseViaje">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseViaje"
                                            aria-expanded="false" aria-controls="collapseViaje" runat="server" id="a1">Servicios requeridos
                                        </a>
                                    </h4>
                                </div>
                                 <div id="collapseViaje" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoViaje">
                                    <div class="panel-body">
                                        
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Items / Servicios requeridos</asp:Label></h4>
                                                <hr class="box box-primary" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_par_detalle"
                                                    FilterControlAltText="Filter id_par_detalle column"
                                                    SortExpression="id_par_detalle" UniqueName="id_par_detalle"
                                                    Visible="False" HeaderText="id_par_detalle"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="cantidad"
                                                    FilterControlAltText="Filter cantidad column" HeaderStyle-Width="19%"
                                                    HeaderText="Cantidad" SortExpression="cantidad"
                                                    UniqueName="colm_cantidad">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion"
                                                    FilterControlAltText="Filter descripcion column" HeaderStyle-Width="47%"
                                                    HeaderText="Descripción / Servicio requerido" SortExpression="descripcion"
                                                    UniqueName="colm_descripcion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="unidad_medida"
                                                    FilterControlAltText="Filter unidad_medida column" HeaderStyle-Width="47%"
                                                    HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                                    UniqueName="colm_unidad_medida">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor_unitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    DataFormatString="{0:n}" 
                                                    FilterControlAltText="Filter valor_unitario column" HeaderStyle-Width="13%"
                                                    HeaderText="Valor unitario" SortExpression="valor_unitario"
                                                    UniqueName="colm_valor_unitario">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                    FilterControlAltText="Filter valor column" HeaderStyle-Width="19%"
                                                    HeaderText="Valor total" SortExpression="valor"
                                                    UniqueName="colm_valor">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                                            
                                        </div>
                                    </div>

                                 </div>
                           </div>--%>
                            

                       </div>
                    </div>
                    
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Anular PAR" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
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
        </div>
    </section>
</asp:Content>
