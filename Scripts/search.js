$(function () {

    $('#search').click(() => {
        var oRegion = $("#oRegion").val()
        var oCity = $("#oCity").val()
        var dRegion = $("#dRegion").val()
        var dCity = $("#dCity").val()
        var date = $("#date").val()

        var SearchRideViewModelObj = {
            RegionFrom: oRegion,
            CityFrom: oCity,
            RegionTo: dRegion,
            CityTo: dCity,
            Date: date
        }

        $.ajax({
            type: "POST",
            url: "/Ride/Search",
            data: ShareRideViewModelObj,
            dataType: "json",
            success: (response) => {

            }
        })

    })

})