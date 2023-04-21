Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Partial Class frm_roles_edit
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROLES_Edit"
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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            Dim Sql = "SELECT nombre_proyecto, id_rol_user, nombre_rol, descripcion_rol, id_usuario FROM vw_ta_roles_emplead WHERE id_rol=" & Me.Request.QueryString("IdType")
            Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds1 As New DataSet("proyecto")
            dm1.Fill(ds1, "proyecto")
            'Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item("nombre_proyecto").ToString
            Me.txt_cat.Text = ds1.Tables("proyecto").Rows(0).Item("nombre_rol").ToString
            Me.txt_des.Text = ds1.Tables("proyecto").Rows(0).Item("descripcion_rol").ToString

            If ds1.Tables("proyecto").Rows(0).Item("id_usuario") > 0 Then
                Me.cmb_usu.SelectedValue = ds1.Tables("proyecto").Rows(0).Item("id_usuario")
                Me.hidd_id_rol_user.Value = ds1.Tables("proyecto").Rows(0).Item("id_rol_user").ToString
            Else
                Me.hidd_id_rol_user.Value = 0
            End If

            Me.HiddenField1.Value = Me.Request.QueryString("IdType")
            chk_RemoveUser.Enabled = IIf(Me.hidd_id_rol_user.Value = 0, False, True)

        End If

    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""
        Dim Sql As String


        If Not chk_RemoveUser.Checked = True Then

            If Val(cmb_usu.SelectedValue) > 0 Then



                Sql = String.Format("SELECT nombre_proyecto, nombre_rol, descripcion_rol, id_usuario 
                                                FROM vw_ta_roles_emplead WHERE id_usuario={0}  AND id_rol <> {1} and id_type_role = 1  and id_programa = {2} ", Me.cmb_usu.SelectedValue, Me.HiddenField1.Value, Me.Session("E_IDPrograma"))
                Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
                Dim ds1 As New DataSet("rol")
                dm1.Fill(ds1, "rol")
                If ds1.Tables("rol").Rows.Count > 0 Then
                    Me.lblmsj_err.Text = String.Format(" User has already been related to another role ({0})  ", ds1.Tables("rol").Rows.Item(0).Item("nombre_rol"))
                    Me.lblmsj_err.Visible = True
                    err = True
                End If

                If err = False Then

                    '*********************************************CHECK THAT THIS ROLES HAS´NT OPEN APPROVAL PROCCESSES***************************************************************
                    Sql = String.Format(" UPDATE ta_roles SET nombre_rol='{0}', descripcion_rol='{1}'  WHERE id_rol={2} ", Me.txt_cat.Text, Me.txt_des.Text, Me.HiddenField1.Value)
                    cnnSAP.Open()
                    Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                    dm.SelectCommand.ExecuteNonQuery()

                    If Me.hidd_id_rol_user.Value > 0 Then
                        '*********************************************HAS TO UPDATE OPEN APPROVALS THAT THIS ROLE IS PARTICIPATING***************************************************************
                        Sql = String.Format("Update ta_role_user set id_usuario = {0} where id_rol_user = {1} ", Me.cmb_usu.SelectedValue, Me.hidd_id_rol_user.Value)
                    Else
                        Sql = String.Format(" insert into ta_role_user  (id_rol, id_usuario) values  ({0},{1}) ", Me.HiddenField1.Value, Me.cmb_usu.SelectedValue)
                    End If

                    dm.SelectCommand.CommandText = Sql
                    dm.SelectCommand.ExecuteNonQuery()
                    cnnSAP.Close()
                    '*********************************************CHECK THAT THIS ROLES HAS´NT OPEN APPROVAL PROCCESSES***************************************************************
                    'Session("In-Out") = "OUT"
                    Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")

                End If

            Else
                Me.lblmsj_err.Text = String.Format(" Select a user to relate to this role ")
                Me.lblmsj_err.Visible = True
                err = True

            End If

        Else

            '*********************************************CHECK THAT THIS ROLES HAS´NT OPEN APPROVAL PROCCESSES***************************************************************
            Sql = String.Format(" delete from ta_role_user where id_rol_user = {0} ", Me.hidd_id_rol_user.Value)
            cnnSAP.Open()
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            dm.SelectCommand.ExecuteNonQuery()
            cnnSAP.Close()
            '*********************************************CHECK THAT THIS ROLES HAS´NT OPEN APPROVAL PROCCESSES***************************************************************
            Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")

        End If

    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")
    End Sub

    Protected Sub chk_RemoveUser_CheckedChanged(sender As Object, e As EventArgs) Handles chk_RemoveUser.CheckedChanged
        If chk_RemoveUser.Checked = True Then
            cmb_usu.SelectedIndex = -1
            cmb_usu.Text = ""
            cmb_usu.Enabled = False
        Else
            cmb_usu.SelectedIndex = -1
            cmb_usu.Text = ""
            cmb_usu.Enabled = True
        End If
    End Sub
End Class
