Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class frm_ActivityF
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "ACTIVITY_FUND"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
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


            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
                Me.curr_International.Value = "USD"

                'loadListas(idPrograma, proyecto)
                'LoadData_code(id)

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

                'Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))

                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)

                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then
                    'Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))
                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                    'Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                    'Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & id.ToString()))

                Else
                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                End If


                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_activity)
                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

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
                '    'Me.alink_waiver.Attributes.Add("href", "#")
                '    'Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oVW_TA_AWARDED_APP As New VW_TA_AWARDED_APP

                If id_aw > 0 Then
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_aw).FirstOrDefault()
                Else
                    oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()
                End If



                If Not IsNothing(oVW_TA_AWARDED_APP) Then

                    Dim proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY).FirstOrDefault()
                    Dim oAportes = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = proyectoAW.ID_AWARDED_ACTIVITY)

                    'Dim oProgramaTasa = dbEntities.t_programas.Find(idPrograma).tasa_cambio
                    If proyectoAW.tasa_cambio_trimestre IsNot Nothing Then

                        'Me.tasaCambio.Value = proyecto.tasa_cambio_actividad
                        ''Me.lbl_tasa_cambio.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", Convert.ToDecimal(proyecto.tasa_cambio))
                        'Me.txt_tasa_cambio.Value = proyecto.tasa_cambio_actividad

                        Me.tasaCambio.Value = proyectoAW.tasa_cambio_actividad
                        Me.txt_tasa_cambio.Value = proyectoAW.tasa_cambio_actividad

                    End If

                    If oAportes.Count > 0 Then
                        Me.txt_tasa_cambio.Value = oAportes.FirstOrDefault.tasa_cambio 'Set the default ExR
                    End If


                    Me.cmb_awards.DataSourceID = ""
                    Me.cmb_awards.DataSource = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.id_programa = idPrograma And p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
                    Me.cmb_awards.DataTextField = "AWARD_CODE"
                    'Me.cmb_awards.DataValueField = "ID_AWARDED_APP"
                    Me.cmb_awards.DataValueField = "ID_AWARDED_ACTIVITY"
                    Me.cmb_awards.DataBind()

                    Dim idAW = oVW_TA_AWARDED_APP.ID_AWARDED_APP
                    'Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_APP
                    Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY



                    Dim actualesAp = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = proyectoAW.ID_AWARDED_ACTIVITY) _
                               .Select(Function(p) p.id_aporte.Value).ToList()

                    Me.cmb_aporte_origen.DataSource = ""
                    Me.cmb_aporte_origen.DataSource = dbEntities.tme_AportesOrigen.Where(Function(p) p.id_programa = idPrograma).ToList()
                    Me.cmb_aporte_origen.DataTextField = "nombre_AporteOrigen"
                    Me.cmb_aporte_origen.DataValueField = "id_AporteOrigen"
                    Me.cmb_aporte_origen.DataBind()

                    Me.cmb_fuente_aporte.DataSource = ""
                    Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) Not actualesAp.Contains(p.id_aporte) And p.id_programa = idPrograma).ToList()
                    Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
                    Me.cmb_fuente_aporte.DataValueField = "id_aporte"
                    Me.cmb_fuente_aporte.DataBind()

                    loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY)

                    fillGrid(True)

                End If


                setGraphFunding()

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



    Sub loadFUNDING(ByVal id_awarded_activity As Integer)

        Using dbEntities As New dbRMS_JIEntities

            'Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()
            Dim proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()
            Dim oAportes = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity)

            'Dim oProgramaTasa = dbEntities.t_programas.Find(idPrograma).tasa_cambio
            If proyectoAW.tasa_cambio_trimestre IsNot Nothing Then

                Me.tasaCambio.Value = proyectoAW.tasa_cambio_actividad
                'Me.lbl_tasa_cambio.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", Convert.ToDecimal(proyecto.tasa_cambio))
                Me.txt_tasa_cambio.Value = proyectoAW.tasa_cambio_actividad

            End If

            If oAportes.Count > 0 Then
                Me.txt_tasa_cambio.Value = oAportes.FirstOrDefault.tasa_cambio 'Set the default ExR
            End If

            fillGrid(True)

        End Using

    End Sub

    'Sub loadAWARD(ByVal id_awarded_app As Integer)
    Sub loadAWARD(ByVal id_awarded_activity As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()
            Dim id_awarded_app = oVW_TA_AWARDED_APP.ID_AWARDED_APP
            Dim oTA_AWARDED_APP = dbEntities.TA_AWARDED_APP.Find(id_awarded_app)
            Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            Dim PercentProgress As Double

            If Not IsNothing(oTA_AWARDED_APP) Then

                Me.LBL_ID_AWARD.Text = oTA_AWARDED_APP.ID_AWARDED_APP

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_activity = idActivity).FirstOrDefault()
                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault()
                Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()

                'set_links(id_awarded_activity)
                Dim idACT_ = Convert.ToInt32(Me.lbl_id_ficha.Text)

                set_links(idACT_, id_awarded_activity)
                Me.cmb_awards.SelectedValue = id_awarded_activity


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

                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()
                'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault().ID_AWARDED_ACTIVITY
                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()
                'Me.grd_archivos.DataBind()

            End If

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)

        End Using

    End Sub



    Sub fillGrid(ByVal bBind As Boolean)
        Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Using dbEntities As New dbRMS_JIEntities

            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            ' Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue


            Dim aportesFicha = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).ToList()
            Me.grd_aportes.DataSource = aportesFicha

            valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)

            If bBind Then
                Me.grd_aportes.DataBind()
            End If

            sumaAportes()

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
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM TA_ACTIVITY_AW_FUNDING WHERE ID_ACTIVITY_AW_FUNDING = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try

            'Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
            'Me.grd_aportes.DataSource = ""
            'Me.grd_aportes.DataSource = dbEntities.VW_TA_ACTIVITY_FUNDING.Where(Function(p) p.ID_ACTIVITY = id).ToList()
            'Me.grd_aportes.DataBind()
            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY

            Dim actualesAp = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
                               .Select(Function(p) p.id_aporte.Value).ToList()

            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) p.id_AporteOrigen = Me.cmb_aporte_origen.SelectedValue And (Not actualesAp.Contains(p.id_aporte))).ToList()
            Me.cmb_fuente_aporte.DataBind()

            fillGrid(True)

            guardarAportes()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Sub sumaAportes()

        valorSuma = 0
        For Each row In Me.grd_aportes.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                'Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_FUNDING")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
                valorSuma += TotalIndicador.Value
            End If
        Next
        If valorSuma = 0 Then
            'Me.lbl_total.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_total.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", 0)
            'Me.lbl_totalUSD.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_totalUSD.Text = String.Format("$ {0:#,###,###.##}", 0)
        Else
            'Me.lbl_total.Text = valorSuma.ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_total.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", valorSuma)
            Me.lbl_totalUSD.Text = String.Format("$ {0:#,###,###.##}", (valorSuma / Me.tasaCambio.Value))
        End If

    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        guardarAportes()

        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityF?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub txt_meta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        sumaAportes()
    End Sub


    Sub guardarAportes()
        valorSuma = 0
        Dim sql As String = ""
        For Each row In Me.grd_aportes.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_FUNDING")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
                sql = "UPDATE TA_ACTIVITY_AW_FUNDING SET monto_aporte=" & TotalIndicador.Value & ", monto_aporte_obligado=" & TotalIndicador.Value & ", tasa_cambio= " & Me.txt_tasa_cambio.Value
                sql &= " WHERE ID_ACTIVITY_AW_FUNDING = " & IDInstrumentoID.ToString
                valorSuma += TotalIndicador.Value
                Using dbEntities As New dbRMS_JIEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
        Next
    End Sub


    Public Sub setGraphFunding()

        Using dbEntities As New dbRMS_JIEntities

            Dim id_ficha As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim serializer As New JavaScriptSerializer()

            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY

            Dim AportesSRC = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
                                        .OrderBy(Function(p) p.nombre_aporte) _
                                        .Select(Function(p) New With {Key _
                                                                    .name = p.nombre_aporte,
                                                                    .y = p.monto_aporte}).ToList()

            Dim strEmpy As String = "[{""name"": ""none"", ""y"": 0}]"
            Dim strValues As String = ""

            If AportesSRC.Count > 0 Then
                For Each item In AportesSRC
                    strValues &= "{ ""name"": """ & item.name & """, ""y"": " & item.y & " },"
                Next
            Else
                strValues = "{""name"": ""NF"", ""y"": 0}"
            End If

            'Me.hdnFunding.Value = If(AportesSRC.Count > 0, serializer.Serialize(AportesSRC), strEmpy)
            Me.hdnFunding.Value = strValues

        End Using



    End Sub

    Protected Sub btn_guardarAporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarAporte.Click

        guardarAportes()


        Using dbEntities As New dbRMS_JIEntities

            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

            Dim oAporte = New TA_ACTIVITY_AW_FUNDING
            oAporte.ID_AWARDED_ACTIVITY = idAW_ACT
            oAporte.MONTO_APORTE = 0
            oAporte.TASA_CAMBIO = Me.txt_tasa_cambio.Value
            oAporte.ID_APORTE = Me.cmb_fuente_aporte.SelectedValue
            oAporte.MONTO_APORTE_OBLIGADO = 0

            dbEntities.TA_ACTIVITY_AW_FUNDING.Add(oAporte)
            dbEntities.SaveChanges()

            'Dim aportesFicha = dbEntities.VW_TA_ACTIVITY_FUNDING.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
            'Me.grd_aportes.DataSource = aportesFicha
            'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)
            fillGrid(True)
            sumaAportes()

            Dim actualesAp = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
                               .Select(Function(p) p.id_aporte.Value).ToList()

            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) Not actualesAp.Contains(p.id_aporte)).ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()

        End Using

        'Me.grd_aportes.DataBind()

    End Sub

    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
            Dim TotalIndicadorUSD As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_aporte_usd"), RadNumericTextBox)
            TotalIndicador.Text = DataBinder.Eval(e.Item.DataItem, "monto_aporte").ToString()
            TotalIndicadorUSD.Text = DataBinder.Eval(e.Item.DataItem, "TotalUSD").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_FUNDING").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_FUNDING").ToString())
        End If
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.MsgReturn.Redireccion = "~/RFP_/frm_ActivityAW?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub cmb_aporte_origen_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_aporte_origen.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY

            Dim actualesAp = dbEntities.VW_TA_ACTIVITY_AW_FUNDING.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT) _
                               .Select(Function(p) p.id_aporte.Value).ToList()
            Me.cmb_fuente_aporte.DataSource = ""
            Dim eee = Me.cmb_fuente_aporte.SelectedValue
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) p.id_AporteOrigen = Me.cmb_aporte_origen.SelectedValue And p.id_programa = id_programa And (Not actualesAp.Contains(p.id_aporte))).ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()
            Me.cmb_fuente_aporte.Text = ""

        End Using


    End Sub

    Private Sub cmb_awards_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_awards.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                'Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                'Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = e.Value).FirstOrDefault()
                loadAWARD(e.Value)
                loadFUNDING(e.Value)
                fillGrid(True)


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

End Class