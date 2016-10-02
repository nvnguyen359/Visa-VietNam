function loadGender() {
    var url = "/Default/LoadGender";
    $.ajax({
        url: url,
       
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            //$("#gender2").empty();
           
            for (var j = 0; j < data.length; j++) {

                var opt1 = new Option(data[j].Name);

                $("#gender2").append(opt1);

            }
        }
    });
}
function loadQuoctich() {
    var url = "/Default/Loadquoctich";
    $.ajax({
        url: url,

        datatype: "JSON",
        type: "POST",
        success: function (data) {
            //$("#gender2").empty();

            for (var j = 0; j < data.length; j++) {

                var opt1 = new Option(data[j].Name);

                $("#nationality2").append(opt1);

            }
        }
    });
}
function radioHide(a) {
    $(a).click(function () {
        $(".collapse").collapse('hide');
    });
};

function rdtoggle(a) {
    $(a).click(function () {
        $(".collapse").collapse('toggle');
    });
}
function PassportexpirydateChange(a) {
    var end = new Date($(a).val());
    var begin = new Date();


    if (GetNumberMonth(begin, end) <= 5) {
        $("#myPassportDate").modal();
        $("#sothang").text("Number Motnths: " + GetNumberMonth(begin, end) + "");
    } else {

        $("#myPassportDate").modal('hide');
    }
}

function UpdateThanhViens() {
    //alert(localStorage['idtvEdit']);
   var url = "/Default/UpdateThanhVien";
   var id = localStorage['idtvEdit'];
   var name = $("#fullname2").val();
   var gender = $("#gender2").find("option:selected").text();
   var qt = $("#nationality2").text();
   var sinhnhat = $("#sinhnhat2").val();
   var sopass = $("#sopassport2").val();
   var hethan = $("#ngayhethanpass2").val();
    var form = $("#frormAdd").serialize();
    $.ajax({
        url: url,
        data: { idtv: parseInt(id), name: name, gender: gender, sinhnhat: sinhnhat, sopass: sopass, hethan: hethan },
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            $("#ths").load(location.href + " #ths>*", "");
            $("#nationality2").removeAttr('disabled');
            $("#btsubmit2").show();
            $("#btsubmit3").hide();
            $("#btsubmit4").hide();
        }
    });
}

function DeleteTv() {
    var id = localStorage['idtvEdit'];
    var url = "/Default/EditThanhVien";
    $.ajax({
        url: url,
        data: { idtv: parseInt(id)},
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            $("#ths").load(location.href + " #ths>*", "");
            $("#nationality2").removeAttr('disabled');
            $("#btsubmit2").show();
            $("#btsubmit3").hide();
            $("#btsubmit4").hide();
        }
    });
}
function ShowThanhVien(a) {
    var idtv = parseInt(a);
    var url = "/Default/ShowThanhVien";
    $("#btsubmit2").hide();
    $("#btsubmit3").show();
    $("#btsubmit4").show();
    $("#divShow").animate({ height: 'show' }).animate({ width: 'show' });
    $.ajax({
        url: url,
        data: { idtv: idtv },

        datatype: "JSON",
        type: "POST",
        success: function (data) {
            $("#fullname2").val(data.fullName);
            $("#gender2 option").filter(function () { return $(this).text() == data.gender }).prop('selected', true);         
            $("#nationality2").attr('disabled', 'disabled');
            $("#nationality2 option").filter(function () { return $(this).text() == data.nationality }).prop('selected', true);
            $("#sinhnhat2").val(new Date(parseInt(data.pateOfBirth.substr(6)) + 24 * 60 * 60 * 1000).toISOString().substr(0, 10));
            $("#sopassport2").val(data.passPortNumber);
            $("#ngayhethanpass2").val(new Date(parseInt(data.passportexpireddate.substr(6)) + 24 * 60 * 60 * 1000).toISOString().substr(0, 10));
            $('html, body').animate({ scrollTop: 0 }, 0);
            localStorage['idtvEdit'] = a;
            
        }
    });
}