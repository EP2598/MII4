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
    let commentSectionSpan = "commentSectionSpan" + commentId;
    let comSecSpan = document.getElementById(commentSectionSpan);

    let commentSectionInput = "commentSectionInput" + commentId;
    let comSecInput = document.getElementById(commentSectionInput);

    let buttonSubmit = "submitEdit" + commentId;
    let btnSubmit = document.getElementById(buttonSubmit);

    let editSection = "editComment" + commentId;
    let editSec = document.getElementById(editSection);

    let cancelSection = "cancelComment" + commentId;
    let cancelSec = document.getElementById(cancelSection);

    comSecSpan.style.display = "none";
    comSecInput.style.removeProperty("display");
    btnSubmit.style.removeProperty("display");
    cancelSec.disabled = false;
    editSec.disabled = true;
}

function cancelComment(commentId) {
    let commentSectionSpan = "commentSectionSpan" + commentId;
    let comSecSpan = document.getElementById(commentSectionSpan);

    let commentSectionInput = "commentSectionInput" + commentId;
    let comSecInput = document.getElementById(commentSectionInput);

    let buttonSubmit = "submitEdit" + commentId;
    let btnSubmit = document.getElementById(buttonSubmit);

    let editSection = "editComment" + commentId;
    let editSec = document.getElementById(editSection);

    let cancelSection = "cancelComment" + commentId;
    let cancelSec = document.getElementById(cancelSection);

    comSecSpan.style.removeProperty("display");
    comSecInput.style.display = "none";
    btnSubmit.style.display = "none";
    cancelSec.disabled = true;
    editSec.disabled = false;
}

function submitEdit(commentId)
{
    let commentSectionInput = "commentSectionInput" + commentId;
    let comSecInput = document.getElementById(commentSectionInput).value;

    let objReq =
    {
        CommentId: commentId,
        Description: comSecInput
    }

    $.ajax({
        type: "post",
        url: "../Comments/EditComment/",
        data: objReq
    }).done((res) => {
        console.log(res);
    });
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
        
        $("#ticket-detail-createddate").val(moment(res.createdAt).format('DD MMMM yyyy HH:mm'));
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
        let currUserId = document.getElementById("currUserAccountId").innerHTML;
        for (var i = 0; i < res.commentOrder.length; i++)
        {
            let commentDate = moment(res.commentTimestamps[i]).format('DD MMMM yyyy HH:mm')
            //Add Comment Section
            if (res.commentIsEdited[i] == false) {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate}</label>
                        <br>
                        <span id="commentSectionSpan${res.commentOrder[i]}">&emsp;${res.commentBody[i]}</span>
                        <input id="commentSectionInput${res.commentOrder[i]}" type="text" class="form-control mb-2" value="${res.commentBody[i]}" style="display:none"></input>
                        <button id="submitEdit${res.commentOrder[i]}" class="btn btn-primary btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                    <div class="col-md-3 mb-3 mt-1" id="editCancel${res.commentOrder[i]}" style="display:none">
                    </div>
                </div>
                    `;
            }
            else
            {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate} (edited)</label>
                        <br>
                        <span id="commentSectionSpan${res.commentOrder[i]}">&emsp;${res.commentBody[i]}</span>
                        <input id="commentSectionInput${res.commentOrder[i]}" type="text" class="form-control mb-2" value="${res.commentBody[i]}" style="display:none"></input>
                        <button id="submitEdit${res.commentOrder[i]}" class="btn btn-primary btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                    <div class="col-md-3 mb-3 mt-1" id="editCancel${res.commentOrder[i]}" style="display:none">
                    </div>
                </div>
                    `;
            }
            //Add Edit Cancel Comment Section
            

        }
        commentDiv.innerHTML = commentSection;
        addCommentDiv.innerHTML = addCommentButton;

        

        for (var i = 0; i < res.commentSenderId.length; i++)
        {
            var now = moment();
            var commentTimestamp = moment(res.commentTimestamps[i]);
            var dateDiff = now.diff(commentTimestamp, 'days');
            if (res.commentSenderId[i] === currUserId) {
                if (dateDiff <= 5) {
                    let editCancelSection = "editCancel" + res.commentOrder[i];
                    let thisCommentSection = document.getElementById(editCancelSection);
                    let innerSection = `
                            <button id="editComment${res.commentOrder[i]}" class="btn btn-success btn-sm" onclick="editComment(${res.commentOrder[i]})">Edit</button>
                            <button id="cancelComment${res.commentOrder[i]}" disabled="true" class="btn btn-danger btn-sm" onclick="cancelComment(${res.commentOrder[i]})">Cancel</button>
`;

                    thisCommentSection.innerHTML = innerSection;
                    thisCommentSection.style.removeProperty("display");
                }
            }
        }

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