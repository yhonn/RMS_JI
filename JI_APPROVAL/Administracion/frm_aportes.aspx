<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_aportes.aspx.vb" Inherits="RMS_APPROVAL.frm_aportes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administration</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Source of Funding</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                </telerik:RadButton>
                <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Nuevo">
                </telerik:RadButton>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    CellSpacing="0" AllowPaging="True" PageSize="15"
                    AutoGenerateColumns="False">
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_aporte" AllowAutomaticUpdates="True">
                        <Columns>

                            <telerik:GridBoundColumn DataField="id_aporte"
                                FilterControlAltText="Filter id_aporte column"
                                SortExpression="id_aporte" UniqueName="id_aporte"
                                Visible="False" DataType="System.Int32" HeaderText="id_aporte"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>

                               <telerik:GridBoundColumn DataField="id_budget"
                                FilterControlAltText="Filter id_budget column"
                                SortExpression="id_budget" UniqueName="id_budget"
                                Visible="False" DataType="System.Int32" HeaderText="id_budget"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="Filter delete column" UniqueName="colm_delete" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="col_hlk_delete" runat="server" Width="10"
                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                            OnClick="Elimina_Elemento">
                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5px" />
                                    <ItemStyle Width="10px" />
                                </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar usuario" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                             <telerik:GridBoundColumn DataField="nombre_AporteOrigen"
                                FilterControlAltText="Filter nombre_AporteOrigen column" HeaderText="Source of Funding"
                                SortExpression="nombre_AporteOrigen"
                                UniqueName="colm_nombre_AporteOrigen">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_aporte"
                                FilterControlAltText="Filter nombre_aporte column"
                                HeaderText="Funding Name" SortExpression="nombre_aporte"
                                UniqueName="colm_nombre_aporte">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="cl_nombreES"
                                FilterControlAltText="Filter cl_nombreES column"
                                HeaderText="Funding Type" SortExpression="cl_nombreES"
                                UniqueName="colm_cl_nombreES">
                            </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="bud_name"
                                FilterControlAltText="Filter bud_name column"
                                HeaderText="Budget" SortExpression="bud_name"
                                UniqueName="colm_bud_name">
                            </telerik:GridBoundColumn>

                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
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

