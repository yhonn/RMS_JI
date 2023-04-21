Imports Telerik.Web.UI
Imports ly_SIME
Imports ClosedXML.Excel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class frm_measurementDetail
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "SKILLS_DETAILS"
    Dim db As New dbRMS_LAC_Entities
    Public MaxValue As Double = 0
    Public chartMax As String = ""
    Public chart As String = ""
    Public chartEndLine As String = ""
    Public showGraphic As String = ""

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim listaControles As New List(Of vw_ctrl_language)
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Sub fillGrid()
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
        Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))
        Me.grd_cate.DataSource = db.vw_ins_measurement_detail.Where(Function(p) p.id_measurement = id_measurement And _
                                                                 (p.sex_type.Contains(Me.txt_doc.Text))) _
                                                             .OrderByDescending(Function(p) p.id_measurement_detail).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

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
            'listaControles = controles.listadoControles()

        End If

        Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))

        If Not Me.IsPostBack Then

            fillGrid()

            Using dbEntities As New dbRMS_LAC_Entities

                Dim measurement = dbEntities.vw_ins_measurement.FirstOrDefault(Function(p) p.id_measurement = id_measurement)
                Me.lbl_school_name.Text = measurement.name
                Me.lbl_moderator.Text = measurement.moderator
                Me.lbl_answer_date.Text = measurement.answer_date.Value.ToString("d", cl_user.regionalizacionCulture)
                Me.lbl_participant_number.Text = measurement.participant_number
                Me.lbl_survey_name.Text = measurement.survey_name
                Me.lbl_School_type.Text = measurement.school_type

                Me.btn_export_template.NavigateUrl = "~/SkillsAssessment/frm_measurement_template?id=" & id_measurement


                Dim themeValuesMAX = db.vw_ins_measurement_valuepertheme_MAX.Where(Function(p) p.id_measurement_survey = measurement.id_measurement_survey) _
                                    .OrderBy(Function(p) p.order_theme) _
                                    .Select(Function(p) New With {Key .theme_name = p.theme_name,
                                                                       .Value = p.max_value}).ToList()
                chartMax = ""
                MaxValue = 0
                For Each item In themeValuesMAX
                    Dim total = Convert.ToString(item.Value)
                    MaxValue = If(total > MaxValue, total, MaxValue)
                    chartMax = chartMax + "{name:'" + item.theme_name + "', y: " + total + "}, "
                Next



                If measurement.baseline = 1 Then

                    Dim themeValuesList = db.vw_ins_measurement_valuepertheme_school.Where(Function(p) (p.id_measurement = id_measurement And p.baseline = 1) Or (p.id_measurement = 0 And p.id_measurement_survey = measurement.id_measurement_survey)) _
                                      .OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme) _
                                      .Select(Function(p) New With {.Key = p.theme_name, .Value = p.ValuePerTheme}).ToList()
                    For Each item In themeValuesList
                        Dim total = Convert.ToString(item.Value)
                        chart = chart + "{name:'" + item.Key + "', y: " + total + "}, "
                    Next

                Else

                    Dim themeValuesList = db.vw_ins_measurement_valuepertheme_school.Where(Function(p) (p.id_organization = measurement.id_organization And p.baseline = 1) Or (p.id_measurement = 0 And p.id_measurement_survey = measurement.id_measurement_survey)) _
                                      .OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme) _
                                      .Select(Function(p) New With {.Key = p.theme_name, .Value = p.ValuePerTheme}).ToList()

                    Dim themeValuesListEnd = db.vw_ins_measurement_valuepertheme_school.Where(Function(p) (p.id_measurement = id_measurement And p.baseline <> 1) Or (p.id_measurement = 0 And p.id_measurement_survey = measurement.id_measurement_survey)) _
                                          .OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme) _
                                          .Select(Function(p) New With {.Key = p.theme_name, .Value = p.ValuePerTheme}).ToList()

                    For Each item In themeValuesList
                        Dim total = Convert.ToString(item.Value)
                        chart = chart + "{name:'" + item.Key + "', y: " + total + "}, "
                    Next

                    For Each item In themeValuesListEnd
                        Dim total = Convert.ToString(item.Value)
                        chartEndLine = chartEndLine + "{name:'" + item.Key + "', y: " + total + "}, "
                    Next

                End If

                'For Each item In themeValuesList
                '    Dim total = Convert.ToString(item.Value)
                '    chart = chart + "{name:'" + item.Key + "', y: " + total + "}, "
                'Next

                'For Each item In themeValuesListEnd
                '    Dim total = Convert.ToString(item.Value)
                '    chartEndLine = chartEndLine + "{name:'" + item.Key + "', y: " + total + "}, "
                'Next

                If db.vw_ins_measurement_valuepertheme_school.Where(Function(p) p.id_measurement = id_measurement).Count() = 0 Then
                    showGraphic = "hidden"
                Else
                    Me.btn_upload_template.Visible = False
                End If

                Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?idMdet=" & id_measurement

            End Using
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_measurement_detail = DataBinder.Eval(e.Item.DataItem, "id_measurement_detail").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", id_measurement_detail)
            hlnkDelete.Attributes.Add("data-identity", id_measurement_detail)

            'Dim hlk_is_new As Label = New Label
            'hlk_is_new = CType(e.Item.FindControl("hlk_is_new"), Label)

            'If DataBinder.Eval(e.Item.DataItem, "is_new").ToString().Equals("1") Then
            '    hlk_is_new.Text = "Continuing"
            'Else
            '    hlk_is_new.Text = "New"
            'End If

            'Dim hlnkEdit As HyperLink = New HyperLink
            'hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            'hlnkEdit.NavigateUrl = "~/Instrumentos/frm_measurementDetailEdit?id=" & id_measurement_detail

            'Dim hlnkEstado As HyperLink = New HyperLink
            'hlnkEstado = CType(e.Item.FindControl("col_hlk_estado"), HyperLink)
            'hlnkEstado.ToolTip = controles.iconosGrid("col_hlk_estado")


            'Dim validadoME = DataBinder.Eval(e.Item.DataItem, "sincronizado")
            'If validadoME Is Nothing Or Not Convert.ToBoolean(validadoME) Then
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            'ElseIf Convert.ToBoolean(validadoME) Then
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/drop-yes.gif"
            '    hlnkDelete.Visible = False
            '    hlnkEdit.Visible = False
            'End If


            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "frm_beneficiaryRep.aspx?Id=" & id_measurement_detail
            'If listaControles.Where(Function(p) p.ctrl_code = "col_hlk_Print").Count() > 0 Then
            '    hlnkPrint.ToolTip = listaControles.FirstOrDefault(Function(p) p.ctrl_code = "col_hlk_Print").valor
            'End If
            'hlnkPrint.Visible = False

            'Dim hlnkRecount As CheckBox = New CheckBox
            'hlnkRecount = CType(e.Item.FindControl("chkActivo"), CheckBox)
            'If DataBinder.Eval(e.Item.DataItem, "sincronizado") IsNot Nothing Then
            '    If DataBinder.Eval(e.Item.DataItem, "sincronizado").ToString().Equals("True") Then
            '        hlnkRecount.Visible = False
            '        hlnkRecount.ToolTip = "Already Counted"
            '    End If
            'End If

            'If DataBinder.Eval(e.Item.DataItem, "recount").ToString() = "True" Then
            '    hlnkRecount.ToolTip = controles.iconosGrid("col_hlk_activo")
            '    hlnkRecount.Checked = True
            'Else
            '    hlnkRecount.Checked = False
            '    hlnkRecount.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            'End If
        End If
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_LAC_Entities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM vw_ins_measurement_detail WHERE id_measurement_detail = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
            Me.Response.Redirect("~/Instrumentos/frm_beneficiaryTechnologies")
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub


    Protected Sub grd_cate_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles grd_cate.SortCommand
        fillGrid()
    End Sub

    Protected Sub btn_upload_template_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_upload_template.Click
        Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))
        Me.Response.Redirect("~/SkillsAssessment/frm_measurementDetailAD?id=" & id_measurement)
    End Sub

    'Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim row As GridItem
    '    Dim sql = ""
    '    Dim Activo As String = ""
    '    cnnME.Open()
    '    For Each row In Me.grd_cate.Items
    '        If TypeOf row Is GridDataItem Then
    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
    '            Dim id_job_created_detail As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_job_created_detail"))
    '            Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkActivo"), CheckBox)
    '            Activo = 1
    '            If chkvisible.Checked = False Then
    '                Activo = 0
    '            End If
    '            sql = "UPDATE ins_job_created_detail SET recount=" & Activo & " WHERE id_job_created_detail=" & id_job_created_detail
    '            Dim dm As New SqlDataAdapter(sql, cnnME)
    '            Dim ds As New DataSet("IdActivo")
    '            dm.Fill(ds, "IdActvo")
    '        End If
    '    Next
    '    fillGrid()
    '    cnnME.Close()
    'End Sub
End Class