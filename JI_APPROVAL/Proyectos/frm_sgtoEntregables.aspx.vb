Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_APPROVAL

Public Class frm_sgtoEntregables
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_STO_HITOS"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim clss_approval As APPROVAL.clss_approval

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
                Dim idHito = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.id_hito.Value = idHito
                Dim producto = dbEntities.tme_ficha_proyecto_hitos.FirstOrDefault(Function(p) p.id_hito = idHito)
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = producto.id_ficha_proyecto)
                Me.lbl_id_ficha.Text = proyecto.id_ficha_proyecto
                Me.lbl_cod_acti.Text = proyecto.codigo_RFA
                Me.lbl_actividad.Text = proyecto.nombre_proyecto
                Me.lbl_fecha_inicio.Text = proyecto.fecha_inicio_proyecto
                Me.lbl_fecha_fin.Text = proyecto.fecha_fin_proyecto
                Me.lbl_ejecutor.Text = proyecto.nombre_ejecutor
                Me.lbl_nom_producto.Text = producto.descripcion_hito
                Me.lbl_num_producto.Text = producto.nro_hito
                Me.lbl_fecha_entrega.Text = producto.fecha_Esperada_entrega
                fillGrid(1)
                Me.SqlDts_comentarios.SelectCommand = "select * from vw_ruta_aprobacion_hito_history where id_hito = " & idHito & " order by fecha_envio "
                Me.grd_ruta.DataBind()

                Me.SqlDts_comentarios2.SelectCommand = "select id_hito, id_ruta_hito, isnull(Convert(varchar(40),fecha_envio),'--') as fecha_envio_text, usuario_aprueba, 
                    comentarios,  soporte from vw_comentario_hitos where id_hito = " & idHito & "  and comentarios is not null order by fecha_envio asc"
                Me.grd_coment.DataBind()
                LoadData(idHito)
            End Using
        End If
    End Sub

    Protected Sub btn_ajustes_Click(sender As Object, e As EventArgs) Handles btn_ajustes.Click
        ajustes(1)
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_hitosEj?id=" & Me.lbl_id_ficha.Text
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Sub ajustes(ByVal responsable As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim idRuta = Convert.ToInt32(Me.id_ruta_hito.Value)
            Dim ruta = dbEntities.tme_ruta_aprobacion_hito.Find(idRuta)
            ruta.comentarios = Me.txtcoments.Text
            ruta.id_usuario_ruta = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            ruta.fecha_envio = Date.Now
            ruta.id_estado_ruta_aprobacion_hito = 3
            Dim id_ejecutor = ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.id_ejecutor
            Dim emil_to_send = ""

            dbEntities.Entry(ruta).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

            Dim hito = ruta.tme_ficha_proyecto_hitos

            dbEntities.SaveChanges()


            Dim newRuta = New tme_ruta_aprobacion_hito
            newRuta.id_responsable_aprueba_hito = 1
            newRuta.id_estado_ruta_aprobacion_hito = 1
            newRuta.id_ruta_hito_solicito_ajuste = ruta.id_ruta_hito

            'If ruta.id_responsable_aprueba_hito = 2 Then
            '    newRuta.id_responsable_aprueba_hito = 1
            '    newRuta.id_estado_ruta_aprobacion_hito = 1
            'ElseIf ruta.id_responsable_aprueba_hito = 3 Or ruta.id_responsable_aprueba_hito = 4 Or ruta.id_responsable_aprueba_hito = 5 Or ruta.id_responsable_aprueba_hito = 6 Then
            '    newRuta.id_responsable_aprueba_hito = 2
            '    newRuta.id_estado_ruta_aprobacion_hito = 4
            'End If
            newRuta.id_hito = ruta.id_hito
            'newRuta.id_estado_ruta_aprobacion_hito = 1
            newRuta.fecha_crea = Date.Now
            dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
            dbEntities.SaveChanges()
            Dim id_notificacion = 1010
            Dim strEmail = "Ajuste de hitos subactividad " & ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.codigo_RFA

            Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), id_notificacion, cl_user.regionalizacionCulture, cl_user.idSys)
            'If (objEmail.Emailing_PRODUCTS_ACTIONS(ruta.id_hito, strEmail, emil_to_send, id_ejecutor, "")) Then
            'Else 'Error mandando Email
            'End If
        End Using
    End Sub
    Sub LoadData(ByVal idHito As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim hito = dbEntities.tme_ficha_proyecto_hitos.Where(Function(p) p.id_hito = idHito).FirstOrDefault()
            Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto)
            Me.lbl_id_ficha.Text = proyecto.id_ficha_proyecto
            Me.lbl_cod_acti.Text = proyecto.codigo_RFA
            Me.lbl_actividad.Text = proyecto.nombre_proyecto
            Me.lbl_fecha_inicio.Text = proyecto.fecha_inicio_proyecto
            Me.lbl_fecha_fin.Text = proyecto.fecha_fin_proyecto
            Me.lbl_ejecutor.Text = proyecto.nombre_ejecutor
            Me.lbl_nom_producto.Text = hito.descripcion_hito
            Me.lbl_num_producto.Text = hito.nro_hito
            Me.lbl_fecha_entrega.Text = hito.fecha_Esperada_entrega
            If hito.url IsNot Nothing Then
                Me.doc_admon_content.Visible = True
            End If
            Me.docs_admon.NavigateUrl = hito.url




            Dim hitosEnAprobacion = dbEntities.vw_ruta_aprobacion_hito_history.AsNoTracking.Where(Function(p) p.id_hito = idHito And p.id_estado_ruta_aprobacion_hito = 1).ToList()


            'Dim comentarios = dbEntities.vw_ruta_aprobacion_hito_history.Where(Function(p) p.id_hito = idHito).ToList()
            Dim idUserLogin = Me.Session("E_IdUser").ToString()


            'Dim aprobacionesPendientes = comentarios.Where(Function(p) p.fecha_envio = "Pendiente")
            'Dim lastComent = comentarios.OrderByDescending(Function(p) p.id_ruta_hito).Take(1).ToList()

            'If lastComent.Count() > 0 Then
            '    If lastComent.FirstOrDefault().id_responsable_aprueba_hito <> 2 And lastComent.FirstOrDefault().id_responsable_aprueba_hito <> 1 Then
            '        lastComent = comentarios.OrderByDescending(Function(p) p.id_ruta_hito).Take(2).ToList()
            '    End If
            'End If
            'Me.addComentario.Visible = False


            Dim ultimoEntregable = dbEntities.tme_ficha_proyecto_hitos.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto).OrderByDescending(Function(p) p.fecha_Esperada_entrega).FirstOrDefault()

            If ultimoEntregable IsNot Nothing Then
                If ultimoEntregable.id_hito = idHito Then
                    Me.ultimo_entregable.Value = 1
                End If
            End If

            If hito.aprobado <> True Then
                If hitosEnAprobacion.Count() > 0 Then
                    Dim comentUserApr = hitosEnAprobacion.Where(Function(p) p.id_usuario_aprueba.Contains(idUserLogin)).ToList()
                    If comentUserApr.Count() > 0 Then
                        Dim respons = comentUserApr.FirstOrDefault()
                        'Me.addComentario.Visible = True
                        Me.id_ruta_hito.Value = respons.id_ruta_hito
                        Me.tiene_ruta.Value = respons.tiene_ruta
                        Me.orden.Value = respons.orden
                        Me.id_rol.Value = respons.id_responsable_aprueba_hito
                        If respons.orden = 0 Then
                            Me.btn_ajustes.Visible = False
                            'Me.soporteURL.Visible = True
                            Me.rv_docs_admin.Enabled = True
                            For Each row In Me.grd_aportes.Items
                                If TypeOf row Is GridDataItem Then
                                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                    Dim modSoporte As HyperLink = CType(row.Cells(0).FindControl("hlk_productos"), HyperLink)
                                    modSoporte.Visible = True
                                End If
                            Next
                            If hito.tme_hitos_entregables.Count() <> hito.tme_hitos_entregables.Where(Function(p) p.soporte IsNot Nothing).Count() Then
                                Me.addComentario.Visible = False
                            End If
                            'Me.accioEncar.Visible = False
                        Else
                            Me.rv_docs_admin.Enabled = False
                            If hito.id_estado_aprobacion_hito <> 6 Then
                                'Me.accioEncar.Visible = True
                                'Me.accioEjecu.Visible = False
                            End If
                        End If
                    Else
                        Me.addComentario.Visible = False
                    End If
                    'If hito.id_estado_aprobacion_hito = 2 And Me.accioEjecu.Visible = False And Me.accioEncar.Visible = False Then
                    '    Me.comentariosAdiconales.Visible = True
                    'End If
                Else
                    Me.addComentario.Visible = False
                End If
            Else
                Me.addComentario.Visible = False
            End If

            'If addComentario.Visible = False Then
            '    If hito.id_estado_aprobacion_hito = 3 Then
            '        'Me.comentariosAdiconales.Visible = True
            '    End If
            'End If


        End Using
    End Sub
    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "~/Proyectos/frm_entregables_Act?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            'Dim hlnkActivar As HyperLink = New HyperLink
            'hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            'hlnkActivar.NavigateUrl = "~/Proyectos/frm_productos?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            Dim hlnktrn As HyperLink = New HyperLink
            hlnktrn = CType(e.Item.FindControl("hlk_view"), HyperLink)
            If DataBinder.Eval(e.Item.DataItem, "soporte") IsNot Nothing Then
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


            Dim hlk_productos As HyperLink = New HyperLink
            hlk_productos = CType(e.Item.FindControl("hlk_productos"), HyperLink)
            hlk_productos.NavigateUrl = "~/Proyectos/frm_ActualizarEntregable?id=" & DataBinder.Eval(e.Item.DataItem, "id_entregable_hito").ToString()
        End If
    End Sub

    Protected Sub btn_aprueba_pro_Click(sender As Object, e As EventArgs) Handles btn_aprueba.Click
        Dim sopExterno As Boolean = False
        Dim existeSoporte As Boolean = False
        Dim soporte As String = ""
        Dim comentarios As String = ""
        Dim idRutaEntre = Convert.ToInt32(Me.id_ruta_hito.Value)

        comentarios = Me.txtcoments.Text
        apruebaProducto(idRutaEntre, comentarios, soporte, True, True)
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_hitosEj?id=" & Me.lbl_id_ficha.Text
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    End Sub
    Sub apruebaProducto(ByVal id_ruta_hito As Integer, Optional comentarios As String = "", Optional soporte As String = "", Optional soporteExterno As Boolean = False, Optional existeSopotrte As Boolean = False)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_notificacion = 1025
                Dim strEmail = ""
                Dim tieneRuta = Convert.ToInt32(Me.tiene_ruta.Value)


                Dim ruta = New tme_ruta_aprobacion_hito
                If tieneRuta > 0 Then
                    ruta = dbEntities.tme_ruta_aprobacion_hito.Find(id_ruta_hito)
                End If
                Dim emil_to_send = ""
                Dim email_sime_don = ""
                'Dim actividad = ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto
                'Dim id_ejecutor = actividad.id_ejecutor
                Dim esEvio = False


                ruta.comentarios = comentarios
                ruta.id_usuario_ruta = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                ruta.fecha_envio = Date.Now
                ruta.id_estado_ruta_aprobacion_hito = 2
                'ruta.soporte = Me.txt_url.Text
                If existeSopotrte Then
                    ruta.soporte = soporte
                End If

                'Dim idSSubR = actividad.tme_ficha_subregion.Select(Function(p) p.id_subregion).ToList()
                Dim idHito = Convert.ToInt32(Me.id_hito.Value)
                Dim hito = dbEntities.tme_ficha_proyecto_hitos.Find(idHito)
                Dim idRolAprobacion = Convert.ToInt32(Me.id_rol.Value)
                Dim rolAp = dbEntities.tme_ficha_proyecto_rol.Find(idRolAprobacion)
                Dim ultimoEntregable = False
                If tieneRuta = 0 Then
                    ruta.fecha_crea = DateTime.Now
                    ruta.id_hito = hito.id_hito
                    ruta.id_rol_aprobacioh_hito = idRolAprobacion
                    dbEntities.tme_ruta_aprobacion_hito.Add(ruta)

                    If Me.txt_url_docs_admin.Text IsNot Nothing And Me.txt_url_docs_admin.Text <> "" Then
                        hito.url = Me.txt_url_docs_admin.Text
                    End If

                    hito.fecha_entrega = Date.Now
                    hito.id_estado_aprobacion_hito = 2

                    dbEntities.Entry(hito).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                    Dim rolesFicha = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto And p.id_rol_aprobacioh_hito <> idRolAprobacion And p.orden = rolAp.orden + 1).ToList()
                    If rolesFicha.Count() > 0 Then
                        For Each item In rolesFicha
                            ruta = New tme_ruta_aprobacion_hito
                            ruta.fecha_crea = DateTime.Now
                            ruta.id_hito = hito.id_hito
                            ruta.id_estado_ruta_aprobacion_hito = 1
                            ruta.id_rol_aprobacioh_hito = item.id_rol_aprobacioh_hito
                            dbEntities.tme_ruta_aprobacion_hito.Add(ruta)
                            dbEntities.SaveChanges()
                        Next

                        Dim emailRuta = rolesFicha.Select(Function(p) New With {Key .email = String.Join(",", p.tme_ficha_proyecto_rol_usuarios.Select(Function(x) x.t_usuarios.email_usuario))}).ToList()
                        emil_to_send = String.Join(",", emailRuta.Select(Function(p) p.email))

                        emil_to_send &= "," & hito.tme_Ficha_Proyecto.t_usuarios.email_usuario
                    End If
                Else
                    dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()
                    Dim rolFicha = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto And p.id_rol_aprobacioh_hito = idRolAprobacion).FirstOrDefault()
                    Dim totalOrden = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto And p.orden = rolFicha.orden).ToList()


                    Dim totalAprol = 0
                    For Each item In totalOrden
                        Dim rutaApp = dbEntities.tme_ruta_aprobacion_hito.Where(Function(p) p.id_hito = hito.id_hito And p.id_rol_aprobacioh_hito = item.id_rol_aprobacioh_hito And p.id_estado_ruta_aprobacion_hito = 2).ToList()
                        If rutaApp.Count() > 0 Then
                            totalAprol += 1
                        End If
                    Next
                    If totalAprol = totalOrden.Count() Then
                        If rolFicha IsNot Nothing Then
                            ultimoEntregable = If(Convert.ToInt32(Me.ultimo_entregable.Value) = 0, False, True)
                            Dim ultimoOrden = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto).OrderByDescending(Function(p) p.orden).FirstOrDefault()
                            If ultimoOrden.orden > rolAp.orden + 1 Then
                                ultimoEntregable = False
                            End If
                            Dim rolesFicha = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto And p.id_rol_aprobacioh_hito <> idRolAprobacion And p.orden = rolAp.orden + 1 And p.aprobacion_ultimo_entregable = ultimoEntregable).ToList()
                            If rolesFicha.Count() > 0 Then
                                For Each item In rolesFicha
                                    ruta = New tme_ruta_aprobacion_hito
                                    ruta.fecha_crea = DateTime.Now
                                    ruta.id_hito = hito.id_hito
                                    ruta.id_estado_ruta_aprobacion_hito = 1
                                    ruta.id_rol_aprobacioh_hito = item.id_rol_aprobacioh_hito
                                    dbEntities.tme_ruta_aprobacion_hito.Add(ruta)
                                    dbEntities.SaveChanges()
                                Next

                                Dim emailRuta = rolesFicha.Select(Function(p) New With {Key .email = String.Join(",", p.tme_ficha_proyecto_rol_usuarios.Select(Function(x) x.t_usuarios.email_usuario))}).ToList()
                                emil_to_send = String.Join(",", emailRuta.Select(Function(p) p.email))

                                emil_to_send &= "," & hito.tme_Ficha_Proyecto.t_usuarios.email_usuario
                            End If
                        End If
                    End If

                    Dim usuario = dbEntities.t_usuarios.Where(Function(p) p.id_usuario = ruta.id_usuario_ruta Or p.id_usuario = hito.tme_Ficha_Proyecto.id_usuario_responsable).ToList()

                    If emil_to_send.Length > 0 Then
                        emil_to_send &= "," & String.Join(",", usuario.Select(Function(p) p.email_usuario))
                    Else
                        emil_to_send = String.Join(",", usuario.Select(Function(p) p.email_usuario))
                    End If


                End If

                dbEntities.SaveChanges()
                Dim roles = New List(Of tme_ficha_proyecto_rol)
                If ultimoEntregable Then
                    roles = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto).ToList()
                Else
                    roles = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = hito.id_ficha_proyecto And p.aprobacion_ultimo_entregable = False).ToList()
                End If
                Dim totalAp = 0
                For Each item In roles
                    Dim rutaApp = dbEntities.tme_ruta_aprobacion_hito.Where(Function(p) p.id_hito = hito.id_hito And p.id_rol_aprobacioh_hito = item.id_rol_aprobacioh_hito And p.id_estado_ruta_aprobacion_hito = 2).ToList()
                    If rutaApp.Count() > 0 Then
                        totalAp += 1
                    End If
                Next
                If totalAp = roles.Count() Then
                    hito.id_estado_aprobacion_hito = 3
                    hito.fecha_aprobacion = DateTime.Now
                    hito.aprobado = True
                    dbEntities.Entry(hito).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()
                End If


                strEmail = "Aprobación de entregables subactividad " & hito.tme_Ficha_Proyecto.codigo_RFA

                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 0, id_notificacion, cl_user.regionalizacionCulture, 0)
                If (objEmail.Emailing_ENTREGABLES_ACTIONS(ruta.id_hito, strEmail, emil_to_send, hito.id_estado_aprobacion_hito, ruta.id_estado_ruta_aprobacion_hito, tieneRuta)) Then
                Else 'Error mandando Email
                End If


                'Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), id_notificacion, cl_user.regionalizacionCulture, cl_user.idSys)
                'If (objEmail.Emailing_ENTREGABLES_ACTIONS(ruta.id_hito, strEmail, emil_to_send, hito.id_estado_aprobacion_hito)) Then
                'Else 'Error mandando Email
                'End If

                'If ruta.id_responsable_aprueba_hito = 1 Then
                '    If Me.txt_url_docs_admin.Text IsNot Nothing And Me.txt_url_docs_admin.Text <> "" Then
                '        hito.url = Me.txt_url_docs_admin.Text
                '    End If
                '    'hito.url = Me.txt_url_docs_admin.Text
                '    ruta.id_estado_ruta_aprobacion_hito = 2

                '    Dim newRuta = New tme_ruta_aprobacion_hito

                '    If ruta.id_ruta_hito_solicito_ajuste IsNot Nothing Then


                '        Dim rutaSolicitoAjustes = dbEntities.tme_ruta_aprobacion_hito.Find(ruta.id_ruta_hito_solicito_ajuste)

                '        newRuta.id_responsable_aprueba_hito = rutaSolicitoAjustes.id_responsable_aprueba_hito
                '        newRuta.id_estado_ruta_aprobacion_hito = rutaSolicitoAjustes.id_estado_ruta_aprobacion_hito_original
                '        newRuta.id_usuario_ruta = rutaSolicitoAjustes.id_usuario_ruta
                '        newRuta.id_grupo_aprobacion_hito = rutaSolicitoAjustes.id_grupo_aprobacion_hito


                '    Else
                '        If Me.txt_url_docs_admin.Text IsNot Nothing And Me.txt_url_docs_admin.Text <> "" Then
                '            hito.url = Me.txt_url_docs_admin.Text
                '        End If

                '        'hito.url = Me.txt_url_docs_admin.Text
                '        ruta.id_estado_ruta_aprobacion_hito = 2
                '        hito.fecha_entrega = Date.Now
                '        hito.id_estado_aprobacion_hito = 2
                '        'id_notificacion = 11

                '        If idSSubR.Where(Function(p) p = 54).Count() > 0 Then
                '            newRuta.id_responsable_aprueba_hito = 3
                '            newRuta.id_estado_ruta_aprobacion_hito = 5
                '        Else
                '            newRuta.id_responsable_aprueba_hito = 2
                '            newRuta.id_estado_ruta_aprobacion_hito = 4
                '        End If



                '    End If

                '    If ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.id_subregion = 1 Then
                '        emil_to_send = "evelyn.moreno@wildlifeworks.com"
                '    End If

                '    'Dim clsNot As New ly_SIME.SIMEly.cls_notification()
                '    'Dim emailsNot = clsNot.get_ids_users_mye_donaciones()
                '    'For Each row As DataRow In emailsNot.Rows
                '    '    email_sime_don = email_sime_don + "," + row("email_usuario")
                '    'Next row


                '    'If emil_to_send <> "nt_to" Then
                '    '    emil_to_send = emil_to_send & ","
                '    'End If
                '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    newRuta.id_hito = ruta.id_hito
                '    newRuta.fecha_crea = Date.Now
                '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    dbEntities.SaveChanges()
                '    strEmail = "Envío de hitos por aprobación subactividad " & ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.codigo_RFA

                'ElseIf ruta.id_responsable_aprueba_hito = 2 Then

                '    'Dim personaltecnico = dbEntities.tme_ficha_proyecto_personal_tecnico.Where(Function(p) p.id_ficha_proyecto = ruta.tme_ficha_proyecto_hitos.id_ficha_proyecto).ToList()
                '    Dim newRuta = New tme_ruta_aprobacion_hito

                '    'If personaltecnico.Count() > 0 Then
                '    '    For Each personal In personaltecnico
                '    '        newRuta = New tme_ruta_aprobacion_hito
                '    '        newRuta.id_estado_ruta_aprobacion_hito = 5
                '    '        newRuta.id_hito = ruta.id_hito
                '    '        newRuta.id_responsable_aprueba_hito = 3
                '    '        If personal.es_grupo = True Then
                '    '            newRuta.id_grupo_aprobacion_hito = personal.id_grupo_arpobacion_hito
                '    '        Else
                '    '            newRuta.id_usuario_ruta = personal.id_usuario
                '    '        End If
                '    '        newRuta.fecha_crea = Date.Now
                '    '        newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '        dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '        dbEntities.SaveChanges()
                '    '    Next
                '    '    newRuta = New tme_ruta_aprobacion_hito
                '    '    newRuta.id_estado_ruta_aprobacion_hito = 6
                '    '    newRuta.id_responsable_aprueba_hito = 4
                '    '    newRuta.id_hito = ruta.id_hito
                '    '    newRuta.fecha_crea = Date.Now
                '    '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '    dbEntities.SaveChanges()
                '    'Else
                '    newRuta = New tme_ruta_aprobacion_hito
                '    newRuta.id_estado_ruta_aprobacion_hito = 5
                '    newRuta.id_responsable_aprueba_hito = 3
                '    newRuta.id_hito = ruta.id_hito
                '    newRuta.fecha_crea = Date.Now
                '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    dbEntities.SaveChanges()

                '    'newRuta = New tme_ruta_aprobacion_hito
                '    '    newRuta.id_estado_ruta_aprobacion_hito = 7
                '    '    newRuta.id_responsable_aprueba_hito = 5
                '    '    newRuta.id_hito = ruta.id_hito
                '    '    newRuta.fecha_crea = Date.Now
                '    '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '    dbEntities.SaveChanges()

                '    '    newRuta = New tme_ruta_aprobacion_hito
                '    '    newRuta.id_estado_ruta_aprobacion_hito = 8
                '    '    newRuta.id_responsable_aprueba_hito = 6
                '    '    newRuta.id_hito = ruta.id_hito
                '    '    newRuta.fecha_crea = Date.Now
                '    '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '    dbEntities.SaveChanges()
                '    'End If



                '    strEmail = "Aprobación hitos subactividad " & ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.codigo_RFA
                'ElseIf ruta.id_responsable_aprueba_hito = 3 Then
                '    strEmail = "Aprobación hitos subactividad " & ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.codigo_RFA
                '    dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                '    dbEntities.SaveChanges()
                '    'Dim personaltecnico = dbEntities.tme_ficha_proyecto_personal_tecnico.Where(Function(p) p.id_ficha_proyecto = ruta.tme_ficha_proyecto_hitos.id_ficha_proyecto).ToList()
                '    Dim personaltecnicoaprobo = dbEntities.vw_ruta_aprobacion_hito_history.Where(Function(p) p.id_hito = ruta.id_hito And p.fecha_envio <> "Pendiente" And p.id_estado_ruta_aprobacion_hito = 5).ToList()

                '    Dim newRuta = New tme_ruta_aprobacion_hito
                '    newRuta.id_estado_ruta_aprobacion_hito = 6
                '    newRuta.id_hito = ruta.id_hito
                '    newRuta.id_responsable_aprueba_hito = 4
                '    newRuta.fecha_crea = Date.Now
                '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    dbEntities.SaveChanges()

                '    'If personaltecnico.Count() = personaltecnicoaprobo.Count() Then
                '    '    Dim newRuta = New tme_ruta_aprobacion_hito
                '    '    newRuta.id_estado_ruta_aprobacion_hito = 7
                '    '    newRuta.id_hito = ruta.id_hito
                '    '    newRuta.id_responsable_aprueba_hito = 5
                '    '    newRuta.fecha_crea = Date.Now
                '    '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '    dbEntities.SaveChanges()

                '    '    newRuta = New tme_ruta_aprobacion_hito
                '    '    newRuta.id_estado_ruta_aprobacion_hito = 8
                '    '    newRuta.id_responsable_aprueba_hito = 6
                '    '    newRuta.id_hito = ruta.id_hito
                '    '    newRuta.fecha_crea = Date.Now
                '    '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    '    dbEntities.SaveChanges()
                '    'End If

                'ElseIf ruta.id_responsable_aprueba_hito = 4 Then
                '    dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                '    dbEntities.SaveChanges()

                '    Dim newRuta = New tme_ruta_aprobacion_hito
                '    newRuta.id_estado_ruta_aprobacion_hito = 8
                '    newRuta.id_hito = ruta.id_hito
                '    newRuta.id_responsable_aprueba_hito = 5
                '    newRuta.fecha_crea = Date.Now
                '    newRuta.id_estado_ruta_aprobacion_hito_original = newRuta.id_estado_ruta_aprobacion_hito
                '    dbEntities.tme_ruta_aprobacion_hito.Add(newRuta)
                '    dbEntities.SaveChanges()


                '    'Dim nroAprobaciones = dbEntities.vw_ruta_aprobacion_hito_history.Where(Function(p) p.id_hito = ruta.id_hito).Where(Function(p) p.id_responsable_aprueba_hito <> 1 And p.fecha_envio <> "Pendiente" And p.id_estado_ruta_aprobacion_hito <> 3).Count()

                '    'Dim totalRutaAprobacion = dbEntities.tme_ficha_proyecto_personal_tecnico.Where(Function(p) p.id_ficha_proyecto = ruta.tme_ficha_proyecto_hitos.id_ficha_proyecto).ToList().Count()

                '    'totalRutaAprobacion = totalRutaAprobacion + 4

                '    'If totalRutaAprobacion = nroAprobaciones Then
                '    '    Dim hitoUp = dbEntities.tme_ficha_proyecto_hitos.Find(ruta.id_hito)
                '    '    hitoUp.aprobado = True
                '    '    hitoUp.fecha_aprobacion = DateTime.Now()
                '    '    hitoUp.id_estado_aprobacion_hito = 3
                '    '    dbEntities.Entry(hitoUp).State = Entity.EntityState.Modified
                '    'End If
                '    'ElseIf ruta.id_responsable_aprueba_hito = 6 Then
                '    '    Dim hitoUp = dbEntities.tme_ficha_proyecto_hitos.Find(ruta.id_hito)
                '    '    hitoUp.aprobado = True
                '    '    hitoUp.fecha_aprobacion = DateTime.Now()
                '    '    hitoUp.id_estado_aprobacion_hito = 3
                '    '    dbEntities.Entry(hitoUp).State = Entity.EntityState.Modified
                '    strEmail = "Aprobación hitos subactividad " & ruta.tme_ficha_proyecto_hitos.tme_Ficha_Proyecto.codigo_RFA
                'ElseIf ruta.id_responsable_aprueba_hito = 5 Then
                '    dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                '    dbEntities.SaveChanges()

                '    Dim nroAprobaciones = dbEntities.vw_ruta_aprobacion_hito_history.Where(Function(p) p.id_hito = ruta.id_hito).Where(Function(p) p.id_responsable_aprueba_hito <> 1 And p.fecha_envio <> "Pendiente" And p.id_estado_ruta_aprobacion_hito <> 3).Count()
                '    'Dim totalRutaAprobacion = 4


                '    Dim totalRutaAprobacion = 0


                '    If idSSubR.Where(Function(p) p = 54).Count() > 0 Then
                '        totalRutaAprobacion = 3
                '    Else
                '        totalRutaAprobacion = 4
                '    End If
                '    'Dim totalRutaAprobacion = dbEntities.tme_ficha_proyecto_personal_tecnico.Where(Function(p) p.id_ficha_proyecto = ruta.tme_ficha_proyecto_hitos.id_ficha_proyecto).ToList().Count()

                '    'totalRutaAprobacion = totalRutaAprobacion + 4

                '    If totalRutaAprobacion = nroAprobaciones Then
                '        Dim hitoUp = dbEntities.tme_ficha_proyecto_hitos.Find(ruta.id_hito)
                '        hitoUp.aprobado = True
                '        hitoUp.fecha_aprobacion = DateTime.Now()
                '        hitoUp.id_estado_aprobacion_hito = 3
                '        dbEntities.Entry(hitoUp).State = Entity.EntityState.Modified
                '    End If
                'End If
                'dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                'dbEntities.SaveChanges()

                'Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), id_notificacion, cl_user.regionalizacionCulture, cl_user.idSys)
                'If (objEmail.Emailing_PRODUCTS_ACTIONS(ruta.id_hito, strEmail, emil_to_send, id_ejecutor, email_sime_don)) Then
                'Else 'Error mandando Email
                'End If

            End Using
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    'Protected Sub cmb_anio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_anio.SelectedIndexChanged
    '    getCmbSelected()
    'End Sub

    Sub fillGrid(ByVal caso As Integer)
        Dim sqlFiltros = " "

        Select Case caso
            Case 3
        End Select
        Me.SqlDts_entregables.SelectCommand = "select * from vw_tme_ficha_proyecto_hitos_entregables where id_hito = " & Me.id_hito.Value & sqlFiltros & " order by nro_entregable"
        Me.grd_aportes.DataBind()
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Proyectos/frm_hitosEj?id=" & Me.lbl_id_ficha.Text)
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_proyectosDocumentos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
End Class