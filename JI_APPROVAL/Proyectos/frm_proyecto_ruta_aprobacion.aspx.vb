Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class frm_proyecto_ruta_aprobacion
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_PROY_APOR"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
    Dim dtConceptos As New DataTable

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

                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosEdit?Id=" & id.ToString()))
                Me.alink_aportes.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_ProyectosAportes?Id=" & id.ToString()))
                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_ficha_proyecto)

                Session.Remove("dtConceptos")
                fillGridRoles()
            End Using
        End If
    End Sub

    Protected Sub add_rol_Click(sender As Object, e As EventArgs) Handles add_rol.Click
        Me.RadWindow2.VisibleOnPageLoad = True
        Me.RadWindow2.Visible = True
        LoadList()
    End Sub
    Sub LoadList()
        Using dbEntities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim usuarios = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = idPrograma And p.id_estado_usr = 1 And p.id_tipo_usuario = 1).ToList()

            Me.cmb_personal_asociado.DataSourceID = ""
            Me.cmb_personal_asociado.DataSource = usuarios
            Me.cmb_personal_asociado.DataTextField = "nombre_usuario"
            Me.cmb_personal_asociado.DataValueField = "id_usuario"
            Me.cmb_personal_asociado.DataBind()
        End Using
    End Sub
    Protected Sub btn_agregar_persona_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_persona.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                If Session("dtConceptos") IsNot Nothing Then
                    dtConceptos = Session("dtConceptos")
                Else
                    createdtcolums()
                End If
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio
                Dim id_usuario = Convert.ToInt32(Me.cmb_personal_asociado.SelectedValue)
                Dim usuario = dbEntities.t_usuarios.Find(id_usuario)
                dtConceptos.Rows.Add(0, usuario.nombre_usuario & " " & usuario.apellidos_usuario, usuario.t_job_title.job, usuario.email_usuario, id_usuario)
                Session("dtConceptos") = dtConceptos
                Me.RadWindow2.VisibleOnPageLoad = True
                Me.RadWindow2.Visible = True
                fillGrid()
            Catch ex As Exception
                Dim mensaje = ex.Message
            End Try
        End Using


    End Sub

    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_rol_usuario_aprobacion", GetType(Integer))
            dtConceptos.Columns.Add("nombre_usuario", GetType(String))
            dtConceptos.Columns.Add("cargo", GetType(String))
            dtConceptos.Columns.Add("correo", GetType(String))
            dtConceptos.Columns.Add("id_usuario", GetType(Integer))
        End If
    End Sub

    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        Me.grd_conceptos.DataSource = dtConceptos
        Me.grd_conceptos.DataBind()
    End Sub
    Protected Sub grd_conceptos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_conceptos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_rol_usuario_aprobacion").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_rol_usuario_aprobacion").ToString())
            hlnkDelete.Attributes.Add("data-user", DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub btn_agregar_rol_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_rol.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                If Session("dtConceptos") IsNot Nothing Then
                    dtConceptos = Session("dtConceptos")
                Else
                    createdtcolums()
                End If


                Dim rolFicha = New tme_ficha_proyecto_rol
                Dim idRol = Convert.ToInt32(Me.id_rol.Value)
                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                If idRol > 0 Then
                    rolFicha = dbEntities.tme_ficha_proyecto_rol.Find(idRol)
                End If

                rolFicha.descripcion = Me.txt_rol.Text
                rolFicha.orden = Me.txt_orden.Value
                rolFicha.tipo = Me.rbn_tipo_aprobacion.SelectedValue
                rolFicha.id_ficha_proyecto = id
                rolFicha.aprobacion_ultimo_entregable = If(Me.rbn_ultimo_entregable.SelectedValue = "0", False, True)
                If idRol > 0 Then
                    dbEntities.Entry(rolFicha).State = Entity.EntityState.Modified
                Else
                    dbEntities.tme_ficha_proyecto_rol.Add(rolFicha)
                End If
                dbEntities.SaveChanges()

                For Each row As DataRow In dtConceptos.Rows
                    Dim oRolUsuario As New tme_ficha_proyecto_rol_usuarios
                    Dim idUsuario As Integer = row("id_usuario")
                    Dim idRolUsuario = row("id_rol_usuario_aprobacion")

                    If idRolUsuario = 0 Then
                        oRolUsuario.id_usuario = idUsuario
                        oRolUsuario.id_rol = rolFicha.id_rol_aprobacioh_hito

                        dbEntities.tme_ficha_proyecto_rol_usuarios.Add(oRolUsuario)
                        dbEntities.SaveChanges()

                        Dim usuAct = dbEntities.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = idUsuario And p.id_ficha_proyecto = id).ToList()
                        If usuAct.Count() = 0 Then
                            Dim oUsuaAct = New t_usuario_ficha_proyecto
                            oUsuaAct.id_usuario = idUsuario
                            oUsuaAct.id_ficha_proyecto = id
                            oUsuaAct.fecha_crea = DateTime.Now
                            oUsuaAct.acc_act = True
                            oUsuaAct.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                            dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                            dbEntities.SaveChanges()
                        End If
                    End If


                Next






                fillGridRoles()

                Me.RadWindow2.VisibleOnPageLoad = False
                Me.id_rol.Value = 0
                dtConceptos = New DataTable
                Session.Remove("dtConceptos")
                fillGrid()
            Catch ex As Exception
                Dim mensaje = ex.Message
            End Try
        End Using


    End Sub
    Protected Sub delete_concepto(sender As Object, e As EventArgs)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim a = CType(sender, LinkButton)
            Dim idRolUser = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Dim idUser = Convert.ToInt32(a.Attributes.Item("data-user").ToString())

            If idRolUser > 0 Then
                Dim rolUser = dbEntities.tme_ficha_proyecto_rol_usuarios.Find(idRolUser)
                dbEntities.Entry(rolUser).State = Entity.EntityState.Deleted
                dbEntities.SaveChanges()
            End If

            Dim foundRow As DataRow() = dtConceptos.Select("id_usuario = '" & idUser.ToString() & "'")
            If foundRow.Count > 0 Then
                dtConceptos.Rows.Remove(foundRow(0))
            End If
            Session("dtConceptos") = dtConceptos
            fillGrid()
        End Using


        'If Session("dtConceptos") IsNot Nothing Then
        '    dtConceptos = Session("dtConceptos")
        'Else
        '    createdtcolums()
        'End If

        'Dim idTipo = Convert.ToInt32(Me.tipo_delete.Value)
        'If idTipo = 1 Then
        '    Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

        '    Dim foundRow As DataRow() = dtConceptos.Select("id_factura_detalle = '" & idConcepto.ToString() & "'")

        '    If foundRow.Count > 0 Then
        '        dtConceptos.Rows.Remove(foundRow(0))
        '    End If
        '    Session("dtConceptos") = dtConceptos
        '    'fillGrid()

        '    Me.RadWindow2.VisibleOnPageLoad = True
        '    Me.RadWindow2.Visible = True

        '    fillGrid()
        'ElseIf idTipo = 2 Then
        '    Using dbEntities As New dbRMS_JIEntities
        '        Dim idDetalle = Convert.ToInt32(Me.identity.Value)
        '        Dim detalle = dbEntities.tme_par_factura.Find(idDetalle)
        '        If detalle IsNot Nothing Then
        '            dbEntities.Entry(detalle).State = Entity.EntityState.Deleted
        '            dbEntities.SaveChanges()
        '        End If
        '        fillGridFacturas()
        '        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        '    End Using
        'End If
    End Sub

    Sub fillGridRoles()
        Using dbEntities As New dbRMS_JIEntities
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
            Dim roles = dbEntities.tme_ficha_proyecto_rol.Where(Function(p) p.id_ficha_proyecto = id).ToList().Select(Function(p) New With {Key _
                                                                                                                      p.descripcion,
                                                                                                                      p.id_rol_aprobacioh_hito,
                                                                                                                      p.orden,
                                                                                                                      p.tipo,
                                                                                                                      .ultimo_entregable = If(p.aprobacion_ultimo_entregable.Value = True, "SÍ", "NO"),
                                                                                                                      .usuarios = String.Join(",", p.tme_ficha_proyecto_rol_usuarios.Select(Function(x) x.t_usuarios.nombre_usuario & " " & x.t_usuarios.apellidos_usuario))}).ToList()

            Me.grd_roles.DataSource = roles
            Me.grd_roles.DataBind()
        End Using
    End Sub

    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_roles.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim col_hlk_editar As LinkButton = New LinkButton
            col_hlk_editar = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
            col_hlk_editar.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_rol_aprobacioh_hito").ToString())
            col_hlk_editar.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_rol_aprobacioh_hito").ToString())
            col_hlk_editar.ToolTip = controles.iconosGrid("col_hlk_editar")


        End If
    End Sub

    Protected Sub editar_ruta(sender As Object, e As EventArgs)
        Session.Remove("dtConceptos")
        createdtcolums()

        Using dbEntities As New dbRMS_JIEntities
            Dim a = CType(sender, LinkButton)
            Dim idRol = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Me.id_rol.Value = idRol

            Dim rol = dbEntities.tme_ficha_proyecto_rol.Find(idRol)
            LoadList()
            Me.txt_rol.Text = rol.descripcion
            Me.txt_orden.Value = rol.orden
            Me.rbn_tipo_aprobacion.SelectedValue = rol.tipo
            Me.rbn_ultimo_entregable.SelectedValue = If(rol.aprobacion_ultimo_entregable, 1, 0)



            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio

            For Each item In rol.tme_ficha_proyecto_rol_usuarios
                dtConceptos.Rows.Add(item.id_rol_usuario_aprobacion, item.t_usuarios.nombre_usuario & " " & item.t_usuarios.apellidos_usuario, item.t_usuarios.t_job_title.job, item.t_usuarios.email_usuario, item.id_usuario)
            Next

            Session("dtConceptos") = dtConceptos
            Me.RadWindow2.VisibleOnPageLoad = True
            Me.RadWindow2.Visible = True
            fillGrid()

        End Using

    End Sub
End Class