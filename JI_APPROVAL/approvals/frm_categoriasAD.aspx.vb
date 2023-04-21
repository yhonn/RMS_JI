Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Partial Class frm_categoriasAD
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_TYPE_APP_ADD"
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
            Me.grd_cate.Columns(0).Visible = True
            Me.btn_save.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.btn_save.Visible = False
            'End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim Sql = " INSERT INTO ta_categoria (descripcion_cat, id_programa) VALUES ('" & Me.txt_cat.Text & "', " & Me.Session("E_IDPrograma") & ") "
        cnnSAP.Open()
        Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        Dim ds As New DataSet("IdPlan")
        dm.Fill(ds, "IdPlan")
        cnnSAP.Close()
        Me.grd_cate.DataBind()
        Me.txt_cat.Text = ""
    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ActualizaDatos()

    End Sub

    Sub ActualizaDatos()

        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each Irow As GridDataItem In Me.grd_cate.Items

            'Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkVisible"), CheckBox)
            Dim chkvisible As CheckBox = CType(Irow("colm_visible").FindControl("chkVisible"), CheckBox)

            If chkvisible.Checked = True Then
                visibleSI &= "," & Irow("id_categoria").Text
            Else
                visibleNO &= "," & Irow("id_categoria").Text 'row.Cells.Item(3).Text
            End If
        Next

        cnnSAP.Open()
        Dim dm As New SqlCommand("UPDATE ta_categoria SET visible='SI' WHERE id_categoria IN(" & visibleSI & ")", cnnSAP)
        dm.ExecuteNonQuery()
        dm.CommandText = "UPDATE ta_categoria SET visible='NO' WHERE id_categoria IN(" & visibleNO & ")"
        dm.ExecuteNonQuery()
        cnnSAP.Close()

        Me.grd_cate.DataBind()

    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim visible As New CheckBox
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'visible = CType(e.Item.FindControl("chkVisible"), CheckBox)
            visible = itemD("colm_visible").FindControl("chkVisible")
            'If e.Item.Cells(6).Text.ToString = "SI" Then
            If itemD("Cvisible").Text = "SI" Then
                visible.ToolTip = "Visible"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Hidden"
            End If
            'If Me.Session("E_IdPerfil") <> 5 And Me.Session("E_IdPerfil") <> 1 Then
            ' visible.Enabled = False
            'End If
        End If
    End Sub

End Class
