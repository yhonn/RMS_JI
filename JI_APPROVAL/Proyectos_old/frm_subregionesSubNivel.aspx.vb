Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class frm_subregionesSubNivel
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_SUBR_SUBN"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_error As New ly_SIME.CORE.ErrorHandler

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_catalogos)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            LoadData()
        End If


    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_HNEntities
                Dim oregiones As New tme_subregion_nivel_avance

                Dim nivel = dbEntities.t_nivel_avance.FirstOrDefault()

                oregiones.id_subregion = Me.lbl_id_subregion.Text
                oregiones.id_relacion = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
                oregiones.id_nivel_avance = nivel.id_nivel_avance

                dbEntities.tme_subregion_nivel_avance.Add(oregiones)
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_subregionesSubNivel?id=" & Me.lbl_id_subregion.Text
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    'Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
    '    Me.Response.Redirect("~/Proyectos/frm_subregiones")
    'End Sub

    Sub LoadData()
        Dim idP = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Me.cmb_programa.DataSource = clListados.get_t_programas(idP)
        Me.cmb_programa.DataTextField = "nombre_programa"
        Me.cmb_programa.DataValueField = "id_programa"
        Me.cmb_programa.DataBind()
        Me.cmb_programa.SelectedValue = idP
        Me.cmb_programa.Enabled = False
        Dim ID = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.lbl_id_subregion.Text = ID


        Using dbEntities As New dbRMS_HNEntities
            Dim subregion = dbEntities.t_subregiones.Find(ID)
            Me.lbl_subtitulo_aux.Text = " - " & subregion.nombre_subregion
            Dim nivel = dbEntities.t_nivel_avance.FirstOrDefault(Function(p) p.id_programa = idP)
            Dim sql = "SELECT " & nivel.campo_nombre_tabla & ", " & nivel.campo_id_tabla & " FROM " & nivel.nombre_tabla & " WHERE " _
                      & nivel.campo_id_tabla & " NOT IN (SELECT id_relacion FROM tme_subregion_nivel_avance WHERE id_subregion = " & Me.lbl_id_subregion.Text & ") ORDER BY " & nivel.campo_nombre_tabla

            Me.SqlDataSource1.SelectCommand = sql


            sql = "SELECT " & nivel.campo_nombre_tabla & " as Nombre, " & nivel.campo_id_tabla & " as ID FROM " & nivel.nombre_tabla & " WHERE " _
                      & nivel.campo_id_tabla & " IN (SELECT id_relacion FROM tme_subregion_nivel_avance WHERE id_subregion = " & Me.lbl_id_subregion.Text & ")"

            Me.SqlDataSource2.SelectCommand = sql

            Me.grd_catalogos.DataBind()
            Me.cmb_departamento.DataValueField = nivel.campo_id_tabla
            Me.cmb_departamento.DataTextField = nivel.campo_nombre_tabla
            Me.cmb_departamento.DataBind()
        End Using

    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_catalogos.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_subregion").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "ID").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "ID").ToString())
            'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")
        End If
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM tme_subregion_nivel_avance WHERE id_relacion = " + Me.identity.Text & " AND id_subregion =" + Me.lbl_id_subregion.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
                Me.MsgGuardar.Redireccion = "~/Proyectos/frm_subregionesSubNivel?id=" + Me.lbl_id_subregion.Text
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