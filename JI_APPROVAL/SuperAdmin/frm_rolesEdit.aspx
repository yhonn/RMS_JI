<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_rolesEdit.aspx.vb" Inherits="RMS_APPROVAL.frm_rolesEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Rol - Editar</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-5">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" Enabled="false"></telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lbl_id_rol" runat="server" Font-Bold="True" Font-Names="Arial"
                                    Font-Size="X-Small" ForeColor="#C00000"
                                    Visible="False"></asp:Label>
                                <asp:Label runat="server" ID="lblt_rol" CssClass="control-label text-bold">Nombre Rol</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_nombreRol" runat="server" Rows="3" TextMode="MultiLine" Width="500px" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txt_nombreRol" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Descripción Rol</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="500px" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txt_descripcion" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
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



