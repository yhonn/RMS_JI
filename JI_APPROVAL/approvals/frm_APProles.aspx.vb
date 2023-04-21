Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Partial Class frm_rolesAPP
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Const TYPErolSIMPLE = 1

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROLES_ADD"
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

            Dim Sql = String.Format(" Select id_usuario, nombre_empleado, estado  " &
                                    "    from vw_user_role_simple     " &
                                    "      where id_programa =  {0} " &
                                    "        and (upper(estado) = 'ACTIVE' or upper(estado) = 'ACTIVO' )" &
                                    "         and id_rol_user = 0 ", Me.Session("E_IDPrograma"))

            Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds1 As New DataSet("User")
            dm1.Fill(ds1, "User")

            If ds1.Tables("User").Rows.Count = 0 Then
                lblt_filerequeried.Visible = "true"
            End If

            cmb_usu.SelectedIndex = -1
            cmb_usu.Text = ""

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""

        If Not Val(Me.cmb_usu.SelectedValue) > 0 Then
            err = True
        End If

        If err = False Then 'SIMPLE ROLE TYPE

            Me.lblt_filerequeried.Text = ext.ToString
            Dim Sql = " INSERT INTO ta_roles (nombre_rol, descripcion_rol, id_programa, id_type_role) VALUES ('" & Me.txt_cat.Text & "', '" & Me.txt_des.Text & "'," & Me.Session("E_IDPrograma") & ", " & TYPErolSIMPLE & " ) SELECT @@IDENTITY "
            cnnSAP.Open()
            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")

            Dim id_role As Integer = ds.Tables("IdPlan").Rows(0).Item(0)

            Sql = String.Format("insert into ta_role_user  (id_rol, id_usuario) values  ({0},{1})", id_role, Me.cmb_usu.SelectedValue)
            dm.SelectCommand.CommandText = Sql
            dm.SelectCommand.ExecuteNonQuery()

            cnnSAP.Close()
            cnnSAP.Close()
            'Session("In-Out") = "OUT"
            Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")

        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")
    End Sub
End Class
