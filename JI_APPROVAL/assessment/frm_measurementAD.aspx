<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurementAD.aspx.vb" Inherits="RMS_APPROVAL.frm_measurementAD" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Create Survey for organization</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-8">
                    <div class="box-body">
                        <div class="form-group row">
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
                                <asp:Label runat="server" ID="lblt_schooType" CssClass="control-label text-bold">Type of School</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadComboBox ID="cmb_typeSchool" runat="server" AllowCustomText="true" Filter="Contains" Width="90%" MaxLength="250">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="cmb_typeSchool" CssClass="Error" Display="Dynamic"
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
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_date" CssClass="control-label text-bold">Answer Date</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadDatePicker ID="dp_answer_date" Width="50%" runat="server"></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="dp_answer_date" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre la descripcion" ValidationGroup="1">*</asp:RequiredFieldValidator>                                      
                            </div>
                        </div>
                        <%--<div class="form-group row">
                            <div class="col-sm-4 text-right">
                                <asp:Label runat="server" ID="lblt_end_date" CssClass="control-label text-bold">Period End Date</asp:Label>
                            </div>
                            <div class="col-sm-8">
                                <telerik:RadDatePicker ID="dp_end_date" Width="50%" runat="server"></telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="dp_end_date" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre la descripcion" ValidationGroup="1">*</asp:RequiredFieldValidator>                                      
                            </div>
                        </div>--%>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" ValidationGroup="1" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>