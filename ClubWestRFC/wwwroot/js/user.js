﻿var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/applicationUser",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "fullName", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "fullNameFamily1", "width": "15%" },
       

            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //currently user is locked
                        return ` <div class="text-center">
                                <a class="btn btn-danger text-white" style="cursor:pointer; width:100px;" onclick=LockUnlock('${data.id}')>
                                   <i class="fas fa-lock-open"></i> Unlock
                                </a></div>`;

                    }
                    else {
                        return ` <div class="text-center">
                                <a class="btn btn-success text-white" style="cursor:pointer; width:100px;" onclick=LockUnlock('${data.id}')>
                                    <i class="fas fa-lock"></i> Lock
                                </a></div>`;
                    }

                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function LockUnlock(id) {

    $.ajax({
        type: 'POST',
        url: '/api/ApplicationUser',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });

}