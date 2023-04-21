Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_consulta_groups
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROl_GROUP_ADM"
    Dim controles As New ly_SIME.CORE.cls_controles



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try
        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            'Dim Sql = "SELECT nombre_proyecto FROM vw_proyectos WHERE id_proyecto=" & Me.Session("E_IdProy")
            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item(0).ToString
            'If Me.Session("E_IdPerfil") = 5 Or Me.Session("E_IdPerfil") = 1 Then



            'Me.grd_cate.Columns(0).Visible = True
            'Me.grd_cate.Columns(1).Visible = True
            'Me.btn_nuevo.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.grd_cate.Columns(1).Visible = False
            '    Me.btn_nuevo.Visible = False
            'End If
        End If


    End Sub


    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click


        Dim sql As String = String.Format("SELECT a.id_grupo, a.nombre_grupo, a.descripcion_grupo, a.id_programa, count(b.id_usuario) AS total_miembros  " &
                                          "   FROM ta_grupos a " &
                                          "     inner join ta_gruposRoles b on (a.id_grupo = b.id_grupo) " &
                                          "   WHERE a.id_programa = {1} AND ((a.nombre_grupo LIKE '%{2}%') OR (descripcion_grupo LIKE '%{3}%')) " &
                                          "   group by a.id_grupo, a.nombre_grupo, a.descripcion_grupo, a.id_programa", Me.Session("E_IDPrograma"), Me.txt_doc.Text.Trim, Me.txt_doc.Text.Trim)
        '"SELECT id_grupo, nombre_grupo, descripcion_grupo, id_proyecto, dbo.TotalMemberGroup(id_grupo) AS total_miembros  FROM ta_grupos WHERE id_proyecto=" & Me.Session("E_IdProy") & " "
        ' Me.SqlDataSource2.SelectCommand = sql
        ' Me.grd_cate.DataBind()

    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_grupo")
        'Me.SqlDataSource2.DeleteCommand = "DELETE FROM ta_grupos WHERE (id_grupo = " & id_temp & ")"
        'Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim editar As New ImageButton
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            'editar = CType(e.Item.FindControl("editar"), ImageButton)
            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton)
            editar.PostBackUrl = "frm_groups_edit.aspx?IdType=" & itemD("id_grupo").Text 'e.Item.Cells(4).Text.ToString
        End If
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Approvals/frm_groups.aspx")
    End Sub
End Class
