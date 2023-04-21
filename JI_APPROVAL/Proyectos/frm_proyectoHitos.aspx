<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectoHitos.aspx.vb" Inherits="RMS_APPROVAL.frm_proyectoHitos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Actividades</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Actividad - Entregables</asp:Label>
                    <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                    <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="identity" runat="server" Text="0" CssClass="deleteIdentity" data-id="" Visible="false" />
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                       
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">
                            <%--<div class="form-group row">
                                <br />
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_aporte" CssClass="control-label text-bold">Aportes</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Label runat="server" ID="lbl_monto_aportes" CssClass="control-label text-bold"></asp:Label>
                                    <asp:HiddenField runat="server" ID="monto_proyecto" />
                                </div>
                            </div>--%>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_numero_entregable" CssClass="control-label text-bold">Número</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_numero_entregable" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="txt_numero_entregable" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_descripcion_entregable_" CssClass="control-label text-bold">Entregable</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadTextBox ID="txt_descripcion_entregable" runat="server" Rows="5" TextMode="MultiLine" Width="500px"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="txt_descripcion_entregable" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_fecha" CssClass="control-label text-bold">Fecha de entrega</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadDatePicker ID="dt_fecha" runat="server">
                                        <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="dt_fecha" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Valor</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_valor_hitho" runat="server" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txt_valor_hitho" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <%--<asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Porcentaje</asp:Label>--%>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadButton ID="btn_guardarEntregable" runat="server" CssClass="btn btn-sm" Text="Agregar" ValidationGroup="1" Width="75px">
                                    </telerik:RadButton>
                                </div>
                            </div>
                            <div class="form-group row">
                                <%--<div class="col-sm-2 text-right">
                                </div>--%>
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True" DataSourceID="SqlDts_comentarios"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDts_comentarios" DataKeyNames="id_hito, id_estado_aprobacion_hito" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_hito column"
                                                    HeaderText="id_hito" ReadOnly="True"
                                                    SortExpression="id_hito" UniqueName="id_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="id_estado_aprobacion_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_estado_aprobacion_hito column"
                                                    HeaderText="id_estado_aprobacion_hito" ReadOnly="True"
                                                    SortExpression="id_estado_aprobacion_hito" UniqueName="id_estado_aprobacion_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar">
                                                    <HeaderStyle Width="10" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                            OnClick="EliminarAporte_Click">
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_editar">
                                                    <HeaderStyle Width="10" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_editar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                                            OnClick="Editar_Click">
                                                            <asp:Image ID="img_editar" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="nro_hito" FilterControlAltText="Filter nro_producto column"
                                                    HeaderText="Número" SortExpression="nro_hito" UniqueName="nro_hito" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion_hito" FilterControlAltText="Filter descripcion_hito column"
                                                    HeaderText="Nombre" SortExpression="descripcion_hito" UniqueName="descripcion_hito" Visible="true">
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridBoundColumn DataField="fecha_inicio" FilterControlAltText="Filter fecha_inicio column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="fecha de inicio" SortExpression="fecha_inicio" UniqueName="fecha_inicio" Visible="true">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridBoundColumn DataField="fecha_esperada_entrega" FilterControlAltText="Filter fecha_esperada_entrega column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="fecha esperada de entrega" SortExpression="fecha_esperada_entrega" UniqueName="fecha_esperada_entrega" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor" FilterControlAltText="Filter valor column" DataFormatString="{0:C}"
                                                    HeaderText="Valor" SortExpression="valor" UniqueName="valor" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="dias_restantes" FilterControlAltText="Filter dias_restantes column"
                                                    HeaderText="Dias faltantes" SortExpression="dias_restantes" UniqueName="dias_restantes" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="estado" FilterControlAltText="Filter estado column"
                                                    HeaderText="Estado" SortExpression="estado" UniqueName="estado" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="numero_entregables" FilterControlAltText="Filter numero_entregables column"
                                                    HeaderText="Número de productos" SortExpression="numero_entregables" UniqueName="numero_entregables" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="alerta" FilterControlAltText="Filter alerta column"
                                                    HeaderText="Alerta" SortExpression="alerta" UniqueName="alerta" Visible="true">
                                                </telerik:GridBoundColumn>  
                                                <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="true"  ItemStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/additem.png"
                                                            ToolTip="Agregar producto">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
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
                            <%--<div class="form-group row">
                                <div class="col-sm-12 text-right">
                                    <h4>
                                        <asp:Label runat="server" ID="lblt_total_aporte" CssClass="text-bold">Total Aporte:</asp:Label>
                                        <asp:Label runat="server" ID="lbl_total"></asp:Label>
                                    </h4>
                                    <h4>
                                        <asp:Label runat="server" ID="lblt_total_aporteUSD" CssClass="text-bold">Total Aporte USD:</asp:Label>
                                        <asp:Label runat="server" ID="lbl_totalUSD"></asp:Label>
                                    </h4>
                                </div>
                            </div>--%>
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
                <div class="modal fade bs-example-modal-sm" id="modalTasaCambio" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                    <h4 class="modal-title" runat="server" id="H1">Alerta</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblt_msn_tasa_cambio" runat="server" Text="Debe ingresar la tasa de cambio correspondiente al periodo" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_registrar_tc" CssClass="btn btn-sm btn-primary btn-ok" Text="Registrar tasa de cambio" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script>
        function FuncModatTrim() {
            $('#modalTasaCambio').modal('show');
        }
        window.onClientFileUploaded = function (radAsyncUpload, args) {

        }
        //$(document).ready(function () {
        //    loadScript()
        //})
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_endRequest(function (s, e) {
        //    loadScript()
        //})
       
        
    </script>
</asp:Content>

