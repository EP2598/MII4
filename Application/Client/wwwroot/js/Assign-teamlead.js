let objReq =
{
    AccountId: ""
}
$.ajax({
    url: "../Customer/GetMyTickets",
    type: "post",
    data: objReq
}).done((res) => {
    let text = '';
    $.each(res, function (key, value) {
        let employeeExist = true;
        if (value.employeeName == null) {
            value.employeeName = "-";
            employeeExist = false;
        }
        text += `<tr>
                    <td>${value.ticketId}</td>
                    <td>${value.customerName}</td>
                    <td>${value.teamLeadName}</td>
                    <td>${value.employeeName}</td>
                    <td>${value.ticketType}</td>
                    <td><select class="form-control ticket-type-${value.ticketId}" id="${value.ticketId}">`
        if (value.status == "In Progress") {
            text += `<option value="In Progress" selected>In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>`
        } else if (value.status == "Solved") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved" selected>Solved</option>
                    <option value="Declined">Declined</option>`
        } else if (value.status == "Request to Escalate") {
            text += `<option value="Request to Escalate" selected>Request to Escalate</option>
                    <option value="In Progress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>`;
        }else {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined" selected>Declined</option>`
        }
        text += `</td>
                    <td>${moment(value.createdAt).format('DD-MM-yyyy')}</td>`;
        if (!employeeExist) {
            text += `<td><span data-bs-toggle="tooltip" data-bs-placement="bottom" title="Assign Employee" ><button type="button" onclick="AssignModel('${value.ticketId}')" class="btn btn-info btn-sm" data-bs-toggle="modal" data-bs-target="#assignModal"><i class="fa-solid fa-person-circle-plus"></i></button></span></td>
                `;
        }

        text += `<td><span data-bs-toggle="tooltip" data-bs-placement="bottom" title="Escalate Ticket" ><button type="button" onclick="RequestEscalate('${value.ticketId}')" class="btn btn-info btn-sm"><i class="fa-solid fa-people-arrows-left-right"></i></button></span></td>
                </tr>`;

    })
    $("#ticket-table-admin").html(text);
    $.each(res, function (key, value) {
        if (value.status == "Request to Escalate") {
            $(`#${value.ticketId}`).attr('disabled', 'disabled');
        }
    })
});

$(document).ready(function () {
    $.ajax({
        url: "../Customer/GetMyTickets",
        type: "post",
        data: objReq
    }).done((res) => {
        $.each(res, function (key, value) {
            $(`.ticket-type-${value.ticketId}`).change(function (e) {
                var elem = $(this);
                var obj = {
                    TicketId: elem.attr('id'),
                    Status: e.target.value
                };
                console.log(obj);
                $.ajax({
                    url: "../Customer/UpdateTicket",
                    type: "put",
                    data: obj
                }).done((res) => {
                    switch (res) {
                        case 200:
                            Swal.fire({
                                icon: 'success',
                                title: 'Update status berhasil dilakukan!',
                            });
                            break;
                        default:
                            Swal.fire({
                                icon: 'error',
                                title: 'Update status gagal dilakukan!',
                            });
                            break;
                    }
                })
            })
        })
    })
    
})

function AssignModel(ticketId) {
    console.log(ticketId);
    let objReq =
    {
        AccountId: ""
    }
    $.ajax({
        url: "../AccountRoles/GetEmployees",
        type: "POST",
        data: objReq
    }).done((res) => {
        console.log(res);
        let text = '';
        $("#ticketid-assign").html(ticketId);
        $.each(res, (key, value) => {
            text += `<div class="form-check">
                      <input type="radio" value="${value.employeeId}" name="employee" id="employee${key + 1}" class="employee-opt">
                      <label for="employee${key + 1}">${value.employeeName}</label>
                    </div>`
        });
        $(".modal-body").html(text);
    })
}
function AssignEmployee() {
    var obj = {
        TicketId: $("#ticketid-assign").html(),
        EmployeeId: $("input[name='employee']:checked").val()
    };
    console.log(obj);

    $.ajax({
        url: "../Customer/AssignTicket",
        method: "PUT",
        data: obj
    }).done((res) => {
        switch (res) {
            case 200:
                Swal.fire({
                    icon: 'success',
                    title: 'Assign Employee berhasil dilakukan!',
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Assign Employee gagal dilakukan!',
                });
                break;
        }
    })

}

function RequestEscalate(ticketId) {
    var obj = {
        TicketId: ticketId,
        TeamLeadId: ""
    };
    $.ajax({
        url: "../TeamLead/EscalateTicket",
        type: "POST",
        data: obj
    }).done((res) => {
        console.log(res);
        switch (res.statusCode) {
            case 200:
                Swal.fire({
                    icon: 'success',
                    title: 'Request escalate berhasil dilakukan!',
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Request escalate gagal dilakukan!',
                });
                break;
        }
    })
}