Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL


Partial Class frm_groups
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROl_GROUP_ADD"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cls_groupRoles As APPROVAL.clss_GroupRoles

    Sub ActualizaDatos()

        Dim sql = ""
        cnnSAP.Open()
        Dim app As String = "NO"
        Dim comment As String = "NO"
        Dim Query As String = "NO"


        For Each IDrow As GridDataItem In Me.grd_cate.Items

            Dim chk_app As CheckBox = CType(IDrow("approval").FindControl("chk_app"), CheckBox)   'CType(row.Cells(i).FindControl("chk_app"), CheckBox)
            Dim chk_comment As CheckBox = CType(IDrow("approval").FindControl("chk_comment"), CheckBox) 'CType(row.Cells(i).FindControl("chk_comment"), CheckBox)
            Dim chk_Query As CheckBox = CType(IDrow("approval").FindControl("chk_Query"), CheckBox) 'CType(row.Cells(i).FindControl("chk_comment"), CheckBox)

            If chk_app.Checked = True Then
                app = "SI"
            Else
                app = "NO"
            End If
            If chk_comment.Checked = True Then
                comment = "SI"
            Else
                comment = "NO"
            End If

            If chk_Query.Checked = True Then
                Query = "SI"
            Else
                Query = "NO"
            End If

            sql = "UPDATE ta_gruposRoles_temp SET aprueba='" & app.ToString & "', comenta='" & comment.ToString & "', consulta='" & Query.ToString & " ' WHERE id=" & IDrow("id").Text 'row.Cells(7).Text

            Dim dm As New SqlDataAdapter(sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")


        Next
        Me.grd_cate.DataBind()
        cnnSAP.Close()
    End Sub
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
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            cls_groupRoles = New APPROVAL.clss_GroupRoles(Me.Session("E_IDPrograma"))
            cmb_rol.DataSource = cls_groupRoles.Query_Roles()
            Me.cmb_rol.DataBind()
            cmb_rol.SelectedIndex = -1

            'Dim Sql = "SELECT nombre_empleado, id_usuario from vw_ta_roles_emplead where id_rol=" & Me.cmb_rol.SelectedValue
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("IdPlan")
            'dm.Fill(ds, "IdPlan")
            'Me.lblEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item(0)
            'Me.lbl_IdEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item(1)

            Me.lblEmpleado.Text = "N/A"
            Me.lbl_IdEmpleado.Text = 0


            Try
                Dim rnd As New Random()
                Dim fecha As DateTime = Date.Now
                Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
                Me.lbl_idTemp.Text = rnd.Next(1, 999).ToString & textfecha.ToString
            Catch ex As Exception
                Me.lbl_idTemp.Text = "-1"
            End Try

            Me.cmb_usu.DataBind()
            Me.cmb_usu.SelectedIndex = -1
            Me.cmb_usu.Text = ""

        Else

            If Me.grd_cate.Items.Count() > 0 Then
                Me.cmb_rol.Enabled = False
            Else
                Me.cmb_rol.Enabled = True
            End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""
        If err = False Then
            'Me.Label3.Text = ext.ToString
            Dim Sql = "INSERT INTO ta_grupos(nombre_grupo, descripcion_grupo, id_programa) VALUES ('" & Me.txt_grupo.Text.Trim & "', '" & Me.txt_des.Text.Trim & "', " & Me.Session("E_IDPrograma") & ") "
            Sql &= " SELECT @@IDENTITY"
            cnnSAP.Open()
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")
            Dim id_grupo = ds.Tables("IdPlan").Rows(0).Item(0)

            '*****************************GUARDANDO LOS DATOS EN ta_gruposRoles el detalle de miembros del grupo****************************
            Sql = " INSERT INTO ta_gruposRoles SELECT " & id_grupo & " AS id_grupo, id_rol, id_usuario, aprueba, comenta, consulta FROM ta_gruposRoles_temp WHERE id_temp='" & Me.lbl_idTemp.Text & "'"
            dm.SelectCommand.CommandText = Sql
            dm.SelectCommand.ExecuteNonQuery()

            cnnSAP.Close()
            Me.Response.Redirect("~/Approvals/frm_consulta_groups.aspx")
        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_groups.aspx")
    End Sub

    Protected Sub cmb_rol_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_rol.SelectedIndexChanged

        If Val(cmb_rol.SelectedValue) > 0 Then



            Dim Sql = "SELECT nombre_empleado, id_usuario from vw_ta_roles_emplead where id_rol=" & Me.cmb_rol.SelectedValue
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")
            Me.lblEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item(0)
            Me.lbl_IdEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item(1)
            Me.cmb_usu.DataBind()
            Me.cmb_usu.SelectedIndex = -1
            Me.cmb_usu.Text = ""

        End If

    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id")
        Me.SqlDataSource3.DeleteCommand = "DELETE FROM ta_gruposRoles_temp WHERE (id = " & id_temp & ")"
        Me.grd_cate.DataBind()
        Me.cmb_usu.DataBind()
        Me.cmb_usu.SelectedIndex = -1
        Me.cmb_usu.Text = ""
    End Sub

    Protected Sub bnt_add_employee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntlk_add_employee.Click

        If Val(cmb_usu.SelectedValue) > 0 Then

            Dim Sql = " INSERT INTO ta_gruposRoles_temp(id_temp, id_rol, id_usuario) VALUES ('" & Me.lbl_idTemp.Text & "'," & Me.cmb_rol.SelectedValue & "," & Me.cmb_usu.SelectedValue & ")"
            cnnSAP.Open()
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")
            cnnSAP.Close()
            Me.grd_cate.DataBind()
            Me.cmb_rol.Enabled = False
            Me.cmb_usu.DataBind()
            Me.cmb_usu.SelectedIndex = -1
            Me.cmb_usu.Text = ""

        End If
    End Sub

    Protected Sub imgUpd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUpd.Click
        Me.cmb_usu.DataBind()
        Me.cmb_usu.SelectedIndex = -1
        Me.cmb_usu.Text = ""
        If Me.grd_cate.Items.Count() > 0 Then
            Me.cmb_rol.Enabled = False
        Else
            Me.cmb_rol.Enabled = True
        End If
    End Sub

    Protected Sub chk_app_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub

    Protected Sub chk_comment_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub

    Protected Sub chk_Query_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = CType(e.Item, GridDataItem)
            Dim chk_app As CheckBox = CType(itemD("approval").FindControl("chk_app"), CheckBox) 'CType(e.Item.FindControl("chk_app"), CheckBox)
            Dim chk_comment As CheckBox = CType(itemD("comment").FindControl("chk_comment"), CheckBox) 'CType(e.Item.FindControl("chk_comment"), CheckBox)
            Dim chk_Query As CheckBox = CType(itemD("Query").FindControl("chk_Query"), CheckBox) 'CType(e.Item.FindControl("chk_Query"), CheckBox)

            Dim app = itemD("Caprueba").Text 'e.Item.Cells(8).Text.ToString
            Dim coment = itemD("Ccomenta").Text 'e.Item.Cells(9).Text.ToString
            Dim Query = itemD("Cconsulta").Text 'e.Item.Cells(10).Text.ToString

            If app.ToString = "SI" Then
                chk_app.Checked = True
            Else
                chk_app.Checked = False
            End If

            If coment.ToString = "SI" Then
                chk_comment.Checked = True
            Else
                chk_comment.Checked = False
            End If

            If Query.ToString = "SI" Then
                chk_Query.Checked = True
            Else
                chk_Query.Checked = False
            End If

        End If
    End Sub

End Class
