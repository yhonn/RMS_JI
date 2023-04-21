<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_corte_facturacion.aspx.vb" Inherits="RMS_APPROVAL.frm_corte_facturacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla_2">Cierres</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_fiscal_year_" CssClass="control-label text-bold text-right">Fecha de cierre</asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <telerik:RadDatePicker ID="dt_fecha_cierre" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                        </telerik:RadDatePicker>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_cierre" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
              
                <div class="form-group row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                       <telerik:RadButton ID="btn_guardar" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar fecha de cierre" ValidationGroup="1">
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
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="False" ShowFooter="False"  DataKeyNames="id_cierre">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_cierre"
                                            FilterControlAltText="Filter id_cierre column"
                                            SortExpression="id_tasa_ser" UniqueName="id_cierre"
                                            Visible="False" DataType="System.Int32" HeaderText="id_cierre"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                       
                                        <telerik:GridMaskedColumn DataField="fecha_cierre" HeaderText="Fecha de cierre" DataFormatString="{0:MM/dd/yyyy}"
                                            AutoPostBackOnFilter="false" >
                                        </telerik:GridMaskedColumn>
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