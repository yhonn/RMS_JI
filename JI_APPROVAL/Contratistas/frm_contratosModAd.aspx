<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratosModAd.aspx.vb" Inherits="RMS_APPROVAL.frm_contratosModAd" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .label-content{
            
        }
        .control-label{
            font-size:14px;
        }
        .org-profile{
            padding-left: 0px;
            padding-right: 0px;
            border-radius: 5px;
            border: 2px solid #ebebeb;
            background-color: #f5f5f5;
        }
        .org-profile{
            padding-left: 0px;
            padding-right: 0px;
            padding-bottom: 15px;
        }
        .hr-org{
            padding:0px;
        }
        .hr-org hr{
            margin-top:0px;
            border-top: 2px solid #ebebeb;
            margin-bottom: 10px;
        }
        .tab-content {
            border-left: 1px solid #ddd;
            border-right: 1px solid #ddd;
            border-bottom: 1px solid #ddd;
            border-radius: 0px 0px 5px 5px;
            padding: 10px;
        }
        .RadSearchBox_Silk .rsbButtonSearch{
            height: 18px;
        }
        .RadSearchBox_Silk .rsbInput{
            height: 18px;
        }
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
        #ctl00_MainContent_fileShaperemove0
        {
            display:none;
        }
        .RadUpload .ruCheck 
        { 
            display:none; 
        } 
        .ruActions{
            display:none;
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Modificaciones del contrato</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="stepwizard">
                    <div class="stepwizard-row setup-panel">
                        <div class="stepwizard-step" style="width:33%">
                            <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-primary btn-circle">1</a>
                            <p>
                                <asp:Label runat="server" ID="lblt_informaciongeneral">Información de la modificación</asp:Label>
                            </p>
                        </div>
                        <div class="stepwizard-step" style="width:33%">
                            <a href="#step-1" id="a1" runat="server" type="button" class="btn btn-default btn-circle disabled">2</a>
                            <p>
                                <asp:Label runat="server" ID="Label6">Información general del contrato</asp:Label>
                            </p>
                        </div>
                        <div class="stepwizard-step" style="width:34%">
                            <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-default btn-circle disabled">3</a>
                            <p>
                                <asp:Label runat="server" ID="lblt_personal_status">Entregables</asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <hr />
                    </div>
                </div>


                <div class="col-lg-12">
                    <div class="box-body">
                       <div class ="row">
                           <asp:HiddenField runat="server" ID="idContrato" Value="0" />
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_contrato" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Contratista"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_contratista" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Fecha de inicio"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_inicio" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_fin" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_valor_contrato" runat="server"  CssClass="control-label text-bold" Text="Valor del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_valor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="Supervisor"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_supervisor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold" Text="Objeto del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_objeto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="Label3" runat="server" Text="" Visible="false" />
                        <hr />
                        <div class="row">
                            <div class="form-group row">
                                 <div class="col-sm-12 text-right">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Información de la modificación</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>

                            </div>
                           
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Fecha de la modificación</asp:Label>
                                    <br />
                                    <telerik:RadDatePicker ID="dt_fecha_modificacion" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="dt_fecha_modificacion" CssClass="Error" Display="Dynamic"
                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label34" CssClass="control-label text-bold">Tipo de modificación</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_tipo_modificacion" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                ControlToValidate="cmb_tipo_modificacion" CssClass="Error" Display="Dynamic"
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
                                            PostbackTriggers="btn_guardar_modificacion"
                                            MaxFileInputsCount="1"
                                            data-clientFilter="application/pdf"
                                            AllowedFileExtensions="pdf"
                                            HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                        </telerik:RadAsyncUpload>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Descripción de la modificación</asp:Label>
                                    <br />
                                        <telerik:RadTextBox ID="txt_objetivo_modificacion" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                ControlToValidate="txt_objetivo_modificacion" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar_modificacion" runat="server" Text="Guardar y continuar" Width="160px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right margin-r-5">
                        </telerik:RadButton>
                        <div class="alert-sm bg-blue col-sm-7" runat="server" id="div_mensaje" visible="false">
                            <asp:Label runat="server" ID="lblt_Error" CssClass="text-center text-bold">The value can´t be greater than the funding assign for the project</asp:Label>
                        </div>
                    </div>
                </div>
                <!-- /.box-footer -->
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
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
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

