$(document).ready(function () {

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html");
    //load list of employees from DB
    loadEmployees().then(function() {
        var table = $('#sort').DataTable();

        $('#tBody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
            $("#rowSelected").text(table.rows('.selected').data().length + ' row(s) selected');
            $('#EmployeeNotification').css({ 'visibility': 'hidden' });
        });
    })
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

function loadEmployees() {
        return $.ajax({
            type: "GET",
            url: '/api/employee',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                var id = 1;
                $.each(data, function (key, entry) {
                    var line = $('<tr>');
                    var fullName = entry.name;
                    var splitName = fullName.split(" ");
                    line.append($('<td data-table-header="Nr" id="NrColumn">').text(id))
                        .append($('</td><td data-table-header="Name">').text(splitName[0]))
                        .append($('</td><td data-table-header="Surname">').text(splitName[1]))
                        .append($('</td><td data-table-header="Email">').text(entry.email))
                        .append($('</td></tr>'));
                    $('#tBody').append(line);
                    id++;
                });
            },
            error: function () {alert('Internet error'); },
        })
}