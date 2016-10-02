$(document).ready(function () {
    var j = "";
    j = "toi la ai";
    $('#btnSun').click(function () {
    //    alert($("#display").val());
        //myFunction();
    });
   
  
});
//$(document).ready(function () {
//    var hg = $(".content");
//    $(hg).load(function() {
//        $(".background-image").height = hg.height();
//        $(".background-image").width = hg.width();
//    });
  

   
//    ////$(".background-image").size = hg.size();
//    //alert($(".background-image").height() + " " + hg.height());
//});
$("#ctynew").click(function () {
    alert($("#dscty").text());
});
function myFunction() {
    var serviceUrl = "/Default/NewCty";
    var name = $("#th").val();
    $.get(serviceUrl, { input: name }, function (data) {
        $("#rData").html(data);
    });
}
$(document).ready(function () {
    $("button").click(function () {
        $("#h3").text("CHI NHÁNH " + $("#ipTencty").val().toUpperCase());
        $("#lbtenkho").text("Tên chi nhánh " + $("#ipTencty").val().toLocaleUpperCase());
    });
});

