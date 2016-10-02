function purpose1Visa(a) {
    var text = $(a).val();
 
    var url = "/Default/OnChangepurpose1Visa";
    //$("#typeOf").reverse();
   
    $.ajax({
        url: url,
        data: { text: text},
       
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            var s = "";
            $("#typeOf").empty();
            for (var i = 0; i < data.length; i++) {
              
                var opt = new Option(data[i].typeOfVisa);
               
                $("#typeOf").append(opt);
               
            }
            //dksau12htrua();

        }
    });
    Money();
}

function Arrivaldate() {
    var begin = $("#begin").val();
    var url = "/Default/ChangeArrivaldate";
    //Money();
    if (begin != "") {
      
        $.ajax({
            url: url,
            data: { date: begin },
            datatype: "JSON",
            type: "POST",
            success: function (data) {

                if (data != '')
                //    var s = "";
                //for (var i = 0; i < data.; i++) {
                    
                    //}
                    //alert(data.split("*")[1]);
                    dksau12htrua(data.split("*")[1]);
                if (parseInt(data.split("*")[1]) >= 0) {
                   
                    $(".alert").show();
                    Money();
                    if (localStorage.getItem("yes") == null) {
                        $("#modaltbBegin").modal();
                    }
                   
                    }
            }
        });
       
    }
    $('#processingTimes').removeAttr('disabled');
}

function Hienthidk() {
    if (localStorage.getItem("yes") == null) {
        $("#modaltbBegin").modal();
    }
}
function Choose() {
   
    $('#processingTimes').attr("disabled", "disabled");
    if (typeof(Storage) !== "undefined") {
        localStorage.setItem("yes", "1");
    }
    //$.session.set("yes", "1");
    //$("#processingTimes").prop("disabled", true);
    //$("#processingTimes option:selected").text($("#processingTimes option:selected").index(1))
}
function ChooseCancel() {
    //alert($("#minNgaybegin").text())
    $('#processingTimes').removeAttr('disabled');
    if (typeof (Storage) !== "undefined") {
        localStorage.setItem("yes", "0");
    }
}
function showDieukienSupper() {
    Arrivaldate();
}
function dksau12htrua(d) {
  
    var url = "/Default/OnChangeProcessingtime";
    var text = $("#purpose1").val();
    $.ajax({
        url: url,
        data: { text: text },

        datatype: "JSON",
        type: "POST",
        success: function (data) {
          
            if (parseInt(d) >= 0) {
                $("#processingTimes").empty();
                for (var i = data.length-1; i >=1; i--) {

                    var opt = new Option(data[i].name);

                    $("#processingTimes").append(opt);

                }
                //var js = " Please,you choose " + $("#processingTimes").find("option:selected").text();
                //$("#proces1").text(js);
            } else {
                $("#processingTimes").empty();
                for (var j = 0; j < data.length; j++) {

                    var opt1 = new Option(data[j].name);

                    $("#processingTimes").append(opt1);

                }
              
            }
           

        }
    });
   
}

function Money() {
    ExitDateFrom();
    var proTime = $("#processingTimes").find("option:selected").text();
    var indrx = $("#processingTimes option:selected").index();
    if (indrx > 0) {
        $(".alert").show();

    }
   
    //dksau12htrua();
    var url = "/Default/XulyStep1";
    var fomr = $("#myform").serialize();
    localStorage.setItem("sokhag", parseInt($("#numberVisa").find("option:selected").text()));
    //alert($("#processingTimes").find("option:selected").text());
    $.ajax({
        url: url,
        data:fomr,
        datatype: "JSON",
        type: "POST",
        success: function(data) {
            $("#tong-tien").text(data);
        }
    });
}
function ExitDateFrom() {

    var url = "/Default/MaxExitdate";
    var dt = $("#begin").val();
   
    //$("#test").text($("#typeOf").find("option:selected").text());
    $.ajax({
        url: url,
        data: { dt: dt, thag: $("#typeOf").find("option:selected").text() },
        datatype: "JSON",
        type: "POST",
        success: function (data) {
           
            $("#exitday").text("Max " + data);
            $("#end").attr("max", data);
        }
    });
    Arrivaldate();
}