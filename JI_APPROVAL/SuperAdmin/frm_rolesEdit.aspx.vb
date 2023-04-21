﻿Imports ly_SIME

Public Class frm_rolesEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_EDIT_ROL"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListado As New ly_SIME.CORE.cls_listados

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
                cl_user.chk_Rights(Page.Controls, 8, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Me.lbl_id_rol.Text = Me.Request.QueryString("Id").ToString
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = clListado.get_t_programas()
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.SelectedValue = id
            FillData(Me.lbl_id_rol.Text)
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim id_rol = Convert.ToInt32(Me.lbl_id_rol.Text)
                Dim oRol = dbEntities.t_rol.Find(id_rol)

                oRol.rol_name = Me.txt_nombreRol.Text
                oRol.rol_desc = Me.txt_descripcion.Text

                dbEntities.Entry(oRol).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/SuperAdmin/frm_roles"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Sub FillData(ByVal Idprograma As String)

        Dim id_rol = Convert.ToInt32(Idprograma)
        Using dbEntities As New dbRMS_JIEntities
            Dim oRol = dbEntities.t_rol.Find(id_rol)

            Me.txt_nombreRol.Text = oRol.rol_name
            Me.txt_descripcion.Text = oRol.rol_desc
            Me.cmb_programa.SelectedValue = oRol.id_programa
        End Using
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/SuperAdmin/frm_roles"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
End Class