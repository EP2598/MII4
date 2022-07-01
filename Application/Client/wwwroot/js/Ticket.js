function doRequest()
{
    let objReq =
    {
        CustomerID: "",
        CustomerName: "",
        CustomerEmail: "",
        CustomerPhone: "",
        TeamLeadID: "",
        TicketType: $("#inputTicketType").val(),
        Description: $("#inputDescription").val(),
    }

    $.ajax({
        type: "post",
        url: "../Customer/RequestTicket/",
        data: objReq
    }).done((res) => {
        console.log(res);
        switch (res.statusCode) {
            case 200:
                setTimeout(function () {
                    window.location.assign("../Customer/MyTicket");
                }, 5500);
                Swal.fire({
                    icon: 'success',
                    title: 'Request submitted!',
                    html: 'Ticket has been submitted.'
                }).then(function () {
                    window.location.assign("../Customer/MyTicket");
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Request failed!',
                    text: res.message,
                })
        }
    }).fail((err) => {
        console.log("Request Ticket - Error Log");
        console.log(err);
    });
}

$(document).ready(function () {
    let cardDiv = document.getElementById("cardDiv");
    let cardCons = "";

    let objReq =
    {
        AccountId: ""
    }

    $.ajax({
        type: "post",
        url: "../Customer/GetMyTickets/",
        data: objReq
    }).done((res) => {
        console.log(res);

        for (var i = 0; i < res.length; i++)
        {
            cardCons += `
                    <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title"><span class="badge badge-success">${res[i].ticketType}</span> ${res[i].ticketId}</h5>
                                    <h6 class="card-subtitle mb-2 text-muted">${res[i].status}</h6>
                                    <p class="card-text">${res[i].description}</p>
                                    <a href="#" class="btn btn-primary">Details</a>
                                </div>
                            </div>
                      </div>
                    <br>
                `;
           
        }
        //$.each(res, function (key, value) {
        //    cardCons += `
        //    <div class="card" style="width: 18rem;">
        //      <div class="card-body">
        //        <h5 class="card-title"><span class="badge badge-success">${value.ticketType}</span> ${value.ticketId}</h5>
        //        <h6 class="card-subtitle mb-2 text-muted">${value.status}</h6>
        //        <p class="card-text">${value.description}</p>
        //        <a href="#" class="btn btn-primary">Details</a>
        //      </div>
        //    </div>
        //`;
        //})

        cardDiv.innerHTML = cardCons;
        
        //switch (res.statusCode) {
        //    case 200:
        //        setTimeout(function () {
        //            window.location.replace("../");
        //        }, 5500);
        //        Swal.fire({
        //            icon: 'success',
        //            title: 'Request submitted!',
        //            html: 'Password has been changed!'
        //        }).then(function () {
        //            window.location.replace("../");
        //        });
        //        break;
        //    default:
        //        Swal.fire({
        //            icon: 'error',
        //            title: 'Request failed!',
        //            text: res.message,
        //        })
        //}
    }).fail((err) => {
        console.log("My Ticket - Error Log");
        console.log(err);
    });
});