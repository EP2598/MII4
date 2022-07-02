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

function addComment(ticketId)
{
    let objReq =
    {
        TicketId: ticketId,
        AccountId:"",
        Description: $("#ticket-detail-inputComment").val()
    }

    $.ajax({
        type: "post",
        url: "../Comments/AddComment/",
        data: objReq
    }).done((res) => {
        console.log(res);
        $("#modalTicket").modal("toggle");
        getDetails(ticketId);
        $("#modalTicket").modal("toggle");

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
        let addCommentButton = `<label for="ticket-detail-inputComment">Comments</label>
                                <input type="text" class="form-control" id="ticket-detail-inputComment" placeholder="Add comment to this ticket...">
                                <span id="ticketId" style="display:none"></span>
                                <button type="button" class="btn btn-primary mt-2" onclick='addComment("${res.ticketId}")'>Comment</button>`;
        let addCommentDiv = document.getElementById("addCommentDiv");
        let commentDiv = document.getElementById("divComments");
        let commentSection = "";
        for (var i = 0; i < res.commentOrder.length; i++)
        {
            let commentDate = moment(res.commentTimestamps[i]).format('DD-MM-yyyy HH:mm')
            if (res.commentIsEdited[i] == false) {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate}</label>
                        <br>
                        <span id="commentSection">${res.commentBody[i]}</span>
                    </div>
                </div>
                    `;
            }
            else
            {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate}</label>
                        <br>
                        <span id="commentSection">${res.commentBody[i]}</span>
                    </div>
                </div>
                    `;
            }
        }
        commentDiv.innerHTML = commentSection;
        addCommentDiv.innerHTML = addCommentButton;

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

$('#modalTicket').on('hidden.bs.modal', function (e) {
    $("#modalTicket").modal("dispose");
})