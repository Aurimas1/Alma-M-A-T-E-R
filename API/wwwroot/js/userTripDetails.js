$(document).ready(function () {
    var queryString = decodeURIComponent(window.location.search);
    queryString = queryString.substring(1);
    queryString = queryString.split("=");
    var ID = Number(queryString[1]);
    var userEmail = "tomaskiziela@gmail.com"; //need to fetch current user email here
    var userName; //needed to filter gas compensation

    $.ajax({
        type: "GET",
        url: 'api/trip/' + ID,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        error: function () {
            console.error('Error');
        },
        success: function (data) {
            var departure = moment(data.departureDate).format('YYYY-MM-DD kk:mm');
            var arrival = moment(data.returnDate).format('YYYY-MM-DD HH:mm');
            $("#Arrival").text(data.arrivalCity + ', ' + data.arrivalCountry);
            $("#Departure").text(data.departureCity + ', ' + data.departureCountry);
            $("#DepartureDate").text(departure);
            $("#ArrivalDate").text(arrival);
            $("#Status").text(data.status);

            for(i = 0; i < data.employeeEmail.length; i++)  //find out user index by matching email
            {
                if(data.employeeEmail[i] == userEmail)
                {
                    userName = data.employeeName[i]; //hacky
                    break;
                }
            }

            if (data.isPlaneNeeded) {
                if (data.employeeName !== null) {
                        if (data.flightCompany !== null) {
                            if (data.flightCompany.length > i) {
                                if (data.ticketFile[i] == null) {
                                    data.ticketFile[i] = "javascript: void(0)";
                                }
                                var forwardFlight = moment(data.forwardFlight[i]).format('YYYY-MM-DD kk:mm');
                                var returnFlight = moment(data.returnFlight[i]).format('YYYY-MM-DD kk:mm');
                            }
                            else {
                                data.flightCompany[i] = " ";
                                data.airport[i] = " ";
                                data.ticketFile[i] = " ";
                                forwardFlight = " ";
                                returnFlight = " ";
                                data.ticketFile[i] = "javascript: void(0)";
                            }
                        }
                        else {
                            data.flightCompany = [" "];
                            data.airport = [" "];
                            data.ticketFile = [" "];
                            forwardFlight = " ";
                            returnFlight = " ";
                            data.ticketFile = ["javascript: void(0)"];
                        }

                        var displayPlaneTicketInfo = '<div class="row"> <div class="col"> <p>' + data.employeeName[i] + '</p></div> <div class="col"> ' +
                            '<p>' + data.flightCompany[i] + '</p></div><div class="col"><p>' + data.airport[i] + '</p></div><div class="col"><p>' + forwardFlight + '</p></div> ' +
                            '<div class="col"><p>' + returnFlight + '</p></div><div class="col"> <a href="' + data.ticketFile[i] + '" class="badge badge-info">Link</a></div>' +
                            '</div>';
                        var addPlaneInfo = document.getElementById("Flights");
                        addPlaneInfo.insertAdjacentHTML('afterend', displayPlaneTicketInfo);
                }
            }
            else {
                $('#PlaneTicketList').hide();
            }


            if (data.employeeName !== undefined) {
                    if (data.accomodation !== null) {
                        if (data.accomodation.length > i) {
                            if (data.accomodationUrl[i] == null) {
                                data.accomodationUrl[i] = "javascript: void(0)";
                            }
                            var checkIn = moment(data.checkIn[i]).format('YYYY-MM-DD kk:mm');
                            var checkOut = moment(data.checkOut[i]).format('YYYY-MM-DD kk:mm');
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
                    }

                    var displayAccomodationInfo = '<div class="row"><div class="col"><p>' + data.employeeName[i] + '</p></div><div class="col"><p>' + data.accomodation[i] +
                        '</p></div><div class="col"><p>' + data.address[i] + '</p></div><div class="col"><p>' + data.roomNumber[i] + '</p></div><div class="col"><p>' + checkIn + '</p></div><div class="col">' +
                        '<p>' + checkOut + '</p></div><div class="col"><a href="' + data.accomodationUrl[i] + '" class="badge badge-info">Link</a></div>' +
                        '</div>';

                    var addAccomodationInfo = document.getElementById("Accomodation");
                    addAccomodationInfo.insertAdjacentHTML('afterend', displayAccomodationInfo);
            }

            if (data.isCarRentalNeeded) {
                if (data.rentalCompany !== undefined) {
                    for (i = 0; i < data.rentalCompany.length; i++) {
                        var carIssueDate = moment(data.carIssueDate[i]).format('YYYY-MM-DD kk:mm');
                        var carReturnDate = moment(data.carReturnDate[i]).format('YYYY-MM-DD kk:mm');
                        if (data.carRentalUrl[i] == null) {
                            data.carRentalUrl[i] = "javascript: void(0)";
                        }

                        var displayCarRentalInfo = '<div class="row"><div class="col"><p>' + data.rentalCompany[i] + '</p></div><div class="col"><p>' + data.carPickupAddress[i] + '</p>' +
                            '</div><div class="col"><p>' + carIssueDate + '</p></div><div class="col"><p>' + carReturnDate + '</p></div><div class="col">' +
                            '<a href="' + data.carRentalUrl[i] + '" class="badge badge-info">Link</a></div></div>';

                        var addCarRentalInfo = document.getElementById("CarRentals");
                        addCarRentalInfo.insertAdjacentHTML('afterend', displayCarRentalInfo);
                    }
                }
            }
            else {
                $('#CarRentalList').hide();
            }

            if (data.isCarCompensationNeeded) {
                if (data.gasCompensation !== undefined) {
                    for (i = 0; i < data.gasCompensation.length; i++) {
                        if(data.gasCompensation[i] == userName){

                            var displayGasCompensationInfo = '<div class="row"><div class="col"><p>' + data.gasCompensation[i] + '</p></div>' +
                                '<div class="col"><p>' + data.amount[i] + '</p></div></div>';

                            var addGasCompensationInfo = document.getElementById("GasCompensations");
                            addGasCompensationInfo.insertAdjacentHTML('afterend', displayGasCompensationInfo);
                        }
                    }
                }
            }
            else {
                $('#GasCompensationList').hide();
            }
        },
    });

});