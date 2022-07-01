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

function addComment()
{
    let objReq =
    {
        TicketId: document.getElementById("ticketId").innerHTML,
        AccountId:"",
        Description: $("#ticket-detail-inputComment").val()
    }

    $.ajax({
        type: "post",
        url: "../Comments/AddComment/",
        data: objReq
    }).done((res) => {
        console.log(res);

    }).fail((err) => {
        console.log("Add Comment - Error Log");
        console.log(err);
    });
}

function editComment(commentId)
{
    let commentSection = "commentSection" + commentId;
    let comSec = document.getElementById(commentSection);

    let editSection = "editComment" + commentId;
    let editSec = document.getElementById(editSection);

    let cancelSection = "cancelComment" + commentId;
    let cancelSec = document.getElementById(cancelSection);

    comSec.readOnly = false;
    cancelSec.disabled = false;
    editSec.disabled = true;
}

function cancelComment(commentId) {
    let commentSection = "commentSection" + commentId;
    let comSec = document.getElementById(commentSection);

    let editSection = "editComment" + commentId;
    let editSec = document.getElementById(editSection);

    let cancelSection = "cancelComment" + commentId;
    let cancelSec = document.getElementById(cancelSection);

    comSec.readOnly = true;
    cancelSec.disabled = true;
    editSec.disabled = false;
}

function getDetails(ticketId)
{
    let objReq =
    {
        TicketID: ticketId
    }

    $.ajax({
        type: "post",
        url: "../Customer/GetTicketDetails/",
        data: objReq
    }).done((res) => {
        console.log(res);

        //Get Ticket Details
        let modalTitle = document.getElementById("modalTicketTitle");
        if (res.status === "In Progress")
        {
            modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-warning">${res.status}</span>`;
        }
        else if (res.status === "Solved")
        {
            modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-success">${res.status}</span>`;
        }
        else
        {
            modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-danger">${res.status}</span>`;
        }

        let ticketId = document.getElementById("ticketId");
        ticketId.innerHTML = res.ticketId;
        
        $("#ticket-detail-createddate").val(moment(res.createdAt).format('DD-MM-yyyy'));
        $("#ticket-detail-cname").val(res.customerName);
        $("#ticket-detail-cemail").val(res.customerEmail);
        $("#ticket-detail-teamlead").val(res.teamLeadName);
        $("#ticket-detail-developer").val(res.employeeName);
        $("#ticket-detail-type").val(res.ticketType);
        $("#ticket-detail-description").val(res.description);

        //Tambah comment
        let commentDiv = document.getElementById("divComments");
        let commentSection = "";
        for (var i = 0; i < res.commentOrder; i++)
        {
            commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1">
                        <label for="ticket-detail-type">${res.commentSender[i]} - ${res.commentTimestamps[i]} (edited)</label>
                        <input id="commentSection" type="text" class="form-control" readonly="readonly">
                    </div>
                </div>
                `;
            //if (res.commentIsEdited[i] === true) {
            //    commentSection += `
            //    <div class="form-row">
            //        <div class="col-md-8 mb-3 mt-1">
            //            <label for="ticket-detail-type">${res.commentSender[i]} - ${res.commentTimestamp[i]} (edited)</label>
            //            <input id="commentSection${i}" type="text" class="form-control" readonly="readonly">
            //        </div>
            //    </div>
            //    `;
            //}
            //else
            //{
            //    commentSection += `
            //    <div class="form-row">
            //        <div class="col-md-8 mb-3 mt-1">
            //            <label for="ticket-detail-type">${res.commentSender[i]} - ${res.commentTimestamp[i]}</label>
            //            <input id="commentSection${i}" type="text" class="form-control" readonly="readonly">
            //        </div>
                    
            //    </div>
            //    `;

                //<div class="col-md-2 mb-3">
                //    <a onclick="editComment(${i})"><small>Edit</small></a>
                //    <a onclick="cancelComment(${i})" disabled><small>Cancel</small></a>
                //</div>
            /*}*/
        }
        commentDiv.innerHTML = commentSection;

    }).fail((err) => {
        console.log("Ticket Details - Error Log");
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
                                    <a class="btn btn-primary" data-toggle="modal" data-target="#modalTicket" onclick='getDetails("${res[i].ticketId}")'>Details</a>
                                </div>
                            </div>
                      </div>
                    <br>
                `;
           
        }
        cardDiv.innerHTML = cardCons;
    }).fail((err) => {
        console.log("My Ticket - Error Log");
        console.log(err);
    });
});