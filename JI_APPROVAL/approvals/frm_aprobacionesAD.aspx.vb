Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Web.Script.Serialization

Partial Class frm_aprobacionesAD
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPROVAL_NW"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cl_AppDef As APPROVAL.cl_ApprovalDefinition

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
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
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
                cmb_category.SelectedIndex = -1

                cl_AppDef = New APPROVAL.cl_ApprovalDefinition(Me.Session("E_IDPrograma"))

                grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
                grd_documentos.DataBind()

                HttpContext.Current.Session.Add("cl_AppDef", cl_AppDef)

            Catch ex As Exception
                Me.lbl_id_sesion_temp.Text = "-1"
            End Try


        Else

            If HttpContext.Current.Session.Item("cl_AppDef") IsNot Nothing Then
                cl_AppDef = Session.Item("cl_AppDef")
            End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click

        Dim err As Boolean = False
        Dim ext As String = ""
        Dim cod_Act As String
        Dim gridlleno As Boolean = False
        Dim item As GridItem
        Dim i = 0

        i = Me.grd_cate.Items.Count

        If i = 0 Then
            Me.lbl_docsValid.Visible = True
            err = True
        End If

        If Val(cmb_category.SelectedValue) = 0 Then
            err = True
        End If

        If err = False Then

            Me.lblt_file_required.Text = ext.ToString

            If Me.chk_codAct.Checked = True Then
                cod_Act = "SI"
            Else
                cod_Act = "NO"
            End If

            cl_AppDef.set_ta_tipoDocumento(0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("id_categoria", Me.cmb_category.SelectedValue, "id_tipoDocumento", 0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("descripcion_aprobacion", Me.txt_aprobacion.Text, "id_tipoDocumento", 0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("condicion", Me.txt_condicion.Text, "id_tipoDocumento", 0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("nivel_aprobacion", Me.txt_level.Text, "id_tipoDocumento", 0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("cod_actividad", cod_Act, "id_tipoDocumento", 0)
            cl_AppDef.set_ta_tipoDocumentoFIELDS("id_approval_tool", Me.cmb_tools.SelectedValue, "id_tipoDocumento", 0)

            If cl_AppDef.save_ta_tipoDocumento() <> -1 Then
                cl_AppDef.save_tipoDocumentosAPP(Me.lbl_id_sesion_temp.Text)
            Else 'Error
            End If

            Session.Remove("cl_AppDef") 'Remove the session
            Me.Response.Redirect("~/approvals/frm_aprobaciones.aspx")

        Else

            If Val(cmb_category.SelectedValue) = 0 Then
                lblt_file_required.Text = "A category type is requeried"
            End If

            lblt_file_required.Visible = True

        End If

    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_aprobaciones.aspx")
    End Sub

    Protected Sub chkVisible_CheckedChangedDOCS(ByVal sender As Object, ByVal e As System.EventArgs)

        ActualizaDatosDOCS()

    End Sub

    Sub ActualizaDatosDOCS()

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

            If chkvisible.Checked = True Then

                Dim Sql = " INSERT INTO ta_aprobacion_docs_temp (id_sesion_temp,  id_doc_soporte, id_programa) VALUES ('" & Me.lbl_id_sesion_temp.Text & "'," & Irow("id_doc_soporte").Text & ", " & Me.Session("E_IDPrograma") & ") "
                cnnSAP.Open()
                Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                Dim ds As New DataSet("IdPlan")
                dm.Fill(ds, "IdPlan")
                cnnSAP.Close()

            End If
        Next

        grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
        grd_documentos.DataBind()
        Me.grd_cate.DataBind()

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




    Sub ActualizaDatos()


        Dim requeridoInicioSI As String = "0"
        Dim requeridoInicioNO As String = "0"

        Dim requeridoFinalSI As String = "0"
        Dim requeridoFinalNO As String = "0"

        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each rowD As GridDataItem In Me.grd_cate.Items

            'Dim chkvisible As CheckBox = CType(Row.Cells(0).FindControl("chkVisible"), CheckBox)
            Dim chkvisible As CheckBox = CType(rowD("visible").FindControl("chkVisible"), CheckBox)
            Dim chkRS As CheckBox = CType(rowD("visibleRS").FindControl("chkRSvisible"), CheckBox)
            Dim chkRE As CheckBox = CType(rowD("visibleRE").FindControl("chkREvisible"), CheckBox)


            If chkvisible.Checked = True Then
                visibleSI &= "," & rowD("id_app_docs_temp").Text
            Else
                visibleNO &= "," & rowD("id_app_docs_temp").Text
            End If

            If chkRS.Checked = True Then
                requeridoInicioSI &= "," & rowD("id_app_docs_temp").Text
            Else
                requeridoInicioNO &= "," & rowD("id_app_docs_temp").Text
            End If

            If chkRE.Checked = True Then
                requeridoFinalSI &= "," & rowD("id_app_docs_temp").Text
            Else
                requeridoFinalNO &= "," & rowD("id_app_docs_temp").Text
            End If

        Next
        cnnSAP.Open()
        Dim dm As New SqlCommand("UPDATE ta_aprobacion_docs_temp SET PermiteRepetir='SI' WHERE id_app_docs_temp IN(" & visibleSI & ")", cnnSAP)
        dm.ExecuteNonQuery()
        dm.CommandText = "UPDATE ta_aprobacion_docs_temp SET PermiteRepetir='NO' WHERE id_app_docs_temp IN(" & visibleNO & ")"
        dm.ExecuteNonQuery()


        dm.CommandText = "UPDATE ta_aprobacion_docs_temp SET RequeridoInicio='SI' WHERE id_app_docs_temp IN(" & requeridoInicioSI & ")"
        dm.ExecuteNonQuery()

        dm.CommandText = "UPDATE ta_aprobacion_docs_temp SET RequeridoInicio='NO' WHERE id_app_docs_temp IN(" & requeridoInicioNO & ")"
        dm.ExecuteNonQuery()

        dm.CommandText = "UPDATE ta_aprobacion_docs_temp SET RequeridoFin='SI' WHERE id_app_docs_temp IN(" & requeridoFinalSI & ")"
        dm.ExecuteNonQuery()

        dm.CommandText = "UPDATE ta_aprobacion_docs_temp SET RequeridoFin='NO' WHERE id_app_docs_temp IN(" & requeridoFinalNO & ")"
        dm.ExecuteNonQuery()

        cnnSAP.Close()
        Me.grd_cate.DataBind()

    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand

        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_app_docs_temp").ToString()
        Me.SqlDataSource4.DeleteCommand = "DELETE FROM ta_aprobacion_docs_temp WHERE (id_app_docs_temp = " & id_temp & ")"
        Me.grd_cate.DataBind()

        grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
        grd_documentos.DataBind()

        ' Me.rb_documents.DataBind()

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

            If itemD("PermiteRepetir").Text = "SI" Then  'e.Item.Cells(7).Text.ToString 
                visible.ToolTip = "Is allowed attached more than once document"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Not allowed attached more than once document"
            End If

            If itemD("RequeridoInicio").Text = "SI" Then  'e.Item.Cells(7).Text.ToString 
                visibleRS.ToolTip = "Is required to attached a document at the start"
                visibleRS.Checked = True
            Else
                visibleRS.Checked = False
                visibleRS.ToolTip = "Is not required to attached a document at the start"
            End If

            If itemD("RequeridoFin").Text = "SI" Then  'e.Item.Cells(7).Text.ToString 
                visibleRE.ToolTip = "Is required to attached a document at the end"
                visibleRE.Checked = True
            Else
                visibleRE.Checked = False
                visibleRE.ToolTip = "Is not required to attached a document at the end"
            End If

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
        grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
    End Sub


    <Web.Services.WebMethod()>
    Public Shared Function GetTools(ByVal idCat As Integer, ByVal idPrograma As Integer) As Object

        Dim jsonITEMS As String = "[]"

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(idPrograma)
        Dim ResultItems As Object = cls_TimeSheet.get_app_tools(idCat)

        Dim serializer As New JavaScriptSerializer()
        If ResultItems.Count() > 0 Then
            jsonITEMS = serializer.Serialize(ResultItems)
        End If


        Return jsonITEMS

    End Function


    Protected Sub getToolsPOST(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

        Me.cmb_tools.DataSource = cls_TimeSheet.get_app_tools(Me.cmb_category.SelectedValue)
        Me.cmb_tools.DataTextField = "Text"
        Me.cmb_tools.DataValueField = "Value"
        Me.cmb_tools.SelectedIndex = -1
        Me.cmb_tools.DataBind()

    End Sub


End Class
