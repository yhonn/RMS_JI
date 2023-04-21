Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Partial Class frm_consulta_roles
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROLES_ADMIN"
    Dim controles As New ly_SIME.CORE.cls_controles

    Const CONST_TYPE_SIMPLE = 1
    Const CONST_TYPE_SHARED = 2

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

            'Dim Sql = "SELECT nombre_proyecto FROM vw_proyectos WHERE id_programa=" & Me.Session("E_IDPrograma")
            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item(0).ToString

            'Session("In-Out") = "IN"
            'Session.Remove("cl_RolUserSH_edit") 'Remove the session

            'If Me.Session("E_IdPerfil") = 5 Or Me.Session("E_IdPerfil") = 1 Then
            Me.grd_cate.Columns(2).Visible = True
            Me.btn_nuevo.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.btn_nuevo.Visible = False
            'End If


        End If


    End Sub


    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        Dim sql = "SELECT * FROM vw_ta_roles_emplead WHERE id_programa=" & Me.Session("E_IDPrograma") & " AND ((nombre_empleado LIKE '%" & Me.txt_doc.Text & "%') OR (descripcion_rol LIKE '%" & Me.txt_doc.Text & "%'))"
        Me.SqlDataSource2.SelectCommand = sql
        Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim editar As New ImageButton

            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton)

            If itemD("id_type_role").Text = CONST_TYPE_SIMPLE Then
                editar.PostBackUrl = "frm_roles_edit.aspx?IdType=" & itemD("id_rol").Text
            Else
                editar.PostBackUrl = "frm_rolesSH_edit.aspx?idR=" & itemD("id_rol").Text
            End If

        End If

    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click

        If cmb_type_role.SelectedValue = CONST_TYPE_SIMPLE Then
            Me.Response.Redirect("~/Approvals/frm_APProles.aspx")
        Else
            Me.Response.Redirect("~/Approvals/frm_rolesSH.aspx")
        End If

    End Sub
End Class
