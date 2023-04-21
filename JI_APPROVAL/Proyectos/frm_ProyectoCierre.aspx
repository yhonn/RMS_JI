<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ProyectoCierre.aspx.vb" Inherits="ACS_SIME.frm_ProyectoCierre" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <section class="content-header">
        <h1>Ficha de Proyecto</h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">Cierre de Proyecto</h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label for="inputEmail3" class="control-label">Código de Ficha</label>
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
                            <label class="control-label">Nombre de Proyecto</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ficha" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Ejecutor</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ejecutor" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Componente</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_componente" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Subregión</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_subregion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Estado</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_estado" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Fecha Actualización</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_fecha_actualizacion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Cambio de estado</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:RadioButtonList ID="rbcierre" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Cerrado" Value="6">
                                </asp:ListItem>
                                <asp:ListItem Text="Finalizado" Value="4">
                                </asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <label class="control-label">Total Aporte</label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lblMontoTotal" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_guardar0" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server"
                            ConfirmText="¿Confirma que desea cancelar la operación?"
                            TargetControlID="btn_guardar0">
                        </cc1:ConfirmButtonExtender>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" CssClass="btn btn-sm pull-right margin-r-5"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
