Imports ly_SIME
Public Class frm_aportesAD
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "NEW_APORTE"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListado As New ly_SIME.CORE.cls_listados

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
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
                cl_user.chk_Rights(Page.Controls, 8, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.cmb_aporte_origen.DataSourceID = ""
            Me.cmb_aporte_origen.DataSource = clListado.get_t_aporte_origen(idPrograma)
            Me.cmb_aporte_origen.DataTextField = "nombre_AporteOrigen"
            Me.cmb_aporte_origen.DataValueField = "id_AporteOrigen"
            Me.cmb_aporte_origen.DataBind()

            Me.cmb_aporte_cl.DataSourceID = ""
            Me.cmb_aporte_cl.DataSource = clListado.get_t_aporte_cl(idPrograma)
            Me.cmb_aporte_cl.DataTextField = "cl_nombreES"
            Me.cmb_aporte_cl.DataValueField = "id_aporteCL"
            Me.cmb_aporte_cl.DataBind()

            'Me.cmb_Budget.DataSourceID = ""
            'Me.cmb_Budget.DataSource = clListado.get_t_budget(idPrograma)
            'Me.cmb_Budget.DataTextField = "bud_name"
            'Me.cmb_Budget.DataValueField = "id_budget"
            'Me.cmb_Budget.DataBind()
            'Me.cmb_Budget.SelectedValue = 7 'Set DO Not Apply, default value

            'Me.SEL_BUD.Checked = False
            'Me.cmb_Budget.Enabled = False

        End If
    End Sub


    'Protected Sub SEL_BUD_CheckedChanged(sender As Object, e As EventArgs) Handles SEL_BUD.CheckedChanged

    '    If SEL_BUD.Checked = True Then
    '        Me.cmb_Budget.Enabled = False
    '    Else
    '        Me.cmb_Budget.Enabled = True
    '    End If

    '    Me.cmb_Budget.Enabled = True

    'End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim oAporte As New tme_Aportes

                oAporte.nombre_aporte = Me.txt_nombreaporte.Text
                oAporte.id_AporteCL = Me.cmb_aporte_cl.SelectedValue
                oAporte.id_AporteOrigen = Me.cmb_aporte_origen.SelectedValue
                'oAporte.id_budget = Val(Me.cmb_Budget.SelectedValue)
                oAporte.id_programa = idPrograma
                dbEntities.tme_Aportes.Add(oAporte)
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_aportes"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Administracion/frm_aportes")
    End Sub
End Class