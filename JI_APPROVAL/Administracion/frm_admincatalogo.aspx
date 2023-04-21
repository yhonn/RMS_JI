<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_admincatalogo.aspx.vb" Inherits="RMS_APPROVAL.frm_admincatalogo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Catálogos</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px"></telerik:RadButton>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <br />
                        <asp:Label ID="lblt_listado_catalogo" runat="server" Style="font-weight: bold" Text="Lista de Catalogos"></asp:Label>
                        <telerik:RadComboBox ID="cmb_catalogo" runat="server" Width="280px" AutoPostBack="True" CausesValidation="False"></telerik:RadComboBox>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Text="Nuevo Elemento" Enabled="false"></telerik:RadButton>
                        <telerik:RadButton ID="btn_asoc" runat="server" AutoPostBack="true" Text="Asociar a instrumentos" Enabled="false"></telerik:RadButton>
                        <asp:HyperLink ID="hlk_export" runat="server" ForeColor="Blue" Visible="false">Exportar listado a excel</asp:HyperLink>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <br />
                        <asp:Label ID="lblt_instrumentos" runat="server" Style="font-weight: 700" Text="Instrumentos asociados"></asp:Label>
                        <asp:BulletedList ID="BulletedList1" runat="server"
                            DataSourceID="SqlDataSource4" DataTextField="nombre_instrumento"
                            DataValueField="id_instrumento">
                        </asp:BulletedList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT tme_Instrumentos.id_instrumento, tme_Instrumentos.codigo_instrumento, tme_Instrumentos.nombre_instrumento, t_CatalogoInstrumento.id_CatalogoMaster FROM t_CatalogoInstrumento INNER JOIN tme_Instrumentos ON t_CatalogoInstrumento.id_instrumento = tme_Instrumentos.id_instrumento WHERE (t_CatalogoInstrumento.id_CatalogoMaster = @id_catalogo)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="cmb_catalogo" DefaultValue="-1"
                                    Name="id_catalogo" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                <hr />
                <div class="col-lg-12">
                    <telerik:RadGrid ID="grd_catalogos" runat="server" CellSpacing="0" DataSourceID="SqlDataSource3" GridLines="None">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="True"></Selecting>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id"
                            DataSourceID="SqlDataSource3">
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter edit column" UniqueName="Edit" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="col_hlk_edit" runat="server"
                                            ImageUrl="~/Imagenes/iconos/b_edit.png" ToolTip="Editar" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5px" />
                                    <ItemStyle Width="10px" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="id" DataType="System.Int32"
                                    FilterControlAltText="Filter ID column"
                                    HeaderText="ID" ReadOnly="True"
                                    SortExpression="ID" UniqueName="ID"
                                    Visible="False">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="tipo"
                                    DataType="System.Int32"
                                    FilterControlAltText="Filter tipo column"
                                    HeaderText="tipo" SortExpression="tipo"
                                    UniqueName="tipo" Visible="False">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CampoPadre"
                                    FilterControlAltText="Filter CampoPadre column"
                                    HeaderText="Descripción Padre" SortExpression="nombre"
                                    UniqueName="colm_CampoPadre">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre"
                                    FilterControlAltText="Filter nombre_actividad_clave column"
                                    HeaderText="Descripción" SortExpression="nombre"
                                    UniqueName="colm_nombre_actividad_clave">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                        ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                        SelectCommand="SELECT id_tipo_indicador AS id, -1 AS tipo, nombre_tipo_indicador AS nombre FROM tme_tipo_indicador"></asp:SqlDataSource>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>


