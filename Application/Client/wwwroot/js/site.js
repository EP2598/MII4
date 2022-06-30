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

function TestSwal() {
    Swal.fire({
        title: "User berhasil dibuat",
        icon: "success"
    })
}
function Register() {
    var obj = new Object();
    obj.Name = $("#name-register").val();
    obj.Email = $("#email-register").val();
    obj.Phone = $("#phone-register").val();
    obj.TeamLeadID = $("#teamleadid-register").val();
    obj.RoleID = $("#roleid-register").val();
    console.log(obj);
    $.ajax({
        url: "https://localhost:44365/Accounts/Register",
        type: "POST",
        data: obj,
        success: function (result) {
            Swal.fire({
                title: "User berhasil dibuat",
                icon: "success"
            })
        }
    })
}