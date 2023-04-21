<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_usuarioProyectos.aspx.vb" Inherits="RMS_APPROVAL.frm_usuarioProyectos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Convenios Usuario</asp:Label></h3>
            </div>
            <div class="box-body">
                <asp:HiddenField ID="id_usuario" runat="server" />
               <div class="panel-body div-bordered">
                            <div class="form-group row" style="padding:0 15px;">
                                <asp:Label runat="server" ID="lblt_upm" CssClass="control-label text-bold">Convenios asignados</asp:Label>
                                <hr />
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_pro_rel" runat="server" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                        <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_usuario_ficha_proyecto">
                                            <Columns>
                                                
                                                <telerik:GridBoundColumn DataField="id_usuario_ficha_proyecto" DataType="System.Int32"
                                                    FilterControlAltText="Filter id_usuario_ficha_proyecto column" HeaderText="id_usuario_ficha_proyecto"
                                                    ReadOnly="True" SortExpression="id_usuario_ficha_proyecto" UniqueName="id_usuario_ficha_proyecto"
                                                    Display="False">
                                                    <HeaderStyle Width="500" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="codigo_RFA"
                                                    FilterControlAltText="Filter codigo_RFA column"
                                                    HeaderText="Código" SortExpression="codigo_RFA"
                                                    UniqueName="colm_codigo_SAPME">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="nombre_proyecto"
                                                    FilterControlAltText="Filter nombre_proyecto column"
                                                    HeaderText="Nombrel del convenio" SortExpression="nombre_proyecto"
                                                    UniqueName="colm_nombre_proyecto">
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridBoundColumn DataField="nombre_ejecutor"
                                                    FilterControlAltText="Filter nombre_ejecutor column"
                                                    HeaderText="Ejecutor" SortExpression="nombre_ejecutor"
                                                    UniqueName="nombre_ejecutor">
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn FilterControlAltText="Filter estadoE column"
                                                    UniqueName="colm_estadoE" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True"
                                                            OnCheckedChanged="chkVisible_CheckedChanged" />
                                                        <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                                            CheckedImageUrl="../Imagenes/iconos/Stock_IndexUp.png" ImageHeight="16" ImageWidth="16"
                                                            TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/Stock_IndexDown.png">
                                                        </cc1:ToggleButtonExtender>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10px" />
                                                    <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>

                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <div class="form-group row" style="padding:0 15px;">
                                <hr />
                                <asp:Label runat="server" ID="lblt_relacionar_upm" CssClass="control-label text-bold">Asignar convenios</asp:Label>
                                <hr />
                                <div class="col-sm-12" style="margin-bottom:20px;">
                                    <telerik:RadTextBox ID="txt_doc" runat="server"
                                        EmptyMessage="Buscar Proyecto..." LabelWidth="" Width="395px"
                                        ValidationGroup="1">
                                    </telerik:RadTextBox>
                                    <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                                    </telerik:RadButton>
                                    <br />
                                </div>
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_upm_add" runat="server" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                        <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_ficha_proyecto">
                                            <Columns>
                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnOrg column"
                                                    HeaderText="" UniqueName="TemplateColumnUpm">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="selectedID" runat="server" AutoPostBack="true" OnCheckedChanged="upm_check" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="id_ficha_proyecto" DataType="System.Int32"
                                                    FilterControlAltText="Filter id_ficha_proyecto column" HeaderText="id_ficha_proyecto"
                                                    ReadOnly="True" SortExpression="id_ficha_proyecto" UniqueName="id_ficha_proyecto"
                                                    Display="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="codigo_RFA"
                                                    FilterControlAltText="Filter codigo_RFA column"
                                                    HeaderText="Código" SortExpression="codigo_RFA"
                                                    UniqueName="colm_codigo_SAPME">
                                                    <HeaderStyle Width="180px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="nombre_proyecto"
                                                    FilterControlAltText="Filter nombre_proyecto column"
                                                    HeaderText="Nombre del convenio" SortExpression="nombre_proyecto"
                                                    UniqueName="colm_nombre_proyecto">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="nombre_ejecutor"
                                                    FilterControlAltText="Filter nombre_ejecutor column"
                                                    HeaderText="Ejecutor" SortExpression="nombre_ejecutor"
                                                    UniqueName="nombre_ejecutor">
                                                </telerik:GridBoundColumn>
                                                
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                        </div>
                    </div>

            </div>
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
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>

