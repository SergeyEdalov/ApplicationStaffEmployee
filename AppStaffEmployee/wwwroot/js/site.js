$(function () {
    $("#dateInput").datepicker({
        dateFormat: "dd.mm.yy"
    });
});

function openDeleteModal(employeeId) {
    console.log("openDeleteModal called with employeeId:", employeeId);
    $.get('/Employee/Delete/' + employeeId, function (data) {
        console.log("Data received from server:", data);

        if ($('#deleteModal').length === 0) {
            $('body').append(data);
        } else {
            $('#deleteModal').replaceWith(data);
        }
        $('#deleteModal').modal({
            backdrop: true,
            keyboard: true
        });

        $('#deleteModal').modal('show');
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("Error loading modal content:", textStatus, errorThrown);
    });
}