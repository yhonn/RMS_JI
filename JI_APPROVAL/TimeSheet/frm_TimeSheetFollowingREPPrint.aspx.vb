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

Public Class frm_TimeSheetFollowingREPPrint
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_TIMESHEET_EDIT"
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
    Public aprobador As String
    Public cargo_aprobador As String
    Public fecha_aprobacion As DateTime
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
        'Try
        '    If Me.Session("E_IdUser").ToString = "" Then
        '    End If
        'Catch ex As Exception
        '    Me.Response.Redirect("~/frm_login.aspx")
        'End Try

        'If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
        '    cl_user = Session.Item("clUser")
        '    If Not cl_user.chk_accessMOD(0, frmCODE) Then
        '        Me.Response.Redirect("~/Proyectos/no_access2")
        '    Else
        '        cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
        '        ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
        '    End If
        '    controles.code_mod = frmCODE
        '    For Each Control As Control In Page.Controls
        '        controles.checkControls(Control, cl_user.id_idioma, cl_user)
        '    Next
        'End If

        If Not IsPostBack Then

            hd_IDtimeSheet.Value = Request.QueryString("ID")

            loadData()
            'loadGRID(1)
            loadTABLE()

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

    Public Sub loadData()

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

        ''************************************SYSTEM INFO********************************************
        'Dim cProgram As New RMS.cls_Program
        'cProgram.get_Sys(0, True)
        'cProgram.get_Programs(cl_user.Id_Cprogram, True)
        'Dim userCulture As CultureInfo
        'Dim timezoneUTC As Integer
        'userCulture = cl_user.regionalizacionCulture
        'timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        'clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        If Not IsNothing(Request.QueryString("ID")) Then
            hd_IDtimeSheet.Value = Convert.ToInt32(Request.QueryString("ID"))
        Else
            hd_IDtimeSheet.Value = 0
        End If

        Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)

        userName = timesheet.nombre_usuario
        userJOB_tittle = timesheet.job
        Month_TS = MonthName(timesheet.mes)

        Dim id_timesheet_estado As Integer = timesheet.id_timesheet_estado


        If id_timesheet_estado = 3 Then
            'Me.aprobadorInfo.Visible = True

            aprobador = timesheet.aprobador
            cargo_aprobador = timesheet.cargo_aprobador
            fecha_aprobacion = timesheet.fecha_aprobacion


        End If

        'hd_month.Value = timesheet.mes
        Year_TS = timesheet.anio
        'hd_year.Value = timesheet.anio
        Status_TS = timesheet.timesheet_estado

        Me.hd_leave.Value = Convert.ToInt32(timesheet.ts_leave_update)

        'Me.hd_IDuser.Value = timesheet.id_usuario
        Me.hd_id_employee_type.Value = timesheet.id_employee_type
        Me.lbl_employeeType.Text = timesheet.employee_type

        If IsDBNull(timesheet.fecha_upd) Then
            DateStatus_TS = String.Format(" {0}<br />", timesheet.fecha_upd)
            HourStatus_TS = String.Format(" {0} ", timesheet.fecha_upd)
        Else
            DateStatus_TS = String.Format(" {0}<br />", timesheet.fecha_creo)
            HourStatus_TS = String.Format(" {0} ", timesheet.fecha_creo)
        End If

        Me.PDescription.InnerHtml = timesheet.description.Trim
        'Me.Pnotes.InnerHtml = timesheet.notes.Trim.Replace("<", "&lt;").Replace(">", "&gt;")
        '&lt;code&gt;

        'Me.anchorInformation.Attributes.Add("class", "btn btn-success btn-circle")
        'Me.anchorInformation.Attributes.Add("href", "frm_TimeSheetAdd.aspx?ID=" & hd_IDtimeSheet.Value)

        'Me.anchorBillable.Attributes.Add("class", "btn btn-success btn-circle")
        'Me.anchorBillable.Attributes.Add("href", "frm_TimeSheetEdit.aspx?ID=" & hd_IDtimeSheet.Value)

        'Me.anchorFollowUp.Attributes.Add("class", "btn btn-primary btn-circle")

        Dim tbl_user_role As New DataTable
        Dim strRoles As String = ""

        '*********************** All Group Roles Just Simple Roles*******************************
        tbl_user_role = clss_approval.get_RolesUser(timesheet.id_usuario, 3) 'User of the TimeSheet
        For Each dtRow In tbl_user_role.Rows
            strRoles &= dtRow("id_rol").ToString & ","
        Next
        If strRoles.Length > 0 Then
            strRoles = strRoles.Substring(0, strRoles.Length - 1)
        End If
        'lbl_ALL_SIMPLE_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
        '*********************** All Roles*******************************************************

        'If timesheet.id_timesheet_estado = 1 Then
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
        'Else
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
        '    Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & hd_IDtimeSheet.Value)
        'End If


        Dim tbl_Suppor_docs As DataTable = cls_TimeSheet.TimeSheet_Support_Documents_detail(Convert.ToInt32(hd_IDtimeSheet.Value))

        If tbl_Suppor_docs.Rows.Count() > 0 Then
            'Me.documentos.Visible = True
            Me.rpt_support_docs.DataSource = tbl_Suppor_docs
            Me.rpt_support_docs.DataBind()
        End If




        If timesheet.id_timesheet_estado > 1 Then 'In process or Approved/Not Approved

            Using db As New dbRMS_JIEntities

                Dim idTS As Integer = Convert.ToInt32(hd_IDtimeSheet.Value)

                If db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).Count > 0 Then
                    Dim idDoc As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = idTS).FirstOrDefault().id_documento
                    Me.rept_msgApproval.DataSource = clss_approval.get_Document_Comments_special(idDoc)
                    Me.rept_msgApproval.DataBind()
                Else
                    Me.rept_msgApproval.DataSource = Nothing
                End If

            End Using


        End If


    End Sub

    Public Sub loadTABLE()

        Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)

        Dim DaysInMonth As Integer = Date.DaysInMonth(timesheet.anio, timesheet.mes)

        'If OPT = 1 Then
        'Me.grd_cate.DataSource = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value) 'The Original Template
        'Else
        'Me.grd_cate.DataSource = cls_TimeSheet.get_Table(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, cmb_billable_Item.SelectedValue) ' To adding extra Item
        'End If

        'Me.grd_cate.DataBind()

        Dim ci As CultureInfo
        ci = New CultureInfo("es-CO")


        strTableResult = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, Me.hd_leave.Value, ci) 'The Original Template

        '************CHANGE FOR OTHER KIND OF PERIOD***********
        'Dim nameCol As String
        'For i = DaysInMonth + 1 To 31
        '    nameCol = String.Format("colm_d{0}", i)
        '    Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
        'Next
        '************CHANGE FOR OTHER KIND OF PERIOD***********

        'Load the Time Sheet
        LoadTimeSheet()


    End Sub


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
        reptTable.DataSource = summarizedTIMEsheet
        reptTable.DataBind()

        Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

        If Not IsNothing(summaryTIMEsheet) Then

            TOThrs = summaryTIMEsheet.TOThours
            TOTloe = summaryTIMEsheet.LOE

        End If

        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "setTimeout(CalculateATall, 100);", True)

    End Sub



    'Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

    '    Dim clDate As APPROVAL.cls_dUtil
    '    '************************************SYSTEM INFO********************************************
    '    Dim cProgram As New RMS.cls_Program
    '    cProgram.get_Sys(0, True)
    '    cProgram.get_Programs(cl_user.Id_Cprogram, True)
    '    Dim userCulture As CultureInfo
    '    Dim timezoneUTC As Integer
    '    userCulture = cl_user.regionalizacionCulture
    '    timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
    '    clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
    '    '************************************SYSTEM INFO********************************************

    '    Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
    '    'Return dateIN.ToShortDateString

    'End Function


    'Public Function getHora(dateIN As DateTime) As String

    '    Dim clDate As APPROVAL.cls_dUtil
    '    '************************************SYSTEM INFO********************************************
    '    Dim cProgram As New RMS.cls_Program
    '    cProgram.get_Sys(0, True)
    '    cProgram.get_Programs(cl_user.Id_Cprogram, True)
    '    Dim userCulture As CultureInfo
    '    Dim timezoneUTC As Integer
    '    userCulture = cl_user.regionalizacionCulture
    '    timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
    '    clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
    '    '************************************SYSTEM INFO********************************************

    '    Return clDate.set_TimeFormat(dateIN, timezoneUTC, True)


    'End Function



End Class
