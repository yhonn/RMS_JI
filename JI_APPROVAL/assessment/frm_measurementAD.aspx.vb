Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_measurementAD
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "SKILLS_MAIN_AD"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim helper As New ly_APPROVAL.APPROVAL.HelperMethods

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        If Not Me.IsPostBack Then
            LoadList()
        End If
    End Sub
    Sub LoadList()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_type_org = {1005, 1006, 1007}
            helper.SetValues(Me.cmb_organization, dbEntities.tme_organization.Where(Function(p) id_type_org.Contains(p.id_organization_type)).ToList(), "name", "id_organization")
            helper.SetValues(Me.cmb_survey_type, dbEntities.ins_measurement_survey.ToList(), "survey_name", "id_measurement_survey")
            ' helper.SetValues(Me.cmb_typeSchool, dbEntities.tme_school_type.ToList(), "school_type", "id_school_type")
        End Using
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim errSave As Boolean = False
        Dim idFicha = Convert.ToInt32(Me.Session("E_IdFicha"))
        If errSave = False Then
            Using dbEntities As New dbRMS_JIEntities
                Dim oMeasurement As New ins_measurement
                oMeasurement.id_organization = Me.cmb_organization.SelectedValue
                oMeasurement.id_measurement_survey = Me.cmb_survey_type.SelectedValue
                oMeasurement.answer_date = Me.dp_answer_date.SelectedDate
                oMeasurement.moderator = Me.txt_moderator.Text
                oMeasurement.participant_number = Me.txt_participant_number.Value
                oMeasurement.recount = True

                If dbEntities.vw_ins_measurement.Where(Function(p) p.id_organization = oMeasurement.id_organization And p.id_measurement_survey = oMeasurement.id_measurement_survey).Count() = 0 Then
                    oMeasurement.baseline = 1
                Else
                    oMeasurement.baseline = 2
                End If

                oMeasurement.id_school_type = Me.cmb_typeSchool.SelectedValue

                Dim sub_region = dbEntities.vw_ins_organization.FirstOrDefault(Function(p) p.id_organization = Me.cmb_organization.SelectedValue).id_subregion
                Dim period = dbEntities.vw_t_periodos.FirstOrDefault(Function(p) p.id_subregion = sub_region And p.activo.Value = True).id_periodo

                oMeasurement.id_periodo = period

                oMeasurement.fecha_crea = Date.UtcNow
                oMeasurement.id_usuario_crea = Me.Session("E_IdUser").ToString()
                oMeasurement.id_ficha_proyecto = idFicha

                dbEntities.ins_measurement.Add(oMeasurement)
                dbEntities.SaveChanges()

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/assessment/frm_measurementDetail?Id=" & oMeasurement.id_measurement
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        End If
    End Sub
End Class