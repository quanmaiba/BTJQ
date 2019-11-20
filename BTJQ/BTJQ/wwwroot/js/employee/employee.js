var employee = employee || {};

employee.drawDataTable = function () {
     dataTableOption =  $("#tbEmployee").DataTable({
        "processing": false, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "ajax": {
            "url": "/Employee/Gets",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "employeeID",
                "name": "EmployeeID",
                "autoWidth": true,
                "title": "Employee ID",
                "searchable": true,
                "orderable": true
            },
            {
                "data": "employeeName",
                "name": "EmployeeName",
                "autoWidth": true,
                "title": "Employee Name",
                "searchable": true,
                "orderable": true
            },
            {
                "data": "phoneNumber",
                "name": "PhoneNumber",
                "autoWidth": true,
                "title": "PhoneNumber",
                "searchable": true,
                "orderable": true
            },
            {
                "data": "skill",
                "name": "Skill",
                "autoWidth": true,
                "title": "Skill",
                "searchable": true,
                "orderable": true
            },
            {
                "data": "yearsExperience",
                "name": "YearsExperience",
                "autoWidth": true,
                "title": "YearsExperience",
                "orderable": true
            },
            {
                "data": "employeeID",
                "render": function (data, type, full, meta) {
                    return "<a href='javascript:void(0);' onclick='employee.showEditModal(" + data + ")'><i class='fas fa-edit'></i></a>" + ' ' + "<a href='javascript:void(0);' onclick='employee.showConfirmDeleteModal(" + data +")' ><i class='fas fa-trash-alt'></i></a>";
                },
                "title": "Actions",
                "orderable": false
            }
        ]

    });
};


employee.showEditModal = function (id) {
  
    $.ajax({
        url: 'Employee/Get/' + id,
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
            if (data.code === 1) {
                console.log(data.code);
                var response = data.response;
                console.log(response);
                $("#EmployeeID").val(response.employeeID);
                $("#EmployeeName").val(response.employeeName);
                $("#PhoneNumber").val(response.phoneNumber);
                //$("#SkillId").val(response.SkillId);
                $("#YearsExperience").val(response.yearsExperience);     
                $("#Skill").prop('selectedIndex', response.skillID);

                $('#addEditEmployee').find(".modal-title").text("Sửa Nhân Viên");
                $('#addEditEmployee').modal('show');
              
            }
        }
    });
};

employee.showModel = function () {
    employee.resetForm();
    $('#addEditModal').find(".modal-title").text("Thêm Nhân Viên");
    $('#addEditEmployee').modal('show')
};

employee.save = function () {
 
    var objEmployee = {};
    var EmployeeID = $("#EmployeeID").val();
    if (EmployeeID == 0) {
        objEmployee["EmployeeID"] = 0;
    } else {
        objEmployee["EmployeeID"] = EmployeeID;
    }
    objEmployee["EmployeeName"] = $("#EmployeeName").val();
    objEmployee["PhoneNumber"] = $("#PhoneNumber").val();
    objEmployee["SkillId"] = $("#Skill").val();
    objEmployee["YearsExperience"] = $("#YearsExperience").val();
  
    $.ajax({
        url: "Employee/Save",
        type: 'POST',
        data: JSON.stringify(objEmployee),
        
        contentType: 'application/json',
        datatype: 'json',
        success: function (data) {
            if (data.status === 1) {              
                $('#addEditEmployee').modal('hide');
                 employee.reloadDataTable();
                //employee.resetForm();
                //user.drawDataTable();
            }
        }
    }); 
    //console.log(data);
    //employee.init();
};

employee.resetForm = function () {
    $('#EmployeeID').val('');
    $('#EmployeeName').val('');
    $('#PhoneNumber').val('');
    $('#YearsExperience').val('');
    $('#SkillId').prop('selectedIndex',0);
    //employee.initSkill();
};

employee.reloadDataTable = function () {
    dataTableOption.ajax.reload(null, false);
}
employee.initSkill = function () {
    $.ajax({
        url: 'Employee/GetSkill',
        method: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
            if (data.code === 1) {
                var response = data.response;
                $.each(response, function (index, value) {
                    $('#Skill').append(
                        "<option value=" + value.id + ">" + value.name + "</option>"
                    );
                });
            }
        }

    });
};


employee.init = function () {
    employee.initSkill();
    employee.resetForm();
    employee.drawDataTable();
};

$(document).ready(function () {
    employee.init();
});

//$(document).ready(function () {
//    //$("#employeeView").DataTable();
//    $("#employeeView").DataTable({
//        "processing": true, // for show progress bar  
//        "serverSide": true, // for process server side  
//        "filter": true, // this is for disable filter (search box)  
//        "orderMulti": false,
//    });
//});