Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class frm_ActivitySTO
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "ACTIVITY_STO"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
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

            Me.btn_eliminarAportes.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not Me.IsPostBack Then

            Using dbEntities As New dbRMS_JIEntities

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Dim contract_Type As String = If(IsNothing(Me.Request.QueryString("tp")), "STO", Me.Request.QueryString("tp").ToString)

                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)

                Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
                Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString()))

                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)
                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then
                    Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & id.ToString()))
                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & id.ToString()))
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & id.ToString()))
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")
                Else
                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                End If




                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************
                Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)

                Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

                Me.alink_stos.Attributes.Add("style", "display:none;")
                Me.alink_po.Attributes.Add("style", "display:none;")
                Me.alink_Ik.Attributes.Add("style", "display:none;")

                Me.alink_stos.Attributes.Add("href", "#")
                Me.alink_po.Attributes.Add("href", "#")
                Me.alink_Ik.Attributes.Add("href", "#")

                Dim i = 0
                If oSubFather.count > 0 Then

                    For Each item In oSubFather

                        If i = 0 Then
                            Me.alink_stos.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_stos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                            Me.alink_stos.Attributes.Add("style", "display:block;")
                        ElseIf i = 1 Then
                            Me.alink_po.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_po.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                            Me.alink_po.Attributes.Add("style", "display:block;")
                        Else
                            Me.alink_Ik.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=IK"))
                            Me.alink_Ik.Attributes.Add("style", "display:block;")
                            Me.alink_Ik.Attributes.Add("style", "display:block;")
                        End If

                        i += 1

                    Next

                End If

                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "PO" Then
                '    Me.alink_areas.Attributes.Add("href", "#")
                '    Me.alink_areas.Attributes.Add("style", "display:none;")
                '    Me.alink_indicadores.Attributes.Add("href", "#")
                '    Me.alink_indicadores.Attributes.Add("style", "display:none;")
                '    Me.alink_waiver.Attributes.Add("href", "#")
                '    Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************




            End Using



            LoadData()



        End If

    End Sub


    Public Sub LoadData()

        Using dbEntities As New dbRMS_JIEntities

            'If proyecto.tasa_cambio_trimestre IsNot Nothing Then
            '    Me.tasaCambio.Value = proyecto.tasa_cambio_actividad
            '    Me.lbl_tasa_cambio.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", Convert.ToDecimal(proyecto.tasa_cambio))
            'End If

            Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
            Me.lbl_id_ficha.Text = id

            'Dim contract_Type As String = If(IsNothing(Me.Request.QueryString("tp")), "STO", Me.Request.QueryString("tp").ToString)
            Dim contract_Type As String = Me.Request.QueryString("tp")
            Me.hd_tp_activity.Value = contract_Type

            Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
            Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto

            Me.lbl_activity_Code.Text = proyecto.codigo_SAPME
            Me.lbl_activity_name.Text = proyecto.nombre_proyecto
            Me.lbl_implementer.Text = proyecto.ORGANIZATIONNAME

            Me.lbl_last_Deliverable.Text = proyecto.STATUS
            Me.lbl_period.Text = String.Format("{0:dd/MM/yyyy} to {1:dd/MM/yyyy}", proyecto.fecha_inicio_proyecto, proyecto.fecha_fin_proyecto)

            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

            Me.lbl_total_type.Text = proyecto.prefijo_mecanismo
            Me.lbl_total_tipo_2.Text = proyecto.prefijo_mecanismo

            'Dim vAportes_USD As Double = If(Me.hd_tp_activity.Value = "STO", proyecto.c_Aportes_ProyectoUSD, If(Me.hd_tp_activity.Value = "INK", proyecto.c_Aportes_ProyectoUSD_2, proyecto.c_Aportes_ProyectoUSD_3))
            Dim vAportes_USD As Double = If(IsNothing(proyecto.OBLIGATED_AMOUNT), 0, proyecto.OBLIGATED_AMOUNT)
            'Dim vAportes As Double = If(Me.hd_tp_activity.Value = "STO", proyecto.c_Aportes_Proyecto, If(Me.hd_tp_activity.Value = "INK", proyecto.c_Aportes_Proyecto_2, proyecto.c_Aportes_Proyecto_3))
            Dim vAportes As Double = If(IsNothing(proyecto.OBLIGATED_AMOUNT_LOCAL), 0.0, proyecto.OBLIGATED_AMOUNT_LOCAL)

            Me.lbl_totalACT2.Text = String.Format("{0:N2} USD", vAportes_USD)
            Me.lbl_totalACT2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", vAportes, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)

            Dim oProyecto = dbEntities.VW_TA_ACTIVITY.Where(Function(f) f.id_ficha_padre = proyecto.id_activity)

            Dim vPerform As Double = 0
            Dim vPerformUSD As Double = 0

            If (oProyecto.Count() > 0) Then

                '(oProyecto.Count() > 0, oProyecto.Sum(Function(s) s.c_Aportes_Proyecto), 0)
                '' If (oProyecto.Count() > 0, oProyecto.Sum(Function(s) s.c_Aportes_ProyectoUSD), 0) 

                'vPerform = If(Me.hd_tp_activity.Value = "STO", oProyecto.Sum(Function(s) s.c_Aportes_Proyecto), If(Me.hd_tp_activity.Value = "INK", oProyecto.Sum(Function(s) s.c_Aportes_Proyecto_2), oProyecto.Sum(Function(s) s.c_Aportes_Proyecto_3)))
                'vPerformUSD = If(Me.hd_tp_activity.Value = "STO", oProyecto.Sum(Function(s) s.c_Aportes_ProyectoUSD), If(Me.hd_tp_activity.Value = "INK", oProyecto.Sum(Function(s) s.c_Aportes_ProyectoUSD_2), oProyecto.Sum(Function(s) s.c_Aportes_ProyectoUSD_3)))

                vPerform = oProyecto.Sum(Function(s) s.OBLIGATED_AMOUNT_LOCAL)
                vPerformUSD = oProyecto.Sum(Function(s) s.OBLIGATED_AMOUNT)

            Else

                vPerform = 0
                vPerformUSD = 0

            End If

            Me.lbl_totalPerf2.Text = String.Format("{0:N2} USD", vPerformUSD)
            Me.lbl_totalPerf2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", vPerform, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)

            Dim strActiveTAB As String = ""

            If (oProyecto.Count() > 0) Then

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)

                Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

                Me.alink_stos.Attributes.Add("style", "display:none;")
                Me.alink_po.Attributes.Add("style", "display:none;")
                Me.alink_Ik.Attributes.Add("style", "display:none;")

                Me.alink_stos.Attributes.Add("href", "#")
                Me.alink_po.Attributes.Add("href", "#")
                Me.alink_Ik.Attributes.Add("href", "#")

                Me.alink_stos.Attributes("class") = If(Me.alink_stos.Attributes("class") Is Nothing, "", Me.alink_stos.Attributes("class").Replace("primary", ""))
                Me.alink_po.Attributes("class") = If(Me.alink_po.Attributes("class") Is Nothing, "", Me.alink_po.Attributes("class").Replace("primary", ""))
                Me.alink_Ik.Attributes("class") = If(Me.alink_Ik.Attributes("class") Is Nothing, "", Me.alink_Ik.Attributes("class").Replace("primary", ""))


                If oSubFather.count > 0 Then

                    For Each item In oSubFather

                        If oPro.tme_sub_mecanismo.tme_mecanismo_contratacion.prefijo_mecanismo = "SUB" And Me.hd_tp_activity.Value = "STO" Then

                            Me.alink_stos.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_stos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                            Me.alink_stos.Attributes.Add("style", "display:block;")

                            If (item.perfijo_sub_mecanismo = Me.hd_tp_activity.Value) Then

                                strActiveTAB = String.Format("$('#{0}').addClass('active');", "t_STO")
                                Me.alink_stos.Attributes("class") = "primary"

                            End If

                        ElseIf oPro.tme_sub_mecanismo.tme_mecanismo_contratacion.prefijo_mecanismo = "GRA" And item.perfijo_sub_mecanismo = "PO" Then

                            Me.alink_po.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_po.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                            Me.alink_po.Attributes.Add("style", "display:block;")

                            If (item.perfijo_sub_mecanismo = Me.hd_tp_activity.Value) Then
                                strActiveTAB = String.Format("$('#{0}').addClass('active');", "t_PO")
                                Me.alink_po.Attributes("class") = "primary"
                            End If

                        ElseIf oPro.tme_sub_mecanismo.tme_mecanismo_contratacion.prefijo_mecanismo = "GRA" And item.perfijo_sub_mecanismo = "INK" Then

                            Me.alink_Ik.InnerText = item.perfijo_sub_mecanismo
                            Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                            Me.alink_Ik.Attributes.Add("style", "display:block;")

                            If (item.perfijo_sub_mecanismo = Me.hd_tp_activity.Value) Then
                                strActiveTAB = String.Format("$('#{0}').addClass('active');", "t_INK")
                                Me.alink_Ik.Attributes("class") = "primary"
                            End If

                        End If

                    Next



                End If


                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "PO" Then
                '    Me.alink_areas.Attributes.Add("href", "#")
                '    Me.alink_areas.Attributes.Add("style", "display:none;")
                '    Me.alink_indicadores.Attributes.Add("href", "#")
                '    Me.alink_indicadores.Attributes.Add("style", "display:none;")
                '    Me.alink_waiver.Attributes.Add("href", "#")
                '    Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************
            Else

                Me.alink_stos.Attributes.Add("style", "display:none;")
                Me.alink_po.Attributes.Add("style", "display:none;")
                Me.alink_Ik.Attributes.Add("style", "display:none;")

                Me.alink_stos.Attributes.Add("href", "#")
                Me.alink_po.Attributes.Add("href", "#")
                Me.alink_Ik.Attributes.Add("href", "#")


                If Me.hd_tp_activity.Value = "STO" Then
                    Me.alink_stos.InnerText = Me.hd_tp_activity.Value
                    Me.alink_stos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & Me.hd_tp_activity.Value))
                    Me.alink_stos.Attributes.Add("style", "display:block;")
                ElseIf Me.hd_tp_activity.Value = "PO" Then
                    Me.alink_po.InnerText = Me.hd_tp_activity.Value
                    Me.alink_po.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & Me.hd_tp_activity.Value))
                    Me.alink_po.Attributes.Add("style", "display:block;")
                ElseIf Me.hd_tp_activity.Value = "INK" Then
                    Me.alink_Ik.InnerText = Me.hd_tp_activity.Value
                    Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySTO?Id=" & id.ToString() & "&tp=" & Me.hd_tp_activity.Value))
                    Me.alink_Ik.Attributes.Add("style", "display:block;")
                End If

            End If


            Dim fechaHoy As Date = Date.Now
            Dim Tot_last As Long = DateDiff(DateInterval.Day, CDate(proyecto.fecha_inicio_proyecto), CDate(proyecto.fecha_fin_proyecto))
            Dim dateFinish As Date

            If proyecto.ID_ACTIVITY_STATUS = 19 Or proyecto.ID_ACTIVITY_STATUS = 22 Then '--Finish

                'If dbEntities.tme_ficha_historico_estado.Where(Function(f) f.id_ficha_proyecto = proyecto.id_ficha_proyecto And (f.id_ficha_estado = 3 Or f.id_ficha_estado = 6)).Count() > 0 Then
                '    dateFinish = dbEntities.tme_ficha_historico_estado.Where(Function(f) f.id_ficha_proyecto = proyecto.id_ficha_proyecto And (f.id_ficha_estado = 3 Or f.id_ficha_estado = 6)).FirstOrDefault.fecha
                'Else
                '    dateFinish = CDate(proyecto.fecha_fin_proyecto)
                'End If

                dateFinish = CDate(proyecto.fecha_fin_proyecto)

            Else
                dateFinish = Date.Now
            End If

            Dim Tot_Current = DateDiff(DateInterval.Day, CDate(proyecto.fecha_inicio_proyecto), dateFinish)
            Dim diff_Date As Double = If(Tot_last > 0, Math.Round((Tot_Current / Tot_last) * 100, 2, MidpointRounding.AwayFromZero), 0)

            ' Dim Tot_Proyecto_USD As Double = If(Me.hd_tp_activity.Value = "STO", CDbl(proyecto.c_Aportes_ProyectoUSD), If(Me.hd_tp_activity.Value = "INK", CDbl(proyecto.c_Aportes_ProyectoUSD_2), CDbl(proyecto.c_Aportes_ProyectoUSD_3)))
            Dim Tot_Proyecto_USD As Double = If(IsNothing(proyecto.OBLIGATED_AMOUNT), 0, proyecto.OBLIGATED_AMOUNT)

            Dim diff_money As Double = If(Tot_Proyecto_USD = 0, 0, Math.Round((vPerformUSD / Tot_Proyecto_USD) * 100, 0, MidpointRounding.AwayFromZero))
            'String.Format("{0:N2} USD", YLAfundingUSD) & "||" &
            'String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", YLAfunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &

            Dim strArray As String() = Get_Ficha_Proyecto_Hijos(id, Me.hd_tp_activity.Value).Split("||")

            ltr_rows_Deliverables.InnerHtml = strArray(0)

            lbl_totalACT_usd.Text = strArray(2)
            lbl_totalACT.Text = strArray(4)

            lbl_totalPend_usd.Text = strArray(6)
            lbl_totalPend.Text = strArray(8)

            lbl_totalPerf_usd.Text = strArray(10)
            lbl_totalPerf.Text = strArray(12)

            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_IQS_time(" & diff_Date.ToString & ",'" & proyecto.prefijo_mecanismo & "'); set_Chart_IQS(" & diff_money.ToString & ",'" & proyecto.prefijo_mecanismo & "');  set_Percent(" & If(diff_money > 100, "100", diff_money.ToString) & "); " & strActiveTAB & " ", True)
            ' ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_IQS(" & diff_money.ToString & ");", True)

        End Using

    End Sub




    Private Function Get_Ficha_Proyecto_Hijos(ByVal idFichaProyecto_padre As Integer, ByVal tp_childs As String) As String

        'Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDPrograma")))

        Using dbEntities As New dbRMS_JIEntities



            'Dim cls_Ficha = New RMS_SIME.cls_ficha_proyectos(Convert.ToInt32(Me.Session("E_IDPrograma")))

            'Dim contextVar As HttpContext = HttpContext.Current
            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

            'Dim ExchangeRate As Double = cls_Deliverable.get_ExchangeRate()

            'Dim tbl_Proyectos As DataTable = cls_Ficha.get_Ficha_proyecto_Childs(idFichaProyecto_padre, tp_childs)
            Dim tbl_childs = dbEntities.VW_TA_ACTIVITY_FATHER.Where(Function(p) p.id_Ficha_padre = idFichaProyecto_padre).ToList
            'Dim oProy = dbEntities.TA_ACTIVITY.Where(Function(p) p.id_activity = idFichaProyecto_padre).FirstOrDefault

            Dim strType_Docs As String
            If tbl_childs.Count > 0 Then

                'strType_Docs = tbl_Proyectos.Rows.Item(0).Item("perfijo_sub_mecanismo")
                strType_Docs = tbl_childs.FirstOrDefault.perfijo_sub_mecanismo
                'Me.lbl_total_tipo_3.Text = tbl_Proyectos.Rows.Item(0).Item("nombre_sub_mecanismo")
                Me.lbl_total_tipo_3.Text = tbl_childs.FirstOrDefault.nombre_sub_mecanismo

            Else
                strType_Docs = "NONE"
                Me.lbl_total_tipo_3.Text = "NONE"
            End If



            Dim strRowsTOT As String = ""
            'Dim strRows As String = "<tr>
            '                           <td><div class='tools'><a href='/RMS_SIME/Proyectos/frm_proyectosEdit?id={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
            '                           <td>{0}</td>
            '                           <td>
            '                              <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
            '                                  {1}
            '                              </div>
            '                           </td>
            '                           <td>
            '                              <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
            '                                 {2}
            '                              </div>
            '                           </td>
            '                           <td>{3:d}</td>
            '                           <td>{4:d}</td>
            '                           <td>{5:P2}</td>
            '                           <td>{6}</td>
            '                           <td>                                                                       
            '                             <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
            '                           </td>
            '                        </tr>"

            Dim strRows As String = "<tr>
                                    <td><div class='tools'><a href='/RMS_APPROVAL/RFP_/frm_ActivityE?Id={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
                                         <td>{0}</td>
                                         <td>{1}</td>
                                         <td>
                                           <div style='overflowscroll; text-align:Left(); max-width:100%; max-height:300px;'>
                                             {2}
                                          </div>
                                         </td>
                                         <td>{3:d}</td>
                                         <td>{4:d}</td>
                                         <td>{5:P2}</td>
                                         <td>{6}</td>
                                           <td>                                                                                                                    
                                             <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
                                          </td>
                                        </tr>"


            Dim strRows2 As String = " <tr>
                                       <td colspan='9'>                                                                     
                                          <div class='progress'>
                                              <div class='progress-bar {0} progress-bar-striped' role='progressbar' aria-valuenow='{1}' aria-valuemin='0' aria-valuemax='100' style='width: {1}%'>
                                                   <span >{1}% </span>
                                              </div> 
                                              <div class='progress-bar {2} progress-bar-striped' role='progressbar' aria-valuenow='{3}' aria-valuemin='0' aria-valuemax='100' style='width: {3}%'>
                                                    <span>{4}% </span>
                                              </div>                                                                            
                                          </div>
                                        </td>  
                                    </tr> "


            Dim strTableDEL As String = "<table class='table table-hover'>
                                                                <tr>
                                                                  <th>Deliverable #</th>
                                                                  <td><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                </tr>
                                                                <tr>                                                                  
                                                                  <td colspan='2'>
                                                                     <div style='text-align:left; max-width:100%;'>
                                                                         {1}
                                                                     </div>
                                                                   </td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                  <th>Due Date</th>
                                                                  <td>{2:d}</td>   
                                                                </tr>     
                                                                <tr>
                                                                  <th>Status</th>
                                                                  <td><span class='label {5}'>{3}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{4}</span></td>   
                                                                </tr>  
                                                                 <tr>
                                                                  <th>Porcent</th>
                                                                  <td>{6:P2}</td>   
                                                                </tr>   
                                                                <tr>
                                                                  <th>Amount</th>
                                                                  <td> {7} / {8:N2} USD</td>   
                                                                </tr>                                                               
                                                              </table>"

            'Dim strTable_nextDEL As String = ""
            Dim id_ficha_entregable As Integer = 0

            Dim strStatus As String = ""
            Dim strTime As String = ""
            Dim strAlert As String = ""

            Dim strAlert2 As String = ""
            Dim strAlert3 As String = ""
            Dim vDiferences As Double = 0
            Dim vDiferences_Adj As Double = 0
            Dim vDays As Double = 0

            Dim Father_funding As Double = 0
            Dim Father_fundingUSD As Double = 0
            Dim PerformedFunding As Double = 0
            Dim PerformedFundingUSD As Double = 0
            Dim PendingFunding As Double = 0
            Dim PendingFundingUSD As Double = 0
            Dim PorcenPerformed As Double = 0

            Dim tot_proyecto As Double = 0
            Dim tot_proyecto_padre As Double = 0
            'Dim IdMaxDeliverable As Integer = cls_Deliverable.get_Last_Deliverable(idFichaProyecto, 1).Rows.Item(0).Item("id_deliverable")

            For Each itemD In tbl_childs

                Dim rDays As Double

                'Father_funding = If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_Proyecto_padre"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_Proyecto_padre_2"), dtRow("c_Aportes_Proyecto_padre_3")))
                'Father_fundingUSD = If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_ProyectoUSD_padre"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_ProyectoUSD_padre_2"), dtRow("c_Aportes_ProyectoUSD_padre_3")))

                Father_funding = If(IsNothing(itemD.OBLIGATED_AMOUNT_LOCAL_padre), 0.0, itemD.OBLIGATED_AMOUNT_LOCAL_padre)
                Father_fundingUSD = If(IsNothing(itemD.OBLIGATED_AMOUNT_padre), 0.0, itemD.OBLIGATED_AMOUNT_padre)

                'PerformedFunding += If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_Proyecto"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_Proyecto_2"), dtRow("c_Aportes_Proyecto_3")))
                'PerformedFundingUSD += If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_ProyectoUSD"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_ProyectoUSD_2"), dtRow("c_Aportes_ProyectoUSD_3")))

                PerformedFunding += If(IsNothing(itemD.OBLIGATED_AMOUNT_LOC), 0.0, itemD.OBLIGATED_AMOUNT_LOC)
                PerformedFundingUSD += If(IsNothing(itemD.OBLIGATED_AMOUNT), 0.0, itemD.OBLIGATED_AMOUNT)

                ''dtRow("c_Aportes_ProyectoUSD")
                'If dtRow("id_deliverable_estado") = 3 Then
                '    PerformedFunding += dtRow("valor")
                'Else
                '    PendingFunding += dtRow("valor") 'Take in account when the value allocated it is not the planned
                'End If

                'If dtRow("D_Days") <= 0 Then 'its not time 
                If itemD.D_Days <= 0 Then 'its not time 

                    rDays = itemD.D_Days * -1

                    If itemD.ID_ACTIVITY_STATUS <> 19 Or itemD.ID_ACTIVITY_STATUS <> 22 Then 'Pending status

                        strStatus = itemD.STATUS
                        strTime = Func_Unit(rDays)
                        strAlert = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 1)

                    Else 'finish processes

                        strTime = Func_Unit(rDays)
                        strAlert = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 1)
                        strStatus = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 2)

                    End If

                    vDiferences = Math.Round(Convert.ToDouble(itemD.porc_Days - itemD.porc_EDays) * 100, 2, MidpointRounding.AwayFromZero)
                    vDays = (itemD.porc_Days * 100) - vDiferences
                    vDiferences_Adj = vDiferences

                Else 'Delayed time

                    If itemD.ID_ACTIVITY_STATUS <> 19 Or itemD.ID_ACTIVITY_STATUS <> 22 Then 'Pending status

                        strStatus = itemD.STATUS
                        strTime = Func_Unit(itemD.D_Days)
                        strAlert = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 1)

                    Else 'finish processes

                        strTime = Func_Unit(itemD.D_Days)
                        strAlert = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 1)
                        strStatus = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 2)

                    End If

                    vDays = itemD.porc_Days * 100
                    vDiferences = Math.Round(Convert.ToDouble(itemD.porc_EDays - itemD.porc_Days) * 100, 2, MidpointRounding.AwayFromZero)
                    vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)

                End If

                strAlert2 = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 3)
                strAlert3 = Func_Alert(itemD.porc_Days, itemD.porc_EDays, 4)

                'tot_proyecto = If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_Proyecto"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_Proyecto_2"), dtRow("c_Aportes_Proyecto_3")))
                'tot_proyecto_padre = If(Me.hd_tp_activity.Value = "STO", dtRow("c_Aportes_Proyecto_padre"), If(Me.hd_tp_activity.Value = "INK", dtRow("c_Aportes_Proyecto_padre_2"), dtRow("c_Aportes_Proyecto_padre_3")))

                tot_proyecto = If(IsNothing(itemD.OBLIGATED_AMOUNT), 0.0, itemD.OBLIGATED_AMOUNT)
                tot_proyecto_padre = If(IsNothing(itemD.OBLIGATED_AMOUNT_padre), 0.0, itemD.OBLIGATED_AMOUNT_padre)


                strRowsTOT &= String.Format(strRows, itemD.codigo_SAPME, itemD.codigo_RFA, itemD.nombre_proyecto, itemD.fecha_inicio_proyecto, itemD.fecha_fin_proyecto, If(tot_proyecto_padre = 0, 0, (tot_proyecto / tot_proyecto_padre)), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", tot_proyecto, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, itemD.id_activity)
                strRowsTOT &= String.Format(strRows2, strAlert2, vDays, strAlert3, vDiferences_Adj, vDiferences)

                'If dtRow("id_ficha_estado") <> 3 Or dtRow("id_ficha_estado") <> 6 Then
                '    id_ficha_entregable = dtRow("id_ficha_entregable")
                'ElseIf dtRow("id_deliverable") = id_deliverable Then
                '    id_ficha_entregable = dtRow("id_ficha_entregable")
                'End If

            Next

            Dim strTable = "<table class='table no-margin'>
                                  <thead>
                                      <tr>
                                          <th style='width:2%;'></th>                                   
                                          <th style='width20%;'>RMS Code</th>                    
                                          <th style='width20%;'>Technical Code</th>
                                          <th style='width35%;'>" & strType_Docs & "</th>
                                          <th style='width8%;'>Start Date</th>
                                          <th style='width8%;'>End Date</th>
                                          <th style='width3%;'>%</th>
                                          <th style='width8%;'>Amount</th>
                                          <th style = 'width:8%;'>Status</th>
                                       </tr>
                                  </thead>
                                  <tbody>   
                                       {0}
                                 </tbody>
                        </table>"

            'Father_fundingUSD = Math.Round((Father_funding / ExchangeRate), 2, MidpointRounding.AwayFromZero)
            'PendingFundingUSD = Math.Round((PendingFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)
            'PerformedFundingUSD = Math.Round((PerformedFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)


            PorcenPerformed = Math.Round(((PerformedFunding / Father_funding) * 100), 2, MidpointRounding.AwayFromZero)

            PendingFunding = If(Father_funding - PerformedFunding < 0, 0, Father_funding - PerformedFunding)
            PendingFundingUSD = If(Father_fundingUSD - PerformedFundingUSD < 0, 0, Father_fundingUSD - PerformedFundingUSD)


            Get_Ficha_Proyecto_Hijos = String.Format(strTable, strRowsTOT.Trim) & "||" &
                             String.Format("{0:N2} USD", Father_fundingUSD) & "||" &
                             String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", Father_funding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                             String.Format("{0:N2} USD", PendingFundingUSD) & "||" &
                             String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PendingFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                             String.Format("{0:N2} USD", PerformedFundingUSD) & "||" &
                             String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PerformedFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                             String.Format("{0:N2}", PorcenPerformed)

            '& "||" & "none" & "||" &id_ficha_entregable

        End Using

    End Function





    Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


        Dim Dif_Porce As Double = porcDays - porcEDays
        Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

        Const c_label_danger As String = "label-danger"
        Const c_label_warning As String = "label-warning"
        Const c_label_primary As String = "label-primary"
        Const c_label_success As String = "label-success"

        Const c_progress_bar_warning = "progress-bar-warning"
        Const c_progress_bar_primary = "progress-bar-primary"
        Const c_progress_bar_danger = "progress-bar-danger"


        Dim strResult As String = ""
        Dim strStatus As String = ""
        Dim strAlertBar1 As String = ""
        Dim strAlertBar2 As String = ""


        If porc_Progress >= 0 Then

            'Inverter number
            If ((1 - porc_Progress) * 100) >= 90 Then
                strResult = c_label_danger
            ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
                strResult = c_label_warning
            ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
                strResult = c_label_primary
            Else
                strResult = c_label_success
            End If

            strStatus = "Pending"
            strAlertBar2 = c_progress_bar_primary

        Else 'Expired
            strResult = c_label_danger
            strStatus = "Expired"
            strAlertBar2 = c_progress_bar_danger
        End If

        strAlertBar1 = c_progress_bar_warning

        If alertType = 1 Then
            Func_Alert = strResult
        ElseIf alertType = 2 Then
            Func_Alert = strStatus
        ElseIf alertType = 3 Then
            Func_Alert = strAlertBar1
        Else
            Func_Alert = strAlertBar2
        End If


    End Function


    Public Shared Function Func_Unit(ByVal Ndays As String) As String


        Dim vDays As Double
        Dim vWeeks As Double
        Dim vMonths As Double
        Dim vYear As Double

        Dim strUnit As String
        Dim vUnit As Double

        vDays = Ndays
        vWeeks = vDays / 7
        vMonths = vDays / 30
        vYear = vDays / 365


        If vWeeks < 1 Then

            vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

            If vDays > 1 Then
                strUnit = " days"
            Else
                strUnit = " day"
            End If

        ElseIf vMonths < 1 Then

            vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

            If vWeeks > 1 Then
                strUnit = " weeks"
            Else
                strUnit = " week"
            End If

        ElseIf vYear < 1 Then

            vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

            If vMonths > 1 Then
                strUnit = " months"
            Else
                strUnit = " month"
            End If

        Else

            vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

            If vYear > 1 Then
                strUnit = " years"
            Else
                strUnit = " year"
            End If

        End If


        Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

    End Function


End Class