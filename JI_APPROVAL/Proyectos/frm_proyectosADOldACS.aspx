<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectosADOldACS.aspx.vb" Inherits="ACS_SIME.frm_proyectosADOldACS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Proyecto - Nuevo</asp:Label></h3>
                <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
                                            runat="server" id="alink_informacion">Información General
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Código de Proyecto</asp:Label>
                                            <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                            <%-- Para mantener el tab activo después de hacer un postback --%>
                                            <asp:Label ID="hfAccordionIndex" runat="server" />
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_codigoproyecto" runat="server" Rows="3" TextMode="SingleLine" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="txt_codigoproyecto" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                            <asp:LinkButton ID="lnk_sugerir_codigo" runat="server">Sugerir código de proyecto</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Nombre Proyecto</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_nombreproyecto" runat="server" Rows="5" TextMode="MultiLine" Width="500px">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txt_nombreproyecto" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Descripción proyecto</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="5" TextMode="MultiLine" Width="500px">
                                            </telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                ControlToValidate="txt_descripcion" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadComboBox ID="cmb_ejecutor" runat="server" Width="500px" Filter="Contains">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" Enabled="false"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Región</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                            <asp:CheckBox runat="server" ID="chk_todos" Text="Todos" AutoPostBack="true" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub Región</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                            <telerik:RadGrid ID="grd_subregion" runat="server" AutoGenerateColumns="False" Visible="false">
                                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_subregion">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                            HeaderText="" UniqueName="TemplateColumnAnual">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ctrl_id" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="15px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="id_subregion" DataType="System.Int32"
                                                            FilterControlAltText="Filter id_subregion column" HeaderText="id_subregion"
                                                            ReadOnly="True" SortExpression="id_subregion" UniqueName="id_subregion"
                                                            Display="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_subregion"
                                                            FilterControlAltText="Filter nombre_subregion column"
                                                            HeaderText="Sub Región" SortExpression="nombre_subregion"
                                                            UniqueName="colm_nombre_subregion">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_componente" CssClass="control-label text-bold">Componente</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_componente" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Fecha de Inicio</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadDatePicker ID="dt_fecha_inicio" runat="server"></telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                                ControlToValidate="dt_fecha_inicio" ErrorMessage="*"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">Fecha de Finalización</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadDatePicker ID="dt_fecha_fin" runat="server"></telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error"
                                                ControlToValidate="dt_fecha_fin" ErrorMessage="*"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_trimestre" CssClass="control-label text-bold">Trimestre</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_periodo" runat="server" Width="300px"></telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigoME" CssClass="control-label text-bold">Código ME</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_codigo_SAPME" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Estado</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_estado" runat="server" Width="300px" Enabled="false"></telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="form-group row hidden">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigo_AID" CssClass="control-label text-bold">Código Ficha AID</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_codigoAID" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row hidden">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigoRFA" CssClass="control-label text-bold">Código RFA</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txtx_codigoRFA" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row hidden">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigo_monitor" CssClass="control-label text-bold">Código Monitor</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_codigoMonitor" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row hidden">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_acta_aprobacion" CssClass="control-label text-bold">Acta de Aprobación</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_acta" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row hidden">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_codigo_convenio" CssClass="control-label text-bold">Código de Convenio</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <telerik:RadTextBox ID="txt_codigo_convenio" runat="server" Width="250px" MaxLength="250">
                                            </telerik:RadTextBox>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="form-group row">
                                            <div class="col-sm-3 text-right">
                                                <asp:Label runat="server" ID="lblt_imagen_proyecto" CssClass="control-label text-bold">Imagen de Proyecto</asp:Label>
                                            </div>
                                            <div class="col-sm-9">
                                                <asp:UpdatePanel ID="UpdatePanel4"
                                                    runat="server">
                                                    <ContentTemplate>

                                                        <asp:Panel ID="Panel5" runat="server" BorderColor="#E0E0E0"
                                                            BorderWidth="1px" Style="border-left-color: gray; border-bottom-color: gray; border-top-style: dashed; border-top-color: gray; border-right-style: dashed; border-left-style: dashed; border-right-color: gray; border-bottom-style: dashed"
                                                            Width="505px">
                                                            <table border="0" cellpadding="0" cellspacing="0"
                                                                style="width: 489px; margin-right: 0px;">
                                                                <tr>
                                                                    <td colspan="2" style="vertical-align: top; width: 462px; height: 53px">
                                                                        <CuteWebUI:Uploader ID="Uploader2" runat="server" CancelAllMsg="Cancel All"
                                                                            CancelUploadMsg="Update Cancel" FileTooLargeMsg="Error al cargar el archivo, Máximo permitido 10Mb"
                                                                            FileTypeNotSupportMsg="Archivo con exensión desconicida. Permitidos:xls,xlsx y zip"
                                                                            InsertText="Attachment (Max 10Mb)"
                                                                            WindowsDialogLimitMsg="Imposible de seleccionar todos los archivos a la vez">
                                                                            <ValidateOption AllowedFileExtensions="png, jpg, bmp" MaxSizeKB="10240" />
                                                                        </CuteWebUI:Uploader>
                                                                        <asp:Panel ID="Panel6" runat="server" Height="50px" Visible="False"
                                                                            Width="385px">
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="border: thin solid #ededed; width: 370px;">
                                                                                <tr>
                                                                                    <td style="vertical-align: middle; width: 100px; height: 16px">
                                                                                        <asp:ImageButton ID="imgEliminaImg" runat="server"
                                                                                            ImageUrl="~/Imagenes/iconos/b_drop.png" ToolTip="Eliminar imagen" />
                                                                                    </td>
                                                                                    <td style="vertical-align: middle; width: 53px; height: 16px">&nbsp;&nbsp;</td>
                                                                                    <td style="width: 1785px; height: 16px">
                                                                                        <asp:Label ID="lblarchivoImg" runat="server" Width="332px"></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 131px; height: 16px">&nbsp;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="vertical-align: middle; width: 762px; height: 53px; text-align: center;">
                                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:Image ID="imgUser" runat="server" BackColor="#CCCCCC" BorderColor="Black"
                                                                                    BorderStyle="Double" Height="64px" ImageUrl="~/Imagenes/iconos/photo64X64.png"
                                                                                    Width="64px" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <asp:HyperLink ID="HLKFoto" runat="server" Font-Bold="True" Font-Names="Arial"
                                                                            Font-Size="X-Small" ForeColor="RoyalBlue" Target="_blank" Visible="False">Ampliar</asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingTwo" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo"
                                            runat="server" id="alink_region">Región beneficiada
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                    <div class="form-group row">
                                        <br />
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_departamento" CssClass="control-label text-bold">Departamento</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_departamento" runat="server" Width="300px" AutoPostBack="true" DataSourceID="SqlDataSource2"></telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="cmb_departamento" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="registre una descripción" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_municipio" CssClass="control-label text-bold">Municipio</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_municipio" runat="server" Width="300px"></telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                ControlToValidate="cmb_municipio" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="registre una descripción" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadButton ID="btn_guardar1" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                                Text="Agregar" ValidationGroup="2" Width="75px">
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadGrid ID="grd_cobertura" runat="server" AllowAutomaticDeletes="True"
                                                AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                                <ClientSettings EnableRowHoverStyle="true">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ficha_municipio_tmp"
                                                    AllowAutomaticUpdates="True">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="id_ficha_municipio_tmp"
                                                            FilterControlAltText="Filter id_ficha_municipio_tmp column"
                                                            SortExpression="id_ficha_municipio_tmp" UniqueName="id_ficha_municipio_tmp"
                                                            Visible="False" HeaderText="id_ficha_municipio_tmp"
                                                            ReadOnly="True">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Eliminar">
                                                            <HeaderStyle Width="10" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                    OnClick="EliminarMunicipio_Click">
                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="Edit" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" Target="_self" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5px" />
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn DataField="nombre_departamento"
                                                            FilterControlAltText="Filter nombre_departamento column" HeaderText="Departamento" UniqueName="colm_nombre_departamento">
                                                            <HeaderStyle Width="150px" />
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="nombre_municipio"
                                                            FilterControlAltText="Filter nombre_municipio column"
                                                            HeaderText="Municipio" SortExpression="nombre_municipio"
                                                            UniqueName="colm_nombre_municipio">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingThree" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="true" aria-controls="collapseThree">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseThree" aria-expanded="true" aria-controls="collapseThree"
                                            runat="server" id="alink_sectores">Sectores
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                    <br />
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadGrid ID="grd_Sectores" runat="server" AutoGenerateColumns="False">
                                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_sector_me">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                            HeaderText="" UniqueName="TemplateColumnAnual">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="IdSector" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="15px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="id_sector_me" DataType="System.Int32"
                                                            FilterControlAltText="Filter id_sector_me column" HeaderText="id_sector_me"
                                                            ReadOnly="True" SortExpression="id_sector_me" UniqueName="id_sector_me"
                                                            Display="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_sector_me"
                                                            FilterControlAltText="Filter nombre_sector_me column"
                                                            HeaderText="Sector asociado" SortExpression="nombre_sector_me"
                                                            UniqueName="colm_nombre_sector_me">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <GroupByExpressions>
                                                        <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="nombre_tipo_sector_me"
                                                                    FieldName="nombre_tipo_sector_me" FormatString="" HeaderText=" "
                                                                    HeaderValueSeparator=" " />
                                                            </SelectFields>
                                                            <GroupByFields>
                                                                <telerik:GridGroupByField FieldAlias="nombre_tipo_sector_me"
                                                                    FieldName="nombre_tipo_sector_me" FormatString="" HeaderText="" SortOrder="Descending" />
                                                            </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                    </GroupByExpressions>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                            <br />
                                            <asp:Label ID="lbl_errorsector" runat="server" Font-Bold="True"
                                                Font-Names="Arial" Font-Size="X-Small" ForeColor="#C00000" Visible="False">[ Seleccione un sector ]</asp:Label><br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingSix" data-toggle="collapse" data-parent="#accordion" href="#collapseSix" aria-expanded="true" aria-controls="collapseSix">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseSix" aria-expanded="true" aria-controls="collapseSix"
                                            runat="server" id="alink_indicadores">Indicadores
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseSix" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSix">
                                    <div class="form-group row">
                                        <br />
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_indicador" CssClass="control-label text-bold">Indicador</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_indicador" runat="server" Width="500px" Filter="Contains"></telerik:RadComboBox>
                                            <telerik:RadButton ID="btn_guardarIndicador" runat="server"  AutoPostBack="true" CssClass="btn btn-sm"
                                                Text="Agregar" ValidationGroup="2" Width="75px">
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadGrid ID="grd_indicadores" runat="server" AllowAutomaticDeletes="True"
                                                AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                                <ClientSettings EnableRowHoverStyle="true">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_meta_ficha" AllowAutomaticUpdates="True">
                                                    <Columns>

                                                        <telerik:GridBoundColumn DataField="id_meta_ficha"
                                                            DataType="System.Int32"
                                                            FilterControlAltText="Filter id_meta_ficha column"
                                                            HeaderText="id_meta_ficha" ReadOnly="True"
                                                            SortExpression="id_meta_ficha" UniqueName="id_meta_ficha"
                                                            Visible="False">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn UniqueName="Eliminar">
                                                            <HeaderStyle Width="10" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                    OnClick="EliminarIndicador_Click">
                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn DataField="definicion_indicador" FilterControlAltText="Filter definicion_indicador column"
                                                            HeaderText="Indicador asociado" SortExpression="definicion_indicador" UniqueName="colm_definicion_indicador">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                            HeaderText="Meta total" UniqueName="colm_meta_total">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="txt_TotalIndicador" runat="server" MinValue="0">
                                                                    <NumberFormat ZeroPattern="n" />
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="15px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="meta_total_ficha" FilterControlAltText="Filter meta_total_ficha column"
                                                            HeaderText="meta_total_ficha" SortExpression="meta_total_ficha" UniqueName="meta_total_ficha" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingFour" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="true" aria-controls="collapseFour">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseFour" aria-expanded="true" aria-controls="collapseFour"
                                            runat="server" id="alink_aportes">Aportes
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                    <div class="form-group row">
                                        <br />
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_fuente_aporte" CssClass="control-label text-bold">Fuente de Aporte</asp:Label>
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadComboBox ID="cmb_fuente_aporte" runat="server" Width="500px" AutoPostBack="true" Filter="Contains"></telerik:RadComboBox>
                                            <telerik:RadButton ID="btn_guardarAporte" runat="server" CssClass="btn btn-sm" Text="Agregar" ValidationGroup="2" Width="75px">
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                        </div>
                                        <div class="col-sm-8">
                                            <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True"
                                                AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                                <ClientSettings EnableRowHoverStyle="true">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_aporte_ficha" AllowAutomaticUpdates="True">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="id_aporte_ficha"
                                                            DataType="System.Int32"
                                                            FilterControlAltText="Filter id_aporte_ficha column"
                                                            HeaderText="id_aporte_ficha" ReadOnly="True"
                                                            SortExpression="id_aporte_ficha" UniqueName="id_aporte_ficha"
                                                            Visible="False">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Eliminar">
                                                            <HeaderStyle Width="10" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                    OnClick="EliminarAporte_Click">
                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn DataField="nombre_aporte" FilterControlAltText="Filter nombre_aporte column"
                                                            HeaderText="Fuente de Aporte" SortExpression="nombre_aporte" UniqueName="colm_nombre_aporte">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                            HeaderText="Meta total" UniqueName="colm_meta_total">
                                                            <ItemTemplate>
                                                                <telerik:RadNumericTextBox ID="txt_total_aporte" runat="server" MinValue="0"
                                                                    AutoPostBack="True" OnTextChanged="txt_meta_TextChanged">
                                                                    <NumberFormat ZeroPattern="n" />
                                                                </telerik:RadNumericTextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="15px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="monto_aporte" FilterControlAltText="Filter monto_aporte column"
                                                            HeaderText="monto_aporte" SortExpression="monto_aporte" UniqueName="monto_aporte" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12 text-right">
                                            <h4>
                                                <asp:Label runat="server" ID="lblt_total_aporte" CssClass="text-bold">Total Aporte:</asp:Label>
                                                <asp:Label runat="server" ID="lbl_total"></asp:Label>
                                            </h4>
                                            <h4>
                                                <asp:Label runat="server" ID="lblt_total_aporteUSD" CssClass="text-bold">Total Aporte USD:</asp:Label>
                                                <asp:Label runat="server" ID="lbl_totalUSD"></asp:Label>
                                            </h4>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingFive" data-toggle="collapse" data-parent="#accordion" href="#collapseFive" aria-expanded="true" aria-controls="collapseFive">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseFive" aria-expanded="true" aria-controls="collapseFive"
                                            runat="server" id="alink_documentos">Documentos
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseFive" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                                    <div class="form-group row">
                                        <div class="col-sm-3 text-right">
                                            <asp:Label runat="server" ID="lblt_documento" CssClass="control-label text-bold">Documento</asp:Label>
                                        </div>
                                        <div class="col-sm-9">
                                            <asp:UpdatePanel ID="PanelFirma" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Panel1" runat="server" BorderColor="#E0E0E0" BorderWidth="1px" Style="border-left-color: gray; border-bottom-color: gray; border-top-style: dashed; border-top-color: gray; border-right-style: dashed; border-left-style: dashed; border-right-color: gray; border-bottom-style: dashed"
                                                        Width="650px">
                                                        <CuteWebUI:Uploader ID="UploaderProyecto" runat="server"
                                                            CancelAllMsg="Cancelar todos" CancelUploadMsg="Cancel Carga"
                                                            FileTooLargeMsg="Error al cargar el archivo, Máximo permitido 10Mb"
                                                            FileTypeNotSupportMsg="Archivo con exensión desconicida. Permitidos: doc, docx, pdf,xls, xlsx"
                                                            InsertText="Adjuntar (Max 10Mb)"
                                                            WindowsDialogLimitMsg="Imposible de seleccionar todos los archivos a la vez">
                                                            <ValidateOption MaxSizeKB="10240" />
                                                        </CuteWebUI:Uploader>
                                                        <asp:Panel ID="Panel1_firma" runat="server" Height="50px" Visible="False"
                                                            Width="625px">
                                                            <table border="0" style="border: thin solid #ededed; width: 469px;">
                                                                <tr>
                                                                    <td style="vertical-align: middle;">
                                                                        <asp:Image ID="Image1_firma" runat="server"
                                                                            ImageUrl="~/Imagenes/iconos/attach.png" /></td>
                                                                    <td style="width: 450px; height: 16px">
                                                                        <asp:Label ID="lblarchivo" runat="server" Height="18px" Text="Error.."
                                                                            Width="450px"></asp:Label></td>
                                                                    <td style="width: 80px; height: 16px"><%--         &nbsp;<asp:ImageButton ID="" runat="server" --%>
                                                                        <asp:ImageButton ID="img_btn_borrar_temp" runat="server"
                                                                            CausesValidation="False" Height="16px" ImageUrl="~/Imagenes/iconos/b_drop.png"
                                                                            ToolTip="Delete file" /></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:Panel>
                                                    <telerik:RadGrid ID="grd_archivos" runat="server" AllowAutomaticDeletes="True"
                                                        AutoGenerateColumns="False" CellSpacing="0" DataSourceID="archivos_temp" GridLines="None" Width="647px">
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Selecting AllowRowSelect="True" />
                                                        </ClientSettings>
                                                        <MasterTableView DataKeyNames="id_archivo_temp" DataSourceID="archivos_temp">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column"
                                                                    HeaderButtonType="PushButton" UniqueName="ImageDownload">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hlk_ImageDownload" runat="server" ImageUrl="~/imagenes/iconos/download.png" ToolTip="Adjunto" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5px" />
                                                                    <ItemStyle Width="5px" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                                    ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                                                    ConfirmDialogWidth="400px"
                                                                    ConfirmText="Confirma que desea eliminar el archivo adjunto?"
                                                                    ConfirmTitle="Eliminar archivo" ImageUrl="../Imagenes/iconos/b_drop.png"
                                                                    UniqueName="Eliminar" />
                                                                <telerik:GridBoundColumn DataField="archivo"
                                                                    FilterControlAltText="Filter ruta_archivos column"
                                                                    HeaderText="Documentos adjuntos" SortExpression="archivo"
                                                                    UniqueName="colm_ruta_archivos">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid><asp:SqlDataSource ID="archivos_temp" runat="server"
                                                        ConflictDetection="CompareAllValues"
                                                        ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                                        OldValuesParameterFormatString="original_{0}"
                                                        ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"
                                                        SelectCommand="SELECT * FROM tme_Anexos_ficha_temp WHERE (id_sesion_temp = @id_sesion)">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lbl_id_sesion_temp" DefaultValue="-1"
                                                                Name="id_sesion" PropertyName="Text" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <br />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingSeven" data-toggle="collapse" data-parent="#accordion" href="#collapseSeven" aria-expanded="true" aria-controls="collapseSeven">
                                    <h4 class="panel-title">
                                        <a role="button" data-toggle="collapse" href="#collapseSeven" aria-expanded="true" aria-controls="collapseSeven">Indicadores
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseSeven" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingSeven">
                                </div>
                            </div>--%>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" runat="server" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                                Width="100px" ValidationGroup="1">
                            </telerik:RadButton>
                        </div>
                    </div>
                    <!-- /.box-footer -->

                    <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                        <div class="vertical-alignment-helper">
                            <div class="modal-dialog modal-sm vertical-align-center">
                                <div class="modal-content">
                                    <div class="modal-header modal-danger">
                                        <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar Registro</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btn_eliminarMunicipio" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarAporte" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarIndicador" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script src="../Scripts/js-cookie.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                //when a group is shown, save it as the active accordion group
                $("#accordion").on('shown.bs.collapse', function () {
                    var active = $("#accordion .in").attr('id');
                    Cookies.set('activeAccordionGroup', active);
                    console.log(active);
                });
                $("#accordion").on('hidden.bs.collapse', function () {
                    Cookies.remove('activeAccordionGroup');
                });
            });
        </script>
    </section>
</asp:Content>

