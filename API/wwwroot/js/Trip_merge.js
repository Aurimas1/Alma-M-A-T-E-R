function startTripMerging(){
    //Go to merge page
}


function getTripsForMerging(tripId){
    $.ajax({
        type: "GET",
        url: '/api/tripMerge/' + tripId,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            $.each(data, function (key, item) {
                displayMergableTrip(item);
            });

            $("div#tripsList span").css({
                "font-weight": 400
            })
            $("div.tripListItem").on('click', function(){
                $("div#tripsList").removeClass("selected");
                $(this).addClass("selected");
                
            })
            $("p.offices").css("color", "#007bff");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function getIfTheTripCanBeMerged(tripId){
    $.ajax({
        type: "GET",
        url: '/api/tripMerge/canMerge/' + tripId,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            if (data == true){
                $("#mergeTrip").css({
                    "display": ""});
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function displayMergableTrip(item){
    var departure = moment(item.departureDate).format('YYYY-MM-DD kk:mm');
    var arrival = moment(item.returnDate).format('YYYY-MM-DD HH:mm');
    $("div#tripsList").append(
        "<div class=\"h5 container shadow-sm p-3 mb-5 bg-white rounded bg-light rounded-lg p-3 tripListItem\"><p class='offices'>" 
        + item.departureOffice + " - " + item.arrivalOffice + "</p> "
        + "<p> Departure: " + departure + "<br/>" + "Arrival: " + arrival + "</p>"
        + "<span>Employees: " + item.confirmedEmployeesCount + "/" + item.employeesCount + "</span><br/>"
        + "<span>Accommodation: " +  item.accomodationCount + "</span><br/>"
        + (item.planeTicketsNeeded ? ("<span>Plane tickets: " + item.boughtPlaneTicketsCount + "</span><br/>") : "")
        + "<span>Cars rented: " + item.carRentalCount + "</span><br/>"
        + "<span>Gas compensations: " + item.gasCompensationCount + "</span><br/>"+"</div>");
}
