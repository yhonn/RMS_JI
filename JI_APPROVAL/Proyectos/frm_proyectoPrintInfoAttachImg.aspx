<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectoPrintInfoAttachImg.aspx.vb" Inherits="ACS_SIME.frm_proyectoPrintInfoAttachImg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Imagenes del Proyecto</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Código Ficha</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                            <asp:Label ID="lbl_id_proyecto" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_id_ejecutor" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_codigo_ficha" runat="server"></asp:Label>
                            <asp:Label ID="lbl_guardado" runat="server" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Nombre de Proyecto</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ficha" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ejecutor" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Subregión</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_subregion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Estado</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_estado" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:LinkButton ID="lnk_adjuntar" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" ForeColor="RoyalBlue">Adjuntar nueva fotografía</asp:LinkButton>
                        </div>
                        <div class="col-sm-8">
                            <asp:UpdatePanel ID="UpdatePanel4"
                                runat="server" Visible="False">
                                <ContentTemplate>

                                    <asp:Panel ID="Panel5" runat="server" BorderColor="#E0E0E0"
                                        BorderWidth="1px" Style="border-left-color: gray; border-bottom-color: gray; border-top-style: dashed; border-top-color: gray; border-right-style: dashed; border-left-style: dashed; border-right-color: gray; border-bottom-style: dashed"
                                        Width="505px" Height="200px" HorizontalAlign="Right">
                                        <table border="0" cellpadding="0" cellspacing="0"
                                            style="width: 489px; margin-right: 0px; height: 0px;">
                                            <tr>
                                                <td colspan="2" style="vertical-align: top; height: 10px">
                                                    <table cellpadding="0"
                                                        style="border-collapse: collapse; width: 503px; border-color: #9a7328; border-width: 0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblproyecto43" runat="server" Font-Bold="True"
                                                                    Font-Strikeout="False" Font-Underline="False" Text="Tipo:"></asp:Label>
                                                            </td>
                                                            <td style="height: 10px">
                                                                <asp:RadioButtonList ID="RBEstados" runat="server"
                                                                    DataSourceID="SqlDataSource1" DataTextField="nombre_tipo_proyecto_imagen"
                                                                    DataValueField="id_tipo_proyecto_imagen" RepeatDirection="Horizontal">
                                                                </asp:RadioButtonList>
                                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                                                    SelectCommand="SELECT * FROM tme_FichaProyectoImageTipo"></asp:SqlDataSource>
                                                            </td>
                                                            <td style="height: 10px; text-align: right; vertical-align: top;">
                                                                <asp:ImageButton ID="ImageButton2" runat="server"
                                                                    ImageUrl="~/Imagenes/iconos/close.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top; width: 462px;">
                                                    <CuteWebUI:Uploader ID="Uploader2" runat="server" CancelAllMsg="Cancel All"
                                                        CancelUploadMsg="Update Cancel"
                                                        FileTooLargeMsg="Error al cargar el archivo, Máximo permitido 10Mb"
                                                        FileTypeNotSupportMsg="Archivo con exensión desconicida. Permitidos:xls,xlsx y zip"
                                                        InsertText="Attachment (Max 10Mb)"
                                                        WindowsDialogLimitMsg="Imposible de seleccionar todos los archivos a la vez">
                                                        <%--<ValidateOption AllowedFileExtensions="png, jpg, bmp" MaxSizeKB="10240" />--%>
                                                    </CuteWebUI:Uploader>
                                                    <asp:Panel runat="server" ID="pnldescripcion">
                                                        <asp:TextBox runat="server" ID="txtdescripcion" TextMode="MultiLine" Width="200" />
                                                    </asp:Panel>
                                                    <asp:Panel ID="Panel6" runat="server" Height="23px" Visible="False"
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
                                                                <td style="width: 131px; height: 16px">
                                                                    <asp:ImageButton ID="imgEliminaImg0" runat="server"
                                                                        ImageUrl="~/Imagenes/iconos/drop-yes.gif" ToolTip="Adjuntar imagen" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                                AllowAutomaticUpdates="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource8">
                                <MasterTableView DataSourceID="SqlDataSource8" DataKeyNames="id_ficha_proyecto_imagen">
                                    <Columns>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" Visible="false"
                                            ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                            ConfirmDialogWidth="400px"
                                            ConfirmText="Confirma que desea eliminar el registro?"
                                            ConfirmTitle="Eliminar registro" ImageUrl="../Imagenes/iconos/b_drop.png"
                                            UniqueName="Eliminar" FilterControlAltText="Filter Eliminar column">
                                            <ItemStyle Width="5px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter archivo column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_archivo_proyecto"
                                            FilterControlAltText="Filter nombre_archivo_proyecto column" HeaderText="Archivo"
                                            SortExpression="nombre_archivo_proyecto"
                                            UniqueName="colm_nombre_archivo_proyecto">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_tipo_proyecto_imagen"
                                            FilterControlAltText="Filter nombre_tipo_proyecto_imagen column" HeaderText="Tipo imagen"
                                            SortExpression="nombre_tipo_proyecto_imagen"
                                            UniqueName="colm_nombre_tipo_proyecto_imagen">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Completo column"
                                            UniqueName="colm_AttachFile">
                                            <ItemTemplate>
                                                <asp:Image ID="FileImg" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5px" />
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Completo column"
                                            UniqueName="AttachFile">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="AttachImg" runat="server" ImageUrl="~/Imagenes/iconos/photo16x16.png"
                                                    ToolTip="Ampliar y descargar">
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                            <ItemStyle Width="5px" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource8" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT * FROM tme_FichaProyectoImagen"></asp:SqlDataSource>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
