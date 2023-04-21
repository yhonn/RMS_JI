Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Web.Script.Serialization
Imports Telerik.Web.UI
Imports System.Web.Services
Imports ly_APPROVAL
Imports ly_RMS
Imports ly_SIME
Imports System.Globalization



Partial Class frm_TimeSheetFollowingREP_pay
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_TIMESHEET_REP_PAY"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim clDate As APPROVAL.cls_dUtil

    Public userName As String
    Public userJOB_tittle As String
    Public Month_TS As String
    Public Year_TS As String
    Public Status_TS As String
    Public DateStatus_TS As String
    Public HourStatus_TS As String
    Public TOThrs As Decimal
    Public TOTloe As Decimal
    Public strTableResult As String
    Dim clss_approval As APPROVAL.clss_approval

    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            '  hd_IDtimeSheet.Value = Request.QueryString("ID")
            loadData(Me.Request.QueryString("y"), Me.Request.QueryString("m"))
            'loadTABLE()

            'Me.lblJobTittle.Text = ""
            'Me.lbl_IdEmpleado.Text = 0


        Else

            'If Me.grd_cate.Items.Count() > 0 Then
            '    Me.cmb_rol.Enabled = False
            'Else
            '    Me.cmb_rol.Enabled = True
            'End If

        End If


    End Sub

    Public Sub loadData(ByVal y As Integer, ByVal m As Integer)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        If Not IsNothing(Request.QueryString("ID")) Then
            hd_IDtimeSheet.Value = Convert.ToInt32(Request.QueryString("ID"))
        Else
            hd_IDtimeSheet.Value = 0
        End If


        Me.hd_month_var.Value = m
        Me.hd_year_var.Value = y

        'Dim timesheet_Period As List(Of vw_ta_timesheet) = cls_TimeSheet.getTimeSheet_period(Me.hd_month_var.Value, Me.hd_year_var.Value)

        Me.rep_Report.DataSource = cls_TimeSheet.getTimeSheet_period_d(Me.hd_month_var.Value, Me.hd_year_var.Value)
        Me.rep_Report.DataBind()



        'userName = timesheet.nombre_usuario
        'userJOB_tittle = timesheet.job
        'Month_TS = MonthName(timesheet.mes)
        'hd_month.Value = timesheet.mes
        'Year_TS = timesheet.anio
        'hd_year.Value = timesheet.anio
        'Status_TS = timesheet.timesheet_estado

        'Me.hd_leave.Value = Convert.ToInt32(timesheet.ts_leave_update)

        'Me.hd_IDuser.Value = timesheet.id_usuario
        'Me.hd_id_employee_type.Value = timesheet.id_employee_type
        'Me.lbl_employeeType.Text = timesheet.employee_type

        'If IsDBNull(timesheet.fecha_upd) Then
        '    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_upd, "m", timezoneUTC, False))
        '    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_upd, timezoneUTC, True))
        'Else
        '    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_creo, "m", timezoneUTC, False))
        '    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_creo, timezoneUTC, True))
        'End If

        'Me.PDescription.InnerHtml = timesheet.description.Trim
        'Me.Pnotes.InnerHtml = timesheet.notes.Trim.Replace("<", "&lt;").Replace(">", "&gt;")

        'Dim tbl_user_role As New DataTable
        'Dim strRoles As String = ""

        ''*********************** All Group Roles Just Simple Roles*******************************
        'tbl_user_role = clss_approval.get_RolesUser(timesheet.id_usuario, 3) 'User of the TimeSheet
        'For Each dtRow In tbl_user_role.Rows
        '    strRoles &= dtRow("id_rol").ToString & ","
        'Next
        'If strRoles.Length > 0 Then
        '    strRoles = strRoles.Substring(0, strRoles.Length - 1)
        'End If
        'lbl_ALL_SIMPLE_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
        ''*********************** All Roles*******************************************************




        'Dim tbl_Suppor_docs As DataTable = cls_TimeSheet.TimeSheet_Support_Documents_detail(Convert.ToInt32(hd_IDtimeSheet.Value))

        'Me.rpt_support_docs.DataSource = tbl_Suppor_docs
        'Me.rpt_support_docs.DataBind()


        'If timesheet.id_timesheet_estado > 1 Then 'In process or Approved/Not Approved

        '    Using db As New dbRMS_JIEntities

        '        Dim idTS As Integer = Convert.ToInt32(hd_IDtimeSheet.Value)

        '        If db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).Count > 0 Then
        '            Dim idDoc As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).FirstOrDefault().id_documento
        '            Me.rept_msgApproval.DataSource = clss_approval.get_Document_Comments_special(idDoc)
        '            Me.rept_msgApproval.DataBind()
        '        Else
        '            Me.rept_msgApproval.DataSource = Nothing
        '        End If

        '    End Using


        'End If

    End Sub


    Protected Sub rep_Report_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) 'Handles reptTable.ItemDataBound


        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(cl_user.Id_Cprogram, True)
            Dim userCulture As CultureInfo
            Dim timezoneUTC As Integer
            userCulture = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
            clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************


            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)

            Dim sp_mes_ctrl As HtmlGenericControl = ItemD.FindControl("sp_mes") 'mes
            Dim sp_date_ctrl As HtmlGenericControl = ItemD.FindControl("sp_date") 'Date
            Dim sp_hour_ctrl As HtmlGenericControl = ItemD.FindControl("sp_hour") 'Date

            If IsDBNull(DataBinder.Eval(e.Item.DataItem, "fecha_upd")) Then

                DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "fecha_upd"), "m", timezoneUTC, False))
                HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(DataBinder.Eval(e.Item.DataItem, "fecha_upd"), timezoneUTC, True))

            Else
                DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "fecha_creo"), "m", timezoneUTC, False))
                HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(DataBinder.Eval(e.Item.DataItem, "fecha_creo"), timezoneUTC, True))
            End If

            sp_mes_ctrl.InnerHtml = MonthName(DataBinder.Eval(e.Item.DataItem, "mes"))

            sp_date_ctrl.InnerHtml = DateStatus_TS
            sp_hour_ctrl.InnerHtml = HourStatus_TS

            Dim p_Description_ctrl As HtmlGenericControl = ItemD.FindControl("PDescription") 'mes
            p_Description_ctrl.InnerHtml = DataBinder.Eval(e.Item.DataItem, "Description")


            Dim p_div_TS_ctrl As HtmlGenericControl = ItemD.FindControl("dv_TS") 'TS
            p_div_TS_ctrl.InnerHtml = loadTABLE(DataBinder.Eval(e.Item.DataItem, "id_timesheet"))


            Dim p_Notes_ctrl As HtmlGenericControl = ItemD.FindControl("Pnotes") 'mes
            p_Notes_ctrl.InnerHtml = DataBinder.Eval(e.Item.DataItem, "notes")

            TOThrs = 0
            TOTloe = 0

            Dim rept_Reports As Repeater = ItemD.FindControl("reptTable")
            rept_Reports.DataSource = LoadTimeSheet_summary(DataBinder.Eval(e.Item.DataItem, "id_timesheet"))
            rept_Reports.DataBind()

            Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(DataBinder.Eval(e.Item.DataItem, "id_timesheet"))

            '        If Not IsNothing(summaryTIMEsheet) Then

            '            TOThrs += summaryTIMEsheet.TOThours
            '            TOTloe += summaryTIMEsheet.LOE
            '            'p_hours_ctrl.InnerHtml = summaryTIMEsheet.TOThours
            '            'p_days_ctrl.InnerHtml = summaryTIMEsheet.LOE

            Dim p_hours_ctrl As HtmlGenericControl = ItemD.FindControl("sp_tot_hours") 'hours
            Dim p_days_ctrl As HtmlGenericControl = ItemD.FindControl("sp_tot_days") 'days

            p_hours_ctrl.InnerHtml = summaryTIMEsheet.TOThours
            p_days_ctrl.InnerHtml = summaryTIMEsheet.TOTdias

            Dim rept_msg As Repeater = ItemD.FindControl("rept_msgApproval")

            Using db As New dbRMS_JIEntities

                Dim idTS As Integer = DataBinder.Eval(e.Item.DataItem, "id_timesheet")
                If db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).Count > 0 Then
                    Dim idDoc As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).FirstOrDefault().id_documento
                    rept_msg.DataSource = clss_approval.get_Document_Comments_special(idDoc)
                    rept_msg.DataBind()
                Else
                    rept_msg.DataSource = Nothing
                End If

            End Using

        End If


    End Sub


    Public Function loadTABLE(ByVal idTS As Integer) As String

        Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(idTS)

        Dim DaysInMonth As Integer = Date.DaysInMonth(timesheet.anio, timesheet.mes)

        'If OPT = 1 Then
        'Me.grd_cate.DataSource = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value) 'The Original Template
        'Else
        'Me.grd_cate.DataSource = cls_TimeSheet.get_Table(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, cmb_billable_Item.SelectedValue) ' To adding extra Item
        'End If

        'Me.grd_cate.DataBind()

        ' strTableResult = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, Me.hd_leave.Value) 'The Original Template

        '************CHANGE FOR OTHER KIND OF PERIOD***********
        'Dim nameCol As String
        'For i = DaysInMonth + 1 To 31
        '    nameCol = String.Format("colm_d{0}", i)
        '    Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
        'Next
        '************CHANGE FOR OTHER KIND OF PERIOD***********

        'Load the Time Sheet


        loadTABLE = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, timesheet.id_timesheet, timesheet.id_employee_type, timesheet.ts_leave_update) 'The Original Template
        'LoadTimeSheet()


    End Function


    Private Function calculaDiaHabil(ByVal nDias As Integer, ByVal fechaPost As Date) As Date
        Dim hoy As Date = Date.UtcNow
        Dim weekend As Integer = 0
        Dim fecha_limite As Date
        Select Case fechaPost.DayOfWeek
            Case DayOfWeek.Sunday
                If nDias < 6 Then
                    weekend = 0
                ElseIf nDias < 11 Then
                    weekend = 2
                ElseIf nDias < 16 Then
                    weekend = 4
                End If
            Case DayOfWeek.Monday
                If nDias < 5 Then
                    weekend = 0
                ElseIf nDias < 10 Then
                    weekend = 2
                ElseIf nDias < 15 Then
                    weekend = 4
                End If
            Case DayOfWeek.Tuesday
                If nDias < 4 Then
                    weekend = 0
                ElseIf nDias < 9 Then
                    weekend = 2
                ElseIf nDias < 14 Then
                    weekend = 4
                End If
            Case DayOfWeek.Wednesday
                If nDias < 3 Then
                    weekend = 0
                ElseIf nDias < 8 Then
                    weekend = 2
                ElseIf nDias < 13 Then
                    weekend = 4
                End If
            Case DayOfWeek.Thursday
                If nDias < 2 Then
                    weekend = 0
                ElseIf nDias < 7 Then
                    weekend = 2
                ElseIf nDias < 12 Then
                    weekend = 4
                End If
            Case DayOfWeek.Friday
                If nDias < 1 Then
                    weekend = 0
                ElseIf nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 11 Then
                    weekend = 4
                End If
            Case DayOfWeek.Saturday
                If nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 10 Then
                    weekend = 4
                End If
        End Select
        Dim totaldias = weekend + nDias
        fecha_limite = DateAdd(DateInterval.DayOfYear, totaldias, fechaPost)
        Return fecha_limite
    End Function

    Public Function generate_CODE(ByVal idCat As Integer) As String


        If idCat > 0 Then

            'Me.lbl_idDocumento.Text = clss_approval.Approval_CodeCreate(idCat)
            Dim strCode As String = clss_approval.Approval_CodeCreate(idCat)
            Dim strResultCode As String = ""
            Dim vTotal As Integer = strCode.Length
            Dim vSubstr As Integer = strCode.IndexOf("-")
            Dim Index As Integer = 0

            If vSubstr > 0 Then
                Index = strCode.IndexOf("-") - 1
            Else
                Index = 0
            End If

            Dim p1 As String = strCode.Substring(0, Index)
            If p1.Length = 0 Then
                p1 = "APP"
            End If

            Dim p2 As String = strCode.Substring(strCode.IndexOf("-") + 1, vTotal - (vSubstr + 1))

            strResultCode = String.Format("{0}-0{1}-{2}", p1, Date.Today.Year.ToString.Substring(2, 2), p2)

            generate_CODE = strResultCode

            'txt_Number.Text = strResultCode
            'txt_codigoAID.Text = strResultCode

        Else

            generate_CODE = "--No Code--"

        End If

    End Function



    Sub LoadTimeSheet()

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        Dim registeredTS As DataTable = cls_TimeSheet.getTimeSheetDetail(hd_IDtimeSheet.Value)
        Dim dia As Integer = 0
        Dim nameCol As String
        Dim txtName As String
        Dim txtRadNumeric As RadNumericTextBox

        If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

            'For Each Irow As GridDataItem In Me.grd_cate.Items

            '    If TypeOf Irow Is GridDataItem Then

            '        Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

            '        For Each Drow In registeredTS.Rows

            '            If idBillableTime = Drow("id_billable_time") Then

            '                dia = Drow("dia")
            '                nameCol = String.Format("colm_d{0}", dia.ToString.Trim)
            '                txtName = String.Format("txt_d{0}", dia.ToString.Trim)

            '                txtRadNumeric = CType(Irow(nameCol).FindControl(txtName), RadNumericTextBox)
            '                txtRadNumeric.Value = Convert.ToInt32(Drow("hours"))
            '                Irow(dia).Text = Convert.ToInt32(Drow("hours"))

            '            End If

            '        Next

            '    End If

            'Next

        End If

        Dim summarizedTIMEsheet As DataTable = cls_TimeSheet.get_TimeSheet_Summary(hd_IDtimeSheet.Value)
        'reptTable.DataSource = summarizedTIMEsheet
        'reptTable.DataBind()

        Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

        If Not IsNothing(summaryTIMEsheet) Then

            TOThrs = summaryTIMEsheet.TOThours
            TOTloe = summaryTIMEsheet.LOE

        End If

        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "setTimeout(CalculateATall, 100);", True)

    End Sub



    Public Function LoadTimeSheet_summary(ByVal idTS As Integer) As DataTable

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        'Dim registeredTS As DataTable = cls_TimeSheet.getTimeSheetDetail(idTS)
        Dim dia As Integer = 0

        'Dim summarizedTIMEsheet As DataTable = cls_TimeSheet.get_TimeSheet_Summary(hd_IDtimeSheet.Value)
        'reptTable.DataSource = summarizedTIMEsheet
        'reptTable.DataBind()

        LoadTimeSheet_summary = cls_TimeSheet.get_TimeSheet_Summary_rept(idTS)

        'Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

        'If Not IsNothing(summaryTIMEsheet) Then

        '    TOThrs = summaryTIMEsheet.TOThours
        '    TOTloe = summaryTIMEsheet.LOE

        'End If

    End Function


    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
        'Return dateIN.ToShortDateString

    End Function


    Public Function getHora(dateIN As DateTime) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        Return clDate.set_TimeFormat(dateIN, timezoneUTC, True)


    End Function

    'Protected Sub reptTable_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then


    '        Dim ItemD As RepeaterItem
    '        ItemD = CType(e.Item, RepeaterItem)

    '        Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(DataBinder.Eval(e.Item.DataItem, "id_timesheet"))

    '        If Not IsNothing(summaryTIMEsheet) Then

    '            TOThrs += summaryTIMEsheet.TOThours
    '            TOTloe += summaryTIMEsheet.LOE
    '            'p_hours_ctrl.InnerHtml = summaryTIMEsheet.TOThours
    '            'p_days_ctrl.InnerHtml = summaryTIMEsheet.LOE

    '        End If


    '    End If


    'End Sub
End Class
