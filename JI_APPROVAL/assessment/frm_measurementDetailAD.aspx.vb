Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_measurementDetailAD
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "SKILLS_DETAILS_AD"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim helper As New ly_APPROVAL.APPROVAL.HelperMethods
    Dim strQuestionMising As String

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
            Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))
            Using dbEntities As New dbRMS_JIEntities
                Dim measurement = dbEntities.vw_ins_measurement.FirstOrDefault(Function(p) p.id_measurement = id_measurement)
                Me.lbl_school_name.Text = measurement.name
                Me.lbl_moderator.Text = measurement.moderator
                Me.lbl_answer_date.Text = measurement.answer_date.Value.ToString("d", cl_user.regionalizacionCulture)
                Me.lbl_participant_number.Text = measurement.participant_number
                Me.lbl_survey_name.Text = measurement.survey_name
                Me.lbl_survey_type.Text = measurement.id_measurement_survey
            End Using
        End If
    End Sub
    Sub LoadList()
        Using dbEntities As New dbRMS_JIEntities
            'helper.SetValues(Me.cmb_organization, dbEntities.tme_organization.ToList(), "name", "id_organization")
            'helper.SetValues(Me.cmb_survey_type, dbEntities.ins_measurement_survey.ToList(), "survey_name", "id_measurement_survey")
        End Using
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_JIEntities


            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            For Each file As UploadedFile In asyncArchivo.UploadedFiles
                Dim nombreArchivo = cl_listados.getNewName(file, Me.Session("E_IdUser").ToString())
                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).documents_folder)
                file.SaveAs(Path + nombreArchivo)
                Me.lbl_archivo_uploaded.Text = Path + nombreArchivo
            Next

            Dim id_reporte = Convert.ToInt32(Me.Request.QueryString("id"))
            Dim id_survey = dbEntities.vw_ins_measurement.FirstOrDefault(Function(p) p.id_measurement = id_reporte).id_measurement_survey

            Dim resp As Boolean
            If id_survey = 6 Then ' New Aflatoon
                resp = sincronizarAFLATON()
            Else
                resp = sincronizar()
            End If


            If resp Then
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/assessment/frm_measurementDetail?id=" & Me.Request.QueryString("id")
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            Else
                Me.lbl_archivo_uploaded.Text = "There are some answers that are not in the scale options of some questions, check them before uploading the excel file in the system again. <br /> Detail: <br /> " & strQuestionMising
            End If

        End Using
    End Sub

    Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
    End Sub

    Function sincronizarAFLATON() As Boolean

        Me.lbl_error.Visible = False
        Dim Sql As String = ""
        Dim noError = True
        Try
            Dim SFileEstension As String = System.IO.Path.GetExtension(Me.lbl_archivo_uploaded.Text)
            Dim file_info As New IO.FileInfo(Me.lbl_archivo_uploaded.Text)
            Dim FileUpload As String = Me.lbl_archivo_uploaded.Text
            Using dbEntities As New dbRMS_JIEntities

                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_periodo = 0
                'Dim Coneccion As String = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" & FileUpload & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
                Dim Coneccion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & FileUpload & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
                'Dim Coneccion As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & FileUpload & ";Extended Properties=Excel 8.0;"
                Dim cnnExcel As New OleDb.OleDbConnection(Coneccion)
                cnnExcel.Open()
                Dim ds As New DataSet("DsDatosExcel")
                'SELECCIONA LOS DATOS FILTRANDO POR LA CEDULA 
                Dim dm As New OleDb.OleDbDataAdapter("SELECT TOP " & (Convert.ToInt32(Me.lbl_participant_number.Text) + 6) & " * FROM [TemplateSkills$] ", cnnExcel)
                'Dim dm As New OleDb.OleDbDataAdapter("SELECT * FROM [TemplateSkills$] " & filtro, cnnExcel)

                dm.Fill(ds, "DsDatosExcel")
                Dim KK = ds.Tables("DsDatosExcel").Rows.Count
                'VERIFCA QUE LA LISTA NO ESTE A CERO PARTICIPANTES

                'VERIFICA EXISTENCIA Y TIPO DE ARCHIVO
                If file_info.Exists = False Or SFileEstension <> ".xlsx" Then
                    'SINO EXISTE MUESTRA UN MENSAJE
                    Me.lbl_error.Visible = True
                Else
                    'SI EXISTE SE PROCESAN LOS DATOS DEL XLSX


                    Dim sex_type = dbEntities.tme_sex_type.ToList()
                    'Dim class_level = dbEntities.tme_class_level.ToList()
                    Dim school_status = dbEntities.tme_schooling_status.ToList()
                    Dim answer_options = dbEntities.tme_measurement_answer_option.ToList()
                    'Dim configuration = dbEntities.vw_ins_measurement_detail_options.ToList()
                    Dim oGrupoEdad = dbEntities.tme_group_age.ToList()
                    Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))
                    Dim survey_type = Convert.ToInt32(Me.lbl_survey_type.Text)
                    Dim oConteo = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = survey_type).ToList()
                    'Dim oListAnswers = dbEntities.vw_ins_measurement_detail_options.Where(Function(p) p.id_measurement = id_measurement).ToList()


                    Dim row = 1
                    Dim a = ds.Tables("DsDatosExcel").Rows.Count
                    strQuestionMising = ""

                    For Each registro In ds.Tables("DsDatosExcel").Rows
                        If row >= 7 And row <= Convert.ToInt32(Me.lbl_participant_number.Text) + 7 Then
                            Dim oInsDetail = New ins_measurement_detail

                            If Not IsDBNull(registro.Item(1)) Then 'Id_Student
                                oInsDetail.id_estudent = registro.Item(1).trim
                            End If


                            If sex_type.Where(Function(p) p.sex_type.Trim().ToLower() = registro.Item(2).Trim().ToLower()).Count() = 1 Then
                                oInsDetail.id_sex_type = sex_type.FirstOrDefault(Function(p) p.sex_type.Trim().ToLower() = registro.Item(2).Trim().ToLower()).id_sex_type
                            End If


                            'If school_status.Where(Function(p) p.schooling_status.Trim().ToLower() = registro.Item(3).Trim().ToLower()).Count() = 1 Then
                            '    Dim id_schooling = school_status.FirstOrDefault(Function(p) p.schooling_status.Trim().ToLower() = registro.Item(3).Trim().ToLower()).id_schooling_status
                            '    If class_level.Where(Function(p) p.class_level_name.Trim().ToLower() = registro.Item(4).Trim().ToLower() And p.id_schooling_status = id_schooling).Count() = 1 Then
                            '        oInsDetail.id_class_level = class_level.FirstOrDefault(Function(p) p.class_level_name.Trim().ToLower() = registro.Item(4).Trim().ToLower()).id_class_level
                            '    End If
                            'End If

                            Dim id_group = oGrupoEdad.Where(Function(p) (p.valorMenor.HasValue And p.valorMayor.HasValue))
                            id_group = id_group.Where(Function(p) p.valorMenor.Value <= registro.Item(5) And p.valorMayor.Value >= registro.Item(5))
                            oInsDetail.age = Convert.ToInt32(registro.Item(5))

                            If id_group.Count > 0 Then
                                oInsDetail.id_group_age = id_group.FirstOrDefault().id_group_age
                            Else
                                oInsDetail.id_group_age = 1
                            End If
                            oInsDetail.fecha_crea = Date.UtcNow
                            oInsDetail.id_usuario_crea = Me.Session("E_IdUser").ToString()

                            oInsDetail.id_measurement = id_measurement

                            For index = 6 To oConteo.Count() + 5

                                Dim oInsPregunta As New ins_measurement_answer
                                Dim aaaaa = ds.Tables("DsDatosExcel").Rows(3)
                                Dim bbbbb = ds.Tables("DsDatosExcel").Rows(5)
                                Dim question = Convert.ToInt32(aaaaa.Item(index))
                                Dim strQuestion = bbbbb.Item(index).ToString
                                If question = 89 Then
                                    Dim asdasdasgdfasgh = 1
                                End If

                                Dim answer = Convert.ToString(registro.Item(index).ToString().TrimEnd())
                                'Dim answer_option = oListAnswers.Where(Function(p) p.id_measurement_question = question And p.option_name.Trim().ToLower() = answer.Trim().ToLower())
                                Dim answer_option = dbEntities.vw_ins_measurement_detail_options.Where(Function(p) p.id_measurement = id_measurement And p.id_measurement_question = question And p.option_name.Trim().ToLower() = answer.Trim().ToLower()).ToList()
                                If answer_option.Count() = 1 Then
                                    oInsPregunta.id_measurement_answer_option = answer_option.FirstOrDefault().id_measurement_answer_option
                                End If
                                oInsPregunta.id_measurement_question_config = oConteo.FirstOrDefault(Function(p) p.id_measurement_question = question).id_measurement_question_config
                                oInsDetail.ins_measurement_answer.Add(oInsPregunta)
                                If Not oInsPregunta.id_measurement_answer_option.HasValue Then
                                    noError = False
                                    strQuestionMising &= strQuestion & "<br />"
                                End If

                            Next
                            dbEntities.ins_measurement_detail.Add(oInsDetail)


                        End If

                        row = row + 1
                    Next

                    If noError Then
                        dbEntities.SaveChanges()
                        cnnExcel.Close()
                        Return True
                    Else
                        cnnExcel.Close()
                        Return False
                    End If


                End If
            End Using
        Catch ex As Exception

            Me.lbl_error.Visible = True
            Me.lbl_error.Text = ex.Message.Trim & ":" & Sql
            Return False
        End Try
        Return False
    End Function



    Function sincronizar() As Boolean

        Me.lbl_error.Visible = False
        Dim Sql As String = ""
        Dim noError = True
        Try
            Dim SFileEstension As String = System.IO.Path.GetExtension(Me.lbl_archivo_uploaded.Text)
            Dim file_info As New IO.FileInfo(Me.lbl_archivo_uploaded.Text)
            Dim FileUpload As String = Me.lbl_archivo_uploaded.Text
            Using dbEntities As New dbRMS_JIEntities

                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                Dim id_periodo = 0
                'Dim Coneccion As String = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" & FileUpload & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
                Dim Coneccion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & FileUpload & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1"""
                'Dim Coneccion As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & FileUpload & ";Extended Properties=Excel 8.0;"
                Dim cnnExcel As New OleDb.OleDbConnection(Coneccion)
                cnnExcel.Open()
                Dim ds As New DataSet("DsDatosExcel")
                'SELECCIONA LOS DATOS FILTRANDO POR LA CEDULA 
                Dim dm As New OleDb.OleDbDataAdapter("SELECT TOP " & (Convert.ToInt32(Me.lbl_participant_number.Text) + 6) & " * FROM [TemplateSkills$] ", cnnExcel)
                'Dim dm As New OleDb.OleDbDataAdapter("SELECT * FROM [TemplateSkills$] " & filtro, cnnExcel)

                dm.Fill(ds, "DsDatosExcel")
                Dim KK = ds.Tables("DsDatosExcel").Rows.Count
                'VERIFCA QUE LA LISTA NO ESTE A CERO PARTICIPANTES

                'VERIFICA EXISTENCIA Y TIPO DE ARCHIVO
                If file_info.Exists = False Or SFileEstension <> ".xlsx" Then
                    'SINO EXISTE MUESTRA UN MENSAJE
                    Me.lbl_error.Visible = True
                Else
                    'SI EXISTE SE PROCESAN LOS DATOS DEL XLSX


                    Dim sex_type = dbEntities.tme_sex_type.ToList()
                    Dim class_level = Nothing ' dbEntities.tme_class_level.ToList()
                    Dim school_status = dbEntities.tme_schooling_status.ToList()
                    Dim answer_options = dbEntities.tme_measurement_answer_option.ToList()
                    'Dim configuration = dbEntities.vw_ins_measurement_detail_options.ToList()
                    Dim oGrupoEdad = dbEntities.tme_group_age.ToList()
                    Dim id_measurement = Convert.ToInt32(Me.Request.QueryString("id"))
                    Dim survey_type = Convert.ToInt32(Me.lbl_survey_type.Text)
                    Dim oConteo = dbEntities.ins_measurement_question_config.Where(Function(p) p.id_measurement_survey = survey_type).ToList()
                    'Dim oListAnswers = dbEntities.vw_ins_measurement_detail_options.Where(Function(p) p.id_measurement = id_measurement).ToList()


                    Dim row = 1
                    Dim a = ds.Tables("DsDatosExcel").Rows.Count
                    For Each registro In ds.Tables("DsDatosExcel").Rows
                        If row >= 7 And row <= Convert.ToInt32(Me.lbl_participant_number.Text) + 7 Then
                            Dim oInsDetail = New ins_measurement_detail
                            If sex_type.Where(Function(p) p.sex_type.Trim().ToLower() = registro.Item(1).Trim().ToLower()).Count() = 1 Then
                                oInsDetail.id_sex_type = sex_type.FirstOrDefault(Function(p) p.sex_type.Trim().ToLower() = registro.Item(1).Trim().ToLower()).id_sex_type
                            End If


                            If school_status.Where(Function(p) p.schooling_status.Trim().ToLower() = registro.Item(2).Trim().ToLower()).Count() = 1 Then
                                Dim id_schooling = school_status.FirstOrDefault(Function(p) p.schooling_status.Trim().ToLower() = registro.Item(2).Trim().ToLower()).id_schooling_status
                                If class_level.Where(Function(p) p.class_level_name.Trim().ToLower() = registro.Item(3).Trim().ToLower() And p.id_schooling_status = id_schooling).Count() = 1 Then
                                    oInsDetail.id_class_level = class_level.FirstOrDefault(Function(p) p.class_level_name.Trim().ToLower() = registro.Item(3).Trim().ToLower()).id_class_level
                                End If
                            End If


                            Dim id_group = oGrupoEdad.Where(Function(p) (p.valorMenor.HasValue And p.valorMayor.HasValue))
                            id_group = id_group.Where(Function(p) p.valorMenor.Value <= registro.Item(4) And p.valorMayor.Value >= registro.Item(4))
                            oInsDetail.age = Convert.ToInt32(registro.Item(4))
                            If id_group.Count > 0 Then
                                oInsDetail.id_group_age = id_group.FirstOrDefault().id_group_age
                            Else
                                oInsDetail.id_group_age = 1
                            End If
                            oInsDetail.fecha_crea = Date.UtcNow
                            oInsDetail.id_usuario_crea = Me.Session("E_IdUser").ToString()

                            oInsDetail.id_measurement = id_measurement
                            For index = 5 To oConteo.Count() + 4

                                Dim oInsPregunta As New ins_measurement_answer
                                Dim aaaaa = ds.Tables("DsDatosExcel").Rows(3)
                                Dim question = Convert.ToInt32(aaaaa.Item(index))
                                If question = 89 Then
                                    Dim asdasdasgdfasgh = 1
                                End If

                                Dim answer = Convert.ToString(registro.Item(index).ToString().TrimEnd())
                                'Dim answer_option = oListAnswers.Where(Function(p) p.id_measurement_question = question And p.option_name.Trim().ToLower() = answer.Trim().ToLower())
                                Dim answer_option = dbEntities.vw_ins_measurement_detail_options.Where(Function(p) p.id_measurement = id_measurement And p.id_measurement_question = question And p.option_name.Trim().ToLower() = answer.Trim().ToLower()).ToList()
                                If answer_option.Count() = 1 Then
                                    oInsPregunta.id_measurement_answer_option = answer_option.FirstOrDefault().id_measurement_answer_option
                                End If
                                oInsPregunta.id_measurement_question_config = oConteo.FirstOrDefault(Function(p) p.id_measurement_question = question).id_measurement_question_config
                                oInsDetail.ins_measurement_answer.Add(oInsPregunta)
                                If Not oInsPregunta.id_measurement_answer_option.HasValue Then
                                    noError = False
                                End If

                            Next
                            dbEntities.ins_measurement_detail.Add(oInsDetail)


                        End If

                        row = row + 1
                    Next

                    If noError Then
                        dbEntities.SaveChanges()
                        cnnExcel.Close()
                        Return True
                    Else
                        cnnExcel.Close()
                        Return False
                    End If


                End If
            End Using
        Catch ex As Exception

            Me.lbl_error.Visible = True
            Me.lbl_error.Text = ex.Message.Trim & ":" & Sql
            Return False
        End Try
        Return False
    End Function


End Class