Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports Telerik.Web.UI.Calendar
Imports System.Data.Entity.Validation
Imports System.Globalization
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Public Class frm_parEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADM_PAR_EDIT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
    Dim PathArchivos = ""
    Dim dtConceptos As New DataTable
    Dim dtAportes As New DataTable
    Dim ListItemsDeleteBD As New List(Of Integer)
    Dim ListItemsDeleteAportesBD As New List(Of Integer)
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
                    'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_itinerario)
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
                Session.Remove("dtAportes")
                Session.Remove("ListItemsDeleteBD")
                Session.Remove("ListItemsDeleteAportesBD")
                LoadList()
                Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
                Me.idPar.Value = id_par

                Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
                Me.esEdicion.Value = es_Edicion

                LoadData(id_par)
            End If
        End Using

    End Sub
    Sub LoadData(ByVal id_par As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        If Session("dtAportes") IsNot Nothing Then
            dtAportes = Session("dtAportes")
        Else
            createdtcolumsAportes()
        End If
        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.tme_pares.FirstOrDefault(Function(p) p.id_par = id_par)

            Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            ''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("habilitar_agregar_viaje")
            dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitarViaje = ds.Tables("habilitar_agregar_viaje").Rows(0).Item(0)
            Me.habilitar_registro.Value = habilitarViaje


            Dim usuario = dbEntities.t_usuarios.Find(par.id_usuario_crea)

            Me.lblt_usuario_solicita.Text = usuario.nombre_usuario & " " & usuario.apellidos_usuario & "(" & usuario.t_job_title.job & ")"
            Me.id_cargo.Value = par.t_job_title.id_job_title
            Me.dt_fecha_solicitud.SelectedDate = par.fecha_solicitud
            validarTasaSer()

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())



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
                Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.t_regiones.id_programa = ID).Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_regiones.nombre_region & " - " & p.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            End If

            If par.id_entidad_evento IsNot Nothing Then
                Me.cmb_entidad.SelectedValue = par.id_entidad_evento
            End If

            Dim estructura = dbEntities.tme_estructura_marcologico.Where(Function(p) p.tme_programa_marco_logico.id_programa = id_programa And p.id_tipo_marcologico = 15).Select(Function(p) New With
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



            'Me.cmb_regional.SelectedValue = par.id_region
            'cargarSubRegion()
            Me.cmb_sub_Region.SelectedValue = par.id_subregion
            Me.dt_fecha_requiere_servicios.SelectedDate = par.fecha_requiere_servicio
            Me.cmb_proposito_par.SelectedValue = par.id_tipo_solicitud
            Me.cmb_departamento_entrega.SelectedValue = par.t_municipios.id_departamento
            loadMunicipios()
            Me.cmb_municipio_entrega.SelectedValue = par.id_municipio_entrega
            'Me.rbn_asociado_actividad.SelectedValue = If(par.asociado_actividad = True, 1, 0)
            'cargarActividades()
            'If par.id_ficha_proyecto IsNot Nothing Then
            '    Me.cmb_sub_actividad.SelectedValue = par.id_ficha_proyecto
            'End If
            Me.cmb_tipo_par.SelectedValue = par.id_tipo_par
            informacionEvneto()
            Me.cmb_cargo_par.SelectedValue = par.id_cargo_a
            'Me.txt_codigo_facturacion.Text = par.codigo_facturación
            Me.rbn_comunicaciones.SelectedValue = If(par.asociado_comunicaciones = True, 1, 0)
            Me.txt_proposito_servicio.Text = par.proposito
            Me.cmb_adjuntos_par.SelectedValue = par.id_tipo_adjunto_par
            Me.txt_descripcion_adjuntos.Text = par.observaciones_adicionales
            If Me.cmb_tipo_par.SelectedValue = "2" Then
                'Me.cmb_objetivo.SelectedValue = par.tme_estructura_marcologico.tme_estructura_marcologico2.id_estructura_marcologico_padre
                'cargarResultadoEsperado()
                'Me.cmb_resultado_esperado.SelectedValue = par.tme_estructura_marcologico.id_estructura_marcologico_padre
                'cargarComponente()
                'Me.cmb_componente.SelectedValue = par.id_estructura_marco_logico
                Me.dt_fecha_inicio_evento.SelectedDate = par.fecha_inicio_evento
                Me.dt_fecha_finalizacion_evento.SelectedDate = par.fecha_fin_evento
                Me.cmb_tipo_evento.SelectedValue = par.id_tipo_evento
                Me.txt_nro_horas.Value = par.numero_horas
                Me.txt_nombre_evento.Text = par.nombre_evento
                'Me.txt_responsable.Text = par.usuario_responable_evento
                'Me.txt_entidad.Text = par.entidad_responsable_evento

            End If


            For Each item In par.tme_par_detalle.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_par_detalle

                dtConceptos.Rows.Add(idunique, item.cantidad, item.descripcion, item.precio_unitario, item.valor_total, item.tme_unidad_medida_par.unidad_medida, item.id_unidad_medida, True, item.id_par_detalle)
            Next



            If par.asociado_recursos_apalancados = True Then

                For Each item In par.tme_par_aporte.ToList()
                    Dim fecha = DateTime.Now
                    Dim rnd As New Random()
                    Dim aleatorio As String = ""
                    Dim index = 1
                    aleatorio = rnd.Next(index, 9999999).ToString()
                    Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_par_apalancamiento
                    dtAportes.Rows.Add(idunique, item.id_aporte, item.tme_Aportes.nombre_aporte, True, item.id_par_apalancamiento)

                Next
                Me.rbn_recursos_apalancados.SelectedValue = 1
                Me.fuentesApalancamiento.Visible = True
            Else
                Me.rbn_recursos_apalancados.SelectedValue = 0
            End If

            Dim componentes = par.tme_par_marco_logico.ToList()
            If componentes.Count() > 0 Then
                For Each row In Me.grd_componente.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        Dim idComponente As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                        If componentes.Where(Function(p) p.id_marco_logico = idComponente).ToList().Count() > 0 Then
                            subR.Checked = True
                        End If
                    End If
                Next
            End If



            Dim parDetalle = dbEntities.vw_tme_par.FirstOrDefault(Function(p) p.id_par = par.id_par)
            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)

            Me.esCancelado.Value = 0
            If par.id_usuario_crea <> idUser And parDetalle.id_documento > 0 And parDetalle.id_estadoDoc <> 6 Then
                Me.btn_guardar_2.Visible = False
                Me.btn_guardar.Visible = False

                If parDetalle.id_estadoDoc = 7 Then

                    Dim intOwner As String() = parDetalle.id_usuario_app.ToString.Split(",")
                    If intOwner.Where(Function(p) p.Contains(idUser)).Count() > 0 Then
                        Me.btn_guardar.Visible = True
                    End If
                End If

            ElseIf par.id_usuario_crea = idUser Then

                Dim intOwnerSolicitud As String() = parDetalle.id_usuario_app.ToString.Split(",")


                If par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).Count() > 0 And (par.par_cancelado Is Nothing Or par.par_cancelado = False) And intOwnerSolicitud.Where(Function(p) p.Contains(idUser)).Count() > 0 Then

                    Dim idDoc = par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).LastOrDefault().id_documento
                    Me.HiddenField1.Value = par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).LastOrDefault().id_documento


                    fillGridRutaAprobacion(par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).LastOrDefault().id_documento)

                    Me.btn_guardar_2.Visible = False
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


                    If par.par_cancelado = True Then
                        Me.esCancelado.Value = 1
                    End If
                Else
                    Me.rept_msgApproval.DataSource = Nothing

                    If par.par_cancelado = True Then
                        Me.esCancelado.Value = 1
                    End If

                    If es_Edicion = 1 Then
                        Me.btn_guardar_2.Visible = True
                        Me.btn_guardar.Visible = False
                    End If


                End If
            Else
                Me.btn_guardar_2.Visible = False
                Me.btn_guardar.Visible = False
            End If

            Me.grd_conceptos.DataSource = dtConceptos
            Me.grd_conceptos.DataBind()
            Session("dtConceptos") = dtConceptos
            Session("dtAportes") = dtAportes



            Me.grd_fuente_aporte.DataSource = dtAportes
            Me.grd_fuente_aporte.DataBind()

        End Using
    End Sub

    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
    End Sub
    Sub LoadList()
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.nombre_departamento).ToList()
            Me.cmb_departamento_entrega.DataSourceID = ""
            Me.cmb_departamento_entrega.DataSource = departamentos
            Me.cmb_departamento_entrega.DataTextField = "nombre_departamento"
            Me.cmb_departamento_entrega.DataValueField = "id_departamento"
            Me.cmb_departamento_entrega.SelectedValue = departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_entrega.DataBind()

            'Dim regionesUsuario = usuario.t_usuario_subregion.Select(Function(p) p.t_subregiones.id_region).Distinct().ToList()
            'Dim regiones = dbEntities.t_regiones.Where(Function(p) p.id_programa = id_programa).ToList()
            'If regionesUsuario.Count() > 0 Then
            '    regiones = regiones.Where(Function(p) regionesUsuario.Contains(p.id_region)).ToList()
            '    Me.cmb_regional.DataSourceID = ""
            '    Me.cmb_regional.DataSource = regiones
            '    Me.cmb_regional.DataTextField = "nombre_region"
            '    Me.cmb_regional.DataValueField = "id_region"
            '    Me.cmb_regional.DataBind()

            '    Dim idRegion = regiones.FirstOrDefault().id_region
            '    Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.id_region = idRegion).ToList()
            '    Me.cmb_sub_Region.DataSourceID = ""
            '    Me.cmb_sub_Region.DataSource = subRegion
            '    Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            '    Me.cmb_sub_Region.DataValueField = "id_subregion"
            '    Me.cmb_sub_Region.DataBind()
            'End If

            Me.cmb_proposito_par.DataSourceID = ""
            Me.cmb_proposito_par.DataSource = dbEntities.tme_tipo_solicitud_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_proposito_par.DataTextField = "tipo_solicitud"
            Me.cmb_proposito_par.DataValueField = "id_tipo_solicitud"
            Me.cmb_proposito_par.DataBind()

            Me.cmb_tipo_par.DataSourceID = ""
            Me.cmb_tipo_par.DataSource = dbEntities.tme_tipo_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_tipo_par.DataTextField = "tipo_par"
            Me.cmb_tipo_par.DataValueField = "id_tipo_par"
            Me.cmb_tipo_par.DataBind()

            Me.cmb_adjuntos_par.DataSourceID = ""
            Me.cmb_adjuntos_par.DataSource = dbEntities.tme_tipo_adjunto_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_adjuntos_par.DataTextField = "tipo_adjunto"
            Me.cmb_adjuntos_par.DataValueField = "id_tipo_adjunto_par"
            Me.cmb_adjuntos_par.DataBind()

            Me.cmb_cargo_par.DataSourceID = ""
            Me.cmb_cargo_par.DataSource = dbEntities.tme_cargo_pares.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_cargo_par.DataTextField = "cargo"
            Me.cmb_cargo_par.DataValueField = "id_cargo_a_par"
            Me.cmb_cargo_par.DataBind()

            Me.cmb_unidad_medida.DataSourceID = ""
            Me.cmb_unidad_medida.DataSource = dbEntities.tme_unidad_medida_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_unidad_medida.DataTextField = "unidad_medida"
            Me.cmb_unidad_medida.DataValueField = "id_unidad_media"
            Me.cmb_unidad_medida.DataBind()

            Dim aportes = dbEntities.tme_Aportes.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_entidad.DataSourceID = ""
            Me.cmb_entidad.DataSource = aportes
            Me.cmb_entidad.DataTextField = "nombre_aporte"
            Me.cmb_entidad.DataValueField = "id_aporte"
            Me.cmb_entidad.DataBind()

            Me.cmb_fuente_aporte.DataSourceID = ""
            Me.cmb_fuente_aporte.DataSource = aportes
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()

            Dim estructura = dbEntities.tme_estructura_marcologico.Where(Function(p) p.tme_programa_marco_logico.id_programa = id_programa And p.id_tipo_marcologico = 15).Select(Function(p) New With
                                                                                                                                          {Key .id_estructura_marcologico = p.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica = p.codigo & " - " & p.descripcion_logica,
                                                                                                                                           Key .id_estructura_marcologico_2 = p.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica_padre = p.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                                           Key .id_estructura_marcologico_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                           Key .descripcion_logica_padre_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.tme_estructura_marcologico2.descripcion_logica
                                                                                                                                          }).ToList()
            Me.grd_componente.DataSource = estructura
            Me.grd_componente.DataBind()


            loadMunicipios()

        End Using
    End Sub

    Sub loadMunicipios()
        Using dbEntities As New dbRMS_JIEntities
            Dim municipios As List(Of t_municipios)
            Dim idDepto = Convert.ToInt32(Me.cmb_departamento_entrega.SelectedValue)
            municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).OrderBy(Function(p) p.nombre_municipio).ToList()
            Me.cmb_municipio_entrega.ClearSelection()
            Me.cmb_municipio_entrega.DataSourceID = ""
            Me.cmb_municipio_entrega.DataSource = municipios
            Me.cmb_municipio_entrega.DataTextField = "nombre_municipio"
            Me.cmb_municipio_entrega.DataValueField = "id_municipio"
            Me.cmb_municipio_entrega.DataBind()
        End Using

    End Sub

    Private Sub cmb_departamento_entrega_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento_entrega.SelectedIndexChanged
        loadMunicipios()
    End Sub

    'Private Sub rbn_asociado_actividad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_asociado_actividad.SelectedIndexChanged
    '    cargarActividades()
    'End Sub
    'Sub cargarActividades()
    '    Using dbEntities As New dbRMS_JIEntities
    '        Me.infoActividad.Visible = False
    '        If Me.rbn_asociado_actividad.SelectedValue = "1" Then
    '            Me.infoActividad.Visible = True
    '            Dim id_programa = Me.Session("E_IDPrograma").ToString()
    '            Dim actividades = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) Trim(p.id_programa) = id_programa).Select(Function(p) _
    '                                         New With {Key .actividad = p.codigo_RFA.ToString() & " (" & p.nombre_ejecutor & ")",
    '                                                   Key .id_ficha_proyecto = p.id_ficha_proyecto}).ToList()

    '            Me.cmb_sub_actividad.ClearSelection()
    '            Me.cmb_sub_actividad.DataSourceID = ""
    '            Me.cmb_sub_actividad.DataSource = actividades
    '            Me.cmb_sub_actividad.DataTextField = "actividad"
    '            Me.cmb_sub_actividad.DataValueField = "id_ficha_proyecto"
    '            Me.cmb_sub_actividad.DataBind()

    '            Me.rv_sub_actividad.ControlToValidate = "cmb_sub_actividad"
    '        Else

    '        End If

    '    End Using
    'End Sub

    Private Sub cmb_tipo_tar_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_par.SelectedIndexChanged
        informacionEvneto()
    End Sub
    Sub informacionEvneto()
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Me.informacion_evento.Visible = If(Me.cmb_tipo_par.SelectedValue = "2", True, False)
            'Me.infoComponente.Visible = If(Me.cmb_tipo_par.SelectedValue = "2", True, False)
            If Me.cmb_tipo_par.SelectedValue = "2" Then
                'cargarObjetivos()
                Me.cmb_tipo_evento.DataSourceID = ""
                Me.cmb_tipo_evento.DataSource = dbEntities.tme_tipo_evento_par.Where(Function(p) p.id_programa = id_programa).ToList()
                Me.cmb_tipo_evento.DataTextField = "tipo_evento"
                Me.cmb_tipo_evento.DataValueField = "id_tipo_evento"
                Me.cmb_tipo_evento.DataBind()
            End If
        End Using
        'Me.infoRecApa.Visible = If(Me.cmb_tipo_tar.SelectedValue = "2", True, False)
    End Sub
    'Sub cargarObjetivos()
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '        Dim estructuraMarcoLogico = dbEntities.vw_estructura_marcologico.Where(Function(p) p.id_programa = id_programa).ToList()
    '        Dim objetivos = estructuraMarcoLogico.Where(Function(p) p.id_tipo_marcologico = 13).ToList()
    '        Me.cmb_objetivo.DataSourceID = ""
    '        Me.cmb_objetivo.DataSource = objetivos
    '        Me.cmb_objetivo.DataTextField = "descripcion_logica"
    '        Me.cmb_objetivo.DataValueField = "id_estructura_marcologico"
    '        Me.cmb_objetivo.DataBind()
    '        Me.cmb_objetivo.SelectedValue = objetivos.FirstOrDefault().id_estructura_marcologico
    '        cargarResultadoEsperado()
    '    End Using
    'End Sub

    'Private Sub cmb_objetivo_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_objetivo.SelectedIndexChanged
    '    cargarResultadoEsperado()
    'End Sub
    'Sub cargarResultadoEsperado()
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '        Dim estructuraMarcoLogico = dbEntities.vw_estructura_marcologico.Where(Function(p) p.id_programa = id_programa).ToList()
    '        Dim id_objetivo = Convert.ToInt32(Me.cmb_objetivo.SelectedValue)
    '        Dim objetivos = estructuraMarcoLogico.Where(Function(p) p.id_tipo_marcologico = 14 And p.id_estructura_marcologico_padre = id_objetivo).ToList()
    '        Me.cmb_resultado_esperado.DataSourceID = ""
    '        Me.cmb_resultado_esperado.DataSource = objetivos
    '        Me.cmb_resultado_esperado.DataTextField = "descripcion_logica"
    '        Me.cmb_resultado_esperado.DataValueField = "id_estructura_marcologico"
    '        Me.cmb_resultado_esperado.DataBind()
    '        Me.cmb_resultado_esperado.SelectedValue = objetivos.FirstOrDefault().id_estructura_marcologico
    '        cargarComponente()
    '    End Using
    'End Sub

    'Sub cargarComponente()
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '        Dim estructuraMarcoLogico = dbEntities.vw_estructura_marcologico.Where(Function(p) p.id_programa = id_programa).ToList()
    '        Dim id_resultado_esperado = Convert.ToInt32(Me.cmb_resultado_esperado.SelectedValue)
    '        Dim objetivos = estructuraMarcoLogico.Where(Function(p) p.id_tipo_marcologico = 15 And p.id_estructura_marcologico_padre = id_resultado_esperado).ToList()
    '        Me.cmb_componente.DataSourceID = ""
    '        Me.cmb_componente.DataSource = objetivos
    '        Me.cmb_componente.DataTextField = "descripcion_logica"
    '        Me.cmb_componente.DataValueField = "id_estructura_marcologico"
    '        Me.cmb_componente.DataBind()
    '        Me.cmb_componente.SelectedValue = objetivos.FirstOrDefault().id_estructura_marcologico
    '    End Using
    'End Sub

    'Private Sub cmb_resultado_esperado_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_resultado_esperado.SelectedIndexChanged
    '    cargarComponente()
    'End Sub

    Private Sub dt_fecha_solicitud_SelectedDateChanged(sender As Object, e As SelectedDateChangedEventArgs) Handles dt_fecha_solicitud.SelectedDateChanged
        validarTasaSer()
    End Sub

    Sub validarTasaSer()
        Using dbEntities As New dbRMS_JIEntities
            Dim fecha = Me.dt_fecha_solicitud.SelectedDate
            Dim anio = fecha?.Year
            Dim mes = fecha?.Month
            Dim tasaSer = dbEntities.tme_tasa_ser.Where(Function(p) p.anio = anio And p.id_mes = mes).FirstOrDefault()
            Dim tasaSerRegistrada As Boolean = True
            If tasaSer Is Nothing Then
                tasaSerRegistrada = False
                Me.errorTasaSer.Visible = True
                Me.continuarPar.Visible = False
            Else
                Me.id_tasa_ser.Value = tasaSer.id_tasa_ser
                Me.txt_tasa_ser.Value = tasaSer.tasa_ser
                Me.continuarPar.Visible = True
                Me.errorTasaSer.Visible = False
            End If

        End Using
    End Sub

    Protected Sub btn_agregar_concepto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_concepto.Click
        Try
            If Session("dtConceptos") IsNot Nothing Then
                dtConceptos = Session("dtConceptos")
            Else
                createdtcolums()
            End If

            If Session("ListItemsDeleteBD") IsNot Nothing Then
                ListItemsDeleteBD = Session("ListItemsDeleteBD")
            End If

            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio
            Dim valor_unitario = Me.txt_valor.Value
            Dim cantidad = Me.txt_cantidad.Value
            Dim valor_total = valor_unitario * cantidad
            dtConceptos.Rows.Add(idunique, Me.txt_cantidad.Value, Me.txt_descripcion_concepto.Text, Me.txt_valor.Value, valor_total, Me.cmb_unidad_medida.Text, Convert.ToInt32(Me.cmb_unidad_medida.SelectedValue), False, 0)
            Session("dtConceptos") = dtConceptos
            Me.txt_cantidad.Text = String.Empty
            Me.txt_valor.Text = String.Empty
            Me.txt_descripcion_concepto.Text = String.Empty
            'Me.txt_unidad_medida.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
            fillGrid()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        Me.grd_conceptos.DataSource = dtConceptos
        Me.grd_conceptos.DataBind()
    End Sub
    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_par_detalle", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("valor", GetType(Double))
            dtConceptos.Columns.Add("unidad_medida", GetType(String))
            dtConceptos.Columns.Add("id_unidad_medida", GetType(Integer))
            dtConceptos.Columns.Add("esta_bd", GetType(Boolean))
            dtConceptos.Columns.Add("id_par_detalle_bd", GetType(Integer))
        End If
    End Sub
    Sub createdtcolumsAportes()
        If dtAportes.Columns.Count = 0 Then
            dtAportes.Columns.Add("id_par_fuente_aporte", GetType(String))
            dtAportes.Columns.Add("id_aporte", GetType(Double))
            dtAportes.Columns.Add("nombre_aporte", GetType(String))
            dtAportes.Columns.Add("esta_bd", GetType(Boolean))
            dtAportes.Columns.Add("id_par_fuente_bd", GetType(Integer))
        End If
    End Sub
    Protected Sub delete_concepto(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.id_concpeto.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipo_delete.Value = 1
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub delete_aporte(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.id_concpeto.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipo_delete.Value = 2
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub grd_conceptos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_conceptos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_par_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_par_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_par_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub

    Protected Sub grd_fuente_aporte_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_fuente_aporte.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_par_fuente_aporte").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_par_fuente_aporte").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")
        End If
    End Sub
    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        If Session("dtAportes") IsNot Nothing Then
            dtAportes = Session("dtAportes")
        Else
            createdtcolumsAportes()
        End If

        If Session("ListItemsDeleteBD") IsNot Nothing Then
            ListItemsDeleteBD = Session("ListItemsDeleteBD")
        End If

        If Session("ListItemsDeleteAportesBD") IsNot Nothing Then
            ListItemsDeleteAportesBD = Session("ListItemsDeleteAportesBD")
        End If


        Dim idTipo = Convert.ToInt32(Me.tipo_delete.Value)
        If idTipo = 1 Then
            Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

            Dim foundRow As DataRow() = dtConceptos.Select("id_par_detalle = '" & idConcepto.ToString() & "'")
            Dim estaBD As Boolean = foundRow(0)("esta_bd")
            Dim id_par_dealle = foundRow(0)("id_par_detalle_bd")

            If estaBD Then
                ListItemsDeleteBD.Add(id_par_dealle)
            End If
            If foundRow.Count > 0 Then
                dtConceptos.Rows.Remove(foundRow(0))
            End If
            Session("dtConceptos") = dtConceptos
            Session("ListItemsDeleteBD") = ListItemsDeleteBD
            fillGrid()
        ElseIf idTipo = 2 Then
            Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

            Dim foundRow As DataRow() = dtAportes.Select("id_par_fuente_aporte = '" & idConcepto.ToString() & "'")
            Dim estaBD As Boolean = foundRow(0)("esta_bd")
            Dim id_par_fuente = foundRow(0)("id_par_fuente_bd")

            If estaBD Then
                ListItemsDeleteAportesBD.Add(id_par_fuente)
            End If
            If foundRow.Count > 0 Then
                dtAportes.Rows.Remove(foundRow(0))
            End If
            Session("dtAportes") = dtAportes
            Session("ListItemsDeleteAportesBD") = ListItemsDeleteAportesBD
            fillGridAportes()
        End If



        If dtConceptos.Rows().Count() = 0 Then
            Me.txt_cantidad_productos.Text = String.Empty
        Else
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
        End If
        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Public Function validarValorPar() As Boolean
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Dim cantidad As Decimal
        Dim descripcion As String
        Dim unidadMedida As String
        Dim valorUnitario As Decimal
        Dim valorTotal As Decimal
        Dim id_uMedida As Decimal

        Dim valorPar As Decimal = 0.0

        For Each row As DataRow In dtConceptos.Rows
            Dim oParDetalle As New tme_par_detalle
            cantidad = row("cantidad")
            descripcion = row("descripcion")
            valorUnitario = row("valor_unitario")
            unidadMedida = row("unidad_medida")
            valorTotal = row("valor")
            id_uMedida = row("id_unidad_medida")

            valorPar += valorTotal
        Next

        Dim valorParDolares As Decimal = valorPar / Me.txt_tasa_ser.Value
        'Dim numDias = DateDiff("d", DateTime.Now, Me.dt_fecha_requiere_servicios.SelectedDate)
        Dim mensaje = ""
        Me.alerta_dias.Visible = False
        Dim habilitarGuardar = True

        Dim numDiasHabiles = 0
        If valorParDolares <= 1000 Then
            numDiasHabiles = 4
        ElseIf valorParDolares > 1000 And valorParDolares < 3500 Then
            numDiasHabiles = 4
        ElseIf valorParDolares >= 3500 Then
            numDiasHabiles = 6
        End If



        'Dim fechaHabil = calculaDiaHabil(numDiasHabiles, DateTime.Now)
        Dim fechaHabil2 = calculaDiaHabil(numDiasHabiles, DateTime.Now)
        Dim fechaHabil = New Date(fechaHabil2.Year, fechaHabil2.Month, fechaHabil2.Day, 0, 0, 0)
        'Dim numDias = DateDiff("d", DateTime.Now, Me.dt_fecha_requiere_servicios.SelectedDate)
        If valorParDolares <= 1000 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR inferiores a 1.000 USD se deben solicitar mínimo con 5 días hábiles de anticipación"
            habilitarGuardar = False
        ElseIf valorParDolares > 1000 And valorParDolares < 3500 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR entre 1.001 USD y 3.499 USD se deben solicitar mínimo con 5 días hábiles de anticipación"
            habilitarGuardar = False
        ElseIf valorParDolares >= 3500 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR mayores a 3.499 USD se deben solicitar mínimo con 7 días hábiles de anticipación"
            habilitarGuardar = False
        End If

        If habilitarGuardar = False Then
            Me.alerta_dias.Visible = True
            Me.lbl_alerta_solicitud_par.Text = mensaje
        End If

        Dim habilitarRegistro = Convert.ToBoolean(Me.habilitar_registro.Value)

        Dim cancelado = Convert.ToInt32(Me.esCancelado.Value)
        If cancelado = 1 Then
            habilitarGuardar = True
            Me.alerta_dias.Visible = False
        End If
        If habilitarRegistro = True Then
            Me.alerta_dias.Visible = False
            Return True
        Else
            Return habilitarGuardar
        End If
    End Function
    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_JIEntities
            Try

                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim parDoc = dbEntities.ta_documento_par.Where(Function(p) p.id_par = id_par And p.reversado = True).FirstOrDefault()

                Dim idDoc = 0

                If parDoc IsNot Nothing Then
                    idDoc = parDoc.id_documento
                End If
                If idDoc = 0 Then
                    If validarValorPar() Then
                        If guardarPar(False) Then
                            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
                            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                        End If

                    End If
                Else
                    guardarPar(False)
                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                End If


            Catch ex As DbEntityValidationException
                Dim st1 As String
                Dim st2 As String
                For Each a In ex.EntityValidationErrors
                    For Each b In a.ValidationErrors
                        st1 = b.PropertyName
                        st2 = b.ErrorMessage
                    Next
                Next
            End Try
        End Using


    End Sub

    'Private Sub cmb_regional_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_regional.SelectedIndexChanged
    '    cargarSubRegion()
    'End Sub

    'Sub cargarSubRegion()
    '    Using dbEntities As New dbRMS_JIEntities
    '        If Me.cmb_regional.SelectedValue <> "" Then
    '            Dim idRegion = Convert.ToInt32(Me.cmb_regional.SelectedValue)
    '            Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.id_region = idRegion).ToList()
    '            Me.cmb_sub_Region.ClearSelection()
    '            Me.cmb_sub_Region.DataSourceID = ""
    '            Me.cmb_sub_Region.DataSource = subRegion
    '            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
    '            Me.cmb_sub_Region.DataValueField = "id_subregion"
    '            Me.cmb_sub_Region.DataBind()
    '        End If
    '    End Using
    'End Sub

    Private Sub btn_guardar_2_Click(sender As Object, e As EventArgs) Handles btn_guardar_2.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim parDoc = dbEntities.ta_documento_par.Where(Function(p) p.id_par = id_par And p.reversado = True).FirstOrDefault()

                Dim idDoc = 0

                If parDoc IsNot Nothing Then
                    idDoc = parDoc.id_documento
                End If

                If idDoc = 0 Then
                    If validarValorPar() Then
                        If guardarPar(True) Then
                            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
                            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                        End If
                    End If
                Else
                    If guardarPar(True) Then
                        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                        Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
                        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
                    End If
                End If





            Catch ex As DbEntityValidationException
                Dim st1 As String
                Dim st2 As String
                For Each a In ex.EntityValidationErrors
                    For Each b In a.ValidationErrors
                        st1 = b.PropertyName
                        st2 = b.ErrorMessage
                    Next
                Next
            End Try
        End Using

    End Sub
    Public Function guardarPar(ByVal enviarAprobacion As Boolean) As Boolean
        Try
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            If Session("dtConceptos") IsNot Nothing Then
                dtConceptos = Session("dtConceptos")
            Else
                createdtcolums()
            End If

            If Session("dtAportes") IsNot Nothing Then
                dtAportes = Session("dtAportes")
            Else
                createdtcolumsAportes()
            End If

            If Session("ListItemsDeleteBD") IsNot Nothing Then
                ListItemsDeleteBD = Session("ListItemsDeleteBD")
            End If

            If Session("ListItemsDeleteAportesBD") IsNot Nothing Then
                ListItemsDeleteAportesBD = Session("ListItemsDeleteAportesBD")
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

                    Dim codInicialPT = 386

                    'Dim codPar = dbEntities.Database.SqlQuery(Of Integer)("SELECT NEXT VALUE FOR SequencePar")
                    Dim id_par = Convert.ToInt32(Me.idPar.Value)
                    Dim oPar = dbEntities.tme_pares.Find(id_par)
                    Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                    Dim subR = dbEntities.t_subregiones.Find(idSubRegion)
                    oPar.fecha_edita = Date.Now
                    oPar.id_usuario_edita = Me.Session("E_IdUser").ToString()
                    Dim usuario = dbEntities.t_usuarios.Find(oPar.id_usuario_crea)
                    oPar.fecha_solicitud = Me.dt_fecha_solicitud.SelectedDate
                    oPar.tasa_ser_cotizacion = Me.txt_tasa_ser.Value
                    oPar.id_tasa_ser_cotizacion = Convert.ToInt32(Me.id_tasa_ser.Value)
                    oPar.id_region = subR.id_region
                    oPar.fecha_requiere_servicio = Me.dt_fecha_requiere_servicios.SelectedDate
                    oPar.id_tipo_par = Convert.ToInt32(Me.cmb_tipo_par.SelectedValue)
                    oPar.id_subregion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)

                    Dim subRegion = dbEntities.t_subregiones.Find(oPar.id_subregion)
                    'oPar.codigo_par = "JI-" & subRegion.prefijo_subregion & "-" & codPar.Single()

                    'oFactura.ciudad = Me.txt_ciudad.Text
                    oPar.id_tipo_solicitud = Convert.ToInt32(Me.cmb_proposito_par.SelectedValue)
                    oPar.id_municipio_entrega = Convert.ToInt32(Me.cmb_municipio_entrega.SelectedValue)
                    'oPar.asociado_actividad = If(Me.rbn_asociado_actividad.SelectedValue = "1", True, False)
                    oPar.asociado_comunicaciones = If(Me.rbn_comunicaciones.SelectedValue = "1", True, False)
                    'If oPar.asociado_actividad Then
                    '    oPar.id_ficha_proyecto = Convert.ToInt32(Me.cmb_sub_actividad.SelectedValue)
                    'End If
                    oPar.id_cargo_a = Convert.ToInt32(Me.cmb_cargo_par.SelectedValue)
                    oPar.id_cargo = Convert.ToInt32(Me.id_cargo.Value)
                    'oPar.codigo_facturación = Me.txt_codigo_facturacion.Text
                    oPar.proposito = Me.txt_proposito_servicio.Text
                    oPar.id_tipo_adjunto_par = Convert.ToInt32(Me.cmb_adjuntos_par.SelectedValue)
                    oPar.observaciones_adicionales = Me.txt_descripcion_adjuntos.Text

                    If enviarAprobacion Then
                        oPar.par_cancelado = 0
                    End If
                    For Each file As UploadedFile In soporte_adjunto.UploadedFiles
                        Dim fecha = DateTime.Now
                        Dim exten = file.GetExtension()
                        Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & oPar.codigo_par & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten

                        Dim Path As String
                        Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                        file.SaveAs(Path + nombreArchivo)

                        oPar.soporte_adjunto = nombreArchivo
                    Next


                    If oPar.id_tipo_par = 2 Then

                        Dim parPT = dbEntities.tme_pares.Where(Function(p) p.id_tipo_par = 2).ToList()
                        Dim consecutivoPT = codInicialPT + parPT.Count()
                        Dim codPT = ""

                        If consecutivoPT < 1000 And consecutivoPT > 99 Then
                            codPT = "0"
                        ElseIf consecutivoPT < 100 And consecutivoPT > 9 Then
                            codPT = "00"
                        ElseIf consecutivoPT < 10 Then
                            codPT = "000"
                        End If



                        If oPar.codigo_pt Is Nothing Then
                            oPar.codigo_pt = "PT-" & codPT & consecutivoPT
                        End If

                        'oPar.id_estructura_marco_logico = Convert.ToInt32(Me.cmb_componente.SelectedValue)
                        oPar.fecha_inicio_evento = Me.dt_fecha_inicio_evento.SelectedDate
                        oPar.fecha_fin_evento = Me.dt_fecha_finalizacion_evento.SelectedDate
                        oPar.id_tipo_evento = Convert.ToInt32(Me.cmb_tipo_evento.SelectedValue)
                        oPar.numero_horas = Me.txt_nro_horas.Value
                        oPar.nombre_evento = Me.txt_nombre_evento.Text
                        'oPar.usuario_responable_evento = Me.txt_responsable.Text
                        'oPar.entidad_responsable_evento = Me.txt_entidad.Text
                        oPar.asociado_recursos_apalancados = If(Me.rbn_recursos_apalancados.SelectedValue = "1", True, False)
                        oPar.id_entidad_evento = Convert.ToInt32(Me.cmb_entidad.SelectedValue)
                    End If

                    Dim cancelado = Convert.ToInt32(Me.esCancelado.Value)
                    If cancelado = 1 Then
                        Dim LastChar As Char = oPar.codigo_par(oPar.codigo_par.Length - 1)
                        If LastChar <> "A" Then
                            oPar.codigo_par = oPar.codigo_par & "-A"
                        End If
                    End If
                    dbEntities.Entry(oPar)
                    dbEntities.SaveChanges()
                    'oPar.celular = Me.txt_celular_.Text
                    'oPar.correo = Me.txt_correo.Text
                    'oPar.numero_factura = codPar.Single()
                    'dbEntities.tme_facturacion.Add(oPar)
                    'dbEntities.SaveChanges()

                    Dim cantidad As Decimal
                    Dim descripcion As String
                    Dim unidadMedida As String
                    Dim valorUnitario As Decimal
                    Dim valorTotal As Decimal
                    Dim estaBD As Boolean
                    Dim id_uMedida As Decimal
                    For Each item In ListItemsDeleteBD
                        Dim oParDetalle = dbEntities.tme_par_detalle.Find(item)
                        dbEntities.Entry(oParDetalle).State = Entity.EntityState.Deleted
                        dbEntities.SaveChanges()
                    Next

                    For Each item In ListItemsDeleteAportesBD
                        Dim oParDetalle = dbEntities.tme_par_aporte.Find(item)
                        dbEntities.Entry(oParDetalle).State = Entity.EntityState.Deleted
                        dbEntities.SaveChanges()
                    Next


                    For Each row As DataRow In dtConceptos.Rows
                        Dim oParDetalle As New tme_par_detalle
                        cantidad = row("cantidad")
                        descripcion = row("descripcion")
                        valorUnitario = row("valor_unitario")
                        unidadMedida = row("unidad_medida")
                        valorTotal = row("valor")
                        id_uMedida = row("id_unidad_medida")
                        estaBD = row("esta_bd")
                        If estaBD = False Then
                            oParDetalle.id_par = oPar.id_par
                            oParDetalle.cantidad = cantidad
                            oParDetalle.descripcion = descripcion
                            oParDetalle.precio_unitario = valorUnitario
                            oParDetalle.id_unidad_medida = id_uMedida
                            'oParDetalle.unidad_medida = unidadMedida
                            dbEntities.tme_par_detalle.Add(oParDetalle)
                            dbEntities.SaveChanges()
                        End If

                    Next

                    Dim id_aporte As Decimal

                    For Each row As DataRow In dtAportes.Rows
                        Dim oParAporte As New tme_par_aporte
                        id_aporte = row("id_aporte")
                        estaBD = row("esta_bd")
                        If estaBD = False Then
                            oParAporte.id_par = oPar.id_par
                            oParAporte.id_aporte = id_aporte
                            dbEntities.tme_par_aporte.Add(oParAporte)
                            dbEntities.SaveChanges()
                        End If
                    Next



                    Dim compoentes = oPar.tme_par_marco_logico.ToList()
                    For Each row In Me.grd_componente.Items
                        If TypeOf row Is GridDataItem Then
                            Dim dataItem As GridDataItem = CType(row, GridDataItem)
                            Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                            Dim idEstructura As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                            If idComponente.Checked = True Then
                                If compoentes.Where(Function(p) p.id_marco_logico = idEstructura).ToList().Count() = 0 Then
                                    Dim oMarco = New tme_par_marco_logico
                                    oMarco.id_par = oPar.id_par
                                    oMarco.id_marco_logico = idEstructura
                                    dbEntities.tme_par_marco_logico.Add(oMarco)
                                    dbEntities.SaveChanges()
                                End If
                            Else
                                If compoentes.Where(Function(p) p.id_marco_logico = idEstructura).ToList().Count() > 0 Then
                                    Dim oMarco = compoentes.Where(Function(p) p.id_marco_logico = idEstructura).FirstOrDefault()
                                    dbEntities.Entry(oMarco).State = Entity.EntityState.Deleted
                                    dbEntities.SaveChanges()
                                End If
                            End If
                        End If
                    Next


                    If enviarAprobacion Then
                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_par = oPar.id_par).FirstOrDefault()

                        Dim id_categoriaAPP = 2044
                        Dim cls_par As APPROVAL.clss_par = New APPROVAL.clss_par(Convert.ToInt32(Me.Session("E_IDprograma")))

                        Dim tblUserApprovalTimeSheet As DataTable = New DataTable

                        If par.valor_total / par.tasa_ser_cotizacion > 50000 Then
                            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_mayor_50000(par.id_usuario, id_categoriaAPP)
                        ElseIf par.asociado_comunicaciones = True Then
                            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_comunicaciones(par.id_usuario, id_categoriaAPP)
                        ElseIf par.id_tipo_par = 1 Then
                            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_administrativo(par.id_usuario, id_categoriaAPP)
                        Else
                            tblUserApprovalTimeSheet = cls_par.get_parApprovalUserEventos(par.id_usuario, id_categoriaAPP)
                        End If


                        If tblUserApprovalTimeSheet.Rows.Count() = 0 Then
                            Me.lblerr_user.Text = "El PAR " & par.codigo_par & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de PAR, contáctese con el administrador."
                            Me.lblerr_user.Visible = True
                            guardarPar = False
                        Else
                            Dim id_documento = guardarDocumento(par, usuario)
                            guardarPar = True
                        End If

                    Else
                        guardarPar = True
                        'guardarRelacionDocumento(id_documento, oPar.id_par)
                    End If



                End Using
            Else
                Me.lblerr_user.Text = "Seleccione los componentes!"
                guardarPar = False
            End If


        Catch ex As DbEntityValidationException
            Dim st1 As String
            Dim st2 As String
            For Each a In ex.EntityValidationErrors
                For Each b In a.ValidationErrors
                    st1 = b.PropertyName
                    st2 = b.ErrorMessage
                Next
            Next
        End Try
    End Function

    Public Function guardarDocumento(ByVal par As vw_tme_par, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2044
        Dim cls_par As APPROVAL.clss_par = New APPROVAL.clss_par(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalTimeSheet As DataTable = New DataTable

        If par.valor_total / par.tasa_ser_cotizacion > 50000 Then
            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_mayor_50000(par.id_usuario, id_categoriaAPP)
        ElseIf par.asociado_comunicaciones = True Then
            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_comunicaciones(par.id_usuario, id_categoriaAPP)
        ElseIf par.id_tipo_par = 1 Then
            tblUserApprovalTimeSheet = cls_par.get_parApprovalUser_administrativo(par.id_usuario, id_categoriaAPP)
        Else
            tblUserApprovalTimeSheet = cls_par.get_parApprovalUserEventos(par.id_usuario, id_categoriaAPP)
        End If


        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
        Dim id_tipo_doc = tblUserApprovalTimeSheet.Rows(0).Item("id_tipoDocumento")



        Dim descripcion = String.Format("Solicitud de par {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, par.fecha_solicitud)
        Dim err As Boolean = False

        clss_approval.set_ta_documento(0) 'Set new Record
        clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", id_tipo_doc, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("numero_instrumento", par.codigo_par, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("descripcion_doc", descripcion, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("nom_beneficiario", usuario.nombre_usuario & " " & usuario.apellidos_usuario, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("comentarios", par.proposito, "id_documento", 0) '.Replace("'", "''")
        clss_approval.set_ta_documentoFIELDS("codigo_AID", par.codigo_par, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", par.codigo_par, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("ficha_actividad", "NO", "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("monto_ficha", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("regional", Me.Session("E_SubRegion").ToString.Trim, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_Approval", par.codigo_par, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("id_tipoAprobacion", 4, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("monto_total", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("tasa_cambio", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("datecreated", Date.UtcNow, "id_documento", 0)

        Dim id_documento = clss_approval.save_ta_documento()

        Dim tbl_Route_By_DOC As New DataTable

        tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipo_doc, 0) 'First Step
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
            tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipo_doc, 1) 'Next Step
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

            guardarRelacionDocumento(id_documento, par.id_par)

            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1023, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_PAR(CType(id_appdocumento, Integer), par.id_par)) Then
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
                ElseIf nDias < 17 Then
                    weekend = 6
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

    Sub guardarRelacionDocumento(ByVal idDocumento As Integer, ByVal idPar As Integer)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim documentoPar As New ta_documento_par
                documentoPar.id_par = idPar
                documentoPar.id_documento = idDocumento
                dbEntities.ta_documento_par.Add(documentoPar)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_agregar_fuente_Click(sender As Object, e As EventArgs) Handles btn_agregar_fuente.Click
        Try
            If Session("dtAportes") IsNot Nothing Then
                dtAportes = Session("dtAportes")
            Else
                createdtcolumsAportes()
            End If
            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio
            dtAportes.Rows.Add(idunique, Convert.ToInt32(Me.cmb_fuente_aporte.SelectedValue), Me.cmb_fuente_aporte.Text, False, 0)
            Session("dtAportes") = dtAportes
            fillGridAportes()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try
    End Sub

    Sub fillGridAportes()
        If Session("dtAportes") IsNot Nothing Then
            dtAportes = Session("dtAportes")
        Else
            createdtcolumsAportes()
        End If
        Me.grd_fuente_aporte.DataSource = dtAportes
        Me.grd_fuente_aporte.DataBind()
    End Sub
    Private Sub rbn_recursos_apalancados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_recursos_apalancados.SelectedIndexChanged
        Me.fuentesApalancamiento.Visible = If(Me.rbn_recursos_apalancados.SelectedValue = "0", False, True)
        fillGridAportes()
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
        guardarPar(False)
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
            Dim id_par = Convert.ToInt32(Me.idPar.Value)
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
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_PAR(CType(id_app_documento, Integer))) Then
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
                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If
                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1020, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If

                                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    'If (objEmail.Emailing_APPROVAL_PAR(CType(id_app_documento, Integer))) Then
                                    'Else 'Error mandando Email
                                    'End If
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_PAR(CType(CType(id_app_documento, Integer), Integer), id_par)) Then
                                    Else 'Error mandando Email
                                    End If


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
                                            If (objEmail.Emailing_APPROVAL_TRAVEL(CType(id_app_documento, Integer), CType(id_par, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "PAR-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
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
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
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
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If

                                End If

                            Else 'Error

                            End If


                    End Select


                    Me.Response.Redirect("~/administrativo/frm_par")


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