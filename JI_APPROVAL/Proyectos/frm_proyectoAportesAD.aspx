<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectoAportesAD.aspx.vb" Inherits="ACS_SIME.frm_proyectoAportesAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1><asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title"><asp:Label runat="server" ID="lblt_subtitulo_pantalla">Recursos Obligados</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_codigo" CssClass="control-label text-bold">Código de Ficha</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                            <asp:Label ID="lbl_id_proyecto" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_codigo_ficha" runat="server"></asp:Label>
                            <asp:Label ID="lbl_guardado" runat="server" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Nombre de Proyecto</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ficha" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ejecutor" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Subregión</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_subregion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Estado</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_estado" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_fecha_actualizacion" CssClass="control-label text-bold">Fecha Actualización</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_fecha_actualizacion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_tasa" CssClass="control-label text-bold">Tasa de Cambio</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txttasacambio" runat="server" MinValue="0" Width="100px">
                                <NumberFormat ZeroPattern="n"></NumberFormat>
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_total_aporte" CssClass="control-label text-bold">Total Aporte</asp:Label>                            
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lblMontoTotal" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_aportes_ficha" CssClass="control-label text-bold">Aportes de la Ficha de Proyecto</asp:Label>
                        </div>
                        <div class="col-sm-8">
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadGrid ID="grd_Aportes" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource11" Width="647px">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="True" />
                                </ClientSettings>
                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_aporteFicha" DataSourceID="SqlDataSource11" ShowGroupFooter="true" ShowFooter="true">
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"
                                        Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"
                                        Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_aporteFicha" DataType="System.Int32"
                                            FilterControlAltText="Filter id_aporteFicha column" ReadOnly="True"
                                            SortExpression="id_aporteFicha" UniqueName="id_aporteFicha" Visible="false">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter Number column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_aporte"
                                            FilterControlAltText="Filter nombre_aporte column" HeaderText="Tipo de aporte"
                                            SortExpression="nombre_aporte" UniqueName="colm_nombre_aporte">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn
                                            DataField="monto_aporte" FilterControlAltText="Filter monto_aporte column"
                                            HeaderText="Aporte estimado" SortExpression="monto_aporte" UniqueName="colm_monto_aporte"
                                            DataType="System.Decimal" DataFormatString="{0:C2}" Aggregate="Sum" FooterText="Total Estimado: ">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                            HeaderText="Monto total obligado" UniqueName="colm_monto_obligado">
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox ID="Totalaporte" runat="server" AutoPostBack="True"
                                                    MinValue="0" OnTextChanged="txt_Aporte_TextChanged" Value="0">
                                                    <NumberFormat ZeroPattern="n" />
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn
                                            FilterControlAltText="filtro comments column"
                                            UniqueName="id_indicador"
                                            HeaderText="Acumular Indicador">
                                            <ItemTemplate>
                                                <telerik:RadComboBox
                                                    ID="cmdIndicador" runat="server" DataSourceID="SqlINDICADOR"
                                                    DataTextField="codigo_indicador" DataValueField="id_indicador" Width="120px">
                                                </telerik:RadComboBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn
                                            DataField="monto_aporte_obligado" FilterControlAltText="Filter monto_aporte_obligado column"
                                            HeaderText="monto_aporte_obligado" SortExpression="monto_aporte_obligado" UniqueName="monto_aporte_obligado" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="calcular_suma"
                                            FilterControlAltText="Filter calcular_suma column" HeaderText="calcular_suma"
                                            SortExpression="calcular_suma" UniqueName="calcular_suma"
                                            Display="false">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldAlias="Fuente" FieldName="nombre_AporteOrigen"
                                                    FormatString="" HeaderText="Origen del Aporte" />
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldAlias="nombre_AporteOrigen" FieldName="nombre_AporteOrigen"
                                                    FormatString="" HeaderText="" />
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ROW_NUMBER() OVER(ORDER BY  nombre_AporteOrigen, nombre_aporte ASC) as Number, id_AporteOrigen, nombre_AporteOrigen, id_aporteFicha, nombre_aporte, monto_aporte, calcular_suma, monto_aporte_obligado, calcular_suma, id_indicador FROM vw_tme_ficha_aportes WHERE id_ficha_proyecto=@id_ficha_proyecto ORDER BY  nombre_AporteOrigen, nombre_aporte ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lbl_id_proyecto" Name="id_ficha_proyecto"
                                        PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <asp:SqlDataSource ID="SqlINDICADOR" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="select id_indicador, codigo_indicador from VW_GR_INDICADOR_APAL WHERE id_ficha_proyecto=@id_ficha_proyecto  ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lbl_id_proyecto" Name="id_ficha_proyecto"
                                        PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" runat="server" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>

