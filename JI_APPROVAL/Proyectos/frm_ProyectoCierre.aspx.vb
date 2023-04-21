Imports ly_SIME

Public Class frm_ProyectoCierre
    Inherits System.Web.UI.Page

    Dim cl_user As New ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "PROY_CIERRE"
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
            LoadData()
        End If
    End Sub

    Sub LoadData()
        Using dbEntities As New dbRMS_HNEntities
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id"))
            Dim oProyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)

            Me.lbl_id_proyecto.Text = id
            Me.lbl_codigo_ficha.Text = oProyecto.codigo_SAPME
            Me.lbl_nombre_ficha.Text = oProyecto.nombre_proyecto
            Me.lbl_nombre_ejecutor.Text = oProyecto.nombre_ejecutor
            Me.lbl_estado.Text = oProyecto.nombre_estado_ficha
            Me.lbl_nombre_subregion.Text = oProyecto.nombre_subregion
            Me.lbl_fecha_actualizacion.Text = Date.UtcNow
        End Using
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_HNEntities
            Dim id = Convert.ToInt32(Me.lbl_id_proyecto.Text)
            Dim oFicha = dbEntities.tme_Ficha_Proyecto.Find(id)
            oFicha.id_ficha_estado = Me.rbcierre.SelectedValue
            dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
        End Using

        Me.MsgGuardar.NuevoMensaje = "Guardado Correctamente"
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosCierre"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
End Class