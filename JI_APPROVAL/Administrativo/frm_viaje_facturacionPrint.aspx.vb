Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL

Public Class frm_viaje_facturacionPrint
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
                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                'LoadData(id_viaje)
                loadData(id_viaje)

                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using
    End Sub
    Sub loadData(ByVal id_viaje As Integer)

        Using dbEntities As New dbRMS_JIEntities
            Dim viajeList = dbEntities.tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Dim viaje = viajeList.FirstOrDefault()
            Me.lbl_nombres.Text = viaje.t_usuarios.nombre_usuario & " " & viaje.t_usuarios.apellidos_usuario
            Me.lbl_numero_documento.Text = viaje.t_usuarios.numero_documento
            Me.lbl_cargo.Text = viaje.t_job_title.job
            Me.lbl_fecha_solicitud.Text = viaje.fecha_crea
            Me.lbl_regional.Text = viaje.t_subregiones.t_regiones.nombre_region & " - " & viaje.t_subregiones.nombre_subregion
            Me.lbl_codigo_viaje.Text = viaje.codigo_solicitud_viaje
            Me.lbl_observaciones.Text = viaje.observaciones_facturacion



            Dim facturacionAlquiler = dbEntities.tme_solicitud_viaje_facturacion_vehiculo.Where(Function(p) p.id_viaje = id_viaje).ToList()
            Me.grd_facturacion_alquiler.DataSource = facturacionAlquiler
            'Me.grd_facturacion_alquiler.DataBind()
            'If viajeList.Where(Function(p) p.fecha_radicacion_alquiler IsNot Nothing).Count() > 0 Then
            'Else
            '    Me.grd_facturacion_alquiler.DataSource = New List(Of tme_solicitud_viaje)
            '    Me.grd_facturacion_alquiler.DataBind()
            'End If


            Dim viajeTiquetes = viaje.tme_solicitud_viaje_facturacion_tiquete.ToList()

            If viajeList.Where(Function(p) p.fecha_radicacion_tiquete IsNot Nothing).Count() > 0 Then
                Me.grd_facturacion_tiquetes.DataSource = viajeList.ToList()
                Me.grd_facturacion_tiquetes.DataBind()
            Else
                Me.grd_facturacion_tiquetes.DataSource = New List(Of tme_solicitud_viaje_facturacion_tiquete)
                Me.grd_facturacion_tiquetes.DataBind()
            End If

            Dim vuelo = viaje.tme_solicitud_viaje_facturacion_tiquete.Select(Function(p) _
                                           New With {Key .id_solicitud_viaje_facturacion_tiquete = p.id_solicitud_viaje_facturacion_tiquete,
                                                     Key .fecha_vuelo = p.fecha_vuelo,
                                                     Key .aerolinea = p.aerolinea,
                                                     Key .ciudad_origen = p.t_municipios.t_departamentos.nombre_departamento & " - " & p.t_municipios.nombre_municipio,
                                                     Key .ciudad_destino = p.t_municipios1.t_departamentos.nombre_departamento & " - " & p.t_municipios1.nombre_municipio
                                                     }).ToList()

            Me.grd_vuelos.DataSource = vuelo.ToList()
            Me.grd_vuelos.DataBind()

            Dim hotel = viaje.tme_solicitud_viaje_facturacion_hotel.Select(Function(p) _
                                           New With {Key .id_solicitud_viaje_facturacion_hotel = p.id_solicitud_viaje_facturacion_hotel,
                                                     Key .fecha_desde = p.fecha_desde,
                                                     Key .fecha_hasta = p.fecha_hasta,
                                                     Key .ciudad_origen = p.t_municipios.t_departamentos.nombre_departamento & " - " & p.t_municipios.nombre_municipio,
                                                     Key .hotel_alojamiento = p.hotel_alojamiento,
                                                     Key .valor_total = p.valor_total,
                                                     Key .numero_factura = p.numero_factura,
                                                     Key .fecha_radicacion = p.fecha_radicacion,
                                                     Key .soporte = p.soporte
                                                     }).ToList()

            Me.grd_hotel.DataSource = hotel.ToList()
            Me.grd_hotel.DataBind()

        End Using
    End Sub

    Protected Sub grd_facturacion_alquiler_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_facturacion_alquiler.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte").ToString()

            Dim hlnkSoporte As HyperLink = New HyperLink
            hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)
            hlnkSoporte.NavigateUrl = PathArchivos & soporte

        End If
    End Sub

    Protected Sub grd_facturacion_tiquetes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_facturacion_tiquetes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'If DataBinder.Eval(e.Item.DataItem, "soporte_tiquetes") IsNot Nothing Then
            '    Dim soporte = DataBinder.Eval(e.Item.DataItem, "soporte_tiquetes").ToString()
            '    Dim hlnkSoporte As HyperLink = New HyperLink
            '    hlnkSoporte = CType(e.Item.FindControl("col_hlk_soporte"), HyperLink)
            '    hlnkSoporte.NavigateUrl = PathArchivos & soporte
            'End If

        End If
    End Sub

    Protected Sub grd_hotel_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_hotel.ItemDataBound
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