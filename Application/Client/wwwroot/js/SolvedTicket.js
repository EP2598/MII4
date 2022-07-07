function editComment(commentId) {
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
    editSec.style.display = "none";
    editSec.disabled = true;
    cancelSec.style.removeProperty("display");
    cancelSec.disabled = false;
}
function getDetails(ticketId) {
    let objReq =
    {
        TicketID: ticketId
    }

    $.ajax({
        type: "post",
        url: "../Customer/GetTicketDetails/",
        data: objReq
    }).done((res) => {
        console.log(res.status);

        //Get Ticket Details
        let modalTitle = document.getElementById("modalTicketTitle");
        let ticketProgress = document.getElementById("ProgressBar");
        let innerProgress = "";
        switch (res.status) {
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
                else {
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
                cancelTicketBtn.style.display = "none";
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
            default:
                modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-danger">${res.status}</span>`;
                cancelTicketBtn.style.display = "none";
                innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span>&emsp;</span></li>
                    <li><time class="done"></time><span>&emsp;</span></li>
                    <li><time class="done"></time><span><strong>Declined</strong></span></li>
                `;
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

        let ticketId = document.getElementById("ticketId");
        ticketId.innerHTML = res.ticketId;

        document.getElementById("ticket-detail-createddate").innerHTML = "&emsp;" + moment(res.createdAt).format('DD MMMM yyyy HH:mm');
        document.getElementById("ticket-detail-cname").innerHTML = "&emsp;" + res.customerName;
        document.getElementById("ticket-detail-cemail").innerHTML = "&emsp;" + res.customerEmail;
        if (res.teamLeadName == null || res.teamLeadName == "") {
            document.getElementById("ticket-detail-developer").innerHTML = "&emsp;-";
        }
        else {
            document.getElementById("ticket-detail-teamlead").innerHTML = "&emsp;" + res.teamLeadName;
        }
        if (res.employeeName == null || res.employeeName == "") {
            document.getElementById("ticket-detail-developer").innerHTML = "&emsp;-";
        }
        else {
            document.getElementById("ticket-detail-developer").innerHTML = "&emsp;" + res.employeeName;
        }
        document.getElementById("ticket-detail-type").innerHTML = "&emsp;" + res.ticketType;
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
        for (var i = 0; i < res.commentOrder.length; i++) {
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
                        <button id="submitEdit${res.commentOrder[i]}" class="btn btn-primary btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                </div>
                    `;
            }
            else {
                commentSection += `
                <div class="form-row">
                    <div class="col-md-8 mb-3 mt-1 border-top">
                        <label for="commentSection">${res.commentSender[i]} - ${commentDate} (edited)</label>
                        <div class="float-right mt-1" id="editCancel${res.commentOrder[i]}" style="display:none"></div>
                        <br>
                        <span id="commentSectionSpan${res.commentOrder[i]}">&emsp;${res.commentBody[i]}</span>
                        <input id="commentSectionInput${res.commentOrder[i]}" type="text" class="form-control mb-2" value="${res.commentBody[i]}" style="display:none"></input>
                        <button id="submitEdit${res.commentOrder[i]}" class="btn btn-primary btn-sm" style="display:none" onclick="submitEdit(${res.commentOrder[i]})">Submit</button>
                    </div>
                </div>
                    `;
            }
            //Add Edit Cancel Comment Section


        }
        commentDiv.innerHTML = commentSection;
        addCommentDiv.innerHTML = addCommentButton;



        for (var i = 0; i < res.commentSenderId.length; i++) {
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
function submitEdit(commentId) {
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
function templateData(resultArray) {
    let cardCons = "";
    for (let i = 0; i < resultArray.length; i++) {
        cardCons += `
                    <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title"><span class="badge badge-info mr-2">${resultArray[i].ticketCategory}</span><span class="badge badge-success">${resultArray[i].ticketType}</span> ${resultArray[i].ticketId}</h5>
                                    <h6 class="card-subtitle mb-2 text-muted">${resultArray[i].status}</h6>
                                    <p class="card-text">${resultArray[i].description}</p>
                                    <a class="btn btn-primary" data-toggle="modal" data-target="#modalTicket" onclick='getDetails("${resultArray[i].ticketId}")'>Details</a>
                                </div>
                            </div>
                      </div>
                    <br>
                `;

    }
    return cardCons;
}
$(document).ready(() => {
    let cardDiv = document.getElementById("cardDiv");
    
    $.ajax({
        type: "GET",
        url: "../Customer/GetAllTickets/"
    }).done((res) => {
        let cardDiv = $("#cardDiv");
        $('#pagination-container').pagination({
            dataSource: res,
            pageSize: 5,
            ulClassName: 'pagination pagination-primary',
            callback: function (data, pagination) {
                console.log(data);
                var html = templateData(data);
                cardDiv.html(html);
                $(".pagination li").addClass("page-item");
                $(".pagination li a").addClass("page-link");
                $(".pagination li").css("border", "0px");
            }
        })
    }).fail((err) => {
        console.log("My Ticket - Error Log");
        console.log(err);
    });

    $("#categoryFilter").change(function (e) {
        var obj = {
            TicketCategory: e.target.value,
            TicketType: $("#typeFilter").val()
        };
        console.log(obj);
        $.ajax({
            url: "../Customer/GetAllTicketsByFilter",
            method: "POST",
            data: obj
        }).done((res) => {
            $("#cardDiv").empty();
            console.log(res);
            let cardDiv = $("#cardDiv");
            $('#pagination-container').pagination({
                dataSource: res,
                pageSize: 5,
                ulClassName: 'pagination pagination-primary',
                callback: function (data, pagination) {
                    console.log(data);
                    var html = templateData(data);
                    cardDiv.html(html);
                    $(".pagination li").addClass("page-item");
                    $(".pagination li a").addClass("page-link");
                    $(".pagination li").css("border", "0px");
                }
            })
        })
    });
    $("#typeFilter").change(function (e) {
        var obj = {
            TicketCategory: $("#categoryFilter").val(),
            TicketType: e.target.value
        };
        console.log(obj);
        $.ajax({
            url: "../Customer/GetAllTicketsByFilter",
            method: "POST",
            data: obj
        }).done((res) => {
            $("#cardDiv").empty();
            console.log(res);
            let cardDiv = $("#cardDiv");
            $('#pagination-container').pagination({
                dataSource: res,
                pageSize: 5,
                ulClassName: 'pagination pagination-primary',
                callback: function (data, pagination) {
                    console.log(data);
                    var html = templateData(data);
                    cardDiv.html(html);
                    $(".pagination li").addClass("page-item");
                    $(".pagination li a").addClass("page-link");
                    $(".pagination li").css("border", "0px");
                }
            })
        })
    })
})

