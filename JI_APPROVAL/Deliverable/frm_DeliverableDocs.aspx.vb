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
Imports System.Web.Script.Serialization


Partial Class frm_DeliverableDocs
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIVERABLE_DOCS"

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval

    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim cls_Deliverable As APPROVAL.clss_Deliverable
    Dim clDate As APPROVAL.cls_dUtil

    Public userName As String = ""
    Public userImplementer As String = ""

    Public Status_TS As String = ""
    Public DateStatus_TS As String = ""
    Public HourStatus_TS As String = ""
    Public cultureUSer As CultureInfo

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

        cultureUSer = cl_user.regionalizacionCulture

        If Not IsPostBack Then

            If Not IsNothing(Me.Request.QueryString("ID")) Then

                hd_id_deliverable.Value = Convert.ToInt32(Me.Request.QueryString("ID"))

            Else

                hd_id_deliverable.Value = 0

            End If

            LoadData(hd_id_deliverable.Value)

        Else

            ' ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)

        End If


    End Sub


    Public Sub LoadData(ByVal idDeliverable As Integer)

        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

        Dim tbl_user_role As New DataTable
        Dim strRoles As String = ""

        '***********************Group Roles***********************************
        ''get_UserRolesALL
        'clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
        'tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 2)
        'strRoles = ""
        'For Each dtRow In tbl_user_role.Rows
        '    strRoles &= dtRow("id_rol").ToString & ","
        'Next
        'If strRoles.Length > 0 Then
        '    strRoles = strRoles.Substring(0, strRoles.Length - 1)
        'End If
        'lbl_GroupRolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
        '*********************** Roles Roles***********************************

        'Dim strUsers As String = lbl_GroupRolID.Text.Trim
        'Dim ArrayUsers As String() = strUsers.Split(New Char() {","c})
        'Dim ArrayINT_Users As Integer() = Array.ConvertAll(ArrayUsers, Function(str) Int32.Parse(str))

        'Me.cmb_usuario.DataSourceID = ""
        'Me.cmb_usuario.DataSource = cls_TimeSheet.get_t_usuarios(Me.Session("E_IdUser"), ArrayINT_Users)
        'Me.cmb_usuario.DataTextField = "nombre_usuario"
        'Me.cmb_usuario.DataValueField = "id_usuario"
        'Me.cmb_usuario.DataBind()

        Dim tbl_Deliverable As DataTable = cls_Deliverable.get_Deliverables(hd_id_deliverable.Value)

        Dim tbl_user As DataTable = cls_Deliverable.get_t_usuarios_Implementer(Me.Session("E_IdUser"))

        If tbl_user.Rows.Count > 0 Then

            userName = tbl_user.Rows.Item(0).Item("nombre_usuario")
            userImplementer = tbl_user.Rows.Item(0).Item("nombre_ejecutor")

        Else

            userName = "----"
            userImplementer = "----"

        End If

        Dim tbl_ActivitiesImplementer As DataTable = cls_Deliverable.get_Activities_Implementer(tbl_user.Rows.Item(0).Item("id_ejecutor"))
        'Me.cmb_activity.DataSourceID = ""
        'Me.cmb_activity.DataSource = tbl_ActivitiesImplementer
        'Me.cmb_activity.DataBind()

        If tbl_ActivitiesImplementer.Rows.Count > 0 Then

            Me.lbl_activity_Code.Text = tbl_ActivitiesImplementer.Rows.Item(0).Item("codigo_SAPME")
            Me.lbl_activity_name.Text = tbl_ActivitiesImplementer.Rows.Item(0).Item("nombre_proyecto")

        Else

            Me.lbl_activity_Code.Text = "--"
            Me.lbl_activity_name.Text = "--"

        End If



        Dim fechaHOY As DateTime = Date.UtcNow
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

        Status_TS = "CREATING"
        DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(fechaHOY, "m", timezoneUTC, False))
        HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(fechaHOY, timezoneUTC, True))

        If tbl_Deliverable.Rows.Item(0).Item("id_deliverable_estado") = 5 Or tbl_Deliverable.Rows.Item(0).Item("id_deliverable_estado") = 1 Then 'Created and Observation Pending
            Me.btnlk_continue.Enabled = True
        Else
            Me.btnlk_continue.Enabled = False
        End If

        'Me.cmb_EmployeeType.DataSourceID = ""
        'Me.cmb_EmployeeType.DataSource = cls_TimeSheet.get_type_employee()
        'Me.cmb_EmployeeType.DataTextField = "employee_type"
        'Me.cmb_EmployeeType.DataValueField = "id_employee_type"
        'Me.cmb_EmployeeType.DataBind()

        'Me.cmb_year.DataSource = cls_TimeSheet.get_years()
        'Me.cmb_year.DataBind()

        'Me.cmb_Month.DataSource = cls_TimeSheet.get_months()
        'Me.cmb_Month.DataBind()

        'Me.cmb_year.SelectedValue = DatePart(DateInterval.Year, Date.UtcNow)
        'Me.cmb_Month.SelectedValue = DatePart(DateInterval.Month, Date.UtcNow)


        If idDeliverable > 0 Then

            SetDeliverable(idDeliverable)

        Else

            Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")
            Me.anchorResults.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        End If



    End Sub




    Public Sub SetDeliverable(ByVal idDeliverable As Integer)

        'cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        'Dim TimeSheet As ta_timesheet = cls_TimeSheet.get_TimeSheet(idTimeSheet)

        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

        'cmb_usuario.SelectedValue = TimeSheet.id_usuario
        'cmb_year.SelectedValue = TimeSheet.anio
        'cmb_Month.SelectedValue = TimeSheet.mes
        'txt_description.Text = TimeSheet.description
        'txt_notes.Text = TimeSheet.notes
        ''TimeSheet.id_timesheet_estado = 1 
        'cmb_EmployeeType.SelectedValue = TimeSheet.id_employee_type

        Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)
        Me.hd_tasa_cambio.Value = cls_Deliverable.get_ExchangeRate()

        userName = Tbl_deliverable.Rows.Item(0).Item("createdBy")
        userImplementer = Tbl_deliverable.Rows.Item(0).Item("Implementer")

        Me.lbl_activity_Code.Text = Tbl_deliverable.Rows.Item(0).Item("codigo_SAPME")
        Me.lbl_activity_name.Text = Tbl_deliverable.Rows.Item(0).Item("nombre_proyecto")

        Me.dv_description.InnerHtml = Tbl_deliverable.Rows.Item(0).Item("description")
        'Me.txt_description.Text = Tbl_deliverable.Rows.Item(0).Item("description")
        Me.dv_notes.InnerHtml = Tbl_deliverable.Rows.Item(0).Item("notes")
        'Me.txt_notes.Text = Tbl_deliverable.Rows.Item(0).Item("notes")

        Dim fechaHOY As DateTime = Date.UtcNow
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

        Status_TS = Tbl_deliverable.Rows.Item(0).Item("deliverable_estado")

        If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") <= 1 Then
            DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), "m", timezoneUTC, False))
            HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), timezoneUTC, True))
        Else
            DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), "m", timezoneUTC, False))
            HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), timezoneUTC, True))
        End If

        Me.anchorInformation.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorInformation.Attributes.Add("href", "frm_DeliverableAdd.aspx?ID=" & idDeliverable)

        Me.anchorResults.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorResults.Attributes.Add("href", "frm_DeliverableEdit.aspx?ID=" & idDeliverable)

        Me.anchorDocuments.Attributes.Add("class", "btn btn-primary btn-circle")

        'Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")
        'Me.anchorResults.Attributes.Add("class", "btn btn-default btn-circle disabled")
        'Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
        Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Docs(idDeliverable)

        If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") > 1 Then

            Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorFollowUp.Attributes.Add("href", "frm_DeliverableFollowing.aspx?ID=" & idDeliverable)

        Else

            If tbl_Suppor_docs.Rows.Count > 0 Then
                Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorFollowUp.Attributes.Add("href", "frm_DeliverableFollowing.aspx?ID=" & idDeliverable)
            Else
                Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
            End If

        End If

        Dim strArray As String() = GetDeliverable_(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable).Split("||")

        'ltr_rows_Deliverables.InnerHtml = strArray(0)
        lbl_totalACT_usd.Text = strArray(2)
        lbl_totalACT.Text = strArray(4)
        'lbl_totalPend_usd.Text = strArray(6)
        'lbl_totalPend.Text = strArray(8)
        lbl_totalPerf_usd.Text = strArray(10)
        lbl_totalPerf.Text = strArray(12)

        Me.hd_performed.Value = strArray(14)

        lbl_period.Text = String.Format(" {0:d} to {1:d} ", Tbl_deliverable.Rows.Item(0).Item("fecha_inicio_proyecto"), Tbl_deliverable.Rows.Item(0).Item("fecha_fin_proyecto"))

        ' Dim Tbl_DelivINFO As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)
        'dvNEXT_delieverable.InnerHtml = strArray(16)
        Me.rep_DelivINFO.DataSource = Tbl_deliverable  'Tbl_DelivINFO 'Deliverable INFO
        Me.rep_DelivINFO.DataBind()

        hd_id_ficha_entregable.Value = Tbl_deliverable.Rows.Item(0).Item("id_ficha_entregable")

        'Me.reptTable.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), True)
        Me.reptTable.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable)

        Me.reptTable.DataBind()

        Dim id_user As Integer = Convert.ToInt32(Me.Session("E_IdUser"))

        Dim tbl_ApprovalROUTES As DataTable = cls_Deliverable.get_Deliverable_ApprovalUser(id_user, Tbl_deliverable.Rows.Item(0).Item("id_TipoDocumento"))

        Me.cmb_approvals.DataSource = tbl_ApprovalROUTES
        Me.cmb_approvals.DataTextField = "descripcion_aprobacion"
        Me.cmb_approvals.DataValueField = "id_tipoDocumento"
        Me.cmb_approvals.DataBind()

        If tbl_ApprovalROUTES.Rows.Count > 0 Then
            Me.cmb_approvals.SelectedValue = Tbl_deliverable.Rows.Item(0).Item("id_TipoDocumento")
            Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(Tbl_deliverable.Rows.Item(0).Item("id_TipoDocumento"), If(Me.hd_files_selected.Value = "0", "", Me.hd_files_selected.Value))
            Me.grd_documentos.DataBind()
        End If

        Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)

        If tbl_result.Rows.Count > 0 Then

            Me.cmb_approvals.SelectedValue = tbl_result.Rows.Item(0).Item("id_tipoDocumento")
            Me.hd_id_documento_deliverable.Value = tbl_result.Rows.Item(0).Item("id_documento_deliverable")
            Me.hd_id_documento.Value = If(IsDBNull(tbl_result.Rows.Item(0).Item("id_documento")), 0, tbl_result.Rows.Item(0).Item("id_documento"))
            'tbl_result.Rows.Item(0).Item("id_documento")

            If (Convert.ToInt32(Me.hd_id_documento.Value) > 0) Then
                'If (Val(Me.hd_id_documento.Value) > 0) Then
                Me.cmb_approvals.Enabled = False
            End If

        Else

            Me.cmb_approvals.Enabled = True
            Me.hd_id_documento_deliverable.Value = 0
            Me.hd_id_documento.Value = 0

        End If

        'Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Docs(idDeliverable)

        Dim tot_files As Integer = If(tbl_Suppor_docs.Rows.Count > 0, tbl_Suppor_docs.Rows.Count, 0)

        If tot_files > 0 Then

            Me.hd_has_files.Value = tot_files ' this is for verify when we save it

            For Each dtRow As DataRow In tbl_Suppor_docs.Rows

                Dim lst_item As RadListBoxItem = New RadListBoxItem(dtRow("archivo"), dtRow("id_doc_soporte"))
                rdListBox_files.Items.Add(lst_item)
                Me.hd_files_selected.Value &= "," & dtRow("id_doc_soporte")

            Next

            Me.lbl_hasFiles.Value = True

            Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(tbl_result.Rows.Item(0).Item("id_tipoDocumento"), If(Me.hd_files_selected.Value = "0", "", Me.hd_files_selected.Value))
            Me.grd_documentos.DataBind()

            'Else

            '    Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(0, "")
            '    Me.grd_documentos.DataBind()

        End If

        'Me.reptTable_2.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), False)
        'Me.reptTable_2.DataBind()


        Me.hd_percent.Value = strArray(14)

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)


    End Sub






    Private Sub btnlk_continue_Click(sender As Object, e As EventArgs) Handles btnlk_continue.Click

        Dim err As Boolean = False
        Dim ext As String = ""

        Dim flg_Err As Boolean = False

        Try

            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

            Dim tbl_ta_documento_deliverable As New ta_documento_deliverable

            tbl_ta_documento_deliverable.id_deliverable = Me.hd_id_deliverable.Value
            tbl_ta_documento_deliverable.id_tipoDocumento = Me.cmb_approvals.SelectedValue

            If Val(Me.hd_id_documento.Value) > 0 Then
                tbl_ta_documento_deliverable.id_documento = Me.hd_id_documento.Value
            End If

            If cls_Deliverable.Save_documento_deliverable(tbl_ta_documento_deliverable, Convert.ToInt32(Me.hd_id_documento_deliverable.Value)) <> -1 Then

                Check_del_Ones() 'For Checkig the deleted ones
                Check_NewFile_FileUploaded() 'Uploading and storing the New Files here
                flg_Err = True

            End If


            If flg_Err Then

                Me.Response.Redirect("~/Deliverable/frm_DeliverableFollowing.aspx?ID=" & hd_id_deliverable.Value)

            Else 'Error

                Me.lblError.Text = String.Format("Error saving results, please contact to the system administrator <br /><br />")
                Me.lblError.Visible = True

            End If

        Catch ex As Exception
            Me.lblError.Text = String.Format("Error saving deliverable results, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
            Me.lblError.Visible = True
        End Try


    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Deliverable/frm_Deliverable.aspx")
    End Sub


    Protected Sub cmb_activity_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.

        Dim DateINI As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_inicio_proyecto").ToString(), Date)
        Dim DateFIN As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_fin_proyecto").ToString(), Date)


        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", (DirectCast(e.Item.DataItem, DataRowView))("codigo_SAPME").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("nombre_proyecto").ToString(), String.Format("{0:d}", DateINI), String.Format("{0:d}", DateFIN))
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_ficha_proyecto").ToString()

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


    Private Function GetDeliverable_(ByVal idFichaProyecto As Integer, Optional id_deliverable As Integer = 0) As String

        Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDPrograma")))

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim ExchangeRate As Double = cls_Deliverable.get_ExchangeRate()

        Dim tbl_Deliverables As DataTable = cls_Deliverable.get_Deliverable_Activity(idFichaProyecto, 0)

        Dim strRowsTOT As String = ""
        Dim strRows As String = "<tr>
                                   <td  rowspan='4'><div class='tools'><a href='/RMS_SIME/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
                                   <td rowspan='4'>{0}</td>
                                   <td rowspan='4'>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                          {1}
                                      </div>
                                   </td>
                                   <td rowspan='4'>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                         {2}
                                      </div>
                                   </td>
                                   <td>Due Date</td>
                                   <td>{3:d}</td>
                                   <td rowspan='4'>{5:P2}</td>
                                   <td rowspan='4'>{6}</td>
                                   <td rowspan='4'>                                                                       
                                     <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
                                   </td>
                                </tr>"

        Dim strRowsFourth As String = "<tr>
                                       <td>Delivered Date</td>
                                       <td>{0:d}</td>
                                      </tr><tr>
                                       <td>Approved Date</td>
                                       <td>{1:d}</td>
                                      </tr><tr>
                                       <td>Disbursed Date</td>
                                       <td>{2:d}</td>
                                      </tr>"


        Dim strRows2 As String = " <tr>
                                       <td colspan='9'>                                                                     
                                          <div class='progress'>
                                              <div class='progress-bar {0} progress-bar-striped' role='progressbar' aria-valuenow='{1}' aria-valuemin='0' aria-valuemax='100' style='width: {1}%'>
                                                   <span >{1}% </span>
                                              </div> 
                                              <div class='progress-bar {2} progress-bar-striped' role='progressbar' aria-valuenow='{3}' aria-valuemin='0' aria-valuemax='100' style='width: {3}%'>
                                                    <span>{4}% </span>
                                              </div>                                                                            
                                          </div>
                                        </td>  
                                    </tr> "



        Dim strTableDEL As String = "<table class='table table-hover'>
                                                                <tr>
                                                                  <th>Deliverable #</th>
                                                                  <td><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                </tr>
                                                                <tr>                                                                  
                                                                  <td colspan='2'>
                                                                     <div style='text-align:left; max-width:100%;'>
                                                                         {1}
                                                                     </div>
                                                                   </td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                  <th>Due Date</th>
                                                                  <td>{2:d}</td>   
                                                                </tr>     
                                                                <tr>
                                                                  <th>Status</th>
                                                                  <td><span class='label {5}'>{3}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{4}</span></td>   
                                                                </tr>  
                                                                 <tr>
                                                                  <th>Porcent</th>
                                                                  <td>{6:P2}</td>   
                                                                </tr>   
                                                                <tr>
                                                                  <th>Amount</th>
                                                                  <td> {7} / {8:N2} USD</td>   
                                                                </tr>                                                               
                                                              </table>"

        'Dim strTable_nextDEL As String = ""
        Dim id_ficha_entregable As Integer = 0

        Dim strStatus As String = ""
        Dim strTime As String = ""
        Dim strAlert As String = ""

        Dim strAlert2 As String = ""
        Dim strAlert3 As String = ""
        Dim vDiferences As Double = 0
        Dim vDiferences_Adj As Double = 0
        Dim vDays As Double = 0

        Dim YLAfunding As Double = 0
        Dim YLAfundingUSD As Double = 0
        Dim PerformedFunding As Double = 0
        Dim PerformedFundingUSD As Double = 0
        Dim PendingFunding As Double = 0
        Dim PendingFundingUSD As Double = 0
        Dim PorcenPerformed As Double = 0

        'Dim IdMaxDeliverable As Integer = cls_Deliverable.get_Last_Deliverable(idFichaProyecto, 1).Rows.Item(0).Item("id_deliverable")

        For Each dtRow As DataRow In tbl_Deliverables.Rows

            Dim rDays As Double

            YLAfunding = dtRow("monto_aporte")

            'If (dtRow("id_deliverable_estado") >= 0 And dtRow("id_deliverable_estado") <= 2) Then
            '    PendingFunding += dtRow("valor")
            'ElseIf dtRow("id_deliverable_estado") = 3 Then
            '    PerformedFunding += dtRow("valor")
            'End If

            If dtRow("id_deliverable_estado") = 3 Then
                PerformedFunding += dtRow("valor")
                PerformedFundingUSD += (dtRow("valor") / dtRow("tasa_cambio_final"))
            Else
                PendingFunding += dtRow("valor") 'Take in account when the value allocated it is not the planned
                PendingFundingUSD += (dtRow("valor") / dtRow("tasa_Cambio"))
            End If

            If dtRow("D_Days") <= 0 Then 'its not time 

                rDays = dtRow("D_Days") * -1

                If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                    strTime = Func_Unit(rDays)
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                    strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                Else 'finish processes

                    strStatus = dtRow("deliverable_estado")
                    strTime = Func_Unit(rDays)
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)

                End If


                vDiferences = Math.Round((dtRow("porc_Days") - dtRow("porc_EDays")) * 100, 2, MidpointRounding.AwayFromZero)
                vDays = (dtRow("porc_Days") * 100) - vDiferences
                'vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)
                vDiferences_Adj = vDiferences

            Else 'Delayed time

                If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                    strTime = Func_Unit(dtRow("D_Days"))
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                    strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                Else 'finish processes

                    strStatus = dtRow("deliverable_estado")
                    strTime = Func_Unit(dtRow("D_Days"))
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)

                End If

                vDays = dtRow("porc_Days") * 100
                vDiferences = Math.Round((dtRow("porc_EDays") - dtRow("porc_Days")) * 100, 2, MidpointRounding.AwayFromZero)
                vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)

            End If

            strAlert2 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 3)
            strAlert3 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 4)

            'strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, dtRow("id_deliverable"))
            strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, dtRow("id_deliverable"))
            strRowsTOT &= String.Format(strRowsFourth, If(IsDBNull(dtRow("fecha_entrego")), "--", dtRow("fecha_entrego")), If(IsDBNull(dtRow("fecha_aprobo")), "--", dtRow("fecha_aprobo")), If(IsDBNull(dtRow("delivered_date")), "--", dtRow("delivered_date")))

            strRowsTOT &= String.Format(strRows2, strAlert2, vDays, strAlert3, vDiferences_Adj, vDiferences)

            If dtRow("id_deliverable_estado") = 0 And id_ficha_entregable = 0 Then

                id_ficha_entregable = dtRow("id_ficha_entregable")
                'strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), dtRow("valor"), Math.Round((dtRow("valor") / 3348), 2, MidpointRounding.AwayFromZero))

            ElseIf dtRow("id_deliverable") = id_deliverable Then

                id_ficha_entregable = dtRow("id_ficha_entregable")
                'strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), dtRow("valor"), Math.Round((dtRow("valor") / 3348), 2, MidpointRounding.AwayFromZero))

            End If

        Next

        Dim strTable = "<table class='table no-margin'>
                                  <thead>
                                      <tr>
                                        <th style='width:2%;'></th>                                   
                                        <th style='width:3%;'>#</th>                    
                                        <th style='width:25%;'>Milestone</th>
                                        <th style='width:35%;'>Verification</th>
                                        <th style='width:8%;'>Due Date</th>
                                        <th style='width:8%;'>Delivered Date</th>
                                        <th style='width:3%;'>%</th>
                                        <th style='width:8%;'>Amount</th>
                                        <th style='width:8%;'>Status</th>
                                      </tr>
                                  </thead>
                                  <tbody>   
                                       {0}
                                 </tbody>
                        </table>"


        YLAfundingUSD = Math.Round((YLAfunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)

        'PendingFundingUSD = Math.Round((PendingFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)
        'PerformedFundingUSD = Math.Round((PerformedFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)

        PorcenPerformed = Math.Round(((PerformedFunding / YLAfunding) * 100), 2, MidpointRounding.AwayFromZero)

        GetDeliverable_ = String.Format(strTable, strRowsTOT.Trim) & "||" &
                         String.Format("{0:N2} USD", YLAfundingUSD) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", YLAfunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2} USD", Math.Round(PendingFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PendingFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2} USD", Math.Round(PerformedFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PerformedFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2}", PorcenPerformed) & "||" &
                         "none" & "||" &
                         id_ficha_entregable



    End Function



    Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


        Dim Dif_Porce As Double = porcDays - porcEDays
        Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

        Const c_label_danger As String = "label-danger"
        Const c_label_warning As String = "label-warning"
        Const c_label_primary As String = "label-primary"
        Const c_label_success As String = "label-success"

        Const c_progress_bar_warning = "progress-bar-warning"
        Const c_progress_bar_primary = "progress-bar-primary"
        Const c_progress_bar_danger = "progress-bar-danger"


        Dim strResult As String = ""
        Dim strStatus As String = ""
        Dim strAlertBar1 As String = ""
        Dim strAlertBar2 As String = ""


        If porc_Progress >= 0 Then

            'Inverter number
            If ((1 - porc_Progress) * 100) >= 90 Then
                strResult = c_label_danger
            ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
                strResult = c_label_warning
            ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
                strResult = c_label_primary
            Else
                strResult = c_label_success
            End If

            strStatus = "Pending"
            strAlertBar2 = c_progress_bar_primary

        Else 'Expired
            strResult = c_label_danger
            strStatus = "Expired"
            strAlertBar2 = c_progress_bar_danger
        End If

        strAlertBar1 = c_progress_bar_warning

        If alertType = 1 Then
            Func_Alert = strResult
        ElseIf alertType = 2 Then
            Func_Alert = strStatus
        ElseIf alertType = 3 Then
            Func_Alert = strAlertBar1
        Else
            Func_Alert = strAlertBar2
        End If


    End Function


    Public Shared Function Func_Unit(ByVal Ndays As String) As String


        Dim vDays As Double
        Dim vWeeks As Double
        Dim vMonths As Double
        Dim vYear As Double

        Dim strUnit As String
        Dim vUnit As Double

        vDays = Ndays
        vWeeks = vDays / 7
        vMonths = vDays / 30
        vYear = vDays / 365


        If vWeeks < 1 Then

            vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

            If vDays > 1 Then
                strUnit = " days"
            Else
                strUnit = " day"
            End If

        ElseIf vMonths < 1 Then

            vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

            If vWeeks > 1 Then
                strUnit = " weeks"
            Else
                strUnit = " week"
            End If

        ElseIf vYear < 1 Then

            vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

            If vMonths > 1 Then
                strUnit = " months"
            Else
                strUnit = " month"
            End If

        Else

            vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

            If vYear > 1 Then
                strUnit = " years"
            Else
                strUnit = " year"
            End If

        End If


        Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

    End Function

    Protected Sub reptTable_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) 'Handles reptTable.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)

            Dim txt_val As RadNumericTextBox = ItemD.FindControl("txt_value")
            Dim hd_Val As HiddenField = ItemD.FindControl("hd_value") 'Avance Actual

            Dim hd_id_meta_indicador_ficha As HiddenField = ItemD.FindControl("hd_id_meta_indicador_ficha")
            Dim strControlName As String = String.Format("txt_value_{0}", hd_id_meta_indicador_ficha.Value)

            Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            Dim hd_deliverable_reported As HiddenField = ItemD.FindControl("hd_deliverable_reported")
            hd_deliverable_reported.Value = cls_Deliverable.get_Indicator_Reports_Tot(hd_id_meta_indicador_ficha.Value)


            txt_val.ID = strControlName
            txt_val.Value = CDbl(hd_deliverable_reported.Value)
            txt_val.MaxValue = CDbl(hd_Val.Value) + CDbl(hd_deliverable_reported.Value)
            txt_val.EmptyMessage = CDbl(hd_Val.Value) + CDbl(hd_deliverable_reported.Value)

            'Dim rept_Reports As Repeater = ItemD.FindControl("reptTable_Ind")
            'rept_Reports.DataSource = cls_Deliverable.get_Indicator_Reports(hd_id_meta_indicador_ficha.Value)
            'rept_Reports.DataBind()

        End If


    End Sub

    'Protected Sub reptTable_Ind_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) 'Handles reptTable.ItemDataBound

    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

    '        Dim ItemD As RepeaterItem
    '        ItemD = CType(e.Item, RepeaterItem)

    '        Dim hd_id_meta_indicador_ficha As HiddenField = ItemD.FindControl("hd_id_meta_indicador_ficha")
    '        Dim hd_id_avance_meta_indicador As HiddenField = ItemD.FindControl("hd_id_avance_meta_indicador")

    '        Dim strControlName As String = "txt_RepValue_" '& hd_id_meta_indicador_ficha.Value.ToString

    '        Dim hd_Val As HiddenField = ItemD.FindControl("hd_ind_value") 'Reported values
    '        Dim hd_report_value As HiddenField = ItemD.FindControl("hd_ind_report_value") 'Total Repor

    '        hd_report_value.ID = "hd_ind_report_value_" & hd_id_avance_meta_indicador.Value
    '        hd_Val.ID = "hd_ind_value_" & hd_id_avance_meta_indicador.Value

    '        Dim txt_val As RadNumericTextBox = ItemD.FindControl("txt_RepValue_") 'ItemD.FindControl(strControlName)
    '        txt_val.ClientEvents.OnValueChanged = String.Format("Calculate.indice(""{0}||{1}"")", hd_id_meta_indicador_ficha.Value.ToString, hd_id_avance_meta_indicador.Value.ToString)
    '        'txt_val.ID = strControlName
    '        txt_val.Value = CDbl(hd_Val.Value) 'Reported value
    '        txt_val.MaxValue = CDbl(hd_report_value.Value) 'Total the report value
    '        txt_val.EmptyMessage = CDbl(hd_report_value.Value) 'Total the report value MAx to report

    '    End If


    'End Sub

    Protected Sub chkVisible_CheckedChangedDOCS(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Me.hd_id_doc_support.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("value"))

        Dim id_tp As Integer = Me.hd_id_doc_support.Value
        Dim boolCHK_funded As Boolean = False

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)


            If chkvisible.Checked = True Then

                If Irow("id_doc_soporte").Text = id_tp Then

                    RadSync_NewFile.Enabled = True
                    RadSync_NewFile.AllowedFileExtensions = Irow("extension").Text.Trim.Replace(" ", "").Split(",")
                    'RadSync_NewFile.AllowedFileExtensions = Strings.Split("xls,doc,pdf,xlsx,docx", ",")
                    RadSync_NewFile.MaxFileSize = (1024 * Convert.ToDouble(Irow("colm_max_size").Text) * 1000) ' 1MG
                    boolCHK_funded = True
                Else
                    chkvisible.Checked = False
                End If

            End If
        Next

        If rdListBox_files.Items.Count <> Convert.ToInt32(Me.hd_has_files.Value) Then 'means that something has changed

            If Val(Me.cmb_approvals.SelectedValue) > 0 Then
                cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
                Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(Val(Me.cmb_approvals.SelectedValue), If(Me.hd_files_selected.Value = "0", "", Me.hd_files_selected.Value))
                Me.grd_documentos.DataBind()
            End If

        End If

        If boolCHK_funded Then
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "div_Control(false)", True)
        End If


    End Sub

    <Web.Services.WebMethod()>
    Public Shared Function get_DocTYPE(ByVal idProgram As Integer, ByVal id_TipoDoc As Integer, ByVal IdDocs As String) As Object

        Dim cls_Deliverable = New APPROVAL.clss_Deliverable(idProgram)

        Dim tbl_DOCS As DataTable = New DataTable
        tbl_DOCS = cls_Deliverable.get_Doc_support_Route_Deliverable(CType(id_TipoDoc, Integer), IdDocs)

        Dim JsonResult As String
        Dim serializer As New JavaScriptSerializer()

        Dim list_TypeDOC As Object = (From dr In tbl_DOCS.AsEnumerable() Select (New With {
                                                Key .id_doc_soporte = dr.Field(Of Int32)("id_doc_soporte"),
                                                Key .nombre_documento = dr.Field(Of String)("nombre_documento"),
                                                Key .id_programa = dr.Field(Of Int32)("id_programa"),
                                                Key .Template = dr.Field(Of String)("Template"),
                                                Key .extension = dr.Field(Of String)("extension"),
                                                Key .id_app_docs = dr.Field(Of Int32)("id_app_docs"),
                                                Key .id_tipoDocumento = dr.Field(Of Int32)("id_tipoDocumento"),
                                                Key .PermiteRepetir = dr.Field(Of String)("PermiteRepetir"),
                                                Key .RequeridoInicio = dr.Field(Of String)("RequeridoInicio"),
                                                Key .RequeridoFin = dr.Field(Of String)("RequeridoFin"),
                                                Key .environmental = dr.Field(Of Integer)("environmental"),
                                                Key .deliverable = dr.Field(Of Integer)("deliverable"),
                                                Key .max_size = dr.Field(Of Decimal)("max_size")})).ToList()

        JsonResult = serializer.Serialize(list_TypeDOC)

        get_DocTYPE = JsonResult

    End Function

    'Private Sub cmb_approvals_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_approvals.SelectedIndexChanged

    '    cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

    '    If e.Value IsNot Nothing Then
    '        Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(e.Value, If(Me.hd_files_selected.Value = "0", "", Me.hd_files_selected.Value))
    '        Me.grd_documentos.DataBind()
    '    End If



    'End Sub

    Private Sub grd_documentos_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_documentos.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim visible As New CheckBox
            Dim hlk_ref As New HyperLink

            visible = itemD("colm_select").FindControl("chkSelect")
            visible.Checked = False
            visible.InputAttributes.Add("value", itemD("id_doc_soporte").Text)

            If Convert.ToInt32(Me.hd_id_doc_support.Value) > 0 Then
                If Convert.ToInt32(itemD("id_doc_soporte").Text) = Convert.ToInt32(Me.hd_id_doc_support.Value) Then
                    visible.Checked = True
                End If
            End If

            Dim str As String = visible.InputAttributes.Item("value")

            visible.ToolTip = "Select a document"

            hlk_ref = itemD("colm_template").FindControl("hlk_template")

            If Not itemD("Template").Text.Contains("--none--") Then
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            Else
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "#"
            End If

        End If

    End Sub

    Public Sub Check_del_Ones()

        Dim tbl_Suppor_docs As New DataTable
        Dim boolDeleted As Boolean = True
        If Convert.ToInt32(Me.hd_has_files.Value) > 0 Then 'Do we need to looking for the deleted Ones

            Dim i As Integer = 0
            tbl_Suppor_docs = cls_Deliverable.Deliv_Support_Docs(Me.hd_id_deliverable.Value)

            For Each dtRow In tbl_Suppor_docs.Rows

                For Each item As RadListBoxItem In rdListBox_files.Items

                    If dtRow("id_doc_soporte") = item.Value And dtRow("archivo").ToString.Trim = item.Text.Trim Then
                        boolDeleted = False ' It´s not new one
                        Exit For
                    End If

                Next

                If boolDeleted Then 'It was already deleted by DB

                    If DelFileParam(dtRow("archivo").ToString.Trim, "\FileUploads\ApprovalProcc\") Then
                        'Delete from the DB
                        cls_Deliverable.Deliv_Support_Docs_del(dtRow("id_deliverable_support_docs"))

                    End If

                End If

                boolDeleted = True

            Next

        End If




    End Sub

    Public Sub Check_NewFile_FileUploaded()

        'Watch it on the server do you need to have \FileUploads\Temp\, but on developer side you need to try with FileUploads\Temp\

        Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"
        Dim dmyhm As String
        Dim extension As String
        Dim fileNameWE As String
        Dim fileNew_name As String
        Dim File As String
        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

        Try

            If Convert.ToBoolean(Me.lbl_hasFiles.Value) Then 'if it has values

                lbl_errExtension.Visible = False

                Dim boolFind As Boolean = False
                Dim tbl_Suppor_docs As New DataTable
                If Convert.ToInt32(Me.hd_has_files.Value) > 0 Then 'Do we need to looking for the new Ones
                    tbl_Suppor_docs = cls_Deliverable.Deliv_Support_Docs(Me.hd_id_deliverable.Value)
                End If

                Dim i As Integer = 0
                For Each item As RadListBoxItem In rdListBox_files.Items

                    'Dim a = item.Text
                    'Dim b = item.Value
                    'item.Checked = True

                    dmyhm = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                    extension = System.IO.Path.GetExtension(item.Text.Trim)
                    fileNameWE = System.IO.Path.GetFileNameWithoutExtension(item.Text.Trim)

                    If Convert.ToInt32(Me.hd_has_files.Value) > 0 Then 'Do we need to looking for the new Ones

                        For Each dtRow In tbl_Suppor_docs.Rows

                            If dtRow("id_doc_soporte") = item.Value And dtRow("archivo").ToString.Trim = item.Text.Trim Then

                                boolFind = True ' It´s not new one
                                Exit For

                            End If


                        Next

                    End If

                    If Not boolFind Then

                        Dim FileOrginal As String
                        FileOrginal = If((fileNameWE.Length > 13), fileNameWE.Substring(13, fileNameWE.Length - 13), fileNameWE)

                        If FileOrginal.Length > 80 Then
                            fileNew_name = FileOrginal.Substring(0, 80)
                        Else
                            fileNew_name = FileOrginal
                        End If

                        File = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", hd_id_deliverable.Value, Me.Session("E_IdUser"), dmyhm, FileOrginal.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-").Replace("'", "").Replace("""", "").Replace("#", ""), "_v1.1", extension)

                        Dim file_info As New IO.FileInfo(sFileDirTemp & fileNameWE & extension)

                        Try

                            If (file_info.Exists) Then

                                file_info.CopyTo(sFileDir & File)
                                DelFileParam(fileNameWE & extension)
                            Else

                                file_info = New IO.FileInfo(sFileDirTemp & FileOrginal & extension)
                                file_info.CopyTo(sFileDir & File)
                                DelFileParam(FileOrginal & extension)

                            End If

                            rdListBox_files.Items.Item(i).Text = File

                            Dim tbl_ta_documento_deliverable As New ta_deliverable_support_docs

                            tbl_ta_documento_deliverable.id_deliverable = Me.hd_id_deliverable.Value
                            tbl_ta_documento_deliverable.archivo = File
                            tbl_ta_documento_deliverable.id_doc_soporte = CType(item.Value, Integer)
                            tbl_ta_documento_deliverable.ver = 1

                            If cls_Deliverable.Save_deliverable_support_docs(tbl_ta_documento_deliverable, 0) <> -1 Then
                                'Error here
                            End If

                        Catch ex As Exception

                            lblerr_user.Text = ex.Message

                        End Try


                    End If

                    i += 1
                    boolFind = False

                Next

            Else

                lbl_errExtension.Visible = True

            End If



        Catch ex As Exception
            lblerr_user.Text = "NewFile: " & ex.Message
        End Try


    End Sub

    Public Function DelFileParam(ByVal archivo As String, Optional strPath As String = "") As Boolean
        Dim sFileName As String = System.IO.Path.GetFileName(archivo)
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"

        Try

            If strPath.Length > 1 Then
                sFileDir = Server.MapPath("~") & strPath.Trim
            End If

            Dim file_info As New IO.FileInfo(sFileDir + sFileName)
            If (file_info.Exists) Then
                file_info.Delete()
                DelFileParam = True
            Else
                DelFileParam = False
            End If

        Catch ex As Exception
            DelFileParam = False
        End Try

    End Function

End Class
