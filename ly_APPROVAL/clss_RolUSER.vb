Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient


Namespace APPROVAL


    Public Class clss_RolUSER
        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_rol_user As New DataTable
        Dim tbl_Users_ROLE As New DataTable
        Dim tbl_ta_roles As New DataTable
        Dim tbl_ta_role_user As New DataTable

        Const CONST_TYPE_SIMPLE = 1
        Const CONST_TYPE_SHARED = 2

        Public Property CountUsers As Integer
        Public Property id_rol As Integer
        Public Property id_rol_user As Integer


        Sub New(ByVal id_p As Integer, Optional ByVal idR As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class
            Init_Users_ROlE(idR) 'Init data table of Users
            id_rol = IIf(idR > 0, idR, 0)
            id_rol_user = 0

            If id_rol > 0 Then
                set_ta_roles(id_rol)
            End If

        End Sub


        '*********************** tbl_rol_user entity handled********************************
        Public Function setRol_USER(ByVal checkIDusers As Boolean, ByVal tblRoles As DataTable) As DataTable


            Dim optUSers As String = ""

            If checkIDusers Then

                For Each dtR As DataRow In tblRoles.Rows
                    optUSers &= dtR("id_usuario") & ", "
                Next

                If optUSers.Trim.Length > 0 Then
                    optUSers = optUSers.Substring(0, optUSers.Trim.Length - 1)
                Else
                    optUSers = ""
                End If

            End If


            optUSers = IIf(optUSers.Length > 0, String.Format("{0} {1} {2}", "  and id_usuario not in ( ", optUSers, ")  "), "")

            Sql = String.Format("  Select id_usuario, nombre_empleado, estado  " &
                                    "       from vw_user_role_all  " &
                                    "           where id_programa =  {0}  " &
                                    "             and (upper(estado) = 'ACTIVE' or upper(estado) = 'ACTIVO'  ) {1} " &
                                    "                order by nombre_empleado", id_programa, optUSers)


            tbl_rol_user = cl_utl.setObjeto("vw_user_role_all", "id_usuario", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_rol_user.Rows.Count = 1 And tbl_rol_user.Rows.Item(0).Item("id_usuario") = 0) Then
                tbl_rol_user.Rows.Remove(tbl_rol_user.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            setRol_USER = tbl_rol_user

        End Function


        '*********************** tbl_rol_user entity handled********************************
        Public Function setRol_USER(ByVal checkIDusers As Boolean) As DataTable


            Dim optUSers As String = ""

            If checkIDusers Then

                For Each dtR As DataRow In tbl_Users_ROLE.Rows
                    optUSers &= dtR("id_usuario") & ", "
                Next

                If optUSers.Trim.Length > 0 Then
                    optUSers = optUSers.Substring(0, optUSers.Trim.Length - 1)
                Else
                    optUSers = ""
                End If

            End If


            optUSers = IIf(optUSers.Length > 0, String.Format("{0} {1} {2}", "  and id_usuario not in ( ", optUSers, ")  "), "")

            Sql = String.Format("  Select id_usuario, nombre_empleado, estado  " &
                                    "       from vw_user_role_all  " &
                                    "           where id_programa =  {0}  " &
                                    "             and (upper(estado) = 'ACTIVE' or upper(estado) = 'ACTIVO'  ) {1} " &
                                    "                order by nombre_empleado", id_programa, optUSers)


            tbl_rol_user = cl_utl.setObjeto("vw_user_role_all", "id_usuario", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_rol_user.Rows.Count = 1 And tbl_rol_user.Rows.Item(0).Item("id_usuario") = 0) Then
                tbl_rol_user.Rows.Remove(tbl_rol_user.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            setRol_USER = tbl_rol_user

        End Function

        Public Function get_UserRolField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_rol_user, campoSearch, campo, valorSearch)

        End Function

        Public Function set_UserRolField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_rol_user, campoSearch, campo, valorSearch)

        End Function

        '*********************** tbl_rol_user entity handled********************************


        '***********************ta_roles entity********************************

        Public Function set_ta_roles(ByVal id_Rl As Integer) As DataTable

            tbl_ta_roles = cl_utl.setObjeto("ta_roles", "id_rol", id_Rl)
            set_ta_roles = tbl_ta_roles

        End Function

        Public Function get_ta_rolesFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_roles, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_rolesFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_roles = cl_utl.setDTval(tbl_ta_roles, campoSearch, campo, valorSearch, valor)

        End Sub
        'tbl_ta_role_user

        Public Function save_ta_roles() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_roles", tbl_ta_roles, "id_rol", id_rol)

            If RES <> -1 Then
                save_ta_roles = RES
                set_ta_rolesFIELDS("id_rol", RES, "id_rol", id_rol)
                id_rol = RES
            Else
                save_ta_roles = RES
            End If

        End Function

        '***********************ta_roles entity********************************



        '***********************ta_role_user entity********************************

        Public Function set_ta_role_user(ByVal id_rolUser As Integer) As DataTable

            id_rol_user = IIf(id_rolUser > 0, id_rolUser, 0)

            tbl_ta_role_user = cl_utl.setObjeto("ta_role_user", "id_rol_user", id_rol_user)
            set_ta_role_user = tbl_ta_role_user

        End Function

        Public Function get_ta_role_userFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_role_user, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_role_userFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_role_user = cl_utl.setDTval(tbl_ta_role_user, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_role_user() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_role_user", tbl_ta_role_user, "id_rol_user", id_rol_user)

            If RES <> -1 Then
                set_ta_role_userFIELDS("id_rol_user", RES, "id_rol_user", id_rol_user)
                id_rol_user = RES
                save_ta_role_user = RES
            Else
                save_ta_role_user = RES
            End If

        End Function


        Public Function Remove_ta_role_user(ByVal id_rolUSER As Integer) As Boolean

            Dim sql As String

            If id_rolUSER > 0 Then

                sql = String.Format(" delete from ta_role_user where id_rol_user = {0}", id_rolUSER)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    Remove_ta_role_user = True

                Catch ex As Exception
                    Remove_ta_role_user = False
                End Try

            Else

                Remove_ta_role_user = False

            End If


        End Function

        '***********************ta_role_user entity********************************


        '***********************Users_ROlE entity Handled********************************


        Public Sub Init_Users_ROlE(ByVal IdR As Integer)

            Sql = String.Format("select b.id_rol_user, a.id_usuario, a.job , a.nombre_usuario, a.email_usuario, " &
                                 "   a.fecha_contrato, a.estado, b.id_rol  from vw_t_usuarios a  " &
                                 "    inner join ta_role_user b on (a.id_usuario = b.id_usuario) " &
                                 "     where b.id_rol={0} and a.id_programa = {1} ", IdR, id_programa)

            tbl_Users_ROLE = cl_utl.setObjeto("vw_user_role_all", "id_rol", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_Users_ROLE.Rows.Count = 1 And tbl_Users_ROLE.Rows.Item(0).Item("id_usuario") = 0) Then
                tbl_Users_ROLE.Rows.Remove(tbl_Users_ROLE.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            'tbl_Users_ROLE.Rows.Remove(tbl_Users_ROLE.Rows.Item(0)) 'To set without any record

            Me.CountUsers = tbl_Users_ROLE.Rows.Count

        End Sub


        Public Function Add_User_ROLE_one(ByVal IdUsr As Integer) As DataTable

            Sql = String.Format("select 0 as id_rol_user, id_usuario, job, nombre_usuario, email_usuario, fecha_contrato, estado, {1} as id_rol  from vw_t_usuarios where id_usuario={0} and id_programa = {2} ", IdUsr, id_rol, id_programa)

            Dim tbl_User As New DataTable
            tbl_User = cl_utl.setObjeto("vw_user_role_all", "id_usuario", 0, Sql)

            For Each drR As DataRow In tbl_User.Rows
                tbl_Users_ROLE.ImportRow(drR)
            Next

            Me.CountUsers = tbl_Users_ROLE.Rows.Count
            Add_User_ROLE_one = tbl_User

        End Function

        Public Function Add_User_ROLE(ByVal IdUsr As Integer) As DataTable

            Sql = String.Format("select 0 as id_rol_user, id_usuario, job, nombre_usuario, email_usuario, fecha_contrato, estado, {1} as id_rol  from vw_t_usuarios where id_usuario={0} and id_programa = {2} ", IdUsr, id_rol, id_programa)

            Dim tbl_User As New DataTable
            tbl_User = cl_utl.setObjeto("vw_user_role_all", "id_usuario", 0, Sql)

            For Each drR As DataRow In tbl_User.Rows
                tbl_Users_ROLE.ImportRow(drR)
            Next

            Me.CountUsers = tbl_Users_ROLE.Rows.Count
            Add_User_ROLE = tbl_Users_ROLE

        End Function

        Public Function Remove_User_Role(ByVal idUser As Integer) As DataTable

            Dim tbl_tmp As New DataTable
            Dim i As Integer = 0

            tbl_tmp = tbl_Users_ROLE.Copy

            For Each drR As DataRow In tbl_tmp.Rows

                If drR("id_usuario") = idUser Then
                    tbl_Users_ROLE.Rows.Remove(tbl_Users_ROLE.Rows.Item(i))
                    i -= 1
                End If
                i += 1

            Next

            Me.CountUsers = tbl_Users_ROLE.Rows.Count
            Remove_User_Role = tbl_Users_ROLE

        End Function

        Public Function Get_User_ROLE() As DataTable
            Get_User_ROLE = tbl_Users_ROLE
        End Function

        Public Function get_UserRol_ROW(ByVal vIndex As Integer) As DataRow
            get_UserRol_ROW = tbl_Users_ROLE.Rows.Item(vIndex)

        End Function

    End Class


    '***************************************Users_ROlE entity Handled*******************************************

End Namespace

