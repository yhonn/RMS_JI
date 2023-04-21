Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Imports ly_APPROVAL
Public Class frm_anticiposAd
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_ANTICIPOS_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtRutas As New DataTable
    Dim dtCompras As New DataTable
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Session.Remove("dtRutas")
            Session.Remove("dtCompras")
            loadLista()
        End If
    End Sub
    Sub loadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim Sql As String = String.Format("SELECT habilitar_agregar_viaje FROM vw_t_usuarios " &
                                                 "   WHERE id_usuario={0} and id_programa ={1} ", idUser, id)
            ''"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("habilitar_agregar_viaje")
            dm.Fill(ds, "habilitar_agregar_viaje")
            Dim habilitar = ds.Tables("habilitar_agregar_viaje").Rows(0).Item(0)
            Me.habilitar_registro.Value = habilitar

            Dim fechaHabil2 = calculaDiaHabil(2, DateTime.Now)

            If habilitar = True Then
                Me.dt_fecha_anticipo.MinDate = DateTime.Now
            Else
                Me.dt_fecha_anticipo.MinDate = fechaHabil2
            End If

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

            Me.cmb_lugar_estipendio.DataSourceID = ""
            Me.cmb_lugar_estipendio.DataSource = dbEntities.tme_lugar_estipendio.ToList()
            Me.cmb_lugar_estipendio.DataTextField = "lugar"
            Me.cmb_lugar_estipendio.DataValueField = "id_lugar_estipendio"
            Me.cmb_lugar_estipendio.DataBind()

        End Using

    End Sub

    Private Sub cmb_sub_Region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_sub_Region.SelectedIndexChanged
        loadPar()
        loadCiudaddes()
    End Sub

    Private Sub cmb_tipo_anticipo_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_anticipo.SelectedIndexChanged
        loadPar()
    End Sub
    Sub loadPar()
        Using dbEntities As New dbRMS_JIEntities
            'Me.anticipo_fondos.Visible = False
            'Me.info_par.Visible = False
            Me.cmb_par.ClearSelection()
            Me.cmb_par.DataSourceID = ""
            Me.info_rutas.Visible = False
            Me.info_compras.Visible = False
            If Me.cmb_tipo_anticipo.SelectedValue = "2" And Me.cmb_sub_Region.SelectedValue <> "" Then
                'Me.info_par.Visible = True
                'Me.anticipo_fondos.Visible = True
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 2).OrderByDescending(Function(p) p.id_par).ToList()
                Me.info_rutas.Visible = True

                loadCiudaddes()
            ElseIf Me.cmb_tipo_anticipo.SelectedValue = "1" And Me.cmb_sub_Region.SelectedValue <> "" Then
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 1).OrderByDescending(Function(p) p.id_par).ToList()
                Me.info_compras.Visible = True
            End If

            Me.cmb_par.DataTextField = "codigo_par"
            Me.cmb_par.DataValueField = "id_par"
            Me.cmb_par.DataBind()
        End Using
    End Sub
    Private Sub cmb_par_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_par.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim idPar = Convert.ToInt32(Me.cmb_par.SelectedValue)
            Dim par = dbEntities.tme_pares.Find(idPar)
            Me.txt_motivo_anticipo.Text = par.proposito
        End Using
    End Sub

    Private Sub cmb_medio_pago_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_medio_pago.SelectedIndexChanged
        Me.otro_medio_pago.Visible = False
        Me.costo_giro.Visible = False
        If Me.cmb_medio_pago.SelectedValue = "2" Then
            Me.otro_medio_pago.Visible = True
            Me.costo_giro.Visible = True
        End If
    End Sub

    Private Sub rbn_estipendio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_estipendio.SelectedIndexChanged
        Me.lugarEstipendio.Visible = False
        Me.infoEstipendio.Visible = False
        If Me.rbn_estipendio.SelectedValue = 1 Then
            Me.lugarEstipendio.Visible = True
            'Me.infoEstipendio.Visible = True

            If Me.cmb_lugar_estipendio.SelectedValue = "1" Then
                Me.infoEstipendio.Visible = True
            End If
        End If
    End Sub
    Sub loadCiudaddes()
        Using dbEntities As New dbRMS_JIEntities
            Me.info_rutas.Visible = False
            If Me.cmb_sub_Region.SelectedValue <> "" And Me.cmb_tipo_anticipo.SelectedValue <> "" Then
                If Convert.ToInt32(Me.cmb_tipo_anticipo.SelectedValue) = 2 Then
                    Me.info_rutas.Visible = True
                    Dim idSubregion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)

                    Dim ciudades = dbEntities.tme_ciudades_rutas.Where(Function(p) p.id_subregion = idSubregion).ToList()

                    Me.cmb_municipio_salida.DataSourceID = ""
                    Me.cmb_municipio_salida.DataSource = ciudades
                    Me.cmb_municipio_salida.DataTextField = "ciudad"
                    Me.cmb_municipio_salida.DataValueField = "id_ciudad"
                    Me.cmb_municipio_salida.DataBind()

                    Me.cmb_municipio_llegada.DataSourceID = ""
                    Me.cmb_municipio_llegada.DataSource = ciudades
                    Me.cmb_municipio_llegada.DataTextField = "ciudad"
                    Me.cmb_municipio_llegada.DataValueField = "id_ciudad"
                    Me.cmb_municipio_llegada.DataBind()
                End If

            End If
        End Using
    End Sub

    Private Sub cmb_municipio_salida_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_municipio_salida.SelectedIndexChanged
        loadRutasList()
        Using dbEntities As New dbRMS_JIEntities
            If Me.cmb_municipio_salida.SelectedValue <> "" Then
                Dim idCiudad = Convert.ToInt32(Me.cmb_municipio_salida.SelectedValue)
                Dim idSubregion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Dim idsCiudadDestino = dbEntities.tme_rutas.Where(Function(p) p.id_ciudad_origen = idCiudad).Select(Function(p) p.id_ciudad_destino).ToList()
                Dim ciudadDestino = dbEntities.tme_ciudades_rutas.Where(Function(p) p.id_subregion = idSubregion And idsCiudadDestino.Contains(p.id_ciudad)).ToList()

                Me.cmb_municipio_llegada.ClearSelection()
                Me.cmb_municipio_llegada.DataSourceID = ""
                Me.cmb_municipio_llegada.DataSource = ciudadDestino
                Me.cmb_municipio_llegada.DataTextField = "ciudad"
                Me.cmb_municipio_llegada.DataValueField = "id_ciudad"
                Me.cmb_municipio_llegada.DataBind()
            End If
        End Using
    End Sub
    Private Sub cmb_municipio_llegada_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_municipio_llegada.SelectedIndexChanged
        loadRutasList()
    End Sub
    Sub loadRutasList()
        Using dbEntities As New dbRMS_JIEntities
            If Me.cmb_municipio_llegada.SelectedValue <> "" And Me.cmb_municipio_salida.SelectedValue <> "" Then

                Dim idCiudadSalida = Convert.ToInt32(Me.cmb_municipio_salida.SelectedValue)
                Dim idCiudadLlegada = Convert.ToInt32(Me.cmb_municipio_llegada.SelectedValue)
                Me.cmb_zona_rural.DataSourceID = ""
                Me.cmb_zona_rural.DataSource = dbEntities.tme_rutas.Where(Function(p) p.id_ciudad_origen = idCiudadSalida And p.id_ciudad_destino = idCiudadLlegada).ToList()
                Me.cmb_zona_rural.DataTextField = "zona_rural"
                Me.cmb_zona_rural.DataValueField = "id_ruta"
                Me.cmb_zona_rural.DataBind()
            End If

        End Using
    End Sub

    Private Sub cmb_zona_rural_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_zona_rural.SelectedIndexChanged
        loadTotal()
    End Sub

    Sub loadTotal()
        Using dbEntities As New dbRMS_JIEntities
            If Me.cmb_zona_rural.SelectedValue <> "" Then
                Dim idZonarural = Convert.ToInt32(Me.cmb_zona_rural.SelectedValue)
                Dim zonaRural = dbEntities.tme_rutas.Find(idZonarural)
                Me.txt_valor_trayecto.Value = zonaRural.valor_ruta
                Me.txt_valor_total.Value = Me.txt_cantidad_personas.Value * zonaRural.valor_ruta
                Me.tiempo_estimado.Value = zonaRural.tiempo_estimado
                Me.observaciones.Value = zonaRural.observaciones
            End If
        End Using
    End Sub

    Private Sub txt_cantidad_personas_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_personas.TextChanged
        loadTotal()
        validarValores()
    End Sub

    Private Sub btn_agregar_ruta_Click(sender As Object, e As EventArgs) Handles btn_agregar_ruta.Click
        Try
            If Session("dtRutas") IsNot Nothing Then
                dtRutas = Session("dtRutas")
            Else
                createdtcolumsRutas()
            End If

            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = Guid.NewGuid().ToString()

            Dim estipendio As Boolean = False
            Dim estipendioDesayuno As Boolean = False
            Dim estipendioAlmuerzo As Boolean = False
            Dim estipendioCena As Boolean = False

            If Me.rbn_estipendio.SelectedValue IsNot Nothing Then
                estipendio = If(Me.rbn_estipendio.SelectedValue = "1", True, False)
            End If

            If Me.rbn_estipendio_desayuno.SelectedValue IsNot Nothing Then
                estipendioDesayuno = If(Me.rbn_estipendio_desayuno.SelectedValue = "1", True, False)
            End If

            If Me.rbn_estipendio_almuerzo.SelectedValue IsNot Nothing Then
                estipendioAlmuerzo = If(Me.rbn_estipendio_almuerzo.SelectedValue = "1", True, False)
            End If

            If Me.rbn_estipendio_cena.SelectedValue IsNot Nothing Then
                estipendioCena = If(Me.rbn_estipendio_cena.SelectedValue = "1", True, False)
            End If
            Dim subTotalEs = (Me.txt_total_desayuno.Value + Me.txt_total_almuerzo.Value + Me.txt_total_cena.Value) * Me.txt_cantidad_personas.Value

            dtRutas.Rows.Add(idunique, Me.cmb_zona_rural.SelectedValue, Me.cmb_municipio_salida.Text, Me.cmb_municipio_llegada.Text, Me.cmb_zona_rural.Text, Me.tiempo_estimado.Value,
                             Me.txt_cantidad_personas.Value, Me.txt_valor_trayecto.Value, Me.txt_valor_total.Value, Me.txt_observaciones_ruta.Text, estipendio, estipendioDesayuno,
                             estipendioAlmuerzo, estipendioCena, Me.txt_cantidad_desayuno.Value, Me.txt_cantidad_almuerzo.Value, Me.txt_cantidad_cena.Value,
                             If(Me.cmb_lugar_estipendio.SelectedValue = "", Nothing, Me.cmb_lugar_estipendio.SelectedValue),
                             Me.cmb_lugar_estipendio.Text, Me.txt_total_desayuno.Value, Me.txt_total_almuerzo.Value, Me.txt_total_cena.Value,
                             (Me.txt_total_desayuno.Value + Me.txt_total_almuerzo.Value + Me.txt_total_cena.Value),
                             (Me.txt_total_desayuno.Value + Me.txt_total_almuerzo.Value + Me.txt_total_cena.Value) * Me.txt_cantidad_personas.Value,
                             ((Me.txt_total_desayuno.Value + Me.txt_total_almuerzo.Value + Me.txt_total_cena.Value) * Me.txt_cantidad_personas.Value) + Me.txt_valor_total.Value,
                             If(estipendio = True, "SI", "NO"), If(estipendioDesayuno = True, "SI", "NO"), If(estipendioAlmuerzo = True, "SI", "NO"), If(estipendioCena = True, "SI", "NO"),
                             Me.observaciones.Value, False, 0)
            Session("dtRutas") = dtRutas


            Me.rbn_estipendio.SelectedValue = 0
            Me.txt_cantidad_desayuno.Value = 0
            Me.txt_cantidad_almuerzo.Value = 0
            Me.txt_cantidad_cena.Value = 0
            Me.txt_total_desayuno.Value = 0
            Me.txt_total_almuerzo.Value = 0
            Me.txt_total_cena.Value = 0

            Me.rbn_estipendio_desayuno.SelectedValue = 0
            Me.rbn_estipendio_almuerzo.SelectedValue = 0
            Me.rbn_estipendio_cena.SelectedValue = 0
            Me.txt_observaciones_ruta.Text = ""

            Me.lugarEstipendio.Visible = False
            Me.infoEstipendio.Visible = False
            Me.info_almuerzo.Visible = False
            Me.info_desayuno.Visible = False
            Me.info_cena.Visible = False

            fillGridRutas()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try
    End Sub

    Protected Sub btn_agregar_concepto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_concepto.Click
        Try
            If Session("dtCompras") IsNot Nothing Then
                dtCompras = Session("dtCompras")
            Else
                createdtcolumsCompras()
            End If



            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim valor_unitario = Me.txt_valor.Value
            Dim cantidad = Me.txt_cantidad.Value
            Dim valor_total = valor_unitario * cantidad

            Dim idunique = Guid.NewGuid().ToString()
            dtCompras.Rows.Add(idunique, Me.txt_cantidad.Value, Me.txt_descripcion_concepto.Text, Me.txt_valor.Value, valor_total, False, 0)
            Session("dtCompras") = dtCompras
            Me.txt_cantidad.Text = String.Empty
            Me.txt_valor.Text = String.Empty
            Me.txt_descripcion_concepto.Text = String.Empty
            'Me.txt_unidad_medida.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtCompras.Rows().Count()
            fillGridCompras()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
    Sub fillGridRutas()
        If Session("dtRutas") IsNot Nothing Then
            dtRutas = Session("dtRutas")
        Else
            createdtcolumsRutas()
        End If
        Me.grd_rutas.DataSource = dtRutas
        Me.grd_rutas.DataBind()
    End Sub
    Sub fillGridCompras()
        If Session("dtCompras") IsNot Nothing Then
            dtCompras = Session("dtCompras")
        Else
            createdtcolumsCompras()
        End If
        Me.grd_compras.DataSource = dtCompras
        Me.grd_compras.DataBind()
    End Sub
    Protected Sub grd_rutas_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_rutas.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta_tem").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta_tem").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta_tem").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

            Dim lbl_tiempo_estimado As Label = CType(e.Item.Cells(0).FindControl("lbl_tiempo_estimado"), Label)
            lbl_tiempo_estimado.Text = DataBinder.Eval(e.Item.DataItem, "tiempo_estimado")
            Dim lbl_observaciones_trayecto As Label = CType(e.Item.Cells(0).FindControl("lbl_observaciones_trayecto"), Label)
            lbl_observaciones_trayecto.Text = DataBinder.Eval(e.Item.DataItem, "observaciones_trayecto")
            Dim lbl_observaciones_ruta As Label = CType(e.Item.Cells(0).FindControl("lbl_observaciones_ruta"), Label)
            lbl_observaciones_ruta.Text = DataBinder.Eval(e.Item.DataItem, "observaciones_ruta")
        End If
    End Sub
    Protected Sub grd_compras_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_compras.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_anticipo_compra_tem").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_anticipo_compra_tem").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_anticipo_compra_tem").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub
    Protected Sub delete_detalle(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.tipo.Value = 1
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub delete_compra(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        Me.tipo.Value = 2
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub
    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Dim tipo_elemento_eliminar = Convert.ToInt32(Me.tipo.Value)
        If tipo_elemento_eliminar = 1 Then
            If Session("dtRutas") IsNot Nothing Then
                dtRutas = Session("dtRutas")
            Else
                createdtcolumsRutas()
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            If Session("dtCompras") IsNot Nothing Then
                dtCompras = Session("dtCompras")
            Else
                createdtcolumsCompras()
            End If
        End If


        Dim identity_ = Convert.ToString(Me.identity.Value)

        If tipo_elemento_eliminar = 1 Then
            Dim foundRow As DataRow() = dtRutas.Select("id_anticipo_ruta_tem = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then
                dtRutas.Rows.Remove(foundRow(0))
                Session("dtRutas") = dtRutas
                fillGridRutas()
            End If
        ElseIf tipo_elemento_eliminar = 2 Then
            Dim foundRow As DataRow() = dtCompras.Select("id_anticipo_compra_tem = '" & identity_.ToString() & "'")

            If foundRow.Count > 0 Then
                dtCompras.Rows.Remove(foundRow(0))
                Session("dtCompras") = dtCompras
                fillGridCompras()
            End If
        End If
        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Sub createdtcolumsCompras()
        If dtCompras.Columns.Count = 0 Then
            dtCompras.Columns.Add("id_anticipo_compra_tem", GetType(String))
            dtCompras.Columns.Add("cantidad", GetType(Double))
            dtCompras.Columns.Add("descripcion", GetType(String))
            dtCompras.Columns.Add("valor_unitario", GetType(Double))
            dtCompras.Columns.Add("valor", GetType(Double))
            dtCompras.Columns.Add("esta_bd", GetType(Boolean))
            dtCompras.Columns.Add("id_anticipo_compra", GetType(Integer))
        End If
    End Sub
    Sub createdtcolumsRutas()
        If dtRutas.Columns.Count = 0 Then
            dtRutas.Columns.Add("id_anticipo_ruta_tem", GetType(String))
            dtRutas.Columns.Add("id_ruta", GetType(Integer))
            dtRutas.Columns.Add("ciudad_salida", GetType(String))
            dtRutas.Columns.Add("ciudad_llegada", GetType(String))
            dtRutas.Columns.Add("zona_rural", GetType(String))
            dtRutas.Columns.Add("tiempo_estimado", GetType(String))
            dtRutas.Columns.Add("cantidad_personas", GetType(Integer))
            dtRutas.Columns.Add("valor_trayecto", GetType(Decimal))
            dtRutas.Columns.Add("sub_total_ruta", GetType(Decimal))
            dtRutas.Columns.Add("observaciones_ruta", GetType(String))
            dtRutas.Columns.Add("requiere_estipendio", GetType(Boolean))
            dtRutas.Columns.Add("estipendio_desayuno", GetType(Boolean))
            dtRutas.Columns.Add("estipendio_almuerzo", GetType(Boolean))
            dtRutas.Columns.Add("estipendio_cena", GetType(Boolean))
            dtRutas.Columns.Add("cantidad_desayuno", GetType(Integer))
            dtRutas.Columns.Add("cantidad_almuerzo", GetType(Integer))
            dtRutas.Columns.Add("cantidad_cena", GetType(Integer))
            dtRutas.Columns.Add("id_lugar_estipendio", GetType(Integer))
            dtRutas.Columns.Add("lugar_estipendio", GetType(String))
            dtRutas.Columns.Add("valor_desayuno", GetType(Decimal))
            dtRutas.Columns.Add("valor_almuerzo", GetType(Decimal))
            dtRutas.Columns.Add("valor_cena", GetType(Decimal))
            dtRutas.Columns.Add("sub_total_estipendio", GetType(Decimal))
            dtRutas.Columns.Add("valor_total_estipendio", GetType(Decimal))
            dtRutas.Columns.Add("valor_total_ruta", GetType(Decimal))
            dtRutas.Columns.Add("requiere_estipendio_text", GetType(String))
            dtRutas.Columns.Add("estipendio_desayuno_text", GetType(String))
            dtRutas.Columns.Add("estipendio_almuerzo_text", GetType(String))
            dtRutas.Columns.Add("estipendio_cena_text", GetType(String))
            dtRutas.Columns.Add("observaciones_trayecto", GetType(String))
            dtRutas.Columns.Add("esta_bd", GetType(Boolean))
            dtRutas.Columns.Add("id_anticipo_ruta", GetType(Integer))
        End If
    End Sub

    Private Sub rbn_estipendio_almuerzo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_estipendio_almuerzo.SelectedIndexChanged
        Me.info_almuerzo.Visible = False
        If Me.rbn_estipendio_almuerzo.SelectedValue = "1" Then
            Me.info_almuerzo.Visible = True
        End If
    End Sub

    Private Sub rbn_estipendio_cena_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_estipendio_cena.SelectedIndexChanged
        Me.info_cena.Visible = False
        If Me.rbn_estipendio_cena.SelectedValue = "1" Then
            Me.info_cena.Visible = True
        End If
    End Sub

    Private Sub rbn_estipendio_desayuno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_estipendio_desayuno.SelectedIndexChanged
        Me.info_desayuno.Visible = False
        If Me.rbn_estipendio_desayuno.SelectedValue = "1" Then
            Me.info_desayuno.Visible = True
        End If
    End Sub

    Private Sub cmb_lugar_estipendio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_lugar_estipendio.SelectedIndexChanged
        If Me.cmb_lugar_estipendio.SelectedValue <> "" Then
            Using db As New dbRMS_JIEntities
                Dim idLugar = Convert.ToInt32(Me.cmb_lugar_estipendio.SelectedValue)
                Dim estipendio = db.tme_lugar_estipendio.Find(idLugar)
                Me.valor_desayuno.Value = estipendio.desayuno
                Me.valor_almuerzo.Value = estipendio.almuerzo
                Me.valor_cena.Value = estipendio.cena
                If Me.rbn_estipendio.SelectedValue = "1" Then
                    Me.infoEstipendio.Visible = True
                End If
                validarValores()
            End Using
        End If
    End Sub

    Private Sub txt_cantidad_cena_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_cena.TextChanged
        validarValores()
    End Sub

    Private Sub txt_cantidad_desayuno_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_desayuno.TextChanged
        validarValores()
    End Sub

    Private Sub txt_cantidad_almuerzo_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_almuerzo.TextChanged
        validarValores()
    End Sub

    Sub validarValores()
#Region "totalDesayuno"
        If Me.rbn_estipendio_desayuno.SelectedValue = "1" And Me.txt_cantidad_desayuno.Value > 0 Then
            Me.txt_total_desayuno.Value = Me.txt_cantidad_desayuno.Value * Me.valor_desayuno.Value
        Else
            Me.txt_total_desayuno.Value = 0
        End If
#End Region
#Region "totalAlmuerzo"
        If Me.rbn_estipendio_almuerzo.SelectedValue = "1" And Me.txt_cantidad_almuerzo.Value > 0 Then
            Me.txt_total_almuerzo.Value = Me.txt_cantidad_almuerzo.Value * Me.valor_almuerzo.Value
        Else
            Me.txt_total_almuerzo.Value = 0
        End If
#End Region
#Region "totalCena"
        If Me.rbn_estipendio_cena.SelectedValue = "1" And Me.txt_cantidad_cena.Value > 0 Then
            Me.txt_total_cena.Value = Me.txt_cantidad_cena.Value * Me.valor_cena.Value
        Else
            Me.txt_total_cena.Value = 0
        End If
#End Region
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            If validarFecha() Then
                Me.alerta_dias.Visible = True
            Else
                guardarAnticipo(False)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function guardarAnticipo(ByVal enviarAprobacion As Boolean) As Boolean
        If Session("dtRutas") IsNot Nothing Then
            dtRutas = Session("dtRutas")
        Else
            createdtcolumsRutas()
        End If
        If Session("dtCompras") IsNot Nothing Then
            dtCompras = Session("dtCompras")
        Else
            createdtcolumsCompras()
        End If
        Try
            Using db As New dbRMS_JIEntities

                Dim id_anticipo = Convert.ToInt32(Me.idanticipo.Value)
                Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                Dim usuario = db.t_usuarios.Find(id_usuario)
                Dim anticipo As New tme_anticipos
                If id_anticipo = 0 Then
                    Dim consecutivoAnticipos = db.tme_anticipos.Count()

                    anticipo.fecha_crea = DateTime.Now
                    anticipo.id_usuario_crea = Me.Session("E_IdUser").ToString()
                    anticipo.id_subregion = Me.cmb_sub_Region.SelectedValue
                    anticipo.fecha_anticipo = Me.dt_fecha_anticipo.SelectedDate
                    anticipo.id_medio_pago = Me.cmb_medio_pago.SelectedValue
                    anticipo.detalle_nro_cuenta_anticipo = Me.txt_detalle_medio_pago.Text
                    anticipo.otro_medio_pago = Me.txt_otro_medio_pago.Text
                    anticipo.costo_total_giro = Me.txt_costo_giro.Value
                    anticipo.id_tipo_anticipo = Me.cmb_tipo_anticipo.SelectedValue
                    anticipo.id_par = Me.cmb_par.SelectedValue
                    anticipo.motivo = Me.txt_motivo_anticipo.Text
                    anticipo.codigo_anticipo = "ANT-JI-" & consecutivoAnticipos + 1
                    db.tme_anticipos.Add(anticipo)
                    db.SaveChanges()

                    If anticipo.id_tipo_anticipo = 1 Then

                        Dim cantidad As Decimal
                        Dim descripcion As String
                        Dim unidadMedida As String
                        Dim valorUnitario As Decimal
                        Dim valorTotal As Decimal
                        Dim id_uMedida As Decimal


                        For Each row As DataRow In dtCompras.Rows
                            Dim oAnticipoCompras As New tme_anticipo_compra
                            cantidad = row("cantidad")
                            descripcion = row("descripcion")
                            valorUnitario = row("valor_unitario")
                            valorTotal = row("valor")

                            oAnticipoCompras.id_anticipo = anticipo.id_anticipo
                            oAnticipoCompras.cantidad = cantidad
                            oAnticipoCompras.precio_unitario = valorUnitario
                            oAnticipoCompras.descripcion = descripcion
                            db.tme_anticipo_compra.Add(oAnticipoCompras)
                            db.SaveChanges()
                        Next

                    ElseIf anticipo.id_tipo_anticipo = 2 Then


                        Dim idRuta As Integer
                        Dim cantidad_personas As Integer
                        Dim valor_trayecto As Decimal
                        Dim observaciones_ruta As String
                        Dim requiere_estipendio As Boolean
                        Dim id_lugar_estipendio As Integer = Nothing
                        Dim estipendio_desayuno As Boolean
                        Dim estipendio_almuerzo As Boolean
                        Dim estipendio_cena As Boolean
                        Dim cantidad_desayuno As Integer
                        Dim cantidad_almuerzo As Integer
                        Dim cantidad_cena As Integer
                        Dim valor_desayuno As Decimal
                        Dim valor_almuerzo As Decimal
                        Dim valor_cena As Decimal

                        For Each row As DataRow In dtRutas.Rows
                            Dim oAnticipoCompras As New tme_anticipo_compra
                            idRuta = row("id_ruta")
                            cantidad_personas = row("cantidad_personas")
                            valor_trayecto = row("valor_trayecto")
                            observaciones_ruta = row("observaciones_ruta")
                            requiere_estipendio = row("requiere_estipendio")
                            If row("id_lugar_estipendio") IsNot DBNull.Value Then
                                id_lugar_estipendio = row("id_lugar_estipendio")
                            End If
                            estipendio_desayuno = row("estipendio_desayuno")
                            estipendio_almuerzo = row("estipendio_almuerzo")
                            estipendio_cena = row("estipendio_cena")
                            cantidad_desayuno = row("cantidad_desayuno")
                            cantidad_almuerzo = row("cantidad_almuerzo")
                            cantidad_cena = row("cantidad_cena")
                            valor_desayuno = row("valor_desayuno")
                            valor_almuerzo = row("valor_almuerzo")
                            valor_cena = row("valor_cena")

                            Dim oAnticipoRuta As New tme_anticipo_ruta
                            oAnticipoRuta.id_anticipo = anticipo.id_anticipo
                            oAnticipoRuta.id_ruta = idRuta
                            oAnticipoRuta.cantidad_personas = cantidad_personas
                            oAnticipoRuta.valor_trayecto = valor_trayecto
                            oAnticipoRuta.observaciones = observaciones_ruta
                            oAnticipoRuta.requiere_estipendio = requiere_estipendio
                            If id_lugar_estipendio > 0 Then
                                oAnticipoRuta.id_lugar_estipendio = id_lugar_estipendio
                            End If
                            oAnticipoRuta.estipendio_desayuno = estipendio_desayuno
                            oAnticipoRuta.estipendio_almuerzo = estipendio_almuerzo
                            oAnticipoRuta.estipendio_cena = estipendio_cena
                            oAnticipoRuta.cantidad_desayuno = cantidad_desayuno
                            oAnticipoRuta.cantidad_almuerzo = cantidad_almuerzo
                            oAnticipoRuta.cantidad_cena = cantidad_cena
                            oAnticipoRuta.valor_unitario_deayuno = valor_desayuno
                            oAnticipoRuta.valor_unitario_almuerzo = valor_almuerzo
                            oAnticipoRuta.valor_unitario_cena = valor_cena

                            db.tme_anticipo_ruta.Add(oAnticipoRuta)
                            db.SaveChanges()
                        Next
                    End If

                    If enviarAprobacion = True Then
                        Dim id_categoriaAPP = 2048
                        Dim cls_anticipo As APPROVAL.clss_anticipos = New APPROVAL.clss_anticipos(Convert.ToInt32(Me.Session("E_IDprograma")))
                        Dim tblUserApprovalAnticipo As DataTable = cls_anticipo.get_SolicitudAnticipoApprovalUser(anticipo.id_usuario_crea, id_categoriaAPP)

                        If tblUserApprovalAnticipo.Rows.Count() = 0 Then
                            Me.lblerr_user.Text = "El anticipo " & anticipo.codigo_anticipo & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de anticipos, contáctese con el administrador."
                            Me.lblerr_user.Visible = True
                            guardarAnticipo = False
                            Me.idanticipo.Value = anticipo.id_anticipo
                        Else
                            Dim id_documento = guardarDocumento(anticipo, usuario)
                            guardarAnticipo = True
                        End If



                    End If
                Else

                    anticipo = db.tme_anticipos.Where(Function(p) p.id_anticipo = id_anticipo).ToList().FirstOrDefault()
                    Dim id_categoriaAPP = 2048
                    Dim cls_anticipo As APPROVAL.clss_anticipos = New APPROVAL.clss_anticipos(Convert.ToInt32(Me.Session("E_IDprograma")))
                    Dim tblUserApprovalAnticipo As DataTable = cls_anticipo.get_SolicitudAnticipoApprovalUser(anticipo.id_usuario_crea, id_categoriaAPP)

                    If tblUserApprovalAnticipo.Rows.Count() = 0 Then
                        Me.lblerr_user.Text = "El anticipo " & anticipo.codigo_anticipo & "  fue guardado correctamente, sin embargo no se puede iniciar la aprobación debido a que no está vinculado a ninguna ruta de aprobación de solicitud de anticipos, contáctese con el administrador."
                        Me.lblerr_user.Visible = True
                        guardarAnticipo = False
                        Me.idanticipo.Value = anticipo.id_anticipo
                    Else
                        Dim id_documento = guardarDocumento(anticipo, usuario)
                        guardarAnticipo = True
                    End If

                End If

            End Using
        Catch ex As Exception

        End Try

    End Function

    Public Function guardarDocumento(ByVal anticipo As tme_anticipos, ByVal usuario As t_usuarios) As Integer
        Dim id_categoriaAPP = 2048
        Dim cls_anticipo As APPROVAL.clss_anticipos = New APPROVAL.clss_anticipos(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tblUserApprovalTimeSheet As DataTable = cls_anticipo.get_SolicitudAnticipoApprovalUser(anticipo.id_usuario_crea, id_categoriaAPP)
        Dim id_tipoDoc = tblUserApprovalTimeSheet.Rows(0).Item("id_tipoDocumento")


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

            guardarRelacionDocumento(id_documento, anticipo.id_anticipo)


            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1027, cl_user.regionalizacionCulture, id_appdocumento)
            If (objEmail.Emailing_APPROVAL_Anticipo(CType(id_appdocumento, Integer), anticipo.id_anticipo)) Then
            Else 'Error mandando Email
            End If
        End If

        Return id_documento

    End Function
    Sub guardarRelacionDocumento(ByVal idDocumento As Integer, ByVal idAnticipo As Integer)
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim documentoAnticipo As New ta_documento_anticipos
                documentoAnticipo.id_anticipo = idAnticipo
                documentoAnticipo.id_documento = idDocumento
                dbEntities.ta_documento_anticipos.Add(documentoAnticipo)
                dbEntities.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Public Function validarFecha() As Boolean

        Dim fechaHabil2 = calculaDiaHabil(2, DateTime.Now)
        Dim fechaHabil = New Date(fechaHabil2.Year, fechaHabil2.Month, fechaHabil2.Day, 0, 0, 0)
        If fechaHabil <= Me.dt_fecha_anticipo.SelectedDate Then
            validarFecha = False
        Else
            validarFecha = True
        End If
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

    Private Sub btn_guardar_enviar_Click(sender As Object, e As EventArgs) Handles btn_guardar_enviar.Click
        Try
            If validarFecha() Then
                Me.alerta_dias.Visible = True
            Else
                guardarAnticipo(True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class