var employeeToTrip;
var needRoom = true;
$(document).ready(function () {
    var queryString = decodeURIComponent(window.location.search);
    queryString = queryString.substring(1);
    queryString = queryString.split("=");
    var ID = Number(queryString[1]);

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
            employeeToTrip = data.employeeToTrip;
            accomodation = data.accomodation;
            $("#statusPill").text(data.employeeStatus);
            if(data.employeeStatus == "APPROVED") {
                $("#statusPill").css("background-color", "#23a94c");
                $("#confirmBtn").hide();
                $("#refuseApartmentBtn").hide();
            }
            if (!data.accomodation == "HOME") {
                $("#refuseApartmentBtn").hide();
                $("#AccomodationList").hide();
            }

            var departure = moment(data.departureDate).format('YYYY-MM-DD kk:mm');
            var arrival = moment(data.returnDate).format('YYYY-MM-DD HH:mm');
            $("#Arrival").text(data.arrivalCity + ', ' + data.arrivalCountry);
            $("#Departure").text(data.departureCity + ', ' + data.departureCountry);
            $("#DepartureDate").text(departure);
            $("#ArrivalDate").text(arrival);
            $("#Status").text(data.status);
            if(data.status == "APPROVED") {
                $("#Status").css("background-color", "#23a94c");
            }
            else if(data.status == "COMPLETED") {
                $("#Status").css("background-color", "#3f3f3f");
            }


            if (data.isPlaneNeeded) {
                if (data.tickets[0] !== undefined) {
                        if (data.tickets[0].flightCompany !== null) {
                                if (data.ticketFile == null) {
                                    data.tickets[0].planeTicketUrl = "javascript: void(0)";
                                }
                                var forwardFlight = moment(data.tickets[0].forwardFlightDate).format('YYYY-MM-DD kk:mm');
                                var returnFlight = moment(data.tickets[0].returnFlightDate).format('YYYY-MM-DD kk:mm');
                            }
                            else {
                                data.tickets[0].flightCompany = " ";
                                data.tickets[0].airport = " ";
                                forwardFlight = " ";
                                returnFlight = " ";
                                data.tickets[0].planeTicketUrl = "javascript: void(0)";
                            }

                        var displayPlaneTicketInfo = '<div class="row"> <div class="col"> <p>' + data.employeeName + '</p></div> <div class="col"> ' +
                            '<p>' + data.tickets[0].flightCompany + '</p></div><div class="col"><p>' + data.tickets[0].airport + '</p></div><div class="col"><p>' + forwardFlight + '</p></div> ' +
                            '<div class="col"><p>' + returnFlight + '</p></div><div class="col"> <a href="' + data.tickets[0].planeTicketUrl + '" class="badge badge-info">Link</a></div>' +
                            '</div>';
                        var addPlaneInfo = document.getElementById("Flights");
                        addPlaneInfo.insertAdjacentHTML('afterend', displayPlaneTicketInfo);
                }
                else {
                    $('#PlaneTicketBtn').prop('disabled', true);
                    $('#Flights').hide();
                }
            }
            else {
                $('#PlaneTicketList').hide();
            }


            if (data.accomodation[0] !== undefined && data.accomodation[0] !== "HOME") {
                    if (data.accomodation[0] !== null){
                            if (data.accomodationUrl[0] == null) {
                                data.accomodationUrl = "javascript: void(0)";
                            }
                            var checkIn = moment(data.checkIn[0]).format('YYYY-MM-DD kk:mm');
                            var checkOut = moment(data.checkOut[0]).format('YYYY-MM-DD kk:mm');
                        }
                        else {
                            data.accomodation = " ";
                            data.address = " ";
                            data.roomNumber = " ";
                            checkIn = " ";
                            checkOut = " ";
                            data.address = " ";
                            data.accomodationUrl = "javascript: void(0)";
                        }

                    var displayAccomodationInfo = '<div class="row"><div class="col"><p>' + data.employeeName + '</p></div><div class="col"><p>' + data.accomodation +
                        '</p></div><div class="col"><p>' + data.address + '</p></div><div class="col"><p>' + data.roomNumber + '</p></div><div class="col"><p>' + checkIn + '</p></div><div class="col">' +
                        '<p>' + checkOut + '</p></div><div class="col"><a href="' + data.accomodationUrl + '" class="badge badge-info">Link</a></div>' +
                        '</div>';

                    var addAccomodationInfo = document.getElementById("Accomodation");
                    addAccomodationInfo.insertAdjacentHTML('afterend', displayAccomodationInfo);
            }
            else {
                $('#AccomodationBtn').prop('disabled', true);
                $('#Accomodation').hide();
            }

            if (data.isCarRentalNeeded) {
                if (data.rentals[0] !== undefined) {
                    for (i = 0; i < data.rentals.length; i++) {
                        var carIssueDate = moment(data.rentals[i].carIssueDate).format('YYYY-MM-DD kk:mm');
                        var carReturnDate = moment(data.rentals[i].carReturnDate).format('YYYY-MM-DD kk:mm');
                        if (data.rentals[i].carRentalUrl == null) {
                            data.rentals[i].carRentalUrl = "javascript: void(0)";
                        }

                        var displayCarRentalInfo = '<div class="row"><div class="col"><p>' + data.rentals[i].carRentalCompany + '</p></div><div class="col"><p>' + data.rentals[i].carPickupAddress + '</p>' +
                            '</div><div class="col"><p>' + carIssueDate + '</p></div><div class="col"><p>' + carReturnDate + '</p></div><div class="col">' +
                            '<a href="' + data.rentals[i].carRentalUrl + '" class="badge badge-info">Link</a></div></div>';

                        var addCarRentalInfo = document.getElementById("CarRentals");
                        addCarRentalInfo.insertAdjacentHTML('afterend', displayCarRentalInfo);
                    }
                }
                else {
                    $('#CarRentalBtn').prop('disabled', true);
                    $('#CarRentals').hide();
                }
            }
            else {
                $('#CarRentalList').hide();
            }

            if (data.isCarCompensationNeeded) {
                if (data.gasCompensation[0] !== undefined) {

                            var displayGasCompensationInfo = '<div class="row"><div class="col"><p>' + data.gasCompensation + '</p></div>' +
                                '<div class="col"><p>' + data.amount + ' ' + data.currency + '</p></div></div>';

                            var addGasCompensationInfo = document.getElementById("GasCompensations");
                            addGasCompensationInfo.insertAdjacentHTML('afterend', displayGasCompensationInfo);
                }
                else {
                    $('#GasCompensationBtn').prop('disabled', true);
                    $('#GasCompensations').hide();
                }
            }
            else {
                $('#GasCompensationList').hide();
            }
            if (!data.employeeRead[0]) {
                markRead();
            }
        },
    });

});

function clickApprove() {
    var result = confirm("Are you sure you want to approve trip?");
    if (result) {
        $.ajax({
            type: "POST",
            url: '/api/trip/approve/' + employeeToTrip + '/' + needRoom,
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
        })
    }
}

function clickRefuse() {
    var result = confirm("Are you sure you don't need accomodation?");
    if (result) {
        needRoom = false;
        $("#AccomodationList").hide();
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
    })
}