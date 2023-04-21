Function.prototype.ExtraParams = function () {
    var method = this, args = Array.prototype.slice.call(arguments);
    return function () {
        return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
    };
};

/*********Módulo de particiapantes calcula la edad ***********/


function setMaxDate() {
    var now = new Date();
    var datePicker = controls.dt_date_birth.control;
    datePicker.set_maxDate(new Date(now.getFullYear(), now.getMonth(), now.getDate()));
}


function calcularEdadParticipanteDP(sender, e) {
    var yearBirth = controls.dt_date_birth.control.get_selectedDate().getFullYear();
    var today = new Date();
    var year = today.getFullYear();

    var ageYears = year - yearBirth;
    if (ageYears != "" && ageYears > 0 && ageYears != null && ageYears != undefined) {
        var ageYearsTxt = $("#" + controls.txt_age_years.ClientID);
        ageYearsTxt.val(ageYears);
        if (ageYears < 13 || ageYears > 70) {
            $("#age_warning").css("display", "block");
            if (ageYears < 13)
                $("#" + controls.lbl_warning.ClientID).html("Age is under 13")
            else
                $("#" + controls.lbl_warning.ClientID).html("Age is above 70")
        }
        else
            $("#age_warning").css("display", "none");
        if (ageYears >= 18) {
            controls.txt_nid_number.control.enable()
            //$("#" + controls.txt_nid_number.ClientID).removeAttr("disabled")
        }
        else {
            controls.txt_nid_number.control.disable()
        }
    }
    /*if (e.get_newDate() != null) {
        logEvent("OnDateSelected: " + e.get_newDate().toDateString() + " selected in " + sender.get_id() + "<br />");
    }
    else {
        logEvent("OnDateSelected: Date cleared in " + sender.get_id() + "<br />");
    }*/
}
function calcularEdadParticipante(sender, eventArgs) {
    var today = new Date();
    var year = today.getFullYear();
    if (controls.txt_year.control.get_value() != null) {
        var ageYears = year - controls.txt_year.control.get_value();
        if (ageYears != "" && ageYears > 0 && ageYears != null && ageYears != undefined) {
            var ageYearsTxt = $("#" + controls.txt_age_years.ClientID);
            ageYearsTxt.val(ageYears);
            if (ageYears < 13 || ageYears > 70) {
                $("#age_warning").css("display", "block");
                if (ageYears < 13)
                    $("#" + controls.lbl_warning.ClientID).html("Age is under 13")
                else
                    $("#" + controls.lbl_warning.ClientID).html("Age is above 70")
            }
            else
                $("#age_warning").css("display", "none");
            if (ageYears >= 18) {
                controls.txt_nid_number.control.enable()
                //$("#" + controls.txt_nid_number.ClientID).removeAttr("disabled")
            }
            else {
                controls.txt_nid_number.control.disable()
            }
        }
    }
    
}


function scrollToAnchorForms(sender, eventArgs, Params) {
    var aid = Params[0];
    //console.log('hi folks ' + vCounter + ' ' + aid);
    var aTag = $("a[name='" + aid + "']");
    $('html,body').animate({ scrollTop: aTag.offset().top }, 'fast');
    //vCounter++;

}

function validarScrollForms(id_tag) {
    $('html,body').animate({ scrollTop: $("a[name='" + id_tag + "']").offset().top }, 'fast');
}



function validarScrollCtrl() {
    var ctrlScrollValue = $("#" + controls.controlScrollGIS.ClientID).val();
    if (ctrlScrollValue == 1) {
        $('html,body').animate({ scrollTop: $("a[name='id_GEOGRAPHIC']").offset().top }, 'fast');
    }
}

/****************Group participant*********************/
function checkGroupParticipant(sender, eventArgs) {
    var idGroup = controls.cmb_group_participant.control.get_value();
    if (idGroup == 1 || idGroup == 2) {

        $(".info_partner_organization").css("display", "block");
        $("#name_producer").css("display", "none");
        ValidatorEnable(controls.rv_cmb_type_organization_partner.control, true);
        ValidatorEnable(controls.rv_cmb_organization_partner.control, true);
        ValidatorEnable(controls.rv_txt_name_producer.control, false);
    }
    else if (idGroup == 3){
        $(".info_partner_organization").css("display", "none");
        $("#name_producer").css("display", "block");
        ValidatorEnable(controls.rv_cmb_type_organization_partner.control, false);
        ValidatorEnable(controls.rv_cmb_organization_partner.control, false);
        ValidatorEnable(controls.rv_txt_name_producer.control, true);
    }
    $(".tab-content").css("height", "auto");
}


function checkGovernment_Group(sender, eventArgs) {
    
    var idGroup = controls.cmb_government_group.control.get_value();

    console.log(' checkGovernment idGroup =' + idGroup);

    if (idGroup == 1 || idGroup == 3) {

        $(".info_government_group").css("display", "block");
        ValidatorEnable(controls.rv_cmb_type_government_group.control, true);
        ValidatorEnable(controls.rv_cmb_organization_government_group.control, true);

        $(".info_government_group_2").css("display", "none");
        ValidatorEnable(controls.rv_txt_organization_govt_name.control, false);

    } else {

        //if (idGroup == 2) {

            $(".info_government_group").css("display", "none");
            ValidatorEnable(controls.rv_cmb_type_government_group.control, false);
            ValidatorEnable(controls.rv_cmb_organization_government_group.control, false);

            $(".info_government_group_2").css("display", "block");
            ValidatorEnable(controls.rv_txt_organization_govt_name.control, true);

        //}
                 
    }


}



function checkCivil_Group(sender, eventArgs) {

    var idGroup = controls.cmb_civil_society_group.control.get_value();

    if (idGroup == 1 || idGroup == 3) {

        $(".info_civil_society_g").css("display", "block");
        ValidatorEnable(controls.rv_cmb_type_civil_society_group.control, true);
        ValidatorEnable(controls.rv_cmb_organization_civil_society_group.control, true);
             
        $(".info_civil_society_2").css("display", "none");
        ValidatorEnable(controls.rv_txt_organization_civil_name.control, false);

        //console.log('  ValidatorEnable(controls.rv_txt_organization_civil_name.control, false);  idGroup = ' + idGroup);               

    } else {
              
        //if (idGroup == 2) {

            $(".info_civil_society_g").css("display", "none");
            ValidatorEnable(controls.rv_cmb_type_civil_society_group.control, false);
            ValidatorEnable(controls.rv_cmb_organization_civil_society_group.control, false);

            $(".info_civil_society_2").css("display", "block");
            ValidatorEnable(controls.rv_txt_organization_civil_name.control, true);

            //console.log('  ValidatorEnable(controls.rv_txt_organization_civil_name.control, false);  idGroup = ' + idGroup);
        
        //}
        
    }
    
}


/******Change Marital Status Módulo de participantes********/

function checkMaritalStatus(sender, eventArgs) {
  
    var idMaritalStatus = controls.cmb_marital_status.control.get_value();
    if (idMaritalStatus == 1 || idMaritalStatus == "Married") {
        $("#" + controls.spouse_status.ClientID).val(1);
        $("#" + controls.lblt_spouse_first_name.ClientID).html("Spouse's first name ")
        $("#" + controls.lblt_spouse_last_name.ClientID).html("Spouse's surname ")
    }
    else {
        $("#" + controls.spouse_status.ClientID).val(0);
        $("#" + controls.lblt_spouse_first_name.ClientID).html("Spouse's first name")
        $("#" + controls.lblt_spouse_last_name.ClientID).html("Spouse's surname")
    }
    var maritalStatus = $("#" + controls.spouse_status.ClientID).val();
    if (maritalStatus == 1) {
        $("#spouse_info").css("display", "block")
    }
    else {
        $("#spouse_info").css("display", "none")
    }
    $(".tab-content").css("height", "auto");
}


/*********Pone los campos de ubicación geografica como obligatorios***************/
function ubiGEORequired() {
    ValidatorEnable(controls.rv_cmb_division.control, true);
    ValidatorEnable(controls.rv_cmb_district.control, true);
    ValidatorEnable(controls.rv_cmb_upazila.control, true);
    ValidatorEnable(controls.rv_cmb_union.control, true);
    ValidatorEnable(controls.rv_cmb_village.control, true);
    $(".tab-content").css("height", "auto");
}

function actualizarHeightStepper() {
    setTimeout(function () {
        $(".tab-content").css("height", "auto");
    }, 1000)
}

/*************Validate contact number**************** */
function ClientValidateContactNumber(source, arguments) {
    if ($("#" + controls.txt_contac_number_text.ClientID).val().length > 11 || $("#" + controls.txt_contac_number_text.ClientID).val().length < 11) {
        $("#contacValidation").css("display", "block")
    }
    else {
        $("#contacValidation").css("display", "none")
    }
}



/*************Check participants status****** */
function participantStatus() {


    $("#info_laborer").css("display", "none");
    $("#organization_name").css("display", "none");
    $("#propriertor_enterprise").css("display", "none");
    $("#info_proprietor").css("display", "none");
    $("#name_enterprise").css("display", "none");
    $("#number_employees").css("display", "none");        
    $("#info_government").css("display", "none");

    var is_producer = $("input[name='" + controls.rbn_producer.UniqueID + "']:checked").val();
    var is_laborer = $("input[name='" + controls.rbn_laborer.UniqueID + "']:checked").val();
    var is_propiertor = $("input[name='" + controls.rbn_proprietor.UniqueID + "']:checked").val();
    var is_grantee = $("input[name='" + controls.rbn_partner_grantee.UniqueID + "']:checked").val();
    var is_govt = $("input[name='" + controls.rbn_govt_employee.UniqueID + "']:checked").val();
    var is_civil = $("input[name='" + controls.rbn_civil_society.UniqueID + "']:checked").val();
    
    if (is_producer == 1) {
        $("#info_producer").css("display", "block");
        $("#info_laborer").css("display", "none");
        $("#organization_name").css("display", "none");
        $("#propriertor_enterprise").css("display", "none");
        $("#info_proprietor").css("display", "none");
        $("#name_enterprise").css("display", "none");
        $("#" + controls.is_producer_value.ClientID).val(1);      
        ValidatorEnable(controls.rv_farmland.control, true);
        ValidatorEnable(controls.rv_number_employees.control, true);
    }
    else {
        $("#info_producer").css("display", "none");
        $("#info_laborer").css("display", "block");
        $("#" + controls.is_producer_value.ClientID).val(0);
        ValidatorEnable(controls.rv_farmland.control, false);
        ValidatorEnable(controls.rv_number_employees.control, false);
        if (is_laborer == 1) {
            //$("#organization_name").css("display", "block");
            $("#propriertor_enterprise").css("display", "none");
            $("#" + controls.is_laborer.ClientID).val(1);          
        }
        else {
            $("#" + controls.is_laborer.ClientID).val(0);
            $("#organization_name").css("display", "none");
            $("#propriertor_enterprise").css("display", "block");

           //console.log('This is the value of is_propiertor ' + is_propiertor);

            if (is_propiertor == 1) {
                $("#" + controls.is_propiertor.ClientID).val(1);
                $("#info_proprietor").css("display", "block");
                if (is_grantee == 1) {
                    $("#name_enterprise").css("display", "block");
                    $("#number_employees").css("display", "none");
                }
                else {
                    $("#name_enterprise").css("display", "none");
                    $("#number_employees").css("display", "block");
                }
            } else {

                $("#" + controls.is_propiertor.ClientID).val(0);
                $("#info_proprietor").css("display", "none");
                //console.log('This is the value of govt ' + is_govt);

                if (is_govt == 1) {

                    $("#" + controls.is_govt_emp.ClientID).val(1);
                    $("#info_government").css("display", "block");

                    var idGroup = controls.cmb_government_group.control.get_value();

                    console.log('(ParticipantStatus) checkGovernment idGroup =' + idGroup);

                            if (idGroup == 1 || idGroup == 3) {

                                $(".info_government_group").css("display", "block");
                                ValidatorEnable(controls.rv_cmb_type_government_group.control, true);
                                ValidatorEnable(controls.rv_cmb_organization_government_group.control, true);

                                $(".info_government_group_2").css("display", "none");
                                ValidatorEnable(controls.rv_txt_organization_govt_name.control, false);

                            } else {

                                //if (idGroup == 2) {

                                    $(".info_government_group").css("display", "none");
                                    ValidatorEnable(controls.rv_cmb_type_government_group.control, false);
                                    ValidatorEnable(controls.rv_cmb_organization_government_group.control, false);

                                    $(".info_government_group_2").css("display", "block");
                                    ValidatorEnable(controls.rv_txt_organization_govt_name.control, true);

                                //}
                                
                            }
                                       
                    //$(".info_government_group").css("display", "none");
                    //$(".info_government_group_2").css("display", "none");
                                       
                } else {

                    $("#" + controls.is_govt_emp.ClientID).val(0);
                    $("#info_government").css("display", "none");
                    $(".info_government_group").css("display", "none");
                    $(".info_government_group_2").css("display", "none");

                    if (is_civil == 1) {

                        $("#" + controls.is_civil_society.ClientID).val(1);
                        $("#info_civil_society").css("display", "block");

                        var idGroup = controls.cmb_civil_society_group.control.get_value();

                        if (idGroup == 1 || idGroup == 3) {

                            $(".info_civil_society_g").css("display", "block");
                            ValidatorEnable(controls.rv_cmb_type_civil_society_group.control, true);
                            ValidatorEnable(controls.rv_cmb_organization_civil_society_group.control, true);

                            $(".info_civil_society_2").css("display", "none");
                            ValidatorEnable(controls.rv_txt_organization_civil_name.control, false);

                        } else {

                            if (idGroup == 2) {

                                $(".info_civil_society_g").css("display", "none");
                                ValidatorEnable(controls.rv_cmb_type_civil_society_group.control, false);
                                ValidatorEnable(controls.rv_cmb_organization_civil_society_group.control, false);

                                $(".info_civil_society_2").css("display", "block");
                                ValidatorEnable(controls.rv_txt_organization_civil_name.control, true);

                            }

                        }



                    } else {

                        $("#" + controls.is_civil_society.ClientID).val(0);
                        $("#info_civil_society").css("display", "none");
                        $(".info_civil_society_g").css("display", "none");
                        $(".info_civil_society_2").css("display", "none");

                    }

                }

            }
        }
    }
    $(".tab-content").css("height", "auto");
}

/*************Check participants status****** */
function checkDishabilities() {
    var has_disabilities = $("input[name='" + controls.rbn_disabilities.UniqueID + "']:checked").val();
    if (has_disabilities == 1) {
        $("#info_desability").css("display", "block");
        $("#" + controls.has_disabilities.ClientID).val(1);
    }
    else {
        $("#info_desability").css("display", "none");
        $("#" + controls.has_disabilities.ClientID).val(0);
    }
    $(".tab-content").css("height", "auto");
}

/************Check Small holder********* */
function calcularSamallholder(source, arguments) {
    var htaFarmland = controls.txt_farmland.control.get_value();
    if (htaFarmland == "") {
        $("#lbl_smallholder").html("")
    }
    else if (htaFarmland < 5) {
        $("#lbl_smallholder").html("<strong>Type: smallholder</strong>")
    }
    else {
        $("#lbl_smallholder").html("<strong>Type: non-smallholder</strong>")
    }
    $("#lbl_smallholder").css("display", "block")
}


/************Check type of  enterprise********* */
function typeEnterprise(source, arguments) {
    var numberEmployees = controls.txt_number_employees.control.get_value();
    if (numberEmployees == "") {
        $("#lbl_type_sector").html("")
    }
    else if (numberEmployees < 10) {
        $("#lbl_type_sector").html("<strong>Type sector: Microenterprise</strong>")
    }
    else if (numberEmployees >= 10 && numberEmployees <= 249) {
        $("#lbl_type_sector").html("<strong>Type sector: SME</strong>")
    }
    else if (numberEmployees > 249) {
        $("#lbl_type_sector").html("<strong>Type sector: Large enterprise </strong>")
    }
    $("#lbl_type_sector").css("display", "block")
}

/**************Módulo training*********************/
/**************Validate end date******************/
function checkEndDate() {
    controls.dt_end_date.control.set_minDate(controls.dt_start_date.control.get_selectedDate());
   // rvEndDate()
}

//function rvEndDate() {
//    controls.rv_end_date.control.se(controls.dt_start_date.control.get_selectedDate())
//}

function setMaxDateTraining() {
    var now = new Date();
    var datePicker = controls.dt_start_date.control;
    datePicker.set_maxDate(new Date(now.getFullYear(), now.getMonth(), now.getDate()));

    var datePicker2 = controls.dt_end_date.control;
    datePicker2.set_maxDate(new Date(now.getFullYear(), now.getMonth(), now.getDate()));
}



/*************Módulo application tecnology************/
/*************Validate other tecnologie***************/

function checkStatus(cntl1, cntl2) {
    var ctrlWhichone = $find(cntl2);
    if ($(cntl1).is(":checked")) {
        ctrlWhichone.enable();
    }
    else {
        ctrlWhichone.disable();
    }
    /*if (ctrlOther.get_items().getItem(0).get_checked()) {
        combo.get_items().getItem(0).set_checked(false);
    }


    if (text2.get_value() > 0) {
        text3.set_value(text1.get_value() / text2.get_value());
    }
    else {
        text3.set_value(0);
    }*/
}


/**********Módulo de sales***************************/
/**********Validar cantidad maxima vendida***********/
function show(cntl1, cntl2) {
    var text1 = $find(cntl1);
    var text2 = $find(cntl2);
    text2.set_maxValue(text1.get_value())
}

/**********Validar precio unitario de las ventas*****/
function checkUnitPrice(cntl1, cntl2, cntl3) {
    var text1 = $find(cntl1);
    var text2 = $find(cntl2);
    var text3 = $find(cntl3);
    if (text2.get_value() > 0) {
        text3.set_value(text1.get_value() / text2.get_value());
    }
    else {
        text3.set_value(0);
    }
    //text2.set_maxValue(text1.get_value())
}





/**********Validar rendimiento de las ventas*****/
function checkYield(cntl1, cntl2, cntl3) {
    var produced = $find(cntl1);
    var arealand = $find(cntl2);
    var yield = $find(cntl3);
    if (arealand.get_value() > 0) {
        yield.set_value((produced.get_value() / 1000) / arealand.get_value());
    }
    else {
        yield.set_value(0);
    }
    //text2.set_maxValue(text1.get_value())
}



/*********Módulo Jobs Created************/
/*********Validar fecha maxima***********/
function checkStartDate(cntl1, cntl2) {
    var dt_star_Date = $find(cntl1);
    var dt_end_date = $find(cntl2);
    dt_end_date.set_minDate(dt_star_Date.get_selectedDate());
   
}
