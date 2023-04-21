<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="frm_regiones.aspx.vb" Inherits="RMS_APPROVAL.frm_regiones" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Regiones</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" AutoPostBack="true" Enabled="false"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadTextBox ID="txt_doc" runat="server"
                            EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px"
                            ValidationGroup="1">
                            <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                        </telerik:RadTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Nuevo region">
                        </telerik:RadButton>
                    </div>
                </div>
                <hr />
                <asp:Label runat="server" ID="lbltotal" CssClass="info"></asp:Label>
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    CellSpacing="0" DataSourceID="SqlDataSource2" AllowSorting="true"
                    GridLines="None" AllowPaging="True" PageSize="15"
                    AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_region" DataSourceID="SqlDataSource2"
                        AllowAutomaticUpdates="True" AllowMultiColumnSorting="true" OverrideDataSourceControlSorting="true">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_region"
                                FilterControlAltText="Filter id_region column"
                                SortExpression="id_region" UniqueName="id_region"
                                Visible="False" DataType="System.Int32" HeaderText="id_region"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Eliminar" Visible="false">
                                <HeaderStyle Width="10" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                        OnClick="Eliminar_Click">
                                        <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar regiones" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="nombre_region"
                                FilterControlAltText="Filter nombre_region column"
                                HeaderText="región" SortExpression="nombre_region"
                                UniqueName="colm_nombre_region">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                UniqueName="visible" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_SubRegiones" runat="server" ImageUrl="../Imagenes/iconos/additem.png" ToolTip="Subregiones" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    SelectCommand="SELECT * FROM [t_regiones]"></asp:SqlDataSource>

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
