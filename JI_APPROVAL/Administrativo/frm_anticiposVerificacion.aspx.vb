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
            Dim idAnticipo = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.id_anticipo.Value = idAnticipo
            loadLista()
            loadData(idAnticipo)
            loadRutas(idAnticipo)
        End If
    End Sub
    Sub loadRutas(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim rutas = dbEntities.vw_anticipo_detalle_ruta.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_rutas.DataSource = rutas
            Me.grd_rutas.DataBind()
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

    Protected Sub participantes(sender As Object, e As EventArgs)

        Using dbEntities As New dbRMS_JIEntities
            Dim a = CType(sender, LinkButton)
            Dim idRutaAnticipo = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Me.id_ruta_anticipo.Value = idRutaAnticipo

            Dim rol = dbEntities.tme_anticipo_ruta.Find(idRutaAnticipo)

            Me.grd_conceptos.DataSource = dbEntities.tme_anticipo_ruta_participantes.Where(Function(p) p.id_ruta_anticipo = idRutaAnticipo).ToList()
            Me.grd_conceptos.DataBind()

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
            participante.nombres = Me.txt_nombre.Text
            participante.primer_apellido = Me.txt_primer_apellido.Text
            participante.segundo_apellido = Me.txt_segundo_apellido.Text
            participante.valor = Me.txt_valor.Value
            participante.telefono = Me.txt_numero_telefono.Text
            participante.tipo_ocumento = Me.txt_tipo_documento.Text
            dbEntities.tme_anticipo_ruta_participantes.Add(participante)
            dbEntities.SaveChanges()

            Me.grd_conceptos.DataSource = dbEntities.tme_anticipo_ruta_participantes.Where(Function(p) p.id_ruta_anticipo = idRutaAnticipo).ToList()
            Me.grd_conceptos.DataBind()

        End Using
    End Sub

    Private Sub btn_guardar_finalizar_Click(sender As Object, e As EventArgs) Handles btn_guardar_finalizar.Click
        Me.RadWindow2.VisibleOnPageLoad = False
    End Sub
End Class