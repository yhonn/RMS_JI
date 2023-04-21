Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class frm_measurement_surveys
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "ASSESMENT_MNG"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
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

            Me.btn_deleteRegister.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

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

                'Dim actuales = dbEntities.tme_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto = id) _
                '           .Select(Function(p) p.id_indicador).ToList()

                Me.grd_surveys.DataSource = dbEntities.ins_measurement_survey.ToList()
                Me.grd_surveys.DataBind()

            End Using
        End If

    End Sub

    Protected Sub DeleteSurvey_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub btn_deleteRegister_Click(sender As Object, e As EventArgs) Handles btn_deleteRegister.Click
        Using dbEntities As New dbRMS_JIEntities
            Try
                dbEntities.Database.ExecuteSqlCommand("DELETE FROM ins_measurement_survey WHERE id_measurement_survey = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            Me.grd_surveys.DataSource = dbEntities.ins_measurement_survey.ToList()
            Me.grd_surveys.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub

    Protected Sub btn_add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_add.Click
        'guardarIndicadores()
        Using dbEntities As New dbRMS_JIEntities
            Dim osurvey = New ins_measurement_survey
            osurvey.survey_name = Me.txt_survey.Text
            dbEntities.ins_measurement_survey.Add(osurvey)
            dbEntities.SaveChanges()

            'listaIndicadores()
        End Using
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_surveys"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    End Sub

    Protected Sub grd_surveys_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_surveys.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            'Dim PercentValue As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_value"), RadNumericTextBox)
            'PercentValue.Text = DataBinder.Eval(e.Item.DataItem, "percent_value").ToString()
            Dim surveyName As RadTextBox = CType(e.Item.Cells(0).FindControl("txt_survey"), RadTextBox)
            surveyName.Text = DataBinder.Eval(e.Item.DataItem, "survey_name").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_measurement_survey").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_measurement_survey").ToString())
        End If
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim sql As String = ""

        For Each row In Me.grd_surveys.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_measurement_survey")
                'Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)
                Dim surveyName As RadTextBox = CType(row.Cells(0).FindControl("txt_survey"), RadTextBox)
                sql = "UPDATE ins_measurement_survey SET survey_name = '" & surveyName.Text & "'"
                sql &= " WHERE id_measurement_survey= " & IDInstrumentoID.ToString

                Using dbEntities As New dbRMS_JIEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
        Next
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/assessment/frm_measurement_surveys"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/assessment/frm_measurement_cat1"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub grd_surveys_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_surveys.PageIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Me.grd_surveys.DataSource = dbEntities.ins_measurement_survey.ToList()
            Me.grd_surveys.DataBind()
        End Using
    End Sub

    Protected Sub grd_surveys_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_surveys.PageSizeChanged
        Using dbEntities As New dbRMS_JIEntities
            Me.grd_surveys.DataSource = dbEntities.ins_measurement_survey.ToList()
            Me.grd_surveys.DataBind()
        End Using
    End Sub

    Function validation() As Double
        Dim totalPercent As Double = 0
        For Each row In Me.grd_surveys.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim toal_percent As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_value"), RadNumericTextBox)
                totalPercent = totalPercent + toal_percent.Value
            End If
        Next
        Return totalPercent
    End Function
End Class