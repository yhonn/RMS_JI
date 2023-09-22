Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL
Public Class frm_anticiposPrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJE_PRINT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Dim PathArchivos = ""

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
                    'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
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
                Dim id_anticipo = Convert.ToInt32(Me.Request.QueryString("id"))
                loadData(id_anticipo)

            End If
        End Using
    End Sub

    Sub loadData(ByVal idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim anticipo = dbEntities.vw_tme_anticipos.Where(Function(p) p.id_anticipo = idAnticipo).FirstOrDefault()
            Me.lbl_nombres.Text = anticipo.usuario_solicita
            Me.lbl_numero_documento.Text = anticipo.numero_documento
            Me.lbl_cargo.Text = anticipo.cargo_usuario
            Me.lbl_fecha_solicitud.Text = anticipo.fecha_anticipo
            Me.lbl_par.Text = anticipo.codigo_par
            Me.lbl_codigo_anticipo.Text = anticipo.codigo_anticipo
            Me.lbl_regional.Text = anticipo.nombre_subregion
            Me.lbl_tipo_Anticipo.Text = anticipo.tipo_par
            Me.lbl_motivo.Text = anticipo.motivo
            Me.lbl_medio_pago.Text = anticipo.medio_pago
            Me.lbl_observaciones_mp.Text = anticipo.observaciones_medio_pago
            Me.lbl_comision_mp.Text = anticipo.costo_total_giro

            If anticipo.id_tipo_par = 2 Then
                Me.eventos.Visible = True
                Me.compras.Visible = False
            Else
                Me.eventos.Visible = False
                Me.compras.Visible = True
            End If

            Dim rutas = dbEntities.vw_anticipo_detalle_ruta.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_rutas.DataSource = rutas
            Me.grd_rutas.DataBind()

            Dim compras = dbEntities.tme_anticipo_compra.Where(Function(p) p.id_anticipo = idAnticipo).ToList()
            Me.grd_compras.DataSource = compras
            Me.grd_compras.DataBind()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(anticipo.id_documento.ToString())
            Me.grd_cate.DataBind()
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
End Class