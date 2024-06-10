$(function () {
    $("#dateInput").datepicker({
        dateFormat: "dd.mm.yy"
    });
});

function openDeleteModal(employeeId) {
    // Логируем для отладки
    console.log("openDeleteModal called with employeeId:", employeeId);
    // Отправляем асинхронный запрос на получение содержимого модального окна
    $.get('/Employee/Delete/' + employeeId, function (data) {
        // Логируем ответ сервера
        console.log("Data received from server:", data);

        // Убеждаемся, что элемент #deleteModal существует в DOM
        if ($('#deleteModal').length === 0) {
            // Если элемент не существует, добавляем его в конец body
            $('body').append(data);
        } else {
            // Обновляем содержимое модального окна данными из ответа на запрос
            $('#deleteModal').replaceWith(data);
        }
        // Инициализируем и открываем модальное окно
        $('#deleteModal').modal({
            backdrop: true,
            keyboard: true
        });

        $('#deleteModal').modal('show');
    }).fail(function (jqXHR, textStatus, errorThrown) {
        // Логируем ошибки
        console.error("Error loading modal content:", textStatus, errorThrown);
    });
}