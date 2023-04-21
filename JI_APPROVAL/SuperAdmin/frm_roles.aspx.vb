Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports ly_SIME

Public Class frm_roles
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_ROL"
    Dim db As New dbRMS_JIEntities
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Me.grd_cate.DataSource = db.t_rol.Where(Function(p) p.rol_name.Contains(Me.txt_doc.Text) And p.id_rol > 1 And p.id_programa = Me.cmb_programa.SelectedValue).ToList()
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
                cl_user.chk_Rights(Page.Controls, 8, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = clListado.get_t_programas()
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.SelectedValue = id
            fillGrid()
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_rolesEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_rol").ToString()
            hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            Dim hlnkAcceso_mod As HyperLink = New HyperLink
            hlnkAcceso_mod = CType(e.Item.FindControl("col_hlk_acceso_mod"), HyperLink)
            hlnkAcceso_mod.NavigateUrl = "frm_acceso_mod?Id=" & DataBinder.Eval(e.Item.DataItem, "id_rol").ToString()
            hlnkAcceso_mod.ToolTip = controles.iconosGrid("col_hlk_acceso_mod")

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_rol").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_rol").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")
        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/SuperAdmin/frm_rolesAD")
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_JIEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM t_roles WHERE id_rol = " + Me.identity.Text)
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

    Protected Sub cmb_programa_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        fillGrid()
    End Sub
End Class