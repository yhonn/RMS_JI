Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports ly_SIME

Partial Class frm_consulta_docs
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim varUSer As String
    Dim IDproceso As Integer
    Dim IDCategory As Integer
    Dim IDStatus As Integer
    Dim IDapproval As Integer
    Private Const ItemsPerRequest As Integer = 10
    Dim clss_approval As APPROVAL.clss_approval

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_SRCH_PROCC1"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim bndLoad As Boolean = False

    Sub fillcombo()

        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        'Me.cmb_cat.DataSource = cl_app_util.get_ta_categoriaBy_User(Me.Session("E_IdUser"))
        Me.cmb_cat.DataSource = cl_app_util.get_ta_categoria()
        Me.cmb_cat.DataBind()

        'Me.cmb_app.DataSource = cl_app_util.get_ta_approvalBy_User(Me.Session("E_IdUser"))
        Me.cmb_app.DataSource = cl_app_util.get_ta_ApprovalBy_Category(Val(Me.cmb_cat.SelectedValue))
        Me.cmb_app.DataBind()

        Me.cmb_Status.DataSource = cl_app_util.get_ta_estadoDocumento()
        Me.cmb_Status.DataBind()

    End Sub


    Sub fillcomboApp(Optional ByVal idOPT As Integer = 0)

        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim sql As String
        Dim idCat As Integer = 0

        If idOPT = -1 Then
            idCat = 0
            Me.cmb_app.DataSource = ""
        Else

            idCat = Me.cmb_cat.SelectedValue
            'Me.cmb_app.DataSource = cl_app_util.get_ta_approvalBy_User(Me.Session("E_IdUser"), idCat)
            Me.cmb_app.DataSource = cl_app_util.get_ta_ApprovalBy_Category(idCat)
            Me.cmb_app.DataBind()

        End If

        cmb_app.SelectedValue = ""
        cmb_app.Text = ""

    End Sub


    Sub actualiza()
        Try

            If Val(Me.cmb_app.SelectedValue) > 0 Then

                Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))
                Dim tbl_Doc As DataTable = cl_app_util.get_ta_documento(Me.cmb_app.SelectedValue)

                If tbl_Doc.Rows.Count > 0 Then
                    Me.lblt_conditions.Text = tbl_Doc.Rows(0).Item("condicion").ToString()
                Else
                    Me.lblt_conditions.Text = "N/A"
                End If

            Else
                Me.lblt_conditions.Text = "N/A"
            End If

        Catch ex As Exception
            Me.lblt_conditions.Text = ex.Message
        End Try

    End Sub
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

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



            Dim currentdate As Date = Date.Today
            Me.txt_finicio.SelectedDate = DateSerial(Year(Today()), Month(Today()), 1)
            Dim diaMax = Date.DaysInMonth(currentdate.Year, currentdate.Month)
            Me.txt_ffin.SelectedDate = DateSerial(Year(Today()), Month(DateAdd("m", 1, Today())), 0)

            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso

            IDStatus = 0
            Me.Session("E_bnd_var_05") = IDStatus

            IDCategory = 0
            Me.Session("E_bnd_var_01") = IDCategory

            IDapproval = 0
            Me.Session("E_bnd_var_04") = IDapproval

            Me.Session("E_bnd_var_03") = Session("E_Nombre")

            fillcombo()

            actualiza()

            grd_cate.PagerStyle.Position = GridPagerPosition.Top

            'If Me.cmb_app.IsEmpty Then
            '    Me.btn_buscar.Visible = False
            'End If

            bndLoad = True

            HttpContext.Current.Session.Add("clss_approval", clss_approval)

        Else

            If HttpContext.Current.Session.Item("clss_approval") IsNot Nothing Then
                clss_approval = Me.Session.Item("clss_approval")
            End If

            IDCategory = Me.Session("E_bnd_var_01")
            IDproceso = Me.Session("E_bnd_var_02")
            varUSer = Me.Session("E_bnd_var_03")
            IDapproval = Me.Session("E_bnd_var_01")
            IDStatus = Me.Session("E_bnd_var_05")

            bndLoad = False

        End If

    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource

        Me.grd_cate.DataSource = getData(0, IDproceso)

    End Sub

    Public Function getData(Optional ByVal idEstatus As Integer = 0, Optional ByVal idProcess As Integer = 0) As DataTable

        Dim strSearch As String = ""
        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim dtStar As Date = txt_finicio.SelectedDate
        Dim dtEnd As Date = txt_ffin.SelectedDate

        Dim bndDate As Integer = If(chkfilterDate.Checked, 1, 0)

        Dim tbl_search As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(idProcess, Val(cmb_cat.SelectedValue), cmb_cat.Text, Val(cmb_app.SelectedValue), cmb_app.Text, Val(cmb_Status.SelectedValue), cmb_Status.Text, rdKeyWord.Text, strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, Val(h_Filter.Value), dtStar, dtEnd)

        Me.lbltotal.Text = "[ " & tbl_search.Rows.Count & " ]" & If(tbl_search.Rows.Count = 0, " Procesos de aprobación", " Procesos de aprobación")

        If Strings.Len(strSearch) > 0 Then
            strSearch = String.Format("...Searching by {0}", "<br />") & strSearch
        Else
            strSearch = String.Format("...Buscando procesos de aprobación {0}", "<br />") & strSearch
        End If

        lblSearch.Text = strSearch

        Return tbl_search

    End Function

    Private Function GetData(ByVal text As String) As DataTable

        'Dim sql As String
        'Dim ds As New DataSet("registeres")


        'sql = String.Format("select a.id_documento, a.descripcion_doc, a.numero_instrumento, a.nom_beneficiario " &
        '                    "    from VW_GR_TA_DOCUMENTOS a " &
        '                    "        where  a.id_programa={1} " &
        '                    "  AND                               (a.descripcion_doc like '{0}' " &
        '                    "                                      or a.numero_instrumento like '{0}' " &
        '                    "                                        or a.nom_beneficiario like '{0}') ", Trim(text), Me.Session("E_IDPrograma"))
        ''--New Query

        'sql &= String.Format("And ( 

        '                      ( --Pending Status 

        '                        (  {0} = IdOriginador And id_estadoDoc = 1 ) 

        '                               Or (  
        '                               ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
        '                                )


        '                                Or (  --StandBy Status
        '                                  ( id_estadoDoc = 6 ) And (  

        '                            (select count(*) as N
        '                             from 
        '                              (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
        '                             inner join (select part from dbo.SDF_SplitString('{1}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
        '                            )
        '                           )	--Or		


        '                                 Or (   --Pending Status
        '                                   ( id_estadoDoc = 1 ) And (  

        '                            (select count(*) as N
        '                             from 
        '                              (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
        '                             inner join (select part from dbo.SDF_SplitString('{1}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
        '                            )

        '                           )  --Or 

        '                          )  --Pending Status                                    


        '                          --Closed Status
        '                      OR ( id_estadoDoc not in (1,6)  --Chages
        '                          And (  	                                      
        '                             (  (1) in (select * from dbo.SDF_SplitString(IdUserPArticipate,','))  )                                      
        '                             Or  (  				
        '		                            (select count(*) as N
        '		                             from 
        '		                              (select part from dbo.SDF_SplitString(IdRolePArticipate,',')) as tab1
        '			                            inner join (select part from dbo.SDF_SplitString('{2}',',')) as tab2 on (tab1.part = tab2.part)) >= 1
        '		                            )
        '                         )  --And
        '                       ) --OR


        '                       --OR
        '                        --New condition to showing all register approval
        '                            OR (
        '                              -----New condition to showing all register approval
        '            ---Bnd All Approvals
        '            {3} = 1

        '              )  

        '                        ) --AND  ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, Val(h_Filter.Value))


        'Dim datefilter As String = ""
        'If Me.chkfilterDate.Checked = True Then
        '    datefilter = " AND ( fecha_recepcion BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        'End If


        'If Val(cmb_cat.SelectedValue) > 0 Then
        '    sql &= String.Format(" AND ( id_categoria = {0} )", Val(cmb_cat.SelectedValue))
        'End If

        'If Val(cmb_app.SelectedValue) > 0 Then
        '    sql &= String.Format(" AND ( id_tipoDocumento = {0} )", Val(cmb_app.SelectedValue))
        'End If

        'If Val(cmb_Status.SelectedValue) > 0 Then
        '    sql &= String.Format(" AND ( id_estadoDoc = {0} )", Val(cmb_Status.SelectedValue))
        'End If

        'If IDproceso > 0 Then
        '    sql &= String.Format(" AND ( id_documento = {0} )", IDproceso)
        'End If

        'sql &= datefilter

        'Dim adapter As New SqlDataAdapter(sql, cnnSAP)


        'Dim data As New DataTable()
        'adapter.Fill(data)

        Dim strSearch As String = ""
        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim dtStar As Date = txt_finicio.SelectedDate
        Dim dtEnd As Date = txt_ffin.SelectedDate

        Dim bndDate As Integer = If(chkfilterDate.Checked, 1, 0)

        Dim tbl_search As DataTable = cl_app_util.get_Build_documentsBy_Type_Query(IDproceso, Val(cmb_cat.SelectedValue), cmb_cat.Text, Val(cmb_app.SelectedValue), cmb_app.Text, Val(cmb_Status.SelectedValue), cmb_Status.Text, text, strSearch, bndDate, Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text, Val(h_Filter.Value), dtStar, dtEnd)

        'Me.lbltotal.Text = "[ " & tbl_search.Rows.Count & " ]" & If(tbl_search.Rows.Count = 0, " approval process", " approval processes")

        'If Strings.Len(strSearch) > 0 Then
        '    strSearch = String.Format("...Searching by {0}", "<br />") & strSearch
        'Else
        '    strSearch = String.Format("...Searching all approvals processes {0}", "<br />") & strSearch
        'End If

        'lblSearch.Text = strSearch


        GetData = tbl_search





    End Function

    Protected Sub rdKeyWord_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) 'Handles rdKeyWord.SelectedIndexChanged

        If Val(rdKeyWord.SelectedValue) > 0 Then
            IDproceso = Val(rdKeyWord.SelectedValue)
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        Else
            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        End If

    End Sub

    Protected Sub rdKeyWord_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles rdKeyWord.ItemsRequested

        Dim strphrase As String = e.Text
        Dim strWords() As String = strphrase.Split(" ")
        Dim strSearch As String = "%"
        Dim bnd_exec As Boolean = False

        If strWords.Count > 0 Then
            For Each strw As String In strWords
                If strw.Trim.Length > 1 Then
                    strSearch &= strw & "%"
                End If
            Next
            If strSearch.Trim.Length > 1 Then
                bnd_exec = True
            End If
        End If

        lblHidden_search.Text = String.Format("Value of search: {0} ", strSearch)

        'We just gonna find over completed words without one letter articules
        If bnd_exec Then

            Dim data As DataTable = GetData(strSearch)

            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
            e.EndOfItems = endOffset = data.Rows.Count

            rdKeyWord.DataSource = data
            rdKeyWord.DataBind()

            If Strings.Len(Trim(e.Text)) = 0 Then
                IDproceso = 0
                Me.Session("E_bnd_var_02") = IDproceso
            End If

        End If

    End Sub

    Protected Sub rdKeyWord_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_doc").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_documento").ToString()

    End Sub

    Protected Sub rdKeyWord_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(rdKeyWord.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(rdKeyWord.Items.Count)
    End Sub

    Sub fillGrid()
        Me.grd_cate.Rebind()
    End Sub
    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim estado As New Image
            estado = CType(e.Item.FindControl("imgStatus"), Image)
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'cnnSAP.Open()
            'Dim SQL = "SELECT MAX(id_App_Documento) FROM vw_ta_AppDocumento WHERE id_documento=" & itemD("id_documento").Text
            'Dim dm1 As New SqlDataAdapter(SQL, cnnSAP)
            'Dim ds1 As New DataSet("ruta")
            'dm1.Fill(ds1, "ruta")
            'Dim id_ruta = ds1.Tables("ruta").Rows(0).Item(0).ToString

            'SQL = "SELECT * FROM vw_ta_AppDocumento WHERE id_App_Documento=" & id_ruta
            'ds1.Tables.Add("ruta2")
            'dm1.SelectCommand.CommandText = SQL
            'dm1.SelectCommand.ExecuteNonQuery()
            'dm1.Fill(ds1, "ruta2")
            'estado.ImageUrl = ds1.Tables("ruta2").Rows(0).Item("icon_msj").ToString
            'estado.ToolTip = ds1.Tables("ruta2").Rows(0).Item("descripcion_estado").ToString & " by " & ds1.Tables("ruta2").Rows(0).Item("nombre_rol").ToString
            'cnnSAP.Close()

            estado.ImageUrl = itemD("icon_msj").Text
            estado.ToolTip = itemD("descripcion_estado").Text & " by " & itemD("propietario").Text

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("Imprimir"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_seguimientoAprobacionMessRep.aspx?IdDoc=" & itemD("id_documento").Text & "&&IdRuta=" & itemD("id_ruta").Text

            Dim hlnkHistorial As HyperLink = New HyperLink
            hlnkHistorial = CType(e.Item.FindControl("verdocFlowchart"), HyperLink)
            hlnkHistorial.NavigateUrl = "frm_HistorialComentarios.aspx?IdDoc=" & itemD("id_documento").Text
            hlnkHistorial.Target = "_blank"
            hlnkHistorial.ToolTip = "See flowchart"
            hlnkHistorial.ImageUrl = "~/Imagenes/iconos/view_tree.png"

            Dim hlnkComments As HyperLink = New HyperLink
            hlnkComments = CType(e.Item.FindControl("Historial"), HyperLink)
            hlnkComments.NavigateUrl = "frm_seguimientoNotificacion.aspx?IdDoc=" & itemD("id_documento").Text & "&&IdRuta=" & itemD("id_ruta").Text
            hlnkComments.Target = "_blank"
            hlnkComments.ToolTip = "Comments and background"
            hlnkComments.ImageUrl = "~/Imagenes/iconos/observaciones.png"

            'Dim id_indicador = itemD("id_documento").Text.ToString
            'Dim hlnk1 As HyperLink = New HyperLink
            'hlnk1 = CType(e.Item.FindControl("tracking"), HyperLink)
            'hlnk1.NavigateUrl = "frm_seguimientoAprobacion.aspx?IdDoc=" & itemD("id_documento").Text
            'hlnk1.Target = "_blank"
            'hlnk1.ToolTip = "Tracking"

            Dim imagenCOmpleto As New Image
            imagenCOmpleto = CType(e.Item.FindControl("imgCompleto"), Image)
            imagenCOmpleto.ImageUrl = "~/Imagenes/iconos/Circle_" & Trim(itemD("Alerta").Text) & ".png"

            Dim aprobar As New HyperLink
            aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

            'Not applied to Stand By status
            Dim intOwner As String() = itemD("idUserOwner").Text.ToString.Split(",")
            'All The Owner USer id related in this step
            Dim boolOWN As Boolean = False

            ' Dim RolOwner As Integer = itemD("rol_owner").Text
            'lbl_ALL_SIMPLE_RolID.Text  'All Group Roles Just Simple Roles
            'lbl_ALL_RolID.Text  ' All Roles (Simple, Shared, Groups)
            'lbl_GroupRolID.Text  'Group Roles
            'lbl_RolID.Text  'Roles  (Simple and Shared)

            Dim bndCompleted As Boolean = False
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

            Else
                bndCompleted = True
            End If


            If Not boolOWN Then
                If Not bndCompleted Then
                    aprobar.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
                    aprobar.ToolTip = "Not allowed to approve"
                Else
                    aprobar.ToolTip = ""
                End If
            Else
                aprobar.NavigateUrl = "frm_DocAprobacion.aspx?IdDoc=" & itemD("id_documento").Text
                aprobar.ImageUrl = "~/Imagenes/iconos/accept.png"
                aprobar.Target = "_self"
                aprobar.ToolTip = "Approvals"
            End If

        End If


    End Sub

    'Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
    '    Me.Response.Redirect("~~/approvals/frm_docsAD.aspx")
    'End Sub

    Protected Sub chkfilterDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkfilterDate.CheckedChanged
        fillGrid()
    End Sub

    Protected Sub cmb_cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) 'Handles cmb_cat.SelectedIndexChanged

        If Val(cmb_cat.SelectedValue) > 0 Then

            IDCategory = Val(cmb_cat.SelectedValue)
            Me.Session("E_bnd_var_01") = IDCategory
            chk_All_Categories.Checked = False

            fillcomboApp()
            actualiza()
            fillGrid()

        Else

            IDCategory = 0
            Me.Session("E_bnd_var_01") = IDCategory
            fillcomboApp(-1)
            actualiza()
            fillGrid()

        End If


    End Sub


    Protected Sub cmb_cat_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles cmb_cat.ItemDataBound
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_cat").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_categoria").ToString()
    End Sub

    Protected Sub cmb_app_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles cmb_app.ItemDataBound
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_aprobacion").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_tipoDocumento").ToString()
    End Sub

    Protected Sub cmb_cat_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

        If Strings.Len(Trim(e.Text)) = 0 Then
            IDCategory = 0
            Me.Session("E_bnd_var_01") = IDCategory
        End If

    End Sub

    Protected Sub cmb_app_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

        If Strings.Len(Trim(e.Text)) = 0 Then
            IDapproval = 0
            Me.Session("E_bnd_var_04") = IDapproval
        End If

    End Sub


    Protected Sub cmb_app_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) 'Handles cmb_app.SelectedIndexChanged


        If Val(cmb_app.SelectedValue) > 0 Then

            IDapproval = Val(cmb_app.SelectedValue)
            Me.Session("E_bnd_var_04") = IDapproval
            chk_all_app.Checked = False

            actualiza()
            fillGrid()

        Else

            If IDapproval > 0 Then

                IDapproval = 0
                Me.Session("E_bnd_var_04") = IDapproval
                actualiza()
                fillGrid()

            End If


        End If

    End Sub

    Protected Sub btn_buscar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_return.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docsPending.aspx")
    End Sub

    Private Sub chk_All_Categories_CheckedChanged(sender As Object, e As EventArgs) Handles chk_All_Categories.CheckedChanged
        If chk_All_Categories.Checked = True Then
            Me.cmb_cat.SelectedValue = ""
            Me.cmb_cat.Text = ""

            IDCategory = 0
            Me.Session("E_bnd_var_01") = IDCategory
            fillcomboApp(-1)
            actualiza()
            fillGrid()

        End If
    End Sub

    Private Sub chk_all_app_CheckedChanged(sender As Object, e As EventArgs) Handles chk_all_app.CheckedChanged
        If chk_all_app.Checked = True Then

            Me.cmb_app.SelectedValue = ""
            Me.cmb_app.Text = ""
            IDapproval = 0

            Me.Session("E_bnd_var_04") = IDapproval
            actualiza()
            fillGrid()

        End If
    End Sub

    Protected Sub cmb_Status_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)


        If Val(cmb_Status.SelectedValue) > 0 Then

            IDStatus = Val(cmb_Status.SelectedValue)
            Me.Session("E_bnd_var_05") = IDStatus
            chk_allstatus.Checked = False

            fillGrid()

        Else

            IDStatus = 0
            Me.Session("E_bnd_var_05") = IDStatus
            fillGrid()

        End If

    End Sub

    Private Sub chk_allstatus_CheckedChanged(sender As Object, e As EventArgs) Handles chk_allstatus.CheckedChanged

        If chk_allstatus.Checked = True Then
            Me.cmb_Status.SelectedValue = ""
            Me.cmb_Status.Text = ""

            IDStatus = 0
            Me.Session("E_bnd_var_05") = IDStatus
            fillGrid()

        End If
    End Sub
End Class
