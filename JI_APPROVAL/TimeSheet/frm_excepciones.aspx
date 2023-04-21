<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_excepciones.aspx.vb" Inherits="RMS_APPROVAL.frm_excepciones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Hojas de tiempo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Excepciones - festivos</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_fiscal_year" CssClass="control-label text-bold text-right">Año</asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="cmb_year" AutoPostBack="true" runat="server" Width="90%">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="cmb_year" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold text-right">Mes</asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadComboBox ID="cmb_mes" AutoPostBack="true" runat="server" Width="90%">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="cmb_mes" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold text-right">Día</asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadNumericTextBox ID="txt_tasa_cambio" Width="90%" runat="server" MinValue="0">
                            <NumberFormat ZeroPattern="n" />
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_tasa_cambio" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                       <telerik:RadButton ID="btn_guardar" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar excepción" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
                <hr />
                   <div class="col-lg-12" style="width:100%; margin: 0 auto; margin-top:10px;">
                       <div style="max-width:100%; overflow-x:auto">
                           

                          <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"
                                AllowFilteringByColumn="False" AllowSorting="True" Width="100%"
                                ShowFooter="False" AllowPaging="True" runat="server">
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="False" ShowFooter="False"  DataKeyNames="id_excepcion_ts">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_excepcion_ts"
                                            FilterControlAltText="Filter id_excepcion_ts column"
                                            SortExpression="id_tasa_ser" UniqueName="id_excepcion_ts"
                                            Visible="False" DataType="System.Int32" HeaderText="id_excepcion_ts"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                       
                                        <telerik:GridMaskedColumn DataField="anio" HeaderText="Año"
                                            AutoPostBackOnFilter="false" >
                                        </telerik:GridMaskedColumn>
                                         <telerik:GridDateTimeColumn DataField="mes" HeaderText="Mes">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridNumericColumn DataField="dia"
                                             ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                           HeaderText="Día" >
                                        </telerik:GridNumericColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>

                    </div>

                      
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
</asp:Content>