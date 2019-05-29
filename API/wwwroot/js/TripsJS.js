var from;
var to;
var storage = localStorage;

$(document).ready(function () {

    setFilters();

    var dateFromTo = getTime() || TodaysDate();
    var parts = dateFromTo.split(" - ");
    from = parts[0];
    to = parts[1];

    $('input[name="daterange"]').val(dateFromTo);

    $(function () {
        $('input[name="daterange"]').daterangepicker({
            opens: 'left'
        }, function (start, end, label) {
                
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
            from = start.format('MM/DD/YYYY');
            to = end.format('MM/DD/YYYY');
        });
    });

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    saveFilter();
    loadFilteredTrips();

    $("#Filter").click(function () {
        console.log("here");
        var myNode = document.getElementById("TripInfo");
        console.log(myNode.length);
        while (myNode.firstChild) {
            myNode.removeChild(myNode.firstChild);
        }
        saveFilter();
        loadFilteredTrips();
    });

    function TodaysDate() {
        var today = moment(new Date()).format('MM/DD/YYYY');
        var date = new Date();
        date = date.setMonth(date.getMonth() + 1);
        var monthLater = moment(date).format('MM/DD/YYYY');

        return today + " - " + monthLater;
    }
});

function loadTripDetails(id){
    window.tripDetailsTripId = id;
    $("div#pageContent").load("../trip_details.html"); 
}

function saveFilter() {
    storage.setItem('tripsAwaitingConfirmation', $('input[name="AwaitingConfirmation"]').is(':checked')); //bool
    storage.setItem('tripsConfirmed', $('input[name="Confirmed"]').is(':checked')); //bool
    storage.setItem('fullyPlannedTrips', $('input[name="PlannedTrips"]').is(':checked')); //bool
    storage.setItem('finishedTrips', $('input[name="Finished"]').is(':checked')); //bool
    storage.setItem('myOrganizedTrips', $('input[name="MyOrganized"]').is(':checked')); //bool
    storage.setItem('otherOrganizedTrips', $('input[name="OtherOrganizer"]').is(':checked')); //bool
    storage.setItem('dateFrom', from); //string
    storage.setItem('dateTo', to); //string
}

function getTime() {
    var dateFrom = storage.getItem('dateFrom');
    var dateTo = storage.getItem('dateTo');
    if (dateFrom !== 'undefined' && dateFrom != undefined && dateTo !== 'undefined' && dateTo != undefined) {
        return `${dateFrom} - ${dateTo}`;
    }
}

function loadFilteredTrips() {
    return $.ajax({
        url: 'api/trip/filter',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: {
            "TripsAwaitingConfirmation": storage.getItem('tripsAwaitingConfirmation') === 'true',
            "TripsConfirmed": storage.getItem('tripsConfirmed') === 'true',
            "FullyPlannedTrips": storage.getItem('fullyPlannedTrips') === 'true',
            "FinishedTrips": storage.getItem('finishedTrips') === 'true',
            "MyOrganizedTrips": storage.getItem('myOrganizedTrips') === 'true',
            "OtherOrganizedTrips": storage.getItem('otherOrganizedTrips') === 'true',
            "DateFrom": new Date(from),
            "DateTo": new Date(new Date(to).getTime() + (24 * 60 * 60 * 1000)),
        },
        error: function () {
            console.error('Error');
        },
        success: function (data) {
            $.each(data, function (key, item) {
                var display = displayTrips(item);
                var d1 = document.getElementById("TripInfo");
                d1.insertAdjacentHTML('afterbegin', display);
            });
        },
        type: 'GET'
    });
}

function setFilters() {
    $('input[name="AwaitingConfirmation"]').prop("checked", storage.getItem('tripsAwaitingConfirmation') ? storage.getItem('tripsAwaitingConfirmation') === 'true' : true);
    $('input[name="Confirmed"]').prop("checked", storage.getItem('tripsConfirmed') ? storage.getItem('tripsConfirmed') === 'true' : true);
    $('input[name="PlannedTrips"]').prop("checked", storage.getItem('fullyPlannedTrips') ? storage.getItem('fullyPlannedTrips') === 'true' : true);
    $('input[name="Finished"]').prop("checked", storage.getItem('finishedTrips') ? storage.getItem('finishedTrips') === 'true' : true);
    $('input[name="MyOrganized"]').prop("checked", storage.getItem('myOrganizedTrips') ? storage.getItem('myOrganizedTrips') === 'true' : true);
    $('input[name="OtherOrganizer"]').prop("checked", storage.getItem('otherOrganizedTrips') ? storage.getItem('otherOrganizedTrips') === 'true' : false);
}

function displayTrips(item) {

    var departure = moment(item.departureDate).format('YYYY-MM-DD kk:mm');
    var arrival = moment(item.returnDate).format('YYYY-MM-DD HH:mm');
    var queryString = "?tripID=" + item.id;
    console.log(queryString);
    var myvar = '<div class="container shadow-sm p-3 mb-5 bg-white rounded bg-light rounded-lg p-3">' +
        `        <a class="h3" onclick="loadTripDetails(${item.id})" href="javascript:void(0)"` + '">' + item.departureOffice.city + ', ' + item.departureOffice.country + ' - ' + item.arrivalOffice.city + ', ' + item.arrivalOffice.country + '</a>' +
        '        <div class="row">' +
        '            <div class="col-sm-1 ml-auto">' +
        '              <div class="progress" data-percentage="' + item.confirmedProcentage + '">' +
        '                <span class="progress-left">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <span class="progress-right">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <div class="progress-value">' +
        '                  <div>' +
        '                    ' + item.employeeCount + '<br>' +
        '                    <span>Confirmed</span>' +
        '                  </div>' +
        '                </div>' +
        '              </div>' +
        '            </div>' +

        '            <div class="col-sm-2">' +
        '              <div class="progress" data-percentage="' + item.accomodationProcentage + '">' +
        '                <span class="progress-left">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <span class="progress-right">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <div class="progress-value">' +
        '                  <div>' +
        '                    ' + item.accomodationCount + '<br>' +
        '                    <span>Accomodation</span>' +
        '                  </div>' +
        '                </div>' +
        '              </div>' +
        '            </div>' +
        '          ' +
        '' +
        '          <div class="col-sm-1">' +
        '              <div class="progress" data-percentage="' + item.planeTicketProcentage + '">' +
        '                <span class="progress-left">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <span class="progress-right">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <div class="progress-value">' +
        '                  <div>' +
        '                    ' + item.planeTicketCount + '<br>' +
        '                    <span>Plane tickets</span>' +
        '                  </div>' +
        '                </div>' +
        '              </div>' +
        '            </div>' +
        '            ' +
        '            ' +
        '            <div class="col-sm-2">' +
        '              <div class="progress" data-percentage="' + item.carRentalProcentage + '">' +
        '                <span class="progress-left">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <span class="progress-right">' +
        '                  <span class="progress-bar"></span>' +
        '                </span>' +
        '                <div class="progress-value">' +
        '                  <div>' +
        '                    ' + item.carRentalCount + '<br>' +
        '                    <span>Car rental</span>' +
        '                  </div>' +
        '                </div>' +
        '              </div>' +
        '            </div>' +
        '            </div>' +
        '' +
        '        <h5>Departure: ' + departure + ' Arrival: ' + arrival + '</h5>' +
        '        ' +
        '      </div><br>';
    return myvar;
}