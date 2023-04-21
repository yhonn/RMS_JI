Imports ly_SIME

Public Class frm_menuEdit
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_EDIT_MENU"
    Dim clListados As New ly_SIME.CORE.cls_listados
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
            Me.lbl_id_menu.Text = Me.Request.QueryString("Id").ToString
            FillData(Me.lbl_id_menu.Text)
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim id_rol = Convert.ToInt32(Me.lbl_id_menu.Text)
                Dim oMenu = dbEntities.t_menu.Find(id_rol)

                oMenu.nombre_item_menu = Me.txt_nombre_item_menu.Text
                'oMenu.nombre_item_menu_en = Me.txt_nombre_item_menu_en.Text

                If chkPadre.Checked Then
                    oMenu.parent_item_menu = Me.cmb_superior.SelectedValue
                Else
                    oMenu.parent_item_menu = Nothing
                End If
                oMenu.id_icono = Me.chk_iconos.SelectedValue

                dbEntities.Entry(oMenu).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/SuperAdmin/frm_menu"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Sub FillData(ByVal Idprograma As String)

        Dim id_rol = Convert.ToInt32(Idprograma)

        Me.cmb_superior.DataSourceID = ""
        Me.cmb_superior.DataSource = clListados.get_t_menu()
        Me.cmb_superior.DataTextField = "nombre_item_menu"
        Me.cmb_superior.DataValueField = "id_menu"
        Me.cmb_superior.DataBind()

        Using dbEntities As New dbRMS_JIEntities

            Me.chk_iconos.DataSource = dbEntities.t_iconos.ToList().OrderBy(Function(p) p.icono_clase)
            Me.chk_iconos.DataTextField = "icono_texto"
            Me.chk_iconos.DataValueField = "id_icono"
            Me.chk_iconos.DataBind()

            Dim oMenu = dbEntities.t_menu.Find(id_rol)

            Me.txt_nombre_item_menu.Text = oMenu.nombre_item_menu
            'Me.txt_nombre_item_menu_en.Text = oMenu.nombre_item_menu_en
            If oMenu.parent_item_menu.HasValue Then
                Me.cmb_superior.SelectedValue = oMenu.parent_item_menu
                Me.chkPadre.Checked = True
            Else
                Me.chkPadre.Checked = False
            End If
            Me.chk_iconos.SelectedValue = oMenu.id_icono

        End Using
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/SuperAdmin/frm_menu"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub chkPadre_CheckedChanged(sender As Object, e As EventArgs)
        If chkPadre.Checked Then
            Me.cmb_superior.Enabled = True
        Else
            Me.cmb_superior.Enabled = False
        End If
    End Sub

    Protected Sub chk_iconos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_iconos.SelectedIndexChanged
        Dim chk_iconos As RadioButtonList = DirectCast(sender, RadioButtonList)
        chk_iconos.SelectedItem.Attributes.Add("style", "color: green")
    End Sub

End Class