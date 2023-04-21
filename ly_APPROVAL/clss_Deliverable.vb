Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.Configuration
Imports ly_SIME

Namespace APPROVAL

    Public Class clss_Deliverable



        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Const cAction_ByProcess = 1
        Const cAction_ByMessage = 2
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Public cl_user As ly_SIME.CORE.cls_user


        Public Sub New(ByVal idP As Integer, Optional user_ As ly_SIME.CORE.cls_user = Nothing)
            id_programa = idP
            cl_user = user_
        End Sub


        Public Function get_t_usuarios_Implementer(Optional idUSr As Integer = 0) As DataTable



            Dim strSQL As String = String.Format("select isnull(b.id_ejecutor_usuario,0) as id_ejecutor_usuario,  
		                                                      isnull(b.id_ejecutor,0) as id_ejecutor, 
		                                                      isnull(a.nombre_ejecutor,'----') as nombre_ejecutor, 
		                                                      c.id_usuario, 
		                                                      c.nombre_usuario, 
		                                                      c.email_usuario, 
		                                                      c.id_tipo_usuario, 
		                                                      c.tipo_usuario 
                                                         from  vw_t_usuarios c 
	                                                      left join t_ejecutor_usuario b  on (c.id_usuario = b.id_usuario)
                                                           left join t_ejecutores a on (a.id_ejecutor = b.id_ejecutor)     
                                                       where c.id_usuario = {0} and c.id_programa = {1} ", idUSr, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_t_usuarios", "id_usuario", idUSr, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_usuario") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_t_usuarios_Implementer = tbl_result

        End Function


        'get_Activities_Implementer
        Public Function get_Activities_Implementer(Optional idImplementer As Integer = 0) As DataTable

            Dim bndImplementer As Integer = If(idImplementer = 0, 1, 0)

            Dim strSQL As String = String.Format(" select  id_ficha_proyecto,
                                                           id_ejecutor,   
                                                           codigo_SAPME, 
                                                           nombre_proyecto, 
                                                           fecha_inicio_proyecto, 
                                                           fecha_fin_proyecto, 
                                                           nombre_estado_ficha,
                                                           nombre_ejecutor
	                                                         from vw_tme_ficha_proyecto
	                                                         where  ( {2} in (select part from dbo.SDF_SplitString(id_programa,',')) )
                                                               and     ( id_ejecutor = {0} or 1= {1} )
                                                       order by id_ficha_proyecto", idImplementer, bndImplementer, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_proyecto", "id_ejecutor", idImplementer, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_proyecto") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Activities_Implementer = tbl_result

        End Function


        Public Function get_Last_Deliverable(ByVal idFichaProyecto As Integer, Optional bType As Integer = 0, Optional idDeliverable As Integer = 0) As DataTable

            Dim strSQL As String = ""
            Dim tbl_result As DataTable
            Dim idDeliv As Integer = 0

            If idDeliverable = 0 Then

                If bType = 1 Then 'Pending

                    strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Pending
	                                        where  ( id_ficha_proyecto ={0} ) and  id_deliverable_estado in (0) 
                                        group by id_Ficha_proyecto", idFichaProyecto)

                    tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                    '****************************************PATCH TO RETURN AN EMPTY TABLE****************************************** No pending
                    If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then

                        strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Running
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (1,2,5) 
                                        group by id_Ficha_proyecto", idFichaProyecto)

                        tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then 'Not Runnig


                            strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --completed
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (3,4,6) 
                                        group by id_Ficha_proyecto   ", idFichaProyecto)

                            tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        End If


                    End If
                    '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

                ElseIf bType = 2 Then ' it´s two 'Running

                    strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Running
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (1,2,5) 
                                        group by id_Ficha_proyecto", idFichaProyecto)


                    tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                    '****************************************PATCH TO RETURN AN EMPTY TABLE****************************************** No running
                    If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then


                        strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Pending
	                                        where  ( id_ficha_proyecto ={0} ) and  id_deliverable_estado in (0) 
                                        group by id_Ficha_proyecto", idFichaProyecto)

                        tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then 'Not Pending

                            strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --completed
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (3,4,6) 
                                        group by id_Ficha_proyecto   ", idFichaProyecto)

                            tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        End If



                    End If

                ElseIf bType = 3 Then ' it´s Completed


                    strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --completed
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (3,4,6) 
                                        group by id_Ficha_proyecto   ", idFichaProyecto)

                    tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                    If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then 'Not Pending


                        strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Running
	                                        where  ( id_ficha_proyecto ={0} ) and id_deliverable_estado in (1,2,5) 
                                        group by id_Ficha_proyecto", idFichaProyecto)


                        tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then 'Not Pending

                            strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --Pending
	                                        where  ( id_ficha_proyecto ={0} ) and  id_deliverable_estado in (0) 
                                        group by id_Ficha_proyecto", idFichaProyecto)

                            tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                        End If


                    End If



                Else  'Nothing

                        strSQL = String.Format("select id_Ficha_proyecto, max(id_deliverable) as id_deliverable from vw_tme_ficha_entregables --At All
	                                        where  ( id_ficha_proyecto ={0} ) 
                                        group by id_Ficha_proyecto   ", idFichaProyecto)


                    tbl_result = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

                    If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then 'Not Pending

                        tbl_result.Rows.Remove(tbl_result.Rows.Item(0))

                    End If


                End If

                idDeliv = tbl_result.Rows.Item(0).Item("id_deliverable")

            Else

                idDeliv = idDeliverable

            End If


            get_Last_Deliverable = get_Deliverables(idDeliv)


        End Function


        Public Function get_Deliverabled_ByActivity(ByVal idFichaProyecto As Integer) As DataTable

            Dim strSQL As String = ""

            strSQL = String.Format("select * from VW_TA_DELIVERABLE_TOT
                                        where id_ficha_proyecto = {0}
                                    	order by monthCORR", idFichaProyecto)

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_DELIVERABLE_TOT", "id_ficha_proyecto", idFichaProyecto, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_proyecto") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverabled_ByActivity = tbl_result

        End Function

        Public Function get_Deliverabled_ByActivity_Serie(ByVal idFichaProyecto As Integer) As DataTable

            Dim strSQL As String = ""

            strSQL = String.Format("select distinct MonthYear from VW_TA_DELIVERABLE_TOT
                                        where id_ficha_proyecto = {0}
                                    order by MonthYear", idFichaProyecto)

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_DELIVERABLE_TOT", "id_ficha_proyecto", idFichaProyecto, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_proyecto") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverabled_ByActivity_Serie = tbl_result

        End Function



        Public Function get_Deliverables_values(ByVal idType As Integer, ByVal strVar As String, ByVal strField As String, ByVal strVar_2 As String, ByVal varMec As Integer, varSubM As Integer) As DataTable

            Dim strSQL As String = ""
            Dim bndOPT As Integer = If(strVar_2 = "-1", 1, 0)
            Dim bndMec As Integer = If(varMec <= 0, 1, 0)
            Dim bndSubM As Integer = If(varSubM <= 0, 1, 0)

            If idType = 1 Then 'FY

                'strSQL = String.Format("select  ROW_NUMBER() OVER(
                '                                     ORDER BY 
                '                                 a.allocated, 
                '                                 a.FY
                '                            ) N,
                '                              a.allocated, 
                '                           a.FY,
                '                           sum(a.valor) as valor,
                '                           sum(a.valorUSD) as valorUSD
                '                          from VW_TA_DELIVERABLE_TOT_DET a
                '                                inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
                '                              where (b.id_programa = {4} )
                '                               and ( a.id_mecanismo_contratacion = {0} or 1 = {1} )
                '                               and ( a.id_sub_mecanismo = {2} or 1 = {3}  )
                '                           group by a.allocated,
                '                                     a.FY", varMec, bndMec, varSubM, bndSubM, id_programa)

                strSQL = String.Format("select  ROW_NUMBER() OVER(
                                                     ORDER BY 
			                                              a.allocated, 
			                                              a.FY_2
				                                        ) N,
                                              a.allocated, 
	                                          a.FY_2,
	                                          sum(a.valor) as valor,
	                                          sum(a.valorUSD) as valorUSD
	                                         from VW_TA_DELIVERABLE_TOT_DET a
                                                inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
                                              where (b.id_programa = {4} )
                                               and ( a.id_mecanismo_contratacion = {0} or 1 = {1} )
                                               and ( a.id_sub_mecanismo = {2} or 1 = {3}  )
                                           group by a.allocated,
                                                     a.FY_2", varMec, bndMec, varSubM, bndSubM, id_programa)


            ElseIf idType = 2 Then 'Month

                'strSQL = String.Format("select  ROW_NUMBER() OVER(
                '              ORDER BY 
                '                  a.allocated,
                '               a.M3) N,
                '             a.allocated, 
                '             a.FY,
                '             a.M + '-' + a.Y as M,
                '             sum(a.valor) as valor,
                '             sum(a.valorUSD) as valorUSD
                '            from VW_TA_DELIVERABLE_TOT_DET a
                '                      inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
                '             where (b.id_programa = {7} )
                '                        and ( a.FY = '{0}' ) and (a.allocated= {1} or 1={2})
                '                        and ( a.id_mecanismo_contratacion = {3} or 1 = {4} )
                '                        and ( a.id_sub_mecanismo = {5} or 1 = {6}  )
                '             group by a.M3,				 
                '             a.allocated,
                '             a.FY,
                '             a.M + '-' + a.Y", strVar, strVar_2, bndOPT, varMec, bndMec, varSubM, bndSubM, id_programa)

                strSQL = String.Format("select  ROW_NUMBER() OVER(
					                         ORDER BY 
					                             a.allocated,
						                         a.M3) N,
			                          a.allocated, 
			                          a.FY_2,
			                          a.M + '-' + a.Y as M,
			                          sum(a.valor) as valor,
			                          sum(a.valorUSD) as valorUSD
			                         from VW_TA_DELIVERABLE_TOT_DET a
                                      inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
			                          where (b.id_programa = {7} )
                                        and ( a.FY_2 = '{0}' ) and (a.allocated= {1} or 1={2})
                                        and ( a.id_mecanismo_contratacion = {3} or 1 = {4} )
                                        and ( a.id_sub_mecanismo = {5} or 1 = {6}  )
		                           group by a.M3,				 
					                        a.allocated,
					                        a.FY_2,
					                        a.M + '-' + a.Y", strVar, strVar_2, bndOPT, varMec, bndMec, varSubM, bndSubM, id_programa)



            ElseIf idType = 3 Then 'Implementer


                strSQL = String.Format("select  ROW_NUMBER() OVER(
					                                     ORDER BY 
					                                        a.allocated,
						                                    a.nombre_ejecutor) N,
			                                      a.allocated, 			  
			                                      a.M + '-' + a.Y as M,
			                                      a.id_ejecutor,
			                                      a.nombre_ejecutor,
			                                      sum(a.valor) as valor,
			                                      sum(a.valorUSD) as valorUSD
			                                     from VW_TA_DELIVERABLE_TOT_DET a
                                                  inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
			                                      where ( b.id_programa = {7} )
                                                    and ( a.M + '-' + a.Y  = '{0}' and (a.allocated= {1} or 1={2}) )
                                                    and ( a.id_mecanismo_contratacion = {3} or 1 = {4} )
                                                    and ( a.id_sub_mecanismo = {5} or 1 = {6}  )
		                                       group by a.allocated,					
					                                    a.M + '-' + a.Y,
					                                    a.nombre_ejecutor,
					                                    a.id_ejecutor", strVar, strVar_2, bndOPT, varMec, bndMec, varSubM, bndSubM, id_programa)

            ElseIf idType = 4 Then 'Activity


                strSQL = String.Format("select  ROW_NUMBER() OVER(
					                             ORDER BY 
						                             a.id_ficha_proyecto,	
                                                     a.id_ejecutor,
						                             a.allocated) N,
			                                  a.id_ficha_proyecto,
                                              a.id_ejecutor,
			                                  a.allocated, 			  			  			 
			                                  a.Codigo_SAPME,
			                                  sum(a.valor) as valor,
			                                  sum(a.valorUSD) as valorUSD
			                                 from VW_TA_DELIVERABLE_TOT_DET a
                                                inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
			                                  where  (b.id_programa = {7} )
                                                and  ( a.id_ejecutor = {0}  and (a.allocated = {1} or 1 = {2}) )
                                                and ( a.id_mecanismo_contratacion = {3} or 1 = {4} )
                                                and ( a.id_sub_mecanismo = {5} or 1 = {6}  )
		                                   group by a.allocated, 			  			  			 
			                                        a.codigo_SAPME,
					                                a.id_ficha_proyecto,
                                                    a.id_ejecutor", strVar, strVar_2, bndOPT, varMec, bndMec, varSubM, bndSubM, id_programa)

            ElseIf idType = 5 Then 'Deliverables

                strSQL = String.Format("select  ROW_NUMBER() OVER(
							                     ORDER BY 
								                     a.id_ficha_proyecto,
								                     a.allocated,
								                     a.monthCORR) N,
					                      a.allocated, 
					                      a.id_ficha_proyecto,			  
					                      a.MonthYear,			  
					                      sum(a.valor) as valor,
					                      sum(a.valorUSD) as valorUSD
					                     from VW_TA_DELIVERABLE_TOT_DET a
                                            inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
					                       where ( b.id_programa = {7} )
                                            and  ( a.id_ficha_proyecto = '{0}' and (a.allocated = {1} or 1 = {2}) )
                                            and ( a.id_mecanismo_contratacion = {3} or 1 = {4} )
                                            and ( a.id_sub_mecanismo = {5} or 1 = {6}  )
				                       group by     a.MonthYear,	
								                    a.id_ficha_proyecto,			 
								                    a.allocated,
								                    a.monthCORR ", strVar, strVar_2, bndOPT, varMec, bndMec, varSubM, bndSubM, id_programa)

            End If

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_DELIVERABLE_TOT", strField, 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item(strField) = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverables_values = tbl_result

        End Function


        Public Function get_Deliverable_Activity(ByVal idFichaProyecto As Integer, ByVal idDeliverable As Integer) As DataTable

            Dim strSQL As String = ""

            If idFichaProyecto > 0 Then
                strSQL = String.Format("select * from vw_tme_ficha_entregables
	                                                 where (  id_ficha_proyecto ={0} )
                                                   order by numero_entregable", idFichaProyecto)
                '--Order by D_Days desc 
            Else
                strSQL = String.Format("select * from vw_tme_ficha_entregables
	                                                 where ( id_deliverable ={0} )
                                                   order by numero_entregable", idDeliverable)
            End If

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_entregable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverable_Activity = tbl_result

        End Function



        Public Function get_Deliverable_Result(ByVal idFichaProyecto As Integer, Optional bnd_avance As Boolean = False) As DataTable

            Dim bndAV As Integer = If(bnd_avance, 0, 1)

            Dim strSQL As String = String.Format("select *,
                                                     case 
	                                                     when porc_previo >= 0 and porc_previo <= 33.33 then
		                                                   'progress-bar-yellow' 
		                                                  when porc_previo > 33.33 and porc_previo <= 66.66 then
		                                                   'progress-bar-primary' 
		                                                   else
		                                                   'progress-bar-success'
		                                                  end as progress_bar_previo,
		                                                  case
		                                                   when (porc_previo + porc_actual) >= 0 and (porc_previo + porc_actual) <= 33.33 then
		                                                   'progress-bar-yellow' 
		                                                  when (porc_previo + porc_actual) > 33.33 and (porc_previo + porc_actual) <= 66.66 then
		                                                   'progress-bar-primary' 
		                                                   else
		                                                   'progress-bar-success'
		                                                  end as progress_bar_actual,
		                                                  case 
	                                                         when (porc_previo + porc_actual)  >= 0 and (porc_previo + porc_actual) <= 33.33 then
		                                                       'bg-yellow'  
		                                                      when (porc_previo + porc_actual) > 33.33 and (porc_previo + porc_actual) <= 66.66 then
		                                                       'bg-light-blue' 
		                                                      else
		                                                        'bg-green'
		                                                        end as bg_color,
                                                             (porc_previo + porc_actual) as porc_total,
															 reports
                                                                from VW_DELIVERABLE_RESULT
                                                                 where    id_ficha_proyecto = {0} 
                                                                    and ( avance_actual > 0 or 1 = {1} )
                                                                    and ( avance_actual = 0 or 0 = {1} )                                                                    
                                                                 order by  orden_matriz_LB ", idFichaProyecto, bndAV)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_meta_indicador_ficha") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverable_Result = tbl_result

        End Function



        Public Function get_Deliverable_Result(ByVal idFichaProyecto As Integer, ByVal idDeliverable As Integer) As DataTable


            Dim strSQL As String = String.Format("select *,
                                                     case 
	                                                     when porc_previo >= 0 and porc_previo <= 33.33 then
		                                                   'progress-bar-yellow' 
		                                                  when porc_previo > 33.33 and porc_previo <= 66.66 then
		                                                   'progress-bar-primary' 
		                                                   else
		                                                   'progress-bar-success'
		                                                  end as progress_bar_previo,
		                                                  case
		                                                   when (porc_previo + porc_actual) >= 0 and (porc_previo + porc_actual) <= 33.33 then
		                                                   'progress-bar-yellow' 
		                                                  when (porc_previo + porc_actual) > 33.33 and (porc_previo + porc_actual) <= 66.66 then
		                                                   'progress-bar-primary' 
		                                                   else
		                                                   'progress-bar-success'
		                                                  end as progress_bar_actual,
		                                                  case 
	                                                         when (porc_previo + porc_actual)  >= 0 and (porc_previo + porc_actual) <= 33.33 then
		                                                       'bg-yellow'  
		                                                      when (porc_previo + porc_actual) > 33.33 and (porc_previo + porc_actual) <= 66.66 then
		                                                       'bg-light-blue' 
		                                                      else
		                                                        'bg-green'
		                                                        end as bg_color,
                                                             (porc_previo + porc_actual) as porc_total,
															 reports
                                                                from VW_DELIVERABLE_ACTIVITY_RESULT
                                                                 where id_ficha_proyecto = {0} 
                                                                 and id_deliverable = {1} 
                                                                 order by  orden_matriz_LB ", idFichaProyecto, idDeliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idFichaProyecto, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_meta_indicador_ficha") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverable_Result = tbl_result

        End Function


        Public Function get_Deliverable_Reported(ByVal idDeliverble As Integer) As DataTable


            Dim strSQL As String = String.Format("select a.codigo_SAPME, 
	                                                        a.id_indicador,
			                                                a.codigo_indicador, 
			                                                a.nombre_indicador_LB, 
			                                                a.meta_total, 
			                                                sum(a.valor_avance) as valor_avance, 
			                                                sum(b.del_res_value) as del_res_value,
			                                                case a.meta_total
			                                                 when 0 then
			                                                   0
			                                                 else
			                                                   round(convert(numeric(16,2),( (sum(a.valor_avance)  / a.meta_total) * 100  ) ),2)			
			                                                 end as porc_INDreported,
			                                                 case a.meta_total
			                                                 when 0 then
			                                                   0
			                                                 else
			                                                   round(convert(numeric(16,2),( (sum(b.del_res_value)  / a.meta_total) * 100)),2)			
			                                                 end as porc_DELIVreported 			  
	                                                   from vw_tme_avance_meta_indicador  a
	                                                   inner join  ta_deliverable_results b on (a.id_avance_meta_indicador = b.id_avance_meta_indicador )
	                                                 where b.id_deliverable = {0}
	                                                 group by a.codigo_SAPME, a.id_indicador, a.codigo_indicador, a.nombre_indicador_LB, a.meta_total", idDeliverble)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_deliverable", idDeliverble, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_indicador") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverable_Reported = tbl_result

        End Function



        Public Function get_Indicator_Reports(ByVal id_meta_indicador_ficha As Integer) As DataTable


            Dim strSQL As String = String.Format("select ROW_NUMBER() OVER(ORDER BY a.fiscal_year, a.id_avance_meta_indicador  ASC) AS N, 
                                                    isnull(b.id_deliverable_results,0) as id_deliverable_results,
                                                    a.id_meta_indicador_ficha,
	                                                a.id_avance_meta_indicador, 
	                                                a.fiscal_year,
	                                                a.valor_avance, 
	                                                sum(isnull(b.del_res_Value,0)) as Reported	
                                                  from vw_tme_avance_meta_indicador a
                                                   left join ta_deliverable_results b on (a.id_avance_meta_indicador = b.id_avance_meta_indicador )
                                                 where a.id_meta_indicador_ficha = {0}
                                                 and a.reversado = 0
                                                 group by  b.id_deliverable_results, 
                                                           a.id_avance_meta_indicador,
                                                           a.id_meta_indicador_ficha, 
			                                               a.fiscal_year,
			                                               a.valor_avance 
                                                order by a.fiscal_year, a.id_avance_meta_indicador", id_meta_indicador_ficha)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_meta_indicador_ficha", id_meta_indicador_ficha, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_avance_meta_indicador") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Indicator_Reports = tbl_result

        End Function



        Public Function Deliv_Result(ByVal id_deliverable As Integer) As Integer

            Dim strSQL As String = String.Format("select count(*) as N from ta_deliverable a
                                                 inner join  ta_deliverable_results b on (a.id_deliverable = b.id_deliverable )
                                                 where a.id_deliverable = {0} ", id_deliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_documento", id_deliverable, strSQL)


            Deliv_Result = tbl_result.Rows.Item(0).Item("N")

        End Function


        Public Function Deliv_Document(ByVal id_deliverable As Integer, Optional idDoc As Integer = 0) As DataTable

            Dim strSQL As String = If(idDoc = 0, String.Format("select * from ta_documento_deliverable where id_deliverable = {0} ", id_deliverable), String.Format("select * from ta_documento_deliverable where id_documento = {0} ", idDoc))

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_documento_deliverable", id_deliverable, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Deliv_Document = tbl_result

        End Function





        Public Function Deliv_Document_detail(ByVal id_deliverable As Integer) As DataTable

            Dim strSQL As String = String.Format("select * from VW_documento_deliverable_detail where id_deliverable = {0} ", id_deliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto(" VW_documento_deliverable_detail", "id_documento_deliverable", id_deliverable, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_documento_deliverable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Deliv_Document_detail = tbl_result

        End Function

        Public Function Deliv_Support_Docs(ByVal id_deliverable As Integer) As DataTable

            Dim strSQL As String = String.Format("select * from ta_deliverable_support_docs where id_deliverable = {0} ", id_deliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_deliverable_support_docs", "id_deliverable", id_deliverable, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Deliv_Support_Docs = tbl_result

        End Function



        Public Function Deliv_Support_Docs_del(ByVal id_deliverable_support_docs As Integer) As Boolean

            Dim strSQL As String = String.Format("delete from ta_deliverable_support_docs where id_deliverable_support_docs = {0} ", id_deliverable_support_docs)

            Try

                CNN_.Open()
                Dim dm As New SqlCommand(strSQL, CNN_)
                dm.ExecuteNonQuery()
                CNN_.Close()

                Deliv_Support_Docs_del = True

            Catch ex As Exception
                Deliv_Support_Docs_del = False
                CNN_.Close()
            End Try

        End Function


        Public Function Deliv_Support_Documents_detail(ByVal id_deliverable As Integer) As DataTable

            Dim strSQL As String = String.Format("select  ROW_NUMBER() OVER(ORDER BY b.nombre_documento, a.archivo ASC) AS no,
                                                        a.id_deliverable_support_docs,
		                                                a.id_deliverable,
		                                                a.archivo,
		                                                a.id_doc_soporte,
		                                                b.nombre_documento,
		                                                a.ver,		
		                                                SUBSTRING(ltrim(rtrim(RIGHT(a.archivo,5))),CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5)))),LEN(ltrim(rtrim(RIGHT(a.archivo,5)))) - ((CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5))))) - 1)) as ext,
		                                                isnull(c.ext_icon,'fa fa-file') as ext_icon
                                                    from ta_deliverable_support_docs a
                                                      inner join ta_docs_soporte b on (a.id_doc_soporte = b.id_doc_soporte)
                                                       left join ta_catalogo_extensiones c on ( SUBSTRING(ltrim(rtrim(RIGHT(a.archivo,5))),CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5)))),LEN(ltrim(rtrim(RIGHT(a.archivo,5)))) - ((CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5))))) - 1)) = ltrim(rtrim(c.nom_ext)) )
                                                     where id_deliverable = {0} order by b.nombre_documento, a.archivo ", id_deliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_deliverable_support_docs", "id_deliverable_support_docs", id_deliverable, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_deliverable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Deliv_Support_Documents_detail = tbl_result

        End Function






        Public Function get_Indicator_Reports_Tot(ByVal id_meta_indicador_ficha As Integer) As Double


            Dim strSQL As String = String.Format("select isnull(sum(b.del_res_Value),0) as Reported
                                                   from vw_tme_avance_meta_indicador a
                                                    left join ta_deliverable_results b on (a.id_avance_meta_indicador = b.id_avance_meta_indicador )
                                                  where a.id_meta_indicador_ficha = {0}
                                                 and a.reversado = 0", id_meta_indicador_ficha)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_meta_indicador_ficha", id_meta_indicador_ficha, strSQL)

            get_Indicator_Reports_Tot = tbl_result.Rows.Item(0).Item("reported")

        End Function

        Public Function get_Deliverable(ByVal idDeliverable As Integer) As ta_deliverable

            Using db As New dbRMS_JIEntities

                get_Deliverable = db.ta_deliverable.Find(idDeliverable)

            End Using

        End Function

        Public Function Save_deliverable(ByVal Deliverable As ta_deliverable, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim ta_deliverable_upd As ta_deliverable

                    If ID = 0 Then

                        db.ta_deliverable.Add(Deliverable)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        ta_deliverable_upd = db.ta_deliverable.Find(ID)

                        ta_deliverable_upd.description = Deliverable.description
                        ta_deliverable_upd.notes = Deliverable.notes
                        ta_deliverable_upd.id_programa = Deliverable.id_programa

                        'ta_deliverable_upd.fecha_creo = Deliverable.fecha_creo
                        'ta_deliverable_upd.usuario_creo = Deliverable.usuario_creo
                        ta_deliverable_upd.valor_final = Deliverable.valor_final
                        ta_deliverable_upd.tasa_cambio = Deliverable.tasa_cambio
                        ta_deliverable_upd.fecha_upd = Deliverable.fecha_upd
                        ta_deliverable_upd.usuario_upd = Deliverable.usuario_upd
                        ta_deliverable_upd.id_deliverable_estado = Deliverable.id_deliverable_estado
                        ta_deliverable_upd.id_ficha_entregable = Deliverable.id_ficha_entregable

                        If Deliverable.fecha_aprobo.HasValue Then
                            ta_deliverable_upd.fecha_aprobo = Deliverable.fecha_aprobo
                        End If

                        If Deliverable.fecha_entrego.HasValue Then
                            ta_deliverable_upd.fecha_entrego = Deliverable.fecha_entrego
                        End If

                        If Deliverable.fecha_complete.HasValue Then
                            ta_deliverable_upd.fecha_complete = Deliverable.fecha_complete
                        End If

                        If Deliverable.id_deliverable_estado = 3 Or Deliverable.id_deliverable_estado = 4 Or Deliverable.id_deliverable_estado = 6 Then

                            'ta_deliverable_upd.fecha_complete = Deliverable.fecha_complete
                            ta_deliverable_upd.usuario_complete = Deliverable.usuario_complete

                        End If

                        db.Entry(ta_deliverable_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then


                        If ID = 0 Then
                            result = Deliverable.id_deliverable
                        Else
                            result = ta_deliverable_upd.id_deliverable
                        End If

                        Save_deliverable = result

                    Else
                        Save_deliverable = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_deliverable = ex.Message

            End Try

        End Function



        Public Function Save_Ficha_Entregable(ByVal oEntregable As tme_ficha_entregables, Optional ID As Integer = 0) As String

            Try


                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim oEntregable_upd As tme_ficha_entregables

                    oEntregable_upd = db.tme_ficha_entregables.Find(ID)

                    oEntregable_upd.delivered_date = oEntregable.delivered_date
                    oEntregable_upd.valor_final = oEntregable.valor_final
                    oEntregable_upd.tasa_cambio = oEntregable.tasa_cambio

                    db.Entry(oEntregable_upd).State = Entity.EntityState.Modified

                    If (db.SaveChanges()) Then

                        result = oEntregable_upd.id_ficha_entregable

                        Save_Ficha_Entregable = result

                    Else
                        Save_Ficha_Entregable = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_Ficha_Entregable = ex.Message

            End Try

        End Function



        Public Function Save_deliverable_result(ByVal deliverable_result As ta_deliverable_results, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim deliverable_result_upd As ta_deliverable_results

                    If ID = 0 Then

                        db.ta_deliverable_results.Add(deliverable_result)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        deliverable_result_upd = db.ta_deliverable_results.Find(ID)

                        deliverable_result_upd.del_res_value = deliverable_result.del_res_value
                        'ta_deliverable_upd.description = Deliverable.description
                        'ta_deliverable_upd.notes = Deliverable.notes
                        'ta_deliverable_upd.id_programa = Deliverable.id_programa
                        'ta_deliverable_upd.fecha_upd = Deliverable.fecha_creo
                        'ta_deliverable_upd.id_deliverable_estado = Deliverable.id_deliverable_estado
                        'ta_deliverable_upd.usuario_upd = Deliverable.usuario_creo
                        'ta_deliverable_upd.id_deliverable_estado = Deliverable.id_deliverable_estado
                        'ta_deliverable_upd.id_ficha_entregable = Deliverable.id_ficha_entregable

                        db.Entry(deliverable_result_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = deliverable_result.id_deliverable_results
                        Else
                            result = deliverable_result_upd.id_deliverable_results
                        End If

                        Save_deliverable_result = result

                    Else
                        Save_deliverable_result = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_deliverable_result = ex.Message

            End Try

        End Function


        Public Function Save_documento_deliverable(ByVal documento_deliverable As ta_documento_deliverable, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim documento_deliverable_upd As ta_documento_deliverable

                    If ID = 0 Then

                        db.ta_documento_deliverable.Add(documento_deliverable)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        documento_deliverable_upd = db.ta_documento_deliverable.Find(ID)
                        documento_deliverable_upd.id_tipoDocumento = documento_deliverable.id_tipoDocumento
                        documento_deliverable_upd.id_documento = documento_deliverable.id_documento '--this Is already selescted For the ones already started

                        db.Entry(documento_deliverable_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = documento_deliverable.id_documento_deliverable
                        Else
                            result = documento_deliverable_upd.id_documento_deliverable
                        End If

                        Save_documento_deliverable = result

                    Else
                        Save_documento_deliverable = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_documento_deliverable = ex.Message

            End Try

        End Function




        Public Function Save_deliverable_support_docs(ByVal deliverable_support_docs As ta_deliverable_support_docs, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim deliverable_support_docs_upd As ta_deliverable_support_docs

                    If ID = 0 Then

                        db.ta_deliverable_support_docs.Add(deliverable_support_docs)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        deliverable_support_docs_upd = db.ta_deliverable_support_docs.Find(ID)

                        deliverable_support_docs_upd.archivo = deliverable_support_docs.archivo
                        deliverable_support_docs_upd.id_doc_soporte = deliverable_support_docs.id_doc_soporte
                        deliverable_support_docs_upd.ver = deliverable_support_docs.ver

                        db.Entry(deliverable_support_docs_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = deliverable_support_docs.id_deliverable_support_docs
                        Else
                            result = deliverable_support_docs_upd.id_deliverable_support_docs
                        End If

                        Save_deliverable_support_docs = result

                    Else
                        Save_deliverable_support_docs = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_deliverable_support_docs = ex.Message

            End Try

        End Function

        Public Function get_Deliverables_Implementer(ByVal idImplementer As Integer, Optional strKeyWord As String = "") As DataTable

            Dim bndImplementer As Integer = If(idImplementer = 0, 1, 0)

            Dim bndSearch As Integer = 1
            If strKeyWord.Length > 0 Then
                bndSearch = 0
            End If

            Dim strSQL As String = String.Format(" select * from vw_tme_ficha_entregables
	                                                  where ( id_programa = {4})	      
                                                       and  ( id_ejecutor = {0} or 1={1} )	      
	                                                   and  ( id_deliverable_estado <> 0 ) 
                                                       and  ( 1={3} and (codigo_SAPME like '%{2}%' or Implementer like '%{2}%' or descripcion_entregable like '%{2}%') )
	                                                        order by codigo_SAPME, D_Days desc", idImplementer, bndImplementer, strKeyWord.Trim, bndSearch, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idImplementer, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_entregable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverables_Implementer = tbl_result

        End Function


        Public Function get_DeliverableID(ByVal codigo_Activity As String, ByVal Deliverable_Number As Integer) As Integer

            Dim strSQL As String = String.Format("select * from vw_tme_ficha_entregables where codigo_SAPME = '{0}' and numero_entregable = {1} ", codigo_Activity, Deliverable_Number)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_entregable") = 0) Then
                get_DeliverableID = -1
            Else
                get_DeliverableID = tbl_result.Rows.Item(0).Item("id_deliverable")
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_Tot_Deliverables(ByVal varMec As Integer, ByVal varSubM As Integer) As DataTable

            Dim bndMec As Integer = If(varMec <= 0, 1, 0)
            Dim bndSubM As Integer = If(varSubM <= 0, 1, 0)

            Dim strSQL As String = String.Format("select ROW_NUMBER() OVER(ORDER BY 
                                                                 a.allocated) N, 
                                                                 a.allocated, 
                                                                sum(a.valor) as valor, 
                                                                sum(a.valorUSD) valorUSD 
                                                           from VW_TA_DELIVERABLE_TOT_DET a
                                                      inner join vw_tme_ficha_entregables b on (a.id_Ficha_entregable = b.id_ficha_entregable)
                                                where    (b.id_programa = {4} )
                                                     and ( a.id_mecanismo_contratacion = {0} or 1 = {1} )
                                                     and ( a.id_sub_mecanismo = {2} or 1 = {3}  )
                                               group by a.allocated", varMec, bndMec, varSubM, bndSubM, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("VW_TA_DELIVERABLE_TOT_DET", "N", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("N") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Tot_Deliverables = tbl_result

        End Function


        Public Function get_Tot_Funding(ByVal varMec As Integer, ByVal varSubM As Integer) As DataTable


            Dim bndMec As Integer = If(varMec <= 0, 1, 0)
            Dim bndSubM As Integer = If(varSubM <= 0, 1, 0)

            Dim strSQL As String = String.Format("select ROW_NUMBER() OVER(ORDER BY 
								                                    Tab.anio) N,
	                                                                Tab.anio,
				                                                    case Tab.anio
		                                                              when 2020 then 'FY1'
				                                                      when 2021 then 'FY2'
				                                                      when 2022 then 'FY3'
				                                                      when 2023 then 'FY4'
				                                                      when 2024 then 'FY5'
				                                                      else 'FY6' end as FY,	
				                                                      sum(Tab.Total) as valor,
				                                                      sum(Tab.TotalUSD) as valorUSD
                                                             from 
		                                                     ( select distinct anio, 0 as Total, 0 as TotalUSD from t_trimestre
                                                                where anio >=2020
		                                                      UNION ALL
		                                                      select b.anio,		  
		                                                          sum(a.monto_aporte) as Total, 
			                                                      sum(convert(decimal(18,2),a.TotalUSD)) as TotalUSD
		                                                          from vw_tme_ficha_aportes a
		                                                       inner join vw_tme_ficha_proyecto b on (a.id_ficha_proyecto = b.id_ficha_proyecto)
		                                                       where a.id_AporteOrigen = 8 
                                                                and ( {4} in (select part from [dbo].[SDF_SplitString](b.id_programa,',')) ) 	    
                                                                and ( a.id_ficha_proyecto not in (1) and b.id_sub_mecanismo not in (5,12,13) )
		                                                        and ( b.id_mecanismo_contratacion = {0}  or 1 = {1} )
			                                                    and ( b.id_sub_mecanismo = {2} or 1 = {3} )
		                                                       group by b.anio) as Tab
                                                             group by Tab.anio
                                                          ORDER BY Tab.anio ", varMec, bndMec, varSubM, bndSubM, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_PB_funding", "N", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("N") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Tot_Funding = tbl_result

        End Function



        Public Function get_Budget_Apportes(ByVal idPrograma As Integer) As DataTable

            Dim strSQL As String = String.Format("select * from vw_aportes where id_programa = {0} and id_budget = 5 ", idPrograma)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_aportes", "id_aporte", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_aporte") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Budget_Apportes = tbl_result

        End Function

        Public Function get_Deliverables(ByVal idDeliverable As Integer) As DataTable

            Dim strSQL As String = String.Format("select * from vw_tme_ficha_entregables where id_deliverable = {0}", idDeliverable)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_ficha_entregable", idDeliverable, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_ficha_entregable") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Deliverables = tbl_result

        End Function


        Public Function get_Deliverable_ApprovalUser(ByVal id_Usr As String, Optional idTipoDocumento As Integer = 0) As DataTable



            Dim bndTipoDoc As Integer = If(idTipoDocumento = 0, 1, 0)

            Dim strSQL As String = String.Format("select * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'DELIV-RMS01' and ( id_TipoDocumento = {2} or 1 = {3} ) ", id_programa, id_Usr, idTipoDocumento, bndTipoDoc)

            get_Deliverable_ApprovalUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Deliverable_ApprovalUser.Rows.Count = 1 And get_Deliverable_ApprovalUser.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_Deliverable_ApprovalUser.Rows.Remove(get_Deliverable_ApprovalUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Deliverable_Approval() As DataTable

            Dim strSQL As String = String.Format("select distinct id_tipoDocumento, descripcion_aprobacion From vw_roles_approvals where id_programa = {0} and tool_code = 'DELIV-RMS01'  ", id_programa)

            get_Deliverable_Approval = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Deliverable_Approval.Rows.Count = 1 And get_Deliverable_Approval.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_Deliverable_Approval.Rows.Remove(get_Deliverable_Approval.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Deliverable_ApprovalSEL(ByVal id_Usr As Integer, ByVal id_tipoDoc As Integer) As DataTable

            'id_usuario= {1} and
            ' id_Usr
            Dim strSQL As String = String.Format("select top 1 * From vw_roles_approvals where id_programa = {0} and  id_tipoDocumento = {1} ", id_programa, id_tipoDoc)

            get_Deliverable_ApprovalSEL = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Deliverable_ApprovalSEL.Rows.Count = 1 And get_Deliverable_ApprovalSEL.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_Deliverable_ApprovalSEL.Rows.Remove(get_Deliverable_ApprovalSEL.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function

        Public Function get_Doc_support_Route_Deliverable(ByVal id_TipoDoc As Integer, Optional strDocs As String = "") As DataTable 'This function gave me the allowed files


            Dim bndDOCS As Integer = If(strDocs.Length > 0, 0, 1)

            If strDocs.Length = 0 Then
                strDocs = "0"
            End If

            Dim Sql As String = String.Format("SELECT ta_docs_soporte.id_doc_soporte, 
				                                    ta_docs_soporte.nombre_documento, 
				                                    ta_docs_soporte.id_programa, 
                                                    ta_docs_soporte.Template, 
                                                    ta_docs_soporte.extension, 
				                                    ta_aprobacion_docs.id_app_docs, 
				                                    ta_aprobacion_docs.id_tipoDocumento,
                                                    ta_aprobacion_docs.PermiteRepetir, 
                                                    ta_aprobacion_docs.RequeridoInicio, 
                                                    ta_aprobacion_docs.RequeridoFin,
                                                    ta_docs_soporte.environmental,
													ta_docs_soporte.deliverable,
													ta_docs_soporte.max_size	          
				                                    FROM ta_docs_soporte            
				                                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
				                                    WHERE ( ta_docs_soporte.id_programa = {0} 
				                                     and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
				                                     AND (  ( ta_aprobacion_docs.id_doc_soporte  NOT IN ({2}) OR 1={3} )
                                                          OR ta_aprobacion_docs.PermiteRepetir='SI' ) ", id_programa, id_TipoDoc, strDocs, bndDOCS)

            get_Doc_support_Route_Deliverable = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_Deliverable.Rows.Count = 1 And get_Doc_support_Route_Deliverable.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_Deliverable.Rows.Remove(get_Doc_support_Route_Deliverable.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        Public Function get_Doc_support_Route_Deliverable_Filtered(ByVal id_TipoDoc As Integer, Optional strDocs As String = "") As DataTable 'This function gave me the allowed files


            Dim bndDOCS As Integer = If(strDocs.Length > 0, 0, 1)

            If strDocs.Length = 0 Then
                strDocs = "0"
            End If

            Dim Sql As String = String.Format("SELECT  ta_docs_soporte.id_doc_soporte, 
		                                                ta_docs_soporte.nombre_documento, 
		                                                ta_docs_soporte.id_programa, 
                                                        ta_docs_soporte.Template, 
                                                        ta_docs_soporte.extension, 
		                                                ta_aprobacion_docs.id_app_docs, 
		                                                ta_aprobacion_docs.id_tipoDocumento,
                                                        ta_aprobacion_docs.PermiteRepetir, 
                                                        ta_aprobacion_docs.RequeridoInicio, 
                                                        ta_aprobacion_docs.RequeridoFin,
                                                        ta_docs_soporte.environmental,
		                                                ta_docs_soporte.deliverable,
		                                                ta_docs_soporte.max_size	          
		                                                FROM ta_docs_soporte            
		                                                INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
		                                                WHERE ( ta_docs_soporte.id_programa = {0} 
		                                                 and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
		                                                 and ta_aprobacion_docs.id_ruta = 0
		                                                 AND ( (ta_aprobacion_docs.id_doc_soporte  NOT IN ({2}) OR 1={3} )
                                                                 OR ta_aprobacion_docs.PermiteRepetir='SI' )
                                                UNION
                                                SELECT  ta_docs_soporte.id_doc_soporte, 
		                                                ta_docs_soporte.nombre_documento, 
		                                                ta_docs_soporte.id_programa, 
                                                        ta_docs_soporte.Template, 
                                                        ta_docs_soporte.extension, 
		                                                ta_aprobacion_docs.id_app_docs, 
		                                                ta_aprobacion_docs.id_tipoDocumento,
                                                        ta_aprobacion_docs.PermiteRepetir, 
                                                        ta_aprobacion_docs.RequeridoInicio, 
                                                        ta_aprobacion_docs.RequeridoFin,
                                                        ta_docs_soporte.environmental,
		                                                ta_docs_soporte.deliverable,
		                                                ta_docs_soporte.max_size	          
		                                                FROM ta_docs_soporte            
		                                                INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
		                                                WHERE ( ta_docs_soporte.id_programa = {0} 
		                                                 and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
		                                                 and ta_aprobacion_docs.id_ruta in ( SELECT id_ruta From vw_ta_ruta_aprobacion WHERE id_tipoDocumento = {1} and orden = 0 )
		                                                 AND ( (ta_aprobacion_docs.id_doc_soporte  NOT IN ({2}) OR 1={3} )
                                                                 OR ta_aprobacion_docs.PermiteRepetir='SI' ) ", id_programa, id_TipoDoc, strDocs, bndDOCS)

            get_Doc_support_Route_Deliverable_Filtered = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_Deliverable_Filtered.Rows.Count = 1 And get_Doc_support_Route_Deliverable_Filtered.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_Deliverable_Filtered.Rows.Remove(get_Doc_support_Route_Deliverable_Filtered.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function CrearCodigo_Deliverable(ByVal idCat As Integer) As String
            Dim codigoApp As String = ""


            Dim CorrrelativoStr As String = ""


            Dim strSQL As String = String.Format("SELECT id_categoria, cod_categoria, correlativos FROM ta_categoria WHERE id_categoria={0}", idCat)

            Dim TblCat As DataTable = cl_utl.setObjeto("ta_categoria", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (TblCat.Rows.Count = 1 And TblCat.Rows.Item(0).Item("id_categoria") = 0) Then
                TblCat.Rows.Remove(TblCat.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


            Dim Corrrelativo As Integer = 0

            If Not IsNothing(TblCat) Then
                Corrrelativo = TblCat.Rows(0).Item("correlativos") + 1
            Else
                Dim rnd As New Random()
                Corrrelativo = rnd.Next(1, 999999)
            End If


            If Corrrelativo < 10 Then
                CorrrelativoStr = "000"
            ElseIf Corrrelativo < 100 Then
                CorrrelativoStr = "00"
            ElseIf Corrrelativo < 1000 Then
                CorrrelativoStr = "0"
            Else
                CorrrelativoStr = ""
            End If

            If Not IsNothing(TblCat) Then
                Corrrelativo = TblCat.Rows(0).Item("correlativos") + 1
                codigoApp = TblCat.Rows(0).Item("cod_categoria") & "-" & CorrrelativoStr & Corrrelativo
            Else

                Corrrelativo += 1
                codigoApp = "CATNOCDE" & "-" & CorrrelativoStr & Corrrelativo

            End If

            Return codigoApp

        End Function





        Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String, ByVal idUser As Integer)

            Dim clss_approval As APPROVAL.clss_approval = New APPROVAL.clss_approval(id_programa)


            Dim strComment As String
            If Trim(Comment).Length = 0 Then
                strComment = "--No Comments--"
            Else
                strComment = Comment
            End If


            clss_approval.set_ta_comentariosDoc(0) 'New Record
            clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", idApp, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByProcess, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_usuario", idUser, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Replace("  ", ""), "id_comment", 0)

            If clss_approval.save_ta_comentariosDoc() = -1 Then
                'Error do somenthing

            End If


        End Sub



        Public Function GetDeliverables(ByVal idFichaProyecto As Integer, Optional idDeliverable As Integer = 0) As String

            Dim tbl_Deliverables As DataTable = get_Deliverable_Activity(idFichaProyecto, idDeliverable)

            Dim strRowsTOT As String = ""
            Dim strRows As String = "<tr>
                                       <td><div class='tools'><a href='/RMS_APPROVAL/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
                                       <td>{0}</td>
                                       <td>
                                          <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                              {1}
                                          </div>
                                       </td>                                    
                                       <td>{3:d}</td>                                       
                                       <td>{5:P2}</td>
                                       <td>{6:N2} UGX</td>
                                       <td>                                                                       
                                         <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
                                       </td>
                                    </tr>"


            Dim strRows2 As String = " <tr>
                                       <td colspan='7'>                                                                     
                                          <div class='progress'>
                                              <div class='progress-bar {0} progress-bar-striped' role='progressbar' aria-valuenow='{1}' aria-valuemin='0' aria-valuemax='100' style='width: {1}%'>
                                                   <span >{1}% </span>
                                              </div> 
                                              <div class='progress-bar {2} progress-bar-striped' role='progressbar' aria-valuenow='{3}' aria-valuemin='0' aria-valuemax='100' style='width: {3}%'>
                                                    <span>{4}% </span>
                                              </div>                                                                            
                                          </div>
                                        </td>  
                                    </tr> "


            Dim strTableDEL As String = "<table class='table table-hover'>
                                                                <tr>
                                                                  <th>Deliverable #</th>
                                                                  <td><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                </tr>
                                                                <tr>                                                                  
                                                                  <td colspan='2'>
                                                                     <div style='text-align:left; max-width:100%;'>
                                                                         {1}
                                                                     </div>
                                                                   </td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                  <th>Due Date</th>
                                                                  <td>{2:d}</td>   
                                                                </tr>     
                                                                <tr>
                                                                  <th>Status</th>
                                                                  <td><span class='label {5}'>{3}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{4}</span></td>   
                                                                </tr>  
                                                                 <tr>
                                                                  <th>Porcent</th>
                                                                  <td>{6:P2}</td>   
                                                                </tr>   
                                                                <tr>
                                                                  <th>Amount</th>
                                                                  <td> {7:N2} UGX / {8:N2} USD</td>   
                                                                </tr>                                                               
                                                              </table>"


            Dim strTable_nextDEL As String = ""
            Dim id_ficha_entregable As Integer = 0

            Dim strStatus As String = ""
            Dim strTime As String = ""
            Dim strAlert As String = ""

            Dim strAlert2 As String = ""
            Dim strAlert3 As String = ""
            Dim vDiferences As Double = 0
            Dim vDiferences_Adj As Double = 0
            Dim vDays As Double = 0

            'Dim YLAfunding As Double = 0
            'Dim YLAfundingUSD As Double = 0
            'Dim PerformedFunding As Double = 0
            'Dim PerformedFundingUSD As Double = 0
            'Dim PendingFunding As Double = 0
            'Dim PendingFundingUSD As Double = 0
            'Dim PorcenPerformed As Double = 0


            For Each dtRow As DataRow In tbl_Deliverables.Rows

                Dim rDays As Double

                'YLAfunding = dtRow("monto_aporte")
                'If dtRow("id_deliverable_estado") = 3 Then
                '    PerformedFunding += dtRow("valor")
                'Else
                '    PendingFunding += dtRow("valor") 'Take in account when the value allocated it is not the planned
                'End If

                If dtRow("D_Days") <= 0 Then 'its not time 

                    rDays = dtRow("D_Days") * -1

                    If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                        strTime = Func_Unit(rDays)
                        strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                        strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                    Else 'finish processes

                        strTime = Func_Unit(rDays)
                        strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                        strStatus = dtRow("deliverable_estado")

                    End If


                    vDiferences = Math.Round((dtRow("porc_Days") - dtRow("porc_EDays")) * 100, 2, MidpointRounding.AwayFromZero)
                    vDays = (dtRow("porc_Days") * 100) - vDiferences
                    'vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)
                    vDiferences_Adj = vDiferences

                Else 'Delayed time

                    If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                        strTime = Func_Unit(dtRow("D_Days"))
                        strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                        strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                    Else 'finish processes

                        strStatus = dtRow("deliverable_estado")
                        strTime = Func_Unit(dtRow("D_Days"))
                        strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)

                    End If

                    vDays = dtRow("porc_Days") * 100
                    vDiferences = Math.Round((dtRow("porc_EDays") - dtRow("porc_Days")) * 100, 2, MidpointRounding.AwayFromZero)
                    vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)

                End If

                strAlert2 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 3)
                strAlert3 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 4)

                'String.Format("{0} #,##0.00", "$")
                'cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol
                'String.Format("{1} {0:#,###,###.##}", 0)
                'String.Format(cl_user.regionalizacionCulture, "{1} {0:N2}",dtRow("valor"),cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol)
                'cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol


                strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(cl_user.regionalizacionCulture, "{0:N2}", dtRow("valor")), strStatus, strTime, strAlert, dtRow("id_deliverable"))
                strRowsTOT &= String.Format(strRows2, strAlert2, vDays, strAlert3, vDiferences_Adj, vDiferences)

                If dtRow("id_deliverable_estado") = 0 And id_ficha_entregable = 0 Then

                    id_ficha_entregable = dtRow("id_ficha_entregable")
                    strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), dtRow("valor"), Math.Round((dtRow("valor") / 3348), 2, MidpointRounding.AwayFromZero))

                End If

            Next


            Dim strTable = "<table class='table no-margin;' style='width:100%'>
                                  <thead>
                                      <tr>
                                        <th style='width:2%;'></th>                                   
                                        <th style='width:3%;'>#</th>                    
                                        <th style='width:30%;'>Milestone</th>                                       
                                        <th style='width:8%;'>Due Date</th>                                        
                                        <th style='width:10%;'>%</th>
                                        <th style='width:8%;'>Amount</th>
                                        <th style='width:8%;'>Status</th>
                                      </tr>
                                  </thead>
                                  <tbody>   
                                       {0}
                                 </tbody>
                        </table>"

            'YLAfundingUSD = Math.Round((YLAfunding / 3348), 2, MidpointRounding.AwayFromZero)
            'PendingFundingUSD = Math.Round((PendingFunding / 3348), 2, MidpointRounding.AwayFromZero)
            'PerformedFundingUSD = Math.Round((PerformedFunding / 3348), 2, MidpointRounding.AwayFromZero)
            'PorcenPerformed = Math.Round(((PerformedFunding / YLAfunding) * 100), 2, MidpointRounding.AwayFromZero)

            GetDeliverables = String.Format(strTable, strRowsTOT.Trim)

        End Function


        Public Shared Function Func_Unit(ByVal Ndays As String) As String


            Dim vDays As Double
            Dim vWeeks As Double
            Dim vMonths As Double
            Dim vYear As Double

            Dim strUnit As String
            Dim vUnit As Double

            vDays = Ndays
            vWeeks = vDays / 7
            vMonths = vDays / 30
            vYear = vDays / 365


            If vWeeks < 1 Then

                vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

                If vDays > 1 Then
                    strUnit = " days"
                Else
                    strUnit = " day"
                End If

            ElseIf vMonths < 1 Then

                vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

                If vWeeks > 1 Then
                    strUnit = " weeks"
                Else
                    strUnit = " week"
                End If

            ElseIf vYear < 1 Then

                vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

                If vMonths > 1 Then
                    strUnit = " months"
                Else
                    strUnit = " month"
                End If

            Else

                vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

                If vYear > 1 Then
                    strUnit = " years"
                Else
                    strUnit = " year"
                End If

            End If


            Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

        End Function


        Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


            Dim Dif_Porce As Double = porcDays - porcEDays
            Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

            Const c_label_danger As String = "label-danger"
            Const c_label_warning As String = "label-warning"
            Const c_label_primary As String = "label-primary"
            Const c_label_success As String = "label-success"

            Const c_progress_bar_warning = "progress-bar-warning"
            Const c_progress_bar_primary = "progress-bar-primary"
            Const c_progress_bar_danger = "progress-bar-danger"


            Dim strResult As String = ""
            Dim strStatus As String = ""
            Dim strAlertBar1 As String = ""
            Dim strAlertBar2 As String = ""


            If porc_Progress >= 0 Then

                'Inverter number
                If ((1 - porc_Progress) * 100) >= 90 Then
                    strResult = c_label_danger
                ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
                    strResult = c_label_warning
                ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
                    strResult = c_label_primary
                Else
                    strResult = c_label_success
                End If

                strStatus = "Pending"
                strAlertBar2 = c_progress_bar_primary

            Else 'Expired
                strResult = c_label_danger
                strStatus = "Expired"
                strAlertBar2 = c_progress_bar_danger
            End If

            strAlertBar1 = c_progress_bar_warning

            If alertType = 1 Then
                Func_Alert = strResult
            ElseIf alertType = 2 Then
                Func_Alert = strStatus
            ElseIf alertType = 3 Then
                Func_Alert = strAlertBar1
            Else
                Func_Alert = strAlertBar2
            End If


        End Function

        Public Function get_ExchangeRate() As Double

            Using db As New dbRMS_JIEntities

                Dim oPeriodo = db.vw_t_periodos.Where(Function(p) p.activo = True And p.id_programa = id_programa).ToList()
                Dim fechaReg = DateTime.Now
                Dim oTasaCambio As List(Of t_trimestre_tasa_cambio)
                'Dim strCurrent_Period As String
                If oPeriodo.Count() = 0 Then
                    oTasaCambio = db.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
                    'strCurrent_Period = "--"
                Else
                    Dim idTrimestre As Integer = Convert.ToInt32(oPeriodo.FirstOrDefault.id_trimestre)
                    oTasaCambio = db.t_trimestre_tasa_cambio.Where(Function(p) p.id_trimestre = idTrimestre).ToList()
                    ' strCurrent_Period = oPeriodo.FirstOrDefault.FiscalYearNotation
                End If

                If oTasaCambio.Count() = 0 Then
                    get_ExchangeRate = 0
                Else
                    Dim valTasaCambio As Double = Convert.ToInt32(oTasaCambio.FirstOrDefault.tasa_cambio)
                    get_ExchangeRate = valTasaCambio
                End If

            End Using

        End Function

        Public Function get_ApprovalEstado_tipo() As DataTable

            Dim strSQL As String = String.Format("Select * from ta_estadoTipo")

            get_ApprovalEstado_tipo = cl_utl.setObjeto("ta_estadoTipo", "id_programa", id_programa, strSQL)

        End Function

        Public Function get_Deliverable_PEND(ByVal varMec As Integer, ByVal varSubM As Integer) As DataTable

            Dim bndMec As Integer = If(varMec <= 0, 1, 0)
            Dim bndSubM As Integer = If(varSubM <= 0, 1, 0)

            Dim strSQL As String = String.Format("select  a.Implementer as Organization, 
	                                                        a.codigo_SAPME as Activity, 
	                                                        a.numero_entregable as [No], 
	                                                        a.descripcion_entregable as Deliverable, 
															a.id_deliverable_estado,
															a.deliverable_estado as Status,
	                                                        a.fecha as Date, (a.D_Days * -1) as D_Days, 
	                                                        a.valor as Amount, 
	                                                        convert(decimal(18,2),(a.valor / a.tasa_cambio)) as AmountUSD
	                                                        from  vw_tme_ficha_entregables a
															inner join tme_ficha_proyecto b on (a.id_ficha_proyecto = b.id_ficha_proyecto)
															inner join tme_sub_mecanismo c on (c.id_sub_mecanismo = b.id_sub_mecanismo)
                                                    where  (a.id_programa = {4} )
                                                    and (c.id_mecanismo_contratacion = {0} or 1 = {1} )
                                                    and ( b.id_sub_mecanismo = {2} or 1 = {3}  )
                                                  order by a.Implementer, a.codigo_SAPME, (a.D_Days * -1)", varMec, bndMec, varSubM, bndSubM, id_programa)

            get_Deliverable_PEND = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Deliverable_PEND.Rows.Count = 1 And get_Deliverable_PEND.Rows.Item(0).Item("No") = 0) Then
                get_Deliverable_PEND.Rows.Remove(get_Deliverable_PEND.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Deliverable_Det(ByVal varMec As Integer, ByVal varSubM As Integer) As DataTable

            Dim bndMec As Integer = If(varMec <= 0, 1, 0)
            Dim bndSubM As Integer = If(varSubM <= 0, 1, 0)

            Dim strSQL As String = String.Format("select    a.id_ficha_entregable as id_entregable,
                                                            a.Implementer as Organization, 
	                                                        a.codigo_SAPME as Activity, 
															a.fecha_inicio_proyecto Start_Date,
															a.fecha_fin_proyecto End_Date,
	                                                        a.numero_entregable as [No], 
	                                                        a.descripcion_entregable as Deliverable, 
															a.verification_mile as Verification,
															a.id_deliverable_estado,
															a.deliverable_estado as Status,
	                                                        a.fecha, (a.D_Days * -1) as Expired_Days, 
	                                                        a.valor as Amount, 
	                                                        convert(decimal(18,2),(a.valor / a.tasa_cambio)) as AmountUSD,
															a.delivered_date as Due_Date,
															a.tasa_cambio as Exchange_Rate,
															a.tasa_cambio_final as Exchange_Applied ,
															a.valor_final as Disbursed,
															a.porcentaje as Percentage,
															a.createdBy,
															a.updatedBy,
															a.fecha_creo as Delievered_Date,
															a.fecha_aprobo  as Approved_Date,
															a.fecha_complete as Disbursed_Date,
															a.completedBy as Completed_By,
															case id_deliverable_minute
															 when 0 then
															  'NO'
															  else 
															   'SI'
															   end as Doc_generated,
															case minute_close
															 when 0 then
															  'NO'
															  else 
															   'SI'
															   end as Doc_Closed
	                                                        from  vw_tme_ficha_entregables a
															inner join tme_ficha_proyecto b on (a.id_ficha_proyecto = b.id_ficha_proyecto)
															inner join tme_sub_mecanismo c on (c.id_sub_mecanismo = b.id_sub_mecanismo)
                                                    where (c.id_mecanismo_contratacion = {0} or 1 = {1} )
                                                    and ( b.id_sub_mecanismo = {2} or 1 = {3}  )
                                                    and (a.id_programa = {4})
                                                  order by a.Implementer, a.codigo_SAPME, (a.D_Days * -1)", varMec, bndMec, varSubM, bndSubM, id_programa)

            get_Deliverable_Det = cl_utl.setObjeto("vw_tme_ficha_entregables", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Deliverable_Det.Rows.Count = 1 And get_Deliverable_Det.Rows.Item(0).Item("id_entregable") = 0) Then
                get_Deliverable_Det.Rows.Remove(get_Deliverable_Det.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function




    End Class





End Namespace
