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


Partial Class frm_DeliverableAdd
    Inherits System.Web.UI.Page

    Protected cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIVERABLE_ADD"

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

            'If Me.grd_cate.Items.Count() > 0 Then
            '    Me.cmb_rol.Enabled = False
            'Else
            '    Me.cmb_rol.Enabled = True
            'End If

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

        Dim tbl_user As DataTable = cls_Deliverable.get_t_usuarios_Implementer(Me.Session("E_IdUser"))

        If tbl_user.Rows.Count > 0 Then

            userName = tbl_user.Rows.Item(0).Item("nombre_usuario")
            userImplementer = tbl_user.Rows.Item(0).Item("nombre_ejecutor")

        Else

            userName = "----"
            userImplementer = "----"

        End If

        Dim tbl_ActivitiesImplementer As DataTable = cls_Deliverable.get_Activities_Implementer(tbl_user.Rows.Item(0).Item("id_ejecutor"))

        Me.cmb_activity.DataSourceID = ""
        Me.cmb_activity.DataSource = tbl_ActivitiesImplementer
        'Me.cmb_activity.DataTextField = "codigo_SAPME"
        'Me.cmb_activity.DataValueField = "id_ficha_proyecto"
        Me.cmb_activity.DataBind()

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

            Me.dvNEXT_delieverable2.Visible = False
            Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")
            Me.anchorResults.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorDocuments.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        End If



    End Sub




    Public Sub SetDeliverable(ByVal idDeliverable As Integer)

        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

        Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)

        Me.hd_tasa_cambio.Value = cls_Deliverable.get_ExchangeRate()

        userName = Tbl_deliverable.Rows.Item(0).Item("createdBy")
        userImplementer = Tbl_deliverable.Rows.Item(0).Item("Implementer")

        Me.cmb_activity.SelectedValue = Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto")

        Me.lbl_activity_Code.Text = Tbl_deliverable.Rows.Item(0).Item("codigo_SAPME")
        Me.lbl_activity_name.Text = Tbl_deliverable.Rows.Item(0).Item("nombre_proyecto")

        Me.txt_description.Text = Tbl_deliverable.Rows.Item(0).Item("description")
        Me.txt_notes.Text = Tbl_deliverable.Rows.Item(0).Item("notes")

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

        Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")

        If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") > 1 Then

            Me.anchorResults.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorResults.Attributes.Add("href", "frm_DeliverableEdit.aspx?ID=" & idDeliverable)

            Me.anchorDocuments.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorDocuments.Attributes.Add("href", "frm_DeliverableDocs.aspx?ID=" & idDeliverable)

            Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorFollowUp.Attributes.Add("href", "frm_DeliverableFollowing.aspx?ID=" & idDeliverable)

        Else

            'looking for result
            If cls_Deliverable.Deliv_Result(idDeliverable) > 0 Then
                Me.anchorResults.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorResults.Attributes.Add("href", "frm_DeliverableEdit.aspx?ID=" & idDeliverable)
            Else
                Me.anchorResults.Attributes.Add("class", "btn btn-default btn-circle disabled")
            End If


            Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)

            If tbl_result.Rows.Count > 0 Then
                Me.anchorDocuments.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorDocuments.Attributes.Add("href", "frm_DeliverableDocs.aspx?ID=" & idDeliverable)
            Else
                Me.anchorDocuments.Attributes.Add("class", "btn btn-default btn-circle disabled")
            End If


            Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Docs(idDeliverable)

            If tbl_Suppor_docs.Rows.Count > 0 Then
                Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
                Me.anchorFollowUp.Attributes.Add("href", "frm_DeliverableFollowing.aspx?ID=" & idDeliverable)
            Else
                Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
            End If


        End If


        If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 5 Or Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 1 Then 'Created and Observation Pending
            Me.btnlk_continue.Enabled = True
        Else
            Me.btnlk_continue.Enabled = False
        End If

        If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 5 Then
            Dim tbl_Deliverable_detail As DataTable = cls_Deliverable.Deliv_Document_detail(idDeliverable)
            Dim idDocumento As Integer = CInt(tbl_Deliverable_detail.Rows(0).Item("id_documento"))

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Dim tbl_AppSTEP As New DataTable
            tbl_AppSTEP = clss_approval.get_ta_AppDocumentoAPP_MAX(idDocumento) 'To get the info on the max step (id_app_doc)

            Me.txt_observation.Text = String.Format("{0}", tbl_AppSTEP.Rows(0).Item("observacion"))
            Me.divObservation.Visible = True

        Else
            Me.divObservation.Visible = False
        End If


        Dim strArray As String() = GetDeliverable_(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable).Split("||")

        ltr_rows_Deliverables.InnerHtml = strArray(0)
        lbl_totalACT_usd.Text = strArray(2)
        lbl_totalACT.Text = strArray(4)
        lbl_totalPend_usd.Text = strArray(6)
        lbl_totalPend.Text = strArray(8)
        lbl_totalPerf_usd.Text = strArray(10)
        lbl_totalPerf.Text = strArray(12)

        lbl_period.Text = String.Format(" {0:d} to {1:d} ", Tbl_deliverable.Rows.Item(0).Item("fecha_inicio_proyecto"), Tbl_deliverable.Rows.Item(0).Item("fecha_fin_proyecto"))
        'dvNEXT_delieverable.InnerHtml =
        'Dim idFichaProyecto As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"))
        'Dim Tbl_LASTdeliv As DataTable = cls_Deliverable.get_Last_Deliverable(idFichaProyecto, 0, idDeliverable)
        Dim Tbl_DelivINFO As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)
        Me.rep_DelivINFO.DataSource = Tbl_DelivINFO
        Me.rep_DelivINFO.DataBind()

        hd_id_ficha_entregable.Value = Tbl_deliverable.Rows.Item(0).Item("id_ficha_entregable")
        Me.hd_percent.Value = strArray(14)

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "$('#deliv_tab').addClass('collapse'); set_Percent(" & strArray(14) & ");", True)

    End Sub






    Private Sub btnlk_continue_Click(sender As Object, e As EventArgs) Handles btnlk_continue.Click

        Dim err As Boolean = False
        Dim ext As String = ""

        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
        Dim deliverable As ta_deliverable = New ta_deliverable

        If hd_id_deliverable.Value > 0 Then
            deliverable = cls_Deliverable.get_Deliverable(hd_id_deliverable.Value)
        End If

        deliverable.description = txt_description.Text.Trim
        deliverable.notes = txt_notes.Text.Trim
        deliverable.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        deliverable.fecha_creo = Date.UtcNow
        deliverable.usuario_creo = Convert.ToInt32(Me.Session("E_IdUser"))
        deliverable.id_ficha_entregable = hd_id_ficha_entregable.Value

        If hd_id_deliverable.Value = 0 Then
            deliverable.id_deliverable_estado = 1
        End If

        Try

            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

            'Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Me.hd_id_deliverable.Value)
            Dim result = cls_Deliverable.Save_deliverable(deliverable, Me.hd_id_deliverable.Value)

            If result <> -1 Then

                Me.Response.Redirect("~/Deliverable/frm_DeliverableEdit.aspx?ID=" & result.Trim)

            Else 'Error

                Me.lblError.Text = String.Format("Error saving Deliverable, please contact to the system administrator <br /><br /> Detail:{0}", result)
                Me.lblError.Visible = True

            End If

        Catch ex As Exception
            Me.lblError.Text = String.Format("Error saving Deliverable, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
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

    Protected Sub cmb_activity_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(cmb_activity.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(cmb_activity.Items.Count)
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
                                   <td  rowspan='4'><div class='tools'><a href='/RMS_APPROVAL/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
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

        Dim YLAfundingUSD As Double = 0
        Dim PerformedFundingUSD As Double = 0
        Dim PendingFundingUSD As Double = 0

        Dim YLAfunding As Double = 0
        Dim PerformedFunding As Double = 0
        Dim PendingFunding As Double = 0
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




    <Web.Services.WebMethod()>
    Public Shared Function GetDeliverables(ByVal idFichaProyecto As Integer, ByVal idPrograma As Integer) As String

        Dim cls_Deliverable = New APPROVAL.clss_Deliverable(idPrograma)

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim ExchangeRate As Double = cls_Deliverable.get_ExchangeRate()

        Dim tbl_Deliverables As DataTable = cls_Deliverable.get_Deliverable_Activity(idFichaProyecto, 0)

        Dim strRowsTOT As String = ""
        Dim strRows As String = "<tr>
                                   <td  rowspan='4'><div class='tools'><a href='/RMS_APPROVAL/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
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
                                                                  <td> {7}/ {8:N2} USD</td>   
                                                                </tr>                                                               
                                                              </table>"



        Dim strTable_nextDEL As String = ""
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


        For Each dtRow As DataRow In tbl_Deliverables.Rows

            Dim rDays As Double

            YLAfunding = dtRow("monto_aporte")

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

                    strTime = Func_Unit(rDays)
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                    strStatus = dtRow("deliverable_estado")

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
                strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), Math.Round((dtRow("valor") / ExchangeRate), 2, MidpointRounding.AwayFromZero))

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

        GetDeliverables = String.Format(strTable, strRowsTOT.Trim) & "||" &
                          String.Format("{0:N2} USD", YLAfundingUSD) & "||" &
                          String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", YLAfunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                          String.Format("{0:N2} USD", Math.Round(PendingFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                          String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PendingFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                          String.Format("{0:N2} USD", Math.Round(PerformedFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                          String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PerformedFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                          String.Format("{0:N2}", PorcenPerformed) & "||" &
                          strTable_nextDEL.Trim & "||" &
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

End Class
