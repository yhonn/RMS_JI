Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient

Namespace APPROVAL


    Public Class cl_ApprovalDefinition
        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_ta_tipoDocumento As DataTable
        Dim tbl_ta_docs_soporte As DataTable

        Public Property id_doc_soporte As Integer
        Public Property id_tipoDocumento As Integer

        Sub New(ByVal id_p As Integer, Optional ByVal idDoc As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class

            If idDoc > 0 Then
                set_ta_tipoDocumento(idDoc)
            End If

        End Sub

        Public Function save_tipoDocumentosAPP(ByVal idTemp As String) As Boolean


            Sql = String.Format("INSERT INTO ta_aprobacion_docs SELECT " & id_tipoDocumento & " as id_tipoDocumento , id_doc_soporte, PermiteRepetir, RequeridoInicio, RequeridoFin FROM ta_aprobacion_docs_temp WHERE id_sesion_temp='{0}'", idTemp)

            Try

                cnnSAP.Open()
                Dim dm As New SqlCommand(Sql, cnnSAP)
                dm.ExecuteNonQuery()
                cnnSAP.Close()

                save_tipoDocumentosAPP = True

            Catch ex As Exception
                save_tipoDocumentosAPP = False
            End Try


        End Function


        Public Function get_DocumentTypesFROM_tmp(ByVal id_ses_temp As String) As DataTable

            Sql = String.Format("SELECT id_doc_soporte, nombre_documento, extension, Template FROM ta_docs_soporte " &
                                 "  WHERE id_programa= {0} and id_doc_soporte not in  " &
                                 "    (select id_doc_soporte from ta_aprobacion_docs_temp where id_sesion_temp ='{1}')", id_programa, id_ses_temp)

            get_DocumentTypesFROM_tmp = cl_utl.setObjeto("ta_docs_soporte", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_DocumentTypesFROM_tmp.Rows.Count = 1 And get_DocumentTypesFROM_tmp.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_DocumentTypesFROM_tmp.Rows.Remove(get_DocumentTypesFROM_tmp.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_DocumentTypes() As DataTable

            Sql = String.Format("SELECT id_doc_soporte, nombre_documento, extension, Template FROM ta_docs_soporte " &
                                "  WHERE id_programa= {0} And id_doc_soporte Not in " &
                                "    (select id_doc_soporte from ta_aprobacion_docs  where id_tipoDocumento ={1} )", id_programa, id_tipoDocumento)

            get_DocumentTypes = cl_utl.setObjeto("ta_docs_soporte", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_DocumentTypes.Rows.Count = 1 And get_DocumentTypes.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_DocumentTypes.Rows.Remove(get_DocumentTypes.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Documents_approval() As DataTable

            Sql = String.Format("SELECT  ta_docs_soporte.nombre_documento,
	                                     ta_docs_soporte.extension, 
	                                     ta_aprobacion_docs.id_app_docs, 
	                                     ta_aprobacion_docs.id_tipoDocumento, 
	                                     ta_docs_soporte.id_doc_soporte, 
	                                     ta_docs_soporte.id_programa,
	                                     ta_aprobacion_docs.PermiteRepetir, 
                                         ta_aprobacion_docs.RequeridoInicio,
                                         ta_aprobacion_docs.RequeridoFin,
	                                     isnull(vw_ta_ruta_aprobacion.id_ruta,0) as id_ruta,
	                                     isnull(vw_ta_ruta_aprobacion.nombre_empleado,'( * ) All Members') as nombre_empleado 
	                             FROM ta_docs_soporte 
	                              INNER JOIN ta_aprobacion_docs ON ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte
	                              LEFT JOIN vw_ta_ruta_aprobacion ON vw_ta_ruta_aprobacion.id_ruta = ta_aprobacion_docs.id_ruta
				                     WHERE (ta_docs_soporte.id_programa = {0} ) AND (ta_aprobacion_docs.id_tipoDocumento = {1} )", id_programa, id_tipoDocumento)

            get_Documents_approval = cl_utl.setObjeto("ta_docs_soporte", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Documents_approval.Rows.Count = 1 And get_Documents_approval.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Documents_approval.Rows.Remove(get_Documents_approval.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        '********************************************************* ta_tipoDocumento ENTITY ************************************************************************************
        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************

        Public Function set_ta_tipoDocumento(ByVal id_docs As Integer) As DataTable

            id_tipoDocumento = IIf(id_docs > 0, id_docs, 0)
            Dim strSql As String = String.Format("select * from  ta_tipoDocumento  where id_tipoDocumento = {0} ", id_tipoDocumento)

            tbl_ta_tipoDocumento = cl_utl.setObjeto("ta_tipoDocumento", "id_tipoDocumento", id_tipoDocumento, strSql)
            set_ta_tipoDocumento = tbl_ta_tipoDocumento

        End Function

        Public Function get_ta_tipoDocumentoFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_tipoDocumento, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_tipoDocumentoFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_tipoDocumento = cl_utl.setDTval(tbl_ta_tipoDocumento, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_tipoDocumento() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_tipoDocumento", tbl_ta_tipoDocumento, "id_tipoDocumento", id_tipoDocumento)

            If RES <> -1 Then
                set_ta_tipoDocumentoFIELDS("id_tipoDocumento", RES, "id_tipoDocumento", id_tipoDocumento)
                id_tipoDocumento = RES
                save_ta_tipoDocumento = RES
            Else
                save_ta_tipoDocumento = RES
            End If

        End Function

        Public Function del_ta_tipoDocumento(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_tipoDocumento WHERE (id_tipoDocumento = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_tipoDocumento = True

                Catch ex As Exception
                    del_ta_tipoDocumento = False
                End Try

            Else

                del_ta_tipoDocumento = False

            End If

        End Function



        Public Function get_ta_tipoDocumentoField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(tbl_ta_tipoDocumento, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_ta_tipoDocumento() As DataTable

            tbl_ta_tipoDocumento = cl_utl.setObjeto("ta_tipoDocumento", "id_tipoDocumento", id_tipoDocumento).Copy

            set_ta_tipoDocumento = tbl_ta_tipoDocumento

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_ta_tipoDocumento.Rows.Count = 1 And set_ta_tipoDocumento.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                set_ta_tipoDocumento.Rows.Remove(set_ta_tipoDocumento.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************
        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************




    End Class

End Namespace