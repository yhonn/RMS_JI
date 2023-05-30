Imports System.Globalization
Imports ly_APPROVAL
Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports Telerik.Web.UI.Calendar

Public Class frm_viaje_legalizacion
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_LGL"
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
                    cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_itinerario)
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
                clss_approval = New APPROVAL.clss_approval(id_programa)

                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                Me.idViaje.Value = id_viaje
                Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
                Me.esEdicion.Value = es_Edicion


                Session.Remove("dtItinerario")
                Session.Remove("dtAlojamiento")
                LoadData(id_viaje)
                fillGrid()
                fillGridAlojamiento()
                fillGridLegalizacion()
                LoadListas()
                fillGridArchivos()
            End If
        End Using

    End Sub
    Sub fillGridArchivos()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Me.idViaje.Value
            Dim archivos = dbEntities.vw_tme_solicitud_viaje_legalizacion_soportes.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_soportes.DataSource = archivos
            Me.grd_soportes.DataBind()
        End Using
    End Sub
    Protected Sub grd_soportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_soportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()

            Dim hlnkSoporte As HyperLink = New HyperLink
            hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)
            hlnkSoporte.NavigateUrl = PathArchivos & soporte


            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_soporte_legalizacion_viaje").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_soporte_legalizacion_viaje").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")



        End If
    End Sub
    Sub fillGridLegalizacion()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_viaje = Me.idViaje.Value
            Dim legalizadionDetalle = dbEntities.vw_tme_solicitud_viaje_legalizacion.Where(Function(p) p.id_viaje = id_viaje).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_general.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 1).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_general.DataBind()

            Me.grd_reuniones.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 2).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_reuniones.DataBind()

            Me.grd_miscelaneos.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 3).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_miscelaneos.DataBind()

            Me.grd_alimentacion_alojamiento.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 4).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_alimentacion_alojamiento.DataBind()
        End Using

    End Sub

    Protected Sub grd_general_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_general.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_legalizacion_viaje_detalle = DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub grd_reuniones_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_reuniones.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_legalizacion_viaje_detalle = DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub grd_miscelaneos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_miscelaneos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_legalizacion_viaje_detalle = DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub grd_alimentacion_alojamiento_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_alimentacion_alojamiento.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_legalizacion_viaje_detalle = DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_legalizacion_viaje_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Sub LoadListas()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Me.cmb_codigo_facturacion.DataSourceID = ""
            Me.cmb_codigo_facturacion.DataSource = dbEntities.tme_codigo_facturacion.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_codigo_facturacion.DataTextField = "codigo_facturacion"
            Me.cmb_codigo_facturacion.DataValueField = "id_codigo_facturacion"
            Me.cmb_codigo_facturacion.DataBind()

            Me.cmb_tipo_legalizacion.DataSourceID = ""
            Me.cmb_tipo_legalizacion.DataSource = dbEntities.tme_tipo_legalizacion_viaje.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_tipo_legalizacion.DataTextField = "tipo_legalizacion"
            Me.cmb_tipo_legalizacion.DataValueField = "id_tipo_legalizacion"
            Me.cmb_tipo_legalizacion.DataBind()

            Dim codigos = dbEntities.tme_codigo_legalizacion_viaje.Where(Function(p) p.id_programa = id).ToList()
            'Me.cmb_codigo_comunicaciones.DataSourceID = ""
            'Me.cmb_codigo_comunicaciones.DataSource = codigos.Where(Function(p) p.id_tipo_codigo_legalizacion = 1).ToList()
            'Me.cmb_codigo_comunicaciones.DataTextField = "codigo"
            'Me.cmb_codigo_comunicaciones.DataValueField = "id_codigo_legalizacion"
            'Me.cmb_codigo_comunicaciones.DataBind()

            Me.cmb_codigo_pasajes.DataSourceID = ""
            Me.cmb_codigo_pasajes.DataSource = codigos.Where(Function(p) p.id_tipo_codigo_legalizacion = 2).ToList()
            Me.cmb_codigo_pasajes.DataTextField = "codigo"
            Me.cmb_codigo_pasajes.DataValueField = "id_codigo_legalizacion"
            Me.cmb_codigo_pasajes.DataBind()

            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id).OrderBy(Function(p) p.nombre_departamento).ToList()
            Me.cmb_departamento.DataSourceID = ""
            Me.cmb_departamento.DataSource = departamentos
            Me.cmb_departamento.DataTextField = "nombre_departamento"
            Me.cmb_departamento.DataValueField = "id_departamento"
            Me.cmb_departamento.DataBind()
        End Using
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
            'RadDateTimePicker1.Culture = New CultureInfo("en-US")
            'RadDateTimePicker1.DateInput.DisplayDateFormat = "dd/MM/yyyy H:mm ss"

            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)

            Dim viajeDetalle = dbEntities.vw_tme_solicitud_viaje.FirstOrDefault(Function(p) p.id_viaje = viaje.id_viaje)

            'Dim id_usuario_app = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault().id_usuario_app

            If viaje.fecha_inicio_viaje_legalizacion IsNot Nothing Then
                Me.dt_fecha_inicio.SelectedDate = viaje.fecha_inicio_viaje_legalizacion
            End If

            If viaje.fecha_finalizacion_viaje_legalizacion IsNot Nothing Then
                Me.dt_fecha_fin.SelectedDate = viaje.fecha_finalizacion_viaje_legalizacion
            End If

            If viaje.hora_inicio_viaje IsNot Nothing Then
                Me.rt_hora_inicio.DbSelectedDate = viaje.hora_inicio_viaje
            End If

            If viaje.hora_fin_viaje IsNot Nothing Then
                Me.rt_hora_fin.DbSelectedDate = viaje.hora_fin_viaje
            End If

            calcularHorasViaje()
            If viaje.fecha_inicio_viaje_legalizacion IsNot Nothing And viaje.fecha_finalizacion_viaje_legalizacion IsNot Nothing And viaje.hora_inicio_viaje IsNot Nothing And viaje.hora_fin_viaje IsNot Nothing Then
                Me.continuar_legalizacion.Visible = True
            End If
            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.lbl_categoria.Text = viaje.ta_documento_viaje.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            Me.lbl_codigo.Text = viaje.codigo_solicitud_viaje
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            Me.lbl_fecha_inicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            Me.lbl_fecha_finalizacion.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            Me.lbl_numero_contacto.Text = viaje.numero_contacto
            Me.lbl_motivo_viaje.Text = viaje.motivo_viaje

            Dim requiereSoporteTiquete = viaje.tme_solicitud_viaje_itinerario.Where(Function(p) p.requiere_linea_aerea = True).Count()
            Me.requiere_soporte_tiquete.Value = requiereSoporteTiquete
            Me.info_soporte_tiquete.Visible = False
            Me.dt_fecha.SelectedDate = viaje.fecha_tasa_ser
            Me.txt_tasa_ser.Value = viaje.tasa_ser
            If viaje.soporte_tiquetes Is Nothing And requiereSoporteTiquete > 0 Then
                Me.info_soporte_tiquete.Visible = True
            ElseIf requiereSoporteTiquete > 0 And viaje.soporte_tiquetes IsNot Nothing Then
                Me.soporte_tiquete_val.Value = 1
                Me.info_soporte_tiquete.Visible = True
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
            If viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                Me.HiddenField1.Value = viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                'fillGridRutaAprobacion(viaje.ta_documento_viaje.FirstOrDefault().id_documento)
            End If


            Dim intOwnerLegalizacion As String() = viajeDetalle.id_usuario_app_legalizacion.ToString.Split(",")
            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)


            If viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).Count() > 0 And intOwnerLegalizacion.Where(Function(p) p.Contains(idUser)).Count() > 0 Then

                Dim idDoc = viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                Me.HiddenField1.Value = viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                fillGridRutaAprobacion(viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento)

                Me.btn_enviar_aprobacion.Visible = False
                Me.rept_msgApproval.DataSource = clss_approval.get_Document_Comments_special(idDoc)
                Me.rept_msgApproval.DataBind()

                Me.grdRutaViaje.Visible = True

                Me.lyHistory.Visible = True

                Dim tbl_AppOrden As New DataTable
                tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc

                Me.btn_STandBy.Visible = False
                Me.btn_NotApproved.Visible = False
                Me.btn_Approved.Text = "Enviar por aprobación"
                If tbl_AppOrden.Rows(0)("id_ruta_next").ToString <> "-1" Then
                    btn_Completed.Visible = False
                    btn_Approved.Visible = True
                Else
                    btn_Completed.Visible = True
                    btn_Approved.Visible = False
                End If
            Else
                Me.rept_msgApproval.DataSource = Nothing
                If es_Edicion = 1 Then
                    Me.btn_enviar_aprobacion.Visible = False
                    Me.btn_guardar_legalizacion.Visible = True
                End If
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


            Dim cierre = dbEntities.tme_fecha_cierre.OrderByDescending(Function(p) p.fecha_cierre).ToList().FirstOrDefault()
            If cierre IsNot Nothing Then
                Dim fechaActual = DateTime.Now
                Dim DaysInMonth As Integer = Date.DaysInMonth(fechaActual.Year, fechaActual.Month)


                Dim intOwnerSolicitud As String() = viajeDetalle.id_usuario_app_legalizacion.ToString.Split(",")
                Dim docLegViaje = viaje.ta_documento_legalizacion_viaje.LastOrDefault()
                If docLegViaje IsNot Nothing Then
                    If docLegViaje.reversado = True Or intOwnerSolicitud.Where(Function(p) p.Contains(viajeDetalle.id_usuario)).Count() > 0 Then
                        If cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month Then

                            If cierre.fecha_cierre.Day < fechaActual.Day Then
                                Me.dt_fecha.MinDate = New DateTime(fechaActual.Year, fechaActual.Month + 1, 1)
                                If viaje.fecha_tasa_ser IsNot Nothing Then
                                    If viaje.fecha_tasa_ser.Value.Month = fechaActual.Month And viaje.fecha_tasa_ser.Value.Year = fechaActual.Month Then
                                        Me.dt_fecha.Clear()
                                    End If
                                Else
                                    Me.dt_fecha.Clear()
                                End If

                            Else
                                Me.dt_fecha.MinDate = viaje.fecha_legalizacion
                                Me.dt_fecha.MaxDate = cierre.fecha_cierre
                            End If
                        Else
                            If cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day >= fechaActual.Day Then
                                Dim dia = cierre.fecha_cierre.Day
                                For index As Integer = (dia + 1) To DaysInMonth
                                    Dim dt = New DateTime(fechaActual.Year, cierre.fecha_cierre.Month, index)
                                    Dim calendarDay = New RadCalendarDay()
                                    calendarDay.Date = dt.Date
                                    calendarDay.IsDisabled = True
                                    calendarDay.IsSelectable = False
                                    Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
                                Next
                            ElseIf cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day < fechaActual.Day Then
                                Me.dt_fecha.MinDate = fechaActual.AddDays(DaysInMonth - (fechaActual.Day - 1))
                            Else
                                Me.dt_fecha.MinDate = fechaActual.AddDays(0 - (fechaActual.Day - 1))
                            End If
                        End If
                    Else
                        If cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day >= fechaActual.Day Then
                            Dim dia = cierre.fecha_cierre.Day
                            For index As Integer = (dia + 1) To DaysInMonth
                                Dim dt = New DateTime(fechaActual.Year, cierre.fecha_cierre.Month, index)
                                Dim calendarDay = New RadCalendarDay()
                                calendarDay.Date = dt.Date
                                calendarDay.IsDisabled = True
                                calendarDay.IsSelectable = False
                                Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
                            Next
                        ElseIf cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day < fechaActual.Day Then
                            Me.dt_fecha.MinDate = fechaActual.AddDays(DaysInMonth - (fechaActual.Day - 1))
                        Else
                            Me.dt_fecha.MinDate = fechaActual.AddDays(0 - (fechaActual.Day - 1))
                        End If
                    End If
                Else
                    If cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day >= fechaActual.Day Then
                        Dim dia = cierre.fecha_cierre.Day
                        For index As Integer = (dia + 1) To DaysInMonth
                            Dim dt = New DateTime(fechaActual.Year, cierre.fecha_cierre.Month, index)
                            Dim calendarDay = New RadCalendarDay()
                            calendarDay.Date = dt.Date
                            calendarDay.IsDisabled = True
                            calendarDay.IsSelectable = False
                            Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
                        Next
                    ElseIf cierre.fecha_cierre.Year = fechaActual.Year And cierre.fecha_cierre.Month = fechaActual.Month And cierre.fecha_cierre.Day < fechaActual.Day Then
                        Me.dt_fecha.MinDate = fechaActual.AddDays(DaysInMonth - (fechaActual.Day - 1))
                    Else
                        Me.dt_fecha.MinDate = fechaActual.AddDays(0 - (fechaActual.Day - 1))
                    End If
                End If




            End If

            'Dim dt = New DateTime(2022, 10, 25)
            'Dim dt1 = New DateTime(2022, 10, 26)
            'Dim dt2 = New DateTime(2022, 10, 27)
            'Dim dt3 = New DateTime(2022, 10, 28)
            'Dim dt4 = New DateTime(2022, 10, 29)
            'Dim dt5 = New DateTime(2022, 10, 30)
            'Dim dt6 = New DateTime(2022, 10, 31)

            'Dim calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt1.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt2.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt3.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt4.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt5.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)
            'calendarDay = New RadCalendarDay()
            'calendarDay.Date = dt6.Date
            'calendarDay.IsDisabled = True
            'calendarDay.IsSelectable = False
            'Me.dt_fecha.Calendar.SpecialDays.Add(calendarDay)


        End Using
    End Sub
    Public Function getHora(dateIN As DateTime) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        Return clDate.set_TimeFormat(dateIN, timezoneUTC, True)


    End Function
    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
        'Return dateIN.ToShortDateString

    End Function
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
    End Sub
    Sub calcularHorasViaje()
        Me.viaje_8.Visible = False
        Me.viaje_9_2.Visible = True
        Me.viaje_9.Visible = True

        Using dbEntities As New dbRMS_JIEntities
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            If Me.dt_fecha_inicio.SelectedDate IsNot Nothing And Me.dt_fecha_fin.SelectedDate IsNot Nothing And Me.rt_hora_inicio.SelectedTime IsNot Nothing And Me.rt_hora_fin.SelectedTime IsNot Nothing Then

                Dim fechaInicio As DateTime = New DateTime(Me.dt_fecha_inicio.SelectedDate.Value.Year, Me.dt_fecha_inicio.SelectedDate.Value.Month,
                                                                Me.dt_fecha_inicio.SelectedDate.Value.Day, Me.rt_hora_inicio.SelectedTime.Value.Hours,
                                                                Me.rt_hora_inicio.SelectedTime.Value.Minutes, Me.rt_hora_inicio.SelectedTime.Value.Seconds)

                Dim fechaFin As DateTime = New DateTime(Me.dt_fecha_fin.SelectedDate.Value.Year, Me.dt_fecha_fin.SelectedDate.Value.Month,
                                                                Me.dt_fecha_fin.SelectedDate.Value.Day, Me.rt_hora_fin.SelectedTime.Value.Hours,
                                                                Me.rt_hora_fin.SelectedTime.Value.Minutes, Me.rt_hora_fin.SelectedTime.Value.Seconds)
                Dim diferencia As TimeSpan = fechaFin.Subtract(fechaInicio)
                If diferencia.Days = 0 And diferencia.Hours <= 12 Then
                    Me.txt_horas_diferencia.Text = "Viaje de corta duración (" & diferencia.Hours & " horas)"
                Else
                    Me.txt_horas_diferencia.Text = "Viaje mayor a 12 horas"
                End If

                Me.numero_horas_viaje.Value = diferencia.Hours
                Me.numero_dias.Value = diferencia.Days
                If diferencia.Days = 0 And diferencia.Hours >= 8 And diferencia.Hours <= 12 Then
                    Me.viaje_8.Visible = True
                    Me.viaje_9_2.Visible = False
                    Me.viaje_9.Visible = False
                End If
                If diferencia.Days = 0 And diferencia.Hours < 8 Then
                    Me.cmb_tipo_legalizacion.DataSourceID = ""
                    Me.cmb_tipo_legalizacion.DataSource = dbEntities.tme_tipo_legalizacion_viaje.Where(Function(p) p.id_programa = id And p.id_tipo_legalizacion <> 4).ToList()
                    Me.cmb_tipo_legalizacion.DataTextField = "tipo_legalizacion"
                    Me.cmb_tipo_legalizacion.DataValueField = "id_tipo_legalizacion"
                    Me.cmb_tipo_legalizacion.DataBind()
                Else
                    Me.cmb_tipo_legalizacion.DataSourceID = ""
                    Me.cmb_tipo_legalizacion.DataSource = dbEntities.tme_tipo_legalizacion_viaje.Where(Function(p) p.id_programa = id).ToList()
                    Me.cmb_tipo_legalizacion.DataTextField = "tipo_legalizacion"
                    Me.cmb_tipo_legalizacion.DataValueField = "id_tipo_legalizacion"
                    Me.cmb_tipo_legalizacion.DataBind()
                End If
            End If
        End Using

    End Sub
    Private Sub btn_continuar_Click(sender As Object, e As EventArgs) Handles btn_continuar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            viaje.fecha_tasa_ser = Me.dt_fecha.SelectedDate
            viaje.fecha_legalizacion = DateTime.Now
            viaje.tasa_ser = Me.txt_tasa_ser.Value
            viaje.fecha_inicio_viaje_legalizacion = Me.dt_fecha_inicio.SelectedDate
            viaje.fecha_finalizacion_viaje_legalizacion = Me.dt_fecha_fin.SelectedDate

            viaje.hora_inicio_viaje = Me.rt_hora_inicio.SelectedDate.Value.ToShortTimeString()
            viaje.hora_fin_viaje = Me.rt_hora_fin.SelectedDate.Value.ToShortTimeString()

            Dim soporte = New tme_soporte_legalizacion_viaje
            For Each file As UploadedFile In soporte_tiquete.UploadedFiles
                Dim fecha = DateTime.Now
                Dim exten = file.GetExtension()
                Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)
                viaje.soporte_tiquetes = nombreArchivo
                soporte.soporte = nombreArchivo
                soporte.id_tipo_soporte_legalizacion = 7
                soporte.id_viaje = viaje.id_viaje
                dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                dbEntities.SaveChanges()
                fillGridArchivos()
            Next
            dbEntities.Entry(viaje).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
        End Using



        Me.continuar_legalizacion.Visible = True
    End Sub

    Private Sub dt_fecha_inicio_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles dt_fecha_inicio.SelectedDateChanged
        calcularHorasViaje()
    End Sub

    Private Sub dt_fecha_fin_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles dt_fecha_fin.SelectedDateChanged
        calcularHorasViaje()
    End Sub

    Private Sub rt_hora_inicio_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles rt_hora_inicio.SelectedDateChanged
        calcularHorasViaje()
    End Sub

    Private Sub rt_hora_fin_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles rt_hora_fin.SelectedDateChanged
        calcularHorasViaje()
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
    Protected Sub grd_alojamiento_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_alojamiento.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_viaje_hotel").ToString()

        End If
    End Sub

    Protected Sub grd_itinerario_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_itinerario.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_viaje_itinerario").ToString()

        End If
    End Sub
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

    Private Sub dt_fecha_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles dt_fecha.SelectedDateChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim fecha = Me.dt_fecha.SelectedDate
            Dim anio = fecha?.Year
            Dim mes = fecha?.Month
            Dim tasaSer = dbEntities.tme_tasa_ser.Where(Function(p) p.anio = anio And p.id_mes = mes).FirstOrDefault()
            Dim tasaSerRegistrada As Boolean = True
            If tasaSer Is Nothing Then
                tasaSerRegistrada = False
                Me.errorTasaSer.Visible = True
                'Me.continuarLegalizacion.Visible = False
            Else
                Me.id_tasa_ser.Value = tasaSer.id_tasa_ser
                Me.txt_tasa_ser.Value = tasaSer.tasa_ser
                'Me.continuarLegalizacion.Visible = True
                Me.errorTasaSer.Visible = False
                calcularPerDiem()

                'Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                ''ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "ResetVal()", True)

                Dim id_Viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_Viaje)

                Dim fechaLegalizacion = viaje.fecha_tasa_ser

                If viaje.fecha_tasa_ser IsNot Nothing Then
                    If fechaLegalizacion.Value.Month <> tasaSer.id_mes Or fechaLegalizacion.Value.Year <> tasaSer.anio Then
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "ajusteTasaSer()", True)
                    End If
                End If
            End If

        End Using
    End Sub

    'Private Sub txt_kilometros_TextChanged(sender As Object, e As EventArgs) Handles txt_kilometros.TextChanged
    '    Dim kilometros = Me.txt_kilometros.Value

    '    If kilometros > 0 Then
    '        Dim montoAuto = (kilometros / 1.609) * 0.56 * Me.txt_tasa_ser.Value
    '        Me.txt_monto_auto.Value = montoAuto
    '    End If
    'End Sub

    Private Sub cmb_tipo_legalizacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_legalizacion.SelectedIndexChanged
        Me.info_general.Visible = False
        Me.info_reuniones.Visible = False
        Me.info_miscelaneos.Visible = False
        Me.info_estadida_alimentacion.Visible = False
        Me.rv_gasto_evento_rbn.Visible = False
        Me.rv_gasto_evento_rbn.ControlToValidate = ""
        Me.rv_monto_evento.Visible = False
        Me.rv_monto_evento.ControlToValidate = ""
        Me.rv_monto_miscelaneos.Visible = False
        Me.rv_monto_miscelaneos.ControlToValidate = ""
        If Me.cmb_tipo_legalizacion.SelectedValue = 1 Then
            Me.info_general.Visible = True
        ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 2 Then
            Me.info_reuniones.Visible = True
            Me.rv_gasto_evento_rbn.ControlToValidate = "rbn_gasto_reunion"
            Me.rv_gasto_evento_rbn.Visible = True

            Me.rv_monto_evento.ControlToValidate = "txt_monto_reuniones"
            Me.rv_monto_evento.Visible = True
        ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 3 Then
            Me.info_miscelaneos.Visible = True

            Me.rv_monto_miscelaneos.ControlToValidate = "txt_monto_miscelaneos"
            Me.rv_monto_miscelaneos.Visible = True
        ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 4 Then
            Me.info_estadida_alimentacion.Visible = True
        End If

    End Sub
    Sub loadMunicipios()
        Using dbEntities As New dbRMS_JIEntities
            Dim idDepto = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()

            Me.cmb_municipio.DataSourceID = ""
            Me.cmb_municipio.DataSource = municipios
            Me.cmb_municipio.DataTextField = "nombre_municipio"
            Me.cmb_municipio.DataValueField = "id_municipio"
            Me.cmb_municipio.DataBind()
        End Using
    End Sub
    Private Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged
        Me.cmb_municipio.ClearSelection()
        loadMunicipios()
    End Sub

    Private Sub cmb_municipio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_municipio.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Me.rv_dia_rbn.Visible = False
            Me.rv_dia_rbn.ControlToValidate = ""
            If Me.cmb_municipio.SelectedValue <> "" Then
                Me.rv_dia_rbn.Visible = True
                Me.rv_dia_rbn.ControlToValidate = "rbn_dia"
            End If

            Me.rbn_zona_legalizacion.Items.Clear()

            If Me.cmb_municipio.SelectedValue <> "" Then
                Me.rbn_zona_legalizacion.Items.Add(New ListItem("Sí", "Rural"))
                Me.rbn_zona_legalizacion.Items.Add(New ListItem("No", "No"))

                Dim idMunicipio = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
                Dim municipio = dbEntities.t_municipios.Find(idMunicipio)
                'If municipio.mye_rate_usd > 0 Then
                '    Me.rbn_zona_legalizacion.Items.Add(New ListItem("No aplica", "NA"))
                'End If
                Me.zr_data.Visible = True
                Me.rbn_zona_legalizacion.Enabled = True
                If municipio.zona_rural IsNot Nothing Then
                    If municipio.zona_rural = True Then
                        Me.rbn_zona_legalizacion.SelectedValue = "Rural"
                        Me.rbn_zona_legalizacion.Enabled = False
                        Me.zr_data.Visible = False
                    End If
                End If


                Me.rbn_zona_legalizacion.DataBind()
            End If





            calcularPerDiem()
        End Using

    End Sub

    Private Sub rbn_dia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_dia.SelectedIndexChanged
        calcularPerDiem()
    End Sub

    Sub calcularPerDiem()
        Using dbEntities As New dbRMS_JIEntities
            Dim idMunicipio = 0
            Dim dia = 0
            If Me.cmb_municipio.SelectedValue <> "" Then
                idMunicipio = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
            End If
            If Me.rbn_dia.SelectedValue <> "" Then
                dia = Convert.ToInt32(Me.rbn_dia.SelectedValue)
            End If
            Dim perdiem = 0

            Dim valorDescuentoAlmuerzo = 0
            Dim descuentoAlmuerzo = 0
            Dim descuentoAlmuerzo75 = 0

            Dim valorDescuentoDesayuno = 0
            Dim descuentoDesayuno = 0
            Dim descuentoDesayuno75 = 0

            Dim valorDescuentoCena = 0
            Dim descuentoCena = 0
            Dim descuentoCena75 = 0

            'Me.zona_legalizacion.Visible = False

            Dim valorTotalDescuento = 0
            Dim tarifa_almuerzo = 0
            Dim numDias = Convert.ToInt32(Me.numero_dias.Value)
            Dim zonaRural = Me.rbn_zona_legalizacion.SelectedValue
            If idMunicipio > 0 And Convert.ToInt32(numero_horas_viaje.Value) >= 8 And Convert.ToInt32(numero_horas_viaje.Value) <= 12 And numDias = 0 And zonaRural <> "" Then
                Dim municipio = dbEntities.t_municipios.Find(idMunicipio)

                If municipio.zona_rural IsNot Nothing Then
                    If municipio.zona_rural = True Then
                        zonaRural = "Rural"
                    End If
                End If

                If municipio.mye_rate_usd > 0 And zonaRural = "No" Then
                    tarifa_almuerzo = municipio.mye_rate_almuerzo * Me.txt_tasa_ser.Value
                ElseIf zonaRural = "No" Then
                    tarifa_almuerzo = 12 * Me.txt_tasa_ser.Value
                ElseIf zonaRural = "Rural" Then
                    tarifa_almuerzo = 6 * Me.txt_tasa_ser.Value
                End If

                Me.txt_per_diem_8.Value = tarifa_almuerzo
            Else
                If idMunicipio > 0 And dia > 0 And zonaRural <> "" Then
                    Dim municipio = dbEntities.t_municipios.Find(idMunicipio)
                    If municipio.zona_rural IsNot Nothing Then
                        If municipio.zona_rural = True Then
                            zonaRural = "Rural"
                        End If
                    End If

                    If municipio.mye_rate_usd > 0 And zonaRural = "No" Then
                        perdiem = municipio.mye_rate_usd * Me.txt_tasa_ser.Value

                        tarifa_almuerzo = municipio.mye_rate_almuerzo
                        descuentoAlmuerzo = municipio.mye_rate_almuerzo * Me.txt_tasa_ser.Value
                        descuentoAlmuerzo75 = municipio.mye_rate_almuerzo_75 * Me.txt_tasa_ser.Value


                        descuentoDesayuno = municipio.mye_rate_desayuno * Me.txt_tasa_ser.Value
                        descuentoDesayuno75 = municipio.mye_rate_desayuno_75 * Me.txt_tasa_ser.Value

                        descuentoCena = municipio.mye_rate_cena * Me.txt_tasa_ser.Value
                        descuentoCena75 = municipio.mye_rate_cena_75 * Me.txt_tasa_ser.Value

                    ElseIf zonaRural = "No" Then
                        perdiem = 50 * Me.txt_tasa_ser.Value

                        tarifa_almuerzo = 12
                        descuentoAlmuerzo = 12 * Me.txt_tasa_ser.Value
                        descuentoAlmuerzo75 = 9 * Me.txt_tasa_ser.Value

                        descuentoDesayuno = 8 * Me.txt_tasa_ser.Value
                        descuentoDesayuno75 = 6 * Me.txt_tasa_ser.Value

                        descuentoCena = 20 * Me.txt_tasa_ser.Value
                        descuentoCena75 = 15 * Me.txt_tasa_ser.Value


                    ElseIf zonaRural = "Rural" Then
                        perdiem = 23 * Me.txt_tasa_ser.Value
                        descuentoAlmuerzo = 6 * Me.txt_tasa_ser.Value
                        descuentoAlmuerzo75 = 4 * Me.txt_tasa_ser.Value

                        descuentoDesayuno = 3 * Me.txt_tasa_ser.Value
                        descuentoDesayuno75 = 3 * Me.txt_tasa_ser.Value

                        descuentoCena = 9 * Me.txt_tasa_ser.Value
                        descuentoCena75 = 7 * Me.txt_tasa_ser.Value
                        tarifa_almuerzo = 6

                    End If


                    If Convert.ToInt32(numero_horas_viaje.Value) >= 8 And Convert.ToInt32(numero_horas_viaje.Value) < 12 Then
                        Me.txt_per_diem_8.Value = tarifa_almuerzo
                    End If
                    Me.valor_perdiem.Value = perdiem




                    If perdiem <> 0 Then
                        If dia = 1 Or dia = 3 Then
                            Me.txt_per_diem.Value = perdiem * 0.75

                            'Me.descuento.Value = descuentoAlmuerzo75
                            'valorDescuentoAlmuerzo = descuentoAlmuerzo75
                        Else
                            Me.txt_per_diem.Value = perdiem

                            'Me.descuento.Value = descuentoAlmuerzo
                            'valorDescuentoAlmuerzo = descuentoAlmuerzo
                        End If
                    End If

                    'If Convert.ToInt32(Me.numero_horas_viaje.Value) >= 8 And Convert.ToInt32(Me.numero_horas_viaje.Value) < 12 Then
                    '    Me.txt_per_diem.Value = descuentoAlmuerzo
                    'End If

                End If
            End If



            'For Each ChkListItem In Me.chk_tipo_descuento.Items
            '    If ChkListItem.Selected = True Then
            '        If ChkListItem.Value = "desayuno" Then
            '        ElseIf ChkListItem.Value = "almuerzo" Then
            '        ElseIf ChkListItem.Value = "cena" Then

            '        End If
            '    End If
            'Next
            Me.txt_descuento_alimentacion.Value = 0
            If Me.rbn_descuento.SelectedValue <> "" Then
                If Me.rbn_descuento.SelectedValue = "1" Then

                    For Each ChkListItem In Me.chk_tipo_descuento.Items
                        If ChkListItem.Selected = True Then
                            If ChkListItem.Value = "desayuno" Then
                                If perdiem <> 0 Then
                                    If dia = 1 Or dia = 3 Then
                                        valorTotalDescuento = valorTotalDescuento + descuentoDesayuno75
                                        Me.descuento_desayuno.Value = descuentoDesayuno75
                                    Else
                                        valorTotalDescuento = valorTotalDescuento + descuentoDesayuno
                                        Me.descuento_desayuno.Value = descuentoDesayuno
                                    End If


                                End If
                            ElseIf ChkListItem.Value = "almuerzo" Then
                                If perdiem <> 0 Then
                                    If dia = 1 Or dia = 3 Then
                                        valorTotalDescuento = valorTotalDescuento + descuentoAlmuerzo75
                                        Me.descuento_almuerzo.Value = descuentoAlmuerzo75
                                    Else
                                        valorTotalDescuento = valorTotalDescuento + descuentoAlmuerzo
                                        Me.descuento_almuerzo.Value = descuentoAlmuerzo
                                    End If
                                End If
                            ElseIf ChkListItem.Value = "cena" Then
                                If perdiem <> 0 Then
                                    If dia = 1 Or dia = 3 Then
                                        valorTotalDescuento = valorTotalDescuento + descuentoCena75
                                        Me.descuento_cena.Value = descuentoCena75
                                    Else
                                        valorTotalDescuento = valorTotalDescuento + descuentoCena
                                        Me.descuento_cena.Value = descuentoCena
                                    End If
                                End If
                            End If
                        End If
                    Next

                    Me.txt_descuento_alimentacion.Value = valorTotalDescuento
                End If
            End If

        End Using
    End Sub
    Private Sub chk_tipo_descuento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chk_tipo_descuento.SelectedIndexChanged
        calcularPerDiem()
    End Sub
    Private Sub rbn_descuento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_descuento.SelectedIndexChanged
        calcularPerDiem()
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim legalizacionViaje = New tme_solicitud_viaje_legalizacion
                legalizacionViaje.id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(legalizacionViaje.id_viaje)

                legalizacionViaje.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                legalizacionViaje.fecha = Me.dt_fecha_adquirio_servicio.SelectedDate
                legalizacionViaje.fecha_crea = DateTime.Now
                legalizacionViaje.tasa_ser = Me.txt_tasa_ser.Value
                legalizacionViaje.id_tasa_ser = Convert.ToInt32(Me.id_tasa_ser.Value)
                legalizacionViaje.id_codigo_facturacion = Convert.ToInt32(Me.cmb_codigo_facturacion.SelectedValue)
                legalizacionViaje.id_tipo_legalizacion = Convert.ToInt32(Me.cmb_tipo_legalizacion.SelectedValue)
                legalizacionViaje.nro_rec = Me.txt_rec.Text
                legalizacionViaje.descripcion_gasto = Me.txt_descripcion_gasto.Text

                Dim guardar = True

                If Me.cmb_tipo_legalizacion.SelectedValue = 1 Then
                    If Me.txt_monto_pasajes.Value = 0 Then
                        guardar = False
                    End If
                ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 2 Then
                    If Me.txt_monto_reuniones.Value = 0 Then
                        guardar = False
                    End If
                ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 3 Then
                    If Me.txt_monto_miscelaneos.Value = 0 Then
                        guardar = False
                    End If
                ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 4 Then
                    If Me.txt_per_diem.Value = 0 And Me.txt_monto_alojamiento.Value = 0 Then
                        guardar = False
                    End If
                End If

                If guardar = False Then
                    Me.errorGuardar.Visible = True
                Else
                    If Me.cmb_tipo_legalizacion.SelectedValue = 1 Then
                        legalizacionViaje.monto_auto = 0
                        'legalizacionViaje.monto_comunicaciones = Me.txt_monto_comunicaciones.Value
                        'If Me.txt_monto_comunicaciones.Value > 0 Then
                        '    legalizacionViaje.id_codigo_legalizacion_comunicaciones = Convert.ToInt32(Me.cmb_codigo_comunicaciones.SelectedValue)
                        'End If
                        legalizacionViaje.monto_pasajes = Me.txt_monto_pasajes.Value
                        If Me.txt_monto_pasajes.Value > 0 Then
                            legalizacionViaje.id_codigo_legalizacion_pasajes = Convert.ToInt32(Me.cmb_codigo_pasajes.SelectedValue)
                        End If
                        'legalizacionViaje.kilometros = Me.txt_kilometros.Value
                        'If Me.txt_monto_auto.Value > 0 Then
                        '    legalizacionViaje.millas = legalizacionViaje.kilometros / 1.609
                        '    legalizacionViaje.monto_auto = Me.txt_monto_auto.Value
                        'End If

                        legalizacionViaje.monto_total = legalizacionViaje.monto_pasajes

                        dbEntities.tme_solicitud_viaje_legalizacion.Add(legalizacionViaje)
                        dbEntities.SaveChanges()

                        'If Me.txt_monto_comunicaciones.Value > 0 Then
                        '    Dim soporte = New tme_soporte_legalizacion_viaje
                        '    soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle

                        '    For Each file As UploadedFile In soporte_comunicaciones.UploadedFiles
                        '        Dim fecha = DateTime.Now
                        '        Dim exten = file.GetExtension()
                        '        Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                        '        soporte.soporte = nombreArchivo
                        '        soporte.id_tipo_soporte_legalizacion = 2
                        '        Dim Path As String
                        '        Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                        '        file.SaveAs(Path + nombreArchivo)

                        '        dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                        '        dbEntities.SaveChanges()
                        '    Next
                        'End If

                        If Me.txt_monto_pasajes.Value > 0 Then
                            Dim soporte = New tme_soporte_legalizacion_viaje
                            soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle

                            For Each file As UploadedFile In soporte_pasajes.UploadedFiles
                                Dim fecha = DateTime.Now
                                Dim exten = file.GetExtension()
                                Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                                soporte.soporte = nombreArchivo
                                soporte.id_tipo_soporte_legalizacion = 3
                                soporte.id_viaje = viaje.id_viaje
                                Dim Path As String
                                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                                file.SaveAs(Path + nombreArchivo)

                                dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                                dbEntities.SaveChanges()
                            Next
                        End If

                        'If Me.txt_monto_auto.Value > 0 Then
                        '    Dim soporte = New tme_soporte_legalizacion_viaje
                        '    soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle

                        '    For Each file As UploadedFile In soporte_auto.UploadedFiles
                        '        Dim fecha = DateTime.Now
                        '        Dim exten = file.GetExtension()
                        '        Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                        '        soporte.soporte = nombreArchivo
                        '        soporte.id_tipo_soporte_legalizacion = 4
                        '        Dim Path As String
                        '        Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                        '        file.SaveAs(Path + nombreArchivo)

                        '        dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                        '        dbEntities.SaveChanges()
                        '    Next
                        'End If


                        Me.txt_monto_pasajes.Text = 0
                        'Me.txt_monto_comunicaciones.Text = 0
                        'Me.txt_monto_auto.Text = 0
                        'Me.txt_kilometros.Text = 0
                        'Me.cmb_codigo_comunicaciones.ClearSelection()
                        Me.cmb_codigo_pasajes.ClearSelection()

                        'Me.soporte_comunicaciones_val.Value = 1
                        Me.soporte_pasajes_val.Value = 1
                        'Me.soporte_auto_val.Value = 1
                    ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 2 Then
                        legalizacionViaje.gasto_entrenamiento = If(Me.rbn_gasto_reunion.SelectedValue = "1", True, False)
                        legalizacionViaje.gasto_reunion = If(Me.rbn_gasto_reunion.SelectedValue = "2", True, False)
                        legalizacionViaje.nro_participantes = Me.txt_numero_participantes.Value
                        legalizacionViaje.monto_total = Me.txt_monto_reuniones.Value

                        dbEntities.tme_solicitud_viaje_legalizacion.Add(legalizacionViaje)
                        dbEntities.SaveChanges()

                        Dim soporte = New tme_soporte_legalizacion_viaje
                        soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle

                        For Each file As UploadedFile In soporte_Reuniones.UploadedFiles
                            Dim fecha = DateTime.Now
                            Dim exten = file.GetExtension()
                            Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                            soporte.soporte = nombreArchivo
                            soporte.id_viaje = viaje.id_viaje
                            soporte.id_tipo_soporte_legalizacion = 5
                            Dim Path As String
                            Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                            file.SaveAs(Path + nombreArchivo)

                            dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                            dbEntities.SaveChanges()
                        Next

                        Me.rbn_gasto_reunion.ClearSelection()
                        Me.txt_numero_participantes.Text = String.Empty
                        Me.txt_monto_reuniones.Text = 0

                        Me.soporte_reuniones_val.Value = 1
                    ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 3 Then
                        legalizacionViaje.monto_total = Me.txt_monto_miscelaneos.Value
                        dbEntities.tme_solicitud_viaje_legalizacion.Add(legalizacionViaje)
                        dbEntities.SaveChanges()

                        Dim soporte = New tme_soporte_legalizacion_viaje
                        soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle

                        For Each file As UploadedFile In soporte_miscelaneos.UploadedFiles
                            Dim fecha = DateTime.Now
                            Dim exten = file.GetExtension()
                            Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                            soporte.soporte = nombreArchivo
                            soporte.id_tipo_soporte_legalizacion = 6
                            soporte.id_viaje = viaje.id_viaje
                            Dim Path As String
                            Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                            file.SaveAs(Path + nombreArchivo)

                            dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                            dbEntities.SaveChanges()
                        Next
                        Me.soporte_miscelaneos_val.Value = 1
                        Me.txt_monto_miscelaneos.Text = 0
                    ElseIf Me.cmb_tipo_legalizacion.SelectedValue = 4 Then
                        Dim numDias = Convert.ToInt32(Me.numero_dias.Value)
                        If Convert.ToInt32(Me.numero_horas_viaje.Value) >= 8 And Convert.ToInt32(Me.numero_horas_viaje.Value) <= 12 And numDias = 0 Then
                            legalizacionViaje.monto_alimentacion = Me.txt_per_diem_8.Value
                            legalizacionViaje.monto_alojamiento = 0
                            legalizacionViaje.descuento_alimentacion = 0
                            legalizacionViaje.primer_dia = True
                            legalizacionViaje.ultimo_dia = False
                            legalizacionViaje.dia_intermedio = False
                            legalizacionViaje.valor_perdiem = Convert.ToDouble(Me.valor_perdiem.Value)
                            legalizacionViaje.id_municipio = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
                            legalizacionViaje.zona_legalizacion_viaticos = Me.rbn_zona_legalizacion.SelectedValue
                            legalizacionViaje.monto_total = legalizacionViaje.monto_alimentacion
                            dbEntities.tme_solicitud_viaje_legalizacion.Add(legalizacionViaje)
                            dbEntities.SaveChanges()
                        Else
                            legalizacionViaje.monto_alojamiento = 0
                            legalizacionViaje.monto_alimentacion = 0
                            legalizacionViaje.descuento_alimentacion = 0
                            legalizacionViaje.zona_legalizacion_viaticos = Me.rbn_zona_legalizacion.SelectedValue
                            If Me.txt_per_diem.Value > 0 Then
                                legalizacionViaje.monto_alimentacion = Me.txt_per_diem.Value
                            End If
                            If Me.txt_descuento_alimentacion.Value > 0 Then
                                legalizacionViaje.descuento_alimentacion = Me.txt_descuento_alimentacion.Value


                                For Each ChkListItem In Me.chk_tipo_descuento.Items
                                    If ChkListItem.Selected = True Then
                                        If ChkListItem.Value = "desayuno" Then
                                            legalizacionViaje.descuento_desayuno = True
                                            legalizacionViaje.valor_descuento_desayuno = Convert.ToDouble(Me.descuento_desayuno.Value)
                                        ElseIf ChkListItem.Value = "almuerzo" Then
                                            legalizacionViaje.descuento_almuerzo = True
                                            legalizacionViaje.valor_descuento_almuerzo = Convert.ToDouble(Me.descuento_almuerzo.Value)
                                        ElseIf ChkListItem.Value = "cena" Then
                                            legalizacionViaje.descuento_cena = True
                                            legalizacionViaje.valor_descuento_cena = Convert.ToDouble(Me.descuento_cena.Value)
                                        End If
                                    End If
                                Next



                            End If
                            If Me.txt_per_diem.Value > 0 Then
                                legalizacionViaje.primer_dia = If(Me.rbn_dia.SelectedValue = "1", True, False)
                                legalizacionViaje.ultimo_dia = If(Me.rbn_dia.SelectedValue = "3", True, False)
                                legalizacionViaje.dia_intermedio = If(Me.rbn_dia.SelectedValue = "2", True, False)
                                legalizacionViaje.porcentaje_perdiem = If(Me.rbn_dia.SelectedValue = "1" Or Me.rbn_dia.SelectedValue = "3", 75, 100)
                                legalizacionViaje.valor_perdiem = Convert.ToDouble(Me.valor_perdiem.Value)
                                legalizacionViaje.id_municipio = Convert.ToInt32(Me.cmb_municipio.SelectedValue)


                            End If

                            If Me.txt_monto_alojamiento.Value > 0 Then
                                legalizacionViaje.monto_alojamiento = Me.txt_monto_alojamiento.Value
                            End If

                            legalizacionViaje.monto_total = legalizacionViaje.monto_alojamiento + legalizacionViaje.monto_alimentacion - legalizacionViaje.descuento_alimentacion
                            dbEntities.tme_solicitud_viaje_legalizacion.Add(legalizacionViaje)
                            dbEntities.SaveChanges()
                            If Me.txt_monto_alojamiento.Value > 0 Then
                                Dim soporte = New tme_soporte_legalizacion_viaje
                                soporte.id_legalizacion_viaje_detalle = legalizacionViaje.id_legalizacion_viaje_detalle
                                For Each file As UploadedFile In soporte_alojamiento.UploadedFiles
                                    Dim fecha = DateTime.Now
                                    Dim exten = file.GetExtension()
                                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                                    soporte.soporte = nombreArchivo
                                    soporte.id_tipo_soporte_legalizacion = 1
                                    soporte.id_viaje = viaje.id_viaje
                                    Dim Path As String
                                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                                    file.SaveAs(Path + nombreArchivo)

                                    dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                                    dbEntities.SaveChanges()
                                Next
                                Me.soporte_alojamiento_val.Value = 1
                            End If
                            Me.soporte_alojamiento_val.Value = 1
                            Me.rbn_dia.ClearSelection()
                            Me.rbn_descuento.ClearSelection()
                            Me.txt_per_diem.Text = String.Empty
                            Me.txt_monto_alojamiento.Text = 0
                            Me.txt_descuento_alimentacion.Value = 0
                        End If

                    End If
                    Me.txt_descripcion_gasto.Text = String.Empty
                    For Each ChkListItem In Me.chk_tipo_descuento.Items
                        ChkListItem.Selected = False
                    Next
                    fillGridArchivos()
                    fillGridLegalizacion()
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "ResetVal()", True)
                End If
            End Using
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    'Private Sub txt_monto_comunicaciones_TextChanged(sender As Object, e As EventArgs) Handles txt_monto_comunicaciones.TextChanged
    '    If Me.txt_monto_comunicaciones.Value > 0 Then
    '        Me.rv_codigo_comunicaciones.ControlToValidate = "cmb_codigo_comunicaciones"
    '        Me.rv_codigo_comunicaciones.Visible = True
    '    End If
    'End Sub

    Private Sub txt_monto_pasajes_TextChanged(sender As Object, e As EventArgs) Handles txt_monto_pasajes.TextChanged
        If Me.txt_monto_pasajes.Value > 0 Then
            Me.rv_codigo_pasajes.ControlToValidate = "cmb_codigo_pasajes"
            Me.rv_codigo_pasajes.Visible = True
        End If

    End Sub

    Protected Sub delete_detalle(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipoEliminar.Value = 1
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub delete_detalle2(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)

        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipoEliminar.Value = 2
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idDetalle = Convert.ToInt32(Me.identity.Value)
            Dim tipo = Convert.ToInt32(Me.tipoEliminar.Value)

            If tipo = 1 Then
                Dim detalle = dbEntities.tme_solicitud_viaje_legalizacion.Find(idDetalle)
                If detalle IsNot Nothing Then

                    Dim soporteAsociado = dbEntities.tme_soporte_legalizacion_viaje.Where(Function(p) p.id_legalizacion_viaje_detalle = idDetalle).ToList()
                    If soporteAsociado.Count() > 0 Then
                        dbEntities.Database.ExecuteSqlCommand("delete from tme_soporte_legalizacion_viaje where id_legalizacion_viaje_detalle = " & idDetalle)
                    End If
                    dbEntities.Entry(detalle).State = Entity.EntityState.Deleted
                    dbEntities.SaveChanges()
                End If
                fillGridArchivos()
                fillGridLegalizacion()
            ElseIf tipo = 2 Then

                Dim detalle = dbEntities.tme_soporte_legalizacion_viaje.Find(idDetalle)
                If detalle IsNot Nothing Then
                    dbEntities.Entry(detalle).State = Entity.EntityState.Deleted
                    dbEntities.SaveChanges()
                End If

                fillGridArchivos()

            End If

            Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using

    End Sub

    Private Sub btn_enviar_aprobacion_Click(sender As Object, e As EventArgs) Handles btn_enviar_aprobacion.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                viaje.fecha_tasa_ser = Me.dt_fecha.SelectedDate
                viaje.fecha_legalizacion = DateTime.Now
                viaje.tasa_ser = Me.txt_tasa_ser.Value
                viaje.fecha_inicio_viaje_legalizacion = Me.dt_fecha_inicio.SelectedDate
                viaje.fecha_finalizacion_viaje_legalizacion = Me.dt_fecha_fin.SelectedDate

                viaje.hora_inicio_viaje = Me.rt_hora_inicio.SelectedDate.Value.ToShortTimeString()
                viaje.hora_fin_viaje = Me.rt_hora_fin.SelectedDate.Value.ToShortTimeString()

                Dim soporte = New tme_soporte_legalizacion_viaje
                For Each file As UploadedFile In soporte_tiquete.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    viaje.soporte_tiquetes = nombreArchivo
                    soporte.soporte = nombreArchivo
                    soporte.id_tipo_soporte_legalizacion = 7
                    soporte.id_viaje = viaje.id_viaje
                    dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                    dbEntities.SaveChanges()
                Next
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                If viaje IsNot Nothing And viaje.ta_documento_legalizacion_viaje.Where(Function(p) p.reversado Is Nothing).Count() = 0 Then

                    Dim id_categoriaAPP = 2043

                    Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
                    Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)

                    If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                        Me.lblerr_user.Text = "La legalización se guardo correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de legalización de viajes, contáctese con el administrador."
                        Me.lblerr_user.Visible = True
                    Else
                        Dim id_documento = guardarDocumento(viaje, viaje.t_usuarios)
                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                        Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    End If


                End If
            End Using

        Catch ex As Exception
            Dim err = ex.Message
        End Try

    End Sub
    Sub guardarRelacionDocumento(ByVal idDocumento As Integer, ByVal idViaje As Integer)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim documentoViaje As New ta_documento_legalizacion_viaje
                documentoViaje.id_viaje = idViaje
                documentoViaje.id_documento = idDocumento
                dbEntities.ta_documento_legalizacion_viaje.Add(documentoViaje)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Function guardarDocumento(ByVal viaje As tme_solicitud_viaje, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2043

        Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)
        Dim id_tipo_documento = tblUserApprovalTimeSheet.Rows(0).Item("id_tipoDocumento")


        Dim descripcion = String.Format("Legalización de viaje {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, viaje.fecha_inicio_viaje)
        Dim err As Boolean = False


        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

        clss_approval.set_ta_documento(0) 'Set new Record
        clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", id_tipo_documento, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("numero_instrumento", viaje.codigo_solicitud_viaje, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("descripcion_doc", descripcion, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("nom_beneficiario", usuario.nombre_usuario & " " & usuario.apellidos_usuario, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("comentarios", viaje.motivo_viaje, "id_documento", 0) '.Replace("'", "''")
        clss_approval.set_ta_documentoFIELDS("codigo_AID", viaje.codigo_solicitud_viaje, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", viaje.codigo_solicitud_viaje, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("ficha_actividad", "NO", "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("monto_ficha", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("regional", Me.Session("E_SubRegion").ToString.Trim, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_Approval", viaje.codigo_solicitud_viaje, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("id_tipoAprobacion", 4, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("monto_total", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("tasa_cambio", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("datecreated", Date.UtcNow, "id_documento", 0)

        Dim id_documento = clss_approval.save_ta_documento()

        Dim tbl_Route_By_DOC As New DataTable

        tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipo_documento, 0) 'First Step
        Dim idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")

        Dim Duracion As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")

        Dim fecha_limit As DateTime = DateAdd(DateInterval.Day, Duracion, Date.UtcNow) 'UTC DATE
        Dim fecha_Recep As DateTime = Date.UtcNow 'UTC DATE


        clss_approval.set_ta_AppDocumento(0) 'New Record
        clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_Recep, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cOPEN, "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("observacion", descripcion, "id_app_documento", 0) '.Replace("'", "''")
        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApprovalTimeSheet.Rows(0).Item("id_rol"), "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

        Dim id_appdocumento = clss_approval.save_ta_AppDocumento()
        If id_appdocumento <> -1 Then
            tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipo_documento, 1) 'Next Step
            Dim NextUser As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
            Dim idNextRol As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_rol")
            fecha_Recep = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
            idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
            fecha_limit = calculaDiaHabil(Duracion, fecha_Recep)
            clss_approval.set_ta_AppDocumento(0) 'New Record
            clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
            clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
            clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
            clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", NextUser, "id_app_documento", 0)
            clss_approval.set_ta_AppDocumentoFIELDS("observacion", descripcion.Trim, "id_app_documento", 0) 'Pending Step 
            clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role --NEW
            clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

            Dim id_appdocumento2 = clss_approval.save_ta_AppDocumento()



            If id_appdocumento2 <> -1 Then

            Else
                err = True
            End If  'app_documento 2
            guardarRelacionDocumento(id_documento, viaje.id_viaje)
            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1021, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(id_appdocumento, Integer), viaje.id_viaje)) Then
            Else 'Error mandando Email
            End If
        End If

        Return id_documento

    End Function



    Protected Sub btn_Approved_Click(sender As Object, e As EventArgs) Handles btn_Approved.Click

        'If check_exchange_Rate() Then
        'EXECUTE_ACTION(cAPPROVED)
        'btn_Approved.Enabled = True
        'End If

        'Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        'Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        'If boolIS_deliverable Then
        '    check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        'Else
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                viaje.fecha_tasa_ser = Me.dt_fecha.SelectedDate
                viaje.fecha_legalizacion = DateTime.Now
                viaje.tasa_ser = Me.txt_tasa_ser.Value
                viaje.fecha_inicio_viaje_legalizacion = Me.dt_fecha_inicio.SelectedDate
                viaje.fecha_finalizacion_viaje_legalizacion = Me.dt_fecha_fin.SelectedDate

                viaje.hora_inicio_viaje = Me.rt_hora_inicio.SelectedDate.Value.ToShortTimeString()
                viaje.hora_fin_viaje = Me.rt_hora_fin.SelectedDate.Value.ToShortTimeString()

                Dim soporte = New tme_soporte_legalizacion_viaje
                For Each file As UploadedFile In soporte_tiquete.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & viaje.codigo_solicitud_viaje & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    viaje.soporte_tiquetes = nombreArchivo
                    soporte.soporte = nombreArchivo
                    soporte.id_tipo_soporte_legalizacion = 7
                    soporte.id_viaje = viaje.id_viaje
                    dbEntities.tme_soporte_legalizacion_viaje.Add(soporte)
                    dbEntities.SaveChanges()
                Next
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End Using

            EXECUTE_ACTION(cAPPROVED)
        Catch ex As Exception
            Dim err = ex.Message
        End Try


        'End If

    End Sub
    Protected Sub btn_Completed_Click(sender As Object, e As EventArgs) Handles btn_Completed.Click


        'Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        'Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        'If boolIS_deliverable Then
        '    check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        'Else
        EXECUTE_ACTION(cAPPROVED)
        'End If




    End Sub

    Protected Sub btn_STandBy_Click(sender As Object, e As EventArgs) Handles btn_STandBy.Click
        EXECUTE_ACTION(cSTANDby)
    End Sub

    Protected Sub btn_NotApproved_Click(sender As Object, e As EventArgs) Handles btn_NotApproved.Click
        EXECUTE_ACTION(cnotAPPROVED)
    End Sub

    Protected Sub EXECUTE_ACTION(ByVal vEsTatE As Integer)
        Using dbEntities As New dbRMS_JIEntities

            'Dim RadAsyncUpload1 As New RadAsyncUpload

            Dim duracion = 0
            Dim Err As Boolean = False

            Dim tbl_rutas_tipo_doc As New DataTable
            Dim strComment As String = ""
            Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
            Try

                Dim tbl_AppOrden As New DataTable
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc)
                'Check_NewFile_FileUploaded() 'Uploading New Files here

                'FileUploaded_UpdatedFiles() 'Uploading Files here

                '******This has to change for Stand BY app*********************
                Dim idRuta = tbl_AppOrden.Rows(0)("id_ruta_next").ToString
                Dim idNextRol = tbl_AppOrden.Rows(0)("id_role_next").ToString
                Dim idNextUserID = tbl_AppOrden.Rows(0)("id_usuario_next").ToString
                Dim idRoute As Integer = 0
                idRoute = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value).Rows(0).Item("id_ruta").ToString 'current route
                Dim id_tipoDoc = tbl_AppOrden.Rows(0).Item("id_tipoDocumento")
                Dim id_app_documento = tbl_AppOrden.Rows(0).Item("id_app_Documento")
                lblerr_user.Text = ""

                If idRuta = -1 Then 'Just for the last step

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                    Dim tbl_Doc As DataTable
                    Dim Bool_pendingDoc As Boolean = False
                    Dim str_pendingDoc As String = ""
                    Dim i As Integer = 0
                    Dim strPart1 As String
                    Dim strPart2 As String
                    Dim Bool_find As Boolean = False

                    tbl_Doc = clss_approval.get_Doc_support_Route_Pending(CType(id_tipoDoc, Integer), Me.HiddenField1.Value)

                    For Each dtRow As DataRow In tbl_Doc.Rows

                        If dtRow("requeridoFin") = "SI" Then


                            'For Each item As RadListBoxItem In rdListBox_files.Items

                            '    If CType(item.Value, Integer) = dtRow("id_doc_soporte") Then
                            '        Bool_find = True
                            '    End If

                            'Next

                            'For i = 0 To Me.ListBox_file.Items.Count - 1
                            '    If CType(Me.ListBox_file.Items(i).Value, Integer) = dtRow("id_doc_soporte") Then
                            '        Bool_find = True
                            '    End If
                            'Next

                            If Not Bool_find Then
                                Bool_pendingDoc = True
                                str_pendingDoc = """" & dtRow("nombre_documento") & " """", "
                                i += 1
                            End If

                            Bool_find = False

                        End If

                    Next

                    If Bool_pendingDoc Then
                        If i > 1 Then
                            strPart1 = "s"
                            strPart2 = "are"
                        Else
                            strPart1 = ""
                            strPart2 = "is"
                        End If
                        str_pendingDoc = str_pendingDoc.Substring(0, str_pendingDoc.Trim.Length - 1)
                        lblerr_user.Text = String.Format("The document{0}: {1} {2} required. Please attached it before completed this approval proccess", strPart1, str_pendingDoc, strPart2)
                        Me.lblerr_user.Visible = True
                        Err = True
                    End If

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                End If
                clss_approval.get_ta_DocumentosINFO(Me.HiddenField1.Value)
                Dim Tool_code As String = clss_approval.get_ta_DocumentosInfoFIELDS("tool_code", "id_documento", Me.HiddenField1.Value)


                'vEsTatE = 1000

                If Err = False Then


                    Select Case vEsTatE


                        Case cAPPROVED '*****************DOCUMENTO APROBADO***************************
                            '****************************OBTENIENDO LA PROXIMA RUTA***********

                            Dim tbl_AppOrderO As New DataTable

                            'If came from StandBy State, We need to retur n to the ROL originator if is required**********************
                            Dim riniciarRuta = Convert.ToBoolean(clss_approval.get_ta_DocumentosInfoFIELDS("reiniciar_ruta_aprobacion", "id_documento", Me.HiddenField1.Value))
                            Dim devolverAprobadorAnterior = Convert.ToBoolean(clss_approval.get_ta_DocumentosInfoFIELDS("devolver_aprobador_anterior", "id_documento", Me.HiddenField1.Value))
                            If clss_approval.get_ta_DocumentosInfoFIELDS("id_estadoDoc", "id_documento", Me.HiddenField1.Value) = cSTANDby And riniciarRuta = False And devolverAprobadorAnterior = False Then

                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value) ' To get the Max ORder values to make the same step again

                                If tbl_AppOrderO.Rows.Count > 0 Then

                                    '*******************************************check this one************************************************
                                    '****************Getting the values of the Max Order Again to return the approve**************************
                                    idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                    idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                    idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'Return to de user that put the document in estand by status
                                    '*****************************Getting the new values of the Max Order Again*******************************

                                End If

                            End If


                            tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                            If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                            End If

                            Dim docState As Integer
                            If idRuta = -1 Then ' there is not more steps
                                docState = cCOMPLETED
                            Else
                                docState = cAPPROVED
                            End If

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                'Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)

                                SaveComment(id_app_documento, docState, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                If idRuta = -1 Then ' there is not more steps
                                    'If you have to do something, do here....

                                    clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                    clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                    If clss_approval.save_ta_documento() <> -1 Then

                                        '***************Ver la categoria y ver si aplica registro ambiental**********************
                                        If clss_approval.get_enviromentalDoc(Me.HiddenField1.Value) = 1 Then

                                            clss_approval.set_ta_documento_ambiental(0)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_documento", Me.HiddenField1.Value, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_estado", cPENDING, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("observacion", clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_doc", "id_documento", Me.HiddenField1.Value), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            'clss_approval.set_ta_documento_ambientalFIELDS("fecha_aprobado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_usuario_creo", Me.Session("E_IdUser"), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("fecha_creado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_tipoApp_Environmental", 0, "id_documento_ambiental", clss_approval.id_documento_ambiental)

                                            If clss_approval.save_ta_documento_ambiental() <> -1 Then
                                            End If

                                        End If

                                        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)


                                        'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '    '**************  Estatus 3 *********** Approved***********************************************************
                                        '    Dim result = clss_approval.TimeSheet_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '    '**************  Estatus 3 *********** Approved***********************************************************

                                        '    If result <> -1 Then
                                        '        '*********************************OPEN****************************************
                                        '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '        Else 'Error mandando Email
                                        '        End If
                                        '        '*********************************OPEN****************************************
                                        '    End If

                                        'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '    '**************  Estatus 3 *********** Approved***********************************************************
                                        '    Dim result = clss_approval.Deliverable_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '    '**************  Estatus 3 *********** Approved***********************************************************

                                        '    If result <> -1 Then

                                        '        '*********************************APPROVED****************************************
                                        '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '        Else 'Error mandando Email
                                        '        End If
                                        '        '*********************************APPROVED****************************************

                                        '    End If



                                        'Else 'No tool related to this approval

                                        '***************Ver la categoria y ver si aplica registro ambiental*****************************************
                                        '********************************COMPLETED DOCUMENT*********************************************************
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                        Else 'Error mandando Email
                                        End If
                                        ' ********************************COMPLETED DOCUMENT*********************************************************

                                        'End If



                                    Else 'Error Saving Docs


                                    End If


                                Else 'Yes there is more steps


                                    Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                    Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                    strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                    clss_approval.set_ta_AppDocumento(0) 'New Record
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
                                    'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role

                                    Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                    If id_appdocumento <> -1 Then
                                        '********************************************************************************
                                        '***********************COPIANDO LOS ARCHIVOS A LA NUEVA VERSION***************
                                        '*************************NEW version change, just we goint to save the files required by the user***************************
                                        'sql = "INSERT INTO ta_archivos_documento SELECT " & id_appdocumento & " as id_App_Documento, archivo, id_doc_soporte FROM ta_archivos_documento WHERE id_App_Documento= " & Me.lblIDocumento.Text
                                        'dm.SelectCommand.CommandText = sql
                                        'dm.SelectCommand.ExecuteNonQuery()
                                    Else 'Error Saving
                                    End If


                                    '' 2 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))
                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 2 'In Approved Process

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then
                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************
                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************APPROVED NEXT STEP****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************APPROVED NEXT STEP****************************************

                                    'End If
                                    '' 2 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 2 *********** Approved In Process ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 2 ***********  Approved In Process***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 2 *********** Approval in Process ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1021, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If

                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
                                    Else 'Error mandando Email
                                    End If

                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    'If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(CType(Me.lblIDocumento.Text, Integer), Integer), id_viaje)) Then
                                    'Else 'Error mandando Email
                                    'End If
                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If




                                End If

                            Else 'Error saving the estep



                            End If 'clss_approval.save_ta_AppDocumento()

                 '****************************************END APPROVED***************************************************

           '****************NO APROBADO ahí se termina el proceso**************************
           '**************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********

                        Case cnotAPPROVED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cnotAPPROVED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(id_app_documento, cnotAPPROVED, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                    'Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)


                                    ' 3 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************NOT APPROVED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************NOT APPROVED ****************************************
                                    'End If
                                    ' 3 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 4 *********** NOT Approved***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 4 *********** NOT Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(id_app_documento, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(id_app_documento, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    Else 'No tool related to this approval

                                        '*********************************APPROVED NEXT STEP****************************************
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 11111, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                        Else 'Error mandando Email
                                        End If
                                        '*********************************APPROVED NEXT STEP****************************************

                                    End If


                                Else 'Error

                                End If


                            Else 'Error happened
                            End If

                        '****************************************FIN***************************************************

                         '********************CANCELADO FIN DEL DOCUMENTO (ACTUALIZAR campo complet a SI EN ta_documentos )
                         '****************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********
                        Case cCANCELLED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cCANCELLED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(id_app_documento, cCANCELLED, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                    'Save_NewFiles(CType(id_app_documento, Integer), Tool_code)


                                    '' 4 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then
                                    '    'Update Here the Time Sheet Status

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************CANCELLED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************CANCELLED****************************************
                                    'End If

                                    '' 4 - *******************************TOOLS TIME SHEET**********************************


                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 6 *********** Cancelled  ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 6 ***********  Cancelled ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(id_app_documento, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 6 *********** Cancelled ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 6 *********** Cancelled ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If


                                Else 'Error
                                End If


                            Else  'Error
                            End If


                        Case cSTANDby
                            '*********************************************************************************

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then


                                SaveComment(id_app_documento, cSTANDby, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                'Save_NewFiles(CType(id_app_documento, Integer), Tool_code)


                                Dim tbl_AppOrderO As New DataTable
                                'tbl_AppOrderO = clss_approval.get_ta_AppDocumento_byOrden(Me.HiddenField1.Value, 0) ' To get the Rol originator Problem when repeat
                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MIN(Me.HiddenField1.Value) 'To get the info on the min step
                                'To Create a New APP to the originator in Stand by state

                                '****************Getting the new values of the originator**************************
                                idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'The user who applied as originator from this Approval procc
                                '****************Getting the new values of the originator**************************

                                tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                                If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                    duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                                End If

                                Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                clss_approval.set_ta_AppDocumento(0) 'New Record in stanb By
                                clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0) 'IdRutaOriginator
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_app_documento", 0) 'Pending Step
                                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step '.Replace("'", "''")
                                'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0) 'IdUSerORiginator
                                clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) ' idrolORiginator

                                Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                If id_appdocumento <> -1 Then

                                    '' 5 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then


                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 5 'Observation Pending

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))


                                    '        'Update Here the Time Sheet Status
                                    '        '*********************************OPEN****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************OPEN****************************************

                                    '    End Using


                                    'Else

                                    '    '*********************************STAND BY****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************STAND BY****************************************

                                    'End If


                                    '' 5 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If

                                End If

                            Else 'Error

                            End If


                    End Select


                    Me.Response.Redirect("~/administrativo/frm_viajes")


                End If

            Catch ex As Exception
                lblerr_user.Text = String.Format("An error was found in the action: {0} ", ex.Message)
                Me.lblerr_user.Visible = True
                Me.btn_Approved.Enabled = False
                Me.btn_NotApproved.Enabled = False
                'Me.btn_Cancelled.Enabled = False
                Me.btn_STandBy.Enabled = False
                Me.btn_Completed.Enabled = False

            End Try
        End Using

    End Sub

    Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String)

        Dim strComment As String
        If Trim(Comment).Length = 0 Then
            strComment = "--No Comments--"
        Else
            strComment = Comment
        End If

        clss_approval.set_ta_comentariosDoc(0) 'New Record
        clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", idApp, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByProcess, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_usuario", Me.Session("E_IdUser"), "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Trim, "id_comment", 0) '.Replace("  ", "")

        If clss_approval.save_ta_comentariosDoc() = -1 Then
            'Error do somenthing

        End If


    End Sub

    Private Sub rbn_zona_legalizacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_zona_legalizacion.SelectedIndexChanged
        calcularPerDiem()
    End Sub

    Private Sub btn_guardar_legalizacion_Click(sender As Object, e As EventArgs) Handles btn_guardar_legalizacion.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)
                If es_Edicion = 1 Then
                    Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                    Dim viajePermisos = dbEntities.tme_solicitud_viaje_permisos.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()
                    If viajePermisos IsNot Nothing Then
                        viajePermisos.editar_legalizacion = False
                        dbEntities.Entry(viajePermisos).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If
                End If

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using

        Catch ex As Exception
            Dim err = ex.Message
        End Try
    End Sub

    Private Sub btn_ajustar_valores_Click(sender As Object, e As EventArgs) Handles btn_ajustar_valores.Click

        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_Viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_Viaje)

                Dim fecha = Me.dt_fecha.SelectedDate
                Dim anio = fecha?.Year
                Dim mes = fecha?.Month


                Dim tasaSer = dbEntities.tme_tasa_ser.Where(Function(p) p.anio = anio And p.id_mes = mes).FirstOrDefault()
                Dim fechaLegalizacion = viaje.fecha_tasa_ser

                If viaje.fecha_tasa_ser IsNot Nothing Then
                    If fechaLegalizacion.Value.Month <> tasaSer.id_mes Or fechaLegalizacion.Value.Year <> tasaSer.anio Then

                        Dim legalizaciones = dbEntities.tme_solicitud_viaje_legalizacion.Where(Function(p) p.id_viaje = id_Viaje And p.id_tipo_legalizacion = 4 And p.monto_alimentacion > 0).ToList()


                        If legalizaciones.Count() > 0 Then
                            For Each item In legalizaciones
                                item.valor_perdiem = (item.valor_perdiem / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.monto_alimentacion = (item.monto_alimentacion / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.descuento_alimentacion = (item.descuento_alimentacion / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.valor_descuento_desayuno = (item.valor_descuento_desayuno / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.valor_descuento_almuerzo = (item.valor_descuento_almuerzo / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.valor_descuento_cena = (item.valor_descuento_cena / item.tasa_ser) * Me.txt_tasa_ser.Value
                                item.monto_total = If(item.monto_alimentacion Is Nothing, 0, item.monto_alimentacion) + If(item.monto_alojamiento Is Nothing, 0, item.monto_alojamiento) - If(item.descuento_alimentacion Is Nothing, 0, item.descuento_alimentacion)
                                item.tasa_ser = Me.txt_tasa_ser.Value

                                dbEntities.Entry(item).State = Entity.EntityState.Modified
                                dbEntities.SaveChanges()
                            Next
                        End If

                        viaje.fecha_tasa_ser = fecha
                        viaje.tasa_ser = Me.txt_tasa_ser.Value
                        dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If
                End If
                fillGridLegalizacion()
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "cerrarModalTasaSer()", True)

            End Using
        Catch ex As Exception
            Dim err = ex.Message
        End Try



    End Sub


End Class