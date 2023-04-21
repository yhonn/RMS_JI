<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActualizarEntregable.aspx.vb" Inherits="RMS_APPROVAL.frm_ActualizarEntregable" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%--<%@ Page validateRequest="false" %>--%>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Subactividades</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Subactividades - Hitos -Entregables</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <asp:HiddenField ID="IdProyecto" runat="server" />
                    <asp:HiddenField ID="idEntregable" runat="server" />
                    <div class="box-body">
                        <div class ="row">
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_acti" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Nombre de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_actividad" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                 <div class=" row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="Label5" runat="server" CssClass="control-label text-bold" Text="Hito"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_nombre_hito" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class=" row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_num_hito" runat="server" CssClass="control-label text-bold" Text="Número del hito"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_num_hito" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <hr />
                                <div class=" row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_entregable" runat="server" CssClass="control-label text-bold" Text="Entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_entregable" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class=" row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_num_entregable" runat="server" CssClass="control-label text-bold" Text="Número del entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_num_entregable" runat="server" ></asp:Label>
                                    </div>
                                </div>
                             
                            </div> 
                        </div>
                        
                    </div>
                    <div runat="server">
                        <div class="box-body">
                            
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-4" id="soporteURL">
                                    <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold">Ingrese la URL del archivo</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_url" runat="server" Width="90%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txt_url" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-left">
                                    <!--Tittle -->
                                    <asp:Label ID="lblt_observaciones_entregable" runat="server"  CssClass="control-label text-bold">Observaciones</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <!--Control -->
                                </div>
                                <div class="col-sm-12">
                                        <telerik:RadTextBox ID="txt_observaciones_entregable" Runat="server" Height="100px"  TextMode="MultiLine" Width="100%">
                                        </telerik:RadTextBox>                                                 
                                    <br />

                                    </div>
                            </div>
                        </div>
                        
                     
                    </div>

                </div>
                
                <div class="box-footer">
                    <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                    </telerik:RadButton>
                    <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" 
                        Width="100px" ValidationGroup="2">
                    </telerik:RadButton>
            </div> 
            </div>
            
        </div>
    </section>
</asp:Content>