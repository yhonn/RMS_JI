Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Globalization
Public Class frm_radicados
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_RADICADO"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim idUser As Integer = 0


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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
        Dim sManager = ScriptManager.GetCurrent(Me)
        sManager.RegisterPostBackControl(Me.btn_generar_reporte)
        If Not Me.IsPostBack Then
            'LoadLista()
            Dim newCulture As CultureInfo = New CultureInfo("es-ES")
            Me.grd_cate.Culture = New System.Globalization.CultureInfo("fr-FR", True)

            Dim Menu As GridFilterMenu = grd_cate.FilterMenu
            For Each item In Menu.Items
                'change the text for the StartsWith menu item
                If item.Text = "NoFilter" Then
                    item.Text = "Limpiar filtro"
                ElseIf item.Text = "Contains" Then
                    item.Text = "Contiene"
                ElseIf item.Text = "DoesNotContain" Then
                    item.Text = "No contiene"
                ElseIf item.Text = "StartsWith" Then
                    item.Text = "Empieza con"
                ElseIf item.Text = "EndsWith" Then
                    item.Text = "Finaliza con"
                ElseIf item.Text = "EqualTo" Then
                    item.Text = "Igual a"
                ElseIf item.Text = "NotEqualTo" Then
                    item.Text = "No igual a"
                ElseIf item.Text = "GreaterThan" Then
                    item.Text = "Mayor a"
                ElseIf item.Text = "LessThan" Then
                    item.Text = "Menor a"
                ElseIf item.Text = "GreaterThanOrEqualTo" Then
                    item.Text = "Mayor o igual a"
                ElseIf item.Text = "LessThanOrEqualTo" Then
                    item.Text = "Menor o igual a"
                ElseIf item.Text = "Between" Then
                    item.Text = "Entre"
                ElseIf item.Text = "NotBetween" Then
                    item.Text = "NoEntre"
                ElseIf item.Text = "IsEmpty" Then
                    item.Text = "Es vacío"
                ElseIf item.Text = "NotIsEmpty" Then
                    item.Text = "No es vacío"
                ElseIf item.Text = "IsNull" Then
                    item.Text = "Es nulo"
                ElseIf item.Text = "NotIsNull" Then
                    item.Text = "No es nulo"
                End If
            Next
            fillGrid(True)
            validarRegistroRadicado()
        End If
    End Sub
    Sub validarRegistroRadicado()
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
        Using dbentities As New dbRMS_JIEntities
            Dim permisos = dbentities.tme_radicados_permisos.Where(Function(p) p.id_programa = idPrograma).FirstOrDefault()
            Dim usuario = dbentities.t_usuarios_permisos.Where(Function(p) p.id_programa = idPrograma And p.id_usuario = id_usuario).FirstOrDefault()
            Dim cierre = dbentities.tme_fecha_cierre.OrderByDescending(Function(p) p.fecha_cierre).ToList().FirstOrDefault()
            Dim fechaRegistro = DateTime.Now
            Dim fechaHabil = New Date(fechaRegistro.Year, fechaRegistro.Month, fechaRegistro.Day, 0, 0, 0)

            Dim habilitarAdd As Boolean = False
            Dim habilitarAdd2 As Boolean = False

            If fechaHabil > cierre.fecha_cierre And fechaHabil.Year = cierre.fecha_cierre.Year And fechaHabil.Month = cierre.fecha_cierre.Month Then
                habilitarAdd = False
                If permisos IsNot Nothing Then
                    If permisos.habilitar_facturacion_post_cierre = True Then
                        habilitarAdd = True
                    End If
                End If
                If usuario IsNot Nothing Then
                    If usuario.registro_radicados_post_cierre = True And fechaHabil <= permisos.fecha_cierre_final Then
                        habilitarAdd2 = True
                    End If
                End If
            Else
                habilitarAdd = True
            End If

            Me.btn_nuevo.Visible = If(habilitarAdd Or habilitarAdd2, True, False)

        End Using
    End Sub
    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            'Dim facturas = dbEntities.vw_tme_facturacion.ToList()

            'Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            'Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            'Dim facturacion As List(Of vw_tme_facturacion) = New List(Of vw_tme_facturacion)

            'Select Case id_filtro_busqueda_viajes
            '    Case 1
            '        facturacion = dbEntities.vw_tme_facturacion.OrderByDescending(Function(p) p.id_facturacion).ToList()
            '    Case 2
            '        facturacion = dbEntities.vw_tme_facturacion.Where(Function(p) subR.Contains(p.id_sub_region)).OrderByDescending(Function(p) p.id_viaje).ToList()
            '    Case 3
            '        facturacion = dbEntities.vw_tme_facturacion.Where(Function(p) (p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
            '                                                                             (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
            '                                                                             (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7) Or
            '                                                                             (p.id_usuario_radica.Contains(id_User)) Or p.id_usuario = id_User Or p.id_supervisor = id_User).OrderByDescending(Function(p) p.id_viaje).ToList()
            'End Select



            Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            'Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim solicitudesFact As List(Of vw_tme_radicados) = New List(Of vw_tme_radicados)



            Dim listarTodas = Convert.ToString(Me.h_Filter.Value)

            If listarTodas = "" Then
                Select Case id_filtro_busqueda_viajes
                    Case 1
                        solicitudesFact = dbEntities.vw_tme_radicados.Where(Function(p) p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_radicado).ToList()
                    Case 2
                        solicitudesFact = dbEntities.vw_tme_radicados.Where(Function(p) subR.Contains(p.id_subregion) Or p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_radicado).ToList()
                    Case 3
                        solicitudesFact = dbEntities.vw_tme_radicados.Where(Function(p) p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_radicado).ToList()

                End Select
            Else
                solicitudesFact = dbEntities.vw_tme_radicados.OrderByDescending(Function(p) p.id_radicado).ToList()
            End If


            Me.grd_cate.DataSource = solicitudesFact

            If bndBind Then
                Me.grd_cate.DataBind()
            End If


        End Using
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim habilitarFechaRadicacion = Convert.ToString(Me.h_Filter_fecha.Value)


            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim idEstado = Convert.ToInt32(itemD("id_estado").Text.ToString())
            Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim idUsuarioCrea = Convert.ToInt32(itemD("id_usuario_crea").Text.ToString())
            Dim idUsuarioAprobo = Convert.ToInt32(itemD("id_usuario_aprobo").Text.ToString())
            Dim hlnkFacturacion As HyperLink = New HyperLink
            hlnkFacturacion = CType(e.Item.FindControl("col_hlk_estado"), HyperLink)

            Dim anulado = Convert.ToBoolean(itemD("anulado").Text.ToString())

            Dim col_img_estado As Image = New Image
            col_img_estado = CType(e.Item.FindControl("col_img_estado"), Image)

            Dim hlnkAnular As HyperLink = New HyperLink
            hlnkAnular = CType(e.Item.FindControl("col_hlk_anular"), HyperLink)

            hlnkAnular.Visible = False

            Dim registroPago = itemD("radicado_pago_contabilizacion").Text.ToString()

            Dim hlnkRechazar As HyperLink = New HyperLink
            hlnkRechazar = CType(e.Item.FindControl("col_hlk_reversar"), HyperLink)

            hlnkRechazar.Visible = False

            Dim hlnkEdit As LinkButton = New LinkButton
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), LinkButton)
            hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString())
            hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString())
            hlnkEdit.Visible = False

            Dim hlnkActualizar As LinkButton = New LinkButton
            hlnkActualizar = CType(e.Item.FindControl("col_hlk_actualizar"), LinkButton)
            hlnkActualizar.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString())
            hlnkActualizar.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString())
            hlnkActualizar.Attributes.Add("data-code", DataBinder.Eval(e.Item.DataItem, "codigo_GJ").ToString())
            hlnkActualizar.Visible = False

            Dim img_editar As Image = New Image
            img_editar = CType(e.Item.FindControl("img_editar"), Image)
            If registroPago = "NO" And idEstado = 2 And anulado = False Then
                hlnkRechazar.Visible = True
                hlnkRechazar.Visible = True
                hlnkRechazar.NavigateUrl = "frm_radicadosEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString()
                hlnkEdit.Visible = True
            ElseIf registroPago = "SI" Then
                hlnkEdit.Visible = True
                img_editar.ImageUrl = "~/Imagenes/iconos/01_SI.png"
                img_editar.Visible = True

                If habilitarFechaRadicacion = "" Then
                    hlnkEdit.Enabled = False
                    hlnkEdit.Attributes.Add("disabled", "true")
                End If
            End If


            If idEstado = 1 And anulado = False Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.NavigateUrl = "frm_radicadosEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString()
                hlnkFacturacion.ToolTip = "Pendiente"
                If idUsuarioCrea = idUsuario Then
                    hlnkAnular.Visible = True
                    hlnkAnular.NavigateUrl = "frm_radicadosEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString() & "&a=1"
                End If
            ElseIf idEstado = 1 And anulado = True Then
                hlnkAnular.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                hlnkAnular.ToolTip = "Anulado"
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                hlnkFacturacion.ToolTip = "Anulado"
                hlnkAnular.Visible = True
                'hlnkFacturacion.Visible = False
                col_img_estado.Visible = False
            ElseIf idEstado = 2 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/01_SI.png"
                hlnkFacturacion.ToolTip = "Recibido"

                col_img_estado.ImageUrl = "~/Imagenes/iconos/01_SI.png"
                col_img_estado.ToolTip = "Recibido"
            ElseIf idEstado = 3 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/01_NO.png"
                hlnkFacturacion.ToolTip = "Rechazado"

                col_img_estado.ImageUrl = "~/Imagenes/iconos/01_NO.png"
                col_img_estado.ToolTip = "Rechazado"
            End If

            If idEstado = 2 And idUsuarioAprobo = idUsuario Then
                hlnkActualizar.Visible = True
            End If

            'If habilitarFechaRadicacion <> "" Then

            'End If

            'Dim hlnkDetalle As HyperLink = New HyperLink
            'hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            'hlnkDetalle.NavigateUrl = "frm_radicadosDetalle?Id=" & DataBinder.Eval(e.Item.DataItem, "id_radicado").ToString()

            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

        End If
    End Sub
    Protected Sub Editar_fp_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadEditModal()", True)
    End Sub

    Protected Sub Editar_gj_click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        Dim codigo = a.Attributes.Item("data-code").ToString()
        Me.txt_codigo_gj.Text = codigo
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadGJModal()", True)
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/financiero/frm_radicadosAD")
    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid(False)
    End Sub

    Private Sub btn_fecha_pago_Click(sender As Object, e As EventArgs) Handles btn_fecha_pago.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim idRadicado = Convert.ToInt32(Me.identity.Value)
                Dim radicado = dbEntities.tme_radicados.Find(idRadicado)
                radicado.fecha_pago_contabilizacion = Me.dt_fecha_pago.SelectedDate
                dbEntities.Entry(radicado).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/financiero/frm_radicados"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_codigo_gj_Click(sender As Object, e As EventArgs) Handles btn_codigo_gj.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim idRadicado = Convert.ToInt32(Me.identity.Value)
                Dim radicado = dbEntities.tme_radicados.Find(idRadicado)
                radicado.codigo_GJ = Me.txt_codigo_gj.Text
                dbEntities.Entry(radicado).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/financiero/frm_radicados"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_generar_reporte_Click(sender As Object, e As EventArgs) Handles btn_generar_reporte.Click
        Try
            Dim fechaInicio = Me.dt_fecha_desde.SelectedDate
            Dim fechaFin = Me.dt_fecha_hasta.SelectedDate
            Dim url = "~/reportes/xls?id=5&desde=" & fechaInicio & "&hasta=" & fechaFin
            Me.Response.Redirect(url)
        Catch ex As Exception

        End Try

        'Response.Write("<script>window.open('~/reportes/xls?id=5','_blank')</script>")
    End Sub
End Class