Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_measurement_question_config
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "ASSES_QUEST_SETUP"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim helper As New ly_APPROVAL.APPROVAL.HelperMethods
    Dim listScale As New List(Of tme_measurement_answer_scale)
    Dim listQuestions As New List(Of tme_measurement_title)
    Dim listTheme As New List(Of tme_measurement_theme)
    Dim listSurvey As New List(Of ins_measurement_survey)



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

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities
                'Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                'Me.lbl_id_ficha.Text = id
                'Dim survey = dbEntities.ins_measurement_survey.Find(id)
                'Me.lbl_survey_name.Text = survey.survey_name
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

                loadList()
                helper.SetValues(Me.cmb_survey, listSurvey, "survey_name", "id_measurement_survey")
                helper.SetValues(Me.cmb_theme, listTheme, "theme_name", "id_measurement_theme")


                Me.cmb_theme.DataBind()
                Dim id_theme = Convert.ToInt32(Me.cmb_theme.SelectedValue)

                Dim ActualQuestions = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue _
                                                                                           And p.tme_measurement_question.id_measurement_theme = id_theme) _
                                                                                       .Select(Function(p) p.id_measurement_question)
                helper.SetValues(Me.cmb_question, dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = id_theme _
                                                                                                And Not ActualQuestions.Contains(p.id_measurement_question)).ToList(), "question_name", "id_measurement_question")
                Me.lbl_max_percentage.Text = dbEntities.tme_measurement_theme.Find(id_theme).percent_value

                Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue And p.tme_measurement_question.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
                Me.grd_questions.DataBind()

            End Using
        End If

    End Sub

    Protected Sub DeleteQuestion_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_deleteRegister_Click(sender As Object, e As EventArgs) Handles btn_deleteRegister.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM ins_measurement_question_config WHERE id_measurement_question_config = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.ToList()
            Me.grd_questions.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Protected Sub btn_add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_add.Click
        'guardarIndicadores()
        Dim maxpercentage = Convert.ToDouble(Me.lbl_max_percentage.Text)
        'If validation() + Me.txt_percent.Value <= maxpercentage Then
        Using dbEntities As New dbRMS_JIEntities
                Dim oQuestion = New ins_measurement_question_config
                oQuestion.id_measurement_question = Me.cmb_question.SelectedValue
                oQuestion.percent_value = Me.txt_percent.Value
                oQuestion.order_number = Me.txt_order_number.Value
                oQuestion.id_measurement_survey = Me.cmb_survey.SelectedValue
                oQuestion.fecha_crea = Date.Now
                oQuestion.id_usuario_crea = cl_user.id_usr
                dbEntities.ins_measurement_question_config.Add(oQuestion)
                dbEntities.SaveChanges()

                'listaIndicadores()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_question_config"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        'Else
        '    Me.lbl_alert.Visible = True
        'End If
    End Sub

    Protected Sub grd_questions_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_questions.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim PercentValue As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_value"), RadNumericTextBox)
            PercentValue.Value = DataBinder.Eval(e.Item.DataItem, "percent_value").ToString()
            Dim OrderNumber As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_order_number"), RadNumericTextBox)
            OrderNumber.Value = DataBinder.Eval(e.Item.DataItem, "order_number").ToString()


            Dim AnswerScale As RadComboBox = CType(e.Item.Cells(0).FindControl("cmb_question"), RadComboBox)

            Using dbEntities As New dbRMS_JIEntities
                helper.SetValues(AnswerScale, dbEntities.tme_measurement_question.ToList(), "question_name", "id_measurement_question")
            End Using
            AnswerScale.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_measurement_question").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_measurement_question_config").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_measurement_question_config").ToString())
        End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim sql As String = ""
        Dim maxpercentage = Convert.ToDouble(Me.lbl_max_percentage.Text)
        ' If validation() <= maxpercentage Then
        For Each row In Me.grd_questions.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_question_config")
                    Dim percent As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)
                    Dim OrderNumber As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_order_number"), RadNumericTextBox)
                    Dim cmb_question As RadComboBox = CType(row.Cells(0).FindControl("cmb_question"), RadComboBox)

                    'oQuestion.id_measurement_question_config = Me.cmb_question.SelectedValue
                    'oQuestion.percent_value = Me.txt_percent.Value
                    'oQuestion.order_number = Me.txt_order_number.Value
                    'oQuestion.id_measurement_survey = Me.cmb_survey.SelectedValue
                    'oQuestion.fecha_crea = Date.Now

                    sql = "UPDATE ins_measurement_question_config SET id_measurement_question = " & cmb_question.SelectedValue
                    sql &= ", order_number = " & OrderNumber.Value & ", percent_value = " & percent.Value
                    sql &= ", fecha_upd = GETDATE(), id_usuario_upd = " & cl_user.id_usr
                    sql &= " WHERE id_measurement_question_config= " & IDInstrumentoID.ToString

                    Using dbEntities As New dbRMS_JIEntities
                        dbEntities.Database.ExecuteSqlCommand(sql)
                    End Using
                End If
            Next

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_questions"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        'Else
        '    Me.lbl_alert.Visible = True
        'End If


    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/assessment/frm_measurement_cat1"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub grd_questions_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_questions.PageIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue And p.tme_measurement_question.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            Me.grd_questions.DataBind()
        End Using
    End Sub

    Protected Sub grd_skills_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_questions.PageSizeChanged
        Using dbEntities As New dbRMS_JIEntities
            Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue And p.tme_measurement_question.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            Me.grd_questions.DataBind()
        End Using
    End Sub

    Protected Sub cmb_theme_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_theme.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            'loadList()
            Dim id_theme = Convert.ToInt32(Me.cmb_theme.SelectedValue)
            Me.lbl_max_percentage.Text = dbEntities.tme_measurement_theme.Find(id_theme).percent_value

            Dim ActualQuestions = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue _
                                                                                           And p.tme_measurement_question.id_measurement_theme = id_theme) _
                                                                                       .Select(Function(p) p.id_measurement_question)
            helper.SetValues(Me.cmb_question, dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = id_theme _
                                                                                            And Not ActualQuestions.Contains(p.id_measurement_question)).ToList(), "question_name", "id_measurement_question")
            Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue And p.tme_measurement_question.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            Me.grd_questions.DataBind()
        End Using
    End Sub

    Protected Sub cmb_survey_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_survey.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim id_theme = Me.cmb_theme.SelectedValue
            Dim ActualQuestions = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue _
                                                                                           And p.tme_measurement_question.id_measurement_theme = id_theme) _
                                                                                       .Select(Function(p) p.id_measurement_question)
            helper.SetValues(Me.cmb_question, dbEntities.tme_measurement_question.Where(Function(p) p.id_measurement_theme = id_theme _
                                                                                            And Not ActualQuestions.Contains(p.id_measurement_question)).ToList(), "question_name", "id_measurement_question")
            Me.grd_questions.DataSource = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = Me.cmb_survey.SelectedValue And p.tme_measurement_question.id_measurement_theme = Me.cmb_theme.SelectedValue).ToList()
            Me.grd_questions.DataBind()
        End Using
    End Sub


    Sub loadList()
        Using dbEntities As New dbRMS_JIEntities
            listSurvey = dbEntities.ins_measurement_survey.ToList()
            listScale = dbEntities.tme_measurement_answer_scale.ToList()
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
End Class