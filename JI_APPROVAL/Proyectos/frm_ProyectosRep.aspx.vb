Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Globalization

Public Class frm_ProyectosRep
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "REP_PROY"
    Dim controles As New ly_SIME.CORE.cls_controles

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            Me.grd_aportes.Culture = cl_user.regionalizacionCulture
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
            If Not IsPostBack Then
                Try
                    Dim id = Convert.ToInt32(Request.QueryString("Id").ToString.Trim)
                    VerDatos(id, cl_user)
                Catch ex As Exception
                End Try

            End If

        End If
    End Sub

    Sub VerDatos(ByVal id As Integer, ByVal cl_user As ly_SIME.CORE.cls_user)
        If Not IsDBNull(id) Then

            Using dbentities As New dbRMS_HNEntities
                Dim oProyecto = dbentities.tme_Ficha_Proyecto.Find(id)

                Me.lbl_codigo_proyecto.Text = oProyecto.codigo_SAPME
                Me.lbl_estado_proyecto.Text = oProyecto.tme_FichaEstado.nombre_estado_ficha
                Me.lbl_nombre_proyecto.Text = oProyecto.nombre_proyecto
                Me.lbl_descripcion.Text = oProyecto.area_intervencion
                Me.lbl_fecha_inicio.Text = oProyecto.fecha_inicio_proyecto.Value.ToShortDateString()
                Me.lbl_fecha_fin.Text = oProyecto.fecha_fin_proyecto.Value.ToShortDateString()
                Me.lbl_region.Text = dbentities.vw_tme_ficha_proyecto_subregiones_all.FirstOrDefault(Function(p) p.id_ficha_proyecto = id).nombre_region
                Me.lbl_subregion.Text = dbentities.vw_tme_ficha_proyecto_subregiones_all.FirstOrDefault(Function(p) p.id_ficha_proyecto = id).nombre_subregion
                Me.lbl_componente.Text = oProyecto.tme_componente_programa.nombre_componente
                Me.lbl_ejecutor.Text = oProyecto.t_ejecutores.nombre_ejecutor
                Me.lbl_codigoAID.Text = oProyecto.codigo_ficha_AID
                Me.lbl_total.Text = oProyecto.costo_total_proyecto.Value.ToString("c2", cl_user.regionalizacionCulture)
                Me.lbl_totalUSD.Text = (oProyecto.costo_total_proyecto / oProyecto.tasa_cambio).Value.ToString("c2", cl_user.regionalizacionCulture)

                Me.grd_municipios.DataSource = dbentities.vw_tme_ficha_municipios.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto).ToList()
                Me.grd_municipios.DataBind()

                Me.grd_sectores.DataSource = dbentities.vw_tme_ficha_sectores.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto).ToList()
                Me.grd_sectores.DataBind()

                Me.grd_indicadores.DataSource = dbentities.vw_tme_ficha_meta_indicador.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto).ToList()
                Me.grd_indicadores.DataBind()

                Dim a = dbentities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto).ToList()
                Me.grd_aportes.DataSource = dbentities.vw_tme_ficha_aportes.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto).ToList()
                Me.grd_aportes.Culture = cl_user.regionalizacionCulture
                Me.grd_aportes.DataBind()

                Me.SqlDataSource7.SelectCommand = "SELECT ROW_NUMBER() OVER(ORDER BY id_anexo_ficha DESC) as Number,* FROM tme_Anexos_ficha WHERE id_ficha_proyecto=" & id
                Me.SqlDataSource7.DataBind()
                Me.GrdArchivos.DataBind()

                Dim sql As String = "SELECT ROW_NUMBER() OVER(ORDER BY id_ficha_proyecto DESC) as Number,nombre_archivo_proyecto, nombre_tipo_proyecto_imagen, Descripcion_Imagen"
                sql &= " FROM tme_FichaProyectoImagen INNER JOIN"
                sql &= " tme_FichaProyectoImageTipo ON tme_FichaProyectoImagen.id_tipo_proyecto_imagen = tme_FichaProyectoImageTipo.id_tipo_proyecto_imagen WHERE id_ficha_proyecto=" & id

                Me.SqlDataSource8.SelectCommand = sql
                Me.GrdArchivosImg.DataBind()

            End Using

            'Me.grd_indicadores.DataBind()
        End If


    End Sub


    Protected Sub GrdArchivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdArchivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            hlnk = CType(e.Item.FindControl("Attach"), HyperLink)
            hlnk.Target = "_blank"
            hlnk.NavigateUrl = "../FileUploads/" & e.Item.Cells(3).Text.ToString
            e.Item.Cells(4).Text = "." & e.Item.Cells(3).Text.ToString.Substring(e.Item.Cells(3).Text.Length - 4, 4).Replace(".", "")
        End If
    End Sub

    Protected Sub GrdArchivosImg_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles GrdArchivosImg.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            hlnk = CType(e.Item.FindControl("AttachImg"), HyperLink)
            hlnk.Target = "_blank"
            hlnk.NavigateUrl = "../FileUploads/" & e.Item.Cells(3).Text.ToString
        End If

    End Sub

    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        'For Each column As GridColumn In grd_aportes.Columns
        '    If column.UniqueName.Contains("fecha") Then
        '        Dim a = TryCast(column, GridBoundColumn)
        '        a.DataFormatString = "dd/mm/yyyy"
        '    ElseIf column.UniqueName.Contains("monto") Then
        '        Dim a = TryCast(column, GridBoundColumn)
        '        a.DataFormatString = "{0:c2}"
        '    End If
        'Next
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim dataItem As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim monto = Convert.ToDecimal(dataItem("colm_monto_aporte").Text)
            dataItem("colm_monto_aporte").Text = monto.ToString("c2", cl_user.regionalizacionCulture)

            monto = Convert.ToDecimal(dataItem("colm_monto_aporte_obligado").Text)
            dataItem("colm_monto_aporte_obligado").Text = monto.ToString("c2", cl_user.regionalizacionCulture)
        End If
    End Sub
End Class