var idToNameMap = {};
var approvedIds = [];
var tripID = finalRegistrationTripId;
var map = {};

$(document).ready(function () {

    //load EventsCalendar
    $("div.events_calendar").load("../Calendar_DB.html", function () {
        loadTripTimeAndTransport().then(function (times) {
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

            if (times.isPlaneNeeded) {
                $("#airplaneRadios").prop("checked", true);
            }
            if (times.isCarCompensationNeeded) {
                $("#employeeCarRadios").prop("checked", true);
            }
            if (times.isCarRentalNeeded) {
                $("#carRadios").prop("checked", true);
            }

            //load events
            selectedEployeesForEvents = $('table#sort tbody tr td#NrColumn').map(function () {
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
                    .append($('<td id="statusColumn">'))
                    .append($("<td>").append($("<span class='table-remove'>").append($("<button class='btn btn-danger'>").text("Remove").on("click", function () {
                        if (confirm("Do you want to delete an employee?")) {
                            $(this).parents('tr').detach();
                            $(`#sort tr:contains(${email})`).removeClass("selected");
                        }
                    }))));
                $('#employeeTBody').append(line);
            }
            else {
                $(`#employeeTBody tr td:contains(${email})`).parents("tr").remove();
            }
            $('#EmployeeNotification').css({ 'visibility': 'hidden' });
        });

        $('.table-remove').click(function () {
            if (confirm("Do you want to delete an employee?")) {
                var email = $(this).parent("td").prev().text();
                $(this).parents('tr').detach();
                $(`#sort tr:contains(${email})`).prop('disabled', false).css("cssText", "background-color:");
            }
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
        var status = $('#statusColumn', b).text();
        if (status == "APPROVED") {
            var name = $('.attrName', b).text();
            var id = $('#NrColumn', b).text();
            var line = $('<li class="list-group-item d-flex justify-content-between align-items-center">');
            line.text(name + " has its own place to stay").css({ "color": "blue" });
            employee.push({ id, name });
            $("#employeeList").append(line);
        }

    });

    loadAccommodation(tripID).then(function (a) {
        let withoutRoom = [...approvedIds];

        // populate table
        for (let roomNr in a) {
            let tr = $('<tr>'); // line
            let td1 = $('<td>').text(roomNr).addClass('SmallColumn'); //first col
            let td2 = $('<td>'); //second col
            let accommodation = a[roomNr];

            if (accommodation.isRoomIsOccupied && accommodation.employeeID) {
                withoutRoom.splice(withoutRoom.indexOf(accommodation.employeeID), 1);
                $(`#employeeList li:contains(${idToNameMap[accommodation.employeeID]})`).text(idToNameMap[accommodation.employeeID] + '    - ' + roomNr + ' room').css({ "color": "green" });
                td2.append(createAccommodationDropdown(roomNr).val(accommodation.employeeID));
            } else if (accommodation.isRoomIsOccupied && !accommodation.employeeID) {
                td2.text('The room is taken');
            } else {
                td2.append(createAccommodationDropdown(roomNr).val(''));
            }

            tr.append(td1).append(td2);
            $('#tableBody').append(tr);
        }

        //prefill
        for (let empId of withoutRoom) {
            let unset = true;
            $.each($('#tableBody select'), (_, select) => {
                if (unset && !$(select).val()) {
                    $(select).val(empId);
                    let nr = $(select).parent().parent().find('td:eq(0)').text();
                    $(`#employeeList li:contains(${idToNameMap[empId]})`).text(idToNameMap[empId] + '    - ' + nr + ' room').css({ "color": "green" });
                    unset = false;
                }
            });
        }

        createMap();
    });
}

function loadTripTimeAndTransport() {
    return $.ajax({
        type: "GET",
        url: `/api/trip/timeAndTransport/${tripID}`,
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
            approvedIds = [];
            $.each(data, function (key, entry) {
                var tdStatus = $("<td id='statusColumn'>").text(entry.status);
                if (entry.status === 'APPROVED') {
                    tdStatus.css({
                        "color": "green",
                        "font-weight": 600
                    });
                    approvedIds.push(entry.employeeID);
                }
                else {
                    tdStatus.css({
                        "color": "red",
                        "font-weight": 600
                    });
                }
                var email = entry.email;
                var line = $("<tr>");
                line.append($('<td id="NrColumn">').text(entry.employeeID))
                    .append($('<td class="attrName">>').text(entry.name))
                    .append($("<td>").text(email))
                    .append(tdStatus)
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
        return;
    }
    var employee = [];
    $('#employeeTBody tr').each(function (a, b) {
        var id = $('#NrColumn', b).text();
        employee.push(id);
    });

    if (employee.length === 0) {
        alert("Zero employees are selected");
        return;
    }

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
            alert("The trip was edited!");
            window.tripDetailsTripId = tripID;
            $("div#pageContent").load("../trip_details.html");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    })
}

function createMap() {
    map = {};
    $.each($('#rooms_table tr'), function (position, entry) { // position starts from 0
        let select = $(entry).find('td:eq(1) select');
        if (select) {
            let id = select.val();
            if (id)
                map[idToNameMap[id]] = position;
        }
    });
}

function createAccommodationDropdown(roomNr) {
    let select = $('<select>');
    for (let id of approvedIds) {
        let option = $('<option>').val(id).text(idToNameMap[id]);
        select.append(option);
    }
    select.append($('<option>'));
    addEventHandlerForSelect(select, roomNr);
    return select;
}

function cancelTrip() {
    location.href = "/index.html";
}

function addEventHandlerForSelect(s, roomNr) {
    let previous;
    s.on('focus', function () {
        previous = $(this).find("option:selected").text();
    }).on("change", function () {
        if (previous !== '') {
            $(`#employeeList li:contains(${previous})`).text(previous + '    - no room').css({ 'color': 'red' });
            map[previous] = undefined;
        }
        var text = $(this).find("option:selected").text();
        if (text !== "") {
            $(`#employeeList li:contains(${text})`).text(text + '    - ' + roomNr + ' room').css({ 'color': 'black' });
            let oldPosision = map[text];
            if (oldPosision)
                $(`#rooms_table tr:eq(${oldPosision}) select`).val('');
            map[text] = roomNr;
        }

        $("#bookingSpan").text($(`#employeeList li:contains(no room)`).length);
    });
}