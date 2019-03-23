
$(document).ready(function () {


    $('[data-toggle="popover"]').popover({
    });

    $(function () {
        $('input[name="daterange"]').daterangepicker({
            opens: 'left'
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        });
    });

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });



    /* $.ajax({
         url:"https://api.myjson.com/bins",
         type:"POST",
         data:'[{ "Trip" : "Vilnius", "Arrival": "Kaunas", "DepartureTime" : "2018-10-12 12:00",  "ArrivalTime" : "2018-10-12 13:30", "Cost" : "3.00" }, { "Departure" : "Vilnius", "Arrival": "Varšuva", "DepartureTime" : "2018-10-13 12:00",  "ArrivalTime" : "2018-10-14 15:20", "Cost" : "15.00"}, { "Departure" : "Vilnius", "Arrival": "Klaipėda","DepartureTime" : "2018-10-13 08:00","ArrivalTime" : "2018-10-13 12:00","Cost" : "3.00"}]',
         contentType:"application/json; charset=utf-8",
         dataType:"json",
         success: function(data, textStatus, jqXHR){
             var json = JSON.stringify(data);
                 var uri = $("#data").val(json);
         }
     });
     // Send an AJAX request
     $.ajax({
         url: uri,
         data: {
            format: 'json'
         },
         error: function() {
            console.error('Error');
         },
         success: function(data) {
             var trips = JSON.parse(data);
             console.log(parsed_data.success);
             sort(trips);
         },
         type: 'GET'
      });
     });
 */
});