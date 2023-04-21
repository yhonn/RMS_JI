Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL

Public Class frm_viajePrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJE_PRINT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Dim PathArchivos = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dbEntities As New dbRMS_JIEntities
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
                    'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                End If
                controles.code_mod = frmCODE
                For Each Control As Control In Page.Controls
                    controles.checkControls(Control, cl_user.id_idioma, cl_user)
                Next
            End If
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            PathArchivos = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder
            If Not Me.IsPostBack Then
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                Session.Remove("dtConceptos")
                Session.Remove("dtItinerario")
                Session.Remove("dtAlojamiento")
                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                'LoadData(id_viaje)
                loadData(id_viaje)

                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using
    End Sub
    Sub loadData(ByVal id_viaje As Integer)
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
            Me.lbl_nombres.Text = viaje.t_usuarios.nombre_usuario & " " & viaje.t_usuarios.apellidos_usuario
            Me.lbl_numero_documento.Text = viaje.t_usuarios.numero_documento
            Me.lbl_cargo.Text = viaje.t_job_title.job
            Me.lbl_fecha_solicitud.Text = viaje.fecha_crea
            Me.lbl_motivo.Text = viaje.motivo_viaje
            Me.lbl_regional.Text = viaje.t_subregiones.t_regiones.nombre_region & " - " & viaje.t_subregiones.nombre_subregion
            Me.lbl_codigo_viaje.Text = viaje.codigo_solicitud_viaje
            Dim parComponentes = dbEntities.tme_solicitud_viaje_marco_logico.Where(Function(p) p.id_viaje = id_viaje).Select(Function(p) New With {Key .sub_objetivo = p.tme_estructura_marcologico.tme_estructura_marcologico2.codigo & " " & p.tme_estructura_marcologico.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                      .objetivo = p.tme_estructura_marcologico.tme_estructura_marcologico2.tme_estructura_marcologico2.codigo & " " & p.tme_estructura_marcologico.tme_estructura_marcologico2.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                      .componente = p.tme_estructura_marcologico.codigo & " " & p.tme_estructura_marcologico.descripcion_logica,
                                                                                                                      .id_viaje_estructura = p.id_viaje_estructura}).ToList()
            Me.RadGrid1.DataSource = parComponentes.ToList()
            Me.RadGrid1.DataBind()

            'Dim itinerario = viaje.tme_solicitud_viaje_itinerario.Select(Function(p) _
            '                                                             New With {Key .fecha_viaje = p.fecha_viaje,
            '                                                                        Key .hora_salida = p.hora_salida,
            '                                                                        Key .ciudad_origen = p.t_municipios1.t_departamentos.nombre_departamento & " - " & p.t_municipios1.nombre_municipio,
            '                                                                        Key .ciudad_destino = p.t_municipios.t_departamentos.nombre_departamento & " - " & p.t_municipios.nombre_municipio
            '}).ToList()

            For Each item In viaje.tme_solicitud_viaje_itinerario.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = "11"

                dtItinerario.Rows.Add(idunique, item.fecha_viaje, item.hora_salida,
                                  item.t_municipios1.t_departamentos.nombre_departamento & " - " & item.t_municipios1.nombre_municipio,
                                  item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                  item.id_municipio_origen, item.id_municipio_destino,
                                  item.requiere_linea_aerea, item.requiere_vehiculo_proyecto,
                                  item.requiere_transporte_fluvial, item.requiere_servicio_publico,
                                  If(item.requiere_linea_aerea = False, "NO", "SÍ") & " - " & item.linea_aerea, If(item.requiere_vehiculo_proyecto = False, "NO", "SI") & " - " & item.observaciones_vehiculo_proyecto,
                                  If(item.requiere_transporte_fluvial = False, "NO", "SÍ") & " - " & item.observaciones_transporte_fluvial, If(item.requiere_servicio_publico = False, "NO", "SÍ") & " - " & item.observaciones_servicio_publico,
                                  item.linea_aerea, item.observaciones_vehiculo_proyecto, item.observaciones_transporte_fluvial, item.observaciones_servicio_publico, True, item.id_viaje_itinerario)
            Next
            Me.grd_itinerario.DataSource = dtItinerario
            Me.grd_itinerario.DataBind()


            For Each item In viaje.tme_solicitud_viaje_hotel.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = "11"

                dtAlojamiento.Rows.Add(idunique, item.fecha_llegada, item.fecha_salida, item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                   item.id_municipio, item.hotel, True, item.id_viaje_hotel)

            Next

            Me.grd_hotel.DataSource = dtAlojamiento
            Me.grd_hotel.DataBind()

            Dim viajeDetail = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()


            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(viajeDetail.id_documento.ToString())
            Me.grd_cate.DataBind()
        End Using
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
End Class