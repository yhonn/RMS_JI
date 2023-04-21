Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Partial Class frm_AdminCatalogosAD
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_NEW_CATA"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

    '***********************FUNCIONES**********************
    Sub fill()
        Using dbEntities As New dbRMS_JIEntities
            Dim idCatalogo = Convert.ToInt32(Me.cmbCatalogo.SelectedValue)
            Dim oAdminCatalogo = dbEntities.t_AdminCatalogos.Find(idCatalogo)
            Me.lbl_tabla.Text = oAdminCatalogo.NombreTabla
            Dim strId = Convert.ToInt32(oAdminCatalogo.id_tabla_padre)
            Me.lbl_Idcampo.Text = oAdminCatalogo.CampoID
            Me.lbl_nombreCam.Text = oAdminCatalogo.CampoDescripcion
            Me.lbl_tipoCampo.Text = oAdminCatalogo.CampoTipo
            Dim strTipo = oAdminCatalogo.CampoTipo
            Dim Sql = ""
            If strTipo = "-1" Then
                Dim itemC As New RadComboBoxItem
                itemC.Text = "N/A"
                itemC.Value = -1
                Me.cmb_tipo.Items.Add(itemC)
                'Me.cmb_tipo.SelectedValue = -1
                Me.cmb_tipo.Enabled = False
            Else
                Dim oAdminCatalogoPadre = dbEntities.t_AdminCatalogos.Find(strId)
                Sql = " SELECT " & oAdminCatalogoPadre.CampoID & "  AS id, " _
                 & oAdminCatalogoPadre.CampoDescripcion & " AS nombre FROM " & oAdminCatalogoPadre.NombreTabla & " WHERE id_programa = " & Me.Session("E_IDPrograma")
                Me.SqlDataSource3.SelectCommand = Sql
                Me.cmb_tipo.DataBind()
                Me.cmb_tipo.Enabled = True
            End If
            Me.cmb_coincidencia.DataSource = loadCoincidencias(Me.cmbCatalogo.SelectedValue)
            Me.cmb_coincidencia.DataBind()
            'Me.cmb_coincidencia.SelectedIndex = 0
            Me.cmb_coincidencia.Filter = 1
        End Using
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        If Not IsPostBack Then
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Using dbEntities As New dbRMS_JIEntities
                Dim oPrograma = dbEntities.t_programas.Find(id_programa)
                Me.lbl_programa.Text = oPrograma.nombre_programa
            End Using

            Me.cmbCatalogo.DataSource = ""
            Me.cmbCatalogo.DataSource = clListados.get_t_admincatalogos(id_programa)
            Me.cmbCatalogo.DataValueField = "id_CatalogoMaster"
            Me.cmbCatalogo.DataTextField = "TipoCatalogo"
            Me.cmbCatalogo.DataBind()

            fill()
        End If
    End Sub


    Protected Sub cmb_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_AdminCatalogo"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub cmb_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Dim sql As String = ""
        Dim selected As String = ""
        Dim campos As String = ""

        If Not String.IsNullOrWhiteSpace(Me.cmb_coincidencia.Text) Then
            For Each item As RadComboBoxItem In Me.cmb_coincidencia.Items
                If item.Text.Contains(Me.cmb_coincidencia.Text) Then
                    Me.lblt_validacion.Text = "Existen elementos semejantes al que va a ingresar, desea guardar?"
                    Me.lblt_validacion.Visible = True
                    Me.chk_confirmacion.Visible = True
                End If
            Next
        Else
            Me.lblt_validacion.Visible = False
            Me.chk_confirmacion.Visible = False
        End If

        If (Me.lblt_validacion.Visible = False And chk_confirmacion.Visible = False) Or (Me.lblt_validacion.Visible = True And chk_confirmacion.Checked) Then
            If Me.lbl_tipoCampo.Text = "-1" Then
                selected = ""
                campos = ""
            Else
                campos = "," & Me.lbl_tipoCampo.Text
                'campos &= If(Me.cmbCatalogo.SelectedValue = 17, ",secProductivo", "")

                Dim m = Me.cmb_tipo.SelectedItem.Text
                selected = "," & Me.cmb_tipo.SelectedValue
                'selected &= If(Me.cmbCatalogo.SelectedValue = 17, "," & If(chkSectorP.Checked, 1, 0).ToString, "")
            End If

            Using dbEntities As New dbRMS_JIEntities
                sql = " INSERT INTO " & Me.lbl_tabla.Text & " (" & Me.lbl_nombreCam.Text & ", id_programa " & campos & ")"
                sql &= " VALUES ('" & Me.txt_descripcion.Text & "', " & Me.Session("E_IDPrograma") & selected & ")"
                dbEntities.Database.ExecuteSqlCommand(sql)
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_admincatalogo"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End If

    End Sub


    Protected Sub TP_cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        If Val(cmbCatalogo.SelectedValue) > 0 Then
            chkSectorP.Visible = False
        End If
        Me.cmb_coincidencia.DataSource = loadCoincidencias(Me.cmbCatalogo.SelectedValue)
        Me.cmb_coincidencia.DataBind()

        If Not String.IsNullOrWhiteSpace(Me.cmb_coincidencia.Text) Then
            For Each item As RadComboBoxItem In Me.cmb_coincidencia.Items
                If item.Text.Contains(Me.cmb_coincidencia.Text) Then
                    Me.lblt_validacion.Text = "Existen elementos semejantes al que va a ingresar, desea guardar?"
                    Me.lblt_validacion.Visible = True
                    Me.chk_confirmacion.Visible = True
                End If
            Next
        Else
            Me.lblt_validacion.Visible = False
            Me.chk_confirmacion.Visible = False
        End If


        'Me.cmb_coincidencia.SelectedIndex = 0
        Me.cmb_coincidencia.Filter = 1
    End Sub

    Protected Sub cmbCatalogo_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbCatalogo.SelectedIndexChanged
        fill()
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

    'Protected Sub cmb_coincidencia_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs) Handles cmb_coincidencia.ItemsRequested
    '    Me.cmb_coincidencia.DataSource = loadCoincidencias(Me.cmb_coincidencia.Text, Me.cmbCatalogo.SelectedValue)
    '    Me.cmb_coincidencia.DataBind()
    'End Sub

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

    Protected Sub cmb_coincidencia_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_coincidencia.SelectedIndexChanged
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
