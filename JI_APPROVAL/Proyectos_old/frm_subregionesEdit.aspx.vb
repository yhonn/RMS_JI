Imports ly_SIME

Public Class frm_subregionesEdit
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "EDIT_SUBR"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 8, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Me.lbl_id_subregion.Text = Me.Request.QueryString("Id").ToString
            FillData(Me.lbl_id_subregion.Text)
        End If

    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_HNEntities

                Dim id_subregion = Convert.ToInt32(Me.lbl_id_subregion.Text)
                Dim oRegiones = dbEntities.t_subregiones.Find(id_subregion)
                oRegiones.nombre_subregion = Me.txt_nombresubRegion.Text
                oRegiones.id_region = Me.cmb_region.SelectedValue
                oRegiones.prefijo_subregion = Me.txt_prefijo.Text

                dbEntities.Entry(oRegiones).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_subregiones"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Proyectos/frm_subregiones"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Sub FillData(ByVal IdSubregion As String)

        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Me.cmb_programa.DataSourceID = ""
        Me.cmb_programa.DataSource = clListados.get_t_programas()
        Me.cmb_programa.DataTextField = "nombre_programa"
        Me.cmb_programa.DataValueField = "id_programa"
        Me.cmb_programa.DataBind()
        Me.cmb_programa.SelectedValue = id

        Me.cmb_region.DataSource = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(Me.cmb_programa.SelectedValue)
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataBind()

        Using dbEntities As New dbRMS_HNEntities
            Dim id_subregion = Convert.ToInt32(IdSubregion)
            Dim oregion = dbEntities.vw_t_subregiones.FirstOrDefault(Function(p) p.id_subregion = id_subregion)

            Me.txt_nombresubRegion.Text = oregion.nombre_subregion
            Me.cmb_region.SelectedValue = oregion.id_region
            Me.txt_prefijo.Text = oregion.prefijo_subregion
            Me.cmb_programa.SelectedValue = oregion.id_programa
        End Using
    End Sub

    Protected Sub cmb_programa_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        Me.cmb_region.DataSource = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(Me.cmb_programa.SelectedValue)
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataBind()
    End Sub
End Class