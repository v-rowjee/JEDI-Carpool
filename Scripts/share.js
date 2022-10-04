$(function () {

    $('#shareForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#share').click(() => {
        var oAddress = $("#oAddress").val()
        var oCity = $("#oCity").val()
        var oCountry = $("#oCountry").val()
        var dAddress = $("#dAddress").val()
        var dCity = $("#dCity").val()
        var dCountry = $("#dCountry").val()
        var date = $("#date").val()
        var time = $("#time").val()
        var fare = $("#fare").val()
        var comment = $("#comment").val()

        var FormData = {
            OAddress: oAddress,
            OCity: oCity,
            OCountry: oCountry,
            DAddress: dAddress,
            DCity: dCity,
            DCountry: dCountry,
            Date: date,
            Time: time,
            Fare: fare,
            Comment: comment
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Share",
            data: formData,
            dataType: "json",
            success: (response) => {
                if (response.result == "Success") {
                    Snackbar.show({ text: "Ride created successfully!" });
                    window.location.replace(response.url);
                }
                else if (response.result == "ToLogin") {
                    Snackbar.show({ text: "Login required" });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({ text: "Unable to share ride" });
                }
            }
        })

    })



})