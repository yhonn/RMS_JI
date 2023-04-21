<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_AdminCatalogosAD" CodeBehind="frm_AdminCatalogosAD.aspx.vb" %>

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
                <h3 class="box-title"><asp:Label runat="server" ID="lblt_subtitulo_pantalla">Catálogos - Nuevo</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_titu_programa" CssClass="control-label text-bold">Programa</asp:Label>

                                <asp:Label ID="lbl_Idcampo" runat="server" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_nombreCam" runat="server" Visible="False"></asp:Label>
                                <asp:Label ID="lbl_tipoCampo" runat="server" Visible="False"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="lbl_programa" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lbl_tipo_catalogo" CssClass="control-label text-bold">Tipo de Catalogo</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmbCatalogo" runat="server" Width="350px"
                                    AutoPostBack="True" CausesValidation="False" OnSelectedIndexChanged="TP_cat_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <asp:CheckBox ID="chkSectorP" runat="server" Text="Sector Productivo" Visible="false" />

                                <asp:Label ID="lbl_tabla" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_tipo" CssClass="control-label text-bold">Tipo</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_tipo" runat="server" DataSourceID="SqlDataSource3" DataTextField="nombre" DataValueField="id" Width="350px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_coincidencias" CssClass="control-label text-bold">Coincidencias</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_coincidencia" runat="server" DataTextField="nombre" DataValueField="id" Width="350px"
                                    AllowCustomText="true"
                                    HighlightTemplatedItems="true"
                                    EnableLoadOnDemand="True"
                                    EmptyMessage="Escriba el elemento a buscar..."
                                    AutoPostBack="true">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Nombre</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="3" TextMode="SingleLine" Width="250px" MaxLength="250" AutoPostBack="true">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txt_descripcion" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <label for="inputEmail3" class="control-label"></label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" id="lblt_validacion" CssClass="label-danger"></asp:Label>
                                <asp:CheckBox runat="server" ID="chk_confirmacion" Text="SI" Visible="false"/>
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

