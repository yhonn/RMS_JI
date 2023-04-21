Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME


Partial Class frm_AdminTrimestresAD

    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_EDIT_PERIO"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim db As New ly_SIME.dbRMS_JIEntities

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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Try
                LoadList()
                VerDatos(Me.cmb_programa.SelectedValue)

            Catch ex As Exception
                Dim a = ex
            End Try
        End If

    End Sub

    Sub VerDatos(ByVal idPrograma As String)
        Using dbEntities As New dbRMS_JIEntities
            Dim oPeriodos = dbEntities.vw_t_periodos.Where(Function(p) p.id_programa = idPrograma).ToList()
            Dim oPeriodo = oPeriodos.Where(Function(p) p.activo.Value)
            Me.cmb_anio.SelectedValue = oPeriodo.FirstOrDefault().anio
            Me.lbl_trimestre.Text = oPeriodo.FirstOrDefault().codigo_anio_calendario

            Me.rd_periodosBloqueado.DataBind()
            If oPeriodo.FirstOrDefault().bloqueado.Value = True Then
                Me.lbl_bloqueado.Text = "SI"
                Me.rd_periodosBloqueado.SelectedValue = 1
            Else
                Me.lbl_bloqueado.Text = "NO"
                Me.rd_periodosBloqueado.SelectedValue = 0
            End If

            LoadTrimestres(oPeriodos, Me.cmb_anio.SelectedValue)

            Me.rd_trimestres.Items(Me.lbl_trimestre.Text - 1).Selected = True

        End Using
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        GuardarDatos(2)
        'VerDatos(Me.cmb_programa.SelectedValue)
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_AdminTrimestres"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub cmb_region_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        VerDatos(Me.cmb_programa.SelectedValue)
    End Sub

    Sub GuardarDatos(ByVal OptionURL As String)
        If Me.cmb_programa.SelectedItem.Text <> "" Then

            Using dbEntities As New dbRMS_JIEntities
                dbEntities.Database.ExecuteSqlCommand("UPDATE vw_t_periodos SET activo=0, bloqueado=1 WHERE id_programa=" & Me.cmb_programa.SelectedValue)
                dbEntities.Database.ExecuteSqlCommand("UPDATE t_periodo SET activo = 1, bloqueado='" & Me.rd_periodosBloqueado.SelectedValue & "' WHERE id_trimestre =" & Me.rd_trimestres.SelectedValue)
                dbEntities.SaveChanges()
            End Using

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_AdminTrimestres"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Sub LoadList()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Me.cmb_programa.DataSourceID = ""
        Me.cmb_programa.DataSource = clListados.get_t_programas(id)
        Me.cmb_programa.DataTextField = "nombre_programa"
        Me.cmb_programa.DataValueField = "id_programa"
        Me.cmb_programa.DataBind()
        Me.cmb_programa.Enabled = False

        Using dbEntities As New dbRMS_JIEntities

            Dim programa = dbEntities.t_programas.Find(id)

            Me.cmb_anio.DataSourceID = ""
            Me.cmb_anio.DataSource = db.t_trimestre.Where(Function(p) p.anio >= programa.fecha_inicio.Value.Year And p.anio <= (programa.fecha_inicio.Value.Year + programa.numero_anios)) _
                .GroupBy(Function(p) p.anio) _
                .Select(Function(p) p.FirstOrDefault()) _
                .Select(Function(p) _
                            New With {Key .anio = p.anio}).ToList()
        End Using


        Me.cmb_anio.DataTextField = "anio"
        Me.cmb_anio.DataValueField = "anio"
        Me.cmb_anio.DataBind()

    End Sub

    Sub LoadTrimestres(ByVal oPeriodos As List(Of vw_t_periodos), ByVal anio As Integer)
        Dim oTrimestres = oPeriodos.Where(Function(p) p.anio = anio) _
                             .GroupBy(Function(p) p.id_trimestre) _
                             .Select(Function(p) p.FirstOrDefault()) _
                             .Select(Function(p) _
                                         New With {Key .nombre_trimestre = "FY" & p.ciclo.Value.ToString() & "-" & p.codigo_anio_fiscal, _
                                                   Key .id_trimestre = p.id_trimestre}).ToList()

        Me.rd_trimestres.DataSource = oTrimestres
        Me.rd_trimestres.DataTextField = "nombre_trimestre"
        Me.rd_trimestres.DataValueField = "id_trimestre"
        Me.rd_trimestres.DataBind()
    End Sub

    Protected Sub cmb_anio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_anio.SelectedIndexChanged
        Dim idPrograma = Convert.ToInt32(Me.cmb_programa.SelectedValue)
        Using dbEntities As New dbRMS_JIEntities
            Dim oPeriodos = dbEntities.vw_t_periodos.Where(Function(p) p.id_programa = idPrograma).ToList()
            LoadTrimestres(oPeriodos, Me.cmb_anio.SelectedValue)
        End Using
    End Sub
End Class