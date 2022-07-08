$.ajax({
    url: "../Customer/GetAllTickets",
    type: "get"
}).done((res) => {
    console.log(res);
    let text = '';
    $.each(res, function (key, value) {
        let teamLeadexist = true;
        if (value.teamLeadName == null) {
            value.teamLeadName = "-";
            teamLeadexist = false;
        }
        if (value.employeeName == null) {
            value.employeeName = "-";
        }
        text += `<tr>
                    <td>${value.ticketId}</td>
                    <td>${value.customerName}</td>
                    <td>${value.teamLeadName}</td>
                    <td>${value.employeeName}</td>`
        if (value.ticketCategory == 'Other') {
            text += `<td>
                        <select class="form-control ticket-type-${value.ticketId}" id="${value.ticketId}">
                            <option selected>Other</option>
                            <option value="Administration">Administration</option>
                            <option value="IT Support">IT Support</option>
                        </select>
                      </td>
                     `;
        } else {
            text += `<td>${value.ticketCategory}</td>`;
        }

        text += `<td>${value.ticketType}</td>
                  <td><select class="form-control ticket-status-${value.ticketId}" id="${value.ticketId}">`
        if (value.status == "In Progress") {
            text += `<option value="In Progress" selected>In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>
                    <option value="Cancelled">Cancelled</option>`
        } else if (value.status == "Solved") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved" selected>Solved</option>
                    <option value="Declined">Declined</option>
                    <option value="Cancelled">Cancelled</option>`
        } else if (value.status == "Request to Escalate") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Request to Escalate" selected>Request to Escalate</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>
                    <option value="Cancelled">Cancelled</option>`
        }
        else if (value.status == "Declined") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined" selected>Declined</option>
                    <option value="Cancelled">Cancelled</option>`
        }
        else {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>
                    <option value="Cancelled" selected>Cancelled</option>`
        }
        text += `</td>
                    <td>${moment(value.createdAt).format('DD-MM-yyyy')}</td>`;
        if (!teamLeadexist) {
            text += `<td><span data-bs-toggle="tooltip" data-bs-placement="bottom" title="Assign Team Lead" ><button type="button" onclick="AssignModel('${value.ticketId}')" class="btn btn-info btn-sm" data-bs-toggle="modal" data-bs-target="#assignModal"><i class="fa-solid fa-person-circle-plus"></i></button></span></td>
                    </tr>`;
        } else {
            text += `<td><span data-bs-toggle="tooltip" data-bs-placement="bottom" title="Change Team Lead" ><button type="button" onclick="AssignModel('${value.ticketId}')" class="btn btn-info btn-sm" data-bs-toggle="modal" data-bs-target="#assignModal"><i class="fa-solid fa-people-arrows-left-right"></span></button></td>
                    </tr>`
        }
    })
    $("#ticket-table-admin").html(text);
});

$(document).ready(function () {
    $.ajax({
        url: "../Customer/GetAllTickets",
        type: "GET"
    }).done((res) => {
        $.each(res, function (key, value) {
            $(`.ticket-status-${value.ticketId}`).change(function (e) {
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
                    console.log(res);
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
            });
            $(`.ticket-type-${value.ticketId}`).change(function (e) {
                var elem = $(this);
                var obj = {
                    TicketId: elem.attr('id'),
                    Type: e.target.value
                };
                console.log(obj);
                $.ajax({
                    url: "../Admin/UpdateTypeTicket",
                    type: "POST",
                    data: obj
                }).done((res) => {
                    console.log(res);
                    switch (res.statusCode) {
                        case 200:
                            Swal.fire({
                                icon: 'success',
                                title: 'Update tipe tiket berhasil dilakukan!',
                            });
                            break;
                        default:
                            Swal.fire({
                                icon: 'error',
                                title: 'Update tipe tiket gagal dilakukan!',
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
    $.ajax({
        url: "../AccountRoles/GetTeamLead",
        type: "GET"
    }).done((res) => {
        console.log(res);
        let text = '';
        $("#ticketid-assign").html(ticketId);
        $.each(res, (key, value) => {
            text += `<div class="form-check">
                      <input type="radio" value="${value.employeeId}" name="teamLead" id="teamLead${key + 1}" class="teamlead-opt">
                      <label for="teamLead${key + 1}">${value.employeeName}</label>
                    </div>`
        });
        $(".modal-body").html(text);
    })
}
function AssignLead() {
    var obj = {
        TicketId: $("#ticketid-assign").html(),
        TeamLeadId: $("input[name='teamLead']:checked").val()
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
                    title: 'Assign Team Lead berhasil dilakukan!',
                });
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Assign Team Lead gagal dilakukan!',
                });
                break;
        }
    })

}