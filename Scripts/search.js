$(function () {

    $('#searchForm').submit((e) => {
        e.preventDefault();
        return false;
    })

    $('#search').click(() => {
        var oAddress = $("#oAddress").val()
        var oCity = $("#oCity").val()
        var dAddress = $("#dAddress").val()
        var dCity = $("#dCity").val()
        var country = $("#country").val()
        var date = $("#date").val()

        var SearchRideViewModelObj = {
            Origin: {
                Address: oAddress,
                City: oCity,
                Country: country
            },
            Destination: {
                Address: dAddress,
                City: dCity,
                Country: country
            },
            Date: date
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Search",
            data: SearchRideViewModelObj,
            dataType: "json",
            success: (response) => {
                if (response.result) {
                    Snackbar.show({
                        text: response.count + "Rides Found",
                        actionTextColor: "#CFE2FF"
                    });
                }
                else {
                    Snackbar.show({
                        text: "No Rides Found",
                        actionTextColor: "#CFE2FF"
                    });
                }
            }
        })


    })

})