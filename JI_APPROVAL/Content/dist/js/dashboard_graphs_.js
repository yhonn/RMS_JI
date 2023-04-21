


$(function () {
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });
});


function Load_Leveraged_chart(jsonTYPE, jsonFunding) {

    //alert('Chart Tmp1.4');
    //Highcharts.Tick.prototype.drillable = function () { };

    Highcharts.chart('Leveraged-chart', {

        title: {
            text: 'BHA Leveraged Funds',
            align: 'left',
            x: 50
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            }
        }
       , events: {
           drilldown: function (e) {
               var chart = this,
                   drilldowns = chart.userOptions.drilldown.series,
                   series = [];

               //alert(e.point.name);

               e.preventDefault();
               Highcharts.each(drilldowns, function (p, i) {
                   if (p.id.includes(e.point.name)) {
                       chart.addSingleSeriesAsDrilldown(e.point, p);
                   }
               });
               chart.applyDrilldown();
           }
       }
         , labels: {
             items: [{
                 html: 'By Source of Funding (USD)',
                 style: {
                     left: '50px',
                     top: '0px',
                     color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                 }
             }]
         },
        legend: {
            align: 'right',
            x: -150,
            verticalAlign: 'top',
            layout: 'horizontal',
            y: -5,
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
                    enabled: true,
                    format: '{point.y:,.2f} USD',                    
                    formatter: function () {
                        //console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    }
                }
            }
        },
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //********************************pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            // pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y} USD</b>'
        },
        series: jsonTYPE,
        drilldown: {
            series: jsonFunding
        }

    });

    //Leveraged-chart.Highcharts.setOptions({
    //    lang: {
    //        thousandsSep: ','
    //    }
    //});



}


//**********************************************************************************
//******************************FOR TESTING PURPOSES********************************
//**********************************************************************************
function FY_chart() {

    // alert('Pending Graph');


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
    //    series: [{
    //        "name": "Q4",
    //        "data": [{ "name": "FY1", "y": 3691, "drilldown": "FY1Q4" }, { "name": "FY2", "y": 44365, "drilldown": "FY2Q4" }]
    //    }, {
    //        "name": "Q3",
    //        "data": [{ "name": "FY1", "y": 2036, "drilldown": "FY1Q3" }, { "name": "FY2", "y": 20282, "drilldown": "FY2Q3" }]
    //    }, {
    //        "name": "Q2",
    //        "data": [{ "name": "FY2", "y": 6741, "drilldown": "FY2Q2" }, { "name": "FY3", "y": 12845, "drilldown": "FY3Q2" }]
    //    }, {
    //        "name": "Q1",
    //        "data": [{ "name": "FY2", "y": 638, "drilldown": "FY2Q1" }, { "name": "FY3", "y": 23423, "drilldown": "FY3Q1" }]
    //    }],
    //    drilldown: {
    //        activeDataLabelStyle: {
    //            color: 'white',
    //            textShadow: '0 0 2px black, 0 0 2px black'
    //        },
    //        series: [{
    //            "id": "FY1Q4",
    //            "name": "FY1-Q4",
    //            "data": [["Agasha", 2347], ["Equator Seeds Ltd", 563], ["K Mubende Farm Supplies", 25], ["Mugalex Agro Enterprise Ltd ", 316], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 159], ["YLA Eastern Team", 58], ["YLA Northern Team", 44], ["YLA Central team", 179]]
    //        }, {
    //            "id": "FY2Q4",
    //            "name": "FY2-Q4",
    //            "data": [["TOTCO Uganda Limited ", 42], ["West Nile Holdings Limited", 50], ["Sing With Me Happily", 31], ["SMART MONEY", 628], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 34159], ["Ngetta Tropical Holdings", 1409], ["East African seeds company Limited", 75], ["Kulika Uganda", 1851], ["KadAfrica Ltd", 320], ["African Innovations Institute (AfrII)", 3], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 3], ["Equator Seeds Ltd", 241], ["Faith Agro Inputs Ltd (Faith Agro)", 1719], ["Agromart Agricultural Co. (U) Ltd. (Agromart)", 203], ["Akorion Company Limited ", 913], ["Balton Uganda Ltd", 15], ["Brilliant Youth Organization (BYO)", 885], ["Byeffe Foods  Company Ltd", 1747], ["Caïo Shea Butter", 71]]
    //        }, {
    //            "id": "FY1Q3",
    //            "name": "FY1-Q3",
    //            "data": [["Agasha", 251], ["Equator Seeds Ltd", 1785]]
    //        }, {
    //            "id": "FY2Q3",
    //            "name": "FY2-Q3",
    //            "data": [["Sing With Me Happily", 12], ["SMART MONEY", 2904], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 11764], ["Mugalex Agro Enterprise Ltd ", 554], ["Ngetta Tropical Holdings", 6], ["Kulika Uganda", 1570], ["Equator Seeds Ltd", 3342], ["Agromart Agricultural Co. (U) Ltd. (Agromart)", 18], ["Akorion Company Limited ", 84], ["Balton Uganda Ltd", 28]]
    //        }, {
    //            "id": "FY2Q2",
    //            "name": "FY2-Q2",
    //            "data": [["Sing With Me Happily", 11], ["SMART MONEY", 3666], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 504], ["Mugalex Agro Enterprise Ltd ", 287], ["East African seeds company Limited", 13], ["Equator Seeds Ltd", 926], ["Byeffe Foods  Company Ltd", 1334]]
    //        }, {
    //            "id": "FY3Q2",
    //            "name": "FY3-Q2",
    //            "data": [["Caïo Shea Butter", 477], ["Agrinet Uganda Ltd (AgriNet)", 1247], ["Green Growers Training and Demonstration Farm ", 123], ["Sebei Farmers Savings and Credit Cooperative Organization ", 1245], ["Sunshine Agro Products Limited", 31], ["TOTCO Uganda Limited ", 9722]]
    //        }, {
    //            "id": "FY2Q1",
    //            "name": "FY2-Q1",
    //            "data": [["YLA Central team", 20], ["YLA Northern Team", 10], ["YLA Western Team", 79], ["Simlaw seed company Limited", 113], ["Sing With Me Happily", 37], ["Mugalex Agro Enterprise Ltd ", 364], ["K Mubende Farm Supplies", 15]]
    //        }, {
    //            "id": "FY3Q1",
    //            "name": "FY3-Q1",
    //            "data": [["Caïo Shea Butter", 245], ["Aponye Uganda Limited", 107], ["Agrinet Uganda Ltd (AgriNet)", 37], ["Faith Agro Inputs Ltd (Faith Agro)", 563], ["Green Growers Training and Demonstration Farm ", 44], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 118], ["African Innovations Institute (AfrII)", 725], ["KadAfrica Ltd", 100], ["Kulika Uganda", 465], ["East African seeds company Limited", 171], ["Ngetta Tropical Holdings", 5386], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 8410], ["Responsible Suppliers Ltd", 1331], ["Sebei Farmers Savings and Credit Cooperative Organization ", 20], ["West Nile Holdings Limited", 5701]]
    //        }]
    //    }
    //})



    //Create the chart


    Highcharts.chart('FY-chart', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'FY - Progress Indicator'
        },
        xAxis: {
            type: 'category'
        },

        legend: {
            enabled: true
        },

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        if (this.y > 0)
                            return this.y;
                    },
                    style: {
                        color: 'white',
                        textShadow: '0 0 2px black, 0 0 2px black'
                    }
                },
                stacking: 'normal'
            }
        },



        //series:[{name:'Q4', 
        //        data:[{name:'FY1', y:3691, drilldown:'FY1Q4'},
        //              {name:'FY2', y:44365, drilldown:'FY2Q4'}]
        //        },
        //        {name:'Q3', 
        //        data:[{name:'FY1', y:2036, drilldown:'FY1Q3'},
        //               {name:'FY2', y:20282, drilldown:'FY2Q3'}]
        //        },
        //        {name:'Q2', 
        //         data:[{name:'FY2', y:6741, drilldown:'FY2Q2'},
        //               {name:'FY3', y:12845, drilldown:'FY3Q2'}]
        //        },
        //        {name:'Q1', 
        //        data:[ {name:'FY2', y:638, drilldown:'FY2Q1'},
        //               {name:'FY3', y:23423, drilldown:'FY3Q1'}]
        //        }],

        //*****************************************************************************
        //*****************************************************************************
        //*****************************************************************************

        series: [{
            name: 'Q4',
            data: [{
                name: 'FY1',
                y: 5000,
                drilldown: 'FY1Q4'
            }, {
                name: 'FY2',
                y: 1000,
                drilldown: 'FY2Q4'
            }, {
                name: 'FY3',
                y: 4500,
                drilldown: 'FY3Q4'
            }, {
                name: 'FY4',
                y: 3500,
                drilldown: 'FY4Q4'
            }]
        }, {
            name: 'Q3',
            data: [{
                name: 'FY1',
                y: 5000,
                drilldown: 'FY1Q3'
            }, {
                name: 'FY2',
                y: 1000,
                drilldown: 'FY2Q3'
            }, {
                name: 'FY3',
                y: 4500,
                drilldown: 'FY3Q3'
            }, {
                name: 'FY4',
                y: 3500,
                drilldown: 'FY4Q3'
            }]
        }, {
            name: 'Q2',
            data: [{
                name: 'FY1',
                y: 5000,
                drilldown: 'FY1Q2'
            }, {
                name: 'FY2',
                y: 1000,
                drilldown: 'FY2Q2'
            }, {
                name: 'FY3',
                y: 4500,
                drilldown: 'FY3Q2'
            }, {
                name: 'FY4',
                y: 3500,
                drilldown: 'FY4Q2'
            }]
        }, {
            name: 'Q1',
            data: [{
                name: 'FY1',
                y: 15000,
                drilldown: 'FY1Q1'
            }, {
                name: 'FY2',
                y: 2000,
                drilldown: 'FY2Q1'
            }, {
                name: 'FY3',
                y: 4000,
                drilldown: 'FY3Q1'
            }, {
                name: 'FY4',
                y: 5000,
                drilldown: 'FY4Q1'
            }]
        }],


        drilldown: {
            activeDataLabelStyle: {
                color: 'white',
                textShadow: '0 0 2px black, 0 0 2px black'
            },


            //***********************************************************************************************
            //***********************************************************************************************
            //***********************************************************************************************


            //    series: [{
            //        id: 'FY1Q4',
            //        name: 'FY1-Q4',
            //        data: [
            //            ['Agasha', 2347],
            //            ['Equator Seeds Ltd', 563],
            //            ['K Mubende Farm Supplies', 25],
            //            ['Mugalex Agro Enterprise Ltd ', 316],
            //            ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 159],
            //            ['YLA Eastern Team', 58],
            //            ['YLA Northern Team', 44],
            //            ['YLA Central team', 179]
            //        ]
            //    },
            //        {
            //            id: 'FY2Q4',
            //            name: 'FY2-Q4',
            //            data: [
            //                ['TOTCO Uganda Limited ', 42],
            //                ['West Nile Holdings Limited', 50],
            //                ['Sing With Me Happily', 31],
            //                ['SMART MONEY', 628],
            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 34159],
            //                ['Ngetta Tropical Holdings', 1409],
            //                ['East African seeds company Limited', 75],
            //                ['Kulika Uganda', 1851],
            //                ['KadAfrica Ltd', 320],
            //                ['African Innovations Institute (AfrII)', 3],
            //                ['Consult Agri-Query Solutions Ltd (Agri-Query)', 3],
            //                ['Equator Seeds Ltd', 241],
            //                ['Faith Agro Inputs Ltd (Faith Agro)', 1719],
            //                ['Agromart Agricultural Co. (U) Ltd. (Agromart)', 203],
            //                ['Akorion Company Limited ', 913],
            //                ['Balton Uganda Ltd', 15],
            //                ['Brilliant Youth Organization (BYO)', 885],
            //                ['Byeffe Foods  Company Ltd', 1747],
            //                ['Caïo Shea Butter', 71]
            //            ]
            //        }, {
            //            id: 'FY1Q3',
            //            name: 'FY1-Q3',
            //            data: [
            //                ['Agasha', 251],
            //                ['Equator Seeds Ltd', 1785]
            //            ]
            //        }, {
            //            id: 'FY2Q3',
            //            name: 'FY2-Q3',
            //            data: [
            //                ['Sing With Me Happily', 12],
            //                ['SMART MONEY', 2904],
            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 11764],
            //                ['Mugalex Agro Enterprise Ltd ', 554],
            //                ['Ngetta Tropical Holdings', 6],
            //                ['Kulika Uganda', 1570],
            //                ['Equator Seeds Ltd', 3342],
            //                ['Agromart Agricultural Co. (U) Ltd. (Agromart)', 18],
            //                ['Akorion Company Limited ', 84],
            //                ['Balton Uganda Ltd', 28]
            //            ]
            //        }, {
            //            id: 'FY2Q2',
            //            name: 'FY2-Q2',
            //            data: [
            //                ['Sing With Me Happily', 11],
            //                ['SMART MONEY', 3666],
            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 504],
            //                ['Mugalex Agro Enterprise Ltd ', 287],
            //                ['East African seeds company Limited', 13],
            //                ['Equator Seeds Ltd', 926],
            //                ['Byeffe Foods  Company Ltd', 1334]
            //            ]
            //        }, {
            //            id: 'FY3Q2',
            //            name: 'FY3-Q2',
            //            data: [
            //                ['Caïo Shea Butter', 477],
            //                ['Agrinet Uganda Ltd (AgriNet)', 1247],
            //                ['Green Growers Training and Demonstration Farm ', 123],
            //                ['Sebei Farmers Savings and Credit Cooperative Organization ', 1245],
            //                ['Sunshine Agro Products Limited', 31],
            //                ['TOTCO Uganda Limited ', 9722]
            //            ]
            //        }, {
            //            id: 'FY2Q1',
            //            name: 'FY2-Q1',
            //            data: [
            //                ['YLA Central team', 20],
            //                ['YLA Northern Team', 10],
            //                ['YLA Western Team', 79],
            //                ['Simlaw seed company Limited', 113],
            //                ['Sing With Me Happily', 37],
            //                ['Mugalex Agro Enterprise Ltd ', 364],
            //                ['K Mubende Farm Supplies', 15]
            //            ]
            //        }, {
            //            id: 'FY3Q1',
            //            name: 'FY3-Q1',
            //            data: [
            //                ['Caïo Shea Butter', 245],
            //                ['Aponye Uganda Limited', 107],
            //                ['Agrinet Uganda Ltd (AgriNet)', 37],
            //                ['Faith Agro Inputs Ltd (Faith Agro)', 563],
            //                ['Green Growers Training and Demonstration Farm ', 44],
            //                ['Consult Agri-Query Solutions Ltd (Agri-Query)', 118],
            //                ['African Innovations Institute (AfrII)', 725],
            //                ['KadAfrica Ltd', 100],
            //                ['Kulika Uganda', 465],
            //                ['East African seeds company Limited', 171],
            //                ['Ngetta Tropical Holdings', 5386],
            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 8410],
            //                ['Responsible Suppliers Ltd', 1331],
            //                ['Sebei Farmers Savings and Credit Cooperative Organization ', 20],
            //                ['West Nile Holdings Limited', 5701]
            //            ]
            //        }],


            //***********************************************************************************************
            //***********************************************************************************************
            //***********************************************************************************************

            series: [{
                id: 'FY1Q1',
                name: 'FY1-Q1',
                data: [
                    ['Equator Seeds Ltd', 5000],
                    ['Byeffe Foods  Company Ltd', 2000],
                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
                    ['Akorion Company Limited ', 2000],
                    ['Faith Agro Inputs Ltd (Faith Agro)', 5000]
                ]
            }, {
                id: 'FY1Q2',
                name: 'FY1-Q2',
                data: [
                        ['Equator Seeds Ltd', 500],
                        ['Byeffe Foods  Company Ltd', 100],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
                        ['Akorion Company Limited ', 200],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 100],
                        ['West Nile Holdings Limited', 100]
                ]
            }, {
                id: 'FY1Q3',
                name: 'FY1-Q3',
                data: [
                        ['Equator Seeds Ltd', 800],
                        ['Byeffe Foods  Company Ltd', 800],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
                        ['Akorion Company Limited ', 400],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 500],
                        ['West Nile Holdings Limited', 100],
                        ['KadAfrica Ltd', 400]
                ]
            }, {
                id: 'FY1Q4',
                name: 'FY1-Q4',
                data: [
                        ['Equator Seeds Ltd', 500],
                        ['Byeffe Foods  Company Ltd', 700],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 800],
                        ['Akorion Company Limited ', 2000],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 100],
                        ['West Nile Holdings Limited', 500],
                        ['Brilliant Youth Organization (BYO)', 400]
                ]
            }, {
                id: 'FY2Q1',
                name: 'FY2-Q1',
                data: [
                       ['Equator Seeds Ltd', 500],
                        ['Byeffe Foods  Company Ltd', 600],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 400],
                        ['Akorion Company Limited ', 200],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 300],
                        ['West Nile Holdings Limited', 1000],
                        ['Brilliant Youth Organization (BYO)', 500],
                        ['MobiPay Agrosys Limited', 1500]
                ]
            }, {
                id: 'FY2Q2',
                name: 'FY2-Q2',
                data: [
                        ['Equator Seeds Ltd', 100],
                        ['Byeffe Foods  Company Ltd', 200],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 100],
                        ['Akorion Company Limited ', 2],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 100],
                        ['West Nile Holdings Limited', 1],
                        ['Brilliant Youth Organization (BYO)', 50],
                        ['MobiPay Agrosys Limited', 50],
                        ['Aponye Uganda Limited', 400]

                ]
            }, {
                id: 'FY2Q3',
                name: 'FY2-Q3',
                data: [
                        ['Equator Seeds Ltd', 500],
                        ['Byeffe Foods  Company Ltd', 300],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 300],
                        ['Akorion Company Limited ', 200],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 500],
                        ['West Nile Holdings Limited', 200],
                        ['Brilliant Youth Organization (BYO)', 500],
                        ['MobiPay Agrosys Limited', 1000],
                        ['Aponye Uganda Limited', 1000]
                ]
            }, {
                id: 'FY2Q4',
                name: 'FY2-Q4',
                data: [
                        ['Equator Seeds Ltd', 100],
                        ['Byeffe Foods  Company Ltd', 200],
                        ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 500],
                        ['Akorion Company Limited ', 1000],
                        ['Faith Agro Inputs Ltd (Faith Agro)', 500],
                        ['West Nile Holdings Limited', 100],
                        ['Brilliant Youth Organization (BYO)', 200],
                        ['MobiPay Agrosys Limited', 700],
                        ['Aponye Uganda Limited', 500]

                ]
            }]


        }

    })


}

