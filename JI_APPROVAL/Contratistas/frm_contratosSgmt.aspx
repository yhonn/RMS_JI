<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratosSgmt.aspx.vb" Inherits="RMS_APPROVAL.frm_contratosSgmt" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Contratistas</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Seguimiento de entregables</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                       <div class ="row">
                           <asp:HiddenField runat="server" ID="idContrato" Value="0" />
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_contrato" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Contratista"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_contratista" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Fecha de inicio"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_inicio" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_fin" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_valor_contrato" runat="server"  CssClass="control-label text-bold" Text="Valor del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_valor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="Supervisor"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_supervisor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold" Text="Objeto del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_objeto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <hr />
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Entregables</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        
                        <div class="panel-body">
                            <div class="form-group row">
                                <%--<div class="col-sm-2 text-right">
                                </div>--%>
                                <div class="col-sm-12">
                                     <telerik:RadGrid ID="grd_entregables" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_contrato_entregable" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_contrato_entregable"
                                                    FilterControlAltText="Filter id_contrato_entregable column"
                                                    SortExpression="id_contrato_entregable" UniqueName="id_contrato_entregable"
                                                    Visible="False" HeaderText="id_contrato_entregable"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>

                                                 <telerik:GridBoundColumn DataField="id_estado_aprobacion_entregable"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_estado_aprobacion_entregable column"
                                                    HeaderText="id_estado_aprobacion_entregable" ReadOnly="True"
                                                    SortExpression="id_estado_aprobacion_entregable" UniqueName="id_estado_aprobacion_entregable"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="numero_entregable"
                                                    FilterControlAltText="Filter numero_entregable column" HeaderStyle-Width="8%"
                                                    HeaderText="Nro" SortExpression="numero_entregable"
                                                    UniqueName="colm_cantidad">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="nombre_entregable"
                                                    FilterControlAltText="Filter nombre_entregable column" HeaderStyle-Width="15%"
                                                    HeaderText="Entregable" SortExpression="nombre_entregable"
                                                    UniqueName="colm_descripcion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                
                                                <telerik:GridBoundColumn DataField="fecha_esperada_entrega"  DataFormatString="{0:MM/dd/yyyy}"
                                                    FilterControlAltText="Filter fecha_esperada_entrega column" HeaderStyle-Width="17%"
                                                    HeaderText="Fecha esperada de entrega" SortExpression="fecha_esperada_entrega"
                                                    UniqueName="colm_fecha_esperada_entrega">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="dias_restantes"
                                                    FilterControlAltText="Filter dias_restantes column" HeaderStyle-Width="10%"
                                                    HeaderText="Dias faltantes" SortExpression="dias_restantes"
                                                    UniqueName="colm_dias_restantes">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="estado"
                                                    FilterControlAltText="Filter estado column" HeaderStyle-Width="8%"
                                                    HeaderText="Estado" SortExpression="estado"
                                                    UniqueName="colm_estado">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="productos"
                                                    FilterControlAltText="Filter productos column" HeaderStyle-Width="47%"
                                                    HeaderText="Productos" SortExpression="productos"
                                                    UniqueName="colm_productos">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor_entregable" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                    FilterControlAltText="Filter valor_entregable column" HeaderStyle-Width="19%"
                                                    HeaderText="Valor total" SortExpression="valor_entregable"
                                                    UniqueName="colm_valor">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="alerta"
                                                    FilterControlAltText="Filter alerta column" HeaderStyle-Width="5%"
                                                    HeaderText="Alerta" SortExpression="alerta"
                                                    UniqueName="alerta">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="url" FilterControlAltText="Filter url column"
                                                    HeaderText="Soporte" SortExpression="url" UniqueName="colm_sub_producto" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="col_hlk_view" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_view" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                            ToolTip="Soporte" Target="_new">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>  
                                                 <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="false" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/additem.png"
                                                            ToolTip="Entregables">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn  UniqueName="colm_detalle" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="col_hlk_detalle" runat="server" ImageUrl="../Imagenes/iconos/printer_off.png" ToolTip="Ver detalle" Target="_blank" />
                                                    </ItemTemplate>
                                                   <HeaderStyle Width="25px" />
                                                   <ItemStyle Width="25px" />                                           
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <div class="alert-sm bg-blue col-sm-7" runat="server" id="div_mensaje" visible="false">
                            <asp:Label runat="server" ID="lblt_Error" CssClass="text-center text-bold">The value can´t be greater than the funding assign for the project</asp:Label>
                        </div>
                    </div>
                </div>
                <!-- /.box-footer -->
                <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar Registro</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    
</asp:Content>

