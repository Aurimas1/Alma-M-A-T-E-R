function offerTripMerging(){
    //Go to merge page
}

function getIfTheTripCanBeMerged(tripId){
    $.ajax({
        type: "GET",
        url: '/api/tripMerge/canMerge/' + tripId,
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            if (data == true){
                $("#mergeTrip").css({
                    "display": ""});
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}