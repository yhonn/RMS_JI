Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Globalization

Public Class frm_facturacion
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_FACT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim idUser As Integer = 0

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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
        If Not Me.IsPostBack Then
            'LoadLista()
            Dim newCulture As CultureInfo = New CultureInfo("es-ES")
            Me.grd_cate.Culture = New System.Globalization.CultureInfo("fr-FR", True)

            Dim Menu As GridFilterMenu = grd_cate.FilterMenu
            For Each item In Menu.Items
                'change the text for the StartsWith menu item
                If item.Text = "NoFilter" Then
                    item.Text = "Limpiar filtro"
                ElseIf item.Text = "Contains" Then
                    item.Text = "Contiene"
                ElseIf item.Text = "DoesNotContain" Then
                    item.Text = "No contiene"
                ElseIf item.Text = "StartsWith" Then
                    item.Text = "Empieza con"
                ElseIf item.Text = "EndsWith" Then
                    item.Text = "Finaliza con"
                ElseIf item.Text = "EqualTo" Then
                    item.Text = "Igual a"
                ElseIf item.Text = "NotEqualTo" Then
                    item.Text = "No igual a"
                ElseIf item.Text = "GreaterThan" Then
                    item.Text = "Mayor a"
                ElseIf item.Text = "LessThan" Then
                    item.Text = "Menor a"
                ElseIf item.Text = "GreaterThanOrEqualTo" Then
                    item.Text = "Mayor o igual a"
                ElseIf item.Text = "LessThanOrEqualTo" Then
                    item.Text = "Menor o igual a"
                ElseIf item.Text = "Between" Then
                    item.Text = "Entre"
                ElseIf item.Text = "NotBetween" Then
                    item.Text = "NoEntre"
                ElseIf item.Text = "IsEmpty" Then
                    item.Text = "Es vacío"
                ElseIf item.Text = "NotIsEmpty" Then
                    item.Text = "No es vacío"
                ElseIf item.Text = "IsNull" Then
                    item.Text = "Es nulo"
                ElseIf item.Text = "NotIsNull" Then
                    item.Text = "No es nulo"
                End If
            Next
            Me.A1.Attributes.Add("href", "~/reportes/xls?id=3&vt=" & Me.h_Filter.Value)
            fillGrid(True)

        End If
    End Sub
    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            'Dim facturas = dbEntities.vw_tme_facturacion.ToList()

            'Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            'Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            'Dim facturacion As List(Of vw_tme_facturacion) = New List(Of vw_tme_facturacion)

            'Select Case id_filtro_busqueda_viajes
            '    Case 1
            '        facturacion = dbEntities.vw_tme_facturacion.OrderByDescending(Function(p) p.id_facturacion).ToList()
            '    Case 2
            '        facturacion = dbEntities.vw_tme_facturacion.Where(Function(p) subR.Contains(p.id_sub_region)).OrderByDescending(Function(p) p.id_viaje).ToList()
            '    Case 3
            '        facturacion = dbEntities.vw_tme_facturacion.Where(Function(p) (p.id_usuario_app.Contains(id_User) And p.id_estadoDoc <> 7) Or
            '                                                                             (p.id_usuario_app_legalizacion.Contains(id_User) And p.id_estadoDoc_legalizacion <> 7) Or
            '                                                                             (p.id_usuario_app_informe.Contains(id_User) And p.id_estadoDoc_informe <> 7) Or
            '                                                                             (p.id_usuario_radica.Contains(id_User)) Or p.id_usuario = id_User Or p.id_supervisor = id_User).OrderByDescending(Function(p) p.id_viaje).ToList()
            'End Select



            Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = idUser).Select(Function(p) p.id_subregion).ToList()
            'Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            Dim id_User As Integer = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim solicitudesFact As List(Of vw_tme_facturacion) = New List(Of vw_tme_facturacion)



            Dim listarTodas = Convert.ToString(Me.h_Filter.Value)

            If listarTodas = "" Then
                Select Case id_filtro_busqueda_viajes
                    Case 1
                        solicitudesFact = dbEntities.vw_tme_facturacion.Where(Function(p) p.eliminada = False And p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_facturacion).ToList()
                    Case 2
                        solicitudesFact = dbEntities.vw_tme_facturacion.Where(Function(p) p.eliminada = False And subR.Contains(p.id_subregion) Or p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_facturacion).ToList()
                    Case 3
                        solicitudesFact = dbEntities.vw_tme_facturacion.Where(Function(p) p.eliminada = False And p.id_usuario_crea = id_User).OrderByDescending(Function(p) p.id_facturacion).ToList()

                End Select
            Else
                solicitudesFact = dbEntities.vw_tme_facturacion.Where(Function(p) p.eliminada = False).OrderByDescending(Function(p) p.id_facturacion).ToList()
            End If


            Me.grd_cate.DataSource = solicitudesFact

            If bndBind Then
                Me.grd_cate.DataBind()
            End If


        End Using
    End Sub

    'Protected Overrides Sub OnPreRenderComplete(e As EventArgs)
    '    Dim menu As RadContextMenu = grd_cate.HeaderContextMenu

    '    For Each item As RadMenuItem In menu.Items
    '        Select Case item.Value
    '            Case "SortAsc"
    '                item.Text = "Sort ascending"
    '                Exit Select
    '            Case "SortDesc"
    '                item.Text = "Sort descending"
    '                Exit Select
    '            Case "SortNone"
    '                item.Text = "Clear sorting"
    '                Exit Select
    '            Case "GroupBy"
    '                item.Text = "Group by"
    '                Exit Select
    '            Case "UnGroupBy"
    '                item.Text = "Ungroup"
    '                Exit Select
    '            Case "ColumnsContainer"
    '                item.Text = "Show/Hide columns"
    '                Exit Select
    '            Case "FilterMenuParent"
    '                'first condition label
    '                Dim lcShowRows As LiteralControl = TryCast(item.Controls(0), LiteralControl)
    '                lcShowRows.Text = "<label class=""rgHCMShow"">Show rows that:</label>"
    '                'first condition RadComboBox
    '                Dim firstConditionCombo As RadComboBox = TryCast(item.FindControl("HCFMRCMBFirstCond"), RadComboBox)
    '                firstConditionCombo.Text = "No filter is selected"
    '                'second condition label
    '                Dim lcAnd As LiteralControl = TryCast(item.Controls(3), LiteralControl)
    '                lcAnd.Text = "<label class=""rgHCMShow"">And also</label>"
    '                'second condition RadComboBox
    '                Dim secondConditionCombo As RadComboBox = TryCast(item.FindControl("HCFMRCMBSecondCond"), RadComboBox)
    '                secondConditionCombo.Text = "No filter is selected"
    '                'filter button  
    '                Dim btnFilter As Button = TryCast(item.FindControl("HCFMFilterButton"), Button)
    '                btnFilter.Text = "Apply filters"
    '                'clear filter button   
    '                Dim btnClearFilter As Button = TryCast(item.FindControl("HCFMClearFilterButton"), Button)
    '                btnClearFilter.Text = "Clear filter"
    '                Exit Select
    '        End Select
    '    Next
    '    MyBase.OnPreRenderComplete(e)
    'End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/financiero/frm_facturacionAD")
    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid(False)
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)


            Dim facturaAnulada = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "anulada"))

            Dim hlnkEstado As HyperLink = New HyperLink
            hlnkEstado = CType(e.Item.FindControl("col_hlk_revertir"), HyperLink)
            'hlnkEstado.NavigateUrl = "frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlnkDetalle As HyperLink = New HyperLink
            hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            hlnkDetalle.NavigateUrl = "frm_facturacionDetalle?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()

            Dim col_hlk_download As HyperLink = New HyperLink
            col_hlk_download = CType(e.Item.FindControl("col_hlk_download"), HyperLink)
            col_hlk_download.NavigateUrl = "frm_plantillaFactura?id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

            hlnkEdit.NavigateUrl = "frm_facturacionEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()
            'If facturaAnulada Then
            '    hlnkEdit.Visible = False
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/report_disabled.gif"
            '    hlnkEstado.ToolTip = "Factura anulada"
            'Else
            '    hlnkEstado.NavigateUrl = "frm_facturacionAnular?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()
            '    hlnkEdit.NavigateUrl = "frm_facturacionEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_facturacion").ToString()
            'End If

        End If


    End Sub
    Protected Sub delete_detalle(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idFactura = Convert.ToInt32(Me.identity.Value)
            Dim factura = dbEntities.tme_facturacion.Find(idFactura)

            factura.eliminada = True

            Dim facturasEliminada = New tme_facturas_eliminadas
            facturasEliminada.id_facturacion = idFactura
            facturasEliminada.codigo_factura = factura.numero_factura
            facturasEliminada.codigo_reasignado = False
            facturasEliminada.fecha_eliminado = DateTime.Now
            facturasEliminada.id_usuario_elimino = Convert.ToInt32(Me.Session("E_IdUser").ToString())

            dbEntities.tme_facturas_eliminadas.Add(facturasEliminada)
            dbEntities.SaveChanges()

            dbEntities.Entry(factura).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
            fillGrid(True)
            Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using

    End Sub
End Class