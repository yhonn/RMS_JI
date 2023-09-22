Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports Telerik.Web.UI.Calendar
Imports System.Data.Entity.Validation
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_parAD
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADM_PAR_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
    Dim PathArchivos = ""
    Dim dtConceptos As New DataTable
    Dim dtAportes As New DataTable
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
                LoadData()
            End If
        End Using

    End Sub
    Sub LoadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)

            Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            ''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("habilitar_agregar_viaje")
            dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitarViaje = ds.Tables("habilitar_agregar_viaje").Rows(0).Item(0)
            Me.habilitar_registro.Value = habilitarViaje


            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Me.lblt_usuario_solicita.Text = usuario.nombre_usuario & " " & usuario.apellidos_usuario & "(" & usuario.t_job_title.job & ")"
            Me.id_cargo.Value = usuario.id_job_title
            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.nombre_departamento).ToList()
            Me.cmb_departamento_entrega.DataSourceID = ""
            Me.cmb_departamento_entrega.DataSource = departamentos
            Me.cmb_departamento_entrega.DataTextField = "nombre_departamento"
            Me.cmb_departamento_entrega.DataValueField = "id_departamento"
            Me.cmb_departamento_entrega.SelectedValue = departamentos.FirstOrDefault().id_departamento
            Me.cmb_departamento_entrega.DataBind()


            Dim regionesUsuario = usuario.t_usuario_subregion.ToList()
            Me.cmb_sub_Region.DataSourceID = ""
            If regionesUsuario.Count() = 1 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                            New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                      Key .id_subregion = p.id_subregion,
                                                      Key .visible = p.t_subregiones.visible}).Where(Function(p) p.visible.Value = True).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.cmb_sub_Region.SelectedValue = subRegion.FirstOrDefault().id_subregion
            ElseIf regionesUsuario.Count() > 0 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion,
                                                       Key .visible = p.t_subregiones.visible}).Where(Function(p) p.visible.Value = True).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            Else
                Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.t_regiones.id_programa = ID).Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_regiones.nombre_region & " - " & p.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion,
                                                       Key .visible = p.visible}).Where(Function(p) p.visible.Value = True).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            End If


            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()


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

            Me.cmb_estrategia.DataSourceID = ""
            Me.cmb_estrategia.DataSource = dbEntities.tme_estrategia.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_estrategia.DataTextField = "estrategia"
            Me.cmb_estrategia.DataValueField = "id_estrategia"
            Me.cmb_estrategia.DataBind()


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

        fillGrid()
    End Sub

    Protected Sub btn_agregar_concepto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_concepto.Click
        Try
            If Session("dtConceptos") IsNot Nothing Then
                dtConceptos = Session("dtConceptos")
            Else
                createdtcolums()
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
            dtConceptos.Rows.Add(idunique, Me.txt_cantidad.Value, Me.txt_descripcion_concepto.Text, Me.txt_valor.Value, valor_total, Me.cmb_unidad_medida.Text, Convert.ToInt32(Me.cmb_unidad_medida.SelectedValue))
            Session("dtConceptos") = dtConceptos
            Me.txt_cantidad.Text = String.Empty
            Me.txt_valor.Text = String.Empty
            Me.txt_descripcion_concepto.Text = String.Empty
            'Me.txt_unidad_medida.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
            fillGrid()
            fillGridAportes()
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

    Sub fillGridAportes()
        If Session("dtAportes") IsNot Nothing Then
            dtAportes = Session("dtAportes")
        Else
            createdtcolumsAportes()
        End If
        Me.grd_fuente_aporte.DataSource = dtAportes
        Me.grd_fuente_aporte.DataBind()
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
        End If
    End Sub

    Sub createdtcolumsAportes()
        If dtAportes.Columns.Count = 0 Then
            dtAportes.Columns.Add("id_par_fuente_aporte", GetType(String))
            dtAportes.Columns.Add("id_aporte", GetType(Double))
            dtAportes.Columns.Add("nombre_aporte", GetType(String))
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

        Dim idTipo = Convert.ToInt32(Me.tipo_delete.Value)
        If idTipo = 1 Then
            Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

            Dim foundRow As DataRow() = dtConceptos.Select("id_par_detalle = '" & idConcepto.ToString() & "'")

            If foundRow.Count > 0 Then
                dtConceptos.Rows.Remove(foundRow(0))
            End If
            Session("dtConceptos") = dtConceptos
            fillGrid()
        ElseIf idTipo = 2 Then
            Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

            Dim foundRow As DataRow() = dtAportes.Select("id_par_fuente_aporte = '" & idConcepto.ToString() & "'")

            If foundRow.Count > 0 Then
                dtAportes.Rows.Remove(foundRow(0))
            End If
            Session("dtAportes") = dtAportes
            fillGridAportes()
        End If

        Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Try
            If validarValorPar() Then
                If guardarPar(False) Then
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
            numDiasHabiles = 7
        ElseIf valorParDolares >= 3500 Then
            numDiasHabiles = 14
        End If



        Dim fechaHabil2 = calculaDiaHabil(numDiasHabiles, DateTime.Now)
        Dim fechaHabil = New Date(fechaHabil2.Year, fechaHabil2.Month, fechaHabil2.Day, 0, 0, 0)
        'Dim numDias = DateDiff("d", DateTime.Now, Me.dt_fecha_requiere_servicios.SelectedDate)
        If valorParDolares <= 1000 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR inferiores a 1.000 USD se deben solicitar mínimo con 5 días hábiles de anticipación"
            habilitarGuardar = False
        ElseIf valorParDolares > 1000 And valorParDolares < 3500 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR entre 1.001 USD y 3.499 USD se deben solicitar mínimo con 8 días hábiles de anticipación"
            habilitarGuardar = False
        ElseIf valorParDolares >= 3500 And Me.dt_fecha_requiere_servicios.SelectedDate < fechaHabil Then
            mensaje = "Los PAR mayores a 3.499 USD se deben solicitar mínimo con 15 días hábiles de anticipación"
            habilitarGuardar = False
        End If

        If habilitarGuardar = False Then
            Me.alerta_dias.Visible = True
            Me.lbl_alerta_solicitud_par.Text = mensaje
        End If


        Dim habilitarRegistro = Convert.ToBoolean(Me.habilitar_registro.Value)
        If habilitarRegistro = True Then
            Me.alerta_dias.Visible = False
            Return True
        Else
            Return habilitarGuardar
        End If
    End Function
    Protected Sub btn_guardar_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar_2.Click
        Try
            If validarValorPar() Then
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

    End Sub

    Public Function guardarDocumento(ByVal par As vw_tme_par, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2044
        Dim cls_par As APPROVAL.clss_par = New APPROVAL.clss_par(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalPar As DataTable = New DataTable

        If par.valor_total / par.tasa_ser_cotizacion > 50000 Then
            tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_50000(par.id_usuario, id_categoriaAPP)
        ElseIf par.valor_total / par.tasa_ser_cotizacion > 3500 And par.id_tipo_par = 2 Then
            tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_3500(par.id_usuario)
        ElseIf par.asociado_comunicaciones = True Then
            tblUserApprovalPar = cls_par.get_parApprovalUser_comunicaciones(par.id_usuario, id_categoriaAPP)
        ElseIf par.id_tipo_par = 1 Then
            tblUserApprovalPar = cls_par.get_parApprovalUser_administrativo(par.id_usuario, id_categoriaAPP)
        Else
            tblUserApprovalPar = cls_par.get_parApprovalUserEventos(par.id_usuario, id_categoriaAPP)
        End If



        Dim id_tipo_doc = tblUserApprovalPar.Rows(0).Item("id_tipoDocumento")



        Dim descripcion = String.Format("Solicitud de par {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, par.fecha_solicitud)
        Dim err As Boolean = False
        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))


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
        clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApprovalPar.Rows(0).Item("id_rol"), "id_app_documento", 0)
        clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

        Dim id_appdocumento = clss_approval.save_ta_AppDocumento()
        If id_appdocumento <> -1 Then
            tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(id_tipo_doc, 1) 'Next Step
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

    'Private Sub cmb_regional_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_regional.SelectedIndexChanged
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

    Public Function guardarPar(ByVal enviarAprobacion As Boolean) As Boolean
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Try
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
                    Dim oPar = New tme_pares
                    oPar.id_usuario_crea = Me.Session("E_IdUser").ToString()
                    Dim usuario = dbEntities.t_usuarios.Find(oPar.id_usuario_crea)
                    Dim id_par = Convert.ToInt32(Me.idPar.Value)
                    If id_par = 0 Then


                        Dim codInicialPT = 388
                        oPar.fecha_crea = Date.Now
                        Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                        Dim subR = dbEntities.t_subregiones.Find(idSubRegion)
                        oPar.fecha_solicitud = Me.dt_fecha_solicitud.SelectedDate
                        oPar.tasa_ser_cotizacion = Me.txt_tasa_ser.Value
                        oPar.id_tasa_ser_cotizacion = Convert.ToInt32(Me.id_tasa_ser.Value)
                        oPar.id_region = subR.id_region
                        oPar.fecha_requiere_servicio = Me.dt_fecha_requiere_servicios.SelectedDate
                        oPar.id_tipo_par = Convert.ToInt32(Me.cmb_tipo_par.SelectedValue)
                        oPar.id_subregion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                        oPar.id_estrategia = Me.cmb_estrategia.SelectedValue
                        Dim subRegion = dbEntities.t_subregiones.Find(oPar.id_subregion)


                        Dim parSubregion = dbEntities.tme_pares.Where(Function(p) p.t_subregiones.id_cod_reg = subRegion.id_cod_reg).ToList()

                        Dim consecutivoPar = subRegion.t_consecutivo_par_regional.consecutivo_inicial + parSubregion.Count()




                        Dim codPar = ""
                        'If parSubregion.Count() > 0 Then
                        '    consecutivoPar = subRegion.t_consecutivo_par_regional.consecutivo_inicial + parSubregion.Count()
                        'Else
                        'End If
                        If consecutivoPar < 1000 And consecutivoPar > 99 Then
                            codPar = "0"
                        ElseIf consecutivoPar < 100 And consecutivoPar > 9 Then
                            codPar = "00"
                        ElseIf consecutivoPar < 10 Then
                            codPar = "000"
                        End If




                        oPar.codigo_par = "JI-" & subRegion.prefijo_subregion & "-" & codPar & consecutivoPar

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

                            oPar.codigo_pt = "PT-" & codPT & consecutivoPT
                            'oPar.codigo_pt = Me.txt_codigo_pt.Text
                            'oPar.id_estructura_marco_logico = Convert.ToInt32(Me.cmb_componente.SelectedValue)
                            oPar.fecha_inicio_evento = Me.dt_fecha_inicio_evento.SelectedDate
                            oPar.fecha_fin_evento = Me.dt_fecha_finalizacion_evento.SelectedDate
                            oPar.id_tipo_evento = Convert.ToInt32(Me.cmb_tipo_evento.SelectedValue)
                            oPar.numero_horas = Me.txt_nro_horas.Value
                            oPar.nombre_evento = Me.txt_nombre_evento.Text
                            oPar.id_entidad_evento = Convert.ToInt32(Me.cmb_entidad.SelectedValue)
                            'oPar.usuario_responable_evento = Me.txt_responsable.Text
                            'oPar.entidad_responsable_evento = Me.txt_entidad.Text
                            oPar.asociado_recursos_apalancados = If(Me.rbn_recursos_apalancados.SelectedValue = "1", True, False)
                        End If

                        dbEntities.tme_pares.Add(oPar)
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
                        Dim id_uMedida As Decimal

                        For Each row As DataRow In dtConceptos.Rows
                            Dim oParDetalle As New tme_par_detalle
                            cantidad = row("cantidad")
                            descripcion = row("descripcion")
                            valorUnitario = row("valor_unitario")
                            unidadMedida = row("unidad_medida")
                            valorTotal = row("valor")
                            id_uMedida = row("id_unidad_medida")

                            oParDetalle.id_par = oPar.id_par
                            oParDetalle.cantidad = cantidad
                            oParDetalle.descripcion = descripcion
                            oParDetalle.precio_unitario = valorUnitario
                            'oParDetalle.unidad_medida = unidadMedida
                            oParDetalle.id_unidad_medida = id_uMedida
                            dbEntities.tme_par_detalle.Add(oParDetalle)
                            dbEntities.SaveChanges()
                        Next


                        Dim id_aporte As Decimal

                        For Each row As DataRow In dtAportes.Rows
                            Dim oParAporte As New tme_par_aporte
                            id_aporte = row("id_aporte")


                            oParAporte.id_par = oPar.id_par
                            oParAporte.id_aporte = id_aporte
                            dbEntities.tme_par_aporte.Add(oParAporte)
                            dbEntities.SaveChanges()
                        Next

                        For Each row In Me.grd_componente.Items
                            If TypeOf row Is GridDataItem Then
                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                                If idComponente.Checked = True Then
                                    Dim idEstructura As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                                    Dim oMarco = New tme_par_marco_logico
                                    oMarco.id_par = oPar.id_par
                                    oMarco.id_marco_logico = idEstructura
                                    dbEntities.tme_par_marco_logico.Add(oMarco)
                                    dbEntities.SaveChanges()
                                End If
                            End If
                        Next

                        If enviarAprobacion Then

                            Dim id_categoriaAPP = 2044
                            Dim cls_par As APPROVAL.clss_par = New APPROVAL.clss_par(Convert.ToInt32(Me.Session("E_IDprograma")))

                            Dim tblUserApprovalPar As DataTable = New DataTable
                            Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_par = oPar.id_par).FirstOrDefault()

                            If par.valor_total / par.tasa_ser_cotizacion > 50000 Then
                                tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_50000(par.id_usuario, id_categoriaAPP)
                            ElseIf par.valor_total / par.tasa_ser_cotizacion > 3500 And par.id_tipo_par = 2 Then
                                tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_3500(par.id_usuario)
                            ElseIf par.asociado_comunicaciones = True Then
                                tblUserApprovalPar = cls_par.get_parApprovalUser_comunicaciones(par.id_usuario, id_categoriaAPP)
                            ElseIf par.id_tipo_par = 1 Then
                                tblUserApprovalPar = cls_par.get_parApprovalUser_administrativo(par.id_usuario, id_categoriaAPP)
                            Else
                                tblUserApprovalPar = cls_par.get_parApprovalUserEventos(par.id_usuario, id_categoriaAPP)
                            End If


                            If tblUserApprovalPar.Rows.Count() = 0 Then
                                Me.lblerr_user.Text = "El PAR " & par.codigo_par & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de PAR, contáctese con el administrador."
                                Me.lblerr_user.Visible = True
                                Me.idPar.Value = par.id_par
                                guardarPar = False
                            Else
                                Dim id_documento = guardarDocumento(par, usuario)
                                guardarPar = True
                            End If

                        Else
                            guardarPar = True
                        End If


                    Else
                        oPar = dbEntities.tme_pares.Where(Function(p) p.id_par = id_par).ToList().FirstOrDefault()

                        Dim id_categoriaAPP = 2044
                        Dim cls_par As APPROVAL.clss_par = New APPROVAL.clss_par(Convert.ToInt32(Me.Session("E_IDprograma")))

                        Dim tblUserApprovalPar As DataTable = New DataTable
                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_par = oPar.id_par).FirstOrDefault()

                        If par.valor_total / par.tasa_ser_cotizacion > 50000 Then
                            tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_50000(par.id_usuario, id_categoriaAPP)
                        ElseIf par.valor_total / par.tasa_ser_cotizacion > 3500 And par.id_tipo_par = 2 Then
                            tblUserApprovalPar = cls_par.get_parApprovalUser_mayor_3500(par.id_usuario)
                        ElseIf par.asociado_comunicaciones = True Then
                            tblUserApprovalPar = cls_par.get_parApprovalUser_comunicaciones(par.id_usuario, id_categoriaAPP)
                        ElseIf par.id_tipo_par = 1 Then
                            tblUserApprovalPar = cls_par.get_parApprovalUser_administrativo(par.id_usuario, id_categoriaAPP)
                        Else
                            tblUserApprovalPar = cls_par.get_parApprovalUserEventos(par.id_usuario, id_categoriaAPP)
                        End If


                        If tblUserApprovalPar.Rows.Count() = 0 Then
                            Me.lblerr_user.Text = "El PAR " & par.codigo_par & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de PAR, contáctese con el administrador."
                            Me.lblerr_user.Visible = True
                            guardarPar = False
                        Else
                            Dim id_documento = guardarDocumento(par, usuario)
                            guardarPar = True
                        End If


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
            dtAportes.Rows.Add(idunique, Convert.ToInt32(Me.cmb_fuente_aporte.SelectedValue), Me.cmb_fuente_aporte.Text)
            Session("dtAportes") = dtAportes
            fillGridAportes()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try
    End Sub

    Private Sub rbn_recursos_apalancados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_recursos_apalancados.SelectedIndexChanged
        Me.fuentesApalancamiento.Visible = If(Me.rbn_recursos_apalancados.SelectedValue = "0", False, True)
        fillGridAportes()
    End Sub
End Class