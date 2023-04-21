<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_tasas_cambio.aspx.vb" Inherits="RMS_APPROVAL.frm_tasas_cambio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>



<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .footer-sales{
            font-size:1em;
            font-weight:bold;
        }
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla_">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Tasa SER</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Fiscal year..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                </telerik:RadButton>
                <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Agregar tasa SER">
                </telerik:RadButton>
                <hr />
                <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"
                    ShowFooter="false" runat="server"
                    ShowColumnFooters="False"
                    ShowGroupFooters="False"
                    ShowGroupPanel="False">
                    <MasterTableView ShowGroupFooter="true" FooterStyle-CssClass="footer-sales" EditFormSettings-EditColumn-HeaderButtonType="LinkButton">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_trimestre_tasa_cambio"
                                FilterControlAltText="Filter id_trimestre_tasa_cambio column"
                                SortExpression="codigo_anio_fiscal" UniqueName="id_trimestre_tasa_cambio"
                                Visible="False" DataType="System.Int32"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                          
                            <telerik:GridBoundColumn UniqueName="colm_anio" DataField="anio"  HeaderText="Fiscal year">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="codigo_anio_fiscal" HeaderText="Quarter" SortExpression="codigo_anio_fiscal"
                                UniqueName="colm_codigo_anio_fiscal">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="colm_mes" DataField="mes" HeaderText="Month">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn Aggregate="Avg" FooterAggregateFormatString="{0:C}" UniqueName="colm_tasa_cambio" DataField="tasa_cambio" HeaderText="Exchange Rate"
                                FooterText="Promedio: ">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="colm_edit" Visible="false">
                                <HeaderStyle Width="10" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_edit" runat="server" Width="10"
                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                        OnClick="Editar_Click">
                                        <asp:Image ID="img_editar" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter delete column" UniqueName="colm_delete" Visible="false">
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
                        </Columns>
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <GroupByFields>
                                    <telerik:GridGroupByField Aggregate="None" FieldName="anio"></telerik:GridGroupByField>
                                </GroupByFields>
                                <SelectFields>
                                    <telerik:GridGroupByField Aggregate="None" FieldName="anio" HeaderText="Fiscal year "></telerik:GridGroupByField>
                                </SelectFields>
                            </telerik:GridGroupByExpression>
                            <telerik:GridGroupByExpression>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="codigo_anio_fiscal" SortOrder="Ascending"></telerik:GridGroupByField>
                                </GroupByFields>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldName="codigo_anio_fiscal" HeaderText="Quarter"></telerik:GridGroupByField>
                                </SelectFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                    </MasterTableView>
                </telerik:RadGrid>
                
            </div>
        </div>
        <asp:HiddenField runat="server" ID="id_tasa_cambio" />
        <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
            <div class="vertical-alignment-helper">
                <div class="modal-dialog modal-sm vertical-align-center">
                    <div class="modal-content">
                        <div class="modal-header modal-danger">
                            <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Remove record</h4>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Would you like to delete the exchange rate?" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                            <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-sm" id="EditTC" data-backdrop="static" data-keyboard="false">
            <div class="vertical-alignment-helper">
                <div class="modal-dialog modal-sm vertical-align-center">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color:#307095; color:#ffffff;">
                            <h4 class="modal-title" runat="server" id="H1">Edit Record</h4>
                        </div>
                        <div class="modal-body">
                            <telerik:RadNumericTextBox ID="txt_tasa_cambio" Width="90%" runat="server" MinValue="0">
                                <NumberFormat ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="modal-footer">
                            <telerik:RadButton ID="btn_edit2" runat="server" CssClass="btn btn-sm btn-primary btn-ok" AutoPostBack="true" Enabled="false" Text="Save">
                            </telerik:RadButton>
                            <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    
    <script type="text/javascript">
        function loadEditModal() {
            $('#EditTC').modal('show');
        }
        function hideDeleteModal() {
            $('#EditTC').modal('hide');
        }
    </script>
</asp:Content>

