$('select[name="hour_from"]').on('change', function() {
    trip_start_hour = $(this).val();
    if(!isMonthMode()) {
        _remove_highlights();
        fillInSelection_afterCalendarNavigation(currentYear, currentMonth+1);
    }
});

$('select[name="hour_to"]').on('change', function() {
    trip_end_hour = $(this).val();
    if(!isMonthMode()) {
        _remove_highlights();
        fillInSelection_afterCalendarNavigation(currentYear, currentMonth+1);
    }
});


