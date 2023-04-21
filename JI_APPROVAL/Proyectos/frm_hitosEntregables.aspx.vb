Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_hitosEntregables
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACT_ENTR_PRO"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
    Dim sumaPorcentaje As Decimal = 0
    Dim sumaAportesTotal As Decimal = 0
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
                Me.id_producto.Value = id
                Dim hito = dbEntities.tme_ficha_proyecto_hitos.Find(id)
                Me.lbl_id_ficha.Text = hito.id_ficha_proyecto
                Me.lbl_fecha_entrega.Text = hito.fecha_Esperada_entrega
                Me.lbl_nom_pro.Text = hito.descripcion_hito
                Me.lbl_nro_pro.Text = hito.nro_hito
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_RFA + ")" + " " + proyecto.nombre_proyecto
                Dim aport = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id And p.id_AporteOrigen = 7)
                Dim aportes As Double = 0
                If aport.Count() > 0 Then
                    aportes = aport.Sum(Function(p) p.monto_aporte)
                End If
                'Me.lbl_monto_aportes.Text = aportes.ToString("N2", cl_user.regionalizacionCulture)
                'Me.monto_proyecto.Value = aportes
                'loadListas(idPrograma, proyecto)

                'LoadData(id)


                Dim fichaHitos = dbEntities.tme_ficha_hitos.Where(Function(p) p.id_ficha_proyecto = id).Select(Function(p) p.id_hito).ToArray()
                Dim hitos = dbEntities.tme_hitos.Where(Function(p) fichaHitos.Contains(p.id_hito)).ToList()


                'Dim aportesFicha = dbEntities.tme_ficha_entregables.Where(Function(p) p.id_ficha_proyecto = Me.lbl_id_ficha.Text) _
                '                   .OrderBy(Function(p) p.numero_entregable).ToList()
                'Me.grd_aportes.DataSource = aportesFicha

                'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)

                'sumaAportes()
                'Me.grd_aportes.DataBind()

                'If aportes = 0 Then
                '    Me.btn_guardarEntregable.Enabled = False
                'End If
                Me.SqlDts_comentarios.SelectCommand = "select * from vw_tme_ficha_proyecto_hitos_entregables where id_hito = " & Me.id_producto.Value & " order by nro_entregable"
                Me.grd_aportes.DataBind()
            End Using
        End If

    End Sub


    Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub Editar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Using dbEntities As New dbRMS_JIEntities
            Dim idEntregable = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Dim oEntregable = dbEntities.tme_hitos_entregables.Find(idEntregable)
            Me.txt_descripcion_entregable.Text = oEntregable.descripcion_entregable
            Me.txt_numero_entregable.Value = oEntregable.nro_entregable
            Me.txt_cantidad.Value = oEntregable.cantidad
            Me.txt_unidad_medida.Text = oEntregable.unidad_medidad
            Me.identity.Text = idEntregable
        End Using
    End Sub

    Protected Sub btn_eliminarAportes_Click(sender As Object, e As EventArgs) Handles btn_eliminarAportes.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_hitos_entregables WHERE id_entregable_hito = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try

            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_HitosEntregables?id=" & Me.id_producto.Value
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    'Sub sumaAportes()
    '    For Each row In Me.grd_aportes.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_ficha_entregable")
    '            Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
    '            valorSuma += TotalIndicador.Value
    '        End If
    '    Next
    '    'If valorSuma = 0 Then
    '    '    Me.lbl_total.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
    '    '    Me.lbl_totalUSD.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
    '    'Else
    '    '    Me.lbl_total.Text = valorSuma.ToString("c2", cl_user.regionalizacionCulture)
    '    '    Me.lbl_totalUSD.Text = (valorSuma / 1000).ToString("c2", cl_user.regionalizacionCulture)
    '    'End If
    'End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        'guardarAportes()
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        'Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosEntregables?id=" & Me.lbl_id_ficha.Text
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectos"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub txt_meta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'sumaAportes()
    End Sub


    'Sub guardarAportes()
    '    valorSuma = 0
    '    Dim sql As String = ""
    '    Using dbEntities As New dbRMS_JIEntities
    '        For Each row In Me.grd_aportes.Items
    '            If TypeOf row Is GridDataItem Then
    '                Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_ficha_entregable")
    '                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
    '                Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
    '                Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
    '                Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)
    '                Dim numero As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_numero_ent"), RadNumericTextBox)

    '                Dim oEntregable = dbEntities.tme_ficha_entregables.Find(IDInstrumentoID)

    '                oEntregable.descripcion_entregable = descripcion_entregable.Text
    '                oEntregable.fecha = fecha.SelectedDate
    '                oEntregable.fecha_inicio = Me.dt_fecha_inicio.SelectedDate
    '                oEntregable.valor = TotalIndicador.Value
    '                oEntregable.porcentaje = porcentaje.Value
    '                oEntregable.numero_entregable = numero.Value

    '                dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
    '            End If
    '        Next
    '        dbEntities.SaveChanges()
    '    End Using
    'End Sub


    Protected Sub btn_guardarEntregable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarEntregable.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim oEntregable = New tme_hitos_entregables
            Dim idEntregable = Convert.ToInt32(Me.identity.Text)
            If (idEntregable <> 0) Then
                oEntregable = dbEntities.tme_hitos_entregables.Find(idEntregable)
            Else
                oEntregable.id_estado_entregable = 1
                oEntregable.fecha_crea = Date.Now
                oEntregable.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oEntregable.aprobado = False
                oEntregable.id_hito = Me.id_producto.Value
            End If
            oEntregable.descripcion_entregable = Me.txt_descripcion_entregable.Text
            oEntregable.nro_entregable = Me.txt_numero_entregable.Value
            oEntregable.cantidad = Me.txt_cantidad.Value
            oEntregable.unidad_medidad = Me.txt_unidad_medida.Text
            If (idEntregable <> 0) Then
                dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
            Else
                dbEntities.tme_hitos_entregables.Add(oEntregable)
            End If
            dbEntities.SaveChanges()

        End Using
        'guardarAportes()
        'Me.grd_aportes.DataBind()
        Me.txt_descripcion_entregable.Text = ""
        'Me.txt_total_aporte.Value = 0
        'Me.txt_porcentaje.Value = 0
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_HitosEntregables?id=" & Me.id_producto.Value
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            'Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
            'TotalIndicador.Value = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "valor"))
            'Dim descripcion_entregable As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
            'descripcion_entregable.Text = DataBinder.Eval(e.Item.DataItem, "descripcion_entregable").ToString()

            'Dim numero_entregable As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_numero_ent"), RadNumericTextBox)
            'numero_entregable.Value = DataBinder.Eval(e.Item.DataItem, "numero_entregable").ToString()
            'Dim fecha As RadDatePicker = CType(e.Item.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
            'fecha.SelectedDate = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "fecha"))
            'fecha.MinDate = Me.dt_fecha.MinDate
            'fecha.MaxDate = Me.dt_fecha.MaxDate
            'Dim porcentaje As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)
            'porcentaje.Value = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "porcentaje"))

            'Dim hlnkEmpleados As HyperLink = New HyperLink
            'hlnkEmpleados = CType(e.Item.FindControl("hlk_productos"), HyperLink)
            'hlnkEmpleados.NavigateUrl = "frm_productos?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_entregable").ToString()
            ''hlnkEmpleados.ToolTip = controles.iconosGrid("hlk_productos")
            Dim EstadoEntregable = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estado_entregable").ToString())
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            Dim hlnkEdit As LinkButton = New LinkButton
            hlnkEdit = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
            If EstadoEntregable = 1 Then
                hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_entregable_hito").ToString())
                hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_entregable_hito").ToString())

                hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_entregable_hito").ToString())
                hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_entregable_hito").ToString())
            Else
                hlnkDelete.Visible = False
                hlnkEdit.Visible = False
            End If

        End If
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Proyectos/frm_proyectos")
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_proyectosDocumentos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    'Protected Sub txt_porcentaje_TextChanged(sender As Object, e As EventArgs)
    '    Dim valor_total = Me.monto_proyecto.Value
    '    Me.txt_total_aporte.Value = valor_total * txt_porcentaje.Value / 100
    'End Sub

    'Protected Sub txt_col_aporte_TextChanged(sender As Object, e As EventArgs)
    '    validarValorTotal()
    'End Sub

    'Sub validarValorTotal()
    '    valorSuma = 0
    '    Dim sql As String = ""
    '    For Each row In Me.grd_aportes.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_ficha_entregable")
    '            Dim total_aporte As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_total_aporte"), RadNumericTextBox)
    '            Dim descripcion_entregable As RadTextBox = CType(row.Cells(0).FindControl("txt_col_descripcion_entregable"), RadTextBox)
    '            Dim fecha As RadDatePicker = CType(row.Cells(0).FindControl("dt_col_fecha"), RadDatePicker)
    '            Dim porcentaje As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_col_porcentaje"), RadNumericTextBox)

    '            Dim valor_total = Me.monto_proyecto.Value

    '            porcentaje.Value = total_aporte.Value * 100 / valor_total
    '            sumaAportesTotal += total_aporte.Value
    '            If sumaPorcentaje > 100 Then
    '                Me.div_mensaje.Visible = True
    '                Me.btn_guardar.Enabled = False
    '            Else
    '                Me.div_mensaje.Visible = False
    '                Me.btn_guardar.Enabled = True
    '            End If

    '        End If
    '    Next
    'End Sub

    'Protected Sub txt_porcentaje_TextChanged1(sender As Object, e As EventArgs) Handles txt_porcentaje.TextChanged

    '    sumaPorcentaje = txt_porcentaje.Value
    '    Dim sql As String = ""
    '    For Each row In Me.grd_aportes.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_ficha_entregable")
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

    'Protected Sub txt_total_aporte_TextChanged(sender As Object, e As EventArgs) Handles txt_total_aporte.TextChanged
    '    valorSuma = txt_total_aporte.Value
    '    Dim valor_total = Me.monto_proyecto.Value

    '    txt_porcentaje.Value = txt_total_aporte.Value * 100 / valor_total
    '    Dim sql As String = ""
    '    For Each row In Me.grd_aportes.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_ficha_entregable")
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
    'End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub
End Class