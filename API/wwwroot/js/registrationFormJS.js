$(document).ready(function () {
    var table = $('#sort').DataTable();

    /*$.ajax({
        url: "",
        type: "GET",
        dataType: "json",
        success: function (data) {
            $.each(data, function () {
                var line = $('<tr>');
                line.append($('<th>').text($(this).Name))
                    .append($('<th>').text($(this).Surname))
                    .append($('<th>').text($(this).Email_address))
            $('#tBody').append(line);
            });
        },
        error: function () {alert('Internet error'); },
    });*/

    $('#tBody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        $("#rowSelected").text(table.rows('.selected').data().length + ' row(s) selected');
        $('#EmployeeNotification').css({ 'visibility': 'hidden' });
    });
});

function CheckEmployees() {
    const table = $('#sort').DataTable();
    if (table.rows('.selected').data().length == '0') {
        if ($('#oneButton').attr("aria-expanded") == "true") {
            $('#EmployeeNotification').text("Please select at least one employee!").css({ "color": "red", "visibility": "visible" });
        }
        else {
            alert('Please select at least one employee!');
            $('#EmployeeNotification').text("Please select the employees.").css({ "color": "black", "visibility": "visible" });
        }

        $('#Card2').removeAttr("data-toggle", "collapse");
        $('#Card3').removeAttr("data-toggle", "collapse");
    }
    else {
        $('#EmployeeNotification').css({ 'visibility': 'hidden' });
        $('#Card2').attr("data-toggle", "collapse");
        $('#Card3').attr("data-toggle", "collapse");
    }
}

function saveTrip() {
    const table = $('#sort').DataTable();
    if (table.rows('.selected').data().length == '0') {
        alert("You didn't choose any employee!");
    }
    //prideti dar kalendoriaus patikrinimą ir išsaugoti tada kelionę
}