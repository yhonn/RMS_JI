﻿Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient

Public Class frm_subregiones
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_SUBR"
    Dim db As New dbRMS_HNEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()

        Me.grd_cate.DataSourceID = ""
        If String.IsNullOrWhiteSpace(Me.Request.QueryString("Id")) Then
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Me.grd_cate.DataSource = db.vw_t_subregiones.Where(Function(p) p.nombre_subregion.Contains(Me.txt_doc.Text) _
                                                                And p.id_programa = id).ToList()
        Else
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
            Me.grd_cate.DataSource = db.t_subregiones.Where(Function(p) p.id_region = id And p.nombre_subregion.Contains(Me.txt_doc.Text)).ToList()
            Dim region = db.t_regiones.Find(id)
            Me.lbl_subtitulo_aux.Text = " - " & region.nombre_region
        End If
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"
        Me.grd_cate.DataBind()

        Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
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
            fillGrid()
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_subRegionesEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("lnk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString())

            Dim hlnkSubNivel As HyperLink = New HyperLink
            hlnkSubNivel = CType(e.Item.FindControl("hlk_subNivel"), HyperLink)
            hlnkSubNivel.NavigateUrl = "frm_subregionesSubNivel?Id=" & DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString()

            'hlnkDelete.NavigateUrl = "frm_regionesEdit.aspx?Id=" & DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString()

        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/proyectos/frm_subRegionesAD")
    End Sub
    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_subregion = Convert.ToInt32(TryCast(e.Item, GridDataItem).GetDataKeyValue("id_subregion").ToString())
        Using dbRMS As New dbRMS_HNEntities
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_subregiones"
            Try
                Dim oregion = dbRMS.t_subregiones.Find(id_subregion)
                dbRMS.t_subregiones.Remove(oregion)
                dbRMS.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Catch ex As DbUpdateException
                Me.MsgGuardar.NuevoMensaje = "Existen registros asociados a la región."
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM t_subregiones WHERE id_subregion = " + Me.identity.Text)
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

End Class