<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_par_facturacion.aspx.vb" Inherits="RMS_APPROVAL.frm_par_facturacion" %>
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
        div.qsf-right-content .qsf-col-wrap 
        {
            position: static;
        }
 
        .rgEditForm {
            width: auto !important;
        }
 
        * + html .rgEditForm.popUpEditForm {
            width: 800px !important;
        }
 
        .rgEditForm > div + div,
        .RadGrid .rgEditForm {
            height: auto !important;
        }
 
        .rgEditForm > div > table{
            height: 100%;
     
        }
 
        .rgEditForm > div > table > tbody > tr > td {
            padding: 4px 10px;
        }
 
        .rfdSelectBoxDropDown {
            z-index: 100011;
        }
        .rwContent
        {
            overflow-x: hidden !important;
        }
        #ctl00_MainContent_RadWindow2_C_soporte_adjuntoListContainer{
            float: left;
        }
    </style>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rbSubmitChanges">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindow2" />
                    <telerik:AjaxUpdatedControl ControlID="btn_agregar_concepto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Registrar facturación PAR</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-2 text-right">   
                                <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('facturacion_par_vf.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                            </div>
                        </div>
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ConfiguratorPanel">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Información del PAR</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="hidden">
                            <asp:HiddenField runat="server" ID="idPar" Value="0" />
                            <telerik:RadNumericTextBox ID="txt_cantidad_productos" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                            </telerik:RadNumericTextBox>
                        </div>
                        
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-6">
                                <asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Usuario solicita:</asp:Label>
                                <asp:Label runat="server" ID="lblt_usuario_solicita" CssClass="control-label text-bold"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha de solicitud</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_solicitud" AutoPostBack="true" Width="90%" Enabled="false" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="dt_fecha_solicitud" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Tasa SER</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_tasa_ser" enabled="false" runat="server"  Width="90%" MinValue="0">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_tasa_ser" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6" visible="false" runat="server" id="errorTasaSer">
                                <div class="alert alert-danger" role="alert">
                                    La Tasa SER para la fecha seleccionada no se encuantra registrada, contactar con finanzas
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField runat="server" ID="id_tasa_ser" Value="0" />
                        <asp:HiddenField runat="server" ID="id_cargo" Value="0" />
                        <asp:HiddenField runat="server" ID="guardarFacturacion" Value="0" />
                         <asp:HiddenField runat="server" ID="esEdicion" Value="0" />
                        <div runat="server" visible="false" id="continuarPar">
                             <div class="form-group row" runat="server" visible="false" id="subRegionVisible">
                               <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_sub_Region" AutoPostBack="false" Enabled="false" emptymessage="Seleccione..." Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server"
                                                ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                           </div>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Fecha en la que se requieren los servicios</asp:Label>
                                    <br />
                                    <telerik:RadDatePicker ID="dt_fecha_requiere_servicios" AutoPostBack="false" Enabled="false" Width="90%" runat="server">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                                ControlToValidate="dt_fecha_requiere_servicios" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div> 
                               
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Proposito del PAR:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_proposito_par" AutoPostBack="false" Enabled="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                ControlToValidate="cmb_proposito_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Departamento de entrega:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_departamento_entrega" AutoPostBack="true" Enabled="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                ControlToValidate="cmb_departamento_entrega" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Municipio de entrega:</asp:Label>
                                    <br />
                                     <telerik:RadComboBox ID="cmb_municipio_entrega" AutoPostBack="false" Enabled="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                                ControlToValidate="cmb_municipio_entrega" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                            
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Tipo de par:</asp:Label>
                                    <br />
                                     <telerik:RadComboBox ID="cmb_tipo_par" emptymessage="Seleccione..." Enabled="false" AutoPostBack="true" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="cmb_tipo_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Asociar par a:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_cargo_par" AutoPostBack="false" Enabled="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                ControlToValidate="cmb_cargo_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label33" CssClass="control-label text-bold">Asociado a comunicaciones:</asp:Label>
                                    <br />
                                    <asp:RadioButtonList ID="rbn_comunicaciones" runat="server" Enabled="false"
                                        RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                        <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                        <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server"
                                                ControlToValidate="rbn_comunicaciones" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-12">
                                    <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Proposito uso final de los articulos / servicios solicitados</asp:Label>
                                    <br />
                                     <telerik:RadTextBox ID="txt_proposito_servicio" runat="server" Width="98%" Enabled="false" TextMode="MultiLine" Rows="3" MaxLength="1000">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server"
                                                ControlToValidate="txt_proposito_servicio" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="form-group row">
                                <div class="col-sm-12">
                                    <hr/>

                                </div>
                                 
                                    <div class="col-sm-12">
                                        <telerik:RadGrid ID="grd_detalle_par" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowFooter="true" 
                                            ShowColumnFooters="true"
                                            ShowGroupFooters="true"
                                            ShowGroupPanel="false">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="cantidad"
                                                        FilterControlAltText="Filter cantidad column" HeaderStyle-Width="19%"
                                                        HeaderText="Cantidad" SortExpression="cantidad"
                                                        UniqueName="colm_cantidad">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion"
                                                        FilterControlAltText="Filter descripcion column" HeaderStyle-Width="47%"
                                                        HeaderText="Descripción / Servicio requerido" SortExpression="descripcion"
                                                        UniqueName="colm_descripcion">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="unidad_medida"
                                                        FilterControlAltText="Filter unidad_medida column" HeaderStyle-Width="47%"
                                                        HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                                        UniqueName="colm_unidad_medida">
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
                             <div class="form-group row">
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Facturación</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                                  <div class="col-sm-12 text-right">
                                     <telerik:RadButton ID="add_factura" runat="server" Text="Registrar factura" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5">
                                    </telerik:RadButton>
                                 </div>
                            </div>
                            
                             
                            <div class="col-sm-12">
                                <div class="form-group row">
                                     <telerik:RadGrid ID="grd_factura" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowFooter="true" 
                                            ShowColumnFooters="true"
                                            ShowGroupFooters="true"
                                            ShowGroupPanel="false">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_factura" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_par_factura"
                                                        FilterControlAltText="Filter id_par_factura column"
                                                        SortExpression="id_par_factura" UniqueName="id_par_factura"
                                                        Visible="False" HeaderText="id_par_factura"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                     <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                        <HeaderStyle Width="4%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                OnClick="delete_detalle">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="numero_factura"
                                                        FilterControlAltText="Filter numero_factura column" HeaderStyle-Width="19%"
                                                        HeaderText="Número de factura" SortExpression="numero_factura"
                                                        UniqueName="numero_factura">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="proveedor"
                                                        FilterControlAltText="Filter proveedor column" HeaderStyle-Width="47%"
                                                        HeaderText="Proveedor" SortExpression="proveedor"
                                                        UniqueName="proveedor">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="categoria"
                                                        FilterControlAltText="Filter categoria column" HeaderStyle-Width="37%"
                                                        HeaderText="Categoría" SortExpression="categoria"
                                                        UniqueName="categoria">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="valor_total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                        DataFormatString="{0:n}" 
                                                        Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                        FilterControlAltText="Filter valor_total column" HeaderStyle-Width="23%"
                                                        HeaderText="Valor total" SortExpression="valor_total"
                                                        UniqueName="valor_total">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="soporte"
                                                        FilterControlAltText="Filter soporte column" visible="false"
                                                        HeaderText="Soporte" SortExpression="soporte"
                                                        UniqueName="soporte">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="colm_soporte" Visible="true">
                                                        <HeaderStyle Width="4%" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="col_hlk_soporte" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                                ToolTip="Soporte" Target="_new">
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                </div>
                            </div>
                                  <div class="form-group row" runat="server" visible="false" id="asistentes_evento">
                                       <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Número de asistentes al evento:</asp:Label>
                                        <br />
                                         <telerik:RadNumericTextBox ID="txt_nro_asistentes" runat="server"  Width="90%" MaxLength="1000">
                                        </telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                    ControlToValidate="txt_nro_asistentes" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                             </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <hr />
                                </div>
                            </div>

                            <div class="col-sm-12 col-md-12 col-lg-12" visible="false" runat="server" id="solicitar_ajuste">
                                <div class="alert alert-danger" role="alert">
                                    El valor total de facturación supera el 10% del valor del PAR, se debe solicitar ajustes al PAR
                                </div>
                            </div>

                             <%--<div class="form-group row">
                              
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Adjuntos al PAR:</asp:Label>
                                    <br />
                                    <telerik:RadComboBox ID="cmb_adjuntos_par" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                ControlToValidate="cmb_adjuntos_par" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                  <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Soporte:</asp:Label>
                                    <br />
                                    <telerik:RadAsyncUpload 
                                        RenderMode="Lightweight" 
                                        runat="server" 
                                        ID="soporte_adjunto"
                                        OnClientFileUploaded="onClientFileUploaded"
                                        MultipleFileSelection="Automatic" 
                                        Skin="Office2007"
                                        TemporaryFolder="~/Temp" 
                                        PostbackTriggers="btn_guardar,btn_guardar_2"
                                        MaxFileInputsCount="1"
                                        HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                    </telerik:RadAsyncUpload>
                                </div>
                                <div class="col-sm-12 col-md-12 col-lg-6">
                                    <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Descripción de adjuntos</asp:Label>
                                    <br />
                                     <telerik:RadTextBox ID="txt_descripcion_adjuntos" runat="server" TextMode="MultiLine" Rows="3" Width="97%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                </div>
                             
                             </div>
                            <div id="informacion_evento" runat="server" visible="false">
                                <hr />
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label40" CssClass="control-label text-bold">Información del evento</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                                
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Fecha de inicio del evento:</asp:Label>
                                        <br />
                                         <telerik:RadDatePicker ID="dt_fecha_inicio_evento" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                    ControlToValidate="dt_fecha_inicio_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Fecha de finalización del evento:</asp:Label>
                                        <br />
                                        <telerik:RadDatePicker ID="dt_fecha_finalizacion_evento" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                            <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                    ControlToValidate="dt_fecha_finalizacion_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Tipo de evento:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_tipo_evento" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                    ControlToValidate="cmb_tipo_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Número de horas:</asp:Label>
                                        <br />
                                         <telerik:RadNumericTextBox ID="txt_nro_horas" runat="server"  Width="90%" MaxLength="1000">
                                        </telerik:RadNumericTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                    ControlToValidate="txt_nro_horas" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     
                                
                                </div>
                                <div class="form-group row">
                             
                                   <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Nombre del evento:</asp:Label>
                                        <br />
                                         <telerik:RadTextBox ID="txt_nombre_evento" runat="server"  TextMode="MultiLine" Rows="3"  Width="90%" MaxLength="1000">
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                    ControlToValidate="txt_nombre_evento" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                       <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Entidad a cargo del evento:</asp:Label>
                                        <br />
                                         <telerik:RadComboBox ID="cmb_entidad" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                    ControlToValidate="cmb_entidad" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                            <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Evento con recursos apalancados:</asp:Label>
                                            <br />
                                            <asp:RadioButtonList ID="rbn_recursos_apalancados" runat="server"
                                                RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                                <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                                <asp:ListItem Value="0">NO &nbsp;</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                        ControlToValidate="rbn_recursos_apalancados" CssClass="Error" Display="Dynamic"
                                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </div>
                                </div>
                            </div>
                            <div class="form-group row" id="fuentesApalancamiento" runat="server" visible="false">
                             
                                   <div class="col-sm-6 col-md-6 col-lg-3">
                                        <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Fuente del recurso apalancado:</asp:Label>
                                        <br />
                                        <telerik:RadComboBox ID="cmb_fuente_aporte" AutoPostBack="false" Filter="Contains" runat="server" Width="100%">
                                        </telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server"
                                                    ControlToValidate="cmb_fuente_aporte" CssClass="Error" Display="Dynamic"
                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                       <br />
                                       <div class="text-right" style="margin-top:10px;">
                                           <telerik:RadButton ID="btn_agregar_fuente" runat="server" CssClass="btn btn-sm" Text="Agregar fuente" ValidationGroup="3" Width="100px">
                                            </telerik:RadButton>
                                       </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-">
                                        <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Fuentes asociadas:</asp:Label>
                                        <br />
                                        <telerik:RadGrid ID="grd_fuente_aporte" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowGroupPanel="false">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_fuente_aporte" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_aporte"
                                                        FilterControlAltText="Filter id_aporte column"
                                                        SortExpression="id_aporte" UniqueName="id_aporte"
                                                        Visible="False" HeaderText="id_aporte"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="id_par_fuente_aporte"
                                                        FilterControlAltText="Filter id_par_fuente_aporte column"
                                                        SortExpression="id_par_fuente_aporte" UniqueName="id_par_fuente_aporte"
                                                        Visible="false" HeaderText="id_par_fuente_aporte"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                        <HeaderStyle Width="32px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                OnClick="delete_aporte">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="nombre_aporte"
                                                        FilterControlAltText="Filter nombre_aporte column"
                                                        HeaderText="Nombre de la fuente" SortExpression="nombre_aporte"
                                                        UniqueName="nombre_aporte">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                   
                                </div>
                             <div class="form-group row">
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Items / Servicios requeridos</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Descripción / Servicio requerido</asp:Label>
                                    <br />
                                    <telerik:RadTextBox ID="txt_descripcion_concepto" runat="server" Rows="3" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                    </telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                ControlToValidate="txt_descripcion_concepto" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Unidad de medida:</asp:Label>
                                    <br />

                                     <telerik:RadComboBox ID="cmb_unidad_medida" AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="cmb_unidad_medida" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Cantidad</asp:Label>
                                    <br />
                                    <telerik:RadNumericTextBox ID="txt_cantidad" runat="server"  Width="90%" MinValue="1">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                                ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-3">
                                    <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Valor unitario</asp:Label>
                                    <br />
                                    <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%">
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server"
                                                ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-8"></div>
                                <div class="col-sm-4 text-right">
                                    <telerik:RadButton ID="btn_agregar_concepto" runat="server" CssClass="btn btn-sm" Text="Agregar item" ValidationGroup="2" Width="100px">
                                    </telerik:RadButton>
                                </div>
                            </div>

                            <div class="form-group row">
                                <hr />
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_par_detalle" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_par_detalle"
                                                    FilterControlAltText="Filter id_par_detalle column"
                                                    SortExpression="id_par_detalle" UniqueName="id_par_detalle"
                                                    Visible="False" HeaderText="id_par_detalle"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                    <HeaderStyle Width="4%" />
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
                                                    HeaderText="Descripción / Servicio requerido" SortExpression="descripcion"
                                                    UniqueName="colm_descripcion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="unidad_medida"
                                                    FilterControlAltText="Filter unidad_medida column" HeaderStyle-Width="47%"
                                                    HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                                    UniqueName="colm_unidad_medida">
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
                        </div>
                        
                         
                       
                        <div class="form-group row" id="infoComponente" runat="server" visible="false">
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Componente</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
                            </div>
                            <div class="form-group row">
                                 <div class="col-sm-12">
                                      <telerik:RadGrid ID="grd_componente" runat="server"  GridLines="None">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true" />
                                        </ClientSettings>
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
                        </div>--%>
                        <asp:HiddenField runat="server" ID="id_concpeto" Value="0" />
                        <asp:HiddenField runat="server" ID="tipo_delete" Value="0" />
                         <telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="true"  Width="1000" Height="600">
                                <Windows>
                                     <telerik:RadWindow RenderMode="Lightweight" runat="server" Width="1000" Behaviors="Close, Pin, Move" Height="600" id="RadWindow2" Modal="true" EnableShadow="false" VisibleOnPageLoad="false" CssClass="windowcss">
                                         <ContentTemplate>
                                               <div class="form-group row">
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="lbl_tipo_actividad2" CssClass="control-label text-bold">Número de factura</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_numero_factura" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3ss" runat="server"
                                                                    ControlToValidate="txt_numero_factura" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Proveedor</asp:Label>
                                                        <br />
                                                        <telerik:RadTextBox ID="txt_proveedor" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="txt_proveedor" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Categoría de la factura</asp:Label>
                                                        <br />
                                                        <telerik:RadComboBox ID="cmb_categoria_factura"  Filter="Contains" emptymessage="Seleccione..." AutoPostBack="false" runat="server" Width="90%">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17ww" runat="server"
                                                                    ControlToValidate="cmb_categoria_factura" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Soporte</asp:Label>
                                                        <br />
                                                         <telerik:RadAsyncUpload 
                                                            RenderMode="Lightweight" 
                                                            runat="server" 
                                                            ID="soporte_adjunto"
                                                            OnClientFileUploaded="onClientFileUploaded"
                                                            MultipleFileSelection="Automatic" 
                                                            Skin="Office2007"
                                                            TemporaryFolder="~/Temp" 
                                                            PostbackTriggers="btn_agregar_factura"
                                                            MaxFileInputsCount="1"
                                                            HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                                        </telerik:RadAsyncUpload>
                                                       
                                                    </div>
                                                   
                                                </div>
                                                <div class="form-group row">
                                                    <hr />
                                                </div>
                                                 <div class="form-group row">
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Descripción / Servicio requerido</asp:Label>
                                                        <br />
                                                        <telerik:RadTextBox ID="txt_descripcion_concepto" runat="server" Rows="3" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                    ControlToValidate="txt_descripcion_concepto" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                    </div>
                                                     <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Unidad de medida:</asp:Label>
                                                        <br />

                                                         <telerik:RadComboBox ID="cmb_unidad_medida" emptymessage="Seleccione..." AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2222" runat="server"
                                                                    ControlToValidate="cmb_unidad_medida" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Cantidad</asp:Label>
                                                        <br />
                                                        <telerik:RadNumericTextBox ID="txt_cantidad" runat="server"  Width="90%" MinValue="1">
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                                                    ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Valor unitario</asp:Label>
                                                        <br />
                                                        <telerik:RadNumericTextBox ID="txt_valor" runat="server"  Width="90%">
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server"
                                                                    ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-8"></div>
                                                    <div class="col-sm-4 text-right">
                                                        <telerik:RadButton ID="btn_agregar_concepto" runat="server" autopostback="true" CssClass="btn btn-sm" Text="Agregar item" ValidationGroup="3" Width="100px">
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                                 <div class="form-group row">
                                                    <hr />
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-12">
                                                        <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="true" 
                                                            ShowColumnFooters="true"
                                                            ShowGroupFooters="true"
                                                            ShowGroupPanel="false">
                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_factura_detalle" AllowAutomaticUpdates="True" >
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="id_factura_detalle"
                                                                        FilterControlAltText="Filter id_factura_detalle column"
                                                                        SortExpression="id_factura_detalle" UniqueName="id_factura_detalle"
                                                                        Visible="False" HeaderText="id_factura_detalle"
                                                                        ReadOnly="True">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                        <HeaderStyle Width="4%" />
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
                                                                        HeaderText="Descripción / Servicio requerido" SortExpression="descripcion"
                                                                        UniqueName="colm_descripcion">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="unidad_medida"
                                                                        FilterControlAltText="Filter unidad_medida column" HeaderStyle-Width="47%"
                                                                        HeaderText="Unidad de medida" SortExpression="unidad_medida"
                                                                        UniqueName="colm_unidad_medida">
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
                                                <div class="form-group row">
                                                    <div class="col-sm-8"></div>
                                                    <div class="col-sm-4 text-right">
                                                       <telerik:RadButton ID="btn_agregar_factura" runat="server" ValidationGroup="2" AutoPostBack="true" Text="Registrar factura" Width="100px" CssClass="btn btn-sm btn-primary btn-ok import">
                                                    </telerik:RadButton>
                                                    </div>
                                                </div>
                                         </ContentTemplate>
                                     </telerik:RadWindow>

                                </Windows>
                            </telerik:RadWindowManager>
                    </div>
                    <asp:HiddenField runat="server" ID="identity" Value="0" />
                    <asp:HiddenField runat="server" ID="tipo" Value="0" />
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar_finalizar" runat="server" Text="Guardar y finalizar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                         <telerik:RadButton ID="btn_solicitar_ajuste_par" runat="server" Text="Solicitar ajuste del PAR" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
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
    <script type="text/javascript">
        var demo = {};

        window.onClientFileUploaded = function (radAsyncUpload, args) {

        }

        function RowDblClick(sender, eventArgs) {
            sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
        }
        function onPopUpShowing(sender, args) {
            args.get_popUp().className += " popUpEditForm";
        }

        function togglePopupModality() {
            var wnd = getModalWindow();
            wnd.set_modal(!wnd.get_modal());
            setCustomPosition(wnd);

            if (!wnd.get_modal()) {
                document.documentElement.focus();
            }
        }

        function getModalWindow() { return $find(demo.modalWindowID); }

    </script>
</asp:Content>