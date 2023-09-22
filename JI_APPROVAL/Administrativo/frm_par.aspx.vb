Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_par
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADM_PAR"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim idUser As Integer = 0
    Dim varUSer As String
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

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
        If Not Me.IsPostBack Then
            'LoadLista()

            Dim Sql As String = String.Format("SELECT nombre_usuario as nombre_empleado, usuario, job as codigo FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            '"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("permisos")
            dm.Fill(ds, "permisos")
            Dim verTodo = ds.Tables("permisos").Rows(0).Item(0)

            varUSer = ds.Tables("permisos").Rows(0).Item("nombre_empleado")

            RadioButton1.Text = String.Format("Solicitudes pendientes de aprobación por {0}", varUSer)
            RadioButton2.Text = String.Format("Solicitudes pendientes de aprobación por otro usuario", varUSer)
            RadioButton3.Text = String.Format("Solicitudes realizadas por {0}", varUSer)
            RadioButton4.Text = String.Format("Todas las solicitudes")
            RadioButton5.Text = String.Format("Solicitudes aprobadas por {0}", varUSer)

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
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/administrativo/frm_parAD")
    End Sub
    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            'Dim par = dbEntities.vw_tme_par.ToList()

            Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim solicitudesPar As List(Of vw_tme_par) = New List(Of vw_tme_par)


            Select Case id_filtro_busqueda_viajes
                Case 1
                    solicitudesPar = dbEntities.vw_tme_par.OrderByDescending(Function(p) p.id_par).ToList()
                Case 2
                    solicitudesPar = dbEntities.vw_tme_par.Where(Function(p) (p.id_usuario_app.Contains(id_User)) Or
                                                                     (p.id_supervisor = id_User) Or
                                                                     (p.id_usuario_radica.Contains(id_User)) Or subR.Contains(p.id_subregion) Or p.id_usuario = id_User).OrderByDescending(Function(p) p.id_par).ToList()
                Case 3
                    solicitudesPar = dbEntities.vw_tme_par.Where(Function(p) (p.id_usuario_app.Contains(id_User)) Or
                                                                     (p.id_supervisor = id_User) Or
                                                                     (p.id_usuario_radica.Contains(id_User)) Or p.id_usuario = id_User).OrderByDescending(Function(p) p.id_par).ToList()

            End Select

            If Me.RadioButton1.Checked = True Then
                solicitudesPar = solicitudesPar.Where(Function(p) ((p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7))).ToList()
            ElseIf Me.RadioButton2.Checked = True Then
                solicitudesPar = solicitudesPar.Where(Function(p) (Not p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7)).ToList()
            ElseIf Me.RadioButton3.Checked = True Then
                solicitudesPar = solicitudesPar.Where(Function(p) p.id_usuario = id_User).ToList()
            ElseIf Me.RadioButton5.Checked = True Then
                solicitudesPar = solicitudesPar.Where(Function(p) p.id_usuario_app.Contains(id_User) And p.id_estadoDoc = 7).ToList()
            End If
            Me.grd_cate.DataSource = solicitudesPar

            If bndBind Then
                Me.grd_cate.DataBind()
            End If

            'Me.grd_cate.DataSource = dbEntities.vw_tme_par.OrderByDescending(Function(p) p.id_par).ToList()
            'If bndBind Then
            '    Me.grd_cate.DataBind()
            'End If
        End Using
    End Sub
    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        fillGrid(True)
    End Sub
    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        fillGrid(True)
    End Sub
    Protected Sub RadioButton3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        fillGrid(True)
    End Sub
    Protected Sub RadioButton4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        fillGrid(True)
    End Sub
    Protected Sub RadioButton5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        fillGrid(True)
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
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            'Dim hlnkEdit As HyperLink = New HyperLink
            'hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)


            'Dim facturaAnulada = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "anulada"))

            'Dim hlnkEstado As HyperLink = New HyperLink
            'hlnkEstado = CType(e.Item.FindControl("col_hlk_revertir"), HyperLink)
            'hlnkEstado.NavigateUrl = "frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlkPermisos As HyperLink = New HyperLink
            hlkPermisos = CType(e.Item.FindControl("col_hlk_mod"), HyperLink)
            hlkPermisos.NavigateUrl = "frm_par_permisos?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()

            Dim habilitarPermisos = Convert.ToString(Me.h_Filter.Value)

            Dim hlnkDetalle As HyperLink = New HyperLink
            hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            hlnkDetalle.NavigateUrl = "frm_parDetalle?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_parEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()

            Dim idUsuario = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())
            Dim idDocumento = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_documento").ToString())

            Dim id_usuario_radica As String() = itemD("id_usuario_radica").Text.ToString.Split(",")
            Dim id_usuario_radico = Convert.ToInt32(itemD("id_usuario_radico").Text.ToString)

            Dim hlnkSeguimiento As HyperLink = New HyperLink
            hlnkSeguimiento = CType(e.Item.FindControl("col_hlk_seguimiento"), HyperLink)
            hlnkSeguimiento.Visible = False
            Dim id_estadDoc = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc").ToString())

            Dim id_supervisor = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_supervisor").ToString())

            If id_supervisor = idUser Or habilitarPermisos <> "" Then
                hlkPermisos.Visible = True
            Else
                hlkPermisos.Visible = False
            End If



            Dim xxx = itemD("par_cancelado").Text.ToString()
            Dim par_cancelado As Boolean = Convert.ToBoolean(itemD("par_cancelado").Text.ToString())
            If idUsuario <> idUser Or (idDocumento > 0 And id_estadDoc <> 4) Then
                hlnkEdit.Visible = False
            ElseIf idUsuario = idUser And par_cancelado = True And id_estadDoc = 4 Then
                hlnkEdit.Visible = True
            End If

            Dim hlnkFacturacion As HyperLink = New HyperLink
            hlnkFacturacion = CType(e.Item.FindControl("col_hlk_facturacion"), HyperLink)

            Dim intOwner As String() = itemD("id_usuario_app").Text.ToString.Split(",")

            If intOwner.Where(Function(p) p.Contains(idUser)).Count() > 0 And id_estadDoc <> 7 Then
                hlnkSeguimiento.Visible = True
                If idUsuario = idUser And id_estadDoc = 6 Then
                    hlnkSeguimiento.NavigateUrl = "frm_parEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                    hlnkEdit.Visible = True
                    hlnkEdit.NavigateUrl = "frm_parEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                Else
                    hlnkSeguimiento.NavigateUrl = "frm_parSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                End If

                'hlnkSeguimiento.NavigateUrl = "frm_parSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/accept.png"
                hlnkSeguimiento.ToolTip = "Continuar con la aprobación"
            ElseIf id_estadDoc = 7 Then
                hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/01_SI.png"
                hlnkSeguimiento.ToolTip = "Aprobado / Proceso finalizado"
                hlnkSeguimiento.Visible = True

                If intOwner.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                    hlnkEdit.NavigateUrl = "frm_parEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                    hlnkEdit.Visible = True
                End If
            ElseIf idDocumento > 0 Then
                    hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkSeguimiento.ToolTip = "En proceso de aprobación"
                hlnkSeguimiento.Visible = True
            End If


            If id_estadDoc = 7 And id_usuario_radico = 0 And id_usuario_radica.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.NavigateUrl = "frm_par_facturacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
            ElseIf id_estadDoc = 7 And id_usuario_radico <> 0 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/printer_off.png"
                hlnkFacturacion.ToolTip = "Facturación registrada"
                hlnkFacturacion.Target = "_blank"
                hlnkFacturacion.NavigateUrl = "frm_par_facturacionPrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
            End If

            'If facturaAnulada Then
            '    hlnkEdit.Visible = False
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
            '    hlnkEstado.ToolTip = "Factura anulada"
            'Else
            '    hlnkEstado.NavigateUrl = "frm_facturacionAnular?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()
            '    hlnkEdit.NavigateUrl = "frm_facturacionEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()
            'End If

            Dim hlnkEstado As HyperLink = New HyperLink
            hlnkEstado = CType(e.Item.FindControl("col_hlk_revertir"), HyperLink)
            Dim facturaAnulada = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "anulado"))

            'If idUsuario <> idUser Then
            '    hlnkEstado.Visible = False
            'End If

            If intOwner.Where(Function(p) p.Contains(idUser)).Count() > 0 And id_estadDoc <> 0 Then
                If facturaAnulada Then
                    hlnkEdit.Visible = False
                    hlnkSeguimiento.Visible = False
                    hlnkFacturacion.Visible = False
                    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                    hlnkEstado.ToolTip = "Par anulado"
                    hlnkEstado.Visible = True
                    hlkPermisos.Visible = False
                Else
                    hlnkEstado.NavigateUrl = "frm_parAnular?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                End If
            ElseIf idUsuario = idUser And id_estadDoc = 0 Then
                If facturaAnulada Then
                    hlnkEdit.Visible = False
                    hlnkSeguimiento.Visible = False
                    hlnkFacturacion.Visible = False
                    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                    hlnkEstado.ToolTip = "Par anulado"
                    hlnkEstado.Visible = True
                    hlkPermisos.Visible = False
                Else
                    hlnkEstado.NavigateUrl = "frm_parAnular?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString()
                End If
            Else
                hlnkEstado.Visible = False
                If facturaAnulada Then
                    hlnkEdit.Visible = False
                    hlnkSeguimiento.Visible = False
                    hlnkFacturacion.Visible = False
                    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                    hlnkEstado.ToolTip = "Par anulado"
                    hlnkEstado.Visible = True
                    hlkPermisos.Visible = False
                End If
            End If

            Dim editar_par As Boolean = Convert.ToBoolean(itemD("editar_par").Text.ToString())
            If editar_par And idUsuario = idUser Then
                hlnkEdit.Visible = True
                hlnkEdit.NavigateUrl = "frm_parEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString() & "&e=1"
            End If

            Dim habilitar_facturacion As Boolean = Convert.ToBoolean(itemD("habilitar_facturacion").Text.ToString())
            If habilitar_facturacion And id_usuario_radica.Where(Function(p) p.Contains(idUser)).Count() > 0 And id_estadDoc = 7 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.NavigateUrl = "frm_par_facturacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_par").ToString() & "&e=1"
            ElseIf habilitar_facturacion Then
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.Enabled = False
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
            End If
        End If


    End Sub
End Class