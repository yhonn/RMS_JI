Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL
Public Class frm_viaje_legalizacionPrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJE_LEG_PRINT"
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
                Session.Remove("dtItinerario")
                Session.Remove("dtAlojamiento")
                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                'LoadData(id_viaje)
                loadData(id_viaje)
                fillGridLegalizacion(id_viaje)
                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using
    End Sub
    Sub fillGridLegalizacion(ByVal id_viaje As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim legalizadionDetalle = dbEntities.vw_tme_solicitud_viaje_legalizacion.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_pasajes.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 1).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_pasajes.DataBind()

            Me.grd_reuniones.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 2).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_reuniones.DataBind()

            Me.grd_miscelaneos.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 3).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_miscelaneos.DataBind()

            Me.grd_alimentacion_alojamiento.DataSource = legalizadionDetalle.Where(Function(p) p.id_tipo_legalizacion = 4).OrderBy(Function(p) p.fecha).ToList()
            Me.grd_alimentacion_alojamiento.DataBind()


        End Using

    End Sub
    Sub LoadData(ByVal id_viaje As Integer)

        Using dbEntities As New dbRMS_JIEntities
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Dim viajeDetail = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(viajeDetail.id_documento_legalizacion.ToString())
            Me.grd_cate.DataBind()
            Dim id_usuario_app = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault().id_usuario_app

            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.lbl_nombres.Text = viaje.t_usuarios.nombre_usuario & " " & viaje.t_usuarios.apellidos_usuario
            Me.lbl_numero_documento.Text = viaje.t_usuarios.numero_documento
            Me.lbl_cargo.Text = viaje.t_job_title.job
            If viaje.fecha_legalizacion IsNot Nothing Then
                Me.lbl_fecha_solicitud.Text = viaje.fecha_legalizacion
            End If
            Me.lbl_motivo.Text = viaje.motivo_viaje
            Me.lbl_regional.Text = viajeDetail.region_solicita
            Me.lbl_codigo_viaje.Text = viaje.codigo_solicitud_viaje

            Me.lbl_Tasa_ser.Text = viaje.tasa_ser
            Me.lbl_fecha_radicacion.Text = viaje.fecha_tasa_ser
            Me.lbl_total_legalizacion.Text = "$" & String.Format("{0:N}", viaje.tme_solicitud_viaje_legalizacion.Sum(Function(p) p.monto_total))


            If viaje.fecha_inicio_viaje_legalizacion IsNot Nothing Then
                Me.lbl_fecha_hora_inicio.Text = viaje.fecha_inicio_viaje_legalizacion.Value.Year & "-" & viaje.fecha_inicio_viaje_legalizacion.Value.Month & "-" & viaje.fecha_inicio_viaje_legalizacion.Value.Day & " " & viaje.hora_inicio_viaje
            End If
            If viaje.fecha_finalizacion_viaje_legalizacion IsNot Nothing Then
                Me.lbl_fecha_hora_fin.Text = viaje.fecha_finalizacion_viaje_legalizacion.Value.Year & "-" & viaje.fecha_finalizacion_viaje_legalizacion.Value.Month & "-" & viaje.fecha_finalizacion_viaje_legalizacion.Value.Day & " " & viaje.hora_fin_viaje
            End If

            Dim archivos = dbEntities.vw_tme_solicitud_viaje_legalizacion_soportes.Where(Function(p) p.id_viaje = viaje.id_viaje).ToList()
            Me.grd_soportes.DataSource = archivos
            Me.grd_soportes.DataBind()

        End Using
    End Sub
    Protected Sub grd_soportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_soportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()

            Dim hlnkSoporte As HyperLink = New HyperLink
            hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)
            hlnkSoporte.NavigateUrl = PathArchivos & soporte

        End If
    End Sub
End Class