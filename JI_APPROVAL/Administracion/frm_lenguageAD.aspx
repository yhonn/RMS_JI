<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_lenguageAD.aspx.vb" Inherits="RMS_APPROVAL.frm_lenguageAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Lenguaje Módulo -</asp:Label>
                    <asp:Label runat="server" ID="lbl_modulo"></asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                            </div>
                            <div class="col-sm-8">
                                <asp:HiddenField ID="HiddenProg" runat="server" />
                                <asp:HiddenField ID="hidden_mod" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:Label runat="server" ID="lblt_lenguaje" CssClass="text-bold">Lenguaje a Traducir</asp:Label>
                                <telerik:RadComboBox runat="server" ID="cmb_id_idioma" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Idioma"
                                    DataTextField="descripcion_idioma" DataValueField="id_idioma" ValidationGroup="1" Width="300px" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                                    ControlToValidate="cmb_id_idioma" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                    SelectCommand="SELECT * FROM t_idiomas WHERE id_idioma IN (SELECT id_idioma FROM t_programas_idioma WHERE id_programa = @id_programa)">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="id_programa" ControlID="HiddenProg" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4>
                                    <asp:Label runat="server" ID="lblt_total" CssClass="control-label text-bold"></asp:Label>
                                    <asp:Label runat="server" ID="lbl_pendientes"></asp:Label>
                                </h4>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:RadGrid ID="grd_actividad" runat="server" CellSpacing="0" GridLines="None" ToolTip="Seleccione un idioma">
                                    <MasterTableView AllowAutomaticDeletes="true" AutoGenerateColumns="False" DataKeyNames="id_control">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_control" Display="false"
                                                FilterControlAltText="Filter id_control column" HeaderText="Control"
                                                SortExpression="id_control" UniqueName="id_control">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ctrl_name" DataType="System.Int32"
                                                FilterControlAltText="Filter ctrl_name column" HeaderText="Control"
                                                SortExpression="ctrl_name" UniqueName="colm_ctrl_name">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn FilterControlAltText="Filter valor column"
                                                HeaderText="valor" UniqueName="colm_valor">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="txt_valor" runat="server"
                                                        TextMode="MultiLine" Width="355px">
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="-" FieldName="descripcion" FormatString=""
                                                        HeaderText="" HeaderValueSeparator="" />
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldAlias="ctrl_type" FieldName="ctrl_type"
                                                        FormatString="" HeaderText="" />
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false"></asp:Label>
                            </div>
                        </div>

                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>

