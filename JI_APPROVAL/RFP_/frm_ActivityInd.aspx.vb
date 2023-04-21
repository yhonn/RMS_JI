Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_ActivityInd
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_IND"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
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

            Me.btn_eliminarIndicador.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities


                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                'loadListas(idPrograma, proyecto)

                'LoadData_code(id)

                Dim id_aw As Integer = 0

                If Not IsNothing(Me.Request.QueryString("Id_AW")) Then
                    id_aw = Convert.ToInt32(Val(Me.Request.QueryString("Id_AW").ToString))
                End If



                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
                'Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString()))
                'Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString()))

                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)
                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    'Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                    'Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))
                    'Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))

                Else

                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")

                End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_activity)
                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

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


                Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()

                If Not IsNothing(oVW_TA_AWARDED_APP) Then

                    Dim proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()

                    Me.cmb_awards.DataSourceID = ""
                    Me.cmb_awards.DataSource = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.id_programa = idPrograma And p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
                    Me.cmb_awards.DataTextField = "AWARD_CODE"
                    Me.cmb_awards.DataValueField = "ID_AWARDED_ACTIVITY"
                    Me.cmb_awards.DataBind()

                    Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY

                    Me.lbl_informacionproyecto.Text = "(" + proyectoAW.codigo_ficha_AID + ")" + " " + proyectoAW.nombre_proyecto

                    Dim idAW = oVW_TA_AWARDED_APP.ID_AWARDED_APP
                    ' Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_APP
                    loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY)

                End If


                fillGrid(True)

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


    Sub loadAWARD(ByVal id_awarded_activity As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()

            Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            Dim PercentProgress As Double

            If Not IsNothing(oVW_TA_AWARDED_APP) Then

                Me.LBL_ID_AWARD.Text = oVW_TA_AWARDED_APP.ID_AWARDED_APP

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_activity = idActivity).FirstOrDefault()
                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault()
                Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity).FirstOrDefault()

                Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
                set_links(idACT, id_awarded_activity)
                Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(idACT)

                Me.lbl_implementer.Text = oVW_TA_ACTIVITY.ORGANIZATIONNAME
                Me.lbl_activity_name.Text = oVW_TA_AWARDED_APP.nombre_proyecto
                Me.lbl_activity_Code.Text = oVW_TA_AWARDED_APP.AWARD_CODE

                Me.lbl_last_Deliverable.Text = oVW_TA_AWARDED_APP.AWARD_STATUS


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


                Dim actuales = dbEntities.TA_ACTIVITY_AW_INDICATORS.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_awarded_activity) _
                            .Select(Function(p) p.id_indicador).ToList()

                Me.cmb_indicador.DataSource = ""
                Me.cmb_indicador.DataSource = dbEntities.vw_indicadores _
                    .Where(Function(p) Not actuales.Contains(p.id_indicador) And p.id_programa = idPrograma).OrderBy(Function(p) p.codigo_indicador) _
                    .Select(Function(p) _
                                          New With {Key .nombre_indicador_LB = "(" + p.codigo_indicador + ") - " & p.nombre_indicador_LB,
                                                    Key .id_indicador = p.id_indicador}) _
                    .ToList()

                Me.cmb_indicador.DataTextField = "nombre_indicador_LB"
                Me.cmb_indicador.DataValueField = "id_indicador"
                Me.cmb_indicador.DataBind()


            End If

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)

        End Using

    End Sub




    Protected Sub EliminarIndicador_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_eliminarIndicador_Click(sender As Object, e As EventArgs) Handles btn_eliminarIndicador.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM TA_ACTIVITY_AW_INDICATORS WHERE ID_ACTIVITY_AW_INDICATORS = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
            fillGrid(True)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Sub fillGrid(ByVal bBind As Boolean)
        Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Using dbEntities As New dbRMS_JIEntities

            'Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

            Me.grd_indicadores.DataSource = dbEntities.VW_TA_ACTIVITY_AW_TARGET.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).OrderBy(Function(p) p.codigo_indicador).ToList()

            If bBind Then
                Me.grd_indicadores.DataBind()
            End If


        End Using
    End Sub

    Protected Sub btn_guardarIndicador_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarIndicador.Click
        'guardarIndicadores()

        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

        Using dbEntities As New dbRMS_JIEntities

            'Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY

            Dim oMetaIndicador = New TA_ACTIVITY_AW_INDICATORS
            oMetaIndicador.ID_AWARDED_ACTIVITY = idAW_ACT
            oMetaIndicador.meta_total = 0
            oMetaIndicador.id_indicador = Me.cmb_indicador.SelectedValue
            oMetaIndicador.fecha_creo = Date.UtcNow
            oMetaIndicador.id_usuario_creo = Me.Session("E_IdUser").ToString()

            dbEntities.TA_ACTIVITY_AW_INDICATORS.Add(oMetaIndicador)
            dbEntities.SaveChanges()

            'listaIndicadores()
        End Using
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityInd?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub grd_indicadores_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_indicadores.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_TotalIndicador"), RadNumericTextBox)
            TotalIndicador.Text = DataBinder.Eval(e.Item.DataItem, "meta_total").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_INDICATORS").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY_AW_INDICATORS").ToString())
        End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim sql As String = ""
        For Each row In Me.grd_indicadores.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("ID_ACTIVITY_AW_INDICATORS")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_TotalIndicador"), RadNumericTextBox)
                sql = "UPDATE TA_ACTIVITY_AW_INDICATORS SET meta_total =" & TotalIndicador.Value
                sql &= " WHERE ID_ACTIVITY_AW_INDICATORS= " & IDInstrumentoID.ToString

                Using dbEntities As New dbRMS_JIEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
        Next

        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityInd?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click

        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.Response.Redirect("~/RFP_/frm_ActivityAW?id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString())
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_Proyectos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub

    Protected Sub grd_indicadores_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_indicadores.PageIndexChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_indicadores_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_indicadores.PageSizeChanged
        fillGrid(False)
    End Sub

    Private Sub cmb_awards_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_awards.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                'Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                'Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = e.Value).FirstOrDefault()
                loadAWARD(e.Value)

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
        e.Item.Value = DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_ACTIVITY").ToString


    End Sub

End Class