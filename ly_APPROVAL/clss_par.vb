
Imports System.Configuration.ConfigurationManager
Imports System.Linq.Expressions
Imports System.Configuration
Imports System.Data.SqlClient
Imports ly_SIME

Namespace APPROVAL
    Public Class clss_par
        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Const cAction_ByProcess = 1
        Const cAction_ByMessage = 2
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Public Sub New(ByVal idP)
            id_programa = idP
        End Sub
        Public Function get_parApprovalUser_mayor_50000(ByVal id_Usr As String, ByVal id_categoria As Integer) As DataTable

            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'PAR-RMS01' and id_categoria = {2} and orden = 0 and id_tipoDocumento = 220", id_programa, id_Usr, id_categoria)

            get_parApprovalUser_mayor_50000 = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_parApprovalUser_mayor_50000.Rows.Count = 1 And get_parApprovalUser_mayor_50000.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_parApprovalUser_mayor_50000.Rows.Remove(get_parApprovalUser_mayor_50000.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_parApprovalUser_comunicaciones(ByVal id_Usr As String, ByVal id_categoria As Integer) As DataTable

            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'PAR-RMS01' and id_categoria = {2} and orden = 0  and id_tipoDocumento = 129", id_programa, id_Usr, id_categoria)

            get_parApprovalUser_comunicaciones = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_parApprovalUser_comunicaciones.Rows.Count = 1 And get_parApprovalUser_comunicaciones.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_parApprovalUser_comunicaciones.Rows.Remove(get_parApprovalUser_comunicaciones.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_parApprovalUser_administrativo(ByVal id_Usr As String, ByVal id_categoria As Integer) As DataTable

            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'PAR-RMS01' and id_categoria = {2} and orden = 0  and id_tipoDocumento in (128,268)", id_programa, id_Usr, id_categoria)

            get_parApprovalUser_administrativo = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_parApprovalUser_administrativo.Rows.Count = 1 And get_parApprovalUser_administrativo.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_parApprovalUser_administrativo.Rows.Remove(get_parApprovalUser_administrativo.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_parApprovalUserEventos(ByVal id_Usr As String, ByVal id_categoria As Integer) As DataTable

            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'PAR-RMS01' and id_categoria = {2} and orden = 0 and id_tipoDocumento not in (128,129,220,127,268) ", id_programa, id_Usr, id_categoria)

            get_parApprovalUserEventos = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_parApprovalUserEventos.Rows.Count = 1 And get_parApprovalUserEventos.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_parApprovalUserEventos.Rows.Remove(get_parApprovalUserEventos.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

    End Class

End Namespace
