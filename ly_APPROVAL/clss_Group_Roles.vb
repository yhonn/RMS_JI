


Namespace APPROVAL

    Public Class clss_GroupRoles
        Inherits clss_approval

        Dim Sql As String

        Public ReadOnly Property id_group As Integer

        Sub New(ByVal id_p As Integer, Optional ByVal idG As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class

            id_group = IIf(idG > 0, idG, 0)

        End Sub

        Public Function Query_Roles(Optional ByVal id_R As Integer = 0) As DataTable

            Dim idR As Integer = IIf(id_R > 0, id_R, 0)

            Dim sqlIdROL As String

            If idR = 0 Then
                sqlIdROL = " And (a.id_rol Not IN (SELECT DISTINCT id_rol FROM ta_gruposRoles  ))  "
            Else
                sqlIdROL = " And ( a.id_rol = " & id_R & ") "
            End If

            'Dim sqlIdROL2 As String = " where id_rol <> " & idR

            Sql = String.Format("select a.id_rol, b.nombre_rol, b.descripcion_rol, a.id_usuario, b.id_programa   " &
                                "   from  vw_user_role_simple a  " &
                                "       inner join ta_roles b on (a.id_rol = b.id_rol) " &
                                "         WHERE a.id_programa = {0} {1} ", id_programa, sqlIdROL)


            Query_Roles = cl_utl.setObjeto("vw_user_role_simple", "id_programa", id_programa, Sql)

        End Function


    End Class


End Namespace