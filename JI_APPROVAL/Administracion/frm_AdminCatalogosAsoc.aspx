<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_AdminCatalogosAsoc" CodeBehind="frm_AdminCatalogosAsoc.aspx.vb" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Catálogos - Asignación a Instrumentos</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_listado_catalogo" runat="server" CssClass="control-label text-bold" Text="Lista de Catalogos"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmbCatalogo" runat="server" Width="325px" AutoPostBack="True" CausesValidation="False">
                                </telerik:RadComboBox>
                                <br />
                                <asp:CheckBoxList ID="chkInstrumentos" runat="server" RepeatColumns="2">
                                </asp:CheckBoxList>
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
                </div>
            </div>
        </div>
    </section>
</asp:Content>

