// Dynamic CSS
var eventColor = "#fa5050"

var events_json = '[\
{\
"date": "2019-03-18",\
"events": [\
{"time_from": "10:15", "time_to": "16:00", "full_name": "vardenis pavardenis"},\
{"time_from": "12:45", "time_to": "14:00", "full_name": "vardenis K."}\
]\
},\
{\
"date": "2019-03-19",\
"events": [\
{"time_from": "15:15", "time_to": "22:00", "full_name": "Something A."},\
{"time_from": "18:45", "time_to": "21:00", "full_name": "Kazkas anonimas"}\
]\
},\
{\
"date": "2019-03-22",\
"events": [\
{"time_from": "15:15", "time_to": "16:00", "full_name": "Anabelė A."},\
{"time_from": "18:45", "time_to": "20:00", "full_name": "Antanas K."}\
]\
},\
{\
"date": "2019-03-23",\
"events": [\
{"time_from": "15:15", "time_to": "24:00", "full_name": "vardenis pavardenis"}\
]\
},\
{\
"date": "2019-03-24",\
"events": [\
{"time_from": "00:00", "time_to": "24:00", "full_name": "vardenis pavardenis"}\
]\
},\
{\
"date": "2019-03-25",\
"events": [\
{"time_from": "00:00", "time_to": "16:00", "full_name": "vardenis pavardenis"}\
]\
}\
]'

var events = JSON.parse(events_json);


function addEvents(){
    
    var date;
    $("*").removeAttr("data-content");
    $("*").removeClass("event");
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
                 var hour_from = el.time_from.split(':')[0];
                 var hour_to = el.time_to.split(':')[0];
                 var hour_from_int = parseInt(hour_from);
                 var hour_to_int = parseInt(hour_to);
                 (el.time_to.split(':'))[1] > 0 ? hour_to_int++ : "";
                 for(var j = hour_from_int+1; j <= hour_to_int; j++){
                     var $element = $("#"+date+"h"+_addZero(j)+".col.cal-w");
                     $($element).css("background-color", eventColor);
                     $($element).addClass("event");
                     addPopoverWeek($($element),el.time_from, el.time_to, el.full_name);
                 }
                 
             });
         }
     }
}

function addPopoverMonth(element, date){
    
    _addPopoverAttributes(element, "Today's events");
    var todaysEvents = events.filter((x) => {return x.date == date});
    todaysEvents[0].events.forEach(function(el){
        $(element).attr("data-content", $(element).attr("data-content") + el.time_from + "-" + el.time_to + " " + el.full_name + "<br/>");
    });    
    $(element).popover();
}

function addPopoverWeek(element, time_from, time_to, full_name){
    
    if (!$(element).attr("data-content")) _addPopoverAttributes(element, "Current events");
    $(element).attr("data-content", $(element).attr("data-content") + time_from + "-" + time_to + " " + full_name + "<br/>");  
    $(element).popover();
}

function isMonthMode(){
    return $(".container.calendar.week").hasClass("d-none");
}
