﻿$(function () {

    $("#date").attr("min", moment().format("YYYY-MM-DD"))
    $("#date").val(moment().format("YYYY-MM-DD"))
    $("#time").val(moment().add(2,'hours').format("HH:mm"))

    $('#shareForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#share').click(() => {
        var oRegion = $("#oRegion").val()
        var oCity = $("#oCity").val()
        var dRegion = $("#dRegion").val()
        var dCity = $("#dCity").val()
        var country = $("#country").val()
        var date = $("#date").val()
        var time = $("#time").val()
        var fare = $("#fare").val()
        var comment = $("#comment").val()

        var error = ''

        var ShareRideViewModelObj = {
            Date: date,
            Time: time,
            Fare: fare,
            Comment: comment,
            Origin: {
                Region: oRegion,
                City: oCity,
                Country: country
            },
            Destination: {
                Region: dRegion,
                City: dCity,
                Country: country
            }
        }

        if (!oRegion) error += "Address of Origin required.<br><br>"
        if (!oCity) error += "City of Origin required.<br><br>"
        if (!dRegion) error += "Address of Destination required.<br><br>"
        if (!dCity) error += "City of Destination required.<br><br>"
        if (!country) error += "Country required.<br><br>"
        if (!date) error += "Date required.<br><br>"
        if (!time) error += "Time required.<br><br>"
        if (!fare) error += "Fare required.<br><br>"

        if (date < moment().format("YYYY-MM-DD")) {
            $("#date").addClass("is-invalid")
            error += "Choose a valid date. <br><br>"
        }
        if (time < moment().add(119,'minutes').format("hh:mm")) {
            $("#time").addClass("is-invalid")
            error += "Choose a time at least <b>two hours<b> before ride. <br><br>"
        }

        if (error != '') {
            error = error.slice(0,-8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF",
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Ride/Share",
                data: ShareRideViewModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result == "Success") {
                        Snackbar.show({
                            text: "Ride created successfully!",
                            actionTextColor: "#CFE2FF",
                        });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "NoUser") {
                        Snackbar.show({
                            text: "Login required.",
                            actionText: "SIGN IN",
                            actionTextColor: "#CFE2FF",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else if (response.result == "NoCar") {
                        Snackbar.show({
                            text: "You did not register any car.",
                            actionText: "ADD CAR",
                            actionTextColor: "#CFE2FF",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else {
                        Snackbar.show({
                            text: "An error occured!",
                            actionTextColor: "#CFE2FF"
                        });
                    }
                }
            })
        }


    })
})