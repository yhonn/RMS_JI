<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_adminLanguage.aspx.vb" Inherits="RMS_APPROVAL.frm_adminLanguage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Lenguaje</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lbl_id_rol" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="X-Small" ForeColor="#C00000"
                            Visible="False"></asp:Label>
                        <asp:Label runat="server" ID="lblt_lenguaje" CssClass="text-bold">Lenguaje a Traducir</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox runat="server" ID="cmb_idioma" EmptyMessage="Seleccione un Idioma"
                            Width="300px" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadTextBox ID="txt_doc" runat="server" EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px" ValidationGroup="1">
                        </telerik:RadTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>
                    </div>
                </div>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" CellSpacing="0"
                    GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_mod" AllowAutomaticUpdates="True">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_mod"
                                FilterControlAltText="Filter id_mod column"
                                SortExpression="id_mod" UniqueName="id_mod"
                                Visible="False" DataType="System.Int32" HeaderText="id_mod"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                ConfirmDialogWidth="400px"
                                ConfirmText="Confirma que desea eliminar el registro?"
                                ConfirmTitle="Eliminar registro" ImageUrl="../Imagenes/iconos/b_drop.png"
                                UniqueName="Eliminar" FilterControlAltText="Filter Eliminar column" Visible="false">
                                <ItemStyle Width="5px" />
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="mod_name"
                                FilterControlAltText="Filter mod_name column" HeaderText="Módulo" UniqueName="colm_mod_name">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mod_desc"
                                FilterControlAltText="Filter mod_desc column" HeaderText="Descripción" UniqueName="colm_mod_desc">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="acm_acc" Visible="false"
                                FilterControlAltText="Filter acm_acc column" HeaderText="Acceso" UniqueName="colm_acm_acc">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                UniqueName="colma_traducir" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_traducir" runat="server" ImageUrl="../Imagenes/iconos/translate-icon.png" ToolTip="Traducir" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </section>
</asp:Content>

