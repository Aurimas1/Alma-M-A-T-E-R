window.onload = function (){
    getUser();
};

function tabChanged(elem){
    if (!$(elem).parent().hasClass("active")){
        //load the contents
        $("div#pageContent").load("../" + elem.id + ".html");
        //Change active tab
        $("li.nav-item.active").removeClass("active");
        $(elem).parent().addClass("active");
    }
}

function goToTripCreation(elem){
    $("div#pageContent").load("../" + elem.id + ".html");
}

function getUser(){
    $.ajax({
        type: "GET",
        url: "/api/employee/currentUser",
        success: function(data, status){
            setUsername(data.split(";")[0]);
            hideTabs(data.split(";")[1]);
        }});
}

function setUsername(username){
    $("span.username").text(username);
}

function hideTabs(role){
    console.log(role);
    //during development
    role = "Admin";
    if (role !== "User"){
        $("li.nav-item > a#statistics, li.nav-item > a#Trips").parent().css("display","unset");
        $("li.nav-item.active").removeClass("active");
        $("li.nav-item > a#Trips").addClass("active");
        if (role === "Admin"){
            $("li.nav-item > a#administration").parent().css("display","unset");
        }
    }
}