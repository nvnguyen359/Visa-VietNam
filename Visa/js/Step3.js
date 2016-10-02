function ThanhToan() {
    var socho = $("#soChoNgoi").find("option:selected").text();
    var url = "/Default/GiaTienCho";
    $.ajax({
        url: url,
        data: { socho: socho },
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            $("#giaTienSanbay").val(data);
        }
    });
}