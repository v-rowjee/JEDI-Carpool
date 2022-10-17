$(function () {

    var fare = parseFloat($("#fare").html().slice(3))

    $("#totFare").html('Rs ' + parseFloat(parseInt($("#seat").val()) * fare))

    $('#minus').click(() => {
        var seat = parseInt($("#seat").val())
        var min = $("#seat").attr('min')

        if (seat > min) {
            $("#seat").val(seat - 1)
            $("#totFare").html('Rs ' + parseFloat((seat-1) * fare))
        }

    })

    $('#plus').click(() => {
        var seat = parseInt($("#seat").val())
        var max = $("#seat").attr('max')

        if (seat < max) {
            $("#seat").val(seat + 1)
            $("#totFare").html('Rs ' + parseFloat((seat+1) * fare))
        }


    })

    $('#bookRideForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $("#seat-feedback").hide()

    $('#book').click(() => {
        var seat = $("#seat").val()
        var rideId = $("#rideId").val()

        if (!seat) {
            error = "Please provide the number of seat to book."
            Snackbar.show({
                text: error,
                actionTextColor: "#CFE2FF",
            });
        }
        else {

            var BookingModelObj = {
                Seat: seat,
                Ride: {
                    RideId: rideId
                }
            }

            $.ajax({
                type: "POST",
                url: "/Booking/Create",
                data: BookingModelObj,
                dataType: "json",
                success: (response) => {
                    if (response.result == "Success") {
                        Snackbar.show({
                            text: "Carpool Booked!",
                            actionTextColor: "#CFE2FF"
                        });
                        window.location.replace(response.url);
                    }
                    else if(response.result == "NoUser") {
                        Snackbar.show({
                            text: "You are not currently signed in!",
                            actionText: "LOGIN",
                            actionTextColor: "#CFE2FF",
                            onActionClick: () => { window.location.replace(response.url) }
                        });
                    }
                    else if (response.result == "LessSeat") {
                        $("#seat-feedback").show()
                        $("#seat").addClass("is-invalid")
                        Snackbar.show({
                            text: "Seats limited. Not enough seats left.",
                            actionTextColor: "#CFE2FF",
                        });
                    }
                    else if (response.result == "NoSeat") {
                        Snackbar.show({
                            text: "No seats left in this carpool!",
                            actionTextColor: "#CFE2FF",
                        });
                    }
                    else {
                        Snackbar.show({
                            text: "Could not place booking!",
                            actionTextColor: "#CFE2FF"

                        });
                    }
                }
            })
        }
    })





})