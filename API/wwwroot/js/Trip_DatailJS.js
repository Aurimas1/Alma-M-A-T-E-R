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
                            .append($('<th>').text(Nr))
                            .append($('<th>').text(fullName[0]))
                            .append($('<th>').text(fullName[1]))
                            .append($('<th>').text(data.employeeEmail[i]))
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
                if(data.gasCompensations)
                    loadGasCompensations(data.gasCompensations);
            }
            else {
                $('#GasCompensationList').hide();
            }


            if (data.employeeName !== undefined) {
                for (i = 0; i < data.employeeName.length; i++) {

                    if (data.accomodation !== null) {
                        if (data.accomodation.length > i) {
                            if (data.accomodationUrl[i] == null) {
                                data.accomodationUrl[i] = "javascript: void(0)";
                            }
                            var checkIn = moment(data.checkIn[i]).format('YYYY-MM-DD HH:mm');
                            var checkOut = moment(data.checkOut[i]).format('YYYY-MM-DD HH:mm');
                        }
                        else {
                            
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
                    if(data.accomodationUrl[i] == "-"){
                        a = $('<a>').text('Link');
                    }
                    else{
                        a = $('<a>').attr("target","_blank").attr('href', data.accomodationUrl[i]).text('Link');
                    }
                    $('#AccommodationTable').append(
                        $('<tr>')
                            .append($('<th>').text(data.employeeName[i]))
                            .append($('<th>').text(data.accomodation[i]))
                            .append($('<th>').text(data.address[i]))
                            .append($('<th>').text(data.roomNumber[i]))
                            .append($('<th>').text(checkIn))
                            .append($('<th>').text(checkOut))
                            .append($('<th>').text(`${data.price[i]} ${data.currency[i]}`))
                            .append($('<th>').append(a))
                            .append($('<th>').append(
                                $('<button>')
                                    .addClass('btn btn-primary')
                                    .text('Add hotel')
                                    .attr('onclick', `window.hotelEmployeeId = ${data.employeeID[i]}`)
                                    .attr('data-toggle', 'modal')
                                    .attr('type', 'button')
                                    .attr('data-target', '#HotelModal')
                            ))
                    );
                }
            }
        },
        type: 'GET'
    });

});

function openFinalRegistration(){
    window.finalRegistrationTripId = ID;
    $("div#pageContent").load("../FinalRegistrationForm.html"); 
}

function loadTrips(tickets, employees, employeeIds) {
    const arr = [...employees];
    const ids = [...employeeIds];
    tickets.forEach(function(ticket) {
        const index = arr.indexOf(ticket.employeeName);
        if (index !== -1) {
            arr.splice(index, 1);
            arr.splice(ids, 1);
        }

        $('#FlightsTable').append(
            $('<tr>')
                .append($('<th>').text(ticket.employeeName))
                .append($('<th>').text(ticket.flightCompany))
                .append($('<th>').text(ticket.airport))
                .append($('<th>').text(moment(ticket.forwardFlightDate).format('YYYY-MM-DD HH:mm')))
                .append($('<th>').text(moment(ticket.returnFlightDate).format('YYYY-MM-DD HH:mm')))
                .append($('<th>').text(`${ticket.price} ${ticket.currency}`))
                .append($('<th>').append($('<a>').attr("target","_blank").attr('href', ticket.planeTicketUrl).text('Link')))
                .append($('<th>').append(
                    $('<button>')
                        .addClass('btn btn-primary')
                        .text('Add ticket')
                        .attr('onclick', `window.planeEmployeeId = ${ticket.employeeID}`)
                        .attr('data-toggle', 'modal')
                        .attr('type', 'button')
                        .attr('data-target', '#AirplaneModal')
                ))
        );
    });

    arr.forEach(function(emp, index) {
        $('#FlightsTable').append(
            $('<tr>')
                .append($('<th>').text(emp))
                .append($('<th>'))
                .append($('<th>'))
                .append($('<th>'))
                .append($('<th>'))
                .append($('<th>'))
                .append($('<th>'))
                .append($('<th>').append(
                    $('<button>')
                        .addClass('btn btn-primary')
                        .text('Add ticket')
                        .attr('onclick', `window.planeEmployeeId = ${ids[index]}`)
                        .attr('data-toggle', 'modal')
                        .attr('type', 'button')
                        .attr('data-target', '#AirplaneModal')
                ))
        );
    });
}

function loadRentals(rentals) {
    rentals.forEach(function(rental) {
        $('#CarRentalTable').append(
            $('<tr>')
                .append($('<th>').text(rental.carRentalCompany))
                .append($('<th>').text(rental.carPickupAddress))
                .append($('<th>').text(moment(rental.carIssueDate).format('YYYY-MM-DD HH:mm')))
                .append($('<th>').text(moment(rental.carReturnDate).format('YYYY-MM-DD HH:mm')))
                .append($('<th>').text(`${rental.price} ${rental.currency}`))
                .append($('<th>').append($('<a>').attr("target","_blank").attr('href', rental.carRentalUrl).text('Link')))
        );
    });
}

function loadGasCompensations(compensations) {
    compensations.forEach(function(c) {
        $('#GasCompensationTable').append(
            $('<tr>')
                .append($('<th>').text(c.name))
                .append($('<th>').text(`${c.price} ${c.currency}`))
        );
    });
}