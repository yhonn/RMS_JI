Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class frm_ProyectosAportes
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_PROY_APOR"
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
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
                Me.curr_International.Value = "USD"

                Dim oAportes = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id)


                'Dim oProgramaTasa = dbEntities.t_programas.Find(idPrograma).tasa_cambio
                If proyecto.tasa_cambio_actividad IsNot Nothing Then

                    Me.tasaCambio.Value = proyecto.tasa_cambio_actividad
                    'Me.lbl_tasa_cambio.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", Convert.ToDecimal(proyecto.tasa_cambio))
                    Me.txt_tasa_cambio.Value = proyecto.tasa_cambio_actividad

                End If

                If oAportes.Count > 0 Then
                    Me.txt_tasa_cambio.Value = oAportes.FirstOrDefault.tasa_cambio 'Set the default ExR
                End If

                'loadListas(idPrograma, proyecto)
                'LoadData(id)

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosEdit?Id=" & id.ToString()))
                Me.link_ruta.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyecto_ruta_aprobacion?Id=" & id.ToString()))
                'Me.alink_regionbeneficiada.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosRegion?Id=" & id.ToString()))
                'Me.alink_value_chain.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosValueChain?Id=" & id.ToString()))
                'Me.alink_upm.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosUpm?Id=" & id.ToString()))
                'Me.alink_indicadores.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosIndicadores?Id=" & id.ToString()))
                'Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosDocumentos?Id=" & id.ToString()))
                'Me.alink_entregables.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosEntregables?Id=" & id.ToString()))
                'Me.alink_predios.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosPredios?Id=" & id.ToString()))
                'Me.alink_areas.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_actividad_areas?Id=" & id.ToString()))
                'Me.alink_waiver.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosWaiver?Id=" & id.ToString()))



                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_ficha_proyecto)
                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

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


                Dim aportesFicha = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text).ToList()
                Me.grd_aportes.DataSource = aportesFicha
                Me.grd_aportes.DataBind()

                'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)
                sumaAportes()
                Dim actualesAp = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text) _
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


                setGraphFunding()

            End Using
        End If

    End Sub


    Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_eliminarAportes_Click(sender As Object, e As EventArgs) Handles btn_eliminarAportes.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_AportesFicha WHERE id_aporteFicha = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Me.grd_aportes.DataSource = ""
            Me.grd_aportes.DataSource = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id).ToList()
            Me.grd_aportes.DataBind()
            Dim actualesAp = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id) _
                               .Select(Function(p) p.id_aporte.Value).ToList()

            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) p.id_AporteOrigen = Me.cmb_aporte_origen.SelectedValue And (Not actualesAp.Contains(p.id_aporte))).ToList()
            Me.cmb_fuente_aporte.DataBind()
            guardarAportes()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Sub sumaAportes()
        valorSuma = 0
        For Each row In Me.grd_aportes.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_aporteFicha")
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
            If Me.txt_tasa_cambio.Value > 0 Then
                Me.lbl_totalUSD.Text = String.Format("$ {0:#,###,###.##}", (valorSuma / Me.txt_tasa_cambio.Value))
            Else
                Me.lbl_totalUSD.Text = String.Format("$ {0:#,###,###.##}", (valorSuma / Me.tasaCambio.Value))
            End If
        End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click

        guardarAportes()
        Using dbEntities As New dbRMS_JIEntities
            Dim idFicha = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim oFicha = dbEntities.tme_Ficha_Proyecto.Find(idFicha)
            oFicha.tasa_cambio = Me.txt_tasa_cambio.Value
            dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

        End Using



        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosAportes?id=" & Me.lbl_id_ficha.Text
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
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_aporteFicha")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
                sql = "UPDATE tme_AportesFicha SET monto_aporte=" & TotalIndicador.Value & ", monto_aporte_obligado=" & TotalIndicador.Value & ", tasa_cambio= " & Me.txt_tasa_cambio.Value
                sql &= " WHERE id_aporteFicha = " & IDInstrumentoID.ToString
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
            Dim AportesSRC = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id_ficha) _
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
            Dim oAporte = New tme_AportesFicha

            oAporte.id_ficha_proyecto = Me.lbl_id_ficha.Text
            oAporte.monto_aporte = 0
            oAporte.tasa_Cambio = Me.txt_tasa_cambio.Value
            oAporte.id_aporte = Me.cmb_fuente_aporte.SelectedValue
            oAporte.monto_aporte_obligado = 0

            dbEntities.tme_AportesFicha.Add(oAporte)
            dbEntities.SaveChanges()

            Dim aportesFicha = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text).ToList()
            Me.grd_aportes.DataSource = aportesFicha

            valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)

            sumaAportes()

            Dim actualesAp = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text) _
                               .Select(Function(p) p.id_aporte.Value).ToList()

            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) Not actualesAp.Contains(p.id_aporte)).ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()

        End Using

        Me.grd_aportes.DataBind()
    End Sub

    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
            Dim TotalIndicadorUSD As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_aporte_usd"), RadNumericTextBox)
            TotalIndicador.Text = DataBinder.Eval(e.Item.DataItem, "monto_aporte").ToString()
            TotalIndicadorUSD.Text = DataBinder.Eval(e.Item.DataItem, "TotalUSD").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_aporteFicha").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_aporteFicha").ToString())
        End If
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Proyectos/frm_Proyectos"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub cmb_aporte_origen_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_aporte_origen.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim actualesAp = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text) _
                               .Select(Function(p) p.id_aporte.Value).ToList()
            Me.cmb_fuente_aporte.DataSource = ""
            Dim eee = Me.cmb_fuente_aporte.SelectedValue
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) p.id_AporteOrigen = Me.cmb_aporte_origen.SelectedValue And (Not actualesAp.Contains(p.id_aporte))).ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()
        End Using
    End Sub
End Class