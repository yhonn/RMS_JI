Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing
Imports Telerik.Web.UI.Calendar
Imports System.Globalization

Public Class frm_viaje_informe
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJES_INFO"
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
            Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
            Me.esEdicion.Value = es_Edicion

            Session.Remove("dtItinerario")
            Session.Remove("dtAlojamiento")
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

            Dim viajeDetalle = dbEntities.vw_tme_solicitud_viaje.FirstOrDefault(Function(p) p.id_viaje = viaje.id_viaje)

            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.lbl_categoria.Text = viaje.ta_documento_viaje.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            Me.lbl_codigo.Text = viaje.codigo_solicitud_viaje
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            Me.lbl_fecha_inicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            Me.lbl_fecha_finalizacion.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            Me.lbl_numero_contacto.Text = viaje.numero_contacto
            Me.lbl_motivo_viaje.Text = viaje.motivo_viaje


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


            Me.txt_resultados.Text = viaje.informe_resultado
            Me.txt_compromisos_conclusiones.Text = viaje.informe_compromiso
            Me.txt_lugares_entidades_personas.Text = viaje.lugares_entidades_personas

            If viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                Me.HiddenField1.Value = viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                'fillGridRutaAprobacion(viaje.ta_documento_viaje.FirstOrDefault().id_documento)
            End If


            Dim intOwnerInforme As String() = viajeDetalle.id_usuario_app_informe.ToString.Split(",")
            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)


            If viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).Count() > 0 And intOwnerInforme.Where(Function(p) p.Contains(idUser)).Count() > 0 Then

                Dim idDoc = viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                Me.HiddenField1.Value = viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                fillGridRutaAprobacion(viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento)

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
                    Me.btn_guardar.Visible = True
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

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim legalizacionViaje = New tme_solicitud_viaje_legalizacion
                legalizacionViaje.id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(legalizacionViaje.id_viaje)

                viaje.informe_compromiso = Me.txt_compromisos_conclusiones.Text
                viaje.informe_resultado = Me.txt_resultados.Text
                viaje.lugares_entidades_personas = Me.txt_lugares_entidades_personas.Text
                viaje.fecha_registro_informe = DateTime.Now
                viaje.id_usuario_registro_compromiso = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)
                If es_Edicion = 1 Then
                    Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                    Dim viajePermisos = dbEntities.tme_solicitud_viaje_permisos.Where(Function(p) p.id_viaje = id_viaje).ToList().FirstOrDefault()
                    If viajePermisos IsNot Nothing Then
                        viajePermisos.editar_informe = False
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

    Private Sub btn_enviar_aprobacion_Click(sender As Object, e As EventArgs) Handles btn_enviar_aprobacion.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_viaje = Convert.ToInt32(Me.idViaje.Value)
                Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                viaje.informe_compromiso = Me.txt_compromisos_conclusiones.Text
                viaje.informe_resultado = Me.txt_resultados.Text
                viaje.fecha_registro_informe = DateTime.Now
                viaje.id_usuario_registro_compromiso = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                viaje.lugares_entidades_personas = Me.txt_lugares_entidades_personas.Text

                dbEntities.Entry(viaje).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                If viaje IsNot Nothing And viaje.ta_documento_viaje_informe.Where(Function(p) p.reversado Is Nothing).Count() = 0 Then

                    Dim id_categoriaAPP = 2046

                    Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
                    Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)

                    If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                        Me.lblerr_user.Text = "El informe se guardo correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de informes de viajes, contáctese con el administrador."
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
                Dim documentoViaje As New ta_documento_viaje_informe
                documentoViaje.id_viaje = idViaje
                documentoViaje.id_documento = idDocumento
                dbEntities.ta_documento_viaje_informe.Add(documentoViaje)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Function guardarDocumento(ByVal viaje As tme_solicitud_viaje, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2046

        Dim cls_viaje As APPROVAL.clss_viaje = New APPROVAL.clss_viaje(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalTimeSheet As DataTable = cls_viaje.get_ViajeApprovalUser(viaje.id_usuario, id_categoriaAPP)
        Dim id_tipo_documento = tblUserApprovalTimeSheet.Rows(0).Item("id_tipoDocumento")



        Dim descripcion = String.Format("Informe de viaje {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, viaje.fecha_inicio_viaje)
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

            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1022, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_appdocumento, Integer), viaje.id_viaje)) Then
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
                viaje.informe_compromiso = Me.txt_compromisos_conclusiones.Text
                viaje.informe_resultado = Me.txt_resultados.Text
                viaje.fecha_registro_informe = DateTime.Now
                viaje.id_usuario_registro_compromiso = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                viaje.lugares_entidades_personas = Me.txt_lugares_entidades_personas.Text

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

                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
                                        Else 'Error mandando Email
                                        End If


                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                        'Else 'Error mandando Email
                                        'End If
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

                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
                                    Else 'Error mandando Email
                                    End If

                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If

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
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
                                            Else 'Error mandando Email
                                            End If
                                            'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            'If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(id_app_documento, Integer), CType(id_viaje, Integer))) Then
                                            'Else 'Error mandando Email
                                            'End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    Else 'No tool related to this approval

                                        '*********************************APPROVED NEXT STEP****************************************
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        'If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
                                        'Else 'Error mandando Email
                                        'End If
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 11111, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                        'Else 'Error mandando Email
                                        'End If
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
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
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

                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1022, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(id_app_documento, Integer), id_viaje)) Then
                                    Else 'Error mandando Email
                                    End If
                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If
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
End Class