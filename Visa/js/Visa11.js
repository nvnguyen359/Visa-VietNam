function add() {
    var url = "/Default/AddVisa11";
    var t1 = $("#hhpax1").val();
   
    var t2 = $("#hhpax2").val();
    var t3 = $("#hhpax3").val();
    var t4 = $("#hhpax4").val();
    var t = $("#tb-visa1-typevisa").find("option:selected").text();
    var stamp = $("#hhpax5").val();
   
    if (t1 == '') t1 = "0";
    if (t2 == '') t2 = "0";
    if (t3 == '') t3 = "0";
    if (t4 == '') t4 = "0";
    if (stamp == '') stamp = "0";

    $.ajax({
        url: url,
        data: {
            t1: parseInt(t1),
            t2: parseInt(t2),
            t3: parseInt(t3),
            t4: parseInt(t4),
            t: t,
            visit: $(".modalH").text(),
            stamp: parseInt(stamp)
        },
        type: "POST",
        success: function (data) {
            //alert(data);
            window.location = "/Default/QuanLyBangGia";
        }
    });
}
function HideShow() {
    $("#tb-visa1-typevisaas").show();
    $(".modalH").text($("#tTOURIST").text());
    $(".btUpdate").hide();
    $(".btDelete").hide();
    $(".btAdd").show();
   ;
}
function UpdateVisa11() {
    //alert($(".modalH").text() + " " + $(".modalH").val());
    var url = "/Default/UpdateVisa11";
    var id = parseInt($(".idpax5").val());
    var stamp = parseInt($("#hhpax5").val());
    $.ajax({
        url: url,
        data: {
            id: id,
            name: $("#tb-visa1-typevisa").find("option:selected").text(),
            pax1: $("#hhpax1").val(),
            pax2: $("#hhpax2").val(),
            pax3: $("#hhpax3").val(),
            pax4: $("#hhpax4").val(),
            visit: $(".modalH").text(),
            stamp: stamp
        },
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            window.location = "/Default/QuanLyBangGia";
        }
    });
}

function datetimeUtc() {
    var url = "/Default/TimeUtc";
    $.ajax({
        url: url,
        data: {
            d: $("#begin").text()
        },
        datatype: "JSON",
        type: "POST",
        success: function(data) {
            $("#btTimeUtc").text(data);
            //setInterval(function () {
            //    var d = new Date();
            //    var n = d.toUTCString();
              

            //}, 1000);
        }

    });
}
function SukienClicj() {
    $('#chij tbody tr').click(function () {
        var h0 = $(this).find(".td01").text();

        var h1 = $(this).find(".td11").text().split('$')[0];
        var h2 = $(this).find(".td21").text().split('$')[0];
        var h3 = $(this).find(".td31").text().split('$')[0];
        var h4 = $(this).find(".td41").text().split('$')[0];
        var h5 = $(this).find(".td51").text().split('$')[0];
        var hid = $(this).find(".tdid").text();
        $("#tb-visa1-typevisaas").hide();
        $(".idpax5").val(hid);
        $("#hhpax1").val(parseInt(h1));
        $("#hhpax2").val(parseInt(h2));
        $("#hhpax3").val(parseInt(h3));
        $("#hhpax4").val(parseInt(h4));
        $("#hhpax5").val(parseInt(h5));
        $("#tb-visa1-typevisa").find("option:selected").text(h0);
        $(".vl5").text(h0 + " Stamping Fee");
        $("#paxPax0").val($(this).find("#td0").text());
        $("#paxPax1").val($(this).find("#td1").text());
        $("#paxPax2").val($(this).find("#td2").text());
        $("#paxPax3").val($(this).find("#td3").text());
        $("#paxPax4").val($(this).find("#td4").text());
       //var tr = h0;
       // tr1 = parseInt(h1);
       // tr2 = parseInt(h2);
       // tr3 = parseInt(h3);
       // tr4 = parseInt(h4);
       // trid = parseInt(hid);
        $(".modalH").text($("#tTOURIST").text());
        $(".btUpdate").show();
        $(".btDelete").show();
        $(".btAdd").hide();
    });
}