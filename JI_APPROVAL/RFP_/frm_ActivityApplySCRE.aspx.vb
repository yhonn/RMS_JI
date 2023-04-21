Imports ly_SIME
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports System.Threading
Imports ly_RMS

Public Class frm_ActivityApplySCRE
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACT_APPLY_SCRE"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim valorSuma As Decimal = 0
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim dateUtil As ly_APPROVAL.APPROVAL.cls_dUtil
    Dim timezoneUTC As Integer

    Public Property document_folder As String = ""
    Public Property solicitation_document_folder As String = ""
    Dim dtApplicants As New DataTable

    Dim dtDocuments As New DataTable
    Dim IDActivity_Solicitation As Guid
    Dim IDSolicitation_AP As Guid
    Dim IDSolicitation_AP_USER_TK As Guid


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Try
        '    If Me.Session("E_IdUser").ToString = "" Then
        '    End If
        'Catch ex As Exception
        '    Me.Response.Redirect("~/frm_login")
        'End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next

            Me.btn_eliminarDocumento.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Session.Item("dateUtil") Is Nothing Then
            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(Convert.ToInt32(Me.Session("E_IDPrograma")), True)
            Dim userCulture As CultureInfo = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", Convert.ToInt32(Me.Session("E_IDPrograma"))))
            dateUtil = New ly_APPROVAL.APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************
            Session.Item("dateUtil") = dateUtil

        Else
            dateUtil = Session.Item("dateUtil")
        End If

        'Me.Editor_approve_comments.CssFiles.Add("~/App_Themes/Default/CustomStyles.css")
        Dim imagepath() As String = {"~/FileUploads/Documents/Submission/ApproveSub"}

        Me.Editor_approve_comments.ImageManager.MaxUploadFileSize = 5243000 '5MB
        Me.Editor_approve_comments.ImageManager.UploadPaths = imagepath
        Me.Editor_approve_comments.ImageManager.ViewPaths = imagepath
        Me.Editor_approve_comments.ImageManager.DeletePaths = imagepath

        Me.Editor_approve_comments.DocumentManager.MaxUploadFileSize = 10490000 '10MB
        Me.Editor_approve_comments.DocumentManager.UploadPaths = imagepath

        Dim pattern() As String = {"*.doc", "*.txt", "*.docx", "*.xls", "*.xlsx", "*.pdf", "*.jpg", "*.jpeg", "*.eps", "*.png", "*.ppt", "*.pptx"}

        Me.Editor_approve_comments.DocumentManager.SearchPatterns = pattern
        Me.Editor_approve_comments.DocumentManager.ViewPaths = imagepath
        Me.Editor_approve_comments.DocumentManager.DeletePaths = imagepath


        If Not Me.IsPostBack Then


            Using dbEntities As New dbRMS_JIEntities

                ' Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                IDActivity_Solicitation = Guid.Parse(Me.Request.QueryString("ab").ToString())
                IDSolicitation_AP = Guid.Parse(Me.Request.QueryString("ac").ToString())
                IDSolicitation_AP_USER_TK = Guid.Parse(Me.Request.QueryString("ad").ToString())

                Dim oUserToken = dbEntities.VW_T_TOKEN_USERS.Where(Function(p) p.tk_Token = IDSolicitation_AP_USER_TK).FirstOrDefault()
                Dim id_usuario = 2075 'Potential grantee user
                'oUserToken.id_usr
                Dim id_programa = oUserToken.id_programa

                Check_SOLICITATION_Status(IDActivity_Solicitation) 'For closing properly

                Dim oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.SOLICITATION_TOKEN = IDActivity_Solicitation).FirstOrDefault()
                Dim id_activity_solicitation = oSolicitation.ID_ACTIVITY_SOLICITATION
                Dim id_activity = oSolicitation.ID_ACTIVITY
                Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.SOLICITATION_TOKEN = IDSolicitation_AP).FirstOrDefault()
                Dim id_solicitation_app = oSolicitationAPP.ID_SOLICITATION_APP
                Me.lbl_id_sol_app.Text = id_solicitation_app
                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id_activity)
                ''Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)
                Dim strWELLCOME As String = Me.lblt_titulo_pantalla.Text.Trim
                Me.lblt_titulo_pantalla.Text = String.Format("{2} {0} ({1})", oSolicitationAPP.ORGANIZATIONNAME, oSolicitationAPP.NAMEALIAS, strWELLCOME)

                Me.lbl_id_ficha.Text = id_activity
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app)
                ''***********************************************************************************************************************************************
                ''***************************************************SCREENING APPLICATION***********************************************************************
                If oTA_APPLY_SCREENING.Count > 0 Then
                    ' Me.Response.Redirect("~/RFP_/frm_ActivityApplySCRE?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString)


                    If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 1 Then 'Pending
                        Me.grd_screening.DataSourceID = ""
                        Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = oTA_APPLY_SCREENING.FirstOrDefault.ID_MEASUREMENT_SURVEY).OrderBy(Function(o) o.order_numberQU).ToList()
                        Me.grd_screening.DataBind()

                        Me.questions_prescreening.Attributes.Add("style", "display:block;")
                        Me.app_prescreening.Attributes.Add("style", "display:none;")

                        Me.btnlk_sent_screening.Attributes.Add("class", "btn btn-warning btn-sm")

                    Else 'Show Result history of Screening

                        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                        Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                        Me.rept_PrescreeningDates.DataBind()

                        Me.questions_prescreening.Attributes.Add("style", "display:none;")
                        Me.app_prescreening.Attributes.Add("style", "display:block;")
                        Me.btnlk_sent_screening.Attributes.Add("class", "btn btn-warning btn-sm disabled")

                    End If

                End If

                ''******************************************************REDIRECT TO SCREENING APPLICATION**********************************************************
                ''***********************************************************************************************************************************************


                Dim bndContinue As Boolean = True

                If oTA_APPLY_SCREENING.Count > 0 Then
                    If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 3 Then 'Accepted & Passed
                        bndContinue = True
                    Else
                        Me.tab_index_order.Value = 0
                        Me.tab_index_max.Value = 1
                        bndContinue = False
                    End If
                End If


                If bndContinue Then


                    Dim listado = dbEntities.VW_TA_SOLICITATION_MATERIALS.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_activity_solicitation) _
                      .Select(Function(p) _
                                  New With {Key .nombre_documento = p.nombre_documento,
                                            Key .id_doc_soporte = p.ID_DOC_SOPORTE}).OrderBy(Function(p) p.nombre_documento).ToList()

                    Me.cmb_type_of_document.DataSourceID = ""
                    Me.cmb_type_of_document.DataSource = listado  'cl_listados.get_ta_docs_soporte(id_programa)
                    Me.cmb_type_of_document.DataTextField = "nombre_documento"
                    Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
                    Me.cmb_type_of_document.DataBind()

                    Me.cmb_solicitation_type.DataSourceID = ""
                    Me.cmb_solicitation_type.DataSource = cl_listados.get_TA_SOLICITATION_TYPE(id_programa)
                    Me.cmb_solicitation_type.DataTextField = "SOLICITATION_ACRONY"
                    Me.cmb_solicitation_type.DataValueField = "ID_SOLICITATION_TYPE"
                    Me.cmb_solicitation_type.DataBind()

                    Me.cmb_solicitation_status.DataSourceID = ""
                    Me.cmb_solicitation_status.DataSource = cl_listados.get_TA_SOLICITATION_STATUS(id_programa)
                    Me.cmb_solicitation_status.DataTextField = "SOLICITATION_STATUS"
                    Me.cmb_solicitation_status.DataValueField = "ID_SOLICITATION_STATUS"
                    Me.cmb_solicitation_status.DataBind()

                End If

                Me.txt_activity_code.Text = proyecto.codigo_ficha_AID

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************


                If Not IsNothing(oSolicitation) Then

                    Me.lbl_id_sol.Text = oSolicitation.ID_ACTIVITY_SOLICITATION

                    If bndContinue Then

                        LoadSolicitation(oSolicitation)

                        solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).submission_documents_path
                        document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).solicitation_documents_path

                        Me.grd_support_Documents.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
                        Me.grd_support_Documents.DataBind()

                        Select_Applicant(id_solicitation_app)

                    End If

                Else

                    Me.lbl_id_sol.Text = "0"

                End If


            End Using


        Else

            dtApplicants = Session("dtApplicants")
            dateUtil = Session.Item("dateUtil")
            IDActivity_Solicitation = Guid.Parse(Me.Request.QueryString("ab").ToString())
            IDSolicitation_AP = Guid.Parse(Me.Request.QueryString("ac").ToString())
            IDSolicitation_AP_USER_TK = Guid.Parse(Me.Request.QueryString("ad").ToString())


        End If


    End Sub



    Public Sub Check_SOLICITATION_Status(ByVal id_activity_sol As Guid)


        Dim date_Now As Date = Date.UtcNow
        Dim SolicitationSTATUS As Integer = 1 'CReated


        Using dbEntities As New dbRMS_JIEntities

            Dim id_activiy_solicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.SOLICITATION_TOKEN = id_activity_sol).FirstOrDefault.ID_ACTIVITY_SOLICITATION
            Dim oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_activiy_solicitation)

            If date_Now >= oSolicitation.start_date And date_Now <= oSolicitation.end_date Then
                SolicitationSTATUS = 2 'OPENED
            ElseIf date_Now > oSolicitation.end_date Then
                SolicitationSTATUS = 3 'CLOSED
            Else
                SolicitationSTATUS = 1  'CREATED
            End If

            If oSolicitation.ID_SOLICITATION_STATUS <> SolicitationSTATUS Then

                oSolicitation.ID_SOLICITATION_STATUS = SolicitationSTATUS
                oSolicitation.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oSolicitation.fecha_upd = Date.UtcNow
                oSolicitation.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.Entry(oSolicitation).State = Entity.EntityState.Modified

                dbEntities.SaveChanges()

            End If
            
            If oSolicitation.ID_SOLICITATION_STATUS = 3 Then 'if is closed

                Me.btn_save_app.Attributes.Add("class", "btn btn-warning btn-lg disabled")
                Me.btnlk_Apply.Attributes.Add("class", "btn btn-success btn-sm disabled")
                Me.btnlk_Apply2.Attributes.Add("class", "btn btn-success btn-sm disabled")
                Me.btnlk_comment.Attributes.Add("class", "btn btn-default  btn-sm ")
                Me.SolicitationExpired.Attributes.Add("style", "display:block;")

            End If

        End Using

    End Sub

    Function get_Activity_Applicants(ByVal id_solicitation As Integer) As DataTable

        Using dbEntities As New dbRMS_JIEntities

            Dim oSolicitation = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation) _
                .Select(Function(p) New With {Key .ID_ORGANIZATION_APP = p.ID_ORGANIZATION_APP,
                                                  .ID_SOLICITATION_APP = p.ID_SOLICITATION_APP,
                                                   .ID_ACTIVITY_SOLICITATION = p.ID_ACTIVITY_SOLICITATION,
                                                   .SOLICITATION_APP_CODE = p.SOLICITATION_APP_CODE,
                                                   .NAMEALIAS = p.NAMEALIAS,
                                                   .ORGANIZATIONNAME = p.ORGANIZATIONNAME,
                                                   .PERSONNAME = p.PERSONNAME,
                                                   .ORGANIZATION_TYPE = p.ORGANIZATION_TYPE,
                                                    .ADDRESSCOUNTRYREGIONID = p.ADDRESSCOUNTRYREGIONID,
                                                    .ADDRESSCITY = p.ADDRESSCITY,
                                                    .ORGANIZATIONSTATUS = p.ORGANIZATIONSTATUS,
                                                    .ORGANIZATIONEMAIL = p.ORGANIZATIONEMAIL,
                                                    .APLICATION_STATUS = p.APLICATION_STATUS,
                                                    .ID_APP_STATUS = p.ID_APP_STATUS,
                                                    .SENT_DATE = p.SENT_DATE,
                                                    .RECEIVED_DATE = p.RECEIVED_DATE,
                                                    .SUBMITTED_DATE = p.SUBMITTED_DATE}).ToList()
            Dim cl_utl As New CORE.cls_util

            Return cl_utl.ConvertToDataTable(oSolicitation)

        End Using

    End Function



    Private Function GET_HOST_DAT(ByVal idType As String) As String

        Dim vRequest As HttpRequest = HttpContext.Current.Request
        Dim strResult As String

        Select Case idType

            Case "HOST"
                strResult = vRequest.ServerVariables("REMOTE_HOST")
            Case "AGENT"
                strResult = vRequest.ServerVariables("HTTP_USER_AGENT")
            Case "LOCAL"
                strResult = vRequest.ServerVariables("LOCAL_ADDR")

        End Select

        GET_HOST_DAT = strResult

    End Function

    Protected Sub LoadSolicitation(ByVal oSolicitation As TA_ACTIVITY_SOLICITATION)


        cmb_solicitation_type.SelectedValue = oSolicitation.ID_SOLICITATION_TYPE
        cmb_solicitation_status.SelectedValue = oSolicitation.ID_SOLICITATION_STATUS
        Me.lbl_COde.Text = oSolicitation.SOLICITATION_CODE
        'Me.txt_solicitation.Text = oSolicitation.SOLICITATION
        Me.txt_tittle.Text = oSolicitation.SOLICITATION_TITLE
        Me.txt_purpose.Text = oSolicitation.SOLICITATION_PURPOSE
        '' oSolicitation.SOLICITATION_TOKEN

        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(Convert.ToInt32(Me.Session("E_IDPrograma")), True)
        Dim userCulture As CultureInfo = cl_user.regionalizacionCulture
        Dim timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", Convert.ToInt32(Me.Session("E_IDPrograma"))))
        Dim dateUtil As ly_APPROVAL.APPROVAL.cls_dUtil = New ly_APPROVAL.APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        lbl_fecha_inicio.Text = dateUtil.set_DateFormat(oSolicitation.start_date, "f", timezoneUTC, True)
        lbl_fecha_final.Text = dateUtil.set_DateFormat(oSolicitation.end_date, "f", timezoneUTC, True)
        'dt_fecha_inicio.SelectedDate = oSolicitation.start_date
        'Dim oFechaFin As DateTime = oSolicitation.end_date
        'dt_fecha_fin.SelectedDate = oFechaFin
        'Me.txt_hour.Value = DatePart(DateInterval.Hour, oFechaFin)
        'Me.txt_min.Value = DatePart(DateInterval.Minute, oFechaFin)

        If oSolicitation.ID_SOLICITATION_STATUS = 3 Then 'if is closed

            Me.btn_save_app.Attributes.Add("class", "btn btn-warning btn-lg disabled")
            Me.btnlk_Apply.Attributes.Add("class", "btn btn-success btn-sm disabled")
            Me.btnlk_Apply2.Attributes.Add("class", "btn btn-success btn-sm disabled")
            Me.btnlk_comment.Attributes.Add("class", "btn btn-default  btn-sm ")
            Me.SolicitationExpired.Attributes.Add("style", "display:block;")

        End If




    End Sub


    'Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
    '    Dim a = CType(sender, LinkButton)
    '    Me.identity.Text = a.Attributes.Item("data-identity").ToString()
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    'End Sub


    Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_ACTIVITY_ANNEX").ToString()
            Dim Id_solicitation_app = Me.lbl_id_sol_app.Text

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_ACTIVITY_DOCUMENTS WHERE (ID_ACTIVITY_ANNEX = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
            Me.grd_archivos.DataBind()

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)
    End Sub
    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).submission_documents_path
                'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path


                Dim btn_delete As ImageButton = CType(itemD("Eliminar").Controls(0), ImageButton)
                ' btn_delete.Visible = False


                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = solicitation_document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"

                Dim aprobar As New HyperLink
                aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

                If DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_ANNEX").ToString() = "0" Then
                    ImageDownload.Visible = False
                    btn_delete.Visible = False


                    If DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString() = "Optional" Then
                        aprobar.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                        aprobar.ToolTip = "Pending To upload"
                    Else
                        aprobar.ImageUrl = "~/Imagenes/iconos/exclamation.gif"
                        aprobar.ToolTip = "Pending To upload"
                    End If

                Else
                    aprobar.ImageUrl = "~/Imagenes/iconos/accept.png"
                    aprobar.ToolTip = "Uploaded"
                End If

                Dim hlk_ref As New HyperLink
                hlk_ref = itemD("colm_template").FindControl("hlk_template")

                If Not DataBinder.Eval(e.Item.DataItem, "Template").ToString().Contains("--none--") Then
                    hlk_ref.Text = DataBinder.Eval(e.Item.DataItem, "Template").ToString()
                    hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
                Else
                    hlk_ref.Text = itemD("Template").Text
                    hlk_ref.NavigateUrl = "#"
                End If

                Dim txtCtrl As New RadTextBox
                txtCtrl = CType(e.Item.FindControl("txtMandatory"), RadTextBox)
                txtCtrl.ReadOnly = True
                txtCtrl.Text = DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString()

                'If DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString() = "True" Then
                '    txtCtrl.Text = "Mandatory"
                'Else
                '    txtCtrl.Text = "Optional"
                'End If



            End Using
        End If

    End Sub

    Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
        'Dim Path As String
        'Path = Server.MapPath("~/FileUploads/")
        'e.File.SaveAs(Path + getNewName(e.File))
    End Sub

    'Protected Sub btn_agregar_Click(sender As Object, e As EventArgs) Handles btn_agregar.Click

    '    Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '    'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_solicitation = Val(Me.lbl_id_sol.Text)
    '    Dim id_solicitation_app = Val(Me.lbl_id_sol_app.Text)

    '    Using dbEntities As New dbRMS_JIEntities

    '        For Each file As UploadedFile In AsyncUpload1.UploadedFiles

    '            Dim exten = file.GetExtension()
    '            Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
    '            Dim anexo As New TA_ACTIVITY_DOCUMENTS
    '            anexo.DOCUMENT_TITLE = Me.txt_document_tittle.Text
    '            anexo.DOCUMENT_NAME = nombreArchivo
    '            anexo.DOCUMENTROLE = "APPLY_ANNEX"
    '            anexo.id_doc_soporte = cmb_type_of_document.SelectedValue
    '            anexo.ID_ACTIVITY = id_activity
    '            anexo.ID_ACTIVITY_SOLICITATION = id_solicitation
    '            anexo.ID_SOLICITATION_APP = id_solicitation_app
    '            anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            anexo.fecha_crea = Date.UtcNow
    '            anexo.visible = True
    '            anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '            dbEntities.TA_ACTIVITY_DOCUMENTS.Add(anexo)

    '            Dim Path As String
    '            Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).submission_documents_path)
    '            file.SaveAs(Path + nombreArchivo)

    '        Next
    '        dbEntities.SaveChanges()

    '        Me.tab_index_order.Value = 2
    '        Me.tab_index_max.Value = 3

    '        Me.MsgGuardar.NuevoMensaje = "Document uploaded Successfully"
    '        'cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString & "&_tab=Applicants"
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '        'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
    '        'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


    '    End Using

    '    'Me.grd_archivos.DataBind()

    'End Sub

    Function getNewName(ByVal file As UploadedFile) As String
        Dim rand As New Random()
        Dim Aleatorio As Double = rand.Next(1, 99999)
        Dim extension As String = System.IO.Path.GetExtension(file.GetExtension())
        Dim newName As String = "doc_" & Me.Session("E_IdUser") & Date.UtcNow.ToShortDateString().Replace("/", "-") & Aleatorio & file.GetNameWithoutExtension().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
        Return newName
    End Function

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/RFP_/frm_ActivityE?Id=" & Me.lbl_id_ficha.Text)
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_Proyectos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub


    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub

    Private Sub cmb_solicitation_type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_solicitation_type.SelectedIndexChanged


        If Val(Me.cmb_solicitation_type.SelectedValue) > 0 Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Me.lbl_COde.Text = cl_listados.CrearCodigoRFA(idPrograma, Me.cmb_solicitation_type.SelectedValue, 0)
            Me.lbl_COde.Visible = True

        End If

    End Sub




    Protected Sub Select_Applicant(ByVal idSolAPP As Integer)

        Dim Id_solicitation_app = idSolAPP
        Me.lbl_id_sol_app.Text = Id_solicitation_app

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        Try

            Using dbEntities As New dbRMS_JIEntities


                Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
                Dim oActivityAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = OSolicitationAPP.ID_SOLICITATION_APP).ToList()

                '*******************************************************************************************************************************
                '*******************************************************MADE FROM ORGANIZATION INTERFACE**************************************
                '*******************************************************************************************************************************
                Dim dateIni As DateTime
                Dim hrD As Long = 25
                Dim dtEMAIL As Object

                If OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT

                    OSolicitationAPP.ID_APP_STATUS = 3 'Open it
                    OSolicitationAPP.RECEIVED_DATE = Date.UtcNow
                    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        'Dim oApplyComm As New TA_APPLY_CO MM
                        'oApplyComm.ID_APPLY_APP = oApply.FirstOrDefault.ID_APPLY_APP
                        'oApplyComm.ID_APPLY_STATUS = oApply.FirstOrDefault.ID_APPLY_STATUS
                        'oApplyComm.APPLY_COMM = "Solicitation Opened"
                        'oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        'oApplyComm.FECHA_CREA = Date.UtcNow
                        'oApplyComm.COMM_BOL = 0

                        'dbEntities.TA_APPLY_COMM.Add(oApplyComm)

                        'If dbEntities.SaveChanges() Then

                        dtEMAIL = dbEntities.t_sended.Where(Function(p) p.EMAILROLE = "SOLICITATION_EMAIL" And p.EMAILACTION = "SOLICITATION OPENED" And p.ID_SOURCE = Id_solicitation_app And p.sn_sended = True).OrderByDescending(Function(o) o.idSend).FirstOrDefault()

                        'If Not IsNothing(dtEMAIL) Then

                        '    dateIni = If(IsNothing(dtEMAIL), Date.UtcNow, dtEMAIL.sn_datesending)
                        '    hrD = DateDiff(DateInterval.Hour, dateIni, Date.UtcNow)

                        'End If

                        If IsNothing(dtEMAIL) Then

                            '**********************************CHEMONICS PROCESSS*******************************************************
                            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1011, regionalizacionCulture)
                            '**********************************CHEMONCS PROCESSS*******************************************************
                            cl_Noti_Process.NOTIFIYING_SOLICITATION_OP(Id_solicitation_app)

                        End If

                        'If hrD > 24 Then

                        '        '**********************************CHEMONICS PROCESSS*******************************************************
                        '        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        '        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1011, regionalizacionCulture)
                        '        '**********************************CHEMONCS PROCESSS*******************************************************
                        '        cl_Noti_Process.NOTIFIYING_SOLICITATION_OP(Id_solicitation_app)

                        '    End If


                    End If

                    'End If

                Else

                    dtEMAIL = dbEntities.t_sended.Where(Function(p) p.EMAILROLE = "SOLICITATION_EMAIL" And p.EMAILACTION = "SOLICITATION OPENED" And p.ID_SOURCE = Id_solicitation_app And p.sn_sended = True).OrderByDescending(Function(o) o.idSend).FirstOrDefault()

                    'If Not IsNothing(dtEMAIL) Then
                    '    dateIni = If(IsNothing(dtEMAIL), Date.UtcNow, dtEMAIL.sn_datesending)
                    '    hrD = DateDiff(DateInterval.Hour, dateIni, Date.UtcNow)
                    'End If

                    If IsNothing(dtEMAIL) Then

                        '**********************************CHEMONICS PROCESSS*******************************************************
                        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1011, regionalizacionCulture)
                        '**********************************CHEMONCS PROCESSS*******************************************************
                        cl_Noti_Process.NOTIFIYING_SOLICITATION_OP(Id_solicitation_app)

                    End If

                    'If hrD > 24 Then

                    '    '**********************************CHEMONICS PROCESSS*******************************************************
                    '    Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                    '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1011, regionalizacionCulture)
                    '    '**********************************CHEMONCS PROCESSS*******************************************************

                    '    cl_Noti_Process.NOTIFIYING_SOLICITATION_OP(Id_solicitation_app)

                    'End If


                End If

                Me.lbl_apply_code.Text = OSolicitationAPP.SOLICITATION_APP_CODE
                Me.lbl_organization.Text = String.Format("{0} ({1})", oActivityAPP.FirstOrDefault.ORGANIZATIONNAME, oActivityAPP.FirstOrDefault.NAMEALIAS)

                Dim oDate As DateTime
                Dim oSTATUS_APP As String = ""
                If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Registered
                    oDate = OSolicitationAPP.fecha_crea
                    oSTATUS_APP = "SOLICITATION REGISTERED"
                ElseIf OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT
                    oDate = OSolicitationAPP.SENT_DATE
                    oSTATUS_APP = "SOLICITATION SENT"
                ElseIf OSolicitationAPP.ID_APP_STATUS = 3 Then 'RECEIVE
                    oDate = OSolicitationAPP.RECEIVED_DATE
                    oSTATUS_APP = "STARTED"
                ElseIf OSolicitationAPP.ID_APP_STATUS = 4 Then 'SUBMITTED
                    oDate = OSolicitationAPP.SUBMITTED_DATE
                    oSTATUS_APP = "SUBMITTED"
                Else
                    oDate = Date.UtcNow
                    oSTATUS_APP = "SOLICITATION REGISTERED"
                End If


                If oApply.Count() > 0 Then

                    Dim idAPP = oApply.FirstOrDefault.ID_APPLY_APP
                    Dim idAPPstatus = oApply.FirstOrDefault.ID_APPLY_STATUS
                    Dim oApplyCOMM = dbEntities.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_APP = idAPP And p.ID_APPLY_STATUS = idAPPstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()

                    Me.txt_apply_desc.Text = oApply.FirstOrDefault.APPLY_DESCRIPTION
                    ''Me.cmb_Apply_status.SelectedValue = oApply.FirstOrDefault.ID_APPLY_STATUS
                    Dim OStatus = dbEntities.TA_APPLY_STATUS.Find(oApply.FirstOrDefault.ID_APPLY_STATUS)
                    Me.lbl_apply_status.Text = OStatus.APPLY_STATUS
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oApplyCOMM.FECHA_CREA, "f", timezoneUTC, True)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("badge {0} text-center", OStatus.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oApplyCOMM.FECHA_CREA, Date.UtcNow)
                    'Me.AppDOCUMENTS.Attributes.Add("style", "display:block;")

                    ' Class="btn btn-warning btn-lg"
                    Me.btn_save_app.Attributes.Add("class", "btn btn-warning btn-lg disabled")
                    Me.tab_index_order.Value = 3
                    Me.tab_index_max.Value = 3


                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        Me.SolicitationACCEPTED.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                        Me.tab_index_order.Value = 4
                        Me.tab_index_max.Value = 4
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then 'On Hold
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.SolicitationACCEPTED.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = True
                        Me.tab_index_order.Value = 4
                        Me.tab_index_max.Value = 4
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 Then 'Accepted
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.SolicitationACCEPTED.Attributes.Add("style", "display:block;")
                        Me.btn_agregar.Enabled = False
                        Me.tab_index_order.Value = 4
                        Me.tab_index_max.Value = 4
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then 'Rejected
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:block;")
                        Me.SolicitationACCEPTED.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                        Me.tab_index_order.Value = 4
                        Me.tab_index_max.Value = 4
                    Else
                        Me.Buttons_app.Attributes.Add("style", "display:block;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.SolicitationACCEPTED.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = True
                    End If

                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 _
                         Or oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                         Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 _
                        Or oApply.FirstOrDefault.ID_APPLY_STATUS = 6 _
                         Then 'Ápplied Or On Hold

                        Me.Buttons_approve.Attributes.Add("style", "display:block; padding-left:30px;")
                        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                        Me.rept_ApplyDates.DataSource = cls_Solicitation.get_Apply_Dates(oApply.FirstOrDefault.ID_APPLY_APP)
                        Me.rept_ApplyDates.DataBind()

                        If oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then
                            'Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            'Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            'Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                        ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                                Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
                            'Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            'Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            'Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left  disabled")
                        Else
                            'Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                            'Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                            'Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
                            Me.btnlk_Apply2.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left  disabled")
                        End If

                    Else
                        Me.Buttons_approve.Attributes.Add("style", "display:none;")
                    End If

                    dtDocuments = get_Apply_Documents(Id_solicitation_app)

                    'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
                    Me.grd_archivos.DataSource = dtDocuments
                    Me.grd_archivos.DataBind()

                Else

                    Me.txt_apply_desc.Text = ""
                    'Me.cmb_Apply_status.SelectedValue = 1
                    Me.lbl_apply_status.Text = oSTATUS_APP
                    'Me.div2.Attributes.Remove("class")
                    'Me.div2.Attributes.Add("class", "alert-sm bg-red text-center")
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oDate, "f", timezoneUTC, True)
                    Me.lbl_Apply_time.Text = Func_Unit(oDate, Date.UtcNow)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", "label-warning"))
                    'Me.AppDOCUMENTS.Attributes.Add("style", "display:none;")
                    Me.Buttons_app.Attributes.Add("style", "display:block;")
                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    Me.btn_agregar.Enabled = True

                    Me.tab_index_order.Value = 0
                    Me.tab_index_max.Value = 3

                End If




                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#dvTab a[href=""#Applications""]').tab('show');", True)

                'Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using


        Catch ex As Exception

        End Try




    End Sub



    Protected Sub SOLITICITATE_APP(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        ''Me.hd_id_doc_support.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
        Dim Id_solicitation_app = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))


        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)


        ''********************************
        ''********************************Enviar Email a ese ID
        ''********************************

        '**********************************CHEMONICS PROCESSS*******************************************************
        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1010, regionalizacionCulture)
        '**********************************CHEMONCS PROCESSS*******************************************************

        Try

            Using dbEntities As New dbRMS_JIEntities


                Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)


                If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Çreated

                    OSolicitationAPP.ID_APP_STATUS = 2 'Sent it
                    OSolicitationAPP.SENT_DATE = Date.UtcNow
                    OSolicitationAPP.id_usuario_sent = Convert.ToInt32(Me.Session("E_IdUser"))
                    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then
                        cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                    End If


                Else
                    cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                End If

                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using


        Catch ex As Exception

        End Try



    End Sub

    Private Sub grd_support_Documents_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_support_Documents.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then


            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                ' solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).submission_documents_path
                document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).solicitation_documents_path

                Dim Id_solicitation_app = Val(Me.lbl_id_sol_app.Text)
                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()

                Dim btn_delete As ImageButton = CType(itemD("Eliminar").Controls(0), ImageButton)

                If oApply.Count > 0 Then
                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                        btn_delete.Visible = False
                    End If
                End If

                If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
                    Dim ImageDownload As New HyperLink
                    ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                    ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                    ImageDownload.Target = "_blank"
                End If

            End Using

        End If

    End Sub

    Private Sub btnlk_Apply_Click(sender As Object, e As EventArgs) Handles btnlk_Apply.Click

        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
        Me.lbl_id_sol_app.Text = Id_solicitation_app


        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Using dbEntities As New dbRMS_JIEntities

            Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
            Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()

            OSolicitationAPP.ID_APP_STATUS = 4 'Submitted
            OSolicitationAPP.SUBMITTED_DATE = Date.UtcNow
            OSolicitationAPP.SUBMITTED = True
            OSolicitationAPP.id_usuario_submit = Convert.ToInt32(Me.Session("E_IdUser"))
            OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

            oApply.ID_APPLY_STATUS = 3 ''Applied
            oApply.APPLY_DATE = Date.UtcNow
            oApply.ID_USUARIO_ASSIGNED = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oApply.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            oApply.APPLY_DESCRIPTION = Me.txt_apply_desc.Text.Trim

            dbEntities.Entry(oApply).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                Dim oApplyComm As New TA_APPLY_COMM
                oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
                oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
                oApplyComm.APPLY_COMM = oApply.APPLY_DESCRIPTION
                oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oApplyComm.FECHA_CREA = Date.UtcNow
                oApplyComm.COMM_BOL = 0

                dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                dbEntities.SaveChanges()

                '**********************************CHEMONICS PROCESSS*******************************************************
                Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1012, regionalizacionCulture)
                '**********************************CHEMONCS PROCESSS*******************************************************
                cl_Noti_Process.NOTIFIYING_SOLICITATION_APPLY(Id_solicitation_app)

            End If

            Me.tab_index_order.Value = 4
            Me.tab_index_max.Value = 4
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using

    End Sub


    Sub createdtcolums(ByVal opt As Integer)


        If opt = 1 Then

            dtDocuments.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
            dtDocuments.Columns.Add("ID_SOLICITATION_APP", GetType(Integer))
            dtDocuments.Columns.Add("ID_ACTIVITY_ANNEX", GetType(Integer))
            dtDocuments.Columns.Add("ID_SOLICITATION_MATERIAL", GetType(Integer))
            dtDocuments.Columns.Add("ID_DOC_SOPORTE", GetType(Integer))
            dtDocuments.Columns.Add("DOCUMENT_TITLE", GetType(String))
            dtDocuments.Columns.Add("DOCUMENTROLE", GetType(String))
            dtDocuments.Columns.Add("nombre_documento", GetType(String))
            dtDocuments.Columns.Add("extension", GetType(String))
            dtDocuments.Columns.Add("max_size", GetType(String))
            dtDocuments.Columns.Add("template", GetType(String))
            dtDocuments.Columns.Add("DOCUMENT_NAME", GetType(String))
            dtDocuments.Columns.Add("REQUIRED_FILE", GetType(String))


        End If

        If opt = 2 Then

            dtApplicants.Columns.Add("ID_ORGANIZATION_APP", GetType(Integer))
            dtApplicants.Columns.Add("ID_SOLICITATION_APP", GetType(Integer))
            dtApplicants.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
            dtApplicants.Columns.Add("SOLICITATION_APP_CODE", GetType(String))
            dtApplicants.Columns.Add("NAMEALIAS", GetType(String))
            dtApplicants.Columns.Add("ORGANIZATIONNAME", GetType(String))
            dtApplicants.Columns.Add("PERSONNAME", GetType(String))
            dtApplicants.Columns.Add("ORGANIZATION_TYPE", GetType(String))
            dtApplicants.Columns.Add("ADDRESSCOUNTRYREGIONID", GetType(String))
            dtApplicants.Columns.Add("ADDRESSCITY", GetType(String))
            dtApplicants.Columns.Add("ORGANIZATIONSTATUS", GetType(String))
            dtApplicants.Columns.Add("ORGANIZATIONEMAIL", GetType(String))
            dtApplicants.Columns.Add("APLICATION_STATUS", GetType(String))
            dtApplicants.Columns.Add("ID_APP_STATUS", GetType(Integer))
            dtApplicants.Columns.Add("SENT_DATE", GetType(DateTime))
            dtApplicants.Columns.Add("RECEIVED_DATE", GetType(DateTime))
            dtApplicants.Columns.Add("SUBMITTED_DATE", GetType(DateTime))

        End If



    End Sub




    Public Function get_Apply_Documents(ByVal idSOL_App As Integer) As DataTable

        Using dbEntities As New dbRMS_JIEntities


            Dim IdACTIVITYsol As Integer = Val(Me.lbl_id_sol.Text)
            Dim oDocuments = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = idSOL_App And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
            Dim oMaterials = dbEntities.VW_TA_SOLICITATION_MATERIALS.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = IdACTIVITYsol).ToList()

            dtDocuments = Nothing
            dtDocuments = New DataTable
            createdtcolums(1)
            Dim Totdocuments As Integer = 0

            For Each itemValues In oDocuments

                If itemValues.Required_file = "MANDATORY" Then
                    Totdocuments += 1
                End If


                dtDocuments.Rows.Add(itemValues.ID_ACTIVITY_SOLICITATION,
                                     itemValues.ID_SOLICITATION_APP,
                                     itemValues.ID_ACTIVITY_ANNEX,
                                     0,
                                     itemValues.id_doc_soporte,
                                     itemValues.DOCUMENT_TITLE,
                                     itemValues.DOCUMENTROLE,
                                     itemValues.nombre_documento,
                                     itemValues.extension,
                                     itemValues.max_size,
                                     itemValues.template,
                                     itemValues.DOCUMENT_NAME,
                                     "ADDITIONAL SUPPORT")



            Next


            'For Each dtRow In tbl_user_role.Rows
            '    strRoles &= dtRow("id_rol").ToString & ","
            'Next

            Dim Totmaterials As Integer = oMaterials.Where(Function(p) p.MANDATORY = True).Count()


            Dim foundMaterial As Boolean = False
            For Each itemMaterial In oMaterials

                For Each dtRow In dtDocuments.Rows

                    If dtRow("id_doc_soporte") = itemMaterial.ID_DOC_SOPORTE Then

                        dtRow("ID_SOLICITATION_MATERIAL") = itemMaterial.ID_DOC_SOPORTE
                        dtRow("REQUIRED_FILE") = If(itemMaterial.MANDATORY, "MANDATORY", "OPTIONAL")
                        foundMaterial = True

                        Exit For

                    End If

                Next


                If foundMaterial = False Then

                    dtDocuments.Rows.Add(itemMaterial.ID_ACTIVITY_SOLICITATION,
                                    idSOL_App,
                                    0,
                                    itemMaterial.ID_SOLICITATION_MATERIAL,
                                    itemMaterial.ID_DOC_SOPORTE,
                                    itemMaterial.DOCUMENT_TITLE,
                                    "",
                                    itemMaterial.nombre_documento,
                                    itemMaterial.extension,
                                    itemMaterial.max_size,
                                    itemMaterial.template,
                                    "",
                                     If(itemMaterial.MANDATORY, "MANDATORY", "Optional"))

                End If

                foundMaterial = False

            Next


            If (Totdocuments = 0) Then
                Me.tab_index_order.Value = 3
                Me.tab_index_max.Value = 3
            ElseIf Totdocuments < Totmaterials Then
                Me.tab_index_order.Value = 3
                Me.tab_index_max.Value = 3
            ElseIf Totdocuments >= Totmaterials Then
                Me.tab_index_order.Value = 4
                Me.tab_index_max.Value = 4
            End If

            get_Apply_Documents = dtDocuments



        End Using



    End Function

    Private Sub btn_save_app_Click(sender As Object, e As EventArgs) Handles btn_save_app.Click


        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)
        Dim id_solicitation_app = Val(Me.lbl_id_sol_app.Text)

        Dim oFechaFin As DateTime

        Using dbEntities As New dbRMS_JIEntities

            Dim oSolicitationAPPLY As New TA_APPLY_APP
            Dim oApplyComm As New TA_APPLY_COMM

            Dim oTA_APPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app)
            Dim AddComm As Boolean = False

            If oTA_APPLY.Count() > 0 Then

                oSolicitationAPPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app).FirstOrDefault()
                oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
                '' oSolicitationAPPLY.ID_APPLY_STATUS = 1 'REgistered
                oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
                oSolicitationAPPLY.APPLY_DESCRIPTION = If(txt_apply_desc.Text.Trim.Length > 1, txt_apply_desc.Text, "")
                'oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oSolicitationAPPLY.fecha_crea = Date.UtcNow
                'oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                dbEntities.Entry(oSolicitationAPPLY).State = Entity.EntityState.Modified

            Else

                oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
                oSolicitationAPPLY.ID_APPLY_STATUS = 2 'REgistered
                oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
                oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
                oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oSolicitationAPPLY.fecha_crea = Date.UtcNow
                oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_APPLY_APP.Add(oSolicitationAPPLY)
                AddComm = True

                '**********************************CHEMONICS PROCESSS*******************************************************
                Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1014, regionalizacionCulture)
                '**********************************CHEMONCS PROCESSS*******************************************************
                cl_Noti_Process.NOTIFIYING_SOLICITATION_STARTED(id_solicitation_app)


            End If

            dbEntities.SaveChanges()

            '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
            'Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'Solicitation / APPLY
            '***************************************************SET TA_ACTIVITY_STATUS************************************************************************




            If AddComm Then

                oApplyComm.ID_APPLY_APP = oSolicitationAPPLY.ID_APPLY_APP
                oApplyComm.ID_APPLY_STATUS = oSolicitationAPPLY.ID_APPLY_STATUS
                oApplyComm.APPLY_COMM = "Starting Application"
                oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oApplyComm.FECHA_CREA = Date.UtcNow
                oApplyComm.COMM_BOL = 0

                dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                dbEntities.SaveChanges()

            End If

            Me.tab_index_order.Value = 3
            Me.tab_index_max.Value = 3

            'Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.TituMensaje = "Confirmation"
            Me.MsgGuardar.NuevoMensaje = "Application started successfully, you can proceed With the Next steps."
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString & "&_tab=Applications"


            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()

        End Using

    End Sub


    Private Sub btnlk_comment_Click(sender As Object, e As EventArgs) Handles btnlk_comment.Click


        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
        Dim id_activity = Val(Me.lbl_id_ficha.Text)


        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_approve_comments.Text.Trim <> "" Then

                Dim strComm As String = Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                Me.Editor_approve_comments.Content = ""

                '*******************************************PENDING SENT NOTIFIACTION************
                ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                '**********************ADDING TRACK COMMENTS*********************************
                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()

                Dim oApplyComm As New TA_APPLY_COMM
                oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
                oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
                oApplyComm.APPLY_COMM = strComm
                oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oApplyComm.FECHA_CREA = Date.UtcNow
                oApplyComm.COMM_BOL = 1

                dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                dbEntities.SaveChanges()

                Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

                '**********************************CHEMONICS PROCESSS*******************************************************
                Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_Programa, 1013, regionalizacionCulture)
                '**********************************CHEMONCS PROCESSS*******************************************************
                cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 0) '' COMMENT"

                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString & "&_tab=Applications"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End If

        End Using

    End Sub

    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getFecha(dateIN, strFormat, boolUTC)

    End Function


    Public Function getHora(dateIN As DateTime) As String

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getHora(dateIN)

    End Function

    Private Sub rept_ApplyDates_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rept_ApplyDates.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rept_ApplyComm")
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            rept_Messages.DataSource = cls_Solicitation.get_Apply_Comments_special(DataBinder.Eval(ItemD.DataItem, "ID_APPLY_APP").ToString(), DataBinder.Eval(ItemD.DataItem, "date_created").ToString())
            rept_Messages.DataBind()

        End If

    End Sub



    Public Shared Function Func_Unit(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As String

        Dim vSeconds As Double
        Dim vMinutes As Double
        Dim vHours As Double
        Dim vDays As Double
        Dim vWeeks As Double
        Dim vMonths As Double
        Dim vYear As Double

        Dim strUnit As String
        Dim vUnit As Double


        vSeconds = DateDiff(DateInterval.Second, StartDate, EndDate)
        vMinutes = DateDiff(DateInterval.Minute, StartDate, EndDate)
        'vHours = DateDiff(DateInterval.Hour, StartDate, EndDate)
        'vDays = DateDiff(DateInterval.Day, StartDate, EndDate)

        vHours = vMinutes / 60
        vDays = vHours / 24
        vWeeks = vDays / 7
        vMonths = vDays / 30
        vYear = vDays / 365


        If vSeconds < 60 Then

            strUnit = " seconds"
            vUnit = Math.Round(vSeconds, 0, MidpointRounding.AwayFromZero)

        ElseIf vMinutes >= 1 And vMinutes < 60 Then

            If vMinutes > 1 Then
                strUnit = " minutes"
            Else
                strUnit = " minute"
            End If

            vUnit = Math.Round(vMinutes, 2, MidpointRounding.AwayFromZero)

        ElseIf vHours >= 1 And vHours < 24 Then

            If vHours > 1 Then
                strUnit = " hours"
            Else
                strUnit = " hour"
            End If

            vUnit = Math.Round(vHours, 2, MidpointRounding.AwayFromZero)

        ElseIf vWeeks < 1 Then

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

    Private Sub btnlk_Apply2_Click(sender As Object, e As EventArgs) Handles btnlk_Apply2.Click


        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

        If Set_Status(3, True) Then 'Applying againg


            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            '**********************************CHEMONICS PROCESSS*******************************************************
            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_Programa, 1013, regionalizacionCulture)
            '**********************************CHEMONCS PROCESSS*******************************************************
            cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 3) '' APPLIED AGAING"

            Me.tab_index_order.Value = 3

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


        End If


    End Sub

    Public Function Set_Status(ByVal idStatus As Integer, ByVal boolUPDdate As Boolean) As Boolean

        Try

            Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
            Me.lbl_id_sol_app.Text = Id_solicitation_app

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities

                If Me.Editor_approve_comments.Text.Trim <> "" Then

                    Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()
                    Dim strComm As String = Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                    Me.Editor_approve_comments.Content = ""

                    oApply.ID_APPLY_STATUS = idStatus
                    If boolUPDdate Then
                        oApply.APPLY_DATE = Date.UtcNow
                    End If
                    oApply.ID_USUARIO_ASSIGNED = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oApply.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.Entry(oApply).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        '*******************************************PENDING SENT NOTIFIACATION************
                        ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

                        '**********************ADDING TRACK COMMENTS*********************************
                        Dim oApplyComm As New TA_APPLY_COMM
                        oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
                        oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
                        oApplyComm.APPLY_COMM = strComm
                        oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oApplyComm.FECHA_CREA = Date.UtcNow
                        oApplyComm.COMM_BOL = 0

                        dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                        dbEntities.SaveChanges()

                        Set_Status = True

                    Else


                        Set_Status = False

                    End If



                Else

                    Set_Status = False

                End If

            End Using


        Catch ex As Exception

            Set_Status = False

        End Try

    End Function



    Private Sub btnlk_add_doc_Click(sender As Object, e As EventArgs) Handles btnlk_add_doc.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)
        Dim id_solicitation_app = Val(Me.lbl_id_sol_app.Text)

        Using dbEntities As New dbRMS_JIEntities

            For Each file As UploadedFile In AsyncUpload1.UploadedFiles

                Dim exten = file.GetExtension()
                Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim anexo As New TA_ACTIVITY_DOCUMENTS
                anexo.DOCUMENT_TITLE = Me.txt_document_tittle.Text
                anexo.DOCUMENT_NAME = nombreArchivo
                anexo.DOCUMENTROLE = "APPLY_ANNEX"
                anexo.id_doc_soporte = cmb_type_of_document.SelectedValue
                anexo.ID_ACTIVITY = id_activity
                anexo.ID_ACTIVITY_SOLICITATION = id_solicitation
                anexo.ID_SOLICITATION_APP = id_solicitation_app
                anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                anexo.fecha_crea = Date.UtcNow
                anexo.visible = True
                anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_ACTIVITY_DOCUMENTS.Add(anexo)

                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).submission_documents_path)
                file.SaveAs(Path + nombreArchivo)

            Next
            dbEntities.SaveChanges()

            Me.tab_index_order.Value = 3
            Me.tab_index_max.Value = 4

            Me.MsgGuardar.TituMensaje = "Confirmation"
            Me.MsgGuardar.NuevoMensaje = "Document uploaded Successfully"
            'cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplyPAR?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString & "&_tab=Applicants"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


        End Using

        'Me.grd_archivos.DataBind()

    End Sub

    Private Sub grd_screening_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_screening.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

                Dim ctrl_Screening_Question As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_colm_SCREENING_QUESTIONS"), RadTextBox)
                ctrl_Screening_Question.Text = DataBinder.Eval(e.Item.DataItem, "question_name")

                Dim ctrl_answer As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_answer"), RadComboBox)
                Dim ctrl_answerTEXT As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                Dim ctrl_answerVALUE As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)

                Dim idScale = DataBinder.Eval(e.Item.DataItem, "id_measurement_answer_scale")

                Dim oTME_Measurement = dbEntities.tme_measurement_answer_scale.Find(idScale)

                Dim answCODE As String = DataBinder.Eval(e.Item.DataItem, "answer_type_code")


                If answCODE = "DROPDOWN" Then

                    ctrl_answer.DataSourceID = ""
                    'ctrl_answer.DataSource = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idScale).Select(Function(p) _
                    '                          New With {Key .option_name = p.option_name,
                    '                                    Key .id_measurement_answer_option = p.id_measurement_answer_option}).ToList()
                    ctrl_answer.DataSource = oTME_Measurement.tme_measurement_answer_option.Select(Function(s) _
                                                                                                   New With {Key .option_name = s.option_name,
                                                                                                              Key .id_measurement_answer_option = s.id_measurement_answer_option}).ToList()

                    ctrl_answer.DataValueField = "id_measurement_answer_option"
                    ctrl_answer.DataTextField = "option_name"
                    ctrl_answer.DataBind()

                    ctrl_answer.Visible = True
                    ctrl_answerTEXT.Visible = False
                    ctrl_answerVALUE.Visible = False

                ElseIf answCODE = "TEXTENTRY" Then

                    ctrl_answer.Visible = False
                    ctrl_answerTEXT.Visible = True
                    ctrl_answerVALUE.Visible = False

                ElseIf answCODE = "VALUEENTRY" Then

                    ctrl_answer.Visible = False
                    ctrl_answerTEXT.Visible = False
                    ctrl_answerVALUE.Visible = True

                End If


            End Using


        End If


    End Sub

    Private Sub btnlk_sent_screening_Click(sender As Object, e As EventArgs) Handles btnlk_sent_screening.Click

        Using dbEntities As New dbRMS_JIEntities

            Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
            ' Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.SOLICITATION_TOKEN =Id_solicitation_app).FirstOrDefault()
            'Dim id_solicitation_app = oSolicitationAPP.ID_SOLICITATION_APP
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

            Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app)
            Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING
            ''***********************************************************************************************************************************************
            ''***************************************************SCREENING APPLICATION***********************************************************************
            'If oTA_APPLY_SCREENING.Count > 0 Then
            '    ' Me.Response.Redirect("~/RFP_/frm_ActivityApplySCRE?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString)

            '    Me.grd_screening.DataSourceID = ""
            '    Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = oTA_APPLY_SCREENING.FirstOrDefault.ID_MEASUREMENT_SURVEY).OrderBy(Function(o) o.order_numberQU).ToList()
            '    Me.grd_screening.DataBind()

            'End If

            Dim bndPASS As Boolean = True

            For Each row In Me.grd_screening.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_answer_scale")
                    Dim ItemD As GridDataItem = CType(row, GridDataItem)
                    Dim answCODE As String = ItemD("answer_type_code").Text
                    'Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)

                    Dim ctrl_answer As RadComboBox = CType(row.Cells(0).FindControl("cmb_answer"), RadComboBox)
                    Dim ctrl_answerTEXT As RadTextBox = CType(row.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                    Dim ctrl_answerVALUE As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)



                    If answCODE = "DROPDOWN" Then

                        If Not IsNothing(ctrl_answer.SelectedValue) Then
                            If Val(ctrl_answer.SelectedValue) = 0 Then
                                bndPASS = False
                            End If
                        Else
                            bndPASS = False
                        End If

                    ElseIf answCODE = "TEXTENTRY" Then

                        If ctrl_answerTEXT.Text.Length < 1 Then
                            bndPASS = False
                        End If

                        'ElseIf answCODE = "VALUEENTRY" Then

                        'If ctrl_answerVALUE.Value = 0 Then
                        '    bndPASS = False
                        'End If

                    End If




                End If
            Next


            If bndPASS Then

                lblt_error.Visible = False


                Dim strHTML As String = " <div class='box-body table-responsive no-padding'>  "
                Dim strHTML_2 As String = " <div class='box-body table-responsive no-padding'>  "

                strHTML &= "<table class='table table-hover'>
                                        <tr>
                                          <th>No</th>
                                          <th>Prescreening Questions</th>
                                          <th>--</th>
                                          <th>--</th>                                          
                                        </tr>"


                strHTML_2 &= "<table class='table table-hover'>
                                        <tr>
                                          <th>No</th>
                                          <th>PreScreening Questions</th>
                                          <th>--</th>
                                          <th>Score</th>
                                          <th>Remaining Question</th>
                                          <th>Remaining Score</th>
                                        </tr>"

                '***********************************************************************************************************

                Dim strRow As String = "<tr>
                                          <th>{3}</th>                     
                                          <td>{0}</td>
                                          <td>{1}</td>
                                          <td>{2}</td>                     
                                        </tr>"

                Dim strRow_2 As String = "<tr>
                                          <th>{3}</th> 
                                          <td>{0}</td>
                                          <td>{1}</td>
                                          <td>{2:N4}</td>
                                          <td>{4}</td>
                                          <td>{5:N4}</td>
                                        </tr>"


                Dim strRowTot As String = ""
                Dim strRowTot_2 As String = ""


                'Dim clDate As APPROVAL.cls_dUtil
                'Dim fechaHOY As DateTime = Date.UtcNow
                ''************************************SYSTEM INFO********************************************
                'Dim cProgram As New SIMEly.cls_ProgramSIME
                'cProgram.get_Sys(0, True)
                'cProgram.get_Programs(cl_user.Id_Cprogram, True)
                'Dim userCulture As CultureInfo
                'Dim timezoneUTC As Integer
                'userCulture = cl_user.regionalizacionCulture
                'timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
                'clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                ''************************************SYSTEM INFO********************************************

                'DateStatus_TS = String.Format(" {0}<br />", clDate.set_DateFormat(fechaHOY, "m", timezoneUTC, False))
                'HourStatus_TS = String.Format(" {0} ", clDate.set_TimeFormat(fechaHOY, timezoneUTC, True))
                ' String.Format("{0:N2}", PorcenPerformed) 
                Dim TotvalScore As Double = 0
                Dim valScore As Double = 0

                Dim idAnswerScale As Integer = 0
                Dim remaining_Score As Double = 0
                Dim totPendingScored As Double = 0
                Dim remaining_Question As String = ""
                Dim idOptionSelected As Integer = 0

                For Each row In Me.grd_screening.Items

                    If TypeOf row Is GridDataItem Then

                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim ItemD As GridDataItem = CType(row, GridDataItem)
                        Dim oTA_APPLY_SCREENING_ANSWER As New TA_APPLY_SCREENING_ANSWER

                        Dim IDquestion_config As Integer = dataItem.GetDataKeyValue("id_measurement_question_config")
                        Dim IDquestion_configEVAL As Integer = dataItem.GetDataKeyValue("id_measurement_question_eval")

                        Dim answCODE As String = ItemD("answer_type_code").Text

                        Dim ctrl_answer As RadComboBox = CType(row.Cells(0).FindControl("cmb_answer"), RadComboBox)
                        Dim ctrl_answerTEXT As RadTextBox = CType(row.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                        Dim ctrl_answerVALUE As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)

                        oTA_APPLY_SCREENING_ANSWER.id_measurement_question_config = IDquestion_config
                        oTA_APPLY_SCREENING_ANSWER.id_measurement_question_eval = IDquestion_configEVAL

                        Dim strAnswer As String = ""

                        If answCODE = "DROPDOWN" Then

                            oTA_APPLY_SCREENING_ANSWER.id_measurement_answer_option = ctrl_answer.SelectedValue
                            strAnswer = ctrl_answer.Text.Trim
                            idOptionSelected = CType(ctrl_answer.SelectedValue, Int32)
                            valScore = dbEntities.tme_measurement_answer_option.Find(idOptionSelected).percent_value
                            ' valScore = dbEntities.ins_measurement_question_config.Find(IDquestion_config).percent_value

                        ElseIf answCODE = "TEXTENTRY" Then

                            oTA_APPLY_SCREENING_ANSWER.measurement_answer_text = ctrl_answerTEXT.Text
                            strAnswer = ctrl_answerTEXT.Text.Trim
                            valScore = 0

                        ElseIf answCODE = "VALUEENTRY" Then

                            oTA_APPLY_SCREENING_ANSWER.measurement_answer_value = ctrl_answerVALUE.Value
                            strAnswer = ctrl_answerVALUE.Value.ToString
                            valScore = 0

                        End If

                        oTA_APPLY_SCREENING_ANSWER.ID_APPLY_SCREENING = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING
                        dbEntities.TA_APPLY_SCREENING_ANSWER.Add(oTA_APPLY_SCREENING_ANSWER)

                        Dim oQUESTION = dbEntities.tme_measurement_question.Find(IDquestion_configEVAL)

                        If Not IsNothing(oQUESTION) Then
                            idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                            remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).Max(Function(f) f.percent_value)
                            remaining_Question = oQUESTION.question_name
                        Else
                            remaining_Score = 0
                            remaining_Question = ""
                        End If

                        totPendingScored += remaining_Score


                        strRowTot &= String.Format(strRow, ItemD("question_name").Text.Trim, strAnswer, "", ItemD("order_numberQU").Text)
                        strRowTot_2 &= String.Format(strRow_2, ItemD("question_name").Text.Trim, strAnswer, valScore, ItemD("order_numberQU").Text, remaining_Question, remaining_Score)

                        TotvalScore += valScore
                        valScore = 0
                        idOptionSelected = 0
                        '{0:N2}
                        'If(ctrl_rbn_YESNO.SelectedValue = 0, "<span class='badge badge-danger'>NOT</span>", "<span class='badge badge-danger'>YES</span>")

                        '***********************************************************************************************************

                    End If

                Next

                strHTML &= strRowTot

                Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)
                Dim oTA_SOLICITATION_APP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
                Dim idACTIVITY_SOL As Integer = oTA_SOLICITATION_APP.ID_ACTIVITY_SOLICITATION

                Dim idScreening As Double = dbEntities.TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = idACTIVITY_SOL).FirstOrDefault.ID_SCREENING
                Dim passValue As Double = dbEntities.TA_SCREENING.Find(idScreening).SCREENING_TOTALPASS

                Dim strTotScore As String = String.Format(If(TotvalScore >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), TotvalScore)
                Dim strtotPendingScored As String = String.Format(If((TotvalScore + totPendingScored) >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), totPendingScored)

                strRowTot_2 &= String.Format("<tr>
                                              <td colspan='2' >{0}</td>
                                              <td>{1}</td>
                                              <td>{2}</td>
                                              <td></td>
                                              <td>{3}</td>
                                             </tr>", "Total Score", "", strTotScore, strtotPendingScored)

                strHTML &= "</table></div><!-- /.box-body -->"

                strHTML_2 &= strRowTot_2 & "</table></div><!-- /.box-body -->"

                Dim lengthA As Integer = Trim(strHTML.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Length
                Dim lengthB As Integer = Trim(strHTML_2.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Length

                lengthA = strHTML.Length
                lengthB = strHTML_2.Length
                '***********************************************************************************************************

                If dbEntities.SaveChanges() Then

                    'Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)
                    oTA_APPLY_SCREE.ID_SCREENING_STATUS = 2 'Responded
                    oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
                    dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        Dim strComment = "Prescreening responded by the applicant " & "<br /><br /><br />" & Trim(strHTML.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                        Dim oScreeningComm As New TA_APPLY_SCREENING_COMM
                        oScreeningComm.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                        oScreeningComm.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                        oScreeningComm.SCREENING_COMM = strComment.Trim
                        oScreeningComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oScreeningComm.FECHA_CREA = Date.UtcNow
                        oScreeningComm.COMM_BOL = 0
                        oScreeningComm.SHOWN_MNGNT_TEAM = 0
                        oScreeningComm.COMM_RES = 1

                        dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm)
                        dbEntities.SaveChanges()

                        strComment = "Prescreening score " & "<br /><br /><br />" & Trim(strHTML_2.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                        Dim oScreeningComm2 = New TA_APPLY_SCREENING_COMM
                        oScreeningComm2.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                        oScreeningComm2.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                        oScreeningComm2.SCREENING_COMM = strComment.Trim
                        oScreeningComm2.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oScreeningComm2.FECHA_CREA = Date.UtcNow
                        oScreeningComm2.COMM_BOL = 0
                        oScreeningComm2.SHOWN_MNGNT_TEAM = 1
                        oScreeningComm2.COMM_RES = 1

                        dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm2)
                        dbEntities.SaveChanges()

                        ''**********************************SCREENNNIG SENT*******************************************************
                        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1015, regionalizacionCulture)
                        '**********************************SCREENNNIG SENT*******************************************************
                        cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING(Id_solicitation_app)

                        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplySCRE?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                    End If

                End If

                '***********************************************************************************************************




            Else

                lblt_error.Visible = True

            End If

            ''******************************************************REDIRECT TO SCREENING APPLICATION**********************************************************
            ''***********************************************************************************************************************************************


        End Using

    End Sub

    Private Sub rept_PrescreeningDates_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rept_PrescreeningDates.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rept_PrescreeningComm")
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            rept_Messages.DataSource = cls_Solicitation.get_Screenning_Comments_special(DataBinder.Eval(ItemD.DataItem, "ID_APPLY_SCREENING").ToString(), DataBinder.Eval(ItemD.DataItem, "date_created").ToString(), 0)
            rept_Messages.DataBind()

        End If

    End Sub
End Class