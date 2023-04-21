<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_sgtoHitos.aspx.vb" Inherits="RMS_APPROVAL.frm_sgtoHitos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Seguimiento</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Seguimiento a avance de hitos</asp:Label></h3>
            </div>
            <div class="box-body">
                 <div class="form-group row">
                    <div class="col-sm-8">
                       
                    </div>
                    <div class="col-sm-2">
                    </div>
                     <div class="col-sm-2 text-right">   
                        <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('entregables.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                    </div>
                </div>
                <div class="form-group row" style="display: none;">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_anio" CssClass="control-label text-bold">Año fiscal de la actividad</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_anio" EmptyMessage="Seleccione el año" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_TodosA" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                 <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_submecanism" CssClass="control-label text-bold">Nombre actividad o código</asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <telerik:RadTextBox ID="txt_doc" runat="server" EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px">
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>
                    </div>
                     <div class="col-sm-2">
                        
                    </div>
                </div>
                <div class="form-group row" style="display: none;">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lbl_estado_sub_producto" CssClass="control-label text-bold">Estado del hito</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_estado_producto" EmptyMessage="Seleccione el estado del producto" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_TodosP" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                <%--<div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Región</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_TodosR" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub Región</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_Todos" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>--%>
               <%-- <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadTextBox ID="txt_doc" runat="server" EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px" ValidationGroup="1">
                        </telerik:RadTextBox>
                    </div>
                </div>--%>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-2">
                        <%--<telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>--%>
                    </div>
                    <div class="col-sm-8">
                        <a href="" id="exportar" runat="server"  class="btn btn-primary btn-sm pull-right margin-r-8"><i class="fa fa-download"></i> Descargar reporte de productos</a>
                    </div>
                </div>
                <hr />
                <asp:HiddenField runat="server" ID="h_Filter" Value="" />  
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" AllowSorting="true" DataSourceID="sql_grid"
                    CellSpacing="0" GridLines="None" PageSize="15" AllowPaging="true" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ficha_proyecto" AllowAutomaticUpdates="True" DataSourceID="sql_grid">

                        <Columns>
                            <telerik:GridBoundColumn DataField="id_ficha_proyecto"
                                FilterControlAltText="Filter id_ficha_proyecto column"
                                SortExpression="id_ficha_proyecto" UniqueName="id_ficha_proyecto"
                                Visible="False" DataType="System.Int32" HeaderText="id_ficha_proyecto"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir detalles" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridTemplateColumn UniqueName="colm_productos" Visible="true">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_productos" runat="server" ImageUrl="~/Imagenes/iconos/observaciones.png"
                                        ToolTip="Comentarios">
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn DataField="codigo_RFA" ItemStyle-Width="150px"
                                FilterControlAltText="Filter codigo_RFA column" HeaderText="Código Proyecto"
                                SortExpression="codigo_RFA"
                                UniqueName="colm_codigo_SAPME">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="codigo_MONITOR"
                                FilterControlAltText="Filter codigo_MONITOR column" HeaderText="Código Monitor" UniqueName="colm_codigo_MONITOR">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_proyecto" ItemStyle-Width="400px"
                                FilterControlAltText="Filter nombre_proyecto column"
                                HeaderText="Nombre de la actividad" SortExpression="nombre_proyecto"
                                UniqueName="colm_nombre_proyecto">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_ejecutor" ItemStyle-Width="300px"
                                FilterControlAltText="Filter nombre_ejecutor column"
                                HeaderText="Nombre Ejecutor" SortExpression="nombre_ejecutor"
                                UniqueName="colm_nombre_ejecutor">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="numero_hitos" ItemStyle-Width="100px"
                                FilterControlAltText="Filter numero_hitos column"
                                HeaderText="Número de hitos" SortExpression="numero_hitos"
                                UniqueName="colm_numero_hitos">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nro_entregables" ItemStyle-Width="100px"
                                FilterControlAltText="Filter nro_entregables column"
                                HeaderText="Número de entregables" SortExpression="nro_entregables"
                                UniqueName="nro_entregables">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_Print" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="../Imagenes/iconos/Informacion1.png" ToolTip="Ver información" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="true"  ItemStyle-Width="15px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/observaciones.png"
                                        ToolTip="Activar">
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="sql_grid" runat="server"
                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                SelectCommand=""></asp:SqlDataSource>
            </div>
        </div>
    </section>
</asp:Content>
