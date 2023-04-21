Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_viajes
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES"
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
    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            'id_usuario_app
            'id_usuario_app_legalizacion
            'id_usuario_app_informe

            Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim solicitudesViaje As List(Of vw_tme_solicitud_viaje) = New List(Of vw_tme_solicitud_viaje)

            'Me.grd_cate.DataSource = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) (p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
            '                                                                     (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
            '                                                                     (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7) Or
            '                                                                     (p.id_usuario_radica.Contains(id_User)) Or p.id_usuario = id_User).ToList()

            Select Case id_filtro_busqueda_viajes
                Case 1
                    solicitudesViaje = dbEntities.vw_tme_solicitud_viaje.OrderByDescending(Function(p) p.id_viaje).ToList()
                Case 2
                    solicitudesViaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) (p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
                                                                                         (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
                                                                                         (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7) Or
                                                                                         (p.id_usuario_radica.Contains(id_User)) Or subR.Contains(p.id_sub_region) Or p.id_usuario = id_User Or
                                                                                         p.id_supervisor = id_User).OrderByDescending(Function(p) p.id_viaje).ToList()
                Case 3
                    solicitudesViaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) (p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
                                                                                         (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
                                                                                         (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7) Or
                                                                                         (p.id_usuario_radica.Contains(id_User)) Or p.id_usuario = id_User Or p.id_supervisor = id_User).OrderByDescending(Function(p) p.id_viaje).ToList()
            End Select

            If Me.RadioButton1.Checked = True Then
                solicitudesViaje = solicitudesViaje.Where(Function(p) p.anulado = False And ((p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
                                                                                         (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
                                                                                         (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7))).ToList()
            ElseIf Me.RadioButton2.Checked = True Then
                solicitudesViaje = solicitudesViaje.Where(Function(p) (Not p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
                                                                                         (Not p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
                                                                                         (Not p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7)).ToList()
            ElseIf Me.RadioButton3.Checked = True Then
                solicitudesViaje = solicitudesViaje.Where(Function(p) p.id_usuario = id_User).ToList()
            End If
            Me.grd_cate.DataSource = solicitudesViaje

            If bndBind Then
                Me.grd_cate.DataBind()
            End If
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
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/administrativo/frm_viajesAD")
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
            Dim intOwner As String() = itemD("id_usuario_app").Text.ToString.Split(",")


            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)

            Dim hlkPermisos As HyperLink = New HyperLink
            hlkPermisos = CType(e.Item.FindControl("col_hlk_mod"), HyperLink)
            hlkPermisos.NavigateUrl = "frm_viaje_permisos?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()


            Dim facturaAnulada = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "anulado"))

            Dim hlnkEstado As HyperLink = New HyperLink
            hlnkEstado = CType(e.Item.FindControl("col_hlk_revertir"), HyperLink)
            'hlnkEstado.NavigateUrl = "frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlnkDetalle As HyperLink = New HyperLink
            hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            hlnkDetalle.NavigateUrl = "frm_viajeDetalle2?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()

            Dim hlnkSeguimiento As HyperLink = New HyperLink
            hlnkSeguimiento = CType(e.Item.FindControl("col_hlk_seguimiento"), HyperLink)
            hlnkSeguimiento.Visible = False

            Dim id_supervisor = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_supervisor").ToString())
            Dim idUsuario = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())
            Dim id_estadDoc = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc").ToString())
            Dim idDocumento = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_documento").ToString())

            Dim numero_reserva = ""
            If DataBinder.Eval(e.Item.DataItem, "numero_reserva") IsNot Nothing Then
                numero_reserva = DataBinder.Eval(e.Item.DataItem, "numero_reserva").ToString()
            End If

            Dim hlnkFacturacion As HyperLink = New HyperLink
            hlnkFacturacion = CType(e.Item.FindControl("col_hlk_facturacion"), HyperLink)


            Dim hlnkCodigoReserva As LinkButton = New LinkButton
            hlnkCodigoReserva = CType(e.Item.FindControl("col_hlk_codigo_Reserva"), LinkButton)
            hlnkCodigoReserva.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString())
            hlnkCodigoReserva.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString())
            hlnkCodigoReserva.ToolTip = controles.iconosGrid("col_hlk_codigo_Reserva")

            Dim imgHlnkCodigoReserva As Image = New Image
            imgHlnkCodigoReserva = CType(e.Item.FindControl("img_codigo_reserva"), Image)


            Dim servicioAereo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "requiere_servicio_aereo").ToString())
            If idUsuario <> idUser Then
                hlnkEstado.Visible = False
            End If



            If idUsuario <> idUser Or (idDocumento > 0 And id_estadDoc <> 6) Then
                hlnkEdit.Visible = False
                'hlnkEstado.Visible = False
                hlnkSeguimiento.Visible = False
            End If

            If intOwner.Where(Function(p) p.Contains(idUser)).Count() > 0 And id_estadDoc <> 7 Then
                hlnkSeguimiento.Visible = True
                If idUsuario = idUser And id_estadDoc = 6 Then
                    hlnkSeguimiento.NavigateUrl = "frm_viajeEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                    hlnkEdit.NavigateUrl = "frm_viajeEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                Else
                    hlnkSeguimiento.NavigateUrl = "frm_viajeSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                End If
                hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/accept.png"
                hlnkSeguimiento.ToolTip = "Solicitud de viaje - Continuar con la aprobación"
            ElseIf id_estadDoc = 7 Then
                hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/printer_off.png"
                hlnkSeguimiento.ToolTip = "Solicitud de viaje - Aprobado / Proceso finalizado"
                hlnkSeguimiento.NavigateUrl = "frm_viajePrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkSeguimiento.Visible = True
                hlnkSeguimiento.Target = "_blank"
            ElseIf idDocumento > 0 Then
                hlnkSeguimiento.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkSeguimiento.ToolTip = "Solicitud de viaje - En proceso de aprobación"
                hlnkSeguimiento.Visible = True
            End If



            '*********************Legalización del viaje*******************************'

            Dim hlnkLegalizacionSeguimiento As HyperLink = New HyperLink
            hlnkLegalizacionSeguimiento = CType(e.Item.FindControl("col_hlk_seguimiento_legalizacion"), HyperLink)
            hlnkLegalizacionSeguimiento.Visible = False

            Dim hlk_informe As HyperLink = New HyperLink
            hlk_informe = CType(e.Item.FindControl("col_hlk_informe"), HyperLink)
            hlk_informe.Visible = False

            'Dim id_estadDocLegalizacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc_legalizacion").ToString())
            'Dim id_estadDocLegalizacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc_legalizacion").ToString())
            'Dim id_estadoDoc_informe = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc_informe").ToString())
            'Dim intOwnerLegalizacion As String() = itemD("id_usuario_app_legalizacion").Text.ToString.Split(",")

            Dim id_estadDocLegalizacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc_legalizacion").ToString())
            Dim id_estadoDoc_informe = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estadoDoc_informe").ToString())
            Dim idDocumentoLegalizacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_documento_legalizacion").ToString())
            Dim intOwnerLegalizacion As String() = itemD("id_usuario_app_legalizacion").Text.ToString.Split(",")
            Dim intOwnerInforme As String() = itemD("id_usuario_app_informe").Text.ToString.Split(",")
            Dim id_usuario_radica As String() = itemD("id_usuario_radica").Text.ToString.Split(",")
            Dim id_usuario_radico = Convert.ToInt32(itemD("id_usuario_radico").Text.ToString)

            If id_estadDoc = 7 And numero_reserva.Length > 0 Then
                imgHlnkCodigoReserva.ImageUrl = "~/Imagenes/iconos/Ok.png"
            ElseIf id_estadDoc <> 7 Then
                hlnkCodigoReserva.Visible = False
            End If

            If id_estadDoc = 7 And id_usuario_radico = 0 And id_usuario_radica.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.NavigateUrl = "frm_viaje_facturacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
            ElseIf id_estadDoc = 7 And id_usuario_radico <> 0 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/printer_off.png"
                hlnkFacturacion.NavigateUrl = "frm_viaje_facturacionPrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkFacturacion.Target = "_blank"
                hlnkFacturacion.ToolTip = "Facturación registrada"
            ElseIf id_estadDoc <> 7 Then
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
                hlnkFacturacion.Visible = False
            Else
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
            End If



            If id_estadDoc = 7 And id_estadoDoc_informe = 7 And id_estadDocLegalizacion = 0 And idUsuario = idUser Then
                hlnkLegalizacionSeguimiento.Visible = True
                hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkLegalizacionSeguimiento.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                hlnkLegalizacionSeguimiento.ToolTip = "Pendiente de legalización"
            ElseIf id_estadDoc = 7 And (id_estadDocLegalizacion <> 0 And id_estadDocLegalizacion <> 7) And intOwnerLegalizacion.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                hlnkLegalizacionSeguimiento.Visible = True


                If idUsuario = idUser And id_estadDocLegalizacion = 6 Then
                    hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                Else
                    'hlnkSeguimiento.NavigateUrl = "frm_viajeSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                    hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacionSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                End If


                'hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacionSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkLegalizacionSeguimiento.ImageUrl = "~/Imagenes/iconos/accept.png"
                hlnkLegalizacionSeguimiento.ToolTip = "Legalización - Continuar con la aprobación"
            ElseIf id_estadDoc = 7 And id_estadDocLegalizacion = 7 Then
                hlnkLegalizacionSeguimiento.ImageUrl = "~/Imagenes/iconos/printer_off.png"
                hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacionPrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlnkLegalizacionSeguimiento.Target = "_blank"
                hlnkLegalizacionSeguimiento.ToolTip = "Legalización - Aprobado / Proceso finalizado"
                hlnkLegalizacionSeguimiento.Visible = True
            ElseIf id_estadDoc = 7 And id_estadDocLegalizacion > 0 Then
                hlnkLegalizacionSeguimiento.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkLegalizacionSeguimiento.ToolTip = "Legalización - En proceso de aprobación"
                hlnkLegalizacionSeguimiento.Visible = True
            End If


            If id_estadDoc = 7 And id_estadoDoc_informe = 0 And idUsuario = idUser Then
                hlk_informe.Visible = True
                hlk_informe.NavigateUrl = "frm_viaje_informe?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlk_informe.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                hlk_informe.ToolTip = "Informe de viaje pendiente"
            ElseIf id_estadDoc = 7 And (id_estadoDoc_informe <> 0 And id_estadoDoc_informe <> 7) And intOwnerInforme.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                hlk_informe.Visible = True

                If idUsuario = idUser And id_estadoDoc_informe = 6 Then
                    'hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                    hlk_informe.NavigateUrl = "frm_viaje_informe?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                Else
                    'hlnkSeguimiento.NavigateUrl = "frm_viajeSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                    hlk_informe.NavigateUrl = "frm_viaje_informeSgmt?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                End If



                hlk_informe.ImageUrl = "~/Imagenes/iconos/accept.png"
                hlk_informe.ToolTip = "Informe de viaje - Continuar con la aprobación"

            ElseIf id_estadDoc = 7 And id_estadoDoc_informe = 7 Then
                hlk_informe.ImageUrl = "~/Imagenes/iconos/printer_off.png"
                hlk_informe.NavigateUrl = "frm_viaje_informePrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                hlk_informe.Target = "_blank"
                hlk_informe.ToolTip = "Informe de viaje - Aprobado / Proceso finalizado"
                hlk_informe.Visible = True
            ElseIf id_estadDoc = 7 And id_estadoDoc_informe > 0 Then
                hlk_informe.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlk_informe.ToolTip = "Informe de viaje - En proceso de aprobación"
                hlk_informe.Visible = True
            End If

            If id_estadDocLegalizacion <> 0 And id_estadDoc > 0 Then
                hlnkEstado.Visible = False
            Else
                If facturaAnulada Then
                    hlnkEdit.Visible = False
                    hlnkSeguimiento.Visible = False
                    hlk_informe.Visible = False
                    hlnkLegalizacionSeguimiento.Visible = False
                    hlnkFacturacion.Visible = False
                    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
                    hlnkEstado.ToolTip = "Viaje anulado"
                    hlnkEstado.Visible = True
                    hlkPermisos.Visible = False
                    hlnkCodigoReserva.Visible = False
                Else

                    hlnkEstado.NavigateUrl = "frm_viajeAnular?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                    hlnkEdit.NavigateUrl = "frm_viajeEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
                End If
            End If

            If id_supervisor = idUser And idDocumento > 0 And id_estadDoc <> 6 And idDocumentoLegalizacion = 0 Then
                hlnkEdit.Visible = True
                hlnkEdit.NavigateUrl = "frm_viajeEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString()
            End If




            Dim editar_solicitud As Boolean = Convert.ToBoolean(itemD("editar_solicitud").Text.ToString())
            If editar_solicitud And idUsuario = idUser Then
                hlnkEdit.Visible = True
                hlnkEdit.NavigateUrl = "frm_viajeEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString() & "&e=1"
            End If

            Dim editar_legalizacion As Boolean = Convert.ToBoolean(itemD("editar_legalizacion").Text.ToString())
            If editar_legalizacion And idUsuario = idUser And id_estadDoc = 7 Then
                hlnkLegalizacionSeguimiento.Visible = True
                hlnkLegalizacionSeguimiento.NavigateUrl = "frm_viaje_legalizacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString() & "&e=1"
                hlnkLegalizacionSeguimiento.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            End If

            Dim editar_informe As Boolean = Convert.ToBoolean(itemD("editar_informe").Text.ToString())
            If editar_informe And idUsuario = idUser And id_estadDoc = 7 Then
                hlk_informe.Visible = True
                hlk_informe.NavigateUrl = "frm_viaje_informe?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString() & "&e=1"
                hlk_informe.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            End If

            Dim habilitar_facturacion As Boolean = Convert.ToBoolean(itemD("habilitar_facturacion").Text.ToString())
            If habilitar_facturacion And id_usuario_radica.Where(Function(p) p.Contains(idUser)).Count() > 0 And id_estadDoc = 7 Then
                hlnkFacturacion.Visible = True
                hlnkFacturacion.Visible = True
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.NavigateUrl = "frm_viaje_facturacion?Id=" & DataBinder.Eval(e.Item.DataItem, "id_viaje").ToString() & "&e=1"
            ElseIf habilitar_facturacion Then
                hlnkFacturacion.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                hlnkFacturacion.Enabled = False
                hlnkFacturacion.ToolTip = "Pendiente de facturación"
            End If

            If servicioAereo = 0 Then
                hlnkCodigoReserva.Visible = False
            End If

        End If


    End Sub

    Protected Sub Editar_codigo_reserva_Click(sender As Object, e As EventArgs)
        Using dbEntities As New dbRMS_JIEntities
            Dim a = CType(sender, LinkButton)
            Me.identity.Text = a.Attributes.Item("data-identity").ToString()
            Dim idCR = Me.identity.Text
            Me.txt_Codigo_reserva.Text = dbEntities.tme_solicitud_viaje.Find(Convert.ToInt32(idCR)).numero_reserva
            Me.idviaje.Value = idCR
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadCodigoReservaModal()", True)
        End Using
    End Sub

    Protected Sub btn_edit2_Click(sender As Object, e As EventArgs) Handles btn_edit2.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim oViaje = dbEntities.tme_solicitud_viaje.Find(Convert.ToInt32(Me.idviaje.Value))
            oViaje.numero_reserva = Me.txt_Codigo_reserva.Text
            dbEntities.Entry(oViaje).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using


    End Sub
End Class