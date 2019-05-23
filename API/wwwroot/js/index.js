window.onload = function (){
    getUser().then(function(user) {
        if(!user){
            location = "login.html";
        }
    });
};

function tabChanged(elem){
    if (!$(elem).parent().hasClass("active")){
        //load the contents
        $("div#pageContent").load("../" + elem.id + ".html");
        //Change active tab
        $("li.nav-item.active").removeClass("active");
        
        if (elem.id != "UsersList" && elem.id != "OfficesAndApartments") $(elem).parent().addClass("active");
        else {
            $("li.nav-item > a#administration").parent().addClass("active");
        }
    }
    else{
        $("div#pageContent").load("../" + elem.id + ".html");
    }
}

function goToTripCreation(elem){
    $("div#pageContent").load("../" + elem.id + ".html");
}

function getUser(){
    return $.ajax({
        type: "GET",
        url: "/api/employee/currentUser",
        success: function(data, status){
            if(data != undefined){
                setUsername(data.split(";")[0]);
                hideTabs(data.split(";")[1]);
            }
            else hideTabs("User");
            
        }});
}

function setUsername(username){
    $("span.username").text(username);
}

function hideTabs(role){
    if (role !== "User"){
        $("li.nav-item > a#statistics, li.nav-item > a#Trips").parent().css("display","unset");
        $("li.nav-item.active").removeClass("active");
        //load Trips tab, default for Admin and Organiser
        tabChanged($("li.nav-item > a#Trips").get(0));
        
        if (role === "Admin"){
            $("li.nav-item > a#administration").parent().css("display","unset");
        }
    }
    else {
        tabChanged($("li.nav-item > a#MyTrips").get(0));
    }
}