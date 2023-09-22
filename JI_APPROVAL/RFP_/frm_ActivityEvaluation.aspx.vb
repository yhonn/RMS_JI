Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports ly_RMS

Public Class frm_ActivityEvaluation
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_EVAL"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim valorSuma As Decimal = 0
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim dateUtil As ly_APPROVAL.APPROVAL.cls_dUtil
    Dim timezoneUTC As Integer

    Public Property document_folder As String = ""
    Public Property solicitation_document_folder As String = ""
    Public MAX_POINTS_SEL As Double = 0
    Public ASSESSMENT_TITTLE As String = ""

    Dim dtApplicants As New DataTable

    Dim dtDocuments As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

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
        Dim imagepath() As String = {"~/FileUploads/Documents/Evaluation/ApproveSub"}

        Me.Editor_eval_comments.ImageManager.MaxUploadFileSize = 5243000 '5MB
        Me.Editor_eval_comments.ImageManager.UploadPaths = imagepath
        Me.Editor_eval_comments.ImageManager.ViewPaths = imagepath
        Me.Editor_eval_comments.ImageManager.DeletePaths = imagepath

        Me.Editor_eval_comments.DocumentManager.MaxUploadFileSize = 10490000 '10MB
        Me.Editor_eval_comments.DocumentManager.UploadPaths = imagepath

        Dim pattern() As String = {"*.doc", "*.txt", "*.docx", "*.xls", "*.xlsx", "*.pdf", "*.jpg", "*.jpeg", "*.eps", "*.png", "*.ppt", "*.pptx"}

        Me.Editor_eval_comments.DocumentManager.SearchPatterns = pattern
        Me.Editor_eval_comments.DocumentManager.ViewPaths = imagepath
        Me.Editor_eval_comments.DocumentManager.DeletePaths = imagepath

        If Not Me.IsPostBack Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id

                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                'loadListas(idPrograma, proyecto)

                'LoadData_code(id)

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString()))
                Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                'Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
                Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString()))

                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)
                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then
                    Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))
                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & id.ToString()))
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")
                Else
                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************


                Me.txt_activity_code.Text = proyecto.codigo_ficha_AID

                Me.cmb_solicitation_type.DataSourceID = ""
                Me.cmb_solicitation_type.DataSource = cl_listados.get_TA_SOLICITATION_TYPE(idPrograma)
                Me.cmb_solicitation_type.DataTextField = "SOLICITATION_ACRONY"
                Me.cmb_solicitation_type.DataValueField = "ID_SOLICITATION_TYPE"
                Me.cmb_solicitation_type.DataBind()

                Me.cmb_solicitation_status.DataSourceID = ""
                Me.cmb_solicitation_status.DataSource = cl_listados.get_TA_SOLICITATION_STATUS(idPrograma)
                Me.cmb_solicitation_status.DataTextField = "SOLICITATION_STATUS"
                Me.cmb_solicitation_status.DataValueField = "ID_SOLICITATION_STATUS"
                Me.cmb_solicitation_status.DataBind()

                'Me.cmb_type_of_document.DataSourceID = ""
                'Me.cmb_type_of_document.DataSource = cl_listados.get_ta_docs_soporte()
                'Me.cmb_type_of_document.DataTextField = "nombre_documento"
                'Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
                'Me.cmb_type_of_document.DataBind()

                ''****************************************************************************************************************************
                ''****************************************************************************************************************************
                ''****************************************************************************************************************************

                Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)
                Dim oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.ID_ACTIVITY = proyecto.id_activity).ToList()
                Me.timeline_activity.ID_ACTIVITY = proyecto.id_activity

                If oSolicitation.Count() > 0 Then

                    Me.lbl_id_sol.Text = oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION
                    'Me.ADDons.Attributes.Add("style", "display:block;")

                    LoadSolicitation(oSolicitation.FirstOrDefault)

                    solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).submission_documents_path
                    document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path

                    Me.grd_support_Documents.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
                    Me.grd_support_Documents.DataBind()

                    'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
                    'Me.grd_archivos.DataBind()

                    Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                    'Me.SqlDataSource2.SelectCommand = strSQL

                    ''dtApplicants = get_Activity_Applicants(oSolicitation.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    ''If dtApplicants.Rows.Count = 0 Then
                    ''    createdtcolums(2)
                    ''End If

                    ''Session("dtApplicants") = dtApplicants
                    ''Me.grd_cate.DataSource = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
                    ''Me.grd_cate.DataSource = dtApplicants
                    ''Me.grd_cate.DataBind()
                    Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                    Me.Repeater_Organization.DataSource = cls_Solicitation.get_Applications_ORG(oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION)
                    Me.Repeater_Organization.DataBind()

                    'id ID Activity
                    'ir ID Round
                    'ia ID Solicitation App
                    'is ID Activity SOlicitation

                    If Me.Request.QueryString("ir") IsNot Nothing Then

                        LoadEvaluation(Convert.ToInt32(Me.Request.QueryString("ir")), Convert.ToInt32(Me.Request.QueryString("ia")), Convert.ToInt32(Me.Request.QueryString("is")))
                        '*********************************UPDATED
                        Select_Applicant(Convert.ToInt32(Me.Request.QueryString("ia")))

                    ElseIf Me.Request.QueryString("ia") IsNot Nothing Then
                        '*********************************UPDATED
                        Select_Applicant(Convert.ToInt32(Me.Request.QueryString("ia")))

                    End If



                Else
                    Me.lbl_id_sol.Text = "0"
                    ' Me.SqlDataSource2.SelectCommand = Nothing
                End If


            End Using


        Else

            dtApplicants = Session("dtApplicants")
            MAX_POINTS_SEL = Me.H_MAX_POINTS_SEL.Value

        End If


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

    Protected Sub LoadSolicitation(ByVal oSolicitation As TA_ACTIVITY_SOLICITATION)


        cmb_solicitation_type.SelectedValue = oSolicitation.ID_SOLICITATION_TYPE
        cmb_solicitation_status.SelectedValue = oSolicitation.ID_SOLICITATION_STATUS
        Me.lbl_COde.Text = oSolicitation.SOLICITATION_CODE
        Me.txt_solicitation.Text = oSolicitation.SOLICITATION
        Me.txt_tittle.Text = oSolicitation.SOLICITATION_TITLE
        Me.txt_purpose.Text = oSolicitation.SOLICITATION_PURPOSE

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


    End Sub


    '*********************************UPDATED
    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).submission_documents_path
                'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path

                'Dim btn_delete As ImageButton = CType(itemD("Eliminar").Controls(0), ImageButton)
                Dim Id_solicitation_app = Val(Me.lbl_id_sol_app.Text)
                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()

                'If oApply.Count > 0 Then
                '    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                '        btn_delete.Visible = False
                '    End If
                'End If

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = solicitation_document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"
                ImageDownload.ToolTip = DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

                Dim aprobar As New HyperLink
                aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

                If DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_ANNEX").ToString() = "0" Then
                    ImageDownload.Visible = False
                    'btn_delete.Visible = False

                    If DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString() = "OPTIONAL" Then
                        aprobar.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                        aprobar.ToolTip = "Pending to upload"
                    Else
                        aprobar.ImageUrl = "~/Imagenes/iconos/exclamation.gif"
                        aprobar.ToolTip = "Pending to upload"
                    End If

                Else
                    aprobar.ImageUrl = "~/Imagenes/iconos/accept.png"
                    aprobar.ToolTip = "Uploaded"
                End If

                Dim hlk_File As New HyperLink
                hlk_File = itemD("colm_FileName").FindControl("hlk_filename")

                Dim strFileName As String = DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

                If strFileName.Trim.Length > 1 Then
                    hlk_File.Text = If(strFileName.Trim.Length > 50, Left(strFileName, 47) & "...", strFileName)
                    hlk_File.NavigateUrl = solicitation_document_folder & strFileName
                    hlk_File.ToolTip = strFileName
                Else
                    hlk_File.Text = "&nbsp;"
                    hlk_File.NavigateUrl = "#"
                    hlk_File.ToolTip = "Pending to upload file"
                End If


                'Dim hlk_ref As New HyperLink
                'hlk_ref = itemD("colm_template").FindControl("hlk_template")

                'If Not DataBinder.Eval(e.Item.DataItem, "Template").ToString().Contains("--none--") Then
                '    hlk_ref.Text = DataBinder.Eval(e.Item.DataItem, "Template").ToString()
                '    hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
                'Else
                '    hlk_ref.Text = itemD("Template").Text
                '    hlk_ref.NavigateUrl = "#"
                'End If

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

    'Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
    '    'Dim Path As String
    '    'Path = Server.MapPath("~/FileUploads/")
    '    'e.File.SaveAs(Path + getNewName(e.File))
    'End Sub

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

    '        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?id=" & id_activity.ToString & "&_tab=Applicants"
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '        'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
    '        'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


    '    End Using

    '    'Me.grd_archivos.DataBind()

    'End Sub

    'Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
    '    Me.Response.Redirect("~/RFP_/frm_ActivityE?Id=" & Me.lbl_id_ficha.Text)
    '    'Me.MsgReturn.Redireccion = "~/Proyectos/frm_Proyectos"
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    'End Sub

    'Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
    '    Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    'End Sub

    'Private Sub cmb_solicitation_type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_solicitation_type.SelectedIndexChanged


    '    If Val(Me.cmb_solicitation_type.SelectedValue) > 0 Then

    '        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

    '        Me.lbl_COde.Text = cl_listados.CrearCodigoRFA(idPrograma, Me.cmb_solicitation_type.SelectedValue, 0)
    '        Me.lbl_COde.Visible = True

    '    End If

    'End Sub

    'Protected Sub Organization_DataSourceSelect(sender As Object, e As SearchBoxDataSourceSelectEventArgs)

    '    'Dim id_ficha As Integer = Convert.ToInt32(Me.Session("E_IdFicha"))

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_solicitation = Val(Me.lbl_id_sol.Text)

    '    Dim source As SqlDataSource = DirectCast(e.DataSource, SqlDataSource)
    '    Dim searchBox As RadSearchBox = DirectCast(sender, RadSearchBox)
    '    Dim likeCondition As String = String.Format("'%{0}' + @filterString + '%'", If(searchBox.Filter = SearchBoxFilter.Contains, "%", ""))
    '    Dim countCondition As String = If(e.ShowAllResults, " ", " TOP " + (searchBox.MaxResultCount + 1).ToString())

    '    Dim strSQL As String = ""

    '    'Dim id_ejecutor As Integer = db.tme_Ficha_Proyecto.Where(Function(p) p.id_ficha_proyecto = id_ficha).FirstOrDefault.id_ejecutor
    '    'Dim lsT_ficha = db.tme_Ficha_Proyecto.Where(Function(p) p.id_ejecutor = id_ejecutor) _
    '    '                                      .Select(Function(s) s.id_ficha_proyecto).ToList()

    '    'Dim ArrFicha = lsT_ficha.Select(Function(x) x.ToString()).ToArray()

    '    ''***************Just Profiles of this Activity*******************
    '    'strSQL = String.Format("select * from vw_beneficiary_organization WHERE (id_ficha_proyecto  in ({2}) ) and name LIKE {1} Order by name", countCondition, likeCondition, String.Join(",", ArrFicha))
    '    ''************Or Organization in diferent Query**********

    '    strSQL = String.Format("select A.ID_ORGANIZATION_APP, 
    '                                ltrim(rtrim(A.organization_type)) + ' || ' +  ltrim(rtrim(A.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(A.NAMEALIAS)) + ' || ' + ltrim(rtrim(A.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(A.PERSONNAME)) as Org_search , 
    '                                 organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME  
    '                                   from VW_TA_ORGANIZATION_APP  A
    '					   Left outer join TA_SOLICITATION_APP b on (a.ID_ORGANIZATION_APP = b.ID_ORGANIZATION_APP and b.ID_ACTIVITY_SOLICITATION = {1} )
    '                                 WHERE b.ID_ORGANIZATION_APP IS NULL AND (ltrim(rtrim(a.organization_type)) + ' || ' +  ltrim(rtrim(a.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(a.NAMEALIAS)) + ' || ' + ltrim(rtrim(a.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(a.PERSONNAME)) like {0} )", likeCondition, id_solicitation)

    '    source.SelectCommand = strSQL.Trim
    '    source.SelectParameters.Add("filterString", e.FilterString.Replace("%", "[%]").Replace("_", "[_]"))

    'End Sub


    'Protected Sub Organization_Search(sender As Object, e As SearchBoxEventArgs)

    '    If e.DataItem IsNot Nothing Then

    '        Using dbEntities As New dbRMS_JIEntities

    '            Dim dataItem = DirectCast(e.DataItem, Dictionary(Of String, Object))

    '            Dim nameOrganization As String = e.Text
    '            Dim idOrganization = e.Value
    '            Dim aliasName = dataItem("NAMEALIAS").ToString()

    '            Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '            Dim id_solicitation = Val(Me.lbl_id_sol.Text)

    '            Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_solicitation)

    '            Dim oSolilcitationAPP As New TA_SOLICITATION_APP

    '            oSolilcitationAPP.ID_ACTIVITY_SOLICITATION = id_solicitation
    '            oSolilcitationAPP.ID_APP_STATUS = 1 'REgistered
    '            oSolilcitationAPP.SOLICITATION_APP_CODE = cl_listados.CrearCodigoSOLICITATION(id_solicitation, oActivitySolicitation.SOLICITATION_CODE)
    '            oSolilcitationAPP.SOLICITATION_TOKEN = Guid.Parse(cl_user.GenerateToken())
    '            oSolilcitationAPP.ID_ORGANIZATION_APP = idOrganization

    '            oSolilcitationAPP.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oSolilcitationAPP.fecha_crea = Date.UtcNow
    '            oSolilcitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '            dbEntities.TA_SOLICITATION_APP.Add(oSolilcitationAPP)

    '            dbEntities.SaveChanges()


    '            dtApplicants = get_Activity_Applicants(id_solicitation)
    '            Session("dtApplicants") = dtApplicants
    '            'Me.grd_cate.DataSource = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
    '            Me.grd_cate.DataSource = dtApplicants
    '            Me.grd_cate.DataBind()


    '            'Dim isBeneficiary = dataItem("is_beneficiary").ToString()
    '            'Me.lbl_id_customer.Text = idCustomer
    '            'Me.lbl_is_beneficiary.Text = isBeneficiary
    '            'Me.lbl_customer.Text = nameCustomer

    '            'Using dbEntities As New dbRMS_JIEntities
    '            '    Dim idsale = Me.Request.QueryString("Id").ToString
    '            '    Dim organizationSalesbyvc = From v In dbEntities.vw_ins_sale_detail
    '            '                                Where v.id_sale = idsale
    '            '                                Group v By v.value_chain Into Group
    '            '                                Select value_chain, total_query = Group.Sum(Function(v) v.total_sale)


    '            '    For Each item In organizationSalesbyvc
    '            '        Dim total = Convert.ToString(item.total_query)
    '            '        chart = chart + "{name:'" + item.value_chain + "', y: " + total + "}, "
    '            '    Next

    '            'End Using

    '        End Using


    '    End If
    'End Sub



    'Protected Sub btn_eliminar_Click(sender As Object, e As EventArgs) Handles btn_eliminarDocumento.Click

    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
    '        Dim id_activity = Val(Me.lbl_id_ficha.Text)

    '        Try

    '            Dim ID_SOL_app = Val(Me.identity_sol.Text)
    '            Dim ID_Doc = Val(Me.identity_doc.Text)


    '            If ID_SOL_app > 0 Then

    '                Dim Sql = "delete from TA_SOLICITATION_APP WHERE ID_SOLICITATION_APP = " & ID_SOL_app.ToString
    '                dbEntities.Database.ExecuteSqlCommand(Sql)

    '            End If


    '            Me.MsgGuardar.NuevoMensaje = "The record has been removed succesfully"

    '        Catch ex As SqlException
    '            Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
    '            Me.MsgGuardar.TituMensaje = "Error on removing"
    '        End Try

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '    End Using
    'End Sub


    'Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound

    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        'Dim hlnkEdit As LinkButton = New LinkButton
    '        'hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), LinkButton)
    '        'hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
    '        'hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
    '        ''hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

    '        'Dim hlnkDelete As LinkButton = New LinkButton
    '        'hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
    '        'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
    '        'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())
    '        'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")

    '        Dim visible As New CheckBox
    '        visible = CType(e.Item.FindControl("chkSelected"), CheckBox)
    '        visible.Checked = False
    '        visible.InputAttributes.Add("ID_SOLICITATION_APP", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())
    '        visible.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())

    '        'Dim visible2 As New CheckBox
    '        'visible2 = CType(e.Item.FindControl("chkActivo"), CheckBox)

    '        'If Val(DataBinder.Eval(e.Item.DataItem, "ID_APP_STATUS").ToString()) > 1 Then
    '        '    'hlnkDelete.Visible = False
    '        '    visible2.Checked = True
    '        'End If

    '        Dim lbl_organization As Label = CType(e.Item.FindControl("lbl_organization_n"), Label)
    '        lbl_organization.Text = String.Format("{0} ({1})", DataBinder.Eval(e.Item.DataItem, "ORGANIZATIONNAME"), DataBinder.Eval(e.Item.DataItem, "NAMEALIAS"))

    '        Dim lbl_representative As Label = CType(e.Item.FindControl("lbl_representative"), Label)
    '        lbl_representative.Text = DataBinder.Eval(e.Item.DataItem, "PERSONNAME")

    '        Dim lbl_email As Label = CType(e.Item.FindControl("lbl_email"), Label)
    '        lbl_email.Text = DataBinder.Eval(e.Item.DataItem, "ORGANIZATIONEMAIL")


    '        Dim lbl_tyep As Label = CType(e.Item.FindControl("lbl_tyep"), Label)
    '        lbl_tyep.Text = DataBinder.Eval(e.Item.DataItem, "ORGANIZATION_TYPE")

    '        Dim lbl_country_n As Label = CType(e.Item.FindControl("lbl_country_n"), Label)
    '        lbl_country_n.Text = DataBinder.Eval(e.Item.DataItem, "ADDRESSCOUNTRYREGIONID")

    '        Dim lbl_City_n As Label = CType(e.Item.FindControl("lbl_City_n"), Label)
    '        lbl_City_n.Text = DataBinder.Eval(e.Item.DataItem, "ADDRESSCITY")

    '        Dim lbl_sent_date As Label = CType(e.Item.FindControl("lbl_sent_date"), Label)

    '        If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "SENT_DATE")) Then
    '            lbl_sent_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "SENT_DATE"), "f", dateUtil.offSET, True)
    '        Else
    '            lbl_sent_date.Text = "--"
    '        End If

    '        Dim lbl_received_date As Label = CType(e.Item.FindControl("lbl_received_date"), Label)

    '        If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "RECEIVED_DATE")) Then
    '            lbl_received_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "RECEIVED_DATE"), "f", dateUtil.offSET, True)
    '        Else
    '            lbl_received_date.Text = "--"
    '        End If

    '        Dim lbl_submitted_date As Label = CType(e.Item.FindControl("lbl_submitted_date"), Label)

    '        If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "SUBMITTED_DATE")) Then
    '            lbl_submitted_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "SUBMITTED_DATE"), "f", dateUtil.offSET, True)
    '        Else
    '            lbl_submitted_date.Text = "--"
    '        End If


    '    End If


    'End Sub


    'Protected Sub Select_Applicant(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim chkSelect As CheckBox = CType(sender, CheckBox)
    '    Dim Id_solicitation_app = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
    '    Me.lbl_id_sol_app.Text = Id_solicitation_app

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

    '    Try

    '        Using dbEntities As New dbRMS_JIEntities


    '            Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
    '            Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
    '            Dim oActivityAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = OSolicitationAPP.ID_SOLICITATION_APP).ToList()

    '            '*******************************************************************************************************************************
    '            '*******************************************************MADE FROM ORGANIZATION INTERFACE**************************************
    '            '*******************************************************************************************************************************

    '            'If OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT

    '            '    OSolicitationAPP.ID_APP_STATUS = 3 'Open it
    '            '    OSolicitationAPP.RECEIVED_DATE = Date.UtcNow
    '            '    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '            '    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

    '            '    If dbEntities.SaveChanges() Then
    '            '        ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
    '            '    End If

    '            'Else
    '            '    ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
    '            'End If

    '            Me.lbl_apply_code.Text = OSolicitationAPP.SOLICITATION_APP_CODE
    '            Me.lbl_organization.Text = String.Format("{0} ({1})", oActivityAPP.FirstOrDefault.ORGANIZATIONNAME, oActivityAPP.FirstOrDefault.NAMEALIAS)

    '            Dim oDate As DateTime
    '            Dim oSTATUS_APP As String = ""
    '            If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Registered
    '                oDate = OSolicitationAPP.fecha_crea
    '                oSTATUS_APP = "SOLICITATION REGISTERED"
    '            ElseIf OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT
    '                oDate = OSolicitationAPP.SENT_DATE
    '                oSTATUS_APP = "SOLICITATION SENT"
    '            ElseIf OSolicitationAPP.ID_APP_STATUS = 3 Then 'RECEIVE
    '                oDate = OSolicitationAPP.RECEIVED_DATE
    '                oSTATUS_APP = "OPENED"
    '            ElseIf OSolicitationAPP.ID_APP_STATUS = 4 Then 'SUBMITTED
    '                oDate = OSolicitationAPP.SUBMITTED_DATE
    '                oSTATUS_APP = "SUBMITTED"
    '            Else
    '                oDate = Date.UtcNow
    '                oSTATUS_APP = "SOLICITATION REGISTERED"
    '            End If

    '            If oApply.Count() > 0 Then

    '                Dim idAPP = oApply.FirstOrDefault.ID_APPLY_APP
    '                Dim idAPPstatus = oApply.FirstOrDefault.ID_APPLY_STATUS
    '                Dim oApplyCOMM = dbEntities.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_APP = idAPP And p.ID_APPLY_STATUS = idAPPstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()

    '                Me.txt_apply_desc.Text = oApply.FirstOrDefault.APPLY_DESCRIPTION
    '                ''Me.cmb_Apply_status.SelectedValue = oApply.FirstOrDefault.ID_APPLY_STATUS
    '                Dim OStatus = dbEntities.TA_APPLY_STATUS.Find(oApply.FirstOrDefault.ID_APPLY_STATUS)
    '                Me.lbl_apply_status.Text = OStatus.APPLY_STATUS
    '                Me.lbl_status_date.Text = dateUtil.set_DateFormat(oApplyCOMM.FECHA_CREA, "f", timezoneUTC, True)
    '                Me.spanSTATUS.Attributes.Remove("class")
    '                Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", OStatus.STATUS_FLAG))

    '                Me.lbl_Apply_time.Text = Func_Unit(oApplyCOMM.FECHA_CREA, Date.UtcNow)
    '                'Me.AppDOCUMENTS.Attributes.Add("style", "display:block;")

    '                If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
    '                    Me.Buttons_app.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
    '                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
    '                    Me.btn_agregar.Enabled = False
    '                ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then 'On Hold
    '                    Me.Buttons_app.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
    '                    Me.btn_agregar.Enabled = False
    '                ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 Then 'Accepted
    '                    Me.Buttons_app.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
    '                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
    '                    Me.btn_agregar.Enabled = False
    '                ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then 'Rejected
    '                    Me.Buttons_app.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationRejected.Attributes.Add("style", "display:block;")
    '                    Me.btn_agregar.Enabled = False
    '                Else
    '                    Me.Buttons_app.Attributes.Add("style", "display:block;")
    '                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
    '                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
    '                    Me.btn_agregar.Enabled = True
    '                End If

    '                If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 _
    '                     Or oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
    '                     Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 _
    '                    Or oApply.FirstOrDefault.ID_APPLY_STATUS = 6 _
    '                     Then 'Ápplied Or On Hold

    '                    Me.Buttons_approve.Attributes.Add("style", "display:block; padding-left:30px;")
    '                    Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
    '                    Me.rept_ApplyDates.DataSource = cls_Solicitation.get_Apply_Dates(oApply.FirstOrDefault.ID_APPLY_APP)
    '                    Me.rept_ApplyDates.DataBind()

    '                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then
    '                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_Apply2.Attributes.Add("style", "display:block;")
    '                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
    '                            Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
    '                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
    '                        Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
    '                    Else
    '                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
    '                        Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
    '                        Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
    '                        Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
    '                    End If

    '                Else
    '                    Me.Buttons_approve.Attributes.Add("style", "display:none;")
    '                    Me.rept_ApplyDates.DataSource = Nothing
    '                    Me.rept_ApplyDates.DataBind()
    '                End If


    '            Else

    '                Me.txt_apply_desc.Text = ""
    '                'Me.cmb_Apply_status.SelectedValue = 1
    '                Me.lbl_apply_status.Text = oSTATUS_APP
    '                'Me.div2.Attributes.Remove("class")
    '                'Me.div2.Attributes.Add("class", "alert-sm bg-red text-center")
    '                Me.lbl_status_date.Text = dateUtil.set_DateFormat(oDate, "f", timezoneUTC, True)
    '                Me.lbl_Apply_time.Text = Func_Unit(oDate, Date.UtcNow)
    '                Me.spanSTATUS.Attributes.Remove("class")
    '                Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", "label-warning"))
    '                'Me.AppDOCUMENTS.Attributes.Add("style", "display:none;")
    '                Me.Buttons_app.Attributes.Add("style", "display:block;")
    '                Me.SolicitationALERT.Attributes.Add("style", "display:none;")
    '                Me.SolicitationRejected.Attributes.Add("style", "display:none;")
    '                Me.btn_agregar.Enabled = True


    '            End If

    '            'error over here
    '            For Each Irow As GridDataItem In Me.grd_cate.Items

    '                Dim chkSEL As CheckBox = CType(Irow("colm_select").FindControl("chkSelected"), CheckBox)

    '                If Irow("ID_SOLICITATION_APP").Text = Id_solicitation_app Then
    '                    chkSEL.Checked = True
    '                Else
    '                    chkSEL.Checked = False
    '                End If

    '            Next

    '            dtDocuments = get_Apply_Documents(Id_solicitation_app)

    '            'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
    '            Me.grd_archivos.DataSource = dtDocuments
    '            Me.grd_archivos.DataBind()
    '            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#dvTab a[href=""#Applications""]').tab('show');", True)

    '            'Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
    '            'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '        End Using


    '    Catch ex As Exception

    '    End Try


    'End Sub


    '*********************************UPDATED
    Public Sub Select_Applicant(ByVal idSOL_APP As Integer)


        Dim Id_solicitation_app = idSOL_APP
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


                Me.lbl_apply_code.Text = OSolicitationAPP.SOLICITATION_APP_CODE
                Me.lbl_organization.Text = String.Format("{0} ({1})", oActivityAPP.FirstOrDefault.ORGANIZATIONNAME, oActivityAPP.FirstOrDefault.NAMEALIAS)

                'Dim oDate As DateTime
                'Dim oSTATUS_APP As String = ""
                'If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Registered
                '    oDate = OSolicitationAPP.fecha_crea
                '    oSTATUS_APP = "SOLICITATION REGISTERED"
                'ElseIf OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT
                '    oDate = OSolicitationAPP.SENT_DATE
                '    oSTATUS_APP = "SOLICITATION SENT"
                'ElseIf OSolicitationAPP.ID_APP_STATUS = 3 Then 'RECEIVE
                '    oDate = OSolicitationAPP.RECEIVED_DATE
                '    oSTATUS_APP = "OPENED"
                'ElseIf OSolicitationAPP.ID_APP_STATUS = 4 Then 'SUBMITTED
                '    oDate = OSolicitationAPP.SUBMITTED_DATE
                '    oSTATUS_APP = "SUBMITTED"
                'Else
                '    oDate = Date.UtcNow
                '    oSTATUS_APP = "SOLICITATION REGISTERED"
                'End If

                If oApply.Count() > 0 Then

                    Dim idAPP = oApply.FirstOrDefault.ID_APPLY_APP
                    Dim idAPPstatus = oApply.FirstOrDefault.ID_APPLY_STATUS
                    Dim oApplyCOMM = dbEntities.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_APP = idAPP And p.ID_APPLY_STATUS = idAPPstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()

                    'Me.txt_apply_desc.Text = oApply.FirstOrDefault.APPLY_DESCRIPTION

                    Dim OStatus = dbEntities.TA_APPLY_STATUS.Find(oApply.FirstOrDefault.ID_APPLY_STATUS)
                    Me.lbl_apply_status.Text = OStatus.APPLY_STATUS
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oApplyCOMM.FECHA_CREA, "f", timezoneUTC, True)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", OStatus.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oApplyCOMM.FECHA_CREA, Date.UtcNow)
                    'Me.AppDOCUMENTS.Attributes.Add("style", "display:block;")

                    'If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                    '    Me.Buttons_app.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                    '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    '    Me.btn_agregar.Enabled = False
                    'ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then 'On Hold
                    '    Me.Buttons_app.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    '    Me.btn_agregar.Enabled = False
                    'ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 Then 'Accepted
                    '    Me.Buttons_app.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                    '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    '    Me.btn_agregar.Enabled = False
                    'ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then 'Rejected
                    '    Me.Buttons_app.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationRejected.Attributes.Add("style", "display:block;")
                    '    Me.btn_agregar.Enabled = False
                    'Else
                    '    Me.Buttons_app.Attributes.Add("style", "display:block;")
                    '    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    '    Me.btn_agregar.Enabled = True
                    'End If

                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 _
                         Or oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                         Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 _
                        Or oApply.FirstOrDefault.ID_APPLY_STATUS = 6 _
                         Then 'Ápplied Or On Hold

                        'Me.Buttons_approve.Attributes.Add("style", "display:block; padding-left:30px;")
                        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                        Me.rept_ApplyDates.DataSource = cls_Solicitation.get_Apply_Dates(oApply.FirstOrDefault.ID_APPLY_APP)
                        Me.rept_ApplyDates.DataBind()

                    End If

                End If

                dtDocuments = get_Apply_Documents(Id_solicitation_app)

                Me.grd_archivos.DataSource = dtDocuments
                Me.grd_archivos.DataBind()
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#dvTab a[href=""#Applications""]').tab('show');", True)


            End Using


        Catch ex As Exception

        End Try


    End Sub



    'Protected Sub SOLITICITATE_APP(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim chkSelect As CheckBox = CType(sender, CheckBox)
    '    ''Me.hd_id_doc_support.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
    '    Dim Id_solicitation_app = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))


    '    Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)


    '    ''********************************
    '    ''********************************Enviar Email a ese ID
    '    ''********************************

    '    '**********************************CHEMONICS PROCESSS*******************************************************
    '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1010, regionalizacionCulture)
    '    '**********************************CHEMONCS PROCESSS*******************************************************

    '    Try

    '        Using dbEntities As New dbRMS_JIEntities


    '            Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)


    '            If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Çreated

    '                OSolicitationAPP.ID_APP_STATUS = 2 'Sent it
    '                OSolicitationAPP.SENT_DATE = Date.UtcNow
    '                OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '                dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

    '                If dbEntities.SaveChanges() Then
    '                    cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
    '                End If


    '            Else
    '                cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
    '            End If

    '            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
    '            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '        End Using


    '    Catch ex As Exception

    '    End Try



    'End Sub

    Private Sub grd_support_Documents_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_support_Documents.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                ' solicitation_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).submission_documents_path
                document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).solicitation_documents_path

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"


            End Using

        End If

    End Sub

    'Private Sub btnlk_Apply_Click(sender As Object, e As EventArgs) Handles btnlk_Apply.Click

    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
    '    Me.lbl_id_sol_app.Text = Id_solicitation_app


    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
    '    Using dbEntities As New dbRMS_JIEntities

    '        Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
    '        Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()

    '        OSolicitationAPP.ID_APP_STATUS = 4 'Submitted
    '        OSolicitationAPP.SUBMITTED_DATE = Date.UtcNow
    '        OSolicitationAPP.SUBMITTED = True
    '        OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '        dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

    '        oApply.ID_APPLY_STATUS = 3 ''Applied
    '        oApply.APPLY_DATE = Date.UtcNow

    '        dbEntities.Entry(oApply).State = Entity.EntityState.Modified

    '        If dbEntities.SaveChanges() Then

    '            '*******************************************PENDING SENT NOTIFIACTION************
    '            ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

    '            '**********************ADDING TRACK COMMENTS*********************************
    '            Dim oApplyComm As New TA_APPLY_COMM
    '            oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
    '            oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
    '            oApplyComm.APPLY_COMM = oApply.APPLY_DESCRIPTION
    '            oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oApplyComm.FECHA_CREA = Date.UtcNow
    '            oApplyComm.COMM_BOL = 0

    '            dbEntities.TA_APPLY_COMM.Add(oApplyComm)
    '            dbEntities.SaveChanges()



    '        End If

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


    '    End Using


    'End Sub


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

            For Each itemValues In oDocuments

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
                                     If(itemMaterial.MANDATORY, "MANDATORY", "OPTIONAL"))

                End If

                foundMaterial = False

            Next

            get_Apply_Documents = dtDocuments



        End Using


    End Function

    'Private Sub btn_save_app_Click(sender As Object, e As EventArgs) Handles btn_save_app.Click

    '    Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '    Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim id_solicitation = Val(Me.lbl_id_sol.Text)
    '    Dim id_solicitation_app = Val(Me.lbl_id_sol_app.Text)

    '    Dim oFechaFin As DateTime

    '    Using dbEntities As New dbRMS_JIEntities

    '        Dim oSolicitationAPPLY As New TA_APPLY_APP
    '        Dim oApplyComm As New TA_APPLY_COMM

    '        Dim oTA_APPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app)
    '        Dim AddComm As Boolean = False

    '        If oTA_APPLY.Count() > 0 Then

    '            oSolicitationAPPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app).FirstOrDefault()
    '            oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
    '            '' oSolicitationAPPLY.ID_APPLY_STATUS = 1 'REgistered
    '            oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
    '            oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
    '            'oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            'oSolicitationAPPLY.fecha_crea = Date.UtcNow
    '            'oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
    '            dbEntities.Entry(oSolicitationAPPLY).State = Entity.EntityState.Modified

    '        Else

    '            oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
    '            oSolicitationAPPLY.ID_APPLY_STATUS = 2 'REgistered
    '            oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
    '            oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
    '            oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oSolicitationAPPLY.fecha_crea = Date.UtcNow
    '            oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '            dbEntities.TA_APPLY_APP.Add(oSolicitationAPPLY)
    '            AddComm = True


    '        End If

    '        dbEntities.SaveChanges()

    '        If AddComm Then

    '            oApplyComm.ID_APPLY_APP = oSolicitationAPPLY.ID_APPLY_APP
    '            oApplyComm.ID_APPLY_STATUS = oSolicitationAPPLY.ID_APPLY_STATUS
    '            oApplyComm.APPLY_COMM = "Application Registered"
    '            oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oApplyComm.FECHA_CREA = Date.UtcNow
    '            oApplyComm.COMM_BOL = 0

    '            dbEntities.TA_APPLY_COMM.Add(oApplyComm)
    '            dbEntities.SaveChanges()

    '        End If

    '        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '        'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
    '        'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()

    '    End Using


    'End Sub


    'Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


    '    Dim Dif_Porce As Double = porcDays - porcEDays
    '    Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

    '    Const c_label_danger As String = "label-danger"
    '    Const c_label_warning As String = "label-warning"
    '    Const c_label_primary As String = "label-primary"
    '    Const c_label_success As String = "label-success"

    '    Const c_progress_bar_warning = "progress-bar-warning"
    '    Const c_progress_bar_primary = "progress-bar-primary"
    '    Const c_progress_bar_danger = "progress-bar-danger"

    '    Dim strResult As String = ""
    '    Dim strStatus As String = ""
    '    Dim strAlertBar1 As String = ""
    '    Dim strAlertBar2 As String = ""

    '    If porc_Progress >= 0 Then

    '        'Inverter number
    '        If ((1 - porc_Progress) * 100) >= 90 Then
    '            strResult = c_label_danger
    '        ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
    '            strResult = c_label_warning
    '        ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
    '            strResult = c_label_primary
    '        Else
    '            strResult = c_label_success
    '        End If

    '        strStatus = "Pending"
    '        strAlertBar2 = c_progress_bar_primary

    '    Else 'Expired
    '        strResult = c_label_danger
    '        strStatus = "Expired"
    '        strAlertBar2 = c_progress_bar_danger
    '    End If

    '    strAlertBar1 = c_progress_bar_warning

    '    If alertType = 1 Then
    '        Func_Alert = strResult
    '    ElseIf alertType = 2 Then
    '        Func_Alert = strStatus
    '    ElseIf alertType = 3 Then
    '        Func_Alert = strAlertBar1
    '    Else
    '        Func_Alert = strAlertBar2
    '    End If


    'End Function


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

        If IsDBNull(StartDate) Then
            StartDate = Date.Now
        End If

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

    'Private Sub bntlk_accept_Click(sender As Object, e As EventArgs) Handles bntlk_accept.Click

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

    '    If Set_Status(4, False) Then 'Accepted

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '    End If

    'End Sub


    'Public Function Set_Status(ByVal idStatus As Integer, ByVal boolUPDdate As Boolean) As Boolean

    '    Try

    '        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
    '        Me.lbl_id_sol_app.Text = Id_solicitation_app

    '        Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

    '        Using dbEntities As New dbRMS_JIEntities

    '            If Me.Editor_approve_comments.Text.Trim <> "" Then

    '                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()
    '                Dim strComm As String = Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
    '                Me.Editor_approve_comments.Content = ""

    '                oApply.ID_APPLY_STATUS = idStatus
    '                If boolUPDdate Then
    '                    oApply.APPLY_DATE = Date.UtcNow
    '                End If


    '                dbEntities.Entry(oApply).State = Entity.EntityState.Modified

    '                If dbEntities.SaveChanges() Then

    '                    '*******************************************PENDING SENT NOTIFIACATION************
    '                    ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

    '                    '**********************ADDING TRACK COMMENTS*********************************
    '                    Dim oApplyComm As New TA_APPLY_COMM
    '                    oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
    '                    oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
    '                    oApplyComm.APPLY_COMM = strComm
    '                    oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '                    oApplyComm.FECHA_CREA = Date.UtcNow
    '                    oApplyComm.COMM_BOL = 0

    '                    dbEntities.TA_APPLY_COMM.Add(oApplyComm)
    '                    dbEntities.SaveChanges()

    '                    Set_Status = True

    '                Else


    '                    Set_Status = False

    '                End If



    '            Else

    '                Set_Status = False

    '            End If

    '        End Using


    '    Catch ex As Exception

    '        Set_Status = False

    '    End Try

    'End Function

    Public Function getDate_(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getFecha(dateIN, strFormat, boolUTC)

    End Function


    Public Function getTime_(dateIN As DateTime) As String

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getHora(dateIN)

    End Function

    '*********************************UPDATED
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


    'Private Sub btnlk_comment_Click(sender As Object, e As EventArgs) Handles btnlk_comment.Click


    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)


    '    Using dbEntities As New dbRMS_JIEntities

    '        If Me.Editor_approve_comments.Text.Trim <> "" Then

    '            Dim strComm As String = Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
    '            Me.Editor_approve_comments.Content = ""

    '            '*******************************************PENDING SENT NOTIFIACTION************
    '            ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

    '            '**********************ADDING TRACK COMMENTS*********************************
    '            Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault()

    '            Dim oApplyComm As New TA_APPLY_COMM
    '            oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
    '            oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
    '            oApplyComm.APPLY_COMM = strComm
    '            oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oApplyComm.FECHA_CREA = Date.UtcNow
    '            oApplyComm.COMM_BOL = 1

    '            dbEntities.TA_APPLY_COMM.Add(oApplyComm)
    '            dbEntities.SaveChanges()

    '            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '        End If

    '    End Using

    'End Sub

    'Private Sub btnlk_hold_Click(sender As Object, e As EventArgs) Handles btnlk_hold.Click


    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

    '    If Set_Status(6, False) Then 'On Hold Status

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '    End If


    'End Sub

    'Private Sub btnlk_Apply2_Click(sender As Object, e As EventArgs) Handles btnlk_Apply2.Click

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

    '    If Set_Status(3, True) Then 'Applying againg

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '    End If

    'End Sub

    'Private Sub btnlk_reject_Click(sender As Object, e As EventArgs) Handles btnlk_reject.Click

    '    Dim id_activity = Val(Me.lbl_id_ficha.Text)
    '    Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

    '    If Set_Status(5, False) Then 'Rejected

    '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '    End If

    'End Sub


    Public Sub LoadEvaluation(ByVal idRound As Integer, ByVal idAPPsol As Integer, ByVal id_ActSol As Integer)

        Using dbEntities As New dbRMS_JIEntities


            Dim oEval = dbEntities.TA_APPLY_EVALUATION.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_ActSol).FirstOrDefault()
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = idAPPsol).FirstOrDefault()
            Me.txt_guidLines.Text = oEval.EVALUATION_DESCRIPTION
            Me.H_ID_SOLICITATION_APP.Value = idAPPsol
            Me.H_ID_ACTIVITY_SOLICITATION.Value = id_ActSol

            lbl_evaluation_startDate.Text = dateUtil.set_DateFormat(oEval.EVALUATION_START_DATE, "f", timezoneUTC, True)
            lbl_evaluation_EndDate.Text = dateUtil.set_DateFormat(oEval.EVALUATION_END_DATE, "f", timezoneUTC, True)

            'Me.txt_rounds.Value = oEval.TOT_ROUNDS

            Dim IDaCTsol = Convert.ToInt32(oEval.ID_ACTIVITY_SOLICITATION)
            Dim APPeval = Convert.ToInt32(oEval.ID_APPLY_EVALUATION)
            Me.H_ID_APPLY_EVALUATION.Value = APPeval
            Dim oMembers = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = APPeval)

            Me.grd_team.DataSource = get_Activity_Members(IDaCTsol)
            Me.grd_team.DataBind()


            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = APPeval And p.ID_ROUND = idRound).FirstOrDefault()
            'Me.H_ROUND_ID.Value = TotRound.Count()
            'Me.lbl_round.Text = String.Format("#{0}", (Val(Me.H_ROUND_ID.Value) + 1).ToString())

            Me.lbl_round_startDate.Text = dateUtil.set_DateFormat(oRound.ROUND_START_DATE, "f", timezoneUTC, True)
            Me.lbl_round_endDate.Text = dateUtil.set_DateFormat(oRound.ROUND_END_DATE, "f", timezoneUTC, True)
            Me.H_ROUND_ID.Value = idRound

            Me.lbl_round_number.Text = String.Format("#{0}", oRound.ID_ROUND)
            Me.lbl_round_organization.Text = String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS)
            Me.lbl_voting_type.Text = oRound.VOTING_TYPE
            Me.lbl_app_tot.Text = oRound.TOT_APP_SELECTED
            Me.lbl_tot_votes.Text = oRound.VOTES_MAX
            Me.lbl_total_points.Text = oRound.POINTS_TOTAL
            Me.lbl_max_points.Text = oRound.POINTS_MAX

            Me.grd_rounds.DataSource = get_Evaluation_Rounds(APPeval)
            Me.grd_rounds.DataBind()

            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)
            Me.H_ID_EVALUATION_ROUND.Value = idEVALround

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idApplyEval = Convert.ToInt32(oEval.ID_APPLY_EVALUATION)
            'Dim oUsers = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval).Select(Function(p) p.ID_USER).ToList()

            'Me.cmb_rol.DataSourceID = ""
            'Me.cmb_rol.DataSource = dbEntities.vw_ta_roles_user_all.Where(Function(p) p.id_programa = idPrograma And p.id_type_role = 1 And Not oUsers.Contains(p.id_usuario)) _
            '                    .Select(Function(p) New With {Key .id_user = p.id_usuario,
            '                                                      .user_name = p.nombre_usuario & " (" & p.descripcion_rol & ") - " & p.email_usuario}).ToList
            'Me.cmb_rol.DataTextField = "user_name"
            'Me.cmb_rol.DataValueField = "id_user"
            'Me.cmb_rol.DataBind()

            'Me.cmb_eval_document_type.DataSourceID = ""
            'Me.cmb_eval_document_type.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
            'Me.cmb_eval_document_type.DataTextField = "nombre_documento"
            'Me.cmb_eval_document_type.DataValueField = "id_doc_soporte"
            'Me.cmb_eval_document_type.DataBind()

            'Me.cmb_voting_type.DataSourceID = ""
            'Me.cmb_voting_type.DataSource = cl_listados.get_TA_VOTING_TYPE(idPrograma)
            'Me.cmb_voting_type.DataTextField = "VOTING_TYPE"
            'Me.cmb_voting_type.DataValueField = "ID_VOTING_TYPE"
            'Me.cmb_voting_type.DataBind()

            'Me.EVAL_2.Attributes.Add("style", "display:block;")

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Me.grd_eval_Document.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "EVALUATION_SETUP_DOC").ToList()
            Me.grd_eval_Document.DataBind()

            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)
            Dim oAPPLYapp = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = idAPPsol).FirstOrDefault()
            Dim idAPPLYapp = Convert.ToInt32(oAPPLYapp.ID_APPLY_APP)
            Me.H_ID_APPLY_APP.Value = idAPPLYapp
            Dim oEVAL_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVALround And p.ID_APPLY_APP = idAPPLYapp)

            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            'Never Opened
            If oEVAL_APP.Count = 0 Then

                Dim oEVALUATION_APP = New TA_EVALUATION_APP

                oEVALUATION_APP.ID_EVALUATION_ROUND = idEVALround
                oEVALUATION_APP.ID_APPLY_APP = idAPPLYapp
                oEVALUATION_APP.EVALUATION_START_DATE = Date.UtcNow
                oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oRound.ID_VOTING_TYPE)
                oEVALUATION_APP.EVALUATION_SCORE = 0
                oEVALUATION_APP.EVALUATION_VOTES = 0
                oEVALUATION_APP.EVALUATION_UNTIED = 0
                oEVALUATION_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oEVALUATION_APP.FECHA_CREA = Date.UtcNow
                oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim idEVALapp As Integer
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, 0), idEVALapp) Then
                    'Save Evaluation Comment
                    Me.H_ID_EVALUATION_APP.Value = idEVALapp
                    Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                    oTA_EVALUATION_APP_COMM.ROUND = oRound.ID_ROUND
                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oRound.ID_VOTING_TYPE)
                    oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} opened for {1} ", idEVALround, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS))
                    oTA_EVALUATION_APP_COMM.SCORE = 0
                    oTA_EVALUATION_APP_COMM.VOTE = 0
                    oTA_EVALUATION_APP_COMM.POINTS = 0
                    oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    oTA_EVALUATION_APP_COMM.COMM_BOL = False
                    oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

                End If

                Me.Buttons_approve.Visible = False
                Me.conflicto_intereses.Visible = True

            Else

                Me.H_ID_EVALUATION_APP.Value = oEVAL_APP.FirstOrDefault().ID_EVALUATION_APP
                Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                Dim idEstadoCIPOSITIVE = Get_STATUS(4, 1, True)
                Dim idEstadoCINEGAVITE = Get_STATUS(4, 1, False)
                Dim oEvalCommCI = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = oEVAL_APP.FirstOrDefault().ID_EVALUATION_APP And p.ID_USUARIO_CREA = idUsuario And (p.ID_EVALUATION_APP_STATUS = idEstadoCIPOSITIVE Or p.ID_EVALUATION_APP_STATUS = idEstadoCINEGAVITE)).ToList()
                If oEvalCommCI.Count() > 0 Then
                    Me.conflicto_intereses.Visible = False
                    Me.Buttons_approve.Visible = True
                    Dim conflictoIntereses = oEvalCommCI.FirstOrDefault()
                    If conflictoIntereses.ID_EVALUATION_APP_STATUS = idEstadoCINEGAVITE Then
                        Me.Buttons_approve.Visible = False
                    End If

                Else
                    Me.conflicto_intereses.Visible = True
                    Me.Buttons_approve.Visible = False
                End If
            End If

            If oRound.ID_VOTING_TYPE = 1 Then 'Score

                Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-primary btn-sm disabled")
                Me.btnlk_accept_Evaluation.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.div_aggregate.Attributes.Add("style", "display:none;")

                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************
                CHECK_EVAL("VOTE_YET_SCORE", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), idEVALround)
                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************

                'Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)
                'Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()
                'Me.grd_answers.DataSourceID = ""
                'Me.grd_answers.DataSource = dbEntities.VW_TA_EVALUATION_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
                'Me.grd_answers.DataBind()
                Me.grd_screening.DataSourceID = ""
                Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = oRound.id_measurement_survey And p.id_programa = idPrograma).OrderBy(Function(o) o.order_number).ThenBy(Function(a) a.order_numberQU).ToList()
                Me.grd_screening.DataBind()
                ASSESSMENT_TITTLE = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = oRound.id_measurement_survey).FirstOrDefault.survey_name
                Me.lbl_round.Text = ASSESSMENT_TITTLE

                Me.lbl_asses_organization.Text = oSol.NAMEALIAS

                Me.lbl_amount_LOC.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oAPPLYapp.APPLY_AMOUNT_LOC, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                Me.lbl_amount.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} USD", oAPPLYapp.APPLY_AMOUNT)
                'String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oAPPLYapp.APPLY_AMOUNT_LOC, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)


            ElseIf oRound.ID_VOTING_TYPE = 2 Then 'Popularity

                Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.div_aggregate.Attributes.Add("style", "display:none;")
                Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************
                CHECK_EVAL("VOTE_YET", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), idEVALround)
                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************

                '*********************************UNTIED BUTTON**********************************
                If (oEVAL_APP.FirstOrDefault.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oRound.ID_VOTING_TYPE)) Then 'TIE STATUS
                    CHECK_EVAL("VOTE_ADD", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), idEVALround)
                End If
                '*********************************UNTIED BUTTON**********************************


            ElseIf oRound.ID_VOTING_TYPE = 3 Then 'Review

                Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left")
                Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")

                Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")
                Me.div_aggregate.Attributes.Add("style", "display:none;")

                Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************
                CHECK_EVAL("VOTE_YET_REV", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oRound.ID_EVALUATION_ROUND)
                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************

                '*********************************UNTIED BUTTON**********************************
                If (oEVAL_APP.FirstOrDefault.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oRound.ID_VOTING_TYPE)) Then 'TIE STATUS
                    CHECK_EVAL("VOTE_REV_ADD", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), idEVALround)
                End If
                '*********************************UNTIED BUTTON**********************************

            ElseIf oRound.ID_VOTING_TYPE = 4 Then 'Points

                Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled hide")
                Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                Me.div_aggregate.Attributes.Add("style", "display:block;")

                Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled hide")

                MAX_POINTS_SEL = oRound.POINTS_MAX
                Me.H_MAX_POINTS_SEL.Value = oRound.POINTS_MAX

                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************
                CHECK_EVAL("VOTE_YET_POINTS", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oRound.ID_EVALUATION_ROUND)
                '****************************CHECK FOR VOTES, POINTS, SCORES, REVIEW, NEGOTIATION***************

                ''*********************************UNTIED BUTTON**********************************
                'If (oEVAL_APP.FirstOrDefault.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oRound.ID_VOTING_TYPE)) Then 'TIE STATUS
                '    CHECK_EVAL("VOTE_REV_ADD", Me.H_ID_EVALUATION_APP.Value, Convert.ToInt32(Me.Session("E_IdUser").ToString()), idEVALround)
                'End If
                ''*********************************UNTIED BUTTON**********************************

            ElseIf oRound.ID_VOTING_TYPE = 5 Then 'Negotiation



            End If

            Me.rept_ApplyDates_eval.DataSource = cls_Solicitation.get_Apply_Dates_EVAL(Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value))
            Me.rept_ApplyDates_eval.DataBind()

        End Using

    End Sub

    Public Function Get_STATUS(ByVal Sorder As Integer, ByVal idTYPE As Integer, Optional STpostivie As Boolean = True) As Integer

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_EVALUATION_APP_STATUS = dbEntities.TA_EVALUATION_APP_STATUS.Where(Function(p) p.ID_VOTING_TYPE = idTYPE And p.STATUS_ORDER = Sorder).ToList()

            If oTA_EVALUATION_APP_STATUS.Count > 1 Then

                If oTA_EVALUATION_APP_STATUS.Where(Function(p) p.STATUS_POSITIVE = STpostivie).Count() > 0 Then

                    Get_STATUS = oTA_EVALUATION_APP_STATUS.Where(Function(p) p.STATUS_POSITIVE = STpostivie).FirstOrDefault.ID_EVALUATION_APP_STATUS

                Else

                    Get_STATUS = 0

                End If

            Else

                If oTA_EVALUATION_APP_STATUS.Count = 1 Then
                    Get_STATUS = oTA_EVALUATION_APP_STATUS.FirstOrDefault.ID_EVALUATION_APP_STATUS
                Else
                    Get_STATUS = 0
                End If

            End If

        End Using

    End Function

    Function get_Activity_Members(ByVal id_solicitation As Integer) As DataTable

        Using dbEntities As New dbRMS_JIEntities

            Dim oSolicitation = dbEntities.VW_TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation) _
                .Select(Function(p) New With {Key .ID_SOLICITATION_EVALUATION_TEAM = p.ID_SOLICITATION_EVALUATION_TEAM,
                                                  .ID_ACTIVITY_SOLICITATION = p.ID_ACTIVITY_SOLICITATION,
                                                  .ID_USER = p.ID_USER,
                                                  .EVALUATION_ROLE = p.EVALUATION_ROLE,
                                                  .id_rol = p.id_rol,
                                                  .nombre_rol = p.nombre_rol,
                                                  .descripcion_rol = p.descripcion_rol,
                                                  .nombre_usuario = p.nombre_usuario,
                                                  .usuario = p.usuario,
                                                   .email_usuario = p.email_usuario}).ToList()
            Dim cl_utl As New CORE.cls_util

            Return cl_utl.ConvertToDataTable(oSolicitation)

        End Using

    End Function

    Function get_Evaluation_Rounds(ByVal ID_APP_EVALUATION As Integer) As DataTable

        Using dbEntities As New dbRMS_JIEntities

            Dim oROUNDS = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = ID_APP_EVALUATION) _
                .Select(Function(p) New With {Key .ID_EVALUATION_ROUND = p.ID_EVALUATION_ROUND,
                                                    .ID_APPLY_EVALUATION = p.ID_APPLY_EVALUATION,
                                                    .ID_VOTING_TYPE = p.ID_VOTING_TYPE,
                                                    .VOTING_TYPE = p.VOTING_TYPE,
                                                    .ID_ROUND = p.ID_ROUND,
                                                    .ROUND_START_DATE = p.ROUND_START_DATE,
                                                    .ROUND_END_DATE = p.ROUND_END_DATE,
                                                    .SCORE_BASE = p.SCORE_BASE,
                                                    .POINTS_TOTAL = p.POINTS_TOTAL,
                                                    .POINTS_MAX = p.POINTS_MAX,
                                                    .VOTES_MAX = p.VOTES_MAX,
                                                    .TOT_APP_SELECTED = p.TOT_APP_SELECTED
                                                   }).ToList()
            Dim cl_utl As New CORE.cls_util

            Return cl_utl.ConvertToDataTable(oROUNDS)

        End Using

    End Function


    Private Sub grd_team_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_team.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'Dim hlk_ref As New HyperLink
            'hlk_ref = itemD("colm_template").FindControl("hlk_template")

            'If Not DataBinder.Eval(e.Item.DataItem, "Template").ToString().Contains("--none--") Then
            '    hlk_ref.Text = DataBinder.Eval(e.Item.DataItem, "Template").ToString()
            '    hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            'Else
            '    hlk_ref.Text = itemD("Template").Text
            '    hlk_ref.NavigateUrl = "#"
            'End If

            'Dim visible As New CheckBox
            'visible = CType(e.Item.FindControl("chkSelected"), CheckBox)
            'visible.Checked = False
            'visible.InputAttributes.Add("ID_SOLICITATION_MATERIAL", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_MATERIAL").ToString())

            Dim txtCtrl As New RadTextBox
            txtCtrl = CType(e.Item.FindControl("txtROLE"), RadTextBox)
            txtCtrl.ReadOnly = True
            txtCtrl.Text = DataBinder.Eval(e.Item.DataItem, "EVALUATION_ROLE").ToString()

            'If DataBinder.Eval(e.Item.DataItem, "EVALUATION_ROLE").ToString() = "True" Then
            'Else
            '    txtCtrl.Text = "Optional"
            'End If

        End If

    End Sub


    Private Sub grd_eval_Document_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_eval_Document.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim evalPath = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).Evaluation_documents_path

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = evalPath & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()
                ImageDownload.Target = "_blank"

            End Using

        End If

    End Sub

    Private Sub btnlk_comment_Click(sender As Object, e As EventArgs) Handles btnlk_comment.Click


        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then

                Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                Me.Editor_eval_comments.Content = ""

                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
                Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                '' Dim oEVALUATION_APP = New TA_EVALUATION_APP

                '    oEVALUATION_APP.ID_EVALUATION_ROUND = idEVALround
                '    oEVALUATION_APP.ID_APPLY_APP = idAPPLYapp
                '    oEVALUATION_APP.EVALUATION_START_DATE = Date.UtcNow
                '    oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oRound.ID_VOTING_TYPE)
                '    oEVALUATION_APP.EVALUATION_SCORE = 0
                '    oEVALUATION_APP.EVALUATION_VOTES = 0
                '    oEVALUATION_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                '    oEVALUATION_APP.FECHA_CREA = Date.UtcNow
                '    oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)

                'Dim idEVAL_round = Convert.ToInt32(oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idEVALround).FirstOrDefault().ID_EVALUATION_ROUND)
                'Dim idEVAL_app = Convert.ToInt32(oTA_EVALUATION_APP.ID_EVALUATION_APP)

                Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = True
                oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened

                    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                    oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                    oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    Dim num As Integer
                    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then
                        'Save Evaluation APP Comment

                        '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                        Dim id_activity = Val(Me.lbl_id_ficha.Text)
                        cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'APPLY / EVALUATION
                        '***************************************************SET TA_ACTIVITY_STATUS************************************************************************

                        '*********CHANGING STATUS******************
                        oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                        '******************SAVING COMMENT****************************************
                        cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

                    End If

                Else

                    '******************SAVING COMMENT****************************************
                    cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

                End If

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Dim ranNUMBER As System.Random = New System.Random()
                ranNUMBER.Next(1, 1000)

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                'Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX&rd={5}", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value, ranNUMBER.Next(1, 1000))
                Me.MsgGuardar.Redireccion = ""
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using



    End Sub

    Private Sub rept_ApplyDates_eval_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rept_ApplyDates_eval.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rept_ApplyComm_eval")
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            rept_Messages.DataSource = cls_Solicitation.get_Apply_Comments_EVAL(DataBinder.Eval(ItemD.DataItem, "ID_EVALUATION_APP").ToString(), DataBinder.Eval(ItemD.DataItem, "date_created").ToString())
            rept_Messages.DataBind()

        End If

    End Sub

    Private Sub bntlk_accept_Click(sender As Object, e As EventArgs) Handles bntlk_accept.Click


        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then


                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                Select Case oRound.ID_VOTING_TYPE

                    Case 1

                        F_SCORING(True, 0, "")


                    Case 2

                        F_VOTING(True)

                    Case 3

                        F_REVIEW(True)

                    Case 4

                        F_POINTS(True, 0)

                    Case 5

                        F_NEGOTIATE(True)

                End Select

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using


    End Sub


    Sub F_SCORING(ByVal boolScoring As Boolean, ByVal scoreValues As Double, ByVal strAssessmentHTML As String)


        Using dbEntities As New dbRMS_JIEntities

            ' If boolVoting Then

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim idApplyEval = Convert.ToInt32(oTA_APPLY_EVALUATION.ID_APPLY_EVALUATION)
            Dim oUsers = dbEntities.VW_TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval And p.ID_USER = idUser).FirstOrDefault

            Dim strComm As String = String.Format("Assessment responded by {0}({1}) ", oUsers.nombre_usuario, oUsers.EVALUATION_ROLE) & "<br /><br /><br />" & strAssessmentHTML.Trim.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
            Me.Editor_eval_comments.Content = ""

            Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

            '****************************ADDING_VOTES**********************************
            Dim TotScore = Convert.ToDouble(oTA_EVALUATION_APP.EVALUATION_SCORE)
            TotScore = TotScore + scoreValues

            oTA_EVALUATION_APP.EVALUATION_SCORE = TotScore
            oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
            Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)


            '--*****************************************************************************************
            Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)
            Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()

            '--*****************************************************************************************


            If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened


                oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                Dim id_activity = Val(Me.lbl_id_ficha.Text)
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'APPLY / EVALUATION
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************

            End If

            Dim num As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm & " -> comments: " & Me.txt_comentarios.Text
                oTA_EVALUATION_APP_COMM.SCORE = scoreValues
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                    ''****************************CHECK THE SELECTED ONE APPLICATION******************************
                    ''******ON EVALUATION
                    Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED_SCORE", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                    If Percent_Obtained >= 100 Then '****Passed tough

                        oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                        oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Passed Status
                        oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                        oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                        oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                        oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                            'Save Evaluation APP Comment

                            oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                            oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Evaluation has been Passed!"
                            oTA_EVALUATION_APP_COMM.SCORE = 0
                            oTA_EVALUATION_APP_COMM.VOTE = 0
                            oTA_EVALUATION_APP_COMM.POINTS = 0
                            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                            oTA_EVALUATION_APP_COMM.COMM_BOL = False
                            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)


                            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status


                                If oRound.TOT_APP_SELECTED > 0 Then 'Not determined the number of applications selected

                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_SCORE", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)
                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    If VotingProgress >= 100 Then '******All Votes Made***************************
                                        CLOSE_ROUND_PROCC_SCORE(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, False)
                                    End If '******All Votes Made***************************
                                    '************************************CHECK THE VOTING PROGRESS********************************************************\

                                Else 'Isolated evaluation

                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_SCORE_IS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)
                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    If VotingProgress >= 100 Then '******All Votes Made***************************
                                        CLOSE_ROUND_PROCC_SCORE_IS(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True, idEVAL_app)
                                    End If '******All Votes Made***************************
                                    '************************************CHECK THE VOTING PROGRESS****************

                                End If

                            End If

                        End If

                    Else

                        If oRound.TOT_APP_SELECTED > 0 Then 'Not determined the number of applications selected

                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_SCORE", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)
                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            If VotingProgress >= 100 Then '******All Votes Made***************************
                                CLOSE_ROUND_PROCC_SCORE(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True)
                            End If '******All Votes Made***************************
                            '************************************CHECK THE VOTING PROGRESS********************************************************

                        Else 'Isolated evaluation

                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_SCORE_IS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)
                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            If VotingProgress >= 100 Then '******All Votes Made***************************
                                CLOSE_ROUND_PROCC_SCORE_IS(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True, idEVAL_app)
                            End If '******All Votes Made***************************
                            '************************************CHECK THE VOTING PROGRESS****************

                        End If


                        '****************************CHECK THE SELECTED ONE******************************

                    End If ' Percent_Obtained > 100


                End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

            End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            'End If

        End Using




    End Sub


    Sub F_VOTING(ByVal boolVoting As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            If boolVoting Then

                Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                Me.Editor_eval_comments.Content = ""

                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
                Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

                '****************************ADDING_VOTES**********************************
                Dim TotVotes = Convert.ToInt32(oTA_EVALUATION_APP.EVALUATION_VOTES)

                oTA_EVALUATION_APP.EVALUATION_VOTES = TotVotes + 1
                oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
                Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened
                    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                    '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                    Dim id_activity = Val(Me.lbl_id_ficha.Text)
                    cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'APPLY / EVALUATION
                    '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                End If

                Dim num As Integer
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

                    Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                    oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                    oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                    oTA_EVALUATION_APP_COMM.SCORE = 0
                    oTA_EVALUATION_APP_COMM.VOTE = 1
                    oTA_EVALUATION_APP_COMM.POINTS = 0
                    oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    oTA_EVALUATION_APP_COMM.COMM_BOL = False
                    'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                        '****************************CHECK THE SELECTED ONE APPLICATION******************************
                        '******ON EVALUATION
                        Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))



                        If Percent_Obtained >= 100 Then '****Accepted tough

                            oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                            oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted Status
                            oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                            oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                            oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                            oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                                'Save Evaluation APP Comment

                                oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Application Accepted!"
                                oTA_EVALUATION_APP_COMM.SCORE = 0
                                oTA_EVALUATION_APP_COMM.VOTE = 0
                                oTA_EVALUATION_APP_COMM.POINTS = 0
                                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                                    '************************************CHECK THE VOTING PROGRESS********************************************************
                                    If VotingProgress >= 100 Then '******All Votes Made***************************


                                        CLOSE_ROUND_PROCC(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, False)


                                    End If '******All Votes Made***************************
                                    '************************************CHECK THE VOTING PROGRESS********************************************************

                                End If

                            End If

                        Else

                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                            '***********INCLUDE THE UNTIED************
                            '************************************CHECK THE VOTING PROGRESS********************************************************
                            If VotingProgress >= 100 Then '******All Votes Made***************************

                                CLOSE_ROUND_PROCC(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True)

                            End If '******All Votes Made***************************
                            '************************************CHECK THE VOTING PROGRESS********************************************************

                            '****************************CHECK THE SELECTED ONE******************************

                        End If ' Percent_Obtained > 100

                    End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

                End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            End If

        End Using


    End Sub


    Sub F_UNTIED_REVIEW(ByVal boolVoting As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            'If boolVoting Then

            Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
            Me.Editor_eval_comments.Content = ""

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

            Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

            '****************************ADDING_VOTES**********************************

            Dim TotVotes = Convert.ToInt32(oTA_EVALUATION_APP.EVALUATION_VOTES)
            Dim TotUnTied = Convert.ToDecimal(oTA_EVALUATION_APP.EVALUATION_UNTIED)

            Dim UnTiedElection = If(boolVoting, 1, -1)
            'oTA_EVALUATION_APP.EVALUATION_VOTES = TotVotes + 1
            oTA_EVALUATION_APP.EVALUATION_UNTIED = TotUnTied + UnTiedElection
            oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
            Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

            'If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened
            '    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
            'End If

            Dim num As Integer

            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then


                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = UnTiedElection
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                    '****************************CHECK THE SELECTED ONE APPLICATION******************************
                    '******ON EVALUATION
                    ' Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                    '****************ON TIED**********

                    'If Percent_Obtained >= 100 Then '****Accepted tough

                    '    oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                    '    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted Status
                    '    oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                    '    oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                    '    oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                    '    oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                    '    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                    '        'Save Evaluation APP Comment

                    '        oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                    '        oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                    '        oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                    '        oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                    '        oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Application Accepted!"
                    '        oTA_EVALUATION_APP_COMM.SCORE = 0
                    '        oTA_EVALUATION_APP_COMM.VOTE = 0
                    '        oTA_EVALUATION_APP_COMM.POINTS = 0
                    '        oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    '        oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    '        oTA_EVALUATION_APP_COMM.COMM_BOL = False

                    '        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                    '            '************************************CHECK THE VOTING PROGRESS********************************************************
                    '            Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                    '            '************************************CHECK THE VOTING PROGRESS********************************************************
                    '            If VotingProgress >= 100 Then '******All Votes Made***************************


                    '                CLOSE_ROUND_PROCC(oTA_EVALUATION_APP.ID_EVALUATION_ROUND)


                    '            End If '******All Votes Made***************************
                    '            '************************************CHECK THE VOTING PROGRESS********************************************************

                    '        End If

                    '    End If

                    'Else

                    '************************************CHECK THE VOTING PROGRESS********************************************************
                    Dim VotingProgress_untied = CHECK_EVAL("VOTE_PROGRESS_REV_UN", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                    '***********INCLUDE THE UNTIED************
                    '************************************CHECK THE VOTING PROGRESS********************************************************
                    If VotingProgress_untied >= 100 Then '******All Votes Made***************************

                        CLOSE_ROUND_PROCC_REV_UN(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True)

                    End If '******All Votes Made***************************
                    '************************************CHECK THE VOTING PROGRESS********************************************************

                    '****************************CHECK THE SELECTED ONE******************************

                    ' End If ' Percent_Obtained > 100

                End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

            End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            'End If

        End Using


    End Sub


    Sub F_UNTIED_VOTING(ByVal boolVoting As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            If boolVoting Then

                Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                Me.Editor_eval_comments.Content = ""

                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
                Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

                '****************************ADDING_VOTES**********************************
                Dim TotVotes = Convert.ToInt32(oTA_EVALUATION_APP.EVALUATION_VOTES)
                Dim TotUnTied = Convert.ToDecimal(oTA_EVALUATION_APP.EVALUATION_UNTIED)
                'oTA_EVALUATION_APP.EVALUATION_VOTES = TotVotes + 1
                oTA_EVALUATION_APP.EVALUATION_UNTIED = TotUnTied + 1
                oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
                Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                'If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened
                '    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                'End If

                Dim num As Integer
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

                    Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                    oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                    oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                    oTA_EVALUATION_APP_COMM.SCORE = 0
                    oTA_EVALUATION_APP_COMM.VOTE = 1
                    oTA_EVALUATION_APP_COMM.POINTS = 0
                    oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    oTA_EVALUATION_APP_COMM.COMM_BOL = False
                    'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)
                    oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                        '****************************CHECK THE SELECTED ONE APPLICATION******************************
                        '******ON EVALUATION
                        ' Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                        '****************ON TIED**********

                        'If Percent_Obtained >= 100 Then '****Accepted tough

                        '    oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                        '    oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted Status
                        '    oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                        '    oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                        '    oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                        '    oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                        '    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                        '        'Save Evaluation APP Comment

                        '        oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                        '        oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                        '        oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                        '        oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                        '        oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Application Accepted!"
                        '        oTA_EVALUATION_APP_COMM.SCORE = 0
                        '        oTA_EVALUATION_APP_COMM.VOTE = 0
                        '        oTA_EVALUATION_APP_COMM.POINTS = 0
                        '        oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        '        oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                        '        oTA_EVALUATION_APP_COMM.COMM_BOL = False

                        '        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                        '            '************************************CHECK THE VOTING PROGRESS********************************************************
                        '            Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                        '            '************************************CHECK THE VOTING PROGRESS********************************************************
                        '            If VotingProgress >= 100 Then '******All Votes Made***************************


                        '                CLOSE_ROUND_PROCC(oTA_EVALUATION_APP.ID_EVALUATION_ROUND)


                        '            End If '******All Votes Made***************************
                        '            '************************************CHECK THE VOTING PROGRESS********************************************************

                        '        End If

                        '    End If

                        'Else

                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        Dim VotingProgress_untied = CHECK_EVAL("VOTE_PROGRESS_UN", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))

                        '***********INCLUDE THE UNTIED************
                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        If VotingProgress_untied >= 100 Then '******All Votes Made***************************

                            CLOSE_ROUND_PROCC_UN(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True)

                        End If '******All Votes Made***************************
                        '************************************CHECK THE VOTING PROGRESS********************************************************

                        '****************************CHECK THE SELECTED ONE******************************

                        ' End If ' Percent_Obtained > 100

                    End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

                End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            End If

        End Using


    End Sub


    Sub CLOSE_ROUND_PROCC_UN(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)


            'Double checked on EStatus
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idVOTING_TYPE = oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED

            Dim IDeva_APP_StatusACC = Get_STATUS(2, idVOTING_TYPE, True)
            Dim TotAcceppted = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusACC).Count()
            Dim IDeva_APP_StatusTIED = Get_STATUS(3, idVOTING_TYPE)
            Dim TotTIED = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).Count()
            Dim TotPEnding = TOTselected - TotAcceppted

            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            'Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND


            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)


            'If TOTselected >= TOTapply Then 'All Applications should be selected

            '    If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

            '        idROUND += 1 'Create Next Steps from the rest of the applications
            '        For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = Get_STATUS(3, idVOTING_TYPE)).ToList()

            '            Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

            '        Next

            '    Else 'None other steps

            '        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage


            '    End If
            '    '**********************************************************************************************
            'Else 'WE need to choice the ones we are gonna move to other stage***********************************
            '    '***********************************************************************************************

            Dim dtTies As New DataTable
            dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
            dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
            dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
            dtTies.Columns.Add("EVALUATION_VOTES", GetType(Double))
            dtTies.Columns.Add("TIED", GetType(Boolean))
            dtTies.Columns.Add("PASS", GetType(Boolean))

            Dim iRows As Integer = 0
            '        For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.OrderByDescending(Function(p) p.EVALUATION_VOTES + p.EVALUATION_UNTIED).ToList()
            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).OrderByDescending(Function(p) p.EVALUATION_VOTES + p.EVALUATION_UNTIED).ToList()

                If ValGotIt = -10000 Then

                    ValGotIt = Ieval.EVALUATION_UNTIED
                    BndTIED = False

                Else

                    If ValGotIt = Ieval.EVALUATION_UNTIED Then 'Tied

                        BndTIED = True
                        ValGotIt = Ieval.EVALUATION_UNTIED
                        ' If iRows = 1 Then 'Set the previous to Tied too
                        dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                        'End If

                    Else

                        BndTIED = False
                        ValGotIt = Ieval.EVALUATION_UNTIED

                    End If

                End If

                dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                    Ieval.ID_EVALUATION_APP,
                                    Ieval.ID_APPLY_APP,
                                    Ieval.EVALUATION_UNTIED,
                                    BndTIED,
                                    False)

                iRows += 1

            Next

            '*************************************CHECK DE TIED ONES**************************

            iRows = 0
            Dim i As Integer = 0
            Dim Tot_to_ADD As Integer = 0

            If dtTies.Rows.Item(TotPEnding - 1).Item("TIED") = True And dtTies.Rows.Item(TotPEnding).Item("TIED") = True Then

                If TotPEnding = 1 Then 'Start From Up

                    i = 0
                    For Each dtRow As DataRow In dtTies.Rows

                        If dtRow("TIED") Then
                            dtRow("PASS") = False
                            i += 1
                            If i > TOTselected Then
                                Tot_to_ADD += 1
                            End If
                        Else
                            dtRow("PASS") = True
                        End If

                    Next

                Else 'Split Search from the middle


                    Dim InterrupTed As Boolean = False
                    For i = (TotPEnding) To (TotTIED - 1)

                        If dtTies.Rows.Item(i).Item("TIED") Then
                            If Not InterrupTed Then
                                dtTies.Rows.Item(i).Item("PASS") = False
                                Tot_to_ADD += 1
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                            End If
                            'Tot_to_ADD += 1
                        Else
                            dtTies.Rows.Item(i).Item("PASS") = True
                            InterrupTed = True
                        End If

                    Next

                    InterrupTed = False
                    For i = (TotPEnding - 1) To 0 Step -1
                        If dtTies.Rows.Item(i).Item("TIED") Then
                            If Not InterrupTed Then
                                dtTies.Rows.Item(i).Item("PASS") = False
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                            End If
                        Else
                            dtTies.Rows.Item(i).Item("PASS") = True
                            InterrupTed = True
                        End If
                    Next

                End If

                Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            Else 'Pass everthihng the tied ones does not affect

                For i = 0 To (TotPEnding - 1)
                    dtTies.Rows.Item(i).Item("PASS") = True
                Next

            End If
            '*************************************CHECK DE TIED ONES**************************

            idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
            If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                idROUND += 1 'Create Next Steps from the rest of the applications
                For i = 0 To ((TotPEnding + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                    'If dtTies.Rows.Item(i).Item("PASS") = True Then
                    '    SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    'Else 'Set Tied Status
                    '    SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    'End If
                    If dtTies.Rows.Item(i).Item("PASS") = True Then
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        End If
                        Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    Else 'Set Tied Status
                        If dtTies.Rows.Item(i).Item("TIED") Then
                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Else
                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        End If
                    End If

                Next

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                '********************The Remaining ones to change status to Dismissed***********
                For i = (TotPEnding + Tot_to_ADD) To TOTapply - 1

                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    'If dtTies.Rows.Item(i).Item("PASS") = True Then
                    '    If bndUpdateSTATUS Then
                    '        SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    End If
                    '    Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    'Else 'Set Tied Status
                    '    If dtTies.Rows.Item(i).Item("TIED") Then
                    '        SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    Else
                    '        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    End If
                    'End If

                Next



            Else 'None other steps

                'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                'idROUND += 1 'This Step should be pointed to next stage
                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                For i = 0 To ((TotPEnding + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                    If dtTies.Rows.Item(i).Item("PASS") = True Then

                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        End If
                        Generate_AWARD_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        '****************SHOULD POINTED TO NEXT STEP*****************************************
                        'Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        '****************SHOULD POINTED TO NEXT STEP*****************************************
                    Else 'Set Tied Status

                        If dtTies.Rows.Item(i).Item("TIED") Then
                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Else
                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        End If

                    End If

                Next

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                '********************The Remaining ones to change status to Dismissed***********
                For i = (TotPEnding + Tot_to_ADD) To TOTapply - 1

                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                Next


            End If


            'End If

        End Using

    End Sub


    Sub CLOSE_ROUND_PROCC_REV_UN(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idVOTING_TYPE = oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED

            Dim IDeva_APP_StatusACC = Get_STATUS(2, idVOTING_TYPE, True)
            Dim TotAcceppted = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusACC).Count()
            Dim IDeva_APP_StatusTIED = Get_STATUS(3, idVOTING_TYPE)
            Dim TotTIED = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).Count()
            Dim TotPEnding = TOTselected - TotAcceppted

            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND


            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)


            'If TOTselected >= TOTapply Then 'All Applications should be selected

            '    If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

            '        idROUND += 1 'Create Next Steps from the rest of the applications
            '        For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = Get_STATUS(3, idVOTING_TYPE)).ToList()

            '            Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

            '        Next

            '    Else 'None other steps

            '        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage


            '    End If
            '    '**********************************************************************************************
            'Else 'WE need to choice the ones we are gonna move to other stage***********************************
            '    '***********************************************************************************************

            Dim dtTies As New DataTable
            dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
            dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
            dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
            dtTies.Columns.Add("EVALUATION_VOTES", GetType(Double))
            dtTies.Columns.Add("TIED", GetType(Boolean))
            dtTies.Columns.Add("PASS", GetType(Boolean))

            Dim iRows As Integer = 0

            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP_STATUS = IDeva_APP_StatusTIED).ToList()

                If ValGotIt = -10000 Then

                    ValGotIt = Ieval.EVALUATION_UNTIED
                    BndTIED = False

                Else

                    If ValGotIt = Ieval.EVALUATION_UNTIED Then 'Tied

                        BndTIED = True
                        ValGotIt = Ieval.EVALUATION_UNTIED
                        ' If iRows = 1 Then 'Set the previous to Tied too
                        dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                        'End If

                    Else

                        BndTIED = False
                        ValGotIt = Ieval.EVALUATION_UNTIED

                    End If

                End If

                dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                    Ieval.ID_EVALUATION_APP,
                                    Ieval.ID_APPLY_APP,
                                    Ieval.EVALUATION_UNTIED,
                                    BndTIED,
                                    False)

                iRows += 1

            Next

            '*************************************CHECK DE TIED ONES**************************

            iRows = 0
            Dim i As Integer = 0
            Dim Tot_to_ADD As Integer = 0

            If dtTies.Rows.Item(TotPEnding - 1).Item("TIED") = True And dtTies.Rows.Item(TotPEnding).Item("TIED") = True Then

                If TotPEnding = 1 Then 'Start From Up

                    i = 0
                    For Each dtRow As DataRow In dtTies.Rows

                        If dtRow("TIED") Then
                            dtRow("PASS") = False
                            i += 1
                            If i > TOTselected Then
                                Tot_to_ADD += 1
                            End If
                        Else
                            dtRow("PASS") = True
                        End If

                    Next

                Else 'Split Search from the middle


                    Dim InterrupTed As Boolean = False
                    For i = (TotPEnding) To (TotTIED - 1)

                        If dtTies.Rows.Item(i).Item("TIED") Then
                            If Not InterrupTed Then
                                dtTies.Rows.Item(i).Item("PASS") = False
                                Tot_to_ADD += 1
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                            End If
                            ' Tot_to_ADD += 1
                        Else
                            dtTies.Rows.Item(i).Item("PASS") = True
                            InterrupTed = True
                        End If

                    Next

                    InterrupTed = False
                    For i = (TotPEnding - 1) To 0 Step -1
                        If dtTies.Rows.Item(i).Item("TIED") Then
                            If Not InterrupTed Then
                                dtTies.Rows.Item(i).Item("PASS") = False
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                            End If
                        Else
                            dtTies.Rows.Item(i).Item("PASS") = True
                            InterrupTed = True
                        End If
                    Next

                End If

                Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            Else 'Pass everthihng the tied ones does not affect

                For i = 0 To (TotPEnding - 1)
                    dtTies.Rows.Item(i).Item("PASS") = True
                Next

            End If
            '*************************************CHECK DE TIED ONES**************************

            idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
            If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                idROUND += 1 'Create Next Steps from the rest of the applications
                For i = 0 To ((TotPEnding + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                    'If dtTies.Rows.Item(i).Item("PASS") = True Then
                    '    SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    'Else 'Set Tied Status
                    '    SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    'End If
                    If dtTies.Rows.Item(i).Item("PASS") = True Then
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        End If
                        Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    Else 'Set Tied Status
                        If dtTies.Rows.Item(i).Item("TIED") Then
                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Else
                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        End If
                    End If

                Next

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                '********************The Remaining ones to change status to Dismissed***********
                For i = (TotPEnding + Tot_to_ADD) To TOTapply - 1

                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    'If dtTies.Rows.Item(i).Item("PASS") = True Then
                    '    If bndUpdateSTATUS Then
                    '        SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    End If
                    '    Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                    'Else 'Set Tied Status
                    '    If dtTies.Rows.Item(i).Item("TIED") Then
                    '        SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    Else
                    '        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    '    End If
                    'End If

                Next



            Else 'None other steps

                'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                idROUND += 1 'This Step should be pointed to next stage
                For i = 0 To ((TotPEnding + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                    If dtTies.Rows.Item(i).Item("PASS") = True Then

                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        End If
                        '****************SHOULD POINTED TO NEXT STEP*****************************************
                        'Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        '****************SHOULD POINTED TO NEXT STEP*****************************************
                    Else 'Set Tied Status

                        If dtTies.Rows.Item(i).Item("TIED") Then
                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Else
                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        End If

                    End If

                Next

                '********************The Remaining ones to change status to Dismissed***********
                For i = (TotPEnding + Tot_to_ADD) To TOTapply - 1

                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                Next


            End If


            'End If

        End Using

    End Sub

    Sub CLOSE_ROUND_PROCC(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Acepted
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)

            If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected

                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()
                        Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                    Next

                Else 'None other steps

                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        'Next STAGE TO PROCCED WIHT THE APPLICATION
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(Ieval.ID_EVALUATION_APP)
                        End If
                        Generate_AWARD_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************

                    Next


                End If
                '**********************************************************************************************
            Else 'WE need to choice the ones we are gonna move to other stage***********************************
                '***********************************************************************************************

                Dim dtTies As New DataTable
                dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
                dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
                dtTies.Columns.Add("EVALUATION_VOTES", GetType(Double))
                dtTies.Columns.Add("TIED", GetType(Boolean))
                dtTies.Columns.Add("PASS", GetType(Boolean))

                Dim iRows As Integer = 0
                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.OrderByDescending(Function(p) p.EVALUATION_VOTES + p.EVALUATION_UNTIED).ToList()

                    If ValGotIt = -10000 Then

                        ValGotIt = Ieval.EVALUATION_VOTES
                        BndTIED = False

                    Else

                        If ValGotIt = Ieval.EVALUATION_VOTES Then 'Tied

                            BndTIED = True
                            ValGotIt = Ieval.EVALUATION_VOTES
                            ' If iRows = 1 Then 'Set the previous to Tied too
                            dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                            'End If

                        Else

                            BndTIED = False
                            ValGotIt = Ieval.EVALUATION_VOTES

                        End If

                    End If

                    dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                    Ieval.ID_EVALUATION_APP,
                                    Ieval.ID_APPLY_APP,
                                    Ieval.EVALUATION_VOTES,
                                    BndTIED,
                                    False)

                    iRows += 1

                Next

                '*************************************CHECK DE TIED ONES**************************
                iRows = 0
                Dim i As Integer = 0
                Dim Tot_to_ADD As Integer = 0
                '******************************CHECK THIS ONE*********************************

                If dtTies.Rows.Item(TOTselected - 1).Item("TIED") = True And dtTies.Rows.Item(TOTselected).Item("TIED") = True Then


                    If TOTselected = 1 Then 'Start From Up

                        i = 0
                        For Each dtRow As DataRow In dtTies.Rows

                            If dtRow("TIED") Then
                                dtRow("PASS") = False
                                i += 1
                                If i > TOTselected Then
                                    Tot_to_ADD += 1
                                End If
                            Else
                                dtRow("PASS") = True
                            End If

                        Next

                    Else 'Split Search from the middle


                        Dim InterrupTed As Boolean = False
                        For i = (TOTselected) To (TOTapply - 1)

                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                    Tot_to_ADD += 1
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                                ' Tot_to_ADD += 1
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If

                        Next

                        InterrupTed = False
                        For i = (TOTselected - 1) To 0 Step -1
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                        Next

                    End If

                    Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                    Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                    oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                    dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                Else 'Pass everthihng the tied ones does not affect

                    For i = 0 To (TOTselected - 1)
                        dtTies.Rows.Item(i).Item("PASS") = True
                    Next

                End If
                '*************************************CHECK DE TIED ONES**************************

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones in case there are some ties

                        If dtTies.Rows.Item(i).Item("PASS") = True Then
                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next



                Else 'None other steps

                    'Doing some stuff to finish the overall evaluation to move the selected on to the other stage

                    'idROUND += 1 'This Step should be pointed to next stage
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then

                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                            'Next STAGE TO PROCCED WIHT THE APPLICATION
                            Generate_AWARD_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************

                        Else 'Set Tied Status

                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If

                        End If

                    Next


                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next


                End If


            End If


        End Using

    End Sub


    Sub CLOSE_ROUND_PROCC_REV(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            'For TYPE = POINTS we need to change to o.EVALUATION_VOTES
            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Acepted
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)


            If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected

                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

                    Next

                Else 'None other steps


                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        'Next STAGE TO PROCCED WIHT THE APPLICATION
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(Ieval.ID_EVALUATION_APP)
                        End If
                        Generate_AWARD_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************

                    Next

                End If
                '**********************************************************************************************
            Else 'WE need to choice the ones we are gonna move to other stage***********************************
                '***********************************************************************************************

                Dim dtTies As New DataTable
                dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
                dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
                dtTies.Columns.Add("EVALUATION_VOTES", GetType(Double)) 'TYPE = POINTS SHOULD CHANGE
                dtTies.Columns.Add("TIED", GetType(Boolean))
                dtTies.Columns.Add("PASS", GetType(Boolean))

                Dim iRows As Integer = 0
                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                    If ValGotIt = -10000 Then

                        ValGotIt = Ieval.EVALUATION_VOTES
                        BndTIED = False

                    Else

                        If ValGotIt = Ieval.EVALUATION_VOTES Then 'Tied

                            BndTIED = True
                            ValGotIt = Ieval.EVALUATION_VOTES
                            ' If iRows = 1 Then 'Set the previous to Tied too
                            dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                            'End If

                        Else

                            BndTIED = False
                            ValGotIt = Ieval.EVALUATION_VOTES

                        End If

                    End If

                    dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                    Ieval.ID_EVALUATION_APP,
                                    Ieval.ID_APPLY_APP,
                                    Ieval.EVALUATION_VOTES,
                                    BndTIED,
                                    False)

                    iRows += 1

                Next

                '*************************************CHECK DE TIED ONES**************************
                iRows = 0
                Dim i As Integer = 0
                Dim Tot_to_ADD As Integer = 0
                If dtTies.Rows.Item(TOTselected - 1).Item("TIED") = True And dtTies.Rows.Item(TOTselected).Item("TIED") = True Then

                    If TOTselected = 1 Then 'Start From Up

                        i = 0
                        For Each dtRow As DataRow In dtTies.Rows

                            If dtRow("TIED") Then
                                dtRow("PASS") = False
                                i += 1
                                If i > TOTselected Then
                                    Tot_to_ADD += 1
                                End If
                            Else
                                dtRow("PASS") = True
                            End If

                        Next

                    Else 'Split Search from the middle


                        Dim InterrupTed As Boolean = False
                        For i = (TOTselected) To (TOTapply - 1)

                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                    Tot_to_ADD += 1
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                            ' Tot_to_ADD += 1
                        Next

                        InterrupTed = False
                        For i = (TOTselected - 1) To 0 Step -1
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                        Next

                    End If

                    Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                    Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                    oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                    dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                Else 'Pass everthihng the tied ones does not affect

                    For i = 0 To (TOTselected - 1)
                        dtTies.Rows.Item(i).Item("PASS") = True
                    Next

                End If
                '*************************************CHECK DE TIED ONES**************************

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then
                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next

                Else 'None other steps

                    'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                    idROUND += 1 'This Step should be pointed to next stage
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then
                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                            'Next STAGE TO PROCCED WIHT THE APPLICATION
                            Generate_AWARD_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************

                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next


                End If


            End If


        End Using

    End Sub


    'Sub CLOSE_ROUND_PROCC_REV(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

    '    Using dbEntities As New dbRMS_JIEntities

    '        'For TYPE = POINTS we need to change to o.EVALUATION_VOTES
    '        Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
    '        Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
    '        Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And p.ID_EVALUATION_APP_STATUS = idEvalStatus).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

    '        Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
    '        Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
    '        Dim ValGotIt As Double = -10000
    '        Dim BndTIED As Boolean = False

    '        Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
    '        Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

    '        Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
    '                            Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
    '                            Group oRound By oRound.ID_APPLY_EVALUATION Into Group
    '                            Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

    '        Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
    '                            Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
    '                            Group oRound By oRound.ID_APPLY_EVALUATION Into Group
    '                            Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)


    '        If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected

    '            If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

    '                idROUND += 1 'Create Next Steps from the rest of the applications
    '                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

    '                    Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

    '                Next

    '            Else 'None other steps

    '                'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
    '                '****************SHOULD POINTED TO NEXT STAGE*****************************************
    '                'Next STAGE TO PROCCED WIHT THE APPLICATION
    '                '****************SHOULD POINTED TO NEXT STAGE*****************************************

    '            End If
    '            '**********************************************************************************************
    '        Else 'WE need to choice the ones we are gonna move to other stage***********************************
    '            '***********************************************************************************************

    '            Dim dtTies As New DataTable
    '            dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
    '            dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
    '            dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
    '            dtTies.Columns.Add("EVALUATION_VOTES", GetType(Double)) 'TYPE = POINTS SHOULD CHANGE
    '            dtTies.Columns.Add("TIED", GetType(Boolean))
    '            dtTies.Columns.Add("PASS", GetType(Boolean))

    '            Dim iRows As Integer = 0
    '            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

    '                If ValGotIt = -10000 Then

    '                    ValGotIt = Ieval.EVALUATION_VOTES
    '                    BndTIED = False

    '                Else

    '                    If ValGotIt = Ieval.EVALUATION_VOTES Then 'Tied

    '                        BndTIED = True
    '                        ValGotIt = Ieval.EVALUATION_VOTES
    '                        ' If iRows = 1 Then 'Set the previous to Tied too
    '                        dtTies.Rows.Item(iRows - 1).Item("TIED") = True
    '                        'End If

    '                    Else

    '                        BndTIED = False
    '                        ValGotIt = Ieval.EVALUATION_VOTES

    '                    End If

    '                End If

    '                dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
    '                                Ieval.ID_EVALUATION_APP,
    '                                Ieval.ID_APPLY_APP,
    '                                Ieval.EVALUATION_VOTES,
    '                                BndTIED,
    '                                False)

    '                iRows += 1

    '            Next

    '            '*************************************CHECK DE TIED ONES**************************
    '            iRows = 0
    '            Dim i As Integer = 0
    '            Dim Tot_to_ADD As Integer = 0
    '            If dtTies.Rows.Item(TOTselected - 1).Item("TIED") = True And dtTies.Rows.Item(TOTselected).Item("TIED") = True Then

    '                If TOTselected = 1 Then 'Start From Up

    '                    i = 0
    '                    For Each dtRow As DataRow In dtTies.Rows

    '                        If dtRow("TIED") Then
    '                            dtRow("PASS") = False
    '                            i += 1
    '                            If i > TOTselected Then
    '                                Tot_to_ADD += 1
    '                            End If
    '                        Else
    '                            dtRow("PASS") = True
    '                        End If

    '                    Next

    '                Else 'Split Search from the middle


    '                    Dim InterrupTed As Boolean = False
    '                    For i = (TOTselected) To (TOTapply - 1)

    '                        If dtTies.Rows.Item(i).Item("TIED") Then
    '                            If Not InterrupTed Then
    '                                dtTies.Rows.Item(i).Item("PASS") = False
    '                                Tot_to_ADD += 1
    '                            Else
    '                                dtTies.Rows.Item(i).Item("PASS") = True
    '                            End If
    '                        Else
    '                            dtTies.Rows.Item(i).Item("PASS") = True
    '                            InterrupTed = True
    '                        End If
    '                        ' Tot_to_ADD += 1
    '                    Next

    '                    InterrupTed = False
    '                    For i = (TOTselected - 1) To 0 Step -1
    '                        If dtTies.Rows.Item(i).Item("TIED") Then
    '                            If Not InterrupTed Then
    '                                dtTies.Rows.Item(i).Item("PASS") = False
    '                            Else
    '                                dtTies.Rows.Item(i).Item("PASS") = True
    '                            End If
    '                        Else
    '                            dtTies.Rows.Item(i).Item("PASS") = True
    '                            InterrupTed = True
    '                        End If
    '                    Next

    '                End If

    '                Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
    '                Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
    '                oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
    '                dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
    '                dbEntities.SaveChanges()

    '            Else 'Pass everthihng the tied ones does not affect

    '                For i = 0 To (TOTselected - 1)
    '                    dtTies.Rows.Item(i).Item("PASS") = True
    '                Next

    '            End If
    '            '*************************************CHECK DE TIED ONES**************************

    '            idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
    '            If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

    '                idROUND += 1 'Create Next Steps from the rest of the applications
    '                For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

    '                    If dtTies.Rows.Item(i).Item("PASS") = True Then
    '                        If bndUpdateSTATUS Then
    '                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                        End If
    '                        Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
    '                    Else 'Set Tied Status
    '                        If dtTies.Rows.Item(i).Item("TIED") Then
    '                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                        Else
    '                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
    '                        End If
    '                    End If

    '                Next

    '                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
    '                '********************The Remaining ones to change status to Dismissed***********
    '                For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

    '                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

    '                Next

    '            Else 'None other steps

    '                'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
    '                idROUND += 1 'This Step should be pointed to next stage
    '                For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

    '                    If dtTies.Rows.Item(i).Item("PASS") = True Then
    '                        If bndUpdateSTATUS Then
    '                            SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                        End If
    '                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
    '                        'Next STAGE TO PROCCED WIHT THE APPLICATION
    '                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
    '                    Else 'Set Tied Status
    '                        If dtTies.Rows.Item(i).Item("TIED") Then
    '                            SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                        Else
    '                            SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                            Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
    '                        End If
    '                    End If

    '                Next

    '                '********************The Remaining ones to change status to Dismissed***********
    '                For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

    '                    SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
    '                    Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

    '                Next


    '            End If


    '        End If


    '    End Using

    'End Sub



    Sub CLOSE_ROUND_PROCC_POINTS(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            'For TYPE = POINTS we need to change to o.EVALUATION_VOTES
            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)


            If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected

                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

                    Next

                Else 'None other steps

                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        'Next STAGE TO PROCCED WIHT THE APPLICATION
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(Ieval.ID_EVALUATION_APP)
                        End If
                        Generate_AWARD_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************

                    Next

                End If
                '**********************************************************************************************
            Else 'WE need to choice the ones we are gonna move to other stage***********************************
                '***********************************************************************************************

                Dim dtTies As New DataTable
                dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
                dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
                dtTies.Columns.Add("EVALUATION_POINTS", GetType(Double)) 'TYPE = POINTS SHOULD CHANGE
                dtTies.Columns.Add("TIED", GetType(Boolean))
                dtTies.Columns.Add("PASS", GetType(Boolean))

                Dim iRows As Integer = 0
                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                    If ValGotIt = -10000 Then

                        ValGotIt = Ieval.EVALUATION_SCORE
                        BndTIED = False

                    Else

                        If ValGotIt = Ieval.EVALUATION_SCORE Then 'Tied

                            BndTIED = True
                            ValGotIt = Ieval.EVALUATION_SCORE
                            ' If iRows = 1 Then 'Set the previous to Tied too
                            dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                            'End If

                        Else

                            BndTIED = False
                            ValGotIt = Ieval.EVALUATION_SCORE

                        End If

                    End If

                    dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                    Ieval.ID_EVALUATION_APP,
                                    Ieval.ID_APPLY_APP,
                                    Ieval.EVALUATION_SCORE,
                                    BndTIED,
                                    False)

                    iRows += 1

                Next

                '*************************************CHECK DE TIED ONES**************************
                iRows = 0
                Dim i As Integer = 0
                Dim Tot_to_ADD As Integer = 0
                If dtTies.Rows.Item(TOTselected - 1).Item("TIED") = True And dtTies.Rows.Item(TOTselected).Item("TIED") = True Then

                    If TOTselected = 1 Then 'Start From Up

                        i = 0
                        For Each dtRow As DataRow In dtTies.Rows

                            If dtRow("TIED") Then
                                dtRow("PASS") = False
                                i += 1
                                If i > TOTselected Then
                                    Tot_to_ADD += 1
                                End If
                            Else
                                dtRow("PASS") = True
                            End If

                        Next

                    Else 'Split Search from the middle


                        Dim InterrupTed As Boolean = False
                        For i = (TOTselected) To (TOTapply - 1)

                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                    Tot_to_ADD += 1
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                            ' Tot_to_ADD += 1
                        Next

                        InterrupTed = False
                        For i = (TOTselected - 1) To 0 Step -1
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                        Next

                    End If

                    Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                    Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                    oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                    dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                Else 'Pass everthihng the tied ones does not affect

                    For i = 0 To (TOTselected - 1)
                        dtTies.Rows.Item(i).Item("PASS") = True
                    Next

                End If
                '*************************************CHECK DE TIED ONES**************************

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then
                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next

                Else 'None other steps

                    'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                    idROUND += 1 'This Step should be pointed to next stage
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then

                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                            'Next STAGE TO PROCCED WIHT THE APPLICATION
                            Generate_AWARD_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************

                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next


                End If


            End If


        End Using

    End Sub







    Sub CLOSE_ROUND_PROCC_SCORE(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            GenerateSummary_Eval(idEVAL_Round, True)

            'For TYPE = POINTS we need to change to o.EVALUATION_VOTES
            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)



            If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected


                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

                    Next

                Else 'None other steps

                    For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        'Next STAGE TO PROCCED WIHT THE APPLICATION
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(Ieval.ID_EVALUATION_APP)
                        End If
                        Generate_AWARD_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************

                        '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************
                        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                        '**********************************CHEMONICS PROCESSS*******************************************************
                        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1018, regionalizacionCulture)
                        '**********************************CHEMONCS PROCESSS*******************************************************
                        cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(Ieval.ID_SOLICITATION_APP, Ieval.ID_EVALUATION_APP_STATUS, Ieval.ID_EVALUATION_ROUND, Ieval.ID_EVALUATION_APP) '' ACCEPTED"
                        'NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer
                        '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************

                    Next

                End If
                '**********************************************************************************************
            Else 'WE need to choice the ones we are gonna move to other stage***********************************
                '***********************************************************************************************

                Dim dtTies As New DataTable
                dtTies.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_APP", GetType(Integer))
                dtTies.Columns.Add("ID_APPLY_APP", GetType(Integer))
                dtTies.Columns.Add("ID_SOLICITATION_APP", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_APP_STATUS", GetType(Integer))
                dtTies.Columns.Add("ID_EVALUATION_ROUND", GetType(Integer))
                dtTies.Columns.Add("EVALUATION_POINTS", GetType(Double)) 'TYPE = POINTS SHOULD CHANGE
                dtTies.Columns.Add("TIED", GetType(Boolean))
                dtTies.Columns.Add("PASS", GetType(Boolean))

                Dim iRows As Integer = 0
                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.OrderByDescending(Function(O) (O.EVALUATION_SCORE + O.EVALUATION_UNTIED)).ToList()

                    If ValGotIt = -10000 Then

                        ValGotIt = Ieval.EVALUATION_SCORE
                        BndTIED = False

                    Else

                        If ValGotIt = Ieval.EVALUATION_SCORE Then 'Tied

                            BndTIED = True
                            ValGotIt = Ieval.EVALUATION_SCORE
                            ' If iRows = 1 Then 'Set the previous to Tied too
                            dtTies.Rows.Item(iRows - 1).Item("TIED") = True
                            'End If

                        Else

                            BndTIED = False
                            ValGotIt = Ieval.EVALUATION_SCORE

                        End If

                    End If

                    dtTies.Rows.Add(Ieval.ID_ACTIVITY_SOLICITATION,
                                Ieval.ID_EVALUATION_APP,
                                Ieval.ID_APPLY_APP,
                                Ieval.ID_SOLICITATION_APP,
                                Ieval.ID_EVALUATION_APP_STATUS,
                                Ieval.ID_EVALUATION_ROUND,
                                Ieval.EVALUATION_SCORE,
                                BndTIED,
                                False)

                    iRows += 1

                Next

                '*************************************CHECK DE TIED ONES**************************
                iRows = 0
                Dim i As Integer = 0
                Dim Tot_to_ADD As Integer = 0
                If dtTies.Rows.Item(TOTselected - 1).Item("TIED") = True And dtTies.Rows.Item(TOTselected).Item("TIED") = True Then

                    If TOTselected = 1 Then 'Start From Up

                        i = 0
                        For Each dtRow As DataRow In dtTies.Rows

                            If dtRow("TIED") Then
                                dtRow("PASS") = False
                                i += 1
                                If i > TOTselected Then
                                    Tot_to_ADD += 1
                                End If
                            Else
                                dtRow("PASS") = True
                            End If

                        Next

                    Else 'Split Search from the middle


                        Dim InterrupTed As Boolean = False
                        For i = (TOTselected) To (TOTapply - 1)

                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                    Tot_to_ADD += 1
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                            ' Tot_to_ADD += 1
                        Next

                        InterrupTed = False
                        For i = (TOTselected - 1) To 0 Step -1
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                If Not InterrupTed Then
                                    dtTies.Rows.Item(i).Item("PASS") = False
                                Else
                                    dtTies.Rows.Item(i).Item("PASS") = True
                                End If
                            Else
                                dtTies.Rows.Item(i).Item("PASS") = True
                                InterrupTed = True
                            End If
                        Next

                    End If

                    Dim oTA_EVALUATION_R = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                    Dim TIES As Integer = oTA_EVALUATION_R.TIED_TOTAL
                    oTA_EVALUATION_R.TIED_TOTAL = TIES + 1
                    dbEntities.Entry(oTA_EVALUATION_R).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                Else 'Pass everthihng the tied ones does not affect

                    For i = 0 To (TOTselected - 1)
                        dtTies.Rows.Item(i).Item("PASS") = True
                    Next

                End If
                '*************************************CHECK DE TIED ONES**************************

                idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

                If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                    idROUND += 1 'Create Next Steps from the rest of the applications
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then
                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            Generate_EVAL_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                            '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************
                            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                            '**********************************CHEMONICS PROCESSS*******************************************************
                            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1018, regionalizacionCulture)
                            '**********************************CHEMONCS PROCESSS*******************************************************
                            cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(dtTies.Rows.Item(i).Item("ID_SOLICITATION_APP"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP_STATUS"), dtTies.Rows.Item(i).Item("ID_EVALUATION_ROUND"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP")) '' ACCEPTED"
                            'NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer
                            '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************



                        Else 'Set Tied Status
                            If dtTies.Rows.Item(i).Item("TIED") Then
                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            Else
                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            End If
                        End If

                    Next

                    idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND
                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND + 1, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next

                Else 'None other steps

                    'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                    'idROUND += 1 'This Step should be pointed to next stage
                    'Testing from here
                    For i = 0 To ((TOTselected + Tot_to_ADD) - 1)   'Tot_to_ADD Additional ones

                        If dtTies.Rows.Item(i).Item("PASS") = True Then

                            If bndUpdateSTATUS Then
                                SET_ACCEPT_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                            End If
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                            'Next STAGE TO PROCCED WIHT THE APPLICATION

                            Generate_AWARD_APP(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))
                            '****************SHOULD POINTED TO NEXT STAGE*****************************************

                            '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************
                            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                            '**********************************CHEMONICS PROCESSS*******************************************************
                            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1018, regionalizacionCulture)
                            '**********************************CHEMONCS PROCESSS*******************************************************
                            cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(dtTies.Rows.Item(i).Item("ID_SOLICITATION_APP"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP_STATUS"), dtTies.Rows.Item(i).Item("ID_EVALUATION_ROUND"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP")) '' ACCEPTED"
                            'NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer
                            '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************

                            '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        Else 'Set Tied Status

                            If dtTies.Rows.Item(i).Item("TIED") Then

                                SET_TIED_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))

                            Else

                                SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                                Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                            End If

                        End If

                    Next

                    '********************The Remaining ones to change status to Dismissed***********
                    For i = (TOTselected + Tot_to_ADD) To TOTapply - 1

                        SET_DISMISS_STATUS(dtTies.Rows.Item(i).Item("ID_EVALUATION_APP"))
                        Generate_EVAL_APP_DISMISSED(idAPPevaluation, idROUND, dtTies.Rows.Item(i).Item("ID_APPLY_APP"))

                    Next


                End If


            End If


        End Using

    End Sub




    Sub CLOSE_ROUND_PROCC_SCORE_IS(ByVal idEVAL_Round As Integer, ByVal bndUpdateSTATUS As Boolean, ByVal idEVAL_APP As Integer)

        Using dbEntities As New dbRMS_JIEntities

            GenerateSummary_Eval_IS(idEVAL_Round, idEVAL_APP, True)

            'For TYPE = POINTS we need to change to o.EVALUATION_VOTES
            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted

            'Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            'Dim TOTselected = oTA_EVALUATION_ROUNDS.TOT_APP_SELECTED
            Dim ValGotIt As Double = -10000
            Dim BndTIED As Boolean = False

            'Dim TOTapply = oVW_TA_ACT_SOL_APP_EVAL.Count()
            Dim idROUND = oTA_EVALUATION_ROUNDS.ID_ROUND

            Dim oID_ROUND_min = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, minID_ROUND = Group.Min(Function(m) m.ID_ROUND)

            Dim oID_ROUND_max = From oRound In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound.ID_APPLY_EVALUATION = idAPPevaluation
                                Group oRound By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)

            'If (TOTselected >= TOTapply) Or (TOTapply = 1) Then 'All Applications should be selected

            If idROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                idROUND += 1 'Create Next Steps from the rest of the applications
                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                    Generate_EVAL_APP(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)

                Next


            Else 'None other steps


                '*********************check if the score reach the minimal for award******************************


                For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                    ' Dim scoreT As Double = If(Ieval.MEMBERS > 0, Math.Round(Ieval.EVALUATION_SCORE / Ieval.MEMBERS, 3, MidpointRounding.AwayFromZero), 0)

                    If Ieval.EVALUATION_SCORE >= Ieval.TARGET_TO_OBTAIN Then

                        'Doing some stuff to finish the overall evaluation to move the selected on to the other stage
                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        'Next STAGE TO PROCCED WIHT THE APPLICATION
                        If bndUpdateSTATUS Then
                            SET_ACCEPT_STATUS(Ieval.ID_EVALUATION_APP)
                        End If

                        '****************************THIS SHOULD BE SELECTEC FOR MANY ACTIVITIES-AWARDS*********************************
                        '****************************************************************************************************
                        Generate_AWARD_APP_IS(idAPPevaluation, idROUND, Ieval.ID_APPLY_APP)
                        '****************************THIS SHOULD BE SELECTEC FOR MANY ACTIVITIES-AWARDS*********************************
                        '****************************************************************************************************

                        '****************SHOULD POINTED TO NEXT STAGE*****************************************
                        '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************
                        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                        '**********************************CHEMONICS PROCESSS*******************************************************
                        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1018, regionalizacionCulture)
                        '**********************************CHEMONCS PROCESSS*******************************************************
                        cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(Ieval.ID_SOLICITATION_APP, Ieval.ID_EVALUATION_APP_STATUS, Ieval.ID_EVALUATION_ROUND, Ieval.ID_EVALUATION_APP) '' ACCEPTED"
                        'NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer
                        '*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************

                    Else

                        SET_DISMISS_STATUS(Ieval.ID_EVALUATION_APP)

                    End If

                Next

            End If

            '**********************************************************************************************
            ' End If


        End Using

    End Sub

    Public Sub GenerateSummary_Eval(ByVal idEVAL_Round As Integer, Optional UpdateScore As Boolean = False)


        Using dbEntities As New dbRMS_JIEntities


            Dim strRowMED_Tot_3 As String = "<tr style='background-color: #2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

            Dim strRowMED_Tot_4 As String = "<tr style='border-bottom-color:#ee7108; border:1px dotted #ee7108;  font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#2ea18e; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"




            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim oTA_EVALUATION_APP_COMM As TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

            Dim idPrograma As Integer = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            Dim tbl_res As DataTable = New DataTable
            Dim tbl_res_2 As DataTable = New DataTable

            '"width:100%"
            Dim strHTML_MainF As String = "<div class='box-body table-responsive no-padding'> {0} </div>"
            Dim strHTML_2 As String = ""

            Dim strHTML_Table As String = "<table class='table table-hover'>{0}</table>"
            Dim strRow_MAIN As String = "<tr>
                                            <td style='background-color:#2ea18e; font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px; padding:10px; width:25%'>
                                               {0}
                                            </td>
                                            <td style='width:75%'>  
                                              {1}  
                                            </td>
                                        </tr>"

            Dim strRowTOT_MAIN As String = ""

            strHTML_2 &= "<table class='table table-hover' style='' >
                                        <tr  style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;' >
                                          <th rowspan='2' style='padding:5px;' >No</th>
                                          <th rowspan='2' style='padding:5px;' >Evaluator</th>
                                          {0}
                                          <th rowspan='2'style='padding:5px;' >TOTAL SCORING</th>                                         
                                        </tr>"

            Dim strHTML_2B As String = ""

            Dim strHTML_Head2 As String = "<tr style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;'>
                                              {0}                                            
                                          </tr>"

            Dim strHTML_Head2B As String = ""

            Dim strRow_2 As String = "<tr  style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>
                                          <th style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0}</th> 
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1}</td>
                                            {2}
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{3:N4}</td>                                         
                                      </tr>"

            Dim strRow_2B As String = "<td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0:N3}</td><td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1:N3}</td>"


            Dim strRowMED_Tot_2 As String = "<tr style='font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size:small; color:#000000; height:20px;' > 
                                                  <td  colspan='{1}' style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;' >Total Scoring {0}</td>                                                                                                                                                   
                                                  <td  style='background-color:#2ea18e; padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108; color:#FFFFFF;' >{2:N3}</td>
                                             </tr>"

            Dim TotThemes As Integer = 0
            Dim idAPP_ As Integer = oVW_TA_ACT_SOL_APP_EVAL.FirstOrDefault.ID_APPLY_APP
            tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(idAPP_)
            'TotThemes = tbl_res.Rows.Count

            Dim strName As String = ""
            Dim strNamePIV As String = ""

            For Each dtRow As DataRow In tbl_res.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(idAPP_)
            ' TotThemes += tbl_res_2.Rows.Count

            strName = ""
            For Each dtRow As DataRow In tbl_res_2.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            strHTML_2 = String.Format(strHTML_2, strHTML_2B)
            strHTML_Head2 = String.Format(strHTML_Head2, strHTML_Head2B)

            strHTML_2 &= strHTML_Head2

            Dim strRowTot_2 As String = ""
            Dim strCols As String = ""

            Dim N As Integer = 1
            Dim TotScore As Decimal = 0
            Dim TotScoreALL As Decimal = 0
            Dim TotValueScore As Decimal = 0
            Dim TotEvaluators As Integer = 0
            Dim Remaining_Score As Decimal = 0

            strName = ""
            strNamePIV = ""

            '*******************************************************SUMMARY EVAL*******************************************************************************
            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(Ieval.ID_APPLY_APP)
                tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(Ieval.ID_APPLY_APP)

                N = 1
                TotScore = 0
                strRowTot_2 = ""
                Remaining_Score = 0

                For Each dtRow As DataRow In tbl_res.Rows

                    If strName.Trim.Length = 0 Then
                        strName = dtRow("nombre_usuario").ToString.Trim
                        strNamePIV = strName
                    Else
                        strNamePIV = dtRow("nombre_usuario").ToString.Trim
                    End If

                    If strName <> strNamePIV Then

                        For Each dtRowII As DataRow In tbl_res_2.Rows

                            If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then


                                If dtRowII("SCORING") >= dtRowII("SCORE_BASE") Then

                                    TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                                    strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                                    TotScore += TotValueScore
                                    Remaining_Score += TotValueScore

                                Else

                                    TotValueScore = Math.Round(0, 3, MidpointRounding.AwayFromZero)
                                    strCols &= String.Format(strRow_2B, 0, TotValueScore)
                                    TotScore += TotValueScore
                                    Remaining_Score += TotValueScore

                                End If

                            End If

                        Next

                        TotEvaluators += 1
                        strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                        TotScoreALL += TotScore
                        strName = strNamePIV
                        strCols = String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore = dtRow("TOT_SCORE") '***********SETTING The Total Score***********

                    Else

                        strCols &= String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore += dtRow("TOT_SCORE")

                    End If

                Next

                '*************************Last Columns***************************

                For Each dtRowII As DataRow In tbl_res_2.Rows

                    If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then

                        If dtRowII("SCORING") >= dtRowII("SCORE_BASE") Then

                            TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                            strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                            TotScore += TotValueScore
                            Remaining_Score += TotValueScore

                        Else

                            TotValueScore = Math.Round(0, 3, MidpointRounding.AwayFromZero)
                            strCols &= String.Format(strRow_2B, 0, TotValueScore)
                            TotScore += TotValueScore
                            Remaining_Score += TotValueScore

                        End If

                    End If

                Next

                TotEvaluators += 1
                strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                TotScoreALL += TotScore
                strName = ""
                strCols = ""

                '*************************Last Row***************************

                Dim strHTML_final As String = strHTML_2
                strHTML_final &= strRowTot_2
                strHTML_final &= String.Format(strRowMED_Tot_2, Ieval.ORGANIZATIONNAME, ((TotThemes * 2) + 2).ToString, (TotScoreALL / TotEvaluators))
                strHTML_final &= "</table>"


                Dim Tot_toReported As Double = TotScoreALL
                'Dim Tot_toRemaining As Double = Remaining_Score / TotEvaluators
                Dim Tot_toRemaining As Double = Remaining_Score

                '*********************RESET THE TOTAL BY APPLICATION********************
                TotScoreALL = 0
                TotEvaluators = 0
                '*********************RESET THE TOTAL BY APPLICATION********************

                Dim strREsult As String = String.Format(strHTML_MainF, strHTML_final).Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                strRowTOT_MAIN &= String.Format(strRow_MAIN, Ieval.ORGANIZATIONNAME, Strings.Replace(strHTML_final, "style=''", "style='width:100%'"))

                Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(Ieval.ID_EVALUATION_APP)

                oTA_EVALUATION_APP.EVALUATION_SCORE = Tot_toReported
                oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim num As Integer = 0
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, Ieval.ID_EVALUATION_APP), num) Then

                    oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '*********ACCEPTED COMMENT*********

                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = Ieval.ID_EVALUATION_APP
                    oTA_EVALUATION_APP_COMM.ROUND = Ieval.ID_ROUND
                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Ieval.ID_EVALUATION_APP_STATUS
                    oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "The Evaluation is completed, the results are shown below<br/><br/><br/>" & String.Format(strHTML_MainF, strHTML_final).Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                    oTA_EVALUATION_APP_COMM.SCORE = Tot_toRemaining
                    oTA_EVALUATION_APP_COMM.VOTE = 0
                    oTA_EVALUATION_APP_COMM.POINTS = 0
                    oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    oTA_EVALUATION_APP_COMM.COMM_BOL = False
                    oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving the summary for this application

                    End If

                End If

            Next
            '*******************************************************SUMMARY EVAL*******************************************************************************


            '*******************************************************SUMMARY EVAL*******************************************************************************

            Dim strFinalTable = String.Format(strHTML_MainF, String.Format(strHTML_Table, strRowTOT_MAIN))
            Dim a As String = strFinalTable

            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '*********ACCEPTED COMMENT*********

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = Ieval.ID_EVALUATION_APP
                oTA_EVALUATION_APP_COMM.ROUND = Ieval.ID_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Ieval.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "The evaluation summary <br/><br/><br/>" & strFinalTable.Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim num As Integer = 0
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving the total summary for this application

                End If

            Next


            '*******************************************************SUMMARY EVAL*******************************************************************************

            '***************************************SendEmailHere***************************************


        End Using

    End Sub


    Public Sub GenerateSummary_Eval_IS(ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer, Optional UpdateScore As Boolean = False)


        Using dbEntities As New dbRMS_JIEntities


            Dim strRowMED_Tot_3 As String = "<tr style='background-color: #2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

            Dim strRowMED_Tot_4 As String = "<tr style='border-bottom-color:#ee7108; border:1px dotted #ee7108;  font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#2ea18e; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"




            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted
            ' Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)
            Dim oTA_EVALUATION_APP_COMM As TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

            Dim idPrograma As Integer = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            Dim tbl_res As DataTable = New DataTable
            Dim tbl_res_2 As DataTable = New DataTable

            '"width:100%"
            Dim strHTML_MainF As String = "<div class='box-body table-responsive no-padding'> {0} </div>"
            Dim strHTML_2 As String = ""

            Dim strHTML_Table As String = "<table class='table table-hover'>{0}</table>"
            Dim strRow_MAIN As String = "<tr>
                                            <td style='background-color:#2ea18e; font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px; padding:10px; width:25%'>
                                               {0}
                                            </td>
                                            <td style='width:75%'>  
                                              {1}  
                                            </td>
                                        </tr>"

            Dim strRowTOT_MAIN As String = ""

            strHTML_2 &= "<table class='table table-hover' style='' >
                                        <tr  style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;' >
                                          <th rowspan='2' style='padding:5px;' >No</th>
                                          <th rowspan='2' style='padding:5px;' >Evaluator</th>
                                          {0}
                                          <th rowspan='2'style='padding:5px;' >TOTAL SCORING</th>                                         
                                        </tr>"

            Dim strHTML_2B As String = ""

            Dim strHTML_Head2 As String = "<tr style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;'>
                                              {0}                                            
                                          </tr>"

            Dim strHTML_Head2B As String = ""

            Dim strRow_2 As String = "<tr  style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>
                                          <th style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0}</th> 
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1}</td>
                                            {2}
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{3:N4}</td>                                         
                                      </tr>"

            Dim strRow_2B As String = "<td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0:N3}</td><td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1:N3}</td>"


            Dim strRowMED_Tot_2 As String = "<tr style='font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size:small; color:#000000; height:20px;' > 
                                                  <td  colspan='{1}' style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;' >Total Scoring {0}</td>                                                                                                                                                   
                                                  <td  style='background-color:#2ea18e; padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108; color:#FFFFFF;' >{2:N3}</td>
                                             </tr>"

            Dim TotThemes As Integer = 0
            Dim idAPP_ As Integer = oVW_TA_ACT_SOL_APP_EVAL.FirstOrDefault.ID_APPLY_APP
            tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(idAPP_)
            'TotThemes = tbl_res.Rows.Count

            Dim strName As String = ""
            Dim strNamePIV As String = ""

            For Each dtRow As DataRow In tbl_res.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(idAPP_)
            ' TotThemes += tbl_res_2.Rows.Count

            strName = ""
            For Each dtRow As DataRow In tbl_res_2.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            strHTML_2 = String.Format(strHTML_2, strHTML_2B)
            strHTML_Head2 = String.Format(strHTML_Head2, strHTML_Head2B)

            strHTML_2 &= strHTML_Head2

            Dim strRowTot_2 As String = ""
            Dim strCols As String = ""

            Dim N As Integer = 1
            Dim TotScore As Decimal = 0
            Dim TotScoreALL As Decimal = 0
            Dim TotValueScore As Decimal = 0
            Dim TotEvaluators As Integer = 0
            Dim Remaining_Score As Decimal = 0

            strName = ""
            strNamePIV = ""

            '*******************************************************SUMMARY EVAL*******************************************************************************
            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(Ieval.ID_APPLY_APP)
                tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(Ieval.ID_APPLY_APP)

                N = 1
                TotScore = 0
                strRowTot_2 = ""
                Remaining_Score = 0

                For Each dtRow As DataRow In tbl_res.Rows

                    If strName.Trim.Length = 0 Then
                        strName = dtRow("nombre_usuario").ToString.Trim
                        strNamePIV = strName
                    Else
                        strNamePIV = dtRow("nombre_usuario").ToString.Trim
                    End If

                    If strName <> strNamePIV Then

                        For Each dtRowII As DataRow In tbl_res_2.Rows

                            If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then


                                If dtRowII("SCORING") >= dtRowII("SCORE_BASE") Then

                                    TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                                    strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                                    TotScore += TotValueScore
                                    Remaining_Score += TotValueScore

                                Else

                                    TotValueScore = Math.Round(0, 3, MidpointRounding.AwayFromZero)
                                    strCols &= String.Format(strRow_2B, 0, TotValueScore)
                                    TotScore += TotValueScore
                                    Remaining_Score += TotValueScore

                                End If

                            End If

                        Next

                        TotEvaluators += 1
                        strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                        TotScoreALL += TotScore
                        strName = strNamePIV
                        strCols = String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore = dtRow("TOT_SCORE") '***********SETTING The Total Score***********

                    Else

                        strCols &= String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore += dtRow("TOT_SCORE")

                    End If

                Next

                '*************************Last Columns***************************

                For Each dtRowII As DataRow In tbl_res_2.Rows

                    If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then

                        If dtRowII("SCORING") >= dtRowII("SCORE_BASE") Then

                            TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                            strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                            TotScore += TotValueScore
                            Remaining_Score += TotValueScore

                        Else

                            TotValueScore = Math.Round(0, 3, MidpointRounding.AwayFromZero)
                            strCols &= String.Format(strRow_2B, 0, TotValueScore)
                            TotScore += TotValueScore
                            Remaining_Score += TotValueScore

                        End If

                    End If

                Next

                TotEvaluators += 1
                strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                TotScoreALL += TotScore
                strName = ""
                strCols = ""

                '*************************Last Row***************************

                Dim strHTML_final As String = strHTML_2
                strHTML_final &= strRowTot_2
                strHTML_final &= String.Format(strRowMED_Tot_2, Ieval.ORGANIZATIONNAME, ((TotThemes * 2) + 2).ToString, (TotScoreALL / TotEvaluators))
                strHTML_final &= "</table>"


                Dim Tot_toReported As Double = TotScoreALL
                'Dim Tot_toRemaining As Double = Remaining_Score / TotEvaluators
                Dim Tot_toRemaining As Double = Remaining_Score

                '*********************RESET THE TOTAL BY APPLICATION********************
                TotScoreALL = 0
                TotEvaluators = 0
                '*********************RESET THE TOTAL BY APPLICATION********************

                Dim strREsult As String = String.Format(strHTML_MainF, strHTML_final).Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                strRowTOT_MAIN &= String.Format(strRow_MAIN, Ieval.ORGANIZATIONNAME, Strings.Replace(strHTML_final, "style=''", "style='width:100%'"))

                Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(Ieval.ID_EVALUATION_APP)

                oTA_EVALUATION_APP.EVALUATION_SCORE = Tot_toReported
                oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim num As Integer = 0
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, Ieval.ID_EVALUATION_APP), num) Then

                    oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '*********ACCEPTED COMMENT*********

                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = Ieval.ID_EVALUATION_APP
                    oTA_EVALUATION_APP_COMM.ROUND = Ieval.ID_ROUND
                    oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Ieval.ID_EVALUATION_APP_STATUS
                    oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "The Evaluation is completed, the results are shown below<br/><br/><br/>" & String.Format(strHTML_MainF, strHTML_final).Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                    oTA_EVALUATION_APP_COMM.SCORE = Tot_toRemaining
                    oTA_EVALUATION_APP_COMM.VOTE = 0
                    oTA_EVALUATION_APP_COMM.POINTS = 0
                    oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                    oTA_EVALUATION_APP_COMM.COMM_BOL = False
                    oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving the summary for this application

                    End If

                End If

            Next
            '*******************************************************SUMMARY EVAL*******************************************************************************


            '*******************************************************SUMMARY EVAL*******************************************************************************

            Dim strFinalTable = String.Format(strHTML_MainF, String.Format(strHTML_Table, strRowTOT_MAIN))
            Dim a As String = strFinalTable

            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '*********ACCEPTED COMMENT*********

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = Ieval.ID_EVALUATION_APP
                oTA_EVALUATION_APP_COMM.ROUND = Ieval.ID_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Ieval.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "The evaluation summary <br/><br/><br/>" & strFinalTable.Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                Dim num As Integer = 0
                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving the total summary for this application

                End If

            Next


            '*******************************************************SUMMARY EVAL*******************************************************************************

            '***************************************SendEmailHere***************************************


        End Using

    End Sub



    Public Function GenerateSummary_Table(ByVal idEVAL_Round As Integer, Optional idEVAL_APP As Integer = 0) As String

        Using dbEntities As New dbRMS_JIEntities


            Dim strRowMED_Tot_3 As String = "<tr style='background-color: #2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

            Dim strRowMED_Tot_4 As String = "<tr style='border-bottom-color:#ee7108; border:1px dotted #ee7108;  font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#2ea18e; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"


            Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
            Dim idEvalStatus As Integer = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Ón Evaluation
            Dim idEvalStatus2 As Integer = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted

            Dim bndID As Integer = If(idEVAL_APP = 0, 1, 0)

            Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round And p.ID_EVALUATION_APP <> 0 _
                                                                                       And (p.ID_EVALUATION_APP_STATUS = idEvalStatus Or p.ID_EVALUATION_APP_STATUS = idEvalStatus2) _
                                                                                       And (p.ID_EVALUATION_APP = idEVAL_APP Or 1 = bndID)).OrderBy(Function(o) o.ID_APPLY_APP).ThenByDescending(Function(o) o.EVALUATION_VOTES)

            Dim idPrograma As Integer = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            Dim tbl_res As DataTable = New DataTable
            Dim tbl_res_2 As DataTable = New DataTable

            '"width:100%"
            Dim strHTML_MainF As String = "<div class='box-body table-responsive no-padding'> {0} </div>"
            Dim strHTML_2 As String = ""

            Dim strHTML_Table As String = "<table class='table table-hover'>{0}</table>"
            Dim strRow_MAIN As String = "<tr>
                                            <td style='background-color:#2ea18e; font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px; padding:10px; width:25%'>
                                               {0}
                                            </td>
                                            <td style='width:75%'>  
                                              {1}  
                                            </td>
                                        </tr>"

            Dim strRowTOT_MAIN As String = ""

            strHTML_2 &= "<table class='table table-hover' style='' >
                                        <tr  style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;' >
                                          <th rowspan='2' style='padding:5px;' >No</th>
                                          <th rowspan='2' style='padding:5px;' >Evaluator</th>
                                          {0}
                                          <th rowspan='2'style='padding:5px;' >TOTAL SCORING</th>                                         
                                        </tr>"

            Dim strHTML_2B As String = ""

            Dim strHTML_Head2 As String = "<tr style='background-color:#2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; text-align:center; height:20px;'>
                                              {0}                                            
                                          </tr>"

            Dim strHTML_Head2B As String = ""

            Dim strRow_2 As String = "<tr  style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>
                                          <th style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0}</th> 
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1}</td>
                                            {2}
                                          <td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{3:N4}</td>                                         
                                      </tr>"

            Dim strRow_2B As String = "<td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{0:N3}</td><td style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;'>{1:N3}</td>"


            Dim strRowMED_Tot_2 As String = "<tr style='font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size:small; color:#000000; height:20px;' > 
                                                  <td  colspan='{1}' style='padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108;' >Total Scoring {0}</td>                                                                                                                                                   
                                                  <td  style='background-color:#2ea18e; padding: 5px; border-bottom-color:#ee7108; border:1px dotted #ee7108; color:#FFFFFF;' >{2:N3}</td>
                                             </tr>"

            Dim TotThemes As Integer = 0
            Dim idAPP_ As Integer = oVW_TA_ACT_SOL_APP_EVAL.FirstOrDefault.ID_APPLY_APP
            tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(idAPP_)
            ' TotThemes = tbl_res.Rows.Count

            Dim strName As String = ""
            Dim strNamePIV As String = ""

            For Each dtRow As DataRow In tbl_res.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(idAPP_)
            'TotThemes += tbl_res_2.Rows.Count

            strName = ""
            For Each dtRow As DataRow In tbl_res_2.Rows

                If strName.Trim.Length = 0 Then
                    strName = dtRow("nombre_usuario").ToString.Trim
                    strNamePIV = strName
                Else
                    strNamePIV = dtRow("nombre_usuario").ToString.Trim
                End If

                If strName.Trim <> strNamePIV.Trim Then
                    Exit For
                Else
                    strHTML_2B &= String.Format("<th colspan='2' style='padding: 5px;' >{0}</th>", dtRow("theme_name"))
                    strHTML_Head2B &= String.Format("<th style='padding: 5px;'>Total Category</th><th style='padding: 5px;'>Total Score</th>")
                    TotThemes += 1
                End If

            Next

            strHTML_2 = String.Format(strHTML_2, strHTML_2B)
            strHTML_Head2 = String.Format(strHTML_Head2, strHTML_Head2B)

            strHTML_2 &= strHTML_Head2

            Dim strRowTot_2 As String = ""
            Dim strCols As String = ""

            Dim N As Integer = 1
            Dim TotScore As Decimal = 0
            Dim TotScoreALL As Decimal = 0
            Dim TotValueScore As Decimal = 0
            Dim TotEvaluators As Integer = 0
            Dim Remaining_Score As Decimal = 0

            strName = ""
            strNamePIV = ""
            Dim strREsult As String = ""

            '*******************************************************SUMMARY EVAL*******************************************************************************
            For Each Ieval In oVW_TA_ACT_SOL_APP_EVAL.ToList()

                tbl_res = cls_Solicitation.get_ACTIVITY_ANSWER_SCORE(Ieval.ID_APPLY_APP)
                tbl_res_2 = cls_Solicitation.get_ACTIVITY_ANSWER_VAL(Ieval.ID_APPLY_APP)

                N = 1
                TotScore = 0
                strRowTot_2 = ""
                Remaining_Score = 0

                For Each dtRow As DataRow In tbl_res.Rows

                    If strName.Trim.Length = 0 Then
                        strName = dtRow("nombre_usuario").ToString.Trim
                        strNamePIV = strName
                    Else
                        strNamePIV = dtRow("nombre_usuario").ToString.Trim
                    End If

                    If strName <> strNamePIV Then

                        For Each dtRowII As DataRow In tbl_res_2.Rows

                            If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then
                                TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                                strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                                TotScore += TotValueScore
                                Remaining_Score += TotValueScore
                            End If

                        Next

                        TotEvaluators += 1
                        strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                        TotScoreALL += TotScore
                        strName = strNamePIV
                        strCols = String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore = dtRow("TOT_SCORE") '***********SETTING The Total Score***********

                    Else

                        strCols &= String.Format(strRow_2B, dtRow("TOT_CAT"), dtRow("TOT_SCORE"))
                        TotScore += dtRow("TOT_SCORE")

                    End If

                Next

                '*************************Last Columns***************************

                For Each dtRowII As DataRow In tbl_res_2.Rows

                    If strName.Trim = dtRowII("nombre_usuario").ToString.Trim Then
                        TotValueScore = Math.Round(((dtRowII("measurement_answer_value_MIN") / dtRowII("measurement_answer_value")) * (dtRowII("percent_valueMTH") / 100)) * 100, 3, MidpointRounding.AwayFromZero)
                        strCols &= String.Format(strRow_2B, dtRowII("measurement_answer_value"), TotValueScore)
                        TotScore += TotValueScore
                        Remaining_Score += TotValueScore
                    End If

                Next

                TotEvaluators += 1
                strRowTot_2 &= String.Format(strRow_2, N.ToString, strName, strCols, TotScore)
                TotScoreALL += TotScore
                strName = ""
                strCols = ""

                '*************************Last Row***************************

                Dim strHTML_final As String = strHTML_2
                strHTML_final &= strRowTot_2
                strHTML_final &= String.Format(strRowMED_Tot_2, Ieval.ORGANIZATIONNAME, ((TotThemes * 2) + 2).ToString, (TotScoreALL / TotEvaluators))
                strHTML_final &= "</table>"


                Dim Tot_toReported As Double = TotScoreALL
                Dim Tot_toRemaining As Double = Remaining_Score

                '*********************RESET THE TOTAL BY APPLICATION********************
                TotScoreALL = 0
                TotEvaluators = 0
                '*********************RESET THE TOTAL BY APPLICATION********************

                strREsult = String.Format(strHTML_MainF, strHTML_final).Trim.Replace("  ", " ").ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                strRowTOT_MAIN &= String.Format(strRow_MAIN, Ieval.ORGANIZATIONNAME, Strings.Replace(strHTML_final, "style=''", "style='width:100%'"))


            Next
            '*******************************************************SUMMARY EVAL*******************************************************************************


            '*******************************************************SUMMARY EVAL*******************************************************************************

            Dim strFinalTable = String.Format(strHTML_MainF, String.Format(strHTML_Table, strRowTOT_MAIN))
            ' Dim a As String = strFinalTable


            If idEVAL_APP = 0 Then
                GenerateSummary_Table = strFinalTable
            Else
                GenerateSummary_Table = strREsult
            End If

            '*******************************************************SUMMARY EVAL*******************************************************************************

            '***************************************SendEmailHere***************************************


        End Using

    End Function

    'Sub SET_ACCEPT_STATUS(ByVal ID_EVAL_APP As Integer)

    '    Using dbEntities As New dbRMS_JIEntities

    '        'Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
    '        'Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

    '        Dim oEVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(ID_EVAL_APP)
    '        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

    '        Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oEVALUATION_APP.TA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

    '        Dim idSTATUS As Integer = Get_STATUS(2, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) 'second one / Negative
    '        Dim oStatus = dbEntities.TA_EVALUATION_APP_STATUS.Find(idSTATUS)

    '        oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) 'second one / Negative
    '        oEVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '        oEVALUATION_APP.FECHA_UPDATE = Date.UtcNow
    '        oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '        Dim idEVALapp As Integer
    '        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, ID_EVAL_APP), idEVALapp) Then

    '            Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

    '            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
    '            oTA_EVALUATION_APP_COMM.ROUND = oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND
    '            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oEVALUATION_APP.ID_EVALUATION_APP_STATUS
    '            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} For {1}, Is {2}.", oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS), oStatus.EVALUATION_APP_STATUS)
    '            oTA_EVALUATION_APP_COMM.SCORE = 0
    '            oTA_EVALUATION_APP_COMM.VOTE = 0
    '            oTA_EVALUATION_APP_COMM.POINTS = 0
    '            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
    '            oTA_EVALUATION_APP_COMM.COMM_BOL = False
    '            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

    '            cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

    '        End If

    '    End Using

    'End Sub


    Sub SET_DISMISS_STATUS(ByVal ID_EVAL_APP As Integer)

        Using dbEntities As New dbRMS_JIEntities

            'Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            'Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oEVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(ID_EVAL_APP)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oEVALUATION_APP.TA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim idSTATUS As Integer = Get_STATUS(2, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) 'second one / Negative
            Dim oStatus = dbEntities.TA_EVALUATION_APP_STATUS.Find(idSTATUS)

            oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) 'second one / Negative
            oEVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oEVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idEVALapp As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, ID_EVAL_APP), idEVALapp) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                oTA_EVALUATION_APP_COMM.ROUND = oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oEVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} For {1}, Is {2}.", oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS), oStatus.EVALUATION_APP_STATUS)
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            End If

        End Using

    End Sub


    Sub SET_TIED_STATUS(ByVal ID_EVAL_APP As Integer)

        Using dbEntities As New dbRMS_JIEntities

            'Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            'Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oEVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(ID_EVAL_APP)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oEVALUATION_APP.TA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'third one Status
            oEVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oEVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idEVALapp As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, ID_EVAL_APP), idEVALapp) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                oTA_EVALUATION_APP_COMM.ROUND = oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oEVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} For {1}, it will require an additional vote To choose the application.", oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS))
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            End If

        End Using

    End Sub


    Sub SET_ACCEPT_STATUS(ByVal ID_EVAL_APP As Integer)

        Using dbEntities As New dbRMS_JIEntities

            'Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            'Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oEVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(ID_EVAL_APP)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oEVALUATION_APP.TA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'third one Status
            oEVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oEVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idEVALapp As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, ID_EVAL_APP), idEVALapp) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                oTA_EVALUATION_APP_COMM.ROUND = oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oEVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Application Accepted For{1} In Evaluation ROUND #{0}.  ", oEVALUATION_APP.TA_EVALUATION_ROUNDS.ID_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS))
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            End If

        End Using

    End Sub

    Sub Generate_EVAL_APP(ByVal id_APP_evaluation As Integer, ByVal id_ROUND As Integer, ByVal ID_APPLYapp As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oTA_APPLY_APP = dbEntities.TA_APPLY_APP.Find(ID_APPLYapp)
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oTA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim oEVALUATION_APP = New TA_EVALUATION_APP

            oEVALUATION_APP.ID_EVALUATION_ROUND = idEVALround
            oEVALUATION_APP.ID_APPLY_APP = ID_APPLYapp
            oEVALUATION_APP.EVALUATION_START_DATE = Date.UtcNow
            oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oRound.ID_VOTING_TYPE) 'First Status
            oEVALUATION_APP.EVALUATION_SCORE = 0
            oEVALUATION_APP.EVALUATION_VOTES = 0
            oEVALUATION_APP.EVALUATION_UNTIED = 0
            oEVALUATION_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oEVALUATION_APP.FECHA_CREA = Date.UtcNow
            oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idEVALapp As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, 0), idEVALapp) Then

                'Save Evaluation Comment
                'Me.H_ID_EVALUATION_APP.Value = idEVALapp
                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                oTA_EVALUATION_APP_COMM.ROUND = id_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oEVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} opened For {1} ", id_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS))
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            End If


        End Using

    End Sub



    Sub Generate_AWARD_APP(ByVal id_APP_evaluation As Integer, ByVal id_ROUND As Integer, ByVal ID_APPLYapp As Integer)


        Using dbEntities As New dbRMS_JIEntities
            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oTA_APPLY_APP = dbEntities.TA_APPLY_APP.Find(ID_APPLYapp)
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oTA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim oTA_ACTIVITY_SOLICITATION = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSol.ID_ACTIVITY_SOLICITATION)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVALround And p.ID_APPLY_APP = ID_APPLYapp)

            ' Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY)
            Dim oTA_AWARDED_APP = New TA_AWARDED_APP

            oTA_AWARDED_APP.ID_APPLY_APP = ID_APPLYapp
            oTA_AWARDED_APP.ID_ACTIVITY = oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY
            oTA_AWARDED_APP.ID_ORGANIZATION_APP = oSol.ID_ORGANIZATION_APP
            oTA_AWARDED_APP.ID_EVALUATION_APP = oTA_EVALUATION_APP.FirstOrDefault.ID_EVALUATION_APP
            oTA_AWARDED_APP.ID_AWARD_STATUS = 1 'Generated
            oTA_AWARDED_APP.AWARD_CODE = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.codigo_SAPME

            oTA_AWARDED_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_AWARDED_APP.FECHA_CREA = Date.UtcNow
            oTA_AWARDED_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            oTA_AWARDED_APP.TOTAL_AMOUNT = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.costo_total_proyecto
            oTA_AWARDED_APP.LEAVERAGED_AMOUNT = 0
            oTA_AWARDED_APP.ID_SUB_MECANISMO = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.id_sub_mecanismo
            oTA_AWARDED_APP.EXCHANGE_RATE = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.tasa_cambio
            oTA_AWARDED_APP.ID_BUDGET = 4 'Default Budget

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim currency = dbEntities.t_programa_currency.FirstOrDefault(Function(p) p.id_programa = idPrograma And p.set_default = True)
            oTA_AWARDED_APP.id_programa_currency = currency.id_programa_currency
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAWARDED As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_APP(oTA_AWARDED_APP, 0), idAWARDED) Then

                'Set to awarded
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY, 5)   'EVALUATION / AWARDED / SELECTED


                '********************CREATE ACTIVITY FOR THE CURRENT AWARD Based on the Main Activity**************************************

                Dim oPro = dbEntities.TA_ACTIVITY.Find(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY)
                Dim oProSUB = dbEntities.TA_ACTIVITY_SUBREGION.Where(Function(p) p.id_activity = oPro.id_activity)


                Dim oTA_AWARDED_ACTIVITY = New TA_AWARDED_ACTIVITY

                'ID_AWARDED_ACTIVITY
                oTA_AWARDED_ACTIVITY.ID_AWARDED_APP = idAWARDED
                oTA_AWARDED_ACTIVITY.id_subregion = oPro.id_subregion
                oTA_AWARDED_ACTIVITY.ID_ORGANIZATION_APP = oSol.ID_ORGANIZATION_APP
                oTA_AWARDED_ACTIVITY.id_componente = oPro.id_componente
                oTA_AWARDED_ACTIVITY.ID_ACTIVITY_STATUS = oPro.ID_ACTIVITY_STATUS
                oTA_AWARDED_ACTIVITY.id_periodo = oPro.id_periodo
                oTA_AWARDED_ACTIVITY.nombre_proyecto = oPro.nombre_proyecto
                oTA_AWARDED_ACTIVITY.area_intervencion = oPro.area_intervencion
                oTA_AWARDED_ACTIVITY.codigo_ficha_AID = oPro.codigo_ficha_AID
                oTA_AWARDED_ACTIVITY.codigo_RFA = oPro.codigo_RFA
                oTA_AWARDED_ACTIVITY.codigo_SAPME = oPro.codigo_SAPME
                oTA_AWARDED_ACTIVITY.codigo_MONITOR = oPro.codigo_MONITOR
                oTA_AWARDED_ACTIVITY.codigo_convenio = oPro.codigo_convenio
                oTA_AWARDED_ACTIVITY.numero_acta_aprobacion = oPro.numero_acta_aprobacion
                oTA_AWARDED_ACTIVITY.fecha_inicio_proyecto = oPro.fecha_inicio_proyecto
                oTA_AWARDED_ACTIVITY.fecha_fin_proyecto = oPro.fecha_fin_proyecto
                oTA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT = oPro.OBLIGATED_AMOUNT
                oTA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT_LOC = oPro.OBLIGATED_AMOUNT_LOC
                oTA_AWARDED_ACTIVITY.costo_total_proyecto = oTA_APPLY_APP.APPLY_AMOUNT  'oPro.costo_total_proyecto
                oTA_AWARDED_ACTIVITY.costo_total_proyecto_LOC = oTA_APPLY_APP.APPLY_AMOUNT_LOC ' oPro.costo_total_proyecto_LOC
                oTA_AWARDED_ACTIVITY.tasa_cambio = oPro.tasa_cambio
                oTA_AWARDED_ACTIVITY.observaciones = oPro.observaciones
                oTA_AWARDED_ACTIVITY.aportes_actualizados = oPro.aportes_actualizados
                oTA_AWARDED_ACTIVITY.idContratoME = oPro.idContratoME
                oTA_AWARDED_ACTIVITY.ActualizacionReciente = oPro.ActualizacionReciente
                oTA_AWARDED_ACTIVITY.datecreated = Date.UtcNow
                oTA_AWARDED_ACTIVITY.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oTA_AWARDED_ACTIVITY.id_usuario_update = oPro.
                'oTA_AWARDED_ACTIVITY.dateUpdate = oPro.
                oTA_AWARDED_ACTIVITY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                oTA_AWARDED_ACTIVITY.id_district = oPro.id_district
                oTA_AWARDED_ACTIVITY.id_sub_mecanismo = oPro.id_sub_mecanismo
                oTA_AWARDED_ACTIVITY.id_documento = oPro.id_documento
                oTA_AWARDED_ACTIVITY.id_usuario_responsable = oPro.id_usuario_responsable
                oTA_AWARDED_ACTIVITY.id_ficha_padre = oPro.id_ficha_padre
                oTA_AWARDED_ACTIVITY.id_programa = oPro.id_programa

                Dim idAWARDED_ACT As Integer
                If Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_ACTIVITY(oTA_AWARDED_ACTIVITY, 0), idAWARDED_ACT) Then


                    For Each dataITEM In oProSUB.ToList()

                        '**************************************** save subregion HERE******************************
                        Dim oTA_AWARDED_ACTIVITY_SUBREGION = New TA_AWARDED_ACTIVITY_SUBREGION

                        oTA_AWARDED_ACTIVITY_SUBREGION.ID_AWARDED_ACTIVITY = idAWARDED_ACT
                        oTA_AWARDED_ACTIVITY_SUBREGION.id_subregion = dataITEM.id_subregion
                        oTA_AWARDED_ACTIVITY_SUBREGION.nivel_cobertura = dataITEM.nivel_cobertura

                        Dim idAWARDED_ACT_SUB As Integer
                        If Not Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_ACTIVITY_SUB(oTA_AWARDED_ACTIVITY_SUBREGION, 0), idAWARDED_ACT_SUB) Then

                            Exit For

                        End If

                        '**************************************** save subregion HERE******************************

                    Next


                End If


                '********************CREATE ACTIVITY FOR THE CURRENT AWARD**************************************



            End If


        End Using

    End Sub


    ' Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY)

    Sub Generate_AWARD_APP_IS(ByVal id_APP_evaluation As Integer, ByVal id_ROUND As Integer, ByVal ID_APPLYapp As Integer)


        Using dbEntities As New dbRMS_JIEntities

            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oTA_APPLY_APP = dbEntities.TA_APPLY_APP.Find(ID_APPLYapp)
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oTA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim oTA_ACTIVITY_SOLICITATION = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSol.ID_ACTIVITY_SOLICITATION)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVALround And p.ID_APPLY_APP = ID_APPLYapp)

            ' Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY)

            Dim oTA_AWARDED_APP = New TA_AWARDED_APP
            oTA_AWARDED_APP.ID_APPLY_APP = ID_APPLYapp
            oTA_AWARDED_APP.ID_ACTIVITY = oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY
            oTA_AWARDED_APP.ID_ORGANIZATION_APP = oSol.ID_ORGANIZATION_APP
            oTA_AWARDED_APP.ID_EVALUATION_APP = oTA_EVALUATION_APP.FirstOrDefault.ID_EVALUATION_APP
            oTA_AWARDED_APP.ID_AWARD_STATUS = 1 'Generated
            oTA_AWARDED_APP.AWARD_CODE = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.codigo_SAPME

            oTA_AWARDED_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_AWARDED_APP.FECHA_CREA = Date.UtcNow
            oTA_AWARDED_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            'oTA_AWARDED_APP.TOTAL_AMOUNT = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.costo_total_proyecto
            oTA_AWARDED_APP.TOTAL_AMOUNT = oTA_APPLY_APP.APPLY_AMOUNT
            oTA_AWARDED_APP.LEAVERAGED_AMOUNT = 0
            oTA_AWARDED_APP.ID_SUB_MECANISMO = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.id_sub_mecanismo
            'oTA_AWARDED_APP.EXCHANGE_RATE = oTA_ACTIVITY_SOLICITATION.TA_ACTIVITY.tasa_cambio
            oTA_AWARDED_APP.EXCHANGE_RATE = oTA_APPLY_APP.APPLY_EXCHANGE_RATE
            oTA_AWARDED_APP.ID_BUDGET = 4 'Default Budget


            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim currency = dbEntities.t_programa_currency.FirstOrDefault(Function(p) p.id_programa = idPrograma And p.set_default = True)
            oTA_AWARDED_APP.id_programa_currency = currency.id_programa_currency
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAWARDED As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_APP(oTA_AWARDED_APP, 0), idAWARDED) Then

                'Set to awarded
                'should  this status change once there is no more application to accept??
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY, 5)   'EVALUATION / AWARDED / SELECTED

                '********************CREATE ACTIVITY FOR THE CURRENT AWARD Based on the Main Activity**************************************

                Dim oPro = dbEntities.TA_ACTIVITY.Find(oTA_ACTIVITY_SOLICITATION.ID_ACTIVITY)
                Dim oProSUB = dbEntities.TA_ACTIVITY_SUBREGION.Where(Function(p) p.id_activity = oPro.id_activity)


                Dim oTA_AWARDED_ACTIVITY = New TA_AWARDED_ACTIVITY

                'ID_AWARDED_ACTIVITY
                oTA_AWARDED_ACTIVITY.ID_AWARDED_APP = idAWARDED
                oTA_AWARDED_ACTIVITY.id_subregion = oPro.id_subregion
                oTA_AWARDED_ACTIVITY.ID_ORGANIZATION_APP = oSol.ID_ORGANIZATION_APP
                oTA_AWARDED_ACTIVITY.id_componente = oPro.id_componente
                oTA_AWARDED_ACTIVITY.ID_ACTIVITY_STATUS = oPro.ID_ACTIVITY_STATUS
                oTA_AWARDED_ACTIVITY.id_periodo = oPro.id_periodo
                oTA_AWARDED_ACTIVITY.nombre_proyecto = oPro.nombre_proyecto
                oTA_AWARDED_ACTIVITY.area_intervencion = oPro.area_intervencion
                oTA_AWARDED_ACTIVITY.codigo_ficha_AID = oPro.codigo_ficha_AID
                oTA_AWARDED_ACTIVITY.codigo_RFA = oPro.codigo_RFA
                oTA_AWARDED_ACTIVITY.codigo_SAPME = oPro.codigo_SAPME
                oTA_AWARDED_ACTIVITY.codigo_MONITOR = oPro.codigo_MONITOR
                oTA_AWARDED_ACTIVITY.codigo_convenio = oPro.codigo_convenio
                oTA_AWARDED_ACTIVITY.numero_acta_aprobacion = oPro.numero_acta_aprobacion
                oTA_AWARDED_ACTIVITY.fecha_inicio_proyecto = oPro.fecha_inicio_proyecto
                oTA_AWARDED_ACTIVITY.fecha_fin_proyecto = oPro.fecha_fin_proyecto
                oTA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT = oPro.OBLIGATED_AMOUNT
                oTA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT_LOC = oPro.OBLIGATED_AMOUNT_LOC
                oTA_AWARDED_ACTIVITY.costo_total_proyecto = oTA_APPLY_APP.APPLY_AMOUNT  'oPro.costo_total_proyecto
                oTA_AWARDED_ACTIVITY.costo_total_proyecto_LOC = oTA_APPLY_APP.APPLY_AMOUNT_LOC ' oPro.costo_total_proyecto_LOC
                oTA_AWARDED_ACTIVITY.tasa_cambio = oPro.tasa_cambio
                oTA_AWARDED_ACTIVITY.observaciones = oPro.observaciones
                oTA_AWARDED_ACTIVITY.aportes_actualizados = oPro.aportes_actualizados
                oTA_AWARDED_ACTIVITY.idContratoME = oPro.idContratoME
                oTA_AWARDED_ACTIVITY.ActualizacionReciente = oPro.ActualizacionReciente
                oTA_AWARDED_ACTIVITY.datecreated = Date.UtcNow
                oTA_AWARDED_ACTIVITY.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oTA_AWARDED_ACTIVITY.id_usuario_update = oPro.
                'oTA_AWARDED_ACTIVITY.dateUpdate = oPro.
                oTA_AWARDED_ACTIVITY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                oTA_AWARDED_ACTIVITY.id_district = oPro.id_district
                oTA_AWARDED_ACTIVITY.id_sub_mecanismo = oPro.id_sub_mecanismo
                oTA_AWARDED_ACTIVITY.id_documento = oPro.id_documento
                oTA_AWARDED_ACTIVITY.id_usuario_responsable = oPro.id_usuario_responsable
                oTA_AWARDED_ACTIVITY.id_ficha_padre = oPro.id_ficha_padre
                oTA_AWARDED_ACTIVITY.id_programa = oPro.id_programa

                Dim idAWARDED_ACT As Integer
                If Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_ACTIVITY(oTA_AWARDED_ACTIVITY, 0), idAWARDED_ACT) Then


                    For Each dataITEM In oProSUB.ToList()

                        '**************************************** save subregion HERE******************************
                        Dim oTA_AWARDED_ACTIVITY_SUBREGION = New TA_AWARDED_ACTIVITY_SUBREGION

                        oTA_AWARDED_ACTIVITY_SUBREGION.ID_AWARDED_ACTIVITY = idAWARDED_ACT
                        oTA_AWARDED_ACTIVITY_SUBREGION.id_subregion = dataITEM.id_subregion
                        oTA_AWARDED_ACTIVITY_SUBREGION.nivel_cobertura = dataITEM.nivel_cobertura

                        Dim idAWARDED_ACT_SUB As Integer
                        If Not Integer.TryParse(cls_Solicitation.Save_TA_AWARDED_ACTIVITY_SUB(oTA_AWARDED_ACTIVITY_SUBREGION, 0), idAWARDED_ACT_SUB) Then

                            Exit For

                        End If

                        '**************************************** save subregion HERE******************************

                    Next


                End If


                '********************CREATE ACTIVITY FOR THE CURRENT AWARD**************************************


            End If


        End Using

    End Sub


    Sub Generate_EVAL_APP_DISMISSED(ByVal id_APP_evaluation As Integer, ByVal id_ROUND As Integer, ByVal ID_APPLYapp As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oTA_APPLY_APP = dbEntities.TA_APPLY_APP.Find(ID_APPLYapp)
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oTA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim oID_ROUND_max = From oRound_ In dbEntities.TA_EVALUATION_ROUNDS
                                Where oRound_.ID_APPLY_EVALUATION = id_APP_evaluation
                                Group oRound_ By oRound.ID_APPLY_EVALUATION Into Group
                                Select ID_APPLY_EVALUATION, maxID_ROUND = Group.Max(Function(m) m.ID_ROUND)

            ''id_ROUND += 1 'Validate this one
            If id_ROUND < oID_ROUND_max.FirstOrDefault.maxID_ROUND Then

                For i = id_ROUND To oID_ROUND_max.FirstOrDefault.maxID_ROUND

                    Generate_EVAL_APP_DISMISSED_(id_APP_evaluation, i, ID_APPLYapp)

                Next

            Else 'None other steps

                ' Generate_EVAL_APP_DISMISSED_(id_APP_evaluation, id_ROUND, ID_APPLYapp)
                'Temporaly removed to validate if thi is neccesary to generate another Evaluation APP with the status dismissed know

            End If

        End Using

    End Sub

    Sub Generate_EVAL_APP_DISMISSED_(ByVal id_APP_evaluation As Integer, ByVal id_ROUND As Integer, ByVal ID_APPLYapp As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oRound = dbEntities.VW_TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = id_APP_evaluation And p.ID_ROUND = id_ROUND).FirstOrDefault()
            Dim idEVALround = Convert.ToInt32(oRound.ID_EVALUATION_ROUND)

            Dim oTA_APPLY_APP = dbEntities.TA_APPLY_APP.Find(ID_APPLYapp)
            Dim oSol = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = oTA_APPLY_APP.ID_SOLICITATION_APP).FirstOrDefault()

            Dim idStatus As Integer = Get_STATUS(2, oRound.ID_VOTING_TYPE, False) 'Discarded

            Dim oTA_EVALUATION_APP_STATUS = dbEntities.TA_EVALUATION_APP_STATUS.Find(idStatus)

            Dim oEVALUATION_APP = New TA_EVALUATION_APP

            oEVALUATION_APP.ID_EVALUATION_ROUND = idEVALround
            oEVALUATION_APP.ID_APPLY_APP = ID_APPLYapp
            oEVALUATION_APP.EVALUATION_START_DATE = Date.UtcNow
            oEVALUATION_APP.ID_EVALUATION_APP_STATUS = idStatus
            oEVALUATION_APP.EVALUATION_SCORE = 0
            oEVALUATION_APP.EVALUATION_VOTES = 0
            oEVALUATION_APP.EVALUATION_UNTIED = 0
            oEVALUATION_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oEVALUATION_APP.FECHA_CREA = Date.UtcNow
            oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idEVALapp As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, 0), idEVALapp) Then

                'Save Evaluation Comment
                'Me.H_ID_EVALUATION_APP.Value = idEVALapp
                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                'Dim oTA_EVALUATION_APP_STATUS = oTA_APPLY_APP.TA_EVALUATION_APP.FirstOrDefault.ID_EVALUATION_APP_STATUS

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                oTA_EVALUATION_APP_COMM.ROUND = id_ROUND
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = idStatus
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} Is setted As {2} For {1} ", id_ROUND, String.Format("( {0}-{1} )", oSol.ORGANIZATIONNAME, oSol.NAMEALIAS), oTA_EVALUATION_APP_STATUS.EVALUATION_APP_STATUS)
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            End If


        End Using

    End Sub


    Sub F_REVIEW(ByVal boolREview As Boolean)


        Using dbEntities As New dbRMS_JIEntities

            ' If boolVoting Then

            Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
            Me.Editor_eval_comments.Content = ""

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

            Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

            '****************************ADDING_VOTES**********************************
            Dim TotVotes = Convert.ToInt32(oTA_EVALUATION_APP.EVALUATION_VOTES)
            TotVotes = TotVotes + (If(boolREview, 1, -1))

            oTA_EVALUATION_APP.EVALUATION_VOTES = TotVotes
            oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
            Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

            If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened
                oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                Dim id_activity = Val(Me.lbl_id_ficha.Text)
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'APPLY / EVALUATION
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************

            End If

            Dim num As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = If(boolREview, 1, -1)
                oTA_EVALUATION_APP_COMM.POINTS = 0
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                    ''****************************CHECK THE SELECTED ONE APPLICATION******************************
                    ''******ON EVALUATION
                    Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED_REV", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))



                    If Percent_Obtained >= 100 Then '****Accepted tough

                        oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                        oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted Status
                        oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                        oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                        oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                        oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                            'Save Evaluation APP Comment

                            oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                            oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Review has been Accepted!"
                            oTA_EVALUATION_APP_COMM.SCORE = 0
                            oTA_EVALUATION_APP_COMM.VOTE = 0
                            oTA_EVALUATION_APP_COMM.POINTS = 0
                            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                            oTA_EVALUATION_APP_COMM.COMM_BOL = False
                            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                                '************************************CHECK THE VOTING PROGRESS********************************************************
                                Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_REV", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)

                                '************************************CHECK THE VOTING PROGRESS********************************************************
                                If VotingProgress >= 100 Then '******All Votes Made***************************


                                    CLOSE_ROUND_PROCC_REV(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, 0)


                                End If '******All Votes Made***************************
                                '************************************CHECK THE VOTING PROGRESS********************************************************

                            End If

                        End If

                    Else

                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_REV", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)

                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        If VotingProgress >= 100 Then '******All Votes Made***************************


                            CLOSE_ROUND_PROCC_REV(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, 1)


                        End If '******All Votes Made***************************
                        '************************************CHECK THE VOTING PROGRESS********************************************************

                        '****************************CHECK THE SELECTED ONE******************************

                    End If ' Percent_Obtained > 100

                End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

            End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            '            End If

        End Using


    End Sub

    Sub F_POINTS(ByVal boolPointing As Boolean, ByVal pointsValues As Double)


        Using dbEntities As New dbRMS_JIEntities

            ' If boolVoting Then

            Dim strComm As String = Trim(Me.Editor_eval_comments.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
            Me.Editor_eval_comments.Content = ""

            Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
            Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

            Dim idEVAL_app = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)

            '****************************ADDING_VOTES**********************************
            Dim TotPoints = Convert.ToInt32(oTA_EVALUATION_APP.EVALUATION_SCORE)
            TotPoints = TotPoints + pointsValues

            oTA_EVALUATION_APP.EVALUATION_SCORE = TotPoints
            oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
            oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma").ToString())
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim idAPPLYapp As Integer = Convert.ToInt32(Me.H_ID_APPLY_APP.Value)
            Dim idEVALround As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

            If oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) Then 'it's just opened
                oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(1, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE) 'Next Status
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
                Dim id_activity = Val(Me.lbl_id_ficha.Text)
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'APPLY / EVALUATION
                '***************************************************SET TA_ACTIVITY_STATUS************************************************************************
            End If

            Dim num As Integer
            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

                Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS
                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = strComm
                oTA_EVALUATION_APP_COMM.SCORE = 0
                oTA_EVALUATION_APP_COMM.VOTE = 0
                oTA_EVALUATION_APP_COMM.POINTS = pointsValues
                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                'oTA_EVALUATION_APP_COMM.COMM_TYPE = Convert.ToInt32(Me.H_COMM_TYPE.Value)

                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then

                    ''****************************CHECK THE SELECTED ONE APPLICATION******************************
                    ''******ON EVALUATION
                    Dim Percent_Obtained = CHECK_EVAL("PERC_OBTAINED_POINTS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()))



                    If Percent_Obtained >= 100 Then '****Accepted tough

                        oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_app)
                        oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'Accepted Status
                        oTA_EVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                        oTA_EVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                        oTA_EVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                        oTA_EVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then 'Saving Accepted
                            'Save Evaluation APP Comment

                            oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVAL_app
                            oTA_EVALUATION_APP_COMM.ROUND = idEVALround
                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS 'Accepted
                            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Evaluation has been Approved!"
                            oTA_EVALUATION_APP_COMM.SCORE = 0
                            oTA_EVALUATION_APP_COMM.VOTE = 0
                            oTA_EVALUATION_APP_COMM.POINTS = 0
                            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                            oTA_EVALUATION_APP_COMM.COMM_BOL = False
                            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                            If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                                '************************************CHECK THE VOTING PROGRESS********************************************************
                                Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_POINTS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)

                                '************************************CHECK THE VOTING PROGRESS********************************************************
                                If VotingProgress >= 100 Then '******All Votes Made***************************


                                    CLOSE_ROUND_PROCC_POINTS(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, False)


                                End If '******All Votes Made***************************
                                '************************************CHECK THE VOTING PROGRESS********************************************************

                            End If

                        End If

                    Else

                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        Dim VotingProgress = CHECK_EVAL("VOTE_PROGRESS_POINTS", idEVAL_app, Convert.ToInt32(Me.Session("E_IdUser").ToString()), oTA_EVALUATION_APP.ID_EVALUATION_ROUND)

                        '************************************CHECK THE VOTING PROGRESS********************************************************
                        If VotingProgress >= 100 Then '******All Votes Made***************************

                            CLOSE_ROUND_PROCC_POINTS(oTA_EVALUATION_APP.ID_EVALUATION_ROUND, True)

                        End If '******All Votes Made***************************
                        '************************************CHECK THE VOTING PROGRESS********************************************************

                        '****************************CHECK THE SELECTED ONE******************************

                    End If ' Percent_Obtained > 100

                End If 'Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) 

            End If  ' If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oTA_EVALUATION_APP, idEVAL_app), num) Then

            '            End If

        End Using


    End Sub

    Sub F_NEGOTIATE(ByVal boolNEG As Boolean)



    End Sub

    Function CHECK_EVAL(ByVal EVAL_type As String, ByVal idEVAL_APP As Integer, ByVal idUser As Integer, Optional idEVA_Round As Integer = 0) As Double

        Using dbEntities As New dbRMS_JIEntities

            CHECK_EVAL = 0

            Select Case EVAL_type

                Case "SCORE"

                Case "VOTE_YET_SCORE"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)

                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)
                    Dim ToVoT As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.SCORE > 0)).Count()

                    Dim id_Apply_Eval As Integer = oTA_EVALUATION_ROUNDS.ID_APPLY_EVALUATION
                    Dim Tot_Members As Integer = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = id_Apply_Eval And p.ID_USER = idUser).Count()

                    If Tot_Members = 1 Then

                        If ((oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                        ) And (ToVoT > 0)) Then

                            Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1

                        ElseIf (ToVoT) > 0 Then
                            Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1
                        Else
                            CHECK_EVAL = 0
                            Me.btnlk_evaluate.Attributes.Add("href", "javascript:OpenRadWindowTool('');")
                        End If

                    Else

                        Me.btnlk_evaluate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 1

                    End If

                Case "VOTE_YET"


                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)

                    Dim oTA_EVALUATION_APP_All = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round)


                    '*********** here

                    Dim oTA_EVALUATION_APP_COMM = (From A In dbEntities.TA_EVALUATION_APP
                                                   Join B In dbEntities.TA_EVALUATION_APP_COMM On A.ID_EVALUATION_APP Equals B.ID_EVALUATION_APP
                                                   Where (A.ID_EVALUATION_ROUND = idEVA_Round And B.VOTE = 1 And B.ID_USUARIO_CREA = idUser)
                                                   Select New With {.ID_EVALUATION_APP = A.ID_EVALUATION_APP,
                                                                     .ID_USUARIO_CREA = B.ID_USUARIO_CREA,
                                                                     .VOTE = B.VOTE}).ToList()


                    'Dim oVW_TA_ACT_SOL_APP_EVAL_Progress = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                    '                                       Where A.ID_EVALUATION_ROUND = idEVAL_Round
                    '                                       Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                    '                                       Select ID_EVALUATION_ROUND, ID_ROUND, PROGRESS = Group.Sum(Function(p) p.EVALUATION_VOTES)


                    'listImplementer = (From IM In dbEntities.t_ejecutores
                    '                   Join AC In dbEntities.tme_Ficha_Proyecto On IM.id_ejecutor Equals AC.id_ejecutor
                    '                   Join TP In dbEntities.tme_organization_type On IM.id_organization_type Equals TP.id_organization_type
                    '                   Where ((AC.id_ficha_estado <> 3) And (IM.id_ejecutor = vImplementer Or bndImplementer = 1))
                    '                   Select New With {.id_ejecutor = IM.id_ejecutor,
                    '                    .Implementer = IM.nombre_ejecutor,
                    '                    .organization_type = TP.organization_type} Distinct).ToList()



                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)
                    Dim ToVoT As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And p.VOTE = 1).Count()

                    Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")

                    Dim id_Apply_Eval As Integer = oTA_EVALUATION_ROUNDS.ID_APPLY_EVALUATION
                    Dim Tot_Members As Integer = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = id_Apply_Eval And p.ID_USER = idUser).Count()


                    If Tot_Members = 1 Then

                        If (oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                        ) Then

                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1

                        ElseIf (ToVoT) > 0 Then
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1
                        ElseIf oTA_EVALUATION_APP_COMM.Count() >= oTA_EVALUATION_ROUNDS.VOTES_MAX Then
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1
                        Else
                            CHECK_EVAL = 0
                        End If


                    Else

                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 1

                    End If



                Case "VOTE_YET_REV"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)


                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)
                    Dim ToVoT As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.VOTE = 1 Or p.VOTE = -1)).Count()


                    Dim id_Apply_Eval As Integer = oTA_EVALUATION_ROUNDS.ID_APPLY_EVALUATION
                    Dim Tot_Members As Integer = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = id_Apply_Eval And p.ID_USER = idUser).Count()

                    If Tot_Members = 1 Then

                        If (oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False)
                        ) Then

                            Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")

                            CHECK_EVAL = 1

                        ElseIf (ToVoT) > 0 Then

                            Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1

                        Else
                            CHECK_EVAL = 0
                        End If

                    Else

                        Me.btnlk_OK.Attributes.Add("class", "btn btn-primary btn-sm margin-r-5 pull-left disabled")
                        Me.btnlk_DISMISS.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 1

                    End If


                Case "VOTE_YET_POINTS"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)


                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)
                    Dim ToVoT As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.POINTS > 0)).Count()

                    Dim id_Apply_Eval As Integer = oTA_EVALUATION_ROUNDS.ID_APPLY_EVALUATION
                    Dim Tot_Members As Integer = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = id_Apply_Eval And p.ID_USER = idUser).Count()

                    If Tot_Members = 1 Then

                        If (oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) _
                        Or oTA_EVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, False)
                        ) Then

                            Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1

                        ElseIf (ToVoT) > 0 Then

                            Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            CHECK_EVAL = 1

                        Else
                            CHECK_EVAL = 0
                        End If


                    Else

                        Me.btnlk_Aggregate.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 1

                    End If


                Case "VOTE_ADD"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)
                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)

                    Dim idVotingType = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim ToVoT As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And p.VOTE = 1 And oTA_EVALUATION_APP.Contains(p.ID_EVALUATION_APP) And p.ID_EVALUATION_APP_STATUS <> idVotingType).Count()
                    Dim ToTIE As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And p.VOTE = 1 And oTA_EVALUATION_APP.Contains(p.ID_EVALUATION_APP) And p.ID_EVALUATION_APP_STATUS = idVotingType).Count()

                    Dim Tot_votes As Integer = oTA_EVALUATION_ROUNDS.VOTES_MAX
                    Dim tot_TIED As Integer = oTA_EVALUATION_ROUNDS.TIED_TOTAL


                    If ((Tot_votes + tot_TIED) - (ToVoT + ToTIE)) = 0 Then
                        Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 0
                    Else
                        Me.btnlk_Untied.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                        CHECK_EVAL = 1
                    End If


                Case "VOTE_REV_ADD"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)
                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)

                    Dim idVotingType = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim ToVoT As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.VOTE = 1 Or p.VOTE = -1) And oTA_EVALUATION_APP.Contains(p.ID_EVALUATION_APP) And p.ID_EVALUATION_APP_STATUS <> idVotingType).Count()
                    Dim ToTIE As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.VOTE = 1 Or p.VOTE = -1) And oTA_EVALUATION_APP.Contains(p.ID_EVALUATION_APP) And p.ID_EVALUATION_APP_STATUS = idVotingType).Count()

                    Dim Tot_votes As Integer = oTA_EVALUATION_ROUNDS.VOTES_MAX
                    Dim tot_TIED As Integer = oTA_EVALUATION_ROUNDS.TIED_TOTAL


                    If ((Tot_votes + tot_TIED) - (ToVoT + ToTIE)) = 0 Then
                        Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        CHECK_EVAL = 0
                    Else
                        Me.btnlk_Untied_Review.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                        CHECK_EVAL = 1
                    End If

                Case "VOTE_TOT"

                    Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault
                    Dim ToVoT As Double = oVW_TA_ACT_SOL_APP_EVAL.EVALUATION_VOTES
                    CHECK_EVAL = ToVoT

                Case "VOTE_PROGRESS"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND
                    Dim oVW_TA_ACT_SOL_APP_EVAL_Progress = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                                                           Where A.ID_EVALUATION_ROUND = idEVAL_Round
                                                           Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                                                           Select ID_EVALUATION_ROUND, ID_ROUND, PROGRESS = Group.Sum(Function(p) p.EVALUATION_VOTES)

                    Dim Tot_Votes = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.TOT_VOTES
                    Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_Progress.FirstOrDefault.PROGRESS

                    CHECK_EVAL = (tot_Votes_Collected / Tot_Votes) * 100

                Case "VOTE_PROGRESS_REV"

                    'Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    'Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND

                    'Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)
                    'Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)

                    'Dim idVotingType = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    'Dim ToVoT As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.VOTE = 1 Or p.VOTE = -1) And oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP)).Count()


                    Dim oTA_EVALUATION_APP_LIST = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()

                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP))
                    Dim tot_Votes_Collected As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.VOTE = 1 Or p.VOTE = -1).Count()

                    'Dim oVW_TA_ACT_SOL_APP_EVAL_Progress = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                    '                                       Where A.ID_EVALUATION_ROUND = idEVAL_Round
                    '                                       Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                    '                                       Select ID_EVALUATION_ROUND, ID_ROUND, PROGRESS = Group.Sum(Function(p) p.EVALUATION_VOTES)

                    Dim Tot_Votes = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.TOT_VOTES

                    'Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_Progress.FirstOrDefault.PROGRESS

                    CHECK_EVAL = (tot_Votes_Collected / Tot_Votes) * 100

                Case "VOTE_PROGRESS_POINTS"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)
                    'Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)

                    'Dim idVotingType = Get_STATUS(3, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    'Dim ToVoT As Integer = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_USUARIO_CREA = idUser And (p.VOTE = 1 Or p.VOTE = -1) And oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP)).Count()


                    Dim oTA_EVALUATION_APP_LIST = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()

                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP))
                    Dim tot_Points_Collected As Double = oEVALUATION_APP_COMM.Where(Function(p) p.POINTS > 0).Count()

                    'Dim oVW_TA_ACT_SOL_APP_EVAL_Progress = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                    '                                       Where A.ID_EVALUATION_ROUND = idEVAL_Round
                    '                                       Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                    '                                       Select ID_EVALUATION_ROUND, ID_ROUND, PROGRESS = Group.Sum(Function(p) p.EVALUATION_VOTES)

                    Dim idVotingType_OPEN = Get_STATUS(0, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim idVotingTypeON_EVAL = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim idVotingTypeON_SEL = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True)

                    Dim TotApplication = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round And (p.ID_EVALUATION_APP_STATUS = idVotingType_OPEN Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_EVAL Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_SEL)).Count()
                    Dim totMemebers = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.MEMBERS

                    'Dim Tot_points = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.POINTS_MAX
                    'Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_Progress.FirstOrDefault.PROGRESS

                    CHECK_EVAL = (tot_Points_Collected / (TotApplication * totMemebers)) * 100



                Case "VOTE_PROGRESS_SCORE"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)


                    Dim oTA_EVALUATION_APP_LIST = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()

                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP))
                    Dim tot_Score_Collected As Double = oEVALUATION_APP_COMM.Where(Function(p) p.SCORE > 0).Count()

                    'Dim oVW_TA_ACT_SOL_APP_EVAL_Progress = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                    '                                       Where A.ID_EVALUATION_ROUND = idEVAL_Round
                    '                                       Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                    '                                       Select ID_EVALUATION_ROUND, ID_ROUND, PROGRESS = Group.Sum(Function(p) p.EVALUATION_VOTES)

                    Dim idVotingType_OPEN = Get_STATUS(0, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim idVotingTypeON_EVAL = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim idVotingTypeON_SEL = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True)

                    Dim TotApplication = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round And (p.ID_EVALUATION_APP_STATUS = idVotingType_OPEN Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_EVAL Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_SEL)).Count()
                    Dim totMemebers = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.MEMBERS

                    'Dim Tot_points = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.POINTS_MAX
                    'Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_Progress.FirstOrDefault.PROGRESS

                    CHECK_EVAL = (tot_Score_Collected / (TotApplication * totMemebers)) * 100



                Case "VOTE_PROGRESS_SCORE_IS"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND
                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVA_Round)

                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP)

                    Dim tot_Score_Collected As Double = oEVALUATION_APP_COMM.Where(Function(p) p.SCORE > 0).Count()

                    'Dim idVotingType_OPEN = Get_STATUS(0, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    'Dim idVotingTypeON_EVAL = Get_STATUS(1, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    'Dim idVotingTypeON_SEL = Get_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True)
                    'Dim TotApplication = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVA_Round And (p.ID_EVALUATION_APP_STATUS = idVotingType_OPEN Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_EVAL Or p.ID_EVALUATION_APP_STATUS = idVotingTypeON_SEL)).Count()

                    Dim totMemebers = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.MEMBERS

                    CHECK_EVAL = (tot_Score_Collected / totMemebers) * 100


                Case "VOTE_PROGRESS_REV_UN"


                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)
                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND

                    Dim oTA_EVALUATION_APP_LIST = dbEntities.TA_EVALUATION_APP.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round).Select(Function(p) p.ID_EVALUATION_APP).ToList()
                    Dim oEVALUATION_APP_COMM = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) oTA_EVALUATION_APP_LIST.Contains(p.ID_EVALUATION_APP))

                    Dim idVotingType = Get_STATUS(3, oTA_EVALUATION_APP.TA_EVALUATION_ROUNDS.ID_VOTING_TYPE)
                    Dim tot_Votes_Collected_untied As Integer = oEVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP_STATUS = idVotingType And (p.VOTE = 1 Or p.VOTE = -1)).Count()

                    'Dim oVW_TA_ACT_SOL_APP_EVAL_UNTIES = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                    '                                     Where A.ID_EVALUATION_ROUND = idEVAL_Round
                    '                                     Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                    '                                     Select ID_EVALUATION_ROUND, ID_ROUND, TOT_UNTIES = Group.Sum(Function(p) p.EVALUATION_UNTIED)
                    'Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_UNTIES.FirstOrDefault.TOT_UNTIES

                    Dim Members As Integer = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.MEMBERS
                    Dim Tot_Break_Untied As Integer = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_EVALUATION_ROUND = idEVAL_Round).FirstOrDefault().TIED_TOTAL
                    Dim Tot_Votes = (Members * Tot_Break_Untied)


                    CHECK_EVAL = (tot_Votes_Collected_untied / Tot_Votes) * 100

                Case "VOTE_PROGRESS_UN"

                    Dim oTA_EVALUATION_APP = dbEntities.TA_EVALUATION_APP.Find(idEVAL_APP)

                    Dim idEVAL_Round = oTA_EVALUATION_APP.ID_EVALUATION_ROUND
                    Dim oVW_TA_ACT_SOL_APP_EVAL_UNTIES = From A In dbEntities.VW_TA_ACT_SOL_APP_EVAL
                                                         Where A.ID_EVALUATION_ROUND = idEVAL_Round
                                                         Group A By A.ID_EVALUATION_ROUND, A.ID_ROUND Into Group
                                                         Select ID_EVALUATION_ROUND, ID_ROUND, TOT_UNTIES = Group.Sum(Function(p) p.EVALUATION_UNTIED)

                    Dim Tot_Votes = (dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.MEMBERS * dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault.TIED_TOTAL)
                    Dim tot_Votes_Collected = oVW_TA_ACT_SOL_APP_EVAL_UNTIES.FirstOrDefault.TOT_UNTIES

                    CHECK_EVAL = (tot_Votes_Collected / Tot_Votes) * 100

                Case "PERC_OBTAINED"

                    Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault
                    CHECK_EVAL = oVW_TA_ACT_SOL_APP_EVAL.PERC_OBTAINED

                Case "PERC_OBTAINED_REV"

                    Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault
                    CHECK_EVAL = oVW_TA_ACT_SOL_APP_EVAL.PERC_OBTAINED

                Case "PERC_OBTAINED_POINTS"

                    Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault
                    CHECK_EVAL = oVW_TA_ACT_SOL_APP_EVAL.PERC_OBTAINED

                Case "PERC_OBTAINED_SCORE"

                    Dim oVW_TA_ACT_SOL_APP_EVAL = dbEntities.VW_TA_ACT_SOL_APP_EVAL.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).FirstOrDefault
                    CHECK_EVAL = oVW_TA_ACT_SOL_APP_EVAL.PERC_OBTAINED

                Case "POINT"

                Case "REVIEW"

                Case "NEGOTIATION"

            End Select

        End Using

    End Function

    Private Sub Repeater_Organization_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles Repeater_Organization.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rept_ORG_EVAL")
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            rept_Messages.DataSource = cls_Solicitation.get_Applications_rounds(DataBinder.Eval(ItemD.DataItem, "ID_ACTIVITY_SOLICITATION").ToString(), DataBinder.Eval(ItemD.DataItem, "ID_ORGANIZATION_APP").ToString())
            rept_Messages.DataBind()

        End If
    End Sub

    Private Sub btnlk_Untied_Click(sender As Object, e As EventArgs) Handles btnlk_Untied.Click

        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then


                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                Select Case oRound.ID_VOTING_TYPE

                    Case 1

                        F_SCORING(True, 0, "")


                    Case 2

                        F_UNTIED_VOTING(True)

                    Case 3

                        F_REVIEW(True)

                    Case 4

                        F_POINTS(True, 0)

                    Case 5

                        F_NEGOTIATE(True)

                End Select

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using

    End Sub

    Private Sub btnlk_OK_Click(sender As Object, e As EventArgs) Handles btnlk_OK.Click



        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then


                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                Select Case oRound.ID_VOTING_TYPE

                    Case 1

                        F_SCORING(True, 0, "")


                    Case 2

                        F_VOTING(True)

                    Case 3 'Buttom for review

                        F_REVIEW(True)

                    Case 4

                        F_POINTS(True, 0)

                    Case 5

                        F_NEGOTIATE(True)

                End Select

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using


    End Sub

    Private Sub btnlk_DISMISS_Click(sender As Object, e As EventArgs) Handles btnlk_DISMISS.Click




        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then


                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                Select Case oRound.ID_VOTING_TYPE

                    Case 1

                        F_SCORING(True, 0, "")


                    Case 2

                        F_VOTING(True)

                    Case 3 'Buttom for review

                        F_REVIEW(False)

                    Case 4

                        F_POINTS(True, 0)

                    Case 5

                        F_NEGOTIATE(True)

                End Select

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using

    End Sub

    Private Sub btnlk_Untied_Review_Click(sender As Object, e As EventArgs) Handles btnlk_Untied_Review.Click


        Using dbEntities As New dbRMS_JIEntities

            If Me.Editor_eval_comments.Text.Trim <> "" Then


                Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                Select Case oRound.ID_VOTING_TYPE

                    Case 1

                        F_SCORING(True, 0, "")


                    Case 2

                        F_UNTIED_VOTING(True)

                    Case 3

                        F_UNTIED_REVIEW(True)

                    Case 4

                        F_POINTS(True, 0)

                    Case 5

                        F_NEGOTIATE(True)

                End Select

                'id ID Activity
                'ir ID Round (0,1,2,3,4)
                'ia ID Solicitation App
                'is ID Activity SOlicitation
                '*********************SET URL AGAING
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

            End If

        End Using



    End Sub

    Private Sub btnlk_Aggregate_Click(sender As Object, e As EventArgs) Handles btnlk_Aggregate.Click


        Using dbEntities As New dbRMS_JIEntities

            Dim aggregateValues As Double = Me.H_POINTS_VAL.Value

            If aggregateValues > 0 Then

                Me.lblt_values_err.Visible = False

                If Me.Editor_eval_comments.Text.Trim <> "" Then

                    Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                    Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

                    Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)

                    Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()


                    Select Case oRound.ID_VOTING_TYPE

                        Case 1

                            F_SCORING(True, 0, "")


                        Case 2

                            F_VOTING(True)

                        Case 3

                            F_REVIEW(True)

                        Case 4

                            F_POINTS(True, aggregateValues)

                        Case 5

                            F_NEGOTIATE(True)

                    End Select

                    'id ID Activity
                    'ir ID Round (0,1,2,3,4)
                    'ia ID Solicitation App
                    'is ID Activity SOlicitation
                    '*********************SET URL AGAING
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    'href="frm_ActivityEvaluation?ut=8A515D75-A2E7-4886-ABB5-413928D1465C&id=1&ia=4&Is=1&ir=3&_tab=EVALUATION_BOX

                End If

            Else

                Me.lblt_values_err.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "SET_Sliders();", True)

            End If

        End Using


    End Sub

    'Private Sub btnlk_evaluate_Click(sender As Object, e As EventArgs) Handles btnlk_evaluate.Click

    '    Using dbEntities As New dbRMS_JIEntities

    '        Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
    '        Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)

    '        Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)
    '        Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()
    '        'Me.grd_answers.DataSourceID = ""
    '        'Me.grd_answers.DataSource = dbEntities.VW_TA_EVALUATION_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
    '        'Me.grd_answers.DataBind()
    '        Me.grd_screening.DataSourceID = ""
    '        Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = oRound.ID_MEASUREMENT_SURVEY).OrderBy(Function(o) o.order_numberQU).ToList()
    '        Me.grd_screening.DataBind()
    '        Me.dv_answers.Attributes.Add("style", "display:block;")

    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", "OpenRadWindowTool('')", True)

    '    End Using

    'End Sub

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

    Private Sub btnlk_eval_Click(sender As Object, e As EventArgs) Handles btnlk_eval.Click


        Using dbEntities As New dbRMS_JIEntities


            Dim ID_EVAL_APP = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idUSer As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            'Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
            'Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app)
            'Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING
            ''***********************************************************************************************************************************************
            ''***************************************************SCREENING APPLICATION***********************************************************************

            Dim bndPASS As Boolean = True

            For Each row In Me.grd_screening.Items

                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_answer_scale")
                    Dim ItemD As GridDataItem = CType(row, GridDataItem)
                    Dim answCODE As String = ItemD("answer_type_code").Text
                    Dim registerValue As Boolean = Convert.ToBoolean(ItemD("register_value").Text)


                    Dim EvalType As String = ItemD("EVAL_TYPE").Text

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

                    ElseIf answCODE = "VALUEENTRY" And registerValue Then

                        If ctrl_answerVALUE.Value = 0 Then
                            bndPASS = False
                        End If

                    End If

                End If

            Next


            If bndPASS Then

                Dim oANSWERcounter As Integer = dbEntities.TA_EVALUATION_ANSWER.Where(Function(p) p.ID_EVALUATION_APP = ID_EVAL_APP And p.ID_USUARIO_CREA = idUSer).Count()
                'TA_EVALUATION_ANSWER.ID_EVALUATION_APP = ID_EVAL_APP


                If (oANSWERcounter = 0) Then

                    lblt_error.Visible = False


                    Dim strHTML As String = " <div class='box-body table-responsive no-padding'>  "
                    Dim strHTML_2 As String = " <div class='box-body table-responsive no-padding'>  "

                    strHTML &= "<table class='table table-hover'>
                                        <tr>
                                          <th>No</th>
                                          <th>Assessment Questions</th>
                                          <th>--</th>
                                          <th>--</th>                                          
                                        </tr>"


                    strHTML_2 &= "<table class='table table-hover'>
                                        <tr>
                                          <th>No</th>
                                          <th>Assessment Questions</th>
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


                    Dim strRowMED_Tot_2 As String = "<tr style='background-color: #2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

                    Dim strRowMED_Tot_3 As String = "<tr style='background-color: #2ea18e; font-weight:500;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#FFFFFF; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

                    Dim strRowMED_Tot_4 As String = "<tr style='border-bottom-color:#ee7108; border:1px dotted #ee7108;  font-weight:700;font-family: Arial, Helvetica, sans-serif;font-size: small; color:#2ea18e; height:20px;' > 
                                                  <td colspan='2' >{0}</td>
                                                  <td>{1}</td>
                                                  <td>{2}</td>
                                                  <td></td>
                                                 <td>{3}</td>
                                                </tr>"

                    Dim strRowTot As String = ""
                    Dim strRowTot_2 As String = ""

                    Dim TotvalScore As Double = 0
                    Dim valScore As Double = 0

                    Dim idAnswerScale As Integer = 0
                    Dim remaining_Score As Double = 0
                    Dim totPendingScored As Double = 0
                    Dim remaining_Question As String = ""
                    Dim idOptionSelected As Integer = 0

                    Dim idTheme As Integer = 0
                    Dim idThemePIV As Integer = 0

                    Dim Percent_Theme As Double = 0
                    Dim Percent_ThemePIV As Double = 0

                    Dim strTheme As String = 0
                    Dim strThemePIV As String = 0

                    Dim Percent_Question As Double = 0
                    Dim Percent_Answer As Double = 0
                    Dim Percent_TOT_Theme As Double = 0
                    Dim Percent_TOT As Double = 0
                    Dim boolswitched As Boolean = False

                    For Each row In Me.grd_screening.Items

                        If TypeOf row Is GridDataItem Then

                            Dim dataItem As GridDataItem = CType(row, GridDataItem)
                            Dim ItemD As GridDataItem = CType(row, GridDataItem)
                            Dim oTA_EVALUATION_ANSWER As New TA_EVALUATION_ANSWER

                            Dim IDquestion_config As Integer = dataItem.GetDataKeyValue("id_measurement_question_config")
                            Dim IDquestion_configEVAL As Integer = dataItem.GetDataKeyValue("id_measurement_question_eval")

                            If idTheme = 0 Then
                                idTheme = dataItem.GetDataKeyValue("id_measurement_theme")
                                idThemePIV = idTheme
                                Percent_Theme = Convert.ToDouble(ItemD("percent_valueMa").Text)
                                strTheme = ItemD("theme_name").Text
                            Else
                                idThemePIV = dataItem.GetDataKeyValue("id_measurement_theme")
                                Percent_ThemePIV = Convert.ToDouble(ItemD("percent_valueMa").Text)
                                strThemePIV = ItemD("theme_name").Text
                            End If

                            Percent_Question = Convert.ToDouble(ItemD("percent_valueQU").Text)

                            Dim answCODE As String = ItemD("answer_type_code").Text
                            Dim EvalType As String = ItemD("EVAL_TYPE").Text

                            Dim ctrl_answer As RadComboBox = CType(row.Cells(0).FindControl("cmb_answer"), RadComboBox)
                            Dim ctrl_answerTEXT As RadTextBox = CType(row.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                            Dim ctrl_answerVALUE As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)

                            oTA_EVALUATION_ANSWER.id_measurement_question_config = IDquestion_config
                            oTA_EVALUATION_ANSWER.id_measurement_question_eval = IDquestion_configEVAL

                            Dim strAnswer As String = ""

                            If answCODE = "DROPDOWN" Then

                                oTA_EVALUATION_ANSWER.id_measurement_answer_option = ctrl_answer.SelectedValue
                                strAnswer = ctrl_answer.Text.Trim
                                idOptionSelected = CType(ctrl_answer.SelectedValue, Int32)
                                valScore = dbEntities.tme_measurement_answer_option.Find(idOptionSelected).percent_value
                                Percent_Answer = valScore

                                ' valScore = dbEntities.ins_measurement_question_config.Find(IDquestion_config).percent_value

                            ElseIf answCODE = "TEXTENTRY" Then

                                oTA_EVALUATION_ANSWER.measurement_answer_text = ctrl_answerTEXT.Text
                                strAnswer = ctrl_answerTEXT.Text.Trim
                                valScore = 0
                                Percent_Answer = 0

                            ElseIf answCODE = "VALUEENTRY" Then

                                oTA_EVALUATION_ANSWER.measurement_answer_value = ctrl_answerVALUE.Value
                                strAnswer = ctrl_answerVALUE.Value.ToString
                                valScore = 0
                                Percent_Answer = 0

                            End If

                            oTA_EVALUATION_ANSWER.ID_EVALUATION_APP = ID_EVAL_APP
                            oTA_EVALUATION_ANSWER.ID_USUARIO_CREA = idUSer

                            dbEntities.TA_EVALUATION_ANSWER.Add(oTA_EVALUATION_ANSWER)

                            Dim oQUESTION = dbEntities.tme_measurement_question.Find(IDquestion_configEVAL)

                            If Not IsNothing(oQUESTION) Then
                                idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                                remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).Max(Function(f) f.percent_value)
                                remaining_Question = oQUESTION.question_name
                            Else
                                remaining_Score = 0
                                remaining_Question = ""
                            End If

                            '***********************************************************************************************************************************************************
                            '*******************************If the Answer Scale is marked with autofill we select the answer option assigned********************************************
                            '***********************************************************************************************************************************************************
                            Dim BoolAutofill As Boolean = Convert.ToBoolean(ItemD("autofill").Text)
                            If BoolAutofill Then

                                idAnswerScale = ItemD("id_measurement_answer_scale").Text
                                idOptionSelected = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).FirstOrDefault.id_measurement_answer_option
                                oTA_EVALUATION_ANSWER.id_measurement_answer_option = idOptionSelected

                            End If

                            totPendingScored += remaining_Score



                            '{0:N2}
                            'If(ctrl_rbn_YESNO.SelectedValue = 0, "<span class='badge badge-danger'>NOT</span>", "<span class='badge badge-danger'>YES</span>")

                            '***********************************************************************************************************

                            If idTheme <> idThemePIV Then

                                Dim subTOT As Double = If(EvalType.Trim = "FACTOR", (Percent_TOT_Theme * (Percent_Theme / 100)), Percent_TOT_Theme)

                                strRowTot_2 &= String.Format(strRowMED_Tot_2, "Total " & strTheme, "", Percent_TOT_Theme, String.Format("{0:N4}", totPendingScored))
                                strRowTot_2 &= String.Format(strRowMED_Tot_3, "Sub-Total Assesment (" & strTheme & ") ", "", subTOT, String.Format("{0:N4}", totPendingScored))

                                strRowTot_2 &= String.Format(strRow_2, ItemD("question_name").Text.Trim, strAnswer, valScore, ItemD("order_numberQU").Text, remaining_Question, remaining_Score)


                                If EvalType.Trim = "FACTOR" Then
                                    Percent_TOT += (Percent_TOT_Theme * (Percent_Theme / 100))
                                Else 'SUM Type of Evaluation
                                    Percent_TOT += Percent_TOT_Theme
                                End If

                                idTheme = idThemePIV
                                Percent_Theme = Percent_ThemePIV
                                strTheme = strThemePIV
                                Percent_TOT_Theme = valScore
                                TotvalScore += valScore
                                'boolswitched = True


                            Else

                                If EvalType.Trim = "FACTOR" Then

                                    Percent_TOT_Theme += (Percent_Question * Percent_Answer)
                                    valScore = (Percent_Question * Percent_Answer)

                                Else 'SUM Type of Evaluation

                                    Percent_TOT_Theme += Percent_Answer

                                End If

                                TotvalScore += valScore


                                strRowTot &= String.Format(strRow, ItemD("question_name").Text.Trim, strAnswer, "", ItemD("order_numberQU").Text)
                                strRowTot_2 &= String.Format(strRow_2, ItemD("question_name").Text.Trim, strAnswer, valScore, ItemD("order_numberQU").Text, remaining_Question, remaining_Score)
                                boolswitched = False

                            End If

                            valScore = 0
                            idOptionSelected = 0

                        End If

                    Next

                    strHTML &= strRowTot

                    'Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)
                    'Dim oTA_SOLICITATION_APP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
                    'Dim idACTIVITY_SOL As Integer = oTA_SOLICITATION_APP.ID_ACTIVITY_SOLICITATION

                    Dim idAPPevaluation = Convert.ToInt32(Me.H_ID_APPLY_EVALUATION.Value)
                    Dim oTA_APPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Find(idAPPevaluation)
                    Dim idRound As Integer = Convert.ToInt32(Me.H_ROUND_ID.Value)
                    Dim oRound = oTA_APPLY_EVALUATION.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_ROUND = idRound).FirstOrDefault()

                    Dim TypeEvaluation As String = dbEntities.ins_measurement_survey.Where(Function(p) p.id_measurement_survey = oRound.ID_MEASUREMENT_SURVEY).FirstOrDefault.EVAL_TYPE

                    'Dim idScreening As Double = dbEntities.TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = idACTIVITY_SOL).FirstOrDefault.ID_SCREENING
                    Dim passValue As Double = oRound.SCORE_BASE

                    'Percent_TOT += (Percent_TOT_Theme * (Percent_Theme / 100))
                    'Dim SubTot As Double = (Percent_TOT_Theme * (Percent_Theme / 100))

                    Dim subTOT_2 As Double = 0.0


                    'If Not boolswitched Then 'just changed the cat

                    If TypeEvaluation.Trim = "FACTOR" Then

                        Percent_TOT += (Percent_TOT_Theme * (Percent_Theme / 100))
                        subTOT_2 = (Percent_TOT_Theme * (Percent_Theme / 100))

                    Else 'SUM Type of Evaluation

                        Percent_TOT += Percent_TOT_Theme
                        subTOT_2 = Percent_TOT_Theme

                    End If

                    'Else

                    '    If TypeEvaluation.Trim = "FACTOR" Then
                    '        subTOT_2 = (Percent_TOT_Theme * (Percent_Theme / 100))
                    '    Else 'SUM Type of Evaluation
                    '        subTOT_2 = Percent_TOT_Theme
                    '    End If

                    'End If


                    strRowTot_2 &= String.Format(strRowMED_Tot_2, "Total " & strTheme, "", Percent_TOT_Theme, String.Format("{0:N4}", totPendingScored))
                    strRowTot_2 &= String.Format(strRowMED_Tot_3, "Sub-Total Assesment (" & strTheme & ") ", "", subTOT_2, String.Format("{0:N4}", totPendingScored))

                    Dim strtotScored As String = String.Format(If(Percent_TOT >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), Percent_TOT)
                    Dim strTotScore As String = String.Format(If(TotvalScore >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), TotvalScore)
                    Dim strtotPendingScored As String = String.Format(If((Percent_TOT + totPendingScored) >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), totPendingScored)

                    strRowTot_2 &= String.Format(strRowMED_Tot_4, "Total (" & lbl_round.Text.Trim & ")", "", strtotScored, strtotPendingScored)

                    'strRowTot_2 &= String.Format("<tr>
                    '                              <td colspan='2' >{0}</td>
                    '                              <td>{1}</td>
                    '                              <td>{2}</td>
                    '                              <td></td>
                    '                              <td>{3}</td>
                    '                             </tr>", "Total Score", "", strTotScore, strtotPendingScored)

                    strHTML &= "</table></div><!-- /.box-body -->"

                    strHTML_2 &= strRowTot_2 & "</table></div><!-- /.box-body -->"

                    Dim lengthA As Integer = Trim(strHTML.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Length
                    Dim lengthB As Integer = Trim(strHTML_2.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Length

                    lengthA = strHTML.Length
                    lengthB = strHTML_2.Length

                    If dbEntities.SaveChanges() Then

                        'oTA_APPLY_SCREE.ID_SCREENING_STATUS = 2 'Responded
                        'oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
                        'dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

                        '********************************************************************************************************************************************************************************

                        Select Case oRound.ID_VOTING_TYPE

                            Case 1

                                F_SCORING(True, Percent_TOT, strHTML_2)


                            Case 2

                                F_VOTING(True)

                            Case 3

                                F_REVIEW(True)

                            Case 4

                                F_POINTS(True, 0)

                            Case 5

                                F_NEGOTIATE(True)

                        End Select

                        'id ID Activity
                        'ir ID Round (0,1,2,3,4)
                        'ia ID Solicitation App
                        'is ID Activity SOlicitation
                        '*********************SET URL AGAING


                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                        Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?Id={0}", Me.lbl_id_ficha.Text)
                        'Me.MsgGuardar.Redireccion = ""
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                        '*****************************************************************************************************************************************************************************


                        '    If dbEntities.SaveChanges() Then

                        '        Dim strComment = "Prescreening responded by the applicant " & "<br /><br /><br />" & Trim(strHTML.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                        '        Dim oScreeningComm As New TA_APPLY_SCREENING_COMM
                        '        oScreeningComm.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                        '        oScreeningComm.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                        '        oScreeningComm.SCREENING_COMM = strComment.Trim
                        '        oScreeningComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        '        oScreeningComm.FECHA_CREA = Date.UtcNow
                        '        oScreeningComm.COMM_BOL = 0
                        '        oScreeningComm.SHOWN_MNGNT_TEAM = 0
                        '        oScreeningComm.COMM_RES = 1

                        '        dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm)
                        '        dbEntities.SaveChanges()

                        '        strComment = "Prescreening score " & "<br /><br /><br />" & Trim(strHTML_2.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")

                        '        Dim oScreeningComm2 = New TA_APPLY_SCREENING_COMM
                        '        oScreeningComm2.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                        '        oScreeningComm2.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                        '        oScreeningComm2.SCREENING_COMM = strComment.Trim
                        '        oScreeningComm2.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        '        oScreeningComm2.FECHA_CREA = Date.UtcNow
                        '        oScreeningComm2.COMM_BOL = 0
                        '        oScreeningComm2.SHOWN_MNGNT_TEAM = 1
                        '        oScreeningComm2.COMM_RES = 1

                        '        dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm2)
                        '        dbEntities.SaveChanges()

                        '        ''**********************************SCREENNNIG SENT*******************************************************
                        '        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        '        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1015, regionalizacionCulture)
                        '        '**********************************SCREENNNIG SENT*******************************************************
                        '        cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING(Id_solicitation_app)

                        '        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApplySCRE?ab=" & IDActivity_Solicitation.ToString & "&ac=" & IDSolicitation_AP.ToString & "&ad=" & IDSolicitation_AP_USER_TK.ToString
                        '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                        '    End If

                    End If

                    '***********************************************************************************************************

                Else 'Already answered


                    Dim a = "No se hace nada"

                    ''Me.MsgGuardar.Redireccion = ""
                    ''ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncReload()", True)
                    Dim Url As String = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
                    Response.Redirect(Url, True)


                End If

            Else


                lblt_error.Visible = True
                ' ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", "OpenRadWindowTool('')", True)

            End If

            ''******************************************************REDIRECT TO SCREENING APPLICATION**********************************************************
            ''***********************************************************************************************************************************************

        End Using



    End Sub

    Private Sub btnlk_test_Click(sender As Object, e As EventArgs) Handles btnlk_test.Click

        'GenerateSummary_Eval(1022)
        'GenerateSummary_Table(1022)

        GenerateSummary_Eval_IS(1039, 1060, True)

        ''*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************
        'Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        ''**********************************CHEMONICS PROCESSS*******************************************************
        'Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        'Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1019, regionalizacionCulture)

        ''**********************************CHEMONCS PROCESSS*******************************************************
        ''cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(dtTies.Rows.Item(i).Item("ID_SOLICITATION_APP"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP_STATUS"), dtTies.Rows.Item(i).Item("ID_EVALUATION_ROUND"), dtTies.Rows.Item(i).Item("ID_EVALUATION_APP")) '' ACCEPTED"
        'cl_Noti_Process.NOTIFIYING_EVALUATION_ACCEPTED(1031, 3, 1024, 40) '' ACCEPTED"
        ''NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer
        ''*********************************************NOTIFIYING THE SELECTED ONE***********************************************************************

        '****************SHOULD POINTED TO NEXT STAGE*****************************************

    End Sub

    Private Sub btnlk_testII_Click(sender As Object, e As EventArgs) Handles btnlk_testII.Click

        'btnlk_testII.Attributes.Add("onclick", "this.disabled='disabled';")
        'btnlk_testII.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")

        Dim vValue As String = "this is a simpel test"
        Dim a = vValue

    End Sub

    Private Sub btn_conflicto_intereses_Click(sender As Object, e As EventArgs) Handles btn_conflicto_intereses.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim ID_EVAL_APP = Convert.ToInt32(Me.H_ID_EVALUATION_APP.Value)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idUSer As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            Dim evaAPP = dbEntities.TA_EVALUATION_APP.Find(ID_EVAL_APP)
            Dim conflictoIntereses = Convert.ToInt32(Me.rbn_conflicto_intereses.SelectedValue)
            Dim conflictoInteresesText = Me.lblt_conflicto_intereses.Text & " " & Me.rbn_conflicto_intereses.SelectedItem.Text
            'evaAPP.conflicto_intereses = If(conflictoIntereses = 1, True, False)
            'evaAPP.ID_EVALUATION_APP_STATUS = Get_STATUS(4, 1, If(conflictoIntereses = 1, False, True))
            dbEntities.Entry(evaAPP).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            Dim Id_solicitation_app = Convert.ToInt32(Me.Request.QueryString("ia"))

            Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)
            Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
            Dim oActivityAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(P) P.ID_SOLICITATION_APP = OSolicitationAPP.ID_SOLICITATION_APP).ToList()


            Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM
            Dim idRound = Convert.ToInt32(Me.Request.QueryString("ir"))
            Dim oRound = dbEntities.TA_EVALUATION_ROUNDS.Find(idRound)
            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = ID_EVAL_APP
            oTA_EVALUATION_APP_COMM.ROUND = idRound
            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Get_STATUS(4, 1, If(conflictoIntereses = 1, False, True))
            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("CONFLICTO DE INTERESES para {0} : {1} ", String.Format("{0} ({1})", oActivityAPP.FirstOrDefault.ORGANIZATIONNAME, oActivityAPP.FirstOrDefault.NAMEALIAS), conflictoInteresesText)
            oTA_EVALUATION_APP_COMM.SCORE = 0
            oTA_EVALUATION_APP_COMM.VOTE = 0
            oTA_EVALUATION_APP_COMM.POINTS = 0
            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
            oTA_EVALUATION_APP_COMM.COMM_BOL = False
            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0)

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = String.Format("~/RFP_/frm_ActivityEvaluation?ut={0}&id={1}&ia={2}&is={3}&ir={4}&_tab=ROUND_BOX#EVA_BOX", Me.Session("idGuiToken").ToString, Me.lbl_id_ficha.Text, Me.H_ID_SOLICITATION_APP.Value, Me.H_ID_ACTIVITY_SOLICITATION.Value, Me.H_ROUND_ID.Value)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using
    End Sub
End Class