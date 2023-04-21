Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing
Imports Telerik.Web.UI.Calendar
Imports System.Globalization
Public Class frm_viaje_permisos
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_PERM"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval

    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_itinerario)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idViaje.Value = id_viaje
            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
            LoadData(id_viaje)
            fillGrid()
            fillGridAlojamiento()
            fillGridHistorial()
        End If
    End Sub
    Sub LoadData(ByVal id_viaje As Integer)
        If Session("dtItinerario") IsNot Nothing Then
            dtItinerario = Session("dtItinerario")
        Else
            createdtcolums()
        End If
        If Session("dtAlojamiento") IsNot Nothing Then
            dtAlojamiento = Session("dtAlojamiento")
        Else
            createdtcolumsAlojamiento()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Dim id_usuario_app = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault().id_usuario_app
            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.lbl_categoria.Text = viaje.ta_documento_viaje.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            Me.lbl_codigo.Text = viaje.codigo_solicitud_viaje
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            Me.lbl_fecha_inicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            Me.lbl_fecha_finalizacion.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            Me.lbl_numero_contacto.Text = viaje.numero_contacto
            Me.lbl_motivo_viaje.Text = viaje.motivo_viaje


            Me.cmb_tipo_permiso.DataSourceID = ""
            Me.cmb_tipo_permiso.DataSource = dbEntities.tme_tipo_permiso_viaje.ToList()
            Me.cmb_tipo_permiso.DataTextField = "permiso"
            Me.cmb_tipo_permiso.DataValueField = "id_tipo_permiso_viaje"
            Me.cmb_tipo_permiso.DataBind()


            For Each item In viaje.tme_solicitud_viaje_itinerario.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_viaje_itinerario

                dtItinerario.Rows.Add(idunique, item.fecha_viaje, item.hora_salida,
                                  item.t_municipios1.t_departamentos.nombre_departamento & " - " & item.t_municipios1.nombre_municipio,
                                  item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                  item.id_municipio_origen, item.id_municipio_destino,
                                  item.requiere_linea_aerea, item.requiere_vehiculo_proyecto,
                                  item.requiere_transporte_fluvial, item.requiere_servicio_publico,
                                  If(item.requiere_linea_aerea = False, "NO", "SÍ"), If(item.requiere_vehiculo_proyecto = False, "NO", "SI"),
                                  If(item.requiere_transporte_fluvial = False, "NO", "SÍ"), If(item.requiere_servicio_publico = False, "NO", "SÍ"), item.linea_aerea,
                                    item.observaciones_vehiculo_proyecto, item.observaciones_transporte_fluvial, item.observaciones_servicio_publico, True, item.id_viaje_itinerario)
            Next

            For Each item In viaje.tme_solicitud_viaje_hotel.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_viaje_hotel

                dtAlojamiento.Rows.Add(idunique, item.fecha_llegada, item.fecha_salida, item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                   item.id_municipio, item.hotel, True, item.id_viaje_hotel)

            Next
            Session("dtItinerario") = dtItinerario
            Session("dtAlojamiento") = dtAlojamiento
            If viaje.ta_documento_viaje.Count() > 0 Then
                Me.HiddenField1.Value = viaje.ta_documento_viaje.FirstOrDefault().id_documento
                'fillGridRutaAprobacion(viaje.ta_documento_viaje.FirstOrDefault().id_documento)
            End If



            If viaje.ta_documento_viaje_informe.Count() > 0 Then
                Me.HiddenField1.Value = viaje.ta_documento_viaje_informe.FirstOrDefault().id_documento
                'fillGridRutaAprobacion(viaje.ta_documento_viaje.FirstOrDefault().id_documento)
            End If


        End Using
    End Sub

    Sub fillGrid()
        If Session("dtItinerario") IsNot Nothing Then
            dtItinerario = Session("dtItinerario")
        Else
            createdtcolums()
        End If
        Me.grd_itinerario.DataSource = dtItinerario
        Me.grd_itinerario.DataBind()
    End Sub

    Sub fillGridHistorial()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Dim viajeHistorial = dbEntities.tme_viaje_historial_permisos.Where(Function(p) p.id_viaje = id_viaje).Select(Function(p) _
                                             New With {Key p.fecha,
                                                       Key p.usuario_solicito,
                                                       Key p.motivo,
                                                       Key p.id_viaje_historial_permisos,
                                                       Key p.tme_tipo_permiso_viaje.permiso}).ToList()
            Me.grd_historial_reversiones.DataSource = viajeHistorial
            Me.grd_historial_reversiones.DataBind()
        End Using
    End Sub

    Sub fillGridAlojamiento()
        If Session("dtAlojamiento") IsNot Nothing Then
            dtAlojamiento = Session("dtAlojamiento")
        Else
            createdtcolumsAlojamiento()
        End If
        Me.grd_alojamiento.DataSource = dtAlojamiento
        Me.grd_alojamiento.DataBind()
    End Sub

    Sub createdtcolums()
        If dtItinerario.Columns.Count = 0 Then
            dtItinerario.Columns.Add("id_viaje_itinerario", GetType(String))
            dtItinerario.Columns.Add("fecha_viaje", GetType(Date))
            dtItinerario.Columns.Add("hora_salida", GetType(String))
            dtItinerario.Columns.Add("ciudad_origen", GetType(String))
            dtItinerario.Columns.Add("ciudad_destino", GetType(String))

            dtItinerario.Columns.Add("id_municipio_origen", GetType(Integer))

            dtItinerario.Columns.Add("id_municipio_destino", GetType(Integer))

            dtItinerario.Columns.Add("requiere_transporte_aereo", GetType(Boolean))
            dtItinerario.Columns.Add("requiere_vehiculo_proyecto", GetType(Boolean))
            dtItinerario.Columns.Add("requiere_transporte_fluvial", GetType(Boolean))
            dtItinerario.Columns.Add("requiere_servicio_publico", GetType(Boolean))
            dtItinerario.Columns.Add("requiere_transporte_aereo_text", GetType(String))
            dtItinerario.Columns.Add("requiere_vehiculo_proyecto_text", GetType(String))
            dtItinerario.Columns.Add("requiere_transporte_fluvial_text", GetType(String))
            dtItinerario.Columns.Add("requiere_servicio_publico_text", GetType(String))
            dtItinerario.Columns.Add("transporte_aereo", GetType(String))
            dtItinerario.Columns.Add("vehiculo_proyecto", GetType(String))
            dtItinerario.Columns.Add("transporte_fluvial", GetType(String))
            dtItinerario.Columns.Add("servicio_publico", GetType(String))
            dtItinerario.Columns.Add("esta_bd", GetType(Boolean))

            dtItinerario.Columns.Add("id_viaje_itinerario_bd", GetType(Integer))
        End If
    End Sub

    Sub createdtcolumsAlojamiento()
        If dtAlojamiento.Columns.Count = 0 Then
            dtAlojamiento.Columns.Add("id_viaje_hotel", GetType(String))
            dtAlojamiento.Columns.Add("fecha_llegada", GetType(Date))
            dtAlojamiento.Columns.Add("fecha_salida", GetType(Date))
            dtAlojamiento.Columns.Add("ciudad", GetType(String))
            dtAlojamiento.Columns.Add("id_municipio", GetType(Integer))
            dtAlojamiento.Columns.Add("hotel", GetType(String))
            dtAlojamiento.Columns.Add("esta_bd", GetType(Boolean))
            dtAlojamiento.Columns.Add("id_viaje_hotel_bd", GetType(Integer))
        End If
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)

                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                Dim oPermisosViaje = dbEntities.tme_solicitud_viaje_permisos.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()
                Dim historial = New tme_viaje_historial_permisos
                Dim esnuevo = False
                If oPermisosViaje Is Nothing Then
                    oPermisosViaje = New tme_solicitud_viaje_permisos
                    esnuevo = True
                End If
                Dim tipoPermiso = Convert.ToInt32(Me.cmb_tipo_permiso.SelectedValue)
                If tipoPermiso = 1 Then
                    oPermisosViaje.editar_solicitud = True
                ElseIf tipoPermiso = 2 Then
                    oPermisosViaje.editar_legalizacion = True
                ElseIf tipoPermiso = 3 Then
                    oPermisosViaje.editar_informe = True
                ElseIf tipoPermiso = 4 Then
                    oPermisosViaje.habilitar_facturacion = True
                ElseIf tipoPermiso = 5 Then
                    oPermisosViaje.reiniciar_solicitud = True

                    Dim documento = dbEntities.ta_documento_viaje.Where(Function(p) p.id_viaje = id_viaje And p.reversado Is Nothing).ToList().FirstOrDefault()
                    If documento IsNot Nothing Then
                        documento.reversado = True
                        documento.id_usuario_reverso = Me.Session("E_IdUser").ToString()
                        documento.fecha_reversion = DateTime.Now
                        documento.motivo_reversion = Me.txt_motivo.Text
                        dbEntities.Entry(documento).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()


                        Dim idDoc = viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                        Dim appDocumeto = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                        appDocumeto.id_estadoDoc = 4
                        appDocumeto.datecreated = DateTime.Now
                        appDocumeto.observacion = Me.txt_motivo.Text & " By " & Me.txt_usuario_solicita.Text
                        appDocumeto.id_usuario_app = Convert.ToInt32(Me.Session("E_IDUser"))

                        dbEntities.Entry(appDocumeto).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()


                    End If

                ElseIf tipoPermiso = 6 Then
                    oPermisosViaje.reiniciar_legalizacion = True
                    Dim documento = dbEntities.ta_documento_legalizacion_viaje.Where(Function(p) p.id_viaje = id_viaje And p.reversado Is Nothing).ToList().FirstOrDefault()
                    If documento IsNot Nothing Then
                        documento.reversado = True
                        documento.id_usuario_reverso = Me.Session("E_IdUser").ToString()
                        documento.fecha_reversion = DateTime.Now
                        documento.motivo_reversion = Me.txt_motivo.Text
                        dbEntities.Entry(documento).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()

                        Dim idDoc = viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                        Dim appDocumeto = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                        appDocumeto.id_estadoDoc = 4
                        appDocumeto.datecreated = DateTime.Now
                        appDocumeto.observacion = Me.txt_motivo.Text & " By " & Me.txt_usuario_solicita.Text
                        appDocumeto.id_usuario_app = Convert.ToInt32(Me.Session("E_IDUser"))

                        dbEntities.Entry(appDocumeto).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If

                ElseIf tipoPermiso = 7 Then
                    oPermisosViaje.reiniciar_informe = True

                    Dim documento = dbEntities.ta_documento_viaje_informe.Where(Function(p) p.id_viaje = id_viaje And p.reversado Is Nothing).ToList().FirstOrDefault()
                    If documento IsNot Nothing Then
                        documento.reversado = True
                        documento.id_usuario_reverso = Me.Session("E_IdUser").ToString()
                        documento.fecha_reversion = DateTime.Now
                        documento.motivo_reversion = Me.txt_motivo.Text
                        dbEntities.Entry(documento).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()

                        Dim idDoc = viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                        Dim appDocumeto = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                        appDocumeto.id_estadoDoc = 4
                        appDocumeto.datecreated = DateTime.Now
                        appDocumeto.observacion = Me.txt_motivo.Text & " By " & Me.txt_usuario_solicita.Text
                        appDocumeto.id_usuario_app = Convert.ToInt32(Me.Session("E_IDUser"))

                        dbEntities.Entry(appDocumeto).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If

                End If

                historial.id_tipo_permiso_viaje = tipoPermiso
                historial.id_viaje = id_viaje
                historial.motivo = Me.txt_motivo.Text
                historial.usuario_solicito = Me.txt_usuario_solicita.Text
                historial.fecha = DateTime.Now

                dbEntities.tme_viaje_historial_permisos.Add(historial)

                If esnuevo Then
                    oPermisosViaje.id_viaje = id_viaje
                    dbEntities.tme_solicitud_viaje_permisos.Add(oPermisosViaje)
                Else
                    dbEntities.Entry(oPermisosViaje).State = Entity.EntityState.Modified
                End If

                dbEntities.SaveChanges()

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


            End Using
        Catch ex As Exception
            Dim aa = ex.Message
        End Try

    End Sub
End Class