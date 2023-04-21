Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_lenguageAD
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_NEW_LANG"
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
            Me.HiddenProg.Value = Me.Session("E_IDPrograma")
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
            Me.hidden_mod.Value = id
            Dim idIdioma = Convert.ToInt32(Me.Request.QueryString("IdI").ToString)
            Me.cmb_id_idioma.SelectedValue = idIdioma
            FillData()
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        Dim id_mod = Convert.ToInt32(Me.hidden_mod.Value)
        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities

                Dim row As GridItem
                Dim id_control = 0
                Dim ItemD As GridDataItem
                Dim controles = dbEntities.vw_ctrl_language.Where(Function(p) p.id_mod = id_mod And p.id_idioma = Me.cmb_id_idioma.SelectedValue)
                For Each row In Me.grd_actividad.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        id_control = Convert.ToInt32(dataItem.GetDataKeyValue("id_control"))
                        ItemD = CType(row, GridDataItem)
                        Dim observaciones As RadTextBox = CType(ItemD.FindControl("txt_valor"), RadTextBox)

                        If controles.Count(Function(p) p.id_control = id_control) > 0 Then
                            Dim oMeta = dbEntities.t_control_idiomas.FirstOrDefault(Function(p) p.id_control = id_control And p.id_idioma = Me.cmb_id_idioma.SelectedValue)
                            oMeta.valor = observaciones.Text
                            dbEntities.Entry(oMeta).State = Entity.EntityState.Modified

                        Else
                            Dim oMeta = New t_control_idiomas
                            oMeta.id_control = id_control
                            oMeta.id_idioma = Me.cmb_id_idioma.SelectedValue
                            oMeta.valor = observaciones.Text

                            dbEntities.t_control_idiomas.Add(oMeta)
                        End If
                    End If
                Next
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_adminLanguage"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_adminLanguage"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Sub FillData()
        Dim ItemD As GridDataItem
        Try REM por si no hay una planificacion
            Using dbEntities As New dbRMS_JIEntities
                Dim id_mod = Convert.ToInt32(Me.hidden_mod.Value)
                Me.lbl_modulo.Text = dbEntities.t_mod.Find(id_mod).mod_name
                Dim controles = dbEntities.vw_controles.Where(Function(p) p.id_mod = id_mod).ToList()
                Dim controlesIdioma = dbEntities.vw_ctrl_language.Where(Function(p) p.id_mod = id_mod And p.id_idioma = Me.cmb_id_idioma.SelectedValue).ToList()
                Me.grd_actividad.DataSource = controles
                Me.grd_actividad.DataBind()
                Dim i = 0
                Dim row As GridItem
                For Each row In Me.grd_actividad.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_control As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_control"))
                        ItemD = CType(row, GridDataItem)
                        Dim observaciones As RadTextBox = CType(ItemD.FindControl("txt_valor"), RadTextBox)
                        If controlesIdioma.Where(Function(p) p.id_control = id_control And p.id_idioma = Me.cmb_id_idioma.SelectedValue).Count() > 0 Then
                            observaciones.Text = controlesIdioma.FirstOrDefault(Function(p) p.id_control = id_control And p.id_idioma = Me.cmb_id_idioma.SelectedValue).valor
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            Dim errorM = ex
        End Try

    End Sub


    Protected Sub cmb_id_idioma_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_id_idioma.SelectedIndexChanged
        FillData()
    End Sub
End Class