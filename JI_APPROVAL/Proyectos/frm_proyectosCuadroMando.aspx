<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectosCuadroMando.aspx.vb" Inherits="ACS_SIME.frm_proyectosCuadroMando" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Proyectos</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
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
                <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px"></asp:Label>
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ficha_proyecto"
                        AllowAutomaticUpdates="True">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_ficha_proyecto"
                                FilterControlAltText="Filter id_ficha_proyecto column"
                                SortExpression="id_ficha_proyecto" UniqueName="id_ficha_proyecto"
                                Visible="False" DataType="System.Int32" HeaderText="id_ficha_proyecto"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_print" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" ToolTip="Imprimir ficha" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridBoundColumn DataField="codigo_SAPME"
                                FilterControlAltText="Filter codigo_SAPME column" HeaderText="Código"
                                SortExpression="codigo_SAPME"
                                UniqueName="colm_codigo_SAPME">
                                <HeaderStyle Width="160px" />
                                <ItemStyle Width="160px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="nombre_proyecto"
                                FilterControlAltText="Filter nombre_proyecto column"
                                HeaderText="Proyecto" SortExpression="nombre_proyecto"
                                UniqueName="colm_nombre_proyecto">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_estado_ficha"
                                FilterControlAltText="Filter nombre_estado_ficha column" HeaderText="Estado"
                                SortExpression="nombre_estado_ficha"
                                UniqueName="colm_nombre_estado_ficha">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="ImgCMP" HeaderText="IMG" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_imagenes" runat="server" ImageUrl="~/Imagenes/iconos/photo16x16.png" ToolTip="Adjuntar imágenes" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="GeoreferenciaCMP" HeaderText="GIS" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_GeoreferenciaCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Georeferencia completa" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="RecursoCMP" HeaderText="($$)" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_RecursosCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Recursos obligados completos" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="OrdenInicioCMP" HeaderText="OI" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_OrdenInicioCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Ejecución" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="EstadoCMP" HeaderText="">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_EstadoCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Estado" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="georeferencia_completa"
                                FilterControlAltText="Filter georeferencia_completa column" HeaderText=""
                                SortExpression="georeferencia_completa"
                                UniqueName="georeferencia_completa" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="aportes_actualizados"
                                FilterControlAltText="Filter aportes_actualizados column" HeaderText="aportes_actualizados"
                                SortExpression="aportes_actualizados"
                                UniqueName="aportes_actualizados" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="id_ficha_estado"
                                FilterControlAltText="Filter id_ficha_estado column" HeaderText="id_ficha_estado"
                                SortExpression="id_ficha_estado"
                                UniqueName="id_ficha_estado" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="bandera_estado"
                                FilterControlAltText="Filter bandera_estado column" HeaderText="bandera_estado"
                                SortExpression="bandera_estado"
                                UniqueName="bandera_estado" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" Visible="false"
                                UniqueName="printMGR" ConvertEmptyStringToNull="False">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_PrintMGR" runat="server" ImageUrl="~/Imagenes/iconos/grafica1.jpg" ToolTip="Imprimir MGR" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="MenuCMP" HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_MenuCMP" runat="server" ImageUrl="~/Imagenes/iconos/icon_clockwise.png" ToolTip="Editar lista de instrumentos" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
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


