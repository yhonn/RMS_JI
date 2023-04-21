
Imports System.Configuration.ConfigurationManager
Imports System.Linq.Expressions
Imports System.Configuration
Imports System.Data.SqlClient
Imports ly_SIME

Namespace APPROVAL
    Public Class clss_viaje

        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Const cAction_ByProcess = 1
        Const cAction_ByMessage = 2
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Public Sub New(ByVal idP)
            id_programa = idP
        End Sub
        Public Function get_ViajeApprovalUser(ByVal id_Usr As String, ByVal id_categoria As Integer) As DataTable

            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'TRAVEL-RM01' and id_categoria = {2} and id_tipoDocumento not in (130,126,125,124,123) and orden = 0 ", id_programa, id_Usr, id_categoria)

            get_ViajeApprovalUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ViajeApprovalUser.Rows.Count = 1 And get_ViajeApprovalUser.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_ViajeApprovalUser.Rows.Remove(get_ViajeApprovalUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

    End Class
End Namespace

