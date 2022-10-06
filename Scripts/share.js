﻿$(function () {

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

        var ShareRideViewModelObj = {
            Date: date,
            Time: time,
            Fare: fare,
            Comment: comment,
            Origin: {
                Address: oAddress,
                City: oCity,
                Country: oCountry
            },
            Destination: {
                Address: dAddress,
                City: dCity,
                Country: dCountry
            }
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Share",
            data: ShareRideViewModelObj,
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