Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class frm_tasas_cambio
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_TACA"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next

            'Me.btn_eliminar.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            fillGrid()
        End If
    End Sub
    Sub fillGrid()
        Dim db As New dbRMS_JIEntities
        Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = db.vw_trimestre_tasa_cambio.Where(Function(p) p.id_programa = id_programa).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Protected Sub btn_nuevo_Click(sender As Object, e As EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Administracion/frm_tasa_cambioAD")
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkEdit As LinkButton = New LinkButton
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), LinkButton)
            hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_trimestre_tasa_cambio").ToString())
            'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")
        End If
    End Sub
    Protected Sub Editar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        Dim idTC = Me.identity.Text
        Dim db As New dbRMS_JIEntities
        Me.txt_tasa_cambio.Value = db.t_trimestre_tasa_cambio.Find(Convert.ToInt32(idTC)).tasa_cambio
        Me.id_tasa_cambio.Value = idTC
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadEditModal()", True)
    End Sub
    Protected Sub Elimina_Elemento(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)

    End Sub
    Protected Sub btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Try
                Dim Sql = "delete from t_trimestre_tasa_cambio WHERE id_trimestre_tasa_cambio = " & Me.identity.Text
                dbEntities.Database.ExecuteSqlCommand(Sql)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_tasas_cambio"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub
    Protected Sub btn_edit2_Click(sender As Object, e As EventArgs) Handles btn_edit2.Click
        Dim db As New dbRMS_JIEntities
        Dim oTasaCambio = db.t_trimestre_tasa_cambio.Find(Convert.ToInt32(Me.id_tasa_cambio.Value))
        oTasaCambio.tasa_cambio = Me.txt_tasa_cambio.Value
        oTasaCambio.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
        oTasaCambio.fecha_upd = Date.Now
        db.Entry(oTasaCambio).State = Entity.EntityState.Modified
        db.SaveChanges()


        Dim trimestre = db.t_trimestre.Find(oTasaCambio.id_trimestre)
        trimestre.tasa_cambio = trimestre.t_trimestre_tasa_cambio.Average(Function(p) p.tasa_cambio)
        db.Entry(trimestre).State = Entity.EntityState.Modified
        db.SaveChanges()


        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Administracion/frm_tasas_cambio"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
End Class