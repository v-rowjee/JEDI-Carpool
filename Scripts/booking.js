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


    $('#editBookingForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $("#editBooking").click(() => {
        var rideId = $('#rideId').val()

        $.ajax({
            type: "POST",
            url: "/Booking/Edit",
            data: { Ride: { RideId: rideId } },
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({
                        text: "number of seats booked saved",
                        actionTextColor: "#CFE2FF",
                    });
                    window.location.replace(response.url);
                }
                else {
                    Snackbar.show({
                        text: "Unable to edit booking!",
                        actionTextColor: "#CFE2FF",
                    });
                }
            }
        })
    })

    $("#seat").attr('max') = $('#seatsLeft')

    $('#minus').click(() => {
        var seat = parseInt($("#seat").val())
        var min = $("#seat").attr('min')

        if (seat > min) {
            $("#seat").val(seat - 1)
        }
    })

    $('#plus').click(() => {
        var seat = parseInt($("#seat").val())
        var max = $("#seat").attr('max')

        if (seat < max) {
            $("#seat").val(seat + 1)
        }
    })

    const editModal = document.getElementById('editModal')
    const seatInput = document.getElementById('seat')

    editModal.addEventListener('shown.bs.modal', () => {
        seatInput.focus()
    })



})