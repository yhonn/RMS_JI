Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Imports ly_APPROVAL



Partial Class frm_consulta_docsPending
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim varUSer As String
    Dim IDstatus As Integer
    Dim IDproceso As Integer
    Private Const ItemsPerRequest As Integer = 10

    Dim clss_approval As APPROVAL.clss_approval

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPR_DOCS_PEN"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cl_utl As New CORE.cls_util

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 14, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            'If Me.Session("E_IdPerfil") = 5 Or Me.Session("E_IdPerfil") = 1 Then
            Me.grd_cate.Columns(0).Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            'End If

            Dim tbl_user_role As New DataTable

            '*********************** All Roles (Simple, Shared, Groups)***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 0)
            Dim strRoles As String = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_ALL_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** All Roles (Simple, Shared, Groups)***********************************


            '*********************** Roles (Simple and Shared)***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 1)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** Roles  (Simple and Shared)***********************************

            '***********************Group Roles***********************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 2)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_GroupRolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** Roles Roles***********************************

            '*********************** All Group Roles Just Simple Roles*******************************
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 3)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_ALL_SIMPLE_RolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** All Roles*******************************************************

            Dim Sql As String = String.Format("SELECT nombre_usuario as nombre_empleado, usuario, job as codigo FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            '"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("permisos")
            dm.Fill(ds, "permisos")
            Dim verTodo = ds.Tables("permisos").Rows(0).Item(0)

            varUSer = ds.Tables("permisos").Rows(0).Item("nombre_empleado")

            RadioButton1.Text = String.Format("Pending action by {0}", varUSer)
            RadioButton2.Text = String.Format("Pending approvals initiated by {0}", varUSer)
            RadioButton3.Text = String.Format("Pending approvals that {0} participates", varUSer)

            'If Me.Session("E_IdPerfil").ToString = "5" Or Me.Session("E_IdPerfil").ToString = "1" Or verTodo.ToString = "1" Then
            '    Me.btn_RadioButton4.Visible = True
            'End If

            '  If Me.Session("E_IdPerfil").ToString = "1" Or Me.Session("E_IdPerfil").ToString = "2" Then
            'Me.btn_RadioButton4.Visible = True
            ' End If

            ' If Me.Session("E_IdPerfil").ToString = "1" Then
            '**********************Solamente los hacemos invisibles los viejos controles***************************
            Me.btn_buscar1.Visible = False
            Me.btn_buscar0.Visible = False
            '**********************Solamente los hacemos invisibles los viejos controles***************************
            'End If

            IDstatus = 0
            IDproceso = 0
            Me.Session("E_bnd_var_01") = IDstatus
            Me.Session("E_bnd_var_02") = IDproceso
            Me.Session("E_bnd_var_03") = varUSer


            Dim currentdate As Date = Date.Today

            'Date Formulas: Find First Day of Previous Month
            'DateAdd("m", -1, DateSerial(Year(Today()), Month(Today()), 1))
            'Find Last Day of Previous Month
            'DateSerial(Year(Today()), Month(Today()), 0)
            'Find First Day of Current Month
            'DateSerial(Year(Today()), Month(Today()), 1)
            'Find Last Day of Current Month
            'DateSerial(Year(Today()), Month(DateAdd("m", 1, Today())), 0)



            Me.txt_finicio.SelectedDate = DateSerial(Year(Today()), Month(Today()), 1) '"01/" & currentdate.Month & "/" & currentdate.Year
            'Dim diaMax = Date.DaysInMonth(currentdate.Year, currentdate.Month)
            Me.txt_ffin.SelectedDate = DateSerial(Year(Today()), Month(DateAdd("m", 1, Today())), 0) 'diaMax.ToString & "/" & currentdate.Month & "/" & currentdate.Year

            grd_cate.PagerStyle.Position = GridPagerPosition.Top
            'fillGrid()



        Else

            IDstatus = Me.Session("E_bnd_var_01")
            IDproceso = Me.Session("E_bnd_var_02")
            varUSer = Me.Session("E_bnd_var_03")


        End If

    End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource

        Me.grd_cate.DataSource = getData(IDstatus, IDproceso)

    End Sub


    Public Function getData(Optional ByVal idEstatus As Integer = 0, Optional ByVal idProcess As Integer = 0) As DataTable


        '  Dim result As Object
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

        Dim idUsr = Convert.ToInt32(Me.Session("E_IdUser"))



        'Using dbEntities As New dbRMS_JIEntities

        '    Try

        '        result = dbEntities.SP_TA_DOCUMENTOS_GR(id_programa, 1, idUsr, lbl_ALL_SIMPLE_RolID.Text.Trim, lbl_ALL_RolID.Text.Trim).ToList()


        '    Catch ex As Exception
        '        result = Nothing
        '    End Try


        'End Using


        'getData = cl_utl.ConvertToDataTable(result)


        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************

        Dim strSearch As String = ""
        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim dtStar As Date = txt_finicio.SelectedDate
        Dim dtEnd As Date = txt_ffin.SelectedDate

        Dim bndDate As Integer = If(chkfilterDate.Checked, 1, 0)

        Dim bndALL As Integer = 0
        If rdb_allpending.Checked Then
            bndALL = 1
        End If

        '1 - -Pending
        '6 -- StandBy

        'Pending
        Dim tbl_Pending As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(0, 0, "", 0, "", 1, "Pending, StandBy", "", strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, bndALL, dtStar, dtEnd)
        'StandBy
        Dim tbl_StandBy As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(0, 0, "", 0, "", 6, "Pending, StandBy", "", strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, bndALL, dtStar, dtEnd)

        Dim tbl_search As DataTable = Nothing


        tbl_search = tbl_Pending.Copy
        If tbl_Pending.Rows.Count > 0 Then
            If tbl_StandBy.Rows.Count > 0 Then
                For Each dtRW In tbl_StandBy.Rows
                    tbl_search.ImportRow(dtRW)
                Next
            End If
        Else
            tbl_search = tbl_StandBy.Copy
        End If


        Me.lbltotal.Text = "[ " & tbl_search.Rows.Count & " ]" & If(tbl_search.Rows.Count = 0, " Procesos de aprobación", " Procesos de aprobación")

        If Strings.Len(strSearch) > 0 Then
            strSearch = String.Format("...Buscar por {0}", "<br />") & strSearch
        Else
            strSearch = String.Format("...Buscando todos los procesos de aprobación {0}", "<br />") & strSearch
        End If

        lblt_Search.Text = strSearch

        Return tbl_search


        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************






        'Dim sql As String = "SELECT ROW_NUMBER() OVER (ORDER BY Fecha_Recepcion) AS [No], * FROM VW_GR_TA_DOCUMENTOS " &
        '                  " WHERE id_programa=" & Me.Session("E_IDPrograma")

        'Dim strSearch As String = ""


        'If Not String.IsNullOrEmpty(Trim(Me.RadComboBox3.Text)) Then
        '    sql &= String.Format(" AND ((descripcion_doc LIKE '%{0}%') OR (nom_beneficiario LIKE '%{0}%') OR (numero_instrumento LIKE '%{0}%')) ", Trim(Me.RadComboBox3.Text))

        '    If idProcess = 0 Then
        '        strSearch &= String.Format(" Contains the words = ""{0}{2}"" {1}", Strings.Left(Trim(RadComboBox3.Text), 45), "<br />", "...")
        '    End If

        'End If

        'If idProcess > 0 Then
        '    sql &= String.Format(" AND ( id_documento = {0} )", idProcess)
        '    strSearch &= String.Format(" Proccess = ""{0}{2}"" {1}", Strings.Left(Trim(RadComboBox3.Text), 45), "<br />", "...")
        'End If


        'Dim datefilter As String = ""
        'If Me.chkfilterDate.Checked = True Then

        '    datefilter = " AND ( fecha_recepcion BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        '    strSearch &= String.Format(" Dates between ""{0:d}"" and ""{1:d}"" {2}", Me.txt_finicio.SelectedDate, Me.txt_ffin.SelectedDate, "<br />")

        'End If


        'If Me.RadioButton1.Checked = True Then



        '    sql &= String.Format("  AND ( " &
        '          "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({2}))) AND id_estadoDoc = 6 ) " &
        '          "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
        '          "      OR ( rol_owner in ({2}) AND id_estadoDoc = 1)  " &
        '          "      Or ( rol_owner in ({3}) AND id_estadoDoc = 1 AND idUserOwner = '0') )  ", Me.Session("E_IdUser"), lbl_GroupRolID.Text, lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)


        '    strSearch &= String.Format("Approval processes that is pending action by {0}{1}", varUSer, "<br />")


        'ElseIf Me.RadioButton2.Checked = True Then


        '    sql &= String.Format(" AND ( (id_estadoDoc in (1,6) ) " &
        '             " AND (   ( {0} = IdOriginador) OR ( IdRolOriginator in ({1}) )   ) ) ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text)


        '    strSearch &= String.Format("Approvals processes that has been initiated by {0}{1}", varUSer, "<br />")

        'ElseIf Me.RadioButton3.Checked = True Then


        '    sql &= String.Format("  And (   
        '                                ( {0} = IdOriginador And id_estadoDoc = 1 ) 
        '                                Or (  
        '                           ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
        '                          )
        '                                Or (
        '                            ( id_estadoDoc = 6 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{1}',',')) )
        '                           ) 
        '                                 Or (
        '                            ( id_estadoDoc = 1 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{2}',',')) )
        '                           ) 

        '                         )  ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)





        '    strSearch &= String.Format(" {0} has participated into the approval process{1}", varUSer, "<br />")

        'Else

        '    sql &= " AND ( id_estadoDoc = 1 OR id_estadoDoc = 6)"
        '    strSearch &= String.Format("All pending approval processes ", varUSer, "<br />")

        'End If

        'sql &= datefilter
        'sql &= " ORDER BY Fecha_Recepcion"


        'Dim ds As New DataSet("TotalRegistros")
        'Dim dm As New SqlDataAdapter(sql, cnnSAP)
        'dm.Fill(ds, "TotalRegistros")
        'Me.lbltotal.Text = "[ " & ds.Tables("TotalRegistros").Rows.Count & " ]" & If(ds.Tables("TotalRegistros").Rows.Count = 0, " approval process", " approval processes")


        'If Strings.Len(strSearch) > 0 Then

        '    strSearch = String.Format("...Searching by {0}", "<br />") & strSearch

        'Else

        '    strSearch = String.Format("...Searching all approvals processes {0}", "<br />") & strSearch

        'End If

        'lblt_Search.Text = strSearch

        'Return ds.Tables("TotalRegistros")

    End Function

    Private Function GetData(ByVal text As String) As DataTable

        'Dim result As Object
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim idUsr = Convert.ToInt32(Me.Session("E_IdUser"))

        'Using dbEntities As New dbRMS_JIEntities

        '    Try

        '        result = dbEntities.SP_TA_DOCUMENTOS_GR(id_programa, 1, idUsr, lbl_ALL_SIMPLE_RolID.ToString(), lbl_ALL_RolID.ToString()).ToList()


        '    Catch ex As Exception
        '        result = Nothing
        '    End Try


        'End Using

        'GetData = cl_utl.ConvertToDataTable(result)


        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************

        Dim strSearch As String = ""
        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim dtStar As Date = txt_finicio.SelectedDate
        Dim dtEnd As Date = txt_ffin.SelectedDate

        Dim bndDate As Integer = If(chkfilterDate.Checked, 1, 0)

        Dim bndALL As Integer = 0
        If rdb_allpending.Checked Then
            bndALL = 1
        End If

        '1 - -Pending
        '6 -- StandBy

        'Pending
        Dim tbl_Pending As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(0, 0, "", 0, "", 1, "", "", strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, bndALL, dtStar, dtEnd)
        'StandBy
        Dim tbl_StandBy As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(0, 0, "", 0, "", 6, "", "", strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, bndALL, dtStar, dtEnd)

        Dim tbl_search As DataTable = Nothing

        If tbl_Pending.Rows.Count > 0 Then

            tbl_search = tbl_Pending.Copy
            If tbl_StandBy.Rows.Count > 0 Then
                For Each dtRW In tbl_StandBy.Rows
                    tbl_search.Rows.Add(dtRW)
                Next
            End If

        Else

            If tbl_StandBy.Rows.Count > 0 Then
                tbl_search = tbl_StandBy.Copy
            End If

        End If


        Me.lbltotal.Text = "[ " & tbl_search.Rows.Count & " ]" & If(tbl_search.Rows.Count = 0, " approval process", " approval processes")

        If Strings.Len(strSearch) > 0 Then
            strSearch = String.Format("...Searching by {0}", "<br />") & strSearch
        Else
            strSearch = String.Format("...Searching all approvals processes {0}", "<br />") & strSearch
        End If

        lblt_Search.Text = strSearch

        Return tbl_search


        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************
        '--*************************************************************************************************************************************************************


        'Dim sql As String
        'Dim ds As New DataSet("registeres")

        'sql = String.Format("select a.id_documento, a.descripcion_doc, a.numero_instrumento, a.nom_beneficiario " &
        '                    "    from VW_GR_TA_DOCUMENTOS a " &
        '                    "        where  a.id_programa={1} " &
        '                    "  AND                               (a.descripcion_doc like '%{0}%' " &
        '                    "                                      or a.numero_instrumento like '%{0}%' " &
        '                    "                                        or a.nom_beneficiario like '%{0}%') ", Trim(text), Me.Session("E_IDPrograma"))

        'Dim datefilter As String = ""
        'If Me.chkfilterDate.Checked = True Then
        '    datefilter = " AND ( fecha_recepcion BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        'End If


        'If Me.RadioButton1.Checked = True Then


        '    sql &= String.Format("  AND ( " &
        '          "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({2}))) AND id_estadoDoc = 6 ) " &
        '          "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
        '          "      OR ( row_owner in ({2}) AND id_estadoDoc = 1)  " &
        '          "      OR ( rol_owner in ({3}) AND id_estadoDoc = 1 AND idUserOwner = '0')  )  ", Me.Session("E_IdUser"), lbl_GroupRolID.Text, lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)


        'ElseIf Me.RadioButton2.Checked = True Then  'Initiated by


        '    sql &= String.Format(" AND ( (id_estadoDoc in (1,6) ) " &
        '             " AND (   ( {0} = IdOriginador) OR ( IdRolOriginator in ({1}) )   ) ) ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text)


        'ElseIf Me.RadioButton3.Checked = True Then


        '    sql &= String.Format("  And (   
        '                                ( {0} = IdOriginador And id_estadoDoc = 1 ) 
        '                                Or (  
        '                           ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
        '                          )
        '                                Or (
        '                            ( id_estadoDoc = 6 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{1}',',')) )
        '                           ) 
        '                                Or (
        '                            ( id_estadoDoc = 1 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{2}',',')) )
        '                           ) 
        '                         )  ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)



        'Else

        '    sql &= " AND ( id_estadoDoc = 1 OR id_estadoDoc = 6)"

        'End If

        'sql &= datefilter

        'Dim adapter As New SqlDataAdapter(sql, cnnSAP)
        ''adapter.SelectCommand.Parameters.AddWithValue("@text", text)

        '' SessionDataSource1.SelectCommand = sql
        ''RadComboBox1.DataBind()

        'Dim data As New DataTable()
        'adapter.Fill(data)

        'Return data

    End Function

    Sub fillGrid()


        'Dim sql As String = "SELECT ROW_NUMBER() OVER (ORDER BY Fecha_Recepcion) AS [No], * FROM VW_GR_TA_DOCUMENTOS " & _
        '                    " WHERE id_proyecto=" & Me.Session("E_IdProy")

        'If Not String.IsNullOrEmpty(Trim(Me.txt_doc.Text)) Then
        '    sql &= String.Format(" AND ((descripcion_doc LIKE '%{0}%') OR (nom_beneficiario LIKE '%{0}%') OR (numero_instrumento LIKE '%{0}%')) ", Trim(Me.txt_doc.Text))
        'End If

        'Dim datefilter As String = ""
        'If Me.chkfilterDate.Checked = True Then
        '    datefilter = " AND ( fecha_recepcion BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        'End If

        'If Me.RadioButton1.Checked = True Then

        '    sql &= String.Format("  AND ( " & _
        '           "  ( {0} = IdOriginador AND id_estadoDoc = 6 ) " & _
        '           "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)) ", Me.Session("E_IdUser"))

        'ElseIf Me.RadioButton2.Checked = True Then

        '    sql &= String.Format(" AND ( (id_estadoDoc = 6 OR id_estadoDoc = 1 ) " & _
        '             " AND ( {0} = IdOriginador) ) ", Me.Session("E_IdUser"))

        'ElseIf Me.RadioButton3.Checked = True Then

        '    sql &= " AND ( " & Me.Session("E_IdUser") & " = IdOriginador AND id_estadoDoc = 1 )"
        '    sql &= String.Format("  OR ( " & _
        '                            " ( id_estadoDoc = 6 OR id_estadoDoc = 1 )  AND ( {0} in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')))) ", Me.Session("E_IdUser"))
        'Else

        '    sql &= " AND ( id_estadoDoc = 1 OR id_estadoDoc = 6)"

        'End If

        'sql &= datefilter
        'sql &= " ORDER BY Fecha_Recepcion"


        'Me.SqlDataSource2.SelectCommand = sql
        'Me.grd_cate.DataBind()
        Me.grd_cate.Rebind()

        'Dim ds As New DataSet("TotalRegistros")
        'Dim dm As New SqlDataAdapter(sql, cnnSAP)
        'dm.Fill(ds, "TotalRegistros")
        'Me.lbltotal.Text = "Total Rows: [ " & ds.Tables("TotalRegistros").Rows.Count & " ]"

    End Sub
    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    'Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
    '    Dim id_app = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_App_Documento").ToString()

    '    'Me.SqlDataSource2.DeleteCommand = "DELETE FROM ta_documento WHERE (id_documento = " & id_temp & ")"
    '    'Me.grd_cate.DataBind()
    'End Sub



    Protected Sub RadComboBox3_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) 'Handles RadComboBox3.SelectedIndexChanged



        If Val(RadComboBox3.SelectedValue) > 0 Then
            IDproceso = Val(RadComboBox3.SelectedValue)
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        Else
            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        End If

    End Sub

    Protected Sub RadComboBox3_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles RadComboBox3.ItemsRequested

        Dim data As DataTable = GetData(e.Text)

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
        e.EndOfItems = endOffset = data.Rows.Count

        RadComboBox3.DataSource = data
        RadComboBox3.DataBind()

        If Strings.Len(Trim(e.Text)) = 0 Then

            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso

        End If

        'For i As Integer = itemOffset To endOffset - 1
        '    RadComboBox3.Items.Add(New RadComboBoxItem(data.Rows(i)("id_documento").ToString(), data.Rows(i)("descripcion_doc").ToString()))
        'Next

        ' e.Message = GetStatusMessage(endOffset, data.Rows.Count)
        ' SessionDataSource1.SelectCommand = sql
        'RadComboBox1.DataBind()

    End Sub

    Protected Sub RadComboBox3_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_doc").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_documento").ToString()

    End Sub


    Protected Sub RadComboBox3_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(RadComboBox3.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(RadComboBox3.Items.Count)
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim estado As New Image
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'estado = CType(e.Item.FindControl("imgStatus"), Image)
            estado = CType(itemD("estado").FindControl("imgStatus"), Image)
            estado.ImageUrl = itemD("icon_msj").Text ' e.Item.Cells(16).Text
            estado.ToolTip = itemD("descripcion_estado").Text & " by " & itemD("propietario").Text  'e.Item.Cells(17).Text & " by " & e.Item.Cells(18).Text

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("Imprimir"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_seguimientoAprobacionRep.aspx?IdDoc=" & itemD("id_documento").Text & "&&IdRuta=" & itemD("id_ruta").Text

            Dim hlnkHistorial As HyperLink = New HyperLink
            hlnkHistorial = CType(e.Item.FindControl("verdocFlowchart"), HyperLink)
            hlnkHistorial.NavigateUrl = "frm_HistorialComentarios.aspx?IdDoc=" & itemD("id_documento").Text
            hlnkHistorial.Target = "_blank"
            hlnkHistorial.ToolTip = "See flowchart"
            hlnkHistorial.ImageUrl = "~/Imagenes/Iconos/view_tree.png"

            Dim hlnkComments As HyperLink = New HyperLink
            hlnkComments = CType(e.Item.FindControl("Historial"), HyperLink)
            hlnkComments.NavigateUrl = "~/approvals/frm_seguimientoNotificacion.aspx?IdDoc=" & itemD("id_documento").Text & "&&IdRuta=" & itemD("id_ruta").Text
            hlnkComments.Target = "_blank"
            hlnkComments.ToolTip = "Comments and background"
            hlnkComments.ImageUrl = "~/Imagenes/Iconos/observaciones.png"

            'Dim id_indicador = e.Item.Cells(3).Text.ToString
            'Dim hlnk1 As HyperLink = New HyperLink
            'hlnk1 = CType(e.Item.FindControl("tracking"), HyperLink)
            'hlnk1.NavigateUrl = "~/approvals/frm_seguimientoAprobacion.aspx?IdDoc=" & itemD("id_documento").Text
            'hlnk1.Target = "_blank"
            'hlnk1.ToolTip = "Tracking"

            Dim imagenCOmpleto As New Image

            imagenCOmpleto = CType(itemD("completo").FindControl("imgCompleto"), Image)
            imagenCOmpleto.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png"

            Dim aprobar As New HyperLink

            aprobar = CType(itemD("colm_approvals").FindControl("aprobar"), HyperLink)

            'Not applied to Stand By status
            Dim intOwner As String() = itemD("idUserOwner").Text.ToString.Split(",")
            'All The Owner USer id related in this step
            Dim boolOWN As Boolean = False

            ' Dim RolOwner As Integer = itemD("rol_owner").Text
            'lbl_ALL_SIMPLE_RolID.Text  'All Group Roles Just Simple Roles
            'lbl_ALL_RolID.Text  ' All Roles (Simple, Shared, Groups)
            'lbl_GroupRolID.Text  'Group Roles
            'lbl_RolID.Text  'Roles  (Simple and Shared)

            Dim intRolOwner As String() = lbl_ALL_SIMPLE_RolID.Text.Trim.ToString.Split(",") 'Groups and Simple Roles
            Dim intRolOwnerALL As String() = lbl_ALL_RolID.Text.Trim.ToString.Split(",") ' 'SHARED, SIMPLE AND GROUPS
            Dim intRolOwner_No_group As String() = lbl_RolID.Text.Trim.ToString.Split(",") ' 'SHARED AND SIMPLE ROLES

            If itemD("id_estadoDoc").Text = 1 Then

                'All The Owner USer id related in this step
                For Each idOw As String In intOwner

                    idOw = IIf(Not String.IsNullOrEmpty(idOw), idOw, "-2")
                    If Val(idOw) = 0 Then 'Represent a Share Role the step, compare the the role
                        For Each idR As String In intRolOwner_No_group
                            idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
                            If itemD("rol_owner").Text = idR Then
                                boolOWN = True
                                Exit For
                            End If
                        Next
                        'Find if this user can take a action
                    ElseIf (idOw = Me.Session("E_IdUser").ToString) Then   'Searching the user inside the owners 
                        boolOWN = True
                        Exit For
                    Else 'Search en All Simple Roles, Shared Roles and Group of any simple  Role
                        For Each idR As String In intRolOwner
                            idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
                            If itemD("rol_owner").Text = idR Then
                                boolOWN = True
                                Exit For
                            End If
                        Next
                    End If
                Next

                'For Each idOw As String In intRolOwnerALL

                '        idOw = IIf(Not String.IsNullOrEmpty(idOw), idOw, "-2")

                '        If Val(idOw) = 0 Then 'is a role the next owner

                '            For Each idR As String In intRolOwnerALL
                '                idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
                '                If itemD("rol_owner").Text = idR Then
                '                    boolOWN = True
                '                    Exit For
                '                End If
                '            Next

                '        Else

                '            If (idOw = Me.Session("E_IdUser").ToString And itemD("id_estadoDoc").Text = 1) Then   '  Pending Document
                '                boolOWN = True
                '            Else 'Search in all Roles, because of a Group of Roles
                '                '*************************************OJOJ**************************
                '                For Each idR As String In intRolOwner
                '                    idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
                '                    If itemD("rol_owner").Text = idR Then
                '                        boolOWN = True
                '                        Exit For
                '                    End If
                '                Next
                '                '*************************************OJOJ**************************
                '            End If

                '        End If

                '    Next


            ElseIf (itemD("id_estadoDoc").Text = 6) Then 'Stand By documents
                ' Stand By Document
                'This rule has tu change because once any user started the route has to redirect him
                'Check wif the IDrolOriginator could be Zero for the shared Roles
                'Here evaluate the id_user or the entire role

                If (itemD("idOriginador").Text = Me.Session("E_IdUser").ToString) Then 'As individual the una!!
                    boolOWN = True
                Else

                    'Search in all Roles into the Originator, because of a Group of Roles, and Shared roles

                    'Dim tbl_user_role As New DataTable
                    'tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser").ToString)
                    'getting all roles of the usser .logged to compare if have the originator role and know if this user could continue with the stand By

                    For Each idR As String In intRolOwner
                        idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
                        If itemD("idRolOriginator").Text = idR Then
                            boolOWN = True
                            Exit For
                        End If
                    Next

                End If

            End If

            If Not boolOWN Then
                aprobar.ImageUrl = "~/Imagenes/Iconos/Informacion2.png"
            Else
                aprobar.NavigateUrl = "~/approvals/frm_DocAprobacion.aspx?IdDoc=" & itemD("id_documento").Text  ' e.Item.Cells(3).Text
                aprobar.ImageUrl = "~/Imagenes/Iconos/accept.png"
                aprobar.Target = "_self"
            End If

        End If
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/approvals/frm_docsAD.aspx")
    End Sub

    Protected Sub btn_nuevo0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo0.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docs.aspx")
    End Sub

    Protected Sub btn_buscar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar0.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docsQuickSearch.aspx")
    End Sub

    Protected Sub btn_buscar1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar1.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docsAllApproved.aspx")
    End Sub

    Protected Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub RadioButton3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub btn_RadioButton4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_allpending.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub chkfilterDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkfilterDate.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub btn_nuevo2_Click(sender As Object, e As System.EventArgs) Handles btn_nuevo2.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docsByStates.aspx")
    End Sub


End Class
