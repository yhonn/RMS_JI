Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_aprobaciones_edit
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPROVAL_EDIT"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cl_AppDef As APPROVAL.cl_ApprovalDefinition

    Dim dtSelectedDocs As DataTable
    Dim dtDocs_approval As DataTable

    Sub cargadatos(ByVal id_tipoDoc As Integer)

        'Dim Sql = "SELECT * FROM vw_aprobaciones WHERE id_tipoDocumento=" & id_tipoDoc
        'cnnSAP.Open()
        'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        'Dim ds As New DataSet("vw_aprobaciones")
        'dm.Fill(ds, "vw_aprobaciones")

        cl_AppDef.set_ta_tipoDocumento(id_tipoDoc)

        If cl_AppDef.get_ta_tipoDocumentoFIELDS("cod_actividad", "id_tipoDocumento", id_tipoDoc) = "SI" Then
            Me.chk_codAct.Checked = True
        Else
            Me.chk_codAct.Checked = False
        End If

        'Me.txt_aprobacion.Text = ds.Tables("vw_aprobaciones").Rows(0).Item("descripcion_aprobacion").ToString
        'Me.txt_condicion.Text = ds.Tables("vw_aprobaciones").Rows(0).Item("condicion").ToString
        'Me.txt_level.Text = ds.Tables("vw_aprobaciones").Rows(0).Item("nivel_aprobacion").ToString
        'Me.cmb_category.SelectedValue = ds.Tables("vw_aprobaciones").Rows(0).Item("id_categoria").ToString

        Me.txt_aprobacion.Text = cl_AppDef.get_ta_tipoDocumentoFIELDS("descripcion_aprobacion", "id_tipoDocumento", id_tipoDoc)
        Me.txt_condicion.Text = cl_AppDef.get_ta_tipoDocumentoFIELDS("condicion", "id_tipoDocumento", id_tipoDoc)
        Me.txt_level.Text = cl_AppDef.get_ta_tipoDocumentoFIELDS("nivel_aprobacion", "id_tipoDocumento", id_tipoDoc)
        Me.cmb_category.SelectedValue = cl_AppDef.get_ta_tipoDocumentoFIELDS("id_categoria", "id_tipoDocumento", id_tipoDoc)


        grd_documentos.DataSource = cl_AppDef.get_DocumentTypes()
        grd_documentos.DataBind()

        'Me.grd_cate.DataBind()
        'cnnSAP.Close()
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = dtDocs_approval
        Me.grd_cate.DataBind()

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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            Try
                Dim rnd As New Random()
                Dim fecha As DateTime = Date.Now
                Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
                Me.lbl_id_sesion_temp.Text = rnd.Next(1, 999999).ToString & textfecha.ToString
            Catch ex As Exception
                Me.lbl_id_sesion_temp.Text = "-1"
            End Try

            Me.HiddenField1.Value = Me.Request.QueryString("IdType")

            cl_AppDef = New APPROVAL.cl_ApprovalDefinition(Me.Session("E_IDPrograma"), Me.HiddenField1.Value)

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtSelectedDocs.Value = String.Format("dtSelectedDocs{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            Me.hd_dtDocs_approval.Value = String.Format("dtDocs_approval{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            Dim cls_approval_route As APPROVAL.clss_approval_route
            cls_approval_route = New APPROVAL.clss_approval_route(Me.Session("E_IDPrograma"), Me.HiddenField1.Value)

            dtSelectedDocs = cls_approval_route.get_Ruta_Rol_Aprobacion(0)
            Session(Me.hd_dtSelectedDocs.Value) = dtSelectedDocs

            dtDocs_approval = cl_AppDef.get_Documents_approval()
            Session(Me.hd_dtDocs_approval.Value) = dtDocs_approval

            cargadatos(Me.HiddenField1.Value)

            HttpContext.Current.Session.Add("cl_AppDef", cl_AppDef)

        Else

            dtSelectedDocs = Session(Me.hd_dtSelectedDocs.Value)
            dtDocs_approval = Session(Me.hd_dtDocs_approval.Value)

            If HttpContext.Current.Session.Item("cl_AppDef") IsNot Nothing Then
                cl_AppDef = Session.Item("cl_AppDef")
            End If

        End If
    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""
        Dim codact As String

        If err = False Then

            If Me.chk_codAct.Checked = True Then
                codact = "SI"
            Else
                codact = "NO"
            End If


            'Dim Sql = "UPDATE ta_tipoDocumento SET id_categoria=" & Me.cmb_category.SelectedValue & ", descripcion_aprobacion='" & Me.txt_aprobacion.Text & "', condicion='" & Me.txt_condicion.Text & "', nivel_aprobacion='" & Me.txt_level.Text & "', cod_actividad='" & codact & "' "
            'Sql &= " WHERE id_tipoDocumento=" & Me.HiddenField1.Value
            'cnnSAP.Open()
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("IdPlan")
            'dm.Fill(ds, "IdPlan")


            cl_AppDef.set_ta_tipoDocumento(Me.HiddenField1.Value)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("id_categoria", Me.cmb_category.SelectedValue, "id_tipoDocumento", Me.HiddenField1.Value)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("descripcion_aprobacion", Me.txt_aprobacion.Text, "id_tipoDocumento", Me.HiddenField1.Value)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("condicion", Me.txt_condicion.Text, "id_tipoDocumento", Me.HiddenField1.Value)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("nivel_aprobacion", Me.txt_level.Text, "id_tipoDocumento", Me.HiddenField1.Value)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("cod_actividad", codact, "id_tipoDocumento", Me.HiddenField1.Value)

            If cl_AppDef.save_ta_tipoDocumento() <> -1 Then
                Session.Remove("cl_AppDef") 'Remove the session
                Session.Remove(Me.hd_dtSelectedDocs.Value)
                Session.Remove(Me.hd_dtDocs_approval.Value)
                Me.Response.Redirect("~/Approvals/frm_aprobaciones.aspx")
            Else 'Error
            End If

            '   cnnSAP.Close()

        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_aprobaciones.aspx")
    End Sub


    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub

    Protected Sub chkRSvisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub

    Protected Sub chkREvisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub


    Protected Sub cmb_Selected_Changed(ByVal sender As Object, ByVal e As System.EventArgs)

        ActualizaDatos()

    End Sub

    Sub ActualizaDatos_cmb()

        Dim dm As New SqlCommand("", cnnSAP)
        Dim strSQL As String = "update ta_aprobacion_docs set id_ruta = {0} where id_app_docs = {1} "

        cnnSAP.Open()
        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim cmb_tp As RadComboBox = CType(rowD("colm_stepAssigned").FindControl("cmb_step"), RadComboBox)
            Dim idRuta As Integer = CType(cmb_tp.SelectedValue, Integer)

            Dim sqlExec As String = String.Format(strSQL, idRuta, rowD("id_app_docs").Text)

            dm.CommandText = sqlExec
            dm.ExecuteNonQuery()


        Next
        cnnSAP.Close()


    End Sub

    Sub ActualizaDatos()

        Dim requeridoInicioSI As String = "0"
        Dim requeridoInicioNO As String = "0"

        Dim requeridoFinalSI As String = "0"
        Dim requeridoFinalNO As String = "0"

        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        Dim bndFound As Boolean = False
        Dim dtROW_approval As DataRow = Nothing

        Dim strSQL As String = "UPDATE ta_aprobacion_docs SET "
        Dim strUpdateList As String = ""
        Dim idRuta As Integer = 0

        Dim dm As New SqlCommand("", cnnSAP)

        cnnSAP.Open()

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkvisible As CheckBox = CType(rowD("visible").FindControl("chkVisible"), CheckBox)
            Dim chkRS As CheckBox = CType(rowD("visibleRS").FindControl("chkRSvisible"), CheckBox)
            Dim chkRE As CheckBox = CType(rowD("visibleRE").FindControl("chkREvisible"), CheckBox)
            Dim cmb_tp As RadComboBox = CType(rowD("colm_stepAssigned").FindControl("cmb_step"), RadComboBox)

            idRuta = cmb_tp.SelectedValue

            For Each itemAPP In dtDocs_approval.Rows

                If CType(itemAPP("id_app_docs"), Integer) = CType(rowD("id_app_docs").Text, Integer) Then
                    bndFound = True
                    dtROW_approval = itemAPP
                    Exit For
                End If

            Next

            If bndFound Then

                If ((dtROW_approval("PermiteRepetir") = "SI" And chkvisible.Checked = False) Or (dtROW_approval("PermiteRepetir") = "NO" And chkvisible.Checked = True)) Then 'Different

                    strUpdateList &= String.Format("  PermiteRepetir='{0}',", If(chkvisible.Checked, "SI", "NO"))

                End If

                If ((dtROW_approval("RequeridoInicio") = "SI" And chkRS.Checked = False) Or (dtROW_approval("RequeridoInicio") = "NO" And chkRS.Checked = True)) Then 'Different

                    strUpdateList &= String.Format("  RequeridoInicio='{0}',", If(chkRS.Checked, "SI", "NO"))

                End If

                If ((dtROW_approval("RequeridoFin") = "SI" And chkRE.Checked = False) Or (dtROW_approval("RequeridoFin") = "NO" And chkRE.Checked = True)) Then 'Different

                    strUpdateList &= String.Format("  RequeridoFin='{0}',", If(chkRE.Checked, "SI", "NO"))

                End If

                If (CType(dtROW_approval("id_ruta"), Integer) <> CType(idRuta, Integer)) Then

                    strUpdateList &= String.Format("  id_ruta={0},", idRuta)

                End If

                If strUpdateList.Trim.Length > 0 Then

                    dm.CommandText = strSQL & strUpdateList.TrimEnd(",") & " where id_app_docs  = " & rowD("id_app_docs").Text
                    dm.ExecuteNonQuery()

                End If

                bndFound = False
                strUpdateList = ""
                dtROW_approval = Nothing
                idRuta = 0

                'If chkvisible.Checked = True Then
                '    visibleSI &= "," & rowD("id_app_docs").Text
                'Else
                '    visibleNO &= "," & rowD("id_app_docs").Text
                'End If

                'If chkRS.Checked = True Then
                '    requeridoInicioSI &= "," & rowD("id_app_docs").Text
                'Else
                '    requeridoInicioNO &= "," & rowD("id_app_docs").Text
                'End If

                'If chkRE.Checked = True Then
                '    requeridoFinalSI &= "," & rowD("id_app_docs").Text
                'Else
                '    requeridoFinalNO &= "," & rowD("id_app_docs").Text
                'End If

            End If

        Next

        cnnSAP.Close()

        dtDocs_approval = cl_AppDef.get_Documents_approval()
        Session(Me.hd_dtDocs_approval.Value) = dtDocs_approval

        Fill_grid()

        'Dim dm As New SqlCommand("UPDATE ta_aprobacion_docs SET PermiteRepetir='SI' WHERE id_app_docs IN(" & visibleSI & ")", cnnSAP)
        'dm.ExecuteNonQuery()

        'dm.CommandText = "UPDATE ta_aprobacion_docs SET PermiteRepetir='NO' WHERE id_app_docs IN(" & visibleNO & ")"
        'dm.ExecuteNonQuery()

        'dm.CommandText = "UPDATE ta_aprobacion_docs SET RequeridoInicio='SI' WHERE id_app_docs IN(" & requeridoInicioSI & ")"
        'dm.ExecuteNonQuery()

        'dm.CommandText = "UPDATE ta_aprobacion_docs SET RequeridoInicio='NO' WHERE id_app_docs IN(" & requeridoInicioNO & ")"
        'dm.ExecuteNonQuery()

        'dm.CommandText = "UPDATE ta_aprobacion_docs SET RequeridoFin='SI' WHERE id_app_docs IN(" & requeridoFinalSI & ")"
        'dm.ExecuteNonQuery()

        'dm.CommandText = "UPDATE ta_aprobacion_docs SET RequeridoFin='NO' WHERE id_app_docs IN(" & requeridoFinalNO & ")"
        'dm.ExecuteNonQuery()

    End Sub


    Protected Sub Fill_grid()

        Me.grd_cate.DataSource = dtDocs_approval
        Me.grd_cate.DataBind()

    End Sub

    Protected Sub chkVisible_CheckedChangedDOCS(ByVal sender As Object, ByVal e As System.EventArgs)

        ActualizaDatosDOCS()

    End Sub

    Sub ActualizaDatosDOCS()

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

            If chkvisible.Checked = True Then

                Dim Sql = " INSERT INTO ta_aprobacion_docs (id_tipoDocumento,  id_doc_soporte, id_ruta) VALUES (" & Me.Request.QueryString("IdType") & "," & Irow("id_doc_soporte").Text & ", 0 ) "
                cnnSAP.Open()
                Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                Dim ds As New DataSet("IdPlan")
                dm.Fill(ds, "IdPlan")
                cnnSAP.Close()
            End If

        Next

        grd_documentos.DataSource = cl_AppDef.get_DocumentTypes()
        grd_documentos.DataBind()

        dtDocs_approval = cl_AppDef.get_Documents_approval()
        Session(Me.hd_dtDocs_approval.Value) = dtDocs_approval

        Fill_grid()

    End Sub


    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand

        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_app_docs").ToString()
        Dim sqlText = "DELETE FROM ta_aprobacion_docs WHERE (id_app_docs = " & id_temp & ")"
        Dim dm As New SqlCommand(sqlText, cnnSAP)

        cnnSAP.Open()
        dm.ExecuteNonQuery()
        cnnSAP.Close()

        grd_documentos.DataSource = cl_AppDef.get_DocumentTypes()
        grd_documentos.DataBind()

        dtDocs_approval = cl_AppDef.get_Documents_approval()
        Session(Me.hd_dtDocs_approval.Value) = dtDocs_approval

        Fill_grid()

    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim visible As New CheckBox
            Dim visibleRS As New CheckBox
            Dim visibleRE As New CheckBox

            visible = CType(itemD("visible").FindControl("chkVisible"), CheckBox)
            visibleRS = CType(itemD("visibleRS").FindControl("chkRSvisible"), CheckBox)
            visibleRE = CType(itemD("visibleRE").FindControl("chkREvisible"), CheckBox)

            If itemD("PermiteRepetir").Text = "SI" Then
                visible.ToolTip = "Is allowed attached more than once document"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Not allowed attached more than once document"
            End If

            If itemD("RequeridoInicio").Text = "SI" Then
                visibleRS.ToolTip = "Is required to attached a document at the start"
                visibleRS.Checked = True
            Else
                visibleRS.Checked = False
                visibleRS.ToolTip = "Is not required to attached a document at the start"
            End If

            If itemD("RequeridoFin").Text = "SI" Then
                visibleRE.ToolTip = "Is required to attached a document at the end"
                visibleRE.Checked = True
            Else
                visibleRE.Checked = False
                visibleRE.ToolTip = "Is not required to attached a document at the end"
            End If

            Dim cmb_tp As RadComboBox = CType(itemD("colm_stepAssigned").FindControl("cmb_step"), RadComboBox)
            cmb_tp.DataSource = dtSelectedDocs
            cmb_tp.DataTextField = "nombre_empleado"
            cmb_tp.DataValueField = "id_ruta"
            cmb_tp.DataBind()

            cmb_tp.SelectedValue = itemD("id_ruta").Text

        End If
    End Sub

    Private Sub grd_documentos_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_documentos.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then


            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim visible As New CheckBox
            Dim hlk_ref As New HyperLink

            visible = itemD("colm_select").FindControl("chkSelect")
            visible.Checked = False
            visible.ToolTip = "Select a document"

            hlk_ref = itemD("colm_template").FindControl("hlk_template")

            If Not itemD("Template").Text.Contains("--none--") Then
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            Else
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "#"
            End If

        End If


    End Sub

    Private Sub grd_documentos_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_documentos.NeedDataSource
        grd_documentos.DataSource = cl_AppDef.get_DocumentTypes()
    End Sub
End Class
