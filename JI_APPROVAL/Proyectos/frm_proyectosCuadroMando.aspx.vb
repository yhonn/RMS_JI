Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient

Public Class frm_proyectosCuadroMando
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "PRO_CMANDO"
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
            fillGrid(3)
        End If
    End Sub

    Sub fillGrid(ByVal caso As Integer)
        Using dbentities As New dbRMS_HNEntities
            caso = 3

            If Chk_Todos.Checked Then
                caso = 1
            End If
            If Chk_TodosR.Checked Then
                caso = 2
            End If
            Select Case caso
                'Todas las subregiones de una region
                Case 1
                    Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
                                                                                         p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_region.Contains(Me.cmb_region.SelectedValue)).ToList()

                    'Todos las regiones
                Case 2
                    Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
                                                                                         p.codigo_SAPME.Contains(Me.txt_doc.Text))).ToList()

                    'SubRegión especifica
                Case 3
                    Me.grd_cate.DataSource = dbentities.vw_tme_ficha_proyecto.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or _
                                                                                         p.codigo_SAPME.Contains(Me.txt_doc.Text)) And p.id_subregion.Contains(Me.cmb_subregion.SelectedValue)).ToList()
            End Select


            Me.grd_cate.DataBind()
        End Using

        Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("col_hlk_print"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            hlnkPrint.ToolTip = controles.iconosGrid("col_hlk_Print")

            Dim hlnkImagenes As HyperLink = New HyperLink
            hlnkImagenes = CType(e.Item.FindControl("col_hlk_imagenes"), HyperLink)
            hlnkImagenes.NavigateUrl = "~/Proyectos/frm_proyectoPrintInfoAttachImg?IdPostURL=2&Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            hlnkImagenes.ToolTip = controles.iconosGrid("col_hlk_imagenes")

            Dim hlnkGeoreferenciaCMP As HyperLink = New HyperLink
            hlnkGeoreferenciaCMP = CType(e.Item.FindControl("col_hlk_GeoreferenciaCMP"), HyperLink)
            hlnkGeoreferenciaCMP.ToolTip = controles.iconosGrid("col_hlk_georeferenciaCompleta")
            If DataBinder.Eval(e.Item.DataItem, "georeferencia_completa").ToString().Equals("NO") Then
                hlnkGeoreferenciaCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
                hlnkGeoreferenciaCMP.ToolTip = controles.iconosGrid("col_hlk_georeferenciaIncompleta")
            End If
            hlnkGeoreferenciaCMP.NavigateUrl = "frm_proyectoGeorefereciaAD?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlnkRecursosCMP As HyperLink = New HyperLink
            hlnkRecursosCMP = CType(e.Item.FindControl("col_hlk_RecursosCMP"), HyperLink)
            hlnkRecursosCMP.ToolTip = controles.iconosGrid("col_hlk_recursosAsignados")
            If DataBinder.Eval(e.Item.DataItem, "aportes_actualizados").ToString().Equals("NO") Then
                hlnkRecursosCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
                hlnkRecursosCMP.ToolTip = controles.iconosGrid("col_hlk_recursosAsignadosIncom")
            End If
            hlnkRecursosCMP.NavigateUrl = "frm_proyectoAportesAD?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()


            '*************HABILITO ORDEN INICIO AGAIN******************************
            Dim hlnk_OrdenInicioCMP As HyperLink = New HyperLink
            hlnk_OrdenInicioCMP = CType(e.Item.FindControl("col_hlk_OrdenInicioCMP"), HyperLink)
            If DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("1") Then
                hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
                hlnk_OrdenInicioCMP.ToolTip = "Pendiente de aprobación"
                hlnk_OrdenInicioCMP.NavigateUrl = "frm_proyectoOrdInicioAD?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            ElseIf DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("3") Then
                hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
                hlnk_OrdenInicioCMP.ToolTip = "Ficha Cancelada"
            Else
                hlnk_OrdenInicioCMP.ImageUrl = "~/Imagenes/iconos/hmenu-lock.png"
                hlnk_OrdenInicioCMP.ToolTip = "Ya se generó la orden de inicio"
            End If
            '*************HABILITO ORDEN INICIO AGAIN******************************

            Dim EstadoCMP As HyperLink = New HyperLink
            EstadoCMP = CType(e.Item.FindControl("hlk_EstadoCMP"), HyperLink)
            If DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("1") Then
                EstadoCMP.NavigateUrl = "frm_proyectoOrdInicioAD?Id" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
            End If
            EstadoCMP.ImageUrl = "~/Imagenes/iconos/" & DataBinder.Eval(e.Item.DataItem, "bandera_estado").ToString()
            EstadoCMP.ToolTip = DataBinder.Eval(e.Item.DataItem, "nombre_estado_ficha").ToString()

            Dim MenuCMP As HyperLink = New HyperLink
            MenuCMP = CType(e.Item.FindControl("hlk_MenuCMP"), HyperLink)
            Dim PrintMGR As HyperLink = New HyperLink
            PrintMGR = CType(e.Item.FindControl("hlk_PrintMGR"), HyperLink)

            If DataBinder.Eval(e.Item.DataItem, "id_ficha_estado").ToString().Equals("2") Then
                MenuCMP.NavigateUrl = "frm_proyectoMenuEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
                PrintMGR.NavigateUrl = "frm_ProyectoAvanceMetasPrintInfo?Id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
                PrintMGR.Visible = False
            Else
                MenuCMP.Visible = False
                PrintMGR.Visible = False
            End If
        End If
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New dbRMS_HNEntities
            Try
                'db.Database.ExecuteSqlCommand("DELETE FROM t_usuarios WHERE id_usuario = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Sub LoadLista()
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Me.cmb_region.DataSourceID = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(idPrograma)
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataBind()

        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
    End Sub

    Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
        fillGrid(3)
    End Sub

    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        fillGrid(3)
    End Sub

    Protected Sub Chk_TodosR_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_TodosR.CheckedChanged
        fillGrid(3)
    End Sub

    Protected Sub Chk_Todos_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_Todos.CheckedChanged
        fillGrid(1)
    End Sub

End Class