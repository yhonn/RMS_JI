Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Public Class frm_measurement
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "SKILLS_MAIN"
    Dim db As New dbRMS_JIEntities

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim listaControles As New List(Of vw_ctrl_language)

    Sub fillGrid()

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
        Me.grd_cate.DataSource = db.vw_ins_measurement.Where(Function(p) p.id_ficha_proyecto = id_ficha And
                                                                 (p.name.Contains(Me.txt_doc.Text) Or p.moderator.Contains(Me.txt_doc.Text))) _
                                                             .OrderByDescending(Function(p) p.id_measurement).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try
        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
            'listaControles = controles.listadoControles()
        End If

        If Not Me.IsPostBack Then
            fillGrid()
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_measurement = DataBinder.Eval(e.Item.DataItem, "id_measurement").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", id_measurement)
            hlnkDelete.Attributes.Add("data-identity", id_measurement)

            'Dim hlk_is_new As Label = New Label
            'hlk_is_new = CType(e.Item.FindControl("hlk_is_new"), Label)

            'If DataBinder.Eval(e.Item.DataItem, "new").ToString().Equals("1") Then
            '    hlk_is_new.Text = "Continuing"
            'Else
            '    hlk_is_new.Text = "New"
            'End If

            'Dim hlk_is_on_farm As Label = New Label
            'hlk_is_on_farm = CType(e.Item.FindControl("hlk_is_on_farm"), Label)

            'If DataBinder.Eval(e.Item.DataItem, "is_on_farm").ToString().Equals("True") Then
            '    hlk_is_on_farm.Text = "On Farm"
            'Else
            '    hlk_is_on_farm.Text = "Off Farm"
            'End If
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "~/assessment/frm_measurementEdit?id=" & id_measurement

            'Dim hlnkAttachment As HyperLink = New HyperLink
            'hlnkAttachment = CType(e.Item.FindControl("col_hlk_upload"), HyperLink)
            'hlnkAttachment.NavigateUrl = "~/Instrumentos/frm_job_createdAttach?id=" & id_job_created

            'Dim hlnkEstado As HyperLink = New HyperLink
            'hlnkEstado = CType(e.Item.FindControl("col_hlk_estado"), HyperLink)
            'hlnkEstado.ToolTip = controles.iconosGrid("col_hlk_estado")


            'Dim validadoME = DataBinder.Eval(e.Item.DataItem, "sincronizado")
            'If validadoME Is Nothing Or Not Convert.ToBoolean(validadoME) Then
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            'ElseIf Convert.ToBoolean(validadoME) Then
            '    hlnkEstado.ImageUrl = "~/Imagenes/iconos/drop-yes.gif"
            '    hlnkDelete.Visible = False
            '    hlnkEdit.Visible = False
            'End If

            Dim hlnkadd As HyperLink = New HyperLink
            hlnkadd = CType(e.Item.FindControl("hlk_add"), HyperLink)
            hlnkadd.ToolTip = controles.iconosGrid("col_hlk_add")
            hlnkadd.NavigateUrl = "frm_measurementDetail?id=" & id_measurement

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "frm_beneficiaryRep.aspx?Id=" & id_measurement
            'If listaControles.Where(Function(p) p.ctrl_code = "col_hlk_Print").Count() > 0 Then
            '    hlnkPrint.ToolTip = listaControles.FirstOrDefault(Function(p) p.ctrl_code = "col_hlk_Print").valor
            'End If
            'hlnkPrint.Visible = False
        End If
    End Sub


    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/assessment/frm_measurementAD")
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_JIEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM ins_measurement WHERE id_measurement = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Deleted Correctly"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Delete Error"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
            Me.Response.Redirect("~/assessment/frm_measurement")
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub


End Class