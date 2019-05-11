$(document).ready(function () {

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html");
    loadReservedApartments();
    loadPlaneTickets();
    loadCarRentals();
    loadGasCompensations();

    loadEmployees().then(function () { return loadTripEmployees(); }).then(function () {

        var table = $('#sort').DataTable();
        
        $('#tBody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
            $("#rowSelected").text(table.rows('.selected').data().length + ' row(s) selected');
            var email = $(this).children('td:eq(3)').text();
            if ($(this).hasClass("selected")) {
                var name = $(this).children('td:eq(1)').text() + " " + $(this).children('td:eq(2)').text();
                var line = $("<tr>");
                line.append($('<td id="NrColumn">').text($(this).children('td:eq(0)').text()))
                    .append($('<td>').text(name))
                    .append($("<td>").text(email))
                    .append($("<td>").append($("<span class='table-remove'>").append($("<button class='btn btn-danger'>").text("Remove").on("click", function () {
                        $(this).parents('tr').detach();
                        $(`#sort tr:contains(${email})`).removeClass("selected");
                    }))));
                $('#employeeTBody').append(line);
            }
            else {
                $(`#employeeTBody tr td:contains(${email})`).parents("tr").remove();
            }
            $('#EmployeeNotification').css({ 'visibility': 'hidden' });
        });
        //----------------------------------------------------------

        $('.table-remove').click(function () {
            var email = $(this).parent("td").prev().text();
            $(this).parents('tr').detach();
            $(`#sort tr:contains(${email})`).prop('disabled', false).css("cssText", "background-color:");
        });

        /*var a = [['1', 'Jonas', 'Jonaitis'], ['2', '', ''], ['3', 'Povilas', 'Povilaitis'], ['4', '', ''], ['5', '', ''], ['6', 'Petras', 'Petraitis']];
        //--------------------------------------------------------------
        var i = 0;
        $.each(a, function () {
            var tr = $('<tr>');
            var td = $('<td>').text($(this).get(0)).addClass('SmallColumn');
            tr.append(td);
            if ($(this).get(1) != '') {
                tr.append('<td>The room is taken</td>');
            }
            else {
                if (employee[i] != null) {
                    var name = employee[i][0] + " " + employee[i][1];
                    var dropDown = $('<select>');
                    $.each(employee, function () {
                        var emp = $(this).get(0) + " " + $(this).get(1);
                        dropDown.append("<option>" + emp + "</option>");
                    });
                    dropDown.prop('selectedIndex', i).css("width:100%");
                    td = $("<td>").append(dropDown);
                    tr.append(td);
                    $(`#li:contains(${name})`).text(name + '    - ' + $(this).get(0) + ' room');
                    i++;
                }
                else {
                    var dropDown = $('<select>');
                    $.each(employee, function () {
                        var emp = $(this).get(0) + " " + $(this).get(1);
                        dropDown.append("<option>" + emp + "</option>");
                    });
                    dropDown.append("<option></option>");
                    dropDown.prop('selectedIndex', i).css("width:100%;position:absolute");
                    td = $("<td>").append(dropDown);
                    tr.append(td);
                    i++;
                }
            }
            $('#tableBody').append(tr);
        });
        if (employee[i] != null) {
            var booking = 0;
            $.each(employee, function () {
                var name = employee[i] + " " + employee[i];
                $(`li:contains(${name})`).text(name + '    - no room').css({ 'color': 'red' });
                i++;
                booking++;
            });
            $('#listDiv').append(`<p>${booking} rooms need to be booking in the hotel</p>`);
        }*/

        if ($("#CheckEmployeeCar").is(':checked')) {
            $("#employee_car").show();
        }
        if ($("#CheckRentCar").is(':checked')) {
            $("#car_rental").show();
        }
        if ($("#CheckPlane").is(':checked')) {
            $("#plane_div").show();
        }
    })
});

/*function thirdCard() {
        $('#changeRooms').show();
        var number = 0;
        var employee = [];
        var dropDownas = $('<select>');
        $.each(table.rows('.selected').data(), function () {
            var li = $('<li>').text($(this).get(1) + " " + $(this).get(2)).addClass("list-group-item");
            $('#employeeList').append(li);
            employee[number] = $(this);
            var name = employee[number].get(1) + " " + employee[number].get(2);
            number++;
            dropDownas.append("<option>" + name + "</option>");
        });
        
        var booking = 0;
        $.each(employee, function () {
            var name = employee[i].get(1) + " " + employee[i].get(2);
            $(`li:contains(${name})`).text(name + '    - no room').css({ 'color': 'red' });
            i++;
            booking++;
        });
        $('#listDiv').append(`<p>${booking} rooms need to be booking in the hotel</p>`);
    }
}*/

function airplane() {
    if ($("#plane_div").is(":visible")) {
        $("#plane_div").hide();
    }
    else {
        $("#plane_div").show();
    }
}
function rentalCar() {
    if ($("#car_rental").is(":visible")) {
        $("#car_rental").hide();
    }
    else {
        $("#car_rental").show();
    }
}
function employeeCar() {
    if ($("#employee_car").is(":visible")) {
        $("#employee_car").hide();
    }
    else {
        $("#employee_car").show();
    }
}

function openPlane() {
    if ($("#plane_div2").is(":visible")) {
        $("#plane_div2").hide();
    }
    else {
        $("#plane_div2").show();
    }
}

function loadTripEmployees() {
    return $.ajax({
        type: "GET",
        url: '/api/trip/employees?id=11', //Insert current trip ID instead of 4
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            $.each(data, function (key, entry) {
                var email = entry.email;
                var line = $("<tr>");
                line.append($('<td id="NrColumn">').text(entry.employeeID))
                    .append($('<td>').text(entry.name))
                    .append($("<td>").text(email))
                    .append($("<td>").append($("<span class='table-remove'>").append($("<button class='btn btn-danger'>").text("Remove"))));
                $("#employeeTBody").append(line);
                $(`#sort tr:contains(${email})`).prop('disabled', true).css("cssText", "background-color:#D33F49");
            });
        },
        error: function () { alert('Internet error'); },
    })
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
            $.each(data, function (key, entry) {
                var line = $('<tr>');
                var fullName = entry.name;
                var splitName = fullName.split(" ");
                line.append($('<td data-table-header="Nr" id="NrColumn">').text(entry.employeeID))
                    .append($('</td><td data-table-header="Name"> id="EmployeeName"').text(splitName[0]))
                    .append($('</td><td data-table-header="Surname">').text(splitName[1]))
                    .append($('</td><td data-table-header="Email">').text(entry.email))
                    .append($('</td></tr>'));
                $('#tBody').append(line);
            });
        },
        error: function () { alert('Internet error'); },
    })
}

function loadReservedApartments() {
    return $.ajax({
        type: "GET",
        url: '/api/trip/reservedApartments?id=4', //Insert current trip ID instead of 4
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {

        },
        error: function () { alert('Internet error'); },
    })
}

function loadPlaneTickets() {
    return $.ajax({
        type: "GET",
        url: '/api/trip/planeTickets?id=4', //Insert current trip ID instead of 4
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {

        },
        error: function () { alert('Internet error'); },
    })
}

function loadCarRentals() {
    return $.ajax({
        type: "GET",
        url: '/api/trip/carRentals?id=4', //Insert current trip ID instead of 4
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {

        },
        error: function () { alert('Internet error'); },
    })
}

function loadGasCompensations() {
    return $.ajax({
        type: "GET",
        url: '/api/trip/gasCompensations?id=4', //Insert current trip ID instead of 4
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {

        },
        error: function () { alert('Internet error'); },
    })
}