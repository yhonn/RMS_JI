<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site.Master" codebehind="frm_anticipoSgmtLegalizacion.aspx.vb" inherits="RMS_APPROVAL.frm_anticipoSgmtLegalizacion" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Seguimiento anticipo</asp:Label></h3>
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">


                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">

                            <div class="panel panel-default">
                                <%--First panel--%>
                                <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseOne"
                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_informacion">Información de la aprobación
                                        </a>
                                    </h4>
                                </div>

                                <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <div class="col-sm-12 text-right">
                                            <div class="form-group row">
                                                <div class="col-sm-12 text-left">
                                                    <telerik:radgrid id="grd_cate" runat="server" allowautomaticdeletes="True"
                                                        allowautomaticupdates="True" autogeneratecolumns="False" cellspacing="0"
                                                        gridlines="None" width="100%" showheader="True">
                                                        <headercontextmenu cssclass="GridContextMenu GridContextMenu_Sunset">
                                                            <webservicesettings>
                                                                <odatasettings initialcontainername=""></odatasettings>
                                                            </webservicesettings>
                                                        </headercontextmenu>
                                                        <mastertableview>
                                                            <rowindicatorcolumn filtercontrolalttext="Filter RowIndicator column" visible="True">
                                                            </rowindicatorcolumn>
                                                            <expandcollapsecolumn filtercontrolalttext="Filter ExpandColumn column" visible="True">
                                                            </expandcollapsecolumn>
                                                            <columns>
                                                                <telerik:gridboundcolumn datafield="orden" datatype="System.Int32" filtercontrolalttext="Filter orden column"
                                                                    headertext="Id" sortexpression="orden" uniquename="orden">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridboundcolumn datafield="nombre_rol" filtercontrolalttext="Filter nombre_rol column"
                                                                    headertext="Rol" sortexpression="nombre_rol" uniquename="nombre_rol">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridboundcolumn datafield="nombre_empleado" filtercontrolalttext="Filter nombre_empleado column"
                                                                    headertext="Usuario" sortexpression="nombre_empleado" uniquename="nombre_empleado">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridboundcolumn datafield="descripcion_estado" filtercontrolalttext="Filter descripcion_estado column"
                                                                    headertext="Estado" sortexpression="descripcion_estado" uniquename="descripcion_estado">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridboundcolumn datafield="fecha_aprobacion"
                                                                    filtercontrolalttext="Filter fecha_aprobacion column"
                                                                    headertext="Fecha de aprobación" sortexpression="fecha_aprobacion"
                                                                    uniquename="fecha_aprobacion">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridboundcolumn datafield="Alerta" filtercontrolalttext="Filter Alerta column"
                                                                    headertext="Alert" sortexpression="Alerta" uniquename="Alerta" visible="true" display="false">
                                                                </telerik:gridboundcolumn>
                                                                <telerik:gridtemplatecolumn uniquename="CompletoC" filtercontrolalttext="Filter Completo column">
                                                                    <itemtemplate>
                                                                        <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                                            ToolTip="Indicador Incompleto">
                                                                        </asp:HyperLink>
                                                                    </itemtemplate>
                                                                </telerik:gridtemplatecolumn>
                                                            </columns>
                                                            <editformsettings>
                                                                <editcolumn filtercontrolalttext="Filter EditCommandColumn column" uniquename="EditCommandColumn1">
                                                                </editcolumn>
                                                            </editformsettings>
                                                        </mastertableview>
                                                        <clientsettings allowdragtogroup="True" enablerowhoverstyle="True">
                                                            <selecting allowrowselect="True" />
                                                        </clientsettings>
                                                        <filtermenu enableimagesprites="False">
                                                            <webservicesettings>
                                                                <odatasettings initialcontainername=""></odatasettings>
                                                            </webservicesettings>
                                                        </filtermenu>
                                                    </telerik:radgrid>
                                                    <hr />
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <div class="col-sm-12 text-left ">
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_category" runat="server" CssClass="control-label text-bold" Text="Tipo de aprobación"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_categoria" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_approval" runat="server" CssClass="control-label text-bold" Text="Código de aprobación"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_codigo" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class=" row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Fecha de solicitud"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_solicitud" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_owner" runat="server" CssClass="control-label text-bold" Text="Fecha requiere anticipo"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_fecha_requiere_anticipo" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_NextApp" runat="server" CssClass="control-label text-bold" Text="Código PAR"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_codigo_par" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_tipo_anticipo" runat="server" CssClass="control-label text-bold" Text="Tipo de anticipo"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_tipo_anticipo" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4 text-left">
                                                            <asp:Label ID="lblt_condition" runat="server" CssClass="control-label text-bold" Text="Motivo"></asp:Label>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <asp:Label ID="lbl_motivo" runat="server"></asp:Label>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                        <!--div 0 lg-12-->
                                    </div>
                                </div>

                            </div>
                            <%--First panel--%>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="infoViaje" data-toggle="collapse" data-parent="#accordion" href="#collapseViaje" aria-expanded="false" aria-controls="collapseViaje">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseViaje"
                                            aria-expanded="false" aria-controls="collapseViaje" runat="server" id="a1">Detalle del anticipo
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseViaje" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoViaje">
                                    <div class="panel-body">
                                        <asp:HiddenField runat="server" ID="idAnticipo" Value="0" />
                                        <asp:HiddenField runat="server" ID="HiddenField1" Value="0" />

                                        <div class="form-group row" runat="server" id="info_rutas">
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
                                                    <mastertableview autogeneratecolumns="False" datakeynames="id_anticipo_ruta_tem" allowautomaticupdates="True">
                                                        <columns>
                                                            <telerik:gridboundcolumn datafield="id_anticipo_ruta_tem"
                                                                filtercontrolalttext="Filter id_anticipo_ruta_tem column"
                                                                sortexpression="id_anticipo_ruta_tem" uniquename="id_anticipo_ruta_tem"
                                                                visible="False" headertext="id_anticipo_ruta_tem"
                                                                readonly="True">
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="ciudad_salida"
                                                                filtercontrolalttext="Filter ciudad_salida column" headerstyle-width="135px"
                                                                headertext="Municipio / ciudad de salida" sortexpression="ciudad_salida"
                                                                uniquename="colm_ciudad_salidan">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="zona_rural"
                                                                filtercontrolalttext="Filter zona_rural column" headerstyle-width="140px"
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
                                                                headertext="# personas" sortexpression="cantidad_personas"
                                                                uniquename="colm_cantidad_personas">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="requiere_estipendio_text"
                                                                filtercontrolalttext="Filter requiere_estipendio_text column" headerstyle-width="70px"
                                                                headertext="Estipendio" sortexpression="requiere_estipendio_text"
                                                                uniquename="colm_requiere_estipendio_text">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="valor_trayecto" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                dataformatstring="{0:n0}"
                                                                filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="120px"
                                                                headertext="Valor ida y regreso" sortexpression="valor_trayecto"
                                                                uniquename="colm_valor_trayecto">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="sub_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor total ida y regreso: {0:n0}</b>" footertext="Valor total ida y regreso: "
                                                                filtercontrolalttext="Filter sub_total_ruta column" headerstyle-width="120px"
                                                                headertext="Valor total ida y regreso" sortexpression="sub_total_ruta"
                                                                uniquename="colm_valor_trayecto">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="valor_total_estipendio" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor estipendio: {0:n0}</b>" footertext="Valor estipendio: "
                                                                filtercontrolalttext="Filter valor_trayecto column" headerstyle-width="120px"
                                                                headertext="Valor estipendio" sortexpression="valor_total_estipendio"
                                                                uniquename="colm_valor_total_estipendio">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="valor_total_ruta" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor total: {0:n0}</b>" footertext="Valor total: "
                                                                filtercontrolalttext="Filter valor_total_ruta column" headerstyle-width="120px"
                                                                headertext="Valor total" sortexpression="valor_total_ruta"
                                                                uniquename="colm_valor_total_ruta">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                        </columns>
                                                    </mastertableview>
                                                </telerik:radgrid>
                                            </div>

                                            <div runat="server" id="info_participantes">
                                                <div class="form-group">
                                                    <div class="col-sm-12">
                                                        <h4 class="text-center">
                                                            <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Detalle participanteas</asp:Label></h4>
                                                        <hr class="box box-primary" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
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
                                                            <mastertableview autogeneratecolumns="False" datakeynames="id_participante,retiro_fondos" allowautomaticupdates="True">
                                                                <columns>
                                                                    <telerik:gridboundcolumn datafield="id_participante"
                                                                        filtercontrolalttext="Filter id_participante column"
                                                                        sortexpression="id_participante" uniquename="id_participante"
                                                                        visible="False" headertext="id_participante"
                                                                        readonly="True">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="retiro_fondos"
                                                                        filtercontrolalttext="Filter retiro_fondos column"
                                                                        sortexpression="retiro_fondos" uniquename="retiro_fondos_val"
                                                                        visible="False" headertext="retiro_fondos"
                                                                        readonly="True">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="tipo_ocumento" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter tipo_ocumento column" headerstyle-width="119px"
                                                                        headertext="Tipo de documento" sortexpression="tipo_ocumento"
                                                                        uniquename="colm_tipo_ocumento">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="numero_documento" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter numero_documento column" headerstyle-width="115px"
                                                                        headertext="Número de documento" sortexpression="numero_documento"
                                                                        uniquename="colm_numero_documento">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="nombres" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter nombres column" headerstyle-width="120px"
                                                                        headertext="Nombres" sortexpression="nombres"
                                                                        uniquename="colm_nombres">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="primer_apellido" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter primer_apellido column" headerstyle-width="115px"
                                                                        headertext="Primer apellido" sortexpression="primer_apellido"
                                                                        uniquename="colm_primer_apellido">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="segundo_apellido" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter segundo_apellido column" headerstyle-width="115px"
                                                                        headertext="Segundo apellido" sortexpression="segundo_apellido"
                                                                        uniquename="colm_segundo_apellido">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="telefono" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter telefono column" headerstyle-width="115px"
                                                                        headertext="Teléfono" sortexpression="telefono"
                                                                        uniquename="colm_telefono_">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" headerstyle-horizontalalign="Right" itemstyle-width="119px"
                                                                        dataformatstring="{0:n}" aggregate="Sum" footeraggregateformatstring="{0:C}" footerstyle-horizontalalign="Right"
                                                                        filtercontrolalttext="Filter valor column" headerstyle-width="110px"
                                                                        headertext="Valor" sortexpression="valor"
                                                                        uniquename="colm_valor">
                                                                    </telerik:gridboundcolumn>
                                                                    <telerik:gridboundcolumn datafield="retiro_fondos_text" itemstyle-width="119px"
                                                                        filtercontrolalttext="Filter retiro_fondos_text column" headerstyle-width="115px"
                                                                        headertext="Retiro de fondos" sortexpression="retiro_fondos_text"
                                                                        uniquename="colm_telefono_">
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
                                                <div class="form-group" id="doc_admon_content" runat="server" visible="false">
                                                    <div class="col-sm-12" style="margin-top: 15px;">
                                                        <div class="row">
                                                            <div class="col-sm-12 text-left">
                                                                <asp:Label ID="Label1" runat="server" CssClass="control-label text-bold" Text="Soporte legalización"></asp:Label><br />
                                                                 <asp:HyperLink id="doc_legalizacion" 
                                                                  NavigateUrl="#"
                                                                  Target="_blank"
                                                                  Text="Ver soporte"
                                                                  runat="server"/> 
                                                            </div>
                                                            <div class="col-sm-5">
                                                               
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                        <div class="form-group row" runat="server" id="info_compras">
                                            <div class="col-sm-12">
                                                <telerik:radgrid id="grd_compras" runat="server" allowautomaticdeletes="True" cellspacing="0" allowpaging="True" pagesize="100" autogeneratecolumns="False" width="100%"
                                                    showfooter="true"
                                                    showcolumnfooters="true"
                                                    showgroupfooters="true"
                                                    showgrouppanel="false">
                                                    <clientsettings enablerowhoverstyle="true">
                                                        <selecting allowrowselect="True"></selecting>
                                                        <resizing allowcolumnresize="true" allowresizetofit="true" />
                                                    </clientsettings>
                                                    <mastertableview autogeneratecolumns="False" datakeynames="id_anticipo_compra_tem" allowautomaticupdates="True">
                                                        <columns>
                                                            <telerik:gridboundcolumn datafield="id_anticipo_compra_tem"
                                                                filtercontrolalttext="Filter id_anticipo_compra_tem column"
                                                                sortexpression="id_anticipo_compra_tem" uniquename="id_anticipo_compra_tem"
                                                                visible="False" headertext="id_anticipo_compra_tem"
                                                                readonly="True">
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="cantidad"
                                                                filtercontrolalttext="Filter cantidad column" headerstyle-width="19%"
                                                                headertext="Cantidad" sortexpression="cantidad"
                                                                uniquename="colm_cantidad">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="descripcion"
                                                                filtercontrolalttext="Filter descripcion column" headerstyle-width="47%"
                                                                headertext="Descripción / Servicio requerido" sortexpression="descripcion"
                                                                uniquename="colm_descripcion">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="valor_unitario" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                dataformatstring="{0:n0}"
                                                                filtercontrolalttext="Filter valor_unitario column" headerstyle-width="13%"
                                                                headertext="Valor unitario" sortexpression="valor_unitario"
                                                                uniquename="colm_valor_unitario">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                            <telerik:gridboundcolumn datafield="valor" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                                aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor total: {0:n0}</b>" footertext="Valor total: "
                                                                filtercontrolalttext="Filter valor column" headerstyle-width="19%"
                                                                headertext="Valor total" sortexpression="valor"
                                                                uniquename="colm_valor">
                                                                <headerstyle cssclass="wrapWord" />
                                                            </telerik:gridboundcolumn>
                                                        </columns>
                                                    </mastertableview>
                                                </telerik:radgrid>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="panel panel-default">

                                <div class="panel-heading" role="tab" id="infoApprobacion" data-toggle="collapse" data-parent="#accordion" href="#collapseApp" aria-expanded="false" aria-controls="collapseApp">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" href="#collapseApp"
                                            aria-expanded="false" aria-controls="collapseApp" runat="server" id="a2">Aprobación
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseApp" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="infoApprobacion">
                                    <div class="panel-body">
                                        <div class="form-group row" runat="server" id="lyHistory" visible="false">
                                            <div class="col-sm-12 text-left">

                                                <%--TAble here--%>
                                                <table class="table table-responsive table-condensed box box-primary ">
                                                    <tr class="box box-default ">
                                                        <td class="text-left" colspan="2">
                                                            <%-- <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold"   Text="Comments"></asp:Label>--%>
                                                            <div class="box-header">
                                                                <i class="fa fa-history"></i>
                                                                <h3 class="box-title">History</h3>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="text-left">
                                                            <br />
                                                            <%-- <div class="direct-chat-messages">--%>

                                                            <asp:Repeater ID="rept_msgApproval" runat="server">
                                                                <itemtemplate>
                                                                    <div class="direct-chat-msg <%# Eval("align1")%> ">
                                                                        <div class="direct-chat-info clearfix">
                                                                            <i class="fa <%# Eval("fa_icon")%> fa-2x <%# Eval("align2")%> " aria-hidden="true" title="<%# Eval("icon_message")%>"></i>&nbsp;&nbsp;&nbsp; <span class="direct-chat-name  <%# Eval("align2")%> "><%# Eval("empleado")%></span>
                                                                            <span class="direct-chat-timestamp  <%# Eval("align3") %> "><%# getFecha(Eval("fecha_comentario"), "m", False) %> <i class="fa fa-clock-o"></i><%#  getHora(Eval("fecha_comentario")) %> </span>
                                                                        </div>
                                                                        <!-- /.direct-chat-info -->
                                                                        <img class="direct-chat-img <%# Eval("bColor")%> " src="<%# Eval("userImagen")%>" alt="<%# Eval("empleado")%>"><!-- /.direct-chat-img -->
                                                                        <div class="direct-chat-text">
                                                                            <%# Eval("comentario")%>
                                                                        </div>
                                                                        <!-- /.direct-chat-text -->
                                                                    </div>
                                                                    <!-- /.direct-chat-msg -->
                                                                </itemtemplate>
                                                            </asp:Repeater>
                                                            <%--  </div><!--/.direct-chat-messages-->--%>                                      
                                          
                                                        </td>
                                                    </tr>
                                                </table>

                                            </div>
                                        </div>
                                        <div runat="server" id="infoApp">
                                            <div class="form-group row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="lblt_writcomments" runat="server" CssClass="control-label text-bold" Text="Comentarios"></asp:Label>
                                                    <br />
                                                    <telerik:radtextbox id="txtcoments" runat="server" height="100px" textmode="MultiLine" width="100%">
                                                    </telerik:radtextbox>
                                                </div>

                                            </div>
                                            <div class="form-group row">
                                                <div class="col-sm-3 text-center  ">
                                                    <!--Buttoms -->
                                                    <asp:Button ID="btn_Approved" runat="server" Text="Aprobar" OnClick="btn_Approved_Click" OnClientClick="  this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" CssClass="btn-lg btn-primary" Width="65%" />
                                                    <asp:Button ID="btn_Completed" runat="server" Text="Aprobar" OnClick="btn_Completed_Click" OnClientClick=" this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" CssClass="btn-lg btn-info" />
                                                </div>
                                                <div class="col-sm-3 text-center  ">
                                                    <!--Buttoms -->
                                                    <asp:Button ID="btn_STandBy" runat="server" Text="Solicitar ajustes" OnClick="btn_STandBy_Click" OnClientClick=" this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" CssClass="btn-lg btn-warning" Width="65%" />
                                                </div>
                                                <div class="col-sm-3 text-center  ">
                                                    <!--Buttoms -->
                                                    <asp:Button ID="btn_NotApproved" runat="server" Text="No aprobar" OnClick="btn_NotApproved_Click" OnClientClick="this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" CssClass="btn-lg btn-danger" Width="65%" />
                                                </div>

                                                <%--                                            <div class="col-sm-3 text-center  ">
                                              <!--Buttoms -->
                                                <asp:Button ID="btn_Cancelled" runat="server" Text="Cancelar"  OnClick="btn_Cancelled_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-danger" Width="65%" />                                                  
                                           </div>        --%>
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <div class="col-sm-12 text-center">
                                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor="Red" Visible="true"></asp:Label>
                                                <br />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />
                <div class="box-footer">
                    <telerik:radbutton id="btn_salir" runat="server" text="Salir" width="100px" causesvalidation="false" cssclass="btn btn-sm pull-right">
                    </telerik:radbutton>
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
