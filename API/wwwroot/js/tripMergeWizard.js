var trip1 = window.currentMergeTrip;
var trip2 = window.tripToMergeWith;
var employeesIds = [];
var departureDateDefault;
var departureDateDefaultString;
var returnDateDefault;
var returnDateDefaultString;

$(document).ready(function () {
    $("div#tripMergeCalendar").load("../Calendar_DB.html");
    getBothTripDates();
});


function getBothTripDates(){
    $.ajax({
        type: "GET",
        url: "/api/tripMerge/dates",
        data: {tripId1:trip1, tripId2:trip2},
        success: function(data, status){
            getDefaultDates(data);
        },
        error: function () { alert('Internet error'); }});
}

function getDefaultDates(data){
    var date1 = new Date(data[0].departureDate);
    var date2 = new Date(data[1].departureDate);
    if (date1 < date2) {
        departureDateDefault = date1;
        departureDateDefaultString = data[0].departureDate;
    }
    else {
        departureDateDefault = date2;
        departureDateDefaultString = data[1].departureDate;
    }
    date1 = new Date(data[0].returnDate);
    date2 = new Date(data[1].returnDate);
    if (date1 > date2) {
        returnDateDefault = date1;
        returnDateDefaultString = data[0].returnDate;
    }
    else {
        returnDateDefault = date2;
        returnDateDefaultString = data[1].returnDate;
    }
    
    //setDefaultDates for the calendar
    $("#departureDate").val(departureDateDefaultString.split('T')[0]);
    $('#departureTime').val(departureDateDefaultString.split('T')[1].slice(0,5));
    $("#arrivalDate").val(returnDateDefaultString.split('T')[0]);
    $('#arrivalTime').val(returnDateDefaultString.split('T')[1].slice(0,5));
    
    getTripsEmployees();
}

function getTripsEmployees(){
    $.ajax({
        type: "GET",
        url: "/api/tripMerge/employees",
        data: {tripId1:trip1, tripId2:trip2},
        success: function(data, status){
            employeesIds = data;
            selectedEployeesForEvents = employeesIds;
            updateTripMergeCalendar();
        },
        error: function () { alert('Internet error'); }});
}

function updateTripMergeCalendar(){
    fillInSelectedDates_FinalRegForm();
    setMonthDate($("#departureDate").val().split("-")[0], $("#departureDate").val().split("-")[1]);
    
    //do not let change the dates unless it's between default ones.
    tripMergingCalendarOn = true;
    tripMergingDepartureDate = departureDateDefault;
    tripMergingReturnDate = returnDateDefault;
}

function cancelMerging(){
    window.tripDetailsTripId = trip1;
    $("div#pageContent").load("../trip_details.html");
}

function mergeTrips(){
    if(confirm("Do you want to proceed with the trip merging? You will not be able to revert this step.")) {

        var dateDeparture = $("#departureDate").val().replace("/","-");
        var departure = dateDeparture + " " + $("#departureTime").val()+ ":00";
        departure = new Date(departure);
        departure.setHours(departure.getHours()+3);


        var dateArrival = $("#arrivalDate").val().replace("/","-");
        var arrival = dateArrival + " " + $("#arrivalTime").val() + ":00";
        arrival = new Date(arrival);
        arrival.setHours(arrival.getHours()+3);
        
        $.ajax({
            type: "PUT",
            url: '/api/tripMerge',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: JSON.stringify({
                "Trip1Id": trip1,
                "Trip2Id": trip2,
                "departureDate": departure,
                "returnDate": arrival
        }),
            success:
                function () {
                    alert("the trips were successfully merged! You will now be redirected to the trip details.");
                    //window.tripDetailsTripId = trip1;
                    //$("div#pageContent").load("../trip_details.html");
                },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })

        //on success, load the newly created trip
    }
}

function deleteMergedTrips(){

    //delete1

    //delete2
}