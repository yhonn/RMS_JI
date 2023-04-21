Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient


Namespace APPROVAL

    Public Class cl_categorie

        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_ta_categoria As DataTable

        Public Property id_categoria As Integer


        Sub New(ByVal id_p As Integer, Optional ByVal idC As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class
            'Init_Users_ROlE(idR) 'Init data table of Users


            If idC > 0 Then
                set_ta_categoria(idC)
            End If

        End Sub

        '********************************************************* ta_categoria ENTITY ************************************************************************************
        '********************************************************* ta_categoria  ENTITY ************************************************************************************

        Public Function set_ta_categoria(ByVal id_cat As Integer) As DataTable

            id_categoria = IIf(id_cat > 0, id_cat, 0)
            Dim strSql As String = String.Format("select * from  ta_categoria  where id_categoria = {0} And id_programa = {1} ", id_categoria, id_programa)

            tbl_ta_categoria = cl_utl.setObjeto("ta_categoria", "id_categoria", id_categoria, strSql)
            set_ta_categoria = tbl_ta_categoria

        End Function

        Public Function get_ta_categoriaFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_categoria, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_categoriaFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_categoria = cl_utl.setDTval(tbl_ta_categoria, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_categoria() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_categoria", tbl_ta_categoria, "id_categoria", id_categoria)

            If RES <> -1 Then
                set_ta_categoriaFIELDS("id_categoria", RES, "id_categoria", id_categoria)
                id_categoria = RES
                save_ta_categoria = RES
            Else
                save_ta_categoria = RES
            End If

        End Function

        Public Function del_ta_categoria(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_categoria WHERE (id_categoria = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_categoria = True

                Catch ex As Exception
                    del_ta_categoria = False
                End Try

            Else

                del_ta_categoria = False

            End If

        End Function



        Public Function get_ta_categoriaField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_categoria, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_ta_categoria() As DataTable

            tbl_ta_categoria = cl_utl.setObjeto("ta_categoria", "id_categoria", id_categoria).Copy

            set_ta_categoria = Tbl_ta_categoria

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_ta_categoria.Rows.Count = 1 And set_ta_categoria.Rows.Item(0).Item("id_categoria") = 0) Then
                set_ta_categoria.Rows.Remove(set_ta_categoria.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* ta_categoria  ENTITY ************************************************************************************
        '********************************************************* ta_categoria  ENTITY ************************************************************************************







    End Class

End Namespace
