Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class frm_hito_entregablePrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_STO_HITOS"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListados As New ly_SIME.CORE.cls_listados

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

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities
                Dim idHito = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Dim producto = dbEntities.tme_ficha_proyecto_hitos.FirstOrDefault(Function(p) p.id_hito = idHito)
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = producto.id_ficha_proyecto)
                Me.lbl_codigo.Text = proyecto.codigo_RFA
                Me.lbl_valor_contrato.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", proyecto.c_Aportes_Proyecto)
                Me.lbl_contratista.Text = proyecto.nombre_ejecutor
                Me.lbl_supervisor.Text = proyecto.supervisor
                Me.lbl_fecha_inicio.Text = proyecto.fecha_inicio_proyecto.Value.Year & "-" & proyecto.fecha_inicio_proyecto.Value.Month & "-" & proyecto.fecha_inicio_proyecto.Value.Day
                Me.lbl_fecha_fin.Text = proyecto.fecha_fin_proyecto.Value.Year & "-" & proyecto.fecha_fin_proyecto.Value.Month & "-" & proyecto.fecha_fin_proyecto.Value.Day
                Me.lbl_numero_entregable.Text = producto.nro_hito
                Me.lbl_valor_entregable.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", producto.valor)
                Me.lbl_fecha_esperada_entrega.Text = producto.fecha_Esperada_entrega.Value.Year & "-" & producto.fecha_Esperada_entrega.Value.Month & "-" & producto.fecha_Esperada_entrega.Value.Day
                Me.lbl_fecha_entrega.Text = producto.fecha_entrega.Value.Year & "-" & producto.fecha_entrega.Value.Month & "-" & producto.fecha_entrega.Value.Day
                Me.lbl_regional.Text = proyecto.nombre_subregion
                Me.lbl_objeto.Text = proyecto.nombre_proyecto

                Me.lbl_producto.Text = producto.descripcion_hito

                Me.grd_productos.DataSource = producto.tme_hitos_entregables
                Me.grd_productos.DataBind()
                'Me.SqlDts_comentarios.SelectCommand = "select * from vw_ruta_aprobacion_hito_history where id_hito = " & idHito & " order by fecha_envio "
                Dim history = dbEntities.vw_ruta_aprobacion_hito_history.AsNoTracking.Where(Function(p) p.id_hito = idHito).OrderBy(Function(p) p.fecha_envio).ToList()
                Me.grd_ruta.DataSource = history
                Me.grd_ruta.DataBind()
                'fillGrid(1)
                'Me.SqlDts_comentarios.SelectCommand = "select * from vw_ruta_aprobacion_hito_history where id_hito = " & idHito & " order by fecha_envio "
                'Me.grd_ruta.DataBind()

                'Me.SqlDts_comentarios2.SelectCommand = "select id_hito, id_ruta_hito, isnull(Convert(varchar(40),fecha_envio),'--') as fecha_envio_text, usuario_aprueba, 
                '    comentarios,  soporte from vw_comentario_hitos where id_hito = " & idHito & "  and comentarios is not null order by fecha_envio asc"
                'Me.grd_coment.DataBind()
                'LoadData(idHito)
            End Using
        End If
    End Sub

    Protected Sub grd_productos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_productos.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "~/Proyectos/frm_entregables_Act?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            'Dim hlnkActivar As HyperLink = New HyperLink
            'hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            'hlnkActivar.NavigateUrl = "~/Proyectos/frm_productos?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            Dim hlnktrn As HyperLink = New HyperLink
            hlnktrn = CType(e.Item.FindControl("hlk_view"), HyperLink)
            Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte")
            If soporte IsNot Nothing Then
                If soporte.ToString().Length > 4 Then
                    Dim file_name = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()
                    hlnktrn.ToolTip = controles.iconosGrid("col_hlk_view")
                    Dim adjunto = controles.iconosGrid("col_hlk_view")
                    hlnktrn.NavigateUrl = file_name
                    If file_name.Length < 4 Then
                        hlnktrn.Visible = False
                    End If
                Else
                    hlnktrn.Visible = False
                End If
            Else
                hlnktrn.Visible = False
            End If
        End If
    End Sub


    'Sub fillGrid(ByVal caso As Integer)
    '    Dim sqlFiltros = " "

    '    Select Case caso
    '        Case 3
    '    End Select
    '    Me.SqlDts_entregables.SelectCommand = "select * from vw_tme_ficha_proyecto_hitos_entregables where id_hito = " & Me.id_hito.Value & sqlFiltros & " order by nro_entregable"
    '    Me.grd_aportes.DataBind()
    'End Sub

    'Sub LoadData(ByVal idHito As Integer)
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim hito = dbEntities.tme_ficha_proyecto_hitos.Where(Function(p) p.id_hito = idHito).FirstOrDefault()
    '        Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto)
    '        Me.lbl_id_ficha.Text = proyecto.id_ficha_proyecto
    '        Me.lbl_cod_acti.Text = proyecto.codigo_RFA
    '        Me.lbl_actividad.Text = proyecto.nombre_proyecto
    '        Me.lbl_fecha_inicio.Text = proyecto.fecha_inicio_proyecto
    '        Me.lbl_fecha_fin.Text = proyecto.fecha_fin_proyecto
    '        Me.lbl_ejecutor.Text = proyecto.nombre_ejecutor
    '        Me.lbl_nom_producto.Text = hito.descripcion_hito
    '        Me.lbl_num_producto.Text = hito.nro_hito
    '        Me.lbl_fecha_entrega.Text = hito.fecha_Esperada_entrega
    '        If hito.url IsNot Nothing Then
    '            Me.doc_admon_content.Visible = True
    '        End If
    '        Me.docs_admon.NavigateUrl = hito.url

    '        Dim comentarios = dbEntities.vw_ruta_aprobacion_hito_history.Where(Function(p) p.id_hito = idHito).ToList()
    '        Dim idUserLogin = "," & Me.Session("E_IdUser").ToString() & ","
    '        Dim aprobacionesPendientes = comentarios.Where(Function(p) p.fecha_envio = "Pendiente")
    '        Dim lastComent = comentarios.OrderByDescending(Function(p) p.id_ruta_hito).Take(1).ToList()

    '        If lastComent.Count() > 0 Then
    '            If lastComent.FirstOrDefault().id_responsable_aprueba_hito <> 2 And lastComent.FirstOrDefault().id_responsable_aprueba_hito <> 1 Then
    '                lastComent = comentarios.OrderByDescending(Function(p) p.id_ruta_hito).Take(2).ToList()
    '            End If
    '        End If
    '        'Me.addComentario.Visible = False
    '        If hito.aprobado <> True Then
    '            If aprobacionesPendientes.Count() > 0 Then
    '                Dim comentUserApr = aprobacionesPendientes.Where(Function(p) p.id_usuario_aprueba.Contains(idUserLogin)).ToList()
    '                If comentUserApr.Count() > 0 Then
    '                    Dim respons = comentUserApr.FirstOrDefault()
    '                    'Me.addComentario.Visible = True
    '                    Me.id_ruta_hito.Value = respons.id_ruta_hito
    '                    If respons.id_responsable_aprueba_hito = 1 Then
    '                        Me.btn_ajustes.Visible = False
    '                        'Me.soporteURL.Visible = True
    '                        Me.rv_docs_admin.Enabled = True
    '                        For Each row In Me.grd_aportes.Items
    '                            If TypeOf row Is GridDataItem Then
    '                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '                                Dim modSoporte As HyperLink = CType(row.Cells(0).FindControl("hlk_productos"), HyperLink)
    '                                modSoporte.Visible = True
    '                            End If
    '                        Next
    '                        If hito.tme_hitos_entregables.Count() <> hito.tme_hitos_entregables.Where(Function(p) p.soporte IsNot Nothing).Count() Then
    '                            Me.addComentario.Visible = False
    '                        End If
    '                        'Me.accioEncar.Visible = False
    '                    Else
    '                        Me.rv_docs_admin.Enabled = False
    '                        If hito.id_estado_aprobacion_hito <> 6 Then
    '                            'Me.accioEncar.Visible = True
    '                            'Me.accioEjecu.Visible = False
    '                        End If
    '                    End If
    '                Else
    '                    Me.addComentario.Visible = False
    '                End If
    '                'If hito.id_estado_aprobacion_hito = 2 And Me.accioEjecu.Visible = False And Me.accioEncar.Visible = False Then
    '                '    Me.comentariosAdiconales.Visible = True
    '                'End If
    '            Else
    '                'Me.addComentario.Visible = False
    '            End If
    '        Else
    '            Me.addComentario.Visible = False
    '        End If

    '        'If addComentario.Visible = False Then
    '        '    If hito.id_estado_aprobacion_hito = 3 Then
    '        '        'Me.comentariosAdiconales.Visible = True
    '        '    End If
    '        'End If


    '    End Using
    'End Sub
End Class