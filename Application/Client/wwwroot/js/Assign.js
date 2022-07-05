$.ajax({
    url: "../Customer/GetAllTickets",
    type: "get"
}).done((res) => {
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
                    <td>${value.employeeName}</td>
                    <td>${value.ticketType}</td>
                    <td><select class="form-control ticket-type" id="${value.ticketId}">`
        if (value.status == "In Progress") {
            text += `<option value="In Progress" selected>In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>`
        } else if (value.status == "Solved") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved" selected>Solved</option>
                    <option value="Declined">Declined</option>`
        } else if (value.status == "Request to Escalate") {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Request to Escalate" selected>Request to Escalate</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined">Declined</option>`
        }
        else {
            text += `<option value="In Progress">In Progress</option>
                    <option value="Solved">Solved</option>
                    <option value="Declined" selected>Declined</option>`
        }
        text += `</td>
                    <td>${moment(value.createdAt).format('DD-MM-yyyy')}</td>`;
        if (!teamLeadexist) {
            text += `<td><button type="button" onclick="AssignModel('${value.ticketId}')" class="btn btn-info btn-sm" data-bs-toggle="modal" data-bs-target="#assignModal">Assign Team Lead</button></td>
                    </tr>`;
        } else {
            text += `<td><button type="button" onclick="AssignModel('${value.ticketId}')" class="btn btn-info btn-sm" data-bs-toggle="modal" data-bs-target="#assignModal">Change Team Lead</button></td>
                    </tr>`
        }
    })
    $("#ticket-table-admin").html(text);
});

$(document).ready(function () {
    $(`.ticket-type`).change(function (e) {
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