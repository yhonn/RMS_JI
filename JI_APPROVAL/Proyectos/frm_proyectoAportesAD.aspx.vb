Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager

Public Class frm_proyectoAportesAD
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\"

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "EDIT_APORT"
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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            LoadData()
            CargarDatos(Me.Request.QueryString("Id").ToString)
        End If

    End Sub

    Sub LoadData()
        Using dbEntities As New dbRMS_HNEntities
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id"))
            Dim oProyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)

            Me.lbl_id_proyecto.Text = id
            Me.lbl_codigo_ficha.Text = oProyecto.codigo_SAPME
            Me.lbl_nombre_ficha.Text = oProyecto.nombre_proyecto
            Me.lbl_nombre_ejecutor.Text = oProyecto.nombre_ejecutor
            Me.lbl_estado.Text = oProyecto.nombre_estado_ficha
            Me.lbl_nombre_subregion.Text = oProyecto.nombre_subregion
            Me.lbl_fecha_actualizacion.Text = Date.UtcNow
            Me.txttasacambio.Value = oProyecto.tasa_cambio
        End Using
    End Sub

    Sub CargarDatos(ByVal id_proyecto As String)
        Dim dm As New SqlDataAdapter("SELECT * FROM vw_tme_Ficha_Proyecto WHERE id_ficha_proyecto=" & id_proyecto, cnnME)
        Dim ds As New DataSet("DsFichaProyecto")
        dm.Fill(ds, "DsFichaProyecto")
        'Me.lbl_fecha.Text = Now()
        'Me.lbl_id_proyecto.Text = id_proyecto
        'Me.lbl_codigo_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("codigo_SAPME")
        'Me.lbl_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("nombre_proyecto")
        'Me.lbl_ejecutor.Text = ds.Tables("DsFichaProyecto")(0)("nombre_ejecutor")
        'Me.lbl_componente.Text = ds.Tables("DsFichaProyecto")(0)("nombre_componente")
        'Me.lbl_region.Text = ds.Tables("DsFichaProyecto")(0)("Region")
        'Me.lbl_estado.Text = ds.Tables("DsFichaProyecto")(0)("nombre_estado_ficha")
        'Me.txttasacambio.Value = FormatNumber(Val(ds.Tables("DsFichaProyecto")(0)("tasa_cambio")), 2).ToString
        Me.grd_Aportes.DataBind()

        Dim itemD As GridDataItem

        For Each row In Me.grd_Aportes.Items

            itemD = CType(row, GridDataItem)
            'Dim TotalAporte As RadNumericTextBox = CType(row.Cells(0).FindControl("TotalAporte"), RadNumericTextBox)
            Dim TotalAporte As RadNumericTextBox = CType(itemD.FindControl("TotalAporte"), RadNumericTextBox)
            ' TotalAporte.Value = Val(row.Cells(7).Text.Trim)
            TotalAporte.Value = itemD("monto_aporte_obligado").Text
        Next

        ActualizarSuma()
    End Sub
    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click

        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False
        Dim ItemD As GridDataItem

        'Me.lbl_errorAportes.Visible = False

        If Val(lblMontototal.Text) = 0 Then
            'Me.lbl_errorAportes.Visible = True
            errSave = True
        End If

        If errSave = False Then

            'Try
            cnnME.Open()
            Dim totalProyecto As New RadNumericTextBox
            totalProyecto.Value = Me.lblMontototal.Text

            ' Dim dmm As New SqlCommand(String.Format(" UPDATE tme_Ficha_Proyecto SET costo_total_proyecto={0},tasa_cambio={1},fechamodificacion_MontoObligado=GETDATE(),aportes_actualizados='SI' WHERE id_ficha_proyecto = {2}", Val(totalProyecto.Value).ToString.Replace(",", "."), Val(Me.txttasacambio.Value).ToString.Replace(",", "."), Me.lbl_id_proyecto.Text), cnnME)
            Dim dmm As New SqlCommand()
            dmm.Connection = cnnME
            Sql = String.Format(" UPDATE tme_Ficha_Proyecto SET costo_total_proyecto={0},tasa_cambio={1},fechamodificacion_MontoObligado=GETDATE(),aportes_actualizados='SI' WHERE id_ficha_proyecto = {2}", Val(totalProyecto.Value).ToString.Replace(",", "."), Val(Me.txttasacambio.Value).ToString.Replace(",", "."), Me.lbl_id_proyecto.Text)
            dmm.CommandText = Sql
            dmm.ExecuteNonQuery()

            ''********* Aportes *********
            For Each row In Me.grd_Aportes.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim idAporte As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_aporteFicha"))
                    ItemD = CType(row, GridDataItem)
                    'Dim idAporte As Integer = row.Cells(2).Text.Trim
                    'Dim TotalAporte As RadNumericTextBox = CType(row.Cells(0).FindControl("TotalAporte"), RadNumericTextBox)
                    Dim TotalAporte As RadNumericTextBox = CType(ItemD.FindControl("TotalAporte"), RadNumericTextBox)
                    Dim cmbINDICADOR As RadComboBox = CType(ItemD.FindControl("cmdIndicador"), RadComboBox)

                    Sql = String.Format("UPDATE tme_AportesFicha SET monto_aporte_obligado ={0}, id_indicador={1} WHERE id_aporteFicha={2} ", Val(TotalAporte.Value).ToString.Replace(",", "."), cmbINDICADOR.SelectedValue, idAporte)
                    'dmm.CommandText = "UPDATE tme_AportesFicha SET monto_aporte_obligado =" & Val(TotalAporte.Value).ToString.Replace(",", ".") & ", id_indicador=" & cmbINDICADOR.SelectedValue & " WHERE id_aporteFicha=" & idAporte
                    dmm.CommandText = Sql
                    dmm.ExecuteNonQuery()
                End If
            Next

            Sql = "DELETE FROM tme_AportesFicha WHERE id_ficha_proyecto IN  ( SELECT id_ficha_proyecto"
            Sql &= "  FROM tme_Ficha_Proyecto WHERE aportes_actualizados = 'SI' ) AND monto_aporte = 0 AND monto_aporte_obligado = 0"
            dmm.CommandText = Sql
            dmm.ExecuteNonQuery()

            cnnME.Close()

            'Catch ex As Exception
            'Me.lbl_errorSQL.Visible = True
            'Me.lbl_errorSQL.Text = ex.Message.Trim & ":" & Sql
            ' End Try

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosCuadroMando"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If


    End Sub
    Sub ActualizarSuma()
        Dim Total As Double = 0
        Dim itemD As GridDataItem

        For Each row In Me.grd_Aportes.Items

            itemD = CType(row, GridDataItem)
            'Dim TotalAporte As RadNumericTextBox = CType(row.Cells(0).FindControl("TotalAporte"), RadNumericTextBox)
            Dim TotalAporte As RadNumericTextBox = CType(itemD.FindControl("TotalAporte"), RadNumericTextBox)

            'If row.Cells(8).Text.Trim = "SI" Then
            If itemD("calcular_suma").Text = "SI" Then
                Total = Total + Val(TotalAporte.Text)
            End If

        Next

        Me.lblMontototal.Text = FormatNumber(Total.ToString, 2, , TriState.True, TriState.True).ToString

    End Sub
    Protected Sub txt_Aporte_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizarSuma()
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/proyectos/frm_proyectoAportes"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    'Protected Sub btn_aceptarG0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_aceptarG0.Click
    '    Me.Response.Redirect("~/proyectos/frm_proyectoAportes")
    'End Sub

    'Protected Sub btn_aceptarG1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_aceptarG1.Click
    '    Me.Response.Redirect("~/proyectos/frm_proyectoAportes")
    'End Sub

    Private Sub grd_Aportes_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grd_Aportes.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim cmbINDICADOR As RadComboBox = CType(e.Item.Cells(1).FindControl("cmdIndicador"), RadComboBox)
            cmbINDICADOR.SelectedValue = itemD("id_indicador").Text.Trim

        End If

    End Sub

End Class