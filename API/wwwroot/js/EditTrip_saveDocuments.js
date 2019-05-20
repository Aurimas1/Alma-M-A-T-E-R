var tripID = window.tripDetailsTripId;

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
    var price = $("#gasPrice").val();
    var select = $("#gasSelect :selected").val();

    if (price == "") {
        $('#gasPrice').css('border-color', 'red');
        return;
    }
    else if (price < 0) {
        $('#gasPrice').css('border-color', 'red');
        alert("Price can't be less than zero");
        return;
    }
    else {
        $('#gasPrice').css('border-color', 'black');
    }

    $('#GasCompensationModal').modal('toggle');
    $.ajax({
        type: window.gasEdit ? "PUT" : "POST",
        url: window.gasEdit ? '/api/gas' : '/api/trip/gasCompensation',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            "TripID": window.gasEdit ? undefined : tripID,
            "GasCompensationID": window.gasEdit ? window.gasId : undefined,
            "Price": price,
            "EmployeeID": employeeID,
            "Currency": select,
        }),
        success: function () {
            alert("The information was saved!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    })
    setTimeout(function(){
        $("div#pageContent").load("../trip_details.html");
    }, 1000);  
}

function saveCarRental() {
    var from = $("#carFrom").val();
    var to = $("#carTo").val();
    var company = $("#carCompany").val();
    var address = $("#carAddress").val();
    var price = $("#carPrice").val();
    var url = $("#carUrl").val();
    var select = $("#carSelect :selected").val();

    var today = new Date();
    var fromDate = new Date(from);
    var toDate = new Date(to);

    if (from == "") {
        $('#carFrom').css('border-color', 'red');
        return;
    }
    else if (fromDate <= today) {
        $('#carFrom').css('border-color', 'red');
        alert("The date is incorrect");
        return;
    }
    else {
        $('#carFrom').css('border-color', 'black');
    }

    if (to == "") {
        $('#carTo').css('border-color', 'red');
        return;
    }
    else if (fromDate > toDate) {
        $('#carFrom').css('border-color', 'red');
        $('#carTo').css('border-color', 'red');
        alert("The dates is incorrect");
        return;
    }
    else {
        $('#carTo').css('border-color', 'black');
    }

    if (company == "") {
        $('#carCompany').css('border-color', 'red');
        return;
    }
    else {
        $('#carCompany').css('border-color', 'black');
    }

    if (address == "") {
        $('#carAddress').css('border-color', 'red');
        return;
    }
    else {
        $('#carAddress').css('border-color', 'black');
    }

    if (url == "") {
        $('#carUrl').css('border-color', 'red');
        return;
    }
    else {
        $('#carUrl').css('border-color', 'black');
    }

    if (price == "") {
        $('#carPrice').css('border-color', 'red');
        return;
    }
    else if (price < 0) {
        $('#carPrice').css('border-color', 'red');
        alert("Price can't be less than zero");
        return;
    }
    else {
        $('#carPrice').css('border-color', 'black');
    }

    $('#CarRentalModal').modal('toggle');
    $.ajax({
        type: window.carEdit ? "PUT" : "POST",
        url: window.carEdit ? '/api/car' : '/api/trip/carRental',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            "TripID": window.carEdit ? undefined : tripID,
            "CarRentalID": window.carEdit ? window.carId : undefined,
            "Price": price,
            "CarRentalCompany": company,
            "CarPickupAddress": address,
            "CarRentalUrl": url,
            "CarIssueDate": fromDate,
            "CarReturnDate": toDate,
            "Currency": select,
        }),
        success: function () {
            alert("The information was saved!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    })
    setTimeout(function(){
        $("div#pageContent").load("../trip_details.html");
    }, 1000); 
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
    var employeeID = window.planeEmployeeId;

    var today = new Date();
    var fromDate = new Date(from);
    var toDate = new Date(to);

    if (from == "") {
        $('#airplaneFrom').css('border-color', 'red');
        return;
    }
    else if (fromDate <= today) {
        $('#airplaneFrom').css('border-color', 'red');
        alert("The date is incorrect");
        return;
    }
    else {
        $('#airplaneFrom').css('border-color', 'black');
    }

    if (to == "") {
        $('#airplaneTo').css('border-color', 'red');
        return;
    }
    else if (fromDate > toDate) {
        $('#airplaneFrom').css('border-color', 'red');
        $('#airplaneTo').css('border-color', 'red');
        alert("The dates is incorrect");
        return;
    }
    else {
        $('#airplaneTo').css('border-color', 'black');
    }

    if (company == "") {
        $('#airplaneCompany').css('border-color', 'red');
        return;
    }
    else {
        $('#airplaneCompany').css('border-color', 'black');
    }

    if (url == "") {
        $('#airplaneUrl').css('border-color', 'red');
        return;
    }
    else {
        $('#airplaneUrl').css('border-color', 'black');
    }

    if (addressFrom == "") {
        $('#airplaneAddressFrom').css('border-color', 'red');
        return;
    }
    else {
        $('#airplaneAddressFrom').css('border-color', 'black');
    }

    if (addressTo == "") {
        $('#airplaneAddressTo').css('border-color', 'red');
        return;
    }
    else {
        $('#airplaneAddressTo').css('border-color', 'black');
    }

    if (price == "") {
        $('#airplanePrice').css('border-color', 'red');
        return;
    }
    else if (price < 0) {
        $('#airplanePrice').css('border-color', 'red');
        alert("Price can't be less than zero");
        return;
    }
    else {
        $('#airplanePrice').css('border-color', 'black');
    }

    $('#AirplaneModal').modal('toggle');
    $.ajax({
        type: window.airplaneEdit ? "PUT" : "POST",
        url: window.airplaneEdit ? '/api/airplane' : '/api/trip/planeTicket',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            "TripID": window.airplaneEdit ? undefined : tripID,
            "PlaneTicketID": window.airplaneEdit ? window.airplaneId : undefined,
            "Price": price,
            "PlaneTicketUrl": url,
            "ForwardFlightDate": fromDate,
            "ReturnFlightDate": toDate,
            "Airport": addressFrom + "-" + addressTo,
            "FlightCompany": company,
            "EmployeeID": employeeID,
            "Currency": select,
        }),
        success: function () {
            alert("The information was saved!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    })
    setTimeout(function(){
        $("div#pageContent").load("../trip_details.html");
    }, 1000); 
}

function saveHotel() {
    var from = $("#hotelFrom").val();
    var to = $("#hotelTo").val();
    var employeeID = window.hotelEmployeeId;

    var today = new Date();
    var fromDate = new Date(from);
    var toDate = new Date(to);

    if (from == "") {
        $('#hotelFrom').css('border-color', 'red');
        return;
    }
    else if (fromDate <= today) {
        $('#hotelFrom').css('border-color', 'red');
        alert("The date is incorrect");
        return;
    }
    else {
        $('#hotelFrom').css('border-color', 'black');
    }

    if (to == "") {
        $('#hotelTo').css('border-color', 'red');
        return;
    }
    else if (fromDate > toDate) {
        $('#hotelFrom').css('border-color', 'red');
        $('#hotelTo').css('border-color', 'red');
        alert("The dates is incorrect");
        return;
    }
    else {
        $('#hotelTo').css('border-color', 'black');
    }

    if ($("#radioDiv :radio[name=types]:checked").val() == "HOTEL") {
        var company = $("#hotelName").val();
        var address = $("#hotelAddress").val();
        var room = $("#hotelRoom").val();
        var price = $("#hotelPrice").val();
        var url = $("#hotelUrl").val();
        var select = $("#hotelSelect :selected").val();

        if (company == "") {
            $('#hotelName').css('border-color', 'red');
            return;
        }
        else {
            $('#hotelName').css('border-color', 'black');
        }

        if (address == "") {
            $('#hotelAddress').css('border-color', 'red');
            return;
        }
        else {
            $('#hotelAddress').css('border-color', 'black');
        }

        if (room == "") {
            $('#hotelRoom').css('border-color', 'red');
            return;
        }
        else {
            $('#hotelRoom').css('border-color', 'black');
        }

        if (url == "") {
            $('#hotelUrl').css('border-color', 'red');
            return;
        }
        else {
            $('#hotelUrl').css('border-color', 'black');
        }

        if (price == "") {
            $('#hotelPrice').css('border-color', 'red');
            return;
        }
        else if (price < 0) {
            $('#hotelPrice').css('border-color', 'red');
            alert("Price can't be less than zero");
            return;
        }
        else {
            $('#hotelPrice').css('border-color', 'black');
        }

        $('#HotelModal').modal('toggle');
        $.ajax({
            type: window.apartmentEdit ? "PUT" : "POST",
            url:  window.apartmentEdit ? '/api/apartment/hotel/' + window.apartmentId : '/api/trip/hotel',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": window.apartmentEdit ? undefined : tripID,
                "Price": price,
                "EmployeeID": employeeID,
                "CheckIn": new Date(from),
                "CheckOut": new Date(to),
                "ReservationUrl": url,
                "Name": company,
                "Address": address,
                "RoomNumber": room,
                "Currency": select,
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
        setTimeout(function(){
            $("div#pageContent").load("../trip_details.html");
        }, 1000); 
    }
    else {
        var homeAddress = $("#homeAddress").val();
        if (homeAddress == "") {
            $('#homeAddress').css('border-color', 'red');
            return;
        }
        else {
            $('#homeAddress').css('border-color', 'black');
        }

        $('#HotelModal').modal('toggle');
        $.ajax({
            type: window.apartmentEdit ? "PUT" : "POST",
            url: window.apartmentEdit ? '/api/apartment/home/' + window.apartmentId :'/api/trip/home',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "TripID": window.apartmentEdit ? undefined : tripID,
                "EmployeeID": employeeID,
                "CheckIn": new Date(from),
                "CheckOut": new Date(to),
                "Address": homeAddress,
            }),
            success: function () {
                alert("The information was saved!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
        setTimeout(function(){
            $("div#pageContent").load("../trip_details.html");
        }, 1000); 
    }

}