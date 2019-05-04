// Dynamic CSS
var eventColor = "#fa5050";

var events = null;

function getEvents(from, to, employeeIds){
    var array=[1,2,3];
    var dateFrom = from.getFullYear()+'-'+_addZero(from.getMonth()<12?from.getMonth()+1:0)+'-'+_addZero(from.getDate());
    var dateTo = to.getFullYear()+'-'+_addZero(to.getMonth()<12?to.getMonth()+1:0)+'-'+_addZero(to.getDate());
    $.ajax({
        type: "GET",
        url: "/api/event",
        data: {dateFrom:dateFrom, dateTo:dateTo, employeeIds: array},
        success: function(data, status){
        addEvents(data);
    },
        traditional: true});
}
function addEvents(data){
    events = data;
    var date;
    
    $("*").removeAttr("data-content");
    $("div.col.cal, div.col.cal-w").removeClass("event");
    $("div.col.cal, div.col.cal-w").popover('dispose');
    for (var i = 0; i < events.length; i++){
         
         date = events[i].date;
         if (isMonthMode()){
             var $element = $("#"+date+".col.cal")
              $($element).css("background-color", eventColor);
              $($element).addClass("event");
              addPopoverMonth($($element), date); //Popover to show events
             
         }  
         else {     
             events[i].events.forEach(function(el){                 
                 var hour_from = el.timeFrom.split(':')[0];
                 var hour_to = el.timeTo.split(':')[0];
                 var hour_from_int = parseInt(hour_from);
                 var hour_to_int = parseInt(hour_to);
                 (el.timeTo.split(':'))[1] > 0 ? hour_to_int++ : "";
                 for(var j = hour_from_int+1; j <= hour_to_int; j++){
                     var $element = $("#"+date+"h"+_addZero(j)+".col.cal-w");
                     $($element).css("background-color", eventColor);
                     $($element).addClass("event");
                     addPopoverWeek($($element),el.timeFrom, el.timeTo, el.fullName);
                 }
                 
             });
         }
     }
}

function addPopoverMonth(element, date){
    
    _addPopoverAttributes(element, "Today's events");
    var todaysEvents = events.filter((x) => {return x.date == date});
    todaysEvents[0].events.forEach(function(el){
        $(element).attr("data-content", $(element).attr("data-content") + el.timeFrom + "-" + el.timeTo + " " + el.fullName + "<br/>");
    });    
    $(element).popover();
}

function addPopoverWeek(element, timeFrom, timeTo, fullName){
    
    if (!$(element).attr("data-content")) _addPopoverAttributes(element, "Current events");
    $(element).attr("data-content", $(element).attr("data-content") + timeFrom + "-" + timeTo + " " + fullName + "<br/>");  
    $(element).popover();
}

function isMonthMode(){
    return $(".container.calendar.week").hasClass("d-none");
}
