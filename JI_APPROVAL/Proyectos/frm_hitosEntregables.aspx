<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_hitosEntregables.aspx.vb" Inherits="RMS_APPROVAL.frm_hitosEntregables" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Actividad - productos</asp:Label>
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
                                <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_nom_produto_" runat="server"  CssClass="control-label text-bold" Text="Entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                            <asp:Label ID="lbl_nom_pro" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_nro_pro_" runat="server"  CssClass="control-label text-bold" Text="Número del entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_nro_pro" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_entrega" runat="server"  CssClass="control-label text-bold" Text="Fecha esperada de entrega"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_entrega" runat="server" ></asp:Label>
                                    </div>
                                </div>
                            </div>
                            </div>
                             
                            <div class="form-group row">
                                <hr />
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_numero_entregable" CssClass="control-label text-bold">Número</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_numero_entregable" runat="server" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="txt_numero_entregable" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_descripcion_sub_producto_" CssClass="control-label text-bold">Producto</asp:Label>
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
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Unidad de medida</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadTextBox ID="txt_unidad_medida" runat="server" Width="500px"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txt_unidad_medida" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <hr />
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Cantidad</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_cantidad" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <%--<div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_porcentaje" CssClass="control-label text-bold">Porcentaje</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_porcentaje" runat="server" MinValue="0" AutoPostBack="True" MaxValue="100" OnTextChanged="txt_porcentaje_TextChanged"
                                        NumberFormat-DecimalDigits="2" Enabled="false"
                                         DisabledStyle-BackColor="LightGray" DisabledStyle-ForeColor="White">
                                        <NumberFormat ZeroPattern="n" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="txt_porcentaje" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>--%>
                            <%--<div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_valor" CssClass="control-label text-bold">Valor</asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <telerik:RadNumericTextBox ID="txt_total_aporte" runat="server" MinValue="0" AutoPostBack="True">
                                        <NumberFormat ZeroPattern="n" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txt_total_aporte" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>--%>
                            
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <%--<asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Porcentaje</asp:Label>--%>
                                </div>
                                <div class="col-sm-10">
                                    <asp:HiddenField ID="id_producto" runat="server" />
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
                                        <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDts_comentarios" DataKeyNames="id_entregable_hito, id_estado_entregable" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_entregable_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_entregable_hito column"
                                                    HeaderText="id_aporteFicha" ReadOnly="True"
                                                    SortExpression="id_entregable_hito" UniqueName="id_entregable_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="id_estado_entregable"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_estado_entregable column"
                                                    HeaderText="id_estado_entregable" ReadOnly="True"
                                                    SortExpression="id_estado_entregable" UniqueName="id_estado_entregable"
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
                                                <telerik:GridBoundColumn DataField="nro_entregable" FilterControlAltText="Filter nro_entregable column" HeaderStyle-Width="10"
                                                    HeaderText="Número" SortExpression="nro_entregable" UniqueName="nro_entregable" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion_entregable" FilterControlAltText="Filter descripcion_entregable column"
                                                    HeaderText="Descripción" SortExpression="descripcion_entregable" UniqueName="descripcion_entregable" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="unidad_medidad" FilterControlAltText="Filter unidad_medidad column"
                                                    HeaderText="Unidad de medida" SortExpression="unidad_medidad" UniqueName="unidad_medidad" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="cantidad" FilterControlAltText="Filter cantidad column"
                                                    HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad" Visible="true">
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridBoundColumn DataField="fecha_inicio" FilterControlAltText="Filter fecha_inicio column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="fecha de inicio" SortExpression="fecha_inicio" UniqueName="fecha_inicio" Visible="true">
                                                </telerik:GridBoundColumn>--%>
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
        $(document).ready(function () {
        })
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
        })
        
    </script>
</asp:Content>

