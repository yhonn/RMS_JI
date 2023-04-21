Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL


Partial Class frm_groups_edit

    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROl_GROUP_EDIT"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cls_groupRoles As APPROVAL.clss_GroupRoles

    Sub fillDatos()

        Dim Sql As String = String.Format("SELECT distinct a.nombre_grupo, a.descripcion_grupo, isnull(b.id_rol,0) as Rol  " &
                                          "  from ta_grupos a " &
                                          "    left join  ta_gruposRoles b on (a.id_grupo = b.id_grupo) " &
                                          "   where a.id_grupo={0}", Me.lbl_idTemp.Text)
        '"SELECT nombre_grupo, descripcion_grupo, dbo.GrupoRol(id_grupo) as Rol from ta_grupos where id_grupo=" & Me.lbl_idTemp.Text
        Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        Dim ds As New DataSet("IdPlan")
        dm.Fill(ds, "IdPlan")
        Me.txt_grupo.Text = ds.Tables("IdPlan").Rows(0).Item("nombre_grupo")
        Me.txt_des.Text = ds.Tables("IdPlan").Rows(0).Item("descripcion_grupo")
        Me.cmb_rol.SelectedValue = ds.Tables("IdPlan").Rows(0).Item("Rol")

        If ds.Tables("IdPlan").Rows(0).Item("Rol") = 0 Then
            Me.cmb_rol.Enabled = True
        Else
            Me.cmb_rol.Enabled = False
        End If

    End Sub

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

            sql = "UPDATE ta_gruposRoles SET aprueba='" & app.ToString & "', comenta='" & comment.ToString & "' , consulta='" & Query.ToString & " ' WHERE id_grupoRol=" & IDrow("id_grupoRol").Text

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
            '' Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item(0).ToString

            cls_groupRoles = New APPROVAL.clss_GroupRoles(Me.Session("E_IDPrograma"))

            Dim Sql As String = String.Format("	SELECT distinct a.nombre_grupo, a.descripcion_grupo, isnull(b.id_rol,0) as Rol  " &
                                              "   from ta_grupos a   " &
                                              "     left join ta_gruposRoles b on (a.id_grupo = b.id_grupo)  " &
                                              "       where a.id_grupo={0} ", Me.Request.QueryString("IdType"))
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("RolT")
            dm.Fill(ds, "RolT")

            cmb_rol.DataSource = cls_groupRoles.Query_Roles(ds.Tables("RolT").Rows(0).Item("Rol"))
            Me.cmb_rol.DataBind()

            Sql = "SELECT id_usuario, nombre_empleado from vw_ta_roles_emplead where id_rol=" & Me.cmb_rol.SelectedValue
            dm.SelectCommand.CommandText = Sql
            dm.Fill(ds, "IdPlan")

            Me.lblEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item("nombre_empleado")
            Me.lbl_IdEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item("id_usuario")
            Me.lbl_idTemp.Text = Me.Request.QueryString("IdType")
            fillDatos()

            Sql = "SELECT nombre_empleado from vw_ta_roles_emplead where id_rol=" & Me.cmb_rol.SelectedValue
            ds.Tables.Add("roles_empleado")
            dm.SelectCommand.CommandText = Sql
            dm.Fill(ds, "roles_empleado")
            Me.lblEmpleado.Text = ds.Tables("roles_empleado").Rows(0).Item(0)

            Me.grd_cate.DataBind()
            Me.grd_cate.Columns(0).Visible = True

        Else

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""
        If err = False Then
            ' Me.Label3.Text = ext.ToString
            Dim Sql = " UPDATE ta_grupos SET nombre_grupo='" & Me.txt_grupo.Text & "', descripcion_grupo='" & Me.txt_des.Text & "' WHERE id_grupo=" & Me.lbl_idTemp.Text
            cnnSAP.Open()
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")
            cnnSAP.Close()
            Me.Response.Redirect("~/Approvals/frm_consulta_groups.aspx")
        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_groups.aspx")
    End Sub

    Protected Sub cmb_rol_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_rol.SelectedIndexChanged
        Dim Sql = "SELECT nombre_empleado from vw_ta_roles_emplead where id_rol=" & Me.cmb_rol.SelectedValue
        Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        Dim ds As New DataSet("IdPlan")
        dm.Fill(ds, "IdPlan")
        Me.lblEmpleado.Text = ds.Tables("IdPlan").Rows(0).Item(0)

    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_grupoRol")
        Me.SqlDataSource3.DeleteCommand = "DELETE FROM ta_gruposRoles WHERE (id_grupoRol = " & id_temp & ")"
        Me.grd_cate.DataBind()
        Me.cmb_usu.DataBind()
        Me.cmb_usu.SelectedIndex = -1
        Me.cmb_usu.Text = ""
    End Sub

    Protected Sub bnt_add_employee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntlk_add_employee.Click

        'Valid null values

        If Val(cmb_usu.SelectedValue) > 0 Then

            Dim Sql = " INSERT INTO ta_gruposRoles(id_grupo, id_rol, id_usuario) VALUES ('" & Me.lbl_idTemp.Text & "'," & Me.cmb_rol.SelectedValue & "," & Me.cmb_usu.SelectedValue & ")"
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
