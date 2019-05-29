var ID = window.tripDetailsTripId;
$(document).ready(function () {
    let countryToCurrencyMap = {
        "USA": "USD",
        "Canada": "CAD",
        "Lithuania": "EUR",
        "United Kingdom": "GBP",
    };
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
            //check if the trip can be merged
            getIfTheTripCanBeMerged(ID);
            
            window.tripCurrency = countryToCurrencyMap[data.departureCountry];

            var departure = moment(data.departureDate).format('YYYY-MM-DD HH:mm');
            var arrival = moment(data.returnDate).format('YYYY-MM-DD HH:mm');
            $("#Arrival").text(data.arrivalCity + ', ' + data.arrivalCountry);
            $("#Departure").text(data.departureCity + ', ' + data.departureCountry);
            $("#DepartureDate").text(departure);
            $("#ArrivalDate").text(arrival);
            $("#Status").text(data.status);

            for (let e of data.employees) {
                if (e.employeeStatus !== "PENDING") {
                    $('#employeeSelect').append($(`<option value="${e.employeeID}" selected>`).text(e.employeeName));
                }
            }

            if (data.employees !== undefined) {
                for (var i = 0; i < data.employees.length; i++) {
                    var fullName = data.employees[i].employeeName.split(" ");

                    var cl = "green";
                    if (data.employees[i].employeeStatus === "PENDING") cl = "red";
                    $('#EmployeesTable').append(
                        $('<tr>')
                            .append($('<td>').text(fullName[0]))
                            .append($('<td>').text(fullName[1]))
                            .append($('<td>').text(data.employees[i].employeeEmail))
                            .append($('<td>').text(data.employees[i].employeeStatus).css({
                                "color": cl,
                                "font-weight": 600
                            }))
                    );
                }
            }

            if (data.isPlaneNeeded) {
                loadTrips(data.tickets, data.employees);
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


            if (data.employees !== undefined) {
                for (let res of data.reservations) {
                    var td = $('<td class="hideColumns">');
                    if (res.reservationUrl == null) {
                        res.reservationUrl = "javascript: void(0)";
                    }
                    var checkIn = moment(res.checkIn).format('YYYY-MM-DD HH:mm');
                    var checkOut = moment(res.checkOut).format('YYYY-MM-DD HH:mm');
                    let checkFrom = res.checkIn;
                    let checkTo = res.checkOut;
                    let idx = res.apartmentID;
                    let reservationId = res.reservationID;
                    let accomodation = res.name;
                    let address = res.address;
                    let roomNumber = res.roomNumber;
                    let price = res.price;
                    let accomodationUrl = res.reservationUrl;
                    let currency = res.currency;

                    if (res.type == "HOTEL") {
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
                            $("#hotelSelect").val(currency);
                        }).append('<i class="material-icons" style="color: #FFC107;">&#xE254;</i>'));
                    }

                    if (res.type == "HOME") {
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
                                url: '/api/reservation/' + reservationId,
                                contentType: "application/json",
                                xhrFields: {
                                    withCredentials: true
                                },
                                success: function () {
                                    alert("The item was succesfully deleted");
                                    setTimeout(function () {
                                        $("div#pageContent").load("../trip_details.html");
                                    }, 1000);
                                },
                                error: function () { alert('Internet error'); },
                            })
                    }).css("cursor", "pointer").append('<i class="material-icons" style="color: #E34724;">&#xE872;</i>'));

                    if (res.reservationUrl == "-") {
                        a = $('<a>').text('Link');
                    }
                    else {
                        a = $('<a>').attr("target", "_blank").attr('href', res.reservationUrl).text('Link');
                    }

                    if (data.employees.find(x => x.employeeName === res.employeeName).employeeStatus !== "PENDING")
                        $('#AccommodationTable').append(
                            $('<tr>')
                                .append($('<td>').text(res.employeeName))
                                .append($('<td>').text(res.name))
                                .append($('<td>').text(res.address))
                                .append($('<td>').text(res.type == "HOME" ? "" : res.roomNumber))
                                .append($('<td>').text(checkIn))
                                .append($('<td>').text(checkOut))
                                .append($('<td>').text(res.type == "HOME" ? "" : `${res.price} ${res.currency}`))
                                .append($('<td>').append(a))
                                .append(td)
                        );
                }

                let empsWithReservation = data.reservations.map(x => x.employeeName);
                let empsWithoutReservation = data.employees.filter(x => x.employeeStatus !== "PENDING").filter(y => empsWithReservation.indexOf(y.employeeName) === -1);
                for (let emp of empsWithoutReservation) {
                    var td = $('<td class="hideColumns">');
                    td.append($('<a>')
                        .attr('onclick', `window.hotelEmployeeId = ${emp.employeeID}`)
                        .attr('data-toggle', 'modal')
                        .css("cursor", "pointer")
                        .attr('data-target', '#HotelModal')
                        .append($("<i class='material-icons' style='color: #E34724;'>&#xE147;</i>").click(() => {
                            $('#hotelSelect').val(window.tripCurrency);
                        }))
                    ).click(function () {
                        window.apartmentEdit = false;
                        $("#hotelFrom").val("");
                        $("#hotelTo").val("");
                        $("#hotelName").val("");
                        $("#hotelAddress").val("");
                        $("#hotelRoom").val("");
                        $("#hotelPrice").val("");
                        $("#hotelUrl").val("");
                    });
                    $('#AccommodationTable').append(
                        $('<tr>')
                            .append($('<td>').text(emp.employeeName))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append($('<td>'))
                            .append(td)
                    );
                }
            }
            if ("FINISHED" == data.status) {
                $("#editBtn").hide();
                $(".hideColumns").hide();
                $(".hideButtons").hide();
            }
            allowEdit();
        },
        type: 'GET'
    });
});

function openFinalRegistration() {
    window.finalRegistrationTripId = ID;
    $("div#pageContent").load("../FinalRegistrationForm.html");
}

function loadTrips(tickets, employees) {
    let arr = employees.filter(function (el) {
        return el.employeeStatus !== "PENDING";
    });

    tickets.forEach(function (ticket) {
        let index = arr.map(x => x.employeeName).indexOf(ticket.employeeName);
        if (index !== -1) {
            arr.splice(index, 1);
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

        let row = $('<tr>').append($('<td>').text(ticket.employeeName))
            .append($('<td>').text(ticket.flightCompany))
            .append($('<td>').text(ticket.airport))
            .append($('<td>').text(moment(ticket.forwardFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(moment(ticket.returnFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(`${ticket.price} ${ticket.currency}`))
            .append($('<td>').append($('<a>').attr("target", "_blank").attr('href', ticket.planeTicketUrl).text('Link')))
            .append(td);
        $('#FlightsTable').append(row);
    });

    arr.forEach(function (emp) {
        let row = $('<tr>')
            .append($('<td>').text(emp.employeeName))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td>'))
            .append($('<td class="hideColumns">').append(
                $('<a>')
                    .attr('onclick', `window.planeEmployeeId = ${emp.employeeID}`)
                    .attr('data-toggle', 'modal')
                    .css("cursor", "pointer")
                    .attr('data-target', '#AirplaneModal')
                    .append("<i class='material-icons' style='color: #E34724;'>&#xE147;</i>").click(() => {
                        $('#airplaneSelect').val(window.tripCurrency);
                    })
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

        let row = $('<tr>')
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
        let row = $('<tr>')
            .append($('<td>').text(c.name))
            .append($('<td>').text(`${c.price} ${c.currency}`))
            .append(td);
        $('#GasCompensationTable').append(row);
    });
}

function deleteTrip() {
    if (confirm("Are you sure you want to delete this trip?")) {
        return $.ajax({
            type: "DELETE",
            url: '/api/trip/' + ID,
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                alert("The trip was sucesfully deleted");
                $("div#pageContent").load("../trips.html");
            },
            error: function () { alert('Internet error'); },
        })
    }
}


$('#TripMergeModal').on('show.bs.modal', function (event) {
    getTripsForMerging(ID);
})

function allowEdit() {
    $.ajax({
        type: "GET",
        url: "/api/trip/allowEdit/" + ID,
        success: function(data, status){
            if (!data) {
                $('#editBtn').hide();
                $('#deleteBtn').hide();
                $('.material-icons').hide();
                $('.hideColumns').hide();
            }
        },
        error: function () { alert('Internet error'); },
    })
}
