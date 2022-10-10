$(function () {

    $("#date").attr("min", moment().format("YYYY-MM-DD"))
    $("#date").val(moment().format("YYYY-MM-DD"))
    $("#time").val(moment().format("HH:mm"))

    $('#shareForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#share').click(() => {
        var oAddress = $("#oAddress").val()
        var oCity = $("#oCity").val()
        var dAddress = $("#dAddress").val()
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
                Address: oAddress,
                City: oCity,
                Country: country
            },
            Destination: {
                Address: dAddress,
                City: dCity,
                Country: country
            }
        }

        if (!oAddress) error += "Address of Origin required.<br><br>"
        if (!oCity) error += "City of Origin required.<br><br>"
        if (!dAddress) error += "Address of Destination required.<br><br>"
        if (!dCity) error += "City of Destination required.<br><br>"
        if (!country) error += "Country required.<br><br>"
        if (!date) error += "Date required.<br><br>"
        if (!time) error += "Time required.<br><br>"
        if (!fare) error += "Fare required.<br><br>"

        if (date < moment().format("YYYY-MM-DD")) {
            $("#date").addClass("is-invalid")
            error += "Choose a valid date. <br><br>"
        }

        if (error != '') {
            error = error.slice(0,-8) // to remove the last <br><br>
            Snackbar.show({
                text: error,
                actionTextColor: "#2B80FD",
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
                        Snackbar.show({ text: "Ride created successfully!" });
                        window.location.replace(response.url);
                    }
                    else if (response.result == "NoUser") {
                        Snackbar.show({
                            text: "Login required.",
                            actionText: "SIGN IN",
                            actionTextColor: "#2B80FD",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else if (response.result == "NoCar") {
                        Snackbar.show({
                            text: "You did not register any car.",
                            actionText: "ADD CAR",
                            actionTextColor: "#2B80FD",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else {
                        Snackbar.show({ text: "An error occured!" });
                    }
                }
            })
        }


    })
})