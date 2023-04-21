<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_measurement_dashboard.aspx.vb" Inherits="RMS_APPROVAL.frm_measurement_dashboard" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
         <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>
         <script src="<%=ResolveUrl("~/Content/dist/js/Skill_Assessment_dash.js")%>"></script>         
         <script src="../Content/plugins/knob/jquery.knob.js"></script>

    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Skills Assessment</asp:Label>
        </h1>
    </section>
    <section class="content">

        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Dashboard</asp:Label></h3>
                <div class="pull-right box-tools">
                    <asp:HyperLink runat="server" ID="btn_export" Text="Export" CssClass="btn btn-primary btn-sm pull-right margin-r-5"></asp:HyperLink>                    
                    <asp:HyperLink runat="server" ID="btn_detail" Text="Export Details" CssClass="btn btn-primary btn-sm pull-right margin-r-5"></asp:HyperLink>
                    
                </div>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="form-group row">
                         <div class="form-group row">
                          <div class="col-sm-12 text-right">
                              <hr />
                          </div>
                        </div>

                         <div class="form-group row">
                            <div class="col-sm-2 text-right">
                                <asp:Label runat="server" ID="lblt_TypeSurvey" CssClass="control-label text-bold"  >Type of Survey</asp:Label>                               
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_TypeSurvey" 
                                    runat="server"                                      
                                    Width="300px" 
                                    OnClientSelectedIndexChanged="TypeSurveyCHG"  >
                                </telerik:RadComboBox>                                
                            </div>
                            <div class="col-sm-1 text-right">
                                <asp:Label runat="server" ID="lblt_TypeAssessment" CssClass="control-label text-bold">Report Type</asp:Label>
                            </div>
                            <div class="col-sm-5">
                               <%--  <asp:RadioButtonList ID="rd_TypeAssessmemt" runat="server" OnSelectedIndexChanged="changeReportType" >
                                      <asp:ListItem Text="Base Line" Value="1"></asp:ListItem>
                                      <asp:ListItem Text="End Line" Value="2"></asp:ListItem>
                                 </asp:RadioButtonList>--%>
                            
                                <asp:CheckBox ID="chkBaseLine" runat="server" Text="Base Line" OnClick="changeReportType();" Checked="true"    /><br />
                                <asp:CheckBox ID="chkEndLine" runat="server" Text="End Line" OnClick="changeReportType();"    />                               

                            </div>                                                                                      
                        </div>    

                        <div class="form-group row">
                            <div class="col-sm-2 text-right">
                                <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Region</asp:Label>
                                <asp:HiddenField runat="server" ID="hdnPeople" />
                                <asp:HiddenField runat="server" ID="hdnManInfo" />
                                <asp:HiddenField runat="server" ID="hdnWomanInfo" />
                                <asp:HiddenField runat="server" ID="hdnStarInfo" />
                                <asp:HiddenField runat="server" ID="hdnClassLevel" />
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" OnClientSelectedIndexChanged="regionCHG.ExtraParams(1)"    ></telerik:RadComboBox> 
                                <asp:CheckBox ID="chk_TodosR" runat="server" Text="All" OnClick="changeFilterAll(1);"    />
                            </div>
                            <div class="col-sm-1 text-right">
                                <asp:Label runat="server" ID="lbl_typeOrg" CssClass="control-label text-bold">School Type</asp:Label>
                            </div>
                            <div class="col-sm-5">
                                <telerik:RadComboBox ID="cmb_typeOrg" runat="server" Width="300px" Filter="Contains"  OnClientSelectedIndexChanged="regionCHG.ExtraParams(4)"    ></telerik:RadComboBox> 
                                <asp:CheckBox ID="chk_typeOrg" runat="server" Text="All"  OnClick="changeFilterAll(4);"    />
                            </div>                                                                                      
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2 text-right">
                                <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub Region</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px"  OnClientSelectedIndexChanged="regionCHG.ExtraParams(2)"    ></telerik:RadComboBox> 
                                <asp:CheckBox ID="chk_TodosSub" runat="server" Text="All"  OnClick="changeFilterAll(2);"    />
                            </div>

                             <div class="col-sm-1 text-right">
                                <asp:Label runat="server" ID="lblt_school" CssClass="control-label text-bold">School</asp:Label>
                            </div>
                            <div class="col-sm-5">
                                <telerik:RadComboBox ID="cmb_school" runat="server" Width="300px" Filter="Contains"  OnClientSelectedIndexChanged="regionCHG.ExtraParams(5)"    ></telerik:RadComboBox> 
                                <asp:CheckBox ID="chk_school" runat="server" Text="All"  OnClick="changeFilterAll(5);"    />
                            </div>

                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2 text-right">
                                <asp:Label runat="server" ID="lblt_district" CssClass="control-label text-bold">District</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_district" runat="server" Width="300px"  OnClientSelectedIndexChanged="regionCHG.ExtraParams(3)"    ></telerik:RadComboBox>  
                                <asp:CheckBox ID="chk_district" runat="server" Text="All"  OnClick="changeFilterAll(3);"    /><br /><br />                                
                                <button class="btn btn-success btn-sm pull-left" onclick="loadchart('From Refresh button');return false"><i class="fa fa-refresh"></i> Refresh results</button>
                            </div>
                            <div class="col-sm-6">                                 
                                  <div class="box-body">
                                       <div class="box-group" id="accordion">
                                            <div class="panel box box-danger" style="border-top: 2px solid red;">
                                                  <div class="box-header with-border">
                                                    <h5>
                                                      <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="text-red">
                                                        Showing up assessment's result By:&nbsp;&nbsp;...&nbsp;&nbsp;<i class="fa fa-arrow-circle-down fa-sm"></i>
                                                      </a>
                                                    </h5>
                                                  </div>
                                                  <div id="collapseTwo" class="panel-collapse collapse">
                                                      <div id="Filter_Options" class="text-red">...</div>
                                                  </div>
                                                </div>
                                        </div>
                                    </div>
                            </div>                         
                        </div>                                             
                        <div class="form-group row">                          
                              <hr />                          
                        </div>                                                                
                    </div>
                     <div class="form-group row">
                         <div class="col-sm-12">  

                                <ul class="nav nav-tabs">
                                    <li class="active"><a data-toggle="tab" href="#Start">BaseLine/EndLine Results</a></li>
                                    <li><a data-toggle="tab" href="#People">Students</a></li>
                                    <li><a data-toggle="tab" href="#Gender">Gender</a></li>
                                    <li><a data-toggle="tab" href="#Class">Class Level</a></li>
                                 </ul>
                                 <div class="tab-content">
                                         <div id="Start" class="tab-pane fade in active">
                                              <div class="form-group row">
                                                 <div class="col-sm-9" id="start_char_container"  style="margin:20px 0px 0px 0px;">        
                                                     <div id="star-chart" style=" width:750px; " ></div>                                           
                                                 </div>
                                                 <div class="col-sm-3" style="margin:130px 0px 0px 0px; ">     
                                                        <div class="info-box ">
                                                            <span class="info-box-icon"><img class="img-responsive" src="../Imagenes/iconos/school_.png" ></span>
                                                            <div class="info-box-content">
                                                                <span class="info-box-text">
                                                                   Schools
                                                                </span>
                                                                <span id="schoolNumber" class="info-box-number">
                                                                    0
                                                                </span>
                                                            </div>
                                                            <!-- /.info-box-content -->
                                                        </div>
                                                     <br /><br /><br />
                                                         <div class="info-box ">
                                                            <span class="info-box-icon"><img class="img-responsive" src="../Imagenes/iconos/checkbook_.png" ></span>
                                                            <div class="info-box-content">
                                                                <span class="info-box-text">
                                                                   Assessments
                                                                </span>
                                                                <span id="AssessmentNumber" class="info-box-number">
                                                                    0
                                                                </span>
                                                            </div>
                                                            <!-- /.info-box-content -->
                                                        </div>
                                                       <br /><br /><br />
                                                         <div class="info-box ">
                                                            <span class="info-box-icon"><img class="img-responsive" src="../Imagenes/iconos/check_1.png" ></span>
                                                            <div class="info-box-content">
                                                                <span class="info-box-text">
                                                                   BaseLine Assessments
                                                                </span>
                                                                <span  Id = "BaseLineNumber" class="info-box-number">
                                                                    0
                                                                </span>
                                                            </div>
                                                            <!-- /.info-box-content -->
                                                        </div>
                                                       <br /><br /><br />
                                                         <div class="info-box ">
                                                            <span class="info-box-icon"><img class="img-responsive" src="../Imagenes/iconos/check_2.png" ></span>
                                                            <div class="info-box-content">
                                                                <span class="info-box-text">
                                                                   EndLine Assessments
                                                                </span>
                                                                <span Id ="EndLineNumber" class="info-box-number">
                                                                    0
                                                                </span>
                                                            </div>
                                                            <!-- /.info-box-content -->
                                                        </div>
                                                       
                                                 </div>
                                               </div> 
                                         </div>
                                         <div id="People" class="tab-pane fade">    
                                              <div class="form-group row">
                                                 <div class="col-sm-7">                                          
                                                    <div id="containerPie" style="height: 500px;  margin: 20px 0px 0px 0px" ></div>
                                                 </div>
                                                 <div class="col-md-2 text-center" style="margin:130px 0px 0px 0px; "> 
                                                    <div class="text-maroon" style="width:140px; height:140px;">                                                         
                                                       <h1 class="no-padding no-margin icon-h1"><i class="fa fa-female fa-2x" aria-hidden="true"></i></h1>
                                                     </div><br />
                                                     <div class="text-blue" style="width:140px; height:140px;">                                                         
                                                       <h1 class="no-padding no-margin icon-h1"><i class="fa fa-male fa-2x" aria-hidden="true"></i></h1>
                                                     </div>
                                                </div>
                                                <div class="col-md-2" style="margin: 100px 0px 0px 0px;">                                                                                                                                                             
                                                    <input type="text" class="Ngirls" value="0" data-width="140" data-height="140" data-thickness="0.30" data-fgColor="#D81B60" data-skin="tron" readonly>
                                                    <br /><br /><input type="text" class="Nboys" value="0" data-width="140" data-height="140" data-thickness="0.30" data-fgColor="#3C8DBC" data-skin="tron" readonly>
                                                </div>                                                
                                              </div>
                                         </div>
                                         <div id="Gender" class="tab-pane fade">                                            
                                             <div id="container" style="width:75%; margin: 20px 0px 0px 0px" ></div>
                                         </div>
                                         <div id="Class" class="tab-pane fade">                                            
                                             <div id="divClassLevel" style="width:75%; margin: 20px 0px 0px 0px"></div>                       
                                         </div>
                                  </div>
                         </div>
                     </div>
                  <%--  <div class="form-group row">
                        <div id="star-chart_OLD" style="height: 500px; margin: 0 auto" class="col-sm-6"></div>
                        <div id="containerPie_OLD" style="height: 500px; margin: 0 auto" class="col-sm-6"></div>
                    </div>
                    <div class="form-group row">
                        <div id="container_OLD" style="height: 500px; margin: 0 auto" class="col-sm-12"></div>
                    </div> --%>

                   <%-- <div class="form-group row">
                        <div id="divClassLevel_OLD" style="height: 500px; margin: 0 auto" class="col-sm-12"></div>                       
                    </div>--%>

              </div>
               <div id="LoadScreen" class="modal" >
                    <asp:Image ID="Image1" runat="server" CssClass="loadingGif"  ImageUrl="~/Imagenes/iconos/Loading.gif" />
               </div>
               

        </div>
        </div> 
        
          
         <script type="text/javascript">

                            //var prm = Sys.WebForms.PageRequestManager.getInstance();
                            var Populating = false;

                           <%-- prm.add_endRequest(function (s, e) {
                                                                
                                loadchart('From End Request');
                                //var valor = [<%= Me.people%>];
                                //var jsonStarData = JSON.parse($("#MainContent_jsonBenByFilter").val())
                                                                
                            });--%>

                            $(document).ready(function () {

                                loadchart('From Document Ready');

                                <%--var BaseLine = get_ReportType();
                                var comboType = $find("<%=cmb_TypeSurvey.ClientID%>");                                
                                var idTypeSurvey = comboType.get_selectedItem().get_value();
                                $("#<%= btn_export.ClientID%>").prop("href", "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&flgTR=" + BaseLine + "&flgTA=" + idTypeSurvey);
                                $("#<%= btn_detail.ClientID%>").prop("href", "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&flgTR=" + BaseLine + "&flgTA=" + idTypeSurvey);--%>
                        
                            });

             function TypeSurveyCHG() {

                    //var BaseLine = $("# %=rd_TypeAssessmemt.ClientID%>").find(":checked").val();
                    var BaseLine = get_ReportType();                                 
            
                    loadchart('From Type of assesment');

                }


             

function regionCHG(sender, eventArgs, Params) {

    //alert('Populating: ' + Populating);

    if (!Populating) {
        //var item = eventArgs.get_item();
        //var clientId = sender.get_id();
        //alert('item: ' + item + ' ID: ' + clientId + ' idLevel: ' + Params[0]);

        Populating = true;
        var idLevel = Params[0];

          var comboType = $find("<%=cmb_TypeSurvey.ClientID%>");
          var idTypeSurvey = comboType.get_selectedItem().get_value();
        
        var comboRegion = $find('<%= cmb_region.Clientid %>');
        var idRegion = comboRegion.get_selectedItem().get_value();
        //var idRegion = eventArgs.get_item().get_value();
        var comboSubregion = $find('<%= cmb_subregion.Clientid %>');
        var comboDistrict = $find('<%=cmb_district.ClientID %>');
        var idDistrict = comboDistrict.get_selectedItem().get_value();
        var comboTypeOrg = $find('<%=cmb_typeOrg.ClientID %>');
        //comboTypeOrg.get_selectedItem().get_text();    
        var idTypeOrg = comboTypeOrg.get_selectedItem().get_value()
        var comboSchool = $find('<%=cmb_school.ClientID %>');
        //comboSchool.get_selectedItem().get_text();    
        //var idSchool = comboSchool.get_selectedItem().get_value();                                 
        //alert('level: ' + idLevel + ' Region: ' + idRegion + ' District: ' + idDistrict);
        var jsonResult;
        //get_DataINFO(ByVal IDLevel As Integer, ByVal IdRegion As Integer, ByVal idDistrict As Integer) As Object

        if (idLevel <= 4) {

            $.ajax({
                type: "POST",
                url: "frm_measurement_dashboard.aspx/get_DataINFO",
                data: '{IDLevel:' + idLevel + ', IdRegion:' + idRegion + ', idDistrict:' + idDistrict + ', idTypeOrg:' + idTypeOrg + ', idTypeSurvey:' + idTypeSurvey  + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    jsonResult = data.d;
                    //alert('Result ' + jsonResult);
                    var jsonResultARR = jsonResult.split('||');
                    //'JsonSubRegion
                    //'JsonDistrict
                    //'JsonOrgTypes
                    //'JsonSchools

                    if (jsonResultARR[0] != '[]' && idLevel <= 1) {//Suregion
                        fillCombo(comboSubregion, jsonResultARR[0]);
                    }

                    if (jsonResultARR[1] != '[]' && idLevel <= 2) {//District
                        fillCombo(comboDistrict, jsonResultARR[1]);
                    }

                    if (jsonResultARR[2] != '[]' && idLevel <= 3) {//OrgType
                        fillCombo(comboTypeOrg, jsonResultARR[2]);
                    }

                    if (jsonResultARR[3] != '[]' && idLevel <= 4) {//JsonSchools
                        //alert(jsonResultARR[3]);
                        fillCombo(comboSchool, jsonResultARR[3]);
                    }

                    //alert('BaseLine: ' + jsonStarFilterARR[0]);
                    //alert('EndLine: ' + jsonStarFilterARR[1]);

                    //if (jsonStarFilterARR[1] == '[]') {
                    //   } else {
                    //    alert('BaseLine: ' + jsonStarFilterARR[0]);
                    //    alert('EndLine: ' + jsonStarFilterARR[1]);
                    //    loadchartStar(JSON.parse(jsonStarFilterARR[0]), JSON.parse(jsonStarFilterARR[1]));
                    //}

                    //JSonData = jQuery.parseJSON(data.d);
                    //alert(JSonData);

                    Populating = false;
                    loadchart('Populating Realoading');

                },
                failure: function (response) {
                    alert('Error Graphing: ' + response.d);
                }
            });


        } else {

            Populating = false;
            loadchart('Populating with Not realoading');

        }


        //Populating = false;


    } //IF Populating


}




         

function changeFilterAll(idLevel) {

    if (idLevel == 1) {

        if ($('#<%=chk_TodosR.ClientID %>').is(":checked")) {
            $('#<%=chk_TodosSub.ClientID %>').prop('checked', true);
            $('#<%=chk_district.ClientID %>').prop('checked', true);
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', true);
            $('#<%=chk_school.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chk_TodosSub.ClientID %>').prop('checked', false);
            $('#<%=chk_district.ClientID %>').prop('checked', false);
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', false);
            $('#<%=chk_school.ClientID %>').prop('checked', false);
        }

    } else if (idLevel == 2) {

        if ($('#<%=chk_TodosSub.ClientID %>').is(":checked")) {
            $('#<%=chk_district.ClientID %>').prop('checked', true);
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', true);
            $('#<%=chk_school.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chk_district.ClientID %>').prop('checked', false);
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', false);
            $('#<%=chk_school.ClientID %>').prop('checked', false);
        }

    } else if (idLevel == 3) {

        if ($('#<%=chk_district.ClientID%>').is(":checked")) {
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', true);
            $('#<%=chk_school.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chk_typeOrg.ClientID %>').prop('checked', false);
            $('#<%=chk_school.ClientID %>').prop('checked', false);
        }

    } else if (idLevel == 4) {

        if ($('#<%=chk_typeOrg.ClientID %>').is(":checked")) {
            $('#<%=chk_school.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chk_school.ClientID %>').prop('checked', false);
        }

    }

    loadchart('From filter All changes');

}



function get_ReportType() {

    var result = 0;

    if ($('#<%=chkBaseLine.ClientID %>').is(":checked")) {

        if ($('#<%=chkEndLine.ClientID %>').is(":checked"))
            result = 3; //Base line and End Line
        else
            result = 1; //Base line

    } else if ($('#<%=chkEndLine.ClientID %>').is(":checked")) {
        result = 2; //End line
    }

    //alert('Check Result: ' + result);

    return result;

}


function changeReportType() {

    var BaseLine = get_ReportType();

<%--    var comboType = $find("<%=cmb_TypeSurvey.ClientID%>");
    var idTypeSurvey = comboType.get_selectedItem().get_value();--%>
       
    loadchart('Change from type report');

}

function loadchart(SourceRequested) {

    //alert(SourceRequested);
    $("#LoadScreen").show();

    var comboType = $find("<%=cmb_TypeSurvey.ClientID%>");
    //comboboxType.get_selectedItem().get_text();   
    // alert(comboType);
    var idTypeSurvey = comboType.get_selectedItem().get_value();

    var BaseLine;
    var All_Region = false;
    var idRegion;
    var All_District = false;
    var idDistrict;
    var All_TypeOrg = false;
    var idTypeOrg;
    var All_School = false;
    var idSchool;

    var strFilter = '<ul>';
    var strReportFilter = '';
    strFilter = strFilter + '<li>The Assessment tool: ' + comboType.get_selectedItem().get_text() + '</li>';
    strReportFilter = strReportFilter + 'flgTA=' + idTypeSurvey;

    // $("# %=rd_TypeAssessmemt.ClientID%>").find(":checked").val();                                
    var BaseLine = get_ReportType();
    if (BaseLine == 1) {
        strFilter = strFilter + '<li>The Report Type: Base Line</li>';        
    } else if (BaseLine == 2) {
        strFilter = strFilter + '<li>The Report Type: End Line</li>';        
    } else if (BaseLine == 3) {
        strFilter = strFilter + '<li>The Report Type: Base Line and End Line</li>';
    } else {
        strFilter = strFilter + '<li>The Report Type: No Report selected</li>';
    }

    strReportFilter = strReportFilter + '&flgTR=' + BaseLine;

    var comboRegion = $find('<%=cmb_region.ClientID %>');
    //comboRegion.get_selectedItem().get_text();    
    idRegion = comboRegion.get_selectedItem().get_value();
    if ($('#<%=chk_TodosR.ClientID %>').is(":checked")) {
        All_Region = true;
        strFilter = strFilter + '<li>All Regions</li>';
        strReportFilter = strReportFilter + '&rAll=1';
    } else {
        strFilter = strFilter + '<li>Region: ' + comboRegion.get_selectedItem().get_text() + '</li>';
        strReportFilter = strReportFilter + '&rAll=' + idRegion;
    }


    var comboDistrict = $find('<%=cmb_district.ClientID %>');
    //comboDistrict.get_selectedItem().get_text();    
    var idDistrict = comboDistrict.get_selectedItem().get_value();
    var All_District = false;
    if ($('#<%=chk_district.ClientID %>').is(":checked")) {
        All_District = true;
        strFilter = strFilter + '<li>All Districts</li>';
        strReportFilter = strReportFilter + '&dAll=1';
    } else {
        strFilter = strFilter + '<li>District: ' + comboDistrict.get_selectedItem().get_text() + '</li>';
        strReportFilter = strReportFilter + '&dAll=' + idDistrict;
    }

    var comboTypeOrg = $find('<%=cmb_typeOrg.ClientID %>');
    //comboTypeOrg.get_selectedItem().get_text();    
    var idTypeOrg = comboTypeOrg.get_selectedItem().get_value();
    var All_TypeOrg = false;

    if ($('#<%=chk_typeOrg.ClientID %>').is(":checked")) {
        All_TypeOrg = true;
        strFilter = strFilter + '<li>All kind of schools</li>';
        strReportFilter = strReportFilter + '&tOrg=1';
    } else {
        strFilter = strFilter + '<li>School Type: ' + comboTypeOrg.get_selectedItem().get_text() + '</li>';
        strReportFilter = strReportFilter + '&tOrg=' + idTypeOrg;
    }

    var comboSchool = $find('<%=cmb_school.ClientID %>');
    //comboSchool.get_selectedItem().get_text();    
    var idSchool = comboSchool.get_selectedItem().get_value();
    var All_School = false;
    if ($('#<%=chk_school.ClientID %>').is(":checked")) {
        All_School = true;
        strFilter = strFilter + '<li>All Schools included</li>';
        strReportFilter = strReportFilter + '&idOrg=1';
    } else {
        strFilter = strFilter + '<li>The School: ' + comboSchool.get_selectedItem().get_text() + '</li>';
        strReportFilter = strReportFilter + '&idOrg=' + idSchool;
    }

    strFilter = strFilter + '</ul>';
    $("#Filter_Options").html(strFilter);

    //alert(strReportFilter);

    $("#<%=btn_export.ClientID%>").prop("href", "../Reportes/Instrumentos/frm_reporteSkillsAssesment?" + strReportFilter);
    $("#<%=btn_detail.ClientID%>").prop("href", "../Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?" + strReportFilter);
    
    //alert('Type of Survey:' + idTypeSurvey + ',   Baseline:' + BaseLine + ',   All_Region:' + All_Region + ',   idRegion:' + idRegion + ',  All_District:' + All_District + ',  idDistrict:' + idDistrict + ',  All_TypeOrg:' + All_TypeOrg + ',  idTypeOrg:' + idTypeOrg + ',  All_School:' + All_School + ', idSchool:' + idSchool);
    //alert('{idTypeSurvey:' + idTypeSurvey + ', BaseLine:' + BaseLine + ', IdRegion:' + idRegion + ', RegionAll:"' + All_Region + '", idDistrict:' + idDistrict + ', DistrictALL:"' + All_District + '", idTypeOrg:' + idTypeOrg + ', TypeOrganizationALL:"' + All_TypeOrg + '", idSchool:' + idSchool + ', TypeSchoolALL:"' + All_School + '"}');
    
    var JSonData;
    var jsonStarFilter;
    var idGraph = 1; //Star

    $.ajax({
        type: "POST",
        url: "frm_measurement_dashboard.aspx/web_filtroStar",
        data: '{IDgraph:' + idGraph + ', idTypeSurvey:' + idTypeSurvey + ', BaseLine:' + BaseLine + ', IdRegion:' + idRegion + ', RegionAll:"' + All_Region + '", idDistrict:' + idDistrict + ', DistrictALL:"' + All_District + '", idTypeOrg:' + idTypeOrg + ', TypeOrganizationALL:"' + All_TypeOrg + '", idSchool:' + idSchool + ', TypeSchoolALL:"' + All_School + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            jsonStarFilter = data.d;

            // alert('Result ' + jsonStarFilter);

            var jsonStarFilterARR = jsonStarFilter.split('||');
            var emptyVal = '[{"name":"Higher Order Thinking","y":0},{"name":"","y":0},{"name":"Positive Self-Concept","y":0},{"name":"","y":0},{"name":"Self-Control","y":0},{"name":"","y":0},{"name":"Communication","y":0},{"name":"","y":0},{"name":"Social","y":0},{"name":"","y":0}]';

            if (jsonStarFilterARR[0] == '[]' || jsonStarFilterARR[0] == undefined) { //BaseLine
                jsonStarFilterARR[0] = emptyVal;
            }

            if (jsonStarFilterARR[1] == '[]' || jsonStarFilterARR[1] == undefined) {  //EndLine
                jsonStarFilterARR[1] = emptyVal;
            }

            if (jsonStarFilterARR[10] == '[]' || jsonStarFilterARR[10] == undefined) {  //EndLine
                jsonStarFilterARR[10] = emptyVal;
            }

            //alert('BaseLine: ' + jsonStarFilterARR[0]);
            //alert('EndLine: ' + jsonStarFilterARR[1]);

            //alert('Optimal: ' + jsonStarFilterARR[10]);

            //*************************START CHART*********************
            loadchartStar(JSON.parse(jsonStarFilterARR[0]), JSON.parse(jsonStarFilterARR[1]), JSON.parse(jsonStarFilterARR[10]));

            //*************************PIE CHART*********************
            if (jsonStarFilterARR[2] == '[]' || jsonStarFilterARR[2] == undefined) { //People
                jsonStarFilterARR[2] = '[]';
            }

            loadChartPie(jsonStarFilterARR[2]);

            
            //*************************GENDER CHART*********************
            if (jsonStarFilterARR[3] == '[]' || jsonStarFilterARR[3] == undefined) { //Men
                jsonStarFilterARR[3] = '[]';
            }
            if (jsonStarFilterARR[4] == '[]' || jsonStarFilterARR[4] == undefined) { //Women
                jsonStarFilterARR[4] = '[]';
            }
            //alert('Men:' + jsonStarFilterARR[3]);
            //alert('Women' + jsonStarFilterARR[4]);
            loadGenderGraph(jsonStarFilterARR[3], jsonStarFilterARR[4]);

            // alert('Men: ' + jsonStarFilterARR[3] + ' Women: ' + jsonStarFilterARR[4]);
            //alert('Women' + jsonStarFilterARR[4]);
                        

            //*************************CLASS CHART*********************
            if (jsonStarFilterARR[5] == '[]' || jsonStarFilterARR[5] == undefined) { //Class
                jsonStarFilterARR[5] = '[]';
            }

            if (jsonStarFilterARR[11] == '[]' || jsonStarFilterARR[11] == undefined) { //Class
                jsonStarFilterARR[11] = '["Primary 4", "Primary 5", "Primary 6"]';
            }

            //alert(jsonStarFilterARR[11]);
            loadClassLevelGraph(jsonStarFilterARR[5], jsonStarFilterARR[11]);


            //*****************************PEOPLE********************************************

            var jsonPeople = eval("([" + jsonStarFilterARR[2] + "])");


            var nBoys = 0;
            var nGirls = 0;

            if (jsonPeople[0] != undefined) {
                nBoys = jsonPeople[0].y;
            } else {
                nBoys = 0;
            }

            if (jsonPeople[1] != undefined) {
                nGirls = jsonPeople[1].y;
            } else {
                nGirls = 0;
            }

            if (nBoys == 0)
                PorcenBoys = 0;
            else
                PorcenBoys = Math.round((nBoys / (nGirls + nBoys)) * 100);

            if (nGirls == 0)
                PorcenGirls = 0;
            else
                PorcenGirls = Math.round((nGirls / (nGirls + nBoys)) * 100);

            if (!isNaN(PorcenBoys))
                $('.Nboys').val(PorcenBoys);
            else
                $('.Nboys').val(0);

            $('.Nboys').trigger('change');
            $('.Nboys').css('font-size', '20px');
            $(".Nboys").knob({
                'format': function (value) {
                    return value + '%';
                }
            });

            if (!isNaN(PorcenGirls))
                $('.Ngirls').val(PorcenGirls);
            else
                $('.Ngirls').val(0);

            $('.Ngirls').trigger('change');
            $(".Ngirls").css('font-size', '20px');
            $(".Ngirls").knob({
                'format': function (value) {
                    return value + '%';
                }
            });
            //*****************************PEOPLE********************************************

            //*****************************OTHER INFO********************************************
            //alert('School: ' + jsonStarFilterARR[6] + ' Assessment: ' + jsonStarFilterARR[7] + ' BaseLine: ' + jsonStarFilterARR[8] + ' Endline: ' + jsonStarFilterARR[9]);
            if (jsonStarFilterARR[6] != undefined) { //Class
                $('#schoolNumber').text(jsonStarFilterARR[6]);
            } else {
                $('#schoolNumber').text(0);
            }

            if (jsonStarFilterARR[7] != undefined) { //Class
                $('#AssessmentNumber').html('').append(jsonStarFilterARR[7]);
            } else {
                $('#AssessmentNumber').html('').append(0);
            }

            if (jsonStarFilterARR[8] != undefined) { //Class
                $('#BaseLineNumber').html('').append(jsonStarFilterARR[8]);
            } else {
                $('#BaseLineNumber').html('').append(0);
            }

            if (jsonStarFilterARR[9] != undefined) { //Class
                $('#EndLineNumber').html('').append(jsonStarFilterARR[9]);
            } else {
                $('#EndLineNumber').html('').append(0);
            }

            //*****************************OTHER INFO********************************************
            
            $("#LoadScreen").hide();

        },
        failure: function (response) {
            alert('Error Graphing: ' + response.d);
            $("#LoadScreen").hide();
        }      

    });


    // idGraph = 2; //People


    //$.ajax({
    //    type: "POST",
    //    url: "frm_measurement_dashboard.aspx/web_filtroStar",
    //    data: '{IDgraph:' + idGraph + ', idTypeSurvey:' + idTypeSurvey + ', BaseLine:' + BaseLine + ', IdRegion:' + idRegion + ', RegionAll:"' + All_Region + '", idDistrict:' + idDistrict + ', DistrictALL:"' + All_District + '", idTypeOrg:' + idTypeOrg + ', TypeOrganizationALL:"' + All_TypeOrg + '", idSchool:' + idSchool + ', TypeSchoolALL:"' + All_School + '"}',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        jsonStarFilter = data.d;
    //        //alert(jsonStarFilter);
    //        loadChartPie(jsonStarFilter);                                       
    //    },
    //    failure: function (response) {
    //        alert('Error Graphing: ' + response.d);
    //    }
    //});

    //idGraph = 3; //People

    //$.ajax({
    //    type: "POST",
    //    url: "frm_measurement_dashboard.aspx/web_filtroStar",
    //    data: '{IDgraph:' + idGraph + ', idTypeSurvey:' + idTypeSurvey + ', BaseLine:' + BaseLine + ', IdRegion:' + idRegion + ', RegionAll:"' + All_Region + '", idDistrict:' + idDistrict + ', DistrictALL:"' + All_District + '", idTypeOrg:' + idTypeOrg + ', TypeOrganizationALL:"' + All_TypeOrg + '", idSchool:' + idSchool + ', TypeSchoolALL:"' + All_School + '"}',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        jsonStarFilter = data.d;
    //        //alert(jsonStarFilter);
    //        loadGenderGraph(jsonStarFilter);
    //    },
    //    failure: function (response) {
    //        alert('Error Graphing: ' + response.d);
    //    }
    //});


    //idGraph = 4; //Class LEvel

    //$.ajax({
    //    type: "POST",
    //    url: "frm_measurement_dashboard.aspx/web_filtroStar",
    //    data: '{IDgraph:' + idGraph + ', idTypeSurvey:' + idTypeSurvey + ', BaseLine:' + BaseLine + ', IdRegion:' + idRegion + ', RegionAll:"' + All_Region + '", idDistrict:' + idDistrict + ', DistrictALL:"' + All_District + '", idTypeOrg:' + idTypeOrg + ', TypeOrganizationALL:"' + All_TypeOrg + '", idSchool:' + idSchool + ', TypeSchoolALL:"' + All_School + '"}',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        jsonStarFilter = data.d;
    //        //alert(jsonStarFilter);
    //        loadClassLevelGraph(jsonStarFilter);
    //    },
    //    failure: function (response) {
    //        alert('Error Graphing: ' + response.d);
    //    }
    //});


    //var strData = JSON.parse('[{"type" : "column", "name": "Higher Order Thinking", "data":[7.47,8,8.04]},{ "type" : "column", "name": "Positive Self-Concept", "data":[8.38,8.21,8.03]},{ "type" : "column", "name": "Self-Control", "data":[9.54,6.88,9.32]},{ "type" : "column", "name": "Communication", "data":[6,6,6]},{"type" : "column", "name": "Social", "data":[10.14,10.36,9.34]},{"type" : "spline", "name": "Optimal Values Higher Order Thinking", "data":[11,11,11]},{"type" : "spline", "name": "Optimal Values Positive Self-Concept", "data":[11,11,11], "marker": { "lineWidth": 2,  "lineColor": Highcharts.getOptions().colors[3], "fillColor": "white"} }]');

    //Highcharts.chart('divClassLevel_OLD', {
    //    title: {
    //        text: 'Score based on class level'
    //    },
    //    credits: false,
    //    xAxis: {
    //        categories: ['Primary 4', 'Primary 5', 'Primary 6']
    //    },
    //    series: strData
    // });


}      
     

             function fillCombo(combo, result) {

                 combo.clearItems();

                 //var items = result.d || result;
                 //alert('items:' + items + ' lenght:' + items.length);
                 data = jQuery.parseJSON(result);
                 //alert('items:' + data + ' lenght:' + data.length);

                 // This just lets your users know that nothing was returned with their search 
                 if (data.length == 0) {
                     var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                     comboItem.set_text("No records found");
                     comboItem.set_value("0");
                     combo.get_items().add(comboItem);
                     //combo.clearSelection();
                 }

                 for (var i = 0; i < data.length; i++) {

                     var item = data[i];
                     //alert('item'+ i + ': ' + item.Text + ' ' + item.Value);                                                       
                     var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                     comboItem.set_text(item.Text);
                     comboItem.set_value(item.Value);

                     //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                     //alert('Combo Text: ' + comboItem.get_text() + ' Combo Value: ' + comboItem.get_value());

                     //combo.trackChanges();
                     //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                     combo.get_items().add(comboItem);
                     //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                     //combo.commitChanges();
                     //alert('item' + i + ': ' + item.Text + ' ' + item.Value);
                 }

                 combo.clearSelection();
                 combo.trackChanges();
                 combo.get_items().getItem(0).select();
                 combo.updateClientState();
                 combo.commitChanges();

             }


             Function.prototype.ExtraParams = function () {
                 var method = this, args = Array.prototype.slice.call(arguments);
                 return function () {
                     return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
                 };
             };



</script>

<style>

                            /* The Modal (background) */
                            .modal_load {
                                display: none; /* Hidden by default */
                                position: fixed; /* Stay in place */
                                z-index: 1; /* Sit on top */
                                padding-top: 100px; /* Location of the box */
                                left: 0;
                                top: 0;
                                width: 100%; /* Full width */
                                height: 100%; /* Full height */
                                overflow: auto; /* Enable scroll if needed */
                                background-color: rgb(128, 128, 128); /* Fallback color */
                                background-color: rgba(195, 66, 66, 0.20); /* Black w/ opacity */
                            }


</style>

    </section>
</asp:Content>

