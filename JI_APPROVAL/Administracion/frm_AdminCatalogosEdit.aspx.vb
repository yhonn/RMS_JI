Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Partial Class frm_AdminCatalogosEdit
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_EDIT_CATA"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    '***********************FUNCIONES**********************
    Sub fillDatos()

        Using dbEntities As New dbRMS_JIEntities
            Dim Sql = ""
            Dim idCatalogo = Convert.ToInt32(Me.cmbCatalogo.SelectedValue)
            Dim oAdminCatalogo = dbEntities.t_AdminCatalogos.Find(idCatalogo)
            Me.lbl_tabla.Text = oAdminCatalogo.NombreTabla
            Me.lbl_Idcampo.Text = oAdminCatalogo.CampoID
            Me.lbl_nombreCam.Text = oAdminCatalogo.CampoDescripcion
            Me.lbl_campo_tipo.Text = oAdminCatalogo.CampoTipo
            Dim strTipo = oAdminCatalogo.CampoTipo
            Dim strId = Convert.ToInt32(oAdminCatalogo.id_tabla_padre)
            If strTipo = "-1" Then
                'Me.SqlDataSource3.SelectCommand = "SELECT id_tipo_actividad_clave AS id, nombre_tipo_actividad_clave AS nombre FROM tme_ActividadClaveTipo WHERE id_tipo_actividad_clave=-1"
                'Me.cmb_tipo.DataBind()
                Dim itemC As New RadComboBoxItem
                itemC.Text = "N/A"
                itemC.Value = -1
                Me.cmb_tipo.Items.Add(itemC)
            Else
                Dim oAdminCatalogoPadre = dbEntities.t_AdminCatalogos.Find(strId)
                Sql = " SELECT " & oAdminCatalogoPadre.CampoID & "  AS id, " & oAdminCatalogoPadre.CampoDescripcion & " AS nombre FROM " & oAdminCatalogoPadre.NombreTabla & " WHERE id_programa = " & Me.Session("E_IDPrograma")
                Me.SqlDataSource3.SelectCommand = Sql
                Me.cmb_tipo.DataBind()
                Me.cmb_tipo.SelectedValue = Convert.ToInt32(Me.Request.QueryString("IdTipo"))
            End If
            Me.cmb_coincidencia.DataSource = loadCoincidencias(Me.cmbCatalogo.SelectedValue)
            Me.cmb_coincidencia.DataBind()
            'Me.cmb_coincidencia.SelectedIndex = 0
            Me.cmb_coincidencia.Filter = 1
        End Using
        'Dim Sql = "SELECT id_CatalogoMaster, id_tabla_padre, id_programa, CampoID, CampoDescripcion, CampoTipo, NombreTabla, TipoCatalogo FROM t_AdminCatalogos WHERE id_CatalogoMaster=" & Me.Request.QueryString("IdCat")
        'Dim dm As New SqlDataAdapter(Sql, cnn)
        'Dim ds As New DataSet("tabla")
        'dm.Fill(ds, "tabla")
        
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
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
                End If
                controles.code_mod = frmCODE
                For Each Control As Control In Page.Controls

                    controles.checkControls(Control, cl_user.id_idioma, cl_user)
                Next
            End If

            Dim Sql = ""
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim idCatalogo = Convert.ToInt32(Me.Request.QueryString("IdCat"))

            Me.lbl_programa.Text = Me.Session("E_Programa")
            Me.cmbCatalogo.DataSource = ""
            Me.cmbCatalogo.DataSource = clListados.get_t_admincatalogos()
            Me.cmbCatalogo.DataValueField = "id_CatalogoMaster"
            Me.cmbCatalogo.DataTextField = "TipoCatalogo"
            Me.cmbCatalogo.DataBind()
            Me.cmbCatalogo.Enabled = False
            Me.cmbCatalogo.SelectedValue = idCatalogo

            fillDatos()
            Me.cmb_tipo.SelectedValue = Me.Request.QueryString("IdTipo")
            Sql = " SELECT " & Me.lbl_Idcampo.Text & " AS id , " & Me.lbl_nombreCam.Text & " AS nombre FROM " & Me.lbl_tabla.Text & " WHERE " & Me.lbl_Idcampo.Text & "=" & Me.Request.QueryString("ID")
            Dim dm As New SqlDataAdapter(Sql, cnn)
            Dim ds As New DataSet("programa")
            ds.Tables.Add("Valor")
            dm.SelectCommand.CommandText = Sql
            dm.Fill(ds, "Valor")
            Me.txt_descripcion.Text = ds.Tables("Valor").Rows(0).Item("nombre")
        End If
    End Sub


    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_AdminCatalogo"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim sql As String = ""
            sql = " UPDATE " & Me.lbl_tabla.Text & " SET " & Me.lbl_nombreCam.Text & "='" & Me.txt_descripcion.Text & "', " & Me.lbl_campo_tipo.Text & _
                " = " & Me.cmb_tipo.SelectedValue & " WHERE " & Me.lbl_Idcampo.Text & "=" & Me.Request.QueryString("ID")
            dbEntities.Database.ExecuteSqlCommand(sql)
        End Using
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Administracion/frm_admincatalogo"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Public Function loadCoincidencias(ByVal strID As Integer) As DataTable
        Using dbEntities As New dbRMS_JIEntities
            Dim oAdminCatalogoPadre = dbEntities.t_AdminCatalogos.Find(strID)
            Dim Sql = " SELECT " & oAdminCatalogoPadre.CampoID & "  AS id, " & oAdminCatalogoPadre.CampoDescripcion & " AS nombre FROM " & oAdminCatalogoPadre.NombreTabla & " WHERE id_programa = " & Me.Session("E_IDPrograma")
            'Me.SqlDSCoincidencia.SelectCommand = Sql
            'Me.cmb_conincidencia.Enabled = True

            'devuelve la información de todas los Centro y corregimientos registradas para un proyecto en el sistema SIME, recibiendo como parametro el id del proyecto

            Dim dm As New SqlDataAdapter(Sql, cnn)
            Dim ds As New DataSet("CC")
            dm.Fill(ds, "CC")
            'ds.Tables.Item("CC").Rows.Add(New Object() {0, "--SELECCIONE--"})
            loadCoincidencias = ds.Tables.Item("CC")
        End Using
    End Function

    Protected Sub cmb_coincidencia_TextChanged(sender As Object, e As EventArgs) Handles cmb_coincidencia.TextChanged
        Me.txt_descripcion.Text = Me.cmb_coincidencia.Text

        If Not String.IsNullOrWhiteSpace(Me.cmb_coincidencia.Text) Then
            Dim coincide = False
            For Each item As RadComboBoxItem In Me.cmb_coincidencia.Items
                If item.Text.ToLower().Contains(Me.cmb_coincidencia.Text.ToLower()) Then
                    coincide = True
                End If
            Next
            If coincide Then
                Me.lblt_validacion.Text = "Existen elementos semejantes al que va a ingresar, desea guardar?"
                Me.lblt_validacion.Visible = True
                Me.chk_confirmacion.Visible = True
            Else
                Me.lblt_validacion.Visible = False
                Me.chk_confirmacion.Visible = False
            End If

        Else
            Me.lblt_validacion.Visible = False
            Me.chk_confirmacion.Visible = False
        End If
    End Sub
End Class
