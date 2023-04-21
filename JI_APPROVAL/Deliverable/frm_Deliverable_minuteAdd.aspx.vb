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
Imports System.Drawing

Partial Class frm_Deliverable_minuteAdd
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIV_MIN_ADD"

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval

    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim cls_Deliverable As APPROVAL.clss_Deliverable
    Dim clDate As APPROVAL.cls_dUtil

    Public userName As String = ""
    Public userImplementer As String = ""

    Protected Status_TS As String = ""
    Protected DateStatus_TS As String = ""
    Protected HourStatus_TS As String = ""
    Public cultureUSer As CultureInfo


    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Dim dtTipoAPP As DataTable

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

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtTipoAPP.Value = String.Format("dtTipoAPP{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

            dtTipoAPP = cls_Deliverable.get_ApprovalEstado_tipo()

            Session(Me.hd_dtTipoAPP.Value) = dtTipoAPP

            LoadData(hd_id_deliverable.Value)

        Else

            ' ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)
            dtTipoAPP = Session(Me.hd_dtTipoAPP.Value)

        End If


    End Sub


    Public Sub LoadData(ByVal idDeliverable As Integer)

        Using dbEntities As New dbRMS_JIEntities

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

            Dim id_programa As Integer = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim departametos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.nombre_departamento).ToList()

            Me.cmb_departamento.DataSourceID = ""
            Me.cmb_departamento.DataSource = departametos
            Me.cmb_departamento.DataTextField = "nombre_departamento"
            Me.cmb_departamento.DataValueField = "id_departamento"
            Me.cmb_departamento.DataBind()

            Me.cmb_accountability.DataSourceID = ""
            Me.cmb_accountability.DataSource = dbEntities.ta_clin_codes.Where(Function(p) p.id_programa = id_programa).ToList
            Me.cmb_accountability.DataTextField = "clin_description"
            Me.cmb_accountability.DataValueField = "id_clin_code"
            Me.cmb_accountability.DataBind()

            Me.cmb_offices.DataSourceID = ""
            Me.cmb_offices.DataSource = dbEntities.ta_offices.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_offices.DataTextField = "office_name"
            Me.cmb_offices.DataValueField = "id_office"
            Me.cmb_offices.DataBind()

            Dim tbl_Deliverable As DataTable = cls_Deliverable.get_Deliverables(hd_id_deliverable.Value)
            Dim tbl_user As DataTable = cls_Deliverable.get_t_usuarios_Implementer(Me.Session("E_IdUser"))

            If tbl_user.Rows.Count > 0 Then

                userName = tbl_user.Rows.Item(0).Item("nombre_usuario")
                userImplementer = tbl_user.Rows.Item(0).Item("nombre_ejecutor")

            Else

                userName = "----"
                userImplementer = "----"

            End If

            ' Dim tbl_ActivitiesImplementer As DataTable = cls_Deliverable.get_Activities_Implementer(tbl_user.Rows.Item(0).Item("id_ejecutor"))
            'If tbl_ActivitiesImplementer.Rows.Count > 0 Then

            'Me.lbl_activity_Code.Text = tbl_ActivitiesImplementer.Rows.Item(0).Item("codigo_SAPME")
            '    Me.lbl_activity_name.Text = tbl_ActivitiesImplementer.Rows.Item(0).Item("nombre_proyecto")

            'Else

            Me.lbl_activity_Code.Text = "--"
            Me.lbl_activity_name.Text = "--"

            'End If

            Dim fechaHOY As DateTime = Date.UtcNow
            '************************************SYSTEM INFO********************************************
            Dim cProgram As New SIMEly.cls_ProgramSIME
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(cl_user.Id_Cprogram, True)
            Dim userCulture As CultureInfo
            Dim timezoneUTC As Integer
            userCulture = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
            clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            Status_TS = "DELIVERABLE APPROVAL PROCESS NOT CREATED"
            DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(fechaHOY, "m", timezoneUTC, False))
            HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(fechaHOY, timezoneUTC, True))
            Me.lbl_approval_route.Text = "DELIVERABLE APPROVAL PROCESS NOT CREATED"

            If idDeliverable > 0 Then
                SetDeliverable(idDeliverable)
            End If

        End Using

    End Sub




    Public Sub SetDeliverable(ByVal idDeliverable As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)
            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)

            Dim idFichaProyecto As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"))
            Dim oFichaProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_ficha_proyecto = idFichaProyecto).FirstOrDefault()
            Dim oEjecutores = dbEntities.t_ejecutores.Where(Function(p) p.id_ejecutor = oFichaProyecto.id_ejecutor).FirstOrDefault()
            Dim oUsuario_Responsable = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_usuario = oFichaProyecto.id_usuario_responsable).FirstOrDefault()

            Me.hd_id_deliverable_minute.Value = Tbl_deliverable.Rows.Item(0).Item("id_deliverable_minute")

            If Val(Me.hd_id_deliverable_minute.Value) > 0 Then

                Dim idM As Integer = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
                Dim oMinute As ta_deliverable_minute = dbEntities.ta_deliverable_minute.Find(idM)

                Me.cmb_accountability.SelectedValue = oMinute.id_clin_code

                Dim oClin = dbEntities.ta_clin_codes.Where(Function(p) p.id_clin_code = oMinute.id_clin_code).FirstOrDefault()
                Me.lbl_CLIN.Text = String.Format("CLIN {0}", oClin.clin_code)
                Me.lbl_GL.Text = String.Format("GL {0}", oClin.GL_code)

                Me.cmb_departamento.SelectedValue = dbEntities.t_municipios.Where(Function(p) p.id_municipio = oMinute.id_municipio).FirstOrDefault.id_departamento
                Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataSource = municipios
                Me.cmb_municipio.DataTextField = "nombre_municipio"
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataBind()
                Me.cmb_municipio.SelectedValue = oMinute.id_municipio

                Me.cmb_offices.SelectedValue = oMinute.id_office
                Me.txt_notes.Text = oMinute.minute_comment

                If oMinute.local_currency.HasValue Then
                    If oMinute.local_currency Then 'COP
                        Me.chk_data_in.Checked = False
                        'Me.currency_entry.InnerHtml = "COP"
                    Else 'USD
                        Me.chk_data_in.Checked = True
                        'Me.currency_entry.InnerHtml = "USD"
                    End If
                Else 'COP
                    Me.chk_data_in.Checked = False
                    'Me.currency_entry.InnerHtml = "COP"
                End If


                Me.btnlk_print_preview.Enabled = True
                If oFichaProyecto.id_mecanismo_contratacion = 3 Then 'Grant
                    Me.btnlk_print_preview.NavigateUrl = "~/Deliverable/frm_Deliverable_minutePrintG.aspx?ID=" & Me.hd_id_deliverable.Value
                Else
                    Me.btnlk_print_preview.NavigateUrl = "~/Deliverable/frm_Deliverable_minutePrint.aspx?ID=" & Me.hd_id_deliverable.Value
                End If


                If oMinute.minute_close = True Then
                    Me.sp_code_minute.InnerHtml = oMinute.minute_code
                    Me.btnlk_generate_code.Enabled = False
                    Me.btnlk_continue.Enabled = False
                Else
                    Me.sp_code_minute.InnerHtml = "-----------"
                End If

            Else

                Me.btnlk_generate_code.Enabled = False
                Me.btnlk_print_preview.Enabled = False
                Me.btnlk_print_preview.NavigateUrl = ""
                'Me.currency_entry.InnerHtml = "COP"
                Me.chk_data_in.Checked = False

            End If

            Me.hd_tasa_cambio.Value = cls_Deliverable.get_ExchangeRate()

            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)
            Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
            Me.curr_International.Value = "USD"
            Me.txt_tasa_cambio.Value = Convert.ToDouble(Tbl_deliverable.Rows.Item(0).Item("tasa_cambio_final"))
            Me.hd_exchange_Rate.Value = Convert.ToDouble(Tbl_deliverable.Rows.Item(0).Item("tasa_cambio_final"))
            Me.hd_value_deliverable.Value = Tbl_deliverable.Rows.Item(0).Item("valor_final")


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
            Dim cProgram As New SIMEly.cls_ProgramSIME
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(cl_user.Id_Cprogram, True)
            Dim userCulture As CultureInfo
            Dim timezoneUTC As Integer
            userCulture = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
            clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            Status_TS = Tbl_deliverable.Rows.Item(0).Item("deliverable_estado")

            'If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") <= 1 Then
            '    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), "m", timezoneUTC, False))
            '    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), timezoneUTC, True))
            'Else
            '    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), "m", timezoneUTC, False))
            '    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), timezoneUTC, True))
            'End If

            If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") <= 1 Then
                DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), "m", timezoneUTC, False))
                HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_creo"), timezoneUTC, True))
            Else
                If Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 3 Or
                   Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 4 Or
                   Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado") = 6 Then 'whn was Closed

                    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("delivered_date"), "m", timezoneUTC, False))
                    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("delivered_date"), timezoneUTC, True))

                Else 'Last updated

                    DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), "m", timezoneUTC, False))
                    HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(Tbl_deliverable.Rows.Item(0).Item("fecha_upd"), timezoneUTC, True))

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

            Me.rep_DelivINFO.DataSource = Tbl_deliverable  'Tbl_DelivINFO 'Deliverable INFO
            Me.rep_DelivINFO.DataBind()

            hd_id_ficha_entregable.Value = Tbl_deliverable.Rows.Item(0).Item("id_ficha_entregable")

            ''Me.reptTable.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable)
            ''Me.reptTable.DataBind()

            Dim id_user As Integer = Convert.ToInt32(Me.Session("E_IdUser"))
            'Me.cmb_approvals.DataSource = cls_Deliverable.get_Deliverable_ApprovalUser(id_user) 'cls_TimeSheet.get_TimeSheetApprovalUser(timesheet.id_usuario)
            'Me.cmb_approvals.DataTextField = "descripcion_aprobacion"
            'Me.cmb_approvals.DataValueField = "id_tipoDocumento"
            'Me.cmb_approvals.DataBind()

            Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)
            If tbl_result.Rows.Count > 0 Then

                'Dim tbl_rutaAPP As DataTable = cls_Deliverable.get_Deliverable_ApprovalSEL(Tbl_deliverable.Rows.Item(0).Item("usuario_creo"), tbl_result.Rows.Item(0).Item("id_tipoDocumento"))
                'If tbl_rutaAPP.Rows.Count > 0 Then
                '    Me.lbl_approval_route.Text = tbl_rutaAPP.Rows.Item(0).Item("descripcion_aprobacion")
                'Else
                '    Me.lbl_approval_route.Text = ""
                'End If
                'Me.cmb_approvals.SelectedValue = tbl_result.Rows.Item(0).Item("id_tipoDocumento")
                Me.hd_id_documento_deliverable.Value = tbl_result.Rows.Item(0).Item("id_documento_deliverable")
                Me.hd_id_documento.Value = tbl_result.Rows.Item(0).Item("id_documento")

                Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.hd_id_documento.Value)
                Me.grd_cate.DataBind()

            Else

                Me.hd_id_documento_deliverable.Value = 0

            End If

            Me.lbl_beneficiario.Text = oFichaProyecto.nombre_ejecutor
            Me.lbl_Activity.Text = String.Format("{0} No.", oFichaProyecto.nombre_mecanismo_contratacion)
            Me.lbl_Activity_Code_2.Text = oFichaProyecto.codigo_ficha_AID

            Me.lbl_pay_number.Text = String.Format("Pago No. {0} ", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"))
            'Me.lbl_pay_value.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", Tbl_deliverable.Rows.Item(0).Item("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
            Me.lbl_OTR.Text = String.Format("{0} ({1})", oUsuario_Responsable.nombre_usuario, oUsuario_Responsable.job)
            Me.lbl_approval_route.Text = String.Format(" {1} - Deliverable No. {0} ", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"), oFichaProyecto.codigo_ficha_AID)

            Me.txt_beneficiary.Text = oEjecutores.nombre_ejecutor
            Me.txt_billing_info.Text = oEjecutores.billing_info
            Me.txt_number_NIT.Text = oEjecutores.nit

            'Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Documents_detail(idDeliverable)
            'Me.rpt_support_docs.DataSource = tbl_Suppor_docs
            'Me.rpt_support_docs.DataBind()

            'Dim Deliv_Estado As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado"))
            'If Deliv_Estado > 1 Then 'In Approval Process/Approved/Rejected
            '    Using db As New dbRMS_JIEntities
            '        Dim idDoc As Integer = db.ta_documento_deliverable.Where(Function(p) p.id_deliverable = idDeliverable).FirstOrDefault().id_documento
            '        Me.rept_msgApproval.DataSource = clss_approval.get_Document_Comments_special(idDoc)
            '        Me.rept_msgApproval.DataBind()
            '    End Using
            'End If

            Me.hd_percent.Value = strArray(14)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & "); Currency_input(); ", True)


        End Using

    End Sub

    Protected Sub cmb_activity_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.

        Dim DateINI As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_inicio_proyecto").ToString(), Date)
        Dim DateFIN As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_fin_proyecto").ToString(), Date)

        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", (DirectCast(e.Item.DataItem, DataRowView))("codigo_SAPME").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("nombre_proyecto").ToString(), String.Format("{0:d}", DateINI), String.Format("{0:d}", DateFIN))
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_ficha_proyecto").ToString()

    End Sub



    Private Function GetDeliverable_(ByVal idFichaProyecto As Integer, Optional id_deliverable As Integer = 0) As String

        Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDPrograma")))

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim ExchangeRate As Double = cls_Deliverable.get_ExchangeRate()

        Dim tbl_Deliverables As DataTable = cls_Deliverable.get_Deliverable_Activity(idFichaProyecto, 0)

        Dim strRowsTOT As String = ""
        Dim strRows As String = "<tr>
                                   <td><div class='tools'><a href='~/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
                                   <td>{0}</td>
                                   <td>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                          {1}
                                      </div>
                                   </td>
                                   <td>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                         {2}
                                      </div>
                                   </td>
                                   <td>{3:d}</td>
                                   <td>{4:d}</td>
                                   <td>{5:P2}</td>
                                   <td>{6}</td>
                                   <td>                                                                       
                                     <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
                                   </td>
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

            'strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), dtRow("valor"), strStatus, strTime, strAlert, dtRow("id_deliverable"))
            'strRowsTOT &= String.Format(strRows2, strAlert2, vDays, strAlert3, vDiferences_Adj, vDiferences)

            strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, dtRow("id_deliverable"))
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

        Else

            generate_CODE = "--No Code--"

        End If

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




    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New SIMEly.cls_ProgramSIME
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
        Dim cProgram As New SIMEly.cls_ProgramSIME
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

    Private Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged

        If Val(e.Value) > 0 Then

            Using dbEntities As New dbRMS_JIEntities
                Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = e.Value).ToList()
                Me.cmb_municipio.DataSource = municipios
                Me.cmb_municipio.DataTextField = "nombre_municipio"
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataBind()
                Me.cmb_municipio.SelectedIndex = -1
                Me.cmb_municipio.Text = ""

            End Using

        End If

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)

    End Sub

    Private Sub cmb_accountability_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_accountability.SelectedIndexChanged

        If Val(e.Value) > 0 Then
            Using dbEntities As New dbRMS_JIEntities

                Dim oClin = dbEntities.ta_clin_codes.Where(Function(p) p.id_clin_code = e.Value).FirstOrDefault()
                Me.lbl_CLIN.Text = String.Format("CLIN {0}", oClin.clin_code)
                Me.lbl_GL.Text = String.Format("GL {0}", oClin.GL_code)

            End Using
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)

    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"

            hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
            If itemD("descripcion_estado").Text = "Pending" Then
                For i As Integer = 2 To 8
                    e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
                Next
            End If

            Dim cmb_tp As RadComboBox = CType(itemD("colm_app_tp").FindControl("cmb_app_tp"), RadComboBox)
            Dim hd_id_app_control As HiddenField = CType(itemD("colm_id_app").FindControl("hd_id_deliverable_minute_app"), HiddenField)

            cmb_tp.DataSource = dtTipoAPP
            cmb_tp.DataTextField = "estado_tipo_prefijo"
            cmb_tp.DataValueField = "id_estadoTipo"
            cmb_tp.DataBind()

            Using dbEntities As New dbRMS_JIEntities

                Dim idRuta As Integer = Convert.ToInt32(itemD("id_ruta").Text)
                cmb_tp.SelectedValue = dbEntities.ta_rutaTipoDoc.Where(Function(p) p.id_ruta = idRuta).FirstOrDefault.id_estadoTipo
                cmb_tp.Enabled = False
                Dim chkUP As CheckBox = CType(itemD("colm_Select").FindControl("chk_select"), CheckBox)


                If Val(Me.hd_id_deliverable_minute.Value) > 0 Then

                    Dim idMin = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
                    Dim idApp = Convert.ToInt32(itemD("id_app_documento").Text)

                    If (dbEntities.ta_deliverable_minute_app.Where(Function(p) p.id_deliverable_minute = idMin And p.id_App_documento = idApp).Count() > 0) Then

                        hd_id_app_control.Value = dbEntities.ta_deliverable_minute_app.Where(Function(p) p.id_deliverable_minute = idMin And p.id_App_documento = idApp).FirstOrDefault().id_deliverable_minute_app
                        chkUP.Checked = True
                    End If

                End If

            End Using




        End If
    End Sub

    Private Sub btnlk_continue_Click(sender As Object, e As EventArgs) Handles btnlk_continue.Click

        Try

            If Save_minute() Then

                'Me.btnlk_print_preview.Enabled = True
                'Me.btnlk_print_preview.NavigateUrl = "~/Deliverable/frm_Deliverable_minutePrint.aspx?ID=" & Me.hd_id_deliverable.Value

                Session.Remove(Me.hd_dtTipoAPP.Value)
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Concat("~/Deliverable/frm_Deliverable_minuteAdd.aspx?ID=", Me.hd_id_deliverable.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End If


        Catch ex As Exception
            Session.Remove(Me.hd_dtTipoAPP.Value)
        End Try

    End Sub


    Public Function Save_minute(Optional strCode As String = "") As Boolean


        Using dbEntities As New dbRMS_JIEntities

            Dim oMinute As ta_deliverable_minute = New ta_deliverable_minute
            Dim idDeliverableMinute As Integer = Convert.ToInt32(hd_id_deliverable_minute.Value)

            If idDeliverableMinute = 0 Then

                oMinute.id_municipio = Me.cmb_municipio.SelectedValue
                oMinute.id_deliverable = Me.hd_id_deliverable.Value
                oMinute.minute_comment = Me.txt_notes.Text
                oMinute.id_office = Me.cmb_offices.SelectedValue
                oMinute.id_clin_code = Me.cmb_accountability.SelectedValue
                oMinute.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser"))
                oMinute.fecha_Creo = Date.UtcNow
                oMinute.local_currency = If(Me.chk_data_in.Checked, 0, 1)

                dbEntities.ta_deliverable_minute.Add(oMinute)

            Else

                oMinute = dbEntities.ta_deliverable_minute.Find(idDeliverableMinute)

                oMinute.id_municipio = Me.cmb_municipio.SelectedValue
                oMinute.id_deliverable = Me.hd_id_deliverable.Value
                oMinute.minute_comment = Me.txt_notes.Text
                oMinute.id_office = Me.cmb_offices.SelectedValue
                oMinute.id_clin_code = Me.cmb_accountability.SelectedValue
                oMinute.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                oMinute.fecha_upd = Date.UtcNow
                oMinute.local_currency = If(Me.chk_data_in.Checked, 0, 1)

                If strCode.Length > 1 Then
                    oMinute.minute_close = True
                    oMinute.minute_code = strCode
                End If

                dbEntities.Entry(oMinute).State = Entity.EntityState.Modified

            End If

            If (dbEntities.SaveChanges()) Then 'save

                hd_id_deliverable_minute.Value = oMinute.id_deliverable_minute

                For Each rowD As GridDataItem In Me.grd_cate.Items

                    Dim chkUP As CheckBox = CType(rowD("colm_Select").FindControl("chk_select"), CheckBox)
                    Dim hd_id_app_control As HiddenField = CType(rowD("colm_id_app").FindControl("hd_id_deliverable_minute_app"), HiddenField)


                    If chkUP.Checked = True Then

                        If hd_id_app_control.Value = 0 Then

                            Dim oMi As ta_deliverable_minute_app = New ta_deliverable_minute_app

                            oMi.id_App_documento = rowD("id_App_documento").Text
                            oMi.id_deliverable_minute = Me.hd_id_deliverable_minute.Value

                            dbEntities.ta_deliverable_minute_app.Add(oMi)
                            dbEntities.SaveChanges()

                        End If

                    Else

                        If hd_id_app_control.Value > 0 Then 'already Exist

                            dbEntities.Database.ExecuteSqlCommand("DELETE FROM ta_deliverable_minute_app WHERE id_deliverable_minute_app = " + hd_id_app_control.Value.ToString())
                            dbEntities.SaveChanges()

                        End If

                    End If

                Next

                Save_minute = True

            Else

                Save_minute = False

            End If

        End Using

    End Function

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Deliverable/frm_Deliverable.aspx")
    End Sub

    Private Sub btnlk_generate_code_Click(sender As Object, e As EventArgs) Handles btnlk_generate_code.Click

        Try


            Using dbEntities As New dbRMS_JIEntities

                Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)
                cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
                'clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                Dim idDeliverable = Convert.ToInt32(hd_id_deliverable.Value)
                Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)

                Dim idFichaProyecto As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"))
                Dim oFichaProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_ficha_proyecto = idFichaProyecto).FirstOrDefault()

                ' AC-GNT-2019-0005
                ' AC-SUB-2019-0008
                Dim dHoy As Date = Date.UtcNow
                Dim Corr As Integer = dbEntities.ta_deliverable_minute.Where(Function(p) p.minute_close = True And p.fecha_Creo.Value.Year = dHoy.Year).Count()
                Corr += 1
                Dim strCODE As String = String.Format("AC-{0}-{1}-{2}", oFichaProyecto.prefijo_mecanismo, dHoy.Year, Corr.ToString.PadLeft(4, "0"))

                If Save_minute(strCODE) Then



                    Me.btnlk_print_preview.Enabled = True

                    If oFichaProyecto.id_mecanismo_contratacion = 3 Then 'Grant
                        Me.btnlk_print_preview.NavigateUrl = "~/Deliverable/frm_Deliverable_minutePrintG.aspx?ID=" & Me.hd_id_deliverable.Value
                    Else
                        Me.btnlk_print_preview.NavigateUrl = "~/Deliverable/frm_Deliverable_minutePrint.aspx?ID=" & Me.hd_id_deliverable.Value
                    End If

                    Session.Remove(Me.hd_dtTipoAPP.Value)
                    ' Me.Response.Redirect("~/Deliverable/frm_Deliverable.aspx")
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = String.Concat("~/Deliverable/frm_Deliverable_minuteAdd.aspx?ID=", Me.hd_id_deliverable.Value)
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                End If

            End Using

        Catch ex As Exception
            Session.Remove(Me.hd_dtTipoAPP.Value)
        End Try


    End Sub

    'Private Sub btnlk_printing__Click(sender As Object, e As EventArgs) Handles btnlk_printing_.Click
    '    Me.Response.Redirect("~/Deliverable/frm_Deliverable_minutePrint.aspx?ID=" & Me.hd_id_deliverable.Value)
    'End Sub


End Class
