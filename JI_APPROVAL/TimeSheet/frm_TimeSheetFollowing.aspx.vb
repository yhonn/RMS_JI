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



Partial Class frm_TimeSheetFollowing
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

        userName = timesheet.nombre_usuario
        userJOB_tittle = timesheet.job
        Month_TS = MonthName(timesheet.mes)
        hd_month.Value = timesheet.mes
        Year_TS = timesheet.anio
        hd_year.Value = timesheet.anio
        Status_TS = timesheet.timesheet_estado

        Me.hd_leave.Value = Convert.ToInt32(timesheet.ts_leave_update)

        Me.hd_IDuser.Value = timesheet.id_usuario
        Me.hd_id_employee_type.Value = timesheet.id_employee_type
        Me.lbl_employeeType.Text = timesheet.employee_type

        If IsDBNull(timesheet.fecha_upd) Then
            DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_upd, "m", timezoneUTC, False))
            HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_upd, timezoneUTC, True))
        Else
            DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(timesheet.fecha_creo, "m", timezoneUTC, False))
            HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(timesheet.fecha_creo, timezoneUTC, True))
        End If

        Me.PDescription.InnerHtml = timesheet.description.Trim

        If timesheet.id_timesheet_estado = 5 Then
            Me.txt_notes.EmptyMessage = timesheet.notes.Trim
        Else
            Me.txt_notes.Text = timesheet.notes.Trim
        End If


        Dim tbl_Approval As DataTable = cls_TimeSheet.get_TimeSheetApprovalUser(timesheet.id_usuario)
        Dim tbl_result As DataTable = cls_TimeSheet.TimeSheet_Document(hd_IDtimeSheet.Value)

        Me.cmb_approvals.DataSource = cls_TimeSheet.get_TimeSheetApprovalUser(timesheet.id_usuario)
        Me.cmb_approvals.DataTextField = "descripcion_aprobacion"
        Me.cmb_approvals.DataValueField = "id_tipoDocumento"
        Me.cmb_approvals.DataBind()

        If tbl_Approval.Rows.Count > 0 Then

            If tbl_result.Rows.Count > 0 Then

                Me.cmb_approvals.SelectedValue = tbl_result.Rows.Item(0).Item("id_tipoDocumento")
                Me.hd_id_documento_timesheet.Value = tbl_result.Rows.Item(0).Item("id_documento_TimeSheet")
                ' Me.hd_id_documento.Value = If(IsDBNull(tbl_result.Rows.Item(0).Item("id_documento")), 0, tbl_result.Rows.Item(0).Item("id_documento"))
                'tbl_result.Rows.Item(0).Item("id_documento")

            Else

                Me.cmb_approvals.SelectedValue = -1
                Me.hd_id_documento_timesheet.Value = 0

            End If


        Else

            Me.cmb_approvals.SelectedIndex = -1
            '  Me.cmb_approvals.Enabled = True
            Me.hd_id_documento_timesheet.Value = 0
            ' Me.hd_id_documento.Value = 0

        End If

        Me.cmb_approvals.Enabled = False


        Dim tbl_Suppor_docs As DataTable = cls_TimeSheet.TimeSheet_Support_Documents_detail(Convert.ToInt32(hd_IDtimeSheet.Value))

        Me.rpt_support_docs.DataSource = tbl_Suppor_docs
        Me.rpt_support_docs.DataBind()


        Me.anchorInformation.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorInformation.Attributes.Add("href", "frm_TimeSheetAdd.aspx?ID=" & hd_IDtimeSheet.Value)

        Me.anchorBillable.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorBillable.Attributes.Add("href", "frm_TimeSheetEdit.aspx?ID=" & hd_IDtimeSheet.Value)

        Me.anchorSupportDocs.Attributes.Add("class", "btn btn-success btn-circle")
        Me.anchorSupportDocs.Attributes.Add("href", "frm_TimeSheetDocs.aspx?ID=" & hd_IDtimeSheet.Value)

        Me.anchorFollowUp.Attributes.Add("class", "btn btn-primary btn-circle")

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
        lbl_ALL_SIMPLE_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
        '*********************** All Roles*******************************************************


        'If timesheet.id_timesheet_estado = 1 Then
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-default btn-circle disabled")
        'Else
        '    Me.anchorFollowUp.Attributes.Add("class", "btn btn-success btn-circle")
        '    Me.anchorFollowUp.Attributes.Add("href", "frm_TimeSheetFollowing.aspx?ID=" & hd_IDtimeSheet.Value)
        'End If

        If ((timesheet.id_timesheet_estado > 1 And timesheet.id_timesheet_estado < 5) Or (timesheet.id_timesheet_estado = 6)) Then 'In Approval Process/Approved/Rejected

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


            Me.lyButtoms.Visible = False
            Me.lyHistory.Visible = True

        Else 'Created /  Observations Pending

            Me.lyButtoms.Visible = True
            Me.lyHistory.Visible = False

            If timesheet.id_timesheet_estado = 5 Then
                Me.btnlk_save.Text = "Apply Observation"
            Else
                Me.btnlk_save.Text = "Start Approval"
            End If

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
        strTableResult = cls_TimeSheet.get_TimeSheetTable(timesheet.anio, timesheet.mes, hd_IDtimeSheet.Value, hd_id_employee_type.Value, Me.hd_leave.Value) 'The Original Template

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

    Protected Sub btnlk_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnlk_save.Click

        Try

            Dim err As Boolean = False
            Dim id_documento As Integer = 0
            Dim id_appdocumento As Integer = 0
            Dim id_appdocumento2 As Integer = 0

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

            Dim VWtimesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(hd_IDtimeSheet.Value)

            Dim tblUserApprovalTimeSheet As DataTable = cls_TimeSheet.get_TimeSheetApprovalUser(Me.hd_IDuser.Value)

            Dim strCode As String = generate_CODE(tblUserApprovalTimeSheet.Rows(0).Item("id_categoria"))
            Dim AppCode As String = cls_TimeSheet.CrearCodigo(tblUserApprovalTimeSheet.Rows(0).Item("id_categoria"))

            Dim strDescripName As String = String.Format("{0} - ({1})", Me.PDescription.InnerText.Trim, VWtimesheet.nombre_usuario)

            Dim FoundPos = InStr(1, Me.txt_notes.Text.Trim, "****Modificación", CompareMethod.Text)
            If FoundPos > 0 Then
                strDescripName = "Modificación " & strDescripName.Trim
            End If

            Dim strDescripTimeSheet As String = String.Format("{0} -- {1}", Me.PDescription.InnerText.Trim, Me.txt_notes.Text.Trim)

            If VWtimesheet.id_timesheet_estado = 1 Then 'Create the New record

                clss_approval.set_ta_documento(0) 'Set new Record
                clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", Me.cmb_approvals.SelectedValue, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("numero_instrumento", strCode, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("descripcion_doc", strDescripName, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("nom_beneficiario", VWtimesheet.nombre_usuario, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("comentarios", strDescripTimeSheet, "id_documento", 0) '.Replace("'", "''")
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

            Else

                Using db As New dbRMS_JIEntities

                    id_documento = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = hd_IDtimeSheet.Value).FirstOrDefault().id_documento

                End Using

            End If



            If id_documento <> -1 And VWtimesheet.id_timesheet_estado = 1 Then 'Save New ones Docs

                Dim tbl_Route_By_DOC As New DataTable
                tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Me.cmb_approvals.SelectedValue, 0) 'First Step

                Dim idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
                Dim Duracion As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")
                '*Actualizar correlativos

                clss_approval.Approval_CategoryCode_UPD(tblUserApprovalTimeSheet.Rows(0).Item("id_categoria"))

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
                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripTimeSheet, "id_app_documento", 0) '.Replace("'", "''")
                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApprovalTimeSheet.Rows(0).Item("id_rol"), "id_app_documento", 0)
                clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

                id_appdocumento = clss_approval.save_ta_AppDocumento()

                If id_appdocumento <> -1 Then

                    '****************LOKSSS*************************************************
                    '****************LOKSSS*************************************************
                    '****************LOKSSS*************************************************
                    cls_TimeSheet.SaveComment(id_appdocumento, cAPPROVED, strDescripTimeSheet.Trim, Me.Session("E_IdUser"))
                    '****************LOKSSS************************************************
                    '****************LOKSSS************************************************
                    '****************LOKSSS************************************************

                    '********************************************************************************************************************
                    '*****************************GUARDAMOS LOS ARCHIVOS DE TEMP A LA CARPETA DESTINA Y GUARDAMOS EN LA BD **************

                    '*********************************************************************************************************************
                    '**********************************************FILES SHOULD BE SAVED HERE ********************************************
                    ''//**************************
                    ''//**************************
                    ''//**************************
                    '*********************************************************************************************************************

                    Dim tbl_Suppor_docs As DataTable = cls_TimeSheet.TimeSheet_Support_Documents_detail(Me.hd_IDtimeSheet.Value)
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



                    'If Not err() Then


                    '*****************************CREAMOS EL SEGUNDO REGISTRO DEL HISTORIAL PARA RUTA = 1

                    tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Me.cmb_approvals.SelectedValue, 1) 'Next Step
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
                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripTimeSheet.Trim, "id_app_documento", 0) 'Pending Step 
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

            ElseIf id_documento <> -1 And VWtimesheet.id_timesheet_estado = 5 Then 'StandBy status

                '******************************HEre the change************************
                strDescripTimeSheet = String.Format("{0}", Me.txt_notes.Text.Trim)

                clss_approval.get_ta_DocumentosINFO(id_documento)

                Dim tbl_AppOrderO As New DataTable
                Dim tbl_rutas_tipo_doc As New DataTable

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
                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripTimeSheet, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?

                If clss_approval.save_ta_AppDocumento() <> -1 Then

                    cls_TimeSheet.SaveComment(id_App_Documento, cAPPROVED, strDescripTimeSheet.Trim, Me.Session("E_IdUser"))

                    Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                    Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                    clss_approval.set_ta_AppDocumento(0) 'New Record
                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strDescripTimeSheet, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
                    'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role

                    Dim id_appdocumento_2 = clss_approval.save_ta_AppDocumento()

                    If id_appdocumento_2 <> -1 Then
                    Else 'Error Saving
                    End If



                    Dim TimeSheet As New ta_timesheet

                    Using db As New dbRMS_JIEntities

                        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(hd_IDtimeSheet.Value))
                        TimeSheet.fecha_upd = Date.UtcNow
                        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                        TimeSheet.id_timesheet_estado = 2 'In Approved Process

                        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(hd_IDtimeSheet.Value))

                        If result <> -1 Then

                            '*******************************TOOLS TIME SHEET**********************************

                            '*********************************OPEN****************************************
                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 10, cl_user.regionalizacionCulture, id_appdocumento_2)
                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(id_appdocumento_2)) Then
                            Else 'Error mandando Email
                            End If
                            '*********************************OPEN****************************************

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

                        Dim TimeSheet As New ta_timesheet

                        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(Me.hd_IDtimeSheet.Value))

                        TimeSheet.fecha_upd = Date.UtcNow
                        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                        TimeSheet.id_timesheet_estado = 2 'Sending to Approval

                        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(Me.hd_IDtimeSheet.Value))

                        If result <> -1 Then

                            '************************************************************************************************************
                            '**************************this part should be added in the in the Documents**********************************************
                            '************************************************************************************************************

                            Dim ResultTimeSheets = db.ta_documento_timesheets.Where(Function(p) p.id_timesheet = result)
                            Dim DocTs As ta_documento_timesheets

                            If ResultTimeSheets.count() = 0 Then

                                DocTs = New ta_documento_timesheets

                                DocTs.id_timesheet = Me.hd_IDtimeSheet.Value
                                DocTs.id_TipoDocumento = Me.cmb_approvals.SelectedValue
                                DocTs.id_documento = id_documento
                                result = cls_TimeSheet.SaveDocumento_TimeSheet(DocTs, Convert.ToInt32(Me.hd_IDtimeSheet.Value))

                            Else

                                DocTs = db.ta_documento_timesheets.Find(ResultTimeSheets.FirstOrDefault.id_documento_timesheet)
                                DocTs.id_documento = id_documento
                                result = cls_TimeSheet.SaveDocumento_TimeSheet(DocTs, ResultTimeSheets.FirstOrDefault.id_documento_timesheet)

                            End If

                            '************************************************************************************************************
                            '**************************this part should be added in the in the Documents**********************************************
                            '************************************************************************************************************

                            If result <> -1 Then

                                If id_appdocumento > 0 Then

                                    '*********************************OPEN****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 10, cl_user.regionalizacionCulture, id_appdocumento)
                                    If (objEmail.Emailing_TIME_SHEET_APPROVAL(id_appdocumento)) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************OPEN****************************************
                                End If

                                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                                Me.MsgGuardar.Redireccion = String.Format("~/TimeSheet/frm_TimeSheetFollowing.aspx?ID={0}", hd_IDtimeSheet.Value)
                                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                            Else

                                Me.lblError.Text = String.Format("An error was found updating the documents in this timesheet: {0} ", result)

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

            Me.lblError.Text = String.Format("Error sending the time sheet approval, please contact to the system administrator <br /><br /> Detail:{0}", ex.Message)
            Me.lblError.Visible = True

        End Try


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

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/TimeSheet/frm_TimeSheet.aspx")
    End Sub


    'Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound


    '    If TypeOf e.Item Is GridGroupHeaderItem Then
    '        Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
    '        Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        'item.DataCell.Text = "$" + groupDataRow("UnitPrice").ToString() + " (" + groupDataRow("InStock").ToString() + ")"
    '        item.DataCell.Text = item.DataCell.Text.Split(":")(1).ToString
    '    End If

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

    '        '*************DISABLED AND BACKGROUND COLOR*************
    '        Dim DateValue As Date
    '        Dim nameCol As String
    '        Dim txtName As String
    '        Dim DaysInMonth As Integer = Date.DaysInMonth(hd_year.Value, hd_month.Value)
    '        Dim txtRadNumeric As RadNumericTextBox

    '        For i = 1 To DaysInMonth 'Marking sunday

    '            DateValue = New Date(hd_year.Value, hd_month.Value, i)
    '            'tblRow.Item(i + 3) = DateValue.ToString("ddd")
    '            nameCol = String.Format("colm_d{0}", i)
    '            txtName = String.Format("txt_d{0}", i)

    '            If Weekday(DateValue) = 1 Then
    '                Me.grd_cate.MasterTableView.GetColumn(nameCol).ItemStyle.BackColor = Drawing.Color.LightGray
    '                txtRadNumeric = CType(itemD(nameCol).FindControl(txtName), RadNumericTextBox)
    '                'txtRadNumeric.BackColor = Drawing.Color.Gray
    '                If Not IsNothing(txtRadNumeric) Then
    '                    txtRadNumeric.BackColor = Drawing.Color.LightGray
    '                End If


    '            End If

    '        Next

    '        'For i = DaysInMonth + 1 To 3
    '        '    nameCol = String.Format("colm_d{0}", i)
    '        '    Me.grd_cate.MasterTableView.GetColumn(nameCol).Display = False
    '        'Next



    '    End If



    ' End Sub




    '<Web.Services.WebMethod()>
    'Public Shared Function GetBillableOptions(ByVal idBillType As Integer, ByVal idPrograma As Integer, ByVal idTimeSheet As Integer) As Object

    '    Dim jsonITEMS As String = "[]"

    '    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(idPrograma)
    '    Dim ResultItems As Object = cls_TimeSheet.get_ta_billable_Option(idBillType, idTimeSheet)

    '    Dim serializer As New JavaScriptSerializer()
    '    If ResultItems.Count() > 0 Then
    '        jsonITEMS = serializer.Serialize(ResultItems)
    '    End If

    '    Return jsonITEMS

    'End Function


    'Private Sub bntlk_addItem_Click(sender As Object, e As EventArgs) Handles bntlk_addItem.Click

    '    'Me.MsgReturn.Redireccion = ""
    '    'Me.MsgReturn.TituMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "TITTLE_CONFIRM").texto
    '    'Me.MsgReturn.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "MSG_CONFIRM").texto
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)

    '    Dim vFoundIT As Boolean = False
    '    For Each Irow As GridDataItem In Me.grd_cate.Items

    '        If TypeOf Irow Is GridDataItem Then

    '            Dim idBillableTime = Convert.ToInt32(Irow("id_billable_time").Text)

    '            If cmb_billable_Item.SelectedValue = idBillableTime Then
    '                vFoundIT = True
    '                Exit For
    '            End If

    '        End If

    '    Next


    '    If Not vFoundIT Then
    '        loadData()
    '        loadGRID(2)
    '    Else 'Its already exist


    '    End If


    'End Sub

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

    Private Sub frm_TimeSheetFollowing_AbortTransaction(sender As Object, e As EventArgs) Handles Me.AbortTransaction

    End Sub
End Class
