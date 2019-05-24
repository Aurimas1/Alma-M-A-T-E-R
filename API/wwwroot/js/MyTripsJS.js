$(document).ready(function () {
        loadMyTrips();
});

function loadTripDetails(id) {
    window.userTripDetailsTripId = id;
    $("div#pageContent").load("../userTripDetails.html");
}

function loadMyTrips() {
    return $.ajax({
        url: 'api/trip/myTrips',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: {
        },
        error: function () {
            console.error('Error');
        },
        success: function (data) {
            $.each(data, function (key, item) {
                var display = displayTrips(item);
                var d1 = document.getElementById("MyTripInfo");
                d1.insertAdjacentHTML('afterbegin', display);
            });
        },
        type: 'GET'
    });
}

function displayTrips(item) {

    var departure = moment(item.departureDate).format('YYYY-MM-DD kk:mm');
    var arrival = moment(item.returnDate).format('YYYY-MM-DD HH:mm');
    var queryString = "?tripID=" + item.id;

    if (item.status[0] == "APPROVED") {
        var status = '<p class="d-inline badge-pill badge-success" >' + item.status[0] + '</p >';
    }
    else status = '<p class="h5 d-inline badge-pill badge-warning" >' + item.status[0] + '</p >';

    var myvar = '<div class="h5 container shadow-sm p-3 mb-5 bg-white rounded bg-light rounded-lg p-3 tripListItem">' +
        `        <a class="h3" onclick="loadTripDetails(${item.id})" href="javascript:void(0)"` + '">' + item.departureOffice.city + ', ' + item.departureOffice.country + ' - ' + item.arrivalOffice.city + ', ' + item.arrivalOffice.country + '</a>' +
        '        <div class="row">' +
        '            <div class="d-inline col-sm-2 ml-auto"> <h4>Trip status: </h4>' + status +
        '            </div>' +
        '            </div>' +
        '        <h5>Departure: ' + departure + ' Arrival: ' + arrival + '</h5>' +
        '      </div><br>';
    return myvar;
    
}