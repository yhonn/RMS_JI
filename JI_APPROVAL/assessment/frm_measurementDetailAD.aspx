<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurementDetailAD.aspx.vb" Inherits="RMS_APPROVAL.frm_measurementDetailAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Skills Assessment</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Survey Participants by School - Upload</asp:Label></h3>
                <asp:Label runat="server" ID="lbl_survey_type" Visible="false"></asp:Label>
            </div>
            <div class="box-body">
                <div class="col-lg-8">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_school_name" CssClass="control-label text-bold">School</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_school_name"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Moderator</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_moderator"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_answer_date" CssClass="control-label text-bold">Survey Answer Date</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_answer_date"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Participant Number</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_participant_number"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_survey_name" CssClass="control-label text-bold">Survey Type</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_survey_name"></asp:Label>
                            </div>
                            <br />
                        </div>
                        <%--<div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_organization" CssClass="control-label text-bold">Name of School</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_organization" runat="server" AllowCustomText="true" Filter="Contains" Width="90%" MaxLength="250">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="cmb_organization" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_survey_type" CssClass="control-label text-bold">Survey Type</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_survey_type" runat="server" AllowCustomText="true" Filter="Contains" Width="90%" MaxLength="250">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="cmb_survey_type" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_moderator" CssClass="control-label text-bold">Name of moderator</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadTextBox ID="txt_moderator" runat="server" Width="90%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rv2" runat="server"
                                    ControlToValidate="txt_moderator" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_participant_number" CssClass="control-label text-bold">Number of participants who answered the survey</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadNumericTextBox ID="txt_participant_number" runat="server" Width="90%" MinValue="0" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txt_participant_number" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_excel_template" CssClass="control-label text-bold">Excel Template</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="asyncArchivo"
                                    TemporaryFolder="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded" AllowedFileExtensions="xlsx" />
                                <telerik:RadButton ID="btn_guardar" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                    Text="Upload" ValidationGroup="2" Width="75px">
                                </telerik:RadButton>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <asp:Label runat="server" ID="lbl_error"></asp:Label>
                        <asp:Label runat="server" ID="lbl_archivo_uploaded" CssClass="text-red"></asp:Label>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>
