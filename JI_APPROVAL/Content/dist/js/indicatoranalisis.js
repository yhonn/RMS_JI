Highcharts.setOptions({
    lang: {
        thousandsSep: ','
    }
});

function loadchartIndividualsSex(participants_sex_category, participants_by_sex, participants_by_group_age, participants_group_age_category,
    participants_by_type_training, participants_type_training_category, participants_by_value_chain, participants_value_chain_category, participants_by_marital_status,
    participants_value_marital_status_category, participants_district_series, participants_county_series, training_organization_category, training_organization_data,
    training_type_training_category, training_type_training_data, training_agricultural_commodity_category, training_agricultural_commodity_data, training_stage_vc_category, training_stage_vc_data) {
    Highcharts.chart('participantsBySexType', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_sex_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_sex
    });

    Highcharts.chart('participantsByGroupAge', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_group_age_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_group_age
    });

    Highcharts.chart('participantsByTypeTraining', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_type_training_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_type_training
    });

    Highcharts.chart('participantsByValueChain', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_chain_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_value_chain
    });

    Highcharts.chart('participantsByMaritalStatus', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_marital_status_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_marital_status
    });

    $('#participantsByGeo').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: participants_district_series,
            name: "Districts"
        }],
        drilldown: {
            series: participants_county_series
        }
    });

    Highcharts.chart('trainingsByOrganizer', {
        chart: {
            zoomType: 'xy'
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: [{
            categories: training_organization_category,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Participants',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Events training',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value} ',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || // theme
                'rgba(255,255,255,0.25)'
        },
        series: training_organization_data
    });

    Highcharts.chart('trainingByTypeTraining', {
        chart: {
            zoomType: 'xy'
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: [{
            categories: training_type_training_category,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Participants',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Events training',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value} ',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || // theme
                'rgba(255,255,255,0.25)'
        },
        series: training_type_training_data
    });

    Highcharts.chart('triningByVC', {
        chart: {
            zoomType: 'xy'
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: [{
            categories: training_agricultural_commodity_category,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Participants',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Events training',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value} ',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || // theme
                'rgba(255,255,255,0.25)'
        },
        series: training_agricultural_commodity_data
    });

    Highcharts.chart('trainingBySVC', {
        chart: {
            zoomType: 'xy'
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: [{
            categories: training_stage_vc_category,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Participants',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Events training',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value} ',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || // theme
                'rgba(255,255,255,0.25)'
        },
        series: training_stage_vc_data
    });
}


function loadchartIndividualsTech(participants_sex_category, participants_by_sex, participants_by_group_age, participants_group_age_category,
    participants_by_type_training, participants_type_training_category, participants_by_value_chain, participants_value_chain_category, participants_by_marital_status,
    participants_value_marital_status_category, participants_district_series, participants_county_series, participants_by_sector_type, technologies_category, technologies_by_type_producer) {
    Highcharts.chart('participantsBySexType', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_sex_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_sex
    });

    Highcharts.chart('participantsByGroupAge', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_group_age_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_group_age
    });

    Highcharts.chart('participantsByTypeTraining', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_type_training_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_type_training
    });

    Highcharts.chart('participantsByValueChain', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_chain_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_value_chain
    });

    Highcharts.chart('participantsByMaritalStatus', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_marital_status_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_marital_status
    });

    $('#participantsByGeo').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: participants_district_series,
            name: "Districts"
        }],
        drilldown: {
            series: participants_county_series
        }
    });

    Highcharts.chart('participantsByProducerType', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y}'
                }
            }
        },
        series: [{
            name: 'Participants',
            colorByPoint: true,
            data: participants_by_sector_type
        }]
    });

    Highcharts.chart('participantsByAppTech', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: technologies_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Number of technologies',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: 'Number of technologies'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: technologies_by_type_producer
    });
}



function loadchartSales(participants_sex_category, participants_by_sex, participants_by_group_age, participants_group_age_category, participants_by_marital_status, participants_value_marital_status_category,
    sales_by_organization, sales_value_organization_category, participants_district_series, participants_county_series, participants_type_vc_series, participants_species_series, firms_district_series,
    firms_county_series, firms_type_vc_series, firms_species_series, salesCategoryIndividualData) {


    Highcharts.chart('salesCategoryIndividual', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b>USD {point.y:.2f}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        credits: {
            enabled: false
        },
        series: [{
            name: 'Sales',
            colorByPoint: true,
            data: salesCategoryIndividualData
        }]
    });

    Highcharts.chart('participantsBySexType', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_sex_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_sex
    });

    Highcharts.chart('participantsByGroupAge', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_group_age_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_group_age
    });

    Highcharts.chart('salesByOrganization', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: sales_value_organization_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: sales_by_organization
    });

    Highcharts.chart('participantsByMaritalStatus', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_marital_status_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_marital_status
    });

    $('#participantsByGeo').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: participants_district_series,
            name: "Districts"
        }],
        drilldown: {
            series: participants_county_series
        }
    });

    $('#salesBySpecies').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: participants_type_vc_series,
            name: "Type of value chain"
        }],
        drilldown: {
            series: participants_species_series
        }
    });

    $('#firmsByGeo').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: firms_district_series,
            name: "Districts"
        }],
        drilldown: {
            series: firms_county_series
        }
    });

    $('#firmsBySpecies').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total sales USD',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: 'USD',
            pointFormat: '<b>{point.y:.2f} USD</b>'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: firms_type_vc_series,
            name: "Type of value chain"
        }],
        drilldown: {
            series: firms_species_series
        }
    });
}



function loadchartJobs(participants_sex_category, participants_by_sex, participants_by_group_age, participants_group_age_category, participants_by_marital_status, participants_value_marital_status_category,
    participants_district_series, participants_county_series, participants_by_value_chain, participants_value_chain_category, participants_by_type_emplyment, participants_type_emplyment_category) {
    Highcharts.chart('participantsBySexType', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_sex_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Full-time equivalent employment',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_sex
    });

    Highcharts.chart('participantsByGroupAge', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_group_age_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Full-time equivalent employment',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_group_age
    });

    Highcharts.chart('participantsByTypeEmployment', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_type_emplyment_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Full-time equivalent employment',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_type_emplyment
    });

    Highcharts.chart('participantsByValueChain', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_chain_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Full-time equivalent employment',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_value_chain
    });

    Highcharts.chart('participantsByMaritalStatus', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_value_marital_status_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Full-time equivalent employment',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_marital_status
    });

    $('#participantsByGeo').highcharts({
        chart: {
            type: 'column'
        },
        xAxis: {
            type: 'category'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valueSuffix: ' Full-time equivalent employment'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: participants_district_series,
            name: "Districts"
        }],
        drilldown: {
            series: participants_county_series
        }
    });

    //Highcharts.chart('participantsByProducerType', {
    //    chart: {
    //        plotBackgroundColor: null,
    //        plotBorderWidth: null,
    //        plotShadow: false,
    //        type: 'pie'
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    tooltip: {
    //        pointFormat: '{series.name}: <b>{point.y}</b>'
    //    },
    //    accessibility: {
    //        point: {
    //            valueSuffix: '%'
    //        }
    //    },
    //    plotOptions: {
    //        pie: {
    //            allowPointSelect: true,
    //            cursor: 'pointer',
    //            dataLabels: {
    //                enabled: true,
    //                format: '<b>{point.name}</b>: {point.y}'
    //            }
    //        }
    //    },
    //    series: [{
    //        name: 'Participants',
    //        colorByPoint: true,
    //        data: participants_by_sector_type
    //    }]
    //});

    //Highcharts.chart('participantsByAppTech', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        categories: technologies_category,
    //        title: {
    //            text: null
    //        }
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Number of technologies',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    tooltip: {
    //        valueSuffix: 'Number of technologies'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    series: technologies_by_type_producer
    //});
}



function loadchartPercentFemale(participants_by_sex, participants_by_group_age, participants_by_type_training, participants_type_training_category) {


    Highcharts.chart('participantsBySexType', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b># {point.y:.2f}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        credits: {
            enabled: false
        },
        series: [{
            name: 'Participants',
            colorByPoint: true,
            data: participants_by_sex
        }]
    });

    Highcharts.chart('participantsByGroupAge', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        tooltip: {
            pointFormat: '{series.name}: <b># {point.y:.2f}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        credits: {
            enabled: false
        },
        series: [{
            name: 'Participants',
            colorByPoint: true,
            data: participants_by_group_age
        }]
    });


    Highcharts.chart('participantsByTypeTraining', {
        chart: {
            type: 'column'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },
        colors: [
            '#002b6c',
            '#BB042B',
            '#5d5d5d'
        ],
        subtitle: {
            text: '',
            style: {
                display: 'none'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: participants_type_training_category,
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Participants',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: ' Participants'
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -10,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
            shadow: true
        },
        credits: {
            enabled: false
        },
        series: participants_by_type_training
    });

    //Highcharts.chart('participantsBySexType', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        categories: participants_sex_category,
    //        title: {
    //            text: null
    //        }
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    series: participants_by_sex
    //});

    //Highcharts.chart('participantsByGroupAge', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        categories: participants_group_age_category,
    //        title: {
    //            text: null
    //        }
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    series: participants_by_group_age
    //});

    //Highcharts.chart('salesByOrganization', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        categories: sales_value_organization_category,
    //        title: {
    //            text: null
    //        }
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    series: sales_by_organization
    //});

    //Highcharts.chart('participantsByMaritalStatus', {
    //    chart: {
    //        type: 'column'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    colors: [
    //        '#002b6c',
    //        '#BB042B',
    //        '#5d5d5d'
    //    ],
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        categories: participants_value_marital_status_category,
    //        title: {
    //            text: null
    //        }
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    series: participants_by_marital_status
    //});

    //$('#participantsByGeo').highcharts({
    //    chart: {
    //        type: 'column'
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    series: [{
    //        colorByPoint: true,
    //        data: participants_district_series,
    //        name: "Districts"
    //    }],
    //    drilldown: {
    //        series: participants_county_series
    //    }
    //});

    //$('#salesBySpecies').highcharts({
    //    chart: {
    //        type: 'column'
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    series: [{
    //        colorByPoint: true,
    //        data: participants_type_vc_series,
    //        name: "Type of value chain"
    //    }],
    //    drilldown: {
    //        series: participants_species_series
    //    }
    //});

    //$('#firmsByGeo').highcharts({
    //    chart: {
    //        type: 'column'
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    series: [{
    //        colorByPoint: true,
    //        data: firms_district_series,
    //        name: "Districts"
    //    }],
    //    drilldown: {
    //        series: firms_county_series
    //    }
    //});

    //$('#firmsBySpecies').highcharts({
    //    chart: {
    //        type: 'column'
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    title: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    subtitle: {
    //        text: '',
    //        style: {
    //            display: 'none'
    //        }
    //    },
    //    legend: {
    //        layout: 'vertical',
    //        align: 'right',
    //        verticalAlign: 'top',
    //        x: -10,
    //        y: 80,
    //        floating: true,
    //        borderWidth: 1,
    //        backgroundColor:
    //            Highcharts.defaultOptions.legend.backgroundColor || '#FFFFFF',
    //        shadow: true
    //    },
    //    yAxis: {
    //        min: 0,
    //        title: {
    //            text: 'Total sales USD',
    //            align: 'high'
    //        },
    //        labels: {
    //            overflow: 'justify'
    //        }
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    tooltip: {
    //        valueSuffix: 'USD',
    //        pointFormat: '<b>{point.y:.2f} USD</b>'
    //    },
    //    plotOptions: {
    //        bar: {
    //            dataLabels: {
    //                enabled: true
    //            }
    //        }
    //    },
    //    series: [{
    //        colorByPoint: true,
    //        data: firms_type_vc_series,
    //        name: "Type of value chain"
    //    }],
    //    drilldown: {
    //        series: firms_species_series
    //    }
    //});
}