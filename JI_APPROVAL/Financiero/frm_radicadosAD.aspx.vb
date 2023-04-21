Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Public Class frm_radicadosAD
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_RADICADO_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
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
        End If

        If Not Me.IsPostBack Then
            LoadLista()
        End If
    End Sub
    Sub LoadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities


            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)

            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy hh:mm}", DateTime.Now)

            Dim regionesUsuario = usuario.t_usuario_subregion.ToList()
            Me.cmb_sub_Region.DataSourceID = ""
            If regionesUsuario.Count() = 1 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                            New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                      Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.cmb_sub_Region.SelectedValue = subRegion.FirstOrDefault().id_subregion
            ElseIf regionesUsuario.Count() > 0 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            Else
                Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.t_regiones.id_programa = id).Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_regiones.nombre_region & " - " & p.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            End If


            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()

            Me.cmb_caracter.DataSource = dbEntities.tme_caracter_radicados.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_caracter.DataTextField = "caracter"
            Me.cmb_caracter.DataValueField = "id_caracter"
            Me.cmb_caracter.DataBind()

            Me.cmb_tipo_documento.DataSource = dbEntities.tme_tipo_documento_radicados.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_tipo_documento.DataTextField = "tipo_documento"
            Me.cmb_tipo_documento.DataValueField = "id_tipo_documento"
            Me.cmb_tipo_documento.DataBind()
        End Using
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim cierre = dbEntities.tme_fecha_cierre.OrderByDescending(Function(p) p.fecha_cierre).ToList().FirstOrDefault()
            Dim oRadicado As tme_radicados = New tme_radicados
            oRadicado.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oRadicado.fecha_crea = DateTime.Now
            oRadicado.fecha_radicado = DateTime.Now
            oRadicado.fuera_tiempo = False

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim usuario = dbEntities.t_usuarios_permisos.Where(Function(p) p.id_programa = idPrograma And p.id_usuario = idUsuario).FirstOrDefault()
            Dim permisos = dbEntities.tme_radicados_permisos.Where(Function(p) p.id_programa = idPrograma).FirstOrDefault()

            Dim fechaRegistro = DateTime.Now
            Dim fechaHabil = New Date(fechaRegistro.Year, fechaRegistro.Month, fechaRegistro.Day, 0, 0, 0)
            'Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
            '                                      "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            '''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("habilitar_agregar_viaje")
            'dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitarViaje = False
            If usuario IsNot Nothing Then
                If usuario.registro_radicados_post_cierre And fechaHabil <= permisos.fecha_cierre_final Then
                    habilitarViaje = usuario.registro_radicados_post_cierre
                End If
            End If



            Dim fechaRadicado = DateTime.Now
            If DateTime.Now.Hour >= 18 Then
                fechaRadicado = New Date(fechaRadicado.Year, fechaRadicado.Month, fechaRadicado.Day + 1, 8, 0, 0, 0)
                oRadicado.fecha_radicado = fechaRadicado
            End If
            Dim fecha = DateTime.Now

            If cierre.fecha_cierre.Year = oRadicado.fecha_radicado.Value.Year And cierre.fecha_cierre.Month >= oRadicado.fecha_radicado.Value.Month And cierre.fecha_cierre.Day < oRadicado.fecha_radicado.Value.Day Then
                'oRadicado.fuera_tiempo = True
                If habilitarViaje = True Then
                    oRadicado.fuera_tiempo = True

                    'Dim usuario = dbEntities.t_usuarios.Find(oRadicado.id_usuario_crea)
                    'usuario.habilitar_agregar_viaje = False
                    'dbEntities.Entry(usuario).State = Entity.EntityState.Modified

                Else
                    fecha = New Date(fecha.Year, fecha.Month + 1, 1)
                    fechaRadicado = New Date(fecha.Year, fecha.Month, fecha.Day, 8, 0, 0, 0)
                    oRadicado.fecha_radicado = fechaRadicado
                End If
            End If




            oRadicado.id_subregion = Me.cmb_sub_Region.SelectedValue
            oRadicado.id_caracter = Me.cmb_caracter.SelectedValue
            oRadicado.valor_factura = Me.txt_valor_factura.Value
            oRadicado.id_estado = 1
            oRadicado.identificacion_tercero_a_pagar = Me.txt_identificacion_tercero_a_pagar_.Value
            oRadicado.tercero_a_pagar = Me.txt_tercero_a_pagar.Text.ToUpper()
            oRadicado.observaciones = Me.txt_observaciones.Text.ToUpper()
            oRadicado.detalle = Me.txt_documento.Text.ToUpper()
            oRadicado.id_tipo_documento = Me.cmb_tipo_documento.SelectedValue
            Dim consecutivo = dbEntities.tme_radicados.Where(Function(p) p.fecha_radicado.Value.Year = fecha.Year And p.fecha_radicado.Value.Month = fecha.Month).Count()
            oRadicado.codigo = "JI-" & If(fecha.Month < 10, "0" & fecha.Month, fecha.Month) & "-" & If(consecutivo + 1 < 10, "0" & consecutivo + 1, consecutivo + 1)

            dbEntities.tme_radicados.Add(oRadicado)
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/financiero/frm_radicados"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using
    End Sub

    Private Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/financiero/frm_radicados")
    End Sub
End Class