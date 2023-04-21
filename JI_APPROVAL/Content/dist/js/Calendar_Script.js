


var d = new Date(Date.now());
var year = d.getFullYear();
var month = d.getMonth();
var day = d.getDate();
var Date1 = new Date(year, month, day);
var Date2 = new Date(year + 1, month, day);

var DateL1 = new Date(year, month, day);
var DateL2 = new Date(year + 1, month, day);
var DatePiv = new Date(year, month, day);

function set_Calendar_Vars(varCalendar, varHidden, VstartDate = null, VendDate = null) {


    d = new Date(Date.now());
    year = d.getFullYear();
    month = d.getMonth();
    day = d.getDate();
    Date1 = new Date(year, month - 6, day);
    Date2 = new Date(year + 1, month, day);

    var Hvar_Date = $('input[id*=' + varHidden + ']');
      
    DateL1 = null;
    if (VstartDate != null) {
        var Hvar_DateR1 = $('input[id*=' + VstartDate + ']');
        if (Date.parse(Hvar_DateR1.val()) != NaN) {
            //DatePiv = new Date(Date.parse(Hvar_DateR1.val())); 
            //DateL1 = DatePiv.setMonth(DatePiv.getMonth() + 1);
            DateL1 = new Date(Date.parse(Hvar_DateR1.val()));
        }
    } else {
        DateL1 = Date1;
    }

    DateL2 = null;
    if (VendDate != null) {
        var Hvar_DateR2 = $('input[id*=' + VendDate + ']');
        if (Date.parse(Hvar_DateR2.val()) != NaN) {
            //DatePiv = new Date(Date.parse(Hvar_DateR2.val()));
            //DateL2 = DatePiv.setMonth(DatePiv.getMonth() + 1);
            DateL2 = new Date(Date.parse(Hvar_DateR2.val()));
        }
    } else {
        DateL2 = Date2;
    }
    
    //var Date1_ = H_Date.val(); 
    
    console.log('Func Date1: ' + Date1);
    console.log('Func StartDate: ' + DateL1);
    console.log('Func Date2: ' + Date2);
    console.log('Func EndDate: ' + DateL2);
    var n;

    console.log('Func Init Date: ' + Hvar_Date.val());

    if (Date.parse(Hvar_Date.val()) != NaN) {
        Date1 = new Date(Date.parse(Hvar_Date.val()));
        n = Date1;
    } else {
        n = d.getUTCDate();
    }

    console.log('Func Init Date Val: ' + Date1);
    console.log('Func Init n: ' + n);

    $('#' + varCalendar).datetimepicker({
        value: n,
        step: 30,
        startDate: Date1,
        endDate: Date2,
        minDate: DateL1,
        maxDate: DateL2,
        minTime: false,
        maxTime: false,
        minDateTime: DateL1,
        maxDateTime: DateL2,
        setDate: n,
        onSelectDate: function (ct, $i) {
            var d = $('#' + varCalendar).datetimepicker('getValue');
            console.log(d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes());
            // var d_V = new Date(d.getFullYear(),d.getMonth(),d.getDate(), d.getHours(), d.getMinutes())
            var d_V = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes();
            console.log(d_V);
            var H_Date = $('input[id*=' + varHidden + ']');
            H_Date.val(d_V);

        },
        onSelectTime: function (current_time, $input) {
            var d = $('#' + varCalendar).datetimepicker('getValue');
            console.log(d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes());
            // var d_V = new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes())
            var d_V = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes();
            console.log(d_V);
            var H_Date = $('input[id*=' + varHidden + ']');
            H_Date.val(d_V);
        }
    });

           
}

