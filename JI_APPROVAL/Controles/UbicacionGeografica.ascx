<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UbicacionGeografica.ascx.vb" Inherits="RMS_APPROVAL.UbicacionGeografica" %>
<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <div class="col-lg-12">
            <div class="form-group row">
                <div class="col-sm-2" runat="server" id="lbl_dep">
                    <asp:Label runat="server" ID="lblt_departamento" CssClass="control-label text-bold">Departamento</asp:Label>
                </div>
                <div class="col-sm-4" runat="server" id="cmbt_text">
                    <telerik:RadComboBox ID="cmb_departamento" runat="server" Width="90%" AutoPostBack="true"
                        CausesValidation="false">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_municipio" CssClass="control-label text-bold">Municipio</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_municipio" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                </div>
            </div>
            <%--<div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_corregimiento" CssClass="control-label text-bold">Corregimiento</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_corregimiento" runat="server" Width="90%"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_vereda" CssClass="control-label text-bold">Vereda</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_vereda" runat="server" Width="90%" AutoPostBack="false"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                </div>
            </div>--%>
            <%--<div class="form-group row" runat="server" id="divParishVillage">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_parish" CssClass="control-label text-bold">Parish</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_parish" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="cmb_parish" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_village" CssClass="control-label text-bold">Village</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_village" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="cmb_village" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
            </div>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
