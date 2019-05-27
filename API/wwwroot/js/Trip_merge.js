var mergingStarted = false;

function startTripMerging(){
    window.tripToMergeWith = $("div.tripListItem.selected").attr('id');
    window.currentMergeTrip = window.tripDetailsTripId;
    console.log(window.tripToMergeWith + " " + window.currentMergeTrip);
    mergingStarted = true;
    $('#TripMergeModal').modal('toggle');
}

$('#TripMergeModal').on('hidden.bs.modal', function (e) {
    if (mergingStarted) $("div#pageContent").load("../tripMergeWizard.html");
})

function getTripsForMerging(tripId){
    $.ajax({
        type: "GET",
        url: '/api/tripMerge/' + tripId,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            //remove previously added trips
            $("div#tripsList").empty();
            $.each(data, function (key, item) {
                displayMergableTrip(item);
            });

            $("div#tripsList span").css({
                "font-weight": 400
            })
            $("div.tripListItem").on('click', function(){
                $("div.tripListItem").removeClass("selected");
                $(this).addClass("selected");
                
                if ($("div.tripListItem.selected").length > 0) $("button#mergingModalButton").prop('disabled', false);
                else $("button#mergingModalButton").prop('disabled', true);
                
            })
            $("p.offices").css("color", "#d30239");
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
        "<div id=" + item.tripID + " class=\"h5 container shadow-sm p-3 mb-5 bg-white rounded bg-light rounded-lg p-3 tripListItem\"><p class='offices'>" 
        + item.departureOffice + " - " + item.arrivalOffice + "</p> "
        + "<p> Departure: " + departure + "<br/>" + "Arrival: " + arrival + "</p>"
        + "<span>Employees: " + item.confirmedEmployeesCount + "/" + item.employeesCount + "</span><br/>"
        + "<span>Accommodation: " +  item.accomodationCount + "</span><br/>"
        + (item.planeTicketsNeeded ? ("<span>Plane tickets: " + item.boughtPlaneTicketsCount + "</span><br/>") : "")
        + "<span>Cars rented: " + item.carRentalCount + "</span><br/>"
        + "<span>Gas compensations: " + item.gasCompensationCount + "</span><br/>"+"</div>");
}
