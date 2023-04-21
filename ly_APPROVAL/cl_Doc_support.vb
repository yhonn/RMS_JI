Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient


Namespace APPROVAL


    Public Class cl_Doc_support

        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_ta_docs_soporte As DataTable

        Public Property id_doc_soporte As Integer


        Sub New(ByVal id_p As Integer, Optional ByVal idD As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class

            If idD > 0 Then
                set_ta_docs_soporte(idD)
            End If

        End Sub

        '********************************************************* ta_docs_soporte ENTITY ************************************************************************************
        '********************************************************* ta_docs_soporte  ENTITY ************************************************************************************

        Public Function set_ta_docs_soporte(ByVal id_docs As Integer) As DataTable

            id_doc_soporte = IIf(id_docs > 0, id_docs, 0)
            Dim strSql As String = String.Format("select * from  ta_docs_soporte  where id_doc_soporte = {0} And id_programa = {1} ", id_doc_soporte, id_programa)

            tbl_ta_docs_soporte = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_doc_soporte, strSql)
            set_ta_docs_soporte = tbl_ta_docs_soporte

        End Function

        Public Function get_ta_docs_soporteFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_docs_soporte, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_docs_soporteFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_docs_soporte = cl_utl.setDTval(tbl_ta_docs_soporte, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_docs_soporte() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_docs_soporte", tbl_ta_docs_soporte, "id_doc_soporte", id_doc_soporte)

            If RES <> -1 Then
                set_ta_docs_soporteFIELDS("id_doc_soporte", RES, "id_doc_soporte", id_doc_soporte)
                id_doc_soporte = RES
                save_ta_docs_soporte = RES
            Else
                save_ta_docs_soporte = RES
            End If

        End Function

        Public Function del_ta_docs_soporte(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_docs_soporte WHERE (id_doc_soporte = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_docs_soporte = True

                Catch ex As Exception
                    del_ta_docs_soporte = False
                End Try

            Else

                del_ta_docs_soporte = False

            End If

        End Function



        Public Function get_ta_docs_soporteField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(tbl_ta_docs_soporte, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_ta_docs_soporte() As DataTable

            tbl_ta_docs_soporte = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_doc_soporte).Copy

            set_ta_docs_soporte = tbl_ta_docs_soporte

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_ta_docs_soporte.Rows.Count = 1 And set_ta_docs_soporte.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                set_ta_docs_soporte.Rows.Remove(set_ta_docs_soporte.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* ta_docs_soporte  ENTITY ************************************************************************************
        '********************************************************* ta_docs_soporte  ENTITY ************************************************************************************








    End Class


End Namespace
