Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient


Namespace APPROVAL


    Public Class cl_environmentalAPP

        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_vw_t_documento_ambiental As DataTable
        Dim tbl_ta_documento_ambiental_archivos As DataTable
        Dim tbl_vw_enviromental_document As DataTable


        Public Property id_archivo_ambiental As Integer


        Sub New(ByVal id_p As Integer, Optional ByVal idD As Integer = 0)

            MyBase.New(id_p) 'Construct            Or fo the base class

            If idD > 0 Then
                set_ta_documento_ambiental(idD)
            End If

        End Sub


        Public Function get_vw_t_documento_ambiental(ByVal id_doc As Integer) As DataTable

            Dim id_doc_environ As Integer = IIf(id_doc > 0, id_documento_ambiental, 0)

            Sql = String.Format(" select * from vw_t_documento_ambiental where id_documento_ambiental = {0} ", id_doc_environ)

            tbl_vw_t_documento_ambiental = cl_utl.setObjeto("vw_t_documento_ambiental", "id_documento_ambiental", id_doc_environ, Sql)
            get_vw_t_documento_ambiental = tbl_vw_t_documento_ambiental

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_vw_t_documento_ambiental.Rows.Count = 1 And get_vw_t_documento_ambiental.Rows.Item(0).Item("id_documento_ambiental") = 0) Then
                get_vw_t_documento_ambiental.Rows.Remove(get_vw_t_documento_ambiental.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function



        Public Function get_vw_enviromental_document(ByVal id_doc As Integer) As DataTable

            Sql = String.Format(" select * from vw_enviromental_document  where  id_documento_ambiental = {0} ", id_doc)

            tbl_vw_enviromental_document = cl_utl.setObjeto("vw_enviromental_document", "id_documento_ambiental", id_doc, Sql)
            get_vw_enviromental_document = tbl_vw_enviromental_document


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_vw_enviromental_document.Rows.Count = 1 And tbl_vw_enviromental_document.Rows.Item(0).Item("id_documento_ambiental") = 0) Then
                tbl_vw_enviromental_document.Rows.Remove(tbl_vw_enviromental_document.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************



        End Function

        Public Function get_environmental_docs_type() As DataTable

            Sql = String.Format("Select * from ta_docs_soporte where id_programa = {0} And environmental = 1 ", id_programa)
            get_environmental_docs_type = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", 0, Sql)

        End Function



        Public Function get_vw_t_documento_ambientalFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_vw_t_documento_ambiental, campoSearch, campo, valorSearch)

        End Function



        '********************************************************* ta_documento_ambiental_archivos ENTITY ************************************************************************************
        '********************************************************* ta_documento_ambiental_archivos  ENTITY ************************************************************************************

        Public Function set_ta_documento_ambiental_archivos(ByVal id_docsENV As Integer) As DataTable

            id_archivo_ambiental = IIf(id_docsENV > 0, id_docsENV, 0)
            Dim strSql As String = String.Format("select * from  ta_documento_ambiental_archivos  where id_archivo_ambiental = {0} ", id_archivo_ambiental)

            tbl_ta_documento_ambiental_archivos = cl_utl.setObjeto("ta_documento_ambiental_archivos", "id_archivo_ambiental", id_archivo_ambiental, strSql)
            set_ta_documento_ambiental_archivos = tbl_ta_documento_ambiental_archivos

        End Function

        Public Function get_ta_documento_ambiental_archivosFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_documento_ambiental_archivos, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_documento_ambiental_archivosFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_documento_ambiental_archivos = cl_utl.setDTval(tbl_ta_documento_ambiental_archivos, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_documento_ambiental_archivos() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_documento_ambiental_archivos", tbl_ta_documento_ambiental_archivos, "id_archivo_ambiental", id_archivo_ambiental)

            If RES <> -1 Then
                set_ta_documento_ambiental_archivosFIELDS("id_archivo_ambiental", RES, "id_archivo_ambiental", id_archivo_ambiental)
                id_archivo_ambiental = RES
                save_ta_documento_ambiental_archivos = RES
            Else
                save_ta_documento_ambiental_archivos = RES
            End If

        End Function

        Public Function del_ta_documento_ambiental_archivos(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_documento_ambiental_archivos WHERE (id_archivo_ambiental = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_documento_ambiental_archivos = True

                Catch ex As Exception
                    del_ta_documento_ambiental_archivos = False
                End Try

            Else

                del_ta_documento_ambiental_archivos = False

            End If

        End Function



        Public Function get_ta_documento_ambiental_archivosField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(tbl_ta_documento_ambiental_archivos, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_ta_documento_ambiental_archivos() As DataTable

            tbl_ta_documento_ambiental_archivos = cl_utl.setObjeto("ta_documento_ambiental_archivos", "id_archivo_ambiental", id_archivo_ambiental).Copy

            set_ta_documento_ambiental_archivos = tbl_ta_documento_ambiental_archivos

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_ta_documento_ambiental_archivos.Rows.Count = 1 And set_ta_documento_ambiental_archivos.Rows.Item(0).Item("id_archivo_ambiental") = 0) Then
                set_ta_documento_ambiental_archivos.Rows.Remove(set_ta_documento_ambiental_archivos.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* ta_documento_ambiental_archivos  ENTITY ************************************************************************************
        '********************************************************* ta_documento_ambiental_archivos  ENTITY ************************************************************************************







    End Class


End Namespace
