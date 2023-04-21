Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Configuration
Imports ly_SIME
Imports ly_RMS
Imports System.Globalization

Namespace APPROVAL



    Public Class cls_solicitations

        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Public cl_user As ly_SIME.CORE.cls_user
        Dim Sql As String
        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)



        Public Sub New(ByVal idP As Integer, Optional user_ As ly_SIME.CORE.cls_user = Nothing)
            id_programa = idP
            cl_user = user_
        End Sub

        Public Function SAVE_TA_APPLY_EVALUATION(ByVal ta_eval As TA_APPLY_EVALUATION, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim TA_APPLY_EVALUATION_upd As TA_APPLY_EVALUATION

                    If ID = 0 Then

                        db.TA_APPLY_EVALUATION.Add(ta_eval)

                    Else

                        TA_APPLY_EVALUATION_upd = db.TA_APPLY_EVALUATION.Find(ID)

                        'TA_APPLY_EVALUATION_upd.ID_EVALUATION_STATUS = ta_eval.ID_EVALUATION_STATUS
                        TA_APPLY_EVALUATION_upd.EVALUATION_START_DATE = ta_eval.EVALUATION_START_DATE
                        TA_APPLY_EVALUATION_upd.EVALUATION_END_DATE = ta_eval.EVALUATION_END_DATE
                        TA_APPLY_EVALUATION_upd.EVALUATION_DESCRIPTION = ta_eval.EVALUATION_DESCRIPTION
                        TA_APPLY_EVALUATION_upd.TOT_ROUNDS = ta_eval.TOT_ROUNDS
                        TA_APPLY_EVALUATION_upd.ID_USUARIO_UPDATE = ta_eval.ID_USUARIO_UPDATE
                        TA_APPLY_EVALUATION_upd.FECHA_UPDATE = ta_eval.FECHA_UPDATE
                        TA_APPLY_EVALUATION_upd.USER_TOKEN_UPDATE = ta_eval.USER_TOKEN_UPDATE

                        db.Entry(TA_APPLY_EVALUATION_upd).State = Entity.EntityState.Modified


                    End If


                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = ta_eval.ID_APPLY_EVALUATION
                        Else
                            result = TA_APPLY_EVALUATION_upd.ID_APPLY_EVALUATION
                        End If

                        SAVE_TA_APPLY_EVALUATION = result

                    Else

                        SAVE_TA_APPLY_EVALUATION = "-1"

                    End If


                End Using

            Catch ex As Exception

                SAVE_TA_APPLY_EVALUATION = ex.Message

            End Try


        End Function


        Public Function Save_TA_APPLY_APP(ByVal ta_apply As TA_APPLY_APP, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim TA_APPLY_APP_upd As TA_APPLY_APP

                    If ID = 0 Then

                        db.TA_APPLY_APP.Add(ta_apply)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        TA_APPLY_APP_upd = db.TA_APPLY_APP.Find(ID)

                        TA_APPLY_APP_upd.ID_APPLY_STATUS = ta_apply.ID_APPLY_STATUS
                        TA_APPLY_APP_upd.APPLY_DATE = ta_apply.APPLY_DATE
                        TA_APPLY_APP_upd.APPLY_DESCRIPTION = ta_apply.APPLY_DESCRIPTION
                        TA_APPLY_APP_upd.USER_TOKEN_UPDATE = ta_apply.USER_TOKEN_UPDATE


                        'If Deliverable.fecha_aprobo.HasValue Then
                        '    ta_deliverable_upd.fecha_aprobo = Deliverable.fecha_aprobo
                        'End If

                        'If Deliverable.fecha_entrego.HasValue Then
                        '    ta_deliverable_upd.fecha_entrego = Deliverable.fecha_entrego
                        'End If

                        'If Deliverable.id_deliverable_estado = 3 Or Deliverable.id_deliverable_estado = 4 Or Deliverable.id_deliverable_estado = 6 Then

                        '    ta_deliverable_upd.fecha_complete = Deliverable.fecha_complete
                        '    ta_deliverable_upd.usuario_complete = Deliverable.usuario_complete

                        'End If

                        db.Entry(TA_APPLY_APP_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then


                        If ID = 0 Then
                            result = ta_apply.ID_APPLY_APP
                        Else
                            result = TA_APPLY_APP_upd.ID_APPLY_APP
                        End If

                        Save_TA_APPLY_APP = result

                    Else
                        Save_TA_APPLY_APP = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_TA_APPLY_APP = ex.Message

            End Try

        End Function


        Public Function Save_TA_EVALUATION_APP(ByVal ta_eval_app As TA_EVALUATION_APP, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim ta_eval_app_upd As TA_EVALUATION_APP

                    If ID = 0 Then

                        db.TA_EVALUATION_APP.Add(ta_eval_app)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        ta_eval_app_upd = db.TA_EVALUATION_APP.Find(ID)

                        ta_eval_app_upd.ID_EVALUATION_ROUND = ta_eval_app.ID_EVALUATION_ROUND
                        ta_eval_app_upd.ID_APPLY_APP = ta_eval_app.ID_APPLY_APP
                        ta_eval_app_upd.EVALUATION_START_DATE = ta_eval_app.EVALUATION_START_DATE
                        ta_eval_app_upd.EVALUATION_END_DATE = ta_eval_app.EVALUATION_END_DATE
                        ta_eval_app_upd.ID_EVALUATION_APP_STATUS = ta_eval_app.ID_EVALUATION_APP_STATUS
                        ta_eval_app_upd.EVALUATION_UNTIED = ta_eval_app.EVALUATION_UNTIED
                        ta_eval_app_upd.EVALUATION_SCORE = ta_eval_app.EVALUATION_SCORE
                        ta_eval_app_upd.EVALUATION_VOTES = ta_eval_app.EVALUATION_VOTES
                        ta_eval_app_upd.ID_USUARIO_UPDATE = ta_eval_app.ID_USUARIO_UPDATE
                        ta_eval_app_upd.FECHA_UPDATE = ta_eval_app.FECHA_UPDATE
                        ta_eval_app_upd.USER_TOKEN_UPDATE = ta_eval_app.USER_TOKEN_UPDATE

                        'If Deliverable.fecha_aprobo.HasValue Then
                        '    ta_deliverable_upd.fecha_aprobo = Deliverable.fecha_aprobo
                        'End If

                        'If Deliverable.fecha_entrego.HasValue Then
                        '    ta_deliverable_upd.fecha_entrego = Deliverable.fecha_entrego
                        'End If

                        'If Deliverable.id_deliverable_estado = 3 Or Deliverable.id_deliverable_estado = 4 Or Deliverable.id_deliverable_estado = 6 Then

                        '    ta_deliverable_upd.fecha_complete = Deliverable.fecha_complete
                        '    ta_deliverable_upd.usuario_complete = Deliverable.usuario_complete

                        'End If

                        db.Entry(ta_eval_app_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = ta_eval_app.ID_EVALUATION_APP
                        Else
                            result = ta_eval_app_upd.ID_EVALUATION_APP
                        End If

                        Save_TA_EVALUATION_APP = result

                    Else
                        Save_TA_EVALUATION_APP = "-1"
                    End If





                End Using

            Catch ex As Exception

                Save_TA_EVALUATION_APP = ex.Message

            End Try

        End Function


        Public Function Save_TA_AWARDED_APP(ByVal ta_awarded_app As TA_AWARDED_APP, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim ta_awarded_app_upd As TA_AWARDED_APP

                    If ID = 0 Then

                        db.TA_AWARDED_APP.Add(ta_awarded_app)


                    Else

                        ta_awarded_app_upd = db.TA_AWARDED_APP.Find(ID)

                        ta_awarded_app_upd.ID_APPLY_APP = ta_awarded_app.ID_APPLY_APP
                        ta_awarded_app_upd.ID_ACTIVITY = ta_awarded_app.ID_ACTIVITY
                        ta_awarded_app_upd.ID_ORGANIZATION_APP = ta_awarded_app.ID_ORGANIZATION_APP
                        ta_awarded_app_upd.ID_EVALUATION_APP = ta_awarded_app.ID_EVALUATION_APP
                        ta_awarded_app_upd.ID_USUARIO_CREA = ta_awarded_app.ID_USUARIO_CREA
                        ta_awarded_app_upd.FECHA_CREA = ta_awarded_app.FECHA_CREA
                        ta_awarded_app_upd.TOTAL_AMOUNT = ta_awarded_app.TOTAL_AMOUNT
                        ta_awarded_app_upd.EXCHANGE_RATE = ta_awarded_app.EXCHANGE_RATE

                        db.Entry(ta_awarded_app_upd).State = Entity.EntityState.Modified

                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = ta_awarded_app.ID_AWARDED_APP
                        Else
                            result = ta_awarded_app_upd.ID_AWARDED_APP
                        End If

                        Save_TA_AWARDED_APP = result

                    Else
                        Save_TA_AWARDED_APP = "-1"
                    End If





                End Using

            Catch ex As Exception

                Save_TA_AWARDED_APP = ex.Message

            End Try

        End Function



        Public Function Save_TA_AWARDED_ACTIVITY(ByVal TA_AWARDED_ACTIVITY As TA_AWARDED_ACTIVITY, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim TA_AWARDED_ACTIVITY_upd As TA_AWARDED_ACTIVITY

                    If ID = 0 Then

                        db.TA_AWARDED_ACTIVITY.Add(TA_AWARDED_ACTIVITY)

                    Else

                        TA_AWARDED_ACTIVITY_upd = db.TA_AWARDED_ACTIVITY.Find(ID)

                        TA_AWARDED_ACTIVITY_upd.ID_AWARDED_APP = TA_AWARDED_ACTIVITY.ID_AWARDED_APP
                        TA_AWARDED_ACTIVITY_upd.id_subregion = TA_AWARDED_ACTIVITY.id_subregion
                        TA_AWARDED_ACTIVITY_upd.ID_ORGANIZATION_APP = TA_AWARDED_ACTIVITY.ID_ORGANIZATION_APP
                        TA_AWARDED_ACTIVITY_upd.id_componente = TA_AWARDED_ACTIVITY.id_componente
                        TA_AWARDED_ACTIVITY_upd.ID_ACTIVITY_STATUS = TA_AWARDED_ACTIVITY.ID_ACTIVITY_STATUS
                        TA_AWARDED_ACTIVITY_upd.id_periodo = TA_AWARDED_ACTIVITY.id_periodo
                        TA_AWARDED_ACTIVITY_upd.nombre_proyecto = TA_AWARDED_ACTIVITY.nombre_proyecto
                        TA_AWARDED_ACTIVITY_upd.area_intervencion = TA_AWARDED_ACTIVITY.area_intervencion
                        TA_AWARDED_ACTIVITY_upd.codigo_ficha_AID = TA_AWARDED_ACTIVITY.codigo_ficha_AID
                        TA_AWARDED_ACTIVITY_upd.codigo_RFA = TA_AWARDED_ACTIVITY.codigo_RFA
                        TA_AWARDED_ACTIVITY_upd.codigo_SAPME = TA_AWARDED_ACTIVITY.codigo_SAPME
                        TA_AWARDED_ACTIVITY_upd.codigo_MONITOR = TA_AWARDED_ACTIVITY.codigo_MONITOR
                        TA_AWARDED_ACTIVITY_upd.codigo_convenio = TA_AWARDED_ACTIVITY.codigo_convenio
                        TA_AWARDED_ACTIVITY_upd.numero_acta_aprobacion = TA_AWARDED_ACTIVITY.numero_acta_aprobacion
                        TA_AWARDED_ACTIVITY_upd.fecha_inicio_proyecto = TA_AWARDED_ACTIVITY.fecha_inicio_proyecto
                        TA_AWARDED_ACTIVITY_upd.fecha_fin_proyecto = TA_AWARDED_ACTIVITY.fecha_fin_proyecto
                        TA_AWARDED_ACTIVITY_upd.OBLIGATED_AMOUNT = TA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT
                        TA_AWARDED_ACTIVITY_upd.OBLIGATED_AMOUNT_LOC = TA_AWARDED_ACTIVITY.OBLIGATED_AMOUNT_LOC
                        TA_AWARDED_ACTIVITY_upd.costo_total_proyecto = TA_AWARDED_ACTIVITY.costo_total_proyecto
                        TA_AWARDED_ACTIVITY_upd.costo_total_proyecto_LOC = TA_AWARDED_ACTIVITY.costo_total_proyecto_LOC
                        TA_AWARDED_ACTIVITY_upd.tasa_cambio = TA_AWARDED_ACTIVITY.tasa_cambio
                        TA_AWARDED_ACTIVITY_upd.observaciones = TA_AWARDED_ACTIVITY.observaciones
                        TA_AWARDED_ACTIVITY_upd.aportes_actualizados = TA_AWARDED_ACTIVITY.aportes_actualizados
                        TA_AWARDED_ACTIVITY_upd.idContratoME = TA_AWARDED_ACTIVITY.idContratoME
                        TA_AWARDED_ACTIVITY_upd.ActualizacionReciente = TA_AWARDED_ACTIVITY.ActualizacionReciente
                        'TA_AWARDED_ACTIVITY_upd.datecreated = Date.UtcNow
                        ' TA_AWARDED_ACTIVITY_upd.id_usuario_creo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                        TA_AWARDED_ACTIVITY_upd.id_usuario_update = TA_AWARDED_ACTIVITY.id_usuario_update
                        TA_AWARDED_ACTIVITY_upd.dateUpdate = TA_AWARDED_ACTIVITY.dateUpdate
                        TA_AWARDED_ACTIVITY_upd.dateUpdate = TA_AWARDED_ACTIVITY.dateUpdate
                        TA_AWARDED_ACTIVITY_upd.USER_TOKEN_UPDATE = TA_AWARDED_ACTIVITY.USER_TOKEN_UPDATE
                        TA_AWARDED_ACTIVITY_upd.id_district = TA_AWARDED_ACTIVITY.id_district
                        TA_AWARDED_ACTIVITY_upd.id_sub_mecanismo = TA_AWARDED_ACTIVITY.id_sub_mecanismo
                        TA_AWARDED_ACTIVITY_upd.id_documento = TA_AWARDED_ACTIVITY.id_documento
                        TA_AWARDED_ACTIVITY_upd.id_usuario_responsable = TA_AWARDED_ACTIVITY.id_usuario_responsable
                        TA_AWARDED_ACTIVITY_upd.id_ficha_padre = TA_AWARDED_ACTIVITY.id_ficha_padre
                        TA_AWARDED_ACTIVITY_upd.id_programa = TA_AWARDED_ACTIVITY.id_programa


                        db.Entry(TA_AWARDED_ACTIVITY_upd).State = Entity.EntityState.Modified

                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = TA_AWARDED_ACTIVITY.ID_AWARDED_ACTIVITY
                        Else
                            result = TA_AWARDED_ACTIVITY.ID_AWARDED_ACTIVITY
                        End If

                        Save_TA_AWARDED_ACTIVITY = result

                    Else
                        Save_TA_AWARDED_ACTIVITY = "-1"
                    End If



                End Using

            Catch ex As Exception

                Save_TA_AWARDED_ACTIVITY = ex.Message

            End Try

        End Function



        Public Function Save_TA_AWARDED_ACTIVITY_SUB(ByVal TA_AWARDED_ACTIVITY_SUBREGION As TA_AWARDED_ACTIVITY_SUBREGION, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim TA_AWARDED_ACTIVITY_SUBREGION_upd As TA_AWARDED_ACTIVITY_SUBREGION

                    If ID = 0 Then

                        db.TA_AWARDED_ACTIVITY_SUBREGION.Add(TA_AWARDED_ACTIVITY_SUBREGION)

                    Else

                        TA_AWARDED_ACTIVITY_SUBREGION_upd = db.TA_AWARDED_ACTIVITY_SUBREGION.Find(ID)
                        TA_AWARDED_ACTIVITY_SUBREGION_upd.ID_AWARDED_ACTIVITY = TA_AWARDED_ACTIVITY_SUBREGION.ID_AWARDED_ACTIVITY
                        TA_AWARDED_ACTIVITY_SUBREGION_upd.id_subregion = TA_AWARDED_ACTIVITY_SUBREGION.id_subregion
                        TA_AWARDED_ACTIVITY_SUBREGION_upd.nivel_cobertura = TA_AWARDED_ACTIVITY_SUBREGION.nivel_cobertura

                        db.Entry(TA_AWARDED_ACTIVITY_SUBREGION_upd).State = Entity.EntityState.Modified

                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = TA_AWARDED_ACTIVITY_SUBREGION.ID_AWARDED_ACTIVITY_SUBREGION
                        Else
                            result = TA_AWARDED_ACTIVITY_SUBREGION.ID_AWARDED_ACTIVITY_SUBREGION
                        End If

                        Save_TA_AWARDED_ACTIVITY_SUB = result

                    Else
                        Save_TA_AWARDED_ACTIVITY_SUB = "-1"
                    End If



                End Using

            Catch ex As Exception

                Save_TA_AWARDED_ACTIVITY_SUB = ex.Message

            End Try

        End Function


        Public Function Set_TA_ACTIVITY_STATUS(ByVal ID_ACTIVITY As Integer, ByVal ID_STATUS As Integer) As String

            Try

                Dim result As String = 0

                Using db As New dbRMS_JIEntities

                    Dim oTA_ACTIVITY = db.TA_ACTIVITY.Find(ID_ACTIVITY)
                    Dim PrevSTATUS = ID_STATUS - 1

                    If oTA_ACTIVITY.ID_ACTIVITY_STATUS <= PrevSTATUS Then

                        oTA_ACTIVITY.ID_ACTIVITY_STATUS = ID_STATUS
                        db.Entry(oTA_ACTIVITY).State = Entity.EntityState.Modified

                        If (db.SaveChanges()) Then

                            result = oTA_ACTIVITY.id_activity
                            Set_TA_ACTIVITY_STATUS = result

                        Else
                            Set_TA_ACTIVITY_STATUS = "-1"
                        End If

                    Else

                        Set_TA_ACTIVITY_STATUS = "-1"

                    End If


                End Using

            Catch ex As Exception

                Set_TA_ACTIVITY_STATUS = "-1"

            End Try


        End Function



        Public Function Set_TA_ACTIVITY_AW_STATUS(ByVal ID_ACTIVITY_AW As Integer, ByVal ID_STATUS As Integer) As String

            Try

                Dim result As String = 0

                Using db As New dbRMS_JIEntities

                    Dim oTA_ACTIVITY = db.TA_AWARDED_ACTIVITY.Find(ID_ACTIVITY_AW)
                    Dim PrevSTATUS = ID_STATUS - 1

                    If oTA_ACTIVITY.ID_ACTIVITY_STATUS <= PrevSTATUS Then

                        oTA_ACTIVITY.ID_ACTIVITY_STATUS = ID_STATUS
                        db.Entry(oTA_ACTIVITY).State = Entity.EntityState.Modified

                        If (db.SaveChanges()) Then

                            result = oTA_ACTIVITY.ID_AWARDED_ACTIVITY
                            Set_TA_ACTIVITY_AW_STATUS = result

                        Else
                            Set_TA_ACTIVITY_AW_STATUS = "-1"
                        End If

                    Else

                        Set_TA_ACTIVITY_AW_STATUS = "-1"

                    End If


                End Using

            Catch ex As Exception

                Set_TA_ACTIVITY_AW_STATUS = "-1"

            End Try


        End Function



        Public Function Save_TA_EVALUATION_APP_COMM(ByVal ta_eval_app_comm As TA_EVALUATION_APP_COMM, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim ta_eval_app_comm_upd As TA_EVALUATION_APP_COMM

                    If ID = 0 Then

                        db.TA_EVALUATION_APP_COMM.Add(ta_eval_app_comm)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        ta_eval_app_comm_upd = db.TA_EVALUATION_APP_COMM.Find(ID)

                        ta_eval_app_comm_upd.ID_EVALUATION_APP = ta_eval_app_comm.ID_EVALUATION_APP
                        ta_eval_app_comm_upd.ROUND = ta_eval_app_comm.ROUND
                        ta_eval_app_comm_upd.ID_EVALUATION_APP_STATUS = ta_eval_app_comm.ID_EVALUATION_APP_STATUS
                        ta_eval_app_comm_upd.EVALUATION_COMM = ta_eval_app_comm.EVALUATION_COMM
                        ta_eval_app_comm_upd.SCORE = ta_eval_app_comm.SCORE
                        ta_eval_app_comm_upd.VOTE = ta_eval_app_comm.VOTE
                        ta_eval_app_comm_upd.POINTS = ta_eval_app_comm.POINTS
                        ta_eval_app_comm_upd.COMM_BOL = ta_eval_app_comm.COMM_BOL


                        'If Deliverable.fecha_aprobo.HasValue Then
                        '    ta_deliverable_upd.fecha_aprobo = Deliverable.fecha_aprobo
                        'End If

                        'If Deliverable.fecha_entrego.HasValue Then
                        '    ta_deliverable_upd.fecha_entrego = Deliverable.fecha_entrego
                        'End If

                        'If Deliverable.id_deliverable_estado = 3 Or Deliverable.id_deliverable_estado = 4 Or Deliverable.id_deliverable_estado = 6 Then

                        '    ta_deliverable_upd.fecha_complete = Deliverable.fecha_complete
                        '    ta_deliverable_upd.usuario_complete = Deliverable.usuario_complete

                        'End If

                        db.Entry(ta_eval_app_comm_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = ta_eval_app_comm.ID_EVALUATION_APP
                        Else
                            result = ta_eval_app_comm_upd.ID_EVALUATION_APP
                        End If

                        Save_TA_EVALUATION_APP_COMM = result

                    Else
                        Save_TA_EVALUATION_APP_COMM = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_TA_EVALUATION_APP_COMM = ex.Message

            End Try

        End Function

        Public Function get_Apply_Comments_special(ByVal id_Apply As Integer, ByVal dateCOMM As Date) As DataTable

            Sql = String.Format("select * from FN_getting_Apply_Comments({0},'{1}', 0)", id_Apply, dateCOMM.ToString("yyyy-MM-dd"))
            get_Apply_Comments_special = cl_utl.setObjeto("TA_APPLY_COMM", "ID_APPLY_APP", id_Apply, Sql)

        End Function


        Public Function get_Screenning_Comments_special(ByVal ID_APPLY_SCREENING As Integer, ByVal dateCOMM As Date, ByVal shownRES As Integer) As DataTable

            Sql = String.Format("select * from [FN_getting_Screening_Comments]({0},'{1}', 0, {2})", ID_APPLY_SCREENING, dateCOMM.ToString("yyyy-MM-dd"), shownRES)
            get_Screenning_Comments_special = cl_utl.setObjeto("TA_APPLY_COMM", "ID_APPLY_APP", ID_APPLY_SCREENING, Sql)

        End Function


        Public Function get_Apply_Comments_EVAL(ByVal id_EVAL_App As Integer, ByVal dateCOMM As Date) As DataTable

            Sql = String.Format("select * from FN_getting_Evaluation_Comments({0},'{1}', 0)", id_EVAL_App, dateCOMM.ToString("yyyy-MM-dd"))
            get_Apply_Comments_EVAL = cl_utl.setObjeto("TA_EVALUATION_APP_COMM", "ID_EVALUATION_APP", id_EVAL_App, Sql)

        End Function

        Public Function get_Apply_Dates_EVAL(ByVal id_EVAL_APP As Integer) As DataTable

            Sql = String.Format("SELECT * FROM [dbo].[FN_getting_Evaluation_Dates]({0}) ORDER BY DATE_CREATED", id_EVAL_APP)
            get_Apply_Dates_EVAL = cl_utl.setObjeto("TA_EVALUATION_APP_COMM", "ID_EVALUATION_APP", id_EVAL_APP, Sql)

        End Function

        Public Function get_Screening_Dates(ByVal ID_APPLY_SCREENING As Integer) As DataTable

            Sql = String.Format("select * from [dbo].[FN_getting_Screening_Dates]({0}) ORDER BY DATE_CREATED", ID_APPLY_SCREENING)
            get_Screening_Dates = cl_utl.setObjeto("TA_APPLY_SCREENING_COMM ", "ID_APPLY_SCREENING", ID_APPLY_SCREENING, Sql)

        End Function



        Public Function get_Apply_Dates(ByVal id_Apply As Integer) As DataTable

            Sql = String.Format("select * from [dbo].[FN_getting_Apply_Dates]({0}) ORDER BY DATE_CREATED", id_Apply)
            get_Apply_Dates = cl_utl.setObjeto("TA_APPLY_COMM", "ID_APPLY_APP", id_Apply, Sql)

        End Function


        Public Function get_ACTIVITY_ANSWER_SCORE(ByVal id_Apply_App As Integer) As DataTable

            Sql = String.Format("select * from VW_TA_EVALUATION_ANSWER_SCORE where id_APPLY_APP = {0}  order by nombre_usuario, id_measurement_theme ", id_Apply_App)

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_EVALUATION_ANSWER_SCORE", "ID_APPLY_APP", id_Apply_App, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_APPLY_APP") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_ACTIVITY_ANSWER_SCORE = tbl_result

        End Function


        Public Function get_ACTIVITY_ANSWER_VAL(ByVal id_Apply_App As Integer) As DataTable

            Sql = String.Format("select * from VW_TA_EVALUATION_ANSWER_VAL where id_APPLY_APP = {0}  order by nombre_usuario, id_measurement_theme ", id_Apply_App)

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_EVALUATION_ANSWER_VAL", "ID_APPLY_APP", id_Apply_App, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_APPLY_APP") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_ACTIVITY_ANSWER_VAL = tbl_result

        End Function


        Public Function get_Applications_ORG(ByVal id_ACT_Apply As Integer) As DataTable

            Sql = String.Format("SELECT * FROM [dbo].[FN_getting_Organization_APPLY]({0}) ORDER BY ID_APPLY_APP", id_ACT_Apply)
            get_Applications_ORG = cl_utl.setObjeto("VW_TA_ACTIVITY_SOLICITATION_STAGEA", "ID_ACTIVITY_SOLICITATION", id_ACT_Apply, Sql)

        End Function

        Public Function get_Applications_rounds(ByVal id_ACT_Apply As Integer, ByVal IdOrg As Integer) As DataTable

            Sql = String.Format("SELECT * FROM [dbo].[FN_getting_Organization_EVAL]({0},{1})   order by ID_ROUND", id_ACT_Apply, IdOrg)
            get_Applications_rounds = cl_utl.setObjeto("VW_TA_ACT_SOL_APP_EVAL", "ID_ACTIVITY_SOLICITATION", id_ACT_Apply, Sql)

        End Function




        Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

            Dim clDate As APPROVAL.cls_dUtil
            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(cl_user.Id_Cprogram, True)
            Dim userCulture As CultureInfo
            Dim timezoneUTC As Integer
            userCulture = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
            clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
            'Return dateIN.ToShortDateString

        End Function


        Public Function getHora(dateIN As DateTime) As String

            Dim clDate As APPROVAL.cls_dUtil
            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(cl_user.Id_Cprogram, True)
            Dim userCulture As CultureInfo
            Dim timezoneUTC As Integer
            userCulture = cl_user.regionalizacionCulture
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
            clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            Return clDate.set_TimeFormat(dateIN, timezoneUTC, True)


        End Function









    End Class

End Namespace
