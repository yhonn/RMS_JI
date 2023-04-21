Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_AdminLanguageControles
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_LANG_CONTR_OT"
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
                Me.cmb_idioma.DataTextField = "descripcion_idioma"
                Me.cmb_idioma.DataValueField = "id_idioma"
                Me.cmb_idioma.DataBind()
                Dim idIdioma = Convert.ToInt32(Me.Request.QueryString("IdI").ToString)
                Me.cmb_idioma.SelectedValue = idIdioma
            End Using
            FillData()
        End If
    End Sub


    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False
        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities

                Dim row As GridItem
                Dim id_control_otro = 0
                Dim ItemD As GridDataItem
                Dim controles = dbEntities.t_control_otro_idioma.ToList()
                For Each row In Me.grd_cate.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        id_control_otro = Convert.ToInt32(dataItem.GetDataKeyValue("id_control_otro"))
                        ItemD = CType(row, GridDataItem)
                        Dim observaciones As RadTextBox = CType(ItemD.FindControl("txt_valor"), RadTextBox)

                        If controles.Where(Function(p) p.id_control_otro = id_control_otro).Count() > 0 Then
                            Dim oMeta = dbEntities.t_control_otro_idioma.Where(Function(p) p.id_control_otro = id_control_otro And p.id_idioma = Me.cmb_idioma.SelectedValue)
                            If oMeta.Count() > 0 Then
                                Dim menuIdioma = oMeta.FirstOrDefault()
                                menuIdioma.texto = observaciones.Text
                                dbEntities.Entry(menuIdioma).State = Entity.EntityState.Modified
                            Else
                                Dim oMenuIdioma = New t_control_otro_idioma
                                oMenuIdioma.id_control_otro = id_control_otro
                                oMenuIdioma.id_idioma = Me.cmb_idioma.SelectedValue
                                oMenuIdioma.texto = observaciones.Text

                                dbEntities.t_control_otro_idioma.Add(oMenuIdioma)
                            End If
                        Else
                            Dim oMenuIdioma = New t_control_otro_idioma
                            oMenuIdioma.id_control_otro = id_control_otro
                            oMenuIdioma.id_idioma = Me.cmb_idioma.SelectedValue
                            oMenuIdioma.texto = observaciones.Text

                            dbEntities.t_control_otro_idioma.Add(oMenuIdioma)
                        End If
                    End If
                Next
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_language_settings"
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
                Dim idiomaMenu = dbEntities.t_control_otro_idioma.ToList()
                Me.grd_cate.DataSource = dbEntities.vw_control_otro_idioma.Where(Function(p) p.id_idioma = Me.cmb_idioma.SelectedValue).ToList()
                Me.grd_cate.DataBind()
                Dim i = 0
                Dim row As GridItem
                For Each row In Me.grd_cate.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_control_otro As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_control_otro"))
                        ItemD = CType(row, GridDataItem)
                        Dim observaciones As RadTextBox = CType(ItemD.FindControl("txt_valor"), RadTextBox)
                        If idiomaMenu.Where(Function(p) p.id_control_otro = id_control_otro And p.id_idioma = Me.cmb_idioma.SelectedValue).Count() > 0 Then
                            observaciones.Text = idiomaMenu.FirstOrDefault(Function(p) p.id_control_otro = id_control_otro And p.id_idioma = Me.cmb_idioma.SelectedValue).texto
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            Dim errorM = ex
        End Try

    End Sub

    Protected Sub cmb_id_idioma_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_idioma.SelectedIndexChanged
        FillData()
    End Sub
End Class