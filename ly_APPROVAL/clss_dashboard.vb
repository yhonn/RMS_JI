Imports ly_SIME
Imports System.Data.SqlClient

Namespace APPROVAL

    Public Class clss_dashboard

        Public cl_utl As New CORE.cls_util
        Dim Sql As String

        Public ReadOnly Property id_programa As Integer



        Public Sub New(ByVal idP As Integer)

            id_programa = idP

        End Sub

        Public Function get_Approvals_Count(ByVal idUser As Integer) As Integer

            'Sql = String.Format("select count(*) as N from ta_documento where id_programa = {0}", id_programa)
            Sql = String.Format("select count(*) as N from ta_documento a
                                      inner join (select distinct id_documento, id_usuario
			                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) b on (a.id_documento = b.id_documento )
                                    where a.id_programa = {0}
                                     and b.id_usuario = {1}", id_programa, idUser)

            get_Approvals_Count = cl_utl.setObjeto("ta_documento", "id_programa", id_programa, Sql).Rows.Item(0).Item("N")

        End Function


        Public Function get_Pending_Approvals_Count(ByVal id_user As Integer) As Integer


            'Sql = String.Format("select count(*) as N from ta_documento b
            '                       inner join ta_AppDocumento a on (b.id_documento = a.id_documento)
            '                      inner join 
            '                      (select id_documento, max(id_App_Documento) as id_App_Dcoumento from ta_AppDocumento
            '                       group by id_documento) as Tab on Tab.id_App_Dcoumento = a.id_App_Documento 
            '                        where a.id_estadoDoc not in (3,4,7) and b.id_programa = {0} ", id_programa)

            Sql = String.Format("select count(b.id_documento) as N 
                                        from ta_documento b
                                        inner join ta_AppDocumento a on (b.id_documento = a.id_documento)
                                        inner join 
                                            ( select id_documento, max(id_App_Documento) as id_App_Dcoumento 
                                                 from ta_AppDocumento
	                                             group by id_documento) as Tab on ( Tab.id_App_Dcoumento = a.id_App_Documento ) 
                                        inner join ta_rutaTipoDoc tp_doc ON (b.id_tipoDocumento = tp_Doc.id_tipoDocumento and a.id_ruta = tp_doc.id_ruta)
                                        inner join VW_GR_USER_PARTICIPATES c on ( a.id_documento = c.id_documento and c.id_rol = tp_doc.id_rol)
                                        where ( a.id_estadoDoc not in (3,4,7,5) and b.id_programa = {0} )
                                        and c.id_usuario = {1}", id_programa, id_user)

            get_Pending_Approvals_Count = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_programa", id_programa, Sql).Rows.Item(0).Item("N")

        End Function


        Public Function get_average_timing(ByVal id_user As Integer) As Double

            'Sql = String.Format("select id_programa, sum(tab.minu) as minu
            '                         from
            '                         (	select id_programa, sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)  as Minu
            '                            from vw_ta_AppDocumento 
            '                             where (id_estadoDoc not in (1,5) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) > 0 )
            '                            group by id_programa
            '                         --All diferent to 1 and 5
            '                          union	
            '                           select id_programa, sum((DATEDIFF(MI,fecha_recepcion, getdate()))) / count(*) as Minu
            '                            from vw_ta_AppDocumento 
            '                          where (id_estadoDoc in (1,6) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0)
            '                            group by id_programa) as tab
            '                             --Pending  
            '                         where tab.id_programa = {0}
            '                         group by id_programa", id_programa)

            Sql = String.Format("select id_programa, sum(tab.minu) as minu
                                  from
                                (	select b.id_programa, sum((DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion))) / count(*)  as Minu
	                                        from ta_AppDocumento  a 
		                                   inner join ta_documento b on (a.id_documento = b.id_documento)
		                                  inner join (select distinct id_documento, id_usuario
			                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} )  c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
		                                 where (a.id_estadoDoc not in (1,5) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) > 0 )
		                              and (c.id_usuario = {1})
                                     group by b.id_programa
	                                 --All diferent to 1 and 5
                                 union	
	                               select b.id_programa, sum((DATEDIFF(MI,a.fecha_recepcion, getdate()))) / count(*) as Minu
	                                   from ta_AppDocumento  a 
		                                   inner join ta_documento b on (a.id_documento = b.id_documento)
	                              inner join (select distinct id_documento, id_usuario
			                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
	                                                          where (a.id_estadoDoc in (1,6) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) <= 0)
		                             and (c.id_usuario = {1})
	                               group by b.id_programa ) as tab
                                 --Pending  
                                 where tab.id_programa = {0}
                                 group by id_programa", id_programa, id_user)


            get_average_timing = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql).Rows.Item(0).Item("minu")


        End Function

        Public Function get_average_cat_timing() As DataTable

            Sql = String.Format("select id_programa, id_categoria, descripcion_cat, mesV, mes, sum(tab.minu) as minu, sum(tab.Hr) as Hr
                                     from
                                     (	  select id_programa, id_categoria, descripcion_cat, DATEPART(MM ,fecha_recepcion) as mesV, left(DATENAME( MONTH ,fecha_recepcion),3) as mes, sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)  as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)) / 60 as Hr
	                                       from vw_ta_AppDocumento 
	                                        where (id_estadoDoc not in (1,5) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) > 0 )
                                         group by id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion), left(DATENAME( MONTH ,fecha_recepcion),3) 
	                                    --All diferent to 1 and 5
                                      union	
	                                      select id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion) as mesV, left(DATENAME( MONTH ,fecha_recepcion),3) as mes, sum((DATEDIFF(MI,fecha_recepcion, getdate()))) / count(*) as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)) / 60 as Hr
	                                       from vw_ta_AppDocumento 
		                                    where (id_estadoDoc in (1,6) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0)
	                                       group by id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion), left(DATENAME( MONTH ,fecha_recepcion),3)) as tab
                                         --Pending  
                                     where tab.id_programa = {0}
                                     group by id_programa, id_categoria, descripcion_cat, mesV, mes ", id_programa)


            get_average_cat_timing = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_average_cat_timing.Rows.Count = 1 And get_average_cat_timing.Rows.Item(0).Item("id_programa") = 0) Then
                get_average_cat_timing.Rows.Remove(get_average_cat_timing.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

        Public Function get_average_cat_timing_user(ByVal id_user As Integer) As DataTable


            Sql = String.Format("select tab4.id_programa, tab4.id_categoria, tab4.id_usuario, tab4.descripcion_cat, tab4.anio, tab4.mesV, tab4.mes, sum(tab4.minu) as minu, sum(tab4.Hr) as Hr
                                  from
                                (select tab2.id_programa, tab2.id_categoria, tab2.id_usuario, tab2.descripcion_cat, tab2.anio, tab2.mesV, tab2.mes, tab2.minu, tab2.Hr
                                 from
                                (select id_programa, id_categoria, id_usuario, descripcion_cat, anio, mesV, mes, sum(tab.minu) as minu, sum(tab.Hr) as Hr
                                     from
                                  ( 
		                                select a.id_programa, a.id_categoria, b.id_usuario, a.descripcion_cat, DATEPART(YY,a.fecha_recepcion) as anio, DATEPART(MM ,a.fecha_recepcion) as mesV, left(DATENAME( MONTH ,a.fecha_recepcion),3) as mes, sum((DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion))) / count(*)  as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion))) / count(*)) / 60 as Hr
		                                 from vw_ta_AppDocumento  a
		                                  inner join (select distinct id_documento, id_usuario
			                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) b on (a.id_documento = b.id_documento and a.id_usuario_app = b.id_usuario)
		                                    inner join FN_Ta_getting_approval_timing_average_interval({0}) interval on ( interval.id_programa = a.id_programa and interval.anio = DATEPART(YY,a.fecha_recepcion) and interval.mesV = DATEPART(MM ,a.fecha_recepcion))
		                                   where (a.id_estadoDoc not in (1,5) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) > 0 )
		                                 group by a.id_programa, a.id_categoria, b.id_usuario, a.descripcion_cat, DATEPART(YY,a.fecha_recepcion), DATEPART( MM ,a.fecha_recepcion), left(DATENAME( MONTH ,a.fecha_recepcion),3) 
		                                 --All diferent to 1 and 5
                                     union all	
		                                select a.id_programa, a.id_categoria, b.id_usuario, a.descripcion_cat, DATEPART(YY,a.fecha_recepcion), DATEPART( MM ,a.fecha_recepcion) as mesV, left(DATENAME( MONTH ,a.fecha_recepcion),3) as mes, sum((DATEDIFF(MI,a.fecha_recepcion, getdate()))) / count(*) as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,a.fecha_recepcion, getdate()))) / count(*)) / 60 as Hr
		                                  from vw_ta_AppDocumento a
			                                inner join (select distinct id_documento, id_usuario
			                                                from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) b on (a.id_documento = b.id_documento and a.id_usuario_app = b.id_usuario)
			                                  inner join FN_Ta_getting_approval_timing_average_interval({0}) interval on ( interval.id_programa = a.id_programa and interval.anio = DATEPART(YY,a.fecha_recepcion) and interval.mesV = DATEPART(MM ,a.fecha_recepcion))
		                                where (a.id_estadoDoc in (1,6) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) <= 0)
		                                 group by a.id_programa, a.id_categoria, b.id_usuario, a.descripcion_cat,DATEPART(YY,a.fecha_recepcion), DATEPART( MM ,a.fecha_recepcion), left(DATENAME( MONTH ,a.fecha_recepcion),3)
	                                 --Pending  
		                                 ) as tab
                                  where tab.id_programa = {0} and tab.id_usuario = {1}
                                group by id_programa, id_categoria, id_usuario, descripcion_cat, anio, mesV, mes) as tab2

                                UNION ALL

                                select tab3.id_programa, tab3.id_categoria, tab3.id_usuario, tab3.descripcion_cat, tab3.anio, tab3.mesV, tab3.mes, tab3.minu, tab3.Hr 
                                 from
	                                (select a.id_programa, cat.id_categoria, up.id_usuario, cat.descripcion_cat, a.anio, a.mesV, a.mes, 0 as minu, 0 as Hr 
		                                  from FN_Ta_getting_approval_timing_average_interval({0}) a 
		                                   inner join ta_categoria cat on (a.id_programa = cat.id_programa)
			                                inner join t_usuario_programa up on (up.id_programa = cat.id_programa)
			                                where a.id_programa = {0} and up.id_usuario = {1}) as tab3 ) as tab4

                                group by tab4.id_programa, tab4.id_categoria, tab4.id_usuario, tab4.descripcion_cat, tab4.anio, tab4.mesV, tab4.mes", id_programa, id_user)


            get_average_cat_timing_user = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_average_cat_timing_user.Rows.Count = 1 And get_average_cat_timing_user.Rows.Item(0).Item("id_programa") = 0) Then
                get_average_cat_timing_user.Rows.Remove(get_average_cat_timing_user.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function




        Public Function get_average_cat_timing_categorie_Serie() As DataTable

            Sql = String.Format("select distinct id_programa, descripcion_cat
                                     from
                                     (	  select id_programa, id_categoria, descripcion_cat, DATEPART(MM ,fecha_recepcion) as mesV, left(DATENAME( MONTH ,fecha_recepcion),3) as mes, sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)  as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)) / 60 as Hr
	                                       from vw_ta_AppDocumento 
	                                        where (id_estadoDoc not in (1,5) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) > 0 )
                                         group by id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion), left(DATENAME( MONTH ,fecha_recepcion),3) 
	                                    --All diferent to 1 and 5
                                      union	
	                                      select id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion) as mesV, left(DATENAME( MONTH ,fecha_recepcion),3) as mes, sum((DATEDIFF(MI,fecha_recepcion, getdate()))) / count(*) as Minu, convert(numeric(15,2), sum((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) / count(*)) / 60 as Hr
	                                       from vw_ta_AppDocumento 
		                                    where (id_estadoDoc in (1,6) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0)
	                                       group by id_programa, id_categoria, descripcion_cat, DATEPART( MM ,fecha_recepcion), left(DATENAME( MONTH ,fecha_recepcion),3)) as tab
                                         --Pending  
                                   where tab.id_programa = {0} ", id_programa)


            get_average_cat_timing_categorie_Serie = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_average_cat_timing_categorie_Serie.Rows.Count = 1 And get_average_cat_timing_categorie_Serie.Rows.Item(0).Item("id_programa") = 0) Then
                get_average_cat_timing_categorie_Serie.Rows.Remove(get_average_cat_timing_categorie_Serie.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function



        Public Function get_average_cat_timing_categorie_AxisX() As DataTable

            Sql = String.Format("select * from FN_Ta_getting_approval_timing_average_interval({0}) order by orden", id_programa)


            get_average_cat_timing_categorie_AxisX = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql)


            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_average_cat_timing_categorie_AxisX.Rows.Count = 1 And get_average_cat_timing_categorie_AxisX.Rows.Item(0).Item("id_programa") = 0) Then
                get_average_cat_timing_categorie_AxisX.Rows.Remove(get_average_cat_timing_categorie_AxisX.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function


        Public Function get_MAX_timing(ByVal id_user As Integer) As Double

            'Sql = String.Format("select tab.id_programa, MAX(tab.Mminu) as Mminu
            '                         from
            '                         (  select id_programa, MAX((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) as Mminu
            '                          from vw_ta_AppDocumento 
            '                           where (id_estadoDoc not in (1,5) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) > 0 )
            '                         group by id_programa
            '                          union
            '                         select id_programa, MAX((DATEDIFF(MI,fecha_recepcion, getdate()))) as Mminu
            '                             from vw_ta_AppDocumento 
            '                            where (id_estadoDoc in (1,6) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0)
            '                          group by id_programa ) as tab 
            '                            where tab.id_programa = {0}
            '                          group by tab.id_programa", id_programa)


            Sql = String.Format("select tab.id_programa, MAX(tab.Mminu) as Mminu
                                             from
                                                (  select b.id_programa, MAX((DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion))) as Mminu
	                                                   from ta_AppDocumento  a 
															inner join ta_documento b on (a.id_documento = b.id_documento)
			                                          inner join (select distinct id_documento, id_usuario
			                                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
													 where (a.id_estadoDoc not in (1,5) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) > 0 )
			                                          and (c.id_usuario = {1})
													 group by b.id_programa
                                                   union
	                                                select b.id_programa, MAX((DATEDIFF(MI,a.fecha_recepcion, getdate()))) as Mminu
		                                                   from ta_AppDocumento  a 
															inner join ta_documento b on (a.id_documento = b.id_documento)
				                                         inner join (select distinct id_documento, id_usuario
			                                                              from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
													 where (a.id_estadoDoc in (1,6) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) <= 0)
			                                          and (c.id_usuario = {1})
													  group by b.id_programa ) as tab 
                                              where tab.id_programa = {0}
                                           group by tab.id_programa", id_programa, id_user)


            get_MAX_timing = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql).Rows.Item(0).Item("Mminu")

        End Function


        Public Function get_MIN_timing(ByVal id_user As Integer) As Double

            'Sql = String.Format("select tab.id_programa, MIN(tab.Mminu) as Mminu
            '                         from
            '                         (  select id_programa, MIN((DATEDIFF(MI,fecha_recepcion, fecha_aprobacion))) as Mminu
            '                          from vw_ta_AppDocumento 
            '                            where (id_estadoDoc not in (1,5) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) > 0 )	                                      
            '                         group by id_programa
            '                          union
            '                         select id_programa, MIN((DATEDIFF(MI,fecha_recepcion, getdate()))) as Mminu
            '                             from vw_ta_AppDocumento 
            '                            where (id_estadoDoc in (1,6) and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0)		                                 
            '                          group by id_programa ) as tab
            '                           where tab.id_programa = {0} 
            '                        group by tab.id_programa	", id_programa)

            Sql = String.Format("select tab.id_programa, MIN(tab.Mminu) as Mminu
                                            from
                                         (select b.id_programa, MIN((DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion))) as Mminu
	                                               from ta_AppDocumento  a 
												    inner join ta_documento b on (a.id_documento = b.id_documento)
		                                      inner join (select distinct id_documento, id_usuario
			                                                  from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
										      where (a.id_estadoDoc not in (1,5) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) > 0 )
		                                      and (c.id_usuario = {1})
										     group by b.id_programa
                                           union
	                                      select b.id_programa, MIN((DATEDIFF(MI,a.fecha_recepcion, getdate()))) as Mminu
		                                       from ta_AppDocumento  a 
										         inner join ta_documento b on (a.id_documento = b.id_documento)
		                                    inner join (select distinct id_documento, id_usuario
			                                            from VW_GR_USER_PARTICIPATES where id_usuario = {1} ) c on (a.id_documento = c.id_documento and a.id_usuario_app = c.id_usuario)
									      where (a.id_estadoDoc in (1,6) and DATEDIFF(MI,a.fecha_recepcion, a.fecha_aprobacion) <= 0)		                                 
	                                      and (c.id_usuario = {1})
								           group by b.id_programa ) as tab
                                          where tab.id_programa = {0} 
                                          group by tab.id_programa", id_programa, id_user)

            get_MIN_timing = cl_utl.setObjeto("vw_ta_AppDocumento", "id_programa", id_programa, Sql).Rows.Item(0).Item("Mminu")


        End Function


        Public Function get_Pending_APP(ByVal idUsr As Integer, ByVal lbl_ALL_SIMPLE_RolID As String, ByVal lbl_ALL_RolID As String) As DataTable

            Dim result As Object

            Using dbEntities As New dbRMS_JIEntities

                Try

                    'result = dbEntities.SP_TA_DOCUMENTOS_GR(id_programa, 1, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()
                    result = dbEntities.SP_DOCUMENTOS_SEARCH_GR(id_programa, 1, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()

                    'Sql = String.Format("select id_documento, descripcion_cat as categorie, 
                    '                         numero_instrumento, 
                    '                   codigo_Approval, 
                    '                   descripcion_doc, 
                    '                   ltrim(rtrim(numero_instrumento)) + ' - ' + ltrim(rtrim(descripcion_doc)) as approval,
                    '                   DATEDIFF(MI,fecha_recepcion, getdate()) as minu, 
                    '                   dbo.Get_V(DATEDIFF(MI,fecha_recepcion, getdate())) as value, 
                    '                   dbo.Get_UNIT(DATEDIFF(MI,fecha_recepcion, getdate()),1) as unit, 
                    '                   dbo.Get_UNIT(DATEDIFF(MI,fecha_recepcion, getdate()),2) as ico
                    '                   from  VW_GR_TA_DOCUMENTOS_VER1   
                    '                    where id_programa = {0} ", id_programa)

                    'Sql &= String.Format("  AND ( " &
                    '      "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({2}))) AND id_estadoDoc = 6 ) " &
                    '      "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
                    '      "      OR ( rol_owner in ({2}) AND id_estadoDoc = 1)  " &
                    '      "      OR ( rol_owner in ({3}) AND id_estadoDoc = 1 AND idUserOwner = '0')  )  ", idUsr, "lbl_GroupRolID.Text", lbl_ALL_SIMPLE_RolID, lbl_ALL_RolID)



                    'Sql &= " Order by categorie, numero_instrumento "

                    'get_Pending_APP = cl_utl.setObjeto_v2("VW_GR_TA_DOCUMENTOS_VER1", "id_programa", id_programa, Sql, , 180)

                    ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
                    'If (get_Pending_APP.Rows.Count = 1 And get_Pending_APP.Rows.Item(0).Item("id_documento") = 0) Then
                    '    get_Pending_APP.Rows.Remove(get_Pending_APP.Rows.Item(0))
                    'End If
                    ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

                Catch ex As Exception
                    get_Pending_APP = Nothing
                End Try

            End Using

            'If result.count > 0 Then

            If IsNothing(result) Then
                get_Pending_APP = Nothing
            Else
                get_Pending_APP = cl_utl.ConvertToDataTable(result)
            End If


            'Else
            '    get_Pending_APP = Nothing
            'End If


        End Function


        Public Function get_Pending_APP_task(ByVal idUsr As Integer, ByVal lbl_ALL_SIMPLE_RolID As String, ByVal lbl_ALL_RolID As String) As DataTable

            'Sql = String.Format("select *, DATEDIFF(DAY,datecreated, getdate()) as vdays, dbo.FN_TA_PROGRESS_APP(id_tipoDocumento,id_ruta) as progress
            '                       from  VW_GR_TA_DOCUMENTOS_VER1 
            '                        where (id_estadoDoc in (1) or (id_estadoDoc = 6 and DATEDIFF(MI,fecha_recepcion, fecha_aprobacion) = 0))
            '                      and   id_programa = {0} ", id_programa)

            'Sql &= String.Format("  AND ( " &
            '      "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({2}))) AND id_estadoDoc = 6 ) " &
            '      "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
            '      "      OR ( rol_owner in ({2}) AND id_estadoDoc = 1)  " &
            '      "      OR ( rol_owner in ({3}) AND id_estadoDoc = 1 AND idUserOwner = '0')  )  ", idUsr, "lbl_GroupRolID.Text", lbl_ALL_SIMPLE_RolID, lbl_ALL_RolID)


            'Sql &= " order by id_documento "

            'get_Pending_APP_task = cl_utl.setObjeto_v2("VW_GR_TA_DOCUMENTOS_VER1", "id_programa", id_programa, Sql, , 180)

            ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            'If (get_Pending_APP_task.Rows.Count = 1 And get_Pending_APP_task.Rows.Item(0).Item("id_documento") = 0) Then
            '    get_Pending_APP_task.Rows.Remove(get_Pending_APP_task.Rows.Item(0))
            'End If
            ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Dim result As Object

            Using dbEntities As New dbRMS_JIEntities

                Try

                    'result = dbEntities.SP_TA_DOCUMENTOS_GR(id_programa, 2, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()
                    result = dbEntities.SP_DOCUMENTOS_SEARCH_GR(id_programa, 1, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()

                Catch ex As Exception
                    get_Pending_APP_task = Nothing
                End Try

            End Using

            If Not IsNothing(result) Then
                get_Pending_APP_task = cl_utl.ConvertToDataTable(result)
            Else
                get_Pending_APP_task = Nothing
            End If


        End Function


        Public Function get_Approvals(ByVal vTop As Integer, ByVal idUsr As Integer, ByVal lbl_ALL_SIMPLE_RolID As String, ByVal lbl_ALL_RolID As String) As DataTable


            'Sql = String.Format("select  top {1} id_documento,
            '                             id_ruta,  
            '                             descripcion_cat as categorie, 
            '                       descripcion_aprobacion,         
            '                             numero_instrumento, 
            '                       codigo_Approval, 
            '                       descripcion_doc, 
            '                       ltrim(rtrim(numero_instrumento)) + ' - ' + ltrim(rtrim(descripcion_doc)) as approval,
            '                       descripcion_estado,
            '                       fecha_recepcion,
            '                       DATEDIFF(MI,fecha_recepcion, getdate()) as minu, 
            '                       dbo.Get_V(DATEDIFF(MI,fecha_recepcion, getdate())) as value, 
            '                       dbo.Get_UNIT(DATEDIFF(MI,fecha_recepcion, getdate()),1) as unit, 
            '                       dbo.Get_UNIT(DATEDIFF(MI,fecha_recepcion, getdate()),2) as ico
            '                   from VW_GR_TA_DOCUMENTOS_VER1     
            '                 where id_programa = {0} ", id_programa, vTop)

            'Sql &= String.Format("  AND ( " &
            '      "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({2}))) AND id_estadoDoc = 6 ) " &
            '      "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
            '      "      OR ( rol_owner in ({2}) AND id_estadoDoc = 1)  " &
            '      "      OR ( rol_owner in ({3}) AND id_estadoDoc = 1 AND idUserOwner = '0')  )  ", idUsr, "lbl_GroupRolID.Text", lbl_ALL_SIMPLE_RolID, lbl_ALL_RolID)


            'Sql &= " order by fecha_recepcion desc "

            'get_Approvals = cl_utl.setObjeto_v2("VW_GR_TA_DOCUMENTOS_VER1", "id_programa", id_programa, Sql, , 80)

            ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            'If (get_Approvals.Rows.Count = 1 And get_Approvals.Rows.Item(0).Item("id_documento") = 0) Then
            '    get_Approvals.Rows.Remove(get_Approvals.Rows.Item(0))
            'End If
            ''****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


            Dim result As Object

            Using dbEntities As New dbRMS_JIEntities

                Try

                    'result = dbEntities.SP_TA_DOCUMENTOS_GR(id_programa, 3, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()
                    result = dbEntities.SP_DOCUMENTOS_SEARCH_GR(id_programa, 0, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).Take(vTop).ToList()



                Catch ex As Exception
                    get_Approvals = Nothing
                End Try

            End Using

            If Not IsNothing(result) Then
                get_Approvals = cl_utl.ConvertToDataTable(result)
            Else
                get_Approvals = Nothing
            End If


        End Function

        Public Function get_Approvals_Total(ByVal id_user As Integer) As DataTable

            ''Sql = String.Format("select * from vw_ta_graph_app where id_programa = {0}", id_programa)
            Sql = String.Format("select * from vw_ta_graph_app_user where id_programa = {0} and id_usuario = {1}", id_programa, id_user)

            get_Approvals_Total = cl_utl.setObjeto("vw_ta_graph_app", "id_programa", id_programa, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Approvals_Total.Rows.Count = 1 And get_Approvals_Total.Rows.Item(0).Item("id_programa") = 0) Then
                get_Approvals_Total.Rows.Remove(get_Approvals_Total.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


        End Function

    End Class

End Namespace
