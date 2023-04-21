Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports ly_RMS


Public Class frm_ActivityAW
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_ACTIVITY_AWARD"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim valorSuma As Decimal = 0
    Dim cero = False
    Dim dateUtil As ly_APPROVAL.APPROVAL.cls_dUtil
    Dim timezoneUTC As Integer
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim db As New ly_SIME.dbRMS_JIEntities

    Dim dtDocuments As New DataTable

    Public Property Award_document_folder As String = ""

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


        If Not Me.IsPostBack Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = cl_listados.get_t_programas(idPrograma)
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.Enabled = False

            'Me.lbl_id_sesion_temp.Text = cl_listados.CodigoRandom()
            Using dbEntities As New dbRMS_JIEntities

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id

                Dim id_aw As Integer = 0

                If Not IsNothing(Me.Request.QueryString("Id_AW")) Then
                    id_aw = Convert.ToInt32(Val(Me.Request.QueryString("Id_AW").ToString))
                End If

                Me.lbl_id_ficha_aw.Text = id_aw

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                'Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString()))


                Dim oVW_TA_AWARDED_APP As New VW_TA_AWARDED_APP


                If id_aw > 0 Then
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw).FirstOrDefault()
                Else
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()
                End If




                If Not IsNothing(oVW_TA_AWARDED_APP) Then


                    Me.lbl_id_award_app.Text = oVW_TA_AWARDED_APP.ID_AWARDED_APP

                    Dim proyecto As New VW_TA_AWARDED_ACTIVITY

                    If id_aw > 0 Then
                        proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw).FirstOrDefault()
                        '= dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw).FirstOrDefault()
                    Else
                        'dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()
                        proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()
                    End If



                    Me.lbl_informacionProyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

                    Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_APP

                    loadLists(idPrograma)

                    ''LoadData_code(id)

                    'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                    Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)
                    If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                        Me.alink_funding.Attributes.Add("style", "display:block;")
                        Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                        Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                    Else

                        Me.alink_funding.Attributes.Add("style", "display:none;")
                        Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                        Me.alink_INDICATORS.Attributes.Add("style", "display:none;")

                    End If

                    'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                    'Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)
                    'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                    '    Me.alink_stos.Attributes.Add("style", "display:none;")
                    'End If
                    'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                    Me.timeline_activity.ID_ACTIVITY = Me.lbl_id_ficha.Text

                    'Dim oTA_AWARDED_APP = dbEntities.TA_AWARDED_APP.Find(oVW_TA_AWARDED_APP.ID_AWARDED_APP)
                    'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP)

                    '****************************************************************************************************************************
                    '****************************************************************************************************************************
                    '****************************************************************************************************************************

                    'Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()
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

                    '****************************************************************************************************************************
                    '****************************************************************************************************************************
                    '****************************************************************************************************************************

                    loadActivity(idPrograma, proyecto)
                    'loadAWARD(Me.lbl_id_ficha.Text)
                    loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_APP, proyecto.ID_AWARDED_ACTIVITY)

                    LoadACTIVITIES(oVW_TA_AWARDED_APP.ID_AWARDED_APP)

                End If


            End Using



        End If
    End Sub



    Sub set_links(ByVal idACT As Integer, ByVal id_aw As Integer)

        Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))

        Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))

    End Sub

    Sub loadAWARD(ByVal id_awarded_app As Integer, ByVal id_aw As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_AWARDED_APP = dbEntities.TA_AWARDED_APP.Find(id_awarded_app)
            Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            Dim PercentProgress As Double

            If Not IsNothing(oTA_AWARDED_APP) Then

                Me.LBL_ID_AWARD.Text = oTA_AWARDED_APP.ID_AWARDED_APP

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_activity = idActivity).FirstOrDefault()
                Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw)

                Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(idACT)

                Me.lbl_implementer.Text = oVW_TA_ACTIVITY.FirstOrDefault().ORGANIZATIONNAME
                Me.lbl_activity_name.Text = oVW_TA_ACTIVITY.FirstOrDefault().nombre_proyecto
                Me.lbl_activity_Code.Text = oTA_AWARDED_APP.AWARD_CODE

                Me.lbl_last_Deliverable.Text = oTA_AWARDED_APP.TA_AWARD_STATUS.AWARD_STATUS


                Me.lbl_totalACT2.Text = String.Format("{0:N2} USD", oTA_ACTIVITY.costo_total_proyecto)
                Me.lbl_totalACT2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_ACTIVITY.costo_total_proyecto_LOC, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                'proyectos.Sum(Function(p) p.tme_AportesFicha.Sum(Function(q) q.monto_aporte_obligado))
                PercentProgress = If(oTA_ACTIVITY.costo_total_proyecto > 0, (oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT) / oTA_ACTIVITY.costo_total_proyecto), 0) * 100
                Me.hd_percent_sol.Value = PercentProgress

                'If PercentProgress > 100 Then
                '    PercentProgress = 100
                'End If

                Me.lbl_totalPerf2.Text = String.Format("{0:N2} USD", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT))
                Me.lbl_totalPerf2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT_LOC), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)


                Me.lbl_period.Text = String.Format("{0:dd/MM/yyyy} to {1:dd/MM/yyyy}", oVW_TA_ACTIVITY.FirstOrDefault().fecha_inicio_proyecto, oVW_TA_ACTIVITY.FirstOrDefault().fecha_fin_proyecto)

                Me.lbl_id_activity_award.Text = oVW_TA_ACTIVITY.FirstOrDefault().ID_AWARDED_ACTIVITY

                set_links(idACT, oVW_TA_ACTIVITY.FirstOrDefault().ID_AWARDED_ACTIVITY)


                Dim oORGANIZATION = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_ORGANIZATION_APP = oTA_AWARDED_APP.ID_ORGANIZATION_APP).FirstOrDefault()

                'Me.lbl_award_code.Text = oVW_TA_ACTIVITY.FirstOrDefault().codigo_SAPME

                If oTA_AWARDED_APP.ID_AWARD_STATUS = 2 Then
                    Me.lbl_award_code.Text = oTA_AWARDED_APP.AWARD_CODE
                End If

                Dim OStatus = dbEntities.TA_AWARD_STATUS.Find(oTA_AWARDED_APP.ID_AWARD_STATUS)

                Me.lbl_apply_status.Text = OStatus.AWARD_STATUS
                Me.lbl_status_date.Text = dateUtil.set_DateFormat(oTA_AWARDED_APP.FECHA_CREA, "f", timezoneUTC, True)
                Me.spanSTATUS.Attributes.Remove("class")
                Me.spanSTATUS.Attributes.Add("class", String.Format("label {0} text-center", OStatus.STATUS_FLAG))

                Me.lbl_Apply_time.Text = Func_Unit(oTA_AWARDED_APP.FECHA_CREA, Date.UtcNow)

                Me.lbl_organization.Text = String.Format("{0} ({1})", oORGANIZATION.ORGANIZATIONNAME, oORGANIZATION.NAMEALIAS)

                Me.lbl_status_date.Text = dateUtil.set_DateFormat(oTA_AWARDED_APP.FECHA_CREA, "f", timezoneUTC, True)

                Me.txt_Exchange_Rate_2.Value = oTA_AWARDED_APP.EXCHANGE_RATE

                '' Me.cmb_budget.SelectedValue = 
                'oFicha.costo_total_proyecto = Me.txt_tot_amount.Value + Me.txt_leaveraged_local.Value
                'Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.OBLIGATED_AMOUNT

                ''************************************************************AWARD****************************************************************************************************
                'Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.costo_total_proyecto
                'Me.txt_tot_activity_amount_LOC.Value = oVW_TA_ACTIVITY.costo_total_proyecto_LOCAL

                'Me.txt_obligated_usd.Value = If(IsNothing(oVW_TA_ACTIVITY.OBLIGATED_AMOUNT), oVW_TA_ACTIVITY.costo_total_proyecto, oVW_TA_ACTIVITY.OBLIGATED_AMOUNT)

                'Me.txt_obligated_local.Value = If(IsNothing(oVW_TA_ACTIVITY.OBLIGATED_AMOUNT_LOCAL), oVW_TA_ACTIVITY.costo_total_proyecto_LOCAL, oVW_TA_ACTIVITY.OBLIGATED_AMOUNT_LOCAL)
                'Me.txt_leaveraged_usd.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT), 0, (oVW_TA_ACTIVITY.costo_total_proyecto - oTA_AWARDED_APP.TOTAL_AMOUNT))
                'Me.txt_leaveraged_local.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT_LOC), 0, (oVW_TA_ACTIVITY.costo_total_proyecto_LOCAL - oTA_AWARDED_APP.TOTAL_AMOUNT_LOC))
                ''************************************************************AWARD****************************************************************************************************

                '************************************************************AWARD****************************************************************************************************


                'Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto)
                'Me.txt_tot_activity_amount_LOC.Value = oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto_LOCAL)

                Me.txt_tot_activity_amount.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT), If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT), oTA_AWARDED_APP.TOTAL_AMOUNT + If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT))
                Me.txt_tot_activity_amount_LOC.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT_LOC), If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT_LOC), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT_LOC), oTA_AWARDED_APP.TOTAL_AMOUNT_LOC + If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT_LOC))

                'Me.txt_obligated_usd.Value = If(IsNothing(oVW_TA_ACTIVITY.Sum(Function(p) p.OBLIGATED_AMOUNT)), oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto), oVW_TA_ACTIVITY.Sum(Function(p) p.OBLIGATED_AMOUNT))
                'Me.txt_obligated_local.Value = If(IsNothing(oVW_TA_ACTIVITY.Sum(Function(p) p.OBLIGATED_AMOUNT_LOCAL)), oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto_LOCAL), oVW_TA_ACTIVITY.Sum(Function(p) p.OBLIGATED_AMOUNT_LOCAL))

                Me.txt_obligated_usd.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT), 0, oTA_AWARDED_APP.TOTAL_AMOUNT)
                Me.txt_obligated_local.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT), 0, oTA_AWARDED_APP.TOTAL_AMOUNT_LOC)

                'Me.txt_leaveraged_usd.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT), 0, (oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto) - oTA_AWARDED_APP.TOTAL_AMOUNT))
                'Me.txt_leaveraged_local.Value = If(IsNothing(oTA_AWARDED_APP.TOTAL_AMOUNT_LOC), 0, (oVW_TA_ACTIVITY.Sum(Function(p) p.costo_total_proyecto_LOCAL) - oTA_AWARDED_APP.TOTAL_AMOUNT_LOC))

                Me.txt_leaveraged_usd.Value = If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT)
                Me.txt_leaveraged_local.Value = If(IsNothing(oTA_AWARDED_APP.LEAVERAGED_AMOUNT_LOC), 0, oTA_AWARDED_APP.LEAVERAGED_AMOUNT_LOC)

                '************************************************************AWARD****************************************************************************************************


                ' Me.txt_tot_activity_amount_LOC.Value = oVW_TA_ACTIVITY.costo_total_proyecto * oTA_AWARDED_APP.FirstOrDefault.EXCHANGE_RATE

                'oTA_AWARDED_APP.FirstOrDefault.TOTAL_AMOUNT
                'Me.txt_obligated_local.Value = oVW_TA_ACTIVITY.OBLIGATED_AMOUNT_LOCAL
                ' Me.txt_obligated_local.Value = oTA_AWARDED_APP.FirstOrDefault.TOTAL_AMOUNT * oTA_AWARDED_APP.FirstOrDefault.EXCHANGE_RATE

                ' Me.txt_leaveraged_local.Value = (oVW_TA_ACTIVITY.costo_total_proyecto_LOCAL - oTA_AWARDED_APP.FirstOrDefault.TOTAL_AMOUNT_LOC)
                'Me.txt_leaveraged_local.Value = (oVW_TA_ACTIVITY.costo_total_proyecto - oTA_AWARDED_APP.FirstOrDefault.TOTAL_AMOUNT) * oTA_AWARDED_APP.FirstOrDefault.EXCHANGE_RATE

                Dim OTA_APPLY_APP = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_APPLY_APP = oTA_AWARDED_APP.ID_APPLY_APP).FirstOrDefault()

                Me.lbl_id_sol_app.Text = OTA_APPLY_APP.ID_SOLICITATION_APP
                dtDocuments = get_Apply_Documents(OTA_APPLY_APP.ID_SOLICITATION_APP)

                cmb_budget.SelectedValue = oTA_AWARDED_APP.ID_BUDGET

                Me.cmb_currency.SelectedValue = oTA_AWARDED_APP.id_programa_currency

                Me.cmb_mecanismo_contratacion2.SelectedValue = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = oTA_AWARDED_APP.ID_SUB_MECANISMO).FirstOrDefault.id_mecanismo_contratacion
                Me.cmb_sub_mecanismo_contratacion2.SelectedValue = oTA_AWARDED_APP.ID_SUB_MECANISMO

                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "APPLY_ANNEX").ToList()
                Me.grd_archivos.DataSource = dtDocuments
                Me.grd_archivos.DataBind()

                Me.lbl_last_update.Text = If(oTA_AWARDED_APP.ACTIVITY_LAST_UPDATED.HasValue, dateUtil.set_DateFormat(oTA_AWARDED_APP.ACTIVITY_LAST_UPDATED, "f", timezoneUTC, True), "--")
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                Dim strUser = "--"

                If oTA_AWARDED_APP.ID_USER_LAST_UPDATED.HasValue Then
                    strUser = dbEntities.vw_t_usuarios.Where(Function(p) p.id_usuario = oTA_AWARDED_APP.ID_USER_LAST_UPDATED And p.id_programa = id_programa).FirstOrDefault.nombre_usuario
                End If


                Me.lbl_last_update_by.Text = strUser
                If oTA_AWARDED_APP.ID_AWARD_STATUS = 2 Then
                    btn_generate_activity.Attributes.Add("class", "btn btn-warning  btn-sm margin-r-5 pull-right")
                    btn_awarded.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-right disabled")
                Else
                    btn_generate_activity.Attributes.Add("class", "btn btn-warning  btn-sm margin-r-5 pull-right disabled")
                    btn_awarded.Attributes.Add("class", "btn btn-success btn-sm margin-r-5 pull-right")
                End If


            Else

                Me.lbl_id_sol_app.Text = "0"
                Me.LBL_ID_AWARD.Text = "0"

                btn_generate_activity.Attributes.Add("class", "btn btn-warning  btn-sm margin-r-5 pull-right disabled")

            End If


            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)


        End Using


    End Sub



    Public Function get_Apply_Documents(ByVal idSOL_App As Integer) As DataTable

        Using dbEntities As New dbRMS_JIEntities


            ' Dim IdACTIVITYsol As Integer = Val(Me.lbl_id_sol.Text)
            Dim oDocuments = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = idSOL_App And p.visible.Value And p.DOCUMENTROLE = "AWARD_ANNEX").ToList()
            'Dim oMaterials = dbEntities.VW_TA_SOLICITATION_MATERIALS.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = IdACTIVITYsol).ToList()

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

            'Dim foundMaterial As Boolean = False
            'For Each itemMaterial In oMaterials

            '    For Each dtRow In dtDocuments.Rows

            '        If dtRow("id_doc_soporte") = itemMaterial.ID_DOC_SOPORTE Then

            '            dtRow("ID_SOLICITATION_MATERIAL") = itemMaterial.ID_DOC_SOPORTE
            '            dtRow("REQUIRED_FILE") = If(itemMaterial.MANDATORY, "MANDATORY", "OPTIONAL")
            '            foundMaterial = True
            '            Exit For

            '        End If

            '    Next

            '    If foundMaterial = False Then

            '        dtDocuments.Rows.Add(itemMaterial.ID_ACTIVITY_SOLICITATION,
            '                        idSOL_App,
            '                        0,
            '                        itemMaterial.ID_SOLICITATION_MATERIAL,
            '                        itemMaterial.ID_DOC_SOPORTE,
            '                        itemMaterial.DOCUMENT_TITLE,
            '                        "",
            '                        itemMaterial.nombre_documento,
            '                        itemMaterial.extension,
            '                        itemMaterial.max_size,
            '                        itemMaterial.template,
            '                        "",
            '                         If(itemMaterial.MANDATORY, "MANDATORY", "OPTIONAL"))

            '    End If

            '    foundMaterial = False

            'Next

            get_Apply_Documents = dtDocuments



        End Using



    End Function



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

        'If opt = 2 Then

        '    dtApplicants.Columns.Add("ID_ORGANIZATION_APP", GetType(Integer))
        '    dtApplicants.Columns.Add("ID_SOLICITATION_APP", GetType(Integer))
        '    dtApplicants.Columns.Add("ID_ACTIVITY_SOLICITATION", GetType(Integer))
        '    dtApplicants.Columns.Add("SOLICITATION_APP_CODE", GetType(String))
        '    dtApplicants.Columns.Add("NAMEALIAS", GetType(String))
        '    dtApplicants.Columns.Add("ORGANIZATIONNAME", GetType(String))
        '    dtApplicants.Columns.Add("PERSONNAME", GetType(String))
        '    dtApplicants.Columns.Add("ORGANIZATION_TYPE", GetType(String))
        '    dtApplicants.Columns.Add("ADDRESSCOUNTRYREGIONID", GetType(String))
        '    dtApplicants.Columns.Add("ADDRESSCITY", GetType(String))
        '    dtApplicants.Columns.Add("ORGANIZATIONSTATUS", GetType(String))
        '    dtApplicants.Columns.Add("ORGANIZATIONEMAIL", GetType(String))
        '    dtApplicants.Columns.Add("APLICATION_STATUS", GetType(String))
        '    dtApplicants.Columns.Add("ID_APP_STATUS", GetType(Integer))
        '    dtApplicants.Columns.Add("SENT_DATE", GetType(DateTime))
        '    dtApplicants.Columns.Add("RECEIVED_DATE", GetType(DateTime))
        '    dtApplicants.Columns.Add("SUBMITTED_DATE", GetType(DateTime))

        'End If



    End Sub


    Sub loadLists(ByVal idPrograma As Integer)



        Using dbEntities As New dbRMS_JIEntities





            Dim entryPoint = From u In db.t_usuarios Join q In db.t_usuario_programa On u.id_usuario Equals q.id_usuario
                             Where q.id_programa = idPrograma
                             Select New With {Key .id_usuario = u.id_usuario, Key .nombre = u.nombre_usuario & " " & u.apellidos_usuario,
                                           Key .busqueda_actividad = u.t_usuario_programa.FirstOrDefault().busqueda_actividad}


            Me.cmb_persona_encargada.DataSourceID = ""
            Me.cmb_persona_encargada.DataSource = entryPoint.ToList()
            Me.cmb_persona_encargada.DataTextField = "nombre"
            Me.cmb_persona_encargada.DataValueField = "id_usuario"
            Me.cmb_persona_encargada.DataBind()

            'Me.cmb_ejecutor.DataSourceID = ""
            'Me.cmb_ejecutor.DataSource = cl_listados.get_t_ejecutores(idPrograma)
            'Me.cmb_ejecutor.DataTextField = "nombre_ejecutor"
            'Me.cmb_ejecutor.DataValueField = "id_ejecutor"
            'Me.cmb_ejecutor.DataBind()
            'Me.cmb_ejecutor.SelectedValue = oProyecto.id_ejecutor

            Me.cmb_region.DataSourceID = ""
            Me.cmb_region.DataSource = cl_listados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_region.DataTextField = "nombre_region"
            Me.cmb_region.DataValueField = "id_region"
            Me.cmb_region.DataBind()

            Me.grd_subregion.DataSource = ""
            Me.grd_subregion.DataSource = db.vw_t_subregiones.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.grd_subregion.DataBind()

            'If Not oProyecto.id_region.Contains(",") Then
            '    Me.cmb_region.SelectedValue = Convert.ToInt32(oProyecto.id_region)
            '    Me.cmb_subregion.DataSourceID = ""
            '    Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
            '    Me.cmb_subregion.DataTextField = "nombre_subregion"
            '    Me.cmb_subregion.DataValueField = "id_subregion"
            '    Me.cmb_subregion.DataBind()
            '    Me.cmb_subregion.SelectedValue = Convert.ToInt32(oProyecto.id_subregion)
            'Else

            Me.cmb_subregion.DataSourceID = ""
            Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
            Me.cmb_subregion.DataTextField = "nombre_subregion"
            Me.cmb_subregion.DataValueField = "id_subregion"
            Me.cmb_subregion.DataBind()
            '' Me.cmb_subregion.SelectedValue =  ''= Convert.ToInt32(oProyecto.id_subregion.Split(",")(0))

            Me.cmb_budget.DataSourceID = ""
            Me.cmb_budget.DataSource = cl_listados.get_t_budget(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_budget.DataTextField = "bud_name"
            Me.cmb_budget.DataValueField = "id_budget"
            Me.cmb_budget.DataBind()


            Me.cmb_currency.DataSourceID = ""
            Me.cmb_currency.DataSource = cl_listados.get_t_programa_currency(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_currency.DataTextField = "currency_prefix"
            Me.cmb_currency.DataValueField = "id_programa_currency"
            Me.cmb_currency.DataBind()

            Me.cmb_type_of_document.DataSourceID = ""
            Me.cmb_type_of_document.DataSource = cl_listados.get_ta_docs_soporte(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_type_of_document.DataTextField = "nombre_documento"
            Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
            Me.cmb_type_of_document.DataBind()



            Me.cmb_periodo.DataSourceID = ""
            Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
            Me.cmb_periodo.DataTextField = "nombre_periodo"
            Me.cmb_periodo.DataValueField = "id_periodo"
            Me.cmb_periodo.DataBind()


            Me.cmb_estado.DataSourceID = ""
            Me.cmb_estado.DataSource = cl_listados.get_TA_ACTIVITY_STATUS(idPrograma)
            Me.cmb_estado.DataTextField = "STATUS"
            Me.cmb_estado.DataValueField = "ID_ACTIVITY_STATUS"
            Me.cmb_estado.DataBind()


            Dim oMECANISMO = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()

            Me.cmb_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_mecanismo_contratacion.DataSource = oMECANISMO
            Me.cmb_mecanismo_contratacion.DataTextField = "nombre_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataValueField = "id_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataBind()

            Me.cmb_mecanismo_contratacion2.DataSourceID = ""
            Me.cmb_mecanismo_contratacion2.DataSource = oMECANISMO
            Me.cmb_mecanismo_contratacion2.DataTextField = "nombre_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion2.DataValueField = "id_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion2.DataBind()


            ' Dim oSUB_MECANISMO = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue).ToList()
            Dim id_mecanismo As Integer = oMECANISMO.FirstOrDefault.id_mecanismo_contratacion
            Dim oSUB_MECANISMO = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = id_mecanismo).ToList()


            Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion.DataSource = oSUB_MECANISMO
            Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataBind()


            Me.cmb_sub_mecanismo_contratacion2.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion2.DataSource = oSUB_MECANISMO
            Me.cmb_sub_mecanismo_contratacion2.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion2.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion2.DataBind()

            Me.cmb_awards.DataSourceID = ""
            Me.cmb_awards.DataSource = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.id_programa = idPrograma And p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
            Me.cmb_awards.DataTextField = "AWARD_CODE"
            Me.cmb_awards.DataValueField = "ID_AWARDED_APP"
            Me.cmb_awards.DataBind()

        End Using


    End Sub


    Sub LoadACTIVITIES(ByVal id_awarded_app As Integer, Optional bnd_bool As Boolean = True)


        Using dbEntities As New dbRMS_JIEntities

            Dim oAWARDED_ACTIVITIES = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).ToList()

            Me.grd_activities.DataSource = oAWARDED_ACTIVITIES

            If bnd_bool Then

                Me.grd_activities.DataBind()

            End If


        End Using


    End Sub



    Sub loadActivity(ByVal idPrograma As Integer, ByVal oProyecto As VW_TA_AWARDED_ACTIVITY)


        Me.txt_codigo_SAPME.Text = oProyecto.codigo_SAPME
        Me.lbl_mensaje.Text = oProyecto.codigo_SAPME

        Me.txt_codigoproyecto.Text = oProyecto.codigo_ficha_AID
        Me.divCodigo.Visible = True
        Me.lbl_mensaje.Visible = True

        Me.txt_nombreproyecto.Text = oProyecto.nombre_proyecto
        Me.txt_codigoMonitor.Text = oProyecto.codigo_MONITOR
        Me.txt_descripcion.Text = oProyecto.area_intervencion
        Me.dt_fecha_inicio.SelectedDate = oProyecto.fecha_inicio_proyecto
        Me.dt_fecha_fin.SelectedDate = oProyecto.fecha_fin_proyecto
        Me.txt_codigoRFA.Text = oProyecto.codigo_RFA

        Me.txt_exchange_rate.Value = oProyecto.tasa_cambio
        Me.txt_tot_amount.Value = oProyecto.costo_total_proyecto
        Me.txt_tot_amount_Local.Value = oProyecto.costo_total_proyecto_LOCAL
        'Me.txt_tot_amount_Local.Value = If(oProyecto.tasa_cambio = 0, 0, oProyecto.costo_total_proyecto * oProyecto.tasa_cambio)

        If oProyecto.id_usuario_responsable > 0 Then
            Me.cmb_persona_encargada.SelectedValue = oProyecto.id_usuario_responsable
        End If

        Me.chk_todos.Checked = True
        Me.grd_subregion.Visible = True
        Me.cmb_subregion.Visible = False

        'For Each row In Me.grd_subregion.Items
        '    If TypeOf row Is GridDataItem Then

        '        Dim dataItem As GridDataItem = CType(row, GridDataItem)
        '        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
        '        subR.Checked = False

        '    End If
        'Next

        For Each Irow As GridDataItem In Me.grd_subregion.Items

            Dim chkvisible As CheckBox = CType(Irow("TemplateColumnAnual").FindControl("ctrl_id"), CheckBox)
            'If Irow("ID_AWARDED_ACTIVITY").Text <> idActivity_selected Then
            chkvisible.Checked = False
            'End If

        Next


        For Each item In db.TA_AWARDED_ACTIVITY_SUBREGION.Where(Function(p) p.ID_AWARDED_ACTIVITY = oProyecto.ID_AWARDED_ACTIVITY)
            For Each row In Me.grd_subregion.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                    Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                    If item.id_subregion = idSubregion Then
                        subR.Checked = True
                        nivel_cobertura.Value = item.nivel_cobertura

                        If item.nivel_cobertura > 0 Then
                            nivel_cobertura.Enabled = True
                        End If
                    End If
                End If
            Next
        Next

        '' End If

        'Dim subregiones = oProyecto.id_subregion.Replace(" ", "").Split(",")
        'Me.grd_district.DataSource = ""
        'Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) subregiones.Contains(p.id_subregion.ToString())).ToList()
        'Me.grd_district.DataBind()

        'For Each item In db.tme_ficha_municipio.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto)
        '    For Each row In Me.grd_district.Items
        '        If TypeOf row Is GridDataItem Then
        '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
        '            Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
        '            Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
        '            Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
        '            If item.id_municipio = id_municipio Then
        '                subR.Checked = True
        '                nivel_cobertura.Value = item.nivel_cobertura

        '                If item.nivel_cobertura > 0 Then
        '                    nivel_cobertura.Enabled = True
        '                End If
        '            End If
        '        End If
        '    Next
        'NextoProyecto.ID_AWARDED_ACTIVITY

        'Me.cmb_componente.DataSourceID = ""
        'Me.cmb_componente.DataSource = cl_listados.get_tme_componente_programa(Convert.ToInt32(Me.cmb_programa.SelectedValue))
        'Me.cmb_componente.DataTextField = "nombre_componente"
        'Me.cmb_componente.DataValueField = "id_componente"
        'Me.cmb_componente.DataBind()

        'If oProyecto.id_componente IsNot Nothing Then
        '    Me.cmb_componente.SelectedValue = oProyecto.id_componente
        'End If

        Using dbEntities As New dbRMS_JIEntities

            'Dim id_tmp = Convert.ToInt64(Me.lbl_id_sesion_temp.Text)
            Dim oPro = dbEntities.TA_AWARDED_ACTIVITY.Find(oProyecto.ID_AWARDED_ACTIVITY)

            'If oProyecto.isprivatepublic.HasValue Then
            '    If oProyecto.isprivatepublic.Value Then
            '        Me.rbn_private_public.SelectedValue = 1
            '        Me.grd_partners.Enabled = True

            '        'listPartners = oPro.tme_ficha_partner.ToList()
            '        'grd_partners.Rebind()
            '    Else
            '        Me.rbn_private_public.SelectedValue = 2
            '    End If
            'Else
            '    Me.rbn_private_public.SelectedValue = 2
            'End If

            Me.cmb_region.SelectedValue = oProyecto.id_region

            Me.cmb_periodo.SelectedValue = oProyecto.id_periodo

            Me.cmb_estado.SelectedValue = oPro.ID_ACTIVITY_STATUS

            Me.cmb_mecanismo_contratacion.SelectedValue = oPro.tme_sub_mecanismo.id_mecanismo_contratacion


            Dim oSUB_MECANISMO = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = oPro.tme_sub_mecanismo.id_mecanismo_contratacion).ToList()

            Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion.DataSource = oSUB_MECANISMO
            Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataBind()

            Me.cmb_sub_mecanismo_contratacion2.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion2.DataSource = oSUB_MECANISMO
            Me.cmb_sub_mecanismo_contratacion2.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion2.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion2.DataBind()

            Me.cmb_sub_mecanismo_contratacion.SelectedValue = oPro.tme_sub_mecanismo.id_sub_mecanismo


            'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
            '    Me.alink_stos.Attributes.Add("style", "display:none;")
            'End If

            Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = oPro.tme_sub_mecanismo.id_sub_mecanismo).FirstOrDefault.id_sub_father

            If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                Dim cls_util As New ly_SIME.CORE.cls_util
                Me.cmb_activity_father.DataSourceID = ""
                Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_sub_mecanismo = idSub_Father).ToList())
                'Me.cmb_activity_father.DataTextField = "nombre_sub_mecanismo"
                'Me.cmb_actividad_father.DataValueField = "id_sub_mecanismo"
                Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = idSub_Father).FirstOrDefault.nombre_sub_mecanismo
                Me.cmb_activity_father.DataBind()
                Me.ly_activity.Visible = True

                If Val(oPro.id_ficha_padre) > 0 Then
                    Me.cmb_activity_father.SelectedValue = oPro.id_ficha_padre
                End If

            Else
                Me.ly_activity.Visible = False
            End If

            'If Not ((oPro.TA_ACTIVITY_STATUS.ORDER = 4 And oPro.TA_ACTIVITY_STATUS.ORDERbool = True) Or oPro.TA_ACTIVITY_STATUS.ORDER > 4) Then
            '********************UPGRADE CODE*******************
            LoadData_code(oPro.TA_ACTIVITY_STATUS.ID_ACTIVITY_STATUS)
            '********************UPGRADE CODE*******************
            'End If

        End Using

    End Sub


    'Sub LoadData_code(ByVal id As Integer)
    '    'Me.txt_codigoproyecto.Text = clListados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)

    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim oProyecto = dbEntities.tme_Ficha_Proyecto.Find(id)
    '        If oProyecto.id_ficha_estado > 1 Then
    '            'Me.lbl_alerta.Text = "El proyecto esta en ejecución o finalizado, no lo puede editar."
    '            'Me.btn_guardar.Enabled = False
    '        End If

    '    End Using
    'End Sub
    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        'If Not chk_todos.Checked Then
        '    Me.grd_district.DataSource = ""
        '    Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
        '    Me.grd_district.DataBind()
        'End If
    End Sub

    Protected Sub lnk_sugerir_codigo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_sugerir_codigo.Click
        Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)
        Me.lbl_mensaje.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)
    End Sub

    Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
        Dim sql = ""
        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
        Me.cmb_periodo.DataBind()
        'If Not chk_todos.Checked Then
        '    Me.grd_district.DataSource = ""
        '    Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
        '    Me.grd_district.DataBind()
        'End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        'Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
        Dim id_ficha = Convert.ToInt32(Me.lbl_id_activity_award.Text)


        Try


            SaveFicha(id_ficha)

            ''*******************************************************************************************************************************************
            ''*******************************************************************************************************************************************
            ''*******************************************************************************************************************************************


            ''sumarLOE()

            'valorSuma = 100

            ''If (valorSuma <> 100 And chk_todos.Checked) Or (chk_todos.Checked And cero = True) Then
            ''    Me.div_mensaje.Visible = True
            ''    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncHide()", True)
            ''Else

            'Using dbEntities As New dbRMS_JIEntities

            '    Dim bndError As Boolean = False
            '    Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue).FirstOrDefault.id_sub_father

            '    If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

            '        If Me.cmb_activity_father.SelectedValue IsNot Nothing Then
            '            If Val(Me.cmb_activity_father.SelectedValue) = 0 Then
            '                bndError = True
            '            Else
            '                bndError = False
            '            End If
            '        Else
            '            bndError = True
            '        End If

            '    End If

            '    If Not bndError Then

            '        Me.lbl_Activity_error.Visible = False

            '        ' Dim oFicha = dbEntities.TA_ACTIVITY.Find(id_ficha)
            '        Dim oFicha = dbEntities.TA_AWARDED_ACTIVITY.Find(id_ficha)
            '        oFicha.codigo_SAPME = Me.txt_codigoproyecto.Text
            '        oFicha.codigo_ficha_AID = Me.txt_codigoproyecto.Text
            '        oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
            '        oFicha.area_intervencion = Me.txt_descripcion.Text

            '        'oFicha.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue
            '        oFicha.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue

            '        ''oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
            '        oFicha.id_subregion = Me.cmb_subregion.SelectedValue
            '        'If Me.cmb_componente.SelectedValue <> "" Then
            '        '    oFicha.id_componente = Me.cmb_componente.SelectedValue
            '        'End If
            '        oFicha.fecha_inicio_proyecto = Me.dt_fecha_inicio.SelectedDate
            '        oFicha.fecha_fin_proyecto = Me.dt_fecha_fin.SelectedDate
            '        oFicha.codigo_RFA = Me.txt_codigoRFA.Text
            '        oFicha.codigo_MONITOR = Me.txt_codigoMonitor.Text
            '        oFicha.id_periodo = Me.cmb_periodo.SelectedValue
            '        oFicha.id_usuario_responsable = Me.cmb_persona_encargada.SelectedValue
            '        'oFicha.ID_ACTIVITY_STATUS = 1

            '        Dim fechaReg = Date.Now
            '        Dim fehcaTasaCambio = ""
            '        If Month(fechaReg) > 9 Then
            '            fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
            '            fechaReg = Convert.ToDateTime(fehcaTasaCambio)
            '        End If

            '        Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
            '        If txt_exchange_rate.Value > 0 Then
            '            oFicha.tasa_cambio = txt_exchange_rate.Value
            '        Else
            '            If oTasaCambio.Count > 0 Then
            '                oFicha.tasa_cambio = oTasaCambio.FirstOrDefault().tasa_cambio
            '            Else
            '                oFicha.tasa_cambio = 0
            '            End If
            '        End If

            '        oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
            '        oFicha.dateUpdate = Date.UtcNow
            '        oFicha.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            '        If Not (Me.cmb_activity_father.SelectedValue Is Nothing) Then
            '            If Val(Me.cmb_activity_father.SelectedValue) > 0 Then
            '                oFicha.id_ficha_padre = Convert.ToInt32(Me.cmb_activity_father.SelectedValue)
            '            Else
            '                oFicha.id_ficha_padre = Nothing
            '            End If
            '        End If

            '        ' Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.OBLIGATED_AMOUNT
            '        oFicha.costo_total_proyecto = Me.txt_tot_activity_amount.Value
            '        oFicha.costo_total_proyecto_LOC = Me.txt_tot_amount_Local.Text

            '        oFicha.OBLIGATED_AMOUNT = Me.txt_obligated_usd.Value
            '        oFicha.OBLIGATED_AMOUNT_LOC = Me.txt_obligated_local.Value
            '        ' oFicha.costo_total_proyecto = Me.txt_tot_amount.Value + Me.txt_leaveraged_local.Value

            '        dbEntities.Database.ExecuteSqlCommand("DELETE FROM [TA_AWARDED_ACTIVITY_SUBREGION] where ID_AWARDED_ACTIVITY = " & id_ficha)


            '        Dim boolAdded As Boolean = False

            '        If chk_todos.Checked Then
            '            For Each row In Me.grd_subregion.Items
            '                If TypeOf row Is GridDataItem Then
            '                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
            '                    Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
            '                    Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
            '                    If subR.Checked = True Then
            '                        Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
            '                        Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
            '                        oSubregion.id_subregion = idSubregion
            '                        oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
            '                        oSubregion.nivel_cobertura = nivel_cobertura.Value
            '                        oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

            '                        boolAdded = True

            '                    End If
            '                End If
            '            Next
            '        End If

            '        If Not boolAdded Then

            '            Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
            '            oSubregion.id_subregion = Me.cmb_subregion.SelectedValue
            '            oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
            '            oSubregion.nivel_cobertura = 100
            '            oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

            '        End If





            '        'For Each row In Me.grd_district.Items
            '        '    If TypeOf row Is GridDataItem Then
            '        '        Dim dataItem As GridDataItem = CType(row, GridDataItem)
            '        '        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
            '        '        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
            '        '        If subR.Checked = True Then
            '        '            Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
            '        '            Dim oDistrict = New tme_ficha_municipio
            '        '            oDistrict.id_municipio = id_municipio
            '        '            oDistrict.id_ficha_proyecto = oFicha.id_ficha_proyecto
            '        '            oDistrict.nivel_cobertura = nivel_cobertura.Value
            '        '            oFicha.tme_ficha_municipio.Add(oDistrict)
            '        '        End If
            '        '    End If
            '        'Next

            '        'If Me.rbn_private_public.SelectedValue = 1 Then
            '        '    oFicha.isprivatepublic = True
            '        '    For Each row In Me.grd_partners.Items
            '        '        If TypeOf row Is GridDataItem Then
            '        '            Dim dataItem As GridDataItem = CType(row, GridDataItem)

            '        '            Dim nombre_partner As TextBox = CType(row.Cells(0).FindControl("txt_nombre_partner"), TextBox)
            '        '            Dim partner_type As RadComboBox = CType(row.Cells(0).FindControl("cmb_partner_type"), RadComboBox)
            '        '            Dim partnership_focus As RadComboBox = CType(row.Cells(0).FindControl("cmb_partnership_focus"), RadComboBox)


            '        '            oFicha.tme_ficha_partner.Add(New tme_ficha_partner() _
            '        '                                 With {.id_partner_type = partner_type.SelectedValue, _
            '        '                                       .id_partnership_focus = partnership_focus.SelectedValue, _
            '        '                                       .nombre_partner = nombre_partner.Text
            '        '                                     })

            '        '        End If
            '        '    Next
            '        'Else
            '        '    oFicha.isprivatepublic = False
            '        'End If

            '        'For Each file As UploadedFile In AsyncUpload1.UploadedFiles
            '        '    Dim nombreArchivo = cl_listados.getNewName(file, Me.Session("E_IdUser").ToString())
            '        '    Dim oImagen = New tme_FichaProyectoImagen
            '        '    oImagen.id_ficha_proyecto = oFicha.id_ficha_proyecto
            '        '    oImagen.nombre_archivo_proyecto = nombreArchivo
            '        '    oImagen.id_tipo_proyecto_imagen = 1

            '        '    dbEntities.tme_FichaProyectoImagen.Add(oImagen)
            '        '    Dim Path As String
            '        '    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).images_folder)
            '        '    file.SaveAs(Path + nombreArchivo)
            '        'Next

            '        dbEntities.Entry(oFicha).State = Entity.EntityState.Modified


            '        If dbEntities.SaveChanges() Then

            save_aw() '**************Change the activity values

            '        End If

            '        'Dim usuAct = db.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = oFicha.id_usuario_responsable And p.id_ficha_proyecto = oFicha.id_ficha_proyecto).ToList()
            '        'If usuAct.Count() = 0 Then
            '        '    Dim oUsuaAct = New t_usuario_ficha_proyecto
            '        '    oUsuaAct.id_usuario = Me.cmb_persona_encargada.SelectedValue
            '        '    oUsuaAct.id_ficha_proyecto = oFicha.id_ficha_proyecto
            '        '    oUsuaAct.fecha_crea = DateTime.Now
            '        '    oUsuaAct.acc_act = True
            '        '    oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
            '        '    dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
            '        '    dbEntities.SaveChanges()
            '        'End If

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & CInt(Me.lbl_id_ficha.Text) & "&Id_AW=" & id_ficha.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            '    Else

            '        Me.lbl_Activity_error.Visible = True

            '    End If

            '    'End If


            '    '*******************************************************************************************************************************************
            '    '*******************************************************************************************************************************************
            '    '*******************************************************************************************************************************************



            'End Using

        Catch ex As Exception
            Me.MsgGuardar.NuevoMensaje = "Error Saving" & ex.Source
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & CInt(Me.lbl_id_ficha.Text) & "&Id_AW=" & id_ficha.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Try

    End Sub


    Private Sub SaveFicha(Optional idF_new As Integer = 0)

        '  Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
        Dim id_ficha = If(idF_new = 0, Convert.ToInt32(Me.lbl_id_activity_award.Text), idF_new)


        Try

            valorSuma = 100


            Using dbEntities As New dbRMS_JIEntities

                Dim id_aw_app As Integer = CInt(Me.lbl_id_award_app.Text)

                Dim bndError As Boolean = False
                Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue).FirstOrDefault.id_sub_father

                Dim oAward_app = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_APP = id_aw_app).FirstOrDefault()

                If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                    If Me.cmb_activity_father.SelectedValue IsNot Nothing Then
                        If Val(Me.cmb_activity_father.SelectedValue) = 0 Then
                            bndError = True
                        Else
                            bndError = False
                        End If
                    Else
                        bndError = True
                    End If

                End If

                If Not bndError Then

                    Me.lbl_Activity_error.Visible = False

                    Dim oFicha As TA_AWARDED_ACTIVITY = New TA_AWARDED_ACTIVITY

                    If id_ficha Then
                        oFicha = dbEntities.TA_AWARDED_ACTIVITY.Find(id_ficha)
                    End If


                    oFicha.codigo_SAPME = Me.txt_codigoproyecto.Text
                    oFicha.codigo_ficha_AID = Me.txt_codigoproyecto.Text
                    oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
                    oFicha.area_intervencion = Me.txt_descripcion.Text
                    oFicha.ID_AWARDED_APP = CInt(Me.LBL_ID_AWARD.Text)


                    '"' """"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
                    'oFicha.ID_ORGANIZATION_APP = oAward_app.ID_ORGANIZATION_APP


                    'oFicha.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue
                    'oFicha.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue
                    oFicha.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion2.SelectedValue


                    ''oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
                    oFicha.id_subregion = Me.cmb_subregion.SelectedValue
                    'If Me.cmb_componente.SelectedValue <> "" Then
                    '    oFicha.id_componente = Me.cmb_componente.SelectedValue
                    'End If

                    oFicha.fecha_inicio_proyecto = Me.dt_fecha_inicio.SelectedDate
                    oFicha.fecha_fin_proyecto = Me.dt_fecha_fin.SelectedDate
                    oFicha.codigo_RFA = Me.txt_codigoRFA.Text
                    oFicha.codigo_MONITOR = Me.txt_codigoMonitor.Text
                    oFicha.id_periodo = Me.cmb_periodo.SelectedValue
                    oFicha.id_usuario_responsable = Me.cmb_persona_encargada.SelectedValue
                    'oFicha.ID_ACTIVITY_STATUS = 1

                    'Dim fechaReg = Date.Now
                    'Dim fehcaTasaCambio = ""
                    'If Month(fechaReg) > 9 Then
                    '    fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                    '    fechaReg = Convert.ToDateTime(fehcaTasaCambio)
                    'End If

                    'Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
                    'If txt_exchange_rate.Value > 0 Then
                    '    oFicha.tasa_cambio = txt_exchange_rate.Value
                    'Else
                    '    If oTasaCambio.Count > 0 Then
                    '        oFicha.tasa_cambio = oTasaCambio.FirstOrDefault().tasa_cambio
                    '    Else
                    '        oFicha.tasa_cambio = 0
                    '    End If
                    'End If


                    oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
                    oFicha.dateUpdate = Date.UtcNow
                    oFicha.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    If Not (Me.cmb_activity_father.SelectedValue Is Nothing) Then
                        If Val(Me.cmb_activity_father.SelectedValue) > 0 Then
                            oFicha.id_ficha_padre = Convert.ToInt32(Me.cmb_activity_father.SelectedValue)
                        Else
                            oFicha.id_ficha_padre = Nothing
                        End If
                    End If

                    oFicha.tasa_cambio = Me.txt_exchange_rate.Value

                    ' Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.OBLIGATED_AMOUNT
                    'oFicha.OBLIGATED_AMOUNT = Me.txt_obligated_usd.Value
                    'oFicha.OBLIGATED_AMOUNT_LOC = Me.txt_obligated_local.Value

                    oFicha.OBLIGATED_AMOUNT = Me.txt_tot_amount.Value
                    oFicha.OBLIGATED_AMOUNT_LOC = Me.txt_tot_amount_Local.Value

                    'oFicha.costo_total_proyecto = Me.txt_obligated_usd.Value + Me.txt_leaveraged_usd.Value
                    'oFicha.costo_total_proyecto_LOC = Me.txt_obligated_local.Value + Me.txt_leaveraged_local.Value

                    oFicha.costo_total_proyecto = Me.txt_tot_amount.Value
                    oFicha.costo_total_proyecto_LOC = Me.txt_tot_amount_Local.Value



                    If id_ficha = 0 Then

                        oFicha.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                        oFicha.ID_ACTIVITY_STATUS = 7
                        oFicha.id_usuario_creo = Me.Session("E_IdUser").ToString()
                        oFicha.datecreated = Date.UtcNow
                        oFicha.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                        dbEntities.TA_AWARDED_ACTIVITY.Add(oFicha)
                        dbEntities.SaveChanges()

                    Else

                        dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()

                    End If


                    '*******************************NEW***************************
                    id_ficha = oFicha.ID_AWARDED_ACTIVITY
                    Dim idACT_ = Convert.ToInt32(Me.lbl_id_ficha.Text)
                    set_links(idACT_, id_ficha)
                    Me.lbl_id_activity_award.Text = id_ficha
                    '*******************************NEW***************************

                    dbEntities.Database.ExecuteSqlCommand("DELETE FROM [TA_AWARDED_ACTIVITY_SUBREGION] where ID_AWARDED_ACTIVITY = " & id_ficha)


                    Dim boolAdded As Boolean = False

                    If chk_todos.Checked Then
                        For Each row In Me.grd_subregion.Items
                            If TypeOf row Is GridDataItem Then
                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                                Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                                If subR.Checked = True Then
                                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                                    Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
                                    oSubregion.id_subregion = idSubregion
                                    oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
                                    oSubregion.nivel_cobertura = nivel_cobertura.Value
                                    oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

                                    boolAdded = True

                                End If
                            End If
                        Next
                    End If

                    If Not boolAdded Then

                        Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
                        oSubregion.id_subregion = Me.cmb_subregion.SelectedValue
                        oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
                        oSubregion.nivel_cobertura = 100
                        oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

                    End If


                    dbEntities.SaveChanges()



                    'For Each row In Me.grd_district.Items
                    '    If TypeOf row Is GridDataItem Then
                    '        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    '        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                    '        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                    '        If subR.Checked = True Then
                    '            Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
                    '            Dim oDistrict = New tme_ficha_municipio
                    '            oDistrict.id_municipio = id_municipio
                    '            oDistrict.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '            oDistrict.nivel_cobertura = nivel_cobertura.Value
                    '            oFicha.tme_ficha_municipio.Add(oDistrict)
                    '        End If
                    '    End If
                    'Next

                    'If Me.rbn_private_public.SelectedValue = 1 Then
                    '    oFicha.isprivatepublic = True
                    '    For Each row In Me.grd_partners.Items
                    '        If TypeOf row Is GridDataItem Then
                    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)

                    '            Dim nombre_partner As TextBox = CType(row.Cells(0).FindControl("txt_nombre_partner"), TextBox)
                    '            Dim partner_type As RadComboBox = CType(row.Cells(0).FindControl("cmb_partner_type"), RadComboBox)
                    '            Dim partnership_focus As RadComboBox = CType(row.Cells(0).FindControl("cmb_partnership_focus"), RadComboBox)


                    '            oFicha.tme_ficha_partner.Add(New tme_ficha_partner() _
                    '                                 With {.id_partner_type = partner_type.SelectedValue, _
                    '                                       .id_partnership_focus = partnership_focus.SelectedValue, _
                    '                                       .nombre_partner = nombre_partner.Text
                    '                                     })

                    '        End If
                    '    Next
                    'Else
                    '    oFicha.isprivatepublic = False
                    'End If

                    'For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                    '    Dim nombreArchivo = cl_listados.getNewName(file, Me.Session("E_IdUser").ToString())
                    '    Dim oImagen = New tme_FichaProyectoImagen
                    '    oImagen.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '    oImagen.nombre_archivo_proyecto = nombreArchivo
                    '    oImagen.id_tipo_proyecto_imagen = 1

                    '    dbEntities.tme_FichaProyectoImagen.Add(oImagen)
                    '    Dim Path As String
                    '    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).images_folder)
                    '    file.SaveAs(Path + nombreArchivo)
                    'Next


                    'Dim usuAct = db.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = oFicha.id_usuario_responsable And p.id_ficha_proyecto = oFicha.id_ficha_proyecto).ToList()
                    'If usuAct.Count() = 0 Then
                    '    Dim oUsuaAct = New t_usuario_ficha_proyecto
                    '    oUsuaAct.id_usuario = Me.cmb_persona_encargada.SelectedValue
                    '    oUsuaAct.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '    oUsuaAct.fecha_crea = DateTime.Now
                    '    oUsuaAct.acc_act = True
                    '    oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
                    '    dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                    '    dbEntities.SaveChanges()
                    'End If

                    'Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    'Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_ficha
                    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                Else

                    Me.lbl_Activity_error.Visible = True

                End If

                'End If

            End Using

        Catch ex As Exception
            Me.MsgGuardar.NuevoMensaje = "Error Saving" & ex.Source
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_ficha & "&Id_AW=" & id_ficha.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Try


    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/RFP_/frm_Activities")
    End Sub

    'Protected Sub rbn_private_public_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_private_public.SelectedIndexChanged
    '    If rbn_private_public.SelectedValue = 1 Then
    '        Me.grd_partners.Enabled = True
    '        'Dim gridCo = CType(Me.placeHolder_grid.FindControl("gridAvance"), RadGrid)
    '        'gridCo.Enabled = True
    '    Else
    '        Me.grd_partners.Enabled = False
    '        'Dim gridCo = CType(Me.placeHolder_grid.FindControl("gridAvance"), RadGrid)
    '        'gridCo.Enabled = False
    '    End If
    'End Sub

    Protected Sub chk_todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_todos.CheckedChanged
        If chk_todos.Checked Then
            Me.grd_subregion.Visible = True
            Me.cmb_subregion.Visible = False
        Else
            Me.grd_subregion.Visible = False
            Me.cmb_subregion.Visible = True
        End If

    End Sub

    Protected Sub txt_nivel_cobertura_TextChanged(sender As Object, e As EventArgs)
        'sumarLOE()
    End Sub

    Sub sumarLOE()
        valorSuma = 0

        For Each row In Me.grd_district.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                'Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_aporteFicha")
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    txt_loe.Enabled = True
                    valorSuma += txt_loe.Value
                    If txt_loe.Value = 0 Then
                        cero = True
                    End If
                Else
                    txt_loe.Enabled = False
                End If
                Dim valor = 100 - valorSuma
                If valor <= 0.4 And valor >= -0.4 Then
                    valorSuma = 100
                End If
                If valorSuma <> 100 Then
                    Me.div_mensaje.Visible = True
                    Me.lbl_errorLOE.Visible = True
                Else
                    Me.div_mensaje.Visible = False
                    Me.lbl_errorLOE.Visible = False
                End If

            End If
        Next

        If cero = True Then
            Me.lbl_errorLOECero.Visible = True
            Me.div_mensaje.Visible = True
        Else
            Me.lbl_errorLOECero.Visible = False
        End If
    End Sub

    Protected Sub ctrl_id_CheckedChanged(sender As Object, e As EventArgs)
        Dim id_subregiones = ""
        For Each row In Me.grd_subregion.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                    id_subregiones &= idSubregion.ToString() & ","
                End If
            End If
        Next
        id_subregiones = id_subregiones.TrimEnd(",")
        Dim ids = id_subregiones.Split(",")
        Me.grd_district.DataSource = ""
        Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) ids.Contains(p.id_subregion)).ToList()
        Me.grd_district.DataBind()
        'sumarLOE()
    End Sub

    Protected Sub ctrl_id_municipio_CheckedChanged(sender As Object, e As EventArgs)
        For Each row In Me.grd_district.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    If txt_loe.Enabled = False Then
                        txt_loe.Value = 0
                    End If
                    txt_loe.Enabled = True
                Else
                    txt_loe.Enabled = False
                    txt_loe.Value = 0
                End If
            End If
        Next
        ' sumarLOE()
    End Sub

    Protected Sub cmb_mecanismo_contratacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanismo_contratacion.SelectedIndexChanged


        'Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, e.Value)
        'Me.lbl_mensaje.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, e.Value)

        Using dbEntities As New dbRMS_JIEntities

            Dim id_aw = 0
            Dim oPro As New TA_AWARDED_ACTIVITY
            Dim idACT_status As Integer = 0

            If e.Value IsNot Nothing Then

                If Convert.ToInt32(Me.lbl_id_activity_award.Text) > 0 Then
                    id_aw = Convert.ToInt32(Me.lbl_id_activity_award.Text)
                    oPro = dbEntities.TA_AWARDED_ACTIVITY.Find(id_aw)
                    idACT_status = oPro.ID_ACTIVITY_STATUS
                End If

                Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
                Me.cmb_sub_mecanismo_contratacion.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = e.Value).ToList()
                Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion.DataBind()
                LoadData_code(idACT_status)

            Else

                LoadData_code(idACT_status)

            End If

        End Using

    End Sub


    Private Sub cmb_sub_mecanismo_contratacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_sub_mecanismo_contratacion.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            Dim id_aw = 0
            Dim oPro As New TA_AWARDED_ACTIVITY
            Dim idACT_status As Integer = 0

            If e.Value IsNot Nothing Then

                If Convert.ToInt32(Me.lbl_id_activity_award.Text) > 0 Then
                    id_aw = Convert.ToInt32(Me.lbl_id_activity_award.Text)
                    oPro = dbEntities.TA_AWARDED_ACTIVITY.Find(id_aw)
                    idACT_status = oPro.ID_ACTIVITY_STATUS
                End If

                Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = e.Value).FirstOrDefault.id_sub_father

                If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                    Dim cls_util As New ly_SIME.CORE.cls_util
                    Me.cmb_activity_father.DataSourceID = ""
                    Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_sub_mecanismo = idSub_Father).ToList())
                    Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = idSub_Father).FirstOrDefault.nombre_sub_mecanismo
                    Me.cmb_activity_father.DataBind()
                    Me.ly_activity.Visible = True

                Else

                    Me.ly_activity.Visible = False

                End If

                LoadData_code(idACT_status)

            End If

        End Using




    End Sub

    Protected Sub cmb_activity_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(cmb_activity_father.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(cmb_activity_father.Items.Count)

    End Sub



    Protected Sub cmb_activity_father_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        'Handles cmb_activity_father.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities



            Dim id_aw = 0
            Dim oPro As New TA_AWARDED_ACTIVITY
            Dim idACT_status As Integer = 0

            If e.Value IsNot Nothing Then

                If Convert.ToInt32(Me.lbl_id_activity_award.Text) > 0 Then
                    id_aw = Convert.ToInt32(Me.lbl_id_activity_award.Text)
                    oPro = dbEntities.TA_AWARDED_ACTIVITY.Find(id_aw)
                    idACT_status = oPro.ID_ACTIVITY_STATUS
                End If

                LoadData_code(idACT_status)

            End If

        End Using


    End Sub





    Sub LoadData_code(ByVal idACT_status As Integer)

        Using dbEntities As New dbRMS_JIEntities

            If idACT_status > 0 Then

                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(idACT_status)

                If Not ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then
                    '********************UPGRADE CODE*******************

                    Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, If(chk_todos.Checked, -1, Me.cmb_subregion.SelectedValue), Me.cmb_mecanismo_contratacion.SelectedValue, If(Me.cmb_sub_mecanismo_contratacion.SelectedValue IsNot Nothing, Me.cmb_sub_mecanismo_contratacion.SelectedValue, -1), If(Me.cmb_activity_father.SelectedValue IsNot Nothing, If(Val(Me.cmb_activity_father.SelectedValue) = 0, -1, Me.cmb_activity_father.SelectedValue), -1))

                    Me.divCodigo.Visible = True
                    Me.lbl_mensaje.Visible = True
                    Me.lbl_mensaje.Text = Me.txt_codigoproyecto.Text
                    Me.lbl_award_code.Text = Me.txt_codigoproyecto.Text

                    '********************UPGRADE CODE*******************
                End If

            End If

        End Using

    End Sub


    Sub LoadData2(ByVal idACT_status As Integer)

        Using dbEntities As New dbRMS_JIEntities


            If idACT_status > 0 Then

                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(idACT_status)

                If Not ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                    '********************UPGRADE CODE*******************
                    Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, If(chk_todos.Checked, -1, Me.cmb_subregion.SelectedValue), Me.cmb_mecanismo_contratacion2.SelectedValue, If(Me.cmb_sub_mecanismo_contratacion2.SelectedValue IsNot Nothing, Me.cmb_sub_mecanismo_contratacion2.SelectedValue, -1), If(Me.cmb_activity_father.SelectedValue IsNot Nothing, If(Val(Me.cmb_activity_father.SelectedValue) = 0, -1, Me.cmb_activity_father.SelectedValue), -1))

                    Me.divCodigo.Visible = True
                    Me.lbl_mensaje.Visible = True
                    Me.lbl_mensaje.Text = Me.txt_codigoproyecto.Text
                    Me.lbl_award_code.Text = Me.txt_codigoproyecto.Text

                    '********************UPGRADE CODE*******************

                End If


            End If

        End Using

    End Sub

    'Protected Sub grd_partners_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
    '    grd_partners.DataSource = listPartners
    'End Sub

    'Protected Sub grd_partners_ItemCommand(sender As Object, e As GridCommandEventArgs)
    '    If e.CommandName = RadGrid.InitInsertCommandName Then
    '        saveAllData()
    '        Dim num = CInt(Rnd() * (Date.UtcNow.Millisecond * 1000) + Date.UtcNow.Millisecond)
    '        listPartners.Insert(Me.grd_partners.Items.Count, New tme_ficha_partner() With {.nombre_partner = "", .id_ficha_partner = num, .id_partner_type = 1, .id_partnership_focus = 1})
    '        e.Canceled = True
    '        grd_partners.Rebind()
    '    End If
    'End Sub



    'Protected Sub saveAllData()
    '    'Update Session
    '    For Each item As GridDataItem In grd_partners.MasterTableView.Items
    '        Dim nombre_partner As TextBox = CType(item.FindControl("txt_nombre_partner"), TextBox)
    '        Dim cmb_partnership_focus As RadComboBox = CType(item.FindControl("cmb_partnership_focus"), RadComboBox)
    '        Dim cmb_partner_type As RadComboBox = CType(item.FindControl("cmb_partner_type"), RadComboBox)
    '        Dim UniqueID = Convert.ToInt32(item.GetDataKeyValue("id_ficha_partner").ToString())
    '        Dim emp As tme_ficha_partner = listPartners.Where(Function(i) i.id_ficha_partner = UniqueID).First()
    '        emp.nombre_partner = nombre_partner.Text
    '        emp.id_partnership_focus = cmb_partnership_focus.SelectedValue
    '        emp.id_partner_type = cmb_partner_type.SelectedValue
    '    Next
    'End Sub


    'Protected Sub grd_partners_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_partners.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim cmb_partner_type As RadComboBox = CType(e.Item.FindControl("cmb_partner_type"), RadComboBox)
    '        cmb_partner_type.DataSource = db.tme_partner_type.ToList()
    '        cmb_partner_type.DataTextField = "partner_type_name"
    '        cmb_partner_type.DataValueField = "id_partner_type"
    '        cmb_partner_type.DataBind()

    '        cmb_partner_type.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partner_type").ToString()

    '        Dim cmb_partnership_focus As RadComboBox = CType(e.Item.FindControl("cmb_partnership_focus"), RadComboBox)
    '        cmb_partnership_focus.DataSource = db.tme_partnership_focus.ToList()
    '        cmb_partnership_focus.DataTextField = "partnership_focus_name"
    '        cmb_partnership_focus.DataValueField = "id_partnership_focus"
    '        cmb_partnership_focus.DataBind()

    '        cmb_partnership_focus.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partnership_focus").ToString()


    '        Dim hlnkDelete As LinkButton = New LinkButton
    '        hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
    '        hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())
    '        hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())

    '    End If
    'End Sub


    'Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
    '    Using db As New ly_SIME.dbRMS_JIEntities
    '        Try
    '            'db.Database.ExecuteSqlCommand("DELETE FROM ins_beneficiaries WHERE id_beneficiary = " + Me.identity.Text)
    '            listPartners = listPartners.Where(Function(p) p.id_ficha_partner <> Me.identity.Text).ToList()
    '            grd_partners.Rebind()
    '            'saveAllData()
    '            Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
    '        Catch ex As SqlException
    '            Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
    '            Me.MsgGuardar.TituMensaje = "Error al eliminar"
    '        End Try
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "hideDeleteModal()", True)
    '    End Using
    'End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub

    'Protected Sub dt_fecha_inicio_SelectedDateChanged(sender As Object, e As Calendar.SelectedDateChangedEventArgs) Handles dt_fecha_inicio.SelectedDateChanged
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim fechaReg = dt_fecha_inicio.SelectedDate
    '        Dim fehcaTasaCambio = ""
    '        If Month(fechaReg) > 9 Then
    '            fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
    '            fechaReg = Convert.ToDateTime(fehcaTasaCambio)
    '        End If

    '        Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
    '        If oTasaCambio.Count() > 0 Then
    '            Me.btn_guardar.Enabled = True
    '        Else
    '            Dim funcion = "FuncModatTrim()"
    '            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)
    '            Me.btn_guardar.Enabled = False
    '        End If
    '    End Using
    'End Sub

    'Public Property listPartners() As List(Of tme_ficha_partner)
    '    Get
    '        If Session("listPartners") IsNot Nothing Then
    '            Return DirectCast(Session("listPartners"), List(Of tme_ficha_partner))
    '        Else
    '            Return New List(Of tme_ficha_partner)()
    '        End If
    '    End Get
    '    Set(value As List(Of tme_ficha_partner))
    '        Session("listPartners") = value
    '    End Set
    'End Property


    Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
        'Dim Path As String
        'Path = Server.MapPath("~/FileUploads/")
        'e.File.SaveAs(Path + getNewName(e.File))
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


    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Using dbEntities As New dbRMS_JIEntities

                Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                Award_document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).awarded_documents_path
                'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).solicitation_documents_path

                Dim btn_delete As ImageButton = CType(itemD("Eliminar").Controls(0), ImageButton)
                'Dim Id_solicitation_app = Val(Me.lbl_id_sol_app.Text)
                Dim ID_AWARD = Val(Me.LBL_ID_AWARD.Text)

                'Dim oApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app).ToList()
                Dim Oaward = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_APP = ID_AWARD).ToList()

                If Oaward.Count > 0 Then
                    If Oaward.FirstOrDefault.ID_AWARD_STATUS = 2 Then 'Awarded
                        btn_delete.Visible = False
                    End If
                End If

                Dim ImageDownload As New HyperLink
                ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
                ImageDownload.NavigateUrl = Award_document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
                ImageDownload.Target = "_blank"
                ImageDownload.ToolTip = DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

                'Dim aprobar As New HyperLink
                'aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

                If DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_ANNEX").ToString() = "0" Then
                    ImageDownload.Visible = False
                    btn_delete.Visible = False

                    'If DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString() = "OPTIONAL" Then
                    '    aprobar.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                    '    aprobar.ToolTip = "Pending to upload"
                    'Else
                    '    aprobar.ImageUrl = "~/Imagenes/iconos/exclamation.gif"
                    '    aprobar.ToolTip = "Pending to upload"
                    'End If

                    'Else
                    '    aprobar.ImageUrl = "~/Imagenes/iconos/accept.png"
                    '    aprobar.ToolTip = "Uploaded"
                End If

                Dim hlk_File As New HyperLink
                hlk_File = itemD("colm_FileName").FindControl("hlk_filename")

                Dim strFileName As String = DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

                If strFileName.Trim.Length > 1 Then
                    hlk_File.Text = If(strFileName.Trim.Length > 50, Left(strFileName, 47) & "...", strFileName)
                    hlk_File.NavigateUrl = Award_document_folder & strFileName
                    hlk_File.ToolTip = strFileName
                    hlk_File.Target = "_blank"
                Else
                    hlk_File.Text = "&nbsp;"
                    hlk_File.NavigateUrl = "#"
                    hlk_File.ToolTip = "Pending to upload file"
                    hlk_File.Target = "_blank"
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

                'Dim txtCtrl As New RadTextBox
                'txtCtrl = CType(e.Item.FindControl("txtMandatory"), RadTextBox)
                'txtCtrl.ReadOnly = True
                'txtCtrl.Text = DataBinder.Eval(e.Item.DataItem, "REQUIRED_FILE").ToString()


            End Using
        End If

    End Sub



    Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_ACTIVITY_ANNEX").ToString()
            Dim Id_solicitation_app = Me.lbl_id_sol_app.Text

            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_ACTIVITY_DOCUMENTS WHERE (ID_ACTIVITY_ANNEX = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()


            dm.CommandText = "DELETE FROM TA_ACTIVITY_AW_DOCUMENTS WHERE (ID_ACTIVITY_ANNEX = " & id_temp & ")"
            dm.ExecuteNonQuery()

            cnnME.Close()

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app And p.visible.Value And p.DOCUMENTROLE = "AWARD_ANNEX").ToList()
            Me.grd_archivos.DataBind()

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)
    End Sub

    Private Sub btn_agregar_doc_Click(sender As Object, e As EventArgs) Handles btn_agregar_doc.Click


        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)



        Dim id_activity = Val(Me.lbl_id_ficha.Text)
        Dim id_solicitation_app = Val(Me.lbl_id_sol_app.Text)

        Dim id_activity_AW = Convert.ToInt32(Me.lbl_id_activity_award.Text)




        Using dbEntities As New dbRMS_JIEntities


            Dim id_solicitation = dbEntities.TA_SOLICITATION_APP.Find(id_solicitation_app).ID_ACTIVITY_SOLICITATION

            For Each file As UploadedFile In AsyncUpload1.UploadedFiles

                Dim exten = file.GetExtension()
                Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim anexo As New TA_ACTIVITY_DOCUMENTS
                Dim oACTIVITY_AW_DOCUMENTS As New TA_ACTIVITY_AW_DOCUMENTS


                anexo.DOCUMENT_TITLE = Me.txt_document_tittle.Text
                anexo.DOCUMENT_NAME = nombreArchivo
                anexo.DOCUMENTROLE = "AWARD_ANNEX"
                anexo.id_doc_soporte = cmb_type_of_document.SelectedValue
                anexo.ID_ACTIVITY = id_activity
                anexo.ID_ACTIVITY_SOLICITATION = id_solicitation
                anexo.ID_SOLICITATION_APP = id_solicitation_app
                anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                anexo.fecha_crea = Date.UtcNow
                anexo.visible = True
                anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)


                oACTIVITY_AW_DOCUMENTS.DOCUMENT_TITLE = Me.txt_document_tittle.Text
                oACTIVITY_AW_DOCUMENTS.DOCUMENT_NAME = nombreArchivo
                oACTIVITY_AW_DOCUMENTS.DOCUMENTROLE = "AWARD_ANNEX"
                oACTIVITY_AW_DOCUMENTS.id_doc_soporte = cmb_type_of_document.SelectedValue
                oACTIVITY_AW_DOCUMENTS.ID_AWARDED_ACTIVITY = id_activity_AW
                oACTIVITY_AW_DOCUMENTS.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oACTIVITY_AW_DOCUMENTS.fecha_crea = Date.UtcNow
                oACTIVITY_AW_DOCUMENTS.visible = True
                oACTIVITY_AW_DOCUMENTS.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                dbEntities.TA_ACTIVITY_DOCUMENTS.Add(anexo)

                If dbEntities.SaveChanges() Then

                    oACTIVITY_AW_DOCUMENTS.ID_ACTIVITY_ANNEX = anexo.ID_ACTIVITY_ANNEX

                    dbEntities.TA_ACTIVITY_AW_DOCUMENTS.Add(oACTIVITY_AW_DOCUMENTS)

                    If dbEntities.SaveChanges() Then

                        Dim Path As String
                        Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).awarded_documents_path)
                        file.SaveAs(Path + nombreArchivo)

                    End If

                End If


            Next



            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_activity.ToString & "&Id_AW=" & id_activity_AW.ToString() & "&_tab=tab_award"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            'document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            'Me.grd_archivos.DataSource = dbEntities.TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.ID_ACTIVITY = id).ToList()

        End Using

    End Sub

    Private Sub btn_save_aw_Click(sender As Object, e As EventArgs) Handles btn_save_aw.Click

        Using dbEntities As New dbRMS_JIEntities

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Dim oAWARD As New TA_AWARDED_APP

            Dim idAWARD = Val(LBL_ID_AWARD.Text)
            oAWARD = dbEntities.TA_AWARDED_APP.Find(idAWARD)

            Dim _TOT_AMOUNT As Double = oAWARD.TOTAL_AMOUNT
            Dim _TOT_LEVERAGED_ As Double = oAWARD.LEAVERAGED_AMOUNT

            'oAWARD.ID_AWARD_STATUS = 2 'ÁWARDED
            If oAWARD.ID_AWARD_STATUS = 1 Then
                oAWARD.AWARD_CODE = Me.lbl_award_code.Text
            End If

            If _TOT_AMOUNT <> Me.txt_obligated_usd.Value Or _TOT_LEVERAGED_ <> Me.txt_leaveraged_usd.Value Then '*** change figures from the AWARD

                oAWARD.TOTAL_AMOUNT = Me.txt_obligated_usd.Value
                oAWARD.LEAVERAGED_AMOUNT = Me.txt_leaveraged_usd.Value
                oAWARD.TOTAL_AMOUNT_LOC = Me.txt_obligated_local.Value
                oAWARD.LEAVERAGED_AMOUNT_LOC = Me.txt_leaveraged_local.Value

            End If

            oAWARD.EXCHANGE_RATE = Me.txt_Exchange_Rate_2.Value
            oAWARD.ID_BUDGET = Val(cmb_budget.SelectedValue)
            oAWARD.id_programa_currency = Val(Me.cmb_currency.SelectedValue)

            oAWARD.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
            oAWARD.ID_SUB_MECANISMO = Val(Me.cmb_sub_mecanismo_contratacion2.SelectedValue)

            dbEntities.Entry(oAWARD).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                'Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                'Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                'cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 5)   'EVALUATION / AWARDED / SELECTED
                ' SaveFicha()

                'cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_activity.ToString & "&_tab=tab_award"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End If

        End Using

    End Sub



    Private Sub save_aw() '****************ONLY cuando este en estado creado

        Using dbEntities As New dbRMS_JIEntities

            Dim id_activity = Val(Me.lbl_id_ficha.Text)
            Dim oAWARD As New TA_AWARDED_APP

            Dim idAWARD = Val(LBL_ID_AWARD.Text)
            oAWARD = dbEntities.TA_AWARDED_APP.Find(idAWARD)

            'oAWARD.ID_AWARD_STATUS = 2 'ÁWARDED

            If oAWARD.ID_AWARD_STATUS = 1 Then
                oAWARD.AWARD_CODE = Me.lbl_award_code.Text
            End If

            oAWARD.TOTAL_AMOUNT = Me.txt_obligated_usd.Value
            oAWARD.LEAVERAGED_AMOUNT = Me.txt_leaveraged_usd.Value

            oAWARD.TOTAL_AMOUNT_LOC = Me.txt_obligated_local.Value
            oAWARD.LEAVERAGED_AMOUNT_LOC = Me.txt_leaveraged_local.Value

            oAWARD.EXCHANGE_RATE = Me.txt_Exchange_Rate_2.Value
            oAWARD.ID_BUDGET = Val(cmb_budget.SelectedValue)
            oAWARD.id_programa_currency = Val(Me.cmb_currency.SelectedValue)
            oAWARD.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

            oAWARD.ID_SUB_MECANISMO = Val(Me.cmb_sub_mecanismo_contratacion.SelectedValue) '**************watch out*********************

            dbEntities.Entry(oAWARD).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then


            End If

        End Using


    End Sub

    Private Sub btn_awarded_Click(sender As Object, e As EventArgs) Handles btn_awarded.Click

        Using dbEntities As New dbRMS_JIEntities

            Dim id_activity = Val(Me.lbl_id_ficha.Text)

            Dim id_activity_AW = Convert.ToInt32(Me.lbl_id_activity_award.Text)
            Dim oAWARD As New TA_AWARDED_APP

            Dim idAWARD = Val(LBL_ID_AWARD.Text)
            oAWARD = dbEntities.TA_AWARDED_APP.Find(idAWARD)

            oAWARD.ID_AWARD_STATUS = 2 'ÁWARDED
            oAWARD.AWARD_CODE = Me.lbl_award_code.Text
            oAWARD.TOTAL_AMOUNT = Me.txt_obligated_usd.Value
            oAWARD.LEAVERAGED_AMOUNT = Me.txt_leaveraged_usd.Value
            oAWARD.EXCHANGE_RATE = Me.txt_Exchange_Rate_2.Value
            oAWARD.ID_BUDGET = Val(cmb_budget.SelectedValue)
            oAWARD.id_programa_currency = Val(Me.cmb_currency.SelectedValue)
            oAWARD.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)


            dbEntities.Entry(oAWARD).State = Entity.EntityState.Modified

            If dbEntities.SaveChanges() Then

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                cls_Solicitation.Set_TA_ACTIVITY_AW_STATUS(id_activity_AW, 5)   'EVALUATION / AWARDED / SELECTED

                cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 5)   'EVALUATION / AWARDED / SELECTED

                Dim oACTIVITY_AWARDED = dbEntities.TA_AWARDED_ACTIVITY.Find(id_activity_AW)
                oACTIVITY_AWARDED.ID_ORGANIZATION_APP = oAWARD.ID_ORGANIZATION_APP
                dbEntities.Entry(oACTIVITY_AWARDED).State = Entity.EntityState.Modified
                If dbEntities.SaveChanges() Then

                    Dim oACTIVITY = dbEntities.TA_ACTIVITY.Find(id_activity)

                    oACTIVITY.ID_ORGANIZATION_APP = oAWARD.ID_ORGANIZATION_APP
                    dbEntities.Entry(oACTIVITY).State = Entity.EntityState.Modified

                    If dbEntities.SaveChanges() Then

                        'cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_activity.ToString & "&Id_AW=" & id_activity_AW.ToString() & "&_tab=tab_award"
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                    End If


                End If


            End If

        End Using


    End Sub

    Private Sub btn_generate_activity_Click(sender As Object, e As EventArgs) Handles btn_generate_activity.Click

        Try


            Using dbEntities As New dbRMS_JIEntities

                Dim id_activity = Val(Me.lbl_id_ficha.Text)

                'Dim id_activity_aw = Val(Me.lbl_id_activity_award.Text)

                Dim id_activity_aw = 0

                Dim oAWARD As New TA_AWARDED_APP

                Dim idAWARD = Val(LBL_ID_AWARD.Text)
                oAWARD = dbEntities.TA_AWARDED_APP.Find(idAWARD)

                ' Dim oACTIVITY = dbEntities.TA_AWARDED_ACTIVITY.Find(id_activity_aw)

                Dim eXecUPD As Integer
                Dim id_awarded_old As Integer = 0
                Dim oFicha_old


                If oAWARD.ID_AWARD_STATUS = 2 Then 'ÁWARDED


                    Dim oTA_AWARDED_ACTIVITY = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAWARD).ToList()

                    For Each oACTIVITY In oTA_AWARDED_ACTIVITY

                        id_activity_aw = oACTIVITY.ID_AWARDED_ACTIVITY

                        Dim oFicha = dbEntities.tme_Ficha_Proyecto.Where(Function(p) p.ID_ACTIVITY = id_activity_aw)
                        Dim bndNEW As Boolean = False
                        Dim bndImplementer_new As Boolean = False

                        Dim idFICHA_PROYECTO As Integer = 0

                        If oFicha.Count = 0 Then
                            bndNEW = False
                            bndImplementer_new = False
                        Else
                            bndNEW = True
                            idFICHA_PROYECTO = oFicha.FirstOrDefault.id_ficha_proyecto
                            bndImplementer_new = True
                        End If

                        '******************************T_EJECUTOR**********************************
                        Dim oT_EJECUTOR As New t_ejecutores
                        Dim oTA_ORGANIZATION_APP = dbEntities.TA_ORGANIZATION_APP.Find(oACTIVITY.ID_ORGANIZATION_APP)

                        If bndImplementer_new Then
                            oT_EJECUTOR = dbEntities.t_ejecutores.Find(oFicha.FirstOrDefault.id_ejecutor)
                        Else

                            'if the activity is not new also check is there is not any other Activity created for the same Implementer
                            Dim OrgN = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_ORGANIZATION_APP = oACTIVITY.ID_ORGANIZATION_APP And p.ID_AWARDED_ACTIVITY <> id_activity_aw).Count()
                            If OrgN > 0 Then

                                id_awarded_old = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_ORGANIZATION_APP = oACTIVITY.ID_ORGANIZATION_APP And p.ID_AWARDED_ACTIVITY <> id_activity_aw).FirstOrDefault.ID_AWARDED_ACTIVITY

                                oFicha = dbEntities.tme_Ficha_Proyecto.Where(Function(p) p.ID_ACTIVITY = id_awarded_old)
                                If oFicha.Count > 0 Then

                                    oFicha_old = dbEntities.tme_Ficha_Proyecto.Where(Function(p) p.ID_ACTIVITY = id_awarded_old).FirstOrDefault()
                                    oT_EJECUTOR = dbEntities.t_ejecutores.Find(oFicha_old.id_ejecutor)
                                    bndImplementer_new = True

                                End If

                            End If

                        End If

                        oT_EJECUTOR.id_programa = oACTIVITY.id_programa
                        oT_EJECUTOR.codigo_ejecutor = ""
                        oT_EJECUTOR.nombre_ejecutor = oTA_ORGANIZATION_APP.ORGANIZATIONNAME
                        oT_EJECUTOR.nombre_corto = oTA_ORGANIZATION_APP.NAMEALIAS
                        oT_EJECUTOR.nit = oTA_ORGANIZATION_APP.ORGANIZATIONNUMBER
                        oT_EJECUTOR.telefono_ejecutor = oTA_ORGANIZATION_APP.ORGANIZATIONPHONE
                        oT_EJECUTOR.email_ejecutor = oTA_ORGANIZATION_APP.PRIMARYCONTACTEMAIL
                        oT_EJECUTOR.representante_legal = oTA_ORGANIZATION_APP.PERSONFIRSTNAME.ToString.Trim + " " + oTA_ORGANIZATION_APP.PERSONLASTNAME.ToString.Trim
                        oT_EJECUTOR.cedula_representante = oTA_ORGANIZATION_APP.PERSONID
                        oT_EJECUTOR.telefono_representante = oTA_ORGANIZATION_APP.PRIMARYCONTACTPHONE
                        oT_EJECUTOR.fecha_constitucion = oTA_ORGANIZATION_APP.ORGANIZATIONREGDATE
                        'oT_EJECUTOR.numero_socios =
                        oT_EJECUTOR.datecreated = oTA_ORGANIZATION_APP.DATECREATED
                        oT_EJECUTOR.dateupd = oTA_ORGANIZATION_APP.DATEUPDATED
                        oT_EJECUTOR.id_usuario_creo = oTA_ORGANIZATION_APP.ID_USER_CREATED
                        oT_EJECUTOR.id_usuario_upd = oTA_ORGANIZATION_APP.ID_USER_UPDATED
                        oT_EJECUTOR.id_EjecutorCL = 0
                        oT_EJECUTOR.id_estado_EJ = 1 ' Áctivo 
                        'oT_EJECUTOR.id_tipo_ejecutor =
                        'oT_EJECUTOR.id_municipio =
                        oT_EJECUTOR.id_organization_type = oTA_ORGANIZATION_APP.ID_ORGANIZATION_TYPE
                        'oT_EJECUTOR.billing_info =
                        'oT_EJECUTOR.id_vereda =
                        'oT_EJECUTOR.id_corregimiento =
                        oT_EJECUTOR.address = oTA_ORGANIZATION_APP.ADDRESSSTREET
                        oT_EJECUTOR.ADDRESSCOUNTRYREGIONID = oTA_ORGANIZATION_APP.ADDRESSCOUNTRYREGIONID
                        oT_EJECUTOR.ADDRESSSTATE = oTA_ORGANIZATION_APP.ADDRESSSTATE
                        oT_EJECUTOR.ADDRESSDISTRICTNAME = oTA_ORGANIZATION_APP.ADDRESSDISTRICTNAME
                        oT_EJECUTOR.ADDRESSCITY = oTA_ORGANIZATION_APP.ADDRESSCITY

                        If bndImplementer_new Then
                            dbEntities.Entry(oT_EJECUTOR).State = Entity.EntityState.Modified
                        Else
                            dbEntities.t_ejecutores.Add(oT_EJECUTOR)
                        End If

                        If dbEntities.SaveChanges() Then

                            '******************************FICHA_PROYECTO**********************************
                            Dim oFICHA_PROYECTO As New tme_Ficha_Proyecto

                            If bndNEW Then
                                oFICHA_PROYECTO = dbEntities.tme_Ficha_Proyecto.Find(idFICHA_PROYECTO)
                            End If

                            oFICHA_PROYECTO.id_subregion = oACTIVITY.id_subregion
                            oFICHA_PROYECTO.id_ejecutor = oT_EJECUTOR.id_ejecutor
                            oFICHA_PROYECTO.id_componente = oACTIVITY.id_componente

                            If Not bndNEW Then
                                oFICHA_PROYECTO.id_ficha_estado = 1 'Iniciado
                            End If

                            oFICHA_PROYECTO.id_periodo = oACTIVITY.id_periodo
                            oFICHA_PROYECTO.nombre_proyecto = oACTIVITY.nombre_proyecto
                            oFICHA_PROYECTO.area_intervencion = oACTIVITY.area_intervencion
                            oFICHA_PROYECTO.codigo_ficha_AID = oACTIVITY.codigo_ficha_AID
                            oFICHA_PROYECTO.codigo_RFA = oACTIVITY.codigo_RFA
                            oFICHA_PROYECTO.codigo_SAPME = oACTIVITY.codigo_SAPME
                            oFICHA_PROYECTO.codigo_MONITOR = oACTIVITY.codigo_MONITOR
                            oFICHA_PROYECTO.codigo_convenio = oACTIVITY.codigo_convenio
                            oFICHA_PROYECTO.numero_acta_aprobacion = oACTIVITY.numero_acta_aprobacion
                            oFICHA_PROYECTO.fecha_inicio_proyecto = oACTIVITY.fecha_inicio_proyecto
                            oFICHA_PROYECTO.fecha_fin_proyecto = oACTIVITY.fecha_fin_proyecto
                            oFICHA_PROYECTO.costo_total_proyecto = oACTIVITY.costo_total_proyecto
                            oFICHA_PROYECTO.tasa_cambio = oACTIVITY.tasa_cambio
                            oFICHA_PROYECTO.observaciones = oACTIVITY.observaciones

                            If Not bndNEW Then
                                oFICHA_PROYECTO.datecreated = Date.UtcNow
                                oFICHA_PROYECTO.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            End If


                            oFICHA_PROYECTO.georeferencia_completa = "NO"
                            'oFICHA_PROYECTO.fechamodificacion_MontoObligado = oACTIVITY.fechamodificacion_MontoObligado
                            oFICHA_PROYECTO.aportes_actualizados = "NO"
                            ' oFICHA_PROYECTO.idContratoME = oACTIVITY.idContratoME
                            oFICHA_PROYECTO.ActualizacionReciente = "NO"

                            If bndNEW Then
                                oFICHA_PROYECTO.id_usuario_update = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                oFICHA_PROYECTO.dateUpdate = Date.UtcNow
                            End If


                            'oFICHA_PROYECTO.id_district = oACTIVITY.id_district
                            'oFICHA_PROYECTO.isprivatepublic = oACTIVITY.isprivatepublic
                            oFICHA_PROYECTO.id_sub_mecanismo = oACTIVITY.id_sub_mecanismo
                            'oFICHA_PROYECTO.id_documento = oACTIVITY.id_documento
                            oFICHA_PROYECTO.id_usuario_responsable = oACTIVITY.id_usuario_responsable
                            oFICHA_PROYECTO.id_ficha_padre = oACTIVITY.id_ficha_padre
                            oFICHA_PROYECTO.ID_ACTIVITY = oACTIVITY.ID_AWARDED_ACTIVITY

                            If bndNEW Then
                                dbEntities.Entry(oFICHA_PROYECTO).State = Entity.EntityState.Modified
                            Else
                                dbEntities.tme_Ficha_Proyecto.Add(oFICHA_PROYECTO)
                            End If


                            If dbEntities.SaveChanges() Then

                                Dim oACTIVITY_SUB = dbEntities.TA_AWARDED_ACTIVITY_SUBREGION.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_activity_aw).ToList

                                dbEntities.Database.ExecuteSqlCommand("DELETE FROM [tme_ficha_subregion] where id_ficha_proyecto = " & idFICHA_PROYECTO.ToString)

                                For Each itemACTIVITY_SUB In oACTIVITY_SUB

                                    Dim otme_ficha_subregion As New tme_ficha_subregion

                                    otme_ficha_subregion.id_ficha_proyecto = oFICHA_PROYECTO.id_ficha_proyecto
                                    otme_ficha_subregion.id_subregion = itemACTIVITY_SUB.id_subregion
                                    otme_ficha_subregion.nivel_cobertura = itemACTIVITY_SUB.nivel_cobertura

                                    dbEntities.tme_ficha_subregion.Add(otme_ficha_subregion)

                                Next

                                If oACTIVITY_SUB.Count > 0 Then
                                    eXecUPD = dbEntities.SaveChanges()
                                Else
                                    eXecUPD = 1
                                End If

                                If eXecUPD Then

                                    eXecUPD = 0
                                    Dim oACTIVITY_FUND = dbEntities.TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_activity_aw).ToList

                                    Dim oFICHA_FUND = dbEntities.tme_AportesFicha.Where(Function(p) p.id_ficha_proyecto = idFICHA_PROYECTO).ToList()
                                    Dim bndFound As Boolean = False

                                    For Each itemACTIVITY_FUN In oACTIVITY_FUND

                                        Dim otme_aportesficha As New tme_AportesFicha

                                        For Each itemFICHA_FUND In oFICHA_FUND

                                            If itemACTIVITY_FUN.ID_APORTE = itemFICHA_FUND.id_aporte Then

                                                bndFound = True
                                                otme_aportesficha = dbEntities.tme_AportesFicha.Find(itemFICHA_FUND.id_aporteFicha)

                                                Exit For

                                            End If

                                        Next

                                        otme_aportesficha.id_ficha_proyecto = oFICHA_PROYECTO.id_ficha_proyecto
                                        otme_aportesficha.id_aporte = itemACTIVITY_FUN.ID_APORTE
                                        otme_aportesficha.tasa_Cambio = itemACTIVITY_FUN.TASA_CAMBIO
                                        otme_aportesficha.monto_aporte = itemACTIVITY_FUN.MONTO_APORTE
                                        otme_aportesficha.monto_aporte_obligado = itemACTIVITY_FUN.MONTO_APORTE_OBLIGADO
                                        otme_aportesficha.id_indicador = itemACTIVITY_FUN.ID_INDICADOR

                                        If bndFound = True Then
                                            dbEntities.Entry(otme_aportesficha).State = Entity.EntityState.Modified
                                            bndFound = False
                                        Else
                                            dbEntities.tme_AportesFicha.Add(otme_aportesficha)
                                        End If

                                    Next

                                    '******************TO REMOVE SOME ROWS***********************
                                    bndFound = False
                                    For Each itemFICHA_FUND In oFICHA_FUND

                                        For Each itemACTIVITY_FUN In oACTIVITY_FUND

                                            If itemACTIVITY_FUN.ID_APORTE = itemFICHA_FUND.id_aporte Then
                                                bndFound = True
                                                Exit For
                                            End If

                                        Next

                                        If bndFound Then
                                            bndFound = False
                                        Else
                                            dbEntities.Database.ExecuteSqlCommand("DELETE FROM [tme_AportesFicha] where id_aporteFicha = " & itemFICHA_FUND.id_aporteFicha.ToString())
                                        End If

                                    Next

                                    If oACTIVITY_FUND.Count > 0 Then
                                        eXecUPD = dbEntities.SaveChanges()
                                    Else
                                        eXecUPD = 1
                                    End If

                                    If eXecUPD Then

                                        eXecUPD = 0
                                        Dim oTA_ACTIVITY_INDICATORS = dbEntities.TA_ACTIVITY_AW_INDICATORS.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_activity_aw).ToList
                                        Dim oFICHA_ind = dbEntities.tme_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto = idFICHA_PROYECTO.ToString).ToList
                                        bndFound = False

                                        For Each itemACTIVITY_INDICATOR In oTA_ACTIVITY_INDICATORS

                                            Dim otme_meta_indicador_ficha As New tme_meta_indicador_ficha

                                            For Each itemFICHA_ind In oFICHA_ind

                                                If itemACTIVITY_INDICATOR.id_indicador = itemFICHA_ind.id_indicador Then
                                                    bndFound = True
                                                    otme_meta_indicador_ficha = dbEntities.tme_meta_indicador_ficha.Find(itemFICHA_ind.id_meta_indicador_ficha)
                                                End If

                                            Next

                                            otme_meta_indicador_ficha.id_ficha_proyecto = oFICHA_PROYECTO.id_ficha_proyecto
                                            otme_meta_indicador_ficha.id_indicador = itemACTIVITY_INDICATOR.id_indicador
                                            otme_meta_indicador_ficha.meta_total = itemACTIVITY_INDICATOR.meta_total
                                            otme_meta_indicador_ficha.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                            otme_meta_indicador_ficha.fecha_creo = Date.UtcNow
                                            otme_meta_indicador_ficha.requiere_aprobacion = "SI"


                                            If bndFound Then
                                                bndFound = False
                                                dbEntities.Entry(otme_meta_indicador_ficha).State = Entity.EntityState.Modified
                                            Else
                                                dbEntities.tme_meta_indicador_ficha.Add(otme_meta_indicador_ficha)
                                            End If

                                        Next

                                        '******************TO REMOVE SOME ROWS***********************
                                        bndFound = False
                                        For Each itemFICHA_ind In oFICHA_ind

                                            For Each itemACTIVITY_INDICATOR In oTA_ACTIVITY_INDICATORS

                                                If itemACTIVITY_INDICATOR.id_indicador = itemFICHA_ind.id_indicador Then
                                                    bndFound = True
                                                    Exit For
                                                End If

                                            Next

                                            If bndFound Then
                                                bndFound = False
                                            Else
                                                dbEntities.Database.ExecuteSqlCommand("DELETE FROM [tme_meta_indicador_ficha] where id_meta_indicador_ficha = " & itemFICHA_ind.id_meta_indicador_ficha.ToString())
                                            End If

                                        Next

                                        If oTA_ACTIVITY_INDICATORS.Count > 0 Then
                                            eXecUPD = dbEntities.SaveChanges()
                                        Else
                                            eXecUPD = 1
                                        End If

                                        '*********************************TEMPORAL STOPPED********************************************************

                                        If eXecUPD Then

                                            eXecUPD = 0
                                            Dim oTA_ACTIVITY_DELIVERABLES = dbEntities.TA_ACTIVITY_AW_DELIVERABLES.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_activity_aw).ToList()
                                            Dim oFICHA_DELIV = dbEntities.tme_ficha_entregables.Where(Function(p) p.id_ficha_proyecto = idFICHA_PROYECTO).ToList()
                                            bndFound = False

                                            Dim bndProceed As Boolean = True

                                            Dim statusList = New List(Of Integer) From {3, 4, 6}

                                            For Each itemACTIVITY_DELIV In oTA_ACTIVITY_DELIVERABLES

                                                Dim otme_ficha_entregables As New tme_ficha_entregables

                                                For Each itemFICHA_DELIV In oFICHA_DELIV

                                                    '***************************************Same number && same date***************************************
                                                    If itemACTIVITY_DELIV.ID_ACTIVITY_AW_DELIVERABLES = itemFICHA_DELIV.ID_ACTIVITY_DELIVERABLES Then
                                                        bndFound = True
                                                        otme_ficha_entregables = dbEntities.tme_ficha_entregables.Find(itemFICHA_DELIV.id_ficha_entregable)
                                                        Exit For
                                                    End If
                                                    '***************************************Same number && same date***************************************

                                                Next

                                                If bndFound = True Then
                                                    If otme_ficha_entregables.ta_deliverable.Count > 0 Then
                                                        If statusList.Contains(otme_ficha_entregables.ta_deliverable.FirstOrDefault.id_deliverable_estado) Then
                                                            bndProceed = False
                                                        End If
                                                    End If
                                                End If

                                                If bndProceed Then

                                                    otme_ficha_entregables.descripcion_entregable = itemACTIVITY_DELIV.descripcion_entregable
                                                    otme_ficha_entregables.fecha = itemACTIVITY_DELIV.fecha
                                                    otme_ficha_entregables.delivered_date = itemACTIVITY_DELIV.delivered_date
                                                    otme_ficha_entregables.valor = itemACTIVITY_DELIV.valor
                                                    otme_ficha_entregables.tasa_cambio = itemACTIVITY_DELIV.tasa_cambio
                                                    otme_ficha_entregables.valor_final = itemACTIVITY_DELIV.valor_final
                                                    otme_ficha_entregables.porcentaje = itemACTIVITY_DELIV.porcentaje
                                                    otme_ficha_entregables.id_ficha_proyecto = oFICHA_PROYECTO.id_ficha_proyecto
                                                    otme_ficha_entregables.numero_entregable = itemACTIVITY_DELIV.numero_entregable
                                                    otme_ficha_entregables.verification_mile = itemACTIVITY_DELIV.verification_mile
                                                    otme_ficha_entregables.ID_ACTIVITY_DELIVERABLES = itemACTIVITY_DELIV.ID_ACTIVITY_AW_DELIVERABLES
                                                    otme_ficha_entregables.id_tipoDocumento = itemACTIVITY_DELIV.id_tipoDocumento

                                                    If bndFound = True Then
                                                        otme_ficha_entregables.FECHA_UPDATE = Date.UtcNow
                                                        otme_ficha_entregables.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                                                    Else
                                                        otme_ficha_entregables.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                                        otme_ficha_entregables.fecha_crea = Date.UtcNow
                                                        otme_ficha_entregables.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                                                    End If

                                                    If bndFound = True Then

                                                        bndFound = False
                                                        dbEntities.Entry(otme_ficha_entregables).State = Entity.EntityState.Modified

                                                    Else

                                                        dbEntities.tme_ficha_entregables.Add(otme_ficha_entregables)

                                                    End If

                                                End If 'bndProceed

                                                'If bndFound = True Then
                                                '    bndFound = False
                                                '    If otme_ficha_entregables.ta_deliverable.Count > 0 Then
                                                '        If Not statusList.Contains(otme_ficha_entregables.ta_deliverable.FirstOrDefault.id_deliverable_estado) Then
                                                '            '  If otme_ficha_entregables.ta_deliverable.FirstOrDefault.id_deliverable_estado <> 3 Or
                                                '            'otme_ficha_entregables.ta_deliverable.FirstOrDefault.id_deliverable_estado <> 6 Or
                                                '            ' otme_ficha_entregables.ta_deliverable.FirstOrDefault.id_deliverable_estado <> 4 Then
                                                '            dbEntities.Entry(otme_ficha_entregables).State = Entity.EntityState.Modified
                                                '        End If
                                                '    Else
                                                '        dbEntities.Entry(otme_ficha_entregables).State = Entity.EntityState.Modified
                                                '    End If
                                                'Else
                                                '    dbEntities.tme_ficha_entregables.Add(otme_ficha_entregables)
                                                'End If

                                                bndFound = False
                                                bndProceed = True

                                            Next


                                            '******************TO REMOVE SOME ROWS***********************
                                            bndFound = False
                                            For Each itemFICHA_DELIV In oFICHA_DELIV

                                                For Each itemACTIVITY_DELIV In oTA_ACTIVITY_DELIVERABLES

                                                    If itemFICHA_DELIV.ID_ACTIVITY_DELIVERABLES = itemACTIVITY_DELIV.ID_ACTIVITY_AW_DELIVERABLES Then
                                                        bndFound = True
                                                        Exit For
                                                    End If

                                                Next

                                                If bndFound Then
                                                    bndFound = False
                                                Else
                                                    dbEntities.Database.ExecuteSqlCommand("DELETE FROM [tme_ficha_entregables] where id_ficha_entregable = " & itemFICHA_DELIV.id_ficha_entregable.ToString())
                                                End If

                                            Next

                                            If oTA_ACTIVITY_DELIVERABLES.Count > 0 Then
                                                eXecUPD = dbEntities.SaveChanges()
                                            Else
                                                eXecUPD = 1
                                            End If

                                            If eXecUPD Then

                                                If oACTIVITY.ID_ACTIVITY_STATUS < 7 Then
                                                    Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                                                    Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)

                                                    cls_Solicitation.Set_TA_ACTIVITY_STATUS(id_activity, 7)   ' AWARDED / ON EXECUTION
                                                    cls_Solicitation.Set_TA_ACTIVITY_AW_STATUS(id_activity_aw, 7)   ' AWARDED / ON EXECUTION

                                                End If

                                                If Not oAWARD.ACTIVITY_DATE_EXECUTUED.HasValue Then
                                                    oAWARD.ACTIVITY_DATE_EXECUTUED = Date.UtcNow
                                                    oAWARD.ID_USER_EXECUTED = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                                                End If

                                                oAWARD.ACTIVITY_LAST_UPDATED = Date.UtcNow
                                                oAWARD.ID_USER_LAST_UPDATED = Convert.ToInt32(Me.Session("E_IdUser").ToString())

                                                dbEntities.Entry(oAWARD).State = Entity.EntityState.Modified

                                                If (dbEntities.SaveChanges()) Then

                                                    '***********************IT's Done

                                                End If


                                            End If ' tme_ficha_entregables


                                        End If  ' tme_meta_indicador_ficha


                                        '*********************************TEMPORAL STOPPED********************************************************



                                    End If ' tme_AportesFicha

                                End If  'tme_ficha_subregiones

                            End If 'tme_ficha_proyecto

                        End If 'T_EJECUTOR

                        'dbEntities.Entry().State = Entity.EntityState.Modified

                    Next


                    'cl_Noti_Process.NOTIFIYING_SOLICITATION(Id_solicitation_app)
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?id=" & id_activity.ToString & "&_tab=tab_award"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


                End If

            End Using

        Catch ex As Exception

            Dim EERR As Object = ex.Message

        End Try


    End Sub

    Private Sub cmb_mecanismo_contratacion2_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanismo_contratacion2.SelectedIndexChanged


        Using dbEntities As New dbRMS_JIEntities


            Dim id_aw = 0
            Dim oPro As New TA_AWARDED_ACTIVITY
            Dim idACT_status As Integer = 0


            If e.Value IsNot Nothing Then

                If Convert.ToInt32(Me.lbl_id_activity_award.Text) > 0 Then
                    id_aw = Convert.ToInt32(Me.lbl_id_activity_award.Text)
                    oPro = dbEntities.TA_AWARDED_ACTIVITY.Find(id_aw)
                    idACT_status = oPro.ID_ACTIVITY_STATUS
                End If

                Me.cmb_sub_mecanismo_contratacion2.DataSourceID = ""
                Me.cmb_sub_mecanismo_contratacion2.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = e.Value).ToList()
                Me.cmb_sub_mecanismo_contratacion2.DataTextField = "nombre_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion2.DataValueField = "id_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion2.DataBind()
                LoadData2(idACT_status)

            Else
                LoadData2(idACT_status)
            End If

        End Using
    End Sub

    'Protected Sub cmb_awards_ItemDataBound(sender As Object, e As RadComboBoxItemEventArgs) Handles cmb_awards.ItemDataBound

    '    'e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", (DirectCast(e.Item.DataItem, DataRowView))("codigo_SAPME").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("nombre_proyecto").ToString(), String.Format("{0:d}", DateINI), String.Format("{0:d}", DateFIN))
    '    e.Item.Text = String.Format(" {0} ==>> {1} ", (DirectCast(e.Item.DataItem, DataRowView))("AWARD_CODE").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("NAMEALIAS").ToString())
    '    e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("ID_AWARDED_APP").ToString()

    'End Sub

    Protected Sub cmb_awards_DataBound(sender As Object, e As EventArgs) Handles cmb_awards.DataBound

        CType(cmb_awards.Footer.FindControl("RadComboItemsCount_award"), Literal).Text = Convert.ToString(cmb_awards.Items.Count)

    End Sub


    Private Sub cmb_awards_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_awards.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = e.Value).FirstOrDefault()

                loadActivity(idPrograma, proyecto)

                loadAWARD(e.Value, proyecto.ID_AWARDED_ACTIVITY)

            End If


        End Using



    End Sub

    Protected Sub cmb_activity_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.

        Dim DateINI As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_inicio_proyecto").ToString(), Date)
        Dim DateFIN As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_fin_proyecto").ToString(), Date)

        'codigo_SAPME
        'fecha_inicio_proyecto
        'fecha_fin_proyecto
        'id_ficha_proyecto
        'nombre_proyecto

        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", (DirectCast(e.Item.DataItem, DataRowView))("codigo_SAPME").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("nombre_proyecto").ToString(), String.Format("{0:d}", DateINI), String.Format("{0:d}", DateFIN))
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_ficha_proyecto").ToString()

    End Sub


    Private Sub cmb_awards_ItemDataBound(sender As Object, e As RadComboBoxItemEventArgs) Handles cmb_awards.ItemDataBound


        Dim DateINI As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_inicio_proyecto"), Date)
        Dim DateFIN As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_fin_proyecto"), Date)


        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", DataBinder.Eval(e.Item.DataItem, "AWARD_CODE").ToString(), DataBinder.Eval(e.Item.DataItem, "nombre_proyecto").ToString(), DateINI, DateFIN)
        e.Item.Value = DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_APP").ToString


        '        Dim cmb_partner_type As RadComboBox = CType(e.Item.FindControl("cmb_partner_type"), RadComboBox)
        '        cmb_partner_type.DataSource = db.tme_partner_type.ToList()
        '        cmb_partner_type.DataTextField = "partner_type_name"
        '        cmb_partner_type.DataValueField = "id_partner_type"
        '        cmb_partner_type.DataBind()

        '        cmb_partner_type.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partner_type").ToString()

        '        Dim cmb_partnership_focus As RadComboBox = CType(e.Item.FindControl("cmb_partnership_focus"), RadComboBox)
        '        cmb_partnership_focus.DataSource = db.tme_partnership_focus.ToList()
        '        cmb_partnership_focus.DataTextField = "partnership_focus_name"
        '        cmb_partnership_focus.DataValueField = "id_partnership_focus"
        '        cmb_partnership_focus.DataBind()

        '        cmb_partnership_focus.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partnership_focus").ToString()


        '        Dim hlnkDelete As LinkButton = New LinkButton
        '        hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
        '        hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())
        '        hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())


    End Sub




    Protected Sub chkVisible_CheckedChangedActivity(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim idActivity_selected As Integer = Convert.ToInt32(chkSelect.InputAttributes.Item("id_value"))


        Using dbEntities As New dbRMS_JIEntities

            Dim PercentProgress As Double = CDbl(Me.hd_percent_sol.Value)
            'Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            'Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            ''Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault()
            'Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(idACT)
            'PercentProgress = If(oTA_ACTIVITY.costo_total_proyecto > 0, (oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT) / oTA_ACTIVITY.costo_total_proyecto), 0) * 100

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim ID_AWARD As Integer = CInt(Me.LBL_ID_AWARD.Text)

            Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = idActivity_selected).FirstOrDefault()
            Me.lbl_id_activity_award.Text = idActivity_selected
            Dim idACT = Convert.ToInt32(Me.lbl_id_ficha.Text)
            set_links(idACT, idActivity_selected)


            For Each Irow As GridDataItem In Me.grd_activities.Items

                Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

                If Irow("ID_AWARDED_ACTIVITY").Text <> idActivity_selected Then

                    chkvisible.Checked = False

                End If

            Next

            loadActivity(idPrograma, proyecto)

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)

        End Using

        'ActualizaDatosDOCS()

        '******************************LOAD ACTIVITY HERE ****************************************
        '--loadActivity()
        '******************************LOAD ACTIVITY HERE ****************************************

        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "div_Control(false)", True)

    End Sub

    Private Sub grd_activities_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_activities.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim visible As New CheckBox
            Dim hlk_ref As New HyperLink

            visible = itemD("colm_select").FindControl("chkSelect")
            visible.Checked = False
            visible.InputAttributes.Add("id_value", itemD("ID_AWARDED_ACTIVITY").Text)

            Dim str As String = visible.InputAttributes.Item("id_value")

            visible.ToolTip = "Select an Activity"

        End If

    End Sub


    'Private Sub grd_activities_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_activities.ItemDataBound

    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
    '        Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
    '        Dim visible As New CheckBox
    '        Dim hlk_ref As New HyperLink

    '        visible = itemD("colm_select").FindControl("chkSelect")
    '        visible.Checked = False
    '        visible.InputAttributes.Add("value", itemD("id_doc_soporte").Text)

    '        Dim str As String = visible.InputAttributes.Item("value")

    '        visible.ToolTip = "Select a document"

    '        hlk_ref = itemD("colm_template").FindControl("hlk_template")

    '        If Not itemD("Template").Text.Contains("--none--") Then
    '            hlk_ref.Text = itemD("Template").Text
    '            hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
    '        Else
    '            hlk_ref.Text = itemD("Template").Text
    '            hlk_ref.NavigateUrl = "#"
    '        End If

    '    End If
    'End Sub

    Private Sub btn_add_activity_Click(sender As Object, e As EventArgs) Handles btn_add_activity.Click

        Dim a = 1
        Dim ID_AWARD As Integer = CInt(Me.LBL_ID_AWARD.Text)

        Me.txt_codigoproyecto.Text = cl_listados.Add_CodigoFicha(ID_AWARD)

        Me.divCodigo.Visible = True
        Me.lbl_mensaje.Visible = True
        Me.lbl_mensaje.Text = Me.txt_codigoproyecto.Text
        'Me.lbl_award_code.Text = Me.txt_codigoproyecto.Text

        Me.txt_nombreproyecto.Text = ""
        Me.txt_descripcion.Text = ""

        Me.cmb_sub_mecanismo_contratacion2.SelectedValue = ""

        Me.cmb_ejecutor.SelectedValue = ""
        Me.cmb_subregion.SelectedValue = ""

        Me.dt_fecha_inicio.SelectedDate = Now()
        Me.dt_fecha_fin.SelectedDate = Now()
        Me.txt_codigoRFA.Text = ""
        Me.txt_codigoMonitor.Text = ""
        'Me.cmb_periodo.SelectedValue
        'Me.txt_Exchange_Rate_2.Value 
        Me.cmb_activity_father.SelectedValue = ""
        ' Me.txt_tot_activity_amount.Value = oVW_TA_ACTIVITY.OBLIGATED_AMOUNT

        Me.txt_tot_amount_Local.Value = 0.0
        Me.txt_tot_amount.Value = 0.0

        Me.txt_obligated_usd.Value = 0.00
        Me.txt_obligated_local.Value = 0.00

        Me.txt_leaveraged_usd.Value = 0.0
        Me.txt_leaveraged_local.Value = 0.0

        Me.lbl_id_activity_award.Text = 0

        'If chk_todos.Checked Then
        '    For Each row In Me.grd_subregion.Items
        '        If TypeOf row Is GridDataItem Then
        '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
        '            Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
        '            Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
        '            If subR.Checked = True Then
        '                Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
        '                Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
        '                oSubregion.id_subregion = idSubregion
        '                oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
        '                oSubregion.nivel_cobertura = nivel_cobertura.Value
        '                oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

        '                boolAdded = True

        '            End If
        '        End If
        '    Next
        'End If

        'If Not boolAdded Then

        '    Dim oSubregion = New TA_AWARDED_ACTIVITY_SUBREGION
        '    oSubregion.id_subregion = Me.cmb_subregion.SelectedValue
        '    oSubregion.ID_AWARDED_ACTIVITY = oFicha.ID_AWARDED_ACTIVITY
        '    oSubregion.nivel_cobertura = 100
        '    oFicha.TA_AWARDED_ACTIVITY_SUBREGION.Add(oSubregion)

        'End If




    End Sub


End Class