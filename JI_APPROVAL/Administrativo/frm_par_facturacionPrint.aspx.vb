Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL
Public Class frm_par_facturacionPrint
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
                Session.Remove("dtConceptos")
                Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                LoadData(id_par)

                'Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using

    End Sub
    Sub LoadData(ByVal id_par As Integer)
        Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_par = id_par).FirstOrDefault()
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_solicitud)
            Me.lbl_fecha_entrega.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_requiere_servicio)




            Me.lbl_solicitado.Text = par.usuario_solicita
            Me.lbl_aprobado.Text = par.nombre_departamento & " - " & par.nombre_municipio
            Me.lbl_cargo.Text = par.cargo_usuario
            Me.lbl_codigo_rfa.Text = If(par.asociado_actividad = True, par.subactividad, "")
            Me.lbl_departamento.Text = par.nombre_region

            Me.rbn_tipo_solicitud.DataSource = dbEntities.tme_tipo_solicitud_par.ToList()
            Me.rbn_tipo_solicitud.DataValueField = "id_tipo_solicitud"
            Me.rbn_tipo_solicitud.DataTextField = "tipo_solicitud"
            Me.rbn_tipo_solicitud.DataBind()
            Me.rbn_tipo_solicitud.SelectedValue = par.id_tipo_solicitud

            Me.lbl_tipo_par.Text = par.tipo_par

            Me.rbn_cargo_a.DataSource = dbEntities.tme_cargo_pares.ToList()
            Me.rbn_cargo_a.DataValueField = "id_cargo_a_par"
            Me.rbn_cargo_a.DataTextField = "cargo"
            Me.rbn_cargo_a.DataBind()
            Me.rbn_cargo_a.SelectedValue = par.id_cargo_a_par


            Dim parFacturacion = dbEntities.tme_par_detalle_factura.Where(Function(p) p.tme_par_factura.id_par = id_par).Select(Function(p) New With {Key _
                                                                                                                                p.id_factura,
                                                                                                                                p.id_factura_detalle,
                                                                                                                                p.tme_par_factura.id_par,
                                                                                                                                .proveedorDe = p.tme_par_factura.proveedor & " -> " & p.tme_par_factura.numero_factura & " -> " & p.tme_par_factura.tme_categoria_factura.categoria,
                                                                                                                                p.tme_par_factura.proveedor,
                                                                                                                                p.descripcion,
                                                                                                                                p.cantidad,
                                                                                                                                p.precio_unitario,
                                                                                                                                .valor_total = p.precio_unitario * p.cantidad,
                                                                                                                                p.tme_par_factura.tme_categoria_factura.categoria,
                                                                                                                                p.tme_par_factura.soporte,
                                                                                                                                p.tme_unidad_medida_par.unidad_medida
                                                                                                                                }).ToList()
            Me.lbl_codigo_facturacion.Text = If(par.id_tipo_par = 2, par.codigo_pt, par.codigo_facturación)

            Me.grd_servicios_requeridos.DataSource = parFacturacion
            Me.grd_servicios_requeridos.DataBind()

            Me.lbl_codigo_par.Text = par.codigo_par

            Me.grd_factura.DataSource = dbEntities.vw_tme_par_factura.Where(Function(p) p.id_par = id_par).ToList()
            Me.grd_factura.DataBind()

        End Using
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
        End If
    End Sub

End Class