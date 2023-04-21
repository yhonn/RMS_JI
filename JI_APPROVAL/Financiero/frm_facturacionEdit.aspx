<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_facturacionEdit.aspx.vb" Inherits="RMS_APPROVAL.frm_facturacionEdit" %>
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Editar factura</asp:Label></h3>
                <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:RadNumericTextBox ID="txt_cantidad_productos" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="form-group row" runat="server" visible="false" id="subRegionVisible">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_sub_Region" AutoPostBack="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server"
                                            ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                       <asp:HiddenField runat="server" ID="idFactura" />
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_factura" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_factura" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Nombre</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_nombre" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_nombre" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Dirección</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_direccion" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                            ControlToValidate="txt_direccion" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Departamento</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_departamento" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="cmb_departamento" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                             <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Municipio</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_municipio" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                            ControlToValidate="cmb_municipio" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Teléfono</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_telefono_" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="txt_telefono_" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Documento de identidad y/o NIT</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_numero_identificacion" runat="server"   Width="90%" MinValue="0" NumberFormat-DecimalDigits="0">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="txt_numero_identificacion" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Celular</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_celular_" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                            ControlToValidate="txt_celular_" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:RegularExpressionValidator Display = "Dynamic" CssClass="Error" ControlToValidate = "txt_celular_" ID="RegularExpressionValidator2" ValidationExpression = "^[0-9]{10,10}$" runat="server" ErrorMessage="Número de celular incorrecto."><</asp:RegularExpressionValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Correo electronico</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_correo" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                            ControlToValidate="txt_correo" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txt_correo"  CssClass="Error" ErrorMessage="* Correo incorrecto"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="1"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Código postal</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_codigo_postal" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                            ControlToValidate="txt_codigo_postal" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txt_cantidad_productos" CssClass="Error" Display="Dynamic"
                                ErrorMessage="Campo obligatorio" ValidationGroup="1">El número de productos no puede ser 0</asp:RequiredFieldValidator>
                            </div>
                        </div>
                         
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Registrar concepto</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Cantidad</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_cantidad" runat="server"  Width="90%" MinValue="1">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                            ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Descripción / Concepto</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_descripcion_concepto" runat="server" Rows="3" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                            ControlToValidate="txt_descripcion_concepto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Valor total</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                            ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-8"></div>
                            <div class="col-sm-4 text-right">
                                <telerik:RadButton ID="btn_agregar_concepto" runat="server" CssClass="btn btn-sm" Text="Agregar concepto" ValidationGroup="2" Width="100px">
                                </telerik:RadButton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Detalle</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" 
                                ShowColumnFooters="true"
                                ShowGroupFooters="true"
                                ShowGroupPanel="false">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="True"></Selecting>                                  
                                    <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_facturacion_producto" AllowAutomaticUpdates="True" >
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_facturacion_producto"
                                            FilterControlAltText="Filter id_facturacion_producto column"
                                            SortExpression="id_facturacion_producto" UniqueName="id_facturacion_producto"
                                            Visible="False" HeaderText="id_facturacion_producto"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                            <HeaderStyle Width="2%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                    OnClick="delete_concepto">
                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="cantidad"
                                            FilterControlAltText="Filter cantidad column" HeaderStyle-Width="19%"
                                            HeaderText="Cantidad" SortExpression="cantidad"
                                            UniqueName="colm_cantidad">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion"
                                            FilterControlAltText="Filter descripcion column" HeaderStyle-Width="47%"
                                            HeaderText="Descripción / Concepto" SortExpression="descripcion"
                                            UniqueName="colm_descripcion">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_unitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            DataFormatString="{0:n}" 
                                            FilterControlAltText="Filter valor_unitario column" HeaderStyle-Width="13%"
                                            HeaderText="Valor unitario" SortExpression="valor_unitario"
                                            UniqueName="colm_valor_unitario">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                            FilterControlAltText="Filter valor column" HeaderStyle-Width="19%"
                                            HeaderText="Valor total" SortExpression="valor"
                                            UniqueName="colm_valor">
                                            <HeaderStyle CssClass="wrapWord"  />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" ID="id_concpeto" Value="0" />
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