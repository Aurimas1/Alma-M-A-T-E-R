var employeeToTrip;
var ID = window.userTripDetailsTripId;
$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: 'api/trip/user/' + ID,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function () {
            console.error('Error');
        },
        success: function (data) {
            
            accomodation = data.accomodation;

            for (let emp of data.employee) {
                employeeToTrip = emp.id;
                $("#statusPill").text(emp.employeeStatus);
                if (emp.employeeStatus == "APPROVED") {
                    $("#statusPill").css("background-color", "#23a94c");
                    $("#confirmBtn").hide();
                    $("#refuseApartmentBtn").hide();
                }
            }

            var departure = moment(data.departureDate).format('YYYY-MM-DD kk:mm');
            var arrival = moment(data.returnDate).format('YYYY-MM-DD HH:mm');
            $("#Arrival").text(data.arrivalCity + ', ' + data.arrivalCountry);
            $("#Departure").text(data.departureCity + ', ' + data.departureCountry);
            $("#DepartureDate").text(departure);
            $("#ArrivalDate").text(arrival);
            $("#Status").text(data.status);

            if (data.status == "CREATED") {
                $("#Status").css("background-color", "#eef442");
            }
            else if (data.status == "CONFIRMED") {
                $("#Status").css("background-color", "#23a94c");
            }
            else if (data.status == "COMPLETED") {
                $("#Status").css("background-color", "#3f3f3f");
                $("#homeAppartment").hide();
            }


            if (data.isPlaneNeeded) {
                loadTrips(data.tickets);
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

            if (data.employee !== undefined) {
                for (let res of data.reservations) {
                    if (res.name === "HOME") {
                        $("#homeAppartment").hide();
                    }

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

                    if (res.reservationUrl == "-") {
                        a = $('<a>').text('Link');
                    }
                    else {
                        a = $('<a>').attr("target", "_blank").attr('href', res.reservationUrl).text('Link');
                    }

                    if (data.employee.find(x => x.employeeName === res.employeeName).employeeStatus !== "PENDING")
                        $('#AccommodationTable').append(
                            $('<tr>')
                                .append($('<td>').text(res.name))
                                .append($('<td>').text(res.address))
                                .append($('<td>').text(res.type == "HOME" ? "" : res.roomNumber))
                                .append($('<td>').text(checkIn))
                                .append($('<td>').text(checkOut))
                                .append($('<td>').text(res.type == "HOME" ? "" : `${res.price} ${res.currency}`))
                                .append($('<td>').append(a))
                        );
                }
            }
        }
    });
});

function loadTrips(tickets) {

    tickets.forEach(function (ticket) {

        let row = $('<tr>').append($('<td>').text(ticket.flightCompany))
            .append($('<td>').text(ticket.airport))
            .append($('<td>').text(moment(ticket.forwardFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(moment(ticket.returnFlightDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(`${ticket.price} ${ticket.currency}`))
            .append($('<td>').append($('<a>').attr("target", "_blank").attr('href', ticket.planeTicketUrl).text('Link')));
        $('#FlightsTable').append(row);
    });
}

function loadRentals(rentals) {
    rentals.forEach(function (rental) {

        let row = $('<tr>')
            .append($('<td>').text(rental.carRentalCompany))
            .append($('<td>').text(rental.carPickupAddress))
            .append($('<td>').text(moment(rental.carIssueDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(moment(rental.carReturnDate).format('YYYY-MM-DD HH:mm')))
            .append($('<td>').text(`${rental.price} ${rental.currency}`))
            .append($('<td>').append($('<a>').attr("target", "_blank").attr('href', rental.carRentalUrl).text('Link')));
        $('#CarRentalTable').append(row);
    });
}

function loadGasCompensations(compensations) {
    compensations.forEach(function (c) {
        let row = $('<tr>')
            .append($('<td>').text(c.name))
            .append($('<td>').text(`${c.price} ${c.currency}`));
        $('#GasCompensationTable').append(row);
    });
}

function clickApprove() {
    var result = confirm("Are you sure you want to approve trip?");
    if (result) {
        $.ajax({
            type: "POST",
            url: '/api/trip/approve/' + employeeToTrip,
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
}

function clickRefuse() {
    var result = confirm("Are you sure you don't need accomodation?");
    if (result) {
        $.ajax({
            type: "POST",
            url: '/api/reservation/setHome/' + employeeToTrip,
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                location.reload();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
}

function markRead() {
    $.ajax({
        type: "PATCH",
        url: '/api/trip/read/' + employeeToTrip,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}