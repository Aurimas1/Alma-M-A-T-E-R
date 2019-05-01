$(document).ready(function () {

    var tripsAwaitingConfirmation = $('input[name="AwaitingConfirmation"]').val;
    var tripsConfirmed = $('input[name="Confirmed"]').val;
    var fullyPlannedTrips = $('input[name="PlannedTrips"]').val;
    var tirpsInProgress = $('input[name="InProgress"]').val;
    var finishedTrips = $('input[name="Finished"]').val;
    var myOrganizedTrips = $('input[name="MyOrganized"]').val;
    var otherOrganizedTrips = $('input[name="OtherOrganized"]').val;
    var dateFromTo = $('input[name="daterange"]').val

    var display = displayTrips(); 
    //let frag = document.createRange().createContextualFragment(display);
    var d1 = document.getElementById("menu-toggle");
    d1.insertAdjacentHTML('afterend', display);
    d1.insertAdjacentHTML('afterend', display);
    d1.insertAdjacentHTML('afterend', display);
    $('[data-toggle="popover"]').popover({
    });

    $('input[name="daterange"]').val(String(JSON.parse(sessionStorage['date'] || '{}')));

    $(function () {
        $('input[name="daterange"]').daterangepicker({
            opens: 'left'
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
            sessionStorage.date = JSON.stringify(start.format('MM/DD/YYYY') + " - " + end.format('MM/DD/YYYY'));
            console.log(JSON.parse(sessionStorage.date));
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
            format: 'json',
            TripsAwaitingConfirmation: tripsAwaitingConfirmation,
            TripsConfirmed: tripsConfirmed,
            FullyPlannedTrips: fullyPlannedTrips,
            TirpsInProgress: tirpsInProgress,
            FinishedTrips: finishedTrips,
            MyOrganizedTrips: myOrganizedTrips,
            OtherOrganizedTrips: otherOrganizedTrips,
            DateFromTo: dateFromTo;
         },
         error: function() {
            console.error('Error');
         },
         success: function(data) {
             console.log(parsed_data.success);
             $.each(data, function (key, item) {
                var display = displayTrips(item); 
                var d1 = document.getElementById("menu-toggle");
                d1.insertAdjacentHTML('afterend', display);
            });
         },
         type: 'GET'
      });
     });
 */
});

function displayTrips(){
    var myvar = '<div class="container bg-light rounded-lg p-3">'+
'        <a class="h3" href="trip_details.html">Vilnius, Lithuania - Warsaw, Poland</a>'+
'        <div class="row">'+
'            <div class="col-sm-1 ml-auto">'+
'              <div class="progress" data-percentage="20">'+
'                <span class="progress-left">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <span class="progress-right">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <div class="progress-value">'+
'                  <div>'+
'                    1/5<br>'+
'                    <span>Confirmed</span>'+
'                  </div>'+
'                </div>'+
'              </div>'+
'            </div>'+
'            <div class="col-sm-2">'+
'              <div class="progress" data-percentage="40">'+
'                <span class="progress-left">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <span class="progress-right">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <div class="progress-value">'+
'                  <div>'+
'                    2/5<br>'+
'                    <span>Accomodation</span>'+
'                  </div>'+
'                </div>'+
'              </div>'+
'            </div>'+
'          '+
''+
'          <div class="col-sm-1">'+
'              <div class="progress" data-percentage="80">'+
'                <span class="progress-left">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <span class="progress-right">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <div class="progress-value">'+
'                  <div>'+
'                    4/5<br>'+
'                    <span>Plane tickets</span>'+
'                  </div>'+
'                </div>'+
'              </div>'+
'            </div>'+
'            '+
'            '+
'            <div class="col-sm-2">'+
'              <div class="progress" data-percentage="100">'+
'                <span class="progress-left">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <span class="progress-right">'+
'                  <span class="progress-bar"></span>'+
'                </span>'+
'                <div class="progress-value">'+
'                  <div>'+
'                    1/1<br>'+
'                    <span>Car rental</span>'+
'                  </div>'+
'                </div>'+
'              </div>'+
'            </div>'+
'            </div>'+
''+
'        <h5>Departure: 2019-01-25 16:00 Arrival: 2019-02-02 12:00</h5>'+
'        '+
'      </div><br>';
    return myvar;
}


$(function() {
        // variable to store our current state
        var cbstate;
        // bind to the onload event
        window.addEventListener('load', function() {
          // Get the current state from localstorage
          // State is stored as a JSON string
          cbstate = JSON.parse(sessionStorage['CBState'] || '{}');

        
          // Loop through state array and restore checked 
          // state for matching elements
          for(var i in cbstate) {
            var el = document.querySelector('input[name="' + i + '"]');
            if (el) el.checked = true;
          }

          

        
          // Get all checkboxes that you want to monitor state for
          var cb = document.getElementsByClassName('box');
          
          // Loop through results and ...
          for(var i = 0; i < cb.length; i++) {
        
            //bind click event handler
            cb[i].addEventListener('click', function(evt) {
              // If checkboxe is checked then save to state
              if (this.checked) {
                cbstate[this.name] = true;
              }
          
             // Else remove from state
              else if (cbstate[this.name]) {
                delete cbstate[this.name];
              }
          
             // Persist state
              sessionStorage.CBState = JSON.stringify(cbstate);
            });
          }
        });
      });