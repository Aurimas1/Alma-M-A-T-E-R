$(document).ready(function () {

    var dateFromTo;

    $('input[name="AwaitingConfirmation"]').prop("checked", true);
    $('input[name="Confirmed"]').prop("checked", true);
    $('input[name="PlannedTrips"]').prop("checked", true);
    $('input[name="MyOrganized"]').prop("checked", true);

    var tripsAwaitingConfirmation = $('input[name="AwaitingConfirmation"]').is(':checked');
    var tripsConfirmed = $('input[name="Confirmed"]').is(':checked');
    var fullyPlannedTrips = $('input[name="PlannedTrips"]').is(':checked');
    var finishedTrips = $('input[name="Finished"]').is(':checked');
    var myOrganizedTrips = $('input[name="MyOrganized"]').is(':checked');
    var otherOrganizedTrips = $('input[name="OtherOrganizer"]').is(':checked');

    dateFromTo = TodaysDate();

    $('input[name="daterange"]').val(dateFromTo);

    $(function () {
        $('input[name="daterange"]').daterangepicker({
            opens: 'left'
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
            dateFrom = start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY');
        });
    });

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    var dates = dateFromTo.split(" - ");
    var dateFrom = new Date(dates[0]);
    var dateTo = new Date(dates[1]);

    $.ajax({
        url: 'api/trip/filter',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: {
            "TripsAwaitingConfirmation": tripsAwaitingConfirmation,
            "TripsConfirmed": tripsConfirmed,
            "FullyPlannedTrips": fullyPlannedTrips,
            "FinishedTrips": finishedTrips,
            "MyOrganizedTrips": myOrganizedTrips,
            "OtherOrganizedTrips": otherOrganizedTrips,
            "DateFrom": dateFrom,
            "DateTo": dateTo
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

    $("#Filter").click(function () {
        console.log("here");
        var myNode = document.getElementById("TripInfo");
        console.log(myNode.length);
        while (myNode.firstChild) {
            myNode.removeChild(myNode.firstChild);
        }

        tripsAwaitingConfirmation = $('input[name="AwaitingConfirmation"]').is(':checked');
        tripsConfirmed = $('input[name="Confirmed"]').is(':checked');
        fullyPlannedTrips = $('input[name="PlannedTrips"]').is(':checked');
        finishedTrips = $('input[name="Finished"]').is(':checked');
        myOrganizedTrips = $('input[name="MyOrganized"]').is(':checked');
        otherOrganizedTrips = $('input[name="OtherOrganizer"]').is(':checked');
        var dates = dateFromTo.split(" - ");
        var dateFrom = new Date(dates[0]);
        var dateTo = new Date(dates[1]);

        $.ajax({
            url: 'api/trip/filter',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: {
                "TripsAwaitingConfirmation": tripsAwaitingConfirmation,
                "TripsConfirmed": tripsConfirmed,
                "FullyPlannedTrips": fullyPlannedTrips,
                "FinishedTrips": finishedTrips,
                "MyOrganizedTrips": myOrganizedTrips,
                "OtherOrganizedTrips": otherOrganizedTrips,
                "DateFrom": dateFrom,
                "DateTo": dateTo
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
    });

    function TodaysDate() {
        var today = moment(new Date()).format('MM/DD/YYYY');
        var date = new Date();
        date = date.setMonth(date.getMonth() + 1);
        var monthLater = moment(date).format('MM/DD/YYYY');

        return today + " - " + monthLater;
    }



    function displayTrips(item) {

        var departure = moment(item.departureDate).format('YYYY-MM-DD kk:mm');
        var arrival = moment(item.returnDate).format('YYYY-MM-DD HH:mm');
        var queryString = "?tripID=" + item.id;
        console.log(queryString);
        var myvar = '<div class="container bg-light rounded-lg p-3">' +
            '        <a class="h3" href="trip_details.html?tripID=' + item.id + '">' + item.departureOffice.city + ', ' + item.departureOffice.country + ' - ' + item.arrivalOffice.city + ', ' + item.arrivalOffice.country + '</a>' +
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
});