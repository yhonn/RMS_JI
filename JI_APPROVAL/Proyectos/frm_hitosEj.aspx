<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_hitosEj.aspx.vb" Inherits="RMS_APPROVAL.frm_hitosEj" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Subactividades</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Subactividad - Hitos</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                       <div class ="row">
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_acti" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Nombre de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_actividad" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Fecha de inicio actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_inicio" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_fin" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_ejecutor" runat="server"  CssClass="control-label text-bold" Text="Nombre ejecutor"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_ejecutor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <hr />
                       <%-- <div class="form-group row">
                            <div class="col-sm-2 text-right">
                                <asp:Label runat="server" ID="lbl_estado" CssClass="control-label text-bold">Estado del hito</asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <telerik:RadComboBox ID="cmb_estado_producto" EmptyMessage="Seleccione el estado del producto" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                <asp:CheckBox ID="chk_TodosP" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                            </div>
                        </div>
                        <hr />--%>
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Hitos</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        
                        <div class="panel-body">
                            <div class="form-group row">
                                <%--<div class="col-sm-2 text-right">
                                </div>--%>
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True" DataSourceID="SqlDts_comentarios"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_hito" DataSourceID="SqlDts_comentarios" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_hito column"
                                                    HeaderText="id_hito" ReadOnly="True"
                                                    SortExpression="id_hito" UniqueName="id_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridTemplateColumn UniqueName="colm_productos" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_productos" runat="server" ImageUrl="~/Imagenes/iconos/observaciones.png"
                                                            ToolTip="Comentarios">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>--%>
                                                 
                                                <telerik:GridBoundColumn DataField="nro_hito" FilterControlAltText="Filter nro_hito column"
                                                    HeaderText="Número" SortExpression="nro_hito" UniqueName="nro_hito" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion_hito" FilterControlAltText="Filter descripcion_hito column"
                                                    HeaderText="Hito" SortExpression="descripcion_hito" UniqueName="descripcion_hito" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor" FilterControlAltText="Filter valor column" DataFormatString="{0:C}"
                                                    HeaderText="Valor" SortExpression="valor" UniqueName="valor" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="estado" FilterControlAltText="Filter estado column"
                                                    HeaderText="Estado" SortExpression="estado" UniqueName="estado" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="fecha_esperada_entrega" FilterControlAltText="Filter fecha_esperada_entrega column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="Fecha esperada de entrega" SortExpression="fecha_esperada_entrega" UniqueName="fecha_esperada_entrega" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="fecha_entrega" FilterControlAltText="Filter fecha column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="Fecha de entrega" SortExpression="fecha_entrega" UniqueName="fecha_entrega" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="fecha_aprobacion" FilterControlAltText="Filter fecha column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="Fecha de aprobación" SortExpression="fecha_aprobacion" UniqueName="fecha_aprobacion" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="dias_restantes" FilterControlAltText="Filter dias_restantes column"
                                                    HeaderText="Dias faltantes" SortExpression="dias_restantes" UniqueName="dias_restantes" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="numero_entregables" FilterControlAltText="Filter numero_entregables column"
                                                    HeaderText="Total de entregables" SortExpression="numero_entregables" UniqueName="numero_entregables" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="alerta" FilterControlAltText="Filter alerta column"
                                                    HeaderText="Alerta" SortExpression="alerta" UniqueName="alerta" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="true"  ItemStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/observaciones.png"
                                                            ToolTip="Entregables">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="false">
                                                    <HeaderStyle Width="10" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Descargar PDF"
                                                            OnClick="generarPdf">
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/adjunto.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
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
                                    <asp:SqlDataSource ID="SqlDts_comentarios" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                            SelectCommand="">
                                        </asp:SqlDataSource>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="2">
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

