Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Globalization
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_viajeEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_EDIT"
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
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2
    Dim ListItemsDeleteBD As New List(Of Integer)
    Dim ListItemsDeleteBDHotel As New List(Of Integer)

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
            Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
            Me.esEdicion.Value = es_Edicion
            Me.idViaje.Value = id_viaje

            Me.rt_hora_salida.TimeView.TimeFormat = "HH:mm"
            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
            Session.Remove("ListItemsDeleteBD")
            Session.Remove("ListItemsDeleteBDHotel")
            LoadLista()
            LoadData(id_viaje)
            fillGrid()
            fillGridAlojamiento()
            loadMunicipios(0)
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

            Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            ''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("habilitar_agregar_viaje")
            dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitarViaje = ds.Tables("habilitar_agregar_viaje").Rows(0).Item(0)
            Me.habilitar_registro_viaje.Value = habilitarViaje


            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now)
            Me.txt_motivo_viaje.Text = viaje.motivo_viaje
            Me.txt_numero_contacto.Text = viaje.numero_contacto
            If viaje.id_sub_region IsNot Nothing Then
                Me.cmb_sub_Region.SelectedValue = viaje.id_sub_region
            End If
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

            Dim componentes = viaje.tme_solicitud_viaje_marco_logico.ToList()
            If componentes.Count() > 0 Then
                For Each row In Me.grd_componente.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        Dim idComponente As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                        If componentes.Where(Function(p) p.id_estructura_marcologico = idComponente).ToList().Count() > 0 Then
                            subR.Checked = True
                        End If
                    End If
                Next
            End If

            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            Dim viajeDetalle = dbEntities.vw_tme_solicitud_viaje.FirstOrDefault(Function(p) p.id_viaje = viaje.id_viaje)


            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)


            If viaje.t_usuarios.id_usuario_padre = idUser And viajeDetalle.id_documento > 0 And viajeDetalle.id_estadoDoc <> 6 And viajeDetalle.id_documento_legalizacion = 0 Then
                Me.btn_guardar_2.Visible = True
                Me.btn_guardar.Visible = False
            ElseIf viaje.id_usuario = idUser Then


                Dim intOwnerSolicitud As String() = viajeDetalle.id_usuario_app.ToString.Split(",")



                If viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).Count() > 0 And intOwnerSolicitud.Where(Function(p) p.Contains(idUser)).Count() > 0 Then

                    Dim idDoc = viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    Me.HiddenField1.Value = viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    fillGridRutaAprobacion(viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento)

                    Me.btn_guardar.Visible = False
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
                        Me.btn_guardar_2.Visible = True
                        Me.btn_guardar.Visible = False
                    End If

                End If
            ElseIf viaje.id_usuario <> idUser Then
                Me.Response.Redirect("~/administrativo/frm_viajePrint?id=" & id_viaje)
            Else
                Me.btn_guardar_2.Visible = False
                Me.btn_guardar.Visible = False
            End If



            Session("dtItinerario") = dtItinerario
            Session("dtAlojamiento") = dtAlojamiento
        End Using
    End Sub
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
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
            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id).OrderBy(Function(p) p.nombre_departamento).ToList()
            Me.cmb_departamento_origen.DataSourceID = ""
            Me.cmb_departamento_origen.DataSource = departamentos
            Me.cmb_departamento_origen.DataTextField = "nombre_departamento"
            Me.cmb_departamento_origen.DataValueField = "id_departamento"
            Me.cmb_departamento_origen.SelectedValue = departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_origen.DataBind()

            Me.cmb_departamento_destino.DataSourceID = ""
            Me.cmb_departamento_destino.DataSource = departamentos
            Me.cmb_departamento_destino.DataTextField = "nombre_departamento"
            Me.cmb_departamento_destino.DataValueField = "id_departamento"
            Me.cmb_departamento_destino.SelectedValue = departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_destino.DataBind()

            Me.cmb_departamento_hotel.DataSourceID = ""
            Me.cmb_departamento_hotel.DataSource = departamentos
            Me.cmb_departamento_hotel.DataTextField = "nombre_departamento"
            Me.cmb_departamento_hotel.DataValueField = "id_departamento"
            Me.cmb_departamento_hotel.SelectedValue = departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_hotel.DataBind()

            'Me.rbn_tipo_viaje.DataSource = dbEntities.tme_tipo_viaje.ToList()
            'Me.rbn_tipo_viaje.DataValueField = "id_tipo_viaje"
            'Me.rbn_tipo_viaje.DataTextField = "tipo_viaje"
            'Me.rbn_tipo_viaje.DataBind()

            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)

            Dim regionesUsuario = usuario.t_usuario_subregion.ToList()
            Me.cmb_sub_Region.DataSourceID = ""
            If regionesUsuario.Count() = 1 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                            New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                      Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
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

            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()
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

        Dim dv = New DataView(dtItinerario)
        dv.Sort = "fecha_viaje ASC"
        Me.grd_itinerario.DataSource = dv
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
                                    Me.txt_vehiculo_proyecto.Text, Me.txt_transporte_fluvial.Text, Me.txt_servicio_publico.Text, False, 0)
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
                                   Convert.ToInt32(Me.cmb_municipio_hotel.SelectedValue), Me.txt_hotel.Text, False, 0)
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

            If Session("ListItemsDeleteBD") IsNot Nothing Then
                ListItemsDeleteBD = Session("ListItemsDeleteBD")
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            If Session("dtAlojamiento") IsNot Nothing Then
                dtAlojamiento = Session("dtAlojamiento")
            Else
                createdtcolumsAlojamiento()
            End If

            If Session("ListItemsDeleteBDHotel") IsNot Nothing Then
                ListItemsDeleteBDHotel = Session("ListItemsDeleteBDHotel")
            End If
        End If


        Dim identity_ = Convert.ToString(Me.identity.Value)

        If tipo_elemento_eliminar = 1 Then
            Dim foundRow As DataRow() = dtItinerario.Select("id_viaje_itinerario = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then
                Dim estaBD As Boolean = foundRow(0)("esta_bd")
                Dim id_viaje_itinerario_bd = foundRow(0)("id_viaje_itinerario_bd")
                If estaBD Then
                    ListItemsDeleteBD.Add(id_viaje_itinerario_bd)
                End If

                dtItinerario.Rows.Remove(foundRow(0))
                Session("dtItinerario") = dtItinerario
                Session("ListItemsDeleteBD") = ListItemsDeleteBD
                fillGrid()
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            Dim foundRow As DataRow() = dtAlojamiento.Select("id_viaje_hotel = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then

                Dim estaBD As Boolean = foundRow(0)("esta_bd")
                Dim id_viaje_hotel = foundRow(0)("id_viaje_hotel_bd")
                If estaBD Then
                    ListItemsDeleteBDHotel.Add(id_viaje_hotel)
                End If

                dtAlojamiento.Rows.Remove(foundRow(0))
                Session("ListItemsDeleteBDHotel") = ListItemsDeleteBDHotel
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
        Using dbEntities As New dbRMS_JIEntities
            Try
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viajeDoc = dbEntities.ta_documento_viaje.Where(Function(p) p.id_viaje = id_viaje And p.reversado = True).FirstOrDefault()

                Dim idDoc = 0

                If viajeDoc IsNot Nothing Then
                    idDoc = viajeDoc.id_documento
                End If


                If idDoc = 0 Then
                    If validarFechaEnvioViaje() Then
                        guardar(True)
                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                        Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    Else
                        Me.mensajeNumeroDiasHabiles.Visible = True
                    End If
                Else
                    guardar(True)
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                End If




            Catch ex As Exception
                Dim mensaje = ex.Message
            End Try
        End Using


    End Sub
    Private Function validarFechaEnvioViaje() As Boolean
        Dim habilitarViaje = Convert.ToBoolean(Me.habilitar_registro_viaje.Value)
        If habilitarViaje = True Then
            Return True
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

            Dim fechaActual = DateTime.Now
            fechaActual = fechaActual.AddDays(6)

            If fecha_viajeRe < fechaActual Then
                Return False
            Else
                Return True
            End If



        End If



    End Function
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

            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1020, cl_user.regionalizacionCulture, id_appdocumento2)
            If (objEmail.Emailing_APPROVAL_TRAVEL(CType(id_appdocumento2, Integer), viaje.id_viaje)) Then
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
            Using dbEntities As New dbRMS_JIEntities
                guardar(False)

                Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)
                If es_Edicion = 1 Then
                    Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                    Dim viajePermisos = dbEntities.tme_solicitud_viaje_permisos.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()
                    If viajePermisos IsNot Nothing Then
                        viajePermisos.editar_solicitud = False
                        dbEntities.Entry(viajePermisos).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If
                End If


                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/administrativo/frm_viajes"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using

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

            If Session("ListItemsDeleteBD") IsNot Nothing Then
                ListItemsDeleteBD = Session("ListItemsDeleteBD")
            End If

            If Session("ListItemsDeleteBDHotel") IsNot Nothing Then
                ListItemsDeleteBDHotel = Session("ListItemsDeleteBDHotel")
            End If
            Using dbEntities As New dbRMS_JIEntities
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                'Dim codigoSolicitud = dbEntities.Database.SqlQuery(Of Integer)("SELECT NEXT VALUE FOR SequenceSolicitudViaje")
                viaje.fecha_edita = DateTime.Now
                viaje.id_usuario_edita = Me.Session("E_IdUser").ToString()

                If viaje.id_usuario <> viaje.id_usuario_edita Then
                    viaje.modificado_supervisor = True
                End If

                'viaje.id_usuario = Me.Session("E_IdUser").ToString()
                'Dim usuario = dbEntities.t_usuarios.Find(viaje.id_usuario)
                'viaje.fecha_inicio_viaje = Me.dt_fecha_inicio.SelectedDate
                'viaje.fecha_fin_viaje = Me.dt_fecha_fin.SelectedDate
                viaje.numero_contacto = Me.txt_numero_contacto.Text
                viaje.motivo_viaje = Me.txt_motivo_viaje.Text
                viaje.id_tipo_viaje = Me.rbn_tipo_viaje.SelectedValue
                viaje.id_sub_region = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                'viaje.codigo_solicitud_viaje = "V-JI-" & codigoSolicitud.Single()
                'viaje.id_cargo = usuario.id_job_title
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
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
                Dim esta_bd As Boolean

                Dim fecha_llegada As Date
                Dim fecha_salida As Date
                Dim id_municipio_hotel As Integer
                Dim hotel As String
                Dim esta_bd_hotel As Boolean


                For Each item In ListItemsDeleteBD
                    Dim oViajeItinerario = dbEntities.tme_solicitud_viaje_itinerario.Find(item)
                    dbEntities.Entry(oViajeItinerario).State = Entity.EntityState.Deleted
                    dbEntities.SaveChanges()
                Next

                For Each item In ListItemsDeleteBDHotel
                    Dim oViajeHotel = dbEntities.tme_solicitud_viaje_hotel.Find(item)
                    dbEntities.Entry(oViajeHotel).State = Entity.EntityState.Deleted
                    dbEntities.SaveChanges()
                Next


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
                    esta_bd = row("esta_bd")

                    If esta_bd = False Then
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
                    End If

                Next

                For Each row As DataRow In dtAlojamiento.Rows
                    Dim viajeHotel = New tme_solicitud_viaje_hotel
                    viajeHotel.id_viaje = viaje.id_viaje
                    fecha_llegada = row("fecha_llegada")
                    fecha_salida = row("fecha_salida")
                    id_municipio_hotel = row("id_municipio")
                    hotel = row("hotel")
                    esta_bd_hotel = row("esta_bd")

                    If esta_bd_hotel = False Then
                        viajeHotel.hotel = hotel
                        viajeHotel.id_municipio = id_municipio_hotel
                        viajeHotel.fecha_salida = fecha_salida
                        viajeHotel.fecha_llegada = fecha_llegada

                        dbEntities.tme_solicitud_viaje_hotel.Add(viajeHotel)
                        dbEntities.SaveChanges()
                    End If

                Next


                Dim compoentes = viaje.tme_solicitud_viaje_marco_logico.ToList()
                For Each row In Me.grd_componente.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        Dim idEstructura As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                        If idComponente.Checked = True Then
                            If compoentes.Where(Function(p) p.id_estructura_marcologico = idEstructura).ToList().Count() = 0 Then
                                Dim oMarco = New tme_solicitud_viaje_marco_logico
                                oMarco.id_viaje = viaje.id_viaje
                                oMarco.id_estructura_marcologico = idEstructura
                                dbEntities.tme_solicitud_viaje_marco_logico.Add(oMarco)
                                dbEntities.SaveChanges()
                            End If
                        Else
                            If compoentes.Where(Function(p) p.id_estructura_marcologico = idEstructura).ToList().Count() > 0 Then
                                Dim oMarco = compoentes.Where(Function(p) p.id_estructura_marcologico = idEstructura).FirstOrDefault()
                                dbEntities.Entry(oMarco).State = Entity.EntityState.Deleted
                                dbEntities.SaveChanges()
                            End If
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
                    If viaje.ta_documento_viaje.Where(Function(p) p.reversado Is Nothing).Count() = 0 Then

                        Dim id_categoriaAPP = 2042
                        Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
                        Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)

                        If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                            Me.lblerr_user.Text = "El viaje fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de viajes, contáctese con el administrador."
                            Me.lblerr_user.Visible = True
                            guardar = False
                        Else
                            Dim id_documento = guardarDocumento(viaje, viaje.t_usuarios)
                            guardar = True
                        End If

                    End If
                Else
                    guardar = True
                End If
            End Using

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
        guardar(False)
        EXECUTE_ACTION(cAPPROVED)
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
                            If clss_approval.get_ta_DocumentosInfoFIELDS("id_estadoDoc", "id_documento", Me.HiddenField1.Value) = cSTANDby Then

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

                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_TRAVEL(CType(id_app_documento, Integer), id_viaje)) Then
                                    Else 'Error mandando Email
                                    End If

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
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_APPROVAL_TRAVEL(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
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
End Class