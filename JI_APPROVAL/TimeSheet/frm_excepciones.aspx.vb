Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_excepciones
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "APP_AD_EXC"
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            LoadData()
            loadMeses()
            fillGrid(True)
        End If
    End Sub
    Sub LoadData()
        Using dbEntities As New dbRMS_JIEntities


            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim programa = dbEntities.t_programas.Find(id)
            Dim anios = dbEntities.t_trimestre.Where(Function(p) p.anio >= programa.fecha_inicio.Value.Year And p.anio <= (programa.fecha_inicio.Value.Year + programa.numero_anios)) _
                .GroupBy(Function(p) p.anio) _
                .Select(Function(p) p.FirstOrDefault()) _
                .Select(Function(p) _
                            New With {Key .anio = p.anio}).ToList()
            Me.cmb_year.DataSourceID = ""
            Me.cmb_year.DataSource = anios

            Me.cmb_year.DataTextField = "anio"
            Me.cmb_year.DataValueField = "anio"
            Me.cmb_year.DataBind()



        End Using
    End Sub

    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            Dim tasaSer = dbEntities.vw_tme_ts_excepciones.ToList()
            Me.grd_cate.DataSource = tasaSer
            If bndBind Then
                Me.grd_cate.DataBind()
            End If
        End Using
    End Sub

    Private Sub cmb_year_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_year.SelectedIndexChanged
        loadMeses()
    End Sub

    Sub loadMeses()
        Using dbEntities As New dbRMS_JIEntities
            Me.cmb_mes.DataSourceID = ""
            Me.cmb_mes.DataSource = dbEntities.tme_meses.ToList()
            Me.cmb_mes.DataTextField = "mes"
            Me.cmb_mes.DataValueField = "id_mes"
            Me.cmb_mes.DataBind()
        End Using

    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim excepcion = New tme_excepcion_ts
                excepcion.id_mes = Me.cmb_mes.SelectedValue
                excepcion.dia = Me.txt_tasa_cambio.Value
                excepcion.anio = Me.cmb_year.SelectedValue
                excepcion.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                dbEntities.tme_excepcion_ts.Add(excepcion)
                dbEntities.SaveChanges()

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/TimeSheet/frm_excepciones"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub
End Class