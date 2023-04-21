




$(function () {
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });
});



function loadchartPeriodAnual(jsonFY, jsonQ, IndName) {

    

    Highcharts.chart('FY-chart', {
        chart: {
            type: 'column'
        },
        title: {
            text: IndName,           
        },
        subtitle: {
            text: 'Click the columns to view period progress',
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Reported'
            }

        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:2f}'
                }
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b> reported<br/>'
        },

        series: [{
            name: 'Fiscal Year',
            colorByPoint: true,
            data: jsonFY 

        }],
        drilldown: {
            series: jsonQ
        }
    });




}



function Load_FY_chart(jsonIMP, jsonFY, IndName) {

   // alert('Chart Tmp1.3');
    
    //Highcharts.Tick.prototype.drillable = function () { };

    Highcharts.chart('FY-chart', {
        chart: {
            type: 'column'
        },
        title: {
            text: IndName,
        },
        subtitle: {
            text: 'Click the columns to view Implementer progress',
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Reported'
            },
            stackLabels: {
                    enabled: true,
                    style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        legend: {
            align: 'right',
            x: -50,
            verticalAlign: 'top',
            y: 15,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: false,
                    format: '{point.y:,.2f}',
                    formatter: function () {
                        //console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    }
                    //,style: {
                    //    color: 'white',
                    //    textShadow: '0 0 2px black, 0 0 2px black'
                    //}
                },
                stacking: 'normal'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            //pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:,.2f}</b> reported <b>({point.percentage:.2f}%)</b><br/>Total: <b>{point.stackTotal:,.2f}</b>'
        },       
        series: jsonFY,
        drilldown: {
            activeDataLabelStyle: {
                color: 'white',
                textShadow: '0 0 2px black, 0 0 2px black'
            },
            series: jsonIMP
        }
    });

    //Highcharts.chart('FY-chart', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: 'FY - Progress Indicator'
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    legend: {
    //        enabled: true
    //    },
    //    plotOptions: {
    //        series: {
    //            borderWidth: 0,
    //            dataLabels: {
    //                enabled: true,
    //                style: {
    //                    color: 'white',
    //                    textShadow: '0 0 2px black, 0 0 2px black'
    //                }
    //            },
    //            stacking: 'normal'
    //        }
    //    },
    //    series:jsonFY,        
    //    drilldown: {
    //        activeDataLabelStyle: {
    //            color: 'white',
    //            textShadow: '0 0 2px black, 0 0 2px black'
    //        },
    //        series:jsonIMP
    //    }
    //})


}

