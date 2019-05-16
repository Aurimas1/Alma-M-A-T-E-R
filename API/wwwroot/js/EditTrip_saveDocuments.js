var idToNameMap = {};
var tripID = location.hash.substr(1);

function changeToHotel() {
    $("#hotelDiv").show();
    $("#homeDiv").hide();
}

function changeToHome() {
    $("#hotelDiv").hide();
    $("#homeDiv").show();
}

function saveGasCompensation() {
    var employeeID = $("#employeeSelect :selected").val();
    var tel = $("#gasTel").val();
    var price = $("#gasPrice").val();
    var select = $("#gasSelect :selected").val();
    if (tel == "" || price == "") {
        alert("You didn't write all information.");
    }
    else if (price < 0) {
        alert("You didn't write price correctly.");
    }

    else {
        $('#GasCompensationModal').modal('toggle');
        return $.ajax({
            type: "POST",
            url: '/api/trip/gasCompensation',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": tripID,
                "Price": price,
                "EmployeeID": employeeID,
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
    }
}

function saveCarRental() {
    var from = $("#carFrom").val();
    var to = $("#carTo").val();
    var company = $("#carCompany").val();
    var address = $("#carAddress").val();
    var model = $("#carModel").val();
    var price = $("#carPrice").val();
    var url = $("#carUrl").val();
    var select = $("#carSelect :selected").val();
    if (from == "" || price == "" || to == "" || company == "" || address == "" || url == "") {
        alert("You didn't write all information.");
        $('#carCompany').css('border-color', 'red');
    }
    else if (price < 0) {
        alert("You didn't write price correctly.");
    }
    else {
        $('#CarRentalModal').modal('toggle');
        return $.ajax({
            type: "POST",
            url: '/api/trip/carRental',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": tripID,
                "Price": price,
                "CarRentalCompany": company,
                "CarPickupAddress": address,
                "CarRentalUrl": url,
                "CarIssueDate": new Date(from),
                "CarReturnDate": new Date(to),
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
    }
}

function saveAirplane() {
    var from = $("#airplaneFrom").val();
    var to = $("#airplaneTo").val();
    var company = $("#airplaneCompany").val();
    var addressFrom = $("#airplaneAddressFrom").val();
    var addressTo = $("#airplaneAddressTo").val();
    var price = $("#airplanePrice").val();
    var url = $("#airplaneUrl").val();
    var select = $("#airplaneSelect :selected").val();
    var employeeID = 1;//sita reik pakeisti veliau!!!!
    if (from == "" || price == "" || to == "" || company == "" || addressFrom == "" || url == "" || addressTo == "") {
        alert("You didn't write all information.");
    }
    else if (price < 0) {
        alert("You didn't write price correctly.");
    }
    else {
        $('#AirplaneModal').modal('toggle');
        return $.ajax({
            type: "POST",
            url: '/api/trip/planeTicket',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": tripID,
                "Price": price,
                "PlaneTicketUrl": url,
                "ForwardFlightDate": new Date(from),
                "ReturnFlightDate": new Date(to),
                "Airport": addressFrom + "-" + addressTo,
                "FlightCompany": company,
                "EmployeeID": employeeID,
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
    }
}

function saveHotel() {
    var from = $("#hotelFrom").val();
    var to = $("#hotelTo").val();
    var employeeID = 1;//sita reik pakeisti veliau!!!!
    var company;
    var address;
    var room;
    var price;
    var url;
    var select;
    
    if ($("#radioDiv :radio[name=types]:checked").val() == "HOTEL") {
        company = $("#hotelName").val();
        address = $("#hotelAddress").val();
        room = $("#hotelRoom").val();
        price = $("#hotelPrice").val();
        url = $("#hotelUrl").val();
        select = $("#hotelSelect :selected").val();

    }
    else {
        address = $("#homeAddress").val();

    }
    if (from == "" || price == "" || to == "" || company == "" || addressFrom == "" || url == "" || addressTo == "") {
        alert("You didn't write all information.");
    }
    else if (price < 0) {
        alert("You didn't write price correctly.");
    }
    else {
        $('#AirplaneModal').modal('toggle');
        return $.ajax({
            type: "POST",
            url: '/api/trip/hotel',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": tripID,
                "Price": price,
                "EmployeeID": employeeID,
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
    }
}