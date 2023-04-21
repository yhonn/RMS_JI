<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurement_question_config.aspx.vb" Inherits="RMS_APPROVAL.frm_measurement_question_config" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>


<asp:content runat="server" id="MainContent" contentplaceholderid="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Assessment</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Assesment Questions Configuration</asp:Label>
                    <asp:Label runat="server" ID="lbl_survey_name" CssClass="text-bold"></asp:Label>
                    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_themes" href="#">Main Category</a></li>
                                <li role="presentation"><a runat="server" id="alink_skills">Sub-Category</a></li>
                                <li role="presentation"><a runat="server" id="alink_title">Group Category</a></li>
                                <li role="presentation"><a runat="server" id="alink_answer_scale">Answer Scale</a></li>
                                <li role="presentation"><a runat="server" id="alink_answer_options">Answer Options</a></li>
                                <li role="presentation"><a runat="server" id="alink_questions">Questions</a></li>
                                <li role="presentation"><a runat="server" id="alink_survey">Assesment</a></li>
                                <li role="presentation" class="active"><a runat="server" class="primary" id="alink_survey_questions">Assesment Questions</a></li>
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">
                            <div class="form-group row">
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lbl_survey" CssClass="control-label text-bold">Assesment Name</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox runat="server" ID="cmb_survey" Width="500px" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_theme" CssClass="control-label text-bold">Main Category</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox runat="server" ID="cmb_theme" Width="500px" AutoPostBack="true"></telerik:RadComboBox>
                                    <br />
                                    <asp:Label ID="lblt_max_percent_text" runat="server" Text="Maximum percentage available for the selected Theme: " CssClass="text-bold" /><asp:Label ID="lbl_max_percentage" runat="server"/>
                                </div>
                            </div>
                            <div class="form-group row">
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_question" CssClass="control-label text-bold">Question</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox runat="server" ID="cmb_question" Width="500px" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_percent_value" CssClass="control-label text-bold">Percentage Value</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadNumericTextBox ID="txt_percent" runat="server" Width="100px" NumberFormat-DecimalDigits="3" MinValue="0" MaxValue="100"></telerik:RadNumericTextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <br />
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_order_number" CssClass="control-label text-bold">Order Number</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadNumericTextBox ID="txt_order_number" runat="server" Width="100px" NumberFormat-DecimalDigits="0" MinValue="0"></telerik:RadNumericTextBox>
                                    <telerik:RadButton ID="btn_add" runat="server" AutoPostBack="true" CssClass="btn btn-sm"
                                            Text="Add" ValidationGroup="2" Width="75px">
                                        </telerik:RadButton>
                                </div>
                            </div>
                            
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_questions" runat="server" AllowAutomaticDeletes="True"
                                        AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_measurement_question_config" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_measurement_question_config"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_measurement_question_config column"
                                                    HeaderText="id_measurement_question_config" ReadOnly="True"
                                                    SortExpression="id_measurement_question_config" UniqueName="id_measurement_question_config"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn UniqueName="colm_delete" Visible="true" Display="true" >
                                                    <HeaderStyle Width="10" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Delete"
                                                            OnClick="DeleteQuestion_Click">
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="Order Number" UniqueName="colm_order_number">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txt_order_number" runat="server" MinValue="0" NumberFormat-DecimalDigits="0" MaxValue="99" Width="100px">
                                                        </telerik:RadNumericTextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="Question" UniqueName="colm_questiion">
                                                    <ItemTemplate>
                                                        <telerik:RadComboBox runat="server" ID="cmb_question" Width="500px" Enabled ="false"></telerik:RadComboBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                    HeaderText="Percentage Value" UniqueName="colm_percent_value">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txt_value" runat="server" MinValue="0" NumberFormat-DecimalDigits="3" MaxValue="100" Width="100px">
                                                        </telerik:RadNumericTextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15px" />
                                                </telerik:GridTemplateColumn>
                                                
                                                <telerik:GridBoundColumn DataField="id_measurement_question" FilterControlAltText="Filter id_measurement_question column"
                                                    HeaderText="id_measurement_question" SortExpression="id_measurement_question" UniqueName="id_measurement_question" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="percent_value" FilterControlAltText="Filter percent_value column"
                                                    HeaderText="percent_value" SortExpression="percent_value" UniqueName="percent_value" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="order_number" FilterControlAltText="Filter order_number column"
                                                    HeaderText="order_number" SortExpression="order_number" UniqueName="order_number" Visible="false">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Save" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label Text="The sum of the percentage of the questions must be less than the total points for the selected theme" runat="server" CssClass="text-red pull-right margin-r-5" Visible="false" ID="lbl_alert" />
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
                                    <asp:Button runat="server" ID="btn_deleteRegister" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:content>

