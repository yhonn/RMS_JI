Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Partial Class frm_AdminTrimestres
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_PERIOD"
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()

        Dim Filter As String  '= "AND id_proyecto_padre = " & Me.Session("E_IdProy")

        If h_Filter.Value.ToString.Trim.Length > 0 Then
            Filter = String.Format(" AND id_programa in ({0}) ", h_Filter.Value.ToString.Trim)
        Else
            Filter = String.Format(" AND id_programa in ({0}) ", Me.Session("E_IdPrograma"))
        End If

        'If Me.Session("E_NivelProy") = "2" Or Me.Session("E_NivelProy") = "-1" Then
        '    Filter = "AND id_proyecto = " & Me.Session("E_IdProy")
        'End If

        Me.SqlDataSource2.SelectCommand = "SELECT * FROM vw_t_periodos WHERE activo=1" & Filter
        Me.grd_cate.DataBind()
        Me.lbltotal.Text = "Total de sub regiones activas: [ " & Me.grd_cate.Items.Count & " ]"
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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            fillGrid()
            'If Me.Session("E_NivelProy") <> "1" Then
            '    Me.btn_buscar.Visible = False
            'End If
        End If

    End Sub
    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridItem
        Dim sql = ""
        Dim Activo As String = ""
        cnnME.Open()
        For Each row In Me.grd_cate.Items
            Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkActivo"), CheckBox)
            Activo = "NO"
            If chkvisible.Checked = False Then
                Activo = "SI"
            End If
            sql = "UPDATE t_planificacion SET bloqueado='" & Activo & "' WHERE id_planificacion=" & row.Cells.Item(2).Text
            Dim dm As New SqlCommand(sql, cnnME)
            dm.ExecuteNonQuery()
        Next
        cnnME.Close()
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            If e.Item.Cells(8).Text.ToString = "NO" Then
                visible.ToolTip = "Periodo ACTIVO. Permite registrar datos"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Periodo BLOQUEADO. No permite registrar datos"
            End If
            If Me.Session("E_NivelProy") <> "1" Then
                visible.Enabled = False
            End If
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        Me.Response.Redirect("frm_AdminTrimestresAD")
    End Sub

    Protected Sub btn_buscar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar0.Click
        'Me.Response.Redirect("frm_ProyectoAvanceMetasRSADSyn.aspx")
    End Sub

    Protected Sub btn_buscar1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar1.Click
        'Me.Response.Redirect("../SeguimientoMetas/frm_ProyectoAvanceIndicadoresMetasSyncRev.aspx")
    End Sub

    Protected Sub btn_crear_Click(sender As Object, e As EventArgs) Handles btn_crear.Click
        Dim id = Convert.ToInt32(Me.Session("E_IdPrograma"))
        Using dbEntities As New dbRMS_JIEntities
            Dim programa = dbEntities.t_programas.Find(id)
            Dim regiones = dbEntities.vw_t_subregiones.Where(Function(p) p.id_programa = id).ToList()
            Dim trimestres = dbEntities.t_trimestre.Where(Function(p) p.anio >= programa.fecha_inicio.Value.Year And p.anio <= (programa.fecha_inicio.Value.Year + programa.numero_anios)).ToList()
            Dim periodosActuales = dbEntities.vw_t_periodos.Where(Function(p) p.id_programa = id).ToList()

            Dim i = 1
            Dim j = 1
            For Each item In regiones
                For Each tri In trimestres
                    Dim oPeriodo As New t_periodo
                    oPeriodo.id_subregion = item.id_subregion
                    oPeriodo.ciclo = i
                    oPeriodo.id_trimestre = tri.id_trimestre
                    If j = 1 And i = 1 Then
                        oPeriodo.bloqueado = False
                        oPeriodo.activo = True
                    Else
                        oPeriodo.bloqueado = True
                        oPeriodo.activo = False
                    End If
                    If periodosActuales.Where(Function(p) p.id_subregion = oPeriodo.id_subregion And p.id_trimestre = oPeriodo.id_trimestre).Count() = 0 Then
                        dbEntities.t_periodo.Add(oPeriodo)
                    End If

                    If j = 4 Then
                        i = i + 1
                        j = 1
                    Else
                        j = j + 1
                    End If
                Next
                i = 1
            Next
            dbEntities.SaveChanges()
        End Using
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Administracion/frm_AdminTrimestres"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
End Class
