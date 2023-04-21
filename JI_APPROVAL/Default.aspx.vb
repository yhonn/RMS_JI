Imports ly_SIME
Imports ly_APPROVAL
Imports ly_RMS
Imports DotNet.Highcharts.Options
Imports DotNet.Highcharts.Enums
Imports DotNet.Highcharts
Imports System.Drawing
Imports System.Globalization

Public Class _Default
    Inherits Page
    Dim cl_user As ly_SIME.CORE.cls_user

    Dim frmCODE As String = "AP_DEFAULT"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cl_Dashboard As APPROVAL.clss_dashboard
    Dim clss_approval As APPROVAL.clss_approval
    Public Property urlSys As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
            'If Not IsPostBack Then
            '    'loadData(cl_user)
            '    'get_chart()
            '    'get_chartIndicadores()
            'End If
        End If

        If Not IsPostBack Then

            cl_Dashboard = New APPROVAL.clss_dashboard(Me.Session("E_IDPrograma"))
            Session.Item("cl_Dashboard") = cl_Dashboard
            Session("N_app_procc") = cl_Dashboard.get_Approvals_Count(Me.Session("E_IdUser").ToString)
            Session("N_app_pending_procc") = cl_Dashboard.get_Pending_Approvals_Count(Me.Session("E_IdUser").ToString)

            'Dim Sys_url As String = Request.Url.GetLeftPart(UriPartial.Authority)
            Dim app_path = HttpContext.Current.Request.ApplicationPath
            urlSys = Request.Url.GetLeftPart(UriPartial.Authority) & app_path
            ' Sys_url.Substring(0, Strings.InStr(Sys_url, "/"))

            Dim tbl_unit As DataTable = set_Unit(cl_Dashboard.get_average_timing(Me.Session("E_IdUser").ToString))
            Dim val As Double = 0

            Dim tbl_user_role As New DataTable
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            '*********************** All Roles (Simple, Shared, Groups)***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 0)
            Dim strRoles As String = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_ALL_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** All Roles (Simple, Shared, Groups)***********************************


            '*********************** Roles (Simple and Shared)***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 1)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** Roles  (Simple and Shared)***********************************

            '***********************Group Roles***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 2)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_GroupRolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** Roles Roles***********************************

            '*********************** All Group Roles Just Simple Roles*******************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 3)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_ALL_SIMPLE_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** All Roles*******************************************************

            'Dim minutes As Double = cl_Dashboard.get_average_timing
            'Dim hours As Double = minutes / 60
            'Dim Days As Double = hours / 24
            'Dim Months As Double = Days / 30
            'Dim Year As Double = Months / 12
            'Dim value As Double = 0
            'Dim unit As String = ""

            'If hours < 1 Then
            '    value = minutes
            '    unit = "min"
            'ElseIf Days < 1 Then
            '    value = hours
            '    unit = "hrs"
            'ElseIf Months < 1 Then
            '    value = Days
            '    unit = "day"
            'ElseIf Year < 1 Then
            '    value = Months
            '    unit = "mon"
            'Else
            '    value = Year
            '    unit = "yrs"
            'End If

            'Session("AVG_app_timing") = Math.Round(value, 3).ToString(cl_user.regionalizacionCulture)
            'Session("AVG_app_unit") = Unit

            If Not IsNothing(tbl_unit) Then
                If tbl_unit.Rows.Count > 0 Then
                    val = CType(tbl_unit.Rows.Item(0).Item("value"), Double)
                    Session("AVG_app_timing") = val.ToString(cl_user.regionalizacionCulture)
                    Session("AVG_app_unit") = tbl_unit.Rows.Item(0).Item("unit")
                Else
                    Session("AVG_app_timing") = 0
                    Session("AVG_app_unit") = "--"
                End If
            Else
                Session("AVG_app_timing") = 0
                Session("AVG_app_unit") = "--"
            End If


            tbl_unit = set_Unit(cl_Dashboard.get_MAX_timing(Me.Session("E_IdUser").ToString))


            'minutes = cl_Dashboard.get_MAX_timing
            'hours = minutes / 60
            'Days = hours / 24
            'Months = Days / 30
            'Year = Months / 12
            'value = 0
            'unit = ""

            'If hours < 1 Then
            '    value = minutes
            '    unit = "min"
            'ElseIf Days < 1 Then
            '    value = hours
            '    unit = "hrs"
            'ElseIf Months < 1 Then
            '    value = Days
            '    unit = "day"
            'ElseIf Year < 1 Then
            '    value = Months
            '    unit = "mon"
            'Else
            '    value = Year
            '    unit = "yrs"
            'End If

            'Session("MAX_app_timing") = Math.Round(value, 3).ToString(cl_user.regionalizacionCulture)
            'Session("MAX_app_unit") = unit

            val = CType(tbl_unit.Rows.Item(0).Item("value"), Double)
            Session("MAX_app_timing") = val.ToString(cl_user.regionalizacionCulture)
            Session("MAX_app_unit") = tbl_unit.Rows.Item(0).Item("unit")

            tbl_unit = set_Unit(cl_Dashboard.get_MIN_timing(Me.Session("E_IdUser").ToString))

            'minutes = cl_Dashboard.get_MIN_timing
            'hours = minutes / 60
            'Days = hours / 24
            'Months = Days / 30
            'Year = Months / 12
            'value = 0
            'unit = ""

            'If hours < 1 Then
            '    value = minutes
            '    unit = "min"
            'ElseIf Days < 1 Then
            '    value = hours
            '    unit = "hrs"
            'ElseIf Months < 1 Then
            '    value = Days
            '    unit = "day"
            'ElseIf Year < 1 Then
            '    value = Months
            '    unit = "mon"
            'Else
            '    value = Year
            '    unit = "yrs"
            'End If

            'Session("MIN_app_timing") = Math.Round(value, 3).ToString(cl_user.regionalizacionCulture)
            'Session("MIN_app_unit") = unit

            val = CType(tbl_unit.Rows.Item(0).Item("value"), Double)
            Session("MIN_app_timing") = val.ToString(cl_user.regionalizacionCulture)
            Session("MIN_app_unit") = tbl_unit.Rows.Item(0).Item("unit")


            Fill_Tables()

            get_APP_chart()

            get_approval_timing_ByUser(Me.Session("E_IdUser"))

            Filling_task()


        Else

            If HttpContext.Current.Session.Item("cl_Dashboard") IsNot Nothing Then

                cl_Dashboard = Session.Item("cl_Dashboard")

            End If

        End If


    End Sub


    Sub Filling_task()

        Dim tbl As New DataTable
        tbl.Locale = cl_user.regionalizacionCulture
        tbl = cl_Dashboard.get_Pending_APP_task(Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)

        'rept_List.DataSource = tbl
        'rept_List.DataBind()
        Dim strSelectedDates As String = ""
        Dim strSelectedD As String = "SelectedDates[new Date({0})] = new Date({0});"
        Dim strSelectedText As String = ""
        Dim strSelectedT As String = "SeletedText[new Date({0})] = '{1} the approval process {2} was open';"
        Dim strNEW_DATES As String = ""
        Dim strNEW_D As String = "new Date({0})"
        Dim dFechaC As DateTime
        Dim strFechaC As String
        Dim strTimeLapse As String = ""


        If Not IsNothing(tbl) Then

            For Each dtR As DataRow In tbl.Rows

                dFechaC = dtR("datecreated")
                strFechaC = String.Format("'{0:00}/{1:00}/{2}'", DatePart(DateInterval.Month, dFechaC), DatePart(DateInterval.Day, dFechaC), DatePart(DateInterval.Year, dFechaC))
                strSelectedD = String.Format("SelectedDates[new Date({0})] = new Date({0});", strFechaC)
                strNEW_D = String.Format("new Date({0})", strFechaC)
                strTimeLapse = getting_timeLapse(dtR("vdays"))
                strSelectedT = String.Format("SeletedText[new Date({0})] = '{1} the approval process {2} was open';", strFechaC, strTimeLapse, dtR("numero_instrumento"))
                strSelectedDates &= strSelectedD & vbCrLf
                strSelectedText &= strSelectedT & vbCrLf
                strNEW_DATES &= strNEW_D & ","

            Next


            If tbl.Rows.Count > 0 Then

                strNEW_DATES = strNEW_DATES.Substring(0, strNEW_DATES.Trim.Length - 1)

                'Else

                '    '************************************SYSTEM INFO********************************************
                '    Dim dateUtil As APPROVAL.cls_dUtil
                '    Dim cProgram As New RMS.cls_Program
                '    cProgram.get_Sys(0, True)
                '    cProgram.get_Programs(Me.Session("E_IDPrograma"), True)
                '    Dim timezoneUTC As Integer = Val(cProgram.getprogramField("huso_horario", "id_programa", Me.Session("E_IDPrograma")))
                '    dateUtil = New APPROVAL.cls_dUtil(cl_user.regionalizacionCulture, timezoneUTC)
                '    '************************************SYSTEM INFO********************************************


                '    dFechaC = Date.UtcNow
                '    dFechaC = CDate(dateUtil.set_DateFormat(dFechaC, "f"))
                '    strFechaC = String.Format("'{0:00}/{1:00}/{2}'", DatePart(DateInterval.Month, dFechaC), DatePart(DateInterval.Day, dFechaC), DatePart(DateInterval.Year, dFechaC))
                '    strNEW_D = String.Format("new Date({0})", strFechaC)
                '    strNEW_DATES &= strNEW_D

            End If


            Dim strSCript As String = "

                    var SelectedDates = {};
                    //SelectedDates[new Date('05/01/2016')] = new Date('05/01/2016');
                    //SelectedDates[new Date('05/17/2016')] = new Date('05/17/2016');
                    //SelectedDates[new Date('05/30/2016')] = new Date('05/30/2016');
                    <!--##selectedDates--##>

                    var SeletedText = {};
                    //SeletedText[new Date('05/01/2016')] = 'The approval request (TR-016-34) has been received ';
                    //SeletedText[new Date('05/17/2016')] = 'The approval request (TR-016-51) has been received ';
                    //SeletedText[new Date('05/30/2016')] = 'The approval request (TR-016-84) has been received ';
                    <!--##selectedText--##>

                     //The Calender
                    $('#calendar').datepicker({
                        multidate: true,
                        todayHighlight: true,
                        minDate: 0,
                        beforeShowDay: function (date) {

                            var Highlight = SelectedDates[date];
                            var HighlighText = SeletedText[date];

                          //  alert(date);

                            if (Highlight) {
                                return { enabled: true, classes: 'alert alert-warning', tooltip: HighlighText };
                            }
                            else {
                                return { enabled: true, classes: '', tooltip: date.toString().slice(0,15) };
                            }                                                
                        }
                    });
                 
                                       
                    //<!--##Set_Dates##-->              

                 "

            strNEW_DATES = String.Format("$('#calendar').datepicker('setDates', [{0}]);", strNEW_DATES)

            strSCript = strSCript.Replace("<!--##selectedDates--##>", strSelectedDates)
            strSCript = strSCript.Replace("<!--##selectedText--##>", strSelectedText)

            If tbl.Rows.Count > 0 Then
                strSCript = strSCript.Replace(" //<!--##Set_Dates##-->", strNEW_DATES)
            End If

            If tbl.Rows.Count = 0 Then
                app_task.Visible = True
            End If

            Dim strTableTask As String = "<div class='col-sm-5'>
                                             <!-- Progress bars -->
                                             <div class='clearfix'>
                                               <span class='pull-left'>Task #1</span>
                                               <small class='pull-right'>90%</small>
                                             </div>
                                             <div class='progress xs'>
                                                <div class='progress-bar progress-bar-green' style='width 90%;'></div>
                                             </div>
                                             <div class='clearfix'>
                                               <span class='pull-left'>Task #2</span>
                                                 <small class='pull-right'>70%</small>
                                               </div>
                                               <div class='progress xs'>
                                                 <div class='progress-bar progress-bar-green' style='width 70%;'></div>
                                               </div>
                                         </div><!-- /.col -->"




            Dim strColOP As String = "<div class='col-sm-5'>"
            Dim strColCL As String = "</div><!-- /.col -->"

            Dim yellowAL As String = "progress-bar-yellow"
            Dim redAL As String = "progress-bar-red"
            Dim greenAL As String = "progress-bar-green"
            Dim strALert As String = ""


            Dim strTask As String = ""

            Dim strContent As String = "<div class='clearfix'>
                                          <a href='/RMS_APPROVAL/approvals/frm_DocAprobacion.aspx?IdDoc=<!--#ID_DOC-->'><span class='pull-left'><!--#NAME--></span> </a>  
                                          <span class='pull-left'></span>
                                          <small class='pull-right'><!--#PORC1-->%</small>
                                    </div>
                                    <div class='progress xs'>
                                        <div class='progress-bar <!--#ALERT-->' style='width: <!--#PORC2-->%;'></div>
                                    </div>"



            Dim i As Integer = 1
            'strTask &= strColOP 'Open COl

            For Each dtR As DataRow In tbl.Rows

                strContent = "<div class='clearfix'>
                                          <a href='" & urlSys & "/approvals/frm_DocAprobacion.aspx?IdDoc=<!--#ID_DOC-->'><span class='pull-left'><!--#NAME--></span> </a>  
                                          <small class='pull-right'><!--#PORC1-->%</small>
                                    </div>
                                    <div class='progress xs'>
                                        <div class='progress-bar <!--#ALERT-->' style='width: <!--#PORC2-->%;'></div>
                                    </div>"

                If (i Mod 2 <> 0) Then
                    strTask &= strColOP 'Open COl
                End If

                If dtR("progress") <= 33.33 Then
                    strALert = redAL
                ElseIf (dtR("progress") > 33.33 And dtR("progress") <= 66.66) Then
                    strALert = yellowAL
                Else
                    strALert = greenAL
                End If

                strContent = strContent.Replace("<!--#NAME-->", dtR("numero_instrumento"))
                strContent = strContent.Replace("<!--#ID_DOC-->", dtR("id_documento"))
                strContent = strContent.Replace("<!--#PORC1-->", String.Format(cl_user.regionalizacionCulture, "{0:##0.00}", dtR("progress"))) ' Convert.ToDecimal(dtR("progress"), cl_user.regionalizacionCulture)
                strContent = strContent.Replace("<!--#PORC2-->", String.Format(CultureInfo.GetCultureInfo("en-US"), "{0:##0.00}", dtR("progress"))) ' Convert.ToDecimal(dtR("progress"), cl_user.regionalizacionCulture)
                strContent = strContent.Replace("<!--#ALERT-->", strALert)

                i += 1


                If (i Mod 2 <> 0) Then
                    strTask &= strContent & strColCL 'Close COl
                    'strTask &= strColOP 'Open COl
                Else
                    strTask &= strContent
                End If

            Next


            If (i Mod 2 = 0) Then
                strTask &= strColCL 'Close COl
            End If

            task_detail.InnerHtml = strTask



            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", strSCript, True)


        End If

        'tbl = cl_Dashboard.get_Approvals(6)
        'rep_lastest.DataSource = tbl
        'rep_lastest.DataBind()

        'tbl_unit = set_Unit(cl_Dashboard.get_MIN_timing)

    End Sub

    Function getting_timeLapse(ByVal vDays As Integer) As String

        Dim vWeeks As Double
        Dim vMonth As Double
        Dim strUnit As String

        vWeeks = vDays / 7
        vMonth = vDays / 30

        If vDays = 0 Then
            strUnit = "Today"
        ElseIf vDays = 1 Then
            strUnit = "Yestarday"
        ElseIf vWeeks < 1 Then
            strUnit = String.Format("{0:#0} days ago, ", vDays)
        ElseIf vMonth < 1 Then
            strUnit = String.Format("{0:#0} week{1} ago, ", Math.Round(vWeeks, 0, MidpointRounding.ToEven), IIf(vWeeks > 1, "s", ""))
        Else
            strUnit = String.Format("{0:#0} month{1} ago, ", Math.Round(vMonth, 0, MidpointRounding.ToEven), IIf(vMonth > 1, "s", ""))
        End If

        Return strUnit

    End Function


    Function set_Unit(ByVal vMinute As Double) As DataTable

        Dim tbl As DataTable
        tbl = New DataTable("tbl_result")

        Dim col As DataColumn = New DataColumn("Unit")
        col.DataType = System.Type.GetType("System.String")
        Dim col2 As DataColumn = New DataColumn("value")
        col2.DataType = System.Type.GetType("System.Double")
        Dim col3 As DataColumn = New DataColumn("ico")
        col3.DataType = System.Type.GetType("System.String")

        tbl.Columns.Add(col)
        tbl.Columns.Add(col2)
        tbl.Columns.Add(col3)

        Dim minutes As Double = vMinute
        Dim hours As Double = minutes / 60
        Dim Days As Double = hours / 24
        Dim Weeks As Double = Days / 7
        Dim Months As Double = Days / 30
        Dim Year As Double = Months / 12
        Dim value As Double = 0
        Dim unit As String = ""
        Dim ico As String = ""
        Dim plural As String = ""

        If hours < 1 Then
            value = minutes
            unit = "minute"
            ico = "label-primary"
        ElseIf Days < 1 Then
            value = hours
            unit = "hour"
            ico = "label-success"
        ElseIf Weeks < 1 Then
            value = Days
            unit = "day"
            ico = "label-warning"
        ElseIf Months < 1 Then
            value = Weeks
            unit = "week"
            ico = "label-danger"
        ElseIf Year < 1 Then
            value = Months
            unit = "month"
            ico = "label-danger"
        Else
            value = Year
            unit = "year"
            ico = "label-danger"
        End If

        If value >= 2 Then
            unit &= "s"
        End If

        tbl.Rows.Add(unit, Math.Round(value, 3, MidpointRounding.AwayFromZero), ico)

        set_Unit = tbl

    End Function

    Sub Fill_Tables()

        Dim tbl As DataTable = cl_Dashboard.get_Pending_APP(Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)

        rept_List.DataSource = tbl
        rept_List.DataBind()


        If IsNothing(tbl) Then
            Div_Review.Visible = True
        Else
            If tbl.Rows.Count = 0 Then
                Div_Review.Visible = True
            End If
        End If

        tbl = cl_Dashboard.get_Approvals(6, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)
        rep_lastest.DataSource = tbl
        rep_lastest.DataBind()

        'NONE*******************tbl_unit = set_Unit(cl_Dashboard.get_MIN_timing)

    End Sub

    Sub loadData(ByVal cl_user As ly_SIME.CORE.cls_user)
        ' Displays i formatted as currency for the bz.
        'Using dbEntities As New dbRMS_JIEntities
        '    Dim proyectos = dbEntities.tme_Ficha_Proyecto.AsNoTracking().ToList()
        '    Dim numeroProyectos = proyectos.Where(Function(p) p.id_ficha_estado = 2).Count()
        '    Dim aportes = proyectos.Sum(Function(p) p.tme_AportesFicha.Sum(Function(q) q.monto_aporte_obligado))
        '    Me.lbl_numeroProyectos.Text = numeroProyectos
        '    Me.lbl_aportes.Text = aportes.Value.ToString("c2", cl_user.regionalizacionCulture)
        'End Using
    End Sub


    Function get_chart() As String

        Using dbEntities As New dbRMS_JIEntities
            Dim abc = dbEntities.vw_tme_ficha_proyecto.GroupBy(Function(p) p.nombre_subregion) _
                      .Select(Function(p) _
                                          New With {Key .titulo = p.FirstOrDefault().nombre_subregion,
                                                    Key .valor = p.Count(Function(x) x.id_ficha_proyecto)}).ToList()

            Dim elementos(abc.Count - 1) As Series
            Dim i = 0

            For Each item In abc
                Dim series As Series = New Series()
                'series.Name = "Regiones"
                series.Data = New Helpers.Data({New With {Key .title = item.titulo, Key .y = item.valor}})
                elementos(i) = series
                i = i + 1
            Next
            Dim serie As Series = New Series()
            serie.Name = "Regiones"
            Dim datos = New List(Of Object())()
            For Each item In abc
                datos.Add({New Object() {item.titulo, item.valor}})
            Next
            serie.Data = New Helpers.Data(datos.ToArray())

            'Dim chart1 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart")

            'chart1.InitChart(New Chart() With { _
            '                 .DefaultSeriesType = ChartTypes.Column _
            '             }).SetTitle(New Title() With { _
            '                         .Text = "Número de proyectos por Sub región" _
            '                     }) _
            '             .SetXAxis(New XAxis() With { _
            '                       .Categories = New String() {"OCOTEPEQUE", "LA ESPERANZA", "COPÁN"} _
            '                   }) _
            '           .SetYAxis(New YAxis() With { _
            '                       .Title = New YAxisTitle() With {.Text = "Meta Indicador"} _
            '                   }) _
            '           .SetSeries(elementos)
            Dim chart1 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart")

            chart1.InitChart(New Chart() With {
                             .DefaultSeriesType = ChartTypes.Pie
                         }).SetTitle(New Title() With {
                                     .Text = "Número de proyectos por Sub región"
                                 }) _
                       .SetSeries(serie)


            Dim chart As Highcharts = New Highcharts("chart").InitChart(New Chart() With {
                                .PlotShadow = False
                            }).SetTitle(New Title() With {
                                        .Text = "Número de proyectos por Sub región"
                                    }).SetPlotOptions(New PlotOptions() With {
                                                                .Pie = New PlotOptionsPie() With {
                                                                .AllowPointSelect = True,
                                                                .Cursor = Cursors.Pointer
                                                            }
                                                            }).SetSeries(New Series() With {
                                                                         .Type = ChartTypes.Pie,
                                                                         .Name = "Proyectos",
                                                                         .Data = New Helpers.Data(datos.ToArray())
                                                                     })

            'Me.ltrChart.Text = chart.ToHtmlString().Replace("[[", "[").Replace("]]", "]")
            Return chart1.ChartScriptHtmlString().ToString()
        End Using
    End Function




    Function get_APP_chart() As String

        Dim tbl As DataTable = cl_Dashboard.get_Approvals_Total(Me.Session("E_IdUser").ToString)

        Dim serie As Series = New Series()
        serie.Name = "Porcentage of Total Processes"
        Dim datos = New List(Of Object())()

        If Not IsNothing(tbl) Then

            For Each dtr As DataRow In tbl.Rows
                datos.Add({New Object() {dtr("descripcion_cat"), dtr("Porc")}})
            Next

            serie.Data = New Helpers.Data(datos.ToArray())

        Else

            datos.Add({New Object() {"--NONE--", 100}})
            serie.Data = New Helpers.Data(datos.ToArray())

        End If


        'Using dbEntities As New dbRMS_JIEntities
        '    Dim abc = dbEntities.vw_tme_ficha_proyecto.GroupBy(Function(p) p.nombre_subregion) _
        '              .Select(Function(p) _
        '                                  New With {Key .titulo = p.FirstOrDefault().nombre_subregion,
        '                                            Key .valor = p.Count(Function(x) x.id_ficha_proyecto)}).ToList()

        'Dim elementos(abc.Count - 1) As Series
        'Dim i = 0

        'For Each item In abc
        '    Dim series As Series = New Series()
        '    'series.Name = "Regiones"
        '    series.Data = New Helpers.Data({New With {Key .title = item.titulo, Key .y = item.valor}})
        '    elementos(i) = series
        '    i = i + 1
        'Next

        'Dim serie As Series = New Series()
        '    serie.Name = "Regiones"
        '    Dim datos = New List(Of Object())()
        '    For Each item In abc
        '        datos.Add({New Object() {item.titulo, item.valor}})
        '    Next
        '    serie.Data = New Helpers.Data(datos.ToArray())

        Dim chart1 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart1")

        '.DefaultSeriesType = ChartTypes.Pie,
        '.PlotBackgroundColor = New Helpers.BackColorOrGradient(Drawing.Color.DarkGray),

        chart1.InitChart(New Chart() With {
                             .PlotBorderWidth = vbNull,
                             .PlotShadow = False,
                             .Type = ChartTypes.Pie
                         }) _
                       .SetTitle(New Title() With {
                                     .Text = "Procesos de aprobación"
                                 }) _
                       .SetPlotOptions(New PlotOptions() With {.Pie = New PlotOptionsPie With {.AllowPointSelect = True,
                                                                                                 .Cursor = Cursors.Pointer,
                                                                                                 .DataLabels = New PlotOptionsPieDataLabels() With {.Enabled = True,
                                                                                                                                                     .Color = ColorTranslator.FromHtml("#000000"),
                                                                                                                                                     .Formatter = "function() { return '<b>'+ this.percentage.toFixed(2) +' %</b>'; }",
                                                                                                                                                     .Distance = -50
                                                                                                                                                   },
                                                                                                  .ShowInLegend = True
                                                                                                }
                                                                }) _
                      .SetTooltip(New Tooltip() With {
                                                        .PointFormat = "Achieve the <b>{point.percentage:.1f}%</b>"
                                                     }) _
                     .SetSeries(serie)

        ' .Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }",
        ' .PointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>"

        'Dim chart As Highcharts = New Highcharts("chart").InitChart(New Chart() With {
        '                    .PlotShadow = False
        '                }).SetTitle(New Title() With {
        '                            .Text = "Approval Processes"
        '                        }).SetPlotOptions(New PlotOptions() With {
        '                                                    .Pie = New PlotOptionsPie() With {
        '                                                    .AllowPointSelect = True,
        '                                                    .Cursor = Cursors.Pointer
        '                                                }
        '                                                }).SetSeries(New Series() With {
        '                                                             .Type = ChartTypes.Pie,
        '                                                             .Name = "Proyectos",
        '                                                             .Data = New Helpers.Data(datos.ToArray())
        '                                                         })

        'Me.ltrChart.Text = Chart.ToHtmlString().Replace("[[", "[").Replace("]]", "]")

        Me.ltrChart.Text = chart1.ToHtmlString().Replace("[[", "[").Replace("]]", "]")

        ' Return chart1.ChartScriptHtmlString().ToString()
        'End Using

    End Function


    Function get_approval_timing() As String

        Dim tblData As DataTable = cl_Dashboard.get_average_cat_timing()
        Dim tblData_Series As DataTable = cl_Dashboard.get_average_cat_timing_categorie_Serie()
        Dim tblData_Axis As DataTable = cl_Dashboard.get_average_cat_timing_categorie_AxisX()

        Dim datosSerie = New List(Of Series)()
        Dim categories = New List(Of String)
        Dim axis = New List(Of String)

        For Each dtr As DataRow In tblData_Axis.Rows
            axis.Add(dtr("mes"))
        Next

        For Each dtr As DataRow In tblData_Series.Rows
            categories.Add(dtr("descripcion_cat"))
        Next

        Dim findIT As Boolean = False
        Dim DataValues = New List(Of Object)


        For Each dtrSeries As DataRow In tblData_Series.Rows 'For each Category I have to find the

            For Each dtrAxis As DataRow In tblData_Axis.Rows

                For Each dtrValue As DataRow In tblData.Rows

                    If ((dtrSeries("descripcion_cat") = dtrValue("descripcion_cat")) And (dtrAxis("mes") = dtrValue("mes"))) Then
                        DataValues.Add(dtrValue("Hr"))
                        findIT = True
                        Exit For
                    End If

                Next

                If Not findIT Then 'Filling out the serie
                    DataValues.Add(0)
                Else
                    findIT = False
                End If

            Next

            datosSerie.Add(New Series With {.Name = dtrSeries("descripcion_cat"), .Data = New Helpers.Data(DataValues.ToArray())})

            DataValues.Clear()

        Next

        Dim chart2 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart2")

        '.DefaultSeriesType = ChartTypes.Pie,
        '.PlotBackgroundColor = New Helpers.BackColorOrGradient(Drawing.Color.DarkGray),

        chart2.InitChart(New Chart() With {
                             .PlotBorderWidth = vbNull,
                             .PlotShadow = False,
                             .Type = ChartTypes.Line
                         }) _
                       .SetTitle(New Title() With {
                                     .Text = "Tiempo promedio de respuesta",
                                     .X = -20
                         }) _
                        .SetSubtitle(New Subtitle() With {
                                    .Text = "Approval System"
                        }) _
                        .SetXAxis(New XAxis() With {
                                   .Categories = axis.ToArray()
                        }) _
                        .SetYAxis(New YAxis() With {
                                 .Title = (New YAxisTitle() With {.Text = "Hours"
                                                                 }),
                                 .PlotLines = {New YAxisPlotLines() With {
                                                                    .Value = 0,
                                                                    .Width = 1,
                                                                    .Color = ColorTranslator.FromHtml("#808080")}},
                                .Min = 0
                        }) _
                        .SetLegend(New Legend With {
                          .Layout = Layouts.Horizontal,
                          .Align = VerticalAligns.Bottom,
                          .BorderWidth = 0,
                          .X = -10,
                          .Y = 0
                        }) _
                       .SetSeries(datosSerie.ToArray())

        '.SetTooltip(New Tooltip() With {.PointFormat = "Achieve the <b>{point.percentage:.1f}%</b>"})
        ' .Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }",
        ' .PointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>"

        Me.ltrChart2.Text = chart2.ToHtmlString().Replace("[[", "[").Replace("]]", "]")
        '    Return chart2.ChartScriptHtmlString().ToString()
        ' End Using

    End Function




    Function get_approval_timing_ByUser(ByVal id_user) As String

        'Dim tblData As DataTable = cl_Dashboard.get_average_cat_timing()
        'Dim tblData_Series As DataTable = cl_Dashboard.get_average_cat_timing_categorie_Serie()
        Dim tblData_Axis As DataTable = cl_Dashboard.get_average_cat_timing_categorie_AxisX()

        Dim tblData_master As DataTable = cl_Dashboard.get_average_cat_timing_user(id_user)

        Dim datosSerie = New List(Of Series)()
        Dim categories = New List(Of String)
        Dim axis = New List(Of String)

        'Months
        For Each dtr As DataRow In tblData_Axis.Rows
            axis.Add(dtr("mes"))
        Next

        '**************Change this thing*****************
        'For Each dtr As DataRow In tblData_Series.Rows
        '    categories.Add(dtr("descripcion_cat"))
        'Next
        '**************Change this thing*****************


        Dim distinctCategories As IEnumerable(Of String) = tblData_master.AsEnumerable().
                    Select(Function(row) row.Field(Of String)("descripcion_cat")).Distinct()

        Dim arrCategories As String() = distinctCategories.ToArray()

        For Each item As String In arrCategories
            categories.Add(item)
        Next

        Dim findIT As Boolean = False
        Dim DataValues = New List(Of Object)
        Dim findValues As Boolean = False

        For Each strCategory As String In arrCategories 'Categories here

            For Each dtrData As DataRow In tblData_master.Rows 'For each Category I have to find the

                If strCategory = dtrData("descripcion_cat") Then

                    If dtrData("Hr") > 0 Then
                        findValues = True
                    End If
                    DataValues.Add(dtrData("Hr"))

                End If

            Next

            If findValues Then
                datosSerie.Add(New Series With {.Name = strCategory, .Data = New Helpers.Data(DataValues.ToArray())})
                findValues = False
            End If

            DataValues.Clear()

        Next

        'For Each dtrSeries As DataRow In tblData_Series.Rows 'For each Category I have to find the

        '    For Each dtrAxis As DataRow In tblData_Axis.Rows

        '        For Each dtrValue As DataRow In tblData.Rows

        '            If ((dtrSeries("descripcion_cat") = dtrValue("descripcion_cat")) And (dtrAxis("mes") = dtrValue("mes"))) Then
        '                DataValues.Add(dtrValue("Hr"))
        '                findIT = True
        '                Exit For
        '            End If

        '        Next

        '        If Not findIT Then 'Filling out the serie
        '            DataValues.Add(0)
        '        Else
        '            findIT = False
        '        End If

        '    Next

        '    datosSerie.Add(New Series With {.Name = dtrSeries("descripcion_cat"), .Data = New Helpers.Data(DataValues.ToArray())})

        '    DataValues.Clear()

        'Next

        Dim chart2 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart2")

        chart2.InitChart(New Chart() With {
                             .PlotBorderWidth = vbNull,
                             .PlotShadow = False,
                             .Type = ChartTypes.Line
                         }) _
                       .SetTitle(New Title() With {
                                     .Text = "Time Average of Response",
                                     .X = -20
                         }) _
                        .SetSubtitle(New Subtitle() With {
                                    .Text = "Approval System"
                        }) _
                        .SetXAxis(New XAxis() With {
                                   .Categories = axis.ToArray()
                        }) _
                        .SetYAxis(New YAxis() With {
                                 .Title = (New YAxisTitle() With {.Text = "Hours"
                                                                 }),
                                 .PlotLines = {New YAxisPlotLines() With {
                                                                    .Value = 0,
                                                                    .Width = 1,
                                                                    .Color = ColorTranslator.FromHtml("#808080")}},
                                .Min = 0
                        }) _
                        .SetLegend(New Legend With {
                          .Layout = Layouts.Horizontal,
                          .Align = VerticalAligns.Bottom,
                          .BorderWidth = 0,
                          .X = -10,
                          .Y = 0
                        }) _
                       .SetSeries(datosSerie.ToArray())

        Me.ltrChart2.Text = chart2.ToHtmlString().Replace("[[", "[").Replace("]]", "]")



    End Function


    Function get_chart2() As String

        Using dbEntities As New dbRMS_JIEntities
            Dim abc = dbEntities.vw_tme_ficha_proyecto.GroupBy(Function(p) p.nombre_subregion) _
                      .Select(Function(p) _
                                          New With {Key .titulo = p.FirstOrDefault().nombre_subregion, _
                                                    Key .valor = p.Count(Function(x) x.id_ficha_proyecto)}).ToList()

            Dim datos = New List(Of Object())()
            For Each item In abc
                datos.Add({New Object() {item.titulo, item.valor}})
            Next


            Dim chart2 As Highcharts = New Highcharts("chart2").InitChart(New Chart() With { _
                                .PlotShadow = False _
                            }).SetTitle(New Title() With { _
                                        .Text = "Número de proyectos por Sub región - TEST" _
                                    }).SetPlotOptions(New PlotOptions() With { _
                                            .Pie = New PlotOptionsPie() With { _
                                            .AllowPointSelect = True, _
                                            .Cursor = Cursors.Pointer _
                                        } _
                                    }).SetSeries(New Series() With { _
                                                    .Type = ChartTypes.Pie, _
                                                    .Name = "Proyectos", _
                                                    .Data = New Helpers.Data(datos.ToArray()) _
                                                })

            Me.ltrChart2.Text = chart2.ToHtmlString().Replace("[[", "[").Replace("]]", "]")
            Return chart2.ChartScriptHtmlString().ToString()
        End Using
    End Function

    Function get_chartIndicadores() As String
        Using dbEntities As New dbRMS_JIEntities

            Dim indicadores = dbEntities.vw_indicador_metas_anuales.GroupBy(Function(p) p.id_indicador) _
                              .Select(Function(p) p.FirstOrDefault()).ToList()

            Dim datos = New List(Of Series)()
            Dim proyectado = New List(Of Object)
            Dim ejecutado = New List(Of Object)
            Dim aaaaaaa = New Series()
            Dim categories = New List(Of String)
            For Each item In indicadores
                categories.Add(item.codigo_indicador)
                aaaaaaa.Data = New Helpers.Data({item.meta_total, item.Totalejecutado})
                ejecutado.Add(item.Totalejecutado)
                proyectado.Add(item.meta_total)
            Next
            datos.Add(New Series With {.Name = "Ejecutado", .Data = New Helpers.Data(ejecutado.ToArray())})
            datos.Add(New Series With {.Name = "Proyectado", .Data = New Helpers.Data(proyectado.ToArray())})
            Dim chart3 As Highcharts = New Highcharts("chart3").InitChart(New Chart() With { _
                                .PlotShadow = False, .Type = ChartTypes.Bar _
                            }).SetTitle(New Title() With { _
                                        .Text = "Ejecución Global Indicador" _
                                    }).SetPlotOptions(New PlotOptions()).SetSeries(datos.ToArray()) _
                                                         .SetXAxis(New XAxis() With { _
                                                                   .Categories = categories.ToArray()})

            'Me.ltrChartIndicadores.Text = chart3.ToHtmlString().Replace("[[", "[").Replace("]]", "]")


            Return chart3.ChartScriptHtmlString().ToString().Replace("[[", "[").Replace("]]", "]")
        End Using
    End Function


    'Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
    '    Me.MsgGuardar.NuevoMensaje = "Guardado Correctamente"
    '    Me.MsgGuardar.Redireccion = "~/SuperAdmin/frm_roles"
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    'End Sub
End Class