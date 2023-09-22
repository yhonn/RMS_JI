Imports ly_SIME
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_ActivityDeliv
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_DELIV"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
    Dim sumaPorcentaje As Decimal = 0
    Dim sumaAportesTotal As Decimal = 0
    Dim dtDeliverable_Routes As DataTable


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

            Me.btn_eliminarAportes.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto

        End If

        If Not Me.IsPostBack Then

            'Me.grd_aportes.DataSource = Nothing
            'Me.txt_total_aporte.Value = 0
            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtDeliverable_Routes.Value = String.Format("dtDeliverable_Routes{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
                Me.curr_International.Value = "USD"

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                'Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

                'Dim idAporte As Integer = If(proyecto.perfijo_sub_mecanismo = "STO", 9, If(proyecto.perfijo_sub_mecanismo = "PO", 37, If(proyecto.perfijo_sub_mecanismo = "INK", 36, 9)))

                'Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosEdit?Id=" & id.ToString()))
                'Me.alink_areas.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_actividad_areas?Id=" & id.ToString()))
                ''Me.alink_regionbeneficiada.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosRegion?Id=" & id.ToString()))
                ''Me.alink_value_chain.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosValueChain?Id=" & id.ToString()))
                'Me.alink_aportes.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosAportes?Id=" & id.ToString()))
                'Me.alink_indicadores.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosIndicadores?Id=" & id.ToString()))
                'Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosDocumentos?Id=" & id.ToString()))
                '' Me.alink_waiver.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosWaiver?Id=" & id.ToString()))

                Dim id_aw As Integer = 0

                If Not IsNothing(Me.Request.QueryString("Id_AW")) Then
                    id_aw = Convert.ToInt32(Val(Me.Request.QueryString("Id_AW").ToString))
                End If

                Me.lbl_id_ficha_aw.Text = id_aw

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))

                Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))
                Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))



                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)

                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    'Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                    'Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))
                    'Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & id.ToString()))

                Else
                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_activity)

                Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
                dtDeliverable_Routes = cls_Deliverable.get_Deliverable_Approval()

                cmb_approvals.DataSource = dtDeliverable_Routes
                cmb_approvals.DataValueField = "id_TipoDocumento"
                cmb_approvals.DataTextField = "descripcion_aprobacion"
                cmb_approvals.DataBind()


                Me.cmb_awards.DataSourceID = ""
                Me.cmb_awards.DataSource = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.id_programa = idPrograma And p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
                Me.cmb_awards.DataTextField = "AWARD_CODE"
                Me.cmb_awards.DataValueField = "ID_AWARDED_ACTIVITY"
                Me.cmb_awards.DataBind()

                '--*********************************************************************************************************************************************

                'Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()
                Dim idAW = 0

                Dim oVW_TA_AWARDED_APP As New VW_TA_AWARDED_APP

                If id_aw > 0 Then
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw).FirstOrDefault()
                Else
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()
                End If

                Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY

                If Not IsNothing(oVW_TA_AWARDED_APP) Then

                    Dim proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()

                    Me.lbl_informacionproyecto.Text = "(" + proyectoAW.codigo_ficha_AID + ")" + " " + proyectoAW.nombre_proyecto

                    idAW = oVW_TA_AWARDED_APP.ID_AWARDED_APP

                    'loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_APP)
                    loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY)


                End If

                '--*********************************************************************************************************************************************


               
                Session(Me.hd_dtDeliverable_Routes.Value) = dtDeliverable_Routes

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
                '    'Me.alink_waiver.Attributes.Add("href", "#")
                '    'Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:block;")
                'Else
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                '    If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "FP" Then
                '        Me.alink_po.Attributes.Add("style", "display:block;")
                '        Me.alink_Ik.Attributes.Add("style", "display:block;")
                '    Else
                '        Me.alink_po.Attributes.Add("style", "display:none;")
                '        Me.alink_Ik.Attributes.Add("style", "display:none;")
                '    End If
                'End If


                'loadListas(idPrograma, proyecto)
                'LoadData_code(id)

                'Dim aportesFicha = dbEntities.TA_ACTIVITY_DELIVERABLES.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text) _
                '                   .OrderBy(Function(p) p.numero_entregable).ToList()

                'If aportes = 0 Then
                '    Me.btn_guardarEntregable.Enabled = False
                'End If

                FillGrid(True)


            End Using

        Else

            dtDeliverable_Routes = Session(Me.hd_dtDeliverable_Routes.Value)


        End If

    End Sub



    Sub set_links(ByVal idACT As Integer, ByVal id_aw As Integer)

        Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))

        Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))


    End Sub


    ' Sub loadAWARD(ByVal id_awarded_app As Integer)
    Sub loadAWARD(ByVal id_awarded_activity As Integer)


        Using dbEntities As New dbRMS_JIEntities

            Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()


            Me.cmb_awards.SelectedValue = id_awarded_activity

            Dim id_awarded_app = oVW_TA_AWARDED_APP.ID_AWARDED_APP
            Dim oTA_AWARDED_APP = dbEntities.TA_AWARDED_APP.Find(id_awarded_app)
            Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            set_links(idACT, id_awarded_activity)
            Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            Dim PercentProgress As Double

            If Not IsNothing(oTA_AWARDED_APP) Then

                Me.LBL_ID_AWARD.Text = oTA_AWARDED_APP.ID_AWARDED_APP

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_activity = idActivity).FirstOrDefault()
                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault()
                Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()


                Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(idACT)

                Me.lbl_implementer.Text = oVW_TA_ACTIVITY.ORGANIZATIONNAME
                Me.lbl_activity_name.Text = oVW_TA_ACTIVITY.nombre_proyecto
                'Me.lbl_activity_Code.Text = oTA_AWARDED_APP.AWARD_CODE
                Me.lbl_activity_Code.Text = oVW_TA_ACTIVITY.codigo_SAPME

                Me.lbl_last_Deliverable.Text = oTA_AWARDED_APP.TA_AWARD_STATUS.AWARD_STATUS

                Me.lbl_totalACT2.Text = String.Format("{0:N2} USD", oTA_ACTIVITY.costo_total_proyecto)
                Me.lbl_totalACT2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_ACTIVITY.costo_total_proyecto_LOC, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                'proyectos.Sum(Function(p) p.tme_AportesFicha.Sum(Function(q) q.monto_aporte_obligado))
                PercentProgress = If(oTA_ACTIVITY.costo_total_proyecto > 0, (oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT) / oTA_ACTIVITY.costo_total_proyecto), 0) * 100

                Me.lbl_totalPerf2.Text = String.Format("{0:N2} USD", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT))
                Me.lbl_totalPerf2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT_LOC), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)


                Me.lbl_period.Text = String.Format("{0:dd/MM/yyyy} to {1:dd/MM/yyyy}", oVW_TA_ACTIVITY.fecha_inicio_proyecto, oVW_TA_ACTIVITY.fecha_fin_proyecto)

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))


                '*********************************************************************************************************************************************
                Dim oMecanismo = dbEntities.tme_sub_mecanismo.FirstOrDefault(Function(p) p.id_sub_mecanismo = oVW_TA_ACTIVITY.id_sub_mecanismo)

                Dim idAporteOrigen As Integer = dbEntities.ta_activity_setup.Where(Function(p) p.id_programa = idPrograma).FirstOrDefault().data01

                Dim idAporte As Integer = oMecanismo.id_aporte
                Dim aport = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity And p.id_AporteOrigen = idAporteOrigen And p.id_aporte = idAporte)
                Dim aportes As Double = 0
                If aport.Count() > 0 Then
                    aportes = aport.Sum(Function(p) p.monto_aporte)
                End If

                'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault().ID_AWARDED_ACTIVITY

                Dim Entregables = dbEntities.VW_TA_ACTIVITY_AW_DELIVERABLES.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity)
                Dim TotEntregable As Double = If(Entregables.Count > 0, Entregables.Sum(Function(p) p.valor), 0)

                Dim tasaCambio As Double = If(Entregables.Count > 0, Entregables.FirstOrDefault.tasa_cambio, oVW_TA_ACTIVITY.tasa_cambio)
                Me.proy_tasa_cambio.Value = tasaCambio
                Me.txt_tasa_cambio.Value = tasaCambio
                Me.lbl_monto_aportes.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", aportes, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                Me.lbl_monto_aportesUSD.Text = If(tasaCambio > 0, String.Format(sesUser.regionalizacionCulture, "{0:N2} USD", Math.Round(aportes / tasaCambio, 2, MidpointRounding.AwayFromZero)), 0)
                ' Me.lbl_monto_aportes.Text = aportes.ToString("N2", cl_user.regionalizacionCulture)
                Me.monto_proyecto.Value = aportes

                Me.lbl_monto_entregables.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", TotEntregable, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                Me.lbl_monto_entregablesUSD.Text = If(tasaCambio > 0, String.Format(sesUser.regionalizacionCulture, "{0:N2} USD", Math.Round(TotEntregable / tasaCambio, 2, MidpointRounding.AwayFromZero)), 0)
                Me.lbl_monto_entregablesPorc.Text = If(aportes > 0, String.Format(sesUser.regionalizacionCulture, "{0:P2}", TotEntregable / aportes), 0)

                Me.dt_fecha.MinDate = oVW_TA_ACTIVITY.fecha_inicio_proyecto
                Me.dt_fecha.MaxDate = oVW_TA_ACTIVITY.fecha_fin_proyecto

                '***********************************************************************************************************************

                ' FillGrid(False)

                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()
                'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault().ID_AWARDED_ACTIVITY
                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()
                'Me.grd_archivos.DataBind()

            End If

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)

        End Using

    End Sub


    Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_eliminarAportes_Click(sender As Object, e As EventArgs) Handles btn_eliminarAportes.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM TA_ACTIVITY_AW_DELIVERABLES WHERE ID_ACTIVITY_AW_DELIVERABLES = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)


            'Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY


            'Dim aportesFicha = dbEntities.VW_TA_ACTIVITY_AW_DELIVERABLES.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
            '                  .OrderBy(Function(p) p.numero_entregable).ToList()


            'Me.grd_aportes.DataSource = ""
            'Me.grd_aportes.DataSource = aportesFicha
            'Me.grd_aportes.DataBind()
            FillGrid(True)

            guardarAportes()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Sub sumaAportes()
        For Each row In Me.grd_aportes.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_DELIVERABLES")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
                valorSuma += TotalIndicador.Value
            End If
        Next
        'If valorSuma = 0 Then
        '    Me.lbl_total.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
        '    Me.lbl_totalUSD.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
        'Else
        '    Me.lbl_total.Text = valorSuma.ToString("c2", cl_user.regionalizacionCulture)
        '    Me.lbl_totalUSD.Text = (valorSuma / 1000).ToString("c2", cl_user.regionalizacionCulture)
        'End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        guardarAportes()
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        'Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosEntregables?id=" & Me.lbl_id_ficha.Text
        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityAW?" & "id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub txt_meta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        sumaAportes()
    End Sub


    Sub guardarAportes()
        valorSuma = 0
        Dim sql As String = ""
        Using dbEntities As New dbRMS_JIEntities
            For Each row In Me.grd_aportes.Items
                If TypeOf row Is GridDataItem Then

                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_DELIVERABLES")
                    Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
                    Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
                    Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)
                    Dim numero As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_numero_ent"), RadNumericTextBox)
                    Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
                    Dim verification As RadTextBox = CType(row.Cells(0).FindControl("txt_verification_deliverable"), RadTextBox)
                    Dim combTipoDoc As RadComboBox = CType(row.Cells(1).FindControl("cmb_deliv_route"), RadComboBox)

                    Dim oEntregable = dbEntities.TA_ACTIVITY_AW_DELIVERABLES.Find(IDInstrumentoID)

                    oEntregable.verification_mile = verification.Text
                    oEntregable.descripcion_entregable = descripcion_entregable.Text
                    oEntregable.fecha = fecha.SelectedDate
                    oEntregable.valor = TotalIndicador.Value
                    oEntregable.porcentaje = porcentaje.Value
                    oEntregable.numero_entregable = numero.Value
                    oEntregable.id_tipoDocumento = combTipoDoc.SelectedValue

                    dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified

                End If
            Next

            dbEntities.SaveChanges()

        End Using
    End Sub


    Protected Sub btn_guardarEntregable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarEntregable.Click

        Using dbEntities As New dbRMS_JIEntities

            Dim oEntregable = New TA_ACTIVITY_AW_DELIVERABLES
            Dim idFicha = CType(Me.lbl_id_ficha.Text, Int32)

            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

            oEntregable.ID_AWARDED_ACTIVITY = idAW_ACT
            oEntregable.descripcion_entregable = Me.txt_descripcion_entregable.Text.Trim
            oEntregable.verification_mile = Me.txt_verification_.Text.Trim
            oEntregable.fecha = Me.dt_fecha.SelectedDate

            'Dim exchange_rat As Double = Me.txt_tasa_cambio.Value
            Dim tasaCambio As Double = Me.txt_tasa_cambio.Value 'CDbl(Me.proy_tasa_cambio.Value)
            Dim valorEntregable As Double = If(Me.chk_data_in.Checked, (Me.txt_total_aporte.Value * tasaCambio), Me.txt_total_aporte.Value)

            oEntregable.valor = valorEntregable
            oEntregable.tasa_cambio = tasaCambio
            oEntregable.porcentaje = Me.txt_porcentaje.Value
            oEntregable.numero_entregable = Me.txt_numero_entregable.Value
            oEntregable.id_tipoDocumento = Me.cmb_approvals.SelectedValue

            dbEntities.TA_ACTIVITY_AW_DELIVERABLES.Add(oEntregable)
            dbEntities.SaveChanges()

            'Dim aportesFicha = dbEntities.TA_ACTIVITY_DELIVERABLES.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
            'Me.grd_aportes.DataSource = aportesFicha

            FillGrid(True)
            'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)
            sumaAportes()


            guardarAportes()
        Me.grd_aportes.DataBind()
        Me.txt_descripcion_entregable.Text = ""
        Me.dt_fecha.Clear()
        Me.txt_total_aporte.Value = 0
        Me.txt_porcentaje.Value = 0

        Session.Remove(Me.hd_dtDeliverable_Routes.Value)

        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityDeliv?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using

    End Sub

    Private Sub cmb_awards_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_awards.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                'Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                'Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = e.Value).FirstOrDefault()
                loadAWARD(e.Value)

                FillGrid(True)

            End If

        End Using

    End Sub


    Protected Sub cmb_awards_DataBound(sender As Object, e As EventArgs) Handles cmb_awards.DataBound

        CType(cmb_awards.Footer.FindControl("RadComboItemsCount_award"), Literal).Text = Convert.ToString(cmb_awards.Items.Count)

    End Sub


    Private Sub cmb_awards_ItemDataBound(sender As Object, e As RadComboBoxItemEventArgs) Handles cmb_awards.ItemDataBound

        Dim DateINI As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_inicio_proyecto"), Date)
        Dim DateFIN As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_fin_proyecto"), Date)

        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", DataBinder.Eval(e.Item.DataItem, "AWARD_CODE").ToString(), DataBinder.Eval(e.Item.DataItem, "nombre_proyecto").ToString(), DateINI, DateFIN)
        'e.Item.Value = DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_APP").ToString
        e.Item.Value = DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_ACTIVITY").ToString

    End Sub


    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
            Dim TotalAporteUSD As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_col_total_aporteUSD"), RadNumericTextBox)
            TotalIndicador.Value = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "valor"))
            TotalAporteUSD.Value = (Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "valor")) / Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "tasa_cambio")))

            Dim lbl_intenational As Label = CType(e.Item.Cells(0).FindControl("lbl_curr_international"), Label)
            Dim lbl_local As Label = CType(e.Item.Cells(0).FindControl("lbl_curr_local"), Label)

            Dim TXTtasa As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_tasa_cambio_"), RadNumericTextBox)
            TXTtasa.Value = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "tasa_cambio"))
            TXTtasa.Visible = True
            TXTtasa.Display = False

            lbl_intenational.Text = Me.curr_International.Value
            lbl_local.Text = Me.curr_local.Value

            Dim descripcion_entregable As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
            descripcion_entregable.Text = DataBinder.Eval(e.Item.DataItem, "descripcion_entregable").ToString()

            Dim verification_entregable As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_verification_deliverable"), RadTextBox)

            If Not IsNothing(DataBinder.Eval(e.Item.DataItem, "verification_mile")) Then
                verification_entregable.Text = DataBinder.Eval(e.Item.DataItem, "verification_mile").ToString()
            Else
                verification_entregable.Text = ""
            End If

            Dim numero_entregable As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_numero_ent"), RadNumericTextBox)
            numero_entregable.Value = DataBinder.Eval(e.Item.DataItem, "numero_entregable").ToString()

            Dim fecha As RadDatePicker = CType(e.Item.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
            fecha.SelectedDate = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "fecha"))
            fecha.MinDate = Me.dt_fecha.MinDate
            fecha.MaxDate = Me.dt_fecha.MaxDate

            Dim porcentaje As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)
            porcentaje.Value = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "porcentaje"))

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_DELIVERABLES").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_DELIVERABLES").ToString())

            Dim cmb_tp As RadComboBox = CType(itemD("colm_descripcion_entregable").FindControl("cmb_deliv_route"), RadComboBox)
            cmb_tp.DataSource = dtDeliverable_Routes
            cmb_tp.DataTextField = "descripcion_aprobacion"
            cmb_tp.DataValueField = "id_tipoDocumento"
            cmb_tp.DataBind()

            cmb_tp.SelectedValue = itemD("id_tipoDocumento").Text


        End If
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click

        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.MsgReturn.Redireccion = "~/RFP_/frm_ActivityAW?Id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)

    End Sub

    Protected Sub txt_porcentaje_TextChanged(sender As Object, e As EventArgs)

        'If Val(Me.hd_updating.Value) = 0 Then

        '    Me.hd_updating.Value = 1 'Updating
        '    Dim valor_total = Me.monto_proyecto.Value
        '    Me.txt_total_aporte.Value = valor_total * txt_porcentaje.Value / 100
        '    Me.hd_updating.Value = 0 'Not Updating

        'End If

    End Sub

    ''Protected Sub txt_col_aporte_TextChanged(sender As Object, e As EventArgs)
    ''    validarValorTotal()
    ''End Sub

    ''Sub validarValorTotal()
    ''    valorSuma = 0
    ''    Dim sql As String = ""
    ''    For Each row In Me.grd_aportes.Items
    ''        If TypeOf row Is GridDataItem Then
    ''            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    ''            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_DELIVERABLES")
    ''            Dim total_aporte As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
    ''            Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
    ''            Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
    ''            Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)

    ''            Dim valor_total = Me.monto_proyecto.Value

    ''            porcentaje.Value = total_aporte.Value * 100 / valor_total
    ''            sumaAportesTotal += total_aporte.Value
    ''            If sumaPorcentaje > 100 Then
    ''                Me.div_mensaje.Visible = True
    ''                Me.btn_guardar.Enabled = False
    ''            Else
    ''                Me.div_mensaje.Visible = False
    ''                Me.btn_guardar.Enabled = True
    ''            End If

    ''        End If
    ''    Next
    ''End Sub

    'Protected Sub txt_porcentaje_TextChanged1(sender As Object, e As EventArgs) Handles txt_porcentaje.TextChanged

    '    sumaPorcentaje = txt_porcentaje.Value
    '    Dim sql As String = ""
    '    For Each row In Me.grd_aportes.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_DELIVERABLES")
    '            Dim total_aporte As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
    '            Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
    '            Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
    '            Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)

    '            Dim valor_total = Me.monto_proyecto.Value

    '            total_aporte.Value = valor_total * porcentaje.Value / 100
    '            sumaPorcentaje += porcentaje.Value
    '            If sumaPorcentaje > 100 Then
    '                Me.div_mensaje.Visible = True
    '                Me.btn_guardarEntregable.Enabled = False
    '            Else
    '                Me.btn_guardarEntregable.Enabled = True
    '                Me.div_mensaje.Visible = False
    '            End If

    '        End If
    '    Next
    'End Sub

    Protected Sub txt_total_aporte_TextChanged(sender As Object, e As EventArgs) Handles txt_total_aporte.TextChanged

        'If Val(Me.hd_updating.Value) = 0 Then

        '    Me.hd_updating.Value = 1 'Updating

        '    valorSuma = txt_total_aporte.Value
        '    Dim valor_total = Me.monto_proyecto.Value

        '    txt_porcentaje.Value = txt_total_aporte.Value * 100 / valor_total
        '    Dim sql As String = ""

        '    For Each row In Me.grd_aportes.Items

        '        If TypeOf row Is GridDataItem Then
        '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
        '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_DELIVERABLES")
        '            Dim total_aporte As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
        '            'Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
        '            Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
        '            Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)

        '            sumaAportesTotal += total_aporte.Value
        '            If sumaAportesTotal > Me.monto_proyecto.Value Then
        '                Me.div_mensaje.Visible = True
        '                Me.btn_guardarEntregable.Enabled = False
        '            Else
        '                Me.btn_guardarEntregable.Enabled = True
        '                Me.div_mensaje.Visible = False
        '            End If

        '        End If
        '    Next

        '    Me.hd_updating.Value = 0 'Not Updating

        'End If

    End Sub

    Protected Sub grd_aportes_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_aportes.NeedDataSource
        FillGrid(False)
    End Sub
    Protected Sub FillGrid(ByVal bBind As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            'Me.lbl_id_ficha.Text
            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

            Dim aportesFicha = dbEntities.VW_TA_ACTIVITY_AW_DELIVERABLES.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
                              .OrderBy(Function(p) p.numero_entregable).ToList()

            Me.grd_aportes.DataSource = aportesFicha
            'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)
            'sumaAportes()

            If bBind Then
                Me.grd_aportes.DataBind()
            End If

        End Using

    End Sub




End Class