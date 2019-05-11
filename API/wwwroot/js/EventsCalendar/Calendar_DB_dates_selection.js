var selection_started = false;
var selection_ended = false;

var trip_start_date = "";
var trip_start_hour = "";
var trip_end_date = "";
var trip_end_hour = "";

$(".col.cal, .col.cal-w").click(function(){
    if (!selection_started)
{
    //Event starts
    selection_started = true;
    selection_ended = false;
    cleanSelection();
    var selected_date = $(this).attr("id").split("h");
    trip_start_date = selected_date[0];
    selected_date.length == 1 ? trip_start_hour = "00:00" : trip_start_hour =  _addZero(parseInt(selected_date[1]-1)) + ":00";
    $(this).addClass("highlighted");
}
    else {
        //Event ends
         var selected_date = $(this).attr("id").split("h");
        trip_end_date = selected_date[0];
        selected_date.length == 1 ? trip_end_hour = "24:00" : trip_end_hour =  _addZero(parseInt(selected_date[1])) + ":00";
        selection_started = false;
        selection_ended = true;
        if (needToFlipDates()) flipDates();
        $(this).addClass("highlighted");
        fillInSelection();
        setTripDatesFromSelection();
    }
});

//Mouse clicked outside of the calendar
$(".events_calendar > *:not(.calendar.month, .calendar.week, .nav_heading)").click(function(){
    if (selection_started && !selection_ended){
        selection_started = false;
        cleanSelection();
    }
});

// Fill in dates between start and end
function fillInSelection(){
    
    var startingDate = new Date (trip_start_date);
    var endingDate = new Date (trip_end_date);
    
    if(isMonthMode()){

        startingDate.setDate(startingDate.getDate());
        while(startingDate <= endingDate){
            $("#" + startingDate.getFullYear() + "-" + _addZero((startingDate.getMonth()+1)) + "-" + _addZero(startingDate.getDate()) + ".col.cal").addClass("highlighted");
            startingDate.setDate(startingDate.getDate()+1);
        }
        $(".col.cal.highlighted.event p").addClass("highlighted_event");
    }
    
    else {     
        var tempDate = new Date (trip_start_date);
        //while not the last date
        //On first day fill from start hour till 24:00
        //On other days fill from 00:00 till 24:00
        while (tempDate < endingDate){
            var i = 1;
            if (tempDate.getTime() == startingDate.getTime()) i = parseInt(trip_start_hour)+1;
            for (i; i <= 24; i++){
                 $("#" + tempDate.getFullYear() + "-" + _addZero((tempDate.getMonth()+1)) + "-" + _addZero(tempDate.getDate()) + "h" + _addZero(i) + ".col.cal-w").addClass("highlighted");
            }
            tempDate.setDate(tempDate.getDate()+1);
        }
        
        //Last date
        //if not also starting date, fill from 00:00 till ending hour
        //if starting date = ending date, fill from start hour till ending hour
        var i = 1;
        if (!(trip_start_date < trip_end_date)) i = parseInt(trip_end_hour);
        for (i; i <= parseInt(trip_end_hour); i++){
             $("#" + tempDate.getFullYear() + "-" + _addZero((tempDate.getMonth()+1)) + "-" + _addZero(tempDate.getDate()) + "h" + _addZero(i) + ".col.cal-w").addClass("highlighted");
        }
        
        $(".col.cal-w.highlighted.event").addClass("highlighted_event_week");
    }
}

function setTripDatesFromSelection(){
    
    //set dates
    $("input[name='date_from']").val(trip_start_date);
    $("input[name='date_to']").val(trip_end_date);
    
    //set hours
    $("select[name='hour_from']").val(trip_start_hour);
    $("select[name='hour_to']").val(trip_end_hour);
    
    if ($("div.event.highlighted").length) pickedDatesWithEvents = true;
}

function cleanSelection(){
    trip_start_date = "";
    trip_start_hour = "";
    trip_end_date = "";
    trip_end_hour = "";
    $("input[name='date_from']").val("");
    $("input[name='date_to']").val("");
    $("select[name='hour_from']").val("00:00");
    $("select[name='hour_to']").val("24:00");   
    _remove_highlights();
    pickedDatesWithEvents = false;
}

function needToFlipDates(){
    if (new Date(trip_start_date) > new Date(trip_end_date)) return true;
    else return false;
}

function flipDates(){
    var temp = trip_start_date;
    trip_start_date = trip_end_date;
    trip_end_date = temp;
    if (!isMonthMode()){
        temp = trip_start_hour;
        trip_start_hour = trip_end_hour;
        trip_end_hour = temp;
    }
}

function fillInSelection_afterCalendarNavigation(currentYear, currentMonth){
    if (trip_start_date != null){
        if ((currentYear >= parseInt(trip_start_date.split("-")[0]) && currentYear <= parseInt(trip_end_date.split("-")[0])) && (currentMonth >= parseInt(trip_start_date.split("-")[1]) && currentMonth <= parseInt(trip_end_date.split("-")[1]))){
            fillInSelection();
        }
    }
}