//dynamic CSS

var weekend_color = "#f4f4f4";
var today_color = "#d1e2ff";
var notThisMonth_color = "grey";

var weekHoursSeparator_border = "1px solid lightgrey";

//constants
var weekdays = ['Sk', 'Pr', 'An', 'Tr', 'Kt', 'Pn', 'St'];
var months = ['January', 'February', 'March', 'April', 'May', 'June','July', 'August','September','October','November','December'];

//vars
var startingWeekDate = new Date();
startingWeekDate.setDate(startingWeekDate.getDate() - startingWeekDate.getDay()+1);
var startingMonthDate = new Date();
var currentMonth = new Date().getMonth();
var currentYear = new Date().getFullYear();


//on load update
window.onload = updateCalendarMonth();


//Calendar logic for Month mode
function updateCalendarMonth(){
    
    _remove_highlights(); 
    
    startingMonthDate = new Date(currentYear, currentMonth, 1);
    startingMonthDate.setDate(startingMonthDate.getDate() - startingMonthDate.getDay()+1);
//    
    //get the elements to fill
    var days = $(".col.cal > .day");
//    
    days.parent().css("background-color", "white");
    
    //fill month
    var newDate = new Date(startingMonthDate);
    for (var i = 0; i < 6*7; i++){
        var $element = days.eq(i);
        $element.text(newDate.getDate());
        $element.parent().attr("id",newDate.getFullYear()+'-'+(_addZero(newDate.getMonth()+1))+'-'+_addZero(newDate.getDate()));
        if (newDate.getMonth() != currentMonth) $element.parent().css("background-color", notThisMonth_color);
        else if (_isToday(newDate)) $element.parent().css("background-color",today_color);
        else if ($element.parent().hasClass("St") || $element.parent().hasClass("Sk"))
            $element.parent().css("background-color", weekend_color);
        newDate.setDate(newDate.getDate()+1);
    }
    //set Month header (Month name + year)
    $(".nav_month_header").text(months[currentMonth]);
    $(".nav_year_header").text(currentYear+" m.");
    
    var selectedEployees = $('table#sort tbody tr.selected td#NrColumn').map(function(){
        return $.trim($(this).text());
    }).get();
    getEvents(startingMonthDate,newDate,selectedEployees);
    fillInSelection_afterCalendarNavigation(currentYear, currentMonth+1);
}


//Calendar logic for Week mode
function updateCalendarWeek(){
    
    _remove_highlights(); 
    
    var currentMonth = startingWeekDate.getMonth();

    //get the elements to fill
    var days = $(".row.day-header > .col:not(.hour_util)");
    days.css("background-color", "white");
    $(".col.cal-w").css("background-color", "white");
    $(".col.cal-w.St").css("background-color", weekend_color);
    $(".col.cal-w.Sk").css("background-color", weekend_color);
    
    //fill in the week
    var newDate = new Date(startingWeekDate);
    for (var i = 0; i < 7; i++){
        var $element = days.eq(i);
        $element.text(newDate.getDate());
        
        var $dayElemets = $(".col.cal-w."+weekdays[newDate.getDay()]);
        $dayElemets.each(function(){
            $(this).attr("id",newDate.getFullYear()+'-'+(_addZero(newDate.getMonth()+1))+'-'+_addZero(newDate.getDate())+"h"+$(this).parent().attr("id"));
            
            if (parseInt($(this).parent().attr("id")) % 2 == 0){
                $(this).parent().css("border-bottom",weekHoursSeparator_border);
            }
            if ($(this).parent().attr("id")=="01") $(this).parent().css("border-top", weekHoursSeparator_border);
            
        });
        
        if (_isToday(newDate)) {
            $(".col.cal-w."+weekdays[newDate.getDay()]).css("background-color", today_color);
            $(".col.d-"+weekdays[newDate.getDay()]).css("background-color", today_color);
        }
        i != 6 ? newDate.setDate(newDate.getDate()+1) : "";
    }
    
    
    //set Month header (Month name + year)
    var month = months[currentMonth];
    var year = startingWeekDate.getFullYear();
    
    if (startingWeekDate.getDate() > newDate.getDate())  {
        currentMonth != 11 ? month = month + ' - ' + months[currentMonth+1] : month = month + ' - ' + months[0];
        if (currentMonth == 11) year = year + ' - ' + (year+1);
    }
        
    $(".nav_month_header").text(month);
    $(".nav_year_header").text(year + ' m.');

    var selectedEployees = $('table#sort tbody tr.selected td#NrColumn').map(function(){
        return $.trim($(this).text());
    }).get();
    getEvents(startingWeekDate, newDate, selectedEployees);
    fillInSelection_afterCalendarNavigation(currentYear, currentMonth+1);
}


//Events for Month Mode
function nextMonth(){  
    if (currentMonth == 11) {
        currentMonth = 0;
        currentYear++;
    }
    else currentMonth++;
    updateCalendarMonth();

}

function previousMonth(){
    if (currentMonth == 0) {
        currentMonth = 11;
        currentYear--;
    }
    else currentMonth--;
    updateCalendarMonth();

}


//Events for Week mode
function nextWeek(){
    _setCurrentDates(7);
    updateCalendarWeek();

}

function previousWeek(){
    _setCurrentDates(-7);
    updateCalendarWeek();

}

function _setCurrentDates(nmb_days){    
    startingWeekDate.setDate(startingWeekDate.getDate()+nmb_days);
    currentMonth = startingWeekDate.getMonth();
}

//Mode changing events
function weekButtonClicked(){
   $(".container.calendar.month").addClass("d-none");
   $(".container.calendar.week").removeClass("d-none");
   $("button.btn.next").attr("onClick","nextWeek()");
   $("button.btn.previous").attr("onClick","previousWeek()");
    
   // Add additional column to header
   expandHeader(true);
   updateCalendarWeek();
}

function monthButtonClicked(){
   $(".container.calendar.month").removeClass("d-none");
   $(".container.calendar.week").addClass("d-none");
   $("button.btn.next").attr("onClick","nextMonth()");
   $("button.btn.previous").attr("onClick","previousMonth()");  
    
   // remove additional column in header
   expandHeader(false);
   updateCalendarMonth();
}

function expandHeader(show){
    if(show){
        $("div.row.heading > .col.hour_util").removeClass("d-none");
    }
    else
        $("div.row.heading > .col.hour_util").addClass("d-none");
}


