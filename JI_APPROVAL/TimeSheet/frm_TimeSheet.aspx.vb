Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Globalization
Imports ly_APPROVAL
Imports ly_RMS

Partial Class frm_TimeSheet
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_TIMESHEET"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim clss_approval As APPROVAL.clss_approval
    Dim booEXEC As Boolean = False

    Sub ActualizaDatos()
        Dim row As GridItem
        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkvisible As CheckBox = CType(rowD("colm_visible").FindControl("chkVisible"), CheckBox)
            If chkvisible.Checked = True Then
                visibleSI &= "," & rowD("id_tipoDocumento").Text
            Else
                visibleNO &= "," & rowD("id_tipoDocumento").Text
            End If
        Next

        cnnSAP.Open()
        Dim dm As New SqlCommand("UPDATE ta_tipoDocumento SET visible='SI' WHERE id_tipoDocumento IN(" & visibleSI & ")", cnnSAP)
        dm.ExecuteNonQuery()
        dm.CommandText = "UPDATE ta_tipoDocumento SET visible='NO' WHERE id_tipoDocumento IN(" & visibleNO & ")"
        dm.ExecuteNonQuery()
        cnnSAP.Close()
        Me.grd_cate.DataBind()

        cnnSAP.Close()
    End Sub


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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then


            Dim tbl_user_role As New DataTable
            Dim strRoles As String = ""
            '***********************Group Roles***********************************
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
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

            Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

            Me.cmb_year.DataSource = cls_TimeSheet.get_years()
            Me.cmb_year.DataBind()

            Me.cmb_Month.DataSource = cls_TimeSheet.get_months()
            Me.cmb_Month.DataBind()

            Dim Menu As GridFilterMenu = grd_cate.FilterMenu
            For Each item In Menu.Items
                'change the text for the StartsWith menu item
                If item.Text = "NoFilter" Then
                    item.Text = "Limpiar filtro"
                ElseIf item.Text = "Contains" Then
                    item.Text = "Contiene"
                ElseIf item.Text = "DoesNotContain" Then
                    item.Text = "No contiene"
                ElseIf item.Text = "StartsWith" Then
                    item.Text = "Empieza con"
                ElseIf item.Text = "EndsWith" Then
                    item.Text = "Finaliza con"
                ElseIf item.Text = "EqualTo" Then
                    item.Text = "Igual a"
                ElseIf item.Text = "NotEqualTo" Then
                    item.Text = "No igual a"
                ElseIf item.Text = "GreaterThan" Then
                    item.Text = "Mayor a"
                ElseIf item.Text = "LessThan" Then
                    item.Text = "Menor a"
                ElseIf item.Text = "GreaterThanOrEqualTo" Then
                    item.Text = "Mayor o igual a"
                ElseIf item.Text = "LessThanOrEqualTo" Then
                    item.Text = "Menor o igual a"
                ElseIf item.Text = "Between" Then
                    item.Text = "Entre"
                ElseIf item.Text = "NotBetween" Then
                    item.Text = "NoEntre"
                ElseIf item.Text = "IsEmpty" Then
                    item.Text = "Es vacío"
                ElseIf item.Text = "NotIsEmpty" Then
                    item.Text = "No es vacío"
                ElseIf item.Text = "IsNull" Then
                    item.Text = "Es nulo"
                ElseIf item.Text = "NotIsNull" Then
                    item.Text = "No es nulo"
                End If
            Next

            Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            Dim timeSheetPending = cls_TimeSheet.getTimeSheetPending(idUsuario)

            If timeSheetPending > 0 Then
                Me.btn_nuevo.Visible = False
                Me.btn_leave_approval.Visible = False
            End If

            LoadGrid(True)

            'Me.btn_nuevo.Visible = True

        End If


    End Sub


    Public Sub LoadGrid(ByVal bndRebind As Boolean)

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim strRoles As String = lbl_GroupRolID.Text.Trim
        Dim strUsers As String = lbl_GroupRolID.Text.Trim
        Dim ArrayRoles As String() = strRoles.Split(New Char() {","c})
        Dim ArrayUsers As String() = strUsers.Split(New Char() {","c})
        Dim ArrayINT_Users As Integer() = Array.ConvertAll(ArrayUsers, Function(str) Int32.Parse(str))
        Dim ArrayINT_Roles As Integer() = Array.ConvertAll(ArrayRoles, Function(str) Int32.Parse(str))
        Me.grd_cate.DataSource = cls_TimeSheet.getTimeSheet(Convert.ToInt32(Me.Session("E_IDuser")), ArrayINT_Roles, Val(h_Filter.Value))

        If bndRebind Then
            Me.grd_cate.DataBind()
        End If


    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        Dim sql = "SELECT id_tipoDocumento, id_categoria, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, descripcion_cat, visible FROM vw_aprobaciones WHERE id_programa=" & Me.Session("E_IDPrograma") & " AND ((descripcion_aprobacion LIKE '%" & Me.txt_doc.Text & "%') OR (descripcion_cat LIKE '%" & Me.txt_doc.Text & "%') OR (condicion LIKE '%" & Me.txt_doc.Text & "%'))"
        Me.SqlDataSource2.SelectCommand = sql
        Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound



        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            'item.DataCell.Text = item.DataCell.Text.Split(":")(1).ToString
            item.DataCell.Text = ""
        End If


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim editar As New ImageButton
            'Dim col_Eliminar As New GridButtonColumn
            'col_Eliminar = CType(grd_cate.MasterTableView.GetColumn("colm_delete"), GridButtonColumn)
            Dim lk_Eliminar As New ImageButton

            'HyperLink ID="hlk_timesheet"
            Dim Preview_ As New HyperLink
            Dim EditAPP_ As New HyperLink


            '    Dim ruta As New ImageButton
            '    Dim visible As New CheckBox
            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton)
            lk_Eliminar = CType(itemD("colm_delete").Controls(0), ImageButton)

            If Val(itemD("id_timesheet_estado").Text) = 1 Then
                lk_Eliminar.Visible = True
            Else
                lk_Eliminar.Visible = False
            End If

            EditAPP_ = CType(itemD("colm_edit_app").FindControl("hlk_edit_app"), HyperLink)
            EditAPP_.NavigateUrl = "frm_TimeSheet_app_edit.aspx?ID=" & itemD("id_timesheet").Text

            If Val(itemD("id_timesheet_estado").Text) = 3 Then
                EditAPP_.Visible = True
            Else
                EditAPP_.Visible = False
            End If

            Preview_ = CType(itemD("colm_open").FindControl("hlk_timesheet"), HyperLink)
            Preview_.NavigateUrl = "frm_TimeSheetFollowingREP.aspx?ID=" & itemD("id_timesheet").Text

            Dim idUsuario As Integer = Convert.ToInt32(Me.Session("E_IdUser"))
            If itemD("id_usuario").Text = idUsuario Or itemD("usuario_creo").Text = idUsuario Then
                editar.PostBackUrl = "frm_TimeSheetAdd.aspx?ID=" & itemD("id_timesheet").Text & "&lv=" & Convert.ToInt32(Convert.ToBoolean(itemD("ts_leave_update").Text)).ToString
            Else
                editar.Visible = False
            End If

            Convert.ToBoolean(itemD("ts_leave_update").Text)

            '    visible = CType(itemD("colm_visible").FindControl("chkVisible"), CheckBox) ' CType(e.Item.FindControl("chkVisible"), CheckBox)

            '    Dim Sql = "SELECT ruta_completa FROM ta_tipoDocumento WHERE id_tipoDocumento=" & itemD("id_tipoDocumento").Text 'e.Item.Cells(4).Text.ToString
            '    Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            '    Dim ds1 As New DataSet("ruta")
            '    dm1.Fill(ds1, "ruta")

            '    ruta = CType(itemD("colm_path").FindControl("ruta"), ImageButton) 'CType(e.Item.FindControl("ruta"), ImageButton)

            '    If ds1.Tables("ruta").Rows(0).Item(0).ToString = "SI" Then
            '        ruta.ImageUrl = "~/Imagenes/iconos/accept.png"
            '    Else
            '        ruta.ImageUrl = "~/Imagenes/iconos/alerta.png"
            '    End If
            '    ruta.PostBackUrl = "frm_aprobaciones_ruta.aspx?IdType=" & itemD("id_tipoDocumento").Text 'e.Item.Cells(4).Text.ToString

            '    If itemD("visibleBound").Text = "SI" Then 'e.Item.Cells(9).Text.ToString = "SI" Then
            '        visible.ToolTip = "Visible"
            '        visible.Checked = True
            '    Else
            '        visible.Checked = False
            '        visible.ToolTip = "Hidden"
            '    End If


        End If


    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/TimeSheet/frm_TimeSheetAdd.aspx?lv=0")
    End Sub


    Protected Sub btn_Period_Report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Period_Report.Click

        Dim url As String = "/TimeSheet/frm_TimeSheetFollowingREP_pay?y=" & cmb_year.SelectedValue.ToString & "&m=" & cmb_Month.SelectedValue.ToString

        Dim s As String = "window.open('" & url & "', '_blank');"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "TimeSheet Report", s, True)

    End Sub




    Private Sub Botom_testing_Click(sender As Object, e As EventArgs) Handles Botom_testing.Click

        'Dim regionalizacionCulture As CultureInfo = cl_user.regionalizacionCulture
        'Dim id_programa As Integer = 2

        'Dim cl_Noti_Process As New APPROVAL.notification_proccess(id_programa, 1007, regionalizacionCulture)

        'Dim tbl_UserPending As DataTable
        'Dim strResult As String = "Starting to Send emails...."

        ''--StandBy for Share Roles, send to the originator 
        'tbl_UserPending = cl_Noti_Process.get_notification_Pending_Summary()

        'For Each dtR As DataRow In tbl_UserPending.Rows

        '    'cl_Noti_Process.Notify_App_ByOriginator(dtR("ID"), System.AppDomain.CurrentDomain.BaseDirectory)
        '    'strResult &= String.Format("Sending to Originator:{0} {1} Pending Approval (StandBy) <b /> ", dtR("ID"), dtR("N"))
        '    cl_Noti_Process.Notify_Reminder_Proc(dtR("id_usuario"), System.AppDomain.CurrentDomain.BaseDirectory)
        '    '**************TESTING PRUPORSES
        '    Exit For
        '    '**************TESTING PRUPORSES


        'Next


        '********************************************************APPROVAL NOTIFICATION******************************************************************************
        '********************************************************APPROVAL NOTIFICATION******************************************************************************

        'Dim id_documento As Integer = 7886
        'Dim id_AppDocumento As Integer = 11856

        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 5, cl_user.regionalizacionCulture, id_AppDocumento)
        'If (objEmail.Emailing_APPROVAL_STEP(id_AppDocumento)) Then

        'Else 'Error mandando Email
        'End If

        '********************************************************APPROVAL NOTIFICATION******************************************************************************
        '********************************************************APPROVAL NOTIFICATION******************************************************************************


        '***********************************************************************************************************************************************************
        '*******************************************************SCHEDULED TESTING*****************************************************************************
        '***********************************************************************************************************************************************************

        Dim booEXEC As Boolean = False

        Dim id_programa As Integer = 2
        Dim cl_scheduled As APPROVAL.cls_scheduled = New APPROVAL.cls_scheduled(id_programa)

        Dim tbl_scheduled_ = cl_scheduled.get_scheduled_
        Dim id_frequency_range As Integer = 0

        For Each dr_Scheduled As DataRow In tbl_scheduled_.Rows

            If Not booEXEC Then

                id_frequency_range = test_scheduled(dr_Scheduled("id_scheduled"))

                If id_frequency_range > 0 Then

                    booEXEC = True

                    Select Case dr_Scheduled("id_notification")

                        Case 1007 'notification pending approval
                            FUNC_Notificate_pending(dr_Scheduled("id_notification"), dr_Scheduled("id_programa"), id_frequency_range)
                        Case 1008 'Notification deliverables
                            'FUNC_Notificate_pending(dr_Scheduled("id_notification"), dr_Scheduled("id_programa"), id_frequency_range)
                    End Select

                    '   booEXEC = False 'Enabled again the notification sending

                End If

            Else

                Exit For 'Is already running a scheduled

            End If

        Next

        '***********************************************************************************************************************************************************
        '***********************************************************************************************************************************************************
        '***********************************************************************************************************************************************************


    End Sub


    Public Function FUNC_Notificate_pending(ByVal id_notification As Integer, ByVal id_programa As Integer, ByVal id_frequency_range As Integer) As Boolean


        Dim cl_scheduled As APPROVAL.cls_scheduled = New APPROVAL.cls_scheduled(id_programa)
        Dim timezoneDate As DateTime
        Dim timezoneUTC As Integer
        Dim regionalizacionCulture As CultureInfo
        Dim cProgram As New RMS.cls_Program

        Try


            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
            regionalizacionCulture = New CultureInfo(cProgram.getprogramField("codigo_regionalizacion", "id_programa", id_programa))
            timezoneDate = DateAdd(DateInterval.Hour, timezoneUTC, Date.UtcNow)

            '**********************************YLA PROCESSS*******************************************************
            Dim cl_Noti_Process As New APPROVAL.notification_proccess(id_programa, id_notification, regionalizacionCulture)
            '**********************************YLA PROCESSS*******************************************************

            '**********************************AGM PROCESSS**************************************************
            'Dim cl_Noti_Process As New APPROVAL.notification_proccess(id_programa, 8, regionalizacionCulture)
            '**********************************YLA PROCESSS***************************************************

            Dim tbl_UserPending As DataTable

            '************TMP
            ' EventLog1.WriteEntry(String.Format("{0} -- Starting to Send emails....", Date.UtcNow))

            tbl_UserPending = cl_Noti_Process.get_notification_Pending_Summary()
            Dim tbl_DocsPendingALL As DataTable = cl_Noti_Process.get_notification_Pending_ByUser(0, "NNN", "--none--", 0) '--to Getting the EmailList for the Documents

            Dim idUsr As Integer = 0

            For Each dtR_Pend As DataRow In tbl_UserPending.Rows

                ''************TMP
                'EventLog1.WriteEntry(String.Format("Sending to ({0})--{1}--{2}--> Pending Approval: {3} Days Delayed: {4} ", dtR_Pend("Title"), dtR_Pend("nombre_usuario"), dtR_Pend("email"), dtR_Pend("N"), dtR_Pend("AVG_Days")))

                idUsr = dtR_Pend("id_usuario")
                cl_Noti_Process.Notify_Reminder_Proc(idUsr, System.AppDomain.CurrentDomain.BaseDirectory, tbl_DocsPendingALL.Copy)

                '**************TESTING PRUPORSES
                'Exit For
                '**************TESTING PRUPORS ES

            Next

            '************TMP
            'EventLog1.WriteEntry(String.Format("<br /><br /> {0} -- Finished proccess!!", Date.UtcNow))

            cl_scheduled.set_t_scheduled_Done(0)
            cl_scheduled.set_t_scheduled_DoneFIELDS("id_scheduled_frecuency_range", id_frequency_range, "id_scheduled_done", 0)
            cl_scheduled.set_t_scheduled_DoneFIELDS("sch_done_date", Date.UtcNow, "id_scheduled_done", 0)

            If cl_scheduled.save_t_scheduled_Done() <> -1 Then 'Error occured
                FUNC_Notificate_pending = False
            Else
                FUNC_Notificate_pending = True
            End If

            booEXEC = False 'Enabled again the notification sending

        Catch ex As Exception

            FUNC_Notificate_pending = False
            booEXEC = False 'Enabled again the notification sending
            '******TMP
            'EventLog1.WriteEntry(String.Format("{0} -- Error running the notification process for.... ""FUNC_Notificate_pending"" {1}", Date.UtcNow, ex.Message))

        End Try

    End Function

    Public Function test_scheduled(ByVal id_scheduled As Integer) As Integer

        Dim id_programa As Integer = 2
        Dim cl_scheduled As APPROVAL.cls_scheduled = New APPROVAL.cls_scheduled(id_programa)
        Dim timezoneDate As DateTime
        Dim timezoneUTC As Integer
        Dim regionalizacionCulture As CultureInfo
        Dim cProgram As New RMS.cls_Program
        Dim id_scheduled_frecuency_id As Integer = 0

        Try


            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
            regionalizacionCulture = New CultureInfo(cProgram.getprogramField("codigo_regionalizacion", "id_programa", id_programa))
            timezoneDate = DateAdd(DateInterval.Hour, timezoneUTC, Date.UtcNow)

            Dim vToday As Integer = timezoneDate.Day 'Just for this day effect

            Dim vDay As Integer = timezoneDate.DayOfWeek 'Date according to the timezone
            Dim vHour As Integer = timezoneDate.Hour 'Hour according to the timezone
            Dim vMinutes As Integer = timezoneDate.Minute 'Minutes according to the timezone

            Dim vYear As Integer = timezoneDate.Year 'Just for this day effect
            Dim vMonth As Integer = timezoneDate.Month 'Just for this day effect

            '--FIRST we need to seek the scheduled pending
            '--Then see the frecuency corresponding to the datetime
            '--See if has not already done
            '--Example of weekly scheduled

            '----GETTING THIS FROM DATABASE--
            ' Dim scheduled_recuency_item() As Integer = {1, 3, 5}

            Dim tbl_scheduled_recuency As DataTable = cl_scheduled.get_t_scheduled_frequency(id_scheduled)
            Dim tbl_scheduled_time As DataTable

            Dim bndNotification As Boolean = False

            'Verify Notification

            For Each dtRow As DataRow In tbl_scheduled_recuency.Rows

                If dtRow("id_type_frequency") = 1 Then 'Weekly

                    If vDay = dtRow("scheduled_frequency_day") Then '0-6

                        tbl_scheduled_time = cl_scheduled.get_t_scheduled_frequency(id_scheduled, vDay) ' for timing

                        For Each dtRow_time As DataRow In tbl_scheduled_time.Rows

                            If dtRow_time("scheduled_frequency_hour") <= vHour Then 'we are in the hour

                                If cl_scheduled.check_scheduled_by_Time(id_scheduled, vToday, vMonth, vYear, vHour, vMinutes, timezoneUTC) = 0 Then 'check if already are a notification in the time

                                    bndNotification = True
                                    id_scheduled_frecuency_id = dtRow_time("id_scheduled_frequency")
                                    Exit For

                                End If

                            End If

                        Next


                    End If

                ElseIf dtRow("id_type_frequency") = 2 Then 'Monthly

                    If vToday = dtRow("scheduled_frequency_day") Then '1-31

                        tbl_scheduled_time = cl_scheduled.get_t_scheduled_frequency(id_scheduled, vToday) ' for timing

                        For Each dtRow_time As DataRow In tbl_scheduled_time.Rows

                            If dtRow_time("scheduled_frequency_hour") <= vHour Then 'we are in the hour

                                If cl_scheduled.check_scheduled_by_Time(id_scheduled, vToday, vMonth, vYear, vHour, vMinutes, timezoneUTC) = 0 Then 'check if already are a notification in the time

                                    bndNotification = True
                                    id_scheduled_frecuency_id = dtRow_time("id_scheduled_frequency")
                                    Exit For

                                End If

                            End If

                        Next

                    End If

                End If

                If bndNotification Then
                    Exit For
                End If

            Next

            test_scheduled = id_scheduled_frecuency_id

            ''--Example of weekly scheduled
            'Dim i As Integer = 0
            'For Each vD As Integer In days

            '    'from zero (which indicates F:System.DayOfWeek.Sunday) to six (which indicates F:System.DayOfWeek.Saturday).
            '    If vD = vDay Then

            '        '--Just suposed that is once at day
            '        id_scheduled_fecuency = scheduled_recuency_item(i)
            '        'We need to make the logicals changes  according each type of frecuency for this case is dialy once time by day

            '        'Figurated out, how can we get it, dialy, weekly, yealy
            '        Dim maxNumberOfHappendByDay As Integer = 1 'Just for test option

            '        '******************************************** for this case if don´t registered ******************************************************************
            '        If cl_scheduled.check_scheduled_byDaily(id_scheduled_fecuency, vToday, vMonth, vYear) < maxNumberOfHappendByDay Then 'Just for this asumptions id don´t we sould to find the numbers of times how occurs in the day for his case

            '            'looking for the hours and minutes if is time
            '            If ((Hour(i) = vHour) And (min(i) <= vMinutes)) Then 'this is the hour and minutes
            '                check_scheduled = True
            '            ElseIf ((Hour(i) < vHour)) Then 'the hour has passed don´t care the minutes
            '                check_scheduled = True
            '            End If

            '        End If
            '        '******************************************** for this case if don´t registered ******************************************************************

            '        Exit For 'once to find go out


            '    End If

            '    i += 1

            'Next

        Catch ex As Exception

            test_scheduled = id_scheduled_frecuency_id
            '*******TMP
            'EventLog1.WriteEntry(String.Format("{0} -- Error running the scheduled search process .... ""test_scheduled"" {1}", Date.UtcNow, ex.Message))

        End Try

    End Function


    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        LoadGrid(False)
    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        Dim id_ts = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_timesheet").ToString()

        Dim result As String = cls_TimeSheet.DeleteTimeSheet(id_ts)

        If result = "0" Then
            'Me.Response.Redirect("~/TimeSheet/frm_TimeSheet.aspx")
        Else
            Me.lbl_text_Error.Text = result
        End If


    End Sub

    Private Sub btn_leave_approval_Click(sender As Object, e As EventArgs) Handles btn_leave_approval.Click

        Me.Response.Redirect("~/TimeSheet/frm_TimeSheetAdd.aspx?lv=1")

    End Sub
End Class
