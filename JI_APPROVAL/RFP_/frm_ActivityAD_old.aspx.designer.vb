'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class frm_ActivityAD_old
    
    '''<summary>
    '''Control MsgGuardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents MsgGuardar As Global.RMS_APPROVAL.ModalConfirm
    
    '''<summary>
    '''Control MsgReturn.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents MsgReturn As Global.RMS_APPROVAL.DeleteConfirm
    
    '''<summary>
    '''Control lblt_titulo_pantalla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_titulo_pantalla As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_subtitulo_pantalla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_subtitulo_pantalla As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btn_approbals.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_approbals As Global.Telerik.Web.UI.RadButton
    
    '''<summary>
    '''Control identity.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents identity As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control alink_informacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_informacion As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control lbl_id_sesion_temp.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_id_sesion_temp As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfAccordionIndex.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfAccordionIndex As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigoproyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigoproyecto As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control RequiredFieldValidator5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator5 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lnk_sugerir_codigo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnk_sugerir_codigo As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control id_documento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents id_documento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblt_nombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_nombre As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_nombreproyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_nombreproyecto As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control RequiredFieldValidator1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_descripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_descripcion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_descripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_descripcion As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control RequiredFieldValidator7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator7 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_persona_encargada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_persona_encargada As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_persona_encargada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_persona_encargada As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_mecanismo_contratacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_mecanismo_contratacion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_mecanismo_contratacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_mecanismo_contratacion As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control RequiredFieldValidator3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator3 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_sub_mecanismo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_sub_mecanismo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_sub_mecanismo_contratacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_sub_mecanismo_contratacion As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control RequiredFieldValidator4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator4 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control ly_activity.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ly_activity As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lblt_actividad_padre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_actividad_padre As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_activity_father.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_activity_father As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lbl_Activity_error.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Activity_error As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_ejecutor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_ejecutor As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_ejecutor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_ejecutor As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_programa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_programa As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_programa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_programa As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_region.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_region As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_region.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_region As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control chk_todos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chk_todos As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Control lblt_region_message.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_region_message As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_subregion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_subregion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_subregion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_subregion As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control grd_subregion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grd_subregion As Global.Telerik.Web.UI.RadGrid
    
    '''<summary>
    '''Control lblt_districts.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_districts As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control grd_district.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grd_district As Global.Telerik.Web.UI.RadGrid
    
    '''<summary>
    '''Control div_mensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents div_mensaje As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lblt_errorLOECero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_errorLOECero As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_errorLOEHundred.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_errorLOEHundred As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_private_public.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_private_public As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control rbn_private_public.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rbn_private_public As Global.System.Web.UI.WebControls.RadioButtonList
    
    '''<summary>
    '''Control grd_partners.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grd_partners As Global.Telerik.Web.UI.RadGrid
    
    '''<summary>
    '''Control lblt_componentes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_componentes As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_componente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_componente As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_fecha_inicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_fecha_inicio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control dt_fecha_inicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dt_fecha_inicio As Global.Telerik.Web.UI.RadDatePicker
    
    '''<summary>
    '''Control RequiredFieldValidator8.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator8 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_fecha_final.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_fecha_final As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control dt_fecha_fin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dt_fecha_fin As Global.Telerik.Web.UI.RadDatePicker
    
    '''<summary>
    '''Control RequiredFieldValidator2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator2 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_exchange_rate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_exchange_rate As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_exchange_rate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_exchange_rate As Global.Telerik.Web.UI.RadNumericTextBox
    
    '''<summary>
    '''Control lblt_total_Amount.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_total_Amount As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_tot_amount.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_tot_amount As Global.Telerik.Web.UI.RadNumericTextBox
    
    '''<summary>
    '''Control lblt_total_Amount_local.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_total_Amount_local As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_tot_amount_Local.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_tot_amount_Local As Global.Telerik.Web.UI.RadNumericTextBox
    
    '''<summary>
    '''Control lblt_trimestre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_trimestre As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_periodo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_periodo As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_codigoME.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigoME As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigo_SAPME.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigo_SAPME As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_estado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_estado As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_estado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_estado As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_codigo_AID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigo_AID As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigoAID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigoAID As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_codigoRFA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigoRFA As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigoRFA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigoRFA As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_codigo_ficha.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigo_ficha As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control divCodigo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divCodigo As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lbl_mensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_mensaje As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_codigo_monitor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigo_monitor As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigoMonitor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigoMonitor As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_acta_aprobacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_acta_aprobacion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_acta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_acta As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_codigo_convenio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_codigo_convenio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_codigo_convenio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_codigo_convenio As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_imagen_proyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_imagen_proyecto As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btn_salir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_salir As Global.Telerik.Web.UI.RadButton
    
    '''<summary>
    '''Control btn_guardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_guardar As Global.Telerik.Web.UI.RadButton
    
    '''<summary>
    '''Control esp_ctrl_h4_eliminar_titulo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents esp_ctrl_h4_eliminar_titulo As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control esp_ctrl_lbl_eliminar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents esp_ctrl_lbl_eliminar As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control esp_ctrl_btn_eliminar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents esp_ctrl_btn_eliminar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control esp_ctrl_btnh_CANCELAR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents esp_ctrl_btnh_CANCELAR As Global.System.Web.UI.HtmlControls.HtmlButton
    
    '''<summary>
    '''Control H1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents H1 As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lblt_msn_tasa_cambio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_msn_tasa_cambio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btn_registrar_tc.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_registrar_tc As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control Button2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Button2 As Global.System.Web.UI.HtmlControls.HtmlButton
End Class
