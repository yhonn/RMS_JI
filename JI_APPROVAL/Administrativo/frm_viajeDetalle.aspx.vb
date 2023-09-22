Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL

Public Class frm_viajeDetalle
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
            PathArchivos = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
            If Not Me.IsPostBack Then
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                Session.Remove("dtConceptos")
                Session.Remove("dtItinerario")
                Session.Remove("dtAlojamiento")
                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                LoadData(id_viaje)

                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                loadDataItinerario(viaje)
                loadDataAlojamiento(viaje)
                fillGridItinerario()
                fillGridAlojamiento()
                fillGridLegalizacion(id_viaje)
                fillGridArchivos(id_viaje)

            End If
        End Using

    End Sub
    Sub fillGridArchivos(ByVal id_viaje As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim archivos = dbEntities.vw_tme_solicitud_viaje_legalizacion_soportes.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_soportes.DataSource = archivos
            Me.grd_soportes.DataBind()
        End Using
    End Sub
    Sub fillGridLegalizacion(ByVal id_viaje As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim legalizadionDetalle = dbEntities.vw_tme_solicitud_viaje_legalizacion.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_general.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 1).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_general.DataBind()

            Me.grd_reuniones.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 2).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_reuniones.DataBind()

            Me.grd_miscelaneos.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 3).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_miscelaneos.DataBind()

            Me.grd_alimentacion_alojamiento.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 4).OrderBy(Function(p) p.fecha).ThenBy(Function(p) p.porcentaje_perdiem).ToList()
            Me.grd_alimentacion_alojamiento.DataBind()
        End Using

    End Sub
    Protected Sub grd_soportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_soportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()

            Dim hlnkSoporte As HyperLink = New HyperLink
            hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)
            hlnkSoporte.NavigateUrl = PathArchivos & soporte

        End If
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
    Sub fillGridItinerario()
        If Session("dtItinerario") IsNot Nothing Then
            dtItinerario = Session("dtItinerario")
        Else
            createdtcolums()
        End If


        Me.grd_itinerario.DataSource = dtItinerario
        Me.grd_itinerario.DataBind()
    End Sub
    Sub loadDataItinerario(ByVal viaje As tme_solicitud_viaje)
        If Session("dtItinerario") IsNot Nothing Then
            dtItinerario = Session("dtItinerario")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities

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
                                  If(item.requiere_linea_aerea = False, "NO", "SÍ"), If(item.requiere_vehiculo_proyecto = False, "NO", "SI"),
                                  If(item.requiere_transporte_fluvial = False, "NO", "SÍ"), If(item.requiere_servicio_publico = False, "NO", "SÍ"), item.linea_aerea,
                                    item.observaciones_vehiculo_proyecto, item.observaciones_transporte_fluvial, item.observaciones_servicio_publico, True, item.id_viaje_itinerario)
            Next
            Session("dtItinerario") = dtItinerario
        End Using
    End Sub
    Sub loadDataAlojamiento(ByVal viaje As tme_solicitud_viaje)
        If Session("dtAlojamiento") IsNot Nothing Then
            dtAlojamiento = Session("dtAlojamiento")
        Else
            createdtcolumsAlojamiento()
        End If
        Using dbEntities As New dbRMS_JIEntities

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

            Session("dtAlojamiento") = dtAlojamiento
        End Using
    End Sub
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
    End Sub
    Sub fillGridRutaAprobacionLegalizacion(ByVal id_documento As Integer)
        Me.grd_cate2.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate2.DataBind()
    End Sub
    Sub fillGridRutaAprobacionInforme(ByVal id_documento As Integer)
        Me.grd_cate3.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate3.DataBind()
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"

            hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
            If itemD("descripcion_estado").Text = "Pending" Then
                For i As Integer = 2 To 8
                    e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
                Next
            End If

        End If
    End Sub
    Protected Sub grd_cate3_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate3.ItemDataBound
        'If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

        '    Dim hlnk As HyperLink = New HyperLink
        '    Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

        '    hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
        '    hlnk.ToolTip = "Alert"

        '    hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
        '    If itemD("descripcion_estado").Text = "Pending" Then
        '        For i As Integer = 2 To 8
        '            e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
        '        Next
        '    End If

        'End If
    End Sub
    'Protected Sub grd_cate2_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate2.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim hlnk As HyperLink = New HyperLink
    '        Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

    '        hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
    '        'hlnk.ToolTip = "Alert"

    '        hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
    '        If itemD("descripcion_estado").Text = "Pending" Then
    '            For i As Integer = 2 To 8
    '                e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
    '            Next
    '        End If

    '    End If
    'End Sub
    Sub LoadData(ByVal id_viaje As Integer)
        Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
        Using dbEntities As New dbRMS_JIEntities
            Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()
            'Dim legalizacion = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()

            Me.lbl_usuario.Text = viaje.nombre_usuario
            Me.lbl_numero_documento.Text = viaje.numero_documento
            Me.lbl_cargo.Text = viaje.cargo
            Me.lbl_codigo_usuario.Text = viaje.codigo_usuario
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            Me.lbl_fecha_indicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            Me.lbl_fecha_fin_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            Me.lbl_motivo.Text = viaje.motivo_viaje

            Me.lblt_resultados.Text = viaje.informe_resultado
            Me.lblt_compromisos.Text = viaje.informe_compromiso


            fillGridRutaAprobacion(viaje.id_documento)
            fillGridRutaAprobacionLegalizacion(viaje.id_documento_legalizacion)
            fillGridRutaAprobacionInforme(viaje.id_documento_informe)
        End Using
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
End Class