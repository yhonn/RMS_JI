<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratosModIG.aspx.vb" Inherits="RMS_APPROVAL.frm_contratosModIG" %>
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
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Contratistas</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Editar contrato</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="stepwizard">
                            <div class="stepwizard-row setup-panel">
                                 <div class="stepwizard-step" style="width:33%">
                                    <a href="#step-1" id="anchorMod" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                                    <p>
                                        <asp:Label runat="server" ID="Label1">Información de la modificación</asp:Label>
                                    </p>
                                </div>
                                <div class="stepwizard-step" style="width:33%">
                                    <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">2</a>
                                    <p>
                                        <asp:Label runat="server" ID="lblt_informaciongeneral">Información general</asp:Label>
                                    </p>
                                </div>
                                <div class="stepwizard-step" style="width:33%">
                                    <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-default btn-circle">3</a>
                                    <p>
                                        <asp:Label runat="server" ID="lblt_personal_status">Entregables</asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="idModificacion" Value="0" />
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <hr />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Fecha de inicio del contrato</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_inicio" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_inicio" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label34" CssClass="control-label text-bold">Fecha de finalización del contrato</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_finalizacion" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                            ControlToValidate="dt_fecha_finalizacion" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                           <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Número de contrato</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_numero_contrato" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                            ControlToValidate="txt_numero_contrato" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label29" CssClass="control-label text-bold">Valor total del contrato</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server"
                                            ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                          
                        </div>
                        
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label30" CssClass="control-label text-bold">Contratista</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_contratista" AutoPostBack="false" Height="200px" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                            ControlToValidate="cmb_contratista" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label33" CssClass="control-label text-bold">Número Contacto contratista</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_numero_contacto" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server"
                                            ControlToValidate="txt_numero_contacto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label36" CssClass="control-label text-bold">Supervisor</asp:Label>
                                <br />
                                <telerik:RadComboBox ID="cmb_supervisor" AutoPostBack="false" Height="200px" Filter="Contains" runat="server" Width="90%">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server"
                                            ControlToValidate="cmb_supervisor" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label32" CssClass="control-label text-bold">Soporte</asp:Label>
                                <br />
                                <telerik:RadAsyncUpload 
                                        RenderMode="Lightweight" 
                                        runat="server" 
                                        ID="soporte"
                                        OnClientFileUploaded="onClientFileUploaded"
                                        MultipleFileSelection="Automatic" 
                                        Skin="Office2007"
                                        TemporaryFolder="~/Temp" 
                                        PostbackTriggers="btn_guardar"
                                        MaxFileInputsCount="1"
                                        data-clientFilter="application/pdf"
                                        AllowedFileExtensions="pdf"
                                        HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                    </telerik:RadAsyncUpload>
                            </div>
                        </div>
                          <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Objeto del contrato</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_objeto_contrato" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                            ControlToValidate="txt_objeto_contrato" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                          <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Componente</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                             <div class="col-sm-12">
                                  <telerik:RadGrid ID="grd_componente" runat="server"  GridLines="None">
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="true" />
                                    </ClientSettings>
                                    <%--<MasterTableView AllowAutomaticDeletes="true" AutoGenerateColumns="False" CommandItemDisplay="None" 
                                        GroupsDefaultExpanded="false" EnableGroupsExpandAll="true"
                                        DataKeyNames="id_estructura_marcologico" >--%>
                                      <MasterTableView AllowAutomaticDeletes="true" AutoGenerateColumns="False" CommandItemDisplay="None" 
                                        DataKeyNames="id_estructura_marcologico" >
                                        <Columns>
                                             <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="" UniqueName="TemplateColumnAnual">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ctrl_id"  AutoPostBack="false" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px" />
                                                </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="id_estructura_marcologico" FilterControlAltText="Filter id_estructura_marcologico column"
                                                HeaderText="id_estructura_marcologico" SortExpression="id_estructura_marcologico" UniqueName="id_estructura_marcologico"
                                                Visible="False">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion_logica" FilterControlAltText="Filter descripcion_logica column"
                                                HeaderText="Descripción" SortExpression="descripcion_logica" UniqueName="colm_descripcion_logica">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                      <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="-" FieldName="descripcion_logica_padre_3"
                                                        FormatString="" HeaderText="" HeaderValueSeparator="" />
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldAlias="id_estructura_marcologico_3" FieldName="id_estructura_marcologico_3"
                                                        FormatString="" HeaderText="" />
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="-" FieldName="descripcion_logica_padre"
                                                        FormatString="" HeaderText="" HeaderValueSeparator="" />
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldAlias="id_estructura_marcologico_2" FieldName="id_estructura_marcologico_2"
                                                        FormatString="" HeaderText="" />
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                    </MasterTableView>
                                </telerik:RadGrid>
                             </div>
                        </div>
                       
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                       
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar y continuar" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
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
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
        })
        window.onClientFileUploaded = function (radAsyncUpload, args) {
        }
    </script>
</asp:Content>