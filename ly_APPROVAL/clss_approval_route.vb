Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient

Namespace APPROVAL


    Public Class clss_approval_route
        Inherits clss_approval

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim Sql As String

        Dim tbl_RutaAprobacion As New DataTable
        Dim tbl_Approval As New DataTable
        Dim tbl_ta_rutaTipoDoc As New DataTable
        Dim tbl_ta_tipoDocumento As New DataTable

        Public Property id_ruta As Integer
        Public Property id_tipoDocumento As Integer

        Sub New(ByVal id_p As Integer, Optional ByVal id_tipoD As Integer = 0)

            MyBase.New(id_p) 'Constructor fo the base class

            id_tipoDocumento = IIf(id_tipoD > 0, id_tipoD, 0)
            id_ruta = 0

        End Sub



        Public Function get_RolesUser(ByVal Optional id_TpDOC As Integer = 0) As DataTable

            Dim strSQlC As String = ""

            If id_TpDOC > 0 Then
                strSQlC = String.Format(" and ( id_rol NOT IN (SELECT id_rol FROM ta_rutaTipoDoc WHERE (id_tipoDocumento = {0})) ) ", id_TpDOC)
            End If

            Sql = String.Format("select *, nombre_rol + ' [' + nombre_usuario + '] ' as role_n from vw_ta_roles_user_all where ( id_programa = {0} )  {1} ", id_programa, strSQlC)

            get_RolesUser = cl_utl.setObjeto("vw_ta_roles_user_all", "id_programa", id_programa, Sql)

        End Function

        Public Function get_Approval_tipo() As DataTable

            Sql = String.Format("Select * from ta_estadoTipo")

            get_Approval_tipo = cl_utl.setObjeto("ta_estadoTipo", "id_programa", id_programa, Sql)

        End Function


        Public Function get_Approval_Deliverable_stage() As DataTable

            Sql = String.Format("Select * from ta_deliverable_stage")

            get_Approval_Deliverable_stage = cl_utl.setObjeto("ta_deliverable_stage", "id_programa", id_programa, Sql)

        End Function

        Public Function get_UserEmail() As DataTable

            Sql = String.Format("select id_usuario, nombre_usuario, email_usuario  from vw_t_usuarios	where id_programa = {0}  and (estado = 'ACTIVE' or estado = 'ACTIVO') ", id_programa)

            get_UserEmail = cl_utl.setObjeto("vw_t_usuarios", "id_usuario", id_programa, Sql)

        End Function




        Public Function get_RutaAprobacion() As DataTable



            Sql = String.Format(" SELECT id_rol, id_usuario, id_programa, id_ruta, id_categoria, nombre_empleado, usuario, email,  " &
                                "       descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, ruta_completa, " &
                                "       descripcion_cat, orden, id_tipoDocumento, duracion, id_approval_tool, trigger_tool, nombre_rol, id_estadoTipo, estado_tipo_prefijo, id_deliverable_stage, deliverable_stage, tool_code" &
                                "   From vw_ta_ruta_aprobacion  " &
                                "     WHERE (id_tipoDocumento = {0}) ORDER BY orden ", id_tipoDocumento)

            tbl_RutaAprobacion = cl_utl.setObjeto("vw_ta_ruta_aprobacion", "id_tipoDocumento", id_tipoDocumento, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_RutaAprobacion.Rows.Count = 1 And tbl_RutaAprobacion.Rows.Item(0).Item("id_rol") = 0) Then
                tbl_RutaAprobacion.Rows.Remove(tbl_RutaAprobacion.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_RutaAprobacion = tbl_RutaAprobacion

        End Function


        Public Function get_Ruta_Rol_Aprobacion(Optional idDoc As Integer = 0) As DataTable

            Dim tbl_res As DataTable
            Dim ID As Integer = If(idDoc = 0, id_tipoDocumento, idDoc)

            Sql = String.Format(" SELECT 0 as id_ruta, '( * ) All Members' as nombre_empleado
                                    union
                                  SELECT id_ruta, nombre_empleado
                                   From vw_ta_ruta_aprobacion  
                                     WHERE (id_tipoDocumento = {0})  ", ID)

            tbl_res = cl_utl.setObjeto("vw_ta_ruta_aprobacion", "id_tipoDocumento", ID, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_res.Rows.Count = 1 And tbl_res.Rows.Item(0).Item("id_ruta") = 0) Then
                tbl_res.Rows.Remove(tbl_res.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_Ruta_Rol_Aprobacion = tbl_res

        End Function

        Public Function get_OPEN_Proccess() As Integer

            ''Sql = String.Format("SELECT count(*) as N FROM vw_ta_AppDocumento WHERE id_tipoDocumento= {0} ", id_tipoDocumento)
            Sql = String.Format("select count(*) as N from VW_GR_TA_DOCUMENTOS WHERE id_tipoDocumento= {0} and id_estadoDoc in (1,6) ", id_tipoDocumento)

            Dim tbl_Res As New DataTable
            tbl_Res = cl_utl.setObjeto("vw_ta_AppDocumento", "id_tipoDocumento", id_tipoDocumento, Sql)

            get_OPEN_Proccess = tbl_Res.Rows.Item(0).Item("N")

        End Function




        Public Function get_ApprovalProC_Order()

            Sql = String.Format("SELECT count(orden) AS Orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento = {0}", id_tipoDocumento)

            Dim tbl_Res As New DataTable
            tbl_Res = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)

            get_ApprovalProC_Order = tbl_Res.Rows.Item(0).Item("Orden")

        End Function


        Public Function get_MinOrder(Optional ByVal vOrder = 0) As Integer

            If vOrder = 0 Then
                Sql = String.Format("SELECT MIN(orden) AS M_orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento = {0}", id_tipoDocumento)
            Else
                Sql = String.Format("SELECT isnull(MAX(orden),0) AS M_orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento = {0} and orden < {1} ", id_tipoDocumento, vOrder)
            End If

            Dim tbl_Res As New DataTable
            tbl_Res = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)

            get_MinOrder = tbl_Res.Rows.Item(0).Item("M_orden")

        End Function


        Public Function get_MaxOrder(Optional ByVal vOrder = 0) As Integer

            If vOrder = 0 Then
                Sql = String.Format("SELECT MAX(orden) AS M_orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento = {0}", id_tipoDocumento)
            Else
                Sql = String.Format("SELECT isnull(MIN(orden),0) AS M_orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento = {0} and orden > {1} ", id_tipoDocumento, vOrder)
            End If

            Dim tbl_Res As New DataTable
            tbl_Res = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)

            get_MaxOrder = tbl_Res.Rows.Item(0).Item("M_orden")

        End Function

        Public Sub route_Reorder(ByVal Oldval As Integer, ByVal NewVal As Integer)

            Dim tbl_tmp As New DataTable
            Sql = String.Format("select * from ta_rutaTipoDoc where id_tipoDocumento = {0} and orden in ({1},{2}) order by orden", id_tipoDocumento, Oldval, NewVal)
            tbl_tmp = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)

            Dim id_rutaOLD As Integer = cl_utl.getDTval(tbl_tmp, "orden", "id_ruta", Oldval)
            Dim id_rutaNew As Integer = cl_utl.getDTval(tbl_tmp, "orden", "id_ruta", NewVal)

            set_ta_rutaTipoDoc(id_rutaOLD) 'set the Objects
            set_ta_rutaTipoDocFIELDS("orden", NewVal, "id_ruta", id_rutaOLD)
            save_ta_rutaTipoDoc()

            set_ta_rutaTipoDoc(id_rutaNew) 'set the Objects
            set_ta_rutaTipoDocFIELDS("orden", Oldval, "id_ruta", id_rutaNew)
            save_ta_rutaTipoDoc()

        End Sub



        Public Sub Approval_Reorder()

            Dim tbl_tmp As New DataTable


            Sql = String.Format("select isnull(max(orden),0) as MA_Orden, isnull(min(orden),0) as MI_Orden, isnull(count(*),0) as N from ta_rutaTipoDoc where id_tipoDocumento = {0} ", id_tipoDocumento)
            tbl_tmp = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)

            Dim max As Integer = tbl_tmp.Rows.Item(0).Item("MA_Orden")
            Dim min As Integer = tbl_tmp.Rows.Item(0).Item("MI_Orden")
            Dim N As Integer = tbl_tmp.Rows.Item(0).Item("N") - 1
            Dim reorder As Boolean = False

            If N = 0 Then
                reorder = False
            ElseIf Not (min = 0 And N = max) Then
                reorder = True
            End If

            If reorder Then

                Sql = String.Format("select * from ta_rutaTipoDoc where id_tipoDocumento = {0} order by orden", id_tipoDocumento)
                tbl_tmp = cl_utl.setObjeto("ta_rutaTipoDoc", "id_tipoDocumento", id_tipoDocumento, Sql)
                Dim i = 0

                For Each dtR As DataRow In tbl_tmp.Rows
                    dtR("orden") = i
                    i += 1
                Next

                For Each dtR As DataRow In tbl_tmp.Rows 'Saving the changes
                    set_ta_rutaTipoDoc(dtR("id_ruta")) 'set the Objects
                    set_ta_rutaTipoDocFIELDS("orden", dtR("orden"), "id_ruta", dtR("id_ruta"))
                    save_ta_rutaTipoDoc()
                Next

            End If


        End Sub



        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************
        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************

        Public Function set_ta_tipoDocumento() As DataTable
            ' id_tipoDocumento = IIf(id_TipoD > 0, id_TipoD, id_tipoDocumento)
            tbl_ta_tipoDocumento = cl_utl.setObjeto("ta_tipoDocumento", "id_tipoDocumento", id_tipoDocumento)
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




        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************
        '********************************************************* ta_tipoDocumento  ENTITY ************************************************************************************



        '********************************************************* ta_rutaTipoDoc  ENTITY ************************************************************************************


        Public Function set_ta_rutaTipoDoc(ByVal id_r As Integer) As DataTable

            id_ruta = IIf(id_r > 0, id_r, 0)
            tbl_ta_rutaTipoDoc = cl_utl.setObjeto("ta_rutaTipoDoc", "id_ruta", id_ruta)
            set_ta_rutaTipoDoc = tbl_ta_rutaTipoDoc

        End Function

        Public Function get_ta_rutaTipoDocFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_ta_rutaTipoDoc, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_ta_rutaTipoDocFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_ta_rutaTipoDoc = cl_utl.setDTval(tbl_ta_rutaTipoDoc, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_ta_rutaTipoDoc() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("ta_rutaTipoDoc", tbl_ta_rutaTipoDoc, "id_ruta", id_ruta)

            If RES <> -1 Then
                set_ta_rutaTipoDocFIELDS("id_ruta", RES, "id_ruta", id_ruta)
                id_ruta = RES
                save_ta_rutaTipoDoc = RES
            Else
                save_ta_rutaTipoDoc = RES
            End If

        End Function

        Public Function del_RutaAprobacion(ByVal idR As Integer) As Boolean

            If idR > 0 Then

                Sql = String.Format(" delete from ta_rutaTipoDoc where id_ruta = {0}", idR)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_RutaAprobacion = True

                Catch ex As Exception
                    del_RutaAprobacion = False
                End Try

            Else

                del_RutaAprobacion = False

            End If

        End Function


        '********************************************************* ta_rutaTipoDoc  ENTITY ************************************************************************************




        '*********************************************************APPROVAL ENTITY************************************************************************************
        '*********************************************************APPROVAL ENTITY************************************************************************************


        Public Function set_Approval() As DataTable

            Sql = String.Format("SELECT * FROM vw_aprobaciones WHERE id_tipoDocumento= {0} ", id_tipoDocumento)
            tbl_Approval = cl_utl.setObjeto("vw_ta_AppDocumento", "id_tipoDocumento", id_tipoDocumento, Sql)
            set_Approval = tbl_Approval

        End Function


        Public Function get_Approval_DT() As DataTable
            get_Approval_DT = tbl_Approval
        End Function

        Public Function get_Approval_dtFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String
            Return cl_utl.getDTval(get_Approval_DT, campoSearch, campo, valorSearch)
        End Function

        Public Sub set_Approval_dtFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)
            tbl_Approval = cl_utl.setDTval(tbl_Approval, campoSearch, campo, valorSearch, valor)
        End Sub



        '*********************************************************APPROVAL ENTITY************************************************************************************
        '*********************************************************APPROVAL ENTITY************************************************************************************




    End Class


End Namespace
