Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports ly_SIME


Namespace APPROVAL

    Public Class clss_approval

        Dim Sql As String
        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Public cl_utl As New CORE.cls_util

        Public tbl_t_Programa As New DataTable
        Dim tbl_ta_archivos_documento_temp As New DataTable
        Dim tbl_ta_documento As New DataTable
        Dim tbl_ta_documento_ambiental As New DataTable
        Dim tbl_ta_DocumentosINFO As New DataTable
        Dim tbl_ta_AppDocumento As New DataTable
        Dim tbl_ta_archivos_documento As New DataTable
        Dim tbl_ta_comentariosDoc As New DataTable

        Public ReadOnly Property id_programa As Integer
        Public Property id_Documento As Integer
        Public Property id_documento_ambiental As Integer
        Public Property id_TipoDocumento As Integer
        Public Property id_App_Documento As Integer
        Public Property id_Archivo As Integer
        Public Property id_comment As Integer

        Dim id_archivo_temp As Integer



        Public Sub New(ByVal idP As Integer)

            id_programa = idP
            id_Documento = 0
            id_TipoDocumento = 0
            id_archivo_temp = 0
            id_App_Documento = 0
            id_Archivo = 0
            id_comment = 0

            get_ProgramINFO(True) 'Set de program INFO

        End Sub


        Public Function get_CategoryUser(ByVal id_R As Integer) As DataTable

            Sql = String.Format("select distinct id_categoria, descripcion_cat from vw_roles_approvals " &
                                "   where id_programa = {0} and orden = 0 " &
                                "       and id_rol = {1}", id_programa, id_R)

            get_CategoryUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_CategoryUser.Rows.Count = 1 And get_CategoryUser.Rows.Item(0).Item("id_categoria") = 0) Then
                get_CategoryUser.Rows.Remove(get_CategoryUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ApprovalUser(ByVal id_usr As Integer, ByVal id_cat As Integer) As DataTable

            Sql = String.Format(" select id_usuario, id_tipoDocumento, descripcion_aprobacion " &
                                "    from vw_roles_approvals            " &
                                "     where id_programa = {0}           " &
                                "           and orden = 0               " &
                                "           and id_usuario = {1}        " &
                                "           and id_categoria = {2}      " &
                                "           and catVisible = 'SI'       " &
                                "           and visible = 'SI' ", id_programa, id_usr, id_cat)

            get_ApprovalUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ApprovalUser.Rows.Count = 1 And get_ApprovalUser.Rows.Item(0).Item("id_usuario") = 0) Then
                get_ApprovalUser.Rows.Remove(get_ApprovalUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

        Public Function get_enviromentalDoc(ByVal idDoc As Integer) As Integer

            Dim tbl_res As DataTable

            Sql = String.Format("select a.* from ta_categoria_ambiental a  " &
                                "  inner join ta_tipodocumento b on (a.id_categoria = b.id_categoria) " &
                                "   inner join ta_documento c on (b.id_tipoDocumento = c.id_tipoDocumento ) " &
                                "     where c.id_documento = {0} and a.visible = 1 ", idDoc)

            tbl_res = cl_utl.setObjeto("ta_categoria_ambiental", "id_categoria_ambiental", 0, Sql)

            get_enviromentalDoc = tbl_res.Rows.Item(0).Item("visible")

        End Function



        Public Function get_TimeSheetDoc(ByVal idDoc As Integer) As Integer

            Dim tbl_res As DataTable

            Sql = String.Format("select count(*) as N from ta_documento_timesheets where id_documento =  {0} ", idDoc)

            tbl_res = cl_utl.setObjeto("ta_documento_timesheets", "id_documento", 0, Sql)

            get_TimeSheetDoc = tbl_res.Rows.Item(0).Item("N")

        End Function

        Public Function get_RolesUser(ByVal id_usr As Integer, Optional vType As Integer = 0) As DataTable

            If vType = 0 Then 'All groups and All Roles 

                Sql = String.Format(" select distinct tab1.id_rol                                                                                      " &
                                "    from                                                                                                              " &
                                "    ( select id_rol from vw_roles_approvals where id_usuario= {1} and id_programa = {0}                               " &
                                "        union                                                                                                         " &
                                "     select id_rol from vw_grupos_roles_approvals where id_usuario= {1} and id_programa = {0} ) as tab1  ", id_programa, id_usr)

            ElseIf vType = 1 Then 'Just Roles


                Sql = String.Format("select distinct id_rol from vw_roles_approvals where id_usuario= {1} and id_programa = {0}  ", id_programa, id_usr)


            ElseIf vType = 2 Then 'just groups

                Sql = String.Format("select distinct id_rol from vw_grupos_roles_approvals where id_usuario= {1} and id_programa = {0}  ", id_programa, id_usr)

            ElseIf vType = 3 Then 'All Group Roles Just Simple Roles

                Sql = String.Format(" select distinct tab1.id_rol                                                                                    " &
                              "    from                                                                                                              " &
                              "    ( select id_rol from vw_roles_approvals where id_usuario= {1} and id_programa = {0}    and id_type_role = 1       " &
                              "        union                                                                                                         " &
                              "     select id_rol from vw_grupos_roles_approvals where id_usuario= {1} and id_programa = {0} ) as tab1  ", id_programa, id_usr)

            End If

            get_RolesUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_RolesUser.Rows.Count = 1 And get_RolesUser.Rows.Item(0).Item("id_rol") = 0) Then
                get_RolesUser.Rows.Remove(get_RolesUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function


        Public Function get_ApprovalRole(ByVal id_R As Integer, ByVal id_cat As Integer) As DataTable

            Sql = String.Format(" Select DISTINCT id_tipoDocumento, descripcion_aprobacion " &
                                "    from vw_roles_approvals            " &
                                "     where id_programa = {0}           " &
                                "           And orden = 0               " &
                                "           And id_rol = {1}            " &
                                "           And id_categoria = {2}      " &
                                "           And catVisible = 'SI'       " &
                                "           And visible = 'SI' " &
                                "           And id_approval_tool = 1  ", id_programa, id_R, id_cat)

            get_ApprovalRole = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ApprovalRole.Rows.Count = 1 And get_ApprovalRole.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_ApprovalRole.Rows.Remove(get_ApprovalRole.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

        Public Function get_Approval_DocumentSupport(ByVal id_doc As Integer, ByVal id_sess As String) As DataTable

            Sql = String.Format(" SELECT ta_docs_soporte.id_doc_soporte,            " &
                                "          ta_docs_soporte.nombre_documento,        " &
                                "          ta_docs_soporte.Template,                " &
                                "          ta_docs_soporte.extension,               " &
                                "          ta_docs_soporte.id_programa,             " &
                                "          ta_aprobacion_docs.id_app_docs,          " &
                                "          ta_aprobacion_docs.id_tipoDocumento,     " &
                                "          ta_aprobacion_docs.PermiteRepetir,       " &
                                "          ta_docs_soporte.max_size,                " &
                                "          ta_aprobacion_docs.RequeridoInicio,      " &
                                "          ta_aprobacion_docs.RequeridoFin          " &
                                "     FROM ta_docs_soporte                          " &
                                "      INNER JOIN ta_aprobacion_docs ON ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte       " &
                                "     WHERE (ta_docs_soporte.id_programa = {0})                                                                  " &
                                "         AND (ta_aprobacion_docs.id_tipoDocumento = {1})                                                        " &
                                "         AND (ta_aprobacion_docs.id_doc_soporte  NOT IN (SELECT id_doc_soporte                                  " &
                                "                                                            FROM ta_archivos_documento_temp                     " &
                                "									                            WHERE (id_sesion_temp = '{2}'))                  " &
                                "                                    OR ta_aprobacion_docs.PermiteRepetir='SI')  ", id_programa, id_doc, id_sess)

            get_Approval_DocumentSupport = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Approval_DocumentSupport.Rows.Count = 1 And get_Approval_DocumentSupport.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Approval_DocumentSupport.Rows.Remove(get_Approval_DocumentSupport.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_Approval_DocumentSupportPending_ByTMP(ByVal id_doc As Integer, ByVal id_sess As String) As DataTable

            Sql = String.Format(" SELECT ta_docs_soporte.id_doc_soporte,            " &
                                "          ta_docs_soporte.nombre_documento,        " &
                                "          ta_docs_soporte.id_programa,             " &
                                "          ta_aprobacion_docs.id_app_docs,          " &
                                "          ta_aprobacion_docs.id_tipoDocumento,     " &
                                "          ta_aprobacion_docs.PermiteRepetir,       " &
                                "          ta_aprobacion_docs.RequeridoInicio,      " &
                                "          ta_aprobacion_docs.RequeridoFin          " &
                                "     FROM ta_docs_soporte                          " &
                                "      INNER JOIN ta_aprobacion_docs ON ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte       " &
                                "     WHERE (ta_docs_soporte.id_programa = {0})                                                                  " &
                                "         AND (ta_aprobacion_docs.id_tipoDocumento = {1})                                                        " &
                                "         AND (ta_aprobacion_docs.id_doc_soporte  NOT IN (SELECT id_doc_soporte                                  " &
                                "                                                            FROM ta_archivos_documento_temp                     " &
                                "									                            WHERE (id_sesion_temp = '{2}')) )  ", id_programa, id_doc, id_sess)

            get_Approval_DocumentSupportPending_ByTMP = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Approval_DocumentSupportPending_ByTMP.Rows.Count = 1 And get_Approval_DocumentSupportPending_ByTMP.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Approval_DocumentSupportPending_ByTMP.Rows.Remove(get_Approval_DocumentSupportPending_ByTMP.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Approval_DocumentSupportSELECTED(ByVal id_doc As Integer) As DataTable

            Sql = String.Format(" SELECT ta_docs_soporte.id_doc_soporte,            " &
                                "          ta_docs_soporte.nombre_documento,        " &
                                "          ta_docs_soporte.id_programa,             " &
                                "          ta_aprobacion_docs.id_app_docs,          " &
                                "          ta_aprobacion_docs.id_tipoDocumento,     " &
                                "          ta_aprobacion_docs.PermiteRepetir,       " &
                                "          ta_aprobacion_docs.RequeridoInicio,      " &
                                "          ta_aprobacion_docs.RequeridoFin          " &
                                "     FROM ta_docs_soporte                          " &
                                "      INNER JOIN ta_aprobacion_docs ON ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte       " &
                                "     WHERE (ta_docs_soporte.id_programa = {0})                                                                  " &
                                "         AND (ta_aprobacion_docs.id_tipoDocumento = {1}) ", id_programa, id_doc)

            get_Approval_DocumentSupportSELECTED = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Approval_DocumentSupportSELECTED.Rows.Count = 1 And get_Approval_DocumentSupportSELECTED.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Approval_DocumentSupportSELECTED.Rows.Remove(get_Approval_DocumentSupportSELECTED.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_DocumentSupport_Extension(ByVal id_docSup As Integer) As DataTable

            Sql = String.Format("SELECT *  FROM ta_docs_soporte where id_programa = {0}  and id_doc_soporte = {1} ", id_programa, id_docSup)

            get_DocumentSupport_Extension = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_docSup, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_DocumentSupport_Extension.Rows.Count = 1 And get_DocumentSupport_Extension.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_DocumentSupport_Extension.Rows.Remove(get_DocumentSupport_Extension.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Doc_support_Route(ByVal id_TipoDoc As Integer) As DataTable 'This function gave me the all completes files

            Sql = String.Format("SELECT ta_docs_soporte.id_doc_soporte, ta_docs_soporte.nombre_documento, ta_docs_soporte.id_programa, ta_aprobacion_docs.id_app_docs, ta_aprobacion_docs.id_tipoDocumento " &
                                "    FROM ta_docs_soporte   " &
                                "         INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte  " &
                                 "            WHERE ( ta_docs_soporte.id_programa = {0} and ta_aprobacion_docs.id_tipoDocumento = {1} ) ", id_programa, id_TipoDoc)

            get_Doc_support_Route = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route.Rows.Count = 1 And get_Doc_support_Route.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route.Rows.Remove(get_Doc_support_Route.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function





        Public Function get_Doc_support_Route_PendingALL(ByVal id_TipoDoc As Integer, ByVal IdDoc As Integer) As DataTable 'This function gave me the all completes files

            Sql = String.Format("SELECT ta_docs_soporte.id_doc_soporte, 
				                    ta_docs_soporte.nombre_documento, 
				                    ta_docs_soporte.id_programa, 
                                    ta_docs_soporte.Template, 
                                    ta_docs_soporte.extension, 
                                    ta_docs_soporte.max_size, 
				                    ta_aprobacion_docs.id_app_docs, 
				                    ta_aprobacion_docs.id_tipoDocumento,
                                    ta_aprobacion_docs.PermiteRepetir, 
                                    ta_aprobacion_docs.RequeridoInicio, 
                                    ta_aprobacion_docs.RequeridoFin          
				                    FROM ta_docs_soporte            
				                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
				                    WHERE ( ta_docs_soporte.id_programa = {0} 
				                     and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
				                     AND (ta_aprobacion_docs.id_doc_soporte  NOT IN (SELECT a.id_doc_soporte 
																                     FROM ta_archivos_documento a
																                      inner join ta_AppDocumento b on (a.id_App_Documento = b.id_App_Documento)
																                       inner join ta_documento c on (b.id_documento = c.id_documento)
																                        WHERE (c.id_documento ={2}))
                                       OR ta_aprobacion_docs.PermiteRepetir='SI' ) ", id_programa, id_TipoDoc, IdDoc)

            get_Doc_support_Route_PendingALL = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_PendingALL.Rows.Count = 1 And get_Doc_support_Route_PendingALL.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_PendingALL.Rows.Remove(get_Doc_support_Route_PendingALL.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Doc_support_By_Route_Pending(ByVal id_TipoDoc As Integer, ByVal IdDoc As Integer, ByVal idRuta As Integer) As DataTable 'This function gave me the all completes files

            Sql = String.Format("SELECT     ta_docs_soporte.id_doc_soporte, 
		                                    ta_docs_soporte.nombre_documento, 
		                                    ta_docs_soporte.id_programa, 
                                            ta_docs_soporte.Template, 
                                            ta_docs_soporte.extension, 
                                            ta_docs_soporte.max_size, 
		                                    ta_aprobacion_docs.id_app_docs, 
		                                    ta_aprobacion_docs.id_tipoDocumento,
                                            ta_aprobacion_docs.PermiteRepetir, 
                                            ta_aprobacion_docs.RequeridoInicio, 
                                            ta_aprobacion_docs.RequeridoFin,
		                                    ta_aprobacion_docs.id_ruta, 
		                                    tab.id_ruta as ruta_   
                                       FROM ta_docs_soporte            
		                                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte   
		                                    LEFT JOIN  (SELECT  distinct a.id_doc_soporte, b.id_ruta 
					                                     FROM ta_archivos_documento a
					                                      inner join ta_AppDocumento b on (a.id_App_Documento = b.id_App_Documento)
					                                       inner join ta_documento c on (b.id_documento = c.id_documento)
						                                    WHERE (c.id_documento = {2})) as tab ON (ta_aprobacion_docs.id_doc_soporte = tab.id_doc_soporte)						 
		                                    WHERE ( ta_docs_soporte.id_programa = {0} 
		                                     and ta_aprobacion_docs.id_tipoDocumento = {1} 
		                                     and ta_aprobacion_docs.id_ruta = 0
		                                     and (tab.id_doc_soporte IS NULL  OR ta_aprobacion_docs.PermiteRepetir = 'SI' ) )
                              UNION
                                    SELECT  ta_docs_soporte.id_doc_soporte, 
		                                    ta_docs_soporte.nombre_documento, 
		                                    ta_docs_soporte.id_programa, 
                                            ta_docs_soporte.Template, 
                                            ta_docs_soporte.extension, 
                                            ta_docs_soporte.max_size, 
		                                    ta_aprobacion_docs.id_app_docs, 
		                                    ta_aprobacion_docs.id_tipoDocumento,
                                            ta_aprobacion_docs.PermiteRepetir, 
                                            ta_aprobacion_docs.RequeridoInicio, 
                                            ta_aprobacion_docs.RequeridoFin,
		                                    ta_aprobacion_docs.id_ruta, 
		                                    tab.id_ruta as ruta_    
                                       FROM ta_docs_soporte            
		                                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte   
		                                    LEFT JOIN  (SELECT distinct a.id_doc_soporte, b.id_ruta 
					                                     FROM ta_archivos_documento a
					                                      inner join ta_AppDocumento b on (a.id_App_Documento = b.id_App_Documento)
					                                       inner join ta_documento c on (b.id_documento = c.id_documento)
						                                    WHERE (c.id_documento = {2})) as tab ON (ta_aprobacion_docs.id_doc_soporte = tab.id_doc_soporte and ta_aprobacion_docs.id_ruta = tab.id_ruta )						 
		                                    WHERE ( ta_docs_soporte.id_programa = {0} 
		                                     and ta_aprobacion_docs.id_tipoDocumento = {1}
		                                     and  ta_aprobacion_docs.id_ruta = {3} )
		                                     and (tab.id_doc_soporte IS NULL  OR ta_aprobacion_docs.PermiteRepetir = 'SI' )", id_programa, id_TipoDoc, IdDoc, idRuta)

            get_Doc_support_By_Route_Pending = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_By_Route_Pending.Rows.Count = 1 And get_Doc_support_By_Route_Pending.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_By_Route_Pending.Rows.Remove(get_Doc_support_By_Route_Pending.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_Doc_support_Route_Pending(ByVal id_TipoDoc As Integer, ByVal IdDoc As Integer) As DataTable 'This function gave me the all completes files

            Sql = String.Format("SELECT ta_docs_soporte.id_doc_soporte, 
				                    ta_docs_soporte.nombre_documento, 
				                    ta_docs_soporte.id_programa, 
				                    ta_aprobacion_docs.id_app_docs, 
				                    ta_aprobacion_docs.id_tipoDocumento,  
                                    ta_aprobacion_docs.PermiteRepetir, 
                                    ta_aprobacion_docs.RequeridoInicio, 
                                    ta_aprobacion_docs.RequeridoFin       
				                    FROM ta_docs_soporte            
				                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
				                    WHERE ( ta_docs_soporte.id_programa = {0} 
				                     and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
				                     AND (ta_aprobacion_docs.id_doc_soporte  NOT IN (SELECT a.id_doc_soporte 
																                     FROM ta_archivos_documento a
																                      inner join ta_AppDocumento b on (a.id_App_Documento = b.id_App_Documento)
																                       inner join ta_documento c on (b.id_documento = c.id_documento)
																                        WHERE (c.id_documento ={2})) ) ", id_programa, id_TipoDoc, IdDoc)

            get_Doc_support_Route_Pending = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_Pending.Rows.Count = 1 And get_Doc_support_Route_Pending.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_Pending.Rows.Remove(get_Doc_support_Route_Pending.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_UserRoles(Optional ByVal id_usr As Integer = 0, Optional ByVal Roles As String = "") As DataTable

            Dim strSQLc As String = ""
            Dim strSQLc2 As String = ""

            strSQLc = IIf(id_usr > 0, String.Format("   And ( id_usuario = {0} )  ", id_usr), "")

            strSQLc2 = IIf(Roles.Length > 0, String.Format("   And ( id_rol In {0} )  ", Roles), "")

            Sql = String.Format(" Select distinct id_programa, id_rol, nombre_rol, id_usuario from vw_roles_approvals " &
                                "  where id_programa = {0} {1} {3} ", id_programa, strSQLc, strSQLc2)

            get_UserRoles = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_UserRoles.Rows.Count = 1 And get_UserRoles.Rows.Item(0).Item("id_rol") = 0) Then
                get_UserRoles.Rows.Remove(get_UserRoles.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function



        Public Function get_TipoDoc(ByVal id_category As Integer) As DataTable

            Sql = String.Format("select * from ta_tipo_aprobacion where id_programa = {0} and id_categoria = {1} ", id_programa, id_category)

            get_TipoDoc = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_TipoDoc.Rows.Count = 1 And get_TipoDoc.Rows.Item(0).Item("id_programa") = 0) Then
                get_TipoDoc.Rows.Remove(get_TipoDoc.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_UserRolesALL(ByVal id_usr As Integer) As DataTable '--Include any Groups too


            Sql = String.Format("  Select distinct id_programa, id_rol, nombre_rol, id_usuario from vw_roles_approvals    " &
                                "     where id_programa = {0}       " &
                                "             And orden = 0         " &
                                "             And id_usuario = {1}  " &
                                " UNION ALL                         " &
                                " Select distinct id_programa, id_rol, nombre_rol, id_usuario from vw_roles_approvals    " &
                                "     where id_programa = {0}       " &
                                "             And orden = 0         " &
                                "             And id_rol In (  Select a.id_rol                                           " &
                                "                                From ta_gruposRoles  a                                  " &
                                "                                 inner join ta_grupos b On (a.id_grupo = b.id_grupo)    " &
                                "                                  Where b.id_programa = {0}     " &
                                "		                                And a.aprueba='SI'       " &
                                "	                                    and a.id_usuario = {1}) ", id_programa, id_usr)

            get_UserRolesALL = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_UserRolesALL.Rows.Count = 1 And get_UserRolesALL.Rows.Item(0).Item("id_rol") = 0) Then
                get_UserRolesALL.Rows.Remove(get_UserRolesALL.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Groups_User(ByVal id_usr As Integer) As DataTable

            Sql = String.Format("  Select b.id_programa, a.id_grupo, b.nombre_grupo, a.id_rol, a.id_usuario, a.aprueba, a.comenta, a.consulta " &
                                "     from  ta_gruposRoles  a  " &
                                "       inner join ta_grupos b On (a.id_grupo = b.id_grupo) " &
                                "  WHERE a.aprueba='SI' and b.id_programa = {0} and a.id_usuario = {1} ", id_programa, id_usr)

            get_Groups_User = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Groups_User.Rows.Count = 1 And get_Groups_User.Rows.Item(0).Item("id_programa") = 0) Then
                get_Groups_User.Rows.Remove(get_Groups_User.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************



        End Function


        Public Function get_Route_By_DocumentType(ByVal id_doc As Integer, Optional ByVal order As Integer = 0) As DataTable

            Sql = String.Format(" SELECT * FROM vw_ta_rutas_tipoDocumento WHERE (id_tipoDocumento = {0})  AND  (orden = {1}) ", id_doc, order)

            get_Route_By_DocumentType = cl_utl.setObjeto("vw_ta_rutas_tipoDocumento", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Route_By_DocumentType.Rows.Count = 1 And get_Route_By_DocumentType.Rows.Item(0).Item("id_ruta") = 0) Then
                get_Route_By_DocumentType.Rows.Remove(get_Route_By_DocumentType.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function


        Public Function get_Route_By_DocumentType(ByVal id_ruta As Integer) As DataTable

            Sql = String.Format(" SELECT * FROM vw_ta_rutas_tipoDocumento WHERE (id_ruta = {0} ) ", id_ruta)

            get_Route_By_DocumentType = cl_utl.setObjeto("vw_ta_rutas_tipoDocumento", "id_ruta", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Route_By_DocumentType.Rows.Count = 1 And get_Route_By_DocumentType.Rows.Item(0).Item("id_ruta") = 0) Then
                get_Route_By_DocumentType.Rows.Remove(get_Route_By_DocumentType.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function



        Public Function get_Approvals(Optional ByVal id_tipoDoc As Integer = 0) As DataTable

            Dim strSQLC As String = ""
            If id_tipoDoc > 0 Then
                strSQLC = String.Format(" where id_tipoDocumento = {0} ", id_tipoDoc)
            End If

            Sql = String.Format("select * from vw_aprobaciones {0} ", strSQLC)

            get_Approvals = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Approvals.Rows.Count = 1 And get_Approvals.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_Approvals.Rows.Remove(get_Approvals.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function Approval_CodeCreate(ByVal id_Cat As Integer) As String

            Dim tbl_ta_categoria As New DataTable
            Dim Correlativo As Integer
            Dim strCorr As String

            If id_Cat > 0 Then

                tbl_ta_categoria = cl_utl.setObjeto("ta_categoria", "id_categoria", id_Cat)

                If tbl_ta_categoria.Rows.Item(0).Item("id_categoria") > 0 Then
                    Correlativo = Val(tbl_ta_categoria.Rows.Item(0).Item("correlativos")) + 1
                    strCorr = String.Format(" {0}-{1:000} ", tbl_ta_categoria.Rows.Item(0).Item("cod_categoria").ToString.Trim, Correlativo)
                Else
                    strCorr = ""
                End If

            Else
                strCorr = ""
            End If

            Approval_CodeCreate = strCorr

        End Function


        Public Function Approval_CategoryCode_UPD(ByVal id_Cat As Integer) As Boolean

            Dim tbl_ta_categoria As New DataTable
            Dim Correlativo As Integer


            If id_Cat > 0 Then

                tbl_ta_categoria = cl_utl.setObjeto("ta_categoria", "id_categoria", id_Cat)

                Correlativo = CType(cl_utl.getDTval(tbl_ta_categoria, "id_categoria", "correlativos", id_Cat), Integer) + 1

                cl_utl.setDTval(tbl_ta_categoria, "id_categoria", "correlativos", id_Cat, Correlativo)

                Sql = String.Format(" UPDATE ta_categoria SET correlativos = {1}  WHERE id_categoria= {0} ", id_Cat, Correlativo)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    Approval_CategoryCode_UPD = True

                Catch ex As Exception
                    Approval_CategoryCode_UPD = False
                End Try

            Else

                Approval_CategoryCode_UPD = False

            End If


        End Function


        Public Function get_ta_archivos_documento_temp(ByVal id_Sess As String) As DataTable

            Sql = String.Format("select id_archivo_temp, id_sesion_temp, id_documento, archivo, id_doc_soporte from  ta_archivos_documento_temp where id_sesion_temp = '{0}' ", id_Sess)

            get_ta_archivos_documento_temp = cl_utl.setObjeto("ta_archivos_documento_temp", "id_archivo_temp", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_archivos_documento_temp.Rows.Count = 1 And get_ta_archivos_documento_temp.Rows.Item(0).Item("id_archivo_temp") = 0) Then
                get_ta_archivos_documento_temp.Rows.Remove(get_ta_archivos_documento_temp.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_archivos_documento_ByApp(ByVal id_doc As String) As DataTable
            ' --Return All documents by App documents

            Sql = String.Format("SELECT ta_AppDocumento.id_documento, ta_AppDocumento.id_ruta, ta_AppDocumento.id_App_Documento, ta_docs_soporte.id_doc_soporte, ta_archivos_documento.id_archivo, ta_archivos_documento.archivo, 'v1.' + rtrim(convert(char(2), ta_archivos_documento.ver)) as ver, ta_roles.nombre_rol, ta_docs_soporte.nombre_documento, ta_docs_soporte.extension  " &
                                "      FROM ta_archivos_documento " &
                                "         INNER JOIN ta_AppDocumento ON ta_archivos_documento.id_App_Documento = ta_AppDocumento.id_App_Documento  " &
                                "           INNER JOIN ta_docs_soporte ON ta_archivos_documento.id_doc_soporte = ta_docs_soporte.id_doc_soporte  " &
                                "             INNER JOIN ta_rutaTipoDoc ON ta_rutaTipoDoc.id_ruta = dbo.ta_AppDocumento.id_ruta " &
                                "               INNER JOIN ta_roles on ta_rutaTipoDoc.id_rol = ta_roles.id_rol    " &
                                "    WHERE (ta_AppDocumento.id_documento = {0}) 
                                   order by ta_docs_soporte.id_doc_soporte, ta_archivos_documento.archivo, ta_archivos_documento.ver ", id_doc)

            get_ta_archivos_documento_ByApp = cl_utl.setObjeto("ta_archivos_documento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_archivos_documento_ByApp.Rows.Count = 1 And get_ta_archivos_documento_ByApp.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_archivos_documento_ByApp.Rows.Remove(get_ta_archivos_documento_ByApp.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_RutaSeguimiento(ByVal id_doc As String) As DataTable

            Sql = String.Format("SELECT * FROM dbo.FN_Ta_RutaSeguimiento({0}) ORDER BY id_app_documento ", id_doc)

            get_ta_RutaSeguimiento = cl_utl.setObjeto("FN_Ta_RutaSeguimiento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_RutaSeguimiento.Rows.Count = 1 And get_ta_RutaSeguimiento.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_RutaSeguimiento.Rows.Remove(get_ta_RutaSeguimiento.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_AppDocumentoOrden_MAX(ByVal id_doc As String) As DataTable

            'id_App_Documento, id_tipoDocumento, nextFase, id_role_next, id_ruta_next, id_usuario_next, nombre_rol, id_rol, id_ruta, id_usuario_app, id_usuario, descripcion_estado, nombre_empleado, completo, observacion
            Sql = String.Format("SELECT a.* " &
                                "  FROM  vw_ta_AppDocumento a " &
                                "    INNER JOIN  VW_GR_MAX_APP_MAX_ORDER b on (a.id_app_documento = b.id_app_documento)  " &
                                "        WHERE ( a.id_documento = {0} ) ", id_doc)

            get_ta_AppDocumentoOrden_MAX = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_AppDocumentoOrden_MAX.Rows.Count = 1 And get_ta_AppDocumentoOrden_MAX.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_AppDocumentoOrden_MAX.Rows.Remove(get_ta_AppDocumentoOrden_MAX.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_AppDocumentoAPP_MAX(ByVal id_doc As String) As DataTable

            'id_App_Documento, id_tipoDocumento, nextFase, id_role_next, id_ruta_next, id_usuario_next, nombre_rol, id_rol, id_ruta, id_usuario_app, id_usuario, descripcion_estado, nombre_empleado, completo, observacion
            Sql = String.Format("SELECT a.* " &
                                "  FROM  vw_ta_AppDocumento a " &
                                "    INNER JOIN VW_GR_MAX_APP_DOC b on (a.id_app_documento = b.id_app_documento)  " &
                                "        WHERE ( a.id_documento = {0} ) ", id_doc)

            get_ta_AppDocumentoAPP_MAX = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_AppDocumentoAPP_MAX.Rows.Count = 1 And get_ta_AppDocumentoAPP_MAX.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_AppDocumentoAPP_MAX.Rows.Remove(get_ta_AppDocumentoAPP_MAX.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_ta_AppDocumentoOrden_MIN(ByVal id_doc As String) As DataTable

            'a.id_App_Documento, a.id_tipoDocumento, a.nextFase, a.id_role_next, a.id_ruta_next, a.id_usuario_next, a.nombre_rol, a.id_rol, a.id_ruta, a.id_usuario_app,  a.id_usuario, a.descripcion_estado, a.nombre_empleado, a.completo, a.observacion
            Sql = String.Format("SELECT a.*  " &
                                "	    FROM  vw_ta_AppDocumento a    " &
                                "         inner join VW_GR_DOCUMENTOS_ESTADOS_DOWN_W2 b on (a.id_app_documento = b.id_app_documento) " &
                                "            WHERE (a.id_documento = {0})  ", id_doc)

            get_ta_AppDocumentoOrden_MIN = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_AppDocumentoOrden_MIN.Rows.Count = 1 And get_ta_AppDocumentoOrden_MIN.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_AppDocumentoOrden_MIN.Rows.Remove(get_ta_AppDocumentoOrden_MIN.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_vw_ta_AppDocumento(Optional id_doc As Integer = 0, Optional id_app_doc As Integer = 0) As DataTable

            Dim strWHERE As String = ""

            If id_doc <> 0 Then
                strWHERE = String.Format(" WHERE (a.id_documento = {0} ) ", id_doc.ToString)
            Else
                strWHERE = String.Format(" WHERE (a.id_app_documento = {0} ) ", id_app_doc.ToString)
            End If

            Sql = String.Format("select     a.id_App_Documento,
			                                a.id_documento,
			                                a.id_ruta,
			                                b.orden,
			                                a.fecha_aprobacion,
			                                a.datecreated,
			                                a.fecha_limite,
			                                a.fecha_recepcion,
			                                a.id_estadoDoc,
			                                a.observacion,
			                                a.id_usuario_app,
			                                a.id_role_app,
			                                c.estado_tipo_prefijo,
			                                f.nombre_usuario,
			                                f.email_usuario,
			                                f.job,
			                                e.numero_instrumento,
			                                e.codigo_Approval			
			                          from ta_AppDocumento a
			                      inner join ta_rutaTipoDoc b on (a.id_ruta = b.id_Ruta)
			                       inner join ta_estadoTipo c on (c.id_estadoTipo = b.id_estadoTipo)
			                        inner join ta_Documento e on (a.id_documento = e.id_documento)
			                         inner join vw_t_usuarios f on (f.id_usuario = a.id_usuario_app and f.id_programa = e.id_programa) {0} ", strWHERE)

            get_vw_ta_AppDocumento = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_vw_ta_AppDocumento.Rows.Count = 1 And get_vw_ta_AppDocumento.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_vw_ta_AppDocumento.Rows.Remove(get_vw_ta_AppDocumento.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_ta_AppDocumento_byOrden(ByVal id_doc As Integer, ByVal vOrden As Integer) As DataTable

            Sql = String.Format("SELECT id_App_Documento, id_tipoDocumento, nextFase, id_role_next, id_ruta_next, id_usuario_next, nombre_rol, id_rol, id_ruta, " &
                                "    id_usuario, descripcion_estado, nombre_empleado, completo, observacion, id_usuario_app  " &
                                "	    FROM  vw_ta_AppDocumento    " &
                                "            WHERE (id_documento = {0})  " &
                                "                AND  (orden = {1}) ", id_doc, vOrden)

            get_ta_AppDocumento_byOrden = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_AppDocumento_byOrden.Rows.Count = 1 And get_ta_AppDocumento_byOrden.Rows.Item(0).Item("id_App_Documento") = 0) Then
                get_ta_AppDocumento_byOrden.Rows.Remove(get_ta_AppDocumento_byOrden.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function IS_User_StepMAX_Permitted(ByVal id_doc As Integer, ByVal id_usu As Integer) As Boolean


            'Sql = String.Format("	 SELECT id_rol, id_usuario FROM vw_ta_AppDocumento         
            '                          WHERE id_documento = {0}        
            '                            AND ( id_usuario = {1} or  id_rol = {2} )          
            '                               And (orden = (Select orden FROM vw_ta_AppDocumento a 
            '                                         inner join VW_GR_DOCUMENTOS_ESTADOS_UP_W2 b on (a.id_documento = b.id_documento and a.id_App_Documento = b.id_app_documento)
            '                                          WHERE a.id_documento ={0}))              
            '                      UNION                     
            '                          Select id_rol, id_usuario           
            '                          FROM  ta_gruposRoles       
            '                                     WHERE ( id_usuario = {1} or  id_rol = {2} )          
            '                                      AND aprueba='SI'    
            '                                And  (id_rol = ((Select id_rol FROM vw_ta_AppDocumento a 
            '                                                            inner join VW_GR_DOCUMENTOS_ESTADOS_UP_W2 b on (a.id_documento = b.id_documento and a.id_App_Documento = b.id_app_documento)
            '                                                           WHERE a.id_documento ={0})))", id_doc, id_usu, idR)



            Sql = String.Format("   select a.id_rol, a.id_usuario    --- MAX BY USER
                                            FROM vw_ta_AppDocumento  a       	                                    
	                                         INNER JOIN 
                                               (SELECT a.id_documento, MAX(a.id_app_documento) AS id_app_documento 
 	                                              FROM dbo.ta_AppDocumento a								 				             
	                                               where a.id_documento = {0}
                                                    group by a.id_documento) as b on (a.id_App_Documento = b.id_app_documento)
                                            where a.id_usuario = {1} 
                                         UNION	
                                          select distinct a.id_rol, c.id_usuario    --- MAX BY ROLE, because Shared Roles
                                            FROM vw_ta_AppDocumento  a       	                                    
	                                         INNER JOIN 
                                               (SELECT a.id_documento, MAX(a.id_app_documento) AS id_app_documento 
 	                                              FROM dbo.ta_AppDocumento a								 				             
	                                               where a.id_documento = {0} 
                                                    group by a.id_documento) as b on (a.id_App_Documento = b.id_app_documento)  
                                               INNER JOIN vw_roles_approvals c on (a.id_rol = c.id_rol)
	                                          where c.id_usuario = {1}  
                                           UNION
                                           select distinct a.id_rol, c.id_usuario --- MAX BY GROUP 
                                            FROM vw_ta_AppDocumento  a       	                                    
	                                         INNER JOIN 
                                               (SELECT a.id_documento, MAX(a.id_app_documento) AS id_app_documento 
 	                                              FROM dbo.ta_AppDocumento a								 				             
	                                               where a.id_documento = {0} 
                                                    group by a.id_documento) as b on (a.id_App_Documento = b.id_app_documento)
	                                            INNER JOIN vw_grupos_roles_approvals c on (a.id_rol = c.id_rol)
                                             where c.id_usuario = {1} and c.aprueba = 'SI' ", id_doc, id_usu)


            Dim tbl_User_Rol As New DataTable

            tbl_User_Rol = cl_utl.setObjeto("vw_ta_AppDocumento", "id_App_Documento", 0, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_User_Rol.Rows.Count = 1 And tbl_User_Rol.Rows.Item(0).Item("id_rol") = 0) Then
                tbl_User_Rol.Rows.Remove(tbl_User_Rol.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            If tbl_User_Rol.Rows.Count = 0 Then
                IS_User_StepMAX_Permitted = False
            Else
                IS_User_StepMAX_Permitted = True
            End If

        End Function


        Public Function get_Document(ByVal id_doc As Integer) As DataTable

            Sql = String.Format("SELECT  ROW_NUMBER() OVER(ORDER BY id_archivo DESC) as Number,* FROM vw_ta_archivos_documento where id_documento = {0} ", id_doc)

            get_Document = cl_utl.setObjeto("vw_ta_archivos_documento", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Document.Rows.Count = 1 And get_Document.Rows.Item(0).Item("id_documento") = 0) Then
                get_Document.Rows.Remove(get_Document.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Document_Comments_special(ByVal id_doc As Integer) As DataTable

            Sql = String.Format("select * from FN_getting_Comments({0})", id_doc)
            get_Document_Comments_special = cl_utl.setObjeto("vw_ta_comentariosDoc", "id_Documento", id_doc, Sql)

        End Function


        Public Function get_DocumentINFO(ByVal id_doc As Integer) As DataTable

            Sql = String.Format("SELECT  * FROM VW_GR_TA_DOCUMENTOS  where id_documento = {0} ", id_doc)

            get_DocumentINFO = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", id_doc, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_DocumentINFO.Rows.Count = 1 And get_DocumentINFO.Rows.Item(0).Item("id_documento") = 0) Then
                get_DocumentINFO.Rows.Remove(get_DocumentINFO.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        '**********************************************PROGRAM**********************************************

        Public Function get_ProgramINFO(Optional ByVal AllFields As Boolean = False) As DataTable

            Dim strFiels As String = IIf(AllFields, " * ", " id_programa, nombre_Programa ")

            Dim strSQL As String = String.Format(" Select {0} from vw_t_programasALL ", strFiels)

            If id_programa > 0 Then
                strSQL &= String.Format(" where id_programa = {0} ", id_programa)
            End If

            tbl_t_Programa = cl_utl.setObjeto("vw_t_programasALL", "id_programa", 0, strSQL).Copy

            get_ProgramINFO = tbl_t_Programa

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ProgramINFO.Rows.Count = 1 And get_ProgramINFO.Rows.Item(0).Item("id_programa") = 0) Then
                get_ProgramINFO.Rows.Remove(get_ProgramINFO.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

        '***********************************************PROGRAM**********************************************









        '********************************************************* ta_archivos_documento_temp ENTITY ************************************************************************************
        '********************************************************* ta_archivos_documento_temp  ENTITY ************************************************************************************

        Public Function set_ta_archivos_documento_temp(ByVal id_archTemp As Integer) As DataTable

            id_archivo_temp = IIf(id_archTemp > 0, id_archTemp, 0)

            tbl_ta_archivos_documento_temp = cl_utl.setObjeto("ta_archivos_documento_temp", "id_archivo_temp", id_archivo_temp)
            set_ta_archivos_documento_temp = tbl_ta_archivos_documento_temp

        End Function

        Public Function get_ta_archivos_documento_tempFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_archivos_documento_temp, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_archivos_documento_tempFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_archivos_documento_temp = cl_utl.setDTval(tbl_ta_archivos_documento_temp, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_archivos_documento_temp() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_archivos_documento_temp", tbl_ta_archivos_documento_temp, "id_archivo_temp", id_archivo_temp)

            If RES <> -1 Then
                set_ta_archivos_documento_tempFIELDS("id_archivo_temp", RES, "id_archivo_temp", id_archivo_temp)
                id_archivo_temp = RES
                save_ta_archivos_documento_temp = RES
            Else
                save_ta_archivos_documento_temp = RES
            End If

        End Function

        Public Function del_ta_archivos_documento_temp(ByVal idTemp As String) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_archivos_documento_temp WHERE (id_archivo_temp = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_archivos_documento_temp = True

                Catch ex As Exception
                    del_ta_archivos_documento_temp = False
                    cnnSAP.Close()
                End Try

            Else

                del_ta_archivos_documento_temp = False

            End If

        End Function


        '********************************************************* ta_archivos_documento_temp  ENTITY ************************************************************************************
        '********************************************************* ta_archivos_documento_temp  ENTITY ************************************************************************************





        '********************************************************* ta_documento ENTITY ************************************************************************************
        '********************************************************* ta_documento  ENTITY ************************************************************************************

        Public Function set_ta_documento(ByVal id_Doc As Integer) As DataTable

            id_Documento = IIf(id_Doc > 0, id_Doc, 0)
            tbl_ta_documento = cl_utl.setObjeto("ta_documento", "id_documento", id_Documento)
            set_ta_documento = tbl_ta_documento

        End Function

        Public Function get_ta_documentoFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_documento, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_documentoFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_documento = cl_utl.setDTval(tbl_ta_documento, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_documento() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_documento", tbl_ta_documento, "id_documento", id_Documento)

            If RES <> -1 Then
                set_ta_documentoFIELDS("id_documento", RES, "id_documento", id_Documento)
                id_Documento = RES
                save_ta_documento = RES
            Else
                save_ta_documento = RES
            End If

        End Function

        Public Function del_ta_documento(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_documento WHERE (id_documento = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_documento = True

                Catch ex As Exception
                    del_ta_documento = False
                End Try

            Else

                del_ta_documento = False

            End If

        End Function


        '********************************************************* ta_documento  ENTITY ************************************************************************************
        '********************************************************* ta_documento  ENTITY ************************************************************************************

        '********************************************************* ta_documento_ambiental_ambiental ENTITY ************************************************************************************
        '********************************************************* ta_documento_ambiental_ambiental  ENTITY ************************************************************************************

        Public Function set_ta_documento_ambiental(ByVal id_Doc As Integer) As DataTable

            id_documento_ambiental = IIf(id_Doc > 0, id_Doc, 0)
            tbl_ta_documento_ambiental = cl_utl.setObjeto("ta_documento_ambiental", "id_documento_ambiental", id_documento_ambiental)
            set_ta_documento_ambiental = tbl_ta_documento_ambiental

        End Function

        Public Function get_ta_documento_ambientalFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_documento_ambiental, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_documento_ambientalFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_documento_ambiental = cl_utl.setDTval(tbl_ta_documento_ambiental, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_documento_ambiental() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_documento_ambiental", tbl_ta_documento_ambiental, "id_documento_ambiental", id_documento_ambiental)

            If RES <> -1 Then
                set_ta_documento_ambientalFIELDS("id_documento_ambiental", RES, "id_documento_ambiental", id_documento_ambiental)
                id_documento_ambiental = RES
                save_ta_documento_ambiental = RES
            Else
                save_ta_documento_ambiental = RES
            End If

        End Function

        Public Function del_ta_documento_ambiental(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_documento_ambiental WHERE (id_documento_ambiental = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_documento_ambiental = True

                Catch ex As Exception
                    del_ta_documento_ambiental = False
                End Try

            Else

                del_ta_documento_ambiental = False

            End If

        End Function


        '********************************************************* ta_documento_ambiental_ambiental ENTITY ************************************************************************************
        '********************************************************* ta_documento_ambiental_ambiental  ENTITY ************************************************************************************




        '********************************************************* ta_categoria  ENTITY ************************************************************************************
        '********************************************************* ta_categoria  ENTITY ************************************************************************************




        '********************************************************* ta_categoria  ENTITY ************************************************************************************
        '********************************************************* ta_categoria  ENTITY ************************************************************************************



        '********************************************************* ta_AppDocumento ENTITY ************************************************************************************
        '********************************************************* ta_AppDocumento  ENTITY ************************************************************************************

        Public Function set_ta_AppDocumento(ByVal id_AppDoc As Integer) As DataTable

            id_App_Documento = IIf(id_AppDoc > 0, id_AppDoc, 0)
            tbl_ta_AppDocumento = cl_utl.setObjeto("ta_AppDocumento", "id_App_Documento", id_App_Documento)
            set_ta_AppDocumento = tbl_ta_AppDocumento

        End Function

        Public Function get_ta_AppDocumentoFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_AppDocumento, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_AppDocumentoFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_AppDocumento = cl_utl.setDTval(tbl_ta_AppDocumento, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_AppDocumento() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_AppDocumento", tbl_ta_AppDocumento, "id_App_Documento", id_App_Documento)

            If RES <> -1 Then
                set_ta_AppDocumentoFIELDS("id_App_Documento", RES, "id_App_Documento", id_App_Documento)
                id_App_Documento = RES
                save_ta_AppDocumento = RES
            Else
                save_ta_AppDocumento = RES
            End If

        End Function

        Public Function del_ta_AppDocumento(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_AppDocumento WHERE (id_App_Documento = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_AppDocumento = True

                Catch ex As Exception
                    del_ta_AppDocumento = False
                End Try

            Else

                del_ta_AppDocumento = False

            End If

        End Function


        '********************************************************* ta_AppDocumento  ENTITY ************************************************************************************
        '********************************************************* ta_AppDocumento  ENTITY ************************************************************************************




        Public Function set_ta_archivos_documento(ByVal id_arch As Integer) As DataTable

            id_archivo = IIf(id_arch > 0, id_arch, 0)
            tbl_ta_archivos_documento = cl_utl.setObjeto("ta_archivos_documento", "id_archivo", id_archivo)
            set_ta_archivos_documento = tbl_ta_archivos_documento

        End Function

        Public Function get_ta_archivos_documentoFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_archivos_documento, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_archivos_documentoFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_archivos_documento = cl_utl.setDTval(tbl_ta_archivos_documento, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_archivos_documento() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_archivos_documento", tbl_ta_archivos_documento, "id_archivo", id_archivo)

            If RES <> -1 Then
                set_ta_archivos_documentoFIELDS("id_archivo", RES, "id_archivo", id_archivo)
                id_archivo = RES
                save_ta_archivos_documento = RES
            Else
                save_ta_archivos_documento = RES
            End If

        End Function

        Public Function del_ta_archivos_documento(ByVal id_Arch As Integer) As Boolean

            If id_Arch > 0 Then

                Sql = String.Format("DELETE FROM ta_archivos_documento WHERE (id_archivo = {0} ) ", id_Arch)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_archivos_documento = True

                Catch ex As Exception
                    del_ta_archivos_documento = False
                End Try

            Else

                del_ta_archivos_documento = False

            End If

        End Function


        '********************************************************* ta_archivos_documento  ENTITY ************************************************************************************
        '********************************************************* ta_archivos_documento  ENTITY ************************************************************************************


        '********************************************************* ta_DocumentosINFO  ENTITY ************************************************************************************
        '********************************************************* ta_DocumentosINFO   ENTITY ************************************************************************************



        Public Function get_ta_DocumentosINFO(ByVal id_Doc As Integer) As DataTable

            Sql = String.Format("Select * FROM vw_ta_documentos WHERE id_documento = {0} ", id_Doc)

            tbl_ta_DocumentosINFO = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_Doc, Sql)

            get_ta_DocumentosINFO = tbl_ta_DocumentosINFO

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_DocumentosINFO.Rows.Count = 1 And get_ta_DocumentosINFO.Rows.Item(0).Item("id_documento") = 0) Then
                get_ta_DocumentosINFO.Rows.Remove(get_ta_DocumentosINFO.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_DocumentosInfoFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_DocumentosINFO, campoSearch, campo, valorSearch)

        End Function


        '********************************************************* ta_DocumentosINFO  ENTITY ************************************************************************************
        '********************************************************* ta_DocumentosINFO   ENTITY ************************************************************************************



        '********************************************************* ta_comentariosDoc ENTITY ************************************************************************************
        '********************************************************* ta_comentariosDoc  ENTITY ************************************************************************************

        Public Function set_ta_comentariosDoc(ByVal id_Com As Integer) As DataTable

            id_comment = IIf(id_Com > 0, id_Com, 0)
            tbl_ta_comentariosDoc = cl_utl.setObjeto("ta_comentariosDoc", "id_comment", id_comment)
            set_ta_comentariosDoc = tbl_ta_comentariosDoc

        End Function

        Public Function get_ta_comentariosDocFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_comentariosDoc, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_comentariosDocFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_comentariosDoc = cl_utl.setDTval(tbl_ta_comentariosDoc, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_comentariosDoc() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_comentariosDoc", tbl_ta_comentariosDoc, "id_comment", id_comment)

            If RES <> -1 Then
                set_ta_comentariosDocFIELDS("id_comment", RES, "id_comment", id_comment)
                id_comment = RES
                save_ta_comentariosDoc = RES
            Else
                save_ta_comentariosDoc = RES
            End If

        End Function

        Public Function del_ta_comentariosDoc(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM ta_comentariosDoc WHERE (id_comment = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_ta_comentariosDoc = True

                Catch ex As Exception
                    del_ta_comentariosDoc = False
                End Try

            Else

                del_ta_comentariosDoc = False

            End If

        End Function


        '********************************************************* ta_comentariosDoc  ENTITY ************************************************************************************
        '********************************************************* ta_comentariosDoc  ENTITY ************************************************************************************




        Public Shared Function ConvertToDataTable(Of t)(
                                                 ByVal list As IList(Of t)
                                              ) As DataTable
            Dim table As New DataTable()
            If Not list.Any Then
                'don't know schema ....
                Return table
            End If
            Dim fields() = list.First.GetType.GetProperties
            For Each field In fields

                If IsNullableType(field.PropertyType) Then
                    Dim UnderlyingType As Type = Nullable.GetUnderlyingType(field.PropertyType)
                    table.Columns.Add(field.Name, UnderlyingType)
                Else
                    table.Columns.Add(field.Name, field.PropertyType)
                End If


            Next
            For Each item In list
                Dim row As DataRow = table.NewRow()
                For Each field In fields

                    Dim p = item.GetType.GetProperty(field.Name)

                    If (Not IsNothing(p.GetValue(item, Nothing))) Then
                        row(field.Name) = p.GetValue(item, Nothing)
                    Else
                        row(field.Name) = DBNull.Value
                    End If

                    'If (Not p.GetValue(item, Nothing) Is Nothing) AndAlso IsNullableType(p.GetType) Then
                    '    Dim UnderlyingType As Type = Nullable.GetUnderlyingType(p.GetType)
                    '    row(field.Name) = p.GetValue(Convert.ChangeType(item, UnderlyingType), Nothing)
                    'Else
                    '    row(field.Name) = p.GetValue(item, Nothing)
                    'End If

                Next
                table.Rows.Add(row)
            Next
            Return table
        End Function


        Public Shared Function IsNullableType(ByVal myType As Type) As Boolean
            Return (myType.IsGenericType) AndAlso (myType.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
        End Function


        Public Function TimeSheet_Update_Status(ByVal idEstado As Integer, ByVal idDoc As Integer, ByVal idUser As Integer) As Integer


            Using db As New dbRMS_JIEntities

                'Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                Dim TimeSheet As New ta_timesheet
                TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                TimeSheet.fecha_upd = Date.UtcNow
                TimeSheet.usuario_upd = idUser
                TimeSheet.id_timesheet_estado = idEstado


                Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(id_programa)
                Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))


                TimeSheet_Update_Status = result

            End Using

        End Function

        Public Function Deliverable_Update_Status(ByVal idEstado As Integer, ByVal idDoc As Integer, ByVal idUser As Integer, ByVal idRoute As Integer) As Integer

            Try

                Using db As New dbRMS_JIEntities

                    'Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim idDeliverable As Integer = db.ta_documento_deliverable.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_deliverable

                    Dim coDeliverable As New ta_deliverable
                    coDeliverable = db.ta_deliverable.Find(Convert.ToInt32(idDeliverable))

                    coDeliverable.fecha_upd = Date.UtcNow
                    coDeliverable.usuario_upd = idUser
                    coDeliverable.id_deliverable_estado = idEstado

                    Dim idDelieverableStage As Integer = db.vw_ta_ruta_aprobacion.Where(Function(p) p.id_ruta = idRoute).FirstOrDefault.id_deliverable_stage

                    'If idDelieverableStage = 1 Then 'Delievered
                    '    coDeliverable.fecha_entrego = Date.UtcNow

                    If idDelieverableStage = 2 Then 'Approved
                        coDeliverable.fecha_aprobo = Date.UtcNow
                        coDeliverable.id_deliverable_estado = 7 'Approved
                    ElseIf idDelieverableStage = 3 Then 'Disbursed
                        coDeliverable.fecha_complete = Date.UtcNow
                        coDeliverable.usuario_complete = idUser
                    End If

                    'idEstado = 3 Or 
                    If idEstado = 4 Or idEstado = 6 Then
                        coDeliverable.usuario_complete = idUser
                        'coDeliverable.fecha_complete = Date.UtcNow
                    End If

                    Dim cls_deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(id_programa)
                    Dim result = cls_deliverable.Save_deliverable(coDeliverable, idDeliverable)

                    If idEstado = 3 Then 'Or idEstado = 4 Or idEstado = 6 just Approved

                        Dim oFicha_Entregable As New tme_ficha_entregables
                        oFicha_Entregable = coDeliverable.tme_ficha_entregables

                        oFicha_Entregable.delivered_date = Date.UtcNow
                        oFicha_Entregable.valor_final = coDeliverable.valor_final
                        'oFicha_Entregable.tasa_cambio = coDeliverable.tasa_cambio

                        cls_deliverable.Save_Ficha_Entregable(oFicha_Entregable, coDeliverable.id_ficha_entregable)

                    End If

                    Deliverable_Update_Status = result

                End Using

            Catch ex As Exception

                Deliverable_Update_Status = "-1"

            End Try


        End Function


    End Class

End Namespace

