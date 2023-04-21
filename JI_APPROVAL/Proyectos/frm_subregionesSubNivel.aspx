<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_subregionesSubNivel.aspx.vb" Inherits="RMS_APPROVAL.frm_subregionesSubNivel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Sub Nivel - Nuevo</asp:Label>
                    <asp:Label runat="server" ID="lbl_subtitulo_aux"></asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lbl_id_subregion" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="X-Small" ForeColor="#C00000"
                            Visible="False"></asp:Label>
                        <asp:Label ID="lblt_programa" runat="server" CssClass="control-label text-bold" Text="Programa"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_programa" runat="server" EmptyMessage="Seleccione un Programa"
                            Width="300px" AutoPostBack="true">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                            ControlToValidate="cmb_programa" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblt_subnivel" runat="server" CssClass="control-label text-bold" Text="Sub Nivel"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox runat="server" ID="cmb_departamento" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione"
                            DataTextField="ctrl_name" DataValueField="id_control" ValidationGroup="1" Width="300px"
                            Filter="Contains"
                            CausesValidation="false"
                            AllowCustomText="true"
                            HighlightTemplatedItems="true"
                            EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand=""></asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                            ControlToValidate="cmb_departamento" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Agregar SubNivel" ValidationGroup="1" Enabled="false">
                        </telerik:RadButton>
                    </div>
                </div>
                <hr />
                <telerik:RadGrid ID="grd_catalogos" runat="server" CellSpacing="0" DataSourceID="SqlDataSource2" GridLines="None">
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource2">
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter edit column" UniqueName="Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="editar" runat="server"
                                        ImageUrl="~/Imagenes/iconos/b_edit.png" ToolTip="Editar" />
                                </ItemTemplate>
                                <HeaderStyle Width="5px" />
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Eliminar" Visible="false">
                                <HeaderStyle Width="10" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                        OnClick="Eliminar_Click">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32"
                                FilterControlAltText="Filter ID column" HeaderText="ID" ReadOnly="True"
                                SortExpression="ID" UniqueName="ID" Visible="False">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre" FilterControlAltText="Filter nombre_actividad_clave column"
                                HeaderText="Descripción" SortExpression="nombre" UniqueName="colm_nombre_actividad_clave">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"></asp:SqlDataSource>
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
