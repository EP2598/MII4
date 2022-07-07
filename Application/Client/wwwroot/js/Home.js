$(document).ready(function () {
    let currUserRole = document.getElementById("currUserRole").innerHTML;

    if (currUserRole === "Admin") {
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
    }
    else if (currUserRole === "Team Lead") {
        //Subordinate Statistic
        let objReq =
        {
            AccountId: document.getElementById("currUserAccountId").innerHTML
        }
        $.ajax({
            type: "POST",
            url: "/Customer/GetSubordinateStatistic",
            data: objReq
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
                        horizontal: false
                    }
                },
                series: [{ data: res.data.ticketCount }],
                xaxis: {
                    categories: res.data.employeeName
                }
            }

            var chart = new ApexCharts(document.querySelector("#innerSelfDetail"), options);

            chart.render();
        });
    }
    else
    {
        //Personal Statistic
        let objReq =
        {
            AccountId: document.getElementById("currUserAccountId").innerHTML
        }
        $.ajax({
            type: "POST",
            url: "/Customer/GetPersonalStatistic",
            data: objReq
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

            var chart = new ApexCharts(document.querySelector("#innerSelfDetail"), options);

            chart.render();
        });
    }
    

    
})