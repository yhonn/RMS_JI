
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Web.UI.Page
Imports ly_SIME
Imports ly_RMS

Namespace APPROVAL

    Public Class cls_scheduled


        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String
        Dim id_programa As Integer
        Dim cl_utl As New CORE.cls_util

        Dim tbl_t_scheduled_Done As DataTable

        Public Property id_scheduled_done As Integer = 0


        Public Sub New(ByVal id_prog As Integer)

            id_programa = id_prog
            id_scheduled_done = 0

        End Sub


        Public Function get_scheduled_() As DataTable

            Sql = String.Format("select a.id_scheduled,
                                        a.scheduled_name,
                                        a.scheduled_descripction,
                                        a.scheduled_start_date,
                                        a.scheduled_end_date,
                                        a.id_scheduled_frequency,
                                        b.scheduled_frequency_name,
                                        b.id_type_frequency,
                                        c.type_frecuency,
                                        c.type_frecuency_description,
                                        a.id_programa,
                                        a.scheduled_active,
                                        a.id_notification
                                         from t_scheduled a
                                            inner join t_scheduled_frequency b on (a.id_scheduled_frequency  = b.id_scheduled_frequency )
                                           inner join type_frequency c on (c.id_type_frequency = b.id_type_frequency)
                                         where scheduled_active = 1")

            get_scheduled_ = cl_utl.setObjeto("t_scheduled", "id_scheduled", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_scheduled_.Rows.Count = 1 And get_scheduled_.Rows.Item(0).Item("id_scheduled") = 0) Then
                get_scheduled_.Rows.Remove(get_scheduled_.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

        '*********************************************************Echeduled Methods************************************************************************************

        Public Function check_scheduled_byDaily(ByVal id_frecuency As Integer, ByVal vToday As Integer, ByVal vMonth As Integer, ByVal vYear As Integer) As Integer

            Sql = String.Format("select count(*) as N from t_scheduled_Done
                                   where id_scheduled_frecuency_range = {0}
		                            and datepart(dd, sch_done_date) = {1}
                                   and datepart(mm, sch_done_date) = {2}
                                 and datepart(yy, sch_done_date) = {3} ", id_frecuency, vToday, vMonth, vYear)

            check_scheduled_byDaily = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", 0, Sql).Rows.Item(0).Item("N")


        End Function



        '********************************************************* Echeduled Method  ************************************************************************************



        '*********************************************************Scheduled Frequency************************************************************************************

        Public Function get_t_scheduled_frequency(ByVal id_scheduled As Integer, Optional vDay As Integer = 0) As DataTable

            Dim bndDay As Integer = If(vDay = 0, 1, 0)


            Sql = String.Format("select d.id_programa, d.id_scheduled, d.scheduled_name, d.scheduled_active, e.id_type_frequency, e.type_frecuency, c.id_scheduled_frequency, c.scheduled_frequency_name, a.id_scheduled_frequency_range, a.scheduled_frequency_day, a.scheduled_frequency_hour, a.scheduled_frequency_minutes 
                                  from t_scheduled_frequency_ranges a
                                inner join t_scheduled_frequency c on (a.id_scheduled_frequency = c.id_scheduled_frequency)
                                inner join t_scheduled d on (d.id_scheduled_frequency = c.id_scheduled_frequency )
                                inner join type_frequency e on (c.id_type_frequency = e.id_type_frequency)
                                where ( a.scheduled_frequency_status = 1 and d.scheduled_active = 1 )  
                                and d.id_scheduled = {0} 
                                and ( a.scheduled_frequency_day = {1} or 1 = {2} ) ", id_scheduled, vDay, bndDay)

            get_t_scheduled_frequency = cl_utl.setObjeto("t_scheduled_frequency_ranges", "id_scheduled_frequency_range", 0, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_t_scheduled_frequency.Rows.Count = 1 And get_t_scheduled_frequency.Rows.Item(0).Item("id_scheduled_frequency_range") = 0) Then
                get_t_scheduled_frequency.Rows.Remove(get_t_scheduled_frequency.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function


        Public Function check_scheduled_by_Time(ByVal id_frecuency As Integer, ByVal vToday As Integer, ByVal vMonth As Integer, ByVal vYear As Integer, ByVal vHour As Integer, ByVal vMin As Integer, ByVal vOffset As Integer) As Integer

            Sql = String.Format("select count(*) as N
                                     from t_scheduled_frequency_ranges a
                                 inner join t_scheduled_Done b on (a.id_scheduled_frequency_range = b.id_scheduled_frequency_range)
                                 inner join t_scheduled_frequency c on (a.id_scheduled_frequency = a.id_scheduled_frequency)
                                 inner join t_scheduled d on (d.id_scheduled_frequency = c.id_scheduled_frequency )
                                where ( a.scheduled_frequency_status = 1 and d.scheduled_active = 1 )
                                 and d.id_scheduled = {0} 
                                 and datepart(dd, scheduled_done_date ) = {1}
                                 and datepart(mm, scheduled_done_date ) = {2}
                                 and datepart(yy, scheduled_done_date ) = {3}
                                 and  ( ( datepart(hh, dateadd(HH,{6},b.scheduled_done_date)) = {4} and datepart(MI, dateadd(HH,{6},b.scheduled_done_date)) >= {5} ) 
                                        OR
                                        ( datepart(hh, dateadd(HH,{6},b.scheduled_done_date)) > {4} ) 
                                      )  ", id_frecuency, vToday, vMonth, vYear, vHour, vMin, vOffset)

            check_scheduled_by_Time = cl_utl.setObjeto("t_scheduled_frequency_ranges", "id_documento", 0, Sql).Rows.Item(0).Item("N")


        End Function


        '********************************************************* Scheduled Frequency ************************************************************************************



        '********************************************************* t_scheduled_Done ENTITY ************************************************************************************
        '********************************************************* t_scheduled_Done  ENTITY ************************************************************************************

        Public Function set_t_scheduled_Done(ByVal id_noti As Integer) As DataTable

            id_scheduled_done = IIf(id_noti > 0, id_noti, 0)
            tbl_t_scheduled_Done = cl_utl.setObjeto("t_scheduled_Done", "id_scheduled_done", id_scheduled_done)
            set_t_scheduled_Done = tbl_t_scheduled_Done

        End Function

        Public Function get_t_scheduled_DoneFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_scheduled_Done, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_t_scheduled_DoneFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_t_scheduled_Done = cl_utl.setDTval(tbl_t_scheduled_Done, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_t_scheduled_Done() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("t_scheduled_Done", tbl_t_scheduled_Done, "id_scheduled_done", id_scheduled_done)

            If RES <> -1 Then
                set_t_scheduled_DoneFIELDS("id_scheduled_done", RES, "id_scheduled_done", id_scheduled_done)
                id_scheduled_done = RES
                save_t_scheduled_Done = RES
            Else
                save_t_scheduled_Done = RES
            End If

        End Function

        Public Function del_t_scheduled_Done(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM t_scheduled_Done WHERE (id_scheduled_done = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_t_scheduled_Done = True

                Catch ex As Exception
                    del_t_scheduled_Done = False
                End Try

            Else

                del_t_scheduled_Done = False

            End If

        End Function


        '********************************************************* t_scheduled_Done  ENTITY ************************************************************************************
        '********************************************************* t_scheduled_Done  ENTITY ************************************************************************************






    End Class


End Namespace