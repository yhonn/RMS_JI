Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Drawing
Imports System.Net.Mail
Imports System.Net
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports ly_APPROVAL


Namespace OrgChart.IntegrationWithRadToolTip

    Partial Class frm_HistorialComentarios
        Inherits System.Web.UI.Page

        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Private isDrillDown As Boolean = False


        Dim clss_approval As APPROVAL.clss_approval

        Dim cl_user As ly_SIME.CORE.cls_user
        Dim frmCODE As String = "AP_APPROVAL_FLOW"
        Dim controles As New ly_SIME.CORE.cls_controles



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                If Me.Session("E_IdUser").ToString = "" Then
                End If
            Catch ex As Exception
                Me.Response.Redirect("~/frm_login.aspx")
            End Try

            If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
                cl_user = Session.Item("clUser")
                If Not cl_user.chk_accessMOD(0, frmCODE) Then
                    Me.Response.Redirect("~/Proyectos/no_access2")
                Else
                    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                    'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                    cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
                End If
                controles.code_mod = frmCODE
                For Each Control As Control In Page.Controls
                    controles.checkControls(Control, cl_user.id_idioma, cl_user)
                Next
            End If

            If Not IsPostBack Then

                Me.lblIDocumento.Text = Me.Request.QueryString("IdDoc").ToString

                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

                Dim tbl_Doc As New DataTable
                tbl_Doc = clss_approval.get_DocumentINFO(Me.lblIDocumento.Text)


                Dim USerAllowed As String() = tbl_Doc.Rows.Item(0).Item("IdUserParticipate").ToString.Split(",")
                Dim indx As Integer = USerAllowed.ToList().IndexOf(Me.Session("E_IDUser"))
                Dim boolAcces As Boolean = Convert.ToBoolean(Val(Me.h_Filter.Value))

                If (indx = -1) Then '--The User is not Allowed
                    If Not boolAcces Then
                        Me.Response.Redirect("~/Proyectos/no_access2_app")
                    End If
                End If


                Me.lblIdRuta.Text = tbl_Doc.Rows.Item(0).Item("id_ruta").ToString

                Me.lbl_categoria.Text = tbl_Doc.Rows.Item(0).Item("descripcion_cat").ToString
                Me.lbl_aprobacion.Text = tbl_Doc.Rows.Item(0).Item("descripcion_aprobacion").ToString
                Me.lbl_nivelaprobacion.Text = tbl_Doc.Rows.Item(0).Item("nivel_aprobacion").ToString
                Me.lbl_condicion.Text = tbl_Doc.Rows.Item(0).Item("condicion").ToString
                Me.lbl_proceso.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString
                Me.lbl_instrumento.Text = tbl_Doc.Rows.Item(0).Item("numero_instrumento").ToString
                Me.lbl_beneficiario.Text = tbl_Doc.Rows.Item(0).Item("nom_beneficiario").ToString

                Me.lbl_codigo.Text = tbl_Doc.Rows.Item(0).Item("codigo_AID").ToString

                Me.lbl_datecreated.Text = tbl_Doc.Rows.Item(0).Item("datecreated").ToString '.Format("{0:dd/MM/yyyy hh:mm:ss}")
                Me.lbl_IdCodigoAPP.Text = tbl_Doc.Rows.Item(0).Item("codigo_Approval").ToString
                Me.lbl_IdCodigoSAP.Text = tbl_Doc.Rows.Item(0).Item("codigo_SAP_APP").ToString
                Me.lbl_status.Text = tbl_Doc.Rows.Item(0).Item("descripcion_estado").ToString
                'Me.lbl_region.Text = tbl_Doc.Rows.Item(0).Item("regional").ToString

                Me.lbl_tipoDocumento.Text = tbl_Doc.Rows.Item(0).Item("nombreTipoAprobacion").ToString
                'Me.lbl_montoProyecto.Text = tbl_Doc.Rows.Item(0).Item("monto_ficha").ToString
                'Me.lbl_montoTotal.Text = tbl_Doc.Rows.Item(0).Item("monto_total").ToString
                'Me.lbl_tasaCambio.Text = tbl_Doc.Rows.Item(0).Item("tasa_cambio").ToString

                Me.lbl_createdby.Text = tbl_Doc.Rows.Item(0).Item("Originador").ToString
                Me.lbl_approvedby.Text = tbl_Doc.Rows.Item(0).Item("propietario").ToString
                Me.lbl_dateapproved.Text = tbl_Doc.Rows.Item(0).Item("fecha_aprobacion").ToString.Substring(0, 10)

                cnnSAP.Open()

                Dim strSQL As String = String.Format("SELECT ROW_NUMBER() OVER (ORDER BY id_App_documento) AS IdReg,         " &
                                                      " CASE WHEN ROW_NUMBER() OVER (ORDER BY id_App_documento) = 1          " &
                                                      "   THEN NULL                                                          " &
                                                      " Else ROW_NUMBER() OVER (ORDER BY id_App_documento) -1 End As Idpadre,   " &
                                                      "  nombre_rol +' (<a target=_blank alt=See details href=frm_seguimientoNotificacion.aspx?IdDoc=" & Me.lblIDocumento.Text & "&&IdRuta='+ CAST( id_ruta AS varchar) + '>'+ descripcion_estado +'</a>)' as Titulo,'<img src=../Imagenes/Iconos/Circle_'+ Alerta +'.png  height=16 width=16 />&nbsp;&nbsp;<b>'+ nombre_empleado +'</b><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+  fecha_aprobacion + '<br /><br />' + isnull(SUBSTRING(USR_Observacion,0,200) + '...','') as Detalle " &
                                                      " FROM dbo.FN_Ta_RutaSeguimiento({0})   " &
                                                      " ORDER BY id_App_documento", Me.lblIDocumento.Text)

                ' Dim dmm As New SqlDataAdapter("SELECT (orden+1) as IdReg, CASE WHEN orden = 0 THEN NULL ELSE orden END AS Idpadre, nombre_rol +' (<a target=_blank alt=See details href=frm_seguimientoNotificacion.aspx?IdDoc=" & Me.lblIDocumento.Text & "&&IdRuta='+ CAST( id_ruta AS varchar) + '>'+ descripcion_estado +'</a>)' as Titulo,'<img src=../Imagenes/Iconos/Circle_'+ Alerta +'.png  height=16 width=16 />&nbsp;&nbsp;'+ nombre_empleado +'<br /><br /><b>'+  fecha_aprobacion + '</b><br /><br />' + isnull( USR_Observacion,'') as Detalle FROM dbo.FN_Ta_RutaSeguimiento(" & Me.lblIDocumento.Text & ") ORDER BY ORDEN", cnnSAP)
                Dim dmm As New SqlDataAdapter(strSQL, cnnSAP)
                Dim dst As New DataSet("DsMaster")
                dmm.Fill(dst, "DsMaster")

                RadOrgChartML.GroupEnabledBinding.NodeBindingSettings.DataFieldID = "IdReg"
                RadOrgChartML.GroupEnabledBinding.NodeBindingSettings.DataFieldParentID = "Idpadre"
                RadOrgChartML.GroupEnabledBinding.NodeBindingSettings.DataSource = dst.Tables("DsMaster")

                RadOrgChartML.GroupEnabledBinding.GroupItemBindingSettings.DataFieldNodeID = "IdReg"
                RadOrgChartML.GroupEnabledBinding.GroupItemBindingSettings.DataFieldID = "IdReg"
                RadOrgChartML.GroupEnabledBinding.GroupItemBindingSettings.DataSource = dst.Tables("DsMaster")



                '    chart.Margin = New Thickness(0);
                'chart.Padding = New Thickness(0);

                cnnSAP.Close()
                RadOrgChartML.DataBind()
            End If
        End Sub

        Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
            Me.Response.Redirect("frm_seguimientoNotificacion.aspx?IdDoc=" & Me.lblIDocumento.Text & "&&IdRuta=" & Me.lblIDruta.Text)
        End Sub

        Private Sub RadOrgChartML_NodeDataBound(sender As Object, e As OrgChartNodeDataBoundEventArguments) Handles RadOrgChartML.NodeDataBound

            'newItemSizeSimple

            For Each groupItem As Object In e.Node.GroupItems
                groupItem.CssClass = "newItemSizeSimple"

            Next


        End Sub

        'Protected Sub RadOrgChartML_GroupItemDataBound(sender As Object, e As Telerik.Web.UI.OrgChartGroupItemDataBoundEventArguments) Handles RadOrgChartML.GroupItemDataBound

        '    'Dim panel = DirectCast(e.Item.FindControl("Panel1"), Panel)
        '    'RadToolTipManager1.TargetControls.Add(panel.ClientID, e.Item.Node.ID, True)

        '    For Each groupItem As Object In e.Item.Node.GroupItems
        '        groupItem.CssClass = "newItemSizeSimple1"
        '        ' hart.DefaultView.ChartLegend.Padding = New Thickness(0);
        '    Next

        'End Sub


        'Public Sub OnAjaxUpdate(sender As Object, e As ToolTipUpdateEventArgs) Handles RadToolTipManager1.AjaxUpdate
        '    Dim ctrl As Control = Page.LoadControl("TooltOBservVB.ascx")
        '    ctrl.ID = "UcEmployeeDetails1"
        '    '~/Aprobaciones/

        '    e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl)
        '    Dim details As TooltOBservVB = DirectCast(ctrl, TooltOBservVB)
        '    details.EmployeeID = e.Value

        'End Sub

        'Private Sub RadOrgChart1_DrillDown(sender As Object, e As OrgChartDrillDownEventArguments) Handles RadOrgChartML.DrillDown
        '    isDrillDown = True
        'End Sub

        'Protected Overrides Sub OnPreRenderComplete(ByVal e As EventArgs)
        '    MyBase.OnPreRenderComplete(e)

        '    If isDrillDown Then
        '        Dim nodes = RadOrgChartML.GetAllNodes()

        '        For Each node As OrgChartNode In nodes
        '            Dim panel = DirectCast(node.GroupItems(0).FindControl("Panel1"), Panel)
        '            RadToolTipManager1.TargetControls.Add(panel.ClientID, node.ID, True)
        '        Next
        '    End If
        'End Sub


        'Protected Sub RadOrgChartML_GroupItemDataBound(sender As Object, e As Telerik.Web.UI.OrgChartGroupItemDataBoundEventArguments) Handles RadOrgChartML.GroupItemDataBound
        '    Dim panel = DirectCast(e.Item.FindControl("Panel1"), Panel)
        '    RadToolTipManager1.TargetControls.Add(panel.ClientID, e.Item.Node.ID, True)
        'End Sub

    End Class

End Namespace