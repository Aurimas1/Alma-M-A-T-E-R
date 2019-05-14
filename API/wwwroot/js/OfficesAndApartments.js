$(document).ready(function () {
    //load list of apartments from DB
    loadApartments().then(function() {
        var table = $('#sort').DataTable({

            //After the page is changed
            "fnDrawCallback": function( oSettings ) {

                //add on click
                $("tbody#tBody > tr").on('click', function() {
                    showEditForm($(this));
                });
            }});
        
    });

});

function loadApartments(){
        return $.ajax({
            type: "GET",
            url: '/api/apartment/officeApartments',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            success: function (data) {
                $.each(data, function (key, entry) {
                    var line = $('<tr>');
                    line.append($('<td data-table-header="Nr" id="NrColumn">').text(entry.apartmentId))
                        .append($('</td><td data-table-header="OfficeId" id="OfficeId">').text(entry.officeId))
                        .append($('</td><td data-table-header="RowVersion" id="RowVersion">').text(entry.rowVersion))
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
    tableToLeft();
    hideEditForm();
    loadOffices();
    $("div.creatingForm").css("display","unset");

}

function hideCreateForm(){
    $("div.creatingForm > form")[0].reset();
    $("div.creatingForm").css("display","none");
}

function showEditForm(elem){
    tableToLeft();
    hideCreateForm();
    //remove previous coloring of the row
    $("tbody#tBody > tr").removeClass("selected");
    $("div.editingForm").css("display","unset");
    
    //color the row
    $(elem).addClass("selected");
    
    //show values in fields
    $("input#apartment_id_edit").val($(elem).children("td[data-table-header='Nr']").text());
    $("input#apartment_RowVersion").val($(elem).children("td[data-table-header='RowVersion']").text());
    var selectedOffice = $(elem).children("td[data-table-header='OfficeId']").text();
    $("select#offices_edit").append($("<option value='"+selectedOffice+"' >"+($(elem).children("td[data-table-header='Office']").text())+"</option>"));
    $("select#offices_edit option[value='"+selectedOffice+"']").attr('selected', "true");
    $("input#apartment_name_edit").val($(elem).children("td[data-table-header='Name']").text());
    $("input#apartment_address_edit").val($(elem).children("td[data-table-header='Address']").text());
    $("input#apartment_room_edit").val($(elem).children("td[data-table-header='RoomNumber']").text());
}

function hideEditForm(){
    $("div.editingForm > form")[0].reset();
    $("div.editingForm").css("display","none");
}

function tableToLeft(){
    if (!$("div.tableColumn").hasClass("col-8")) {
        $("div.container.officesAndApartments").css({
            "margin-left": "20px",
            "margin-right": "0px",
            "min-width": "90%"
        });
        $("div.tableColumn").addClass("col-8");
    }
}

function updateApartment(event){
    event.preventDefault();
    var apartment = {}
    $.each($("div.editingForm > form").serializeArray(), function() {
        apartment[this.name] = this.value;
    });
    apartment["Type"] = "OFFICE";
    try {
        $.post({
            url: '/api/apartment/update',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            statusCode: {
                409: function() {
                    alert("Update is not allowed. "
                    + "The apartment information has been changed in the database and so you are currently editing an older version."
                    + " Please refresh the page to see the up to date information.");
                    }
            },
            data: JSON.stringify(apartment),
            success: function () {
                $("div#pageContent").load("../OfficesAndApartments.html");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (xhr.status != 409){
                    alert(xhr.status);
                    alert(thrownError); 
                }
            }
        })
    }
    catch(e){
        console.log(e);
    }
}

function createApartment(){
    event.preventDefault();
    var apartment = {}
    $.each($("div.creatingForm > form").serializeArray(), function() {
        apartment[this.name] = this.value;
    });
    apartment["Type"] = "OFFICE";
    $.ajax({
        type: "POST",
        url: '/api/apartment/create',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(apartment),
        success: function () {
            $("div#pageContent").load("../OfficesAndApartments.html");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function deleteApartment(){
    var apartment = $("input#apartment_id_edit").val();

    if (confirm("Do you really want to delete this apartment? All of the associated reservations will be deleted.")){
        $.ajax({
            type: "POST",
            url: '/api/apartment/delete',
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: apartment,
            success: function () {
                $("div#pageContent").load("../OfficesAndApartments.html");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
}

//for dropdown
function loadOffices(){
    return $.ajax({
        type: "GET",
        url: '/api/office',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            $.each(data, function (key, entry) {
                var o = new Option(entry.city + ", " + entry.country, entry.officeID);
                $("select#offices_create").append($(o));
            });
        },
        error: function () {alert('Internet error'); },
    })
}