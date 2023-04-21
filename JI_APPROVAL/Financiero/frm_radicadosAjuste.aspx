<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_radicadosAjuste.aspx.vb" Inherits="RMS_APPROVAL.frm_radicadosAjuste" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .RadUpload_Office2007 .ruStyled .ruFileInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 input.ruFakeInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 .ruButton{
            border: 1px solid #e5e5e5;
            color: #767676;
            background-color: #fff;
            background-image:none
        }

        .upper
        {
            text-transform:uppercase;
        }
       
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Financiero</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Radicados</asp:Label></h3>
                <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_productos" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        <asp:HiddenField runat="server" ID="h_Filter_editar_radicado" Value="" />  
                       <div class="form-group row" runat="server" visible="false" id="subRegionVisible">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Regional:</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_sub_Region" AutoPostBack="false" enabled="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server"
                                            ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                               <asp:Label runat="server" ID="lbl_tipo_actividad_" CssClass="control-label text-bold">Fecha de radicación</asp:Label>
                                <br />
                                 <asp:Label runat="server" ID="lbl_fecha_solicitud" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Id a quién se va a pagar</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_identificacion_tercero_a_pagar_" enabled="false" NumberFormat-DecimalDigits="0" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_identificacion_tercero_a_pagar_" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Tercero (a quién se va a pagar)</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_tercero_a_pagar" runat="server" enabled="false"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                            ControlToValidate="txt_tercero_a_pagar" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Documento <small>(Factura, Cta Cobro, PTR, etc.)</small></asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_documento" runat="server" Rows="3" enabled="false" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="txt_documento" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Valor de la factura</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_valor_factura" runat="server" enabled="false" Width="90%">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txt_valor_factura" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>   
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Caracter</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_caracter" AutoPostBack="false" enabled="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="cmb_caracter" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            
                            <div class="col-sm-12">
                                <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Observaciones</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_observaciones" runat="server" enabled="false" Rows="3" TextMode="MultiLine" Width="97%" MaxLength="1000">
                                </telerik:RadTextBox>
                            </div>
                            
                        </div>
                        <div class="form-group row">
                             <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Código GJ</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_codigo_gj" runat="server" Width="98%"  MaxLength="1000">
                                </telerik:RadTextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Fecha de pago / contabilización</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_pago" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                            </telerik:RadDatePicker>
                            </div>
                        </div>
                      
                    </div>
                    <asp:HiddenField runat="server" ID="idRadicado" Value="0" />
                    <asp:HiddenField runat="server" ID="anularRadicadoV" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar y Continuar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
                </div>
                <div class="modal fade" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog  modal-dialog-centered">
                            <div class="modal-content modal-lg ">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar concepto</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Delete" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>