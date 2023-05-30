<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_anticiposVerificacion.aspx.vb" Inherits="RMS_APPROVAL.frm_anticiposVerificacion" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ register tagprefix="uc" tagname="Confirm" src="~/Controles/ModalConfirm.ascx" %>
<%@ register tagprefix="uc" tagname="ReturnConfirm" src="~/Controles/DeleteConfirm.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .RadUpload_Office2007 .ruStyled .ruFileInput {
            border-color: #e5e5e5;
        }

        .RadUpload_Office2007 input.ruFakeInput {
            border-color: #e5e5e5;
        }

        .RadUpload_Office2007 .ruButton {
            border: 1px solid #e5e5e5;
            color: #767676;
            background-color: #fff;
            background-image: none
        }

        .upper {
            text-transform: uppercase;
        }
    </style>
    <uc:confirm runat="server" id="MsgGuardar" />
    <uc:returnconfirm runat="server" id="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Anticipo - verificación de los fondos</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="subRegionVisible">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_sub_Region" autopostback="false" enabled="false" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                </telerik:radcombobox>
                            </div>

                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="lbl_tipo_actividad_" CssClass="control-label text-bold">Fecha que se requiere el anticipo</asp:Label>
                                <br />
                                <telerik:raddatepicker id="dt_fecha_anticipo" enabled="false" autopostback="false" width="90%" runat="server">
                                    <calendar userowheadersasselectors="False" usecolumnheadersasselectors="False" viewselectortext="x"></calendar>
                                    <datepopupbutton imageurl="" hoverimageurl=""></datepopupbutton>
                                </telerik:raddatepicker>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Medio de pago</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_medio_pago" enabled="false" autopostback="false" emptymessage="Seleccione..." runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="cmb_medio_pago" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="otro_medio_pago">
                                <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Ingrese el medio de pago:</asp:Label>
                                <br />
                                <telerik:radtextbox id="txt_otro_medio_pago" enabled="false" runat="server" width="90%" maxlength="1000">
                                </telerik:radtextbox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txt_otro_medio_pago" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Observaciones medio de pago</asp:Label>
                                <br />
                                <telerik:radtextbox id="txt_detalle_medio_pago" enabled="false" runat="server" width="90%" maxlength="1000">
                                </telerik:radtextbox>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="costo_giro">
                                <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Costo total comisión giro</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox id="txt_costo_giro" enabled="false" runat="server" width="90%" maxlength="1000">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Tipo de anticipo</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_tipo_anticipo" enabled="false" autopostback="false" emptymessage="Seleccione..." runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="cmb_tipo_anticipo" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Código del PAR:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_par" autopostback="false" enabled="false" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="cmb_par" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Motivo del anticipo</asp:Label>
                                <br />
                                <telerik:radtextbox cssclass="upper" id="txt_motivo_anticipo" enabled="false" runat="server" rows="3" textmode="MultiLine" width="97%" maxlength="1000">
                                </telerik:radtextbox>
                            </div>
                        </div>
                    </div>
                    <div runat="server">
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Rutas</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:RadGrid ID="grd_rutas" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                    ShowFooter="true" 
                                    ShowColumnFooters="true"
                                    ShowGroupFooters="true"
                                    ShowGroupPanel="false">
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_ruta" AllowAutomaticUpdates="True" >
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_anticipo_ruta"
                                                FilterControlAltText="Filter id_anticipo_ruta column"
                                                SortExpression="id_anticipo_ruta" UniqueName="id_anticipo_ruta"
                                                Visible="False" HeaderText="id_anticipo_ruta"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="colm_personas" Visible="true">
                                                <HeaderStyle Width="32px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="col_hlk_editar" runat="server" Width="32px"
                                                        ImageUrl="../Imagenes/iconos/Family.png" ToolTip="Eliminar"
                                                        OnClick="participantes">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/Family.png" Style="border-width: 0px;" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="ciudad_salida"
                                                FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="135px"
                                                HeaderText="Municipio / ciudad de salida" SortExpression="ciudad_salida"
                                                UniqueName="colm_ciudad_salidan">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="zona_rural"
                                                FilterControlAltText="Filter zona_rural column" HeaderStyle-Width="140px"
                                                HeaderText="Zona rural" SortExpression="zona_rural"
                                                UniqueName="colm_zona_rural">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ciudad_llegada"
                                                FilterControlAltText="Filter ciudad_llegada column"  HeaderStyle-Width="135px"
                                                HeaderText="Municipio / ciudad de llegada" SortExpression="ciudad_llegada"
                                                UniqueName="colm_ciudad_llegada">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn  HeaderStyle-Width="150px"
                                              HeaderText="Información adicional" UniqueName="informacion_adicional" >
                                             <ItemTemplate>                                     
                                                 <%--<asp:Label runat="server" ID="lblt_rms_code" CssClass="control-label text-bold" Text="Código RMS: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_rms_code" CssClass="control-label" Text=""></asp:Label><br />--%>
                                                 <asp:Label runat="server" ID="lblt_tiempo_estimado" CssClass="control-label text-bold" Text="Tiepo estimado: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_tiempo_estimado" CssClass="control-label" Text=""></asp:Label><br />
                                                 <asp:Label runat="server" ID="lblt_observaciones_trayecto" CssClass="control-label text-bold" Text="Observaciones trayecto: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_observaciones_trayecto" CssClass="control-label" Text=""></asp:Label><br />
                                                 <asp:Label runat="server" ID="lblt_observaciones_ruta" CssClass="control-label text-bold" Text="Observaciones adicionales: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_observaciones_ruta" CssClass="control-label" Text=""></asp:Label>
                                                 <br />

                                                 </ItemTemplate>
                                                  <HeaderStyle Width="150px" />
                                                  <ItemStyle Width="150px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="tiempo_estimado"
                                                FilterControlAltText="Filter tiempo_estimado column" HeaderText="tiempo_estimado" UniqueName="tiempo_estimado" Visible="true" Display="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="observaciones_ruta"
                                                FilterControlAltText="Filter observaciones_ruta column" HeaderText="observaciones_ruta" UniqueName="observaciones_ruta" Visible="true" Display="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="observaciones_trayecto"
                                                FilterControlAltText="Filter observaciones_trayecto column" HeaderText="observaciones_trayecto" UniqueName="observaciones_trayecto" Visible="true" Display="false">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="cantidad_personas"
                                                FilterControlAltText="Filter cantidad_personas column" HeaderStyle-Width="75px"
                                                HeaderText="# personas" SortExpression="cantidad_personas"
                                                UniqueName="colm_cantidad_personas">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="requiere_estipendio_text"
                                                FilterControlAltText="Filter requiere_estipendio_text column"  HeaderStyle-Width="70px"
                                                HeaderText="Estipendio" SortExpression="requiere_estipendio_text"
                                                UniqueName="colm_requiere_estipendio_text">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_trayecto" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                DataFormatString="{0:n}" 
                                                FilterControlAltText="Filter valor_trayecto column"  HeaderStyle-Width="120px"
                                                HeaderText="Valor ida y regreso" SortExpression="valor_trayecto"
                                                UniqueName="colm_valor_trayecto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="sub_total_ruta" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total ida y regreso: {0:n}</b>" FooterText="Valor total ida y regreso: "
                                                FilterControlAltText="Filter sub_total_ruta column" HeaderStyle-Width="120px"
                                                HeaderText="Valor total ida y regreso" SortExpression="sub_total_ruta"
                                                UniqueName="colm_valor_trayecto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total_estipendio" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor estipendio: {0:n}</b>" FooterText="Valor estipendio: "
                                                FilterControlAltText="Filter valor_trayecto column" HeaderStyle-Width="120px"
                                                HeaderText="Valor estipendio" SortExpression="valor_total_estipendio"
                                                UniqueName="colm_valor_total_estipendio">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total_ruta" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                FilterControlAltText="Filter valor_total_ruta column" HeaderStyle-Width="120px"
                                                HeaderText="Valor total" SortExpression="valor_total_ruta"
                                                UniqueName="colm_valor_total_ruta">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12 text-center">                                              
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                            <br /><br />
                        </div>
                    </div>
                     <div class="form-group row" runat="server" visible="false" id="alerta_dias">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="alert alert-danger" role="alert">
                                <asp:Label ID="lbl_alerta_solicitud" runat="server" Font-Names="Arial" Visible="true">Los anticipos se deben solicitar minimo con 3 días habiles de anticipación!</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:radbutton id="btn_salir" runat="server" text="Salir" width="100px" causesvalidation="false" cssclass="btn btn-sm pull-right">
                        </telerik:radbutton>
                        <telerik:radbutton id="btn_guardar" runat="server" text="Guardar" enabled="false" autopostback="true" cssclass="btn btn-sm pull-right margin-r-5"
                            validationgroup="1">
                        </telerik:radbutton>
                        <telerik:radbutton id="btn_guardar_enviar" runat="server" text="Guardar y envíar por aprobación" enabled="false" autopostback="true" cssclass="btn btn-sm pull-right margin-r-5"
                            validationgroup="1">
                        </telerik:radbutton>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
                </div>
                 <telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="true"  Width="700" Height="400">
                                <Windows>
                                     <telerik:RadWindow RenderMode="Lightweight" runat="server" Width="700" Behaviors="Close, Pin, Move" Height="400" id="RadWindow2" Modal="true" EnableShadow="false" VisibleOnPageLoad="false" CssClass="windowcss">
                                         <ContentTemplate>
                                               <div class="form-group row">
                                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Numero de documento</asp:Label>
                                                        <br />
                                                         <telerik:RadNumericTextBox ID="txt_numero_documento" runat="server" decimal Width="90%" MinValue="0">
                                                             <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                    ControlToValidate="txt_numero_documento" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Tipo de documento</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_tipo_documento" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                                    ControlToValidate="txt_tipo_documento" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Nombres</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_nombre" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                    ControlToValidate="txt_nombre" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Primer apellido</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_primer_apellido" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                    ControlToValidate="txt_primer_apellido" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Segundo apellido</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_segundo_apellido" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                                    ControlToValidate="txt_segundo_apellido" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Valor</asp:Label>
                                                        <br />
                                                         <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadNumericTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                                    ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Número de teléfono</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_numero_telefono" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                                    ControlToValidate="txt_numero_telefono" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <telerik:RadButton ID="btn_guardar_participante" runat="server" autopostback="true" CssClass="btn btn-sm" Text="Agregar persona" ValidationGroup="3" Width="100px" style="margin-top:10px;">
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                               
                                               <div class="form-group row">
                                                    <div class="col-sm-12">
                                                        <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="false" 
                                                            ShowColumnFooters="false"
                                                            ShowGroupFooters="false"
                                                            ShowGroupPanel="false">
                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_participante" AllowAutomaticUpdates="True" >
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="id_participante"
                                                                        FilterControlAltText="Filter id_participante column"
                                                                        SortExpression="id_participante" UniqueName="id_participante"
                                                                        Visible="False" HeaderText="id_participante"
                                                                        ReadOnly="True">
                                                                    </telerik:GridBoundColumn>
                                                                   
                                                                    <telerik:GridBoundColumn DataField="numero_documento"
                                                                        FilterControlAltText="Filter numero_documento column" HeaderStyle-Width="30%"
                                                                        HeaderText="Participante" SortExpression="numero_documento"
                                                                        UniqueName="numero_documento">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="nombres"
                                                                        FilterControlAltText="Filter nombres column" HeaderStyle-Width="30%"
                                                                        HeaderText="Nombres" SortExpression="nombres"
                                                                        UniqueName="nombres">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="segundo_apellido"
                                                                        FilterControlAltText="Filter segundo_apellido column" HeaderStyle-Width="30%"
                                                                        HeaderText="Segundo apellido" SortExpression="segundo_apellido"
                                                                        UniqueName="segundo_apellido">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="telefono"
                                                                        FilterControlAltText="Filter telefono column" HeaderStyle-Width="30%"
                                                                        HeaderText="Teléfono" SortExpression="telefono"
                                                                        UniqueName="telefono">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="valor"
                                                                        FilterControlAltText="Filter valor column" HeaderStyle-Width="30%"
                                                                        HeaderText="Valor" SortExpression="valor"
                                                                        UniqueName="valor">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                                 <div class="form-group row">
                                                    <hr />
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-12">
                                                       
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-8"></div>
                                                    <div class="col-sm-4 text-right">
                                                       <telerik:RadButton ID="btn_guardar_finalizar" runat="server" ValidationGroup="2" AutoPostBack="true" Text="Guardar rol" Width="100px" CssClass="btn btn-sm btn-primary btn-ok import">
                                                    </telerik:RadButton>
                                                    </div>
                                                </div>
                                         </ContentTemplate>
                                     </telerik:RadWindow>

                                </Windows>
                            </telerik:RadWindowManager>
                <asp:HiddenField runat="server" ID="id_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="id_ruta_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />
                <asp:HiddenField runat="server" ID="tiempo_estimado"/>
                <asp:HiddenField runat="server" ID="observaciones"/>
                <asp:HiddenField runat="server" ID="idanticipo" Value="0" />
                <asp:HiddenField runat="server" ID="valor_desayuno" Value="0" />
                <asp:HiddenField runat="server" ID="valor_almuerzo" Value="0" />
                <asp:HiddenField runat="server" ID="valor_cena" Value="0" />
                <asp:HiddenField runat="server" ID="habilitar_registro" Value="0" />
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
