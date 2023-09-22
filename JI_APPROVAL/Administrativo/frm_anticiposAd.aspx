<%@ page title="" language="vb" autoeventwireup="false" masterpagefile="~/Site.Master" codebehind="frm_anticiposAd.aspx.vb" inherits="RMS_APPROVAL.frm_anticiposAd" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Registrar anticipo</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="hidden">
                            <telerik:radnumerictextbox id="txt_cantidad_productos" runat="server" visible="true" width="90%" minvalue="1" numberformat-decimaldigits="0">
                            </telerik:radnumerictextbox>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="subRegionVisible">
                                <asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Región a la que pertenece:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_sub_Region" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server"
                                    ControlToValidate="cmb_sub_Region" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>

                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="lbl_tipo_actividad_" CssClass="control-label text-bold">Fecha que se requiere el anticipo</asp:Label>
                                <br />
                                <telerik:raddatepicker id="dt_fecha_anticipo" autopostback="true" width="90%" enabled="true" runat="server">
                                    <calendar userowheadersasselectors="False" usecolumnheadersasselectors="False" viewselectortext="x"></calendar>
                                    <datepopupbutton imageurl="" hoverimageurl=""></datepopupbutton>
                                </telerik:raddatepicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="dt_fecha_anticipo" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Medio de pago</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_medio_pago" autopostback="true" emptymessage="Seleccione..." runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="cmb_medio_pago" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="otro_medio_pago">
                                <asp:Label runat="server" ID="Label8" CssClass="control-label text-bold">Ingrese el medio de pago:</asp:Label>
                                <br />
                                <telerik:radtextbox id="txt_otro_medio_pago" runat="server" width="90%" maxlength="1000">
                                </telerik:radtextbox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txt_otro_medio_pago" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Observaciones medio de pago</asp:Label>
                                <br />
                                <telerik:radtextbox id="txt_detalle_medio_pago" runat="server" width="90%" maxlength="1000">
                                </telerik:radtextbox>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="costo_giro">
                                <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Costo total comisión giro</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox id="txt_costo_giro" runat="server" width="90%" maxlength="1000">
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Tipo de anticipo</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_tipo_anticipo" autopostback="true" emptymessage="Seleccione..." runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="cmb_tipo_anticipo" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="1">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label7" CssClass="control-label text-bold">Código del PAR:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_par" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
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
                                <telerik:radtextbox cssclass="upper" id="txt_motivo_anticipo" runat="server" rows="3" textmode="MultiLine" width="97%" maxlength="1000">
                                </telerik:radtextbox>
                            </div>
                        </div>
                    </div>
                    <div runat="server" visible="false" id="info_compras">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label26" CssClass="control-label text-bold">Registrar items / servicios requeridos</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Descripción / Servicio requerido</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_descripcion_concepto" runat="server" Rows="3" TextMode="MultiLine" Width="90%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                            ControlToValidate="txt_descripcion_concepto" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label29" CssClass="control-label text-bold">Cantidad</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_cantidad" runat="server"  Width="90%" MinValue="1">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                            ControlToValidate="txt_cantidad" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label30" CssClass="control-label text-bold">Valor unitario</asp:Label>
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
                                <telerik:RadButton ID="btn_agregar_concepto" runat="server" CssClass="btn btn-sm" Text="Agregar item" ValidationGroup="3" Width="100px">
                                </telerik:RadButton>
                            </div>
                        </div>
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <telerik:RadGrid ID="grd_compras" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_compra_tem" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_anticipo_compra_tem"
                                                    FilterControlAltText="Filter id_anticipo_compra_tem column"
                                                    SortExpression="id_anticipo_compra_tem" UniqueName="id_anticipo_compra_tem"
                                                    Visible="False" HeaderText="id_anticipo_compra_tem"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                    <HeaderStyle Width="4%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                            OnClick="delete_compra">
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
                    </div>
                    <div runat="server" visible="false" id="info_rutas">
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Registrar rutas</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label9" CssClass="control-label text-bold">Municipio / ciudad de salida:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_municipio_salida" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="cmb_municipio_salida" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label10" CssClass="control-label text-bold">Municipio / ciudad de llegada:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_municipio_llegada" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                    ControlToValidate="cmb_municipio_llegada" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label11" CssClass="control-label text-bold">Zona rural:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_zona_rural" autopostback="true" emptymessage="Seleccione..." filter="Contains" runat="server" width="90%" HighlightTemplatedItems="true">
                                     <HeaderTemplate>
                                        <ul>
                                            <li class="col1">Zona rural</li>
                                            <li class="col2">Valor ida y regreso </li>
                                            <li class="col3">Tiempo estimado</li>
                                            <li class="col3">Observaciones adicionales</li>
                                        </ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
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
                                    </ItemTemplate>
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                    ControlToValidate="cmb_zona_rural" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label12" CssClass="control-label text-bold">Cantidad de personas:</asp:Label>
                                <br />
                                <telerik:radnumerictextbox id="txt_cantidad_personas" runat="server" autopostback="true" visible="true" width="90%" value="0" minvalue="0" numberformat-decimaldigits="0">
                                </telerik:radnumerictextbox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                    ControlToValidate="txt_cantidad_personas" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label13" CssClass="control-label text-bold">Valor por trayecto (ida y regreso):</asp:Label>
                                <br />
                                <telerik:radnumerictextbox id="txt_valor_trayecto" runat="server" enabled="false" visible="true" width="90%">
                                </telerik:radnumerictextbox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                    ControlToValidate="txt_valor_trayecto" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label14" CssClass="control-label text-bold">Valor total:</asp:Label>
                                <br />
                                <telerik:radnumerictextbox id="txt_valor_total" runat="server" enabled="false" visible="true" width="90%">
                                </telerik:radnumerictextbox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                    ControlToValidate="txt_valor_total" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">
                                    *</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                             <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Requiere estipendio?:</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_estipendio" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-4" runat="server" visible="false" id="lugarEstipendio">
                                <asp:Label runat="server" ID="Label16" CssClass="control-label text-bold">Lugar estipendio?:</asp:Label>
                                <br />
                                <telerik:radcombobox id="cmb_lugar_estipendio" autopostback="true" emptymessage="Seleccione..." runat="server" width="90%">
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
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
                                <asp:Label runat="server" ID="Label17" CssClass="control-label text-bold">Estipendio de desayuno?:</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_estipendio_desayuno" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                 <div runat="server" visible="false" id="info_desayuno">
                                     <br />
                                     <asp:Label runat="server" ID="Label18" CssClass="control-label text-bold">Cantidad de desayunos por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_cantidad_desayuno" numberformat-decimaldigits="0" value="0" runat="server" AutoPostBack="true" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                     <br />
                                      <asp:Label runat="server" ID="Label19" CssClass="control-label text-bold">Total desayuno por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_total_desayuno" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                 </div>
                                 
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label20" CssClass="control-label text-bold">Estipendio de almuerzo?:</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_estipendio_almuerzo" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                  <div runat="server" visible="false" id="info_almuerzo">
                                       <br />
                                     <asp:Label runat="server" ID="Label21" CssClass="control-label text-bold">Cantidad de aluerzos por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_cantidad_almuerzo" numberformat-decimaldigits="0" value="0" runat="server" AutoPostBack="true" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                     <br />
                                      <asp:Label runat="server" ID="Label22" CssClass="control-label text-bold">Total almuerzos por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_total_almuerzo" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                  </div>
                                
                            </div>
                             <div class="col-sm-6 col-md-6 col-lg-4">
                                <asp:Label runat="server" ID="Label23" CssClass="control-label text-bold">Estipendio de cena?:</asp:Label>
                                <br />
                                <asp:RadioButtonList ID="rbn_estipendio_cena" runat="server"
                                    RepeatColumns="2" Style="height: 26px" AutoPostBack="true">
                                    <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                    <asp:ListItem Value="0">No &nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                                  <div runat="server" visible="false" id="info_cena">
                                      <br />
                                     <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Cantidad de cenas por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_cantidad_cena" numberformat-decimaldigits="0" value="0" runat="server" AutoPostBack="true" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                     <br />
                                      <asp:Label runat="server" ID="Label25" CssClass="control-label text-bold">Total cena por persona:</asp:Label>
                                        <br />
                                     <telerik:radnumerictextbox id="txt_total_cena" value="0" enabled="false" runat="server" width="90%" maxlength="1000">
                                    </telerik:radnumerictextbox>
                                  </div>
                                 
                            </div>
                         </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:Label runat="server" ID="Label15" CssClass="control-label text-bold">Observaciones adicionales:</asp:Label>
                                <br />
                                 <telerik:radtextbox cssclass="upper" id="txt_observaciones_ruta" runat="server" rows="3" textmode="MultiLine" width="97%" maxlength="1000">
                                </telerik:radtextbox>
                            </div>
                          
                        </div>
                         <div class="form-group row">
                            <div class="col-sm-8"></div>
                            <div class="col-sm-4 text-right">
                                <telerik:RadButton ID="btn_agregar_ruta" runat="server" CssClass="btn btn-sm" Text="Agregar ruta" ValidationGroup="2" Width="100px">
                                </telerik:RadButton>
                            </div>
                        </div>
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <hr/>
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
                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_anticipo_ruta_tem" AllowAutomaticUpdates="True" >
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_anticipo_ruta_tem"
                                                FilterControlAltText="Filter id_anticipo_ruta_tem column"
                                                SortExpression="id_anticipo_ruta_tem" UniqueName="id_anticipo_ruta_tem"
                                                Visible="False" HeaderText="id_anticipo_ruta_tem"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                <HeaderStyle Width="4%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                        ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                        OnClick="delete_detalle">
                                                        <asp:Image ID="Image3" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="ciudad_salida"
                                                FilterControlAltText="Filter ciudad_salida column" HeaderStyle-Width="135px"
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
                                                DataFormatString="{0:n0}" 
                                                FilterControlAltText="Filter valor_trayecto column"  HeaderStyle-Width="120px"
                                                HeaderText="Valor ida y regreso" SortExpression="valor_trayecto"
                                                UniqueName="colm_valor_trayecto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="sub_total_ruta" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total ida y regreso: {0:n0}</b>" FooterText="Valor total ida y regreso: "
                                                FilterControlAltText="Filter sub_total_ruta column" HeaderStyle-Width="120px"
                                                HeaderText="Valor total ida y regreso" SortExpression="sub_total_ruta"
                                                UniqueName="colm_valor_trayecto">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total_estipendio" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor estipendio: {0:n0}</b>" FooterText="Valor estipendio: "
                                                FilterControlAltText="Filter valor_trayecto column" HeaderStyle-Width="120px"
                                                HeaderText="Valor estipendio" SortExpression="valor_total_estipendio"
                                                UniqueName="colm_valor_total_estipendio">
                                                <HeaderStyle CssClass="wrapWord"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="valor_total_ruta" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                Aggregate="Sum" DataFormatString="{0:n0}" FooterAggregateFormatString="<b>Valor total: {0:n0}</b>" FooterText="Valor total: "
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
