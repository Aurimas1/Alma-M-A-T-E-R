

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