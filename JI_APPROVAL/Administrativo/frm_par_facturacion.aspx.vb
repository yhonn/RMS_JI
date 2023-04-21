Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports Telerik.Web.UI.Calendar
Imports System.Data.Entity.Validation

Public Class frm_par_facturacion
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
            PathArchivos = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder
            If Not Me.IsPostBack Then
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                Session.Remove("dtConceptos")
                Session.Remove("dtAportes")
                Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
                Me.idPar.Value = id_par
                Dim es_Edicion = Convert.ToInt32(Me.Request.QueryString("e"))
                Me.esEdicion.Value = es_Edicion
                LoadList()
                fillGrid()
                LoadData(id_par)
                'LoadData(id_par)
            End If
        End Using
    End Sub
    Sub fillGrid2()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        Me.grd_conceptos.DataSource = dtConceptos
        Me.grd_conceptos.DataBind()
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
    Sub loadMunicipios()
        Using dbEntities As New dbRMS_JIEntities
            Dim municipios As List(Of t_municipios)
            Dim idDepto = Convert.ToInt32(Me.cmb_departamento_entrega.SelectedValue)
            municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).ToList()
            Me.cmb_municipio_entrega.ClearSelection()
            Me.cmb_municipio_entrega.DataSourceID = ""
            Me.cmb_municipio_entrega.DataSource = municipios
            Me.cmb_municipio_entrega.DataTextField = "nombre_municipio"
            Me.cmb_municipio_entrega.DataValueField = "id_municipio"
            Me.cmb_municipio_entrega.DataBind()
        End Using

    End Sub
    Sub LoadData(ByVal id_par As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If


        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.tme_pares.FirstOrDefault(Function(p) p.id_par = id_par)
            If par.id_tipo_par = 2 Then
                Me.asistentes_evento.Visible = True
            End If
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


            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()



            'Me.cmb_regional.SelectedValue = par.id_region
            'cargarSubRegion()
            Me.cmb_proposito_par.DataSourceID = ""
            Me.cmb_proposito_par.DataSource = dbEntities.tme_tipo_solicitud_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_proposito_par.DataTextField = "tipo_solicitud"
            Me.cmb_proposito_par.DataValueField = "id_tipo_solicitud"
            Me.cmb_proposito_par.DataBind()

            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_departamento_entrega.DataSourceID = ""
            Me.cmb_departamento_entrega.DataSource = departamentos
            Me.cmb_departamento_entrega.DataTextField = "nombre_departamento"
            Me.cmb_departamento_entrega.DataValueField = "id_departamento"
            Me.cmb_departamento_entrega.SelectedValue = par.t_municipios.id_departamento
            Me.cmb_departamento_entrega.DataBind()

            Dim municipios As List(Of t_municipios)
            Dim idDepto = Convert.ToInt32(Me.cmb_departamento_entrega.SelectedValue)
            municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = par.t_municipios.id_departamento).ToList()
            Me.cmb_municipio_entrega.ClearSelection()
            Me.cmb_municipio_entrega.DataSourceID = ""
            Me.cmb_municipio_entrega.DataSource = municipios
            Me.cmb_municipio_entrega.DataTextField = "nombre_municipio"
            Me.cmb_municipio_entrega.DataValueField = "id_municipio"
            Me.cmb_municipio_entrega.DataBind()

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
            Me.cmb_tipo_par.DataSourceID = ""
            Me.cmb_tipo_par.DataSource = dbEntities.tme_tipo_par.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_tipo_par.DataTextField = "tipo_par"
            Me.cmb_tipo_par.DataValueField = "id_tipo_par"
            Me.cmb_tipo_par.DataBind()

            Me.cmb_cargo_par.DataSourceID = ""
            Me.cmb_cargo_par.DataSource = dbEntities.tme_cargo_pares.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_cargo_par.DataTextField = "cargo"
            Me.cmb_cargo_par.DataValueField = "id_cargo_a_par"
            Me.cmb_cargo_par.DataBind()


            Me.cmb_tipo_par.SelectedValue = par.id_tipo_par
            Me.cmb_cargo_par.SelectedValue = par.id_cargo_a
            'Me.txt_codigo_facturacion.Text = par.codigo_facturación
            Me.rbn_comunicaciones.SelectedValue = If(par.asociado_comunicaciones = True, 1, 0)
            Me.txt_proposito_servicio.Text = par.proposito


            Me.grd_detalle_par.DataSource = par.tme_par_detalle.Select(Function(p) New With {Key .descripcion = p.descripcion,
                Key .cantidad = p.cantidad,
                Key .valor_unitario = p.precio_unitario,
                Key .valor = p.cantidad * p.precio_unitario,
                Key .unidad_medida = p.tme_unidad_medida_par.unidad_medida,
                Key .id_par_detalle = p.id_par_detalle}).ToList()

            Me.grd_detalle_par.DataBind()

            fillGridFacturas()


            Dim parDetalle = dbEntities.vw_tme_par.FirstOrDefault(Function(p) p.id_par = par.id_par)
            Dim intOwnerFacturacion As String() = parDetalle.id_usuario_radica.ToString.Split(",")
            Dim idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)

            Me.btn_guardar.Visible = False

            If intOwnerFacturacion.Where(Function(p) p.Contains(idUser)).Count() > 0 And es_Edicion = 1 Then
                Me.btn_guardar_finalizar.Visible = False
                Me.btn_guardar.Visible = True
            End If
        End Using
    End Sub

    Sub fillGridFacturas()
        Using dbEntities As New dbRMS_JIEntities
            Me.btn_solicitar_ajuste_par.Visible = False
            Dim id_par = Convert.ToInt32(Me.idPar.Value)
            Dim oPar = dbEntities.tme_pares.Find(id_par)
            Me.grd_factura.DataSource = dbEntities.vw_tme_par_factura.Where(Function(p) p.id_par = id_par).ToList()
            Me.grd_factura.DataBind()

            Dim parDetalle = dbEntities.tme_par_detalle.Where(Function(p) p.id_par = id_par).ToList()
            Dim parFacturacion = dbEntities.tme_par_detalle_factura.Where(Function(p) p.tme_par_factura.id_par = id_par).ToList()
            Dim habilitarGuardar = False
            If parFacturacion.Count() > 0 Or oPar.id_cargo_a = 6 Or oPar.tme_par_detalle.Sum(Function(p) p.valor_total) = 0 Then
                habilitarGuardar = True
            End If
            If parDetalle.Count() > 0 And parDetalle.Count() > 0 Then
                Dim totalDetalle = parDetalle.Sum(Function(p) p.valor_total)
                Dim totalFactura = parFacturacion.Sum(Function(p) p.precio_unitario * p.cantidad)

                If ((totalFactura * 100) / totalDetalle) - 100 > 10 Then
                    habilitarGuardar = False
                    Me.solicitar_ajuste.Visible = True
                    Me.btn_solicitar_ajuste_par.Visible = True
                Else
                    Me.solicitar_ajuste.Visible = False
                End If

            End If

            Me.btn_guardar_finalizar.Enabled = habilitarGuardar

        End Using
    End Sub

    Sub LoadList()
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

            Dim categoria = dbEntities.tme_categoria_factura.Where(Function(p) p.id_programa = id_programa).ToList()

            Me.cmb_categoria_factura.DataSourceID = ""
            Me.cmb_categoria_factura.DataSource = categoria
            Me.cmb_categoria_factura.DataTextField = "categoria"
            Me.cmb_categoria_factura.DataValueField = "id_categoria_factura_par"
            Me.cmb_categoria_factura.DataBind()

            Dim uMedida = dbEntities.tme_unidad_medida_par.Where(Function(p) p.id_programa = id_programa).ToList()

            Me.cmb_unidad_medida.DataSourceID = ""
            Me.cmb_unidad_medida.DataSource = uMedida
            Me.cmb_unidad_medida.DataTextField = "unidad_medida"
            Me.cmb_unidad_medida.DataValueField = "id_unidad_media"
            Me.cmb_unidad_medida.DataBind()



        End Using
    End Sub
    Protected Sub add_factura_Click(sender As Object, e As EventArgs) Handles add_factura.Click
        Me.RadWindow2.VisibleOnPageLoad = True
        Me.RadWindow2.Visible = True

        LoadList()
        'Dim funcion = "FuncApprobals()"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)
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
            Me.cmb_unidad_medida.ClearSelection()
            Me.cmb_unidad_medida.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
            Me.RadWindow2.VisibleOnPageLoad = True
            Me.RadWindow2.Visible = True
            fillGrid()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_factura_detalle", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("valor", GetType(Double))
            dtConceptos.Columns.Add("unidad_medida", GetType(String))
            dtConceptos.Columns.Add("id_unidad_medida", GetType(Integer))
        End If
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
    Protected Sub delete_concepto(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.id_concpeto.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipo_delete.Value = 1

        Me.RadWindow2.VisibleOnPageLoad = False


        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub grd_conceptos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_conceptos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_factura_detalle").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_factura_detalle").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_factura_detalle").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub

    Private Sub btn_agregar_factura_Click(sender As Object, e As EventArgs) Handles btn_agregar_factura.Click

        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim id_par = Convert.ToInt32(Me.idPar.Value)
            Dim oPar = dbEntities.tme_pares.Find(id_par)
            Dim factura = New tme_par_factura
            factura.id_par = id_par
            factura.numero_factura = Me.txt_numero_factura.Text
            factura.fecha_crea = DateTime.Now
            factura.proveedor = Me.txt_proveedor.Text
            factura.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            factura.id_categoria_factura = Convert.ToInt32(Me.cmb_categoria_factura.SelectedValue)
            For Each file As UploadedFile In soporte_adjunto.UploadedFiles
                Dim fecha = DateTime.Now
                Dim exten = file.GetExtension()
                Dim nombreArchivo = "Fact_" & fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & oPar.codigo_par & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten

                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)

                factura.soporte = nombreArchivo
            Next
            dbEntities.tme_par_factura.Add(factura)
            dbEntities.SaveChanges()


            Dim cantidad As Decimal
            Dim descripcion As String
            Dim unidadMedida As String
            Dim valorUnitario As Decimal
            Dim valorTotal As Decimal
            Dim id_uMedida As Decimal

            For Each row As DataRow In dtConceptos.Rows
                Dim oParDetalle As New tme_par_detalle_factura
                cantidad = row("cantidad")
                descripcion = row("descripcion")
                valorUnitario = row("valor_unitario")
                unidadMedida = row("unidad_medida")
                valorTotal = row("valor")
                id_uMedida = row("id_unidad_medida")

                oParDetalle.id_factura = factura.id_par_factura
                oParDetalle.cantidad = cantidad
                oParDetalle.descripcion = descripcion
                oParDetalle.precio_unitario = valorUnitario
                'oParDetalle.unidad_medida = unidadMedida
                oParDetalle.id_unidad_medida = id_uMedida
                dbEntities.tme_par_detalle_factura.Add(oParDetalle)
                dbEntities.SaveChanges()
            Next

            Me.txt_numero_factura.Text = String.Empty
            Me.txt_proveedor.Text = String.Empty

            Me.RadWindow2.VisibleOnPageLoad = False
            'Me.RadWindow2.Visible = False
            Me.cmb_unidad_medida.ClearSelection()
            Me.cmb_categoria_factura.ClearSelection()
            dtConceptos = New DataTable
            Session.Remove("dtConceptos")

            fillGridFacturas()
            fillGrid()
        End Using
    End Sub

    Private Sub btn_guardar_finalizar_Click(sender As Object, e As EventArgs) Handles btn_guardar_finalizar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim par = dbEntities.tme_pares.Find(id_par)
                par.fecha_radicacion = DateTime.Now
                par.id_usuario_radico = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                par.numero_asistentes_evento = Me.txt_nro_asistentes.Value
                dbEntities.Entry(par).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btn_solicitar_ajuste_par_Click(sender As Object, e As EventArgs) Handles btn_solicitar_ajuste_par.Click
        Try
            Using dbEntities As New dbRMS_JIEntities

                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim par = dbEntities.tme_pares.Find(id_par)
                Dim parDetalle = dbEntities.tme_par_detalle.Where(Function(p) p.id_par = id_par).ToList()
                Dim parFacturacion = dbEntities.tme_par_detalle_factura.Where(Function(p) p.tme_par_factura.id_par = id_par).ToList()
                Dim documentoPar = par.ta_documento_par.LastOrDefault()
                Dim documento = documentoPar.ta_documento
                Dim appDocumento = documento.ta_AppDocumento.Where(Function(p) p.id_estadoDoc = 7).LastOrDefault()

                appDocumento.id_estadoDoc = 4
                appDocumento.observacion = "Se cancela el PAR " & par.codigo_par & " debido a la facturación supera el 10% del valor del PAR"

                par.par_cancelado = True

                Dim parAudit = New tme_par_audit
                parAudit.id_usuario_audit = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                parAudit.fecha_audit = DateTime.Now
                parAudit.id_documento_par = documentoPar.id_documento_par
                parAudit.observaciones = "Se cancela el PAR " & par.codigo_par & " debido a la facturación supera el 10% del valor del PAR"
                parAudit.valor_inicial_par = parDetalle.Sum(Function(p) p.valor_total)
                parAudit.valor_facturacion = parFacturacion.Sum(Function(p) p.precio_unitario * p.cantidad)
                dbEntities.tme_par_audit.Add(parAudit)

                dbEntities.Entry(appDocumento).State = Entity.EntityState.Modified
                dbEntities.Entry(par).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grd_factura_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_factura.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkSoporte As HyperLink = New HyperLink
            hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)

            If DataBinder.Eval(e.Item.DataItem, "soporte") IsNot Nothing Then
                Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()
                hlnkSoporte.NavigateUrl = PathArchivos & soporte
            Else
                hlnkSoporte.Visible = False
            End If

            Dim id_legalizacion_viaje_detalle = DataBinder.Eval(e.Item.DataItem, "id_par_factura").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_par_factura").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_par_factura").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub

    Protected Sub delete_detalle(sender As Object, e As EventArgs)
        Me.tipo_delete.Value = 2
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click

        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Dim idTipo = Convert.ToInt32(Me.tipo_delete.Value)
        If idTipo = 1 Then
            Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

            Dim foundRow As DataRow() = dtConceptos.Select("id_factura_detalle = '" & idConcepto.ToString() & "'")

            If foundRow.Count > 0 Then
                dtConceptos.Rows.Remove(foundRow(0))
            End If
            Session("dtConceptos") = dtConceptos
            'fillGrid()

            Me.RadWindow2.VisibleOnPageLoad = True
            Me.RadWindow2.Visible = True

            fillGrid()
        ElseIf idTipo = 2 Then
            Using dbEntities As New dbRMS_JIEntities
                Dim idDetalle = Convert.ToInt32(Me.identity.Value)
                Dim detalle = dbEntities.tme_par_factura.Find(idDetalle)
                If detalle IsNot Nothing Then
                    dbEntities.Entry(detalle).State = Entity.EntityState.Deleted
                    dbEntities.SaveChanges()
                End If
                fillGridFacturas()
                Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        End If



    End Sub


    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim par = dbEntities.tme_pares.Find(id_par)
                par.fecha_radicacion = DateTime.Now
                par.id_usuario_radico = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                par.numero_asistentes_evento = Me.txt_nro_asistentes.Value
                dbEntities.Entry(par).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                Dim es_Edicion = Convert.ToInt32(Me.esEdicion.Value)
                If es_Edicion = 1 Then
                    Dim parPermisos = dbEntities.tme_par_permisos.Where(Function(p) p.id_par = id_par).ToList().FirstOrDefault()
                    If parPermisos IsNot Nothing Then
                        parPermisos.habilitar_facturacion = False
                        dbEntities.Entry(parPermisos).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If
                End If


            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception

        End Try
    End Sub
End Class