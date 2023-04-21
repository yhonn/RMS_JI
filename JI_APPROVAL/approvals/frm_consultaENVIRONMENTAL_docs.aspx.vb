Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_consultaENVIRONMENTAL_docs

    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim varUSer As String
    Dim IDproceso As Integer
    Dim IDCategory As Integer
    Dim IDapproval As Integer
    Dim ID_TpReview As Integer
    Private Const ItemsPerRequest As Integer = 10

    Dim clss_approval As APPROVAL.clss_approval
    Dim cl_envir_app As APPROVAL.cl_environmentalAPP

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_SRCH_ENV_PROCC"
    Dim controles As New ly_SIME.CORE.cls_controles

    'Sub fillcombo()

    'Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))
    'Me.cmb_cat.DataSource = cl_app_util.get_ta_categoriaBy_User(Me.Session("E_IdUser")) 'clss_approval.get_CategoryUser(cmb_rol.SelectedValue) just to a user especifed
    'Me.cmb_cat.DataBind()
    'Me.cmb_app.DataSource = cl_app_util.get_ta_approvalBy_User(Me.Session("E_IdUser"))
    'Me.cmb_app.DataBind()

    'End Sub


    'Sub fillcomboApp(Optional ByVal idOPT As Integer = 0)

    '    Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

    '    Dim sql As String
    '    Dim idCat As Integer = 0
    '    If idOPT = -1 Then
    '        idCat = 0
    '    Else
    '        idCat = Me.cmb_cat.SelectedValue

    '        Me.cmb_app.DataSource = cl_app_util.get_ta_approvalBy_User(Me.Session("E_IdUser"), idCat)
    '        Me.cmb_app.DataBind()

    '    End If


    '    cmb_app.SelectedValue = ""
    '    cmb_app.Text = ""


    'End Sub


    'Sub actualiza()
    '    Try

    '        If Val(Me.cmb_app.SelectedValue) > 0 Then

    '            Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))
    '            Dim tbl_Doc As DataTable = cl_app_util.get_ta_documento(Me.cmb_app.SelectedValue)

    '            If tbl_Doc.Rows.Count > 0 Then
    '                Me.lblt_conditions.Text = tbl_Doc.Rows(0).Item("condicion").ToString()
    '            Else
    '                Me.lblt_conditions.Text = "N/A"
    '            End If

    '        Else
    '            Me.lblt_conditions.Text = "N/A"
    '        End If

    '    Catch ex As Exception
    '        Me.lblt_conditions.Text = ex.Message
    '    End Try

    'End Sub


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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            cmb_rev_type.SelectedIndex = -1

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

            'IDCategory = 0
            'Me.Session("E_bnd_var_01") = IDCategory

            ID_TpReview = 0
            Me.Session("E_bnd_var_01") = ID_TpReview

            IDapproval = 0
            Me.Session("E_bnd_var_04") = IDapproval

            Me.Session("E_bnd_var_03") = Session("E_Nombre")

            'fillcombo()
            'actualiza()

            grd_cate.PagerStyle.Position = GridPagerPosition.Top

            'If Me.cmb_app.IsEmpty Then
            '    Me.btn_buscar.Visible = False
            'End If

            HttpContext.Current.Session.Add("clss_approval", clss_approval)

        Else

            If HttpContext.Current.Session.Item("clss_approval") IsNot Nothing Then
                clss_approval = Me.Session.Item("clss_approval")
            End If

            'IDCategory = Me.Session("E_bnd_var_01")
            ID_TpReview = Me.Session("E_bnd_var_01")
            IDproceso = Me.Session("E_bnd_var_02")
            varUSer = Me.Session("E_bnd_var_03")
            IDapproval = Me.Session("E_bnd_var_01")

        End If

    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource

        Me.grd_cate.DataSource = getData(IDproceso)

    End Sub

    Public Function getData(Optional ByVal idProcess As Integer = 0) As DataTable

        Dim strSearch As String = ""
        Dim cl_app_util As New APPROVAL.cl_approval_util(Me.Session("E_IDPrograma"))

        Dim id_review As Integer = -1
        If Not String.IsNullOrEmpty(cmb_rev_type.SelectedValue) Then
            id_review = cmb_rev_type.SelectedValue
        End If

        Dim tbl_search As DataTable = cl_app_util.get_Build_EnvironmentalDocs_Query(idProcess, id_review, cmb_rev_type.Text, rdKeyWord.Text, strSearch, False)

        Me.lbltotal.Text = "[ " & tbl_search.Rows.Count & " ]" & If(tbl_search.Rows.Count = 0, " approval process", " approval processes")

        If Strings.Len(strSearch) > 0 Then
            strSearch = String.Format("...Searching by {0}", "<br />") & strSearch
        Else
            strSearch = String.Format("...Searching all Environmentals approvals {0}", "<br />") & strSearch
        End If

        lblSearch.Text = strSearch

        Return tbl_search

    End Function

    Private Function GetData(ByVal text As String) As DataTable

        Dim ds As New DataSet("registeres")
        Dim sql As String = String.Format("select  ROW_NUMBER() OVER (ORDER BY Fecha_Aprobado) AS [No], * " &
                                          "      from vw_t_documento_ambiental WHERE id_programa={0} " &
                                          " AND ( (observacion like '{1}') OR (usuario_creo like '{1}') OR (numero_instrumento like '{1}') OR (nom_beneficiario like '{1}'))", Me.Session("E_IdPrograma"), Trim(text))

        Dim datefilter As String = ""
        If Me.chkfilterDate.Checked = True Then
            datefilter = " And ( fecha_aprobado BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        End If

        Dim id_review As Integer = -1
        If Not String.IsNullOrEmpty(cmb_rev_type.SelectedValue) Then
            id_review = cmb_rev_type.SelectedValue
        End If

        If id_review > -1 Then
            sql &= String.Format(" AND ( id_TipoApp_Environmental = {0} )", Val(cmb_rev_type.SelectedValue))
        End If

        If IDproceso > 0 Then
            sql &= String.Format(" AND ( id_documento_ambiental = {0} )", IDproceso)
        End If

        sql &= datefilter

        Dim adapter As New SqlDataAdapter(sql, cnnSAP)


        Dim data As New DataTable()
        adapter.Fill(data)

        Return data

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

        If bnd_exec Then

            Dim data As DataTable = GetData(strSearch)

            Dim itemOffset As Integer = e.NumberOfItems
            Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
            e.EndOfItems = endOffset = data.Rows.Count

            rdKeyWord.DataSource = data
            rdKeyWord.DataBind()

            If Strings.Len(Trim(strSearch)) = 0 Then

                IDproceso = 0
                Me.Session("E_bnd_var_02") = IDproceso

            End If

        End If

    End Sub

    Protected Sub rdKeyWord_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("observacion").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_documento_ambiental").ToString()

    End Sub


    Protected Sub rdKeyWord_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(rdKeyWord.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(rdKeyWord.Items.Count)
    End Sub
    Sub fillGrid()
        'Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total Rows: [ " & Me.grd_cate.Items.Count & " ]"
        Me.grd_cate.Rebind()
    End Sub
    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        'Dim sql = ""
        'Dim datefilter As String = ""
        'If Me.chkfilterDate.Checked = True Then
        '    datefilter = " AND fecha_recepcion BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "')"
        'End If
        'sql = "SELECT * FROM vw_ta_documentos WHERE ( (numero_instrumento LIKE '%" & Me.txt_doc.Text & "%') OR (descripcion_doc LIKE '%" & Me.txt_doc.Text & "%') OR (nom_beneficiario LIKE '%" & Me.txt_doc.Text & "%')) AND (id_tipoDocumento = " & Me.cmb_app.SelectedValue & ")" + datefilter
        'sql &= " ORDER BY Fecha_Recepcion"
        'Me.SqlDataSource2.SelectCommand = sql
        fillGrid()
    End Sub

    'Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
    '    Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_documento").ToString()
    '    Me.SqlDataSource2.DeleteCommand = "DELETE FROM ta_documento WHERE (id_documento = " & id_temp & ")"
    '    Me.grd_cate.DataBind()
    'End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            'Dim estado As New Image
            'estado = CType(e.Item.FindControl("imgStatus"), Image)
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'estado.ImageUrl = itemD("icon_msj").Text
            'estado.ToolTip = itemD("descripcion_estado").Text & " by " & itemD("propietario").Text

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("Imprimir"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_seguimientoAprobacionEnvironmental.aspx?IdDoc=" & itemD("id_documento_ambiental").Text
            hlnkPrint.Target = "_blank"
            hlnkPrint.ToolTip = "Print"

            Dim hlnkHistorial As HyperLink = New HyperLink
            hlnkHistorial = CType(e.Item.FindControl("verdocFlowchart"), HyperLink)
            hlnkHistorial.NavigateUrl = "frm_EnvironmentalApprovals.aspx?IdDoc=" & itemD("id_documento_ambiental").Text
            hlnkHistorial.Target = "_blank"
            hlnkHistorial.ToolTip = "Detail"
            hlnkHistorial.ImageUrl = "~/Imagenes/iconos/view_tree.png"

            'Dim hlnkComments As HyperLink = New HyperLink
            'hlnkComments = CType(e.Item.FindControl("Historial"), HyperLink)
            'hlnkComments.NavigateUrl = "frm_seguimientoNotificacion.aspx?IdDoc=" & itemD("id_documento").Text & "&&IdRuta=" & itemD("id_ruta").Text
            'hlnkComments.Target = "_blank"
            'hlnkComments.ToolTip = "Comments and background"
            'hlnkComments.ImageUrl = "~/Imagenes/iconos/observaciones.png"

            'Dim id_indicador = itemD("id_documento").Text.ToString
            'Dim hlnk1 As HyperLink = New HyperLink
            'hlnk1 = CType(e.Item.FindControl("tracking"), HyperLink)
            'hlnk1.NavigateUrl = "frm_seguimientoAprobacion.aspx?IdDoc=" & itemD("id_documento").Text
            'hlnk1.Target = "_blank"
            'hlnk1.ToolTip = "Tracking"

            'Dim imagenCOmpleto As New Image
            'imagenCOmpleto = CType(e.Item.FindControl("imgCompleto"), Image)

            'imagenCOmpleto.ImageUrl = "~/Imagenes/iconos/Circle_" & Trim(itemD("Alerta").Text) & ".png"

            'Dim aprobar As New HyperLink
            'aprobar = CType(e.Item.FindControl("aprobar"), HyperLink)

            'Dim intOwner As String() = itemD("idUserOwner").Text.ToString.Split(",")
            'Dim boolOWN As Boolean = False
            'For Each idOw As String In intOwner

            '    idOw = IIf(Not String.IsNullOrEmpty(idOw), idOw, "-1")

            '    If (idOw = Me.Session("E_IdUser").ToString And itemD("id_estadoDoc").Text = 1) Then
            '        boolOWN = True
            '    ElseIf (itemD("idOriginador").Text = Me.Session("E_IdUser").ToString And itemD("id_estadoDoc").Text = 6) Then
            '        boolOWN = True
            '    End If
            'Next

            ''Not applied to Stand By status
            'Dim intOwner As String() = itemD("idUserOwner").Text.ToString.Split(",")
            'Dim boolOWN As Boolean = False

            '' Dim RolOwner As Integer = itemD("rol_owner").Text
            ''lbl_ALL_SIMPLE_RolID.Text  'All Group Roles Just Simple Roles
            ''lbl_ALL_RolID.Text  ' All Roles (Simple, Shared, Groups)
            ''lbl_GroupRolID.Text  'Group Roles
            ''lbl_RolID.Text  'Roles  (Simple and Shared)

            'Dim intRolOwner As String() = lbl_ALL_SIMPLE_RolID.Text.Trim.ToString.Split(",")
            'Dim intRolOwnerALL As String() = lbl_ALL_RolID.Text.Trim.ToString.Split(",")

            'If itemD("id_estadoDoc").Text = 1 Then

            '    For Each idOw As String In intRolOwnerALL

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
            '                For Each idR As String In intRolOwner
            '                    idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
            '                    If itemD("rol_owner").Text = idR Then
            '                        boolOWN = True
            '                        Exit For
            '                    End If
            '                Next
            '            End If

            '        End If

            '    Next


            'ElseIf (itemD("id_estadoDoc").Text = 6) Then 'Stand By documents
            '    ' Stand By Document

            '    'Check wif the IDrolOriginator could be Zero for the shared Roles
            '    'Here evaluate the id_user or the entire role
            '    If (itemD("idOriginador").Text = Me.Session("E_IdUser").ToString) Then 'As individual the una!!
            '        boolOWN = True
            '    Else
            '        'Search in all Roles into the Originator, because of a Group of Roles, and Shared roles

            '        'Dim tbl_user_role As New DataTable
            '        'tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser").ToString)
            '        'getting all roles of the usser .logged to compare if have the originator role and know if this user could continue with the stand By

            '        For Each idR As String In intRolOwner
            '            idR = IIf(Not String.IsNullOrEmpty(idR), idR, "-1")
            '            If itemD("idRolOriginator").Text = idR Then
            '                boolOWN = True
            '                Exit For
            '            End If
            '        Next

            '    End If

            'End If



            'If Not boolOWN Then
            '    aprobar.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            'Else
            '    aprobar.NavigateUrl = "~~/approvals/frm_DocAprobacion.aspx?IdDoc=" & itemD("id_documento").Text
            '    aprobar.ImageUrl = "~/Imagenes/iconos/accept.png"
            '    aprobar.Target = "_self"
            'End If

        End If


    End Sub

    'Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
    '    Me.Response.Redirect("~~/approvals/frm_docsAD.aspx")
    'End Sub

    Protected Sub chkfilterDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkfilterDate.CheckedChanged
        fillGrid()
    End Sub

    'Protected Sub cmb_cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) 'Handles cmb_cat.SelectedIndexChanged

    '    If Val(cmb_cat.SelectedValue) > 0 Then

    '        IDCategory = Val(cmb_cat.SelectedValue)
    '        Me.Session("E_bnd_var_01") = IDCategory

    '        'fillcomboApp()
    '        'actualiza()
    '        fillGrid()

    '    Else

    '        IDCategory = 0
    '        Me.Session("E_bnd_var_01") = IDCategory
    '        ' fillcomboApp(-1)
    '        'actualiza()
    '        fillGrid()

    '    End If


    'End Sub


    'Protected Sub cmb_cat_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles cmb_cat.ItemDataBound
    '    e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_cat").ToString()
    '    e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_categoria").ToString()
    'End Sub

    'Protected Sub cmb_app_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles cmb_app.ItemDataBound
    '    e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("descripcion_aprobacion").ToString()
    '    e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_tipoDocumento").ToString()
    'End Sub

    'Protected Sub cmb_cat_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

    '    If Strings.Len(Trim(e.Text)) = 0 Then
    '        IDCategory = 0
    '        Me.Session("E_bnd_var_01") = IDCategory
    '    End If

    'End Sub

    'Protected Sub cmb_app_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

    '    If Strings.Len(Trim(e.Text)) = 0 Then
    '        IDapproval = 0
    '        Me.Session("E_bnd_var_04") = IDapproval
    '    End If

    'End Sub

    'Protected Sub cmb_app_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) 'Handles cmb_app.SelectedIndexChanged

    '    If Val(cmb_app.SelectedValue) > 0 Then

    '        IDapproval = Val(cmb_app.SelectedValue)
    '        Me.Session("E_bnd_var_04") = IDapproval

    '        'actualiza()
    '        fillGrid()

    '    Else

    '        If IDapproval > 0 Then

    '            IDapproval = 0
    '            Me.Session("E_bnd_var_04") = IDapproval
    '            ' actualiza()
    '            fillGrid()

    '        End If


    '    End If

    'End Sub

    Protected Sub btn_buscar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_return.Click
        Me.Response.Redirect("~/approvals/frm_consulta_docsPending.aspx")
    End Sub

    Protected Sub cmb_rev_type_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_rev_type.SelectedIndexChanged

        'If Val(cmb_rev_type.SelectedValue) > 0 Then
        If Not String.IsNullOrEmpty(cmb_rev_type.SelectedValue) Then

            ID_TpReview = Val(cmb_rev_type.SelectedValue)
            Me.Session("E_bnd_var_01") = ID_TpReview
            'fillcomboApp()
            'actualiza()
            fillGrid()
            chk_allreview.Checked = False

        Else

            ID_TpReview = 0
            Me.Session("E_bnd_var_01") = ID_TpReview
            ' fillcomboApp(-1)
            'actualiza()
            fillGrid()

        End If


    End Sub

    Private Sub chk_allreview_CheckedChanged(sender As Object, e As EventArgs) Handles chk_allreview.CheckedChanged

        If chk_allreview.Checked = True Then
            Me.cmb_rev_type.SelectedValue = ""
            Me.cmb_rev_type.Text = ""

            ID_TpReview = 0
            Me.Session("E_bnd_var_01") = ID_TpReview
            fillGrid()

        End If
    End Sub
End Class
