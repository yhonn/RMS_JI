<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectosCierre.aspx.vb" Inherits="ACS_SIME.frm_proyectosCierre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Proyectos</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Cierres de Proyecto</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                </telerik:RadButton>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" AllowSorting="true"
                    CellSpacing="0" GridLines="None" PageSize="15" AllowPaging="true" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ficha_proyecto" AllowAutomaticUpdates="True">

                        <Columns>
                            <telerik:GridBoundColumn DataField="id_ficha_proyecto"
                                FilterControlAltText="Filter id_ficha_proyecto column"
                                SortExpression="id_ficha_proyecto" UniqueName="id_ficha_proyecto"
                                Visible="False" DataType="System.Int32" HeaderText="id_ficha_proyecto"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" ToolTip="Imprimir detalles" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="codigo_SAPME" ItemStyle-Width="150"
                                FilterControlAltText="Filter codigo_SAPME column" HeaderText="Código Proyecto"
                                SortExpression="codigo_SAPME"
                                UniqueName="colm_codigo_SAPME">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_proyecto"
                                FilterControlAltText="Filter nombre_proyecto column"
                                HeaderText="Nombre Proyecto" SortExpression="nombre_proyecto"
                                UniqueName="colm_nombre_proyecto">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_ejecutor"
                                FilterControlAltText="Filter nombre_ejecutor column"
                                HeaderText="Nombre Ejecutor" SortExpression="nombre_ejecutor"
                                UniqueName="colm_nombre_ejecutor">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="activar">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/Circle_Red.png"
                                        ToolTip="Cerrar">
                                    </asp:HyperLink>
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
