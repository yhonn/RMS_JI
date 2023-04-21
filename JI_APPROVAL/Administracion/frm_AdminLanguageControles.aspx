<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_AdminLanguageControles.aspx.vb" Inherits="RMS_APPROVAL.frm_AdminLanguageControles" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Menú - Idioma</asp:Label></h3>
            </div>
            <div class="box-body">
                <asp:Label runat="server" ID="lblt_lenguaje" CssClass="text-bold">Lenguaje a Traducir</asp:Label>
                <telerik:RadComboBox runat="server" ID="cmb_idioma" EmptyMessage="Seleccione un Idioma"
                    Width="383px" AutoPostBack="true">
                </telerik:RadComboBox>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" CellSpacing="0" GridLines="None" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_control_otro" AllowAutomaticUpdates="True">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_control_otro"
                                FilterControlAltText="Filter id_control_otro column"
                                SortExpression="id_control_otro" UniqueName="id_control_otro"
                                Visible="False" DataType="System.Int32" HeaderText="id_control_otro"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="pertenece" SortExpression="pertenece" AllowSorting="true"
                                FilterControlAltText="Filter pertenece column" HeaderText="Pertenece A" UniqueName="colm_pertenece">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="control_code"
                                FilterControlAltText="Filter control_code column"
                                HeaderText="Menú Superior" SortExpression="control_code"
                                UniqueName="colm_control_code">
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
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <div class="form-group row">
                <div class="col-sm-4 text-right">
                </div>
                <div class="col-sm-8">
                    <asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false"></asp:Label>
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
    </section>
    <!-- /.content -->
</asp:Content>

