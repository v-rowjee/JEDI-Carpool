$(function () {

    $('#deleteBookingForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $("#deleteBooking").click(() => {
        var rideId = $('#rideId').val()

        $.ajax({
            type: "POST",
            url: "/Booking/Delete",
            data: { Ride: {RideId: rideId}},
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({
                        text: "Booking removed successfully!",
                        actionTextColor: "#CFE2FF",
                    });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({
                        text: "Unable to remove booking!",
                        actionTextColor: "#CFE2FF",
                    });
                }
            }
        })
    })

})