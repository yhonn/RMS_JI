<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UbicacionGeograficaLAC.ascx.vb" Inherits="RMS_APPROVAL.UbicacionGeograficaLAC_OlD" %>
<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>

        <div class="col-lg-12 col-md-12 col-sm-12">

             <div class="form-group row">
                <div class="col-sm-2" runat="server" id="lbl_country">
                    <asp:Label runat="server" ID="lblt_country" CssClass="control-label text-bold">Country</asp:Label>
                </div>
                <div class="col-sm-4" runat="server" id="cmbt_country">
                    <telerik:RadComboBox ID="cmb_country" runat="server" Width="90%" AutoPostBack="true"
                        CausesValidation="false">
                    </telerik:RadComboBox>
                </div>
              </div>

               <div class="form-group row">
                <div class="col-sm-2" runat="server" id="lbl_zone">
                    <asp:Label runat="server" ID="lblt_zone" CssClass="control-label text-bold">Zone</asp:Label>
                </div>
                <div class="col-sm-4" runat="server" id="cmbt_zone">
                    <telerik:RadComboBox ID="cmb_zone" runat="server" Width="90%" AutoPostBack="true"
                        CausesValidation="false">
                    </telerik:RadComboBox>
                </div>
              </div>

            <div class="form-group row">
                <div class="col-sm-2" runat="server" id="lbl_division">
                    <asp:Label runat="server" ID="lblt_division" CssClass="control-label text-bold">Division</asp:Label>
                </div>
                <div class="col-sm-4" runat="server" id="cmbt_division">
                    <telerik:RadComboBox ID="cmb_division" runat="server" Width="90%" AutoPostBack="true"
                        CausesValidation="false">
                    </telerik:RadComboBox>
                </div>
              </div>

            <div class="form-group row">
                <div class="col-sm-2" runat="server" id="lbl_dep">
                    <asp:Label runat="server" ID="lblt_departamento" CssClass="control-label text-bold">District</asp:Label>
                </div>
                <div class="col-sm-4" runat="server" id="cmbt_text">
                    <telerik:RadComboBox ID="cmb_departamento" runat="server" Width="90%" AutoPostBack="true"
                        CausesValidation="false">
                    </telerik:RadComboBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_municipio" CssClass="control-label text-bold">Upazila</asp:Label>
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
            <div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_corregimiento" CssClass="control-label text-bold">Union/City/Town</asp:Label>
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
                    <asp:Label runat="server" ID="lblt_vereda" CssClass="control-label text-bold">Village/Ward</asp:Label>
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
            </div>
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
