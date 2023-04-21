Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports ly_RMS

Public Class frm_ActivityPrescreening
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_PREESCRE"
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
                'Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
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

            Dim oSolicitation = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation And p.ID_SCREENING.HasValue) _
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
                                                    .SCREENING_STATUS = p.SCREENING_STATUS,
                                                    .DATE_ANSWERED = p.DATE_ANSWERED,
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

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "DATE_ANSWERED")) Then
                lbl_submitted_date.Text = dateUtil.set_DateFormat(DataBinder.Eval(e.Item.DataItem, "DATE_ANSWERED"), "f", dateUtil.offSET, True)
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
                'Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()

                Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
                Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING

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

                If oTA_APPLY_SCREENING.Count() > 0 Then

                    'Dim idAPP = oApply.FirstOrDefault.ID_APPLY_APP
                    'Dim idAPPstatus = oApply.FirstOrDefault.ID_APPLY_STATUS
                    'Dim oApplyCOMM = dbEntities.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_APP = idAPP And p.ID_APPLY_STATUS = idAPPstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()
                    Dim idSCREENINGstatus = oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS
                    Dim oTA_APPLY_SCREENING_COMM = dbEntities.TA_APPLY_SCREENING_COMM.Where(Function(p) p.ID_APPLY_SCREENING = idAPPLY_screening And p.ID_SCREENING_STATUS = idSCREENINGstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()

                    Dim id_Survey As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_MEASUREMENT_SURVEY

                    ' Me.txt_apply_desc.Text = oApply.FirstOrDefault.APPLY_DESCRIPTION
                    Me.txt_apply_desc.Text = ""
                    ''Me.cmb_Apply_status.SelectedValue = oApply.FirstOrDefault.ID_APPLY_STATUS
                    ' Dim OStatus = dbEntities.TA_APPLY_STATUS.Find(oApply.FirstOrDefault.ID_APPLY_STATUS)
                    Dim oSTATUS_SCREENING = dbEntities.TA_SCREENING_STATUS.Find(oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS)
                    Me.lbl_apply_status.Text = oSTATUS_SCREENING.SCREENING_STATUS
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oTA_APPLY_SCREENING_COMM.FECHA_CREA, "f", timezoneUTC, True)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("badge {0} text-center", oSTATUS_SCREENING.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oTA_APPLY_SCREENING_COMM.FECHA_CREA, Date.UtcNow)

                    Me.Buttons_app.Attributes.Add("style", "display:none;")

                    If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 1 Then 'Peding

                        Me.grd_screening.DataSourceID = ""
                        Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = id_Survey).OrderBy(Function(o) o.order_numberQU).ToList()
                        Me.grd_screening.DataBind()

                        Me.questions_prescreening.Attributes.Add("style", "display:block;")


                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5  disabled")

                        '    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        '    Me.btn_agregar.Enabled = False
                    ElseIf oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 2 Then 'Responded

                        Dim TotScore As Double = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).Sum(Function(p) p.percent_valueAO)
                        Dim TotPass As Double = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault.SCREENING_TOTALPASS

                        Me.lbl_score.Text = String.Format("{0:N4}", TotScore)
                        Me.hd_totScore.Value = TotScore
                        Dim strClassA As String = "badge bg-red"
                        Dim strClassB As String = "badge bg-blue-active"

                        If (TotPass > CDbl(Me.hd_totScore.Value)) Then

                            Me.span_score.Attributes.Add("Class", strClassA)
                            Me.spa_icons.InnerHtml = "<i class='fas fa-smile'></i>"
                            Me.lbl_score_status.Text = "Hasn't passed yet"

                        Else

                            Me.span_score.Attributes.Add("Class", strClassB)
                            Me.spa_icons.InnerHtml = "<i class='fas fa-frown'></i>"
                            Me.lbl_score_status.Text = "Passed"

                        End If

                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right")

                        Dim totEvalQ As Integer = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.id_measurement_question_eval <> 0).Count
                        Dim totEvalQ_answ As Integer = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.id_measurement_question_eval <> 0 And p.id_measurement_answer_option_eval.HasValue).Count

                        If totEvalQ = totEvalQ_answ Then

                            Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right disabled")
                            Me.btn_save_app.Attributes.Add("style", "display:none;")
                            Me.questions_prescreening.Attributes.Add("style", "display:none;")

                            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                            Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                            Me.rept_PrescreeningDates.DataBind()

                            Me.app_prescreening.Attributes.Add("style", "display:block;")
                            Me.Buttons_approve.Attributes.Add("style", "display:block;")

                        Else

                            Me.questions_prescreening.Attributes.Add("style", "display:block;")
                            Me.grd_answers.DataSourceID = ""
                            Me.grd_answers.DataSource = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
                            Me.grd_answers.DataBind()

                        End If


                    ElseIf oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 3 Or oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 4 Then 'PAssed

                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right disabled")
                        Me.btn_save_app.Attributes.Add("style", "display:none;")
                        Me.questions_prescreening.Attributes.Add("style", "display:none;")

                        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                        Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                        Me.rept_PrescreeningDates.DataBind()

                        Me.app_prescreening.Attributes.Add("style", "display:block;")
                        Me.Buttons_approve.Attributes.Add("style", "display:block;")

                        Me.btnlk_comment.Attributes.Add("class", "btn btn-default  btn-sm margin-r-5 pull-left disabled")
                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")

                    End If

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

                    'If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 3 _
                    '     Or oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 4 Then
                    '    'PAssed Or not passed

                    '    Me.Buttons_approve.Attributes.Add("style", "display:block; padding-left:30px;")
                    '    Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)

                    '    'Me.rept_ApplyDates.DataSource = cls_Solicitation.get_Apply_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                    '    'Me.rept_ApplyDates.DataBind()

                    '    Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                    '    Me.rept_PrescreeningDates.DataBind()

                    'If oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:block;")
                    'ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                    '        Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                    'Else
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                    'End If

                    'Else
                    '    Me.Buttons_approve.Attributes.Add("style", "display:none;")
                    '    Me.rept_ApplyDates.DataSource = Nothing
                    '    Me.rept_ApplyDates.DataBind()
                    'End If


                Else

                    Me.txt_apply_desc.Text = ""
                    'Me.cmb_Apply_status.SelectedValue = 1
                    Me.lbl_apply_status.Text = oSTATUS_APP
                    'Me.div2.Attributes.Remove("class")
                    'Me.div2.Attributes.Add("class", "alert-sm bg-red text-center")
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oDate, "f", timezoneUTC, True)
                    Me.lbl_Apply_time.Text = Func_Unit(oDate, Date.UtcNow)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("badge {0} text-center", "badge-warning"))
                    Me.Buttons_app.Attributes.Add("style", "display:block;")
                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    'Me.btn_agregar.Enabled = True


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
                'Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()

                Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
                Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING

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

                If oTA_APPLY_SCREENING.Count() > 0 Then

                    'Dim idAPP = oApply.FirstOrDefault.ID_APPLY_APP
                    'Dim idAPPstatus = oApply.FirstOrDefault.ID_APPLY_STATUS
                    'Dim oApplyCOMM = dbEntities.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_APP = idAPP And p.ID_APPLY_STATUS = idAPPstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()
                    Dim idSCREENINGstatus = oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS
                    Dim oTA_APPLY_SCREENING_COMM = dbEntities.TA_APPLY_SCREENING_COMM.Where(Function(p) p.ID_APPLY_SCREENING = idAPPLY_screening And p.ID_SCREENING_STATUS = idSCREENINGstatus).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault()

                    Dim id_Survey As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_MEASUREMENT_SURVEY

                    ' Me.txt_apply_desc.Text = oApply.FirstOrDefault.APPLY_DESCRIPTION
                    Me.txt_apply_desc.Text = ""
                    ''Me.cmb_Apply_status.SelectedValue = oApply.FirstOrDefault.ID_APPLY_STATUS
                    ' Dim OStatus = dbEntities.TA_APPLY_STATUS.Find(oApply.FirstOrDefault.ID_APPLY_STATUS)
                    Dim oSTATUS_SCREENING = dbEntities.TA_SCREENING_STATUS.Find(oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS)
                    Me.lbl_apply_status.Text = oSTATUS_SCREENING.SCREENING_STATUS
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oTA_APPLY_SCREENING_COMM.FECHA_CREA, "f", timezoneUTC, True)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("badge {0} text-center", oSTATUS_SCREENING.STATUS_FLAG))

                    Me.lbl_Apply_time.Text = Func_Unit(oTA_APPLY_SCREENING_COMM.FECHA_CREA, Date.UtcNow)

                    Me.Buttons_app.Attributes.Add("style", "display:none;")

                    If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 1 Then 'Peding

                        Me.grd_screening.DataSourceID = ""
                        Me.grd_screening.DataSource = dbEntities.VW_ASSESMENT_QUESTIONS.Where(Function(p) p.id_measurement_survey = id_Survey).OrderBy(Function(o) o.order_numberQU).ToList()
                        Me.grd_screening.DataBind()

                        Me.questions_prescreening.Attributes.Add("style", "display:block;")


                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5  disabled")

                        '    Me.SolicitationALERT.Attributes.Add("style", "display:block;")
                        '    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                        '    Me.btn_agregar.Enabled = False
                    ElseIf oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 2 Then 'Responded

                        Dim TotScore As Double = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).Sum(Function(p) p.percent_valueAO)
                        Dim TotPass As Double = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).FirstOrDefault.SCREENING_TOTALPASS

                        Me.lbl_score.Text = String.Format("{0:N4}", TotScore)
                        Me.hd_totScore.Value = TotScore
                        Dim strClassA As String = "badge bg-red"
                        Dim strClassB As String = "badge bg-blue-active"

                        If (TotPass > CDbl(Me.hd_totScore.Value)) Then

                            Me.span_score.Attributes.Add("Class", strClassA)
                            Me.spa_icons.InnerHtml = "<i class='fas fa-smile'></i>"
                            Me.lbl_score_status.Text = "Hasn't passed yet"

                        Else

                            Me.span_score.Attributes.Add("Class", strClassB)
                            Me.spa_icons.InnerHtml = "<i class='fas fa-frown'></i>"
                            Me.lbl_score_status.Text = "Passed"

                        End If


                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right")


                        Dim totEvalQ As Integer = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.id_measurement_question_eval <> 0).Count
                        Dim totEvalQ_answ As Integer = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.id_measurement_question_eval <> 0 And p.id_measurement_answer_option_eval.HasValue).Count

                        If totEvalQ = totEvalQ_answ Then

                            Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right disabled")
                            Me.btn_save_app.Attributes.Add("style", "display:none;")
                            Me.questions_prescreening.Attributes.Add("style", "display:none;")

                            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                            Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                            Me.rept_PrescreeningDates.DataBind()

                            Me.app_prescreening.Attributes.Add("style", "display:block;")
                            Me.Buttons_approve.Attributes.Add("style", "display:block;")

                        Else

                            Me.questions_prescreening.Attributes.Add("style", "display:block;")
                            Me.grd_answers.DataSourceID = ""
                            Me.grd_answers.DataSource = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
                            Me.grd_answers.DataBind()

                        End If


                    ElseIf oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 3 Or oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 4 Then 'PAssed

                        Me.btn_save_app.Attributes.Add("class", "btn btn-info btn-sm margin-r-5 pull-right disabled")
                        Me.btn_save_app.Attributes.Add("style", "display:none;")
                        Me.questions_prescreening.Attributes.Add("style", "display:none;")

                        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                        Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                        Me.rept_PrescreeningDates.DataBind()

                        Me.app_prescreening.Attributes.Add("style", "display:block;")
                        Me.Buttons_approve.Attributes.Add("style", "display:block;")

                        Me.btnlk_comment.Attributes.Add("class", "btn btn-default  btn-sm margin-r-5 pull-left disabled")
                        Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                        Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")

                    End If

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

                    'If oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 3 _
                    '     Or oTA_APPLY_SCREENING.FirstOrDefault.ID_SCREENING_STATUS = 4 Then
                    '    'PAssed Or not passed

                    '    Me.Buttons_approve.Attributes.Add("style", "display:block; padding-left:30px;")
                    '    Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)

                    '    'Me.rept_ApplyDates.DataSource = cls_Solicitation.get_Apply_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                    '    'Me.rept_ApplyDates.DataBind()

                    '    Me.rept_PrescreeningDates.DataSource = cls_Solicitation.get_Screening_Dates(oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING)
                    '    Me.rept_PrescreeningDates.DataBind()

                    'If oApply.FirstOrDefault.ID_APPLY_STATUS = 6 Then
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:block;")
                    'ElseIf oApply.FirstOrDefault.ID_APPLY_STATUS = 4 _
                    '        Or oApply.FirstOrDefault.ID_APPLY_STATUS = 5 Then
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left disabled")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                    'Else
                    '    Me.bntlk_accept.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_reject.Attributes.Add("class", "btn btn-danger btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_hold.Attributes.Add("class", "btn btn-warning btn-sm margin-r-5 pull-left")
                    '    Me.btnlk_Apply2.Attributes.Add("style", "display:none;")
                    'End If

                    'Else
                    '    Me.Buttons_approve.Attributes.Add("style", "display:none;")
                    '    Me.rept_ApplyDates.DataSource = Nothing
                    '    Me.rept_ApplyDates.DataBind()
                    'End If


                Else

                    Me.txt_apply_desc.Text = ""
                    'Me.cmb_Apply_status.SelectedValue = 1
                    Me.lbl_apply_status.Text = oSTATUS_APP
                    'Me.div2.Attributes.Remove("class")
                    'Me.div2.Attributes.Add("class", "alert-sm bg-red text-center")
                    Me.lbl_status_date.Text = dateUtil.set_DateFormat(oDate, "f", timezoneUTC, True)
                    Me.lbl_Apply_time.Text = Func_Unit(oDate, Date.UtcNow)
                    Me.spanSTATUS.Attributes.Remove("class")
                    Me.spanSTATUS.Attributes.Add("class", String.Format("badge {0} text-center", "badge-warning"))
                    Me.Buttons_app.Attributes.Add("style", "display:block;")
                    Me.SolicitationALERT.Attributes.Add("style", "display:none;")
                    Me.SolicitationRejected.Attributes.Add("style", "display:none;")
                    'Me.btn_agregar.Enabled = True


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

                Me.TabName.Value = "Applications"
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

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
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

            'Dim oSolicitationAPPLY As New TA_APPLY_APP
            'Dim oApplyComm As New TA_APPLY_COMM

            'Dim oTA_APPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app)
            'Dim AddComm As Boolean = False

            'If oTA_APPLY.Count() > 0 Then

            '    oSolicitationAPPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_solicitation_app).FirstOrDefault()
            '    oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
            '    '' oSolicitationAPPLY.ID_APPLY_STATUS = 1 'REgistered
            '    oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
            '    oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
            '    'oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            '    'oSolicitationAPPLY.fecha_crea = Date.UtcNow
            '    'oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            '    dbEntities.Entry(oSolicitationAPPLY).State = Entity.EntityState.Modified

            'Else

            '    oSolicitationAPPLY.ID_SOLICITATION_APP = id_solicitation_app
            '    oSolicitationAPPLY.ID_APPLY_STATUS = 2 'REgistered
            '    oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
            '    oSolicitationAPPLY.APPLY_DESCRIPTION = txt_apply_desc.Text
            '    oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            '    oSolicitationAPPLY.fecha_crea = Date.UtcNow
            '    oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            '    dbEntities.TA_APPLY_APP.Add(oSolicitationAPPLY)
            '    AddComm = True


            'End If

            'dbEntities.SaveChanges()


            ''***************************************************SET TA_ACTIVITY_STATUS************************************************************************
            ''Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            'Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            'cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 3)   'Solicitation / APPLY
            ''***************************************************SET TA_ACTIVITY_STATUS************************************************************************


            'If AddComm Then

            '    oApplyComm.ID_APPLY_APP = oSolicitationAPPLY.ID_APPLY_APP
            '    oApplyComm.ID_APPLY_STATUS = oSolicitationAPPLY.ID_APPLY_STATUS
            '    oApplyComm.APPLY_COMM = "Application Registered"
            '    oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            '    oApplyComm.FECHA_CREA = Date.UtcNow
            '    oApplyComm.COMM_BOL = 0

            '    dbEntities.TA_APPLY_COMM.Add(oApplyComm)
            '    dbEntities.SaveChanges()

            'End If

            'Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            'Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & id_solicitation_app.ToString()
            'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            ''document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            ''Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


            Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app)
            Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING

            Dim bndPASS As Boolean = True

            For Each row In Me.grd_answers.Items

                If TypeOf row Is GridDataItem Then

                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_answer_scale")
                    Dim ItemD As GridDataItem = CType(row, GridDataItem)
                    Dim answCODE As String = ItemD("answer_type_code").Text
                    'Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)

                    Dim ctrl_evaluate As RadComboBox = CType(row.Cells(0).FindControl("cmb_evaluate"), RadComboBox)
                    Dim ctrl_evaluateTEXT As RadTextBox = CType(row.Cells(0).FindControl("txt_colm_EVALUATION_QUESTION"), RadTextBox)
                    'Dim ctrl_answerVALUE As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)


                    If ctrl_evaluateTEXT.Text.Length > 2 Then

                        If Not IsNothing(ctrl_evaluate.SelectedValue) Then
                            If Val(ctrl_evaluate.SelectedValue) = 0 Then
                                bndPASS = False
                            End If
                        Else
                            bndPASS = False
                        End If

                    End If

                End If

            Next


            If bndPASS Then

                lblt_error.Visible = False

                Dim strHTML_2 As String = " <div class='box-body table-responsive no-padding'>  "

                strHTML_2 &= "<table class='table table-hover'>
                                        <tr>
                                          <th>No</th>
                                          <th>PreScreening Questions</th>
                                          <th>--</th>
                                          <th>Score</th>
                                          <th>Evaluation Question</th>
                                          <th>--</th>
                                          <th>Evaluation Score</th>
                                        </tr>"

                '***********************************************************************************************************



                Dim strRow_2 As String = "<tr>
                                          <th>{3}</th> 
                                          <td>{0}</td>
                                          <td>{1}</td>
                                          <td>{2:N4}</td>
                                          <td>{4}</td>
                                          <td>{5}</td>
                                          <td>{6:N4}</td>
                                        </tr>"



                Dim strRowTot_2 As String = ""


                Dim TotvalScore As Double = 0
                Dim valScore As Double = 0

                Dim idAnswerScale As Integer = 0
                Dim remaining_Score As Double = 0
                Dim remaining_answer As String = ""
                Dim totPendingScored As Double = 0
                Dim remaining_Question As String = ""
                Dim idOptionSelected As Integer = 0

                For Each row In Me.grd_answers.Items

                    If TypeOf row Is GridDataItem Then

                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim ItemD As GridDataItem = CType(row, GridDataItem)

                        Dim IDquestion_config As Integer = dataItem.GetDataKeyValue("id_measurement_question_config")
                        Dim IDquestion_configEVAL As Integer = dataItem.GetDataKeyValue("id_measurement_question_eval")

                        Dim IDAPPLY_SCREEN_ANSWER = CType(ItemD("ID_APPLY_SCREENING_ANSWER").Text, Int32)
                        Dim oTA_APPLY_SCREENING_ANSWER As TA_APPLY_SCREENING_ANSWER = dbEntities.TA_APPLY_SCREENING_ANSWER.Find(IDAPPLY_SCREEN_ANSWER)

                        Dim answCODE As String = ItemD("answer_type_code").Text

                        Dim ctrl_answer As RadComboBox = CType(row.Cells(0).FindControl("cmb_answer"), RadComboBox)
                        Dim ctrl_answerTEXT As RadTextBox = CType(row.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                        Dim ctrl_answerVALUE As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)

                        Dim ctrl_evaluate As RadComboBox = CType(row.Cells(0).FindControl("cmb_evaluate"), RadComboBox)
                        'oTA_APPLY_SCREENING_ANSWER.id_measurement_question_config = IDquestion_config
                        'oTA_APPLY_SCREENING_ANSWER.id_measurement_question_eval = IDquestion_configEVAL

                        If IDquestion_configEVAL <> 0 Then

                            oTA_APPLY_SCREENING_ANSWER.id_measurement_answer_option_eval = ctrl_evaluate.SelectedValue
                            dbEntities.Entry(oTA_APPLY_SCREENING_ANSWER).State = Entity.EntityState.Modified

                        End If

                        Dim strAnswer As String = ""


                        If answCODE = "DROPDOWN" Then

                            'oTA_APPLY_SCREENING_ANSWER.id_measurement_answer_option = ctrl_answer.SelectedValue
                            strAnswer = ctrl_answer.Text.Trim
                            idOptionSelected = CType(ctrl_answer.SelectedValue, Int32)
                            valScore = dbEntities.tme_measurement_answer_option.Find(idOptionSelected).percent_value


                        ElseIf answCODE = "TEXTENTRY" Then

                            'oTA_APPLY_SCREENING_ANSWER.measurement_answer_text = ctrl_answerTEXT.Text
                            strAnswer = ctrl_answerTEXT.Text.Trim
                            valScore = 0

                        ElseIf answCODE = "VALUEENTRY" Then

                            'oTA_APPLY_SCREENING_ANSWER.measurement_answer_value = ctrl_answerVALUE.Value
                            strAnswer = ctrl_answerVALUE.Value.ToString
                            valScore = 0

                        End If

                        'oTA_APPLY_SCREENING_ANSWER.ID_APPLY_SCREENING = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING
                        'dbEntities.TA_APPLY_SCREENING_ANSWER.Add(oTA_APPLY_SCREENING_ANSWER)

                        Dim oQUESTION = dbEntities.tme_measurement_question.Find(IDquestion_configEVAL)

                        If Not IsNothing(oQUESTION) Then
                            idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                            remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_option = ctrl_evaluate.SelectedValue).FirstOrDefault.percent_value
                            remaining_Question = oQUESTION.question_name
                            remaining_answer = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_option = ctrl_evaluate.SelectedValue).FirstOrDefault.option_name
                        Else
                            remaining_Score = 0
                            remaining_Question = ""
                            remaining_answer = ""
                        End If

                        totPendingScored += remaining_Score


                        strRowTot_2 &= String.Format(strRow_2, ItemD("question_name").Text.Trim, strAnswer, valScore, ItemD("order_numberQC").Text, remaining_Question, remaining_answer, remaining_Score)

                        TotvalScore += valScore
                        valScore = 0
                        idOptionSelected = 0
                        '{0:N2}
                        'If(ctrl_rbn_YESNO.SelectedValue = 0, "<span class='badge badge-danger'>NOT</span>", "<span class='badge badge-danger'>YES</span>")

                        '***********************************************************************************************************

                    End If

                Next



                Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)
                Dim oTA_SOLICITATION_APP = dbEntities.TA_SOLICITATION_APP.Find(id_solicitation_app)
                Dim idACTIVITY_SOL As Integer = oTA_SOLICITATION_APP.ID_ACTIVITY_SOLICITATION

                Dim idScreening As Double = dbEntities.TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = idACTIVITY_SOL).FirstOrDefault.ID_SCREENING
                Dim passValue As Double = dbEntities.TA_SCREENING.Find(idScreening).SCREENING_TOTALPASS

                Dim strTotScore As String = String.Format(If((TotvalScore + totPendingScored) >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), TotvalScore)
                Dim strtotPendingScored As String = String.Format(If((TotvalScore + totPendingScored) >= passValue, "<span class='badge badge-info'>{0:N4}</span>", "<span class='badge badge-danger'>{0:N4}</span>"), totPendingScored)

                strRowTot_2 &= String.Format("<tr>
                                              <td colspan='2' >{0}</td>
                                              <td>{1}</td>
                                              <td>{2}</td>
                                              <td></td>
                                              <td></td>
                                              <td>{3}</td>
                                             </tr>", "Total Score", "", strTotScore, strtotPendingScored)

                strHTML_2 &= strRowTot_2 & "</table></div>"

                Dim lengthB As Integer = strHTML_2.Length
                '***********************************************************************************************************

                If dbEntities.SaveChanges() Then

                    Dim strComment = "Prescreening evaluated" & "<br /><br />" & Trim(strHTML_2.Trim.Replace("  ", " ")).ToString().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                    Dim oScreeningComm = New TA_APPLY_SCREENING_COMM
                    oScreeningComm.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                    oScreeningComm.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                    oScreeningComm.SCREENING_COMM = strComment.Trim
                    oScreeningComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oScreeningComm.FECHA_CREA = Date.UtcNow
                    oScreeningComm.COMM_BOL = 0
                    oScreeningComm.SHOWN_MNGNT_TEAM = 1
                    oScreeningComm.COMM_RES = 1

                    dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm)
                    dbEntities.SaveChanges()


                    'If ((TotvalScore + totPendingScored) >= passValue) Then

                    'oTA_APPLY_SCREE.ID_SCREENING_STATUS = 3 'Passed
                    'oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
                    'dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

                    'Else

                    '    oTA_APPLY_SCREE.ID_SCREENING_STATUS = 4 'Did not Pass
                    '    oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
                    '    dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

                    'End If

                    'dbEntities.SaveChanges() 'Upgrading the Status
                    Dim strTotScore_ALL As String = String.Format(If((TotvalScore + totPendingScored) >= passValue, "<h3><span class='badge badge-info'>{0:N4}</span></h3>", "<h3><span class='badge badge-danger'>{0:N4}</span></h3>"), TotvalScore + totPendingScored)


                    If ((TotvalScore + totPendingScored) >= passValue) Then
                        strComment = "<h3><span class='badge badge-info'>The Prescreening result meets the required score <i class='fa fa-thumbs-up'></i></span></h3><br /><br />" & strTotScore_ALL
                    Else
                        strComment = "<h3><span class='badge badge-danger'>The Prescreening result did not meet the required score <i class='fa fa-thumbs-down'></i></span></h3><br /><br />" & strTotScore_ALL
                    End If


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

                    If dbEntities.SaveChanges() Then

                        oTA_APPLY_SCREE.PERCENT_VALUE = TotvalScore + totPendingScored
                        dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()

                        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & id_solicitation_app.ToString()
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


                        '**********************************SCREENNNIG SENT*******************************************************
                        'Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                        '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1015, regionalizacionCulture)
                        '    '**********************************SCREENNNIG SENT*******************************************************
                        '    cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING(id_solicitation_app)

                    End If

                    '***********************************************************************************************************


                End If



            Else

                lblt_error.Visible = True

            End If



        End Using


    End Sub


    Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


        Dim Dif_Porce As Double = porcDays - porcEDays
        Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

        Const c_label_danger As String = "badge-danger"
        Const c_label_warning As String = "badge-warning"
        Const c_label_primary As String = "badge-primary"
        Const c_label_success As String = "badge-success"

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
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        'If Set_Status(4, False) Then 'Accepted

        '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        '    '**********************************CHEMONICS PROCESSS*******************************************************
        '    Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1013, regionalizacionCulture)
        '    '**********************************CHEMONCS PROCESSS*******************************************************
        '    cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 4) '' ACCEPTED"

        '    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
        '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        'End If
        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app)
            Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING

            Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)

            oTA_APPLY_SCREE.ID_SCREENING_STATUS = 3 'Passed
            oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
            dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                Dim strComment = String.Format("Prescreening has been accepted with a score of {0:N4}", oTA_APPLY_SCREE.PERCENT_VALUE) & "<br /><br />" & Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " "))

                Dim oScreeningComm = New TA_APPLY_SCREENING_COMM
                oScreeningComm.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                oScreeningComm.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                oScreeningComm.SCREENING_COMM = strComment.Trim
                oScreeningComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                oScreeningComm.FECHA_CREA = Date.UtcNow
                oScreeningComm.COMM_BOL = 0
                oScreeningComm.SHOWN_MNGNT_TEAM = 0
                oScreeningComm.COMM_RES = 0

                dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm)
                dbEntities.SaveChanges()



                ''**********************************SCREENNNIG SENT*******************************************************
                Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1017, regionalizacionCulture)
                '**********************************SCREENNNIG SENT*******************************************************
                cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING_RESP(Id_solicitation_app)


                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


            End If

        End Using



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

    Private Sub rept_ApplyDates_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)

        'Handles rept_ApplyDates.ItemDataBound

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


                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
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


            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
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

            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End If

    End Sub

    Private Sub btnlk_reject_Click(sender As Object, e As EventArgs) Handles btnlk_reject.Click

        'Dim id_activity = Val(Me.lbl_id_ficha.Text)
        'Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)

        'If Set_Status(5, False) Then 'Rejected

        '    Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        '    '**********************************CHEMONICS PROCESSS*******************************************************
        '    Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_Programa, 1013, regionalizacionCulture)
        '    '**********************************CHEMONCS PROCESSS*******************************************************
        '    cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 5) '' REJECTED"



        '    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
        '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        'End If



        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim Id_solicitation_app = Convert.ToInt32(Me.lbl_id_sol_app.Text)
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        'If Set_Status(4, False) Then 'Accepted

        '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        '    '**********************************CHEMONICS PROCESSS*******************************************************
        '    Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        '    Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1013, regionalizacionCulture)
        '    '**********************************CHEMONCS PROCESSS*******************************************************
        '    cl_Noti_Process.NOTIFIYING_SOLICITATION_RESPONSE(Id_solicitation_app, 4) '' ACCEPTED"

        '    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
        '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        'End If
        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app)
            Dim idAPPLY_screening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING

            Dim oTA_APPLY_SCREE = dbEntities.TA_APPLY_SCREENING.Find(idAPPLY_screening)

            oTA_APPLY_SCREE.ID_SCREENING_STATUS = 4 'did not Pass
            oTA_APPLY_SCREE.DATE_ANSWERED = Date.UtcNow
            dbEntities.Entry(oTA_APPLY_SCREE).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                Dim strComment = String.Format("Pre Screening did not meet the requirements established, with a score of {0:N4}", oTA_APPLY_SCREE.PERCENT_VALUE) & "<br /><br />" & Trim(Me.Editor_approve_comments.Content.Trim.Replace("  ", " "))

                Dim oScreeningComm = New TA_APPLY_SCREENING_COMM
                oScreeningComm.ID_APPLY_SCREENING = oTA_APPLY_SCREE.ID_APPLY_SCREENING
                oScreeningComm.ID_SCREENING_STATUS = oTA_APPLY_SCREE.ID_SCREENING_STATUS
                oScreeningComm.SCREENING_COMM = strComment.Trim
                oScreeningComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oScreeningComm.FECHA_CREA = Date.UtcNow
                oScreeningComm.COMM_BOL = 0
                oScreeningComm.SHOWN_MNGNT_TEAM = 0
                oScreeningComm.COMM_RES = 0

                dbEntities.TA_APPLY_SCREENING_COMM.Add(oScreeningComm)
                dbEntities.SaveChanges()

                ''**********************************SCREENNNIG SENT*******************************************************
                Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
                Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1017, regionalizacionCulture)
                '**********************************SCREENNNIG SENT*******************************************************
                cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING_RESP(Id_solicitation_app)

                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityPrescreening?Id=" & id_activity.ToString & "&_tab=Applications" & "&_idAppnts=" & Id_solicitation_app.ToString()
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


            End If

        End Using

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

    Public Sub Load_grdApplicant(ByVal tbl_Applicants As DataTable)

        Me.grd_cate.DataSource = tbl_Applicants
        Me.grd_cate.DataBind()

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

    Private Sub grd_answers_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_answers.ItemDataBound



        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then


            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

                Dim ctrl_Screening_Question As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_colm_SCREENING_QUESTIONS"), RadTextBox)
                ctrl_Screening_Question.Text = DataBinder.Eval(e.Item.DataItem, "question_name")

                Dim ctrl_answer As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_answer"), RadComboBox)
                Dim ctrl_answerTEXT As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_answer_text"), RadTextBox)
                Dim ctrl_answerVALUE As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_answer_value"), RadNumericTextBox)

                Dim ctrl_score As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_score"), RadNumericTextBox)

                Dim ctrl_evaluation_Question As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_colm_EVALUATION_QUESTION"), RadTextBox)
                Dim ctrl_evaluation_answer As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_evaluate"), RadComboBox)

                Dim idScale = DataBinder.Eval(e.Item.DataItem, "id_measurement_answer_scale")
                Dim idQuestion_Eval = DataBinder.Eval(e.Item.DataItem, "id_measurement_question_eval")

                Dim oTME_Measurement = dbEntities.tme_measurement_answer_scale.Find(idScale)

                Dim answCODE As String = DataBinder.Eval(e.Item.DataItem, "answer_type_code")

                Dim idAnswerScale As Integer = 0
                Dim remaining_Score As Double = 0
                'Dim remaining_Question As String = ""
                Dim TotPASS As Double = DataBinder.Eval(e.Item.DataItem, "SCREENING_TOTALPASS")

                Dim oQUESTION = dbEntities.tme_measurement_question.Find(idQuestion_Eval)

                If Not IsNothing(oQUESTION) Then

                    idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                    remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).Max(Function(f) f.percent_value)
                    'remaining_Question = oQUESTION.question_name
                    ctrl_evaluation_Question.Text = oQUESTION.question_name
                    ctrl_evaluation_Question.ReadOnly = True

                    ctrl_evaluation_answer.DataSourceID = ""
                    ctrl_evaluation_answer.DataSource = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).Select(Function(s) _
                                                                                                   New With {Key .option_name = s.option_name,
                                                                                                              Key .id_measurement_answer_option = s.id_measurement_answer_option}).ToList()

                    ctrl_evaluation_answer.DataValueField = "id_measurement_answer_option"
                    ctrl_evaluation_answer.DataTextField = "option_name"
                    ctrl_evaluation_answer.DataBind()



                Else

                    remaining_Score = 0
                    'remaining_Question = ""
                    idAnswerScale = 0
                    ctrl_evaluation_Question.Text = ""
                    ctrl_evaluation_Question.ReadOnly = True
                    ctrl_evaluation_answer.DataSourceID = ""

                End If

                Me.hd_remaining.Value = CDbl(Me.hd_remaining.Value) + remaining_Score
                Me.lbl_remaining_score.Text = CDbl(Me.hd_remaining.Value)

                Dim strClassA As String = "badge bg-red"
                Dim strClassB As String = "badge bg-blue-active"

                If TotPASS > CDbl(Me.hd_remaining.Value) Then
                    Me.span_remaining.Attributes.Add("Class", strClassA)
                Else
                    Me.span_remaining.Attributes.Add("Class", strClassA)
                End If



                If answCODE = "DROPDOWN" Then

                    ctrl_answer.DataSourceID = ""
                    ctrl_answer.DataSource = oTME_Measurement.tme_measurement_answer_option.Select(Function(s) _
                                                                                                   New With {Key .option_name = s.option_name,
                                                                                                              Key .id_measurement_answer_option = s.id_measurement_answer_option}).ToList()

                    ctrl_answer.DataValueField = "id_measurement_answer_option"
                    ctrl_answer.DataTextField = "option_name"
                    ctrl_answer.DataBind()
                    ctrl_answer.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_answer_option")
                    ctrl_answer.Enabled = False

                    ctrl_score.Value = DataBinder.Eval(e.Item.DataItem, "percent_valueAO")
                    ctrl_score.ReadOnly = True

                    ctrl_answer.Visible = True
                    ctrl_answerTEXT.Visible = False
                    ctrl_answerVALUE.Visible = False

                ElseIf answCODE = "TEXTENTRY" Then

                    ctrl_answer.Visible = False
                    ctrl_answerTEXT.Visible = True
                    ctrl_answerTEXT.Text = DataBinder.Eval(e.Item.DataItem, "measurement_answer_text")
                    ctrl_answerTEXT.ReadOnly = True
                    ctrl_answerVALUE.Visible = False

                ElseIf answCODE = "VALUEENTRY" Then

                    ctrl_answer.Visible = False
                    ctrl_answerTEXT.Visible = False
                    ctrl_answerVALUE.Visible = True
                    ctrl_answerVALUE.Value = DataBinder.Eval(e.Item.DataItem, "measurement_answer_value")
                    ctrl_answerVALUE.ReadOnly = True

                End If


            End Using


        End If




    End Sub

    Private Sub rept_PrescreeningDates_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rept_PrescreeningDates.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rept_PrescreeningComm")
            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

            rept_Messages.DataSource = cls_Solicitation.get_Screenning_Comments_special(DataBinder.Eval(ItemD.DataItem, "ID_APPLY_SCREENING").ToString(), DataBinder.Eval(ItemD.DataItem, "date_created").ToString(), 1)
            rept_Messages.DataBind()

        End If


    End Sub
End Class