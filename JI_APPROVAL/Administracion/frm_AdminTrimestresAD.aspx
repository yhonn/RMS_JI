<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_AdminTrimestresAD" CodeBehind="frm_AdminTrimestresAD.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de periodos activos</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-5">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_programa" runat="server" CssClass="control-label text-bold" Text="Programa"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_programa" runat="server" Width="350px" AutoPostBack="True" Enabled="false">
                                </telerik:RadComboBox>
                                <asp:Label ID="lbl_region" runat="server" Style="font-weight: 700" Visible="False"></asp:Label>
                                <asp:HiddenField runat="server" ID="h_Filter" Value="" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_anio_activo" runat="server" CssClass="control-label text-bold" Text="Año Activo"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_anio" runat="server" Enabled="false"  AutoPostBack="true"></telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_trimestre_activo" runat="server" CssClass="control-label text-bold" Text="Trimestre Activo"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="lbl_trimestre" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_bloqueado" runat="server" CssClass="control-label text-bold" Text="Bloqueado"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label ID="lbl_bloqueado" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_seleccionar_trimestre_activo" runat="server" CssClass="control-label text-bold" Text="Seleccionar trimestre Activo"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="rd_trimestres" runat="server"></asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="rd_trimestres" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblt_bloquear" runat="server" CssClass="control-label text-bold" Text="Bloqueado / Restringir agregar resultados?"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:RadioButtonList ID="rd_periodosBloqueado" runat="server"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1">SI</asp:ListItem>
                                    <asp:ListItem Value="0">NO</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
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
    </section>
</asp:Content>

