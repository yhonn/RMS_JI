Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Imports ly_APPROVAL
Public Class frm_anticipoLegalizacion
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_ANTICIPOS_LEG"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtRutas As New DataTable
    Dim dtCompras As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Dim dtConceptos As New DataTable
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Dim idAnticipo = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.id_anticipo.Value = idAnticipo
            Session.Remove("dtConceptos")
            loadLista()
            loadData(idAnticipo)
            loadRutas(idAnticipo)
            loadCompras(idAnticipo)
            loadParticipantes(idAnticipo)
            fillGridFacturas()
            fillGrid()
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
            Dim idAnticipo = Convert.ToInt32(Me.id_anticipo.Value)
            Dim oAnticipo = dbEntities.tme_anticipos.Find(idAnticipo)
            Dim factura = New tme_anticipos_facturas
            factura.id_anticipo = idAnticipo
            factura.numero_factura = Me.txt_numero_factura.Text
            factura.fecha_crea = DateTime.Now
            factura.proveedor = Me.txt_proveedor.Text
            For Each file As UploadedFile In soporte_adjunto.UploadedFiles
                Dim fecha = DateTime.Now
                Dim exten = file.GetExtension()
                Dim nombreArchivo = "Fact_" & fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & oAnticipo.codigo_anticipo & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten

                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)

                factura.soporte = nombreArchivo
            Next
            dbEntities.tme_anticipos_facturas.Add(factura)
            dbEntities.SaveChanges()


            Dim cantidad As Decimal
            Dim descripcion As String
            Dim unidadMedida As String
            Dim valorUnitario As Decimal
            Dim valorTotal As Decimal

            For Each row As DataRow In dtConceptos.Rows
                Dim oAnticipoDetalle As New tme_anticipos_detalle_factura
                cantidad = row("cantidad")
                descripcion = row("descripcion")
                valorUnitario = row("valor_unitario")
                valorTotal = row("valor")

                oAnticipoDetalle.id_anticipo_factura = factura.id_anticipo_factura
                oAnticipoDetalle.cantidad = cantidad
                oAnticipoDetalle.descripcion = descripcion
                oAnticipoDetalle.precio_unitario = valorUnitario
                'oAnticipoDetalle.unidad_medida = unidadMedida
                dbEntities.tme_anticipos_detalle_factura.Add(oAnticipoDetalle)
                dbEntities.SaveChanges()
            Next

            Me.txt_numero_factura.Text = String.Empty
            Me.txt_proveedor.Text = String.Empty

            Me.RadWindow2.VisibleOnPageLoad = False
            'Me.RadWindow2.Visible = False
            dtConceptos = New DataTable
            Session.Remove("dtConceptos")

            fillGridFacturas()
            fillGrid()
        End Using
    End Sub
    Sub fillGridFacturas()
        Using dbEntities As New dbRMS_JIEntities
            Dim idAnticipo = Convert.ToInt32(Me.id_anticipo.Value)

            Dim facturas = dbEntities.tme_anticipos_facturas.Where(Function(p) p.id_anticipo = idAnticipo).Select(Function(p) New With {Key .numero_factura = p.numero_factura,
                                                      Key .proveedor = p.proveedor, .id_anticipo_factura = p.id_anticipo_factura,
                                                      .soporte = p.soporte,
                                                      .total = p.tme_anticipos_detalle_factura.Sum(Function(x) x.cantidad * x.precio_unitario)}).ToList()

            Me.grd_factura.DataSource = facturas
            Me.grd_factura.DataBind()

        End Using
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
            dtConceptos.Columns.Add("id_factura_detalle", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("valor", GetType(Double))
        End If
    End Sub
    Sub loadRutas(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim rutas = dbEntities.vw_anticipo_detalle_ruta.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_rutas.DataSource = rutas
            Me.grd_rutas.DataBind()
        End Using
    End Sub

    Sub loadCompras(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim rutas = dbEntities.tme_anticipo_compra.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_compras.DataSource = rutas
            Me.grd_compras.DataBind()
        End Using
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
            dtConceptos.Rows.Add(idunique, Me.txt_cantidad.Value, Me.txt_descripcion_concepto.Text, Me.txt_valor.Value, valor_total)
            Session("dtConceptos") = dtConceptos
            Me.txt_cantidad.Text = String.Empty
            Me.txt_valor.Text = String.Empty
            Me.txt_descripcion_concepto.Text = String.Empty
            'Me.txt_unidad_medida.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
            Me.RadWindow2.VisibleOnPageLoad = True
            Me.RadWindow2.Visible = True
            fillGrid()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Sub loadParticipantes(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim participantes = (From a In dbEntities.tme_anticipo_ruta_participantes
                                 Join b In dbEntities.vw_anticipo_detalle_ruta On a.id_ruta_anticipo Equals b.id_anticipo_ruta
                                 Where a.tme_anticipo_ruta.id_anticipo = idAnticipo
                                 Select a.numero_documento, a.primer_apellido, a.segundo_apellido, a.valor, a.nombres,
                                     a.tipo_ocumento, a.telefono, b.num_ruta, a.id_participante, a.id_ruta_anticipo, a.retiro_fondos).ToList()

            Me.grd_participantes_resumen.DataSource = participantes
            Me.grd_participantes_resumen.DataBind()


            For Each row In Me.grd_participantes_resumen.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim retiroFondos As RadioButtonList = CType(row.Cells(0).FindControl("rbn_retiro_fondos"), RadioButtonList)
                    Dim idParticipante As Integer = dataItem.GetDataKeyValue("id_participante")
                    Dim retiro_fondos = dataItem.GetDataKeyValue("retiro_fondos")
                    If retiro_fondos IsNot Nothing Then
                        retiroFondos.SelectedValue = 2
                        If retiro_fondos Then
                            retiroFondos.SelectedValue = 1
                        Else
                            retiroFondos.SelectedValue = 2
                        End If
                    End If
                End If

            Next

        End Using
    End Sub
    Protected Sub grd_rutas_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_rutas.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta").ToString()
            Dim lbl_tiempo_estimado As Label = CType(e.Item.Cells(0).FindControl("lbl_tiempo_estimado"), Label)
            lbl_tiempo_estimado.Text = DataBinder.Eval(e.Item.DataItem, "tiempo_estimado")
            Dim lbl_observaciones_trayecto As Label = CType(e.Item.Cells(0).FindControl("lbl_observaciones_trayecto"), Label)
            lbl_observaciones_trayecto.Text = DataBinder.Eval(e.Item.DataItem, "observaciones_trayecto")
            Dim lbl_observaciones_ruta As Label = CType(e.Item.Cells(0).FindControl("lbl_observaciones_ruta"), Label)
            lbl_observaciones_ruta.Text = DataBinder.Eval(e.Item.DataItem, "observaciones_ruta")
        End If
    End Sub
    Sub loadData(idAnticipo As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim anticipo = dbEntities.tme_anticipos.Find(idAnticipo)
            If anticipo.id_tipo_anticipo = 2 Then
                Me.info_rutas.Visible = True
                Me.info_participantes.Visible = True
                Me.info_leg_compras.Visible = False
            Else
                Me.info_rutas.Visible = False
                Me.info_participantes.Visible = False
                Me.info_leg_compras.Visible = True
            End If
            Me.cmb_sub_Region.SelectedValue = anticipo.id_subregion
            Me.dt_fecha_anticipo.SelectedDate = anticipo.fecha_anticipo
            Me.cmb_medio_pago.SelectedValue = anticipo.id_medio_pago
            Me.txt_detalle_medio_pago.Text = anticipo.observaciones_medio_pago
            Me.cmb_tipo_anticipo.SelectedValue = anticipo.id_tipo_anticipo
            loadPar()
            Me.cmb_par.SelectedValue = anticipo.id_par
            Me.txt_motivo_anticipo.Text = anticipo.motivo
        End Using
    End Sub
    Sub loadPar()
        Using dbEntities As New dbRMS_JIEntities
            'Me.anticipo_fondos.Visible = False
            'Me.info_par.Visible = False
            Me.cmb_par.ClearSelection()
            Me.cmb_par.DataSourceID = ""
            If Me.cmb_tipo_anticipo.SelectedValue = "2" And Me.cmb_sub_Region.SelectedValue <> "" Then
                'Me.info_par.Visible = True
                'Me.anticipo_fondos.Visible = True
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 2).OrderByDescending(Function(p) p.id_par).ToList()
            ElseIf Me.cmb_tipo_anticipo.SelectedValue = "1" And Me.cmb_sub_Region.SelectedValue <> "" Then
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 1).OrderByDescending(Function(p) p.id_par).ToList()
            End If

            Me.cmb_par.DataTextField = "codigo_par"
            Me.cmb_par.DataValueField = "id_par"
            Me.cmb_par.DataBind()
        End Using
    End Sub
    Sub loadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)
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

            Me.cmb_medio_pago.DataSourceID = ""
            Me.cmb_medio_pago.DataSource = dbEntities.tme_medio_pago.ToList()
            Me.cmb_medio_pago.DataTextField = "medio_pago"
            Me.cmb_medio_pago.DataValueField = "id_medio_pago"
            Me.cmb_medio_pago.DataBind()

            Me.cmb_tipo_anticipo.DataSourceID = ""
            Me.cmb_tipo_anticipo.DataSource = dbEntities.tme_tipo_par.ToList()
            Me.cmb_tipo_anticipo.DataTextField = "tipo_par"
            Me.cmb_tipo_anticipo.DataValueField = "id_tipo_par"
            Me.cmb_tipo_anticipo.DataBind()
        End Using

    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        If guardar(False) Then
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_anticipos"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Private Sub btn_guardar_enviar_Click(sender As Object, e As EventArgs) Handles btn_guardar_enviar.Click
        If guardar(True) Then
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_anticipos"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Public Function guardar(ByVal enviarAprobacion As Boolean) As Boolean
        Try
            Using db As New dbRMS_JIEntities
                Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                Dim usuario = db.t_usuarios.Find(id_usuario)
                For Each row In Me.grd_participantes_resumen.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim retiroFondos As RadioButtonList = CType(row.Cells(0).FindControl("rbn_retiro_fondos"), RadioButtonList)
                        Dim idParticipante As Integer = dataItem.GetDataKeyValue("id_participante")

                        Dim participante = db.tme_anticipo_ruta_participantes.Find(idParticipante)
                        If participante IsNot Nothing Then
                            If retiroFondos.SelectedValue <> "" Then
                                participante.retiro_fondos = If(retiroFondos.SelectedValue = 1, True, False)
                                db.Entry(participante).State = Entity.EntityState.Modified
                                db.SaveChanges()
                            End If
                        End If
                    End If
                Next
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim idAnticipo = Convert.ToInt32(Me.id_anticipo.Value)
                Dim anticipo = db.tme_anticipos.Find(idAnticipo)
                For Each file As UploadedFile In soporte_legalizacion.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & anticipo.codigo_anticipo & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    Path = Server.MapPath(db.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                    anticipo.soporte_legalizacion_eventos = nombreArchivo
                    db.Entry(anticipo).State = Entity.EntityState.Modified
                    db.SaveChanges()
                Next

                If enviarAprobacion Then
                    Dim codigoCategoria = ""
                    If (anticipo.id_tipo_anticipo = 2) Then
                        codigoCategoria = "ANT-LEG-EVENTOS"
                    Else
                        codigoCategoria = "ANT-LEG-COMPRAS"
                    End If
                    'Dim id_categoriaAPP = 2048
                    Dim cls_anticipo As APPROVAL.clss_anticipos = New APPROVAL.clss_anticipos(Convert.ToInt32(Me.Session("E_IDprograma")))
                    Dim tblUserApprovalAnticipos As DataTable = cls_anticipo.get_SolicitudLegalizacionAnticipoApprovalUser(id_usuario, codigoCategoria)

                    If tblUserApprovalAnticipos.Rows.Count() = 0 Then
                        Me.lblerr_user.Text = "El anticipo " & anticipo.codigo_anticipo & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de legalización de anticipos, contáctese con el administrador."
                        Me.lblerr_user.Visible = True
                        guardar = False
                        Me.idanticipo.Value = anticipo.id_anticipo
                    Else
                        Dim id_documento = guardarDocumento(anticipo, usuario)
                        guardar = True
                    End If
                End If
            End Using
        Catch ex As Exception
            guardar = False
        End Try


    End Function

    Public Function guardarDocumento(ByVal anticipo As tme_anticipos, ByVal usuario As t_usuarios) As Integer

        Dim codigoCategoria = ""
        If (anticipo.id_tipo_anticipo = 2) Then
            codigoCategoria = "ANT-LEG-EVENTOS"
        Else
            codigoCategoria = "ANT-LEG-COMPRAS"
        End If
        'Dim id_categoriaAPP = 2048
        Dim cls_anticipo As APPROVAL.clss_anticipos = New APPROVAL.clss_anticipos(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalAnticipos As DataTable = cls_anticipo.get_SolicitudLegalizacionAnticipoApprovalUser(usuario.id_usuario, codigoCategoria)
        Dim id_tipoDoc = tblUserApprovalAnticipos.Rows(0).Item("id_tipoDocumento")


        Dim descripcion = String.Format("Solicitud de anticipo {0} {1} - fecha {2}", usuario.nombre_usuario, usuario.apellidos_usuario, anticipo.fecha_anticipo)
        Dim err As Boolean = False
        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

        clss_approval.set_ta_documento(0) 'Set new Record
        clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", id_tipoDoc, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("numero_instrumento", anticipo.codigo_anticipo, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("descripcion_doc", descripcion, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("nom_beneficiario", usuario.nombre_usuario & " " & usuario.apellidos_usuario, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("comentarios", anticipo.motivo, "id_documento", 0) '.Replace("'", "''")
        clss_approval.set_ta_documentoFIELDS("codigo_AID", anticipo.codigo_anticipo, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", anticipo.codigo_anticipo, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("ficha_actividad", "NO", "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("monto_ficha", 0, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("regional", Me.Session("E_SubRegion").ToString.Trim, "id_documento", 0)
        clss_approval.set_ta_documentoFIELDS("codigo_Approval", anticipo.codigo_anticipo, "id_documento", 0)
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
        clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", tblUserApprovalAnticipos.Rows(0).Item("id_rol"), "id_app_documento", 0)
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

            guardarRelacionDocumento(id_documento, anticipo.id_anticipo)


            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1027, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_Anticipo(CType(id_appdocumento, Integer), anticipo.id_anticipo, True)) Then
            Else 'Error mandando Email
            End If
        End If

        Return id_documento

    End Function

    Sub guardarRelacionDocumento(ByVal idDocumento As Integer, ByVal idAnticipo As Integer)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim documentoAnticipo As New ta_documento_anticipo_legalizacion
                documentoAnticipo.id_anticipo = idAnticipo
                documentoAnticipo.id_documento = idDocumento
                dbEntities.ta_documento_anticipo_legalizacion.Add(documentoAnticipo)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
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
    Protected Sub delete_detalle(sender As Object, e As EventArgs)
        Me.tipo_delete.Value = 2
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub delete_concepto(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.id_concpeto.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipo_delete.Value = 1
        Me.RadWindow2.VisibleOnPageLoad = False
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub add_factura_Click(sender As Object, e As EventArgs) Handles add_factura.Click
        Me.RadWindow2.VisibleOnPageLoad = True
        Me.RadWindow2.Visible = True

        'LoadList()
        'Dim funcion = "FuncApprobals()"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)
    End Sub


End Class