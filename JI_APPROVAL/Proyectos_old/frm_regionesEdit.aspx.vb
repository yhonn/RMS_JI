Imports ly_SIME

Public Class frm_regionesEdit
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_EDIT_REGI"
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Me.lbl_id_region.Text = Me.Request.QueryString("Id").ToString
            FillData(Me.lbl_id_region.Text)
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_HNEntities
                Dim id_region = Convert.ToInt32(Me.lbl_id_region.Text)
                Dim oregion = dbEntities.t_regiones.Find(id_region)

                oregion.nombre_region = Me.txt_nombreRegion.Text
                oregion.id_programa = Me.cmb_programa.SelectedValue

                dbEntities.Entry(oregion).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_regiones"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Sub FillData(ByVal IdRegion As String)

        Using dbEntities As New dbRMS_HNEntities
            Dim id_region = Convert.ToInt32(IdRegion)
            Dim oregion = dbEntities.t_regiones.Find(id_region)

            Me.txt_nombreRegion.Text = oregion.nombre_region
            Me.cmb_programa.SelectedValue = oregion.id_programa
        End Using
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Proyectos/frm_regiones"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

End Class