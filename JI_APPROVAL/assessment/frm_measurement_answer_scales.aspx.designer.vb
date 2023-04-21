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


Partial Public Class frm_measurement_answer_scales
    
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
    '''Control identity.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents identity As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control Hiddenindi.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Hiddenindi As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control alink_themes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_themes As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_skills.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_skills As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_title.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_title As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_answer_scale.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_answer_scale As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_answer_options.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_answer_options As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_questions.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_questions As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_survey.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_survey As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control alink_survey_questions.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents alink_survey_questions As Global.System.Web.UI.HtmlControls.HtmlAnchor
    
    '''<summary>
    '''Control lbl_id_ficha.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_id_ficha As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_answer_scale.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_answer_scale As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_answer_scale.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_answer_scale As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control btn_add.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_add As Global.Telerik.Web.UI.RadButton
    
    '''<summary>
    '''Control lblt_answer_type.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_answer_type As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_answer_type.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_answer_type As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control grd_answer_scales.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grd_answer_scales As Global.Telerik.Web.UI.RadGrid
    
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
    '''Control btn_deleteRegister.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_deleteRegister As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control esp_ctrl_btnh_CANCELAR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents esp_ctrl_btnh_CANCELAR As Global.System.Web.UI.HtmlControls.HtmlButton
End Class
