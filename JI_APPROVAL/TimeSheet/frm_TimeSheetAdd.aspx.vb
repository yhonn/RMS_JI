Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports ly_SIME
Imports System.Web.Services
Imports ly_RMS
Imports System.Globalization
Imports System.Web.Script.Services

Partial Class frm_TimeSheetAdd
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_TIMESHEET_ADD"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Public strPeriodo As String = ""

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


            If Not IsNothing(Me.Request.QueryString("ID")) Then

                hd_idTimeSheet.Value = Convert.ToInt32(Me.Request.QueryString("ID"))

            Else

                hd_idTimeSheet.Value = 0

            End If

            Me.hd_leave.Value = Convert.ToInt32(Me.Request.QueryString("lv"))

            If Convert.ToInt32(Me.hd_leave.Value) = 1 Then

                Me.lblt_subtitulo_pantalla_2.Visible = True
                Me.lblt_subtitulo_pantalla.Visible = False

            Else


                Me.lblt_subtitulo_pantalla_2.Visible = False
                Me.lblt_subtitulo_pantalla.Visible = True

            End If


            LoadData(hd_idTimeSheet.Value)



            'Me.grd_cate.DataSource = cls_TimeSheet.get_Table(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)
            'Me.grd_cate.DataBind()

            'Dim DaysInMonth As Integer = Date.DaysInMonth(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)

            'Dim nameCol As String
            'For i = DaysInMonth + 1 To 31
            '    nameCol = String.Format("colm_d{0}", i)
            '    Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
            'Next

            Me.lblJobTittle.Text = ""
            Me.lbl_IdEmpleado.Text = 0


            'Try
            '    Dim rnd As New Random()
            '    Dim fecha As DateTime = Date.Now
            '    Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
            '    Me.lbl_idTemp.Text = rnd.Next(1, 999).ToString & textfecha.ToString
            'Catch ex As Exception
            '    Me.lbl_idTemp.Text = "-1"
            'End Try



        Else

            'If Me.grd_cate.Items.Count() > 0 Then
            '    Me.cmb_rol.Enabled = False
            'Else
            '    Me.cmb_rol.Enabled = True
            'End If

        End If


    End Sub


    Public Sub LoadData(ByVal idTimeSheet As Integer)


        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

        Dim tbl_user_role As New DataTable
        Dim strRoles As String = ""
        '***********************Group Roles***********************************
        'get_UserRolesALL
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

        Dim strUsers As String = lbl_GroupRolID.Text.Trim
        Dim ArrayUsers As String() = strUsers.Split(New Char() {","c})
        Dim ArrayINT_Users As Integer() = Array.ConvertAll(ArrayUsers, Function(str) Int32.Parse(str))


        Me.cmb_usuario.DataSourceID = ""
        Me.cmb_usuario.DataSource = cls_TimeSheet.get_t_usuarios(Me.Session("E_IdUser"), ArrayINT_Users)
        Me.cmb_usuario.DataTextField = "nombre_usuario"
        Me.cmb_usuario.DataValueField = "id_usuario"
        Me.cmb_usuario.DataBind()


        Me.cmb_EmployeeType.DataSourceID = ""
        Me.cmb_EmployeeType.DataSource = cls_TimeSheet.get_type_employee()
        Me.cmb_EmployeeType.DataTextField = "employee_type"
        Me.cmb_EmployeeType.DataValueField = "id_employee_type"
        Me.cmb_EmployeeType.DataBind()

        Me.cmb_year.DataSource = cls_TimeSheet.get_years()
        Me.cmb_year.DataBind()

        Me.cmb_Month.DataSource = cls_TimeSheet.get_months()
        Me.cmb_Month.DataBind()

        Me.cmb_year.SelectedValue = DatePart(DateInterval.Year, Date.UtcNow)
        Me.cmb_Month.SelectedValue = DatePart(DateInterval.Month, Date.UtcNow)

        If idTimeSheet > 0 Then

            SetTimeSheet(idTimeSheet)

        Else

            Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")
            Me.anchorBillable.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorSupportDocs.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        End If



    End Sub




    Public Sub SetTimeSheet(ByVal idTimeSheet As Integer)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim TimeSheet As ta_timesheet = cls_TimeSheet.get_TimeSheet(idTimeSheet)

        cmb_usuario.SelectedValue = TimeSheet.id_usuario
        cmb_year.SelectedValue = TimeSheet.anio
        cmb_Month.SelectedValue = TimeSheet.mes
        txt_description.Text = TimeSheet.description
        txt_notes.Text = TimeSheet.notes
        'TimeSheet.id_timesheet_estado = 1 
        cmb_EmployeeType.SelectedValue = TimeSheet.id_employee_type

        Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")

        Me.anchorBillable.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorBillable.Attributes.Add("href", "frm_TimeSheetEdit.aspx?ID=" & idTimeSheet.ToString)




        If TimeSheet.id_timesheet_estado = 1 Or TimeSheet.id_timesheet_estado = 5 Then
            Me.btnlk_continue.Enabled = True
        Else
            Me.btnlk_continue.Enabled = False
        End If

        If TimeSheet.id_timesheet_estado > 1 Then

            Me.anchorSupportDocs.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorSupportDocs.Attributes.Add("href", "frm_TimeSheetDocs.aspx?ID=" & idTimeSheet.ToString)

            Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & idTimeSheet.ToString)

        Else
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorSupportDocs.Attributes.Add("class", "btn btn-default btn-circle disabled")
        End If



    End Sub


    Public Function EXEC_Action() As Boolean

        Dim err As Boolean = False
        Dim ext As String = ""
        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim TimeSheet As ta_timesheet = New ta_timesheet

        If Me.hd_idTimeSheet.Value > 0 Then
            TimeSheet = cls_TimeSheet.get_TimeSheet(Me.hd_idTimeSheet.Value)
        End If

        TimeSheet.id_usuario = cmb_usuario.SelectedValue
        TimeSheet.anio = cmb_year.SelectedValue
        TimeSheet.mes = cmb_Month.SelectedValue
        TimeSheet.description = txt_description.Text.Trim
        TimeSheet.notes = txt_notes.Text.Trim
        TimeSheet.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        TimeSheet.fecha_creo = Date.UtcNow
        TimeSheet.usuario_creo = Convert.ToInt32(Me.Session("E_IdUser"))

        If Me.hd_idTimeSheet.Value = 0 Then
            TimeSheet.id_timesheet_estado = 1
        End If

        TimeSheet.id_employee_type = Convert.ToInt32(cmb_EmployeeType.SelectedValue)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))


        Try

            Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Me.hd_idTimeSheet.Value)

            If result <> -1 Then

                Me.Response.Redirect("~/TimeSheet/frm_TimeSheetEdit.aspx?ID=" & result.Trim)
                Return True

            Else 'Error

                Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", result)
                Me.lblError.Visible = True
                Return False

            End If

        Catch ex As Exception
            Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
            Me.lblError.Visible = True
            Return False
        End Try



    End Function


    Private Sub btnlk_continue_Click(sender As Object, e As EventArgs) Handles btnlk_continue.Click

        Dim err As Boolean = False
        Dim ext As String = ""
        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim TimeSheet As ta_timesheet = New ta_timesheet

        If Me.hd_idTimeSheet.Value > 0 Then
            TimeSheet = cls_TimeSheet.get_TimeSheet(Me.hd_idTimeSheet.Value)
        End If

        TimeSheet.id_usuario = cmb_usuario.SelectedValue
        TimeSheet.anio = cmb_year.SelectedValue
        TimeSheet.mes = cmb_Month.SelectedValue
        TimeSheet.description = txt_description.Text.Trim
        TimeSheet.notes = txt_notes.Text.Trim
        TimeSheet.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        TimeSheet.fecha_creo = Date.UtcNow
        TimeSheet.usuario_creo = Convert.ToInt32(Me.Session("E_IdUser"))
        TimeSheet.ts_leave_update = Convert.ToInt32(Me.hd_leave.Value)

        If Me.hd_idTimeSheet.Value = 0 Then
            TimeSheet.id_timesheet_estado = 1
        End If

        TimeSheet.id_employee_type = Convert.ToInt32(cmb_EmployeeType.SelectedValue)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))


        Try

            Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Me.hd_idTimeSheet.Value)

            If result <> -1 Then

                Me.Response.Redirect("~/TimeSheet/frm_TimeSheetEdit.aspx?ID=" & result.Trim)

            Else 'Error

                Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", result)
                Me.lblError.Visible = True

            End If

        Catch ex As Exception
            Me.lblError.Text = String.Format("Error saving timeSheet, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
            Me.lblError.Visible = True
        End Try


    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/TimeSheet/frm_TimeSheet.aspx")
    End Sub




    'Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim itemD As GridDataItem = CType(e.Item, GridDataItem)

    '        Dim txtRadNumeric1 As RadNumericTextBox = CType(itemD("colm_d1").FindControl("txt_d1"), RadNumericTextBox)
    '        Dim txtRadNumeric2 As RadNumericTextBox = CType(itemD("colm_d2").FindControl("txt_d2"), RadNumericTextBox)
    '        Dim txtRadNumeric3 As RadNumericTextBox = CType(itemD("colm_d3").FindControl("txt_d3"), RadNumericTextBox)
    '        Dim txtRadNumeric4 As RadNumericTextBox = CType(itemD("colm_d4").FindControl("txt_d4"), RadNumericTextBox)
    '        Dim txtRadNumeric5 As RadNumericTextBox = CType(itemD("colm_d5").FindControl("txt_d5"), RadNumericTextBox)
    '        Dim txtRadNumeric6 As RadNumericTextBox = CType(itemD("colm_d6").FindControl("txt_d6"), RadNumericTextBox)
    '        Dim txtRadNumeric7 As RadNumericTextBox = CType(itemD("colm_d7").FindControl("txt_d7"), RadNumericTextBox)
    '        Dim txtRadNumeric8 As RadNumericTextBox = CType(itemD("colm_d8").FindControl("txt_d8"), RadNumericTextBox)
    '        Dim txtRadNumeric9 As RadNumericTextBox = CType(itemD("colm_d9").FindControl("txt_d9"), RadNumericTextBox)
    '        Dim txtRadNumeric10 As RadNumericTextBox = CType(itemD("colm_d10").FindControl("txt_d10"), RadNumericTextBox)
    '        Dim txtRadNumeric11 As RadNumericTextBox = CType(itemD("colm_d11").FindControl("txt_d11"), RadNumericTextBox)
    '        Dim txtRadNumeric12 As RadNumericTextBox = CType(itemD("colm_d12").FindControl("txt_d12"), RadNumericTextBox)
    '        Dim txtRadNumeric13 As RadNumericTextBox = CType(itemD("colm_d13").FindControl("txt_d13"), RadNumericTextBox)
    '        Dim txtRadNumeric14 As RadNumericTextBox = CType(itemD("colm_d14").FindControl("txt_d14"), RadNumericTextBox)
    '        Dim txtRadNumeric15 As RadNumericTextBox = CType(itemD("colm_d15").FindControl("txt_d15"), RadNumericTextBox)
    '        Dim txtRadNumeric16 As RadNumericTextBox = CType(itemD("colm_d16").FindControl("txt_d16"), RadNumericTextBox)
    '        Dim txtRadNumeric17 As RadNumericTextBox = CType(itemD("colm_d17").FindControl("txt_d17"), RadNumericTextBox)
    '        Dim txtRadNumeric18 As RadNumericTextBox = CType(itemD("colm_d18").FindControl("txt_d18"), RadNumericTextBox)
    '        Dim txtRadNumeric19 As RadNumericTextBox = CType(itemD("colm_d19").FindControl("txt_d19"), RadNumericTextBox)
    '        Dim txtRadNumeric20 As RadNumericTextBox = CType(itemD("colm_d20").FindControl("txt_d20"), RadNumericTextBox)
    '        Dim txtRadNumeric21 As RadNumericTextBox = CType(itemD("colm_d21").FindControl("txt_d21"), RadNumericTextBox)
    '        Dim txtRadNumeric22 As RadNumericTextBox = CType(itemD("colm_d22").FindControl("txt_d22"), RadNumericTextBox)
    '        Dim txtRadNumeric23 As RadNumericTextBox = CType(itemD("colm_d23").FindControl("txt_d23"), RadNumericTextBox)
    '        Dim txtRadNumeric24 As RadNumericTextBox = CType(itemD("colm_d24").FindControl("txt_d24"), RadNumericTextBox)
    '        Dim txtRadNumeric25 As RadNumericTextBox = CType(itemD("colm_d25").FindControl("txt_d25"), RadNumericTextBox)
    '        Dim txtRadNumeric26 As RadNumericTextBox = CType(itemD("colm_d26").FindControl("txt_d26"), RadNumericTextBox)
    '        Dim txtRadNumeric27 As RadNumericTextBox = CType(itemD("colm_d27").FindControl("txt_d27"), RadNumericTextBox)
    '        Dim txtRadNumeric28 As RadNumericTextBox = CType(itemD("colm_d28").FindControl("txt_d28"), RadNumericTextBox)
    '        Dim txtRadNumeric29 As RadNumericTextBox = CType(itemD("colm_d29").FindControl("txt_d29"), RadNumericTextBox)
    '        Dim txtRadNumeric30 As RadNumericTextBox = CType(itemD("colm_d30").FindControl("txt_d30"), RadNumericTextBox)
    '        Dim txtRadNumeric31 As RadNumericTextBox = CType(itemD("colm_d31").FindControl("txt_d31"), RadNumericTextBox)

    '        Dim txtRadNumericTotal As RadNumericTextBox = CType(itemD("Total").FindControl("txt_all"), RadNumericTextBox)


    '        If Not CType(itemD("visible").Text, Boolean) = True Then


    '            itemD("colm_d1").Text = itemD("1").Text
    '            itemD("colm_d2").Text = itemD("2").Text
    '            itemD("colm_d3").Text = itemD("3").Text
    '            itemD("colm_d4").Text = itemD("4").Text
    '            itemD("colm_d5").Text = itemD("5").Text
    '            itemD("colm_d6").Text = itemD("6").Text
    '            itemD("colm_d7").Text = itemD("7").Text
    '            itemD("colm_d8").Text = itemD("8").Text
    '            itemD("colm_d9").Text = itemD("9").Text
    '            itemD("colm_d10").Text = itemD("10").Text
    '            itemD("colm_d11").Text = itemD("11").Text
    '            itemD("colm_d12").Text = itemD("12").Text
    '            itemD("colm_d13").Text = itemD("13").Text
    '            itemD("colm_d14").Text = itemD("14").Text
    '            itemD("colm_d15").Text = itemD("15").Text
    '            itemD("colm_d16").Text = itemD("16").Text
    '            itemD("colm_d17").Text = itemD("17").Text
    '            itemD("colm_d18").Text = itemD("18").Text
    '            itemD("colm_d19").Text = itemD("19").Text
    '            itemD("colm_d20").Text = itemD("20").Text
    '            itemD("colm_d21").Text = itemD("21").Text
    '            itemD("colm_d22").Text = itemD("22").Text
    '            itemD("colm_d23").Text = itemD("23").Text
    '            itemD("colm_d24").Text = itemD("24").Text
    '            itemD("colm_d25").Text = itemD("25").Text
    '            itemD("colm_d26").Text = itemD("26").Text
    '            itemD("colm_d27").Text = itemD("27").Text
    '            itemD("colm_d28").Text = itemD("28").Text
    '            itemD("colm_d29").Text = itemD("29").Text
    '            itemD("colm_d30").Text = itemD("30").Text
    '            itemD("colm_d31").Text = itemD("31").Text
    '            itemD("Total").Text = ""

    '            txtRadNumeric1.Visible = False
    '            txtRadNumeric2.Visible = False
    '            txtRadNumeric3.Visible = False
    '            txtRadNumeric4.Visible = False
    '            txtRadNumeric5.Visible = False
    '            txtRadNumeric6.Visible = False
    '            txtRadNumeric7.Visible = False
    '            txtRadNumeric8.Visible = False
    '            txtRadNumeric9.Visible = False
    '            txtRadNumeric10.Visible = False
    '            txtRadNumeric11.Visible = False
    '            txtRadNumeric12.Visible = False
    '            txtRadNumeric13.Visible = False
    '            txtRadNumeric14.Visible = False
    '            txtRadNumeric15.Visible = False
    '            txtRadNumeric16.Visible = False
    '            txtRadNumeric17.Visible = False
    '            txtRadNumeric18.Visible = False
    '            txtRadNumeric19.Visible = False
    '            txtRadNumeric20.Visible = False
    '            txtRadNumeric21.Visible = False
    '            txtRadNumeric22.Visible = False
    '            txtRadNumeric23.Visible = False
    '            txtRadNumeric24.Visible = False
    '            txtRadNumeric25.Visible = False
    '            txtRadNumeric26.Visible = False
    '            txtRadNumeric27.Visible = False
    '            txtRadNumeric28.Visible = False
    '            txtRadNumeric29.Visible = False
    '            txtRadNumeric30.Visible = False
    '            txtRadNumericTotal.Visible = False

    '            'Else

    '            '    txtRadNumeric1.Value = Val(itemD("1").Text)
    '            '    txtRadNumeric2.Value = Val(itemD("2").Text)
    '            '    txtRadNumeric3.Value = Val(itemD("3").Text)
    '            '    txtRadNumeric4.Value = Val(itemD("4").Text)
    '            '    txtRadNumeric5.Value = Val(itemD("5").Text)
    '            '    txtRadNumeric6.Value = Val(itemD("6").Text)
    '            '    txtRadNumeric7.Value = Val(itemD("7").Text)
    '            '    txtRadNumeric8.Value = Val(itemD("8").Text)
    '            '    txtRadNumeric9.Value = Val(itemD("9").Text)
    '            '    txtRadNumeric10.Value = Val(itemD("10").Text)
    '            '    txtRadNumeric11.Value = Val(itemD("11").Text)
    '            '    txtRadNumeric12.Value = Val(itemD("12").Text)
    '            '    txtRadNumeric13.Value = Val(itemD("13").Text)
    '            '    txtRadNumeric14.Value = Val(itemD("14").Text)
    '            '    txtRadNumeric15.Value = Val(itemD("15").Text)
    '            '    txtRadNumeric16.Value = Val(itemD("16").Text)
    '            '    txtRadNumeric17.Value = Val(itemD("17").Text)
    '            '    txtRadNumeric18.Value = Val(itemD("18").Text)
    '            '    txtRadNumeric19.Value = Val(itemD("19").Text)
    '            '    txtRadNumeric20.Value = Val(itemD("20").Text)
    '            '    txtRadNumeric21.Value = Val(itemD("21").Text)
    '            '    txtRadNumeric22.Value = Val(itemD("22").Text)
    '            '    txtRadNumeric23.Value = Val(itemD("23").Text)
    '            '    txtRadNumeric24.Value = Val(itemD("24").Text)
    '            '    txtRadNumeric25.Value = Val(itemD("25").Text)
    '            '    txtRadNumeric26.Value = Val(itemD("26").Text)
    '            '    txtRadNumeric27.Value = Val(itemD("27").Text)
    '            '    txtRadNumeric28.Value = Val(itemD("28").Text)
    '            '    txtRadNumeric29.Value = Val(itemD("29").Text)
    '            '    txtRadNumeric30.Value = Val(itemD("30").Text)
    '            '    txtRadNumeric31.Value = Val(itemD("31").Text)



    '        End If



    '    End If



    'End Sub


    Protected Sub cmb_usuario_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("nombre_usuario").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_usuario").ToString()

    End Sub


    Protected Sub cmb_usuario_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(cmb_usuario.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(cmb_usuario.Items.Count)
    End Sub


    'Protected Sub cmb_usuario_IndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)

    '    cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

    '    Dim tblUSr As DataTable = cls_TimeSheet.get_t_usuarios(Me.cmb_usuario.SelectedValue)

    '    lblJobTittle.Text = tblUSr.Rows(0).Item("job")


    'End Sub

    'Protected Sub cmb_Month_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)

    '    cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

    '    Me.grd_cate.DataSource = cls_TimeSheet.get_Table(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)
    '    Me.grd_cate.DataBind()

    '    Dim DaysInMonth As Integer = Date.DaysInMonth(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)

    '    Dim nameCol As String
    '    For i = 29 To 31
    '        nameCol = String.Format("colm_d{0}", i)
    '        Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = True
    '    Next


    '    For i = DaysInMonth + 1 To 31
    '        nameCol = String.Format("colm_d{0}", i)
    '        Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
    '    Next

    'End Sub

    'Protected Sub cmb_Year_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)

    '    cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

    '    Me.grd_cate.DataSource = cls_TimeSheet.get_Table(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)
    '    Me.grd_cate.DataBind()

    '    Dim DaysInMonth As Integer = Date.DaysInMonth(Me.cmb_year.SelectedValue, Me.cmb_Month.SelectedValue)

    '    Dim nameCol As String
    '    For i = 29 To 31
    '        nameCol = String.Format("colm_d{0}", i)
    '        Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = True
    '    Next

    '    For i = DaysInMonth + 1 To 31
    '        nameCol = String.Format("colm_d{0}", i)
    '        Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
    '    Next

    'End Sub

    <Web.Services.WebMethod()>
    Public Shared Function GetUSerJobTittle(ByVal idUsuario As Integer, ByVal idPrograma As Integer) As String

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(idPrograma)

        Dim tblUSr As DataTable = cls_TimeSheet.get_t_usuarios(idUsuario)

        'lblJobTittle.Text = tblUSr.Rows(0).Item("job")
        Return tblUSr.Rows(0).Item("job")

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

    Private Sub cmb_Month_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_Month.SelectedIndexChanged

        If Val(Me.cmb_Month.SelectedValue) > 0 And Val(Me.cmb_year.SelectedValue) > 0 Then

            Dim DateMes As DateTime = New Date(Val(Me.cmb_year.SelectedValue), Val(Me.cmb_Month.SelectedValue), 1)
            strPeriodo = String.Format(" {0} / {1} ", DateMes.ToString("MMMM"), Val(Me.cmb_year.SelectedValue))

            cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
            Dim TimeSheets As DataTable = cls_TimeSheet.getTimeSheetPeriod(Convert.ToInt32(Me.Session("E_IdUser")), Val(Me.cmb_Month.SelectedValue), Val(Me.cmb_year.SelectedValue))

            Dim strMess As String = ""
            Dim strMess1 As String = ""

            If TimeSheets.Rows.Count > 0 Then

                strMess = String.Format(Me.h_texto_mensaje.Value, TimeSheets.Rows.Count, If(TimeSheets.Rows.Count > 1, "s", ""), strPeriodo)
                Me.texto_mensaje.InnerText = strMess

                strMess1 = String.Format(Me.h_H1.Value, strPeriodo)
                Me.H1.InnerText = strMess1

                Me.rept_timesheet.DataSource = TimeSheets
                Me.rept_timesheet.DataBind()

                Dim funcion = "FuncModal_Mensaje()"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)

            End If

        End If

    End Sub

    Private Sub btn_ok_Click(sender As Object, e As EventArgs) Handles btn_ok.Click

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim TimeSheets As DataTable = cls_TimeSheet.getTimeSheetPeriod(Convert.ToInt32(Me.Session("E_IdUser")), Val(Me.cmb_Month.SelectedValue), Val(Me.cmb_year.SelectedValue))

        Dim idTS As Integer = 0
        If TimeSheets.Rows.Count > 0 Then
            idTS = TimeSheets.Rows.Item(0).Item("id_timesheet")
        End If

        Me.Response.Redirect("~/TimeSheet/frm_TimeSheetAdd.aspx?ID=" & idTS.ToString)

    End Sub

End Class
