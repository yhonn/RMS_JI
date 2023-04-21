Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_Activities_tmp
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_ACTIVITY"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            LoadLista()
            fillGrid(True)
        End If
    End Sub

    Sub fillGrid(ByVal bndBind As Boolean)


        Using dbentities As New dbRMS_JIEntities

            Dim caso = 3

            Dim bndRegion As Integer = 0
            Dim idRegion As Integer = 0
            Dim bndSubRegion As Integer = 0
            Dim idSubRegion As Integer = 0


            If chk_Todos.Checked Then 'SubRegion All
                bndSubRegion = 1
                idSubRegion = 0
                'caso = 1
            Else
                bndSubRegion = 0
                idSubRegion = Val(Me.cmb_subregion.SelectedValue)
            End If

            If chk_TodosR.Checked Then 'Region All
                'caso = 2
                bndRegion = 1
                idRegion = 0
            Else
                bndRegion = 0
                idRegion = Val(Me.cmb_region.SelectedValue)
            End If

            Dim bndMecanism As Integer = 0
            Dim idMecanism As Integer = 0
            Dim bndSubMecanism As Integer = 0
            Dim idSubMecanism As Integer = 0

            If Me.chk_allMecanism.Checked Then
                bndMecanism = 1
                idMecanism = 0
            Else
                bndMecanism = 0
                idMecanism = Val(Me.cmb_mecanism.SelectedValue)
            End If

            If Me.chk_allSubmecanims.Checked Then
                bndSubMecanism = 1
                idSubMecanism = 0
            Else
                bndSubMecanism = 0
                idSubMecanism = Val(Me.cmb_Sub_mecanism.SelectedValue)
            End If

            Dim id_programa = Me.Session("E_IDPrograma").ToString()
            Dim idProyectos As Integer() = get_Proyectos()

            'Or p.nombre_ejecutor.Contains(Me.txt_doc.Text)
            '   (p.id_subregion.Contains(Me.cmb_subregion.SelectedValue) Or 1 = bndSubRegion) And
            '  (p.id_region.Contains(Me.cmb_region.SelectedValue) Or 1 = bndRegion) And
            '  (idProyectos.Contains(p.id_ficha_proyecto))


            Me.grd_cate.DataSource = dbentities.VW_TA_AWARDED_APP.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
                                                                                                 p.codigo_SAPME.Contains(Me.txt_doc.Text) Or
                                                                                                 p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or
                                                                                                 p.NAMEALIAS.Contains(Me.txt_doc.Text) Or
                                                                                                 p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
                                                                                                 p.codigo_ficha_AID.Contains(Me.txt_doc.Text) Or
                                                                                                 p.codigo_RFA.Contains(Me.txt_doc.Text) Or
                                                                                                 p.codigo_MONITOR.Contains(Me.txt_doc.Text)) And
                                                                                                 p.id_programa = id_programa And
                                                                                                 (p.id_mecanismo_contratacion = idMecanism Or 1 = bndMecanism) And
                                                                                                 (p.id_sub_mecanismo = idSubMecanism Or 1 = bndSubMecanism)).ToList()

            'Select Case caso
            '    'Todas las subregiones de una region

            '    Case 1
            '        'Dim regions = dbentities.t_regiones.Where(Function (p) p.id_programa == )
            '        Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto _
            '            .Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
            '                                p.codigo_SAPME.Contains(Me.txt_doc.Text) Or
            '                                p.codigo_ficha_AID.Contains(Me.txt_doc.Text) Or
            '                                p.codigo_MONITOR.Contains(Me.txt_doc.Text) Or
            '                                p.nombre_ejecutor.Contains(Me.txt_doc.Text)) And (p.id_region.Contains(Me.cmb_region.SelectedValue) And idProyectos.Contains(p.id_ficha_proyecto))).ToList()
            '        'Todos las regiones
            '    Case 2
            '        Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_SAPME.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_ficha_AID.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_MONITOR.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.nombre_ejecutor.Contains(Me.txt_doc.Text)) And p.id_programa.Contains(id_programa) And idProyectos.Contains(p.id_ficha_proyecto)).ToList()
            '        'SubRegión especifica
            '    Case 3
            '        Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_SAPME.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_ficha_AID.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.codigo_MONITOR.Contains(Me.txt_doc.Text) Or
            '                                                                                     p.nombre_ejecutor.Contains(Me.txt_doc.Text)) And
            '                                                                                     (p.id_mecanismo_contratacion = idMecanism Or 1 = bndMecanism) And
            '                                                                                     (p.id_sub_mecanismo = idSubMecanism Or 1 = bndSubMecanism) And

            '                                                                                     (p.id_subregion.Contains(Me.cmb_subregion.SelectedValue) And idProyectos.Contains(p.id_ficha_proyecto))).ToList()
            'End Select

            If bndBind Then
                Me.grd_cate.DataBind()
            End If

        End Using

        Me.lbltotal.Text = "Total rows: [ " & Me.grd_cate.Items.Count & " ]"


    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            ''hlnkPrint.ToolTip = controles.iconosGrid("col_hlk_Print")

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_ActivityAW?Id=" & DataBinder.Eval(e.Item.DataItem, "ID_ACTIVITY").ToString() & "&Id_AW=" & DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_ACTIVITY").ToString()
            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            'Dim hlnkCompleto As HyperLink = New HyperLink
            'hlnkCompleto = CType(e.Item.FindControl("col_hlk_Completo"), HyperLink)
            ''hlnkCompleto.NavigateUrl = "frm_proyectosEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            ''hlnkCompleto.ToolTip = controles.iconosGrid("col_hlk_completo")
            'hlnkCompleto.ToolTip = "Incomplete"

            'Dim hlnkDelete As LinkButton = New LinkButton
            'hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString())
            'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString())
            ''hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

            'Dim hlnk_OrdenInicioCMP As HyperLink = New HyperLink
            'hlnk_OrdenInicioCMP = CType(e.Item.FindControl("col_hlk_OrdenInicioCMP"), HyperLink)

            'If DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("1") Then
            '    hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
            '    hlnk_OrdenInicioCMP.ToolTip = "Pendiente de aprobación"
            '    hlnk_OrdenInicioCMP.NavigateUrl = "frm_proyectoOrdInicioAD?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            'ElseIf DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("3") Then
            '    hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
            '    hlnk_OrdenInicioCMP.ToolTip = "Ficha Cancelada"
            'Else
            '    hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/hmenu-lock.png"
            '    hlnk_OrdenInicioCMP.ToolTip = "Ya se generó la orden de inicio"
            '    hlnk_OrdenInicioCMP.NavigateUrl = "frm_proyectoMenuEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            'End If

            Dim lbl_rms_code As Label = CType(e.Item.Cells(0).FindControl("lbl_rms_code"), Label)
            lbl_rms_code.Text = DataBinder.Eval(e.Item.DataItem, "codigo_SAPME")
            Dim lbl_control_code As Label = CType(e.Item.Cells(0).FindControl("lbl_internal_code"), Label)
            lbl_control_code.Text = DataBinder.Eval(e.Item.DataItem, "codigo_RFA")
            Dim lbl_monitor_code As Label = CType(e.Item.Cells(0).FindControl("lbl_monitor_code"), Label)
            lbl_monitor_code.Text = DataBinder.Eval(e.Item.DataItem, "codigo_MONITOR")


        End If


    End Sub


    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/RFP_/frm_ActivityAD_")
    End Sub

    'Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
    '    Using db As New dbRMS_JIEntities
    '        Try
    '            'db.Database.ExecuteSqlCommand("DELETE FROM t_usuarios WHERE id_usuario = " + Me.identity.Text)
    '            Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
    '        Catch ex As SqlException
    '            Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
    '            Me.MsgGuardar.TituMensaje = "Error al eliminar"
    '        End Try
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '    End Using
    'End Sub

    'Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
    '    Dim a = CType(sender, LinkButton)
    '    Me.identity.Text = a.Attributes.Item("data-identity").ToString()
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    'End Sub

    Sub LoadLista()

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

        Me.cmb_region.DataSourceID = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(idPrograma)
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataBind()
        Me.cmb_region.SelectedIndex = 1

        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        Me.cmb_subregion.SelectedIndex = 1

        'Me.cmb_subregion.DataSourceID = ""
        'Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        'Me.cmb_subregion.DataTextField = "nombre_subregion"
        'Me.cmb_subregion.DataValueField = "id_subregion"
        'Me.cmb_subregion.DataBind()

        Using dbEntities As New dbRMS_JIEntities

            Me.cmb_mecanism.DataSourceID = ""
            Me.cmb_mecanism.DataSource = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = idPrograma).ToList()
            Me.cmb_mecanism.DataTextField = "nombre_mecanismo_contratacion"
            Me.cmb_mecanism.DataValueField = "id_mecanismo_contratacion"
            Me.cmb_mecanism.DataBind()
            Me.cmb_mecanism.SelectedIndex = 1

            Me.cmb_Sub_mecanism.DataSourceID = ""
            Me.cmb_Sub_mecanism.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanism.SelectedValue).ToList()
            Me.cmb_Sub_mecanism.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_Sub_mecanism.DataValueField = "id_sub_mecanismo"
            Me.cmb_Sub_mecanism.DataBind()

        End Using

        Me.chk_Todos.Checked = True
        Me.chk_TodosR.Checked = True
        Me.chk_allMecanism.Checked = True
        Me.chk_allSubmecanims.Checked = True

    End Sub


    Public Function get_Proyectos() As Integer()
        Using dbEntities As New dbRMS_JIEntities

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim subregiones = dbEntities.vw_t_usuarios_subregiones.Where(Function(p) p.id_programa = idPrograma And p.id_usuario = cl_user.id_usr).ToList()
            Dim idSubRegiones As List(Of String) = New List(Of String)
            Dim idProyectos As List(Of Integer) = New List(Of Integer)

            If subregiones.Count() > 0 Then
                For Each item In subregiones
                    idSubRegiones.Add(item.id_subregion)
                Next
            Else
                idSubRegiones.Add(-1)
            End If

            Dim arrSubregiones As String() = idSubRegiones.ToArray()

            Dim oProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_programa.Contains(idPrograma)).ToList()

            For Each item In oProyecto

                Dim A As String() = item.id_region.Split(",")
                Dim bFind As Boolean = False

                For Each i As Integer In A

                    For Each b As Integer In arrSubregiones
                        If i = b Then
                            bFind = True
                            Exit For
                        End If
                    Next

                    If bFind Then
                        Exit For
                    End If
                    ''If idSubRegiones.Contains(i) Then
                    'End If
                Next


                If bFind Then
                    idProyectos.Add(item.id_ficha_proyecto)
                End If

            Next

            get_Proyectos = idProyectos.ToArray()

        End Using
    End Function

    Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
        fillGrid(True)
    End Sub

    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        fillGrid(True)
    End Sub

    Protected Sub Chk_TodosR_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosR.CheckedChanged
        fillGrid(True)
    End Sub

    Protected Sub Chk_Todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Todos.CheckedChanged
        fillGrid(True)
    End Sub

    'Protected Sub btn_nuevo0_Click(sender As Object, e As EventArgs) Handles btn_nuevo0.Click
    '    Me.Response.Redirect("~/proyectos/frm_proyectosCuadroMando")
    'End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid(False)
    End Sub

    Protected Sub btn_buscar_Click(sender As Object, e As EventArgs) Handles btn_buscar.Click
        fillGrid(True)
    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub

    Private Sub cmb_mecanism_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanism.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If Val(e.Value) > 0 Then
                Me.cmb_Sub_mecanism.DataSourceID = ""
                Me.cmb_Sub_mecanism.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanism.SelectedValue).ToList()
                Me.cmb_Sub_mecanism.DataTextField = "nombre_sub_mecanismo"
                Me.cmb_Sub_mecanism.DataValueField = "id_sub_mecanismo"
                Me.cmb_Sub_mecanism.DataBind()
                fillGrid(True)
            End If

        End Using


    End Sub
End Class