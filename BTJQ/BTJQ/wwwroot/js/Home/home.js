var student = student || {};

student.drawTable = () => {
    $.ajax({
        url: '/Home/Gets',
        method: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data.status === 1) {
                var response = data.response;
                $('#tbStudent').empty();
                $.each(response, function (index, value) {
                    $('#tbStudent').append("<tr>" +
                        "<td>" + value.studentId + "</td>" +
                        "<td>" + value.fullName + "</td>" +
                        "<td>" + value.dob + "</td>" +
                        "<td>" + value.sex + "</td>" +
                        "<td>" + value.className + "</td>" +
                        //"<td>" + value.jobName + "</td>" +
                        "<td>" +
                        "<a href ='javascript:void(0);' onclick='user.showEditModal(" + value.id + ")' > <i class='fas fa-edit'></i></a>" +
                        "<a href ='javascript:void(0);'> <i class='fas fa-trash-alt'></i></a>" +
                        "</td > " +
                        "</tr>");
                });
            }
        }
    });
};

student.init = () => {
    student.drawTable();
};


$(document).ready(() => {

    student.init();
});

