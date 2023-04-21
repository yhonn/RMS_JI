Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing

Public Class frm_viaje_facturacion
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_FACT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
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
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idViaje.Value = id_viaje
            Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
            Me.esEdicion.Value = es_Edicion


            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
            LoadData(id_viaje)
            LoadListas()
            fillGrid()
            fillHotel()
            fillVuelo()
            fillGridAlojamiento()
            fillAlquiler()
        End If
    End Sub
    Sub fillVuelo()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Dim vuelo = viaje.tme_solicitud_viaje_facturacion_tiquete.Select(Function(p) _
                                            New With {Key .id_solicitud_viaje_facturacion_tiquete = p.id_solicitud_viaje_facturacion_tiquete,
                                                      Key .fecha_vuelo = p.fecha_vuelo,
                                                      Key .aerolinea = p.aerolinea,
                                                      Key .ciudad_origen = p.t_municipios.t_departamentos.nombre_departamento & " - " & p.t_municipios.nombre_municipio,
                                                      Key .ciudad_destino = p.t_municipios1.t_departamentos.nombre_departamento & " - " & p.t_municipios1.nombre_municipio
                                                      }).ToList()

            Me.grd_vuelos.DataSource = vuelo.ToList()
            Me.grd_vuelos.DataBind()
        End Using
    End Sub


    Sub fillHotel()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Dim hotel = viaje.tme_solicitud_viaje_facturacion_hotel.Select(Function(p) _
                                            New With {Key .id_solicitud_viaje_facturacion_hotel = p.id_solicitud_viaje_facturacion_hotel,
                                                      Key .fecha_desde = p.fecha_desde,
                                                      Key .fecha_hasta = p.fecha_hasta,
                                                      Key .ciudad_origen = p.t_municipios.t_departamentos.nombre_departamento & " - " & p.t_municipios.nombre_municipio,
                                                      Key .hotel_alojamiento = p.hotel_alojamiento,
                                                      Key .valor_total = p.valor_total,
                                                      Key .numero_factura = p.numero_factura,
                                                      Key .fecha_radicacion = p.fecha_radicacion
                                                      }).ToList()

            Me.grd_hotel.DataSource = hotel.ToList()
            Me.grd_hotel.DataBind()
        End Using
    End Sub
    Sub LoadListas()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_departamento.DataSourceID = ""
            Me.cmb_departamento.DataSource = departamentos
            Me.cmb_departamento.DataTextField = "nombre_departamento"
            Me.cmb_departamento.DataValueField = "id_departamento"
            Me.cmb_departamento.DataBind()

            Dim id_departamento = departamentos.FirstOrDefault().id_departamento
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = id_departamento).ToList()
            Me.cmb_municipio.DataSourceID = ""
            Me.cmb_municipio.DataSource = municipios
            Me.cmb_municipio.DataTextField = "nombre_municipio"
            Me.cmb_municipio.DataValueField = "id_municipio"
            Me.cmb_municipio.DataBind()

            Me.cmb_departamento_hotel.DataSourceID = ""
            Me.cmb_departamento_hotel.DataSource = departamentos
            Me.cmb_departamento_hotel.DataTextField = "nombre_departamento"
            Me.cmb_departamento_hotel.DataValueField = "id_departamento"
            Me.cmb_departamento_hotel.DataBind()

            Me.cmb_municipio_hotel.DataSourceID = ""
            Me.cmb_municipio_hotel.DataSource = municipios
            Me.cmb_municipio_hotel.DataTextField = "nombre_municipio"
            Me.cmb_municipio_hotel.DataValueField = "id_municipio"
            Me.cmb_municipio_hotel.DataBind()

            Me.cmb_departamento_destino.DataSourceID = ""
            Me.cmb_departamento_destino.DataSource = departamentos
            Me.cmb_departamento_destino.DataTextField = "nombre_departamento"
            Me.cmb_departamento_destino.DataValueField = "id_departamento"
            Me.cmb_departamento_destino.DataBind()

            Me.cmb_municipio_destino.DataSourceID = ""
            Me.cmb_municipio_destino.DataSource = municipios
            Me.cmb_municipio_destino.DataTextField = "nombre_municipio"
            Me.cmb_municipio_destino.DataValueField = "id_municipio"
            Me.cmb_municipio_destino.DataBind()

        End Using
    End Sub
    'Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim hlnk As HyperLink = New HyperLink
    '        Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

    '        hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
    '        hlnk.ToolTip = "Alert"

    '        hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
    '        If itemD("descripcion_estado").Text = "Pending" Then
    '            For i As Integer = 2 To 8
    '                e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
    '            Next
    '        End If

    '    End If
    'End Sub
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        'Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        'Me.grd_cate.DataBind()
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
            Dim viajeDetalle = dbEntities.vw_tme_solicitud_viaje.FirstOrDefault(Function(p) p.id_viaje = viaje.id_viaje)
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            Me.lbl_fecha_inicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            Me.lbl_fecha_finalizacion.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            Me.lbl_numero_contacto.Text = viaje.numero_contacto
            Me.lbl_motivo_viaje.Text = viaje.motivo_viaje


            Dim id_usuario_app = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault().id_usuario_app

            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.dt_fecha_radicacion_alquiler.SelectedDate = viaje.fecha_radicacion_alquiler
            Me.txt_factura_alquiler.Text = viaje.factura_alquiler
            Me.txt_valor_factura_alquiler.Value = viaje.valor_factura_alquiler

            Me.txt_valor_factura_tiquetes.Value = viaje.valor_factura_tiquetes
            Me.dt_fecha_radicacion.SelectedDate = viaje.fecha_radicacion_tiquete
            Me.txt_factura_tiquete.Text = viaje.factura_tiquete

            Me.txt_observaciones.Text = viaje.observaciones_facturacion
            'If id_usuario_app <> indx Then '--The User is not Allowed
            '    Me.Response.Redirect("~/Proyectos/no_access2_app")
            'End If


            'Me.lbl_categoria.Text = viaje.ta_documento_viaje.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            'Me.lbl_codigo.Text = viaje.codigo_solicitud_viaje
            'Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            'Me.lbl_fecha_inicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            'Me.lbl_fecha_finalizacion.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            'Me.lbl_numero_contacto.Text = viaje.numero_contacto
            'Me.lbl_motivo_viaje.Text = viaje.motivo_viaje


            For Each item In viaje.tme_solicitud_viaje_itinerario.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_viaje_itinerario

                dtItinerario.Rows.Add(item.id_viaje_itinerario, item.fecha_viaje, item.hora_salida,
                                  item.t_municipios1.t_departamentos.nombre_departamento & " - " & item.t_municipios1.nombre_municipio,
                                  item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                  item.id_municipio_origen, item.id_municipio_destino,
                                  item.requiere_linea_aerea, item.requiere_vehiculo_proyecto,
                                  item.requiere_transporte_fluvial, item.requiere_servicio_publico,
                                  If(item.requiere_linea_aerea = False, "NO", "SÍ - " & item.linea_aerea), If(item.requiere_vehiculo_proyecto = False, "NO", "SI - " & item.observaciones_vehiculo_proyecto),
                                  If(item.requiere_transporte_fluvial = False, "NO", "SÍ - " & item.observaciones_transporte_fluvial), If(item.requiere_servicio_publico = False, "NO", "SÍ - " & item.observaciones_servicio_publico), item.linea_aerea,
                                    item.observaciones_vehiculo_proyecto, item.observaciones_transporte_fluvial, item.observaciones_servicio_publico, True,
                                    item.id_viaje_itinerario, item.valor_total_aereo, item.numero_factura_aereo, item.valor_total_alquiler, item.numero_factura_alquiler,
                                    item.valor_total, item.soportes, item.fecha_radicacion_aereo, item.fecha_radicacion_alquiler)
            Next

            For Each item In viaje.tme_solicitud_viaje_hotel.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_viaje_hotel
                Dim hotelAlojamiento = item.hotel

                If item.hotel_alojamiento IsNot Nothing Then
                    hotelAlojamiento = item.hotel_alojamiento
                End If

                dtAlojamiento.Rows.Add(item.id_viaje_hotel, item.fecha_llegada, item.fecha_salida, item.t_municipios.t_departamentos.nombre_departamento & " - " & item.t_municipios.nombre_municipio,
                                   item.id_municipio, item.hotel, True, item.id_viaje_hotel, item.valor_total, item.numero_factura, item.nro_dias, hotelAlojamiento, item.fecha_radicacion)

            Next
            Session("dtItinerario") = dtItinerario
            Session("dtAlojamiento") = dtAlojamiento
            If viaje.ta_documento_viaje.Count() > 0 Then
                Me.HiddenField1.Value = viaje.ta_documento_viaje.FirstOrDefault().id_documento
                fillGridRutaAprobacion(viaje.ta_documento_viaje.FirstOrDefault().id_documento)
            End If

            Dim intOwnerFacturacion As String() = viajeDetalle.id_usuario_radica.ToString.Split(",")
            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)


            If intOwnerFacturacion.Where(Function(p) p.Contains(idUser)).Count() > 0 And es_Edicion = 1 Then
                Me.btn_guardar_finalizar.Visible = False
            End If


            'Dim tbl_AppOrden As New DataTable
            'tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc

            'If tbl_AppOrden.Rows(0)("id_ruta_next").ToString <> "-1" Then
            '    btn_Completed.Visible = False
            '    btn_Approved.Visible = True
            'Else
            '    btn_Completed.Visible = True
            '    btn_Approved.Visible = False
            'End If
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
            dtItinerario.Columns.Add("id_viaje_itinerario", GetType(Integer))
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
            dtItinerario.Columns.Add("valor_total_aereo", GetType(Decimal))
            dtItinerario.Columns.Add("numero_factura_aereo", GetType(String))
            dtItinerario.Columns.Add("valor_total_alquiler", GetType(Decimal))
            dtItinerario.Columns.Add("numero_factura_alquiler", GetType(String))
            dtItinerario.Columns.Add("valor_total", GetType(Decimal))
            dtItinerario.Columns.Add("soportes", GetType(String))
            dtItinerario.Columns.Add("fecha_radicacion_aereo", GetType(Date))
            dtItinerario.Columns.Add("fecha_radicacion_alquiler", GetType(Date))
        End If
    End Sub

    Sub createdtcolumsAlojamiento()
        If dtAlojamiento.Columns.Count = 0 Then
            dtAlojamiento.Columns.Add("id_viaje_hotel", GetType(Integer))
            dtAlojamiento.Columns.Add("fecha_llegada", GetType(Date))
            dtAlojamiento.Columns.Add("fecha_salida", GetType(Date))
            dtAlojamiento.Columns.Add("ciudad", GetType(String))
            dtAlojamiento.Columns.Add("id_municipio", GetType(Integer))
            dtAlojamiento.Columns.Add("hotel", GetType(String))
            dtAlojamiento.Columns.Add("esta_bd", GetType(Boolean))
            dtAlojamiento.Columns.Add("id_viaje_hotel_bd", GetType(Integer))
            dtAlojamiento.Columns.Add("valor_total", GetType(Decimal))
            dtAlojamiento.Columns.Add("numero_factura", GetType(String))
            dtAlojamiento.Columns.Add("numero_noches", GetType(Integer))
            dtAlojamiento.Columns.Add("hotel_alojamiento", GetType(String))
            dtAlojamiento.Columns.Add("fecha_radicacion", GetType(Date))

        End If
    End Sub

    Protected Sub grd_vuelos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_vuelos.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_tiquete").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_tiquete").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_tiquete").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub grd_alojamiento_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_alojamiento.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            'Dim txt_total As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total"), RadNumericTextBox)
            'txt_total.Text = DataBinder.Eval(e.Item.DataItem, "valor_total").ToString()
            'Dim txt_nro_factura As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_nro_factura"), RadTextBox)
            'txt_nro_factura.Text = DataBinder.Eval(e.Item.DataItem, "numero_factura").ToString()

            'Dim txt_hotel_seleccionado As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_hotel_seleccionado"), RadTextBox)
            'txt_hotel_seleccionado.Text = DataBinder.Eval(e.Item.DataItem, "hotel_alojamiento").ToString()

            'Dim dt_fecha_radicacion As RadDatePicker = CType(e.Item.Cells(0).FindControl("dt_fecha_radicacion"), RadDatePicker)
            'If DataBinder.Eval(e.Item.DataItem, "fecha_radicacion").ToString() <> "" Then
            '    dt_fecha_radicacion.SelectedDate = DataBinder.Eval(e.Item.DataItem, "fecha_radicacion").ToString()
            'End If

        End If
    End Sub

    Protected Sub grd_itinerario_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_itinerario.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            'Dim trasnporteAereo = True
            'Dim alquiler = True
            'If DataBinder.Eval(e.Item.DataItem, "requiere_transporte_aereo").ToString() <> "" Then
            '    trasnporteAereo = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "requiere_transporte_aereo").ToString())
            'End If
            'If DataBinder.Eval(e.Item.DataItem, "requiere_vehiculo_proyecto").ToString() <> "" Then
            '    alquiler = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "requiere_vehiculo_proyecto").ToString())
            'End If


            'Dim txt_total_alquiler_vehiculo As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_alquiler_vehiculo"), RadNumericTextBox)
            'txt_total_alquiler_vehiculo.Text = DataBinder.Eval(e.Item.DataItem, "valor_total_alquiler").ToString()
            'Dim txt_nro_factura_alquiler_vehiculo As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_nro_factura_alquiler_vehiculo"), RadTextBox)
            'txt_nro_factura_alquiler_vehiculo.Text = DataBinder.Eval(e.Item.DataItem, "numero_factura_alquiler").ToString()

            'Dim txt_total As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total"), RadNumericTextBox)
            'txt_total.Text = DataBinder.Eval(e.Item.DataItem, "valor_total_aereo").ToString()
            'Dim txt_nro_factura As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_nro_factura"), RadTextBox)
            'txt_nro_factura.Text = DataBinder.Eval(e.Item.DataItem, "numero_factura_aereo").ToString()

            'Dim dt_fecha_factura As RadDatePicker = CType(e.Item.Cells(0).FindControl("dt_fecha_factura"), RadDatePicker)
            'If DataBinder.Eval(e.Item.DataItem, "fecha_radicacion_aereo").ToString() <> "" Then
            '    dt_fecha_factura.SelectedDate = DataBinder.Eval(e.Item.DataItem, "fecha_radicacion_aereo").ToString()
            'End If

            'If trasnporteAereo = False Then
            '    dt_fecha_factura.Visible = False
            '    txt_total.Visible = False
            '    txt_nro_factura.Visible = False
            'End If

            'Dim dt_fecha_alquiler As RadDatePicker = CType(e.Item.Cells(0).FindControl("dt_fecha_alquiler"), RadDatePicker)
            'If DataBinder.Eval(e.Item.DataItem, "fecha_radicacion_alquiler").ToString() <> "" Then
            '    dt_fecha_alquiler.SelectedDate = DataBinder.Eval(e.Item.DataItem, "fecha_radicacion_alquiler").ToString()
            'End If
            'If alquiler = False Then
            '    dt_fecha_alquiler.Visible = False
            '    txt_total_alquiler_vehiculo.Visible = False
            '    txt_nro_factura_alquiler_vehiculo.Visible = False
            'End If
        End If

    End Sub
    'Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
    '    Try
    '        guardar(True)

    '        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '    Catch ex As Exception
    '        Dim mensaje = ex.Message
    '    End Try

    'End Sub
    'Public Function guardarDocumento(ByVal viaje As tme_solicitud_viaje, ByVal usuario As t_usuarios) As Integer
    '    Dim descripcion = String.Format("Solicitud de viaje {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, viaje.fecha_inicio_viaje)
    '    Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
    '    Dim err As Boolean = False
    '    clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

    '    clss_approval.set_ta_documento(0) 'Set new Record
    '    clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", 123, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("numero_instrumento", viaje.codigo_solicitud_viaje, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("descripcion_doc", descripcion, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("nom_beneficiario", usuario.nombre_usuario & " " & usuario.apellidos_usuario, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("comentarios", viaje.motivo_viaje, "id_documento", 0) '.Replace("'", "''")
    '    clss_approval.set_ta_documentoFIELDS("codigo_AID", viaje.codigo_solicitud_viaje, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", viaje.codigo_solicitud_viaje, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("ficha_actividad", "NO", "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("monto_ficha", 0, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("regional", Me.Session("E_SubRegion").ToString.Trim, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("codigo_Approval", viaje.codigo_solicitud_viaje, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("id_tipoAprobacion", 4, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("monto_total", 0, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("tasa_cambio", 0, "id_documento", 0)
    '    clss_approval.set_ta_documentoFIELDS("datecreated", Date.UtcNow, "id_documento", 0)

    '    Dim id_documento = clss_approval.save_ta_documento()

    '    Dim tbl_Route_By_DOC As New DataTable

    '    tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(123, 0) 'First Step
    '    Dim idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")

    '    Dim Duracion As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")

    '    Dim fecha_limit As DateTime = DateAdd(DateInterval.Day, Duracion, Date.UtcNow) 'UTC DATE
    '    Dim fecha_Recep As DateTime = Date.UtcNow 'UTC DATE

    '    Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario)

    '    clss_approval.set_ta_AppDocumento(0) 'New Record
    '    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_Recep, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cOPEN, "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("observacion", descripcion, "id_app_documento", 0) '.Replace("'", "''")
    '    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApprovalTimeSheet.Rows(0).Item("id_rol"), "id_app_documento", 0)
    '    clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

    '    Dim id_appdocumento = clss_approval.save_ta_AppDocumento()
    '    If id_appdocumento <> -1 Then
    '        tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(123, 1) 'Next Step
    '        Dim NextUser As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
    '        Dim idNextRol As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
    '        fecha_Recep = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
    '        idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
    '        fecha_limit = calculaDiaHabil(Duracion, fecha_Recep)
    '        clss_approval.set_ta_AppDocumento(0) 'New Record
    '        clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
    '        clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
    '        clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
    '        clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
    '        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
    '        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", NextUser, "id_app_documento", 0)
    '        clss_approval.set_ta_AppDocumentoFIELDS("observacion", descripcion.Trim, "id_app_documento", 0) 'Pending Step 
    '        clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role --NEW
    '        clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

    '        Dim id_appdocumento2 = clss_approval.save_ta_AppDocumento()

    '        If id_appdocumento2 <> -1 Then

    '        Else
    '            err = True
    '        End If  'app_documento 2
    '    End If

    '    Return id_documento

    'End Function

    Private Function calculaDiaHabil(ByVal nDias As Integer, ByVal fechaPost As Date) As Date
        Dim hoy As Date = Date.UtcNow
        Dim weekend As Integer = 0
        Dim fecha_limite As Date
        Select Case fechaPost.DayOfWeek
            Case DayOfWeek.Sunday
                If nDias < 6 Then
                    weekend = 0
                ElseIf nDias < 11 Then
                    weekend = 2
                ElseIf nDias < 16 Then
                    weekend = 4
                End If
            Case DayOfWeek.Monday
                If nDias < 5 Then
                    weekend = 0
                ElseIf nDias < 10 Then
                    weekend = 2
                ElseIf nDias < 15 Then
                    weekend = 4
                End If
            Case DayOfWeek.Tuesday
                If nDias < 4 Then
                    weekend = 0
                ElseIf nDias < 9 Then
                    weekend = 2
                ElseIf nDias < 14 Then
                    weekend = 4
                End If
            Case DayOfWeek.Wednesday
                If nDias < 3 Then
                    weekend = 0
                ElseIf nDias < 8 Then
                    weekend = 2
                ElseIf nDias < 13 Then
                    weekend = 4
                End If
            Case DayOfWeek.Thursday
                If nDias < 2 Then
                    weekend = 0
                ElseIf nDias < 7 Then
                    weekend = 2
                ElseIf nDias < 12 Then
                    weekend = 4
                End If
            Case DayOfWeek.Friday
                If nDias < 1 Then
                    weekend = 0
                ElseIf nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 11 Then
                    weekend = 4
                End If
            Case DayOfWeek.Saturday
                If nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 10 Then
                    weekend = 4
                End If
        End Select
        Dim totaldias = weekend + nDias
        fecha_limite = DateAdd(DateInterval.DayOfYear, totaldias, fechaPost)
        Return fecha_limite
    End Function
    'Private Sub btn_guardar_2_Click(sender As Object, e As EventArgs) Handles btn_guardar_2.Click
    '    Try
    '        guardar(False)
    '        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '    Catch ex As Exception
    '        Dim mensaje = ex.Message
    '    End Try

    'End Sub
    Sub guardar(ByVal enviarAprobacion As Boolean)
        Try
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
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            End Using

        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    Protected Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/administrativo/frm_viajes")
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                guardarFacturacion()

                Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)
                If es_Edicion = 1 Then
                    Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                    Dim viajePermisos = dbEntities.tme_solicitud_viaje_permisos.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()
                    If viajePermisos IsNot Nothing Then
                        viajePermisos.habilitar_facturacion = False
                        dbEntities.Entry(viajePermisos).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If
                End If

            End Using

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception
            Dim err = ex.Message
        End Try

    End Sub

    Sub guardarFacturacion()
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                viaje.fecha_radicacion_alquiler = Me.dt_fecha_radicacion_alquiler.SelectedDate
                'viaje.factura_alquiler = Me.txt_factura_alquiler.Text
                'viaje.valor_factura_alquiler = Me.txt_valor_factura_alquiler.Value

                viaje.valor_factura_tiquetes = Me.txt_valor_factura_tiquetes.Value
                viaje.fecha_radicacion_tiquete = Me.dt_fecha_radicacion.SelectedDate
                viaje.factura_tiquete = Me.txt_factura_tiquete.Text
                viaje.observaciones_facturacion = Me.txt_observaciones.Text
                'For Each file As UploadedFile In soporte_alquiler.UploadedFiles
                '    Dim fecha = DateTime.Now
                '    Dim exten = file.GetExtension()
                '    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                '    Dim Path As String
                '    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                '    file.SaveAs(Path + nombreArchivo)
                '    viaje.soporte_alquiler = nombreArchivo
                'Next
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception
            Dim err = ex.Message
        End Try
    End Sub

    Private Sub btn_guardar_finalizar_Click(sender As Object, e As EventArgs) Handles btn_guardar_finalizar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                guardarFacturacion()
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                viaje.fecha_radicacion = DateTime.Now
                viaje.id_usuario_radico = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim id_departamento = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = id_departamento).ToList()
            Me.cmb_municipio.DataSourceID = ""
            Me.cmb_municipio.DataSource = municipios
            Me.cmb_municipio.DataTextField = "nombre_municipio"
            Me.cmb_municipio.DataValueField = "id_municipio"
            Me.cmb_municipio.DataBind()
        End Using
    End Sub

    Private Sub cmb_departamento_hotel_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_hotel.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim id_departamento = Convert.ToInt32(Me.cmb_departamento_hotel.SelectedValue)
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = id_departamento).ToList()
            Me.cmb_municipio_hotel.DataSourceID = ""
            Me.cmb_municipio_hotel.DataSource = municipios
            Me.cmb_municipio_hotel.DataTextField = "nombre_municipio"
            Me.cmb_municipio_hotel.DataValueField = "id_municipio"
            Me.cmb_municipio_hotel.DataBind()
        End Using
    End Sub

    Private Sub btn_agregar_hotel_Click(sender As Object, e As EventArgs) Handles btn_agregar_hotel.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim id_usuario As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)

                Dim viajeHotel = New tme_solicitud_viaje_facturacion_hotel
                viajeHotel.id_viaje = id_viaje
                viajeHotel.id_municipio = Me.cmb_municipio_hotel.SelectedValue
                viajeHotel.fecha_desde = Me.dt_fecha_llegada.SelectedDate
                viajeHotel.fecha_hasta = Me.dt_fecha_salida.SelectedDate
                viajeHotel.valor_total = Me.txt_valor_total_hotel.Value
                viajeHotel.numero_factura = Me.txt_factura_hotel.Text
                viajeHotel.hotel_alojamiento = Me.txt_hotel.Text
                viajeHotel.fecha_radicacion = Me.dt_fecha_radicacion_hotel.SelectedDate
                viajeHotel.fecha_crea = DateTime.Now
                viajeHotel.id_usuario_crea = id_usuario

                For Each file As UploadedFile In soporte_hotel.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    viajeHotel.soporte = nombreArchivo
                Next


                dbEntities.tme_solicitud_viaje_facturacion_hotel.Add(viajeHotel)
                dbEntities.SaveChanges()
                fillHotel()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_agregar_itinerario_Click(sender As Object, e As EventArgs) Handles btn_agregar_itinerario.Click
        Try

            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim id_usuario As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)

                Dim viajeTiquete = New tme_solicitud_viaje_facturacion_tiquete
                viajeTiquete.id_viaje = id_viaje
                viajeTiquete.id_municipio_origen = Me.cmb_municipio.SelectedValue
                viajeTiquete.id_municipio_destino = Me.cmb_municipio_destino.SelectedValue
                viajeTiquete.fecha_vuelo = Me.dt_fecha_vuelo.SelectedDate
                viajeTiquete.aerolinea = Me.txt_linea_aerea.Text
                viajeTiquete.fecha_crea = DateTime.Now
                viajeTiquete.id_usuario_crea = id_usuario

                For Each file As UploadedFile In soporte_aereo.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    viajeTiquete.soporte = nombreArchivo
                Next


                dbEntities.tme_solicitud_viaje_facturacion_tiquete.Add(viajeTiquete)
                dbEntities.SaveChanges()
                fillVuelo()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmb_departamento_destino_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_destino.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim id_departamento = Convert.ToInt32(Me.cmb_departamento_destino.SelectedValue)
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = id_departamento).ToList()
            Me.cmb_municipio_destino.DataSourceID = ""
            Me.cmb_municipio_destino.DataSource = municipios
            Me.cmb_municipio_destino.DataTextField = "nombre_municipio"
            Me.cmb_municipio_destino.DataValueField = "id_municipio"
            Me.cmb_municipio_destino.DataBind()
        End Using
    End Sub

    Protected Sub delete_itinerario(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.tipo.Value = 1
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub delete_hotel(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.tipo.Value = 2
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub grd_hotel_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_hotel.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_hotel").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_hotel").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_facturacion_hotel").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities

            Dim tipo_elemento_eliminar = Convert.ToInt32(Me.tipo.Value)
            If tipo_elemento_eliminar = 1 Then

                Dim identity_ = Convert.ToInt32(Me.identity.Value)

                Dim tiquete = dbEntities.tme_solicitud_viaje_facturacion_tiquete.Find(identity_)
                dbEntities.Entry(tiquete).State = Entity.EntityState.Deleted
                dbEntities.SaveChanges()
                fillVuelo()
            ElseIf tipo_elemento_eliminar = 2 Then
                Dim identity_ = Convert.ToInt32(Me.identity.Value)

                Dim hotel = dbEntities.tme_solicitud_viaje_facturacion_hotel.Find(identity_)
                dbEntities.Entry(hotel).State = Entity.EntityState.Deleted
                dbEntities.SaveChanges()
                fillHotel()
            ElseIf tipo_elemento_eliminar = 3 Then
                Dim identity_ = Convert.ToInt32(Me.identity.Value)

                Dim alquiler = dbEntities.tme_solicitud_viaje_facturacion_vehiculo.Find(identity_)
                dbEntities.Entry(alquiler).State = Entity.EntityState.Deleted
                dbEntities.SaveChanges()
                fillAlquiler()
            End If

            Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Private Sub btn_add_factura_Click(sender As Object, e As EventArgs) Handles btn_add_factura.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim id_usuario As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                Dim facturaAlquiler = New tme_solicitud_viaje_facturacion_vehiculo
                facturaAlquiler.id_viaje = id_viaje
                facturaAlquiler.valor_factura = Me.txt_valor_factura_alquiler.Value
                facturaAlquiler.numero_factura = Me.txt_factura_alquiler.Text
                facturaAlquiler.fecha_servicio = Me.dt_fecha_servicio.SelectedDate
                facturaAlquiler.fecha_crea = DateTime.Now
                facturaAlquiler.observaciones = Me.txt_observaciones_alquiler.Text
                facturaAlquiler.id_usuario_crea = id_usuario

                viaje.fecha_radicacion_alquiler = Me.dt_fecha_radicacion_alquiler.SelectedDate

                For Each file As UploadedFile In soporte_alquiler.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    facturaAlquiler.soporte = nombreArchivo
                Next

                dbEntities.tme_solicitud_viaje_facturacion_vehiculo.Add(facturaAlquiler)
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                fillAlquiler()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Sub fillAlquiler()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Dim alquilerVehiculo = dbEntities.tme_solicitud_viaje_facturacion_vehiculo.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_facturacion_alquiler.DataSource = alquilerVehiculo.ToList()
            Me.grd_facturacion_alquiler.DataBind()
        End Using
    End Sub

    Protected Sub delete_alquiler(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.tipo.Value = 3
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub grd_facturacion_alquiler_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_facturacion_alquiler.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_vehiculo").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_vehiculo").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_solicitud_viaje_vehiculo").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
End Class