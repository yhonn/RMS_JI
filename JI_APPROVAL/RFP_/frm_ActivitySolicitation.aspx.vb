Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager
Imports System.Globalization

Public Class frm_ActivitySolicitation
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_SOLIC"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim valorSuma As Decimal = 0
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Public Property document_folder As String = ""
    Dim dtApplicants As New DataTable
    Dim dtMembers As New DataTable

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

        If Not Me.IsPostBack Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id

                Dim proyecto As New VW_TA_ACTIVITY


                If id > 0 Then
                    proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                    Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                Else
                    Me.lbl_informacionproyecto.Text = "(JI-XXX-XX-XX.XXX-XXXX-000)" + " New Solicitation"
                    proyecto = Nothing
                End If
                'loadListas(idPrograma, proyecto)
                'LoadData_code(id)

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString()))

                '' Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
                Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString()))
                Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString()))


                If Not IsNothing(proyecto) Then

                    'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                    Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)

                    If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                        Me.alink_funding.Attributes.Add("style", "display:block;")
                        Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                        Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                        Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & id.ToString()))
                        Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                        Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))

                    Else
                        Me.alink_funding.Attributes.Add("style", "display:none;")
                        Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                        Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                    End If


                Else

                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")


                End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Me.cmb_type_of_document.DataSourceID = ""
                Me.cmb_type_of_document.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
                Me.cmb_type_of_document.DataTextField = "nombre_documento"
                Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
                Me.cmb_type_of_document.DataBind()

                Me.cmb_Material_type.DataSourceID = ""
                Me.cmb_Material_type.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
                Me.cmb_Material_type.DataTextField = "nombre_documento"
                Me.cmb_Material_type.DataValueField = "id_doc_soporte"
                Me.cmb_Material_type.DataBind()

                Me.cmb_solicitation_type.DataSourceID = ""
                Me.cmb_solicitation_type.DataSource = cl_listados.get_TA_SOLICITATION_TYPE(idPrograma)
                Me.cmb_solicitation_type.DataTextField = "SOLICITATION_TYPE"
                Me.cmb_solicitation_type.DataValueField = "ID_SOLICITATION_TYPE"
                Me.cmb_solicitation_type.DataBind()

                Me.cmb_solicitation_status.DataSourceID = ""
                Me.cmb_solicitation_status.DataSource = cl_listados.get_TA_SOLICITATION_STATUS(idPrograma)
                Me.cmb_solicitation_status.DataTextField = "SOLICITATION_STATUS"
                Me.cmb_solicitation_status.DataValueField = "ID_SOLICITATION_STATUS"
                Me.cmb_solicitation_status.DataBind()

                Dim objRegion = cl_listados.get_t_regiones(Convert.ToInt32(idPrograma))

                Me.cmb_regionII.DataSourceID = ""
                Me.cmb_regionII.DataSource = objRegion
                Me.cmb_regionII.DataTextField = "nombre_region"
                Me.cmb_regionII.DataValueField = "id_region"
                Me.cmb_regionII.DataBind()
                Me.cmb_regionII.Items.Item(0).Selected = True

                Me.cmb_subregionII.DataSourceID = ""
                Me.cmb_subregionII.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_regionII.SelectedValue))
                Me.cmb_subregionII.DataTextField = "nombre_subregion"
                Me.cmb_subregionII.DataValueField = "id_subregion"
                Me.cmb_subregionII.DataBind()
                Me.cmb_subregionII.Items.Item(0).Selected = True


                Me.grd_subregionII.DataSource = ""
                Me.grd_subregionII.DataSource = dbEntities.vw_t_subregiones.Where(Function(p) p.id_programa = idPrograma).OrderBy(Function(p) p.nombre_region).ThenBy(Function(p) p.nombre_subregion).ToList()
                Me.grd_subregionII.DataBind()


                Me.cmb_periodo.DataSourceID = ""
                Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
                Me.cmb_periodo.DataTextField = "nombre_periodo"
                Me.cmb_periodo.DataValueField = "id_periodo"
                Me.cmb_periodo.DataBind()
                Me.cmb_periodo.Items.Item(0).Selected = True


                Dim entryPoint = From u In dbEntities.t_usuarios Join q In dbEntities.t_usuario_programa On u.id_usuario Equals q.id_usuario
                                 Where q.id_programa = idPrograma
                                 Select New With {Key .id_usuario = u.id_usuario, Key .nombre = u.nombre_usuario & " " & u.apellidos_usuario,
                                       Key .busqueda_actividad = u.t_usuario_programa.FirstOrDefault().busqueda_actividad}


                Me.cmb_persona_encargada.DataSourceID = ""
                Me.cmb_persona_encargada.DataSource = entryPoint.ToList()
                Me.cmb_persona_encargada.DataTextField = "nombre"
                Me.cmb_persona_encargada.DataValueField = "id_usuario"
                Me.cmb_persona_encargada.DataBind()

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                ' Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)                
                '' Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find()


                If id > 0 Then

                    Me.timeline_activity.ID_ACTIVITY = proyecto.id_activity
                    Me.txt_activity_code.Text = proyecto.codigo_ficha_AID
                    Me.txt_nombreproyecto.Text = proyecto.nombre_proyecto
                    Me.txt_descripcion.Text = proyecto.area_intervencion

                Else

                    Me.timeline_activity.ID_ACTIVITY = 0
                    Me.txt_activity_code.Text = "JI-XXX-XX-XX.XXX-XXXX-000"
                    Me.txt_nombreproyecto.Text = "New Solicitation"
                    Me.txt_descripcion.Text = "New Solicitation Desc"

                End If


                Dim oSolicitation As New List(Of TA_ACTIVITY_SOLICITATION)


                If id > 0 Then

                    oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.ID_ACTIVITY = proyecto.id_activity).ToList()

                    If oSolicitation.Count() > 0 Then

                        Check_SOLICITATION_Status(proyecto.id_activity) 'For closing properly

                        Me.lbl_id_sol.Text = oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION
                        Dim idActivitySolicitation As Integer = oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION
                        'Me.ADDons.Attributes.Add("style", "display:block;")

                        LoadSolicitation(oSolicitation.FirstOrDefault)

                        document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
                        Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
                        Me.grd_archivos.DataBind()

                        Dim idSOlicitationAPP = Convert.ToInt32(oSolicitation.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                        Me.grd_materials.DataSource = dbEntities.VW_TA_SOLICITATION_MATERIALS.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = idSOlicitationAPP).ToList()
                        Me.grd_materials.DataBind()

                        Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                        Me.grd_cate.DataSourceID = ""

                        Dim strSQL = String.Format("select A.ID_ORGANIZATION_APP, 
                                                     ltrim(rtrim(A.organization_type)) + ' || ' +  ltrim(rtrim(A.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(A.NAMEALIAS)) + ' || ' + ltrim(rtrim(A.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(A.PERSONNAME)) as Org_search , 
                                                    organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME  
                                                   from VW_TA_ORGANIZATION_APP  A
									             Left outer join TA_SOLICITATION_APP b on (a.ID_ORGANIZATION_APP = b.ID_ORGANIZATION_APP and b.ID_ACTIVITY_SOLICITATION = {1} )
                                                WHERE b.ID_ORGANIZATION_APP IS NULL 
                                                    AND A.ID_PROGRAMA = {2}
                                                    AND (ltrim(rtrim(a.organization_type)) + ' || ' +  ltrim(rtrim(a.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(a.NAMEALIAS)) + ' || ' + ltrim(rtrim(a.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(a.PERSONNAME)) like '%{0}%' )", " ", oSolicitation.FirstOrDefault().ID_ACTIVITY_SOLICITATION, idPrograma)

                        Me.SqlDataSource2.SelectCommand = strSQL

                        dtApplicants = get_Activity_Applicants(oSolicitation.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                        If dtApplicants.Rows.Count = 0 Then
                            createdtcolums(1)
                        End If

                        Session("dtApplicants") = dtApplicants
                        'Me.grd_cate.DataSource = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
                        'Me.grd_cate.DataSource = dtApplicants
                        'Me.grd_cate.DataBind()
                        Load_grdApplicant(dtApplicants)

                        LoadPrescreening(oSolicitation)

                    Else
                        Me.lbl_id_sol.Text = "0"
                        Me.SqlDataSource2.SelectCommand = Nothing
                        Me.grd_materials.DataSource = ""
                    End If


                Else
                    Me.lbl_id_sol.Text = "0"
                    Me.SqlDataSource2.SelectCommand = Nothing
                    Me.grd_materials.DataSource = ""
                End If



            End Using


        Else

            dtApplicants = Session("dtApplicants")
            dtMembers = Session(" dtMembers")

        End If

    End Sub


    Public Sub Load_grdApplicant(ByVal tbl_Applicants As DataTable, Optional bndRebind As Boolean = True)

        Me.grd_cate.DataSource = tbl_Applicants
        If bndRebind Then
            Me.grd_cate.DataBind()
        End If

    End Sub



    Public Sub Check_SOLICITATION_Status(ByVal id_activity As Integer)

        Dim date_Now As Date = Date.UtcNow
        Dim SolicitationSTATUS As Integer = 1 'CReated


        Using dbEntities As New dbRMS_JIEntities

            Dim id_activiy_solicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Where(Function(p) p.ID_ACTIVITY = id_activity).FirstOrDefault.ID_ACTIVITY_SOLICITATION
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
                                                    .modificated = p.modificated}).ToList()
            Dim cl_utl As New CORE.cls_util

            Return cl_utl.ConvertToDataTable(oSolicitation)

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
                                                    .TOT_APP_SELECTED = p.TOT_APP_SELECTED,
                                                    .ID_MEASUREMENT_SURVEY = p.id_measurement_survey,
                                                    .survey_name = p.survey_name
                                                   }).ToList()
            Dim cl_utl As New CORE.cls_util

            Return cl_utl.ConvertToDataTable(oROUNDS)

        End Using

    End Function



    Protected Sub LoadPrescreening(ByVal oSolicitation As List(Of TA_ACTIVITY_SOLICITATION))

        Using dbEntities As New dbRMS_JIEntities


            Me.cmb_prescreening.DataSourceID = ""
            Me.cmb_prescreening.DataSource = dbEntities.VW_TA_SCREENING.Select(Function(p) New With {Key .ID_SCREENING = p.ID_SCREENING,
                                                                                                         .SCREENING_NAME = p.SCREENING_NAME}).ToList
            Me.cmb_prescreening.DataTextField = "SCREENING_NAME"
            Me.cmb_prescreening.DataValueField = "ID_SCREENING"
            Me.cmb_prescreening.DataBind()



            '**************TA_APPLY_SCREENING
            Dim ID_ACTIVITY_SOLICITATION As Integer = oSolicitation.FirstOrDefault.ID_ACTIVITY_SOLICITATION
            Dim oTA_SOLICITATION_SCREENING = dbEntities.VW_TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = ID_ACTIVITY_SOLICITATION).ToList()

            Me.grd_Prescreening.DataSource = oTA_SOLICITATION_SCREENING
            Me.grd_Prescreening.DataBind()

            'If oTA_SOLICITATION_SCREENING.Count > 0 Then

            '    Dim id_SCREENING As Integer = oTA_SOLICITATION_SCREENING.FirstOrDefault.ID_SCREENING
            '    Dim oTA_SCREENING = dbEntities.TA_SCREENING.Where(Function(p) p.ID_SCREENING = id_SCREENING).FirstOrDefault

            '    Dim oTA_APPLY_SCREENING As New TA_APPLY_SCREENING
            '    oTA_APPLY_SCREENING.ID_SOLICITATION_APP = Id_solicitation_app
            '    oTA_APPLY_SCREENING.ID_MEASUREMENT_SURVEY = oTA_SCREENING.ID_MEASUREMENT_SURVEY
            '    oTA_APPLY_SCREENING.ID_SCREENING_STATUS = 1 'Pending
            '    oTA_APPLY_SCREENING.PERCENT_VALUE = 0
            '    oTA_APPLY_SCREENING.ID_ORGANIZATION_APP = OSolicitationAPP.ID_ORGANIZATION_APP
            '    oTA_APPLY_SCREENING.DATE_CREATED = Date.UtcNow
            '    oTA_APPLY_SCREENING.id_usuario_crea = cl_user.id_usr

            '    dbEntities.TA_APPLY_SCREENING.Add(oTA_APPLY_SCREENING)

            '    dbEntities.SaveChanges()

            'End If


        End Using

    End Sub



    Protected Sub LoadSolicitation(ByVal oSolicitation As TA_ACTIVITY_SOLICITATION)

        Using dbEntities As New dbRMS_JIEntities


            cmb_solicitation_type.SelectedValue = oSolicitation.ID_SOLICITATION_TYPE
            cmb_solicitation_status.SelectedValue = oSolicitation.ID_SOLICITATION_STATUS
            Me.lbl_COde.Text = oSolicitation.SOLICITATION_CODE
            Me.txt_solicitation.Text = oSolicitation.SOLICITATION
            Me.txt_modifications.Text = If(IsDBNull(oSolicitation.MODIFICATIONS), "", oSolicitation.MODIFICATIONS)

            Me.txt_tittle.Text = oSolicitation.SOLICITATION_TITLE
            Me.txt_purpose.Text = oSolicitation.SOLICITATION_PURPOSE
            '' oSolicitation.SOLICITATION_TOKEN
            'dt_fecha_inicio.SelectedDate = oSolicitation.start_date
            Me.HEval_StartDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", oSolicitation.start_date, oSolicitation.start_date)

            'Dim oFechaFin As DateTime = oSolicitation.end_date
            'dt_fecha_fin.SelectedDate = oFechaFin
            'Me.txt_hour.Value = DatePart(DateInterval.Hour, oFechaFin)
            'Me.txt_min.Value = DatePart(DateInterval.Minute, oFechaFin)
            Me.HEval_EndDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", oSolicitation.end_date, oSolicitation.end_date)

            Dim idACTsol = Convert.ToInt32(oSolicitation.ID_ACTIVITY_SOLICITATION)
            Dim oEval = dbEntities.TA_APPLY_EVALUATION.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = idACTsol)

            If (oEval.Count > 0) Then

                LoadEvaluation(oEval.FirstOrDefault())

            Else

                Me.HEvalSETT_StartDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", Date.Now, Date.Now)
                Me.HEvalSETT_EndDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", DateAdd("m", 1, Date.Now), DateAdd("m", 1, Date.Now))

                Me.HEvalROUND_StartDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", Date.Now, Date.Now)
                Me.HEvalROUND_EndDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", DateAdd("m", 1, Date.Now), DateAdd("m", 1, Date.Now))


            End If



            If oSolicitation.id_usuario_res.HasValue Then
                If oSolicitation.id_usuario_res > 0 Then
                    Me.cmb_persona_encargada.SelectedValue = oSolicitation.id_usuario_res
                Else
                    Me.cmb_persona_encargada.SelectedValue = ""
                End If
            End If

            Me.txt_email_to.Text = oSolicitation.email_to
            Me.txt_email_CC.Text = oSolicitation.email_cc


        End Using


    End Sub


    Public Sub LoadEvaluation(ByVal oEval As TA_APPLY_EVALUATION)

        Using dbEntities As New dbRMS_JIEntities

            Me.txt_guidLines.Text = oEval.EVALUATION_DESCRIPTION
            Me.HEvalSETT_StartDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", oEval.EVALUATION_START_DATE, oEval.EVALUATION_START_DATE)
            Me.HEvalSETT_EndDate.Value = String.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss}", oEval.EVALUATION_END_DATE, oEval.EVALUATION_END_DATE)
            Me.txt_rounds.Value = oEval.TOT_ROUNDS

            btn_add_round.Attributes.Add("href", "javascript:OpenRadWindowTool('');")

            Dim IDaCTsol = Convert.ToInt32(oEval.ID_ACTIVITY_SOLICITATION)
            Dim APPeval = Convert.ToInt32(oEval.ID_APPLY_EVALUATION)
            Dim oMembers = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = APPeval)

            dtMembers = get_Activity_Members(IDaCTsol)

            If dtMembers.Rows.Count = 0 Then
                createdtcolums(2)
            End If

            Session("dtMembers") = dtMembers
            Me.grd_team.DataSource = dtMembers
            Me.grd_team.DataBind()

            Dim TotRound = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = APPeval)
            Me.H_ROUND_ID.Value = TotRound.Count()
            Me.lbl_round.Text = String.Format("#{0}", (Val(Me.H_ROUND_ID.Value) + 1).ToString())

            If TotRound.Count() > 0 Then
                Me.grd_rounds.DataSource = get_Evaluation_Rounds(APPeval)
                Me.grd_rounds.DataBind()
            End If

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idApplyEval = Convert.ToInt32(oEval.ID_APPLY_EVALUATION)
            Dim oUsers = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval).Select(Function(p) p.ID_USER).ToList()

            Me.cmb_rol.DataSourceID = ""
            Me.cmb_rol.DataSource = dbEntities.vw_ta_roles_user_all.Where(Function(p) p.id_programa = idPrograma And p.id_type_role = 1 And Not oUsers.Contains(p.id_usuario)) _
                                .Select(Function(p) New With {Key .id_user = p.id_usuario,
                                                                  .user_name = p.nombre_usuario & " (" & p.descripcion_rol & ") - " & p.email_usuario}).ToList
            Me.cmb_rol.DataTextField = "user_name"
            Me.cmb_rol.DataValueField = "id_user"
            Me.cmb_rol.DataBind()

            Me.cmb_eval_document_type.DataSourceID = ""
            Me.cmb_eval_document_type.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
            Me.cmb_eval_document_type.DataTextField = "nombre_documento"
            Me.cmb_eval_document_type.DataValueField = "id_doc_soporte"
            Me.cmb_eval_document_type.DataBind()

            Me.cmb_voting_type.DataSourceID = ""
            Me.cmb_voting_type.DataSource = cl_listados.get_TA_VOTING_TYPE(idPrograma)
            Me.cmb_voting_type.DataTextField = "VOTING_TYPE"
            Me.cmb_voting_type.DataValueField = "ID_VOTING_TYPE"
            Me.cmb_voting_type.DataBind()

            Me.cmb_assessment.DataSourceID = ""
            Me.cmb_assessment.DataSource = dbEntities.VW_MEASUREMENT_SURVEY_CAT.Where(Function(p) p.id_programa = idPrograma).ToList()
            Me.cmb_assessment.DataTextField = "survey_name"
            Me.cmb_assessment.DataValueField = "id_measurement_survey"
            Me.cmb_assessment.DataBind()

            Me.EVAL_2.Attributes.Add("style", "display:block;")

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Me.grd_eval_Document.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "EVALUATION_SETUP_DOC").ToList()
            Me.grd_eval_Document.DataBind()

        End Using


    End Sub

    'Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
    '    Dim a = CType(sender, LinkButton)
    '    Me.identity.Text = a.Attributes.Item("data-identity").ToString()
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    'End Sub


    Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_ACTIVITY_ANNEX").ToString()

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_ACTIVITY_DOCUMENTS WHERE (ID_ACTIVITY_ANNEX = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
            Me.grd_archivos.DataBind()

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)
    End Sub
    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Using dbEntities As New dbRMS_JIEntities

                Dim idPrograma = Convert.ToInt32(Session("E_IDPrograma"))
                Dim document_sol_path = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path
                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = document_sol_path & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"

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

        Using dbEntities As New dbRMS_JIEntities

            For Each file As UploadedFile In AsyncUpload1.UploadedFiles

                Dim exten = file.GetExtension()
                Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim anexo As New TA_ACTIVITY_DOCUMENTS
                anexo.DOCUMENT_TITLE = Me.txt_document_tittle.Text
                anexo.DOCUMENT_NAME = nombreArchivo
                anexo.DOCUMENTROLE = "SOLICITATION_ANNEX"
                anexo.id_doc_soporte = cmb_type_of_document.SelectedValue
                anexo.ID_ACTIVITY = id_activity
                anexo.ID_ACTIVITY_SOLICITATION = id_solicitation
                anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                anexo.fecha_crea = Date.UtcNow
                anexo.visible = True
                anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_ACTIVITY_DOCUMENTS.Add(anexo)

                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path)
                file.SaveAs(Path + nombreArchivo)

            Next
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=Documents"
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

        Try



            Using dbEntities As New dbRMS_JIEntities

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

                Dim id_activity = Val(Me.lbl_id_ficha.Text)
                Dim id_solicitation = Val(Me.lbl_id_sol.Text)

                If id_activity = 0 Then '*************ADD THE ACTIVITY
                    id_activity = Save_Ficha()
                End If

                Dim oFechaFin As DateTime

                If id_activity > 0 Then

                    Dim oSolicitation As New TA_ACTIVITY_SOLICITATION

                    Dim date_Now As Date = Date.UtcNow
                    Dim SolicitationSTATUS As Integer = 1 'CReated

                    If date_Now >= Convert.ToDateTime(Me.HEval_StartDate.Value) And date_Now <= Convert.ToDateTime(Me.HEval_EndDate.Value) Then
                        SolicitationSTATUS = 2 'OPENED
                    ElseIf date_Now > Convert.ToDateTime(Me.HEval_EndDate.Value) Then
                        SolicitationSTATUS = 3 'CLOSED
                    Else
                        SolicitationSTATUS = 1  'CREATED
                    End If


                    If id_solicitation > 0 Then

                        oSolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_solicitation)

                        'oSolicitation.ID_ACTIVITY = id_activity
                        oSolicitation.ID_SOLICITATION_TYPE = cmb_solicitation_type.SelectedValue
                        oSolicitation.ID_SOLICITATION_STATUS = SolicitationSTATUS
                        oSolicitation.SOLICITATION_CODE = Me.lbl_COde.Text
                        oSolicitation.SOLICITATION = Me.txt_solicitation.Text.Trim
                        oSolicitation.MODIFICATIONS = Me.txt_modifications.Text.Trim


                        oSolicitation.SOLICITATION_TITLE = Me.txt_tittle.Text.Trim
                        oSolicitation.SOLICITATION_PURPOSE = Me.txt_purpose.Text.Trim
                        ''oSolicitation.SOLICITATION_TOKEN = Guid.Parse(cl_user.GenerateToken())
                        'oSolicitation.start_date = dt_fecha_inicio.SelectedDate
                        oSolicitation.start_date = Convert.ToDateTime(Me.HEval_StartDate.Value)

                        'Dim OFecha = Convert.ToDateTime(Me.HEval_StartDate.Value)
                        'oFechaFin = Convert.ToDateTime(String.Format("{0:d} {1}:{2}:00 ", dt_fecha_fin.SelectedDate, Me.txt_hour.Text.Trim, Me.txt_min.Text.Trim))
                        'oSolicitation.end_date = oFechaFin
                        oSolicitation.end_date = Convert.ToDateTime(Me.HEval_EndDate.Value)

                        oSolicitation.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oSolicitation.fecha_upd = Date.UtcNow
                        oSolicitation.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                        oSolicitation.id_usuario_res = Me.cmb_persona_encargada.SelectedValue
                        oSolicitation.email_to = Me.txt_email_to.Text
                        oSolicitation.email_cc = Me.txt_email_CC.Text



                        dbEntities.Entry(oSolicitation).State = Entity.EntityState.Modified

                    Else

                        oSolicitation.ID_ACTIVITY = id_activity
                        oSolicitation.ID_SOLICITATION_TYPE = cmb_solicitation_type.SelectedValue
                        oSolicitation.ID_SOLICITATION_STATUS = SolicitationSTATUS
                        oSolicitation.SOLICITATION_CODE = Me.lbl_COde.Text
                        oSolicitation.SOLICITATION = Me.txt_solicitation.Text.Trim
                        oSolicitation.MODIFICATIONS = Me.txt_modifications.Text.Trim
                        oSolicitation.SOLICITATION_TITLE = Me.txt_tittle.Text.Trim
                        oSolicitation.SOLICITATION_PURPOSE = Me.txt_purpose.Text.Trim
                        oSolicitation.SOLICITATION_TOKEN = Guid.Parse(cl_user.GenerateToken())
                        'oSolicitation.start_date = dt_fecha_inicio.SelectedDate
                        oSolicitation.start_date = Convert.ToDateTime(Me.HEval_StartDate.Value)

                        'oFechaFin = Convert.ToDateTime(String.Format("{0:d} {1}:{2}:00 ", dt_fecha_fin.SelectedDate, Me.txt_hour.Text.Trim, Me.txt_min.Text.Trim))
                        'oSolicitation.end_date = oFechaFin
                        oSolicitation.end_date = Convert.ToDateTime(Me.HEval_EndDate.Value)

                        oSolicitation.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oSolicitation.fecha_crea = Date.UtcNow
                        oSolicitation.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                        oSolicitation.id_usuario_res = Me.cmb_persona_encargada.SelectedValue
                        oSolicitation.email_to = Me.txt_email_to.Text
                        oSolicitation.email_cc = Me.txt_email_CC.Text

                        dbEntities.TA_ACTIVITY_SOLICITATION.Add(oSolicitation)

                    End If


                    dbEntities.SaveChanges()

                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
                    'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


                Else

                    Me.lblt_error.Text = "Error Saving Solicitation"

                End If

            End Using

        Catch ex As Exception

            Me.lblt_error.Text = ex.Message

        End Try

    End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)

        Using dbEntities As New dbRMS_JIEntities

            Dim oEvaluation = dbEntities.TA_APPLY_EVALUATION.Where(Function(P) P.ID_ACTIVITY_SOLICITATION = id_solicitation).FirstOrDefault()

            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            Dim idApplyEval = oEvaluation.ID_APPLY_EVALUATION

            'Dim oROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval)

            '*************************************NEW ROUNDS*********************************************
            Dim oROUNDS As New TA_EVALUATION_ROUNDS

            oROUNDS.ID_APPLY_EVALUATION = idApplyEval
            oROUNDS.ID_VOTING_TYPE = Val(Me.cmb_voting_type.SelectedValue)
            oROUNDS.ID_ROUND = Val(Me.H_ROUND_ID.Value) + 1
            oROUNDS.ROUND_START_DATE = Convert.ToDateTime(Me.HEvalROUND_StartDate.Value)
            oROUNDS.ROUND_END_DATE = Convert.ToDateTime(Me.HEvalROUND_EndDate.Value)
            oROUNDS.VOTES_MAX = Me.txt_tot_votes.Value
            oROUNDS.TIED_TOTAL = 0

            If Val(Me.cmb_voting_type.SelectedValue) = 1 Then 'By Score

                oROUNDS.ID_MEASUREMENT_SURVEY = Val(cmb_assessment.SelectedValue)
                oROUNDS.SCORE_BASE = Me.txt_min_score.Value

            End If

            oROUNDS.POINTS_TOTAL = Me.txt_total_points.Value
            oROUNDS.POINTS_MAX = Me.txt_max_points.Value
            oROUNDS.TOT_APP_SELECTED = Me.txt_app_tot.Value
            oROUNDS.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oROUNDS.FECHA_CREA = Date.UtcNow
            oROUNDS.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            'oEval.ID_ACTIVITY_SOLICITATION = id_solicitation
            'oEval.ID_EVALUATION_STATUS = 1 'REgistered
            'oEval.EVALUATION_START_DATE = Convert.ToDateTime(Me.HEvalSETT_StartDate.Value)
            'oEval.EVALUATION_END_DATE = Convert.ToDateTime(Me.HEvalSETT_EndDate.Value)
            'oEval.EVALUATION_DESCRIPTION = Me.txt_guidLines.Text.Trim
            dbEntities.TA_EVALUATION_ROUNDS.Add(oROUNDS)

            If dbEntities.SaveChanges() Then

                Dim TotRound = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval)
                oEvaluation.TOT_ROUNDS = TotRound.Count()

                If cls_Solicitation.SAVE_TA_APPLY_EVALUATION(oEvaluation, idApplyEval) Then

                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=Eval_Team"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                End If

            End If

            'oEval.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            'oEval.FECHA_UPDATE = Date.UtcNow
            'oEval.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)



        End Using


    End Sub

    Private Sub cmb_solicitation_type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_solicitation_type.SelectedIndexChanged


        If Val(Me.cmb_solicitation_type.SelectedValue) > 0 Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Me.lbl_COde.Text = cl_listados.CrearCodigoRFA(idPrograma, Me.cmb_solicitation_type.SelectedValue, 0)
            Me.lbl_COde.Visible = True

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " set_Calendars();", True)


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
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))


        strSQL = String.Format("select A.ID_ORGANIZATION_APP, 
                                    ltrim(rtrim(A.organization_type)) + ' || ' +  ltrim(rtrim(A.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(A.NAMEALIAS)) + ' || ' + ltrim(rtrim(A.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(A.PERSONNAME)) as Org_search , 
                                     organization_type, ORGANIZATIONNAME, NAMEALIAS,  ADDRESSCOUNTRYREGIONID, PERSONNAME  
                                       from VW_TA_ORGANIZATION_APP  A
									   Left outer join TA_SOLICITATION_APP b on (a.ID_ORGANIZATION_APP = b.ID_ORGANIZATION_APP and b.ID_ACTIVITY_SOLICITATION = {1} )
                                     WHERE b.ID_ORGANIZATION_APP IS NULL 
                                        AND A.ID_PROGRAMA = {2}
                                        AND (ltrim(rtrim(a.organization_type)) + ' || ' +  ltrim(rtrim(a.ORGANIZATIONNAME)) + ' || ' +  ltrim(rtrim(a.NAMEALIAS)) + ' || ' + ltrim(rtrim(a.ADDRESSCOUNTRYREGIONID)) + ' || ' +  ltrim(rtrim(a.PERSONNAME)) like {0} )", likeCondition, id_solicitation, idPrograma)

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

                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#ADDons a[href=""#Applicants""]').tab('show');", True)


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

    Sub createdtcolums(ByVal opt As Integer)


        If opt = 1 Then

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

        End If

        If opt = 2 Then

            dtMembers.Columns.Add("ID_SOLICITATION_EVALUATION_TEAM", GetType(Integer))
            dtMembers.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
            dtMembers.Columns.Add("ID_USER", GetType(Integer))
            dtMembers.Columns.Add("EVALUATION_ROLE", GetType(String))
            dtMembers.Columns.Add("id_rol", GetType(Integer))
            dtMembers.Columns.Add("nombre_rol", GetType(String))
            dtMembers.Columns.Add("descripcion_rol", GetType(String))
            dtMembers.Columns.Add("nombre_usuario", GetType(String))
            dtMembers.Columns.Add("usuario", GetType(String))
            dtMembers.Columns.Add("email_usuario", GetType(String))

        End If

        'dtApplicants.Columns.Add("id_organizacion_capacidad", GetType(Integer))
        'dtApplicants.Columns.Add("id_capacidad_mejorar", GetType(Integer))
        'dtApplicants.Columns.Add("id_tipo_capacidad_mejorar", GetType(Integer))


    End Sub

    Protected Sub Elimina_Elemento(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity_sol.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)

    End Sub

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

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
            ''hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())
            'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")


            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            visible.Checked = False
            visible.InputAttributes.Add("ID_SOLICITATION_APP", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())


            Dim visible_mod As New CheckBox
            visible_mod = CType(e.Item.FindControl("chkActivo_mod"), CheckBox)
            visible_mod.Checked = False
            visible_mod.InputAttributes.Add("ID_SOLICITATION_APP", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_APP").ToString())


            If Val(DataBinder.Eval(e.Item.DataItem, "ID_APP_STATUS").ToString()) > 1 Then
                hlnkDelete.Visible = False
                visible.Checked = True
            End If

            If DataBinder.Eval(e.Item.DataItem, "modificated").ToString() = "True" Then

                visible_mod.Checked = True

            End If

        End If


    End Sub


    Protected Sub SOLITICITATE_APP(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        ''Me.hd_id_doc_support.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
        Dim Id_solicitation_app = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_APP"))
        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        Dim boolSend_Invitation As Boolean = True
        Dim redirect_URL As String = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString

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
                    OSolicitationAPP.id_usuario_sent = Convert.ToInt32(Me.Session("E_IDUser"))
                    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        '********************************************************TA_SOLICITATION_SCREENING******************************************
                        '*********************************************HERE REGISTERED THE APPLY SCREENING*******************************************
                        '**************TA_APPLY_SCREENING
                        Dim ID_ACTIVITY_SOLICITATION As Integer = OSolicitationAPP.ID_ACTIVITY_SOLICITATION
                        Dim oTA_SOLICITATION_SCREENING = dbEntities.TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = ID_ACTIVITY_SOLICITATION)

                        If oTA_SOLICITATION_SCREENING.Count > 0 Then

                            Dim id_SCREENING As Integer = oTA_SOLICITATION_SCREENING.FirstOrDefault.ID_SCREENING
                            Dim oTA_SCREENING = dbEntities.TA_SCREENING.Where(Function(p) p.ID_SCREENING = id_SCREENING).FirstOrDefault

                            Dim oTA_APPLY_SCREENING As New TA_APPLY_SCREENING
                            oTA_APPLY_SCREENING.ID_SOLICITATION_APP = Id_solicitation_app
                            oTA_APPLY_SCREENING.ID_MEASUREMENT_SURVEY = oTA_SCREENING.ID_MEASUREMENT_SURVEY
                            oTA_APPLY_SCREENING.ID_SCREENING_STATUS = 1 'Pending
                            oTA_APPLY_SCREENING.PERCENT_VALUE = 0
                            oTA_APPLY_SCREENING.ID_ORGANIZATION_APP = OSolicitationAPP.ID_ORGANIZATION_APP
                            oTA_APPLY_SCREENING.DATE_CREATED = Date.UtcNow
                            oTA_APPLY_SCREENING.id_usuario_crea = cl_user.id_usr

                            dbEntities.TA_APPLY_SCREENING.Add(oTA_APPLY_SCREENING)

                            dbEntities.SaveChanges()

                        End If
                        '*********************************************HERE REGISTERED THE APPLY SCREENING*******************************************
                        'cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

                        Dim idSOL_TYPE As Integer = OSolicitationAPP.TA_ACTIVITY_SOLICITATION.ID_SOLICITATION_TYPE
                        Dim oSOLICITATION_TYPE = dbEntities.TA_SOLICITATION_TYPE.Find(idSOL_TYPE)

                        If oSOLICITATION_TYPE.SOLICITATION_ACRONY = "DI" Then 'Direct Invitation




                            boolSend_Invitation = False
                            DI_Generate_AW(OSolicitationAPP.ID_SOLICITATION_APP)
                            redirect_URL = "~/RFP_/frm_ActivityAW?Id=" & id_activity.ToString



                        End If

                    End If

                    ' Else
                    ' cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)

                End If

                If boolSend_Invitation Then
                    cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                End If

                ' Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                Me.MsgGuardar.Redireccion = redirect_URL
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using


        Catch ex As Exception

            Me.MsgGuardar.NuevoMensaje = ex.Message & Chr(13) & "  " & ex.InnerException.Message
            Me.MsgGuardar.Redireccion = ""
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


        End Try



        'Dim id_tp As Integer = Me.hd_id_doc_support.Value
        'Dim boolCHK_funded As Boolean = False

        'For Each Irow As GridDataItem In Me.grd_documentos.Items

        '    Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)


        '    If chkvisible.Checked = True Then

        '        If Irow("id_doc_soporte").Text = id_tp Then

        '            RadSync_NewFile.Enabled = True
        '            RadSync_NewFile.AllowedFileExtensions = Irow("extension").Text.Trim.Replace(" ", "").Split(",")
        '            'RadSync_NewFile.AllowedFileExtensions = Strings.Split("xls,doc,pdf,xlsx,docx", ",")
        '            RadSync_NewFile.MaxFileSize = (1024 * Convert.ToDouble(Irow("colm_max_size").Text) * 1000) ' 1MG
        '            boolCHK_funded = True
        '        Else
        '            chkvisible.Checked = False
        '        End If

        '    End If
        'Next


    End Sub



    Public Sub DI_Generate_AW(ByVal id_sol_app As Integer)

        Try

            Using dbEntities As New dbRMS_JIEntities

                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(id_sol_app)
                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                'cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 2)   'Created / Solicitation

                OSolicitationAPP.ID_APP_STATUS = 4 'Submitted
                OSolicitationAPP.SENT_DATE = Date.UtcNow
                OSolicitationAPP.id_usuario_sent = Convert.ToInt32(Me.Session("E_IDUser"))
                OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                OSolicitationAPP.RECEIVED_DATE = Date.UtcNow
                OSolicitationAPP.SUBMITTED_DATE = Date.UtcNow
                OSolicitationAPP.SUBMITTED = True
                OSolicitationAPP.id_usuario_submit = Convert.ToInt32(Me.Session("E_IDUser"))
                OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                If dbEntities.SaveChanges() Then

                    '****************************************************************************************************************************************************************************
                    Dim oSolicitationAPPLY As New TA_APPLY_APP
                    Dim oApplyComm As New TA_APPLY_COMM

                    ' Dim oTA_APPLY = dbEntities.TA_APPLY_APP.Where(Function(P) P.ID_SOLICITATION_APP = id_sol_app)
                    'Dim AddComm As Boolean = False
                    oSolicitationAPPLY.ID_SOLICITATION_APP = id_sol_app
                    oSolicitationAPPLY.ID_APPLY_STATUS = 4 'Accepted
                    oSolicitationAPPLY.APPLY_DATE = Date.UtcNow
                    oSolicitationAPPLY.APPLY_DESCRIPTION = "Auto applying by direct invitation process " & OSolicitationAPP.TA_ACTIVITY_SOLICITATION.SOLICITATION_CODE
                    oSolicitationAPPLY.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oSolicitationAPPLY.fecha_crea = Date.UtcNow
                    oSolicitationAPPLY.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.TA_APPLY_APP.Add(oSolicitationAPPLY)

                    If dbEntities.SaveChanges() Then '***************Comment 

                        oApplyComm.ID_APPLY_APP = oSolicitationAPPLY.ID_APPLY_APP
                        oApplyComm.ID_APPLY_STATUS = oSolicitationAPPLY.ID_APPLY_STATUS
                        oApplyComm.APPLY_COMM = "Auto accepting by direct invitation process " & OSolicitationAPP.TA_ACTIVITY_SOLICITATION.SOLICITATION_CODE
                        oApplyComm.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        oApplyComm.FECHA_CREA = Date.UtcNow
                        oApplyComm.COMM_BOL = 0

                        dbEntities.TA_APPLY_COMM.Add(oApplyComm)

                        If dbEntities.SaveChanges() Then

                            Dim oEval As New TA_APPLY_EVALUATION

                            oEval.ID_ACTIVITY_SOLICITATION = OSolicitationAPP.ID_ACTIVITY_SOLICITATION
                            oEval.ID_EVALUATION_STATUS = 3 'Finished
                            oEval.EVALUATION_START_DATE = Convert.ToDateTime(Me.HEvalSETT_StartDate.Value)
                            oEval.EVALUATION_END_DATE = Convert.ToDateTime(Me.HEvalSETT_EndDate.Value)
                            oEval.EVALUATION_DESCRIPTION = Me.txt_guidLines.Text.Trim
                            oEval.TOT_ROUNDS = 1
                            oEval.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            'oEval.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            oEval.FECHA_CREA = Date.UtcNow
                            'oEval.FECHA_UPDATE = Date.UtcNow
                            oEval.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                            Dim idApplyEval As Integer = 0

                            dbEntities.TA_APPLY_EVALUATION.Add(oEval)

                            'If cls_Solicitation.SAVE_TA_APPLY_EVALUATION(oEval, idApplyEval) Then
                            If dbEntities.SaveChanges() Then

                                '*********************************************************************************************************************************************************

                                Dim oTeams As New TA_SOLICITATION_EVALUATION_TEAM
                                ' Dim oAPPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation).FirstOrDefault()
                                Dim idAPP_EVA = oEval.ID_APPLY_EVALUATION
                                Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                Dim oRoles = dbEntities.vw_ta_roles_user_all.Where(Function(p) p.id_usuario = idUser).FirstOrDefault()

                                Dim strRol As String = "--none--"

                                If Not IsNothing(oRoles) Then
                                    strRol = oRoles.descripcion_rol
                                End If

                                ' oTeams.ID_ACTIVITY_SOLICITATION = id_solicitation
                                oTeams.ID_USER = idUser
                                oTeams.ID_APPLY_EVALUATION = idAPP_EVA
                                oTeams.EVALUATION_ROLE = strRol
                                oTeams.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                'oMaterial.id_usuario_upd =
                                oTeams.fecha_crea = Date.UtcNow
                                'oMaterial.fecha_upd
                                oTeams.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                                dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Add(oTeams)

                                If dbEntities.SaveChanges() Then

                                    '*************************************NEW ROUNDS*********************************************
                                    Dim oROUNDS As New TA_EVALUATION_ROUNDS

                                    oROUNDS.ID_APPLY_EVALUATION = oEval.ID_APPLY_EVALUATION
                                    oROUNDS.ID_VOTING_TYPE = 2 'Popularity
                                    oROUNDS.ID_ROUND = 1
                                    oROUNDS.ROUND_START_DATE = Convert.ToDateTime(Me.HEvalROUND_StartDate.Value)
                                    oROUNDS.ROUND_END_DATE = Convert.ToDateTime(Me.HEvalROUND_EndDate.Value)
                                    oROUNDS.VOTES_MAX = 1
                                    oROUNDS.TIED_TOTAL = 0
                                    oROUNDS.POINTS_TOTAL = 0
                                    oROUNDS.POINTS_MAX = 0
                                    oROUNDS.TOT_APP_SELECTED = 1
                                    oROUNDS.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                    oROUNDS.FECHA_CREA = Date.UtcNow
                                    oROUNDS.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                                    dbEntities.TA_EVALUATION_ROUNDS.Add(oROUNDS)

                                    If dbEntities.SaveChanges() Then


                                        Dim oEVALUATION_APP = New TA_EVALUATION_APP

                                        oEVALUATION_APP.ID_EVALUATION_ROUND = oROUNDS.ID_EVALUATION_ROUND
                                        oEVALUATION_APP.ID_APPLY_APP = oSolicitationAPPLY.ID_APPLY_APP
                                        oEVALUATION_APP.EVALUATION_START_DATE = Date.UtcNow
                                        oEVALUATION_APP.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oROUNDS.ID_VOTING_TYPE, True)
                                        oEVALUATION_APP.EVALUATION_SCORE = 0
                                        oEVALUATION_APP.EVALUATION_VOTES = 1
                                        oEVALUATION_APP.EVALUATION_UNTIED = 0
                                        oEVALUATION_APP.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                        oEVALUATION_APP.FECHA_CREA = Date.UtcNow
                                        oEVALUATION_APP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                                        oEVALUATION_APP.EVALUATION_END_DATE = Date.UtcNow
                                        oEVALUATION_APP.FECHA_UPDATE = Date.UtcNow
                                        oEVALUATION_APP.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                                        Dim idEVALapp As Integer
                                        If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP(oEVALUATION_APP, 0), idEVALapp) Then
                                            'Save Evaluation Comment

                                            Dim oTA_EVALUATION_APP_COMM As New TA_EVALUATION_APP_COMM

                                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                                            oTA_EVALUATION_APP_COMM.ROUND = oROUNDS.ID_ROUND
                                            oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Get_STATUS(0, oROUNDS.ID_VOTING_TYPE)

                                            Dim idORG As Integer = OSolicitationAPP.ID_ORGANIZATION_APP
                                            Dim oVW_TA_ORGANIZATION_APP = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_ORGANIZATION_APP = idORG).FirstOrDefault

                                            oTA_EVALUATION_APP_COMM.EVALUATION_COMM = String.Format("Evaluation ROUND #{0} opened for {1} ", oROUNDS.ID_ROUND, String.Format("( {0}-{1} )", oVW_TA_ORGANIZATION_APP.ORGANIZATIONNAME, oVW_TA_ORGANIZATION_APP.NAMEALIAS))
                                            oTA_EVALUATION_APP_COMM.SCORE = 0
                                            oTA_EVALUATION_APP_COMM.VOTE = 0
                                            oTA_EVALUATION_APP_COMM.POINTS = 0
                                            oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                            oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                                            oTA_EVALUATION_APP_COMM.COMM_BOL = False
                                            oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                                            If cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0) Then

                                                oTA_EVALUATION_APP_COMM = New TA_EVALUATION_APP_COMM '***ACCEPTED COMMENT***

                                                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP = idEVALapp
                                                oTA_EVALUATION_APP_COMM.ROUND = oROUNDS.ID_ROUND
                                                oTA_EVALUATION_APP_COMM.ID_EVALUATION_APP_STATUS = Get_STATUS(2, oROUNDS.ID_VOTING_TYPE, True)
                                                oTA_EVALUATION_APP_COMM.EVALUATION_COMM = "Application Auto-Accepting  by direct invitation process " & OSolicitationAPP.TA_ACTIVITY_SOLICITATION.SOLICITATION_CODE
                                                oTA_EVALUATION_APP_COMM.SCORE = 0
                                                oTA_EVALUATION_APP_COMM.VOTE = 1
                                                oTA_EVALUATION_APP_COMM.POINTS = 0
                                                oTA_EVALUATION_APP_COMM.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                                oTA_EVALUATION_APP_COMM.FECHA_CREA = Date.UtcNow
                                                oTA_EVALUATION_APP_COMM.COMM_BOL = False
                                                oTA_EVALUATION_APP_COMM.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                                                Dim num As Integer = 0
                                                If Integer.TryParse(cls_Solicitation.Save_TA_EVALUATION_APP_COMM(oTA_EVALUATION_APP_COMM, 0), num) Then 'Saving Accepted Status

                                                    SET_ACCEPT_STATUS(idEVALapp)

                                                    Generate_AWARD_APP(oEval.ID_APPLY_EVALUATION, 1, oSolicitationAPPLY.ID_APPLY_APP)

                                                End If '*******TA_EVALUATION_APP_COMM

                                            End If '*******TA_EVALUATION_APP_COMM

                                        End If '*****************TA_EVALUATION_APP

                                    End If '****************TA_EVALUATION_ROUNDS

                                End If '***********TA_SOLICITATION_EVALUATION_TEAM

                                '*********************************************************************************************************************************************************

                            End If '***************TA_APPLY_EVALUATION

                        End If '*******************TA_APPLY_COMM

                    End If '**********************TA_APPLY_APP

                    '****************************************************************************************************************************************************************************

                End If '************************TA_SOLICITATION_APP

            End Using

        Catch ex As Exception

            Me.MsgGuardar.NuevoMensaje = ex.Message & Chr(13) & "  " & ex.InnerException.Message
            Me.MsgGuardar.Redireccion = ""
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Try


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


            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
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



    Protected Sub SOLICITATE_MOD(ByVal sender As Object, ByVal e As System.EventArgs)

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
        Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(id_programa, 1016, regionalizacionCulture)
        '**********************************CHEMONCS PROCESSS*******************************************************

        Try

            Using dbEntities As New dbRMS_JIEntities

                Dim OSolicitationAPP = dbEntities.TA_SOLICITATION_APP.Find(Id_solicitation_app)

                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(id_programa, cl_user)
                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 2)   'Created / Solicitation

                If OSolicitationAPP.ID_APP_STATUS = 1 Then 'Çreated

                    OSolicitationAPP.ID_APP_STATUS = 2 'Sent it
                    OSolicitationAPP.SENT_DATE = Date.UtcNow
                    OSolicitationAPP.modificated = True
                    OSolicitationAPP.id_usuario_sent = Convert.ToInt32(Me.Session("E_IDUser"))
                    OSolicitationAPP.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified


                    If dbEntities.SaveChanges() Then

                        '********************************************************TA_SOLICITATION_SCREENING******************************************
                        '*********************************************HERE REGISTERED THE APPLY SCREENING*******************************************
                        '**************TA_APPLY_SCREENING
                        Dim ID_ACTIVITY_SOLICITATION As Integer = OSolicitationAPP.ID_ACTIVITY_SOLICITATION
                        Dim oTA_SOLICITATION_SCREENING = dbEntities.TA_SOLICITATION_SCREENING.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = ID_ACTIVITY_SOLICITATION)


                        If oTA_SOLICITATION_SCREENING.Count > 0 Then

                            Dim id_SCREENING As Integer = oTA_SOLICITATION_SCREENING.FirstOrDefault.ID_SCREENING
                            Dim oTA_SCREENING = dbEntities.TA_SCREENING.Where(Function(p) p.ID_SCREENING = id_SCREENING).FirstOrDefault

                            Dim oTA_APPLY_SCREENING As New TA_APPLY_SCREENING
                            oTA_APPLY_SCREENING.ID_SOLICITATION_APP = Id_solicitation_app
                            oTA_APPLY_SCREENING.ID_MEASUREMENT_SURVEY = oTA_SCREENING.ID_MEASUREMENT_SURVEY
                            oTA_APPLY_SCREENING.PERCENT_VALUE = 0
                            oTA_APPLY_SCREENING.ID_ORGANIZATION_APP = OSolicitationAPP.ID_ORGANIZATION_APP
                            oTA_APPLY_SCREENING.DATE_CREATED = Date.UtcNow
                            oTA_APPLY_SCREENING.id_usuario_crea = cl_user.id_usr

                            dbEntities.TA_APPLY_SCREENING.Add(oTA_APPLY_SCREENING)

                            dbEntities.SaveChanges()

                        End If
                        '*********************************************HERE REGISTERED THE APPLY SCREENING*******************************************

                        cl_Noti_Process.NOTIFIYING_SOLICITATION_MOD(Id_solicitation_app)


                    End If


                Else

                    OSolicitationAPP.modificated = True
                    dbEntities.Entry(OSolicitationAPP).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        cl_Noti_Process.NOTIFIYING_SOLICITATION_MOD(Id_solicitation_app)

                    End If

                End If

                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?Id=" & id_activity.ToString
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using


        Catch ex As Exception

        End Try





        'Dim id_tp As Integer = Me.hd_id_doc_support.Value
        'Dim boolCHK_funded As Boolean = False

        'For Each Irow As GridDataItem In Me.grd_documentos.Items

        '    Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)


        '    If chkvisible.Checked = True Then

        '        If Irow("id_doc_soporte").Text = id_tp Then

        '            RadSync_NewFile.Enabled = True
        '            RadSync_NewFile.AllowedFileExtensions = Irow("extension").Text.Trim.Replace(" ", "").Split(",")
        '            'RadSync_NewFile.AllowedFileExtensions = Strings.Split("xls,doc,pdf,xlsx,docx", ",")
        '            RadSync_NewFile.MaxFileSize = (1024 * Convert.ToDouble(Irow("colm_max_size").Text) * 1000) ' 1MG
        '            boolCHK_funded = True
        '        Else
        '            chkvisible.Checked = False
        '        End If

        '    End If
        'Next


    End Sub


    Private Sub grd_materials_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_materials.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim hlk_ref As New HyperLink
            hlk_ref = itemD("colm_template").FindControl("hlk_template")

            If Not DataBinder.Eval(e.Item.DataItem, "Template").ToString().Contains("--none--") Then
                hlk_ref.Text = DataBinder.Eval(e.Item.DataItem, "Template").ToString()
                hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            Else
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "#"
            End If

            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkSelected"), CheckBox)
            visible.Checked = False
            visible.InputAttributes.Add("ID_SOLICITATION_MATERIAL", DataBinder.Eval(e.Item.DataItem, "ID_SOLICITATION_MATERIAL").ToString())

            Dim txtCtrl As New RadTextBox
            txtCtrl = CType(e.Item.FindControl("txtMandatory"), RadTextBox)
            txtCtrl.ReadOnly = True
            If DataBinder.Eval(e.Item.DataItem, "MANDATORY").ToString() = "True" Then
                txtCtrl.Text = "Mandatory"
            Else
                txtCtrl.Text = "Optional"
            End If

            'Dim ImageDownload As New HyperLink
            'ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
            'ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
            'ImageDownload.Target = "_blank"

        End If

    End Sub

    Private Sub grd_materials_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles grd_materials.DeleteCommand

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_SOLICITATION_MATERIAL").ToString()

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_SOLICITATION_MATERIALS WHERE (ID_SOLICITATION_MATERIAL = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "SOLICITATION_ANNEX").ToList()
            Me.grd_archivos.DataBind()

        End Using

    End Sub

    Private Sub btn_add_material_Click(sender As Object, e As EventArgs) Handles btn_add_material.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)

        Using dbEntities As New dbRMS_JIEntities


            Dim oMaterial As New TA_SOLICITATION_MATERIALS
            Dim idMaterial As Integer = Val(Me.idSOLmaterial.Value)

            If idMaterial > 0 Then

                oMaterial = dbEntities.TA_SOLICITATION_MATERIALS.Find(idMaterial)
                'oMaterial.ID_ACTIVITY_SOLICITATION = id_solicitation
                oMaterial.DOCUMENT_TITLE = Me.txt_Material_Title.Text
                oMaterial.ID_DOC_SOPORTE = Me.cmb_Material_type.SelectedValue
                'oMaterial.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oMaterial.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oMaterial.MANDATORY = If(chk_data_in.Checked, True, False)
                'oMaterial.fecha_crea = Date.UtcNow
                oMaterial.fecha_upd = Date.UtcNow
                oMaterial.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.Entry(oMaterial).State = Entity.EntityState.Modified

            Else


                oMaterial.ID_ACTIVITY_SOLICITATION = id_solicitation
                oMaterial.DOCUMENT_TITLE = Me.txt_Material_Title.Text
                oMaterial.ID_DOC_SOPORTE = Me.cmb_Material_type.SelectedValue
                oMaterial.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oMaterial.id_usuario_upd =
                oMaterial.MANDATORY = If(chk_data_in.Checked, True, False)
                oMaterial.fecha_crea = Date.UtcNow
                'oMaterial.fecha_upd
                oMaterial.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_SOLICITATION_MATERIALS.Add(oMaterial)

            End If


            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=MAT_REQU "
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()

        End Using



    End Sub


    Protected Sub Edit_Entry(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim Id_solicitation_mat = Convert.ToInt32(chkSelect.InputAttributes.Item("ID_SOLICITATION_MATERIAL"))
        Me.idSOLmaterial.Value = Id_solicitation_mat

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))





        Try

            Using dbEntities As New dbRMS_JIEntities


                Dim oMaterial = dbEntities.TA_SOLICITATION_MATERIALS.Find(Id_solicitation_mat)
                Me.txt_Material_Title.Text = oMaterial.DOCUMENT_TITLE
                Me.cmb_Material_type.SelectedValue = oMaterial.ID_DOC_SOPORTE
                Me.chk_data_in.Checked = If(oMaterial.MANDATORY, True, False)
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#ADDons a[href=""#MAT_REQU""]').tab('show');MUST_input();", True)

            End Using


        Catch ex As Exception

        End Try


    End Sub

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
            ''txtCtrl.ReadOnly = True
            txtCtrl.Text = DataBinder.Eval(e.Item.DataItem, "EVALUATION_ROLE").ToString()

            'If DataBinder.Eval(e.Item.DataItem, "EVALUATION_ROLE").ToString() = "True" Then
            'Else
            '    txtCtrl.Text = "Optional"
            'End If

        End If



    End Sub

    Private Sub grd_team_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles grd_team.DeleteCommand

        Dim id_solicitation = Val(Me.lbl_id_sol.Text)
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_SOLICITATION_EVALUATION_TEAM").ToString()

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_SOLICITATION_EVALUATION_TEAM WHERE (ID_SOLICITATION_EVALUATION_TEAM = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            dtMembers = get_Activity_Members(id_solicitation)

            If dtMembers.Rows.Count = 0 Then
                createdtcolums(2)
            End If
            Session("dtMembers") = dtMembers
            Me.grd_team.DataSource = dtMembers
            Me.grd_team.DataBind()

            'Dim oUsers = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation).Select(Function(p) p.ID_USER).ToList()
            'Me.cmb_rol.DataSource = dbEntities.vw_ta_roles_user_all.Where(Function(p) p.id_programa = idPrograma And p.id_type_role = 1 And Not oUsers.Contains(p.id_usuario)) _
            '                                .Select(Function(p) New With {Key .id_user = p.id_usuario,
            '                                                                  .user_name = p.nombre_usuario & " (" & p.descripcion_rol & ") - " & p.email_usuario}).ToList
            Me.cmb_rol.DataTextField = "user_name"
            Me.cmb_rol.DataValueField = "id_user"
            Me.cmb_rol.DataBind()

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#ADDons a[href=""#Eval_Team""]').tab('show');", True)

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)

    End Sub



    Private Sub grd_round_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles grd_rounds.DeleteCommand

        Dim id_solicitation = Val(Me.lbl_id_sol.Text)
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_EVALUATION_ROUND").ToString()

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_EVALUATION_ROUNDS WHERE (ID_EVALUATION_ROUND= " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Dim oEval = dbEntities.TA_APPLY_EVALUATION.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation)
            Dim idAPPeval = Convert.ToInt32(oEval.FirstOrDefault.ID_APPLY_EVALUATION)


            Dim oRounds = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = idAPPeval).ToList().OrderBy(Function(o) o.FECHA_CREA)

            Dim cRound As Integer = 1
            For Each item In oRounds

                cnnME.Open()
                dm.CommandText = String.Format("UPDATE TA_EVALUATION_ROUNDS SET ID_ROUND = {0}  WHERE (ID_EVALUATION_ROUND={1}) ", cRound, item.ID_EVALUATION_ROUND)
                dm.ExecuteNonQuery()
                cnnME.Close()

                cRound += 1

            Next

            cRound -= 1
            cnnME.Open()
            dm.CommandText = String.Format("UPDATE TA_APPLY_EVALUATION SET TOT_ROUNDS = {0}  WHERE (ID_APPLY_EVALUATION={1}) ", cRound, idAPPeval)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Me.grd_rounds.DataSource = get_Evaluation_Rounds(idAPPeval)
            Me.grd_rounds.DataBind()

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", " $('#ADDons a[href=""#Eval_Team""]').tab('show');", True)

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)

    End Sub

    'Private Sub btn_add_round_Click(sender As Object, e As EventArgs) Handles btn_add_round.Click

    '    Dim funcion = "FuncModal_rounds()"
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)

    'End Sub

    Private Sub btn_save_eval_Click(sender As Object, e As EventArgs) Handles btn_save_eval.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)


        Dim oFechaFin As DateTime

        Using dbEntities As New dbRMS_JIEntities

            'Dim oApplyComm As New TA_APPLY_COMM
            Dim oEvaluation = dbEntities.TA_APPLY_EVALUATION.Where(Function(P) P.ID_ACTIVITY_SOLICITATION = id_solicitation)
            Dim AddComm As Boolean = False

            Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            Dim idApplyEval = 0

            Dim oEval As New TA_APPLY_EVALUATION

            If oEvaluation.Count() = 0 Then

                oEval.ID_ACTIVITY_SOLICITATION = id_solicitation
                oEval.ID_EVALUATION_STATUS = 1 'REgistered
                oEval.EVALUATION_START_DATE = Convert.ToDateTime(Me.HEvalSETT_StartDate.Value)
                oEval.EVALUATION_END_DATE = Convert.ToDateTime(Me.HEvalSETT_EndDate.Value)
                oEval.EVALUATION_DESCRIPTION = Me.txt_guidLines.Text.Trim

                oEval.TOT_ROUNDS = 0
                oEval.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oEval.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oEval.FECHA_CREA = Date.UtcNow
                'oEval.FECHA_UPDATE = Date.UtcNow
                oEval.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            Else

                idApplyEval = oEvaluation.FirstOrDefault().ID_APPLY_EVALUATION

                oEval.ID_ACTIVITY_SOLICITATION = id_solicitation
                'oEval.ID_EVALUATION_STATUS = 1 'REgistered
                oEval.EVALUATION_START_DATE = Convert.ToDateTime(Me.HEvalSETT_StartDate.Value)
                oEval.EVALUATION_END_DATE = Convert.ToDateTime(Me.HEvalSETT_EndDate.Value)
                oEval.EVALUATION_DESCRIPTION = Me.txt_guidLines.Text.Trim

                Dim TotRound = dbEntities.TA_EVALUATION_ROUNDS.Where(Function(p) p.ID_APPLY_EVALUATION = idApplyEval)

                oEval.TOT_ROUNDS = TotRound.Count()
                'oEval.ID_USUARIO_CREA = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oEval.ID_USUARIO_UPDATE = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oEval.FECHA_CREA = Date.UtcNow
                oEval.FECHA_UPDATE = Date.UtcNow
                oEval.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            End If

            If cls_Solicitation.SAVE_TA_APPLY_EVALUATION(oEval, idApplyEval) Then

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=Eval_Team"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End If

        End Using

    End Sub


    Protected Sub VotingTYPE_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)


        If Val(Me.cmb_voting_type.SelectedValue) > 0 Then

            Select Case Val(Me.cmb_voting_type.SelectedValue)

                Case 1 'By Score

                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False

                    Me.txt_min_score.Enabled = True
                    Me.cmb_assessment.Enabled = True

                Case 2 'By Popularity'
                    Me.txt_tot_votes.Enabled = True
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 3 'By Review
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 4 'By Points
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = True
                    Me.txt_max_points.Enabled = True
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 5 'By Negotiation
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

            End Select

        End If

    End Sub

    Private Sub btn_add_memebers_Click(sender As Object, e As EventArgs) Handles btn_add_memebers.Click


        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTeams As New TA_SOLICITATION_EVALUATION_TEAM
            Dim idUser As Integer = Val(Me.cmb_rol.SelectedValue)


            Dim oAPPLY_EVALUATION = dbEntities.TA_APPLY_EVALUATION.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_solicitation).FirstOrDefault()
            Dim idAPP_EVA = Convert.ToInt32(oAPPLY_EVALUATION.ID_APPLY_EVALUATION)

            ' oTeams.ID_ACTIVITY_SOLICITATION = id_solicitation
            oTeams.ID_USER = idUser
            oTeams.ID_APPLY_EVALUATION = idAPP_EVA
            oTeams.EVALUATION_ROLE = dbEntities.vw_ta_roles_user_all.Where(Function(p) p.id_usuario = idUser).First().descripcion_rol

            oTeams.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            'oMaterial.id_usuario_upd =
            oTeams.fecha_crea = Date.UtcNow
            'oMaterial.fecha_upd
            oTeams.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Add(oTeams)

            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=Eval_Team"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)



        End Using

    End Sub

    Private Sub btn_add_eval_document_Click(sender As Object, e As EventArgs) Handles btn_add_eval_document.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation = Val(Me.lbl_id_sol.Text)

        Using dbEntities As New dbRMS_JIEntities

            For Each file As UploadedFile In AsyncUpload2.UploadedFiles

                Dim exten = file.GetExtension()
                Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim anexo As New TA_ACTIVITY_DOCUMENTS
                anexo.DOCUMENT_TITLE = Me.txt_eval_title.Text
                anexo.DOCUMENT_NAME = nombreArchivo
                anexo.DOCUMENTROLE = "EVALUATION_SETUP_DOC"
                anexo.id_doc_soporte = cmb_eval_document_type.SelectedValue
                anexo.ID_ACTIVITY = id_activity
                anexo.ID_ACTIVITY_SOLICITATION = id_solicitation
                anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                anexo.fecha_crea = Date.UtcNow
                anexo.visible = True
                anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_ACTIVITY_DOCUMENTS.Add(anexo)

                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).Evaluation_documents_path)
                file.SaveAs(Path + nombreArchivo)

            Next
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & id_activity.ToString & "&_tab=Eval_Team"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()


        End Using

    End Sub

    Private Sub grd_eval_Document_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles grd_eval_Document.DeleteCommand
        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_ACTIVITY_ANNEX").ToString()

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_ACTIVITY_DOCUMENTS WHERE (ID_ACTIVITY_ANNEX = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Me.grd_eval_Document.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id_activity And p.visible.Value And p.DOCUMENTROLE = "EVALUATION_SETUP_DOC").ToList()
            Me.grd_eval_Document.DataBind()

        End Using
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

    Private Sub cmb_voting_type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)


        If Val(Me.cmb_voting_type.SelectedValue) > 0 Then

            Select Case Val(Me.cmb_voting_type.SelectedValue)

                Case 1 'By Score

                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False

                    Me.txt_min_score.Enabled = True
                    Me.cmb_assessment.Enabled = True

                Case 2 'By Popularity'
                    Me.txt_tot_votes.Enabled = True
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 3 'By Review
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 4 'By Points
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = True
                    Me.txt_max_points.Enabled = True
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

                Case 5 'By Negotiation
                    Me.txt_tot_votes.Enabled = False
                    Me.txt_total_points.Enabled = False
                    Me.txt_max_points.Enabled = False
                    Me.txt_min_score.Enabled = False
                    Me.cmb_assessment.Enabled = False

            End Select

            'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "showWindoww", "OpenRadWindowTool('',false)", True)

        End If


    End Sub


    Protected Sub cmb_regionII_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_regionII.SelectedIndexChanged

        Me.cmb_subregionII.DataSourceID = ""
        Me.cmb_subregionII.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_regionII.SelectedValue))
        Me.cmb_subregionII.DataTextField = "nombre_subregion"
        Me.cmb_subregionII.DataValueField = "id_subregion"
        Me.cmb_subregionII.DataBind()

        'If Not chk_todosII.Checked Then
        '    Me.grd_districtII.DataSource = ""
        '    Me.grd_districtII.DataSource = db.vw_t_village.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
        '    Me.grd_districtII.DataBind()
        'End If

        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()
        LoadDataSUBs()

    End Sub


    Protected Sub cmb_subregionII_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregionII.SelectedIndexChanged
        Dim sql = ""
        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()

        LoadDataSUBs()

    End Sub


    Sub LoadDataSUBs()

        '***TMP***

        Using db As New dbRMS_JIEntities

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim MEC = db.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = idPrograma And p.prefijo_mecanismo = "ACT").FirstOrDefault()

            Me.txt_activity_code.Text = cl_listados.CrearCodigoFichaACT(idPrograma, If(chk_todosII.Checked, -1, Me.cmb_subregionII.SelectedValue), MEC.id_mecanismo_contratacion, MEC.tme_sub_mecanismo.FirstOrDefault().id_sub_mecanismo, -1)

            Me.divCodigo.Visible = True
            Me.lbl_mensaje.Visible = True
            Me.lbl_mensaje.Text = Me.txt_activity_code.Text


        End Using

    End Sub


    Protected Function Save_Ficha() As Integer

        Using dbEntities As New dbRMS_JIEntities

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Dim oFicha = New TA_ACTIVITY

            oFicha.codigo_SAPME = Me.txt_activity_code.Text
            oFicha.codigo_ficha_AID = Me.txt_activity_code.Text
            oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
            oFicha.area_intervencion = Me.txt_descripcion.Text

            Dim id_subMEC = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = id_programa And p.prefijo_mecanismo = "ACT").FirstOrDefault().tme_sub_mecanismo.FirstOrDefault.id_sub_mecanismo

            oFicha.id_sub_mecanismo = id_subMEC
            ''oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
            oFicha.id_usuario_responsable = Me.cmb_persona_encargada.SelectedValue

            oFicha.fecha_inicio_proyecto = Convert.ToDateTime(Me.HEval_StartDate.Value)
            oFicha.fecha_fin_proyecto = Convert.ToDateTime(Me.HEval_EndDate.Value)

            oFicha.codigo_RFA = ""
            oFicha.codigo_MONITOR = ""


            oFicha.id_periodo = Me.cmb_periodo.SelectedValue
            oFicha.ID_ACTIVITY_STATUS = 1
            oFicha.id_programa = id_programa

            Dim idPrograma = id_programa
            Dim fechaReg = Date.Now
            Dim fehcaTasaCambio = ""
            If Month(fechaReg) > 9 Then
                fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                fechaReg = Convert.ToDateTime(fehcaTasaCambio)
            End If

            'String.Format(cl_user.regionalizacionCulture, "{1} {0:N2}",dtRow("valor"),cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol)
            Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()

            If oTasaCambio.Count > 0 Then

                oFicha.tasa_cambio = oTasaCambio.FirstOrDefault().tasa_cambio

            Else
                oFicha.tasa_cambio = 0

            End If

            oFicha.costo_total_proyecto = 0.0
            oFicha.costo_total_proyecto_LOC = 0.0

            oFicha.id_usuario_creo = Me.Session("E_IdUser").ToString()
            oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
            oFicha.datecreated = Date.UtcNow
            oFicha.dateUpdate = Date.UtcNow
            oFicha.id_subregion = Me.cmb_subregionII.SelectedValue
            oFicha.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            oFicha.id_ficha_padre = Nothing

            ''******************THINKING ABOUT***************************
            '' oFicha.tme_ficha_historico_estado.Add(cl_listados.createFichaHistorico(1, cl_user.id_usr))

            dbEntities.TA_ACTIVITY.Add(oFicha)
            dbEntities.SaveChanges()

            Dim boolAdded As Boolean = False

            If chk_todosII.Checked Then
                For Each row In Me.grd_subregionII.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                        If subR.Checked = True Then
                            Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                            Dim oSubregion = New TA_ACTIVITY_SUBREGION
                            oSubregion.id_subregion = idSubregion
                            oSubregion.id_activity = oFicha.id_activity
                            oSubregion.nivel_cobertura = nivel_cobertura.Value
                            oFicha.TA_ACTIVITY_SUBREGION.Add(oSubregion)

                            boolAdded = True

                        End If
                    End If
                Next
            End If

            If Not boolAdded Then

                Dim oSubregion = New TA_ACTIVITY_SUBREGION
                oSubregion.id_subregion = Me.cmb_subregionII.SelectedValue
                oSubregion.id_activity = oFicha.id_activity
                oSubregion.nivel_cobertura = 100
                oFicha.TA_ACTIVITY_SUBREGION.Add(oSubregion)

            End If

            If dbEntities.SaveChanges() Then
                Me.lbl_id_ficha.Text = oFicha.id_activity
                Save_Ficha = oFicha.id_activity
            Else
                Save_Ficha = 0
            End If

        End Using

    End Function



    Protected Sub chk_todosII_CheckedChanged(sender As Object, e As EventArgs) Handles chk_todosII.CheckedChanged
        If chk_todosII.Checked Then
            Me.grd_subregionII.Visible = True
            Me.cmb_subregionII.Visible = False
        Else
            Me.grd_subregionII.Visible = False
            Me.cmb_subregionII.Visible = True
        End If
        LoadDataSUBs()
    End Sub


End Class