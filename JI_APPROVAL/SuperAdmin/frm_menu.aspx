<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_menu.aspx.vb" Inherits="RMS_APPROVAL.frm_menu" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Menú</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
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
                        <telerik:RadTextBox ID="txt_doc" runat="server"
                            EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px"
                            ValidationGroup="1">
                        </telerik:RadTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Nuevo Programa" Visible="false">
                        </telerik:RadButton>
                    </div>
                </div>
                <hr />
                <asp:Label runat="server" ID="lbltotal" CssClass="info"></asp:Label>
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    CellSpacing="0" GridLines="None" PageSize="12" AllowPaging="true"
                    AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_menu" AllowAutomaticUpdates="True">
                        <Columns>

                            <telerik:GridBoundColumn DataField="id_menu"
                                FilterControlAltText="Filter id_menu column"
                                SortExpression="id_menu" UniqueName="id_menu"
                                Visible="False" DataType="System.Int32" HeaderText="id_menu"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar menu" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="nombre_item_menu" SortExpression="nombre_item_menu" AllowSorting="true"
                                FilterControlAltText="Filter nombre_item_menu column" HeaderText="Nombre Menú" UniqueName="colm_nombre_item_menu">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Nombre_padre"
                                FilterControlAltText="Filter Nombre_padre column"
                                HeaderText="Menú Superior" SortExpression="Nombre_padre"
                                UniqueName="colm_Nombre_padre">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>

