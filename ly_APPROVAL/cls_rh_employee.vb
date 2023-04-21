
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Configuration
Imports ly_SIME

Namespace APPROVAL


    Public Class cls_rh_employee


        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Const cAction_ByProcess = 1
        Const cAction_ByMessage = 2
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Public cl_user As ly_SIME.CORE.cls_user


        Public Sub New(ByVal idP As Integer, Optional user_ As ly_SIME.CORE.cls_user = Nothing)
            id_programa = idP
            cl_user = user_
        End Sub

        Public Function get_fields(ByVal type_data As Integer) As DataTable



            Dim strSQL As String

            If type_data = 1 Then
                strSQL = String.Format("select  distinct CONVERT(char(8), convert(char(4),anio) + '_' + convert(char(3), FORMAT(DateAdd( month , mes , 0 ) - 1, 'MMM', 'es-es'))) as Period	
	                                     from vw_ta_timesheet_detail
	                                       WHERE (    (id_usuario = 0 or 1 = 1)
			                                 and (anio = 0 or 1 = 1 ) 													
			                                 and (id_timesheet_estado = 3) )")
            ElseIf type_data = 2 Then
                strSQL = String.Format("select  distinct (billable_time_category + '_' + convert(char(3), FORMAT(DateAdd( month , mes , 0 ) - 1, 'MMM', 'es-es'))) as Period	
	                                     from vw_ta_timesheet_detail
	                                       WHERE (    (id_usuario = 0 or 1 = 1)
			                                 and (anio = 0 or 1 = 1 ) 													
			                                 and (id_timesheet_estado = 3) ) ")
            ElseIf type_data = 5 Then
                strSQL = String.Format("select  distinct (billable_time_category + '_' + convert(char(3), FORMAT(DateAdd( month , mes , 0 ) - 1, 'MMM', 'es-es'))) as Period	
	                                     from vw_ta_timesheet_detail
	                                       WHERE (    (id_usuario = 0 or 1 = 1)
			                                 and (anio = 0 or 1 = 1 ) 													
			                                 and (id_timesheet_estado = 3) ) ")
            Else

                strSQL = String.Format("Select Case distinct billable_time_category As Period	
                                             From vw_ta_timesheet_detail
                                               Where ((id_usuario = 0 Or 1 = 1)
			                                     And (anio = 0 Or 1 = 1 ) 													
			                                     And (id_timesheet_estado = 3) ) ")

            End If



            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_ta_timesheet_detail", "id_usuario", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("Period") = "") Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_fields = tbl_result

        End Function



    End Class

End Namespace
