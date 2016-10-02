function getAddDateMonthYear(now1, d, m, y) {
    var now = new Date(now1);
    var day1 = ("0" + (now.getDate() + parseInt(d))).slice(-2);
    var month1 = ("0" + (now.getMonth() + parseInt(m) + 1)).slice(-2);
    var year1 = (now.getFullYear() + parseInt(y));
    var today = year1 + "-" + (month1) + "-" + (day1);
   
    return today;
}

function GetNumberMonth( begin,end) {
    var b;
    try {
        b = new Date(begin);
    } catch (e) {
        b = begin;
    }
    var e;
    try {
        e = new Date(end);
    } catch (e) {
        e = end;
    }
    var sothang = (e.getFullYear() - b.getFullYear()) * 12;
    return (e.getMonth() - b.getMonth() + sothang);
}
function GetNumberDate(begin, end) {
   
    return GetNumberMonth(begin,end)*30;
}

function FullName(a) {
    var v = "fname["+a+"]";
    //alert($(a).val());
    try {
        return $(a).val();
    } catch (e) {
       
    }
    return "";
}
function Nationa(a) {
    return $(a).find("option:selected").text();
}
function Gender(a) {
    return $(a).find("option:selected").text();
}
function Datefbirth(a) {
    return $(a).val();
}
function PassNumber(a) {
    return $(a).val();
}
function ExpDate(a) {
    return $(a).val();
}
function SaveData() {
    //var s = "";
    var s = FullName(a);
    $("#thongbao").text( s);
}

function AddItemSelectOptionText(a) {
    var optionValues = [];
    var  s = "";
    //var s1 = "";
    $(a).each(function() {
        optionValues.push($(this).text());
        s += $(this).text() + ",";
        //s1 += $(this).val() + ",";
    });
    return s.split(",");;
}

function totalSelect() {
    var llocaltion = AddItemSelectOptionText("#slLocaltion");
    var lgiaphong = AddItemSelectOptionText("#slPriceHotel");
    var lhotelroomtype = AddItemSelectOptionText("#hotel_room_type");
    var lgender = AddItemSelectOptionText("#tbn-Gender");
    var lNumberofvisa = AddItemSelectOptionText("#numberVisa");
    var lPurposeofvisit = AddItemSelectOptionText("#Purpose");
    var lsanbay = AddItemSelectOptionText("#Port");
    var vl = $("#values").val();
    var quoctich = $("#tbn-Nationality").val();
    var url = "/Default/DatabaseSelect";
    $.post(url, {
        llocaltion: llocaltion, lgiaphong: lgiaphong, lhotelroomtype: lhotelroomtype,
        lgender: lgender, lNumberofvisa: lNumberofvisa, lPurposeofvisit: lPurposeofvisit
    }, function (data) {
        $("#dsqtcount").val("Danh Sách Quốc Tịch Gồm:" + data);
        window.location = "/Default/Admin";
    });
}
function totalSelectTyofVisa() {
    var llocaltion = AddItemSelectOptionText("#typeOf");
  
}
function AddItemSelectOptionValue(a) {
    var optionValues = [];
    //var s = "";
    var s1 = "";
    $(a).each(function () {
        optionValues.push($(this).text());
        //s += $(this).text() + ",";
        s1 += $(this).val() + ",";
    });
    return s1.split(",");;
}
function AddNationality() {
   

    //$('#result').html(optionValues);

    //var ik=   s.split(","); var ik1=   s1.split(",");
    var vl = $("#values").val();
    var quoctich = $("#tbn-Nationality").val();
    var url = "/Default/QuocTich";
    $.post(url, { quoctich: quoctich, valueff: vl }, function (data) {
        $("#dsqtcount").val("Danh Sách Quốc Tịch Gồm:" + data);
        window.location = "/Default/Admin";
    });
}
function AddSanbay() {
   
    var vl = $("#valuessanbay").val();
    var quoctich = $("#tbn-sanbay").val();
    var url = "/Default/SanBay";
    $.post(url, { quoctich: quoctich, valueff: vl }, function (data) {
        $("#dsqtcount").val("Danh Sách Quốc Tịch Gồm:" + data);
        window.location = "/Default/Admin";
    });
}
function AddSex() {

  
    var quoctich = $("#tbn-sex").val();
    var url = "/Default/GioiTinh";
    $.post(url, { sex: quoctich }, function (data) {
        //$("#dsqtcount").val("Danh Sách Quốc Tịch Gồm:" + data);
        window.location = "/Default/Admin";
    });
}
function DeleteAddNationality() {
    var vl = $("#values").val();
    var quoctich = $("#tbn-Nationality").val();
    var url = "/Default/DeleteQt";
    $.post(url, { quoctich: quoctich }, function (data) {
        window.location = "/Default/Admin";
    });
}

function DeleteSanbay() {
    var vl = $("#valuessanbay").val();
    var quoctich = $("#tbn-sanbay").val();
    var url = "/Default/DeleteSanbay";
    $.post(url, { quoctich: quoctich }, function (data) {
        window.location = "/Default/Admin";
    });
}

function DeleteSex() {
   
    var quoctich = $("#tbn-sex").val();
    var url = "/Default/DeleteSex";
    $.post(url, { sex: quoctich }, function (data) {
        window.location = "/Default/Admin";
    });
}

function AddGiaPhong() {
    var from = $("#from").val();
    var to = $("#to").val();
    var dv = $("#dv").val();
    var url = "/Default/GiaPhong1";
    $.post(url, { from: from,to:to,dv:dv }, function (data) {
        window.location = "/Default/Admin";
    });
}
function DeleteGiaPhong() {
    var from = $("#from").val();
    var to = $("#to").val();
    var dv = $("#dv").val();
    var url = "/Default/DeleteGiaPhong";
    $.post(url, { from: from, to: to, dv: dv }, function (data) {
        window.location = "/Default/Admin";
    });
}
function GiaPhongFist() {
  
}


function AddDiadiem() {
    var from = $("#tbn-diadiem").val();
  
    var url = "/Default/AddDiadiem";
    $.post(url, { dv: from }, function (data) {
        window.location = "/Default/Admin";
    });
}
function DeleteDiadiem() {
    var from = $("#tbn-diadiem").val();

    var url = "/Default/DeleteDiadiem";
    $.post(url, { dv: from }, function (data) {
        window.location = "/Default/Admin";
    });
}
function AddRom() {
    var rom = $("#tbn-rom").val();

    var url = "/Default/AddRom";
    $.post(url, { rom: rom }, function (data) {
        window.location = "/Default/Admin";
    });
}
function DeleteRom() {
    var rom = $("#tbn-rom").val();

    var url = "/Default/DeleteRom";
    $.post(url, { rom: rom }, function (data) {
        window.location = "/Default/Admin";
    });
}
function Addnumberofvisa() {
    var rom = $("#tbn-numberofvisa").val();
    var valueText = $("#valueText").val();
    var valueNumber = $("#valueNumber").val();
    var url = "/Default/AddnumberofVisa";
    $.post(url, { text: valueText,so:valueNumber }, function (data) {
        window.location = "/Default/Admin";
    });
}
function DeleteNumberOfVisa() {
    var rom = $("#tbn-numberofvisa").val();
    var valueText = $("#valueText").val();
    var valueNumber = $("#valueNumber").val();
    var url = "/Default/Deletenumberofvisa";
    $.post(url, { text: valueText, so: valueNumber }, function (data) {
        window.location = "/Default/Admin";
    });
}

function OnChangePrice(a) {
    //var select = $("#tbn-giaphong").find("option:selected").text();
    var h = $(a).val().split(' ');
    $("#from").val(parseInt(h[1].split('$')[0]));
    $("#to").val(parseInt(h[3].split('$')[0]));
    //$("#dv").val(h[3].split(h[3].split('$')[0])[0]);
}

function OnselectNumberOfVisa() {
    var h = $("#tbn-numberofvisa").val().split(' ');
    $("#valueText").val(h[1]);
    $("#valueNumber").val(h[0]);
}