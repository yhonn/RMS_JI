Imports System.Data.SqlClient
Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_proyectoHitos
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACT_ENTR"
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
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_RFA + ")" + " " + proyecto.nombre_proyecto
                Dim aport = dbEntities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = id And p.id_AporteOrigen = 7)
                Dim aportes As Double = 0
                If aport.Count() > 0 Then
                    aportes = aport.Sum(Function(p) p.monto_aporte)
                End If
                'Me.lbl_monto_aportes.Text = aportes.ToString("N2", cl_user.regionalizacionCulture)
                'Me.monto_proyecto.Value = aportes


                'Me.dt_fecha.MinDate = proyecto.fecha_inicio_proyecto
                'Me.dt_fecha.MaxDate = proyecto.fecha_fin_proyecto
                'loadListas(idPrograma, proyecto)

                'LoadData(id)


                Dim fichaHitos = dbEntities.tme_ficha_hitos.Where(Function(p) p.id_ficha_proyecto = id).Select(Function(p) p.id_hito).ToArray()
                Dim hitos = dbEntities.tme_hitos.Where(Function(p) fichaHitos.Contains(p.id_hito)).ToList()

                Me.SqlDts_comentarios.SelectCommand = "select * from vw_ficha_proyecto_hitos where id_ficha_proyecto = " & Me.lbl_id_ficha.Text & " order by nro_hito"
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
            Dim idHito = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Me.identity.Text = idHito
            Dim oHito = dbEntities.tme_ficha_proyecto_hitos.Find(idHito)

            Me.txt_descripcion_entregable.Text = oHito.descripcion_hito
            Me.dt_fecha.SelectedDate = oHito.fecha_Esperada_entrega
            Me.txt_valor_hitho.Value = oHito.valor
            Me.txt_numero_entregable.Value = oHito.nro_hito

        End Using
    End Sub

    Protected Sub btn_eliminarAportes_Click(sender As Object, e As EventArgs) Handles btn_eliminarAportes.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                'dbEntities.Database.ExecuteSqlCommand("delete from tme_ruta_entregable where id_ficha_entregable = (select id_ficha_entregable from tme_ficha_entregables where id_ficha_producto = " & Me.identity.Text & ")")
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_hitos_entregables WHERE id_hito = " & Me.identity.Text)
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_ruta_aprobacion_hito WHERE id_hito = " & Me.identity.Text)
                dbEntities.Database.ExecuteSqlCommand("delete from tme_ficha_proyecto_hitos where id_hito = " & Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectoHitos?id=" & Me.lbl_id_ficha.Text
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub
    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        'guardarAportes()
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        'Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosEntregables?id=" & Me.lbl_id_ficha.Text
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectos"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Protected Sub btn_guardarEntregable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarEntregable.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim oHito = New tme_ficha_proyecto_hitos
            Dim idHito = Convert.ToInt32(Me.identity.Text)
            If (idHito <> 0) Then
                oHito = dbEntities.tme_ficha_proyecto_hitos.Find(idHito)
            Else
                oHito.id_ficha_proyecto = Me.lbl_id_ficha.Text
                oHito.fecha_crea = Date.Now
                oHito.id_estado_aprobacion_hito = 1
                oHito.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oHito.aprobado = False
            End If


            oHito.descripcion_hito = Me.txt_descripcion_entregable.Text
            oHito.fecha_Esperada_entrega = Me.dt_fecha.SelectedDate
            oHito.valor = Me.txt_valor_hitho.Value
            oHito.nro_hito = Me.txt_numero_entregable.Value

            If (idHito <> 0) Then
                dbEntities.Entry(oHito).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            Else
                dbEntities.tme_ficha_proyecto_hitos.Add(oHito)
                dbEntities.SaveChanges()
                'Dim oRutaHito = New tme_ruta_aprobacion_hito
                'oRutaHito.id_estado_ruta_aprobacion_hito = 1
                'oRutaHito.fecha_crea = Date.Now
                'oRutaHito.id_hito = oHito.id_hito
                'oRutaHito.id_responsable_aprueba_hito = 1
                'dbEntities.tme_ruta_aprobacion_hito.Add(oRutaHito)
                'dbEntities.SaveChanges()
            End If







            Me.SqlDts_comentarios.SelectCommand = "select * from vw_ficha_proyecto_hitos where id_ficha_proyecto = " & Me.lbl_id_ficha.Text & " order by nro_hito"
            Me.grd_aportes.DataBind()

            Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), 10, cl_user.regionalizacionCulture, cl_user.idSys)
            'If (objEmail.Emailing_PRODUCTS_PENDING(oProducto.id_ficha_producto)) Then
            'Else 'Error mandando Email
            'End If

            'valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)

            'sumaAportes()

        End Using
        'guardarAportes()
        'Me.grd_aportes.DataBind()
        Me.txt_descripcion_entregable.Text = ""
        Me.dt_fecha.Clear()
        'Me.txt_total_aporte.Value = 0
        'Me.txt_porcentaje.Value = 0
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectoHitos?id=" & Me.lbl_id_ficha.Text
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
            Dim EstadoEntregable = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estado_aprobacion_hito").ToString())
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)

            Dim hlnkEdit As LinkButton = New LinkButton
            hlnkEdit = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
            If EstadoEntregable = 1 Then
                hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())
                hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())

                hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())
                hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())
            Else
                hlnkDelete.Visible = False
                hlnkEdit.Visible = False
            End If

            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "~/Proyectos/frm_hitosEntregables?id=" & DataBinder.Eval(e.Item.DataItem, "id_hito").ToString()

        End If
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Proyectos/frm_proyectos")
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_proyectosDocumentos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
End Class