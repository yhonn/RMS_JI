Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_hitosEj
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_STO_HITOS"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListados As New ly_SIME.CORE.cls_listados
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

            Me.btn_eliminarAportes.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities
                Dim idFichaPro = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = idFichaPro

                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = idFichaPro)
                Me.lbl_cod_acti.Text = proyecto.codigo_RFA
                Me.lbl_actividad.Text = proyecto.nombre_proyecto
                Me.lbl_fecha_inicio.Text = proyecto.fecha_inicio_proyecto
                Me.lbl_fecha_fin.Text = proyecto.fecha_fin_proyecto
                Me.lbl_ejecutor.Text = proyecto.nombre_ejecutor
                LoadLista()
                fillGrid(1)
                'Me.SqlDts_comentarios.SelectCommand = "select * from vw_productos where id_ficha_entregable = " & idEntregable
                'Me.grd_aportes.DataBind()
            End Using
        End If
    End Sub
    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "~/Proyectos/frm_sgtoEntregables?id=" & DataBinder.Eval(e.Item.DataItem, "id_hito").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_hito").ToString())


            Dim hlnkDetalle As HyperLink = New HyperLink
            hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            hlnkDetalle.NavigateUrl = "frm_hito_entregablePrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_hito").ToString()

            If DataBinder.Eval(e.Item.DataItem, "fecha_entrega").ToString().Length = 0 Then
                hlnkDetalle.Visible = False
            End If


        End If
    End Sub
    Sub LoadLista()
        Using dbentities As New dbRMS_JIEntities
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
            'Me.cmb_estado_producto.DataSourceID = ""
            'Me.cmb_estado_producto.DataSource = dbentities.tme_estado_entregables_hitos.Where(Function(p) p.id_programa = id_programa).ToList()
            'Me.cmb_estado_producto.DataTextField = "estado"
            'Me.cmb_estado_producto.DataValueField = "id_estado_entregable"
            'Me.cmb_estado_producto.DataBind()
        End Using
    End Sub
    Sub generarPdf(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Dim identity = a.Attributes.Item("data-identity").ToString()
        Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)

            Dim startInfo = New ProcessStartInfo()
            startInfo.UseShellExecute = False
            startInfo.RedirectStandardOutput = True
            startInfo.RedirectStandardInput = True
            startInfo.RedirectStandardError = True
            startInfo.FileName = "C:\\Program Files\\wkhtmltopdf\\bin\\wkhtmltopdf.exe"
            'startInfo.Arguments = "http://rms.paramosybosques.org/RMS_SIME/Proyectos/frm_formato_aprobacion_hitos?id=" & identity + " " + "E:\\inetpub\\wwwroortPB\\RMS_SIME\\formatoentregables\\doc_hito_" & identity & ".pdf"
            startInfo.Arguments = "http://localhost:37812/Proyectos/frm_formato_aprobacion_hitos?id=" & identity + " " + "C:\\Users\\jhons\\Source\\Workspaces\\RMS_PB\\RMS_SIME_PB\\PB_SIME\\formatoentregables\\doc_hito_" & identity & ".pdf"
            Dim myProcess = Process.Start(startInfo)
            myProcess.WaitForExit()
            myProcess.Close()
            'Response.Clear()
            'Response.AddHeader("content-disposition", "attachment;filename=doc_hito_" & identity & ".pdf")
            'Response.ContentType = "application/pdf"
            ''Response.WriteFile("C:\\db\\doc_hito_" & identity & ".pdf")
            'Response.WriteFile("C:\\Users\\jhons\\Source\\Workspaces\\RMS_PB\\RMS_SIME_PB\\PB_SIME\\formatoentregables\\doc_hito_" & identity & ".pdf")
            'Response.End()
        Catch ex As Exception
            Dim errore = ex.Message
        End Try
        Me.Response.Write("<script>")
        Me.Response.Write("window.open('~/formatoentregables/doc_hito_" & identity & ".pdf','_blank')")
        Me.Response.Write("</script>")
        'Me.Response.Redirect("~/formatoentregables/doc_hito_" & identity & ".pdf")
    End Sub
    'Protected Sub cmb_anio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_anio.SelectedIndexChanged
    '    getCmbSelected()
    'End Sub
    'Protected Sub cmb_estado_producto_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_estado_producto.SelectedIndexChanged
    '    getCmbSelected()
    'End Sub
    'Sub getCmbSelected()
    '    If Me.cmb_estado_producto.SelectedValue.Length > 0 Then
    '        fillGrid(3)
    '    Else
    '        fillGrid(1)
    '    End If
    'End Sub
    'Protected Sub Chk_TodosR_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosP.CheckedChanged
    '    viewCheck()
    'End Sub
    'Sub viewCheck()
    '    If chk_TodosP.Checked Then
    '        Me.cmb_estado_producto.Enabled = False
    '        fillGrid(1)
    '    Else
    '        Me.cmb_estado_producto.Enabled = True
    '        getCmbSelected()
    '    End If
    'End Sub
    Sub fillGrid(ByVal caso As Integer)
        Dim sqlFiltros = " "
        Me.SqlDts_comentarios.SelectCommand = "select *, numero_entregables as numero_entregables from vw_ficha_proyecto_hitos where id_ficha_proyecto = " & Me.lbl_id_ficha.Text & " order by nro_hito"
        Select Case caso
            Case 3
                Me.SqlDts_comentarios.SelectCommand = "select a.*, count(b.id_entregable_hito) numero_entregables from vw_ficha_proyecto_hitos a 
                     inner join tme_hitos_entregables b on a.id_hito = b.id_hito 
                     where a.id_ficha_proyecto = " & Me.lbl_id_ficha.Text & "
                     group by a.id_hito, a.id_ficha_proyecto, descripcion_hito, a.fecha_esperada_entrega, 
                     a.nro_hito, a.aprobado, dias_restantes, alerta, id_estado_aprobacion_hito, a.estado, a.fecha_entrega, 
                     nombre_ejecutor, codigo_RFA, codigo_MONITOR, nombre_proyecto, fecha_inicio_proyecto, fecha_fin_proyecto, 
                     id_usuario_ejecutor, a.id_usuario_responsable, usuario_responsable, 
                     usuario_ejecutor, a.fecha_aprobacion, texto_alerta, id_mecanismo_contratacion, a.numero_entregables, a.valor 
                     order by a.nro_hito"



                'Me.SqlDts_comentarios.SelectCommand = "select a.*, count(b.id_ficha_entregable) nro_sub_productos from vw_productos a " &
                '    "inner join tme_ficha_entregables b on a.id_ficha_producto = b.id_ficha_producto " &
                '    "where a.id_ficha_proyecto = " & Me.lbl_id_ficha.Text & " and b.id_estado_entregable = " & Me.cmb_estado_producto.SelectedValue & " " &
                '    "group by a.id_ficha_producto, a.id_ficha_proyecto, nombre_producto, fecha_esperada_entrega, " &
                '    "nro_producto, a.aprobado, dias_restantes, alerta, id_estado_producto, estado, a.fecha_entrega, " &
                '    "nombre_ejecutor, codigo_RFA, codigo_MONITOR, nombre_proyecto, fecha_inicio_proyecto, fecha_fin_proyecto, " &
                '    "id_usuario_ejecutor, a.id_usuario_responsable, usuario_responsable, " &
                '    "usuario_ejecutor, a.fecha_aprobacion, texto_alerta, id_mecanismo_contratacion, nnumero_sub_productos " &
                '    "order by nro_producto"
        End Select
        'Me.SqlDts_comentarios.SelectCommand = "select *, (select count(*) from tme_ficha_entregables z where z.id_ficha_producto = a.id_ficha_producto " & sqlFiltros & ") nro_sub_productos " &
        '    "from vw_productos a where id_ficha_proyecto = " & Me.lbl_id_ficha.Text & " order by nro_producto"
        Me.grd_aportes.DataBind()
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Proyectos/frm_sgtoHitos")
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_proyectosDocumentos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
End Class