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
        console.log(res);
        console.log(res.status);

        //Get Ticket Details
        let modalTitle = document.getElementById("modalTicketTitle");
        let ticketProgress = document.getElementById("ProgressBar");
        let innerProgress = "";

        modalTitle.innerHTML = `${res.ticketId} <span class="badge badge-success">${res.status}</span>`;
        innerProgress = `
                    <li><time class="done"></time><span><strong>In Progress</strong></span></li>
                    <li><time class="done"></time><span><strong>Assigned to </strong> ${res.teamLeadName}</span></li>
                    <li><time class="done"></time><span><strong>Handled by </strong> ${res.employeeName}</span></li>
                    <li><time class="done"></time><span><strong>Solved</strong></span></li>
                `;

        ticketProgress.innerHTML = innerProgress;

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
        let commentDiv = document.getElementById("divComments");
        let commentSection = "";
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
function templateData(resultArray) {
    console.log(resultArray);
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
        type: "POST",
        url: "../Customer/GetAllTicketsByFilter"
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

