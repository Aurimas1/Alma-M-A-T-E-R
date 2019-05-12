$(document).ready(function () {
    //load list of employees from DB
    loadEmployees().then(function() {
        var table = $('#sort').DataTable();

        //Subscribe to on click for Role changing
        $("td[data-table-header='Role']").on('click', function() {
            showDropdownAndSaveButton($(this));
        });
        
        $("li#sort_previous, li#sort_next, a.page-link").on('click', function(){
            hideDropdownsAndSaveButton();
        })
    })
    
    
});

function loadEmployees() {
    return $.ajax({
        type: "GET",
        url: '/api/employee',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            $.each(data, function (key, entry) {
                var line = $('<tr>');
                var fullName = entry.name;
                var splitName = fullName.split(" ");
                line.append($('<td data-table-header="Nr" id="NrColumn">').text(entry.employeeID))
                    .append($('</td><td data-table-header="Name">').text(splitName[0]))
                    .append($('</td><td data-table-header="Surname">').text(splitName[1]))
                    .append($('</td><td data-table-header="Email">').text(entry.email))
                    .append($('</td><td data-table-header="Role">').text(entry.role == null?"User":entry.role))
                    .append($('</td></tr>'));
                $('#tBody').append(line);
            });
        },
        error: function () {alert('Internet error'); },
    })
}

function showDropdownAndSaveButton(elem){
    //if the dropdown was not already added
    if($(elem).children().length === 0){
        
        //Add dropdown
        var role = $(elem).text();
          $(elem).text("");
          $(elem).append($(dropdown).clone().val(role));
          
          //if the save button is not already visible, show it
          $("button#savingEmployees").css("display","unset");
            $("p.saving").css("display","block");
      }
}

var dropdown = $("<select>\n" +
    "  <option value=\"User\">User</option>\n" +
    "  <option value=\"Organiser\">Organiser</option>\n" +
    "  <option value=\"Admin\">Admin</option>\n" +
    "</select>");

function saveEmployees(){
    //get rows with dropdown visible
    var employees = $("td[data-table-header='Role'] > select").closest("tr").children("td[data-table-header='Nr']").map
    (function(){
        return { EmployeeId: $.trim($(this).text()), EmployeeRole: ($(this).parent().children("td[data-table-header='Role']").children("select").val())};
    }).get();
    
    //save employees
    $.ajax({
        type: "POST",
        url: '/api/employee/update',
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(employees),
        success: function () {
        hideDropdownsAndSaveButton();
    },
    error: function (xhr, ajaxOptions, thrownError) {
        alert(xhr.status);
        alert(thrownError);
    }
});
    
}

function hideDropdownsAndSaveButton(){
    $("td[data-table-header='Role'] > select").each(function() {
        var role = $(this).val();
        $(this).parent().text(role);
    });
    $("button#savingEmployees").css("display","none");
    $("p.saving").css("display","none");
}