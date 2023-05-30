Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_viajesAD
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Public numViajes As Integer
    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
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
            'Me.rt_hora_salida.TimeView.TimeFormat = "HH:mm"
            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
            LoadLista()
            fillGrid()
            fillGridAlojamiento()
            loadMunicipios(0)
        End If

    End Sub

    Private Function validarTotalDias() As Boolean
        Dim habilitarViaje = Convert.ToBoolean(Me.habilitar_registro_viaje.Value)
        If habilitarViaje = True Then
            Return False
        Else
            If Session("dtItinerario") IsNot Nothing Then
                dtItinerario = Session("dtItinerario")
            Else
                createdtcolums()
            End If

            Dim fecha_viajeDt As Date
            Dim fecha_viajeRe As Date
            Dim numItinerario = 0
            For Each row As DataRow In dtItinerario.Rows

                fecha_viajeDt = row("fecha_viaje")
                If numItinerario = 0 Then
                    fecha_viajeRe = fecha_viajeDt
                End If
                If fecha_viajeDt < fecha_viajeRe Then
                    fecha_viajeRe = fecha_viajeDt
                End If
                numItinerario += numItinerario + 1
            Next
            Dim bloquearRegistro = False
            Using dbEntities As New dbRMS_JIEntities
                Dim fechaActual = fecha_viajeRe
                Dim mes = fechaActual.Month

                Dim currentMonth As New DateTime(fechaActual.Year, mes, 1)
                Dim ending As DateTime = currentMonth.AddMonths(1)

                Dim daysNotLAb = 0

                While currentMonth < ending
                    If currentMonth.DayOfWeek = DayOfWeek.Saturday Or currentMonth.DayOfWeek = DayOfWeek.Sunday Then
                        daysNotLAb += 1
                    End If
                    currentMonth = currentMonth.AddDays(1)
                End While

                Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                Dim festivos = dbEntities.tme_excepcion_ts.Where(Function(p) p.anio = fechaActual.Year And p.id_mes = mes).ToList()
                If festivos.Count() > 0 Then
                    daysNotLAb += festivos.Count()
                    currentMonth = currentMonth.AddDays(festivos.Count())
                End If
                Dim viajesMes = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_usuario = idUsuario And p.fecha_inicio_viaje.Value.Year = fechaActual.Year And p.fecha_inicio_viaje.Value.Month = mes And p.anulado = False).ToList()
                Dim diasMes = System.DateTime.DaysInMonth(fechaActual.Year, mes)
                Dim diasHabiles = diasMes - daysNotLAb


                If viajesMes.Count() >= diasHabiles / 2 Then
                    bloquearRegistro = True
                End If
                numViajes = viajesMes.Count()
            End Using

            Return bloquearRegistro
        End If



    End Function
    Sub loadMunicipios(ByVal tipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim municipios As List(Of t_municipios)
            If tipo = 0 Then
                Dim idDepto = Convert.ToInt32(Me.cmb_departamento_origen.SelectedValue)
                municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()

                Me.cmb_municipio_origen.DataSourceID = ""
                Me.cmb_municipio_origen.DataSource = municipios
                Me.cmb_municipio_origen.DataTextField = "nombre_municipio"
                Me.cmb_municipio_origen.DataValueField = "id_municipio"
                Me.cmb_municipio_origen.DataBind()

                Me.cmb_municipio_destino.DataSourceID = ""
                Me.cmb_municipio_destino.DataSource = municipios
                Me.cmb_municipio_destino.DataTextField = "nombre_municipio"
                Me.cmb_municipio_destino.DataValueField = "id_municipio"
                Me.cmb_municipio_destino.DataBind()

                Me.cmb_municipio_hotel.DataSourceID = ""
                Me.cmb_municipio_hotel.DataSource = municipios
                Me.cmb_municipio_hotel.DataTextField = "nombre_municipio"
                Me.cmb_municipio_hotel.DataValueField = "id_municipio"
                Me.cmb_municipio_hotel.DataBind()

            ElseIf tipo = 1 Then
                Dim idDepto = Convert.ToInt32(Me.cmb_departamento_origen.SelectedValue)
                municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()
                Me.cmb_municipio_origen.DataSourceID = ""
                Me.cmb_municipio_origen.DataSource = municipios
                Me.cmb_municipio_origen.DataTextField = "nombre_municipio"
                Me.cmb_municipio_origen.DataValueField = "id_municipio"
                Me.cmb_municipio_origen.DataBind()
            ElseIf tipo = 2 Then
                Dim idDepto = Convert.ToInt32(Me.cmb_departamento_destino.SelectedValue)
                municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()
                Me.cmb_municipio_destino.DataSourceID = ""
                Me.cmb_municipio_destino.DataSource = municipios
                Me.cmb_municipio_destino.DataTextField = "nombre_municipio"
                Me.cmb_municipio_destino.DataValueField = "id_municipio"
                Me.cmb_municipio_destino.DataBind()
            ElseIf tipo = 3 Then
                Dim idDepto = Convert.ToInt32(Me.cmb_departamento_hotel.SelectedValue)
                municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()
                Me.cmb_municipio_hotel.DataSourceID = ""
                Me.cmb_municipio_hotel.DataSource = municipios
                Me.cmb_municipio_hotel.DataTextField = "nombre_municipio"
                Me.cmb_municipio_hotel.DataValueField = "id_municipio"
                Me.cmb_municipio_hotel.DataBind()
            End If
        End Using
    End Sub
    Sub LoadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities


            Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            ''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("habilitar_agregar_viaje")
            dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitarViaje = ds.Tables("habilitar_agregar_viaje").Rows(0).Item(0)
            Me.habilitar_registro_viaje.Value = habilitarViaje

            If habilitarViaje = True Then
                Me.dt_fecha_viaje.MinDate = DateTime.Now
            Else
                Me.dt_fecha_viaje.MinDate = DateTime.Now.AddDays(7)
            End If


            Me.dt_fecha_llegada.MinDate = DateTime.Now
            Me.dt_fecha_salida.MinDate = DateTime.Now

            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now)
            Dim Departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id).OrderBy(Function(p) p.nombre_departamento).ToList()
            Me.cmb_departamento_origen.DataSourceID = ""
            Me.cmb_departamento_origen.DataSource = Departamentos
            Me.cmb_departamento_origen.DataTextField = "nombre_departamento"
            Me.cmb_departamento_origen.DataValueField = "id_departamento"
            Me.cmb_departamento_origen.SelectedValue = Departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_origen.DataBind()

            Me.cmb_departamento_destino.DataSourceID = ""
            Me.cmb_departamento_destino.DataSource = Departamentos
            Me.cmb_departamento_destino.DataTextField = "nombre_departamento"
            Me.cmb_departamento_destino.DataValueField = "id_departamento"
            Me.cmb_departamento_destino.SelectedValue = Departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_destino.DataBind()

            Me.cmb_departamento_hotel.DataSourceID = ""
            Me.cmb_departamento_hotel.DataSource = Departamentos
            Me.cmb_departamento_hotel.DataTextField = "nombre_departamento"
            Me.cmb_departamento_hotel.DataValueField = "id_departamento"
            Me.cmb_departamento_hotel.SelectedValue = Departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_hotel.DataBind()

            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)


            If usuario.tipo = "Operativo" Then
                Me.rbn_tipo_viaje.SelectedValue = 1
            ElseIf usuario.tipo = "Técnico" Then
                Me.rbn_tipo_viaje.SelectedValue = 2
            Else
                Me.tipoViaje.Visible = True
            End If


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
            'Me.rbn_tipo_viaje.DataSource = dbEntities.tme_tipo_viaje.ToList()
            'Me.rbn_tipo_viaje.DataValueField = "id_tipo_viaje"
            'Me.rbn_tipo_viaje.DataTextField = "tipo_viaje"
            'Me.rbn_tipo_viaje.DataBind()
            Dim estructura = dbEntities.tme_estructura_marcologico.Where(Function(p) p.tme_programa_marco_logico.id_programa = id And p.id_tipo_marcologico = 15).Select(Function(p) New With
                                                                                                                                          {Key .id_estructura_marcologico = p.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica = p.codigo & " - " & p.descripcion_logica,
                                                                                                                                           Key .id_estructura_marcologico_2 = p.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica_padre = p.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                                           Key .id_estructura_marcologico_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica_padre_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.tme_estructura_marcologico2.descripcion_logica
                                                                                                                                          }).ToList()
            Me.grd_componente.DataSource = estructura
            Me.grd_componente.DataBind()

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
        End If
    End Sub
    Protected Sub btn_agregar_itinerario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_itinerario.Click
        Try
            If Session("dtItinerario") IsNot Nothing Then
                dtItinerario = Session("dtItinerario")
            Else
                createdtcolums()
            End If

            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio

            dtItinerario.Rows.Add(idunique, Me.dt_fecha_viaje.SelectedDate, Me.rt_hora_salida.SelectedDate.Value.ToShortTimeString(),
                                  Me.cmb_departamento_origen.Text & " - " & Me.cmb_municipio_origen.Text,
                                  Me.cmb_departamento_destino.Text & " - " & Me.cmb_municipio_destino.Text,
                                  Convert.ToInt32(Me.cmb_municipio_origen.SelectedValue), Convert.ToInt32(Me.cmb_municipio_destino.SelectedValue),
                                  If(Me.rbn_transporte_aereo.SelectedValue = "0", False, True), If(Me.rbn_vehiculo_proyecto.SelectedValue = "0", False, True),
                                  If(Me.rbn_transporte_fluvial.SelectedValue = "0", False, True), If(Me.rbn_servicio_publico.SelectedValue = "0", False, True),
                                  If(Me.rbn_transporte_aereo.SelectedValue = "0", "NO", "SÍ"), If(Me.rbn_vehiculo_proyecto.SelectedValue = "0", "NO", "SI"),
                                  If(Me.rbn_transporte_fluvial.SelectedValue = "0", "NO", "SÍ"), If(Me.rbn_servicio_publico.SelectedValue = "0", "NO", "SÍ"), Me.txt_linea_aerea.Text,
                                    Me.txt_vehiculo_proyecto.Text, Me.txt_transporte_fluvial.Text, Me.txt_servicio_publico.Text)
            dtItinerario.DefaultView.Sort = "fecha_viaje ASC"
            Session("dtItinerario") = dtItinerario
            Me.txt_vehiculo_proyecto.Text = String.Empty
            Me.txt_transporte_fluvial.Text = String.Empty
            Me.txt_servicio_publico.Text = String.Empty
            Me.dt_fecha_viaje.Clear()
            Me.rt_hora_salida.Clear()
            'Me.cmb_departamento_origen.SelectedValue = String.Empty
            'Me.cmb_departamento_destino.SelectedValue = String.Empty
            'Me.cmb_municipio_origen.SelectedValue = String.Empty
            'Me.cmb_municipio_destino.SelectedValue = String.Empty
            'LoadLista()
            'loadMunicipios(0)
            'Me.txt_ciudad_origen.Text = String.Empty
            'Me.txt_ciudad_destino.Text = String.Empty
            Me.txt_linea_aerea.Text = String.Empty
            Me.rbn_transporte_aereo.SelectedValue = Nothing
            Me.rbn_vehiculo_proyecto.SelectedValue = Nothing
            Me.rbn_transporte_fluvial.SelectedValue = Nothing
            Me.rbn_servicio_publico.SelectedValue = Nothing
            Me.servicio_pubiclo.Visible = False
            Me.vehiculo_proyecto.Visible = False
            Me.transporte_fluvial.Visible = False
            Me.transporte_aereo.Visible = False
            fillGrid()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub


    Protected Sub grd_alojamiento_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_alojamiento.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_viaje_hotel").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_viaje_hotel").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_viaje_hotel").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub

    Protected Sub grd_itinerario_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_itinerario.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_viaje_itinerario").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_viaje_itinerario").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_viaje_itinerario").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub btn_agregar_alojamiento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_alojamiento.Click
        Try
            If Session("dtAlojamiento") IsNot Nothing Then
                dtAlojamiento = Session("dtAlojamiento")
            Else
                createdtcolumsAlojamiento()
            End If

            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio
            dtAlojamiento.Rows.Add(idunique, Me.dt_fecha_llegada.SelectedDate, Me.dt_fecha_salida.SelectedDate, Me.cmb_departamento_hotel.Text & " - " & Me.cmb_municipio_hotel.Text,
                                   Convert.ToInt32(Me.cmb_municipio_hotel.SelectedValue), Me.txt_hotel.Text)
            Session("dtAlojamiento") = dtAlojamiento
            Me.cmb_departamento_hotel.SelectedValue = String.Empty
            Me.cmb_municipio_hotel.SelectedValue = String.Empty
            'LoadLista()
            'loadMunicipios(0)
            Me.txt_hotel.Text = String.Empty
            Me.dt_fecha_llegada.Clear()
            Me.dt_fecha_salida.Clear()
            fillGridAlojamiento()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    Protected Sub delete_itinerario(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.tipo.Value = 1
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub delete_alojamiento(sender As Object, e As EventArgs)
        Me.tipo.Value = 2
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Dim tipo_elemento_eliminar = Convert.ToInt32(Me.tipo.Value)
        If tipo_elemento_eliminar = 1 Then
            If Session("dtItinerario") IsNot Nothing Then
                dtItinerario = Session("dtItinerario")
            Else
                createdtcolums()
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            If Session("dtAlojamiento") IsNot Nothing Then
                dtAlojamiento = Session("dtAlojamiento")
            Else
                createdtcolumsAlojamiento()
            End If
        End If


        Dim identity_ = Convert.ToString(Me.identity.Value)

        If tipo_elemento_eliminar = 1 Then
            Dim foundRow As DataRow() = dtItinerario.Select("id_viaje_itinerario = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then
                dtItinerario.Rows.Remove(foundRow(0))
                Session("dtItinerario") = dtItinerario
                fillGrid()
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            Dim foundRow As DataRow() = dtAlojamiento.Select("id_viaje_hotel = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then
                dtAlojamiento.Rows.Remove(foundRow(0))
                Session("dtAlojamiento") = dtAlojamiento
                fillGridAlojamiento()
            End If
        End If
        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Private Sub rbn_servicio_publico_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_servicio_publico.SelectedIndexChanged
        Me.servicio_pubiclo.Visible = False
        If Convert.ToInt32(Me.rbn_servicio_publico.SelectedValue) = 1 Then
            Me.servicio_pubiclo.Visible = True
        End If
    End Sub

    Private Sub rbn_transporte_aereo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_transporte_aereo.SelectedIndexChanged
        Me.transporte_aereo.Visible = False
        If Convert.ToInt32(Me.rbn_transporte_aereo.SelectedValue) = 1 Then
            Me.transporte_aereo.Visible = True
        End If
    End Sub

    Private Sub rbn_transporte_fluvial_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_transporte_fluvial.SelectedIndexChanged
        Me.transporte_fluvial.Visible = False
        If Convert.ToInt32(Me.rbn_transporte_fluvial.SelectedValue) = 1 Then
            Me.transporte_fluvial.Visible = True
        End If
    End Sub

    Private Sub rbn_vehiculo_proyecto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_vehiculo_proyecto.SelectedIndexChanged
        Me.vehiculo_proyecto.Visible = False
        If Convert.ToInt32(Me.rbn_vehiculo_proyecto.SelectedValue) = 1 Then
            Me.vehiculo_proyecto.Visible = True
        End If
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            If validarTotalDias() = False Then
                If guardar(True) Then

                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                End If

            Else
                Me.mensajeNumeroDiasHabiles.Visible = True
            End If



        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Public Function guardarDocumento(ByVal viaje As tme_solicitud_viaje, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2042
        Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)
        Dim id_tipoDoc = tblUserApprovalTimeSheet.Rows(0).Item("id_tipoDocumento")


        Dim descripcion = String.Format("Solicitud de viaje {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, viaje.fecha_inicio_viaje)
        Dim err As Boolean = False
        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

        clss_approval.set_ta_documento(0) 'Set new Record
        clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", id_tipoDoc, "id_documento", 0)
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

        tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipoDoc, 0) 'First Step
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
            tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipoDoc, 1) 'Next Step
            Dim NextUser As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
            Dim idNextRol As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
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


            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1020, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_TRAVEL(CType(id_appdocumento, Integer), viaje.id_viaje)) Then
            Else 'Error mandando Email
            End If
        End If

        Return id_documento

    End Function

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
    Private Sub cmb_departamento_origen_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_origen.SelectedIndexChanged
        loadMunicipios(1)
    End Sub

    Private Sub cmb_departamento_destino_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_destino.SelectedIndexChanged
        loadMunicipios(2)
    End Sub
    Private Sub cmb_departamento_hotel_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_hotel.SelectedIndexChanged
        loadMunicipios(3)
    End Sub

    Private Sub btn_guardar_2_Click(sender As Object, e As EventArgs) Handles btn_guardar_2.Click
        Try

            If validarTotalDias() = False Then
                If guardar(False) Then
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                End If
            Else
                Me.mensajeNumeroDiasHabiles.Visible = True
            End If




        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Public Function guardar(ByVal enviarAprobacion As Boolean) As Boolean
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

            Dim totalComponentes = 0

            For Each row In Me.grd_componente.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                    If idComponente.Checked = True Then
                        totalComponentes += 1
                    End If
                End If
            Next

            If totalComponentes > 0 Then
                Using dbEntities As New dbRMS_JIEntities
                    Dim id_viaje = Convert.ToInt32(Me.idviaje.Value)
                    Dim viaje = New tme_solicitud_viaje
                    viaje.id_usuario = Me.Session("E_IdUser").ToString()
                    Dim usuario = dbEntities.t_usuarios.Find(viaje.id_usuario)
                    If id_viaje = 0 Then
                        usuario.habilitar_agregar_viaje = False
                        dbEntities.Entry(usuario).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()

                        'Dim codigoSolicitud = dbEntities.Database.SqlQuery(Of Integer)("SELECT NEXT VALUE FOR SequenceSolicitudViaje")
                        viaje.fecha_crea = DateTime.Now
                        'viaje.fecha_inicio_viaje = Me.dt_fecha_inicio.SelectedDate
                        'viaje.fecha_fin_viaje = Me.dt_fecha_fin.SelectedDate
                        viaje.numero_contacto = Me.txt_numero_contacto.Text
                        viaje.motivo_viaje = Me.txt_motivo_viaje.Text


                        Dim consecutivoViaje = dbEntities.tme_solicitud_viaje.Count()


                        viaje.codigo_solicitud_viaje = "V-JI-" & consecutivoViaje + 1
                        viaje.id_tipo_viaje = Convert.ToInt32(Me.rbn_tipo_viaje.SelectedValue)
                        viaje.id_cargo = usuario.id_job_title
                        viaje.id_sub_region = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                        dbEntities.tme_solicitud_viaje.Add(viaje)
                        dbEntities.SaveChanges()

                        Dim fecha_viaje As Date
                        Dim hora_salida As String
                        Dim ciudad_origen As String
                        Dim ciudad_destino As String
                        Dim id_municipio_origen As Integer
                        Dim id_municipio_destino As Integer
                        Dim requiere_transporte_aereo As Boolean
                        Dim requiere_vehiculo_proyecto As Boolean
                        Dim requiere_transporte_fluvial As Boolean
                        Dim requiere_servicio_publico As Boolean
                        Dim transporte_aereo As String
                        Dim vehiculo_proyecto As String
                        Dim transporte_fluvial As String
                        Dim servicio_publico As String

                        Dim fecha_llegada As Date
                        Dim fecha_salida As Date
                        Dim id_municipio_hotel As Integer
                        Dim hotel As String


                        For Each row As DataRow In dtItinerario.Rows
                            Dim viajeItinerario = New tme_solicitud_viaje_itinerario
                            viajeItinerario.id_viaje = viaje.id_viaje

                            fecha_viaje = row("fecha_viaje")
                            hora_salida = row("hora_salida")
                            ciudad_origen = row("ciudad_origen")
                            ciudad_destino = row("ciudad_destino")
                            id_municipio_origen = row("id_municipio_origen")
                            id_municipio_destino = row("id_municipio_destino")
                            requiere_transporte_aereo = row("requiere_transporte_aereo")
                            requiere_vehiculo_proyecto = row("requiere_vehiculo_proyecto")
                            requiere_transporte_fluvial = row("requiere_transporte_fluvial")
                            requiere_servicio_publico = row("requiere_servicio_publico")
                            transporte_aereo = row("transporte_aereo")
                            vehiculo_proyecto = row("vehiculo_proyecto")
                            transporte_fluvial = row("transporte_fluvial")
                            servicio_publico = row("servicio_publico")

                            viajeItinerario.fecha_viaje = fecha_viaje
                            viajeItinerario.hora_salida = hora_salida
                            viajeItinerario.requiere_linea_aerea = requiere_transporte_aereo
                            viajeItinerario.requiere_vehiculo_proyecto = requiere_vehiculo_proyecto
                            viajeItinerario.requiere_transporte_fluvial = requiere_transporte_fluvial
                            viajeItinerario.requiere_servicio_publico = requiere_servicio_publico
                            viajeItinerario.observaciones_vehiculo_proyecto = vehiculo_proyecto
                            viajeItinerario.observaciones_servicio_publico = servicio_publico
                            viajeItinerario.observaciones_transporte_fluvial = transporte_fluvial
                            viajeItinerario.linea_aerea = transporte_aereo
                            viajeItinerario.id_municipio_origen = id_municipio_origen
                            viajeItinerario.id_municipio_destino = id_municipio_destino

                            dbEntities.tme_solicitud_viaje_itinerario.Add(viajeItinerario)
                            dbEntities.SaveChanges()
                        Next

                        For Each row As DataRow In dtAlojamiento.Rows
                            Dim viajeHotel = New tme_solicitud_viaje_hotel
                            viajeHotel.id_viaje = viaje.id_viaje
                            fecha_llegada = row("fecha_llegada")
                            fecha_salida = row("fecha_salida")
                            id_municipio_hotel = row("id_municipio")
                            hotel = row("hotel")

                            viajeHotel.hotel = hotel
                            viajeHotel.id_municipio = id_municipio_hotel
                            viajeHotel.fecha_salida = fecha_salida
                            viajeHotel.fecha_llegada = fecha_llegada

                            dbEntities.tme_solicitud_viaje_hotel.Add(viajeHotel)
                            dbEntities.SaveChanges()
                        Next

                        For Each row In Me.grd_componente.Items
                            If TypeOf row Is GridDataItem Then
                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                                If idComponente.Checked = True Then
                                    Dim idEstructura As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                                    Dim oMarco = New tme_solicitud_viaje_marco_logico
                                    oMarco.id_viaje = viaje.id_viaje
                                    oMarco.id_estructura_marcologico = idEstructura
                                    dbEntities.tme_solicitud_viaje_marco_logico.Add(oMarco)
                                    dbEntities.SaveChanges()
                                End If
                            End If
                        Next

                        Dim viajeItinerarioList = dbEntities.tme_solicitud_viaje_itinerario.Where(Function(p) p.id_viaje = viaje.id_viaje).ToList()
                        If viajeItinerarioList.Count() > 0 Then
                            viaje.fecha_inicio_viaje = viajeItinerarioList.OrderBy(Function(p) p.fecha_viaje).FirstOrDefault().fecha_viaje
                            viaje.fecha_fin_viaje = viajeItinerarioList.OrderByDescending(Function(p) p.fecha_viaje).FirstOrDefault().fecha_viaje
                        End If
                        dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                        If enviarAprobacion Then

                            Dim id_categoriaAPP = 2042
                            Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
                            Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)

                            If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                                Me.lblerr_user.Text = "El viaje " & viaje.codigo_solicitud_viaje & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de viajes, contáctese con el administrador."
                                Me.lblerr_user.Visible = True
                                guardar = False
                                Me.idviaje.Value = viaje.id_viaje
                            Else
                                Dim id_documento = guardarDocumento(viaje, usuario)
                                guardar = True
                            End If

                            'guardarRelacionDocumento(id_documento, viaje.id_viaje)

                        Else
                            guardar = True
                        End If
                    Else
                        viaje = dbEntities.tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()

                        Dim id_categoriaAPP = 2042
                        Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
                        Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)

                        If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                            Me.lblerr_user.Text = "El viaje " & viaje.codigo_solicitud_viaje & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de viajes, contáctese con el administrador."
                            Me.lblerr_user.Visible = True
                            guardar = False
                            Me.idviaje.Value = viaje.id_viaje
                        Else
                            Dim id_documento = guardarDocumento(viaje, usuario)
                            guardar = True
                        End If


                    End If


                End Using
            Else
                Me.lblerr_user.Text = "Seleccione los componentes!"
                guardar = False
            End If


        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Function

    Sub guardarRelacionDocumento(ByVal idDocumento As Integer, ByVal idViaje As Integer)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim documentoViaje As New ta_documento_viaje
                documentoViaje.id_viaje = idViaje
                documentoViaje.id_documento = idDocumento
                dbEntities.ta_documento_viaje.Add(documentoViaje)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class