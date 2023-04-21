<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPopUp.Master" CodeBehind="frm_ProyectoAvanceMetasPrintInfo.aspx.vb" Inherits="ACS_SIME.frm_ProyectoAvanceMetasPrintInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row invoice-info">
        <div class="col-md-12">
            <script language="javascript">
                var formatNumber =
                {
                    sepMiles: ".",
                    sepDecimal: ',',

                    formatear: function (num) {
                        num += '';
                        var splitStr = num.split('.');
                        var splitLeft = splitStr[0];
                        var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : ',00';
                        var regx = /(\d+)(\d{3})/;
                        while (regx.test(splitLeft)) {
                            splitLeft = splitLeft.replace(regx, '$1' + this.sepMiles + '$2');
                        }
                        return this.simbol + splitLeft + splitRight;
                    },
                    new: function (num, simbol) {
                        this.simbol = simbol || '';
                        return this.formatear(num);
                    }
                }

            </script>
            <table class="table table-responsive table-condensed box box-primary">
                <tr>
                    <td>
                        <asp:Label ID="lblt_codigo_proyecto" runat="server"><b>Código de Proyecto:</b></asp:Label>
                        <asp:Label ID="lbl_codigoSAPME" runat="server"></asp:Label>
                        <asp:Label ID="lbl_id_ficha_proyecto" runat="server"></asp:Label>
                        <%--<asp:Label ID="lbl_codigo_proyecto" runat="server"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_estado_proyecto" runat="server"><b>Estado del Proyecto:</b></asp:Label>
                        <%--<asp:Label ID="lbl_estado_proyecto" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_estadoFicha" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_nombre_proyecto" runat="server"><b>Nombre del Proyecto:</b></asp:Label>
                        <%--<asp:Label ID="lbl_nombre_proyecto" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_proyecto" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_descripcion" runat="server"><b>Descripción del Proyecto:</b></asp:Label>
                        <asp:Label ID="lbl_descripcion" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_fecha_inicio" runat="server"><b>Fecha Inicio:</b></asp:Label>
                        <%--<asp:Label ID="lbl_fecha_inicio" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_fechainicio" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_fecha_fin" runat="server"><b>Fecha Fin:</b></asp:Label>
                        <%--<asp:Label ID="lbl_fecha_fin" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_fechafin" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_region" runat="server"><b>Región:</b></asp:Label>
                        <%--<asp:Label ID="Label2" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_region" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_subregion" runat="server"><b>Sub Región:</b></asp:Label>
                        <asp:Label ID="lbl_subregion" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_componente" runat="server"><b>Componente:</b></asp:Label>
                        <%--<asp:Label ID="Label3" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_componente" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_ejecutor" runat="server"><b>Ejecutor:</b></asp:Label>
                        <%--<asp:Label ID="Label4" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_ejecutor" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_codigoAID" runat="server"><b>Código Ficha AID:</b></asp:Label>
                        <asp:Label ID="Label5" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_periodo" runat="server"><b>Periodo:</b></asp:Label>
                        <%--<asp:Label ID="lbl_periodo" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_periodoActivo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_activo" runat="server"><b>Activo:</b></asp:Label>
                        <%--<asp:Label ID="lbl_activo" runat="server"></asp:Label>--%>
                        <asp:Label ID="lbl_periodoActivoSN" runat="server">SI</asp:Label>
                        &nbsp;
                <asp:Image ID="img_periodoActivoSN" runat="server"
                    ImageUrl="~/Imagenes/iconos/flag_green.png" ToolTip="Activo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server"><b>Estatus del periodo:</b></asp:Label>
                        <asp:Label ID="lbl_periodoActivoEstatus" runat="server">Periodo ACTIVO. Permite registrar datos</asp:Label>
                        &nbsp;
                <asp:Image ID="img_periodoActivoEstatus" runat="server"
                    ImageUrl="~/Imagenes/iconos/flag_green.png"
                    ToolTip="Periodo ACTIVO. Permite registrar datos" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td>
                        <asp:Label ID="Label3" runat="server"><b>Código Ficha AID:</b></asp:Label>
                        <asp:Label ID="Label4" runat="server"></asp:Label>
                        <asp:Label ID="lbl_codigoAID" runat="server"></asp:Label>
                        <asp:Label ID="lbl_codigoRFA" runat="server"></asp:Label>
                        <asp:Label ID="lbl_periodoActivoEstatusSync" runat="server">Los datos se han actualizado correctamente. No requiere actualizar o precesar información.</asp:Label>
                        &nbsp;
                <asp:Image ID="img_periodoActivoEstatusSync" runat="server"
                    ImageUrl="~/Imagenes/iconos/flag_green.png"
                    ToolTip="Periodo ACTIVO. Permite registrar datos" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server"><b>Tasa de Cambio:</b></asp:Label>
                        <asp:Label ID="lbl_tasaCambio" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblproyecto18" runat="server" Font-Bold="True"
                            Font-Strikeout="False" Font-Underline="False" Text="Avance de indicadores"></asp:Label>
                        &nbsp;<br />
                        <telerik:RadTreeView ID="TreeViewMetas" runat="server" Skin="Sitefinity"
                            Width="950px">
                        </telerik:RadTreeView>
                        <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT ID_TRIMESTRE, NOMBRE_TRIMESTRE FROM vw_t_periodos where id_trimestre = -1"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

