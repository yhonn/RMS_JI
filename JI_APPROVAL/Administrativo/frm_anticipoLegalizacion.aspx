<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_anticipoLegalizacion.aspx.vb" Inherits="RMS_APPROVAL.frm_anticipoLegalizacion" %>
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
        html:first-child .RadWindow ul{
            float:left !important;
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Anticipo - Legalización</asp:Label></h3>
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
                    <div runat="server" id="info_rutas">
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
                                            <telerik:GridBoundColumn DataField="num_ruta"
                                                FilterControlAltText="Filter num_ruta column" HeaderStyle-Width="30px"
                                                HeaderText="No" SortExpression="num_ruta"
                                                UniqueName="colm_num_ruta">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ciudad_salida"
                                                FilterControlAltText="Filter ciudad_origen column" HeaderStyle-Width="135px"
                                                HeaderText="Municipio / ciudad de salida" SortExpression="ciudad_salida"
                                                UniqueName="colm_ciudad_salidan">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="zona_rural"
                                                FilterControlAltText="Filter zona_rural column" HeaderStyle-Width="120px"
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
                                                HeaderText="# personas solicitud" SortExpression="cantidad_personas"
                                                UniqueName="colm_cantidad_personas">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="participantes_verificados"
                                                FilterControlAltText="Filter participantes_verificados column" HeaderStyle-Width="120px"
                                                HeaderText="# personas verificación fondos" SortExpression="participantes_verificados"
                                                UniqueName="colm_participantes_verificados">
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
                                                FilterControlAltText="Filter valor_trayecto column"  HeaderStyle-Width="110px"
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
                                                FilterControlAltText="Filter valor_trayecto column" HeaderStyle-Width="110px"
                                                HeaderText="Valor estipendio" SortExpression="valor_total_estipendio"
                                                UniqueName="colm_valor_total_estipendio">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total_ruta" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                FilterControlAltText="Filter valor_total_ruta column" HeaderStyle-Width="90px"
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
                    <div runat="server" id="info_participantes">
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Detalle participanteas</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:RadGrid ID="grd_participantes_resumen" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" 
                                    AutoGenerateColumns="False" Width="100%"
                                    ShowFooter="true" 
                                    ShowColumnFooters="true"
                                    ShowGroupFooters="true"
                                    ShowGroupPanel="false">
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_participante,retiro_fondos" AllowAutomaticUpdates="True" >
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_participante"
                                                FilterControlAltText="Filter id_participante column"
                                                SortExpression="id_participante" UniqueName="id_participante"
                                                Visible="False" HeaderText="id_participante"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="retiro_fondos"
                                                FilterControlAltText="Filter retiro_fondos column"
                                                SortExpression="retiro_fondos" UniqueName="retiro_fondos_val"
                                                Visible="False" HeaderText="retiro_fondos"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="tipo_ocumento" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter tipo_ocumento column" HeaderStyle-Width="119px"
                                                HeaderText="Tipo de documento" SortExpression="tipo_ocumento"
                                                UniqueName="colm_tipo_ocumento">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="numero_documento" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter numero_documento column" HeaderStyle-Width="115px"
                                                HeaderText="Número de documento" SortExpression="numero_documento"
                                                UniqueName="colm_numero_documento">
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="nombres" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter nombres column" HeaderStyle-Width="120px"
                                                HeaderText="Nombres" SortExpression="nombres"
                                                UniqueName="colm_nombres">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="primer_apellido" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter primer_apellido column"  HeaderStyle-Width="115px"
                                                HeaderText="Primer apellido" SortExpression="primer_apellido"
                                                UniqueName="colm_primer_apellido">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="segundo_apellido" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter segundo_apellido column"  HeaderStyle-Width="115px"
                                                HeaderText="Segundo apellido" SortExpression="segundo_apellido"
                                                UniqueName="colm_segundo_apellido">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="telefono" ItemStyle-Width="119px"
                                                FilterControlAltText="Filter telefono column"  HeaderStyle-Width="115px"
                                                HeaderText="Teléfono" SortExpression="telefono"
                                                UniqueName="colm_telefono_">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="119px"
                                                DataFormatString="{0:n}"  Aggregate="Sum" FooterAggregateFormatString="{0:C}"  FooterStyle-HorizontalAlign="Right"
                                                FilterControlAltText="Filter valor column"  HeaderStyle-Width="110px" 
                                                HeaderText="Valor" SortExpression="valor"
                                                UniqueName="colm_valor">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn FilterControlAltText="Filter retiro_fondos_ column"  HeaderStyle-Width="115px" ItemStyle-Width="119px"
                                                HeaderText="Retiro de fondos" UniqueName="cmb_retiro_fondos">
                                                <ItemTemplate>
                                                    <asp:RadioButtonList ID="rbn_retiro_fondos" runat="server"
                                                        RepeatColumns="2" AutoPostBack="false">
                                                        <asp:ListItem Value="1">SÍ &nbsp;</asp:ListItem>
                                                        <asp:ListItem Value="2">NO &nbsp;</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                          <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField Aggregate="None" FieldName="num_ruta"></telerik:GridGroupByField>
                                                </GroupByFields>
                                                <SelectFields>
                                                    <telerik:GridGroupByField Aggregate="None" FieldName="num_ruta" HeaderText="No ruta "></telerik:GridGroupByField>
                                                </SelectFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>

                        <div class="form-group row">
                             <div class="col-sm-6 col-md-6 col-lg-4" >
                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Soporte</asp:Label>
                                    <br />
                                    <telerik:RadAsyncUpload 
                                        RenderMode="Lightweight" 
                                        runat="server" 
                                        ID="soporte_legalizacion"
                                        OnClientFileUploaded="onClientFileUploaded"
                                        MultipleFileSelection="Automatic" 
                                        Skin="Office2007"
                                        TemporaryFolder="~/Temp" 
                                        PostbackTriggers="btn_guardar,btn_guardar_enviar"
                                        MaxFileInputsCount="8"
                                        data-clientFilter="application/pdf"
                                        AllowedFileExtensions="pdf"
                                        HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx">
                                    </telerik:RadAsyncUpload>
                                     
                                    </div>
                            </div>
                        </div>
                    <div runat="server" id="info_leg_compras">

                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Detalle de servicios requeridos</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>

                        <div class="col-sm-12">
                                <div class="form-group row">
                                     <telerik:radgrid id="grd_compras" runat="server" allowautomaticdeletes="True" cellspacing="0" allowpaging="True" pagesize="100" autogeneratecolumns="False" width="100%"
                                        showfooter="true"
                                        showcolumnfooters="true"
                                        showgroupfooters="true"
                                        showgrouppanel="false">
                                        <clientsettings enablerowhoverstyle="true">
                                            <selecting allowrowselect="True"></selecting>
                                            <resizing allowcolumnresize="true" allowresizetofit="true" />
                                        </clientsettings>
                                        <mastertableview autogeneratecolumns="False" datakeynames="id_anticipo_compra" allowautomaticupdates="True">
                                            <columns>
                                                <telerik:gridboundcolumn datafield="id_anticipo_compra"
                                                    filtercontrolalttext="Filter id_anticipo_compra column"
                                                    sortexpression="id_anticipo_compra" uniquename="id_anticipo_compra"
                                                    visible="False" headertext="id_anticipo_compra"
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
                                                <telerik:gridboundcolumn datafield="precio_unitario" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                    dataformatstring="{0:n0}"
                                                    filtercontrolalttext="Filter precio_unitario column" headerstyle-width="13%"
                                                    headertext="Valor unitario" sortexpression="precio_unitario"
                                                    uniquename="colm_valor_unitario">
                                                    <headerstyle cssclass="wrapWord" />
                                                </telerik:gridboundcolumn>
                                                <telerik:gridboundcolumn datafield="valor_total" itemstyle-horizontalalign="Right" footerstyle-horizontalalign="Right" headerstyle-horizontalalign="Right"
                                                    aggregate="Sum" dataformatstring="{0:n0}" footeraggregateformatstring="<b>Valor total: {0:n0}</b>" footertext="Valor total: "
                                                    filtercontrolalttext="Filter valor_total column" headerstyle-width="19%"
                                                    headertext="Valor total" sortexpression="valor_total"
                                                    uniquename="colm_valor">
                                                    <headerstyle cssclass="wrapWord" />
                                                </telerik:gridboundcolumn>
                                            </columns>
                                        </mastertableview>
                                    </telerik:radgrid>
                                </div>
                            </div>


                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Legalización</asp:Label></h4>
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
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_factura" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_anticipo_factura"
                                                        FilterControlAltText="Filter id_anticipo_factura column"
                                                        SortExpression="id_anticipo_factura" UniqueName="id_anticipo_factura"
                                                        Visible="False" HeaderText="id_anticipo_factura"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                   <%--  <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                        <HeaderStyle Width="4%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                OnClick="delete_detalle">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>
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
                                                    <telerik:GridBoundColumn DataField="total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                        DataFormatString="{0:n0}" 
                                                        Aggregate="Sum" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
                                                        FilterControlAltText="Filter total column" HeaderStyle-Width="23%"
                                                        HeaderText="Valor total" SortExpression="total"
                                                        UniqueName="total">
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
                    </div>
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
                                                        <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Proveedor</asp:Label>
                                                        <br />
                                                        <telerik:RadTextBox ID="txt_proveedor" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                    ControlToValidate="txt_proveedor" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   
                                                    <div class="col-sm-6 col-md-6 col-lg-3">
                                                        <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Soporte</asp:Label>
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
                                             <div class="hidden">
                                                    <telerik:RadNumericTextBox ID="txt_cantidad_productos" runat="server" Visible="true"  Width="90%" MinValue="1" NumberFormat-DecimalDigits="0">
                                                    </telerik:RadNumericTextBox>
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
                                                                    <telerik:GridBoundColumn DataField="valor_unitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                        DataFormatString="{0:n0}" 
                                                                        FilterControlAltText="Filter valor_unitario column" HeaderStyle-Width="13%"
                                                                        HeaderText="Valor unitario" SortExpression="valor_unitario"
                                                                        UniqueName="colm_valor_unitario">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="valor" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                        Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
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
                    <div class="form-group row">
                        <div class="col-sm-12 text-center">                                              
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                            <br /><br />
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
                <asp:HiddenField runat="server" ID="id_concpeto" Value="0" />
                <asp:HiddenField runat="server" ID="tipo_delete" Value="0" />
                <asp:HiddenField runat="server" ID="id_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="id_ruta_anticipo" Value="0" />
                <asp:HiddenField runat="server" ID="identity" Value="0" />
                <asp:HiddenField runat="server" ID="tipo" Value="0" />
                <asp:HiddenField runat="server" ID="tiempo_estimado"/>
                <asp:HiddenField runat="server" ID="observaciones"/>
                <asp:HiddenField runat="server" ID="idanticipo" Value="0" />
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
