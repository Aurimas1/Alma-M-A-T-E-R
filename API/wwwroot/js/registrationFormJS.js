$(document).ready(function () {

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html");
    loadOffices();
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
    
    if($('input[name="exampleRadios"]:checked').val() == undefined)
    {
        alert("You didn't choose any departure office.");
        return;
    }
    else if($('input[name="exampleRadios2"]:checked').val() == undefined){
        alert("You didn't choose any arrival office.");
        return;
    }
        else if($('input[name="exampleRadios"]:checked').val() == $('input[name="exampleRadios2"]:checked').val()){
            alert("You choose the same departure and arrival office");
            return;
        }
    
    if (table.rows('.selected').data().length == '0') {
        alert("You didn't choose any employee.");
        return;
    }
    alert($("#departureDate").val());
    if($("#departureDate").val() == undefined){
        alert("You didn't choose departure date");
        return;
    }
    else if($("#arrivalDate").val() == undefined){
        alert("You didn't choose arrival date");
        return;
    }

    /*$.ajax({
        type: "POST",
        url: '/api/trip',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({"DepartureOfficeID": $('input[name="exampleRadios"]:checked').val(), "ArrivalOfficeID": $('input[name="exampleRadios2"]:checked').val()}),
        success: function (data) {
            alert('Trip was saved');
        },
        error: function () {alert('Internet error'); },
    })*/
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

function loadOffices() {
    return $.ajax({
        type: "GET",
        url: '/api/office',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            var id = 1;
            $.each(data, function (key, entry) {
                var line1 = $(`<div class="form-check"><input class="form-check-input" type="radio" name="exampleRadios" id="exampleRadios${id}" value=${entry.officeID}><label class="form-check-label" for="exampleRadios${id}">
                ${entry.city}, ${entry.country}</label></div>`);
                id++;
                var line2 = $(`<div class="form-check"><input class="form-check-input" type="radio" name="exampleRadios2" id="exampleRadios${id}" value=${entry.officeID}><label class="form-check-label" for="exampleRadios${id}">${entry.city}, ${entry.country}</label></div>`);
                id++;
                $('#js-dep-office').append(line1);
                $('#js-arr-office').append(line2);
            });
        },
        error: function () {alert('Internet error'); },
    })
}