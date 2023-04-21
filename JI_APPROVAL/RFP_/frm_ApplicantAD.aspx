<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.master" Inherits="RMS_APPROVAL.frm_ApplicantAD" CodeBehind="frm_ApplicantAD.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%--<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>--%>
<%--<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeograficaBHA.ascx" %>--%>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeograficaJI.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">REQUEST FOR PROPOSAL</asp:Label>
         </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Potential Grantees - New</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_codigo" CssClass="control-label text-bold hidden">Grantees Code</asp:Label>
                                <asp:Label runat="server" ID="lblt_tipo_organizacion" CssClass="control-label text-bold">Organization Type</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="lbl_IDapplicant" runat="server" CssClass="hidden" Text=""></asp:Label>
                                <asp:Label ID="lbl_codigo_ejecutor" runat="server" Font-Bold="True" CssClass="hidden"
                                    Font-Strikeout="False" Font-Underline="False" Text="Código ejecutor:"></asp:Label>
                                <telerik:RadComboBox runat="server" ID="cmb_tipo_organizacion" Filter="Contains" EmptyMessage="Select" Width="90%"></telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="cmb_tipo_organizacion" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Grantee Name</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_nombreEjecutor" runat="server" Width="90%" MaxLength="1000"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txt_nombreEjecutor" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br />
                                <asp:CheckBox ID="chk_Active"  runat="server" Text="&nbsp;&nbsp;ACTIVE" CssClass="btn btn-sm" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_nombre_corto" CssClass="control-label text-bold">Short Name</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_NombreCorto" runat="server" Width="90%" MaxLength="200"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txt_NombreCorto" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_nit" CssClass="control-label text-bold">Identifier Number</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_nit" runat="server" Rows="3" Width="90%" MaxLength="50">
                                </telerik:RadTextBox>
                               
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_telefono" CssClass="control-label text-bold">Phone</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_telefono_ejecutor" runat="server" Rows="3" Width="90%" MaxLength="50">
                                </telerik:RadTextBox>
                              
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_email" CssClass="control-label text-bold">Email</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_email" runat="server" MaxLength="100" Width="90%">
                                </telerik:RadTextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txt_email" ErrorMessage="* Error en Email"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="1"></asp:RegularExpressionValidator>                                
                            </div>
                        </div>
                        <div class="form-group row">
                             <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_fecha_constitucion" CssClass="control-label text-bold">Registration Date</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadDatePicker ID="dt_fecha_inicio" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                </telerik:RadDatePicker>                                
                            </div>
                        </div>                       

                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_contact_name" CssClass="control-label text-bold">Contact Name</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_contact_name" runat="server" Rows="3" Width="90%"
                                    MaxLength="500">
                                </telerik:RadTextBox>                               
                            </div>
                             <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_contact_lastname" CssClass="control-label text-bold">Contact Last Name</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_contact_lastname" runat="server" Rows="3" Width="90%"
                                    MaxLength="500">
                                </telerik:RadTextBox>                               
                            </div>                            
                        </div>

                         <div class="form-group row">
                             <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_cedula" CssClass="control-label text-bold">Contact ID</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_cedula" runat="server" Rows="3" Width="90%"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                               
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_email2" CssClass="control-label text-bold">Contact Email</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_email2" runat="server" MaxLength="100" Width="90%">
                                </telerik:RadTextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ControlToValidate="txt_email2" ErrorMessage="* Error en Email"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="1"></asp:RegularExpressionValidator>
                                
                            </div>
                        </div>

                        <div class="form-group row">

                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_telefono_rep" CssClass="control-label text-bold">Primary Contact Phone</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_telefono_representante" runat="server" Rows="3" Width="90%"
                                    MaxLength="500">
                                </telerik:RadTextBox>                               
                            </div> 
                        </div>
                        <hr />
                        <uc:UbicacionGeografica runat="server" ID="ctrl_ubicacionGeografica" /><br />
                        <div class="form-group row">
                            <div class="col-sm-2">
                                  <asp:Label runat="server" ID="lblt_direccion" CssClass="control-label text-bold">Primary Contact Address</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_direccion" runat="server" Width="90%" MaxLength="200" TextMode="MultiLine" Rows="3" ></telerik:RadTextBox>
                            </div>  
                             <div class="col-sm-2">
                                  <asp:Label runat="server" ID="lblt_city" CssClass="control-label text-bold">Contact City</asp:Label>
                            </div>
                            <div class="col-sm-4">
                               <telerik:RadTextBox ID="txt_cicty" runat="server" Width="90%" MaxLength="200" ></telerik:RadTextBox>
                            </div>
                        </div>
                        
                        <div class="form-group row hide">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_billing_info" CssClass="control-label text-bold">Cuenta de Cobro</asp:Label>
                           </div>
                            <div class="col-sm-4">
                                <telerik:RadTextBox ID="txt_billin_info" Runat="server" EmptyMessage="Billing info here.."  Width="90%" ValidationGroup="1" TextMode="MultiLine" Rows="5" MaxLength="499">
                                </telerik:RadTextBox>                                                                  
                            </div>
                        </div>

                      
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false" Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>
