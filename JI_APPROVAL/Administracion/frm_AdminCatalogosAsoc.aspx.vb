Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME


Partial Class frm_AdminCatalogosAsoc
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ASOC_CATA"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub actualizaIndicadores()
        Dim ChkListItem As New ListItem
        Dim i As Integer = 0
        Dim Checked As Boolean
        Using dbEntities As New dbRMS_JIEntities
            Dim idCatalogo = Convert.ToInt32(Me.cmbCatalogo.SelectedValue)
            Dim oCatalogo = dbEntities.t_CatalogoInstrumento.Where(Function(p) p.id_CatalogoMaster = idCatalogo).ToList()
            For Each ChkListItem In Me.chkInstrumentos.Items
                Checked = False
                For Each item In oCatalogo
                    If ChkListItem.Value = item.id_instrumento Then
                        Checked = True
                        ChkListItem.Attributes.CssStyle.Add("background-color", "#F7E56B")
                    End If
                Next
                ChkListItem.Selected = Checked
                'ChkListItem.Attributes.Add("OnClick", "return cambiarColorFondo(1,this,'#F7E56B','#E8E8E0',document.aspnetForm." & Me.chkInstrumentos.ClientID.Trim & "_" & i.ToString & ")")
                'ChkListItem.Attributes.Add("OnMouseover", "return cambiarColorFondo(1,this,'#F7E56B','#E8E8E0',document.aspnetForm." & Me.chkInstrumentos.ClientID.Trim & "_" & i.ToString & ")")
                'ChkListItem.Attributes.Add("OnMouseout", "return cambiarColorFondo(2,this,'#F7E56B','#E8E8E0',document.aspnetForm." & Me.chkInstrumentos.ClientID.Trim & "_" & i.ToString & ")")
                i += 1
            Next
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
            FillData()
            actualizaIndicadores()
        Else
            'Me.RadNotification1.VisibleOnPageLoad = False
        End If
    End Sub

    Protected Sub cmbCatalogo_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbCatalogo.SelectedIndexChanged
        actualizaIndicadores()
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Dim ChkListItem As New ListItem
        Dim i = 0
        Dim seleccion As Boolean = False
        Dim Sql As String = ""
        i = 0
        Using dbEntities As New dbRMS_JIEntities
            For Each ChkListItem In Me.chkInstrumentos.Items

                Dim idCatalogo = Convert.ToInt32(Me.cmbCatalogo.SelectedValue)
                Dim idInstrumento = Convert.ToInt32(ChkListItem.Value)
                Dim oCatalogos = dbEntities.t_CatalogoInstrumento.Where(Function(p) p.id_CatalogoMaster = idCatalogo And p.id_instrumento = idInstrumento).ToList()

                If oCatalogos.Count > 0 And ChkListItem.Selected = False Then
                    Sql = "DELETE FROM t_CatalogoInstrumento WHERE id_CatalogoMaster=" & Me.cmbCatalogo.SelectedValue & " AND id_instrumento=" & ChkListItem.Value
                    dbEntities.Database.ExecuteSqlCommand(Sql)
                ElseIf oCatalogos.Count > 0 And ChkListItem.Selected = True Then
                    'NADA
                ElseIf oCatalogos.Count = 0 And ChkListItem.Selected = True Then
                    Dim oCatalogo = New t_CatalogoInstrumento
                    oCatalogo.id_instrumento = Convert.ToInt32(ChkListItem.Value)
                    oCatalogo.id_CatalogoMaster = Convert.ToInt32(Me.cmbCatalogo.SelectedValue)
                    dbEntities.t_CatalogoInstrumento.Add(oCatalogo)
                End If
            Next
            dbEntities.SaveChanges()
        End Using
        'actualizaIndicadores()
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Administracion/frm_admincatalogo"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        'Me.RadNotification1.VisibleOnPageLoad = True
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_AdminCatalogo"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Sub FillData()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        Me.cmbCatalogo.DataSource = clListados.get_t_admincatalogos(id)
        Me.cmbCatalogo.DataTextField = "TipoCatalogo"
        Me.cmbCatalogo.DataValueField = "id_CatalogoMaster"
        Me.cmbCatalogo.DataBind()

        Me.chkInstrumentos.DataSource = clListados.get_tme_instrumentos(id)
        Me.chkInstrumentos.DataTextField = "nombre_instrumento"
        Me.chkInstrumentos.DataValueField = "id_instrumento"
        Me.chkInstrumentos.DataBind()
    End Sub
End Class
