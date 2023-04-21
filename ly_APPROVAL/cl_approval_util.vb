
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports ly_SIME
Imports System.Net.Mail
Imports System.Net
Imports System.Web.UI.Page


Namespace APPROVAL


    Public Class cl_approval_util

        Dim Sql As String
        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Dim cl_utl As New CORE.cls_util
        Dim id_programa As Integer

        Public Sub New(ByVal id_p As Integer)

            id_programa = id_p

        End Sub



        Public Function get_ta_categoria(Optional ByVal id_cat As Integer = 0, Optional ByVal Fields As String = "") As DataTable

            Dim strFields As String = ""
            If strFields.Length > 1 Then
                strFields = Fields
            Else
                strFields = " * "
            End If

            Sql = String.Format("select {1} from ta_categoria where id_programa = {0} and visible = 'SI'", id_programa, strFields)

            get_ta_categoria = cl_utl.setObjeto("ta_categoria", "id_categoria", id_cat, Sql)

            If (get_ta_categoria.Rows.Count = 1 And get_ta_categoria.Rows.Item(0).Item("id_categoria") = 0) Then
                get_ta_categoria.Rows.Remove(get_ta_categoria.Rows.Item(0))
            End If


        End Function


        Public Function get_ta_categoriaBy_User(ByVal id_User As Integer) As DataTable

            Sql = String.Format("Select distinct id_categoria, descripcion_cat, catVisible As Visible  From vw_roles_approvals where id_programa = {0} and id_usuario = {1}  and visible = 'SI'", id_programa, id_User)

            get_ta_categoriaBy_User = cl_utl.setObjeto("ta_categoria", "id_categoria", 0, Sql)

            If (get_ta_categoriaBy_User.Rows.Count = 1 And get_ta_categoriaBy_User.Rows.Item(0).Item("id_categoria") = 0) Then
                get_ta_categoriaBy_User.Rows.Remove(get_ta_categoriaBy_User.Rows.Item(0))
            End If


        End Function



        Public Function get_ta_approvalBy_User(ByVal id_User As Integer, Optional ByVal idCat As Integer = 0) As DataTable

            Dim OptStr As String = ""

            If idCat > 0 Then
                OptStr = String.Format(" and id_categoria = {0} ", idCat)
            End If


            Sql = String.Format("select id_tipoDocumento, descripcion_aprobacion from vw_roles_approvals where id_programa = {0} and id_usuario = {1}  and visible = 'SI'  {2} ", id_programa, id_User, OptStr)

            get_ta_approvalBy_User = cl_utl.setObjeto("ta_categoria", "id_categoria", 0, Sql)

            If (get_ta_approvalBy_User.Rows.Count = 1 And get_ta_approvalBy_User.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_ta_approvalBy_User.Rows.Remove(get_ta_approvalBy_User.Rows.Item(0))
            End If


        End Function


        Public Function get_ta_estadoDocumento() As DataTable

            Sql = String.Format(" select * from ta_estadoDocumento where visible = 'SI'", id_programa)

            get_ta_estadoDocumento = cl_utl.setObjeto("ta_estadoDocumento", "id_estadoDoc", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_estadoDocumento.Rows.Count = 1 And get_ta_estadoDocumento.Rows.Item(0).Item("id_estadoDoc") = 0) Then
                get_ta_estadoDocumento.Rows.Remove(get_ta_estadoDocumento.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_ta_ApprovalBy_Category(ByVal id_cat As Integer) As DataTable

            Sql = String.Format(" Select DISTINCT id_tipoDocumento, descripcion_aprobacion " &
                                "    from vw_roles_approvals            " &
                                "     where id_programa = {0}           " &
                                "           And id_categoria = {1}      " &
                                "           And catVisible = 'SI'       ", id_programa, id_cat)

            get_ta_ApprovalBy_Category = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_ta_ApprovalBy_Category.Rows.Count = 1 And get_ta_ApprovalBy_Category.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_ta_ApprovalBy_Category.Rows.Remove(get_ta_ApprovalBy_Category.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_documento(ByVal id_doc As Integer) As DataTable

            get_ta_documento = cl_utl.setObjeto("ta_documento", "id_documento", id_doc)

            If (get_ta_documento.Rows.Count = 1 And get_ta_documento.Rows.Item(0).Item("id_documento") = 0) Then
                get_ta_documento.Rows.Remove(get_ta_documento.Rows.Item(0))
            End If

        End Function


        Public Function get_Build_documentsBy_Type_Query(ByVal idProcc As Integer,
                                                         ByVal id_cat As Integer,
                                                         ByVal strCat As String,
                                                         ByVal id_app As Integer,
                                                         ByVal strApp As String,
                                                         ByVal id_status As Integer,
                                                         ByVal strStatus As String,
                                                         ByVal strKey As String,
                                                         ByRef strQuery As String,
                                                         ByVal boolDate As Integer,
                                                         ByVal id_User As Integer,
                                                         ByVal lbl_ALL_SIMPLE_RolID As String,
                                                         ByVal lbl_ALL_RolID As String,
                                                         ByVal AllAPP As Integer,
                                                         ByVal startDate As DateTime,
                                                         ByVal endDate As DateTime) As DataTable

            ' Dim sql As String = String.Format("SELECT ROW_NUMBER() OVER (ORDER BY Fecha_Recepcion) AS [No], * FROM VW_GR_TA_DOCUMENTOS  WHERE id_programa={0} ", id_programa)

            Dim strSearch As String = ""

            If id_cat > 0 Then

                'sql &= String.Format(" AND ( id_categoria = {0} )", id_cat)
                strSearch &= String.Format(" Category = ""{0}"" {1}", Trim(strCat), "<br />")

            End If


            If id_app > 0 Then

                'sql &= String.Format(" AND ( id_tipoDocumento = {0} )", id_app)
                strSearch &= String.Format(" Approval = ""{0}"" {1}", Trim(strApp), "<br />")

            End If


            If id_status > 0 Then

                'sql &= String.Format(" AND ( id_estadoDoc = {0} )", id_status)
                strSearch &= String.Format(" Status = ""{0}"" {1}", Trim(strStatus), "<br />")

            End If

            If Not String.IsNullOrEmpty(Trim(strKey)) Then

                'sql &= String.Format(" AND ((descripcion_doc LIKE '%{0}%') OR (nom_beneficiario LIKE '%{0}%') OR (numero_instrumento LIKE '%{0}%')) ", Trim(strKey))

                If idProcc = 0 Then
                    strSearch &= String.Format(" Contains the words = ""{0}{2}"" {1}", Strings.Left(Trim(strKey), 45), "<br />", "...")
                End If

            End If


            If Not String.IsNullOrEmpty(Trim(strKey)) Then

                If idProcc = 0 Then
                    strSearch &= String.Format(" Contains the words = ""{0}{2}"" {1}", Strings.Left(Trim(strKey), 45), "<br />", "...")
                End If

            End If


            If idProcc > 0 Then

                ' sql &= String.Format(" AND ( id_documento = {0} )", idProcc)
                strSearch &= String.Format(" Proccess = ""{0}{2}"" {1}", Strings.Left(Trim(strKey), 45), "<br />", "...")

            End If


            'Dim datefilter As String = ""
            If boolDate = 1 Then

                ' datefilter = " AND ( fecha_recepcion BETWEEN CONVERT(DATETIME, '" & startDate & "') AND CONVERT(DATETIME, '" & endDate & "') ) "
                strSearch &= String.Format(" Dates between ""{0:d}"" and ""{1:d}"" {2}", startDate, endDate, "<br />")

            End If


            'sql &= String.Format(" AND ( 

            '                  ( --Pending Status 

            '                    (  {0} = IdOriginador And id_estadoDoc = 1 ) 

            '                           Or (  
            '                           ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
            '                            )


            '                            Or (  --StandBy Status
            '                              ( id_estadoDoc = 6 ) And (  

            '                        (select count(*) as N
            '                         from 
            '                          (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
            '                         inner join (select part from dbo.SDF_SplitString('{1}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
            '                        )
            '                       )	--Or		


            '                             Or (   --Pending Status
            '                               ( id_estadoDoc = 1 ) And (  

            '                        (select count(*) as N
            '                         from 
            '                          (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
            '                         inner join (select part from dbo.SDF_SplitString('{1}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
            '                        )

            '                       )  --Or 

            '                      )  --Pending Status                                    


            '                      --Closed Status
            '                  OR ( id_estadoDoc not in (1,6)  --Chages
            '                      And (  	                                      
            '                         (  (1) in (select * from dbo.SDF_SplitString(IdUserPArticipate,','))  )                                      
            '                         Or  (  				
            '                          (select count(*) as N
            '                           from 
            '                            (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
            '                           inner join (select part from dbo.SDF_SplitString('{2}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
            '                          )
            '                     )  --And
            '                   ) --OR


            '                   --OR
            '                   --New condition to showing all register approval
            '                       OR (
            '                          -----New condition to showing all register approval
            '        ---Bnd All Approvals
            '        {3} = 1

            '          )  


            '                    ) --AND  ", id_User, lbl_ALL_SIMPLE_RolID, lbl_ALL_RolID, AllAPP)


            'sql &= datefilter
            'sql &= " ORDER BY Fecha_Recepcion "

            'get_Build_documentsBy_Type_Query = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS ", "id_documento", 0, sql)

            strQuery = strSearch

            'If (get_Build_documentsBy_Type_Query.Rows.Count = 1 And get_Build_documentsBy_Type_Query.Rows.Item(0).Item("id_documento") = 0) Then
            '    get_Build_documentsBy_Type_Query.Rows.Remove(get_Build_documentsBy_Type_Query.Rows.Item(0))
            'End If



            Dim result As Object

            Using dbEntities As New dbRMS_JIEntities

                Try

                    result = dbEntities.SP_TA_DOCUMENTOS_SEARCH(id_programa, id_User, lbl_ALL_SIMPLE_RolID.Trim, lbl_ALL_RolID.Trim, strKey.Trim, AllAPP, startDate, endDate, boolDate, id_cat, id_app, id_status, idProcc).ToList()

                Catch ex As Exception
                    result = Nothing
                End Try


            End Using


            get_Build_documentsBy_Type_Query = cl_utl.ConvertToDataTable(result)



        End Function

        Public Function get_Build_EnvironmentalDocs_Query(ByVal idProcc As Integer,
                                                         ByVal id_TP As Integer,
                                                         ByVal strReview As String,
                                                         ByVal strKey As String,
                                                         ByRef strQuery As String,
                                                         ByVal boolDate As Boolean,
                                                         Optional ByVal startDate As String = "",
                                                         Optional ByVal endDate As String = "") As DataTable

            Dim sql As String = String.Format("select  ROW_NUMBER() OVER (ORDER BY Fecha_Aprobado) AS [No], * from vw_t_documento_ambiental WHERE id_programa={0} ", id_programa)

            Dim strSearch As String = ""

            If id_TP > -1 Then

                sql &= String.Format(" AND ( id_TipoApp_Environmental = {0} )", id_TP)
                strSearch &= String.Format(" Review Type = ""{0}"" {1}", Trim(strReview), "<br />")

            End If


            If Not String.IsNullOrEmpty(Trim(strKey)) Then

                sql &= String.Format(" AND ( (observacion like '%{0}%') OR (usuario_creo like '%{0}%') OR (numero_instrumento like '%{0}%') OR (nom_beneficiario like '%{0}%'))", Trim(strKey))

                If idProcc = 0 Then
                    strSearch &= String.Format(" Contains the words = ""{0}{2}"" {1}", Strings.Left(Trim(strKey), 45), "<br />", "...")
                End If

            End If


            If idProcc > 0 Then

                sql &= String.Format(" AND ( id_documento_ambiental = {0} )", idProcc)
                strSearch &= String.Format(" Proccess = ""{0}{2}"" {1}", Strings.Left(Trim(strKey), 45), "<br />", "...")

            End If

            Dim datefilter As String = ""
            If boolDate = True Then

                datefilter = " AND ( fecha_aprobado  BETWEEN CONVERT(DATETIME, '" & startDate & "') AND CONVERT(DATETIME, '" & endDate & "') ) "
                strSearch &= String.Format(" Dates between ""{0:d}"" and ""{1:d}"" {2}", startDate, endDate, "<br />")

            End If


            'If Not (Me.Session("E_IdPerfil").ToString = "1" Or Me.Session("E_IdPerfil").ToString = "2") Then

            '    sql &= String.Format("  AND ( " &
            '                            "      ( {0} in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')))) ", Me.Session("E_IdUser"))

            '    strSearch &= String.Format(" {0} has been participated {1}", varUSer, "<br />")

            'End If

            sql &= datefilter
            sql &= " ORDER BY Fecha_Aprobado "


            get_Build_EnvironmentalDocs_Query = cl_utl.setObjeto("vw_t_documento_ambiental", "id_documento_ambiental", 0, sql)

            strQuery = strSearch

            If (get_Build_EnvironmentalDocs_Query.Rows.Count = 1 And get_Build_EnvironmentalDocs_Query.Rows.Item(0).Item("id_documento_ambiental") = 0) Then
                get_Build_EnvironmentalDocs_Query.Rows.Remove(get_Build_EnvironmentalDocs_Query.Rows.Item(0))
            End If



        End Function






    End Class


End Namespace
