Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Imports ly_APPROVAL
Public Class frm_anticiposVerificacion
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_ANTICIPOS_VAL"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_rutas)
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
            loadLista()
            loadData(idAnticipo)
            loadRutas(idAnticipo)
            loadParticipantes(idAnticipo)
            loadCiudaddes()
        End If
    End Sub
    Sub loadRutas(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim rutas = dbEntities.vw_anticipo_detalle_ruta.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_rutas.DataSource = rutas
            Me.grd_rutas.DataBind()
        End Using
    End Sub

    Sub loadParticipantes(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim participantes = (From a In dbEntities.tme_anticipo_ruta_participantes
                                 Join b In dbEntities.vw_anticipo_detalle_ruta On a.id_ruta_anticipo Equals b.id_anticipo_ruta
                                 Where a.tme_anticipo_ruta.id_anticipo = idAnticipo
                                 Select a.numero_documento, a.primer_apellido, a.segundo_apellido, a.valor, a.nombres,
                                     a.tipo_ocumento, a.telefono, b.num_ruta, a.id_participante, a.id_ruta_anticipo, a.valor_estipendio, a.valor_trayecto).ToList()
            Me.totalParticipantes.Value = participantes.Count()

            If participantes.Count() > 0 Then
                Me.btn_guardar.Enabled = True
            End If
            Me.grd_participantes_resumen.DataSource = participantes
            Me.grd_participantes_resumen.DataBind()
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

            Dim col_hlk_editar As LinkButton = New LinkButton
            col_hlk_editar = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
            col_hlk_editar.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta").ToString())
            col_hlk_editar.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_anticipo_ruta").ToString())
            col_hlk_editar.ToolTip = controles.iconosGrid("col_hlk_editar")
        End If
    End Sub
    Sub loadData(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim anticipo = dbEntities.tme_anticipos.Find(idAnticipo)
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
            Me.cmb_tipo_documento.DataSource = dbEntities.tme_tipo_documento.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_tipo_documento.DataTextField = "tipo_documento"
            Me.cmb_tipo_documento.DataValueField = "tipo_documento"
            Me.cmb_tipo_documento.DataBind()

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

    Protected Sub participantes(sender As Object, e As EventArgs)

        Using dbEntities As New dbRMS_JIEntities
            Dim a = CType(sender, LinkButton)
            Dim idRutaAnticipo = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Me.id_ruta_anticipo.Value = idRutaAnticipo

            Dim ruta = dbEntities.tme_anticipo_ruta.Find(idRutaAnticipo)

            Me.grd_conceptos.DataSource = dbEntities.tme_anticipo_ruta_participantes.Where(Function(p) p.id_ruta_anticipo = idRutaAnticipo).ToList()
            Me.grd_conceptos.DataBind()


            Me.txt_valor.Value = ruta.total_ruta / ruta.cantidad_personas
            Me.txt_estipendio.Value = ruta.total_estipendio / ruta.cantidad_personas
            Me.txt_trayecto.Value = ruta.total_trayecto / ruta.cantidad_personas
            Me.RadWindow2.VisibleOnPageLoad = True
            Me.RadWindow2.Visible = True
        End Using

    End Sub

    Private Sub btn_guardar_participante_Click(sender As Object, e As EventArgs) Handles btn_guardar_participante.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idRutaAnticipo = Convert.ToInt32(Me.id_ruta_anticipo.Value)
            Dim participante = New tme_anticipo_ruta_participantes
            participante.id_ruta_anticipo = idRutaAnticipo
            participante.numero_documento = Me.txt_numero_documento.Value
            participante.nombres = stripAccent(Me.txt_nombre.Text.ToUpper)
            participante.primer_apellido = stripAccent(Me.txt_primer_apellido.Text.ToUpper)
            participante.segundo_apellido = stripAccent(Me.txt_segundo_apellido.Text.ToUpper)
            participante.valor = Me.txt_valor.Value
            participante.telefono = Me.txt_numero_telefono.Text
            participante.tipo_ocumento = stripAccent(Me.cmb_tipo_documento.Text.ToUpper)
            participante.valor_trayecto = Me.txt_trayecto.Value
            participante.valor_estipendio = Me.txt_estipendio.Value
            dbEntities.tme_anticipo_ruta_participantes.Add(participante)
            dbEntities.SaveChanges()

            Me.grd_conceptos.DataSource = dbEntities.tme_anticipo_ruta_participantes.Where(Function(p) p.id_ruta_anticipo = idRutaAnticipo).ToList()
            Me.grd_conceptos.DataBind()

            Dim idanticipo = Convert.ToInt32(Me.id_anticipo.Value)


            Me.txt_nombre.Text = String.Empty
            Me.txt_primer_apellido.Text = String.Empty
            Me.txt_segundo_apellido.Text = String.Empty
            Me.txt_numero_telefono.Text = String.Empty
            Me.cmb_tipo_documento.Text = String.Empty
            Me.txt_numero_documento.Text = String.Empty

            loadRutas(idanticipo)
            loadParticipantes(idanticipo)
        End Using
    End Sub

    Private Sub btn_guardar_finalizar_Click(sender As Object, e As EventArgs) Handles btn_guardar_finalizar.Click
        Me.RadWindow2.VisibleOnPageLoad = False
    End Sub

    Protected Sub btn_add_ruta_Click(sender As Object, e As EventArgs) Handles btn_add_ruta.Click
        Me.RadWindowRutas.VisibleOnPageLoad = True
        Me.RadWindowRutas.Visible = True
    End Sub

    Function stripAccent(Text As String) As String

        Const AccChars = "ŠŽšžŸÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïðñòóôõöùúûüýÿñÑ"
        Const RegChars = "SZszYAAAAAACEEEEIIIIDNOOOOOUUUUYaaaaaaceeeeiiiidnooooouuuuyynN"

        Dim A As String
        Dim B As String
        Dim i As Integer

        For i = 1 To Len(AccChars)
            A = Mid(AccChars, i, 1)
            B = Mid(RegChars, i, 1)
            Text = Replace(Text, A, B)
        Next

        stripAccent = Text

    End Function


    Sub loadCiudaddes()
        Using dbEntities As New dbRMS_JIEntities
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
    Private Sub txt_cantidad_cena_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_cena.TextChanged
        validarValores()
    End Sub

    Private Sub txt_cantidad_desayuno_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_desayuno.TextChanged
        validarValores()
    End Sub

    Private Sub txt_cantidad_almuerzo_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_almuerzo.TextChanged
        validarValores()
    End Sub
    Private Sub txt_cantidad_personas_TextChanged(sender As Object, e As EventArgs) Handles txt_cantidad_personas.TextChanged
        loadTotal()
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


    Private Sub btn_agregar_ruta_Click(sender As Object, e As EventArgs) Handles btn_agregar_ruta.Click
        Using db As New dbRMS_JIEntities
            Dim oAnticipoRuta = New tme_anticipo_ruta
            Dim idAnticipo = Convert.ToInt32(Me.Request.QueryString("id"))
            oAnticipoRuta.id_anticipo = idAnticipo
            oAnticipoRuta.id_ruta = Me.cmb_zona_rural.SelectedValue
            oAnticipoRuta.cantidad_personas = Me.txt_cantidad_personas.Value
            oAnticipoRuta.valor_trayecto = Me.txt_valor_trayecto.Value
            oAnticipoRuta.observaciones = Me.txt_observaciones_ruta.Text
            oAnticipoRuta.requiere_estipendio = If(Me.rbn_estipendio.SelectedValue = "1", True, False)
            oAnticipoRuta.registro_modificacion = True
            If oAnticipoRuta.requiere_estipendio Then
                oAnticipoRuta.id_lugar_estipendio = Me.cmb_lugar_estipendio.SelectedValue
            End If
            oAnticipoRuta.estipendio_desayuno = If(Me.rbn_estipendio_desayuno.SelectedValue = "1", True, False)
            oAnticipoRuta.estipendio_almuerzo = If(Me.rbn_estipendio_almuerzo.SelectedValue = "1", True, False)
            oAnticipoRuta.estipendio_cena = If(Me.rbn_estipendio_cena.SelectedValue = "1", True, False)
            oAnticipoRuta.cantidad_desayuno = Me.txt_cantidad_desayuno.Value
            oAnticipoRuta.cantidad_almuerzo = Me.txt_cantidad_almuerzo.Value
            oAnticipoRuta.cantidad_cena = Me.txt_cantidad_cena.Value
            oAnticipoRuta.valor_unitario_deayuno = Me.valor_desayuno.Value
            oAnticipoRuta.valor_unitario_almuerzo = Me.valor_almuerzo.Value
            oAnticipoRuta.valor_unitario_cena = Me.valor_cena.Value


            db.tme_anticipo_ruta.Add(oAnticipoRuta)
            db.SaveChanges()
            loadRutas(idAnticipo)
            Me.RadWindowRutas.VisibleOnPageLoad = False
        End Using

    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Using db As New dbRMS_JIEntities
            Dim id_anticipo = Convert.ToInt32(Me.Request.QueryString("id"))
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim anticipo = db.tme_anticipos.Find(id_anticipo)
            anticipo.finalizo_registro_participantes = True

            anticipo.observaciones_dipsersion_fondos = Me.txt_observaciones_fondos.Text
            For Each file As UploadedFile In soporte_legalizacion.UploadedFiles
                Dim fecha = DateTime.Now
                Dim exten = file.GetExtension()
                Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & anticipo.codigo_anticipo & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim Path As String
                Path = Server.MapPath(db.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)
                anticipo.soporte_verificacion_fondos = nombreArchivo
                db.Entry(anticipo).State = Entity.EntityState.Modified
                db.SaveChanges()
            Next


            db.Entry(anticipo).State = Entity.EntityState.Modified
            db.SaveChanges()
            Me.RadWindow2.VisibleOnPageLoad = False
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_anticipos"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub
End Class