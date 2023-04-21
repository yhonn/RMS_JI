<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurementDetail.aspx.vb" Inherits="RMS_APPROVAL.frm_measurementDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Skills Assessment</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Survey Participants by School</asp:Label></h3>
                <div class="pull-right box-tools">
                    <asp:HyperLink runat="server" ID="btn_export" Text="Export" CssClass="btn btn-primary btn-sm pull-right margin-r-5"></asp:HyperLink>
                </div>
            </div>
            <div class="box-body">
                <div class="col-sm-12">
                    <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_school_name" CssClass="control-label text-bold">School</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_school_name"></asp:Label>
                        </div>
                    </div>
                     <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_schoolType" CssClass="control-label text-bold">Type of School</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_School_type"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_moderator" CssClass="control-label text-bold">Moderator</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_moderator"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_answer_date" CssClass="control-label text-bold">Survey Answer Date</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_answer_date"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_participant_number" CssClass="control-label text-bold">Participant Number</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_participant_number"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2 text-right">
                            <asp:Label runat="server" ID="lblt_survey_name" CssClass="control-label text-bold">Survey Type</asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:Label runat="server" ID="lbl_survey_name"></asp:Label>
                        </div>
                        <br />
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <div id="revenue-chart" style="width: 100%; height: 675px; margin: 0 auto" class="<%= Me.showGraphic %>"></div>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
                            <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>
                            <script type="text/javascript">
                                var prm = Sys.WebForms.PageRequestManager.getInstance();
                                prm.add_endRequest(function (s, e) {
                                    loadchart()
                                });
                                $(document).ready(function () {
                                    loadchart()

                                });
                                function loadchart() {
                                    $('#revenue-chart').highcharts({
                                        chart: {
                                            polar: true,
                                            type: 'area',
                                            height: 650
                                        },
                                        title: {
                                            text: 'Theme score by school'
                                        },
                                        plotOptions: {
                                            column: {
                                                colorByPoint: true
                                            }
                                        },
                                        //colors: [
                                        //    '#9C9C9C',
                                        //    '#d0f6ff',
                                        //    '#ff0000']
                                        //,
                                        xAxis: {
                                            categories: ['Higher Order Thinking Skills', '', 'Positive Self-Concept', '',
                                                'Self-Control', '', 'Communication', '', 'Social', ''],
                                            tickmarkPlacement: 'on',
                                            tickInterval: 0.5,
                                            lineWidth: 0
                                        },
                                        yAxis: {
                                            gridLineInterpolation: 'polygon',
                                            lineWidth: 0,
                                            tickInterval: 0.5,
                                            min: 0,
                                            max: <%= MaxValue %>
                                        },
                                        legend: {
                                            enabled: true
                                        },

                                        tooltip: {
                                            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}</b><br/>'
                                        },

                                        series: [
                                            {
                                                name: 'Optimal',
                                                pointPlacement: 'on',
                                                data: [<%= Me.chartMax %>],
                                                color: '#ED7620',
                                                fillColor: 'rgba(237, 118, 33, 0.86)'                                              
                                            }, {
                                                name: 'Baseline',
                                                pointPlacement: 'on',
                                                data: [<%= Me.chart %>],
                                                color: '#adadad',   
                                                fillColor: 'rgba(173, 173, 173, 0.74)' 
                                            }
                                            , {
                                                name: 'EndLine',
                                                pointPlacement: 'on',
                                                data: [<%= Me.chartEndLine %>],
                                                color: '#51946a',                                                
                                                fillColor: 'rgba(81, 148, 106, 0.55)'
                                            }

                                        ]

                                    });
                                }
                            </script>
                        </div>
                    </div>
                    <telerik:RadTextBox ID="txt_doc" runat="server"
                        EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                        ValidationGroup="1">
                        <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                    </telerik:RadTextBox>
                    <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                    </telerik:RadButton>
                    <asp:HyperLink runat="server" ID="btn_export_template" Text="Export Template" CssClass="btn-export RadButton RadButton_Metro rbSkinnedButton"></asp:HyperLink>
                    <telerik:RadButton ID="btn_upload_template" runat="server" AutoPostBack="true" Text="Upload Template" Width="100px">
                    </telerik:RadButton>
                    <hr />
                    <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                        CellSpacing="0" AllowPaging="True" PageSize="15"
                        AutoGenerateColumns="False">
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_detail" AllowAutomaticUpdates="True" AllowSorting="true">
                            <Columns>
                                <telerik:GridBoundColumn DataField="id_measurement_detail"
                                    FilterControlAltText="Filter id_measurement_detail column"
                                    SortExpression="id_measurement_detail" UniqueName="id_measurement_detail"
                                    Visible="False" DataType="System.Int32" HeaderText="id_measurement_detail"
                                    ReadOnly="True">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="false">
                                    <HeaderStyle Width="10" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar" OnClick="Eliminar_Click">
                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="Edit" Visible="false">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar usuario" Target="_self" />
                                    </ItemTemplate>
                                    <ItemStyle Width="5px" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Print" Visible="false">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/printer_off.png" Target="_blank"
                                            ToolTip="Imprimir">
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="sex_type"
                                    FilterControlAltText="Filter sex_type column"
                                    HeaderText="Sex" SortExpression="sex_type"
                                    UniqueName="colm_sex_type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="schooling_status"
                                    FilterControlAltText="Filter schooling_status column"
                                    HeaderText="School Status" SortExpression="schooling_status"
                                    UniqueName="colm_schooling_status">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="class_level_name"
                                    FilterControlAltText="Filter class_level_name column"
                                    HeaderText="Class Level" SortExpression="class_level_name"
                                    UniqueName="colm_class_level_name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="age"
                                    FilterControlAltText="Filter age column" HeaderText="Age" UniqueName="colm_age">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="sincronizado" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="recount" UniqueName="recount" Visible="False"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>

            </div>
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
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>

