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



Partial Class frm_TimeSheetEdit
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
    Public TOThrs As Decimal
    Public TOTloe As Decimal

    Dim lsBill_id As New List(Of Integer)
    Dim excepciones As New List(Of Integer)

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

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtBill.Value = String.Format("dtBill{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            hd_IDtimeSheet.Value = Request.QueryString("ID")

            Session(Me.hd_dtBill.Value) = lsBill_id

            loadData()
            loadGRID(1)

            'Me.lblJobTittle.Text = ""
            'Me.lbl_IdEmpleado.Text = 0

        Else

            lsBill_id = Session(Me.hd_dtBill.Value)

            'If Me.grd_cate.Items.Count() > 0 Then
            '    Me.cmb_rol.Enabled = False
            'Else
            '    Me.cmb_rol.Enabled = True
            'End If

        End If


    End Sub


    Public Sub addBill_toList(ByVal idBill As Integer)

        lsBill_id.Add(idBill)
        Session(Me.hd_dtBill.Value) = lsBill_id

    End Sub


    Public Sub loadData()
        Using dbEntities As New dbRMS_JIEntities
            cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

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

            Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)
            Dim usuario = dbEntities.t_usuarios.Find(timesheet.id_usuario)
            If usuario.fecha_contrato IsNot Nothing Then
                Me.anio.Value = usuario.fecha_contrato.Value.Year
                Me.mes.Value = usuario.fecha_contrato.Value.Month
                Me.dia.Value = usuario.fecha_contrato.Value.Day
            End If
            Me.hd_leave.Value = Convert.ToInt32(timesheet.ts_leave_update)
            Me.anio_ts.Value = timesheet.anio
            Me.mes_ts.Value = timesheet.mes
            userName = timesheet.nombre_usuario
            userJOB_tittle = timesheet.job
            Month_TS = MonthName(timesheet.mes)
            hd_month.Value = timesheet.mes
            Year_TS = timesheet.anio
            hd_year.Value = timesheet.anio
            Dim mes = Convert.ToInt32(hd_month.Value)
            Dim anio = Convert.ToInt32(hd_year.Value)
            Status_TS = timesheet.timesheet_estado

            Dim excepcionesList = dbEntities.vw_tme_ts_excepciones.Where(Function(p) p.id_mes = mes And p.anio = anio).Select(Function(p) p.dia).ToList()
            Session("excepciones") = excepcionesList


            Me.hd_id_employee_type.Value = timesheet.id_employee_type
            Me.lbl_employeeType.Text = timesheet.employee_type

            If IsDBNull(timesheet.fecha_upd) Then
                DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_upd, "m", timezoneUTC, False))
                HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_upd, timezoneUTC, True))
            Else
                DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_creo, "m", timezoneUTC, False))
                HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_creo, timezoneUTC, True))
            End If

            txt_description.Text = timesheet.description.Trim
            txt_notes.Text = timesheet.notes.Trim

            Me.cmb_billable_Type.DataSource = cls_TimeSheet.get_ta_billable_type()
            Me.cmb_billable_Type.DataTextField = "billable_time_type"
            Me.cmb_billable_Type.DataValueField = "id_billable_time_type"
            Me.cmb_billable_Type.SelectedIndex = -1
            Me.cmb_billable_Type.DataBind()

            Me.anchorInformation.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorInformation.Attributes.Add("href", "frm_TimeSheetAdd.aspx?ID=" & hd_IDtimeSheet.Value)

            Me.anchorBillable.Attributes.Add("class", "btn btn-primary btn-circle")

            If timesheet.id_timesheet_estado = 1 Then

                Me.anchorSupportDocs.Attributes.Add("class", "btn btn-default btn-circle disabled")
                Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

            Else

                Me.anchorSupportDocs.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorSupportDocs.Attributes.Add("href", "frm_TimeSheetDocs.aspx?ID=" & hd_IDtimeSheet.Value)

                Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & hd_IDtimeSheet.Value)

            End If

            If timesheet.id_timesheet_estado = 5 Or timesheet.id_timesheet_estado = 1 Then 'Created and Observation Pending
                Me.btnlk_save.Enabled = True
            Else
                Me.btnlk_save.Enabled = False
            End If
        End Using


    End Sub


    Public Sub loadGRID(ByVal OPT As Integer)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)
        Dim DaysInMonth As Integer = Date.DaysInMonth(timesheet.anio, timesheet.mes)

        If OPT = 1 Then
            Me.grd_cate.DataSource = cls_TimeSheet.get_Table(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, Me.hd_leave.Value) 'The Original Template
        Else
            'Me.grd_cate.DataSource = cls_TimeSheet.get_Table(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, cmb_billable_Item.SelectedValue) ' To adding extra Item
            Me.grd_cate.DataSource = cls_TimeSheet.get_Table(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, Me.hd_leave.Value, lsBill_id) ' To adding extra Item

        End If

        Me.grd_cate.DataBind()

        '************CHANGE FOR OTHER KIND OF PERIOD***********
        Dim nameCol As String
        For i = DaysInMonth + 1 To 31
            nameCol = String.Format("colm_d{0}", i)
            Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
        Next
        '************CHANGE FOR OTHER KIND OF PERIOD***********


        '***************LOOK FOR THE LEAVE APPROVALS*************************************

        Dim LeaveApprovals As List(Of Integer) = cls_TimeSheet.getLeaveAPP(timesheet.id_usuario, timesheet.mes)

        Dim TSstatus As New List(Of Integer) From {1, 2, 5}

        If LeaveApprovals.Count > 0 And TSstatus.Contains(timesheet.id_timesheet_estado) Then

            For Each IDtimesheet As Integer In LeaveApprovals
                LoadLeave(IDtimesheet, timesheet.ts_leave_update)
            Next

        End If

        '***************LOOK FOR THE LEAVE APPROVALS*************************************

        'Load the Time Sheet
        LoadTimeSheet()


    End Sub

    Protected Sub btnlk_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnlk_save.Click

        Try


            Dim err As Boolean = False
            Dim ext As String = ""

            Dim DaysInMonth As Integer = Date.DaysInMonth(hd_year.Value, hd_month.Value)
            Dim NameCol As String = ""
            Dim NameControl As String = ""
            cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
            Dim OldVal As Integer
            Dim result As String = ""

            Using db As New dbRMS_JIEntities

                If err = False Then

                    Dim TimeSheet As ta_timesheet = db.ta_timesheet.Find(Convert.ToInt32(hd_IDtimeSheet.Value))

                    'TimeSheet.id_usuario = 
                    'TimeSheet.anio = 
                    'TimeSheet.mes = 
                    TimeSheet.description = txt_description.Text.Trim
                    TimeSheet.notes = txt_notes.Text.Trim
                    'TimeSheet.id_programa =
                    'TimeSheet.fecha_creo = 
                    'TimeSheet.usuario_creo = 
                    TimeSheet.fecha_upd = Date.UtcNow
                    TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                    'TimeSheet.id_timesheet_estado = 

                    result = cls_TimeSheet.SaveTimeSheet(TimeSheet, hd_IDtimeSheet.Value)

                    If result = -1 Then
                        Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", result)
                        Me.lblError.Visible = True
                        err = True
                    End If

                    If err = False Then


                        For Each DTrow As GridDataItem In Me.grd_cate.Items

                            ' CType(itemD("colm_d1").FindControl("txt_d1"), RadNumericTextBox)
                            ' txtRadNumeric31.Value = Val(itemD("31").Text

                            For i = 1 To DaysInMonth 'looking for values

                                NameCol = String.Format("colm_d{0}", i.ToString)
                                NameControl = String.Format("txt_d{0}", i.ToString)

                                Dim txtDayValue As RadNumericTextBox = CType(DTrow(NameCol).FindControl(NameControl), RadNumericTextBox)
                                OldVal = Val(DTrow(i.ToString).Text)

                                If ((txtDayValue.Value > 0)) Then 'Save IT

                                    Dim TimeSheetDetail As New ta_timesheet_detail

                                    TimeSheetDetail.id_timesheet = hd_IDtimeSheet.Value
                                    TimeSheetDetail.id_billable_time = CType(DTrow("id_billable_time").Text, Integer)
                                    TimeSheetDetail.dia = i
                                    TimeSheetDetail.hours = Val(txtDayValue.Value)
                                    TimeSheetDetail.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser"))
                                    TimeSheetDetail.fecha_creo = Date.UtcNow

                                    result = cls_TimeSheet.SaveTimeSheetDetail(TimeSheetDetail, hd_IDtimeSheet.Value, CType(DTrow("id_billable_time").Text, Integer))

                                    If result = -1 Then

                                        Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", result)
                                        Me.lblError.Visible = True
                                        Exit For

                                    End If

                                ElseIf ((txtDayValue.Value = 0) Or (OldVal > 0)) Then

                                    result = cls_TimeSheet.DeleteTimeSheetDetail(i, hd_IDtimeSheet.Value, CType(DTrow("id_billable_time").Text, Integer))

                                End If

                            Next

                        Next

                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto

                        Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

                        If Not IsNothing(summaryTIMEsheet) Then

                            If summaryTIMEsheet.LOE > 0 Then
                                Me.MsgGuardar.Redireccion = String.Format("~/TimeSheet/frm_TimeSheetDocs.aspx?ID={0}", hd_IDtimeSheet.Value)
                            Else
                                Me.MsgGuardar.Redireccion = String.Format("~/TimeSheet/frm_TimeSheetEdit.aspx?ID={0}", hd_IDtimeSheet.Value)
                            End If

                        Else

                            Me.MsgGuardar.Redireccion = String.Format("~/TimeSheet/frm_TimeSheetEdit.aspx?ID={0}", hd_IDtimeSheet.Value)

                        End If

                        Session.Remove(Me.hd_dtBill.Value)
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                    End If

                    '    'Me.Label3.Text = ext.ToString
                    '    Dim Sql = "INSERT INTO ta_grupos(nombre_grupo, descripcion_grupo, id_programa) VALUES ('" & Me.txt_grupo.Text.Trim & "', '" & Me.txt_des.Text.Trim & "', " & Me.Session("E_IDPrograma") & ") "
                    '    Sql &= " SELECT @@IDENTITY"
                    '    cnnSAP.Open()
                    '    Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                    '    Dim ds As New DataSet("IdPlan")
                    '    dm.Fill(ds, "IdPlan")
                    '    Dim id_grupo = ds.Tables("IdPlan").Rows(0).Item(0)

                    '    '*****************************GUARDANDO LOS DATOS EN ta_gruposRoles el detalle de miembros del grupo****************************
                    '    Sql = " INSERT INTO ta_gruposRoles SELECT " & id_grupo & " AS id_grupo, id_rol, id_usuario, aprueba, comenta, consulta FROM ta_gruposRoles_temp WHERE id_temp='" & Me.lbl_idTemp.Text & "'"
                    '    dm.SelectCommand.CommandText = Sql
                    '    dm.SelectCommand.ExecuteNonQuery()

                    '    cnnSAP.Close()
                    '    Me.Response.Redirect("~/Approvals/frm_consulta_groups.aspx")


                End If

            End Using

        Catch ex As Exception

            Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
            Me.lblError.Visible = True

        End Try


    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/TimeSheet/frm_TimeSheet.aspx")
    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound


        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            'item.DataCell.Text = "$" + groupDataRow("UnitPrice").ToString() + " (" + groupDataRow("InStock").ToString() + ")"
            item.DataCell.Text = item.DataCell.Text.Split(":")(1).ToString
        End If

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_billable_time = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_billable_time").ToString())
            Dim itemD As GridDataItem = CType(e.Item, GridDataItem)

            Dim txtRadNumeric1 As RadNumericTextBox = CType(itemD("colm_d1").FindControl("txt_d1"), RadNumericTextBox)
            Dim txtRadNumeric2 As RadNumericTextBox = CType(itemD("colm_d2").FindControl("txt_d2"), RadNumericTextBox)
            Dim txtRadNumeric3 As RadNumericTextBox = CType(itemD("colm_d3").FindControl("txt_d3"), RadNumericTextBox)
            Dim txtRadNumeric4 As RadNumericTextBox = CType(itemD("colm_d4").FindControl("txt_d4"), RadNumericTextBox)
            Dim txtRadNumeric5 As RadNumericTextBox = CType(itemD("colm_d5").FindControl("txt_d5"), RadNumericTextBox)
            Dim txtRadNumeric6 As RadNumericTextBox = CType(itemD("colm_d6").FindControl("txt_d6"), RadNumericTextBox)
            Dim txtRadNumeric7 As RadNumericTextBox = CType(itemD("colm_d7").FindControl("txt_d7"), RadNumericTextBox)
            Dim txtRadNumeric8 As RadNumericTextBox = CType(itemD("colm_d8").FindControl("txt_d8"), RadNumericTextBox)
            Dim txtRadNumeric9 As RadNumericTextBox = CType(itemD("colm_d9").FindControl("txt_d9"), RadNumericTextBox)
            Dim txtRadNumeric10 As RadNumericTextBox = CType(itemD("colm_d10").FindControl("txt_d10"), RadNumericTextBox)
            Dim txtRadNumeric11 As RadNumericTextBox = CType(itemD("colm_d11").FindControl("txt_d11"), RadNumericTextBox)
            Dim txtRadNumeric12 As RadNumericTextBox = CType(itemD("colm_d12").FindControl("txt_d12"), RadNumericTextBox)
            Dim txtRadNumeric13 As RadNumericTextBox = CType(itemD("colm_d13").FindControl("txt_d13"), RadNumericTextBox)
            Dim txtRadNumeric14 As RadNumericTextBox = CType(itemD("colm_d14").FindControl("txt_d14"), RadNumericTextBox)
            Dim txtRadNumeric15 As RadNumericTextBox = CType(itemD("colm_d15").FindControl("txt_d15"), RadNumericTextBox)
            Dim txtRadNumeric16 As RadNumericTextBox = CType(itemD("colm_d16").FindControl("txt_d16"), RadNumericTextBox)
            Dim txtRadNumeric17 As RadNumericTextBox = CType(itemD("colm_d17").FindControl("txt_d17"), RadNumericTextBox)
            Dim txtRadNumeric18 As RadNumericTextBox = CType(itemD("colm_d18").FindControl("txt_d18"), RadNumericTextBox)
            Dim txtRadNumeric19 As RadNumericTextBox = CType(itemD("colm_d19").FindControl("txt_d19"), RadNumericTextBox)
            Dim txtRadNumeric20 As RadNumericTextBox = CType(itemD("colm_d20").FindControl("txt_d20"), RadNumericTextBox)
            Dim txtRadNumeric21 As RadNumericTextBox = CType(itemD("colm_d21").FindControl("txt_d21"), RadNumericTextBox)
            Dim txtRadNumeric22 As RadNumericTextBox = CType(itemD("colm_d22").FindControl("txt_d22"), RadNumericTextBox)
            Dim txtRadNumeric23 As RadNumericTextBox = CType(itemD("colm_d23").FindControl("txt_d23"), RadNumericTextBox)
            Dim txtRadNumeric24 As RadNumericTextBox = CType(itemD("colm_d24").FindControl("txt_d24"), RadNumericTextBox)
            Dim txtRadNumeric25 As RadNumericTextBox = CType(itemD("colm_d25").FindControl("txt_d25"), RadNumericTextBox)
            Dim txtRadNumeric26 As RadNumericTextBox = CType(itemD("colm_d26").FindControl("txt_d26"), RadNumericTextBox)
            Dim txtRadNumeric27 As RadNumericTextBox = CType(itemD("colm_d27").FindControl("txt_d27"), RadNumericTextBox)
            Dim txtRadNumeric28 As RadNumericTextBox = CType(itemD("colm_d28").FindControl("txt_d28"), RadNumericTextBox)
            Dim txtRadNumeric29 As RadNumericTextBox = CType(itemD("colm_d29").FindControl("txt_d29"), RadNumericTextBox)
            Dim txtRadNumeric30 As RadNumericTextBox = CType(itemD("colm_d30").FindControl("txt_d30"), RadNumericTextBox)
            Dim txtRadNumeric31 As RadNumericTextBox = CType(itemD("colm_d31").FindControl("txt_d31"), RadNumericTextBox)

            Dim txtRadNumericTotal As RadNumericTextBox = CType(itemD("Total").FindControl("txt_all"), RadNumericTextBox)

            If Not CType(itemD("visible").Text, Boolean) = True Then

                itemD("colm_d1").Text = itemD("1").Text
                itemD("colm_d2").Text = itemD("2").Text
                itemD("colm_d3").Text = itemD("3").Text
                itemD("colm_d4").Text = itemD("4").Text
                itemD("colm_d5").Text = itemD("5").Text
                itemD("colm_d6").Text = itemD("6").Text
                itemD("colm_d7").Text = itemD("7").Text
                itemD("colm_d8").Text = itemD("8").Text
                itemD("colm_d9").Text = itemD("9").Text
                itemD("colm_d10").Text = itemD("10").Text
                itemD("colm_d11").Text = itemD("11").Text
                itemD("colm_d12").Text = itemD("12").Text
                itemD("colm_d13").Text = itemD("13").Text
                itemD("colm_d14").Text = itemD("14").Text
                itemD("colm_d15").Text = itemD("15").Text
                itemD("colm_d16").Text = itemD("16").Text
                itemD("colm_d17").Text = itemD("17").Text
                itemD("colm_d18").Text = itemD("18").Text
                itemD("colm_d19").Text = itemD("19").Text
                itemD("colm_d20").Text = itemD("20").Text
                itemD("colm_d21").Text = itemD("21").Text
                itemD("colm_d22").Text = itemD("22").Text
                itemD("colm_d23").Text = itemD("23").Text
                itemD("colm_d24").Text = itemD("24").Text
                itemD("colm_d25").Text = itemD("25").Text
                itemD("colm_d26").Text = itemD("26").Text
                itemD("colm_d27").Text = itemD("27").Text
                itemD("colm_d28").Text = itemD("28").Text
                itemD("colm_d29").Text = itemD("29").Text
                itemD("colm_d30").Text = itemD("30").Text
                itemD("colm_d31").Text = itemD("31").Text
                itemD("Total").Text = ""

                txtRadNumeric1.Visible = False
                txtRadNumeric2.Visible = False
                txtRadNumeric3.Visible = False
                txtRadNumeric4.Visible = False
                txtRadNumeric5.Visible = False
                txtRadNumeric6.Visible = False
                txtRadNumeric7.Visible = False
                txtRadNumeric8.Visible = False
                txtRadNumeric9.Visible = False
                txtRadNumeric10.Visible = False
                txtRadNumeric11.Visible = False
                txtRadNumeric12.Visible = False
                txtRadNumeric13.Visible = False
                txtRadNumeric14.Visible = False
                txtRadNumeric15.Visible = False
                txtRadNumeric16.Visible = False
                txtRadNumeric17.Visible = False
                txtRadNumeric18.Visible = False
                txtRadNumeric19.Visible = False
                txtRadNumeric20.Visible = False
                txtRadNumeric21.Visible = False
                txtRadNumeric22.Visible = False
                txtRadNumeric23.Visible = False
                txtRadNumeric24.Visible = False
                txtRadNumeric25.Visible = False
                txtRadNumeric26.Visible = False
                txtRadNumeric27.Visible = False
                txtRadNumeric28.Visible = False
                txtRadNumeric29.Visible = False
                txtRadNumeric30.Visible = False
                txtRadNumericTotal.Visible = False

                'Else

                '    txtRadNumeric1.Value = Val(itemD("1").Text)
                '    txtRadNumeric2.Value = Val(itemD("2").Text)
                '    txtRadNumeric3.Value = Val(itemD("3").Text)
                '    txtRadNumeric4.Value = Val(itemD("4").Text)
                '    txtRadNumeric5.Value = Val(itemD("5").Text)
                '    txtRadNumeric6.Value = Val(itemD("6").Text)
                '    txtRadNumeric7.Value = Val(itemD("7").Text)
                '    txtRadNumeric8.Value = Val(itemD("8").Text)
                '    txtRadNumeric9.Value = Val(itemD("9").Text)
                '    txtRadNumeric10.Value = Val(itemD("10").Text)
                '    txtRadNumeric11.Value = Val(itemD("11").Text)
                '    txtRadNumeric12.Value = Val(itemD("12").Text)
                '    txtRadNumeric13.Value = Val(itemD("13").Text)
                '    txtRadNumeric14.Value = Val(itemD("14").Text)
                '    txtRadNumeric15.Value = Val(itemD("15").Text)
                '    txtRadNumeric16.Value = Val(itemD("16").Text)
                '    txtRadNumeric17.Value = Val(itemD("17").Text)
                '    txtRadNumeric18.Value = Val(itemD("18").Text)
                '    txtRadNumeric19.Value = Val(itemD("19").Text)
                '    txtRadNumeric20.Value = Val(itemD("20").Text)
                '    txtRadNumeric21.Value = Val(itemD("21").Text)
                '    txtRadNumeric22.Value = Val(itemD("22").Text)
                '    txtRadNumeric23.Value = Val(itemD("23").Text)
                '    txtRadNumeric24.Value = Val(itemD("24").Text)
                '    txtRadNumeric25.Value = Val(itemD("25").Text)
                '    txtRadNumeric26.Value = Val(itemD("26").Text)
                '    txtRadNumeric27.Value = Val(itemD("27").Text)
                '    txtRadNumeric28.Value = Val(itemD("28").Text)
                '    txtRadNumeric29.Value = Val(itemD("29").Text)
                '    txtRadNumeric30.Value = Val(itemD("30").Text)
                '    txtRadNumeric31.Value = Val(itemD("31").Text)

            End If

            '*************DISABLED AND BACKGROUND COLOR*************
            Dim DateValue As Date
            Dim strDayName As String
            Dim nameCol As String
            Dim txtName As String
            Dim DaysInMonth As Integer = Date.DaysInMonth(hd_year.Value, hd_month.Value)
            Dim txtRadNumeric As RadNumericTextBox
            Dim excepciones As List(Of Integer) = Session("excepciones")
            For i = 1 To DaysInMonth 'Marking sunday

                DateValue = New Date(hd_year.Value, hd_month.Value, i)
                'tblRow.Item(i + 3) = DateValue.ToString("ddd")
                nameCol = String.Format("colm_d{0}", i)
                txtName = String.Format("txt_d{0}", i)

                strDayName = DateValue.ToString("ddd").Trim.ToLower.Substring(0, 3)

                '****************TEMPORARY P&B CHANGE*****************
                If (strDayName = "sáb" Or strDayName = "sat") Or (strDayName = "dom" Or strDayName = "sun") Then

                    'Me.grd_cate.MasterTableView.GetColumn(nameCol).ItemStyle.BackColor = Drawing.Color.LightGray
                    txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)

                    If Not IsNothing(txtRadNumeric) Then
                        'txtRadNumeric.ReadOnly = True
                        txtRadNumeric.Enabled = False
                        txtRadNumeric.BackColor = Drawing.Color.LightGray
                    End If

                End If






                Dim xx = Weekday(DateValue, vbFriday)
                If Weekday(DateValue, vbFriday) = 2 Or Weekday(DateValue, vbFriday) = 3 Then

                    Me.grd_cate.MasterTableView.GetColumn(nameCol).ItemStyle.BackColor = Drawing.Color.LightGray
                    txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)
                    'txtRadNumeric.BackColor = Drawing.Color.Gray
                    If Not IsNothing(txtRadNumeric) Then
                        '*****txtRadNumeric.ReadOnly = True
                        txtRadNumeric.BackColor = Drawing.Color.LightGray
                    End If

                End If

                If CType(itemD("ts_leave").Text, Boolean) = True And CType(Me.hd_leave.Value, Boolean) = False Then

                    txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)
                    If Not IsNothing(txtRadNumeric) Then
                        txtRadNumeric.BackColor = Drawing.Color.LightGray
                        txtRadNumeric.ReadOnly = True
                    End If

                End If

                If excepciones.Where(Function(p) p = i).Count() > 0 Then
                    txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)
                    If Not IsNothing(txtRadNumeric) Then
                        Me.grd_cate.MasterTableView.GetColumn(nameCol).ItemStyle.BackColor = Drawing.Color.LightGray
                        txtRadNumeric.BackColor = Drawing.Color.LightGray
                        txtRadNumeric.Enabled = False
                    End If
                End If

                txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)
                Dim diaV = Convert.ToInt32(Me.dia.Value)
                Dim mesV = Convert.ToInt32(Me.mes.Value)
                Dim anioV = Convert.ToInt32(Me.anio.Value)
                Dim mesTS = Convert.ToInt32(Me.mes_ts.Value)
                Dim anioTS = Convert.ToInt32(Me.anio_ts.Value)

                Dim setHora As Boolean = True
                If diaV <> 0 And mesV <> 0 And anioV <> 0 Then
                    If anioV = anioTS And mesV = mesTS And i < diaV Then
                        setHora = False
                    End If
                End If


                If Not IsNothing(txtRadNumeric) Then
                    Dim txtRadNumericAll = CType(itemD("Total").FindControl("txt_all"), RadNumericTextBox)
                    If txtRadNumeric.Value Is Nothing And Weekday(DateValue, vbFriday) <> 2 And Weekday(DateValue, vbFriday) <> 3 And excepciones.Where(Function(p) p = i).Count() = 0 And id_billable_time = 3 Then
                        If setHora Then
                            txtRadNumeric.Value = 8
                        End If
                    ElseIf txtRadNumeric.Value Is Nothing And Weekday(DateValue, vbFriday) <> 2 And Weekday(DateValue, vbFriday) <> 3 And excepciones.Where(Function(p) p = i).Count() > 0 And id_billable_time = 14 Then
                        If setHora Then
                            txtRadNumeric.Value = 8
                        End If
                    End If
                End If
            Next

            'For i = DaysInMonth + 1 To 3
            '    nameCol = String.Format("colm_d{0}", i)
            '    Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
            'Next

        End If



    End Sub




    <Web.Services.WebMethod()>
    Public Shared Function GetBillableOptions(ByVal idBillType As Integer, ByVal idPrograma As Integer, ByVal idTimeSheet As Integer, ByVal ts_leave As Integer) As Object

        Dim jsonITEMS As String = "[]"

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(idPrograma)
        Dim ResultItems As Object = cls_TimeSheet.get_ta_billable_Option(idBillType, idTimeSheet, ts_leave)

        Dim serializer As New JavaScriptSerializer()
        If ResultItems.Count() > 0 Then
            jsonITEMS = serializer.Serialize(ResultItems)
        End If


        Return jsonITEMS

    End Function


    Private Sub bntlk_addItem_Click(sender As Object, e As EventArgs) Handles bntlk_addItem.Click

        'Me.MsgReturn.Redireccion = ""
        'Me.MsgReturn.TituMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "TITTLE_CONFIRM").texto
        'Me.MsgReturn.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "MSG_CONFIRM").texto
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)

        Dim vFoundIT As Boolean = False
        For Each Irow As GridDataItem In Me.grd_cate.Items

            If TypeOf Irow Is GridDataItem Then

                Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

                If cmb_billable_Item.SelectedValue = idBillableTime Then
                    vFoundIT = True
                    Exit For
                End If

            End If

        Next


        If Not vFoundIT Then

            ' loadData()
            addBill_toList(cmb_billable_Item.SelectedValue)
            Me.cmb_billable_Item.ClearSelection()
            Me.cmb_billable_Item.Text = ""
            Me.cmb_billable_Type.ClearSelection()
            loadGRID(2)

            'Else 'Its already exist
        End If


    End Sub

    Sub LoadTimeSheet()

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        Dim registeredTS As DataTable = cls_TimeSheet.getTimeSheetDetail(hd_IDtimeSheet.Value)
        Dim dia As Integer = 0
        Dim nameCol As String
        Dim txtName As String
        Dim txtRadNumeric As RadNumericTextBox

        Dim TSstatus As New List(Of Integer) From {1, 2, 5}
        Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)


        If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

            For Each Irow As GridDataItem In Me.grd_cate.Items
                If TypeOf Irow Is GridDataItem Then
                    For number As Integer = 1 To 31 Step 1
                        Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

                        If idBillableTime = 3 Then
                            nameCol = String.Format("colm_d{0}", number.ToString.Trim)
                            txtName = String.Format("txt_d{0}", number.ToString.Trim)
                            Dim txtRadNumeric2 = CType(Irow("Total").FindControl("txt_all"), RadNumericTextBox)
                            txtRadNumeric = CType(Irow(nameCol).FindControl(txtName), RadNumericTextBox)

                            If txtRadNumeric IsNot Nothing Then
                                txtRadNumeric.Text = String.Empty
                            End If
                            If txtRadNumeric2 IsNot Nothing Then
                                txtRadNumeric2.Text = String.Empty
                            End If
                        End If
                    Next
                End If
            Next


            For Each Irow As GridDataItem In Me.grd_cate.Items

                If TypeOf Irow Is GridDataItem Then

                    Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

                    ' Convert.ToInt32(Irow("ts_leave").Text)

                    For Each Drow In registeredTS.Rows

                        If idBillableTime = Drow("id_billable_time") Then

                            dia = Drow("dia")
                            nameCol = String.Format("colm_d{0}", dia.ToString.Trim)
                            txtName = String.Format("txt_d{0}", dia.ToString.Trim)
                            Dim txtRadNumeric2 = CType(Irow("Total").FindControl("txt_all"), RadNumericTextBox)
                            txtRadNumeric = CType(Irow(nameCol).FindControl(txtName), RadNumericTextBox)

                            If Convert.ToInt32(Me.hd_leave.Value) = 0 Then

                                If Convert.ToInt32(Irow("ts_leave").Text) = 0 Then

                                    txtRadNumeric.Value = Convert.ToDecimal(Drow("hours"))
                                    Irow(dia).Text = Convert.ToDecimal(Drow("hours"))

                                Else

                                    If TSstatus.Contains(timesheet.id_timesheet_estado) Then

                                        'Leave values
                                        If IsNothing(txtRadNumeric.Value) Then

                                            txtRadNumeric.Value = 0.0
                                            Irow(dia).Text = 0.0

                                        End If

                                    Else

                                        'TS values Final values
                                        txtRadNumeric.Value = Convert.ToDecimal(Drow("hours"))
                                        Irow(dia).Text = Convert.ToDecimal(Drow("hours"))

                                    End If

                                End If

                            Else

                                'Leave Values
                                txtRadNumeric.Value = Convert.ToDecimal(Drow("hours"))
                                Irow(dia).Text = Convert.ToDecimal(Drow("hours"))

                            End If

                        End If

                    Next

                End If

            Next

        End If

        Dim summarizedTIMEsheet As DataTable = cls_TimeSheet.get_TimeSheet_Summary(hd_IDtimeSheet.Value)
        reptTable.DataSource = summarizedTIMEsheet
        reptTable.DataBind()

        Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

        If Not IsNothing(summaryTIMEsheet) Then

            TOThrs = summaryTIMEsheet.TOThours
            TOTloe = summaryTIMEsheet.LOE
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & hd_IDtimeSheet.Value)

        Else

            TOThrs = 0
            TOTloe = 0
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        End If

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "setTimeout(CalculateATall, 100);", True)

    End Sub



    Sub LoadLeave(ByVal idTS As Integer, ByVal isLeaveApprovan As Boolean)

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        Dim registeredTS As DataTable = cls_TimeSheet.getTimeSheetDetail(idTS)
        Dim dia As Integer = 0
        Dim nameCol As String
        Dim txtName As String
        Dim txtRadNumeric As RadNumericTextBox
        Dim bndFind As Boolean = False
        Dim MaxWorkedHours As Decimal = 8.0 'Handled the max hours to report


        If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

            For Each Irow As GridDataItem In Me.grd_cate.Items

                If TypeOf Irow Is GridDataItem Then

                    Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

                    For Each Drow In registeredTS.Rows

                        If idBillableTime = Drow("id_billable_time") Then

                            dia = Drow("dia")
                            nameCol = String.Format("colm_d{0}", dia.ToString.Trim)
                            txtName = String.Format("txt_d{0}", dia.ToString.Trim)

                            txtRadNumeric = CType(Irow(nameCol).FindControl(txtName), RadNumericTextBox)
                            If Not IsNothing(txtRadNumeric.Value) Then
                                txtRadNumeric.Value = Convert.ToDecimal(txtRadNumeric.Value) + Convert.ToDecimal(Drow("hours"))
                                Irow(dia).Text = Convert.ToDecimal(txtRadNumeric.Value) + Convert.ToDecimal(Drow("hours"))
                            Else
                                '**************************Deshabilitar*****************

                                If isLeaveApprovan = False Then
                                    txtRadNumeric.Value = Convert.ToDecimal(Drow("hours"))
                                    Irow(dia).Text = Convert.ToDecimal(Drow("hours"))
                                End If
                            End If
                            ' txtRadNumeric.Enabled = False
                            If isLeaveApprovan = False Then
                                txtRadNumeric.BackColor = Drawing.Color.LightSalmon
                            End If

                            bndFind = True

                        End If

                    Next

                End If

            Next

        End If


        If Not bndFind Then '********************* not found it by id_billable***********************

            If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

                For Each Drow In registeredTS.Rows


                    For Each Irow As GridDataItem In Me.grd_cate.Items

                        If TypeOf Irow Is GridDataItem Then

                            Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

                            If idBillableTime = Drow("id_billable_time") Then

                                dia = Drow("dia")
                                nameCol = String.Format("colm_d{0}", dia.ToString.Trim)
                                txtName = String.Format("txt_d{0}", dia.ToString.Trim)

                                txtRadNumeric = CType(Irow(nameCol).FindControl(txtName), RadNumericTextBox)
                                If Not IsNothing(txtRadNumeric.Value) Then
                                    txtRadNumeric.Value = Convert.ToDecimal(txtRadNumeric.Value) + Convert.ToDecimal(Drow("hours"))
                                    Irow(dia).Text = Convert.ToDecimal(txtRadNumeric.Value) + Convert.ToDecimal(Drow("hours"))
                                Else
                                    txtRadNumeric.Value = Convert.ToDecimal(Drow("hours"))
                                    Irow(dia).Text = Convert.ToDecimal(Drow("hours"))
                                End If
                                ' txtRadNumeric.Enabled = False
                                txtRadNumeric.BackColor = Drawing.Color.LightSalmon

                                bndFind = True

                            End If


                        End If

                    Next

                Next

            End If

        End If

        'Dim summarizedTIMEsheet As DataTable = cls_TimeSheet.get_TimeSheet_Summary(hd_IDtimeSheet.Value)
        'reptTable.DataSource = summarizedTIMEsheet
        'reptTable.DataBind()

        'Dim summaryTIMEsheet As vw_time_sheet_total = cls_TimeSheet.get_SummaryTimeSheet(hd_IDtimeSheet.Value)

        'If Not IsNothing(summaryTIMEsheet) Then

        '    TOThrs = summaryTIMEsheet.TOThours
        '    TOTloe = summaryTIMEsheet.LOE
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
        '    Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & hd_IDtimeSheet.Value)

        'Else

        '    TOThrs = 0
        '    TOTloe = 0
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        'End If

        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "setTimeout(CalculateATall, 100);", True)

    End Sub


End Class
