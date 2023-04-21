Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports ly_SIME

Public Class frm_sgtoHitos
    Inherits System.Web.UI.Page
    Dim cl_user As New ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_STO_HITOS"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim db As New dbRMS_JIEntities
    Dim clListados As New ly_SIME.CORE.cls_listados
    'Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
    '    fillGrid(1)
    'End Sub
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                'cl_user.chk_Rights(Page.Controls, 7, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=4")
            LoadLista()
            'fillGrid(1)
        End If
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "~/Proyectos/frm_entregables_Act?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            hlnkPrint.NavigateUrl = "../Proyectos/frm_hitos_print?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()


            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "~/Proyectos/frm_hitosEj?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            'Dim hlnkComentarios As HyperLink = New HyperLink
            'hlnkComentarios = CType(e.Item.FindControl("hlk_productos"), HyperLink)
            'hlnkComentarios.NavigateUrl = "~/Proyectos/frm_comentarios_productos?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
        End If
    End Sub
    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        getCmbSelected()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        getCmbSelected()
    End Sub

    Sub fillGrid(ByVal caso As Integer)
        Using dbEntities As New dbRMS_JIEntities

            Dim listarTodas = Convert.ToString(Me.h_Filter.Value)
            'caso = 3

            'If chk_Todos.Checked Then
            '    caso = 1
            'End If
            'If chk_TodosR.Checked Then
            '    caso = 2
            'End If

            Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            Dim usuario = dbEntities.t_usuarios.Find(id_usuario)
            Dim regionesUsuario = usuario.t_usuario_subregion.Select(Function(p) p.id_subregion).ToList()

            Dim regionStr = String.Join(",", regionesUsuario)

            Dim id_programa = Me.Session("E_IDPrograma").ToString()
            Dim id_tipo_usuario = cl_user.getUsuarioField("id_tipo_usuario", "id_usuario", Me.Session("E_IdUser"))
            Dim busqueda_programa = cl_user.getUsuarioField("busqueda_actividad", "id_usuario", Me.Session("E_IdUser"))
            Dim ListPro = cl_user.chk_accPRO()
            'Dim proyectos = db.vw_tme_proyecto_hitos.Where(Function(p) p.numero_hitos > 0).ToList()
            Dim id_ejecutor = 0
            Dim sqlFiltros = " where 0 = 0 and es_componente = 0 "
            Dim sqlFiltros2 = " and 0 = 0"
            'Select Case caso
            '    Case 2
            '        'sqlFiltros2 = sqlFiltros2 & " and (case when MONTH(a.fecha_inicio_proyecto) > 9 and MONTH(a.fecha_inicio_proyecto) <= 12 then YEAR(a.fecha_inicio_proyecto) + 1 else YEAR(a.fecha_inicio_proyecto) end) = " & Me.cmb_anio.SelectedValue
            '        sqlFiltros = sqlFiltros & " and (case when MONTH(a.fecha_inicio_proyecto) > 9 and MONTH(a.fecha_inicio_proyecto) <= 12 then YEAR(a.fecha_inicio_proyecto) + 1 else YEAR(a.fecha_inicio_proyecto) end) = " & Me.cmb_anio.SelectedValue
            '    Case 3
            '        sqlFiltros2 = sqlFiltros2 & " and z.id_estado_entregable = " & Me.cmb_estado_producto.SelectedValue
            '    'sqlFiltros = sqlFiltros & " and b.id_estado_entregable = " & Me.cmb_estado_producto.SelectedValue
            '    Case 4
            '        'sqlFiltros = sqlFiltros & " and (case when MONTH(a.fecha_inicio_proyecto) > 9 and MONTH(a.fecha_inicio_proyecto) <= 12 then YEAR(a.fecha_inicio_proyecto) + 1 else YEAR(a.fecha_inicio_proyecto) end) = " & Me.cmb_anio.SelectedValue & " and b.id_estado_entregable = " & Me.cmb_estado_producto.SelectedValue
            '        sqlFiltros = sqlFiltros & " and (case when MONTH(a.fecha_inicio_proyecto) > 9 and MONTH(a.fecha_inicio_proyecto) <= 12 then YEAR(a.fecha_inicio_proyecto) + 1 else YEAR(a.fecha_inicio_proyecto) end) = " & Me.cmb_anio.SelectedValue
            '        sqlFiltros2 = sqlFiltros2 & " and z.id_estado_entregable = " & Me.cmb_estado_producto.SelectedValue
            'End Select
            If id_tipo_usuario = 2 Then
                sqlFiltros2 = sqlFiltros2 & " and d.id_tipo_usuario = 2 "
                sqlFiltros = sqlFiltros & " and d.id_tipo_usuario = 2 "
                Dim ejecutor = db.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
                id_ejecutor = 0
                If ejecutor.Count() > 0 Then
                    id_ejecutor = ejecutor.FirstOrDefault().id_ejecutor
                    sqlFiltros2 = sqlFiltros2 & " and a.id_ejecutor = " & id_ejecutor
                    sqlFiltros = sqlFiltros & " and a.id_ejecutor = " & id_ejecutor
                End If

            End If

            If busqueda_programa = True Then
                If ListPro.Count() > 0 Then
                    sqlFiltros2 = sqlFiltros2 & " and a.id_ficha_proyecto in (" & String.Join(",", ListPro) & ")"
                    sqlFiltros = sqlFiltros & " and a.id_ficha_proyecto in (" & String.Join(",", ListPro) & ")"
                End If
            Else
                sqlFiltros &= " and (e.id_subregion in (" & regionStr & ")) "

                'proyectos = proyectos.Where(Function(p) ListPro.Contains(p.id_ficha_proyecto)).ToList()
            End If



            If listarTodas = "" Then
            Else
                sqlFiltros = " where 0 = 0"
                sqlFiltros2 = " and 0 = 0"
            End If

            If caso = 5 Then
                sqlFiltros = sqlFiltros & " and (a.codigo_rfa like '%" & Me.txt_doc.Text & "%' or a.nombre_ejecutor like '%" & Me.txt_doc.Text & "%' or a.nombre_proyecto like '%" & Me.txt_doc.Text & "%')"
            End If

            Me.sql_grid.SelectCommand = "select a.id_ficha_proyecto, a.nombre_proyecto, a.nombre_ejecutor, a.nombre_region, a.nombre_estado_ficha, a.id_region, a.id_subregion, 
             count(b.id_hito) numero_hitos,
             0.00 avance_actividad,
             a.codigo_SAPME,
             a.id_ficha_estado,
             a.id_tipo_usuario, 
             a.id_programa, 
             a.codigo_MONITOR, 
             a.codigo_RFA, 
             a.id_ejecutor, 
            (select count(*) from vw_tme_ficha_proyecto_hitos_entregables z where z.id_ficha_proyecto = a.id_ficha_proyecto )  nro_entregables 
             from vw_tme_ficha_proyecto a 
             left join vw_ficha_proyecto_hitos b on a.id_ficha_proyecto = b.id_ficha_proyecto 
             Left JOIN t_ejecutor_usuario c on A.id_ejecutor = c.id_ejecutor      
			 Left JOIN t_usuarios as d on c.id_usuario = d.id_usuario 
             left join tme_ficha_subregion e on a.id_ficha_proyecto = e.id_ficha_proyecto
             " & sqlFiltros & " 
             group by a.id_ficha_proyecto, a.nombre_proyecto, a.nombre_ejecutor, a.nombre_region, a.nombre_estado_ficha, a.id_region, a.id_subregion,
             a.codigo_SAPME,
             a.id_ficha_estado, 
             a.id_tipo_usuario, 
             a.id_programa, 
             a.codigo_MONITOR,
             a.codigo_RFA, 
             d.id_tipo_usuario,
             a.id_ejecutor having count(b.id_hito) > 0"



            'Me.sql_grid.SelectCommand = "select a.id_ficha_proyecto, a.nombre_proyecto, a.nombre_ejecutor, a.nombre_region, a.nombre_estado_ficha, a.id_region, a.id_subregion, " &
            '    " count(b.id_ficha_producto) numero_productos," &
            '    " 0.00 avance_actividad," &
            '    " a.codigo_SAPME," &
            '    " a.id_ficha_estado," &
            '    " a.id_tipo_usuario, " &
            '    " a.id_programa, " &
            '    " a.codigo_MONITOR, " &
            '    " a.codigo_RFA, " &
            '    " a.id_ejecutor, " &
            '    "(select count(*) from vw_entregables z where z.id_ficha_proyecto = a.id_ficha_proyecto " & sqlFiltros2 & ")  nro_sub_productos " &
            '    " from vw_tme_ficha_proyecto a " &
            '    " left join vw_productos b on a.id_ficha_proyecto = b.id_ficha_proyecto" & sqlFiltros &
            '    " group by a.id_ficha_proyecto, a.nombre_proyecto, a.nombre_ejecutor, a.nombre_region, a.nombre_estado_ficha, a.id_region, a.id_subregion," &
            '    " a.codigo_SAPME," &
            '    " a.id_ficha_estado, " &
            '    " a.id_tipo_usuario, " &
            '    " a.id_programa, " &
            '    " a.codigo_MONITOR," &
            '    " a.codigo_RFA, " &
            '    " a.id_ejecutor having count(b.id_ficha_producto) > 0"
            'Select Case caso
            '    'Todas las subregiones de una region

            '    Case 1
            '        'Dim regions = dbentities.t_regiones.Where(Function (p) p.id_programa == )
            '        If id_tipo_usuario = 2 Then
            '            Dim ejecutor = dbentities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
            '            Dim id_ejecutor = 0
            '            If ejecutor.Count() > 0 Then
            '                id_ejecutor = ejecutor.FirstOrDefault().id_ejecutor
            '            End If
            '            proyectos = proyectos _
            '                .Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                    p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_region.Contains(Me.cmb_region.SelectedValue) And p.id_ficha_estado > 1 And p.id_tipo_usuario = id_tipo_usuario And p.id_ejecutor = id_ejecutor).ToList()
            '        Else
            '            proyectos = proyectos _
            '                .Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                    p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_region.Contains(Me.cmb_region.SelectedValue) And p.id_ficha_estado > 1).ToList()
            '        End If
            '        'Todos las regiones
            '    Case 2
            '        If id_tipo_usuario = 2 Then
            '            Dim ejecutor = dbentities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
            '            Dim id_ejecutor = 0
            '            If ejecutor.Count() > 0 Then
            '                id_ejecutor = ejecutor.FirstOrDefault().id_ejecutor
            '            End If
            '            proyectos = proyectos.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                                                                 p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_programa.Contains(id_programa) And p.id_ficha_estado > 1 And p.id_tipo_usuario = id_tipo_usuario And p.id_ejecutor = id_ejecutor).ToList()
            '        Else
            '            proyectos = proyectos.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                                                                 p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_programa.Contains(id_programa) And p.id_ficha_estado > 1).ToList()
            '        End If
            '        'SubRegión especifica
            '    Case 3
            '        If id_tipo_usuario = 2 Then
            '            Dim ejecutor = dbentities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
            '            Dim id_ejecutor = 0
            '            If ejecutor.Count() > 0 Then
            '                id_ejecutor = ejecutor.FirstOrDefault().id_ejecutor
            '            End If
            '            proyectos = proyectos.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                                                                 p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_subregion.Contains(Me.cmb_subregion.SelectedValue) And p.id_ficha_estado > 1 And p.id_tipo_usuario = id_tipo_usuario And p.id_ejecutor = id_ejecutor).ToList()
            '        Else
            '            proyectos = proyectos.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
            '                                                                                 p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_subregion.Contains(Me.cmb_subregion.SelectedValue) And p.id_ficha_estado > 1).ToList()
            '        End If
            'End Select

            'Me.grd_cate.DataSource = proyectos.ToList()
            Me.grd_cate.DataBind()
        End Using

    End Sub

    Sub LoadLista()
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        'Me.cmb_region.DataSourceID = ""
        'Me.cmb_region.DataSource = clListados.get_t_regiones(idPrograma)
        'Me.cmb_region.DataTextField = "nombre_region"
        'Me.cmb_region.DataValueField = "id_region"
        'Me.cmb_region.DataBind()

        'Me.cmb_subregion.DataSourceID = ""
        'Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        'Me.cmb_subregion.DataTextField = "nombre_subregion"
        'Me.cmb_subregion.DataValueField = "id_subregion"
        'Me.cmb_subregion.DataBind()

        'Me.chk_Todos.Checked = True
        'Me.chk_TodosR.Checked = True
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Me.cmb_anio.DataSourceID = ""
        Dim anioActual As Integer = DateTime.Now.Year
        Dim fecha = DateTime.Now

        If fecha.Month >= 10 Then
            anioActual = DateTime.Now.Year + 1
        End If

        Me.cmb_anio.DataSource = db.t_programa_planificacion.Where(Function(p) (p.id_programa = id_programa Or id_programa = 0) And (p.anio_ejecucion > 2017 And p.anio_ejecucion <= anioActual)) _
                            .Select(Function(p) _
                                        New With {Key .anio_ejecucion = p.anio_ejecucion,
                                                Key .id_anio = p.anio_ejecucion}).ToList()
        Me.cmb_anio.DataTextField = "anio_ejecucion"
        Me.cmb_anio.DataValueField = "id_anio"
        Me.cmb_anio.DataBind()
        Me.cmb_anio.SelectedValue = anioActual

        Me.cmb_estado_producto.DataSourceID = ""
        Me.cmb_estado_producto.DataSource = db.tme_estado_entregables_hitos.Where(Function(p) p.id_programa = id_programa).ToList()
        Me.cmb_estado_producto.DataTextField = "estado"
        Me.cmb_estado_producto.DataValueField = "id_estado_entregable"
        Me.cmb_estado_producto.DataBind()
        getCmbSelected()
    End Sub

    Protected Sub cmb_anio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_anio.SelectedIndexChanged
        getCmbSelected()
    End Sub
    Protected Sub cmb_estado_producto_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_estado_producto.SelectedIndexChanged
        getCmbSelected()
    End Sub
    Sub getCmbSelected()
        Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=4")
        If (Me.cmb_anio.SelectedValue.Length > 0 And chk_TodosA.Checked = False) And (Me.cmb_estado_producto.SelectedValue.Length > 0 And chk_TodosP.Checked = False) Then
            'Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&anio=" & Me.cmb_anio.SelectedValue & "&estado=" & Me.cmb_estado_producto.SelectedValue)
            fillGrid(4)
        ElseIf (Me.cmb_anio.SelectedValue.Length > 0 And chk_TodosA.Checked = False) And (Me.cmb_estado_producto.SelectedValue.Length = 0 And chk_TodosP.Checked = False) Then
            'Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&anio=" & Me.cmb_anio.SelectedValue)
            fillGrid(2)
        ElseIf (Me.cmb_anio.SelectedValue.Length = 0 And chk_TodosA.Checked = False) And (Me.cmb_estado_producto.SelectedValue.Length > 0 And chk_TodosP.Checked = False) Then
            'Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&estado=" & Me.cmb_estado_producto.SelectedValue)
            fillGrid(3)
        ElseIf (Me.chk_TodosA.Checked = True) And (Me.cmb_estado_producto.SelectedValue.Length > 0 And chk_TodosP.Checked = False) Then
            'Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&estado=" & Me.cmb_estado_producto.SelectedValue)
            fillGrid(3)
        ElseIf (Me.cmb_anio.SelectedValue.Length > 0 And chk_TodosA.Checked = False) And (Me.chk_TodosP.Checked = True) Then
            'Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&anio=" & Me.cmb_anio.SelectedValue)
            fillGrid(2)
        Else
            fillGrid(1)
        End If
    End Sub

    'Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
    '    Me.cmb_subregion.DataSourceID = ""
    '    Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
    '    Me.cmb_subregion.DataTextField = "nombre_subregion"
    '    Me.cmb_subregion.DataValueField = "id_subregion"
    '    Me.cmb_subregion.DataBind()
    '    fillGrid(3)
    'End Sub

    Protected Sub Chk_TodosR_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosA.CheckedChanged
        viewCheck()
    End Sub
    Sub viewCheck()
        If chk_TodosA.Checked And chk_TodosP.Checked Then
            Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9")
            Me.cmb_anio.Enabled = False
            Me.cmb_estado_producto.Enabled = False
            fillGrid(1)
        ElseIf chk_TodosA.Checked And (chk_TodosP.Checked = False And cmb_estado_producto.SelectedValue.Length > 0) Then
            Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&estado=" & Me.cmb_estado_producto.SelectedValue)
            Me.cmb_anio.Enabled = False
            Me.cmb_estado_producto.Enabled = True
            fillGrid(3)
        ElseIf chk_TodosA.Checked And (chk_TodosP.Checked = False And cmb_estado_producto.SelectedValue.Length = 0) Then
            Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9")
            Me.cmb_anio.Enabled = False
            Me.cmb_estado_producto.Enabled = True
            fillGrid(1)
        ElseIf chk_TodosP.Checked And (chk_TodosA.Checked = False And cmb_anio.SelectedValue.Length > 0) Then
            Me.cmb_anio.Enabled = True
            Me.cmb_estado_producto.Enabled = False
            Me.exportar.Attributes.Add("href", "~/Reportes/xls?id=9&anio=" & Me.cmb_anio.SelectedValue)
            fillGrid(2)
        ElseIf chk_TodosP.Checked And (chk_TodosA.Checked = False And cmb_anio.SelectedValue.Length = 0) Then
            Me.cmb_anio.Enabled = True
            Me.cmb_estado_producto.Enabled = False
            fillGrid(1)
        Else
            Me.cmb_anio.Enabled = True
            Me.cmb_estado_producto.Enabled = True
            getCmbSelected()
        End If
    End Sub
    Protected Sub Chk_Todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosP.CheckedChanged
        viewCheck()
    End Sub

    Private Sub btn_buscar_Click(sender As Object, e As EventArgs) Handles btn_buscar.Click
        fillGrid(5)
    End Sub
End Class