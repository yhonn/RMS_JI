<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurement.aspx.vb" Inherits="RMS_APPROVAL.frm_measurement" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Skills Assessment</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Surveys by school</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Search" Width="100px">
                </telerik:RadButton>
                <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Text="Add New">
                </telerik:RadButton>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                    CellSpacing="0" AllowPaging="True" PageSize="15" ShowGroupPanel="false" GridLines="None"
                    AutoGenerateColumns="False">
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement" AllowAutomaticUpdates="True">
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="nombre_district" FieldName="nombre_district" HeaderText="District"></telerik:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="nombre_district"></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldAlias="name" FieldName="name" HeaderText="School Name"></telerik:GridGroupByField>
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="name"></telerik:GridGroupByField>
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_measurement"
                                FilterControlAltText="Filter id_measurement column"
                                SortExpression="id_measurement" UniqueName="id_measurement"
                                Visible="False" DataType="System.Int32" HeaderText="id_measurement"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true" Display="false">
                                <HeaderStyle Width="10" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar" OnClick="Eliminar_Click">
                                        <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar usuario" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Print">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" Target="_blank" Visible="false"
                                        ToolTip="Imprimir">
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="name"
                                FilterControlAltText="Filter name column"
                                HeaderText="School" SortExpression="name"
                                UniqueName="colm_school_name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_district"
                                FilterControlAltText="Filter nombre_district column"
                                HeaderText="District (Where school is located)" SortExpression="nombre_district"
                                UniqueName="colm_district_name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="moderator"
                                FilterControlAltText="Filter moderator column"
                                HeaderText="Survey Moderators" SortExpression="moderator"
                                UniqueName="colm_moderator">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="participant_number" Aggregate="Sum"
                                FilterControlAltText="Filter participant_number column"
                                HeaderText="# of Participants" SortExpression="participant_number"
                                UniqueName="colm_participant_number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Kind"
                                FilterControlAltText="Filter Kind column"
                                HeaderText="Type of survey" SortExpression="Kind"
                                UniqueName="colm_Kind">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="colm_add">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlk_add" Text="Sale" runat="server" ImageUrl="~/Imagenes/iconos/drop-add.gif"
                                        ToolTip="Add">
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="id_organization"
                                FilterControlAltText="Filter id_organization column"
                                Visible="false" UniqueName="id_organization">
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
