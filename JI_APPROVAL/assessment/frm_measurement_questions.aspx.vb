Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class frm_measurement_questions
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "QUESTIONS_ASSEMENT"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim helper As New ly_APPROVAL.APPROVAL.HelperMethods
    Dim listScale As New List(Of tme_measurement_answer_scale)
    Dim listSkill As New List(Of tme_measurement_skill)
    Dim listTitle As New List(Of tme_measurement_title)
    Dim listTheme As New List(Of tme_measurement_theme)


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

            Me.btn_deleteRegister.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        loadList()

        If Not Me.IsPostBack Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities
                'Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                'Me.lbl_id_ficha.Text = id
                'loadListas(idPrograma, proyecto)

                'LoadData(id)

                Me.alink_themes.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_cat1"))
                Me.alink_skills.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_cat2"))
                Me.alink_title.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_cat3"))
                Me.alink_answer_scale.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_answer_scales"))
                Me.alink_answer_options.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_answer_options"))
                Me.alink_questions.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_questions"))
                Me.alink_survey.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_surveys"))
                Me.alink_survey_questions.Attributes.Add("href", ResolveUrl("~/assessment/frm_measurement_question_config"))

                helper.SetValues(Me.cmb_answer_scale, listScale, "scale_name", "id_measurement_answer_scale")
                helper.SetValues(Me.cmb_skill, listSkill, "skill_name", "id_measurement_skill")
                helper.SetValues(Me.cmb_title, listTitle, "title_name", "id_measurement_title")
                helper.SetValues(Me.cmb_theme, listTheme, "theme_name", "id_measurement_theme")

                Me.cmb_theme.DataBind()
                Dim id_theme = Convert.ToInt32(Me.cmb_theme.SelectedValue)
                Me.lbl_max_percentage.Text = dbEntities.tme_measurement_theme.Find(id_theme).percent_value

                Fill_Grid(True)

            End Using

        End If


    End Sub

    Public Sub Fill_Grid(ByVal booRebind As Boolean)

        Using dbEntities As New dbRMS_JIEntities

            Me.grd_questions.DataSource = dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            If booRebind Then
                Me.grd_questions.DataBind()
            End If

        End Using

    End Sub

    Protected Sub DeleteQuestion_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_deleteRegister_Click(sender As Object, e As EventArgs) Handles btn_deleteRegister.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_measurement_question WHERE id_measurement_question = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            Me.grd_questions.DataSource = dbEntities.tme_measurement_question.ToList()
            Me.grd_questions.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Protected Sub btn_add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_add.Click
        'guardarIndicadores()
        Dim maxpercentage = Convert.ToDouble(Me.lbl_max_percentage.Text)
        'If validation() + Me.txt_percent.Value <= maxpercentage Then
        Using dbEntities As New dbRMS_JIEntities

            Dim oQuestion = New tme_measurement_question
            oQuestion.question_name = Me.txt_question.Text.Trim.Replace("'", "''")
            'oQuestion.percent_value = Me.txt_percent.Value
            'oQuestion.order_number = Me.txt_order_number.Value
            oQuestion.id_measurement_answer_scale = Me.cmb_answer_scale.SelectedValue
            oQuestion.id_measurement_skill = Me.cmb_skill.SelectedValue
            oQuestion.id_measurement_title = Me.cmb_title.SelectedValue
            oQuestion.id_measurement_theme = Me.cmb_theme.SelectedValue
            oQuestion.fecha_crea = Date.Now
            oQuestion.id_usuario_crea = cl_user.id_usr

            If Not String.IsNullOrEmpty(Me.txt_QuestionCode.Text) Then
                oQuestion.question_code = Me.txt_QuestionCode.Text.Trim.ToUpper
            End If

            dbEntities.tme_measurement_question.Add(oQuestion)
            dbEntities.SaveChanges()
            'listaIndicadores()

        End Using
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_questions"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        'Else
        'Me.lbl_alert.Visible = True
        'End If
    End Sub

    Protected Sub grd_questions_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_questions.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            'Dim PercentValue As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_value"), RadNumericTextBox)
            'PercentValue.Value = DataBinder.Eval(e.Item.DataItem, "percent_value").ToString()
            'Dim OrderNumber As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_order_number"), RadNumericTextBox)
            'OrderNumber.Value = DataBinder.Eval(e.Item.DataItem, "order_number").ToString()
            Dim ThemeName As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_question"), RadTextBox)
            ThemeName.Text = DataBinder.Eval(e.Item.DataItem, "question_name").ToString()

            Dim CodQuestion As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_code"), RadTextBox)

            If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "question_code")) Then
                CodQuestion.Text = DataBinder.Eval(e.Item.DataItem, "question_code")
            Else
                CodQuestion.Text = ""
            End If

            Dim AnswerScale As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_answer_scale"), RadComboBox)

            helper.SetValues(AnswerScale, listScale, "scale_name", "id_measurement_answer_scale")
            AnswerScale.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_answer_scale").ToString()

            Dim cmb_skills As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_skills"), RadComboBox)
            helper.SetValues(cmb_skills, listSkill, "skill_name", "id_measurement_skill")
            cmb_skills.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_skill").ToString()

            Dim cmb_titles As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_titles"), RadComboBox)
            helper.SetValues(cmb_titles, listTitle, "title_name", "id_measurement_title")
            cmb_titles.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_title").ToString()

            Dim cmb_themes As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_themes"), RadComboBox)
            helper.SetValues(cmb_themes, listTheme, "theme_name", "id_measurement_theme")
            cmb_themes.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_theme").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_measurement_question").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_measurement_question").ToString())
        End If

    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim sql As String = ""
        'Dim maxpercentage = Convert.ToDouble(Me.lbl_max_percentage.Text)
        'If validation() <= maxpercentage Then
        For Each row In Me.grd_questions.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_question")
                'Dim percent As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)
                Dim AnsweroptionName As RadTextBox = CType(row.Cells(0).FindControl("txt_question"), RadTextBox)
                'Dim OrderNumber As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_order_number"), RadNumericTextBox)
                Dim AnswerScale As RadComboBox = CType(row.Cells(0).FindControl("cmb_answer_scale"), RadComboBox)
                Dim cmb_skills As RadComboBox = CType(row.Cells(0).FindControl("cmb_skills"), RadComboBox)
                Dim cmb_titles As RadComboBox = CType(row.Cells(0).FindControl("cmb_titles"), RadComboBox)
                Dim cmb_themes As RadComboBox = CType(row.Cells(0).FindControl("cmb_themes"), RadComboBox)
                Dim strCode As RadTextBox = CType(row.Cells(0).FindControl("txt_code"), RadTextBox)


                sql = "UPDATE tme_measurement_question SET question_name = '" & AnsweroptionName.Text.Trim.Replace("'", "''") & "' "
                'sql &= "', percent_value =" & percent.Value ", order_number =" & OrderNumber.Value
                sql &= ", id_measurement_answer_scale = " & AnswerScale.SelectedValue & ", id_measurement_skill = " & cmb_skills.SelectedValue
                sql &= ", id_measurement_title = " & cmb_titles.SelectedValue & ", id_measurement_theme = " & cmb_themes.SelectedValue & ", question_code='" & strCode.Text.Trim.ToUpper & "' "
                sql &= ", fecha_upd = GETDATE(), id_usuario_upd = " & cl_user.id_usr
                sql &= " WHERE id_measurement_question= " & IDInstrumentoID.ToString

                Using dbEntities As New dbRMS_JIEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
        Next

        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_questions"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        'Else
        'Me.lbl_alert.Visible = True
        'End If


    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/assessment/frm_measurement_cat1"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub grd_questions_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_questions.PageIndexChanged
        'Using dbEntities As New dbRMS_JIEntities
        '    Me.grd_questions.DataSource = dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
        '    Me.grd_questions.DataBind()
        'End Using
        Fill_Grid(False)
    End Sub

    Protected Sub grd_skills_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_questions.PageSizeChanged
        'Using dbEntities As New dbRMS_JIEntities
        '    Me.grd_questions.DataSource = dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
        '    Me.grd_questions.DataBind()
        'End Using
        Fill_Grid(False)
    End Sub

    Protected Sub cmb_theme_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_theme.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            loadList()
            Dim id_theme = Convert.ToInt32(Me.cmb_theme.SelectedValue)
            Me.lbl_max_percentage.Text = dbEntities.tme_measurement_theme.Find(id_theme).percent_value
            'Me.grd_questions.DataSource = dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            'Me.grd_questions.DataBind()
            Fill_Grid(True)
        End Using
    End Sub

    Sub loadList()
        Using dbEntities As New dbRMS_JIEntities
            listScale = dbEntities.tme_measurement_answer_scale.ToList()
            listSkill = dbEntities.tme_measurement_skill.ToList()
            listTitle = dbEntities.tme_measurement_title.ToList()
            listTheme = dbEntities.tme_measurement_theme.ToList()
        End Using
    End Sub

    Function validation() As Double
        Dim totalPercent As Double = 0
        For Each row In Me.grd_questions.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim total_percent As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)
                totalPercent = totalPercent + total_percent.Value
            End If
        Next
        Return totalPercent
    End Function

    Private Sub grd_questions_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_questions.NeedDataSource
        Fill_Grid(False)
    End Sub
End Class