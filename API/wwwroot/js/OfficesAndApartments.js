$(document).ready(function () {
    //load list of apartments from DB
    loadOffices().then(function() {
        var table = $('#sort').DataTable({

            //After the page is changed
            "fnDrawCallback": function( oSettings ) {

                //add on click
                $("tbody#tBody").on('click', function() {
                    hideForms();
                    showEditForm();
                });
            }});

        //add on click
        $("tbody#tBody").on('click', function() {
            hideForms();
            showEditForm();
        });
    });

});

function loadOffices(){
        return $.ajax({
            type: "GET",
            url: '/api/apartment/officeApartments',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                console.log(data);
                $.each(data, function (key, entry) {
                    var line = $('<tr>');
                    line.append($('<td data-table-header="Nr" id="NrColumn">').text(entry.apartmentId))
                        .append($('</td><td data-table-header="Office">').text(entry.office))
                        .append($('</td><td data-table-header="Name">').text(entry.name))
                        .append($('</td><td data-table-header="Address">').text(entry.address))
                        .append($('</td><td data-table-header="RoomNumber">').text(entry.roomNumber))
                        .append($('</td></tr>'));
                    $('#tBody').append(line);
                });
            },
            error: function () {alert('Internet error'); },
        })
}

function showCreateForm(){
    $("div.container.officesAndApartments").css({
        "margin-left":"20px",
        "margin-right":"0px",
        "min-width":"80%"
    });
    $("div.col.creatingForm").css("display","unset");
}

function showEditForm(){
    $("div.container.officesAndApartments").css({
        "margin-left":"20px",
        "margin-right":"0px",
        "min-width":"80%"
    });
    $("div.col.editingForm").css("display","unset");
}

function hideForms(){
    $("div.col.editingForm").css("display","none");
    $("div.col.creatingForm").css("display","none");
}
