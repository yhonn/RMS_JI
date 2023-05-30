

var gaugeOptions = {

    chart: {
        type: 'solidgauge'
    },

    title: 'Percentage of performed',

    pane: {
        center: ['50%', '85%'],
        size: '120%',
        startAngle: -90,
        endAngle: 90,
        background: {
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || '#EEE',
            innerRadius: '60%',
            outerRadius: '100%',
            shape: 'arc'
        }
    },

    tooltip: {
        enabled: false
    },

    // the value axis
    yAxis: {
        stops: [
            [0.33,'#55BF3B'], // red
            [0.66,'#DDDF0D'], // yellow
            [1,'#DF5353'] // green
        ],
        lineWidth: 0,
        minorTickInterval: null,
        tickPixelInterval: 400,
        tickWidth: 0,
        title: {
            y: -70
        },
        labels: {
            y: 16
        }
    },

    plotOptions: {
        solidgauge: {
            dataLabels: {
                y: 5,
                borderWidth: 0,
                useHTML: true
            }
        }
    }
};



function set_Chart_IQS(avance,tipo) {


   
    //var avance = $("#valor_avance").val();
    // var avance = 25.75;


    $('#container-money').highcharts(
        //Highcharts.chart('container-money', 
        Highcharts.merge(gaugeOptions, {
            yAxis: {
                min: 0,
                max: 100,
                title: {
                    text: tipo + ' Percentage Obligated'
                }
            },

            credits: {
                enabled: false
            },

            series: [{
                name: '',
                data: [Number(avance)],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:18px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y} %</span><br/>' +
                           //'<span style="font-size:12px;color:silver">km/h</span></div>'
                           '</div>'
                },
                tooltip: {
                    //valueSuffix: ' km/h'
                }
            }]

        })
     );


}



function set_Chart_IQS_time(avance, tipo) {



    //var avance = $("#valor_avance").val();
    // var avance = 25.75;
    $('#container-time').highcharts(
   // Highcharts.chart('container-time',
        Highcharts.merge(gaugeOptions, {
            yAxis: {
                min: 0,
                max: 100,
                title: {
                    text: tipo + ' Timeline'
                }
            },

            credits: {
                enabled: false
            },

            series: [{
                name: '',
                data: [Number(avance)],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:18px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y} %</span><br/>' +
                           //'<span style="font-size:12px;color:silver">km/h</span></div>'
                           '</div>'
                },
                tooltip: {
                    //valueSuffix: ' km/h'
                }
            }]

        })
     );


}
