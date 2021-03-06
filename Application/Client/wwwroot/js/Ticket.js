function doRequest()
{
    let btnSubmit = document.getElementById("btnSubmit");
    btnSubmit.disabled = true;
    let objReq =
    {
        CustomerID: "",
        CustomerName: "",
        CustomerEmail: "",
        CustomerPhone: "",
        TeamLeadID: "",
        TicketCategory: $("#inputTicketCategory").val(),
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
                    btnSubmit.disabled = false;
                    window.location.assign("../Customer/MyTicket");
                });
                break;
            default:
                btnSubmit.disabled = false;
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
    let addCommentBtn = document.getElementById("addCommentBtn");
    addCommentBtn.disabled = true;
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
        $("#modalTicket").modal("toggle");
        getDetails(ticketId);
        $("#modalTicket").modal("toggle");
        addCommentBtn.disabled = false;

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
    btnSubmit.disabled = false;
    editSec.style.display = "none";
    editSec.disabled = true;
    cancelSec.style.removeProperty("display");
    cancelSec.disabled = false;
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
    cancelSec.style.display = "none";
    cancelSec.disabled = true;
    editSec.style.removeProperty("display");
    editSec.disabled = false;
}

function submitEdit(commentId)
{
    let submitBtnId = "submitEdit" + commentId;
    let submitBtn = document.getElementById(submitBtnId);
    submitBtn.disabled = true;
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
        comSecSpan.innerHTML = objReq.Description;
        comSecInput.style.display = "none";
        btnSubmit.style.display = "none";
        cancelSec.style.display = "none";
        cancelSec.disabled = true;
        editSec.style.removeProperty("display");
        editSec.disabled = false;
    });
}

function confirmCancel(ticketId) {
    Swal.fire({
        title: 'Cancel ticket?',
        text: "Are you sure you want to cancel this ticket?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            let objReq =
            {
                TicketId: ticketId,
                Status: "Cancelled"
            }
            $.ajax({
                url: "../Customer/UpdateTicket",
                type: "put",
                data: objReq
            }).done((res) => {
                if (res == 200) {
                    Swal.fire(
                        'Success!',
                        'Your request has been submitted',
                        'success'
                    ).then((result) => {
                        location.reload();
                    });
                }
                else {
                    Swal.fire(
                        'Failed!',
                        'Your request has not been submitted',
                        'error'
                    );
                }
            });
            
        }
    })

    let objReq =
    {
        TicketId: ticketId,
        Status: "Cancelled"
    }
    $.ajax({
        url: "../Customer/UpdateTicket",
        type: "put",
        data: obj
    }).done((res) => {

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
        let ticketProgress = document.getElementById("StepProgress");
        let innerProgress = "";
        let modalFooter = document.getElementById("modalTicketFooter");
        let currUserRole = document.getElementById("currUserRole").innerHTML;
        let btnSubmit;
        if (currUserRole === "Developer") {
            let divSubmit = document.getElementById("divSubmitSolution");
            divSubmit.innerHTML = `<button id="submitSolution" class="btn bg-gradient-info" onclick='submitSolution("${res.ticketId}")'>Submit Solution</button>`;

            btnSubmit = document.getElementById("submitSolution");
            if (res.status === "Submitted") {
                btnSubmit.style.display = "none";
            }
        }
        let btnAccept;
        let btnDecline;
        if (currUserRole === "Customer") {
            let divSelect = document.getElementById("divSelectSolution");
            divSelect.innerHTML = `<button id="confirmWork" class="btn bg-gradient-success" onclick='confirmWork("${res.ticketId}")'>Accept Solution</button>
                                                            <button id="denyWork" class="btn bg-gradient-danger" onclick='denyWork("${res.ticketId}")'>Decline Solution</button>`;

            btnAccept = document.getElementById("confirmWork");
            btnAccept.style.display = "none";
            btnDecline = document.getElementById("denyWork");
            btnDecline.style.display = "none";
        }
        if (res.status === "Solved" || res.status === "Cancelled" || res.status === "Declined") {
            modalFooter.style.display = "none";
        }
        else {
            if (currUserRole === "Customer") {
                modalFooter.style.removeProperty("display");
                modalFooter.innerHTML = `<button id="cancelTicketBtn" type="button" class="btn btn-danger" onclick='confirmCancel("${res.ticketId}")'>Cancel Ticket</button>`;
            }
        }
        switch (res.status)
        {
            case "In Progress":
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-warning">${res.status}</span>`;
                if (res.employeeName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="current"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                }
                else if (res.teamLeadName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="current"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="incomplete"></time><span><strong>Handled by </strong> -</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;                  
                }
                else
                {
                    innerProgress = `
                    <li><time class="current"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="incomplete"></time><span><strong>Assigned to </strong> -</span></li>
                    <li><time class="incomplete"></time><span><strong>Handled by </strong> -</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                }
                break;
            case "Solved":
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-success">${res.status}</span>`;
                innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="done"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="done"></time><span><strong>Solved</strong></span></li>
                `;
                break;
            case "Request to Escalate":
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-warning">${res.status}</span>`;
                if (res.employeeName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>Escalated</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="current"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                }
                else if (res.teamLeadName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>Escalated</strong></span></li>
                    <li><time class="current"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="incomplete"></time><span><strong>Handled by </strong> -</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                }
                else {
                    innerProgress = `
                    <li><time class="current"></time><span><strong>Escalated</strong></span></li>
                    <li><time class="incomplete"></time><span><strong>Assigned to </strong> - </span></li>
                    <li><time class="incomplete"></time><span><strong>Handled by </strong> - </span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                }
                break;
            case "Submitted":
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-warning">In Progress</span>`;
                if (currUserRole === "Developer") {
                    btnSubmit.style.display = "none";
                }
                else if (currUserRole === "Customer") {
                    btnAccept.style.removeProperty("display");
                    btnDecline.style.removeProperty("display");
                }
                innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="current"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="incomplete"></time><span><strong>Solved</strong></span></li>
                `;
                break;
            case "Declined":
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-danger">${res.status}</span>`;
                innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span>&emsp;</span></li>
                    <li><time class="done"></time><span>&emsp;</span></li>
                    <li><time class="done"></time><span><strong>Declined</strong></span></li>
                `;
                break;
                break;
            default:
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-danger">${res.status}</span>`;
                if (res.employeeName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="done"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="done"></time><span><strong>Cancelled</strong></span></li>
                `;
                }
                else if (res.teamLeadName != null) {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="done"></time><span><strong>Handled by </strong> -</span></li>
                    <li><time class="done"></time><span><strong>Cancelled</strong></span></li>
                `;
                }
                else {
                    innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> - </span></li>
                    <li><time class="done"></time><span><strong>Handled by </strong> - </span></li>
                    <li><time class="done"></time><span><strong>Cancelled</strong></span></li>
                `;
                }
                break;
        }
        ticketProgress.innerHTML = innerProgress;
        //if (res.status === "In Progress")
        //{
        //    modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-warning">${res.status}</span>`;
        //}
        //else if (res.status === "Solved")
        //{
        //    modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-success">${res.status}</span>`;
        //}
        //else
        //{
        //    modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-danger">${res.status}</span>`;
        //}
        
        document.getElementById("ticket-detail-createddate").innerHTML = "&emsp;" + moment(res.createdAt).format('DD MMMM yyyy HH:mm');
        document.getElementById("ticket-detail-cname").innerHTML = "&emsp;" + res.customerName;
        document.getElementById("ticket-detail-cemail").innerHTML = "&emsp;" + res.customerEmail;
        if (res.teamLeadName == null || res.teamLeadName == "") {
            document.getElementById("ticket-detail-teamlead").innerHTML = "&emsp;-";
        }
        else {
            document.getElementById("ticket-detail-teamlead").innerHTML = "&emsp;" + res.teamLeadName;
        }
        if (res.employeeName == null || res.employeeName == "") {
            document.getElementById("ticket-detail-developer").innerHTML = "&emsp;-";
        }
        else
        {
            document.getElementById("ticket-detail-developer").innerHTML = "&emsp;" + res.employeeName;
        }
        document.getElementById("ticket-detail-type").innerHTML = "&emsp;" + res.ticketType;
        $("#ticket-detail-description").val(res.description);

        //Tambah comment
        let addCommentButton = `<label for="ticket-detail-inputComment">Comments</label>
                                <input type="text" class="form-control" id="ticket-detail-inputComment" placeholder="Add comment to this ticket...">
                                <span id="ticketId" style="display:none"></span>
                                <button id="addCommentBtn" type="button" class="btn bg-gradient-info mt-2" onclick='addComment("${res.ticketId}")'>Comment</button>`;
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
                    <div class="col-md-8 mb-3 mt-1 border-top">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate}</label>
                        <div class="float-right mt-1" id="editCancel${res.commentOrder[i]}" style="display:none"></div>
                        <br>
                        <span id="commentSectionSpan${res.commentOrder[i]}">&emsp;${res.commentBody[i]}</span>
                        <input id="commentSectionInput${res.commentOrder[i]}" type="text" class="form-control mb-2" value="${res.commentBody[i]}" style="display:none"></input>
                        <button id="submitEdit${res.commentOrder[i]}" class="btn bg-gradient-info btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                </div>
                    `;
            }
            else
            {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1 border-top">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate} (edited)</label>
                        <div class="float-right mt-1" id="editCancel${res.commentOrder[i]}" style="display:none"></div>
                        <br>
                        <span id="commentSectionSpan${res.commentOrder[i]}">&emsp;${res.commentBody[i]}</span>
                        <input id="commentSectionInput${res.commentOrder[i]}" type="text" class="form-control mb-2" value="${res.commentBody[i]}" style="display:none"></input>
                        <button id="submitEdit${res.commentOrder[i]}" class="btn bg-gradient-info btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                </div>
                    `;
            }
            //Add Edit Cancel Comment Section
            

        }
        commentDiv.innerHTML = commentSection;

        if (res.status !== "Cancelled") {
            addCommentDiv.innerHTML = addCommentButton;
            let ticketId = document.getElementById("ticketId");
            ticketId.innerHTML = res.ticketId;
        }
        

        for (var i = 0; i < res.commentSenderId.length; i++)
        {
            var now = moment();
            var commentTimestamp = moment(res.commentTimestamps[i]);
            var dateDiff = now.diff(commentTimestamp, 'days');
            if (res.commentSenderId[i] === currUserId) {
                if (dateDiff <= 2) {
                    let editCancelSection = "editCancel" + res.commentOrder[i];
                    let thisCommentSection = document.getElementById(editCancelSection);
                    let innerSection = `
                            <button id="editComment${res.commentOrder[i]}" class="btn btn-sm" onclick="editComment(${res.commentOrder[i]})"><i class="fas fa-cog"></i> Edit</button>
                            <button id="cancelComment${res.commentOrder[i]}" style="display:none" disabled="true" class="btn btn-sm" onclick="cancelComment(${res.commentOrder[i]})"><i class="fas fa-times"></i> Cancel</button>
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

function submitSolution(ticketId) {
    let btnSubmit = document.getElementById("submitSolution");
    btnSubmit.disabled = true;
    Swal.fire({
        title: 'Submit solution?',
        text: "Are you sure you want to submit?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            let objReq =
            {
                TicketId: ticketId,
                Status: "Submitted"
            }
            $.ajax({
                url: "../Customer/UpdateTicket",
                type: "put",
                data: objReq
            }).done((res) => {
                if (res == 200) {
                    Swal.fire(
                        'Success!',
                        'Your request has been submitted',
                        'success'
                    ).then((result) => {
                        btnSubmit.disabled = false;
                        location.reload();
                    });
                }
                else {
                    btnSubmit.disabled = false;
                    Swal.fire(
                        'Failed!',
                        'Your request has not been submitted',
                        'error'
                    );
                }
            });

        }
    })
}

function confirmWork(ticketId) {
    let btnConfirm = document.getElementById("confirmWork");
    let btnDeny = document.getElementById("denyWork");

    btnConfirm.disabled = true;
    btnDeny.disabled = true;
    Swal.fire({
        title: 'Accept solution?',
        text: "Are you sure you want to accept this solution?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            let objReq =
            {
                TicketId: ticketId,
                Status: "Solved"
            }
            $.ajax({
                url: "../Customer/UpdateTicket",
                type: "put",
                data: objReq
            }).done((res) => {
                if (res == 200) {
                    btnConfirm.disabled = false;
                    btnDeny.disabled = false;
                    Swal.fire(
                        'Success!',
                        'Your request has been submitted',
                        'success'
                    ).then((result) => {
                        location.reload();
                    });
                }
                else {
                    btnConfirm.disabled = false;
                    btnDeny.disabled = false;
                    Swal.fire(
                        'Failed!',
                        'Your request has not been submitted',
                        'error'
                    );
                }
            });

        }
    })
}

function denyWork(ticketId) {
    Swal.fire({
        title: 'Decline solution?',
        text: "Are you sure you want to decline this solution?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            let objReq =
            {
                TicketId: ticketId,
                Status: "In Progress"
            }
            $.ajax({
                url: "../Customer/UpdateTicket",
                type: "put",
                data: objReq
            }).done((res) => {
                if (res == 200) {
                    Swal.fire(
                        'Success!',
                        'Your request has been submitted',
                        'success'
                    ).then((result) => {
                        location.reload();
                    });
                }
                else {
                    Swal.fire(
                        'Failed!',
                        'Your request has not been submitted',
                        'error'
                    );
                }
            });

        }
    })
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
        let cardDiv = $("#cardDiv");
        $('#pagination-container').pagination({
            dataSource: res,
            pageSize: 5,
            ulClassName: 'pagination pagination-primary',
            callback: function (data, pagination) {
                /*$("#cardDiv").empty();*/
               /* console.log(data);*/
                for (var i = 0; i < data.length; i++) {
                    cardCons += `
                    <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title"><span class="badge badge-info mr-2">${data[i].ticketCategory}</span><span class="badge badge-success">${data[i].ticketType}</span> ${data[i].ticketId}</h5>
                                    <h6 class="card-subtitle mb-2 text-muted">${data[i].status}</h6>
                                    <p class="card-text">${data[i].description}</p>
                                    <a class="btn bg-gradient-info" data-toggle="modal" data-target="#modalTicket" onclick='getDetails("${data[i].ticketId}")'>Details</a>
                                </div>
                            </div>
                      </div>
                    <br>
                `;

                }
                /*var html = templateData(data);*/
                cardDiv.html(cardCons);
                $(".pagination li").addClass("page-item");
                $(".pagination li a").addClass("page-link");
                $(".pagination li").css("border", "0px");
                cardCons = "";
            }
        })
      
    }).fail((err) => {
        console.log("My Ticket - Error Log");
        console.log(err);
    });
});

$('#modalTicket').on('hidden.bs.modal', function (e) {
    $("#modalTicket").modal("dispose");
})