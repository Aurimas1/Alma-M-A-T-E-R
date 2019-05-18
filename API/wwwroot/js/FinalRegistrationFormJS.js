var idToNameMap = {};
var tripID = finalRegistrationTripId;
$(document).ready(function () {

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html", function () {
        loadTripTime().then(function (times) {
            var departureParts = times.departureDate.split("T");
            $("#departureDate").val(departureParts[0]);
            departureParts[1] = departureParts[1].substr(0, 5);
            $(`#departureTime option:contains(${departureParts[1]})`).attr('selected', 'selected');
            $('#departureTime').attr('disabled', true);
            var arrivalParts = times.returnDate.split("T");
            $("#arrivalDate").val(arrivalParts[0]);
            arrivalParts[1] = arrivalParts[1].substr(0, 5);
            $(`#arrivalTime option:contains(${arrivalParts[1]})`).attr('selected', 'selected');
            $('#arrivalTime').attr('disabled', true);
            
            //load events
            selectedEployeesForEvents = $('table#sort tbody tr td#NrColumn').map(function(){
                return $.trim($(this).text());
            }).get();

            //fill in selected dates
            fillInSelectedDates_FinalRegForm();
            //load
            monthButtonClicked();
            calendar_dates_selection_is_allowed = false;
            
        });
    });


    loadEmployees().then(function () { return loadTripEmployees(tripID); }).then(function () {

        var table = $('#sort').DataTable();

        $('#tBody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
            $("#rowSelected").text(table.rows('.selected').data().length + ' row(s) selected');
            var email = $(this).children('td:eq(3)').text();
            if ($(this).hasClass("selected")) {
                var name = $(this).children('td:eq(1)').text() + " " + $(this).children('td:eq(2)').text();
                var line = $("<tr>");
                line.append($('<td id="NrColumn">').text($(this).children('td:eq(0)').text()))
                    .append($('<td class="attrName">').text(name))
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

        $('.table-remove').click(function () {
            var email = $(this).parent("td").prev().text();
            $(this).parents('tr').detach();
            $(`#sort tr:contains(${email})`).prop('disabled', false).css("cssText", "background-color:");
        });

    });
});

function clickAccomodation() {
    $("#tableBody").empty();
    $("#employeeList").empty();
    $('#hotelRooms').remove();
    var booking = 0;
    var employee = [];
    $('#employeeTBody tr').each(function (a, b) {
        var name = $('.attrName', b).text();
        var id = $('#NrColumn', b).text();
        var line = $('<li class="list-group-item d-flex justify-content-between align-items-center">').text(name);
        employee.push({ id, name });
        $("#employeeList").append(line);
    });

    loadAccommodation(tripID).then(function (a) {
        let map = {};

        var i = 0;
        var emp2 = [];
        $.each(a, function (j, t) {
            var obj = employee.find(x => x.id == t.employeeID);
            if (t.isRoomIsOccupied && obj != null) {
                employee = $.grep(employee, function (e) {
                    return e.id != t.employeeID;
                });
                emp2.push(obj);
            }
        });

        $.each(a, function (j, t) {
            var tr = $('<tr>');
            var td = $('<td>').text(j).addClass('SmallColumn');
            tr.append(td);
            var dropDown = $('<select>').on('focus', function () {
                previous = $(this).find("option:selected").text();
            }).on("change", function () {
                if (previous !== '') {
                    $(`#employeeList li:contains(${previous})`).text(previous + '    - no room').css({ 'color': 'red' });
                    map[previous] = undefined;
                }
                var text = $(this).find("option:selected").text();
                if (text !== "") {
                    $(`#employeeList li:contains(${text})`).text(text + '    - ' + j + ' room').css({ 'color': 'black' });
                    const oldPosision = map[text];
                    if (oldPosision)
                        $(`#rooms_table tr:eq(${oldPosision}) select`).val('');
                    map[text] = j;
                }

                $("#bookingSpan").text($(`#employeeList li:contains(no room)`).length);
            });
            var obj = emp2.find(x => x.id == t.employeeID);
            if (t.isRoomIsOccupied) {
                if (obj == undefined) {
                    if (idToNameMap[t.employeeID] != null) {
                        obj = { id: t.employeeID, name: idToNameMap[t.employeeID] };
                        $.each(emp2, function (i, e) {
                            dropDown.append($('<option>').val(e.id).html(e.name));
                        });
                        $.each(employee, function (i, e) {
                            dropDown.append($('<option>').val(e.id).html(e.name));
                        });
                        dropDown.append("<option selected></option>")
                        td = $("<td>").append(dropDown);
                        tr.append(td);
                        $(`#employeeList li:contains(${obj.name})`).text(obj.name + '    - ' + j + ' room');
                    }
                    else {
                        tr.append('<td>The room is taken</td>');
                        tr.append(td);
                    }
                }
                else {
                    $.each(emp2, function (i, e) {
                        if (obj.id === e.id) {
                            dropDown.append($('<option selected>').val(e.id).html(e.name));
                        }
                        else {
                            dropDown.append($('<option>').val(e.id).html(e.name));
                        }
                    });
                    $.each(employee, function (i, e) {
                        dropDown.append($('<option>').val(e.id).html(e.name));
                    });
                    dropDown.append("<option></option>")
                    td = $("<td>").append(dropDown);
                    tr.append(td);
                    $(`#employeeList li:contains(${obj.name})`).text(obj.name + '    - ' + j + ' room');
                }
            }
            else {
                if (employee[i] != null) {
                    var name = employee[i].name;
                    $.each(employee, function (i, e) {
                        dropDown.append($('<option>').val(e.id).html(e.name));
                    });
                    $.each(emp2, function (i, e) {
                        dropDown.append($('<option>').val(e.id).html(e.name));
                    });
                    dropDown.prop('selectedIndex', i).css("width:100%");
                    dropDown.append("<option></option>")
                    td = $("<td>").append(dropDown);
                    tr.append(td);
                    $(`#employeeList li:contains(${name})`).text(name + '    - ' + j + ' room');
                    i++;
                }
                else {
                    $.each(employee, function (i, e) {
                        dropDown.append($('<option></option>').val(e.id).html(e.name));
                    });
                    $.each(emp2, function (i, e) {
                        dropDown.append($('<option></option>').val(e.id).html(e.name));
                    });
                    dropDown.append("<option selected></option>")
                    td = $("<td>").append(dropDown);
                    tr.append(td);
                    i++;
                }
            }
            $('#tableBody').append(tr);
        });


        for (var i; i < employee.length; i++) {
            var name = employee[i].name;
            $(`li:contains(${name})`).text(name + '    - no room').css({ 'color': 'red' });
            booking++;
        }
        $("#bookingSpan").text(booking);

        map = createMap();
    });
}

function loadTripTime() {
    return $.ajax({
        type: "GET",
        url: `/api/trip/time/${tripID}`,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function () { alert('Internet error'); },
    })
}

function loadAccommodation(id) {
    return $.ajax({
        type: "GET",
        url: `/api/apartment/${id}`,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function () { alert('Internet error'); },
    })
}

function loadTripEmployees(id) {
    return $.ajax({
        type: "GET",
        url: `/api/trip/employees/${id}`,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            $.each(data, function (key, entry) {
                var email = entry.email;
                var line = $("<tr>");
                line.append($('<td id="NrColumn">').text(entry.employeeID))
                    .append($('<td class="attrName">>').text(entry.name))
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
                idToNameMap[entry.employeeID] = entry.name;
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

function saveTrip() {
    var a = [];
    $.each($("#tableBody tr"), function (i, j) {
        if ($(j).children('td:eq(1)').text() !== 'The room is taken' && $(j).find('select').val() != undefined) {
            a.push({ roomID: $(j).children('td:eq(0)').text(), employeeID: $(j).find('select').val() })
        }
    });

    var check = false;
    a.map(function (x) { return x.employeeID }).forEach(function (x, i, arr) {
        if (arr.indexOf(x) !== i && x != "") {
            alert("You selected the same person two times in apartaments table");
            check = true;
        }
    });
    if (check) {
        return Promise.reject();
    }
    var employee = [];
    $('#employeeTBody tr').each(function (a, b) {
        var id = $('#NrColumn', b).text();
        employee.push(id);
    });

    $.ajax({
        type: "PUT",
        url: '/api/trip/' + tripID,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            "IsPlaneNeeded": $("#airplaneRadios").is(':checked'),
            "IsCarRentalNeeded": $("#carRadios").is(':checked'),
            "IsCarCompensationNeeded": $("#employeeCarRadios").is(':checked'),
            "Employees": employee,
            "Rooms": a.filter(function (x) {
                return x.employeeID != '';
            }),
        }),
        success: function () {
            alert("The trip was successfully created!");
            window.location.href = "/index.html";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    })
}

function createMap() {
    const map = {};
    $.each($('#rooms_table tr'), function (position, entry) { // position starts from 0
        const select = $(entry).find('td:eq(1) select');
        if (select) {
            const id = select.val();
            if (id)
                map[idToNameMap[id]] = position;
        }
    });
    return map;
}

function cancelTrip(){
    location.href = "/index.html";
}