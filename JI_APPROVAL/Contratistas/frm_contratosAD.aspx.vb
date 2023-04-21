Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Public Class frm_contratosAD
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONTRATOS_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

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

        If Not Me.IsPostBack Then
            LoadData()
        End If

    End Sub
    Sub LoadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim usuarios = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_estado_usr = 1).ToList()
            Me.cmb_contratista.DataSourceID = ""
            Me.cmb_contratista.DataSource = usuarios.Where(Function(p) p.id_tipo_usuario = 3).ToList()
            Me.cmb_contratista.DataTextField = "nombre_usuario"
            Me.cmb_contratista.DataValueField = "id_usuario"
            Me.cmb_contratista.DataBind()

            Me.cmb_supervisor.DataSourceID = ""
            Me.cmb_supervisor.DataSource = usuarios.Where(Function(p) p.id_tipo_usuario = 1).ToList()
            Me.cmb_supervisor.DataTextField = "nombre_usuario"
            Me.cmb_supervisor.DataValueField = "id_usuario"
            Me.cmb_supervisor.DataBind()

            Dim estructura = dbEntities.tme_estructura_marcologico.Where(Function(p) p.tme_programa_marco_logico.id_programa = id And p.id_tipo_marcologico = 15).Select(Function(p) New With
                                                                                                                                         {Key .id_estructura_marcologico = p.id_estructura_marcologico,
                                                                                                                                          Key .descripcion_logica = p.codigo & " - " & p.descripcion_logica,
                                                                                                                                          Key .id_estructura_marcologico_2 = p.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                          Key .descripcion_logica_padre = p.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                                          Key .id_estructura_marcologico_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.id_estructura_marcologico,
                                                                                                                                          Key .descripcion_logica_padre_3 = p.tme_estructura_marcologico2.tme_estructura_marcologico2.codigo & " - " & p.tme_estructura_marcologico2.tme_estructura_marcologico2.descripcion_logica
                                                                                                                                         }).ToList()
            Me.grd_componente.DataSource = estructura
            Me.grd_componente.DataBind()
        End Using
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Using dbEntities As New dbRMS_JIEntities
                Dim oContrato = New tme_contratos
                oContrato.id_contratista = Convert.ToInt32(Me.cmb_contratista.SelectedValue)
                oContrato.id_supervisor = Convert.ToInt32(Me.cmb_supervisor.SelectedValue)
                oContrato.fecha_inicio = dt_fecha_inicio.SelectedDate
                oContrato.fecha_finalizacion = dt_fecha_finalizacion.SelectedDate
                oContrato.numero_contrato = Me.txt_numero_contrato.Text
                oContrato.numero_contacto = Me.txt_numero_contacto.Text
                oContrato.valor_contrato = Me.txt_valor.Value
                oContrato.objeto_contrato = Me.txt_objeto_contrato.Text
                oContrato.id_usuario_crea = Me.Session("E_IdUser").ToString()
                oContrato.fecha_crea = DateTime.Now
                For Each file As UploadedFile In soporte.UploadedFiles
                    Dim fecha = DateTime.Now
                    Dim exten = file.GetExtension()
                    Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & oContrato.numero_contrato & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                    Dim Path As String
                    oContrato.soporte = nombreArchivo
                    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                    file.SaveAs(Path + nombreArchivo)
                Next
                oContrato.id_estado_contrato = 1
                dbEntities.tme_contratos.Add(oContrato)
                dbEntities.SaveChanges()

                For Each row In Me.grd_componente.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim idComponente As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        If idComponente.Checked = True Then
                            Dim idEstructura As Integer = dataItem.GetDataKeyValue("id_estructura_marcologico")
                            Dim oMarco = New tme_contrato_marco_logico
                            oMarco.id_contrato = oContrato.id_contrato
                            oMarco.id_marco_logico = idEstructura
                            dbEntities.tme_contrato_marco_logico.Add(oMarco)
                            dbEntities.SaveChanges()
                        End If
                    End If
                Next

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratosEntregables?id=" & oContrato.id_contrato
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    Private Sub btn_cargar_contratos_Click(sender As Object, e As EventArgs) Handles btn_cargar_contratos.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim fecha = DateTime.Now
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim idSession = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second
            Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim fullPath = ""
            For Each file As UploadedFile In uploadFileRCE.UploadedFiles
                Dim exten = file.GetExtension()
                Dim nombreArchivo = idSession & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)

                fullPath = Path & nombreArchivo
            Next


            Dim cmd As New SqlCommand("CargarContratos", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@filePath", SqlDbType.VarChar).Value = fullPath
            cmd.Parameters.Add("@id_session", SqlDbType.VarChar).Value = idSession
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id_usuario

            Try
                cmd.Connection.Open()
                cmd.ExecuteNonQuery()
                cmd.Connection.Close()
            Catch ex As Exception
                Dim errores = ex.Message & ex.Source
            End Try

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratos"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using

    End Sub
End Class