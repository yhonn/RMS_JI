<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UbicacionGeograficaJI.ascx.vb" Inherits="RMS_APPROVAL.UbicacionGeograficaJI" %>
<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>

        <div class="card">
           <div class="card-body">
               <a name="id_GEOGRAPHIC"></a>

            
            <div id="dv_cmb_country" runat="server" >
          
               <div class="row"  >
                 <div class="col-sm-2">
                 </div>
                 <div class="col-sm-2">
                      <div class="form-group" id="lbl_country" >
                           <asp:Label runat="server" ID="lblt_country_" CssClass="control-label text-bold">Pais</asp:Label>
                       </div>
                 </div>
                 <div class="col-sm-6">
                      <div class="form-group"  id="cmbt_country" >
                            <telerik:RadComboBox ID="cmb_country" runat="server" Width="90%" AutoPostBack="true"
                                CausesValidation="false">
                            </telerik:RadComboBox>
                       </div>
                 </div>
               </div>

                <div class="row">
                   <div class="col-sm-12">
                         <div class="form-group">
                             <hr />
                         </div>
                   </div>                            
               </div>

            </div>

                <div class="row">
                       <div class="col-sm-2">
                       </div>
                      <div class="col-sm-2" id="lbl_zone">
                         <div class="form-group">
                           <asp:Label runat="server" ID="lblt_zone_" CssClass="control-label text-bold">Región *</asp:Label>
                         </div>
                      </div>
                      <div class="col-sm-6"  id="cmbt_zone">
                           <div class="form-group">                           
                                <telerik:RadComboBox ID="cmb_zone" runat="server" Width="90%" AutoPostBack="true"
                                     CausesValidation="false"                                       
                                     Filter="Contains"
                                     AllowCustomText="true"
                                     HighlightTemplatedItems="true"
                                     EnableLoadOnDemand="True"  
                                     EmptyMessage="Select the Zone..."
                                    OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')">
                                </telerik:RadComboBox>
                            </div>
                      </div>                
                </div>

               <div class="row">
                     <div class="col-sm-2">
                      </div>
                     <div class="col-sm-2" id="lbl_division">
                          <div class="form-group">
                              <asp:Label runat="server" ID="lblt_division_" CssClass="control-label text-bold">Sub-Region *</asp:Label>
                          </div>
                     </div>
                     <div class="col-sm-6" id="cmbt_division">
                          <div class="form-group">

                                 <telerik:RadComboBox ID="cmb_division" runat="server" Width="90%" AutoPostBack="true"
                                    CausesValidation="false"
                                    Filter="Contains"
                                    AllowCustomText="true"
                                    HighlightTemplatedItems="true"
                                    EnableLoadOnDemand="True"  
                                    EmptyMessage="Select Division..." OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')"  >
                                     
                                </telerik:RadComboBox>
                              <asp:RequiredFieldValidator ID="rv_cmb_division" Enabled="false" runat="server"
                            ControlToValidate="cmb_division" CssClass="Error" Display="Dynamic"
                            ErrorMessage="" ValidationGroup="1">Required</asp:RequiredFieldValidator>
                               <%--OnClientSelectedIndexChanged="scrollToAnchor('id_GEOGRAPHIC')" --%>

                          </div>
                     </div>
             </div>

                  <div class="row">
                       <div class="col-sm-12">
                             <div class="form-group">
                                 <hr />
                             </div>
                       </div>
                 </div>


                       <div class="row">
                              <div class="col-sm-6">
                                   <div class="form-group">
                                          <asp:Label runat="server" ID="lblt_departamento_" CssClass="control-label text-bold">Departamento *</asp:Label><br />
                                            <telerik:RadComboBox ID="cmb_district" runat="server" Width="90%" AutoPostBack="true"
                                                CausesValidation="false"
                                                Filter="Contains"
                                                AllowCustomText="true"
                                                HighlightTemplatedItems="true"
                                                EnableLoadOnDemand="True"   EmptyMessage="Select District..."  OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')" >
                                            </telerik:RadComboBox>
                                       <asp:RequiredFieldValidator ID="rv_cmb_district" Enabled="false" runat="server"
                                        ControlToValidate="cmb_district" CssClass="Error" Display="Dynamic"
                                        ErrorMessage="" ValidationGroup="1">Required</asp:RequiredFieldValidator>
                                    </div>
                              </div>
                              <div class="col-sm-6">
                                   <div class="form-group">
                                        <asp:Label runat="server" ID="lblt_municipio_" CssClass="control-label text-bold">Municipio *</asp:Label><br />
                                          <telerik:RadComboBox ID="cmb_upazila" runat="server" Width="90%" AutoPostBack="true"
                                            Filter="Contains"
                                            CausesValidation="false"
                                            AllowCustomText="true"
                                            HighlightTemplatedItems="true"
                                            EnableLoadOnDemand="True"
                                            EmptyMessage="Select County..."  OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')" >
                                        </telerik:RadComboBox>
                                       <asp:RequiredFieldValidator ID="rv_cmb_upazila" Enabled="false" runat="server"
                                        ControlToValidate="cmb_upazila" CssClass="Error" Display="Dynamic"
                                        ErrorMessage="" ValidationGroup="1">Required</asp:RequiredFieldValidator>
                                    </div>
                              </div>
                    </div>


                  <div class="row">

<%--                        <div class="col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblt_union" CssClass="control-label text-bold">Sub-County  *</asp:Label><br />
                                <telerik:RadComboBox ID="cmb_union" runat="server" Width="90%" AutoPostBack="true"
                                    Filter="Contains"
                                    CausesValidation="false"
                                    AllowCustomText="true"
                                    HighlightTemplatedItems="true"
                                    EnableLoadOnDemand="True"
                                    EmptyMessage="Select Sub-county..." OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')">
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rv_cmb_union" Enabled="false" runat="server"
                                        ControlToValidate="cmb_union" CssClass="Error" Display="Dynamic"
                                        ErrorMessage="" ValidationGroup="1">Required</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblt_parish" CssClass="control-label text-bold">Parish / Ward</asp:Label>

                                <telerik:RadComboBox ID="cmb_parish" runat="server" Width="90%" AutoPostBack="true"
                                        Filter="Contains"
                                        CausesValidation="false"
                                        AllowCustomText="true"
                                        HighlightTemplatedItems="true"
                                        EnableLoadOnDemand="True"
                                        EmptyMessage="Select a parish.."  OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')" >
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rv_cmb_parish" runat="server"
                                    ControlToValidate="cmb_parish" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="" ValidationGroup="1">*</asp:RequiredFieldValidator>

                           </div>
                        </div>
                     --%>
                 </div>
                             
                <div class="row">

<%--                       <div class="col-sm-6" id="dv_cmb_village" runat="server" >
                            <div class="form-group">
                                 <asp:Label runat="server" ID="lblt_village" CssClass="control-label text-bold">Village *</asp:Label><br />
                                  <telerik:RadComboBox ID="cmb_village" runat="server" Width="90%" AutoPostBack="false"
                                     Filter="Contains"
                                     CausesValidation="false"
                                     AllowCustomText="true"
                                     HighlightTemplatedItems="true"
                                     EnableLoadOnDemand="True"
                                     EmptyMessage="Select Parish..." OnClientSelectedIndexChanged="scrollToAnchor.ExtraParams('id_GEOGRAPHIC')" >
                                 </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="rv_cmb_village" Enabled="false" runat="server"
                                         ControlToValidate="cmb_village" CssClass="Error" Display="Dynamic"
                                         ErrorMessage="" ValidationGroup="1">Required</asp:RequiredFieldValidator>
                            </div>
                        </div>--%>

                </div>


               <asp:HiddenField id="controlScrollGIS" runat="server" Value="0"/>
                                                  

            <%--
                
                
           <div class="form-group row" runat="server" id="divParishVillage">
                
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
        </div>
     <script>

         var vCounter = 0;

         
            Function.prototype.ExtraParams = function () {
                var method = this, args = Array.prototype.slice.call(arguments);
                return function () {
                    return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                };
            };


         function scrollToAnchor(sender, eventArgs, Params)  {

                var aid = Params[0];

             //console.log('hi folks ' + vCounter + ' ' + aid);
             var aTag = $("a[name='"+ aid +"']");
             $('html,body').animate({ scrollTop: aTag.offset().top }, 'fast');

                 //vCounter++;
             //$("[id*=controlScrollGIS]").val(1);
             $("#<%= controlScrollGIS.ClientID %>").val(1);
         }


            $("#link").click(function() {
               scrollToAnchor('id3');
            });



     </script>

    </ContentTemplate>
</asp:UpdatePanel>
