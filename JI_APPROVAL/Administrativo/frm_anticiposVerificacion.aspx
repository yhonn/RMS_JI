<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site.Master" codebehind="frm_anticiposVerificacion.aspx.vb" inherits="RMS_APPROVAL.frm_anticiposVerificacion" %>

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
                                <telerik:radnumerictextbox id="txt_costo_giro" enabled="false" runat="server" width="90%" maxlength="1000">
                                </telerik:radnumerictextbox>
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
                                <h4 class="text-center">
                                    <asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Rutas</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 text-right">
                                <telerik:radbutton id="btn_add_ruta" runat="server" cssclass="btn btn-sm" text="Agregar ruta" validationgroup="6" width="100px">
                                </telerik:radbutton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:radgrid id="grd_rutas" runat="server" allowautomaticdeletes="True" cellspacing="0" allowpaging="True" pagesize="100" autogeneratecolumns="False" width="100%"
                                    showfooter="true"
                                    showcolumnfooters="true"
                                    showgroupfooters="true"
                                    showgrouppanel="false">
                                    <clientsettings enablerowhoverstyle="true">
                                        <selecting allowrowselect="True"></selecting>
                                        <resizing allowcolumnresize="true" allowresizetofit="true" />
                                    </clientsettings>
                                    <mastertableview autogeneratecolumns="False" datakeynames="id_anticipo_ruta" allowautomaticupdates="True">
                                        <columns>
                                            <telerik:gridboundcolumn datafield="id_anticipo_ruta"
                                                filtercontrolalttext="Filter id_anticipo_ruta column"
                                                sortexpression="id_anticipo_ruta" uniquename="id_anticipo_ruta"
                                                visible="False" headertext="id_anticipo_ruta"
                                                readonly="True">
                                            </telerik:gridboundcolumn>
                                            <telerik:gridtemplatecolumn uniquename="colm_personas" visible="false">
                                                <headerstyle width="30px" />
                                                <itemtemplate>
                                                    <asp:LinkButton ID="col_hlk_editar" runat="server" Width="30px"
                                                        ImageUrl="../Imagenes/iconos/Family.png" ToolTip="Eliminar"
                                                        OnClick="participantes">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/Family.png" Style="border-width: 0px;" />
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </telerik:gridtemplatecolumn>
                                            <telerik:gridboundcolumn datafield="num_ruta"
                                                filtercontrolalttext="Filter num_ruta column" headerstyle-width="30px"
                                                headertext="No" sortexpression="num_ruta"
                                                uniquename="colm_num_ruta">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="ciudad_salida"
                                                filtercontrolalttext="Filter ciudad_origen column" headerstyle-width="135px"
                                                headertext="Municipio / ciudad de salida" sortexpression="ciudad_salida"
                                                uniquename="colm_ciudad_salidan">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="zona_rural"
                                                filtercontrolalttext="Filter zona_rural column" headerstyle-width="120px"
                                                headertext="Zona rural" sortexpression="zona_rural"
                                                uniquename="colm_zona_rural">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="ciudad_llegada"
                                                filtercontrolalttext="Filter ciudad_llegada column" headerstyle-width="135px"
                                                headertext="Municipio / ciudad de llegada" sortexpression="ciudad_llegada"
                                                uniquename="colm_ciudad_llegada">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridtemplatecolumn headerstyle-width="150px"
                                                headertext="Información adicional" uniquename="informacion_adicional">
                                                <itemtemplate>
                                                    <%--<asp:Label runat="server" ID="lblt_rms_code" CssClass="control-label text-bold" Text="Código RMS: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_rms_code" CssClass="control-label" Text=""></asp:Label><br />--%>
                                                    <asp:Label runat="server" ID="lblt_tiempo_estimado" CssClass="control-label text-bold" Text="Tiepo estimado: "></asp:Label>
                                                    <br />
                                                    &nbsp;<asp:Label runat="server" ID="lbl_tiempo_estimado" CssClass="control-label" Text=""></asp:Label><br />
                                                    <asp:Label runat="server" ID="lblt_observaciones_trayecto" CssClass="control-label text-bold" Text="Observaciones trayecto: "></asp:Label>
                                                    <br />
                                                    &nbsp;<asp:Label runat="server" ID="lbl_observaciones_trayecto" CssClass="control-label" Text=""></asp:Label><br />
                                                    <asp:Label runat="server" ID="lblt_observaciones_ruta" CssClass="control-label text-bold" Text="Observaciones adicionales: "></asp:Label>
                                                    <br />
                                                    &nbsp;<asp:Label runat="server" ID="lbl_observaciones_ruta" CssClass="control-label" Text=""></asp:Label>
                                                    <br />

                                                </itemtemplate>
                                                <headerstyle width="150px" />
                                                <itemstyle width="150px" />
                                            </telerik:gridtemplatecolumn>
                                            <telerik:gridboundcolumn datafield="tiempo_estimado"
                                                filtercontrolalttext="Filter tiempo_estimado column" headertext="tiempo_estimado" uniquename="tiempo_estimado" visible="true" display="false">
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="observaciones_ruta"
                                                filtercontrolalttext="Filter observaciones_ruta column" headertext="observaciones_ruta" uniquename="observaciones_ruta" visible="true" display="false">
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="observaciones_trayecto"
                                                filtercontrolalttext="Filter observaciones_trayecto column" headertext="observaciones_trayecto" uniquename="observaciones_trayecto" visible="true" display="false">
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="cantidad_personas"
                                                filtercontrolalttext="Filter cantidad_personas column" headerstyle-width="75px"
                                                headertext="# personas solicitud" sortexpression="cantidad_personas"
                                                uniquename="colm_cantidad_personas">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="participantes_verificados"
                                                filtercontrolalttext="Filter participantes_verificados column" headerstyle-width="120px"
                                                headertext="# personas verificación fondos" sortexpression="participantes_verificados"
                                                uniquename="colm_participantes_verificados">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="requiere_estipendio_text"
                                                filtercontrolalttext="Filter requiere_estipendio_text column" headerstyle-width="70px"
                                                headertext="Estipendio" sortexpression="requiere_estipendio_text"
                                                uniquename="colm_requiere_estipendio_text">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                dataformatstring="{0:n}"
                                                filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                headertext="Valor ida y regreso" sortexpression="valor_trayecto"
                                                uniquename="colm_valor_trayecto">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="sub_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor total ida y regreso: {0:n}</b>" footertext="Valor total ida y regreso: "
                                                filtercontrolalttext="Filter sub_total_ruta column" headerstyle-width="120px"
                                                headertext="Valor total ida y regreso" sortexpression="sub_total_ruta"
                                                uniquename="colm_valor_trayecto">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="valor_total_estipendio" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor estipendio: {0:n}</b>" footertext="Valor estipendio: "
                                                filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                headertext="Valor estipendio" sortexpression="valor_total_estipendio"
                                                uniquename="colm_valor_total_estipendio">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="valor_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                aggregate="Sum" dataformatstring="{0:n}" footeraggregateformatstring="<b>Valor total: {0:n}</b>" footertext="Valor total: "
                                                filtercontrolalttext="Filter valor_total_ruta column" headerstyle-width="90px"
                                                headertext="Valor total" sortexpression="valor_total_ruta"
                                                uniquename="colm_valor_total_ruta">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                        </columns>
                                    </mastertableview>
                                </telerik:radgrid>
                            </div>
                        </div>
                    </div>
                    <div runat="server">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center">
                                    <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Detalle participanteas</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:radgrid id="grd_participantes_resumen" runat="server" allowautomaticdeletes="True" cellspacing="0" allowpaging="True" pagesize="100"
                                    autogeneratecolumns="False" width="100%"
                                    showfooter="true"
                                    showcolumnfooters="true"
                                    showgroupfooters="true"
                                    showgrouppanel="false">
                                    <clientsettings enablerowhoverstyle="true">
                                        <selecting allowrowselect="True"></selecting>
                                        <resizing allowcolumnresize="true" allowresizetofit="true" />
                                    </clientsettings>
                                    <mastertableview autogeneratecolumns="False" datakeynames="id_participante" allowautomaticupdates="True">
                                        <columns>
                                            <telerik:gridboundcolumn datafield="id_participante"
                                                filtercontrolalttext="Filter id_participante column"
                                                sortexpression="id_participante" uniquename="id_participante"
                                                visible="False" headertext="id_participante"
                                                readonly="True">
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="tipo_ocumento"
                                                filtercontrolalttext="Filter tipo_ocumento column" headerstyle-width="135px"
                                                headertext="Tipo de documento" sortexpression="tipo_ocumento"
                                                uniquename="colm_tipo_ocumento">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="numero_documento"
                                                filtercontrolalttext="Filter numero_documento column" headerstyle-width="135px"
                                                headertext="Número de documento" sortexpression="numero_documento"
                                                uniquename="colm_numero_documento">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="nombres"
                                                filtercontrolalttext="Filter nombres column" headerstyle-width="120px"
                                                headertext="Nombres" sortexpression="nombres"
                                                uniquename="colm_nombres">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="primer_apellido"
                                                filtercontrolalttext="Filter primer_apellido column" headerstyle-width="135px"
                                                headertext="Primer apellido" sortexpression="primer_apellido"
                                                uniquename="colm_primer_apellido">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="segundo_apellido"
                                                filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="135px"
                                                headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                uniquename="colm_segundo_apellido">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="telefono"
                                                filtercontrolalttext="Filter telefono column" headerstyle-width="135px"
                                                headertext="Teléfono" sortexpression="telefono"
                                                uniquename="colm_telefono_">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                             <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="110px"
                                                headertext="Valor transporte" sortexpression="valor"
                                                uniquename="colm_valor_trayecto">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                             <telerik:gridboundcolumn datafield="valor_estipendio" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                filtercontrolalttext="Filter valor_estipendio column" headerstyle-width="110px"
                                                headertext="Valor alimentación" sortexpression="valor"
                                                uniquename="colm_valor_estipendio">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                            <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                filtercontrolalttext="Filter valor column" headerstyle-width="110px"
                                                headertext="Valor" sortexpression="valor"
                                                uniquename="colm_valor">
                                                <headerstyle cssclass="wrapWord" />
                                            </telerik:gridboundcolumn>
                                        </columns>
                                        <groupbyexpressions>
                                            <telerik:gridgroupbyexpression>
                                                <groupbyfields>
                                                    <telerik:gridgroupbyfield aggregate="None" fieldname="num_ruta"></telerik:gridgroupbyfield>
                                                </groupbyfields>
                                                <selectfields>
                                                    <telerik:gridgroupbyfield aggregate="None" fieldname="num_ruta" headertext="No ruta "></telerik:gridgroupbyfield>
                                                </selectfields>
                                            </telerik:gridgroupbyexpression>
                                        </groupbyexpressions>
                                    </mastertableview>
                                </telerik:radgrid>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12 text-center">
                            <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor="Red" Visible="true"></asp:Label>
                            <br />
                            <br />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                                 <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Observaciones generales de la dispersión de los fondos</asp:Label>
                                <br />
                            <telerik:radtextbox cssclass="upper" id="txt_observaciones_fondos" runat="server" rows="3" textmode="MultiLine" width="97%" maxlength="1000">
                                </telerik:radtextbox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                                 <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Soporte dispersión de los fondos</asp:Label>
                                <br />
                                <telerik:RadAsyncUpload 
                                    RenderMode="Lightweight" 
                                    runat="server" 
                                    ID="soporte_legalizacion"
                                    OnClientFileUploaded="onClientFileUploaded"
                                    MultipleFileSelection="Automatic" 
                                    Skin="Office2007"
                                    TemporaryFolder="~/Temp" 
                                    PostbackTriggers="btn_guardar"
                                    MaxFileInputsCount="8"
                                    data-clientFilter="application/pdf"
                                    AllowedFileExtensions="pdf"
                                    HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                </telerik:RadAsyncUpload>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:radbutton id="btn_salir" runat="server" text="Salir" width="100px" causesvalidation="false" cssclass="btn btn-sm pull-right">
                        </telerik:radbutton>
                        <telerik:radbutton id="btn_guardar" runat="server" text="Finalizar registro de participantes" enabled="false" autopostback="true" cssclass="btn btn-sm pull-right margin-r-5"
                            validationgroup="8">
                        </telerik:radbutton>
                           <%--   <telerik:radbutton id="btn_guardar_enviar" runat="server" text="Guardar y envíar por aprobación" enabled="false" autopostback="true" cssclass="btn btn-sm pull-right margin-r-5"
                            validationgroup="1">
                        </telerik:radbutton>--%>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
                </div>
                <telerik:radwindowmanager runat="Server" id="RadWindowManager1" enableviewstate="true" width="700" height="400">
                    <windows>
                        <telerik:radwindow rendermode="Lightweight" runat="server" width="700" behaviors="Close, Pin, Move" height="400" id="RadWindow2" modal="true" enableshadow="false" visibleonpageload="false" cssclass="windowcss">
                            <contenttemplate>
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Numero de documento</asp:Label>
                                        <br />
                                        <telerik:radnumerictextbox id="txt_numero_documento" runat="server" decimal width="80%" minvalue="0">
                                            <numberformat allowrounding="true" decimaldigits="0" />
                                        </telerik:radnumerictextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txt_numero_documento" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Tipo de documento</asp:Label>
                                        <br />
                                         <telerik:radcombobox id="cmb_tipo_documento" autopostback="false" emptymessage="Seleccione..." runat="server" width="90%">
                                        </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                            ControlToValidate="cmb_tipo_documento" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Nombres</asp:Label>
                                        <br />
                                        <telerik:radtextbox id="txt_nombre" runat="server" width="80%" maxlength="1000">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                            ControlToValidate="txt_nombre" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Primer apellido</asp:Label>
                                        <br />
                                        <telerik:radtextbox id="txt_primer_apellido" runat="server" width="80%" maxlength="1000">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                            ControlToValidate="txt_primer_apellido" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Segundo apellido</asp:Label>
                                        <br />
                                        <telerik:radtextbox id="txt_segundo_apellido" runat="server" width="80%" maxlength="1000">
                                        </telerik:radtextbox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="txt_segundo_apellido" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Valor</asp:Label>
                                        <br />
                                        <telerik:radnumerictextbox id="txt_valor" runat="server" enabled="false" width="80%" maxlength="1000">
                                        </telerik:radnumerictextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                            ControlToValidate="txt_valor" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Número de teléfono</asp:Label>
                                        <br />
                                        <telerik:radtextbox id="txt_numero_telefono" runat="server" width="80%" maxlength="1000">
                                        </telerik:radtextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                            ControlToValidate="txt_numero_telefono" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div style="display:none">
                                        <telerik:radnumerictextbox id="txt_estipendio" runat="server" enabled="false" width="80%" maxlength="1000">
                                        </telerik:radnumerictextbox>
                                        <telerik:radnumerictextbox id="txt_trayecto" runat="server" enabled="false" width="80%" maxlength="1000">
                                        </telerik:radnumerictextbox>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <telerik:radbutton id="btn_guardar_participante" runat="server" autopostback="true" cssclass="btn btn-sm" text="Agregar persona" validationgroup="3" width="100px" style="margin-top: 10px;">
                                        </telerik:radbutton>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <telerik:radgrid id="grd_conceptos" runat="server" allowautomaticdeletes="True" cellspacing="0" allowpaging="True" pagesize="100" autogeneratecolumns="False" width="100%"
                                            showfooter="false"
                                            showcolumnfooters="false"
                                            showgroupfooters="false"
                                            showgrouppanel="false">
                                            <clientsettings enablerowhoverstyle="true">
                                                <selecting allowrowselect="True"></selecting>
                                                <resizing allowcolumnresize="true" allowresizetofit="true" />
                                            </clientsettings>
                                            <mastertableview autogeneratecolumns="False" datakeynames="id_participante" allowautomaticupdates="True">
                                                <columns>
                                                    <telerik:gridboundcolumn datafield="id_participante"
                                                        filtercontrolalttext="Filter id_participante column"
                                                        sortexpression="id_participante" uniquename="id_participante"
                                                        visible="False" headertext="id_participante"
                                                        readonly="True">
                                                    </telerik:gridboundcolumn>

                                                    <telerik:gridboundcolumn datafield="numero_documento"
                                                        filtercontrolalttext="Filter numero_documento column" headerstyle-width="30%"
                                                        headertext="Número documento" sortexpression="numero_documento"
                                                        uniquename="numero_documento">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="nombres"
                                                        filtercontrolalttext="Filter nombres column" headerstyle-width="30%"
                                                        headertext="Nombres" sortexpression="nombres"
                                                        uniquename="nombres">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="primer_apellido"
                                                        filtercontrolalttext="Filter primer_apellido column" headerstyle-width="30%"
                                                        headertext="Primer apellido" sortexpression="primer_apellido"
                                                        uniquename="primer_apellido">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="segundo_apellido"
                                                        filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="30%"
                                                        headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                        uniquename="segundo_apellido">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="telefono"
                                                        filtercontrolalttext="Filter telefono column" headerstyle-width="30%"
                                                        headertext="Teléfono" sortexpression="telefono"
                                                        uniquename="telefono">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="valor_trayecto" dataformatstring="{0:n0}" itemstyle-horizontalalign="Right"
                                                        filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="30%"
                                                        headertext="Valor transporte" sortexpression="valor_trayecto"
                                                        uniquename="valor_trayecto">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="valor_estipendio" dataformatstring="{0:n0}" itemstyle-horizontalalign="Right"
                                                        filtercontrolalttext="Filter valor_estipendio column" headerstyle-width="30%"
                                                        headertext="Valor alimentación" sortexpression="valor_estipendio"
                                                        uniquename="valor_estipendio">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                    <telerik:gridboundcolumn datafield="valor" dataformatstring="{0:n0}" itemstyle-horizontalalign="Right"
                                                        filtercontrolalttext="Filter valor column" headerstyle-width="30%"
                                                        headertext="Valor total" sortexpression="valor"
                                                        uniquename="valor">
                                                        <headerstyle cssclass="wrapWord" />
                                                    </telerik:gridboundcolumn>
                                                </columns>
                                            </mastertableview>
                                        </telerik:radgrid>
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
                                        <telerik:radbutton id="btn_guardar_finalizar" runat="server" autopostback="true" text="Guardar" width="100px" cssclass="btn btn-sm btn-primary btn-ok import">
                                        </telerik:radbutton>
                                    </div>
                                </div>
                            </contenttemplate>
                        </telerik:radwindow>
                    </windows>
                </telerik:radwindowmanager>
                   <telerik:radwindowmanager runat="Server" id="RadWindowManager2" enableviewstate="true" width="700" height="400">
                    <windows>

                        <telerik:radwindow rendermode="Lightweight" runat="server" width="1000" behaviors="Close, Pin, Move" height="600" id="RadWindowRutas" modal="true" enableshadow="false" visibleonpageload="false" cssclass="windowcss">
                            <contenttemplate>
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Municipio / ciudad de salida:</asp:Label>
                                        <br />
                                        <telerik:radcombobox id="cmb_municipio_salida" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                        </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                            ControlToValidate="cmb_municipio_salida" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Municipio / ciudad de llegada:</asp:Label>
                                        <br />
                                        <telerik:radcombobox id="cmb_municipio_llegada" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                        </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                            ControlToValidate="cmb_municipio_llegada" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Zona rural:</asp:Label>
                                        <br />
                                        <telerik:radcombobox id="cmb_zona_rural" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%" highlighttemplateditems="true">
                                            <headertemplate>
                                                <ul>
                                                    <li class="col1">Zona rural</li>
                                                    <li class="col2">Valor ida y regreso </li>
                                                    <li class="col3">Tiempo estimado</li>
                                                    <li class="col3">Observaciones adicionales</li>
                                                </ul>
                                            </headertemplate>
                                            <itemtemplate>
                                                <ul>
                                                    <li class="col1">
                                                        <%# DataBinder.Eval(Container.DataItem, "zona_rural")%></li>
                                                    <li class="col2">
                                                        <%# String.Format("{0:N}", DataBinder.Eval(Container.DataItem, "valor_ruta"))%></li>
                                                    <li class="col3">
                                                        <%# DataBinder.Eval(Container.DataItem, "tiempo_estimado")%></li>
                                                    <li class="col3">
                                                        <%# DataBinder.Eval(Container.DataItem, "observaciones")%></li>
                                                </ul>
                                            </itemtemplate>
                                        </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                            ControlToValidate="cmb_zona_rural" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Cantidad de personas:</asp:Label>
                                        <br />
                                        <telerik:radnumerictextbox id="txt_cantidad_personas" runat="server" autopostback="true" visible="true" width="90%" value="0" minvalue="0" numberformat-decimaldigits="0">
                                        </telerik:radnumerictextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                            ControlToValidate="txt_cantidad_personas" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Valor por trayecto (ida y regreso):</asp:Label>
                                        <br />
                                        <telerik:radnumerictextbox id="txt_valor_trayecto" runat="server" enabled="false" visible="true" width="90%">
                                        </telerik:radnumerictextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server"
                                            ControlToValidate="txt_valor_trayecto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label29" CssClass="control-label text-bold">Valor total:</asp:Label>
                                        <br />
                                        <telerik:radnumerictextbox id="txt_valor_total" runat="server" enabled="false" visible="true" width="90%">
                                        </telerik:radnumerictextbox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                            ControlToValidate="txt_valor_total" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label30" CssClass="control-label text-bold">Requiere estipendio?:</asp:Label>
                                        <br />
                                        <asp:RadioButtonList ID="rbn_estipendio" runat="server"
                                            RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                            <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                            <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="lugarEstipendio">
                                        <asp:Label runat="server" ID="Label32" CssClass="control-label text-bold">Lugar estipendio?:</asp:Label>
                                        <br />
                                        <telerik:radcombobox id="cmb_lugar_estipendio" autopostback="true" emptymessage="Seleccione..." runat="server" width="90%">
                                        </telerik:radcombobox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server"
                                            ControlToValidate="cmb_lugar_estipendio" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                            *</asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <hr />
                                    </div>
                                </div>
                                <div class="form-group row" runat="server" visible="false" id="infoEstipendio">
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label33" CssClass="control-label text-bold">Estipendio de esayuno?:</asp:Label>
                                        <br />
                                        <asp:RadioButtonList ID="rbn_estipendio_desayuno" runat="server"
                                            RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                            <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                            <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div runat="server" visible="false" id="info_desayuno">
                                            <br />
                                            <asp:Label runat="server" ID="Label34" CssClass="control-label text-bold">Cantidad de desayunos por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_cantidad_desayuno" numberformat-decimaldigits="0" value="0" runat="server" autopostback="true" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                            <br />
                                            <asp:Label runat="server" ID="Label35" CssClass="control-label text-bold">Total desayuno por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_total_desayuno" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                        </div>

                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label36" CssClass="control-label text-bold">Estipendio de almuerzo?:</asp:Label>
                                        <br />
                                        <asp:RadioButtonList ID="rbn_estipendio_almuerzo" runat="server"
                                            RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                            <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                            <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div runat="server" visible="false" id="info_almuerzo">
                                            <br />
                                            <asp:Label runat="server" ID="Label37" CssClass="control-label text-bold">Cantidad de aluerzos por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_cantidad_almuerzo" numberformat-decimaldigits="0" value="0" runat="server" autopostback="true" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                            <br />
                                            <asp:Label runat="server" ID="Label38" CssClass="control-label text-bold">Total almuerzos por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_total_almuerzo" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                        </div>

                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                        <asp:Label runat="server" ID="Label39" CssClass="control-label text-bold">Estipendio de cena?:</asp:Label>
                                        <br />
                                        <asp:RadioButtonList ID="rbn_estipendio_cena" runat="server"
                                            RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                            <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                            <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div runat="server" visible="false" id="info_cena">
                                            <br />
                                            <asp:Label runat="server" ID="Label40" CssClass="control-label text-bold">Cantidad de cenas por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_cantidad_cena" numberformat-decimaldigits="0" value="0" runat="server" autopostback="true" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                            <br />
                                            <asp:Label runat="server" ID="Label41" CssClass="control-label text-bold">Total cena por persona:</asp:Label>
                                            <br />
                                            <telerik:radnumerictextbox id="txt_total_cena" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                            </telerik:radnumerictextbox>
                                        </div>

                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <asp:Label runat="server" ID="Label42" CssClass="control-label text-bold">Observaciones adicionales:</asp:Label>
                                        <br />
                                        <telerik:radtextbox cssclass="upper" id="txt_observaciones_ruta" runat="server" rows="3" textmode="MultiLine" width="97%" maxlength="1000">
                                        </telerik:radtextbox>
                                    </div>

                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-8"></div>
                                    <div class="col-sm-4 text-right">
                                        <telerik:radbutton id="btn_agregar_ruta" runat="server" cssclass="btn btn-sm" text="Agregar ruta" validationgroup="2" width="100px">
                                        </telerik:radbutton>
                                    </div>
                                </div>
                            </contenttemplate>
                        </telerik:radwindow>

                    </windows>
                </telerik:radwindowmanager>
                <asp:HiddenField runat="server" ID="id_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="totalParticipantes" Value="0" />
                <asp:HiddenField runat="server" ID="id_ruta_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />
                <asp:HiddenField runat="server" ID="tiempo_estimado" />
                <asp:HiddenField runat="server" ID="observaciones" />
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
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
         
        })
        window.onClientFileUploaded = function (radAsyncUpload, args) {
         
        }
    </script>
</asp:Content>
