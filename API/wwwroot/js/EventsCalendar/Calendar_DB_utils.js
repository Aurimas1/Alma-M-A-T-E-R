
//Utils

function _isToday(date2){
    var date1 = new Date();
    if (date1.getFullYear() == date2.getFullYear() && date1.getMonth() == date2.getMonth() && date1.getDate() == date2.getDate()) return true;
    else return false;
}

function _addZero(digit){
    if ((digit+'').length == 1){
        return "0"+digit;
    }
    else return digit;
}

function _remove_highlights(){
    $("*").removeClass("highlighted");
    $("*").removeClass("highlighted_event");
    $("*").removeClass("highlighted_event_week"); 
}

function _addPopoverAttributes(element, title){
    $(element).attr("data-toggle", "popover");
    $(element).attr("title", title);
    $(element).attr("data-html", "true");
    $(element).attr("data-placement", "right");
    $(element).attr("data-trigger", "focus");
    $(element).attr("tabindex", "0");
    $(element).attr("data-content", "");
}