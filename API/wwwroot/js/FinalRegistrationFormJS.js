$(document).ready(function () {
    var table = $('#sort').DataTable();

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html");
    
    /*$.ajax({
        url: "",
        type: "GET",
        dataType: "json",
        success: function (data) {
            employee = data;
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

    $('tr:contains("Šmigelskis")').prop('disabled', true).css("cssText", "background-color:#D33F49 !important;");
    $('tr:contains("Kiziela")').prop('disabled', true).css("cssText", "background-color:#D33F49 !important;");

    $('#tBody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        $("#rowSelected").text(table.rows('.selected').data().length + ' row(s) selected');
        $('#EmployeeNotification').css({ 'visibility': 'hidden' });
    });
    //----------------------------------------------------------
    var employee = [['Tomas', 'Kiziela'], ['Marijus', 'Šmigelskis']];
    var li = $('<li>').text("Tomas Kiziela").addClass("list-group-item").attr('id', 'li');
    $('#employeeList').append(li);
    var li = $('<li>').text("Marijus Šmigelskis").addClass("list-group-item").attr('id', 'li');
    $('#employeeList').append(li);

    var a = [['1', 'Jonas', 'Jonaitis'], ['2', '', ''], ['3', 'Povilas', 'Povilaitis'], ['4', '', ''], ['5', '', ''], ['6', 'Petras', 'Petraitis']];
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
                alert(name);
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
    }

    if ($("#CheckEmployeeCar").is(':checked')) {
        $("#employee_car").show();
    }
    if ($("#CheckRentCar").is(':checked')) {
        $("#car_rental").show();
    }
    if ($("#CheckPlane").is(':checked')) {
        $("#plane_div").show();
    }


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

function openPlane(){
    if ($("#plane_div2").is(":visible")) {
        $("#plane_div2").hide();
    }
    else {
        $("#plane_div2").show();
    }
}