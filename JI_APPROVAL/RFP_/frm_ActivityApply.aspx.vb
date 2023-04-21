Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports ly_RMS

Public Class frm_ActivityApply
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_APPLY"
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
                'Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
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

                ' Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

                'Me.alink_stos.Attributes.Add("style", "display:none;")
                'Me.alink_po.Attributes.Add("style", "display:none;")
                'Me.alink_Ik.Attributes.Add("style", "display:none;")

                'Me.alink_stos.Attributes.Add("href", "#")
                'Me.alink_po.Attributes.Add("href", "#")
                'Me.alink_Ik.Attributes.Add("href", "#")


                'Dim i = 0
                'If oSubFather.count > 0 Then

                '    For Each item In oSubFather

                '        If i = 0 Then
                '            Me.alink_stos.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                '            Me.alink_stos.Attributes.Add("style", "display:block;")
                '        ElseIf i = 1 Then
                '            Me.alink_po.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_po.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                '            Me.alink_po.Attributes.Add("style", "display:block;")
                '        Else
                '            Me.alink_Ik.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=IK"))
                '            Me.alink_Ik.Attributes.Add("style", "display:block;")
                '            Me.alink_Ik.Attributes.Add("style", "display:block;")
                '        End If

                '        i += 1

                '    Next

                'End If

                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "PO" Then
                '    Me.alink_areas.Attributes.Add("href", "#")
                '    Me.alink_areas.Attributes.Add("style", "display:none;")
                '    Me.alink_indicadores.Attributes.Add("href", "#")
                '    Me.alink_indicadores.Attributes.Add("style", "display:none;")
                '    Me.alink_waiver.Attributes.Add("href", "#")
                '    Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                Me.txt_activity_code.Text = proyecto.codigo_ficha_AID
                'Me.txt_nombreproyecto.Text = proyecto.nombre_proyecto
                'Me.txt_descripcion.Text = proyecto.area_intervencion


                Me.cmb_type_of_document.DataSourceID = ""
                Me.cmb_type_of_document.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
                Me.cmb_type_of_document.DataTextField = "nombre_documento"
                Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
                Me.cmb_type_of_document.DataBind()

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

                'Me.cmb_Apply_status.DataSourceID = ""
                'Me.cmb_Apply_status.DataSource = cl_listados.get_TA_APPLY_STATUS(idPrograma)
                'Me.cmb_Apply_status.DataTextField = "APPLY_STATUS"
                'Me.cmb_Apply_status.DataValueField = "ID_APPLY_STATUS"
                'Me.cmb_Apply_status.DataBind()

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)
                Dim oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.ID_ACTIVITY = proyecto.id_activity).ToList()

                Me.timeline_activity.ID_ACTIVITY = proyecto.id_activity
                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

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
                    Me.grd_cate.DataSourceID = ""

                    'Dim strSQL = String.Format("select A.ID_ORGANIZATION_APP, 
                    '                                 ltrim(rtrim(A.organization_type)) + ' || ' +  ltrim(rtrim(A.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(A.NAMEALIAS)) + ' || ' + ltrim(rtrim(A.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(A.PERSONNAME)) as Org_search , 
                    '                                organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME  
                    '                               from VW_TA_ORGANIZATION_APP  A
                    '  Left outer join TA_SOLICITATION_APP b on (a.ID_ORGANIZATION_APP = b.ID_ORGANIZATION_APP and b.ID_ACTIVITY_SOLICITATION = {1} )
                    '                            WHERE b.ID_ORGANIZATION_APP IS NULL AND (ltrim(rtrim(a.organization_type)) + ' || ' +  ltrim(rtrim(a.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(a.NAMEALIAS)) + ' || ' + ltrim(rtrim(a.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(a.PERSONNAME)) like '%{0}%' )", " ", oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION)

                    'Me.SqlDataSource2.SelectCommand = strSQL

                    dtApplicants = get_Activity_Applicants(oSolicitation.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    If dtApplicants.Rows.Count = 0 Then
                        createdtcolums(2)
                    End If

                    Session("dtApplicants") = dtApplicants
                    'Me.grd_cate.DataSource = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
                    Load_grdApplicant(dtApplicants)

                    'Me.grd_cate.DataSource = dtApplicants
                    'Me.grd_cate.DataBind()


                    If Me.Request.QueryString("_idAppnts") IsNot Nothing Then

                        Select_Applicant(Convert.ToInt32(Me.Request.QueryString("_idAppnts")))

                    End If



                Else
                    Me.lbl_id_sol.Text = "0"
                    ' Me.SqlDataSource2.SelectCommand = Nothing
                End If


            End Using


        Else

            dtApplicants = Session("dtApplicants")


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
                Dim Id_solicitation_app = Val(Me.lbl_id_sol_app.Text)
                Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()

                If oApply.Count > 0 Then
                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                        btn_delete.Visible = False
                    End If
                End If

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = solicitation_document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"
                ImageDownload.ToolTip = DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

                Dim aprobar As New HyperLink
                aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

                If DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_ANNEX").ToString() = "0" Then
                    ImageDownload.Visible = False
                    btn_delete.Visible = False

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

    Protected Sub btn_agregar_Click(sender As Object, e As EventArgs) Handles btn_agregar.Click

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

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?id=" & id_activity.ToString & "&_tab=Applicants"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


        End Using

        'Me.grd_archivos.DataBind()

    End Sub

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

    Protected Sub btn_continue_Click(sender As Object, e As EventArgs) Handles btn_continue.Click




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

    Protected Sub Organization_DataSourceSelect(sender As Object, e As SearchBoxDataSourceSelectEventArgs)

        'Dim id_ficha As Integer = Convert.ToInt32(Me.Session("E_IdFicha"))

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)

        Dim source As SqlDataSource = DirectCast(e.DataSource, SqlDataSource)
        Dim searchBox As RadSearchBox = DirectCast(sender, RadSearchBox)
        Dim likeCondition As String = String.Format("'%{0}' + @filterString + '%'", If(searchBox.Filter = SearchBoxFilter.Contains, "%", ""))
        Dim countCondition As String = If(e.ShowAllResults, " ", " TOP " + (searchBox.MaxResultCount + 1).ToString())

        Dim strSQL As String = ""

        'Dim id_ejecutor As Integer = db.tme_Ficha_Proyecto.Where(Function(p) p.id_ficha_proyecto = id_ficha).FirstOrDefault.id_ejecutor
        'Dim lsT_ficha = db.tme_Ficha_Proyecto.Where(Function(p) p.id_ejecutor = id_ejecutor) _
        '                                      .Select(Function(s) s.id_ficha_proyecto).ToList()

        'Dim ArrFicha = lsT_ficha.Select(Function(x) x.ToString()).ToArray()

        ''***************Just Profiles of this Activity*******************
        'strSQL = String.Format("select * from vw_beneficiary_organization WHERE (id_ficha_proyecto  in ({2}) ) and name LIKE {1} Order by name", countCondition, likeCondition, String.Join(",", ArrFicha))
        ''************Or Organization in diferent Query**********

        strSQL = String.Format("select A.ID_ORGANIZATION_APP, 
                                    ltrim(rtrim(A.organization_type)) + ' || ' +  ltrim(rtrim(A.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(A.NAMEALIAS)) + ' || ' + ltrim(rtrim(A.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(A.PERSONNAME)) as Org_search , 
                                     organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME  
                                       from VW_TA_ORGANIZATION_APP  A
									   Left outer join TA_SOLICITATION_APP b on (a.ID_ORGANIZATION_APP = b.ID_ORGANIZATION_APP and b.ID_ACTIVITY_SOLICITATION = {1} )
                                     WHERE b.ID_ORGANIZATION_APP IS NULL AND (ltrim(rtrim(a.organization_type)) + ' || ' +  ltrim(rtrim(a.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(a.NAMEALIAS)) + ' || ' + ltrim(rtrim(a.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(a.PERSONNAME)) like {0} )", likeCondition, id_solicitation)

        source.SelectCommand = strSQL.Trim
        source.SelectParameters.Add("filterString", e.FilterString.Replace("%", "[%]").Replace("_", "[_]"))

    End Sub


    Protected Sub Organization_Search(sender As Object, e As SearchBoxEventArgs)

        If e.DataItem IsNot Nothing Then

            Using dbEntities As New dbRMS_JIEntities

                Dim dataItem = DirectCast(e.DataItem, Dictionary(Of String, Object))

                Dim nameOrganization As String = e.Text
                Dim idOrganization = e.Value
                Dim aliasName = dataItem("NAMEALIAS").ToString()

                Dim id_activity = Val(Me.lbl_id_ficha.Text)
                Dim id_solicitation = Val(Me.lbl_id_sol.Text)

                Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_solicitation)

                Dim oSolilcitationAPP As New TA_SOLICITATION_APP

                oSolilcitationAPP.ID_ACTIVITY_SOLICITATION = id_solicitation
                oSolilcitationAPP.ID_APP_STATUS = 1 'REgistered
                oSolilcitationAPP.SOLICITATION_APP_CODE = cl_listados.CrearCodigoSOLICITATION(id_solicitation, oActivitySolicitation.SOLICITATION_CODE)
                oSolilcitationAPP.SOLICITATION_TOKEN = Guid.Parse(cl_user.GenerateToken())
                oSolilcitationAPP.ID_ORGANIZATION_APP = idOrganization

                oSolilcitationAPP.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oSolilcitationAPP.fecha_crea = Date.UtcNow
                oSolilcitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_SOLICITATION_APP.Add(oSolilcitationAPP)

                dbEntities.SaveChanges()


                dtApplicants = get_Activity_Applicants(id_solicitation)
                Session("dtApplicants") = dtApplicants
                'Me.grd_cate.DataSource = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
                'Me.grd_cate.DataSource = dtApplicants
                'Me.grd_cate.DataBind()
                Load_grdApplicant(dtApplicants)

                'Dim isBeneficiary = dataItem("is_beneficiary").ToString()
                'Me.lbl_id_customer.Text = idCustomer
                'Me.lbl_is_beneficiary.Text = isBeneficiary
                'Me.lbl_customer.Text = nameCustomer

                'Using dbEntities As New dbRMS_JIEntities
                '    Dim idsale = Me.Request.QueryString("Id").ToString
                '    Dim organizationSalesbyvc = From v In dbEntities.vw_ins_sale_detail
                '                                Where v.id_sale = idsale
                '                                Group v By v.value_chain Into Group
                '                                Select value_chain, total_query = Group.Sum(Function(v) v.total_sale)


                '    For Each item In organizationSalesbyvc
                '        Dim total = Convert.ToString(item.total_query)
                '        chart = chart + "{name:'" + item.value_chain + "', y: " + total + "}, "
                '    Next

                'End Using

            End Using


        End If
    End Sub



    'Protected Sub Elimina_Elemento(sender As Object, e As EventArgs)
    '    Dim a = CType(sender, LinkButton)
    '    Me.identity_sol.Text = a.Attributes.Item("data-identity").ToString()
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)

    'End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As EventArgs) Handles btn_eliminarDocumento.Click

        Using dbEntities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Try

                Dim ID_SOL_app = Val(Me.identity_sol.Text)
                Dim ID_Doc = Val(Me.identity_doc.Text)


                If ID_SOL_app > 0 Then

                    Dim Sql = "delete from TA_SOLICITATION_APP WHERE ID_SOLICITATION_APP = " & ID_SOL_app.ToString
                    dbEntities.Database.ExecuteSqlCommand(Sql)

                End If


                Me.MsgGuardar.NuevoMensaje = "The record has been removed succesfully"

            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error on removing"
            End Try

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using
    End Sub

    Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim hlnkEdit As LinkButton = New LinkButton
            'hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), LinkButton)
            'hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            'hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            ''hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            'Dim hlnkDelete As LinkButton = New LinkButton
            'hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
            'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())
            'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")

            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkSelected"), CheckBox)
            visible.Checked = False
            visible.InputAttributes.Add("ID_SOLICITATION_APP", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())
            visible.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())

            'Dim visible2 As New CheckBox
            'visible2 = CType(e.Item.FindControl("chkActivo"), CheckBox)

            'If Val(DataBinder.Eval(e.Item.DataItem, "ID_APP_STATUS").ToString()) > 1 Then
            '    'hlnkDelete.Visible = False
            '    visible2.Checked = True
            'End If

            Dim lbl_organization As Label = CType(e.Item.FindControl("lbl_organization_n"), Label)
            lbl_organization.Text = String.Format("{0} ({1})", DataBinder.Eval(e.Item.DataItem, "ORGANIZATIONNAME"), DataBinder.Eval(e.Item.DataItem, "NAMEALIAS"))

            Dim lbl_representative As Label = CType(e.Item.FindControl("lbl_representative"), Label)
            lbl_representative.Text = DataBinder.Eval(e.Item.DataItem, "PERSONNAME")

            Dim lbl_email As Label = CType(e.Item.FindControl("lbl_email"), Label)
            lbl_email.Text = DataBinder.Eval(e.Item.DataItem, "ORGANIZATIONEMAIL")


            Dim lbl_tyep As Label = CType(e.Item.FindControl("lbl_tyep"), Label)
            lbl_tyep.Text = DataBinder.Eval(e.Item.DataItem, "ORGANIZATION_TYPE")

            Dim lbl_country_n As Label = CType(e.Item.FindControl("lbl_country_n"), Label)
            lbl_country_n.Text = DataBinder.Eval(e.Item.DataItem, "ADDRESSCOUNTRYREGIONID")

            Dim lbl_City_n As Label = CType(e.Item.FindControl("lbl_City_n"), Label)
            lbl_City_n.Text = DataBinder.Eval(e.Item.DataItem, "ADDRESSCITY")

            Dim lbl_sent_date As Label = CType(e.Item.FindControl("lbl_sent_date"), Label)

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "SENT_DATE")) Then
                lbl_sent_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "SENT_DATE"), "f", dateUtil.offSET, True)
            Else
                lbl_sent_date.Text = "--"
            End If

            Dim lbl_received_date As Label = CType(e.Item.FindControl("lbl_received_date"), Label)

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "RECEIVED_DATE")) Then
                lbl_received_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "RECEIVED_DATE"), "f", dateUtil.offSET, True)
            Else
                lbl_received_date.Text = "--"
            End If

            Dim lbl_submitted_date As Label = CType(e.Item.FindControl("lbl_submitted_date"), Label)

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "SUBMITTED_DATE")) Then
                lbl_submitted_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "SUBMITTED_DATE"), "f", dateUtil.offSET, True)
            Else
                lbl_submitted_date.Text = "--"
            End If


        End If


    End Sub


    Protected Sub Select_Applicant(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim Id_solicitation_app = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
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

                'If OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT

                '    OSolicitationAPP.ID_APP_STATUS = 3 'Open it
                '    OSolicitationAPP.RECEIVED_DATE = Date.UtcNow
                '    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                '    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                '    If dbEntities.SaveChanges() Then
                '        ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                '    End If

                'Else
                '    ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                'End If

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
                    oSTATUS_APP = "OPENED"
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
                    Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", OStatus.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oApplyCOMM.FECHA_CREA, Date.UtcNow)
                    'Me.AppDOCUMENTS.Attributes.Add("style", "display:block;")

                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then 'On Hold
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 Then 'Accepted
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then 'Rejected
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:block;")
                        Me.btn_agregar.Enabled = False
                    Else
                        Me.Buttons_app.Attributes.Add("style", "display:block;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
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
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:block;")
                        ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                                Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                        Else
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                        End If

                    Else
                        Me.Buttons_approve.Attributes.Add("style", "display:none;")
                        Me.rept_ApplyDates.DataSource = Nothing
                        Me.rept_ApplyDates.DataBind()
                    End If


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


                End If

                'error over here
                For Each Irow As GridDataItem In Me.grd_cate.Items

                    Dim chkSEL As CheckBox = CType(Irow("colm_select").FindControl("chkSelected"), CheckBox)

                    If Irow("ID_SOLICITATION_APP").Text = Id_solicitation_app Then
                        chkSEL.Checked = True
                    Else
                        chkSEL.Checked = False
                    End If

                Next

                dtDocuments = get_Apply_Documents(Id_solicitation_app)

                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
                Me.grd_archivos.DataSource = dtDocuments
                Me.grd_archivos.DataBind()

                Me.TabName.Value = "Applications"
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#dvTab a[href=""#Applications""]').tab('show');", True)

                'Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using


        Catch ex As Exception

        End Try


    End Sub



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

                'If OSolicitationAPP.ID_APP_STATUS = 2 Then 'SENT

                '    OSolicitationAPP.ID_APP_STATUS = 3 'Open it
                '    OSolicitationAPP.RECEIVED_DATE = Date.UtcNow
                '    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                '    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                '    If dbEntities.SaveChanges() Then
                '        ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                '    End If

                'Else
                '    ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                'End If


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
                    oSTATUS_APP = "OPENED"
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
                    Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", OStatus.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oApplyCOMM.FECHA_CREA, Date.UtcNow)
                    'Me.AppDOCUMENTS.Attributes.Add("style", "display:block;")

                    If oApply.FirstOrDefault.ID_APPLY_STATUS = 3 Then 'Ápplied
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then 'On Hold
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 Then 'Accepted
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        Me.btn_agregar.Enabled = False
                    ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then 'Rejected
                        Me.Buttons_app.Attributes.Add("style", "display:none;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:block;")
                        Me.btn_agregar.Enabled = False
                    Else
                        Me.Buttons_app.Attributes.Add("style", "display:block;")
                        Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                        Me.SolicitationRejected.Attributes.Add("style", "display:none;")
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
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:block;")
                        ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                                Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                        Else
                            Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                            Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                            Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
                            Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                        End If

                    Else
                        Me.Buttons_approve.Attributes.Add("style", "display:none;")
                    End If

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

                End If


                'error over here
                For Each Irow As GridDataItem In Me.grd_cate.Items

                    Dim chkSEL As CheckBox = CType(Irow("colm_select").FindControl("chkSelected"), CheckBox)

                    If Irow("ID_SOLICITATION_APP").Text = Id_solicitation_app Then
                        chkSEL.Checked = True
                    Else
                        chkSEL.Checked = False
                    End If

                Next


                dtDocuments = get_Apply_Documents(Id_solicitation_app)

                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
                Me.grd_archivos.DataSource = dtDocuments
                Me.grd_archivos.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#dvTab a[href=""#Applications""]').tab('show');", True)

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

                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 2)   'Created / Solicitation


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

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"


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
            OSolicitationAPP.id_usuario_submit = Convert.ToInt32(Me.Session("E_IDUser"))
            OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

            oApply.ID_APPLY_STATUS = 3 ''Applied
            oApply.APPLY_DATE = Date.UtcNow
            oApply.ID_USUARIO_ASSIGNED = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oApply.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            dbEntities.Entry(oApply).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                '*******************************************PENDING SENT NOTIFIACTION************
                ''cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

                '**********************ADDING TRACK COMMENTS*********************************
                Dim oApplyComm As New TA_APPLY_COMM
                oApplyComm.ID_APPLY_APP = oApply.ID_APPLY_APP
                oApplyComm.ID_APPLY_STATUS = oApply.ID_APPLY_STATUS
                oApplyComm.APPLY_COMM = oApply.APPLY_DESCRIPTION
                oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oApplyComm.FECHA_CREA = Date.UtcNow
                oApplyComm.COMM_BOL = 0

                dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                dbEntities.SaveChanges()



            End If

            '**********************************CHEMONICS PROCESSS*******************************************************
            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1012, regionalizacionCulture)
            '**********************************CHEMONCS PROCESSS*******************************************************
            cl_Noti_Process.NOTIFIYING_SOLICITATION_APPLY(Id_solicitation_app)

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
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
                oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
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
                oApplyComm.APPLY_COMM = "Application Registered"
                oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oApplyComm.FECHA_CREA = Date.UtcNow
                oApplyComm.COMM_BOL = 0

                dbEntities.TA_APPLY_COMM.Add(oApplyComm)
                dbEntities.SaveChanges()

            End If

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & id_solicitation_app.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()

        End Using


    End Sub


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

    Private Sub bntlk_accept_Click(sender As Object, e As EventArgs) Handles bntlk_accept.Click

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

        If Set_Status(4, False) Then 'Accepted

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            '**********************************CHEMONICS PROCESSS*******************************************************
            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1013, regionalizacionCulture)
            '**********************************CHEMONCS PROCESSS*******************************************************
            cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 4) '' ACCEPTED"

            cl_Noti_Process = New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1018, regionalizacionCulture)
            cl_Noti_Process.NOTIFIYING_EVALUATION(Id_solicitation_app, 4) '' ACCEPTED"

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
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


                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End If

        End Using

    End Sub

    Private Sub btnlk_hold_Click(sender As Object, e As EventArgs) Handles btnlk_hold.Click


        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)


        If Set_Status(6, False) Then 'On Hold Status

            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            '**********************************CHEMONICS PROCESSS*******************************************************
            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_Programa, 1013, regionalizacionCulture)
            '**********************************CHEMONCS PROCESSS*******************************************************
            cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 6) '' ON HOLD"


            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End If


    End Sub

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

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End If

    End Sub

    Private Sub btnlk_reject_Click(sender As Object, e As EventArgs) Handles btnlk_reject.Click

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

        If Set_Status(5, False) Then 'Rejected

            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            '**********************************CHEMONICS PROCESSS*******************************************************
            Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
            Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_Programa, 1013, regionalizacionCulture)
            '**********************************CHEMONCS PROCESSS*******************************************************
            cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 5) '' REJECTED"



            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityApply?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End If

    End Sub

    Private Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        If Not IsNothing(Session("dtApplicants")) Then
            Load_grdApplicant(Session("dtApplicants"))
        End If

    End Sub

    Private Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged

        If Not IsNothing(Session("dtApplicants")) Then
            Load_grdApplicant(Session("dtApplicants"))
        End If

    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        If Not IsNothing(Session("dtApplicants")) Then
            Load_grdApplicant(Session("dtApplicants"), False)
        End If
    End Sub

    Public Sub Load_grdApplicant(ByVal tbl_Applicants As DataTable, Optional bndRebind As Boolean = True)

        Me.grd_cate.DataSource = tbl_Applicants

        If bndRebind Then
            Me.grd_cate.DataBind()
        End If

    End Sub

End Class