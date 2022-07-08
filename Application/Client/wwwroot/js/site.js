// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Add option for team lead
$.ajax({
    url: "https://localhost:44365/AccountRoles/GetTeamLead"
}).done((res) => {
    console.log(res);
    let text = "";
    $.each(res, function (key, value) {
        text += `<option value="${value.employeeId}">${value.employeeName}</option>`;
    });
    $("#teamleadid-register").html(text);
})



$.ajax({
    url: "https://localhost:44365/Roles/GetRole"
}).done((res) => {
    let text = "";
    $.each(res, function (key, value) {
        text += `<option value="${value.roleId}">${value.roleName}</option>`;
    });
    $("#roleid-register").html(text);
});


function Register() {
    let btnRegister = document.getElementById("btnRegister");
    btnRegister.disabled = true;

    var obj = new Object();
    obj.Name = $("#name-register").val();
    obj.Email = $("#email-register").val();
    obj.Phone = $("#phone-register").val();
    obj.TeamLeadID = $("#teamleadid-register").val();
    obj.RoleID = $("#roleid-register").val();
    console.log(obj);
    $.ajax({
        url: "https://localhost:44365/Accounts/Register",
        data: obj,
        type: "post"
    }).done((res) => {
        console.log(res);
        console.log(res.result.statusCode);
        btnRegister.disabled = false;
        if (res.result.statusCode == 400) {
            Swal.fire({
                icon: 'error',
                title: 'Warning',
                text: res.result.message,
            })
        }
        else {
            Swal.fire({
                icon: 'success',
                title: 'User berhasil dibuat',
                text: '',
            })
        }
    })
}

function validateTeamLead() {
    //Disable Phone for Employee
    if (document.getElementById("roleid-register").value == 1 || document.getElementById("roleid-register").value == 2 || document.getElementById("roleid-register").value == 3) {
        document.getElementById("phone-register").disabled = true;
    }
    else {
        document.getElementById("phone-register").disabled = false;
    }
    //Disable Team Lead for TeamLead / Customer
    if (document.getElementById("roleid-register").value == 1 || document.getElementById("roleid-register").value == 2 || document.getElementById("roleid-register").value == 4) {
        document.getElementById("teamleadid-register").value = "";
        document.getElementById("teamleadid-register").disabled = true;
    }
    else {
        $("#teamleadid-register").val($("#teamleadid-register option:first").val());
        document.getElementById("teamleadid-register").disabled = false;
    }
}
