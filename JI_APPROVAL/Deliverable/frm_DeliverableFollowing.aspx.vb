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


Partial Class frm_DeliverableFollowing
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIVERABLE_FOLL"

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


        If idDeliverable > 0 Then

            SetDeliverable(idDeliverable)

        Else

            Me.anchorInformation.Attributes.Add("class", "btn btn-primary btn-circle")
            Me.anchorResults.Attributes.Add("class", "btn btn-default btn-circle disabled")
            Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")

        End If



    End Sub




    Public Sub SetDeliverable(ByVal idDeliverable As Integer)

        cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

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

        Me.anchorDocuments.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorDocuments.Attributes.Add("href", "frm_DeliverableDocs.aspx?ID=" & idDeliverable)

        Me.anchorFollowUp.Attributes.Add("class", "btn btn-primary btn-circle")

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

        'Me.reptTable.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), True)
        Me.reptTable.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable)
        Me.reptTable.DataBind()

        Dim id_user As Integer = Convert.ToInt32(Me.Session("E_IdUser"))
        'Me.cmb_approvals.DataSource = cls_Deliverable.get_Deliverable_ApprovalUser(id_user) 'cls_TimeSheet.get_TimeSheetApprovalUser(timesheet.id_usuario)
        'Me.cmb_approvals.DataTextField = "descripcion_aprobacion"
        'Me.cmb_approvals.DataValueField = "id_tipoDocumento"
        'Me.cmb_approvals.DataBind()

        Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)

        If tbl_result.Rows.Count > 0 Then

            Dim tbl_rutaAPP As DataTable = cls_Deliverable.get_Deliverable_ApprovalSEL(Tbl_deliverable.Rows.Item(0).Item("usuario_creo"), tbl_result.Rows.Item(0).Item("id_tipoDocumento")) 'cls_TimeSheet.get_TimeSheetApprovalUser(timesheet.id_usuario)
            'tbl_rutaAPP.Rows.Item(0).Item("descripcion_aprobacion")
            Me.lbl_approval_route.Text = tbl_rutaAPP.Rows.Item(0).Item("descripcion_aprobacion")
            'Me.cmb_approvals.SelectedValue = tbl_result.Rows.Item(0).Item("id_tipoDocumento")
            Me.hd_id_documento_deliverable.Value = tbl_result.Rows.Item(0).Item("id_documento_deliverable")

        Else

            Me.hd_id_documento_deliverable.Value = 0

        End If

        Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Documents_detail(idDeliverable)

        Me.rpt_support_docs.DataSource = tbl_Suppor_docs
        Me.rpt_support_docs.DataBind()

        'Dim tot_files As Integer = If(tbl_Suppor_docs.Rows.Count > 0, tbl_Suppor_docs.Rows.Count, 0)

        'If tot_files > 0 Then

        '    Me.hd_has_files.Value = tot_files ' this is for verify when we save it

        '    For Each dtRow As DataRow In tbl_Suppor_docs.Rows

        '        Dim lst_item As RadListBoxItem = New RadListBoxItem(dtRow("archivo"), dtRow("id_doc_soporte"))
        '        rdListBox_files.Items.Add(lst_item)
        '        Me.hd_files_selected.Value &= "," & dtRow("id_doc_soporte")

        '    Next

        '    Me.lbl_hasFiles.Value = True

        '    Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(tbl_result.Rows.Item(0).Item("id_tipoDocumento"), If(Me.hd_files_selected.Value = "0", "", Me.hd_files_selected.Value))
        '    Me.grd_documentos.DataBind()

        'Else

        '    Me.grd_documentos.DataSource = cls_Deliverable.get_Doc_support_Route_Deliverable(0, "")
        '    Me.grd_documentos.DataBind()

        'End If

        'Me.reptTable_2.DataSource = cls_Deliverable.get_Deliverable_Result(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), False)
        'Me.reptTable_2.DataBind()

        Dim Deliv_Estado As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_deliverable_estado"))

        If ((Deliv_Estado > 1 And Deliv_Estado < 5) Or (Deliv_Estado = 6)) Then 'In Approval Process/Approved/Rejected

            Using db As New dbRMS_JIEntities

                Dim idDoc As Integer = db.ta_documento_deliverable.Where(Function(p) p.id_deliverable = idDeliverable).FirstOrDefault().id_documento
                Me.rept_msgApproval.DataSource = clss_approval.get_Document_Comments_special(idDoc)
                Me.rept_msgApproval.DataBind()

            End Using

            Me.lyButtoms.Visible = False
            Me.lyHistory.Visible = True
            Me.divObservation.Visible = False
        Else 'Created /  Observations Pending

            Me.lyButtoms.Visible = True
            Me.lyHistory.Visible = False

            If Deliv_Estado = 5 Then
                Me.btnlk_continue.Text = "Apply Observation"
                Me.divObservation.Visible = True
            Else
                Me.btnlk_continue.Text = "Start Approval"
                Me.divObservation.Visible = False
            End If

        End If


        Me.hd_percent.Value = strArray(14)

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Percent(" & Convert.ToDouble(Me.hd_performed.Value) & ");", True)


    End Sub


    Private Sub btnlk_continue_Click(sender As Object, e As EventArgs) Handles btnlk_continue.Click

        Try

            Dim err As Boolean = False
            Dim id_documento As Integer = 0
            Dim id_appdocumento As Integer = 0
            Dim id_appdocumento2 As Integer = 0

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

            Dim tbl_Deliverable_detail As DataTable = cls_Deliverable.Deliv_Document_detail(Me.hd_id_deliverable.Value)
            Dim tblUserApproval_Deliverable As DataTable = cls_Deliverable.get_Deliverable_ApprovalUser(tbl_Deliverable_detail.Rows(0).Item("usuario_creo"))

            Dim strCode As String = generate_CODE(tbl_Deliverable_detail.Rows(0).Item("id_categoria"))
            Dim AppCode As String = cls_Deliverable.CrearCodigo_Deliverable(tbl_Deliverable_detail.Rows(0).Item("id_categoria"))

            Dim strDescripName As String = String.Format("Deliverable #{0} {1} ({2}), Due Date: {3:m}", tbl_Deliverable_detail.Rows(0).Item("numero_entregable"), tbl_Deliverable_detail.Rows(0).Item("Implementer"), tbl_Deliverable_detail.Rows(0).Item("codigo_SAPME"), tbl_Deliverable_detail.Rows(0).Item("DueDate"))
            Dim strDescripDeliverable As String = String.Format("{0}", tbl_Deliverable_detail.Rows(0).Item("description"))

            If tbl_Deliverable_detail.Rows(0).Item("id_deliverable_estado") = 1 Then 'Create the New record

                clss_approval.set_ta_documento(0) 'Set new Record
                clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", tbl_Deliverable_detail.Rows(0).Item("id_tipoDocumento"), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("numero_instrumento", strCode, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("descripcion_doc", strDescripName, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("nom_beneficiario", tbl_Deliverable_detail.Rows(0).Item("Implementer"), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("comentarios", strDescripDeliverable, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("codigo_AID", strCode, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", strCode, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("ficha_actividad", "NO", "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("monto_ficha", 0, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("regional", Me.Session("E_SubRegion").ToString.Trim, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("codigo_Approval", AppCode, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("id_tipoAprobacion", 4, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("monto_total", 0, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("tasa_cambio", 0, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("datecreated", Date.UtcNow, "id_documento", 0)

                id_documento = clss_approval.save_ta_documento()

                'ElseIf tbl_Deliverable_detail.Rows(0).Item("id_deliverable_estado") = 5 Then 'IT´s on StandBy

                '    Using db As New dbRMS_JIEntities

                '        id_documento = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = hd_IDtimeSheet.Value).FirstOrDefault().id_documento
                '        clss_approval.set_ta_documento(id_documento) 'Get new Record

                '    End Using
            Else

                'Using db As New dbRMS_JIEntities
                'id_documento = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = hd_IDtimeSheet.Value).FirstOrDefault().id_documento
                'End Using
                id_documento = tbl_Deliverable_detail.Rows(0).Item("id_documento")

            End If

            If id_documento <> -1 And tbl_Deliverable_detail.Rows(0).Item("id_deliverable_estado") = 1 Then 'Save New ones Docs

                Dim tbl_Route_By_DOC As New DataTable
                tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Convert.ToInt32(tbl_Deliverable_detail.Rows(0).Item("id_tipoDocumento")), 0) 'First Step

                Dim idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
                Dim Duracion As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")
                '*Actualizar correlativos

                clss_approval.Approval_CategoryCode_UPD(tbl_Deliverable_detail.Rows(0).Item("id_categoria"))

                '*****************************CREAMOS EL PRIMER REGISTRO DEL HISTORIAL PARA RUTA = 0
                'Addin the hours number lapsed required for each step
                Dim fecha_limit As DateTime = DateAdd(DateInterval.Day, Duracion, Date.UtcNow) 'UTC DATE
                Dim fecha_Recep As DateTime = Date.UtcNow 'UTC DATE

                clss_approval.set_ta_AppDocumento(0) 'New Record
                clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_Recep, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cOPEN, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripDeliverable, "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApproval_Deliverable.Rows(0).Item("id_rol"), "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

                id_appdocumento = clss_approval.save_ta_AppDocumento()

                If id_appdocumento <> -1 Then

                    '****************LOKSSS*************************************************
                    '****************LOKSSS*************************************************
                    '****************LOKSSS*************************************************
                    cls_Deliverable.SaveComment(id_appdocumento, cOPEN, strDescripDeliverable.Trim, Me.Session("E_IdUser"))
                    '****************LOKSSS************************************************
                    '****************LOKSSS************************************************
                    '****************LOKSSS************************************************

                    '*********************************************************************************************************************
                    '**********************************************FILES SHOULD BE SAVED HERE ********************************************
                    ''//**************************
                    ''//**************************
                    ''//**************************
                    '*********************************************************************************************************************

                    Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Documents_detail(Me.hd_id_deliverable.Value)
                    Dim idArch As Integer
                    For Each dtRow As DataRow In tbl_Suppor_docs.Rows

                        Dim lst_item As RadListBoxItem = New RadListBoxItem(dtRow("archivo"), dtRow("id_doc_soporte"))

                        clss_approval.set_ta_archivos_documento(0) 'New Record
                        clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_appdocumento, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("archivo", dtRow("archivo"), "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", dtRow("id_doc_soporte"), "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("ver", dtRow("ver"), "id_archivo", 0)
                        idArch = clss_approval.save_ta_archivos_documento()

                    Next


                    'For Each dRow In tbl_archivos_temp.Rows
                    '    If Not String.IsNullOrEmpty(dRow("archivo").ToString) Then

                    '        Dim idArch As Integer = 0

                    '        dmyhm = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                    '        extension = System.IO.Path.GetExtension(dRow("archivo"))
                    '        fileNameWE = System.IO.Path.GetFileNameWithoutExtension(dRow("archivo"))
                    '        strFile = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", id_documento, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)

                    '        clss_approval.set_ta_archivos_documento(0) 'New Record
                    '        clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_appdocumento, "id_archivo", 0)
                    '        clss_approval.set_ta_archivos_documentoFIELDS("archivo", strFile, "id_archivo", 0)
                    '        clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", dRow("id_doc_soporte"), "id_archivo", 0)
                    '        clss_approval.set_ta_archivos_documentoFIELDS("ver", 1, "id_archivo", 0)
                    '        idArch = clss_approval.save_ta_archivos_documento()

                    '        If idArch = -1 Then 'Erro Happenned
                    '            err = True
                    '            Exit For
                    '        Else
                    '            CopyFileParam(dRow("archivo"), strFile)
                    '        End If


                    '    End If
                    'Next

                    'If Not err() Then


                    '****************************************************************************************************************************************
                    '*****************************CREAMOS EL SEGUNDO REGISTRO DEL HISTORIAL PARA RUTA = 1****************************************************
                    '****************************************************************************************************************************************

                    tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Convert.ToInt32(tbl_Deliverable_detail.Rows(0).Item("id_tipoDocumento")), 1) 'Next Step
                    idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
                    Duracion = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")

                    Dim NextUser As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
                    Dim idNextRol As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")


                    fecha_Recep = Date.UtcNow 'UTC DATE
                    fecha_limit = calculaDiaHabil(Duracion, fecha_Recep)


                    '************************************************************************************
                    '*****************************GUARDAMOS LOS ARCHIVOS DE TEMP A LA CARPETA DESTINA Y GUARDAMOS EN LA BD 

                    clss_approval.set_ta_AppDocumento(0) 'New Record
                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", NextUser, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripDeliverable.Trim, "id_app_documento", 0) 'Pending Step 
                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role --NEW
                    clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

                    id_appdocumento2 = clss_approval.save_ta_AppDocumento()

                    If id_appdocumento2 <> -1 Then

                    Else
                        err = True
                    End If  'app_documento 2


                    'Else
                    '    err = True
                    'End If 'Archivos_documento 

                Else
                    err = True
                End If 'app_documento 1


            ElseIf id_documento <> -1 And tbl_Deliverable_detail.Rows(0).Item("id_deliverable_estado") = 5 Then 'StandBy status


                clss_approval.get_ta_DocumentosINFO(id_documento)

                Dim tbl_AppOrderO As New DataTable
                Dim tbl_rutas_tipo_doc As New DataTable

                strDescripDeliverable = If(Len(Me.txt_observation.Text.Trim) > 1, Me.txt_observation.Text.Trim, "--no comments--")

                '******Because comes from Stanby This has to change for Stand BY app*********************
                Dim idRuta = 0
                Dim idNextRol = 0
                Dim idNextUserID = 0
                Dim duracion = 0

                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MAX(id_documento) ' To get the Max ORder values to make the same step again

                If tbl_AppOrderO.Rows.Count > 0 Then

                    '*******************************************check this one************************************************
                    '****************Getting the values of the Max Order Again to return the approve**************************
                    idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                    idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                    idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'Return to de user that put the document in estand by status
                    '*****************************Getting the new values of the Max Order Again*******************************

                End If

                tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                    duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                End If

                Dim docState As Integer
                If idRuta = -1 Then ' there is not more steps
                    docState = cCOMPLETED
                Else
                    docState = cAPPROVED
                End If

                Dim tbl_AppOrden As New DataTable
                tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(id_documento) 'To get the info on the max step (id_app_doc)

                Dim id_App_Documento As Integer = tbl_AppOrden.Rows(0).Item("id_App_Documento")

                clss_approval.set_ta_AppDocumento(id_App_Documento)
                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripDeliverable, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?

                '**************************************************************************************************************************************
                '**************************************************************************************************************************************
                '**************************************************************************************************************************************

                Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Documents_detail(Me.hd_id_deliverable.Value)
                Dim tbl_App_docs As DataTable = clss_approval.get_Document(id_documento)
                Dim idArch As Integer
                Dim bndFound As Boolean = False

                For Each dtRow As DataRow In tbl_Suppor_docs.Rows

                    For Each dtRow2 As DataRow In tbl_App_docs.Rows

                        If dtRow("archivo") = dtRow2("archivo") Then
                            bndFound = True
                            Exit For
                        End If

                    Next

                    'Dim lst_item As RadListBoxItem = New RadListBoxItem(dtRow("archivo"), dtRow("id_doc_soporte"))

                    If Not bndFound Then

                        clss_approval.set_ta_archivos_documento(0) 'New Record
                        clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_App_Documento, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("archivo", dtRow("archivo"), "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", dtRow("id_doc_soporte"), "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("ver", dtRow("ver"), "id_archivo", 0)
                        idArch = clss_approval.save_ta_archivos_documento()

                    End If

                    bndFound = False

                Next

                '**************************************************************************************************************************************
                '**************************************************************************************************************************************
                '**************************************************************************************************************************************


                If clss_approval.save_ta_AppDocumento() <> -1 Then

                    cls_TimeSheet.SaveComment(id_App_Documento, cAPPROVED, strDescripDeliverable.Trim, Me.Session("E_IdUser"))

                    Dim fecha_recep As DateTime = Date.UtcNow
                    Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                    clss_approval.set_ta_AppDocumento(0) 'New Record
                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripDeliverable, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
                    'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role

                    id_appdocumento2 = clss_approval.save_ta_AppDocumento()

                    If id_appdocumento2 <> -1 Then
                    Else 'Error Saving
                    End If


                    Dim Deliverable_ As New ta_deliverable

                    Using db As New dbRMS_JIEntities

                        Deliverable_ = db.ta_deliverable.Find(Convert.ToInt32(hd_id_deliverable.Value))
                        Deliverable_.fecha_upd = Date.UtcNow
                        Deliverable_.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                        Deliverable_.id_deliverable_estado = 2 'In Approved Process

                        Dim result = cls_Deliverable.Save_deliverable(Deliverable_, Convert.ToInt32(Me.hd_id_deliverable.Value))

                        If result <> -1 Then

                            '**********************************************************************************
                            '*******************************TOOLS DELIVERABLE**********************************
                            '**********************************************************************************

                            '*********************************APPROVED****************************************
                            'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1009, cl_user.regionalizacionCulture, id_appdocumento2)
                            'If (objEmail.Emailing_DELIVERABLE_APPROVAL(id_appdocumento2)) Then
                            'Else 'Error mandando Email
                            'End If
                            '*********************************APPROVED****************************************

                            '**********************************************************************************
                            '*******************************TOOLS DELIVERABLE**********************************
                            '**********************************************************************************



                        End If

                    End Using

                Else 'Error saving the estep

                End If 'clss_approval.save_ta_AppDocumento()


            Else

                err = True
            End If 'documento



            If Not err Then

                Try

                    'Before to send  the Email set the Status
                    Using db As New dbRMS_JIEntities

                        Dim Deliverable_ As New ta_deliverable

                        Deliverable_ = db.ta_deliverable.Find(Convert.ToInt32(hd_id_deliverable.Value))
                        Deliverable_.fecha_upd = Date.UtcNow
                        Deliverable_.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))

                        If Deliverable_.id_deliverable_estado = 1 Then 'Created
                            Deliverable_.fecha_entrego = Date.UtcNow
                        End If
                        Deliverable_.id_deliverable_estado = 2 'In Approved Process

                        Dim result = cls_Deliverable.Save_deliverable(Deliverable_, Convert.ToInt32(Me.hd_id_deliverable.Value))

                        If result <> -1 Then

                            Dim Doc_deliverable As ta_documento_deliverable
                            Doc_deliverable = db.ta_documento_deliverable.Find(Convert.ToInt32(Me.hd_id_documento_deliverable.Value))
                            Doc_deliverable.id_documento = id_documento

                            result = cls_Deliverable.Save_documento_deliverable(Doc_deliverable, Convert.ToInt32(Me.hd_id_documento_deliverable.Value))

                            If result <> -1 Then

                                '**********************************************************************************
                                '*******************************TOOLS DELIVERABLE**********************************
                                '**********************************************************************************

                                '*********************************OPEN****************************************
                                'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 10, cl_user.regionalizacionCulture, id_appdocumento)
                                'If (objEmail.Emailing_TIME_SHEET_APPROVAL(id_appdocumento)) Then
                                'Else 'Error mandando Email
                                'End If

                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1009, cl_user.regionalizacionCulture, id_appdocumento2)
                                If (objEmail.Emailing_DELIVERABLE_APPROVAL(id_appdocumento2)) Then
                                Else 'Error mandando Email
                                End If

                                '*********************************OPEN****************************************

                                '**********************************************************************************
                                '*******************************TOOLS DELIVERABLE**********************************
                                '**********************************************************************************

                                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                                Me.MsgGuardar.Redireccion = String.Format("~/Deliverable/frm_DeliverableFollowing.aspx?ID={0}", Convert.ToInt32(Me.hd_id_deliverable.Value))
                                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                            Else

                                Me.lblError.Text = String.Format("An error was found updating the documents in this Deliverable: {0} ", result)

                            End If


                        Else

                            Me.lblError.Text = String.Format("An error was found updating the status: {0} ", result)

                        End If

                    End Using


                Catch ex As Exception

                    Me.lblError.Text = String.Format("An error was found sending the email: {0} ", ex.Message)

                End Try

            Else

                Me.lblError.Text = String.Format("An error ocurred during saving the the approval ")

            End If



        Catch ex As Exception

            Me.lblError.Text = String.Format("Error sending the deliverable approval, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
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



End Class
