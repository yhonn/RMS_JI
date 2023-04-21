Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL

Public Class frm_viajeAnular
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_ANULAR"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable

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
            Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idViaje.Value = id_viaje

            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
            LoadLista()
            LoadData(id_viaje)
            fillGrid()
            fillGridAlojamiento()
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
            Me.dt_fecha_inicio.SelectedDate = viaje.fecha_inicio_viaje
            Me.dt_fecha_fin.SelectedDate = viaje.fecha_fin_viaje
            Me.txt_motivo_viaje.Text = viaje.motivo_viaje
            Me.txt_numero_contacto.Text = viaje.numero_contacto

            If viaje.id_tipo_viaje IsNot Nothing Then
                Me.rbn_tipo_viaje.SelectedValue = viaje.id_tipo_viaje
            End If

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
        End Using
    End Sub

    Sub LoadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Me.rbn_tipo_viaje.DataSource = dbEntities.tme_tipo_viaje.ToList()
            Me.rbn_tipo_viaje.DataValueField = "id_tipo_viaje"
            Me.rbn_tipo_viaje.DataTextField = "tipo_viaje"
            Me.rbn_tipo_viaje.DataBind()
        End Using
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/administrativo/frm_viajes")
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
                viaje.id_usuario_anula = Me.Session("E_IdUser").ToString()
                viaje.fecha_anula = DateTime.Now
                viaje.anulado = True
                viaje.motivo_anula = Me.txt_motivo_anular.Text
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                If viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                    Dim idDoc = viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    Dim documento = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                    documento.id_estadoDoc = 4
                    documento.datecreated = DateTime.Now
                    documento.observacion = Me.txt_motivo_anular.Text
                    documento.id_usuario_app = viaje.id_usuario_anula

                    dbEntities.Entry(documento).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                End If

                If viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                    Dim idDoc = viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    Dim documento = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                    documento.id_estadoDoc = 4
                    documento.datecreated = DateTime.Now
                    documento.observacion = Me.txt_motivo_anular.Text
                    documento.id_usuario_app = viaje.id_usuario_anula
                    dbEntities.Entry(documento).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                End If

                If viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                    Dim idDoc = viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    Dim documento = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                    documento.id_estadoDoc = 4
                    documento.datecreated = DateTime.Now
                    documento.observacion = Me.txt_motivo_anular.Text
                    documento.id_usuario_app = viaje.id_usuario_anula
                    dbEntities.Entry(documento).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                End If

                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 0, 1020, cl_user.regionalizacionCulture, 0)
                If (objEmail.Emailing_APPROVAL_TRAVEL(CType(0, Integer), id_viaje, True, 4)) Then
                Else 'Error mandando Email
                End If
            End Using





            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
End Class