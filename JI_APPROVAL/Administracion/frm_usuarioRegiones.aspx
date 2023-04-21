<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_usuarioRegiones.aspx.vb" Inherits="RMS_APPROVAL.frm_usuarioRegiones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <uc:Confirm runat="server" ID="MsgGuardar" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Sub Regiones Usuario</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                        <asp:Label ID="lbl_id_usuario" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="X-Small" ForeColor="#C00000"
                            Visible="False"></asp:Label>
                        <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" AutoPostBack="true" Enabled="false"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Región</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox runat="server" ID="cmb_id_subregion" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un subregión"
                            DataTextField="nombre_subregion" DataValueField="id_subregion" ValidationGroup="1" Width="300px">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT * FROM t_subregiones WHERE id_subregion NOT IN (SELECT id_subregion FROM t_usuario_subregion WHERE id_usuario = @id_usuario) and id_region = @id_region">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="id_usuario" QueryStringField="Id" />
                                <asp:ControlParameter Name="id_region" ControlID="cmb_region" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Text="Asignar sub región" ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                            ControlToValidate="cmb_id_subregion" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <hr />

                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15"
                    AutoGenerateColumns="False">
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                        <WebServiceSettings>
                            <ODataSettings InitialContainerName=""></ODataSettings>
                        </WebServiceSettings>
                    </HeaderContextMenu>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_subregion" DataSourceID="SqlDataSource2"
                        AllowAutomaticUpdates="True">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_usuario_subregion"
                                FilterControlAltText="Filter id_usuario_subregion column"
                                SortExpression="id_usuario_subregion" UniqueName="id_usuario_subregion"
                                Visible="False" DataType="System.Int32" HeaderText="id_usuario_subregion"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Eliminar" Visible="false">
                                <HeaderStyle Width="10px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10px"
                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                        OnClick="Eliminar_Click">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="nombre_subregion"
                                FilterControlAltText="Filter nombre_subregion column" HeaderText="Sub Región" UniqueName="nombre_subregion">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    SelectCommand="SELECT * FROM [vw_t_usuarios_subregiones] WHERE id_usuario = @id_usuario">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="id_usuario" QueryStringField="Id" />
                    </SelectParameters>
                </asp:SqlDataSource>
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

