var ID = window.tripDetailsTripId;
$(document).ready(function () {
    $.ajax({
        url: 'api/trip/' + ID,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function () {
            console.error('Error');
        },
        success: function (data) {
            var departure = moment(data.departureDate).format('YYYY-MM-DD HH:mm');
            var arrival = moment(data.returnDate).format('YYYY-MM-DD HH:mm');
            $("#Arrival").text(data.arrivalCity + ', ' + data.arrivalCountry);
            $("#Departure").text(data.departureCity + ', ' + data.departureCountry);
            $("#DepartureDate").text(departure);
            $("#ArrivalDate").text(arrival);
            $("#Status").text(data.status);

            if (data.employeeName !== undefined) {
                for (var i = 0; i < data.employeeName.length; i++) {
                    var fullName = data.employeeName[i].split(" ");
                    var Nr = data.employeeName.length - i;

                    $('#EmployeesTable').append(
                        $('<tr>')
                            .append($('<td>').text(Nr))
                            .append($('<td>').text(fullName[0]))
                            .append($('<td>').text(fullName[1]))
                            .append($('<td>').text(data.employeeEmail[i]))
                    );

                    $('#employeeSelect').append($(`<option value="${data.employeeID[i]}" selected>`).text(data.employeeName[i]));
                }
            }

            if (data.isPlaneNeeded) {
                loadTrips(data.tickets, data.employeeName, data.employeeID);
            } else {
                $('#PlaneTicketList').hide();
            }

            if (data.isCarRentalNeeded) {
                if (data.rentals)
                    loadRentals(data.rentals);
            }
            else {
                $('#CarRentalList').hide();
            }

            if (data.isCarCompensationNeeded) {
                if (data.gasCompensations)
                    loadGasCompensations(data.gasCompensations);
            }
            else {
                $('#GasCompensationList').hide();
            }


            if (data.employeeName !== undefined) {
                for (i = 0; i < data.employeeName.length; i++) {
                    var td = $('<td class="hideColumns">');
                    if (data.accomodation !== null) {
                        if (data.accomodation.length > i) {
                            if (data.accomodationUrl[i] == null) {
                                data.accomodationUrl[i] = "javascript: void(0)";
                            }
                            var checkIn = moment(data.checkIn[i]).format('YYYY-MM-DD HH:mm');
                            var checkOut = moment(data.checkOut[i]).format('YYYY-MM-DD HH:mm');
                            const checkFrom = data.checkIn[i];
                            const checkTo = data.checkOut[i];
                            const idx = data.apartmentId[i];
                            const accomodation = data.accomodation[i];
                            const address = data.address[i];
                            const roomNumber = data.roomNumber[i];
                            const price = data.price[i];
                            const accomodationUrl = data.accomodationUrl[i];
                            const currency = data.currency[i];

                            if (data.apartmentType[i] == "HOTEL") {
                                td.append($('<a class="edit" title="Edit" data-toggle="modal" data-target="#HotelModal">').css("cursor", "pointer").click(function () {
                                    $("#radioDiv").hide();
                                    window.apartmentEdit = true;
                                    window.apartmentId = idx;
                                    $("#hotelFrom").val(checkFrom);
                                    $("#hotelTo").val(checkTo);
                                    $("#hotelName").val(accomodation);
                                    $("#hotelAddress").val(address);
                                    $("#hotelRoom").val(roomNumber);
                                    $("#hotelPrice").val(price);
                                    $("#hotelUrl").val(accomodationUrl);
                                    $("#hotelSelect :selected").val(currency);
                                }).append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));
                            }

                            if (data.apartmentType[i] == "HOME") {
                                td.append($('<a class="edit" title="Edit" data-toggle="modal" data-target="#HotelModal">').css("cursor", "pointer").click(function () {
                                    $("#radioDiv").hide();
                                    window.apartmentEdit = true;
                                    window.apartmentId = idx;
                                    $("#hotelFrom").val(checkFrom);
                                    $("#hotelTo").val(checkTo);
                                    $("#homeAddress").val(address);
                                    $("#hotelDiv").hide();
                                    $("#homeDiv").show();
                                }).append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));
                            }


                            td.append($('<a class="delete" title="Delete" data-toggle="tooltip">').click(function () {
                                if (confirm("Are you sure you want to delete this data?"))
                                    return $.ajax({
                                        type: "DELETE",
                                        url: '/api/apartment/' + idx,
                                        contentType: "application/json",
                                        xhrFields: {
                                            withCredentials: true
                                        },
                                        success: function () {
                                            setTimeout(function () {
                                                $("div#pageContent").load("../trip_details.html");
                                            }, 1000);
                                        },
                                        error: function () { alert('Internet error'); },
                                    })
                            }).css("cursor", "pointer").append('<i class="material-icons" style="color: #E34724;">&#xE872;</i>'));

                        }
                        else {
                            td.append(
                                $('<a>')
                                    .attr('onclick', `window.hotelEmployeeId = ${data.employeeID[i]}`)
                                    .attr('data-toggle', 'modal')
                                    .css("cursor", "pointer")
                                    .attr('data-target', '#HotelModal')
                                    .append("<i class='material-icons' style='color: #E34724;'>&#xE147;</i>")
                            ).click(function () {
                                window.apartmentEdit = false;
                                $("#hotelFrom").val("");
                                $("#hotelTo").val("");
                                $("#hotelName").val("");
                                $("#hotelAddress").val("");
                                $("#hotelRoom").val("");
                                $("#hotelPrice").val("");
                                $("#hotelUrl").val("");
                            })
                            data.accomodation[i] = " ";
                            console.log(data.accomodation[i]);
                            data.address[i] = " ";
                            data.roomNumber[i] = " ";
                            checkIn = " ";
                            checkOut = " ";
                            data.accomodationUrl[i] = "javascript: void(0)";
                            data.address[i] = " ";
                            data.price[i] = ' ';
                            data.currency[i] = ' ';
                        }
                    }
                    else {
                        data.accomodation = [" "];
                        data.address = [" "];
                        data.roomNumber = [" "];
                        checkIn = " ";
                        checkOut = " ";
                        data.accomodationUrl = ["javascript: void(0)"];
                        data.address = [" "];
                        data.price = ' ';
                        data.currency = ' ';
                    }
                    var a;
                    if (data.accomodationUrl[i] == "-") {
                        a = $('<a>').text('Link');
                    }
                    else {
                        a = $('<a>').attr("target", "_blank").attr('href', data.accomodationUrl[i]).text('Link');
                    }
                    $('#AccommodationTable').append(
                        $('<tr>')
                            .append($('<td>').text(data.employeeName[i]))
                            .append($('<td>').text(data.accomodation[i]))
                            .append($('<td>').text(data.address[i]))
                            .append($('<td>').text(data.apartmentType[i] == "HOME" ? "" : data.roomNumber[i]))
                            .append($('<td>').text(checkIn))
                            .append($('<td>').text(checkOut))
                            .append($('<td>').text(data.apartmentType[i] == "HOME" ? "" : `${data.price[i]} ${data.currency[i]}`))
                            .append($('<td>').append(a))
                            .append(td)
                    );
                }
            }
            if ("COMPLETED" == data.status) {
                $("#editBtn").hide();
                $(".hideColumns").hide();
                $(".hideButtons").hide();
            }
        },
        type: 'GET'
    });


});

function openFinalRegistration() {
    window.finalRegistrationTripId = ID;
    $("div#pageContent").load("../FinalRegistrationForm.html");
}

function loadTrips(tickets, employees, employeeIds) {
    const arr = [...employees];
    const ids = [...employeeIds];

    tickets.forEach(function (ticket) {
        const index = arr.indexOf(ticket.employeeName);
        if (index !== -1) {
            arr.splice(index, 1);
            ids.splice(index, 1);
        }

        var td = $('<td class="hideColumns">').append($('<a class="edit" title="Edit" data-toggle="modal" data-target="#AirplaneModal">').css("cursor", "pointer").click(function () {
            window.airplaneEdit = true;
            window.airplaneId = ticket.id;
            var address = ticket.airport.split('-');
            from = $("#airplaneFrom").val(ticket.forwardFlightDate);
            $("#airplaneTo").val(ticket.returnFlightDate);
            $("#airplaneCompany").val(ticket.flightCompany);
            $("#airplaneAddressFrom").val(address[0]);
            $("#airplaneAddressTo").val(address[1]);
            $("#airplanePrice").val(ticket.price);
            $("#airplaneUrl").val(ticket.planeTicketUrl);
            $("#airplaneSelect :selected").val(ticket.currency);
        }).append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));

        td.append($('<a class="delete" title="Delete" data-toggle="tooltip">').click(function () {
            if (confirm("Are you sure you want to delete this data?"))
                return $.ajax({
                    type: "DELETE",
                    url: '/api/airplane/' + ticket.id,
                    contentType: "application/json",
                    xhrFields: {
                        withCredentials: true
                    },
                    success: function () {
                        setTimeout(function () {
                            $("div#pageContent").load("../trip_details.html");
                        }, 1000);
                    },
                    error: function () { alert('Internet error'); },
                })
        }).css("cursor", "pointer").append('<i class="material-icons" style="color: #E34724;">&#xE872;</i>'));

        const row = $('<tr>').append($('<td>').text(ticket.employeeName))
            .append($('<td>').text(ticket.flightCompany))
            .append($('<td>').text(ticket.airport))
            .append($('<td>').text(moment(ticket.forwardFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(moment(ticket.returnFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(`${ticket.price} ${ticket.currency}`))
            .append($('<td>').append($('<a>').attr("target", "_blank").attr('href', ticket.planeTicketUrl).text('Link')))
            .append(td);
        $('#FlightsTable').append(row);
    });

    arr.forEach(function (emp, index) {
        const row = $('<tr>')
            .append($('<td>').text(emp))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td class="hideColumns">').append(
                $('<a>')
                    .attr('onclick', `window.planeEmployeeId = ${ids[index]}`)
                    .attr('data-toggle', 'modal')
                    .css("cursor", "pointer")
                    .attr('data-target', '#AirplaneModal')
                    .append("<i class='material-icons' style='color: #E34724;'>&#xE147;</i>")
                    .click(function () {
                        window.airplaneEdit = false;
                        $("#airplaneFrom").val("");
                        $("#airplaneTo").val("");
                        $("#airplaneCompany").val("");
                        $("#airplaneAddressFrom").val("");
                        $("#airplaneAddressTo").val("");
                        $("#airplanePrice").val("");
                        $("#airplaneUrl").val("");
                    })
            ));
        $('#FlightsTable').append(row);
    });
}

function loadRentals(rentals) {
    rentals.forEach(function (rental) {
        var td = $('<td class="hideColumns">').append($('<a class="edit" title="Edit" data-toggle="modal" data-target="#CarRentalModal">').css("cursor", "pointer").click(function () {
            window.carEdit = true;
            window.carId = rental.id;
            $("#carFrom").val(rental.carIssueDate);
            $("#carTo").val(rental.carReturnDate);
            $("#carCompany").val(rental.carRentalCompany);
            $("#carAddress").val(rental.carPickupAddress);
            $("#carPrice").val(rental.price);
            $("#carUrl").val(rental.carRentalUrl);
            $("#carSelect :selected").val(rental.currency);
        }).append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));

        td.append($('<a class="delete" title="Delete" data-toggle="tooltip">').click(function () {
            if (confirm("Are you sure you want to delete this data?"))
                return $.ajax({
                    type: "DELETE",
                    url: '/api/car/' + rental.id,
                    contentType: "application/json",
                    xhrFields: {
                        withCredentials: true
                    },
                    success: function () {
                        row.remove();
                    },
                    error: function () { alert('Internet error'); },
                })
        }).css("cursor", "pointer").append('<i class="material-icons" style="color: #E34724;">&#xE872;</i>'));

        const row = $('<tr>')
            .append($('<td>').text(rental.carRentalCompany))
            .append($('<td>').text(rental.carPickupAddress))
            .append($('<td>').text(moment(rental.carIssueDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(moment(rental.carReturnDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(`${rental.price} ${rental.currency}`))
            .append($('<td>').append($('<a>').attr("target", "_blank").attr('href', rental.carRentalUrl).text('Link')))
            .append(td)
        $('#CarRentalTable').append(row);
    });
}

function loadGasCompensations(compensations) {
    compensations.forEach(function (c) {
        var td = $('<td class="hideColumns">').append($('<a class="edit" title="Edit" data-toggle="modal" data-target="#GasCompensationModal">').click(function () {
            window.gasEdit = true;
            window.gasId = c.id;
            $('#gasPrice').val(c.price);
            $('#employeeSelect').val($(`#employeeSelect option:contains('${c.name}')`).val())
            $('#gasSelect').val(c.currency);
        }).css("cursor", "pointer").append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));

        td.append($('<a class="delete" title="Delete" data-toggle="tooltip">').click(function () {
            if (confirm("Are you sure you want to delete this data?"))
                return $.ajax({
                    type: "DELETE",
                    url: '/api/gas/' + c.id,
                    contentType: "application/json",
                    xhrFields: {
                        withCredentials: true
                    },
                    success: function () {
                        row.remove();
                    },
                    error: function () { alert('Internet error'); },
                })
        }).css("cursor", "pointer").append('<i class="material-icons" style="color: #E34724;">&#xE872;</i>'));
        const row = $('<tr>')
            .append($('<td>').text(c.name))
            .append($('<td>').text(`${c.price} ${c.currency}`))
            .append(td);
        $('#GasCompensationTable').append(row);
    });
}