$(document).ready(function () {
    //System Statistic
    $.ajax({
        type: "GET",
        url: "/Customer/GetSystemStatistic"
    }).done((res) => {
        var options = {
            dataLabels: {
                enabled: false,
            },
            chart: {
                type: 'bar',
                height: 350
            },
            plotOptions: {
                bar: {
                    borderRadius: 4,
                    horizontal: true
                }
            },
            series: [{ data: res.data.statusCount }],
            xaxis: {
                categories: res.data.statusName
            }
        }

        var chart = new ApexCharts(document.querySelector("#innerDetailTickets"), options);

        chart.render();


    });
})