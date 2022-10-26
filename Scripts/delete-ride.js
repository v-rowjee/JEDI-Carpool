$('#deleteRideForm').submit((e) => {
    e.preventDefault();
    return false;
})

$('#deleteRide').click(() => {

    var rideId = $('#rideId').val()

    $.ajax({
        type: "POST",
        url: "/Ride/Delete",
        data: { id: rideId },
        dataType: "json",
        success: (response) => {
            if (response.result) {
                Snackbar.show({
                    text: "Ride removed!",
                    actionTextColor: "#CFE2FF",
                    actionText: "Share New",
                    onActionClick: () => window.location.replace(response.url)
                });
            }
            else {
                Snackbar.show({
                    text: "Unable to remove ride",
                    actionTextColor: "#CFE2FF"
                });
            }
        }
    })
})